using System;

namespace PoolHockeyBLL.BizEntities
{
    public interface IUserEntity
    {
         int I_Pk { get; set; }
         string C_Code { get; set; }
         string C_UserEmail { get; set; }
         string C_DisplayName { get; set; }
         int I_Games { get; set; }
         int I_Goals { get; set; }
         int I_Assists { get; set; }
         int I_Points { get; set; }
         string C_Pic { get; set; }
         int I_BestDay { get; set; }
         DateTime D_BestDay { get; set; }
         int I_BestMonth { get; set; }
         DateTime D_BestMonth { get; set; }
         int I_BestLastD { get; set; }
         int I_PtLastD { get; set; }
         int I_PtLastW { get; set; }
         int I_PtLastM { get; set; }
    }
}
