using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PoolHockeyBLL.Constants;
using PoolHockeyBLL.Contracts;
using PoolHockeyBOL;

namespace PoolHockeyBLL.Api
{
    public class TeamScheduleApiTransactions
    {
        private readonly ITeamScheduleServices _teamScheduleServices;

        public TeamScheduleApiTransactions(ITeamScheduleServices teamScheduleServices)
        {
            _teamScheduleServices = teamScheduleServices; //new TeamScheduleServices();
        }

        private List<Match> GetMatches()
        {
            List<Match> data = null;
            try
            {
                // file in Client/bin
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
                var json = Path.Combine(basePath, "SeasonSchedule-20162017.json");

                // FIle.ReadAllText : open, read, then clode the file no need of using(){}
                data = JsonConvert.DeserializeObject<List<Match>>(File.ReadAllText(json));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
        }

        private List<TeamSchedule> TransformDataToModel(List<Match> matches)
        {
            var teamSchedules = new List<TeamSchedule>();

            foreach (var team in Teams.Season)
            {
                var teamSchedule = matches.Where(x => String.Equals(x.a, team, StringComparison.CurrentCultureIgnoreCase)
                || String.Equals(x.h, team, StringComparison.CurrentCultureIgnoreCase));

                foreach (var match in teamSchedule)
                {
                    var dateString = match.est.Split(' ');
                    // dateString[0] => 20161130, change to 2016-11-30
                    var newDate = DateTime.ParseExact(dateString[0],
                                  "yyyyMMdd",
                                   CultureInfo.InvariantCulture);
                    teamSchedules.Add(new TeamSchedule()
                    {
                        C_Team = team,
                        D_Date = newDate
                    });
                }
            }

            if (!teamSchedules.Any())
                return null;

            return teamSchedules;
        }

        public bool SaveSchedule()
        {
            var saved = false;

            var matches = GetMatches();

            var teamSchedules = TransformDataToModel(matches);

            foreach (var teamSchedule in teamSchedules)
            {
                _teamScheduleServices.Create(teamSchedule);
            }
            return saved;
        }

    }



    internal class Match
    {
        // game ID
        public int id { get; set; }
        // DateTime EST
        public string est { get; set; }
        // AWAY team
        public string a { get; set; }
        // home team
        public string h { get; set; }
    }
}

