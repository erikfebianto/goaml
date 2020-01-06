<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="MGroupAccessAdd.aspx.vb" Inherits="MGroupAccessAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">


        Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
            return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
        };


        Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
            return {
                type: "string",
                op: "*",
                value: value
            };
        };


        var SelectAll = function (value) {

            
            if (value.tag != 'disabled') {

                window.App.StoreView.data.each(function (record) {

                    if (record.get('IsSupportAdd') == true) {
                        record.set('BAdd', value.checked);
                    }
                    else {
                        record.set('BAdd', '-');
                    }


                    if (record.get('IsSupportEdit') == true) {
                        record.set('BEdit', value.checked);
                    }
                    else {
                        record.set('BEdit', '-');
                    }


                    if (record.get('IsSupportDelete') == true) {
                        record.set('BDelete', value.checked);
                    }
                    else {
                        record.set('BDelete', '-');
                    }


                    if (record.get('IsSupportActivation') == true) {
                        record.set('BActivation', value.checked);
                    }
                    else {
                        record.set('BActivation', '-');
                    }

                    if (record.get('IsSupportView') == true) {
                        record.set('BView', value.checked);
                    }
                    else {
                        record.set('BView', '-');
                    }

                    if (record.get('IsSupportUpload') == true) {
                        record.set('BUpload', value.checked);
                    }
                    else {
                        record.set('BUpload', '-');
                    }

                    if (record.get('IsSupportDetail') == true) {
                        record.set('BDetail', value.checked);
                    }
                    else {
                        record.set('BDetail', '-');
                    }

                    if (record.get('IsUseApproval') == true) {
                        record.set('BApproval', value.checked);
                    }
                    else {
                        record.set('BApproval', '-');
                    }


                    value.tag = 'disabled1';

                });
            }
            else {
                value.tag = ""
            }
        };

        var onClear = function () {

            
            window.App.GridpanelAdd.filterHeader.clearFilter();

          
        };
    </script>

    <ext:Panel ID="Panel1" runat="server" Layout="BorderLayout" Flex="1">
        <Items>
            <ext:Panel ID="Panel6" runat="server" Border="true" Region="Center" Flex="3" AnchorVertical="100%" Layout="FitLayout">
                <Items>
                    <ext:GridPanel ID="GridpanelAdd" ClientIDMode="Static"  runat="server" Title="Title" AutoScroll="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                                <Items>

                                    <ext:ComboBox runat="server" ID="cboGroupMenu" Editable="false" EmptyText="[Select Group Menu]" FieldLabel="Group Menu:" DisplayField="GroupMenuName" ValueField="PK_MGroupMenu_ID">
                                        <Store>
                                            <ext:Store ID="GroupMenuStore" runat="server" OnReadData="CboGroupmenu_Read">
                                                <Model>
                                                    <ext:Model runat="server" ID="GroupmenuModel">
                                                        <Fields>
                                                            <ext:ModelField Name="GroupMenuName" Type="String"></ext:ModelField>
                                                            <ext:ModelField Name="PK_MGroupMenu_ID" Type="Int"></ext:ModelField>
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:Button runat="server" ID="BtnOK" Text="Select" ClientIDMode="Static">
                                        <DirectEvents>
                                            <Click OnEvent="BtnOk_DirectClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                                            </Click>

                                        </DirectEvents>
                                    </ext:Button>

