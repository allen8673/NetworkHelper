using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHelper.Model
{
    /// <summary>
    /// Uri Information
    /// </summary>
    public class UriInfo
    {
        [JsonProperty("region_id")]
        public int RegionId { get; set; }
        [JsonProperty("region_name")]
        public string RegionName { get; set; }
        [JsonProperty("server_ip")]
        public string ServerIp { get; set; }
        [JsonProperty("server_protocol")]
        public string ServerProtocol { get; set; }
        public string ServerUri => $"{(string.IsNullOrEmpty(ServerProtocol) ? "https" : ServerProtocol)}://{ServerIp}";
    }

    public class UriInfoCompare : IEqualityComparer<UriInfo>
    {
        public bool Equals(UriInfo x, UriInfo y)
        {
            return x.RegionId == y.RegionId;
        }

        public int GetHashCode(UriInfo obj)
        {
            return base.GetHashCode();
        }
    }
}
