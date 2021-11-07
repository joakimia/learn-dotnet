using System.Threading.Tasks;
using ELDEL_API.DTOs;
using ELDEL_EntityFramework_Library.Models;

namespace ELDEL_API.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressByAddressIdAsync(string addressUniqueId);
        Task<Address> GetAddressByAccountIdAsync(string accountUniqueId);
        Task<Address> GetAddressByChargerIdAsync(string chargerUniqueId);

        Task<Account> AddAccountAddressAsync(Account account, AddressDTO addressDTO, string createdByEmail);
        Task<Account> UpdateAccountAddressAsync(Account account, AddressDTO addressDTO, string updatedByEmail);
        Task<Account> DeleteAccountAddressAsync(Account account, string updatedByEmail);

        Task<Charger> AddChargerAddressAsync(Charger charger, AddressDTO addressDTO, string createdByEmail);
        Task<Charger> UpdateChargerAddressAsync(Charger charger, AddressDTO adresssDTO, string updatedByEmail);
        Task<Charger> DeleteChargerAddressAsync(Charger charger, string updatedByEmail);
    }
}
