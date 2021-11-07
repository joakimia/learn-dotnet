using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;
using Microsoft.AspNetCore.Identity;

namespace ELDEL_API.Services
{
    public interface IAccountService
    {
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Account> GetAccountByAccountIdAsync(string accountUniqueId);
        Task<Account> GetAccountWithAddressByAccountIdAsync(string accountUniqueId);

        Task<IdentityResult> AddAccountAsync(AccountCredentialDTO accountCredentialDTO);
        Task<Account> UpdateAccountInfoAsync(Account account, AccountDTO accountDTO, string updatedByEmail);
        Task<IdentityResult> DeleteAccountAsync(Account account);

        Task<SignInResult> CheckPasswordSignInAsync(Account account, string password, bool signInResult);
    }
}
