<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyObjectTypes.aspx.cs" Inherits="CloudDaemonWeb.Members.MyObjectTypes" %>
<%@ Register src="../Controls/Objects/MyObjectTypes.ascx" tagname="MyObjectTypes" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>My Object Types</h1>
    <uc1:MyObjectTypes ID="MyObjectTypes1" runat="server" />
</asp:Content>
