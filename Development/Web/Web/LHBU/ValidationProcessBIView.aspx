<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="ValidationProcessBIView.aspx.vb" Inherits="ValidationProcessBIView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .x-tree-icon {
            display: none !important;
        }

        .x-panel-default-outer-border-rbl {
            border-color: #c2c2c2 !important;
        }

        .container-treeform {
            border: 1px solid rgb(194, 194, 194);
            border-radius: 5px;
        }
    </style>

    <script>
        onCheckTreeParent = function (node) {
            var filterText = App.TbFilter.value.trim();

            if (filterText == null || filterText == '') {
                node.set('checked', node.parentNode.data.checked);
            }
            else {
                if (node.data.level == 'group') {
                    if (node.data.children.filter(function (x) { return x.text.toLowerCase().indexOf(filterText) > -1; })[0] != null)
                        node.set('checked', node.parentNode.data.checked);
                }
                else if (node.data.level == 'form') {
                    if (node.data.text.toLowerCase().indexOf(filterText) > -1)
                        node.set('checked', node.parentNode.data.checked);
                }
            }
        }
        
        var filterTreeForm = function (tf, e) {
            var tree = App.TreePanelForm,
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
                
                tree.filterBy(function (node) {
                    return re.test(node.data.text);
                });
                
                tree.filterBy(function (node) {
                    return re.test(node.data.text);
                }, { expandNodes: true });

                /*tree.filterBy(function (node) {
                    var tags = node.data.text,
                        hasTags = Ext.isArray(node.data.text) && node.data.text.length > 0,
                        match = false,
                        pn = node.parentNode,
                        pnIsFixed = false,
                        i, len;

                    match = re.test(node.data.text);

                   

                    if (match && node.isLeaf()) {
                        pn.hasMatchNode = true;
                    }

                    if (pn) {
                        node.bubble(function (n) {
                            if (node != n) {
                                pnIsFixed = re.test(n.data.text);

                                return !pnIsFixed;
                            }
                        });
                    }

                    if (pn != null && pnIsFixed) {
                        return true;
                    }

                    if (node.isLeaf() === false) {
                        return match;
                    }
                    
                    return pnIsFixed || match;
                }, { expandNodes: true });

                tree.filterBy(function (node) {
                    var tags = node.data.text,
                        hasTags = Ext.isArray(node.data.text) && node.data.text.length > 0,
                        match = false,
                        pn = node.parentNode,
                        pnIsFixed = false,
                        i, len;

                    match = re.test(node.data.text);

                    if (match && node.isLeaf()) {
                        pn.hasMatchNode = true;
                    }

                    if (pn) {
                        node.bubble(function (n) {
                            if (node != n) {
                                pnIsFixed = re.test(n.data.text);

                                return !pnIsFixed;
                            }
                        });
                    }

                    if (pn != null && pnIsFixed) {
                        return true;
                    }

                    if (node.isLeaf() === false) {
                        return match;
                    }
                    
                    return pnIsFixed || match;
                }, { expandNodes: true });*/
            }
        };

        var clearFilterForm = function () {
            var field = App.TbFilter,
                tree = App.TreePanelForm;
            
            field.setValue("");
            tree.clearFilter();
            field.focus(false, 100);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:Window ID="WindowProgress" runat="server" Icon="ApplicationViewDetail" Title="Progress" Hidden="true" Layout="FitLayout" Modal="true" Width="400">
        <Items>
            <ext:FormPanel ID="FormPanelProgress" runat="server" Padding="5" AnchorHorizontal="100%" StyleSpec="overflow:hidden;">
                <Items>
                    <ext:ProgressBar ID="Progress1" runat="server" />
                    <ext:Label ID="LblValidateError" runat="server" AutoScroll="true" Hidden="true" StyleSpec="display:block; height:auto; float:left; padding: 8px; max-height: 300px;" />
                </Items>
                <Buttons>
                    <ext:Button ID="BtnConfirm" runat="server" Icon="Disk" Text="OK" StyleSpec="cursor: pointer;">
                        <Listeners>
                            <Click Handler="if (!#{FormPanelProgress}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="BtnConfirm_OnClicked">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <Resize Handler="#{WindowProgress}.center()" />
        </Listeners>
    </ext:Window>
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyPadding="20" AutoScroll="true" Layout="AnchorLayout">
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
         <%--   <ext:ComboBox ID="CboDataSet" runat="server" FieldLabel="Data Set" DisplayField="Label" ValueField="ID" LabelWidth="160"
                AnchorHorizontal="35%" AllowBlank="false" ForceSelection="true" BlankText="Data Set is required" EmptyText="[Select Data Set]">
                <Store>
                    <ext:Store ID="StoreDataSet" ClientIDMode="Static" runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int">
                                    </ext:ModelField>
                                    <ext:ModelField Name="Label" Type="String">
                                    </ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <DirectEvents>
                    <Select OnEvent="CboDataSet_OnSelected">
                        <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"></EventMask>
                    </Select>
                </DirectEvents>
            </ext:ComboBox>--%>
            <ext:Container ID="ContainerDocStatus" runat="server" ClientIDMode="Static" PaddingSpec="8 5" MarginSpec="10 0" Cls="container-treeform" >
                <Items>
                    <ext:Label runat="server" ID="lblSelection" Text="Select Form(s) to Validate :" />
                    <ext:TreePanel ID="TreePanelForm" ClientIDMode="Static" runat="server"
                            MaxHeight="375" MinHeight="375" AutoScroll="True" AnimCollapse="false">
                        <TopBar>
                            <ext:Toolbar runat="server" StyleSpec="border: none !important; padding: 10px 0 !important">
                                <Items>
                                    <ext:ToolbarTextItem runat="server" Text="Filter:"></ext:ToolbarTextItem>
                                    <ext:ToolbarSpacer />
                                    <ext:TextField  ID="TbFilter" ClientIDMode="Static" runat="server" EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyUp Fn="filterTreeForm" Buffer="250" />
                                            <TriggerClick Handler="clearFilterForm();"/>
                                        </Listeners>
                                        <DirectEvents>
                                            <%--<KeyDown OnEvent="TbFilter_OnKeyDown" />--%>
                                            <Change OnEvent="TbFilter_OnChanged" />
                                        </DirectEvents>
                                    </ext:TextField>
                                    <ext:Button runat="server" ID="BtnSelect" Text="Select All" Icon="Tick" MarginSpec="0 4" StyleSpec="cursor: pointer;">
                                        <DirectEvents>
                                            <Click OnEvent="BtnSelect_OnClick"></Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="BtnExpand" Text="Expand All" Icon="ArrowOut" MarginSpec="0 4" StyleSpec="cursor: pointer;">
                                        <DirectEvents>
                                            <Click OnEvent="BtnExpand_OnClick"></Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="BtnCollapse" Text="Collapse All" Icon="ArrowIn"  MarginSpec="0 4" StyleSpec="cursor: pointer;">
                                        <DirectEvents>
                                            <Click OnEvent="BtnCollapse_OnClick"></Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Checkbox runat="server" ID="CeSelectAll" StyleSpec="display:none;"></ext:Checkbox>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <DirectEvents>
                            <ItemClick OnEvent="TreePanelItem_OnClicked">
                                <ExtraParams>
                                    <ext:Parameter Name="NodeID" Value="record.data.id" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="Level" Value="record.data.level" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="Period" Value="record.data.period" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="GroupID" Value="record.data.groupID" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="ModuleID" Value="record.data.moduleID" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="IsChecked" Value="record.data.checked" Mode="Raw" ></ext:Parameter>
                                </ExtraParams>
                            </ItemClick>
                        </DirectEvents>
                    </ext:TreePanel>
                </Items>
            </ext:Container>
            <ext:Button runat="server" ID="BtnGenerate" Text="Validate Form(s)" StyleSpec="cursor: pointer;">
                <DirectEvents>
                    <Click OnEvent="BtnGenerate_OnClicked"></Click>
                </DirectEvents>
            </ext:Button>
        </Items>
    </ext:FormPanel>
</asp:Content>

