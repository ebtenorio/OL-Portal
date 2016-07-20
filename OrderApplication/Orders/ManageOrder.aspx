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
                            ShowExpandCollapse="False" OnPreRender="tvNewOrders_PreRender">
                        </asp:TreeView>
                    </div>
                    <br />
                    <br />
                    <div class="row-fluid">
                        <asp:TreeView ID="tvSentOrders" runat="server" ForeColor="#000000" OnSelectedNodeChanged="tvSentOrders_SelectedNodeChanged"
                            ShowExpandCollapse="False" OnPreRender="tvSentOrders_PreRender">
                        </asp:TreeView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="span10">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportOfficeOrders" />
                    <asp:PostBackTrigger ControlID="btnExportSalesRepNewOrders" />
                    <asp:PostBackTrigger ControlID="btnExportAllNewOrders" />                    
                    <asp:PostBackTrigger ControlID="btnExportOfficeSentOrders" />
                    <asp:PostBackTrigger ControlID="btnExportSalesRepSentOrders" />
                    <asp:PostBackTrigger ControlID="btnExportAllSentOrders" />                    
                </Triggers>

                <ContentTemplate>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="InitialLoadPage" runat="server">
                        </asp:View>

                        <asp:View ID="CreateOrder" runat="server">
                            <div class="row-fluid">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="span6">

                                        <!-- BEGIN TEST: PO NUmber Display-->
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    PO No:
                                                </div>
                                                <div class="span3" style="width: 72%">
                                                    <asp:TextBox ID="txtPONoCreateOrder" runat="server" Enabled="False" Visible="True"></asp:TextBox>
                                                </div>
                                            </div>
                                        <!-- END TEST -->


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
                                                        DataTextField="ProviderName" DataValueField="ProviderID"
                                                        OnSelectedIndexChanged="ddlProviderCreateOrder_SelectedIndexChanged">
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
                                                            placeholder="Store ID" OnTextChanged="txtCustomerCreateOrder_TextChanged" onkeypress="return isNumberKey(event);" />
                                                        <asp:ImageButton ID="imgBtnSelectCustomer" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgBtnSelectCustomer_Click" CommandName="imageButton" />
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
                                                        DaysModeTitleFormat="dd,MMM,yyyy"></asp:CalendarExtender>
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
                                                        DataTextField="ProviderWarehouseName" DataValueField="ProviderWarehouseID" Width='350px'>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Store Name:
                                                </div>
                                                <div class="span5">
                                                    <asp:TextBox ID="txtStoreNameCreateOrder" runat="server" ReadOnly="True" Width="315px"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Order Type:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlOrderType" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                                                        <asp:ListItem Value="True">Regular</asp:ListItem>
                                                        <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Release Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtCreateOrderReleaseDate" 
                                                        runat="server" 
                                                        ReadOnly="false"
                                                        onkeydown="return disableKeyDown();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </asp:TextBox>
                                                    <asp:CalendarExtender ID="calCreateOrderReleaseDate" 
                                                        runat="server"
                                                        TargetControlID="txtCreateOrderReleaseDate"                                                       
                                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                    Hold Until Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtHoldUntilDate" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txtHoldUntilDate"
                                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                    Delivery Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtDeliveryDateCreateOrder" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeliveryDateCreateOrder"
                                                        Format="MM/dd/yyyy"></asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid">
                                <table width="100%" id="tblOrderLineButtonsOrderDetails" runat="server">
                                    <tr>
                                        <td style="width: 100px">
                                            <h4>
                                                Order Line
                                            </h4>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Button ID="btnAddProductLine" runat="server" Text="Product List" CssClass="btn btn-primary"
                                                OnClick="btnAddProductLine_Click" />
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Button ID="btnAddProductLineByProductCode" runat="server" Text="Add By Code"
                                                CssClass="btn btn-success" OnClick="btnAddProductLineByProductCode_Click" />
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row-fluid">
                                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrderErrorMessage" runat="server" Text="" Font-Size="Small" ForeColor="#FF3300"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid" style="min-height: 300px;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvOrderLineCreateOrder" runat="server" EmptyDataText="No Line Added"
                                            GridLines="None" CssClass="table table-condensed" AutoGenerateColumns="False"
                                            OnRowCommand="gvOrderLineCreateOrder_RowCommand" OnRowDeleted="gvOrderLineCreateOrder_RowDeleted"
                                            OnRowDeleting="gvOrderLineCreateOrder_RowDeleting" 
                                            DataKeyNames="ProductID" Width="1000px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="imgbtnViewOrderLineProduct" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                            Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                                <asp:BoundField DataField="GTINCode" HeaderText="Product GTIN" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Code" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                                <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                            <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                            <asp:BoundField DataField="UOM" HeaderText="UOM" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                            <asp:BoundField DataField="Discount" HeaderText="% Discount" DataFormatString="{0:F}"><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:TemplateField HeaderText="Delete"><ItemTemplate><asp:ImageButton ID="imgbtnDeleteOrderlineProduct" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                            Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>

                                                <asp:TemplateField HeaderText="OrderLineID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderLineID" runat="server" Text='<%# Bind("OrderLineID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderLineID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div>
                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnbackSentOrder" runat="server" Text="Back" CssClass="btn btn-primary"
                                            OnClick="btnbackSentOrder_Click" />
                                        <asp:Button ID="btnSaveCreateOrder" runat="server" Text="Save" CssClass="btn btn-primary"
                                            ValidationGroup="SaveNewOrder" OnClick="btnSaveCreateOrder_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>

                        <asp:View ID="OfficeNewOrders" runat="server">
                            <div class="row-fluid" style="min-height: 520px;min-width: 1000px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="Label1" runat="server" Text=" Office - New Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCreateOrder" runat="server" Text="Create Order" CssClass="btn btn-primary"
                                                OnClick="btnCreateOrder_Click" />
                                        </td>
                                        <td style="text-align: right">
                                                <asp:Button ID="btnExportOfficeOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportOfficeOrders_Click"/>
                                            <asp:Button ID="btnReleaseAllOrdersOfficeNew" runat="server" Text="Release Orders"
                                                CssClass="btn btn-primary" OnClick="btnReleaseAllOrdersOfficeNew_Click" />
                                        </td>
                                    </tr>
                                </table>

                                <!-- BEGIN: TEST OFFICE NEW SEARCH -->
                                <div class="row-fluid" style="margin-top: 10px;">

                                        <!-- A <TABLE> contains the rows for the UI below -->
                                        <!-- A <TR> tells the UI that it is displayed in a row. -->
                                        <!-- A <TD> tells the <TR> to display UI left to right, according to the sequence. -->

                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderOfficeNewOrder" runat="server" Width="150px" DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers"></asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtOrderNoSearchOfficeNewOrder" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCustomerNameSearchOfficeNewOrder" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCustomerSearchOfficeNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgbtnCustomerSearchOfficeNewOrder_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtCreatedBySearchOfficeNewOrder" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtCreatedBySearchOfficeNewOrder_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCreatedByUserIDSearchOfficeNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgbtnCreatedByUserIDSearchOfficeNewOrder_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtOfficeSentOrdersGTINCodeSearchOfficeNewOrder" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgGTINCodeSearchOfficeNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgGTINCodeSearchOfficeNewOrder_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStatesOfficeNewOrder" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
