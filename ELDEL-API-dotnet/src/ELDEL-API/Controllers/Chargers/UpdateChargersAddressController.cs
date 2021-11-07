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
using ELDEL_API.AddressExtensions;
using System.Linq;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}/address/{addressId}")]
    public class UpdateChargersAddressController : Controller
    {
        private readonly ILogger<UpdateChargersAddressController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IChargersService _chargersService;

        public UpdateChargersAddressController(
            ILogger<UpdateChargersAddressController> logger,
            IAccountService accountService,
            IAddressService addressService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _addressService = addressService;
            _chargersService = chargersService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressDTO>> PutUpdateChargersAddressAsync(
            [FromRoute] string chargerId,
            [FromRoute] string addressId,
            [FromBody] AddressDTO addressDTO
        )
        {
            _logger.LogInformation($"PUT update charger's address request called, chargerUniqueId {chargerId}, addressUniqueId: {addressId}.");
            return await UpdateChargersAddressAsync(chargerId, addressId, addressDTO);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressDTO>> PatchUpdateChargersAddressAsync(
            [FromRoute] string chargerId,
            [FromRoute] string addressId,
            [FromBody] AddressDTO addressDTO
        )
        {
            _logger.LogInformation($"PATCH update charger's address request called, chargerUniqueId {chargerId}, addressUniqueId: {addressId}.");
            return await UpdateChargersAddressAsync(chargerId, addressId, addressDTO);
        }

        private async Task<ActionResult<AddressDTO>> UpdateChargersAddressAsync(string chargerId, string addressId, AddressDTO addressDTO)
        {
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Update charger's address. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Update charger's address. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(UpdateChargersAddressController),
                ["function"] = nameof(UpdateChargersAddressAsync),
                ["email"] = tokenAccount.Email,
                ["chargerUniqueId"] = chargerId,
                ["addressUniqueId"] = addressId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    addressDTO = addressDTO.ToValidatedAddressDTO();
                    if (!string.IsNullOrEmpty(addressDTO.Message))
                    {
                        _logger.LogInformation($"Update charger's address request failed. Validation error, message: {addressDTO.Message}.");
                        return BadRequest(new { addressDTO.Message });
                    }

                    var account = await _accountService.GetAccountByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Update charger's address request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Update charger's address request failed. Account does not exist." });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Update charger's address failed attempt by {tokenAccount.Email}. Charger does not exist.");
                        return BadRequest(new { Message = $"Update charger's address request failed. Charger does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Update charger's address request failed. Account tried to update another charger's address without sufficient access rights.");
                        return Unauthorized(new { Message = $"Update charger's address request failed. Insufficient access rights." });
                    }

                    if (charger.Address == null)
                    {
                        _logger.LogInformation($"Update charger's address request failed. Unable to update non-existing address.");
                        return BadRequest(new { Message = $"Update charger's address request failed. Address does not exist." });
                    }

                    if (charger.AddressUniqueId != addressId)
                    {
                        _logger.LogWarning($"Update charger's address request failed. Address uniqueId ( {addressId} ) in URL is not the same as charger.AddressUniqueId ( {charger.AddressUniqueId} ).");
                        return Unauthorized(new { Message = $"Update charger's address request failed. Invalid addressId." });
                    }

                    addressDTO.Id = charger.AddressUniqueId;
                    charger = await _addressService.UpdateChargerAddressAsync(charger, addressDTO, tokenAccount.Email);
                    addressDTO = AddressDTO.Map(charger.Address);

                    _logger.LogInformation($"Update charger's address request succeeded.");
                    return Ok(addressDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update charger's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update charger's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
