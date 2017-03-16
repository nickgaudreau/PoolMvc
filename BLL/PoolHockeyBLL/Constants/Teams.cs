using System.Collections.Generic;

namespace PoolHockeyBLL.Constants
{
    public static class Teams
    {
        public static readonly string[] Playoff =
        {
            "STL", "MIN", "NSH", "CHI",
            "ANA", "TBL", "DET", "NYR",
            "PIT", "WSH", "NYI", "DAL",
            "FLA", "PHI", "SJS", "LAK"

        };

        /// <summary>
        /// New MysposrtsFeeds api. WPG => WPJ, and FLA => FLO
        /// </summary>
        public static readonly string[] Season =
        {
            "STL", "MIN", "NSH", "CHI",
            "ANA", "WPJ", "VAN", "CGY",
            "MTL", "OTT", "TBL", "DET",
            "NYR", "PIT", "WSH", "NYI",
            "ARI", "BUF", "CAR", "COL",
            "CBJ", "DAL", "EDM", "FLO",
            "NJD", "PHI", "SJS", "TOR",
            "BOS", "LAK"
        };

        public static readonly Dictionary<int, string> SeasonWebScrappingDict = new Dictionary<int, string>()
        {
            {1,"COL" },
            {2,"DET" },
            {3,"BOS" },
            {4,"NYI" },
            {5,"CAR" },
            {6,"NYR" },
            {7,"PIT" },
            {8,"TOR" },
            {9,"OTT" },
            {10,"BUF" },
            {11,"MTL" },
            {12,"PHI" },
            {13,"NJD" },
            {14,"FLA" },
            {15,"WSH" },
            {16,"STL" },
            {17,"TBL" },
            {18,"CGY" },
            {19,"ARI" },
            {20,"DAL" },
            {21,"SJS" },
            {22,"CHI" },
            {23,"LAK" },
            {24,"EDM" },
            {25,"ANA" },
            {26,"VAN" },
            {27,"NSH" },
            {28,"WPG" },
            {29,"MIN" },
            {30,"CBJ" }
        };
    }
}
