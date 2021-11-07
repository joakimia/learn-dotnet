using ELDEL_EntityFramework_Library.Models;
using System.Threading.Tasks;

namespace ELDEL_EntityFramework_Library.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByAddressIdAsync(string addressUniqueId);
        Task<Address> GetAddressByAccountIdAsync(string accountUniqueId);
        Task<Address> GetAddressByChargerIdAsync(string chargerUniqueId);

        Task<Address> UpdateAddressAsync(
            Address address,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string updatedByEmail
        );

        Task<Address> AddAccountAddressAsync(
            Account account,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string createdByEmail
        );
        Task<Account> UpdateAccountAddressAsync(Account account, Address address, string updatedByEmail);
        Task<Account> DeleteAccountAddressAsync(Account account, string updatedByEmail);

        Task<Address> AddChargerAddressAsync(
            Charger charger,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string createdByEmail
        );
        Task<Charger> UpdateChargerAddressAsync(Charger charger, Address Address, string updatedByEmail);
        Task<Charger> DeleteChargerAddressAsync(Charger charger, string updatedByEmail);
    }
}
