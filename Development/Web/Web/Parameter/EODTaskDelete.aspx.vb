Imports Ext.Net
Partial Class EODTaskDelete
    Inherits Parent

#Region "Session"
    Public Property StrUnikKey() As String
        Get
            Return Session("EODTaskDelete.StrUnikKey")
        End Get
        Set(ByVal value As String)
            Session("EODTaskDelete.StrUnikKey") = value
        End Set
    End Property
    Public Property objmodule() As NawaDAL.Module
        Get
            Return Session("EODTaskDelete.objmodule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODTaskDelete.objmodule") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim Moduleid As String = Request.Params("ModuleID")
                Dim intModuleid As Integer = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objmodule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)

                Dim IDData As String = Request.Params("ID")
                StrUnikKey = NawaBLL.Common.DecryptQueryString(IDData, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                If Not objmodule Is Nothing Then
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Delete) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If
                Else
                    Throw New Exception("Invalid Module ID")
                End If

                FormPanelInput.Title = objmodule.ModuleLabel & " Delete"
                Panelconfirmation.Title = objmodule.ModuleLabel & " Delete"

                Dim objEODTaskBLL As New NawaDevBLL.EODTaskBLL
                objEODTaskBLL.LoadPanelDelete(FormPanelInput, objmodule.ModuleName, StrUnikKey)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Direct Events"
    Protected Sub Callback(sender As Object, e As DirectEventArgs)
        Try
            Select Case e.ExtraParams("command")
                Case "Delete"
                    Dim objEODTaskBLL As New NawaDevBLL.EODTaskBLL
                    If NawaBLL.EODTaskBLL.IsDataValidDelete(StrUnikKey, objmodule) Then
                        If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID = 1 OrElse Not objmodule.IsUseApproval Then
                            objEODTaskBLL.DeleteTanpaapproval(StrUnikKey, objmodule)
                            LblConfirmation.Text = "Data  " & objmodule.ModuleLabel & " is deleted."
                        Else
                            objEODTaskBLL.DeleteDenganapproval(StrUnikKey, objmodule)
                            LblConfirmation.Text = "Data " & objmodule.ModuleLabel & " Saved into Pending Approval"
                        End If

                        Panelconfirmation.Hidden = False
                        FormPanelInput.Hidden = True
                    End If
            End Select
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
    Private Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & objmodule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & objmodule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
#End Region

End Class
