<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="FileSubmitApprovalView.aspx.vb" Inherits="LHBU_FileSubmitApprovalView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:Window ID="WindowReason" runat="server" Icon="Application" Title="Reject Confirmation" Hidden="true" Layout="FitLayout" ClientIDMode="Static" Modal="true">
        <Items>
            <ext:FormPanel ID="FormReason" runat="server" BodyPadding="15" AnchorHorizontal="100%" Layout="AnchorLayout">
                <Items>
                    <ext:TextArea ID="TxtReason" runat="server" FieldLabel ="Reject Reason" Height="100" AnchorHorizontal="100%" LabelWidth="115">
                    </ext:TextArea>
                </Items>
                <Buttons>
                    <ext:Button ID="BtnConfirmReject" runat="server" Text="OK" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="BtnConfirmReject_OnClicked">
                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelReject" runat="server" Text="Cancel" Icon="Cancel">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelReject_OnClicked"></Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.45});" />
            <Resize Handler="#{WindowReason}.center()" />
        </Listeners>
    </ext:Window>
    <ext:GridPanel ID="gridFileList" runat="server">
        <Store>
            <ext:Store ID="storeFileList" runat="server" RemoteFilter="false" IsPagingStore="true">
                <Model>
                    <ext:Model runat="server" IDProperty="PK_GeneratedFileList_ID">
                        <Fields>
                            <ext:ModelField Name="Request_ID" Type="Int"></ext:ModelField>
                            <ext:ModelField Name="PK_GeneratedFileList_ID" Type="Auto"></ext:ModelField>
                            <ext:ModelField Name="PeriodeLaporan" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="KodeCabang" Type="String"></ext:ModelField>
                            <ext:ModelField Name="FormInfo" Type="String"></ext:ModelField>
                            <ext:ModelField Name="FileName" Type="String"></ext:ModelField>
                            <ext:ModelField Name="FileType" Type="String"></ext:ModelField>
                            <ext:ModelField Name="FileBin" Type="Auto"></ext:ModelField>
                            <ext:ModelField Name="StartDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="CompleteDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="ErrorMsg" Type="String"></ext:ModelField>
                            <ext:ModelField Name="Comment" Type="String"></ext:ModelField>
                            <ext:ModelField Name="GenerationStatus" Type="String"></ext:ModelField>
                            <ext:ModelField Name="SubmitStatus" Type="String"></ext:ModelField>
                            <ext:ModelField Name="TotalData" Type="String"></ext:ModelField>
                            <ext:ModelField Name="GenerateFileTemplateName" Type="String"></ext:ModelField>
                            <%--<ext:ModelField Name="CreatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="UpdatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="ApprovedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="CreatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="UpdatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="ApprovedDate" Type="Date"></ext:ModelField>--%>
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="StartDate" Direction="DESC" />
                    <ext:DataSorter Property="FileName" Direction="ASC" />
                </Sorters>
            </ext:Store>
        </Store>
        <ColumnModel>
            <Columns>
                <ext:Column ID="colRequestID" runat="server" DataIndex="Request_ID" Text="Batch" Width="60px"></ext:Column>
                <ext:DateColumn ID="colTanggalData" runat="server" DataIndex="PeriodeLaporan" Text="Report Date" Width="100px"></ext:DateColumn>
                <ext:Column ID="colKodeCabang" runat="server" DataIndex="KodeCabang" Text="Cabang" Width="70px"></ext:Column>
                <ext:Column ID="colTemplateName" runat="server" DataIndex="GenerateFileTemplateName" Text="Template" Width="240px"></ext:Column>
                <ext:Column ID="colFileName" runat="server" DataIndex="FileName" Text="File Name" Width="240px"></ext:Column>
                <ext:Column ID="colTotalData" runat="server" DataIndex="TotalData" Text="Total Data" Width="80px" Align="Right"></ext:Column>
                <%--<ext:CommandColumn ID="colCommandAction" runat="server" Width="155px" Visible="false">
                    <Commands>
                        <ext:GridCommand CommandName="Approve" Icon="Accept" Text="Approve"></ext:GridCommand>
                        <ext:GridCommand CommandName="Reject" Icon="Cancel" Text="Reject"></ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="GridCommand">
                            <ExtraParams>
                                <ext:Parameter Name="unikkeyDP" Value="record.data.PK_GeneratedFileList_ID" Mode="Raw"></ext:Parameter>
                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                            </ExtraParams>
                            <Confirmation Message="Are you sure you want to do this process?" ConfirmRequest="true"  Title="Process"></Confirmation>
                            <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"></EventMask>
                        </Command>
                    </DirectEvents>
                </ext:CommandColumn>--%>
                <ext:DateColumn ID="colStartDate" runat="server" DataIndex="StartDate" Text="Start Progress Date" Width="150px"></ext:DateColumn>
                <ext:DateColumn ID="colCompleteDate" runat="server" DataIndex="CompleteDate" Text="Complete Progress Date" Width="150px"></ext:DateColumn>
                <%--<ext:Column ID="colApprovalStatus" runat="server" DataIndex="SubmitStatus" Text="Approval Status" Width="150px"></ext:Column>--%>
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:CheckboxSelectionModel ID="SelectionModel" runat="server" Mode="Multi" />
        </SelectionModel>
        <Plugins>
            <ext:FilterHeader id="GridHeader1" runat="server" Remote="false"/>
        </Plugins>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
        </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="toolbar1">
                <Items>
                    <ext:Button runat="server" ID="BtnApprove" Icon="Accept" Text="Approve" StyleSpec="cursor: pointer;">
                        <DirectEvents>
                            <Click OnEvent="BtnApprove_OnClicked">
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="BtnReject" Icon="Cancel" Text="Reject" StyleSpec="cursor: pointer;">
                        <DirectEvents>
                            <Click OnEvent="BtnReject_OnClicked">
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="200"> </EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:GridPanel>
</asp:Content>

