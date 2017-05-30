using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeService.Web
{
    public partial class ManageClasses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                loadVehicleClasses();
            }
        }

        private void loadVehicleClasses() {
            ddlClasses.Items.Clear();
            ListItem add = new ListItem();
            add.Text = "Add";
            ddlClasses.Items.Add(add);
            foreach (vehicleClass vc in globalData.vehicleClasses) {
                ListItem li = new ListItem();
                li.Text = vc.vehicleClassName;
                li.Value = vc.vehicleClassID.ToString();
                ddlClasses.Items.Add(li);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlClasses.SelectedItem;
            SQLCode sql = new SQLCode();
            vehicleClass vc = new vehicleClass();
            if (li.Text == "Add")
            {
                //new class, just add it to the db and memory list

                vc.vehicleClassID = Guid.NewGuid();
                vc.vehicleClassName = txtClassName.Text;
                sql.updateVehicleClass(vc);
                globalData.vehicleClasses.Add(vc);
            }
            else {
                //this is an update, find it and update it in db and memory
                vehicleClass found = globalData.vehicleClasses.Find(delegate (vehicleClass find) {
                    return find.vehicleClassID == Guid.Parse(li.Value);
                });
                if (found != null) {
                    found.vehicleClassName = txtClassName.Text;
                    for (int i = globalData.vehicleClasses.Count - 1; i >= 0; i--) {
                        if (globalData.vehicleClasses[i].vehicleClassID == Guid.Parse(li.Value)) {
                            globalData.vehicleClasses.RemoveAt(i);
                        }
                    }
                    sql.deleteVehicleClass(found);
                    sql.updateVehicleClass(found);
                }
            }
            loadVehicleClasses();
            Response.Write("Finished");
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlClasses.SelectedItem;
            txtClassName.Text = li.Text;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlClasses.SelectedItem;
            if (li.Text != "Add") {
                vehicleClass vc = globalData.vehicleClasses.Find(delegate (vehicleClass find)
                {
                    return find.vehicleClassID == Guid.Parse(li.Value);
                });
                if (vc != null) {
                    for (int i = globalData.vehicleClasses.Count - 1; i >= 0; i--)
                    {
                        if (globalData.vehicleClasses[i].vehicleClassID == Guid.Parse(li.Value))
                        {
                            globalData.vehicleClasses.RemoveAt(i);
                        }
                    }
                    SQLCode sql = new SQLCode();
                    sql.deleteVehicleClass(vc);
                }
            }
            loadVehicleClasses();
        }
    }
}