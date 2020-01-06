<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site1.Master" inherits="ParameterAdd, App_Web_parameteradd.aspx.252c98" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript">
         var isHurufAngkaKeyAndChar = function (field, e) {
         	return;
            //var charCode = e.getKey();
            // if ((charCode > 37 && charCode < 42) || charCode == 47 || charCode == 60 || charCode == 62 || charCode == 59) {
            //     console.log(charCode);
            //    window.event.returnValue = false;
            //} else {
            //    return;
            //}
        }
 function selectcontrol(objcontrol,data) {

        objcontrol.markInvalid(data.innerHTML);
             objcontrol.focus();
  
  
       
         }
       </script> 
</asp:Content>  

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <ext:FormPanel ID="FormPanelInput" BodyPadding="20" runat="server" ClientIDMode="Static"   Layout="AnchorLayout" ButtonAlign="Center" AutoScroll="true" >
           
          <DockedItems>
            <ext:Toolbar ID="ToolbarInput" ClientIDMode="Static" runat="server" EnableOverflow="true" Dock="Top" Layout="FitLayout" MaxHeight="200"  >                
                <Items>
                    <%--<ext:InfoPanel ID="InfoPanel1" runat="server" Html="Testing</br>Testing</br>Testing</br>Testing</br>Testing</br>Testing" Hidden="false"  AutoScroll="true">                        
                    </ext:InfoPanel>--%>
                </Items>
                </ext:Toolbar>                                                     
        </DockedItems>
        <Items>
          

        </Items>
        <Buttons>
            <ext:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="Save" Enabled="false" Icon="DiskBlack">
                <Listeners>
                    <Click Handler="if (!#{FormPanelInput}.getForm().isValid()) return false;"></Click>
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Callback">
                        <ExtraParams>
                            <ext:Parameter Name="command" Value="New" Mode="Value">
                            </ext:Parameter>

                        </ExtraParams>
                        <EventMask ShowMask="true"></EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                <DirectEvents>
                    <Click OnEvent="BtnCancel_Click"></Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>

    </ext:FormPanel>
    <ext:FormPanel ID="Panelconfirmation" BodyPadding="20" runat="server" ClientIDMode="Static" Border="false" Frame="false" Layout="HBoxLayout" ButtonAlign="Center" DefaultAnchor="100%" Visible="false">
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
            </ext:Button>
        </Buttons>
    </ext:FormPanel>

</asp:Content>
