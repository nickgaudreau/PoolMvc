using System;

namespace PoolHockeyBLL.Api
{
    class ApiException : Exception
    {
        public ApiException()
        {
        }

        public ApiException(string details) : base($"Application force to stop due to this reason: {details}")
        {
            
        }
    }
}
