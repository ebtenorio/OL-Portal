<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="ViewProductCatalogs.aspx.cs" Inherits="OrderApplication.WebForm5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/Jquery/js/jquery-1.8.3.js" type="text/javascript"></script>
    <div class="container">
        <div class="row-fluid">
            <div class="page-header">
                <h1>
                    <small>Product Catalogs</small></h1>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span3" style="border-right: 1px solid #e3e3e3; height: 500px;">
                <div class="row-fluid">
                    <div class="span1">
                        Catalogs:
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span1" style="min-height: 480px">
                        <asp:GridView ID="gvProductCatalogs" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span1">
                        <asp:Button ID="btnAddCatalog" runat="server" Text="Add Product Catalog" CssClass="btn btn-primary"
                            OnClick="btnAddCatalog_Click" />
                    </div>
                </div>
            </div>
            <div class="span9">
                <div class="row-fluid">
                    <asp:GridView ID="gvProducts" runat="server">
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div id="ProductCatalogPopUp">
        <asp:Panel ID="pnlProductCatalog" runat="server" CssClass="modal" Style="margin: -10px 0 0 30px;
            top: 200px; width: 1080px; display: none;">
            <div class="modal-header">
                <h3>
                    <asp:Label ID="lblProductCatalog" runat="server" Text="Add Product Catalog"></asp:Label>
                </h3>
            </div>
            <div class="modal-body">
                <div class="row-fluid">
                    <div class="span2">
                        Catalog Name:
                    </div>
                    <div class="span2" style="margin-left: -30px">
                        <asp:TextBox ID="txtProductCatalogName" runat="server" Width="245px"></asp:TextBox>
                    </div>
                    <div class="span5 offset1" style="margin-left: 150px">
                        <h3>
                            Products
                        </h3>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span5" style="border-right: 1px solid #e3e3e3; height: 350px;">
                        <div class="row-fluid">
                            <div class="span2" style="width: 45%">
                                Brand:
                            </div>
                            <div class="span2" style="width: 45%">
                                Category:
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2" style="width: 45%">
                                <asp:DropDownList ID="ddlBrands" runat="server" Width="180px">
                                </asp:DropDownList>
                            </div>
                            <div class="span2" style="width: 45%">
                                <asp:DropDownList ID="ddlCategories" runat="server" Width="180px">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <asp:GridView ID="gvProductsAvailable" runat="server" CssClass="table table-condense">
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="span8">
                               <asp:GridView ID="gvCatalogProducts" runat="server" CssClass="table table-condense">
                            </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSave" runat="server" Text="Save Catalog" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" />
            </div>
            <asp:ModalPopupExtender ID="mpeProductCatalog" runat="server" TargetControlID="btnTarget"
                CancelControlID="btnCancel" PopupControlID="pnlProductCatalog">
            </asp:ModalPopupExtender>
            <asp:Button ID="btnTarget" runat="server" Style="display: none" />
        </asp:Panel>
    </div>
    
</asp:Content>
