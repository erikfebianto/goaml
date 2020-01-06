<%@ page title="" language="VB" masterpagefile="~/Site1.Master" autoeventwireup="false" inherits="ReportQueryEdit, App_Web_reportqueryedit.aspx.252c98" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">
         function OpenQuery() {

             var x = window.open("buttonQuery.aspx", "popupquery", "width=1000,height=600,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
             x.focus();


         }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true">
        <Items>

            <ext:DisplayField ID="txtID" runat="server" FieldLabel="ID" Text="AutoNumber">
            </ext:DisplayField>
            <ext:TextField ID="txtReportName" runat="server" FieldLabel="Report Name" AllowBlank="false" AnchorHorizontal="50%" MaskRe="/[A-Za-z0-9]/">
            </ext:TextField>
            <ext:TextField ID="txtReportDescription" runat="server" FieldLabel="Report Description" AnchorHorizontal="80%">
            </ext:TextField>
            <ext:TextArea ID="txtquery" ClientIDMode="Static" runat="server" AllowBlank="false" AnchorHorizontal="80%" AnchorVertical="80%" FieldLabel="Query" AutoScroll="True" Resizable="True">
            </ext:TextArea>
                  <ext:Checkbox ID="chkIsUseDesigner" runat="server" BoxLabel="Is Use Designer" FieldLabel="Designer">
                <DirectEvents>
                    <Change OnEvent="chkIsUseDesigner_DirectEvent">
                         <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                    </Change>
                </DirectEvents>
            </ext:Checkbox>
          
            <ext:Hidden ID="hQueryObjectDesigner" runat="server" ClientIDMode="Static">
            </ext:Hidden>
            <ext:Button ID="btnDesigner" runat="server" Text="Use Visual Quary Designer" ValidationGroup="MainForm" Hidden="true">
               
                <Listeners>
                    <Click Handler="OpenQuery()"></Click>
                </Listeners>
            </ext:Button>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Report">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
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
                    <Click OnEvent="BtnConfirmation_DirectClick"></Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
</asp:Content>


