using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PL.PersistenceServices.Enumerations;
using PL.PersistenceServices.IDataContracts;
using PL.PersistenceServices.DTOS;
using OrderLinc.DTOs;
using System.Transactions;

namespace OrderApplication
{
    public partial class WebForm5 : System.Web.UI.Page
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
                // cmpVal1.ValueToCompare = DateTime.Now.ToString(GlobalVariables.GetDateFormat);
                try
                {
                    ddlProductGroupSelect.SelectedValue = Session["SelectedGroup"].ToString();
                }
                catch
                {

                }

                lblEndDateError.Text = "";
                txtSortPositionBox.Text = "";
                txtEndDate.Text = "";
                lblProductErrorMessage1.Text = "";
                txtProductID.Text = "0";
                hidProviderTempID.Value = Guid.NewGuid().ToString();
                Session[hidProviderTempID.Value] = GetTable();
                int SalesOrgID = 0;
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);
                FillProvider(SalesOrgID);

                try
                {
                    cbEndDateProd.Format = GlobalVariables.GetDateFormat;
                    cbStartDateProd.Format = GlobalVariables.GetDateFormat;

                }
                catch
                {


                }

                if (Session["ManageMode"].ToString() == "Create")
                {

                    FillUOMDropDownList();
                    ddlUOM.ClearSelection();
                    FillProductGroupDropDownList();
                    //  ddlUOM.Items.Insert(0, new ListItem(string.Empty, string.Empty));



                    ShowProviderProduct();

                    ClearManageProductFields();
                }
                else
                {

                    FillUOMDropDownList();
                    ddlUOM.ClearSelection();
                    FillProductGroupDropDownList();
                    //  ddlUOM.Items.Insert(0, new ListItem(string.Empty, string.Empty));


                    // lblProduct.Text = "Manage Product";
                    lblEndDateError.Text = "";
                    ClearManageProductFields();
                    int ProductID = int.Parse(Session["ProductID"].ToString());
                    FillManageProductControlFields(ProductID);
                    ListProviderProductbyProductID(ProductID);
                }


            }

        }


        private void ListProviderProductbyProductID(int ProductID)
        {

            DTOProviderProductList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(ProductID);


            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            m_ProviderTable.Rows.Clear();

            m_ProviderTable.AcceptChanges();

            string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

            foreach (DTOProviderProduct mPro in mDTO)
            {
                DataRow mRow = m_ProviderTable.NewRow();
                if (String.Format(dateformat, mPro.EndDate) == "09/09/9999")
                {
                    mRow["EndDate"] = null;
                }
                else
                {
                    mRow["EndDate"] = String.Format(dateformat, mPro.EndDate);
                }
                
                mRow["StartDate"] = String.Format(dateformat, mPro.StartDate);
                mRow["ProviderProductCode"] = mPro.ProviderProductCode;
                mRow["ProviderName"] = mPro.ProviderName;
                mRow["ProviderID"] = mPro.ProviderID;
                mRow["ProviderProductID"] = mPro.ProviderProductID;
                mRow["Discount"] = mPro.Discount;
                m_ProviderTable.Rows.Add(mRow);
                m_ProviderTable.AcceptChanges();

            }

            if (m_ProviderTable != null && m_ProviderTable.Rows.Count > 0)
            {
                DataRow[] foundRows = m_ProviderTable.Select("", "ProviderName ASC");
                DataTable dt = foundRows.CopyToDataTable();
                Session[hidProviderTempID.Value] = dt;
                gvProvider.DataSource = dt;
                gvProvider.DataBind();

            }
            else
            {

                Session[hidProviderTempID.Value] = m_ProviderTable;
                gvProvider.DataSource = m_ProviderTable;
                gvProvider.DataBind();

            }



        }
        private void FillManageProductControlFields(int ProductID)
        {

            lblProductErrorMessage1.Text = "";

            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(0, ProductID);
            DTOProductGroupLine mGroupLine = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineListByProductID(ProductID);

            txtProductID.Text = mDTOProduct.ProductID.ToString();
            txtGTINCode.Text = mDTOProduct.GTINCode.ToString();
            txtProdCode.Text = mDTOProduct.ProductCode;
            txtSortPositionBox.Text = mGroupLine.SortPosition.ToString();
            txtProductDescription.Text = mDTOProduct.ProductDescription.ToString();
            txtSKU.Text = mDTOProduct.PrimarySKU.ToString();
            try
            {
                ddlProductGroupSelect.SelectedValue = mGroupLine.ProductGroupID.ToString();
            }
            catch
            {

            }
            try
            {
                ddlUOM.SelectedValue = mDTOProduct.UOMID.ToString();
            }
            catch
            {

            }
            if (mDTOProduct.EndDate.ToShortDateString() != "1/1/0001" && mDTOProduct.EndDate.ToShortDateString() != "9/9/9999")
            {
                txtEndDate.Text = mDTOProduct.EndDate.ToShortDateString();
            }
            else
            {

                txtEndDate.Text = "";
            }
        }
        /// <summary>
        /// Populate Product Group DropDownList on Add Orderline Pop up 
        /// </summary>
        private void FillProductGroupDropDownList()
        {
             var respProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            
            ddlProductGroupSelect.DataSource = respProductGroup;
            ddlProductGroupSelect.DataBind();

        }

        public DataTable GetTable()
        {
            //
            // Here we create a DataTable with four columns.
            //
            m_ProviderTable = new DataTable();
            m_ProviderTable.Columns.Add("ProviderProductID", typeof(int));
            m_ProviderTable.Columns.Add("ProviderID", typeof(int));
            m_ProviderTable.Columns.Add("ProviderName", typeof(string));
            m_ProviderTable.Columns.Add("ProviderProductCode", typeof(string));
            m_ProviderTable.Columns.Add("StartDate", typeof(string));       
            m_ProviderTable.Columns.Add("EndDate", typeof(string));
            m_ProviderTable.Columns.Add("Discount", typeof(float));


            return m_ProviderTable;
        }




        protected void btnSaveGTINChanged_Click(object sender, EventArgs e)
        {
            try
            {
                DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(long.Parse(ddlProvider.SelectedValue), int.Parse(txtProductID.Text));

                UpdateProductRecord(mDTOProduct);
                UpdateGroupLineRecord(int.Parse(mDTOProduct.ProductID.ToString()));

                foreach (GridViewRow mRow in gvProvider.Rows)
                {

                    string mProviderID = gvProvider.DataKeys[mRow.RowIndex].Value.ToString();
                    string mProviderProductID = gvProvider.DataKeys[mRow.RowIndex].Values[1].ToString();

                    DTOProviderProduct mDTO = new DTOProviderProduct();


                    mDTO.ProviderProductCode = mRow.Cells[1].Text;
                    mDTO.ProviderProductID = int.Parse(mProviderProductID);
                    mDTO.ProductID = int.Parse(txtProductID.Text);
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
                        throw new Exception("Invalid Enddate format.");

                    }

                    try
                    {
                        mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, dateformat, null);
                    }
                    catch
                    {
                        throw new Exception("Invalid Startdate format.");
                    }



                    mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

                }

                // FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                Session["SelectedGroup"] = ddlProductGroupSelect.SelectedValue;
                Response.Redirect("~/ProductCatalogs/ViewProducts.aspx");
            }
            catch (Exception ex)
            {

                lblEndDateError.Text = ex.Message;
            }

        }

        private Boolean CheckProductCodeifExist(string ProductCode, Boolean isUpdate, out string message) // Reintegration Request
        {
            try
            {
                Boolean result = false;
                message = "";




                int ProductID = 0;

                int.TryParse(txtProductID.Text, out ProductID);

                //var respProductCode = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductCode(int.Parse(Session["RefID"].ToString()), txtProductCodePop.Text);
                Int64 otherProductID = GlobalVariables.OrderAppLib.ProviderService.CheckifProductCodeExist_by_SalesOrgAndProvider(int.Parse(Session["RefID"].ToString()), int.Parse(ddlProvider.SelectedValue), ProductID, txtProductCodePop.Text);
                if (otherProductID != 0)
                {
                    DTOProduct mProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(int.Parse(ddlProvider.SelectedValue), otherProductID);

                    DTOProductGroupLine mGroupLine = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineListByProductID(otherProductID);

                    DTOProductGroup mGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListByID(mGroupLine.ProductGroupID);

                    message = "Product Code already exists for this Provider on GTIN " + mProduct.GTINCode + " in Product Group " + mGroup.ProductGroupText + ".";
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
                        message = "Provider entry already exists for this Product.";
                        return true;
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

        protected void btnSaveProviderProduct_Click(object sender, EventArgs e)
        {

            bool _hasAffectedOrder = false;
            string OrderNumber = "";
            DateTime _ReleaseDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            DateTime _StartDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            DateTime _EndDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);

            if (this.txtStartDatePop.Text.Trim() == "")
            {
                _StartDate = DateTime.ParseExact("09/09/9999", "dd/MM/yyyy", null);
            }
            else
            {
                _StartDate = DateTime.ParseExact(this.txtStartDatePop.Text, "dd/MM/yyyy", null);
            }

            if (this.txtEndDatePop.Text.Trim() == "")
            {
                _EndDate = DateTime.ParseExact("09/09/9999", "dd/MM/yyyy", null);
            }
            else
            {
                _EndDate = DateTime.ParseExact(this.txtEndDatePop.Text, "dd/MM/yyyy", null);
            }

            int _productID = 0;
            if (txtProductID.Text.Trim() != "")
            {
                _productID = int.Parse(txtProductID.Text);
            }

            _hasAffectedOrder = GlobalVariables.OrderAppLib.CatalogService.HasAffectedOrderByDateChange(
                int.Parse(Session["RefID"].ToString()),
                _productID,
                this.txtProductCodePop.Text.Trim(),
                _StartDate,
                _EndDate,
                out OrderNumber,
                out _ReleaseDate
                );

            if (_hasAffectedOrder)
            {
                this.lblUpdateProviderProduct.Text = String.Format("Order No. {0} has Release date {1}.", OrderNumber, string.Format("{0:dd/MM/yyyy}", _ReleaseDate, null));
                this.lblUpdateProviderProductConfirm.Text = "Are you sure?";
                this.mpeProductProvider.Show();
                this.mpeUpdateProviderProduct.Show();
            }
            else
            {
                this.UpdateProviderProductGrid();
            }

        }


        public void UpdateProviderProductGrid()
        {
            lblProductErrorMessage1.Text = "";
            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            string _message = "";

            if (hidProviderID.Value == "")
            {
                if (!CheckProductCodeifExist(txtProductCodePop.Text, false, out _message))
                {
                    if (txtEndDatePop.Text == "9999-09-09" || txtEndDatePop.Text == "09/09/9999")
                    {
                        txtEndDatePop.Text = "";
                    }

                    if (this.txtDiscount.Text.Trim() == "") this.txtDiscount.Text = "0.00";
                    m_ProviderTable.Rows.Add(0, int.Parse(ddlProvider.SelectedValue), ddlProvider.SelectedItem.Text, txtProductCodePop.Text, txtStartDatePop.Text, txtEndDatePop.Text, txtDiscount.Text);
                }
                else
                {
                    lblProviderProductErrorMessage.Text = _message;
                    mpeProductProvider.Show();
                }
            }
            else
            {
                if (!CheckProductCodeifExist(txtProductCodePop.Text, true, out _message))
                {
                    DataRow[] mRow = m_ProviderTable.Select("ProviderID = " + hidProviderID.Value);

                    if (mRow.Count() > 0)
                    {
                        if (txtEndDatePop.Text == "9999-09-09" || txtEndDatePop.Text == "09/09/9999")
                        {
                            txtEndDatePop.Text = "";
                        }

                        if (txtDiscount.Text.Trim() == "") this.txtDiscount.Text = "0.00";

                        mRow[0]["EndDate"] = txtEndDatePop.Text;
                        mRow[0]["StartDate"] = txtStartDatePop.Text;
                        mRow[0]["ProviderProductCode"] = txtProductCodePop.Text;
                        mRow[0]["Discount"] = txtDiscount.Text;

                        m_ProviderTable.AcceptChanges();
                    }
                }
                else
                {
                    lblProviderProductErrorMessage.Text = _message;
                    mpeProductProvider.Show();
                }
            }

            ShowProviderProduct();

        }

        protected void img_updateProvider_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn;
            GridViewRow mrow;

            imgbtn = (ImageButton)sender;
            mrow = (GridViewRow)imgbtn.NamingContainer;

            gvProvider.SelectedIndex = mrow.RowIndex;

            string ProviderID = gvProvider.SelectedDataKey.Value.ToString();
            hidProviderID.Value = ProviderID;
            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

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

                if (mRow[0]["Discount"] != null || mRow[0]["Discount"].ToString().Trim() != "")
                {
                    this.txtDiscount.Text = float.Parse(mRow[0]["Discount"].ToString()).ToString("0.00");
                }
                else
                {
                    this.txtDiscount.Text = "0.00";
                }

                txtStartDatePop.Text = mRow[0]["StartDate"].ToString();
                txtProductCodePop.Text = mRow[0]["ProviderProductCode"].ToString();

                lblProviderProductErrorMessage.Text = "";

                mpeProductProvider.Show();
                //  mpeProduct.Show();
            }
        }



        private void FillProvider(int SalesOrgID)
        {

            DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);

            List<DTOProvider> SortedList = mDTO.OrderBy(o => o.ProviderName).ToList();

            ddlProvider.DataSource = SortedList;
            ddlProvider.DataBind();

        }

        private void FillUOMDropDownList()
        {
            var respUOM = GlobalVariables.OrderAppLib.CatalogService.ProductUOMList();

            ddlUOM.DataSource = respUOM;
            ddlUOM.DataBind();


        }

        private void ClearManageProductFields()
        {
            txtProductID.Text = "";
            txtGTINCode.Text = "";
            txtSKU.Text = "1";
            txtProductDescription.Text = "";
            txtProdCode.Text = "";

            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            m_ProviderTable.Rows.Clear();

            Session[hidProviderTempID.Value] = m_ProviderTable;
        }
        private bool IsNumeric(string Number)
        {

            try
            {
                double doub = double.Parse(Number);
                return true;
            }
            catch
            {
                return false;
            }

        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private void UpdateGroupLineRecord(int ProductID)
        {
            DTOProductGroupLine mProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineListByProductID(ProductID);

            DTOProductGroupLine mDTO = new DTOProductGroupLine();
            mDTO.ProductGroupLineID = int.Parse(mProductGroup.ProductGroupLineID.ToString());
            mDTO.ProductGroupID = int.Parse(ddlProductGroupSelect.SelectedValue);
            mDTO.SortPosition = int.Parse(txtSortPositionBox.Text); //int.Parse(mProductGroup.Rows[0]["SortPosition"].ToString());
            mDTO.ProductID = long.Parse(txtProductID.Text);
            mDTO.DefaultQty = int.Parse(mProductGroup.DefaultQty.ToString());


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineSaveRecord(mDTO);
            }
            else
            {
                //show error here
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private void CreateGroupLineRecord()
        {

            //DTOProductGroupLineList mDTOProductGroupLineList = GlobalVariables.OrderAppLib.ProductService.ProductGroupLineListByGroupID(int.Parse(ddlProductGroupSelect.SelectedValue));

            //int SortPosition = (mDTOProductGroupLineList.Count * 10) + 10;

            DTOProductGroupLine mDTO = new DTOProductGroupLine();
            mDTO.ProductGroupLineID = 0;
            mDTO.ProductGroupID = int.Parse(ddlProductGroupSelect.SelectedValue);
            mDTO.SortPosition = 0; // int.Parse(txtSortPositionBox.Text); commented out -  Ringo Ray Piedraverde 26-8-2014
            mDTO.ProductID = int.Parse(txtProductID.Text);
            mDTO.DefaultQty = 0;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 

            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineSaveRecord(mDTO);
                //this.txtProductGroupLineID.Text = mDTO.ProductGroupLineID.ToString();
            }
            else
            {
                lblProductErrorMessage.Text = mMessage;
                mpeProductAlreadyExistOnGroup.Show();
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private Boolean CreateProductRecord()
        {
            DTOProduct mDTO = new DTOProduct();
            mDTO.ProductID = 0;
            mDTO.SalesOrgID = int.Parse(Session["RefID"].ToString());
            mDTO.GTINCode = txtGTINCode.Text;
            mDTO.ProductBrandID = 101;
            mDTO.ProductCategoryID = 101;
            mDTO.PrimarySKU = int.Parse(txtSKU.Text);
            mDTO.ProductDescription = txtProductDescription.Text;
            mDTO.UOMID = int.Parse(ddlUOM.SelectedValue);
            mDTO.Inactive = false;


            //mDTO.ProductCode = txtProdCode.Text;
            //mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
            //mDTO.StartDate = DateTime.Parse("01/01/2014");
            //mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductSaveRecord(mDTO);
                this.txtProductID.Text = mDTO.ProductID.ToString();
                return true;
            }
            else
            {

                lblProductErrorMessage.Text = mMessage;
                mpeProductAlreadyExistOnGroup.Show();
                // mpeProduct.Show();
                return false;
                // mpeProductAlreadyExistOnGroup.Show();
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private Boolean UpdateProductRecord(DTOProduct mDTOProduct)
        {
            DTOProduct mDTO = new DTOProduct();
            mDTO.ProductID = mDTOProduct.ProductID;
            mDTO.SalesOrgID = mDTOProduct.SalesOrgID;
            mDTO.GTINCode = txtGTINCode.Text;
            mDTO.ProductBrandID = mDTOProduct.ProductBrandID;
            mDTO.ProductCategoryID = mDTOProduct.ProductCategoryID;
            mDTO.ProductCode = txtProdCode.Text;
            mDTO.PrimarySKU = int.Parse(txtSKU.Text);
            mDTO.ProviderID = 0;
            mDTO.ProductDescription = txtProductDescription.Text;
            mDTO.UOMID = int.Parse(ddlUOM.SelectedValue.ToString());
            mDTO.Inactive = mDTOProduct.Inactive;
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

            if (txtDiscount.Text.Trim() == "") txtDiscount.Text = "0.00";
            mDTO.Discount = float.Parse(txtDiscount.Text);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductSaveRecord(mDTO);
                return true;
            }
            else
            {
                lblProductErrorMessage.Text = mMessage;
                mpeProductAlreadyExistOnGroup.Show();
                //  mpeProduct.Show();
                return false;
            }
        }

        protected void btnUpdateProviderProductOK_Click(object sender, EventArgs e)
        {
            this.UpdateProviderProductGrid();
        }

        protected void btnSavePRoduct_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveProviderProduct();
            }
            catch (Exception ex)
            {
                lblProductErrorMessage1.Text = "Error - " + ex.Message;
            }
        }

        public void SaveProviderProduct()
        {
            lblGTINProdW.Visible = false;
            lblSortPositionW.Visible = false;
            lblProductErrorMessage1.Text = "";

            if (gvProvider.Rows.Count > 0)
            {

                if (string.IsNullOrEmpty(txtGTINCode.Text))
                {
                    lblProductErrorMessage.Text = "GTIN Code cannnot be empty.";
                }
                else
                {
                    DTOProduct product = GlobalVariables.OrderAppLib.CatalogService.ProductListByGTINCode(long.Parse(Session["RefID"].ToString()), txtGTINCode.Text);
                    if (product != null && product.ProductID != int.Parse(txtProductID.Text))
                    {
                        DTOProductGroupLine mGroupLine = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineListByProductID(int.Parse(product.ProductID.ToString()));

                        DTOProductGroup mGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListByID(mGroupLine.ProductGroupID);

                        lblProductErrorMessage.Text = "GTINCode already exists in Product Group " + mGroup.ProductGroupText + ".";
                        mpeProductAlreadyExistOnGroup.Show();
                        return;
                    }

                }

                if (Session["ManageMode"].ToString() == "Create")
                {
                    //if (IsNumeric(txtSKU.Text) == false)
                    //{
                    //    lblSKUW.Visible = true;
                    //}
                    //if (IsNumeric(txtSortPositionBox.Text) == false)
                    //{
                    //    //lblSortPositionW.Text = "Fill in Sort Position";
                    //    //lblSortPositionW.Visible = true;
                    //}
                    //if (txtGTINCode.Text == "" && txtProdCode.Text == "")
                    //{
                    //    lblGTINProdW.Visible = true;
                    //}

                    //      var respSalesOrg = GlobalVariables.OrderAppLib.CustomerService.SalesOrgListByID(int.Parse(Session["RefID"].ToString()));

                    //if (respSalesOrg.UseGTINExport == true && txtGTINCode.Text == "")
                    //{
                    //    lblGTINProdW.Text = "Use of GTIN is required.";
                    //    lblGTINProdW.Visible = true;
                    //}

                    //if (respSalesOrg.UseGTINExport == false && txtProdCode.Text == "")
                    //{
                    //    lblGTINProdW.Text = "Use of Product Code is required.";
                    //    lblGTINProdW.Visible = true;

                    //}  


                    if (lblSortPositionW.Visible != true && lblGTINProdW.Visible != true)
                    {

                        //var respProduct = GlobalVariables.OrderAppLib.ProductService.ProductGroupLineListByGroupIDDescription(int.Parse(ddlProductGroup.SelectedValue), txtProductDescription.Text);
                        DTOProduct respProductGTIN = new DTOProduct();
                        DTOProduct respProductCode = new DTOProduct();

                        int mProductID = 0;

                        int.TryParse(txtProductID.Text, out mProductID);


                        using (TransactionScope mScope = new TransactionScope()) // TRANSACTION DEPENDENCIES
                        {
                            if (CreateProductRecord())
                            {
                                if (txtProductID.Text != "")
                                {
                                    CreateGroupLineRecord();

                                    foreach (GridViewRow mRow in gvProvider.Rows)
                                    {

                                        string mProviderID = gvProvider.DataKeys[mRow.RowIndex].Value.ToString();

                                        DTOProviderProduct mDTO = new DTOProviderProduct();


                                        mDTO.ProviderProductCode = mRow.Cells[1].Text;
                                        mDTO.ProviderProductID = 0;
                                        mDTO.ProductID = int.Parse(txtProductID.Text);
                                        mDTO.ProviderID = int.Parse(mProviderID);
                                        mDTO.Discount = float.Parse(mRow.Cells[2].Text);


                                        string EndDate = HttpUtility.HtmlDecode(mRow.Cells[4].Text);
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
                                            mDTO.StartDate = DateTime.ParseExact(mRow.Cells[3].Text, dateformat, null);
                                        }
                                        catch
                                        {

                                            mDTO.StartDate = DateTime.ParseExact(mRow.Cells[3].Text, "dd/MM/yyyy", null);
                                        }

                                        mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

                                    }
                                }
                                mScope.Complete();
                                Session["SelectedGroup"] = ddlProductGroupSelect.SelectedValue;
                                Response.Redirect("~/ProductCatalogs/ViewProducts.aspx");

                            }



                        }
                        // FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));



                    }
                    else
                    {

                        // mpeProduct.Show();
                    }

                }
                else
                {

                    if (lblSortPositionW.Visible != true && lblGTINProdW.Visible != true)
                    {
                        DTOProduct respProductGTIN = new DTOProduct();
                        DTOProduct respProductCode = new DTOProduct();

                        if (txtGTINCode.Text != "")
                        {
                            respProductGTIN = GlobalVariables.OrderAppLib.CatalogService.ProductListByGTINCode(long.Parse(Session["RefID"].ToString()), txtGTINCode.Text);
                        }
                        else
                        {
                            respProductGTIN.ProductID = 0;
                        }

                        if (txtProdCode.Text != "")
                        {
                            respProductCode = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductCode(int.Parse(Session["RefID"].ToString()), txtProdCode.Text);
                        }
                        else
                        {
                            respProductCode.ProductID = 0;
                        }


                        if (txtEndDate.Text == "")
                        {
                            txtEndDate.Text = "09/09/9999";
                        }

                        //if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
                        //{

                        //    lblEndDateError.Text = "End date should be greater than or equal to date yesterday";
                        //    // mpeProduct.Show();
                        //    return;
                        //}

                        //Check if GTIN if change 

                        DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(0, int.Parse(txtProductID.Text));


                        if (mDTOProduct != null && mDTOProduct.GTINCode != txtGTINCode.Text)
                        {

                            Boolean result = GlobalVariables.OrderAppLib.CatalogService.CheckProductIfithastSentOrder(int.Parse(Session["RefID"].ToString()), int.Parse(txtProductID.Text));

                            if (result)
                            {
                                lblGtINChangeNotification.Text = "An order has already been sent for this GTIN code. Are you sure? ";
                                mpeGTINChanged.Show();
                                return;
                            }

                        }


                        if (UpdateProductRecord(mDTOProduct))
                        {
                            UpdateGroupLineRecord(int.Parse(mDTOProduct.ProductID.ToString()));

                            foreach (GridViewRow mRow in gvProvider.Rows)
                            {

                                string mProviderID = gvProvider.DataKeys[mRow.RowIndex].Value.ToString();
                                string mProviderProductID = gvProvider.DataKeys[mRow.RowIndex].Values[1].ToString();

                                DTOProviderProduct mDTO = new DTOProviderProduct();

                                mDTO.ProviderProductCode = mRow.Cells[1].Text;
                                mDTO.Discount = float.Parse(mRow.Cells[2].Text);
                                mDTO.ProviderProductID = int.Parse(mProviderProductID);
                                mDTO.ProductID = int.Parse(txtProductID.Text);
                                mDTO.ProviderID = int.Parse(mProviderID);

                                string EndDate = HttpUtility.HtmlDecode(mRow.Cells[4].Text);
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
                                    mDTO.StartDate = DateTime.ParseExact(mRow.Cells[3].Text, dateformat, null);
                                }
                                catch
                                {

                                    mDTO.StartDate = DateTime.ParseExact(mRow.Cells[3].Text, "dd/MM/yyyy", null);
                                }

                                // Additional Code here?
                                mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);


                            }

                            /// FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                            /// 
                            Session["SelectedGroup"] = ddlProductGroupSelect.SelectedValue;
                            Response.Redirect("~/ProductCatalogs/ViewProducts.aspx");
                        }

                    }
                    else
                    {
                        //   mpeProduct.Show();
                    }
                }
            }
            else
            {
                lblProductErrorMessage.Text = "Product cannot be ordered unless a Provider Product Code is added..";
                mpeProductAlreadyExistOnGroup.Show();
                // mpeProduct.Show();
            }
        }


        protected void btnAddProvider_Click(object sender, EventArgs e)
        {            
            txtStartDatePop.Text = DateTime.Today.ToString(GlobalVariables.GetDateFormat);
            txtEndDatePop.Text = "";
            txtProductCodePop.Text = "";
            ddlProvider.SelectedIndex = 0;
            ddlProvider.Enabled = true;
            hidProviderID.Value = "";
            txtDiscount.Text = "0.00";
            lblProviderProductErrorMessage.Text = "";
            mpeProductProvider.Show();
        }


        public void ShowProviderProduct()
        {
            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            if (m_ProviderTable != null && m_ProviderTable.Rows.Count > 0)
            {
                DataRow[] foundRows = m_ProviderTable.Select("", "ProviderName ASC");
                DataTable dt = foundRows.CopyToDataTable();
                Session[hidProviderTempID.Value] = dt;
                gvProvider.DataSource = dt;
                gvProvider.DataBind();
            }
            else
            {
                Session[hidProviderTempID.Value] = m_ProviderTable;
                gvProvider.DataSource = m_ProviderTable;
                gvProvider.DataBind();
            }
        }

        protected void gvProvider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Enddate = HttpUtility.HtmlDecode(e.Row.Cells[3].Text);

                if (String.Format("{0:MM/dd/yyyy}", Enddate) == "09/09/9999")
                {
                    e.Row.Cells[3].Text = "";
                }
                else
                {
                    e.Row.Cells[3].Text = String.Format("{0:MM/dd/yyyy}", Enddate);
                }
            }
        }

        protected void gvProvider_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProvider.PageIndex = e.NewPageIndex;
        }

        protected void gvProvider_PageIndexChanged(object sender, EventArgs e)
        {
            this.ShowProviderProduct();
        }

        protected void btnProdClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProductCatalogs/ViewProducts.aspx");
        }

        protected void ddlProductGroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PageIndexProduct"] = "1";
        }
    }
}