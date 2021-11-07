using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.JWTExtensions;
using ELDEL_API.DTOs;
using System.Linq;
using ELDEL_API.AccountCredentialExtensions;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}/address/{addressId}")]
    public class DeleteChargersAddressController : Controller
    {
        private readonly ILogger<DeleteChargersAddressController> _logger;
        private readonly IAddressService _addressService;
        private readonly IChargersService _chargersService;

        public DeleteChargersAddressController(
            ILogger<DeleteChargersAddressController> logger,
            IAddressService addressService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _addressService = addressService;
            _chargersService = chargersService;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChargerDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ChargerDTO>> DeleteChargersAddressAsync([FromRoute] string chargerId, string addressId)
        {
            _logger.LogInformation($"Delete chargers's address request called, chargerUniqueId: {chargerId}, addressUniqueId: {addressId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Delete charger's address request failed. Unable to decode access token from header, chargerUniqueId: {chargerId}, addressUniqueId: {addressId}.");
                return Unauthorized(new { Message = "Delete charger's address failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(DeleteChargersAddressController),
                ["function"] = nameof(DeleteChargersAddressAsync),
                ["email"] = tokenAccount.Email,
                ["chargerUniqueId"] = chargerId,
                ["addressUniqueId"] = addressId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    tokenAccount = tokenAccount.ToValidatedAccountDTO();
                    if (!string.IsNullOrEmpty(tokenAccount.Message))
                    {
                        _logger.LogInformation($"Delete charger's address request failed. Validation error, message: {tokenAccount.Message}.");
                        return BadRequest(new { tokenAccount.Message });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Delete charger's address request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Delete charger's address request failed. Account does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Delete charger's address request failed. Account tried to delete another charger's address without sufficient access rights.");
                        return Unauthorized(new { Message = $"Delete charger's address request failed. Insufficient access rights." });
                    }

                    charger = await _addressService.DeleteChargerAddressAsync(charger, tokenAccount.Email);
                    var chargerDTO = ChargerDTO.Map(charger);

                    _logger.LogInformation($"Delete charger's address request succeeded.");
                    return Ok(chargerDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Delete charger's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete charger's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
