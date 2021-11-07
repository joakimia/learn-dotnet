using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;
using ELDEL_EntityFramework_Library.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ELDEL_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<Account> GetAccountByAccountIdAsync(string accountUniqueId)
        {
            return await _accountRepository.GetAccountByAccountIdAsync(accountUniqueId);
        }

        public async Task<Account> GetAccountWithAddressByAccountIdAsync(string accountUniqueId)
        {
            return await _accountRepository.GetAccountWithAddressByIdAsync(accountUniqueId);
        }

        public async Task<IdentityResult> AddAccountAsync(AccountCredentialDTO accountCredentialDTO)
        {
            return await _accountRepository.AddAccountAsync(accountCredentialDTO.Email, accountCredentialDTO.Password);
        }

        public async Task<Account> UpdateAccountInfoAsync(Account account, AccountDTO accountDTO, string updatedByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AccountService),
                ["function"] = nameof(UpdateAccountInfoAsync),
                ["email"] = updatedByEmail,
                ["accountUniqueId"] = account.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (account == null)
                {
                    _logger.LogInformation($"Update account info failed: Unable to update account info for non-existing account (account == null).");
                    return null;
                }

                return await _accountRepository.UpdateAccountInfoAsync(
                    account,
                    accountDTO.FirstName,
                    accountDTO.LastName,
                    accountDTO.Email,
                    accountDTO.PhoneNumberPrefix,
                    accountDTO.PhoneNumber,
                    updatedByEmail
                );
            }
        }

        public async Task<IdentityResult> DeleteAccountAsync(Account account)
        {
            return await _accountRepository.DeleteAccountAsync(account);
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(Account account, string password, bool signInResult)
        {
            return await _accountRepository.CheckPasswordSignInAsync(account, password, signInResult);
        }
    }
}
