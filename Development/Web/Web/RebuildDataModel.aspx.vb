Imports System.IO
Imports OfficeOpenXml
Imports System.Data
Imports NawaDevDAL
Imports System.Data.SqlClient
Imports Microsoft.SqlServer.Server

Partial Class RebuildDataModel
    Inherits ParentPage

#Region "Session"
    Private Property ObjModule As NawaDAL.Module
        Get
            Return Session("RebuildDataModel.ObjModule")
        End Get
        Set(value As NawaDAL.Module)
            Session("RebuildDataModel.ObjModule") = value
        End Set
    End Property
    Private Property ListModule As List(Of NawaDevDAL.Module)
        Get
            Return Session("RebuildDataModel.ListModule")
        End Get
        Set(value As List(Of NawaDevDAL.Module))
            Session("RebuildDataModel.ListModule") = value
        End Set
    End Property
    Private Property ListModuleField As DataTable
        Get
            Return Session("RebuildDataModel.ListModuleField")
        End Get
        Set(value As DataTable)
            Session("RebuildDataModel.ListModuleField") = value
        End Set
    End Property
    Private Property ListModuleValidation As DataTable
        Get
            Return Session("RebuildDataModel.ListModuleValidation")
        End Get
        Set(value As DataTable)
            Session("RebuildDataModel.ListModuleValidation") = value
        End Set
    End Property
    Private Property ListValidationParam As DataTable
        Get
            Return Session("RebuildDataModel.ListValidationParam")
        End Get
        Set(value As DataTable)
            Session("RebuildDataModel.ListValidationParam") = value
        End Set
    End Property
    Private Property ListRegex As DataTable
        Get
            Return Session("RebuildDataModel.ListRegex")
        End Get
        Set(value As DataTable)
            Session("RebuildDataModel.ListRegex") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                ClearSession()

                Dim moduleEncrypt As String = Request.Params("ModuleID")
                Dim moduleID As Integer = NawaBLL.Common.DecryptQueryString(moduleEncrypt, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleID)

                If Me.ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                End If

                FormPanelInput.Title = Me.ObjModule.ModuleLabel
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Private Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        ''Untuk kalau perlu ada audit trail
        ActionType = NawaBLL.Common.ModuleActionEnum.view
    End Sub
#End Region

