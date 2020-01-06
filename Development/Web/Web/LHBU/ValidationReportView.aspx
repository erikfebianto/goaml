<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="ValidationReportView.aspx.vb" Inherits="SLIK_ValidationReportView" %>

<%@ Register Src="~/Component/AdvancedFilter.ascx" TagPrefix="uc1" TagName="AdvancedFilter" %>
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

        var FilterSegmen = function (value) {
            window.App.GridpanelView.filterHeader.setValue({ SegmentData: '' + value + '' });
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:GridPanel ID="GridpanelView" runat="server" Title="Title" ClientIDMode="Static">
            <Store>
                <ext:Store ID="StoreView" runat="server"  OnReadData="StoreView_ReadData" RemoteFilter="true"  RemoteSort="true" >
                    <Model>
                        <ext:Model ID="ModelView" runat="server" IDProperty="PK_ID">
                            <Fields>
                                <ext:ModelField Name="PK_ID" Type="Auto"></ext:ModelField>
                                <ext:ModelField Name="SegmentData" Type="String"></ext:ModelField>                                
                                <ext:ModelField Name="MessageDetailView" Type="String"></ext:ModelField>
                                <ext:ModelField Name="KeyFieldValue" Type="String"></ext:ModelField>
                                <ext:ModelField Name="KeyField" Type="String"></ext:ModelField>
                                <ext:ModelField Name="ReferenceField" Type="String"></ext:ModelField>
                                <ext:ModelField Name="ReferenceValue" Type="String"></ext:ModelField>
                                <ext:ModelField Name="FK_OJK_KodeKantorCabang" Type="string"></ext:ModelField>                                                                
                                <ext:ModelField Name="ModuleURL" Type="String"></ext:ModelField>
                                <ext:ModelField Name="RecordID" Type="String"></ext:ModelField>
                                <ext:ModelField Name="ModuleID" Type="Int"></ext:ModelField>
                                <ext:ModelField Name="Edited" Type="Boolean"></ext:ModelField>
                                <ext:ModelField Name="TanggalData" Type="Date"></ext:ModelField>
                                <ext:ModelField Name="KodeCabang" Type="String"></ext:ModelField>
                            </Fields>
                        </ext:Model>

                    </Model>
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
        <ColumnModel>
            <Columns>
                <ext:Column ID="colSegmentData" runat="server" DataIndex="SegmentData" Text="Segment Data" Flex="1" MinWidth="200" CellWrap="true"></ext:Column>
                <ext:DateColumn ID="colTanggalData" runat="server" DataIndex="TanggalData" Text="Report Date" Flex="1" MinWidth="105">
                    <Items>
                        <ext:DateField ID="TanggalDataFilter" runat="server" Format="dd-MMM-yyyy" >
                            <Plugins>
                                <ext:ClearButton runat="server" /> 
                            </Plugins>
                        </ext:DateField>
                    </Items>
                </ext:DateColumn>
                <ext:Column ID="colKodeCabang" runat="server" DataIndex="KodeCabang" Text="Kode Cabang" Flex="1" MinWidth="105"></ext:Column>
                <ext:Column ID="colKeyField" runat="server" DataIndex="ReferenceField" Text="Reference Field" Flex="1" MinWidth="100"></ext:Column>
                <ext:Column ID="colKeyFieldValue" runat="server" DataIndex="ReferenceValue" Text="Reference Value" Flex="1" MinWidth="100"></ext:Column>
                                                
                <ext:Column ID="Column1" runat="server" DataIndex="Edited" Text="Edited" Flex="1" MinWidth="50"></ext:Column>
                <ext:Column ID="colValidationMessage" runat="server" DataIndex="MessageDetailView" Text="Validation Message" Flex="5" MinWidth="100" CellWrap="true"></ext:Column>
                <ext:CommandColumn ID="CommandColumn2" runat="server" Flex="1" Width="450">
                    <Commands>
                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit" ></ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="GridCommand">
                            <ExtraParams>
                                <ext:Parameter Name="unikkey" Value="record.data.PK_ID" Mode="Raw"></ext:Parameter>
                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                </ext:CommandColumn>
            </Columns>
        </ColumnModel>
        <Plugins>
                <ext:FilterHeader id="GridHeader1" runat="server" Remote="true"  ClientIDMode="Static" />
        </Plugins>
        <DockedItems>
            <ext:Toolbar ID="Toolbar" runat="server" EnableOverflow="true">
                <Items>
                    <ext:Button runat="server" ID="BtnExportSelectedRow" Text="Export Selected By Row" AutoPostBack="true" OnClick="ExportExcelSelectedRow" />
                    <ext:Button runat="server" ID="BtnExportAllRow" Text="Export ALL By Row" AutoPostBack="true" OnClick="ExportExcelALLRow" />
                    <ext:Button runat="server" ID="BtnExportSelectedField" Text="Export Selected By Field" AutoPostBack="true" OnClick="ExportExcelSelectedByField" />
                    <ext:Button runat="server" ID="btnExportAllField" Text="Export All By Field" AutoPostBack="true" OnClick="ExportExcelALLByField" />
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
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar2" runat="server" HideRefresh="True" />
        </BottomBar>
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
</asp:Content>