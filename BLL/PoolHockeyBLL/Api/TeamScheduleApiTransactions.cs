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

        private MySportsFeedSchedule GetMatches()
        {
            MySportsFeedSchedule data = null;
            try
            {
                // file in Client/bin
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
                var json = Path.Combine(basePath, "Playoff1stRound.json");

                // FIle.ReadAllText : open, read, then clode the file no need of using(){}
                data = JsonConvert.DeserializeObject<MySportsFeedSchedule>(File.ReadAllText(json));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
        }

        private List<TeamSchedule> TransformDataToModel(MySportsFeedSchedule matches)
        {
            var teamSchedules = new List<TeamSchedule>();

            foreach (var team in Teams.Playoff)
            {
                var teamSchedule = matches.fullgameschedule.gameentry.Where(x => String.Equals(x.awayTeam.Abbreviation, team, StringComparison.CurrentCultureIgnoreCase)
                || String.Equals(x.homeTeam.Abbreviation, team, StringComparison.CurrentCultureIgnoreCase));

                foreach (var match in teamSchedule)
                {
                    //var dateString = match.date.Split(' ');
                    // dateString[0] => 20161130, change to 2016-11-30
                    //var newDate = DateTime.ParseExact(match.date,
                    //              "yyyyMMdd",
                    //               CultureInfo.InvariantCulture);
                    var newDate = DateTime.Parse(match.date);
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

    // New MySportsFeed
    public class MySportsFeedSchedule
    {
        public Fullgameschedule fullgameschedule { get; set; }
    }

    public class Fullgameschedule
    {
        public string lastUpdatedOn { get; set; }
        public Gameentry[] gameentry { get; set; }
    }

    public class Gameentry
    {
        public string id { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public Awayteam awayTeam { get; set; }
        public Hometeam homeTeam { get; set; }
        public string location { get; set; }
    }

    public class Awayteam
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Hometeam
    {
        public string ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

}