#Region "Method"
    Private Sub ClearSession()
        ObjModule = Nothing
        ListModule = New List(Of NawaDevDAL.[Module])
        ListModuleField = New DataTable
        ListModuleValidation = New DataTable
        ListValidationParam = New DataTable
        ListRegex = New DataTable
    End Sub

    Private Sub GetDownloadData(ByVal package As ExcelPackage)
        Using objDB As New NawaDatadevEntities
            'Load Data Module
            Dim listModule As List(Of NawaDevDAL.Module) = objDB.Modules.Where(Function(x) x.PK_Module_ID <= 100000 And objDB.Prefix_Module_Generator.Any(Function(y) x.ModuleName.StartsWith(y.TableName))).OrderBy(Function(z) z.PK_Module_ID).ToList()
            Dim listModuleID As List(Of Integer) = listModule.Select(Function(x) x.PK_Module_ID).ToList

            Dim wsModule As ExcelWorksheet = package.Workbook.Worksheets("Module")
            If wsModule IsNot Nothing Then
                Dim currentRow As Long = 2

                For Each item In listModule
                    wsModule.Cells(currentRow, 1).Value = item.PK_Module_ID
                    wsModule.Cells(currentRow, 2).Value = item.ModuleName
                    wsModule.Cells(currentRow, 3).Value = item.ModuleLabel
                    wsModule.Cells(currentRow, 4).Value = item.ModuleDescription
                    wsModule.Cells(currentRow, 5).Value = item.IsUseDesigner
                    wsModule.Cells(currentRow, 6).Value = item.IsUseApproval
                    wsModule.Cells(currentRow, 7).Value = item.IsSupportAdd
                    wsModule.Cells(currentRow, 8).Value = item.IsSupportEdit
                    wsModule.Cells(currentRow, 9).Value = item.IsSupportDelete
                    wsModule.Cells(currentRow, 10).Value = item.IsSupportActivation
                    wsModule.Cells(currentRow, 11).Value = item.IsSupportView
                    wsModule.Cells(currentRow, 12).Value = item.IsSupportUpload
                    wsModule.Cells(currentRow, 13).Value = item.IsSupportDetail
                    wsModule.Cells(currentRow, 14).Value = item.UrlAdd
                    wsModule.Cells(currentRow, 15).Value = item.UrlEdit
                    wsModule.Cells(currentRow, 16).Value = item.UrlDelete
                    wsModule.Cells(currentRow, 17).Value = item.UrlActivation
                    wsModule.Cells(currentRow, 18).Value = item.UrlView
                    wsModule.Cells(currentRow, 19).Value = item.UrlUpload
                    wsModule.Cells(currentRow, 20).Value = item.UrlApproval
                    wsModule.Cells(currentRow, 21).Value = item.UrlApprovalDetail
                    wsModule.Cells(currentRow, 22).Value = item.UrlDetail
                    wsModule.Cells(currentRow, 23).Value = item.IsUseStoreProcedureValidation

                    currentRow += 1
                Next
            End If

            'Load Data Module Field
            Dim listModuleField As List(Of ModuleField) = objDB.ModuleFields.Where(Function(x) listModuleID.Any(Function(y) x.FK_Module_ID = y)).OrderBy(Function(z) z.FK_Module_ID).ToList
            listModuleField.RemoveAll(Function(x) objDB.ANT_CustomFieldList.ToList.Any(Function(y) x.FK_Module_ID = y.FK_Module_ID And x.FieldName = y.FieldName))

            Dim listFieldID As List(Of Long) = listModuleField.Select(Function(x) x.PK_ModuleField_ID).ToList
            Dim wsModuleField As ExcelWorksheet = package.Workbook.Worksheets("ModuleField")
            If wsModuleField IsNot Nothing Then
                Dim currentRow As Long = 2

                For Each item In listModuleField
                    wsModuleField.Cells(currentRow, 1).Value = item.PK_ModuleField_ID

                    Dim moduleName As String = NawaBLL.ModuleBLL.GetModuleByModuleID(item.FK_Module_ID).ModuleName
                    If moduleName Is Nothing Then
                        wsModuleField.Cells(currentRow, 2).Value = "<Unidentified Module ID>"
                    Else
                        wsModuleField.Cells(currentRow, 2).Value = moduleName
                    End If

                    wsModuleField.Cells(currentRow, 3).Value = item.FieldName
                    wsModuleField.Cells(currentRow, 4).Value = item.FieldLabel
                    wsModuleField.Cells(currentRow, 5).Value = item.Sequence
                    wsModuleField.Cells(currentRow, 6).Value = item.Required
                    wsModuleField.Cells(currentRow, 7).Value = item.IsPrimaryKey
                    wsModuleField.Cells(currentRow, 8).Value = item.IsUnik
                    wsModuleField.Cells(currentRow, 9).Value = item.IsShowInView
                    wsModuleField.Cells(currentRow, 10).Value = item.IsShowInForm
                    wsModuleField.Cells(currentRow, 11).Value = item.DefaultValue

                    Dim objFieldType As MFieldType = (From x In objDB.MFieldTypes Where x.PK_FieldType_ID = item.FK_FieldType_ID Select x).FirstOrDefault
                    If objFieldType IsNot Nothing Then
                        wsModuleField.Cells(currentRow, 12).Value = objFieldType.FieldTypeCaption
                    End If

                    wsModuleField.Cells(currentRow, 13).Value = item.SizeField

                    Dim objExtType As MExtType = (From x In objDB.MExtTypes Where x.PK_ExtType_ID = item.FK_ExtType_ID Select x).FirstOrDefault
                    If objExtType IsNot Nothing Then
                        wsModuleField.Cells(currentRow, 14).Value = objExtType.ExtTypeName
                    End If

                    wsModuleField.Cells(currentRow, 15).Value = item.TabelReferenceName
                    wsModuleField.Cells(currentRow, 16).Value = item.TabelReferenceNameAlias
                    wsModuleField.Cells(currentRow, 17).Value = item.TableReferenceFieldKey
                    wsModuleField.Cells(currentRow, 18).Value = item.TableReferenceFieldDisplayName
                    wsModuleField.Cells(currentRow, 19).Value = item.TableReferenceFilter
                    wsModuleField.Cells(currentRow, 20).Value = item.IsUseRegexValidation
                    wsModuleField.Cells(currentRow, 21).Value = item.TableReferenceAdditonalJoin
                    wsModuleField.Cells(currentRow, 22).Value = item.BCasCade
                    wsModuleField.Cells(currentRow, 23).Value = item.FieldNameParent
                    wsModuleField.Cells(currentRow, 24).Value = item.FilterCascade

                    currentRow += 1
                Next
            End If

            'Load Data Module Validation
            Dim listModuleValidation As List(Of ModuleValidation) = objDB.ModuleValidations.Where(Function(x) listModuleID.Any(Function(y) x.FK_Module_ID = y)).OrderBy(Function(z) z.FK_Module_ID).ToList
            Dim wsModuleValidation As ExcelWorksheet = package.Workbook.Worksheets("ModuleValidation")
            If wsModuleValidation IsNot Nothing Then
                Dim currentRow As Long = 2

                For Each item In listModuleValidation
                    wsModuleValidation.Cells(currentRow, 1).Value = item.PK_ModuleValidation_ID

                    Dim moduleName As String = NawaBLL.ModuleBLL.GetModuleByModuleID(item.FK_Module_ID).ModuleName
                    If moduleName Is Nothing Then
                        wsModuleValidation.Cells(currentRow, 2).Value = "<Unidentified Module ID>"
                    Else
                        wsModuleValidation.Cells(currentRow, 2).Value = moduleName
                    End If

                    Dim objAction As ModuleAction = objDB.ModuleActions.FirstOrDefault(Function(x) x.PK_ModuleAction_ID = item.FK_ModuleAction_ID)
                    If Not objAction Is Nothing Then
                        wsModuleValidation.Cells(currentRow, 3).Value = objAction.ModuleActionName
                    End If

                    Dim objTime As ModuleTime = objDB.ModuleTimes.FirstOrDefault(Function(x) x.PK_ModuleTime_ID = item.FK_ModuleTime_ID)
                    If Not objTime Is Nothing Then
                        wsModuleValidation.Cells(currentRow, 4).Value = objTime.ModuleTimeName
                    End If

                    wsModuleValidation.Cells(currentRow, 5).Value = item.StoreProcedureName
                    wsModuleValidation.Cells(currentRow, 6).Value = item.StoreProcedureParameter
                    wsModuleValidation.Cells(currentRow, 7).Value = item.StoreProcedureParameterValueFieldSequence

                    currentRow += 1
                Next
            End If

            'Load Data Validation Parameter
            Dim listValidationParam As List(Of ValidationParameter) = objDB.ValidationParameters.Where(Function(x) x.Fk_Ref_KategoriValidasi_Id = 2 And listModuleID.Any(Function(y) x.TableName = y)).OrderBy(Function(z) z.TableName).ToList
            listValidationParam.RemoveAll(Function(x) objDB.ModuleFields.ToList.Where(Function(aa) objDB.ANT_CustomFieldList.ToList.Any(Function(bb) aa.FK_Module_ID = bb.FK_Module_ID And aa.FieldName = bb.FieldName)).Any(Function(y) y.FieldName = x.FieldName))

            Dim wsValidationParam As ExcelWorksheet = package.Workbook.Worksheets("ValidationParameter")
            If wsValidationParam IsNot Nothing Then
                Dim currentRow As Long = 2

                For Each item In listValidationParam
                    wsValidationParam.Cells(currentRow, 1).Value = item.PK_ValidationParameter

                    Dim moduleName As String = NawaBLL.ModuleBLL.GetModuleByModuleID(item.TableName).ModuleName
                    If moduleName Is Nothing Then
                        wsValidationParam.Cells(currentRow, 2).Value = "<Unidentified Module ID>"
                    Else
                        wsValidationParam.Cells(currentRow, 2).Value = moduleName
                    End If

                    wsValidationParam.Cells(currentRow, 3).Value = item.FieldName
                    wsValidationParam.Cells(currentRow, 4).Value = item.ExpressionType
                    wsValidationParam.Cells(currentRow, 5).Value = item.ValidationExpression
                    wsValidationParam.Cells(currentRow, 6).Value = item.Description
                    wsValidationParam.Cells(currentRow, 7).Value = item.ValidationMessage

                    Dim objValidType As ValidationType = objDB.ValidationTypes.FirstOrDefault(Function(x) x.PK_ValidationType_ID = item.ValidationType)
                    If Not objValidType Is Nothing Then
                        wsValidationParam.Cells(currentRow, 8).Value = objValidType.ValidationType1
                    End If

                    wsValidationParam.Cells(currentRow, 9).Value = item.ErrorType

                    Dim objDataSet As Ref_DataSet = objDB.Ref_DataSet.FirstOrDefault(Function(x) x.Pk_Ref_DataSet_Id = item.Fk_Ref_DataSet_Id)
                    If Not objDataSet Is Nothing Then
                        wsValidationParam.Cells(currentRow, 10).Value = objDataSet.Ref_DataSetName
                    End If

                    Dim objCategory As Ref_KategoriValidasi = objDB.Ref_KategoriValidasi.FirstOrDefault(Function(x) x.Pk_Ref_KategoriValidasi_Id = item.Fk_Ref_KategoriValidasi_Id)
                    If Not objCategory Is Nothing Then
                        wsValidationParam.Cells(currentRow, 11).Value = objCategory.Ref_KategoriValidasiName
                    End If

                    currentRow += 1
                Next
            End If

            'Load Data Module Field Regex
            Dim listRegex As List(Of ModuleFieldRegex) = objDB.ModuleFieldRegexes.Where(Function(x) listFieldID.Any(Function(y) x.FK_ModuleField_ID = y)).OrderBy(Function(z) z.FK_ModuleField_ID).ToList
            Dim wsRegex As ExcelWorksheet = package.Workbook.Worksheets("ModuleFieldRegex")
            If wsRegex IsNot Nothing Then
                Dim currentRow As Long = 2

                For Each item In listRegex
                    wsRegex.Cells(currentRow, 1).Value = item.PK_ModuleFieldRegex

                    Dim objField As ModuleField = listModuleField.FirstOrDefault(Function(x) x.PK_ModuleField_ID = item.FK_ModuleField_ID)
                    If Not objField Is Nothing Then
                        wsRegex.Cells(currentRow, 3).Value = objField.FieldName
                        Dim moduleName As String = NawaBLL.ModuleBLL.GetModuleByModuleID(objField.FK_Module_ID).ModuleName
                        If moduleName Is Nothing Then
                            wsRegex.Cells(currentRow, 2).Value = "<Unidentified Module ID>"
                        Else
                            wsRegex.Cells(currentRow, 2).Value = moduleName
                        End If
                    Else
                        wsRegex.Cells(currentRow, 3).Value = "<Unidentified Field>"
                    End If

                    wsRegex.Cells(currentRow, 4).Value = item.Regex

                    currentRow += 1
                Next
            End If
        End Using
    End Sub
    Private Function SaveFileUpload(ByVal fileUpload As FileUploadField) As String
        Dim filePath As String
        Dim fileName As String = ""

        Try
            'Save file upload ke file fisik di server untuk di-read
            If fileUpload.PostedFile.ContentLength <> 0 Then
                If fileUpload.HasFile Then
                    If fileUpload.FileName.Contains("\") Then
                        Dim listFileName() As String = fileUpload.FileName.Split("\")
                        fileName = listFileName(listFileName.Count - 1)
                    Else
                        fileName = fileUpload.FileName
                    End If

                    filePath = Server.MapPath("~/Temp/RebuildDataModel/" & fileName)
                    If System.IO.File.Exists(filePath) Then
                        Dim isExist As Boolean = True
                        Dim i As Integer = 1
                        While isExist
                            fileName = fileName.Split(CChar("."))(0) & "_" & i.ToString & "." & fileName.Split(CChar("."))(1)
                            filePath = Server.MapPath("~/Temp/RebuildDataModel/" & fileName)
                            If Not System.IO.File.Exists(filePath) Then
                                isExist = False
                                Exit While
                            End If
                            i = i + 1
                        End While
                    End If

                    System.IO.File.WriteAllBytes(filePath, fileUpload.FileBytes)
                End If
            End If

            Return fileName
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Function
    Private Sub RefreshGrid()
        Try
            GridModule.GetStore().DataSource = ListModule
            GridModule.GetStore().DataBind()

            GridModuleField.GetStore().DataSource = ListModuleField
            GridModuleField.GetStore().DataBind()

            GridModuleValidation.GetStore().DataSource = ListModuleValidation
            GridModuleValidation.GetStore().DataBind()

            GridValidationParam.GetStore().DataSource = ListValidationParam
            GridValidationParam.GetStore().DataBind()

            GridFieldRegex.GetStore().DataSource = ListRegex
            GridFieldRegex.GetStore().DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub ReadModuleSheet(ByVal sheet As ExcelWorksheet)
        Using objDB As New NawaDAL.NawaDataEntities
            Dim x As Integer = 2

            While x <> 0
                Dim dataModule As New NawaDevDAL.Module
                dataModule.ModuleName = sheet.Cells(x, 2).Value

                If Not String.IsNullOrEmpty(dataModule.ModuleName) Then
                    dataModule.PK_Module_ID = sheet.Cells(x, 1).Value
                    dataModule.ModuleLabel = sheet.Cells(x, 3).Value
                    dataModule.ModuleDescription = sheet.Cells(x, 4).Value
                    dataModule.IsUseDesigner = sheet.Cells(x, 5).Value
                    dataModule.IsUseApproval = sheet.Cells(x, 6).Value
                    dataModule.IsSupportAdd = sheet.Cells(x, 7).Value
                    dataModule.IsSupportEdit = sheet.Cells(x, 8).Value
                    dataModule.IsSupportDelete = sheet.Cells(x, 9).Value
                    dataModule.IsSupportActivation = sheet.Cells(x, 10).Value
                    dataModule.IsSupportView = sheet.Cells(x, 11).Value
                    dataModule.IsSupportUpload = sheet.Cells(x, 12).Value
                    dataModule.IsSupportDetail = sheet.Cells(x, 13).Value
                    dataModule.UrlAdd = sheet.Cells(x, 14).Value
                    dataModule.UrlEdit = sheet.Cells(x, 15).Value
                    dataModule.UrlDelete = sheet.Cells(x, 16).Value
                    dataModule.UrlActivation = sheet.Cells(x, 17).Value
                    dataModule.UrlView = sheet.Cells(x, 18).Value
                    dataModule.UrlUpload = sheet.Cells(x, 19).Value
                    dataModule.UrlApproval = sheet.Cells(x, 20).Value
                    dataModule.UrlApprovalDetail = sheet.Cells(x, 21).Value
                    dataModule.UrlDetail = sheet.Cells(x, 22).Value
                    dataModule.IsUseStoreProcedureValidation = sheet.Cells(x, 23).Value

                    ListModule.Add(dataModule)
                    x += 1
                Else
                    x = 0
                End If
            End While
        End Using
    End Sub
    Private Sub ReadModuleFieldSheet(ByVal sheet As ExcelWorksheet)
        'Ambil list column/field table ModuleField dari information schema
        Dim queryStr As String = "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'ModuleField' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)"
        Dim objDefault As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryStr, Nothing)
        For Each item As DataRow In objDefault.Rows
            ListModuleField.Columns.Add(item(0), GetType(System.String))
        Next

        ListModuleField.Columns.Add("ModuleName", GetType(System.String))
        ListModuleField.Columns.Add("FieldTypeCaption", GetType(System.String))
        ListModuleField.Columns.Add("ExtTypeName", GetType(System.String))

        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Dim x As Integer = 2

            While x <> 0
                If Not String.IsNullOrEmpty(sheet.Cells(x, 2).Value) Then
                    Dim objRow As Data.DataRow = ListModuleField.NewRow()
                    objRow.Item("PK_ModuleField_ID") = sheet.Cells(x, 1).Value
                    objRow.Item("ModuleName") = sheet.Cells(x, 2).Value

                    Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(y) y.ModuleName = objRow.Item("ModuleName"))
                    If Not objModule Is Nothing Then
                        objRow.Item("FK_Module_ID") = objModule.PK_Module_ID
                    End If

                    objRow.Item("FieldName") = sheet.Cells(x, 3).Value
                    objRow.Item("FieldLabel") = sheet.Cells(x, 4).Value
                    objRow.Item("Sequence") = sheet.Cells(x, 5).Value
                    objRow.Item("Required") = sheet.Cells(x, 6).Value
                    objRow.Item("IsPrimaryKey") = sheet.Cells(x, 7).Value
                    objRow.Item("IsUnik") = sheet.Cells(x, 8).Value
                    objRow.Item("IsShowInView") = sheet.Cells(x, 9).Value
                    objRow.Item("IsShowInForm") = sheet.Cells(x, 10).Value
                    objRow.Item("DefaultValue") = sheet.Cells(x, 11).Value
                    objRow.Item("FieldTypeCaption") = sheet.Cells(x, 12).Value
                    objRow.Item("FK_FieldType_ID") = objDB.MFieldTypes.ToList().FirstOrDefault(Function(y) y.FieldTypeCaption = objRow.Item("FieldTypeCaption")).PK_FieldType_ID
                    objRow.Item("SizeField") = sheet.Cells(x, 13).Value
                    objRow.Item("ExtTypeName") = sheet.Cells(x, 14).Value
                    objRow.Item("FK_ExtType_ID") = objDB.MExtTypes.ToList().FirstOrDefault(Function(y) y.ExtTypeName = objRow.Item("ExtTypeName")).PK_ExtType_ID
                    objRow.Item("TabelReferenceName") = sheet.Cells(x, 15).Value
                    objRow.Item("TabelReferenceNameAlias") = sheet.Cells(x, 16).Value
                    objRow.Item("TableReferenceFieldKey") = sheet.Cells(x, 17).Value
                    objRow.Item("TableReferenceFieldDisplayName") = sheet.Cells(x, 18).Value
                    objRow.Item("TableReferenceFilter") = sheet.Cells(x, 19).Value
                    objRow.Item("IsUseRegexValidation") = sheet.Cells(x, 20).Value
                    objRow.Item("TableReferenceAdditonalJoin") = sheet.Cells(x, 21).Value
                    objRow.Item("BCasCade") = sheet.Cells(x, 22).Value
                    objRow.Item("FieldNameParent") = sheet.Cells(x, 23).Value
                    objRow.Item("FilterCascade") = sheet.Cells(x, 24).Value

                    ListModuleField.Rows.Add(objRow)
                    x += 1
                Else
                    x = 0
                End If
            End While

            ListModuleField.TableName = "ModuleField"
        End Using
    End Sub
    Private Sub ReadModuleValidationSheet(ByVal sheet As ExcelWorksheet)
        'Ambil list column/field table ModuleValidation dari information schema
        Dim queryStr As String = "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'ModuleValidation' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)"
        Dim objDefault As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryStr, Nothing)
        For Each item As DataRow In objDefault.Rows
            ListModuleValidation.Columns.Add(item(0), GetType(System.String))
        Next

        ListModuleValidation.Columns.Add("ModuleName", GetType(System.String))
        ListModuleValidation.Columns.Add("ModuleActionName", GetType(System.String))
        ListModuleValidation.Columns.Add("ModuleTimeName", GetType(System.String))

        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Dim x As Integer = 2

            While x <> 0
                If Not String.IsNullOrEmpty(sheet.Cells(x, 2).Value) Then
                    Dim objRow As Data.DataRow = ListModuleValidation.NewRow()
                    objRow.Item("PK_ModuleValidation_ID") = sheet.Cells(x, 1).Value
                    objRow.Item("ModuleName") = sheet.Cells(x, 2).Value

                    Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(y) y.ModuleName = objRow.Item("ModuleName"))
                    If Not objModule Is Nothing Then
                        objRow.Item("FK_Module_ID") = objModule.PK_Module_ID
                    End If

                    objRow.Item("ModuleActionName") = sheet.Cells(x, 3).Value
                    Dim objAction As ModuleAction = objDB.ModuleActions.ToList.FirstOrDefault(Function(y) y.ModuleActionName = objRow.Item("ModuleActionName"))
                    If Not objAction Is Nothing Then
                        objRow.Item("FK_ModuleAction_ID") = objAction.PK_ModuleAction_ID
                    End If

                    objRow.Item("ModuleTimeName") = sheet.Cells(x, 4).Value
                    Dim objTime As ModuleTime = objDB.ModuleTimes.ToList.FirstOrDefault(Function(y) y.ModuleTimeName = objRow.Item("ModuleTimeName"))
                    If Not objTime Is Nothing Then
                        objRow.Item("FK_ModuleTime_ID") = objTime.PK_ModuleTime_ID
                    End If

                    objRow.Item("StoreProcedureName") = sheet.Cells(x, 5).Value
                    objRow.Item("StoreProcedureParameter") = sheet.Cells(x, 6).Value
                    objRow.Item("StoreProcedureParameterValueFieldSequence") = sheet.Cells(x, 7).Value

                    ListModuleValidation.Rows.Add(objRow)
                    x += 1
                Else
                    x = 0
                End If
            End While

            ListModuleValidation.TableName = "ModuleValidation"
        End Using
    End Sub
    Private Sub ReadValidationParamSheet(ByVal sheet As ExcelWorksheet)
        'Ambil list column/field table ModuleValidation dari information schema
        Dim queryStr As String = "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'ValidationParameter' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)"
        Dim objDefault As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryStr, Nothing)
        For Each item As DataRow In objDefault.Rows
            ListValidationParam.Columns.Add(item(0), GetType(System.String))
        Next

        ListValidationParam.Columns.Add("ModuleName", GetType(System.String))
        ListValidationParam.Columns.Add("FK_ValidationType_ID", GetType(System.Int32))
        ListValidationParam.Columns.Add("Ref_DataSetName", GetType(System.String))
        ListValidationParam.Columns.Add("Ref_KategoriValidasiName", GetType(System.String))

        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Dim x As Integer = 2

            While x <> 0
                If Not String.IsNullOrEmpty(sheet.Cells(x, 2).Value) Then
                    Dim objRow As Data.DataRow = ListValidationParam.NewRow()
                    objRow.Item("PK_ValidationParameter") = sheet.Cells(x, 1).Value

                    objRow.Item("ModuleName") = sheet.Cells(x, 2).Value
                    Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(y) y.ModuleName = objRow.Item("ModuleName"))
                    If Not objModule Is Nothing Then
                        objRow.Item("TableName") = objModule.PK_Module_ID
                    End If

                    objRow.Item("FieldName") = sheet.Cells(x, 3).Value
                    objRow.Item("ExpressionType") = sheet.Cells(x, 4).Value
                    objRow.Item("ValidationExpression") = sheet.Cells(x, 5).Value
                    objRow.Item("Description") = sheet.Cells(x, 6).Value
                    objRow.Item("ValidationMessage") = sheet.Cells(x, 7).Value

                    objRow.Item("ValidationType") = sheet.Cells(x, 8).Value
                    Dim objType As ValidationType = objDB.ValidationTypes.ToList.FirstOrDefault(Function(y) y.ValidationType1 = objRow.Item("ValidationType"))
                    If Not objType Is Nothing Then
                        objRow.Item("FK_ValidationType_ID") = objType.PK_ValidationType_ID
                    End If

                    objRow.Item("ErrorType") = sheet.Cells(x, 9).Value

                    objRow.Item("Ref_DataSetName") = sheet.Cells(x, 10).Value
                    Dim objDataSet As Ref_DataSet = objDB.Ref_DataSet.ToList.FirstOrDefault(Function(y) y.Ref_DataSetName = objRow.Item("Ref_DataSetName"))
                    If Not objDataSet Is Nothing Then
                        objRow.Item("FK_Ref_DataSet_ID") = objDataSet.Pk_Ref_DataSet_Id
                    End If

                    objRow.Item("Ref_KategoriValidasiName") = sheet.Cells(x, 11).Value
                    Dim objCategory As Ref_KategoriValidasi = objDB.Ref_KategoriValidasi.ToList.FirstOrDefault(Function(y) y.Ref_KategoriValidasiName = objRow.Item("Ref_KategoriValidasiName"))
                    If Not objCategory Is Nothing Then
                        objRow.Item("FK_Ref_KategoriValidasi_ID") = objCategory.Pk_Ref_KategoriValidasi_Id
                    End If

                    ListValidationParam.Rows.Add(objRow)
                    x += 1
                Else
                    x = 0
                End If
            End While

            ListValidationParam.TableName = "ValidationParameter"
        End Using
    End Sub
    Private Sub ReadFieldRegexSheet(ByVal sheet As ExcelWorksheet)
        'Ambil list column/field table ModuleValidation dari information schema
        Dim queryStr As String = "SELECT c.COLUMN_NAME FROM INFORMATION_SCHEMA.[COLUMNS] AS c WHERE c.TABLE_NAME = 'ModuleFieldRegex' AND COLUMN_NAME NOT IN (SELECT mfd.FieldName FROM ModuleFieldDefault AS mfd)"
        Dim objDefault As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryStr, Nothing)
        For Each item As DataRow In objDefault.Rows
            ListRegex.Columns.Add(item(0), GetType(System.String))
        Next

        ListRegex.Columns.Add("FK_Module_ID", GetType(System.Int32))
        ListRegex.Columns.Add("ModuleName", GetType(System.String))
        ListRegex.Columns.Add("FieldName", GetType(System.String))

        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Dim x As Integer = 2

            While x <> 0
                If Not String.IsNullOrEmpty(sheet.Cells(x, 2).Value) Then
                    Dim objRow As Data.DataRow = ListRegex.NewRow()
                    objRow.Item("PK_ModuleFieldRegex") = sheet.Cells(x, 1).Value
                    objRow.Item("ModuleName") = sheet.Cells(x, 2).Value

                    Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(y) y.ModuleName = objRow.Item("ModuleName"))
                    If Not objModule Is Nothing Then
                        objRow.Item("FK_Module_ID") = objModule.PK_Module_ID
                    End If

                    objRow.Item("FieldName") = sheet.Cells(x, 3).Value
                    Dim objField As ModuleField = objDB.ModuleFields.ToList.FirstOrDefault(Function(y) y.FieldName = objRow.Item("FieldName") And y.FK_Module_ID = objRow.Item("FK_Module_ID"))
                    If Not objField Is Nothing Then
                        objRow.Item("FK_ModuleField_ID") = objField.PK_ModuleField_ID
                    End If

                    objRow.Item("Regex") = sheet.Cells(x, 4).Value

                    ListRegex.Rows.Add(objRow)
                    x += 1
                Else
                    x = 0
                End If
            End While

            ListRegex.TableName = "ModuleFieldRegex"
        End Using
    End Sub

    Private Sub RebuildDataModel()
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    'Save Module
                    For Each item In ListModule
                        Dim objExistModule As NawaDevDAL.Module = objDB.Modules.FirstOrDefault(Function(x) x.ModuleName = item.ModuleName)
                        If objExistModule Is Nothing Then
                            'Insert Module
                            Dim queryInsert As String = " SET IDENTITY_INSERT Module ON; " & vbCrLf
                            queryInsert += " INSERT INTO Module (PK_Module_ID, ModuleName, ModuleLabel, ModuleDescription, IsUseDesigner, IsUseApproval, " & vbCrLf &
                                           " IsSupportAdd, IsSupportEdit, IsSupportDelete, IsSupportActivation, IsSupportView, IsSupportUpload, IsSupportDetail, " & vbCrLf &
                                           " UrlAdd, UrlEdit, UrlDelete, UrlActivation, UrlView, UrlUpload, UrlApproval, UrlApprovalDetail, UrlDetail, IsUseStoreProcedureValidation, " & vbCrLf &
                                           " [Active], CreatedBy, LastUpdateBy, ApprovedBy, CreatedDate, LastUpdateDate, ApprovedDate)"
                            queryInsert += " VALUES (@ModuleID, @ModuleName, @ModuleLabel, @Description, @IsUseDesigner, @IsUseApproval, " & vbCrLf &
                                           " @IsSupportAdd, @IsSupportEdit, @IsSupportDelete, @IsSupportActivation, @IsSupportView, @IsSupportUpload, @IsSupportDetail, " & vbCrLf &
                                           " @UrlAdd, @UrlEdit, @UrlDelete, @UrlActivation, @UrlView, @UrlUpload, @UrlApproval, @UrlApprovalDetail, @UrlDetail, @IsUseStoreProcedureValidation, " & vbCrLf &
                                           String.Format(" 1, '{0}', '{0}', '{0}', GETDATE(), GETDATE(), GETDATE())", NawaBLL.Common.SessionCurrentUser.UserID) & vbCrLf
                            queryInsert += " SET IDENTITY_INSERT Module OFF; "

                            Dim listModuleParam As New List(Of SqlParameter)
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@ModuleID", .Value = item.PK_Module_ID})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@ModuleName", .Value = item.ModuleName})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@ModuleLabel", .Value = item.ModuleLabel})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@Description", .Value = item.ModuleDescription})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsUseDesigner", .Value = item.IsUseDesigner})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsUseApproval", .Value = item.IsUseApproval})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportAdd", .Value = item.IsSupportAdd})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportEdit", .Value = item.IsSupportEdit})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportDelete", .Value = item.IsSupportDelete})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportActivation", .Value = item.IsSupportActivation})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportView", .Value = item.IsSupportView})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportUpload", .Value = item.IsSupportUpload})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsSupportDetail", .Value = item.IsSupportDetail})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlAdd", .Value = item.UrlAdd})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlEdit", .Value = item.UrlEdit})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlDelete", .Value = item.UrlDelete})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlActivation", .Value = item.UrlActivation})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlView", .Value = item.UrlView})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlUpload", .Value = item.UrlUpload})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlApproval", .Value = item.UrlApproval})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlApprovalDetail", .Value = item.UrlApprovalDetail})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@UrlDetail", .Value = item.UrlDetail})
                            listModuleParam.Add(New SqlParameter With {.ParameterName = "@IsUseStoreProcedureValidation", .Value = item.IsUseStoreProcedureValidation})

                            objDB.Database.SqlQuery(Of List(Of String))(queryInsert, listModuleParam.ToArray()).ToList
                        Else
                            'Update Module
                            With objExistModule
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

                            objDB.Entry(objExistModule).State = Entity.EntityState.Modified
                            objDB.SaveChanges()
                        End If
                    Next

                    'Save Module Field
                    Dim listDataField As New List(Of ModuleField)
                    For Each item As DataRow In ListModuleField.Rows
                        Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(x) x.ModuleName = item("ModuleName"))
                        If Not objModule Is Nothing Then
                            item("FK_Module_ID") = objModule.PK_Module_ID
                        Else
                            Throw New Exception("No module found for field: " & item("FieldName"))
                        End If

                        Dim objExistField As ModuleField = objDB.ModuleFields.ToList().FirstOrDefault(Function(x) x.FieldName = item("FieldName") And x.FK_Module_ID = objModule.PK_Module_ID)
                        If objExistField Is Nothing Then
                            'Insert ModuleField
                            Dim objNewField As New ModuleField
                            With objNewField
                                .FK_Module_ID = objModule.PK_Module_ID
                                .FieldName = CStr(item("FieldName"))
                                .FieldLabel = CStr(item("FieldLabel"))
                                .Sequence = CInt(item("Sequence"))
                                .Required = CBool(item("Required"))
                                .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                .IsUnik = CBool(item("IsUnik"))
                                .IsShowInView = CBool(item("IsShowInView"))
                                .IsShowInForm = CBool(item("IsShowInForm"))
                                .DefaultValue = CStr(item("DefaultValue"))
                                .FK_FieldType_ID = CInt(item("FK_FieldType_ID"))
                                .SizeField = CInt(item("SizeField"))
                                .FK_ExtType_ID = CInt(item("FK_ExtType_ID"))
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

                            objDB.Entry(objNewField).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            listDataField.Add(objNewField)
                        Else
                            'Update Module
                            With objExistField
                                .FieldLabel = CStr(item("FieldLabel"))
                                .Sequence = CInt(item("Sequence"))
                                .Required = CBool(item("Required"))
                                .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                .IsUnik = CBool(item("IsUnik"))
                                .IsShowInView = CBool(item("IsShowInView"))
                                .IsShowInForm = CBool(item("IsShowInForm"))
                                .DefaultValue = CStr(item("DefaultValue"))
                                .FK_FieldType_ID = CInt(item("FK_FieldType_ID"))
                                .SizeField = CInt(item("SizeField"))
                                .FK_ExtType_ID = CInt(item("FK_ExtType_ID"))
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

                            objDB.Entry(objExistField).State = Entity.EntityState.Modified
                            objDB.SaveChanges()

                            listDataField.Add(objExistField)
                        End If
                    Next

                    Dim listDeletedField As List(Of ModuleField) = objDB.ModuleFields.ToList().Where(Function(x) listDataField.Any(Function(y) y.FK_Module_ID = x.FK_Module_ID)).ToList
                    listDeletedField.RemoveAll(Function(x) listDataField.Any(Function(y) y.FK_Module_ID = x.FK_Module_ID And y.FieldName = x.FieldName))

                    'Filter delete selain yg ada di ANT_CustomFieldList
                    listDeletedField.RemoveAll(Function(x) objDB.ANT_CustomFieldList.ToList().Any(Function(y) x.FK_Module_ID = y.FK_Module_ID And x.FieldName = y.FieldName))

                    For Each itemDelete In listDeletedField
                        objDB.Entry(itemDelete).State = Entity.EntityState.Deleted
                        objDB.SaveChanges()
                    Next

                    'Save ModuleField dari ANT_CustomFieldList
                    Dim listFieldCustom As List(Of ANT_CustomFieldList) = objDB.ANT_CustomFieldList.ToList.Where(Function(x) ListModule.Any(Function(y) x.FK_Module_ID = y.PK_Module_ID)).ToList
                    For Each item In listFieldCustom
                        Dim objExistCustom As ModuleField = objDB.ModuleFields.ToList.FirstOrDefault(Function(x) x.FK_Module_ID = item.FK_Module_ID And x.FieldName = item.FieldName)
                        If Not objExistCustom Is Nothing Then
                            objExistCustom.Sequence = objDB.ModuleFields.ToList.Where(Function(aa) aa.FK_Module_ID = objExistCustom.FK_Module_ID).Select(Function(x) x.Sequence).Max() + 1

                            objDB.Entry(objExistCustom).State = Entity.EntityState.Modified
                            objDB.SaveChanges()
                        End If
                    Next

                    'Save Module Validation
                    Dim listDataValidation As New List(Of ModuleValidation)
                    For Each item As DataRow In ListModuleValidation.Rows
                        Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(x) x.ModuleName = item("ModuleName"))
                        If Not objModule Is Nothing Then
                            item("FK_Module_ID") = objModule.PK_Module_ID
                        Else
                            Throw New Exception("No module found: " & item("ModuleName"))
                        End If

                        Dim objExistValidation As ModuleValidation = objDB.ModuleValidations.ToList().FirstOrDefault(Function(x) x.FK_Module_ID = objModule.PK_Module_ID And x.FK_ModuleAction_ID = item("FK_ModuleAction_ID") And x.FK_ModuleTime_ID = item("FK_ModuleTime_ID"))
                        If objExistValidation Is Nothing Then
                            Dim objNewValidation As New ModuleValidation
                            With objNewValidation
                                .FK_Module_ID = objModule.PK_Module_ID
                                .FK_ModuleAction_ID = CInt(item("FK_ModuleAction_ID"))
                                .FK_ModuleTime_ID = CInt(item("FK_ModuleTime_ID"))
                                .StoreProcedureName = CStr(item("StoreProcedureName"))
                                .StoreProcedureParameter = CStr(item("StoreProcedureParameter"))
                                .StoreProcedureParameterValueFieldSequence = CStr(item("StoreProcedureParameterValueFieldSequence"))
                            End With

                            objDB.Entry(objNewValidation).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            listDataValidation.Add(objNewValidation)
                        Else
                            With objExistValidation
                                .StoreProcedureName = CStr(item("StoreProcedureName"))
                                .StoreProcedureParameter = CStr(item("StoreProcedureParameter"))
                                .StoreProcedureParameterValueFieldSequence = CStr(item("StoreProcedureParameterValueFieldSequence"))
                            End With

                            objDB.Entry(objExistValidation).State = Entity.EntityState.Modified
                            objDB.SaveChanges()

                            listDataValidation.Add(objExistValidation)
                        End If
                    Next

                    Dim listDeletedValidation As List(Of ModuleValidation) = objDB.ModuleValidations.ToList().Where(Function(x) ListModule.Any(Function(y) y.PK_Module_ID = x.FK_Module_ID)).ToList
                    listDeletedValidation.RemoveAll(Function(x) listDataValidation.Any(Function(y) y.FK_Module_ID = x.FK_Module_ID And y.FK_ModuleAction_ID = x.FK_ModuleAction_ID And y.FK_ModuleTime_ID = x.FK_ModuleTime_ID))

                    For Each itemDelete In listDeletedValidation
                        objDB.Entry(itemDelete).State = Entity.EntityState.Deleted
                        objDB.SaveChanges()
                    Next

                    'Save Validation Parameter
                    Dim listDeletedParam As List(Of ValidationParameter) = objDB.ValidationParameters.ToList().Where(Function(x) ListModule.Any(Function(y) y.PK_Module_ID = x.TableName And x.Fk_Ref_KategoriValidasi_Id = 2)).ToList
                    listDeletedParam.RemoveAll(Function(x) listFieldCustom.Any(Function(y) y.FK_Module_ID = x.TableName And y.FieldName = x.FieldName))

                    For Each itemDelete In listDeletedParam
                        objDB.Entry(itemDelete).State = Entity.EntityState.Deleted
                        objDB.SaveChanges()
                    Next

                    For Each item In ListValidationParam.Rows
                        Dim objModule As NawaDevDAL.Module = ListModule.FirstOrDefault(Function(x) x.ModuleName = item("ModuleName"))
                        If Not objModule Is Nothing Then
                            item("TableName") = objModule.PK_Module_ID
                        Else
                            Throw New Exception("No module found: " & item("ModuleName"))
                        End If

                        Dim objNewParam As New ValidationParameter
                        With objNewParam
                            .TableName = objModule.PK_Module_ID
                            .FieldName = CStr(item("FieldName"))
                            .ExpressionType = CStr(item("ExpressionType"))
                            .ValidationExpression = CStr(item("ValidationExpression"))
                            .Description = CStr(item("Description"))
                            .ValidationMessage = CStr(item("ValidationMessage"))
                            .ValidationType = CInt(item("FK_ValidationType_ID"))
                            .ErrorType = CStr(item("ErrorType"))
                            .Fk_Ref_DataSet_Id = CInt(item("Fk_Ref_DataSet_Id"))
                            .Fk_Ref_KategoriValidasi_Id = CInt(item("Fk_Ref_KategoriValidasi_Id"))
                            .Active = 1
                            .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                            .ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            .CreatedDate = DateTime.Now
                            .LastUpdateDate = DateTime.Now
                            .ApprovedDate = DateTime.Now
                        End With

                        objDB.Entry(objNewParam).State = Entity.EntityState.Added
                        objDB.SaveChanges()
                    Next

                    'Save Module Field Regex
                    Dim listDeletedRegex As List(Of ModuleFieldRegex) = objDB.ModuleFieldRegexes.ToList().Where(Function(x) listDataField.Any(Function(y) y.PK_ModuleField_ID = x.FK_ModuleField_ID)).ToList
                    listDeletedRegex.RemoveAll(Function(x) objDB.ModuleFields.ToList.Where(Function(aa) listFieldCustom.Any(Function(bb) aa.FK_Module_ID = bb.FK_Module_ID And aa.FieldName = bb.FieldName)).ToList.Any(Function(y) x.FK_ModuleField_ID = y.PK_ModuleField_ID))
                    For Each itemDelete In listDeletedRegex
                        objDB.Entry(itemDelete).State = Entity.EntityState.Deleted
                        objDB.SaveChanges()
                    Next

                    For Each item In ListRegex.Rows
                        Dim objModuleField As ModuleField = listDataField.FirstOrDefault(Function(x) x.FK_Module_ID = item("FK_Module_ID") And x.FieldName = item("FieldName"))
                        If objModuleField Is Nothing Then
                            Throw New Exception("No field found: " & item("FieldName"))
                        Else
                            Dim objNewRegex As New ModuleFieldRegex
                            With objNewRegex
                                .FK_ModuleField_ID = objModuleField.PK_ModuleField_ID
                                .Regex = CStr(item("Regex"))
                            End With

                            objDB.Entry(objNewRegex).State = Entity.EntityState.Added
                            objDB.SaveChanges()
                        End If
                    Next

                    'Generate Table Module
                    For Each itemRebuild In ListModule
                        Dim queryRebuild As String = String.Format("EXEC usp_generateTable {0}; EXEC usp_GenerateTableUpload {0}; ", itemRebuild.PK_Module_ID)
                        objDB.Database.SqlQuery(Of List(Of String))(queryRebuild, {}).ToList
                    Next

                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw ex
                End Try
            End Using
        End Using
    End Sub
