<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageSerializationVerification.ascx.cs" Inherits="IOT.Controls.MessageSerializationVerification" %>
<h1>Message Serialization Verification</h1>
<p>
To verify message serialization please upload zip file containing messages serialized according to  reference message list.
</p>
<table>
<tr>
<td>Choose zip file to upload: </td>
<td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
</tr>
</table>
<asp:Button ID="VerifyButton" runat="server" onclick="VerifyButton_Click" 
    Text="Verify Message Serialization" />

