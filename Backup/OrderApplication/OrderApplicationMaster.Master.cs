using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderApplication
{
    public partial class OrderApplicationMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {
                lblVersion.Text = "2.1.1.18";
                FilterMenus();

            }
        }

        protected void lnkBtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect(ResolveUrl("~/LogIn.aspx"));
        }

        private void FilterMenus()
        {

            switch (Session["AccountTypeID"].ToString())
            {

                case "1":
                    //System Admin
                    break;
                case "2":
                    //Office Admin
                    liReports.Visible = true;
                    ManageUsers.Visible = true;
                    break;
                case "3":
                    //Office User
                    ManageUsers.Visible = false;
                    ViewProducts.Visible = false;
                    ManageUsers.Visible = true;
                    ManageOrganizations.Visible = false;
                    liReports.Visible = false;
                    break;
                case "4":
                    //Sales Rep
                     ManageUsers.Visible = false;
                    ViewProducts.Visible = false;
                        ManageUsers.Visible = false;
                    ManageOrganizations.Visible = false;
                    break;

            }

        }

    }
}