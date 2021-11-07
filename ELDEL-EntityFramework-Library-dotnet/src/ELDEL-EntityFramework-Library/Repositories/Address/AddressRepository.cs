using ELDEL_EntityFramework_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly EldelContext _context;

        public AddressRepository(EldelContext context)
        {
            _context = context;
        }

        public async Task<Address> GetAddressByAddressIdAsync(string addressUniqueId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(ad => ad.UniqueId == addressUniqueId);
        }

        public async Task<Address> GetAddressByAccountIdAsync(string accountUniqueId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(ad => ad.AccountUniqueId == accountUniqueId);
        }

        public async Task<Address> GetAddressByChargerIdAsync(string chargerUniqueId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(ad => ad.ChargerUniqueId == chargerUniqueId);
        }

        public async Task<Address> AddAccountAddressAsync(
            Account account,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string createdByEmail
        )
        {
            var address = new Address(
                account,
                primaryStreetName,
                secondaryStreetName,
                zipCode,
                city,
                province,
                country,
                countryCode,
                createdByEmail
            );
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<Address> AddChargerAddressAsync(
            Charger charger,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string createdByEmail
        )
        {
            var address = new Address(
                charger,
                primaryStreetName,
                secondaryStreetName,
                zipCode,
                city,
                province,
                country,
                countryCode,
                createdByEmail
            );
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<Address> UpdateAddressAsync(
            Address address,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string updatedByEmail
        )
        {
            if (address == null)
            {
                throw new KeyNotFoundException($"Can not update non-existing address (address == null). Update attempted by email: {updatedByEmail}.");
            }

            address.PrimaryStreetName = primaryStreetName ?? address.PrimaryStreetName;
            address.SecondaryStreetName = secondaryStreetName ?? address.SecondaryStreetName;
            address.ZipCode = zipCode ?? address.ZipCode;
            address.City = city ?? address.City;
            address.Province = province ?? address.Province;
            address.Country = country ?? address.Country;
            address.CountryCode = countryCode ?? address.CountryCode;
            address.UpdatedByEmail = updatedByEmail;
            address.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                address = await GetAddressByAddressIdAsync(address.UniqueId);
            }

            return address == null;
        }

        public async Task<Account> UpdateAccountAddressAsync(Account account, Address address, string updatedByEmail)
        {
            if (account == null)
            {
                throw new KeyNotFoundException($"Can not update address to a non-existing account (account == null). Update attempted by email: {updatedByEmail}.");
            }

            account.Address = address ?? throw new InvalidOperationException($"Unable to update non-existing address (address == null) to account with accountUniqueId: {account.UniqueId}. Update attempted by email: {updatedByEmail}.");
            account.AddressRowId = address.RowId;
            account.AddressUniqueId = address.UniqueId;
            account.UpdatedDate = DateTime.UtcNow;
            account.UpdatedByEmail = updatedByEmail;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account> DeleteAccountAddressAsync(Account account, string updatedByEmail)
        {
            if (account == null)
            {
                throw new KeyNotFoundException($"Can not update address to a non-existing account (account == null). Update attempted by email: {updatedByEmail}.");
            }

            if (account.Address != null)
            {
                account.Address = null;
                account.AddressRowId = null;
                account.AddressUniqueId = null;
                account.UpdatedDate = DateTime.UtcNow;
                account.UpdatedByEmail = updatedByEmail;
                await _context.SaveChangesAsync();
            }

            return account;
        }

        public async Task<Charger> UpdateChargerAddressAsync(Charger charger, Address address, string updatedByEmail)
        {
            if (charger == null)
            {
                throw new KeyNotFoundException($"Can not update address to a non-existing charger (charger == null). Update attempted by email: {updatedByEmail}.");
            }

            charger.Address = address ?? throw new InvalidOperationException($"Unable to update non-existing address (address == null) to charger with chargerUniqueId: {charger.UniqueId}. Update attempted by email: {updatedByEmail}.");
            charger.AddressRowId = address.RowId;
            charger.AddressUniqueId = address.UniqueId;
            charger.UpdatedDate = DateTime.UtcNow;
            charger.UpdatedByEmail = updatedByEmail;
            await _context.SaveChangesAsync();

            return charger;
        }

        public async Task<Charger> DeleteChargerAddressAsync(Charger charger, string updatedByEmail)
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
