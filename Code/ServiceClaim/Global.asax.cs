using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ServiceClaim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //EncryptConfigSection("appSettings");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        //private void EncryptConfigSection(string sectionKey)
        //{
        //    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    ConfigurationSection section = config.GetSection(sectionKey);
        //    if (section != null)
        //    {
        //        if (!section.SectionInformation.IsProtected)
        //        {
        //            if (!section.ElementInformation.IsLocked)
        //            {
        //                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
        //                section.SectionInformation.ForceSave = true;
        //                config.Save(ConfigurationSaveMode.Full);
        //            }
        //        }
        //    }
        //}
    }
}
