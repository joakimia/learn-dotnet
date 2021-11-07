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

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/accounts/{accountId}/address/{addressId}")]
    public class UpdateAccountsAddressController : Controller
    {
        private readonly ILogger<UpdateAccountsAddressController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public UpdateAccountsAddressController(
            ILogger<UpdateAccountsAddressController> logger,
            IAccountService accountService,
            IAddressService addressService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _addressService = addressService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PutUpdateAddressAsync(
            [FromRoute] string accountId,
            [FromRoute] string addressId,
            [FromBody] AddressDTO addressDTO
        )
        {
            _logger.LogInformation($"PUT update account's address called , accountUniqueId: {accountId}, addressUniqueId: {addressId}.");
            return await UpdateAccountAddressAsync(accountId, addressId, addressDTO);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PatchUpdateChargerAsync(
            [FromRoute] string accountId,
            [FromRoute] string addressId,
            [FromBody] AddressDTO addressDTO
        )
        {
            _logger.LogInformation($"PATCH update account's address request called, accountUniqueId: {accountId}, addressUniqueId: {addressId}.");
            return await UpdateAccountAddressAsync(accountId, addressId, addressDTO);
        }

        private async Task<ActionResult<AccountDTO>> UpdateAccountAddressAsync(string accountId, string addressId, AddressDTO addressDTO)
        {
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Update account's address request failed. Unable to decode access token from header, acountUniqueId: {accountId}, addressUniqueId: {addressId}.");
                return Unauthorized(new { Message = "Update account's address request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(UpdateAccountsAddressController),
                ["function"] = nameof(UpdateAccountAddressAsync),
                ["email"] = tokenAccount.Email,
                ["accountId"] = accountId,
                ["addressUniqueId"] = addressId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    addressDTO = addressDTO.ToValidatedAddressDTO();
                    if (!string.IsNullOrEmpty(addressDTO.Message))
                    {
                        _logger.LogInformation($"Update account's address request failed. Validation error, message: {addressDTO.Message}.");
                        return BadRequest(new { addressDTO.Message });
                    }

                    var account = await _accountService.GetAccountWithAddressByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Update account's address request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Update account's address request failed. Account does not exist." });
                    }

                    if (account.Address == null)
                    {
                        _logger.LogInformation($"Update account's address request. Unable to update non-existing address.");
                        return BadRequest(new { Message = $"Update account's address request failed. Address does not exist." });
                    }

                    if (tokenAccount.Id != account.UniqueId)
                    {
                        _logger.LogWarning($"Update account's address request failed. Account tried to delete another account's address, with email: {account.Email}.");
                        return Unauthorized(new { Message = $"Update account's address request. Unauthorized, insufficient access rights to update other accounts." });
                    }

                    if (account.AddressUniqueId != addressId)
                    {
                        _logger.LogInformation($"Update account's address request failed. Address unique id in URL is not the same as charger.AddressUniqueId.");
                        return Unauthorized(new { Message = $"Update account's address request failed. Invalid addressId." });
                    }

                    addressDTO.Id = account.AddressUniqueId;
                    account = await _addressService.UpdateAccountAddressAsync(account, addressDTO, tokenAccount.Email);
                    addressDTO = AddressDTO.Map(account.Address);

                    _logger.LogInformation($"Update account's address request succeeded.");
                    return Ok(addressDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update account's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update account's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
