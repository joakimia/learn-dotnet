using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ELDEL_API.Config;
using ELDEL_API.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ELDEL_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IEldelAuthenticationConfig _config;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IEldelAuthenticationConfig config
        )
        {
            _logger = logger;
            _config = config;
        }

        public EldelJWTAccessTokenDTO AuthorizeByAccountDTO(AccountDTO accountDTO)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AuthenticationService),
                ["function"] = nameof(AuthorizeByAccountDTO),
                ["email"] = accountDTO.Email,
                ["accountUniqueId"] = accountDTO.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var claims = GetClaimsByAccountDTO(accountDTO);
                if (claims == null)
                {
                    _logger.LogInformation("Authorization failed.");
                    return null;
                }

                return GenerateToken(claims.ToArray(), DateTime.UtcNow.Add(_config.TokenLifetime));
            }
        }

        public AccountDTO ValidateToken(string token)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AuthenticationService),
                ["function"] = nameof(ValidateToken),
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Validation failed, empty token.");
                    return null;
                }

                try
                {
                    return DecodeToken(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erroneous token.");
                    return null;
                }
            }
        }

        private static Claim[] GetClaimsByAccountDTO(AccountDTO account)
        {
            return new Claim[]
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.GivenName, account.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, account.LastName ?? ""),
                new Claim(ClaimTypes.Name, account.FullName ?? ""),
            };
        }

        private static TokenValidationParameters GetValidationParameters(IEldelAuthenticationConfig config)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret))
            };
        }

        private AccountDTO DecodeToken(string token)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AuthenticationService),
                ["function"] = nameof(DecodeToken),
            };

            using (_logger.BeginScope(loggerScope))
            {
                token = token.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Empty token.");
                    return null;
                }

                var validationParameters = GetValidationParameters(_config);
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var claims = jwtSecurityToken.Claims;
                    return new AccountDTO
                    {
                        Email = claims.FirstOrDefault(x => x.Type == "email").Value,
                        Id = claims.FirstOrDefault(x => x.Type == "nameid").Value,
                        FirstName = claims.FirstOrDefault(x => x.Type == "given_name").Value,
                        LastName = claims.FirstOrDefault(x => x.Type == "family_name").Value,
                        FullName = claims.FirstOrDefault(x => x.Type == "unique_name").Value
                    };
                }

                _logger.LogInformation("Decoding finished, but no token retrieved.");
                return null;
            }
        }

        private EldelJWTAccessTokenDTO GenerateToken(Claim[] claims, DateTime? expiration = null)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration ?? DateTime.UtcNow.AddHours(4),
                SigningCredentials = creds,
                Issuer = _config.Issuer,
                IssuedAt = DateTime.UtcNow,
                Audience = _config.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var localAuthToken = tokenHandler.CreateToken(tokenDescriptor);
            var responseToken = tokenHandler.WriteToken(localAuthToken);

            return new EldelJWTAccessTokenDTO
            {
                AccessToken = responseToken,
            };
        }
    }
}
