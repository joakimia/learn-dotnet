namespace ELDEL_API.DoubleExtensions
{
    public static class DoubleExtensions
    {
        public static bool IsValidLatitude(this double? latitude)
        {
            try
            {
                const int LATITUDE_MIN_RANGE = -90;
                const int LATITUDE_MAX_RANGE = 90;

                if (latitude < LATITUDE_MIN_RANGE)
                {
                    return false;
                }
                if (latitude > LATITUDE_MAX_RANGE)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidLongitude(this double? longitude)
        {
            try
            {
                const int LONGITUDE_MIN_RANGE = -180;
                const int LONGITUDE_MAX_RANGE = 180;

                if (longitude < LONGITUDE_MIN_RANGE)
                {
                    return false;
                }
                if (longitude > LONGITUDE_MAX_RANGE)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
