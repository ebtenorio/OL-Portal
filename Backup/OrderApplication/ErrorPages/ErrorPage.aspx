<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="OrderApplication.ErrorPages.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../Scripts/Jquery/js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="../Scripts/CosmoStrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="../Scripts/CosmoStrap/css/cosmo_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/CosmoStrap/css/bootstrap-responsive.css" rel="stylesheet"
        type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="page-header">
            <h1>
                <small>
                    <asp:Label ID="Label2" runat="server" Text="Error Has Occurred." ForeColor="Black"></asp:Label></small></h1>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <h2>
                    <small> 
                        <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="An unexpected error occurred on our website. The website administrator has been
                        notified."></asp:Label></small>
                </h2>
            </div>
        </div>
        <div class="row-fluid">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Dashboard.aspx">Return to Dashboard</asp:HyperLink>
        </div>
        <div class="row-fluid" style="margin-top:250px">
        Powered by:
     
        </div>
        <div class="row-fluid">
               <asp:Image ID="Image1" runat="server" ImageUrl="~/Resources/Images/agilitilinc_logo.png"
                    Width="250px" />
        </div>
    </div>
    </form>
</body>
</html>
