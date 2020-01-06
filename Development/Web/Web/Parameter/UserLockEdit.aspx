<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="UserLockEdit.aspx.vb" Inherits="UserLockEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:Window
        ID="Window1"
        runat="server"
        Closable="false"
        Resizable="false"
        Height="350"
        Icon="Lock"
        Title="Change Password"
        Draggable="false"
        Width="350"
        Modal="true"
        BodyPadding="5"
        Layout="FormLayout">

        <Items>
            <ext:DisplayField ID="txtUserID"
                FieldLabel="User ID">
            </ext:DisplayField>
            <ext:DisplayField ID="txtUsername"
                FieldLabel="Staff Name">
            </ext:DisplayField>
            <ext:Checkbox
                ID="CbInUsed"
                runat="server"
                FieldLabel="In Used"
                MsgTarget="Qtip">
            </ext:Checkbox>
            <ext:Checkbox
                ID="CbLockPassword"
                runat="server"
                FieldLabel="Lock User"
                MsgTarget="Qtip">
            </ext:Checkbox>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Accept">
                <DirectEvents>
                    <Click OnEvent="btnSave_Click">
                        <EventMask ShowMask="true" Msg="Saving..." MinDelay="500" />
                    </Click>
                </DirectEvents>
            </ext:Button>

            <ext:Button ID="BtnCancel" runat="server" Text="Cancel" Icon="Cancel">
                <DirectEvents>
                    <Click OnEvent="btnCancel_DirectEvent">
                        <EventMask ShowMask="true" Msg="Loading..." MinDelay="500">
                        </EventMask>
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>



    <ext:Window
        ID="WindowConfirmation"
        runat="server"
        Closable="false"
        Resizable="false"
        Height="175"
        Icon="Lock"
        Title="Confirmation"
        Draggable="false"
        Width="350"
        Modal="true"
        BodyPadding="5"
        Layout="FormLayout" Hidden="true" Anchor="center">

        <Items>
            <ext:DisplayField runat="server" ID="lblConfirmation" Text="User Lock Changed Sucessfully."></ext:DisplayField>
        </Items>
        <Buttons>


            <ext:Button ID="btnBack" runat="server" Text="OK" Icon="Accept">
                <DirectEvents>
                    <Click OnEvent="btnBack_DirectEvent">
                        <EventMask ShowMask="true" Msg="Loading..."></EventMask>
                    </Click>
                </DirectEvents>

            </ext:Button>
        </Buttons>

    </ext:Window>


</asp:Content>

