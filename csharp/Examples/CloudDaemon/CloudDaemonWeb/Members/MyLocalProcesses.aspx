<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyLocalProcesses.aspx.cs" Inherits="CloudDaemonWeb.Members.MyLocalProcesses" %>
<%@ Register src="../Controls/Processes/MyLocalProcesses.ascx" tagname="MyLocalProcesses" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>My Hosts</h1>
    <p>These are my hosts which can run my bubbles.</p>
    <uc1:MyLocalProcesses ID="MyLocalProcesses1" runat="server" />
</asp:Content>
