using DataCacher.GlobalCache;
using NetworkHelper.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Process
{
    public static class UriFetcher
    {
        public static Uri BaseUri => Global.Host;
    }
}
