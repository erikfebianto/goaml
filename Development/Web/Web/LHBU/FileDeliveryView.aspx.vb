Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports System.IO
Imports OfficeOpenXml
Imports Ionic.Zip

Partial Class LHBU_FileDeliveryView
    Inherits Parent

#Region "Session"
    Public ObjFileGenBLL As FileGenerationBLL
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("FileDeliveryView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("FileDeliveryView.ObjModule") = value
        End Set
    End Property
    Public Property ListTextFile As List(Of Vw_GeneratedFileList)
        Get
            Return Session("FileDeliveryView.ListTextFile")
        End Get
        Set(ByVal value As List(Of Vw_GeneratedFileList))
            Session("FileDeliveryView.ListTextFile") = value
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

                    GridFileList.Title = ObjModule.ModuleLabel
                    StoreFileList.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    colPeriodeLaporan.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    colStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
                    colCompleteDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
                    ListTextFile = FileGenerationBLL.GetDataList()

                    LoadGridData()
                    CheckApproval()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Method"
    Private Sub ClearSession()
        ObjModule = Nothing
        ListTextFile = New List(Of Vw_GeneratedFileList)
    End Sub

    Private Sub LoadGridData()
        Dim data As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(ListTextFile)

        StoreFileList.DataSource = data
        StoreFileList.DataBind()
    End Sub
    Private Sub CheckApproval()
        BtnRequestSubmit.Hidden = Not ObjModule.IsUseApproval
        'If Not ObjModule.IsUseApproval Then
        '    btnRequestSubmit.Hidden = True
        '    btnRequestDownload.Hidden = True
        '    'commands.Hidden = True
        'Else
        '    btnRequestSubmit.Hidden = False
        '    btnRequestDownload.Hidden = False
        '    'commands.Hidden = False
        'End If
    End Sub
    Private Function ValidateRow() As List(Of Vw_GeneratedFileList)
        Dim rowData As RowSelectionModel = TryCast(Me.GridFileList.GetSelectionModel(), RowSelectionModel)
        Dim listSelectedFile As New List(Of Vw_GeneratedFileList)

        If rowData.SelectedRows.Count > 0 Then
            For Each item As SelectedRow In rowData.SelectedRows
                listSelectedFile.Add(ListTextFile.Find(Function(x) x.PK_GeneratedFileList_ID = item.RecordID.ToString))
            Next
        Else
            Throw New ApplicationException("File(s) must be selected")
        End If

        Return listSelectedFile
    End Function
#End Region

#Region "Direct Events"
    Protected Sub BtnDownloadZip_OnClicked(sender As Object, e As EventArgs)
        Try
            Dim listSelectedFile As New List(Of Vw_GeneratedFileList)
            listSelectedFile = ValidateRow()

            Dim dirPath As String = Server.MapPath("~/Temp")
            dirPath = dirPath & "\TextFile_" & Now.ToString("yyyy-MMM-dd-HHmmss")
            If IO.Directory.Exists(dirPath) Then
                IO.Directory.Delete(dirPath, True)
            End If
            IO.Directory.CreateDirectory(dirPath)

            Dim listFilePath As New List(Of String)
            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
                For Each item In listSelectedFile
                    Dim fileTxt As String = dirPath & "\" & item.FileName
                    Dim objFileDownload As GeneratedFileList = objDb.GeneratedFileLists.ToList.Where(Function(x) x.PK_GeneratedFileList_ID = item.PK_GeneratedFileList_ID).FirstOrDefault

                    If objFileDownload Is Nothing Then
                        objFileDownload = New GeneratedFileList
                    End If

                    If IO.File.Exists(fileTxt) Then
                        IO.File.Delete(fileTxt)
                    End If

                    If item.SubmitStatus = "SUBMITTED" Then
                        fileTxt = fileTxt & " [Submitted_" & String.Format("{0:yyyy-MM-dd_HHmm}", item.CompleteDate) & "]"
                    End If

                    fileTxt = fileTxt & item.FileType
                    IO.File.WriteAllBytes(fileTxt, objFileDownload.FileBin)
                    listFilePath.Add(fileTxt)
                Next
            End Using

            Dim fileNamezip As String = dirPath & "\TextFile_" & Now.ToString("yyyyMMdd_HHmmss") & ".zip"
            Using zip As New ZipFile()
                'Dim zipFolder As String = "TextFile_" & Now.ToString("yyyyMMdd")
                'zip.AddDirectoryByName(zipFolder)

                If IO.File.Exists(fileNamezip) Then
                    IO.File.Delete(fileNamezip)
                End If
                zip.CompressionLevel = CompressionLevel.Level9
                zip.AddFiles(listFilePath, "")
                zip.Save(fileNamezip)
            End Using

            Dim fileContents As Byte()
            fileContents = My.Computer.FileSystem.ReadAllBytes(fileNamezip)

            If IO.Directory.Exists(dirPath) Then
                IO.Directory.Delete(dirPath, True)
            End If

            Context.Response.Clear()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-Disposition", "attachment;filename=" & IO.Path.GetFileName(fileNamezip))
            Context.Response.Charset = Encoding.UTF8.WebName
            Context.Response.ContentEncoding = Encoding.UTF8
            Context.Response.AddHeader("cache-control", "max-age=0")
            Context.Response.ContentType = "text/plain"
            Context.Response.BinaryWrite(fileContents)
            Context.Response.End()

        Catch appEx As Exception When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnRequestSubmit_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            Dim listSelectedFile As New List(Of Vw_GeneratedFileList)
            listSelectedFile = ValidateRow()

            Dim sb As StringBuilder = New StringBuilder
            For Each item In listSelectedFile
                If item.SubmitStatus = "NOT YET SUBMITTED" Then
                    item.SubmitStatus = "WAITING APPROVAL"
                    sb.Append(
                        String.Format("Tanggal Data {0}, File <b>{1}</b> is waiting for approval<br>",
                                      item.PeriodeLaporan.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat),
                                      item.FileName))
                End If
            Next

            ObjFileGenBLL = New FileGenerationBLL
            ObjFileGenBLL.SaveAllDocument(listSelectedFile, ObjModule)
            LoadGridData()

            If sb.Length > 0 Then
                Ext.Net.X.Msg.Alert("Information", sb.ToString).Show()
            Else
                sb.Append("No file(s) processed")
                sb.Append("<br>Only file with submit status NOT YET SUBMITTED will be processed")
                Throw New ApplicationException(sb.ToString)
            End If
        Catch appEx As Exception When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim id As String = e.ExtraParams(0).Value
            If e.ExtraParams(1).Value = "Request" Then
                Dim objFile As Vw_GeneratedFileList = ListTextFile.Find(Function(x) x.PK_GeneratedFileList_ID = id)
                If objFile.ApprovalStatus = "DONE" Then
                    objFile.ApprovalStatus = "WAITING APPROVAL"
                End If

                ObjFileGenBLL = New FileGenerationBLL
                ObjFileGenBLL.SaveDocument(objFile, ObjModule)

                LoadGridData()
            ElseIf e.ExtraParams(1).Value = "Download" Then
                Dim objFile As Vw_GeneratedFileList = ListTextFile.Find(Function(x) x.PK_GeneratedFileList_ID = id)
                Dim RecordID As String = NawaBLL.Common.EncryptQueryString(id, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                'Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ID={0}&ModuleID={1}", RecordID, Moduleid), "Loading")
                If ObjModule.IsUseApproval = False Or (ObjModule.IsUseApproval = True And objFile.ApprovalStatus = "APPROVED") Then
                    Response.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/LHBU/FileDownload.ashx" & "?ID={0}&Val={1}", id, ObjModule.IsUseApproval))
                Else
                    Throw New Exception("Approval Required")
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    'Protected Sub RequestDownload()
    '    Dim sm As RowSelectionModel = TryCast(Me.gridFileList.GetSelectionModel(), RowSelectionModel)
    '    Dim selectedItems As List(Of Vw_GeneratedFileList) = New List(Of Vw_GeneratedFileList)

    '    If sm.SelectedRows.Count > 0 Then
    '        For Each item As SelectedRow In sm.SelectedRows
    '            selectedItems.Add(ListTextFile.Find(Function(x) x.PK_GeneratedFileList_ID = item.RecordID.ToString))
    '        Next
    '    End If

    '    Dim sb As StringBuilder = New StringBuilder
    '    For Each item In selectedItems
    '        If item.ApprovalStatus = "" Then
    '            item.ApprovalStatus = "WAITING APPROVAL"
    '            sb.Append(
    '                String.Format("Tanggal Data {0}, Form {1} waiting for approval<br>",
    '                              item.PeriodeLaporan.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat),
    '                              item.FK_LHBU_FormInfo_ID))
    '        End If
    '    Next

    '    FileGeneration = New FileGenerationBLL
    '    FileGeneration.SaveAllDocument(selectedItems, ObjModule)
    '    LoadData()

    '    If sb.Length > 0 Then
    '        Ext.Net.X.Msg.Alert("Info", sb.ToString).Show()
    '    Else
    '        Ext.Net.X.Msg.Alert("Info", "No item processed").Show()
    '    End If


    'End Sub
#End Region

End Class
