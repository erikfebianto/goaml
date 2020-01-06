Imports Ext.Net
Imports OfficeOpenXml
Partial Class AuditTrailEmailView
    Inherits Parent
    Public objFormModuleView As NawaBLL.FormModuleView
    Public Property strWhereClause() As String
        Get
            Return Session("AuditTrailEmailView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEmailView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("AuditTrailEmailView.strSort")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEmailView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("AuditTrailEmailView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEmailView.indexStart") = value
        End Set
    End Property
    'Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    objFormModuleView = New NawaBLL.FormModuleView(Me.GridpanelView, Me.BtnAdd)
    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Dim intModuleid As Integer
            Try
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim objmodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)


                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If

                objFormModuleView.ModuleID = objmodule.PK_Module_ID
                objFormModuleView.ModuleName = objmodule.ModuleName



                objFormModuleView.AddField("EmailID", "EmailID", 1, True, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailTemplateName", "Email Template ", 2, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailTo", "Email To", 3, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailCC", "Email CC", 4, False, False, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailBCC", "Email BCC", 4, False, False, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailSubject", "Email Subject", 5, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("EmailBody", "Email Body", 6, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )

                objFormModuleView.AddField("ProcessDate", "ProcessDate", 7, False, True, NawaBLL.Common.MFieldType.DATETIMEValue)
                objFormModuleView.AddField("SendEmailDate", "SendEmailDate", 8, False, True, NawaBLL.Common.MFieldType.DATETIMEValue, NawaBLL.SystemParameterBLL.GetDateFormat & " HH:mm:ss")

                objFormModuleView.AddField("EmailStatusName", "Email Status", 9, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("ErrorMessage", "Error Message", 10, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
                objFormModuleView.AddField("retrycount", "Retrycount", 11, False, False, NawaBLL.Common.MFieldType.INTValue,,,,, )







                objFormModuleView.SettingFormView()
            Catch ex As Exception

            End Try


            If Not Ext.Net.X.IsAjaxRequest Then
                cboExportExcel.SelectedItem.Text = "Excel"
            End If




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try



            Dim intStart As Integer = e.Start
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objFormModuleView.GetWhereClauseHeader(e)

            strfilter = strfilter.Replace("Active", objFormModuleView.ModuleName & ".Active")


            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            Me.indexStart = intStart
            Me.strWhereClause = strfilter
            Me.strOrder = strsort
            If strOrder = "" Then strOrder = "SendEmailDate desc"
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strWhereClause, strsort, intStart, intLimit, inttotalRecord)

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("EmailTemplateScheduler A INNER JOIN EmailTemplateSchedulerDetail b ON a.PK_EmailTemplateScheduler_ID=b.FK_EmailTEmplateScheduler_ID INNER JOIN EmailTemplate c ON  c.PK_EmailTemplate_Id=a.PK_EmailTemplate_ID INNER JOIN EmailStatus d ON d.PK_EmailStatus_ID=b.FK_EmailStatus_ID", "b.EmailID, c.EmailTemplateName , b.UnikFieldTablePrimary, b.EmailTo, b.EmailCC,b.EmailBCC, b.EmailSubject, b.EmailBody, b.ProcessDate, b.SendEmailDate,b.FK_EmailStatus_ID,d.EmailStatusName, b.ErrorMessage, b.retrycount", strWhereClause, Me.strOrder, intStart, intLimit, inttotalRecord)
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

    'Public Sub BtnAdd_Click()
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

End Class

