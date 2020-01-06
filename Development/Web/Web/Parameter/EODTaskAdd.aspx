<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="EODTaskAdd.aspx.vb" Inherits="EODTaskAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">  
        var LoadType = function (value) {
            switch (value) {
                case 1:
                    return "SSIS";
                    break;
                case 2:
                    return "Store Procedure";
                    break;
                case 3:
                    return "SSIS in SQL Agent";
                    break;
                case 4:
                    return "API";
                    break;
            }
        };
    </script>

    <style>
        .useparam-label .x-form-item-label-seperator {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:Window ID="WindowDetail" runat="server" Icon="ApplicationViewDetail" Title="Task Detail" Hidden="true" Layout="FitLayout" Modal="true" MinHeight="150">
        <Items>
            <ext:FormPanel ID="FormPanelTaskDetail" runat="server" AnchorHorizontal="100%" BodyPadding="10" Hidden="true" DefaultAlign="center" AutoScroll="true">
                <Items>
                    <ext:ComboBox ID="CboTaskDetailType" runat="server" DisplayField="EODTaskDetailType"  ValueField="PK_EODTaskDetailType_ID" ForceSelection="True" AllowBlank="False" BlankText="Task Detail Type Is Required" FieldLabel="Task Detail Type" AnchorHorizontal="85%">
                        <Store>
                            <ext:Store ID="StoreDetailType" runat="server"  OnReadData="CboTaskDetailType_ReadData" >
                                <Model>
                                    <ext:Model ID="Modeltaskdetail" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EODTaskDetailType_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="EODTaskDetailType" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <Listeners>
                            <Expand Handler="this.picker.setWidth(500);" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:FieldContainer ID="FieldSSIS" runat="server" Hidden="true" AnchorHorizontal="100%" Layout="AnchorLayout" Margin="0">
                        <Items>
                            <ext:FileUploadField ID="FileSSIS" runat="server" FieldLabel="File SSIS" AnchorHorizontal="100%">
                            </ext:FileUploadField>
                            <ext:DisplayField ID="txtFileName" runat="server" FieldLabel="File Name" AnchorHorizontal="100%">
                            </ext:DisplayField>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldProc" runat="server" Hidden="true" AnchorHorizontal="100%" Layout="AnchorLayout" Margin="0">
                        <Items>
                            <ext:TextField ID="txtProcName" runat="server" FieldLabel="Store Procedure Name" AnchorHorizontal="100%">
                            </ext:TextField>
                            <ext:Checkbox ID="ChkIsUseprocessDate" runat="server" BoxLabel="Use Parameter Data Date" Cls="useparam-label" FieldLabel=" " StyleSpec="display: inline-block;">
                            </ext:Checkbox>
                            <ext:Checkbox ID="ChkIsUseBranch" runat="server" BoxLabel="Use Parameter Branch" StyleSpec="display: inline-block; margin-left: 10px;">
                            </ext:Checkbox>
                        </Items>
                    </ext:FieldContainer>
                    <ext:TextArea ID="txtKeterangan" runat="server" FieldLabel="Description" AnchorHorizontal="100%">
                    </ext:TextArea>
                </Items>
                <Buttons>
                    <ext:Button ID="BtnSaveDetail" runat="server" Icon="Disk" Text="Save">
                        <DirectEvents>
                            <Click OnEvent="BtnSaveDetail_DirectEvent">
                                <EventMask Msg="Loading..." ShowMask="true" MinDelay="500"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelDetail" runat="server" Icon="Cancel" Text="Cancel">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelDetail_DirectEvent">
                                <EventMask Msg="Loading..." ShowMask="true" MinDelay="500"></EventMask>
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
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true"  >
        <Items>
            <ext:DisplayField ID="LblID" runat="server" FieldLabel="ID" Name="PK_EODTaskDetail_ID" AnchorHorizontal="80%" Text="AutoNumber">
            </ext:DisplayField>
            <ext:TextField ID="TxtTaskName" runat="server" FieldLabel="Task Name" AnchorHorizontal="80%" AllowBlank="false">
            </ext:TextField>            
            <ext:TextArea ID="TxtTaskDescription" runat="server" FieldLabel="Task Description" AnchorHorizontal="80%" >
            </ext:TextArea>
            <ext:FieldSet ID="FieldSetTaskDetail" runat="server"  Title="Task Detail" AnchorHorizontal="100%">
                <Items>
                </Items>
                <Content>
                    <ext:GridPanel ID="GridPaneldetail" runat="server" Title="Task Detail Data" AutoScroll="true" EmptyText="No Available Data">
                        <TopBar>
                            <ext:Toolbar ID="toolbar" runat="server">
                                <Items>
                                    <ext:Button ID="BtnAddNew" runat="server" Text="Add New Detail" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="BtnAddNew_DirectClick">
                                                <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreTaskDetail" runat="server"  >
                                <Model>
                                    <ext:Model ID="ModelDetail" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EODTaskDetail_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EODTask_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EODTaskDetailType_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="OrderNo" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="SSISName" Type="String"></ext:ModelField>   
                                            <ext:ModelField Name="StoreProcedureName" Type="String"></ext:ModelField>                                                                                        
                                            <ext:ModelField Name="Keterangan" Type="String"></ext:ModelField>                                                                                        
                                            <ext:ModelField Name="IsUseParameterProcessDate" Type="Boolean"></ext:ModelField>                                                                                        
                                            <ext:ModelField Name="IsUseParameterBranch" Type="Boolean"></ext:ModelField>                                                                                       
                                        </Fields>
                                    </ext:Model>
                                </Model>                                
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumberTaskDetail" runat="server"></ext:RowNumbererColumn>
                                <%--<ext:Column ID="ColPK_EODTaskDetail_ID" runat="server" DataIndex="PK_EODTaskDetail_ID" Text="ID"></ext:Column>--%>
                                <ext:Column ID="colFK_EODTaskDetailType_ID" runat="server" DataIndex="FK_EODTaskDetailType_ID" Text="Task Detail Type" Width="125">
                                    <Renderer Fn="LoadType" /> 
                                </ext:Column>
                                 <ext:Column ID="colSSISName" runat="server" DataIndex="SSISName" Text="SSIS File Name" Width="170">                                     
                                     <Commands>
                                         <ext:ImageCommand Icon="DiskDownload" CommandName="Download" ToolTip-Text="Download SSIS File"></ext:ImageCommand>         
                                     </Commands>
                                     <DirectEvents>
                                         <Command OnEvent="GridCommand">
                                              <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EODTaskDetail_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                         </Command>
                                     </DirectEvents>
                                 </ext:Column>
                                
                                <ext:Column ID="colStoreProcedure" runat="server" DataIndex="StoreProcedureName" Text="Store Procedure Name" Width="170"></ext:Column>
                                <ext:Column ID="colIsuseProcessDate" runat="server" DataIndex="IsUseParameterProcessDate" Text="Use Parameter Data Date" Width="90"></ext:Column>
                                <ext:Column runat="server" DataIndex="IsUseParameterBranch" Text="Use Parameter Branch" Width="90"></ext:Column>                                
                                <ext:Column ID="Colorder" runat="server" DataIndex="OrderNo" Text="Order No" Width="80"></ext:Column>
                                <ext:Column ID="Column2" runat="server" DataIndex="Keterangan" Text="Description" Width="100"></ext:Column>
                                <ext:CommandColumn runat="server" Width="185">
                                    <Commands>
                                        <%--<ext:GridCommand CommandName="Download" Icon="DiskDownload"  ></ext:GridCommand>--%>
                                        <ext:GridCommand CommandName="Edit" Icon="Pencil" Text="Edit" ToolTip-Text="Edit" ></ext:GridCommand>
                                        <ext:GridCommand CommandName="Delete" Icon="PencilDelete" Text="Delete" ToolTip-Text="Delete" ></ext:GridCommand>
                                        <ext:GridCommand CommandName="MoveUp" Icon="ArrowUp"  ToolTip-Text="Move Up" ></ext:GridCommand>
                                        <ext:GridCommand CommandName="MoveDown" Icon="ArrowDown"  ToolTip-Text="Move Down" ></ext:GridCommand>
                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="GridCommand"  >
                                            <ExtraParams>
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EODTaskDetail_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if (command=='Download' || command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true"  Title="Delete"  >
                                            </Confirmation>
                                        </Command>
                                    </DirectEvents>
                                </ext:CommandColumn>
                            </Columns> 
                        </ColumnModel>
                    </ext:GridPanel>
                </Content>
            </ext:FieldSet>
        </Items>
        <Buttons>
            <ext:Button ID="BtnSave" runat="server" Icon="Disk" Text="Save Task" >
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="BtnCancel" runat="server" Icon="Cancel" Text="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click">
                         <EventMask ShowMask="true" Msg="Loading Data..." MinDelay="500"> </EventMask>
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
            <ext:Label ID="LblConfirmation" runat="server" Align="center" Cls="NawaLabel"></ext:Label>
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
