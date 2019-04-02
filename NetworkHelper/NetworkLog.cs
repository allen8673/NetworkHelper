using NetworkHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper
{
    public class NetworkLog
    {
        public static Action<Exception> ErrorLog { get; set; }
        public static Action<RequestInfo> ResponseLog { get; set; }

        public static void RegisterLogAction(Action<RequestInfo> responseLog, Action<Exception> errorLog)
        {
            ResponseLog = responseLog;
            ErrorLog = errorLog;
        }
    }
}
