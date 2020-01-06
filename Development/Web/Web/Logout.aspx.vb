
Partial Class Logout
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not Ext.Net.X.IsAjaxRequest Then
                NawaBLL.AuditTrail_UserLoginBLL.Logout(Request.UserHostAddress, NawaBLL.Common.SessionCurrentUser.UserID)
                Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Me.Session.Abandon()
                Me.Request.Cookies.Clear()
                System.Web.Security.FormsAuthentication.SignOut()

            End If

        Catch ex As Exception

        End Try


    End Sub
End Class
