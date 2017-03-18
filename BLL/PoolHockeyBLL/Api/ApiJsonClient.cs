using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PoolHockeyBLL.Api
{
    public class ApiJsonClient : IApiClient
    {
        /// <summary>
        /// Retrieves REST API base URL from configuration file
        /// </summary>
        /// <returns></returns>
        private string GetRestApiBaseUrl()
        {
            var baseUrl = "";
            try
            {
                baseUrl = ConfigurationManager.AppSettings["RestApiBaseUrl"] ?? "Not Found";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading app settings: " + ex);
            }
            return baseUrl;
        }

        /// <summary>
        /// Basic Auth for Web Api Controller: [ApiAuthenticationFilter] . Builds HttpClient object for a given URL and HTTP method and initializes its basic properties 
        /// </summary>
        /// <returns></returns>
        private HttpClient ConfigureBasicAuthRequest(string url)
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(url);
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "bmlja2dhdWRyZWF1OkdvZHJvNTU="
                //Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{password}"))
                );
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // needed for github
            request.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            return request;
        }

        private HttpClient ConfigureRequest(string url)
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(url);
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // needed for github
            request.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            return request;
        }


        /// <summary>
        /// Generic Web API caller
        /// </summary>
        /// <returns></returns>
        public ApiResponse<T> Get<T>(string url, string uri) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                var request = ConfigureRequest(url);
                using (
                    var response = request.GetAsync(uri).ContinueWith((taskWithResponse) =>
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
                                var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                result.Success = true;
                                result.Data = data;
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
            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for POST
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="o">Object to post</param>
        /// <returns></returns>
        public ApiResponse<T> Post<T>(string url, string uri, T o) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest(url);

                StringContent content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                using (
                    var response = request.PostAsync(uri, content).ContinueWith((taskWithResponse) =>
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
                                var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
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

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for PUT
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="uri"></param>
        /// <param name="o">Object to post</param>
        /// <returns></returns>
        public ApiResponse<T> Put<T>(string url, string uri, T o) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest(url);

                StringContent content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                using (
                    var response = request.PutAsync(uri, content).ContinueWith((taskWithResponse) =>
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
                                //var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //jsonString.Wait();
                                //var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                taskWithResponse.Wait(); // TODO use async/await?
                                result.Success = true;
                                //result.Data = data;
                                
                            }

                        }
                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for DELETE
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public ApiResponse<T> Delete<T>(string url, string uri) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest(url);
                using (
                    var response = request.DeleteAsync(uri).ContinueWith((taskWithResponse) =>
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
                                //var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //jsonString.Wait();
                                //var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                taskWithResponse.Wait();
                                result.Success = true;
                                //result.Data = data;
                                
                            }

                        }


                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }
        
    }
}