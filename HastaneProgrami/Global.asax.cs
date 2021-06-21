using HastaneProgrami.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HastaneProgrami
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            GlobalFilters.Filters.Add(new ElmahExceptionFilter());
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }
}
