using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.JWTExtensions;
using ELDEL_API.AccountCredentialExtensions;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/accounts/{accountId}")]
    public class DeleteAccountsController : Controller
    {
        private readonly ILogger<DeleteAccountsController> _logger;
        private readonly IAccountService _accountService;

        public DeleteAccountsController(
            ILogger<DeleteAccountsController> logger,
            IAccountService accountService
        )
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAccountAsync([FromRoute] string accountId)
        {
            _logger.LogInformation($"Delete account request called, accountUniqueId: {accountId}.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning($"Delete account request failed. Unable to decode access token from header, accountUniqueId: {accountId}.");
                return Unauthorized(new { Message = "Delete account request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(DeleteAccountsController),
                ["function"] = nameof(DeleteAccountAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = accountId
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    tokenAccount = tokenAccount.ToValidatedAccountDTO();
                    if (!string.IsNullOrEmpty(tokenAccount.Message))
                    {
                        _logger.LogInformation($"Delete account request failed. Validation error, message: {tokenAccount.Message}.");
                        return BadRequest(new { tokenAccount.Message });
                    }

                    var account = await _accountService.GetAccountByAccountIdAsync(accountId);
                    if (account == null)
                    {
                        _logger.LogInformation($"Delete account request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Delete account request failed. Account does not exist." });
                    }

                    if (tokenAccount.Id != account.UniqueId)
                    {
                        _logger.LogWarning($"Delete account request failed. Account tried to delete another email: {account.Email}.");
                        return Unauthorized(new { Message = $"Delete account request failed. Unauthorized, insufficient access rights to delete other accounts." });
                    }

                    var deleteAccountResult = await _accountService.DeleteAccountAsync(account);
                    if (deleteAccountResult.Succeeded)
                    {
                        _logger.LogInformation($"Delete account request succeeded.");
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Delete account request failed. Account exists, but removal failed.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Delete account request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Delete account request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
