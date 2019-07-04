using DataCacher.GlobalCache;
using NetworkHelper.Model;
using NetworkHelper.Process;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper
{
    public class Connecter
    {

        //public static Action<RequestInfo> ResponseLog { get; set; }
        //public static Action<Exception> ErrorLog { get; set; }
        private static object _locker = new { };

        public static void SetBaseUri(Uri uri)
        {
            Global.Host = new Uri(uri.GetLeftPart(UriPartial.Authority));
            Global.LocalPath = uri.LocalPath;
        }

        public static void SetToken(string token) => TokenFetcher.SetToken(token);
        public static void RegisterCheckToken(Func<string, bool> method) => TokenFetcher.CheckToken = method;
        public static void RegisterRefreshToken(Func<Task<string>> method) => TokenFetcher.RefreshToken = method;


        public static string LocalPath => Global.LocalPath;

        #region The POST Methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRsp"></typeparam>
        /// <param name="api"></param>
        /// <param name="mediaType"></param>
        /// <param name="requestHeader"></param>
        /// <param name="httpContent"></param>
        /// <param name="failureAct"></param>
        /// <returns></returns>
        public async static Task<TRsp> Post<TRsp>(Uri baseUri, string api,
            MediaTypeWithQualityHeaderValue mediaType,
            RequestHeader requestHeader,
            HttpContent httpContent,
            Func<HttpResponseMessage, TRsp> failureAct = null,
            params object[] jPath)
        {
            HttpResponseMessage response = await ApiCaller.Post(
           baseUri, api,
           mediaType,
           requestHeader,
           httpContent);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return failureAct(response);
            return ParseJPath<TRsp>(content, jPath);
            //return JsonConvert.DeserializeObject<TRsp>(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRsp"></typeparam>
        /// <param name="api"></param>
        /// <param name="mediaType"></param>
        /// <param name="requestHeader"></param>
        /// <param name="httpContent"></param>
        /// <param name="failureAct"></param>
        /// <returns></returns>
        public async static Task<TRsp> Post<TRsp>(string api,
            MediaTypeWithQualityHeaderValue mediaType,
            RequestHeader requestHeader,
            HttpContent httpContent,
            Func<HttpResponseMessage, TRsp> failureAct = null,
            params object[] jPath)
        {

            return Post<TRsp>(Global.Host, api, mediaType, requestHeader, httpContent, failureAct, jPath).Result;
        }

        /// <summary>
        /// Restful Post function
        /// </summary>
        /// <param name="api">Post api url</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<bool> PostJson(string api, object request)
        {
            RequestInfo requestUrl = new RequestInfo
            {
                HttpMethod = HttpMethod.Post,
                RequestUrl = $"{ UriFetcher.BaseUri}{api}",
                RequestContent = JsonConvert.SerializeObject(request)
            };
            try
            {
                HttpResponseMessage response = await ApiCaller.PostAsJson(
                    UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    }, request);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestUrl.ResponseContent = content;
                requestUrl.SetHttpStatus(response.StatusCode);
                NetworkLog.ResponseLog?.Invoke(requestUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        /// <summary>
        /// Restful Post function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Post api url</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<TRsp> PostJson<TRsp>(string api, object request, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            RequestInfo requestUrl = new RequestInfo
            {
                HttpMethod = HttpMethod.Post,
                RequestUrl = $"{UriFetcher.BaseUri}{api}",
                RequestContent = JsonConvert.SerializeObject(request)
            };
            try
            {
                HttpResponseMessage response = await ApiCaller.PostAsJson(
                    UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    }, request);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestUrl.ResponseContent = content;
                requestUrl.SetHttpStatus(response.StatusCode);
                NetworkLog.ResponseLog?.Invoke(requestUrl);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);
                return ParseJPath<TRsp>(content, jPath);
                //return JsonConvert.DeserializeObject<TRsp>(content);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Restful Post function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="baseUri">Base Uri</param>
        /// <param name="api">Post api url</param>
        /// <param name="request">Request content</param>
        /// <returns></returns>
        public async static Task<TRsp> PostJson<TRsp>(Uri baseUri, string api, object request, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            RequestInfo requestUrl = new RequestInfo
            {
                HttpMethod = HttpMethod.Post,
                RequestUrl = $"{baseUri}{api}",
                RequestContent = JsonConvert.SerializeObject(request)
            };
            try
            {
                HttpResponseMessage response = await ApiCaller.PostAsJson(
                    baseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    }, request);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestUrl.ResponseContent = content;
                requestUrl.SetHttpStatus(response.StatusCode);
                NetworkLog.ResponseLog?.Invoke(requestUrl);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);
                return ParseJPath<TRsp>(content, jPath);
                //return JsonConvert.DeserializeObject<TRsp>(content);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }
        #endregion

        #region The GET Methods
        /// <summary>
        /// Restful Get function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<TRsp> Get<TRsp>(string api, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            try
            {
                HttpResponseMessage response = await ApiCaller.Get(UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);

                return ParseJPath<TRsp>(content, jPath);
                //if (jPath.Count() == 0)
                //    return JsonConvert.DeserializeObject<TRsp>(content);
                //JToken jobject = JObject.Parse(content);
                //foreach (object jpath in jPath)
                //{
                //    jobject = jobject[jpath];
                //}
                //return jobject.ToObject<TRsp>();
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        /// <summary>
        /// Restful Get function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="baseUri">Base uri</param>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<TRsp> Get<TRsp>(Uri baseUri, string api, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            try
            {
                HttpResponseMessage response = await ApiCaller.Get(baseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);

                return ParseJPath<TRsp>(content, jPath);
                //return JsonConvert.DeserializeObject<TRsp>(content);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        

        #endregion

        #region The PUT Methods
        /// <summary>
        /// Restful Put function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Put api</param>
        /// <param name="request">Request content</param>
        /// <param name="failureAct">Failure callback</param>
        /// <returns></returns>
        public async static Task<TRsp> Put<TRsp>(string api, object request, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            RequestInfo requestUrl = new RequestInfo
            {
                HttpMethod = HttpMethod.Put,
                RequestUrl = $"{UriFetcher.BaseUri}{api}",
                RequestContent = JsonConvert.SerializeObject(request)
            };

            try
            {
                HttpResponseMessage response = await ApiCaller.PutAsJson(
                    UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    }, request);

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestUrl.ResponseContent = content;
                requestUrl.SetHttpStatus(response.StatusCode);
                NetworkLog.ResponseLog?.Invoke(requestUrl);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);

                return ParseJPath<TRsp>(content, jPath);
                //return JsonConvert.DeserializeObject<TRsp>(content);
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Restful Put function
        /// </summary>
        /// <param name="api">Put api</param>
        /// <param name="request">Request content</param>
        /// <param name="failureAct">Failure callback</param>
        /// <returns></returns>
        public async static Task<bool> Put(string api, object request, Action<HttpResponseMessage> failureAct = null)
        {
            RequestInfo requestUrl = new RequestInfo
            {
                HttpMethod = HttpMethod.Put,
                RequestUrl = $"{UriFetcher.BaseUri}{api}",
                RequestContent = JsonConvert.SerializeObject(request)
            };

            try
            {
                HttpResponseMessage response = await ApiCaller.PutAsJson(
                    UriFetcher.BaseUri, api,
                    new RequestHeader
                    {
                        token = await TokenFetcher.GetToken()
                    }, request);

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestUrl.ResponseContent = content;
                requestUrl.SetHttpStatus(response.StatusCode);
                NetworkLog.ResponseLog?.Invoke(requestUrl);
                if (!response.IsSuccessStatusCode)
                    ( failureAct ?? new Action<HttpResponseMessage>((rsp) => { }) )(response);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// Restful Delete function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<TRsp> Delete<TRsp>(string api, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            try
            {
                HttpResponseMessage response = await ApiCaller.Delete(UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return ( failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)) )(response);
                return ParseJPath<TRsp>(content, jPath);
                //if (jPath.Count() == 0)
                //    return JsonConvert.DeserializeObject<TRsp>(content);
                //JToken jobject = JObject.Parse(content);
                //foreach (object jpath in jPath)
                //{
                //    jobject = jobject[jpath];
                //}
                //return jobject.ToObject<TRsp>();
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        /// <summary>
        /// Restful Delete function
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Get api</param>
        /// <returns></returns>
        public async static Task<bool> Delete(string api)
        {
            try
            {
                HttpResponseMessage response = await ApiCaller.Delete(UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                });
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                NetworkLog.ErrorLog?.Invoke(ex);
                throw new Exception("Please check the Network status!");
            }
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Upload Api url</param>
        /// <param name="files">Upload file list</param>
        /// <returns></returns>
        public async static Task<TRsp> Upload<TRsp>(string api, IEnumerable<Model.FileInfo> files, Func<HttpResponseMessage, TRsp> failureAct = null, params object[] jPath)
        {
            if (files == null)
                throw new Exception("File list is demanded");
            HttpContent scontent;
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (Model.FileInfo item in files)
            {
                FileStream data = File.OpenRead(item.Path);
                scontent = new StreamContent(data);
                scontent.Headers.Add("Content-Type", "application/octet-stream");
                scontent.Headers.Add("Content-Disposition", "form-data; name=\"files\"; filename=\"" + item.FileName + "\"");
                form.Add(scontent, "file", item.FileName);
            }

            HttpResponseMessage response = await ApiCaller.Post(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                }, form);

            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return (failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)))(response);
            return ParseJPath<TRsp>(content, jPath);
            //if (!response.IsSuccessStatusCode)
            //    return failureAct(response);
            //return JsonConvert.DeserializeObject<TRsp>(content);
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <typeparam name="TRsp">Result generic type</typeparam>
        /// <param name="api">Upload Api url</param>
        /// <param name="files">Upload file list</param>
        /// <returns></returns>
        public async static Task<TRsp> Upload<TRsp>(string api,
            IEnumerable<Model.FileInfo> files,
            object data,
            Func<HttpResponseMessage, TRsp> failureAct = null,
            params object[] jPath)
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

            foreach (Model.FileInfo item in files)
            {
                byte[] img_data = System.IO.File.ReadAllBytes(item.Path);
                form.Add(new StreamContent(new MemoryStream(img_data)), "img_file", item.FileName);
            }

            {
                HttpResponseMessage response = await ApiCaller.Post(
                UriFetcher.BaseUri, api,
                new RequestHeader
                {
                    token = await TokenFetcher.GetToken()
                }, form, false);

                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return (failureAct ?? new Func<HttpResponseMessage, TRsp>((rsp) => default(TRsp)))(response);
                return ParseJPath<TRsp>(content, jPath);
                //if (!response.IsSuccessStatusCode)
                //    return failureAct(response);
                //return JsonConvert.DeserializeObject<TRsp>(content);
            }
        }

        public async static Task Download(DownloadSetting downloadSetting)
        {
            WebClient _Client = new WebClient();
            _Client.DownloadProgressChanged += downloadSetting.DownloadProgressChanged;
            _Client.DownloadFileCompleted += downloadSetting.DownloadFileCompleted;
            await _Client.DownloadFileTaskAsync(downloadSetting.DownloadUri, downloadSetting.DownloadPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRsp"></typeparam>
        /// <param name="content"></param>
        /// <param name="jPath"></param>
        /// <returns></returns>
        public static TRsp ParseJPath<TRsp>(string content, params object[] jPath)
        {
            if (jPath.Count() == 0)
                return JsonConvert.DeserializeObject<TRsp>(content);
            JToken jobject = JObject.Parse(content);
            foreach (object jpath in jPath)
            {
                jobject = jobject[jpath];
            }
            return jobject.ToObject<TRsp>();
        }

        //public static bool GetToken()
        //{
        //    return TokenFetcher.FetchToken();
        //}

        //public class Promise
        //{
        //    /// <summary>
        //    /// Restful Post function
        //    /// </summary>
        //    /// <typeparam name="TRsp">Result generic type</typeparam>
        //    /// <param name="api">Post api url</param>
        //    /// <param name="request">Request content</param>
        //    /// <returns></returns>
        //    public async static Task<PromiseResponse<TRsp>> PostJson<TRsp>(string api, object request)
        //    {
        //        try
        //        {
        //            return await ApiCaller.PostAsJson(
        //                UriFetcher.BaseUri, api,
        //                new RequestHeader
        //                {
        //                    token = TokenFetcher.Token
        //                }, request);

        //        }
        //        catch (Exception ex)
        //        {
        //            NetworkLog.ErrorLog?.Invoke(ex);
        //            return ex;
        //        }
        //    }

        //    /// <summary>
        //    /// Restful Get function
        //    /// </summary>
        //    /// <typeparam name="TRsp">Result generic type</typeparam>
        //    /// <param name="api">Get api</param>
        //    /// <param name="jPath">Parse jParh for Response json</param>
        //    /// <returns></returns>
        //    public async static Task<PromiseResponse<TRsp>> Get<TRsp>(string api, params object[] jPath)
        //    {
        //        PromiseResponse<TRsp> response;
        //        try
        //        {
        //            response = await ApiCaller.Get(UriFetcher.BaseUri, api,
        //            new RequestHeader
        //            {
        //                token = TokenFetcher.Token
        //            });

        //            return response % jPath;
        //        }
        //        catch (Exception ex)
        //        {
        //            NetworkLog.ErrorLog?.Invoke(ex);
        //            return ex;
        //        }
        //    }

        //    /// <summary>
        //    /// Restful Put function
        //    /// </summary>
        //    /// <typeparam name="TRsp">Result generic type</typeparam>
        //    /// <param name="api">Put api</param>
        //    /// <param name="request">Request content</param>
        //    /// <param name="failureAct">Failure callback</param>
        //    /// <returns></returns>
        //    public async static Task<PromiseResponse<TRsp>> Put<TRsp>(string api, object request)
        //    {
        //        try
        //        {
        //            return await ApiCaller.PutAsJson(
        //                UriFetcher.BaseUri, api,
        //                new RequestHeader
        //                {
        //                    token = TokenFetcher.Token
        //                }, request);
        //            //return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            NetworkLog.ErrorLog?.Invoke(ex);
        //            return ex;
        //        }
        //    }

        //    /// <summary>
        //    /// Restful Delete function
        //    /// </summary>
        //    /// <typeparam name="TRsp">Result generic type</typeparam>
        //    /// <param name="api">Get api</param>
        //    /// <returns></returns>
        //    public async static Task<PromiseResponse> Delete(string api)
        //    {
        //        PromiseResponse response;
        //        try
        //        {
        //            response = await ApiCaller.Delete(UriFetcher.BaseUri, api,
        //            new RequestHeader
        //            {
        //                token = TokenFetcher.Token
        //            });
        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            NetworkLog.ErrorLog?.Invoke(ex);
        //            throw new Exception("Please check the Network status!");
        //        }
        //    }
        //}
    }
}
