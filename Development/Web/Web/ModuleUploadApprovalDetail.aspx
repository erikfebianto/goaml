<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="ModuleUploadApprovalDetail.aspx.vb" Inherits="ModuleUploadApprovalDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:FormPanel runat="server" ID="FormPanelInput" ClientIDMode="Static" ButtonAlign="Center" Title="Upload Module and Module Field" Width="2000" BodyPadding="10" AutoScroll="true">
        <Items>
            <ext:FormPanel ID="PanelInfo" runat="server" Title="Module Approval" BodyPadding="5">
                <Items>
                    <ext:DisplayField ID="lblModuleName" runat="server" FieldLabel="Module Name">
                    </ext:DisplayField>
                    <ext:DisplayField ID="lblModuleKey" runat="server" FieldLabel="Module Key">
                    </ext:DisplayField>
                    <ext:DisplayField ID="lblAction" runat="server" FieldLabel="Action">
                    </ext:DisplayField>
                    <ext:DisplayField ID="LblCreatedBy" runat="server" FieldLabel="Created By">
                    </ext:DisplayField>
                    <ext:DisplayField ID="lblCreatedDate" runat="server" FieldLabel="Created Date">
                    </ext:DisplayField>
                </Items>
            </ext:FormPanel>
            <%--<ext:Button ID="btnUpload" runat="server" Text="Upload File" OnDirectClick="btnUpload_Click">
                <LoadingState Text="Please Wait..." />
            </ext:Button>--%>
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
            <ext:Container runat="server">
                <Items>
                    
                </Items>
            </ext:Container>
        </Items>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <%--<ext:Button
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
                    </ext:Button>--%>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Buttons>
                    <ext:Button ID="BtnSave" runat="server" Text="Save" Icon="DiskBlack">
                        <DirectEvents>
                            <Click OnEvent="BtnSave_Click">
                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnReject" runat="server" Text="Reject" Icon="Decline">
                        <DirectEvents>
                            <Click OnEvent="BtnReject_Click">
                                <EventMask ShowMask="true" Msg="Saving Reject Data..." MinDelay="500"> </EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancel" runat="server" Text="Cancel" Icon="PageBack">
                        <DirectEvents>
                            <Click OnEvent="BtnCancel_Click">
                                <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"> </EventMask>

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

