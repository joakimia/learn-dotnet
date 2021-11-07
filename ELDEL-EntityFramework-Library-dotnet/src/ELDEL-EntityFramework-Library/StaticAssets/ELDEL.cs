using shortid;
using shortid.Configuration;

namespace ELDEL_EntityFramework_Library.StaticAssets
{
    public partial class ELDEL
    {
        public static partial class UniqueId
        {
            public static int UniqueIdLength = 15;

            public static string GenerateShortUniqueId()
            {
                var shortIdOptions = new GenerationOptions
                {
                    Length = UniqueIdLength,
                    UseSpecialCharacters = false,
                    UseNumbers = true,
                };
                return ShortId.Generate(shortIdOptions);
            }
        }

        public partial class Coordinates
        {
            public const int LatitudeMinValue = -90;
            public static int LatitudeMaxValue = 90;

            public static int LongitudeMinValue = -180;
            public static int LongitudeMaxValue = 180;
        }

        public static partial class Phone
        {
            public static int PhoneNumberPrefixMaxLength = 3;
            public static int PhoneNumberMaxLength = 15;
        }
    }
}
