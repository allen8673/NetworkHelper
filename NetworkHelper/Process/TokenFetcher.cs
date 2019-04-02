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
        public static string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token)) FetchToken();
                return _token;
            }
        }
        private static string _token;

        public static bool FetchToken()
        {
            try
            {
                _token = string.IsNullOrEmpty(Global.Token) ? null : Global.Token;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }



}
