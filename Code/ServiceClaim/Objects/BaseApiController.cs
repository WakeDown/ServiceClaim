using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using ServiceClaim.Helpers;

namespace ServiceClaim.Objects
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {

            //SetCurUser();
        }

        protected AdUser GetCurUser()
        {
            var session = HttpContext.Current.Session;

            if (session?["CurUser"] != null)
            {
                return (AdUser)session["CurUser"];
            }

            //object context;
            //if (Request.Properties.TryGetValue("MS_HttpContext", out context))
            //{
            //    var httpContext = context as HttpContextBase;
            //    if (httpContext != null && httpContext.Session != null)
            //    {
            //        if (httpContext.Session?["CurUser"] != null)
            //        {
            //            return (AdUser)httpContext.Session["CurUser"];
            //        }
            //    }
            //}

            AdUser curUser = new AdUser();
            curUser.User = base.RequestContext.Principal;


            string sid = null;
            var wi = (WindowsIdentity)RequestContext.Principal.Identity;
            if (wi.User != null)
            {
                var domain = new PrincipalContext(ContextType.Domain);
                sid = wi.User.Value;

                if (ConfigurationManager.AppSettings["UserProxy"] == "True")
                {
                    sid = ConfigurationManager.AppSettings["UserProxySid"];
                }

                curUser.Sid = sid;

                AdHelper.SetUserAdGroups(wi, ref curUser);
            }
            if (session != null)
            {
                session["CurUser"] = curUser;
            }

            //if (Request.Properties.TryGetValue("MS_HttpContext", out context))
            //{
            //    var httpContext = context as HttpContextBase;
            //    if (httpContext != null && httpContext.Session != null)
            //    {
            //        httpContext.Session["CurUser"] = curUser;
            //    }
            //}

            return curUser;
        }
    }
}