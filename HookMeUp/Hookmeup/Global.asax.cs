using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hookmeup.Handlers;
using SS.DL.AzureTableStorage;
using Hookmeup.Models;
using SS.Framework.Common;

namespace Hookmeup
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        OnlineScheduler scheduler = null ;
        const int FREQ_WHO_IS_ONLINE = 5*1000; // 5*1000*60 

        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //CorsConfig.RegisterCors(GlobalConfiguration.Configuration);

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
            IntialiseDb();

            int Offilineinterval = Int32.Parse(Utility.GetHighstPriorityPatameter("OfflineSchedulerInterval"));
            //scheduler = new OnlineScheduler();

            //scheduler.Start(Offilineinterval);


        }

        private void IntialiseDb()
        {
            // IntialiseDb()
            EntityContext.DefaultConfiguration = "RemoteDataStorage";
            EntityContext.setForAppConfig();

        }


        void Application_End(object sender, EventArgs e)
        {

            if (scheduler != null)
            {
                scheduler.Dispose();
                scheduler = null;
            }

            // Force the App to be restarted immediately
            //new OnlineScheduler().PingServer();
        }
        
    }
}