<%--                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlStatusSearchOfficeNewOrder" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>--%>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesOfficeNewOrder" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtDateFromOfficeNewOrder" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender19" runat="server" TargetControlID="txtDateFromOfficeNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtDateToOfficeNewOrder" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender20" runat="server" TargetControlID="txtDateToOfficeNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtReleaseFromOfficeNewOrder" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender21" runat="server" TargetControlID="txtReleaseFromOfficeNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtReleaseToOfficeNewOrder" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender22" runat="server" TargetControlID="txtReleaseToOfficeNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:Button ID="btnOfficeNewOrderSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnOfficeNewOrderSearch_Click" /> <asp:Button ID="btnSearchOfficeNewOrderClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnSearchOfficeNewOrderClear_Click" /> 
                                                    </td>
                                                </tr>

                                                </tr>
                                            </table>
                                        </div>

                                <!-- END: TEST OFFICE NEW SEARCH -->

                                <asp:GridView ID="gvOfficeNewOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" OnRowCommand="gvOfficeNewOrders_RowCommand"
                                    EmptyDataText="No Records Found" OnDataBound="gvOfficeNewOrders_DataBound" DataKeyNames="OrderID,OrderNumber,IsRegularOrder,RequestedReleaseDate,PONumber"
                                    OnRowDataBound="gvOfficeNewOrders_RowDataBound" width="1000px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="imgbtnOfficeNewOrders" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" Width="60px"/><ItemStyle Height="60px" HorizontalAlign="Center" /></asp:TemplateField>

                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" Width="87px"/></asp:BoundField>

                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" Width="250px"/></asp:BoundField>

                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" Width="82px"/></asp:BoundField>

                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ><HeaderStyle HorizontalAlign="Left" Width="100px"/></asp:BoundField>

                                        <asp:BoundField DataField="IsRegularOrder" HeaderText="Order Type" ><HeaderStyle HorizontalAlign="Left" Width="90px" /></asp:BoundField>

                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" Width="80px"/></asp:BoundField>

                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" Visible="True"><HeaderStyle HorizontalAlign="Left" Width="74px"/></asp:BoundField>

                                        <asp:TemplateField HeaderText="Release"><ItemTemplate><asp:ImageButton ID="imgBtnRelease" runat="server" ImageUrl="~/Resources/Images/upload-26.png" Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /><asp:Label ID="lblHoldUntilDate" runat="server" Text='<%# Eval("HoldUntilDate") %>'  style="display:none"></asp:Label><asp:Label ID="lblRequestedReleaseDate" runat="server" Text='<%# Eval("RequestedReleaseDate") %>'  style="display:none"></asp:Label><asp:Label ID="lblReleaseDate" Text='<%# Eval("ReleaseDate") %>' runat="server" style="display:none" Visible="false"></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" Width="70px"/><ItemStyle Height="52px" HorizontalAlign="Center" /></asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" Visible="false"><ItemStyle HorizontalAlign="Center" /><ItemTemplate><asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate></asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            <%--</div>-- REMOVED --%>

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
                            </div>
                        </asp:View>
                        <asp:View ID="ViewOrders" runat="server">
                            <div class="row-fluid">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="span6">

                                        <!-- TEST VIEW ORDERS-->
                                        <div class="row-fluid">
                                                <div class="span2" style="width: 80px">
                                                    PO No:
                                                </div>
                                                <div class="span3" style="width: 72%">
                                                    <asp:TextBox ID="txtPONoViewOrders" runat="server" Enabled="False"></asp:TextBox>
                                                </div>
                                            </div>

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
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtOrderDateViewOrders"></asp:CalendarExtender>
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
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                    Hold Until Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtHoldUntildateView" runat="server" Enabled="false"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender12" runat="server" TargetControlID="txtHoldUntildateView"
                                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row-fluid" style="display: none">
                                                <div class="span2" style="width: 100px">
                                                    Delivery Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:TextBox ID="txtDeliveryDateViewOrders" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtDeliveryDateViewOrders"></asp:CalendarExtender>
                                                </div>
                                            </div>

                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Order Type:
                                                </div>
                                                <div class="span3">
                                                    <asp:Label ID="lblOrderDetailsOrderType" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>


                                            <div class="row-fluid">
                                                <div class="span2" style="width: 100px">
                                                    Release Date:
                                                </div>
                                                <div class="span3">
                                                    <asp:Label ID="lblOrderDetailsReleaseDate" runat="server" Text=""></asp:Label>
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
                            <div class="row-fluid" style="min-height: 300px;">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvOrderLineViewOrders" runat="server" EmptyDataText="No Line Added"
                                            GridLines="None" CssClass="table table-condensed" AutoGenerateColumns="False" Width="1000">
                                            <Columns>

                                                <asp:BoundField DataField="ProductCode" HeaderText="Product Code"><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="ProductDescription" HeaderText="Product Description"><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="OrderQty" HeaderText="Order Qty"><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="Discount" HeaderText="% Discount" DataFormatString="{0:F}"><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>

                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row-fluid">
                                <%--  <asp:LinkButton ID="lnkButtonViewOrders" runat="server" Text="<< Back to List" OnClick="BackToListFunction">
                                
                                </asp:LinkButton>--%>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBackOfficeOrder" runat="server" Text="Back" CssClass="btn btn-primary"
                                            OnClick="btnBackOfficeOrder_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                        <asp:View ID="SalesRepViewNO" runat="server">
                            <div class="row-fluid">


                            <!-- TEST BEGIN : TABLE HERE -->

                                <table width="1000px">
                                    <tr>
                                        <td style="width: 500px">
                                            <asp:Label ID="Label2" runat="server" Text="Sales Rep - New Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportSalesRepNewOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportSalesRepNewOrders_Click"/>                                        
                                        <asp:Button ID="btnReleaseAllOrders" runat="server" Text="Release Orders" CssClass="btn btn-primary"
                                        OnClick="btnReleaseAllOrders_Click" />
                                        </td>
                                    </tr>
                                </table>

                            <!-- TEST END : TABLE HERE -->


