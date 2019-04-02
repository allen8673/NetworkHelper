using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Extend.Model
{
    public class PromiseResponse
    {
        public HttpResponseMessage HttpResponse { get; private set; }
        private PromiseResponse(HttpResponseMessage httpResponse) => HttpResponse = httpResponse;

        public PromiseResponse IfFailure(Action act)
        {
            if (!HttpResponse.IsSuccessStatusCode)
            {
                act();
            }

            return this;
        }

        public PromiseResponse IfSuccess(Action act)
        {
            if (HttpResponse.IsSuccessStatusCode)
            {
                act();
            }
            return this;
        }

        public static implicit operator bool(PromiseResponse response) => response.HttpResponse.IsSuccessStatusCode;

        public static implicit operator PromiseResponse(HttpResponseMessage httpResponse) => new PromiseResponse(httpResponse);
    }

    public class PromiseResponse<T>
    {
        public HttpResponseMessage HttpResponse { get; private set; }
        public Exception HttpException { get; private set; }
        public object[] ParserJPath { get; private set; } = new object[] { };
        private PromiseResponse(HttpResponseMessage httpResponse) => HttpResponse = httpResponse;
        private PromiseResponse(Exception httpException)
        {
            HttpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
            HttpException = httpException;
        }

        public PromiseResponse<T> IfFailure(Action act)
        {
            if (!HttpResponse.IsSuccessStatusCode)
            {
                act();
            }

            return this;
        }

        public PromiseResponse<T> IfSuccess(Action act)
        {
            if (HttpResponse.IsSuccessStatusCode)
            {
                act();
            }

            return this;
        }

        public static implicit operator T(PromiseResponse<T> response)
        {
            if (!response.HttpResponse.IsSuccessStatusCode)
            {
                return default(T);
            }

            string content = response.HttpResponse.Content.ReadAsStringAsync().Result;
            if (response.ParserJPath?.Count() == 0)
            {
                return JsonConvert.DeserializeObject<T>(content);
            }

            JToken jobject = JObject.Parse(content);
            foreach (object jpath in response.ParserJPath)
            {
                jobject = jobject[jpath];
            }
            return jobject.ToObject<T>();
        }

        public static implicit operator PromiseResponse<T>(HttpResponseMessage httpResponse) => new PromiseResponse<T>(httpResponse);

        public static implicit operator PromiseResponse<T>(Exception httpException) => new PromiseResponse<T>(httpException);


        public static PromiseResponse<T> operator %(PromiseResponse<T> response, object[] jPath)
        {
            response.ParserJPath = jPath;
            return response;
        }
    }
}
