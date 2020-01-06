<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.master" AutoEventWireup="false" CodeFile="ArchieveRestoreData.aspx.vb" Inherits="EODProcessManualSatuan" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>

        var getValues = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();

            Ext.each(selNodes, function (node) {
                msg.push(node.data.id);
            });

            return msg.join(",");
        };

        var getText = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            msg.push("[");

            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                }

                msg.push(node.data.text);
            });

            msg.push("]");

            return msg.join("");
        };

        var syncValue = function (value) {
            var tree = this.component;

            var ids = value.split(",");
            tree.setChecked({ ids: ids, silent: true });

            tree.getSelectionModel().deselectAll();
            Ext.each(ids, function (id) {
                var node = tree.store.getNodeById(id);

                if (node) {
                    tree.getSelectionModel().select(node, true);
                }
            }, this);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:Window ID="WindowPrompt" runat="server" Icon="Disk" Title="Konfirmasi" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelPrompt" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>
                    <ext:DisplayField ID="LblPrompt" runat="server" FieldLabel="" Text="Apakah Anda yakin melakukan submit data?" EmptyText="" AutoScroll="true" AnchorHorizontal="90%" ClientIDMode="Static">
                    </ext:DisplayField>
                </Items>
                <Buttons>
                    <ext:Button ID="BtnSubmitPrompt" runat="server" Icon="Disk" Text="Yes">
                        <DirectEvents>
                            <Click OnEvent="BtnSubmitPrompt_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="100" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelPrompt" runat="server" Icon="Cancel" Text="No">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelPrompt_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="100" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.3});" />
            <Resize Handler="#{WindowPrompt}.center()" />
        </Listeners>
    </ext:Window>
    <ext:Window ID="WindowProgress" runat="server" Icon="ApplicationViewDetail" Title="Progress" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelProgress" runat="server" Padding="5" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>
                    <ext:ProgressBar ID="Progress1" runat="server" Width="300" />

                </Items>
                <Buttons>
                    <ext:Button ID="btnOK" runat="server" Icon="Disk" Text="OK">
                        <Listeners>
                            <Click Handler="if (!#{FormPanelProgress}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="BtnConfirmation_DirectClick">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.3});" />
            <Resize Handler="#{WindowProgress}.center()" />
        </Listeners>
    </ext:Window>
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
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
            <ext:DateField ID ="txtStartDate" runat="server" FieldLabel="Process Date" Type="Month" AllowBlank="false"/>
            
            <ext:ComboBox
            FieldLabel="Task" 
            runat="server" 
            Id ="ucProcess"
            Editable="false">
                <Items>
                    <ext:ListItem Text="Archieve" Value="Archieving" />
                    <ext:ListItem Text="Restore" Value="Restoring" />
                </Items>
                <SelectedItems>
                    <ext:ListItem Index="0"></ext:ListItem>
                </SelectedItems>    
            </ext:ComboBox>

        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Run Process">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Clear">
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
