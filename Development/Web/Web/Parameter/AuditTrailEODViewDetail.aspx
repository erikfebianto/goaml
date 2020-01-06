<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="AuditTrailEODViewDetail.aspx.vb" Inherits="AuditTrailEODViewDetail" %>

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
       <ext:FormPanel ID="FormPanelInput" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="AnchorLayout" ButtonAlign="Center" autoscroll="true">

        <Buttons>
            <%--<ext:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="Delete" Enabled="false" Icon="DiskBlack">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Callback">
                        <ExtraParams>
                            <ext:Parameter Name="command" Value="Delete" Mode="Value">
                            </ext:Parameter>

                        </ExtraParams>
                        <EventMask ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>--%>
            <ext:Button ID="btnCancel" runat="server" Text="Back" Icon="PageBack">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click"></Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>

    </ext:FormPanel>
    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%">
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

