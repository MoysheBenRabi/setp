<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CloudDaemonWeb._Default"
	MasterPageFile="~/Site.Master" Title="OpenID + InfoCard Relying Party template" %>

<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth" TagPrefix="dnoa" %>
<%@ Register src="Controls/Processes/AllProcessSates.ascx" tagname="AllProcessSates" tagprefix="uc1" %>
<%@ Register src="Controls/Bubbles/PublicBubbles.ascx" tagname="PublicBubbles" tagprefix="uc2" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<dnoa:XrdsPublisher runat="server" XrdsUrl="~/xrds.aspx" XrdsAdvertisement="Both" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h1>Welcome to Bubble Cloud Demonstration</h1>
    <p>
    Welcome <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label> to the cloud daemon web user interface. This web interface can be used for creating private bubble clouds with your OpenId identity and connecting it with clouds of others.
    </p>
		
    <h2>Online Public Bubbles</h2>
	<uc2:PublicBubbles ID="PublicBubbles1" runat="server" />
		
	<h2>All Processes Running</h2>
	<uc1:AllProcessSates ID="AllProcessSates1" runat="server" />

</asp:Content>
