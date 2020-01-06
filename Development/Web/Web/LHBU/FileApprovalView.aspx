<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="FileApprovalView.aspx.vb" Inherits="LHBU_FileApprovalView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:Window ID="WindowReason" runat="server" Collapsible="true" Height="300" Icon="Application" Title="Confirmation" Width="340" Hidden="true" Layout="FitLayout" ClientIDMode="Static">
        <Items>
            <ext:FormPanel ID="FormReason" runat="server" Padding="5" Title="Confirm Reason" AnchorHorizontal="100%" Layout="FitLayout" Hidden="true">
                <Items>
                    <ext:TextArea ID="txtReason" runat="server" FieldLabel ="Reject Reason"></ext:TextArea>
                </Items>
                <Buttons>
                    <ext:Button ID="btnConfirmReject" runat="server" Text="OK" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnConfirmReject_DirectClick"></Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnCancelReject" runat="server" Text="Cancel" Icon="Cancel">
                        <DirectEvents>
                            <Click OnEvent="btnCancelReject_DirectClick"></Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.5});" />
            <Resize Handler="#{WindowReason}.center()" />
        </Listeners>
    </ext:Window>

            <ext:GridPanel ID="gridFileList" runat="server">
                <Store>
                    <ext:Store ID="storeFileList" runat="server" IsPagingStore="true">
                        <Model>
                            <ext:Model ID="modelFileList" runat="server">
                                <Fields>
                                    <ext:ModelField Name="PK_GeneratedFileList_ID" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="PeriodeLaporan" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="FK_LHBU_FormInfo_ID" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FileName" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FileType" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="FileBin" Type="Auto"></ext:ModelField>
                                    <ext:ModelField Name="StartDate" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="CompleteDate" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="ErrorMsg" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="Comment" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="GenerationStatus" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ApprovalStatus" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="CreatedBy" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="UpdatedBy" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="ApprovedBy" Type="String"></ext:ModelField>
                                    <ext:ModelField Name="CreatedDate" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="UpdatedDate" Type="Date"></ext:ModelField>
                                    <ext:ModelField Name="ApprovedDate" Type="Date"></ext:ModelField>
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                    <ext:DataSorter Property="StartDate" Direction="DESC" />
                </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:DateColumn ID="colTanggalData" runat="server" DataIndex="PeriodeLaporan" Text="Tanggal Data" Width="150px" Format="yyyy-MM-dd"></ext:DateColumn>
                        <ext:Column ID="colFK_LHBU_FormInfo_ID" runat="server" DataIndex="FK_LHBU_FormInfo_ID" Text="Kode Form" Width="150px"></ext:Column>
                        <ext:Column ID="colFileName" runat="server" DataIndex="FileName" Text="File Name" Width="150px"></ext:Column>
                        <ext:Column ID="colStartDate" runat="server" DataIndex="StartDate" Text="Start Progress Date" Width="150px"></ext:Column>
                        <ext:Column ID="colCompleteDate" runat="server" DataIndex="CompleteDate" Text="Complete Progress Date" Width="150px"></ext:Column>
                        <ext:Column ID="colApprovalStatus" runat="server" DataIndex="ApprovalStatus" Text="Approval Status" Width="150px"></ext:Column>
                        <ext:CommandColumn ID="commands" runat="server" Width="150px">
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
                                    <Confirmation Message="Are you sure you want to do this process?" ConfirmRequest="true"  Title="Process"  ></Confirmation>
                                </Command>
                            </DirectEvents>
                        </ext:CommandColumn>
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader id="GridHeader1" runat="server" Remote="false"/>
                </Plugins>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
                </BottomBar>
                <TopBar>
                    <ext:Toolbar runat="server" ID="toolbar1">
                        <Items>
                            <ext:Button runat="server" ID="btnApprove" Text="Approve All" AutoPostBack="true" OnClick="ApproveAll" />
                            <ext:Button runat="server" ID="btnReject" Text="Reject All" AutoPostBack="true" OnClick="RejectAll" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>

</asp:Content>

