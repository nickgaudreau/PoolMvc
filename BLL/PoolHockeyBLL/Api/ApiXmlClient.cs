using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PoolHockeyBLL.Log;

namespace PoolHockeyBLL.Api
{
    public class ApiXmlClient : IApiClient
    {
        private HttpClient ConfigureRequest()
        {
            var request = new HttpClient();
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            return request;
        }

        public ApiResponse<T> Get<T>(string url, string uri) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                var request = ConfigureRequest();
                using (
                    var response = request.GetAsync(url + uri).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                result.Success = false;
                                result.Exception =
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).";
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                var serializer = new XmlSerializer(typeof(T));
                                T data;
                                using (TextReader reader = new StringReader(jsonString.Result))
                                    data = (T)serializer.Deserialize(reader);
                                result.Success = true;
                                result.Data = data;
                            }
                            else if (taskWithResponse.Result.StatusCode == HttpStatusCode.InternalServerError)
                            {
                                var jsonCisFeedback = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //var cisServerError = JsonConvert.DeserializeObject<CisServerError>(jsonCisFeedback.Result);
                                //var message =
                                //$"CIS Internal server exception: {cisServerError.ExceptionMessage}, for BizObject: {typeof(T)}";
                                throw new Exception(jsonCisFeedback.Result);
                            }
                            // anything else such as 404. 
                            else
                            {
                                var jsonCisFeedback = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //var formattedFeedback = JsonConvert.DeserializeObject<ApiFeedbackItem[]>(jsonCisFeedback.Result);
                                result.Success = false;
                                //result.FormattedFeedbackItems = formattedFeedback;
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }
                    }))
                {
                    response.Wait();
                }
                //var response = await request.GetAsync(url);
                //var res = await response.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<T>(res);
                //return data;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();
                LogError.Write(ex, "ApiXmlClient GET" );
            }
            return result;
        }

        public ApiResponse<T> Post<T>(string url, string uri, T o) where T : class
        {
            throw new NotImplementedException();
        }

        public ApiResponse<T> Put<T>(string url, string uri, T o) where T : class
        {
            throw new NotImplementedException();
        }

        public ApiResponse<T> Delete<T>(string url, string uri) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
