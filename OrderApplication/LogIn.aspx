<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="OrderApplication.LogIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log In</title>
    <script src="Scripts/Jquery/js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="Scripts/CosmoStrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="Scripts/CosmoStrap/css/cosmo_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/CosmoStrap/css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .orderappBody
        {
            width: 100%;
            height: 600px;
            margin-top: 90px;
            background-color: White;
            box-shadow: 0px 10px 5px #888, 0px -10px 5px #888;
        }
        
        .effect7
        {
            position: relative;
            -webkit-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3), 0 0 40px rgba(0, 0, 0, 0.1) inset;
            -moz-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3), 0 0 40px rgba(0, 0, 0, 0.1) inset;
            box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3), 0 0 40px rgba(0, 0, 0, 0.1) inset;
        }
        .effect7:before, .effect7:after
        {
            content: "";
            position: absolute;
            z-index: -1;
            -webkit-box-shadow: 0 0 20px rgba(0,0,0,0.8);
            -moz-box-shadow: 0 0 20px rgba(0,0,0,0.8);
            box-shadow: 0 0 20px rgba(0,0,0,0.8);
            top: 0;
            bottom: 0;
            left: 10px;
            right: 10px;
            -moz-border-radius: 100px / 10px;
            border-radius: 100px / 10px;
        }
        .effect7:after
        {
            right: 10px;
            left: auto;
            -webkit-transform: skew(8deg) rotate(3deg);
            -moz-transform: skew(8deg) rotate(3deg);
            -ms-transform: skew(8deg) rotate(3deg);
            -o-transform: skew(8deg) rotate(3deg);
            transform: skew(8deg) rotate(3deg);
        }
    </style>
</head>
<body background="Resources/Images/7.jpg">
    <form id="form1" runat="server" autocomplete="off">
    <div id="orderappbody" class="orderappBody">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6 offset1" style="margin-top: 120px; border-right: 1px solid #e3e3e3;
                    height: 100%">
                    </br> </br>
                    <h1>
                        OrderLinc
                    </h1>
                    </br>
                </div>
                <div class="span4" style="margin-top: 150px;">
                    <div class="row-fluid">
                        <div class="span2">
                            Username:
                        </div>
                        <div class="span2" style="margin-left: 30px">
                            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span2" style="margin-top: 15px;">
                            Password:
                        </div>
                        <div class="span2" style="margin-left: 30px; margin-top: 15px;">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span7 offset2" style="margin-left: 95px">
                            <asp:Label ID="lblErrorMsg" runat="server" Text="Invalid Username or Password." ForeColor="Red"
                                Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span2 offset2" style="text-align: right; margin-left: 205px;">
                            <asp:Button ID="btnLogin" runat="server" Text="Log-In" CssClass="btn btn-large btn-primary"
                                Style="height: 50px; padding: 15px 30px;" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="row-fluid" style="margin-left: 220px">
                        <h2>
                            <small>Powered by: </small>
                        </h2>
                    </div>
                    <div class="row-fluid" style="margin-left: 140px">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Resources/Images/agilitilinc_logo.png"
                            Width="200px" />
                    </div>
                </div>
                <div class="span1">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
