<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicBubbles.ascx.cs" Inherits="CloudDaemonWeb.Controls.Bubbles.PublicBubbles" %>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="BubbleId" DataSourceID="SqlDataSource1" 
    EmptyDataText="There are no data records to display.">
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="Address,ServerPort,BubbleId" 
            DataNavigateUrlFormatString="../../Members/GetLoginSecret.aspx?goto=http%3A%2F%2F{0}%3A{1}%2F{2}" 
            DataTextField="BubbleName" DataTextFormatString="Deck Login &amp;raquo;" />
        <asp:BoundField DataField="LocalProcessName" HeaderText="Process" 
            SortExpression="LocalProcessName" />
        <asp:BoundField DataField="BubbleName" HeaderText="Bubble" 
            SortExpression="BubbleName" />
        <asp:BoundField DataField="Range" HeaderText="Range" SortExpression="Range" />
        <asp:BoundField DataField="OwnerOpenId" HeaderText="Owner OpenId" 
            SortExpression="OwnerOpenId" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [Bubble] WHERE [BubbleId] = @BubbleId" 
    InsertCommand="INSERT INTO [Bubble] ([BubbleId], [OwnerId], [LocalProcessId], [Name], [Range], [PerceptionRange], [Published], [Enabled]) VALUES (@BubbleId, @OwnerId, @LocalProcessId, @Name, @Range, @PerceptionRange, @Published, @Enabled)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" SelectCommand="SELECT LocalProcess.Name AS LocalProcessName, Bubble.Name AS BubbleName, Participant.OpenIdUrl AS OwnerOpenId, Bubble.Range, Bubble.PerceptionRange, LocalProcess.Address, LocalProcess.ServerPort, Bubble.BubbleId FROM Bubble INNER JOIN LocalProcess ON Bubble.LocalProcessId = LocalProcess.LocalProcessId INNER JOIN LocalProcessState ON LocalProcess.LocalProcessId = LocalProcessState.LocalProcessId INNER JOIN Participant ON Bubble.OwnerId = Participant.ParticipantId WHERE (Bubble.Published = 1) AND (Bubble.Enabled = 1) ORDER BY LocalProcessName, BubbleName" 
    
    UpdateCommand="UPDATE [Bubble] SET [OwnerId] = @OwnerId, [LocalProcessId] = @LocalProcessId, [Name] = @Name, [Range] = @Range, [PerceptionRange] = @PerceptionRange, [Published] = @Published, [Enabled] = @Enabled WHERE [BubbleId] = @BubbleId">
    <DeleteParameters>
        <asp:Parameter Name="BubbleId" Type="Object" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="BubbleId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="LocalProcessId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Range" Type="Double" />
        <asp:Parameter Name="PerceptionRange" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="LocalProcessId" Type="Object" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Range" Type="Double" />
        <asp:Parameter Name="PerceptionRange" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
        <asp:Parameter Name="BubbleId" Type="Object" />
    </UpdateParameters>
</asp:SqlDataSource>
