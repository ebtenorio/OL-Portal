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

namespace OrderApplication
{
    public partial class WebForm4 : System.Web.UI.Page
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
            this.Title = "Manage Products";
            if (IsPostBack)
            {

            }
            else
            {
                FillUOMDropDownList();
                FillProductGroupDropDownList();
                int SalesOrgID = 0;
                int productgroup = 0;
                int.TryParse(ddlProductGroup.SelectedValue, out productgroup);
                int.TryParse(Session["RefID"].ToString(), out SalesOrgID);
                FillProvider(SalesOrgID);
                FillProductsByGroupID(productgroup);
            }
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
            if (ddlProvider.Items.Count > 0)
            {
                var respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(ProductGroupID, long.Parse(ddlProvider.SelectedValue), 1, int.Parse(Session["PageSize"].ToString()));

                if (respProducts.Count != 0)
                {

                    if (respProducts.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblProductsPages.Text = (respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respProducts.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlProductsPages.Items.Clear();
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

            }
            else
            {
                ProductsPanel.Visible = false;
                gvProducts.DataSource = null;
                gvProducts.DataBind();

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
            lblProduct.Text = "Manage Product";
            lblEndDateError.Text = "";
            txtSortPositionBox.Text = "";
            txtEndDate.Text = "";
            ClearManageProductFields();
            try
            {
                ddlProductGroupSelect.SelectedValue = ddlProductGroup.SelectedValue;
                FillUOMDropDownList();
                ddlUOM.ClearSelection();

                ddlUOM.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            }
            catch
            {

            }
            mpeProduct.Show();
        }

        /// <summary>
        /// Clearing of all Manage Product Controls on Pop up event.
        /// </summary>
        private void ClearManageProductFields()
        {
            txtProductID.Text = "";
            txtGTINCode.Text = "";
            txtSKU.Text = "";
            txtProductDescription.Text = "";
            txtProdCode.Text = "";


        }


        /// <summary>
        /// Product Group selected index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            ddlProductsPages.SelectedValue = PageNumber.ToString();

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
            lblGTINProdW.Visible = false;
            lblSortPositionW.Visible = false;

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

                    //var respProduct = GlobalVariables.OrderAppLib.ProductService.ProductGroupLineListByGroupIDDescription(int.Parse(ddlProductGroup.SelectedValue), txtProductDescription.Text);
                    DTOProduct respProductGTIN = new DTOProduct();
                    DTOProduct respProductCode = new DTOProduct();

                    if (txtGTINCode.Text != "")
                    {
                        respProductGTIN = GlobalVariables.OrderAppLib.CatalogService.ProductListByGTINCode(long.Parse(Session["RefID"].ToString()),txtGTINCode.Text);
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


                    int mProductID = 0;

                    int.TryParse(txtProductID.Text, out mProductID);


                    if (txtEndDate.Text == "")
                    {
                        txtEndDate.Text = "09/09/9999";
                    }

                    if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
                    {

                        lblEndDateError.Text = "End date should be greater than or equal to date yesterday";
                        mpeProduct.Show();
                        return;
                    }

                    CreateProductRecord();
                    if (txtProductID.Text != "")
                    {
                        CreateGroupLineRecord();

                        //DTOProviderProduct mDTO = new DTOProviderProduct();

                        //mDTO.StartDate = DateTime.Parse("2014-01-01");
                        //mDTO.ProviderProductCode = txtProdCode.Text;
                        //mDTO.ProviderProductID = 0;
                        //mDTO.ProductID = int.Parse(txtProductID.Text);
                        //mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
                        //mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

                        //mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);
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
                        respProductGTIN = GlobalVariables.OrderAppLib.CatalogService.ProductListByGTINCode(long.Parse(Session["RefID"].ToString()),txtGTINCode.Text);
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

                    //int mProductID = 0;

                    //int.TryParse(txtProductID.Text, out mProductID);

                    //if (GlobalVariables.OrderAppLib.CatalogService.CheckPRoductNameIfexistinGroup(txtProductDescription.Text, mProductID, int.Parse(ddlProductGroup.SelectedValue)))
                    //{
                    //    mpeProductAlreadyExistOnGroup.Show();
                    //    mpeProduct.Show();

                    //}
                    //else
                    //{
                    if (txtEndDate.Text == "")
                    {
                        txtEndDate.Text = "09/09/9999";
                    }

                    if (DateTime.Parse(txtEndDate.Text) < DateTime.Today.AddDays(-1))
                    {

                        lblEndDateError.Text = "End date should be greater than or equal to date yesterday";
                        mpeProduct.Show();
                        return;
                    }


                    //if (GlobalVariables.OrderAppLib.CatalogService.CheckPRoductCodeandGTINIfexistinProduct(txtProdCode.Text, txtGTINCode.Text, mProductID, int.Parse(Session["RefID"].ToString())))
                    //{
                    //    mpeGTINProductCodeAlreadyExist.Show();
                    //    mpeProduct.Show();
                    //}
                    //else
                    //{


                    DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(long.Parse(ddlProvider.SelectedValue), int.Parse(txtProductID.Text));
                    UpdateProductRecord(mDTOProduct);


                    //DTOProviderProduct mDTO = GlobalVariables.OrderAppLib.ProviderService.providerpr

                    //mDTO.StartDate = DateTime.Parse("2014-01-01");
                    //mDTO.ProviderProductCode = txtProdCode.Text;
                    //mDTO.ProviderProductID = ;
                    //mDTO.ProductID = int.Parse(txtProductID.Text);
                    //mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
                    //mDTO.EndDate = DateTime.Parse(txtEndDate.Text);

                    //mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderProductSaveRecord(mDTO);

                    UpdateGroupLineRecord(int.Parse(mDTOProduct.ProductID.ToString()));
                    FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                    ////}


                    //if (respProductCode.ProductID == 0 && respProductGTIN.ProductID == 0)
                    //{
                    //    DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.ProductService.ProductListByID(int.Parse(txtProductID.Text));
                    //    UpdateProductRecord(mDTOProduct);
                    //    UpdateGroupLineRecord(int.Parse(mDTOProduct.ProductID.ToString()));
                    //    FillProductsByGroupID(int.Parse(ddlProductGroup.SelectedValue));
                    //}
                    //else
                    //{

                    //    mpeGTINProductCodeAlreadyExist.Show();
                    //    mpeProduct.Show();
                    //}
                    //}

                    //var respProduct = GlobalVariables.OrderAppLib.ProductService.ProductGroupLineListByGroupIDDescription(int.Parse(ddlProductGroup.SelectedValue), txtProductDescription.Text);


                    //if (respProduct.Count < 1)
                    //{


                    //}
                    //else
                    //{
                    //    mpeProductAlreadyExistOnGroup.Show();
                    //    mpeProduct.Show();

                    //}



                }
                else
                {
                    mpeProduct.Show();
                }

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
            int ProductID = 0;

            switch (e.CommandName)
            {
                case "View":
                    Session["ManageMode"] = "Edit";
                    lblProduct.Text = "Manage Product";
                    lblEndDateError.Text = "";
                    ClearManageProductFields();
                    ProductID = int.Parse(((Label)(gvProducts.Rows[RowIndex].FindControl("lblProductID"))).Text);
                    FillManageProductControlFields(ProductID);
                    mpeProduct.Show();
                    break;
            }

        }

        /// <summary>
        /// Populate All control fields on the Manage Product Pop up
        /// </summary>
        /// <param name="ProductID"></param>
        private void FillManageProductControlFields(int ProductID)
        {



            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByID(long.Parse(ddlProvider.SelectedValue), ProductID);
            DTOProductGroupLine mGroupLine = GlobalVariables.OrderAppLib.CatalogService.ProductGroupLineListByProductID(ProductID);

            txtProductID.Text = mDTOProduct.ProductID.ToString();
            txtGTINCode.Text = mDTOProduct.GTINCode.ToString();
            txtProdCode.Text = mDTOProduct.ProductCode;
            txtSortPositionBox.Text = mGroupLine.SortPosition.ToString();
            txtProductDescription.Text = mDTOProduct.ProductDescription.ToString();
            txtSKU.Text = mDTOProduct.PrimarySKU.ToString();
            ddlProductGroupSelect.SelectedValue = mGroupLine.ProductGroupID.ToString();
            ddlUOM.SelectedValue = mDTOProduct.UOMID.ToString();
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
            mDTO.SortPosition = int.Parse(txtSortPositionBox.Text);
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
            mDTO.SortPosition = int.Parse(txtSortPosition.Text);
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
            mDTO.SortPosition = int.Parse(txtSortPosition.Text);
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
        private void CreateProductRecord()
        {
            DTOProduct mDTO = new DTOProduct();
            mDTO.ProductID = 0;
            mDTO.SalesOrgID = int.Parse(Session["RefID"].ToString());
            mDTO.GTINCode = txtGTINCode.Text;
            mDTO.ProductBrandID = 101;
            mDTO.ProductCategoryID = 101;
            mDTO.ProductCode = txtProdCode.Text;
            mDTO.PrimarySKU = int.Parse(txtSKU.Text);
            mDTO.ProductDescription = txtProductDescription.Text;
            mDTO.UOMID = int.Parse(ddlUOM.SelectedValue);
            mDTO.ProviderID = int.Parse(ddlProvider.SelectedValue);
            mDTO.Inactive = false;
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
                this.txtProductID.Text = mDTO.ProductID.ToString();
            }
            else
            {
                lblProductErrorMessage.Text = mMessage;
                mpeProductAlreadyExistOnGroup.Show();
            }
        }

        //CASE Generated Code 6/24/2013 12:23:19 PM Lazy Dog 3.3.1.0
        private void UpdateProductRecord(DTOProduct mDTOProduct)
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
            }
            else
            {
                lblProductErrorMessage.Text = mMessage;
                mpeProductAlreadyExistOnGroup.Show();
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