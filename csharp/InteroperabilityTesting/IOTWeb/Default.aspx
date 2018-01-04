<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="IOT._default" %>
<%@ Register src="Controls/MessageSerializationVerification.ascx" tagname="MessageSerializationVerification" tagprefix="uc1" %>
<%@ Register src="Controls/TestState.ascx" tagname="TestState" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <uc2:TestState ID="TestState1" runat="server" />

</asp:Content>
