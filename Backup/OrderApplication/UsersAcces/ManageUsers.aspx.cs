using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PL.PersistenceServices;
using OrderLinc.DTOs;

namespace OrderApplication
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["AccountID"].ToString() != "")
                {

                }
                else
                {

                }
            }
            catch
            {
                ViewState.Clear();
                Session.Clear();
                Response.Redirect("~/LogIn.aspx");
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Manage Users";
            if (IsPostBack)
            {

            }
            else
            {

                gvAccounts.DataSource = null;
                gvAccounts.DataBind();

                gvAssignedCustomers.DataSource = null;
                gvAssignedCustomers.DataBind();

                MultiView1.SetActiveView(ViewUsers);

                FillOrganizationUnitDropdowns();
                int orgUnitID = 0;
                int.TryParse(ddlOrganizationUnitSelect.SelectedValue.ToString(), out orgUnitID);
                FillAccountsBasedOnOrganizationID(orgUnitID);
                FillStateDropdownlist();
                FillAccountTypeDropDownList();
            }




        }

        //public void FillProvider(int mSalesOrgID)
        //{
        //    try
        //    {

        //        DTOProviderList mDTOProviders = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(mSalesOrgID);

        //        ddlProvider.DataSource = mDTOProviders;
        //        ddlProvider.DataBind();

        //    }
        //    catch
        //    {

        //    }
        //}


        /// <summary>
        /// Populate Organization Unit Dropdownlists
        /// </summary>
        private void FillOrganizationUnitDropdowns()
        {


            var respOrgUnits = GlobalVariables.OrderAppLib.CustomerService.OrgUnitListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

            ddlOrganizationUnitSelect.DataSource = respOrgUnits;
            ddlOrganizationUnitSelect.DataBind();

            ddlOrganizationUnit.DataSource = respOrgUnits;
            ddlOrganizationUnit.DataBind();


        }


        /// <summary>
        /// Populate Gridview Accounts based on OrganizationID
        /// </summary>
        /// <param name="OrgID"></param>
        private void FillAccountsBasedOnOrganizationID(int OrgID)
        {

            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(OrgID, int.Parse(Session["RefID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), txtSearchUserText.Text);

            if (respAccounts.Count != 0)
            {

                if (respAccounts.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblAccountsPages.Text = (respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlAccountsPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAccountsPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblAccountsPages.Text = (respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlAccountsPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAccountsPages.Items.Add(newCount);
                    }
                }

                AccountsPanel.Visible = true;
                gvAccounts.DataSource = respAccounts;
                gvAccounts.DataBind();
                FilterAccountType();

            }
            else
            {
                AccountsPanel.Visible = false;
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
            }

        }


        private void FilterAccountType()
        {

            foreach (GridViewRow mRow in gvAccounts.Rows)
            {
                int AccountTypeID = int.Parse(((Label)(gvAccounts.Rows[mRow.RowIndex].FindControl("lblAccountTypeID"))).Text);
                if (AccountTypeID == 4)
                {

                }
                else
                {
                    gvAccounts.Rows[mRow.RowIndex].FindControl("imgbtnAssign").Visible = false;
                }
            }

        }


        /// <summary>
        /// Organization Unit dropdownlist select index change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOrganizationUnitSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrganizationUnitSelect.SelectedValue));
        }

        /// <summary>
        /// Populate Account Type Dropdownlist 
        /// </summary>
        private void FillAccountTypeDropDownList()
        {


            var respAccountType = GlobalVariables.OrderAppLib.AccountService.AccountTypeList();

            respAccountType.RemoveAt(0);
            ddlAccountType.DataSource = respAccountType;
            ddlAccountType.DataBind();


        }



        /// <summary>
        /// Cancel Button Event on Manage User View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCancelUser_Click(object sender, EventArgs e)
        {
            ClearManageUserFields();
            MultiView1.SetActiveView(ViewUsers);

        }


        /// <summary>
        /// Clear Manage User Control Fields
        /// </summary>
        private void ClearManageUserFields()
        {
            txtAccountID.Text = "";
            txtAddressID.Text = "";
            txtContactID.Text = "";
            txtUsernameMangeUser.Text = "";
            txtPasswordMangeUser.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtPhone.Text = "";
            txtMobile.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtCity.Text = "";
            txtPostalZipCode.Text = "";
            txtDeviceNo.Text = "";
            FillStateDropdownlist();
            FillAccountTypeDropDownList();

        }


        /// <summary>
        /// Add User Button Event on View User View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Session["ManageMode"] = "Create";
            txtEndDate.Text = "";
            div_deviceNo.Visible = false;
            lblManageUserError.Text = "";
            ddlOrganizationUnit.SelectedIndex = ddlOrganizationUnitSelect.SelectedIndex;
            //MultiView1.SetActiveView(ManageUser);
            mpeManageUserAccounts.Show();
            ClearManageItems();


        }

        private void ClearManageItems()
        {
            txtUsernameMangeUser.Text = "";
            txtPasswordMangeUser.Text = "";
            txtPasswordMangeUser.Attributes["value"] = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtPhone.Text = "";
            txtMobile.Text = "";
            txtFax.Text = "";
            txtCity.Text = "";
            txtPostalZipCode.Text = "";
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtEmail.Text = "";

        }


        /// <summary>
        /// Button Clicked Event For Paging Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AccountsPaging(object sender, EventArgs e)
        {

            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlAccountsPages.SelectedValue);

            switch (lnkButton.CommandName)
            {
                case "First":
                    PageNumber = 1;

                    break;
                case "Previous":
                    if (PageNumber - 1 != 0)
                    {
                        PageNumber = PageNumber - 1;
                    }
                    break;
                case "Next":
                    if (PageNumber < int.Parse(lblAccountsPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblAccountsPages.Text);
                    break;
            }

            ddlAccountsPages.SelectedValue = PageNumber.ToString();

            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(int.Parse(ddlOrganizationUnitSelect.SelectedValue), int.Parse(Session["RefID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), txtSearchUserText.Text);


            gvAccounts.DataSource = respAccounts;
            gvAccounts.DataBind();

            FilterAccountType();

        }


        /// <summary>
        /// Select index changed event for dropdownlist account pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAccountsPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PageNumber = int.Parse(ddlAccountsPages.SelectedValue);


            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearchAccountType(int.Parse(Session["RefID"].ToString()), OrderLinc.AccountType.SalesRep, 1, int.Parse(Session["PageSize"].ToString()), txtSearchUserText.Text);

            gvAccounts.DataSource = respAccounts;
            gvAccounts.DataBind();

            FilterAccountType();

        }

        /// <summary>
        /// Button Save Clicked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveUser_Click(object sender, EventArgs e)
        {

            int AccountID = 0;
            int.TryParse(txtAccountID.Text, out AccountID);


            if (txtEndDate.Text == "")
            {
                txtEndDate.Text = "09/09/9999";
            }

            if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
            {

                lblManageUserError.Text = "End date should be greater than or equal to yesterday's date.";
                mpeManageUserAccounts.Show();
                return;
            }
            if (Session["ManageMode"].ToString() == "Create")
            {

                //var respUsers = GlobalVariables.OrderAppLib.OrderAppService.AccountListByUserName(txtUsernameMangeUser.Text);

                //if (respUsers.Count() < 1)
                //{

                if (CreateAddressRecord() && CreateContactRecord() && CreateAccountRecord())
                {
                    //ClientScript.RegisterStartupScript(GetType(), "Successful", "alert('Successfully Created User')", true);
                    txtManageUserNotification.Text = "Successfully Created User";
                    mpeManageUserNotif.Show();
                    ClearManageUserFields();
                    //MultiView1.SetActiveView(ViewUsers);
                    //mpeManageUserAccounts.Show();
                    FillAccountsBasedOnOrganizationID(int.Parse(ddlOrganizationUnitSelect.SelectedValue));
                }

                //}
                //else
                //{
                //    mpeUserAlreadyExist.Show();
                //    mpeManageUserAccounts.Show();

                //}


            }
            else
            {

                //DTOAccountList respUsers = GlobalVariables.OrderAppLib.OrderAppService.AccountListByUserName(txtUsernameMangeUser.Text);
                //DTOAccount respUserID = new DTOAccount();
                //if (respUsers.Count > 0)
                //{
                //    respUserID = GlobalVariables.OrderAppLib.OrderAppService.AccountListByID(int.Parse(respUsers[0].AccountID.ToString()));
                //}
                //else { 


                //}
                //if (respUserID.AccountID.ToString() == txtAccountID.Text)
                //{
                DTOAccount mDTOAccount = GlobalVariables.OrderAppLib.AccountService.AccountListByID(int.Parse(txtAccountID.Text));
                DTOAddress mDTOAddress = GlobalVariables.OrderAppLib.AddressService.AddressListByID(int.Parse(mDTOAccount.AddressID.ToString()));
                if (mDTOAccount.AddressID != 0)
                {
                    UpdateAddressRecord(mDTOAddress);
                }
                else
                {

                    CreateAddressRecord();
                }

                if (mDTOAccount.ContactID != 0)
                {
                    UpdateContactRecord();
                }
                else
                {
                    CreateContactRecord();
                }
                if (UpdateAccountRecord(mDTOAccount, false))
                {
                    //ClientScript.RegisterStartupScript(GetType(), "Successful", "alert('Successfully Saved Changes')", true);
                    txtManageUserNotification.Text = "Successfully Update User";
                    mpeManageUserNotif.Show();
                    ClearManageUserFields();
                    MultiView1.SetActiveView(ViewUsers);
                    FillAccountsBasedOnOrganizationID(int.Parse(ddlOrganizationUnitSelect.SelectedValue));
                }
                //}
                //else
                //{
                //    mpeUserAlreadyExist.Show();
                //    mpeManageUserAccounts.Show();

                //}

            }

        }

        /// <summary>
        /// Populate State Dropdownlist event
        /// </summary>
        private void FillStateDropdownlist()
        {

            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();

            ddlSYSStateID.DataSource = respState;
            ddlSYSStateID.DataBind();

        }


        /// <summary>
        /// Row Command event for Grid View Accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int AccountID = int.Parse(((Label)(gvAccounts.Rows[RowIndex].FindControl("lblAccountID"))).Text);

            txtAccountID.Text = AccountID.ToString();

            switch (e.CommandName)
            {
                case "View":
                    lblManageUserError.Text = "";
                    Session["ManageMode"] = "View";
                    FillViewControls(AccountID);
                    //MultiView1.SetActiveView(ManageUser);
                    mpeManageUserAccounts.Show();
                    break;
                case "Delete":
                    DTOAccount mDTOAccount = GlobalVariables.OrderAppLib.AccountService.AccountListByID(AccountID);
                    FillViewControls(AccountID);
                    UpdateAccountRecord(mDTOAccount, true);
                    FillAccountsBasedOnOrganizationID(int.Parse(ddlOrganizationUnitSelect.SelectedValue));
                    break;
                case "Assign":
                    DTOAccount mDTOAccountDetails = GlobalVariables.OrderAppLib.AccountService.AccountListByID(AccountID);
                    var respContactDetails = GlobalVariables.OrderAppLib.CustomerService.ContactListByID(int.Parse(mDTOAccountDetails.ContactID.ToString()));
                    lblSalesRepName.Text = respContactDetails.LastName.ToString() + ", " + respContactDetails.FirstName.ToString();
                    PopulateStateDropdown();
                    //FillProvider(int.Parse(Session["RefID"].ToString()));
                    PopulateCustomerGridview();
                    PopulateAssignedCustomersGridview();
                    MultiView1.SetActiveView(AssignSalesReps);
                    break;

            }

        }

        /// <summary>
        /// Row Deleting event on Gridview gvAccounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Populate Controls for Viewing or Editing.
        /// </summary>
        /// <param name="AccountID"></param>
        private void FillViewControls(int AccountID)
        {
            try
            {

                var respAccountDetails = GlobalVariables.OrderAppLib.AccountService.AccountListByID(AccountID);
                var respContactDetails = GlobalVariables.OrderAppLib.CustomerService.ContactListByID(int.Parse(respAccountDetails.ContactID.ToString()));
                var respAddressDetails = GlobalVariables.OrderAppLib.AddressService.AddressListByID(int.Parse(respAccountDetails.AddressID.ToString()));

                div_deviceNo.Visible = true;
                if (respAccountDetails != null)
                {
                    txtAccountID.Text = AccountID.ToString();
                    txtAddressID.Text = respAccountDetails.AddressID.ToString();
                    txtContactID.Text = respAccountDetails.ContactID.ToString();
                    txtUsernameMangeUser.Text = respAccountDetails.Username;
                    txtPasswordMangeUser.Text = respAccountDetails.Password;
                    txtPasswordMangeUser.Attributes.Add("value", respAccountDetails.Password);
                    hidServerID.Value = respAccountDetails.ServerID.ToString();
                    txtDeviceNo.Text = respAccountDetails.DeviceNo;
                }

                if (respContactDetails != null)
                {
                    txtLastName.Text = respContactDetails.LastName;
                    txtFirstName.Text = respContactDetails.FirstName;
                    txtPhone.Text = respContactDetails.Phone;
                    txtMobile.Text = respContactDetails.Mobile;
                    txtFax.Text = respContactDetails.Fax;
                    txtEmail.Text = respContactDetails.Email;

                }

                if (respAddressDetails != null)
                {
                    txtAddressLine1.Text = respAddressDetails.AddressLine1;
                    txtAddressLine2.Text = respAddressDetails.AddressLine2;
                    txtCity.Text = respAddressDetails.CitySuburb;
                    txtPostalZipCode.Text = respAddressDetails.PostalZipCode;

                }
                if (respAccountDetails.EndDate.ToShortDateString() != "1/1/0001" && respAccountDetails.EndDate.ToShortDateString() != "9/9/9999")
                {
                    txtEndDate.Text = respAccountDetails.EndDate.ToShortDateString();

                }
                else
                {
                    txtEndDate.Text = "";
                }

                try
                {
                    ddlAccountType.SelectedValue = respAccountDetails.AccountTypeID.ToString();
                }
                catch (Exception ex)
                {
                    // lblErrorMessage.Text = "Error - FillViewControls -" + ex.Message;
                }
                try
                {
                    ddlOrganizationUnit.SelectedValue = respAccountDetails.OrgUnitID.ToString();
                }
                catch (Exception ex)
                {
                    // lblErrorMessage.Text = "Error - FillViewControls -" + ex.Message;
                }
                try
                {
                    ddlSYSStateID.SelectedValue = respAddressDetails.SYSStateID.ToString();
                }
                catch (Exception ex)
                {
                    //lblErrorMessage.Text = "Error - FillViewControls -" + ex.Message;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error - FillViewControls -" + ex.Message;
            }

        }


        /// <summary>
        /// Event: Customer View Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CustomerViewPaging(object sender, EventArgs e)
        {
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlCustomerViewPages.SelectedValue);

            switch (lnkButton.CommandName)
            {
                case "First":
                    PageNumber = 1;

                    break;
                case "Previous":
                    if (PageNumber - 1 != 0)
                    {
                        PageNumber = PageNumber - 1;
                    }
                    break;
                case "Next":
                    if (PageNumber < int.Parse(lblCustomerViewPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblCustomerViewPages.Text);
                    break;
            }

            ddlCustomerViewPages.SelectedValue = PageNumber.ToString();

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesOrgSearch(int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), "", PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvCustomersView.DataSource = respCustomer;
            gvCustomersView.DataBind();

        }

        /// <summary>
        /// Dropdownlist: ddlCustomerViewPages, Selected Index Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCustomerViewPages_SelectedIndexChanged(object sender, EventArgs e)
        {

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesOrgSearch(int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, int.Parse(ddlCustomerViewPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvCustomersView.DataSource = respCustomer;
            gvCustomersView.DataBind();

        }


        private void PopulateStateDropdown()
        {
            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();

            ddlStateForCustomerView.DataSource = respState;
            ddlStateForCustomerView.DataBind();

        }

        /// <summary>
        /// Function: Populate gvCustomer with data
        /// </summary>
        private void PopulateCustomerGridview()
        {

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesOrgSearch(int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));

            gvCustomersView.DataSource = respCustomer;
            gvCustomersView.DataBind();

            if (respCustomer.Count != 0)
            {

                if (respCustomer.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblCustomerViewPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlCustomerViewPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerViewPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblCustomerViewPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlCustomerViewPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerViewPages.Items.Add(newCount);
                    }
                }

                pnlCustomerView.Visible = true;
                gvCustomersView.DataSource = respCustomer;
                gvCustomersView.DataBind();
            }
            else
            {
                pnlCustomerView.Visible = false;
                gvCustomersView.DataSource = null;
                gvCustomersView.DataBind();
            }

        }

        protected void ddlStateForCustomerView_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomerSearch.Text = "";
            PopulateCustomerGridview();
        }



        protected void gvCustomersView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = int.Parse(e.CommandArgument.ToString());

            txtCustomerID.Text = ((Label)(gvCustomersView.Rows[RowIndex].FindControl("lblCustomerID"))).Text;

            mpeNotificationAssign.Show();

        }

        protected void btnNotificationAssignOk_Click(object sender, EventArgs e)
        {
            var respIfExistingCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepListSearchSalesRepAndCustomer(int.Parse(txtAccountID.Text), int.Parse(txtCustomerID.Text));

            if (respIfExistingCustomer.Count == 0)
            {
                CreateCustomerSalesRepRecord();
                PopulateAssignedCustomersGridview();
            }
            else
            {
                mpeAlreadyAssigned.Show();
            }

        }


        private void PopulateAssignedCustomersGridview()
        {
            var respAssignedCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesRepID(int.Parse(txtAccountID.Text), 1, int.Parse(Session["PageSize"].ToString()));


            gvAssignedCustomers.DataSource = respAssignedCustomer;
            gvAssignedCustomers.DataBind();

            if (respAssignedCustomer.Count != 0)
            {

                if (respAssignedCustomer.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblAssignedCustomerPages.Text = (respAssignedCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respAssignedCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlAssignedCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAssignedCustomerPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblAssignedCustomerPages.Text = (respAssignedCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respAssignedCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlAssignedCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAssignedCustomerPages.Items.Add(newCount);
                    }
                }

                pnlAssignedCustomer.Visible = true;
                gvAssignedCustomers.DataSource = respAssignedCustomer;
                gvAssignedCustomers.DataBind();
            }
            else
            {
                pnlAssignedCustomer.Visible = false;
                gvAssignedCustomers.DataSource = null;
                gvAssignedCustomers.DataBind();
            }

        }

        protected void ddlAssignedCustomerPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CustomerSalesRepListBySalesRepID
            var respAssignedCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesRepID(int.Parse(txtAccountID.Text), int.Parse(ddlAssignedCustomerPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvAssignedCustomers.DataSource = respAssignedCustomer;
            gvAssignedCustomers.DataBind();
        }

        /// <summary>
        /// Event: Assigned Customer View Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AssignedCustomerPaging(object sender, EventArgs e)
        {
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlAssignedCustomerPages.SelectedValue);

            switch (lnkButton.CommandName)
            {
                case "First":
                    PageNumber = 1;

                    break;
                case "Previous":
                    if (PageNumber - 1 != 0)
                    {
                        PageNumber = PageNumber - 1;
                    }
                    break;
                case "Next":
                    if (PageNumber < int.Parse(lblAssignedCustomerPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblAssignedCustomerPages.Text);
                    break;
            }

            ddlAssignedCustomerPages.SelectedValue = PageNumber.ToString();
          //  CustomerSalesRepListBySalesRepID
            var respAssignedCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySalesRepID(int.Parse(txtAccountID.Text), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvAssignedCustomers.DataSource = respAssignedCustomer;
            gvAssignedCustomers.DataBind();

        }

        protected void btnBackToUserList_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(ViewUsers);
        }


        protected void gvAssignedCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = int.Parse(e.CommandArgument.ToString());

            string CustomerSalesRepID = (((Label)(gvAssignedCustomers.Rows[RowIndex].FindControl("lblCustomerSalesRepID"))).Text);
            hidCustomerSalesRepID.Value = CustomerSalesRepID;

            DTOCustomerSalesRep mDTOCustomerSalesRep = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepListByID(int.Parse(CustomerSalesRepID));
            lblAssignedCustomerName.Text = mDTOCustomerSalesRep.CustomerName;
            txtSalesRepEndDate.Text = mDTOCustomerSalesRep.EndDate.ToShortDateString();
            //var resp = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepDeleteRecord(mDTOCustomerSalesRep);
            mpeUpdateSalesrepEndDate.Show();

         
          

        }

        /// <summary>
        /// Button: btnSearchCustomerText, Event on Click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchCustomerText_Click(object sender, EventArgs e)
        {

            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(int.Parse(ddlOrganizationUnitSelect.SelectedValue), int.Parse(Session["RefID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), txtSearchUserText.Text);

            if (respAccounts.Count != 0)
            {

                if (respAccounts.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblAccountsPages.Text = (respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlAccountsPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAccountsPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblAccountsPages.Text = (respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respAccounts.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlAccountsPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlAccountsPages.Items.Add(newCount);
                    }
                }

                AccountsPanel.Visible = true;
                gvAccounts.DataSource = respAccounts;
                gvAccounts.DataBind();
                FilterAccountType();

            }
            else
            {
                AccountsPanel.Visible = false;
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
            }

        }


        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {

            PopulateCustomerGridview();

        }







        #region ************************tblContact CRUDS ******************************************

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOContact mDTO)
        //{
        //    txtContactID.Text = mDTO.ContactID.ToString();
        //    txtPhone.Text = mDTO.Phone.ToString();
        //    txtFax.Text = mDTO.Fax.ToString();
        //    txtMobile.Text = mDTO.Mobile.ToString();
        //    txtEmail.Text = mDTO.Email.ToString();
        //    txtLastName.Text = mDTO.LastName.ToString();
        //    txtFirstName.Text = mDTO.FirstName.ToString();
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool CreateContactRecord()
        {
            bool response = false;

            DTOContact mDTO = new DTOContact();
            mDTO.ContactID = 0;
            mDTO.Phone = txtPhone.Text;
            mDTO.Fax = txtFax.Text;
            mDTO.Mobile = txtMobile.Text;
            mDTO.Email = txtEmail.Text;
            mDTO.LastName = txtLastName.Text;
            mDTO.FirstName = txtFirstName.Text;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.ContactIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.ContactSaveRecord(mDTO);
                this.txtContactID.Text = mDTO.ContactID.ToString();
                response = true;
            }
            else
            {
                response = false;
                //show error here
            }

            return response;

        }

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool UpdateContactRecord()
        {
            bool response = false;

            DTOContact mDTO = new DTOContact();
            mDTO.ContactID = int.Parse(txtContactID.Text);
            mDTO.Phone = txtPhone.Text;
            mDTO.Fax = txtFax.Text;
            mDTO.Mobile = txtMobile.Text;
            mDTO.Email = txtEmail.Text;
            mDTO.LastName = txtLastName.Text;
            mDTO.FirstName = txtFirstName.Text;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.ContactIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.ContactSaveRecord(mDTO);
                response = true;
            }
            else
            {
                response = false;
                //show error here
            }

            return response;

        }

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOContact FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOContact mDTO = new DTOContact();
        //    mDTO.ContactID = int.Parse(mcurrentRow["ContactID"].ToString());
        //    mDTO.Phone = mcurrentRow["Phone"].ToString();
        //    mDTO.Fax = mcurrentRow["Fax"].ToString();
        //    mDTO.Mobile = mcurrentRow["Mobile"].ToString();
        //    mDTO.Email = mcurrentRow["Email"].ToString();
        //    mDTO.LastName = mcurrentRow["LastName"].ToString();
        //    mDTO.FirstName = mcurrentRow["FirstName"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //     private DTOContact PopulateDTOContact(int ObjectID)
        //{
        //DTOContact mDTO = new DTOContact();
        //      using (DataTable mDT = GlobalVariables.OrderAppSVC.ContactListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ContactID = int.Parse(mcurrentRow["ContactID"].ToString());
        //              mDTO.Phone = mcurrentRow["Phone"].ToString();
        //              mDTO.Fax = mcurrentRow["Fax"].ToString();
        //              mDTO.Mobile = mcurrentRow["Mobile"].ToString();
        //              mDTO.Email = mcurrentRow["Email"].ToString();
        //              mDTO.LastName = mcurrentRow["LastName"].ToString();
        //              mDTO.FirstName = mcurrentRow["FirstName"].ToString();
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblContact CRUDS *********************************


        #region ************************tblAddress CRUDS ******************************************

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOAddress mDTO)
        //{
        //    txtAddressID.Text = mDTO.AddressID.ToString();
        //    txtAddressTypeID.Text = mDTO.AddressTypeID.ToString();
        //    txtAddressLine1.Text = mDTO.AddressLine1.ToString();
        //    txtAddressLine2.Text = mDTO.AddressLine2.ToString();
        //    txtCitySuburb.Text = mDTO.CitySuburb.ToString();
        //    txtSYSStateID.Text = mDTO.SYSStateID.ToString();
        //    txtPostalZipCode.Text = mDTO.PostalZipCode.ToString();
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool CreateAddressRecord()
        {

            bool response = false;

            DTOAddress mDTO = new DTOAddress();
            mDTO.AddressID = 0;
            mDTO.AddressTypeID = 2;
            mDTO.AddressLine1 = txtAddressLine1.Text;
            mDTO.AddressLine2 = txtAddressLine2.Text;
            mDTO.CitySuburb = txtCity.Text;
            mDTO.SYSStateID = int.Parse(ddlSYSStateID.SelectedValue);
            mDTO.PostalZipCode = txtPostalZipCode.Text;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AddressService.AddressIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.AddressService.AddressSaveRecord(mDTO);
                this.txtAddressID.Text = mDTO.AddressID.ToString();
                response = true;
            }
            else
            {
                response = false;
                //show error here
            }
            return response;
        }

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool UpdateAddressRecord(DTOAddress mDTOAddress)
        {

            bool response = false;

            DTOAddress mDTO = new DTOAddress();
            mDTO.AddressID = int.Parse(txtAddressID.Text);
            mDTO.AddressTypeID = mDTOAddress.AddressTypeID;
            mDTO.AddressLine1 = txtAddressLine1.Text;
            mDTO.AddressLine2 = txtAddressLine2.Text;
            mDTO.CitySuburb = txtCity.Text;
            mDTO.SYSStateID = int.Parse(ddlSYSStateID.SelectedValue);
            mDTO.PostalZipCode = txtPostalZipCode.Text;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AddressService.AddressIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.AddressService.AddressSaveRecord(mDTO);
                response = true;
            }
            else
            {
                response = false;
                //show error here
            }

            return response;
        }

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOAddress FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOAddress mDTO = new DTOAddress();
        //    mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //    mDTO.AddressTypeID = int.Parse(mcurrentRow["AddressTypeID"].ToString());
        //    mDTO.AddressLine1 = mcurrentRow["AddressLine1"].ToString();
        //    mDTO.AddressLine2 = mcurrentRow["AddressLine2"].ToString();
        //    mDTO.CitySuburb = mcurrentRow["CitySuburb"].ToString();
        //    mDTO.SYSStateID = int.Parse(mcurrentRow["SYSStateID"].ToString());
        //    mDTO.PostalZipCode = mcurrentRow["PostalZipCode"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //     private DTOAddress PopulateDTOAddress(int ObjectID)
        //{
        //DTOAddress mDTO = new DTOAddress();
        //      using (DataTable mDT = GlobalVariables.OrderAppSVC.AddressListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //              mDTO.AddressTypeID = int.Parse(mcurrentRow["AddressTypeID"].ToString());
        //              mDTO.AddressLine1 = mcurrentRow["AddressLine1"].ToString();
        //              mDTO.AddressLine2 = mcurrentRow["AddressLine2"].ToString();
        //              mDTO.CitySuburb = mcurrentRow["CitySuburb"].ToString();
        //              mDTO.SYSStateID = int.Parse(mcurrentRow["SYSStateID"].ToString());
        //              mDTO.PostalZipCode = mcurrentRow["PostalZipCode"].ToString();
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblAddress CRUDS *********************************





        #region ************************tblAccount CRUDS ******************************************

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOAccount mDTO)
        //{
        //    txtAccountID.Text = mDTO.AccountID.ToString();
        //    txtRefID.Text = mDTO.RefID.ToString();
        //    txtAccountTypeID.Text = mDTO.AccountTypeID.ToString();
        //    txtOrgUnitID.Text = mDTO.OrgUnitID.ToString();
        //    txtUsernameMangeUser.Text = mDTO.Username.ToString();
        //    txtPasswordMangeUser.Text = mDTO.Password.ToString();
        //    txtDeviceNo.Text = mDTO.DeviceNo.ToString();
        //    txtRoleID.Text = mDTO.RoleID.ToString();
        //    txtAddressID.Text = mDTO.AddressID.ToString();
        //    txtContactID.Text = mDTO.ContactID.ToString();
        //    txtDeleted.Text = mDTO.Deleted.ToString();
        //    txtInActive.Text = mDTO.InActive.ToString();
        //    txtLockout.Text = mDTO.Lockout.ToString();
        //    txtLastIpAddress.Text = mDTO.LastIpAddress.ToString();
        //    txtDateLockout.Text = mDTO.DateLockout.ToShortDateString();
        //    txtLastLoginDate.Text = mDTO.LastLoginDate.ToShortDateString();
        //    txtDateCreated.Text = mDTO.DateCreated.ToShortDateString();
        //    txtDateUpdated.Text = mDTO.DateUpdated.ToShortDateString();
        //    txtCreatedByUserID.Text = mDTO.CreatedByUserID.ToString();
        //    txtUpdatedByUserID.Text = mDTO.UpdatedByUserID.ToString();
        //    txtExpiryDate.Text = mDTO.ExpiryDate.ToShortDateString();
        //    txtDateActivated.Text = mDTO.DateActivated.ToShortDateString();
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool CreateAccountRecord()
        {

            bool response = false;

            DTOAccount mDTO = new DTOAccount();
            mDTO.AccountID = 0;
            mDTO.RefID = int.Parse(Session["RefID"].ToString());
            mDTO.AccountTypeID = int.Parse(ddlAccountType.SelectedValue);
            mDTO.OrgUnitID = int.Parse(ddlOrganizationUnit.SelectedValue);
            mDTO.Username = txtUsernameMangeUser.Text;
            mDTO.Password = txtPasswordMangeUser.Text;
            mDTO.DeviceNo = "";
            mDTO.RoleID = 1;
            mDTO.AddressID = int.Parse(txtAddressID.Text);
            mDTO.ContactID = int.Parse(txtContactID.Text);
            mDTO.Deleted = false;
            mDTO.InActive = false;
            mDTO.Lockout = false;
            mDTO.LastIpAddress = "192.168.N.N";
            mDTO.DateLockout = DateTime.Now;
            mDTO.LastLoginDate = DateTime.Now;
            mDTO.DateCreated = DateTime.Now;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.CreatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.UpdatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.ExpiryDate = DateTime.Now.AddYears(1);
            mDTO.DateActivated = DateTime.Now;
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AccountService.AccountIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.AccountService.AccountSaveRecord(mDTO);
                this.txtAccountID.Text = mDTO.AccountID.ToString();
                response = true;
            }
            else
            {
                response = false;
                lblManageUserError.Text = mMessage;
               // mpeUserAlreadyExist.Show();
                mpeManageUserAccounts.Show();
                //show error here
            }

            return response;
        }



        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        private bool UpdateAccountRecord(DTOAccount mDTOAccount, bool Deleted)
        {

            bool response = false;

            DTOAccount mDTO = new DTOAccount();
            mDTO.AccountID = int.Parse(txtAccountID.Text);
            mDTO.RefID = mDTOAccount.RefID;
            mDTO.AccountTypeID = int.Parse(ddlAccountType.SelectedValue);
            mDTO.OrgUnitID = int.Parse(ddlOrganizationUnit.SelectedValue);
            mDTO.Username = txtUsernameMangeUser.Text;
            mDTO.Password = txtPasswordMangeUser.Text;
            mDTO.DeviceNo = txtDeviceNo.Text;
            mDTO.RoleID = mDTOAccount.RoleID;
            mDTO.AddressID = int.Parse(txtAddressID.Text);
            mDTO.ContactID = int.Parse(txtContactID.Text);
            mDTO.Deleted = Deleted;
            mDTO.InActive = mDTOAccount.InActive;
            mDTO.Lockout = mDTOAccount.Lockout;
            mDTO.LastIpAddress = mDTOAccount.LastIpAddress;
            mDTO.DateLockout = mDTOAccount.DateLockout;
            mDTO.LastLoginDate = mDTOAccount.LastLoginDate;
            mDTO.DateCreated = mDTOAccount.DateCreated;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.CreatedByUserID = mDTOAccount.CreatedByUserID;
            mDTO.UpdatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.ExpiryDate = mDTOAccount.ExpiryDate;
            mDTO.DateActivated = mDTOAccount.DateActivated;
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            int serverid = 0;
            int.TryParse(hidServerID.Value, out serverid);
            mDTO.ServerID = serverid;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AccountService.AccountIsValid(mDTO, out mMessage) == true)
            {
                response = true;
                mDTO = GlobalVariables.OrderAppLib.AccountService.AccountSaveRecord(mDTO);
            }
            else
            {
                response = false;
                lblManageUserError.Text = mMessage;
              //  mpeUserAlreadyExist.Show();
                mpeManageUserAccounts.Show();
                //show error here
            }

            return response;

        }







        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOAccount FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOAccount mDTO = new DTOAccount();
        //    mDTO.AccountID = mcurrentRow["AccountID"].ToString();
        //    mDTO.RefID = mcurrentRow["RefID"].ToString();
        //    mDTO.AccountTypeID = int.Parse(mcurrentRow["AccountTypeID"].ToString());
        //    mDTO.OrgUnitID = int.Parse(mcurrentRow["OrgUnitID"].ToString());
        //    mDTO.Username = mcurrentRow["Username"].ToString();
        //    mDTO.Password = mcurrentRow["Password"].ToString();
        //    mDTO.DeviceNo = mcurrentRow["DeviceNo"].ToString();
        //    mDTO.RoleID = int.Parse(mcurrentRow["RoleID"].ToString());
        //    mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //    mDTO.ContactID = mcurrentRow["ContactID"].ToString();
        //    mDTO.Deleted = mcurrentRow["Deleted"].ToString();
        //    mDTO.InActive = mcurrentRow["InActive"].ToString();
        //    mDTO.Lockout = mcurrentRow["Lockout"].ToString();
        //    mDTO.LastIpAddress = mcurrentRow["LastIpAddress"].ToString();
        //    mDTO.DateLockout = (DateTime)mcurrentRow["DateLockout"];
        //    mDTO.LastLoginDate = (DateTime)mcurrentRow["LastLoginDate"];
        //    mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];
        //    mDTO.DateUpdated = (DateTime)mcurrentRow["DateUpdated"];
        //    mDTO.CreatedByUserID = int.Parse(mcurrentRow["CreatedByUserID"].ToString());
        //    mDTO.UpdatedByUserID = int.Parse(mcurrentRow["UpdatedByUserID"].ToString());
        //    mDTO.ExpiryDate = (DateTime)mcurrentRow["ExpiryDate"];
        //    mDTO.DateActivated = (DateTime)mcurrentRow["DateActivated"];


        //    return mDTO;
        //}

        //CASE Generated Code 6/28/2013 8:43:19 AM Lazy Dog 3.3.1.0
        //     private DTOAccount PopulateDTOAccount(int ObjectID)
        //{
        //DTOAccount mDTO = new DTOAccount();
        //      using (DataTable mDT = GlobalVariables.OrderAppSVC.AccountListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.AccountID = mcurrentRow["AccountID"].ToString();
        //              mDTO.RefID = mcurrentRow["RefID"].ToString();
        //              mDTO.AccountTypeID = int.Parse(mcurrentRow["AccountTypeID"].ToString());
        //              mDTO.OrgUnitID = int.Parse(mcurrentRow["OrgUnitID"].ToString());
        //              mDTO.Username = mcurrentRow["Username"].ToString();
        //              mDTO.Password = mcurrentRow["Password"].ToString();
        //              mDTO.DeviceNo = mcurrentRow["DeviceNo"].ToString();
        //              mDTO.RoleID = int.Parse(mcurrentRow["RoleID"].ToString());
        //              mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //              mDTO.ContactID = mcurrentRow["ContactID"].ToString();
        //              mDTO.Deleted = (Boolean)mcurrentRow["Deleted"];
        //              mDTO.InActive = (Boolean)mcurrentRow["InActive"];
        //              mDTO.Lockout = (Boolean)mcurrentRow["Lockout"];
        //              mDTO.LastIpAddress = mcurrentRow["LastIpAddress"].ToString();
        //              mDTO.DateLockout = (DateTime)mcurrentRow["DateLockout"];
        //              mDTO.LastLoginDate = (DateTime)mcurrentRow["LastLoginDate"];
        //              mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];
        //              mDTO.DateUpdated = (DateTime)mcurrentRow["DateUpdated"];
        //              mDTO.CreatedByUserID = int.Parse(mcurrentRow["CreatedByUserID"].ToString());
        //              mDTO.UpdatedByUserID = int.Parse(mcurrentRow["UpdatedByUserID"].ToString());
        //              mDTO.ExpiryDate = (DateTime)mcurrentRow["ExpiryDate"];
        //              mDTO.DateActivated = (DateTime)mcurrentRow["DateActivated"];
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblAccount CRUDS *********************************



        #region ************************tblCustomerSalesRep CRUDS ******************************************

        //     //CASE Generated Code 8/19/2013 2:59:30 AM Lazy Dog 3.3.1.0
        //     private void ShowRecord(DTOCustomerSalesRep mDTO)
        //{
        //      txtCustomerSalesRepID.Text = mDTO.CustomerSalesRepID.ToString();
        //      txtCustomerID.Text = mDTO.CustomerID.ToString();
        //      txtSalesRepAccountID.Text = mDTO.SalesRepAccountID.ToString();
        //      if (mDTO.DateCreated == DateTime.MinValue);
        //      {
        //          txtDateCreated.Text = string.Empty;
        //      }
        //      else
        //      {
        //      txtDateCreated.Text = mDTO.DateCreated.ToShortDateString();
        //      }
        //}

        //     //CASE Generated Code 8/19/2013 2:59:30 AM Lazy Dog 3.3.1.0
        private void CreateCustomerSalesRepRecord()
        {
            DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
            mDTO.CustomerSalesRepID = 0;
            mDTO.CustomerID = int.Parse(txtCustomerID.Text);
            mDTO.SalesRepAccountID = int.Parse(txtAccountID.Text);
            mDTO.DateCreated = DateTime.Now;
            mDTO.StartDate = DateTime.Parse("1/1/2014");
            mDTO.EndDate = DateTime.Parse("9/9/9999");

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepSaveRecord(mDTO);
                this.txtCustomerSalesRepID.Text = mDTO.CustomerSalesRepID.ToString();
            }
            else
            {
                //show error here
            }
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {


        }

        protected void img_btnDelete_User_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton m_btn;
            GridViewRow m_row;

            m_btn = (ImageButton)sender;
            m_row = (GridViewRow)m_btn.NamingContainer;

            gvAccounts.SelectedIndex = m_row.RowIndex;
            string mAccountID = gvAccounts.DataKeys[m_row.RowIndex].Value.ToString();


            GlobalVariables.OrderAppLib.AccountService.AccountDeleteRecord(long.Parse(mAccountID));

            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrganizationUnitSelect.SelectedValue));
        }

        protected void ddlProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCustomerGridview();
            PopulateAssignedCustomersGridview();
        }

        protected void tbnUpdateEndDate_Click(object sender, EventArgs e)
        {
            try
            {
                DTOCustomerSalesRep mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepListByID(int.Parse(hidCustomerSalesRepID.Value));

                mDTO.EndDate = DateTime.Parse( txtSalesRepEndDate.Text);

                GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepSaveRecord(mDTO);
            }
            catch { 
            
            }
        }

        protected void btnClearDeviceNo_Click(object sender, EventArgs e)
        {  txtDeviceNo.Text = "";
        }






        //     //CASE Generated Code 8/19/2013 2:59:30 AM Lazy Dog 3.3.1.0
        //     private void UpdateRecord()
        //     {
        //         DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
        //         mDTO.CustomerSalesRepID = txtCustomerSalesRepID.Text;
        //         mDTO.CustomerID = txtCustomerID.Text;
        //         mDTO.SalesRepAccountID = txtSalesRepAccountID.Text;
        //         mDTO.DateCreated = DateTime.Parse(txtDateCreated.Text);


        //         //NOTE: devs need to create a global variable to represent the Service Class. 
        //         //      1. Create a class GlobalVariables on the UI level.
        //         //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //         //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //         string mMessage;
        //         if (GlobalVariables.CustomerSalesRep.CustomerSalesRepIsValid(mDTO, out mMessage) == true)
        //         {
        //             mDTO = GlobalVariables.CustomerSalesRep.CustomerSalesRepUpdateRecord(mDTO);
        //         }
        //         else
        //         {
        //             //show error here
        //         }
        //     }

        //     //CASE Generated Code 8/19/2013 2:59:30 AM Lazy Dog 3.3.1.0
        //     //This is to be placed in the main ENTITY form that calls the CRUD
        //     //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //     //example: mDTO = FetchRecord(mRow); 

        //     private DTOCustomerSalesRep FetchRecord(DataRowView mcurrentRow)
        //     {
        //         DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
        //         mDTO.CustomerSalesRepID = mcurrentRow["CustomerSalesRepID"].ToString();
        //         mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //         mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //         mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];


        //         return mDTO;
        //     }

        //     //CASE Generated Code 8/19/2013 2:59:30 AM Lazy Dog 3.3.1.0
        //     private DTOCustomerSalesRep PopulateDTOCustomerSalesRep(int ObjectID)
        //{
        //DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
        //      using (DataTable mDT = GlobalVariables.CustomerSalesRep.CustomerSalesRepListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.CustomerSalesRepID = mcurrentRow["CustomerSalesRepID"].ToString();
        //              mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //              mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //              if (mcurrentRow["DateCreated"] != System.DBNull.Value);
        //              {
        //                 mDTO.DateCreated = DateTime.Parse(mcurrentRow["DateCreated"].ToString());
        //              }
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblCustomerSalesRep CRUDS *********************************





    }
}