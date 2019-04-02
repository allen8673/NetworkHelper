using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Model
{
    public class RequestInfo
    {
        public HttpMethod HttpMethod { get; internal set; }
        public string RequestUrl { get; internal set; }
        public int HttpStatusCode { get; private set; }
        public string HttpStatus { get; private set; }
        public string RequestContent { get; internal set; }
        public string ResponseContent { get; internal set; }

        internal void SetHttpStatus(HttpStatusCode httpStatus)
        {
            this.HttpStatusCode = (int)httpStatus;
            this.HttpStatus = httpStatus.ToString();
        }

    }
}
