<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CloudDaemonWeb.Login"
	MasterPageFile="~/Site.Master" ValidateRequest="false" %>

<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.RelyingParty"
	TagPrefix="rp" %>
<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.InfoCard" TagPrefix="ic" %>
<asp:Content runat="server" ContentPlaceHolderID="Body">
	<iframe src="LoginFrame.aspx" frameborder="0" width="800" height="300"></iframe>
</asp:Content>
