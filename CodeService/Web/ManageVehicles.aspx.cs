using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeService.Web
{
    public partial class ManageVehicles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                loadVehicles();
                loadVehicleClasses();
                loadServiceLookups();
            }
        }

        #region " Loaders "

        private void loadVehicles() {
            ddlVehicles.Items.Clear();
            ListItem nv = new ListItem();
            nv.Text = "New Vehicle";
            ddlVehicles.Items.Add(nv);
            foreach (vehicle v in globalData.vehicles) {
                ListItem li = new ListItem();
                li.Value = v.vehicleID.ToString();
                li.Text = v.vehicleName;
                ddlVehicles.Items.Add(li);
            }
        }

        private void loadVehicleClasses() {
            ddlVehicleClasses.Items.Clear();
            ListItem nc = new ListItem();
            nc.Text = "UNASSOCIATED";
            ddlVehicleClasses.Items.Add(nc);
            foreach (vehicleClass vc in globalData.vehicleClasses) {
                ListItem li = new ListItem();
                li.Text = vc.vehicleClassName;
                li.Value = vc.vehicleClassID.ToString();
                ddlVehicleClasses.Items.Add(li);
            }
        }

        private void loadServiceLookups() {
            ddlServices.Items.Clear();
            ListItem ns = new ListItem();
            ns.Text = "UNASSOCIATED";
            ddlServices.Items.Add(ns);
            foreach (serviceLookup su in globalData.serviceLookups) {
                ListItem li = new ListItem();
                li.Text = su.companyName;
                li.Value = su.serviceLookupID.ToString();
                ddlServices.Items.Add(li);
            }
        }

        #endregion

        #region " Vehicles "

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlVehicles.SelectedItem;
            if (li.Text != "New Vehicle")
            {
                vehicle v = globalData.vehicles.Find(delegate (vehicle find) { return find.vehicleID == new Guid(li.Value); });
                if (v != null)
                {
                    //set up the basics
                    txtVehicleName.Text = v.vehicleName;
                    txtMACAddress.Text = v.MACAddress;
                    //check for existing vehicle class membership
                    vehicleVehicleClass vvc = globalData.vehicleVehicleClasses.Find(delegate (vehicleVehicleClass find) { return find.vehicleID == v.vehicleID; });
                    if (vvc != null) {
                        for (int i = 0; i <= ddlVehicleClasses.Items.Count - 1; i++) {
                            if (ddlVehicleClasses.Items[i].Text != "UNASSOCIATED" && Guid.Parse(ddlVehicleClasses.Items[i].Value) == vvc.vehicleClassID) {
                                ddlVehicleClasses.SelectedIndex = i;
                            }
                        }
                    }
                    //check for existing service membership
                    vehicleService vs = globalData.vehicleServices.Find(delegate (vehicleService find) { return find.vehicleID == v.vehicleID; });
                    if (vs != null) {
                        for (int i = 0; i <= ddlServices.Items.Count - 1; i++) {
                            if (ddlServices.Items[i].Text != "UNASSOCIATED" && Guid.Parse(ddlServices.Items[i].Value) == vs.serviceLookupID) {
                                ddlServices.SelectedIndex = i;
                            }
                        }
                    }
                }
            }
            else {
                txtVehicleName.Text = "New";
                txtMACAddress.Text = "New";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlVehicles.SelectedItem;
            
            SQLCode sql = new SQLCode();
            vehicle v = new vehicle();
            if (li.Text != "New Vehicle")
            {
                v = globalData.vehicles.Find(delegate (vehicle find) { return find.vehicleID == Guid.Parse(li.Value); });
                if (v != null)
                {
                    v.vehicleName = txtVehicleName.Text;
                    v.MACAddress = txtMACAddress.Text;
                }
            }
            else {
                v = new vehicle();
                v.vehicleID = Guid.NewGuid();
                v.MACAddress = txtMACAddress.Text;
                v.vehicleName = txtVehicleName.Text;
                vehicle fFind = globalData.vehicles.Find(delegate (vehicle find) { return find.MACAddress == txtMACAddress.Text; });
                if (fFind != null) {
                    Response.Write("A vehicle with that MAC Address already exists, aborting");
                    return;
                }
                globalData.vehicles.Add(v);
            }
            try {
                sql.updateVehicle(v);
                /*************
                *Vehicle Class
                *************/
                ListItem vclass = (ListItem)ddlVehicleClasses.SelectedItem;
                //check to see if we have selected a vehicle class
                if (vclass.Text != "UNASSOCIATED")
                {
                    //make sure we can find the vehicle class
                    vehicleClass vc = globalData.vehicleClasses.Find(delegate (vehicleClass find)
                    {
                        return find.vehicleClassID == Guid.Parse(vclass.Value);
                    });
                    if (vc != null)
                    {
                        //got a vehicle class. Check for the vehicle/vehicleclass link in vehicleVehicleClass
                        vehicleVehicleClass vvc = globalData.vehicleVehicleClasses.Find(delegate (vehicleVehicleClass find)
                        {
                            return find.vehicleClassID == vc.vehicleClassID && find.vehicleID == v.vehicleID;
                        });
                        //a vehicle can only be in one class, if we have a match, we need to delete the existing data in
                        //memory and db, then add the new information
                        if (vvc != null)
                        {
                            //blow out the existing data
                            sql.deleteVehicleVehicleClass(vvc);
                            for (int i = globalData.vehicleVehicleClasses.Count - 1; i >= 0; i--)
                            {
                                if (globalData.vehicleVehicleClasses[i].vehicleClassID == vc.vehicleClassID &&
                                    globalData.vehicleVehicleClasses[i].vehicleID == v.vehicleID)
                                {
                                    globalData.vehicleVehicleClasses.RemoveAt(i);
                                }
                            }
                            //add the new data
                            vvc = new vehicleVehicleClass();
                            vvc.vehicleClassID = vc.vehicleClassID;
                            vvc.vehicleID = v.vehicleID;
                            sql.updateVehicleVehicleClass(vvc);
                            globalData.vehicleVehicleClasses.Add(vvc);
                        }
                        else {
                            vvc = new vehicleVehicleClass();
                            vvc.vehicleClassID = vc.vehicleClassID;
                            vvc.vehicleID = v.vehicleID;
                            sql.updateVehicleVehicleClass(vvc);
                            globalData.vehicleVehicleClasses.Add(vvc);
                        }
                    }
                }
                else {
                    //the vehicle class is unassociated, located and remove vehicleVehicleClass information for this vehicle
                    vehicleClass vc = globalData.vehicleClasses.Find(delegate (vehicleClass find)
                    {
                        return find.vehicleClassID == Guid.Parse(vclass.Value);
                    });
                    if (vc != null)
                    {
                        //got a vehicle class. Check for the vehicle/vehicleclass link in vehicleVehicleClass
                        vehicleVehicleClass vvc = globalData.vehicleVehicleClasses.Find(delegate (vehicleVehicleClass find)
                        {
                            return find.vehicleClassID == vc.vehicleClassID && find.vehicleID == v.vehicleID;
                        });
                        //a vehicle can only be in one class, if we have a match, we need to delete the existing data in
                        //memory and db, then add the new information
                        if (vvc != null)
                        {
                            //blow out the existing data
                            sql.deleteVehicleVehicleClass(vvc);
                            for (int i = globalData.vehicleVehicleClasses.Count - 1; i >= 0; i--)
                            {
                                if (globalData.vehicleVehicleClasses[i].vehicleClassID == vc.vehicleClassID &&
                                    globalData.vehicleVehicleClasses[i].vehicleID == v.vehicleID)
                                {
                                    globalData.vehicleVehicleClasses.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                /************************
                * Service
                ************************/
                ListItem vService = (ListItem)ddlServices.SelectedItem;
                if (vService.Text != "UNASSOCIATED") {
                    vehicleService vs = globalData.vehicleServices.Find(delegate (vehicleService find) {
                        return find.vehicleID == v.vehicleID;
                    });
                    if (vs != null) {
                        //got an association, delete the old data and replace with new data
                        sql.deleteVehicleService(vs);
                        for (int i = globalData.vehicleServices.Count - 1; i >= 0; i--) {
                            if (globalData.vehicleServices[i].vehicleID == v.vehicleID) {
                                globalData.vehicleServices.RemoveAt(i);
                            }
                        }
                        //create the new information
                        serviceLookup sl = globalData.serviceLookups.Find(delegate (serviceLookup find) {
                            return find.serviceLookupID == Guid.Parse(vService.Value);
                        });
                        if (sl != null) {
                            vs.serviceLookupID = sl.serviceLookupID;
                            sql.updateVehicleService(vs);
                            List<serviceLookup> sList = new List<serviceLookup>();
                            globalData.vehicleServices.Add(vs);
                        }

                    }

                }
                else {
                    vehicleService vs = globalData.vehicleServices.Find(delegate (vehicleService find) {
                        return find.vehicleID == v.vehicleID;
                    });
                    if (vs != null)
                    {
                        //got an association, delete the old data and replace with new data
                        sql.deleteVehicleService(vs);
                        for (int i = globalData.vehicleServices.Count - 1; i >= 0; i--)
                        {
                            if (globalData.vehicleServices[i].vehicleID == v.vehicleID)
                            {
                                globalData.vehicleServices.RemoveAt(i);
                            }
                        }
                    }
                }
                txtMACAddress.Text = string.Empty;
                txtVehicleName.Text = string.Empty;
                loadVehicles();
                Response.Write("Updated vehicle");
            }
            catch (Exception ex) {
                Response.Write(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlVehicles.SelectedItem;
            if (li.Text == "New Vehicle")
            {
                Response.Write("Cannot delete. New Vehicle selected");
                return;
            }
            else {
                vehicle found = globalData.vehicles.Find(delegate (vehicle find) { return find.vehicleID == Guid.Parse(li.Value); });
                if (found != null)
                {
                    SQLCode sql = new SQLCode();
                    sql.deleteVehicle(found);
                    //clear out existing service and vehicleclass links
                    for (int i = globalData.vehicleServices.Count - 1; i >= 0; i--) {
                        if (globalData.vehicleServices[i].vehicleID == Guid.Parse(li.Value)) {
                            globalData.vehicleServices.RemoveAt(i);
                        }
                    }
                    for (int i = globalData.vehicleVehicleClasses.Count - 1; i >= 0; i--) {
                        if (globalData.vehicleVehicleClasses[i].vehicleID == Guid.Parse(li.Value)) {
                            globalData.vehicleVehicleClasses.RemoveAt(i);
                        }
                    }
                    for (int i = globalData.vehicles.Count - 1; i >= 0; i--) {
                        if (globalData.vehicles[i].vehicleID == Guid.Parse(li.Value)) {
                            globalData.vehicles.RemoveAt(i);
                        }
                    }
                    loadVehicles();
                    txtMACAddress.Text = string.Empty;
                    txtVehicleName.Text = string.Empty;
                    Response.Write("Vehicle removed");
                }
                else {
                    Response.Write("Couldn't find vehicle");
                }
            }
        }

        #endregion
    }
}