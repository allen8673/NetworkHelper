using NetworkHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Process
{
    /// <summary>
    /// WebApi Caller
    /// </summary>
    public class ApiCaller
    {
        /// <summary>
        /// Get/Past Data by Http Get
        /// </summary>
        /// <param name="apiUrl">Api URL</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(Uri baseUri, string apiUrl, RequestHeader hearder)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseCookies = false }) { Timeout = HelperSettings.ApiTimeout })
            {
                // *** Create Cookies Ref:http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage ***
                client.BaseAddress = baseUri;
                var message = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate
                { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    message.Headers.Add(key, value.ToString());
                }
                //return client.SendAsync(message).Result;
                return await client.SendAsync(message).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// Get/Past Data by Http Post
        /// </summary>
        /// <param name="baseUri">Base URI</param>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="hearder">Cookie Header Content</param>
        /// <param name="content">Response Content</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post(Uri baseUri, string apiUrl, RequestHeader hearder, HttpContent content, bool contentType = true/* string contentType = "application/json"*/)
        {
            using (var client = new HttpClient(new HttpClientHandler { }) { Timeout = HelperSettings.ApiTimeout })
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                if (contentType)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                //if (!string.IsNullOrEmpty(contentType))
                //{
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //}
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    client.DefaultRequestHeaders.Add(key, value.ToString());
                }
                //var result = client.PostAsync(apiUrl, content).Result;
                return await client.PostAsync(apiUrl, content).ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Get/Past Data by Http Post
        /// </summary>
        /// <param name="baseUri">Base URI</param>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="hearder">Cookie Header Content</param>
        /// <param name="content">Response Content</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post(
            Uri baseUri, string apiUrl,
            MediaTypeWithQualityHeaderValue mediaType,
            RequestHeader hearder, HttpContent content)
        {
            using (var client = new HttpClient(new HttpClientHandler { }) { Timeout = HelperSettings.ApiTimeout })
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                if (mediaType != null)
                    client.DefaultRequestHeaders.Accept.Add(mediaType);
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                object value;
                string key;
                foreach (var prop in hearder?.GetType().GetProperties() ?? new PropertyInfo[] { })
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    client.DefaultRequestHeaders.Add(key, value.ToString());
                }
                //var result = client.PostAsync(apiUrl, content).Result;
                return await client.PostAsync(apiUrl, content);
            }
        }

        /// <summary>
        /// Get/Past Data by Http Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseUri">Base URI</param>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="hearder">Cookie Header Content</param>
        /// <param name="data">Pasted Data</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsJson<T>(Uri baseUri, string apiUrl, RequestHeader hearder, T data)
        {

            using (var client = new HttpClient(new HttpClientHandler { }) { Timeout = HelperSettings.ApiTimeout })
            {
                // *** Create Cookies Ref:http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage ***
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate
                { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    client.DefaultRequestHeaders.Add(key, value.ToString());
                }
                return await client.PostAsJsonAsync(apiUrl, data);
            }
        }

        /// <summary>
        /// Get/Past Data by Http Post
        /// </summary>
        /// <param name="baseUri">Base URI</param>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="hearder">Cookie Header Content</param>
        /// <param name="content">Response Content</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Put(Uri baseUri, string apiUrl, RequestHeader hearder, HttpContent content)
        {
            using (var client = new HttpClient(new HttpClientHandler { }) { Timeout = HelperSettings.ApiTimeout })
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate
                { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    client.DefaultRequestHeaders.Add(key, value.ToString());
                }
                //var result = client.PutAsync(apiUrl, content).Result;
                return await client.PutAsync(apiUrl, content);
            }
        }

        /// <summary>
        /// Get/Past Data by Http Post
        /// </summary>
        /// <param name="baseUri">Base URI</param>
        /// <param name="apiUrl">Api URL</param>
        /// <param name="hearder">Cookie Header Content</param>
        /// <param name="content">Response Content</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAsJson<T>(Uri baseUri, string apiUrl, RequestHeader hearder, T data)
        {
            using (var client = new HttpClient(new HttpClientHandler { }) { Timeout = HelperSettings.ApiTimeout })
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate
                { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    client.DefaultRequestHeaders.Add(key, value.ToString());
                }
                //var result = client.PutAsJsonAsync(apiUrl, data).Result;
                return await client.PutAsJsonAsync(apiUrl, data);
            }
        }

        /// <summary>
        /// Past Data by Http Delete
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="apiUrl"></param>
        /// <param name="hearder"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Delete(Uri baseUri, string apiUrl, RequestHeader hearder)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseCookies = false }) { Timeout = HelperSettings.ApiTimeout })
            {
                // *** Create Cookies Ref:http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage ***
                client.BaseAddress = baseUri;
                var message = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate
                { return true; });
                object value;
                string key;
                foreach (var prop in hearder.GetType().GetProperties())
                {
                    if (( value = prop.GetValue(hearder) ) == null)
                        continue;
                    key = prop.GetCustomAttribute<HearderAttribute>().Key;
                    message.Headers.Add(key, value.ToString());
                }
                //return client.SendAsync(message).Result;
                return await client.SendAsync(message).ConfigureAwait(false);
            }

        }
    }
}
