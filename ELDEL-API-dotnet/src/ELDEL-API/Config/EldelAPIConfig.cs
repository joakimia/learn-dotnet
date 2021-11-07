namespace ELDEL_API.Config
{
    public class EldelAPIConfig : IEldelAPIConfig
    {
        public string ChargersAPIKey { get; set; }
    }

    public interface IEldelAPIConfig
    {
        string ChargersAPIKey { get; }
    }
}
