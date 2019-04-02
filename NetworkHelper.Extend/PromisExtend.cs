using NetworkHelper.Extend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Extend
{
    public static class PromisExtend
    {
        public static async Task<PromiseResponse<T>> IfFailure<T>(this Task<PromiseResponse<T>> response, Action<HttpResponseMessage> act)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                act(reponse);
            }
            return await response;
        }

        public static async Task<PromiseResponse> IfFailure(this Task<PromiseResponse> response, Action<HttpResponseMessage> act)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                act(reponse);
            }
            return await response;
        }

        private static Action<HttpResponseMessage> FailureAct { get; set; }

        public static void RegisterFailureAct(Action<HttpResponseMessage> act) => FailureAct = act;

        public static async Task<PromiseResponse<T>> IfFailure<T>(this Task<PromiseResponse<T>> response)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                (FailureAct ?? new Action<HttpResponseMessage>((rsp) => Console.WriteLine("網路連線失敗..."))).Invoke(reponse);
            }
            return await response;
        }

        public static async Task<PromiseResponse> IfFailure(this Task<PromiseResponse> response)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                (FailureAct ?? new Action<HttpResponseMessage>((rsp) => Console.WriteLine("網路連線失敗..."))).Invoke(reponse);
            }
            return await response;
        }

        public static async Task<PromiseResponse<T>> IfTimeOut<T>(this Task<PromiseResponse<T>> response, Action act)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                (FailureAct ?? new Action<HttpResponseMessage>((rsp) => Console.WriteLine("網路連線失敗..."))).Invoke(reponse);
            }
            return await response;
        }

        public static async Task<PromiseResponse> IfTimeOut(this Task<PromiseResponse> response, Action act)
        {
            HttpResponseMessage reponse;
            if (!(reponse = (await response).HttpResponse).IsSuccessStatusCode)
            {
                (FailureAct ?? new Action<HttpResponseMessage>((rsp) => Console.WriteLine("網路連線失敗..."))).Invoke(reponse);
            }
            return await response;
        }

        public static async Task<PromiseResponse<T>> IfSuccess<T>(this Task<PromiseResponse<T>> response, Action act)
        {
            if ((await response).HttpResponse.IsSuccessStatusCode)
            {
                act();
            }
            return await response;
        }

        public static async Task<PromiseResponse<T>> IfSuccess<T>(this Task<PromiseResponse<T>> response, Action<T> act)
        {
            if ((await response).HttpResponse.IsSuccessStatusCode)
            {
                T result = await response;
                act(result);
            }
            return await response;
        }

        public static async Task<PromiseResponse> IfSuccess(this Task<PromiseResponse> response, Action act)
        {
            if ((await response).HttpResponse.IsSuccessStatusCode)
            {
                act();
            }
            return await response;
        }

        public static async Task<HttpStatusCode> GetStatusCode<T>(this Task<PromiseResponse<T>> response) => (await response).HttpResponse.StatusCode;
    }
}
