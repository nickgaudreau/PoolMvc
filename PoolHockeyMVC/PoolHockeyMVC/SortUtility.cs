using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyMVC
{
    public static class SortUtility
    {

        public static IEnumerable<IPlayerEntity> SortPlayerInfoTable(IEnumerable<IPlayerEntity> playerInfoEntities, string sortBy, string sortOrder )
        {
            var rm = new ResourceManager("PoolHockeyMVC.Resources.Global", Assembly.GetExecutingAssembly());
            if (sortBy == rm.GetString("Name"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.C_Name).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.C_Name).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Team"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.C_Team).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.C_Team).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Game"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_Game).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_Game).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Goal"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_Goal).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_Goal).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Assist"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_Assist).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_Assist).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Points"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_Point).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_Point).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("TimeOnIce"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.C_Toi).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.C_Toi).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("PowerPlay"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_PpP).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_PpP).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("ShortHanded"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_ShP).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_ShP).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("GameWinning"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_GwG).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_GwG).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("OverTime"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_OtG).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_OtG).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Yesterday"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_PtLastD).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_PtLastD).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Week"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_PtLastW).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_PtLastW).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Month"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        playerInfoEntities = playerInfoEntities.OrderBy(x => x.I_PtLastM).ToList();
                        break;
                    case "Desc":
                        playerInfoEntities = playerInfoEntities.OrderByDescending(x => x.I_PtLastM).ToList();
                        break;
                    default:
                        break;
                }
            }
            return playerInfoEntities;
        }

        public static IEnumerable<IUserEntity> SortUserInfoTable(IEnumerable<IUserEntity> userInfoEntities, string sortBy, string sortOrder)
        {
            var rm = new ResourceManager("PoolHockeyMVC.Resources.Global", Assembly.GetExecutingAssembly());
            
            if (sortBy == rm.GetString("Game"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_Games).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_Games).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Goal"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_Goals).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_Goals).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Assist"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_Assists).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_Assists).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Points"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_Points).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_Points).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Yesterday"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_PtLastD).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_PtLastD).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Week"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_PtLastW).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_PtLastW).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (sortBy == rm.GetString("Month"))
            {
                switch (sortOrder)
                {
                    case "Asc":
                        userInfoEntities = userInfoEntities.OrderBy(x => x.I_PtLastM).ToList();
                        break;
                    case "Desc":
                        userInfoEntities = userInfoEntities.OrderByDescending(x => x.I_PtLastM).ToList();
                        break;
                    default:
                        break;
                }
            }

            return userInfoEntities;
        }
    }
}
