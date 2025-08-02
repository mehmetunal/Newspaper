# 📄 Newspaper Proje Dokümanı (ASP.NET Core 8 + Clean Architecture + Maggsoft Framework)

## 1. 📊 Proje Bilgileri

- **Proje Adı**: Newspaper
- **Amaç**: Güncel haberleri yayınlayan, mobil uyumlu ve yönetilebilir bir haber portalı geliştirmek.
- **Teknolojiler**:
  - ASP.NET Core 8
  - Entity Framework Core
  - Microsoft SQL Server
  - ASP.NET Core Identity (rol bazlı yetkilendirme)
  - Maggsoft Framework

---

## 2. 🚀 Ana Özellikler

- Manşet haber görsel slider
- Haber detay sayfası (içerik, yorumlar, paylaş butonları)
- Kategori bazlı haber listeleme
- Arama fonksiyonu
- Yorum yapma (giriş zorunluluğu)
- Mobil uyum (responsive)
- SEO dostu URL ve meta etiket desteği

---

## 3. 📑 Sistem Mimarisine Genel Bakış

### 3.1. Katmanlar (.cursorrules'a göre)

- **Libraries/Data**: `Newspaper.Data.Mssql` - Veri katmanı
- **Libraries/Dto**: `Newspaper.Dto.Mssql` - DTO katmanı
- **Libraries**: 
  - `Newspaper.IdentityManager` - Kimlik yönetimi
  - `Newspaper.Mssql` - MSSQL işlemleri
  - `Newspaper.Mssql.Services` - Servis katmanı
- **Presentation/Api**: `Newspaper.Api` - API katmanı
- **Presentation/Web**: 
  - `Newspaper.Web` - Web projesi
  - `Newspaper.Web.Framework` - Web framework

### 3.2. Veritabanı Tabloları

- Users (Identity)
- Roles (Identity)
- Haberler (Id, Baslik, Ozet, Icerik, ResimUrl, KategoriId, YazarId, YayınTarihi)
- Kategoriler (Id, Ad)
- Yorumlar (Id, HaberId, KullaniciId, Icerik, Tarih)
- Abonelikler (Id, Email, OnayDurumu)

---

## 4. 🔐 Rol Bazlı Yetkilendirme

### 4.1. Roller

- **Admin**: Tam yetki, roller ve kullanıcıları yönetebilir
- **Editor**: Haber ekleyebilir/güncelleyebilir
- **User**: Haberleri okuyabilir, yorum yapabilir
- **Guest**: Sadece haber görüntüleyebilir

### 4.2. Seed Data

```csharp
// Admin kullanıcısı: admin@gmail.com / Super123!
```

---

## 5. 🚪 Yönetim Paneli Özellikleri

- Kullanıcı/rol yönetimi (Admin)
- Haber CRUD işlemleri (Editor/Admin)
- Kategori ekle/düzenle
- Yorum moderasyonu
- Dashboard (istatistikler: toplam haber, yorum, yazar sayısı vb.)

---

## 6. 🛠️ Teknik Detaylar

- **Kimlik Doğrulama**: Microsoft.AspNetCore.Identity.EntityFrameworkCore
- **Migration**: FluentMigrator
- **Validasyon**: FluentValidation
- **View Engine**: Razor
- **Stil**: Bootstrap 5 (sadece)

---

## 7. 📦 Maggsoft Framework Paketleri

### Temel Paketler:
- **Maggsoft.Core** (v2.0.39) - Temel altyapı
- **Maggsoft.Data** (v2.0.20) - Veri katmanı
- **Maggsoft.Framework** (v2.3.6) - Framework altyapısı
- **Maggsoft.Data.Mssql** (v2.0.10) - MSSQL entegrasyonu
- **Maggsoft.Mssql** (v2.0.12) - MSSQL repository
- **Maggsoft.Cache** (v2.0.22) - Önbellekleme
- **Maggsoft.Cache.MemoryCache** (v2.0.7) - Memory cache
- **Maggsoft.Services** (v2.0.8) - Servis katmanı
- **Maggsoft.Aspect.Core** (v1.0.15) - AOP
- **Maggsoft.Mssql.Services** (v2.0.17) - MSSQL servisleri
- **Maggsoft.Endpoints** (v2.0.7) - API endpoint'leri

