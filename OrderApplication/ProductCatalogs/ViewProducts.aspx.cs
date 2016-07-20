using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PL.PersistenceServices.Enumerations;
using PL.PersistenceServices.IDataContracts;
using PL.PersistenceServices.DTOS;
using OrderLinc.DTOs;
using System.Transactions;

namespace OrderApplication
{
    public partial class WebForm4 : System.Web.UI.Page
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
            if (IsPostBack)
            {

            }
            else
            {
                FillUOMDropDownList();
                FillProductGroupDropDownList();


                try
                {
                    ddlProductGroup.SelectedValue = Session["SelectedGroup"].ToString();
                }
                catch(Exception ex)
                {

                }
                int SalesOrgID = 0;
                int productgroup = 0;
                int.TryParse(ddlProductGroup.SelectedValue, out productgroup);
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);
                FillProvider(SalesOrgID);
                FillProductsByGroupID(productgroup);

                hidProviderTempID.Value = Guid.NewGuid().ToString();
                Session[hidProviderTempID.Value] = GetTable();
            }
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


            return m_ProviderTable;
        }


        private void FillProvider(int SalesOrgID)
        {

            DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);

            ddlProvider.DataSource = mDTO;
            ddlProvider.DataBind();

        }

        private void FillUOMDropDownList()
        {
            var respUOM = GlobalVariables.OrderAppLib.CatalogService.ProductUOMList();

            ddlUOM.DataSource = respUOM;
            ddlUOM.DataBind();


        }


        /// <summary>
        /// Populate Product Group DropDownList on Add Orderline Pop up 
        /// </summary>
        private void FillProductGroupDropDownList()
        {
            var respProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

            ddlProductGroup.DataSource = respProductGroup;
            ddlProductGroup.DataBind();

            ddlProductGroupSelect.DataSource = respProductGroup;
            ddlProductGroupSelect.DataBind();

        }


        /// <summary>
        /// Populate Products based on their Product GroupID
        /// </summary>
        /// <param name="ProductGroupID"></param>
        private void FillProductsByGroupID(int ProductGroupID)
        {
            int pageIndex = 1;
            int.TryParse("" + Session["PageIndexProduct"], out pageIndex);

            var respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(ProductGroupID, 0, pageIndex, int.Parse(Session["PageSize"].ToString()));

                if (respProducts.Count != 0)
                {

                    if (respProducts.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblProductsPages.Text = (respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString() )+ 1;
                        ddlProductsPages.Items.Clear();
                        if (count == 0)
                        {
                            ddlProductsPages.Items.Add(new ListItem("1"));
                        }
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlProductsPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblProductsPages.Text = (respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString());
                        ddlProductsPages.Items.Clear();
                        if (count == 0)
                        {
                            ddlProductsPages.Items.Add(new ListItem("1"));
                        }
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlProductsPages.Items.Add(newCount);
                        }
                    }
                }

                if (respProducts.Count != 0)
                {
                    ProductsPanel.Visible = true;
                    gvProducts.DataSource = respProducts;
                    gvProducts.DataBind();
                }
                else
                {
                    ProductsPanel.Visible = false;
                    gvProducts.DataSource = null;
                    gvProducts.DataBind();
                }
                try
                {
                    ddlProductsPages.SelectedValue = pageIndex.ToString();
                }
                catch { 
                
                }
        }

        /// <summary>
        /// Link button Onclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnAddProdGroup_Click(object sender, EventArgs e)
        {
            txtProductGroupName.Text = "";
            txtSortPosition.Text = "";
            lblProductGroupHeader.Text = "Manage Product Group";
            txtProductGroupLineID.Text = "";
            mpeProductGroup.Show();
        }



        /// <summary>
        /// Add Product Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Session["ManageMode"] = "Create";
            Session["SelectedGroup"] = ddlProductGroup.SelectedValue;
            Session["PageIndexProduct"] = ddlProductsPages.SelectedValue;
            //lblProduct.Text = "Manage Product";
            //lblEndDateError.Text = "";
            //txtSortPositionBox.Text = "";
            //txtEndDate.Text = "";
            //lblProductErrorMessage1.Text = "";
            //txtProductID.Text = "0";

            //ClearManageProductFields();
            //try
            //{
            //    ddlProductGroupSelect.SelectedValue = ddlProductGroup.SelectedValue;
            //    FillUOMDropDownList();
            //    ddlUOM.ClearSelection();

            //    ddlUOM.Items.Insert(0, new ListItem(string.Empty, string.Empty));

            //    int SalesOrgID = 0;
            //    int.TryParse(Session["RefID"].ToString(), out SalesOrgID);
            //    FillProvider(SalesOrgID);
            //    ShowProviderProduct();
            //}
            //catch
            //{

            //}

            //mpeProduct.Show();

            Response.Redirect("~/ProductCatalogs/ViewProductCatalogs.aspx");
        }

        /// <summary>
        /// Clearing of all Manage Product Controls on Pop up event.
        /// </summary>
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


        /// <summary>
        /// Product Group selected index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["PageIndexProduct"] = "1";
            FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));

        }



        /// <summary>
        /// Products Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProductsPaging(object sender, EventArgs e)
        {

            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlProductsPages.SelectedValue);

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
                    if (PageNumber < int.Parse(lblProductsPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblProductsPages.Text);
                    break;
            }

            try
            {
                ddlProductsPages.SelectedValue = PageNumber.ToString();
            }
            catch { }

            var respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(ddlProductGroup.SelectedValue), long.Parse(ddlProvider.SelectedValue), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvProducts.DataSource = respProducts;
            gvProducts.DataBind();

        }


        /// <summary>
        /// Button Save Product Group Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveProductGroup_Click(object sender, EventArgs e)
        {


            if (txtProductGroupLineID.Text == "")
            {
                //var respProductGroupLine = GlobalVariables.OrderAppLib.ProductService.ProductGroupListByName(txtProductGroupName.Text, int.Parse(Session["RefID"].ToString()));

                //if (respProductGroupLine.Count() < 1)
                //{
                CreateProductGroupRecord();
                FillProductGroupDropDownList();
                FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                mpeProductGroup.Hide();
                //}
                //else
                //{
                //    mpeProductGroup.Show();
                //    mpeProductGroupAlreadyExist.Show();
                //}


            }
            else
            {
                //var respProductGroupLine = GlobalVariables.OrderAppLib.ProductService.ProductGroupListByName(txtProductGroupName.Text, int.Parse(Session["RefID"].ToString()));

                //if (respProductGroupLine.Count() < 1)
                //{
                DTOProductGroup mDTOProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListByID(int.Parse(ddlProductGroup.SelectedValue));
                UpdateProductGroupRecord(mDTOProductGroup);
                FillProductGroupDropDownList();
                FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                mpeProductGroup.Hide();
                //}
                //else
                //{
                //    mpeProductGroup.Show();
                //    mpeProductGroupAlreadyExist.Show();
                //}


            }




        }


        /// <summary>
        /// Button Save Product Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSavePRoduct_Click(object sender, EventArgs e)
        {

            try
            {
                lblGTINProdW.Visible = false;
                lblSortPositionW.Visible = false;
                lblProductErrorMessage1.Text = "";
                txtSortPositionBox.Text = "0";
                if (gvProvider.Rows.Count > 0)
                {

                    if (Session["ManageMode"].ToString() == "Create")
                    {
                        //if (IsNumeric(txtSKU.Text) == false)
                        //{
                        //    lblSKUW.Visible = true;
                        //}
                        if (IsNumeric(txtSortPositionBox.Text) == false)
                        {
                            //lblSortPositionW.Text = "Fill in Sort Position";
                            //lblSortPositionW.Visible = true;
                        }
                        if (txtGTINCode.Text == "" && txtProdCode.Text == "")
                        {
                            lblGTINProdW.Visible = true;
                        }

                        var respSalesOrg = GlobalVariables.OrderAppLib.CustomerService.SalesOrgListByID(int.Parse(Session["RefID"].ToString()));

                        if (respSalesOrg.UseGTINExport == true && txtGTINCode.Text == "")
                        {
                            lblGTINProdW.Text = "Use of GTIN is required.";
                            lblGTINProdW.Visible = true;
                        }

                        if (respSalesOrg.UseGTINExport == false && txtProdCode.Text == "")
                        {
                            lblGTINProdW.Text = "Use of Product Code is required.";
                            lblGTINProdW.Visible = true;

                        }


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


                                            string EndDate = HttpUtility.HtmlDecode(mRow.Cells[3].Text);
                                            if (EndDate.Replace(" ", "").Trim() == "")
                                            {

                                                EndDate = "09/09/9999";
                                            }


                                            mDTO.EndDate = DateTime.ParseExact(EndDate, "MM/dd/yyyy", null);
                                            mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, "MM/dd/yyyy", null); 

                                            mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

                                        }
                                    }


                                }


                                mScope.Complete();
                            }
                            FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));


                        }
                        else
                        {

                            mpeProduct.Show();
                        }

                    }
                    else
                    {

                        if (IsNumeric(txtSortPositionBox.Text) == false)
                        {
                            lblSortPositionW.Text = "Fill in Sort Position";
                            lblSortPositionW.Visible = true;
                        }
                        if (txtGTINCode.Text == "" && txtProdCode.Text == "")
                        {
                            lblGTINProdW.Visible = true;
                        }
                        //if (lblSortPositionW.Visible != true && lblSKUW.Visible != true && lblGTINProdW.Visible != true)

                        if (int.Parse(txtSortPositionBox.Text) > 9999)
                        {
                            lblSortPositionW.Text = "Sort Position Over the Limit";
                            lblSortPositionW.Visible = true;
                        }

                        var respSalesOrg = GlobalVariables.OrderAppLib.CustomerService.SalesOrgListByID(int.Parse(Session["RefID"].ToString()));

                        if (respSalesOrg.UseGTINExport == true && txtGTINCode.Text == "")
                        {
                            lblGTINProdW.Text = "Use of GTIN is required.";
                            lblGTINProdW.Visible = true;

                        }

                        if (respSalesOrg.UseGTINExport == false && txtProdCode.Text == "")
                        {

                            lblGTINProdW.Text = "Use of Product Code is required.";
                            lblGTINProdW.Visible = true;

                        }


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
                            //    mpeProduct.Show();
                            //    return;
                            //}








                            //Check if GTIN if change 

                            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(long.Parse(ddlProvider.SelectedValue), int.Parse(txtProductID.Text));


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
                                    mDTO.ProviderProductID = int.Parse(mProviderProductID);
                                    mDTO.ProductID = int.Parse(txtProductID.Text);
                                    mDTO.ProviderID = int.Parse(mProviderID);

                                    string EndDate = HttpUtility.HtmlDecode(mRow.Cells[3].Text);
                                    if (EndDate.Replace(" ", "").Trim() == "")
                                    {

                                        EndDate = "09/09/9999";
                                    }


                                    mDTO.EndDate = DateTime.ParseExact(EndDate, "MM/dd/yyyy", null);

                                    mDTO.StartDate = DateTime.ParseExact(mRow.Cells[2].Text, "MM/dd/yyyy", null); 

                                    mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

                                }

                                FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));

                            }

                        }
                        else
                        {
                            mpeProduct.Show();
                        }


                    }

                }
                else
                {
                    lblProductErrorMessage.Text = "Product cannot be ordered unless a Provider Product Code is added..";
                    mpeProductAlreadyExistOnGroup.Show();
                    mpeProduct.Show();
                }

            }
            catch (Exception ex)
            {
                lblProductErrorMessage1.Text = "Error - " + ex.Message;
                mpeProduct.Show();

            }
        }


        /// <summary>
        /// Row Command Event of Gridview Products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            //int ProductID = 0;

            switch (e.CommandName)
            {
                case "View":
                    Session["ManageMode"] = "Edit";
                    lblProduct.Text = "Manage Product";
                    lblEndDateError.Text = "";
                    ClearManageProductFields();
                    Session["ProductID"] = int.Parse(((Label)(gvProducts.Rows[RowIndex].FindControl("lblProductID"))).Text);

                    Session["SelectedGroup"] = ddlProductGroup.SelectedValue;
                    Session["PageIndexProduct"] = ddlProductsPages.SelectedValue;

                    Response.Redirect("~/ProductCatalogs/ViewProductCatalogs.aspx");
                    //FillManageProductControlFields(ProductID);
                    //ListProviderProductbyProductID(ProductID);
                    //mpeProduct.Show();
                    break;
            }

        }


        private void ListProviderProductbyProductID(int ProductID)
        {

            DTOProviderProductList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(ProductID);


            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            m_ProviderTable.Rows.Clear();

            m_ProviderTable.AcceptChanges();

            foreach (DTOProviderProduct mPro in mDTO)
            {
                DataRow mRow = m_ProviderTable.NewRow();
                mRow["EndDate"] = String.Format("{0:MM/dd/yyyy}", mPro.EndDate);
                mRow["StartDate"] = String.Format("{0:MM/dd/yyyy}", mPro.StartDate);
                mRow["ProviderProductCode"] = mPro.ProviderProductCode;
                mRow["ProviderName"] = mPro.ProviderName;
                mRow["ProviderID"] = mPro.ProviderID;
                mRow["ProviderProductID"] = mPro.ProviderProductID;
                m_ProviderTable.Rows.Add(mRow);
                m_ProviderTable.AcceptChanges();

            }


            gvProvider.DataSource = mDTO;
            gvProvider.DataBind();

        }
        /// <summary>
        /// Populate All control fields on the Manage Product Pop up
        /// </summary>
        /// <param name="ProductID"></param>
        private void FillManageProductControlFields(int ProductID)
        {

            lblProductErrorMessage1.Text = "";

            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(long.Parse(ddlProvider.SelectedValue), ProductID);
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
        /// Image button Edit ProductGroup Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnEditProdGroup_Click(object sender, ImageClickEventArgs e)
        {

            DTOProductGroup mDTOProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListByID(int.Parse(ddlProductGroup.SelectedValue));
            txtProductGroupName.Text = ddlProductGroup.SelectedItem.Text;
            txtProductGroupLineID.Text = ddlProductGroup.SelectedValue.ToString();
            txtSortPosition.Text = mDTOProductGroup.SortPosition.ToString();
            lblProductGroupHeader.Text = "Manage Product Group";
            mpeProductGroup.Show();

        }


        /// <summary>
        /// IsNumeric Validator
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
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


        #region ************************tblProductGroupLine CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProductGroupLine mDTO)
        //{
        //    txtProductGroupLineID.Text = mDTO.ProductGroupLineID.ToString();
        //    txtProductGroupID.Text = mDTO.ProductGroupID.ToString();
        //    txtSortPosition.Text = mDTO.SortPosition.ToString();
        //    txtProductID.Text = mDTO.ProductID.ToString();
        //    txtDefaultQty.Text = mDTO.DefaultQty.ToString();
        //}

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
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProductGroupLine FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProductGroupLine mDTO = new DTOProductGroupLine();
        //    mDTO.ProductGroupLineID = int.Parse(mcurrentRow["ProductGroupLineID"].ToString());
        //    mDTO.ProductGroupID = int.Parse(mcurrentRow["ProductGroupID"].ToString());
        //    mDTO.SortPosition = int.Parse(mcurrentRow["SortPosition"].ToString());
        //    mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //    mDTO.DefaultQty = mcurrentRow["DefaultQty"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProductGroupLine PopulateDTOProductGroupLine(int ObjectID)
        //{
        //      DTOProductGroupLine mDTO = new DTOProductGroupLine();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductGroupLineListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductGroupLineID = int.Parse(mcurrentRow["ProductGroupLineID"].ToString());
        //              mDTO.ProductGroupID = int.Parse(mcurrentRow["ProductGroupID"].ToString());
        //              mDTO.SortPosition = int.Parse(mcurrentRow["SortPosition"].ToString());
        //              mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //              mDTO.DefaultQty = mcurrentRow["DefaultQty"].ToString();
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblProductGroupLine CRUDS *********************************


        #region ************************tblProductData CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProductData mDTO)
        //{
        //    txtProductDataID.Text = mDTO.ProductDataID.ToString();
        //    txtProductID.Text = mDTO.ProductID.ToString();
        //    txtWidth.Text = mDTO.Width.ToString();
        //    txtHeight.Text = mDTO.Height.ToString();
        //    txtLength.Text = mDTO.Length.ToString();
        //    txtFileBin.Text = mDTO.FileBin.ToString();
        //    txtFileName.Text = mDTO.FileName.ToString();
        //    txtOriginPath.Text = mDTO.OriginPath.ToString();
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void CreateRecord()
        //{
        //    DTOProductData mDTO = new DTOProductData();
        //    mDTO.ProductDataID = 0;
        //    mDTO.ProductID = txtProductID.Text;
        //    mDTO.Width = txtWidth.Text;
        //    mDTO.Height = txtHeight.Text;
        //    mDTO.Length = txtLength.Text;
        //    mDTO.FileBin = txtFileBin.Text;
        //    mDTO.FileName = txtFileName.Text;
        //    mDTO.OriginPath = txtOriginPath.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductDataIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductDataCreateRecord(mDTO);
        //        this.txtProductDataID.Text = mDTO.ProductDataID.ToString();
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void UpdateRecord()
        //{
        //    DTOProductData mDTO = new DTOProductData();
        //    mDTO.ProductDataID = txtProductDataID.Text;
        //    mDTO.ProductID = txtProductID.Text;
        //    mDTO.Width = txtWidth.Text;
        //    mDTO.Height = txtHeight.Text;
        //    mDTO.Length = txtLength.Text;
        //    mDTO.FileBin = txtFileBin.Text;
        //    mDTO.FileName = txtFileName.Text;
        //    mDTO.OriginPath = txtOriginPath.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductDataIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductDataUpdateRecord(mDTO);
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProductData FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProductData mDTO = new DTOProductData();
        //    mDTO.ProductDataID = mcurrentRow["ProductDataID"].ToString();
        //    mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //    mDTO.Width = mcurrentRow["Width"].ToString();
        //    mDTO.Height = mcurrentRow["Height"].ToString();
        //    mDTO.Length = mcurrentRow["Length"].ToString();
        //    mDTO.FileBin = mcurrentRow["FileBin"].ToString();
        //    mDTO.FileName = mcurrentRow["FileName"].ToString();
        //    mDTO.OriginPath = mcurrentRow["OriginPath"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProductData PopulateDTOProductData(int ObjectID)
        //{
        //DTOProductData mDTO = new DTOProductData();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductDataListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductDataID = mcurrentRow["ProductDataID"].ToString();
        //              mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //              mDTO.Width = mcurrentRow["Width"].ToString();
        //              mDTO.Height = mcurrentRow["Height"].ToString();
        //              mDTO.Length = mcurrentRow["Length"].ToString();
        //              mDTO.FileBin = mcurrentRow["FileBin"].ToString();
        //              mDTO.FileName = mcurrentRow["FileName"].ToString();
        //              mDTO.OriginPath = mcurrentRow["OriginPath"].ToString();
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblProductData CRUDS *********************************


        #region ************************tblProductCategory CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProductCategory mDTO)
        //{
        //    txtProductCategoryID.Text = mDTO.ProductCategoryID.ToString();
        //    txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //    txtProductCategoryText.Text = mDTO.ProductCategoryText.ToString();
        //    txtParentCategoryID.Text = mDTO.ParentCategoryID.ToString();
        //    txtInActive.Text = mDTO.InActive.ToString();
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void CreateRecord()
        //{
        //    DTOProductCategory mDTO = new DTOProductCategory();
        //    mDTO.ProductCategoryID = 0;
        //    mDTO.SalesOrgID = txtSalesOrgID.Text;
        //    mDTO.ProductCategoryText = txtProductCategoryText.Text;
        //    mDTO.ParentCategoryID = int.Parse(txtParentCategoryID.Text);
        //    mDTO.InActive = txtInActive.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductCategoryIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductCategoryCreateRecord(mDTO);
        //        this.txtProductCategoryID.Text = mDTO.ProductCategoryID.ToString();
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void UpdateRecord()
        //{
        //    DTOProductCategory mDTO = new DTOProductCategory();
        //    mDTO.ProductCategoryID = int.Parse(txtProductCategoryID.Text);
        //    mDTO.SalesOrgID = txtSalesOrgID.Text;
        //    mDTO.ProductCategoryText = txtProductCategoryText.Text;
        //    mDTO.ParentCategoryID = int.Parse(txtParentCategoryID.Text);
        //    mDTO.InActive = txtInActive.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductCategoryIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductCategoryUpdateRecord(mDTO);
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProductCategory FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProductCategory mDTO = new DTOProductCategory();
        //    mDTO.ProductCategoryID = int.Parse(mcurrentRow["ProductCategoryID"].ToString());
        //    mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //    mDTO.ProductCategoryText = mcurrentRow["ProductCategoryText"].ToString();
        //    mDTO.ParentCategoryID = int.Parse(mcurrentRow["ParentCategoryID"].ToString());
        //    mDTO.InActive = mcurrentRow["InActive"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProductCategory PopulateDTOProductCategory(int ObjectID)
        //{
        //DTOProductCategory mDTO = new DTOProductCategory();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductCategoryListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductCategoryID = int.Parse(mcurrentRow["ProductCategoryID"].ToString());
        //              mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //              mDTO.ProductCategoryText = mcurrentRow["ProductCategoryText"].ToString();
        //              mDTO.ParentCategoryID = int.Parse(mcurrentRow["ParentCategoryID"].ToString());
        //              mDTO.InActive = (Boolean)mcurrentRow["InActive"];
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblProductCategory CRUDS *********************************


        #region ************************tblProductGroup CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProductGroup mDTO)
        //{
        //    txtProductGroupID.Text = mDTO.ProductGroupID.ToString();
        //    txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //    txtSortPosition.Text = mDTO.SortPosition.ToString();
        //    txtProductGroupText.Text = mDTO.ProductGroupText.ToString();
        //    txtInActive.Text = mDTO.InActive.ToString();
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private void CreateProductGroupRecord()
        {
            
            //DTOProductGroupList mDTOProductGroupList = GlobalVariables.OrderAppLib.ProductService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

            //int SortPosition = (mDTOProductGroupList.Count() * 10) + 10;

            DTOProductGroup mDTO = new DTOProductGroup();
            mDTO.ProductGroupID = 0;
            mDTO.SalesOrgID = int.Parse(Session["RefID"].ToString());
            mDTO.SortPosition = 0;//int.Parse(txtSortPosition.Text);
            mDTO.ProductGroupText = txtProductGroupName.Text;
            mDTO.InActive = false;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductGroupIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductGroupSaveRecord(mDTO);
                //this.txtProductGroupID.Text = mDTO.ProductGroupID.ToString();
            }
            else
            {
                lblErrorMessageProductGroup.Text = mMessage;
                mpeProductGroup.Show();
                mpeProductGroupAlreadyExist.Show();
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private void UpdateProductGroupRecord(DTOProductGroup mDTOProductGroup)
        {
            DTOProductGroup mDTO = new DTOProductGroup();
            mDTO.ProductGroupID = mDTOProductGroup.ProductGroupID;
            mDTO.SalesOrgID = mDTOProductGroup.SalesOrgID;
            mDTO.SortPosition = 0;//int.Parse(txtSortPosition.Text);
            mDTO.ProductGroupText = txtProductGroupName.Text;
            mDTO.InActive = mDTOProductGroup.InActive;


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.CatalogService.ProductGroupIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.CatalogService.ProductGroupSaveRecord(mDTO);
            }
            else
            {
                lblErrorMessageProductGroup.Text = mMessage;
                mpeProductGroup.Show();
                mpeProductGroupAlreadyExist.Show();
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProductGroup FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProductGroup mDTO = new DTOProductGroup();
        //    mDTO.ProductGroupID = int.Parse(mcurrentRow["ProductGroupID"].ToString());
        //    mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //    mDTO.SortPosition = int.Parse(mcurrentRow["SortPosition"].ToString());
        //    mDTO.ProductGroupText = mcurrentRow["ProductGroupText"].ToString();
        //    mDTO.InActive = mcurrentRow["InActive"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProductGroup PopulateDTOProductGroup(int ObjectID)
        //{
        //DTOProductGroup mDTO = new DTOProductGroup();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductGroupListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductGroupID = int.Parse(mcurrentRow["ProductGroupID"].ToString());
        //              mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //              mDTO.SortPosition = int.Parse(mcurrentRow["SortPosition"].ToString());
        //              mDTO.ProductGroupText = mcurrentRow["ProductGroupText"].ToString();
        //              mDTO.InActive = (Boolean)mcurrentRow["InActive"];
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblProductGroup CRUDS *********************************


        #region ************************tblProductBrand CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProductBrand mDTO)
        //{
        //    txtProductBrandID.Text = mDTO.ProductBrandID.ToString();
        //    txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //    txtProductBrandText.Text = mDTO.ProductBrandText.ToString();
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void CreateRecord()
        //{
        //    DTOProductBrand mDTO = new DTOProductBrand();
        //    mDTO.ProductBrandID = 0;
        //    mDTO.SalesOrgID = int.Parse(txtSalesOrgID.Text);
        //    mDTO.ProductBrandText = txtProductBrandText.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductBrandIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductBrandCreateRecord(mDTO);
        //        this.txtProductBrandID.Text = mDTO.ProductBrandID.ToString();
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void UpdateRecord()
        //{
        //    DTOProductBrand mDTO = new DTOProductBrand();
        //    mDTO.ProductBrandID = txtProductBrandID.Text;
        //    mDTO.SalesOrgID = int.Parse(txtSalesOrgID.Text);
        //    mDTO.ProductBrandText = txtProductBrandText.Text;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.ProductSVC.ProductBrandIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.ProductSVC.ProductBrandUpdateRecord(mDTO);
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProductBrand FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProductBrand mDTO = new DTOProductBrand();
        //    mDTO.ProductBrandID = mcurrentRow["ProductBrandID"].ToString();
        //    mDTO.SalesOrgID = int.Parse(mcurrentRow["SalesOrgID"].ToString());
        //    mDTO.ProductBrandText = mcurrentRow["ProductBrandText"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProductBrand PopulateDTOProductBrand(int ObjectID)
        //{
        //DTOProductBrand mDTO = new DTOProductBrand();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductBrandListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductBrandID = mcurrentRow["ProductBrandID"].ToString();
        //              mDTO.SalesOrgID = int.Parse(mcurrentRow["SalesOrgID"].ToString());
        //              mDTO.ProductBrandText = mcurrentRow["ProductBrandText"].ToString();
        //          }
        //       return mDTO
        //      }

        //}

        #endregion ************************End of tblProductBrand CRUDS *********************************


        #region ************************tblProduct CRUDS ******************************************

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOProduct mDTO)
        //{
        //    txtProductID.Text = mDTO.ProductID.ToString();
        //    txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //    txtGTINCode.Text = mDTO.GTINCode.ToString();
        //    txtProductBrandID.Text = mDTO.ProductBrandID.ToString();
        //    txtProductCategoryID.Text = mDTO.ProductCategoryID.ToString();
        //    txtProductCode.Text = mDTO.ProductCode.ToString();
        //    txtPrimarySKU.Text = mDTO.PrimarySKU.ToString();
        //    txtProductDescription.Text = mDTO.ProductDescription.ToString();
        //    txtUOMID.Text = mDTO.UOMID.ToString();
        //    txtInactive.Text = mDTO.Inactive.ToString();
        //}

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
                lblProductErrorMessage1.Text = mMessage;
                mpeProduct.Show();
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
            mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
            mDTO.ProductDescription = txtProductDescription.Text;
            mDTO.UOMID = int.Parse(ddlUOM.SelectedValue.ToString());
            mDTO.Inactive = mDTOProduct.Inactive;
            mDTO.StartDate = DateTime.Parse("01/01/2014");
            mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

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
                mpeProduct.Show();
                return false;
            }
        }

        protected void img_btnDelete_Product_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton m_btn;
            GridViewRow m_row;

            m_btn = (ImageButton)sender;
            m_row = (GridViewRow)m_btn.NamingContainer;

            gvProducts.SelectedIndex = m_row.RowIndex;
            string mProductID = gvProducts.DataKeys[m_row.RowIndex].Value.ToString();


            GlobalVariables.OrderAppLib.CatalogService.ProductDeleteRecord(long.Parse(mProductID), long.Parse(Session["AccountID"].ToString()));

            FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
        }

        protected void img_btnDelete_Product_Click1(object sender, ImageClickEventArgs e)
        {

            GlobalVariables.OrderAppLib.CatalogService.ProductGroupDeleteRecord(int.Parse(ddlProductGroup.SelectedValue));

            FillProductGroupDropDownList();
            FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
        }


        // Reintegration 8-18-2014 Ringo Ray Piedraverde


        protected void gvProducts_DataBound(object sender, GridViewRowEventArgs e)
        {
            //foreach (GridViewRow item in gvProducts.Rows)
            //{

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string str = item.Cells[5].Text;
                //if (str == "9999-09-09" || str == "09/09/9999")
                //{

                //    item.Cells[5].Text = "";
                //}
                DateTime _EndDate = ((DTOProduct)e.Row.DataItem).EndDate;

                if (String.Format("{0:MM/dd/yyyy}", _EndDate) == "09/09/9999")
                {
                    e.Row.Cells[5].Text = "";
                }
                else
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                    try
                    {

                        e.Row.Cells[5].Text = String.Format(dateformat, _EndDate);
                    }
                    catch
                    {
                        e.Row.Cells[5].Text = String.Format("{0:dd/MM/yyyy}", _EndDate);
                    }

                   
                }

            }
            // }
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
                txtEndDatePop.Text = mRow[0]["EndDate"].ToString();
                txtStartDatePop.Text = mRow[0]["StartDate"].ToString();
                txtProductCodePop.Text = mRow[0]["ProviderProductCode"].ToString();

                lblProviderProductErrorMessage.Text = "";

                mpeProductProvider.Show();
                mpeProduct.Show();
            }
        }

        protected void btnAddProvider_Click(object sender, EventArgs e)
        {
            txtStartDatePop.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtEndDatePop.Text = "";
            txtProductCodePop.Text = "";
            ddlProvider.SelectedIndex = 0;
            ddlProvider.Enabled = true;
            hidProviderID.Value = "";
            lblProviderProductErrorMessage.Text = "";


            mpeProductProvider.Show();
            mpeProduct.Show();

        }

        public void ShowProviderProduct()
        {
            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            gvProvider.DataSource = m_ProviderTable;
            gvProvider.DataBind();

            Session[hidProviderTempID.Value] = m_ProviderTable;
            mpeProduct.Show();

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
                    message = "Product Code already exists for this Provider on GTIN " + txtGTINCode.Text;
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
            lblProductErrorMessage1.Text = "";
            m_ProviderTable = (DataTable)Session[hidProviderTempID.Value];

            string _message = "";

            if (txtEndDatePop.Text == "")
            {

                txtEndDatePop.Text = "09/09/9999";

            }


            if (hidProviderID.Value == "")
            {

                if (!CheckProductCodeifExist(txtProductCodePop.Text, false, out _message))
                {
                    if (txtEndDatePop.Text == "9999-09-09" || txtEndDatePop.Text == "09/09/9999")
                    {

                        txtEndDatePop.Text = "";
                    }

                    m_ProviderTable.Rows.Add(0, int.Parse(ddlProvider.SelectedValue), ddlProvider.SelectedItem.Text, txtProductCodePop.Text, txtStartDatePop.Text, txtEndDatePop.Text);
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


                        mRow[0]["EndDate"] = txtEndDatePop.Text;
                        mRow[0]["StartDate"] = txtStartDatePop.Text;
                        mRow[0]["ProviderProductCode"] = txtProductCodePop.Text;

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

        protected void gvProvider_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProvider.PageIndex = e.NewPageIndex;
        }

        protected void gvProvider_PageIndexChanged(object sender, EventArgs e)
        {
            ShowProviderProduct();
        }

        protected void btnSaveGTINChanged_Click(object sender, EventArgs e)
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
                mDTO.EndDate = DateTime.Parse(mRow.Cells[3].Text);
                mDTO.StartDate = DateTime.Parse(mRow.Cells[2].Text);

                mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

            }

            FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));

        }

        protected void gvProvider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string str = item.Cells[5].Text;
                //if (str == "9999-09-09" || str == "09/09/9999")
                //{

                //    item.Cells[5].Text = "";
                //}

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

        protected void ddlProductsPages_SelectedIndexChanged(object sender, EventArgs e)
        {

            var respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(ddlProductGroup.SelectedValue), long.Parse(ddlProvider.SelectedValue), int.Parse(ddlProductsPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvProducts.DataSource = respProducts;
            gvProducts.DataBind();
        }






        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOProduct FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOProduct mDTO = new DTOProduct();
        //    mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //    mDTO.SalesOrgID = int.Parse(mcurrentRow["SalesOrgID"].ToString());
        //    mDTO.GTINCode = mcurrentRow["GTINCode"].ToString();
        //    mDTO.ProductBrandID = mcurrentRow["ProductBrandID"].ToString();
        //    mDTO.ProductCategoryID = int.Parse(mcurrentRow["ProductCategoryID"].ToString());
        //    mDTO.ProductCode = mcurrentRow["ProductCode"].ToString();
        //    mDTO.PrimarySKU = mcurrentRow["PrimarySKU"].ToString();
        //    mDTO.ProductDescription = mcurrentRow["ProductDescription"].ToString();
        //    mDTO.UOMID = mcurrentRow["UOMID"].ToString();
        //    mDTO.Inactive = mcurrentRow["Inactive"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        //     private DTOProduct PopulateDTOProduct(int ObjectID)
        //{
        //DTOProduct mDTO = new DTOProduct();
        //      using (DataTable mDT = GlobalVariables.ProductSVC.ProductListByID(ObjectID))
        //      {
        //          if (mDT.Rows.Count > 0)
        //          {
        //              DataRow mcurrentRow = mDT.Rows[0];
        //              mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //              mDTO.SalesOrgID = int.Parse(mcurrentRow["SalesOrgID"].ToString());
        //              mDTO.GTINCode = mcurrentRow["GTINCode"].ToString();
        //              mDTO.ProductBrandID = mcurrentRow["ProductBrandID"].ToString();
        //              mDTO.ProductCategoryID = int.Parse(mcurrentRow["ProductCategoryID"].ToString());
        //              mDTO.ProductCode = mcurrentRow["ProductCode"].ToString();
        //              mDTO.PrimarySKU = mcurrentRow["PrimarySKU"].ToString();
        //              mDTO.ProductDescription = mcurrentRow["ProductDescription"].ToString();
        //              mDTO.UOMID = mcurrentRow["UOMID"].ToString();
        //              mDTO.Inactive = (Boolean)mcurrentRow["Inactive"];
        //          }
        //       return mDTO
        //      }

        //}


        #endregion ************************End of tblProduct CRUDS *********************************









    }
}