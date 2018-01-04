<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyLocalProcessLog.ascx.cs" Inherits="CloudDaemonWeb.Controls.Processes.MyLocalProcessLog" %>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    ProviderName="<%$ ConnectionStrings:CloudDaemonWeb.ProviderName %>" 
    
    SelectCommand="SELECT TOP 2000 [Id], [Date], [Thread], [Level], [Logger], [Message], [Exception] FROM [Log] WHERE ([Thread] LIKE @Thread) ORDER BY [Date] DESC">
    <SelectParameters>
        <asp:QueryStringParameter Name="Thread" QueryStringField="LocalProcessId" 
            Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick">
        </asp:Timer>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource1" 
            EmptyDataText="There are no data records to display." 
            EnableViewState="False">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                <asp:BoundField DataField="Thread" HeaderText="Thread" SortExpression="Thread" 
                    Visible="False" />
                <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" />
                <asp:BoundField DataField="Logger" HeaderText="Logger" 
                    SortExpression="Logger" />
                <asp:BoundField DataField="Message" HeaderText="Message" 
                    SortExpression="Message" />
                <asp:BoundField DataField="Exception" HeaderText="Exception" 
                    SortExpression="Exception" />
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
