using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.JWTExtensions;
using ELDEL_API.ChargerExtensions;
using System.Linq;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}")]
    public class UpdateChargersController : Controller
    {
        private readonly ILogger<UpdateChargersController> _logger;
        private readonly IAccountService _accountService;
        private readonly IChargersService _chargersService;

        public UpdateChargersController(
            ILogger<UpdateChargersController> logger,
            IAccountService accountService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _chargersService = chargersService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChargerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PutUpdateChargerAsync(
            [FromRoute] string chargerId,
            [FromBody] ChargerDTO chargerDTO
        )
        {
            return await UpdateChargerAsync(chargerId, chargerDTO);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChargerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PatchUpdateChargerAsync(
            [FromRoute] string chargerId,
            [FromBody] ChargerDTO chargerDTO
        )
        {
            return await UpdateChargerAsync(chargerId, chargerDTO);
        }

        private async Task<ActionResult<AccountDTO>> UpdateChargerAsync(string chargerId, ChargerDTO chargerDTO)
        {
            _logger.LogInformation($"Update charger called, chargerUniqueId: {chargerId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Update charger failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Update charger failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(UpdateChargersController),
                ["function"] = nameof(UpdateChargerAsync),
                ["email"] = tokenAccount.Email,
                ["chargerUniqueId"] = chargerId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    chargerDTO = chargerDTO.ToValidatedChargerDTO();
                    if (!string.IsNullOrEmpty(chargerDTO.Message))
                    {
                        _logger.LogInformation($"Update charger request failed. Validation error, message: {chargerDTO.Message}.");
                        return BadRequest(new { chargerDTO.Message });
                    }

                    var account = await _accountService.GetAccountByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Update charger request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Update charger request failed. Account does not exist." });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Update charger request failed. Charger does not exist.");
                        return BadRequest(new { Message = $"Update charger request failed. Charger does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Update charger request failed. Account tried to update another charger without sufficient access rights.");
                        return Unauthorized(new { Message = $"Update charger request failed. Insufficient access rights." });
                    }

                    charger = await _chargersService.UpdateChargerAsync(charger, chargerDTO, tokenAccount);
                    chargerDTO = ChargerDTO.Map(charger);

                    _logger.LogInformation($"Update charger request succeeded.");
                    return Ok(chargerDTO);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update charger request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update charger request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
