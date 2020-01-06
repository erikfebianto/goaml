Imports Ext.Net
Partial Class ParameterApprovalDetail
    Inherits Parent




    Private _IDReq As Long
    Public Property IDReq() As Long
        Get
            Return _IDReq
        End Get
        Set(ByVal value As Long)
            _IDReq = value
        End Set
    End Property

    Private _objApproval As NawaDAL.ModuleApproval
    Public Property ObjApproval() As NawaDAL.ModuleApproval
        Get
            Return _objApproval
        End Get
        Set(ByVal value As NawaDAL.ModuleApproval)
            _objApproval = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not NawaBLL.Common.SessionCurrentUser Is Nothing Then

                Dim strid As String = Request.Params("ID")
                Dim strModuleid As String = Request.Params("ModuleID")

                Try

                    IDReq = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                Catch ex As Exception
                    Throw New Exception("Invalid ID Approval.")
                End Try


                ObjApproval = NawaBLL.ModuleApprovalBLL.GetModuleApprovalByID(IDReq)
                ValidateSecurity()

                If Not Ext.Net.X.IsAjaxRequest Then



                    If Not ObjApproval Is Nothing Then
                        PanelInfo.Title = ObjApproval.ModuleName & " Approval"
                        lblModuleName.Text = ObjApproval.ModuleName
                        lblModuleKey.Text = ObjApproval.ModuleKey
                        lblAction.Text = NawaBLL.ModuleBLL.GetModuleActionNamebyID(ObjApproval.PK_ModuleAction_ID)
                        LblCreatedBy.Text = ObjApproval.CreatedBy
                        lblCreatedDate.Text = ObjApproval.CreatedDate.Value.ToString("dd-MMM-yyyy")

                    End If

                    Select Case ObjApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert
                            NawaBLL.FormModuleApprovalDetail.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, IDReq, "new")
                            FormPanelOld.Hidden = True
                        Case NawaBLL.Common.ModuleActionEnum.Delete
                            NawaBLL.FormModuleApprovalDetail.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, IDReq, "new")
                            FormPanelOld.Hidden = True
                        Case NawaBLL.Common.ModuleActionEnum.Update
                            NawaBLL.FormModuleApprovalDetail.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, IDReq, "new")
                            NawaBLL.FormModuleApprovalDetail.LoadPanel(FormPanelOld, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleName, IDReq, "old")

                        Case NawaBLL.Common.ModuleActionEnum.Activation
                            NawaBLL.FormModuleApprovalDetail.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, IDReq, "new")
                            FormPanelOld.Hidden = True
                            'Case NawaBLL.Common.ModuleActionEnum.Upload

                            '    Dim objapp As New NawaBLL.FormModuleApprovalDetail

                            '    objapp.LoadPanelUpload(FormPanelNew, ObjApproval, ObjApproval.ModuleName)
                            '    FormPanelOld.Visible = False
                    End Select





                    If ObjApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Upload Then
                        Dim objapp As New NawaBLL.FormModuleApprovalDetail

                        objapp.LoadPanelUpload(FormPanelNew, ObjApproval, ObjApproval.ModuleName)
                        FormPanelOld.Hidden = True
                    End If

                End If
            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub Lempar()

        Dim strIDCode As String = 1
        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)
        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")

    End Sub
    Sub ValidateSecurity()
        If Not ObjApproval Is Nothing Then
            If ObjApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID Then
                'Lempar()
                BtnSave.Hidden = True
                BtnReject.Hidden = True
                BtnCancel.Text = "Back"
            End If

            Dim objuser As NawaDAL.MUser = NawaBLL.MUserBLL.GetMuserbyUSerId(ObjApproval.CreatedBy)
            If objuser Is Nothing Then
                Lempar()
            Else
                If objuser.FK_MRole_ID <> NawaBLL.Common.SessionCurrentUser.FK_MRole_ID Then
                    Lempar()
                End If
            End If

        End If

    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            NawaBLL.FormModuleApprovalDetail.Accept(Me.IDReq)



            LblConfirmation.Text = "Data Approved. Click Ok to Back To Module Approval."
            Panelconfirmation.Hidden = False
            container.Hidden = True

            'container.Render()
            'Panelconfirmation.Render()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
    Protected Sub BtnReject_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            NawaBLL.FormModuleApprovalDetail.Reject(Me.IDReq)
            LblConfirmation.Text = "Data Rejected. Click Ok to Back To Module Approval."
            Panelconfirmation.Hidden = False
            container.Hidden = True

            'container.Render()
            'Panelconfirmation.Render()

            
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If BtnSave.Hidden = True Then
                Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/Parameter/WaitingApproval.aspx?ModuleID={0}", Request.Params("ModuleID")))
            Else
                Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/Parameter/ParameterApproval.aspx?ModuleID={0}", Request.Params("ModuleID")))
            End If



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try


            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/Parameter/ParameterApproval.aspx?ModuleID={0}", Request.Params("ModuleID")))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
End Class