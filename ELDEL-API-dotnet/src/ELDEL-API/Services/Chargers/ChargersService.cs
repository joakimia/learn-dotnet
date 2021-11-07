using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;
using ELDEL_EntityFramework_Library.Repositories;
using Microsoft.Extensions.Logging;

namespace ELDEL_API.Services
{
    public class ChargersService : IChargersService
    {
        private readonly ILogger<ChargersService> _logger;
        private readonly IChargerRepository _chargerRepository;
        private readonly IAccountRepository _accountRepository;

        public ChargersService(
            ILogger<ChargersService> logger,
            IChargerRepository chargerRepository,
            IAccountRepository accountRepository
        )
        {
            _logger = logger;
            _chargerRepository = chargerRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Charger> GetChargerByChargerIdAsync(string chargerUniqueId)
        {
            return await _chargerRepository.GetChargerByChargerIdAsync(chargerUniqueId);
        }

        public async Task<IEnumerable<Charger>> GetAllChargersAsync()
        {
            return await _chargerRepository.GetAllChargersAsync();
        }

        public async Task<IEnumerable<Charger>> GetChargersByAccountIdAsync(string accountUniqueId)
        {
            return await _chargerRepository.GetChargersByAccountIdAsync(accountUniqueId);
        }

        public async Task<Charger> GetChargerWithAddressByChargerIdAsync(string chargerUniqueId)
        {
            return await _chargerRepository.GetChargerWithAddressByChargerIdAsync(chargerUniqueId);
        }

        public async Task<Charger> GetChargerWithAccountsAndAddressByChargerIdAsync(string chargerUniqueId)
        {
            return await _chargerRepository.GetChargerWithAccountsAndAddressByChargerIdAsync(chargerUniqueId);
        }

        public async Task<Charger> AddChargerAsync(ChargerDTO chargerDTO, AccountDTO accountDTO)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(ChargersService),
                ["function"] = nameof(AddChargerAsync),
                ["email"] = accountDTO.Email,
                ["chargerUniqueId"] = chargerDTO.Id,
                ["accountUniqueId"] = accountDTO.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var account = await _accountRepository.GetAccountByAccountIdAsync(accountDTO.Id);
                if (account == null)
                {
                    _logger.LogInformation($"Add charger failed: Unable to add new charger with an empty account (account == null).");
                    return null;
                }

                return await _chargerRepository.AddChargerAsync(chargerDTO.Name, account, accountDTO.Email);
            }
        }

        public async Task<Charger> UpdateChargerAsync(Charger charger, ChargerDTO chargerDTO, AccountDTO accountDTO)
        {
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(ChargersService),
                ["function"] = nameof(UpdateChargerAsync),
                ["email"] = accountDTO.Email,
                ["chargerUniqueId"] = chargerDTO.Id,
                ["accountUniqueId"] = accountDTO.Id,
            };

            using (_logger.BeginScope(loggerScope))
            {
                if (charger == null)
                {
                    _logger.LogInformation($"Update charger failed: Unable to update non-existing charger (charger == null).");
                    return null;
                }

                return await _chargerRepository.UpdateChargerAsync(
                    charger,
                    chargerDTO.Name,
                    chargerDTO.Longitude,
                    chargerDTO.Latitude,
                    chargerDTO.SocketType,
                    chargerDTO.ManufacturerType,
                    accountDTO.Email
                );
            }
        }

        public async Task<bool> DeleteChargerAsync(Charger charger)
        {
            return await _chargerRepository.DeleteChargerAsync(charger);
        }

        public async Task<Charger> AddAccountToChargerAsync(string chargerUniqueId, string accountUniqueId, string updatedByEmail)
        {
            // TODO: Remove relational link between charger -> account as well (currently we only delete the account) - preferably with Entity framework settings/configuration.
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(ChargersService),
                ["function"] = nameof(AddAccountToChargerAsync),
                ["email"] = updatedByEmail,
                ["chargerUniqueId"] = chargerUniqueId,
                ["accountUniqueId"] = accountUniqueId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var charger = await _chargerRepository.GetChargerByChargerIdAsync(chargerUniqueId);
                var account = await _accountRepository.GetAccountByAccountIdAsync(accountUniqueId);
                var isChargerAlreadyAdded = account.Chargers.Contains(charger);
                if (isChargerAlreadyAdded)
                {
                    _logger.LogInformation($"Add account to charger failed: Account is already added to charger.");
                    return charger;
                }
                else
                {
                    return await _chargerRepository.AddAccountToChargerAsync(charger, account, updatedByEmail);
                }
            }
        }

        public async Task<Charger> RemoveAccountFromChargerAsync(string chargerUniqueId, string accountUniqueId, string updatedByEmail)
        {
            // TODO: Remove relational link between charger -> account as well (currently we only delete the account) - preferably with Entity framework settings/configuration.
            var loggerScope = new Dictionary<string, object>
            {
                ["service"] = nameof(ChargersService),
                ["function"] = nameof(RemoveAccountFromChargerAsync),
                ["email"] = updatedByEmail,
                ["chargerUniqueId"] = chargerUniqueId,
                ["accountUniqueId"] = accountUniqueId,
            };

            using (_logger.BeginScope(loggerScope))
            {
                var charger = await _chargerRepository.GetChargerByChargerIdAsync(chargerUniqueId);
                var account = await _accountRepository.GetAccountByAccountIdAsync(accountUniqueId);
                if (charger == null)
                {
                    _logger.LogInformation($"Remove account from charger failed: Unable to remove account from non-existing charger (charger == null).");
                    return null;
                }

                if (account == null)
                {
                    _logger.LogInformation($"Remove account from charger failed: Unable to remove account from non-existing account (account == null).");
                    return null;
                }

                var isChargerAlreadyRemoved = !account.Chargers.Contains(charger) && charger.Accounts.Contains(account);
                if (isChargerAlreadyRemoved)
                {
                    _logger.LogInformation($"Remove account from charger failed: Charger is already removed.");
                    return charger;
                }
                else
                {
                    return await _chargerRepository.RemoveAccountFromChargerAsync(charger, account, updatedByEmail);
                }
            }
        }
    }
}
