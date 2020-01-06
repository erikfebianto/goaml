
Partial Class ResetPasswordEdit
    Inherits Parent
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("ResetPasswordEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ResetPasswordEdit.ObjModule") = value
        End Set
    End Property
    Protected Sub btnCancel_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnBack_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As DirectEventArgs)
        Try
            If IsDataPasswordValid() Then
                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim objuserreset As NawaDAL.MUser = NawaBLL.MUserBLL.GetMuserbyPKuserId(id)

                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                    NawaDevBLL.ResetPasswordBLL.EditPaswordDirect(objuserreset.UserID, txtPassword.Text.Trim)
                    lblConfirmation.Text = "Reset Password Saved"
                Else
                    NawaDevBLL.ResetPasswordBLL.EditPaswordApproval(objuserreset.UserID, txtPassword.Text.Trim, ObjModule)
                    lblConfirmation.Text = "Reset Password Save into pending Approval."
                End If
                Window1.Hidden = True
                WindowConfirmation.Hidden = False
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Public Function IsDataPasswordValid() As Boolean
        Try


            Dim strMinimumpasswordlength As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(NawaBLL.SystemParameterBLL.SytemParameterEnum.MinimumPasswordLength).SettingValue
            If Not NawaBLL.MUserBLL.ValidateMinimumPasswordLengh(txtPassword.Text.Trim) Then

                Throw New Exception("Minimum Password Lengh is " & strMinimumpasswordlength & " char")

            End If

            ' Dim txtUserId As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserID"), Ext.Net.TextField)

            If Not NawaBLL.MUserBLL.ValidateLowerAndUpperCase(txtPassword.Text.Trim) Then

                Throw New Exception("Password Must have Lower And Upper Case char")

            End If

            If Not NawaBLL.MUserBLL.ValidateAlfaNumericSymbol(txtPassword.Text.Trim) Then

                Throw New Exception("Password Must have Alfa Numeric Symbol")

            End If

            'If Not NawaBLL.MUserBLL.ValidateAlfaNumeric(txtPassword.Text.Trim) Then

            '    Throw New Exception("Password Must have Alfa Numeric")

            'End If


            ''validate unik userid
            'If Not NawaBLL.MUserBLL.IsUserIdUnikAdd(txtUserID.Text.Trim) Then

            '    Throw New Exception("Userid " & txtUserID.Text.Trim & " already exists.")
            'End If

            ''validate userid belum ada di approval

            'If NawaBLL.MUserBLL.IsUseridExistInApproval(txtUserID.Text.Trim) Then

            '    Throw New Exception("Userid " & txtUserID.Text.Trim & " already exists in approval.")
            'End If



            If Not NawaBLL.MUserBLL.ValidatePasswordRecycleCount(txtUserID.Text, txtPassword.Text.Trim) Then

                Throw New Exception("Your Password have Ever Used.Please Change to another Password.")
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub LoadUser()
        Dim strid As String = Request.Params("ID")
        Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
        Dim objuserreset As NawaDAL.MUser = NawaBLL.MUserBLL.GetMuserbyPKuserId(id)
        If Not objuserreset Is Nothing Then
            txtUserID.Text = objuserreset.UserID
            txtUsername.Text = objuserreset.UserName
        End If
    End Sub


    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load

        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")

                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Update) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                  LoadUser
                    Window1.Title = ObjModule.ModuleLabel
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            'objEodTask.BentukformAdd()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
End Class
