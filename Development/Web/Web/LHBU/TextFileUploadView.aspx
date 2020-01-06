<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="TextFileUploadView.aspx.vb" Inherits="SLIK_TextFileUploadView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:Window ID="WindowProgress" runat="server" Icon="ApplicationViewDetail" Title="Uploading Data..." Hidden="true" Layout="FitLayout" Modal="true"  Width="700">
        <Items>
            <ext:FormPanel ID="FormPanelProgress" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="false" AutoScroll="false" Width="700">
                <Items>
                    <ext:ProgressBar ID="Progress1" runat="server"  />
                </Items>
                <Buttons>
                    <ext:Button ID="btnOK" runat="server" Icon="Disk" Text="OK" Hidden="true">
                        <Listeners>
                            <Click Handler="if (!#{FormPanelProgress}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="btnOK_DirectEvent" >
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" Width="900" AutoScroll="true" Layout="AnchorLayout">
        <Bin>
            <ext:TaskManager ID="TaskManager1" runat="server">
                <Tasks>
                    <ext:Task
                        TaskID="longactionprogress"
                        Interval="1000"
                        AutoRun="false">
                        <DirectEvents>
                            <Update OnEvent="RefreshProgress" />
                        </DirectEvents>
                    </ext:Task>
                </Tasks>
            </ext:TaskManager>
        </Bin>
        <Items>
            <ext:Container runat="server" ID="containerUpload" MarginSpec="0 0 10 0" Width="900" AnchorHorizontal="100%">
                <Items>
                    <ext:ComboBox ID="CboJenisFile" runat="server" FieldLabel="Jenis File" DisplayField="DisplayField"
                        ValueField="DisplayValue" TriggerAction="All" ForceSelection="True" EmptyText="Silahkan Pilih Jenis File" AllowBlank="false" AnchorHorizontal="90%">
                        <Store>
                            <ext:Store runat="server" ID="StoreJenisFile">
                                <Model>
                                    <ext:Model ID="Model14" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="DisplayField" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="DisplayValue" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:FileUploadField runat="server" ID="uploadText" FieldLabel="Upload" MarginSpec="0 10 0 0"></ext:FileUploadField>
                    <ext:Button runat="server" ID="btnUpload" Text="Upload Text File">
                        <DirectEvents>
                            <Click OnEvent="btnUpload_Click" />
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Container>
        </Items>
    </ext:FormPanel>
</asp:Content>

