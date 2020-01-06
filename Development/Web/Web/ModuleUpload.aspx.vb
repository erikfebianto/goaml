Imports System.IO
Imports OfficeOpenXml
Imports System.Data
Imports NawaDevDAL
Imports System.Data.SqlClient
Imports Microsoft.SqlServer.Server

Partial Class NAWADATA_ModuleUpload
    Inherits ParentPage

    Dim addresses As New List(Of List(Of String))
    ' Dim dataparam As New datamodules

    Public ReadOnly Property dataparam() As NawaDevBLL.datamodules
        Get
            If Session("NAWADATA_ModuleUpload.dataparam") Is Nothing Then
                Dim dataparamx As New NawaDevBLL.datamodules
                Session("NAWADATA_ModuleUpload.dataparam") = dataparamx
            End If

            Return Session("NAWADATA_ModuleUpload.dataparam")
        End Get
        'Set(ByVal value As datamodules)
        '    Session("NAWADATA_ModuleUpload.dtModule") = value
        'End Set
    End Property

    'Public Property dtModuleField() As DataTable
    '    Get
    '        'Return Session("NAWADATA_ModuleUpload.dtModuleField")
    '    End Get
    '    Set(ByVal value As DataTable)
    '        'Session("NAWADATA_ModuleUpload.dtModuleField") = value
    '    End Set
    'End Property

    Private Function strModuleID() As String
        Dim strid As String = Request.Params("ModuleID")
        Dim id As String = 0

        id = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey) 'Convert.ToInt64(strid)
        Return id
    End Function

    Private Sub ClearSession()
        'datamodules.dtmodule = Nothing
        'dtModuleField = Nothing
        Session("NAWADATA_ModuleUpload.dataparam") = Nothing
    End Sub

    Private Sub CheckAccess()
        ''Perlu ada hak akses?
    End Sub

    Private Sub NAWADATA_ModuleUpload_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            LoadGridModel()

            If Not Ext.Net.X.IsAjaxRequest Then
                ClearSession()
            End If
            'CheckAccess()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub NAWADATA_ModuleUpload_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        ''Untuk kalau perlu ada audit trail
        ActionType = NawaBLL.Common.ModuleActionEnum.Upload
    End Sub

    Protected Sub btnDownloadData_Click(sender As Object, e As EventArgs)
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Try
                Dim wb As FileInfo = New FileInfo(Server.MapPath("~/assets/templates/ModuleUploadTemplate.xlsx"))
                Using package As ExcelPackage = New ExcelPackage(wb)
                    GenerateReferences(package)

                    ''Load Data Module
                    Dim wsModule As ExcelWorksheet = package.Workbook.Worksheets("Module")
                    If wsModule IsNot Nothing Then
                        Dim currentRow As Long = 2

                        For Each item In NawaBLL.ModuleBLL.GetModuleAll.OrderBy(Function(x) x.PK_Module_ID)
                            ''Kasih default update aja biar ga salah
                            wsModule.Cells(currentRow, 1).Value = "Update"
                            wsModule.Cells(currentRow, 2).Value = item.PK_Module_ID
                            wsModule.Cells(currentRow, 3).Value = item.ModuleName
                            wsModule.Cells(currentRow, 4).Value = item.ModuleLabel
                            wsModule.Cells(currentRow, 5).Value = item.ModuleDescription
                            wsModule.Cells(currentRow, 6).Value = item.IsUseDesigner
                            wsModule.Cells(currentRow, 7).Value = item.IsUseApproval
                            wsModule.Cells(currentRow, 8).Value = item.IsSupportAdd
                            wsModule.Cells(currentRow, 9).Value = item.IsSupportEdit
                            wsModule.Cells(currentRow, 10).Value = item.IsSupportDelete
                            wsModule.Cells(currentRow, 11).Value = item.IsSupportActivation
                            wsModule.Cells(currentRow, 12).Value = item.IsSupportView
                            wsModule.Cells(currentRow, 13).Value = item.IsSupportUpload
                            wsModule.Cells(currentRow, 14).Value = item.IsSupportDetail
                            wsModule.Cells(currentRow, 15).Value = item.UrlAdd
                            wsModule.Cells(currentRow, 16).Value = item.UrlEdit
                            wsModule.Cells(currentRow, 17).Value = item.UrlDelete
                            wsModule.Cells(currentRow, 18).Value = item.UrlActivation
                            wsModule.Cells(currentRow, 19).Value = item.UrlView
                            wsModule.Cells(currentRow, 20).Value = item.UrlUpload
                            wsModule.Cells(currentRow, 21).Value = item.UrlApproval
                            wsModule.Cells(currentRow, 22).Value = item.UrlApprovalDetail
                            wsModule.Cells(currentRow, 23).Value = item.UrlDetail
                            wsModule.Cells(currentRow, 24).Value = item.IsUseStoreProcedureValidation

                            currentRow += 1
                        Next
                    End If

                    ''Load Data Module Field
                    Dim wsModuleField As ExcelWorksheet = package.Workbook.Worksheets("ModuleField")
                    If wsModuleField IsNot Nothing Then
                        Dim currentRow As Long = 2

                        Using objNawadal As New NawaDAL.NawaDataEntities

                            For Each item In objNawadal.ModuleFields.ToList
                                ''Kasih default update aja biar ga salah
                                wsModuleField.Cells(currentRow, 1).Value = "Update"
                                wsModuleField.Cells(currentRow, 2).Value = item.PK_ModuleField_ID

                                Dim moduleName As String = NawaBLL.ModuleBLL.GetModuleByModuleID(item.FK_Module_ID).ModuleName

                                ''Untuk jaga-jaga kalau ada modulefield tanpa parent
                                If moduleName Is Nothing Then
                                    wsModuleField.Cells(currentRow, 3).Value = "<Unidentified Module ID>"
                                Else
                                    wsModuleField.Cells(currentRow, 3).Value = moduleName
                                End If
                                wsModuleField.Cells(currentRow, 4).Value = item.FieldName
                                wsModuleField.Cells(currentRow, 5).Value = item.FieldLabel
                                wsModuleField.Cells(currentRow, 6).Value = item.Sequence
                                wsModuleField.Cells(currentRow, 7).Value = item.Required
                                wsModuleField.Cells(currentRow, 8).Value = item.IsPrimaryKey
                                wsModuleField.Cells(currentRow, 9).Value = item.IsUnik
                                wsModuleField.Cells(currentRow, 10).Value = item.IsShowInView
                                wsModuleField.Cells(currentRow, 11).Value = item.IsShowInForm
                                wsModuleField.Cells(currentRow, 12).Value = item.DefaultValue

                                Dim FieldType As String = (From x In objNawadal.MFieldTypes Where x.PK_FieldType_ID = item.FK_FieldType_ID Select x).FirstOrDefault.FieldTypeCaption

                                If FieldType IsNot Nothing Then
                                    wsModuleField.Cells(currentRow, 13).Value = FieldType
                                End If

                                wsModuleField.Cells(currentRow, 14).Value = item.SizeField

                                Dim ExtType As String = (From x In objNawadal.MExtTypes Where x.PK_ExtType_ID = item.FK_ExtType_ID Select x).FirstOrDefault.ExtTypeName

                                If ExtType IsNot Nothing Then
                                    wsModuleField.Cells(currentRow, 15).Value = ExtType
                                End If

                                wsModuleField.Cells(currentRow, 16).Value = item.TabelReferenceName
                                wsModuleField.Cells(currentRow, 17).Value = item.TabelReferenceNameAlias
                                wsModuleField.Cells(currentRow, 18).Value = item.TableReferenceFieldKey
                                wsModuleField.Cells(currentRow, 19).Value = item.TableReferenceFieldDisplayName
                                wsModuleField.Cells(currentRow, 20).Value = item.TableReferenceFilter
                                wsModuleField.Cells(currentRow, 21).Value = item.IsUseRegexValidation
                                wsModuleField.Cells(currentRow, 22).Value = item.TableReferenceAdditonalJoin
                                wsModuleField.Cells(currentRow, 23).Value = item.BCasCade
                                wsModuleField.Cells(currentRow, 24).Value = item.FieldNameParent
                                wsModuleField.Cells(currentRow, 25).Value = item.FilterCascade

                                currentRow += 1
                            Next
                        End Using
                    End If

                    Response.Clear()
                    Response.AddHeader("content-disposition", "attachment;filename=ModuleUploadTemplate" & Now.ToString & ".xlsx")
                    Response.Charset = ""
                    Me.EnableViewState = False
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.BinaryWrite(package.GetAsByteArray())
                    Response.End()
                End Using
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try
        End Using
    End Sub

    Protected Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs)
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Try
                Dim wb As FileInfo = New FileInfo(Server.MapPath("~/assets/templates/ModuleUploadTemplate.xlsx"))
                Using package As ExcelPackage = New ExcelPackage(wb)
                    GenerateReferences(package)

                    Response.Clear()
                    Response.AddHeader("content-disposition", "attachment;filename=ModuleUploadTemplate" & Now.ToString & ".xlsx")
                    Response.Charset = ""
                    Me.EnableViewState = False
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.BinaryWrite(package.GetAsByteArray())
                    Response.End()
                End Using
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try
        End Using
    End Sub

    Protected Sub btnUpload_Click()
        Try
            Using objdb As New NawaDAL.NawaDataEntities
                If fuData.HasFile Then
                    ''Directory file fisik yang di-write di server
                    Dim strFilePath As String = Server.MapPath("~/Temp/UploadModule/")
                    Dim strworkingpaper As String = Me.SaveFileImportToSave(fuData)
                    Dim br As BinaryReader

                    ''Format yang diterima sementara hanya xlsx
                    If strworkingpaper.EndsWith(".xlsx") Then
                        Dim bData As Byte()
                        br = New BinaryReader(System.IO.File.OpenRead(strFilePath & strworkingpaper))
                        bData = br.ReadBytes(br.BaseStream.Length)
                        Dim ms As MemoryStream = New MemoryStream(bData, 0, bData.Length)
                        ms.Write(bData, 0, bData.Length)

                        Using excelFile As New ExcelPackage(ms)
                            ''add NR
                            'Dim xmldata As String = NawaBLL.Common.Serialize(excelFile)

                            ReadModuleSheet(excelFile.Workbook.Worksheets("Module"))
                            ReadModuleFieldSheet(excelFile.Workbook.Worksheets("ModuleField"))

                            ''Hide upload UI
                            fuData.Hidden = True
                            btnUpload.Hidden = True
                            Label3.Hidden = True
                        End Using

                        ms.Flush()
                    ElseIf strworkingpaper.EndsWith(".xls") Then
                        Ext.Net.X.Msg.Alert("Error", "Format Excel Not Supported!").Show()
                        Return
                    Else
                        Ext.Net.X.Msg.Alert("Error", "Unsupported format File!").Show()
                        Return
                    End If
                Else
                    Ext.Net.X.Msg.Alert("Error", "No File Choosen").Show()
                End If
            End Using
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try



    End Sub

    Protected Sub btnSubmit_Click()

        If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 Then 'OrElse Not (ObjModule.IsUseApproval) Then

            Try
                SubmitData(dataparam)
                LblConfirmation.Text = "Data Saved into Database"
                Panelconfirmation.Hidden = False
                FormPanelInput.Hidden = True
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try

        Else

            Using objdb As New NawaDevDAL.NawaDatadevEntities
                Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()


                    Try


                        Dim xmldata As String = NawaBLL.Common.Serialize(dataparam)

                        Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                        With objModuleApproval
                            .ModuleName = "ModuleUpload"
                            .ModuleKey = 0
                            .ModuleField = xmldata
                            .ModuleFieldBefore = ""
                            .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Upload
                            .CreatedDate = Now
                            .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                        End With

                        objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                        objdb.SaveChanges()
                        objtrans.Commit()

                        LblConfirmation.Text = "Data Saved into Pending Approval"
                        Panelconfirmation.Hidden = False
                        FormPanelInput.Hidden = True


                    Catch ex As Exception

                        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

                    End Try
                End Using
            End Using
        End If








    End Sub

    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnCancel_Click()
        Try
            Using objdb As New NawaDevDAL.NawaDatadevEntities
                Dim ModuleID As String = strModuleID()
                Dim strDirectory As String = (From x In objdb.Modules Where x.PK_Module_ID = ModuleID Select x).FirstOrDefault.UrlView
                Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & strDirectory & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(ModuleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)), "Loading")
            End Using
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Public Function SaveFileImportToSave(ByVal FileUpload As FileUploadField) As String
        Dim filePath As String
        Dim FileNameToGetData As String = ""
        Try
            ''Save file upload ke file fisik di server untuk di-read
            If FileUpload.PostedFile.ContentLength <> 0 Then
                If FileUpload.HasFile Then
                    Dim strFileName As String = ""
                    If FileUpload.FileName.Contains("\") Then
                        Dim lstFileName() As String = FileUpload.FileName.Split("\")
                        strFileName = lstFileName(lstFileName.Count - 1)
                    Else
                        strFileName = FileUpload.FileName
                    End If

                    filePath = Server.MapPath("~/Temp/UploadModule/" & strFileName)
                    FileNameToGetData = strFileName
                    If System.IO.File.Exists(filePath) Then
                        Dim bexist As Boolean = True
                        Dim i As Integer = 1
                        While bexist
                            filePath = Server.MapPath("~/Temp/UploadModule/" & strFileName.Split(CChar("."))(0) & "-" & i.ToString & "." & strFileName.Split(CChar("."))(1))
                            FileNameToGetData = strFileName.Split(CChar("."))(0) & "-" & i.ToString & "." & strFileName.Split(CChar("."))(1)
                            If Not System.IO.File.Exists(filePath) Then
                                bexist = False
                                Exit While
                            End If
                            i = i + 1
                        End While
                    End If
                    'FileUpload.SaveAs(filePath)
                    System.IO.File.WriteAllBytes(filePath, FileUpload.FileBytes)
                End If
            End If
            Return FileNameToGetData
        Catch
            Throw
        End Try
    End Function

    Private Sub ReadModuleSheet(ByVal sheet As ExcelWorksheet)
        ''Note: Apakah field-field boolean juga perlu dikasih default, atau dijadikan error kalau kosong?
        ''Note: Apakah URL perlu diisikan default seperti di UI kalau di flag support designer?

        ''Validasi-validasi yang masih perlu ditambahkan:
        ''-Validasi module name harus unique (Perlu?)
        ''-Validasi support designer + support ..., URL harus diisi (Perlu?)
        'Dim dt As New DataTable
        'Dim dt As New datamodules

        ''' Define columns
        'dt.Columns.Add("Action", GetType(System.String))
        'dt.Columns.Add("ErrorMessage", GetType(System.String))

        '''Ambil list column/field table Module dari information schema
        'Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'Module' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)", Nothing)

        'For Each item As DataRow In objdata.Rows
        '    dt.Columns.Add(item(0), GetType(System.String))
        'Next

        Using objdb As New NawaDAL.NawaDataEntities
            Dim x As Integer = 2

            While x <> 0
                Dim ErrorMessage As String = ""
                Dim CheckModuleName As String = ""

                ''Cari nama module, jika kosong diasumsikan bahwa itu row terakhir
                CheckModuleName = sheet.Cells(x, 3).Text

                If Not String.IsNullOrEmpty(CheckModuleName) Then
                    'Dim dr As Data.DataRow = dt.NewRow()
                    Dim dr As New NawaDevBLL.datamodules.ClassModules


                    ''Kalau action kosong dianggap defaultnya insert seperti upload biasa
                    If sheet.Cells(x, 1).Text = "" Or sheet.Cells(x, 1).Text Is Nothing Then
                        dr.Action = "Insert"
                    Else
                        dr.Action = sheet.Cells(x, 1).Text
                    End If

                    dr.PK_Module_ID = sheet.Cells(x, 2).Text

                    If String.IsNullOrEmpty(sheet.Cells(x, 3).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Module Name is empty"
                    Else
                        ''Validasi action update/delete
                        If sheet.Cells(x, 1).Text = "Delete" Or sheet.Cells(x, 1).Text = "Update" Then
                            Dim moduleName As String = sheet.Cells(x, 3).Text
                            Dim moduledata As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleName(moduleName)

                            If moduledata Is Nothing Then
                                ''Kalau nama ga ada, cek primary key
                                moduleName = sheet.Cells(x, 2).Text

                                moduledata = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleName)

                                If moduledata Is Nothing Then
                                    If ErrorMessage <> "" Then
                                        ErrorMessage = ErrorMessage & ", "
                                    End If
                                    ErrorMessage = ErrorMessage & "cannot find module in database"
                                End If
                            End If
                        End If

                        dr.ModuleName = sheet.Cells(x, 3).Text
                    End If

                    If String.IsNullOrEmpty(sheet.Cells(x, 4).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Module Label is empty"
                    Else
                        dr.ModuleLabel = sheet.Cells(x, 4).Text
                    End If

                    dr.ModuleDescription = sheet.Cells(x, 5).Text

                    If sheet.Cells(x, 6).Text = "" Or sheet.Cells(x, 6).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsUseDesigner = 0
                    Else
                        dr.IsUseDesigner = sheet.Cells(x, 6).Text
                    End If

                    If sheet.Cells(x, 7).Text = "" Or sheet.Cells(x, 7).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsUseApproval = 0
                    Else
                        dr.IsUseApproval = sheet.Cells(x, 7).Text
                    End If

                    If sheet.Cells(x, 8).Text = "" Or sheet.Cells(x, 8).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportAdd = 0
                    Else
                        dr.IsSupportAdd = sheet.Cells(x, 8).Text
                    End If

                    If sheet.Cells(x, 9).Text = "" Or sheet.Cells(x, 9).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportEdit = 0
                    Else
                        dr.IsSupportEdit = sheet.Cells(x, 9).Text
                    End If

                    If sheet.Cells(x, 10).Text = "" Or sheet.Cells(x, 10).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportDelete = 0
                    Else
                        dr.IsSupportDelete = sheet.Cells(x, 10).Text
                    End If

                    If sheet.Cells(x, 11).Text = "" Or sheet.Cells(x, 11).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportActivation = 0
                    Else
                        dr.IsSupportActivation = sheet.Cells(x, 11).Text
                    End If

                    If sheet.Cells(x, 12).Text = "" Or sheet.Cells(x, 12).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportView = 0
                    Else
                        dr.IsSupportView = sheet.Cells(x, 12).Text
                    End If

                    If sheet.Cells(x, 13).Text = "" Or sheet.Cells(x, 13).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportUpload = 0
                    Else
                        dr.IsSupportUpload = sheet.Cells(x, 13).Text
                    End If

                    If sheet.Cells(x, 14).Text = "" Or sheet.Cells(x, 14).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsSupportDetail = 0
                    Else
                        dr.IsSupportDetail = sheet.Cells(x, 14).Text
                    End If

                    dr.UrlAdd = sheet.Cells(x, 15).Text
                    dr.UrlEdit = sheet.Cells(x, 16).Text
                    dr.UrlDelete = sheet.Cells(x, 17).Text
                    dr.UrlActivation = sheet.Cells(x, 18).Text
                    dr.UrlView = sheet.Cells(x, 19).Text
                    dr.UrlUpload = sheet.Cells(x, 20).Text
                    dr.UrlApproval = sheet.Cells(x, 21).Text
                    dr.UrlApprovalDetail = sheet.Cells(x, 22).Text
                    dr.UrlDetail = sheet.Cells(x, 23).Text

                    If sheet.Cells(x, 24).Text = "" Or sheet.Cells(x, 24).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.IsUseStoreProcedureValidation = 0
                    Else
                        dr.IsUseStoreProcedureValidation = sheet.Cells(x, 24).Text
                    End If

                    ''Harus minimal ada satu checkbox yang dicentang
                    If dr.IsUseApproval = 0 And dr.IsSupportAdd = 0 And dr.IsSupportDelete = 0 And dr.IsSupportEdit = 0 And dr.IsSupportActivation = 0 And dr.IsSupportView = 0 And dr.IsSupportUpload = 0 And dr.IsSupportDetail = 0 Then
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "action must at least support View Or Upload"
                    End If

                    dr.ErrorMessage = ErrorMessage

                    'dt.Rows.Add(dr)
                    If dataparam.dtmodule Is Nothing Then
                        dataparam.dtmodule = New List(Of NawaDevBLL.datamodules.ClassModules)
                    End If
                    dataparam.dtmodule.Add(dr)
                    x += 1
                Else
                    x = 0
                End If
            End While

            'dt.TableName = "ModuleDesigner"

            'dataparam.dtmodule = dt


            storeModule.DataSource = dataparam.dtmodule
            storeModule.DataBind()
            gridModule.Hidden = False
        End Using
    End Sub

    Private Sub ReadModuleFieldSheet(ByVal sheet As ExcelWorksheet)
        ''Note: Apakah field-field boolean juga perlu dikasih default, atau dijadikan error kalau kosong?

        ''Validasi-validasi yang masih perlu ditambahkan:
        ''-Validasi field name pada setiap module harus unique (Perlu?)
        ''-Validasi table reference, table reference key, table reference name harus object yang valid (Perlu?)
        ''-Validasi Primary key harus Required dan Unik (Perlu?)
        Dim dt As New DataTable

        ' Define columns
        dt.Columns.Add("Action", GetType(System.String))
        dt.Columns.Add("ErrorMessage", GetType(System.String))

        ''Ambil list column/field table Module dari information schema
        Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'ModuleField' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)", Nothing)

        For Each item As DataRow In objdata.Rows
            If item(0).ToString = "FK_Module_ID" Then
                dt.Columns.Add("ModuleName", GetType(System.String))
            Else
                dt.Columns.Add(item(0), GetType(System.String))
            End If
        Next

        Using objdb As New NawaDAL.NawaDataEntities
            Dim x As Integer = 2

            While x <> 0
                Dim ErrorMessage As String = ""
                Dim CheckModuleFieldName As String = ""

                ''Cari nama module, jika kosong diasumsikan bahwa itu row terakhir
                CheckModuleFieldName = sheet.Cells(x, 3).Text

                If Not String.IsNullOrEmpty(CheckModuleFieldName) Then
                    Dim dr As Data.DataRow = dt.NewRow()

                    ''Kalau action kosong dianggap defaultnya insert seperti upload biasa
                    If sheet.Cells(x, 1).Text = "" Or sheet.Cells(x, 1).Text Is Nothing Then
                        dr.Item("Action") = "Insert"
                    Else
                        dr.Item("Action") = sheet.Cells(x, 1).Text
                    End If

                    dr.Item("PK_ModuleField_ID") = sheet.Cells(x, 2).Text
                    dr.Item("ModuleName") = sheet.Cells(x, 3).Text

                    If String.IsNullOrEmpty(sheet.Cells(x, 4).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Field Name is empty"
                    Else
                        ''Validasi action update/delete
                        If sheet.Cells(x, 1).Text = "Delete" Or sheet.Cells(x, 1).Text = "Update" Then
                            Dim moduleName As String = sheet.Cells(x, 3).Text
                            Dim fieldName As String = sheet.Cells(x, 4).Text
                            Dim moduledata As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleName(moduleName)
                            Dim modulefieldData As NawaDAL.ModuleField = NawaBLL.ModuleBLL.GetModuleFieldByModuleID(moduledata.PK_Module_ID).Find(Function(y) y.FieldName = fieldName)

                            If modulefieldData Is Nothing Then
                                ''Kalau nama ga ada, cek primary key
                                fieldName = sheet.Cells(x, 2).Text

                                modulefieldData = NawaBLL.ModuleBLL.GetModuleFieldByModuleID(moduledata.PK_Module_ID).Find(Function(y) y.PK_ModuleField_ID = fieldName)

                                If modulefieldData Is Nothing Then
                                    If ErrorMessage <> "" Then
                                        ErrorMessage = ErrorMessage & ", "
                                    End If
                                    ErrorMessage = ErrorMessage & "cannot find field in database"
                                End If
                            End If
                        End If

                        dr.Item("FieldName") = sheet.Cells(x, 4).Text
                    End If

                    If String.IsNullOrEmpty(sheet.Cells(x, 5).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Field Label is empty"
                    Else
                        dr.Item("FieldLabel") = sheet.Cells(x, 5).Text
                    End If

                    dr.Item("Sequence") = sheet.Cells(x, 6).Text

                    If sheet.Cells(x, 7).Text = "" Or sheet.Cells(x, 7).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("Required") = 0
                    Else
                        dr.Item("Required") = sheet.Cells(x, 7).Text
                    End If

                    If sheet.Cells(x, 8).Text = "" Or sheet.Cells(x, 8).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("IsPrimaryKey") = 0
                    Else
                        dr.Item("IsPrimaryKey") = sheet.Cells(x, 8).Text
                    End If

                    If sheet.Cells(x, 9).Text = "" Or sheet.Cells(x, 9).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("IsUnik") = 0
                    Else
                        dr.Item("IsUnik") = sheet.Cells(x, 9).Text
                    End If

                    If sheet.Cells(x, 10).Text = "" Or sheet.Cells(x, 10).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("IsShowInView") = 0
                    Else
                        dr.Item("IsShowInView") = sheet.Cells(x, 10).Text
                    End If

                    If sheet.Cells(x, 11).Text = "" Or sheet.Cells(x, 11).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("IsShowInForm") = 0
                    Else
                        dr.Item("IsShowInForm") = sheet.Cells(x, 11).Text
                    End If

                    dr.Item("DefaultValue") = sheet.Cells(x, 12).Text

                    If String.IsNullOrEmpty(sheet.Cells(x, 13).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Field Type is empty"
                    Else
                        dr.Item("FK_FieldType_ID") = sheet.Cells(x, 13).Text
                    End If

                    dr.Item("SizeField") = sheet.Cells(x, 14).Text

                    If String.IsNullOrEmpty(sheet.Cells(x, 15).Text) Then
                        ''Validasi mandatory
                        If ErrorMessage <> "" Then
                            ErrorMessage = ErrorMessage & ", "
                        End If
                        ErrorMessage = ErrorMessage & "Ext Type is empty"
                    Else
                        dr.Item("FK_ExtType_ID") = sheet.Cells(x, 15).Text
                    End If

                    dr.Item("TabelReferenceName") = sheet.Cells(x, 16).Text
                    dr.Item("TabelReferenceNameAlias") = sheet.Cells(x, 17).Text
                    dr.Item("TableReferenceFieldKey") = sheet.Cells(x, 18).Text
                    dr.Item("TableReferenceFieldDisplayName") = sheet.Cells(x, 19).Text
                    dr.Item("TableReferenceFilter") = sheet.Cells(x, 20).Text
                    dr.Item("IsUseRegexValidation") = sheet.Cells(x, 21).Text
                    dr.Item("TableReferenceAdditonalJoin") = sheet.Cells(x, 22).Text

                    If sheet.Cells(x, 23).Text = "" Or sheet.Cells(x, 23).Text Is Nothing Then
                        ''Kalau NULL dianggap false
                        dr.Item("BCasCade") = 0
                    Else
                        dr.Item("BCasCade") = sheet.Cells(x, 23).Text
                    End If

                    dr.Item("FieldNameParent") = sheet.Cells(x, 24).Text
                    dr.Item("FilterCascade") = sheet.Cells(x, 25).Text

                    dr.Item("ErrorMessage") = ErrorMessage

                    dt.Rows.Add(dr)

                    x += 1
                Else
                    x = 0
                End If
            End While

            dt.TableName = "ModuleField"

            dataparam.dtmodulefield = dt

            storeModuleField.DataSource = dt
            storeModuleField.DataBind()
            gridModuleField.Hidden = False
        End Using
    End Sub

    Private Function SubmitData(ByVal dtModule As NawaDevBLL.datamodules) As Boolean
        ''Note: Sekarang ini untuk module yang di flag sebagai update modulename tidak di update ke DB.
        ''Note: Sekarang ini module field yang di flag update juga tidak ada update fieldname dan moduleID.
        ''Note: Untuk module field apakah perlu diperlakukan sama seperti di module designer? Delete dan Insert.
        ''      Jadi tidak ada proses update.

        'Dim OldObj As NawaDAL.Module

        'Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
        '    OldObj = (From x In objDb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault
        'End Using

        Dim ModuleList As New List(Of Integer)
        Dim DeletedModule As String = ""

        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    ''Save Module
                    For Each item As NawaDevBLL.datamodules.ClassModules In dtModule.dtmodule 'DataRow In dtModule.Rows
                        If item.ErrorMessage.ToString <> "" And item.ErrorMessage.ToString IsNot Nothing Then
                            Throw New Exception("Record(s) with error(s) still exist on Module list. Module " & CStr(item.ModuleName) & ". Message:" & item.ErrorMessage)
                        Else
                            If item.Action.ToString = "Insert" Then
                                Dim newObj As New NawaDAL.Module
                                With newObj
                                    .ModuleName = CStr(item.ModuleName)
                                    .ModuleLabel = CStr(item.ModuleLabel)
                                    .ModuleDescription = CStr(item.ModuleDescription)
                                    .IsUseDesigner = CBool(item.IsUseDesigner)
                                    .IsUseApproval = CBool(item.IsUseApproval)
                                    .IsSupportAdd = CBool(item.IsSupportAdd)
                                    .IsSupportEdit = CBool(item.IsSupportEdit)
                                    .IsSupportDelete = CBool(item.IsSupportDelete)
                                    .IsSupportActivation = CBool(item.IsSupportActivation)
                                    .IsSupportView = CBool(item.IsSupportView)
                                    .IsSupportUpload = CBool(item.IsSupportUpload)
                                    .IsSupportDetail = CBool(item.IsSupportDetail)
                                    .UrlAdd = CStr(item.UrlAdd)
                                    .UrlEdit = CStr(item.UrlEdit)
                                    .UrlDelete = CStr(item.UrlDelete)
                                    .UrlActivation = CStr(item.UrlActivation)
                                    .UrlView = CStr(item.UrlView)
                                    .UrlUpload = CStr(item.UrlUpload)
                                    .UrlApproval = CStr(item.UrlApproval)
                                    .UrlApprovalDetail = CStr(item.UrlApprovalDetail)
                                    .UrlDetail = CStr(item.UrlDetail)
                                    .IsUseStoreProcedureValidation = CBool(item.IsUseStoreProcedureValidation)

                                    .CreatedDate = Now
                                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                                    .LastUpdateDate = Now
                                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                                End With

                                objdb.Entry(newObj).State = Entity.EntityState.Added
                                objdb.SaveChanges()

                                ''Untuk kebutuhan di save modulefield
                                item.PK_Module_ID = newObj.PK_Module_ID

                                ''Untuk diproses
                                ModuleList.Add(newObj.PK_Module_ID)
                            ElseIf item.Action.ToString = "Update" Then
                                Dim ID As String = item.PK_Module_ID.ToString

                                Dim modifiedObj As NawaDAL.Module = (From x In objdb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault
                                With modifiedObj
                                    .ModuleLabel = CStr(item.ModuleLabel)
                                    .ModuleDescription = CStr(item.ModuleDescription)
                                    .IsUseDesigner = CBool(item.IsUseDesigner)
                                    .IsUseApproval = CBool(item.IsUseApproval)
                                    .IsSupportAdd = CBool(item.IsSupportAdd)
                                    .IsSupportEdit = CBool(item.IsSupportEdit)
                                    .IsSupportDelete = CBool(item.IsSupportDelete)
                                    .IsSupportActivation = CBool(item.IsSupportActivation)
                                    .IsSupportView = CBool(item.IsSupportView)
                                    .IsSupportUpload = CBool(item.IsSupportUpload)
                                    .IsSupportDetail = CBool(item.IsSupportDetail)
                                    .UrlAdd = CStr(item.UrlAdd)
                                    .UrlEdit = CStr(item.UrlEdit)
                                    .UrlDelete = CStr(item.UrlDelete)
                                    .UrlActivation = CStr(item.UrlActivation)
                                    .UrlView = CStr(item.UrlView)
                                    .UrlUpload = CStr(item.UrlUpload)
                                    .UrlApproval = CStr(item.UrlApproval)
                                    .UrlApprovalDetail = CStr(item.UrlApprovalDetail)
                                    .UrlDetail = CStr(item.UrlDetail)
                                    .IsUseStoreProcedureValidation = CBool(item.IsUseStoreProcedureValidation)

                                    .LastUpdateDate = Now
                                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                                End With

                                objdb.Entry(modifiedObj).State = Entity.EntityState.Modified
                                objdb.SaveChanges()

                                ''Untuk diproses
                                ModuleList.Add(modifiedObj.PK_Module_ID)
                            ElseIf item.Action.ToString = "Delete" Then
                                Dim ID As String = item.PK_Module_ID.ToString

                                Dim deletedObj As NawaDAL.Module = (From x In objdb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault

                                ''Query untuk drop table module yang di delete
                                If DeletedModule <> "" Then
                                    DeletedModule = DeletedModule & " DROP TABLE " & deletedObj.ModuleName & " DROP TABLE " & deletedObj.ModuleName & "_Upload"
                                Else
                                    DeletedModule = "DROP TABLE " & deletedObj.ModuleName & " DROP TABLE " & deletedObj.ModuleName & "_Upload"
                                End If

                                objdb.Entry(deletedObj).State = Entity.EntityState.Deleted
                                objdb.SaveChanges()
                            Else
                                Throw New Exception("Undefined Action On Module list. Action" & item.Action)
                            End If
                        End If
                    Next

                    ''Save Module Field
                    Dim dtModuleField As DataTable = dtModule.dtmodulefield
                    For Each item As DataRow In dtModuleField.Rows
                        If item("ErrorMessage").ToString <> "" And item("ErrorMessage").ToString IsNot Nothing Then
                            Throw New Exception("Record(s) with error(s) still exist on Module Field list. Field " & CStr(item("FieldName")) & " at module " & CStr(item("ModuleName")) & ". Message:" & item("ErrorMessage"))
                        Else
                            If item("Action").ToString = "Insert" Then
                                ''Validasi Primary Key
                                Dim validateKey() As DataRow
                                validateKey = dtModuleField.Select("ModuleName = '" & CStr(item("ModuleName")) & "' And IsPrimaryKey=1")
                                If validateKey.Count = 0 Then
                                    Throw New Exception("No Primary Key found for module " & CStr(item("ModuleName")))
                                End If

                                Dim newObj As New NawaDAL.ModuleField
                                With newObj
                                    Dim ModuleID As Long = 0
                                    Try
                                        Dim Param As String = CStr(item("ModuleName"))
                                        ModuleID = (From x In objdb.Modules Where x.ModuleName = Param Select x).FirstOrDefault.PK_Module_ID

                                        If ModuleID = Nothing Or ModuleID = 0 Then
                                            Throw New Exception
                                        End If
                                    Catch ex As Exception
                                        Dim findRow() As DataRow

                                        For Each LstModule As NawaDevBLL.datamodules.ClassModules In dtModule.dtmodule
                                            If LstModule.ModuleName.ToUpper = CStr(item("ModuleName")).ToUpper Then
                                                ModuleID = LstModule.PK_Module_ID
                                            End If
                                        Next
                                        'findRow = dtModule.Select("ModuleName = '" & CStr(item("ModuleName")) & "'")

                                        ''Harusnya pasti return 1 row saja. Kalau lebih dari 1 berarti validasinya kelewat
                                        For Each item2 In findRow
                                            ModuleID = CLng(item2("PK_Module_ID"))
                                        Next
                                    End Try

                                    .FK_Module_ID = ModuleID

                                    .FieldName = CStr(item("FieldName"))
                                    .FieldLabel = CStr(item("FieldLabel"))
                                    .Sequence = CInt(item("Sequence"))
                                    .Required = CBool(item("Required"))
                                    .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                    .IsUnik = CBool(item("IsUnik"))
                                    .IsShowInView = CBool(item("IsShowInView"))
                                    .IsShowInForm = CBool(item("IsShowInForm"))
                                    .DefaultValue = CStr(item("DefaultValue"))

                                    Dim FieldTypeID As Integer = 0
                                    If CStr(item("FK_FieldType_ID")) <> "" And CStr(item("FK_FieldType_ID")) IsNot Nothing Then
                                        Dim Param As String = CStr(item("FK_FieldType_ID"))
                                        FieldTypeID = (From x In objdb.MFieldTypes Where x.FieldTypeCaption = Param Select x).FirstOrDefault.PK_FieldType_ID
                                    End If

                                    .FK_FieldType_ID = FieldTypeID

                                    .SizeField = CInt(item("SizeField"))

                                    Dim ExtTypeID As Integer = 0
                                    If CStr(item("FK_ExtType_ID")) <> "" And CStr(item("FK_ExtType_ID")) IsNot Nothing Then
                                        Dim Param As String = CStr(item("FK_ExtType_ID"))
                                        ExtTypeID = (From x In objdb.MExtTypes Where x.ExtTypeName = Param Select x).FirstOrDefault.PK_ExtType_ID
                                    End If

                                    .FK_ExtType_ID = ExtTypeID

                                    .TabelReferenceName = CStr(item("TabelReferenceName"))
                                    .TabelReferenceNameAlias = CStr(item("TabelReferenceNameAlias"))
                                    .TableReferenceFieldKey = CStr(item("TableReferenceFieldKey"))
                                    .TableReferenceFieldDisplayName = CStr(item("TableReferenceFieldDisplayName"))
                                    .TableReferenceFilter = CStr(item("TableReferenceFilter"))
                                    .IsUseRegexValidation = CBool(item("IsUseRegexValidation"))
                                    .TableReferenceAdditonalJoin = CStr(item("TableReferenceAdditonalJoin"))
                                    .BCasCade = CBool(item("BCasCade"))
                                    .FieldNameParent = CStr(item("FieldNameParent"))
                                    .FilterCascade = CStr(item("FilterCascade"))
                                End With

                                objdb.Entry(newObj).State = Entity.EntityState.Added
                                objdb.SaveChanges()
                            ElseIf item("Action").ToString = "Update" Then
                                ''Validasi Primary Key
                                Dim validateKey() As DataRow
                                validateKey = dtModuleField.Select("ModuleName = '" & CStr(item("ModuleName")) & "' And IsPrimaryKey=1")
                                If validateKey.Count = 0 Then
                                    Throw New Exception("No Primary Key found for module " & CStr(item("ModuleName")))
                                End If

                                Dim ID As String = item("PK_ModuleField_ID").ToString

                                Dim modifiedObj As NawaDAL.ModuleField = (From x In objdb.ModuleFields Where x.PK_ModuleField_ID = ID Select x).FirstOrDefault
                                With modifiedObj
                                    .FieldLabel = CStr(item("FieldLabel"))
                                    .Sequence = CInt(item("Sequence"))
                                    .Required = CBool(item("Required"))
                                    .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                    .IsUnik = CBool(item("IsUnik"))
                                    .IsShowInView = CBool(item("IsShowInView"))
                                    .IsShowInForm = CBool(item("IsShowInForm"))
                                    .DefaultValue = CStr(item("DefaultValue"))

                                    Dim FieldTypeID As Integer = 0
                                    If CStr(item("FK_FieldType_ID")) <> "" And CStr(item("FK_FieldType_ID")) IsNot Nothing Then
                                        Dim strFK_FieldType_ID As String = CStr(item("FK_FieldType_ID"))
                                        FieldTypeID = (From x In objdb.MFieldTypes Where x.FieldTypeCaption = strFK_FieldType_ID Select x).FirstOrDefault.PK_FieldType_ID
                                    End If

                                    .FK_FieldType_ID = FieldTypeID

                                    .SizeField = CInt(item("SizeField"))

                                    Dim ExtTypeID As Integer = 0
                                    If CStr(item("FK_ExtType_ID")) <> "" And CStr(item("FK_ExtType_ID")) IsNot Nothing Then
                                        Dim strFK_ExtType_ID As String = CStr(item("FK_ExtType_ID"))
                                        ExtTypeID = (From x In objdb.MExtTypes Where x.ExtTypeName = strFK_ExtType_ID Select x).FirstOrDefault.PK_ExtType_ID
                                    End If

                                    .FK_ExtType_ID = ExtTypeID

                                    .TabelReferenceName = CStr(item("TabelReferenceName"))
                                    .TabelReferenceNameAlias = CStr(item("TabelReferenceNameAlias"))
                                    .TableReferenceFieldKey = CStr(item("TableReferenceFieldKey"))
                                    .TableReferenceFieldDisplayName = CStr(item("TableReferenceFieldDisplayName"))
                                    .TableReferenceFilter = CStr(item("TableReferenceFilter"))
                                    .IsUseRegexValidation = CBool(item("IsUseRegexValidation"))
                                    .TableReferenceAdditonalJoin = CStr(item("TableReferenceAdditonalJoin"))
                                    .BCasCade = CBool(item("BCasCade"))
                                    .FieldNameParent = CStr(item("FieldNameParent"))
                                    .FilterCascade = CStr(item("FilterCascade"))
                                End With

                                objdb.Entry(modifiedObj).State = Entity.EntityState.Modified
                                objdb.SaveChanges()
                            ElseIf item("Action").ToString = "Delete" Then
                                Dim ID As String = item("PK_ModuleField_ID").ToString

                                Dim deletedObj As NawaDAL.ModuleField = (From x In objdb.ModuleFields Where x.PK_ModuleField_ID = ID Select x).FirstOrDefault

                                objdb.Entry(deletedObj).State = Entity.EntityState.Deleted
                                objdb.SaveChanges()
                            Else
                                Throw New Exception("Undefined Action on Module Field list. Action:" & item("Action"))
                            End If
                        End If
                    Next
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw New Exception(ex.Message)
                End Try
            End Using
        End Using

        Using objdb As New NawaDAL.NawaDataEntities
            ''Create, Alter, Drop table fisik
            ProcessModule(DeletedModule, ModuleList, objdb)
        End Using
    End Function

    Protected Sub ProcessModule(QueryDelete As String, ListModule As List(Of Integer), db As NawaDAL.NawaDataEntities)
        Try
            Dim parameters As SqlParameter() = {
                        New SqlParameter() With {.SqlDbType = SqlDbType.VarChar, .Size = -1, .ParameterName = "DeleteQuery", .Value = QueryDelete},
                        FillModuleList(ListModule)
                    }

            NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_Process_ModuleUpload", parameters)
            'Dim detailInfos As List(Of NawaDAL.Module) = db.Database.SqlQuery(Of NawaDAL.Module)("EXEC usp_Process_ModuleUpload @DeleteQuery, @ModuleList", parameters).ToList
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function FillModuleList(ListData As List(Of Integer)) As SqlParameter
        Dim tableSchema As SqlMetaData() = {
                    New SqlMetaData("ID", SqlDbType.Int)
                }

        Dim tableData As New List(Of SqlDataRecord)

        If ListData IsNot Nothing Then
            If ListData.Count > 0 Then
                For Each itemlist As Integer In ListData
                    Dim tableRow As New SqlDataRecord(tableSchema)
                    tableRow.SetInt32(0, itemlist)
                    tableData.Add(tableRow)
                Next
            Else
                tableData = Nothing
            End If
        Else
            tableData = Nothing
        End If

        Dim sqlParameter As New SqlParameter() With {
                .SqlDbType = SqlDbType.Structured,
                .ParameterName = "ModuleList",
                .TypeName = "[dbo].[LocationTableType]",
                .Value = tableData
            }

        Return sqlParameter
    End Function

    Public Sub GenerateReferences(package As ExcelPackage)
        Try
            ''Dropdown field di excel. 
            ''Table reference module field ga ada reference karena jadi terlalu panjang listnya. Ketik manual saja.
            Dim wsModule As ExcelWorksheet = package.Workbook.Worksheets("Module")
            If wsModule IsNot Nothing Then
                Dim Length As String = ""

                Length = "A2:A" & ((Int16.MaxValue) - 1).ToString
                ''Pilihan Action
                Dim validation As DataValidation.ExcelDataValidationList = wsModule.DataValidations.AddListValidation(Length)
                validation.Formula.Values.Add("Insert")
                validation.Formula.Values.Add("Update")
                validation.Formula.Values.Add("Delete")
            End If

            Dim wsModuleField As ExcelWorksheet = package.Workbook.Worksheets("ModuleField")
            If wsModuleField IsNot Nothing Then
                Dim Length As String = ""

                Length = "A2:A" & ((Int16.MaxValue) - 1).ToString
                ''Pilihan Action
                Dim validation As DataValidation.ExcelDataValidationList = wsModuleField.DataValidations.AddListValidation(Length)
                validation.Formula.Values.Add("Insert")
                validation.Formula.Values.Add("Update")
                validation.Formula.Values.Add("Delete")

                Using objdb As New NawaDAL.NawaDataEntities
                    Dim ListEntity As List(Of NawaDAL.MFieldType) = objdb.MFieldTypes.ToList

                    Length = "M2:M" & ((Int16.MaxValue) - 1).ToString
                    ''Field Type
                    Dim FieldType As DataValidation.ExcelDataValidationList = wsModuleField.DataValidations.AddListValidation(Length)

                    For Each item In ListEntity
                        FieldType.Formula.Values.Add(item.FieldTypeCaption)
                    Next
                End Using

                Using objdb As New NawaDAL.NawaDataEntities
                    Dim ListEntity As List(Of NawaDAL.MExtType) = objdb.MExtTypes.ToList

                    Length = "O2:O" & ((Int16.MaxValue) - 1).ToString
                    ''Ext Type
                    Dim ExtType As DataValidation.ExcelDataValidationList = wsModuleField.DataValidations.AddListValidation(Length)

                    For Each item In ListEntity
                        ExtType.Formula.Values.Add(item.ExtTypeName)
                    Next
                End Using
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function addNewModelField(FieldName As String) As Ext.Net.ModelField
        Dim objfield As Ext.Net.ModelField
        objfield = New Ext.Net.ModelField
        objfield.Name = FieldName
        Return objfield
    End Function

    Shared Function addNewColumn(ColumnText As String, ColumnIndex As String) As Ext.Net.Column
        Dim objcolum As Ext.Net.Column
        objcolum = New Ext.Net.Column
        objcolum.Text = ColumnText
        objcolum.DataIndex = ColumnIndex
        objcolum.ClientIDMode = Web.UI.ClientIDMode.Static
        Return objcolum
    End Function

    Private Sub LoadGridModel()
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Try
                ''Untuk generate column & model.
                modelModule.Fields.Add(addNewModelField("Action"))
                modelModule.Fields.Add(addNewModelField("ErrorMessage"))
                modelModule.Fields.Add(addNewModelField("PK_Module_ID"))
                modelModule.Fields.Add(addNewModelField("ModuleName"))
                modelModule.Fields.Add(addNewModelField("ModuleLabel"))
                modelModule.Fields.Add(addNewModelField("ModuleDescription"))
                modelModule.Fields.Add(addNewModelField("IsUseDesigner"))
                modelModule.Fields.Add(addNewModelField("IsUseApproval"))
                modelModule.Fields.Add(addNewModelField("IsSupportAdd"))
                modelModule.Fields.Add(addNewModelField("IsSupportEdit"))
                modelModule.Fields.Add(addNewModelField("IsSupportDelete"))
                modelModule.Fields.Add(addNewModelField("IsSupportActivation"))
                modelModule.Fields.Add(addNewModelField("IsSupportView"))
                modelModule.Fields.Add(addNewModelField("IsSupportUpload"))
                modelModule.Fields.Add(addNewModelField("IsSupportDetail"))
                modelModule.Fields.Add(addNewModelField("UrlAdd"))
                modelModule.Fields.Add(addNewModelField("UrlEdit"))
                modelModule.Fields.Add(addNewModelField("UrlDelete"))
                modelModule.Fields.Add(addNewModelField("UrlActivation"))
                modelModule.Fields.Add(addNewModelField("UrlView"))
                modelModule.Fields.Add(addNewModelField("UrlUpload"))
                modelModule.Fields.Add(addNewModelField("UrlApproval"))
                modelModule.Fields.Add(addNewModelField("UrlApprovalDetail"))
                modelModule.Fields.Add(addNewModelField("UrlDetail"))
                modelModule.Fields.Add(addNewModelField("IsUseStoreProcedureValidation"))

                gridModule.ColumnModel.Add(addNewColumn("Action", "Action"))
                gridModule.ColumnModel.Add(addNewColumn("Error Message", "ErrorMessage"))
                gridModule.ColumnModel.Add(addNewColumn("Module Name", "ModuleName"))
                gridModule.ColumnModel.Add(addNewColumn("Module Label", "ModuleLabel"))
                gridModule.ColumnModel.Add(addNewColumn("Module Description", "ModuleDescription"))
                gridModule.ColumnModel.Add(addNewColumn("Use Designer", "IsUseDesigner"))
                gridModule.ColumnModel.Add(addNewColumn("Use Approval", "IsUseApproval"))
                gridModule.ColumnModel.Add(addNewColumn("Support Add", "IsSupportAdd"))
                gridModule.ColumnModel.Add(addNewColumn("Support Edit", "IsSupportEdit"))
                gridModule.ColumnModel.Add(addNewColumn("Support Delete", "IsSupportDelete"))
                gridModule.ColumnModel.Add(addNewColumn("Support Activation", "IsSupportActivation"))
                gridModule.ColumnModel.Add(addNewColumn("Support View", "IsSupportView"))
                gridModule.ColumnModel.Add(addNewColumn("Support Upload", "IsSupportUpload"))
                gridModule.ColumnModel.Add(addNewColumn("Support Detail", "IsSupportDetail"))
                gridModule.ColumnModel.Add(addNewColumn("URL Add", "UrlAdd"))
                gridModule.ColumnModel.Add(addNewColumn("URL Edit", "UrlEdit"))
                gridModule.ColumnModel.Add(addNewColumn("URL Delete", "UrlDelete"))
                gridModule.ColumnModel.Add(addNewColumn("URL Activation", "UrlActivation"))
                gridModule.ColumnModel.Add(addNewColumn("URL View", "UrlView"))
                gridModule.ColumnModel.Add(addNewColumn("URL Upload", "UrlUpload"))
                gridModule.ColumnModel.Add(addNewColumn("URL Approval", "UrlApproval"))
                gridModule.ColumnModel.Add(addNewColumn("URL Approval Detail", "UrlApprovalDetail"))
                gridModule.ColumnModel.Add(addNewColumn("URL Detail", "UrlDetail"))
                gridModule.ColumnModel.Add(addNewColumn("Use Stored Procedure Validation", "IsUseStoreProcedureValidation"))

                ''Module Field
                modelModuleField.Fields.Add(addNewModelField("Action"))
                modelModuleField.Fields.Add(addNewModelField("ErrorMessage"))
                modelModuleField.Fields.Add(addNewModelField("PK_ModuleField_ID"))
                modelModuleField.Fields.Add(addNewModelField("ModuleName"))
                modelModuleField.Fields.Add(addNewModelField("FieldName"))
                modelModuleField.Fields.Add(addNewModelField("FieldLabel"))
                modelModuleField.Fields.Add(addNewModelField("Sequence"))
                modelModuleField.Fields.Add(addNewModelField("Required"))
                modelModuleField.Fields.Add(addNewModelField("IsPrimaryKey"))
                modelModuleField.Fields.Add(addNewModelField("IsUnik"))
                modelModuleField.Fields.Add(addNewModelField("IsShowInView"))
                modelModuleField.Fields.Add(addNewModelField("IsShowInForm"))
                modelModuleField.Fields.Add(addNewModelField("DefaultValue"))
                modelModuleField.Fields.Add(addNewModelField("FK_FieldType_ID"))
                modelModuleField.Fields.Add(addNewModelField("SizeField"))
                modelModuleField.Fields.Add(addNewModelField("FK_ExtType_ID"))
                modelModuleField.Fields.Add(addNewModelField("TabelReferenceName"))
                modelModuleField.Fields.Add(addNewModelField("TabelReferenceNameAlias"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceFieldKey"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceFieldDisplayName"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceFilter"))
                modelModuleField.Fields.Add(addNewModelField("IsUseRegexValidation"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceAdditonalJoin"))
                modelModuleField.Fields.Add(addNewModelField("BCasCade"))
                modelModuleField.Fields.Add(addNewModelField("FieldNameParent"))
                modelModuleField.Fields.Add(addNewModelField("FilterCascade"))

                gridModuleField.ColumnModel.Add(addNewColumn("Action", "Action"))
                gridModuleField.ColumnModel.Add(addNewColumn("Error Message", "ErrorMessage"))
                gridModuleField.ColumnModel.Add(addNewColumn("Module Name", "ModuleName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Name", "FieldName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Label", "FieldLabel"))
                gridModuleField.ColumnModel.Add(addNewColumn("Sequence", "Sequence"))
                gridModuleField.ColumnModel.Add(addNewColumn("Required", "Required"))
                gridModuleField.ColumnModel.Add(addNewColumn("Primary Key", "IsPrimaryKey"))
                gridModuleField.ColumnModel.Add(addNewColumn("Unik", "IsUnik"))
                gridModuleField.ColumnModel.Add(addNewColumn("Show in View", "IsShowInView"))
                gridModuleField.ColumnModel.Add(addNewColumn("Show in Form", "IsShowInForm"))
                gridModuleField.ColumnModel.Add(addNewColumn("Default Value", "DefaultValue"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Type", "FK_FieldType_ID"))
                gridModuleField.ColumnModel.Add(addNewColumn("Size Field", "SizeField"))
                gridModuleField.ColumnModel.Add(addNewColumn("Ext Type", "FK_ExtType_ID"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Name", "TabelReferenceName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Alias", "TabelReferenceNameAlias"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Key", "TableReferenceFieldKey"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Display Name", "TableReferenceFieldDisplayName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Filter", "TableReferenceFilter"))
                gridModuleField.ColumnModel.Add(addNewColumn("Use Regex", "IsUseRegexValidation"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Additional Join", "TableReferenceAdditonalJoin"))
                gridModuleField.ColumnModel.Add(addNewColumn("Cascade", "BCasCade"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Name Parent", "FieldNameParent"))
                gridModuleField.ColumnModel.Add(addNewColumn("Filter Cascade", "FilterCascade"))

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try
        End Using
    End Sub



End Class


