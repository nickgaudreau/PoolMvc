using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Log;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace PoolHockeyBLL.Api
{
    public class NhlWebScrapingApiTransactions
    {


        public static List<PlayerInfoEntity> GetTeamPlayerData(int teamNo)
        {
            if (teamNo < 1)
                LogError.Write(new Exception("NhlWebScrapingApiTransactions.GetTeamPlayerData Team # is less than 1"), $"NhlWebScrapingApiTransactions.GetTeamPlayerData Team # is {teamNo}");

            // setup the browser
            var browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true
            };
            // Browser has many settings you can access in setup

            //Get the webste - get data by team
            var pageResult = browser.NavigateToPage(
                new Uri("http://sportsstats.cbc.ca/hockey/nhl-teams.aspx?page=/data/nhl/teams/stats/stats" + teamNo + ".html"));

            // get data table
            var tableTeamPlayerStats = pageResult.Html.CssSelect(".sdi-data-wide").FirstOrDefault();
            var playerInfoEntities = new List<PlayerInfoEntity>();


            if (tableTeamPlayerStats != null)
            {
                var tRows = tableTeamPlayerStats.SelectNodes("tr").Skip(1); // skip first table header
                foreach (var tRow in tRows)
                {
                    try
                    {
                        var tData = tRow.ChildNodes.Where(x => x.Name.ToString() == "td").ToList();

                        var playerInfoEntity = new PlayerInfoEntity();

                        var name = tData.ElementAt(0).InnerText;
                        name = AdaptNameToApi(CleanString(name));
                        var pos = AdaptPositionToApi(CleanString(tData.ElementAt(1).InnerHtml));

                        var team = "";
                        Constants.Teams.SeasonWebScrappingDict.TryGetValue(teamNo, out team);

                        var game = CleanString(tData.ElementAt(2).InnerHtml);
                        var goal = CleanString(tData.ElementAt(3).InnerHtml);
                        var ass = CleanString(tData.ElementAt(4).InnerHtml);
                        var pts = CleanString(tData.ElementAt(5).InnerHtml);
                        var ppg = CleanString(tData.ElementAt(8).InnerHtml);
                        var shg = CleanString(tData.ElementAt(9).InnerHtml);
                        var toi = CleanString(tData.ElementAt(11).InnerHtml);

                        // TODO: Make constructor
                        playerInfoEntity.C_Name = name;
                        playerInfoEntity.C_Pos = pos;
                        playerInfoEntity.C_Team = team;
                        playerInfoEntity.I_Game = Int32.Parse(game);
                        playerInfoEntity.I_Goal = Int32.Parse(goal);
                        playerInfoEntity.I_Assist = Int32.Parse(ass);
                        playerInfoEntity.I_Point = Int32.Parse(pts);
                        playerInfoEntity.I_PpP = Int32.Parse(ppg);
                        playerInfoEntity.I_ShP = Int32.Parse(shg);
                        playerInfoEntity.C_Toi = toi;

                        playerInfoEntities.Add(playerInfoEntity);


                    }
                    catch (Exception e)
                    {
                        LogError.Write(e,
                            "Exception in NhlWebScrapingApiTransactions.GetTeamPlayerData() at tRow: " + tRow?.InnerHtml);
                    }

                }
            }
            else
            {
                LogError.Write(new Exception("Error"),
                            $"No data for team {teamNo}");
            }

            return playerInfoEntities;
        }

        static string CleanString(string rawInnerHtml)
        {
            var decoded = WebUtility.HtmlDecode(rawInnerHtml);
            var cleanString = Regex.Replace(decoded, @"\t|\n|\r", "");
            return cleanString.Trim();
        }

        static string AdaptNameToApi(string fullName)
        {
            var names = fullName.Split(' ');
            var firstName = names[0];
            var lastName = "";

            // there should never be more than 2. Already rare.
            if (names.Length > 2)
                lastName = names[1] + " " + names[2];
            else
                lastName = names[1];

            var firstLetterFirstName = firstName[0].ToString();
            var cleanedLastName = lastName.Replace("*", "").TrimEnd();

            var abbrName = firstLetterFirstName + ". " + cleanedLastName;
            return abbrName;
        }

        static string AdaptPositionToApi(string position)
        {
            var positionLetter = position[0].ToString();
            return positionLetter;
        }

    }



}
