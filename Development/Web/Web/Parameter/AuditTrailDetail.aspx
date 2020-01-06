<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master"AutoEventWireup="false" CodeFile="AuditTrailDetail.aspx.vb" Inherits="AuditTrailDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Padding="5" Layout="FormLayout" AutoScroll="true">
        <Items>
            <ext:DisplayField ID="LblID" runat="server" FieldLabel="ID" DataIndex="PK_AuditTrail_ID" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>
            <ext:DisplayField ID="LblCreatedDate" runat="server" FieldLabel="Created Date" DataIndex="CreatedDate" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>
            <ext:DisplayField ID="LblCreatedBy" runat="server" FieldLabel="Created By" DataIndex="CreatedBy" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>

            <ext:DisplayField ID="LblApproveBy" runat="server" FieldLabel="Approve By" DataIndex="ApproveBy" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>
            <ext:DisplayField ID="LblModuleLabel" runat="server" FieldLabel="Module Label" DataIndex="ModuleLabel" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>

            <ext:DisplayField ID="lblModuleAction" runat="server" FieldLabel="Module Action" DataIndex="ModuleActionName" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>
            <ext:DisplayField ID="lblStatusAuditTrail" runat="server" FieldLabel="Audit Trail Status" DataIndex="AuditTrailStatus" LabelStyle="word-wrap: break-word">
            </ext:DisplayField>


        </Items>
        <Content>
            <ext:GridPanel ID="GridPanelDetail" runat="server" Title="AuditTrailDetail" AutoScroll="true">
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <Store>
                    <ext:Store ID="storedetail" runat="server" OnReadData="StoreDetail_readdata">
                        <Model>
                            <ext:Model runat="server" ID="ModelDetail">
                                <Fields>
                                    <ext:ModelField Name="PK_AuditTrailDetail_id" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="FK_AuditTrailHeader_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="FieldName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="OldValue" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="NewValue" Type="String"></ext:ModelField>

                                </Fields>
                            </ext:Model>
                        </Model>

                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                    </ext:Store>
                </Store>

                <ColumnModel>
                    <Columns>
                        <ext:RowNumbererColumn runat="server" Text="No."></ext:RowNumbererColumn>
                        <ext:Column runat="server" Text="ID" DataIndex="PK_AuditTrailDetail_id"></ext:Column>
                        <ext:Column ID="Column1" runat="server" Text="Field Name" DataIndex="FieldName"></ext:Column>
                        <ext:Column ID="Column2" runat="server" Text="Old Value" DataIndex="OldValue"></ext:Column>
                        <ext:Column ID="Column3" runat="server" Text="New Value" DataIndex="NewValue"></ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" />
                </BottomBar>
            </ext:GridPanel>
        </Content>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Icon="PageBack" Text="Back">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click">
                        <EventMask Msg="Loading..." MinDelay="500">
                        </EventMask>
                    </Click>

                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
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



