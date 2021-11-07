using ELDEL_API.Models;

namespace ELDEL_API.DTOs
{
    public class ChargerDetailsDTO : ValidatedDTO
    {
        public string Id { get; set; }
        public string ChargerId { get; set; }
        public string Description { get; set; }
    }
}
