<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginFrame.aspx.cs" Inherits="CloudDaemonWeb.LoginFrame"
	EnableViewState="false" ValidateRequest="false" %>

<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.RelyingParty"
	TagPrefix="rp" %>
<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.Extensions.SimpleRegistration" TagPrefix="sreg" %>
<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.InfoCard" TagPrefix="ic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- COPYRIGHT (C) 2009 Andrew Arnott.  All rights reserved. -->
<!-- LICENSE: Microsoft Public License available at http://opensource.org/licenses/ms-pl.html -->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link type="text/css" href="theme/ui.all.css" rel="Stylesheet" />
	<link href="styles/loginpopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
<% if (Request.Url.IsLoopback) { %>
	<script type="text/javascript" src="scripts/jquery-1.3.1.js"></script>
	<script type="text/javascript" src="scripts/jquery-ui-personalized-1.6rc6.js"></script>
<% } else { %>
	<script type="text/javascript" language="javascript" src="http://www.google.com/jsapi"></script>
	<script type="text/javascript" language="javascript">
		google.load("jquery", "1.3.2");
		google.load("jqueryui", "1.7.2");
	</script>
<% } %>

	<script type="text/javascript" src="scripts/jquery.cookie.js"></script>

				<!--<rp:SelectorInfoCardButton>
					<InfoCardSelector Issuer="">
						<ClaimsRequested>
							<ic:ClaimType Name="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" IsOptional="false" />
							<ic:ClaimType Name="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" IsOptional="true" />
							<ic:ClaimType Name="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname" IsOptional="true" />
						</ClaimsRequested>
					</InfoCardSelector>
				</rp:SelectorInfoCardButton>-->
				
	<form runat="server" id="form1" target="_top">
	<div class="wrapper" style="text-align: left">
		<rp:OpenIdSelector runat="server" ID="openIdSelector" OnLoggedIn="openIdSelector_LoggedIn"
			OnFailed="openIdSelector_Failed" OnCanceled="openIdSelector_Failed" OnReceivedToken="openIdSelector_ReceivedToken"
			OnTokenProcessingError="openIdSelector_TokenProcessingError">
			<Buttons>
				<rp:SelectorProviderButton OPIdentifier="https://me.yahoo.com/" Image="images/yahoo.gif" />
				<rp:SelectorProviderButton OPIdentifier="https://www.myopenid.com/" Image="images/myopenid.png" />
				<rp:SelectorProviderButton OPIdentifier="https://pip.verisignlabs.com/" Image="images/verisign.gif" />
				<rp:SelectorOpenIdButton Image="images/openid.gif" />
			</Buttons>
			<Extensions>
				<sreg:ClaimsRequest Email="Require" FullName="Request" />
			</Extensions>
		</rp:OpenIdSelector>
		<asp:HiddenField runat="server" ID="topWindowUrl" />
		<asp:Panel ID="errorPanel" runat="server" EnableViewState="false" Visible="false" ForeColor="Red">
			Oops. Something went wrong while logging you in. Trying again may work. <a href="#" onclick="$('#errorMessage').show()">
				What went wrong?</a>
			<span id="errorMessage" style="display: none">
				<asp:Label ID="errorMessageLabel" runat="server" Text="Login canceled." />
			</span>
		</asp:Panel>
	</div>
	</form>
</body>
</html>
