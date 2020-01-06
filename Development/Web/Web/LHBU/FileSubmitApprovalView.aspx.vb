Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Partial Class LHBU_FileSubmitApprovalView
    Inherits Parent

#Region "Session"
    Public FileGeneration As FileGenerationBLL
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("FileSubmitApprovalView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("FileSubmitApprovalView.ObjModule") = value
        End Set
    End Property
    Public Property ListFile As List(Of Vw_GeneratedFileList)
        Get
            Return Session("FileSubmitApprovalView.ListFile")
        End Get
        Set(ByVal value As List(Of Vw_GeneratedFileList))
            Session("FileSubmitApprovalView.ListFile") = value
        End Set
    End Property
    Public Property ListSelectedFile As List(Of Vw_GeneratedFileList)
        Get
            Return Session("FileSubmitApprovalView.ListSelectedFile")
        End Get
        Set(ByVal value As List(Of Vw_GeneratedFileList))
            Session("FileSubmitApprovalView.ListSelectedFile") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                ClearSession()

                Dim strmodule As String = Request.Params("ModuleID")
                Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                If ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                Else
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    gridFileList.Title = ObjModule.ModuleLabel
                    colTanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    colStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
                    colCompleteDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"

                    LoadData()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Method"
    Protected Sub ClearSession()
        ObjModule = Nothing
        ListFile = New List(Of Vw_GeneratedFileList)
        ListSelectedFile = New List(Of Vw_GeneratedFileList)
    End Sub
    Protected Sub LoadData()
        ListFile = FileGenerationBLL.GetDataSubmitToApproveList()
        Dim dataFile As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(ListFile)

        storeFileList.DataSource = dataFile
        storeFileList.DataBind()
    End Sub
    Private Function ValidateRow() As List(Of Vw_GeneratedFileList)
        Dim rowData As RowSelectionModel = TryCast(Me.gridFileList.GetSelectionModel(), RowSelectionModel)
        Dim listSelectedFile As New List(Of Vw_GeneratedFileList)

        If rowData.SelectedRows.Count > 0 Then
            For Each item As SelectedRow In rowData.SelectedRows
                listSelectedFile.Add(ListFile.Find(Function(x) x.PK_GeneratedFileList_ID = item.RecordID.ToString))
            Next
        Else
            Throw New ApplicationException("File(s) must be selected")
        End If

        Return listSelectedFile
    End Function
#End Region

#Region "Direct Events"
    Protected Sub BtnApprove_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            ListSelectedFile = ValidateRow()

            For Each item In ListSelectedFile
                item.SubmitStatus = "SUBMITTED"
            Next

            FileGeneration = New FileGenerationBLL
            FileGeneration.SaveAllDocument(ListSelectedFile, ObjModule)

            Using objDB As New NawaDatadevEntities
                For Each item In ListSelectedFile
                    Dim paramID As New Data.SqlClient.SqlParameter("@recordID", item.PK_GeneratedFileList_ID)
                    objDB.Database.ExecuteSqlCommand("EXEC usp_UpdateForm_SubmitStatus @recordID", paramID)
                Next
            End Using

            LoadData()
        Catch appEx As Exception When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub BtnReject_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            ListSelectedFile = ValidateRow()
            WindowReason.Hidden = False
            TxtReason.Value = Nothing
        Catch appEx As Exception When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnConfirmReject_OnClicked(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            For Each item In ListSelectedFile
                item.SubmitStatus = "REJECTED"
                item.SubmitNote = txtReason.Value
            Next

            FileGeneration = New FileGenerationBLL
            FileGeneration.SaveAllDocument(ListSelectedFile, ObjModule)

            WindowReason.Hidden = True
            LoadData()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnCancelReject_OnClicked(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            WindowReason.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    'Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
    '    Try
    '        Dim id As String = e.ExtraParams(0).Value
    '        Dim command As String = e.ExtraParams(1).Value

    '        ObjFile = ListFile.Find(Function(x) x.PK_GeneratedFileList_ID = id)
    '        If ObjFile Is Nothing Then
    '            Throw New Exception("Data not found")
    '        End If

    '        If command = "Approve" Then
    '            ObjFile.SubmitStatus = "SUBMITTED"

    '            FileGeneration = New FileGenerationBLL
    '            FileGeneration.SaveDocument(ObjFile, ObjModule)

    '            Using objDB As New NawaDatadevEntities
    '                Dim paramID As New Data.SqlClient.SqlParameter("@recordID", ObjFile.PK_GeneratedFileList_ID)
    '                objDB.Database.ExecuteSqlCommand("EXEC usp_UpdateForm_SubmitStatus @recordID", paramID)
    '            End Using

    '            ObjFile = Nothing
    '            LoadData()
    '        ElseIf command = "Reject" Then
    '            WindowReason.Hidden = False
    '            FormReason.Hidden = False
    '        End If
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub
#End Region

End Class
