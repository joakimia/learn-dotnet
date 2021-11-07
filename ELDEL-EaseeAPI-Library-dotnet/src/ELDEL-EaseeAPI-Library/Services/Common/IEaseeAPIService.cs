using System.Collections.Generic;
using System.Threading.Tasks;
using ELDEL_EaseeAPI_Library.DTOs;

namespace ELDEL_EaseeAPI_Library.Services
{
    public interface IEaseeAPIService
    {
        Task<IEnumerable<EaseeChargerResponseDTO>> GetChargersAsync();
        Task<EaseeChargerResponseDTO> AddChargerAsync(string serialNo, string pinCode);
    }
}
