<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyLocalProcesses.ascx.cs" Inherits="CloudDaemonWeb.Controls.Processes.MyLocalProcesses" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<asp:Button ID="ReserveProcessButton" runat="server" CausesValidation="False" 
    onclick="ReserveProcessButton_Click" Text="Reserve Process" />
<br />
<br />
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="LocalProcessId" 
    DataSourceID="SqlDataSource1" 
    EmptyDataText="There are no data records to display.">
    <Columns>
        <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
            ReadOnly="True" SortExpression="LocalProcessId" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
            SortExpression="OwnerId" Visible="False" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="Address" HeaderText="Address" ReadOnly="True" 
            SortExpression="Address" />
        <asp:BoundField DataField="ServerPort" HeaderText="Server Port" ReadOnly="True" 
            SortExpression="ServerPort" />
        <asp:BoundField DataField="HubPort" HeaderText="Hub Port" ReadOnly="True" 
            SortExpression="HubPort" />
        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" 
            SortExpression="Enabled" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
        <asp:HyperLinkField DataNavigateUrlFields="LocalProcessId" 
            DataNavigateUrlFormatString="~/Members/MyLocalProcess.aspx?LocalProcessId={0}" 
            Text="Manage &amp;raquo;" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [LocalProcess] WHERE [LocalProcessId] = @LocalProcessId" 
    InsertCommand="INSERT INTO [LocalProcess] ([LocalProcessId], [OwnerId], [Name], [Address], [ServerPort], [HubPort], [Enabled]) VALUES (@LocalProcessId, @OwnerId, @Name, @Address, @ServerPort, @HubPort, @Enabled)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT * FROM [LocalProcess] WHERE ([OwnerId] = @OwnerId)" 
    UpdateCommand="UPDATE [LocalProcess] SET [Name] = @Name, [Enabled] = @Enabled WHERE [LocalProcessId] = @LocalProcessId">
    <SelectParameters>
        <asp:SessionParameter Name="OwnerId" SessionField="ParticipantId" 
            Type="Object" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="LocalProcessId" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="LocalProcessId"  />
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="ServerPort" Type="Int32" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="ServerPort" Type="Int32" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="LocalProcessId" />
    </UpdateParameters>
</asp:SqlDataSource>

