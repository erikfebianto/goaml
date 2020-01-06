<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="ModuleUpload.aspx.vb" Inherits="NAWADATA_ModuleUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:FormPanel runat="server" ID="FormPanelInput" ClientIDMode="Static" ButtonAlign="Center" Title="Upload Module and Module Field" Width="2000" BodyPadding="10" AutoScroll="true">
        <Items>
            <ext:FileUploadField
                ID="fuData"
                ClientIDMode="Static"
                runat="server"
                AnchorHorizontal="100%"
                FieldLabel="Select Excel Template">
                <Validator Handler="if( value.split('.').pop().toLowerCase() === 'enc'){
                            return true;
                        }else{
                            return false;
                        }" />
            </ext:FileUploadField>
            <ext:Button ID="btnUpload" runat="server" Text="Upload File" OnDirectClick="btnUpload_Click">
                <LoadingState Text="Please Wait..." />
            </ext:Button>
            <ext:Label ID="Label3" runat="server" Html="<br />" />
            <ext:Container runat="server">
                <Items>
                    <ext:GridPanel ID="gridModule"
                        runat="server" Hidden="true"
                        Title="Module" Collapsible="true"
                        Scrollable="Both"
                        ColumnWidth="0.6">
                        <Store>
                            <ext:Store ID="storeModule" runat="server">
                                <Model>
                                    <ext:Model ID="modelModule" runat="server">
                                        <Fields>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="columnModelModule" runat="server">
                            <Columns>

                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:Container>
            <ext:Container runat="server">
                <Items>
                    <ext:GridPanel ID="gridModuleField"
                        runat="server" Hidden="true"
                        Title="Module Field" Collapsible="true"
                        Scrollable="Both"
                        ColumnWidth="0.6">
                        <Store>
                            <ext:Store ID="storeModuleField" runat="server">
                                <Model>
                                    <ext:Model ID="modelModuleField" runat="server">
                                        <Fields>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="columnModelModuleField" runat="server">
                            <Columns>

                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:Container>
        </Items>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button
                        ID="btnDownloadData"
                        runat="server"
                        Text="Download With Data"
                        Icon="DiskDownload"
                        AutoPostBack="true" OnClick="btnDownloadData_Click">
                    </ext:Button>
                    <ext:Button
                        ID="btnDownloadTemplate"
                        runat="server"
                        Text="Download Template Only"
                        Icon="DiskDownload"
                        AutoPostBack="true" OnClick="btnDownloadTemplate_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Buttons>
            <ext:Button runat="server" Text="Save" ID="btnSubmit">
                <DirectEvents>
                    <Click OnEvent="btnSubmit_Click">
                        <EventMask ShowMask="true" Msg="Download Data..." MinDelay="500" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="btnCancel_Click">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500" />
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

