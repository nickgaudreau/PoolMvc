using System.Collections.Generic;

namespace PoolHockeyBLL.ViewModels
{
    public class UserInfoVm
    {
        public string Code { get; set; }
        public string UserEmail { get; set; }
        public string DisplayName { get; set; }
        public int Games { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public string Pic { get; set; }
    }
}
