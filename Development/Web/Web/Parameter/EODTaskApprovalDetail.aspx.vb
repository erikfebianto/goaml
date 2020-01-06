Imports Ext.Net
Imports NawaDevDAL
Partial Class EODTaskApprovalDetail
    Inherits Parent

#Region "Session"
    Public Property ObjApproval As NawaDAL.ModuleApproval
        Get
            Return Session("EODTaskApprovalDetail.ObjApproval")
        End Get
        Set(ByVal value As NawaDAL.ModuleApproval)
            Session("EODTaskApprovalDetail.ObjApproval") = value
        End Set
    End Property
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("EODTaskApprovalDetail.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODTaskApprovalDetail.ObjModule") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim moduleStr As String = Request.Params("ModuleID")
                Dim moduleID As Integer = NawaBLL.Common.DecryptQueryString(moduleStr, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleID)

                Dim dataStr As String = Request.Params("ID")
                Dim dataID As Long = NawaBLL.Common.DecryptQueryString(dataStr, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjApproval = NawaBLL.ModuleApprovalBLL.GetModuleApprovalByID(dataID)

                If ObjModule Is Nothing Then
                    Throw New Exception("Module not found")
                End If

                If Not ObjApproval Is Nothing Then
                    'Validasi hak akses & created by
                    If ObjApproval.CreatedBy <> NawaBLL.Common.SessionCurrentUser.UserID Then
                        If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Approval) Then
                            Dim strIDCode As String = 1
                            strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                            Exit Sub
                        End If
                    End If

                    PanelInfo.Title = ObjModule.ModuleLabel & " Approval"
                    lblModuleName.Text = ObjModule.ModuleLabel
                    lblModuleKey.Text = ObjApproval.ModuleKey
                    lblAction.Text = NawaBLL.ModuleBLL.GetModuleActionNamebyID(ObjApproval.PK_ModuleAction_ID)
                    LblCreatedBy.Text = ObjApproval.CreatedBy
                    lblCreatedDate.Text = ObjApproval.CreatedDate.Value.ToString("dd-MMM-yyyy")
                Else
                    Throw New Exception("Invalid ID Approval")
                End If

                FormPanelOld.Visible = (ObjApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update)
                BtnSave.Visible = ObjApproval.CreatedBy <> NawaBLL.Common.SessionCurrentUser.UserID
                BtnReject.Visible = ObjApproval.CreatedBy <> NawaBLL.Common.SessionCurrentUser.UserID

                Select Case ObjApproval.PK_ModuleAction_ID
                    Case NawaBLL.Common.ModuleActionEnum.Insert
                        Dim unikID As String = Guid.NewGuid.ToString
                        Dim objEODTask As New NawaDevBLL.EODTaskBLL
                        objEODTask.LoadPanel(FormPanelNew, ObjApproval.ModuleField, unikID)

                    Case NawaBLL.Common.ModuleActionEnum.Delete
                        Dim unikID As String = Guid.NewGuid.ToString
                        Dim objEODTask As New NawaDevBLL.EODTaskBLL
                        objEODTask.LoadPanel(FormPanelNew, ObjApproval.ModuleField, unikID)

                    Case NawaBLL.Common.ModuleActionEnum.Update
                        Dim unikOldID As String = Guid.NewGuid.ToString
                        Dim unikNewID As String = Guid.NewGuid.ToString

                        Dim objEODTask As New NawaDevBLL.EODTaskBLL
                        objEODTask.LoadPanel(FormPanelOld, ObjApproval.ModuleFieldBefore, unikOldID)
                        objEODTask.LoadPanel(FormPanelNew, ObjApproval.ModuleField, unikNewID)

                    Case NawaBLL.Common.ModuleActionEnum.Activation
                        Dim unikID As String = Guid.NewGuid.ToString
                        Dim objEODTask As New NawaDevBLL.EODTaskBLL
                        objEODTask.LoadPanel(FormPanelNew, ObjApproval.ModuleField, unikID)
                End Select
            End If
        Catch ex As Exception
            BtnSave.Visible = False
            BtnReject.Visible = False
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Direct Events"
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim objEODTask As New NawaDevBLL.EODTaskBLL
            objEODTask.Accept(ObjApproval.PK_ModuleApproval_ID)

            LblConfirmation.Text = "Data Approved. Click Ok to Back To " & ObjModule.ModuleLabel & " Approval."
            container.Hidden = True
            Panelconfirmation.Hidden = False
            container.Render()
            Panelconfirmation.Render()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnReject_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim objEODTask As New NawaDevBLL.EODTaskBLL
            objEODTask.Reject(ObjApproval.PK_ModuleApproval_ID)

            LblConfirmation.Text = "Data Rejected. Click Ok to Back To " & ObjModule.ModuleLabel & " Approval."
            container.Hidden = True
            Panelconfirmation.Hidden = False
            container.Render()
            Panelconfirmation.Render()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim strURL As String
            strURL = String.Format(NawaBLL.Common.GetApplicationPath & "/Parameter/WaitingApproval.aspx?ModuleID={0}", Request.Params("ModuleID"))

            If Not ObjApproval Is Nothing Then
                If ObjApproval.CreatedBy <> NawaBLL.Common.SessionCurrentUser.UserID Then
                    strURL = String.Format(NawaBLL.Common.GetApplicationPath & ObjModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID"))
                End If
            End If

            Ext.Net.X.Redirect(strURL)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try
            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & ObjModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID")))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

End Class