<%--                                <div class="span5">
                                    <asp:Label ID="Label2" runat="server" Text="Sales Rep - New Orders" Font-Size="20px"></asp:Label>
                                </div>

                                <div class="span2 offset4">
                                   <asp:Button ID="btnReleaseAllOrders" runat="server" Text="Release Orders" CssClass="btn btn-success"
                                        OnClick="btnReleaseAllOrders_Click" />
                                </div>
--%>
                            </div>

                                <!-- BEGIN: TEST SALES REP NEW SEARCH -->
                                <div class="row-fluid" style="margin-top: 10px;">

                                        <!-- A <TABLE> contains the rows for the UI below -->
                                        <!-- A <TR> tells the UI that it is displayed in a row. -->
                                        <!-- A <TD> tells the <TR> to display UI left to right, according to the sequence. -->

                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderSalesRepNewOrder" runat="server" Width="150px" DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers"></asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtOrderNoSearchSalesRepNewOrder" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCustomerNameSearchSalesRepNewOrder" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCustomerSearchSalesRepNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgbtnCustomerSearchSalesRepNewOrder_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtCreatedBySearchSalesRepNewOrder" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtCreatedBySearchSalesRepNewOrder_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCreatedByUserIDSearchSalesRepNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgbtnCreatedByUserIDSearchSalesRepNewOrder_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtOfficeSentOrdersGTINCodeSearchSalesRepNewOrder" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgGTINCodeSearchSalesRepNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgGTINCodeSearchSalesRepNewOrder_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStatesSalesRepNewOrder" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
<%--                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlStatusSearchSalesRepNewOrder" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>--%>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesSalesRepNewOrder" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtDateFromSalesRepNewOrder" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender23" runat="server" TargetControlID="txtDateFromSalesRepNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtDateToSalesRepNewOrder" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender24" runat="server" TargetControlID="txtDateToSalesRepNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtReleaseFromSalesRepNewOrder" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender25" runat="server" TargetControlID="txtReleaseFromSalesRepNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtReleaseToSalesRepNewOrder" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender26" runat="server" TargetControlID="txtReleaseToSalesRepNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:Button ID="btnSalesRepNewOrderSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnSalesRepNewOrderSearch_Click" /> <asp:Button ID="btnSearchSalesRepNewOrderClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnSearchSalesRepNewOrderClear_Click" /> 
                                                    </td>
                                                </tr>

                                                </tr>
                                            </table>
                                        </div>

                                <!-- END: TEST SALES REP NEW SEARCH -->

                            <br />
                            <br />
                            <div class="row-fluid" style="min-height: 520px; margin-top: -30px;">
                                <asp:GridView ID="gvSalesRepNewOrders" runat="server" CssClass="table table-condensed" EnableViewState=true
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvSalesRepNewOrders_RowCommand" OnDataBound="gvSalesRepNewOrders_DataBound"
                                    DataKeyNames="OrderID,OrderNumber,IsRegularOrder,RequestedReleaseDate,PONumber" OnRowDataBound="gvSalesRepNewOrders_RowDataBound" Width="1000px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle Height="60px" HorizontalAlign="Center" /></asp:TemplateField>

                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" Width="87px" /></asp:BoundField>

                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" Width="250px" /></asp:BoundField>
                                        
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" Width="82px"/></asp:BoundField>

                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ><HeaderStyle HorizontalAlign="Left" Width="100px" /></asp:BoundField>

                                        <asp:BoundField DataField="IsRegularOrder" HeaderText="Order Type" ><HeaderStyle HorizontalAlign="Left" Width="90"/></asp:BoundField>

                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" Width="80px" /></asp:BoundField>

                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" ><HeaderStyle HorizontalAlign="Left" Width="60px"/></asp:BoundField>

                                        <asp:TemplateField HeaderText="Release"  ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Label ID="lblHoldUntilDateSalesRepNew" runat="server" Text='<%# Eval("HoldUntilDate") %>' ></asp:Label><asp:Label ID="lblRequestedReleaseDateSalesRepNew" runat="server" Text='<%# Eval("RequestedReleaseDate") %>'  style="display:none" ></asp:Label><asp:Label ID="lblReleaseDateSalesRepNew" Text='<%# Eval("ReleaseDate") %>' runat="server" style="display:none" ></asp:Label><asp:Label ID="lblOrderIDSalesRepNew" Text='<%# Eval("OrderID") %>' runat="server" style="display:none" ></asp:Label><asp:ImageButton ID="imgBtnReleaseSalesRepNew" runat="server" ImageUrl="~/Resources/Images/upload-26.png" Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><ItemStyle   CssClass="gvItemCenter" /><HeaderStyle CssClass="gvHeaderCenter" Width="64px"/></asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cancel"><ItemTemplate><asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle CssClass="gvHeaderCenter" Width="57px" /><ItemStyle CssClass="gvItemCenter" /></asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
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
                                                                                           
                            <!-- TEST BEGIN : TABLE HERE -->

                                <table width="1000px">
                                    <tr>
                                        <td style="width: 500px">
                                            <asp:Label ID="Label3" runat="server" Text="All - New Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <!-- Added : 2015-06-03 -->
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportAllNewOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportAllNewOrders_Click"/>
                                        </td>
                                    </tr>
                                </table>

                            <!-- TEST END : TABLE HERE -->



                                <!-- BEGIN: TEST ALL NEW SEARCH -->
                                <div class="row-fluid" style="margin-top: 10px;">

                                        <!-- A <TABLE> contains the rows for the UI below -->
                                        <!-- A <TR> tells the UI that it is displayed in a row. -->
                                        <!-- A <TD> tells the <TR> to display UI left to right, according to the sequence. -->

                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderAllNewOrder" runat="server" Width="150px" DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers"></asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtOrderNoSearchAllNewOrder" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCustomerNameSearchAllNewOrder" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCustomerSearchAllNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgbtnCustomerSearchAllNewOrder_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtCreatedBySearchAllNewOrder" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtCreatedBySearchAllNewOrder_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCreatedByUserIDSearchAllNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgbtnCreatedByUserIDSearchAllNewOrder_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtOfficeSentOrdersGTINCodeSearchAllNewOrder" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgGTINCodeSearchAllNewOrder" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgGTINCodeSearchAllNewOrder_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStatesAllNewOrder" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
