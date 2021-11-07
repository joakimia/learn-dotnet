using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ELDEL_API.Extensions.DTOs;

namespace ELDEL_API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/accounts/sign-in")]
    public class SignInAccountsController : Controller
    {
        private readonly ILogger<SignInAccountsController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;

        public SignInAccountsController(
            ILogger<SignInAccountsController> logger,
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EldelJWTAccessTokenDTO>> PostSignInAsync([FromBody] AccountCredentialDTO accountCredentialDTO)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["controller"] = nameof(SignInAccountsController),
                ["function"] = nameof(PostSignInAsync),
                ["email"] = accountCredentialDTO?.Email
            };

            using (_logger.BeginScope(loggerScope))
            {
                try
                {
                    _logger.LogInformation($"Sign in request called.");

                    accountCredentialDTO = accountCredentialDTO.ToValidatedAccountCredentialDTO();
                    if (!string.IsNullOrEmpty(accountCredentialDTO.Message))
                    {
                        _logger.LogInformation($"Sign in request failed. Validation error, message: {accountCredentialDTO.Message}.");
                        return BadRequest(new { accountCredentialDTO.Message });
                    }

                    var account = await _accountService.GetAccountByEmailAsync(accountCredentialDTO.Email);
                    if (account == null)
                    {
                        _logger.LogInformation($"Sign in request failed. Account does not exist.");
                        return BadRequest(new { Message = $"Sign in request failed. Account does not exist." });
                    }

                    var signInResult = await _accountService.CheckPasswordSignInAsync(account, accountCredentialDTO.Password, false);
                    if (signInResult.Succeeded)
                    {
                        var eldelJWTToken = _authenticationService.AuthorizeByAccountDTO(AccountDTO.Map(account));

                        _logger.LogInformation($"Sign in request succeeded.");
                        return Ok(eldelJWTToken);
                    }
                    else
                    {
                        _logger.LogInformation($"Sign in request failed. Bad password input.");
                        return BadRequest(new { Message = $"Sign in request failed. Invalid email or password." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Sign in request error.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Sign in request failed. An unhandled exception have appeared." });
                }
            }
        }
    }
}
