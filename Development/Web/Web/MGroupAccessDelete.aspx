<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="MGroupAccessDelete.aspx.vb" Inherits="MGroupAccessDelete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        

    <ext:Panel ID="Panel1" runat="server" Layout="BorderLayout" Flex="1">
        <Items>
            <ext:Panel ID="Panel6" runat="server" Border="true" Region="center" Flex="3" AnchorVertical="100%" Layout="FitLayout">
                <Items>
                   
                    <ext:GridPanel ID="GridpanelAdd" runat="server" Title="Title" AutoScroll="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                                <Items>

                                    <ext:DisplayField ID="txtGroupMenuName" runat="server" FieldLabel="Group Menu:" Text=""></ext:DisplayField>


                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <Store>
                            <ext:Store ID="StoreView" runat="server" OnReadData="Store_ReadData">
                               
                                <Sorters>
                                    <%--<ext:DataSorter Property="" Direction="ASC" />--%>
                                </Sorters>
                                <Model>
                                    <ext:Model runat="server" ID="modelgrid">
                                        <Fields>
                                             <ext:ModelField Name="PK_Module_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="PK_MGroupMenu_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="GroupMenuName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="ModuleLabel" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="BAdd" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BEdit" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BDelete" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BActivation" Type="String">
                                            <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BView" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BApproval" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BUpload" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
                                            </ext:ModelField>
                                            <ext:ModelField Name="BDetail" Type="String">
                                                <Convert Handler=" if (value === 'Y') { return 'Yes' } else if(value=='N') {return 'No'} else { return value};" />
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

                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>

                                <ext:Column ID="ColModuleLabel" runat="server" DataIndex="ModuleLabel" Flex="1" Text="Module"></ext:Column>
                                <ext:Column ID="ColBAdd" runat="server" DataIndex="BAdd" Flex="1" Text="Add"></ext:Column>
                                <ext:Column ID="ColBEdit" runat="server" DataIndex="BEdit" Flex="1" Text="Edit"></ext:Column>
                                <ext:Column ID="ColBDelete" runat="server" DataIndex="BDelete" Flex="1" Text="Delete"></ext:Column>
                                <ext:Column ID="ColBView" runat="server" DataIndex="BView" Flex="1" Text="View"></ext:Column>
                                  <ext:Column ID="ColBActivation" runat="server" DataIndex="BActivation" Flex="1" Text="Activation"></ext:Column>
                                <ext:Column ID="ColBApproval" runat="server" DataIndex="BApproval" Flex="1" Text="Approval"></ext:Column>
                                <ext:Column ID="ColBUpload" runat="server" DataIndex="BUpload" Flex="1" Text="Upload"></ext:Column>
                              

                            </Columns>
                        </ColumnModel>

                        <Buttons>
                            <ext:Button ID="btnSaveData" runat="server" Text="Delete All Group Menu Access">
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
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>

                    </ext:GridPanel>

                </Items>
            </ext:Panel>


            <ext:Panel ID="Panel2" runat="server" Layout="VBoxLayout" Region="East" Flex="1" Collapsible="true" Collapsed="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <Items>

                    <ext:TreePanel ID="TreePanelMenu" runat="server" Title="Menu" Flex="1" AutoScroll="true" SortableColumns="false" Mode="Remote" Hidden="true"
                        >
                        <SelectionSubmitConfig WithChildren="true" />
                        
                  

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
                        
                    </ext:TreePanel>

                   

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
