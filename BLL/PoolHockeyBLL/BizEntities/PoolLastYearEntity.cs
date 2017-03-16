namespace PoolHockeyBLL.BizEntities
{
    public class PoolLastYearEntity
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
        public int I_PpG { get; set; }
        public int I_ShG { get; set; }
        public int I_GwG { get; set; }
        public int I_OtG { get; set; }
    }
}
