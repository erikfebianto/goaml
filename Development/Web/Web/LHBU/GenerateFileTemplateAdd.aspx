<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="GenerateFileTemplateAdd.aspx.vb" Inherits="GenerateFileTemplateAdd" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function GridClick() {

            record = window.App.GridPaneltask.getSelectionModel().selected.items[0];
            window.App.CboMonitoringDuration.setValue(record.get(window.App.GridPaneltask.columns[0].dataIndex) + '-' + record.get(window.App.GridPaneltask.columns[1].dataIndex));
            window.App.GridPaneltask.getSelectionModel().clearSelections()
            window.App.Windowtask.hide();


        }

        function OpenQuery1() {

            var x = window.open("buttonQuery.aspx?object=1", "popupquery", "width=1000,height=600,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
            x.focus();


        }


        function OpenQuery() {

            var x = window.open("buttonQuery.aspx", "popupquery", "width=1000,height=600,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
            x.focus();


        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Window ID="WindowAdditional" runat="server" Icon="ApplicationViewDetail" Title="Table Reference" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelAdditional" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>
                    <ext:TextField ID="txtTableName" runat="server" FieldLabel="Table Name" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please Fill Table Name Alias">
                    </ext:TextField>
                    <ext:TextArea ID="txtquery" runat="server" FieldLabel="Query Data" AllowBlank="false" EmptyText="Please Fill QueryData. Use @ID for Field unik Name from primary table." Height="270" AutoScroll="true" AnchorHorizontal="90%" ClientIDMode="Static">
                        <RightButtons>
                            <ext:Button ID="btnDesigner" runat="server" Text="Use Visual Quary Designer" ValidationGroup="MainForm" Hidden="true">
                                <Listeners>
                                    <Click Handler="OpenQuery()"></Click>
                                </Listeners>
                            </ext:Button>
                        </RightButtons>
                    </ext:TextArea>
                    <ext:Hidden ID="hQueryObjectDesigner" runat="server" ClientIDMode="Static">
                    </ext:Hidden>
                    <ext:TextField ID="txtFieldUnik" runat="server" FieldLabel="Field Unik Name" AnchorHorizontal="80%" EmptyText="Please Fill Field Unik Name">
                    </ext:TextField>
                    <ext:TextField ID="txtOutputFormat" runat="server" FieldLabel="Output Format" AnchorHorizontal="80%" EmptyText="Please Fill Output Format" Hidden="true"/>
                    <ext:TextField ID="txtTableSource" runat="server" FieldLabel="Table Source" AnchorHorizontal="80%" EmptyText="Please Fill Table Source" Hidden="true"/>
                    <ext:ComboBox ID="cbTanggalData" runat="server" FieldLabel="Tanggal Data" QueryMode="Local" TriggerAction="All" AnchorHorizontal="70%" AllowBlank="false" EmptyText="Select Field" >
                        <Items>
                        </Items>
                        <DirectEvents>
                            <Expand OnEvent="LoadComboFieldReff">
                                <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                            </Expand>
                        </DirectEvents>
                    </ext:ComboBox>
                    <ext:ComboBox ID="cbKodeCabang" runat="server" FieldLabel="Kode Cabang" QueryMode="Local" TriggerAction="All" AnchorHorizontal="70%" AllowBlank="false" EmptyText="Select Field" >
                        <Items>
                        </Items>
                        <DirectEvents>
                            <Expand OnEvent="LoadComboFieldReff">
                                <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                            </Expand>
                        </DirectEvents>
                    </ext:ComboBox>
                    <ext:ComboBox ID="cbRecordId" runat="server" FieldLabel="Record Id" QueryMode="Local" TriggerAction="All" AnchorHorizontal="70%" AllowBlank="true" EmptyText="Select Field" >
                        <Items>
                        </Items>
                        <DirectEvents>
                            <Expand OnEvent="LoadComboFieldReff">
                                <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                            </Expand>
                        </DirectEvents>
                    </ext:ComboBox>
                </Items>
                <Buttons>
                    <ext:Button ID="btnsaveAdditional" runat="server" Icon="Disk" Text="Save Table Reference">
                        <Listeners>
                            <Click Handler="if (!#{FormPanelAdditional}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="btnsaveAdditional_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btncancelAddtional" runat="server" Icon="Cancel" Text="Cancel Table Reference">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelAdditional_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.8});" />

            <Resize Handler="#{WindowDetail}.center()" />
        </Listeners>

    </ext:Window>

    <ext:Window ID="WindowDetail" runat="server" Icon="ApplicationViewDetail" Title="Replacer" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormSchedulerDetail" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>
                    <ext:TextField ID="txtRepalcer" runat="server" FieldLabel="Replacer" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please Fill Replacer using $ReplacerItem$">
                    </ext:TextField>

                    <ext:ComboBox ID="cboField" runat="server" FieldLabel="Primary and Addtional Field" QueryMode="Local" TriggerAction="All" AnchorHorizontal="70%" AllowBlank="false" EmptyText="Select Field" >
                        <Items>

                        </Items>
                    </ext:ComboBox>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSaveReplacer" runat="server" Icon="Disk" Text="Save Replacer">
                        <Listeners>
                            <Click Handler="if (!#{FormSchedulerDetail}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="btnSaveReplacer_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelReplacer" runat="server" Icon="Cancel" Text="Cancel Replacer">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelReplacer_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.35});" />

            <Resize Handler="#{WindowDetail}.center()" />
        </Listeners>

    </ext:Window>

    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true">
        <Items>
            <ext:TextField ID="txtGenerateFileTemplate" runat="server" FieldLabel="Template Name" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please Fill Template Name">
            </ext:TextField>
            <ext:TextField ID="txtFileNameFormat" runat="server" FieldLabel="File Name Format" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please Fill File Name Format">
            </ext:TextField>
            <ext:ComboBox ID="CbOJKSegmenData" runat="server" FieldLabel="Information Name" DisplayField="DisplayField" ValueField="DisplayValue" ForceSelection="True" 
                EmptyText="Please Select Information Data" AllowBlank="false" AnchorHorizontal="70%" QueryMode="Local" AnyMatch="true">
                <Store>
                    <ext:Store runat="server" ID="StoreSegmenData">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="DisplayField" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="DisplayValue" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:ComboBox ID="CbDelivMethod" runat="server" FieldLabel="Delivery Method" DisplayField="MethodType" ValueField="PK_ID" ForceSelection="True" 
                EmptyText="Please Select Delivery Method" AllowBlank="false" AnchorHorizontal="70%" QueryMode="Local" AnyMatch="true">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="MethodType" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
            </ext:ComboBox>
            <ext:FieldSet ID="FieldSet4" runat="server" Title="Table Reference" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:GridPanel ID="GridPanelAdditional" runat="server" Title="Table Reference">
                        <TopBar>
                            <ext:Toolbar ID="toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="btnAdditional" runat="server" Text="Add New Table Reference">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddAdditionalTable_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreAdditional" runat="server">
                                <Model>
                                    <ext:Model ID="Model1" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_GenerateFileTemplateAdditional_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_GenerationFileTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="SQLQuery" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="Alias" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="UnikField" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="Sequence" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="OutputFormat" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="TableSource" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldTahun" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldCabang" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldBulan" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:Column ID="Column2" runat="server" DataIndex="Alias" Text="Alias" Flex="1"/>
                                <ext:Column ID="Column1" runat="server" DataIndex="SQLQuery" Text="SQL Query" Flex="1"/>
                                <ext:Column ID="Column3" runat="server" DataIndex="UnikField" Text="Unique Field" Flex="1" Hidden="true"/>
                                <ext:Column ID="Column4" runat="server" DataIndex="Sequence" Text="Seq" Flex="1"/>
                                <ext:Column ID="Column5" runat="server" DataIndex="OutputFormat" Text="OutputFormat" Flex="1" Hidden="true"/>
                                <ext:Column ID="Column6" runat="server" DataIndex="TableSource" Text="Table Source" Flex="1" Hidden="true"/>
                                <ext:Column ID="Column7" runat="server" DataIndex="FieldTahun" Text="Field Tahun" Flex="1" Hidden="true"/>
                                <ext:Column ID="Column8" runat="server" DataIndex="FieldCabang" Text="Field Cabang" Flex="1" Hidden="true"/>
                                <ext:Column ID="Column9" runat="server" DataIndex="FieldBulan" Text="Field Bulan" Flex="1" Hidden="true"/>
                                <ext:CommandColumn ID="CommandColumn2" runat="server" Flex="1" Width="450">
                                    <Commands>
                                        <%--<ext:GridCommand CommandName="Download" Icon="DiskDownload"  ></ext:GridCommand>--%>
                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>
                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommandAdditional">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_GenerateFileTemplateAdditional_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>

                                        </Command>
                                    </DirectEvents>

                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>


            <ext:FieldSet ID="FieldSet2" runat="server" Title="Template Format" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:TextArea ID="txtBody" runat="server" FieldLabel="Template Body" Hidden="false" AllowBlank="true" EmptyText="Please Fill Template Body" Height="200" AutoScroll="true">
                    </ext:TextArea>
                    <ext:GridPanel ID="gridReplacer" runat="server" Title="Replacer">
                        <TopBar>
                            <ext:Toolbar ID="toolbar" runat="server">
                                <Items>
                                    <ext:Button ID="btnAddNew" runat="server" Text="Add new Replacer Detail">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddNew_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>

                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreEmailDetail" runat="server">
                                <Model>
                                    <ext:Model ID="ModelDetail" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_GenerateFileTemplateDetail_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_GenerationFileTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="Replacer" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldReplacer" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumberTaskDetail" runat="server" Text="No"></ext:RowNumbererColumn>

                                <ext:Column ID="ColReplacerID" runat="server" DataIndex="Replacer" Text="Replacer" Flex="1">
                                </ext:Column>
                                <ext:Column ID="colQueryReplacer" runat="server" DataIndex="FieldReplacer" Text="Field Replacer" Flex="1">
                                </ext:Column>


                                <ext:CommandColumn ID="CommandColumn1" runat="server" Flex="1" Width="450">
                                    <Commands>
                                        <%--<ext:GridCommand CommandName="Download" Icon="DiskDownload"  ></ext:GridCommand>--%>
                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>

                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommand">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_GenerateFileTemplateDetail_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>

                                        </Command>
                                    </DirectEvents>

                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Template">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
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
