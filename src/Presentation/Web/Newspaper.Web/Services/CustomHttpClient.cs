using System.Text;
using System.Text.Json;
using Maggsoft.Core.Base;
using Maggsoft.Core.Model;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.Web.Models;

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
}
