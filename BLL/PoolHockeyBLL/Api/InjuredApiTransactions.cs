using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using PoolHockeyBLL.Log;
using PoolHockeyBOL.JsonModels;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace PoolHockeyBLL.Api
{
    public class InjuredApiTransactions
    {
        
        public static IEnumerable<InjuredApi> GetInjuredList()
        {
            // setup the browser
            var browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true
            };
            // Browser has many settings you can access in setup

            //Get the webste
            var pageResult = browser.NavigateToPage(
                new Uri("http://sportsstats.cbc.ca/hockey/nhl-injuries.aspx?page=/data/nhl/injury/injuries.html%3fhf=off"));

            // get data table
            var tableTeamInjuredPlayersNodes = pageResult.Html.CssSelect(".sdi-data-wide").ToList();

            var injuredPlayers = new List<InjuredApi>();
            foreach (var tables in tableTeamInjuredPlayersNodes)
            {
                var tRows = tables.SelectNodes("tr").Where(x => x.Attributes.Contains("valign")).ToList();
                foreach (var tRow in tRows)
                {
                    try
                    {
                        var tData = tRow.ChildNodes.Where(x => x.Name.ToString() == "td").ToList();
                        var injuredPlayer = new InjuredApi();
                        var name = tData.First(x => x.FirstChild.Name == "a");
                        var injuredName = CleanString(name.InnerText);
                        injuredPlayer.Name = injuredName; // new api no need to adapt take full name ...AdaptNameToApi(injuredName);
                        var pos = tData[1].InnerText;
                        var injuredPos = CleanString(pos);
                        injuredPlayer.Position = AdaptPositionToApi(injuredPos);
                        var status = tData[2].InnerText;
                        injuredPlayer.Status = CleanString(status);
                        var details = tData[3].InnerText;
                        injuredPlayer.Details = CleanString(details);

                        injuredPlayers.Add(injuredPlayer);
                    }
                    catch (Exception e)
                    {
                        LogError.Write(e, "Exception in GetInjuredList() at TROW: " + tRow);
                    }

                }
            }

            return injuredPlayers;
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
            var lastName = names[1];

            var firstLetterFirstName = firstName[0].ToString();

            var abbrName = firstLetterFirstName + ". " + lastName;
            return abbrName;
        }

        static string AdaptPositionToApi(string position)
        {
            var positionLetter = position[0].ToString();
            return positionLetter;
        }

    }
}
