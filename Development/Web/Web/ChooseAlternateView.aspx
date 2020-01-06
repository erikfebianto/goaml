<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChooseAlternateView.aspx.vb" Inherits="ChooseAlternateView" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <style>
        .list-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

            .list-item h3 {
                display: block;
                font: inherit;
                font-weight: bold;
                margin: 0px;
                color: #222;
            }
    </style>

    <script src="Scripts/Form.Master.js"></script>
    <link href="Styles/Site.Master.css" rel="stylesheet" />
    <%--<script type="text/javascript">

         function loaddata()
         {

             document.getElementById("txtUsername-inputEl").focus();
         }
        </script>--%>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Crisp" />
            <br />
            <ext:Viewport ID="Viewport1" runat="server" Layout="BorderLayout">

                <Items>
                    <ext:Panel ID="TopPanel" runat="server" Region="North" Title="NAWA DATA APPLICATION">
                        <Items></Items>
                    </ext:Panel>
                    <ext:Panel ID="MainPanel" runat="server" Region="Center" Title="">
                        <Listeners>
                            <Resize Handler="#{Window1}.center();"></Resize>
                        </Listeners>
                        <Items>
                            
                            <ext:Window
                                ID="Window1"
                                runat="server"
                                Closable="false"
                                Resizable="false"
                                Height="250"
                                Icon="Lock"
                                Title="Login As"
                                Draggable="false"
                                Width="350"
                                Modal="true"
                                BodyPadding="5"
                                Layout="FormLayout">

                                <Items>
                                    
                                    <ext:DisplayField ID="Label1" runat="server" Text="You have been assigned as an alternate for certain period.Please choose user to login." >
                                        </ext:DisplayField>
                                    
                                    <ext:ComboBox ID="CboListUser" runat="server"
                                        runat="server"
                                        Width="250"
                                        Editable="false"
                                        DisplayField="UserName"
                                        ValueField="UserID"
                                        QueryMode="Local"
                                        ForceSelection="true"
                                        TriggerAction="All"   >
                                        
                                        <Store>
                                            <ext:Store ID="StoreListUser" runat="server">
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="UserID" />
                                                            <ext:ModelField Name="UserName" />
                                                            <ext:ModelField Name="StartDate" />
                                                            <ext:ModelField Name="EndDate" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ListConfig>
                                            <ItemTpl runat="server">
                                                <Html>
                                                    <div class="list-item">
                                <h3>{UserName}</h3>
                                Valid From :{StartDate:date('d-M-y')} To {EndDate:date('d-M-y')}
                        </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>
                                </Items>
                                <Buttons>
                                    <ext:Button ID="btnLogin" runat="server" Text="Save" Icon="Accept">

                                        <DirectEvents>
                                            <Click OnEvent="btnLogin_Click">
                                                <EventMask ShowMask="true" Msg="Verifying..." MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                        <DirectEvents>
                                            <Click OnEvent="btnCancel_Click">
                                                <EventMask ShowMask="true" Msg="Loading..." MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                        </ext:Button>
                                </Buttons>
                            </ext:Window>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>

        </div>
    </form>
</body>
</html>