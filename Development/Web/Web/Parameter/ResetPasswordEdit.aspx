<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeFile="ResetPasswordEdit.aspx.vb" Inherits="ResetPasswordEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ext:Window
        ID="Window1"
        runat="server"
        Closable="false"
        Resizable="false"
        Height="250"
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
            <ext:TextField
                ID="txtPassword"
                runat="server"
                FieldLabel="Password"
                AllowBlank="false"
                RemoveClearTrigger="true"
                BlankText="Your password is required."
                RightButtonsShowMode="MouseOverOrFocus"
                MsgTarget="Qtip">
                <RightButtons>
                    <ext:Button ID="Button1" runat="server"
                        Icon="ArrowSwitch"
                        ToolTip="Generate password"
                        Handler="this.up('textfield').passwordMask.generatePassword(); #{PassMode}.toggle(true, true);" />


                    <ext:Button runat="server"
                        ID="PassMode"
                        Icon="TextAb"
                        AllowDepress="true"
                        EnableToggle="true"
                        ToolTip="Show password">
                        <Listeners>
                            <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                        </Listeners>
                    </ext:Button>
                </RightButtons>
                <Plugins>
                    <ext:PasswordMask ID="PasswordMask1" runat="server" AcceptRate="0" Mode="HideAll" />
                </Plugins>
            </ext:TextField>

            <ext:TextField
                ID="txtRetypePassword"
                runat="server"
                FieldLabel="Retype Password"
                AllowBlank="false"
                RemoveClearTrigger="true"
                BlankText="Your password is required."
                RightButtonsShowMode="MouseOverOrFocus"
                MsgTarget="Qtip">
                <RightButtons>


                    <ext:Button runat="server"
                        ID="Button3"
                        Icon="TextAb"
                        AllowDepress="true"
                        EnableToggle="true"
                        ToolTip="Show password">
                        <Listeners>
                            <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                        </Listeners>
                    </ext:Button>
                </RightButtons>
                <Plugins>
                    <ext:PasswordMask ID="PasswordMask2" runat="server" AcceptRate="0" Mode="HideAll" />
                </Plugins>
            </ext:TextField>
        </Items>
        <Buttons>
            <ext:Button ID="btnLogin" runat="server" Text="Save" Icon="Accept">
                <Listeners>
                    <Click Handler="
                            if (!#{txtRetypePassword}.validate() || !#{txtPassword}.validate()) {
                                Ext.Msg.alert('Error','The Password and Retype Password fields are both required');
                                // return false to prevent the btnLogin_Click Ajax Click event from firing.
                                return false; 
                            }" />
                </Listeners>
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
            <ext:DisplayField runat="server" ID="lblConfirmation" Text="Password Changed Sucessfully."></ext:DisplayField>
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
