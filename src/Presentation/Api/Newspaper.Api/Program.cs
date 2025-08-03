
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Newspaper.Data.Mssql;
using Newspaper.Mssql;
using Newspaper.Mssql.Services;
using Newspaper.IdentityManager;
using Newspaper.Api.Services;
using Maggsoft.Framework.Middleware;
using System.Reflection;
using Maggsoft.Core.IO;
using Microsoft.Extensions.FileProviders;
using Maggsoft.Mssql.Repository;
using Maggsoft.Mssql.Extensions;
using Maggsoft.Services.Extensions;
using Maggsoft.Core.IoC;
using Maggsoft.Framework.Extensions;
using Maggsoft.Framework.Middleware.ApiResponseMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Serilog konfigürasyonu - MSSQL Database'e log
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Newspaper API",
        Version = "v1",
        Description = "Newspaper yönetim sistemi API'si"
    });
});

// Database konfigürasyonu
builder.Services
        .AddMssqlConfig<NewspaperDbContext>(builder.Configuration, o =>
        {
            o.UseCompatibilityLevel(120);
        })
        .AddFluentMigratorConfig(builder.Configuration);

builder.Services.AddAutoMapperConfig(p => p.AddProfile<AutoMapperProfile>(), typeof(Program));

// Identity konfigürasyonu
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<NewspaperDbContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<Role>>();

// Custom Identity sınıfları
builder.Services.AddScoped<AuditableSignInManager>();
builder.Services.AddScoped<CustomClaimsPrincipalFactory>();
builder.Services.AddScoped<UserConfirmation>();

// Database servisleri
builder.Services.AddScoped<IMigrationService, MigrationService>();
builder.Services.AddScoped<ISeedDataService, SeedDataService>();

// Maggsoft Repository servisleri
builder.Services.AddScoped(typeof(IMssqlRepository<>), typeof(Repository<>));

// Maggsoft IoC ile tüm servisleri register et
builder.Services.RegisterAll<IService>();

// Maggsoft File Management Services
builder.Services.AddSingleton<IFileManagerProviderBase>(
    new FileManagerProviderBase(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

builder.Services.AddTransient<IFilesManager, FilesManager>();
builder.Services.AddTransient<IMaggsoftFileProvider, MaggsoftFileProvider>();
builder.Services.AddHttpContextAccessor();

// Maggsoft Framework Middleware'leri
builder.Services.AddExceptionHandler<ExceptionMiddleware>();
builder.Services.AddProblemDetails();

// CORS konfigürasyonu
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// FluentValidation konfigürasyonu

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Memory Cache
builder.Services.AddMemoryCache();

// Maggsoft Global Response Middleware
builder.Services.AddGlobalResponseMiddlewareWithOptions(p => p.IgnoreAcceptHeader = ["image/", "txt"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Newspaper API V1");
        c.RoutePrefix = string.Empty; // Swagger UI'ı root'ta göster
    });
}

// Maggsoft Framework Middleware'leri
app.UseMiddleware<ApiResponseMiddleware>();
app.UseMiddleware<IPFilterMiddleware>();

app.UseHttpsRedirection();

// CORS middleware
app.UseCors("AllowAll");

// Authentication ve Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Minimal API Endpoints - Database Operations
app.MapPost("/api/database/migrate", async (IMigrationService migrationService) =>
{
    try
    {
        await migrationService.MigrateAsync();
        return Results.Ok(new { success = true, message = "Migration başarıyla tamamlandı" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Migration hatası: {ex.Message}", statusCode: 500);
    }
});

// Minimal API Endpoints - Seed Data
app.MapPost("/api/database/seed", async (ISeedDataService seedDataService) =>
{
    try
    {
        await seedDataService.SeedAllDataAsync();
        return Results.Ok(new { success = true, message = "Seed data başarıyla oluşturuldu" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Seed data hatası: {ex.Message}", statusCode: 500);
    }
});

// Minimal API Endpoints - Database tables endpoint
app.MapGet("/api/tables", async (NewspaperDbContext context) =>
{
    try
    {
        var tables = new List<string>();

        // SQL Server'da tabloları listele
        var sql = @"
            SELECT TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE = 'BASE TABLE'
            AND TABLE_CATALOG = 'NewspaperDb'
            ORDER BY TABLE_NAME";

        var tableNames = await context.Database.SqlQueryRaw<string>(sql).ToListAsync();

        return Results.Ok(new {
            message = $"Veritabanında {tableNames.Count} tablo bulundu",
            tables = tableNames,
            expectedCount = 14
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = $"Tablo listesi alınırken hata: {ex.Message}" });
    }
})
.WithName("GetTables");

// Minimal API Endpoints - Database columns endpoint
app.MapGet("/api/tables/{tableName}/columns", async (NewspaperDbContext context, string tableName) =>
{
    try
    {
        // SQL Server'da tablo kolonlarını listele
        var sql = @"
            SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = @tableName
            AND TABLE_CATALOG = 'NewspaperDb'
            ORDER BY ORDINAL_POSITION";

        var columns = await context.Database.SqlQueryRaw<dynamic>(sql, new[] { new Microsoft.Data.SqlClient.SqlParameter("@tableName", tableName) }).ToListAsync();

        return Results.Ok(new {
            message = $"{tableName} tablosunda {columns.Count} kolon bulundu",
            tableName = tableName,
            columns = columns
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = $"Kolon listesi alınırken hata: {ex.Message}" });
    }
})
.WithName("GetTableColumns");

// Minimal API Endpoints - Database columns endpoint (simple)
app.MapGet("/api/columns/{tableName}", async (NewspaperDbContext context, string tableName) =>
{
    try
    {
        // SQL Server'da tablo kolonlarını listele
        var sql = $@"
            SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{tableName}'
            AND TABLE_CATALOG = 'NewspaperDb'
            ORDER BY ORDINAL_POSITION";

        var columns = await context.Database.SqlQueryRaw<string>(sql).ToListAsync();

        return Results.Ok(new {
            message = $"{tableName} tablosunda {columns.Count} kolon bulundu",
            tableName = tableName,
            columns = columns
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = $"Kolon listesi alınırken hata: {ex.Message}" });
    }
})
.WithName("GetColumns");

// Minimal API Endpoints - Drop all tables endpoint
app.MapPost("/api/drop-all-tables", async (NewspaperDbContext context) =>
{
    try
    {
        // Tüm tabloları sil
        var sql = @"
            DECLARE @sql NVARCHAR(MAX) = N'';

            SELECT @sql += 'ALTER TABLE [' + sch.name + '].[' + t.name + '] DROP CONSTRAINT [' + fk.name + '];' + CHAR(13)
            FROM sys.foreign_keys fk
                    JOIN sys.tables t ON fk.parent_object_id = t.object_id
                    JOIN sys.schemas sch ON t.schema_id = sch.schema_id;

            EXEC sp_executesql @sql;


            DECLARE @sql2 NVARCHAR(MAX) = N'';

            SELECT @sql2 += 'DROP TABLE [' + s.name + '].[' + t.name + '];' + CHAR(13)
            FROM sys.tables t
                    JOIN sys.schemas s ON t.schema_id = s.schema_id;

            EXEC sp_executesql @sql2;";

        await context.Database.ExecuteSqlRawAsync(sql);

        return Results.Ok(new { message = "Tüm tablolar başarıyla silindi" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = $"Tablolar silinirken hata: {ex.Message}" });
    }
})
.WithName("DropAllTables");

app.Run();
