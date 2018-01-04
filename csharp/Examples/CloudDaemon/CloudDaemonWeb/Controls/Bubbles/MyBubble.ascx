<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyBubble.ascx.cs" Inherits="CloudDaemonWeb.Controls.Bubbles.MyBubble" %>
<p>
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateEditButton="True" 
        AutoGenerateRows="False" DataKeyNames="BubbleId" DataSourceID="BubbleSqlDs" 
        Height="74px" Width="369px">
        <Fields>
            <asp:BoundField DataField="BubbleId" HeaderText="BubbleId" ReadOnly="True" 
                SortExpression="BubbleId" />
            <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
                SortExpression="OwnerId" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
                SortExpression="LocalProcessId" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="Range" HeaderText="Range" SortExpression="Range" />
            <asp:BoundField DataField="PerceptionRange" HeaderText="PerceptionRange" 
                SortExpression="PerceptionRange" />
            <asp:CheckBoxField DataField="Published" HeaderText="Published" 
                SortExpression="Published" />
            <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" 
                SortExpression="Enabled" />
        </Fields>
    </asp:DetailsView>
    <br />
</p>
<asp:SqlDataSource ID="BubbleSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [Bubble] WHERE [BubbleId] = @BubbleId" 
    InsertCommand="INSERT INTO [Bubble] ([BubbleId], [OwnerId], [LocalProcessId], [Name], [Range], [PerceptionRange], [Published]) VALUES (@BubbleId, @OwnerId, @LocalProcessId, @Name, @Range, @PerceptionRange, @Published)" 
    SelectCommand="SELECT * FROM [Bubble] WHERE ([BubbleId] = @bubbleId)" 
    UpdateCommand="UPDATE [Bubble] SET [Name] = @Name, [Range] = @Range, [PerceptionRange] = @PerceptionRange, [Published] = @Published, [Enabled] = @Enabled WHERE [BubbleId] = @BubbleId">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="bde2a7df-af3f-43c0-8f79-ee1d3504e3db" 
            Name="BubbleId" QueryStringField="BubbleId" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="BubbleId" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="LocalProcessId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Range" Type="Double" />
        <asp:Parameter Name="PerceptionRange" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="BubbleId" Type="Object" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="BubbleId"/>
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="LocalProcessId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Range" Type="Double" />
        <asp:Parameter Name="PerceptionRange" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
