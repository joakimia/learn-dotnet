using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;
using ELDEL_EntityFramework_Library.Repositories;
using Microsoft.Extensions.Logging;

namespace ELDEL_API.Services
{
    public class AddressService : IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly IAddressRepository _addressRepository;
        private readonly IChargerRepository _chargerRepository;

        public AddressService(
            ILogger<AddressService> logger,
            IAddressRepository addressRepository,
            IChargerRepository chargerRepository
        )
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _chargerRepository = chargerRepository;
        }

        public async Task<Address> GetAddressByAddressIdAsync(string addressUniqueId)
        {
            return await _addressRepository.GetAddressByAddressIdAsync(addressUniqueId);
        }

        public async Task<Address> GetAddressByAccountIdAsync(string accountId)
        {
            return await _addressRepository.GetAddressByAccountIdAsync(accountId);
        }

        public async Task<Address> GetAddressByChargerIdAsync(string chargerUniqueId)
        {
            var charger = await _chargerRepository.GetChargerByChargerIdAsync(chargerUniqueId);
            return charger?.Address;
        }

        public async Task<Account> AddAccountAddressAsync(Account account, AddressDTO addressDTO, string createdByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(AddAccountAddressAsync),
                ["email"] = createdByEmail,
                ["adddressUniqueId"] = addressDTO.Id,
                ["accountUniqueId"] = account.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var address = await _addressRepository.GetAddressByAddressIdAsync(addressDTO.Id);
                if (account == null)
                {
                    _logger.LogInformation($"Add account's address failed: Unable to add address to non-existing account (account == null).");
                    return null;
                }

                if (address == null)
                {
                    _logger.LogInformation($"No address was provided, adding new address.");
                    address = await _addressRepository.AddAccountAddressAsync(
                        account,
                        addressDTO.PrimaryStreetName,
                        addressDTO.SecondaryStreetName,
                        addressDTO.ZipCode,
                        addressDTO.City,
                        addressDTO.Province,
                        addressDTO.Country,
                        addressDTO.CountryCode,
                        createdByEmail
                    );
                    account = await _addressRepository.UpdateAccountAddressAsync(account, address, createdByEmail);
                    _logger.LogInformation($"Updated account's address with newly added address.");
                }
                else
                {
                    account = await UpdateAccountAddressAsync(account, addressDTO, createdByEmail);
                    _logger.LogInformation($"Updated account's address with existing address.");
                }

                return account;
            }
        }

        public async Task<Account> UpdateAccountAddressAsync(Account account, AddressDTO addressDTO, string updatedByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(UpdateAccountAddressAsync),
                ["email"] = updatedByEmail,
                ["adddressUniqueId"] = addressDTO.Id,
                ["accountUniqueId"] = account.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var address = await _addressRepository.GetAddressByAddressIdAsync(addressDTO.Id);
                if (account == null)
                {
                    _logger.LogInformation($"Update account's address failed: Unable to update non-existing account (account == null).");
                    return null;
                }

                if (address == null)
                {
                    _logger.LogInformation($"Update account's address failed: Unable to update account with non-existing address (address == null).");
                    return null;
                }

                address = await _addressRepository.UpdateAddressAsync(
                    address,
                    addressDTO.PrimaryStreetName,
                    addressDTO.SecondaryStreetName,
                    addressDTO.ZipCode,
                    addressDTO.City,
                    addressDTO.Province,
                    addressDTO.Country,
                    addressDTO.CountryCode,
                    updatedByEmail
                );

                if (account.AddressUniqueId != address.UniqueId)
                {
                    account = await _addressRepository.UpdateAccountAddressAsync(account, address, updatedByEmail);
                }

                return account;
            }
        }

        public async Task<Account> DeleteAccountAddressAsync(Account account, string updatedByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(DeleteAccountAddressAsync),
                ["email"] = updatedByEmail,
                ["accountUniqueId"] = account.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (account == null)
                {
                    _logger.LogInformation($"Delete account's address failed: Unable to delete address for a non-existing account (account == null).");
                    return null;
                }

                var isAddressAlreadyRemoved = account.Address == null;
                if (isAddressAlreadyRemoved)
                {
                    _logger.LogInformation($"Delete account's address failed: Address is already removed.");
                    return account;
                }
                else
                {
                    account = await _addressRepository.DeleteAccountAddressAsync(account, updatedByEmail);
                }

                return account;
            }
        }

        public async Task<Charger> AddChargerAddressAsync(Charger charger, AddressDTO addressDTO, string createdByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(AddChargerAddressAsync),
                ["email"] = createdByEmail,
                ["chargerUniqueId"] = charger.UniqueId,
                ["addressUniqueId"] = addressDTO.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var address = await _addressRepository.GetAddressByAddressIdAsync(addressDTO.Id);

                if (charger == null)
                {
                    _logger.LogInformation($"Add charger's address failed: Unable to add to non-existing charger (charger == null).");
                    return null;
                }

                if (address == null)
                {
                    address = await _addressRepository.AddChargerAddressAsync(
                        charger,
                        addressDTO.PrimaryStreetName,
                        addressDTO.SecondaryStreetName,
                        addressDTO.ZipCode,
                        addressDTO.City,
                        addressDTO.Province,
                        addressDTO.Country,
                        addressDTO.CountryCode,
                        createdByEmail
                    );
                    charger = await _addressRepository.UpdateChargerAddressAsync(charger, address, createdByEmail);
                }
                else
                {
                    charger = await UpdateChargerAddressAsync(charger, addressDTO, createdByEmail);
                }

                return charger;
            }
        }

        public async Task<Charger> UpdateChargerAddressAsync(Charger charger, AddressDTO addressDTO, string updatedByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(UpdateChargerAddressAsync),
                ["email"] = updatedByEmail,
                ["chargerUniqueId"] = charger.UniqueId,
                ["addressUniqueId"] = addressDTO.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (charger == null)
                {
                    _logger.LogInformation($"Update charger's address failed: Unable to update non-existing charger (charger == null).");
                    return null;
                }

                var address = await _addressRepository.GetAddressByAddressIdAsync(addressDTO.Id);
                if (address == null)
                {
                    _logger.LogInformation($"Update charger's address failed: Unable to update charger with non-existing address (address == null).");
                    return charger;
                }

                address = await _addressRepository.UpdateAddressAsync(
                    address,
                    addressDTO.PrimaryStreetName,
                    addressDTO.SecondaryStreetName,
                    addressDTO.ZipCode,
                    addressDTO.City,
                    addressDTO.Province,
                    addressDTO.Country,
                    addressDTO.CountryCode,
                    updatedByEmail
                );

                return await _addressRepository.UpdateChargerAddressAsync(charger, address, updatedByEmail);
            }
        }

        public async Task<Charger> DeleteChargerAddressAsync(Charger charger, string updatedByEmail)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(AddressService),
                ["function"] = nameof(DeleteChargerAddressAsync),
                ["email"] = updatedByEmail,
                ["chargerUniqueId"] = charger.UniqueId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (charger == null)
                {
                    _logger.LogInformation($"Delete charger's address failed: Unable to delete from non-existing charger (charger == null)");
                    return null;
                }

                var isAddressAlreadyRemoved = charger.Address == null;
                if (isAddressAlreadyRemoved)
                {
                    _logger.LogInformation($"Delete charger's address failed: Address is already removed.");
                    return charger;
                }
                else
                {
                    return await _addressRepository.DeleteChargerAddressAsync(charger, updatedByEmail);
                }
            }
        }
    }
}
