<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="ApplicationParameterView.aspx.vb" Inherits="ApplicationParameterView" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript" >


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
   
    <ext:GridPanel ID="GridpanelView" runat="server" Title="Title" Layout="FitLayout">
         <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
        <Store>
            <ext:Store ID="StoreView" runat="server" RemoteFilter="true" RemoteSort="true" OnReadData="Store_ReadData">

                <Sorters>
                    <%--<ext:DataSorter Property="" Direction="ASC" />--%>
                </Sorters>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
            </ext:Store>
        </Store>
        <Plugins>
            <%--<ext:GridFilters ID="GridFilters1" runat="server" />--%>
            <ext:FilterHeader ID="grdiheaderfilter" runat="server" Remote="true"></ext:FilterHeader>

        </Plugins>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" />
        </BottomBar>
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                <Items>

                    <ext:ComboBox runat="server" ID="cboExportExcel" Editable="false"  FieldLabel="Export :">

                        <Items>
                            <ext:ListItem Text="Excel" Value="Excel"></ext:ListItem>
                            <ext:ListItem Text="CSV" Value="CSV"></ext:ListItem>
                        </Items>

                    </ext:ComboBox>
                    <ext:Button runat="server" ID="BtnExport" Text="Export Current Page" AutoPostBack="true" OnClick="ExportExcel" ClientIDMode="Static" />
                    <ext:Button runat="server" ID="BtnExportAll" Text="Export All Page" AutoPostBack="true" OnClick="ExportAllExcel" />
                    <%--<ext:Button ID="Button1" runat="server" Text="Print Current Page" Icon="Printer" Handler="this.up('grid').print({currentPageOnly : true});" />--%>
                    <ext:Button ID="BtnAdd" runat="server" Text="Add New Record" Icon="Add" Handler="NawadataDirect.BtnAdd_Click()">
                       
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>

    </ext:GridPanel>


</asp:Content>



