<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="EODSchedulerAdd.aspx.vb" Inherits="EODSchedulerAdd" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Window ID="WindowDetail" runat="server"  Icon="ApplicationViewDetail" Title="EOD Scheduler Detail" Hidden="true" Layout="FitLayout" Modal="true" BodyPadding="10">
        <Items>
            <ext:FormPanel ID="FormSchedulerDetail" runat="server" AnchorHorizontal="100%" Hidden="true">
                <Items>
                    <ext:ComboBox ID="cboTask" runat="server" FieldLabel="EOD Task" DisplayField="EODTaskName" ValueField="PK_EODTask_ID" MatchFieldWidth="true" TypeAhead="true" 
                        AllowBlank="false" AnyMatch="true" QueryMode="Local" ForceSelection="True" EmptyText="Please Select EOD Task" AnchorHorizontal="95%" LabelWidth="90">
                        <Store>
                            <ext:Store runat="server" ID="StoreTask">
                                <Model>
                                    <ext:Model ID="ModelTask" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EODTask_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="EODTaskName" Type="String"></ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <Triggers>
                            <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                        </Triggers>
                        <Listeners>
                            <Select Handler="this.getTrigger(0).show();" />
                            <TriggerClick Handler="if (index == 0) { 
                                           this.clearValue(); 
                                           this.getTrigger(0).hide();
                                       }" />
                        </Listeners>
                    </ext:ComboBox>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSaveSchedulerDetail" runat="server" Icon="Disk" Text="Save Scheduler Detail">
                        <DirectEvents>
                            <Click OnEvent="btnSaveSchedulerDetail_DirectEvent">
                                <EventMask Msg="Saving..." MinDelay="500" ShowMask="true"></EventMask>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="BtnCancelSchedulerDetail" runat="server" Icon="Cancel" Text="Cancel Scheduler Detail">
                        <DirectEvents>
                            <Click OnEvent="BtnCancelSchedulerDetail_DirectEvent">
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
    <ext:FormPanel ID="FormPanelInput" runat="server" ButtonAlign="Center" Title="" BodyStyle="padding:20px" AutoScroll="true"  >
        <Items>
            <ext:DisplayField ID="txtID" runat="server" Text="AutoNumber" FieldLabel="ID">
            </ext:DisplayField>
            <ext:TextField ID="txtSchedulerName" runat="server" FieldLabel="Scheduler Name" AnchorHorizontal="80%">
            </ext:TextField>
            <ext:TextArea ID="txtSchedulerDesc" runat="server"  FieldLabel="Scheduler Description" AnchorHorizontal="80%" >
            </ext:TextArea>
            <ext:Checkbox ID="chkUsePeriodicalScheduler" runat="server" FieldLabel="Has Periodical Scheduler" BoxLabel="Yes" Checked="true">
                <DirectEvents>
                    <Change OnEvent="chkUsePeriodicalScheduler_DirectEvent">
                          <EventMask Msg="Loading..." MinDelay="500" ShowMask="true"></EventMask>
                    </Change>
                </DirectEvents>
            </ext:Checkbox>
            <ext:FieldContainer ID="FieldContainer1" runat="server" FieldLabel="Scheduler Period">
                <Items>
                    <ext:NumberField ID="txtPeriod" runat="server" FieldLabel="Period">
                    </ext:NumberField> 
                    <ext:ComboBox ID="CboPeriod" runat="server" FieldLabel="Period Type" DisplayField="MsEODPeriodName" ValueField="PK_MsEODPeriod_Id" AnchorHorizontal="30%">
                        <Store>
                            <ext:Store ID="storeperiod" ClientIDMode="Static" runat="server" >
                                <Model>
                                    <ext:Model ID="Modelperiod" runat="server" >
                                        <Fields>
                                            <ext:ModelField Name="PK_MsEODPeriod_Id" Type="Int">                                                
                                            </ext:ModelField>
                                            <ext:ModelField Name="MsEODPeriodName" Type="String">                                                
                                            </ext:ModelField>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                      <ext:DateField ID="txtStartDate" runat="server" FieldLabel="StartDate" Format="dd-MMM-yyyy">
            </ext:DateField>
            <ext:TimeField ID="TxtTime" runat="server" FieldLabel="Time" Format="HH:mm:ss"></ext:TimeField>
                </Items>
            </ext:FieldContainer>
          
            <ext:FieldSet ID="FieldSet1" runat="server" Title="Scheduler Detail">
                <Items>
               

                </Items>
                <Content>
                    <ext:GridPanel ID="gridSchedulerDetail" runat="server" >
                        <TopBar>
                            <ext:Toolbar ID="toolbar" runat="server">
                                <Items>
                                    <ext:Button ID="btnAddNew" runat="server" Text="Add new Detail" >
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
                            <ext:Store ID="StoreSchedulerDetail" runat="server"  >
                                <Model>
                                    <ext:Model ID="ModelDetail" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="PK_EODSchedulerDetail_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EODSCheduler_ID" Type="Auto"></ext:ModelField>
                                            <ext:ModelField Name="FK_EODTask_ID" Type="Int"></ext:ModelField>
                                            <ext:ModelField Name="TaskName" Type="String"></ext:ModelField>
                                            <ext:ModelField Name="OrderNo" Type="Int"></ext:ModelField>                                            
                                        </Fields>
                                    </ext:Model>
                                </Model>                                
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumberTaskDetail" runat="server" Text="No"></ext:RowNumbererColumn>
                                
                                <ext:Column ID="ColTaskID" runat="server" DataIndex="TaskName" Text="Task Name" Flex="1" >                                    
                                </ext:Column>
                                 <ext:Column ID="colOrderNo" runat="server" DataIndex="OrderNo" Text="Order No" Flex="1">                                                                         
                                 </ext:Column>
                                
                              
                                <ext:CommandColumn ID="CommandColumn1" runat="server" Flex="1" Width="450"  >
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
                                                <ext:Parameter Name="unikkey" Value="record.data.PK_EODSchedulerDetail_ID" Mode="Raw"></ext:Parameter>
                                                <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                            </ExtraParams>
                                            <Confirmation Message="Are You Sure To Delete This Record ?" BeforeConfirm="if ( command=='Edit' || command=='MoveUp' || command=='MoveDown') return false; " ConfirmRequest="true"  Title="Delete"  >
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
            <ext:Button ID="btnSave" runat="server" Icon="Disk" Text="Save Process" >
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="BtnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving Data..." MinDelay="500"> </EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="Cancel">
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


