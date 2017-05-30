using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeService.Web
{
    public partial class DataViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                ddlDataSets.Items.Add("Code Bases");
                ddlDataSets.Items.Add("Requests");
                ddlDataSets.Items.Add("Responses");
                ddlDataSets.Items.Add("Services");
                ddlDataSets.Items.Add("Vehicle CAN Data");
                ddlDataSets.Items.Add("Vehicles by Service");
                ddlDataSets.Items.Add("Vehicles");
                ddlDataSets.Items.Add("VehicleVehicleClasses");
                ddlDataSets.Items.Add("VehicleClasses");
                ddlDataSets.Items.Add("AccelVehicleClass");
                ddlDataSets.Items.Add("accelVals");
            }
        }

        protected void btnViewData_Click(object sender, EventArgs e)
        {
            gvData.DataSource = null;
            switch (ddlDataSets.Text) {
                case "Code Bases":
                    gvData.DataSource = globalData.canCodeBases;
                    gvData.DataBind();
                    break;
                case "Requests":
                    gvData.DataSource = globalData.pidRequests;
                    gvData.DataBind();
                    break;
                case "Responses":
                    gvData.DataSource = globalData.pidResponses;
                    gvData.DataBind();
                    break;
                case "Services":
                    gvData.DataSource = globalData.serviceLookups;
                    gvData.DataBind();
                    break;
                case "Vehicle CAN Data":
                    gvData.DataSource = globalData.vehicleClassCANs;
                    gvData.DataBind();
                    break;
                case "Vehicles by Service":
                    gvData.DataSource = globalData.vehicleServices;
                    gvData.DataBind();
                    break;
                case "Vehicles":
                    gvData.DataSource = globalData.vehicles;
                    gvData.DataBind();
                    break;
                case "VehicleVehicleClasses":
                    gvData.DataSource = globalData.vehicleVehicleClasses;
                    gvData.DataBind();
                    break;
                case "VehicleClasses":
                    gvData.DataSource = globalData.vehicleClasses;
                    gvData.DataBind();
                    break;
                case "AccelVehicleClass":
                    gvData.DataSource = globalData.accelVehicleClasses;
                    gvData.DataBind();
                    break;
                case "accelVals":
                    gvData.DataSource = globalData.accelValues;
                    gvData.DataBind();
                    break;
            }
        }
    }
}