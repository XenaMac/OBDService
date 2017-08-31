using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeService.Web
{
    public partial class VehiclePID : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                foreach (vehicleService vs in globalData.vehicleServices) {
                    string vn = globalData.vehicles.Where(v => v.vehicleID == vs.vehicleID).Select(n => n.vehicleName).FirstOrDefault();
                    ListItem item = new ListItem();
                    item.Value = vs.vehicleID.ToString();
                    item.Text = vn;
                    ddlMACs.Items.Add(item);
                }
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlMACs.Text)) {
                Response.Write("Please select a vehicle");
                return;
            }
            lblSelectedVehicle.Text = ddlMACs.Text;
            foreach (pidRequest r in globalData.pidRequests) {
                ListItem lo = new ListItem();
                lo.Text = r.requestName;
                lo.Value = r.requestID.ToString();
                chkPIDList.Items.Add(lo);
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            List<vehicleClassCAN> cList = new List<vehicleClassCAN>();
            if (string.IsNullOrEmpty(txtGroupName.Text))
            {
                Response.Write("Please provide a group name");
                txtGroupName.BackColor = System.Drawing.Color.Red;
            }
            else {
                txtGroupName.BackColor = System.Drawing.Color.White;
                foreach (ListItem i in chkPIDList.Items) {
                    if (i.Selected == true) {
                        vehicleClassCAN c = new vehicleClassCAN();
                        c.VehicleClassCANID = Guid.NewGuid();
                        //c.vehicleClassID = lblSelectedVehicle.Text;
                        c.requestID = Guid.Parse(i.Value);
                        c.canRequestGroup = txtGroupName.Text;
                        cList.Add(c);
                    }
                }
                SQLCode sql = new SQLCode();
                if (cList.Count > 0)
                {
                    sql.updateVehicleClassCAN(cList);
                    //sql.loadVehicleCANs();
                    Response.Write("Finished");
                }
            }
        }

        public class pidListObject {
            public Guid requestID { get; set; }
            public string requestName { get; set; }
        }
    }
}