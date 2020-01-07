Imports Ext.Net
Imports System.Data
Imports NawaDevBLL

Public Class _Default
    Inherits Parent

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then

                Dim strpath As String = Request.Params("RedirectURL")

                If strpath = "" Then
                    Panel1.Loader = New ComponentLoader()
                    Panel1.Loader.Url = "Default2.aspx"
                    Panel1.Loader.LoadMask.ShowMask = True
                    Panel1.Loader.Mode = LoadMode.Frame
                Else
                    Dim struri As String = ""
                    If HttpContext.Current.Request.IsSecureConnection Then
                        struri = "https://" & Request.UserHostAddress.Replace("::1", "localhost") & Request.ApplicationPath & strpath
                    Else
                        struri = "http://" & Request.UserHostAddress.Replace("::1", "localhost") & Request.ApplicationPath & strpath
                    End If
                    'Dim struri As String = Request.Url.ToString.Replace(Request.Path, Request.ApplicationPath & strpath)

                    Dim strModulevalue As String = ""
                    If HttpUtility.ParseQueryString(New Uri(struri).Query).GetValues("ModuleID").Length > 0 Then
                        strModulevalue = HttpUtility.ParseQueryString(New Uri(struri).Query).GetValues("ModuleID")(0)
                    End If

                    Dim strRedirect As String
                    If Not strModulevalue Is Nothing Then
                        Dim strencriptmodule As String = NawaBLL.Common.EncryptQueryString(strModulevalue, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                        Dim type As String

                        If Not HttpUtility.ParseQueryString(New Uri(struri).Query).GetValues("Type") Is Nothing Then
                            type = HttpUtility.ParseQueryString(New Uri(struri).Query).GetValues("Type")(0)
                        End If

                        'If type = "ValidReport" Then
                        '    Dim id As String = HttpUtility.ParseQueryString(New Uri(struri).Query).GetValues("PK_ID")(0)
                        '    Dim objData = ValidationReportBLL.GetDataByID(id)

                        '    Dim recordID As String = NawaBLL.Common.EncryptQueryString(objData.RecordID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                        '    Dim tableName As String = "Web_ValidationReport_RowComplate"
                        '    Dim fieldID As String = "PK_ID"
                        '    Dim fieldValue As String = id
                        '    Dim msgField As String = "Messagedetail"
                        '    Dim intModuleBack As String = NawaBLL.Common.EncryptQueryString(strModulevalue, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                        '    Dim actionBack As String = NawaBLL.SystemParameterBLL.ModuleAction.View

                        '    strRedirect = String.Format(objData.ModuleURL & "?ID={0}&ModuleID={1}&Table={2}&PKName={3}&PKValue={4}&MsgField={5}&ModuleBack={6}&actionback={7}", recordID, strencriptmodule, tableName, fieldID, fieldValue, msgField, intModuleBack, actionBack)
                        'Else
                        '    strRedirect = strpath.Replace("moduleid=" & strModulevalue, "moduleid=" & strencriptmodule)
                        'End If

                        Panel1.Loader = New ComponentLoader()
                        Panel1.Loader.Url = "~" & strRedirect
                        Panel1.Loader.LoadMask.ShowMask = True
                        Panel1.Loader.Mode = LoadMode.Frame
                    End If
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub




    <DirectMethod>
    Public Sub RedirectLogout()
        Ext.Net.X.Redirect("~/Logout.aspx")

    End Sub
End Class