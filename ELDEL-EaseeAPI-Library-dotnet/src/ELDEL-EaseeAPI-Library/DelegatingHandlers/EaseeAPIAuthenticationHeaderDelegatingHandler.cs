using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ELDEL_EaseeAPI_Library.Services;
using Microsoft.Extensions.Logging;

namespace ELDEL_EaseeAPI_Library.Interceptors
{
    public class EaseeAPIAuthenticationHeaderDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<EaseeAPIAuthenticationHeaderDelegatingHandler> _logger;
        private readonly IEaseeAPIAuthenticationService _authService;

        public EaseeAPIAuthenticationHeaderDelegatingHandler(ILogger<EaseeAPIAuthenticationHeaderDelegatingHandler> logger, IEaseeAPIAuthenticationService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(_authService.AccessToken))
                {
                    // Authorize with Easee API and allocate AccessToken and RefreshToken into memory.
                    await _authService.AuthorizeAsync();
                }

                // Send original request
                var response = await SendRequestWithAuthBearerHeaderAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // If original request returned 401, we assume it is a problem with the access/refresh JWT tokens.
                    // Try to Re-Authorize with Easee API (acesss + refresh token is expired),
                    // and allocate a new set of AccessToken and RefreshToken into memory
                    await _authService.ReAuthorizeAsync();
                    response = await SendRequestWithAuthBearerHeaderAsync(request, cancellationToken);
                }
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SendAsync)} error. Attempted to send http reqeuest: {request.RequestUri}.");
                throw;
            }
        }

        private async Task<HttpResponseMessage> SendRequestWithAuthBearerHeaderAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
