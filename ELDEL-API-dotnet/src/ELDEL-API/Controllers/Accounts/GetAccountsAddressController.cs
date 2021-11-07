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

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/accounts/{accountId}/address")]
    public class GetAccountsAddressController : Controller
    {
        private readonly ILogger<GetAccountsAddressController> _logger;
        private readonly IAccountService _accountService;

        public GetAccountsAddressController(
            ILogger<GetAccountsAddressController> logger,
            IAccountService accountService
        )
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressDTO>> GetAccountsAddressAsync()
        {
            _logger.LogInformation($"Get account's address called.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Get account's address request failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Get account's address request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(GetAccountsAddressController),
                ["function"] = nameof(GetAccountsAddressAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = tokenAccount.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    var account = await _accountService.GetAccountWithAddressByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Get account's address request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Get account's address request failed. Account does not exist." });
                    }

                    var addressDTO = account.Address == null ? null : AddressDTO.Map(account.Address);

                    _logger.LogInformation($"Get account's address request succeeded.");
                    return Ok(addressDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get account's address request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Get account's address request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