<%--                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlStatusSearchAllNewOrder" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>--%>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesAllNewOrder" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtDateFromAllNewOrder" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender27" runat="server" TargetControlID="txtDateFromAllNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtDateToAllNewOrder" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender28" runat="server" TargetControlID="txtDateToAllNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtReleaseFromAllNewOrder" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender29" runat="server" TargetControlID="txtReleaseFromAllNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtReleaseToAllNewOrder" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender30" runat="server" TargetControlID="txtReleaseToAllNewOrder" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:Button ID="btnAllNewOrderSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnAllNewOrderSearch_Click" /> <asp:Button ID="btnSearchAllNewOrderClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnSearchAllNewOrderClear_Click" /> 
                                                    </td>
                                                </tr>

                                                </tr>
                                            </table>
                                        </div>

                                <!-- END: TEST ALL NEW SEARCH -->

                                <asp:GridView ID="gvAllNewOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvAllNewOrders_RowCommand" OnDataBound="gvAllNewOrders_DataBound"
                                    DataKeyNames="OrderId,OrderNumber,IsRegularOrder,RequestedReleaseDate,PONumber" OnRowDataBound="gvAllNewOrders_RowDataBound" Width="1000px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle Height="60px" HorizontalAlign="Center" /></asp:TemplateField>
                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="87px" HorizontalAlign="Left" /></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="250px" HorizontalAlign="Left" /></asp:BoundField>
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="82px" HorizontalAlign="Left" /></asp:BoundField>
                                        
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                        
                                        <asp:BoundField DataField="IsRegularOrder" HeaderText="Order Type" />

                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" ><HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                        <asp:TemplateField HeaderText="Release"><ItemTemplate><asp:ImageButton ID="imgBtnReleaseAllOrders" runat="server" ImageUrl="~/Resources/Images/upload-26.png" Width="20px" CommandName="Release" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /><asp:Label ID="lblHoldUntilDateAll" runat="server" Text='<%# Eval("HoldUntilDate") %>'></asp:Label><asp:Label ID="lblRequestedReleaseDateAllNewOrders" runat="server" Text='<%# Eval("RequestedReleaseDate") %>'  style="display:none"></asp:Label><asp:Label ID="lblReleaseDateAllNewOrders" Text='<%# Eval("ReleaseDate") %>' runat="server" style="display:none" Visible="false"></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete"><ItemTemplate><asp:ImageButton ID="imgbtnDeleteOrder" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                                    Width="20px" CommandName="Nothing" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
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
                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnOfficeSentSearch">

                                <table width="1000px">
                                    <tr>
                                        <td style="width: 500px">
                                            <asp:Label ID="Label4" runat="server" Text="Office - Sent Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <!-- Added : 2015-06-03 -->
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportOfficeSentOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportOfficeSentOrders_Click"/>
                                        </td>
                                    </tr>
                                </table>

<%--                                        <div class="row-fluid">
                                        <!-- galing dito -->
                                            <asp:Label ID="Label4" runat="server" Text=" Office - Sent Orders" Font-Size="20px"></asp:Label>

                                        </div>

                                        <!-- Added : 2015-06-04 -->
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportOfficeSentOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportOfficeSentOrders_Click"/>
                                        </td>
--%>
                                        <div class="row-fluid" style="margin-top: 10px;">

                                        <!-- A <TABLE> contains the rows for the UI below -->
                                        <!-- A <TR> tells the UI that it is displayed in a row. -->
                                        <!-- A <TD> tells the <TR> to display UI left to right, according to the sequence. -->

                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderOffice" runat="server" Width="150px" DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers"></asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtOrderNoSearch" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCustomerNameSearch" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCustomerSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgbtnCustomerSearch_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtCreatedBySearch" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtCreatedBySearch_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgbtnCreatedByUserIDSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgbtnCreatedByUserIDSearch_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtOfficeSentOrdersGTINCodeSearch" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgGTINCodeSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgGTINCodeSearch_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStates" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlStatusSearch" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesOfficeSent" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtDateFrom" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtDateTo" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>

                                                    </td>

                                                    <td style="width: 300px;">
                                                        <asp:TextBox ID="txtReleaseFrom" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender13" runat="server" TargetControlID="txtReleaseFrom" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtReleaseTo" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender14" runat="server" TargetControlID="txtReleaseTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 300px;" align="right" colspan="2"> 
                                                        <asp:Button ID="btnOfficeSentSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnOfficeSentSearch_Click" /><asp:Button ID="btnSearchOfficeSentClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnSearchOfficeSentClear_Click" />                                                    
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <div class="row-fluid" style="min-height: 520px">
                                        <asp:GridView ID="gvOfficeSentOrders" runat="server" CssClass="table table-condensed"
                                            AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                            OnRowCommand="gvOfficeSentOrders_RowCommand" DataKeyNames="OrderID,OrderNumber,IsRegularOrder,ReleaseDate,PONumber"
                                            OnRowDataBound="gvOfficeSentOrders_RowDataBound" width="1000px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="imgbtnViewOfficeSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                            Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle Height="58px" HorizontalAlign="Center" Width="50px"/></asp:TemplateField>

                                                <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="87px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="250px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="82px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="136px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="IsRegularOrder" HeaderText="Order Type" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                                    HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" 
                                                    HeaderText="Release Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="115px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="63px" HorizontalAlign="Left" /></asp:BoundField>

                                                <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:View>
                        <asp:View ID="SalesRepSentOrders" runat="server">

                                <table width="1000px">
                                    <tr>
                                        <td style="width: 500px">
                                            <asp:Label ID="Label5" runat="server" Text="Sales Rep - Sent Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <!-- Added : 2015-06-03 -->
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportSalesRepSentOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportSalesRepSentOrders_Click"/>
                                        </td>
                                    </tr>
                                </table>                        

