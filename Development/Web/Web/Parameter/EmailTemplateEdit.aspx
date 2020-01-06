<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="EmailTemplateEdit.aspx.vb" Inherits="EmailTemplateEdit" validaterequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        
          String.format = function (b) {
            var a = arguments;
            return b.replace(/(\{\{\d\}\}|\{\d\})/g, function (b) {
                if (b.substring(0, 2) == "{{") return b;
                var c = parseInt(b.match(/\d/)[0]);
                return a[c + 1]
            })
        };

        function GridClick() {

            
            
            record = window.App.GridPaneltask.getSelectionModel().selected.items[0];
            window.App.CboMonitoringDuration.setValue(record.get(window.App.GridPaneltask.columns[0].dataIndex) + '-' + record.get(window.App.GridPaneltask.columns[1].dataIndex));
            window.App.GridPaneltask.getSelectionModel().clearSelections()
            window.App.Windowtask.hide();


        }

        function OpenQuery1() {

            var x = window.open("buttonQuery.aspx?object=1", "popupquery", "width=1000,height=600,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
            x.focus();


        }


        function OpenQuery() {

            var x = window.open("buttonQuery.aspx", "popupquery", "width=1000,height=600,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
            x.focus();


        }
          var UpdateUploadInfo = function (el, maxsize, strsizemax) {

            var ret = true;
            var size = 0;
            if (Ext.isIE) {

                var myFSO = new ActiveXObject("Scripting.FileSystemObject");
                var filepath = el.files[0].value;
                var thefile = myFSO.getFile(filepath);
                size = thefile.size;

                if (size > maxsize) {
                    txt = String.format('You are trying to upload {0}. Max. allowed upload size is ' + strsizemax, fileSize);
                    alert(txt);
                    ret = false;
                } else {
                    txt = String.format('{0} file(s) of total size {1}', el.files.length, fileSize);
                    ret = true;
                }

                return ret;
            }

            var names = '';
            for (var num1 = 0; num1 < el.files.length; num1++) {

                var file = el.files[num1];
                names += file.name + '\r\n';
                //alert(file.name+" "+file.type+" "+file.size);
                size += file.size;
            }
            var txt = '';
            var fileSize = Ext.util.Format.fileSize(size);

            if (size > maxsize) {
                txt = String.format('You are trying to upload {0}. Max. allowed upload size is ' + strsizemax, fileSize);
                alert(txt);
                ret = false;
            } else {
                txt = String.format('{0} file(s) of total size {1}', el.files.length, fileSize);
                ret = true;
            }

            return ret;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Window ID="WindowAdditional" runat="server" Icon="ApplicationViewDetail" Title="Table Reference" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormPanelAdditional" runat="server" Padding="5" Title="Form" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>

                    <ext:TextField ID="txtTableName" runat="server" FieldLabel="Table Name" AnchorHorizontal="100%" AllowBlank="false" EmptyText="Please Fill Table Name Alias">
                    </ext:TextField>
                    <ext:ComboBox ID="cboTableType" runat="server" FieldLabel="Table Type" AllowBlank="false" EmptyText="Please Select Table Type" DisplayField="EmailTableTypeName" ValueField="PK_EmailTableType_ID" ForceSelection="true"  AnchorHorizontal="100%">
                        <Store>
                            <ext:Store runat="server" ID ="StoreTableType">
                                 <Model>
                                    <ext:Model ID="Model2" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailTableType_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailTableTypeName" Type="String"></ext:ModelField>
                                         </Fields>
                                        </ext:Model>
                                   </Model>
                            </ext:Store>
                        </Store>
                        <DirectEvents>
                            <Change OnEvent="cboTableType_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                            </Change>
                        </DirectEvents>
                    </ext:ComboBox>


                    <ext:TextArea ID="txtquery" runat="server" FieldLabel="Query Data" AllowBlank="false" EmptyText="Please Fill QueryData. Use @ID for Field unik Name from primary table." Height="150" AutoScroll="true" AnchorHorizontal="90%" ClientIDMode="Static">
                        <RightButtons>
                            <ext:Button ID="btnDesigner" runat="server" Text="Use Visual Quary Designer" ValidationGroup="MainForm">
                                <Listeners>
                                    <Click Handler="OpenQuery()"></Click>
                                </Listeners>
                            </ext:Button>
                        </RightButtons>
                    </ext:TextArea>
                    <ext:Hidden ID="hQueryObjectDesigner" runat="server" ClientIDMode="Static">
                    </ext:Hidden>
                    <ext:TextField ID="txtFieldUnik" runat="server" FieldLabel="Field Unik Name" AnchorHorizontal="80%" EmptyText="Please Fill Field Unik Name" Hidden="true">
                    </ext:TextField>

                </Items>
                <Buttons>
                    <ext:Button ID="btnsaveAdditional" runat="server" Icon="Disk" Text="Save Table Reference">
                        <Listeners>
                            <Click Handler="if (!#{FormPanelAdditional}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="btnsaveAdditional_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btncancelAddtional" runat="server" Icon="Cancel" Text="Cancel Table Reference">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelAdditional_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.8});" />

            <Resize Handler="#{WindowDetail}.center()" />
        </Listeners>
    </ext:Window>

    <ext:Window ID="WindowEmailAction" runat="server" ButtonAlign="Center" Collapsible="true" Height="185" Icon="Application" Title="Title" Width="350" Hidden="true">
        <Items>

            <ext:FormPanel ID="FormPanelEmailAction" runat="server" Padding="5" Layout="AnchorLayout">
                <Items>
                    <ext:ComboBox ID="cboEmailAction" runat="server" FieldLabel="Email Action Type" AllowBlank="false" EmptyText="Please Select Email Action Type" DisplayField="EmailActionTypeName" ValueField="PK_EmailActionType_Id" ForceSelection="true">
                        <Store>
                            <ext:Store runat="server" ID="StoreEmailActionType">
                                <Model>
                                    <ext:Model ID="ModelEmailActionType" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailActionType_Id" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailActionTypeName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:TextArea Height="200" ID="TxtsqlToExecute" runat="server" AnchorHorizontal="100%" FieldLabel="Statement to Execute" AllowBlank="false" EmptyText="Use @ID for Field Unik Name from primary table" >
                    </ext:TextArea>
                </Items>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.8, height: size.height * 0.8});" />
            <Resize Handler="#{WindowEmailAction}.center()" />
        </Listeners>
        <Buttons>
            <ext:Button ID="btnSubmitAction" runat="server" Icon="Disk" Text="Save">
                <Listeners>
                    <Click Handler="if (!#{FormPanelEmailAction}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="btnSubmitAction_DirectEvent">
                        <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancelAction" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancelAction_Click">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>

    <ext:Window ID="WindowEmailAttachment" runat="server" ButtonAlign="Center" Collapsible="true" Height="185" Icon="Application" Title="Attachment" Width="350" Hidden="true">
        <Items>

            <ext:FormPanel ID="FormPanelEmailAttachment" runat="server" Padding="5" Layout="AnchorLayout">
                <Items>
                    <ext:ComboBox ID="ComboBoxEmailAttachmentType" runat="server" FieldLabel="Email Attachment Type" AllowBlank="false" EmptyText="Please Select Email Attachment Type" DisplayField="EmailAttachmentType1" ValueField="PK_EmailAttachmentType_ID" ForceSelection="true">
                        <Store>
                            <ext:Store runat="server" ID="StorecboEmailAttachmentType">
                                <Model>
                                    <ext:Model ID="ModelAttachmentType" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailAttachmentType_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailAttachmentType1" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>                           
                        </Store>
                        <DirectEvents>
                            <Change OnEvent="cboEmailAttachmentType_change"></Change>
                        </DirectEvents>
                    </ext:ComboBox>
                    <ext:TextField ID="TxtReportName" runat="server" FieldLabel="Report Name" AnchorHorizontal="80%" hidden="true" EmptyText="Please Fill Report Name">
                    </ext:TextField>
                       <ext:ComboBox ID="cboRenderAs" runat="server" FieldLabel="Render As" EmptyText="Please Select RenderAs" DisplayField="EmailRenderAsName" ValueField="PK_EmailRenderAs_ID" ForceSelection="true" Hidden="true">
                        <Store>
                            <ext:Store runat="server" ID="StoreRenderAs">
                                <Model>
                                    <ext:Model ID="ModelRenderAs" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailRenderAs_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailRenderAsName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>                           
                        </Store>
                       
                    </ext:ComboBox>
                    <ext:TextField ID="TxtReportParameter" runat="server" FieldLabel="Report Parameter" AnchorHorizontal="80%"  EmptyText="sample PK_PAM_ID=$PK_PAM_ID$" Hidden="true">
                    </ext:TextField>
                    <ext:FileUploadField ID="FileReport" runat="server" FieldLabel="File Report" AnchorHorizontal="100%" Hidden="true" >
                        
                        <RightButtons>
                            <ext:Button ID="btnClear" runat="server" Text="" Icon="Erase" ClientIDMode="Static">
                                <Listeners>
                                    <Click Handler="#{FileReport}.reset();#{LblFileReport}.setValue(#{FileReport}.value);"></Click>
                                </Listeners>
                            </ext:Button>
                        </RightButtons>
                    </ext:FileUploadField>
                    <ext:DisplayField ID="LblFileReport" runat="server" ClientIDMode="Static" FieldLabel="FileName" Hidden="true">
                    </ext:DisplayField>
                       <ext:TextField ID="txtFileName" runat="server" FieldLabel="File Name" AnchorHorizontal="80%"  EmptyText="Input File Name" Hidden="true">
                    </ext:TextField>
                </Items>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.7, height: size.height * 0.7});" />
            <Resize Handler="#{WindowEmailAttachment}.center()" />
        </Listeners>
        <Buttons>
            <ext:Button ID="Button3" runat="server" Icon="Disk" Text="Save">
                <Listeners>
                    <Click Handler="if (!#{FormPanelEmailAttachment}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="btnSubmitAttachment_DirectEvent">
                        <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="Button4" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancelAttachment_Click">
                        <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>

    <ext:Window ID="WindowDetail" runat="server" Icon="ApplicationViewDetail" Title="Replacer" Hidden="true" Layout="FitLayout" Modal="true">
        <Items>
            <ext:FormPanel ID="FormSchedulerDetail" runat="server" Padding="5" Title="Form" AnchorHorizontal="100%" Hidden="true" AutoScroll="true">
                <Items>
                    <ext:TextField ID="txtRepalcer" runat="server" FieldLabel="Replacer" AnchorHorizontal="100%" AllowBlank="false" EmptyText="Please Fill Replacer using $ReplacerItem$">
                    </ext:TextField>

                    <ext:ComboBox ID="cboField" runat="server" FieldLabel="Primary and Addtional Field" QueryMode="Local" TriggerAction="All" AnchorHorizontal="100%" AllowBlank="false" EmptyText="Select Field" >
                         <Listeners>
                            <Expand Handler="this.picker.setWidth(500);" />
                        </Listeners>
                        <Items>

                        </Items>
                    </ext:ComboBox>


                    <%--  <ext:TextArea ID="txtquery" runat="server" FieldLabel="Query Replacer" AllowBlank="false" EmptyText="Please Fill QueryReplacer" Height="150" AutoScroll="true" AnchorHorizontal="90%" ClientIDMode="Static">
                    </ext:TextArea>
                    <ext:Hidden ID="hQueryObjectDesigner" runat="server" ClientIDMode="Static">
                    </ext:Hidden>
                    <ext:Button ID="btnDesigner" runat="server" Text="Use Visual Quary Designer" ValidationGroup="MainForm">

                        <Listeners>
                            <Click Handler="OpenQuery()"></Click>
                        </Listeners>
                    </ext:Button>--%>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSaveReplacer" runat="server" Icon="Disk" Text="Save Replacer">
                        <Listeners>
                            <Click Handler="if (!#{FormSchedulerDetail}.getForm().isValid()) return false;"></Click>
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="btnSaveReplacer_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelReplacer" runat="server" Icon="Cancel" Text="Cancel Replacer">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelReplacer_DirectEvent">
                                <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
        <Listeners>
            <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.5, height: size.height * 0.5});" />

            <Resize Handler="#{WindowDetail}.center()" />
        </Listeners>

    </ext:Window>



    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true">
        <Items>
            <ext:TextField ID="txtEmailTemplate" runat="server" FieldLabel="Email Template Name" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please Fill Email Template Name">
            </ext:TextField>


           


            <ext:FieldSet ID="FieldSet4" runat="server" Title="Table Reference" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:GridPanel ID="GridPanelAdditional" runat="server" Title="Table Reference">

                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                                <Items>
                                    <ext:Button ID="btnAdditional" runat="server" Text="Add New Table Reference">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddAdditionalTable_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>

                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreAdditional" runat="server">
                                <Model>
                                    <ext:Model ID="Model1" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailTemplateAdditional_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="QueryData" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="QueryDataDesigner" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="NamaTable" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldUnikPrimaryTable" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailTableType_ID" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="EmailTableTypeName" Type="String"></ext:ModelField>
                                            

                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:Column ID="Column2" runat="server" DataIndex="NamaTable" Text="NamaTable" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column4" runat="server" DataIndex="EmailTableTypeName" Text="Table Type" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column1" runat="server" DataIndex="QueryData" Text="QueryData" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column3" runat="server" DataIndex="FieldUnikPrimaryTable" Text="FieldUnikPrimaryTable" Flex="1">
                                </ext:Column>

                                <ext:CommandColumn ID="CommandColumn2" runat="server" Flex="1" Width="450">
                                    <Commands>
                                        <%--<ext:GridCommand CommandName="Download" Icon="DiskDownload"  ></ext:GridCommand>--%>
                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>

                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommandAdditional">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EmailTemplateAdditional_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>

                                        </Command>
                                    </DirectEvents>

                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>


            <ext:FieldSet ID="FieldSet2" runat="server" Title="Email" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:TextField ID="txtEmailTo" runat="server" FieldLabel="Email To" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please fill Email To">
                    </ext:TextField>
                    <ext:TextField ID="txtEmailCC" runat="server" FieldLabel="Email CC" AnchorHorizontal="80%">
                    </ext:TextField>
                    <ext:TextField ID="txtEmailBCC" runat="server" FieldLabel="Email BCC" AnchorHorizontal="80%">
                    </ext:TextField>
                    <ext:TextField ID="txtEmailSubject" runat="server" FieldLabel="Email Subject" AnchorHorizontal="80%" AllowBlank="false" EmptyText="Please fill Email Subject">
                    </ext:TextField>
                    <ext:HtmlEditor ID="txtBody" runat="server" FieldLabel="EmailBody" AllowBlank="false" EmptyText="Please Fill Email Body" Height="200" AutoScroll="true" >
                    </ext:HtmlEditor>

                  <%--  <ext:TextArea ID="txtBody" runat="server" FieldLabel="EmailBody" AllowBlank="false" EmptyText="Please Fill Email Body" Height="200" AutoScroll="true">
                    </ext:TextArea>--%>
                    <ext:GridPanel ID="gridReplacer" runat="server" Title="Replacer">
                        <TopBar>
                            <ext:Toolbar ID="toolbar" runat="server">
                                <Items>
                                    <ext:Button ID="btnAddNew" runat="server" Text="Add new Replacer Detail">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddNew_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>

                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreEmailDetail" runat="server">
                                <Model>
                                    <ext:Model ID="ModelDetail" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailTemplateDetail_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="Replacer" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FieldReplacer" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumberTaskDetail" runat="server" Text="No"></ext:RowNumbererColumn>

                                <ext:Column ID="ColReplacerID" runat="server" DataIndex="Replacer" Text="Replacer" Flex="1">
                                </ext:Column>
                                <ext:Column ID="colQueryReplacer" runat="server" DataIndex="FieldReplacer" Text="Field Replacer" Flex="1">
                                </ext:Column>


                                <ext:CommandColumn ID="CommandColumn1" runat="server" Flex="1" Width="450">
                                    <Commands>
                                        <%--<ext:GridCommand CommandName="Download" Icon="DiskDownload"  ></ext:GridCommand>--%>
                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>

                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommand">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EmailTemplateDetail_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>
                                        </Command>
                                    </DirectEvents>
                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet5" runat="server" Title="Email Action" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:GridPanel ID="GridPanelEmailAction" runat="server" Title="Email Action">
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true" />
                        </View>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server" EnableOverflow="true">
                                <Items>
                                    <ext:Button ID="Button1" runat="server" Text="Add New Email Action">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddEmailAction_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreEmailTemplateAction" runat="server" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="Model3" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailTemplate_Action_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailActionType_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailActionTypeName" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="TSQLtoExecute" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:Column ID="Column5" runat="server" DataIndex="EmailActionTypeName" Text="Email Action Type" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column6" runat="server" DataIndex="TSQLtoExecute" Text="Statement To Execute" Flex="1">
                                </ext:Column>

                                <ext:CommandColumn ID="CommandColumn3" runat="server" Flex="1" Width="450">
                                    <Commands>

                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>
                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommandEmailAction">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EmailTemplate_Action_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>
                                        </Command>
                                    </DirectEvents>
                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>

            <ext:FieldSet ID="FieldSet6" runat="server" Title="Email Attachment" Collapsible="true" DefaultAnchor="100%">
                <Items>

                    <ext:GridPanel ID="GridPanelEmailAttachment" runat="server" Title="Email Attachment">
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true" />
                        </View>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server" EnableOverflow="true">
                                <Items>
                                    <ext:Button ID="Button2" runat="server" Text="Add New Email Attachment">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddEmailAttachment_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreEmailAttachment" runat="server" IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="ModelEmailAttachment" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EmailTemplateAttachment_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailTemplate_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailAttachmentType_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EmailAttachmentType" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="NamaReport" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="ParameterReport" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="NamaFile" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="FK_EmailRenderAs_Id" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="EmailRenderAsName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn3" runat="server" Text="No"></ext:RowNumbererColumn>
                                <ext:Column ID="Column7" runat="server" DataIndex="EmailAttachmentType" Text="Email Attachment Type" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column8" runat="server" DataIndex="NamaReport" Text="Report Name" Flex="1">
                                </ext:Column>

                                <ext:Column ID="Column9" runat="server" DataIndex="ParameterReport" Text="Report Parameter" Flex="1">
                                </ext:Column>
                                <ext:Column ID="Column10" runat="server" DataIndex="NamaFile" Text="Report File Name" Flex="1">
                                    <Commands>
                                        <ext:ImageCommand Icon="DiskDownload" CommandName="Download" ToolTip-Text="Download Report File"></ext:ImageCommand>
                                    </Commands>

                                    <DirectEvents>
                                        <Command OnEvent="GridEmailAttachmentCommand">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EmailTemplateAttachment_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                        </Command>
                                    </DirectEvents>
                                </ext:Column>
                                <ext:Column ID="Column11" runat="server" DataIndex="EmailRenderAsName" Text="Render As" Flex="1"></ext:Column>
                                <ext:CommandColumn ID="CommandColumnEmailAction" runat="server" Flex="1" Width="450">
                                    <Commands>

                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit"></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete"></ext:GridCommand>
                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridEmailAttachmentCommand">
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EmailTemplateAttachment_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true" Title="Delete">
                                            </Confirmation>
                                        </Command>
                                    </DirectEvents>
                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:FieldSet>

            <ext:FieldSet ID="FieldSet3" runat="server" Title="Scheduler" Collapsible="true" DefaultAnchor="100%">
                <Items>
                    <ext:DropDownField ID="CboMonitoringDuration" runat="server" Editable="false" TriggerIcon="SimpleArrowDown" FieldLabel="Monitoring Duration" AnchorHorizontal="40%" AllowBlank="false" ClientIDMode="Static" BlankText="Please Select Monitoring Duration Type">
                        <Listeners>
                            <Expand Handler="this.picker.setWidth(500);" />

                        </Listeners>
                        <Component>
                            <ext:Window ID="Windowtask" runat="server" Collapsible="true" Height="300" Icon="Application" Title="Monitoring Duration Picker" Width="340" Layout="FitLayout" ClientIDMode="Static">
                                <Items>
                                    <ext:GridPanel ID="GridPaneltask" runat="server" ClientIDMode="Static">
                                        <Store>
                                            <ext:Store ID="storepicker" runat="server" IsPagingStore="true" RemoteFilter="true" RemoteSort="true" OnReadData="Storetrigger_Readdata" RemotePaging="true">
                                                <Sorters>
                                                </Sorters>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                            </ext:Store>
                                        </Store>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" HideRefresh="True" />
                                        </BottomBar>
                                    </ext:GridPanel>

                                </Items>
                            </ext:Window>
                        </Component>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Change Handler="this.getTrigger(0).show();NawadataDirect.CekMonitoringStatus()" />
                            <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:DropDownField>
                    <ext:FieldSet ID="FieldSet1" runat="server" Title="Detail" Collapsible="true" DefaultAnchor="100%" Hidden="true">
                        <Items>
                            <ext:Checkbox ID="chkExcludeHoliday" runat="server" FieldLabel="Exclude Holiday">
                            </ext:Checkbox>
                            <ext:DateField ID="startDate" runat="server" FieldLabel="Start Date" AnchorHorizontal="40%">
                            </ext:DateField>
                            <ext:TimeField ID="StartTime" runat="server" FieldLabel="Start Time" AnchorHorizontal="40%" Format="HH:mm:ss"></ext:TimeField>
                        </Items>
                    </ext:FieldSet>
                </Items>
            </ext:FieldSet>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Task">
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
