<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="ReportRWA2C.aspx.vb" Inherits="Report_ReportRWA2C" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true" Layout="AnchorLayout">
        <Items>
            <ext:Container ID="ContainerDocStatus" runat="server" Border="true" MarginSpec="0 0 10 10">
                <Items>
                    <ext:DateField ID="DatTanggalData" runat="server" FieldLabel="Tanggal Data"></ext:DateField>
                    <ext:Button runat="server" ID="BtnShow" Text="Show Report" AutoPostBack="true" OnClick="Generate">
                    </ext:Button>
                    <ext:DisplayField runat="server" ID="LblPeriode" FieldLabel=""></ext:DisplayField>

                    <ext:Panel runat="server" ID="PanelReport" Collapsible="true" AutoScroll="true" Hidden="false" Title="Report 1C" TitleAlign="Center">
                        <Content>
                            <asp:ScriptManager ID="ScriptManager1" runat="server" ViewStateMode="Enabled"></asp:ScriptManager>
                            <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" EnableViewState="True"></rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="300px" ShowRefreshButton="true" Width="100%">
                                <LocalReport ReportPath="Report\ReportLocal\Report1C.rdlc">
                                </LocalReport>
                            </rsweb:ReportViewer>
                        </Content>
                    </ext:Panel>
                </Items>
            </ext:Container>
        </Items>
    </ext:FormPanel>
</asp:Content>

