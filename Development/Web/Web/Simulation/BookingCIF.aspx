<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="BookingCIF.aspx.vb" Inherits="BookingCIF" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/NDSDropDownField.ascx" TagPrefix="NDS" TagName="NDSDropDownField" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
<SCRIPT   type ="text/javascript">

function valconvert(val,controlinput,controlresult){

var nm=String(val);
inpt=document.getElementById(controlinput);
rslt=document.getElementById(controlresult);

    if (nm == "") {
        nm = "0";
        inpt.value = "0";
    }
    if (isNaN(nm) == true) {
        nm = nm.replace(/[^0-9]/g, "");
        inpt.value = nm;
    }

while(nm.substring(0,1)=="0"&&nm.length>1)
{
    nm = nm.substring(1, nm.length);
    inpt.value = nm;
}
   
    if (nm == "0") {
    
 zr="Nol.";
 
 rslt.innerHTML=zr;
inpt.value="0";

}else{
var hsl=String(bilang(nm));
    hsl = (hsl.substring(0, 1).toUpperCase() + hsl.substring(1, hsl.length) + ".").replace(" .", ".");
    rslt.innerHTML=hsl;
    }

    }



    function bilang(n) {
    
var s=["","satu","dua","tiga","empat","lima","enam","tujuh","delapan","sembilan"];

var p=new Array(6);
p[1]="puluh";
p[2]="ratus";
p[3]="ribu";
p[6]="juta";
p[9]="milyar";
p[12]="triliyun";

    while (n.substring(0, 1) == 0 && n.length > 1)
    {
        n = n.substring(1, n.length);
    }
        if (n == "") {
            return "";
        }
        if (n.length == 1) {
            return s[(n)];
        }

        if (String(n).substring(0, 1) == "1" & n.length > 1){

    var f = "se";

        } else {

    var f = s[n.substring(0, 1)] + " ";

        }
         

if(n.length==2){
    if (n.substring(0, 1) == 1 & n.substring(n.length, n.length - 1) > 0) {

        if (n.substring(n.length, n.length - 1) == 1) {
          
            return "sebelas";

        } else {
           
            return s[n.substring(1, 2)] + " belas";

        }

    } else {
      
        return f + p[n.length - 1] + " " + s[n.substring(1, 2)];

    }
} else if (n.length == 3 | n.length == 4) {
    
    return f + p[n.length - 1] + " " + bilang(n.substring(1, n.length));
} else {
    t = (1 + (n.length - 1) % (3));
   
    return (bilang(n.substring(0, t)) + " " + p[n.length - t] + " " + bilang(n.substring(t, n.length))).replace(/  /g, " ");
        }
    }
