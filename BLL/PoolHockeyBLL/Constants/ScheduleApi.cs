namespace PoolHockeyBLL.Constants
{
    public static class ScheduleApi
    {
        public static readonly string UrlPrefix = "http://nhlwc.cdnak.neulion.com/fs1/nhl/league/clubschedule/";

        public static readonly string UrlSuffix = "/iphone/clubschedule.json";

        public static readonly string[] Season1StQMonths =
        {
            "09", "10"
        };

        public static readonly string[] Season2NdQMonths =
        {
            "11", "12"
        };

        public static readonly string[] Season3RdQMonths =
        {
            "01", "02", "03", "04"
        };

        public static readonly string[] Season4ThQMonths =
        {
            "01", "02", "03", "04"
        };

        public static readonly string[] PlayoffMonths =
        {
            "04", "05", "06"
        };

        public static readonly string Year16 = "2016";

        public static readonly string Year17 = "2017";

    }
}
