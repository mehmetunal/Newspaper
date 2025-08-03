using System.Text;
using System.Text.Json;
using Maggsoft.Core.Base;
using Maggsoft.Core.Model;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Newspaper.Web.Services
{

    /// <summary>
    /// Custom HTTP Client implementation for MinimalAirbnb Admin project
    /// </summary>
    public class CustomHttpClient : IMaggsoftHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomHttpClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomHttpClient(HttpClient httpClient, IConfiguration configuration, ILogger<CustomHttpClient> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;

        // Configure base address
        var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5125/";
        _httpClient.BaseAddress = new Uri(apiBaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// Token'ı claims'den alır ve Bearer header'ı ekler
    /// </summary>
    private void AddAuthorizationHeader()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                // Mevcut Authorization header'ını temizle
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                // Yeni Bearer token header'ı ekle
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token header eklenirken hata oluştu");
        }
    }

    public async Task PingAsync()
    {
        AddAuthorizationHeader();
        await _httpClient.GetStringAsync("/");
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(string url) where TResult : class
    {
        try
        {
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
        AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
            AddAuthorizationHeader();
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
}
