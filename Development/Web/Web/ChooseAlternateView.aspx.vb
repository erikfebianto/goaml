
Imports NawaBLL
Imports NawaDAL
Partial Class ChooseAlternateView
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(sender As Object, e As DirectEventArgs)
        Try
            If Not Common.SessionTempAlternateUser Is Nothing Then
                If CboListUser.SelectedItem.Value <> "" Then

                    Dim objUser As MUser = LoginBLL.GetMuserByUserID(CboListUser.SelectedItem.Value.Trim)

                    NawaBLL.LoginBLL.SetAlternateUserLogin(objUser, Common.SessionTempAlternateUser, Request.UserHostAddress)


                    Dim strpath As String = Request.ApplicationPath
                    If strpath = "/" Then strpath = ""
                    NawaBLL.Common.GetApplicationPath = strpath

                    Dim strredirect As String = ""
                    Dim strredirectpath As String = Request.Params("RedirectURL")




                    If strredirectpath = "" Then
                        strredirect = "Default.aspx"
                    Else
                        strredirect = "Default.aspx?RedirectURL=" & strredirectpath.Replace(strpath.ToLower, "")
                End If
                    FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                    'Me.Response.Redirect(StrRedirect, False)
                    Ext.Net.X.Redirect(StrRedirect)

                Else
                    Throw New Exception("Select at least one Login As.")
                End If
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As DirectEventArgs)
        Try
            Ext.Net.X.Redirect("Logout.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Private Sub ChooseAlternateView_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                If Not Common.SessionTempAlternateUser Is Nothing Then
                    TopPanel.Title = LoginBLL.GetAplicationName
                    ResourceManager1.Theme = NawaBLL.SystemParameterBLL.GetThemeApplication
                    Dim objdt As Data.DataTable = NawaBLL.LoginBLL.LoadAlternateData(Common.SessionTempAlternateUser.UserID)
                    StoreListUser.DataSource = objdt
                    StoreListUser.DataBind()

                Else
                    Ext.Net.X.Redirect("Logout.aspx")
                End If
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
