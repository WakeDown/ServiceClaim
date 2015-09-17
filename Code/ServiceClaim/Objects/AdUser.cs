using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ServiceClaim.Helpers;

namespace ServiceClaim.Objects
{
    public class AdUser
    {
        public string Sid { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DisplayName
        {
            get { return MainHelper.ShortName(FullName); }
        }
        //public List<AdGroup> AdGroups { get; set; }

        //private static string GetShortName(string name)
        //{

        //    if (String.IsNullOrEmpty(name)) return "Имя отсутствует";
        //    var shortName = new StringBuilder();
        //    string res = String.Empty;
        //    var partNames = name.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //    if (partNames.Count() > 2)
        //    {
        //        shortName.Append(partNames[0]);
        //        shortName.Append(" ");
        //        shortName.Append(partNames[1].Substring(0, 1));
        //        shortName.Append(".");
        //        shortName.Append(partNames[2].Substring(0, 1));
        //        shortName.Append(".");

        //        res = shortName.ToString();
        //    }
        //    else if (partNames.Count() == 2)
        //    {
        //        shortName.Append(partNames[0]);
        //        shortName.Append(" ");
        //        shortName.Append(partNames[1].Substring(0, 1));
        //        shortName.Append(".");

        //        res = shortName.ToString();
        //    }
        //    else
        //    {
        //        res = name;
        //    }
        //    return res;
        //}

        public bool Is(params AdGroup[] groups)
        {
            bool result = false;

            if (String.IsNullOrEmpty(Sid)) return false;
            result = AdHelper.UserInGroup(Sid, groups);
            //foreach (AdGroup group in groups)
            //{
            //    result = AdGroups.Contains(group);
            //    if (result) break;
            //}
            return result;
        }

        public bool HasAccess(params AdGroup[] groups)
        {
            bool result = false;
            if (String.IsNullOrEmpty(Sid)) return false;
            if (AdHelper.UserInGroup(Sid, AdGroup.SuperAdmin)) return true;
            result = AdHelper.UserInGroup(Sid, groups);
            return result;
        }

        public bool UserIsAdmin()
        {
            if (String.IsNullOrEmpty(Sid)) return false;
            return HasAccess(AdGroup.SuperAdmin);
        }

        
    }
}