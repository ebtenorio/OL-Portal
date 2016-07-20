<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="OrderApplication.Reports.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div class="page-header">
            <div class="row-fluid">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="40px" />
                <asp:Label ID="Label1" runat="server" Text="Report Export to CSV" Font-Size="20px"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <ul>
            <li>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbUser" runat="server" OnClick="lbUser_Click">User</asp:LinkButton></li>
            <li>
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbProducts" runat="server" OnClick="lbProducts_Click">Products and Products/Providers</asp:LinkButton></li>
            <li>
                <asp:Image ID="Image4" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbStores" runat="server" OnClick="lbStores_Click">Stores and Stores/Provider</asp:LinkButton></li>
            <li style="display:none">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbReleaseOrder" runat="server" OnClick="lbReleaseOrder_Click">Orders release in the previous day</asp:LinkButton></li>
            <li>
               <asp:Image ID="Image6" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbProvider" runat="server" OnClick="lbProvider_Click">Providers</asp:LinkButton></li>
            <li>
               <asp:Image ID="Image7" runat="server" ImageUrl="~/Resources/Images/csv.jpg" Width="20px" /> <asp:LinkButton ID="lbWarehouse" runat="server" OnClick="lbWarehouse_Click">Warehouse</asp:LinkButton></li>
        </ul>
    </div>
</asp:Content>
