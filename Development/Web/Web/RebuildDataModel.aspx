<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="RebuildDataModel.aspx.vb" Inherits="RebuildDataModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:FormPanel runat="server" ID="FormPanelInput" ClientIDMode="Static" ButtonAlign="Center" BodyPadding="15" AutoScroll="true" Layout="AnchorLayout" AnchorHorizontal="100%">
        <TopBar>
            <ext:Toolbar runat="server" StyleSpec="border-bottom: 1px solid #e7e7e7 !important; padding: 7px 10px !important;">
                <Items>
                    <ext:Button ID="BtnDownloadData" runat="server" Text="Download Data Model" Icon="DiskDownload" StyleSpec="cursor: pointer;">
                        <DirectEvents>
                            <Click OnEvent="BtnDownloadData_OnClicked"></Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:FileUploadField ID="BtnAttachFile" runat="server" ClientIDMode="Static" LabelWidth="90" AnchorHorizontal="60%" 
                FieldLabel="Excel File" MarginSpec="0 20 15 0" AllowBlank="false" StyleSpec="float:left;">
                <Validator Handler=" const type = value.split('.').pop().toLowerCase(); if(type !== '') { return (type === 'xlsx' || type === 'xls'); } else return true; " />
            </ext:FileUploadField>
            <ext:Button ID="BtnUpload" runat="server" Text="Upload File" MarginSpec="0 0 15" Icon="DiskUpload" StyleSpec="cursor: pointer; float:left;">
                <DirectEvents>
                    <Click OnEvent="BtnUpload_OnClicked">
                        <EventMask Msg="Uploading..." MinDelay="500" ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:GridPanel ID="GridModule" runat="server" Title="Module" Collapsible="true" CollapseToolText="Collapse" ExpandToolText="Expand" AutoScroll="true" 
                EmptyText="No Data Available" MarginSpec="0 0 10px" MaxHeight="200">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_Module_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ModuleLabel" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ModuleDescription" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="IsUseDesigner" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsUseApproval" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportAdd" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportEdit" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportDelete" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportActivation" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportView" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportUpload" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportDetail" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="UrlAdd" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlEdit" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlDelete" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlActivation" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlView" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlUpload" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlApproval" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlApprovalDetail" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UrlDetail" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="IsUseStoreProcedureValidation" Type="Boolean"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column runat="server" Text="ID" DataIndex="PK_Module_ID" Width="50"></ext:Column>
                        <ext:Column runat="server" Text="Module Name" DataIndex="ModuleName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Module Label" DataIndex="ModuleLabel" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Module Description" DataIndex="ModuleDescription"></ext:Column>
                        <ext:Column runat="server" Text="Use Designer" DataIndex="IsUseDesigner" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Use Approval" DataIndex="IsUseApproval" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Support Add" DataIndex="IsSupportAdd" Width="100"></ext:Column>
                        <ext:Column runat="server" Text="Support Edit" DataIndex="IsSupportEdit" Width="100"></ext:Column>
                        <ext:Column runat="server" Text="Support Delete" DataIndex="IsSupportDelete" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Support Activation" DataIndex="IsSupportActivation" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Support View" DataIndex="IsSupportView" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Support Upload" DataIndex="IsSupportUpload" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Support Detail" DataIndex="IsSupportDetail" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="URL Add" DataIndex="UrlAdd" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Edit" DataIndex="UrlEdit" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Delete" DataIndex="UrlDelete" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Activation" DataIndex="UrlActivation" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL View" DataIndex="UrlView" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Upload" DataIndex="UrlUpload" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Approval" DataIndex="UrlApproval" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Approval Detail" DataIndex="UrlApprovalDetail" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="URL Detail" DataIndex="UrlDetail" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Use Stored Procedure Validation" DataIndex="IsUseStoreProcedureValidation"></ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
            <ext:GridPanel ID="GridModuleField" runat="server" Title="Module Field" Collapsible="true" CollapseToolText="Collapse" ExpandToolText="Expand" AutoScroll="true" 
                EmptyText="No Data Available" MarginSpec="0 0 10px" MaxHeight="200">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_ModuleField_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FieldName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FieldLabel" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Sequence" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="Required" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsPrimaryKey" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsUnik" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsShowInView" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsShowInForm" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="DefaultValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportUpload" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="IsSupportDetail" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="FK_FieldType_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="FieldTypeCaption" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="SizeField" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="FK_ExtType_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ExtTypeName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TabelReferenceName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TabelReferenceNameAlias" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TableReferenceFieldKey" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TableReferenceFieldDisplayName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="TableReferenceFilter" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="IsUseRegexValidation" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="TableReferenceAdditonalJoin" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="BCasCade" Type="Boolean"></ext:ModelField>
                                    <ext:ModelField Name="FieldNameParent" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FilterCascade" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column runat="server" Text="Module Name" DataIndex="ModuleName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Field Name" DataIndex="FieldName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Field Label" DataIndex="FieldLabel" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Sequence" DataIndex="Sequence" Width="90"></ext:Column>
                        <ext:Column runat="server" Text="Required" DataIndex="Required" Width="90"></ext:Column>
                        <ext:Column runat="server" Text="Primary Key" DataIndex="IsPrimaryKey" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Unik" DataIndex="IsUnik" Width="50"></ext:Column>
                        <ext:Column runat="server" Text="Show in View" DataIndex="IsShowInView" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Show in Form" DataIndex="IsShowInForm" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Default Value" DataIndex="DefaultValue" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Field Type" DataIndex="FieldTypeCaption" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Size Field" DataIndex="SizeField" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Ext Type" DataIndex="ExtTypeName" Width="105"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Name" DataIndex="TabelReferenceName" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Alias" DataIndex="TabelReferenceNameAlias" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Key" DataIndex="TableReferenceFieldKey" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Display Name" DataIndex="TableReferenceFieldDisplayName" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Filter" DataIndex="TableReferenceFilter" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Table Reference Additional Join" DataIndex="TableReferenceAdditonalJoin" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Use Regex" DataIndex="IsUseRegexValidation" Width="90"></ext:Column>
                        <ext:Column runat="server" Text="Cascade" DataIndex="BCasCade" Width="80"></ext:Column>
                        <ext:Column runat="server" Text="Field Name Parent" DataIndex="FieldNameParent"></ext:Column>
                        <ext:Column runat="server" Text="Filter Cascade" DataIndex="FilterCascade"></ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
            <ext:GridPanel ID="GridModuleValidation" runat="server" Title="Module Validation" Collapsible="true" CollapseToolText="Collapse" ExpandToolText="Expand" AutoScroll="true" 
                EmptyText="No Data Available" MarginSpec="0 0 10px" MaxHeight="200">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_ModuleValidation_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="FK_Module_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FK_ModuleAction_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ModuleActionName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FK_ModuleTime_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ModuleTimeName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="StoreProcedureName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="StoreProcedureParameter" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="StoreProcedureParameterValueFieldSequence" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column runat="server" Text="Module Name" DataIndex="ModuleName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Action" DataIndex="ModuleActionName" Width="120"></ext:Column>
                        <ext:Column runat="server" Text="Time" DataIndex="ModuleTimeName" Width="150"></ext:Column>
                        <ext:Column runat="server" Text="Stored Procedure Name" DataIndex="StoreProcedureName" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Stored Procedure Parameter" DataIndex="StoreProcedureParameter" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Parameter Field Sequence" DataIndex="StoreProcedureParameterValueFieldSequence" Width="200"></ext:Column>                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
            <ext:GridPanel ID="GridValidationParam" runat="server" Title="Validation Parameter" Collapsible="true" CollapseToolText="Collapse" ExpandToolText="Expand" AutoScroll="true" 
                EmptyText="No Data Available" MarginSpec="0 0 10px" MaxHeight="200">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_ValidationParameter" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="TableName" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FieldName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ExpressionType" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ValidationExpression" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Description" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ValidationMessage" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FK_ValidationType_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ValidationType" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ErrorType" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Fk_Ref_DataSet_Id" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="Ref_DataSetName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Fk_Ref_KategoriValidasi_Id" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="Ref_KategoriValidasiName" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column runat="server" Text="Module Name" DataIndex="ModuleName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Field Name" DataIndex="FieldName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Expression Type" DataIndex="ExpressionType" Width="130"></ext:Column>
                        <ext:Column runat="server" Text="Validation Expression" DataIndex="ValidationExpression" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Validation Message" DataIndex="ValidationMessage" Width="200"></ext:Column>
                        <ext:Column runat="server" Text="Validation Type" DataIndex="ValidationType" Width="120"></ext:Column>
                        <ext:Column runat="server" Text="Data Set" DataIndex="Ref_DataSetName" Width="120"></ext:Column>
                        <ext:Column runat="server" Text="Kategori Validasi" DataIndex="Ref_KategoriValidasiName" Width="150"></ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
            <ext:GridPanel ID="GridFieldRegex" runat="server" Title="Module Field Regex" Collapsible="true" CollapseToolText="Collapse" ExpandToolText="Expand" AutoScroll="true" 
                EmptyText="No Data Available" MarginSpec="0 0 10px" MaxHeight="200">
                <Store>
                    <ext:Store runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_ModuleFieldRegex" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="FK_Module_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="ModuleName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FK_ModuleField_ID" Type="Int"></ext:ModelField>
                                    <ext:ModelField Name="FieldName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Regex" Type="String"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column runat="server" Text="Module Name" DataIndex="ModuleName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Field Name" DataIndex="FieldName" Width="180"></ext:Column>
                        <ext:Column runat="server" Text="Regex" DataIndex="Regex" Width="200"></ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
        </Items>
        <Buttons>
            <ext:Button ID="BtnRebuild" runat="server" Text="Rebuild" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="BtnRebuild_OnClicked">
                        <EventMask ShowMask="true" Msg="Rebuilding Data Model..." MinDelay="500" />
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" 
        DefaultAnchor="100%" Hidden="true">
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

