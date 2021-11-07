using ELDEL_EntityFramework_Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly EldelContext _context;

        public AccountRepository(EldelContext context, UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Account> GetAccountByAccountIdAsync(string accountUniqueId)
        {
            return await _context.Accounts.SingleOrDefaultAsync(u => u.UniqueId == accountUniqueId);
        }

        public async Task<Account> GetAccountWithAddressByIdAsync(string accountUniqueId)
        {
            return await _context.Accounts
                .Include(ac => ac.Address)
                .SingleOrDefaultAsync(u => u.UniqueId == accountUniqueId);
        }

        public async Task<IdentityResult> AddAccountAsync(string email, string password)
        {
            var account = await GetAccountByEmailAsync(email);
            if (account != null)
            {
                throw new InvalidOperationException($"Unable to add another account with email: {email}, (account != null). Account already exist.");
            }

            account = new Account(email);

            return await _userManager.CreateAsync(account, password);
        }

        public async Task<Account> UpdateAccountInfoAsync(
            Account account,
            string firstName,
            string lastName,
            string email,
            string phoneNumberPrefix,
            string phoneNumber,
            string updatedByEmail
        )
        {
            if (account == null)
            {
                throw new KeyNotFoundException($"Can not update non-existing account (account == null). Account does not exist. Update attempted by email: {email}.");
            }

            account.FirstName = firstName ?? account.FirstName;
            account.LastName = lastName ?? account.LastName;
            account.FullName = string.IsNullOrEmpty(lastName) ? account.FirstName : $"{account.FirstName} {account.LastName}";
            account.PhoneNumberPrefix = phoneNumberPrefix ?? account.PhoneNumberPrefix;
            account.PhoneNumber = phoneNumber ?? account.PhoneNumber;
            account.UpdatedDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(account);

            return account;
        }

        public async Task<IdentityResult> DeleteAccountAsync(Account account)
        {
            return await _userManager.DeleteAsync(account);
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(Account account, string password, bool signInResult)
        {
            return await _signInManager.CheckPasswordSignInAsync(account, password, signInResult);
        }
    }
}
