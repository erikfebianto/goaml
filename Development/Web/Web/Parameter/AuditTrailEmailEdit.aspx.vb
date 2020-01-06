Partial Class AuditTrailEmailEdit
    Inherits Parent

    Public objEmailBLL As NawaDevBLL.EmailTemplateSchedulerDetailBLL

    Public Property ObjEmailTemplateSchedulerDetail() As NawaDevDAL.EmailTemplateSchedulerDetail
        Get
            Return Session("AuditTrailEmailEdit.ObjMUser")
        End Get
        Set(ByVal value As NawaDevDAL.EmailTemplateSchedulerDetail)
            Session("AuditTrailEmailEdit.ObjMUser") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("AuditTrailEmailEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("AuditTrailEmailEdit.ObjModule") = value
        End Set
    End Property

    Private Sub MUserAdd_Init(sender As Object, e As EventArgs) Handles Me.Init
        objEmailBLL = New NawaDevBLL.EmailTemplateSchedulerDetailBLL(FormPanelInput)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()
                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Update) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Edit"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Edit"
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            objEmailBLL.BentukformEdit()


            If Not Ext.Net.X.IsAjaxRequest Then


                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjEmailTemplateSchedulerDetail = NawaDevBLL.EmailTemplateSchedulerDetailBLL.GetEmailTemplateSchedulerbyEmailID(id)
                LoadDataUser()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub LoadDataUser()
        If Not ObjEmailTemplateSchedulerDetail Is Nothing Then
            With ObjEmailTemplateSchedulerDetail
                CType(FormPanelInput.FindControl("EmailID"), DisplayField).Text = .EmailID
                CType(FormPanelInput.FindControl("EmailTemplate"), DisplayField).Text = NawaDevBLL.EmailTemplateSchedulerDetailBLL.GetEmailTemplateNameBySchedulerID(.FK_EmailTEmplateScheduler_ID)
                CType(FormPanelInput.FindControl("EmailTo"), TextField).Text = Server.HtmlDecode(.EmailTo)
                CType(FormPanelInput.FindControl("EmailCC"), TextField).Text = .EmailCC
                CType(FormPanelInput.FindControl("EmailBCC"), TextField).Text = .EmailBCC
                CType(FormPanelInput.FindControl("EmailSubject"), TextField).Text = .EmailSubject
                CType(FormPanelInput.FindControl("EmailBody"), HtmlEditor).Text = Server.HtmlDecode(.EmailBody)
                CType(FormPanelInput.FindControl("Processdate"), DisplayField).Text = .ProcessDate.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat)
                If .SendEmailDate.HasValue Then
                    CType(FormPanelInput.FindControl("SendEmailDate"), DisplayField).Text = .SendEmailDate.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat & "HH:mm:ss")
                End If


                CType(FormPanelInput.FindControl("EmailStatus"), DisplayField).Text = NawaDevBLL.EmailTemplateSchedulerDetailBLL.GetEmailStatus(.FK_EmailStatus_ID)
                CType(FormPanelInput.FindControl("ErrorMessage"), DisplayField).Text = .ErrorMessage



            End With

        End If

    End Sub

    Sub ClearSession()
        ObjEmailTemplateSchedulerDetail = Nothing
    End Sub
    Public Function IsDataAddValid()
        Try





            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub


    Protected Sub BtnSaveAndSend_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then

                Dim DisEmailID As DisplayField = CType(FormPanelInput.FindControl("EmailID"), DisplayField)
                Dim txtEmailto As TextField = CType(FormPanelInput.FindControl("EmailTo"), TextField)
                Dim txtEmailCC As TextField = CType(FormPanelInput.FindControl("EmailCC"), TextField)
                Dim txtEmailBCC As TextField = CType(FormPanelInput.FindControl("EmailBCC"), TextField)
                Dim txtEmailSubject As TextField = CType(FormPanelInput.FindControl("EmailSubject"), TextField)
                Dim txtemailbody As HtmlEditor = CType(FormPanelInput.FindControl("EmailBody"), HtmlEditor)



                With ObjEmailTemplateSchedulerDetail
                    .EmailTo = Server.HtmlEncode(txtEmailto.Text.Trim)
                    .EmailCC = Server.HtmlEncode(txtEmailCC.Text.Trim)
                    .EmailBCC = Server.HtmlEncode(txtEmailBCC.Text.Trim)
                    .EmailSubject = Server.HtmlEncode(txtEmailSubject.Text.Trim)
                    .EmailBody = txtemailbody.Text.Trim 'Server.HtmlEncode(txtemailbody.Text.Trim)
                    .FK_EmailStatus_ID = 2  'onqueue
                    .ErrorMessage = ""
                    .retrycount = 0
                End With



                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objEmailBLL.SaveEditTanpaApproval(ObjEmailTemplateSchedulerDetail, ObjModule)

                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else

                    'objEmailBLL.SaveEditApproval(ObjEmailTemplateSchedulerDetail, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Pending Approval"
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then

                Dim DisEmailID As DisplayField = CType(FormPanelInput.FindControl("EmailID"), DisplayField)
                Dim txtEmailto As TextField = CType(FormPanelInput.FindControl("EmailTo"), TextField)
                Dim txtEmailCC As TextField = CType(FormPanelInput.FindControl("EmailCC"), TextField)
                Dim txtEmailBCC As TextField = CType(FormPanelInput.FindControl("EmailBCC"), TextField)
                Dim txtEmailSubject As TextField = CType(FormPanelInput.FindControl("EmailSubject"), TextField)
                Dim txtemailbody As HtmlEditor = CType(FormPanelInput.FindControl("EmailBody"), HtmlEditor)



                With ObjEmailTemplateSchedulerDetail
                    .EmailTo = Server.HtmlEncode(txtEmailto.Text.Trim)
                    .EmailCC = Server.HtmlEncode(txtEmailCC.Text.Trim)
                    .EmailBCC = Server.HtmlEncode(txtEmailBCC.Text.Trim)
                    .EmailSubject = Server.HtmlEncode(txtEmailSubject.Text.Trim)
                    .EmailBody = txtemailbody.Text.Trim
                    '.FK_EmailStatus_ID = 2  'onqueue
                    '.ErrorMessage = ""
                    '.retrycount = 0
                End With



                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objEmailBLL.SaveEditTanpaApproval(ObjEmailTemplateSchedulerDetail, ObjModule)

                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else

                    'objEmailBLL.SaveEditApproval(ObjEmailTemplateSchedulerDetail, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Pending Approval"
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub

End Class