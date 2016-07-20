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

namespace OrderApplication
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        #region #MEMBERS#
        //  public static DataTable OrderLineDataTable = GetOrderLineTable();
        public Boolean SelectionChange = false;
        public static ArrayList OrderLineIDList = new ArrayList();

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
            this.Title = "Manage Orders";
            if (IsPostBack)
            {

            }
            else
            {


                //  Session["MyTempOrderLineTable"] = GetOrderLineTable();
                txtOrderDateCreateOrder.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateStateDropdown();

                MultiView1.SetActiveView(InitialLoadPage);

                gvOrderLineCreateOrder.DataSource = null;
                gvOrderLineCreateOrder.DataBind();

                gvCustomerSelect.DataSource = null;
                gvCustomerSelect.DataBind();

                gvCreatedByUserIDs.DataSource = null;
                gvCreatedByUserIDs.DataBind();


                FillNewOrderTreeViews();
                FillSentOrderTreeViews();
                FillProviderDropDownList();
                int mProviderId = 0;
                //int.TryParse(ddlProviderCreateOrder.SelectedValue.ToString(), out mProviderId);
                //FillProviderWarehouseDropDownList(mProviderId);

                FillProductGroupDropDownList();
                int mProductGroupID = 0;
                int.TryParse(ddlGroupCreateOrder.SelectedValue.ToString(), out mProductGroupID);
                FillProductsByGroupID(mProductGroupID);

                FillProvider(int.Parse(Session["RefID"].ToString()));

                OfficeNewOrdersPagingPanel.Visible = false;

                Session["NodeReturn"] = "OfficeNewOrders";
                SetViewBasedOnSelectedNodeNewOrders("Office");

            }
        }


        private void FillProvider(int SalesOrgID)
        {

            DTOProviderList mDTO = GlobalVariables.OrderAppLib.ProviderService.ProviderListBySalesOrgID(SalesOrgID);

            DTOProvider mProvider = new DTOProvider();

            mProvider.ProviderID = 0;
            mProvider.ProviderName = "<All Provider>";

            mDTO.Insert(0, mProvider);

            ddlProviderOffice.DataSource = mDTO;
            ddlProviderOffice.DataBind();



            ddlProviderAllSent.DataSource = mDTO;
            ddlProviderAllSent.DataBind();

            ddlProviderSentOrderSearch.DataSource = mDTO;
            ddlProviderSentOrderSearch.DataBind();


        }

        private void PopulateStateDropdown()
        {
            var respState = GlobalVariables.OrderAppLib.AddressService.SYSStateList();

            ddlStateForCustomerView.DataSource = respState;
            ddlStateForCustomerView.DataBind();

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
            FillProductsByGroupID(int.Parse(ddlGroupCreateOrder.SelectedValue));
            mpeOrderLine.Show();
        }

        /// <summary>
        /// Populate Product Group DropDownList on Add Orderline Pop up 
        /// </summary>
        private void FillProductGroupDropDownList()
        {
            SelectionChange = true;
            var respProductGroup = GlobalVariables.OrderAppLib.CatalogService.ProductGroupListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

            ddlGroupCreateOrder.DataSource = respProductGroup;
            ddlGroupCreateOrder.DataBind();

        }

        /// <summary>
        /// Populate Products based on their Product GroupID
        /// </summary>
        /// <param name="ProductGroupID"></param>
        private void FillProductsByGroupID(int ProductGroupID)
        {
            SelectionChange = true;
            var respProducts = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductGroupID(ProductGroupID, long.Parse(ddlProviderCreateOrder.SelectedValue), 1, 100000);

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

            return table;
        }


        /// <summary>
        /// Changing of Quantity on the Product Orderline Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    if (long.Parse(((TextBox)(row.FindControl("txtQtyOderLine"))).Text) != 0)
                    {
                        // ((Label)row.FindControl("lblProductID")).Text

                        int ProductID = 0;

                        int.TryParse(((Label)(row.FindControl("lblProductID"))).Text, out ProductID);
                        if (CheckIfProductIDIsExistingOnOrderLineTable(ProductID, int.Parse(((TextBox)(row.FindControl("txtQtyOderLine"))).Text)))
                        {
                            InsertProductForOrderLine(row);
                        }
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
            DataTable MytempTable = (DataTable)Session[hidOrderLineTempID.Value];

            foreach (DataRow mrow in MytempTable.Rows)
            {
                if (int.Parse(mrow["ProductID"].ToString()) == ProductID)
                {
                    mrow["OrderQty"] = int.Parse(mrow["OrderQty"].ToString()) + OrderQty;
                    response = false;
                    break;
                }
                else
                {
                    response = true;
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
            SelectionChange = true;
            DataTable MyTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];

            DataRow Row = MyTempOrderLineTable.NewRow();

            long ProductID = int.Parse(((Label)mRow.FindControl("lblProductID")).Text);
            float OrderQty = int.Parse(((TextBox)(mRow.FindControl("txtQtyOderLine"))).Text);
            string ProductDescription = HttpUtility.HtmlDecode(mRow.Cells[2].Text);
            string ProductCode = HttpUtility.HtmlDecode(mRow.Cells[1].Text);

            MyTempOrderLineTable.Rows.Add(0, 1, ProductID, OrderQty, 1, "NA", 1, 1, "NA", "NA", ProductDescription, ProductCode);
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

            gvOrderLineCreateOrder.DataSource = (DataTable)Session[hidOrderLineTempID.Value];
            gvOrderLineCreateOrder.DataBind();

        }


        /// <summary>
        /// Populate Sales Rep Dropdownlist
        /// </summary>
        private void PopulateSalesRepDropDownList()
        {
            SelectionChange = true;
            var respSalesRep = GlobalVariables.OrderAppLib.AccountService.AccountListByAccountTypeID(4);

            ddlSalesRepCreateOrder.DataTextField = "Fullname";
            ddlSalesRepCreateOrder.DataValueField = "AccountID";
            ddlSalesRepCreateOrder.DataSource = respSalesRep;
            ddlSalesRepCreateOrder.DataBind();


            ddlSalesRepViewOrders.DataTextField = "Fullname";
            ddlSalesRepViewOrders.DataValueField = "AccountID";
            ddlSalesRepViewOrders.DataSource = respSalesRep;
            ddlSalesRepViewOrders.DataBind();
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
            if (respCustomer.CustomerCode != null)
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

            DataTable myTempOrderLinetable = (DataTable)Session[hidOrderLineTempID.Value];

            if (myTempOrderLinetable.Rows.Count != 0)
            {
                if (txtOrderID.Text == "")
                {
                    if (txtCustomerCreateOrder.Style["border-color"].ToString() == "Gray")
                    {
                        if (CreateOrderRecord())
                        {
                            FillNewOrderTreeViews();
                            FillSentOrderTreeViews();
                            PopulateOfficeNewOrders();
                            MultiView1.SetActiveView(OfficeNewOrders);
                        }
                    }

                }
                else
                {
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(int.Parse(txtOrderID.Text));
                    DeleteOrderLines();
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


        /// <summary>
        /// Populate Gridview of Office New Orders
        /// </summary>
        private void PopulateOfficeNewOrders()
        {
            SelectionChange = true;

            var respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()));

            if (respOfficeNewOrders.Count != 0)
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

        /// <summary>
        /// Populate Gridview of Sales Rep New Orders
        /// </summary>
        private void PopulateSalesRepNewOrders()
        {
            var respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()));

            if (respSalesRepNewOrders.Count != 0)
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
            var respAllNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()));

            if (respAllNewOrders.Count != 0)
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
            var respOfficeSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderOffice.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            if (respOfficeSentOrders.Count != 0)
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
            var respSalesRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderSentOrderSearch.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            if (respSalesRepSentOrders.Count != 0)
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
            var respAllSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderAllSent.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), 1, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            if (respAllSentOrders.Count != 0)
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
                        if (mRow.Cells[5].Text == "Released")
                        {
                            gvOfficeNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnRelease").Visible = false;
                            gvOfficeNewOrders.Rows[mRow.RowIndex].FindControl("imgbtnDeleteOrder").Visible = false;
                        }

                    }
                    break;

                case "SalesRepNewOrders":
                    foreach (GridViewRow mRow in gvSalesRepNewOrders.Rows)
                    {
                        if (mRow.Cells[5].Text == "Released")
                        {
                            gvSalesRepNewOrders.Rows[mRow.RowIndex].FindControl("imgBtnReleaseSalesRep").Visible = false;
                            gvSalesRepNewOrders.Rows[mRow.RowIndex].FindControl("imgbtnDeleteOrder").Visible = false;
                        }

                    }
                    break;

                case "AllNewOrders":
                    foreach (GridViewRow mRow in gvAllNewOrders.Rows)
                    {
                        if (mRow.Cells[5].Text == "Released")
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
            SelectionChange = true;
            txtCustomerCreateOrder.Style.Add("border-color", "Gray");
            string str = txtCustomerCreateOrder.Style["border-color"].ToString();
            ClearCreateOrderFields();
            txtOrderDateCreateOrder.Text = txtOrderDateCreateOrder.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

            SelectionChange = true;
            string NodeName = tvNewOrders.SelectedNode.Value;
            Session["NodeReturn"] = NodeName + "NewOrders";
            SetViewBasedOnSelectedNodeNewOrders(NodeName);
            tvNewOrders.SelectedNode.Selected = false;

        }

        /// <summary>
        /// Tree View Sent Orders Selected Node Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvSentOrders_SelectedNodeChanged(object sender, EventArgs e)
        {

            SelectionChange = true;
            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            string NodeName = tvSentOrders.SelectedNode.Value;
            Session["NodeReturn"] = NodeName + "SentOrders";
            SetViewBasedOnSelectedNodeSentOrders(NodeName);
            tvSentOrders.SelectedNode.Selected = false;

        }

        /// <summary>
        /// Changing of Views based on Selected Node for New Orders
        /// </summary>
        private void SetViewBasedOnSelectedNodeNewOrders(string NodeName)
        {
            SelectionChange = true;
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
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    // UpdateOrderRecord(respOrderDetails, false, 102);
                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateSalesRepNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvSalesRepNewOrders.Rows[RowIndex].Cells[5].Text;
                    DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    FillProviderWarehouseDropDownList(int.Parse(respOrderDetailsView.ProviderID.ToString()));
                    txtCustomerIDCreateOrder.Text = respOrderDetailsView.CustomerID.ToString();
                    txtOrderID.Text = OrderID.ToString();

                    if (OrderStatus == "Released")
                    {
                        PopulateSalesRepDropDownList();
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        PopulateSalesRepDropDownList();
                        FillOrderDetailsForEdit(OrderID);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    break;
                case "Nothing":
                    OrderID = int.Parse(((Label)(gvSalesRepNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderNoForDeletion.Text = OrderID.ToString();
                    txtOrderID.ToolTip = "SalesRep";
                    txtOrderID.Text = OrderID.ToString();
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
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvOfficeNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":

                    OrderID = int.Parse(((Label)(gvOfficeNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvOfficeNewOrders.Rows[RowIndex].Cells[5].Text;
                    DTOOrder respOrderDetailsView = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    txtCustomerIDCreateOrder.Text = respOrderDetailsView.CustomerID.ToString();

                    txtOrderID.Text = OrderID.ToString();

                    if (OrderStatus == "Released")
                    {
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(OrderID);
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
            switch (e.CommandName)
            {
                case "Release":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    // UpdateOrderRecord(respOrderDetails, false, 102);

                    ReleaseOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvAllNewOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();
                    PopulateSalesRepDropDownList();
                    if (OrderStatus == "Released")
                    {
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(OrderID);
                        MultiView1.SetActiveView(CreateOrder);
                    }

                    //PopulateTheFieldsForEdit
                    //View the Asp VIEW
                    break;

                case "Nothing":
                    OrderID = int.Parse(((Label)(gvAllNewOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    txtOrderID.ToolTip = "All";
                    txtOrderID.Text = OrderID.ToString();
                    txtOrderNoForDeletion.Text = OrderID.ToString();
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
                    OrderID = int.Parse(((Label)(gvOfficeSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvOfficeSentOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();
                    PopulateSalesRepDropDownList();
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(OrderID);
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
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    UpdateOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvAllSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvAllSentOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();
                    PopulateSalesRepDropDownList();
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(OrderID);
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
        private void FillOrderDetailsForEdit(int OrderID)
        {
            OrderLineIDList.Clear();
            ClearCreateOrderFields();
            DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(OrderID);
            DTOSYSState respSYSState = GlobalVariables.OrderAppLib.AddressService.SYSStateListByCustomerID(int.Parse(respOrderDetails.CustomerID.ToString()));
            DTOCustomer respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(respOrderDetails.ProviderID, int.Parse(respOrderDetails.CustomerID.ToString()));

            //OrderLineDataTable.Clear();
            FillProviderWarehouseDropDownList(int.Parse(respOrderDetails.ProviderID.ToString()));

            Session[hidOrderLineTempID.Value] = GetOrderLineTable();


            FillOrderLineTableWithViewOrderLineDetails(respOrderLineDetails);
            ddlProviderCreateOrder.SelectedValue = respOrderDetails.ProviderID.ToString();
            txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
            txtOrderNoCreateOrder.Text = respOrderDetails.OrderID.ToString();
            DateTime OrderDate = (DateTime)respOrderDetails.OrderDate;
            txtOrderDateCreateOrder.Text = OrderDate.ToString("dd/MM/yyyy");

            if (respCustomer != null)
            {
                txtCustomerCreateOrder.Text = respCustomer.CustomerCode;
                txtStoreNameCreateOrder.Text = respCustomer.CustomerName;
                txtStoreNameCreateOrder.Text = respCustomer.CustomerName.ToString();
            }

            if (respSYSState != null)
                txtStateNameCreateOrder.Text = respSYSState.StateName;


            DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
            if (DeliverDate < DateTime.Parse("01/01/2000"))
            {
                txtDeliveryDateCreateOrder.Text = "";
            }
            else
            {
                txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
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
                ddlSalesRepCreateOrder.SelectedValue = respOrderDetails.CreatedByUserID.ToString();
            }
            catch { }


            gvOrderLineCreateOrder.DataSource = (DataTable)Session[hidOrderLineTempID.Value];
            gvOrderLineCreateOrder.DataBind();

        }

        /// <summary>
        /// Populate Fields on the Order View.
        /// </summary>
        /// <param name="OrderID"></param>
        private void FillOrderDetails(int OrderID)
        {

            OrderLineIDList.Clear();
            DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
            DTOOrderLineList respOrderLineDetails = GlobalVariables.OrderAppLib.OrderService.OrderLineListByOrderID(OrderID);
            DTOSYSState respSYSState = GlobalVariables.OrderAppLib.AddressService.SYSStateListByCustomerID(int.Parse(respOrderDetails.CustomerID.ToString()));
            DTOCustomer respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByID(respOrderDetails.ProviderID, long.Parse(respOrderDetails.CustomerID.ToString()));

            //OrderLineDataTable.Clear();
            Session[hidOrderLineTempID.Value] = GetOrderLineTable();
            FillOrderLineTableWithViewOrderLineDetails(respOrderLineDetails);


            txtCustomerViewOrders.Text = respCustomer.CustomerCode;
            txtStoreNameViewOrders.Text = respCustomer.CustomerName;
            txtOrderNoViewOrders.Text = respOrderDetails.OrderID.ToString();
            try
            {
                ddlProviderViewOrders.SelectedValue = respOrderDetails.ProviderID.ToString();
            }
            catch
            {
            }
            txtStateViewOrders.Text = respSYSState.StateName;
            DateTime OrderDate = (DateTime)respOrderDetails.OrderDate;
            txtOrderDateViewOrders.Text = OrderDate.ToString("dd/MM/yyyy");
            DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
            if (DeliverDate < DateTime.Parse("01/01/2000"))
            {
                txtDeliveryDateViewOrders.Text = "";

            }
            else
            {
                txtDeliveryDateViewOrders.Text = DeliverDate.ToString("dd/MM/yyyy");
            }

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

                ddlSalesRepViewOrders.SelectedValue = respOrderDetails.CreatedByUserID.ToString();
            }
            catch { }

            gvOrderLineViewOrders.DataSource = (DataTable)Session[hidOrderLineTempID.Value];
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

                DataRow Row = myTempOrderLineTable.NewRow();
                myTempOrderLineTable.Rows.Add(OrderLine.OrderLineID, OrderLine.LineNum, OrderLine.ProductID, OrderLine.OrderQty, OrderLine.DespatchQty, OrderLine.UOM, OrderLine.OrderPrice, OrderLine.DespatchPrice, OrderLine.ItemStatus, OrderLine.ErrorText, OrderLine.ProductName, OrderLine.ProductCode);
            }

            Session[hidOrderLineTempID.Value] = myTempOrderLineTable;
        }


        /// <summary>
        /// Populate Provider Dropdownlist function.
        /// </summary>
        private void FillProviderDropDownList()
        {
            SelectionChange = true;

            var respProvider = GlobalVariables.OrderAppLib.ProviderService.ProviderList();

            ddlProviderCreateOrder.DataSource = respProvider;
            ddlProviderCreateOrder.DataBind();

            ddlProviderViewOrders.DataSource = respProvider;
            ddlProviderViewOrders.DataBind();

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
            MultiView1.SetActiveView(OfficeNewOrders);
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
            mpeCompanySearch.Show();

        }


        /// <summary>
        /// Customer Concat Search event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            SelectionChange = true;
            DTOCustomerList respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, 1, int.Parse(Session["PageSize"].ToString()));

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

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, int.Parse(ddlCustomerPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();

            mpeCompanySearch.Show();

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

            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListBySearch(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), int.Parse(ddlStateForCustomerView.SelectedValue), txtCustomerSearch.Text, int.Parse(ddlCustomerPages.SelectedValue), int.Parse(Session["PageSize"].ToString()));

            gvCustomerSelect.DataSource = respCustomer;
            gvCustomerSelect.DataBind();


            mpeCompanySearch.Show();
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

            var respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvOfficeNewOrders.DataSource = respOfficeNewOrders;
            gvOfficeNewOrders.DataBind();

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

            var respOfficeNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvOfficeNewOrders.DataSource = respOfficeNewOrders;
            gvOfficeNewOrders.DataBind();
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

            ddlSalesRepNewOrdersPages.SelectedValue = PageNumber.ToString();

            var respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()));

            gvSalesRepNewOrders.DataSource = respSalesRepNewOrders;
            gvSalesRepNewOrders.DataBind();
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

            var respSalesRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()));

            gvSalesRepNewOrders.DataSource = respSalesRepNewOrders;
            gvSalesRepNewOrders.DataBind();
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

            var respAllRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvAllNewOrders.DataSource = respAllRepNewOrders;
            gvAllNewOrders.DataBind();

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

            var respAllRepNewOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllNewOrders(int.Parse(Session["RefID"].ToString()), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()));

            gvAllNewOrders.DataSource = respAllRepNewOrders;
            gvAllNewOrders.DataBind();
            ManageReleaseButtonOnNewOrders("AllNewOrders");

        }


        /// <summary>
        /// Office Sent Orders Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OfficeSentOrdersPaging(object sender, EventArgs e)
        {
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



            var respOfficeSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderOffice.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvOfficeSentOrders.DataSource = respOfficeSentOrders;
            gvOfficeSentOrders.DataBind();

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


            var respOfficeSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_OfficeSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderOffice.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvOfficeSentOrders.DataSource = respOfficeSentOrders;
            gvOfficeSentOrders.DataBind();

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

            var respSalesRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderSentOrderSearch.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvSalesRepSentOrders.DataSource = respSalesRepSentOrders;
            gvSalesRepSentOrders.DataBind();

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

            var respSalesRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_SalesRepSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderSentOrderSearch.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvSalesRepSentOrders.DataSource = respSalesRepSentOrders;
            gvSalesRepSentOrders.DataBind();

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

            var respAllRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderAllSent.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvAllSentOrders.DataSource = respAllRepSentOrders;
            gvAllSentOrders.DataBind();

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

            var respAllRepSentOrders = GlobalVariables.OrderAppLib.OrderService.OrderListBySalesOrgID_AllSentOrders(int.Parse(Session["RefID"].ToString()), long.Parse(ddlProviderAllSent.SelectedValue), int.Parse(Session["AccountID"].ToString()), int.Parse(Session["AccountTypeID"].ToString()), PageNumber, int.Parse(Session["PageSize"].ToString()), int.Parse(Session["OrgUnit"].ToString()), int.Parse(txtOrderNoHidden.Text), int.Parse(txtCustomerIDHidden.Text), int.Parse(txtCreatedByUserIDHidden.Text), DateTime.Parse(txtDateFromHidden.Text), DateTime.Parse(txtDateToHidden.Text), int.Parse(txtStatusIDHidden.Text));

            gvAllSentOrders.DataSource = respAllRepSentOrders;
            gvAllSentOrders.DataBind();

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
                        txtCustomerCreateOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[1].Text);
                        txtStoreNameCreateOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[2].Text);
                        txtStateNameCreateOrder.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[3].Text);

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                        txtCustomerCreateOrder.Style.Add("border-color", "Gray");
                    }
                    else if (Session["SearchType"].ToString() == "Search Order")
                    {
                        txtCustomerNoSearch.Text = gvCustomerSelect.Rows[RowIndex].Cells[1].Text;
                        txtCustomerNameSearch.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[2].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                    }
                    else if (Session["SearchType"].ToString() == "Search SalesRep")
                    {
                        txtSalesRepSentCustomerNo.Text = gvCustomerSelect.Rows[RowIndex].Cells[1].Text;
                        txtSalesRepSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[2].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                    }
                    else if (Session["SearchType"].ToString() == "Search All")
                    {
                        txtAllSentCustomerNo.Text = gvCustomerSelect.Rows[RowIndex].Cells[1].Text;
                        txtAllSentCustomerName.Text = HttpUtility.HtmlDecode(gvCustomerSelect.Rows[RowIndex].Cells[2].Text);
                        txtCustomerIDSearch.Text = CustomerID.ToString();

                        txtCustomerSearch.Text = "";
                        gvCustomerSelect.DataSource = null;
                        gvCustomerSelect.DataBind();
                    }

                    mpeCompanySearch.Hide();
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

            //OrderNo
            if (txtOrderNoSearch.Text == "")
            {
                txtOrderNoHidden.Text = "0";
            }
            else
            {
                txtOrderNoHidden.Text = txtOrderNoSearch.Text;
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
            if (txtCreatedBySearch.Text == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {

            }
            //DateFrom
            if (txtDateFrom.Text == "")
            {

                txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            }
            else
            {
                DateTime fromOrderDate = DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", null);

                txtDateFromHidden.Text = fromOrderDate.ToString();
            }
            //DateTo
            if (txtDateTo.Text == "")
            {
                txtDateToHidden.Text = DateTime.Now.ToString();
            }
            else
            {
                DateTime ToOrderDate = DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", null);
                txtDateToHidden.Text = ToOrderDate.ToString();
            }
            //StatusID
            txtStatusIDHidden.Text = ddlStatusSearch.SelectedValue;

            PopulateOfficeSentOrders();

        }

        /// <summary>
        /// Image Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnCustomerSearch_Click(object sender, ImageClickEventArgs e)
        {
            Session["SearchType"] = "Search Order";
            mpeCompanySearch.Show();
        }

        /// <summary>
        /// Check Store ID Code if existing for Office Sent Order Search
        /// </summary>
        /// <param name="StoreCode"></param>B
        private void CheckStoreIDSearchIfExisting(string StoreCode)
        {
            SelectionChange = true;
            var respCustomer = GlobalVariables.OrderAppLib.CustomerService.CustomerListByCustomerCode(long.Parse(ddlProviderCreateOrder.SelectedValue), int.Parse(Session["RefID"].ToString()), txtCustomerCreateOrder.Text);
            if (respCustomer.CustomerCode != null)
            {
                txtCustomerNoSearch.Style.Add("border-color", "Gray");
                txtCustomerNameSearch.Text = respCustomer.CustomerName.ToString();
                txtCustomerIDSearch.Text = respCustomer.CustomerID.ToString();
            }
            else
            {
                txtCustomerNoSearch.Style.Add("border-color", "Red");
                txtCustomerNameSearch.Text = "";
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
            CheckStoreIDSearchIfExisting(txtCustomerNoSearch.Text);
        }


        /// <summary>
        /// Button Clear Event for Office Sent Orders Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchOfficeSentClear_Click(object sender, EventArgs e)
        {
            txtOrderNoSearch.Text = "";
            txtCustomerIDSearch.Text = "";
            txtCustomerNoSearch.Text = "";
            txtCreatedBySearch.Text = "";
            txtCustomerNameSearch.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";

            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlStatusSearch.SelectedValue = "0";

            PopulateOfficeSentOrders();
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
                    txtProductCodeChangeQty.Text = HttpUtility.HtmlDecode(gvOrderLineCreateOrder.Rows[RowIndex].Cells[1].Text);
                    txtDescriptionChangeQty.Text = HttpUtility.HtmlDecode(gvOrderLineCreateOrder.Rows[RowIndex].Cells[2].Text);
                    txtChangeQuantity.Text = gvOrderLineCreateOrder.Rows[RowIndex].Cells[3].Text;
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
                        mrow["OrderQty"] = txtChangeQuantity.Text;
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

                GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mOrder);
                PopulateSalesRepNewOrders();
                FillNewOrderTreeViews();
            }
            else
            {
                mOrder.SYSOrderStatusID = 106;

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

            DTOProduct mDTOProduct = GlobalVariables.OrderAppLib.CatalogService.ProductListByProductCode(int.Parse(Session["RefID"].ToString()), txtAddProductByProductCode.Text);
            if (mDTOProduct.ProductCode != null)
            {
                hidProductIDnew.Value = mDTOProduct.ProductID.ToString();
                txtProductIDAddProductByProductCode.Text = mDTOProduct.ProductID.ToString();
                txtProductDescriptionByProductCode.Text = mDTOProduct.ProductDescription.ToString();
            }
            else
            {
                hidProductIDnew.Value = mDTOProduct.ProductID.ToString();
                txtProductIDAddProductByProductCode.Text = mDTOProduct.ProductID.ToString();
                txtProductDescriptionByProductCode.Text = "Invalid Product Code, No Product Found";
            }
            txtAddByProductCodeQty.Focus();
            mpeAddProductByProductCode.Show();

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


            myTempOrderLineTable.Rows.Add(0, 1, ProductID, OrderQty, 1, "NA", 1, 1, "NA", "NA", ProductDescription, ProductCode);
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
            gvOrderLineCreateOrder.DataSource = (DataTable)Session[hidOrderLineTempID.Value];
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

        }


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
                case "Office":
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[0].Text;
                    txtCreatedBySearch.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    break;
                case "SalesRep":
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[0].Text;
                    txtSalesRepSentCreatedBy.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[3].Text;
                    break;
                case "All":
                    txtCreatedByUserIDHidden.Text = gvCreatedByUserIDs.Rows[RowIndex].Cells[0].Text;
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
                mpeReleaseAllOrders.Show();
            }
        }


        protected void btnReleaseAllOrdersOfficeNew_Click(object sender, EventArgs e)
        {
            if (gvOfficeNewOrders.Rows.Count != 0)
            {
                Session["ReleaseType"] = "Office";
                mpeReleaseAllOrders.Show();
            }
        }


        protected void btnReleaseAllOrdersOk_Click(object sender, EventArgs e)
        {

            switch (Session["ReleaseType"].ToString())
            {
                case "Office":
                    foreach (GridViewRow mRow in gvOfficeNewOrders.Rows)
                    {
                        if (mRow.Cells[5].Text != "Released")
                        {
                            //if (mRow.Cells[5].Text != "Held")
                            //{
                            //    int OrderID = int.Parse(mRow.Cells[1].Text);
                            //    DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                            //    DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                            //    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                            //    txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                            //    ReleaseOrderRecord(mDTOOrder, false, 102);
                            //}
                            //else
                            //{


                            string date = ((Label)(mRow.FindControl("lblHoldUntilDate"))).Text;

                            DateTime HeldDate;
                            bool result = DateTime.TryParse(date, out HeldDate);
                            if (!result)
                            {
                                HeldDate = DateTime.ParseExact(HeldDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            }

                            if (HeldDate < DateTime.Today)
                            {
                                int OrderID = int.Parse(mRow.Cells[1].Text);
                                DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                                txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                                ReleaseOrderRecord(mDTOOrder, false, 102);
                            }
                            //}
                        }
                    }
                    PopulateOfficeNewOrders();
                    break;

                case "SalesRep":
                    foreach (GridViewRow mRow in gvSalesRepNewOrders.Rows)
                    {
                        if (mRow.Cells[6].Text != "Released")
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

                            string date = ((Label)(mRow.FindControl("lblHoldUntilDateSalesRep"))).Text;

                            DateTime HeldDate;

                            bool result = DateTime.TryParse(date, out HeldDate);
                            if (date != "")
                            {
                               // HeldDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);

                                if (HeldDate <= DateTime.Today)
                                {
                                    int OrderID = int.Parse(mRow.Cells[1].Text);
                                    DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                    DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                    txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                                    // UpdateOrderRecord(mDTOOrder, false, 102);
                                    ReleaseOrderRecord(mDTOOrder, false, 102);
                                }
                            }
                            else
                            {
                                int OrderID = int.Parse(mRow.Cells[1].Text);
                                DTOOrder mDTOOrder = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                                DateTime DeliverDate = (DateTime)mDTOOrder.DeliveryDate;
                                txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                                txtCustomerIDCreateOrder.Text = mDTOOrder.CustomerID.ToString();
                                // UpdateOrderRecord(mDTOOrder, false, 102);
                                ReleaseOrderRecord(mDTOOrder, false, 102);
                            }
                        }
                    }
                    PopulateSalesRepNewOrders();
                    break;
            }

        }

        protected void gvSalesRepSentOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SelectionChange = true;
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            int OrderID = 0;
            switch (e.CommandName)
            {

                case "Release":
                    OrderID = int.Parse(((Label)(gvAllSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    DTOOrder respOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderListByID(OrderID);
                    DateTime DeliverDate = (DateTime)respOrderDetails.DeliveryDate;
                    txtDeliveryDateCreateOrder.Text = DeliverDate.ToString("dd/MM/yyyy");
                    txtCustomerIDCreateOrder.Text = respOrderDetails.CustomerID.ToString();
                    UpdateOrderRecord(respOrderDetails, false, 102);
                    PopulateOfficeNewOrders();
                    break;

                case "View":
                    OrderID = int.Parse(((Label)(gvSalesRepSentOrders.Rows[RowIndex].FindControl("lblOrderID"))).Text);
                    string OrderStatus = gvSalesRepSentOrders.Rows[RowIndex].Cells[5].Text;
                    txtOrderID.Text = OrderID.ToString();
                    PopulateSalesRepDropDownList();
                    if (OrderStatus == "Acknowledged" || OrderStatus == "Sent")
                    {
                        FillOrderDetails(OrderID);
                        MultiView1.SetActiveView(ViewOrders);
                    }
                    else
                    {
                        FillOrderDetailsForEdit(OrderID);
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
            mpeCompanySearch.Show();
        }


        protected void btnSalesRepSentOrdersClear_Click(object sender, EventArgs e)
        {
            txtSalesRepSentOrderNo.Text = "";
            txtCustomerIDSearch.Text = "";
            txtSalesRepSentCustomerNo.Text = "";
            txtSalesRepSentCreatedBy.Text = "";
            txtSalesRepSentCustomerName.Text = "";
            txtSalesRepSentDateFrom.Text = "";
            txtSalesRepSentDateTo.Text = "";

            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderSentOrderSearch.SelectedValue = "0";
            //PopulateOfficeSentOrders();
        }


        protected void btnAllSentOrdersClear_Click(object sender, EventArgs e)
        {

            txtAllSentOrder.Text = "";
            txtCustomerIDSearch.Text = "";
            txtAllSentCustomerNo.Text = "";
            txtAllSentCreatedBy.Text = "";
            txtAllSentCustomerName.Text = "";
            txtAllSentDateFrom.Text = "";
            txtAllSentDateTo.Text = "";

            txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            txtDateToHidden.Text = DateTime.Now.ToString();
            txtOrderNoHidden.Text = "0";
            txtCreatedByUserIDHidden.Text = "0";
            txtCustomerIDHidden.Text = "0";
            txtStatusIDHidden.Text = "0";
            ddlSalesRepSent.SelectedValue = "0";
            ddlProviderAllSent.SelectedValue = "0";
        }

        protected void btnSalesRepSentOrdersSearch_Click(object sender, EventArgs e)
        {
            //OrderNo
            if (txtSalesRepSentOrderNo.Text == "")
            {
                txtOrderNoHidden.Text = "0";
            }
            else
            {
                txtOrderNoHidden.Text = txtSalesRepSentOrderNo.Text;
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
            if (txtSalesRepSentCreatedBy.Text == "")
            {
                txtCreatedByUserIDHidden.Text = "0";
            }
            else
            {

            }
            //DateFrom
            if (txtSalesRepSentDateFrom.Text == "")
            {

                txtDateFromHidden.Text = DateTime.Now.AddYears(-1).ToString();
            }
            else
            {
                DateTime fromOrderDate = DateTime.ParseExact(txtSalesRepSentDateFrom.Text, "dd/MM/yyyy", null);

                txtDateFromHidden.Text = fromOrderDate.ToString();
            }
            //DateTo
            if (txtSalesRepSentDateTo.Text == "")
            {
                txtDateToHidden.Text = DateTime.Now.ToString();
            }
            else
            {
                DateTime ToOrderDate = DateTime.ParseExact(txtSalesRepSentDateTo.Text, "dd/MM/yyyy", null);
                txtDateToHidden.Text = ToOrderDate.ToString();
            }
            //StatusID
            txtStatusIDHidden.Text = ddlSalesRepSent.SelectedValue;

            PopulateSalesRepSentOrders();

        }

        protected void btnAllSentOrdersSearch_Click(object sender, EventArgs e)
        {
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
                DateTime ToOrderDate = DateTime.ParseExact(txtAllSentDateTo.Text, "dd/MM/yyyy", null);
                txtDateToHidden.Text = ToOrderDate.ToString();
            }
            //StatusID
            txtStatusIDHidden.Text = ddlAllSentOrders.SelectedValue;

            PopulateAllSentOrders();

        }




        #region ************************tblOrderLine CRUDS ******************************************

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        //private void ShowRecord(DTOOrderLine mDTO)
        //{
        //    txtOrderLineID.Text = mDTO.OrderLineID.ToString();
        //    txtOrderID.Text = mDTO.OrderID.ToString();
        //    txtLineNum.Text = mDTO.LineNum.ToString();
        //    txtProductID.Text = mDTO.ProductID.ToString();
        //    txtOrderQty.Text = mDTO.OrderQty.ToString();
        //    txtDespatchQty.Text = mDTO.DespatchQty.ToString();
        //    txtUOM.Text = mDTO.UOM.ToString();
        //    txtOrderPrice.Text = mDTO.OrderPrice.ToString();
        //    txtDespatchPrice.Text = mDTO.DespatchPrice.ToString();
        //    txtItemStatus.Text = mDTO.ItemStatus.ToString();
        //    txtErrorText.Text = mDTO.ErrorText.ToString();
        //}

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool CreateOrderLineRecord(DataTable OrderLineDataTable)
        {
            bool response = false;

            foreach (DataRow mrow in OrderLineDataTable.Rows)
            {
                DTOOrderLine mDTO = new DTOOrderLine();
                mDTO.OrderLineID = 0;
                mDTO.OrderID = long.Parse(txtOrderID.Text);
                mDTO.LineNum = (int)mrow["LineNum"];
                mDTO.ProductID = (long)mrow["ProductID"];
                mDTO.OrderQty = (float)mrow["OrderQty"];
                mDTO.DespatchQty = (float)mrow["DespatchQty"];
                mDTO.UOM = mrow["UOM"].ToString();
                mDTO.OrderPrice = (float)mrow["OrderPrice"];
                mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                mDTO.ErrorText = mrow["ErrorText"].ToString();

                //NOTE: devs need to create a global variable to represent the Service Class. 
                //      1. Create a class GlobalVariables on the UI level.
                //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
                //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
                string mMessage;
                if (GlobalVariables.OrderAppLib.OrderService.OrderLineIsValid(mDTO, out mMessage) == true)
                {
                    mDTO = GlobalVariables.OrderAppLib.OrderService.OrderLineSaveRecord(mDTO);
                    this.txtOrderLineID.Text = mDTO.OrderLineID.ToString();
                    response = true;
                }
                else
                {
                    response = false;
                    //show error here
                }

            }

            return response;

        }

        //CASE Generated Code 6/17/2013 11:33:55 AM Lazy Dog 3.3.1.0
        private bool UpdateOrderLineRecord(DataTable OrderLineDataTable)
        {
            bool response = false;


            foreach (DataRow mrow in OrderLineDataTable.Rows)
            {

                if (int.Parse(mrow["OrderLineID"].ToString()) == 0)
                {
                    DTOOrderLine mDTO = new DTOOrderLine();
                    mDTO.OrderLineID = 0;
                    mDTO.OrderID = long.Parse(txtOrderID.Text);
                    mDTO.LineNum = (int)mrow["LineNum"];
                    mDTO.ProductID = (long)mrow["ProductID"];
                    mDTO.OrderQty = (float)mrow["OrderQty"];
                    mDTO.DespatchQty = (float)mrow["DespatchQty"];
                    mDTO.UOM = mrow["UOM"].ToString();
                    mDTO.OrderPrice = (float)mrow["OrderPrice"];
                    mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                    mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                    mDTO.ErrorText = mrow["ErrorText"].ToString();

                    //NOTE: devs need to create a global variable to represent the Service Class. 
                    //      1. Create a class GlobalVariables on the UI level.
                    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
                    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
                    string mMessage;
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
                    mDTO.OrderID = long.Parse(txtOrderID.Text);
                    mDTO.LineNum = (int)mrow["LineNum"];
                    mDTO.ProductID = (long)mrow["ProductID"];
                    mDTO.OrderQty = (float)mrow["OrderQty"];
                    mDTO.DespatchQty = (float)mrow["DespatchQty"];
                    mDTO.UOM = mrow["UOM"].ToString();
                    mDTO.OrderPrice = (float)mrow["OrderPrice"];
                    mDTO.DespatchPrice = (float)mrow["DespatchPrice"];
                    mDTO.ItemStatus = mrow["ItemStatus"].ToString();
                    mDTO.ErrorText = mrow["ErrorText"].ToString();


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
            mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
            mDTO.SalesRepAccountID = 99;
            mDTO.ProviderID = int.Parse(ddlProviderCreateOrder.SelectedValue);
            mDTO.ProviderWarehouseID = int.Parse(ddlProviderWarehouseCreateOrder.SelectedValue);
            DateTime newOrderDate = DateTime.Now;// DateTime.ParseExact(txtOrderDateCreateOrder.Text, "dd/MM/yyyy", null);
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

            mDTO.OrderNumber = "100o";
            mDTO.ReceivedDate = DateTime.Now;
            mDTO.ReleaseDate = DateTime.Now;
            mDTO.DateCreated = DateTime.Now;
            mDTO.DateUpdated = DateTime.Now;
            mDTO.IsSent = false;
            mDTO.IsHeld = true;
            mDTO.CreatedByUserID = Int64.Parse(Session["AccountID"].ToString());
            mDTO.UpdatedByUserID = Int64.Parse(Session["AccountID"].ToString());



            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.OrderService.OrderIsValid(mDTO, out mMessage) == true)
            {

                mDTO = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTO);
                this.txtOrderID.Text = mDTO.OrderID.ToString();
                DataTable myTempOrderLineTable = (DataTable)Session[hidOrderLineTempID.Value];
                //  CreateMessageInboundRecord(int.Parse(mDTO.OrderID.ToString()));
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
            //   DTOOrder mDTO = new DTOOrder();
            //   mDTO.OrderID = mDTOOrderDetails.OrderID;
            //   mDTO.SalesOrgID = mDTOOrderDetails.SalesOrgID;
            //   mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
            //   mDTO.SalesRepAccountID = mDTOOrderDetails.SalesRepAccountID;
            //   mDTO.ProviderID = int.Parse(ddlProviderCreateOrder.SelectedValue);
            //   mDTO.ProviderWarehouseID = int.Parse(ddlProviderWarehouseCreateOrder.SelectedValue);
            //   mDTO.OrderDate = mDTOOrderDetails.OrderDate;
            //   if (txtDeliveryDateCreateOrder.Text == "")
            //   {
            //       mDTO.DeliveryDate = null;
            //   }
            //   else
            //   {
            //       DateTime newDeliverDate = DateTime.ParseExact(txtDeliveryDateCreateOrder.Text, "dd/MM/yyyy", null);
            //       mDTO.DeliveryDate = newDeliverDate;
            //   }
            // //  mDTO.InvoiceDate = mDTOOrderDetails.InvoiceDate;
            //  // mDTO.SYSOrderStatusID = OrderStatusID;
            //  // mDTO.OrderNumber = mDTOOrderDetails.OrderNumber;
            // //  mDTO.ReceivedDate = mDTOOrderDetails.ReceivedDate;
            //   mDTO.ReleaseDate = DateTime.Now;
            // //  mDTO.DateCreated = mDTOOrderDetails.DateCreated;
            //  // mDTO.DateUpdated = DateTime.Now;
            // //  mDTO.IsSent = mDTOOrderDetails.IsSent;
            //   mDTO.IsHeld = IsHeld;
            ////   mDTO.CreatedByUserID = mDTOOrderDetails.CreatedByUserID;
            // //  mDTO.UpdatedByUserID = Int64.Parse(Session["AccountID"].ToString());


            //NOTE: devs need to create a global variable to represent the Service Class. 
            //      1. Create a class GlobalVariables on the UI level.
            //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
            //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
            string mMessage;
            if (GlobalVariables.OrderAppLib.OrderService.OrderIsValid(mDTOOrderDetails, out mMessage) == true)
            {
                mDTOOrderDetails = GlobalVariables.OrderAppLib.OrderService.OrderSaveRecord(mDTOOrderDetails);
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
            mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
            mDTO.SalesRepAccountID = mDTOOrderDetails.SalesRepAccountID;
            mDTO.ProviderID = int.Parse(ddlProviderCreateOrder.SelectedValue);
            mDTO.ProviderWarehouseID = int.Parse(ddlProviderWarehouseCreateOrder.SelectedValue);
            mDTO.OrderDate = mDTOOrderDetails.OrderDate;
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

        //private void CreateMessageInboundRecord(int OrderID)
        //{
        //    DTOMessageInbound mDTO = new DTOMessageInbound();
        //    mDTO.MessageinboundID = 0;
        //    mDTO.OrderID = OrderID;
        //    mDTO.CustomerID = int.Parse(txtCustomerIDCreateOrder.Text);
        //    mDTO.SentFlag = false;
        //    mDTO.DateSent = DateTime.Now;


        //    //NOTE: devs need to create a global variable to represent the Service Class. 
        //    //      1. Create a class GlobalVariables on the UI level.
        //    //      2. Create a STATIC global var: public static VoterLib.VoterServices gVoterServices
        //    //      3. Create a SetVariables Method to initialize the value, then call from a startup form. 
        //    string mMessage;
        //    if (GlobalVariables.OrderAppLib.OrderService.MessageInboundIsValid(mDTO, out mMessage) == true)
        //    {
        //        mDTO = GlobalVariables.OrderAppLib.OrderAppService.MessageInboundCreateRecord(mDTO);
        //        //this.txtMessageinboundID.Text = mDTO.MessageinboundID.ToString();
        //    }
        //    else
        //    {
        //        //show error here
        //    }
        //}

        #endregion ************************End of tblMessageInbounds CRUDS *********************************

        protected void gvOfficeNewOrders_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow mGrow in gvOfficeNewOrders.Rows)
            {

                //string Status = mGrow.Cells[5].Text;
                //if (Status != "Held")
                //{

                //    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                //    ((Label)(mGrow.FindControl("lblHoldUntilDate"))).Visible = false;
                //}
                //else
                //{
                DateTime HoldDate;
                DateTime.TryParse(((Label)(mGrow.FindControl("lblHoldUntilDate"))).Text, out HoldDate);
                if (HoldDate > DateTime.Today)
                {
                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDate"))).Visible = true;
                    string Holddate = String.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Label)(mGrow.FindControl("lblHoldUntilDate"))).Text));
                    ((Label)(mGrow.FindControl("lblHoldUntilDate"))).Text = Holddate;
                }
                else
                {
                    ((ImageButton)(mGrow.FindControl("imgBtnRelease"))).Visible = true;
                    ((Label)(mGrow.FindControl("lblHoldUntilDate"))).Visible = false;

                }
                //}

            }
        }

        protected void gvSalesRepNewOrders_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow mGrow in gvSalesRepNewOrders.Rows)
            {

                string Status = mGrow.Cells[6].Text;
                if (Status == "Released")
                {

                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRep"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Visible = true;
                }
                else
                {
                    DateTime HoldDate;
                     DateTime.TryParse(((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text, out HoldDate);
                    //try
                    //{
                    //    HoldDate = DateTime.ParseExact(((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text, "dd/MM/yyyy", null);
                    //}
                    //catch
                    //{

                    //}
                    if (HoldDate > DateTime.Today)
                    {
                        ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRep"))).Visible = false;
                        ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Visible = true;
                        string Holddate = String.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text));
                        ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Text = Holddate;
                    }
                    else
                    {
                        ((ImageButton)(mGrow.FindControl("imgBtnReleaseSalesRep"))).Visible = true;
                        ((Label)(mGrow.FindControl("lblHoldUntilDateSalesRep"))).Visible = false;

                    }
                }

            }

        }

        protected void gvAllNewOrders_DataBound(object sender, EventArgs e)
        {

            foreach (GridViewRow mGrow in gvAllNewOrders.Rows)
            {

                string Status = mGrow.Cells[6].Text;
                if (Status == "Released")
                {

                    ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                    ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = true;
                }
                else
                {
                    DateTime HoldDate;
                    DateTime.TryParse(((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text, out HoldDate);
                    if (HoldDate > DateTime.Today)
                    {
                        ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = false;
                        ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = true;
                        string Holddate = String.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text));
                        ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Text = Holddate;
                    }
                    else
                    {
                        ((ImageButton)(mGrow.FindControl("imgBtnReleaseAllOrders"))).Visible = true;
                        ((Label)(mGrow.FindControl("lblHoldUntilDateAll"))).Visible = false;

                    }
                }

            }

        }
    }
}