<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyRemoteProcesses.aspx.cs" Inherits="CloudDaemonWeb.Members.MyRemoteProcesses" %>
<%@ Register src="../Controls/Processes/MyRemoteProcesses.ascx" tagname="MyRemoteProcesses" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>Attached Hosts</h1>
    <p>These remote hosts can link to my bubbles.</p>
    <uc1:MyRemoteProcesses ID="MyRemoteProcesses1" runat="server" />
</asp:Content>
