using NetworkHelper.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Model
{
    /// <summary>
    /// Helper Settings
    /// </summary>
    static class HelperSettings
    {
        /// <summary>
        /// Webapi timeout
        /// </summary>
        public static TimeSpan ApiTimeout => TimeSpan.FromSeconds(30);
        ///// <summary>
        ///// Url export file path
        ///// </summary>
        //public static string UrlFilePath => InteralHelper.GetUrlFilePath();
    }
}
