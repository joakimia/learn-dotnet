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

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/accounts/{accountId}/address")]
    public class PostAccountsAddressController : Controller
    {
        private readonly ILogger<PostAccountsAddressController> _logger;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;

        public PostAccountsAddressController(
            ILogger<PostAccountsAddressController> logger,
            IAddressService addressService,
            IAccountService accountService
        )
        {
            _logger = logger;
            _addressService = addressService;
            _accountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PostAccountsAddressAsync([FromRoute] string accountId, [FromBody] AddressDTO addressDTO)
        {
            _logger.LogInformation($"POST add account's address request called, accountUniqueId: {accountId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Add account's address request failed. Unable to decode access token from header, accountUniqueId: {accountId}.");
                return Unauthorized(new { Message = "Add account's address request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(PostAccountsAddressController),
                ["function"] = nameof(PostAccountsAddressAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = accountId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    addressDTO = addressDTO.ToValidatedAddressDTO();
                    if (!string.IsNullOrEmpty(addressDTO.Message))
                    {
                        _logger.LogInformation($"Add account's address request failed. Validation error, message: {addressDTO.Message}.");
                        return BadRequest(new { addressDTO.Message });
                    }

                    var account = await _accountService.GetAccountByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Add account's address request failed attempt by {tokenAccount.Email}. Account does not exist.");
                        return BadRequest(new { Message = $"Add account's address request failed. Account does not exist." });
                    }

                    account = await _addressService.AddAccountAddressAsync(account, addressDTO, tokenAccount.Email);
                    var accountDTO = AccountDTO.Map(account);

                    _logger.LogInformation("Add account's address request succeeded.");
                    return Ok(accountDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Add account's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Add account's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
