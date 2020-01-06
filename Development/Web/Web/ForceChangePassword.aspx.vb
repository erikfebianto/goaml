
Partial Class ForceChangePassword
    Inherits Page


    Protected Sub btnCancel_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Ext.Net.X.Redirect("~/Logout.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnBack_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Ext.Net.X.Redirect("~/Logout.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnLogin_Click(sender As Object, e As DirectEventArgs)
        Try
            If IsDataPasswordValid() Then
                NawaBLL.ChangePasswordBLL.ChangePasword(objUser.UserID, txtPassword.Text.Trim)
                Ext.Net.X.Redirect("~/Logout.aspx")
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



            If Not NawaBLL.MUserBLL.ValidatePasswordRecycleCount(txtUserID.Text, txtPassword.Text.Trim) Then

                Throw New Exception("Your Password have Ever Used.Please Change to another Password.")
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub LoadUser()
        txtUserID.Text = objUser.UserID
    End Sub


    Public ReadOnly Property PKUserID() As String
        Get
            Dim struserdecript As String = Request.Params("PKUserID")
            Dim struser As String = NawaBLL.Common.DecryptQueryString(struserdecript, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            Session("ForceChangePassword.UserId") = struser
            Return Session("ForceChangePassword.UserId")
        End Get

    End Property


    Public ReadOnly Property objUser() As NawaDAL.MUser
        Get
            Session("ForceChangePassword.objUser") = NawaBLL.MUserBLL.GetMuserbyPKuserId(Me.PKUserID)
            Return Session("ForceChangePassword.objUser")
        End Get
    End Property
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
