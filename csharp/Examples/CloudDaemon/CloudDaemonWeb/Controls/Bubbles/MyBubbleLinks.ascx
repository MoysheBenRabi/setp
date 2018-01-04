<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyBubbleLinks.ascx.cs" Inherits="CloudDaemonWeb.Controls.Bubbles.MyBubbleLinks" %>
<asp:Button ID="AddBubbleLink" runat="server" onclick="AddBubbleLink_Click" 
    Text="Add Bubble Link" />
<asp:GridView ID="BubbleLinkView" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="BubbleLinkId" 
    DataSourceID="BubbleLinkSqlDs" 
    EmptyDataText="There are no data records to display.">
    <Columns>
        <asp:BoundField DataField="BubbleLinkId" HeaderText="BubbleLinkId" 
            ReadOnly="True" SortExpression="BubbleLinkId" Visible="False" />
        <asp:BoundField DataField="BubbleId" HeaderText="BubbleId" 
            SortExpression="BubbleId" Visible="False" />
        <asp:BoundField DataField="RemoteBubbleId" HeaderText="Remote Bubble Id" 
            SortExpression="RemoteBubbleId" />
        <asp:BoundField DataField="Address" HeaderText="Address" 
            SortExpression="Address" />
        <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
        <asp:BoundField DataField="X" HeaderText="X" SortExpression="X" />
        <asp:BoundField DataField="Y" HeaderText="Y" SortExpression="Y" />
        <asp:BoundField DataField="Z" HeaderText="Z" SortExpression="Z" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="BubbleLinkSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [BubbleLink] WHERE [BubbleLinkId] = @BubbleLinkId" 
    InsertCommand="INSERT INTO [BubbleLink] ([BubbleLinkId], [BubbleId], [Address], [Port], [X], [Y], [Z]) VALUES (@BubbleLinkId, @BubbleId, @Address, @Port, @X, @Y, @Z)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT * FROM [BubbleLink] WHERE BubbleId=@bubbleId" 
    
    UpdateCommand="UPDATE [BubbleLink] SET [Address] = @Address, [Port] = @Port, [X] = @X, [Y] = @Y, [Z] = @Z,[RemoteBubbleId] = @RemoteBubbleId WHERE [BubbleLinkId] = @BubbleLinkId">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="1c5b290c-88c7-4bcb-a22c-cfc0f442268a" 
            Name="bubbleId" QueryStringField="BubbleId" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="BubbleLinkId" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="BubbleLinkId" />
        <asp:Parameter Name="BubbleId" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="Port" Type="Int32" />
        <asp:Parameter Name="X" Type="Double" />
        <asp:Parameter Name="Y" Type="Double" />
        <asp:Parameter Name="Z" Type="Double" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="BubbleId" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="Port" Type="Int32" />
        <asp:Parameter Name="X" Type="Double" />
        <asp:Parameter Name="Y" Type="Double" />
        <asp:Parameter Name="Z" Type="Double" />
        <asp:Parameter Name="BubbleLinkId" />
        <asp:Parameter Name="RemoteBubbleId" />
    </UpdateParameters>
</asp:SqlDataSource>
