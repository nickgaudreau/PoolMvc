using System;

namespace PoolHockeyBLL.BizEntities
{
    public class UserInfoEntity
    {
        public int I_Pk { get; set; }
        public string C_Code { get; set; }
        public string C_UserEmail { get; set; }
        public string C_DisplayName { get; set; }
        public int I_Games { get; set; }
        public int I_Goals { get; set; }
        public int I_Assists { get; set; }
        public int I_Points { get; set; }
        public string C_Pic { get; set; }
        public int I_BestDay { get; set; }
        public DateTime D_BestDay { get; set; }
        public int I_BestMonth { get; set; }
        public DateTime D_BestMonth { get; set; }
        public int I_BestLastD { get; set; }
        public int I_PtLastD { get; set; }
        public int I_PtLastW { get; set; }
        public int I_PtLastM { get; set; }

        // Basic create
        public UserInfoEntity(string userEmail, string displayName)
        {
            C_Code = Guid.NewGuid().ToString();
            C_UserEmail = userEmail;
            C_DisplayName = displayName;
        }

        // stats updater 
        public UserInfoEntity(int game, int g, int a, int pts)
        {
            
        }

        // TODO to remove - unsafe object creator
        public UserInfoEntity() { }
    }


}
