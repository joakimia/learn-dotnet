using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;

namespace ELDEL_API.Services
{
    public interface IChargersService
    {
        Task<Charger> GetChargerByChargerIdAsync(string chargerUniqueId);
        Task<Charger> GetChargerWithAddressByChargerIdAsync(string chargerUniqueId);
        Task<Charger> GetChargerWithAccountsAndAddressByChargerIdAsync(string chargerUniqueId);
        Task<IEnumerable<Charger>> GetAllChargersAsync();
        Task<IEnumerable<Charger>> GetChargersByAccountIdAsync(string accountUniqueId);

        Task<Charger> AddChargerAsync(ChargerDTO chargerDTO, AccountDTO accountDTO);
        Task<Charger> UpdateChargerAsync(Charger charger, ChargerDTO chargerDTO, AccountDTO accountDTO);
        Task<bool> DeleteChargerAsync(Charger charger);

        Task<Charger> AddAccountToChargerAsync(string chargerUniqueId, string accountUniqueId, string updatedByEmail);
        Task<Charger> RemoveAccountFromChargerAsync(string chargerUniqueId, string accountUniqueId, string updatedByEmail);
    }
}
