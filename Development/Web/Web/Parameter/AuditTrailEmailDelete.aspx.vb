Partial Class AuditTrailEmailDelete
    Inherits Parent

    Private mModuleName As String

    Public Property ModulName() As String
        Get
            Return mModuleName
        End Get
        Set(ByVal value As String)
            mModuleName = value
        End Set
    End Property

    Public Property StrUnikKey() As String
        Get
            Return Session("AuditTrailEmailDelete.StrUnikKey")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEmailDelete.StrUnikKey") = value
        End Set
    End Property

    Public Property objmodule() As NawaDAL.Module
        Get
            Return Session("AuditTrailEmailDelete.objmodule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("AuditTrailEmailDelete.objmodule") = value
        End Set
    End Property

    'Public Shadows ReadOnly Property Master() As Site1
    '    Get
    '        Return DirectCast(MyBase.Master, Site1)
    '    End Get
    'End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Dim IDData As String = Request.Params("ID")
            Dim intModuleid As Integer
            Try

                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objmodule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)

                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Delete) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If
                FormPanelInput.Title = objmodule.ModuleLabel & " Delete"
                Panelconfirmation.Title = objmodule.ModuleLabel & " Delete"
                StrUnikKey = NawaBLL.Common.DecryptQueryString(IDData, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                NawaDevBLL.EmailTemplateSchedulerDetailBLL.LoadPanel(FormPanelInput, objmodule.ModuleName, StrUnikKey)
            Catch ex As Exception

            End Try

            'objFormModuleAdd.ModuleName = "MRole"

            'objFormModuleAdd.SettingFormAdd()
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

    Protected Sub Callback(sender As Object, e As DirectEventArgs)
        Try
            Select Case e.ExtraParams("command")
                Case "Delete"
                    If NawaDevBLL.EmailTemplateSchedulerDetailBLL.IsDataValidDelete(StrUnikKey, objmodule) Then

                        If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID = 1 OrElse Not objmodule.IsUseApproval Then

                            NawaDevBLL.EmailTemplateSchedulerDetailBLL.DeleteTanpaapproval(StrUnikKey, objmodule)

                            Panelconfirmation.Visible = True
                            LblConfirmation.Text = "Data Email  " & objmodule.ModuleLabel & " is deleted."
                            FormPanelInput.Hidden = True
                            Panelconfirmation.Render()
                            FormPanelInput.Render()
                        Else
                            'NawaBLL.MUserBLL.DeleteDenganapproval(StrUnikKey, objmodule)

                            'Panelconfirmation.Visible = True
                            'LblConfirmation.Text = "Data " & objmodule.ModuleLabel & " Saved into Pending Approval"
                            'FormPanelInput.Hidden = True
                            'Panelconfirmation.Render()
                            'FormPanelInput.Render()
                        End If

                    End If

            End Select
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'objFormModuleDelete = New NawaBLL.FormModuleDelete(FormPanelInput, Panelconfirmation, LblConfirmation)

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

End Class