using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClaimStateGroup:DbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public int OrderNum { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public int ClaimCount { get; set; }

        public static IEnumerable<ClaimStateGroup> GetFilterList()
        {
            Uri uri = new Uri(String.Format("{0}/ClaimState/GetGroupFilterList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimStateGroup>>(jsonString);
            return model;
        }
    }
}