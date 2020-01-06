Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net

Partial Class LHBU_FileApprovalView
    Inherits Parent

    Public FileGeneration As FileGenerationBLL

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("FileApprovalView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("FileApprovalView.ObjModule") = value
        End Set
    End Property

    Public Property objListFile() As List(Of Vw_GeneratedFileList)
        Get
            If Session("FileApprovalView.objListFile") Is Nothing Then
                Dim SettingSLIK As SettingPersonal = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal

                Session("FileApprovalView.objListFile") = FileGenerationBLL.GetDataToApproveList() '.GetDataParam2(SettingSLIK.KodeCabang, SettingSLIK.Bulan, SettingSLIK.Tahun)
            End If
            Return Session("FileApprovalView.objListFile")
        End Get
        Set(ByVal value As List(Of Vw_GeneratedFileList))
            Session("FileApprovalView.objListFile") = value
        End Set
    End Property

    Public Property objFile() As Vw_GeneratedFileList
        Get
            Return Session("FileApprovalView.objFile")
        End Get
        Set(ByVal value As Vw_GeneratedFileList)
            Session("FileApprovalView.objFile") = value
        End Set
    End Property

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()

                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    gridFileList.Title = ObjModule.ModuleLabel
                    storeFileList.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    colTanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat

                    LoadData()
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ApproveAll()
        For Each item In objListFile
            item.ApprovalStatus = "APPROVED"
            item.SubmitStatus = "NOT YET SUBMITTED"
        Next

        FileGeneration = New FileGenerationBLL
        FileGeneration.SaveAllDocument(objListFile, ObjModule)

        ClearSession()
        LoadData()
    End Sub

    Protected Sub RejectAll()
        objFile = Nothing
        WindowReason.Hidden = False
        FormReason.Hidden = False
    End Sub

    Protected Sub ClearSession()
        ObjModule = Nothing
        objListFile = Nothing
        objFile = Nothing
    End Sub

    Protected Sub LoadData()
        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListFile)

        storeFileList.DataSource = objdt
        storeFileList.DataBind()
    End Sub

    Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim id As String = e.ExtraParams(0).Value
            objFile = objListFile.Find(Function(x) x.PK_GeneratedFileList_ID = id)

            If e.ExtraParams(1).Value = "Approve" Then
                objFile.ApprovalStatus = "APPROVED"
                objFile.SubmitStatus = "NOT YET SUBMITTED"

                FileGeneration = New FileGenerationBLL
                FileGeneration.SaveDocument(objFile, ObjModule)

                ClearSession()
                LoadData()
            ElseIf e.ExtraParams(1).Value = "Reject" Then
                WindowReason.Hidden = False
                FormReason.Hidden = False
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnConfirmReject_DirectClick(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If objFile Is Nothing Then
                For Each item In objListFile
                    item.ApprovalStatus = "REJECTED"
                    item.Comment = txtReason.Value
                Next

                FileGeneration = New FileGenerationBLL
                FileGeneration.SaveAllDocument(objListFile, ObjModule)

                objFile = Nothing
            Else
                objFile.ApprovalStatus = "REJECTED"
                objFile.Comment = txtReason.Value

                FileGeneration = New FileGenerationBLL
                FileGeneration.SaveDocument(objFile, ObjModule)

                objFile = Nothing
            End If

            txtReason.Value = Nothing
            WindowReason.Hidden = True
            FormReason.Hidden = True
            ClearSession()
            LoadData()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnCancelReject_DirectClick(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            txtReason.Value = Nothing
            WindowReason.Hidden = True
            FormReason.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
