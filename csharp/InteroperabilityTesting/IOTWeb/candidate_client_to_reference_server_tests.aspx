<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="candidate_client_to_reference_server_tests.aspx.cs" Inherits="IOT.candidate_client_to_reference_server_tests" %>
<%@ Register src="Controls/CandidateClientToReferenceServerTests.ascx" tagname="CandidateClientToReferenceServerTests" tagprefix="uc1" %>
<%@ Register src="Controls/ServiceController.ascx" tagname="ServiceController" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <h1>Candidate Client to Reference Server Tests</h1>
    <uc1:CandidateClientToReferenceServerTests ID="CandidateClientToReferenceServerTests2" 
        runat="server" />
    <uc2:ServiceController ID="ServiceController1" runat="server" />
</asp:Content>
