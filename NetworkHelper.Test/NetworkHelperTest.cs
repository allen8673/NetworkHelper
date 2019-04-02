using System;
using System.Collections.Generic;
using DataCacher.GlobalCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkHelper.Extend;
using Newtonsoft.Json;

namespace NetworkHelper.Test
{
    [TestClass]
    public class NetworkHelperTest
    {
        public NetworkHelperTest()
        {
            Global.Host = new Uri("http://192.168.100.235:9000");
            Global.LocalPath = "dev";
            Global.Token = "Bearer 3c1fa688462c30c105df08326406d4fb";
        }

        [TestMethod]
        public void SiccessGet()
        {
            string msg = "";
            List<City> result =
                Promise.Get<List<City>>($"{Global.LocalPath}/api/v1/lists/cities", "City")
                .IfFailure((rsp) => msg = "Failure")
                .IfSuccess((rsp) => msg = "Success")
                .Result;
            Assert.IsTrue(msg == "Success");
        }

        [TestMethod]
        public void FailureGet()
        {
            string msg = "";
            List<City> result = Promise.Get<List<City>>($"{Global.LocalPath}/api/v1/lists/citie", "City")
                .IfFailure((rsp) => msg = "Failure")
                .IfSuccess((rsp) => msg = "Success")
                .Result;
            Assert.IsTrue(msg == "Failure");
        }
    }

    public class City
    {
        [JsonProperty("city_id")]
        public int CityId { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }
    }
}
