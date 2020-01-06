<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="EODTaskApprovalDetail.aspx.vb" Inherits="EODTaskApprovalDetail" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <ext:Container ID="container" runat="server" Layout="VBoxLayout"  >
        <LayoutConfig>
            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
        </LayoutConfig>
        <Items>
            <ext:FormPanel ID="PanelInfo" runat="server" Title="Module Approval" BodyPadding="10">
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

            <ext:Panel ID="Panel1" runat="server" Layout="HBoxLayout" ButtonAlign="Center" Flex="1" AutoScroll="true">
                <Items>
                    <ext:FormPanel ID="FormPanelOld" runat="server" Title="Old Value" Flex="1" BodyPadding="10" MarginSpec="0 5 0 0">
                        <Items>
                        </Items>
                    </ext:FormPanel>

                    <ext:FormPanel ID="FormPanelNew" runat="server" Title="New Value" Flex="1" BodyPadding="10">
                        <Items>
                        </Items>
                    </ext:FormPanel>

                </Items>
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
            </ext:Panel>
           

        </Items>
    </ext:Container>
     <ext:FormPanel ID="Panelconfirmation" runat="server" ClientIDMode="Static" Title="Confirmation" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%" Hidden="true">
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