### Diğer Paketler:
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** - Kimlik doğrulama ve yetkilendirme
- **Maggsoft.Dto.Mssql** (v1.0.2) - MSSQL DTO'ları
- **Maggsoft.Logging** (v1.0.6) - Loglama
- **Maggsoft.EventBus** (v1.0.19) - Event bus

---

## 8. ⌛ Geliştirme Akışı (Sprint Planı)

| Aşama                   | Süre    |
| ----------------------- | ------- |
| Proje planlama          | 2 gün   |
| Kimlik sistemi kurulumu | 2-3 gün |
| Admin paneli iskeleti   | 3-4 gün |
| Haber/Kategori yapısı   | 4-5 gün |
| Yorum sistemi           | 2 gün   |
| Test & Yayın            | 2-3 gün |

---

## 9. 📤 Yayın ve Deployment

- Publish to folder (IIS için)
- Migration apply (FluentMigrator)
- AppSettings.Production.json ile bağlantı ayarları
- Dockerfile (MSSQL container)

---

## 10. 🔒 Güvenlik Notları

- CSRF koruması (MVC ile entegre gelir)
- XSS için HTML içerik doğrulama
- Role bazlı güvenli routing
- Şifre karma algoritması (ASP.NET Identity default: PBKDF2)

---

## 11. 📄 Ekstra Özellikler (v2)

- Abonelik sistemi (e-posta bülteni)
- Web push bildirimleri
- Sosyal medya girişleri (Google, Facebook)
- Çok dil desteği (i18n)
- Haber etiketleme ve filtreleme sistemi
- RESTful API desteği (mobil uygulamalar için JSON endpoint)

- Serilog ile loglama ve hata takibi
- Sitemap.xml ve robots.txt otomatik oluşturma

---

## 12. 🎯 Kod Yazım Kuralları

