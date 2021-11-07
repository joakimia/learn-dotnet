using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace ELDEL_EaseeAPI_Library.Services
{
    public class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _logger;

        public HttpService(ILogger<HttpService> logger)
        {
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(HttpClient httpClient, string requestUri, string token, Dictionary<string, string> queryParameters = null)
        {
            using var client = httpClient;
            try
            {
                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
                var responseMessage = await client.GetAsync(requestUri);
                var jsonResult = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(jsonResult);
                }
                else
                {
                    throw new Exception($"Status code: {responseMessage.StatusCode}, body: {jsonResult}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetAsync<{typeof(T).Name}>.");
                throw;
            }
        }

        public async Task<T> PostAsync<T>(HttpClient httpClient, Uri uri, string body, string token, Dictionary<string, string> queryParameters = null)
        {
            if (queryParameters != null)
            {
                uri = AppendQueryParameters(uri, queryParameters);
            }

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            using (requestMessage.Content = new StringContent(body))
            using (var client = httpClient)
            {
                try
                {
                    requestMessage.Headers.Add(HeaderNames.Authorization, token);
                    var responseMessage = await client.SendAsync(requestMessage);
                    var jsonResult = await responseMessage.Content.ReadAsStringAsync();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<T>(jsonResult);
                    }
                    else
                    {
                        throw new Exception($"Status code: {responseMessage.StatusCode}, reason {responseMessage.ReasonPhrase}, body: {jsonResult}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in PostAsync<{typeof(T).Name}>.");
                    throw;
                }
            }
        }

        public async Task<T> PutAsync<T>(HttpClient httpClient, Uri uri, string body, string token, Dictionary<string, string> queryParameters = null)
        {
            if (queryParameters != null)
            {
                uri = AppendQueryParameters(uri, queryParameters);
            }

            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            using (requestMessage.Content = new StringContent(body))
            using (var client = httpClient)
            {
                try
                {
                    requestMessage.Headers.Add(HeaderNames.Authorization, token);
                    var responseMessage = await client.SendAsync(requestMessage);
                    var jsonResult = await responseMessage.Content.ReadAsStringAsync();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<T>(jsonResult);
                    }
                    else
                    {
                        throw new Exception($"Status code: {responseMessage.StatusCode}, reason {responseMessage.ReasonPhrase}, body: {jsonResult}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in PutAsync<{typeof(T).Name}>.");
                    throw;
                }
            }
        }

        public async Task<T> DeleteAsync<T>(HttpClient httpClient, Uri uri, string token, Dictionary<string, string> queryParameters = null)
        {
            if (queryParameters != null)
            {
                uri = AppendQueryParameters(uri, queryParameters);
            }

            using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            using var client = httpClient;
            try
            {
                requestMessage.Headers.Add(HeaderNames.Authorization, token);
                var responseMessage = await client.SendAsync(requestMessage);
                var jsonResult = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(jsonResult);
                }
                else
                {
                    throw new Exception($"Status code: {responseMessage.StatusCode}, reason {responseMessage.ReasonPhrase}, body: {jsonResult}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteAsync<{typeof(T).Name}>.");
                throw;
            }
        }

        private static Uri AppendQueryParameters(Uri uri, Dictionary<string, string> queryParameters)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var pair in queryParameters)
            {
                query.Add(pair.Key, pair.Value);
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
}
