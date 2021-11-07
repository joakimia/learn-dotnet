using System;
using System.Text.Json.Serialization;
using ELDEL_API.Models;
using ELDEL_EntityFramework_Library.Models;

namespace ELDEL_API.DTOs
{
    public class ChargerDTO : ValidatedDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string SocketType { get; set; }
        public string ManufacturerType { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AddressDTO Address { get; set; }

        internal static ChargerDTO Map(Charger charger)
        {
            return new ChargerDTO
            {
                Id = charger.UniqueId,
                Name = charger.Name,
                Longitude = charger.Longitude,
                Latitude = charger.Latitude,
                SocketType = charger.SocketType,
                ManufacturerType = charger.ManufacturerType,
                Address = charger.Address == null ? null : AddressDTO.Map(charger.Address),
            };
        }
    }
}
