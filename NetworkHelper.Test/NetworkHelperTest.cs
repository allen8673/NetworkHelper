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
            Global.Host = new Uri("http://140.110.141.72:3003");
            Global.LocalPath = "dev";
            Global.Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJvY2MiLCJ1c2VyX2lkIjoxMDMsInJvbGVfaWQiOjEsIm9yZ2FuaXphdGlvbl9pZCI6MTA1LCJvcmdhbml6YXRpb25fbmFtZSI6Im9jY2JhY2tlbmQiLCJvcmdhbml6YXRpb25fZW1haWwiOiJvY2NiYWNrZW5kQGdtYWlsLmNvbSIsIm9yZ2FuaXphdGlvbl9jaXRpZXMiOlt7ImNpdHlfaWQiOjF9LHsiY2l0eV9pZCI6Mn0seyJjaXR5X2lkIjozfV0sImlhdCI6MTU2MDkzMzY5NiwiZXhwIjoxNTYwOTM3Mjk2fQ.zTVSQGoqtlkPBMmUMJspI9d_uzgeZjJlJ3e4fAYmmUA";
        }

        [TestMethod]
        public void SuccessConnecterGet()
        {
            string msg = "";
            List<City> result = Connecter.Get<List<City>>($"{Global.LocalPath}/api/v1/lists/cities", null, "City").Result;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void SuccessGet()
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
