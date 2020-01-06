Imports System.Data
Imports System.Drawing
Imports System.IO
Imports NawaDAL
Imports NawaBLL

<DirectMethodProxyID(IDMode:=DirectMethodProxyIDMode.Alias)> Partial Class Component_WebUserControl
    Inherits System.Web.UI.UserControl

    Private Sub Component_WebUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim data As DataTable = NawaDevBLL.GettingStartedBLL.GetGettingStarted(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID)

                Using objDB As New NawaDataEntities
                    Dim objModuleStarted As [Module] = objDB.Modules.Where(Function(x) x.ModuleName = "GettingStarted").FirstOrDefault()

                    If Not objModuleStarted Is Nothing Then
                        If NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objModuleStarted.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                            Dim rowDefault As DataRow = data.NewRow()

                            rowDefault.Item("ModuleID") = objModuleStarted.PK_Module_ID
                            rowDefault.Item("StartedName") = "Add Shortcut"
                            rowDefault.Item("ModuleAction") = NawaBLL.Common.ModuleActionEnum.Insert
                            rowDefault.Item("Urldata") = objModuleStarted.UrlAdd

                            Dim fileByte As Byte() = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory & "images/add_shortcut.png")
                            Dim fileStr As String = Convert.ToBase64String(fileByte, 0, fileByte.Length)
                            rowDefault.Item("Iconfile") = fileStr

                            data.Rows.Add(rowDefault)
                        End If
                    End If
                End Using

                Store1.DataSource = data
                Store1.DataBind()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    <DirectMethod>
    Public Sub RedirectData(data As Object)
        Dim objJObject As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(data.ToString)
        If Not objJObject Is Nothing Then
            Dim intmodulid As String = objJObject.Item("ModuleID").ToString
            Dim redirecturl As String = objJObject.Item("Urldata").ToString
            Dim originalredirecturl As String = redirecturl
            Dim strEncriptuser As String = ""
            Dim apppath As String = Request.ApplicationPath
            If apppath = "/" Then apppath = ""

            Dim objSupportEncript As SystemParameter = SystemParameterBLL.GetSystemParameterByPk(28)
            If Not objSupportEncript Is Nothing Then
                ' strEncriptuser = JsonWebToken.GetJwtToken(NawaBLL.Common.SessionCurrentUser.UserID)
            End If

            If redirecturl.Contains("?") Then
                If Not objSupportEncript Is Nothing Then
                    redirecturl = String.Format(originalredirecturl.ToLower.Replace("$userid$", strEncriptuser) & "&ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                Else
                    redirecturl = String.Format(originalredirecturl & "&ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                End If

            Else
                If Not objSupportEncript Is Nothing Then
                    redirecturl = String.Format(originalredirecturl.ToLower.Replace("$userid$", strEncriptuser) & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                Else
                    redirecturl = String.Format(originalredirecturl & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                End If
            End If

            Ext.Net.X.Redirect(apppath & redirecturl)
        Else
            Throw New Exception("There is No Setting for this Getting Started.")
        End If
    End Sub
End Class
