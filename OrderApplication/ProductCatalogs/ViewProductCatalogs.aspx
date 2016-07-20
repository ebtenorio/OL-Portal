<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ViewProductCatalogs.aspx.cs" Inherits="OrderApplication.WebForm5"  Culture = "en-GB"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Jquery/js/jquery-1.8.3.js" type="text/javascript"></script>
    <div class="page-header">
        <div class="row-fluid">
           
              <asp:Label ID="Label8" runat="server" Text=" Manage Product" Font-Size="20px"></asp:Label>
                        
        </div>
    </div>
    <div class="row-fluid">
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div class="row-fluid">
                    <div class="span7">
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
                                    DataValueField="ProductGroupID" 
                                    onselectedindexchanged="ddlProductGroupSelect_SelectedIndexChanged">
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
                                <asp:TextBox ID="txtProductDescription" runat="server" Width="470px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtProductDescription" ToolTip="Don't leave this blank" ValidationGroup="Product"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="span5">
                        <div class="row-fluid" style="display: none">
                            <div class="span4">
                                End Date:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                <asp:CalendarExtender TargetControlID="txtEndDate" ID="CalendarExtender1" runat="server">
                                </asp:CalendarExtender>
                                <%--    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEndDate"
                                            age="*" ToolTip="Please input valid date" ForeColor="Red"
                                ValidationExpression="^[0-9m]{1,2}/[0-9d]{1,2}/[0-9y]{4}$"></asp:RegularExpressionValidator>--%>
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
                                    ControlToValidate="txtSKU" ValidationExpression="^[1-9]\d*$" Display="Static" EnableClientScript="true"
                                    ErrorMessage="Packing Units cannot be less than 1." runat="server" ValidationGroup="Product"
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
                <br />
                <div class="row-fluid" style=" min-height:200px">
                    <asp:GridView ID="gvProvider" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False"
                        GridLines="None" EmptyDataText="No record available" DataKeyNames="ProviderID,ProviderProductID"
                        AllowPaging="True" OnPageIndexChanging="gvProvider_PageIndexChanging"
                        OnRowDataBound="gvProvider_RowDataBound" 
                        onpageindexchanged="gvProvider_PageIndexChanged" Width="940px">
                        <Columns>
                            <asp:BoundField DataField="ProviderName" HeaderText="Provider" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="250px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="ProviderProductCode" HeaderText="Product Code" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="250px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Discount" DataFormatString="{0:F}" HeaderText="% Discount">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:d}" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="110px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:d}" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="110px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Update">
                                <ItemTemplate>
                                    <asp:ImageButton ID="img_updateProvider" runat="server" ImageUrl="~/Resources/Images/edit-26.png"
                                        Width="20px" OnClick="img_updateProvider_Click" />
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
    <div class="row-fluid">
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
                <asp:Button ID="btnProdClose" runat="server" Text="Cancel" CssClass="btn" 
                    onclick="btnProdClose_Click"  />
            </div>
        </div>
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
                    <div class="modal-body" style="height:280px">
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
                                             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtProductCodePop"
                                            ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="Please input an alphanumeric value." ValidationGroup="ProviderProduct"
                                            Style="display: none"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" TargetControlID="RegularExpressionValidator2" runat="server">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span4">
                                        % Discount :
                                    </div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtDiscount" runat="server" onkeypress="return validateDiscount(this, event);"></asp:TextBox>                                        

                                        <asp:RegularExpressionValidator Style="display: none" ForeColor="Red" Font-Size="Smaller" 
                                            ValidationGroup="ProviderProduct" ControlToValidate="txtDiscount" ID="DiscountRegularExpressionValidator"
                                            runat="server" ErrorMessage="Discount must be between 0.00 and 99.99, and can't have more than 2 decimal places." ValidationExpression="^([0-9]{1,2}[.][0-9]{1,2}|[0-9]{1,2})$">
				                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="DiscountValidatorCalloutExtender" runat="server" TargetControlID="DiscountRegularExpressionValidator">
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
                                        <asp:CalendarExtender TargetControlID="txtStartDatePop" ID="cbStartDateProd" runat="server" Format="dd/MM/yyyy">
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
                                        <asp:CalendarExtender TargetControlID="txtEndDatePop" ID="cbEndDateProd" runat="server" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    
                                       <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtStartDatePop" style="display:none" ControlToValidate="txtEndDatePop" Type="Date" Operator="GreaterThanEqual"  ErrorMessage="End date cannot be less than Start Date." runat="server" ValueToCompare="dd/MM/yyyy"  ValidationGroup="ProviderProduct"></asp:CompareValidator>
                                         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" TargetControlID="cmpVal1"
                                            runat="server" PopupPosition="Left">
                                        </asp:ValidatorCalloutExtender>
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

    <div id="UpdateProviderProductDIV">
        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlUpdateProviderProduct" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
                    top: 100px; width: 450px; height: 200px; display: none">
                    <div class="modal-header">
                        <h3>
                            Warning:
                        </h3>
                    </div>
                    <div class="modal-body">
                        <h4>
                            <asp:Label ID="lblUpdateProviderProduct" runat="server"></asp:Label><br />
                            <asp:Label ID="lblUpdateProviderProductConfirm" runat="server"></asp:Label><br />
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnUpdateProviderProductOK" runat="server" Text="Yes" CssClass="btn btn-primary"
                            OnClick="btnUpdateProviderProductOK_Click" />
                        <asp:Button ID="btnUpdateProviderProductCancel" runat="server" Text="No" CssClass="btn" />
                    </div>
                </asp:Panel>
                <asp:Button ID="Button15" runat="server" Text="Button" Style="display: none" />
                <asp:ModalPopupExtender ID="mpeUpdateProviderProduct" runat="server" CancelControlID="btnUpdateProviderProductCancel"
                    TargetControlID="Button15" PopupControlID="pnlUpdateProviderProduct" BackgroundCssClass="ShadedBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">

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

        //thanks: http://javascript.nwbox.com/cursor_position/
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }

        function fnConfirmDelete() {
            return confirm("Are you sure you want to delete this?");
        }
    </script>
    

</asp:Content>
