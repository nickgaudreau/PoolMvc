using System;
using System.Net;
using Newtonsoft.Json;
using PoolHockeyBLL.Log;

namespace PoolHockeyBLL
{
    public static class JsonUtility
    {
        /// <summary>
        /// Get Json Serialized Data from URL
        /// </summary>
        /// <param name="url"></param>
        public static string GetSerializedJsonData(string url)
        {
            using (var w = new WebClient())
            {
                var jsonData = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    jsonData = w.DownloadString(url);
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Get data from url nhl api fail");
                }
                return !string.IsNullOrEmpty(jsonData) ? jsonData : "";
            }
        }

        /// <summary>
        /// De-serialized Data from json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>deserialize data to class and return its instance </returns>
        public static T DeserializedJsonData<T>(string jsonData) where T : new()
        {
            if(string.IsNullOrEmpty(jsonData)) return new T();

            try
            {
                var type = JsonConvert.DeserializeObject<T>(jsonData);
                return type;
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Deserialization Fail");
            }

            return new T();
        }

    }
}
