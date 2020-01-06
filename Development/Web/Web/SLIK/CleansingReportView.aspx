<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="CleansingReportView.aspx.vb" Inherits="SLIK_CleansingReportView" %>

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
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:GridPanel ID="gridFileList" runat="server">
                <Store>
                    <ext:Store ID="storeFileList" runat="server"  OnReadData="StoreView_ReadData">
                        <Model>
                            <ext:Model ID="modelFileList" runat="server" IDProperty="PK_Record_ID">
                                <Fields>
                                    <ext:ModelField Name="PK_Record_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="SegmentData" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="KeyField" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="KeyFieldValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="NamaField" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="OriginalValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="CleanedValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Status" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Bulan" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Tahun" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FK_KodeKantorCabang" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UniqueField" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UniqueValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ModuleID" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="LastUpdateBy" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="LastUpdateDate" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="Clean" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="Keep" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="CleansingRules" Type="String"></ext:ModelField>
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
                        <ext:Column ID="colSegmentData" runat="server" DataIndex="SegmentData" Text="Segment Data" Width="150px"></ext:Column>
                        <ext:Column ID="colCleansingRules" runat="server" DataIndex="CleansingRules" Text="Cleansing Rules" Width="200px"></ext:Column>
                        <ext:Column ID="colUniqueField" runat="server" DataIndex="UniqueField" Text="Unique Field" Width="150px"></ext:Column>
                        <ext:Column ID="colUniqueValue" runat="server" DataIndex="UniqueValue" Text="Unique Value" Width="150px"></ext:Column>
                        <ext:Column ID="colNamaField" runat="server" DataIndex="NamaField" Text="Nama Field" Width="150px"></ext:Column>
                        <ext:Column ID="colOriginalValue" runat="server" DataIndex="OriginalValue" Text="Original Value" Width="150px"></ext:Column>
                        <ext:Column ID="colCleanedValue" runat="server" DataIndex="CleanedValue" Text="Cleaned Value" Width="150px"></ext:Column>
                        <ext:Column ID="colStatus" runat="server" DataIndex="Status" Text="Status" Width="150px"></ext:Column>
                        <ext:Column ID="colLastUpdateBy" runat="server" DataIndex="LastUpdateBy" Text="Cleaned By" Width="150px"></ext:Column>
                        <ext:DateColumn ID="colLastUpdateDate" runat="server" DataIndex="LastUpdateDate" Text="Cleaned Date" Format="dd-MMM-yyyy HH:mm" Width="150px"></ext:DateColumn>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="SelectionModel" runat="server" Mode="Multi" />
                </SelectionModel>
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
                            <ext:Button runat="server" ID="btnClean" Text="Clean Selected">
                                <DirectEvents>
                                    <Click OnEvent="CleanSelected">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button runat="server" ID="btnCleanAll" Text="Clean All"> 
                                <DirectEvents>
                                    <Click OnEvent="CleanAll">

                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnKeep" Text="Keep Selected">
                                <DirectEvents>
                                    <Click OnEvent="KeepSelected">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                                </ext:Button>
                            <ext:Button runat="server" ID="btnKeepAll" Text="Keep All"> 
                                <DirectEvents>
                                    <Click OnEvent="KeepAll">

                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
        </Items>
    </ext:FormPanel>
</asp:Content>

