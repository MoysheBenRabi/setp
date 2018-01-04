<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyLocalProcessStates.aspx.cs" Inherits="CloudDaemonWeb.Members.MyLocalProcessStates" %>
<%@ Register src="../Controls/Processes/MyLocalProcessStates.ascx" tagname="MyLocalProcessStates" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>My Running Processes</h1>
    <uc1:MyLocalProcessStates ID="MyLocalProcessStates1" runat="server" />
</asp:Content>
