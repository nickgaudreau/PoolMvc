using Newtonsoft.Json;

namespace PoolHockeyBLL.ApiModels
{
    public class MySportsFeeds
    {
        public Cumulativeplayerstats cumulativeplayerstats { get; set; }
    }

    public class Cumulativeplayerstats
    {
        public string lastUpdatedOn { get; set; }
        public Playerstatsentry[] playerstatsentry
        {
            get;
            set;
        }
    }

    public class Playerstatsentry
    {
        public Player player { get; set; }
        public Team team
        {
            get;
            set;
        }
        [JsonProperty("stats")]
        public Stats stats
        {
            get;
            set;
        }
    }

    public class Player
    {
        public string ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string JerseyNumber { get; set; }
        public string Position { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BirthDate { get; set; }
        public string Age { get; set; }
        public string BirthCity { get; set; }
        public string BirthCountry { get; set; }
        public string IsRookie { get; set; }
    }

    public class Team
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Stats
    {
        public Gamesplayed GamesPlayed
        {
            get;
            set;
        }
        [JsonProperty("stats")]
        public Stats1 stats
        {
            get;
            set;
        }
    }

    public class Gamesplayed
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation
        {
            get;
            set;
        }
        [JsonProperty("#text")]
        public string text
        {
            get;
            set;
        }
    }

    [JsonObject("stats")]
    public class Stats1
    {
        public Goals Goals
        {
            get;
            set;
        }
        public Assists Assists
        {
            get;
            set;
        }
        public Points Points { get; set; }
        public Hattricks HatTricks { get; set; }
        public Plusminus PlusMinus { get; set; }
        public Shots Shots { get; set; }
        public Shotpercentage ShotPercentage { get; set; }
        public Penalties Penalties { get; set; }
        public Penaltyminutes PenaltyMinutes { get; set; }
        public Powerplaygoals PowerplayGoals { get; set; }
        public Powerplayassists PowerplayAssists { get; set; }
        public Powerplaypoints PowerplayPoints { get; set; }
        public Shorthandedgoals ShorthandedGoals { get; set; }
        public Shorthandedassists ShorthandedAssists { get; set; }
        public Shorthandedpoints ShorthandedPoints { get; set; }
        public Gamewinninggoals GameWinningGoals { get; set; }
        public Gametyinggoals GameTyingGoals { get; set; }
        public Hits Hits { get; set; }
        public Faceoffs Faceoffs { get; set; }
        public Faceoffwins FaceoffWins { get; set; }
        public Faceofflosses FaceoffLosses { get; set; }
        public Faceoffpercent FaceoffPercent { get; set; }
    }

    public class Goals
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Assists
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Points
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Hattricks
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Plusminus
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Shots
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Shotpercentage
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Penalties
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Penaltyminutes
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Powerplaygoals
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Powerplayassists
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Powerplaypoints
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Shorthandedgoals
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Shorthandedassists
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Shorthandedpoints
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Gamewinninggoals
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Gametyinggoals
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Hits
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Faceoffs
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Faceoffwins
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Faceofflosses
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Faceoffpercent
    {
        [JsonProperty("@abbreviation")]
        public string abbreviation { get; set; }
        [JsonProperty("#text")]
        public string text { get; set; }
    }
}
