<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="FileGenerationListView.aspx.vb" Inherits="LHBU_FileGenerationListView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta http-equiv="refresh" content="10">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:GridPanel ID="gridFileList" runat="server" Title="">
        <Store>
            <ext:Store ID="storeFileList" runat="server" ClientIDMode="Static" RemoteFilter="false" RemoteSort="false" >
                <Model>
                    <ext:Model ID="modelFileList" runat="server">
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
                            <ext:ModelField Name="TotalData" Type="String"></ext:ModelField>
                            <ext:ModelField Name="ApprovalStatus" Type="String"></ext:ModelField>
                            <%-- <ext:ModelField Name="CreatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="UpdatedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="ApprovedBy" Type="String"></ext:ModelField>
                            <ext:ModelField Name="CreatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="UpdatedDate" Type="Date"></ext:ModelField>
                            <ext:ModelField Name="ApprovedDate" Type="Date"></ext:ModelField>--%>
                            <ext:ModelField Name="SubmitStatus" Type="String"></ext:ModelField>
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
                <ext:DateColumn ID="colPeriodeLaporan" runat="server" DataIndex="PeriodeLaporan" Text="Report Date" Width="100px"></ext:DateColumn>
                <ext:Column ID="colKodeCabang" runat="server" DataIndex="KodeCabang" Text="Cabang" Width="75px"></ext:Column>
                <ext:Column ID="colFormInfoID" runat="server" DataIndex="FormInfo" Text="Kode Informasi" Width="160px"></ext:Column>
                <ext:Column ID="colFileName" runat="server" DataIndex="FileName" Text="File Name" Flex="1"></ext:Column>
                <ext:DateColumn ID="colStartDate" runat="server" DataIndex="StartDate" Text="Start Progress Date" Width="145px"></ext:DateColumn>
                <ext:DateColumn ID="colCompleteDate" runat="server" DataIndex="CompleteDate" Text="Complete Progress Date" Width="150px"></ext:DateColumn>
                <ext:Column ID="colGenerationStatus" runat="server" DataIndex="GenerationStatus" Text="Status" Width="65px"></ext:Column>
                <ext:Column ID="colTotalData" runat="server" DataIndex="TotalData" Text="Total Data" Width="95px" Align="Right"></ext:Column>
                <%--<ext:Column ID="colErrorMsg" runat="server" DataIndex="ErrorMsg" Text="Error Message" Width="150px"></ext:Column>--%>
            </Columns>
        </ColumnModel>
        <Plugins>
            <ext:FilterHeader id="GridHeader1" runat="server" Remote="false"/>
        </Plugins>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar7" runat="server" HideRefresh="True" />
        </BottomBar>
    </ext:GridPanel>
</asp:Content>