</SCRIPT>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    

 
            <ext:FormPanel BodyPadding="15" ID="FormPanelInput" runat="server"   Margin ="0"  AutoScroll="true"    >
                <Items>
            
                    <ext:Panel  ID="panelInput" runat="server"   Layout="FormLayout"   BodyPadding ="0" >
                          
                         <Items>
                             <ext:Container ID="Container1" runat="server"    >

                             <Items>  
                                   <ext:Panel  AlignTarget="left"   runat="server" Layout="AnchorLayout" Width ="505"   AnchorHorizontal ="80%">
                                <Content>
                                  <NDS:NDSDropDownField  LabelAlign="Left" LabelWidth="200"   ID="tCIF" runat="server" StringField="CUST_NO,CustnoName" StringTable="vw_CustomerList" Label="CIF Number"   AllowBlank="false" OnOnValueChanged="tCIF_Change"    /> 
                                </Content>
                            </ext:Panel> 

                             
                             <ext:DisplayField ID="lblCIF" runat="server" FieldLabel="Customer Name" ClientIDMode="Static"  Width ="500" LabelWidth ="200" ></ext:DisplayField>

                                
                                 
                            <ext:ComboBox   AllowBlank="false" ID="CbFacilityType" runat="server" FieldLabel="Facility Type" DisplayField="CLS_MS_FacilityName"
                                ValueField="FAC_ID" ForceSelection="True" Editable="false" EmptyText="Please Select Field Name"  Width ="500" LabelWidth ="200" >
                                <Store>
                                    <ext:Store runat="server" ID="StoreCbFacilityType">
                                        <Model>
                                            <ext:Model ID="ModelCbFacilityType" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="FAC_ID" Type="String"></ext:ModelField>
                                                    <ext:ModelField Name="CLS_MS_FacilityName" Type="String"></ext:ModelField>
                                         
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                               
                            </ext:ComboBox>
                             
 

                             <ext:ComboBox ID="CbCurrencyCode"   AllowBlank="false" runat="server" FieldLabel="Currency Code" DisplayField="CLS_MS_MataUangName" Width ="500" LabelWidth ="200"
                                ValueField="CLS_MS_MataUangCode" ForceSelection="True" Editable="false" EmptyText="Please Select Field Name" >
                                <Store>
                                    <ext:Store runat="server" ID="StoreCbCurrencyCode">
                                        <Model>
                                            <ext:Model ID="ModelCbCurrencyCode" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CLS_MS_MataUangCode" Type="String"></ext:ModelField>
                                                    <ext:ModelField Name="CLS_MS_MataUangName" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                               
                            </ext:ComboBox>

                            <ext:TextField ID="txtNewLimit"  ClientIDMode="Static"     AllowBlank="false"    runat="server" Text="" FieldLabel="New Limit (New Additional)"  Width ="500" LabelWidth ="200" >
                             <Listeners >
                                     <Change Handler ="valconvert(this.value,'txtNewLimit','lblterbilangNewLimit');" ></Change>
                                 </Listeners>
                             </ext:TextField>
                           

                                 
                                    <ext:Panel ID="Panel1" runat="server"   Layout="FitLayout" >
                                        <Items>        
                                            <ext:Label ID="lblterbilangNewLimit"   PaddingSpec ="10 0 10 200" ClientIDMode="Static" runat="server" AlignTarget="right"    Width ="500" ></ext:Label>
                                        </Items>
                                    </ext:Panel>

                              <ext:ComboBox ID="CbCollateralCurrencyCode" runat="server" FieldLabel="Cash Collateral Curr Code" DisplayField="CLS_MS_MataUangName" Width ="500" LabelWidth ="200" AllowBlank="false"
                                ValueField="CLS_MS_MataUangCode" ForceSelection="True" Editable="false"  EmptyText="Please Select Field Name" >
                                <Store>
                                    <ext:Store runat="server" ID="StoreCbCollateralCurrencyCode">
                                        <Model>
                                            <ext:Model ID="ModelCbCollateralCurrencyCode" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CLS_MS_MataUangCode" Type="String"></ext:ModelField>
                                                    <ext:ModelField Name="CLS_MS_MataUangName" Type="String"></ext:ModelField>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                               
                            </ext:ComboBox>

                             <ext:TextField ID="txtCollateralAmt"   AllowBlank="false" ClientIDMode="Static"    runat="server" Text="" FieldLabel="Cash Collateral Amount" Width ="500" LabelWidth ="200">
                                 <Listeners >
                                     <Change Handler ="valconvert(this.value,'txtCollateralAmt','lblterbilangCollateralAmt');" ></Change>
                                 </Listeners>
                             </ext:TextField>
                                 
                                    
                                
                                                           <ext:Panel ID="pnlterbilang" runat="server"   Layout="FitLayout" >
                                                                <Items>        
                                                                   <ext:Label ID="lblterbilangCollateralAmt"    PaddingSpec ="10 0 10 200" ClientIDMode="Static" runat="server" AlignTarget="right"    Width ="500"  ></ext:Label>
                             
                                                                </Items>
                                                            </ext:Panel>
                                
                          
                            
                              <ext:Button ID="ButtonSimulate"  runat="server" Icon="Zoom" Scale="Medium" Text="Simulate"  AutoPostBack="true">
                            <Listeners>
                               <Click Handler="Ext.net.Mask.show({ msg : 'Loading...' });" />
                          </Listeners>
                                  </ext:Button>

                            </Items>
                           </ext:Container> 
                        </Items>
                        <Content>
                    <asp:ScriptManager ID="ScriptManager1" runat="server" ViewStateMode="Enabled"></asp:ScriptManager>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" EnableViewState="True"></rsweb:ReportViewer>
                  </Content>
                    </ext:Panel>

                     </Items>
            </ext:FormPanel>
         

    
</asp:Content>

