<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CodeService.Web.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Web/ManageClasses.aspx">Manage Classes</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/Web/ManageVehicles.aspx">Manage Vehicles</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/Web/ManageServices.aspx">Manage Services</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="~/Web/VehiclePID.aspx">Manage PIDs</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/Web/DataViewer.aspx">Data Viewer</asp:LinkButton>
    </form>
</body>
</html>
