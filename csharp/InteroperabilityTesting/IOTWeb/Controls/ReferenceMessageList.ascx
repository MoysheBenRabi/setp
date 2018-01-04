<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferenceMessageList.ascx.cs" Inherits="IOT.Controls.ReferenceMessageList" %>
<h1>Help for Message Serialization Verification</h1>
<p>
This list describes the content of reference messages in message serialization verification test. 
Packet headers should contain 1 as session id. Packet ids should start counting from 1 for each message.
Packet first send time should be always 2009-11-05 15:33:25 which corresponds to serialized format
88-E1-2A-5A-48. Message Id should always be 1. The messages content and serialized bytes to packet 
stream can be viewed from the following table:
</p>
<asp:Button ID="DownloadReferenceMessagesButton" runat="server" 
    Text="Download Reference Messages" 
    onclick="DownloadReferenceMessagesButton_Click" />