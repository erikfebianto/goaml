<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="MUserEdit.aspx.vb" Inherits="MUserEdit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <ext:FormPanel ID="FormPanelInput" runat="server" Layout="AnchorLayout" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true">
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save User" >
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel">
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