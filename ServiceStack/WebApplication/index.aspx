<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebApplication.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>主页</h1>
        ID：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        Name：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="提交" OnClick="Button1_Click" /><br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
        <hr />
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><br />
        <asp:Button ID="Button2" runat="server" Text="获取" OnClick="Button2_Click" /><br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
