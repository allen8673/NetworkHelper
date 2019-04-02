using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Model
{
    public class DownloadSetting
    {
        public DownloadProgressChangedEventHandler DownloadProgressChanged { get; set; }
        public AsyncCompletedEventHandler DownloadFileCompleted { get; set; }
        public Uri DownloadUri { get; set; }
        public string DownloadPath { get; set; }
    }
}
