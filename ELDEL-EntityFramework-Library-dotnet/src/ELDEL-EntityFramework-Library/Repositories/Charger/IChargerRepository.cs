using ELDEL_EntityFramework_Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public interface IChargerRepository
    {
        Task<Charger> GetChargerByChargerIdAsync(string chargerUniqueId);
        Task<Charger> GetChargerWithAddressByChargerIdAsync(string chargerUniqueId);
        Task<Charger> GetChargerWithAccountsAndAddressByChargerIdAsync(string chargerUniqueId);
        Task<IEnumerable<Charger>> GetAllChargersAsync();
        Task<IEnumerable<Charger>> GetChargersByAccountIdAsync(string accountUniqueId);

        Task<Charger> AddChargerAsync(string name, Account account, string createdByEmail);
        Task<Charger> UpdateChargerAsync(
            Charger charger,
            string name,
            double? longitude,
            double? latitude,
            string socketType,
            string manufacturerType,
            string updatedByEmail
        );
        Task<bool> DeleteChargerAsync(Charger charger);

        Task<Charger> AddAccountToChargerAsync(Charger charger, Account account, string updatedByEmail);
        Task<Charger> RemoveAccountFromChargerAsync(Charger charger, Account account, string updatedByEmail);
    }
}
