<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageClasses.aspx.cs" Inherits="CodeService.Web.ManageClasses" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create & Manage Classes</title>
</head>
<body>
    <form id="form1" runat="server">
        <div><asp:LinkButton ID="lnk" runat="server" Text="Home" PostBackUrl="~/Web/Home.aspx"></asp:LinkButton></div>
    <div>
        <h2>Create & Manage Classes</h2>
        <p>Select Vehicle Class <asp:DropDownList ID="ddlClasses" runat="server"></asp:DropDownList>
            <asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Select" />
        </p>
        <p>Enter Vehicle Class Name <asp:TextBox ID="txtClassName" runat="server"></asp:TextBox>
        </p>
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" /><asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
    </div>
    </form>
</body>
</html>
