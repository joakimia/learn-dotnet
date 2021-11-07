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
    [Route("api/v1/account")]
    public class GetAccountsController : Controller
    {
        private readonly ILogger<GetAccountsController> _logger;
        private readonly IAccountService _accountService;

        public GetAccountsController(
            ILogger<GetAccountsController> logger,
            IAccountService accountService
        )
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDTO>> GetAccountAsync()
        {
            _logger.LogInformation($"Get account request called.");
            var tokenAccount = this.GetAccountFromAccessToken();
            if (tokenAccount == null)
            {
                _logger.LogWarning("Get account request failed. Unable to decode access token from header.");
                return Unauthorized(new { Message = "Get account request failed. Invalid token." });
            }

            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(GetAccountsController),
                ["function"] = nameof(GetAccountAsync),
                ["email"] = tokenAccount.Email,
                ["accountUniqueId"] = tokenAccount.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    if (tokenAccount == null)
                    {
                        _logger.LogInformation($"Get account request failed. Invalid token.");
                        return Unauthorized(new { Message = "Get account request failed. Invalid token." });
                    }

                    var account = await _accountService.GetAccountWithAddressByAccountIdAsync(tokenAccount.Id);
                    if (account == null)
                    {
                        _logger.LogInformation($"Get account request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Get account request failed. Account does not exist." });
                    }

                    _logger.LogInformation($"Get account request succeeded.");
                    return Ok(AccountDTO.Map(account));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get account request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Get account request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
