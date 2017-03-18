using System.Net;

namespace PoolHockeyBLL.Api
{
    public class ApiResponse<T> where T : class
    {

        public bool Success { get; set; }
        public T Data { get; set; }
        public string Exception { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public ApiFeedbackItem[] FormattedFeedbackItems { get; set; }
    }


    public class ApiFeedbackItem
    {
        public string Message;
        public string Field;
        public object Value;
    }
}