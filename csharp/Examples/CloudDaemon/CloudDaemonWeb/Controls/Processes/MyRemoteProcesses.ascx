<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyRemoteProcesses.ascx.cs" Inherits="CloudDaemonWeb.Controls.Processes.MyRemoteProcesses" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.DynamicData" tagprefix="cc1" %>
<%@ Import Namespace="DaemonLogic" %>

Local process with which to attach new process:
<asp:DropDownList ID="LocalProcessList" runat="server" 
    DataSourceID="LocalProcessSqlDS" DataTextField="Name" 
    DataValueField="LocalProcessId">
</asp:DropDownList>
&nbsp;<asp:Button ID="AddRemoteProcessButton" runat="server" 
    onclick="AddRemoteProcessButton_Click" Text="Attach Process" />
<br />
<br />
<asp:GridView ID="RemoteProcessView" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="RemoteProcessId" DataSourceID="RemoteProcessSqlDS" 
    AllowPaging="True" AllowSorting="True">
    <Columns>
        <asp:BoundField DataField="RemoteProcessId" HeaderText="RemoteProcessId" 
            ReadOnly="True" SortExpression="RemoteProcessId" Visible="False" />
        <asp:BoundField DataField="LocalProcessId" HeaderText="LocalProcessId" 
            SortExpression="LocalProcessId" ReadOnly="True" Visible="False" />
        <asp:BoundField DataField="OwnerId" HeaderText="OwnerId" ReadOnly="True" 
            Visible="False" SortExpression="OwnerId" />
        <asp:TemplateField HeaderText="Local Process">
            <EditItemTemplate>
                <asp:DropDownList ID="LocalProcessList" runat="server" 
                    DataSourceID="LocalProcessSqlDS" DataTextField="Name" 
                    DataValueField="LocalProcessId" SelectedValue='<%# Bind("LocalProcessId") %>'>
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:DropDownList ID="LocalProcessList" runat="server" 
                    DataSourceID="LocalProcessSqlDS" 
                    SelectedValue='<%# Bind("LocalProcessId") %>' DataTextField="Name" 
                    DataValueField="LocalProcessId" Enabled="False">
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Address" HeaderText="Remote Process Address" 
            SortExpression="Address" />
        <asp:BoundField DataField="HubPort" HeaderText="Remote Process Hub Port" 
            SortExpression="HubPort" />
        <asp:CheckBoxField DataField="Trusted" HeaderText="Trusted" 
            SortExpression="Trusted" />
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    </Columns>
</asp:GridView>
<asp:SqlDataSource ID="RemoteProcessSqlDS" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    SelectCommand="SELECT * FROM [RemoteProcess] WHERE ([OwnerId] = @OwnerId)" 
    DeleteCommand="DELETE FROM [RemoteProcess] WHERE [RemoteProcessId] = @RemoteProcessId" 
    InsertCommand="INSERT INTO [RemoteProcess] ([RemoteProcessId], [LocalProcessId], [OwnerId], [Address], [HubPort], [Trusted]) VALUES (@RemoteProcessId, @LocalProcessId, @OwnerId, @Address, @HubPort, @Trusted)" 
    
    UpdateCommand="UPDATE [RemoteProcess] SET [LocalProcessId] = @LocalProcessId,[Address] = @Address, [HubPort] = @HubPort, [Trusted] = @Trusted WHERE [RemoteProcessId] = @RemoteProcessId">
    <SelectParameters>
        <asp:SessionParameter Name="OwnerId" SessionField="ParticipantId" 
            Type="Object" DefaultValue="c32544de-7b78-4006-a885-8d937bcd84bd" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="RemoteProcessId" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="LocalProcessId" />
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Trusted" Type="Boolean" />
        <asp:Parameter Name="RemoteProcessId" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="RemoteProcessId" />
        <asp:Parameter Name="LocalProcessId" />
        <asp:Parameter Name="OwnerId" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="HubPort" Type="Int32" />
        <asp:Parameter Name="Trusted" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="LocalProcessSqlDS" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CloudDaemonWeb %>" 
    
    SelectCommand="SELECT [LocalProcessId], [Name] FROM [LocalProcess] WHERE OwnerId=@OwnerId">
    <SelectParameters>
        <asp:SessionParameter Name="OwnerId" SessionField="ParticipantId" />
    </SelectParameters>
</asp:SqlDataSource>
