Imports Ext.Net
Imports OfficeOpenXml
Partial Class AuditTrailEODView
    Inherits ParentPage
    Public objFormModuleView As NawaBLL.FormModuleView
    Public Property strWhereClause() As String
        Get
            Return Session("AuditTrailEODView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEODView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("AuditTrailEODView.strSort")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEODView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("AuditTrailEODView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEODView.indexStart") = value
        End Set
    End Property
    'Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    objFormModuleView = New NawaBLL.FormModuleView(Me.GridpanelView, Me.BtnAdd)
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try



            objFormModuleView.ModuleID = ObjModule.PK_Module_ID
            objFormModuleView.ModuleName = ObjModule.ModuleName

            objFormModuleView.AddField("PK_EODTaskDetailLog_ID", "ID", 1, True, True, NawaBLL.Common.MFieldType.IDENTITY)
            objFormModuleView.AddField("EODSchedulerName", "EOD Scheduler Name", 2, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("EODTaskName", "EOD Task Name", 3, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("EODTaskDetailType", "EOD Task Detail Type", 4, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("NAME", "Task Detail", 5, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("ExecuteBy", "Execute By", 6, False, True, NawaBLL.Common.MFieldType.VARCHARValue)

            objFormModuleView.AddField("DataDate", "Report Date", 7, False, True, NawaBLL.Common.MFieldType.DATETIMEValue, NawaBLL.SystemParameterBLL.GetDateFormat)
            objFormModuleView.AddField("ProcessDate", "Process Date", 8, False, True, NawaBLL.Common.MFieldType.DATETIMEValue, NawaBLL.SystemParameterBLL.GetDateFormat & " HH:mm:ss")
            objFormModuleView.AddField("StartDate", "Start Date", 9, False, True, NawaBLL.Common.MFieldType.DATETIMEValue, NawaBLL.SystemParameterBLL.GetDateFormat & " HH:mm:ss")
            objFormModuleView.AddField("EndDate", "End Date", 10, False, True, NawaBLL.Common.MFieldType.DATETIMEValue, NawaBLL.SystemParameterBLL.GetDateFormat & " HH:mm:ss")

            objFormModuleView.AddField("ErrorMessage", "Error Message", 11, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("MsEODStatusName", "Task Detail Status", 12, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("TaskStatus", "Task Status", 13, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
            objFormModuleView.AddField("FK_MsEODStatus_ID", "", 14, False, False, NawaBLL.Common.MFieldType.VARCHARValue)

            objFormModuleView.SettingFormView()

            InjectNewCancelButton()

            Dim objcommandcol As Ext.Net.CommandColumn = GridpanelView.ColumnModel.Columns.Find(Function(x) x.ID = "columncrud")
            objcommandcol.PrepareToolbar.Fn = "prepareCommandCollection"

            If Not Ext.Net.X.IsAjaxRequest Then
                cboExportExcel.SelectedItem.Text = "Excel"
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub InjectNewCancelButton()

        Dim existingCommandColumn As CommandColumn = CType(GridpanelView.ColumnModel.Columns.Find(Function(x) x.ID = "columncrud"), CommandColumn)
        Dim newGridCommand As New GridCommand
        Dim parameterPK As New Parameter
        Dim parameterCommand As New Parameter

        With newGridCommand
            .CommandName = "Cancel"
            .Icon = Icon.Cancel
            .Text = "Cancel"
        End With

        With parameterPK
            .Name = "unikkey"
            .Value = "record.data.PK_EODTaskDetailLog_ID"
            .Mode = ParameterMode.Raw
        End With

        With parameterCommand
            .Name = "command"
            .Value = "command"
            .Mode = ParameterMode.Raw
        End With

        With existingCommandColumn.DirectEvents.Command
            .ExtraParams.Add(parameterPK)
            .ExtraParams.Add(parameterCommand)
        End With

        existingCommandColumn.Commands.Add(newGridCommand)

        existingCommandColumn.SetWidth(CType(existingCommandColumn.Width.Value * 2, Integer))

        AddHandler existingCommandColumn.DirectEvents.Command.Event, New ComponentDirectEvent.DirectEventHandler(AddressOf OnCancelPressedEvent)

    End Sub

    Public Sub OnCancelPressedEvent(sender As Object, e As DirectEventArgs)

        GridpanelView.Mask("Cancelling...")

        Dim keyValue As String
        Dim commandValue As String = e.ExtraParams.Item(1).Value
        Dim msgConfig As MessageBoxButtonsConfig
        Dim msgBoxConfig As MessageBoxConfig

        If Not commandValue = "Cancel" Then Return

        '================================================================

        keyValue = e.ExtraParams.Item(0).Value

        msgConfig = New MessageBoxButtonsConfig With {
                        .Yes = New MessageBoxButtonConfig With {
                                                .Handler = "Nawadata.CancelSchedule(" & keyValue & ");",
                                                    .Text = "Yes"},
                        .No = New MessageBoxButtonConfig With {
                                                .Handler = "Nawadata.HideMask();", .Text = "No"}
                    }

        msgBoxConfig = New MessageBoxConfig With {
            .Title = "Warning",
            .Message = "Are you sure to cancel this schedule ?",
            .Buttons = MessageBox.Button.YESNO,
            .Closable = False,
            .Icon = MessageBox.Icon.WARNING,
            .MessageBoxButtonsConfig = msgConfig
        }

        Ext.Net.X.Msg.Show(msgBoxConfig)

    End Sub

    <DirectMethod([Namespace]:="Nawadata")>
    Public Sub HideMask()

        GridpanelView.Unmask()

    End Sub

    <DirectMethod([Namespace]:="Nawadata")>
    Public Sub CancelSchedule(taskDetailLogPKParam As Long)

        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            NawaDevBLL.AuditTrailEODBLL.ChangeScheduleStatusToCancel(taskDetailLogPKParam, ObjModule)

            GridpanelView.Unmask()

            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objFormModuleView.objSchemaModule.UrlView & "?ModuleID={0}", Moduleid), "Loading")
        Catch ex As Exception
            GridpanelView.Unmask()
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
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
            If strsort = "" Then
                strsort = "PK_EODSchedulerLog_ID desc"
            End If

            Me.indexStart = intStart

            'If Not String.IsNullOrWhiteSpace(strfilter) Then
            '    strfilter += " AND FK_EODSchedulerID = " & AuditTrailEODBLL.GetCoreEngineSystemParameterValue()
            'Else
            '    strfilter += " FK_EODSchedulerID = " & AuditTrailEODBLL.GetCoreEngineSystemParameterValue()
            'End If

            'If Not String.IsNullOrWhiteSpace(strfilter) Then
            '    strfilter += " AND FK_EODSchedulerID != " & AuditTrailEODBLL.GetCoreEngineSystemParameterValue()
            'Else
            '    strfilter += " FK_EODSchedulerID != " & AuditTrailEODBLL.GetCoreEngineSystemParameterValue()
            'End If

            'If Not String.IsNullOrWhiteSpace(strfilter) Then
            '    strfilter += " AND PK_MsEODStatus_ID = 2"
            'Else
            '    strfilter += " PK_MsEODStatus_ID = 2"
            'End If

            Me.strWhereClause = strfilter
            Me.strOrder = strsort
            Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strWhereClause, strsort, intStart, intLimit, inttotalRecord)
            'Dim DataPaging As Data.DataTable = DLL.SQLHelper.ExecuteTabelPaging("CustomerInformation_WebTempTable", "CIFNo, Name, DateOfBirth, AccountOwnerId, OpeningDate, WorkingUnitName, IsCustomerInList", strfilter, strsort, intStart, intLimit, inttotalRecord)
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
            ' GridpanelView.ColumnModel.Columns(11).Hide()

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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.DataIndex) Then
                                    objtbl.Columns.Remove(item.DataIndex)
                                End If

                            End If
                        Next

                        Using resource As New ExcelPackage(objfileinfo)
                            Dim ws As ExcelWorksheet = resource.Workbook.Worksheets.Add(objFormModuleView.ModuleName)
                            ws.Cells("A1").LoadFromDataTable(objtbl, True)

                            Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
                            Dim intcolnumber As Integer = 1
                            For Each item As System.Data.DataColumn In objtbl.Columns
                                If item.DataType = GetType(Date) Then
                                    ws.Column(intcolnumber).Style.Numberformat.Format = dateformat
                                End If
                                intcolnumber = intcolnumber + 1
                            Next
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.DataIndex) Then
                                    objtbl.Columns.Remove(item.DataIndex)
                                End If

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
                                 stringWriter_Temp.Write("""" & objtbl.Rows(i).Item(k).ToString & """" + ","c)
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.DataIndex) Then
                                    objtbl.Columns.Remove(item.DataIndex)
                                End If

                            End If
                        Next


                        Using resource As New ExcelPackage(objfileinfo)
                            Dim ws As ExcelWorksheet = resource.Workbook.Worksheets.Add(objFormModuleView.ModuleName)
                            ws.Cells("A1").LoadFromDataTable(objtbl, True)
                            Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
                            Dim intcolnumber As Integer = 1
                            For Each item As System.Data.DataColumn In objtbl.Columns
                                If item.DataType = GetType(Date) Then
                                    ws.Column(intcolnumber).Style.Numberformat.Format = dateformat
                                End If
                                intcolnumber = intcolnumber + 1
                            Next
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.DataIndex) Then
                                    objtbl.Columns.Remove(item.DataIndex)
                                End If

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
                                stringWriter_Temp.Write("""" & objtbl.Rows(i).Item(k).ToString & """" + ","c)
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

    <DirectMethod>
    Public Sub BtnAdd_Click()
        Try


            Dim Moduleid As String = Request.Params("ModuleID")


            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objFormModuleView.objSchemaModule.UrlAdd & "?ModuleID={0}", Moduleid), "Loading")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
        objFormModuleView = New NawaBLL.FormModuleView(Me.GridpanelView, Me.BtnAdd)
    End Sub

    Private Sub AuditTrailEODView_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        ActionType = NawaBLL.Common.ModuleActionEnum.view

    End Sub
End Class