### Naming Conventions
- **PascalCase**: Class, Method, Property
- **camelCase**: Local variables, parameters
- **UPPER_CASE**: Constants
- **PascalCase**: File names (C#)
- **kebab-case**: File names (HTML, CSS, JS)

### Repository Pattern
```csharp
public interface I[Entity]Repository
{
    // READ
    Task<IEnumerable<[Entity]>> GetAllAsync();
    Task<[Entity]?> GetByIdAsync(Guid id);
    
    // WRITE
    Task<[Entity]> AddAsync([Entity] entity);
    Task<[Entity]> UpdateAsync([Entity] entity);
    Task DeleteAsync(Guid id);
    Task<int> SaveChangesAsync();
}
```

### Responslar 
  -- Maggsoft.Core.Base.Result kullanılmalı


**Sayfalama işlemleri Maggsoft.Core.Model.Pagination.PagedList<T> bunu kullanılmalı 
örnek  var pagedList = await query.ToPagedListAsync(request.PageNumber - 1, request.PageSize);
apiden dönen değeri webden veya adminden karşılamak için
```csharp
/// <summary>
/// Web tarafında API'den gelen PagedList'i karşılayacak wrapper sınıfı
/// </summary>
public class PagedListWrapper<T>
{
    /// <summary>
    /// Veri listesi
    /// </summary>
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Sayfa numarası (0-based)
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Toplam kayıt sayısı
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Önceki sayfa var mı?
    /// </summary>
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Sonraki sayfa var mı?
    /// </summary>
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// 1-based sayfa numarası
    /// </summary>
    public int CurrentPage => PageNumber + 1;

    /// <summary>
    /// Boş PagedList oluşturur
    /// </summary>
    public static PagedListWrapper<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return new PagedListWrapper<T>
        {
            Data = new List<T>(),
            PageNumber = pageNumber - 1, // 1-based'den 0-based'e çevir
            PageSize = pageSize,
            TotalCount = 0,
            TotalPages = 0,
            HasPreviousPage = false,
            HasNextPage = false
        };
    }
}
```

Webden veya Adminden Apiye istek IMaggsoftHttpClient ile irtibat kurulmalı
```csharp
public class CustomHttpClient : IMaggsoftHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomHttpClient> _logger;

    public CustomHttpClient(HttpClient httpClient, IConfiguration configuration, ILogger<CustomHttpClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;

        // Configure base address
        var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5125/";
        _httpClient.BaseAddress = new Uri(apiBaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task PingAsync()
    {
        await _httpClient.GetStringAsync("/");
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(string url) where TResult : class
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<object>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Data == null)
                return new List<TResult>();

            var jsonString = result.Data.ToString();
            if (string.IsNullOrEmpty(jsonString))
                return new List<TResult>();

            var listResult = JsonSerializer.Deserialize<List<TResult>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return listResult ?? new List<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync request: {Url}", url);
            return new List<TResult>();
        }
    }

    public async Task<TResult> GetAsync<TResult>(string url) where TResult : class
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            // Result<T> için özel deserializasyon
            if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultData = JsonSerializer.Deserialize<TResult>(responseBody, jsonOptions);
                return resultData;
            }

            // PagedListWrapper için özel deserializasyon
            if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(MinimalAirbnb.Web.Models.PagedListWrapper<>))
            {
                var resultData = JsonSerializer.Deserialize<TResult>(responseBody, jsonOptions);
                return resultData;
            }

            var result = JsonSerializer.Deserialize<TResult>(responseBody, jsonOptions);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAsync request: {Url}", url);
            return null;
        }
    }

    public async Task<HttpResponseMessage> GetClientAsync(string url, Dictionary<string, string>? qParametre = null)
    {
        if (qParametre != null && qParametre.Any())
        {
            var queryString = string.Join("&", qParametre.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            url = $"{url}?{queryString}";
        }

        return await _httpClient.GetAsync(url);
    }

    public async Task<Result<TResult>> PostAsJsonAsync<TResult>(string url, object body) where TResult : class
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PostAsJsonAsync request: {Url}", url);
            return new Result<TResult>();
        }
    }

    public async Task<Result<TResult>> PostAsync<TResult>(string url, object body) where TResult : class
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PostAsync request: {Url}", url);
            return new Result<TResult>();
        }
    }

    public async Task<TResult> SendAsync<TResult>(string url, object body, HttpMethod method) where TResult : class
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await _httpClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResult>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendAsync request: {Url}", url);
            return null;
        }
    }

    public async Task SendAsync(string url, object body, HttpMethod method)
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, url) { Content = content };
            await _httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendAsync request: {Url}", url);
            throw ex;
        }
    }

    public async Task<Result<TResult>> PostHttpContentAsync<TResult>(string url, HttpContent content) where TResult : class
    {
        try
        {
            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PostHttpContentAsync request: {Url}", url);
            return Result<TResult>.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<Result<TResult>> PutAsJsonAsync<TResult>(string url, object body) where TResult : class
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PutAsJsonAsync request: {Url}", url);
            return Result<TResult>.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<Result<TResult>> PutAsync<TResult>(string url, object body) where TResult : class
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PutAsync request: {Url}", url);
            return Result<TResult>.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<Result<TResult>> PutHttpContentAsync<TResult>(string url, HttpContent content) where TResult : class
    {
        try
        {
            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<TResult>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result<TResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PutHttpContentAsync request: {Url}", url);
            return Result<TResult>.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(string url, object id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{url}/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new Result();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteAsync request: {Url}", url);
            return new Result();
        }
    }
}
```
---

## 13. 🎨 UI/UX Rules

### Bootstrap 5
- **Components**: Navbar, Cards, Buttons, Modals, Forms, Tables
- **Utilities**: Spacing, Colors, Typography
- **Grid System**: Responsive layout
- **No Custom CSS**: Mümkün olduğunca utility class'ları kullan

### Accessibility
- **Semantic HTML**: Doğru HTML element'leri
- **Alt Text**: Tüm resimler için
- **Form Labels**: Tüm form kontrol'leri için
- **ARIA**: Gerekli yerlerde ARIA attribute'ları

---

## 14. 🚨 Anti-Patterns (Yapma!)

### ❌ Yapılmayacaklar
- Business logic'i controller'lara yazma
- Repository'lerde SaveChanges çağırma
- Hard-coded connection string'ler
- Exception'ları yakalamadan bırakma
- Null reference exception'lara izin verme
- Circular dependency oluşturma

### ✅ Yapılacaklar
- Clean Architecture'a uy
- Proper error handling
- Comprehensive validation
- Documentation ekle
- Security best practices uygula

---

Bu doküman geliştirme süreci boyunca güncellenebilir. Tüm proje modülleri ASP.NET Core 8 yapısına uygun parçalar halinde ayrılmalı ve SOLID prensipleri gözetilerek geliştirilmelidir.

## Clean Architecture Kuralı

- Web ve Admin Panel projeleri, Clean Architecture gereği sadece API ile haberleşir. Data ve Service katmanlarına doğrudan erişim yasaktır. Tüm işlemler IMaggsoftHttpClient veya benzeri bir HTTP client ile API endpoint'leri üzerinden yapılır.

## Web ve Admin Panel Projelerinde IMaggsoftHttpClient Kullanımı

- **CustomHttpClient**: Her iki projede de `Maggsoft.Framework.HttpClientApi.IMaggsoftHttpClient` interface'ini implement eden `CustomHttpClient` sınıfı kullanılır.
- **Program.cs Konfigürasyonu**: 
  ```csharp
  builder.Services.AddHttpClient();
  builder.Services.AddScoped<CustomHttpClient>();
  ```
- **ApiBaseUrl**: `appsettings.json` dosyasında API'nin base URL'i tanımlanır.
- **PagedListWrapper<T>**: API'den gelen `PagedList<T>` verilerini karşılamak için wrapper model kullanılır.
- **Cookie Authentication**: AddIdentity kullanılmaz, sadece Cookie Authentication ve Authorization kullanılır.

## Sayfalama İşlemleri

### Database Sorgusunda (API Katmanında)
```csharp
var pagedList = await query.ToPagedListAsync(request.PageNumber - 1, request.PageSize, new List<Filter>());
```

### Web ve Admin Projelerinde API'den Veri Çekerken
```csharp
await _httpClient.GetAsync<PagedListWrapper<PropertyDto>>("api/endpoint")
```

### PagedListWrapper<T> Sınıfı
```csharp
/// <summary>
/// Web tarafında API'den gelen PagedList'i karşılayacak wrapper sınıfı
/// </summary>
public class PagedListWrapper<T>
{
    /// <summary>
    /// Veri listesi
    /// </summary>
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Sayfa numarası (0-based)
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Toplam kayıt sayısı
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Önceki sayfa var mı?
    /// </summary>
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Sonraki sayfa var mı?
    /// </summary>
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// 1-based sayfa numarası
    /// </summary>
    public int CurrentPage => PageNumber + 1;

    /// <summary>
    /// Boş PagedList oluşturur
    /// </summary>
    public static PagedListWrapper<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return new PagedListWrapper<T>
        {
            Data = new List<T>(),
            PageNumber = pageNumber - 1, // 1-based'den 0-based'e çevir
            PageSize = pageSize,
            TotalCount = 0,
            TotalPages = 0,
            HasPreviousPage = false,
            HasNextPage = false
        };
    }
}
```


## docker run
    -- docker ps -a
    -- docker stop <container_adı_veya_id>
    -- docker rm <container_adı_veya_id>
    -- docker compose up -d