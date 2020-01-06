<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterOther.Master" CodeFile="Default.aspx.vb" Inherits="_Default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript" >


           //Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
           //    return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
           //};


           //Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
           //    return {
           //        type: "string",
           //        op: "*",
           //        value: value
           //    };
           //};
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Panel ID="Panel1" runat="server" >
        <Items></Items>
    </ext:Panel>

</asp:Content>
