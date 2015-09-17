using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class UserList:DbModel
    {
        public static SelectList GetUserSelectionList(AdGroup group)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Ad/GetUserListByAdGroup?group={group}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);
            return new SelectList(model, "Key", "Value");
        }
    }
}