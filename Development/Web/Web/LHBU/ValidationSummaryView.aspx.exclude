<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="ValidationSummaryView.aspx.vb" Inherits="SLIK_ValidationSummaryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
            <ext:GridPanel ID="gridView" runat="server">
                <Store>
                    <ext:Store ID="storeView" runat="server"  OnReadData="StoreView_ReadData" RemoteFilter="true" RemoteSort="true" >
                        <Model>
                            <ext:Model ID="modelView" runat="server" IDProperty="PK_Record_ID">
                                <Fields>
                                    <ext:ModelField Name="PK_Record_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="SegmentData" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Status" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Bulan" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Tahun" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TanggalData" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="ModuleURL" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="KodeCabang" Type="String"></ext:ModelField>
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
                        <ext:DateColumn ID="TanggalData" runat="server" DataIndex="TanggalData" Text="Report Date" Width="120px">
                            <Items>
                                <ext:DateField ID="TanggalDataFilter" runat="server" Format="dd-MMM-yyyy" >
                                    <Plugins>
                                        <ext:ClearButton runat="server" /> 
                                    </Plugins>
                                </ext:DateField>
                            </Items>
                        </ext:DateColumn>
                        <ext:Column ID="colKodeCabang" runat="server" DataIndex="KodeCabang" Text="Kode Cabang" Width="105px"></ext:Column>
                        <ext:Column ID="colSegmentData" runat="server" DataIndex="SegmentData" Text="Form Name" Width="400px"></ext:Column>
                        <ext:HyperlinkColumn ID="colStatus" runat="server" DataIndex="Status" DataIndexHref="ModuleURL" Text="Status" Width="200px" HrefTarget="_self"></ext:HyperlinkColumn>
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader id="GridHeader1" runat="server" Remote="true"/>
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
                </BottomBar>
                <TopBar>
                    <ext:Toolbar runat="server" ID="toolbar1">
                        <Items>
                            <ext:ComboBox runat="server" ID="cboExportExcel" Editable="false" EmptyText="[Select Format]" FieldLabel="Export :">
                                <Items>
                                    <ext:ListItem Text="Excel" Value="Excel"></ext:ListItem>
                                    <ext:ListItem Text="CSV" Value="CSV"></ext:ListItem>
                                </Items>
                            </ext:ComboBox>
                            <ext:Button runat="server" ID="BtnExport" Text="Export Current Page" AutoPostBack="true" OnClick="ExportExcel" ClientIDMode="Static" />
                            <ext:Button runat="server" ID="BtnExportAll" Text="Export All Page" AutoPostBack="true" OnClick="ExportAllExcel" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
     
</asp:Content>

