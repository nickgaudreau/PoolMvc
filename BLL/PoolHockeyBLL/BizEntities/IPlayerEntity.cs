namespace PoolHockeyBLL.BizEntities
{
    /// <summary>
    /// For player info and playoff player info
    /// </summary>
    public interface IPlayerEntity
    {
        int I_Pk { get; set; }
        int I_ApiId { get; set; }
        string C_Team { get; set; }
        string C_Code { get; set; }
        string C_Pos { get; set; }
        string C_Name { get; set; }
        int I_Game { get; set; }
        int I_Goal { get; set; }
        int I_Assist { get; set; }
        int I_Point { get; set; }
        string C_UserEmail { get; set; }
        int I_Round { get; set; }
        double F_Avg { get; set; }
        int I_Status { get; set; }
        int I_PtLastD { get; set; }
        int I_PtLastW { get; set; }
        int I_PtLastM { get; set; }
        bool L_Traded { get; set; }
        string C_TradedTeam { get; set; }
        string C_Toi { get; set; }
        bool L_IsInjured { get; set; }
        string C_InjStatus { get; set; }
        string C_InjDetails { get; set; }
        bool L_IsPlayoff { get; set; }
        bool L_IsPlaying { get; set; }
        int I_PpP { get; set; }
        int I_ShP { get; set; }
        int I_GwG { get; set; }
        int I_OtG { get; set; }
        bool L_IsRookie { get; set; }
    }
}
