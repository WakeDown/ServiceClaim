using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public class AdUserGroup
    {
        public AdGroup Group { get; set; }
        public string Sid { get; set; }
        public string Name { get; set; }

        public AdUserGroup(AdGroup grp, string sid, string name)
        {
            Group = grp;
            Sid = sid;
            Name = name;
        }

        public static IEnumerable<AdUserGroup> GetList()
        {
            var list = new List<AdUserGroup>();
            list.Add(new AdUserGroup(AdGroup.ZipClaimClient, "S-1-5-21-1970802976-3466419101-4042325969-3639", "zipclaim-client"));
            list.Add(new AdUserGroup(AdGroup.ZipClaimClientCounterView, "S-1-5-21-1970802976-3466419101-4042325969-4219", "zipclaim-client-counter-view"));
            list.Add(new AdUserGroup(AdGroup.ZipClaimClientZipView, "S-1-5-21-1970802976-3466419101-4042325969-4218", "zipclaim-client-zip-view"));
            list.Add(new AdUserGroup(AdGroup.SuperAdmin, "S-1-5-21-1970802976-3466419101-4042325969-4031", "SuperAdmin"));//Суперадмин
            list.Add(new AdUserGroup(AdGroup.ServiceAdmin, "S-1-5-21-1970802976-3466419101-4042325969-2566", "ServiceAdmin"));
            list.Add(new AdUserGroup(AdGroup.ServiceManager, "S-1-5-21-1970802976-3466419101-4042325969-2567", "ServiceManager"));
            list.Add(new AdUserGroup(AdGroup.ServiceEngeneer, "S-1-5-21-1970802976-3466419101-4042325969-2558", "ServiceEngeneer"));
            list.Add(new AdUserGroup(AdGroup.ServiceOperator, "S-1-5-21-1970802976-3466419101-4042325969-2568", "ServiceOperator"));
            list.Add(new AdUserGroup(AdGroup.ServiceControler, "S-1-5-21-1970802976-3466419101-4042325969-4066", "ServiceControler"));
            list.Add(new AdUserGroup(AdGroup.ServiceTech, "S-1-5-21-1970802976-3466419101-4042325969-4079", "ServiceTech"));
            list.Add(new AdUserGroup(AdGroup.ServiceClaimContractorAccess, "S-1-5-21-1970802976-3466419101-4042325969-4092", "ServiceClaimContractorAccess"));
            //---
            list.Add(new AdUserGroup(AdGroup.ServiceClaimClassifier, "S-1-5-21-1970802976-3466419101-4042325969-4081", "ServiceClaimClassifier"));
            list.Add(new AdUserGroup(AdGroup.ServiceClaimClientAccess, "S-1-5-21-1970802976-3466419101-4042325969-4082", "ServiceClaimClientAccess"));

            return list;
        }

        public static string GetSidByAdGroup(AdGroup grp)
        {
            return GetList().Single(g => g.Group == grp).Sid;
        }

        public static AdGroup GetAdGroupBySid(string sid)
        {
            if (string.IsNullOrEmpty(sid)) return AdGroup.None;
            var grp = GetList().Single(g => g.Sid == sid).Group;
            return grp;
        }
    }
}