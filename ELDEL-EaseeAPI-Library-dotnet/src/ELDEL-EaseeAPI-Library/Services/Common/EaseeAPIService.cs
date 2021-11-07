using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ELDEL_EaseeAPI_Library.Config;
using ELDEL_EaseeAPI_Library.DTOs;
using ELDEL_EaseeAPI_Library.Interceptors;
using Microsoft.Extensions.Logging;

namespace ELDEL_EaseeAPI_Library.Services
{
    public class EaseeAPIService : IEaseeAPIService
    {
        private readonly ILogger<EaseeAPIService> _logger;
        private readonly IHttpService _httpService;
        private readonly IEaseeAPILibraryConfig _config;
        private readonly IEaseeAPIAuthenticationService _authService;
        private readonly IEaseeChargerService _chargersService;
        private readonly HttpClient _httpClient;

        public EaseeAPIService(
            ILogger<EaseeAPIService> logger,
            IEaseeAPILibraryConfig config,
            IEaseeAPIAuthenticationService authService,
            IHttpService httpService,
            IHttpClientFactory httpClientFactory,
            IEaseeChargerService chargersService
        )
        {
            _logger = logger;
            _config = config;
            _authService = authService;
            _httpService = httpService;
            _httpClient = httpClientFactory.CreateClient(nameof(EaseeAPIAuthenticationHeaderDelegatingHandler));
            _chargersService = chargersService;
        }

        public async Task<IEnumerable<EaseeChargerResponseDTO>> GetChargersAsync()
        {
            return await _chargersService.GetChargersAsync();
        }

        public async Task<EaseeChargerResponseDTO> AddChargerAsync(string serialCode, string pinCode)
        {
            return await _chargersService.AddChargerAsync(serialCode, pinCode);
        }
    }
}
