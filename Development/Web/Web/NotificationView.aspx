<%@ Page Title="" Language="VB" MasterPageFile="~/Site1.Master" AutoEventWireup="false" CodeFile="NotificationView.aspx.vb" Inherits="NotificationView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

         <ext:Panel ID="Panel1" runat="server" Title="Notification" MinWidth="300"  Region="Center">
             <Items>
                 <ext:Panel ID="Panel2" runat="server" MinWidth="300" Title="Title">
                     <Items></Items>
                 </ext:Panel>
             </Items>
         </ext:Panel>
        
        
</asp:Content>

