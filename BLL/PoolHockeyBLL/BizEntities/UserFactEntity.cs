namespace PoolHockeyBLL.BizEntities
{
    public class UserFactEntity
    {
        public int I_Pk { get; set; }
        public string C_Code { get; set; }
        public string C_UserEmail { get; set; }
        public int I_BestMonth { get; set; }
        public System.DateTime D_BestMonth { get; set; }
        public int I_BestWeek { get; set; }
        public System.DateTime D_BestWeek { get; set; }
        public int I_BestDay { get; set; }
        public System.DateTime D_BestDay { get; set; }
        public int I_BestRank { get; set; }
        public System.DateTime D_BestRank { get; set; }
        public int I_WorstMonth { get; set; }
        public System.DateTime D_WorstMonth { get; set; }
        public int I_WorstWeek { get; set; }
        public System.DateTime D_WorstWeek { get; set; }
        public int I_WorstDay { get; set; }
        public System.DateTime D_WorstDay { get; set; }
        public int I_WorstRank { get; set; }
        public System.DateTime D_WorstRank { get; set; }
        public int I_PtDiffOn1st { get; set; }
    }
}
