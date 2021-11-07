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
using System.Linq;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/chargers/{chargerId}/address")]
    public class GetChargersAddressController : Controller
    {
        private readonly ILogger<GetChargersAddressController> _logger;
        private readonly IAccountService _accountService;
        private readonly IChargersService _chargersService;

        public GetChargersAddressController(
            ILogger<GetChargersAddressController> logger,
            IAccountService accountService,
            IChargersService chargersService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _chargersService = chargersService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressDTO>> GetChargersAddressAsync([FromRoute] string chargerId)
        {
            _logger.LogInformation($"Get charger's address request called.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Get charger's address request failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Get charger's address request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(GetChargersAddressController),
                ["function"] = nameof(GetChargersAddressAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = tokenAccount.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    var account = await _accountService.GetAccountByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Get charger's address request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Get chargers's address request failed. Account does not exist." });
                    }

                    var charger = await _chargersService.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerId);
                    if (charger == null)
                    {
                        _logger.LogInformation($"Get charger's address request failed. No charger was found.");
                        return BadRequest(new { Message = $"Get chargers's address request failed. Charger does not exist." });
                    }

                    if (!charger.Accounts.Any(ch => ch.Email == tokenAccount.Email))
                    {
                        _logger.LogWarning($"Get charger's address request failed. Account tried to get another charger's address without sufficient access rights.");
                        return Unauthorized(new { Message = $"Get charger's address request failed. Insufficient access rights." });
                    }

                    var addressDTO = charger.Address == null ? null : AddressDTO.Map(charger.Address);

                    _logger.LogInformation($"Get charger's address request succeeded.");
                    return Ok(addressDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get charger's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Get charger's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
