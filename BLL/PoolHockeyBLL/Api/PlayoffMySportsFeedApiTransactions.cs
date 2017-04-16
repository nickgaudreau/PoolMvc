using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoolHockeyBLL.ApiModels;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Log;

namespace PoolHockeyBLL.Api
{
    public class PlayoffMySportsFeedApiTransactions
    {
        /// <summary>
        /// get "cumulative_player_stats.json" from MySportsFeeds NHL API and return it parse into entity list
        /// </summary>
        /// <returns></returns>
        public List<PlayoffPlayerInfoEntity> GetData()
        {
            var playerInfoEntities = new List<PlayoffPlayerInfoEntity>();

            MySportsFeeds apiResults = null;

            var request = new HttpClient();
            // possibly this 1st one
            request.BaseAddress = new Uri("https://www.mysportsfeeds.com/api/feed/pull/nhl/2017-playoff/");
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "bmlja2dvb2Ryb3c6R29kcm81NQ=="
                //Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{password}"))
                );
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                using (
                    //possibly this one instead.. 
                    var response = request.GetAsync("cumulative_player_stats.json?playerstats=G,A,Pts,PPPts,SHPts,GWG").ContinueWith((taskWithResponse) =>
                    //var response = request.GetAsync("cumulative_player_stats.json?").ContinueWith((taskWithResponse) =>
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
                                apiResults = data;
                            }
                            else
                            {
                                var e = new Exception($"{taskWithResponse.Result.StatusCode} : {taskWithResponse.Result.ReasonPhrase}");
                                LogError.Write(e, 
                                    "issue while retriving data from MySportsFeed -> GetData()");
                                throw e;
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

                            var playerInfo = new PlayoffPlayerInfoEntity(id, team, pos, name, game, goal, assist, pts, ppp, shp, gwg, isRookie);

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
            catch (Exception ex)
            {
                LogError.Write(ex, "issue while retriving data from MySportsFeed -> GetData()");
            }
            return playerInfoEntities;
        }

        public List<List<PlayoffPlayerInfoEntity>> SplitDataTeamLists()
        {
            var listOfTeamList = new List<List<PlayoffPlayerInfoEntity>>();

            var dataAsPlayerInfoEntities = GetData();
            foreach (var team in Constants.Teams.Playoff)
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

}

