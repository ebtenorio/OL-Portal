using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PL.PersistenceServices;
using OrderLinc.DTOs;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;


namespace OrderApplication.Organizations
{
    public partial class Organization : System.Web.UI.Page
    {
        public DataTable m_ProviderTable;


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
            this.Title = "OrderLinc";

            if (!IsPostBack)
            {



                hidIsAssignSalesRep.Value = "0";
                int SalesOrgID = 0;
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);

                hidProviderCustomerTempID.Value = Guid.NewGuid().ToString();
                Session[hidProviderCustomerTempID.Value] = GetTable();

                FillProvider(SalesOrgID);
                PopulateDropdownStates();

                try
                {
                    ddlState.SelectedValue = Session["SelectedState"].ToString();
                    cestartDate.Format = GlobalVariables.GetDateFormat;
                    ceEnddate.Format = GlobalVariables.GetDateFormat;
                }
                catch
                {

                }


                txtAddressID.Text = "0";
                txtContactID.Text = "0";
                txtBillToAddressID.Text = "0";
                txtShipToAddressID.Text = "0";

                if (Session["CustomerID"].ToString() == "")
                {
                    // pnlCustomer.Enabled = true;
                    ClearMpeCustomerControls();
                    //ddlState.SelectedValue = ddlStateForCustomerView.SelectedValue;
                    txtEndDate.Text = "";
                    lblEndDateError.Text = "";
                    ShowProviderCustomer();
                    m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];
                    if (m_ProviderTable != null)
                    {
                        m_ProviderTable.Rows.Clear();
                        m_ProviderTable.AcceptChanges();

                        Session[hidProviderCustomerTempID.Value] = m_ProviderTable;


                        // mpeCustomer.Show();
                    }
                }
                else
                {

                    ShowRecord(int.Parse(Session["CustomerID"].ToString()));

                }

            }
        }




        private void ShowRecord(int CustomerID)
        {

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(0, CustomerID);

            if (respCustomer != null)
            {
                txtCustomerID.Text = respCustomer.CustomerID.ToString();
                hidCustomerID.Value = respCustomer.CustomerID.ToString();
                txtContactID.Text = respCustomer.ContactID.ToString();
                txtCustomerCode.Text = respCustomer.CustomerCode.ToString();
                txtBusinessNumberCust.Text = respCustomer.BusinessNumber.ToString();
                txtCustomerName.Text = respCustomer.CustomerName.ToString();
                txtSalesRepID.Text = respCustomer.SalesRepAccountID.ToString();
                ddlState.SelectedValue = respCustomer.SYSStateID.ToString();
                txtBillToAddressID.Text = respCustomer.BillToAddressID.ToString();
                txtShipToAddressID.Text = respCustomer.ShipToAddressID.ToString();
                txtAddressID.Text = respCustomer.AddressID.ToString();

                ListProviderCustomerbyCustomerID(int.Parse(respCustomer.CustomerID.ToString()));


                if (respCustomer.EndDate != null)
                {

                    txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", respCustomer.EndDate);
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

                if (respAccountID != null)
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

                if (respAddress != null)
                {
                    txtCustPostal.Text = respAddress.PostalZipCode;
                    txtCustCitySuburb.Text = respAddress.CitySuburb;
                    txtCustAddLine1.Text = respAddress.AddressLine1;
                    txtCustAddLine2.Text = respAddress.AddressLine2;
                    txtOfficeAddress.Text = txtCustAddLine1.Text + " " + txtCustAddLine2.Text + " " + txtCustCitySuburb.Text + " " + txtCustPostal.Text;


                    try
                    {
                        ddlCustState.SelectedValue = respAddress.SYSStateID.ToString();
                    }
                    catch
                    {

                    }

                }
            }


            //if (respBillToAddress != null)
            //{

            //    txtBillToPostal.Text = respBillToAddress.PostalZipCode;
            //    txtBillToCitySuburb.Text = respBillToAddress.CitySuburb;
            //    txtBillToAddLine1.Text = respBillToAddress.AddressLine1;
            //    txtBillToAddLine2.Text = respBillToAddress.AddressLine2;

            //    try
            //    {
            //        ddlBillToState.SelectedValue = respBillToAddress.SYSStateID.ToString();
            //    }
            //    catch { }


            //}


            //if (respShipToAddress != null)
            //{

            //    txtShipToPostal.Text = respShipToAddress.PostalZipCode;
            //    txtShipToCitySuburb.Text = respShipToAddress.CitySuburb;

            //    txtShipToAddLine1.Text = respShipToAddress.AddressLine1;
            //    txtShipToAddLine2.Text = respShipToAddress.AddressLine2;
            //    try
            //    {
            //        ddlShipToState.SelectedValue = respShipToAddress.SYSStateID.ToString();
            //    }
            //    catch
            //    { }
            //}


        }


        private void ListProviderCustomerbyCustomerID(int CustomerID)
        {

            DTOProviderCustomerList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderCustomerListbyCustomerID(CustomerID);


            m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];

            m_ProviderTable.Rows.Clear();
            m_ProviderTable.AcceptChanges();

            string dateformat;

            dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

            foreach (DTOProviderCustomer mPro in mDTO)
            {
                DataRow mRow = m_ProviderTable.NewRow();
                mRow["EndDate"] = String.Format(dateformat, mPro.EndDate);
                mRow["StartDate"] = String.Format(dateformat, mPro.StartDate);
                mRow["ProviderCustomerCode"] = mPro.ProviderCustomerCode;
                mRow["ProviderName"] = mPro.ProviderName;
                mRow["ProviderID"] = mPro.ProviderID;
                mRow["ProviderCustomerID"] = mPro.ProviderCustomerID;
                m_ProviderTable.Rows.Add(mRow);
                m_ProviderTable.AcceptChanges();

            }


            if (m_ProviderTable != null && m_ProviderTable.Rows.Count > 0)
            {
                DataRow[] foundRows = m_ProviderTable.Select("", "ProviderName ASC");
                DataTable dt = foundRows.CopyToDataTable();
                Session[hidProviderCustomerTempID.Value] = dt;
                gvProviderCustomer.DataSource = dt;
                gvProviderCustomer.DataBind();
            }
            else
            {

                Session[hidProviderCustomerTempID.Value] = m_ProviderTable;
                gvProviderCustomer.DataSource = m_ProviderTable;
                gvProviderCustomer.DataBind();
            }


        }


        private void ClearMpeCustomerControls()
        {
            txtCustomerCode.Text = "";
            txtCustomerID.Text = "";

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

            //txtBillToPostal.Text = "";
            //txtBillToCitySuburb.Text = "";
            //txtBillToAddLine1.Text = "";
            //txtBillToAddLine2.Text = "";
            //txtShipToPostal.Text = "";
            //txtShipToCitySuburb.Text = "";
            //txtShipToAddLine1.Text = "";
            //txtShipToAddLine2.Text = "";
            lblWarning.Text = "";

        }
        public DataTable GetTable()
        {
            //
            // Here we create a DataTable with four columns.
            //
            m_ProviderTable = new DataTable();
            m_ProviderTable.Columns.Add("ProviderCustomerID", typeof(int));
            m_ProviderTable.Columns.Add("ProviderID", typeof(int));
            m_ProviderTable.Columns.Add("ProviderName", typeof(string));
            m_ProviderTable.Columns.Add("ProviderCustomerCode", typeof(string));
            m_ProviderTable.Columns.Add("StartDate", typeof(string));
            m_ProviderTable.Columns.Add("EndDate", typeof(string));


            return m_ProviderTable;
        }

        public void ShowProviderCustomer()
        {
            m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];

            if (m_ProviderTable != null && m_ProviderTable.Rows.Count > 0)
            {
                DataRow[] foundRows = m_ProviderTable.Select("", "ProviderName ASC");
                DataTable dt = foundRows.CopyToDataTable();
                Session[hidProviderCustomerTempID.Value] = dt;
                gvProviderCustomer.DataSource = dt;
                gvProviderCustomer.DataBind();
            }
            else
            {

                Session[hidProviderCustomerTempID.Value] = m_ProviderTable;
                gvProviderCustomer.DataSource = m_ProviderTable;
                gvProviderCustomer.DataBind();
            }

        }



        private void SaveCustomer()
        {

            if (gvProviderCustomer.Rows.Count > 0)
            {

                if (txtEndDate.Text == "")
                {
                    txtEndDate.Text = "09/09/9999";
                }

                //if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
                //{

                //    lblEndDateError.Text = "End date should be greater than or equal to date yesterday";
                //    mpeCustomer.Show();
                //    return;
                //}

                if (Session["CustomerID"].ToString() == "")
                {
                    using (TransactionScope mScope = new TransactionScope()) // TRANSACTION DEPENDENCIES
                    {

                        if (CreateAddressRecords() && CreateContactRecord())
                        {

                            if (CreateCustomerRecord())
                            {

                                bool _selectedToCreateOrder = false;

                                foreach (GridViewRow mRow in gvProviderCustomer.Rows)
                                {

                                    string mProviderID = gvProviderCustomer.DataKeys[mRow.RowIndex].Value.ToString();

                                    DTOProviderCustomer mDTO = new DTOProviderCustomer();

                                    mDTO.ProviderCustomerCode = HttpUtility.HtmlDecode(mRow.Cells[1].Text).Trim().Replace(" ","");
                                    mDTO.ProviderCustomerID = 0;
                                    mDTO.CustomerID = int.Parse(txtCustomerID.Text);
                                    mDTO.ProviderID = int.Parse(mProviderID);

                                    string EndDate = HttpUtility.HtmlDecode(mRow.Cells[3].Text);
                                    if (EndDate.Replace(" ", "").Trim() == "")
                                    {

                                        EndDate = "09/09/9999";
                                    }

                                    string dateformat = GlobalVariables.GetDateFormat;

                                    try
                                    {
                                        mDTO.EndDate = DateTime.ParseExact(EndDate, dateformat, null);
                                    }
                                    catch
                                    {
                                        mDTO.EndDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
                                    }

                                    try
                                    {
                                        mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, dateformat, null);
                                    }
                                    catch
                                    {
                                        mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, "dd/MM/yyyy", null);
                                    }

                                    mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderCustomerSaveRecord(mDTO);

                                }

                                if (hidIsAssignSalesRep.Value == "1")
                                { // Assign Sales Rep in the selected state to customer. 

                                    AssignSalesReptoCustomer(long.Parse(ddlState.SelectedValue.ToString()), long.Parse(Session["RefID"].ToString()), long.Parse(txtCustomerID.Text));
                                }

                                mScope.Complete();

                                Response.Redirect("~/Organizations/ViewOrganizations.aspx"); // Goes back to the List

                            }

                        }


                        // ClearMpeCustomerControls();

                        // PopulateCustomerGridview();
                    }


                }
                else
                {

                    if (UpdateAddressRecords() && UpdateContactRecord())
                    {
                        bool _selectedToCreateOrder = false;


                        if (UpdateCustomerRecord())
                        {

                            foreach (GridViewRow mRow in gvProviderCustomer.Rows)
                            {

                                string mProviderID = gvProviderCustomer.DataKeys[mRow.RowIndex].Value.ToString();
                                string mProviderCustomerID = gvProviderCustomer.DataKeys[mRow.RowIndex].Values[1].ToString();

                                DTOProviderCustomer mDTO = new DTOProviderCustomer();
                                mDTO.ProviderCustomerCode = HttpUtility.HtmlDecode(mRow.Cells[1].Text).Trim().Replace(" ", "");
                                mDTO.ProviderCustomerID = long.Parse(mProviderCustomerID);
                                mDTO.CustomerID = int.Parse(hidCustomerID.Value);
                                mDTO.ProviderID = int.Parse(mProviderID);
                                string EndDate = HttpUtility.HtmlDecode(mRow.Cells[3].Text);
                                
                                // Check for ProviderID and CustomerID
                                if (((CheckBox)mRow.FindControl("cbxProviderCustomerCreateOrder")).Checked)
                                {
                                    Session["CustomerPageProviderID"] = mProviderID;
                                    Session["CustomerPageCustomerID"] = hidCustomerID.Value;
                                    Session["CustomerPageCustomerCode"] = HttpUtility.HtmlDecode(mRow.Cells[1].Text);
                                    Session["CustomerPageCustomerName"] = this.txtCustomerName.Text;
                                    Session["CustomerPageStateName"] = this.ddlState.SelectedItem.Text;
                                   
                                    _selectedToCreateOrder = true;
                                }


                                if (EndDate.Replace(" ", "").Trim() == "")
                                {

                                    EndDate = "09/09/9999";
                                }

                                string dateformat = GlobalVariables.GetDateFormat;

                                try
                                {
                                    mDTO.EndDate = DateTime.ParseExact(EndDate, dateformat, null);
                                }
                                catch
                                {
                                    mDTO.EndDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
                                }


                                try
                                {
                                    mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, dateformat, null);
                                }
                                catch
                                {
                                    mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, "dd/MM/yyyy", null);
                                }


                                mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderCustomerSaveRecord(mDTO);

                            }
                        }
                        //  ClearMpeCustomerControls();

                        //   PopulateCustomerGridview();

                        if (_selectedToCreateOrder)
                        {
                            Response.Redirect("~/Orders/ManageOrder.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Organizations/ViewOrganizations.aspx");
                        }

                    }


                }



            }
            else
            {

                lblCustomerNotification.Text = "Warning: Customer cannot make an order unless a Provider entry is added.";
                //   mpeCustomer.Show();
                mpeCustomerNotification.Show();
            }

        }


        protected void btnSaveCustomer_Click(object sender, EventArgs e)
        {

            int CustomerID = 0;
            int SalesOrgID = 0;
            int.TryParse(txtCustomerID.Text, out CustomerID);
            int.TryParse(Session["RefID"].ToString(), out SalesOrgID);


            DTOCustomer mCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByBusinessNumber(long.Parse(SalesOrgID.ToString()), txtBusinessNumberCust.Text, CustomerID);

            if (mCustomer != null && mCustomer.CustomerID != 0 && txtBusinessNumberCust.Text.Trim() != "")
            {
                lblNotificationBusinessNumber.Text = "Business No. already exists for Customer " + mCustomer.CustomerName + ". Continue?";
                mpeCheckBusinessNumber.Show();
            }
            else
            {
                string message = "";

                if (CheckCustomerName(CustomerID, out message))
                {
                    SaveCustomer();
                }
                else
                {

                    mpeCustomerExist.Show();
                    lblCustomerNameIfExist.Text = message;

                }

            }

        }


        private Boolean CheckCustomerName(int customerID, out string message)
        {
            try
            {
                Boolean result = true;
                message = "";
                int CustomerID = 0;
                int SalesOrgID = 0;
                int.TryParse(txtCustomerID.Text, out CustomerID);
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);

                DTOCustomer customer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerName(SalesOrgID, txtCustomerName.Text, CustomerID);

                if (customer != null && customer.CustomerID != 0)
                {
                    message = "Warining: Customer Name already exists in " + customer.StateName + ". Do you want to proceed?";

            
                      
                        btnCustomerExist.Text = "Yes";
                        btnCancelCustomerExist.Visible = true;
                 

                    result = false;
                }
                //if (customerID > 0 && (customer != null && customer.CustomerID != customerID))
                //{
                //    message = "Warining: Customer Name already exists in " + customer.StateName + ". Do you want to proceed?";
                //    result = false;

                 
                        
                //        btnCustomerExist.Text = "Yes";
                //        btnCancelCustomerExist.Visible = true;
                 
                //}

                return result;
            }
            catch (Exception ex)
            {


                message = ex.Message;
                return false;
            }


        }
        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        private Boolean UpdateCustomerRecord()
        {


            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(0, int.Parse(txtCustomerID.Text));

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

            mDTO.EndDate = DateTime.Parse("09/09/9999");



            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.CustomerIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSaveRecord(mDTO);
                return true;
            }
            else
            {

                lblWarning.Text = mMessage;
                //  mpeCustomerAlreadyExist.Show();
                //   mpeCustomer.Show();
                return false;
            }
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
                {
                    txtAddressID.Text = UpdateAddressRecord(CustDTOAddress);
                }
                else
                {
                    if (txtCustAddLine1.Text != "")
                        txtAddressID.Text = CreateAddressRecord(CustDTOAddress);
                }

                //DTOAddress BillToDTOAddress = new DTOAddress();
                //BillToDTOAddress.AddressID = int.Parse(txtBillToAddressID.Text);
                //BillToDTOAddress.AddressLine1 = txtBillToAddLine1.Text.Replace("\r\n", "").Trim();
                //BillToDTOAddress.AddressLine2 = txtBillToAddLine2.Text.Replace("\r\n", "").Trim();
                //BillToDTOAddress.AddressTypeID = 3;
                //BillToDTOAddress.CitySuburb = txtBillToCitySuburb.Text;
                //BillToDTOAddress.PostalZipCode = txtBillToPostal.Text;
                //BillToDTOAddress.SYSStateID = int.Parse(ddlBillToState.SelectedValue);
                //if (txtBillToAddressID.Text != "0")
                //{
                //    txtBillToAddressID.Text = UpdateAddressRecord(BillToDTOAddress);
                //}
                //else
                //{
                //    if (txtBillToAddLine1.Text != "")
                //        txtBillToAddressID.Text = CreateAddressRecord(BillToDTOAddress);
                //}

                //DTOAddress ShipToDTOAddress = new DTOAddress();
                //ShipToDTOAddress.AddressID = int.Parse(txtShipToAddressID.Text);
                //ShipToDTOAddress.AddressLine1 = txtShipToAddLine1.Text.Replace("\r\n", "").Trim();
                //ShipToDTOAddress.AddressLine2 = txtShipToAddLine2.Text.Replace("\r\n", "").Trim();
                //ShipToDTOAddress.AddressTypeID = 5;
                //ShipToDTOAddress.CitySuburb = txtShipToCitySuburb.Text;
                //ShipToDTOAddress.PostalZipCode = txtShipToPostal.Text;
                //ShipToDTOAddress.SYSStateID = int.Parse(ddlShipToState.SelectedValue);
                //if (txtShipToAddressID.Text != "0")
                //{
                //    txtShipToAddressID.Text = UpdateAddressRecord(ShipToDTOAddress);
                //}
                //else
                //{
                //    if (txtShipToAddLine1.Text != "")
                //        txtShipToAddressID.Text = CreateAddressRecord(ShipToDTOAddress);
                //}


                return true;
            }
            catch
            {
                return false;
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


        //     //CASE Generated Code 8/6/2013 2:44:20 PM Lazy Dog 3.3.1.0
        private Boolean CreateCustomerRecord()
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




            mDTO.EndDate = DateTime.Parse("09/09/9999");




            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CustomerService.CustomerIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CustomerService.CustomerSaveRecord(mDTO);
                this.txtCustomerID.Text = mDTO.CustomerID.ToString();
                return true;
            }
            else
            {

                lblWarning.Text = mMessage;
                // mpeCustomerAlreadyExist.Show();
                // mpeCustomer.Show();
                return false;
            }
        }

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
            //string mMessage;
            //if (GlobalVariables.OrderAppLib.CustomerService.ContactIsValid(mDTO, out mMessage) == true)
            //{
            mDTO = GlobalVariables.OrderAppLib.CustomerService.ContactSaveRecord(mDTO);
            this.txtContactID.Text = mDTO.ContactID.ToString();
            response = true;

            //}
            //else
            //{
            //    //show error here
            //    response = false;
            //}

            return response;
        }
        private bool CreateAddressRecords()
        {
            try
            {


                if (txtCustAddLine1.Text != "")
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
                }

                else
                {

                    txtAddressID.Text = "0";




                }

                //if (txtBillToAddLine1.Text != "")
                //{

                //    DTOAddress BillToDTOAddress = new DTOAddress();
                //    BillToDTOAddress.AddressID = 0;
                //    BillToDTOAddress.AddressLine1 = txtBillToAddLine1.Text.Replace("\r\n", "").Trim();
                //    BillToDTOAddress.AddressLine2 = txtBillToAddLine2.Text.Replace("\r\n", "").Trim();
                //    BillToDTOAddress.AddressTypeID = 3;
                //    BillToDTOAddress.CitySuburb = txtBillToCitySuburb.Text;
                //    BillToDTOAddress.PostalZipCode = txtBillToPostal.Text;
                //    BillToDTOAddress.SYSStateID = int.Parse(ddlBillToState.SelectedValue);
                //    txtBillToAddressID.Text = CreateAddressRecord(BillToDTOAddress);

                //}
                //else
                //{

                //    txtBillToAddressID.Text = "0";

                //}

                //if (txtShipToAddLine1.Text != "")
                //{

                //    DTOAddress ShipToDTOAddress = new DTOAddress();
                //    ShipToDTOAddress.AddressID = 0;
                //    ShipToDTOAddress.AddressLine1 = txtShipToAddLine1.Text.Replace("\r\n", "").Trim();
                //    ShipToDTOAddress.AddressLine2 = txtShipToAddLine2.Text.Replace("\r\n", "").Trim();
                //    ShipToDTOAddress.AddressTypeID = 5;
                //    ShipToDTOAddress.CitySuburb = txtShipToCitySuburb.Text;
                //    ShipToDTOAddress.PostalZipCode = txtShipToPostal.Text;
                //    ShipToDTOAddress.SYSStateID = int.Parse(ddlShipToState.SelectedValue);
                //    txtShipToAddressID.Text = CreateAddressRecord(ShipToDTOAddress);
                //}
                //else
                //{

                //    txtShipToAddressID.Text = "0";

                //}

                return true;

            }
            catch
            {
                return false;
            }

        }


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

        protected void btnSaveProviderCustomer_Click(object sender, EventArgs e)
        {
            lblProviderProductErrorMessage.Text = "";
            m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];
            // pnlCustomer.Enabled = true;

            string _message = "";



            if (hidProviderCustomerID.Value == "")
            {
                if (!CheckCustomerCodeifExist(txtCustomerCodePop.Text, false, out _message))
                {

                    if (txtEndDatePop.Text == "")
                    {

                        txtEndDatePop.Text = "09/09/9999";

                    }

                    AddProvider();
                    ShowProviderCustomer();
                }
                else
                {

                    lblProviderProductErrorMessage.Text = _message;
                    mpeCustomerProvider.Show();
                }
            }
            else
            {
                if (!CheckCustomerCodeifExist(txtCustomerCodePop.Text, true, out _message))
                {

                    if (txtEndDatePop.Text == "")
                    {

                        txtEndDatePop.Text = "09/09/9999";

                    }

                    UpdateProviderCustomer();
                    ShowProviderCustomer();
                }
                else
                {

                    lblProviderProductErrorMessage.Text = _message;
                    mpeCustomerProvider.Show();
                }
            }




        }


        private void UpdateProviderCustomer()
        {


            DataRow[] mRow = m_ProviderTable.Select("ProviderID= " + hidProviderID.Value);

            if (mRow.Count() > 0)
            {



                mRow[0]["EndDate"] = txtEndDatePop.Text;
                mRow[0]["StartDate"] = txtStartDatePop.Text;
                mRow[0]["ProviderCustomerCode"] = txtCustomerCodePop.Text;

                m_ProviderTable.AcceptChanges();

            }
        }


        private Boolean CheckCustomerCodeifExist(string CustomerCode, Boolean isUpdate, out string message) // Reintegration Request
        {
            try
            {
                Boolean result = false;
                message = "";




                int CustomerID = 0;


                int.TryParse(hidProviderCustomerID.Value, out CustomerID);

                string CustomerCodeText = txtCustomerCodePop.Text.Replace(" ", "").Trim();
                //var respProductCode = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductCode(int.Parse(Session["RefID"].ToString()), txtProductCodePop.Text);
                DTOProviderCustomer mDTO = GlobalVariables.OrderAppLib.ProviderService.CheckifCustomerCodeExist_by_SalesOrgAndProvider(int.Parse(Session["RefID"].ToString()), int.Parse(ddlProvider.SelectedValue), CustomerID, CustomerCodeText);

                if (mDTO != null && mDTO.ProviderCustomerID != 0)
                {
                    message = "Customer Code already exists for this Provider for Customer  " + mDTO.CustomerName;
                    return true;

                }
                else
                {


                    result = false;
                }


                if (!isUpdate)
                {
                    DataRow[] mRow = m_ProviderTable.Select("ProviderID = " + ddlProvider.SelectedValue);


                    if (mRow.Count() > 0)
                    {
                        result = true;

                        message = "Provider entry already exists for this Customer.";
                        mpeCustomerProvider.Show();
                    }
                    else
                    {
                        result = false;
                    }
                }



                return result;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return true;
            }

        }

        private void AddProvider()
        {


            if (txtEndDatePop.Text == "9999-09-09" || txtEndDatePop.Text == "09/09/9999")
            {

                txtEndDatePop.Text = "";
            }

            m_ProviderTable.Rows.Add(0, int.Parse(ddlProvider.SelectedValue), ddlProvider.SelectedItem.Text, txtCustomerCodePop.Text, txtStartDatePop.Text, txtEndDatePop.Text);


        }

        protected void btnCancelCustomerProvider_Click(object sender, EventArgs e)
        {
            //pnlCustomer.Enabled = true;
        }


        protected void btnCancelAddresses_Click(object sender, EventArgs e)
        {
            //  pnlCustomer.Enabled = true;
        }


        protected void btnSaveAddresses_Click(object sender, EventArgs e)
        {
            // pnlCustomer.Enabled = true;
            txtOfficeAddress.Text = txtCustAddLine1.Text + " " + txtCustAddLine2.Text + " " + txtCustCitySuburb.Text + " " + txtCustPostal.Text;

        }

        protected void btnAddProvider_Click(object sender, EventArgs e)
        {
            txtStartDatePop.Text = DateTime.Today.ToString(GlobalVariables.GetDateFormat);
            ddlProvider.Enabled = true;
            hidProviderCustomerID.Value = "";
            txtCustomerCodePop.Text = "";
            txtEndDatePop.Text = "";
            lblProviderProductErrorMessage.Text = "";
            // pnlCustomer.Enabled = false;
            txtCustomerCodePop.Focus();
            mpeCustomerProvider.Show();
        }


        //gvOfficeNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnRelease").Visible = false;
        protected void gvProviderCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int RowIndex = Convert.ToInt32(e.CommandArgument);

            //((CheckBox)gvProviderCustomer.Rows[RowIndex].FindControl("cbxProviderCustomerCreateOrder")).Enabled = false;

        }

        protected void gvProviderCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string CustomerCode = HttpUtility.HtmlDecode(e.Row.Cells[1].Text).Trim();
                string Startdate = HttpUtility.HtmlDecode(e.Row.Cells[2].Text).Trim();
                string Enddate = HttpUtility.HtmlDecode(e.Row.Cells[3].Text).Trim();

                if (Enddate == "") Enddate = "09/09/9999";

                DateTime _startDate = DateTime.ParseExact(Startdate, "dd/MM/yyyy", null);
                DateTime _endDate = DateTime.ParseExact(Enddate, "dd/MM/yyyy", null);

                if (
                    (_startDate <= DateTime.Now.Date)
                    && (_endDate >= DateTime.Now.Date)
                    && CustomerCode.Trim() != ""
                    )
                {
                    ((CheckBox)e.Row.FindControl("cbxProviderCustomerCreateOrder")).Enabled = true;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("cbxProviderCustomerCreateOrder")).Enabled = false;
                }


                string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                if (String.Format(dateformat, Enddate) == "09/09/9999")
                {
                    e.Row.Cells[3].Text = "";
                }
                else
                {
                    try
                    {
                        e.Row.Cells[3].Text = String.Format(dateformat, Enddate, null);
                    }
                    catch
                    {
                        e.Row.Cells[3].Text = String.Format("{0:dd/MM/yyyy}", Enddate, null);
                    }
                }

            }
        }

        protected void img_updateProvider_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn;
            GridViewRow mrow;

            imgbtn = (ImageButton)sender;
            mrow = (GridViewRow)imgbtn.NamingContainer;


            gvProviderCustomer.SelectedIndex = mrow.RowIndex;
            string ProviderID = gvProviderCustomer.SelectedDataKey.Value.ToString();
            string ProviderCustomerID = gvProviderCustomer.SelectedDataKey.Values[1].ToString();


            hidProviderID.Value = ProviderID;
            hidProviderCustomerID.Value = ProviderCustomerID;
            m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];

            DataRow[] mRow = m_ProviderTable.Select("ProviderID = " + ProviderID);

            if (mRow.Count() > 0)
            {
                string ProviderName = mRow[0]["ProviderName"].ToString();
                ddlProvider.SelectedValue = ddlProvider.Items.FindByText(ProviderName).Value;

                ddlProvider.Enabled = false;


                if (mRow[0]["EndDate"].ToString() == "9999-09-09" || mRow[0]["EndDate"].ToString() == "09/09/9999")
                {

                    txtEndDatePop.Text = "";
                }
                else
                {

                    txtEndDatePop.Text = mRow[0]["EndDate"].ToString();

                }



                txtStartDatePop.Text = mRow[0]["StartDate"].ToString();
                txtCustomerCodePop.Text = mRow[0]["ProviderCustomerCode"].ToString();

                lblProviderProductErrorMessage.Text = "";



                mpeCustomerProvider.Show();

                //  mpeCustomer.Show();
            }
        }


        protected void lbManageAddress_Click(object sender, EventArgs e)
        {

            ddlCustState.SelectedValue = ddlState.SelectedValue;
            //ddlBillToState.SelectedValue = ddlState.SelectedValue;
            //ddlShipToState.SelectedValue = ddlState.SelectedValue;

            if (txtAddressID.Text == "0")
            {
                txtCustAddLine1.Text = "";
                txtCustAddLine2.Text = "";
                txtCustCitySuburb.Text = "";
                txtCustPostal.Text = "";
            }

            //if (txtBillToAddressID.Text == "0")
            //{
            //    txtBillToAddLine1.Text = "";
            //    txtBillToAddLine2.Text = "";
            //    txtBillToCitySuburb.Text = "";
            //    txtBillToPostal.Text = "";
            //}

            //if (txtShipToAddressID.Text == "0")
            //{
            //    txtShipToAddLine1.Text = "";
            //    txtShipToAddLine2.Text = "";
            //    txtShipToCitySuburb.Text = "";
            //    txtShipToPostal.Text = "";
            //}

            //   pnlCustomer.Enabled = false;
            mpeAddresses.Show();
        }
        private void FillProvider(int SalesOrgID)
        {
            try
            {

                DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);
                List<DTOProvider> SortedList = mDTO.OrderBy(o => o.ProviderName).ToList();
                ddlProvider.DataSource = SortedList;
                ddlProvider.DataBind();

                string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                if (mDTO != null && mDTO.Count == 1)
                {
                    m_ProviderTable = (DataTable)Session[hidProviderCustomerTempID.Value];
                    if (txtEndDatePop.Text == "")
                    {

                        txtEndDatePop.Text = "09/09/9999";

                    }
                    txtStartDatePop.Text = string.Format(dateformat, DateTime.Now); // DateTime.Today.ToString(GlobalVariables.GetDateFormat);
                    AddProvider();
                    ShowProviderCustomer();
                }

            }
            catch (Exception ex)
            {
                lblWarning.Text = ex.Message;
            }
        }
        private void PopulateDropdownStates()
        {


            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();

            ddlState.DataSource = respState;
            ddlState.DataBind();

            ddlCustState.DataSource = respState;
            ddlCustState.DataBind();

            //ddlBillToState.DataSource = respState;
            //ddlBillToState.DataBind();

            //ddlShipToState.DataSource = respState;
            //ddlShipToState.DataBind();

            //  ddlStateForCustomerView.DataSource = respState;
            //  ddlStateForCustomerView.DataBind();

        }





        protected void gvProvider_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvProviderCustomer.PageIndex = e.NewPageIndex;
        }

        protected void gvProvider_PageIndexChanged(object sender, EventArgs e)
        {

            ShowProviderCustomer();
        }

        protected void btnCancelCustomer1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Organizations/ViewOrganizations.aspx");
        }

        protected void btnSaveCustomerBusiness_Click(object sender, EventArgs e)
        {
            int CustomerID = 0;
        
            int.TryParse(txtCustomerID.Text, out CustomerID);
            string message = "";

            if (CheckCustomerName(CustomerID, out message))
            {
                SaveCustomer();
            }
            else
            {

                mpeCustomerExist.Show();
                lblCustomerNameIfExist.Text = message;

            }

          
        }

        protected void btnAssignSalesRep_Click(object sender, EventArgs e)
        {
            if (Session["CustomerID"].ToString() == "")
            {
                lblNotificationAssignSalesRep.Text = "When saved, new Customer will be assigned to all Sales Reps in State";
                btnCancelAssignSalesRep.Visible = false;

            }
            else
            {

                lblNotificationAssignSalesRep.Text = "Assign Customer to all Sales Reps in " + ddlState.SelectedItem.Text + "?";
                btnCancelAssignSalesRep.Visible = true;
            }

            mpeAssignSalesReps.Show();
        }

        protected void btnSaveAssignSalesRep_Click(object sender, EventArgs e)
        {

            if (Session["CustomerID"].ToString() == "")
            {
                hidIsAssignSalesRep.Value = "1";

            }
            else
            {  // Assign Customer To All SalesRep

                AssignSalesReptoCustomer(long.Parse(ddlState.SelectedValue.ToString()), long.Parse(Session["RefID"].ToString()), long.Parse(Session["CustomerID"].ToString()));

            }
        }

        private void AssignSalesReptoCustomer(long StateID, long SalesOrgID, long customerId)
        {

            DTOAccountList mDTOList = GlobalVariables.OrderAppLib.AccountService.AccountListByStateIDAndSalesOrgID(StateID, SalesOrgID);

            foreach (DTOAccount item in mDTOList)
            {
                DTOCustomerSalesRepList respIfExistingCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepListSearchSalesRepAndCustomer(item.AccountID, customerId);

                if (respIfExistingCustomer.Count == 0)
                {

                    DTOCustomerSalesRep mDTO = new DTOCustomerSalesRep();

                    mDTO.CustomerID = customerId;
                    mDTO.SalesRepAccountID = item.AccountID;
                    mDTO.StartDate = DateTime.Now;
                    mDTO.EndDate = DateTime.Parse("09/09/9999");
                    mDTO.DateCreated = DateTime.Now;

                    //  GlobalVariables.OrderAppLib.CustomerService.customersa
                    GlobalVariables.OrderAppLib.CustomerService.CustomerSalesRepSaveRecord(mDTO);
                }
            }

        }

        protected void UpdatePanel4_PreRender(object sender, EventArgs e)
        {
            TextBox1.Focus();
        }

        protected void UpdatePanel10_PreRender(object sender, EventArgs e)
        {
            txtCustomerCodePop.Focus();
        }

        protected void btnCustomerExist_Click(object sender, EventArgs e)
        {
            if (btnCustomerExist.Text == "Yes")
            {
                SaveCustomer();
            }
        }
    }
}