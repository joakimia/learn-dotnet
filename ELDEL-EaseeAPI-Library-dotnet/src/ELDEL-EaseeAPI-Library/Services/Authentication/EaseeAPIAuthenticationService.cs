using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ELDEL_EaseeAPI_Library.Config;
using ELDEL_EaseeAPI_Library.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace ELDEL_EaseeAPI_Library.Services
{
    public class EaseeAPIAuthenticationService : IEaseeAPIAuthenticationService
    {
        private readonly ILogger<EaseeAPIAuthenticationService> _logger;
        private readonly IEaseeAPILibraryConfig _config;

        private static string _accessToken;
        private static string _refreshToken;

        public EaseeAPIAuthenticationService(
            ILogger<EaseeAPIAuthenticationService> logger,
            IEaseeAPILibraryConfig config
        )
        {
            _logger = logger;
            _config = config;
        }

        public string AccessToken
        {
            get => _accessToken;
        }

        public string RefreshToken
        {
            get => _refreshToken;
        }

        // Gets and sets the access token and refresh token from Easee API
        // Currently, this function logs us in with joakim.amundsen@eldel.net credential, where username is his phone number.
        public async Task AuthorizeAsync()
        {
            _logger.LogInformation("Authorizing with Easee API, fetching new access and refresh tokens.");

            var uri = new Uri($"{_config.BaseUri}/{_config.LoginEndpoint}");
            var body = new EaseeAPITokenRequestDTO { GrantType = _config.GrantType, Username = _config.Username, Password = _config.Password };
            using (var httpClient = new HttpClient())
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri))
            using (requestMessage.Content = new StringContent(JsonConvert.SerializeObject(body)))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var responseMessage = await httpClient.SendAsync(requestMessage);
                    var jsonResult = await responseMessage.Content.ReadAsStringAsync();

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Authorizing with Easee API succeeded, login completed.");
                        await SetTokensByResponseAsync(responseMessage);
                    }
                    else
                    {
                        throw new Exception($"Status code: {responseMessage.StatusCode}, reason {responseMessage.ReasonPhrase}, body: {jsonResult}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(AuthorizeAsync)} error, login failed.");
                    throw;
                }
            }
        }

        // Gets a new set of acess and refersh token.
        // If even the refresh token are invalid, we do a clean authenticate.
        public async Task ReAuthorizeAsync()
        {
            _logger.LogInformation("Re-Authorizing with Easee API, access token expired.");

            var uri = new Uri(_config.RefreshTokenEndpoint);
            var body = new RefreshTokenRequestDTO { AccessToken = _accessToken, RefreshToken = _refreshToken };

            using (var httpClient = new HttpClient())
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri))
            using (requestMessage.Content = new StringContent(JsonConvert.SerializeObject(body)))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var responseMessage = await httpClient.SendAsync(requestMessage);
                    var jsonResult = await responseMessage.Content.ReadAsStringAsync();

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Authorizing with Easee API succeeded.");
                        await SetTokensByResponseAsync(responseMessage);
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogInformation("Re-Authorizing with Easee API, access token expired.");
                        await AuthorizeAsync();
                    }
                    else
                    {
                        throw new Exception($"Status code: {responseMessage.StatusCode}, reason {responseMessage.ReasonPhrase}, body: {jsonResult}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(AuthorizeAsync)} error, refresh token failed.");
                    throw;
                }
            }
        }

        private static async Task SetTokensByResponseAsync(HttpResponseMessage response)
        {
            var jsonResult = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<EaseeAPITokenResponseDTO>(jsonResult);

            _accessToken = authResponse.AccessToken;
            _refreshToken = authResponse.RefreshToken;
        }
    }
}
