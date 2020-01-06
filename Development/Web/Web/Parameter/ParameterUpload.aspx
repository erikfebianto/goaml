<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="ParameterUpload, App_Web_parameterupload.aspx.252c98" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="Title" Layout="formLayout" Hidden="false">
        <TopBar>
            <ext:Toolbar runat="server">

                <Items>
                    <ext:HyperlinkButton ID="exportDataTemplate" Text="Export Data and Template" runat="server">
                        <DirectEvents>
                            <Click OnEvent="exportDataTemplate_DirectClick">
                            </Click>
                        </DirectEvents>

                    </ext:HyperlinkButton>
                    <ext:HyperlinkButton ID="exportTemplate" Text="Export Template" runat="server">
                        <DirectEvents>
                            <Click OnEvent="exportTemplate_DirectClick">
                            </Click>
                        </DirectEvents>
                    </ext:HyperlinkButton>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:FileUploadField ID="FileUploadField1" runat="server" FieldLabel="Input File ">
            </ext:FileUploadField>
            <ext:ComboBox ID="CboMode" runat="server" FieldLabel="Import Mode" ForceSelection="True" AllowBlank="false" BlankText="Please Select Import Mode">
            
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

        </Items>
        <Buttons>
            <ext:Button ID="BtnSave" runat="server" Icon="Disk" Text="Import">
                <DirectEvents>
                    <Click IsUpload="true" 
                        OnEvent="BtnSave_DirectClick"
                        Before="if (!#{FormPanelInput}.getForm().isValid()) { return false; } 
                                "
                        Failure="Ext.Msg.show({ 
                                title   : 'Error', 
                                msg     : 'Error during uploading', 
                                minWidth: 200, 
                                modal   : true, 
                                icon    : Ext.Msg.ERROR, 
                                buttons : Ext.Msg.OK 
                            });">
                        <EventMask ShowMask="true" Msg="Uploading File...."></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="BtnCancel" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click"></Click>

                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
    <ext:FormPanel ID="FormPanelsave" runat="server" ButtonAlign="Center" Title="Title" Hidden="true" AutoScroll="true">
        <Items>
            <ext:DisplayField ID="txtMode" runat="server" FieldLabel="Import Mode"></ext:DisplayField>
            <ext:GridPanel ID="GridPanelValid" runat="server" Title="" >
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <Store>
                    <ext:Store ID="StoreViewValid" runat="server" RemoteFilter="true" RemoteSort="true"  OnReadData="StoreValid_ReadData" IsPagingStore="true" >
                        
                        <Sorters>
                        </Sorters>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                    </ext:Store>
                </Store>
                <Plugins>
                    <ext:GridFilters ID="GridFiltersValid" runat="server" />

                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbarValid" runat="server" HideRefresh="True" />
                </BottomBar>

            </ext:GridPanel>

         
            <ext:GridPanel ID="GridPanelInValid" runat="server" Title="" >
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                        <Items>
                            <ext:Button runat="server" ID="BtnExport" Text="Export Invalid Data" AutoPostBack="true" OnClick="ExportInvalidData" ClientIDMode="Static" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="StoreInvalid" runat="server" RemoteFilter="true" RemoteSort="true" OnReadData="StoreInValid_ReadData" IsPagingStore="true">
                        <Sorters>
                        </Sorters>
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                    </ext:Store>
                </Store>
                <Plugins>
                    <ext:GridFilters ID="GridFiltersInValid" runat="server" />
                    
                <ext:RowExpander ID="RowExpander1" runat="server">
                    
                    <Template ID="Template1" runat="server">
                        <Html>
							<p><br>Error List:</br> {KeteranganError}</p><br/>							
						</Html>
                    </Template>
                </ext:RowExpander>
            
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbarInValid" runat="server" HideRefresh="True" />
                </BottomBar>
            </ext:GridPanel>
         
           <%-- <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Height="200">
                <Items>
                    <ext:TextArea ID="txtErrorupload" runat="server" FieldLabel="Validation Result" AutoScroll="true">
                    </ext:TextArea>

                </Items>
            </ext:Panel>--%>

        </Items>

        <Buttons>
            <ext:Button ID="btnSaveUpload" runat="server" Text="Save" >
               
                <DirectEvents>
                    <Click OnEvent="btnSaveUpload_Click">
                        <EventMask Msg="Loading.."></EventMask>
                    </Click>
                </DirectEvents>

            </ext:Button>
            <ext:Button ID="btnCancelUpload" runat="server" Text="Back">
                <DirectEvents>
                    <Click OnEvent="btnCancelUpload_Click"></Click>

                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>

          <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%" hidden="true">
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
