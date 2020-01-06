<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="MenuTemplateView, App_Web_menutemplateview.aspx.cdcab7d2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">



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
            <ext:Panel ID="Panel6" runat="server" Layout="FormLayout" Region="West" Flex="1">
                <Items>

                    <ext:TreePanel ID="TreePanelMenu" ClientIDMode="Static" runat="server" Title="Title" AutoScroll="true" SortableColumns="false"  >
                        <Fields>
                            <ext:ModelField Name="mMenuID"  Type="Auto" />
                            <ext:ModelField Name="MenuLabel"  Type="String" />
                            <ext:ModelField Name="MenuParent"  Type="Auto"  />
                            <ext:ModelField Name="MenuURL"  Type="String" />                            
                            <ext:ModelField Name="moduleid"  Type="Auto"  />
                            <ext:ModelField Name="actionid"  Type="Auto" />
                            <ext:ModelField Name="urutan"  Type="Int" />
                        </Fields>
                        <ColumnModel >
                            <Columns>
                                <ext:TreeColumn runat="server" Text="Menu Tree" ID="treecol1" DataIndex="MenuLabel" Flex="1" ></ext:TreeColumn>
                                
                                
                                <ext:Column ID="Column1" runat="server" DataIndex="moduleid" Text="Module" ></ext:Column>
                                <ext:Column ID="Column2" runat="server" DataIndex="actionid" Text="Action" ></ext:Column>
                            </Columns>
                        </ColumnModel>
                        <DirectEvents>
                            <ItemClick OnEvent="treepanel_itemClick">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.mMenuID" Mode="Raw" >
                                    </ext:Parameter>
                                </ExtraParams>
                            </ItemClick>
                        </DirectEvents>                        
                    </ext:TreePanel>
                </Items>
            </ext:Panel>

            <ext:FormPanel ID="PanelData" runat="server" Region="Center" Border="true" Flex="1" ButtonAlign="Right" Padding="1">
                <Items>
                    <ext:DisplayField ID="IDData" runat="server" FieldLabel="ID">
                    </ext:DisplayField>
                    <ext:DisplayField ID="MenuLabel" runat="server" FieldLabel="Name">
                    </ext:DisplayField>
                    <ext:DisplayField ID="MenuParent" runat="server" FieldLabel="Parent">
                    </ext:DisplayField>
                    <ext:DisplayField ID="MenuURL" runat="server" FieldLabel="URL">
                    </ext:DisplayField>
                    <ext:DisplayField ID="MenuModule" runat="server" FieldLabel="Module">
                    </ext:DisplayField>
                    <ext:DisplayField ID="ModuleAction" runat="server" FieldLabel="Action">
                    </ext:DisplayField>
                </Items>
                <Buttons>
                    
                </Buttons>
            </ext:FormPanel>


        </Items>
    </ext:Panel>

</asp:Content>
