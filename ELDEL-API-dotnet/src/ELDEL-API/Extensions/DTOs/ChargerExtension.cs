using ELDEL_API.DoubleExtensions;
using ELDEL_API.DTOs;
using ELDEL_API.StringExtensions;

namespace ELDEL_API.ChargerExtensions
{
    public static class ChargerExtensions
    {
        public static ChargerDTO ToValidatedChargerDTO(this ChargerDTO chargerDTO)
        {
            if (!string.IsNullOrEmpty(chargerDTO.Id) && !chargerDTO.Id.IsValidUniqueId())
            {
                chargerDTO.Message = $"Invalid property 'id' (string). You provided: {chargerDTO.Id}.";
                return chargerDTO;
            }

            if (chargerDTO.Latitude != null && !chargerDTO.Latitude.IsValidLatitude())
            {
                chargerDTO.Message = $"Invalid property 'latitude' (double). It must be a number between -90 and 90. You provided: {chargerDTO.Latitude}.";
                return chargerDTO;
            }

            if (chargerDTO.Longitude != null && !chargerDTO.Longitude.IsValidLongitude())
            {
                chargerDTO.Message = $"Invalid property 'longitude' (string). It must be a number between -180 and 180. You provided: {chargerDTO.Longitude}.";
                return chargerDTO;
            }

            // TODO: Validate socket type and manufacturer type

            return chargerDTO;
        }
    }
}
