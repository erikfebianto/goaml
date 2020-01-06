Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports OfficeOpenXml
Imports System.Xml
Imports CookComputing.XmlRpc

Partial Class SLIK_CleansingReportView
    Inherits Parent
    Public objFormModuleView As NawaBLL.FormModuleView
    Public CleansingProcess As CleansingBLL

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("CleansingReportView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("CleansingReportView.ObjModule") = value
        End Set
    End Property

    Public Property strWhereClause() As String
        Get
            Return Session("CleansingReportView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("CleansingReportView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("CleansingReportView.strSort")
        End Get
        Set(ByVal value As String)
            Session("CleansingReportView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("CleansingReportView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("CleansingReportView.indexStart") = value
        End Set
    End Property

    Public Property objData() As List(Of CleansingReport)
        Get
            Return Session("CleansingReportView.objData")
        End Get
        Set(ByVal value As List(Of CleansingReport))
            Session("CleansingReportView.objData") = value
        End Set
    End Property

    Public Property objDataUpdate() As CleansingReport
        Get
            Return Session("CleansingReportView.objDataUpdate")
        End Get
        Set(ByVal value As CleansingReport)
            Session("CleansingReportView.objDataUpdate") = value
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

                    gridFileList.Title = "Cleansing Report"

                    storeFileList.PageSize = NawaBLL.SystemParameterBLL.GetPageSize

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ClearSession()
        ObjModule = Nothing
        objData = Nothing
        objDataUpdate = Nothing
    End Sub

    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
        objFormModuleView = New NawaBLL.FormModuleView(Me.gridFileList, Nothing)
    End Sub

    Protected Sub StoreView_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
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
            Me.strOrder = strsort
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strWhereClause, strsort, intStart, intLimit, inttotalRecord)

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("dbo.CleansingReport", "PK_Record_ID,SegmentData,CleansingRules,KeyField,KeyFieldValue,NamaField,OriginalValue,CleanedValue,Status,Report_Date, Kode_Cabang_BI, UniqueField,UniqueValue,ModuleID,ModuleName,Clean,Keep,LastUpdateBy,LastUpdateDate", strfilter, strsort, intStart, intLimit, inttotalRecord)
            ''-- start paging ------------------------------------------------------------
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If
            'Dim rangeData As List(Of Object) = If((e.Start < 0 OrElse limit < 0), data, data.GetRange(e.Start, limit))
            ''-- end paging ------------------------------------------------------------
            e.Total = inttotalRecord
            gridFileList.GetStore.DataSource = DataPaging
            gridFileList.GetStore.DataBind()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Protected Sub KeepSelected(sender As Object, e As DirectEventArgs)
        Dim proxy As ServicesForWeb
        Dim Successrun As Boolean = False

        Try
            proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            Dim RecordID() As String = AddLists().ToArray

            Successrun = proxy.InsertAsDictionary(RecordID)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub KeepAll(sender As Object, e As DirectEventArgs)
        Dim proxy As ServicesForWeb
        Dim Successrun As Boolean = False

        Try
            proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            Successrun = proxy.InsertAsDictionary(Nothing)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub CleanSelected(sender As Object, e As DirectEventArgs)
        Dim proxy As ServicesForWeb
        Dim Successrun As Boolean = False

        Try
            proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            Dim RecordID() As String = AddLists().ToArray
            Dim UserName As String = ""
            UserName = NawaBLL.Common.SessionCurrentUser.UserID

            Successrun = proxy.CleanRecords(RecordID, UserName)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub CleanAll(sender As Object, e As DirectEventArgs)
        Dim proxy As ServicesForWeb
        Dim Successrun As Boolean = False

        Try
            proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            Dim UserName As String = ""
            UserName = NawaBLL.Common.SessionCurrentUser.UserID

            Successrun = proxy.CleanRecords(Nothing, UserName)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ExportAllExcel(sender As Object, e As EventArgs)
        Dim objfileinfo As IO.FileInfo
        Try
            If Not cboExportExcel.SelectedItem Is Nothing Then
                If cboExportExcel.SelectedItem.Value = "Excel" Then
                    Dim tempfilexls As String = Guid.NewGuid.ToString & ".xlsx"
                    objfileinfo = New IO.FileInfo(Server.MapPath("~\temp\" & tempfilexls))

                    Using objtbl As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("CleansingReport", "PK_Record_ID,SegmentData,CleansingRules,KeyField,KeyFieldValue,NamaField,OriginalValue,CleanedValue,Status,Report_Date, Kode_Cabang_BI, UniqueField,UniqueValue,ModuleID,ModuleName,Clean,Keep,LastUpdateBy,LastUpdateDate", Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        For Each item As Ext.Net.ColumnBase In gridFileList.ColumnModel.Columns

                            If item.Hidden Then
                                objtbl.Columns.Remove(item.Text)

                            End If
                        Next

                        Using resource As New ExcelPackage(objfileinfo)
                            Dim ws As ExcelWorksheet = resource.Workbook.Worksheets.Add("Cleansing Report")
                            ws.Cells("A1").LoadFromDataTable(objtbl, True)
                            ws.Cells(ws.Dimension.Address).AutoFitColumns()
                            resource.Save()
                            Response.Clear()
                            Response.ClearHeaders()
                            Response.ContentType = "application/vnd.ms-excel"
                            Response.AddHeader("content-disposition", "attachment;filename=downloaddataxls.xlsx")
                            Response.Charset = ""
                            Response.AddHeader("cache-control", "max-age=0")
                            Me.EnableViewState = False
                            Response.ContentType = "ContentType"
                            Response.BinaryWrite(IO.File.ReadAllBytes(objfileinfo.FullName))
                            Response.End()
                        End Using
                    End Using
                ElseIf cboExportExcel.SelectedItem.Value = "CSV" Then
                    Dim tempfilexls As String = Guid.NewGuid.ToString & ".csv"
                    objfileinfo = New IO.FileInfo(Server.MapPath("~\temp\" & tempfilexls))
                    Dim stringWriter_Temp = New IO.StreamWriter(objfileinfo.FullName)
                    Using objtbl As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("CleansingReport", "PK_Record_ID,SegmentData,CleansingRules,KeyField,KeyFieldValue,NamaField,OriginalValue,CleanedValue,Status,Report_Date, Kode_Cabang_BI, UniqueField,UniqueValue,ModuleID,ModuleName,Clean,Keep,LastUpdateBy,LastUpdateDate", Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        For Each item As Ext.Net.ColumnBase In gridFileList.ColumnModel.Columns

                            If item.Hidden Then
                                objtbl.Columns.Remove(item.Text)

                            End If
                        Next

                        For k As Integer = 0 To objtbl.Columns.Count - 1
                            'add separator 
                            stringWriter_Temp.Write(objtbl.Columns(k).ColumnName + ","c)
                        Next
                        'append new line 
                        stringWriter_Temp.Write(vbCr & vbLf)
                        For i As Integer = 0 To objtbl.Rows.Count - 1
                            For k As Integer = 0 To objtbl.Columns.Count - 1
                                'add separator 
                                stringWriter_Temp.Write(objtbl.Rows(i).Item(k).ToString + ","c)
                            Next
                            'append new line 
                            stringWriter_Temp.Write(vbCr & vbLf)
                        Next
                        stringWriter_Temp.Close()
                        Response.Clear()
                        Response.AddHeader("content-disposition", "attachment;filename=downloaddatacsv.csv")
                        Response.Charset = ""
                        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        Me.EnableViewState = False
                        'Response.ContentType = "application/ms-excel"
                        Response.ContentType = "text/csv"
                        Response.BinaryWrite(System.IO.File.ReadAllBytes(objfileinfo.FullName))
                        Response.End()
                    End Using
                Else
                    Ext.Net.X.MessageBox.Alert("error", "Please Choose Format").Show()
                End If
            Else
                Ext.Net.X.MessageBox.Alert("error", "Please Choose Format").Show()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim objfileinfo As IO.FileInfo = Nothing
        Try
            If Not cboExportExcel.SelectedItem Is Nothing Then
                If cboExportExcel.SelectedItem.Value = "Excel" Then
                    Dim tempfilexls As String = Guid.NewGuid.ToString & ".xlsx"
                    objfileinfo = New IO.FileInfo(Server.MapPath("~\temp\" & tempfilexls))

                    Using objtbl As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("CleansingReport", "PK_Record_ID,SegmentData,CleansingRules,KeyField,KeyFieldValue,NamaField,OriginalValue,CleanedValue,Status,Report_Date, Kode_Cabang_BI, UniqueField,UniqueValue,ModuleID,ModuleName,Clean,Keep,LastUpdateBy,LastUpdateDate", Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        For Each item As Ext.Net.ColumnBase In gridFileList.ColumnModel.Columns

                            If item.Hidden Then
                                objtbl.Columns.Remove(item.Text)

                            End If
                        Next


                        Using resource As New ExcelPackage(objfileinfo)
                            Dim ws As ExcelWorksheet = resource.Workbook.Worksheets.Add("Cleansing Report")
                            ws.Cells("A1").LoadFromDataTable(objtbl, True)
                            ws.Cells(ws.Dimension.Address).AutoFitColumns()
                            resource.Save()
                            Response.Clear()
                            Response.ClearHeaders()
                            Response.ContentType = "application/vnd.ms-excel"
                            Response.AddHeader("content-disposition", "attachment;filename=downloaddataxls.xlsx")
                            Response.Charset = ""
                            Response.AddHeader("cache-control", "max-age=0")
                            Me.EnableViewState = False
                            Response.ContentType = "ContentType"
                            Response.BinaryWrite(IO.File.ReadAllBytes(objfileinfo.FullName))
                            Response.End()
                        End Using
                    End Using
                    'Dim json As String = Me.Hidden1.Value.ToString()
                    'Dim eSubmit As New StoreSubmitDataEventArgs(json, Nothing)
                    'Dim xml As XmlNode = eSubmit.Xml
                    'Me.Response.Clear()
                    'Me.Response.ContentType = "application/vnd.ms-excel"
                    'Me.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls")
                    'Dim xtExcel As New Xsl.XslCompiledTransform()
                    'xtExcel.Load(Server.MapPath("Excel.xsl"))
                    'xtExcel.Transform(xml, Nothing, Me.Response.OutputStream)
                    'Me.Response.[End]()
                ElseIf cboExportExcel.SelectedItem.Value = "CSV" Then
                    Dim tempfilexls As String = Guid.NewGuid.ToString & ".csv"
                    objfileinfo = New IO.FileInfo(Server.MapPath("~\temp\" & tempfilexls))
                    Dim stringWriter_Temp = New IO.StreamWriter(objfileinfo.FullName)
                    Using objtbl As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("CleansingReport", "PK_Record_ID,SegmentData,CleansingRules,KeyField,KeyFieldValue,NamaField,OriginalValue,CleanedValue,Status,Report_Date, Kode_Cabang_BI, UniqueField,UniqueValue,ModuleID,ModuleName,Clean,Keep,LastUpdateBy,LastUpdateDate", Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        For Each item As Ext.Net.ColumnBase In gridFileList.ColumnModel.Columns

                            If item.Hidden Then
                                objtbl.Columns.Remove(item.Text)

                            End If
                        Next
                        For k As Integer = 0 To objtbl.Columns.Count - 1
                            'add separator 
                            stringWriter_Temp.Write(objtbl.Columns(k).ColumnName + ","c)
                        Next
                        'append new line 
                        stringWriter_Temp.Write(vbCr & vbLf)
                        For i As Integer = 0 To objtbl.Rows.Count - 1
                            For k As Integer = 0 To objtbl.Columns.Count - 1
                                'add separator 
                                stringWriter_Temp.Write(objtbl.Rows(i).Item(k).ToString + ","c)
                            Next
                            'append new line 
                            stringWriter_Temp.Write(vbCr & vbLf)
                        Next
                        stringWriter_Temp.Close()
                        Response.Clear()
                        Response.AddHeader("content-disposition", "attachment;filename=downloaddatacsv.csv")
                        Response.Charset = ""
                        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        Me.EnableViewState = False
                        'Response.ContentType = "application/ms-excel"
                        Response.ContentType = "text/csv"
                        Response.BinaryWrite(System.IO.File.ReadAllBytes(objfileinfo.FullName))
                        Response.End()
                    End Using
                Else
                    Ext.Net.X.MessageBox.Alert("error", "Please Choose Format").Show()
                End If
            Else
                Ext.Net.X.MessageBox.Alert("error", "Please Choose Format").Show()
            End If
        Catch ex As Exception
            Ext.Net.X.MessageBox.Alert("error", "Please Choose Format").Show()
        Finally
            If Not objfileinfo Is Nothing Then
                objfileinfo.Delete()
            End If
        End Try
    End Sub

    Protected Function AddLists() As List(Of String)
        Dim RecordIDList As List(Of String) = New List(Of String)
        Dim sm As RowSelectionModel = TryCast(Me.gridFileList.GetSelectionModel(), RowSelectionModel)

        If sm.SelectedRows.Count > 0 Then
            For Each item As SelectedRow In sm.SelectedRows
                RecordIDList.Add(item.RecordID.ToString)
            Next
        End If

        Return RecordIDList
    End Function
End Class
