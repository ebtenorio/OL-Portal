﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageEvent.aspx.cs" Inherits="OrderApplication.PageEvent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script language=C# runat="server">
void Page_Load(object sender, System.EventArgs e)
{
	throw(new ArgumentNullException());
}

public void Page_Error(object sender,EventArgs e)
{
	Exception objErr = Server.GetLastError().GetBaseException();
	string err =	"<b>Error Caught in Page_Error event</b><hr><br>" + 
			"<br><b>Error in: </b>" + Request.Url.ToString() +
			"<br><b>Error Message: </b>" + objErr.Message.ToString()+
			"<br><b>Stack Trace:</b><br>" + 
	                  objErr.StackTrace.ToString();
	Response.Write(err.ToString());
	Server.ClearError();
}
</script> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
