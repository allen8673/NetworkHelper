using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataCacher.GlobalCache;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetworkHelper.Process
{
    public class TokenFetcher
    {
        public const string TokenErrorMsg = "Fresh token failure";
        internal static Func<string, bool> CheckToken { get; set; } = new Func<string, bool>((token) => true);
        internal static Func<Task<string>> RefreshToken { get; set; } = new Func<Task<string>>(async () => Global.Token);
        public static async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(Global.Token) && !CheckToken(Global.Token))
            {
                ClearToken();
                Global.Token = await RefreshToken();

                if (string.IsNullOrEmpty(Global.Token))
                {
                    throw new Exception(TokenErrorMsg);
                }
            }
            return Global.Token;
        }

        public static void SetToken(string token) => Global.Token = token;

        public static void ClearToken() => Global.Token = null;

        //public static string Token
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_token)) FetchToken();
        //        return _token;
        //    }
        //}
        //private static string _token;

        //public static bool FetchToken()
        //{
        //    try
        //    {
        //        _token = string.IsNullOrEmpty(Global.Token) ? null : Global.Token;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }



}
