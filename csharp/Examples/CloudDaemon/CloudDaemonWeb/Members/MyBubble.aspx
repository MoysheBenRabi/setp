<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyBubble.aspx.cs" Inherits="CloudDaemonWeb.Members.MyBubble" %>
<%@ Register src="../Controls/Bubbles/MyBubble.ascx" tagname="MyBubble" tagprefix="uc1" %>
<%@ Register src="../Controls/Bubbles/MyBubbleLinks.ascx" tagname="MyBubbleLinks" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>Bubble Management</h1>
    <h2>Bubble Details</h2>
    <uc1:MyBubble ID="MyBubble1" runat="server" />
    <h2>Bubble Links</h2>
    <uc2:MyBubbleLinks ID="MyBubbleLinks1" runat="server" />
</asp:Content>