<%--                            <div class="row-fluid">
                                <asp:Label ID="Label5" runat="server" Text="Sales Rep - Sent Orders" Font-Size="20px"></asp:Label>                                
                            </div>

                            <!-- Added : 2015-06-04 -->
                            <td style="text-align: right; width: 500px" >
                                <asp:Button ID="btnExportSalesRepSentOrders" runat="server" Text="Export List"
                                    CssClass="btn btn-success" OnClick="btnExportSalesRepSentOrders_Click"/>
                            </td>--%>

                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                <ContentTemplate>
                                    <div class="row-fluid" style="margin-top: 10px">
                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderSentOrderSearch" runat="server" Width="150px"
														DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers">
														</asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtSalesRepSentOrderNo" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtSalesRepSentCustomerName" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="imgSalesRepSentCustomerNo" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgSalesRepSentCustomerNo_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtSalesRepSentCreatedBy" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtSalesRepSentCreatedBy_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgSalesRepSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgSalesRepSentCreatedBy_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtOfficeSentOrdersGTINCodeSearchSalesRepSentOrders" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgGTINCodeSearchSalesRepSentOrders" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgGTINCodeSearchSalesRepSentOrders_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStatesSalesRepSentOrders" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlSalesRepSent" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesSalesRepSentOrders" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtSalesRepSentDateFrom" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtSalesRepSentDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtSalesRepSentDateTo" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtSalesRepSentDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>

                                                    </td>

                                                    <td style="width: 300px;">
                                                        <asp:TextBox ID="txtReleaseFromSalesRepSentOrders" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender15" runat="server" TargetControlID="txtReleaseFromSalesRepSentOrders" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtReleaseToSalesRepSentOrders" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtReleaseToSalesRepSentOrders" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <!-- SEARCH : SENT SALES REP ORDERS -->
                                                    <td style="width: 300px;" align="right" colspan="2"> 
                                                        <asp:Button ID="btnSalesRepSentOrdersSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnSalesRepSentOrdersSearch_Click" /><asp:Button ID="btnSalesRepSentOrdersClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnSalesRepSentOrdersClear_Click" />
                                                    </td>

                                                </tr>
                                            </table>

                                    <!-- END TEST CODE -->

<%--                                        <table width="100%" >
                                            <tr>
                                                <td style="width: 200px">
                                                    <asp:DropDownList ID="ddlProviderSentOrderSearch" runat="server" Width="164px" DataTextField="ProviderName"
                                                        DataValueField="ProviderID">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px">
                                                    <asp:TextBox ID="txtSalesRepSentOrderNo" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSalesRepSentCustomerName" runat="server" placeholder="Customer Name"
                                                        Width="450px" ReadOnly="true"></asp:TextBox>
                                                    <asp:ImageButton ID="imgSalesRepSentCustomerNo" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                        Style="margin-top: -10px" OnClick="imgSalesRepSentCustomerNo_Click" />
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
                                                        Width="110px" AutoPostBack="True" OnTextChanged="txtSalesRepSentCreatedBy_TextChanged"></asp:TextBox>
                                                    <asp:ImageButton ID="imgSalesRepSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                        Style="margin-top: -10px" OnClick="imgSalesRepSentCreatedBy_Click" />
                                                </td>
                                            </tr>
                                        </table>--%>
                                    </div>
                                    <%-- </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row-fluid" style="min-height: 490px">
                                <asp:GridView ID="gvSalesRepSentOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvSalesRepSentOrders_RowCommand" DataKeyNames="OrderID,OrderNumber,IsRegularOrder,ReleaseDate,PONumber"
                                    OnRowDataBound="gvSalesRepSentOrders_RowDataBound" Width="1000px">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="imgbtnViewSalesRepSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle Height="60px" HorizontalAlign="Center" Width="50px"/></asp:TemplateField>

                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="87px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="250px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="82px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="136px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="IsRegularOrder" 
                                            HeaderText="Order Type" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Release Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="115px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="63px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
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


                                <table width="1000px">
                                    <tr>
                                        <td style="width: 500px">
                                            <asp:Label ID="Label23" runat="server" Text="All - Sent Orders" Font-Size="20px"></asp:Label>
                                        </td>
                                        <!-- Added : 2015-06-03 -->
                                        <td style="text-align: right; width: 500px" >
                                                <asp:Button ID="btnExportAllSentOrders" runat="server" Text="Export List"
                                                CssClass="btn btn-success" OnClick="btnExportAllSentOrders_Click"/>
                                        </td>
                                    </tr>
                                </table>

<%--                            <div class="row-fluid">
                                <asp:Label ID="Label6" runat="server" Text=" All - Sent Orders" Font-Size="20px"></asp:Label>
                            
                            <!-- Added : 2015-06-04 -->
                            <td style="text-align: right; width: 500px" >
                                <asp:Button ID="btnExportAllSentOrders" runat="server" Text="Export List"
                                    CssClass="btn btn-success" OnClick="btnExportAllSentOrders_Click"/>
                            </td>

                            </div>--%>


                            <div class="row-fluid" style="margin-top: 10px;">

                                        <!-- A <TABLE> contains the rows for the UI below -->
                                        <!-- A <TR> tells the UI that it is displayed in a row. -->
                                        <!-- A <TD> tells the <TR> to display UI left to right, according to the sequence. -->

                                            <table width="1000px">

                                                <!-- First row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlProviderAllSent" runat="server" Width="150px" DataTextField="ProviderName" DataValueField="ProviderID" Placeholder="All Providers"></asp:DropDownList>
                                                    </td>

                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtAllSentOrder" runat="server" placeholder="Order No" Width="150px"></asp:TextBox>
                                                    </td>

                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtAllSentCustomerName" runat="server" placeholder="Customer Name" Width="500px" ReadOnly="True"></asp:TextBox>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Resources/Images/list-26.png" Style="margin-top: -10px" OnClick="imgAllSentCustomer_Click" />
                                                    </td>
                                                </tr>

                                                <!-- Second row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:TextBox ID="txtAllSentCreatedBy" runat="server" placeholder="Created By" Width="110px"
                                                            AutoPostBack="True" OnTextChanged="txtAllSentCreatedBy_TextChanged"></asp:TextBox>
                                                        <asp:ImageButton ID="imgAllSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" OnClick="imgAllSentCreatedBy_Click" />
                                                    </td>                                                    

                                                    <td style="width: 250px">
                                                        <asp:TextBox ID="txtAllSentOrdersGTINCodeSearch" runat="server" placeholder="GTIN Code" Width="150px"
                                                            AutoPostBack="True" ></asp:TextBox>
                                                        <asp:ImageButton ID="imgAllSentGTINCodeSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                            Style="margin-top: -10px" onclick="imgAllSentGTINCodeSearch_Click" />
                                                    </td>

                                                    <td style="width: 200px">
                                                        <asp:DropDownList ID="ddlStatesAllSentOrders" runat="server" Width="164px" DataTextField="StateName"
                                                             DataValueField="SYSStateID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                    <td style="width: 300px;" align="right" colspan="2">
                                                        <asp:DropDownList ID="ddlAllSentOrders" runat="server" Width="164px">
                                                            <asp:ListItem Value="0">&lt;Any Status&gt;</asp:ListItem>
                                                            <asp:ListItem Value="103">Sent</asp:ListItem>
                                                            <asp:ListItem Value="104">Acknowledged</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <!-- 3rd row of UI controls -->
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlOrderTypesAllSent" runat="server" Width="170px">
                                                            <asp:ListItem Value="">&lt;All Order Types&gt;</asp:ListItem>
                                                            <asp:ListItem Value="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="False">Pre-sell</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                    <td style="width: 250px;">
                                                        <asp:TextBox ID="txtAllSentDateFrom" runat="server" placeholder="Order From" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txtAllSentDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtAllSentDateTo" runat="server" placeholder="Order To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtAllSentDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>

                                                    <td style="width: 300px;">
                                                        <asp:TextBox ID="txtAllSentReleaseFrom" runat="server" placeholder="Release From" Width="100px"></asp:TextBox><asp:CalendarExtender
                                                            ID="CalendarExtender17" runat="server" TargetControlID="txtAllSentReleaseFrom" Format="dd/MM/yyyy"></asp:CalendarExtender> <asp:TextBox ID="txtAllSentReleaseTo" runat="server" placeholder="Release To" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender18" runat="server" TargetControlID="txtAllSentReleaseTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>
                                                    <!-- SEARCH : ALL SENT ORDERS -->
                                                    <td style="width: 300px;" align="right" colspan="2"> 
                                                        <asp:Button ID="btnAllSentOrdersSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                        Width="120px" OnClick="btnAllSentOrdersSearch_Click" /><asp:Button ID="btnAllSentOrdersClear" runat="server" Text="Clear" CssClass="btn"
                                                        OnClick="btnAllSentOrdersClear_Click" />

                                                    </td>

                                                </tr>
                                            </table>
                                        </div>

