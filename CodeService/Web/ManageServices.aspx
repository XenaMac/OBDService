<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageServices.aspx.cs" Inherits="CodeService.Web.ManageServices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <div><asp:LinkButton ID="lnk" runat="server" Text="Home" PostBackUrl="~/Web/Home.aspx"></asp:LinkButton></div>
        <div>
            Edit
            <br />
            Select Service:
            <asp:DropDownList ID="ddlServices" runat="server" Width="324px">
            </asp:DropDownList>
&nbsp;<asp:Button ID="btnSelectService" runat="server" Text="Go" OnClick="btnSelectService_Click" />
            <br />
            Selected Service
            <asp:Label ID="lblSelectedService" runat="server" Text="NONE"></asp:Label>
            <br />
            Company Name
            <asp:TextBox ID="txtCompanyName" runat="server" Width="324px"></asp:TextBox>
            <br />
            ConnString
            <asp:TextBox ID="txtConnString" runat="server" Width="324px"></asp:TextBox>
            <br />
            Service URL
            <asp:TextBox ID="txtServiceURL" runat="server" Width="324px"></asp:TextBox>
            <br />
            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />
&nbsp;<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
            <br />
        </div>
        <hr />
        <div>
            Create
        <br />
            Company Name
            <asp:TextBox ID="txtAddCompanyName" runat="server" Width="324px"></asp:TextBox>
            <br />
            Conn String
            <asp:TextBox ID="txtAddConnString" runat="server" Width="324px"></asp:TextBox>
            <br />
            Service URL
            <asp:TextBox ID="txtAddServiceURL" runat="server" Width="324px"></asp:TextBox>
            <br />
            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" />
        </div>
    </form>
</body>
</html>
