<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ViewOrganizations.aspx.cs" Inherits="OrderApplication.WebForm6" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/Jquery/js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="../Scripts/CosmoStrap/js/bootstrap.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div class="row-fluid">
            <div class="page-header">
                <h1>
                    <small>Manage Customers</small>
                </h1>
            </div>
        </div>
        <div class="row-fluid">
            <table>
                <tr>
                    <td style="width: 70px">
                        Provider:
                    </td>
                    <td style="width: 240px">
                        <asp:DropDownList ID="ddlProvider" runat="server" DataTextField="ProviderName" DataValueField="ProviderID"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50px">
                        State:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStateForCustomerView" runat="server" DataTextField="StateName"
                            DataValueField="SYSStateID" AutoPostBack="True" OnSelectedIndexChanged="ddlStateForCustomerView_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCustomerSearch" runat="server"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnCustomerSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                            Width="110px" OnClick="btnCustomerSearch_Click" Style="margin-bottom: 10px" />
                    </td>
                </tr>
            </table>
            <%--  <div class="span1">
            </div>
            <div class="span3">
            </div>
            <div class="span3">
            </div>
            <div class="span2">
            </div>--%>
        </div>
        <div class="row-fluid" style="min-height: 350px">
            <asp:GridView ID="gvCustomersView" runat="server" CssClass="table table-condensed"
                AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvCustomersView_RowCommand"
                DataKeyNames="CustomerID">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnSelectCustomer" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                Width="20px" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                    <asp:BoundField DataField="StateName" HeaderText="State" />
                    <asp:TemplateField HeaderText="CustomerID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="img_btnDelete_Customer" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                Width="20px" OnClientClick="return confirm('Are you sure you want to delete this customer?');"
                                OnClick="img_btnDelete_Customer_Click" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="row-fluid" style="text-align: center">
            <asp:Panel ID="pnlCustomerView" runat="server">
                <asp:LinkButton ID="lnkbtnFirstCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                    CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnPrevCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                    CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                <asp:Label ID="Label3" runat="server" Text="Page"></asp:Label>
                <asp:DropDownList ID="ddlCustomerViewPages" runat="server" Width="90px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCustomerViewPages_SelectedIndexChanged" Style="margin-top: 10px">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" Text="Of"></asp:Label>
                <asp:Label ID="lblCustomerViewPages" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="lnkbtnNextCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                    CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnLastCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                    CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
            </asp:Panel>
        </div>
        <div class="row-fluid">
            <asp:Button ID="btnAdd" runat="server" Text="Add New Customer" CssClass="btn btn-primary"
                OnClick="btnAdd_Click" />
        </div>
    </div>
    <div id="SaledOrgPopUp">
        <asp:Panel ID="pnlSalesOrg" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 200px; width: 850px; display: none;">
            <div class="modal-header">
                <h3>
                    <asp:Label ID="lblSalesOrdPanel" runat="server" Text="Add Sales Organization"></asp:Label>
                </h3>
            </div>
            <div class="modal-body">
                <div class="row-fluid">
                    <div class="span4">
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Sales Org Code:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtSalesOrgCode" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Sales Org Name:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtSalesOrgName" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Business No:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtBusinessNo" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Address One:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="input-large" TextMode="MultiLine"
                                    Width="205px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="span4" style="margin-left: 130px">
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                City Suburb:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtCitySubUrb" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                State:
                            </div>
                            <div class="span3">
                                <asp:DropDownList ID="ddlSYState" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Postal Zip Code:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtPostalZipCode" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 120px">
                                Address Two:
                            </div>
                            <div class="span3">
                                <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="input-large" TextMode="MultiLine"
                                    Width="205px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" />
            </div>
            <asp:ModalPopupExtender ID="mpeSalesOrg" runat="server" CancelControlID="btnCancel"
                PopupControlID="pnlSalesOrg" TargetControlID="Button1">
            </asp:ModalPopupExtender>
            <asp:Button ID="Button1" runat="server" Style="display: none" />
        </asp:Panel>
    </div>
    <div id="CustomerPopUp">
        <asp:Panel ID="pnlCustomer" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 200px; width: 950px; display: none;" DefaultButton="btnSaveCustomer">
            <div class="modal-header">
                <h3>
                    <asp:Label ID="Label1" runat="server" Text="Manage Customer"></asp:Label>
                </h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-body" style="max-height: 700px">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row-fluid">
                                    <div class="span6" style="width: 400px">
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Customer Code:
                                            </div>
                                            <div class="span3" style="width: 235px">
                                                <asp:TextBox ID="txtCustomerCode" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Business No:
                                            </div>
                                            <div class="span3" style="width: 235px">
                                                <asp:TextBox ID="txtBusinessNumberCust" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Customer Name:
                                            </div>
                                            <div class="span3" style="width: 235px">
                                                <asp:TextBox ID="txtCustomerName" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCustomerName"
                                                    ErrorMessage="*" ForeColor="Red" ValidationGroup="CustomerDetails"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <%--               <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Sales Rep:
                                            </div>
                                            <div class="span5" style="width: 195px">
                                                <asp:TextBox ID="txtSalesRepName" runat="server" Width="165px"></asp:TextBox>
                                            </div>
                                            <div class="span1">
                                                <asp:ImageButton ID="imgbtn_SalesRepSearch" runat="server" ImageUrl="~/Resources/Images/list-26.png"
                                                    OnClick="imgbtn_SalesRepSearch_Click" />
                                            </div>
                                        </div>--%>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                End Date:
                                            </div>
                                            <div class="span6" style="width: 235px">
                                                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                                <asp:CalendarExtender TargetControlID="txtEndDate" ID="CalendarExtender1" runat="server">
                                                </asp:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEndDate"
                                                    ErrorMessage="*" ToolTip="Please input valid date" ForeColor="Red" ValidationExpression="^[0-9m]{1,2}/[0-9d]{1,2}/[0-9y]{4}$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                State:
                                            </div>
                                            <div class="span3">
                                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                First Name:
                                            </div>
                                            <div class="span3" style="width: 235px">
                                                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Last Name:
                                            </div>
                                            <div class="span3" style="width: 235px">
                                                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Contact:
                                            </div>
                                            <div class="span2" style="width: 146px">
                                                <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                            </div>
                                            <div class="span2" style="width: 116px">
                                                <asp:TextBox ID="txtMobile" runat="server" placeholder="Mobile"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                            </div>
                                            <div class="span2" style="width: 116px">
                                                <asp:TextBox ID="txtFax" runat="server" placeholder="Fax"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                            </div>
                                            <div class="span2">
                                                <asp:TextBox ID="txtEmail" Width="320px" runat="server" placeholder="Email"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6" style="margin-left: 30px">
                                        <div class="row-fluid">
                                            <div class="tabbable">
                                                <ul class="nav nav-tabs">
                                                    <li class="active"><a href="#pane1" data-toggle="tab">Office</a></li>
                                                    <li><a href="#pane2" data-toggle="tab">Bill To</a></li>
                                                    <li><a href="#pane3" data-toggle="tab">Ship To</a></li>
                                                </ul>
                                                <div class="tab-content">
                                                    <div id="pane1" class="tab-pane active">
                                                        <div class="row-fluid">
                                                            <div class="span2">
                                                                Address:
                                                            </div>
                                                            <div class="span2">
                                                                <asp:TextBox ID="txtCustPostal" runat="server" placeholder="Postal"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtCustCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span4 offset2">
                                                                <asp:DropDownList ID="ddlCustState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtCustAddLine1" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtCustAddLine2" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="pane2" class="tab-pane">
                                                        <div class="row-fluid">
                                                            <div class="span2">
                                                                Address:
                                                            </div>
                                                            <div class="span2">
                                                                <asp:TextBox ID="txtBillToPostal" runat="server" placeholder="Postal"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtBillToCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span4 offset2">
                                                                <asp:DropDownList ID="ddlBillToState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtBillToAddLine1" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtBillToAddLine2" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 2"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="pane3" class="tab-pane">
                                                        <div class="row-fluid">
                                                            <div class="span2">
                                                                Address:
                                                            </div>
                                                            <div class="span2">
                                                                <asp:TextBox ID="txtShipToPostal" runat="server" placeholder="Postal"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtShipToCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span4 offset2">
                                                                <asp:DropDownList ID="ddlShipToState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtShipToAddLine1" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span2 offset2">
                                                                <asp:TextBox ID="txtShipToAddLine2" runat="server" CssClass="input-large" TextMode="MultiLine"
                                                                    Width="205px" placeholder="Address 2"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblWarning" runat="server" Text="Please Provide the Neccessary Data"
                                                        ForeColor="Red" Visible="false"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-footer">
                <div class="row-fluid">
                    <div class="span6" style="text-align: left">
                        <asp:Label ID="lblEndDateError" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="span6" style="text-align: right">
                        <asp:Button ID="btnSaveCustomer" runat="server" Text="Save" CssClass="btn btn-primary"
                            ValidationGroup="CustomerDetails" OnClick="btnSaveCustomer_Click" />
                        <asp:Button ID="btnCancelCustomer" runat="server" Text="Cancel" CssClass="btn" />
                    </div>
                </div>
            </div>
            <asp:Button ID="Button4" runat="server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeCustomer" runat="server" CancelControlID="btnCancelCustomer"
                BackgroundCssClass="ShadedBackground" PopupControlID="pnlCustomer" TargetControlID="Button4">
            </asp:ModalPopupExtender>
        </asp:Panel>
        <div id="SalesRepPopUp">
            <asp:Panel ID="pnlSalesRepSearch" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                top: 200px; width: 650px; height: 550px; display: none">
                <div class="modal-header">
                    <h3>
                        <asp:Label ID="Label2" runat="server" Text="Select SalesRep"></asp:Label>
                    </h3>
                </div>
                <div class="modal-body" style="min-height: 300px; max-height: 500px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row-fluid">
                                <div class="span5" style="width: 210px">
                                    <asp:TextBox ID="txtSalesRepSearch" runat="server" Width="200px"></asp:TextBox>
                                </div>
                                <div class="span2">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSalesRepSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                Width="100px" OnClick="btnSalesRepSearch_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="row-fluid" style="min-height: 300px">
                                <asp:GridView ID="gvSalesRepSearch" runat="server" CssClass="table table-condensed"
                                    GridLines="None" AutoGenerateColumns="False" EmptyDataText="No Records Found"
                                    OnRowCommand="gvSalesRepSearch_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="AccountID" HeaderText="AccountID" />
                                        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                        <asp:BoundField DataField="Username" HeaderText="Username" />
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtn_SelectSalesRepSearch" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Select</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row-fluid" style="text-align: center">
                                <asp:Panel ID="SalesRepPagingPanel" runat="server">
                                    <asp:LinkButton ID="lnkbtnSalesRepFirst" runat="server" CssClass="btn" OnClick="SalesRepPaging"
                                        CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepPrev" runat="server" CssClass="btn" OnClick="SalesRepPaging"
                                        CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                    <asp:Label ID="Label7" runat="server" Text="Page"></asp:Label>
                                    <asp:DropDownList ID="ddlSalesRepPages" runat="server" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSalesRepPages_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label8" runat="server" Text="Of"></asp:Label>
                                    <asp:Label ID="lblSalesRepPages" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="lnkbtnSalesRepNext" runat="server" CssClass="btn" OnClick="SalesRepPaging"
                                        CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnSalesRepLast" runat="server" CssClass="btn" OnClick="SalesRepPaging"
                                        CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button2" runat="server" Style="display: none" />
                    <asp:Button ID="btnSalesRepSearchClose" runat="server" Text="Close" CssClass="btn" />
                </div>
                <asp:ModalPopupExtender ID="mpeSalesRepSearch" runat="server" CancelControlID="btnSalesRepSearchClose"
                    PopupControlID="pnlSalesRepSearch" TargetControlID="Button2">
                </asp:ModalPopupExtender>
            </asp:Panel>
        </div>
        <div id="CustomerNameAlreadyExist">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlCustomerAlreadyExist" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; height: 175px; display: none">
                        <div class="modal-header">
                            <h3>
                                Notification
                            </h3>
                        </div>
                        <div class="modal-body">
                            <h4>
                                <asp:Label ID="lblErroMessageCustomer" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAlreadyExist" runat="server" Text="Ok" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>
                    <asp:Button ID="Button3" runat="server" Text="Button" Style="display: none" />
                    <asp:Button ID="Button5" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeCustomerAlreadyExist" runat="server" CancelControlID="btnAlreadyExist"
                        TargetControlID="Button3" PopupControlID="pnlCustomerAlreadyExist">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="HiddenControls">
        <asp:TextBox ID="txtSalesRepID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCustomerID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtSYSStateID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtShipToAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtBillToAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtContactID" runat="server" Visible="false"></asp:TextBox>
    </div>
</asp:Content>
