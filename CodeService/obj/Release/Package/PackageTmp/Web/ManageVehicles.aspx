<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageVehicles.aspx.cs" Inherits="CodeService.Web.ManageVehicles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create & Manage Vehicles</title>
</head>
<body>
    <form id="form1" runat="server">
        <div><asp:LinkButton ID="lnk" runat="server" Text="Home" PostBackUrl="~/Web/Home.aspx"></asp:LinkButton></div>
        <div>
            <h2>Create & Manage Vehicles</h2>
            <p>Select Vehicle<asp:DropDownList ID="ddlVehicles" runat="server" Width="320px">
                </asp:DropDownList>
                <asp:Button ID="btnSelect" Text="Select" runat="server" OnClick="btnSelect_Click" />
            </p>
            <p>Vehicle Name <asp:TextBox ID="txtVehicleName" runat="server" Width="320px"></asp:TextBox></p>
            <p>MAC Address <asp:TextBox ID="txtMACAddress" runat="server" Width="320px"></asp:TextBox></p>
            <p>Select Class <asp:DropDownList ID="ddlVehicleClasses" runat="server" Width="320px"></asp:DropDownList></p>
            <p>Select Service <asp:DropDownList ID="ddlServices" runat="server" Width="320px"></asp:DropDownList></p>
            <p><asp:Button ID="btnUpdate" Text="Update" runat="server" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
            </p>
        </div>
    </form>
</body>
</html>
