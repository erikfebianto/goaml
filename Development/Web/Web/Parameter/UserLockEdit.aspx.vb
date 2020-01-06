
Partial Class UserLockEdit
    Inherits Parent
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("UserLockEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("UserLockEdit.ObjModule") = value
        End Set
    End Property
    Protected Sub btnCancel_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnBack_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As DirectEventArgs)
        Try
            If IsDataPasswordValid() Then
                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim objuserreset As NawaDAL.MUser = NawaBLL.MUserBLL.GetMuserbyPKuserId(id)

                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                    NawaDevBLL.UserLockBLL.EditDirect(objuserreset.UserID, CbInUsed.Checked, CbLockPassword.Checked)
                    lblConfirmation.Text = "User Lock Changed Successfull"
                Else
                    NawaDevBLL.UserLockBLL.EditApproval(objuserreset.UserID, CbInUsed.Checked, CbLockPassword.Checked, ObjModule)
                    lblConfirmation.Text = "User Lock Saved into Pending Approval."
                End If
                Window1.Hidden = True
                WindowConfirmation.Hidden = False
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Public Function IsDataPasswordValid() As Boolean
        Try



            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub LoadUser()
        Dim strid As String = Request.Params("ID")
        Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
        Dim objuserreset As NawaDAL.MUser = NawaBLL.MUserBLL.GetMuserbyPKuserId(id)
        If Not objuserreset Is Nothing Then
            txtUserID.Text = objuserreset.UserID
            txtUsername.Text = objuserreset.UserName
            CbInUsed.Checked = objuserreset.InUsed
            CbLockPassword.Checked = objuserreset.IsDisabled
        End If
    End Sub


    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load

        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")

                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Update) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    LoadUser()
                    Window1.Title = ObjModule.ModuleLabel
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            'objEodTask.BentukformAdd()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
End Class
