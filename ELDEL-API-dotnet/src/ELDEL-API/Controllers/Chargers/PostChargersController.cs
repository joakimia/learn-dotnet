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
using ELDEL_API.ChargerExtensions;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers")]
    public class PostChargersController : Controller
    {
        private readonly ILogger<PostChargersController> _logger;
        private readonly IChargersService _chargersService;
        private readonly IAccountService _accountService;

        public PostChargersController(
            ILogger<PostChargersController> logger,
            IChargersService chargersService,
            IAccountService accountService
        )
        {
            _logger = logger;
            _chargersService = chargersService;
            _accountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChargerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ChargerDTO>> PostChargerAsync([FromBody] ChargerDTO chargerDTO)
        {
            _logger.LogInformation("POST add charger request called.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Add charger request failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Add charger request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(PostChargersController),
                ["function"] = nameof(PostChargerAsync),
                ["email"] = tokenAccount.Email
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    chargerDTO = chargerDTO.ToValidatedChargerDTO();
                    if (!string.IsNullOrEmpty(chargerDTO.Message))
                    {
                        _logger.LogInformation($"Add charger request failed. Validation error, message: {chargerDTO.Message}.");
                        return BadRequest(new { chargerDTO.Message });
                    }

                    var charger = await _chargersService.AddChargerAsync(chargerDTO, tokenAccount);
                    chargerDTO = ChargerDTO.Map(charger);

                    _logger.LogInformation("Add charger request succeeded.");
                    return Ok(chargerDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Add charger request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Add charger request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
