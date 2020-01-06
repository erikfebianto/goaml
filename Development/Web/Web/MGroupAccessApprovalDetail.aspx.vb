Imports Ext.Net
Public Class MGroupAccessApprovalDetail
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



    Public Property objSchemaModule() As NawaDAL.Module
        Get
            Return Session("MGroupAccessApprovalDetail.objSchemaModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MGroupAccessApprovalDetail.objSchemaModule") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim strid As String = Request.Params("ID")
            Dim strModuleid As String = Request.Params("ModuleID")

            Try

                IDReq = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim moduleid As Integer = NawaBLL.Common.DecryptQueryString(strModuleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objSchemaModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objSchemaModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If
            Catch ex As Exception
                Throw New Exception("Invalid ID Approval.")
            End Try

            If Not Ext.Net.X.IsAjaxRequest Then


                ObjApproval = NawaBLL.ModuleApprovalBLL.GetModuleApprovalByID(IDReq)
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
                        Dim unikkey As String = Guid.NewGuid.ToString

                        Dim objNewMGroupAccess As New NawaBLL.MGroupAccessBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkey)


                        FormPanelOld.Visible = False
                    Case NawaBLL.Common.ModuleActionEnum.Delete
                        Dim unikkeyNew As String = Guid.NewGuid.ToString
                        Dim objNewMGroupAccess As New NawaBLL.MGroupAccessBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)

                        FormPanelOld.Visible = False
                    Case NawaBLL.Common.ModuleActionEnum.Update
                        Dim unikkeyOld As String = Guid.NewGuid.ToString
                        Dim unikkeyNew As String = Guid.NewGuid.ToString

                        Dim objNewMGroupAccess As New NawaBLL.MGroupAccessBLL
                        objNewMGroupAccess.LoadPanel(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyNew)
                        objNewMGroupAccess.LoadPanel(FormPanelOld, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleName, unikkeyOld)



                        'NawaBLL.ModuleBLL.SettingColor(FormPanelOld, FormPanelNew, ObjApproval.ModuleFieldBefore, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkeyOld, unikkeyNew)

                    Case NawaBLL.Common.ModuleActionEnum.Activation
                        'Dim unikkey As String = Guid.NewGuid.ToString
                        'NawaBLL.ModuleBLL.LoadPanelActivation(FormPanelNew, ObjApproval.ModuleField, ObjApproval.ModuleName, unikkey)
                        'FormPanelOld.Visible = False
                End Select


            End If




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            NawaBLL.MGroupAccessBLL.Accept(Me.IDReq, "$AppPathApplication$")


            Session("DataMenu") = Nothing
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
            NawaBLL.MGroupAccessBLL.Reject(Me.IDReq)
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