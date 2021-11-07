using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ELDEL_API.AccountCredentialExtensions;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.JWTExtensions;
using ELDEL_API.DTOs;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/accounts/{accountId}/address/{addressId}")]
    public class DeleteAccountsAddressController : Controller
    {
        private readonly ILogger<DeleteAccountsAddressController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public DeleteAccountsAddressController(
            ILogger<DeleteAccountsAddressController> logger,
            IAccountService accountService,
            IAddressService addressService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _addressService = addressService;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> DeleteAccountsAddressAsync([FromRoute] string accountId, string addressId)
        {
            _logger.LogInformation($"Delete account's address request called, accountUniqueId: {accountId}, addressUniqueId: {addressId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Delete account's address request failed. Unable to decode access token from header, accountUniqueId: {accountId}, addressUniqueId: {addressId}.");
                return Unauthorized(new { Message = "Delete account request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(DeleteAccountsAddressController),
                ["function"] = nameof(DeleteAccountsAddressAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = accountId,
                ["addressUniqueId"] = addressId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    var accountDTO = tokenAccount.ToValidatedAccountDTO();
                    if (!string.IsNullOrEmpty(accountDTO.Message))
                    {
                        _logger.LogInformation($"Delete account's address request failed. Validation error, message: {accountDTO.Message}.");
                        return BadRequest(new { accountDTO.Message });
                    }

                    var account = await _accountService.GetAccountWithAddressByAccountIdAsync(accountId);
                    if (account == null)
                    {
                        _logger.LogInformation($"Delete account's address request. Account does not exist.");
                        return BadRequest(new { Message = $"Delete failed request. Account does not exist." });
                    }

                    if (tokenAccount.Id != account.UniqueId)
                    {
                        _logger.LogWarning($"Delete account's address request failed. Account tried to delete another email: {account.Email}.");
                        return Unauthorized(new { Message = $"Delete account's address failed. Unauthorized, insufficient access rights to delete other accounts." });
                    }

                    account = await _addressService.DeleteAccountAddressAsync(account, tokenAccount.Email);
                    accountDTO = AccountDTO.Map(account);

                    _logger.LogInformation($"Delete account's address request succeeded.");
                    return Ok(accountDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Delete account's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete account's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
