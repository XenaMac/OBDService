<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehiclePID.aspx.cs" Inherits="CodeService.Web.VehiclePID" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <div><asp:LinkButton ID="lnk" runat="server" Text="Home" PostBackUrl="~/Web/Home.aspx"></asp:LinkButton></div>
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Select Vehicle"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlMACs" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Select" />
        <br />
        Selected Vehicle:
        <asp:Label ID="lblSelectedVehicle" runat="server" Text="NONE"></asp:Label>
    
    </div>
        <asp:CheckBoxList ID="chkPIDList" runat="server">
        </asp:CheckBoxList>
        Set Group Name
        <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update Selected Data" />
    </form>
</body>
</html>
