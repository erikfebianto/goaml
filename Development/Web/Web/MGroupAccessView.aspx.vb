Imports Ext.Net
Imports OfficeOpenXml
Public Class MGroupAccessView
    Inherits Parent
    Public objFormModuleView As NawaBLL.FormMGroupMenuAccessView

    Public Property TableName() As String
        Get
            Return Session("MGroupAccessView.TableName")
        End Get
        Set(ByVal value As String)
            Session("MGroupAccessView.TableName") = value
        End Set
    End Property



    Public Property strWhereClause() As String
        Get
            Return Session("MGroupAccessView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("MGroupAccessView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("MGroupAccessView.strSort")
        End Get
        Set(ByVal value As String)
            Session("MGroupAccessView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("MGroupAccessView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("MGroupAccessView.indexStart") = value
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

                objFormModuleView.ModuleName = objmodule.ModuleName
                objFormModuleView.AddField("PK_MGroupMenuAcess_ID", "ID", 1, True, False, NawaBLL.Common.MFieldType.IDENTITY)
                objFormModuleView.AddField("FK_GroupMenu_ID", "FK_GroupMenu_ID", 2, False, False, NawaBLL.Common.MFieldType.INTValue)
                objFormModuleView.AddField("GroupMenuName", "GroupMenu Name", 3, False, True, NawaBLL.Common.MFieldType.VARCHARValue, True)
                objFormModuleView.AddField("ModuleLabel", "Module", 4, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bAdd", "Add", 5, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bEdit", "Edit", 6, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bDelete", "Delete", 7, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bActivation", "Activation", 8, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bView", "View", 9, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bApproval", "Approval", 10, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bUpload", "Upload", 11, False, True, NawaBLL.Common.MFieldType.VARCHARValue)
                objFormModuleView.AddField("bDetail", "Detail", 12, False, True, NawaBLL.Common.MFieldType.VARCHARValue)

                Me.TableName = " MGroupMenuAccess a INNER JOIN Module b ON a.FK_Module_ID=b.PK_Module_ID INNER JOIN MGroupMenu c ON c.PK_MGroupMenu_ID=a.FK_GroupMenu_ID "





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

    <DirectMethod>
    Public Sub Submitdata(command As String, groupdata As Object)

    End Sub

    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start
            'If intStart = 0 Then intStart = 1
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objFormModuleView.GetWhereClauseHeader(e)

            strfilter = strfilter.Replace("Active", objFormModuleView.ModuleName & ".Active")

            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            If strsort = "" Then strsort = "GroupMenuName,ModuleLabel"
            Me.indexStart = intStart
            Me.strWhereClause = strfilter
            Me.strOrder = strsort
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(Me.TableName, strWhereClause, strsort, intStart, intLimit, inttotalRecord)
            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging(Me.TableName, "PK_MGroupMenuAcess_ID,FK_GroupMenu_ID,GroupMenuName,ModuleLabel,CASE WHEN b.IsSupportAdd=1 THEN case when a.bAdd='True' THEN 'Yes' ELSE 'No' end ELSE '-' END AS bAdd, CASE WHEN b.IsSupportEdit=1 THEN  case when a.bEdit='True' THEN 'Yes' ELSE 'No' end ELSE '-' END AS bEdit,CASE WHEN b.IsSupportDelete=1 THEN case when a.bDelete='True'THEN 'Yes' ELSE 'No' end ELSE '-' END AS bDelete,CASE WHEN b.IsSupportActivation=1 THEN case when a.bActivation='True' THEN 'Yes' ELSE 'No' end ELSE '-' END AS bActivation,CASE WHEN b.IsSupportView=1 THEN case when a.bView='True' THEN 'Yes' ELSE 'No' end ELSE '-' END AS bView ,CASE WHEN b.IsUseApproval=1 THEN case when a.bApproval='True' THEN 'Yes' ELSE 'No' end  ELSE '-' END AS bApproval ,CASE WHEN b.IsSupportUpload=1 THEN case when a.bUpload='True' THEN 'Yes' ELSE 'No' end  ELSE '-' END AS bUpload ,CASE WHEN b.IsSupportDetail=1 THEN case when a.bDetail='True' THEN 'Yes' ELSE 'No' end  ELSE '-' END AS bDetail", strWhereClause, strsort, intStart, intLimit, inttotalRecord)
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.TableName, Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.Text) Then
                                    objtbl.Columns.Remove(item.Text)
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.TableName, Me.strWhereClause, Me.strOrder, 0, Integer.MaxValue, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.Text) Then
                                    objtbl.Columns.Remove(item.Text)
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.TableName, Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.Text) Then
                                    objtbl.Columns.Remove(item.Text)
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
                    Using objtbl As Data.DataTable = objFormModuleView.getDataPaging(Me.TableName, Me.strWhereClause, Me.strOrder, Me.indexStart, NawaBLL.SystemParameterBLL.GetPageSize, 0)

                        objFormModuleView.changeHeader(objtbl)
                        For Each item As Ext.Net.ColumnBase In GridpanelView.ColumnModel.Columns

                            If item.Hidden Then
                                If objtbl.Columns.Contains(item.Text) Then
                                    objtbl.Columns.Remove(item.Text)
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
        objFormModuleView = New NawaBLL.FormMGroupMenuAccessView(Me.GridpanelView, Me.BtnAdd)
    End Sub

End Class

