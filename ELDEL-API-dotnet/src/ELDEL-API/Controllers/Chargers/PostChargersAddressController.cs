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
using ELDEL_API.AddressExtensions;
using System.Linq;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}/address")]
    public class PostChargersAddressController : Controller
    {
        private readonly ILogger<PostChargersAddressController> _logger;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly IChargersService _chargersService;

        public PostChargersAddressController(
            ILogger<PostChargersAddressController> logger,
            IAddressService addressService,
            IAccountService accountService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _addressService = addressService;
            _accountService = accountService;
            _chargersService = chargersService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChargerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ChargerDTO>> PostChargersAddressAsync([FromRoute] string chargerId, [FromBody] AddressDTO addressDTO)
        {
            _logger.LogInformation($"POST add charger's address request called, chargerUniqueId: {chargerId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Add charger's address request failed. Unable to decode access token from header, chargerUniqueId: {chargerId}.");
                return Unauthorized(new { Message = "Add charger's address requet failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(PostChargersAddressController),
                ["function"] = nameof(PostChargersAddressAsync),
                ["email"] = tokenAccount.Email,
                ["chargerUniqueId"] = chargerId,
            };


            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    addressDTO = addressDTO.ToValidatedAddressDTO();
                    if (!string.IsNullOrEmpty(addressDTO.Message))
                    {
                        _logger.LogInformation($"Add charger's address request failed. Validation error, message: {addressDTO.Message}.");
                        return BadRequest(new { addressDTO.Message });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Add charger's address request failed. Charger does not exist.");
                        return BadRequest(new { Message = $"Add charger's address request failed. Charger does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Add charger's address request failed. Account tried to get another charger's address without sufficient access rights.");
                        return Unauthorized(new { Message = $"Add charger's address request failed. Insufficient access rights." });
                    }

                    charger = await _addressService.AddChargerAddressAsync(charger, addressDTO, tokenAccount.Email);
                    var chargerDTO = ChargerDTO.Map(charger);

                    _logger.LogInformation("Add charger's address request succeeded.");
                    return Ok(chargerDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Add charger's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Add charger's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
