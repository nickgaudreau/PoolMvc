using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PoolHockeyBLL.Constants
{
    public static class Rules
    {
        public static readonly int Rounds = 12;

        public static readonly List<int> RoundList = new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12};

        public static readonly int PlayoffRounds = 6;

        public static readonly List<int> PlayoffRoundList = new List<int>() { 1, 2, 3, 4, 5, 6 };
    }
}
