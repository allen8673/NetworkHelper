using NetworkHelper.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Model
{
    /// <summary>
    /// Request Header Content
    /// </summary>
    public class RequestHeader
    {
        #region Header Content; Add properties if something demanded.
        /// <summary>
        /// Token
        /// </summary>
        [Hearder("Authorization")]
        public string token { get; set; } 
        #endregion

        /// <summary>
        /// Set/Get? Property Value By Mapping Key 
        /// </summary>
        /// <param name="key">Header Key</param>
        /// <returns></returns>
        private string this[string key]
        {
            set
            {
                this.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<HearderAttribute>().Key == key)?
                    .SetValue(this, value);
            }
        }

    }

    /// <summary>
    /// Request / Response Header Mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class HearderAttribute : Attribute
    {
        public HearderAttribute(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Header Key
        /// </summary>
        public string Key { get; private set; }
    }
}
