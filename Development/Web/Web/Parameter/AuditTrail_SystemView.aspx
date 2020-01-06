<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master"AutoEventWireup="false" CodeFile="AuditTrail_SystemView.aspx.vb" Inherits="AuditTrail_SystemView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">


        Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
            return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
        };


        Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
            return {
                type: "string",
                op: "*",
                value: value
            };
        };

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ext:Panel ID="Panel1" runat="server" >
        <Items></Items>
    </ext:Panel>

</asp:Content>