<%--                            <div class="row-fluid" style="margin-top: 10px">
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
                                        <td colspan="2">
                                            <asp:TextBox ID="txtAllSentCustomerName" runat="server" placeholder="Customer Name"
                                                Width="450px" ReadOnly="true"></asp:TextBox>
                                            <asp:ImageButton ID="imgAllSentCustomer" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgAllSentCustomer_Click" />
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
                                            <asp:TextBox ID="txtAllSentCreatedBy" runat="server" placeholder="Created By" Width="110px"
                                                AutoPostBack="True" OnTextChanged="txtAllSentCreatedBy_TextChanged"></asp:TextBox>
                                            <asp:ImageButton ID="imgAllSentCreatedBy" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                Style="margin-top: -10px" OnClick="imgAllSentCreatedBy_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <div class="row-fluid" style="min-height: 490px">
                                <asp:GridView ID="gvAllSentOrders" runat="server" CssClass="table table-condensed"
                                    AutoGenerateColumns="False" Font-Size="14px" GridLines="None" EmptyDataText="No Records Found"
                                    OnRowCommand="gvAllSentOrders_RowCommand" DataKeyNames="OrderID,OrderNumber,IsRegularOrder,ReleaseDate,PONumber"
                                    OnRowDataBound="gvAllSentOrders_RowDataBound" Width="1000px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select"><ItemTemplate><asp:ImageButton ID="imgbtnViewAllSO" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                                    Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /></ItemTemplate><HeaderStyle HorizontalAlign="Left" /><ItemStyle Height="60px" HorizontalAlign="Left" /></asp:TemplateField>

                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="87px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="250px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="82px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="136px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="IsRegularOrder" 
                                            HeaderText="Order Type" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" 
                                            HeaderText="Order Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="99px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="Release Date" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="115px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:BoundField DataField="SYSOrderStatusText" HeaderText="Status" ><HeaderStyle HorizontalAlign="Left" /><ItemStyle Width="63px" HorizontalAlign="Left" /></asp:BoundField>

                                        <asp:TemplateField HeaderText="OrderID" Visible="False"><ItemTemplate><asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OrderID") %>'></asp:TextBox></EditItemTemplate></asp:TemplateField>
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
            top: 100px; width: 750px; height: 700px; display: none" DefaultButton="btnSaveOrderLine">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <h3>
                            Product List
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

                                    <asp:TemplateField HeaderText="UOMID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUOMID" runat="server" Text='<%# Bind("UOMID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUOMID" runat="server" Text='<%# Bind("UOMID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                    <asp:BoundField DataField="GTINCode" HeaderText="Product GTIN" >
                                     <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                    
                                    <asp:TemplateField HeaderText="Code" Visible="True">
                                        
