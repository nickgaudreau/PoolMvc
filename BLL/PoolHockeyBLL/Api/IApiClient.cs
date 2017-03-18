using System.Net.Http;

namespace PoolHockeyBLL.Api
{
    public interface IApiClient
    {
        ApiResponse<T> Get<T>(string url, string uri) where T : class;

        ApiResponse<T> Post<T>(string url, string uri, T o) where T : class;

        ApiResponse<T> Put<T>(string url, string uri, T o) where T : class;

        ApiResponse<T> Delete<T>(string url, string uri) where T : class;
    }
}