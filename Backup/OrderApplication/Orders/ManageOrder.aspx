<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ManageOrder.aspx.cs" Inherits="OrderApplication.WebForm7"
    EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Manage Order</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel5">
        <ProgressTemplate>
            <div id="blur">
            </div>
            <div id="progress">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Resources/Images/loading.gif" />
                        </td>
                        <td>
                            Loading please wait...
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
        <ProgressTemplate>
            <div id="blur">
            </div>
            <div id="progress">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Resources/Images/loading.gif" />
                        </td>
                        <td>
                            Loading please wait...
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="row-fluid">
        <div class="span2" style="border-right: 1px solid #e3e3e3; height: 500px">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <div class="row-fluid">
                        <asp:TreeView ID="tvNewOrders" runat="server" ForeColor="#000000" OnSelectedNodeChanged="tvNewOrders_SelectedNodeChanged"
                            ShowExpandCollapse="False">
                        </asp:TreeView>
                    </div>
                    <br />
                    <br />
                    <div class="row-fluid">
                        <asp:TreeView ID="tvSentOrders" runat="server" ForeColor="#000000" OnSelectedNodeChanged="tvSentOrders_SelectedNodeChanged"
                            ShowExpandCollapse="False">
                        </asp:TreeView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="span10">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="InitialLoadPage" runat="server">
                        </asp:View>
                        <asp:View ID="CreateOrder" runat="server">
                            <div class="row-fluid">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Order No:
                                                </div>
                                                <div class="span3" style="width: 72%">
                                                    <asp:TextBox ID="txtOrderNoCreateOrder" runat="server" Enabled="False"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Provider:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlProviderCreateOrder" runat="server" AutoPostBack="True"
                                                        DataTextField="ProviderName" DataValueField="ProviderID" Enabled="False">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Store ID:
                                                </div>
                                                <div class="span3" style="width: 72.5%">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="Button10">
                                                        <asp:Button ID="Button10" runat="server" Text="Button" Visible="false" />
                                                        <asp:TextBox ID="txtCustomerCreateOrder" runat="server" Width="180px" AutoPostBack="True"
                                                            Enabled="false" placeholder="Store ID" OnTextChanged="txtCustomerCreateOrder_TextChanged"
                                                            onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                        <asp:ImageButton ID="imgBtnSelectCustomer" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgBtnSelectCustomer_Click" CommandName="imageButton"
                                                            Enabled="False" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                                            ForeColor="Red" ControlToValidate="txtCustomerCreateOrder" ValidationGroup="SaveNewOrder"
                                                            ToolTip="Please input StoreID"></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    State:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtStateNameCreateOrder" runat="server" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Order Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtOrderDateCreateOrder" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOrderDateCreateOrder"
                                                        DaysModeTitleFormat="dd,MMM,yyyy">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Created By:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlSalesRepCreateOrder" runat="server" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Warehouse:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlProviderWarehouseCreateOrder" runat="server" AutoPostBack="True"
                                                        DataTextField="ProviderWarehouseName" DataValueField="ProviderWarehouseID" Enabled="False">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span5">
                                                    <asp:TextBox ID="txtStoreNameCreateOrder" runat="server" ReadOnly="True" Width="315px"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Hold Until Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtHoldUntilDate" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txtHoldUntilDate"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                    Delivery Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtDeliveryDateCreateOrder" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeliveryDateCreateOrder"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <h4>
                                                Order Line
                                            </h4>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Button ID="btnAddProductLine" runat="server" Text="Add By List" CssClass="btn btn-primary"
                                                OnClick="btnAddProductLine_Click" Visible="False" />
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Button ID="btnAddProductLineByProductCode" runat="server" Text="Add By Product Code"
                                                CssClass="btn btn-success" OnClick="btnAddProductLineByProductCode_Click" Visible="False" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSaveCreateOrder" runat="server" Text="Save" CssClass="btn btn-primary"
                                                        ValidationGroup="SaveNewOrder" OnClick="btnSaveCreateOrder_Click" Visible="False" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click"
                                                        Style="display: none" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <%-- <div class="span1">
                                </div>
                                <div class="span2">
                                </div>
                                <div class="span2">
                                </div>
                                <div class="span3 offset3">
                                </div>--%>
                            </div>
                            <div class="row-fluid" style="min-height: 350px">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvOrderLineCreateOrder" runat="server" EmptyDataText="No Line Added"
                                            GridLines="None" CssClass="table table-condensed" AutoGenerateColumns="False"
                                            OnRowCommand="gvOrderLineCreateOrder_RowCommand" OnRowDeleted="gvOrderLineCreateOrder_RowDeleted"
                                            OnRowDeleting="gvOrderLineCreateOrder_RowDeleting" DataKeyNames="ProductID">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnViewOrderLineProduct" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                            Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                                <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                                                <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" />
                                                <asp:TemplateField HeaderText="OrderLineID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderLineID" runat="server" Text='<%# Bind("OrderLineID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderLineID") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDeleteOrderlineProduct" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                            Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div>
                                &nbsp;
                            </div>
                            <div>
                                &nbsp;
                            </div>
                        </asp:View>
                        <asp:View ID="OfficeNewOrders" runat="server">
                            <div class="row-fluid" style="min-height: 520px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="Label1" runat="server" Text=" Office - New Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCreateOrder" runat="server" Text="Create Order" CssClass="btn btn-primary"
                                                OnClick="btnCreateOrder_Click" Visible="False" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnReleaseAllOrdersOfficeNew" runat="server" Text="Release All Orders"
                                                CssClass="btn btn-success" OnClick="btnReleaseAllOrdersOfficeNew_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:GridView ID="gvOfficeNewOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" OnRowCommand="gvOfficeNewOrders_RowCommand"
                                    EmptyDataText="No Records Found" OnDataBound="gvOfficeNewOrders_DataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnOfficeNewOrders" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Release">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnRelease" runat="server" ImageUrl="~/Resources/Images/upload-26.png"
                                                    Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                <asp:Label ID="lblHoldUntilDate" runat="server" Text='<%# Eval("HoldUntilDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" Visible= "false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="OfficeNewOrdersPagingPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnOfficeNewOrdersFirst" runat="server" CssClass="btn" OnClick="OfficeNewOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnOfficeNewOrdersPrev" runat="server" CssClass="btn" OnClick="OfficeNewOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label7" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlOfficeNewOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlOfficeNewOrdersPages_SelectedIndexChanged" Style="margin-top: 10px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label8" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblOfficeNewOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnOfficeNewOrdersNext" runat="server" CssClass="btn" OnClick="OfficeNewOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnOfficeNewOrdersLast" runat="server" CssClass="btn" OnClick="OfficeNewOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                        <asp:View ID="ViewOrders" runat="server">
                            <div class="row-fluid">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Order No:
                                                </div>
                                                <div class="span3" style="width: 72%">
                                                    <asp:TextBox ID="txtOrderNoViewOrders" runat="server" Enabled="False"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Provider:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlProviderViewOrders" runat="server" AutoPostBack="True" DataTextField="ProviderName"
                                                        DataValueField="ProviderID" Enabled="False">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Store ID:
                                                </div>
                                                <div class="span3" style="width: 70%">
                                                    <asp:TextBox ID="txtCustomerViewOrders" runat="server" Width="180px" AutoPostBack="True"
                                                        Enabled="False"></asp:TextBox>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                        Style="margin-top: -10px" />
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    State:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtStateViewOrders" runat="server" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    Order Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtOrderDateViewOrders" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtOrderDateViewOrders">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Sales Rep:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlSalesRepViewOrders" runat="server" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Warehouse:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlProviderWarehouseViewOrders" runat="server" AutoPostBack="True"
                                                        DataTextField="ProviderWarehouseName" DataValueField="ProviderWarehouseID" Enabled="False">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span5">
                                                    <asp:TextBox ID="txtStoreNameViewOrders" runat="server" ReadOnly="True" Width="315px"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlWarehouseViewOrders" runat="server" Enabled="false" Visible="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Hold Until Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtHoldUntildateView" runat="server" Enabled="false"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender12" runat="server" TargetControlID="txtHoldUntildateView"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Delivery Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtDeliveryDateViewOrders" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtDeliveryDateViewOrders">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid">
                                <h4>
                                    Order Line
                                </h4>
                            </div>
                            <div class="row-fluid" style="height: 350px">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvOrderLineViewOrders" runat="server" EmptyDataText="No Line Added"
                                            GridLines="None" CssClass="table table-condensed" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                                <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                                                <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid">
                                <%--  <asp:LinkButton ID="lnkButtonViewOrders" runat="server" Text="<< Back to List" OnClick="BackToListFunction">
                                
                                </asp:LinkButton>--%>
                            </div>
                        </asp:View>
                        <asp:View ID="SalesRepViewNO" runat="server">
                            <div class="row-fluid">
                                <div class="span5">
                                    <asp:Label ID="Label2" runat="server" Text=" Sales Rep - New Orders" Font-Size="20px"></asp:Label>
                                </div>
                                <div class="span2 offset4">
                                    <asp:Button ID="btnReleaseAllOrders" runat="server" Text="Release All Orders" CssClass="btn btn-success"
                                        OnClick="btnReleaseAllOrders_Click" />
                                </div>
                            </div>
                            <div class="row-fluid" style="min-height: 520px; margin-top: -30px;">
                                <br />
                                <br />
                                <asp:GridView ID="gvSalesRepNewOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvSalesRepNewOrders_RowCommand" OnDataBound="gvSalesRepNewOrders_DataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Release">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnReleaseSalesRep" runat="server" ImageUrl="~/Resources/Images/upload-26.png"
                                                    Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                <asp:Label ID="lblHoldUntilDateSalesRep" runat="server" 
                                                    Text='<%# Eval("HoldUntilDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="SalesRepNewOrdersPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnSalesRepNOFirst" runat="server" CssClass="btn" OnClick="SalesRepNewOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepNOPrev" runat="server" CssClass="btn" OnClick="SalesRepNewOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label9" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlSalesRepNewOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSalesRepNewOrdersPages_SelectedIndexChanged" Style="margin-top: 10px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label10" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblSalesRepNewOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnSalesRepNONext" runat="server" CssClass="btn" OnClick="SalesRepNewOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepNOLast" runat="server" CssClass="btn" OnClick="SalesRepNewOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                        <asp:View ID="AllNewOrders" runat="server">
                            <div class="row-fluid" style="min-height: 520px">
                                <asp:Label ID="Label3" runat="server" Text=" All - New Orders" Font-Size="20px"></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="gvAllNewOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvAllNewOrders_RowCommand" OnDataBound="gvAllNewOrders_DataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Release">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnReleaseAllOrders" runat="server" ImageUrl="~/Resources/Images/upload-26.png"
                                                    Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                <asp:Label ID="lblHoldUntilDateAll" runat="server" Text='<%# Eval("HoldUntilDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Delete" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="AllNewOrdersPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnAllNewOrdersFirst" runat="server" CssClass="btn" OnClick="AllNewOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnAllNewOrdersPrev" runat="server" CssClass="btn" OnClick="AllNewOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label11" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlAllNewOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlAllNewOrdersPages_SelectedIndexChanged" Style="margin-top: 10px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label12" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblAllNewOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnAllNewOrdersNext" runat="server" CssClass="btn" OnClick="AllNewOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnAllNewOrdersLast" runat="server" CssClass="btn" OnClick="AllNewOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                        <asp:View ID="OfficeSentOrders" runat="server">
                            <div class="row-fluid">
                                <asp:Label ID="Label4" runat="server" Text=" Office - Sent Orders" Font-Size="20px"></asp:Label>
                                <asp:Button ID="btnOfficeSentSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                    Width="120px" OnClick="btnOfficeSentSearch_Click" />
                                <asp:Button ID="btnSearchOfficeSentClear" runat="server" Text="Clear" CssClass="btn"
                                    OnClick="btnSearchOfficeSentClear_Click" />
                            </div>
                            <div class="row-fluid" style="margin-top: 10px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlProviderOffice" runat="server" Width="164px" DataTextField="ProviderName"
                                                DataValueField="ProviderID">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtOrderNoSearch" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtCustomerNoSearch" runat="server" placeholder="Customer No" Width="110px"
                                                OnTextChanged="txtCustomerNoSearch_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="imgbtnCustomerSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgbtnCustomerSearch_Click" />
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtCustomerNameSearch" runat="server" placeholder="Customer Name"
                                                Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlStatusSearch" runat="server" Width="164px">
                                                <asp:ListItem Value="0">&lt;All Status&gt;</asp:ListItem>
                                                <asp:ListItem Value="103">Sent</asp:ListItem>
                                                <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtDateFrom" runat="server" placeholder="From" Width="150px"></asp:TextBox><asp:CalendarExtender
                                                ID="CalendarExtender5" runat="server" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtDateTo" runat="server" placeholder="To" Width="150px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtCreatedBySearch" runat="server" placeholder="Created By" Width="110px"></asp:TextBox>
                                            <asp:ImageButton ID="imgbtnCreatedByUserIDSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgbtnCreatedByUserIDSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row-fluid" style="min-height: 520px">
                                <asp:GridView ID="gvOfficeSentOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvOfficeSentOrders_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnViewOfficeSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Release Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="OfficeSentOrdersPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnOfficeSentOrdersFirst" runat="server" CssClass="btn" OnClick="OfficeSentOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnOfficeSentOrdersPrev" runat="server" CssClass="btn" OnClick="OfficeSentOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label13" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlOfficeSentOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlOfficeSentOrdersPages_SelectedIndexChanged" Style="margin-top: 10px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label14" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblOfficeSentOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnOfficeSentOrdersNext" runat="server" CssClass="btn" OnClick="OfficeSentOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnOfficeSentOrdersLast" runat="server" CssClass="btn" OnClick="OfficeSentOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                        <asp:View ID="SalesRepSentOrders" runat="server">
                            <div class="row-fluid">
                                <asp:Label ID="Label5" runat="server" Text=" Sales Rep - Sent Orders" Font-Size="20px"></asp:Label>
                                <asp:Button ID="btnSalesRepSentOrdersSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                    Width="120px" OnClick="btnSalesRepSentOrdersSearch_Click" />
                                <asp:Button ID="btnSalesRepSentOrdersClear" runat="server" Text="Clear" CssClass="btn"
                                    OnClick="btnSalesRepSentOrdersClear_Click" />
                            </div>
                            <div class="row-fluid" style="margin-top: 10px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlProviderSentOrderSearch" runat="server" Width="164px" DataTextField="ProviderName"
                                                DataValueField="ProviderID">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtSalesRepSentOrderNo" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtSalesRepSentCustomerNo" runat="server" placeholder="Customer No"
                                                Width="110px"></asp:TextBox>
                                            <asp:ImageButton ID="imgSalesRepSentCustomerNo" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgSalesRepSentCustomerNo_Click" />
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtSalesRepSentCustomerName" runat="server" placeholder="Customer Name"
                                                Width="300px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlSalesRepSent" runat="server" Width="164px">
                                                <asp:ListItem Value="0">&lt;All Status&gt;</asp:ListItem>
                                                <asp:ListItem Value="103">Sent</asp:ListItem>
                                                <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtSalesRepSentDateFrom" runat="server" placeholder="From" Width="150px"></asp:TextBox><asp:CalendarExtender
                                                ID="CalendarExtender7" runat="server" TargetControlID="txtSalesRepSentDateFrom"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtSalesRepSentDateTo" runat="server" placeholder="To" Width="150px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtSalesRepSentDateTo"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtSalesRepSentCreatedBy" runat="server" placeholder="Created By"
                                                Width="110px"></asp:TextBox>
                                            <asp:ImageButton ID="imgSalesRepSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgSalesRepSentCreatedBy_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row-fluid" style="min-height: 520px">
                                <asp:GridView ID="gvSalesRepSentOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvSalesRepSentOrders_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnViewSalesRepSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Release Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="SalesRepSentOrdersPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnSalesRepNewOrdersFirst" runat="server" CssClass="btn" OnClick="SalesRepSentOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepNewOrdersPrev" runat="server" CssClass="btn" OnClick="SalesRepSentOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label15" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlSalesRepSentOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSalesRepSentOrdersPages_SelectedIndexChanged" Style="margin-top: 10px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label16" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblSalesRepSentOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnSalesRepNewOrdersNext" runat="server" CssClass="btn" OnClick="SalesRepSentOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepNewOrdersLast" runat="server" CssClass="btn" OnClick="SalesRepSentOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                        <asp:View ID="AllSentOrders" runat="server">
                            <div class="row-fluid">
                                <asp:Label ID="Label6" runat="server" Text=" All - Sent Orders" Font-Size="20px"></asp:Label>
                                <asp:Button ID="btnAllSentOrdersSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                    Width="120px" OnClick="btnAllSentOrdersSearch_Click" />
                                <asp:Button ID="btnAllSentOrdersClear" runat="server" Text="Clear" CssClass="btn"
                                    OnClick="btnAllSentOrdersClear_Click" />
                            </div>
                            <div class="row-fluid" style="margin-top: 10px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlProviderAllSent" runat="server" Width="164px" DataTextField="ProviderName"
                                                DataValueField="ProviderID">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtAllSentOrder" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtAllSentCustomerNo" runat="server" placeholder="Customer No" Width="110px"></asp:TextBox>
                                            <asp:ImageButton ID="imgAllSentCustomer" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgAllSentCustomer_Click" />
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtAllSentCustomerName" runat="server" placeholder="Customer Name"
                                                Width="300px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlAllSentOrders" runat="server" Width="164px">
                                                <asp:ListItem Value="0">&lt;All Status&gt;</asp:ListItem>
                                                <asp:ListItem Value="103">Sent</asp:ListItem>
                                                <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtAllSentDateFrom" runat="server" placeholder="From" Width="150px"></asp:TextBox><asp:CalendarExtender
                                                ID="CalendarExtender9" runat="server" TargetControlID="txtAllSentDateFrom" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtAllSentDateTo" runat="server" placeholder="To" Width="150px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtAllSentDateTo"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="width: 400px">
                                            <asp:TextBox ID="txtAllSentCreatedBy" runat="server" placeholder="Created By" Width="110px"></asp:TextBox>
                                            <asp:ImageButton ID="imgAllSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgAllSentCreatedBy_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row-fluid" style="min-height: 520px">
                                <asp:GridView ID="gvAllSentOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvAllSentOrders_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnViewAllSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrderID" HeaderText="Order No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" />
                                        <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Release Date" />
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="OrderID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="AllSentOrdersPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnAllSentOrdersFirst" runat="server" CssClass="btn" OnClick="AllSentOrdersPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnAllSentOrdersPrev" runat="server" CssClass="btn" OnClick="AllSentOrdersPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label17" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlAllSentOrdersPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlAllSentOrdersPages_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label18" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblAllSentOrdersPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnAllSentOrdersNext" runat="server" CssClass="btn" OnClick="AllSentOrdersPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnAllSentOrdersLast" runat="server" CssClass="btn" OnClick="AllSentOrdersPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="AddProductPopUp">
        <asp:Panel ID="pnlAddProductOnOrderLine" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 100px; width: 750px; height: 700px; display: none">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <h3>
                            Add Products
                        </h3>
                        <div class="row-fluid" style="margin-top: 10px">
                            <div class="span2">
                                Product Group:
                            </div>
                            <div class="span3">
                                <asp:DropDownList ID="ddlGroupCreateOrder" runat="server" AutoPostBack="True" DataValueField="ProductGroupID"
                                    DataTextField="ProductGroupText" OnSelectedIndexChanged="ddlGroupCreateOrder_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body" style="max-height: 500px; height: 500px">
                        <div class="row-fluid">
                            <asp:GridView ID="gvProductPerGroup" runat="server" AutoGenerateColumns="False" Font-Size="12px"
                                CssClass="table table-condensed" GridLines="None" OnRowCommand="gvProductPerGroup_RowCommand"
                                EmptyDataText="No Records Found">
                                <Columns>
                                    <asp:TemplateField HeaderText="ProductID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ProductID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnLess" runat="server" CommandName="Less" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ImageUrl="~/Resources/Images/minus2-26.png" />
                                            <asp:TextBox ID="txtQtyOderLine" runat="server" Width="25px" Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <asp:ImageButton ID="btnMore" runat="server" CommandName="More" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ImageUrl="~/Resources/Images/plus2-26.png" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                                ToolTip="Please input value." ControlToValidate="txtQtyOderLine" ValidationGroup="saveproductline"
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveOrderLine" runat="server" Text="Save" CssClass="btn btn-primary"
                            OnClick="btnSaveOrderLine_Click" ValidationGroup="saveproductline" />
                        <asp:Button ID="btnCancelOrderLine" runat="server" Text="Cancel" CssClass="btn" />
                    </div>
                    <asp:ModalPopupExtender ID="mpeOrderLine" runat="server" PopupControlID="pnlAddProductOnOrderLine"
                        TargetControlID="Button3" CancelControlID="btnCancelOrderLine" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                    <asp:Button ID="Button3" runat="server" Style="display: none" />
                    <asp:HiddenField ID="hidProductIDnew" runat="server" />
                    <asp:HiddenField ID="hidOrderLineTempID" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div id="StorePopUp">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlStorePopUp" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 750px; height: 700px; display: none">
                    <div class="modal-header">
                        <h3>
                            Search Store
                        </h3>
                    </div>
                    <div class="modal-body" style="min-height: 550px">
                        <div class="row-fluid">
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnCustomerSearch">
                                <div class="span1">
                                    State:
                                </div>
                                <div class="span4">
                                    <asp:DropDownList ID="ddlStateForCustomerView" runat="server" DataTextField="StateName"
                                        DataValueField="SYSStateID" AutoPostBack="True" OnSelectedIndexChanged="btnCustomerSearch_Click">
                                    </asp:DropDownList>
                                </div>
                                <div class="span4">
                                    <asp:TextBox ID="txtCustomerSearch" runat="server"></asp:TextBox>
                                </div>
                                <div class="span1">
                                    <asp:Button ID="btnCustomerSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                        OnClick="btnCustomerSearch_Click" Width="100px" />
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="row-fluid">
                            <asp:GridView ID="gvCustomerSelect" runat="server" CssClass="table table-condensed"
                                EmptyDataText="No Records Found" AutoGenerateColumns="False" GridLines="None"
                                OnRowCommand="gvCustomerSelect_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="CustomerID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerIDSearch" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessNumber" HeaderText="Customer Code" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="StateName" HeaderText="State" />
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSearchCustomer" runat="server" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row-fluid" style="text-align: center">
                            <asp:LinkButton ID="lnkCustomerFirst" runat="server" CssClass="btn" OnClick="CustomerPaging"
                                CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkCustomerPrevious" runat="server" CssClass="btn" OnClick="CustomerPaging"
                                CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                            <asp:Label ID="Label53" runat="server" Text="Page"></asp:Label>
                            <asp:DropDownList ID="ddlCustomerPages" runat="server" Width="90px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCustomerPages_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="Label54" runat="server" Text="Of"></asp:Label>
                            <asp:Label ID="lblCustomerPages" runat="server" Text=""></asp:Label>
                            <asp:LinkButton ID="lnkCustomerNext" runat="server" CssClass="btn" OnClick="CustomerPaging"
                                CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkCustomerLast" runat="server" CssClass="btn" OnClick="CustomerPaging"
                                CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                        </div>
                        <div class="row-fluid" style="text-align: right">
                            <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="btn" Style="margin-top: -40px" />
                        </div>
                        <asp:Button ID="Button2" runat="server" Text="Button" Style="display: none" />
                    </div>
                    <asp:ModalPopupExtender ID="mpeCompanySearch" runat="server" CancelControlID="Button4"
                        TargetControlID="Button2" PopupControlID="pnlStorePopUp" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ChangeProductQtyPopUp">
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlChangeProdQtyPopUp" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 400px; height: 255px; display: none">
                    <div class="modal-header">
                        <h3>
                            Change Quantity
                        </h3>
                    </div>
                    <div class="modal-body">
                        <div class="row-fluid">
                            <div class="span2" style="width: 100px">
                                Product Code:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtProductCodeChangeQty" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 100px">
                                Description:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtDescriptionChangeQty" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 100px">
                                Quantity:
                            </div>
                            <div class="span3" style="width: 240px">
                                <asp:TextBox ID="txtChangeQuantity" runat="server" Enabled="false"></asp:TextBox>
                                <asp:Label ID="txtErrorMessageChangeQuantity" runat="server" Text="*" ForeColor="Red"
                                    Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveChangeQuantity" runat="server" Text="Save" CssClass="btn btn-primary"
                            OnClick="btnSaveChangeQuantity_Click" />
                        <asp:Button ID="btnCancelChangeQuantity" runat="server" Text="Cancel" CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeChangeQtyPopUp" runat="server" CancelControlID="btnCancelChangeQuantity"
                    PopupControlID="pnlChangeProdQtyPopUp" TargetControlID="Button1" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="DeleteOrderPopUp">
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlDeleteOrderPopUp" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 580px; height: 210px; display: none">
                    <div class="modal-header">
                        <h3>
                            <asp:Label ID="lbldeletePopupTitle" runat="server" Text=""></asp:Label>
                        </h3>
                    </div>
                    <div class="modal-body">
                        <div class="row-fluid">
                            <div class="span2" style="width: 80px">
                                Order No:
                            </div>
                            <div class="span2" style="width: 100px">
                                <asp:TextBox ID="txtOrderNoForDeletion" runat="server" Enabled="false" Width="80px"></asp:TextBox>
                            </div>
                            <div class="span2" style="width: 80px">
                                Customer:
                            </div>
                            <div class="span2">
                                <asp:TextBox ID="txtCustomerForDeletion" runat="server" Enabled="false" Width="220px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <h4>
                                <asp:Label ID="lblDeletePopupText" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btn btn-primary" OnClick="btnYes_Click" />
                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn" />
                    </div>
                    <asp:Button ID="Button5" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeDeleteOrderPopUp" runat="server" CancelControlID="btnNo"
                        PopupControlID="pnlDeleteOrderPopUp" TargetControlID="Button5" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="DeleteOrderLinePopUp">
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlDeleteOrderLine" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Delete Orderline
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            Are you sure you want to DELETE this Orderline?
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnDeleteOrderLineOk" runat="server" Text="Ok" CssClass="btn btn-primary"
                            OnClick="btnDeleteOrderLineOk_Click" />
                        <asp:Button ID="btnDeleteOrderLineCancel" runat="server" Text="Cancel" CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button8" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeDeleteOrderLine" runat="server" CancelControlID="btnDeleteOrderLineCancel"
                    PopupControlID="pnlDeleteOrderLine" TargetControlID="Button8" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="PleaseAddProductNotificationPopUp">
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAddProductNotification" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            Please Add a Product First Before Proceeding.
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddProductNotifOK" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button7" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button6" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeAddProductNotification" runat="server" CancelControlID="btnAddProductNotifOK"
                    TargetControlID="Button6" PopupControlID="pnlAddProductNotification" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="QtyEnteredIsNotValid">
        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlQtyEnteredIsNotValid" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            Please Enter a Valid Quantity
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="QtyEnteredNotValidOK" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button13" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button14" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeQtyNotValidPopUp" runat="server" CancelControlID="QtyEnteredNotValidOK"
                    TargetControlID="Button14" PopupControlID="pnlQtyEnteredIsNotValid" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ReleaseAllButton">
        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlReleaseAllOrders" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Alert
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            Are you sure you want to release all Orders?
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnReleaseAllOrdersOk" runat="server" Text="Ok" CssClass="btn btn-primary"
                            OnClick="btnReleaseAllOrdersOk_Click" />
                        <asp:Button ID="btnReleaseAllOrdersCancel" runat="server" Text="Cancel" CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button15" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeReleaseAllOrders" runat="server" CancelControlID="btnReleaseAllOrdersCancel"
                    TargetControlID="Button15" PopupControlID="pnlReleaseAllOrders" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="AddProductByProductCodePopUp">
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAddProductByProductCode" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 400px; height: 280px; display: none;" DefaultButton="btnAddproductByProductCode">
                    <div class="modal-header">
                        <h3>
                            Add Product
                        </h3>
                    </div>
                    <div class="modal-body">
                        <div class="row-fluid">
                            <div class="span2" style="width: 110px">
                                Product Code:
                            </div>
                            <div class="span2 offset2">
                                Qty:
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 200px">
                                <asp:TextBox ID="txtAddProductByProductCode" runat="server" Width="150px" OnTextChanged="txtAddProductByProductCode_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtAddProductByProductCode"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtAddByProductCodeQty"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" Operator="DataTypeCheck"
                                    Type="Integer" ControlToValidate="txtAddProductByProductCode" ForeColor="Red"
                                    ErrorMessage="*" ValidationGroup="ProductByProductCode" ToolTip="Value must be a valid number" />
                            </div>
                            <div class="span2" style="width: 150px">
                                <asp:TextBox ID="txtAddByProductCodeQty" runat="server" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtAddByProductCodeQty"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                                    Type="Integer" ControlToValidate="txtAddByProductCodeQty" ForeColor="Red" ErrorMessage="*"
                                    ValidationGroup="ProductByProductCode" ToolTip="Value must be a valid number" />
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 150px">
                                Product Description:
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2">
                                <asp:TextBox ID="txtProductDescriptionByProductCode" runat="server" Width="280px"
                                    Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddproductByProductCode" runat="server" Text="Add" CssClass="btn btn-primary"
                            ValidationGroup="ProductByProductCode" OnClick="btnAddproductByProductCode_Click" />
                        <asp:Button ID="btnCloseProductByProductCode" runat="server" Text="Close" CancelControlID="btnCloseProductByProductCode"
                            CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button11" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeAddProductByProductCode" runat="server" PopupControlID="pnlAddProductByProductCode"
                    CancelControlID="btnCloseProductByProductCode" TargetControlID="Button11" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="CreatedByUserIdSearchPopUp">
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlSearchCreatedByUserID" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 620px; height: 600px; display: none">
                    <div class="modal-header">
                        <h3>
                            Search User
                        </h3>
                    </div>
                    <div class="modal-body" style="max-height: 500px">
                        <div class="row-fluid">
                            <div class="span3" style="width: 150px">
                                Office Org Unit
                            </div>
                            <div class="span3" style="width: 150px">
                                Search
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span3" style="width: 150px">
                                <asp:DropDownList AutoPostBack="true" ID="ddlOrgUnitSearch" runat="server" Width="150px"
                                    DataTextField="OrgUnitName" DataValueField="OrgUnitID" OnSelectedIndexChanged="ddlOrgUnitSearch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="span3" style="width: 255px">
                                <asp:TextBox ID="txtSearchCreatedByUserID" runat="server" Width="250px"></asp:TextBox>
                            </div>
                            <div class="span3">
                                <asp:Button ID="btnSearchCreatedByUserID" runat="server" Text="Search" CssClass="btn btn-primary"
                                    Width="100px" OnClick="btnSearchCreatedByUserID_Click" />
                            </div>
                        </div>
                        <div class="row-fluid" style="min-height: 350px">
                            <asp:GridView ID="gvCreatedByUserIDs" runat="server" CssClass="table table-condensed"
                                GridLines="None" EmptyDataText="No Records Found" AutoGenerateColumns="False"
                                OnRowCommand="gvCreatedByUserIDs_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="AccountID" HeaderText="AccountID" />
                                    <asp:BoundField DataField="LastName" HeaderText="LastName" />
                                    <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                                    <asp:BoundField DataField="Username" HeaderText="Username" />
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="row-fluid" style="text-align: center">
                            <asp:Panel ID="AccountsPanel" runat="server">
                                <asp:LinkButton ID="lnkbtnAccountsFirst" runat="server" CssClass="btn" OnClick="AccountsPaging"
                                    CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnAccountsPrev" runat="server" CssClass="btn" OnClick="AccountsPaging"
                                    CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                <asp:Label ID="Label19" runat="server" Text="Page"></asp:Label>
                                <asp:DropDownList ID="ddlAccountsPages" runat="server" Width="90px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAccountsPages_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="Label20" runat="server" Text="Of"></asp:Label>
                                <asp:Label ID="lblAccountsPages" runat="server" Text=""></asp:Label>
                                <asp:LinkButton ID="lnkbtnAccountsNext" runat="server" CssClass="btn" OnClick="AccountsPaging"
                                    CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnAccountsLast" runat="server" CssClass="btn" OnClick="AccountsPaging"
                                    CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button9" runat="server" Text="Button" Style="display: none" />
                        <asp:Button ID="btnCloseSearchCreatedByUserID" runat="server" Text="Close" CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:ModalPopupExtender ID="mpeSearchCreatedByUserID" runat="server" PopupControlID="pnlSearchCreatedByUserID"
                    CancelControlID="btnCloseSearchCreatedByUserID" TargetControlID="Button9" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="HiddenControls">
        <asp:TextBox ID="txtOrderID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtOrderLineID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCustomerIDCreateOrder" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtStateCreateOrder" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCustomerIDSearch" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtDateFromHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtDateToHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtOrderNoHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCreatedByUserIDHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCustomerIDHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtStatusIDHidden" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtProductIDAddProductByProductCode" runat="server" Visible="false"></asp:TextBox>
    </div>
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
                            && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>
</asp:Content>
