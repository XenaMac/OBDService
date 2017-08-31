using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CodeService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            SQLCode sql = new SQLCode();
            sql.loadCANCodeBases();
            sql.loadCANRequests();
            sql.loadCANResponses();
            sql.loadServiceLookups();
            sql.loadVehiceServices();
            sql.loadVehicleClassCANs();
            sql.loadVehicles();
            sql.loadVehicleClasses();
            sql.loadVehicleVehicleClasses();
            sql.loadAccelVals();
            sql.loadAccelVehicleClasses();
            //sql.loadVehicleClassCANs();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}