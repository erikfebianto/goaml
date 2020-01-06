<%@ control language="VB" autoeventwireup="false" inherits="Component_AdvancedFilter, App_Web_advancedfilter.ascx.43883e39" %>

<ext:Window ID="WindowFilter" runat="server" Height="185" Icon="Application" Title="Advanced Filter" Width="350" Hidden="true" ClientIDMode="Static" ButtonAlign="Center" AutoScroll="true">
    <Items>

        <ext:FormPanel ID="FormPanel1" runat="server" ButtonAlign="Center" Height="100" BodyPadding="5" AnchorHorizontal="100%">
            <Items>
                <ext:FieldContainer ID="FieldContainer1" runat="server" AnchorHorizontal="100%" Padding="5" PaddingSpec="5"
                    Layout="HBoxLayout">
                    
                    <Items>

                        <ext:Label ID="Label1" runat="server" Text="Filter  : ">
                        </ext:Label>
                        <ext:ComboBox ID="cboAndOr" runat="server" MarginSpec="0 10 0 0" Width="70" >
                            <Items>
                                <ext:ListItem Text="--" Value=""></ext:ListItem>
                                <ext:ListItem Text="And" Value="And"></ext:ListItem>
                                <ext:ListItem Text="OR" Value="OR"></ext:ListItem>
                            </Items>
                        </ext:ComboBox>

                        <ext:ComboBox ID="cboField" runat="server" MarginSpec="0 10 0 0" ValueField="FieldName" DisplayField="FieldLabel" ForceSelection="true" AllowBlank="false" >
                            <Store>
                                <ext:Store runat="server" ID="StoreField">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="FieldLabel" Type="String"></ext:ModelField>
                                                <ext:ModelField Name="FieldName" Type="String"></ext:ModelField>
                                                
                                                
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Change OnEvent="cboField_DirectSelect"></Change>
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:ComboBox ID="cboFilterClause" runat="server" MarginSpec="0 10 0 0" DisplayField="FilterWhereClause" MinChars="0" AllowBlank="false" ValueField="PK_AdvancedFilterWhereClause_ID" ForceSelection="true">
                            <Store>
                                <ext:Store runat="server" ID="StoreFilterClause">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="FilterWhereClause" Type="String"></ext:ModelField>
                                                <ext:ModelField Name="FilterWhereFormat" Type="String"></ext:ModelField>
                                                <ext:ModelField Name="PK_AdvancedFilterWhereClause_ID" Type="String"></ext:ModelField>
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Change OnEvent="cboFilterClause_DirectSelect"></Change>
                            </DirectEvents>
                        </ext:ComboBox>
                        <ext:TextField ID="txtvaluebefore" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:TextField>
                        <ext:TextField ID="txtvalueafter" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:TextField>
                        <ext:DateField ID="txtDatebefore" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:DateField>
                        <ext:DateField ID="txtDateafter" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:DateField>

                        <ext:NumberField ID="txtNumberbefore" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:NumberField>
                        <ext:NumberField ID="txtNumberafter" runat="server" MarginSpec="0 10 0 0" Hidden="true">
                        </ext:NumberField>

                        <ext:Button ID="btnAdd" runat="server" Text="Add">
                            <DirectEvents>
                                <Click OnEvent="btnadd_click">
                                    <EventMask MinDelay="500" Msg="Adding..."></EventMask>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:FieldContainer>
            </Items>
        </ext:FormPanel>

        <ext:GridPanel ID="GridViewFilter" runat="server">
            <Store>
                <ext:Store ID="StoreFilter" runat="server" IDProperty="pk">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="pk" Type="Int" />
                                <ext:ModelField Name="OperatorAndOR" Type="String" />
                                <ext:ModelField Name="FieldData" Type="String" />
                                <ext:ModelField Name="OperatorFilter" Type="String" />
                                <ext:ModelField Name="ValueFilter" Type="String" />
                                <ext:ModelField Name="AllFilterview" Type="String" />
                                <ext:ModelField Name="AllFilterQuery" Type="String" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:CommandColumn ID="frmModuleField" runat="server" Text="Action">
                        <DirectEvents>

                            <Command OnEvent="CallBackAdvance">
                                <ExtraParams>
                                    <ext:Parameter Name="unikkey" Value="record.data.pk" Mode="Raw"></ext:Parameter>
                                    <ext:Parameter Name="command" Value="command" Mode="Raw"></ext:Parameter>
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                        <Commands>

                            <ext:GridCommand Text="Delete" CommandName="Delete" Icon="Delete" MinWidth="70">
                                <ToolTip Text="Delete"></ToolTip>
                            </ext:GridCommand>
                        </Commands>
                    </ext:CommandColumn>

                    <ext:Column runat="server" Text="Filter" Flex="1" DataIndex="AllFilterview" />
                    <ext:Column runat="server" Text="Filter SQL" Flex="1" DataIndex="AllFilterQuery" />
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
    </Items>
    <Listeners>
        <BeforeShow Handler="var size = Ext.getBody().getSize(); this.setSize({ width: size.width * 0.8, height: size.height * 0.6});" />
        <Resize Handler="#{WindowFilter}.center()" />
    </Listeners>
    <Buttons>
        <ext:Button ID="btnSubmit" runat="server" Icon="Disk" Text="Filter">
            <DirectEvents>
                <Click OnEvent="btnSubmit_Click">

                </Click>
            </DirectEvents>
        </ext:Button>

    </Buttons>
</ext:Window>