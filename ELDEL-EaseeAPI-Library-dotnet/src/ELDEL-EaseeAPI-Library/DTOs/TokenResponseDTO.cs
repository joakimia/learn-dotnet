using System.Collections.Generic;

namespace ELDEL_EaseeAPI_Library.DTOs
{
    public class EaseeAPITokenResponseDTO
    {
        public IEnumerable<string> AccessClaims { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}
