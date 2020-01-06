<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" Inherits="ModuleAdd, App_Web_moduleadd.aspx.252c98" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Window ID="WindowDetail" runat="server" Icon="ApplicationViewDetail" Title="Module field" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelField" runat="server" ButtonAlign="Center" Padding="5" AutoScroll="true" Layout="AnchorLayout">
                <Items>
                    <ext:TextField ID="TxtFieldName" runat="server" AnchorHorizontal="85%" FieldLabel="Field Name" AllowBlank="false" BlankText="Field Name is Required" MaxLength="250">
                    </ext:TextField>
                    <ext:TextField ID="TxtFieldLabel" runat="server" AnchorHorizontal="85%" FieldLabel="Field Label" AllowBlank="false" BlankText="Field Label is Required" MaxLength="250">
                    </ext:TextField>
                    <ext:NumberField ID="txtseq" runat="server" FieldLabel="Sequence" AllowBlank="false" BlankText="Sequence is Required">
                    </ext:NumberField>
                    <ext:Checkbox ID="chkIsPrimaryKey" runat="server" FieldLabel="Primary Key">
                        <DirectEvents>
                            <Change OnEvent="chkIsPrimaryKey_DirectClick"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkFieldRequired" runat="server" FieldLabel="Required">
                        <DirectEvents>
                            <Change OnEvent="chkFieldRequired_DirectClick"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkIsUnik" runat="server" FieldLabel="Unik">
                        <DirectEvents>
                            <Change OnEvent="chkIsUnik_DirectClick"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkShowInView" runat="server" FieldLabel="Show In View">
                        <DirectEvents>
                            <Change OnEvent="chkShowInView_DirectClick"></Change>
                        </DirectEvents>
                    </ext:Checkbox>

                    <ext:Checkbox ID="chkShowInForm" runat="server" FieldLabel="Show In Form">
                        <DirectEvents>
                            <Change OnEvent="chkShowInForm_DirectClick"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:TextArea ID="TxtDefaultValue" runat="server" FieldLabel="Default Value" Height="100" AnchorHorizontal="90%">
                    </ext:TextArea>
                    <ext:ComboBox ID="cboExtType" runat="server" FieldLabel="Ext Type" DisplayField="ExtTypeName" ValueField="PK_ExtType_ID" AnchorHorizontal="80%" MinChars="0" ForceSelection="true" TriggerAction="Query" AllowBlank="false" BlankText="Ext Type is required">
                        <Store>
                            <ext:Store ID="StoreextType" runat="server" OnReadData="StoreextType_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelExtType" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_ExtType_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="ExtTypeName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                    <ext:PageProxy>
                                    </ext:PageProxy>
                                </Proxy>
                            </ext:Store>
                        </Store>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:ComboBox ID="cboFieldType" runat="server" FieldLabel="Field Type" AnchorHorizontal="80%" DisplayField="FieldTypeCaption" ValueField="PK_FieldType_ID" MinChars="0" ForceSelection="true" TriggerAction="Query" AllowBlank="false" BlankText="Field Type is required">
                        <Store>
                            <ext:Store ID="StoreFieldType" runat="server" OnReadData="StoreFieldType_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelFieldType" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_FieldType_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="FieldTypeCaption" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                    <ext:PageProxy>
                                    </ext:PageProxy>
                                </Proxy>
                            </ext:Store>
                        </Store>
                        <DirectEvents>
                            <Change OnEvent="cboFieldType_DirectSelect"></Change>
                        </DirectEvents>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:NumberField ID="txtFieldSize" runat="server" FieldLabel="Field Size" Hidden="true">
                    </ext:NumberField>

                    <ext:FieldSet ID="fieldsetref" runat="server" Hidden="true">
                        <Items>

                            <ext:ComboBox ID="cboTableReference" ClientIDMode="Static" runat="server" FieldLabel="Table Reference" AnchorHorizontal="80%" DisplayField="TABLE_NAME" ValueField="TABLE_NAME" MinChars="0" EmptyText="[Select Data]" ForceSelection="true" TriggerAction="Query">
                                <Store>
                                    <ext:Store ID="StoreTableReference" runat="server" OnReadData="StoreTableReference_ReadData" IsPagingStore="true">
                                        <Model>
                                            <ext:Model ID="ModelTableReference" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="TABLE_NAME" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy>
                                            </ext:PageProxy>
                                        </Proxy>
                                    </ext:Store>
                                </Store>

                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                </Triggers>
                                <Listeners>
                                    <Select Handler="this.getTrigger(0).show();#{CboTableReferencKey}.clearValue();#{CboTableReferenceDisplayName}.clearValue();#{StoreTableReferenceKey}.reload();#{StoreTableReferenceDisplayName}.reload();" />
                                    <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:TextField ID="txtTableAlias" runat="server" FieldLabel="Table Reference Alias" AnchorHorizontal="80%" ClientIDMode="Static">
                            </ext:TextField>

                            <ext:ComboBox ID="CboTableReferencKey" runat="server" FieldLabel="Table Reference Key" AnchorHorizontal="80%" DisplayField="COLUMN_NAME" ValueField="COLUMN_NAME" MinChars="0" EmptyText="[Select Data]" ForceSelection="true" TriggerAction="Query">
                                <Store>
                                    <ext:Store ID="StoreTableReferenceKey" runat="server" OnReadData="StoreTableReferenceKey_ReadData" IsPagingStore="true">
                                        <Model>
                                            <ext:Model ID="ModelTableReferenceKey" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="COLUMN_NAME" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy>
                                            </ext:PageProxy>
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                </Triggers>
                                <Listeners>
                                    <Select Handler="this.getTrigger(0).show();" />
                                    <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                                </Listeners>
                            </ext:ComboBox>

                            <ext:ComboBox ID="CboTableReferenceDisplayName" runat="server" FieldLabel="Table Reference Display Name" AnchorHorizontal="80%" DisplayField="COLUMN_NAME" ValueField="COLUMN_NAME" MinChars="0" EmptyText="[Select Data]" ForceSelection="true" TriggerAction="Query">
                                <Store>
                                    <ext:Store ID="StoreTableReferenceDisplayName" runat="server" OnReadData="StoreTableReferenceDisplayName_ReadData" IsPagingStore="true">
                                        <Model>
                                            <ext:Model ID="ModelTableReferenceDisplayName" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="COLUMN_NAME" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Proxy>
                                            <ext:PageProxy>
                                            </ext:PageProxy>
                                        </Proxy>
                                    </ext:Store>
                                </Store>
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                </Triggers>
                                <Listeners>
                                    <Select Handler="this.getTrigger(0).show();" />
                                    <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                                </Listeners>
                            </ext:ComboBox>

                            <ext:TextField ID="txtTableReferenceAdditionalJoin" runat="server" FieldLabel="Table Reference Additional Join" AnchorHorizontal="85%">
                            </ext:TextField>

                            <%--<ext:TextField ID="txtTabelReference" runat="server" FieldLabel="Table Reference" AnchorHorizontal="85%">
                                </ext:TextField>
                                <ext:TextField ID="txtTableReferenceKey" runat="server" FieldLabel="Table Reference Key" AnchorHorizontal="85%">
                                </ext:TextField>
                                <ext:TextField ID="txtTableReferenceDisplayName" runat="server" FieldLabel="Table Reference Display Name" AnchorHorizontal="85%">
                                </ext:TextField>--%>
                            <ext:TextField ID="txtTableReferenceFilter" runat="server" FieldLabel="Table Reference Filter" AnchorHorizontal="85%">
                            </ext:TextField>
                            <ext:Checkbox ID="chkCascade" runat="server" FieldLabel="CasCade" BoxLabel="CaseCade">
                                <DirectEvents>
                                    <Change OnEvent="chkCascade_DirectEvent"></Change>
                                </DirectEvents>
                            </ext:Checkbox>
                            <ext:FieldSet ID="FieldSetCascade" runat="server" Hidden="true">
                                <Items>
                                    <ext:TextField ID="txtFieldNameParent" runat="server" FieldLabel="FieldName Parent" AnchorHorizontal="80%">
                                    </ext:TextField>
                                    <ext:TextField ID="txtFilterCaseCade" runat="server" FieldLabel="FilterCaseCade" EmptyText="Fk_fgnkey_id=@Parent" AnchorHorizontal="80%">
                                    </ext:TextField>
                                </Items>
                            </ext:FieldSet>
                        </Items>
                    </ext:FieldSet>
                    <ext:Checkbox ID="chkIsUseRegex" runat="server" FieldLabel="Is Use Regex">
                        <DirectEvents>
                            <Change OnEvent="chkIsUseRegex_DirectCheck"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:FieldSet ID="FieldsetRegex" runat="server" Title="Regex Data" Hidden="true">
                        <Items>

                            <ext:FormPanel ID="FormPanelRegex" runat="server" ButtonAlign="Center" Padding="10" Title="Regex">
                                <Items>
                                    <ext:TextField ID="txtRegex" runat="server" FieldLabel="Regex">
                                    </ext:TextField>
                                </Items>
                                <Buttons>
                                    <ext:Button ID="btnSaveRegex" runat="server" Icon="Disk" Text="Save Regex">
                                        <DirectEvents>
                                            <Click OnEvent="btnSaveRegex_DirectClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                        <Listeners>
                                            <Click Handler="if (!#{FormPanelRegex}.getForm().isValid()) return false;"></Click>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="btnCancelRegex" runat="server" Icon="Cancel" Text="Cancel">
                                    </ext:Button>
                                </Buttons>
                            </ext:FormPanel>
                            <ext:GridPanel ID="GridPanelRegex" Padding="10" runat="server" Title="Regex List">
                                <Store>
                                    <ext:Store ID="StoreRegex" runat="server">
                                        <Model>
                                            <ext:Model runat="server" ID="ModelRegex" IDProperty="PK_ModuleFieldRegex">
                                                <Fields>
                                                    <ext:ModelField Name="PK_ModuleFieldRegex" Type="Auto"></ext:ModelField>
                                                    <ext:ModelField Name="FK_ModuleField_ID" Type="Auto"></ext:ModelField>
                                                    <ext:ModelField Name="Regex" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:RowNumbererColumn ID="RowNumbererRegex" runat="server" Text="No"></ext:RowNumbererColumn>
                                        <ext:Column ID="PK_ModuleFieldRegex" runat="server" DataIndex="PK_ModuleFieldRegex" Text="ID"></ext:Column>
                                        <ext:Column ID="Regex" runat="server" DataIndex="Regex" Text="Regex" Flex="1"></ext:Column>
                                        <ext:CommandColumn ID="commandregex" runat="server">
                                            <DirectEvents>

                                                <Command OnEvent="CallBackRegex">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="unikkey" Value="record.data.PK_ModuleFieldRegex" Mode="Raw"></ext:Parameter>
                                                        <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                                    </ExtraParams>
                                                </Command>
                                            </DirectEvents>
                                            <Commands>

                                                <ext:GridCommand Text="Delete" CommandName="Delete" Icon="Delete">
                                                    <ToolTip Text="Delete"></ToolTip>
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:FieldSet>
                </Items>

                <Buttons>
                    <ext:Button ID="btnsaveField" runat="server" Icon="Disk" Text="Add Field">
                        <DirectEvents>
                            <Click OnEvent="BtnSaveField_Directclick">
                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                        <Listeners>
                            <Click Handler="if (!#{FormPanelField}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnCancelField" runat="server" Icon="Cancel" Text="Cancel">
                        <DirectEvents>
                            <Click OnEvent="btnCancelField_Directclick">
                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.8, height: size.height * 1});" />

            <Resize Handler="#{WindowDetail}.center()" />
        </Listeners>
    </ext:Window>

    <ext:Window ID="Windowsp" runat="server" Icon="ApplicationViewDetail" Title="Store Procedure Validation" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelStoreProcedure" runat="server" ButtonAlign="Center" Padding="5" AutoScroll="true" Layout="AnchorLayout">
                <Items>

                    <ext:ComboBox ID="CboAction" runat="server" FieldLabel="Action" MinChars="0" AnchorHorizontal="80%" DisplayField="ModuleActionName" ValueField="PK_ModuleAction_ID" ForceSelection="true" TriggerAction="Query" EmptyText="[Select Data]" AllowBlank="false">
                        <Store>
                            <ext:Store ID="StoreAction" runat="server" OnReadData="StoreAction_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelAction" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_ModuleAction_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="ModuleActionName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                    <ext:PageProxy>
                                    </ext:PageProxy>
                                </Proxy>
                            </ext:Store>
                        </Store>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:ComboBox ID="CboModuleTime" runat="server" FieldLabel="Time" MinChars="0" AnchorHorizontal="80%" DisplayField="ModuleTimeName" ValueField="PK_ModuleTime_ID" ForceSelection="true" TriggerAction="Query" EmptyText="[Select Data]" AllowBlank="false">
                        <Store>
                            <ext:Store ID="StoreModuleTime" runat="server" OnReadData="StoreModuleTime_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelModuleTime" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_ModuleTime_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="ModuleTimeName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                    <ext:PageProxy>
                                    </ext:PageProxy>
                                </Proxy>
                            </ext:Store>
                        </Store>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:TextField ID="TxtStoreProcedureName" runat="server" FieldLabel="Store Procedure Name" BlankText="Store Procedure Name is Required" AllowBlank="false" AnchorHorizontal="85%">
                    </ext:TextField>
                    <ext:TextField ID="TxtStoreProcedureParameter" runat="server" FieldLabel="Store Procedure Parameter" AnchorHorizontal="85%">
                    </ext:TextField>
                    <ext:TextField ID="TxtStoreProcedureValueSeq" runat="server" FieldLabel="Store Procedure  Field Value Seq" AnchorHorizontal="85%">
                    </ext:TextField>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSaveProcedure" runat="server" Icon="Disk" Text="Add Store Procedure">
                        <DirectEvents>
                            <Click OnEvent="btnSaveProcedure_DirectClick">
                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                            </Click>
                        </DirectEvents>

                        <Listeners>
                            <Click Handler="if (!#{FormPanelStoreProcedure}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnCancelProcedure" runat="server" Icon="Cancel" Text="Cancel">
                        <DirectEvents>
                            <Click OnEvent="btnCancelProcedure_DirectClick">
                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.8});" />

            <Resize Handler="#{Windowsp}.center()" />
        </Listeners>
    </ext:Window>
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="Module Designer" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:DisplayField ID="IDModule" runat="server" Text="Auto Number" FieldLabel="Module ID">
            </ext:DisplayField>
            <ext:TextField ID="TxtModuleName" runat="server" Text="" FieldLabel="Module Name" AnchorHorizontal="85%" AllowBlank="false" BlankText="Module Name is Required" MaxLength="250" ValidationGroup="MainForm">
            </ext:TextField>
            <ext:TextField ID="TxtModuleLabel" runat="server" Text="" FieldLabel="Module Label" AnchorHorizontal="85%" AllowBlank="false" BlankText="Module Label is Required" MaxLength="250" ValidationGroup="MainForm">
            </ext:TextField>
            <ext:TextArea ID="TxtDescription" runat="server" Text="" FieldLabel="Module Description" AnchorHorizontal="85%">
            </ext:TextArea>

            <ext:FieldContainer ID="FieldContainerSupportAction" runat="server" Layout="FitLayout" FieldLabel="Supported Action">
                <Items>

                    <ext:Checkbox ID="chkSupportAdd" runat="server" BoxLabel="Support Add">
                        <DirectEvents>
                            <Change OnEvent="chkSupportAdd_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chksupportEdit" runat="server" BoxLabel="Support Edit">
                        <DirectEvents>
                            <Change OnEvent="chksupportEdit_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chksupportDelete" runat="server" BoxLabel="Support Delete">
                        <DirectEvents>
                            <Change OnEvent="chksupportDelete_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkSupportView" runat="server" BoxLabel="Support View">
                        <DirectEvents>
                            <Change OnEvent="chkSupportView_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>

                    <ext:Checkbox ID="chkSupportUpload" runat="server" BoxLabel="Support Upload">
                        <DirectEvents>
                            <Change OnEvent="chkSupportupload_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>

                    <ext:Checkbox ID="chksupportActivation" runat="server" BoxLabel="Support Activation">
                        <DirectEvents>
                            <Change OnEvent="chksupportActivation_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkSupportApproval" runat="server" BoxLabel="Support Approval">
                        <DirectEvents>
                            <Change OnEvent="chkSupportApproval_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                    <ext:Checkbox ID="chkSupportDetail" runat="server" BoxLabel="Support Detail">
                        <DirectEvents>
                            <Change OnEvent="chkSupportDetail_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>
                </Items>
            </ext:FieldContainer>

            <ext:FieldContainer ID="FieldContainerSupportDesigner" runat="server" Layout="FormLayout" FieldLabel="Support Designer" Width="800">
                <Items>
                    <ext:Checkbox ID="chkSupportDesigner" runat="server" BoxLabel="Support Designer">
                        <DirectEvents>
                            <Change OnEvent="chkSupportDesigner_CheckedChanged"></Change>
                        </DirectEvents>
                    </ext:Checkbox>

                    <ext:TextField ID="TxtUrlAdd" runat="server" FieldLabel="URL Add" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlEdit" runat="server" FieldLabel="URL Edit" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlDelete" runat="server" FieldLabel="URL Delete" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlView" runat="server" FieldLabel="URL View" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlActivation" runat="server" FieldLabel="URL Activation" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlUpload" runat="server" FieldLabel="URL Upload" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlApproval" runat="server" FieldLabel="URL Approval" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlApprovalDetail" runat="server" FieldLabel="URL Approval Detail" Text="" Hidden="true">
                    </ext:TextField>
                    <ext:TextField ID="TxtUrlDetail" runat="server" FieldLabel="URL Detail" Text="" Hidden="true">
                    </ext:TextField>
                </Items>
            </ext:FieldContainer>

            <ext:FieldSet ID="FieldContainerModuleField" runat="server" Title="ModuleField" Hidden="true" Height="300" Layout="FitLayout">
                <Items>

                    <ext:GridPanel ID="GridPanelModuleField" Padding="5" runat="server" Title="Module Field List" AutoScroll="true">
                        <TopBar>
                            <ext:Toolbar ID="toolbar" runat="server">
                                <Items>
                                    <ext:Button ID="btnAddNew" runat="server" Text="Add new Detail">
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
                            <ext:Store ID="StoreModuleField" runat="server">
                                <Model>
                                    <ext:Model ID="ModelModuleField" runat="server" IDProperty="PK_ModuleField_ID">
                                        <Fields>
                                            <ext:ModelField Name="PK_ModuleField_ID" Type="Auto" />
                                            <ext:ModelField Name="FK_Module_ID" Type="int" />
                                            <ext:ModelField Name="FieldName" Type="String" />
                                            <ext:ModelField Name="FieldLabel" Type="String" />
                                            <ext:ModelField Name="Sequence" Type="Int" />
                                            <ext:ModelField Name="Required" Type="Boolean" />
                                            <ext:ModelField Name="IsPrimaryKey" Type="Boolean" />
                                            <ext:ModelField Name="IsUnik" Type="Boolean" />
                                            <ext:ModelField Name="IsShowInView" Type="Boolean" />
                                            <ext:ModelField Name="IsShowInForm" Type="Boolean" />
                                            <ext:ModelField Name="DefaultValue" Type="String" />
                                            <ext:ModelField Name="FK_FieldType_ID" Type="Int" />
                                            <ext:ModelField Name="MFieldType" ServerMapping="MFieldType.FieldTypeCaption" />
                                            <ext:ModelField Name="SizeField" Type="Int" />
                                            <ext:ModelField Name="FK_ExtType_ID" Type="Int" />
                                            <ext:ModelField Name="MExtType" ServerMapping="MExtType.ExtTypeName" />
                                            <ext:ModelField Name="TabelReferenceName" Type="String" />
                                            <ext:ModelField Name="TabelReferenceNameAlias" Type="String" />
                                            <ext:ModelField Name="TableReferenceFieldKey" Type="String" />
                                            <ext:ModelField Name="TableReferenceFieldDisplayName" Type="String" />
                                            <ext:ModelField Name="TableReferenceAdditonalJoin" Type="String" />
                                            <ext:ModelField Name="TableReferenceFilter" Type="String" />
                                            <ext:ModelField Name="BCasCade" Type="Boolean" />
                                            <ext:ModelField Name="FieldNameParent" Type="String" />
                                            <ext:ModelField Name="FilterCascade" Type="String" />

                                            <ext:ModelField Name="IsUseRegexValidation" Type="Boolean" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="rownumModuleField" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:CommandColumn ID="frmModuleField" runat="server" Width="120">

                                    <DirectEvents>

                                        <Command OnEvent="CallBackModuleField">
                                            <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true" Message="Are You sure to Delete This Record" Title="Delete"></Confirmation>
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_ModuleField_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                        </Command>
                                    </DirectEvents>
                                    <Commands>

                                        <ext:GridCommand Text="Edit" CommandName="Edit" Icon="NoteEdit" MinWidth="50">
                                            <ToolTip Text="Edit"></ToolTip>
                                        </ext:GridCommand>
                                        <ext:GridCommand Text="Delete" CommandName="Delete" Icon="Delete" MinWidth="70">
                                            <ToolTip Text="Delete"></ToolTip>
                                        </ext:GridCommand>
                                    </Commands>
                                </ext:CommandColumn>
                                <ext:Column ID="PK_ModuleField_ID" runat="server" DataIndex="PK_ModuleField_ID" Text="ID"></ext:Column>
                                <ext:Column ID="FieldName" runat="server" DataIndex="FieldName" Text="Field Name"></ext:Column>
                                <ext:Column ID="FieldLabel" runat="server" DataIndex="FieldLabel" Text="Field Label"></ext:Column>
                                <ext:Column ID="Sequence" runat="server" DataIndex="Sequence" Text="Sequence"></ext:Column>
                                <ext:Column ID="Required" runat="server" DataIndex="Required" Text="Required"></ext:Column>
                                <ext:Column ID="IsPrimaryKey" runat="server" DataIndex="IsPrimaryKey" Text="IsPrimaryKey"></ext:Column>
                                <ext:Column ID="IsUnik" runat="server" DataIndex="IsUnik" Text="IsUnik"></ext:Column>
                                <ext:Column ID="IsShowInView" runat="server" DataIndex="IsShowInView" Text="IsShowInView"></ext:Column>
                                <ext:Column ID="Column1" runat="server" DataIndex="IsShowInForm" Text="IsShowInForm"></ext:Column>
                                <ext:Column ID="Column2" runat="server" DataIndex="DefaultValue" Text="DefaultValue"></ext:Column>

                                <ext:Column ID="MFieldType" runat="server" DataIndex="MFieldType" Text="Field Type">
                                </ext:Column>
                                <ext:Column ID="MExtType" runat="server" DataIndex="MExtType" Text="Ext Type">
                                </ext:Column>
                                <ext:Column ID="TabelReferenceName" runat="server" DataIndex="TabelReferenceName" Text="Table Reference Name"></ext:Column>
                                <ext:Column ID="TabelReferenceNameAlias" runat="server" DataIndex="TabelReferenceNameAlias" Text="Table Reference Name Alias"></ext:Column>

                                <ext:Column ID="TableReferenceFieldKey" runat="server" DataIndex="TableReferenceFieldKey" Text="Table Reference Key"></ext:Column>
                                <ext:Column ID="TableReferenceFieldDisplayName" runat="server" DataIndex="TableReferenceFieldDisplayName" Text="Table Reference Field Display Name"></ext:Column>
                                <ext:Column ID="TableReferenceAdditonalJoin" runat="server" DataIndex="TableReferenceAdditonalJoin" Text="Table Reference Additional Join"></ext:Column>
                                <ext:Column ID="TableReferenceFilter" runat="server" DataIndex="TableReferenceFilter" Text="Table Reference Filter"></ext:Column>
                                <ext:Column ID="Column3" runat="server" DataIndex="BCasCade" Text="Cascade"></ext:Column>
                                <ext:Column ID="Column4" runat="server" DataIndex="FieldNameParent" Text="FieldName Parent"></ext:Column>
                                <ext:Column ID="Column5" runat="server" DataIndex="FilterCascade" Text="Filter CasCade"></ext:Column>

                                <ext:Column ID="IsUseRegexValidation" runat="server" DataIndex="IsUseRegexValidation" Text="Is Use Regex Validation"></ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>

            <ext:Checkbox ID="chkUseStoreProcedure" runat="server" FieldLabel="Use Store Procedure Validation" Hidden="true">
                <DirectEvents>
                    <Change OnEvent="chkUseStoreProcedure_DirectCheck"></Change>
                </DirectEvents>
            </ext:Checkbox>
            <ext:FieldSet ID="FieldSetUseStoreProcedure" runat="server" Title="Store Procedure Validation" Hidden="true">

                <Items>

                    <ext:GridPanel ID="GridPanelStoreProcedure" runat="server" Title="Store Procedure List">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                                <Items>
                                    <ext:Button ID="btnaddspnew" runat="server" Text="Add new Detail">
                                        <DirectEvents>
                                            <Click OnEvent="btnaddspnew_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreStoreProcedure" runat="server">
                                <Model>
                                    <ext:Model ID="ModelStoreProcedure" runat="server" IDProperty="PK_ModuleValidation_ID">
                                        <Fields>

                                            <ext:ModelField Name="PK_ModuleValidation_ID" Type="Auto" />
                                            <ext:ModelField Name="FK_ModuleAction_ID" Type="Int" />
                                            <ext:ModelField Name="ModuleAction" ServerMapping="ModuleAction.ModuleActionName" />
                                            <ext:ModelField Name="ModuleTime" ServerMapping="ModuleTime.ModuleTimeName" />
                                            <ext:ModelField Name="StoreProcedureName" Type="String" />
                                            <ext:ModelField Name="StoreProcedureParameter" Type="String" />
                                            <ext:ModelField Name="StoreProcedureParameterValueFieldSequence" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererStoreProcedure" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:Column ID="PK_ModuleValidation_ID" runat="server" DataIndex="PK_ModuleValidation_ID" Text="ID"></ext:Column>
                                <ext:Column ID="ModuleAction" runat="server" DataIndex="ModuleAction" Text="Action"></ext:Column>
                                <ext:Column ID="ModuleTime" runat="server" DataIndex="ModuleTime" Text="Time"></ext:Column>
                                <ext:Column ID="StoreProcedureName" runat="server" DataIndex="StoreProcedureName" Text="Store Procedure Name"></ext:Column>
                                <ext:Column ID="StoreProcedureParameter" runat="server" DataIndex="StoreProcedureParameter" Text="Store Procedure Parameter" Flex="1"></ext:Column>
                                <ext:Column ID="StoreProcedureParameterValueFieldSequence" runat="server" DataIndex="StoreProcedureParameterValueFieldSequence" Text="Store Procedure Parameter Value Field Sequence"></ext:Column>

                                <ext:CommandColumn ID="CommandColumn1" runat="server" Width="150">

                                    <DirectEvents>

                                        <Command OnEvent="CallBackStoreProcedure">
                                            <Confirmation BeforeConfirm="if (command=='Edit') return false;" ConfirmRequest="true" Message="Are You sure to Delete This Record" Title="Delete"></Confirmation>
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_ModuleValidation_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                        </Command>
                                    </DirectEvents>
                                    <Commands>

                                        <ext:GridCommand Text="Edit" CommandName="Edit" Icon="NoteEdit">
                                            <ToolTip Text="Edit"></ToolTip>
                                        </ext:GridCommand>
                                        <ext:GridCommand Text="Delete" CommandName="Delete" Icon="Delete">
                                            <ToolTip Text="Delete"></ToolTip>
                                        </ext:GridCommand>
                                    </Commands>
                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Module" ValidationGroup="MainForm">
                <%--         <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
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