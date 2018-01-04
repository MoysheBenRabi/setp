<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyBubbles.ascx.cs" Inherits="CloudDaemonWeb.Controls.Bubbles.MyBubbles" %>
Process which will host the bubble: 
<asp:DropDownList ID="LocalProcessList" runat="server" 
    DataSourceID="LocalProcessSqlDs" DataTextField="Name" 
    DataValueField="LocalProcessId">
</asp:DropDownList>
&nbsp;<asp:Button ID="AddBubbleButton" runat="server" 
    onclick="AddBubbleButton_Click" Text="Add Bubble" />
<br />
<br />
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="BubbleId" 
    DataSourceID="BubbleSqlDs" 
    EmptyDataText="There are no data records to display.">
    <Columns>
        <asp:BoundField DataField="BubbleId" HeaderText="BubbleId" ReadOnly="True" 
            SortExpression="BubbleId" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
            SortExpression="OwnerId" Visible="False" />
        <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
            SortExpression="LocalProcessId" Visible="False" />
        <asp:TemplateField HeaderText="Host Process">
            <EditItemTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataSourceID="LocalProcessSqlDs" DataTextField="Name" 
                    DataValueField="LocalProcessId" SelectedValue='<%# Bind("LocalProcessId") %>'>
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataSourceID="LocalProcessSqlDs" DataTextField="Name" 
                    DataValueField="LocalProcessId" Enabled="False" 
                    SelectedValue='<%# Bind("LocalProcessId") %>'>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="Range" HeaderText="Range" SortExpression="Range" />
        <asp:BoundField DataField="PerceptionRange" HeaderText="Perception Range" 
            SortExpression="PerceptionRange" />
        <asp:CheckBoxField DataField="Published" HeaderText="Published" 
            SortExpression="Published" />
        <asp:CheckBoxField DataField="Enabled" SortExpression="Enabled" 
            HeaderText="Enabled" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
        <asp:HyperLinkField DataNavigateUrlFields="BubbleId" 
            DataNavigateUrlFormatString="~/Members/MyBubble.aspx?BubbleId={0}" 
            DataTextField="Name" DataTextFormatString="Manage {0} &amp;raquo;" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="BubbleSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [Bubble] WHERE [BubbleId] = @BubbleId" 
    InsertCommand="INSERT INTO [Bubble] ([BubbleId], [OwnerId], [LocalProcessId], [Name], [Range], [PerceptionRange], [Published], [Enabled]) VALUES (@BubbleId, @OwnerId, @LocalProcessId, @Name, @Range, @PerceptionRange, @Published, @Enabled)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT * FROM [Bubble] WHERE ([OwnerId] = @OwnerId)" 
    
    
    UpdateCommand="UPDATE [Bubble] SET [LocalProcessId] = @LocalProcessId, [Name] = @Name, [Range] = @Range, [PerceptionRange] = @PerceptionRange, [Published] = @Published, [Enabled] = @Enabled WHERE [BubbleId] = @BubbleId">
    <SelectParameters>
        <asp:SessionParameter 
            Name="OwnerId" SessionField="ParticipantId" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="BubbleId" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="BubbleId" />
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="LocalProcessId" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="Range" Type="Double" />
        <asp:Parameter Name="PerceptionRange" Type="Double" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="Enabled" Type="Boolean" />
    </InsertParameters>
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
</asp:SqlDataSource>
<asp:SqlDataSource ID="LocalProcessSqlDs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    SelectCommand="SELECT [LocalProcessId], [Name] FROM [LocalProcess] WHERE ([OwnerId] = @OwnerId)">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="c32544de-7b78-4006-a885-8d937bcd84bd" 
            Name="OwnerId" SessionField="ParticipantId" Type="Object" />
    </SelectParameters>
</asp:SqlDataSource>

