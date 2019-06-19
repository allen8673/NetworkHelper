using NetworkHelper.Model;
using NetworkHelper.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper
{
    public class Networker
    {
        /// <summary>
        /// Restful Post function
        /// </summary>
        /// <param name="api">Post api url</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<HttpResponseMessage> PostAsJson(string api, object request)
            => await ApiCaller.PostAsJson(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                }, request);

        /// <summary>
        /// Restful Get function
        /// </summary>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<HttpResponseMessage> Get(string api)
            => await ApiCaller.Get(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });

        /// <summary>
        /// Restful Put function
        /// </summary>
        /// <param name="api">Put api</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<HttpResponseMessage> PutAsJson(string api, object request)
            => await ApiCaller.PutAsJson(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                }, request);

        /// <summary>
        /// Restful Delete function
        /// </summary>
        /// <param name="api">Delete api</param>
        /// <returns></returns>
        public async static Task<HttpResponseMessage> Delete(string api)
            => await ApiCaller.Delete(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });

    }
}
