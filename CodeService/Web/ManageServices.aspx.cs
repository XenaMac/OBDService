using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeService.Web
{
    public partial class ManageServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                loadServices();
            }
        }

        private void loadServices() {
            ddlServices.Items.Clear();
            foreach (serviceLookup sl in globalData.serviceLookups) {
                ListItem li = new ListItem();
                li.Text = sl.companyName;
                li.Value = sl.serviceLookupID.ToString();
                ddlServices.Items.Add(li);
            }
        }

        protected void btnSelectService_Click(object sender, EventArgs e)
        {
            ListItem li = (ListItem)ddlServices.SelectedItem;
            serviceLookup sl = globalData.serviceLookups.Find(delegate (serviceLookup find) {
                return find.serviceLookupID == new Guid(li.Value);
            });
            if (sl != null) {
                lblSelectedService.Text = sl.companyName;
                txtCompanyName.Text = sl.companyName;
                txtConnString.Text = sl.connString;
                txtServiceURL.Text = sl.serviceURL;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblSelectedService.Text != "NONE") {
                SQLCode sql = new SQLCode();
                ListItem li = (ListItem)ddlServices.SelectedItem;
                Guid slID = new Guid(li.Value);
                sql.deleteServiceLookup(slID);
                Response.Write("deleted");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lblSelectedService.Text != "NONE")
            {
                SQLCode sql = new SQLCode();
                ListItem li = (ListItem)ddlServices.SelectedItem;
                serviceLookup sl = globalData.serviceLookups.Find(delegate (serviceLookup find) {
                    return find.serviceLookupID == new Guid(li.Value);
                });
                if (sl != null)
                {
                    sl.companyName = txtCompanyName.Text;
                    sl.connString = txtConnString.Text;
                    sl.serviceURL = txtServiceURL.Text;
                    sql.updateServiceLookup(sl);
                }
                else {
                    Response.Write("Couldn't find selected service lookup");
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Guid slID = Guid.NewGuid();
            serviceLookup sl = new serviceLookup();
            sl.serviceLookupID = slID;
            sl.companyName = txtAddCompanyName.Text;
            sl.connString = txtAddConnString.Text;
            sl.serviceURL = txtAddServiceURL.Text;
            globalData.serviceLookups.Add(sl);
            SQLCode sql = new SQLCode();
            sql.updateServiceLookup(sl);
            loadServices();
            Response.Write("Finished");
        }
    }
}