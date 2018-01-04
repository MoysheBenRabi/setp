<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceController.ascx.cs"
    Inherits="IOT.Controls.ServiceController" %>
<h2>Test Service Control Panel</h2>
<table>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="My test is started"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ServiceReservedSelfCheckBox" runat="server" Enabled="False" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Someone else is executing test"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ServiceReservedOtherCheckBox" runat="server" Enabled="False" />
        </td>
    </tr>    
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Time left for current test"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ReservedTimeLeftTextBox" runat="server" Enabled="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="ReserveServiceButton" runat="server" Text="Start my test (Launches Reference Server)" 
                onclick="ReserveServiceButton_Click" />
        </td>
    </tr>
</table>
<h2>Test Service Log</h2>