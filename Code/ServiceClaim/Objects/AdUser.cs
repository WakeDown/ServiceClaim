using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using ServiceClaim.Helpers;

namespace ServiceClaim.Objects
{
    public class AdUser
    {
        public IPrincipal User { get; set; }
        public string Sid { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DisplayName => MainHelper.ShortName(FullName);
        public List<AdGroup> AdGroups { get; set; }
        public bool IsAnonymous { get; set; }

        public AdUser()
        {
            IsAnonymous = false;
        }

        public AdUser(string sid, bool isAnonymous)
        {
            var user = AdHelper.GetUserBySid(sid);
            Sid = user.AdSid;
            FullName = user.FullName;
            Email = user.Email;
            IsAnonymous = isAnonymous;
        }

        public bool Is(params AdGroup[] groups)
        {
            if (IsAnonymous) return false;
            return groups.Select(grp => AdGroups.Contains(grp)).Any(res => res);
            //bool result = false;

            //if (String.IsNullOrEmpty(Sid)) return false;
            //result = AdHelper.UserInGroup(Sid, groups);
            ////foreach (AdGroup group in groups)
            ////{
            ////    result = AdGroups.Contains(group);
            ////    if (result) break;
            ////}
            //return result;
        }

        public bool HasAccess(params AdGroup[] groups)
        {
            if (IsAnonymous) return false;
            if (AdGroups == null || !AdGroups.Any()) return false;
            if (AdGroups.Contains(AdGroup.SuperAdmin) || AdGroups.Contains(AdGroup.ServiceControler)) return true;
            return groups.Select(grp => AdGroups.Contains(grp)).Any(res => res);
            //bool result = false;
            //if (String.IsNullOrEmpty(Sid)) return false;
            //if (AdHelper.UserInGroup(Sid, AdGroup.SuperAdmin)) return true;
            //if (AdHelper.UserInGroup(Sid, AdGroup.ServiceControler)) return true;
            //result = AdHelper.UserInGroup(Sid, groups);
            //return result;
        }

        public bool UserIsAdmin()
        {
            if (String.IsNullOrEmpty(Sid)) return false;
            return HasAccess(AdGroup.SuperAdmin);
        }

        
    }
}