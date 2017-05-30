<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataViewer.aspx.cs" Inherits="CodeService.Web.DataViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                        <div><asp:LinkButton ID="lnk" runat="server" Text="Home" PostBackUrl="~/Web/Home.aspx"></asp:LinkButton></div>
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Select DataSet"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlDataSets" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Button ID="btnViewData" runat="server" OnClick="btnViewData_Click" Text="View Data" />
    
    </div>
        <hr />
        <asp:GridView ID="gvData" runat="server">
        </asp:GridView>
    </form>
</body>
</html>