<ext:ToolbarSeparator ></ext:ToolbarSeparator>

                                    <ext:Checkbox ID="btnSelectAll" runat="server" Text="Select All" BoxLabel="Select ALL Current Page">
                                        <Listeners>
                                            <Change Handler="SelectAll(this);" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <Store>
                            <ext:Store ID="StoreView" ClientIDMode="Static" runat="server" OnReadData="Store_ReadData">

                                <Sorters>
                                    <%--<ext:DataSorter Property="" Direction="ASC" />--%>
                                </Sorters>
                                <Model>
                                    <ext:Model runat="server" ID="modelgrid" IDProperty="PK_Module_ID">
                                        <Fields>
                                            <ext:ModelField Name="PK_Module_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="PK_MGroupMenu_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="GroupMenuName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="ModuleLabel" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="BAdd" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BEdit" Type="String">
                                                     <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BDelete" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BActivation" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BView" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BApproval" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BUpload" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BDetail" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return true } else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="IsSupportAdd" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportEdit" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportDelete" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportActivation" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportView" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportUpload" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsSupportDetail" Type="Boolean"></ext:ModelField>
                                            <ext:ModelField Name="IsUseApproval" Type="Boolean"></ext:ModelField>
                                            
                                        </Fields>
                                    </ext:Model>
                                </Model>

                                
                            </ext:Store>
                        </Store>
                        <Plugins>
                            <%--<ext:GridFilters ID="GridFilters1" runat="server" />--%>
                            <ext:FilterHeader ID="GridHeader1" runat="server" Remote="false" ClientIDMode="Static" />

                        </Plugins>
                            <BottomBar>
            
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" >
                <DirectEvents>
                    <Change OnEvent="ChangePage"></Change>
                </DirectEvents>
            </ext:PagingToolbar>
                                        
        </BottomBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>

                                <ext:Column ID="ColModuleLabel" runat="server" DataIndex="ModuleLabel" Flex="1" Text="Sub Module"></ext:Column>
                                <ext:ComponentColumn ID="Componentadd" runat="server" Editor="true" DataIndex="BAdd" Flex="1" Text="Add" Align="Center" >
                                    <Component>
                                        <%--<ext:ComboBox ID="comboadd" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                        <ext:Checkbox ID="comboadd" runat="server" Editor="true" DataIndex="BAdd"  Flex="1" Text="Add"   >                                           </ext:Checkbox> 
                                        
                                        
                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportAdd==0)  {return false;  }"></BeforeBind>
                                        
                                    </Listeners>
                                </ext:ComponentColumn>

                                <ext:ComponentColumn ID="Componentedit" runat="server" Editor="true" DataIndex="BEdit" Flex="1" Text="Edit" Align="Center">
                                    <Component>
                                        <%--<ext:ComboBox ID="ComboBoxedit" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                         <ext:Checkbox ID="ComboBoxedit" runat="server" Editor="true" DataIndex="BEdit"  Flex="1" Text="Edit"   ></ext:Checkbox>  

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportEdit==0)  return false;"></BeforeBind>
                                        
                                    </Listeners>
                                </ext:ComponentColumn>

                                <ext:ComponentColumn ID="ComponentDelete" runat="server" Editor="true" DataIndex="BDelete" Flex="1" Text="Delete" Align="Center">
                                    <Component>
                                      <%--  <ext:ComboBox ID="ComboBoxDelete" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                        <ext:Checkbox ID="ComboBoxDelete" runat="server" Editor="true" DataIndex="BDelete"  Flex="1" Text="Delete"   ></ext:Checkbox>

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportDelete==0)  return false;"></BeforeBind>
                                        
                                    </Listeners>
                                </ext:ComponentColumn>

                                <ext:ComponentColumn ID="ComponentView" runat="server" Editor="true" DataIndex="BView" Flex="1" Text="View" Align="Center">
                                    <Component>
                                     <%--   <ext:ComboBox ID="ComboBoxView" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                         <ext:Checkbox ID="ComboBoxView" runat="server" Editor="true" DataIndex="BView"  Flex="1" Text="View"   ></ext:Checkbox>

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportView==0)  return false;"></BeforeBind>
                                    </Listeners>
                                </ext:ComponentColumn>
                                <ext:ComponentColumn ID="ComponentActivation" runat="server" Editor="true" DataIndex="BActivation" Flex="1" Text="Activation" Align="Center">
                                    <Component>
                                     <%--   <ext:ComboBox ID="ComboBoxActivation" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                         <ext:Checkbox ID="ComboBoxActivation" runat="server" Editor="true" DataIndex="BActivation"  Flex="1" Text="Activation"   ></ext:Checkbox>

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportActivation==0)  return false;"></BeforeBind>
                                    </Listeners>
                                </ext:ComponentColumn>
                                <ext:ComponentColumn ID="ComponentApproval" runat="server" Editor="true" DataIndex="BApproval" Flex="1" Text="Approval" Align="Center">
                                    <Component>
                                       <%-- <ext:ComboBox ID="ComboBoxApproval" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>
                                        </ext:ComboBox>--%>
                                           <ext:Checkbox ID="ComboBoxApproval" runat="server" Editor="true" DataIndex="BApproval"  Flex="1" Text="Approval"   ></ext:Checkbox>

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsUseApproval==0)  return false;"></BeforeBind>
                                    </Listeners>
                                </ext:ComponentColumn>


                                <ext:ComponentColumn ID="ComponentUpload" runat="server" Editor="true" DataIndex="BUpload" Flex="1" Text="Upload" Align="Center">
                                    <Component>
                                       <%-- <ext:ComboBox ID="ComboBoxUpload" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>

                                        </ext:ComboBox>--%>
                                          <ext:Checkbox ID="ComboBoxUpload" runat="server" Editor="true" DataIndex="BUpload"  Flex="1" Text="Upload"   ></ext:Checkbox>



                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportUpload==0)  return false;"></BeforeBind>
                                    </Listeners>
                                </ext:ComponentColumn>
                                <ext:ComponentColumn ID="ComponentDetail" runat="server" Editor="true" DataIndex="BDetail" Flex="1" Text="Detail" Align="Center">
                                    <Component>
                                       <%-- <ext:ComboBox ID="ComboBoxDetail" runat="server">
                                            <Items>
                                                <ext:ListItem Text="True" Value="True"></ext:ListItem>
                                                <ext:ListItem Text="False" Value="False"></ext:ListItem>
                                            </Items>

                                        </ext:ComboBox>--%>


                                          <ext:Checkbox ID="ComboBoxDetail" runat="server" Editor="true" DataIndex="BDetail"  Flex="1" Text="Detail"   ></ext:Checkbox>

                                    </Component>
                                    <Listeners>
                                        <BeforeBind Handler=" if (e.record.data.IsSupportDetail==0)  return false;"></BeforeBind>
                                    </Listeners>
                                </ext:ComponentColumn>

                            </Columns>
                        </ColumnModel>

                        <Buttons>
                            <ext:Button ID="btnSaveData" runat="server" Text="Save All Role Access">
                                <DirectEvents>
                                    <Click OnEvent="BtnSaveData_DirectClick">
                                        <ExtraParams>
                                            <ext:Parameter Name="Grid1" Value="Ext.encode(#{GridpanelAdd}.getRowsValues({selectedOnly : false}))" Mode="Raw" />

                                        </ExtraParams>
                                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="BtnCancelSave" runat="server" Text="Cancel">
                                <DirectEvents>
                                    <Click OnEvent="BtnCancelSave_DirectClick">
                                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"> </EventMask>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>

                    </ext:GridPanel>

                </Items>
            </ext:Panel>


            <ext:Panel ID="Panel2" Title="Detail"  runat="server" Layout="VBoxLayout" Region="East" Flex="1" Collapsed="true" Collapsible="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <Items>

                    <ext:TreePanel ID="TreePanelMenu" runat="server" Title="Menu" Flex="1" AutoScroll="true" SortableColumns="false" Mode="Remote" Hidden="true"
                        OnRemoteMove="RemoteMove">
                        <SelectionSubmitConfig WithChildren="true" />
                        <View>
                            <ext:TreeView runat="server" ID="TreeViewMenu">
                                <Plugins>
                                    <ext:TreeViewDragDrop ID="TreeViewDragDrop1" runat="server" AllowContainerDrops="true" AllowLeafDrop="true" AllowParentInserts="true">
                                    </ext:TreeViewDragDrop>
                                </Plugins>

                            </ext:TreeView>
                        </View>
                        <SelectionModel>
                            <ext:TreeSelectionModel ID="TreeSelectionModel1" runat="server" Mode="Multi"></ext:TreeSelectionModel>
                        </SelectionModel>
                        <TopBar>

                            <ext:Toolbar runat="server" ID="toolbarmenu">
                                <Items>

                                    <ext:Button ID="btnUp" runat="server" Icon="ArrowUp" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnUp_directClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnDown" runat="server" Icon="ArrowDown" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDown_directClick">
                                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
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
                            <ext:ModelField Name="PK_MGroupMenuSettting_ID" Type="Int" />
                            <ext:ModelField Name="FK_MGroupMenu_ID" Type="Int" />


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

                    <ext:FormPanel ID="PanelData" runat="server" Region="Center" ButtonAlign="Center" Flex="1" Title="Menu Detail" Hidden="true">
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
                                    <ext:Store ID="StoreModule" runat="server" OnReadData="StoreModule_ReadData" IsPagingStore="true">
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
                                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
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
                    <Click OnEvent="BtnConfirmation_DirectClick">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"> </EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>


