namespace PoolHockeyBLL.ViewModels
{
    public class PlayerInfoVm
    {
        public int ApiId { get; set; }
        public string Team { get; set; }
        public string Code { get; set; }
        public string Pos { get; set; }
        public string Name { get; set; }
        public int Game { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public int Point { get; set; }
        public string UserEmail { get; set; }
        public int Round { get; set; }
        public double Avg { get; set; }
        public int Status { get; set; }
        public int PtLastD { get; set; }
        public int PtLastW { get; set; }
        public int PtLastM { get; set; }
        public bool Traded { get; set; }
        public string TradedTeam { get; set; }
        public string Toi { get; set; }
        public bool IsInjured { get; set; }
        public string InjStatus { get; set; }
        public string InjDetails { get; set; }
        public bool IsPlayoff { get; set; }
        public bool IsPlaying { get; set; }
    }
}
