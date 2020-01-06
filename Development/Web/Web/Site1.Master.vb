
Partial Class Site1
    Inherits System.Web.UI.MasterPage


    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then

                If Not NawaBLL.Common.SessionCurrentUser Is Nothing Then
                    NawaBLL.AuditTrail_UserLoginBLL.AuditAccesss(Request.UserHostAddress, NawaBLL.Common.SessionCurrentUser.UserID, System.IO.Path.GetFileName(Request.Path))
                    ResourceManager1.Theme = NawaBLL.SystemParameterBLL.GetThemeApplication
                End If

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


End Class

