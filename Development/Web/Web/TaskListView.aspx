<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="TaskListView.aspx.vb" Inherits="TaskListView" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
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
                                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" />
                                        </BottomBar>
                                    </ext:GridPanel>


</asp:Content>