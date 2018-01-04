<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyBubbles.aspx.cs" Inherits="CloudDaemonWeb.Members.MyBubbles" %>
<%@ Register src="../Controls/Bubbles/MyBubbles.ascx" tagname="MyBubbles" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>My Bubbles</h1>
    <uc1:MyBubbles ID="MyBubbles1" runat="server" />
</asp:Content>
