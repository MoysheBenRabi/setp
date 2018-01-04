<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyLocalProcessLog.aspx.cs" Inherits="CloudDaemonWeb.Members.MyLocalProcessLog" %>
<%@ Register src="../Controls/Processes/MyLocalProcessLog.ascx" tagname="MyLocalProcessLog" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<h1>My Process Log</h1>
    <uc1:MyLocalProcessLog ID="MyLocalProcessLog1" runat="server" />
</asp:Content>
