<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="OrderApplication.Reports.Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row-fluid">
        <div class="page-header">
            <div class="row-fluid">
                <asp:Label ID="Label1" runat="server" Text="Log" Font-Size="20px"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <asp:GridView ID="gvLog" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False"
            GridLines="None">
            <Columns>
                <asp:BoundField DataField="LogID" HeaderText="LogID" >
                <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Source" HeaderText="Source" >
                <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated">
                <HeaderStyle Width="100px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