<%--                                            <asp:BoundField DataField="ProductCode" HeaderText="Code" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
--%>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("ProductCode") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" >
                                     <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnLess" runat="server" CommandName="Less" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ImageUrl="~/Resources/Images/minus2-26.png" TabIndex="-1" />
                                            <asp:TextBox ID="txtQtyOderLine" runat="server" Width="40px" Text="0" onkeypress="return validateQuantity(this, event);"></asp:TextBox>
                                            <asp:ImageButton ID="btnMore" runat="server" CommandName="More" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ImageUrl="~/Resources/Images/plus2-26.png" TabIndex="-1"/>
                                            <asp:RegularExpressionValidator Style="display: none" ForeColor="Red" Font-Size="Smaller" 
                                                ValidationGroup="saveproductline" ControlToValidate="txtQtyOderLine" ID="QuantityRegularExpressionValidator"
                                                runat="server" ErrorMessage="Invalid quantity" ValidationExpression="^([0-9]{0,10})$">
				                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				                            </asp:RegularExpressionValidator>

                                            <asp:ValidatorCalloutExtender ID="QuantityValidatorCalloutExtender" runat="server" TargetControlID="QuantityRegularExpressionValidator" PopupPosition="Left">
                                            </asp:ValidatorCalloutExtender>
                                      </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="% Discount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscount" runat="server" Width="50px"
                                                OnKeyPress="return validateDiscount(this, event);"
                                                Text='<%# String.Format("{0:F}",DataBinder.Eval(Container.DataItem, "Discount"))%>'>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                             </asp:TextBox>

                                            <asp:RegularExpressionValidator Style="display: none" ForeColor="Red" Font-Size="Smaller" 
                                                ValidationGroup="saveproductline" ControlToValidate="txtDiscount" ID="DiscountRegularExpressionValidator"
                                                runat="server" ErrorMessage="Discount must be between 0.00 and 99.99, and can't have more than 2 decimal places." ValidationExpression="^([0-9]{1,2}[.][0-9]{1,2}|[0-9]{1,2})$">
				                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				                            </asp:RegularExpressionValidator>

                                            <asp:ValidatorCalloutExtender ID="DiscountValidatorCalloutExtender" runat="server" TargetControlID="DiscountRegularExpressionValidator" PopupPosition="Left">
                                            </asp:ValidatorCalloutExtender>


                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StartDate" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="EndDate" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveOrderLine" runat="server" Text="Ok" CssClass="btn btn-primary"
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

    <!-- Search Products -->

        <div id="SearchProductPopUp">
        <asp:UpdatePanel ID="SearchProductUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlSearchProductPopUp" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 500px; display: none; height:600px">
                    <div class="modal-header">
                        <h3>
                            Search Product
                        </h3>
                    </div>
                    <div class="modal-body" style="min-height: 600px">
                        <div class="row-fluid">
                            <asp:Panel ID="SearchProductPanel" runat="server" DefaultButton="btnSearchProduct">
                                <table>
                                    <tr>
                                        <td style="width: 100px">
                                            Product Group: 
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchProductGroup" runat="server" DataTextField="ProductGroupText"
                                                DataValueField="ProductGroupID" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchProductGroup_SelectedIndexChanged"
                                                Width="350px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    </table>
                            </asp:Panel>
                        <div class="row-fluid">
                            <asp:GridView ID="gvProductPerGroupSearch" runat="server" AutoGenerateColumns="False" Font-Size="12px"
                                CssClass="table table-condensed" GridLines="None" OnRowCommand="gvProductPerGroupSearch_RowCommand"
                                OnRowDataBound="gvProductPerGroupSearch_RowDataBound" EmptyDataText="No Records Found" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="ProductID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductGTINCodeOfficeSentOrders" runat="server" Text='<%# Bind("GTINCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProductGTINCodeOfficeSentOrders" runat="server" Text='<%# Bind("GTINCode") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="GTINCode" HeaderText="Product GTIN" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" CommandName = "Select" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>                  
                           </div>
                        </div>
                            <!-- Footer buttons -->
                      <div class="modal-footer">
                        <%--<div class="row-fluid" style="text-align: center">--%>
                            <asp:Panel ID="GTINSentOfficeOrderPanel" runat="server">
                                <asp:LinkButton ID="lnkFirstGTINOfficeSentOrder" runat="server" CssClass="btn" OnClick="GTINOfficeSentOrderPaging"
                                    CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkPreviousGTINOfficeSentOrder" runat="server" CssClass="btn" OnClick="GTINOfficeSentOrderPaging"
                                    CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                <asp:Label ID="Label21" runat="server" Text="Page"></asp:Label>
                                <asp:DropDownList ID="ddlPagesGTINOfficeSentOrder" runat="server" Width="90px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlPagesGTINOfficeSentOrder_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="Label22" runat="server" Text="Of"></asp:Label>
                                <asp:Label ID="lblGTINOfficeSentOrderPages" runat="server" Text=""></asp:Label>
                                <asp:LinkButton ID="lnkNextGTINOfficeSentOrder" runat="server" CssClass="btn" OnClick="GTINOfficeSentOrderPaging"
                                    CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkLastGTINOfficeSentOrder" runat="server" CssClass="btn" OnClick="GTINOfficeSentOrderPaging"
                                    CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>

                                <asp:Button ID="btnCancelGTINOfficeSentOrders" runat="server" Text="Cancel" CssClass="btn"/>
                                    
                            </asp:Panel>
                        <%--</div>--%>
                        <%--</div>--%>

                            <!-- /Footer button -->
                                        <td>
                                            <asp:Button ID="btnSearchProduct" runat="server" Text="Search" CssClass="btn btn-primary"
                                                OnClick="btnSearchProduct_Click" Width="100px" Style="margin-bottom: 10px" visible="false" />
                                        </td>
                                    <%--</tr>--%>
                                </table>
                                <div class="span1">
                                </div>
                                <div class="span4">
                                </div>
                                <div class="span4">
                                </div>
                                <div class="span1">
                                </div>
                            <%--</asp:Panel>--%>
                        </div>
