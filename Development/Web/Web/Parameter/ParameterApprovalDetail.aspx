<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="ParameterApprovalDetail, App_Web_parameterapprovaldetail.aspx.252c98" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <ext:Panel ID="container" runat="server" Layout="FormLayout" AutoScroll="true"  ButtonAlign="Center">

        <Items>
             <ext:FieldSet ID="FieldSet1" runat="server" Collapsible="true">
                        <Items>

            <ext:TabPanel ID="PanelInfo" runat="server"  >
                <Items>
                   
                                 <ext:Panel ID="Panel2" runat="server" Height="250"  Title="Approval"   >
                        <Items>
                            <ext:DisplayField ID="lblModuleName" runat="server" FieldLabel="Module Name">
                            </ext:DisplayField>
                            <ext:DisplayField ID="lblModuleKey" runat="server" FieldLabel="Module Key">
                            </ext:DisplayField>
                            <ext:DisplayField ID="lblAction" runat="server" FieldLabel="Action">
                            </ext:DisplayField>
                            <ext:DisplayField ID="LblCreatedBy" runat="server" FieldLabel="Created By">
                            </ext:DisplayField>
                            <ext:DisplayField ID="lblCreatedDate" runat="server" FieldLabel="Created Date">
                            </ext:DisplayField>
                        </Items>
                    </ext:Panel>
                  
               
                    <ext:Panel ID="PanelWorkflow" runat="server" Title="Workflow History" Collapsible="true">
                <Items>
                    <ext:GridPanel ID="GridPanelHistory" runat="server" Height="250">
                        <Store>
                            <ext:Store ID="StoreHistory" runat="server" IsPagingStore="true" PageSize="10">
                                <Model>
                                    <ext:Model runat="server" IDProperty="PK_MWorkflow_History_ID">
                                        <Fields>
                                            <ext:ModelField Name="PK_MWorkflow_History_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="FK_Module_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="FK_Unik_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="FK_MUserId" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="FK_MRoleId" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="RoleName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="UserName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="UserNameExecute" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="CreatedDate" Type="Date"></ext:ModelField>
                                            <ext:ModelField Name="ResponseDate" Type="Date"></ext:ModelField>
                                            <ext:ModelField Name="FK_MWorkflow_ApprovalStatus_ID" Type="int"></ext:ModelField>
                                            <ext:ModelField Name="ApprovalStatusName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="Notes" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <ColumnModel runat="server">
                            <Columns>
                                <ext:RowNumbererColumn runat="server" Text="No" CellWrap="true" Width="75"></ext:RowNumbererColumn>
                                 <ext:DateColumn runat="server" Text="Created Date" DataIndex="CreatedDate" CellWrap="true" Width="150" Format="dd-MMM-yyyy" />
                                <ext:Column runat="server" Text="User Type" DataIndex="RoleName" CellWrap="true" Width="150" />
                                <ext:Column runat="server" Text="Role Name" DataIndex="RoleName" CellWrap="true" Width="150" />
                                <ext:Column runat="server" Text="User Name" DataIndex="UserName" CellWrap="true" Width="150" />
                               <ext:Column runat="server" Text="User Name Executor" DataIndex="UserNameExecute" CellWrap="true" Width="150" />
                                <ext:DateColumn runat="server" Text="Response Date" DataIndex="ResponseDate" CellWrap="true" Width="150" Format="dd-MMM-yyyy" />
                                
                                <ext:Column runat="server" Text="Approval Status" DataIndex="ApprovalStatusName" CellWrap="true" Width="100" />
                                <ext:Column runat="server" Text="Notes" DataIndex="Notes" CellWrap="true" Width="200" />
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar13" runat="server" HideRefresh="True" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
                </Items>
            </ext:TabPanel>

                </Items>
                    </ext:FieldSet>

            <ext:Panel ID="Panel1" runat="server" Layout="HBoxLayout" ButtonAlign="Center" Flex="1" AutoScroll="true">

                <DockedItems>
                    <ext:Panel ID="FormPanel1" runat="server" Layout="HBoxLayout" Flex="1">
                        <Items>
                            <ext:Panel ID="PanelHeaderOld" runat="server" Layout="AnchorLayout" Title="Old Value" Flex="1">
                                <Items>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="PanelHeaderNew" runat="server" Layout="AnchorLayout" Title="New Value" Flex="1">
                                <Items>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </DockedItems>
                <Items>
                    <ext:FormPanel ID="FormPanelOld" runat="server" Title="" Flex="1">
                        <Items>
                        </Items>
                    </ext:FormPanel>

                    <ext:FormPanel ID="FormPanelNew" runat="server" Title="" Flex="1">
                        <Items>
                        </Items>
                    </ext:FormPanel>
                </Items>
                
            </ext:Panel>
        </Items>
        <Buttons>
            
                    <ext:Button ID="BtnSave" runat="server" Text="Approve" Icon="DiskBlack">
                        <DirectEvents>
                            <Click OnEvent="BtnSave_Click" >
                                <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnReject" runat="server" Text="Reject" Icon="Decline">
                        <DirectEvents>
                            <Click OnEvent="BtnReject_Click">
                                <EventMask ShowMask="true" Msg="Saving Reject Data..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
              <ext:Button ID="btnRevise" runat="server" Text="Revise" Icon="RewindGreen">
                        <DirectEvents>
                            <Click OnEvent="BtnRevise_Click">
                                <EventMask ShowMask="true" Msg="Saving Revise Data..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancel" runat="server" Text="Cancel" Icon="PageBack">
                        <DirectEvents>
                            <Click OnEvent="BtnCancel_Click">
                                <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
    </ext:Panel>
    <ext:FormPanel ID="Panelconfirmation" runat="server" ClientIDMode="Static" Title="Confirmation" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%" Hidden="true">
        <Defaults>
            <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
        </Defaults>
        <LayoutConfig>
            <ext:HBoxLayoutConfig Padding="5" Align="Middle" Pack="Center" />
        </LayoutConfig>
        <Items>
            <ext:Label ID="LblConfirmation" runat="server" Align="center" Cls="NawaLabel" Tex="aa">
            </ext:Label>
        </Items>

        <Buttons>

            <ext:Button ID="BtnConfirmation" runat="server" Text="OK" Icon="ApplicationGo">
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>