using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;

using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;

using Movie.Core;
using Movie.Core.Interfaces;
using Movie.Infrastructure;

namespace Movie.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(config);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.EnsureInitialized();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
           
            //Dipendancy Injection
            var builder = new ContainerBuilder();          
            builder.RegisterControllers(typeof(Global).Assembly);           
            builder.RegisterApiControllers(typeof(Global).Assembly);

            builder.RegisterType<MoviesRepository>()
           .As<IMoviesRepository>()
           .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
        }
    }
}