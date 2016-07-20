using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PL.PersistenceServices;
using OrderLinc.DTOs;
using System.Text.RegularExpressions;
namespace OrderApplication
{
    public partial class WebForm6 : System.Web.UI.Page
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
            this.Title = "Manage Customers";
            if (IsPostBack)
            {

            }
            else
            {
                int SalesOrgID = 0;
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);
                FillProvider(SalesOrgID);
                PopulateDropdownStates();
                PopulateCustomerGridview();

                txtAddressID.Text = "0";
                txtContactID.Text = "0";
                txtBillToAddressID.Text = "0";
                txtShipToAddressID.Text = "0";


            }
        }

        private void FillProvider(int SalesOrgID)
        {

            DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);

            ddlProvider.DataSource = mDTO;
            ddlProvider.DataBind();

        }
        private void PopulateDropdownStates()
        {


            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();

            ddlState.DataSource = respState;
            ddlState.DataBind();

            ddlCustState.DataSource = respState;
            ddlCustState.DataBind();

            ddlBillToState.DataSource = respState;
            ddlBillToState.DataBind();

            ddlShipToState.DataSource = respState;
            ddlShipToState.DataBind();

            ddlStateForCustomerView.DataSource = respState;
            ddlStateForCustomerView.DataBind();

        }

        /// <summary>
        /// Button: btnAdd, btnAdd Event Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearMpeCustomerControls();
            txtEndDate.Text = "";
            lblEndDateError.Text = "";
            mpeCustomer.Show();
        }

        /// <summary>
        /// Image Button: img_btnSalesRep, img_btnSalesRep Event Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtn_SalesRepSearch_Click(object sender, ImageClickEventArgs e)
        {
            mpeCustomer.Show();
            mpeSalesRepSearch.Show();
        }


        /// <summary>
        /// Button: btnSalesRepSearch, btnSalesRepSearch Event Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalesRepSearch_Click(object sender, EventArgs e)
        {


            var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListBySearchAccountType(int.Parse(Session["RefID"].ToString()), OrderLinc.AccountType.SalesRep, 1, int.Parse(Session["PageSize"].ToString()), txtSalesRepSearch.Text);

            gvSalesRepSearch.DataSource = respSalesRep;
            gvSalesRepSearch.DataBind();

            if (respSalesRep.Count != 0)
            {

                if (respSalesRep.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblSalesRepPages.Text = (respSalesRep.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respSalesRep.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlSalesRepPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlSalesRepPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblSalesRepPages.Text = (respSalesRep.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respSalesRep.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlSalesRepPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlSalesRepPages.Items.Add(newCount);
                    }
                }

                SalesRepPagingPanel.Visible = true;
                gvSalesRepSearch.DataSource = respSalesRep;
                gvSalesRepSearch.DataBind();
            }
            else
            {
                SalesRepPagingPanel.Visible = false;
                gvSalesRepSearch.DataSource = null;
                gvSalesRepSearch.DataBind();
            }
        }


        /// <summary>
        /// Event: Sales Reps Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SalesRepPaging(object sender, EventArgs e)
        {
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlSalesRepPages.SelectedValue);

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
                    if (PageNumber < int.Parse(lblSalesRepPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblSalesRepPages.Text);
                    break;
            }

            ddlSalesRepPages.SelectedValue = PageNumber.ToString();

            var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListBySearchAccountType(int.Parse(Session["RefID"].ToString()), OrderLinc.AccountType.SalesRep, PageNumber, int.Parse(Session["PageSize"].ToString()), txtSalesRepSearch.Text);

            gvSalesRepSearch.DataSource = respSalesRep;
            gvSalesRepSearch.DataBind();



        }

        /// <summary>
        /// Dropdownlist: ddlSalesRepPages, ddlSalesRepPages Selected Index Changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSalesRepPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListBySearchAccountType(int.Parse(Session["RefID"].ToString()), OrderLinc.AccountType.SalesRep, 1, int.Parse(Session["PageSize"].ToString()), txtSalesRepSearch.Text);

            gvSalesRepSearch.DataSource = respSalesRep;
            gvSalesRepSearch.DataBind();

        }

        /// <summary>
        /// Gridview: gvSalesRepSearch, gvSalesRepSearch RowCommand Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSalesRepSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int RowIndex = int.Parse(e.CommandArgument.ToString());

            int CustomerID = int.Parse(gvSalesRepSearch.Rows[RowIndex].Cells[0].Text);
            txtSalesRepID.Text = CustomerID.ToString();
            //txtSalesRepName.Text = gvSalesRepSearch.Rows[RowIndex].Cells[1].Text + ", " + gvSalesRepSearch.Rows[RowIndex].Cells[2].Text;
            mpeSalesRepSearch.Hide();

        }

        /// <summary>
        /// Function: Validation on the three addresses
        /// </summary>
        /// <returns></returns>
        private bool CheckAddresses()
        {
            lblWarning.Text = "";
            bool result = true;

            if (txtCustPostal.Text == "" || txtCustCitySuburb.Text == "" || txtCustAddLine1.Text == "")
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Missing Office Address Data ";
                result = false;
            }

            if (txtBillToPostal.Text == "" || txtBillToCitySuburb.Text == "" || txtBillToAddLine1.Text == "")
            {
                lblWarning.Visible = true;
                result = false;
                if (lblWarning.Text == "")
                {
                    lblWarning.Text = "Missing Bill to Address Data ";
                }
                else
                {
                    lblWarning.Text = lblWarning.Text + ", Missing Bill to Address Data ";
                }

            }

            if (txtShipToPostal.Text == "" || txtShipToCitySuburb.Text == "" || txtShipToAddLine1.Text == "")
            {
                lblWarning.Visible = true;
                result = false;
                if (lblWarning.Text == "")
                {
                    lblWarning.Text = "Missing Ship to Address Data ";
                }
                else
                {
                    lblWarning.Text = lblWarning.Text + ", Missing Ship to Address Data ";
                }

            }

            return result;

        }

        /// <summary>
        /// Button: btnSaveCustomer, btnSaveCustomer event onclick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveCustomer_Click(object sender, EventArgs e)
        {

            int CustomerID = 0;
            int SalesOrgID = 0;
            int.TryParse(txtCustomerID.Text, out CustomerID);
            int.TryParse(Session["RefID"].ToString(), out SalesOrgID);


            if (txtEndDate.Text == "")
            {
                txtEndDate.Text = "09/09/9999";
            }

            if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
            {

                lblEndDateError.Text = "End date should be greater than or equal to date yesterday";
                mpeCustomer.Show();
                return;
            }
            if (txtCustomerID.Text == "")
            {

                if (CreateAddressRecords() && CreateContactRecord())
                {
                    CreateCustomerRecord();
                    ClearMpeCustomerControls();
                    mpeCustomer.Hide();
                    PopulateCustomerGridview();
                }


            }
            else
            {

                if (UpdateAddressRecords() && UpdateContactRecord())
                {
                    UpdateCustomerRecord();
                    ClearMpeCustomerControls();
                    mpeCustomer.Hide();
                    PopulateCustomerGridview();
                }


            }




        }

        /// <summary>
        /// Function: Create Address Records
        /// </summary>
        /// <returns></returns>
        private bool CreateAddressRecords()
        {
            try
            {
                DTOAddress CustDTOAddress = new DTOAddress();
                CustDTOAddress.AddressID = 0;
                CustDTOAddress.AddressLine1 = txtCustAddLine1.Text.Replace("\r\n", "").Trim();
                CustDTOAddress.AddressLine2 = txtCustAddLine2.Text.Replace("\r\n", "").Trim();
                CustDTOAddress.AddressTypeID = 2;
                CustDTOAddress.CitySuburb = txtCustCitySuburb.Text;
                CustDTOAddress.PostalZipCode = txtCustPostal.Text;
                CustDTOAddress.SYSStateID = int.Parse(ddlCustState.SelectedValue);
                txtAddressID.Text = CreateAddressRecord(CustDTOAddress);

                DTOAddress BillToDTOAddress = new DTOAddress();
                BillToDTOAddress.AddressID = 0;
                BillToDTOAddress.AddressLine1 = txtBillToAddLine1.Text.Replace("\r\n", "").Trim();
                BillToDTOAddress.AddressLine2 = txtBillToAddLine2.Text.Replace("\r\n", "").Trim();
                BillToDTOAddress.AddressTypeID = 3;
                BillToDTOAddress.CitySuburb = txtBillToCitySuburb.Text;
                BillToDTOAddress.PostalZipCode = txtBillToPostal.Text;
                BillToDTOAddress.SYSStateID = int.Parse(ddlBillToState.SelectedValue);
                txtBillToAddressID.Text = CreateAddressRecord(BillToDTOAddress);

                DTOAddress ShipToDTOAddress = new DTOAddress();
                ShipToDTOAddress.AddressID = 0;
                ShipToDTOAddress.AddressLine1 = txtShipToAddLine1.Text.Replace("\r\n", "").Trim();
                ShipToDTOAddress.AddressLine2 = txtShipToAddLine2.Text.Replace("\r\n", "").Trim();
                ShipToDTOAddress.AddressTypeID = 5;
                ShipToDTOAddress.CitySuburb = txtShipToCitySuburb.Text;
                ShipToDTOAddress.PostalZipCode = txtShipToPostal.Text;
                ShipToDTOAddress.SYSStateID = int.Parse(ddlShipToState.SelectedValue);
                txtShipToAddressID.Text = CreateAddressRecord(ShipToDTOAddress);

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Function: Create Address Records
        /// </summary>
        /// <returns></returns>
        private bool UpdateAddressRecords()
        {
            try
            {
                DTOAddress CustDTOAddress = new DTOAddress();
                CustDTOAddress.AddressID = int.Parse(txtAddressID.Text);
                CustDTOAddress.AddressLine1 = txtCustAddLine1.Text.Replace("\r\n", "").Trim();
                CustDTOAddress.AddressLine2 = txtCustAddLine2.Text.Replace("\r\n", "").Trim();
                CustDTOAddress.AddressTypeID = 2;
                CustDTOAddress.CitySuburb = txtCustCitySuburb.Text;
                CustDTOAddress.PostalZipCode = txtCustPostal.Text;
                CustDTOAddress.SYSStateID = int.Parse(ddlCustState.SelectedValue);
                if (txtAddressID.Text != "0")
                    txtAddressID.Text = UpdateAddressRecord(CustDTOAddress);
                else
                    txtAddressID.Text = CreateAddressRecord(CustDTOAddress);

                DTOAddress BillToDTOAddress = new DTOAddress();
                BillToDTOAddress.AddressID = int.Parse(txtBillToAddressID.Text);
                BillToDTOAddress.AddressLine1 = txtBillToAddLine1.Text.Replace("\r\n", "").Trim();
                BillToDTOAddress.AddressLine2 = txtBillToAddLine2.Text.Replace("\r\n", "").Trim();
                BillToDTOAddress.AddressTypeID = 3;
                BillToDTOAddress.CitySuburb = txtBillToCitySuburb.Text;
                BillToDTOAddress.PostalZipCode = txtBillToPostal.Text;
                BillToDTOAddress.SYSStateID = int.Parse(ddlBillToState.SelectedValue);
                if (txtBillToAddressID.Text != "0")
                    txtBillToAddressID.Text = UpdateAddressRecord(BillToDTOAddress);
                else
                    txtBillToAddressID.Text = CreateAddressRecord(BillToDTOAddress);

                DTOAddress ShipToDTOAddress = new DTOAddress();
                ShipToDTOAddress.AddressID = int.Parse(txtShipToAddressID.Text);
                ShipToDTOAddress.AddressLine1 = txtShipToAddLine1.Text.Replace("\r\n", "").Trim();
                ShipToDTOAddress.AddressLine2 = txtShipToAddLine2.Text.Replace("\r\n", "").Trim();
                ShipToDTOAddress.AddressTypeID = 5;
                ShipToDTOAddress.CitySuburb = txtShipToCitySuburb.Text;
                ShipToDTOAddress.PostalZipCode = txtShipToPostal.Text;
                ShipToDTOAddress.SYSStateID = int.Parse(ddlShipToState.SelectedValue);
                if (txtShipToAddressID.Text != "0")
                    txtShipToAddressID.Text = UpdateAddressRecord(ShipToDTOAddress);
                else
                    txtShipToAddressID.Text = CreateAddressRecord(ShipToDTOAddress);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Function: Clear Controls on PopUp Customers
        /// </summary>
        private void ClearMpeCustomerControls()
        {
            txtCustomerCode.Text = "";
            txtCustomerID.Text = "";
            txtBusinessNo.Text = "";
            txtCustomerName.Text = "";
            txtBusinessNumberCust.Text = "";
            // txtSalesRepName.Text = "";
            txtSalesRepID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMobile.Text = "";
            txtFax.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtPostalZipCode.Text = "";
            txtCitySubUrb.Text = "";
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtBillToPostal.Text = "";
            txtBillToCitySuburb.Text = "";
            txtBillToAddLine1.Text = "";
            txtBillToAddLine2.Text = "";
            txtShipToPostal.Text = "";
            txtShipToCitySuburb.Text = "";
            txtShipToAddLine1.Text = "";
            txtShipToAddLine2.Text = "";

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

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProvider.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, PageNumber, int.Parse(Session["PageSize"].ToString()));


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

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProvider.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));

            gvCustomersView.DataSource = respCustomer;
            gvCustomersView.DataBind();

        }

        /// <summary>
        /// Function: Populate gvCustomer with data
        /// </summary>
        private void PopulateCustomerGridview()
        {
            if (ddlProvider.Items.Count > 0)
            {
                var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProvider.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));


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
            else {
                pnlCustomerView.Visible = false;
                gvCustomersView.DataSource = null;
                gvCustomersView.DataBind();
            
            }
        }


        /// <summary>
        /// Gridview: gvCustomersView, Row Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCustomersView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblEndDateError.Text = "";
            int RowIndex = int.Parse(e.CommandArgument.ToString());

            int CustomerID = int.Parse(((Label)(gvCustomersView.Rows[RowIndex].FindControl("lblCustomerID"))).Text);

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(long.Parse(ddlProvider.SelectedValue), CustomerID);

            if (respCustomer != null)
            {
                txtCustomerID.Text = respCustomer.CustomerID.ToString();
                txtContactID.Text = respCustomer.ContactID.ToString();
                txtCustomerCode.Text = respCustomer.CustomerCode.ToString();
                txtBusinessNumberCust.Text = respCustomer.BusinessNumber.ToString();
                txtCustomerName.Text = respCustomer.CustomerName.ToString();
                txtSalesRepID.Text = respCustomer.SalesRepAccountID.ToString();
                ddlState.SelectedValue = respCustomer.SYSStateID.ToString();
                txtBillToAddressID.Text = respCustomer.BillToAddressID.ToString();
                txtShipToAddressID.Text = respCustomer.ShipToAddressID.ToString();
            }

            txtAddressID.Text = respCustomer.AddressID.ToString();
            if (respCustomer.EndDate != null)
            {
                txtEndDate.Text = respCustomer.EndDate.ToString();
            }
            else
            {
                txtEndDate.Text = "";
            }

            int mSalesReAccount = 0;
            int maccountID = 0;
            int mContactId = 0;
            int mBilltoAddressId = 0;
            int mShipToAddressID = 0;
            int mAddressID = 0;

            int.TryParse(respCustomer.SalesRepAccountID.ToString(), out mSalesReAccount);
            var respAccountID = GlobalVariables.OrderAppLib.AccountService.AccountListByID(mSalesReAccount);

            if(respAccountID != null)
            int.TryParse(respAccountID.ContactID.ToString(), out maccountID);
            var respContactAccount = GlobalVariables.OrderAppLib.CustomerService.ContactListByID(maccountID);

            int.TryParse(respCustomer.ContactID.ToString(), out mContactId);
            var respContact = GlobalVariables.OrderAppLib.CustomerService.ContactListByID(mContactId);

            int.TryParse(txtBillToAddressID.Text, out mBilltoAddressId);
            var respBillToAddress = GlobalVariables.OrderAppLib.AddressService.AddressListByID(mBilltoAddressId);

            int.TryParse(txtShipToAddressID.Text, out mShipToAddressID);
            var respShipToAddress = GlobalVariables.OrderAppLib.AddressService.AddressListByID(mShipToAddressID);

            int.TryParse(txtAddressID.Text, out mAddressID);
            var respAddress = GlobalVariables.OrderAppLib.AddressService.AddressListByID(mAddressID);

            //txtSalesRepName.Text = respContactAccount.LastName.ToString() + ", " + respContactAccount.FirstName.ToString();
            if (respContact != null)
            {
                txtFirstName.Text = respContact.FirstName;
                txtLastName.Text = respContact.LastName;
                txtMobile.Text = respContact.Mobile;
                txtFax.Text = respContact.Fax;
                txtPhone.Text = respContact.Phone;
                txtEmail.Text = respContact.Email;
            }

            if(respAddress != null){
            txtCustPostal.Text = respAddress.PostalZipCode;
            txtCustCitySuburb.Text = respAddress.CitySuburb;
            }
            try
            {
                ddlCustState.SelectedValue = respAddress.SYSStateID.ToString();
            }
            catch
            {

            }
            txtCustAddLine1.Text = respAddress.AddressLine1;
            txtCustAddLine2.Text = respAddress.AddressLine2;

            txtBillToPostal.Text = respBillToAddress.PostalZipCode;
            txtBillToCitySuburb.Text = respBillToAddress.CitySuburb;
            try
            {
                ddlBillToState.SelectedValue = respBillToAddress.SYSStateID.ToString();
            }
            catch { }
            txtBillToAddLine1.Text = respBillToAddress.AddressLine1;
            txtBillToAddLine2.Text = respBillToAddress.AddressLine2;


            txtShipToPostal.Text = respShipToAddress.PostalZipCode;
            txtShipToCitySuburb.Text = respShipToAddress.CitySuburb;
            try
            {
                ddlShipToState.SelectedValue = respShipToAddress.SYSStateID.ToString();
            }
            catch
            {

            }
            txtShipToAddLine1.Text = respShipToAddress.AddressLine1;
            txtShipToAddLine2.Text = respShipToAddress.AddressLine2;

            mpeCustomer.Show();

        }


        protected void ddlStateForCustomerView_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomerSearch.Text = "";
            PopulateCustomerGridview();
        }

        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            PopulateCustomerGridview();
        }



        #region ************************tblCustomerSalesRep CRUDS ******************************************

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     private void ShowRecord(DTOCustomerSalesRep mDTO)
        //     {
        //         txtCustomerSalesRepID.Text = mDTO.CustomerSalesRepID.ToString();
        //         txtCustomerID.Text = mDTO.CustomerID.ToString();
        //         txtSalesRepAccountID.Text = mDTO.SalesRepAccountID.ToString();
        //         txtDateCreated.Text = mDTO.DateCreated.ToShortDateString();
        //     }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     private void CreateRecord()
        //     {
        //         DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
        //         mDTO.CustomerSalesRepID = 0;
        //         mDTO.CustomerID = txtCustomerID.Text;
        //         mDTO.SalesRepAccountID = txtSalesRepAccountID.Text;
        //         mDTO.DateCreated = DateTime.Parse(txtDateCreated.Text);


        //         //NOTE: devs need to create a global variable to represent the Service Class. 
        //         //      1. Create a class GlobalVariables on the UI level.
        //         //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //         //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //         string mMessage;
        //         if (GlobalVariables.customersvc.CustomerSalesRepIsValid(mDTO, out mMessage) == true)
        //         {
        //             mDTO = GlobalVariables.customersvc.CustomerSalesRepCreateRecord(mDTO);
        //             this.txtCustomerSalesRepID.Text = mDTO.CustomerSalesRepID.ToString();
        //         }
        //         else
        //         {
        //             //show error here
        //         }
        //     }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
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
        //         if (GlobalVariables.customersvc.CustomerSalesRepIsValid(mDTO, out mMessage) == true)
        //         {
        //             mDTO = GlobalVariables.customersvc.CustomerSalesRepUpdateRecord(mDTO);
        //         }
        //         else
        //         {
        //             //show error here
        //         }
        //     }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
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

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     private DTOCustomerSalesRep PopulateDTOCustomerSalesRep(int ObjectID)
        //{
        //DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();
        //      using (DataTable mDT = GlobalVariables.customersvc.CustomerSalesRepListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.CustomerSalesRepID = mcurrentRow["CustomerSalesRepID"].ToString();
        //              mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //              mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //              mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblCustomerSalesRep CRUDS *********************************


        #region ************************tblCustomer CRUDS ******************************************

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     private void ShowRecord(DTOCustomer mDTO)
        //     {
        //         txtCustomerID.Text = mDTO.CustomerID.ToString();
        //         txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //         txtCustomerCode.Text = mDTO.CustomerCode.ToString();
        //         txtBusinessNumber.Text = mDTO.BusinessNumber.ToString();
        //         txtCustomerName.Text = mDTO.CustomerName.ToString();
        //         txtSalesRepAccountID.Text = mDTO.SalesRepAccountID.ToString();
        //         txtAddressID.Text = mDTO.AddressID.ToString();
        //         txtSYSStateID.Text = mDTO.SYSStateID.ToString();
        //         txtRegionID.Text = mDTO.RegionID.ToString();
        //         txtContactID.Text = mDTO.ContactID.ToString();
        //         txtBillToAddressID.Text = mDTO.BillToAddressID.ToString();
        //         txtShipToAddressID.Text = mDTO.ShipToAddressID.ToString();
        //         txtLongitude.Text = mDTO.Longitude.ToString();
        //         txtLatitude.Text = mDTO.Latitude.ToString();
        //         txtDeleted.Text = mDTO.Deleted.ToString();
        //         txtInActive.Text = mDTO.InActive.ToString();
        //         txtDateCreated.Text = mDTO.DateCreated.ToShortDateString();
        //         txtDateUpdated.Text = mDTO.DateUpdated.ToShortDateString();
        //         txtCreatedByUserID.Text = mDTO.CreatedByUserID.ToString();
        //         txtUpdatedByUserID.Text = mDTO.UpdatedByUserID.ToString();
        //     }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        private void CreateCustomerRecord()
        {
            DTOCustomer mDTO = new DTOCustomer();
            mDTO.CustomerID = 0;
            mDTO.SalesOrgID = int.Parse(Session["RefID"].ToString());
            mDTO.CustomerCode = txtCustomerCode.Text;
            mDTO.BusinessNumber = txtBusinessNumberCust.Text;
            mDTO.CustomerName = txtCustomerName.Text.Replace("\r\n", "").Trim();
            mDTO.SalesRepAccountID = 1;
            mDTO.AddressID = int.Parse(txtAddressID.Text);
            mDTO.SYSStateID = int.Parse(ddlState.SelectedValue);
            mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
            mDTO.RegionID = 100;
            mDTO.ContactID = int.Parse(txtContactID.Text);
            mDTO.BillToAddressID = int.Parse(txtBillToAddressID.Text);
            mDTO.ShipToAddressID = int.Parse(txtShipToAddressID.Text);
            mDTO.Longitude = 100;
            mDTO.Latitude = 101;
            mDTO.Deleted = false;
            mDTO.InActive = false;
            mDTO.DateCreated = DateTime.Now;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.CreatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.UpdatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.CustomerIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSaveRecord(mDTO);
                this.txtCustomerID.Text = mDTO.CustomerID.ToString();
            }
            else
            {
                lblErroMessageCustomer.Text = mMessage;
              //  mpeCustomerAlreadyExist.Show();
                mpeCustomer.Show();
            }
        }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        private void UpdateCustomerRecord()
        {

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(long.Parse(ddlProvider.SelectedValue), int.Parse(txtCustomerID.Text));

            DTOCustomer mDTO = new DTOCustomer();
            mDTO.CustomerID = int.Parse(txtCustomerID.Text);
            mDTO.SalesOrgID = int.Parse(Session["RefID"].ToString());
            mDTO.CustomerCode = txtCustomerCode.Text;
            mDTO.BusinessNumber = txtBusinessNumberCust.Text;
            mDTO.CustomerName = txtCustomerName.Text.Replace("\r\n", "").Trim(); ;
            mDTO.SalesRepAccountID = 1;
            mDTO.AddressID = int.Parse(txtAddressID.Text);
            mDTO.SYSStateID = int.Parse(ddlState.SelectedValue);
            mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
            mDTO.RegionID = 100;
            mDTO.ContactID = int.Parse(txtContactID.Text);
            mDTO.BillToAddressID = int.Parse(txtBillToAddressID.Text);
            mDTO.ShipToAddressID = int.Parse(txtShipToAddressID.Text);
            mDTO.Longitude = respCustomer.Longitude;
            mDTO.Latitude = respCustomer.Latitude;
            mDTO.Deleted = respCustomer.Deleted;
            mDTO.InActive = respCustomer.InActive;
            mDTO.DateCreated = respCustomer.DateCreated;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.CreatedByUserID = int.Parse(respCustomer.CreatedByUserID.ToString());
            mDTO.UpdatedByUserID = int.Parse(Session["AccountID"].ToString());
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.CustomerIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSaveRecord(mDTO);
            }
            else
            {
                lblErroMessageCustomer.Text = mMessage;
              //  mpeCustomerAlreadyExist.Show();
                mpeCustomer.Show();
            }
        }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     //This is to be placed in the main ENTITY form that calls the CRUD
        //     //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //     //example: mDTO = FetchRecord(mRow); 

        //     private DTOCustomer FetchRecord(DataRowView mcurrentRow)
        //     {
        //         DTOCustomer mDTO = new DTOCustomer();
        //         mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //         mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //         mDTO.CustomerCode = mcurrentRow["CustomerCode"].ToString();
        //         mDTO.BusinessNumber = mcurrentRow["BusinessNumber"].ToString();
        //         mDTO.CustomerName = mcurrentRow["CustomerName"].ToString();
        //         mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //         mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //         mDTO.SYSStateID = mcurrentRow["SYSStateID"].ToString();
        //         mDTO.RegionID = int.Parse(mcurrentRow["RegionID"].ToString());
        //         mDTO.ContactID = mcurrentRow["ContactID"].ToString();
        //         mDTO.BillToAddressID = mcurrentRow["BillToAddressID"].ToString();
        //         mDTO.ShipToAddressID = mcurrentRow["ShipToAddressID"].ToString();
        //         mDTO.Longitude = mcurrentRow["Longitude"].ToString();
        //         mDTO.Latitude = mcurrentRow["Latitude"].ToString();
        //         mDTO.Deleted = mcurrentRow["Deleted"].ToString();
        //         mDTO.InActive = mcurrentRow["InActive"].ToString();
        //         mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];
        //         mDTO.DateUpdated = (DateTime)mcurrentRow["DateUpdated"];
        //         mDTO.CreatedByUserID = int.Parse(mcurrentRow["CreatedByUserID"].ToString());
        //         mDTO.UpdatedByUserID = int.Parse(mcurrentRow["UpdatedByUserID"].ToString());


        //         return mDTO;
        //     }

        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        //     private DTOCustomer PopulateDTOCustomer(int ObjectID)
        //{
        //DTOCustomer mDTO = new DTOCustomer();
        //      using (DataTable mDT = GlobalVariables.customersvc.CustomerListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //              mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //              mDTO.CustomerCode = mcurrentRow["CustomerCode"].ToString();
        //              mDTO.BusinessNumber = mcurrentRow["BusinessNumber"].ToString();
        //              mDTO.CustomerName = mcurrentRow["CustomerName"].ToString();
        //              mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //              mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //              mDTO.SYSStateID = mcurrentRow["SYSStateID"].ToString();
        //              mDTO.RegionID = int.Parse(mcurrentRow["RegionID"].ToString());
        //              mDTO.ContactID = mcurrentRow["ContactID"].ToString();
        //              mDTO.BillToAddressID = mcurrentRow["BillToAddressID"].ToString();
        //              mDTO.ShipToAddressID = mcurrentRow["ShipToAddressID"].ToString();
        //              mDTO.Longitude = mcurrentRow["Longitude"].ToString();
        //              mDTO.Latitude = mcurrentRow["Latitude"].ToString();
        //              mDTO.Deleted = (Boolean)mcurrentRow["Deleted"];
        //              mDTO.InActive = (Boolean)mcurrentRow["InActive"];
        //              mDTO.DateCreated = (DateTime)mcurrentRow["DateCreated"];
        //              mDTO.DateUpdated = (DateTime)mcurrentRow["DateUpdated"];
        //              mDTO.CreatedByUserID = int.Parse(mcurrentRow["CreatedByUserID"].ToString());
        //              mDTO.UpdatedByUserID = int.Parse(mcurrentRow["UpdatedByUserID"].ToString());
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblCustomer CRUDS *********************************


        #region ************************tblContact CRUDS ******************************************

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     private void ShowRecord(DTOContact mDTO)
        //     {
        //         txtContactID.Text = mDTO.ContactID.ToString();
        //         txtPhone.Text = mDTO.Phone.ToString();
        //         txtFax.Text = mDTO.Fax.ToString();
        //         txtMobile.Text = mDTO.Mobile.ToString();
        //         txtEmail.Text = mDTO.Email.ToString();
        //         txtLastName.Text = mDTO.LastName.ToString();
        //         txtFirstName.Text = mDTO.FirstName.ToString();
        //     }

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        private bool CreateContactRecord()
        {
            bool response;

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
                //show error here
                response = false;
            }

            return response;
        }

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        private bool UpdateContactRecord()
        {

            bool response;

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
                if (mDTO.ContactID != 0)
                {
                    mDTO = GlobalVariables.OrderAppLib.CustomerService.ContactSaveRecord(mDTO);
                }
                else
                {
                    mDTO = GlobalVariables.OrderAppLib.CustomerService.ContactSaveRecord(mDTO);
                    txtContactID.Text = mDTO.ContactID.ToString();
                }
                response = true;
            }
            else
            {
                response = false;
                //show error here
            }

            return response;
        }

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     //This is to be placed in the main ENTITY form that calls the CRUD
        //     //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //     //example: mDTO = FetchRecord(mRow); 

        //     private DTOContact FetchRecord(DataRowView mcurrentRow)
        //     {
        //         DTOContact mDTO = new DTOContact();
        //         mDTO.ContactID = int.Parse(mcurrentRow["ContactID"].ToString());
        //         mDTO.Phone = mcurrentRow["Phone"].ToString();
        //         mDTO.Fax = mcurrentRow["Fax"].ToString();
        //         mDTO.Mobile = mcurrentRow["Mobile"].ToString();
        //         mDTO.Email = mcurrentRow["Email"].ToString();
        //         mDTO.LastName = mcurrentRow["LastName"].ToString();
        //         mDTO.FirstName = mcurrentRow["FirstName"].ToString();


        //         return mDTO;
        //     }

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     private DTOContact PopulateDTOContact(int ObjectID)
        //{
        //DTOContact mDTO = new DTOContact();
        //      using (DataTable mDT = GlobalVariables.Address.ContactListByID(ObjectID))
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

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     private void ShowRecord(DTOAddress mDTO)
        //     {
        //         txtAddressID.Text = mDTO.AddressID.ToString();
        //         txtAddressTypeID.Text = mDTO.AddressTypeID.ToString();
        //         txtAddressLine1.Text = mDTO.AddressLine1.ToString();
        //         txtAddressLine2.Text = mDTO.AddressLine2.ToString();
        //         txtCitySuburb.Text = mDTO.CitySuburb.ToString();
        //         txtSYSStateID.Text = mDTO.SYSStateID.ToString();
        //         txtPostalZipCode.Text = mDTO.PostalZipCode.ToString();
        //     }

        //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        private string CreateAddressRecord(DTOAddress mDTOAddress)
        {
            DTOAddress mDTO = new DTOAddress();
            mDTO.AddressID = 0;
            mDTO.AddressTypeID = mDTOAddress.AddressTypeID;
            mDTO.AddressLine1 = mDTOAddress.AddressLine1;
            mDTO.AddressLine2 = mDTOAddress.AddressLine2;
            mDTO.CitySuburb = mDTOAddress.CitySuburb;
            mDTO.SYSStateID = mDTOAddress.SYSStateID;
            mDTO.PostalZipCode = mDTOAddress.PostalZipCode;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AddressService.AddressIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.AddressService.AddressSaveRecord(mDTO);
                return mDTO.AddressID.ToString();
            }
            else
            {
                return "";
                //show error here
            }
        }





        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        private string UpdateAddressRecord(DTOAddress mDTOAddress)
        {
            DTOAddress mDTO = new DTOAddress();
            mDTO.AddressID = mDTOAddress.AddressID;
            mDTO.AddressTypeID = mDTOAddress.AddressTypeID;
            mDTO.AddressLine1 = mDTOAddress.AddressLine1;
            mDTO.AddressLine2 = mDTOAddress.AddressLine2;
            mDTO.CitySuburb = mDTOAddress.CitySuburb;
            mDTO.SYSStateID = mDTOAddress.SYSStateID;
            mDTO.PostalZipCode = mDTOAddress.PostalZipCode;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.AddressService.AddressIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.AddressService.AddressSaveRecord(mDTO);
                return mDTO.AddressID.ToString();
            }
            else
            {
                //show error here
                return "";
            }
        }

        protected void img_btnDelete_Customer_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton m_btn;
            GridViewRow m_row;

            m_btn = (ImageButton)sender;
            m_row = (GridViewRow)m_btn.NamingContainer;

            gvCustomersView.SelectedIndex = m_row.RowIndex;
            string mcustomerID = gvCustomersView.DataKeys[m_row.RowIndex].Value.ToString();

            DTOCustomer mCust = new DTOCustomer();

            mCust.CustomerID = int.Parse(mcustomerID);
            GlobalVariables.OrderAppLib.CustomerService.CustomerDeleteRecord(long.Parse(ddlProvider.SelectedValue), long.Parse(mcustomerID), long.Parse(Session["AccountID"].ToString()));

            PopulateCustomerGridview();
        }


        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     //This is to be placed in the main ENTITY form that calls the CRUD
        //     //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //     //example: mDTO = FetchRecord(mRow); 

        //     private DTOAddress FetchRecord(DataRowView mcurrentRow)
        //     {
        //         DTOAddress mDTO = new DTOAddress();
        //         mDTO.AddressID = mcurrentRow["AddressID"].ToString();
        //         mDTO.AddressTypeID = int.Parse(mcurrentRow["AddressTypeID"].ToString());
        //         mDTO.AddressLine1 = mcurrentRow["AddressLine1"].ToString();
        //         mDTO.AddressLine2 = mcurrentRow["AddressLine2"].ToString();
        //         mDTO.CitySuburb = mcurrentRow["CitySuburb"].ToString();
        //         mDTO.SYSStateID = int.Parse(mcurrentRow["SYSStateID"].ToString());
        //         mDTO.PostalZipCode = mcurrentRow["PostalZipCode"].ToString();


        //         return mDTO;
        //     }

        //     //CASE Generated Code 8/6/2013 3:10:09 PM Lazy Dog 3.3.1.0
        //     private DTOAddress PopulateDTOAddress(int ObjectID)
        //{
        //DTOAddress mDTO = new DTOAddress();
        //      using (DataTable mDT = GlobalVariables.Address.AddressListByID(ObjectID))
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









    }
}