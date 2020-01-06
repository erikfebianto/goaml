Imports Ext.Net
Partial Class MUserApprovalDetail
    Inherits Parent

    Public Property AuthenticationMode() As Integer
        Get
            Return Session("MUserApprovalDetail.AuthenticationMode")
        End Get
        Set(ByVal value As Integer)
            Session("MUserApprovalDetail.AuthenticationMode") = value
        End Set
    End Property
    Public Property ObjApproval() As NawaDAL.ModuleApproval
        Get
            Return Session("MUserApprovalDetail.ObjApproval")
        End Get
        Set(ByVal value As NawaDAL.ModuleApproval)
            Session("MUserApprovalDetail.ObjApproval") = value
        End Set
    End Property
    Public Property objSchemaModule() As NawaDAL.Module
        Get
            Return Session("MUserApprovalDetail.objSchemaModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MUserApprovalDetail.objSchemaModule") = value
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

                Dim objsystemparameter As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3)
                Dim intauthentication As Integer = 2 'default formauthentication=2
                If Not objsystemparameter Is Nothing Then
                    intauthentication = objsystemparameter.SettingValue
                End If
                AuthenticationMode = intauthentication

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
                        Dim objNewMuser As New NawaBLL.MUserBLL
                        objNewMuser.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkey)

                    Case NawaBLL.Common.ModuleActionEnum.Delete
                        Dim unikkeyNew As String = Guid.NewGuid.ToString
                        Dim objNewMuser As New NawaBLL.MUserBLL
                        objNewMuser.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)

                    Case NawaBLL.Common.ModuleActionEnum.Update
                        Dim unikkeyOld As String = Guid.NewGuid.ToString
                        Dim unikkeyNew As String = Guid.NewGuid.ToString

                        Dim objNewMuser As New NawaBLL.MUserBLL
                        objNewMuser.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)
                        objNewMuser.LoadPanel(FormPanelOld, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleName, unikkeyOld)
                        objNewMuser.SettingColor(FormPanelOld, FormPanelNew, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyOld, unikkeyNew)

                    Case NawaBLL.Common.ModuleActionEnum.Activation
                        Dim unikkeyNew As String = Guid.NewGuid.ToString
                        Dim objNewMuser As New NawaBLL.MUserBLL
                        objNewMuser.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)
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
            Dim objUser As NawaDAL.MUser = NawaBLL.Common.Deserialize(ObjApproval.ModuleField, GetType(NawaDAL.MUser))
            Dim struserid = objUser.UserID

            NawaBLL.MUserBLL.Accept(ObjApproval.PK_ModuleApproval_ID)

            If AuthenticationMode = 1 Then
                Select Case ObjApproval.PK_ModuleAction_ID
                    Case NawaBLL.Common.ModuleActionEnum.Insert, NawaBLL.Common.ModuleActionEnum.Update
                        Dim osqlparam(0) As System.Data.SqlClient.SqlParameter
                        osqlparam(0) = New Data.SqlClient.SqlParameter
                        osqlparam(0).ParameterName = "@userid"
                        osqlparam(0).Value = struserid

                        NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.StoredProcedure, "Usp_InsertMsEmployeeAccept", osqlparam)
                End Select
            End If

            LblConfirmation.Text = "Data Approved. Click Ok to Back To Module Approval."
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
            NawaBLL.MUserBLL.Reject(ObjApproval.PK_ModuleApproval_ID)
            LblConfirmation.Text = "Data Rejected. Click Ok to Back To Module Approval."
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
                    strURL = String.Format(NawaBLL.Common.GetApplicationPath & objSchemaModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID"))
                End If
            End If

            Ext.Net.X.Redirect(strURL)
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