<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NDSDropDownField.ascx.vb" Inherits="NDSDropDownField" %>
<script type="text/javascript">

        function GridClick(cbodata,griddata,objWindow) { 
            record = griddata.getSelectionModel().selected.items[0];
            //cbodata.setValue(record.get(griddata.columns[0].dataIndex) + '-' + record.get(griddata.columns[1].dataIndex));
            cbodata.setValue(record.get(griddata.columns[1].dataIndex));
            griddata.getSelectionModel().deselectAll();
            objWindow.hide() 
        }

        //function syncValue(values) {
        //    if (values !== '' && values) { 
        //        NawadataDirect.combobox1_TextChanged();
        //    }
        //}
    </script>

<ext:DropDownField 
    ID="ComboBox1" 
    runat="server" 
    Editable="false" 
    TriggerIcon="SimpleArrowDown"  
    > 
<%--    <SyncValue Fn="syncValue" />--%>
    <Listeners>
        <Expand Handler="this.picker.setWidth(600);#{store}.reload();" />
    </Listeners>
    <Component>
        <ext:Window 
            ID="Window" 
            runat="server" 
            Collapsible="true" 
            Height="300" 
            Icon="Application" 
            Title="Picker" 
            Width="340" 
            Layout="FitLayout"  >
            <Items>  
                <ext:GridPanel ID="GridPanel" runat="server" >
                    <DirectEvents>
                        <Select OnEvent="GridPanel_OnClick">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                        </Select>
                    </DirectEvents>
                    <View>
                        <ext:GridView runat="server" EnableTextSelection="true" />
                    </View> 
                    <Listeners>
                        <%--<RowClick Handler="Ext.net.DirectMethods.RowOnClicked();"/>--%>
                        <BeforeRender Handler="this.columns[0].setVisible(false);" />
                    </Listeners>
                    <Store>
                        <ext:Store 
                            ID="store" 
                            runat="server"  
                            IsPagingStore="true" 
                            RemoteFilter="true" 
                            RemoteSort="true" 
                            OnReadData="storepicker_ReadData" 
                            RemotePaging="true"  >
                            <Sorters></Sorters>
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                        </ext:Store>
                    </Store>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar2" runat="server" HideRefresh="false" >
                            <Items>
                                <ext:Button runat="server" Text="Close Picker" ID="ClosePicker">
                                    <Listeners>
                                        <Click Handler="#{Window}.hide()"> </Click>
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:PagingToolbar>    
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Window>
    </Component>
    <Triggers>
        <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
    </Triggers>
    <Listeners>
        <AfterRender Handler="this.getTrigger(0).show();" />
        <Change Handler="this.getTrigger(0).show();" />
        <TriggerClick Handler="if (index == 0) {
                                this.getTrigger(0).hide();                        
                                this.clearValue(); 
                            }" />
    </Listeners>
</ext:DropDownField>
 