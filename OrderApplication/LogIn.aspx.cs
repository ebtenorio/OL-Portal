using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PL.PersistenceServices.DTOS;
using OrderLinc.DTOs;

namespace OrderApplication
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "OrderLinc";
            txtUsername.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DTOAccount mDTOAccount = GlobalVariables.OrderAppLib.AccountService.AccountAuthenticate(txtUsername.Text,txtPassword.Text);
            DTOSYSConfigList mDTOSYSConfigList = GlobalVariables.OrderAppLib.ConfigurationService.SYSConfigList();

            foreach (DTOSYSConfig mDTOSYSConfig in mDTOSYSConfigList) 
            {
                if (mDTOSYSConfig.ConfigKey == "PageSize") 
                {
                    Session["PageSize"] =  mDTOSYSConfig.ConfigValue;
                }
            
            }
            if (mDTOAccount != null)
            {
                if (int.Parse(mDTOAccount.AccountID.ToString()) != 0 && mDTOAccount.AccountTypeID.ToString() != "4")
                {
                    Session["AccountID"] = mDTOAccount.AccountID;
                    Session["RefID"] = mDTOAccount.RefID;
                    Session["AccountTypeID"] = mDTOAccount.AccountTypeID;
                    Session["RoleID"] = mDTOAccount.RoleID;
                    Session["OrgUnit"] = mDTOAccount.OrgUnitID;
                    Response.Redirect("~/Orders/ManageOrder.aspx");
                }
                else
                {
                    lblErrorMsg.Visible = true;
                }
            }
            else {

                lblErrorMsg.Visible = true;
            }
        }
    }
}