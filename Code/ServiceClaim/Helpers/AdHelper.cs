using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Helpers
{
    public class AdHelper
    {
        public static NetworkCredential GetAdUserCredentials()
        {
            string accUserName = @"UN1T\adUnit_prog";
            string accUserPass = "1qazXSW@";

            string domain = "UN1T";//accUserName.Substring(0, accUserName.IndexOf("\\"));
            string name = "adUnit_prog";//accUserName.Substring(accUserName.IndexOf("\\") + 1);

            NetworkCredential nc = new NetworkCredential(name, accUserPass, domain);

            return nc;
        }

        private static NetworkCredential nc = GetAdUserCredentials();

        public static IEnumerable<KeyValuePair<string, string>> GetSpecialistList(AdGroup grp)
        {
            var list = new Dictionary<string, string>();

            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                var domain = new PrincipalContext(ContextType.Domain);
                var group = GroupPrincipal.FindByIdentity(domain, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp));
                if (group != null)
                {
                    var members = group.GetMembers(true);
                    foreach (var principal in members)
                    {
                        var userPrincipal = UserPrincipal.FindByIdentity(domain, principal.SamAccountName);
                        if (userPrincipal != null)
                        {
                            var name = MainHelper.ShortName(userPrincipal.DisplayName);
                            var sid = userPrincipal.Sid.Value;
                            list.Add(sid, name);
                        }
                    }
                }

                return list.OrderBy(x => x.Value);
            }
        }

        public static MailAddress[] GetRecipientsFromAdGroup(AdGroup group)
        {
            var list = new List<MailAddress>();
            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                string sid = AdUserGroup.GetSidByAdGroup(group);
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Sid, sid);

                if (grp != null)
                {
                    foreach (Principal p in grp.GetMembers(true))
                    {
                        string email = new EmployeeSm(p.Sid.Value).Email;
                        if (String.IsNullOrEmpty(email)) continue;
                        list.Add(new MailAddress(email));
                    }
                    grp.Dispose();
                }

                ctx.Dispose();

                return list.ToArray();
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> GetUserListByAdGroup(string grpSid)
        {
            var list = new Dictionary<string, string>();

            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                var domain = new PrincipalContext(ContextType.Domain);
                var group = GroupPrincipal.FindByIdentity(domain, IdentityType.Sid, grpSid);
                if (group != null)
                {
                    var members = group.GetMembers(true);
                    foreach (var principal in members)
                    {
                        var userPrincipal = UserPrincipal.FindByIdentity(domain, principal.SamAccountName);
                        if (userPrincipal != null)
                        {
                            var name = EmployeeSm.ShortName(userPrincipal.DisplayName);
                            var sid = userPrincipal.Sid.Value;
                            list.Add(sid, name);
                        }
                    }
                }
            }

            return list.OrderBy(x => x.Value);
        }

        public static IEnumerable<KeyValuePair<string, string>> GetUserListByAdGroup(AdGroup grp)
        {
            var list = new Dictionary<string, string>();

            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                var domain = new PrincipalContext(ContextType.Domain);
                var group = GroupPrincipal.FindByIdentity(domain, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp));
                if (group != null)
                {
                    var members = group.GetMembers(true);
                    foreach (var principal in members)
                    {
                        var userPrincipal = UserPrincipal.FindByIdentity(domain, principal.SamAccountName);
                        if (userPrincipal != null)
                        {
                            var name = EmployeeSm.ShortName(userPrincipal.DisplayName);
                            var sid = userPrincipal.Sid.Value;
                            list.Add(sid, name);
                        }
                    }
                }
            }

            return list.OrderBy(x => x.Value);
        }
        public static EmployeeSm GetUserBySid(string sid)
        {
            var result = new EmployeeSm();

            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                var context = new PrincipalContext(ContextType.Domain);
                var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, sid);

                if (userPrincipal != null)
                {
                    result.AdSid = sid;
                    result.FullName = userPrincipal.DisplayName;
                    result.DisplayName = EmployeeSm.ShortName(result.FullName);
                }
            }

            return result;
        }

        public static void SetUserAdGroups(IIdentity identity, ref AdUser user)
        {


            //using (WindowsImpersonationContextFacade impersonationContext
            //    = new WindowsImpersonationContextFacade(
            //        nc))
            //{
            var wi = (WindowsIdentity)identity;
            var context = new PrincipalContext(ContextType.Domain);


            if (identity != null && wi.User != null && user != null)
            {

                user.AdGroups = new List<AdGroup>();

                var wp = new WindowsPrincipal(wi);
                foreach (AdUserGroup grp in AdUserGroup.GetList())
                {
                    var grpSid = new SecurityIdentifier(grp.Sid);
                    if (wp.IsInRole(grpSid))
                    {
                        user.AdGroups.Add(grp.Group);
                    }
                }
            }
            //}

        }

        public static bool UserInGroup(IPrincipal user, params AdGroup[] groups)
        {
            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                string fakseLogin = null;

                if (ConfigurationManager.AppSettings["UserProxy"] == "True")
                {
                    fakseLogin = ConfigurationManager.AppSettings["UserProxyLogin"];
                }
                string login = fakseLogin ?? user.Identity.Name;
                var context = new PrincipalContext(ContextType.Domain);
                var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login);

                if (userPrincipal == null) return false;
                if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(AdGroup.SuperAdmin))) { return true; }//Если юзер Суперадмин

                //using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                //using (UserPrincipal user = UserPrincipal.FindByIdentity(context, userName))
                //using (PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups())
                //{
                //    return groups.OfType<GroupPrincipal>().Any(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
                //}

                foreach (var grp in groups)
                {
                    if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp)))
                    {
                        return true;
                    }
                }
                return false;
                //return groups.Select(grp => GroupPrincipal.FindByIdentity(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp))).Where(g => g != null).Any(g => g.GetMembers(true).Cast<UserPrincipal>().Any(usr => usr.SamAccountName == login));
            }
        }

        public static bool UserInGroup(string sid, params AdGroup[] groups)
        {
            using (WindowsImpersonationContextFacade impersonationContext
                = new WindowsImpersonationContextFacade(
                    nc))
            {
                var context = new PrincipalContext(ContextType.Domain);
                var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, sid);

                if (userPrincipal == null) return false;
                ////if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(AdGroup.SuperAdmin))) { return true; }//Если юзер Суперадмин

                foreach (var grp in groups)
                {
                    if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp)))
                    {
                        return true;
                    }
                }


                return false;
            }
        }

        //public static bool UserInGroup(IPrincipal user, params AdGroup[] groups)
        //{
        //    using (WindowsImpersonationContextFacade impersonationContext
        //        = new WindowsImpersonationContextFacade(
        //            nc))
        //    {
        //        var context = new PrincipalContext(ContextType.Domain);
        //        var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, user);

        //        if (userPrincipal == null) return false;
        //        ////if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(AdGroup.SuperAdmin))) { return true; }//Если юзер Суперадмин

        //        foreach (var grp in groups)
        //        {
        //            if (userPrincipal.IsMemberOf(context, IdentityType.Sid, AdUserGroup.GetSidByAdGroup(grp)))
        //            {
        //                return true;
        //            }
        //        }


        //        return false;
        //    }
        //}
    }
}