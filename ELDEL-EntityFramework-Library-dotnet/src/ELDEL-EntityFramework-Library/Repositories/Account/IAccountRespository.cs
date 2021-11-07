using ELDEL_EntityFramework_Library.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Account> GetAccountByAccountIdAsync(string accountUniqueId);
        Task<Account> GetAccountWithAddressByIdAsync(string accountUniqueId);

        Task<IdentityResult> AddAccountAsync(string email, string password);
        Task<Account> UpdateAccountInfoAsync(
            Account account,
            string firstName,
            string lastName,
            string email,
            string phoneNumberPrefix,
            string phoneNumber,
            string updatedByEmail
        );
        Task<IdentityResult> DeleteAccountAsync(Account account);

        Task<SignInResult> CheckPasswordSignInAsync(Account account, string password, bool signInResult);
    }
}
