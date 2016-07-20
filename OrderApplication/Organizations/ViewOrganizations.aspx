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
                <asp:Label ID="Label5" runat="server" Text=" Manage Customers" Font-Size="20px"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="width: 70px; display: none;">
                                Provider:
                            </td>
                            <td style="width: 240px; display: none;">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row-fluid" style="min-height: 350px">
            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvCustomersView" runat="server" CssClass="table table-condensed"
                        AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvCustomersView_RowCommand"
                        DataKeyNames="CustomerID" OnRowDataBound="gvCustomersView_RowDataBound" EmptyDataText="No data available"
                        AllowPaging="True" Width="940px">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnSelectCustomer" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                        Width="20px" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <ItemStyle Width="70px" HorizontalAlign="Center" />
                            </asp:TemplateField>

                             <asp:BoundField DataField="BusinessNumber" HeaderText="Business No." >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="130px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="" HeaderText="Customer Code" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="290px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="340px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                                DataFormatString="{0:d}" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="110px" HorizontalAlign="Left" />
                            </asp:BoundField>

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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row-fluid" style="text-align: center">
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row-fluid">
            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnAdd" runat="server" Text="Add New Customer" CssClass="btn btn-primary"
                        OnClick="btnAdd_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
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
                                    <div class="span7" style="width: 500px">
                                        <div class="row-fluid" style="display: none">
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
                                            <div class="span8">
                                                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCustomerName"
                                                    ErrorMessage="Customer name is required." ForeColor="Red" ValidationGroup="CustomerDetails"
                                                    Style="display: none"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator3">
                                                </asp:ValidatorCalloutExtender>
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
                                        <div class="row-fluid" style="display: none">
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
                                                Address:
                                            </div>
                                            <div class="span7">
                                                <asp:TextBox ID="txtOfficeAddress" Width="330px" runat="server" ReadOnly="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                            </div>
                                            <div class="span4">
                                                <asp:LinkButton ID="lbManageAddress" runat="server" OnClick="lbManageAddress_Click">Manage Address</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5" style="margin-left: 30px">
                                        <div class="row-fluid">
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
                                                <div class="span4">
                                                    <asp:TextBox ID="txtEmail" Width="336px" runat="server" placeholder="Email"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="validateEmail" ToolTip="Invalid Email" runat="server"
                                                        ErrorMessage="*" ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                                        ValidationGroup="CustomerDetails" ForeColor="Red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblWarning" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="row-fluid">
                                    <asp:GridView ID="gvProviderCustomer" runat="server" CssClass="table table-condensed"
                                        AutoGenerateColumns="False" GridLines="None" DataKeyNames="ProviderID,ProviderCustomerID"
                                        EmptyDataText="No data records" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvProvider_PageIndexChanging"
                                        OnPageIndexChanged="gvProvider_PageIndexChanged" OnRowDataBound="gvProviderCustomer_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                            <asp:BoundField DataField="ProviderCustomerCode" HeaderText="Customer Code" />
                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:d}" />
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="img_updateCustomerProvider" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
                                                        Width="20px" OnClick="img_updateProvider_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-footer">
                <div class="row-fluid">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <div class="span6" style="text-align: left">
                                <asp:Button ID="btnAddProvider" runat="server" Text="Add Provider" CssClass="btn btn-primary"
                                    OnClick="btnAddProvider_Click" />
                                <asp:Label ID="lblEndDateError" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="span6" style="text-align: right">
                        <asp:Button ID="btnSaveCustomer" runat="server" Text="Save" CssClass="btn btn-primary"
                            ValidationGroup="CustomerDetails" OnClick="btnSaveCustomer_Click" />
                        <asp:Button ID="btnCancelCustomer1" runat="server" Text="Cancel" CssClass="btn" 
                             />
                    </div>
                </div>
            </div>
            <asp:Button ID="Button4" runat="server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeCustomer" runat="server" CancelControlID="btnCancelCustomer1"
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
    <div id="Div2">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAddresses" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 500px; display: none;">
                    <div class="modal-header">
                        <h3>
                            Manage Customer Addresses
                        </h3>
                    </div>
                    <div class="modal-body">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#pane1" data-toggle="tab">Office</a></li>
                               
                            </ul>
                            <div class="tab-content">
                                <div id="pane1" class="tab-pane active">
                                    <div class="row-fluid">
                                        <div class="span2">
                                            Address:
                                        </div>
                                        <div class="span10">
                                            <asp:TextBox ID="txtCustAddLine1" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" ValidationGroup="Address"
                                                ControlToValidate="txtCustAddLine1" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <asp:TextBox ID="txtCustAddLine2" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <div class="row-fluid">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtCustCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" ValidationGroup="Address"
                                                                ControlToValidate="txtCustCitySuburb" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCustPostal" Width="65px" runat="server" placeholder="Postal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
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
                                        </div>
                                    </div>
                                </div>
                               <%-- <div id="pane2" class="tab-pane">
                                    <div class="row-fluid">
                                        <div class="span2">
                                            Address:
                                        </div>
                                        <div class="span10">
                                            <asp:TextBox ID="txtBillToAddLine1" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" ValidationGroup="Address"
                                                ControlToValidate="txtBillToAddLine1" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <asp:TextBox ID="txtBillToAddLine2" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <div class="row-fluid">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtBillToCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" ValidationGroup="Address"
                                                                ControlToValidate="txtBillToCitySuburb" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBillToPostal" runat="server" Width="65px" placeholder="Postal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span4 offset2">
                                            <asp:DropDownList ID="ddlBillToState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                        </div>
                                    </div>
                                </div>
                                <div id="pane3" class="tab-pane">
                                    <div class="row-fluid">
                                        <div class="span2">
                                            Address:
                                        </div>
                                        <div class="span10">
                                            <asp:TextBox ID="txtShipToAddLine1" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Address"
                                                ControlToValidate="txtShipToAddLine1" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <asp:TextBox ID="txtShipToAddLine2" runat="server" CssClass="input-large" Width="300px"
                                                placeholder="Address 2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                            <div class="row-fluid">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtShipToCitySuburb" runat="server" placeholder="City Suburb"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtShipToCitySuburb"
                                                                ValidationGroup="Address" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtShipToPostal" runat="server" Width="65px" placeholder="Postal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span4 offset2">
                                            <asp:DropDownList ID="ddlShipToState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span10 offset2">
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveAddresses" runat="server" Text="Ok" CssClass="btn btn-primary"
                            OnClick="btnSaveAddresses_Click" ValidationGroup="Address" />
                        <asp:Button ID="btnCancelAddresses" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancelAddresses_Click" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button12" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button6" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeAddresses" runat="server" CancelControlID="Button6"
                    TargetControlID="Button12" PopupControlID="pnlAddresses" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="Div1">
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlProviderCustomer" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; display: none; width: 858px;">
                    <div class="modal-header">
                        <h3>
                            Manage Customer Code
                        </h3>
                    </div>
                    <div class="modal-body" style="height: 213px">
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="row-fluid">
                                    <div class="span4">
                                        Provider :
                                    </div>
                                    <div class="span8">
                                        <asp:DropDownList ID="ddlProvider" runat="server" DataTextField="ProviderName" DataValueField="ProviderID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="ddlProvider"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4">
                                        Customer Code :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtCustomerCodePop" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCustomerCodePop"
                                            ValidationExpression="^\d+$" ErrorMessage="Please input a numeric value." ValidationGroup="ProviderProduct"
                                            Style="display: none"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" TargetControlID="RegularExpressionValidator1"
                                            runat="server">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="row-fluid">
                                    <div class="span4">
                                        Start Date :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtStartDatePop" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="txtStartDatePop"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                            ControlToValidate="txtStartDatePop" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtStartDatePop" ID="CalendarExtender45" runat="server"
                                           >
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4">
                                        End Date :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtEndDatePop" runat="server" MaxLength="10"></asp:TextBox>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="txtEndDatePop"></asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                            ControlToValidate="txtEndDatePop" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20|99)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtEndDatePop" ID="CalendarExtender46" runat="server"
                                            >
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row-fluid" style="text-align: left">
                            <asp:Label ID="lblProviderProductErrorMessage" runat="server" Font-Size="Smaller"
                                ForeColor="#CC3300"></asp:Label>
                        </div>
                        <div class="row-fluid">
                            <asp:Button ID="btnSaveProviderCustomer" runat="server" Text="Ok" CssClass="btn btn-primary"
                                ValidationGroup="ProviderProduct" OnClick="btnSaveProviderCustomer_Click" />
                            <asp:Button ID="btnCancelCustomerProvider" runat="server" Text="Cancel" CssClass="btn"
                                OnClick="btnCancelCustomerProvider_Click " />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Button ID="Button10" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button11" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeCustomerProvider" runat="server" CancelControlID="Button11"
                    TargetControlID="Button10" PopupControlID="pnlProviderCustomer" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="Div3">
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlCustomerNotification" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblCustomerNotification" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnOKCustomer" runat="server" Text="Ok" CssClass="btn btn-primary" />
                        <asp:Button ID="btnCancelGTINChanged" runat="server" Text="Cancel" CssClass="btn"
                            Style="display: none" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button7" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeCustomerNotification" runat="server" CancelControlID="btnOKCustomer"
                    TargetControlID="btnOKCustomer" PopupControlID="pnlCustomerNotification" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="HiddenControls">
        <asp:TextBox ID="txtSalesRepID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCustomerID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtSYSStateID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtShipToAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtBillToAddressID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtContactID" runat="server" Visible="false"></asp:TextBox>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidCustomerID" runat="server" />
                <asp:HiddenField ID="hidProviderCustomerID" runat="server" />
                <asp:HiddenField ID="hidProviderCustomerTempID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
