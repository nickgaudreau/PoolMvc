using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// UserInfo Service Contract
    /// </summary>
    public interface IUserInfoServices
    {
        UserInfoEntity GetByEmail(string email);
        UserInfoEntity GetTopBestDay();        UserInfoEntity GetTopBestMonth();
        //UserInfoEntity GetBestLastD();        IEnumerable<UserInfoEntity> GetAll();        IEnumerable<UserInfoEntity> GetAllWhere(string userEmail);        bool Create(UserInfoEntity userInfoEntity);        bool Update(UserInfoEntity userInfoEntity, string code);        bool UpdateAll();
        void UpdateBestDay();
        void UpdateBestMonth();
        //void UpdateBestLastD();        bool Delete(string code);
    }
}
