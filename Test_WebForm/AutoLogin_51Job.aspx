<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoLogin_51Job.aspx.cs" Inherits="Test_WebForm.AutoLogin_51Job" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Content/Style/bootstrap.min.css" rel="stylesheet" />
    <script src="Content/Script/bootstrap.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnGetData" runat="server" Text="获取简历" Width="100" Height="30" OnClick="btnGetData_Click" /><br />
        <asp:TextBox ID="txtContext" runat="server" Columns="150" Rows="30" TextMode="MultiLine"></asp:TextBox>
    </form>
</body>
</html>
