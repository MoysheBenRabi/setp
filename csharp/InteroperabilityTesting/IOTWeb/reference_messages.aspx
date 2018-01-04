<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="reference_messages.aspx.cs" Inherits="IOT.reference_messages" %>
<%@ Register src="Controls/ReferenceMessageList.ascx" tagname="ReferenceMessageList" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <uc1:ReferenceMessageList ID="ReferenceMessageList1" runat="server" />
</asp:Content>