<%--                        <div class="row-fluid">
                        </div>--%>
                                           <%-- </div> <!-- I removed this -->--%>
                    <div class="modal-footer">
                        <div class="row-fluid" style="text-align: center">
                          <asp:Button ID="Button12" runat="server" Text="Button" Style="display: none" />
                        </div>
                    </div>
                    <asp:ModalPopupExtender ID="mpeSearchProduct" runat="server" CancelControlID="btnCancelGTINOfficeSentOrders"
                        TargetControlID="Button12" PopupControlID="pnlSearchProductPopUp" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                </asp:Panel> <!-- test this -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- End Search Products -->

    <div id="StorePopUp">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlStorePopUp" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 900px; height: 700px; display: none">
                    <div class="modal-header">
                        <h3>
                            Search Store
                        </h3>
                    </div>
                    <div class="modal-body" style="min-height: 550px">
                        <div class="row-fluid">
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnCustomerSearch">
                                <table>
                                    <tr>
                                        <td style="width: 100px">
                                            State:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStateForCustomerView" runat="server" DataTextField="StateName"
                                                DataValueField="SYSStateID" AutoPostBack="True" OnSelectedIndexChanged="btnCustomerSearch_Click">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustomerSearch" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCustomerSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                OnClick="btnCustomerSearch_Click" Width="100px" Style="margin-bottom: 10px" />
                                        </td>
                                    </tr>
                                </table>
                                <div class="span1">
                                </div>
                                <div class="span4">
                                </div>
                                <div class="span4">
                                </div>
                                <div class="span1">
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="row-fluid">
                            <asp:GridView ID="gvCustomerSelect" runat="server" CssClass="table table-condensed"
                                EmptyDataText="No Records Found" AutoGenerateColumns="False" GridLines="None"
                                DataKeyNames="ProviderCustomerCode" OnRowCommand="gvCustomerSelect_RowCommand"
                                OnRowDataBound="gvCustomerSelect_RowDataBound" Width="870px">
                                <Columns>
                                    <asp:TemplateField HeaderText="CustomerID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerIDSearch" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessNumber" HeaderText="BusinessNo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="154px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProviderCustomerCode" HeaderText="Code" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="99px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">  <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="216px" HorizontalAlign="Left"/>
                                            </asp:BoundField>
                                    <asp:BoundField DataField="StateName" HeaderText="State">  <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="313px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSearchCustomer" runat="server" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="88px" HorizontalAlign="Center" />
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
                            <asp:DropDownList ID="ddlCustomerPages" runat="server" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomerPages_SelectedIndexChanged">
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
                    top: 100px; width: 660px; height: 255px; display: none">
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
                                Quantity:
                            </div>
                            <div class="span3" style="width: 240px">
                                <asp:TextBox ID="txtChangeQuantity" runat="server" onkeypress="return validateQuantity(this, event);"></asp:TextBox>
                                <asp:Label ID="txtErrorMessageChangeQuantity" runat="server" Text="*" ForeColor="Red"
                                    Visible="false"></asp:Label>
                            </div>

                            <!-- Place third control here -->
                            <div class="span2" style="width: 50px">
                                Discount: 
                            </div>
                            <div class="span2" style="width: 100px">
                               <asp:TextBox ID="txtDiscountChangeQuantity" runat="server" Enabled="True" Width="130px" onkeypress="return validateDiscount(this, event);" ></asp:TextBox>
                                
                                <asp:RegularExpressionValidator Style="display: none" ForeColor="Red" Font-Size="Smaller" 
                                    ValidationGroup="savechangequantity" ControlToValidate="txtDiscountChangeQuantity" ID="DiscountChangeRegularExpressionValidator"
                                    runat="server" ErrorMessage="Discount must be between 0.00 and 99.99, and can't have more than 2 decimal places." ValidationExpression="^([0-9]{1,2}|[0-9]{1,2}[.][0-9]{1,2})$">
				                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="DiscountValidatorCalloutExtender" runat="server" TargetControlID="DiscountChangeRegularExpressionValidator" PopupPosition="Left">
                                </asp:ValidatorCalloutExtender>

                            </div>

                            <!-- -->

                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 100px">
                                Description:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtDescriptionChangeQty" runat="server" Enabled="False" Width="450px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveChangeQuantity" runat="server" Text="Ok" CssClass="btn btn-primary"
                            OnClick="btnSaveChangeQuantity_Click" ValidationGroup="savechangequantity" />
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
                            <asp:Label ID="lblOfficeNewRelease" runat="server"></asp:Label>                            
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
                    top: 100px; width: 650px; height: 250px; display: none;" DefaultButton="btnAddproductByProductCode">
                    <div class="modal-header">
                        <h3>
                            Add Product
                        </h3>
                    </div>
                    <div class="modal-body">
                        <div class="row-fluid">
                            <div class="span3" style="width: 100px">
                                Product Code:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtAddProductByProductCode" runat="server"  OnTextChanged="txtAddProductByProductCode_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtAddProductByProductCode"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtAddByProductCodeQty"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" Operator="DataTypeCheck"
                                    Type="String" ControlToValidate="txtAddProductByProductCode" ForeColor="Red"
                                    ErrorMessage="*" ValidationGroup="ProductByProductCode" ToolTip="Value must be a valid number" />
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span3" style="width: 100px">
                                Quantity:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtAddByProductCodeQty" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtAddByProductCodeQty"
                                    runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="ProductByProductCode"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                                    Type="Integer" ControlToValidate="txtAddByProductCodeQty" ForeColor="Red" ErrorMessage="*"
                                    ValidationGroup="ProductByProductCode" ToolTip="Value must be a valid number" />
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span3" style="width: 100px">
                                Description:
                            </div>
                            <div class="span5">
                                <asp:TextBox ID="txtProductDescriptionByProductCode" runat="server" Width="450px"
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
                    top: 100px; width: 900px; height: 600px; display: none">
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
                        <div class="row-fluid" style="min-height: 300px">
                            <asp:GridView ID="gvCreatedByUserIDs" runat="server" CssClass="table table-condensed"
                                GridLines="None" EmptyDataText="No Records Found" AutoGenerateColumns="False" OnRowDataBound="gvCreatedByUserIDs_RowDataBound"
                                OnRowCommand="gvCreatedByUserIDs_RowCommand" DataKeyNames="AccountID" Width="800">

                                <Columns>
                                    <asp:BoundField DataField="AccountID" HeaderText="AccountID" Visible="false" >
                                       <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LastName" HeaderText="LastName" >
                                       <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="FirstName" HeaderText="FirstName" >
                                       <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Username" HeaderText="Username" > 
                                       <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <!-- FOOTER -->
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
                        <!-- /FOOTER -->

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
        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidOrderID" runat="server" />
                <asp:HiddenField ID="hidOrderNumber" runat="server" />
                <asp:HiddenField ID="hidProductGTINCode" runat="server" />
                <asp:HiddenField ID="hidIsViewDetails" runat="server" />
                <asp:HiddenField ID="hidDiscount" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">

        function validateDiscountOnKeyDown(el, evt) {

            var curPos = el.selectionStart; 
            var dotPos = el.value.indexOf(".");

            // 
            if (curPos - 1 == dotPos && event.keyCode == 8 && el.value > 99.99) {
                return false;
            }

            if (el.value > 99.99) {
                return false;
            }

            //alert('curpos = ' + curPos + ', dotpos = ' + dotPos);
            return true;
        }
            
        function validateDiscount(el, evt) {

            // get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");

            // charcode is the currently pressed key
            var charCode = (evt.which) ? evt.which : event.keyCode;            

            // el is the previous value of the testfield prior to the current key pressed
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            // if no dot, restrict to only two chars
            if (number[0].length >= 2 && charCode != 46 && number.length == 1) {
                return false
            }

            // If there is a point, and length is 2, and the first part length is already 2,
            if (dotPos > 0 && number.length > 1 && number[0].length == 2 && number[1].length == 2) {
                return false;
            }

            // if there is one dot, and the left portion is 2-chars
            if (number.length > 1 && charCode == 46) {
                //if (number[0].length == 2) {
                    return false;
                //}                
            }

//            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
//                return false;
//            }

            return true;
        }


        function validateQuantity(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            //get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }

        //script from: http://javascript.nwbox.com/cursor_position/
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }

        // script from: http://asp.net-informations.com/ajax/calendarextender-validation.htm
        function verifyDate(sender, args) {
            var d = new Date();
            d.setDate(d.getDate() - 1);
            if (sender._selectedDate < d) {
                alert("Date should be Today or Greater than Today");
                sender._textbox.set_Value('')
            }
        }

        function disableKeyDown() {
            return false;
        }
        

    </script>

</asp:Content>
