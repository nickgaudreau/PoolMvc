using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;

namespace PoolHockeyBLL.Api
{
    public class MySportsFeedApiTransactions
    {

        private readonly IPlayerInfoServices _playerInfoServices;
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;

        public MySportsFeedApiTransactions(IPlayerInfoServices playerInfoServices, IPastPlayerInfoServices pastPlayerInfoServices)
        {
            _playerInfoServices = playerInfoServices;
            _pastPlayerInfoServices = pastPlayerInfoServices;
        }

        /// <summary>
        /// get "cumulative_player_stats.json" from MySportsFeeds NHL API and return it parse into entity list
        /// </summary>
        /// <returns></returns>
        public List<PlayerInfoEntity> GetData()
        {
            List<PlayerInfoEntity> playerInfoEntities = new List<PlayerInfoEntity>();

            MySportsFeeds apiResults = null;

            var request = new HttpClient();
            request.BaseAddress = new Uri("https://www.mysportsfeeds.com/api/feed/pull/nhl/2016-2017-regular/");
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "bmlja2dvb2Ryb3c6R29kcm81NQ=="
                //Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{password}"))
                );
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                using (
                    var response = request.GetAsync("cumulative_player_stats.json?").ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                throw new Exception(
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).");
                            }

                            if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                jsonString.Wait();
                                var data = JsonConvert.DeserializeObject<MySportsFeeds>(jsonString.Result);
                                taskWithResponse.Wait(); // TODO no Wait and keep async...?
                                apiResults = data;
                            }
                        }
                    }))
                {
                    response.Wait();

                    var resultCompleted = apiResults;

                    var listTeam = resultCompleted.cumulativeplayerstats.playerstatsentry;

                    if (listTeam.Length <= 0)
                        throw new ApiException("No data return from API");

                    var id = 0;
                    try
                    {
                        foreach (var playerstatsentry in listTeam)
                        {
                            id = Int32.Parse(playerstatsentry.player.ID);
                            var team = playerstatsentry.team.Abbreviation;
                            var pos = playerstatsentry.player.Position[0].ToString(); // only 1st char of string, e.g. LW => L
                            var name = playerstatsentry.player.FirstName + " " + playerstatsentry.player.LastName;
                            var game = Int32.Parse(playerstatsentry.stats.GamesPlayed.text);
                            var goal = Int32.Parse(playerstatsentry.stats.stats.Goals.text);
                            var assist = Int32.Parse(playerstatsentry.stats.stats.Assists.text);
                            var pts = Int32.Parse(playerstatsentry.stats.stats.Points.text);
                            var ppp = Int32.Parse(playerstatsentry.stats.stats.PowerplayPoints.text);
                            var shp = Int32.Parse(playerstatsentry.stats.stats.ShorthandedPoints.text);
                            var gwg = Int32.Parse(playerstatsentry.stats.stats.GameWinningGoals.text);
                            var isRookie = Boolean.Parse(playerstatsentry.player.IsRookie);

                            var playerInfo = new PlayerInfoEntity(id, team, pos, name, game, goal, assist, pts, ppp, shp, gwg, isRookie);

                            playerInfoEntities.Add(playerInfo);
                        }

                        return playerInfoEntities.OrderBy(x => x.C_Team).ToList();
                    }
                    catch (Exception e)
                    {
                        LogError.Write(e, $"Issue at this player id (if id is 0 means it happened on the 1st item.): ID :  {id}");
                    }

                }
            }
            catch (ApiException apiX)
            {
                LogError.Write(apiX, "Success 200,  but Empty cumulative_player_stats.json results");
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "issue while retriving data from MySportsFeed -> GetData()");
            }
            return playerInfoEntities;
        }

        public List<List<PlayerInfoEntity>> SplitDataTeamLists()
        {
            var listOfTeamList = new List<List<PlayerInfoEntity>>();

            var dataAsPlayerInfoEntities = GetData();
            foreach (var team in Constants.Teams.Season)
            {
                var teamList = dataAsPlayerInfoEntities.Where(x => x.C_Team == team).ToList();
                listOfTeamList.Add(teamList);
            }
            return listOfTeamList;
        }


        public static void TestWithJsonFile()
        {
            try
            {
                // file in Client/bin
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
                var json = Path.Combine(basePath, "cumulative_player_stats.json");

                // FIle.ReadAllText : open, read, then clode the file no need of using(){}
                var data = JsonConvert.DeserializeObject<MySportsFeeds>(File.ReadAllText(json));

                var test = data;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }



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

