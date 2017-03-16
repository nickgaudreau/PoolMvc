namespace PoolHockeyBOL.JsonModels
{
    /// <summary>
    /// Json Logic
    /// </summary>
    public class NhlTeamPlayerInfoApi
    {

        public int skaterHighlight { get; set; }
        public string timestamp { get; set; }
        public Skaterdata[] skaterData { get; set; }
        public string skaterCategories { get; set; }
        public int goalieHighlight { get; set; }
        public Goaliedata[] goalieData { get; set; }
        public string goalieCategories { get; set; }


    }

    /// <summary>
    /// Json Logic element of an array object
    /// </summary>
    public class Goaliedata
    {
        public int id { get; set; }
        public string data { get; set; }
    }

    /// <summary>
    /// Json Logic element of an array object
    /// </summary>
    public class Skaterdata
    {

        public int id { get; set; }
        public string data { get; set; }
    }

}