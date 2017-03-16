namespace PoolHockeyBOL.JsonModels
{
    /// <summary>
    /// Collected form api. http://nhlwc.cdnak.neulion.com/fs1/nhl/league/clubschedule/TEAM/2016/MONTH_IN_NUMBER/iphone/clubschedule.json
    /// </summary>

    public class TeamScheduleApi
    {
        public string timestamp { get; set; }
        public Game[] games { get; set; }
    }

    public class Game
    {
        public string caTvNationalURL { get; set; }
        public string startTime { get; set; }
        public string abb { get; set; }
        public string cPeriod { get; set; }
        public int gameId { get; set; }
        public Hg hg { get; set; }
        public string loc { get; set; }
        public int gs { get; set; }
        public string usTvLocalMsg { get; set; }
        public string caTvLocalMsg { get; set; }
    }

    public class Hg
    {
        public string caLogo { get; set; }
        public bool verizon { get; set; }
        public string usLogo { get; set; }
    }


}