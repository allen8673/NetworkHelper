using NetworkHelper.Extend.Model;
using NetworkHelper.Model;
using NetworkHelper.Process;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public async static Task<PromiseResponse<TRsp>> PostJson<TRsp>(string api, object request, params object[] jPath)
        {
            try
            {
                PromiseResponse<TRsp> response = await ApiCaller.PostAsJson(
                        UriFetcher.BaseUri, api,
                        new RequestHeader
                        {
                            token = await TokenFetcher.GetToken()
                        }, request);

                return response % jPath;
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
                response = await ApiCaller.Get(UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    });
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
        public async static Task<PromiseResponse<TRsp>> Put<TRsp>(string api, object request, params object[] jPath)
        {
            PromiseResponse<TRsp> response;
            try
            {
                response = await ApiCaller.PutAsJson(
                        UriFetcher.BaseUri, api,
                        new RequestHeader
                        {
                            token = await TokenFetcher.GetToken()
                        }, request);
                return response % jPath;

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
                response = await ApiCaller.Delete(UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    });
                return response;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        public async static Task<PromiseResponse<TRsp>> Upload<TRsp>(string api, IEnumerable<NetworkHelper.Model.FileInfo> files, object data, params object[] jPath)
        {

            if (files == null)
                throw new Exception("File list is demanded");
            //HttpContent scontent;
            MultipartFormDataContent form = new MultipartFormDataContent();


            object value;
            foreach (var prop in data.GetType().GetProperties())
            {
                value = prop.GetValue(data);
                if (value == null)
                    continue;
                form.Add(new StringContent(value.ToString()), prop.Name);
            }

            foreach (NetworkHelper.Model.FileInfo item in files)
            {
                byte[] img_data = System.IO.File.ReadAllBytes(item.Path);
                form.Add(new StreamContent(new MemoryStream(img_data)), "file", item.FileName);
                form.Add(new StringContent(item.FileName), "filename");
            }


            PromiseResponse<TRsp> response;
            try
            {
                response = await ApiCaller.Post(UriFetcher.BaseUri, api, new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                }, form, false);
                return response % jPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Please check the Network status!");
            }
        }
    }
}
