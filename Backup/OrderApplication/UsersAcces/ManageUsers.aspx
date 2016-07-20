<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="OrderApplication.WebForm2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="ViewUsers" runat="server">
                <div class="row-fluid">
                    <div class="page-header">
                        <h1>
                            <small>
                                <asp:Label ID="lblTitle" runat="server" Text="Manage Users"></asp:Label></small></h1>
                    </div>
                </div>
                <div class="row-fluid">
                    <table>
                        <tr>
                            <td style="width: 150px">
                                Organization Unit:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOrganizationUnitSelect" runat="server" AutoPostBack="True"
                                    DataValueField="OrgUnitID" DataTextField="OrgUnitName" OnSelectedIndexChanged="ddlOrganizationUnitSelect_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchUserText" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearchUserText" runat="server" Text="Search" CssClass="btn btn-primary"
                                    OnClick="btnSearchCustomerText_Click" Style="margin-bottom: 10px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row-fluid" style="min-height: 300px">
                    <asp:GridView ID="gvAccounts" class="table table-condensed" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="AccountID" EmptyDataText="No records to show" GridLines="None"
                        OnRowCommand="gvAccounts_RowCommand" OnRowDeleting="gvAccounts_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" Width="20px" ImageUrl="~/Resources/Images/about-26.png"
                                        CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountID" runat="server" Text='<%# Bind("AccountID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("AccountID") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastName" HeaderText="Last Name">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FirstName" HeaderText="First Name">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Username" HeaderText="Username">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" Visible="False">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Assign">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnAssign" runat="server" ImageUrl="~/Resources/Images/redo.png"
                                        Width="20px" CommandName="Assign" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" Visible="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" Width="20px" OnClientClick="return confirm('Are you sure you want to delete this User?');"
                                        ImageUrl="~/Resources/Images/delete-26.png" CommandName="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        OnClick="ImageButton2_Click" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AccountType" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountTypeID" runat="server" Text='<%# Bind("AccountTypeID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("AccountTypeID") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" Visible="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="img_btnDelete_User" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                        CommandName="Del" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Width="20px" OnClientClick="return confirm('Are you sure you want to delete this user?');"
                                        OnClick="img_btnDelete_User_Click" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="50px" HorizontalAlign="Center" />
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
                        <asp:Label ID="Label11" runat="server" Text="Page"></asp:Label>
                        <asp:DropDownList ID="ddlAccountsPages" runat="server" Width="90px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlAccountsPages_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="Label4" runat="server" Text="Of"></asp:Label>
                        <asp:Label ID="lblAccountsPages" runat="server" Text=""></asp:Label>
                        <asp:LinkButton ID="lnkbtnAccountsNext" runat="server" CssClass="btn" OnClick="AccountsPaging"
                            CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnAccountsLast" runat="server" CssClass="btn" OnClick="AccountsPaging"
                            CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                    </asp:Panel>
                </div>
                <div class="row-fluid">
                    <div class="span4">
                        <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="btn btn-primary"
                            OnClick="btnAddUser_Click" />
                    </div>
                </div>
                <div class="row-fluid">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red" Font-Size="Small"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:View>
            <asp:View ID="ManageUser" runat="server">
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSaveUser">
                    <br />
                </asp:Panel>
            </asp:View>
            <asp:View ID="AssignSalesReps" runat="server">
                <div class="row-fluid" style="height: 60px">
                    <div class="page-header">
                        <h1>
                            <small>
                                <asp:Label ID="Label1" runat="server" Text="Manage Sales Reps"></asp:Label></small></h1>
                    </div>
                </div>
                <div class="row-fluid">
                    <h3>
                        <asp:Label ID="lblSalesRepName" runat="server" Text="Chan, Steven"></asp:Label>
                    </h3>
                </div>
                <div class="row-fluid">
                    <table>
                        <tr>
                            <td style="width: 50px">
                                State:
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlStateForCustomerView" runat="server" AutoPostBack="True"
                                            DataTextField="StateName" DataValueField="SYSStateID" OnSelectedIndexChanged="ddlStateForCustomerView_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCustomerSearch" runat="server"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnCustomerSearch" runat="server" CssClass="btn btn-primary" OnClick="btnCustomerSearch_Click"
                                    Text="Search" Width="110px" Style="margin-bottom: 10px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row-fluid">
                    <div class="span6">
                        <div class="row-fluid">
                            <strong>Customer List </strong>
                        </div>
                        <div class="row-fluid" style="min-height: 350px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvCustomersView" runat="server" CssClass="table table-condensed"
                                        AutoGenerateColumns="False" GridLines="None" EmptyDataText="No Records Found"
                                        OnRowCommand="gvCustomersView_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="BusinessNumber" HeaderText="Code" />
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="StateName" HeaderText="State" />
                                            <asp:TemplateField HeaderText="Assign">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnAssignCustomer" runat="server" ImageUrl="~/Resources/Images/redo.png"
                                                        Width="20px" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CustomerID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row-fluid" style="text-align: center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlCustomerView" runat="server">
                                        <asp:LinkButton ID="lnkbtnFirstCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                                            CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnPrevCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                                            CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                        <asp:Label ID="Label3" runat="server" Text="Page"></asp:Label>
                                        <asp:DropDownList ID="ddlCustomerViewPages" runat="server" Width="90px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCustomerViewPages_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label2" runat="server" Text="Of"></asp:Label>
                                        <asp:Label ID="lblCustomerViewPages" runat="server" Text=""></asp:Label>
                                        <asp:LinkButton ID="lnkbtnNextCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                                            CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnLastCustomer" runat="server" CssClass="btn" OnClick="CustomerViewPaging"
                                            CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="span6">
                        <div class="row-fluid">
                            <strong>Assigned Customers </strong>
                        </div>
                        <div class="row-fluid" style="min-height: 350px">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvAssignedCustomers" runat="server" CssClass="table table-condensed"
                                        GridLines="None" AutoGenerateColumns="False" EmptyDataText="No Records Found"
                                        OnRowCommand="gvAssignedCustomers_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="BusinessNumber" HeaderText="Code" />
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="StateName" HeaderText="State" />
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnRemoveCustomer" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
                                                        Width="20px" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CustomerID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AccountSalesRepID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerSalesRepID" runat="server" Text='<%# Bind("CustomerSalesRepID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SalesRepAccountID") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row-fluid" style="text-align: center">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlAssignedCustomer" runat="server">
                                        <asp:LinkButton ID="lnkbtnAssignedCustomerFirst" runat="server" CssClass="btn" OnClick="AssignedCustomerPaging"
                                            CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnAssignedCustomerPrev" runat="server" CssClass="btn" OnClick="AssignedCustomerPaging"
                                            CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                                        <asp:Label ID="Label5" runat="server" Text="Page"></asp:Label>
                                        <asp:DropDownList ID="ddlAssignedCustomerPages" runat="server" Width="90px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlAssignedCustomerPages_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label6" runat="server" Text="Of"></asp:Label>
                                        <asp:Label ID="lblAssignedCustomerPages" runat="server" Text=""></asp:Label>
                                        <asp:LinkButton ID="lnkbtnAssignedCustomerNext" runat="server" CssClass="btn" OnClick="AssignedCustomerPaging"
                                            CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnAssignedCustomerLast" runat="server" CssClass="btn" OnClick="AssignedCustomerPaging"
                                            CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="row-fluid">
                    <asp:Button ID="btnBackToUserList" runat="server" Text="Back" CssClass="btn btn-primary"
                        Width="100px" OnClick="btnBackToUserList_Click" />
                </div>
            </asp:View>
        </asp:MultiView>
        <div id="Notification">
            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlNotificationAssign" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; height: 175px; display: none">
                        <div class="modal-header">
                            <h3>
                                Alert
                            </h3>
                        </div>
                        <div class="modal-body">
                            <h4>
                                Are you sure you want to assign Customer?
                            </h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnNotificationAssignOk" runat="server" Text="Ok" CssClass="btn btn-primary"
                                OnClick="btnNotificationAssignOk_Click" />
                            <asp:Button ID="btnNotificationAssignCancel" runat="server" Text="Cancel" CssClass="btn" />
                        </div>
                    </asp:Panel>
                    <asp:Button ID="Button15" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeNotificationAssign" runat="server" CancelControlID="btnNotificationAssignCancel"
                        TargetControlID="Button15" PopupControlID="pnlNotificationAssign">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="AlreadyAssignedNotification">
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlAlreadyAssigned" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; height: 175px; display: none">
                        <div class="modal-header">
                            <h3>
                                Notification
                            </h3>
                        </div>
                        <div class="modal-body">
                            <h4>
                                Customer is Already on the List
                            </h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="AlreadyAssignedOK" runat="server" Text="Ok" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>
                    <asp:Button ID="Button13" runat="server" Text="Button" Style="display: none" />
                    <asp:Button ID="Button14" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeAlreadyAssigned" runat="server" CancelControlID="AlreadyAssignedOK"
                        TargetControlID="Button14" PopupControlID="pnlAlreadyAssigned">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="ManageUserNotification">
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlManageUserNotification" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; height: 175px; display: none">
                        <div class="modal-header">
                            <h3>
                                Notification
                            </h3>
                        </div>
                        <div class="modal-body">
                            <h4>
                                <asp:Label ID="txtManageUserNotification" runat="server" Text="Label"></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnManageUserNotif" runat="server" Text="Ok" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>
                    <asp:Button ID="Button4" runat="server" Text="Button" Style="display: none" />
                    <asp:Button ID="Button5" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeManageUserNotif" runat="server" CancelControlID="btnManageUserNotif"
                        TargetControlID="Button4" PopupControlID="pnlManageUserNotification">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="ManageUserAccounts">
            <asp:Panel ID="pnlManageUserAccount" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                top: 100px; width: 950px; display: none; z-index: 99">
                <div class="modal-header">
                    <h3>
                        Manage User
                    </h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <div class="modal-body" style="min-height: 430px">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Account Type:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlAccountType" runat="server" DataTextField="AccountTypeText"
                                                        DataValueField="AccountTypeID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Organization Unit:
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="ddlOrganizationUnit" runat="server" DataTextField="OrgUnitName"
                                                        DataValueField="OrgUnitID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    End Date:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender TargetControlID="txtEndDate" ID="CalendarExtender1" runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                        ErrorMessage="*" ToolTip="Please input valid date" ForeColor="Red" ValidationExpression="^[0-9m]{1,2}/[0-9d]{1,2}/[0-9y]{4}$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Username:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtUsernameMangeUser" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                                        ForeColor="Red" ControlToValidate="txtUsernameMangeUser" ValidationGroup="Manage"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Password:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtPasswordMangeUser" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                        ForeColor="Red" ControlToValidate="txtPasswordMangeUser" ValidationGroup="Manage"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Last Name:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                                        ForeColor="Red" ControlToValidate="txtLastName" ValidationGroup="Manage"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    First Name:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                                        ForeColor="Red" ControlToValidate="txtFirstName" ValidationGroup="Manage"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                    Contact:
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtMobile" runat="server" placeholder="Mobile"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtFax" runat="server" placeholder="Fax"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2" style="width: 135px">
                                                </div>
                                                <div class="span3" style="width: 250px">
                                                    <asp:TextBox ID="txtEmail" Width="320px" runat="server" placeholder="Email"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="row-fluid">
                                                <div class="span2">
                                                    Address:
                                                </div>
                                                <div class="span3" style="width: 290px">
                                                    <asp:TextBox ID="txtAddressLine1" runat="server" TextMode="MultiLine" Width="260px"
                                                        placeholder="Address Line 1"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span3 offset2" style="width: 270px">
                                                    <asp:TextBox ID="txtAddressLine2" runat="server" TextMode="MultiLine" Width="260px"
                                                        placeholder="Address Line 2"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2 offset2">
                                                    <asp:DropDownList ID="ddlSYSStateID" runat="server" Width="265px" DataValueField="SYSStateID"
                                                        DataTextField="StateName">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span2 offset2" style="width: 120px">
                                                    <asp:TextBox ID="txtCity" runat="server" Width="90px" placeholder="City Suburb"></asp:TextBox>
                                                </div>
                                                <div class="span3" style="width: 160px">
                                                    <asp:TextBox ID="txtPostalZipCode" runat="server" Width="120px" placeholder="Postal Code"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row-fluid" runat="server" id="div_deviceNo">
                                                <table>
                                                    <tr>
                                                        <td style="width:74px">
                                                            Device:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDeviceNo" runat="server" Width="250px" 
                                                                ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td >
                                                            <asp:Button ID="btnClearDeviceNo" runat="server" Text="Clear"  CssClass="btn" OnClick="btnClearDeviceNo_Click" style="margin-bottom:10px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <div class="row-fluid" style="text-align: left">
                        <div class="span8">
                            <asp:Label ID="lblManageUserError" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="span4" style="text-align: right">
                            <asp:Button ID="btnSaveUser" runat="server" Text="Save" CssClass="btn btn-primary"
                                Width="100px" OnClick="btnSaveUser_Click" ValidationGroup="Manage" />
                            <asp:Button ID="btnCancelUser" runat="server" Text="Cancel" CssClass="btn" OnClick="txtCancelUser_Click" />
                        </div>
                    </div>
                </div>
                <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeManageUserAccounts" runat="server" CancelControlID="btnCancelUser"
                    BackgroundCssClass="ShadedBackground" PopupControlID="pnlManageUserAccount" TargetControlID="Button1">
                </asp:ModalPopupExtender>
            </asp:Panel>
        </div>
        <div id="UserNameAlreadyExist">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlUserAlreadyExist" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; display: none">
                        <div class="modal-header">
                            <h3>
                                Notification
                            </h3>
                        </div>
                        <div class="modal-body">
                            <h4>
                                User Name already assigned – try another
                            </h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAlreadyExist" runat="server" Text="Ok" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>
                    <asp:Button ID="Button2" runat="server" Text="Button" Style="display: none" />
                    <asp:Button ID="Button3" runat="server" Text="Button" Style="display: none" />
                    <asp:ModalPopupExtender ID="mpeUserAlreadyExist" runat="server" CancelControlID="btnAlreadyExist"
                        TargetControlID="Button2" PopupControlID="pnlUserAlreadyExist" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="Div1">
            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel2" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                        top: 100px; width: 450px; display: none">
                        <div class="modal-header">
                            <h3>
                                Update
                            </h3>
                        </div>
                        <div class="modal-body" style="min-height: 200px">
                            <div class="row-fluid">
                                <div class="span4">
                                    <asp:Label ID="lblAssisnCustomerName" runat="server" Text="Customer Name:"></asp:Label>
                                </div>
                                <div class="span8">
                                    <asp:Label ID="lblAssignedCustomerName" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span4">
                                    <asp:Label ID="Label7" runat="server" Text="End Date:"></asp:Label>
                                </div>
                                <div class="span8">
                                    <asp:TextBox ID="txtSalesRepEndDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtSalesRepEndDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button6" runat="server" Text="Ok" CssClass="btn btn-primary" Style="display: none" />
                            <asp:Button ID="tbnUpdateEndDate" runat="server" Text="Update" OnClick="tbnUpdateEndDate_Click"
                                CssClass="btn btn-primary" />
                            <asp:Button ID="Button8" runat="server" Text="Cancel" CssClass="btn" />
                        </div>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="mpeUpdateSalesrepEndDate" runat="server" CancelControlID="Button8"
                        TargetControlID="Button6" PopupControlID="Panel2" BackgroundCssClass="ShadedBackground">
                    </asp:ModalPopupExtender>
                    <asp:HiddenField ID="hidCustomerSalesRepID" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="HiddenFields">
            <asp:TextBox ID="txtAccountID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtAddressID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtContactID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtCustomerID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtCustomerSalesRepID" runat="server" Visible="false"></asp:TextBox>
            <asp:HiddenField ID="hidServerID" runat="server" />
        </div>
    </div>
    <script type="text/javascript">


        $(function () {
            $('.fileupload').fileupload()
        });

    
    </script>
</asp:Content>
