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
    [Route("api/v1/accounts/{accountId}")]
    public class UpdateAccountsController : Controller
    {
        private readonly ILogger<UpdateAccountsController> _logger;
        private readonly IAccountService _accountService;

        public UpdateAccountsController(
            ILogger<UpdateAccountsController> logger,
            IAccountService accountService
        )
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PutUpdateAccountInfoAsync(
            [FromRoute] string accountId,
            [FromBody] AccountDTO accountDTO
        )
        {
            _logger.LogInformation($"PUT update account request called, accountUniqueId: {accountId}.");
            return await UpdateAccountInfoAsync(accountId, accountDTO);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> PatchUpdateAccountInfoAsync(
            [FromRoute] string accountId,
            [FromBody] AccountDTO accountDTO
        )
        {
            _logger.LogInformation($"PATCH update account request called, accountUniqueId: {accountId}.");
            return await UpdateAccountInfoAsync(accountId, accountDTO);
        }

        private async Task<ActionResult<AccountDTO>> UpdateAccountInfoAsync(string accountId, AccountDTO accountDTO)
        {
            _logger.LogInformation($"Update account request called, accountUniqueId: {accountId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Update account request failed. Unable to decode access token from header, accountUniqueId: {accountId}.");
                return Unauthorized(new { Message = "Update account request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(UpdateAccountsController),
                ["function"] = nameof(UpdateAccountInfoAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = accountId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    var account = await _accountService.GetAccountByAccountIdAsync(accountId);
                    if (account == null)
                    {
                        _logger.LogInformation($"Update account request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Update account request failed. Account does not exist." });
                    }

                    if (tokenAccount.Id != account.UniqueId)
                    {
                        _logger.LogWarning($"Update account request failed. Account tried to update another account's address, with email: {account.Email}.");
                        return Unauthorized(new { Message = $"Update account request failed. Unauthorized, insufficient access rights to update other accounts." });
                    }

                    account = await _accountService.UpdateAccountInfoAsync(account, accountDTO, tokenAccount.Email);
                    accountDTO = AccountDTO.Map(account);

                    _logger.LogInformation($"Update account request succeeded.");
                    return Ok(accountDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Update account request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Update account request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
