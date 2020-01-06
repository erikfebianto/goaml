
Partial Class Site3
    Inherits System.Web.UI.MasterPage


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

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                ResourceManager1.Theme = NawaBLL.SystemParameterBLL.GetThemeApplication


                If Not NawaBLL.Common.SessionCurrentUser Is Nothing Then


                End If

                'Dim omenuitem1 As New Ext.Net.MenuItem("menuitem1")
                'omenuitem1.ID = "c"




                'Dim omenu As New Ext.Net.Menu
                'omenu.ID = "b"
                'Dim omenuitem2 As New Ext.Net.MenuItem("menuitem2")
                'omenuitem2.ID = "e"



                'omenuitem1.Menu.Add(omenu)
                'omenu.Items.Add(omenuitem2)

                ''MenuPanel1.Controls.Add(obj1)
                'Dim apppath As String = Request.ApplicationPath
                'If apppath = "/" Then apppath = ""

                'NawaBLL.MGroupMenuBLL.LoadMenu(MenuPanel1, apppath)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class

