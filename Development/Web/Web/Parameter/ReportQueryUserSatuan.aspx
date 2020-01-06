<%@ page title="" language="VB" masterpagefile="~/Site1.master" autoeventwireup="false" inherits="ReportQueryUserSatuan, App_Web_reportqueryusersatuan.aspx.252c98" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
            <ext:GridPanel ID="GridPreview" runat="server" ClientIDMode="Static" Title="Preview" Height="300">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server" EnableOverflow="true">
                        <Items>

                            <ext:ComboBox runat="server" ID="cboExportExcel" Editable="false"  FieldLabel="Export :">

                                <Items>
                                    <ext:ListItem Text="Excel" Value="Excel"></ext:ListItem>
                                    <ext:ListItem Text="CSV" Value="CSV"></ext:ListItem>
                                </Items>

                            </ext:ComboBox>
                            <ext:Button runat="server" ID="BtnExport" Text="Export Current Page" AutoPostBack="true" OnClick="ExportExcel" ClientIDMode="Static" />
                            <ext:Button runat="server" ID="BtnExportAll" Text="Export All Page" AutoPostBack="true" OnClick="ExportAllExcel" />

                            <%--<ext:Button ID="Button1" runat="server" Text="Print Current Page" Icon="Printer" Handler="this.up('grid').print({currentPageOnly : true});" />--%>

                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
                <Store>
                    <ext:Store ID="storePreview" runat="server" IsPagingStore="true" RemoteFilter="true" RemoteSort="true" OnReadData="storePreview_Readdata" RemotePaging="true" ClientIDMode="Static">
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
        

</asp:Content>


