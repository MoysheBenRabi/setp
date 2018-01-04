<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyLocalProcess.ascx.cs" Inherits="CloudDaemonWeb.Controls.Processes.MyLocalProcess" %>
<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
    DataKeyNames="LocalProcessId" DataSourceID="LocalProcessSqlDs" Height="64px" 
    Width="309px">
    <Fields>
        <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
            ReadOnly="True" SortExpression="LocalProcessId" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
            SortExpression="OwnerId" Visible="False" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="Address" HeaderText="Address" 
            SortExpression="Address" />
        <asp:BoundField DataField="ServerPort" HeaderText="ServerPort" 
            SortExpression="ServerPort" />
        <asp:BoundField DataField="HubPort" HeaderText="HubPort" 
            SortExpression="HubPort" />
        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" 
            SortExpression="Enabled" />
    </Fields>
</asp:DetailsView>
<asp:SqlDataSource ID="LocalProcessSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [LocalProcess] WHERE [LocalProcessId] = @LocalProcessId" 
    InsertCommand="INSERT INTO [LocalProcess] ([LocalProcessId], [OwnerId], [Name], [Address], [ServerPort], [HubPort], [Enabled]) VALUES (@LocalProcessId, @OwnerId, @Name, @Address, @ServerPort, @HubPort, @Enabled)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT * FROM [LocalProcess] WHERE ([LocalProcessId] = @LocalProcessId)" 
    UpdateCommand="UPDATE [LocalProcess] SET [OwnerId] = @OwnerId, [Name] = @Name, [Address] = @Address, [ServerPort] = @ServerPort, [HubPort] = @HubPort, [Enabled] = @Enabled WHERE [LocalProcessId] = @LocalProcessId">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="2c8b3b83-75b2-4485-97ea-347744a711af" 
            Name="LocalProcessId" QueryStringField="LocalProcessId" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="LocalProcessId" Type="Object" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="LocalProcessId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="ServerPort" Type="Int32" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="ServerPort" Type="Int32" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="LocalProcessId" Type="Object" />
    </UpdateParameters>
</asp:SqlDataSource>
