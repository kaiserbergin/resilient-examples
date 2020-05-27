using System.Net;

namespace ResilientAPI.Models
{
    public class GenerateResponseRequestWaitWhat
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int DelayInMilliseconds { get; set; }
    }
}
