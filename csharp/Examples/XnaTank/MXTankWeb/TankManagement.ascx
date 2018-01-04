<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TankManagement.ascx.cs" Inherits="MXTankWeb.TankManagement" %>

<h1>Tank Management</h1>

<div>
<asp:Button ID="StartTankButton" runat="server" onclick="StartTankButton_Click" Text="Start Tank" />
<asp:Button ID="StopTankButton" runat="server" onclick="StopTankButton_Click" Text="Stop Tank" />
    <asp:Button ID="RefreshButton" runat="server" onclick="RefreshButton_Click" 
        Text="Refresh" />
    <br />
    <asp:Button ID="StartTestParticipantButton" runat="server" 
        onclick="StartTestParticipantButton_Click" Text="Start Test Participant" />
    <asp:Button ID="StopTestParticipantButton" runat="server" 
        onclick="StopTestParticipantButton_Click" Text="Stop Test Participant" />
</div>