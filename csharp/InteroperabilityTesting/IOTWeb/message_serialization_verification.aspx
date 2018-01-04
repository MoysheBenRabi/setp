<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="message_serialization_verification.aspx.cs" Inherits="IOT.message_serialization_verification" %>
<%@ Register src="Controls/MessageSerializationVerification.ascx" tagname="MessageSerializationVerification" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <uc1:MessageSerializationVerification ID="MessageSerializationVerification1" 
    runat="server" />
</asp:Content>
