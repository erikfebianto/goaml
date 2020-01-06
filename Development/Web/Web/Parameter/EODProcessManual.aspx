<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="EODProcessManual.aspx.vb" Inherits="EODProcessManual" %>

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

        var filterTreeForm = function (tf, e) {
            var tree = App.Treecombo1,
                text = tf.getRawValue();
            
            tree.clearFilter();
            
            if (Ext.isEmpty(text, false)) {
                clearFilterForm(tf);
                return;
            }

            if (e.getKey() === e.ESC) {
                clearFilterForm();
            } else {
                var re = new RegExp(".*" + text + ".*", "i");

                tree.filterBy(function (node) {
                    return re.test(node.data.text);
                });
            }
        };

        var clearFilterForm = function () {
            var field = App.TbFilter,
                tree = App.Treecombo1;
            
            field.setValue("");
            tree.clearFilter();
            field.focus(false, 100);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:DateField ID="txtStartDate" runat="server" FieldLabel="Process For Report Date" AllowBlank="false" LabelWidth="160">
            </ext:DateField>

            <ext:ComboBox ID="cboProcess" runat="server" FieldLabel="Process" AnchorHorizontal="80%" DisplayField="EODSchedulerName" ValueField="PK_EODScheduler_ID" 
                LabelWidth="160" MinChars="0" ForceSelection="true" TriggerAction="Query" AllowBlank="false" BlankText="Process is required" >
                        <Store>
                            <ext:Store ID="StoreProcess" runat="server" OnReadData="StoreProcess_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelProcess" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EODScheduler_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="EODSchedulerName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Proxy>
                                    <ext:PageProxy>
                                    </ext:PageProxy>
                                </Proxy>
                            </ext:Store>

                        </Store>
                        <DirectEvents>
                            <Change OnEvent="cboProcess_DirectSelect">
                                <EventMask ShowMask="true" MinDelay="100" Msg="Loading..."></EventMask>
                            </Change>
                        </DirectEvents>
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
            <ext:DropDownField ID="cbotask" runat="server" Editable="false" TriggerIcon="SimpleArrowDown" FieldLabel="List Task" LabelWidth="160"
                AnchorHorizontal="80%" AllowBlank="false" ClientIDMode="Static" Mode="ValueText">

                <Listeners>
                    <%-- <Expand Handler="this.picker.setWidth(500);" />--%>
                    <Expand Handler="this.picker.setWidth(600);" />
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
                        ClientIDMode="Static"
                        Frame="true">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:TextField ID="TbFilter" runat="server" ClientIDMode="Static" FieldLabel="Filter :" LabelWidth="50" MinWidth="280" AnchorHorizontal="80%">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyUp Fn="filterTreeForm" Buffer="250" />
                                            <TriggerClick Handler="clearFilterForm();"/>
                                        </Listeners>
                                    </ext:TextField>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
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

