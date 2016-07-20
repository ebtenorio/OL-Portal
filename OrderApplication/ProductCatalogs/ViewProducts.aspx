<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ViewProducts.aspx.cs" Inherits="OrderApplication.WebForm4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div class="row-fluid">
            <div class="page-header">
             <asp:Label ID="Label8" runat="server" Text=" Manage Products" Font-Size="20px"></asp:Label>
                
            </div>
        </div>
        <div class="row-fluid">
            <table>
                <tr>
                    <td style="width: 120px">
                        Product Group:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductGroup" runat="server" AutoPostBack="True" DataTextField="ProductGroupText"
                            DataValueField="ProductGroupID" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:ImageButton ID="imgbtnEditProdGroup" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
                            Width="20px" OnClick="imgbtnEditProdGroup_Click" Style="margin-top: -10px" />
                        <asp:ImageButton ID="img_btnDelete_Product" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                            Width="20px" OnClientClick="return confirm('Are you sure you want to delete this group?');"
                            OnClick="img_btnDelete_Product_Click1" Style="margin-top: -10px" Visible="false" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkbtnAddProdGroup" runat="server" OnClick="lnkbtnAddProdGroup_Click">Add Product Group</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="row-fluid" style="min-height: 350px">
            <asp:GridView ID="gvProducts" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False"
                GridLines="None" OnRowCommand="gvProducts_RowCommand" DataKeyNames="ProductID"
                EmptyDataText="No record available" OnRowDataBound="gvProducts_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnViewProduct" runat="server" ImageUrl="~/Resources/Images/about-26.png"
                                Width="20px" CommandName="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ProductID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblProductID" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ProductID") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GTINCode" HeaderText="GTIN Code" >
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" 
                        Visible="False" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductDescription" HeaderText="Description" >
                    <HeaderStyle HorizontalAlign="Left" Width="500px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                        DataFormatString="{0:d}" >
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Delete" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="img_btnDelete_Product" runat="server" ImageUrl="~/Resources/Images/delete-26.png"
                                Width="20px" OnClientClick="return confirm('Are you sure you want to delete this product?');"
                                OnClick="img_btnDelete_Product_Click" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="row-fluid" style="text-align: center">
            <asp:Panel ID="ProductsPanel" runat="server">
                <asp:LinkButton ID="lnkbtnProductsFirst" runat="server" CssClass="btn" OnClick="ProductsPaging"
                    CommandName="First"><i class="icon-backward"></i></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnProductsPrev" runat="server" CssClass="btn" OnClick="ProductsPaging"
                    CommandName="Previous"><i class="icon-chevron-left"></i></asp:LinkButton>
                <asp:Label ID="Label11" runat="server" Text="Page"></asp:Label>
                <asp:DropDownList ID="ddlProductsPages" runat="server" Width="90px" AutoPostBack="True"
                    Style="margin-top: 10px" 
                    onselectedindexchanged="ddlProductsPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" Text="Of"></asp:Label>
                <asp:Label ID="lblProductsPages" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="lnkbtnProductsNext" runat="server" CssClass="btn" OnClick="ProductsPaging"
                    CommandName="Next"><i class="icon-chevron-right"></i></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnProductsLast" runat="server" CssClass="btn" OnClick="ProductsPaging"
                    CommandName="Last"><i class="icon-forward" ></i></asp:LinkButton>
            </asp:Panel>
        </div>
        <div class="row-fluid">
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-primary"
                        OnClick="btnAddProduct_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="ProductPopUp">
        <asp:Panel ID="pnlProduct" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 100px; width: 860px; display: none;" DefaultButton="btnSavePRoduct">
            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <h3>
                            <asp:Label ID="lblProduct" runat="server" Text="Add Product"></asp:Label>
                        </h3>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-body" style="min-height: 210px;">
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="row-fluid">
                                    <div class="span3 ">
                                        <asp:Label ID="Label1" runat="server" Text="GTIN Code:"></asp:Label>
                                    </div>
                                    <div class="span9">
                                        <asp:TextBox ID="txtGTINCode" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator58" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtGTINCode" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator Style="display: none" ForeColor="Red" Font-Size="Smaller"
                                            ValidationGroup="Product" ControlToValidate="txtGTINCode" ID="RegularExpressionValidator5"
                                            runat="server" ErrorMessage="GTIN must be 8, 12, 13 or 14 digits." ValidationExpression="^([0-9]{8}|[0-9]{12}|[0-9]{13}|[0-9]{14})$"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender11" runat="server" TargetControlID="RegularExpressionValidator5">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span3 ">
                                        <asp:Label ID="Label2" runat="server" Text="Product Group:"></asp:Label>
                                    </div>
                                    <div class="span9 ">
                                        <asp:DropDownList ID="ddlProductGroupSelect" runat="server" DataTextField="ProductGroupText"
                                            DataValueField="ProductGroupID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <%--   <div class="row-fluid">
                                    <div class="span3 ">
                                        <asp:Label ID="Label8" runat="server" Text="Provider:"></asp:Label>
                                    </div>
                                    <div class="span9 ">
                                       
                                    </div>
                                </div>--%>
                                <div class="row-fluid" style="display: none">
                                    <div class="span3 ">
                                        <asp:Label ID="Label3" runat="server" Text="Sort Position:"></asp:Label>
                                    </div>
                                    <div class="span9">
                                        <asp:TextBox ID="txtSortPositionBox" runat="server"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Fill in Sort Position"
                                            ControlToValidate="txtSortPositionBox" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                            ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        <asp:Label ID="lblSortPositionW" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span3">
                                        <asp:Label ID="Label12" runat="server" Text="Product Description:"></asp:Label>
                                    </div>
                                    <div class="span9">
                                        <asp:TextBox ID="txtProductDescription" runat="server" Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtProductDescription" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="row-fluid" style="display: none">
                                    <div class="span4">
                                        End Date:
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender TargetControlID="txtEndDate" ID="CalendarExtender1" runat="server">
                                        </asp:CalendarExtender>
                                      <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEndDate"
                                           age="*" ToolTip="Please input valid date" ForeColor="Red" ValidationExpression="^[0-9m]{1,2}/[0-9d]{1,2}/[0-9y]{4}$"></asp:RegularExpressionValidator>--%>
                                    </div>
                                </div>
                                <div class="row-fluid" style="display: none">
                                    <div class="span4 ">
                                        <asp:Label ID="Label6" runat="server" Text="Product Code:"></asp:Label>
                                    </div>
                                    <div class="span8 ">
                                        <asp:TextBox ID="txtProdCode" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4 ">
                                        <asp:Label ID="Label5" runat="server" Text="Packing Units:"></asp:Label>
                                    </div>
                                    <div class="span8 ">
                                        <asp:TextBox ID="txtSKU" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtSKU" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Style="display: none"
                                            ControlToValidate="txtSKU" ValidationExpression="\d+" Display="Static" EnableClientScript="true"
                                            ErrorMessage="Must be numeric value." runat="server" ValidationGroup="Product"
                                            ForeColor="Red" />
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender113" runat="server" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4 ">
                                        <asp:Label ID="Label7" runat="server" Text="UOM:"></asp:Label>
                                    </div>
                                    <div class="span8">
                                        <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="ProductUOM" DataValueField="ProductUOMID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                            ControlToValidate="ddlUOM" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <asp:Label ID="lblGTINProdW" runat="server" Text="Please Input GTIN or Product Code"
                                ForeColor="Red" Visible="false" Style="margin-left: 65px"></asp:Label>
                        </div>
                        <div class="row-fluid">
                            <asp:GridView ID="gvProvider" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False"
                                GridLines="None" EmptyDataText="No record available" DataKeyNames="ProviderID,ProviderProductID"
                                AllowPaging="True" PageSize="10" 
                                OnPageIndexChanging="gvProvider_PageIndexChanging" 
                                onrowdatabound="gvProvider_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                                    <asp:BoundField DataField="ProviderProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:d}" />
                                    <asp:TemplateField HeaderText="Update">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img_updateProvider" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
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
            <div class="modal-footer">
                <div class="row-fluid" style="text-align: left">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblProductErrorMessage1" runat="server" Text="" Font-Size="Small"
                                ForeColor="#CC3300"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="row-fluid">
                    <div class="span6" style="text-align: left">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnAddProvider" runat="server" Text="Add Provider" CssClass="btn btn-primary"
                                    OnClick="btnAddProvider_Click" />
                                <asp:Label ID="lblEndDateError" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="span6" style="text-align: right">
                        <asp:Button ID="btnSavePRoduct" runat="server" Text="Save" CssClass="btn btn-primary"
                            ValidationGroup="Product" OnClick="btnSavePRoduct_Click" />
                        <asp:Button ID="btnProdClose" runat="server" Text="Close" CssClass="btn" />
                    </div>
                </div>
            </div>
            <asp:ModalPopupExtender ID="mpeProduct" runat="server" PopupControlID="pnlProduct"
                BackgroundCssClass="ShadedBackground" TargetControlID="Button4" CancelControlID="btnProdClose">
            </asp:ModalPopupExtender>
            <asp:Button ID="Button4" runat="server" Style="display: none" />
        </asp:Panel>
    </div>
    <div id="ProductGroupPopUp">
        <asp:Panel ID="pnlProductGroup" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 100px; width: 450px; display: none">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <h3>
                            <asp:Label ID="lblProductGroupHeader" runat="server" Text="Add Product Group"></asp:Label>
                        </h3>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-body">
                <div class="row-fluid">
                    <div class="span1" style="width: 100px">
                        Group Name:
                    </div>
                    <div class="span3">
                        <asp:TextBox ID="txtProductGroupName" runat="server" Width="280px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ForeColor="Red" ValidationGroup="ProductGroup" ControlToValidate="txtProductGroupName"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-fluid" style="display:none">
                    <div class="span1" style="width: 100px">
                        Sort Position:
                    </div>
                    <div class="span3">
                        <asp:TextBox ID="txtSortPosition" runat="server" Text="0" Width="280px"></asp:TextBox>
                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                            ForeColor="Red" ValidationGroup="ProductGroup" ControlToValidate="txtSortPosition"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSaveProductGroup" runat="server" Text="Save" CssClass="btn btn-primary"
                    OnClick="btnSaveProductGroup_Click" ValidationGroup="ProductGroup" />
                <asp:Button ID="btnCancelGroup" runat="server" Text="Cancel" CssClass="btn" />
            </div>
            <asp:Button ID="Button3" runat="server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeProductGroup" runat="server" CancelControlID="btnCancelGroup"
                PopupControlID="pnlProductGroup" TargetControlID="Button3" BackgroundCssClass="ShadedBackground">
            </asp:ModalPopupExtender>
        </asp:Panel>
    </div>
    <div id="ProductGroupAlreadyExist">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlProductGroupAlreadyExist" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblErrorMessageProductGroup" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAlreadyExist" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button5" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeProductGroupAlreadyExist" runat="server" CancelControlID="btnAlreadyExist"
                    TargetControlID="Button1" PopupControlID="pnlProductGroupAlreadyExist">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ProductAlreadyExistOnGroup">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlProductAlreadyExistOnGroup" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 205px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblProductErrorMessage" runat="server" Text="Label"></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnProductAlreadyExistOnGroup" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button6" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button7" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeProductAlreadyExistOnGroup" runat="server" CancelControlID="btnProductAlreadyExistOnGroup"
                    TargetControlID="Button6" PopupControlID="pnlProductAlreadyExistOnGroup" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="GTINProductCodeAlreadyExist">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlGTINProductCodeAlreadyExist" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 175px; display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            GTIN Code and/or Product Code already in use elsewhere
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGTINProductCodeAlreadyExist" runat="server" Text="Ok" CssClass="btn btn-primary" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button8" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button9" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeGTINProductCodeAlreadyExist" runat="server" CancelControlID="btnGTINProductCodeAlreadyExist"
                    TargetControlID="Button8" PopupControlID="pnlGTINProductCodeAlreadyExist">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

     <div id="Div2">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlGTINChangeNotification" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px;  display: none">
                    <div class="modal-header">
                        <h3>
                            Notification
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>                        
                         <asp:Label ID="lblGtINChangeNotification" runat="server"  Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveGTINChanged" runat="server" Text="Yes" 
                            CssClass="btn btn-primary" onclick="btnSaveGTINChanged_Click" />
                        <asp:Button ID="btnCancelGTINChanged" runat="server" Text="Cancel"  CssClass="btn"  />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button12" runat="server" Text="Button" Style="display: none" />
                
                <asp:ModalPopupExtender ID="mpeGTINChanged" runat="server" CancelControlID="btnCancelGTINChanged"
                    TargetControlID="Button12" PopupControlID="pnlGTINChangeNotification" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <div id="Div1">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlProviderProduct" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; display: none; width: 858px;">
                    <div class="modal-header">
                        <h3>
                            Manage Product Code
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
                                        Product Code :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtProductCodePop" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                            ForeColor="Red" ValidationGroup="ProviderProduct" ControlToValidate="txtProductCodePop" ToolTip="Product Code must be entered"></asp:RequiredFieldValidator>
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
                                            ControlToValidate="txtStartDatePop" ValidationExpression="^(0[1-9]|1[012])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtStartDatePop" ID="CalendarExtender45" runat="server" Format="MM/dd/yyyy">
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
                                            ControlToValidate="txtEndDatePop" ValidationExpression="^(0[1-9]|1[012])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20|99)\d\d$"
                                            Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="ProviderProduct"
                                            ToolTip="Please input a valid date"></asp:RegularExpressionValidator>
                                        <asp:CalendarExtender TargetControlID="txtEndDatePop" ID="CalendarExtender46" runat="server" Format="MM/dd/yyyy">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row-fluid" style="text-align:left">
                            <asp:Label ID="lblProviderProductErrorMessage" runat="server" 
                                Font-Size="Smaller" ForeColor="#CC3300"></asp:Label>
                        </div>
                        <div class="row-fluid">
                            <asp:Button ID="btnSaveProviderProduct" runat="server" Text="Ok" CssClass="btn btn-primary"
                                OnClick="btnSaveProviderProduct_Click" ValidationGroup="ProviderProduct" />
                            <asp:Button ID="btnCancelProductProvider" runat="server" Text="Cancel" CssClass="btn" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Button ID="Button10" runat="server" Text="Button" Style="display: none" />
                <asp:Button ID="Button11" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeProductProvider" runat="server" CancelControlID="btnCancelProductProvider"
                    TargetControlID="Button10" PopupControlID="pnlProviderProduct" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="HiddenValues">
        <asp:TextBox ID="txtProductID" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtProductGroupLineID" runat="server" Visible="false"></asp:TextBox>
        <asp:UpdatePanel ID="UpdatePanel98" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidProviderID" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hidProviderTempID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
