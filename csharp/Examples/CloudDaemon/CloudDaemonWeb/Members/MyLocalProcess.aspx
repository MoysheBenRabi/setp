<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyLocalProcess.aspx.cs" Inherits="CloudDaemonWeb.Members.MyLocalProcess" %>
<%@ Register src="../Controls/Processes/MyLocalProcess.ascx" tagname="MyLocalProcess" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<h1>Process Management</h1>
<h2>Process Details</h2>
<uc1:MyLocalProcess ID="MyLocalProcess1" runat="server" />
</asp:Content>
