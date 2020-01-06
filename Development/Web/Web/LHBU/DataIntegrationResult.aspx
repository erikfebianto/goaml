<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="DataIntegrationResult.aspx.vb" Inherits="LHBU_DataIntegrationResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        var prepareToolbar = function (grid, toolbar, rowIndex, record) {
            if (record.get("ErrorMessage") == '') {
                //hide Download button
                toolbar.items.getAt(0).hide();
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <ext:GridPanel ID="gridFileList" runat="server">
        <Store>
            <ext:Store ID="storeFileList" runat="server" OnReadData="StoreView_ReadData" RemoteFilter="true" RemoteSort="true">
                <Model>
                    <ext:Model ID="modelFileList" runat="server" IDProperty="PK_GeneratedFileList_ID">
                        <Fields>
                            <ext:ModelField Name="Pk_TextFileTemporaryTable_ID" Type="Auto"></ext:ModelField>
                            <ext:ModelField Name="FileName" Type="String"></ext:ModelField>
                            <ext:ModelField Name="FK_FormInfo_ID" Type="String"></ext:ModelField>
                            <ext:ModelField Name="DataSource" Type="String"></ext:ModelField>
                            <ext:ModelField Name="TanggalData" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="Status" Type="String"></ext:ModelField>
                            <ext:ModelField Name="StartDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="EndDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="TotalRecord" Type="Int"></ext:ModelField>
                            <ext:ModelField Name="ErrorMessage" Type="String"></ext:ModelField>

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
                <ext:Column ID="FileName" runat="server" DataIndex="FileName" Text="File Name" Width="80px"></ext:Column>
                <ext:Column ID="FK_FormInfo_ID" runat="server" DataIndex="FK_FormInfo_ID" Text="Form Name" Width="100px"></ext:Column>
                <ext:Column ID="DataSource" runat="server" DataIndex="DataSource" Text="Data Source" Width="100px"></ext:Column>
                <ext:DateColumn ID="TanggalData" runat="server" DataIndex="TanggalData" Text="Tanggal Data" Width="100px">
                    <Items>
                        <ext:DateField ID="DateField1" runat="server" Format="dd-MMM-yyyy" >
                            <Plugins>
                                <ext:ClearButton runat="server" /> 
                            </Plugins>
                        </ext:DateField>
                    </Items>
                </ext:DateColumn>
                <ext:Column ID="Status" runat="server" DataIndex="Status" Text="Status" Width="100px"></ext:Column>
                <ext:DateColumn ID="colStartDate" runat="server" DataIndex="StartDate" Text="Start Date" Width="100px">
                    <Items>
                        <ext:DateField ID="TanggalDataFilter" runat="server" Format="dd-MMM-yyyy" >
                            <Plugins>
                                <ext:ClearButton runat="server" /> 
                            </Plugins>
                        </ext:DateField>
                    </Items>
                </ext:DateColumn>
                <ext:DateColumn ID="colCompleteDate" runat="server" DataIndex="EndDate" Text="End Date" Width="100px">
                    <Items>
                        <ext:DateField ID="DateField2" runat="server" Format="dd-MMM-yyyy" >
                            <Plugins>
                                <ext:ClearButton runat="server" /> 
                            </Plugins>
                        </ext:DateField>
                    </Items>
                </ext:DateColumn>
                <ext:Column ID="TotalRecord" runat="server" DataIndex="TotalRecord" Text="Total Record" Width="100px"></ext:Column>
                <ext:CommandColumn ID="downnload" runat="server" Width="120">
                    <Commands>
                        <ext:GridCommand CommandName="Download" Icon="DiskDownload" Text="Download File"></ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="GridCommand">
                            <ExtraParams>
                                <ext:Parameter Name="unikkeyDP" Value="record.data.Pk_TextFileTemporaryTable_ID" Mode="Raw"></ext:Parameter>
                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                </ext:CommandColumn>
                <ext:Column ID="ErrorMessage" runat="server" DataIndex="ErrorMessage" Text="Error Message" Width="100px"></ext:Column>
                <ext:CommandColumn ID="DownloadError" runat="server" Width="120" AutoShow ="false">
                    <Commands>
                        <ext:GridCommand CommandName="DownloadError" Icon="ErrorAdd" Text="Download Error"></ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="GridCommandError">
                            <ExtraParams>
                                <ext:Parameter Name="unikkeyDE" Value="record.data.Pk_TextFileTemporaryTable_ID" Mode="Raw" ></ext:Parameter>
                                <ext:Parameter Name="command" Value="command" Mode="Raw" ></ext:Parameter>
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                    <PrepareToolbar Fn="prepareToolbar" />
                </ext:CommandColumn>
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:CheckboxSelectionModel ID="SelectionModel" runat="server" Mode="Multi" />
        </SelectionModel>
        <Plugins>
            <ext:FilterHeader ID="GridHeader1" runat="server" Remote="true" ClientIDMode="Static" />
        </Plugins>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
        </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="toolbar1">
                <Items>

                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:GridPanel>

</asp:Content>
