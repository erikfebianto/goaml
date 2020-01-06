<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false"  CodeFile="ReportingServiceSatuanView.aspx.vb" Inherits="ReportingServiceSatuanView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

    <ext:FormPanel ID="FormPanel1" runat="server" Padding="5" AutoScroll="true" Layout="FormLayout">

        <Items>
            <ext:Container ID="Container1" runat="server" Layout="FormLayout" BodyPadding="5" AnchorHorizontal="70%">

                <Items>
                    
                    <ext:Hidden runat="server" ID="hreport">
                        </ext:Hidden>
                    <%--<ext:ComboBox ID="cboReport" runat="server" FieldLabel="Report" AllowBlank="false" Width="300"></ext:ComboBox>--%>

                    <%--<ext:Button ID="btnviewReport" runat="server" Text="View Report" AutoPostBack="true">
                    </ext:Button>--%>
                </Items>
            </ext:Container>
        </Items>
        <Content>

            <asp:ScriptManager ID="ScriptManager1" runat="server" ViewStateMode="Enabled"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" EnableViewState="True"></rsweb:ReportViewer>
        </Content>
        <Buttons>
        </Buttons>
    </ext:FormPanel>
</asp:Content>