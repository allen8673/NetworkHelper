using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCacher.GlobalCache
{
    public class Global : GlobalBase
    {
        private static Global _globalCatcher = new Global();
        //public static ILog TLog { get; private set; } = LogManager.GetLogger("TLog");

        private const string _tokenKey = "token";
        private const string _hostKey = "host";
        private const string _localPathKey = "localPath";
        public static string Token
        {
            get
            {
                return Get<string>(_tokenKey);
            }
            set { Set(_tokenKey, value); }
        }

        public static Uri Host
        {
            get
            {
                return Get<Uri>(_hostKey);
            }
            set { Set(_hostKey, value); }
        }

        public static string LocalPath
        {
            get
            {
                return Get<string>(_localPathKey);
            }
            set { Set(_localPathKey, value); }
        }

        private static void Set(string key, object value)
        {
            _globalCatcher.SetItem(key, value);
        }

        private static T Get<T>(string key)
        {
            object value = _globalCatcher.GetItem(key, false);
            if (value != null && value.GetType() != typeof(T)) throw new Exception("Type Error");
            return (T)value;
        }
    }
}
