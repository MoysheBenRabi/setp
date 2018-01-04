<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyObjects.ascx.cs" Inherits="CloudDaemonWeb.Controls.Objects.MyObjects" %>
Object Type:
<asp:DropDownList ID="ObjectTypeDropDownList" runat="server" 
    DataSourceID="ObjectTypeSqlDs" DataTextField="Name" 
    DataValueField="ObjectTypeId">
</asp:DropDownList>
&nbsp;Bubble:
<asp:DropDownList ID="BubbleDropDownList" runat="server" 
    DataSourceID="BubbleSqlDs" DataTextField="Name" DataValueField="BubbleId">
</asp:DropDownList>
&nbsp;<asp:Button ID="AddObjectButton" runat="server" 
    onclick="AddObjectButton_Click" Text="Add Object" />
<br />
<br />
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="CloudObjectId" DataSourceID="ObjectSqlDs" 
    EmptyDataText="There are no data records to display." EnableViewState="False">
    <Columns>
        <asp:TemplateField HeaderText="Type">
            <ItemTemplate>
                <asp:DropDownList ID="DropDownList3" runat="server" 
                    DataSourceID="ObjectTypeSqlDs" DataTextField="Name" 
                    DataValueField="ObjectTypeId" Enabled="False" 
                    SelectedValue='<%# Bind("ObjectTypeId") %>'>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Bubble">
            <ItemTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="BubbleSqlDs" 
                    DataTextField="Name" DataValueField="BubbleId" Enabled="False" 
                    SelectedValue='<%# Bind("BubbleId") %>'>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ParentId" HeaderText="ParentId" 
            SortExpression="ParentId" />
        <asp:BoundField DataField="CloudObjectId" HeaderText="CloudObjectId" 
            ReadOnly="True" SortExpression="CloudObjectId" Visible="False" />
        <asp:BoundField DataField="BubbleId" HeaderText="BubbleId" 
            SortExpression="BubbleId" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
            SortExpression="OwnerId" Visible="False" />
        <asp:BoundField DataField="ObjectTypeId" HeaderText="ObjectTypeId" 
            SortExpression="ObjectTypeId" Visible="False" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="X" HeaderText="X" SortExpression="X" />
        <asp:BoundField DataField="Y" HeaderText="Y" SortExpression="Y" />
        <asp:BoundField DataField="Z" HeaderText="Z" SortExpression="Z" />
        <asp:BoundField DataField="OX" HeaderText="OX" SortExpression="OX" />
        <asp:BoundField DataField="OY" HeaderText="OY" SortExpression="OY" />
        <asp:BoundField DataField="OZ" HeaderText="OZ" SortExpression="OZ" />
        <asp:BoundField DataField="OW" HeaderText="OW" SortExpression="OW" />
        <asp:BoundField DataField="Radius" HeaderText="Radius" 
            SortExpression="Radius" />
        <asp:BoundField DataField="Mass" HeaderText="Mass" SortExpression="Mass" />
        <asp:BoundField DataField="ModelUrl" HeaderText="Model Url" 
            SortExpression="ModelUrl" />
        <asp:BoundField DataField="ModelScale" HeaderText="Model Scale" 
            SortExpression="ModelScale" />
        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" 
            SortExpression="Enabled" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="ObjectSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [CloudObject] WHERE [CloudObjectId] = @CloudObjectId" 
    InsertCommand="INSERT INTO [CloudObject] ([CloudObjectId], [ParentId], [BubbleId], [OwnerId], [ObjectTypeId], [Name], [X], [Y], [Z], [OX], [OY], [OZ], [OW], [Radius], [Mass], [ModelUrl], [ModelScale], [Enabled], [Created], [Modified]) VALUES (@CloudObjectId, @ParentId, @BubbleId, @OwnerId, @ObjectTypeId, @Name, @X, @Y, @Z, @OX, @OY, @OZ, @OW, @Radius, @Mass, @ModelUrl, @ModelScale, @Enabled, @Created, @Modified)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT * FROM [CloudObject] WHERE ([OwnerId] = @OwnerId)" 
    
    
    UpdateCommand="UPDATE [CloudObject] SET [ParentId] = @ParentId, [BubbleId] = @BubbleId, [ObjectTypeId] = @ObjectTypeId, [Name] = @Name, [X] = @X, [Y] = @Y, [Z] = @Z, [OX] = @OX, [OY] = @OY, [OZ] = @OZ, [OW] = @OW, [Radius] = @Radius, [Mass] = @Mass, [ModelUrl] = @ModelUrl, [ModelScale] = @ModelScale, [Enabled] = @Enabled, [Modified] = GETDATE() WHERE [CloudObjectId] = @CloudObjectId">
    <SelectParameters>
        <asp:SessionParameter 
            Name="OwnerId" SessionField="ParticipantId" Type="Object" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="CloudObjectId" Type="Object" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="CloudObjectId" Type="Object" />
        <asp:Parameter Name="ParentId" Type="Object" />
        <asp:Parameter Name="BubbleId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="ObjectTypeId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="X" Type="Double" />
        <asp:Parameter Name="Y" Type="Double" />
        <asp:Parameter Name="Z" Type="Double" />
        <asp:Parameter Name="OX" Type="Double" />
        <asp:Parameter Name="OY" Type="Double" />
        <asp:Parameter Name="OZ" Type="Double" />
        <asp:Parameter Name="OW" Type="Double" />
        <asp:Parameter Name="Radius" Type="Double" />
        <asp:Parameter Name="Mass" Type="Double" />
        <asp:Parameter Name="ModelUrl" Type="String" />
        <asp:Parameter Name="ModelScale" Type="Double" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="Created" Type="DateTime" />
        <asp:Parameter Name="Modified" Type="DateTime" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="ParentId" />
        <asp:Parameter Name="BubbleId" />
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="ObjectTypeId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="X" Type="Double" />
        <asp:Parameter Name="Y" Type="Double" />
        <asp:Parameter Name="Z" Type="Double" />
        <asp:Parameter Name="OX" Type="Double" />
        <asp:Parameter Name="OY" Type="Double" />
        <asp:Parameter Name="OZ" Type="Double" />
        <asp:Parameter Name="OW" Type="Double" />
        <asp:Parameter Name="Radius" Type="Double" />
        <asp:Parameter Name="Mass" Type="Double" />
        <asp:Parameter Name="ModelUrl" Type="String" />
        <asp:Parameter Name="ModelScale" Type="Double" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="Created" Type="DateTime" />
        <asp:Parameter Name="Modified" Type="DateTime" />
        <asp:Parameter Name="CloudObjectId" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="ObjectTypeSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    SelectCommand="SELECT * FROM [ObjectType] WHERE ([OwnerId] = @OwnerId)">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="0f6fc313-30f4-466d-bdad-9e69d423f803" 
            Name="OwnerId" SessionField="ParticipantId" Type="Object" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="BubbleSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    SelectCommand="SELECT * FROM [Bubble] WHERE ([OwnerId] = @OwnerId)">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="" Name="OwnerId" 
            SessionField="ParticipantId" Type="Object" />
    </SelectParameters>
</asp:SqlDataSource>
