<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="FileDeliveryView.aspx.vb" Inherits="LHBU_FileDeliveryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        var prepareToolbar = function (grid, toolbar, rowIndex, record) {
            //Hide button download jik abelum di approve
            /*if (record.get("ApprovalStatus") != 'APPROVED') {
                //hide Download button
                toolbar.items.getAt(0).hide();
            }*/

            /*if (record.get("ErrorMsg") != '' && record.get("ErrorMsg") != null) {
                toolbar.items.getAt(0).hide();
            }*/
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:GridPanel ID="GridFileList" runat="server">
        <Store>
            <ext:Store ID="StoreFileList" runat="server" RemoteFilter="false" IsPagingStore="true">
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
                            <ext:ModelField Name="StartDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="CompleteDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="ErrorMsg" Type="String"></ext:ModelField>
                            <ext:ModelField Name="Comment" Type="String"></ext:ModelField>
                            <ext:ModelField Name="GenerationStatus" Type="String"></ext:ModelField>
                            <ext:ModelField Name="ApprovalStatus" Type="String"></ext:ModelField>
                            <%--<ext:ModelField Name="CreatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="UpdatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="ApprovedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="CreatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="UpdatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="ApprovedDate" Type="Date"></ext:ModelField>--%>
                            <ext:ModelField Name="SubmitStatus" Type="String"></ext:ModelField>
                            <ext:ModelField Name="SubmitNote" Type="String"></ext:ModelField>
                            <ext:ModelField Name="TotalData" Type="String"></ext:ModelField>
                            <ext:ModelField Name="GenerateFileTemplateName" Type="String"></ext:ModelField>

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
                <ext:CommandColumn ID="colCommandDownload" runat="server" Width="95" Text="Action">
                    <Commands>
                        <ext:GridCommand CommandName="Download" Icon="DiskDownload" Text="Download"></ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="GridCommand">
                            <ExtraParams>
                                <ext:Parameter Name="unikkeyDP" Value="record.data.PK_GeneratedFileList_ID" Mode="Raw"></ext:Parameter>
                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                    <PrepareToolbar Fn="prepareToolbar" />
                </ext:CommandColumn>
                <ext:Column ID="colRequestID" runat="server" DataIndex="Request_ID" Text="Batch" Width="60px"></ext:Column>
                <ext:DateColumn ID="colPeriodeLaporan" runat="server" DataIndex="PeriodeLaporan" Text="Report Date" Width="100px"></ext:DateColumn>
                <ext:Column ID="colKodeCabang" runat="server" DataIndex="KodeCabang" Text="Cabang" Width="70px"></ext:Column>
                <ext:Column ID="colTemplateName" runat="server" DataIndex="GenerateFileTemplateName" Text="Template" Width="180px"></ext:Column>
                <%--<ext:Column ID="colFormInfoID" runat="server" DataIndex="FormInfo" Text="Kode Informasi" Width="120px"></ext:Column>--%>
                <ext:Column ID="colFileName" runat="server" DataIndex="FileName" Text="File Name" Width="240px"></ext:Column>
                <ext:Column ID="colTotalData" runat="server" DataIndex="TotalData" Text="Total Data" Width="80px" Align="Right"></ext:Column>
                <ext:Column ID="colSubmitStatus" runat="server" DataIndex="SubmitStatus" Text="Submit Status" Width="115px"></ext:Column>
                <ext:DateColumn ID="colStartDate" runat="server" DataIndex="StartDate" Text="Start Progress Date" Width="140px"></ext:DateColumn>
                <ext:DateColumn ID="colCompleteDate" runat="server" DataIndex="CompleteDate" Text="Complete Progress Date" Width="140px"></ext:DateColumn>
                <%--<ext:Column ID="colApprovalStatus" runat="server" DataIndex="ApprovalStatus" Text="Download Status" Width="125px"></ext:Column>
                <ext:Column ID="colComment" runat="server" DataIndex="Comment" Text="Download Reject Note" Width="120px"></ext:Column>--%>
                <ext:Column ID="colSubmitNote" runat="server" DataIndex="SubmitNote" Text="Rejected Note" Width="120px"></ext:Column>
            </Columns>
        </ColumnModel>
        <SelectionModel>
            <ext:CheckboxSelectionModel ID="SelectionModel" runat="server" Mode="Multi" />
        </SelectionModel>
        <Plugins>
            <ext:FilterHeader ID="GridHeader1" runat="server" Remote="false" ClientIDMode="Static" />
        </Plugins>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
        </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="toolbar1">
                <Items>
                    <%--<ext:Button ID="BtnRequestDownload" runat="server" Icon="DiskDownload" Text="Request Download">
                        <DirectEvents>
                            <Click OnEvent="RequestDownload">
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>--%>
                    <ext:Button  ID="BtnDownloadZip" runat="server" Icon="DiskDownload" Text="Download as Zip File" StyleSpec="cursor: pointer;" AutoPostBack="true" OnClick="BtnDownloadZip_OnClicked">
                    </ext:Button>
                    <ext:Button ID="BtnRequestSubmit" runat="server" Icon="ApplicationGo" Text="Request Submitted" StyleSpec="cursor: pointer;">
                        <DirectEvents>
                            <Click OnEvent="BtnRequestSubmit_OnClicked">
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{gridFileList}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="500"> </EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:GridPanel>
</asp:Content>

