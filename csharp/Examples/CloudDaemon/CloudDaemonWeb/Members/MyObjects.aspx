<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyObjects.aspx.cs" Inherits="CloudDaemonWeb.Members.MyObjects" %>
<%@ Register src="../Controls/Objects/MyObjects.ascx" tagname="MyObjects" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<h1>My Objects</h1>
    <uc1:MyObjects ID="MyObjects1" runat="server" />
</asp:Content>
