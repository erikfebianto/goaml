Imports Ext.Net
Partial Class ApplicationParameterApprovalDetail
    Inherits Parent

    Public Property ObjApproval As NawaDAL.ModuleApproval
        Get
            Return Session("ApplicationParameterApprovalDetail.ObjApproval")
        End Get
        Set(ByVal value As NawaDAL.ModuleApproval)
            Session("ApplicationParameterApprovalDetail.ObjApproval") = value
        End Set
    End Property
    Public Property objSchemaModule() As NawaDAL.Module
        Get
            Return Session("ApplicationParameterApprovalDetail.objSchemaModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ApplicationParameterApprovalDetail.objSchemaModule") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strModuleid As String = Request.Params("ModuleID")
                Dim moduleid As Integer = NawaBLL.Common.DecryptQueryString(strModuleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objSchemaModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                Dim strid As String = Request.Params("ID")
                Dim dataID As Long = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjApproval = NawaBLL.ModuleApprovalBLL.GetModuleApprovalByID(dataID)

                If objSchemaModule Is Nothing Then
                    Throw New Exception("Module not found")
                End If

                If Not ObjApproval Is Nothing Then
                    'Validasi hak akses & created by
                    If ObjApproval.CreatedBy <> NawaBLL.Common.SessionCurrentUser.UserID Then
                        If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objSchemaModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Approval) Then
                            Dim strIDCode As String = 1
                            strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                            Exit Sub
                        End If
                    End If

                    PanelInfo.Title = objSchemaModule.ModuleLabel & " Approval"
                    lblModuleName.Text = objSchemaModule.ModuleLabel
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
                        Dim unikkey As String = Guid.NewGuid.ToString
                        Dim objNewEODTask As New NawaBLL.SystemParameterBLL
                        'done:buat load panel applicationparameter

                    Case NawaBLL.Common.ModuleActionEnum.Delete
                        Dim unikkeyNew As String = Guid.NewGuid.ToString
                        Dim objNewMGroupAccess As New NawaBLL.SystemParameterBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)

                    Case NawaBLL.Common.ModuleActionEnum.Update
                        Dim unikkeyOld As String = Guid.NewGuid.ToString
                        Dim unikkeyNew As String = Guid.NewGuid.ToString

                        Dim objNewMGroupAccess As New NawaBLL.SystemParameterBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)
                        objNewMGroupAccess.LoadPanel(FormPanelOld, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleName, unikkeyOld)

                        'done:buat method setting color kalau terjadi perubahan
                        objNewMGroupAccess.SettingColor(FormPanelOld, FormPanelNew, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyOld, unikkeyNew)
                        'objNewMGroupAccess.SettingColor(FormPanelOld, FormPanelNew, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyOld, unikkeyNew)

                    Case NawaBLL.Common.ModuleActionEnum.Activation
                        Dim unikkeyNew As String = Guid.NewGuid.ToString
                        Dim objNewMGroupAccess As New NawaBLL.SystemParameterBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)
                End Select
            End If
        Catch ex As Exception
            BtnSave.Visible = False
            BtnReject.Visible = False
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            'done:buat method accept
            NawaBLL.SystemParameterBLL.Accept(ObjApproval.PK_ModuleApproval_ID)
            LblConfirmation.Text = "Data Approved. Click Ok to Back To " & objSchemaModule.ModuleLabel & " Approval."
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
            'done:buat method reject
            NawaBLL.SystemParameterBLL.Reject(ObjApproval.PK_ModuleApproval_ID)
            LblConfirmation.Text = "Data Rejected. Click Ok to Back To " & objSchemaModule.ModuleLabel & " Approval."
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

            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objSchemaModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID")))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try


            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objSchemaModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID")))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
End Class