<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.master" AutoEventWireup="false" CodeFile="EODProcessManualSatuan.aspx.vb" Inherits="EODProcessManualSatuan" %>


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
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:DateField ID="txtStartDate" runat="server" FieldLabel="Process For Report Date" AllowBlank="false" LabelWidth="160">
            </ext:DateField>
            <ext:DisplayField ID="ProcessID" runat="server" FieldLabel="Process" LabelWidth="160"></ext:DisplayField>
           
            <ext:DropDownField ID="cbotask" runat="server" Editable="false" TriggerIcon="SimpleArrowDown" FieldLabel="List Task" AnchorHorizontal="65%" 
                AllowBlank="false" ClientIDMode="Static" Mode="ValueText" LabelWidth="160">

                <Listeners>
                    <%-- <Expand Handler="this.picker.setWidth(500);" />--%>
                    <Expand Handler="this.picker.setWidth(500);" />
                </Listeners>
                <Component>
                    <ext:TreePanel
                        runat="server"
                        Title="Picker Process"
                        Icon="Accept"
                        Height="300"
                        Width="300"
                        Shadow="false"
                        UseArrows="true"
                        AutoScroll="true"
                        Animate="true"
                        EnableDD="true"
                        ContainerScroll="true"
                        RootVisible="false" ID="Treecombo1"
                        Frame="true">
                        <Buttons>
                            <ext:Button ID="BtnSelectTask" runat="server" Text="Select All" Icon="Tick" StyleSpec="cursor: pointer;">
                                <DirectEvents>
                                    <Click OnEvent="BtnSelectTask_OnClicked"></Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="BtnUnselectTask" runat="server" Text="Unselect All" Icon="Cross" StyleSpec="cursor: pointer;">
                                <DirectEvents>
                                    <Click OnEvent="BtnUnselectTask_OnClicked"></Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" Text="Close" Icon="Decline" StyleSpec="cursor: pointer;">
                                <Listeners>
                                    <Click Handler="#{cbotask}.collapse();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                      <Listeners>
                        <CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
                    </Listeners>
                    </ext:TreePanel>
                </Component>
                <Triggers>
                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                </Triggers>
                <Listeners>
                    <Change Handler="this.getTrigger(0).show();" />
                    <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                </Listeners>
            </ext:DropDownField>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Process" StyleSpec="cursor: pointer;">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel" StyleSpec="cursor: pointer;">
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
