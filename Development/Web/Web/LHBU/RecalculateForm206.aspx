<%@ Page Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="RecalculateForm206.aspx.vb" Inherits="LHBU_RecalculateForm206" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:Window ID="WindowProgress" runat="server" Icon="ApplicationViewDetail" Title="Uploading Data..." Hidden="true" Layout="FitLayout" Modal="true"  Width="700">
        <Items>
            <ext:FormPanel ID="FormPanelProgress" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="false" AutoScroll="false" Width="700">
                <Items>
                    <ext:ProgressBar ID="Progress1" runat="server"  />
                </Items>
                <Buttons>
                    <ext:Button ID="btnOK" runat="server" Icon="Disk" Text="OK" Hidden="true">
                        <%--<Listeners>
                            <Click Handler="if (!#{FormPanelProgress}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        --%><DirectEvents>
                            <Click OnEvent="btnOK_DirectEvent" >
                                <%--<EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>--%>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:Container ID="ContainerDocStatus" runat="server" Border="true" MarginSpec="0 0 10 10">
                <Items>
                    <ext:Button runat="server" ID="btnRecalculate" Text="Recalculate Form 206" AutoPostBack="true" OnClick="btnRecalculate_Click">
                    </ext:Button>
                </Items>
            </ext:Container>
        </Items>
    </ext:FormPanel>
</asp:Content>

