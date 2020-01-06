
Partial Class Parameter_ChangePassword
    Inherits Parent



    Protected Sub btnCancel_DirectEvent(sender As Object, e As DirectEventArgs)
        Try



            Ext.Net.X.Redirect("~/Default.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnBack_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Ext.Net.X.Redirect("~/Default.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnLogin_Click(sender As Object, e As DirectEventArgs)
        Try
            If IsDataPasswordValid() Then
                NawaBLL.ChangePasswordBLL.ChangePasword(NawaBLL.Common.SessionCurrentUser.UserID, txtPassword.Text.Trim)
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

            If txtPassword.Text.Trim <> txtRetypePassword.Text.Trim Then
                Throw New Exception("Password and Retype Password Not Same ")
            End If

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



            'validate unik userid
            ' If Not NawaBLL.MUserBLL.IsUserIdUnikAdd(txtUserID.Text.Trim) Then

            '    Throw New Exception("Userid " & txtUserID.Text.Trim & " already exists.")
            'End If

            'validate userid belum ada di approval

            '            If NawaBLL.MUserBLL.IsUseridExistInApproval(txtUserID.Text.Trim) Then

            '               Throw New Exception("Userid " & txtUserID.Text.Trim & " already exists in approval.")
            '          End If



            If Not NawaBLL.MUserBLL.ValidatePasswordRecycleCount(txtUserID.Text, txtPassword.Text.Trim) Then

                Throw New Exception("Your Password have Ever Used.Please Change to another Password.")
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub LoadUser()
        txtUserID.Text = NawaBLL.Common.SessionCurrentUser.UserID
    End Sub


    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                LoadUser()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
