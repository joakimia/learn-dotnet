using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.JWTExtensions;
using System.Linq;
using ELDEL_API.AccountCredentialExtensions;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}")]
    public class DeleteChargersController : Controller
    {
        private readonly ILogger<DeleteChargersController> _logger;
        private readonly IAccountService _accountService;
        private readonly IChargersService _chargersService;

        public DeleteChargersController(
            ILogger<DeleteChargersController> logger,
            IAccountService accountService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _chargersService = chargersService;
            _accountService = accountService;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteChargerAsync([FromRoute] string chargerId)
        {
            _logger.LogInformation($"Delete charger request called, chargerUniqueId: {chargerId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Delete charger request failed. Unable to decode access token from header, chargerUniqueId: {chargerId}.");
                return Unauthorized(new { Message = "Delete charger request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(DeleteChargersController),
                ["function"] = nameof(DeleteChargerAsync),
                ["email"] = tokenAccount.Email,
                ["chargerUniqueId"] = chargerId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    tokenAccount = tokenAccount.ToValidatedAccountDTO();
                    if (!string.IsNullOrEmpty(tokenAccount.Message))
                    {
                        _logger.LogInformation($"Delete charger request failed. Validation error, message: {tokenAccount.Message}.");
                        return BadRequest(new { tokenAccount.Message });
                    }

                    var account = await _accountService.GetAccountByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Delete charger request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Delete charger request failed. Account does not exist." });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Delete charger request failed. Charger does not exist.");
                        return BadRequest(new { Message = $"Delete charger request failed. Charger does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Delete charger request failed. Account tried to delete another charger without sufficient access rights.");
                        return Unauthorized(new { Message = $"Delete charger failed. Insufficient access rights." });
                    }

                    var isDeleted = await _chargersService.DeleteChargerAsync(charger);
                    if (isDeleted)
                    {
                        _logger.LogInformation($"Delete charger request succeeded.");
                        return Ok();
                    }
                    else
                    {
                        throw new Exception($"Delete charger request failed. Charger exist, but removal failed.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Delete charger error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete charger request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
