<%@ page title="" language="VB" masterpagefile="~/Site1.Master" autoeventwireup="false" inherits="ReportQueryUserView, App_Web_reportqueryuserview.aspx.cdcab7d2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript" >


         //Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
             
         //      return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
         //  };


         //  Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
         //      return {
         //          type: "string",
         //          op: "*",
         //          value: value
         //      };
         //  };


           var columnAutoResize = function (grid) {
               
               
               grid.columns.forEach(function (col) {
                   

                   if (col.xtype != 'commandcolumn') {

                       col.autoSize();
                   }

               });
           };

    </script>
    <script type="text/javascript">
        function GridClick() {


            //record = App.ContentPlaceHolder1_GridPaneltask.getSelectionModel().selected.items[0];            
            //App.ContentPlaceHolder1_cboReport.setValue(record.get(App.ContentPlaceHolder1_GridPaneltask.columns[0].dataIndex) + '-' + record.get(App.ContentPlaceHolder1_GridPaneltask.columns[1].dataIndex));
            //App.ContentPlaceHolder1_GridPaneltask.getSelectionModel().clearSelections()
            //App.ContentPlaceHolder1_Windowtask.hide();

            record = window.App.GridPaneltask.getSelectionModel().selected.items[0];
            window.App.cboReport.setValue(record.get(window.App.GridPaneltask.columns[0].dataIndex) + '-' + record.get(window.App.GridPaneltask.columns[1].dataIndex));
            window.App.GridPaneltask.getSelectionModel().clearSelections()
            window.App.Windowtask.hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>

            <ext:DropDownField ID="cboReport" runat="server" Editable="false" TriggerIcon="SimpleArrowDown" FieldLabel="Report Query" AnchorHorizontal="40%" AllowBlank="false" ClientIDMode="Static">

                <Listeners>
                    <Expand Handler="this.picker.setWidth(500);" />
                </Listeners>
                <Component>
                    <ext:Window ID="Windowtask" runat="server" Collapsible="true" Height="300" Icon="Application" Title="Process Picker" Width="340" Layout="FitLayout" ClientIDMode="Static">
                        <Items>
                            <ext:GridPanel ID="GridPaneltask" runat="server" ClientIDMode="Static">
                                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                                <Store>
                                    <ext:Store ID="storepicker" runat="server" IsPagingStore="true" RemoteFilter="true" RemoteSort="true" OnReadData="Storetrigger_Readdata" RemotePaging="true">
                                        <Sorters>
                                        </Sorters>
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolbar2" runat="server" HideRefresh="True" />
                                </BottomBar>
                            </ext:GridPanel>

                        </Items>
                    </ext:Window>
                </Component>
                <Triggers>
                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                </Triggers>
                <Listeners>
                    <Change Handler="this.getTrigger(0).show();" />
                    <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                </Listeners>
            </ext:DropDownField>

            <ext:GridPanel ID="GridPreview"  runat="server" ClientIDMode="Static" Title="Preview" Layout="FitLayout"   AutoScroll="true" Hidden="true">
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <Store>
                    <ext:Store ID="storePreview" runat="server"  RemoteFilter="true" RemoteSort="true" OnReadData="storePreview_Readdata"  ClientIDMode="Static"  AutoLoad="false">
                        <Sorters>
                        </Sorters>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                       <%-- <Model>
                            <ext:Model ID="Model1" runat="server" ClientIDMode="Static" />
                        </Model>--%>
                    </ext:Store>
                </Store>
                <Plugins>

                    <ext:FilterHeader ID="GridHeader1"  runat="server"  Remote="true"  DateFormat="dd-MMM-yyyy"  />
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="true" />
                                                
                </BottomBar>
                 <%-- <Listeners>
                    
                      <ViewReady Handler="columnAutoResize(this);
                    this.getStore().on('load', Ext.bind(columnAutoResize, null, [this]));" 
           Delay="10" />
                </Listeners>--%>
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                        <Items>

                            <ext:ComboBox runat="server" ID="cboExportExcel" Editable="false" EmptyText="[Select Format]" FieldLabel="Export :">

                                <Items>
                                    <ext:ListItem Text="Excel" Value="Excel"></ext:ListItem>
                                    <ext:ListItem Text="CSV" Value="CSV"></ext:ListItem>
                                </Items>

                            </ext:ComboBox>
                            <ext:Button runat="server" ID="BtnExport" Text="Export Current Page" AutoPostBack="true" OnClick="ExportExcel" ClientIDMode="Static" />
                            <ext:Button runat="server" ID="BtnExportAll" Text="Export All Page" AutoPostBack="true" OnClick="ExportAllExcel" />

                            <%--<ext:Button ID="Button1" runat="server" Text="Print Current Page" Icon="Printer" Handler="this.up('grid').print({currentPageOnly : true});" />--%>

                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
        </Items>

        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="View Report">
                <%--<Listeners>
                    <Click Handler="#{GridPreview}.store.load()"></Click>
                </Listeners>--%>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%" Hidden="true">
        <Defaults>
            <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
        </Defaults>
        <LayoutConfig>
            <ext:HBoxLayoutConfig Padding="5" Align="Middle" Pack="Center" />
        </LayoutConfig>
        <Items>
            <ext:Label ID="LblConfirmation" runat="server" Align="center" Cls="NawaLabel">
            </ext:Label>

        </Items>

        <Buttons>

            <ext:Button ID="BtnConfirmation" runat="server" Text="OK" Icon="ApplicationGo">
                <DirectEvents>
                    <Click OnEvent="BtnConfirmation_DirectClick"></Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>

