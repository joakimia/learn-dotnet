using System.Text.Json.Serialization;

namespace ELDEL_API.Models
{
    public class ValidatedDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }
    }
}
