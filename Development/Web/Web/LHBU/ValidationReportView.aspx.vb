Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports OfficeOpenXml
Imports Ionic.Zip
Imports System.Data

Partial Class SLIK_ValidationReportView
    Inherits Parent
    Public objFormModuleView As NawaBLL.FormModuleView
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("ValidationReportView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ValidationReportView.ObjModule") = value
        End Set
    End Property

    Public Property strWhereClause() As String
        Get
            Return Session("ValidationReportView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("ValidationReportView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("ValidationReportView.strSort")
        End Get
        Set(ByVal value As String)
            Session("ValidationReportView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("ValidationReportView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("ValidationReportView.indexStart") = value
        End Set
    End Property

    Public Property ObjData() As NawaDevDAL.Web_ValidationReport_RowComplate
        Get
            Return Session("ValidationReportView.ObjData")
        End Get
        Set(ByVal value As NawaDevDAL.Web_ValidationReport_RowComplate)
            Session("ValidationReportView.ObjData") = value
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

                    GridpanelView.Title = "Invalid Validation Details"
                    colTanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    StoreView.PageSize = NawaBLL.SystemParameterBLL.GetPageSize

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try

                Dim SegmenData As String = Request.Params("Segmen")
                If SegmenData IsNot Nothing And SegmenData <> "" Then
                    SegmenData = SegmenData.Replace("_", " ")

                    Ext.Net.X.Js.Call("FilterSegmen", SegmenData)
                End If
            End If

            If Not Ext.Net.X.IsAjaxRequest Then


                Session("Component_AdvancedFilter.AdvancedFilterData") = Nothing
                Session("Component_AdvancedFilter.AdvancedFilterDataQuery") = Nothing
                Session("Component_AdvancedFilter.objTableFilter") = Nothing
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Protected Sub btnClear_Click(sender As Object, e As DirectEventArgs)
        Try
            Session("Component_AdvancedFilter.AdvancedFilterData") = Nothing
            Session("Component_AdvancedFilter.AdvancedFilterDataQuery") = Nothing
            Session("Component_AdvancedFilter.objTableFilter") = Nothing
            Toolbar2.Hidden = True
            StoreView.Reload()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ClearSession()
        ObjModule = Nothing
        ObjData = Nothing
    End Sub

    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
        objFormModuleView = New NawaBLL.FormModuleView(Me.GridpanelView, Nothing)
    End Sub

    Protected Sub StoreView_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try

            If Session("Component_AdvancedFilter.AdvancedFilterData") = "" Then
                Toolbar2.Hidden = True
            Else
                Toolbar2.Hidden = False
            End If
            LblAdvancedFilter.Text = Session("Component_AdvancedFilter.AdvancedFilterData")



            Dim intStart As Integer = e.Start
            'If intStart = 0 Then intStart = 1
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objFormModuleView.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            Me.indexStart = intStart
            Me.strWhereClause = strfilter

            Dim objSetting As SettingPersonal = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal
            If Not objSetting Is Nothing Then

                If Not IsDBNull(objSetting.ReportDate) Or objSetting.ReportDate <> DateTime.MinValue Then
                    If strWhereClause <> "" Then
                        Me.strWhereClause = Me.strWhereClause & " AND "
                    End If

                    Me.strWhereClause &= " TanggalData = '" & NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.ReportDate.ToString("yyyyMMdd") & "' "
                End If

                If Not String.IsNullOrWhiteSpace(objSetting.KodeCabang) And objSetting.KodeCabang <> "All" Then
                    If strWhereClause <> "" Then
                        Me.strWhereClause = Me.strWhereClause & " AND "
                    End If

                    Me.strWhereClause &= " KodeCabang = '" & NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang & "' "
                End If
            End If

            If strWhereClause.Length > 0 Then
                If Not Session("Component_AdvancedFilter.AdvancedFilterDataQuery") = "" Then
                    strWhereClause &= "and ( " & Session("Component_AdvancedFilter.AdvancedFilterDataQuery") & ")"
                End If
            Else
                If Not Session("Component_AdvancedFilter.AdvancedFilterDataQuery") = "" Then
                    strWhereClause &= " (" & Session("Component_AdvancedFilter.AdvancedFilterDataQuery") & ")"
                End If
            End If

            Me.strOrder = strsort
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strWhereClause, strsort, intStart, intLimit, inttotalRecord)

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("Web_ValidationReport_RowComplate A ", "PK_ID, A.RecordID, A.SegmentData, A.TanggalData, A.KodeCabang, A.KeyField, KeyFieldValue, a.MessageDetailView, a.Edited, a.ReferenceField, a.ReferenceValue", strWhereClause, strsort, intStart, intLimit, inttotalRecord)
            ''-- start paging ------------------------------------------------------------
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If
            'Dim rangeData As List(Of Object) = If((e.Start < 0 OrElse limit < 0), data, data.GetRange(e.Start, limit))
            ''-- end paging ------------------------------------------------------------
            e.Total = inttotalRecord
            GridpanelView.GetStore.DataSource = DataPaging
            GridpanelView.GetStore.DataBind()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If e.ExtraParams(1).Value = "Edit" Then

                Dim id As String = e.ExtraParams(0).Value

                ObjData = ValidationReportBLL.GetDataByID(id)



                Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(ObjData.ModuleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim RecordID As String = NawaBLL.Common.EncryptQueryString(ObjData.RecordID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim TableName As String = "Web_ValidationReport_RowComplate"
                Dim IDField As String = "PK_ID"
                Dim IDValue As String = e.ExtraParams(0).Value.ToString()
                Dim MsgField As String = "Messagedetail"
                Dim intmoduleback As String = NawaBLL.Common.EncryptQueryString(ObjModule.PK_Module_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim actionback As String = NawaBLL.SystemParameterBLL.ModuleAction.View

                Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & ObjData.ModuleURL & "?ID={0}&ModuleID={1}&Table={2}&PKName={3}&PKValue={4}&MsgField={5}&ModuleBack={6}&actionback={7}", RecordID, Moduleid, TableName, IDField, IDValue, MsgField, intmoduleback, actionback), "Loading")

                'Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & ObjData.ModuleURL & "?ID={0}&ModuleID={1}&table={2}&pkname={3}&pkvalue={4}", RecordID, Moduleid, TableName,), "Loading")
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub




    Protected Sub ExportExcelALLRow(sender As Object, e As EventArgs)
        Try
            Dim dirPath As String = Server.MapPath("~/Temp")
            dirPath = dirPath & "\" & NawaBLL.Common.SessionCurrentUser.UserID & "\"
            If Not IO.Directory.Exists(dirPath) Then
                IO.Directory.CreateDirectory(dirPath)
            End If

            Dim lstFile As New List(Of String)

            Dim objdt As Data.DataTable = ValidationReportBLL.GetSegmentDataToExportALLByRow(strWhereClause)

            For Each item As Data.DataRow In objdt.Rows
                Dim modulename As String = item("ModuleName")

                Dim objreportexcel As New NawaDevBLL.ValidationReportBLL
                objreportexcel.TableValidationReport = "Web_ValidationReport_RowComplate"
                objreportexcel.TableValidationReportPKName = "PK_ID"
                objreportexcel.ModuleName = modulename



                Dim strcount As String = objreportexcel.GenerateCountSqlImportDataWithErrorMessageALL(strWhereClause)

                Dim objdtcount As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strcount, Nothing)
                Dim inttotalrow As Long = objdtcount.Rows(0)(0)

                Dim intmaxrowerror As Long = 100000
                Dim objsystemparametermaxrowerror As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9011)
                If Not objsystemparametermaxrowerror Is Nothing Then
                    intmaxrowerror = Convert.ToInt64(objsystemparametermaxrowerror.SettingValue)

                End If
                If inttotalrow > intmaxrowerror Then
                    Throw New Exception("Maximum Record to export is " & intmaxrowerror & ". There is " & inttotalrow)
                End If

                Dim pagesize As Integer = 1048575
                Dim totalPage As Integer = Math.Ceiling(inttotalrow / pagesize)


                For indexpage As Integer = 1 To totalPage

                    Dim strFileName As String = objreportexcel.CreateExcel2007WithDataWithErrorMessageALLRow(dirPath, strWhereClause, indexpage, pagesize)


                    Dim fileNamexls As String = dirPath & "\" & modulename & indexpage & ".xlsx"
                    'delete
                    If IO.File.Exists(fileNamexls) Then
                        IO.File.Delete(fileNamexls)
                    End If

                    'copy
                    IO.File.Copy(strFileName, fileNamexls)
                    lstFile.Add(fileNamexls)
                    If IO.File.Exists(strFileName) Then
                        IO.File.Delete(strFileName)
                    End If
                Next



            Next
            Dim fileNamezip As String = dirPath & "\InvalidReport.zip"
            Using zip As New ZipFile()
                If IO.File.Exists(fileNamezip) Then
                    IO.File.Delete(fileNamezip)
                End If
                zip.CompressionLevel = CompressionLevel.Level9
                zip.AddFiles(lstFile, "")
                zip.Save(fileNamezip)
            End Using


            Dim fileContents As Byte()
            fileContents = My.Computer.FileSystem.ReadAllBytes(fileNamezip)


            Response.Clear()
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=" & IO.Path.GetFileName(fileNamezip))
            Response.Charset = ""
            Response.AddHeader("cache-control", "max-age=0")
            Me.EnableViewState = False

            Response.ContentType = "application/x-compressed"
            Response.BinaryWrite(fileContents)
            Response.End()
            'Else
            '    Ext.Net.X.Msg.Alert("Error", "Please Select at least one item to export.").Show()
            'End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    <DirectMethod>
    Public Sub BtnAdvancedFilter_Click()
        Try

            Dim Moduleid As String = Request.Params("ModuleID")

            Dim intModuleid As Integer = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            Dim objmodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)

            AdvancedFilter1.TableName = "Web_ValidationReport_RowComplate"
            AdvancedFilter1.objListModuleField = Nothing

            Dim objwindow As Ext.Net.Window = Ext.Net.X.GetCmp("WindowFilter")
            objwindow.Hidden = False
            AdvancedFilter1.BindData()
            ' AdvancedFilter1.ClearFilter()
            AdvancedFilter1.StoreToReleoad = "StoreView"




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub ExportExcelSelectedByField(sender As Object, e As EventArgs)
        Try
            Dim dirPath As String = Server.MapPath("~/Temp")
            dirPath = dirPath & "\" & NawaBLL.Common.SessionCurrentUser.UserID & "\"
            If Not IO.Directory.Exists(dirPath) Then
                IO.Directory.CreateDirectory(dirPath)
            End If


            Dim sm As RowSelectionModel = TryCast(Me.GridpanelView.GetSelectionModel(), RowSelectionModel)
            If sm.SelectedRows.Count > 0 Then

                '  Dim SelectionID As ArrayList = New ArrayList
                Dim strfilter As String = ""
                Dim strfilteritem As String = ""
                For Each item As SelectedRow In sm.SelectedRows
                    strfilter += item.RecordID & ","

                Next

                If strfilter.Length > 0 Then
                    strfilter = strfilter.Substring(0, Len(strfilter) - 1)
                    strfilter = " where PK_ID in (" & strfilter & ") and " & strWhereClause


                End If

                Dim strpktoprocess As String = ValidationReportBLL.GetPKToExport(strfilter)

                If strpktoprocess <> "" Then
                    strpktoprocess = "   recordid in( " & strpktoprocess & ")"
                End If
                Dim lstFile As New List(Of String)

                strpktoprocess = strpktoprocess & " and " & strWhereClause

                Dim objdt As Data.DataTable = ValidationReportBLL.GetSegmentDataToExport(strfilter)
                For Each item As Data.DataRow In objdt.Rows


                    Dim modulename As String = item("ModuleName")

                    Dim objreportexcel As New NawaDevBLL.ValidationReportBLL
                    objreportexcel.TableValidationReport = "Web_ValidationReport_RowComplate"
                    objreportexcel.TableValidationReportPKName = "PK_ID"

                    Dim strFileName As String = objreportexcel.CreateExcel2007WithData(dirPath, strpktoprocess, modulename)
                    Dim fileNamexls As String = dirPath & modulename & ".xlsx"
                    'delete
                    If IO.File.Exists(fileNamexls) Then
                        IO.File.Delete(fileNamexls)
                    End If

                    'copy
                    IO.File.Copy(strFileName, fileNamexls)
                    lstFile.Add(fileNamexls)
                    If IO.File.Exists(strFileName) Then
                        IO.File.Delete(strFileName)
                    End If





                Next

                Dim fileNamezip As String = dirPath & "\InvalidReport.zip"
                Using zip As New ZipFile()


                    If IO.File.Exists(fileNamezip) Then
                        IO.File.Delete(fileNamezip)
                    End If
                    zip.CompressionLevel = CompressionLevel.Level9
                    zip.AddFiles(lstFile, "")
                    zip.Save(fileNamezip)

                End Using


                Dim fileContents As Byte()
                fileContents = My.Computer.FileSystem.ReadAllBytes(fileNamezip)


                Response.Clear()
                Response.ClearHeaders()
                Response.AddHeader("content-disposition", "attachment;filename=" & IO.Path.GetFileName(fileNamezip))
                Response.Charset = ""
                Response.AddHeader("cache-control", "max-age=0")
                Me.EnableViewState = False

                Response.ContentType = "application/x-compressed"
                Response.BinaryWrite(fileContents)
                Response.End()
            Else
                Ext.Net.X.Msg.Alert("Error", "Please Select at least one item to export.").Show()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Protected Sub ExportExcelALLByField(sender As Object, e As EventArgs)
        Try
            Dim dirPath As String = Server.MapPath("~/Temp")
            dirPath = dirPath & "\" & NawaBLL.Common.SessionCurrentUser.UserID & "\"
            If Not IO.Directory.Exists(dirPath) Then
                IO.Directory.CreateDirectory(dirPath)
            End If




            'If strfilter.Length > 0 Then
            '    strfilter = strfilter.Substring(0, Len(strfilter) - 1)
            '    strfilter = " where PK_ID in (" & strfilter & ")"


            'End If

            'Dim strpktoprocess As String = ValidationReportBLL.GetPKToExport(strfilter)

            'If strpktoprocess <> "" Then
            '    strpktoprocess = "   recordid in( " & strpktoprocess & ")"
            'End If
            Dim lstFile As New List(Of String)



            Dim objdt As Data.DataTable = ValidationReportBLL.GetSegmentDataToExportALLByRow(strWhereClause)
            For Each item As Data.DataRow In objdt.Rows


                Dim modulename As String = item("ModuleName")

                Dim objreportexcel As New NawaDevBLL.ValidationReportBLL
                objreportexcel.TableValidationReport = "Web_ValidationReport_RowComplate"
                objreportexcel.TableValidationReportPKName = "PK_ID"



                ' Dim strcount As String = objreportexcel.GenerateCountSqlImportDataFieldALL(strWhereClause)

                'Dim objdtcount As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strcount, Nothing)
                'Dim inttotalrow As Long = objdtcount.Rows(0)(0)

                'Dim intmaxrowerror As Long = 100000
                'Dim objsystemparametermaxrowerror As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9006)
                'If Not objsystemparametermaxrowerror Is Nothing Then
                '    intmaxrowerror = Convert.ToInt64(objsystemparametermaxrowerror.SettingValue)

                'End If
                'If inttotalrow > intmaxrowerror Then
                '    Throw New Exception("Maximum Record to export is " & intmaxrowerror & ". There is " & inttotalrow)
                'End If





                Dim strFileName As String = objreportexcel.CreateExcel2007WithDataALL(dirPath, strWhereClause, modulename)
                Dim fileNamexls As String = dirPath & modulename & ".xlsx"
                'delete
                If IO.File.Exists(fileNamexls) Then
                    IO.File.Delete(fileNamexls)
                End If

                'copy
                IO.File.Copy(strFileName, fileNamexls)
                lstFile.Add(fileNamexls)
                If IO.File.Exists(strFileName) Then
                    IO.File.Delete(strFileName)
                End If





            Next

            Dim fileNamezip As String = dirPath & "\InvalidReport.zip"
            Using zip As New ZipFile()


                If IO.File.Exists(fileNamezip) Then
                    IO.File.Delete(fileNamezip)
                End If
                zip.CompressionLevel = CompressionLevel.Level9
                zip.AddFiles(lstFile, "")
                zip.Save(fileNamezip)

            End Using


            Dim fileContents As Byte()
            fileContents = My.Computer.FileSystem.ReadAllBytes(fileNamezip)


            Response.Clear()
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=" & IO.Path.GetFileName(fileNamezip))
            Response.Charset = ""
            Response.AddHeader("cache-control", "max-age=0")
            Me.EnableViewState = False

            Response.ContentType = "application/x-compressed"
            Response.BinaryWrite(fileContents)
            Response.End()


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Protected Sub ExportExcelSelectedRow(sender As Object, e As EventArgs)
        Try
            Dim dirPath As String = Server.MapPath("~/Temp")
            dirPath = dirPath & "\" & NawaBLL.Common.SessionCurrentUser.UserID & "\"
            If Not IO.Directory.Exists(dirPath) Then
                IO.Directory.CreateDirectory(dirPath)
            End If


            Dim sm As RowSelectionModel = TryCast(Me.GridpanelView.GetSelectionModel(), RowSelectionModel)
            If sm.SelectedRows.Count > 0 Then

                '  Dim SelectionID As ArrayList = New ArrayList
                Dim strfilter As String = ""
                Dim strfilteritem As String = ""
                For Each item As SelectedRow In sm.SelectedRows
                    strfilter += item.RecordID & ","

                Next

                If strfilter.Length > 0 Then
                    strfilter = strfilter.Substring(0, Len(strfilter) - 1)
                    strfilter = " where PK_ID in (" & strfilter & ") and " & strWhereClause


                End If



                Dim strpktoprocess As String = ValidationReportBLL.GetPKToExport(strfilter)

                If strpktoprocess <> "" Then
                    strpktoprocess = "   recordid in( " & strpktoprocess & ")"
                End If
                strpktoprocess = strpktoprocess & " and " & strWhereClause
                Dim lstFile As New List(Of String)

                Dim objdt As Data.DataTable = ValidationReportBLL.GetSegmentDataToExport(strfilter)
                For Each item As Data.DataRow In objdt.Rows


                    Dim modulename As String = item("ModuleName")

                    Dim objreportexcel As New NawaDevBLL.ValidationReportBLL
                    objreportexcel.TableValidationReport = "Web_ValidationReport_RowComplate"
                    objreportexcel.TableValidationReportPKName = "PK_ID"
                    objreportexcel.ModuleName = modulename
                    Dim strFileName As String = objreportexcel.CreateExcel2007WithDataWithErrorMessage(dirPath, strpktoprocess)
                    Dim fileNamexls As String = dirPath & "\" & modulename & ".xlsx"
                    'delete
                    If IO.File.Exists(fileNamexls) Then
                        IO.File.Delete(fileNamexls)
                    End If

                    'copy
                    IO.File.Copy(strFileName, fileNamexls)
                    lstFile.Add(fileNamexls)
                    If IO.File.Exists(strFileName) Then
                        IO.File.Delete(strFileName)
                    End If





                Next

                Dim fileNamezip As String = dirPath & "\InvalidReport.zip"
                Using zip As New ZipFile()


                    If IO.File.Exists(fileNamezip) Then
                        IO.File.Delete(fileNamezip)
                    End If
                    zip.CompressionLevel = CompressionLevel.Level9
                    zip.AddFiles(lstFile, "")
                    zip.Save(fileNamezip)

                End Using


                Dim fileContents As Byte()
                fileContents = My.Computer.FileSystem.ReadAllBytes(fileNamezip)


                Response.Clear()
                Response.ClearHeaders()
                Response.AddHeader("content-disposition", "attachment;filename=" & IO.Path.GetFileName(fileNamezip))
                Response.Charset = ""
                Response.AddHeader("cache-control", "max-age=0")
                Me.EnableViewState = False

                Response.ContentType = "application/x-compressed"
                Response.BinaryWrite(fileContents)
                Response.End()
            Else
                Ext.Net.X.Msg.Alert("Error", "Please Select at least one item to export.").Show()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
End Class
