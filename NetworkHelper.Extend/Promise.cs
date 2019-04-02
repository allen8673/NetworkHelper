using NetworkHelper.Extend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Extend
{
    public class Promise
    {
        /// <summary>
        /// Restful Post function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Post api url</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<PromiseResponse<TRsp>> PostJson<TRsp>(string api, object request)
        {
            try
            {
                return await Networker.PostAsJson(api, request);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                return ex;
            }
        }

        /// <summary>
        /// Restful Get function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Get api</param>
        /// <param name="jPath">Parse jParh for Response json</param>
        /// <returns></returns>
        public async static Task<PromiseResponse<TRsp>> Get<TRsp>(string api, params object[] jPath)
        {
            PromiseResponse<TRsp> response;
            try
            {
                response = await Networker.Get(api);
                return response % jPath;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                return ex;
            }
        }

        /// <summary>
        /// Restful Put function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Put api</param>
        /// <param name="request">Request content</param>
        /// <param name="failureAct">Failure callback</param>
        /// <returns></returns>
        public async static Task<PromiseResponse<TRsp>> Put<TRsp>(string api, object request)
        {
            try
            {
                return await Networker.PutAsJson(api, request);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                return ex;
            }
        }

        /// <summary>
        /// Restful Delete function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<PromiseResponse> Delete(string api)
        {
            PromiseResponse response;
            try
            {
                response = await Networker.Delete(api);
                return response;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }
    }
}
