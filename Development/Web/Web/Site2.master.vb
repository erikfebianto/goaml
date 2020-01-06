Partial Class Site2
    Inherits System.Web.UI.MasterPage

    'Public objConn As NawaDAL.DB.Data.dbConnection

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try

            'If Not IsPostBack Then
            '    objConn = New NawaDAL.DB.Data.dbConnection(System.Configuration.ConfigurationManager.AppSettings("DbType"), System.Configuration.ConfigurationManager.ConnectionStrings("NawaDataSql").ConnectionString)
            'End If



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then

                NawaBLL.AuditTrail_UserLoginBLL.AuditAccesss(Request.UserHostAddress, NawaBLL.Common.SessionCurrentUser.UserID, System.IO.Path.GetFileName(Request.Path))


                ResourceManager1.Theme = NawaBLL.SystemParameterBLL.GetThemeApplication

                'Dim omenuitem1 As New Ext.Net.MenuItem("menuitem1")
                'omenuitem1.ID = "c"




                'Dim omenu As New Ext.Net.Menu
                'omenu.ID = "b"
                'Dim omenuitem2 As New Ext.Net.MenuItem("menuitem2")
                'omenuitem2.ID = "e"



                'omenuitem1.Menu.Add(omenu)
                'omenu.Items.Add(omenuitem2)

                ''MenuPanel1.Controls.Add(obj1)

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub
End Class