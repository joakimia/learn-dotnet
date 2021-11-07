using System;

namespace ELDEL_API.Config
{
    public class EldelAuthenticationConfig : IEldelAuthenticationConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }

    public interface IEldelAuthenticationConfig
    {
        string Secret { get; }
        string Issuer { get; }
        string Audience { get; }
        TimeSpan TokenLifetime { get; }
    }
}
