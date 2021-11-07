using ELDEL_EntityFramework_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public class ChargerRepository : IChargerRepository
    {
        private readonly EldelContext _context;

        public ChargerRepository(EldelContext context)
        {
            _context = context;
        }

        public async Task<Charger> GetChargerByChargerIdAsync(string chargerUniqueId)
        {
            return await _context.Chargers.SingleOrDefaultAsync(ch => ch.UniqueId == chargerUniqueId);
        }

        public async Task<Charger> GetChargerWithAddressByChargerIdAsync(string chargerUniqueId)
        {
            return await _context.Chargers
                .Include(ch => ch.Address)
                .SingleOrDefaultAsync(ch => ch.UniqueId == chargerUniqueId);
        }

        public async Task<Charger> GetChargerWithAccountsAndAddressByChargerIdAsync(string chargerUniqueId)
        {
            return await _context.Chargers
                .Include(ch => ch.Address)
                .Include(ch => ch.Accounts)
                .SingleOrDefaultAsync(ch => ch.UniqueId == chargerUniqueId);
        }

        public async Task<IEnumerable<Charger>> GetAllChargersAsync()
        {
            return await _context.Chargers.ToListAsync();
        }

        public async Task<IEnumerable<Charger>> GetChargersByAccountIdAsync(string accountUniqueId)
        {
            var chargers = await _context.Chargers
                .Include(ch => ch.Accounts.Where(ac => ac.UniqueId == accountUniqueId))
                .ToListAsync();

            return chargers;
        }

        public async Task<Charger> AddChargerAsync(string name, Account account, string createdByEmail)
        {
            if (account == null)
            {
                throw new InvalidOperationException($"Unable to add new charger to a non-existing account (account == null). Update attempted by email: {createdByEmail}.");
            }

            var charger = new Charger(name, account, createdByEmail);
            _context.Chargers.Add(charger);
            await _context.SaveChangesAsync();

            return charger;
        }

        public async Task<Charger> UpdateChargerAsync(
            Charger charger,
            string name,
            double? longitude,
            double? latitude,
            string socketType,
            string manufacturerType,
            string updatedByEmail
        )
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not update a non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            charger.Name = name ?? charger.Name;
            charger.Longitude = longitude ?? charger.Longitude;
            charger.Latitude = latitude ?? charger.Latitude;
            charger.SocketType = socketType ?? charger.SocketType;
            charger.ManufacturerType = manufacturerType ?? charger.ManufacturerType;
            charger.UpdatedByEmail = updatedByEmail;
            charger.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return charger;
        }

        public async Task<bool> DeleteChargerAsync(Charger charger)
        {
            if (charger != null)
            {
                _context.Chargers.Remove(charger);
                await _context.SaveChangesAsync();
                charger = await GetChargerByChargerIdAsync(charger.UniqueId);
            }

            return charger == null;
        }

        public async Task<Charger> AddAccountToChargerAsync(Charger charger, Account account, string updatedByEmail)
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not add account to a non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            if (account == null)
            {
                throw new InvalidOperationException($"Unable to add non-existing account (account == null) to charger with chargerUniqueId: {charger.UniqueId}. Update attempted by email: {updatedByEmail}.");
            }

            charger.Accounts.Add(account);
            charger.UpdatedDate = DateTime.UtcNow;
            charger.UpdatedByEmail = updatedByEmail;
            await _context.SaveChangesAsync();

            return charger;
        }

        public async Task<Charger> RemoveAccountFromChargerAsync(Charger charger, Account account, string updatedByEmail)
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not remove account from a non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            if (account == null)
            {
                throw new InvalidOperationException($"Unable to remove non-existing account (account == null) from charger with chargerUniqueId: {charger.UniqueId}. Update attempted by email: {updatedByEmail}.");
            }

            account = charger.Accounts.SingleOrDefault(ac => ac.UniqueId == account.UniqueId);

            if (account != null)
            {
                charger.Accounts.Remove(account);
                charger.UpdatedDate = DateTime.UtcNow;
                charger.UpdatedByEmail = updatedByEmail;
                await _context.SaveChangesAsync();
            }

            return charger;
        }

        public async Task<Charger> UpdateAddressToChargerAsync(Charger charger, Address address, string updatedByEmail)
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not update address to non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            charger.Address = address ?? throw new InvalidOperationException($"Unable to update non-existing address (address == null) to charger with chargerUniqueId: {charger.UniqueId}. Update attempted by email: {updatedByEmail}.");
            charger.AddressRowId = address.RowId;
            charger.AddressUniqueId = address.UniqueId;
            charger.UpdatedDate = DateTime.UtcNow;
            charger.UpdatedByEmail = updatedByEmail;
            await _context.SaveChangesAsync();

            return charger;
        }

        public async Task<Charger> RemoveAddressFromChargerAsync(Charger charger, string updatedByEmail)
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not remove address from non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            if (charger.Address != null)
            {
                charger.Address = null;
                charger.AddressRowId = null;
                charger.AddressUniqueId = null;
                charger.UpdatedDate = DateTime.UtcNow;
                charger.UpdatedByEmail = updatedByEmail;
                await _context.SaveChangesAsync();
            }

            return charger;
        }
    }
}
