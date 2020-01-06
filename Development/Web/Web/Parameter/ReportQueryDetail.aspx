<%@ page title="" language="VB" masterpagefile="~/Site1.Master" autoeventwireup="false" inherits="ReportQueryDetail, App_Web_reportquerydetail.aspx.252c98" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var columnAutoResize = function (grid) {
            
            var view = grid.getView(),
                store = grid.getStore(),
                colModel = grid.getColumnModel(),
                columns = colModel.config,
                maxAutoWidth = 250, //0 to disable
                cell,
                value,
                width = 0;

            Ext.each(columns, function (column, colIdx) {
                // Data Width
                var colWidth = width;
                store.each(function (record, rowIdx) {
                    cell = view.getCell(rowIdx, colIdx);
                    value = record.get(column.dataIndex);
                    colWidth = Math.max(colWidth, Ext.util.TextMetrics.measure(cell, value).width);
                });

                if (!column.isRowNumberer) {
                    // Header Width
                    header = view.getHeaderCell(colIdx);
                    headerWidth = Ext.util.TextMetrics.measure(header, column.header).width;
                }
                else {
                    // Calc width using total rows
                    lengthWidth = store.getTotalCount().toString().length;
                    headerWidth = (lengthWidth * 10) + (lengthWidth > 1 ? 0 : 10);
                }

                // Choose the biggest width
                if (colWidth < headerWidth || colWidth == 0) {
                    colWidth = headerWidth;
                }

                // Max Length
                if (colWidth > maxAutoWidth && maxAutoWidth > 0) {
                    colWidth = maxAutoWidth;
                }

                //Add space to avoid ...
                if (!column.isRowNumberer) {
                    colWidth += 20
                }
                
                colModel.setColumnWidth(colIdx, colWidth);
            });
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:FormPanel ID="FormPanelInput" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" ButtonAlign="Center" DefaultAnchor="100%" AutoScroll="true" Flex="1">
        <Content>
            <ext:GridPanel ID="GridPreview" runat="server" ClientIDMode="Static" Title="Preview" Height="300">
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <Store>
                    <ext:Store ID="storePreview" runat="server" IsPagingStore="true" RemoteFilter="true" RemoteSort="true" OnReadData="storePreview_Readdata" RemotePaging="true" ClientIDMode="Static">
                        <Sorters>
                        </Sorters>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                         
                    </ext:Store>
                   
                </Store>
                <Listeners>
                    
                        <ViewReady Handler="columnAutoResize(this);
                    this.getStore().on('load', Ext.createDelegate(columnAutoResize, null, [this]));" 
           Delay="100" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" />
                </BottomBar>
            </ext:GridPanel>
        </Content>
        <Buttons>
            <%-- <ext:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="Preview" Icon="ApplicationViewDetail">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Callback">
                        <ExtraParams>
                            <ext:Parameter Name="command" Value="Preview" Mode="Value">
                            </ext:Parameter>

                        </ExtraParams>
                        <EventMask ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>--%>
            <ext:Button ID="btnCancel" runat="server" Text="Back" Icon="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click">
                        <EventMask ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>

    </ext:FormPanel>
    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%">
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
            </ext:Button>
        </Buttons>
    </ext:FormPanel>

</asp:Content>

