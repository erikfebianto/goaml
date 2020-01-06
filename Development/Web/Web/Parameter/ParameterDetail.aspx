<%@ page title="" language="VB" masterpagefile="~/Site1.Master" autoeventwireup="false" inherits="ParameterDetail, App_Web_parameterdetail.aspx.252c98" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:TabPanel ID="PanelInfo" runat="server" ButtonAlign="Center">

        <Items>
            <ext:FormPanel ID="FormPanelInput" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="AnchorLayout" ButtonAlign="Center" AutoScroll="true">
            </ext:FormPanel>
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
                                <ext:DateColumn runat="server" Text="Created Date" DataIndex="CreatedDate" CellWrap="true" Width="150" Format="dd-MMM-yyyy " />
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
        <Buttons>

            <ext:Button ID="btnCancel" runat="server" Text="Back" Icon="PageBack">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click"></Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:TabPanel>

    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%">
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
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>