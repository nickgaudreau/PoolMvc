using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// IPlayoffPlayoffUser Service Contract
    /// </summary>
    public interface IPlayoffUserInfoServices
    {
        PlayoffUserInfoEntity GetByEmail(string email);
        PlayoffUserInfoEntity GetTopBestDay();        PlayoffUserInfoEntity GetTopBestMonth();
        //PlayoffUserInfoEntity GetBestLastD();        IEnumerable<PlayoffUserInfoEntity> GetAll();        IEnumerable<PlayoffUserInfoEntity> GetAllWhere(string userEmail);        bool Create(PlayoffUserInfoEntity userInfoEntity);        bool Update(PlayoffUserInfoEntity userInfoEntity, string code);        bool UpdateAll();
        void UpdateBestDay();
        void UpdateBestMonth();
        //void UpdateBestLastD();        bool Delete(string code);
    }
}
