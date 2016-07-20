<%@ Page Title="" Language="C#" MasterPageFile="~/OrderApplicationMaster.Master"
    AutoEventWireup="true" CodeBehind="UsersForm.aspx.cs" Inherits="OrderApplication.WebForm3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row-fluid">
            <div class="page-header">
                <h1>
                    <small>User Details</small></h1>
            </div>
        </div>
        <div class="row" style="width: 1080px">
            <div class="span5">
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Account Type:
                            </div>
                            <div class="span8">
                                <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="dropdown" Width="223px"
                                    Enabled="False">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlAccountType" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                User Name:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="input-large"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtUserName" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Password:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-large" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtPassword" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Last Name:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="input-large"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtLastName" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                First Name:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="input-large"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtFirstName" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="span6">
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Middle Name:
                            </div>
                            <div class="span8">
                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="input-large"></asp:TextBox>
                                <asp:TextBox ID="txtContactID" runat="server" CssClass="input-large" Visible="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtMiddleName" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Birthdate:
                            </div>
                            <div class="span8">
                                <asp:DropDownList ID="ddlBirthMonth" runat="server" CssClass="dropdown" Width="60px">
                                    <asp:ListItem Value="1">Jan</asp:ListItem>
                                    <asp:ListItem Value="2">Feb</asp:ListItem>
                                    <asp:ListItem Value="3">Mar</asp:ListItem>
                                    <asp:ListItem Value="4">Apr</asp:ListItem>
                                    <asp:ListItem Value="5">May</asp:ListItem>
                                    <asp:ListItem Value="6">June</asp:ListItem>
                                    <asp:ListItem Value="7">July</asp:ListItem>
                                    <asp:ListItem Value="8">Aug</asp:ListItem>
                                    <asp:ListItem Value="9">Sept</asp:ListItem>
                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtDay" runat="server" CssClass="input-large" placeholder="Day"
                                    Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtYear" runat="server" CssClass="input-large" placeholder="Year"
                                    Width="60px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlBirthMonth" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtDay" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtYear" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Gender:
                            </div>
                            <div class="span8">
                                <asp:RadioButton ID="rbMale" GroupName="genderType" runat="server" Text="Male" CssClass="radio inline"
                                    Checked="True" />
                                <asp:RadioButton ID="rbFemale" GroupName="genderType" runat="server" Text="Female"
                                    CssClass="radio inline" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Marital Status:
                            </div>
                            <div class="span8">
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server">
                                    <asp:ListItem>Single</asp:ListItem>
                                    <asp:ListItem>Married</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlMaritalStatus" ToolTip="Don't leave this blank" ValidationGroup="User"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Role ID:
                            </div>
                            <div class="span8">
                                <asp:RadioButton ID="rbAdmin" GroupName="roleType" runat="server" Text="Admin" CssClass="radio inline"
                                    value="3" Checked="True" />
                                <asp:RadioButton ID="rbUser" GroupName="roleType" runat="server" Text="User" CssClass="radio inline"
                                    value="4" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span6">
                        <div class="row-fluid">
                            <div class="span4">
                                Validity:
                            </div>
                            <div class="span8">
                                <div class="span5">
                                    <asp:TextBox ID="txtDateValidity" runat="server"></asp:TextBox><asp:CalendarExtender
                                        ID="CalendarExtender1" runat="server" TargetControlID="txtDateValidity">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span5">
                <div class="row-fluid">
                    <div class="span3">
                        State/Province:
                    </div>
                    <div class="span9">
                        <asp:DropDownList ID="ddlStateProvince" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"
                            ToolTip="Don't leave blank." ControlToValidate="ddlStateProvince" ForeColor="Red"
                            ValidationGroup="company"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                        City:
                    </div>
                    <div class="span9">
                        <asp:DropDownList ID="ddlCity" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*"
                            ToolTip="Don't leave blank." ControlToValidate="ddlCity" ForeColor="Red" ValidationGroup="company"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                        Barangay:
                    </div>
                    <div class="span9">
                        <asp:DropDownList ID="ddlBarangay" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*"
                            ToolTip="Don't leave blank." ControlToValidate="ddlBarangay" ForeColor="Red"
                            ValidationGroup="company"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                        Zip Code:
                    </div>
                    <div class="span9">
                        <asp:TextBox ID="txtZipPostal" runat="server" CssClass="input-large"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="*"
                            ToolTip="Don't leave blank." ControlToValidate="txtZipPostal" ForeColor="Red"
                            ValidationGroup="User"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtAddressID" runat="server" CssClass="input-large" Visible="false"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                        Street:
                    </div>
                    <div class="span9">
                        <asp:TextBox ID="txtStreet" runat="server" CssClass="input-large"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="*"
                            ToolTip="Don't leave blank." ControlToValidate="txtStreet" ForeColor="Red" ValidationGroup="company"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                        Contact:
                    </div>
                    <div class="span8">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="input-large" placeholder="Phone No."
                            Width="130px"></asp:TextBox>
                        <asp:TextBox ID="txtFAX" runat="server" CssClass="input-large" placeholder="Fax No."
                            Width="130px"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                    </div>
                    <div class="span8">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="input-large" placeholder="Mobile No."
                            Width="100px"></asp:TextBox>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-large" placeholder="Email"
                            Width="170px"></asp:TextBox>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span3">
                    </div>
                    <div class="span8 pull-right">
                        <div class="fileupload fileupload-new" data-provides="fileupload">
                            <div class="fileupload-new thumbnail" style="width: 200px; height: 150px;">
                                <asp:Image ID="userIMG" runat="server" />
                            </div>
                            <div class="fileupload-preview fileupload-exists thumbnail" style="max-width: 200px;
                                max-height: 150px; line-height: 20px;">
                            </div>
                            <div>
                                <span class="btn btn-file"><span class="fileupload-new">Select image</span> <span
                                    class="fileupload-exists">Change</span>
                                    <input type="file" id="filMyFile" runat="server" />
                                </span><a href="#" class="btn fileupload-exists" data-dismiss="fileupload">Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span1">
                <asp:Button ID="btnSave" runat="server" Text="Save" cssclass="btn btn-primary" Width="70px"/>
            </div>
            <div class="span1">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" cssclass="btn" Width="70px" />
            </div>
        </div>
    </div>
</asp:Content>
