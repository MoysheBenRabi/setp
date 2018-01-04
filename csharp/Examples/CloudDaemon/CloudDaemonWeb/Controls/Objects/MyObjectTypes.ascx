<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyObjectTypes.ascx.cs" Inherits="CloudDaemonWeb.Controls.Objects.MyObjectTypes" %>
<asp:Button ID="AddObjectTypeButton" runat="server" 
    onclick="AddObjectTypeButton_Click" Text="Add Object Type" />
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ObjectTypeId" 
    DataSourceID="SqlDataSource1" 
    EmptyDataText="There are no data records to display." 
    EnableViewState="False">
    <Columns>
        <asp:BoundField DataField="ObjectTypeId" HeaderText="ObjectTypeId" 
            ReadOnly="True" SortExpression="ObjectTypeId" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
            SortExpression="OwnerId" Visible="False" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="Radius" HeaderText="Radius" 
            SortExpression="Radius" />
        <asp:BoundField DataField="Mass" HeaderText="Mass" SortExpression="Mass" />
        <asp:BoundField DataField="ModelUrl" HeaderText="ModelUrl" 
            SortExpression="ModelUrl" />
        <asp:BoundField DataField="ModelScale" HeaderText="ModelScale" 
            SortExpression="ModelScale" />
        <asp:CheckBoxField DataField="Published" HeaderText="Published" 
            SortExpression="Published" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [ObjectType] WHERE [ObjectTypeId] = @ObjectTypeId" 
    InsertCommand="INSERT INTO [ObjectType] ([ObjectTypeId], [OwnerId], [Name], [Radius], [Mass], [ModelUrl], [ModelScale], [Published]) VALUES (@ObjectTypeId, @OwnerId, @Name, @Radius, @Mass, @ModelUrl, @ModelScale, @Published)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT [ObjectTypeId], [OwnerId], [Name], [Radius], [Mass], [ModelUrl], [ModelScale], [Published] FROM [ObjectType] WHERE ([OwnerId] = @OwnerId)" 
    UpdateCommand="UPDATE [ObjectType] SET [Name] = @Name, [Radius] = @Radius, [Mass] = @Mass, [ModelUrl] = @ModelUrl, [ModelScale] = @ModelScale, [Published] = @Published WHERE [ObjectTypeId] = @ObjectTypeId">
    <SelectParameters>
        <asp:SessionParameter Name="OwnerId" SessionField="ParticipantId" 
            Type="Object" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="ObjectTypeId" Type="Object" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="ObjectTypeId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Radius" Type="Double" />
        <asp:Parameter Name="Mass" Type="Double" />
        <asp:Parameter Name="ModelUrl" Type="String" />
        <asp:Parameter Name="ModelScale" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Radius" Type="Double" />
        <asp:Parameter Name="Mass" Type="Double" />
        <asp:Parameter Name="ModelUrl" Type="String" />
        <asp:Parameter Name="ModelScale" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="ObjectTypeId" />
    </UpdateParameters>
</asp:SqlDataSource>
