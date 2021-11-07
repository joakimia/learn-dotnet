using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_API.Extensions.DTOs;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ELDEL_API.Controllers.Accounts
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/accounts/register")]
    public class RegisterAccountsController : ControllerBase
    {
        private readonly ILogger<RegisterAccountsController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;

        public RegisterAccountsController(
            ILogger<RegisterAccountsController> logger,
            IAccountService accountService,
            IAuthenticationService authenticationService
        )
        {
            _logger = logger;
            _accountService = accountService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EldelJWTAccessTokenDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EldelJWTAccessTokenDTO>> RegisterAccountAsync([FromBody] AccountCredentialDTO accountCredentialDTO)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(RegisterAccountsController),
                ["function"] = nameof(RegisterAccountAsync),
                ["email"] = accountCredentialDTO?.Email,
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    _logger.LogInformation($"Register account request called.");

                    accountCredentialDTO = accountCredentialDTO.ToValidatedAccountCredentialDTO();
                    if (!string.IsNullOrEmpty(accountCredentialDTO.Message))
                    {
                        _logger.LogInformation($"Register account failed. Validation error, message: {accountCredentialDTO.Message}.");
                        return BadRequest(new { accountCredentialDTO.Message });
                    }

                    var account = await _accountService.GetAccountByEmailAsync(accountCredentialDTO.Email);
                    if (account != null)
                    {
                        _logger.LogInformation($"Register account failed request. Email already exists.");
                        return NoContent();
                    }

                    var newAccountResult = await _accountService.AddAccountAsync(accountCredentialDTO);
                    if (!newAccountResult.Succeeded)
                    {
                        var errorMessage = new StringBuilder();
                        newAccountResult.Errors.ToList().ForEach(e => errorMessage.Append($"{e.Description} {Environment.NewLine} "));
                        _logger.LogInformation($"Register account request failed. Reason: {errorMessage}.");
                        return BadRequest(new { Message = errorMessage });
                    }

                    account = await _accountService.GetAccountByEmailAsync(accountCredentialDTO.Email);
                    var eldelJWTTokenDTO = _authenticationService.AuthorizeByAccountDTO(AccountDTO.Map(account));

                    _logger.LogInformation($"Register account request succeeded.");
                    return Ok(eldelJWTTokenDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Register account request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Register account request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
