using System;
using System.Collections.Generic;
using System.Linq;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Constants;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBOL.JsonModels;

namespace PoolHockeyBLL.Api
{
    public class NhlApiTransactions
    {
        private readonly IPlayerInfoServices _playerInfoServices;
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;

        public NhlApiTransactions(IPlayerInfoServices playerInfoServices, IPastPlayerInfoServices pastPlayerInfoServices)
        {
            _playerInfoServices = playerInfoServices;
            _pastPlayerInfoServices = pastPlayerInfoServices;
        }

        /// <summary>
        /// TODO - This could/should be in his own class - NhlApiServices. ...2 methods. UpdatePastPlayer(), UpdatePlayer()
        /// </summary>
        public void UpDbSeason()
        {
            foreach (var team in Teams.Season)
            {
                var playerInfoEntities = GetNhlApiTeamPlayersInfo(team);

                if (playerInfoEntities == null)
                {
                    LogError.Write(new Exception("Error"), "Fail while parsing deserialize NHl aPi Data, playerInfoEntities == null, for team:" + team);
                    return;
                }
                if (!playerInfoEntities.Any())
                {
                    continue;
                }

                _pastPlayerInfoServices.Create(playerInfoEntities);
                _playerInfoServices.Update(playerInfoEntities);

            }
        }

        public void UpDbSeasonWebScrapping()
        {
            for (int i = 1; i <= PoolHockeyBLL.Constants.Teams.SeasonWebScrappingDict.Count; i++)
            {
                var playerInfoEntities = NhlWebScrapingApiTransactions.GetTeamPlayerData(i);
                if (playerInfoEntities == null)
                {
                    LogError.Write(new Exception("Error"), "Fail GEt web scrapping NHl aPi Data, playerInfoEntities == null, for team:" + i);
                    return;
                }
                if (!playerInfoEntities.Any())
                {
                    LogError.Write(new Exception("Error"), "Fail GEt web scrapping NHl aPi Data, playerInfoEntities count is 0 , for team:" + i);
                    continue;
                }

                _pastPlayerInfoServices.Create(playerInfoEntities);
                _playerInfoServices.Update(playerInfoEntities);
            }
        }

        private static List<PlayerInfoEntity> GetNhlApiTeamPlayersInfo(string team)
        {

            var url = NhlApi.UrlPrefixSeason + team + NhlApi.UrlSuffix;

            var rawJsonData = JsonUtility.GetSerializedJsonData(url);
            if (string.IsNullOrEmpty(rawJsonData))
            {
                LogError.Write(new Exception("Dummy exception"), "NO raw data returned from JsonUtility.GetSerializedJsonData(url)");
                return null;
            }

            var nhlApiTeamPlayerInfo = JsonUtility.DeserializedJsonData<NhlTeamPlayerInfoApi>(rawJsonData);


            if (!nhlApiTeamPlayerInfo.skaterData.Any())
            {
                //LogError.Write(new Exception("No Data for this team"), "Fail to deserialize - DeserializedJsonData<T>(string jsonData) where T : new()");
                return new List<PlayerInfoEntity>(); // retrun empty 0 count
            }

            var playerInfoEntities = new List<PlayerInfoEntity>();
            try
            {
                foreach (Skaterdata skaterdata in nhlApiTeamPlayerInfo.skaterData)
                {
                    var id = skaterdata.id;
                    var tokens = skaterdata.data.Split(',');

                    // API changes - removed name, pos, and jersey #: length 12. Regualr 15
                    if (tokens.Length == 12)
                    {
                        var game12 = int.Parse(tokens[0]);
                        var goal12 = int.Parse(tokens[1]);
                        var assist12 = int.Parse(tokens[2]);
                        var point12 = int.Parse(tokens[3]);
                        var toi12 = tokens[7].Trim();
                        var ppG12 = int.Parse(tokens[8]);
                        var shG12 = int.Parse(tokens[9]);
                        var gwG12 = int.Parse(tokens[10]);
                        var otG12 = int.Parse(tokens[11]);

                        var playerInfo12 = new PlayerInfoEntity(id, team, game12, goal12, assist12, point12, toi12, ppG12, shG12, gwG12, otG12);

                        playerInfoEntities.Add(playerInfo12);

                        continue;
                    }

                    // Regular 15 length api data
                    var pos = tokens[1].Trim();
                    var name = (tokens[2]).Trim();
                    var game = int.Parse(tokens[3]);
                    var goal = int.Parse(tokens[4]);
                    var assist = int.Parse(tokens[5]);
                    var point = int.Parse(tokens[6]);
                    var toi = tokens[10].Trim();
                    var ppG = int.Parse(tokens[11]);
                    var shG = int.Parse(tokens[12]);
                    var gwG = int.Parse(tokens[13]);
                    var otG = int.Parse(tokens[14]);

                    var playerInfo = new PlayerInfoEntity(id, team, pos, name, game, goal, assist, point, toi, ppG, shG, gwG, otG);

                    playerInfoEntities.Add(playerInfo);

                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex, $"Fail while parsing deserrialize NHl aPi Data for team: {team}");
                return null;
            }

            return playerInfoEntities;
        }
    }
}
