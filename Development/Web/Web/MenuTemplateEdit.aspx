<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="MenuTemplateEdit, App_Web_menutemplateedit.aspx.cdcab7d2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function refreshmenu() {
            window.App.TreePanelMenu.expandAll();
            window.App.TreePanelMenu.collapseAll();

        }

        var filterTreemenu = function (tf, e) {
            var tree = App.TreePanelMenu,
                store = tree.store,
                logic = tree,
                text = tf.getRawValue();

            logic.clearFilter();

            if (Ext.isEmpty(text, false)) {
                clearFilter(tf);
                return;
            }

            if (e.getKey() === e.ESC) {
                clearFilter();
            } else {
                var re = new RegExp(".*" + text + ".*", "i");

                //logic.filterBy(function (node) {
                //    return re.test(node.data.text);
                //});
                tree.clearFilter(true);

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
                }, { expandNodes: true });

            }
        };

        var clearFiltermenu = function () {
            var field = App.TriggerFieldmenu,
                tree = App.TreePanelMenu,
                store = tree.store,
                logic = tree;

            field.setValue("");
            logic.clearFilter(true);
            tree.clearFilter(true);
            field.focus(false, 100);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:Panel ID="Panel1" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="Panel6" runat="server" Layout="FormLayout" Region="West" Flex="1" AutoScroll="true">
                <Items>

                    <ext:TreePanel ID="TreePanelMenu" runat="server" Title="Title" AutoScroll="true" SortableColumns="false" Mode="Remote"
                        OnRemoteMove="RemoteMove" ClientIDMode="Static">
                        <SelectionSubmitConfig WithChildren="true" />
                        <View>
                            <ext:TreeView runat="server" ID="TreeViewMenu">
                                <Plugins>
                                    <ext:TreeViewDragDrop runat="server" AllowContainerDrops="true" AllowLeafDrop="true" AllowParentInserts="true">
                                    </ext:TreeViewDragDrop>
                                </Plugins>
                                <%-- <DirectEvents>
                                    <Drop OnEvent="NodeDragOverEvent">
                                        <EventMask ShowMask="true"></EventMask>
                                        <ExtraParams>
                                            <ext:Parameter Name="Nodedata" Value="record.data.mMenuID" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Drop>
                                </DirectEvents>--%>
                            </ext:TreeView>
                        </View>
                        <SelectionModel>
                            <ext:TreeSelectionModel runat="server" Mode="Multi"></ext:TreeSelectionModel>
                        </SelectionModel>
                        <TopBar>

                            <ext:Toolbar runat="server" ID="toolbarmenu">
                                <Items>
                                    <ext:Button ID="btnAddNew" runat="server" Icon="Add" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddNew_DirectClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnDelete" runat="server" Icon="Delete" Disabled="true">

                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_DirectClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnSaveMenuAll" runat="server" Icon="Disk">

                                        <DirectEvents>
                                            <Click OnEvent="btnSaveMenuAll_DirectClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnUp" runat="server" Icon="ArrowUp" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnUp_directClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnDown" runat="server" Icon="ArrowDown" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDown_directClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <Fields>
                            <ext:ModelField Name="mMenu" Type="Auto" />
                            <ext:ModelField Name="mMenuID" Type="Auto" />
                            <ext:ModelField Name="MenuLabel" Type="String" />
                            <ext:ModelField Name="MenuParent" Type="Auto" />
                            <ext:ModelField Name="MenuURL" Type="String" />
                            <ext:ModelField Name="moduleid" Type="Auto" />
                            <ext:ModelField Name="actionid" Type="Auto" />
                        </Fields>
                        <ColumnModel>
                            <Columns>
                                <ext:TreeColumn runat="server" Text="Menu Tree" ID="treecol1" DataIndex="MenuLabel" Flex="1"></ext:TreeColumn>

                                <ext:Column ID="Column1" runat="server" DataIndex="moduleid" Text="Module"></ext:Column>
                                <ext:Column ID="Column2" runat="server" DataIndex="actionid" Text="Action"></ext:Column>
                            </Columns>
                        </ColumnModel>
                        <DirectEvents>
                            <ItemClick OnEvent="treepanel_itemClick">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.mMenu" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="index" Value="index" Mode="Raw"></ext:Parameter>
                                </ExtraParams>
                            </ItemClick>
                        </DirectEvents>
                    </ext:TreePanel>
                </Items>
            </ext:Panel>

            <ext:FormPanel ID="PanelData" runat="server" Region="Center" Border="true" Flex="1" ButtonAlign="Center" Padding="1">
                <Items>
                    <ext:DisplayField ID="IDData" runat="server" FieldLabel="ID">
                    </ext:DisplayField>
                    <ext:TextField ID="MenuLabel" runat="server" FieldLabel="Name">
                    </ext:TextField>
                    <ext:DisplayField ID="MenuParent" runat="server" FieldLabel="Parent"></ext:DisplayField>
                    <ext:DisplayField ID="MenuURL" runat="server" FieldLabel="URL">
                    </ext:DisplayField>
                    <ext:ComboBox ID="cboModule" runat="server" FieldLabel="Module" MinChars="0" AnchorHorizontal="70%" DisplayField="ModuleLabel" ValueField="PK_Module_ID" ForceSelection="true" TriggerAction="Query" EmptyText="[Select Data to Set Module]">
                        <Store>
                            <ext:Store ID="StoreModule" runat="server" OnReadData="StoreModule_ReadData">
                                <Model>
                                    <ext:Model ID="ModelModule" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_Module_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="ModuleLabel" Type="String"></ext:ModelField>
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
                            <Select OnEvent="MenuModule_Select">
                            </Select>
                        </DirectEvents>

                        <%--   <Listeners>
                            <Select Handler="#{cboAction}.refresh();"></Select>
                        </Listeners>--%>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();NawadataDirect.ClearURL();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>

                    <ext:ComboBox ID="cboAction" runat="server" FieldLabel="Action" MinChars="0" AnchorHorizontal="70%" DisplayField="ModuleActionName" ValueField="PK_ModuleAction_ID" ForceSelection="true" TriggerAction="Query" EmptyText="[Select Data to Set Action]">
                        <Store>
                            <ext:Store ID="StoreAction" runat="server" OnReadData="StoreAction_ReadData" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelAction" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_ModuleAction_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="ModuleActionName" Type="String"></ext:ModelField>
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
                            <Select OnEvent="cboAction_DirectClick">
                            </Select>
                        </DirectEvents>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) {
                                           this.clearValue();NawadataDirect.ClearURL();
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSaveMenu" runat="server" Icon="Disk" Text="Update Menu">
                        <DirectEvents>
                            <Click OnEvent="BtnSaveMenu_Click">
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel">
                        <Listeners>
                            <Click Handler="App.ContentPlaceHolder1_TreePanelMenu.getSelectionModel().clearSelections();"></Click>
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Panel>
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