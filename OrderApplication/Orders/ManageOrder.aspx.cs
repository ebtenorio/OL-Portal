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
using OrderLinc;
using System.Collections;
using OrderLinc.DTOs;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;

namespace OrderApplication
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        #region #MEMBERS#
        //  public static DataTable OrderLineDataTable = GetOrderLineTable();
        public Boolean SelectionChange = false;
        public static ArrayList OrderLineIDList = new ArrayList();

        public DTOOrderList mAllOfficeOrdersSearchResult;
        public DTOOrderList mAllSalesRepOrdersSearchResult;
        public DTOOrderList mAllNewOrdersSearchResult;

        #endregion

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
                // This is the first time the page is called.
                Session["PageIndexSalesRepSent"] = 1;
                Session["PageIndexAllSent"] = 1;
                Session["PageIndexOfficeSent"] = 1;
                Session["PageIndexOffice"] = 1;
                Session["PageIndexAllOffice"] = 1;
                Session["PageIndexSalesRepNew"] = 1;
                Session["PageIndexOfficeNew"] = 1;

                Session["IsOfficeNewSearchButtonClicked"] = false;
                Session["IsSalesRepNewSearchButtonClicked"] = false;
                Session["IsAllNewSearchButtonClicked"] = false;

                this.calCreateOrderReleaseDate.StartDate = DateTime.Now;
                //  Session["MyTempOrderLineTable"] = GetOrderLineTable();
                //txtOrderDateCreateOrder.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateStateDropdown();

                MultiView1.SetActiveView(InitialLoadPage);

                gvOrderLineCreateOrder.DataSource = null;
                gvOrderLineCreateOrder.DataBind();

                gvCustomerSelect.DataSource = null;
                gvCustomerSelect.DataBind();

                gvCreatedByUserIDs.DataSource = null;
                gvCreatedByUserIDs.DataBind();

                // Show or Hide Discount Field according to Sales Org Config
                gvOrderLineCreateOrder.Columns[5].Visible = true;

                FillNewOrderTreeViews();
                FillSentOrderTreeViews();
                FillProviderDropDownList();
                //int mProviderId = 0;
                //int.TryParse(ddlProviderCreateOrder.SelectedValue.ToString(), out mProviderId);
                //FillProviderWarehouseDropDownList(mProviderId);

                FillProductGroupDropDownList();
                int mProductGroupID = 0;
                int.TryParse(ddlGroupCreateOrder.SelectedValue.ToString(), out mProductGroupID);
                FillProductsByGroupID(mProductGroupID);

                FillProvider(int.Parse(Session["RefID"].ToString()));

                OfficeNewOrdersPagingPanel.Visible = false;

                Session["NodeReturn"] = "Sales RepNewOrders";
                SetViewBasedOnSelectedNodeNewOrders("Sales Rep");

                // Sales Rep is Default. If there are no orders in Sales Rep, Set Active view to Office
                if (gvSalesRepNewOrders.DataSource == null)
                {
                    PopulateOfficeNewOrders(); // Correct
                    MultiView1.SetActiveView(OfficeNewOrders); // Correct

                    // If both Office and Sales Rep are empty, then All New Orders is shown instead.
                    if (gvOfficeNewOrders.DataSource == null)
                    {
                        // PopulateAllNewOrders();
                        MultiView1.SetActiveView(AllNewOrders);

                    }
                }
       
            }

            if (Session["CustomerPageProviderID"] != null && Session["CustomerPageCustomerID"] != null)                        
            {                
                // Set values here and destroy the Session
                string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                this.ClearCreateOrderFields();
                this.ddlProviderCreateOrder.SelectedValue = Session["CustomerPageProviderID"].ToString();
                this.txtCustomerIDCreateOrder.Text = Session["CustomerPageCustomerID"].ToString();
                this.txtStoreNameCreateOrder.Text = Session["CustomerPageCustomerName"].ToString();
                this.txtCustomerCreateOrder.Text = Session["CustomerPageCustomerCode"].ToString();
                this.FillProviderWarehouseDropDownList(int.Parse(Session["CustomerPageProviderID"].ToString()));
                this.PopulateSalesRepDropDownList(long.Parse(Session["AccountID"].ToString()));
                this.txtStateNameCreateOrder.Text = Session["CustomerPageStateName"].ToString();
                this.txtCreateOrderReleaseDate.Text = string.Format(dateformat, DateTime.Now);
                this.txtOrderDateCreateOrder.Text = string.Format(dateformat, DateTime.Now);

                this.InitiateCreateOrder();

                Session["CustomerPageProviderID"] = null;
                Session["CustomerPageCustomerID"] = null;                
            }
           
        }


        private void FillProvider(int SalesOrgID)
        {

            DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);

            DTOProvider mProvider = new DTOProvider();

            mProvider.ProviderID = 0;
            if (mDTO.Count > 1)
            {
                mProvider.ProviderName = "<All Providers>";
                mDTO.Insert(0, mProvider);
            }

            ddlProviderOffice.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList(); ;
            ddlProviderOffice.DataBind();

            ddlProviderAllSent.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList(); ;
            ddlProviderAllSent.DataBind();

            ddlProviderSentOrderSearch.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList(); ;
            ddlProviderSentOrderSearch.DataBind();

            // Populate Providers for New Orders tab
            this.ddlProviderOfficeNewOrder.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList();
            this.ddlProviderOfficeNewOrder.DataBind();

            this.ddlProviderSalesRepNewOrder.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList();
            this.ddlProviderSalesRepNewOrder.DataBind();

            this.ddlProviderAllNewOrder.DataSource = mDTO.OrderBy(x => x.ProviderName).ToList();
            this.ddlProviderAllNewOrder.DataBind();
        }


        private void PopulateStateDropdown()
        {
            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();
            
            ddlStateForCustomerView.DataSource = respState;
            ddlStateForCustomerView.DataBind();

            DTOSYSState mDTOState = new DTOSYSState();
            mDTOState.StateName = "<All States>";
            respState.Insert(0, mDTOState);
            
            this.ddlStatesSalesRepSentOrders.DataSource = respState;
            this.ddlStatesSalesRepSentOrders.DataBind();

            this.ddlStates.DataSource = respState;
            this.ddlStates.DataBind();

            this.ddlStatesOfficeNewOrder.DataSource = respState;
            this.ddlStatesOfficeNewOrder.DataBind();

            this.ddlStatesSalesRepNewOrder.DataSource = respState;
            this.ddlStatesSalesRepNewOrder.DataBind();

            this.ddlStatesAllNewOrder.DataSource = respState;
            this.ddlStatesAllNewOrder.DataBind();

            this.ddlStatesAllSentOrders.DataSource = respState;
            this.ddlStatesAllSentOrders.DataBind();


        }

        /// <summary>
        /// Order Line Pop up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProductLine_Click(object sender, EventArgs e)
        {
            SelectionChange = true;
            FillProductGroupDropDownList();
            if (this.ddlGroupCreateOrder.SelectedIndex != -1)
            {
                FillProductsByGroupID(int.Parse(ddlGroupCreateOrder.SelectedValue));
                mpeOrderLine.Show();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Pre-sell Order", "alert('Cannot create Pre-sell order.');", true);
            }
            
        }

        /// <summary>
        /// Populate Product Group DropDownList on Add Orderline Pop up 
        /// </summary>
        private void FillProductGroupDropDownList()
        {
            SelectionChange = true;

            long selectedProviderID = 0;
            if (ddlProviderCreateOrder.SelectedValue.Trim() != "")
            {
                selectedProviderID = long.Parse(ddlProviderCreateOrder.SelectedValue);
            }


            // Original:
            var respProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID_WithProduct(int.Parse(Session["RefID"].ToString()), int.Parse(selectedProviderID.ToString()));            
           
            // At Default, these should be REGULAR
            bool IsRegularOrder = bool.Parse(this.ddlOrderType.SelectedValue);

            DateTime OrderDate = DateTime.Now;
            DateTime RequestedReleaseDate = DateTime.Now;

            // GET ORDER DATE OF THE CURRENT ORDER
            if (this.txtOrderDateCreateOrder.Text.Trim() != "")
            {
                OrderDate = DateTime.ParseExact(this.txtOrderDateCreateOrder.Text.Trim(), "dd/MM/yyyy", null);
            }

            // GET REQUESTED DATE OF THE CURRENT ORDER: For use with REGULAR ORDERS
            if (this.txtCreateOrderReleaseDate.Text.Trim() != "")
            {
                RequestedReleaseDate = DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text.Trim(), "dd/MM/yyyy", null);
            }

            long selectedOrderID = 0;

            if (this.hidOrderID.Value.Trim() != "")
            {
                selectedOrderID = long.Parse(this.hidOrderID.Value.Trim());
            }

            List<DTOProductGroup> updatedProducGroup = new List<DTOProductGroup>();

            if (!IsRegularOrder)
            {
                // Get the PRE-SELL Product Groups. Note: Existing Orderline Items not yet included.
                foreach (DTOProductGroup productGroup in respProductGroup)
                {
                    int ProductCount = 0;

                    // get all products for the selected provider
                    var products = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupIDandProviderID(
                        productGroup.ProductGroupID,
                        selectedProviderID, 1, 100000);

                    foreach (DTOProduct product in products)
                    {
                        // Product Code has been added here to check if the Product is in the orderline or not.
                        if ((product.StartDate > OrderDate && product.EndDate >= RequestedReleaseDate) // Changed from DateTime.Now
                            || this.isProductExistInOrderline(product.ProductCode))
                        {
                            ProductCount++; // Get product count
                        }
                    }

                    if (ProductCount > 0) updatedProducGroup.Add(productGroup); // if there are products that matches the criteria above, add the product group (for the dropdownlist)
                }
            }
            else // If the OrderType is Regular
            {
                // Get the REGULAR Product Groups. Note: Existing Orderline Items not yet included.
                foreach (DTOProductGroup productGroup in respProductGroup)
                {

                    int ProductCount = 0;

                    var products = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupIDandProviderID(
                        productGroup.ProductGroupID,
                        selectedProviderID, 1, 100000);

                    foreach (DTOProduct product in products)
                    {
                        // USE IsExist here?
                        if (product.StartDate <= RequestedReleaseDate && product.EndDate >= RequestedReleaseDate) ProductCount++;
                    }

                    if (ProductCount > 0) updatedProducGroup.Add(productGroup);
                }
            }

            ddlGroupCreateOrder.DataSource = updatedProducGroup;//respProductGroup;
            ddlGroupCreateOrder.DataBind();
        }

        /// <summary>
        /// Populate Products based on their Product GroupID
        /// </summary>
        /// <param name="ProductGroupID"></param>
        private void FillProductsByGroupID(int ProductGroupID)
        {
            // Get the highest start date here for the Pre-sell orders - all start dates should be future
            SelectionChange = true;

            long selectedProviderID = 0;
            if (ddlProviderCreateOrder.SelectedValue.Trim() != "")
            {
                selectedProviderID = long.Parse(ddlProviderCreateOrder.SelectedValue);
            }

            // Products to Display, Depending on the Selected (or defaultly selected) Provider
            List<DTOProduct> respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupIDandProviderID(ProductGroupID, selectedProviderID, 1, 100000);
           
            // Products currently in the OrderLines
            DataTable currentOrderlines = (DataTable)Session[hidOrderLineTempID.Value]; // OrderLines

            if (currentOrderlines != null)
            {
                for (int iProducts = 0; iProducts < respProducts.Count; iProducts++)
                {
                    for (int iOrderline = 0; iOrderline < currentOrderlines.Rows.Count; iOrderline++)
                    {
                        // long providerID = long.Parse(ddlProviderCreateOrder.SelectedValue);
                        long providerID = selectedProviderID;

                        string productCode = currentOrderlines.Rows[iOrderline]["ProductCode"].ToString();

                        var currentProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByProviderProductCode(providerID, productCode);

                        if (respProducts[iProducts].ProductID == (long)currentOrderlines.Rows[iOrderline]["ProductID"])
                        {
                            // Check if Discount is 0 in OrderLines, output 
                            if ((float)currentOrderlines.Rows[iOrderline]["Discount"] == 0 && !this.isProductExistInOrderline(productCode)) // and that it is in the orderline,
                            {
                                respProducts[iProducts].Discount = float.Parse(currentProduct.Discount.ToString());
                            }
                            else
                            {
                                respProducts[iProducts].Discount = float.Parse(currentOrderlines.Rows[iOrderline]["Discount"].ToString());
                            }
                        }
                    }
                }
            }

            if (respProducts.Count != 0)
            {
                gvProductPerGroup.DataSource = respProducts;
                gvProductPerGroup.DataBind();
            }
            else
            {
                gvProductPerGroup.DataSource = null;
                gvProductPerGroup.DataBind();
            }

            // Modify the Qty TextBox when the Pop-up shows up, retrieving values from the current Orderlines
            var currentProductsRows = gvProductPerGroup.Rows;
            var currentOrderlinesRows = gvOrderLineCreateOrder.Rows;

            // For Pre-Sell : Display Products
            if (this.ddlOrderType.SelectedItem.Text == "Pre-sell")
            {
                foreach (GridViewRow productRow in currentProductsRows)
                {
                    DateTime StartDate = DateTime.Parse(((Label)(productRow.FindControl("lblStartDate"))).Text);

                    DateTime EndDate = DateTime.Parse(((Label)(productRow.FindControl("lblEndDate"))).Text);

                    String ProductID = ((Label)(productRow.FindControl("lblProductId"))).Text;

                    String ProductCode = ((Label)(productRow.FindControl("lblProductCode"))).Text;

                    if (this.txtOrderDateCreateOrder.Text.Trim() == "")
                    {
                        // This block of code is under testing - a product group is not showing up
                        if ((StartDate > DateTime.Today && EndDate >= DateTime.Now) || this.isProductExistInOrderline(ProductCode))
                        {
                            productRow.Visible = true;
                        }
                        else
                        {
                            productRow.Visible = false;
                        }
                    }
                    else
                    {
                        if ((StartDate > DateTime.ParseExact(this.txtOrderDateCreateOrder.Text, "dd/MM/yyyy", null) && EndDate >= DateTime.Now) || this.isProductExistInOrderline(ProductCode))
                        {
                            productRow.Visible = true;
                        }
                        else
                        {
                            productRow.Visible = false;
                        }
                    }
                }
            }

            //FOR REGULAR : Display Products

            else if (this.ddlOrderType.SelectedItem.Text == "Regular")
            {
                DateTime RequestedReleaseDate;

                if (this.txtCreateOrderReleaseDate.Text.Trim() != "")
                {
                    RequestedReleaseDate = DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null);

                    foreach (GridViewRow productRow in currentProductsRows)
                    {
                        DateTime StartDate = DateTime.Parse(((Label)(productRow.FindControl("lblStartDate"))).Text);

                        DateTime EndDate = DateTime.Parse(((Label)(productRow.FindControl("lblEndDate"))).Text);

                        String ProductID = ((Label)(productRow.FindControl("lblProductId"))).Text;

                        String ProductCode = ((Label)(productRow.FindControl("lblProductCode"))).Text;

                        if ((StartDate <= RequestedReleaseDate && RequestedReleaseDate <= EndDate) || this.isProductExistInOrderline(ProductCode))
                        {
                            productRow.Visible = true;
                        }
                        else
                        {
                            productRow.Visible = false;
                        }
                    }
                }
            }

            foreach (GridViewRow productRow in currentProductsRows)
            {                
                for (int iOrderLine = 0; iOrderLine < currentOrderlinesRows.Count; iOrderLine++)
                {                   
                    // Get the current quantity from the OrderLine
                    if (long.Parse(((Label)(productRow.FindControl("lblProductId"))).Text) == (long)currentOrderlines.Rows[iOrderLine]["ProductID"])
                    {
                        ((TextBox)(productRow.FindControl("txtQtyOderLine"))).Text = currentOrderlines.Rows[iOrderLine]["OrderQty"].ToString();                       
                    }        
                }
            }
        }

        private bool isProductExistInOrderline(string productCode)
        {
            DataTable currentOrderlines = (DataTable)Session[hidOrderLineTempID.Value]; // Current OrderLines

            for (int iOrderline = 0; iOrderline < currentOrderlines.Rows.Count; iOrderline++)
            {
                if (productCode == currentOrderlines.Rows[iOrderline]["ProductCode"].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Select Index Change for Product Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroupCreateOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            FillProductsByGroupID(int.Parse(ddlGroupCreateOrder.SelectedValue));
            mpeOrderLine.Show();
        }

        /// <summary>
        /// Populate Treeviews for NEW ORDERS
        /// </summary>
        private void FillNewOrderTreeViews()
        {
            SelectionChange = true;

            DataTable mDTOOrderCounts = GlobalVariables.OrderAppLib.OrderService.OrderGetCount(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(Session["AccountTypeID"].ToString()));

            tvNewOrders.Nodes.Clear();

            TreeNode m_tvItemPrnt = new TreeNode();

            m_tvItemPrnt.Value = "New Orders";
            m_tvItemPrnt.Text = "New Orders";

            this.tvNewOrders.Nodes.Add(m_tvItemPrnt);
            m_tvItemPrnt.SelectAction = TreeNodeSelectAction.None;

            TreeNode m_tvItem = new TreeNode();
            m_tvItem.Value = "Office";
            m_tvItem.Text = "Office (" + mDTOOrderCounts.Rows[0]["OfficeNewOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            m_tvItem = new TreeNode();
            m_tvItem.Value = "Sales Rep";
            m_tvItem.Text = "Sales Rep (" + mDTOOrderCounts.Rows[0]["SalesRepNewOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            m_tvItem = new TreeNode();
            m_tvItem.Value = "All";
            m_tvItem.Text = "All (" + mDTOOrderCounts.Rows[0]["AllNewOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            tvNewOrders.ExpandAll();

        }

        /// <summary>
        /// Populate Treeviews for SENT ORDERS
        /// </summary>
        private void FillSentOrderTreeViews()
        {
            SelectionChange = true;

            DataTable mDTOOrderCounts = GlobalVariables.OrderAppLib.OrderService.OrderGetCount(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(Session["AccountTypeID"].ToString()));
            tvSentOrders.Nodes.Clear();

            TreeNode m_tvItemPrnt = new TreeNode();

            m_tvItemPrnt.Value = "Sent Orders";
            m_tvItemPrnt.Text = "Sent Orders";

            this.tvSentOrders.Nodes.Add(m_tvItemPrnt);
            m_tvItemPrnt.SelectAction = TreeNodeSelectAction.None;

            TreeNode m_tvItem = new TreeNode();
            m_tvItem.Value = "Office";
            m_tvItem.Text = "Office (" + mDTOOrderCounts.Rows[0]["OfficeSentOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            m_tvItem = new TreeNode();
            m_tvItem.Value = "Sales Rep";
            m_tvItem.Text = "Sales Rep (" + mDTOOrderCounts.Rows[0]["SalesRepSentOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            m_tvItem = new TreeNode();
            m_tvItem.Value = "All";
            m_tvItem.Text = "All (" + mDTOOrderCounts.Rows[0]["AllSentOrders"].ToString() + ")";
            m_tvItemPrnt.ChildNodes.Add(m_tvItem);

            tvSentOrders.ExpandAll();

        }

        /// <summary>
        /// Get Static Table for OrderLine
        /// </summary>
        /// <returns></returns>
        static DataTable GetOrderLineTable()
        {

            DataTable table = new DataTable();
            table.Columns.Add("OrderLineID", typeof(int));
            table.Columns.Add("LineNum", typeof(int));
            table.Columns.Add("ProductID", typeof(long));
            table.Columns.Add("OrderQty", typeof(float));
            table.Columns.Add("DespatchQty", typeof(float));
            table.Columns.Add("UOM", typeof(string));
            table.Columns.Add("OrderPrice", typeof(float));
            table.Columns.Add("DespatchPrice", typeof(float));
            table.Columns.Add("ItemStatus", typeof(string));
            table.Columns.Add("ErrorText", typeof(string));
            table.Columns.Add("ProductDescription", typeof(string));
            table.Columns.Add("ProductCode", typeof(string));
            table.Columns.Add("GTINCode", typeof(string));
            table.Columns.Add("Discount", typeof(float));

            return table;
        }


        /// <summary>
        /// Changing of Quantity on the Product Orderline Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void gvProductPerGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int Qty = int.Parse(((TextBox)(gvProductPerGroup.Rows[RowIndex].FindControl("txtQtyOderLine"))).Text);
            switch (e.CommandName)
            {
                case "Less":
                    if (Qty < 2)
                    {
                        ((TextBox)(gvProductPerGroup.Rows[RowIndex].FindControl("txtQtyOderLine"))).Text = "0";
                    }
                    else
                    {
                        ((TextBox)(gvProductPerGroup.Rows[RowIndex].FindControl("txtQtyOderLine"))).Text = (Qty - 1).ToString();
                    }
                    break;
                case "More":

                    ((TextBox)(gvProductPerGroup.Rows[RowIndex].FindControl("txtQtyOderLine"))).Text = (Qty + 1).ToString();

                    break;
            }

            mpeOrderLine.Show();

        }


        // Additional Criteria for the Pre-sell and Future-Dated Orders
        protected void gvProductPerGroupSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = Convert.ToInt32(e.CommandArgument);

            // Check what view is active. In this case, this search functionality will not be required
            // to be replicated n times. Only one is needed, just get the active view.

            switch (e.CommandName)
            {

                case "Select":
                    {
                        string GTINCode = ((Label)(gvProductPerGroupSearch.Rows[RowIndex].FindControl("lblProductGTINCodeOfficeSentOrders"))).Text;

                        if (this.MultiView1.GetActiveView().ID == "OfficeSentOrders")
                        {
                            this.txtOfficeSentOrdersGTINCodeSearch.Text = GTINCode;
                            Session["OfficeSentGTINCode"] = GTINCode;
                        }
                        else if (this.MultiView1.GetActiveView().ID == "SalesRepSentOrders")
                        {
                            this.txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders.Text = GTINCode;
                            Session["SalesRepSentGTINCode"] = GTINCode;
                        }
                        else if (this.MultiView1.GetActiveView().ID == "AllSentOrders")
                        {
                            this.txtAllSentOrdersGTINCodeSearch.Text = GTINCode;
                            Session["AllSentGTINCode"] = GTINCode;
                        }
                        else if (this.MultiView1.GetActiveView().ID == "OfficeNewOrders")
                        {
                            this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text = GTINCode;
                            this.RerenderGridData(this.gvOfficeNewOrders);
                            Session["OfficeNewGTINCode"] = GTINCode;
                        }
                        else if (this.MultiView1.GetActiveView().ID == "SalesRepViewNO")
                        {
                            this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text = GTINCode;
                            this.RerenderGridData(this.gvSalesRepNewOrders);
                            Session["SalesRepNewGTINCode"] = GTINCode;
                        }
                        else if (this.MultiView1.GetActiveView().ID == "AllNewOrders")
                        {
                            this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text = GTINCode;
                            this.RerenderGridData(this.gvAllNewOrders);
                            Session["AllNewGTINCode"] = GTINCode;
                        }
                        
                        break;
                    }

            }

            // this.RerenderGridData();
        }


        /// <summary>
        /// Filtering of Non Zero Quantitied Product on the OrderLine Pop up
        /// </summary>
        private void FilterProductsForOrderLine()
        {
            SelectionChange = true;

            foreach (GridViewRow row in gvProductPerGroup.Rows)
            {
                if (((TextBox)(row.FindControl("txtQtyOderLine"))).Text.Length > 5)
                {
                    mpeQtyNotValidPopUp.Show();
                    FillProductGroupDropDownList();
                    FillProductsByGroupID(int.Parse(ddlGroupCreateOrder.SelectedValue));
                    mpeOrderLine.Show();
                    break;
                }
                else
                {
                    if (((TextBox)(row.FindControl("txtQtyOderLine"))).Text.Trim() == "")
                    {
                        ((TextBox)(row.FindControl("txtQtyOderLine"))).Text = "0";
                    }

                    if (long.Parse(((TextBox)(row.FindControl("txtQtyOderLine"))).Text) != 0)
                    {
                        int ProductID = 0;

                        int.TryParse(((Label)(row.FindControl("lblProductID"))).Text, out ProductID);

                        if (CheckIfProductIDIsExistingOnOrderLineTable(ProductID, int.Parse(((TextBox)(row.FindControl("txtQtyOderLine"))).Text)))
                        {
                            InsertProductForOrderLine(row);
                        }

                    }
                    else
                    {
                        // Delete the OrderLine item here.
                        DataTable MytempTable = (DataTable)Session[hidOrderLineTempID.Value]; // Current OrderLines
                        var rowsToDelete = new List<DataRow>();
                        int ProductID = 0;
                        int.TryParse(((Label)(row.FindControl("lblProductID"))).Text, out ProductID);

                        foreach (DataRow mrow in MytempTable.Rows)
                        {
                            if ((long.Parse(((TextBox)(row.FindControl("txtQtyOderLine"))).Text) == 0)
                                && (int.Parse(mrow["ProductID"].ToString()) == ProductID))
                            {
                                // Add the rows to be deleted to a separate datatable
                                OrderLineIDList.Add(mrow["OrderLineID"].ToString());
                                rowsToDelete.Add(mrow);
                            }
                        }

                        // Session[hidOrderLineTempID.Value] = MytempTable;
                        // this.gvOrderLineCreateOrder.DataSource = MytempTable;
                        // Delete 

                        rowsToDelete.ForEach(x => MytempTable.Rows.Remove(x));

                        Session[hidOrderLineTempID.Value] = MytempTable;
                        this.gvOrderLineCreateOrder.DataSource = MytempTable;

                    }
                }

            }
        }

        /// <summary>
        /// Managing inserted products on the orderline table (Increase Quantity)
        /// </summary>
        /// <param name="ProductID"></param>
        private bool CheckIfProductIDIsExistingOnOrderLineTable(int ProductID, int OrderQty)
        {

            bool response = true;
            DataTable MytempTable = (DataTable)Session[hidOrderLineTempID.Value]; // Current OrderLines - ACTUAL ORDERLINES
            var currentProductsRows = gvProductPerGroup.Rows; // Current Product Row - PRODUCT LIST

            foreach (DataRow mrow in MytempTable.Rows)
            {
                if (int.Parse(mrow["OrderQty"].ToString()) == 0)
                {
                    // remove 
                    MytempTable.Rows.Remove(mrow);
                }

                if (int.Parse(mrow["ProductID"].ToString()) == ProductID)
                {
                    mrow["OrderQty"] = OrderQty; //int.Parse(mrow["OrderQty"].ToString());//+ OrderQty;                    
                    response = false;
                    break;
                }
                else
                {
                    response = true;
                }
            }

            // Gets the rows - BUG HERE.
            foreach (DataRow mrow in MytempTable.Rows) // FOR EACH ORDERLINES IN AN ORDER
            {
                foreach (GridViewRow _row in currentProductsRows) // FOR EACH PRODUCT IN THE LIST
                {
                    string _OrderListProductCode = ((Label)_row.FindControl("lblProductCode")).Text.Trim();
                    //if (mrow["ProductCode"].ToString() == _row.Cells[2].Text)
                    if (mrow["ProductCode"].ToString() == _OrderListProductCode)
                    {
                        if (((TextBox)_row.FindControl("txtDiscount")).Text.Trim() == "")
                        {
                            mrow["Discount"] = 0.00;
                        }
                        else
                        {
                            mrow["Discount"] = String.Format("{0:0.00}", ((TextBox)_row.FindControl("txtDiscount")).Text);
                        }
                        
                    }
                }
            }
          
            return response;
        }



        /// <summary>
        /// Inserting of Products on the OrderLine Datatable from Gridview
        /// </summary>
        /// <param name="mRow"></param>
        private void InsertProductForOrderLine(GridViewRow mRow)
        {

            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", ((Label)mRow.FindControl("lblUOMID")).Text, 100);

            SelectionChange = true;
            DataTable MyTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];

            long ProductID = int.Parse(((Label)mRow.FindControl("lblProductID")).Text);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "ProductID OK", 100);

            int UOMID = int.Parse(((Label)mRow.FindControl("lblUOMID")).Text);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "UOMID OK", 100);

            float OrderQty = int.Parse(((TextBox)(mRow.FindControl("txtQtyOderLine"))).Text);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "OrderQTY OK", 100);

            string ProductDescription = HttpUtility.HtmlDecode(mRow.Cells[3].Text);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "Product Desc OK", 100);

            string ProductCode = ((Label)(mRow.FindControl("lblProductCode"))).Text;
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "Product Code OK", 100);

            string ProductGTINCode = HttpUtility.HtmlDecode(mRow.Cells[1].Text);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "Product GTIN OK", 100);
            
            float Discount;

            if (((TextBox)(mRow.FindControl("txtDiscount"))).Text.Trim() == "")
            {
                Discount = 0.00f;
            }
            else
            {
                Discount = float.Parse(((TextBox)(mRow.FindControl("txtDiscount"))).Text);
            }

            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "Product Discount OK", 100);
            // Find me here
            // INclude UOM here
            DTOProductUOM pUOM = GlobalVariables.OrderAppLib.CatalogService.ProductUOMListByID(UOMID);
            // GlobalVariables.OrderAppLib.LogService.LogSave("InsertProductForOrderLine()", "Product UOM OK", 100);
            
            MyTempOrderLineTable.Rows.Add(0, 1, ProductID, OrderQty, 1, pUOM.ProductUOM, 1, 1, "NA", "NA", ProductDescription, ProductCode, ProductGTINCode, Discount);

            Session[hidOrderLineTempID.Value] = MyTempOrderLineTable;

        }

        /// <summary>
        /// Save Button on Orderline Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveOrderLine_Click(object sender, EventArgs e)
        {
            SelectionChange = true;
            gvOrderLineCreateOrder.DataSource = null;

            gvOrderLineCreateOrder.DataBind();

            FilterProductsForOrderLine();

            DataTable mDT = (DataTable)Session[hidOrderLineTempID.Value];

            if (mDT != null & mDT.Rows.Count > 0)
            {
                ddlProviderCreateOrder.Enabled = false;
                this.ddlOrderType.Enabled = false;
            }
            else
            {
                ddlProviderCreateOrder.Enabled = true;
                this.ddlOrderType.Enabled = true;
            }

            DataView dv = mDT.DefaultView;
            //dv.Sort = "OrderLineID Asc";
            DataTable sortedDT = dv.ToTable();

            gvOrderLineCreateOrder.DataSource = sortedDT;
            gvOrderLineCreateOrder.DataBind();

        }


        /// <summary>
        /// Populate Sales Rep Dropdownlist
        /// </summary>
        private void PopulateSalesRepDropDownList(long AccountTypeID)
        {
            SelectionChange = true;
            var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListByID(AccountTypeID);

            DTOAccountList mList = new DTOAccountList();

            mList.Add(respSalesRep);

            ddlSalesRepCreateOrder.DataTextField = "Fullname";
            ddlSalesRepCreateOrder.DataValueField = "AccountID";
            ddlSalesRepCreateOrder.DataSource = mList;
            ddlSalesRepCreateOrder.DataBind();

            ddlSalesRepViewOrders.DataTextField = "Fullname";
            ddlSalesRepViewOrders.DataValueField = "AccountID";
            ddlSalesRepViewOrders.DataSource = mList;
            ddlSalesRepViewOrders.DataBind();
        }

        private void CheckCustomerCodeIfExist(object sender)
        {
            TextBox Txt = (TextBox)sender;
        }

        /// <summary>
        /// Customer or Store ID input text Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCustomerCreateOrder_TextChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            if (txtCustomerCreateOrder.Text != "")
            {
                CheckStoreIDIfExisting(txtCustomerCreateOrder.Text);
                DTOCustomer mcust = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), txtCustomerCreateOrder.Text);

                if (mcust != null)
                    txtCustomerIDCreateOrder.Text = "" + mcust.CustomerID;
            }
            else
            {
                txtStoreNameCreateOrder.Text = "";
                txtStateNameCreateOrder.Text = "";
            }

        }

        /// <summary>
        /// Check Function for Store Code if its existing on the database
        /// </summary>
        /// <param name="StoreCode"></param>
        private void CheckStoreIDIfExisting(string StoreCode)
        {
            SelectionChange = true;
            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), txtCustomerCreateOrder.Text);
            if (respCustomer != null && respCustomer.CustomerCode != null)
            {
                txtCustomerCreateOrder.Style.Add("border-color", "Gray");
                txtStoreNameCreateOrder.Text = respCustomer.CustomerName.ToString();
                txtStateNameCreateOrder.Text = respCustomer.StateName.ToString();
                txtStateCreateOrder.Text = respCustomer.SYSStateID.ToString();
                txtCustomerIDCreateOrder.Text = respCustomer.CustomerID.ToString();

                SelectProviderWareHouse(respCustomer.SYSStateID.ToString(), ddlProviderCreateOrder.SelectedValue);
            }
            else
            {
                txtCustomerCreateOrder.Style.Add("border-color", "Red");
                txtStoreNameCreateOrder.Text = "";
                txtStateNameCreateOrder.Text = "";
                txtStateCreateOrder.Text = "";
                txtCustomerIDCreateOrder.Text = "";
            }

        }

        /// <summary>
        /// Save/Create Order Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveCreateOrder_Click(object sender, EventArgs e)
        {
            SelectionChange = true;

            lblOrderErrorMessage.Text = "";

            DataTable myTempOrderLinetable = (DataTable)Session[hidOrderLineTempID.Value];            

            if (myTempOrderLinetable.Rows.Count != 0)
            {

                GlobalVariables.OrderAppLib.LogService.LogSave("SaveCreateOrder", "hidOrderID :" + hidOrderID.Value.ToString(), 100);

                if (hidOrderID.Value == "")
                {
                    if (txtCustomerCreateOrder.Style["border-color"].ToString() == "Gray")
                    {
                        // Get Max Order Number - Ringo Ray Piedraverde 9-4-2014

                        long OrderNumber = GlobalVariables.OrderAppLib.CatalogService.GetMaxProductNumberbySalesOrg(int.Parse(Session["RefID"].ToString()));

                        // Increment Ordernumber by one
                        if (OrderNumber == 0)
                            OrderNumber = 0;

                        for (int i = 0; i <= 8; i++)   // Check for OrderNumber by SalesOrg if already exist - Ringo Ray Piedraverde 9-4-2014 
                        {
                            OrderNumber = OrderNumber + 1;
                            Boolean result = GlobalVariables.OrderAppLib.CatalogService.CheckProductNumberIfExistBySalesOrg(int.Parse(Session["RefID"].ToString()), OrderNumber);
                            if (result == true)
                            {
                                if (i == 8)
                                {
                                    lblOrderErrorMessage.Text = "Order cannot be saved. Please try again.";
                                    return;
                                }

                            }
                            else
                            {

                                break;

                            }


                        }

                        hidOrderNumber.Value = OrderNumber.ToString();

                        if (CreateOrderRecord())
                        {
                            FillNewOrderTreeViews();
                            FillSentOrderTreeViews();
                            PopulateOfficeNewOrders();

                            // If Session is not empty, go back to previous page
                            if (Session["CustomerPageStateName"] != null)
                            {
                                Session["CustomerPageStateName"] = null;
                                Response.Redirect("~/Organizations/ViewOrganizations.aspx");                                
                            }
                            else
                            {                                
                                MultiView1.SetActiveView(OfficeNewOrders);
                            }                            
                        }
                        else
                        {
                            lblOrderErrorMessage.Text = "Order cannot be saved. Please try again.";
                        }
                    }
                }
                else
                {
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(int.Parse(hidOrderID.Value.ToString()));                    

                    DeleteOrderLines();

                    string _outProductDescriptionL = "" ;
                    string _outProductDescriptionH = "" ;

                    bool _IsValidReleaseDate = true;

                    DateTime _lowestEndDate = this.LowestProductEndDate(respOrderDetails, out _outProductDescriptionL).Date;
                    DateTime _highestStartDate = this.HighestProductStartDate(respOrderDetails, out _outProductDescriptionH).Date;

                    // IF IS REGULAR
                    if (this.txtCreateOrderReleaseDate.Text.Trim() != "")
                    {
                        if (_highestStartDate > DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null) && _highestStartDate != DateTime.MinValue)
                        {
                            string errorMessage = "alert('Error: Release date cannot be less than " + string.Format("{0:dd/MM/yyyy}", _highestStartDate) + " (" + _outProductDescriptionH + ").');";
                            _IsValidReleaseDate = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Product Start Date", errorMessage, true);
                        }

                        if (_lowestEndDate < DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null) && _lowestEndDate != DateTime.ParseExact("09/09/9999","dd/MM/yyyy",null))
                        {
                            string errorMessage = "alert('Error: Release date cannot be greater than " + string.Format("{0:dd/MM/yyyy}", _lowestEndDate) + " (" + _outProductDescriptionL + ").');";
                            _IsValidReleaseDate = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Product End Date", errorMessage, true);
                        }
                    }
                    // IF PRESELL
                    else
                    {
                        _IsValidReleaseDate = true;
                    }

                    if(_IsValidReleaseDate){
                        if (UpdateOrderRecord(respOrderDetails, true, int.Parse(respOrderDetails.SYSOrderStatusID.ToString())))
                        {
                            if (respOrderDetails.SYSOrderStatusID.ToString() == "100")
                            {
                                FillNewOrderTreeViews();
                                FillSentOrderTreeViews();
                                PopulateSalesRepNewOrders();
                                MultiView1.SetActiveView(SalesRepViewNO);
                            }
                            else
                            {
                                FillNewOrderTreeViews();
                                FillSentOrderTreeViews();
                                PopulateOfficeNewOrders();
                                MultiView1.SetActiveView(OfficeNewOrders);
                            }
                        }
                    }

                    txtCustomerIDCreateOrder.Text = "";
                    txtOrderID.Text = "";
                    Session[hidOrderLineTempID.Value] = myTempOrderLinetable;
                }
            }
            else
            {
                mpeAddProductNotification.Show();
            }
        }


        /// <summary>
        /// Delete Order Lines from Database
        /// </summary>
        private void DeleteOrderLines()
        {
            for (int i = 0; i < OrderLineIDList.Count; i++)
            {
                GlobalVariables.OrderAppLib.OrderService.OrderLineDeleteRecord(int.Parse(OrderLineIDList[i].ToString()));
            }
            OrderLineIDList.Clear();
        }

        private DataTable PopulateSalesRepSentOrdersForExport()
        {
            bool? officeOrderType = null;
            if (this.ddlOrderTypesSalesRepSentOrders.SelectedValue != "")
            {
                officeOrderType = bool.Parse(this.ddlOrderTypesSalesRepSentOrders.SelectedValue);
            }

            // Dates
            DateTime _releaseFrom = DateTime.ParseExact("01-01-1900", "dd-MM-yyyy", null);
            if (this.txtReleaseFromSalesRepSentOrders.Text != "")
            {
                _releaseFrom = DateTime.ParseExact(this.txtReleaseFromSalesRepSentOrders.Text, "dd/MM/yyyy", null);
            }

            DateTime _releaseTo = DateTime.ParseExact("01-01-1900", "dd-MM-yyyy", null);
            if (this.txtReleaseToSalesRepSentOrders.Text != "")
            {
                _releaseTo = DateTime.ParseExact(this.txtReleaseToSalesRepSentOrders.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesSalesRepSentOrders.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesSalesRepSentOrders.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtSalesRepSentDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtSalesRepSentDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtSalesRepSentDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtSalesRepSentDateTo.Text, "dd/MM/yyyy", null);
            }

            if (Session["SalesRepSentSelectedCustomerID"] == null || Session["SalesRepSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["SalesRepSentSelectedCustomerID"].ToString();
            }

            if (Session["SalesRepSentCreatedByUserID"] == null || Session["SalesRepSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["SalesRepSentCreatedByUserID"].ToString();
            }

            DataTable respSalesRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(ddlProviderSentOrderSearch.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1,
                1000,
                int.Parse(txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDHidden.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                int.Parse(txtStatusIDHidden.Text),
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders.Text,
                int.Parse(this.ddlStatesSalesRepSentOrders.SelectedValue),
                _IsRegularOrder,
                _releaseFrom,
                _releaseTo,
                "SentSalesRep");

            return respSalesRepSentOrders;

        }

        private DataTable PopulateOfficeSentOrdersForExport()
        {
            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFrom.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtReleaseTo.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseTo.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesOfficeSent.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesOfficeSent.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateTo.Text, "dd/MM/yyyy", null);
            }

            if (Session["OfficeSentSelectedCustomerID"] == null || Session["OfficeSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["OfficeSentSelectedCustomerID"].ToString();
            }

            if (Session["OfficeSentCreatedByUserID"] == null || Session["OfficeSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["OfficeSentCreatedByUserID"].ToString();
            }

            DataTable respOfficeSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(ddlProviderOffice.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1,
                1000,
                int.Parse(txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDHidden.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                int.Parse(txtStatusIDHidden.Text),
                this.txtOfficeSentOrdersGTINCodeSearch.Text,
                int.Parse(this.ddlStates.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate,
                "SentOffice");

            return respOfficeSentOrders;

        }

        private DataTable PopulateAllSentOrdersForExport()
        {

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtAllSentReleaseFrom.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtAllSentReleaseFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtAllSentReleaseTo.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtAllSentReleaseTo.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesAllSent.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesAllSent.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtAllSentDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtAllSentDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtAllSentDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtAllSentDateTo.Text, "dd/MM/yyyy", null);
            }

            if (txtStatusIDHidden.Text == "")
            {
                txtStatusIDHidden.Text = "0";
            }

            if (Session["AllSentSelectedCustomerID"] == null || Session["AllSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["AllSentSelectedCustomerID"].ToString();
            }

            if (Session["AllSentCreatedByUserID"] == null || Session["AllSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["AllSentCreatedByUserID"].ToString();
            }

            DataTable respAllSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(ddlProviderAllSent.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1,
                1000,
                int.Parse(txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDHidden.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                int.Parse(txtStatusIDHidden.Text),
                this.txtAllSentOrdersGTINCodeSearch.Text,
                int.Parse(this.ddlStatesAllSentOrders.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate,
                "SentAll");

            return respAllSentOrders;
        }

        private DataTable PopulateAllNewOrdersForExport()
        {
            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesAllNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesAllNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtReleaseFromAllNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToAllNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromAllNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToAllNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchAllNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchAllNewOrder.Text;

            if (Session["AllNewSelectedCustomerID"] == null || Session["AllNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["AllNewSelectedCustomerID"].ToString();
            }

            if (Session["AllNewCreatedByUserID"] == null || Session["AllNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["AllNewCreatedByUserID"].ToString();
            }

            DataTable respAllNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(this.ddlProviderAllNewOrder.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1,
                1000,
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                0,
                this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text,
                int.Parse(this.ddlStatesAllNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate,
                "NewAll");

            return respAllNewOrders;

        }

        private DataTable PopulateSalesRepNewOrdersForExport()
        {
            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesSalesRepNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesSalesRepNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFromSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchSalesRepNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchSalesRepNewOrder.Text;

            if (Session["SalesRepNewSelectedCustomerID"] == null || Session["SalesRepNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["SalesRepNewSelectedCustomerID"].ToString();
            }

            if (Session["SalesRepNewCreatedByUserID"] == null || Session["SalesRepNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["SalesRepNewCreatedByUserID"].ToString();
            }

            DataTable respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(this.ddlProviderSalesRepNewOrder.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1,
                1000,
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                0,
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text,
                int.Parse(this.ddlStatesSalesRepNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate,
                "NewSalesRep");

            return respSalesRepNewOrders;
        }

        private DataTable PopulateOfficeNewOrdersForExport()
        {

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesOfficeNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesOfficeNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFromOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromOfficeNewOrder.Text, "dd/MM/yyyy", null);

                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToOfficeNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromOfficeNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToOfficeNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchOfficeNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchOfficeNewOrder.Text;

            if (Session["OfficeNewSelectedCustomerID"] == null || Session["OfficeNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["OfficeNewSelectedCustomerID"].ToString();
            }

            if (Session["OfficeNewCreatedByUserID"] == null || Session["OfficeNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["OfficeNewCreatedByUserID"].ToString();
            }

            DataTable respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_ExportOrders(
                    int.Parse(Session["RefID"].ToString()),
                    long.Parse(this.ddlProviderOfficeNewOrder.SelectedValue),
                    int.Parse(Session["AccountID"].ToString()),
                    int.Parse(Session["AccountTypeID"].ToString()),
                    1,
                    1000,
                    long.Parse(this.txtOrderNoHidden.Text),
                    int.Parse(txtCustomerIDSearch.Text),
                    int.Parse(txtCreatedByUserIDHidden.Text),
                    _OrderDateFrom,
                    _OrderDateTo,
                    0,
                    this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text,
                    int.Parse(this.ddlStatesOfficeNewOrder.SelectedValue),
                    _IsRegularOrder,
                    _ReleaseFromDate,
                    _ReleaseToDate,
                    "NewOffice");

            return respOfficeNewOrders;

        }

        /// <summary>
        /// Populate Gridview of Office New Orders
        /// </summary>
        private void PopulateOfficeNewOrders()
        {
            SelectionChange = true;
            int pageIndex = 1;
            int.TryParse(Session["PageIndexOfficeNew"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;

            // Changed for Searching : Pre-sell and Future-Dated Orders

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesOfficeNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesOfficeNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtReleaseFromOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromOfficeNewOrder.Text, "dd/MM/yyyy", null);

                if(_IsRegularOrder == null) _IsRegularOrder = true;
            }
            
            if (this.txtReleaseToOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToOfficeNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromOfficeNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToOfficeNewOrder.Text.Trim() != "" )
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "" ) txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "" ) txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchOfficeNewOrder.Text == "" )
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchOfficeNewOrder.Text;

            if (Session["OfficeNewSelectedCustomerID"] == null || Session["OfficeNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["OfficeNewSelectedCustomerID"].ToString();
            }

            if (Session["OfficeNewCreatedByUserID"] == null || Session["OfficeNewCreatedByUserID"].ToString().Trim() == "" )
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["OfficeNewCreatedByUserID"].ToString();
            }

            if (bool.Parse(Session["IsOfficeNewSearchButtonClicked"].ToString()) != true)
            {

                this.ddlProviderOfficeNewOrder.SelectedValue = ddlProviderOfficeNewOrder.Items[0].Value;
                this.txtOrderNoHidden.Text = "0";
                this.txtCustomerIDSearch.Text = "0";
                this.txtCreatedByUserIDHidden.Text = "0";
                _OrderDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _OrderDateTo = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text = "";
                this.ddlStatesOfficeNewOrder.SelectedValue = "0";
                _IsRegularOrder = null;
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            
            var respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex,
                int.Parse(Session["PageSize"].ToString()),
                long.Parse(this.ddlProviderOfficeNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text,
                int.Parse(this.ddlStatesOfficeNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            this.btnReleaseAllOrdersOfficeNew.Enabled = (respOfficeNewOrders.Count > 0);

            if (respOfficeNewOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respOfficeNewOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblOfficeNewOrdersPages.Text = (respOfficeNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respOfficeNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlOfficeNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlOfficeNewOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblOfficeNewOrdersPages.Text = (respOfficeNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respOfficeNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlOfficeNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlOfficeNewOrdersPages.Items.Add(newCount);
                        }
                    }
                }
                OfficeNewOrdersPagingPanel.Visible = true;
                btnReleaseAllOrders.Visible = true;
                gvOfficeNewOrders.DataSource = respOfficeNewOrders;
                gvOfficeNewOrders.DataBind();
                ManageReleaseButtonOnNewOrders("OfficeNewOrders");
            }
            else
            {
                OfficeNewOrdersPagingPanel.Visible = false;
                gvOfficeNewOrders.DataSource = null;
                gvOfficeNewOrders.DataBind();
                btnReleaseAllOrders.Visible = false;

            }

        }

        public DTOOrderList PopulateAllNewSalesRepOrdersSearch()
        {
            int pageIndex = 1;
            int.TryParse(Session["PageIndexSalesRepNew"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;

            // Changed for Searching : Pre-sell and Future-Dated Orders
            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesSalesRepNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesSalesRepNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFromSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate  = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchSalesRepNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchSalesRepNewOrder.Text;

            if (Session["SalesRepNewSelectedCustomerID"] == null || Session["SalesRepNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["SalesRepNewSelectedCustomerID"].ToString();
            }

            var respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1, // from PageIndex
                1000000,
                int.Parse(Session["OrgUnit"].ToString()),
                long.Parse(this.ddlProviderSalesRepNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text,
                int.Parse(this.ddlStatesSalesRepNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            return respSalesRepNewOrders;


        }

        public DTOOrderList PopulateAllNewOrdersSearch()
        {
            int pageIndex = 1;
            int.TryParse(Session["PageIndexAllOffice"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;


            // Changed for Searching : Pre-sell and Future-Dated Orders
            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesAllNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesAllNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtReleaseFromAllNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToAllNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromAllNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToAllNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchAllNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchAllNewOrder.Text;

            if (Session["AllNewSelectedCustomerID"] == null || Session["AllNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["AllNewSelectedCustomerID"].ToString();
            }

            if (Session["AllNewCreatedByUserID"] == null || Session["AllNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["AllNewCreatedByUserID"].ToString();
            }

            var respAllNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex,
                int.Parse(Session["PageSize"].ToString()),
                long.Parse(this.ddlProviderAllNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text,
                int.Parse(this.ddlStatesAllNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            return respAllNewOrders;
        }

        public DTOOrderList PopulateAllNewOfficeOrdersSearch()
        {
            SelectionChange = true;
            int pageIndex = 1;
            int.TryParse(Session["PageIndexOfficeNew"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;

            // Changed for Searching : Pre-sell and Future-Dated Orders

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesOfficeNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesOfficeNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFromOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromOfficeNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToOfficeNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToOfficeNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromOfficeNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToOfficeNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToOfficeNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchOfficeNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchOfficeNewOrder.Text;


            if (Session["OfficeNewSelectedCustomerID"] == null || Session["OfficeNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["OfficeNewSelectedCustomerID"].ToString();
            }

            if (Session["OfficeNewCreatedByUserID"] == null || Session["OfficeNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["OfficeNewCreatedByUserID"].ToString();
            }

            var respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                1, // PageIndex
                1000000,
                long.Parse(this.ddlProviderOfficeNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text,
                int.Parse(this.ddlStatesOfficeNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            return respOfficeNewOrders;
        }

        /// <summary>
        /// Populate Gridview of Sales Rep New Orders
        /// </summary>
        private void PopulateSalesRepNewOrders()
        {
            int pageIndex = 1;
            int.TryParse(Session["PageIndexSalesRepNew"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;
           
            // Changed for Searching : Pre-sell and Future-Dated Orders
            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesSalesRepNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesSalesRepNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFromSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToSalesRepNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate  = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToSalesRepNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToSalesRepNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchSalesRepNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchSalesRepNewOrder.Text;

            if (Session["SalesRepNewSelectedCustomerID"] == null || Session["SalesRepNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["SalesRepNewSelectedCustomerID"].ToString();
            }

            if (Session["SalesRepNewCreatedByUserID"] == null || Session["SalesRepNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["SalesRepNewCreatedByUserID"].ToString();
            }

            if (bool.Parse(Session["IsSalesRepNewSearchButtonClicked"].ToString()) != true) // If New Office Search is not clicked.
            {
                this.ddlProviderSalesRepNewOrder.SelectedValue = this.ddlProviderSalesRepNewOrder.Items[0].Value;
                this.txtOrderNoHidden.Text = "0";
                this.txtCustomerIDSearch.Text = "0";
                this.txtCreatedByUserIDHidden.Text = "0";
                _OrderDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _OrderDateTo = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text = "";
                this.ddlStatesSalesRepNewOrder.SelectedValue = "0";
                _IsRegularOrder = null;
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }


            var respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex,
                int.Parse(Session["PageSize"].ToString()),
                int.Parse(Session["OrgUnit"].ToString()),
                long.Parse(this.ddlProviderSalesRepNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text,
                int.Parse(this.ddlStatesSalesRepNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            this.btnReleaseAllOrders.Enabled = this.GetReceivedAndCreatedOrders(respSalesRepNewOrders).Count > 0;

            if (respSalesRepNewOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respSalesRepNewOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblSalesRepNewOrdersPages.Text = (respSalesRepNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();                        
                        int count = respSalesRepNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlSalesRepNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlSalesRepNewOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblSalesRepNewOrdersPages.Text = (respSalesRepNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respSalesRepNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlSalesRepNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlSalesRepNewOrdersPages.Items.Add(newCount);
                        }
                    }
                }

                SalesRepNewOrdersPanel.Visible = true;
                gvSalesRepNewOrders.DataSource = respSalesRepNewOrders;
                gvSalesRepNewOrders.DataBind();
                btnReleaseAllOrders.Visible = true;
                ManageReleaseButtonOnNewOrders("SalesRepNewOrders");
            }
            else
            {
                SalesRepNewOrdersPanel.Visible = false;
                gvSalesRepNewOrders.DataSource = null;
                gvSalesRepNewOrders.DataBind();

                btnReleaseAllOrders.Visible = false;
            }

        }

        /// <summary>
        /// Populate Gridview of All New Orders
        /// </summary>
        private void PopulateAllNewOrders()
        {
            int pageIndex = 1;
            int.TryParse(Session["PageIndexAllOffice"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;

            // Changed for Searching : Pre-sell and Future-Dated Orders

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesAllNewOrder.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesAllNewOrder.SelectedValue);
            }

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtReleaseFromAllNewOrder.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFromAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            if (this.txtReleaseToAllNewOrder.Text.Trim() == "")
            {
                _ReleaseToDate  = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseToAllNewOrder.Text, "dd/MM/yyyy", null);
                if (_IsRegularOrder == null) _IsRegularOrder = true;
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFromAllNewOrder.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFromAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateToAllNewOrder.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateToAllNewOrder.Text, "dd/MM/yyyy", null);
            }

            if (txtCustomerIDSearch.Text == "") txtCustomerIDSearch.Text = "0";

            if (txtCreatedByUserIDHidden.Text == "") txtCreatedByUserIDHidden.Text = "0";

            if (txtOrderNoSearchAllNewOrder.Text == "")
                txtOrderNoHidden.Text = "0";
            else
                txtOrderNoHidden.Text = txtOrderNoSearchAllNewOrder.Text;

            if (Session["AllNewSelectedCustomerID"] == null || Session["AllNewSelectedCustomerID"].ToString().Trim() == "")
            {
                txtCustomerIDSearch.Text = "0";
            }
            else
            {
                txtCustomerIDSearch.Text = Session["AllNewSelectedCustomerID"].ToString();
            }

            if (Session["AllNewCreatedByUserID"] == null || Session["AllNewCreatedByUserID"].ToString().Trim() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["AllNewCreatedByUserID"].ToString();
            }


            if (bool.Parse(Session["IsAllNewSearchButtonClicked"].ToString()) != true) // If New Office Search is not clicked.
            {
                this.ddlProviderAllNewOrder.SelectedValue = this.ddlProviderAllNewOrder.Items[0].Value;
                this.txtOrderNoHidden.Text = "0";
                this.txtCustomerIDSearch.Text = "0";
                this.txtCreatedByUserIDHidden.Text = "0";
                _OrderDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _OrderDateTo = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text = "";
                this.ddlStatesAllNewOrder.SelectedValue = "0";
                _IsRegularOrder = null;
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }

            var respAllNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllNewOrders(
                int.Parse(Session["RefID"].ToString()),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex,
                int.Parse(Session["PageSize"].ToString()),
                long.Parse(this.ddlProviderAllNewOrder.SelectedValue),
                long.Parse(this.txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDSearch.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text,
                int.Parse(this.ddlStatesAllNewOrder.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            if (respAllNewOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respAllNewOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblAllNewOrdersPages.Text = (respAllNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respAllNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlAllNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlAllNewOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblAllNewOrdersPages.Text = (respAllNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respAllNewOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlAllNewOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlAllNewOrdersPages.Items.Add(newCount);
                        }
                    }
                }
                AllNewOrdersPanel.Visible = true;
                gvAllNewOrders.DataSource = respAllNewOrders;
                gvAllNewOrders.DataBind();
                ManageReleaseButtonOnNewOrders("AllNewOrders");
            }
            else
            {
                AllNewOrdersPanel.Visible = false;
                gvAllNewOrders.DataSource = null;
                gvAllNewOrders.DataBind();
            }
        }


        /// <summary>
        /// Populate Gridview of Office Sent Orders
        /// </summary>
        private void PopulateOfficeSentOrders()
        {
            int pageIndex = 1;
            int.TryParse(Session["PageIndexOfficeSent"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;


            // Changed for Searching : Pre-sell and Future-Dated Orders

            DateTime _ReleaseFromDate;
            DateTime _ReleaseToDate;

            if (this.txtReleaseFrom.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtReleaseFrom.Text, "dd/MM/yyyy", null);
            }
            
            if (this.txtReleaseTo.Text.Trim() == "")
            {
                _ReleaseToDate  = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtReleaseTo.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesOfficeSent.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesOfficeSent.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtDateTo.Text, "dd/MM/yyyy", null);
            }

            if (Session["OfficeSentSelectedCustomerID"] == null || Session["OfficeSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["OfficeSentSelectedCustomerID"].ToString();
            }

            if (Session["OfficeSentCreatedByUserID"] == null || Session["OfficeSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["OfficeSentCreatedByUserID"].ToString();
            }

            // StatusID
            txtStatusIDHidden.Text = ddlStatusSearch.SelectedValue;

            // Order Number
            if (txtOrderNoSearch.Text.Trim() == "")
            {
                txtOrderNoHidden.Text = "0";
            }
            else
            {
                txtOrderNoHidden.Text = txtOrderNoSearch.Text.Trim();
            }



            var respOfficeSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeSentOrders(
                int.Parse(Session["RefID"].ToString()), 
                long.Parse(ddlProviderOffice.SelectedValue), 
                int.Parse(Session["AccountID"].ToString()), 
                int.Parse(Session["AccountTypeID"].ToString()), 
                pageIndex, 
                int.Parse(Session["PageSize"].ToString()), 
                int.Parse(txtOrderNoHidden.Text), 
                int.Parse(txtCustomerIDHidden.Text), 
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo, 
                int.Parse(txtStatusIDHidden.Text), 
                this.txtOfficeSentOrdersGTINCodeSearch.Text,
                int.Parse(this.ddlStates.SelectedValue), 
                _IsRegularOrder, 
                _ReleaseFromDate, 
                _ReleaseToDate);

            if (respOfficeSentOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respOfficeSentOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblOfficeSentOrdersPages.Text = (respOfficeSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respOfficeSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlOfficeSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlOfficeSentOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblOfficeSentOrdersPages.Text = (respOfficeSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respOfficeSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlOfficeSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlOfficeSentOrdersPages.Items.Add(newCount);
                        }
                    }
                }

                OfficeSentOrdersPanel.Visible = true;
                gvOfficeSentOrders.DataSource = respOfficeSentOrders;
                gvOfficeSentOrders.DataBind();
                //ManageReleaseButtonOnNewOrders("AllNewOrders");
            }
            else
            {
                OfficeSentOrdersPanel.Visible = false;
                gvOfficeSentOrders.DataSource = null;
                gvOfficeSentOrders.DataBind();
            }
        }

        /// <summary>
        /// Populate Gridview ofSales Rep Sent Orders
        /// </summary>
        private void PopulateSalesRepSentOrders()
        {            
            int pageIndex = 1;
            long ProviderID = 0;
            
            int.TryParse(Session["PageIndexSalesRepSent"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;
            long.TryParse(ddlProviderSentOrderSearch.SelectedValue.ToString(), out ProviderID);

            // Pre-sell and Future-dated orders changes
            bool? officeOrderType = null;
            if (this.ddlOrderTypesSalesRepSentOrders.SelectedValue != "")
            {
                officeOrderType = bool.Parse(this.ddlOrderTypesSalesRepSentOrders.SelectedValue);
            }

            // Dates
            DateTime _releaseFrom = DateTime.ParseExact("01-01-1900", "dd-MM-yyyy", null);
            if (this.txtReleaseFromSalesRepSentOrders.Text != "")
            {
                _releaseFrom = DateTime.ParseExact(this.txtReleaseFromSalesRepSentOrders.Text, "dd/MM/yyyy", null);
            }

            DateTime _releaseTo = DateTime.ParseExact("01-01-1900", "dd-MM-yyyy", null);
            if (this.txtReleaseToSalesRepSentOrders.Text != "")
            {
                _releaseTo = DateTime.ParseExact(this.txtReleaseToSalesRepSentOrders.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesSalesRepSentOrders.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesSalesRepSentOrders.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtSalesRepSentDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtSalesRepSentDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtSalesRepSentDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtSalesRepSentDateTo.Text, "dd/MM/yyyy", null);
            }

            if (Session["SalesRepSentSelectedCustomerID"] == null || Session["SalesRepSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["SalesRepSentSelectedCustomerID"].ToString();
            }

            if (Session["SalesRepSentCreatedByUserID"] == null || Session["SalesRepSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["SalesRepSentCreatedByUserID"].ToString();
            }

            // StatusID
            this.txtStatusIDHidden.Text = ddlSalesRepSent.SelectedValue;

            // Order No.
            if (txtSalesRepSentOrderNo.Text.Trim() == "")
            {
                txtOrderNoHidden.Text = "0";
            }
            else
            {
                txtOrderNoHidden.Text = txtSalesRepSentOrderNo.Text.Trim();
            }

            var respSalesRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepSentOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(ddlProviderSentOrderSearch.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex,
                int.Parse(Session["PageSize"].ToString()),
                int.Parse(Session["OrgUnit"].ToString()),
                int.Parse(txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDHidden.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                int.Parse(txtStatusIDHidden.Text),
                this.txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders.Text,
                int.Parse(this.ddlStatesSalesRepSentOrders.SelectedValue),
                _IsRegularOrder,
                _releaseFrom,
                _releaseTo);

            if (respSalesRepSentOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respSalesRepSentOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblSalesRepSentOrdersPages.Text = (respSalesRepSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respSalesRepSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlSalesRepSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlSalesRepSentOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblSalesRepSentOrdersPages.Text = (respSalesRepSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respSalesRepSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlSalesRepSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlSalesRepSentOrdersPages.Items.Add(newCount);
                        }
                    }
                }
                SalesRepSentOrdersPanel.Visible = true;
                gvSalesRepSentOrders.DataSource = respSalesRepSentOrders;
                gvSalesRepSentOrders.DataBind();
                //ManageReleaseButtonOnNewOrders("AllNewOrders");
            }
            else
            {
                SalesRepSentOrdersPanel.Visible = false;
                gvSalesRepSentOrders.DataSource = null;
                gvSalesRepSentOrders.DataBind();
            }
        }

        /// <summary>
        /// Populate Gridview of All Sent Orders
        /// </summary>
        private void PopulateAllSentOrders()
        {

            int pageIndex = 1;
            int.TryParse(Session["PageIndexAllSent"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;


            // Changed for Searching : Pre-sell and Future-Dated Orders

            DateTime _ReleaseFromDate;

            DateTime _ReleaseToDate;

            if (this.txtAllSentReleaseFrom.Text.Trim() == "")
            {
                _ReleaseFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseFromDate = DateTime.ParseExact(this.txtAllSentReleaseFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtAllSentReleaseTo.Text.Trim() == "")
            {
                _ReleaseToDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            }
            else
            {
                _ReleaseToDate = DateTime.ParseExact(this.txtAllSentReleaseTo.Text, "dd/MM/yyyy", null);
            }

            bool? _IsRegularOrder = null;

            if (this.ddlOrderTypesAllSent.SelectedValue.Trim() != "")
            {
                _IsRegularOrder = bool.Parse(this.ddlOrderTypesAllSent.SelectedValue);
            }

            DateTime _OrderDateFrom = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            DateTime _OrderDateTo = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null);

            if (this.txtAllSentDateFrom.Text.Trim() != "")
            {
                _OrderDateFrom = DateTime.ParseExact(this.txtAllSentDateFrom.Text, "dd/MM/yyyy", null);
            }

            if (this.txtAllSentDateTo.Text.Trim() != "")
            {
                _OrderDateTo = DateTime.ParseExact(this.txtAllSentDateTo.Text, "dd/MM/yyyy", null);
            }

            if (txtStatusIDHidden.Text == "")
            {
                txtStatusIDHidden.Text = "0";
            }

            if (Session["AllSentSelectedCustomerID"] == null || Session["AllSentSelectedCustomerID"].ToString() == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = Session["AllSentSelectedCustomerID"].ToString();
            }

            if (Session["AllSentCreatedByUserID"] == null || Session["AllSentCreatedByUserID"].ToString() == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {
                txtCreatedByUserIDHidden.Text = Session["AllSentCreatedByUserID"].ToString();
            }

            var respAllSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllSentOrders(
                int.Parse(Session["RefID"].ToString()),
                long.Parse(ddlProviderAllSent.SelectedValue),
                int.Parse(Session["AccountID"].ToString()),
                int.Parse(Session["AccountTypeID"].ToString()),
                pageIndex, int.Parse(Session["PageSize"].ToString()),
                int.Parse(Session["OrgUnit"].ToString()),
                int.Parse(txtOrderNoHidden.Text),
                int.Parse(txtCustomerIDHidden.Text),
                int.Parse(txtCreatedByUserIDHidden.Text),
                _OrderDateFrom,
                _OrderDateTo,
                int.Parse(txtStatusIDHidden.Text),
                this.txtAllSentOrdersGTINCodeSearch.Text,
                int.Parse(this.ddlStatesAllSentOrders.SelectedValue),
                _IsRegularOrder,
                _ReleaseFromDate,
                _ReleaseToDate);

            if (respAllSentOrders.Count != 0)
            {
                if (pageIndex == 1)
                {
                    if (respAllSentOrders.TotalRows % int.Parse(Session["PageSize"].ToString()) != 0)
                    {
                        lblAllSentOrdersPages.Text = (respAllSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                        int count = respAllSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString()) + 1;
                        ddlAllSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlAllSentOrdersPages.Items.Add(newCount);
                        }
                    }
                    else
                    {
                        lblAllSentOrdersPages.Text = (respAllSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString())).ToString();
                        int count = respAllSentOrders.TotalRows / int.Parse(Session["PageSize"].ToString());
                        ddlAllSentOrdersPages.Items.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var newCount = new ListItem((i + 1).ToString());
                            ddlAllSentOrdersPages.Items.Add(newCount);
                        }
                    }
                }
                AllSentOrdersPanel.Visible = true;
                gvAllSentOrders.DataSource = respAllSentOrders;
                gvAllSentOrders.DataBind();

                //ManageReleaseButtonOnNewOrders("AllNewOrders");
            }
            else
            {
                AllSentOrdersPanel.Visible = false;
                gvAllSentOrders.DataSource = null;
                gvAllSentOrders.DataBind();
            }
        }

        /// <summary>
        /// Show/Unshow Image button of releasing based on order status.
        /// </summary>
        private void ManageReleaseButtonOnNewOrders(string GridView)
        {
            switch (GridView)
            {
                case "OfficeNewOrders":
                    foreach (GridViewRow mRow in gvOfficeNewOrders.Rows)
                    {
                        if (mRow.Cells[7].Text == "Released")
                        {
                            gvOfficeNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnRelease").Visible = false;
                            gvOfficeNewOrders.Rows[mRow.RowIndex].FindControl("imgbtnDeleteOrder").Visible = false;
                        }

                    }
                    break;

                case "SalesRepNewOrders":
                    foreach (GridViewRow mRow in gvSalesRepNewOrders.Rows)
                    {
                        if (mRow.Cells[7].Text == "Released")
                        {
                            gvSalesRepNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnReleaseSalesRepNew").Visible = false;
                            gvSalesRepNewOrders.Rows[mRow.RowIndex].FindControl("imgbtnDeleteOrder").Visible = false;
                        }

                    }
                    break;

                case "AllNewOrders":
                    foreach (GridViewRow mRow in gvAllNewOrders.Rows)
                    {
                        if (mRow.Cells[7].Text == "Released")
                        {
                            gvAllNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnReleaseAllOrders").Visible = false;
                        }

                    }
                    break;

            }
        }

        /// <summary>
        /// Create Order Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            hidOrderID.Value = "";
            SelectionChange = true;
            ddlProviderCreateOrder.Enabled = true;
            txtCustomerCreateOrder.Style.Add("border-color", "Gray");
            string str = txtCustomerCreateOrder.Style["border-color"].ToString();
            ClearCreateOrderFields();
            FillProviderDropDownList();

            txtOrderDateCreateOrder.Text = txtOrderDateCreateOrder.Text = DateTime.Now.ToString("dd/MM/yyyy");
            FillProviderWarehouseDropDownList(int.Parse(ddlProviderCreateOrder.SelectedValue));

            this.ddlOrderType.Enabled = true;
            this.ddlOrderType.SelectedIndex = 0; // Regular
            this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now);// DateTime.ParseExact(DateTime.Now.ToShortDateString(), "dd/MM/yyyy", null).ToShortDateString();
            this.txtCreateOrderReleaseDate.Enabled = true;

            this.txtPONoCreateOrder.Text = "";

            try
            {
               // DTOAccountList mDTO = GlobalVariables.OrderAppLib.AccountService.AccountListByAccountTypeID(3);


                var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListByID(int.Parse(Session["AccountID"].ToString()));

                DTOAccountList mList = new DTOAccountList();

                mList.Add(respSalesRep);

                ddlSalesRepCreateOrder.DataTextField = "FullName";
                ddlSalesRepCreateOrder.DataValueField = "AccountID";
                ddlSalesRepCreateOrder.DataSource = mList;
                ddlSalesRepCreateOrder.DataBind();

              //  ddlSalesRepCreateOrder.SelectedValue = Session["AccountID"].ToString();
            }
            catch
            {

            }
            MultiView1.SetActiveView(CreateOrder);

        }


        /// <summary>
        /// Clear Create Order Fields function
        /// </summary>
        private void ClearCreateOrderFields()
        {
            try
            {   //OrderLineDataTable.Clear();
                if (hidOrderLineTempID.Value != "")
                {
                    Session.Remove(hidOrderLineTempID.Value);
                }

                hidOrderLineTempID.Value = Guid.NewGuid().ToString();
                Session[hidOrderLineTempID.Value] = GetOrderLineTable();

                txtHoldUntilDate.Text = "";
                txtOrderNoCreateOrder.Text = "";
                txtCustomerCreateOrder.Text = "";
                txtStoreNameCreateOrder.Text = "";
                txtStateNameCreateOrder.Text = "";
                txtStateCreateOrder.Text = "";
                txtDeliveryDateCreateOrder.Text = "";
                txtOrderID.Text = "";

                gvOrderLineCreateOrder.DataSource = null;
                gvOrderLineCreateOrder.DataBind();

                gvSalesRepNewOrders.DataBind();
                gvOfficeNewOrders.DataBind();

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Tree View New Orders Selected Node Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvNewOrders_SelectedNodeChanged(object sender, EventArgs e)
        {

            // Clear Hidden IDs - COMMENTED FOR THE MEANTIME

            //txtOrderNoHidden.Text = "0";
            //txtCreatedByUserIDHidden.Text = "0";
            //txtCustomerIDHidden.Text = "0";
            //txtStatusIDHidden.Text = "0";

            SelectionChange = true;
            string NodeName = tvNewOrders.SelectedNode.Value;
            Session["NodeReturn"] = NodeName + "NewOrders";
            SetViewBasedOnSelectedNodeNewOrders(NodeName);
            btnbackSentOrder.Visible = false;
            btnCancel.Visible = true;
            btnSaveCreateOrder.Visible = true;

            btnBackOfficeOrder.Visible = true;
            //   tvNewOrders.SelectedNode.Selected = false;

        }

        /// <summary>
        /// Tree View Sent Orders Selected Node Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvSentOrders_SelectedNodeChanged(object sender, EventArgs e)
        {

            SelectionChange = true;

            // Clear Hidden IDs - COMMENTED FOR THE MEANTIME
            //txtDateFromHidden.Text = DateTime.Now.ToString();
            //txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";

            string NodeName = tvSentOrders.SelectedNode.Value;
            Session["NodeReturn"] = NodeName + "SentOrders";

            SetViewBasedOnSelectedNodeSentOrders(NodeName);
            btnbackSentOrder.Visible = true;
            btnCancel.Visible = false;
            btnSaveCreateOrder.Visible = false;

            btnBackOfficeOrder.Visible = false;
            // tvSentOrders.SelectedNode.Selected = false;

        }

        /// <summary>
        /// Changing of Views based on Selected Node for New Orders
        /// </summary>
        private void SetViewBasedOnSelectedNodeNewOrders(string NodeName)
        {
            
            OrderDetailsEnable(true);
            SelectionChange = true;
            hidIsViewDetails.Value = "false";
            btnBackOfficeOrder.Visible = true;
            btnbackSentOrder.Visible = false;
            switch (NodeName)
            {
                case "Office":
                    PopulateOfficeNewOrders();
                    MultiView1.SetActiveView(OfficeNewOrders);
                    break;
                case "Sales Rep":
                    PopulateSalesRepNewOrders();
                    MultiView1.SetActiveView(SalesRepViewNO);
                    break;
                case "All":
                    PopulateAllNewOrders();
                    MultiView1.SetActiveView(AllNewOrders);
                    break;
            }

        }

        /// <summary>
        /// Changing of Views based on Selected Node for Sent Orders
        /// </summary>
        private void SetViewBasedOnSelectedNodeSentOrders(string NodeName)
        {
            OrderDetailsEnable(false);
            hidIsViewDetails.Value = "false";

            SelectionChange = true;
            switch (NodeName)
            {
                case "Office":
                    PopulateOfficeSentOrders();
                    MultiView1.SetActiveView(OfficeSentOrders);
                    break;
                case "Sales Rep":
                    PopulateSalesRepSentOrders();
                    MultiView1.SetActiveView(SalesRepSentOrders);
                    break;
                case "All":
                    PopulateAllSentOrders();
                    MultiView1.SetActiveView(AllSentOrders);
                    break;
            }

        }


        private void OrderDetailsEnable(Boolean isEnable)
        {


            txtCustomerCreateOrder.Enabled = isEnable;

            ddlProviderCreateOrder.Enabled = isEnable;

            imgBtnSelectCustomer.Enabled = isEnable;

            ddlProviderWarehouseCreateOrder.Enabled = isEnable;

            btnAddProductLine.Enabled = isEnable;

            // Removed, during Allergan Project
            //btnAddProductLineByProductCode.Enabled = isEnable;

            btnSaveCreateOrder.Enabled = isEnable;

            btnCancel.Enabled = isEnable;


            tblOrderLineButtonsOrderDetails.Visible = isEnable;
            gvOrderLineCreateOrder.Columns[0].Visible = isEnable;
            gvOrderLineCreateOrder.Columns[6].Visible = isEnable;
        }

        /// <summary>
        /// Row Command Event of Gridview gvSalesRepNewOrders for Releasing/Viewing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSalesRepNewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            ddlProviderCreateOrder.Enabled = false;
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvSalesRepNewOrders.DataKeys[RowIndex].Values[1].ToString();
                    // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID); Reintegration : Comented out 18-09-2014 Ringo Ray Piedraverde
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
                    // DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = ""; //DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    
                    // UpdateOrderRecord(respOrderDetails, false, 102);
                    
                    // Identify if the SalesOrg is PepsiCo and the Provider is a Distributor.

                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateSalesRepNewOrders();
                    break;

                case "View":
                    
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvSalesRepNewOrders.DataKeys[RowIndex].Values[1].ToString();
                    txtPONoCreateOrder.Text = gvSalesRepNewOrders.DataKeys[RowIndex].Values[4].ToString();
                    hidOrderID.Value = OrderID.ToString();
                    string OrderStatus = gvSalesRepNewOrders.Rows[RowIndex].Cells[7].Text; // Order Status (Released, Created, Acknowledge)
                    string OrderType = bool.Parse(gvSalesRepNewOrders.DataKeys[RowIndex].Values[2].ToString()) ? "Regular" : "Pre-sell";
                    string PONumber = gvSalesRepNewOrders.Rows[RowIndex].Cells[7].Text;

                    DateTime RequestedReleaseDate = gvSalesRepNewOrders.DataKeys[RowIndex].Values[3] != null ? (DateTime)gvSalesRepNewOrders.DataKeys[RowIndex].Values[3] : DateTime.MinValue;

                    this.lblOrderDetailsOrderType.Text = OrderType;
                    this.ddlOrderType.Enabled = false;

                    if (OrderType == "Regular")
                    {
                        this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", RequestedReleaseDate);
                        this.txtCreateOrderReleaseDate.Enabled = true;
                        this.ddlOrderType.SelectedIndex = 0;
                    }
                    else
                    {
                        this.ddlOrderType.SelectedIndex = 1;
                        this.txtCreateOrderReleaseDate.Enabled = false;
                        this.txtCreateOrderReleaseDate.Text = "";
                    }

                    //

                    //DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID); Reintegration: Comented out - Ringo Ray

                    DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));

                    FillProviderWarehouseDropDownList(int.Parse(respOrderDetailsView.ProviderID.ToString()));
                    txtCustomerIDCreateOrder.Text = respOrderDetailsView.CustomerID.ToString();
                    txtOrderID.Text = OrderID.ToString();

                    if (OrderStatus == "Released")
                    {
                        //PopulateSalesRepDropDownList(4);
                        FillOrderDetails(respOrderDetailsView);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                       // PopulateSalesRepDropDownList(4);
                        FillOrderDetailsForEdit(respOrderDetailsView);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    break;
                case "Nothing":
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);                  
                    txtOrderNoForDeletion.Text = HttpUtility.HtmlDecode(gvSalesRepNewOrders.Rows[RowIndex].Cells[1].Text);//OrderID.ToString();
                    txtOrderID.ToolTip = "SalesRep";
                    txtOrderID.Text = OrderID.ToString();
                    txtOrderNoViewOrders.Text = HttpUtility.HtmlDecode(gvSalesRepNewOrders.Rows[RowIndex].Cells[1].Text);
                    txtCustomerForDeletion.Text = HttpUtility.HtmlDecode(gvSalesRepNewOrders.Rows[RowIndex].Cells[2].Text);
                    lbldeletePopupTitle.Text = "Cancel";
                    lblDeletePopupText.Text = "Are you sure you want to cancel this order?";
                    mpeDeleteOrderPopUp.Show();
                    break;

            }
            FillNewOrderTreeViews();
        }


        /// <summary>
        /// Row Command Event of Gridview gvOfficeNewOrders for Releasing/Viewing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOfficeNewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            ddlProviderCreateOrder.Enabled = false;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;

            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvOfficeNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    //DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    //txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtDeliveryDateCreateOrder.Text = "";
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":

                    OrderID = int.Parse(((Label)(gvOfficeNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvOfficeNewOrders.DataKeys[RowIndex].Values[1].ToString();
                    // txtPONoViewOrders.Text = gvOfficeNewOrders.DataKeys[RowIndex].Values[4].ToString();
                    txtPONoCreateOrder.Text = gvOfficeNewOrders.DataKeys[RowIndex].Values[4].ToString();
                    
                    string OrderStatus = gvOfficeNewOrders.Rows[RowIndex].Cells[7].Text;
                    string OrderType = bool.Parse(gvOfficeNewOrders.DataKeys[RowIndex].Values[2].ToString()) ? "Regular" : "Pre-sell";

                    DateTime RequestedReleaseDate = gvOfficeNewOrders.DataKeys[RowIndex].Values[3] != null ? (DateTime)gvOfficeNewOrders.DataKeys[RowIndex].Values[3] : DateTime.MinValue;

                    this.lblOrderDetailsOrderType.Text = OrderType;
                    this.ddlOrderType.Enabled = false;

                    if (OrderType == "Regular")
                    {
                        this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", RequestedReleaseDate); // DateTime.ParseExact( RequestedReleaseDate.ToShortDateString(), "dd/MM/yyyy", null).ToShortDateString();
                        this.txtCreateOrderReleaseDate.Enabled = true;
                        this.ddlOrderType.SelectedIndex = 0;
                    }
                    else
                    {
                        this.ddlOrderType.SelectedIndex = 1;
                        this.txtCreateOrderReleaseDate.Enabled = false;
                        this.txtCreateOrderReleaseDate.Text = "";
                    }


                    //  DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
                    txtCustomerIDCreateOrder.Text = respOrderDetailsView.CustomerID.ToString();
                  //  PopulateSalesRepDropDownList(2);
                    txtOrderID.Text = OrderID.ToString();
                    hidOrderID.Value = OrderID.ToString();
                    if (OrderStatus == "Released")
                    {
                        FillOrderDetails(respOrderDetailsView);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(respOrderDetailsView);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;

                case "Nothing":
                    OrderID = int.Parse(((Label)(gvOfficeNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoForDeletion.Text = OrderID.ToString();
                    txtOrderID.ToolTip = "Office";
                    txtOrderID.Text = OrderID.ToString();

                    txtCustomerForDeletion.Text = HttpUtility.HtmlDecode(gvOfficeNewOrders.Rows[RowIndex].Cells[2].Text);
                    mpeDeleteOrderPopUp.Show();
                    break;

            }
            FillNewOrderTreeViews();
        }

        /// <summary>
        /// Row Command Event of Gridview gvAllNewOrders for Releasing/Viewing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAllNewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            ddlProviderCreateOrder.Enabled = false;
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvAllNewOrders.DataKeys[RowIndex].Values[1].ToString();
                    // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);

                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));

                    //DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    //txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    // UpdateOrderRecord(respOrderDetails, false, 102);

                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvAllNewOrders.DataKeys[RowIndex].Values[1].ToString();
                    //txtPONoViewOrders.Text = gvAllNewOrders.DataKeys[RowIndex].Values[4].ToString();
                    txtPONoCreateOrder.Text = gvAllNewOrders.DataKeys[RowIndex].Values[4].ToString();
                    
                    DTOOrder respOrderDetails1 = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));

                    string OrderStatus = gvAllNewOrders.Rows[RowIndex].Cells[7].Text;
                    txtOrderID.Text = OrderID.ToString();
                    hidOrderID.Value = OrderID.ToString();

                    //
                    string OrderType = bool.Parse(gvAllNewOrders.DataKeys[RowIndex].Values[2].ToString()) ? "Regular" : "Pre-sell";

                    DateTime RequestedReleaseDate = gvAllNewOrders.DataKeys[RowIndex].Values[3] != null ? (DateTime)gvAllNewOrders.DataKeys[RowIndex].Values[3] : DateTime.MinValue;

                    this.lblOrderDetailsOrderType.Text = OrderType;
                    this.ddlOrderType.Enabled = false;

                    if (OrderType == "Regular")
                    {
                        this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", RequestedReleaseDate);
                        this.txtCreateOrderReleaseDate.Enabled = true;
                        this.ddlOrderType.SelectedIndex = 0;
                    }
                    else
                    {
                        this.ddlOrderType.SelectedIndex = 1;
                        this.txtCreateOrderReleaseDate.Enabled = false;
                        this.txtCreateOrderReleaseDate.Text = "";
                    }

                    //




                 //   PopulateSalesRepDropDownList(0);
                    if (OrderStatus == "Released")
                    {
                        FillOrderDetails(respOrderDetails1);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(respOrderDetails1);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;

                case "Nothing":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderID.ToolTip = "All";
                    txtOrderID.Text = OrderID.ToString();
                    txtOrderNoForDeletion.Text = HttpUtility.HtmlDecode(gvAllNewOrders.Rows[RowIndex].Cells[1].Text);
                    txtCustomerForDeletion.Text = HttpUtility.HtmlDecode(gvAllNewOrders.Rows[RowIndex].Cells[2].Text);
                    lbldeletePopupTitle.Text = "Delete";
                    lblDeletePopupText.Text = "Are you sure you want to delete this order?";
                    mpeDeleteOrderPopUp.Show();
                    break;

            }
            FillNewOrderTreeViews();
        }


        /// <summary>
        /// Row Command Event of Gridview gvOfficeSentOrders for Releasing/Viewing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOfficeSentOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvOfficeSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    UpdateOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":

                    PopulateStateDropdown();
                    OrderID = int.Parse(((Label)(gvOfficeSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvOfficeSentOrders.DataKeys[RowIndex].Values[1].ToString();
                    //txtPONoViewOrders.Text = gvOfficeSentOrders.DataKeys[RowIndex].Values[4].ToString();                    
                    txtPONoCreateOrder.Text = gvOfficeSentOrders.DataKeys[RowIndex].Values[4].ToString();                    

                    DTOOrder respOrderDetails1 = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
                    DateTime ActualReleaseDate = DateTime.Parse(gvOfficeSentOrders.DataKeys[RowIndex].Values[3].ToString());
                    this.ddlOrderType.SelectedValue = gvOfficeSentOrders.DataKeys[RowIndex].Values[2].ToString();
                    this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", ActualReleaseDate);
                    
                    this.ddlOrderType.Enabled = false;
                    this.txtCreateOrderReleaseDate.Enabled = false;

                    string OrderStatus = gvOfficeSentOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();
             
                   // PopulateSalesRepDropDownList(2);
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(respOrderDetails1);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(respOrderDetails1);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;
            }
        }

        /// <summary>
        /// Row Command Event of Gridview gvAllSentOrders for Releasing/Viewing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAllSentOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvAllSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    //DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);

                    txtOrderNoCreateOrder.Text = gvAllSentOrders.DataKeys[RowIndex].Values[1].ToString();
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    UpdateOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvAllSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvAllSentOrders.DataKeys[RowIndex].Values[1].ToString();
                    // txtPONoViewOrders.Text = gvAllSentOrders.DataKeys[RowIndex].Values[4].ToString();
                    txtPONoCreateOrder.Text = gvAllSentOrders.DataKeys[RowIndex].Values[4].ToString();
                    

                    DTOOrder respOrderDetails1 = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
                    string OrderStatus = gvAllSentOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();

                    DateTime ActualReleaseDate = DateTime.Parse(gvAllSentOrders.DataKeys[RowIndex].Values[3].ToString());
                    this.ddlOrderType.SelectedValue = gvAllSentOrders.DataKeys[RowIndex].Values[2].ToString();
                    this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", ActualReleaseDate);
                    

                  //  PopulateSalesRepDropDownList(0);
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(respOrderDetails1);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(respOrderDetails1);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;
            }
        }


        /// <summary>
        /// Populate Fields on the Order View.
        /// </summary>
        /// <param name="OrderID"></param>
        private void FillOrderDetailsForEdit(DTOOrder respOrderDetails)
        {
            hidIsViewDetails.Value = "true";
            OrderLineIDList.Clear();

            // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);


            //DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text) , long.Parse(Session["RefID"].ToString()));
            ClearCreateOrderFields();


            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(respOrderDetails.OrderID);
            DTOSYSState respSYSState = GlobalVariables.OrderAppLib.AddressService.SYSStateListByCustomerID(int.Parse(respOrderDetails.CustomerID.ToString()));
            DTOCustomer respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(respOrderDetails.ProviderID, int.Parse(respOrderDetails.CustomerID.ToString()));

            //OrderLineDataTable.Clear();
            FillProviderWarehouseDropDownList(int.Parse(respOrderDetails.ProviderID.ToString()));
            txtPONoViewOrders.Text = respOrderDetails.PONumber;

            Session[hidOrderLineTempID.Value] = GetOrderLineTable();

            FillOrderLineTableWithViewOrderLineDetails(respOrderLineDetails);
            try
            {
                DTOProvider mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListByID(respOrderDetails.ProviderID);

                DTOProviderList mDTOList = new DTOProviderList();

                mDTOList.Add(mDTO);

                ddlProviderCreateOrder.DataSource = mDTOList;
                ddlProviderCreateOrder.DataBind();
            }
            catch
            {

            }

            txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
            txtOrderNoCreateOrder.Text = respOrderDetails.OrderNumber.ToString();
            DateTime OrderDate = (DateTime)respOrderDetails.OrderDate;
            txtOrderDateCreateOrder.Text = OrderDate.ToString("dd/MM/yyyy");



            if (respCustomer != null)
            {
                txtCustomerCreateOrder.Text = respCustomer.ProviderCustomerCode;
                txtStoreNameCreateOrder.Text = respCustomer.CustomerName;
                txtStoreNameCreateOrder.Text = respCustomer.CustomerName.ToString();
            }

            if (respSYSState != null)
                txtStateNameCreateOrder.Text = respSYSState.StateName;


            if (respOrderDetails.DeliveryDate != null)
            {

                DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                if (DeliverDate < DateTime.Parse("01/01/2000"))
                {
                    txtDeliveryDateCreateOrder.Text = "";
                }
                else
                {
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                }

            }



            if (respOrderDetails.HoldUntilDate != null)
            {

                txtHoldUntilDate.Text = String.Format("{0:dd/MM/yyyy}", respOrderDetails.HoldUntilDate);

            }
            else
            {
                txtHoldUntilDate.Text = "";
            }

            try
            {
                ddlProviderWarehouseCreateOrder.SelectedValue = respOrderDetails.ProviderWarehouseID.ToString();
            }
            catch
            {
            }




            try
            {
                PopulateSalesRepDropDownList(respOrderDetails.CreatedByUserID);
                ddlSalesRepCreateOrder.SelectedValue = respOrderDetails.CreatedByUserID.ToString();
            }
            catch { }


            DataTable mDT = (DataTable)Session[hidOrderLineTempID.Value]; 

            DataView dv = mDT.DefaultView;
            dv.Sort = "OrderLineID asc";
            DataTable sortedDT = dv.ToTable();

            gvOrderLineCreateOrder.DataSource = sortedDT;
            gvOrderLineCreateOrder.DataBind();

        }

        /// <summary>
        /// Populate Fields on the Order View.
        /// </summary>
        /// <param name="OrderID"></param>
        private void FillOrderDetails(DTOOrder respOrderDetails)
        {
            OrderLineIDList.Clear();
            // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
            // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));
            ClearCreateOrderFields();
            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(respOrderDetails.OrderID);
            DTOSYSState respSYSState = GlobalVariables.OrderAppLib.AddressService.SYSStateListByCustomerID(int.Parse(respOrderDetails.CustomerID.ToString()));
            DTOCustomer respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(respOrderDetails.ProviderID, long.Parse(respOrderDetails.CustomerID.ToString()));

            //OrderLineDataTable.Clear();
            Session[hidOrderLineTempID.Value] = GetOrderLineTable();
            FillOrderLineTableWithViewOrderLineDetails(respOrderLineDetails);


            txtCustomerViewOrders.Text = respCustomer.ProviderCustomerCode;
            txtStoreNameViewOrders.Text = respCustomer.CustomerName;
            txtOrderNoViewOrders.Text = respOrderDetails.OrderID.ToString();
            txtPONoViewOrders.Text = respOrderDetails.PONumber;

            try
            {

                DTOProvider mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListByID(respOrderDetails.ProviderID);

                DTOProviderList mDTOList = new DTOProviderList();

                mDTOList.Add(mDTO);

                ddlProviderViewOrders.DataSource = mDTOList;
                ddlProviderViewOrders.DataBind();

             
            }
            catch
            {
            }
            txtStateViewOrders.Text = respSYSState.StateName;
            DateTime OrderDate = (DateTime)respOrderDetails.OrderDate;

            txtOrderDateViewOrders.Text = OrderDate.ToString("dd/MM/yyyy");

            //  DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;



            if (respOrderDetails.ReleaseDate.ToString() != "")
            {

                lblOrderDetailsReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", respOrderDetails.ReleaseDate);

            }
            else
            {
                lblOrderDetailsReleaseDate.Text = "";
            }

            txtDeliveryDateViewOrders.Text = "";
            //if (DeliverDate < DateTime.Parse("01/01/2000"))
            //{
            //    txtDeliveryDateViewOrders.Text = "";

            //}
            //else
            //{
            //    txtDeliveryDateViewOrders.Text = DeliverDate.ToString("dd/MM/yyyy");
            //}

            if (respOrderDetails.HoldUntilDate != null)
            {

                txtHoldUntildateView.Text = String.Format("{0:dd/MM/yyyy}", respOrderDetails.HoldUntilDate);

            }
            else
            {
                txtHoldUntildateView.Text = "";
            }


            if (respOrderDetails.HoldUntilDate != null)
            {

                txtHoldUntildateView.Text = String.Format("{0:dd/MM/yyyy}", respOrderDetails.HoldUntilDate);

            }
            else
            {

                txtHoldUntildateView.Text = "";

            }

            try
            {
                ddlProviderWarehouseViewOrders.SelectedValue = respOrderDetails.ProviderWarehouseID.ToString();
            }
            catch
            {

            }


            try
            {
                PopulateSalesRepDropDownList(respOrderDetails.CreatedByUserID);
                ddlSalesRepViewOrders.SelectedValue = respOrderDetails.CreatedByUserID.ToString();
            }
            catch { }



            DataTable mDT = (DataTable)Session[hidOrderLineTempID.Value];

            DataView dv = mDT.DefaultView;
            dv.Sort = "OrderLineID asc";
            DataTable sortedDT = dv.ToTable();

            gvOrderLineViewOrders.DataSource = sortedDT;
            gvOrderLineViewOrders.DataBind();


        }


        /// <summary>
        /// Populate OrderLineDataTable with OrderLine from ViewedOrders.
        /// </summary>
        /// <param name="OrderLineDetails"></param>
        private void FillOrderLineTableWithViewOrderLineDetails(DTOOrderLineList OrderLineDetails)
        {
            SelectionChange = true;

            DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];

            foreach (DTOOrderLine OrderLine in OrderLineDetails)
            {
                myTempOrderLineTable.Rows.Add(OrderLine.OrderLineID, OrderLine.LineNum, OrderLine.ProductID, OrderLine.OrderQty, OrderLine.DespatchQty, OrderLine.UOM, OrderLine.OrderPrice, OrderLine.DespatchPrice, OrderLine.ItemStatus, OrderLine.ErrorText, OrderLine.ProductName, OrderLine.ProductCode, OrderLine.GTINCode, OrderLine.Discount);
            }

            Session[hidOrderLineTempID.Value] = myTempOrderLineTable;
        }


        /// <summary>
        /// Populate Provider Dropdownlist function.
        /// </summary>
        private void FillProviderDropDownList()
        {
            SelectionChange = true;

            DTOProviderList respProviderWithFilter = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgIDWithFilter(long.Parse(Session["RefID"].ToString()));
          //  DTOProviderList respProvider = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(long.Parse(Session["RefID"].ToString()));
            ddlProviderCreateOrder.DataSource = respProviderWithFilter.OrderBy(x => x.ProviderName).ToList(); 
            ddlProviderCreateOrder.DataBind();
            //ddlProviderViewOrders.DataSource = respProvider.OrderBy(x => x.ProviderName).ToList(); 
            //ddlProviderViewOrders.DataBind();
        }

        private void SelectProviderWareHouse(string StateID, string mProviderID)
        {
            try
            {
                string stateCodeValue = "";
                string ProviderValue = "";
                var respProviderWarehouse = GlobalVariables.OrderAppLib.ProviderService.ProviderWarehouseListByProviderID(int.Parse(mProviderID));

                DTOSYSStateList mState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();
                long mstateID = long.Parse(StateID);

                var mStateCode = from SysState in mState
                                 where SysState.SYSStateID == mstateID
                                 select SysState;

                foreach (var item in mStateCode)
                {
                    stateCodeValue = item.StateCode.ToString();
                }
                var mProvider = from Provider in respProviderWarehouse
                                where Provider.ProviderWarehouseCode == stateCodeValue
                                select Provider;

                foreach (var item in mProvider)
                {
                    ProviderValue = item.ProviderWarehouseID.ToString();
                }

                ddlProviderWarehouseCreateOrder.SelectedValue = ProviderValue.ToString();

            }
            catch
            {


            }
        }


        /// <summary>
        /// Populate Provider Warehouse Dropdownlist based on ProviderID function.
        /// </summary>
        /// <param name="ProviderID"></param>
        private void FillProviderWarehouseDropDownList(int ProviderID)
        {
            SelectionChange = true;

            var respProviderWarehouse = GlobalVariables.OrderAppLib.ProviderService.ProviderWarehouseListByProviderID(ProviderID);


            ddlProviderWarehouseCreateOrder.DataSource = respProviderWarehouse;
            ddlProviderWarehouseCreateOrder.DataBind();


            ddlProviderWarehouseViewOrders.DataSource = respProviderWarehouse;
            ddlProviderWarehouseViewOrders.DataBind();

        }

        /// <summary>
        /// Cancel Order Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SelectionChange = true;
            ClearCreateOrderFields();

            this.PopulateOfficeNewOrders();
            this.PopulateSalesRepNewOrders();
            this.PopulateAllNewOrders();

            // Add PopulateSent?

            switch (Session["NodeReturn"].ToString())
            {
                case "OfficeNewOrders":
                    MultiView1.SetActiveView(OfficeNewOrders);
                    break;
                case "Sales RepNewOrders":
                    MultiView1.SetActiveView(SalesRepViewNO);
                    break;
                case "AllNewOrders":
                    MultiView1.SetActiveView(AllNewOrders);
                    break;
            }

        }

        /// <summary>
        /// Back To List Function for each View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BackToListFunction(object sender, EventArgs e)
        {

            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;

            switch (Session["NodeReturn"].ToString())
            {
                case "OfficeNewOrders":
                    MultiView1.SetActiveView(OfficeNewOrders);
                    break;
                case "Sales RepNewOrders":
                    MultiView1.SetActiveView(SalesRepViewNO);
                    break;
                case "AllNewOrders":
                    MultiView1.SetActiveView(AllNewOrders);
                    break;
                case "OfficeSentOrders":
                    MultiView1.SetActiveView(OfficeSentOrders);
                    break;
                case "Sales RepSentOrders":
                    MultiView1.SetActiveView(SalesRepSentOrders);
                    break;
                case "AllSentOrders":
                    MultiView1.SetActiveView(AllSentOrders);
                    break;
            }


        }


        /// <summary>
        /// Select Customer button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnSelectCustomer_Click(object sender, ImageClickEventArgs e)
        {
            SelectionChange = true;
            Session["SearchType"] = "Create Order";
            ShowCustomerListByProviderWithDateFilter(long.Parse(ddlProviderCreateOrder.SelectedValue.ToString()));
        
        }


        /// <summary>
        /// Customer Concat Search event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            if (Session["NodeReturn"].ToString() == "OfficeNewOrders")
            {
                ShowCustomerListByProviderWithDateFilter(long.Parse(ddlProviderCreateOrder.SelectedValue.ToString()));
            }
            else
            {
                ShowCustomerListByProvider(long.Parse(ddlProviderCreateOrder.SelectedValue.ToString()));
            }

            // place here daw
            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }


        }


        public void ShowCustomerListByProvider(long ProviderID)
        {

            SelectionChange = true;
            DTOCustomerList respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByProviderSearchPage(ProviderID, int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));

            try
            {
                if (respCustomer.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblCustomerPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblCustomerPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerPages.Items.Add(newCount);
                    }
                }
            }

            catch { }

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();

            mpeCompanySearch.Show();

        }

        public void ShowCustomerListByProviderWithDateFilter(long ProviderID)
        {

            SelectionChange = true;
            DTOCustomerList respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByProviderSearchPage_WithDateFilter(ProviderID, int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));

            try
            {
                if (respCustomer.TotalRecords % int.Parse(Session["PageSize"].ToString()) != 0)
                {
                    lblCustomerPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString()) + 1;
                    ddlCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerPages.Items.Add(newCount);
                    }
                }
                else
                {
                    lblCustomerPages.Text = (respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString())).ToString();
                    int count = respCustomer.TotalRecords / int.Parse(Session["PageSize"].ToString());
                    ddlCustomerPages.Items.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        var newCount = new ListItem((i + 1).ToString());
                        ddlCustomerPages.Items.Add(newCount);
                    }
                }
            }

            catch { }

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();

            mpeCompanySearch.Show();

        }



        /// <summary>
        /// Customer Search Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CustomerPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlCustomerPages.SelectedValue);

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
                    if (PageNumber < int.Parse(lblCustomerPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblCustomerPages.Text);
                    break;
            }

            ddlCustomerPages.SelectedValue = PageNumber.ToString();

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByProviderSearchPage(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, int.Parse(ddlCustomerPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();

            mpeCompanySearch.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }

        }


        /// <summary>
        /// Select Index change paging on Customer Search COncat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCustomerPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlCustomerPages.SelectedValue);

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByProviderSearchPage(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, int.Parse(ddlCustomerPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();
            mpeCompanySearch.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }

        }

        /// <summary>
        /// Office New Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OfficeNewOrdersPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlOfficeNewOrdersPages.SelectedValue);
            Session["PageIndexOfficeNew"] = PageNumber;

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
                    if (PageNumber < int.Parse(lblOfficeNewOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblOfficeNewOrdersPages.Text);
                    break;
            }

            ddlOfficeNewOrdersPages.SelectedValue = PageNumber.ToString();
            Session["PageIndexOfficeNew"] = PageNumber;
            this.PopulateOfficeNewOrders();
            ManageReleaseButtonOnNewOrders("OfficeNewOrders");
            

        }

        /// <summary>
        /// Select Index change Dropdownlist event on Office New Orders 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOfficeNewOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlOfficeNewOrdersPages.SelectedValue);
            Session["PageIndexOfficeNew"] = PageNumber;
            this.PopulateOfficeNewOrders();
            ManageReleaseButtonOnNewOrders("OfficeNewOrders");
        }

        /// <summary>
        /// Sales Rep New Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SalesRepNewOrdersPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlSalesRepNewOrdersPages.SelectedValue);
            Session["PageIndexSalesRepNew"] = PageNumber;

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
                    if (PageNumber < int.Parse(lblSalesRepNewOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblSalesRepNewOrdersPages.Text);
                    break;
            }

            Session["PageIndexSalesRepNew"] = PageNumber;
            ddlSalesRepNewOrdersPages.SelectedValue = PageNumber.ToString();
            this.PopulateSalesRepNewOrders();
            ManageReleaseButtonOnNewOrders("SalesRepNewOrders");
        }

        /// <summary>
        /// Select Index change Dropdownlist event on Sales Rep New Orders 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSalesRepNewOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlSalesRepNewOrdersPages.SelectedValue);
            Session["PageIndexSalesRepNew"] = PageNumber;
            this.PopulateSalesRepNewOrders();
            ManageReleaseButtonOnNewOrders("SalesRepNewOrders");
        }


        /// <summary>
        /// All New Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AllNewOrdersPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlAllNewOrdersPages.SelectedValue);
            Session["PageIndexAllOffice"] = PageNumber;

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
                    if (PageNumber < int.Parse(lblAllNewOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblAllNewOrdersPages.Text);
                    break;
            }
            ddlAllNewOrdersPages.SelectedValue = PageNumber.ToString();
            Session["PageIndexAllOffice"] = PageNumber;
            this.PopulateAllNewOrders();
            ManageReleaseButtonOnNewOrders("AllNewOrders");
        }

        /// <summary>
        /// Select Index change Dropdownlist event on All New Orders 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAllNewOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlAllNewOrdersPages.SelectedValue);
            Session["PageIndexAllOffice"] = PageNumber;
            this.PopulateAllNewOrders();    
            ManageReleaseButtonOnNewOrders("AllNewOrders");
        }

        /// <summary>
        /// Office Sent Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OfficeSentOrdersPaging(object sender, EventArgs e)
        {
            int pageIndex = 1; 
            int.TryParse(Session["PageIndexOfficeSent"].ToString(), out pageIndex);

            if (pageIndex == 0)
                pageIndex = 1;

            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlOfficeSentOrdersPages.SelectedValue);

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
                    if (PageNumber < int.Parse(lblOfficeSentOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblOfficeSentOrdersPages.Text);
                    break;
            }

            ddlOfficeSentOrdersPages.SelectedValue = PageNumber.ToString();
            Session["PageIndexOfficeSent"] = PageNumber;
            this.PopulateOfficeSentOrders();

        }

        /// <summary>
        /// Office Sent Orders dropdownlist Select Index Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOfficeSentOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlOfficeSentOrdersPages.SelectedValue);
            Session["PageIndexOfficeSent"] = PageNumber;
            this.PopulateOfficeSentOrders();
        }

        /// <summary>
        /// Sales Rep Sent Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SalesRepSentOrdersPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;
            int PageNumber = int.Parse(ddlSalesRepSentOrdersPages.SelectedValue);

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
                    if (PageNumber < int.Parse(lblSalesRepSentOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblSalesRepSentOrdersPages.Text);
                    break;
            }

            ddlSalesRepSentOrdersPages.SelectedValue = PageNumber.ToString();
            Session["PageIndexSalesRepSent"] = ddlSalesRepSentOrdersPages.SelectedValue;
            this.PopulateSalesRepSentOrders();
        }

        /// <summary>
        /// Sales Rep Sent Orders dropdownlist Select Index Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSalesRepSentOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlSalesRepSentOrdersPages.SelectedValue);
            Session["PageIndexSalesRepSent"] = ddlSalesRepSentOrdersPages.SelectedValue;
            this.PopulateSalesRepSentOrders();

        }

        /// <summary>
        /// All New Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AllSentOrdersPaging(object sender, EventArgs e)
        {
            SelectionChange = true;
            LinkButton lnkButton = sender as LinkButton;

            int PageNumber = int.Parse(ddlAllSentOrdersPages.SelectedValue);
            Session["PageIndexAllSent"] = PageNumber; // Under testing.

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
                    if (PageNumber < int.Parse(lblAllSentOrdersPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(lblAllSentOrdersPages.Text);
                    break;
            }

            ddlAllSentOrdersPages.SelectedValue = PageNumber.ToString();
            Session["PageIndexAllSent"] = PageNumber;
            this.PopulateAllSentOrders();
        }


        /// <summary>
        /// All Sent Orders dropdownlist Select Index Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAllSentOrdersPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectionChange = true;
            int PageNumber = int.Parse(ddlAllSentOrdersPages.SelectedValue);
            Session["PageIndexAllSent"] = PageNumber;
            this.PopulateAllSentOrders();
        }


        /// <summary>
        /// Row command event for gvCustomer Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCustomerSelect_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int CustomerID = 0;
            switch (e.CommandName)
            {
                case "Select":
                    CustomerID = int.Parse(((Label)(gvCustomerSelect.Rows[RowIndex].FindControl("lblCustomerIDSearch"))).Text);
                    if (Session["SearchType"].ToString() == "Create Order")
                    {
                        txtCustomerIDCreateOrder.Text = CustomerID.ToString();
                        txtCustomerCreateOrder.Text = gvCustomerSelect.DataKeys[RowIndex].Value.ToString(); 
                        txtStoreNameCreateOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                        txtStateNameCreateOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[4].Text);

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                        txtCustomerCreateOrder.Style.Add("border-color", "Gray");
                    }
                    else if (Session["SearchType"].ToString() == "Search Order")
                    {
                        if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
                        {
                            this.txtCustomerNameSearchOfficeNewOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            this.RerenderGridData(this.gvOfficeNewOrders);
                            Session["OfficeNewSelectedCustomerID"] = CustomerID.ToString();
                        }
                        else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
                        {
                            this.txtCustomerNameSearchSalesRepNewOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            Session["SalesRepNewSelectedCustomerID"] = CustomerID.ToString();
                            this.RerenderGridData(this.gvSalesRepNewOrders);
                        }
                        else if (MultiView1.GetActiveView().ID == "AllNewOrders")
                        {
                            this.txtCustomerNameSearchAllNewOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            Session["AllNewSelectedCustomerID"] = CustomerID.ToString();
                            this.RerenderGridData(this.gvAllNewOrders);
                        }
                        else if (MultiView1.GetActiveView().ID == "OfficeSentOrders")
                        {
                            this.txtCustomerNameSearch.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            Session["OfficeSentSelectedCustomerID"] = CustomerID.ToString();
                        }
                        else if (MultiView1.GetActiveView().ID == "SaleRepNewOrders")
                        {
                            this.txtSalesRepSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            Session["SalesRepSentSelectedCustomerID"] = CustomerID.ToString();
                        }
                        else if (MultiView1.GetActiveView().ID == "AllSentOrders")
                        {
                            this.txtAllSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                            Session["AllSentSelectedCustomerID"] = CustomerID.ToString();
                        }

                        txtCustomerNameSearch.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();
                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();

                        // txtCustomerNoSearch.Style.Add("border-color", "Gray");
                    }
                    else if (Session["SearchType"].ToString() == "Search SalesRep")
                    {
                        //txtSalesRepSentCustomerNo.Text = gvCustomerSelect.DataKeys[RowIndex].Value.ToString();  // gvCustomerSelect.DataKeys[RowIndex].Value.ToString();
                        txtSalesRepSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();
                        Session["SalesRepSentSelectedCustomerID"] = CustomerID.ToString();

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                        // txtSalesRepSentCustomerNo.Style.Add("border-color", "Gray");
                    }
                    else if (Session["SearchType"].ToString() == "Search All")
                    {
                        //  txtAllSentCustomerNo.Text = gvCustomerSelect.DataKeys[RowIndex].Value.ToString();  // HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[1].Text);
                        txtAllSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();
                        Session["AllSentSelectedCustomerID"] = CustomerID.ToString();
                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                        // txtAllSentCustomerNo.Style.Add("border-color", "Gray");
                    }                    
                    mpeCompanySearch.Hide();

                    // this.RerenderGridData();
                    break;
            }

            SelectProviderWareHouse(ddlStateForCustomerView.SelectedValue, ddlProviderCreateOrder.SelectedValue);
        }



        /// <summary>
        /// Button Search event for Search Office Sent Orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOfficeSentSearch_Click(object sender, EventArgs e)
        {
            Session["PageIndexOfficeSent"] = 1;

            ////OrderNo
            //if (txtOrderNoSearch.Text == "")
            //{
            //    txtOrderNoHidden.Text = "0";
            //}
            //else
            //{
            //    txtOrderNoHidden.Text = txtOrderNoSearch.Text;
            //}
            ////CustomerID  Manage some shit #Do this First
            //if (txtCustomerIDSearch.Text == "")
            //{
            //    txtCustomerIDHidden.Text = "0";
            //}
            //else
            //{
            //    txtCustomerIDHidden.Text = txtCustomerIDSearch.Text;
            //}
            ////CreatedbyUserID
            //if (txtCreatedBySearch.Text == "")
            //{
            //    txtCreatedByUserIDHidden.Text = "0";
            //}
            //else
            //{

            //}
            ////DateFrom
            //if (txtDateFrom.Text == "")
            //{
            //    txtDateFromHidden.Text = DateTime.Now.ToString();
            //}
            //else
            //{
            //    DateTime fromOrderDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", null);

            //    txtDateFromHidden.Text = fromOrderDate.ToString();
            //}
            ////DateTo
            //if (txtDateTo.Text == "")
            //{
            //    txtDateToHidden.Text = DateTime.Now.ToString();
            //}
            //else
            //{
            //    DateTime ToOrderDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", null).AddDays(1);
            //    txtDateToHidden.Text = ToOrderDate.ToString();
            //}
            ////StatusID
            //txtStatusIDHidden.Text = ddlStatusSearch.SelectedValue;

            PopulateOfficeSentOrders();

        }

        protected void btnOfficeNewOrderSearch_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated event
            Session["PageIndexOfficeNew"] = "1";
            Session["SearchButtonClicked"] = "OK";
            Session["IsOfficeNewSearchButtonClicked"] = true;
            this.PopulateOfficeNewOrders();
            this.btnReleaseAllOrdersOfficeNew.Enabled = (this.GetReceivedAndCreatedOrders(this.PopulateAllNewOfficeOrdersSearch()).Count > 0);
        }

        // btnSalesRepNewOrderSearch_Click
        protected void btnSalesRepNewOrderSearch_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated event
            Session["PageIndexSalesRepNew"] = "1";
            Session["SearchButtonClicked"] = "OK";
            Session["IsSalesRepNewSearchButtonClicked"] = true;
            this.PopulateSalesRepNewOrders();
            this.btnReleaseAllOrders.Enabled = (this.GetReceivedAndCreatedOrders(this.PopulateAllNewSalesRepOrdersSearch()).Count > 0);
        }

        // btnAllNewOrderSearch_Click
        protected void btnAllNewOrderSearch_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated event
            Session["PageIndexAllOffice"] = "1";
            Session["IsAllNewSearchButtonClicked"] = true;
            this.PopulateAllNewOrders();
        }

        // btnExportOfficeOrders_Click
        protected void btnExportOfficeOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateOfficeNewOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "Office New Orders", DateTime.Now));
        }

        // btnExportOfficeOrders_Click
        protected void btnExportSalesRepNewOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateSalesRepNewOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "Sales Rep New Orders", DateTime.Now));
        }

        // btnExportOfficeOrders_Click
        protected void btnExportAllNewOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateAllNewOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "All New Orders", DateTime.Now));
        }

        protected void btnExportOfficeSentOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateOfficeSentOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "Office Sent Orders", DateTime.Now));
        }

        protected void btnExportSalesRepSentOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateSalesRepSentOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "Sales Rep Sent Orders", DateTime.Now));
        }

        protected void btnExportAllSentOrders_Click(object sender, EventArgs e)
        {
            DataTable mDT = this.PopulateAllSentOrdersForExport();
            this.ExportToCSVFile(mDT, string.Format("{0} as of {1:ddMMyyyy}", "All Sent Orders", DateTime.Now));
        }


        /// <summary>
        /// Image Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnCustomerSearch_Click(object sender, ImageClickEventArgs e)
        {
            Session["SearchType"] = "Search Order";
            ShowCustomerListByProvider(long.Parse(ddlProviderOffice.SelectedValue));
            mpeCompanySearch.Show();
        }

        // imgbtnCustomerSearchOfficeNewOrder_Click
        protected void imgbtnCustomerSearchOfficeNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated orders event
             Session["SearchType"] = "Search Order";
             ShowCustomerListByProvider(long.Parse(ddlProviderOfficeNewOrder.SelectedValue));
             mpeCompanySearch.Show();
             this.RerenderGridData(this.gvOfficeNewOrders);
        }

        // imgbtnCustomerSearchSalesRepNewOrder_Click
        protected void imgbtnCustomerSearchSalesRepNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated orders event
            Session["SearchType"] = "Search Order";
            ShowCustomerListByProvider(long.Parse(ddlProviderSalesRepNewOrder.SelectedValue));
            mpeCompanySearch.Show();
            this.RerenderGridData(this.gvSalesRepNewOrders);
        }

        // imgbtnCustomerSearchAllNewOrder_Click
        protected void imgbtnCustomerSearchAllNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated orders event
            Session["SearchType"] = "Search Order";
            ShowCustomerListByProvider(long.Parse(ddlProviderAllNewOrder.SelectedValue));
            mpeCompanySearch.Show();
            this.RerenderGridData(gvAllNewOrders);
        }



        /// <summary>
        /// Check Store ID Code if existing for Office Sent Order Search
        /// </summary>
        /// <param name="StoreCode"></param>B
        private void CheckStoreIDSearchIfExistingOffice(string StoreCode, long ProviderID)
        {
            SelectionChange = true;
            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(ProviderID, int.Parse(Session["RefID"].ToString()), StoreCode);
            if (respCustomer != null && respCustomer.CustomerCode != null)
            {
                //  txtCustomerNoSearch.Style.Add("border-color", "Gray"); // remove
                txtCustomerNameSearch.Text = respCustomer.CustomerName.ToString();
                txtCustomerIDSearch.Text = respCustomer.CustomerID.ToString();
            }
            else
            {
                //if (txtCustomerNoSearch.Text != "")// remove
                //{
                //    txtCustomerNoSearch.Style.Add("border-color", "Red");
                //}
                //else
                //{

                //    txtCustomerNoSearch.Style.Add("border-color", "Gray");
                //}
                txtCustomerNameSearch.Text = "";
                txtCustomerIDSearch.Text = "";
            }
        }


        private void CheckStoreIDSearchIfExistingSalesRep(string StoreCode, long ProviderID)
        {
            SelectionChange = true;
            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(ProviderID, int.Parse(Session["RefID"].ToString()), StoreCode);
            if (respCustomer != null && respCustomer.CustomerCode != null)
            {
                //txtSalesRepSentCustomerNo.Style.Add("border-color", "Gray");
                txtSalesRepSentCustomerName.Text = respCustomer.CustomerName.ToString();
                txtCustomerIDSearch.Text = respCustomer.CustomerID.ToString();
            }
            else
            {
                //if (txtSalesRepSentCustomerNo.Text != "")
                //{
                //    txtSalesRepSentCustomerNo.Style.Add("border-color", "Red");
                //}
                //else
                //{

                //    txtSalesRepSentCustomerNo.Style.Add("border-color", "Gray");
                //}
                //txtSalesRepSentCustomerName.Text = "";
                txtCustomerIDSearch.Text = "";
            }
        }

        private void CheckStoreIDSearchIfExistingAll(string StoreCode, long ProviderID)
        {
            SelectionChange = true;
            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(ProviderID, int.Parse(Session["RefID"].ToString()), StoreCode);
            if (respCustomer != null && respCustomer.CustomerCode != null)
            {
                // txtAllSentCustomerNo.Style.Add("border-color", "Gray");
                txtAllSentCustomerName.Text = respCustomer.CustomerName.ToString();
                txtCustomerIDSearch.Text = respCustomer.CustomerID.ToString();
            }
            else
            {
                //if (txtAllSentCustomerNo.Text != "")
                //{
                //    txtAllSentCustomerNo.Style.Add("border-color", "Red");
                //}
                //else
                //{
                //    txtAllSentCustomerNo.Style.Add("border-color", "Gray");
                //}
                txtAllSentCustomerName.Text = "";
                txtCustomerIDSearch.Text = "";
            }
        }


        /// <summary>
        /// Customer No Text Changed Event for Office Order Sent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCustomerNoSearch_TextChanged(object sender, EventArgs e)
        {
            SelectionChange = true;

            //txtCustomerNoSearch.Text


            //  CheckStoreIDSearchIfExistingOffice(txtCustomerNoSearch.Text, long.Parse(ddlProviderOffice.SelectedValue));
        }


        // Search Product - for the additional search criteria: Pre-sell/Future Dated
        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
        }



        /// <summary>
        /// Button Clear Event for Office Sent Orders Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchOfficeSentClear_Click(object sender, EventArgs e)
        {
            Session["PageIndexOfficeSent"] = 1;
            Session["OfficeSentSelectedCustomerID"] = "";
            Session["OfficeSentCreatedByUserID"] = "";
            txtOrderNoSearch.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCreatedBySearch.Text = "";
            txtCustomerNameSearch.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";

            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlStatusSearch.SelectedValue = "0";
            ddlProviderOffice.SelectedIndex = 0; ;

            this.txtReleaseFrom.Text = "";
            this.txtReleaseTo.Text = "";
            this.ddlOrderTypesOfficeSent.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearch.Text = "";
            this.ddlStates.ClearSelection();

            PopulateOfficeSentOrders();
        }

        private void ClearOfficeSentFields()
        {
            txtOrderNoSearch.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCreatedBySearch.Text = "";
            txtCustomerNameSearch.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";

            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlStatusSearch.SelectedValue = "0";
            ddlProviderOffice.SelectedIndex = 0; ;
        }

        protected void btnSearchOfficeNewOrderClear_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-Dated event            
            Session["PageIndexOfficeNew"] = "1";
            Session["OfficeNewSelectedCustomerID"] = "";
            Session["OfficeNewCreatedByUserID"] = "";
            Session["IsOfficeNewSearchButtonClicked"] = false;

            this.txtOrderNoSearchOfficeNewOrder.Text = "";
            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchOfficeNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchOfficeNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromOfficeNewOrder.Text = "";
            this.txtDateToOfficeNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();                     
            this.ddlProviderOfficeNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromOfficeNewOrder.Text = "";
            this.txtReleaseToOfficeNewOrder.Text = "";
            this.txtDateFromOfficeNewOrder.Text = "";
            this.txtDateToOfficeNewOrder.Text = "";
            this.ddlOrderTypesOfficeNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text = "";
            this.ddlStatesOfficeNewOrder.ClearSelection();
            
            PopulateOfficeNewOrders();

        }

        private void ClearOfficeNewFields()
        {
            this.txtOrderNoSearchOfficeNewOrder.Text = "";
            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchOfficeNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchOfficeNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromOfficeNewOrder.Text = "";
            this.txtDateToOfficeNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            this.ddlProviderOfficeNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromOfficeNewOrder.Text = "";
            this.txtReleaseToOfficeNewOrder.Text = "";
            this.txtDateFromOfficeNewOrder.Text = "";
            this.txtDateToOfficeNewOrder.Text = "";
            this.ddlOrderTypesOfficeNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder.Text = "";
            this.ddlStatesOfficeNewOrder.ClearSelection();
        }

        // btnSearchSalesRepNewOrderClear_Click
        protected void btnSearchSalesRepNewOrderClear_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-Dated event
            Session["PageIndexSalesRepNew"] = "1";
            this.txtOrderNoSearchSalesRepNewOrder.Text = "";
            Session["SalesRepNewSelectedCustomerID"] = "";
            Session["SalesRepNewCreatedByUserID"] = "";
            Session["IsSalesRepNewSearchButtonClicked"] = false;

            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchSalesRepNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchSalesRepNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromSalesRepNewOrder.Text = "";
            this.txtDateToSalesRepNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            this.ddlProviderSalesRepNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromSalesRepNewOrder.Text = "";
            this.txtReleaseToSalesRepNewOrder.Text = "";
            this.txtDateFromSalesRepNewOrder.Text = "";
            this.txtDateToSalesRepNewOrder.Text = "";
            this.ddlOrderTypesSalesRepNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text = "";
            this.ddlStatesSalesRepNewOrder.ClearSelection();

            PopulateSalesRepNewOrders();

        }

        private void ClearSalesRepNewFields()
        {
            this.txtOrderNoSearchSalesRepNewOrder.Text = "";
            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchSalesRepNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchSalesRepNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromSalesRepNewOrder.Text = "";
            this.txtDateToSalesRepNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            this.ddlProviderSalesRepNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromSalesRepNewOrder.Text = "";
            this.txtReleaseToSalesRepNewOrder.Text = "";
            this.txtDateFromSalesRepNewOrder.Text = "";
            this.txtDateToSalesRepNewOrder.Text = "";
            this.ddlOrderTypesSalesRepNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder.Text = "";
            this.ddlStatesSalesRepNewOrder.ClearSelection();
        }

        // btnSearchAllNewOrderClear_Click
        protected void btnSearchAllNewOrderClear_Click(object sender, EventArgs e)
        {
            // Pre-sell and Future-Dated event
            Session["PageIndexAllOffice"] = "1";
            Session["AllNewSelectedCustomerID"] = "";
            Session["AllNewCreatedByUserID"] = "";
            Session["IsAllNewSearchButtonClicked"] = false ;

            this.txtOrderNoSearchAllNewOrder.Text = "";
            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchAllNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchAllNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromAllNewOrder.Text = "";
            this.txtDateToAllNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            this.ddlProviderAllNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromAllNewOrder.Text = "";
            this.txtReleaseToAllNewOrder.Text = "";
            this.txtDateFromAllNewOrder.Text = "";
            this.txtDateToAllNewOrder.Text = "";
            this.ddlOrderTypesAllNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text = "";
            this.ddlStatesAllNewOrder.ClearSelection();

            PopulateAllNewOrders();

        }

        private void ClearAllNewFields()
        {
            this.txtOrderNoSearchAllNewOrder.Text = "";
            txtOrderNoHidden.Text = "0";
            this.txtCustomerNameSearchAllNewOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerIDHidden.Text = "0";
            this.txtCreatedBySearchAllNewOrder.Text = "";
            txtCreatedByUserIDHidden.Text = "0";
            this.txtDateFromAllNewOrder.Text = "";
            this.txtDateToAllNewOrder.Text = "";
            txtDateFromHidden.Text = DateTime.Now.ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            this.ddlProviderAllNewOrder.SelectedIndex = 0; ;
            this.txtReleaseFromAllNewOrder.Text = "";
            this.txtReleaseToAllNewOrder.Text = "";
            this.txtDateFromAllNewOrder.Text = "";
            this.txtDateToAllNewOrder.Text = "";
            this.ddlOrderTypesAllNewOrder.ClearSelection();
            this.txtOfficeSentOrdersGTINCodeSearchAllNewOrder.Text = "";
            this.ddlStatesAllNewOrder.ClearSelection();

        }

        /// <summary>
        /// Row Command for Gridview OrderLine Create Order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrderLineCreateOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = int.Parse(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "View":
                    int ProductID = 0;

                    int.TryParse(gvOrderLineCreateOrder.DataKeys[RowIndex].Value.ToString(), out ProductID);
                    hidProductIDnew.Value = "" + ProductID;
                    txtProductCodeChangeQty.Text = HttpUtility.HtmlDecode(gvOrderLineCreateOrder.Rows[RowIndex].Cells[2].Text);
                    txtDescriptionChangeQty.Text = HttpUtility.HtmlDecode(gvOrderLineCreateOrder.Rows[RowIndex].Cells[3].Text);
                    txtChangeQuantity.Text = gvOrderLineCreateOrder.Rows[RowIndex].Cells[4].Text;
                    txtDiscountChangeQuantity.Text = gvOrderLineCreateOrder.Rows[RowIndex].Cells[6].Text;

                    txtErrorMessageChangeQuantity.Visible = false;
                    mpeChangeQtyPopUp.Show();
                    break;

                case "Nothing":
                    int ProductID1 = 0;

                    int.TryParse(gvOrderLineCreateOrder.DataKeys[RowIndex].Value.ToString(), out ProductID1);
                    hidProductIDnew.Value = "" + ProductID1;
                    txtProductCodeChangeQty.Text = HttpUtility.HtmlDecode(gvOrderLineCreateOrder.Rows[RowIndex].Cells[1].Text);
                    mpeDeleteOrderLine.Show();
                    break;

            }

        }

        /// <summary>
        /// Change Quantity Save Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChangeQuantity_Click(object sender, EventArgs e)
        {
            if (this.txtChangeQuantity.Text.Trim() == "") this.txtChangeQuantity.Text = "0";
            if (int.Parse(txtChangeQuantity.Text) < 1 || txtChangeQuantity.Text.Length > 5)
            {
                txtErrorMessageChangeQuantity.Visible = true;
                mpeChangeQtyPopUp.Show();
            }
            else
            {
                DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];
                txtErrorMessageChangeQuantity.Visible = false;
                foreach (DataRow mrow in myTempOrderLineTable.Rows)
                {
                    if (mrow["ProductID"].ToString() == hidProductIDnew.Value)
                    {
                        if (txtDiscountChangeQuantity.Text.Trim() == "") txtDiscountChangeQuantity.Text = "0.00";
                        mrow["OrderQty"] = txtChangeQuantity.Text;
                        mrow["Discount"] = txtDiscountChangeQuantity.Text;
                        break;
                    }
                    else
                    {

                    }
                }

                Session[hidOrderLineTempID.Value] = myTempOrderLineTable;
                gvOrderLineCreateOrder.DataSource = myTempOrderLineTable;
                gvOrderLineCreateOrder.DataBind();

            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrderLineCreateOrder_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrderLineCreateOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];
            foreach (DataRow mrow in myTempOrderLineTable.Rows)
            {
                if (mrow["ProductCode"].ToString() == txtProductCodeChangeQty.Text)
                {
                    OrderLineIDList.Add(mrow["OrderLineID"].ToString());
                    mrow.Delete();
                    break;
                }
                else
                {

                }

            }

            gvOrderLineCreateOrder.DataSource = null;
            gvOrderLineCreateOrder.DataBind();

            Session[hidOrderLineTempID.Value] = myTempOrderLineTable;
            gvOrderLineCreateOrder.DataSource = myTempOrderLineTable;
            gvOrderLineCreateOrder.DataBind();
        }

        /// <summary>
        /// Button Yes Event for Order Deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnYes_Click(object sender, EventArgs e)
        {

            // GlobalVariables.OrderAppLib.OrderService.OrderDeleteRecord(int.Parse(txtOrderID.Text));
            DTOOrder mOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(int.Parse(txtOrderID.Text));

            //ClientScript.RegisterStartupScript(GetType(), "Successful", "alert('Successfully Deleted Order')", true);
            //FillNewOrderTreeViews();
            if (txtOrderID.ToolTip == "Office")
            {
                PopulateOfficeNewOrders();
            }
            else if (txtOrderID.ToolTip == "SalesRep")
            {
                mOrder.SYSOrderStatusID = 105;
                mOrder.DateUpdated = DateTime.Now;
                GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mOrder);
                PopulateSalesRepNewOrders();
                FillNewOrderTreeViews();
            }
            else
            {
                mOrder.SYSOrderStatusID = 106;
                mOrder.DateUpdated = DateTime.Now;
                GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mOrder);

                PopulateAllNewOrders();
                FillNewOrderTreeViews();
            }



        }

        /// <summary>
        /// Button Delete OrderLine Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteOrderLineOk_Click(object sender, EventArgs e)
        {
            DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];
            foreach (DataRow mrow in myTempOrderLineTable.Rows)
            {
                if (mrow["ProductID"].ToString() == hidProductIDnew.Value)
                {
                    OrderLineIDList.Add(mrow["OrderLineID"].ToString());
                    mrow.Delete();
                    break;
                }
                else
                {

                }
            }

            gvOrderLineCreateOrder.DataSource = null;
            gvOrderLineCreateOrder.DataBind();

            Session[hidOrderLineTempID.Value] = myTempOrderLineTable;

            if (myTempOrderLineTable != null && myTempOrderLineTable.Rows.Count > 0)
            {
                this.ddlProviderCreateOrder.Enabled = false;
                this.ddlOrderType.Enabled = false;

            }
            else
            {
                if (bool.Parse(this.ddlOrderType.SelectedValue))
                {
                    this.txtCreateOrderReleaseDate.Enabled = true;
                }
                else
                {
                    this.txtCreateOrderReleaseDate.Enabled = false;
                }
                this.ddlProviderCreateOrder.Enabled = true;
                
                this.ddlOrderType.Enabled = true;

            }

            gvOrderLineCreateOrder.DataSource = myTempOrderLineTable;
            gvOrderLineCreateOrder.DataBind();
        }



        /// <summary>
        /// Add Product By Product Code SelecIndexChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAddProductByProductCode_TextChanged(object sender, EventArgs e)
        {
           // ProductListByProductCode
            int ProviderID = 0;

            int.TryParse(ddlProviderCreateOrder.SelectedValue, out ProviderID);

            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByProviderProductCode(ProviderID, txtAddProductByProductCode.Text);

            bool IsRegularOrder = bool.Parse(this.ddlOrderType.SelectedValue);

            DateTime RequestedReleaseDate;
            DateTime OrderDate;

            
 
            
            // ADD Date conditions here.
            // If Pre-sell

            // PRE-SELL
            if (!IsRegularOrder)
            {
                OrderDate = DateTime.ParseExact(this.txtOrderDateCreateOrder.Text, "dd/MM/yyyy", null);
                if (mDTOProduct != null && mDTOProduct.ProductCode != null && mDTOProduct.StartDate > OrderDate && mDTOProduct.EndDate >= DateTime.Now)
                {
                    hidProductIDnew.Value = mDTOProduct.ProductID.ToString();
                    txtProductIDAddProductByProductCode.Text = mDTOProduct.ProductID.ToString();
                    txtProductDescriptionByProductCode.Text = mDTOProduct.ProductDescription.ToString();
                    hidProductGTINCode.Value = mDTOProduct.GTINCode;
                    hidDiscount.Value = mDTOProduct.Discount.ToString();
                }
                else
                {
                    hidProductIDnew.Value = "0";
                    txtProductIDAddProductByProductCode.Text = "0";
                    txtProductDescriptionByProductCode.Text = "Invalid Product Code, No Product Found";                   
                }
                txtAddByProductCodeQty.Focus();
                mpeAddProductByProductCode.Show();

            }
            // REGULAR
            else
            {
                RequestedReleaseDate = DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null);

                OrderDate = DateTime.ParseExact(this.txtOrderDateCreateOrder.Text, "dd/MM/yyyy", null);
                if (mDTOProduct != null && mDTOProduct.ProductCode != null && mDTOProduct.StartDate <= RequestedReleaseDate && mDTOProduct.EndDate >= DateTime.Now)
                {
                    hidProductIDnew.Value = mDTOProduct.ProductID.ToString();
                    txtProductIDAddProductByProductCode.Text = mDTOProduct.ProductID.ToString();
                    txtProductDescriptionByProductCode.Text = mDTOProduct.ProductDescription.ToString();
                    hidProductGTINCode.Value = mDTOProduct.GTINCode;
                    hidDiscount.Value = mDTOProduct.Discount.ToString();
                }
                else
                {
                    hidProductIDnew.Value = "0";
                    txtProductIDAddProductByProductCode.Text = "0";
                    txtProductDescriptionByProductCode.Text = "Invalid Product Code, No Product Found";                   
                }
                txtAddByProductCodeQty.Focus();
                mpeAddProductByProductCode.Show();

            }


        }

        /// <summary>
        /// Inserting of Products on the OrderLine Datatable from Pop Up AddProductByProductCode
        /// </summary>
        /// <param name="mRow"></param>
        private void InsertProductForOrderLineFromPopUp()
        {
            SelectionChange = true;

            DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];

            DataRow Row = myTempOrderLineTable.NewRow();

            long ProductID = int.Parse(txtProductIDAddProductByProductCode.Text);
            float OrderQty = int.Parse(txtAddByProductCodeQty.Text);
            string ProductDescription = txtProductDescriptionByProductCode.Text;
            string ProductCode = txtAddProductByProductCode.Text;
            string ProductGTINcode = hidProductGTINCode.Value;
            
            // Default Discount
            float ProductDiscount = float.Parse(this.hidDiscount.Value); // Get Discount from tblProviderProduct Table

            myTempOrderLineTable.Rows.Add(0, 1, ProductID, OrderQty, 1, "NA", 1, 1, "NA", "NA", ProductDescription, ProductCode, ProductGTINcode, ProductDiscount);
            Session[hidOrderLineTempID.Value] = myTempOrderLineTable;
        }


        /// <summary>
        /// Add Product Code From AddProductByProductCode Pop Up Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddproductByProductCode_Click(object sender, EventArgs e)
        {
            // txtProductIDAddProductByProductCode.Text = txtAddProductByProductCode.Text;

            if (CheckIfProductIDIsExistingOnOrderLineTable(int.Parse(txtProductIDAddProductByProductCode.Text), int.Parse(txtAddByProductCodeQty.Text)) && txtProductDescriptionByProductCode.Text != "Invalid Product Code, No Product Found")
            {
                InsertProductForOrderLineFromPopUp();
                ClearAddProductByProductCodeFields();
                mpeAddProductByProductCode.Show();
            }
            else
            {
                mpeAddProductByProductCode.Show();
            }

            gvOrderLineCreateOrder.DataSource = null;
            gvOrderLineCreateOrder.DataBind();

            DataTable mDT = (DataTable)Session[hidOrderLineTempID.Value];

            if (mDT != null && mDT.Rows.Count > 0)
            {
                ddlProviderCreateOrder.Enabled = false;
                ddlOrderType.Enabled = false;
            }
            else
            {
                ddlProviderCreateOrder.Enabled = true;
            }

            gvOrderLineCreateOrder.DataSource = mDT;
            gvOrderLineCreateOrder.DataBind();

        }

        /// <summary>
        /// Clear AddProductByProductCode Pop Up Fields
        /// </summary>
        private void ClearAddProductByProductCodeFields()
        {

            txtProductIDAddProductByProductCode.Text = "";
            txtProductDescriptionByProductCode.Text = "";
            txtAddByProductCodeQty.Text = "";
            txtAddProductByProductCode.Text = "";

        }


        /// <summary>
        /// Add ProductLine using AddProductByProductID Pop Up Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProductLineByProductCode_Click(object sender, EventArgs e)
        {
            ClearAddProductByProductCodeFields();
            txtAddProductByProductCode.Focus();
            mpeAddProductByProductCode.Show();

        }

        /// <summary>
        /// CreatedByUserIDSearch button clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnCreatedByUserIDSearch_Click(object sender, ImageClickEventArgs e)
        {

            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "Office";
            mpeSearchCreatedByUserID.Show();
            this.PopulateAllNewOrders();

        }

        // imgbtnCreatedByUserIDSearchOfficeNewOrder_Click
        protected void imgbtnCreatedByUserIDSearchOfficeNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event

            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "OfficeNewOrder";
            mpeSearchCreatedByUserID.Show();
            this.RerenderGridData(this.gvOfficeNewOrders);

        }

        // imgbtnCreatedByUserIDSearchSalesRepNewOrder_Click
        protected void imgbtnCreatedByUserIDSearchSalesRepNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event

            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "SalesRepNewOrder";
            mpeSearchCreatedByUserID.Show();

            this.RerenderGridData(this.gvSalesRepNewOrders);
        }

        // imgbtnCreatedByUserIDSearchAllNewOrder_Click
        protected void imgbtnCreatedByUserIDSearchAllNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event

            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "AllNewOrder";
            mpeSearchCreatedByUserID.Show();
            this.RerenderGridData(this.gvAllNewOrders);
        }


        protected void imgGTINCodeSearch_Click(object sender, ImageClickEventArgs e)
        {
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();

            this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            {
                this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            }
        }

        // imgGTINCodeSearchOfficeNewOrder_Click
        protected void imgGTINCodeSearchOfficeNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();

            this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            {
                this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            }

            this.mpeSearchProduct.Show();
            this.RerenderGridData(this.gvOfficeNewOrders);
          
        }

        // imgGTINCodeSearchSalesRepNewOrder_Click
        protected void imgGTINCodeSearchSalesRepNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();
            this.RerenderGridData(gvSalesRepNewOrders);

            //this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            //for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            //{
            //    this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            //}

            //this.mpeSearchProduct.Show();
        }

        // imgGTINCodeSearchAllNewOrder_Click
        protected void imgGTINCodeSearchAllNewOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Pre-sell and Future-dated event
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();
            this.RerenderGridData(this.gvAllNewOrders);
            //this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            //for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            //{
            //    this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            //}

            //this.mpeSearchProduct.Show();
        }


        // imgAllSentGTINCodeSearch_Click
        protected void imgAllSentGTINCodeSearch_Click(object sender, ImageClickEventArgs e)
        {
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();
            
            //this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            //for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            //{
            //    this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            //}
        }


        // imgGTINCodeSearchSalesRepSentOrders_Click
        protected void imgGTINCodeSearchSalesRepSentOrders_Click(object sender, ImageClickEventArgs e)
        {
            this.ddlSearchProductGroup.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
            this.ddlSearchProductGroup.DataBind();
            this.SetProductPerGroupOfficeSentOrders();

            //this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            //for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            //{
            //    this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());
            //}
        }


        public void SetProductPerGroupOfficeSentOrders()
        {

            var resp = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(this.ddlSearchProductGroup.SelectedValue), 0L, 1, 1000000);

            int pagesByPageSize = resp.Count / int.Parse(Session["PageSize"].ToString());
            int extraPage = resp.Count % int.Parse(Session["PageSize"].ToString()) > 0 ? 1 : 0;

            this.lblGTINOfficeSentOrderPages.Text = (pagesByPageSize + extraPage).ToString();            

            this.gvProductPerGroupSearch.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(this.ddlSearchProductGroup.SelectedValue), 0L, 1, 10);
            this.gvProductPerGroupSearch.DataBind();
            this.mpeSearchProduct.Show();

        }

        //public void SetProductPerGroupOfficeSentOrders()
        //{

        //    var resp = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(this.ddlSearchProductGroup.SelectedValue), 0L, 1, 1000000);

        //    int pagesByPageSize = resp.Count / int.Parse(Session["PageSize"].ToString());
        //    int extraPage = resp.Count % int.Parse(Session["PageSize"].ToString()) > 0 ? 1 : 0;

        //    this.lblGTINOfficeSentOrderPages.Text = (pagesByPageSize + extraPage).ToString();

        //    this.gvProductPerGroupSearch.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(int.Parse(this.ddlSearchProductGroup.SelectedValue), 0L, 1, 10);
        //    this.gvProductPerGroupSearch.DataBind();
        //    this.mpeSearchProduct.Show();

        //}



        /// <summary>
        /// Populate Organization Unit Dropdownlists
        /// </summary>
        private void FillOrganizationUnitDropdowns()
        {


            var respOrgUnits = GlobalVariables.OrderAppLib.CustomerService.OrgUnitListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

            ddlOrgUnitSearch.DataSource = respOrgUnits;
            ddlOrgUnitSearch.DataBind();

        }


        /// <summary>
        /// Populate Gridview Accounts based on OrganizationID
        /// </summary>
        /// <param name="OrgID"></param>
        private void FillAccountsBasedOnOrganizationID(int OrgID)
        {
            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(OrgID, int.Parse(Session["RefID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), txtSearchCreatedByUserID.Text);

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
                gvCreatedByUserIDs.DataSource = respAccounts;
                gvCreatedByUserIDs.DataBind();

            }
            else
            {
                AccountsPanel.Visible = false;
                gvCreatedByUserIDs.DataSource = null;
                gvCreatedByUserIDs.DataBind();
            }

            mpeSearchCreatedByUserID.Show();

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

            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(int.Parse(ddlOrgUnitSearch.SelectedValue), int.Parse(Session["RefID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), txtSearchCreatedByUserID.Text);

            gvCreatedByUserIDs.DataSource = respAccounts;

            gvCreatedByUserIDs.DataBind();

            mpeSearchCreatedByUserID.Show();

            // Dito rin po
            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }


        }

        /// <summary>
        /// Button Clicked Event For Paging Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GTINOfficeSentOrderPaging(object sender, EventArgs e)
        {
            // GTIN Paging

            LinkButton lnkButton = sender as LinkButton;

            int PageNumber = int.Parse(ddlPagesGTINOfficeSentOrder.SelectedItem.Text);

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
                    if (PageNumber < int.Parse(this.lblGTINOfficeSentOrderPages.Text))
                    {
                        PageNumber = PageNumber + 1;
                    }
                    break;
                case "Last":
                    PageNumber = int.Parse(this.lblGTINOfficeSentOrderPages.Text);
                    break;
            }

            this.ddlPagesGTINOfficeSentOrder.SelectedValue = PageNumber.ToString();

            var productsByGroup = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(
                                int.Parse(this.ddlSearchProductGroup.SelectedValue),
                                long.Parse(this.ddlProviderSentOrderSearch.SelectedValue),
                                PageNumber, int.Parse(Session["PageSize"].ToString()));

            this.gvProductPerGroupSearch.DataSource = productsByGroup;

            this.gvProductPerGroupSearch.DataBind();

            this.mpeSearchProduct.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }
        }

        /// <summary>
        /// Select index changed event for dropdownlist account pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAccountsPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PageNumber = int.Parse(ddlAccountsPages.SelectedValue);

            var respAccounts = GlobalVariables.OrderAppLib.AccountService.AccountListBySearch(int.Parse(ddlOrgUnitSearch.SelectedValue), int.Parse(Session["RefID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), txtSearchCreatedByUserID.Text);

            gvCreatedByUserIDs.DataSource = respAccounts;

            gvCreatedByUserIDs.DataBind();

            mpeSearchCreatedByUserID.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }

        }

        /// <summary>
        /// Select index changed event for dropdownlist SalesOrgUnit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOrgUnitSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            mpeSearchCreatedByUserID.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }

        }

        protected void gvCreatedByUserIDs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (Session["NodeReturn"].ToString() == "OfficeNewOrders") this.PopulateOfficeNewOrders();
            //    if (Session["NodeReturn"].ToString() == "Sales RepNewOrders") this.PopulateSalesRepNewOrders();
            //    if (Session["NodeReturn"].ToString() == "AllNewOrders") this.PopulateAllNewOrders();

            //}
                
        }

        protected void gvProductPerGroupSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (Session["NodeReturn"].ToString() == "OfficeNewOrders") this.PopulateOfficeNewOrders();
            //    if (Session["NodeReturn"].ToString() == "Sales RepNewOrders") this.PopulateSalesRepNewOrders();
            //    if (Session["NodeReturn"].ToString() == "AllNewOrders") this.PopulateAllNewOrders();

            //}

        }



        /// <summary>
        /// gvCreatedByUsersID RowCommand Event on Search CreatedByUserID Pop up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCreatedByUserIDs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = int.Parse(e.CommandArgument.ToString());

            switch (Session["SentMode"].ToString())
            {
                case "OfficeNewOrder":
                    Session["OfficeNewCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchOfficeNewOrder.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchOfficeNewOrder.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    this.RerenderGridData(this.gvOfficeNewOrders);
                    break;
                case "SalesRepNewOrder":
                    Session["SalesRepNewCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchSalesRepNewOrder.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchSalesRepNewOrder.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    this.RerenderGridData(this.gvSalesRepNewOrders);
                    break;
                case "AllNewOrder":
                    Session["AllNewCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchAllNewOrder.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearchAllNewOrder.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    this.RerenderGridData(this.gvAllNewOrders);
                    break;
                case "Office":
                    Session["OfficeSentCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearch.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtCreatedBySearch.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    break;
                case "SalesRep":
                    Session["SalesRepSentCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtSalesRepSentCreatedBy.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtSalesRepSentCreatedBy.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    break;
                case "All":
                    Session["AllSentCreatedByUserID"] = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtAllSentCreatedBy.Style.Add("border-color", "Gray");
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.DataKeys[RowIndex].Value.ToString();
                    txtAllSentCreatedBy.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    break;
            }
            
        }

        /// <summary>
        /// btnSearch Click event Search CreatedByUserID Pop up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchCreatedByUserID_Click(object sender, EventArgs e)
        {
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            mpeSearchCreatedByUserID.Show();
        }


        /// <summary>
        /// btnReleaseAllOrders on click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReleaseAllOrders_Click(object sender, EventArgs e)
        {
            if (gvSalesRepNewOrders.Rows.Count != 0)
            {
                Session["ReleaseType"] = "SalesRep";

                var ordersToBeReleased = new DTOOrderList();

                if (Session["SearchButtonClicked"] != null)
                {
                    ordersToBeReleased = this.GetReceivedAndCreatedOrders(this.PopulateAllNewSalesRepOrdersSearch());
                }
                else
                {
                  ordersToBeReleased = GlobalVariables.OrderAppLib.OrderService.OrderListToBeReleased(
                       int.Parse(Session["RefID"].ToString()),
                       DateTime.Now.Date,
                       Session["ReleaseType"].ToString()
                    );
                }

                //Session["SearchButtonClicked"] = null;
                this.lblOfficeNewRelease.Text = "Are you sure you want to release " + ordersToBeReleased.Count() + " Orders?";
                mpeReleaseAllOrders.Show();
                // added for testing
                this.PopulateSalesRepNewOrders();
            }
        }


        protected void btnReleaseAllOrdersOfficeNew_Click(object sender, EventArgs e)
        {
            if (gvOfficeNewOrders.Rows.Count != 0)
            {
                Session["ReleaseType"] = "Office";

                var ordersToBeReleased = new DTOOrderList();

                if (Session["SearchButtonClicked"] != null)
                {
                    ordersToBeReleased  = this.GetReceivedAndCreatedOrders(this.PopulateAllNewOfficeOrdersSearch());
                }
                else
                {
                    ordersToBeReleased = GlobalVariables.OrderAppLib.OrderService.OrderListToBeReleased(
                        int.Parse(Session["RefID"].ToString()),
                        DateTime.Now.Date,
                        Session["ReleaseType"].ToString()
                    );

                }

                //Session["SearchButtonClicked"] = null;
                ManageReleaseButtonOnNewOrders("OfficeNewOrders");
                this.lblOfficeNewRelease.Text = "Are you sure you want to release " + ordersToBeReleased.Count() + " Orders?";
                this.PopulateOfficeNewOrders();
                mpeReleaseAllOrders.Show();
            }
        }

        protected void btnReleaseAllOrdersAllOrders_Click(object sender, EventArgs e)
        {
            //if (gvAllNewOrders.Rows.Count != 0)
            //{
            //    // DECOMISSIONED METHOD
            //    // Session["ReleaseType"] = "All";
            //    // var newAllOrders = this.PopulateAllNewOrdersSearch();
            //    // this.lblOfficeNewRelease.Text = "Are you sure you want to release " + newAllOrders.Count() + " Orders?";
            //    // mpeReleaseAllOrders.Show();
            //}
        }


        protected void btnReleaseAllOrdersOk_Click(object sender, EventArgs e)
        {

            switch (Session["ReleaseType"].ToString())
            {
                case "Office":

                    DTOOrderList _officeOrders = null;
                    if (Session["SearchButtonClicked"] != null)
                    {
                        _officeOrders = this.GetReceivedAndCreatedOrders(this.PopulateAllNewOfficeOrdersSearch());
                    }
                    else
                    {
                        _officeOrders = GlobalVariables.OrderAppLib.OrderService.OrderListToBeReleased(
                            int.Parse(Session["RefID"].ToString()),
                            DateTime.Now.Date,
                            Session["ReleaseType"].ToString());
                    }

                    //foreach (GridViewRow mRow in gvOfficeNewOrders.Rows)
                    foreach (DTOOrder mRow in _officeOrders)
                    {
                        //if (mRow.Cells[7].Text != "Released") // ORIGINAL CODE
                        //if(mRow.SYSOrderStatusID != 102)
                        if (mRow.SYSOrderStatusID == 100 || mRow.SYSOrderStatusID == 101)
                        {                            
                            DateTime HeldDate;

                            //bool result = DateTime.TryParse(date, out HeldDate); // ORIGINAL CODE
                            bool result = DateTime.TryParse(string.Format("{0:dd/MM/yyyy}", mRow.HoldUntilDate), out HeldDate);

                            if (!result)
                            {
                                HeldDate = DateTime.ParseExact(HeldDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            }

                            if (HeldDate <= DateTime.Today)
                            {
                                // int OrderID = int.Parse(gvOfficeNewOrders.DataKeys[mRow.RowIndex].Value.ToString()); // ORIGINAL CODE                               
                                // DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);  // ORIGINAL CODE
                                txtDeliveryDateCreateOrder.Text = "";
                                //txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();  // ORIGINAL CODE
                                txtCustomerIDCreateOrder.Text = mRow.CustomerID.ToString();
                                
                                ReleaseOrderRecord(mRow, false, 102);
                            }
                        }
                    }
                    PopulateOfficeNewOrders();
                    break;

                case "SalesRep":


                    DTOOrderList _salesRepOrders = null;
                    if (Session["SearchButtonClicked"] != null)
                    {
                        _salesRepOrders = this.GetReceivedAndCreatedOrders(this.PopulateAllNewSalesRepOrdersSearch());                       
                    }
                    else
                    {
                        _salesRepOrders = GlobalVariables.OrderAppLib.OrderService.OrderListToBeReleased(
                            int.Parse(Session["RefID"].ToString()),
                            DateTime.Now.Date,
                            Session["ReleaseType"].ToString());
                    }

                    // foreach (GridViewRow mRow in gvSalesRepNewOrders.Rows)
                    foreach (DTOOrder mRow in _salesRepOrders)
                    {
                        // if (mRow.Cells[7].Text != "Released")
                        if (mRow.SYSOrderStatusID == 100 || mRow.SYSOrderStatusID == 101) // Chage this.
                        {
                            //if (mRow.Cells[5].Text != "Held")
                            //{
                            //    int OrderID = int.Parse(mRow.Cells[1].Text);
                            //    DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                            //    DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                            //    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                            //    txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                            //    // UpdateOrderRecord(mDTOOrder, false, 102);
                            //    ReleaseOrderRecord(mDTOOrder, false, 102);
                            //}
                            //else
                            //{

                            // string date = ((Label)(mRow.FindControl("lblHoldUntilDateSalesRepNew"))).Text;
                            
                            DateTime HeldDate;
                            bool result = DateTime.TryParse(string.Format("{0:dd/MM/yyyy}", mRow.HoldUntilDate), out HeldDate);

                            if (!result)
                            {
                                HeldDate = DateTime.ParseExact(HeldDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            }

                            //bool result = DateTime.TryParse(date, out HeldDate);

                            if (HeldDate <= DateTime.Today)
                            {
                                //int OrderID = int.Parse(gvSalesRepNewOrders.DataKeys[mRow.RowIndex].Value.ToString());
                                //DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                //DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                                //txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                txtDeliveryDateCreateOrder.Text = "";
                                txtCustomerIDCreateOrder.Text = mRow.CustomerID.ToString();
                                // UpdateOrderRecord(mDTOOrder, false, 102);
                                ReleaseOrderRecord(mRow, false, 102);

                            }
                            else
                            {
                                //int OrderID = int.Parse(gvSalesRepNewOrders.DataKeys[mRow.RowIndex].Value.ToString());
                                //DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);

                                //DateTime DeliverDate = HeldDate;//(DateTime)mDTOOrder.DeliveryDate;
                                //txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                //txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                                //// UpdateOrderRecord(mDTOOrder, false, 102);
                                //ReleaseOrderRecord(mDTOOrder, false, 102);
                            }
                        }
                    }
                    PopulateSalesRepNewOrders();
                    break;

                case "All":
                    {

                        DTOOrderList _allNewOrders = this.PopulateAllNewOrdersSearch();

                        // foreach (GridViewRow mRow in gvAllNewOrders.Rows)
                        foreach(DTOOrder mRow in _allNewOrders)
                        {
                            //if (mRow.Cells[7].Text != "Released")
                            if (mRow.SYSOrderStatusID != 102)
                            {
                                // string date = ((Label)(mRow.FindControl("lblHoldUntilDateAll"))).Text;

                                DateTime HeldDate;

                                bool result = DateTime.TryParse(string.Format("{0:dd/MM/yyyy}", mRow.HoldUntilDate), out HeldDate);

                                if (!result)
                                {
                                    HeldDate = DateTime.ParseExact(HeldDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                                }

                                if (HeldDate <= DateTime.Today)
                                {
                                    // int OrderID = int.Parse(gvAllNewOrders.DataKeys[mRow.RowIndex].Value.ToString());
                                    // DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                    txtDeliveryDateCreateOrder.Text = "";
                                    txtCustomerIDCreateOrder.Text = mRow.CustomerID.ToString();
                                    ReleaseOrderRecord(mRow, false, 102);
                                }
                                else
                                {
                                    //int OrderID = int.Parse(gvSalesRepNewOrders.DataKeys[mRow.RowIndex].Value.ToString());
                                    //DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                    //DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                                    //txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                    //txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                                    //ReleaseOrderRecord(mDTOOrder, false, 102);
                                }
                            }
                        }
                        PopulateAllNewOrders();
                        break;
                    }

            }

            Session["SearchButtonClicked"] = null; // Only this

        }

        protected void gvSalesRepSentOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            switch (e.CommandName)
            {

                case "Release":
                    OrderID = int.Parse(((Label)(gvSalesRepSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    // DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);

                    txtOrderNoCreateOrder.Text = gvSalesRepSentOrders.DataKeys[RowIndex].Values[1].ToString();
                    txtPONoViewOrders.Text = gvSalesRepSentOrders.DataKeys[RowIndex].Values[4].ToString();
                    

                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));

                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    UpdateOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    Session["ViewOrders"] = null;
                    PopulateStateDropdown();
                    OrderID = int.Parse(((Label)(gvSalesRepSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoCreateOrder.Text = gvSalesRepSentOrders.DataKeys[RowIndex].Values[1].ToString();
                    // txtPONoViewOrders.Text = gvSalesRepSentOrders.DataKeys[RowIndex].Values[4].ToString();
                    txtPONoCreateOrder.Text = gvSalesRepSentOrders.DataKeys[RowIndex].Values[4].ToString();
                    
                    DTOOrder respOrderDetails1 = GlobalVariables.OrderAppLib.OrderService.ListOrderByOrderNumberANDSalesOrgID(long.Parse(txtOrderNoCreateOrder.Text), long.Parse(Session["RefID"].ToString()));

                    string OrderStatus = gvSalesRepSentOrders.Rows[RowIndex].Cells[5].Text;

                    DateTime ActualReleaseDate = DateTime.Parse(gvSalesRepSentOrders.DataKeys[RowIndex].Values[3].ToString());
                    this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", ActualReleaseDate);

                    txtOrderID.Text = OrderID.ToString();

                    this.ddlOrderType.Enabled = false;

                    this.txtCreateOrderReleaseDate.Enabled = false;

                    this.ddlOrderType.SelectedItem.Text = bool.Parse(gvSalesRepSentOrders.DataKeys[RowIndex].Values[2].ToString()) ? "Regular" : "Pre-sell";
                
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(respOrderDetails1);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(respOrderDetails1);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;
            }
        }

        protected void imgSalesRepSentCreatedBy_Click(object sender, ImageClickEventArgs e)
        {
            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "SalesRep";
            mpeSearchCreatedByUserID.Show();

        }


        protected void imgAllSentCreatedBy_Click(object sender, ImageClickEventArgs e)
        {
            gvCreatedByUserIDs.DataSource = null;
            gvCreatedByUserIDs.DataBind();
            txtSearchCreatedByUserID.Text = "";

            FillOrganizationUnitDropdowns();
            FillAccountsBasedOnOrganizationID(int.Parse(ddlOrgUnitSearch.Text));
            Session["SentMode"] = "All";
            mpeSearchCreatedByUserID.Show();
        }


        protected void imgSalesRepSentCustomerNo_Click(object sender, ImageClickEventArgs e)
        {
            Session["SearchType"] = "Search SalesRep";


            btnCustomerSearch_Click(this.btnCustomerSearch, EventArgs.Empty);
            mpeCompanySearch.Show();
        }

        protected void imgAllSentCustomer_Click(object sender, ImageClickEventArgs e)
        {
            Session["SearchType"] = "Search All";
            btnCustomerSearch_Click(this.btnCustomerSearch, EventArgs.Empty);
            mpeCompanySearch.Show();
        }

        protected void btnSalesRepSentOrdersClear_Click(object sender, EventArgs e)
        {
            //Session["PageIndexOfficeSent"] = 1;
            Session["PageIndexSalesRepSent"] = 1;
            txtSalesRepSentOrderNo.Text = "";
            txtCustomerIDSearch.Text = "";
            Session["SalesRepSentSelectedCustomerID"] = "";
            Session["SalesRepSentCreatedByUserID"] = "";
            //txtSalesRepSentCustomerNo.Text = "";
            txtSalesRepSentCreatedBy.Text = "";
            txtSalesRepSentCustomerName.Text = "";
            txtSalesRepSentDateFrom.Text = "";
            txtSalesRepSentDateTo.Text = "";
            this.txtReleaseFromSalesRepSentOrders.Text = "";
            this.txtReleaseToSalesRepSentOrders.Text = "";
            this.txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders.Text = "";
            this.ddlStatesSalesRepSentOrders.SelectedIndex = 0;
            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderSentOrderSearch.SelectedIndex = 0;
            ddlOrderTypesSalesRepSentOrders.SelectedIndex = 0;
       
            PopulateSalesRepSentOrders();
        }

        private void ClearSalesRepSentFields()
        {
            txtSalesRepSentOrderNo.Text = "";
            txtCustomerIDSearch.Text = "";
            //txtSalesRepSentCustomerNo.Text = "";
            txtSalesRepSentCreatedBy.Text = "";
            txtSalesRepSentCustomerName.Text = "";
            txtSalesRepSentDateFrom.Text = "";
            txtSalesRepSentDateTo.Text = "";
            this.txtReleaseFromSalesRepSentOrders.Text = "";
            this.txtReleaseToSalesRepSentOrders.Text = "";
            this.txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders.Text = "";
            this.ddlStatesSalesRepSentOrders.SelectedIndex = 0;
            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderSentOrderSearch.SelectedIndex = 0;
            ddlOrderTypesSalesRepSentOrders.SelectedIndex = 0;

        }


        protected void btnAllSentOrdersClear_Click(object sender, EventArgs e)
        {
            Session["PageIndexAllSent"] = "1";
            Session["AllSentSelectedCustomerID"] = "";
            Session["AllSentCreatedByUserID"] = "";
            txtAllSentOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtAllSentCreatedBy.Text = "";
            txtAllSentCustomerName.Text = "";
            txtAllSentDateFrom.Text = "";
            txtAllSentDateTo.Text = "";
            this.txtAllSentReleaseFrom.Text = "";
            this.txtAllSentReleaseTo.Text = "";
            this.txtAllSentOrdersGTINCodeSearch.Text = "";
            this.ddlAllSentOrders.SelectedIndex = 0;
            this.ddlOrderTypesAllSent.SelectedIndex = 0;

            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderAllSent.SelectedIndex = 0;
            this.ddlStatesAllSentOrders.SelectedIndex = 0;

            this.PopulateAllSentOrders();
        }

        private void ClearAllSentFields()
        {
            txtAllSentOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtAllSentCreatedBy.Text = "";
            txtAllSentCustomerName.Text = "";
            txtAllSentDateFrom.Text = "";
            txtAllSentDateTo.Text = "";
            this.txtAllSentReleaseFrom.Text = "";
            this.txtAllSentReleaseTo.Text = "";
            this.txtAllSentOrdersGTINCodeSearch.Text = "";
            this.ddlAllSentOrders.SelectedIndex = 0;
            this.ddlOrderTypesAllSent.SelectedIndex = 0;

            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderAllSent.SelectedIndex = 0;
            this.ddlStatesAllSentOrders.SelectedIndex = 0;

        }

        protected void btnSalesRepSentOrdersSearch_Click(object sender, EventArgs e)
        {
            Session["PageIndexSalesRepSent"] = 1;

            //if (txtSalesRepSentOrderNo.Text == "")
            //{
            //    txtOrderNoHidden.Text = "0";
            //}
            //else
            //{
            //    txtOrderNoHidden.Text = txtSalesRepSentOrderNo.Text;
            //}
            ////CustomerID  Manage some shit #Do this First
            //if (txtCustomerIDSearch.Text == "")
            //{
            //    txtCustomerIDHidden.Text = "0";
            //}
            //else
            //{
            //    txtCustomerIDHidden.Text = txtCustomerIDSearch.Text;
            //}
            ////CreatedbyUserID
            //if (txtSalesRepSentCreatedBy.Text == "")
            //{
            //    txtCreatedByUserIDHidden.Text = "0";
            //}
            //else
            //{

            //}
            ////DateFrom
            //if (txtSalesRepSentDateFrom.Text == "")
            //{
            //    txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            //}
            //else
            //{
            //    DateTime fromOrderDate = DateTime.ParseExact(txtSalesRepSentDateFrom.Text, "dd/MM/yyyy", null);

            //    txtDateFromHidden.Text = fromOrderDate.ToString();
            //}
            ////DateTo
            //if (txtSalesRepSentDateTo.Text == "")
            //{
            //    txtDateToHidden.Text = DateTime.Now.ToString();
            //}
            //else
            //{
            //    DateTime ToOrderDate = DateTime.ParseExact(txtSalesRepSentDateTo.Text, "dd/MM/yyyy", null).AddDays(1);
            //    txtDateToHidden.Text = ToOrderDate.ToString();
            //}
            ////StatusID
            //txtStatusIDHidden.Text = ddlSalesRepSent.SelectedValue;

            PopulateSalesRepSentOrders();

        }

        protected void btnAllSentOrdersSearch_Click(object sender, EventArgs e)
        {

            Session["PageIndexAllSent"] = 1;

            //OrderNo
            if (txtAllSentOrder.Text == "")
            {
                txtOrderNoHidden.Text = "0";
            }
            else
            {
                txtOrderNoHidden.Text = txtAllSentOrder.Text;
            }
            //CustomerID  Manage some shit #Do this First
            if (txtCustomerIDSearch.Text == "")
            {
                txtCustomerIDHidden.Text = "0";
            }
            else
            {
                txtCustomerIDHidden.Text = txtCustomerIDSearch.Text;
            }
            //CreatedbyUserID
            if (txtAllSentCreatedBy.Text == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {

            }
            //DateFrom
            if (txtAllSentDateFrom.Text == "")
            {

                txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            }
            else
            {
                DateTime fromOrderDate = DateTime.ParseExact(txtAllSentDateFrom.Text, "dd/MM/yyyy", null);

                txtDateFromHidden.Text = fromOrderDate.ToString();
            }
            //DateTo
            if (txtAllSentDateTo.Text == "")
            {
                txtDateToHidden.Text = DateTime.Now.ToString();
            }
            else
            {
                DateTime ToOrderDate = DateTime.ParseExact(txtAllSentDateTo.Text, "dd/MM/yyyy", null).AddDays(1);
                txtDateToHidden.Text = ToOrderDate.ToString();
            }
            //StatusID
            txtStatusIDHidden.Text = ddlAllSentOrders.SelectedValue;

            PopulateAllSentOrders();
        }

        #region ************************tblOrderLine CRUDS ******************************************

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool CreateOrderLineRecord(DataTable OrderLineDataTable)
        {
            bool response = false;

            int LineNum = 1;

            foreach (DataRow mrow in OrderLineDataTable.Rows)
            {
                DTOOrderLine mDTO = new DTOOrderLine();
                mDTO.OrderLineID = 0;
                mDTO.OrderID = long.Parse(txtOrderID.Text);
                mDTO.LineNum =  LineNum;
                mDTO.ProductID = (long)mrow["ProductID"];
                mDTO.OrderQty = (float)mrow["OrderQty"];
                mDTO.DespatchQty = (float)mrow["DespatchQty"];
                mDTO.UOM = mrow["UOM"].ToString();
                mDTO.OrderPrice = (float)mrow["OrderPrice"];
                mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                mDTO.ErrorText = mrow["ErrorText"].ToString();
                mDTO.Discount = (float)mrow["Discount"];                

                string mMessage;
                if (GlobalVariables.OrderAppLib.OrderService.OrderLineIsValid(mDTO, out mMessage) == true)
                {                   
                    this.txtOrderLineID.Text = mDTO.OrderLineID.ToString();
                    // HERE?
                    GlobalVariables.OrderAppLib.OrderService.OrderLineSaveRecord(mDTO);
                    response = true;
                }
                else
                {
                    response = false;
                }
                                
                LineNum++;
            }
            

            return response;

        }

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool UpdateOrderLineRecord(DataTable OrderLineDataTable)
        {
            bool response = false;

            int LineNum = 1;
            foreach (DataRow mrow in OrderLineDataTable.Rows)
            {

                if (int.Parse(mrow["OrderLineID"].ToString()) == 0)
                {
                    DTOOrderLine mDTO = new DTOOrderLine();
                    mDTO.OrderLineID = 0;
                    mDTO.OrderID = long.Parse(hidOrderID.Value);
                    mDTO.LineNum = LineNum;// (int)mrow["LineNum"];
                    mDTO.ProductID = (long)mrow["ProductID"];
                    mDTO.OrderQty = (float)mrow["OrderQty"];
                    mDTO.DespatchQty = (float)mrow["DespatchQty"];
                    mDTO.UOM = mrow["UOM"].ToString();
                    mDTO.OrderPrice = (float)mrow["OrderPrice"];
                    mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                    mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                    mDTO.ErrorText = mrow["ErrorText"].ToString();
                    mDTO.Discount = (float)mrow["Discount"];

                    //NOTE: devs need to create a global variable to represent the Service Class. 
                    //      1. Create a class GlobalVariables on the UI level.
                    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
                    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
                    //string mMessage;
                    //if (GlobalVariables.OrderAppLib.OrderService.OrderLineIsValid(mDTO, out mMessage) == true)
                    //{

                    mDTO = GlobalVariables.OrderAppLib.OrderService.OrderLineSaveRecord(mDTO);
                    this.txtOrderLineID.Text = mDTO.OrderLineID.ToString();
                    response = true;
                    //}
                    //else
                    //{
                    //    response = false;
                    //    //show error here
                    //}
                }
                else
                {
                    DTOOrderLine mDTO = new DTOOrderLine();

                    mDTO.OrderLineID = (int)mrow["OrderLineID"];
                    mDTO.OrderID = long.Parse(hidOrderID.Value);
                    mDTO.LineNum = LineNum;//(int)mrow["LineNum"];
                    mDTO.ProductID = (long)mrow["ProductID"];
                    mDTO.OrderQty = (float)mrow["OrderQty"];
                    mDTO.DespatchQty = (float)mrow["DespatchQty"];
                    mDTO.UOM = mrow["UOM"].ToString();
                    mDTO.OrderPrice = (float)mrow["OrderPrice"];
                    mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                    mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                    mDTO.ErrorText = mrow["ErrorText"].ToString();
                    mDTO.Discount = (float)mrow["Discount"];


                    //NOTE: devs need to create a global variable to represent the Service Class. 
                    //      1. Create a class GlobalVariables on the UI level.
                    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
                    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
                    //string mMessage;
                    //if (GlobalVariables.OrderAppLib.OrderService.OrderLineIsValid(mDTO, out mMessage) == true)
                    //{
                    mDTO = GlobalVariables.OrderAppLib.OrderService.OrderLineSaveRecord(mDTO);
                    response = true;
                    //}
                    //else
                    //{
                    //    response = false;
                    //    //show error here
                    //}

                }

                LineNum++;

            }

            return response;

        }

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOOrderLine FetchOrderLineRecord(DataRowView mcurrentRow)
        //{
        //    DTOOrderLine mDTO = new DTOOrderLine();
        //    mDTO.OrderLineID = Int64.Parse(mcurrentRow["OrderLineID"].ToString());
        //    mDTO.OrderID =Int64.Parse(mcurrentRow["OrderID"].ToString());
        //    mDTO.LineNum = int.Parse(mcurrentRow["LineNum"].ToString());
        //    mDTO.ProductID = Int64.Parse(mcurrentRow["ProductID"].ToString());
        //    mDTO.OrderQty = float.Parse(mcurrentRow["OrderQty"].ToString());
        //    mDTO.DespatchQty = float.Parse(mcurrentRow["DespatchQty"].ToString());
        //    mDTO.UOM = mcurrentRow["UOM"].ToString();
        //    mDTO.OrderPrice = float.Parse(mcurrentRow["OrderPrice"].ToString());
        //    mDTO.DespatchPrice = float.Parse(mcurrentRow["DespatchPrice"].ToString());
        //    mDTO.ItemStatus = mcurrentRow["ItemStatus"].ToString();
        //    mDTO.ErrorText = mcurrentRow["ErrorText"].ToString();


        //    return mDTO;
        //}

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //private DTOOrderLine PopulateDTOOrderLine(int ObjectID)
        //{
        //    DTOOrderLine mDTO = new DTOOrderLine();
        //    using (DataTable mDT = GlobalVariables.OrderAppLib.OrderService.OrderLineListByID(ObjectID))
        //    {
        //        if (mDT.Rows.Count > 0)
        //        {
        //            DataRow mcurrentRow = mDT.Rows[0];
        //            mDTO.OrderLineID = mcurrentRow["OrderLineID"].ToString();
        //            mDTO.OrderID = mcurrentRow["OrderID"].ToString();
        //            mDTO.LineNum = int.Parse(mcurrentRow["LineNum"].ToString());
        //            mDTO.ProductID = mcurrentRow["ProductID"].ToString();
        //            mDTO.OrderQty = mcurrentRow["OrderQty"].ToString();
        //            mDTO.DespatchQty = mcurrentRow["DespatchQty"].ToString();
        //            mDTO.UOM = mcurrentRow["UOM"].ToString();
        //            mDTO.OrderPrice = mcurrentRow["OrderPrice"].ToString();
        //            mDTO.DespatchPrice = mcurrentRow["DespatchPrice"].ToString();
        //            mDTO.ItemStatus = mcurrentRow["ItemStatus"].ToString();
        //            mDTO.ErrorText = mcurrentRow["ErrorText"].ToString();
        //        }
        //        return mDTO;
        //    }

        //}

        #endregion ************************End of tblOrderLine CRUDS *********************************


        #region ************************tblOrder CRUDS ******************************************

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //private void ShowOrderRecord(DTOOrder mDTO)
        //{-
        //    txtOrderID.Text = mDTO.OrderID.ToString();
        //    txtSalesOrgID.Text = mDTO.SalesOrgID.ToString();
        //    txtChargetoCustomerID.Text = mDTO.ChargetoCustomerID.ToString();
        //    txtDelivertoCustomerID.Text = mDTO.DelivertoCustomerID.ToString();
        //    txtCustomerID.Text = mDTO.CustomerID.ToString();
        //    txtSalesRepAccountID.Text = mDTO.SalesRepAccountID.ToString();
        //    txtProviderID.Text = mDTO.ProviderID.ToString();
        //    txtProviderWarehouseID.Text = mDTO.ProviderWarehouseID.ToString();
        //    txtOrderDate.Text = mDTO.OrderDate.ToShortDateString();
        //    txtDeliveryDate.Text = mDTO.DeliveryDate.ToShortDateString();
        //    txtInvoiceDate.Text = mDTO.InvoiceDate.ToShortDateString();
        //    txtSYSOrderStatusID.Text = mDTO.SYSOrderStatusID.ToString();
        //    txtOrderNumber.Text = mDTO.OrderNumber.ToString();
        //    txtReceivedDate.Text = mDTO.ReceivedDate.ToShortDateString();
        //    txtReleaseDate.Text = mDTO.ReleaseDate.ToShortDateString();
        //    txtIsSent.Text = mDTO.IsSent.ToString();
        //    txtIsHeld.Text = mDTO.IsHeld.ToString();
        //}

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool CreateOrderRecord()
        {
            bool response = false;

            DTOOrder mDTO = new DTOOrder();
            
            mDTO.OrderID = 0;
            mDTO.SalesOrgID = Int64.Parse(Session["RefID"].ToString());

            int CustomerID = 0;

            int.TryParse(txtCustomerIDCreateOrder.Text, out CustomerID);

            mDTO.CustomerID = CustomerID;
            mDTO.SalesRepAccountID = 99;
            mDTO.ProviderID = int.Parse(ddlProviderCreateOrder.SelectedValue);
            mDTO.ProviderWarehouseID = int.Parse(ddlProviderWarehouseCreateOrder.SelectedValue);
            DateTime newOrderDate = DateTime.Now;
            mDTO.OrderDate = newOrderDate;
            if (txtDeliveryDateCreateOrder.Text == "")
            {
                mDTO.DeliveryDate = null;
            }
            else
            {
                DateTime newDeliverDate = DateTime.ParseExact(txtDeliveryDateCreateOrder.Text, "dd/MM/yyyy", null);
                mDTO.DeliveryDate = newDeliverDate;
            }
            if (txtHoldUntilDate.Text == "")
            {
                mDTO.HoldUntilDate = null;
            }
            else
            {
                DateTime newholdUntilDate = DateTime.ParseExact(txtHoldUntilDate.Text, "dd/MM/yyyy", null);
                mDTO.HoldUntilDate = newholdUntilDate.Date;
            }

            mDTO.InvoiceDate = DateTime.Now;
            mDTO.SYSOrderStatusID = 101;
            mDTO.OrderNumber = hidOrderNumber.Value;
            mDTO.ReceivedDate = DateTime.Now;
            mDTO.ReleaseDate = DateTime.Now;
            mDTO.DateCreated = DateTime.Now;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.IsSent = false;
            mDTO.IsHeld = true;
            mDTO.CreatedByUserID = Int64.Parse(Session["AccountID"].ToString());
            mDTO.UpdatedByUserID = Int64.Parse(Session["AccountID"].ToString());
            mDTO.OrderGUID = "";

            if (ddlOrderType.SelectedItem.Text == "Regular")
            {
                mDTO.IsRegularOrder = true;
                mDTO.RequestedReleaseDate = DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null);
                
            }
            else if (ddlOrderType.SelectedItem.Text == "Pre-sell")
            {
                mDTO.IsRegularOrder = false;
                mDTO.RequestedReleaseDate = null;
            }


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.OrderService.OrderIsValid(mDTO, out mMessage) == true)
            {

                mDTO = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTO);

                

                this.hidOrderID.Value = mDTO.OrderID.ToString();
                this.txtOrderID.Text = mDTO.OrderID.ToString();
                
                
                DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];

                // TESTING PURPOSES ONLY 2016-04-16
                //DTOProvider _provider = GlobalVariables.OrderAppLib.ProviderService.ProviderListByID(mDTO.ProviderID);
                //if (_provider.IsPepsiDistributor == true)
                //{
                //    CreateMessageInboundRecord(int.Parse(mDTO.OrderID.ToString()));
                //}
               
                if (CreateOrderLineRecord(myTempOrderLineTable))
                {
                    response = true;

                }
                else
                {
                    response = false;
                }
            }
            else
            {
                lblOrderErrorMessage.Text = mMessage;
                response = false;
                //show error here
            }
            
            return response;
        }

        private bool ReleaseOrderRecord(DTOOrder mDTOOrderDetails, bool IsHeld, int OrderStatusID)
        {
            bool response = false;

            txtOrderID.Text = mDTOOrderDetails.OrderID.ToString();
            mDTOOrderDetails.IsHeld = IsHeld;
            mDTOOrderDetails.ReleaseDate = DateTime.Now;
            mDTOOrderDetails.SYSOrderStatusID = OrderStatusID;
            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.OrderService.OrderIsValid(mDTOOrderDetails, out mMessage) == true)
            {
                // REGULAR: If the requested release date is less than or equal to today, release the order.
                if (mDTOOrderDetails.IsRegularOrder == true && mDTOOrderDetails.RequestedReleaseDate <= DateTime.Now)
                {
                    mDTOOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTOOrderDetails);
                }

                // PRE-SELL: FINALIZED 
                if (mDTOOrderDetails.IsRegularOrder == false)
                {
                    DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(mDTOOrderDetails.OrderID);
                    DateTime highestProductStartDate = DateTime.MinValue;

                    // Loop thru the OrderLineList
                    foreach (DTOOrderLine _orderLine in respOrderLineDetails)
                    {
                        // For each OrderLineList, get the ProductID and Get the ProviderID
                        DTOProviderProduct _product =
                        GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(mDTOOrderDetails.ProviderID, _orderLine.ProductID);

                        // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                        if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;
                    }

                    // if highest start date of the products is <= today show the Release Icon
                    if (highestProductStartDate <= DateTime.Now)
                    {
                        // Place creation of email here for PepsiCo Distriobutor only??
                        // For Pre-sell orders, this will be the only time pre-sell will have to be emailed for Distributor ONLY

                        // If !PepsiCo and !PepsiDistributor, just update. else if it is, then create the MessageInbound, all else will follow the same route.


                        mDTOOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTOOrderDetails);
                    }

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

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool UpdateOrderRecord(DTOOrder mDTOOrderDetails, bool IsHeld, int OrderStatusID)
        {
            bool response = false;

            txtOrderID.Text = mDTOOrderDetails.OrderID.ToString();
            DTOOrder mDTO = new DTOOrder();
            mDTO.OrderID = mDTOOrderDetails.OrderID;
            mDTO.SalesOrgID = mDTOOrderDetails.SalesOrgID;

            if (txtCustomerIDCreateOrder.Text.Trim() == "")
            {
                mDTO.CustomerID = mDTOOrderDetails.CustomerID;
            }
            else
            {
                mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
            }
            
            mDTO.SalesRepAccountID = mDTOOrderDetails.SalesRepAccountID;
            mDTO.ProviderID = int.Parse(ddlProviderCreateOrder.SelectedValue);
            mDTO.ProviderWarehouseID = int.Parse(ddlProviderWarehouseCreateOrder.SelectedValue);
            mDTO.OrderDate = mDTOOrderDetails.OrderDate;

            if (this.txtCreateOrderReleaseDate.Text.Trim() != "")
            {
                mDTO.RequestedReleaseDate = DateTime.ParseExact(this.txtCreateOrderReleaseDate.Text, "dd/MM/yyyy", null);
            }
            else
            {
                mDTO.RequestedReleaseDate = null;                 
            }



            if (txtDeliveryDateCreateOrder.Text == "")
            {
                mDTO.DeliveryDate = null;
            }
            else
            {
                DateTime newDeliverDate = DateTime.ParseExact(txtDeliveryDateCreateOrder.Text, "dd/MM/yyyy", null);
                mDTO.DeliveryDate = newDeliverDate;
            }

            if (txtHoldUntilDate.Text == "")
            {
                mDTO.HoldUntilDate = null;
            }
            else
            {
                DateTime newHoldUntilDate = DateTime.ParseExact(txtHoldUntilDate.Text, "dd/MM/yyyy", null);
                mDTO.HoldUntilDate = newHoldUntilDate.Date;
            }

            mDTO.InvoiceDate = mDTOOrderDetails.InvoiceDate;
            mDTO.SYSOrderStatusID = OrderStatusID;

            mDTO.OrderNumber = mDTOOrderDetails.OrderNumber;
            mDTO.ReceivedDate = mDTOOrderDetails.ReceivedDate;
            mDTO.ReleaseDate = mDTOOrderDetails.ReleaseDate;
            mDTO.DateCreated = mDTOOrderDetails.DateCreated;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.IsSent = mDTOOrderDetails.IsSent;
            mDTO.IsHeld = IsHeld;
            mDTO.CreatedByUserID = mDTOOrderDetails.CreatedByUserID;
            mDTO.UpdatedByUserID = Int64.Parse(Session["AccountID"].ToString());
            mDTO.OrderGUID = "";
            mDTO.IsRegularOrder = bool.Parse(this.ddlOrderType.SelectedValue);

            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.OrderService.OrderIsValid(mDTO, out mMessage) == true)
            {
                mDTO = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTO);
                DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];
                if (UpdateOrderLineRecord(myTempOrderLineTable))
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }
            else
            {
                lblOrderErrorMessage.Text = mMessage;
                response = false;
                //show error here
            }

            return response;

        }

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //This is to be placed in the main ENTITY form that calls the CRUD
        //Populated by a binding source control: DataRowView mRow =(DataRowView)this.bsRegisteredVoters.Current; 
        //example: mDTO = FetchRecord(mRow); 

        //private DTOOrder FetchRecord(DataRowView mcurrentRow)
        //{
        //    DTOOrder mDTO = new DTOOrder();
        //    mDTO.OrderID = mcurrentRow["OrderID"].ToString();
        //    mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //    mDTO.ChargetoCustomerID = int.Parse(mcurrentRow["ChargetoCustomerID"].ToString());
        //    mDTO.DelivertoCustomerID = int.Parse(mcurrentRow["DelivertoCustomerID"].ToString());
        //    mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //    mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //    mDTO.ProviderID = mcurrentRow["ProviderID"].ToString();
        //    mDTO.ProviderWarehouseID = int.Parse(mcurrentRow["ProviderWarehouseID"].ToString());
        //    mDTO.OrderDate = (DateTime)mcurrentRow["OrderDate"];
        //    mDTO.DeliveryDate = (DateTime)mcurrentRow["DeliveryDate"];
        //    mDTO.InvoiceDate = (DateTime)mcurrentRow["InvoiceDate"];
        //    mDTO.SYSOrderStatusID = int.Parse(mcurrentRow["SYSOrderStatusID"].ToString());
        //    mDTO.OrderNumber = mcurrentRow["OrderNumber"].ToString();
        //    mDTO.ReceivedDate = (DateTime)mcurrentRow["ReceivedDate"];
        //    mDTO.ReleaseDate = (DateTime)mcurrentRow["ReleaseDate"];
        //    mDTO.IsSent = bool.Parse(mcurrentRow["IsSent"].ToString());
        //    mDTO.IsHeld = bool.Parse(mcurrentRow["IsHeld"].ToString());


        //    return mDTO;
        //}

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //private DTOOrder PopulateDTOOrder(int ObjectID)
        //{
        //    DTOOrder mDTO = new DTOOrder();
        //    using (DataTable mDT = GlobalVariables.OrderAppLib.OrderService.OrderListByID(ObjectID))
        //    {
        //        if (mDT.Rows.Count > 0)
        //        {
        //            DataRow mcurrentRow = mDT.Rows[0];
        //            mDTO.OrderID = mcurrentRow["OrderID"].ToString();
        //            mDTO.SalesOrgID = mcurrentRow["SalesOrgID"].ToString();
        //            mDTO.ChargetoCustomerID = int.Parse(mcurrentRow["ChargetoCustomerID"].ToString());
        //            mDTO.DelivertoCustomerID = int.Parse(mcurrentRow["DelivertoCustomerID"].ToString());
        //            mDTO.CustomerID = mcurrentRow["CustomerID"].ToString();
        //            mDTO.SalesRepAccountID = mcurrentRow["SalesRepAccountID"].ToString();
        //            mDTO.ProviderID = mcurrentRow["ProviderID"].ToString();
        //            mDTO.ProviderWarehouseID = int.Parse(mcurrentRow["ProviderWarehouseID"].ToString());
        //            mDTO.OrderDate = (DateTime)mcurrentRow["OrderDate"];
        //            mDTO.DeliveryDate = (DateTime)mcurrentRow["DeliveryDate"];
        //            mDTO.InvoiceDate = (DateTime)mcurrentRow["InvoiceDate"];
        //            mDTO.SYSOrderStatusID = int.Parse(mcurrentRow["SYSOrderStatusID"].ToString());
        //            mDTO.OrderNumber = mcurrentRow["OrderNumber"].ToString();
        //            mDTO.ReceivedDate = (DateTime)mcurrentRow["ReceivedDate"];
        //            mDTO.ReleaseDate = (DateTime)mcurrentRow["ReleaseDate"];
        //            mDTO.IsSent = (Boolean)mcurrentRow["IsSent"];
        //            mDTO.IsHeld = (Boolean)mcurrentRow["IsHeld"];
        //        }
        //        return mDTO;
        //    }

        //}

        #endregion ************************End of tblOrder CRUDS *********************************

        #region **********************tblMessageInbound CRUDS*************************************

        // TESTING PURPOSES ONLY 2016-04-16
        private void CreateMessageInboundRecord(int OrderID)
        {
            DTOOrderSignature _officeSignature = new DTOOrderSignature();
            _officeSignature.OrderID = OrderID;
            _officeSignature.Path = "officecreatedorder.png";
            _officeSignature.DateCreated = DateTime.Now;
            GlobalVariables.OrderAppLib.OrderService.OrderSignatureSaveRecord(_officeSignature);

            DTOMessageInbound mDTO = new DTOMessageInbound();
            mDTO.MessageinboundID = 0;
            mDTO.OrderID = OrderID;
            mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
            mDTO.SentFlag = false;
            mDTO.DateSent = DateTime.Now;
            mDTO.MessageInboundType = "order";
            mDTO = GlobalVariables.OrderAppLib.OrderService.MessageInboundSaveRecord(mDTO);
        }

        #endregion ************************End of tblMessageInbounds CRUDS *********************************

        protected void gvOfficeNewOrders_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow mGrow in gvOfficeNewOrders.Rows)
            {
                string Status = mGrow.Cells[7].Text; // Status : take note of the index, since it changes when there are new fields added.

                if (Status == "Released")
                {
                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDate"))).Visible = false;
                }
                else
                {
                    DateTime RequestedReleaseDate;
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                    DateTime.TryParse(((Label)(mGrow.FindControl("lblRequestedReleaseDate"))).Text, out RequestedReleaseDate);

                    if (mGrow.Cells[5].Text == "Regular")
                    {
                        if (RequestedReleaseDate > DateTime.Now)
                        {
                            mGrow.Cells[8].Text = string.Format(dateformat, RequestedReleaseDate);
                        }
                        else
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                        }
                    }
                    else if (mGrow.Cells[5].Text == "Pre-sell")
                    {
                        // OrderID from the Hidden Field in the GridView
                        long _OrderID = long.Parse(((Label)(mGrow.FindControl("lblOrderID"))).Text);

                        // Get order details
                        DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(_OrderID);

                        // Get order line details
                        DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_OrderID);

                        DateTime highestProductStartDate = DateTime.MinValue;
                        // Loop thru the OrderLineList
                        foreach (DTOOrderLine _orderLine in respOrderLineDetails)
                        {
                            // For each OrderLineList, get the ProductID and Get the ProviderID
                            DTOProviderProduct _product =
                            GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(respOrderDetails.ProviderID, _orderLine.ProductID);

                            // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                            if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;

                        }

                        // if highest start date of the products is <= today show the Release Icon
                        if (highestProductStartDate <= DateTime.Now)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                        }
                        // if highest start date is > today display blank
                        else if (highestProductStartDate > DateTime.Now)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = false;
                            mGrow.Cells[8].Text = "";
                        }
                    }
                }
            }
        }

        private void RerenderGridData(GridView OrdersGridView)
        {
            if (OrdersGridView != null || OrdersGridView.Rows.Count <= 0)
            {
                foreach (GridViewRow mGrow in OrdersGridView.Rows)
                {
                    string Status = mGrow.Cells[7].Text;

                    if (Status == "Released")
                    {
                        if (OrdersGridView == this.gvOfficeNewOrders)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = false;
                        }
                        else if (OrdersGridView == this.gvSalesRepNewOrders)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = false;
                        }
                        else if (OrdersGridView == this.gvAllNewOrders)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                        }

                    }
                    else
                    {
                        DateTime? RequestedReleaseDate = DateTime.ParseExact("01-01-1900", "dd-MM-yyyy", null);

                        string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                        long mOrderID = long.Parse(((Label)(mGrow.FindControl("lblOrderID"))).Text);
                        DTOOrder _Order = GlobalVariables.OrderAppLib.OrderService.OrderListByID(mOrderID);

                        RequestedReleaseDate = _Order.RequestedReleaseDate;

                        if (mGrow.Cells[5].Text == "Regular")
                        {
                            if (RequestedReleaseDate > DateTime.Now)
                            {
                                mGrow.Cells[8].Text = string.Format(dateformat, RequestedReleaseDate);
                            }
                            else
                            {
                                if (OrdersGridView == this.gvOfficeNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                                }
                                else if (OrdersGridView == this.gvSalesRepNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = true;
                                }
                                else if (OrdersGridView == this.gvAllNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                                }
                            }
                        }
                        else if (mGrow.Cells[5].Text == "Pre-sell")
                        {
                            // OrderID from the Hidden Field in the GridView
                            long _OrderID = long.Parse(((Label)(mGrow.FindControl("lblOrderID"))).Text);

                            // Get order details
                            DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(_OrderID);

                            // Get order line details
                            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_OrderID);

                            DateTime highestProductStartDate = DateTime.MinValue;
                            // Loop thru the OrderLineList
                            foreach (DTOOrderLine _orderLine in respOrderLineDetails)
                            {
                                // For each OrderLineList, get the ProductID and Get the ProviderID
                                DTOProviderProduct _product =
                                GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(respOrderDetails.ProviderID, _orderLine.ProductID);

                                // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                                if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;

                            }

                            // if highest start date of the products is <= today show the Release Icon
                            if (highestProductStartDate <= DateTime.Now)
                            {
                                if (OrdersGridView == this.gvOfficeNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                                }
                                else if (OrdersGridView == this.gvSalesRepNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = true;
                                }
                                else if (OrdersGridView == this.gvAllNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                                }
                            }
                            // if highest start date is > today display blank
                            else if (highestProductStartDate > DateTime.Now)
                            {
                                if (OrdersGridView == this.gvOfficeNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = false;
                                }
                                else if (OrdersGridView == this.gvSalesRepNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = false;
                                }
                                else if (OrdersGridView == this.gvAllNewOrders)
                                {
                                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                                }

                                mGrow.Cells[8].Text = "";
                            }
                        }
                    }
                }
            }
        }

        protected void gvSalesRepNewOrders_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow mGrow in gvSalesRepNewOrders.Rows)
            {
                string Status = mGrow.Cells[7].Text;

                if (Status == "Released")
                {
                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRepNew"))).Visible = false;
                }

                else
               {
            //        //// COMMENTED OUT FOR TESTING
            //        //DateTime HoldDate;
            //        //DateTime.TryParse(((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text, out HoldDate);

            //        //if (HoldDate > DateTime.Today)
            //        //{
            //        //    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRep"))).Visible = false;
            //        //    ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Visible = false;
            //        //    string Holddate = String.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text));
            //        //    ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text = Holddate;
            //        //}
            //        //else
            //        //{
            //        //    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRep"))).Visible = true;
            //        //    ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Visible = false;

            //        //}

                   DateTime RequestedReleaseDate;
                   string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                   DateTime.TryParse(((Label)(mGrow.FindControl("lblRequestedReleaseDateSalesRepNew"))).Text, out RequestedReleaseDate);

                   string __date = ((Label)(mGrow.FindControl("lblRequestedReleaseDateSalesRepNew"))).Text;

                   if (mGrow.Cells[5].Text == "Regular")
                   {
                       if (RequestedReleaseDate > DateTime.Now)
                       {
                           mGrow.Cells[8].Text = string.Format(dateformat, RequestedReleaseDate);
                       }
                       else
                       {
                           ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = true;
                       }
                   }
                   else if (mGrow.Cells[5].Text == "Pre-sell")
                   {
                       // OrderID from the Hidden Field in the GridView
                       long _OrderID = long.Parse(((Label)(mGrow.FindControl("lblOrderIDSalesRepNew"))).Text);

                       // Get order details
                       DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(_OrderID);

                       // Get order line details
                       DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_OrderID);

                       DateTime highestProductStartDate = DateTime.MinValue;
                       // Loop thru the OrderLineList
                       foreach (DTOOrderLine _orderLine in respOrderLineDetails)
                       {
                           // For each OrderLineList, get the ProductID and Get the ProviderID
                           DTOProviderProduct _product =
                           GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(respOrderDetails.ProviderID, _orderLine.ProductID);

                           // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                           if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;

                       }

                       // if highest start date of the products is <= today show the Release Icon
                       if (highestProductStartDate <= DateTime.Now)
                       {
                           ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = true;
                       }
                       // if highest start date is > today display blank
                       else if (highestProductStartDate > DateTime.Now)
                       {
                           ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRepNew"))).Visible = false;
                           mGrow.Cells[8].Text = "";
                       }
                   }
               }

            }

        }

        protected void gvAllNewOrders_DataBound(object sender, EventArgs e)
        {

            foreach (GridViewRow mGrow in gvAllNewOrders.Rows)
            {

                string Status = mGrow.Cells[7].Text;
                if (Status == "Released")
                {
                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = true;
                }
                else
                {

                    DateTime RequestedReleaseDate;
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";

                    DateTime.TryParse(((Label)(mGrow.FindControl("lblRequestedReleaseDateAllNewOrders"))).Text, out RequestedReleaseDate);

                    if (mGrow.Cells[5].Text == "Regular")
                    {
                        if (RequestedReleaseDate > DateTime.Now)
                        {
                            mGrow.Cells[8].Text = string.Format(dateformat, RequestedReleaseDate);
                        }
                        else
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                        }
                    }
                    else if (mGrow.Cells[5].Text == "Pre-sell")
                    {
                        // OrderID from the Hidden Field in the GridView
                        long _OrderID = long.Parse(((Label)(mGrow.FindControl("lblOrderID"))).Text);

                        // Get order details
                        DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(_OrderID);

                        // Get order line details
                        DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_OrderID);

                        DateTime highestProductStartDate = DateTime.MinValue;
                        // Loop thru the OrderLineList
                        foreach (DTOOrderLine _orderLine in respOrderLineDetails)
                        {
                            // For each OrderLineList, get the ProductID and Get the ProviderID
                            DTOProviderProduct _product =
                            GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(respOrderDetails.ProviderID, _orderLine.ProductID);

                            // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                            if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;

                        }

                        // if highest start date of the products is <= today show the Release Icon
                        if (highestProductStartDate <= DateTime.Now)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                        }
                        // if highest start date is > today display blank
                        else if (highestProductStartDate > DateTime.Now)
                        {
                            ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                            mGrow.Cells[8].Text = "";
                        }
                    }
                    


                    //DateTime HoldDate;
                    //DateTime.TryParse(((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text, out HoldDate);
                    //if (HoldDate > DateTime.Today)
                    //{
                    //    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                    //    ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = true;
                    //    string Holddate = String.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text));
                    //    ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text = Holddate;
                    //}
                    //else
                    //{
                    //    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                    //    ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = false;

                    //}





                }

            }

        }

        private void CheckifUsernameExist(string userName, string Type)
        {

            //Boolean result = false;

            DTOAccount mDTO = GlobalVariables.OrderAppLib.AccountService.AccountListByUsername(long.Parse(Session["RefID"].ToString()), userName);




            if (mDTO != null && mDTO.AccountID != 0)
            {

                txtCreatedByUserIDHidden.Text = mDTO.AccountID.ToString();

                txtCreatedBySearch.Style.Add("border-color", "Gray");
                txtSalesRepSentCreatedBy.Style.Add("border-color", "Gray");
                txtAllSentCreatedBy.Style.Add("border-color", "Gray");


            }
            else
            {
                txtCreatedByUserIDHidden.Text = "0";
                if (Type == "Office")
                {
                    if (txtCreatedBySearch.Text != "")
                    {
                        txtCreatedBySearch.Style.Add("border-color", "Red");
                    }
                    else
                    {

                        txtCreatedBySearch.Style.Add("border-color", "Gray");
                    }


                }
                else if (Type == "SalesRep")
                {
                    if (txtSalesRepSentCreatedBy.Text != "")
                    {
                        txtSalesRepSentCreatedBy.Style.Add("border-color", "Red");
                    }
                    else
                    {
                        txtSalesRepSentCreatedBy.Style.Add("border-color", "Gray");
                    }

                }
                else
                {
                    if (txtAllSentCreatedBy.Text != "")
                    {
                        txtAllSentCreatedBy.Style.Add("border-color", "Red");
                    }
                    else
                    {

                        txtAllSentCreatedBy.Style.Add("border-color", "Gray");

                    }

                }

            }



        }

        protected void btnbackSentOrder_Click(object sender, EventArgs e)
        {
            SetViewBasedOnSelectedNodeSentOrders(Session["NodeReturn"].ToString().Replace("SentOrders", ""));

        }


        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).SelectedItem.Text == "Regular")
            {
                this.txtCreateOrderReleaseDate.Enabled = true;
                this.txtCreateOrderReleaseDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            }
            else if (((DropDownList)sender).SelectedItem.Text == "Pre-sell")
            {
                this.txtCreateOrderReleaseDate.Text = "";
                this.txtCreateOrderReleaseDate.Enabled = false;
            }

        }

        protected void ddlPagesGTINOfficeSentOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mpeSearchProduct.Show();

            if (MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);
            }
            else if (MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }

        }

        protected void ddlProviderCreateOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomerCreateOrder.Text = "";
            txtStoreNameCreateOrder.Text = "";
            FillProviderWarehouseDropDownList(int.Parse(ddlProviderCreateOrder.SelectedValue));


        }

        protected void txtSalesRepSentCustomerNo_TextChanged(object sender, EventArgs e)
        {
            // CheckStoreIDSearchIfExistingSalesRep(txtSalesRepSentCustomerNo.Text, long.Parse(ddlProviderSentOrderSearch.SelectedValue));

        }

        protected void txtAllSentCustomerNo_TextChanged(object sender, EventArgs e)
        {
            // CheckStoreIDSearchIfExistingAll(txtAllSentCustomerNo.Text, long.Parse(ddlProviderAllSent.SelectedValue));
        }

        protected void txtCreatedBySearch_TextChanged(object sender, EventArgs e)
        {
            CheckifUsernameExist(txtCreatedBySearch.Text, "Office");
        }

        // txtCreatedBySearchOfficeNewOrder_TextChanged
        protected void txtCreatedBySearchOfficeNewOrder_TextChanged(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated Orders event
            // CheckifUsernameExist(txtCreatedBySearch.Text, "Office");
        }

        // txtCreatedBySearchSalesRepNewOrder_TextChanged
        protected void txtCreatedBySearchSalesRepNewOrder_TextChanged(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated Orders event
            // CheckifUsernameExist(txtCreatedBySearch.Text, "Office");
        }

        // txtCreatedBySearchAllNewOrder_TextChanged
        protected void txtCreatedBySearchAllNewOrder_TextChanged(object sender, EventArgs e)
        {
            // Pre-sell and Future-dated Orders event
            //CheckifUsernameExist(txtCreatedBySearch.Text, "Office");
        }


        protected void txtSalesRepSentCreatedBy_TextChanged(object sender, EventArgs e)
        {
            CheckifUsernameExist(txtSalesRepSentCreatedBy.Text, "SalesRep");
        }

        protected void txtAllSentCreatedBy_TextChanged(object sender, EventArgs e)
        {

            CheckifUsernameExist(txtAllSentCreatedBy.Text, "All");
        }

        protected void btnBackOfficeOrder_Click(object sender, EventArgs e)
        {
            SetViewBasedOnSelectedNodeNewOrders(Session["NodeReturn"].ToString().Replace("NewOrders", ""));
        }

        protected void tvNewOrders_PreRender(object sender, EventArgs e)
        {
            FillNewOrderTreeViews();
        }

        protected void tvSentOrders_PreRender(object sender, EventArgs e)
        {
            FillSentOrderTreeViews();
        }

        protected void gvCustomerSelect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int customerID = int.Parse(((Label)(e.Row.FindControl("lblCustomerIDSearch"))).Text);

                if ((Session["NodeReturn"].ToString() == "OfficeSentOrders" && ddlProviderOffice.SelectedValue == "0") || (Session["NodeReturn"].ToString() == "Sales RepSentOrders" && ddlProviderSentOrderSearch.SelectedValue == "0") || (Session["NodeReturn"].ToString() == "AllSentOrders" && ddlProviderAllSent.SelectedValue == "0"))
                {

                    DTOProviderCustomerList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderCustomerListbyCustomerID(customerID);

                    StringBuilder CustomerCode1 = new StringBuilder();

                    int counter = 0;

                    String[] items = new String[mDTO.Count()];

                    for (int i = 0; i < mDTO.Count(); i++)
                    {
                        if (counter < 3)
                        {
                            if (HttpUtility.HtmlDecode(mDTO[i].ProviderCustomerCode).Trim().Replace(" ", "") != "" && !items.Contains(HttpUtility.HtmlDecode(mDTO[i].ProviderCustomerCode)))
                            {
                                if (counter != 0)
                                {
                                    CustomerCode1.Append(", ");
                                }

                                items[counter] = HttpUtility.HtmlDecode(mDTO[i].ProviderCustomerCode);


                                CustomerCode1.Append(HttpUtility.HtmlDecode(mDTO[i].ProviderCustomerCode));


                                if (counter == 2)
                                {
                                    CustomerCode1.Append(" ...more");
                                }
                                counter++;
                            }
                        }
                    }

                    e.Row.Cells[2].Text = CustomerCode1.ToString();
                }

                string CustomerCode = HttpUtility.HtmlDecode(e.Row.Cells[2].Text);
                if (CustomerCode != null && CustomerCode.Trim() != "")
                {

                    ((LinkButton)e.Row.FindControl("lnkbtnSearchCustomer")).Visible = true;
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkbtnSearchCustomer")).Visible = false;

                }

                //if (Session["NodeReturn"].ToString() == "OfficeNewOrders") this.PopulateOfficeNewOrders();
                //if (Session["NodeReturn"].ToString() == "Sales RepNewOrders") this.PopulateSalesRepNewOrders();
                //if (Session["NodeReturn"].ToString() == "AllNewOrders") this.PopulateAllNewOrders();

            }

            
        }

        protected void gvOfficeNewOrders_RowDataBound(object sender, GridViewRowEventArgs e) // Format date from sys config table
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;
                    DateTime? mReleaseDate = ((DTOOrder)(e.Row.DataItem)).RequestedReleaseDate;

                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);
                    //e.Row.Cells[8].Text = String.Format(dateformat, mReleaseDate);

                    if (bool.Parse(e.Row.Cells[5].Text) == true)
                    {
                        e.Row.Cells[5].Text = "Regular";
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "Pre-sell";
                    }

                }
                catch(Exception ex)
                {

                }
            }            
        }

        protected void gvSalesRepNewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {

                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;
                    
                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate); // Order Date

                    if (bool.Parse(e.Row.Cells[5].Text) == true)
                    {
                        e.Row.Cells[5].Text = "Regular";
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "Pre-sell";
                    }

                }
                catch
                {

                }
            }
        }

        protected void gvAllNewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;

                    e.Row.Cells[5].Text = bool.Parse(e.Row.Cells[5].Text) ? "Regular" : "Pre-sell";

                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);

                }
                catch
                {

                }
            }
        }

        protected void gvOfficeSentOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;
                    DateTime mReleaseDate = (DateTime)((DTOOrder)(e.Row.DataItem)).ReleaseDate;
                    bool? IsRegularOrder = ((DTOOrder)(e.Row.DataItem)).IsRegularOrder;

                    // For Testing : Remove when DONE
                    // e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);
                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);
                    e.Row.Cells[7].Text = String.Format(dateformat, mReleaseDate);
                    
                    // FOR TESTING - Remove if done
                    if ((bool)IsRegularOrder)
                    {
                        e.Row.Cells[5].Text = "Regular";
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "Pre-sell";
                    }

                }
                catch
                {

                }
            }
        }

        protected void gvSalesRepSentOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;

                    e.Row.Cells[5].Text = (bool)((DTOOrder)(e.Row.DataItem)).IsRegularOrder ? "Regular" : "Pre-sell";
                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);
                }
                catch
                {

                }
            }
        }

        protected void gvAllSentOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string dateformat = "{0:" + GlobalVariables.GetDateFormat + "}";                    
                    DateTime mOrderDate = ((DTOOrder)(e.Row.DataItem)).OrderDate;

                    e.Row.Cells[5].Text = ((bool)((DTOOrder)(e.Row.DataItem)).IsRegularOrder) ? "Regular" : "Pre-sell";
                    e.Row.Cells[6].Text = String.Format(dateformat, mOrderDate);
                }
                catch
                {

                }
            }
        }

        public Boolean setDiscountFieldVisibility()
        {
            // Supply some codes here:
            // Read tblConfig
            // If Key = ReportOnDiscount = True, then sslesOrgID

            return true;
        }


        protected void ddlSearchProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetProductPerGroupOfficeSentOrders();

            this.ddlPagesGTINOfficeSentOrder.Items.Clear();
            for (int i = 1; i <= int.Parse(this.lblGTINOfficeSentOrderPages.Text); i++)
            {
                this.ddlPagesGTINOfficeSentOrder.Items.Add(i.ToString());               
            }

            //this.gvProductPerGroupSearch.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(
            //    int.Parse(this.ddlSearchProductGroup.SelectedValue),
            //    0L,
            //    1,
            //    10);

            //this.gvProductPerGroupSearch.DataBind();
            this.mpeSearchProduct.Show();

            if (this.MultiView1.GetActiveView().ID == "OfficeNewOrders")
            {
                this.RerenderGridData(this.gvOfficeNewOrders);
            }
            else if (this.MultiView1.GetActiveView().ID == "SalesRepViewNO")
            {
                this.RerenderGridData(this.gvSalesRepNewOrders);                
            }
            else if (this.MultiView1.GetActiveView().ID == "AllNewOrders")
            {
                this.RerenderGridData(this.gvAllNewOrders);
            }



        }

        //protected void ddlProviderSentOfficeOrders_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.gvProductPerGroupSearch.DataSource = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(
        //        int.Parse(this.ddlSearchProductGroup.SelectedValue),
        //        long.Parse(this.ddlProviderSentOfficeOrders.SelectedValue), 
        //        1, 
        //        10);

        //    this.gvProductPerGroupSearch.DataBind();
        //    this.mpeSearchProduct.Show();
        //}


        protected void rangeValidator_Init(object sender, EventArgs e)
        {
            ((RangeValidator)sender).MaximumValue = DateTime.Now.Year.ToString();
        }

        public DateTime getHighestStartDate(long _OrderID)
        {
            // Get order details
            DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(_OrderID);

            // Get order line details
            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_OrderID);

            DateTime highestProductStartDate = DateTime.MinValue;// DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            // Loop thru the OrderLineList
            foreach (DTOOrderLine _orderLine in respOrderLineDetails)
            {
                // For each OrderLineList, get the ProductID and Get the ProviderID
                DTOProviderProduct _product =
                GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(respOrderDetails.ProviderID, _orderLine.ProductID);

                // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                if (_product.StartDate > highestProductStartDate) highestProductStartDate = _product.StartDate;

            }
            return highestProductStartDate;
        }

        public DateTime HighestProductStartDate(DTOOrder _order, out string _productDescription)
        {
            DateTime highestProductStartDate = DateTime.MinValue;

            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_order.OrderID);

            _productDescription = "";
            // Loop thru the OrderLineList
            foreach (DTOOrderLine _orderLine in respOrderLineDetails)
            {
                // For each OrderLineList, get the ProductID and Get the ProviderID
                DTOProviderProduct _product =
                GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(_order.ProviderID, _orderLine.ProductID);

                // Get the StartDate of that product, and compare it to the rest. Get the maximum Start Date
                if (_product.StartDate > highestProductStartDate)
                {
                    _productDescription = _orderLine.ProductName;
                    highestProductStartDate = _product.StartDate;
                }

            }

            return highestProductStartDate;

        }

        public DateTime LowestProductEndDate(DTOOrder _order, out string _productDescription)
        {
            DateTime lowestProductEndDate = DateTime.ParseExact("09/09/9999", "dd/MM/yyyy", null);

            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(_order.OrderID);

            _productDescription = "";
            // Loop thru the OrderLineList
            foreach (DTOOrderLine _orderLine in respOrderLineDetails)
            {
                // For each OrderLineList, get the ProductID and Get the ProviderID
                DTOProviderProduct _product =
                GlobalVariables.OrderAppLib.ProviderService.ProviderProductListByProductID(_order.ProviderID, _orderLine.ProductID);

                // Get the EndDate of that product, and compare it to the rest. Get the minimum End Date
                if (_product.EndDate < lowestProductEndDate)
                {
                    _productDescription = _orderLine.ProductName;
                    lowestProductEndDate = _product.EndDate;
                }

            }

            return lowestProductEndDate;

        }

        private void InitiateCreateOrder()
        {
            hidOrderID.Value = "";
            SelectionChange = true;
            ddlProviderCreateOrder.Enabled = true;
            txtCustomerCreateOrder.Style.Add("border-color", "Gray");
            string str = txtCustomerCreateOrder.Style["border-color"].ToString();
            //ClearCreateOrderFields();
            FillProviderDropDownList();

            txtOrderDateCreateOrder.Text = txtOrderDateCreateOrder.Text = DateTime.Now.ToString("dd/MM/yyyy");
            FillProviderWarehouseDropDownList(int.Parse(ddlProviderCreateOrder.SelectedValue));

            this.ddlOrderType.Enabled = true;
            this.ddlOrderType.SelectedIndex = 0; // Regular
            this.txtCreateOrderReleaseDate.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now);// DateTime.ParseExact(DateTime.Now.ToShortDateString(), "dd/MM/yyyy", null).ToShortDateString();
            this.txtCreateOrderReleaseDate.Enabled = true;

            try
            {
                var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListByID(int.Parse(Session["AccountID"].ToString()));
                DTOAccountList mList = new DTOAccountList();
                mList.Add(respSalesRep);
                ddlSalesRepCreateOrder.DataTextField = "FullName";
                ddlSalesRepCreateOrder.DataValueField = "AccountID";
                ddlSalesRepCreateOrder.DataSource = mList;
                ddlSalesRepCreateOrder.DataBind();
            }
            catch
            {

            }
            MultiView1.SetActiveView(CreateOrder);

        }

        public DTOOrderList GetReceivedAndCreatedOrders(DTOOrderList mOrderList)
        {
            DTOOrderList tmpFilteredOrders = new DTOOrderList();

            foreach (DTOOrder _order in mOrderList)
            {
                // Get Regular Orders
                if (_order.IsRegularOrder == true || _order.IsRegularOrder == null)
                {
                    if ((_order.SYSOrderStatusID == 100 || _order.SYSOrderStatusID == 101) && (_order.RequestedReleaseDate < DateTime.Now || _order.RequestedReleaseDate == null))
                    {
                        tmpFilteredOrders.Add(_order);
                    }
                }
                // Get Pre-sell Orders
                else if (_order.IsRegularOrder == false)
                {
                    string prodDesc = "";
                    if (this.HighestProductStartDate(_order, out prodDesc) < DateTime.Now.Date && (_order.SYSOrderStatusID == 100 || _order.SYSOrderStatusID == 101))
                    {
                        tmpFilteredOrders.Add(_order);
                    }
                }
            }

            return tmpFilteredOrders;
        }

        private void ExportToCSVFile(DataTable mDT, string mFilename)
        {
            try
            {
                StringBuilder sbldr = new StringBuilder();
                string mOutput = "";
                if (mDT.Columns.Count != 0)
                {
                    // Get the columns
                    foreach (DataColumn col in mDT.Columns)
                    {
                        sbldr.Append(col.ColumnName + ',');
                    }

                    sbldr.Append("\r\n");

                    foreach (DataColumn col in mDT.Columns)
                    {

                        sbldr.Append(new string('-', col.ColumnName.Length) + ',');
                    }


                    sbldr.Append("\r\n");
                    foreach (DataRow row in mDT.Rows)
                    {
                        foreach (DataColumn column in mDT.Columns)
                        {
                            if (column.ToString() == "ProjectedLossVSNational")
                            {

                                sbldr.Append((string.Format("{0:P}", double.Parse(row[column].ToString()))) + ',');
                            }
                            else
                            {
                                sbldr.Append((row[column].ToString().Replace(",", "")) + ',');
                            }
                        }
                        sbldr.Append("\r\n");
                    }
                }

                mOutput = sbldr.ToString();
                Response.Clear();
                Response.ContentType = "application/CSV";
                Response.AddHeader("content-disposition", "attachment;filename=" + mFilename.Replace(" ", "_") + ".csv");
                Response.Write(mOutput);
                Response.End();

            }

            catch (Exception ex)
            {

            }
        }

        public DataTable ToDataTable<T>(IList<T> data)// T is any generic type
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public void WriteToLog(string _Source, string _Message, EventLogEntryType _EventType)
        {

            if (!EventLog.SourceExists(_Source))
            {
                EventLog.CreateEventSource(_Source, "Application");
            }

            EventLog myLog = new EventLog();
            myLog.Source = _Source;
            myLog.WriteEntry(_Message, _EventType);
        }
    }
}