#End Region

#Region "Direct Events"
    Protected Sub BtnDownloadData_OnClicked(sender As Object, e As EventArgs)
        Try
            Dim wb As FileInfo = New FileInfo(Server.MapPath("~/assets/templates/RebuildDataModelTemplate.xlsx"))
            Using package As ExcelPackage = New ExcelPackage(wb)
                GetDownloadData(package)

                Response.Clear()
                Response.AddHeader("content-disposition", "attachment;filename=RebuildDataModel_" & Now.ToString("yyMMdd") & ".xlsx")
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
    End Sub
    Protected Sub BtnUpload_OnClicked()
        Try
            Using objDB As New NawaDAL.NawaDataEntities
                ListModule = New List(Of NawaDevDAL.Module)
                ListModuleField = New DataTable
                ListModuleValidation = New DataTable
                ListValidationParam = New DataTable
                ListRegex = New DataTable

                If Not BtnAttachFile.HasFile Then
                    Ext.Net.X.Msg.Alert("Error", "No File Choosen").Show()
                Else
                    ''Directory file fisik yang di-write di server
                    Dim filePath As String = Server.MapPath("~/Temp/RebuildDataModel/")
                    Dim fileName As String = Me.SaveFileUpload(BtnAttachFile)
                    Dim reader As BinaryReader

                    ''Format yang diterima sementara hanya xlsx
                    If Not fileName.EndsWith(".xlsx") Then
                        Ext.Net.X.Msg.Alert("Error", "Unsupported Format File!").Show()
                    Else
                        Dim fileByte As Byte()
                        reader = New BinaryReader(System.IO.File.OpenRead(filePath & fileName))
                        fileByte = reader.ReadBytes(reader.BaseStream.Length)
                        Dim ms As MemoryStream = New MemoryStream(fileByte, 0, fileByte.Length)
                        ms.Write(fileByte, 0, fileByte.Length)

                        Using excelFile As New ExcelPackage(ms)
                            ReadModuleSheet(excelFile.Workbook.Worksheets("Module"))
                            GridModule.GetStore().DataSource = ListModule
                            GridModule.GetStore().DataBind()

                            ReadModuleFieldSheet(excelFile.Workbook.Worksheets("ModuleField"))
                            GridModuleField.GetStore().DataSource = ListModuleField
                            GridModuleField.GetStore().DataBind()

                            ReadModuleValidationSheet(excelFile.Workbook.Worksheets("ModuleValidation"))
                            GridModuleValidation.GetStore().DataSource = ListModuleValidation
                            GridModuleValidation.GetStore().DataBind()

                            ReadValidationParamSheet(excelFile.Workbook.Worksheets("ValidationParameter"))
                            GridValidationParam.GetStore().DataSource = ListValidationParam
                            GridValidationParam.GetStore().DataBind()

                            ReadFieldRegexSheet(excelFile.Workbook.Worksheets("ModuleFieldRegex"))
                            GridFieldRegex.GetStore().DataSource = ListRegex
                            GridFieldRegex.GetStore().DataBind()
                        End Using

                        ms.Flush()
                    End If
                End If
            End Using
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnRebuild_OnClicked()
        Try
            If Not BtnAttachFile.HasFile Then
                Throw New Exception("File must be choosen")
            ElseIf ListModule.Count = 0 Then
                Throw New Exception("There is no data to rebuild")
            End If

            RebuildDataModel()
            LblConfirmation.Text = "Rebuild Done. Click OK to Go Back."
            Panelconfirmation.Hidden = False
            FormPanelInput.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim moduleID As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & moduleID)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

#End Region

End Class


