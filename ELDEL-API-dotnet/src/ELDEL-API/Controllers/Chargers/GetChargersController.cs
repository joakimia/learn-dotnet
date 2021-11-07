using ELDEL_API.DTOs;
using ELDEL_API.Services;
using ELDEL_API.JWTExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using ELDEL_API.Filters;

namespace ELDEL_API.Controllers
{
    [ApiController]
    public class GetChargersController : Controller
    {
        private readonly ILogger<GetChargersController> _logger;
        private readonly IChargersService _chargersService;

        public GetChargersController(ILogger<GetChargersController> logger, IChargersService chargersService)
        {
            _logger = logger;
            _chargersService = chargersService;
        }

        [HttpGet]
        [Route("api/v1/accounts/chargers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChargerDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ChargerDTO>>> GetChargersAsync()
        {
            _logger.LogInformation("Get chargers request called.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Get chargers request failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Get chargers request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(GetChargersController),
                ["function"] = nameof(GetChargersAsync),
                ["email"] = tokenAccount.Email,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    var chargers = await _chargersService.GetChargersByAccountIdAsync(tokenAccount.Id);

                    var sortedChargerDTOs = chargers
                        .Select(ch => ChargerDTO.Map(ch))
                        .OrderByDescending(ch => ch.Name);

                    _logger.LogInformation("Get chargers request succeeded.");
                    return Ok(sortedChargerDTOs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get chargers request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Get chargers request failed. An unhandled exception have appeared." });
                }
            }
        }

        [HttpGet]
        [Route("api/v1/chargers")]
        [ChargersApiKey]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChargerDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ChargerDTO>>> GetAllhargersAsync()
        {

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(GetChargersController),
                ["function"] = nameof(GetAllhargersAsync),
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    _logger.LogInformation("Get all chargers request called.");

                    var chargers = await _chargersService.GetAllChargersAsync();

                    var sortedChargerDTOs = chargers
                        .Select(ch => ChargerDTO.Map(ch))
                        .OrderByDescending(ch => ch.Name);

                    _logger.LogInformation("Get all chargers request succeeded.");
                    return Ok(sortedChargerDTOs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get all chargers request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Get all chargers request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
