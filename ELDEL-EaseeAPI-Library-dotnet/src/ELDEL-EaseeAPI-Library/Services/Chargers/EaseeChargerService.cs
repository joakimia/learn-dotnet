using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ELDEL_EaseeAPI_Library.Config;
using ELDEL_EaseeAPI_Library.DTOs;
using ELDEL_EaseeAPI_Library.Interceptors;
using Microsoft.Extensions.Logging;

namespace ELDEL_EaseeAPI_Library.Services
{
    public class EaseeChargersService : IEaseeChargerService
    {
        private readonly ILogger<EaseeChargersService> _logger;
        private readonly IEaseeAPILibraryConfig _config;
        private readonly IEaseeAPIAuthenticationService _authService;
        private readonly IHttpService _httpService;
        private readonly HttpClient _httpClient;

        public EaseeChargersService(
            ILogger<EaseeChargersService> logger,
            IEaseeAPILibraryConfig config,
            IEaseeAPIAuthenticationService authService,
            IHttpClientFactory httpClientFactory,
            IHttpService httpService
        )
        {
            _logger = logger;
            _config = config;
            _authService = authService;
            _httpService = httpService;
            _httpClient = httpClientFactory.CreateClient(nameof(EaseeAPIAuthenticationHeaderDelegatingHandler));
        }

        public async Task<IEnumerable<EaseeChargerResponseDTO>> GetChargersAsync()
        {
            var requestUriString = $"{_config.BaseUri}/{_config.ChargerEndpoint}";
            var chargers = await _httpService.GetAsync<IEnumerable<EaseeChargerResponseDTO>>(_httpClient, requestUriString, _authService.AccessToken);

            return chargers;
        }

        public async Task<EaseeChargerResponseDTO> AddChargerAsync(string serialCode, string pinCode)
        {
            var newChargerDTO = new AddEaseeChargerDTO { SerialNo = serialCode, PinCode = pinCode };

            var uri = new Uri($"{_config.BaseUri}/{_config.ChargerEndpoint}");
            var body = JsonSerializer.Serialize(newChargerDTO);

            var newEaseeCharger = await _httpService.PostAsync<EaseeChargerResponseDTO>(_httpClient, uri, body, _authService.AccessToken);

            return newEaseeCharger;
        }
    }
}
