<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="Organization.aspx.cs" Inherits="OrderApplication.Organizations.Organization"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div class="row-fluid">
            <div class="page-header">
                <asp:Label ID="Label2" runat="server" Text=" Manage Customer" Font-Size="20px"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row-fluid">
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
                                            <div class="span7">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlState" runat="server" DataTextField="StateName" DataValueField="SYSStateID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnAssignSalesRep" runat="server" Text="Assign Sales Reps" CssClass="btn btn-primary"
                                                                Style="margin-bottom: 10px" OnClick="btnAssignSalesRep_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span2" style="width: 120px">
                                                Address:
                                            </div>
                                            <div class="span7">
                                                <asp:TextBox ID="txtOfficeAddress" Width="352px" runat="server" ReadOnly="True"></asp:TextBox>
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
                                                        ErrorMessage="*" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
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
                                        EmptyDataText="No data records" AllowPaging="True" OnPageIndexChanging="gvProvider_PageIndexChanging"
                                        OnPageIndexChanged="gvProvider_PageIndexChanged" OnRowCommand="gvProviderCustomer_RowCommand" OnRowDataBound="gvProviderCustomer_RowDataBound" Width="940">
                                        <Columns>

                                            <asp:BoundField DataField="ProviderName" HeaderText="Provider">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="178px" HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="ProviderCustomerCode" HeaderText="Customer Code">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="297px" HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:d}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="216px" HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:d}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="187px" HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="img_updateCustomerProvider" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
                                                        Width="20px" OnClick="img_updateProvider_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="62px" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Create Order">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxProviderCustomerCreateOrder" runat="server" ValidationGroup="ProviderCustomer" onclick="CheckBoxCheck(this);"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="62px" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row-fluid">
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
                        <asp:Button ID="btnCancelCustomer1" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancelCustomer1_Click" />
                    </div>
                </div>
            </div>
            <br />
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
                                <%--<li><a href="#pane2" data-toggle="tab">Bill To</a></li>
                                <li><a href="#pane3" data-toggle="tab">Ship To</a></li>--%>
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
                                <%--    <div id="pane2" class="tab-pane">
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
        <asp:UpdatePanel ID="UpdatePanel10" runat="server" OnPreRender="UpdatePanel10_PreRender">
            <ContentTemplate>
                <asp:Panel ID="pnlProviderCustomer" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; display: none; width: 858px;" DefaultButton="btnSaveProviderCustomer">
                    <div class="modal-header">
                        <h3>
                            Manage Customer Code
                        </h3>
                    </div>
                    <div class="modal-body" style="height: 250px">
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="row-fluid">
                                    <div class="span4">
                                        Provider :
                                    </div>
                                    <div class="span8">
                                        <asp:DropDownList ID="ddlProvider" runat="server" DataTextField="ProviderName" DataValueField="ProviderID"
                                            TabIndex="1">
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
                                        <asp:TextBox ID="txtCustomerCodePop" runat="server" TabIndex="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCustomerCodePop"
                                            ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="Please input an alphanumeric value."
                                            ValidationGroup="ProviderProduct" Style="display: none"></asp:RegularExpressionValidator>
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
                                        <asp:TextBox ID="txtStartDatePop" runat="server" MaxLength="10" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="txtStartDatePop"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                            ControlToValidate="txtStartDatePop" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20|99)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtStartDatePop" ID="cestartDate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4">
                                        End Date :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtEndDatePop" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="txtEndDatePop"></asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                            ControlToValidate="txtEndDatePop" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20|99)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtEndDatePop" ID="ceEnddate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtStartDatePop" Style="display: none"
                                            ControlToValidate="txtEndDatePop" Type="Date" Operator="GreaterThanEqual" ErrorMessage="End date cannot be less than Start Date."
                                            runat="server" ValueToCompare="dd/MM/yyyy" ValidationGroup="ProviderProduct"></asp:CompareValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" TargetControlID="cmpVal1"
                                            runat="server" PopupPosition="Left">
                                        </asp:ValidatorCalloutExtender>
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
                <asp:ModalPopupExtender ID="mpeCustomerProvider" runat="server" CancelControlID="btnCancelCustomerProvider"
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
    <div id="Div4">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlBusinessNumber" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblNotificationBusinessNumber" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveCustomerBusiness" runat="server" Text="Ok" CssClass="btn btn-primary"
                            OnClick="btnSaveCustomerBusiness_Click" />
                        <asp:Button ID="btnCancelCustomerBusiness" runat="server" Text="Cancel" CssClass="btn" />
                        <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="btn" Style="display: none" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button3" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeCheckBusinessNumber" runat="server" CancelControlID="btnCancelCustomerBusiness"
                    TargetControlID="Button1" PopupControlID="pnlBusinessNumber" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="Div5">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlCheckCustomer" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblCustomerNameIfExist" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCustomerExist" runat="server" Text="Yes" CssClass="btn btn-primary"
                            OnClick="btnCustomerExist_Click" />
                        <asp:Button ID="btnCancelCustomerExist" runat="server" Text="Cancel" CssClass="btn" />
                        <asp:Button ID="Button9" runat="server" Text="Cancel" CssClass="btn" Style="display: none" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button13" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeCustomerExist" runat="server" CancelControlID="Button9"
                    TargetControlID="Button9" PopupControlID="pnlCheckCustomer" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnlAssignSalesRep" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
        top: 100px; width: 450px; display: none" DefaultButton="btnSaveAssignSalesRep">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnPreRender="UpdatePanel4_PreRender">
            <ContentTemplate>
                <div class="modal-header">
                    <div class="row-fluid">
                        <div class="span6">
                            <h3>
                                Notification
                            </h3>
                        </div>
                        <div class="span6">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
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
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <h4>
                        <asp:Label ID="lblNotificationAssignSalesRep" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" Style="display: none"></asp:TextBox>
                    </h4>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveAssignSalesRep" runat="server" Text="Ok" CssClass="btn btn-primary"
                        OnClick="btnSaveAssignSalesRep_Click" />
                    <asp:Button ID="btnCancelAssignSalesRep" runat="server" Text="Cancel" CssClass="btn" />
                    <asp:Button ID="Button5" runat="server" Text="Cancel" CssClass="btn" Style="display: none" />
                </div>
                <asp:Button ID="Button8" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeAssignSalesReps" runat="server" CancelControlID="Button5"
                    TargetControlID="Button8" PopupControlID="pnlAssignSalesRep" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div class=" row-fluid">
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtSalesRepID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtCustomerID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtAddressID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtSYSStateID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtShipToAddressID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtBillToAddressID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtContactID" runat="server" Visible="false"></asp:TextBox>
                <asp:HiddenField ID="hidProviderID" runat="server" />
                <asp:HiddenField ID="hidCustomerID" runat="server" />
                <asp:HiddenField ID="hidProviderCustomerID" runat="server" />
                <asp:HiddenField ID="hidProviderCustomerTempID" runat="server" />
                <asp:HiddenField ID="hidIsAssignSalesRep" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function CheckBoxCheck(rb) {
            debugger;
            var gv = document.getElementById("<%=gvProviderCustomer.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].type == "checkbox") {
                    if (chk[i].checked && chk[i] != rb) {
                        chk[i].checked = false;
                        break;
                    }
                }
            }
        }    
    </script>

</asp:Content>
