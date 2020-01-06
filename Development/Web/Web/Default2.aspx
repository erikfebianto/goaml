<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" MasterPageFile="~/Site1.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/Chooser.css" />
    <script type="text/javascript">
        Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
            return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
        };
        
        Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
            return {
                type: "string",
                op: "*",
                value: value
            };
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:Panel ID="panelmain" runat="server" Layout="FitLayout" Border="false" AutoScroll="true">
        <Items>
            <ext:Portal ID="Portal1" runat="server" Border="false" BodyStyle="background-color: transparent;" BodyPadding="0">
                <Items>
                    <ext:PortalColumn ID="PortalShortcut" runat="server" ColumnWidth="1" PaddingSpec="0 0" Hidden="true">
                        <Items>
                            <ext:Portlet ID="PortletShortcut" runat="server" Title="Getting Started" Closable="false" Collapsible="true" Draggable="false" MinHeight="210" BodyPadding="5" StyleSpec="margin: 0 0 10 0;">
                                <Items>
                                    <ext:UserControlLoader ID="UserControl1" runat="server" Path="Component/GetttingStarted.ascx" ClientIDMode="Static"></ext:UserControlLoader>
                                </Items>
                            </ext:Portlet>
                        </Items>
                    </ext:PortalColumn>
                    <ext:PortalColumn runat="server" ColumnWidth=".5" PaddingSpec="0 8 0 0">
                        <Items>
                            <ext:Portlet ID="Portlet1" runat="server" Title="Task List" Closable="false" Collapsible="true" >
                                <Items>
                                    <ext:GridPanel ID="GridTaskList" runat="server" Height="200">
                                        <Store>
                                            <ext:Store ID="StoreView" runat="server" ClientIDMode="Static" RemoteFilter="true" RemoteSort="true" OnReadData="Store_ReadData">
                                                <Sorters>
                                                </Sorters>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                        <Plugins>
                                            <ext:FilterHeader ID="grdiheaderfilter" runat="server" Remote="true"></ext:FilterHeader>
                                        </Plugins>
                                        <BottomBar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Portlet>
                        </Items>
                    </ext:PortalColumn>
                    <ext:PortalColumn runat="server" ColumnWidth=".5" PaddingSpec="0">
                        <Items>
                            <ext:Portlet ID="Portlet2" runat="server" Title="My Request for Approval" Closable="false" Collapsible="true" Draggable="false">
                                <Items>
                                    <ext:GridPanel ID="GridApproval" runat="server" Height="200">
                                        <Store>
                                            <ext:Store ID="StoreApproval" runat="server" ClientIDMode="Static" RemoteFilter="true" RemoteSort="true" OnReadData="StoreApproval_ReadData">
                                                <Model>
                                                    <ext:Model ID="modelApproval" runat="server" IDProperty="PK_Record_ID">
                                                        <Fields>
                                                            <ext:ModelField Name="jumlah" Type="Auto"></ext:ModelField>
                                                            <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                                            <ext:ModelField Name="mMenuURL" Type="String"></ext:ModelField>
                                                            <ext:ModelField Name="ModuleID" Type="String"></ext:ModelField>
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Proxy>
                                                    <ext:PageProxy>
                                                    </ext:PageProxy>
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column ID="jumlah" runat="server" DataIndex="jumlah" Text="Count" Width="100px"></ext:Column>
                                                <ext:Column ID="ModuleName" runat="server" DataIndex="ModuleName" Text="Model Name" Width="300px"></ext:Column>

                                                <ext:CommandColumn ID="downnload" runat="server" Width="100" Text ="Detail">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Download" Icon="ApplicationViewDetail" Text="Detail"></ext:GridCommand>
                                                    </Commands>
                                                    <DirectEvents>
                                                        <Command OnEvent="ApprovalDetail_OnClicked">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="mMenuURL" Value="record.data.mMenuURL" Mode="Raw"></ext:Parameter>
                                                                <ext:Parameter Name="ModuleID" Value="record.data.ModuleID" Mode="Raw"></ext:Parameter>
                                                            </ExtraParams>
                                                        </Command>
                                                    </DirectEvents>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <Plugins>
                                            <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true"></ext:FilterHeader>
                                        </Plugins>
                                        <BottomBar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Portlet>
                        </Items>
                    </ext:PortalColumn>
                </Items>
            </ext:Portal>
        </Items>
    </ext:Panel>
</asp:Content>
