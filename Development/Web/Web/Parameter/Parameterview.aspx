<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="Parameterview, App_Web_parameterview.aspx.252c98" %>

<%--<%@ Register Assembly="CodeEffects.Rule" Namespace="CodeEffects.Rule.Asp" TagPrefix="cc1" %>--%>
<%@ Register Src="~/Component/AdvancedFilter.ascx" TagPrefix="uc1" TagName="AdvancedFilter" %>



<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <ext:ResourcePlaceHolder runat="server" Mode="ScriptFiles" />
    <script type="text/javascript">
        //function coba() {
        //    App.GridpanelView.columns.forEach(function (col) {

        //        NawadataDirect.Coba(col.text, col.hidden)

        //    }
        //)
        //}

        var columnAutoResize = function (grid) {
            
            App.GridpanelView.columns.forEach(function (col) {
                   
                if (col.xtype != 'commandcolumn')
                {
                    
                    col.autoSize();
                }
                
            });
        };
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
    <h1>

        
        
        

        <ext:GridPanel ID="GridpanelView"    ClientIDMode="Static" runat="server" Title="Title"    >
          

            <Store>
                <ext:Store ID="StoreView" runat="server" RemoteFilter="true" RemoteSort="true" OnReadData="Store_ReadData">

                    <Sorters>
                        <%--<ext:DataSorter Property="" Direction="ASC" />--%>
                    </Sorters>
                    <Proxy>

                        <ext:PageProxy />
                    </Proxy>
                    <Reader>
                    </Reader>
                </ext:Store>
                
            </Store>
           
            <Plugins>
                <%--<ext:GridFilters ID="GridFilters1" runat="server" />--%>
                <ext:FilterHeader ID="GridHeader1" runat="server" Remote="true" ClientIDMode="Static" />
                
            </Plugins>
             
            <BottomBar>

                <%--   <ext:Toolbar ID="Toolbar2"  runat="server">
                    <Items>
                        
                    </Items>
                </ext:Toolbar>--%>

                <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True">
                    <Items>
                        <ext:Button runat="server" ID="BtnExportForImport" ClientIDMode="Static" Text="Export Selected For Import" AutoPostBack="true" OnClick="ExportExcelForImport" />
                    </Items>
                </ext:PagingToolbar>
            </BottomBar>
            <DockedItems>
                <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true" Dock="Top">
                    <Items>

                        <ext:ComboBox runat="server" ID="cboExportExcel" Editable="false" EmptyText="[Select Format]" FieldLabel="Export :">

                            <Items>
                                <ext:ListItem Text="Excel" Value="Excel"></ext:ListItem>
                                <ext:ListItem Text="CSV" Value="CSV"></ext:ListItem>
                            </Items>

                        </ext:ComboBox>
                        <ext:Button runat="server" ID="BtnExport" Text="Export Current Page" AutoPostBack="true" OnClick="ExportExcel" ClientIDMode="Static" >
                        

                            </ext:Button>
                        <ext:Button runat="server" ID="BtnExportAll" Text="Export All Page" AutoPostBack="true" OnClick="ExportAllExcel" />

                        <%--<ext:Button ID="btnprint" runat="server" Text="Print Current Page" Icon="Printer" Handler="this.up('grid').print({currentPageOnly : true});" >
                           </ext:Button> --%>


                        <ext:Button ID="BtnAdd" runat="server" Text="Add New Record" Icon="Add" Visible="false" Handler="NawadataDirect.BtnAdd_Click()">
                        </ext:Button>
                        <ext:Button ID="AdvancedFilter" runat="server" Text="Advanced Filter" Icon="Add" Handler="NawadataDirect.BtnAdvancedFilter_Click()">
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
                 <ext:Toolbar ID="Toolbar2" runat="server" EnableOverflow="true" Dock="Top" Hidden="true" >
                 <Items>
                     <ext:HyperlinkButton ID="btnClear" runat="server" Text="Clear Advanced Filter">
                         <DirectEvents>
                             <Click OnEvent="btnClear_Click"> </Click>
                         </DirectEvents>
                     </ext:HyperlinkButton> <ext:Label ID="LblAdvancedFilter" runat="server" Text="" >
                    </ext:Label>    
                 </Items>
                 </ext:Toolbar>
            </DockedItems>
            <Listeners>
                <ViewReady Handler="columnAutoResize(this);
                    this.getStore().on('load', Ext.bind(columnAutoResize, null, [this]));"
                    Delay="10" />
            </Listeners>
            <View>
                <ext:GridView runat="server" EnableTextSelection="true"  />
            </View>
            
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi">
                </ext:CheckboxSelectionModel>
            </SelectionModel>
        </ext:GridPanel>

        <ext:Panel ID="Panel1" runat="server" Hidden="true">
            <Content>
                <uc1:AdvancedFilter runat="server" ID="AdvancedFilter1" />
            </Content>
            <Items>
            </Items>
        </ext:Panel>
    </h1>
</asp:Content>
