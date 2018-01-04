<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllProcessSates.ascx.cs" Inherits="CloudDaemonWeb.Controls.Processes.AllProcessSates" %>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="LocalProcessStateId" DataSourceID="SqlDataSource1" 
            EmptyDataText="There are no data records to display." 
            EnableViewState="False">
            <Columns>
                <asp:BoundField DataField="LocalProcessStateId" 
                    HeaderText="LocalProcessStateId" ReadOnly="True" 
                    SortExpression="LocalProcessStateId" Visible="False" />
                <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
                    SortExpression="LocalProcessId" Visible="False" />
                <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" 
                    SortExpression="OwnerId" Visible="False" />
                <asp:TemplateField HeaderText="Local Process">
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" 
                            DataSourceID="LocalProcessSqlDs" DataTextField="Name" 
                            DataValueField="LocalProcessId" Enabled="False" 
                            SelectedValue='<%# Bind("LocalProcessId") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="LocalProcessSqlDs" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
                            SelectCommand="SELECT [LocalProcessId], [Name] FROM [LocalProcess]">
                        </asp:SqlDataSource>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Created" HeaderText="Created" 
                    SortExpression="Created" />
                <asp:BoundField DataField="Modified" HeaderText="Modified" 
                    SortExpression="Modified" />
                <asp:BoundField DataField="Cpu" HeaderText="Cpu [%]" SortExpression="Cpu" />
                <asp:BoundField DataField="Mem" HeaderText="Mem [kbytes]" 
                    SortExpression="Mem" />
                <asp:HyperLinkField DataNavigateUrlFields="LocalProcessId" 
                    DataNavigateUrlFormatString="~/Members/MyLocalProcessLog.aspx?LocalProcessId={0}%" 
                    Text="Log &amp;raquo;" />
            </Columns>
        </asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    DeleteCommand="DELETE FROM [LocalProcessState] WHERE [LocalProcessStateId] = @LocalProcessStateId" 
    InsertCommand="INSERT INTO [LocalProcessState] ([LocalProcessStateId], [LocalProcessId], [OwnerId], [Created], [Modified], [Cpu], [Mem]) VALUES (@LocalProcessStateId, @LocalProcessId, @OwnerId, @Created, @Modified, @Cpu, @Mem)" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    SelectCommand="SELECT [LocalProcessStateId], [LocalProcessId], [OwnerId], [Created], [Modified], [Cpu], [Mem] FROM [LocalProcessState] ORDER BY Created" 
    
    UpdateCommand="UPDATE [LocalProcessState] SET [LocalProcessId] = @LocalProcessId, [OwnerId] = @OwnerId, [Created] = @Created, [Modified] = @Modified, [Cpu] = @Cpu, [Mem] = @Mem WHERE [LocalProcessStateId] = @LocalProcessStateId">
    <SelectParameters>
        <asp:SessionParameter Name="OwnerId" SessionField="ParticipantId" 
            Type="Object" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="LocalProcessStateId" Type="Object" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="LocalProcessStateId" Type="Object" />
        <asp:Parameter Name="LocalProcessId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="Created" Type="DateTime" />
        <asp:Parameter Name="Modified" Type="DateTime" />
        <asp:Parameter Name="Cpu" Type="Double" />
        <asp:Parameter Name="Mem" Type="Double" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="LocalProcessId" Type="Object" />
        <asp:Parameter Name="OwnerId" Type="Object" />
        <asp:Parameter Name="Created" Type="DateTime" />
        <asp:Parameter Name="Modified" Type="DateTime" />
        <asp:Parameter Name="Cpu" Type="Double" />
        <asp:Parameter Name="Mem" Type="Double" />
        <asp:Parameter Name="LocalProcessStateId" Type="Object" />
    </UpdateParameters>
</asp:SqlDataSource>


