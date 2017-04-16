using System;

namespace PoolHockeyBLL.BizEntities
{
    /// <summary>
    /// SkaterData receievd from NhlAPI
    /// </summary>
    public class PlayoffPlayerInfoEntity : IPlayerEntity
    {
        public int I_Pk { get; set; } 
        public int I_ApiId { get; set; }
        public string C_Team { get; set; }
        public string C_Code { get; set; }
        public string C_Pos { get; set; }
        public string C_Name { get; set; }
        public int I_Game { get; set; }
        public int I_Goal { get; set; }
        public int I_Assist { get; set; }
        public int I_Point { get; set; }
        public string C_UserEmail { get; set; }
        public int I_Round { get; set; }
        public double F_Avg { get; set; }
        public int I_Status { get; set; }
        public int I_PtLastD { get; set; }
        public int I_PtLastW { get; set; }
        public int I_PtLastM { get; set; }
        public bool L_Traded { get; set; }
        public string C_TradedTeam { get; set; }
        public string C_Toi { get; set; }
        public bool L_IsInjured { get; set; }
        public string C_InjStatus { get; set; }
        public string C_InjDetails { get; set; }
        public bool L_IsPlayoff { get; set; }
        public bool L_IsPlaying { get; set; }
        public int I_PpP { get; set; }
        public int I_ShP { get; set; }
        public int I_GwG { get; set; }
        public int I_OtG { get; set; }
        public bool L_IsRookie { get; set; }


        /// <summary>
        /// MySportsFeeds Api data - PP and SH points not goal
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="team"></param>
        /// <param name="pos"></param>
        /// <param name="name"></param>
        /// <param name="game"></param>
        /// <param name="goal"></param>
        /// <param name="assist"></param>
        /// <param name="point"></param>
        public PlayoffPlayerInfoEntity(int apiId, string team, string pos, string name, int game, int goal, int assist, int point, int ppp, int shp, int gwG, bool isRookie)
        {
            // TODO throw exception if some values are null + log

            this.I_ApiId = apiId;
            this.C_Team = team;
            this.C_Pos = pos;
            this.C_Name = name;
            this.I_Game = game;
            this.I_Goal = goal;
            this.I_Assist = assist;
            this.I_Point = point;
            
            this.I_PpP = ppp;
            this.I_ShP = shp;
            this.I_GwG = gwG;
            this.L_IsRookie = isRookie;

            // DEFAULT in DB design
            this.C_Toi = ""; // not in this new API
            this.C_UserEmail = "";
            this.C_TradedTeam = "";
            this.C_InjStatus = "";
            this.C_InjDetails = "";
        }

        /// <summary>
        /// New Api data received
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="team"></param>
        /// <param name="pos"></param>
        /// <param name="name"></param>
        /// <param name="game"></param>
        /// <param name="goal"></param>
        /// <param name="assist"></param>
        /// <param name="point"></param>
        /// <param name="toi"></param>
        public PlayoffPlayerInfoEntity(int apiId, string team, string pos, string name, int game, int goal, int assist, int point, string toi, int ppG, int shG, int gwG, int otG)
        {
            // TODO throw exception if some values are null + log

            this.I_ApiId = apiId;
            this.C_Team = team;
            this.C_Pos = pos;
            this.C_Name = name;
            this.I_Game = game;
            this.I_Goal = goal;
            this.I_Assist = assist;
            this.I_Point = point;
            this.C_Toi = toi;
            this.I_PpP = ppG;
            this.I_ShP = shG;
            this.I_GwG = gwG;
            this.I_OtG = otG;

            // default init
            this.C_Code = "";
            this.C_UserEmail = "";
            this.C_TradedTeam = "";
            this.C_InjStatus = "";
            this.C_InjDetails = "";
        }

        /// <summary>
        /// New 12 ONLY no jersey #, name or POS, Api data received
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="team"></param>
        /// <param name="game"></param>
        /// <param name="goal"></param>
        /// <param name="assist"></param>
        /// <param name="point"></param>
        /// <param name="toi"></param>
        public PlayoffPlayerInfoEntity(int apiId, string team, int game, int goal, int assist, int point, string toi, int ppG, int shG, int gwG, int otG)
        {
            // TODO throw exception if some values are null + log

            this.I_ApiId = apiId;
            this.C_Team = team;
            this.I_Game = game;
            this.I_Goal = goal;
            this.I_Assist = assist;
            this.I_Point = point;
            this.C_Toi = toi;
            this.I_PpP = ppG;
            this.I_ShP = shG;
            this.I_GwG = gwG;
            this.I_OtG = otG;

            // default init
            this.C_Code = "";
            this.C_UserEmail = "";
            this.C_TradedTeam = "";
            this.C_InjStatus = "";
            this.C_InjDetails = "";
        }

        /// <summary>
        /// No args constructor needed for AutoMapper
        /// </summary>
        public PlayoffPlayerInfoEntity() { }



        //// Full might never be in used...
        //public PlayerInfoEntity(int apiId, string team, string pos, string name, int game, int goal, int assist, int point,
        // string userEmail, int round, double avg, int status, int ptLastD, int ptLastW, int ptLastM, bool traded, string tradedTeam, string toi,
        // bool isInjured, string injStatus, string injDetails, bool isPlayoff, bool isPlaying)
        //{
        //    // TODO throw exception if some values are null + log

        //    this.I_ApiId = apiId;
        //    this.C_Team = team;
        //    this.C_Code = Guid.NewGuid().ToString();
        //    this.C_Pos = pos;
        //    this.C_Name = name;
        //    this.I_Game = game;
        //    this.I_Goal = goal;
        //    this.I_Assist = assist;
        //    this.I_Point = point;
        //    this.C_UserEmail = userEmail;
        //    this.I_Round = round;
        //    this.F_Avg = avg;
        //    this.I_Status = status;
        //    this.I_PtLastD = ptLastD;
        //    this.I_PtLastW = ptLastW;
        //    this.I_PtLastM = ptLastM;
        //    this.L_Traded = traded;
        //    this.C_TradedTeam = tradedTeam;
        //    this.C_Toi = toi;
        //    this.L_IsInjured = isInjured;
        //    this.C_InjStatus = injStatus;
        //    this.C_InjDetails = injDetails;
        //    this.L_IsPlayoff = isPlayoff;
        //    this.L_IsPlaying = isPlaying;
        //}

        

    }
}
