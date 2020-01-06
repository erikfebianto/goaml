<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="GenerateTextFileView.aspx.vb" Inherits="LHBU_GenerateTextFileView" %>

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
                else if (node.data.level == 'category') {
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
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyPadding="20" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:ComboBox ID="CboDataSet" runat="server" FieldLabel="Data Set" DisplayField="Label" ValueField="ID" LabelWidth="160"
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
            </ext:ComboBox>
            <ext:Container ID="ContainerDocStatus" runat="server" ClientIDMode="Static" PaddingSpec="8 5" MarginSpec="10 0" Cls="container-treeform" >
                <Items>
                    <ext:Label runat="server" ID="LblSelection" Text="Select Category(s) to Generate :" />
                    <ext:TreePanel ID="TreePanelForm" ClientIDMode="Static" runat="server"
                            MaxHeight="370" MinHeight="370" AutoScroll="True" AnimCollapse="false">
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
                                    <ext:Parameter Name="Category" Value="record.data.category" Mode="Raw" ></ext:Parameter>
                                    <ext:Parameter Name="IsChecked" Value="record.data.checked" Mode="Raw" ></ext:Parameter>
                                </ExtraParams>
                            </ItemClick>
                        </DirectEvents>
                    </ext:TreePanel>
                </Items>
            </ext:Container>
            <ext:Button runat="server" ID="BtnGenerate" Text="Generate" StyleSpec="display: inline-block; cursor: pointer;">
                <DirectEvents>
                    <Click OnEvent="BtnGenerate_OnClicked">
                        <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Items>
    </ext:FormPanel>
</asp:Content>

