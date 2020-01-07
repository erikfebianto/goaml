Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports OfficeOpenXml
Imports Ext.Net
Imports System.Data.SqlClient
Public Class ValidationReportBLL


    Public TableValidationReport As String
    Public TableValidationReportPKName As String



    Public objSchemaModule As NawaDAL.Module
    Public objSchemaModuleField As List(Of NawaDAL.ModuleField)
    Public objSchemaModuleFieldDefault As List(Of NawaDAL.ModuleFieldDefault)
    Public objshemaModuleValidation As List(Of NawaDAL.ModuleValidation)

    Private _ModuleID As String
    Public Property ModuleID() As String
        Get
            Return _ModuleID
        End Get
        Set(ByVal value As String)
            _ModuleID = value
        End Set
    End Property


    Private _ModuleName As String
    Public Property ModuleName() As String
        Get
            Return _ModuleName
        End Get
        Set(ByVal value As String)
            _ModuleName = value
        End Set
    End Property


    Shared Function GetSegmentDataToExportALLByRow(strfilter As String) As DataTable

        Dim sql As String

        sql = "SELECT DISTINCT SegmentData, "
        sql &= "       m.PK_Module_ID, "
        sql &= "       m.ModuleName, "
        sql &= "       m.modulelabel "
        sql &= "FROM   Web_ValidationReport_RowComplate A "
        'sql &= "       INNER JOIN Vw_SettingPersonal b "
        'sql &= "            ON  a.TanggalData = b.ReportDate "
        'sql &= "            AND a.KodeCabang = b.KdCabang "
        sql &= "       INNER JOIN Module m "
        sql &= "            ON  m.ModuleLabel = a.SegmentData "


        If strfilter <> "" Then
            sql &= " where " & strfilter
        End If




        Return SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, sql)
    End Function

    Shared Function GetSegmentDataToExport(strfilter As String) As DataTable
        Dim sql As String
        sql = "SELECT DISTINCT SegmentData,m.PK_Module_ID,m.ModuleName,m.modulelabel "
        sql &= "  FROM Web_ValidationReport_RowComplate B  "
        sql &= "  INNER JOIN Module m on m.ModuleLabel=b.SegmentData "

        If strfilter <> "" Then
            sql &= strfilter
        End If

        Return SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, sql)
    End Function



    Shared Function GetPKToExport(strfilter As String) As String

        Dim sql As String


        sql = "  DECLARE @List varchar(max)   "
        sql &= "   SELECT @List =COALESCE(@List + ', ', '') +  B.RecordID "
        sql &= "          FROM Web_ValidationReport_RowComplate B   "
        sql &= "          INNER JOIN Module m on m.ModuleLabel=b.SegmentData  "
        If strfilter <> "" Then
            sql &= strfilter
        End If

        sql &= "    select @List "


        Return SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, sql, Nothing)
    End Function
    Shared Function GetData() As List(Of NawaDevDAL.vw_ValidationReport)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.vw_ValidationReport.ToList
        End Using
    End Function




    Function GenerateCountSqlImportDataFieldALL(strfilteradditional As String, strmoduledata As String) As String

        Dim sql As String
        sql = "SELECT count(1) FROM   ValidationReport a        INNER JOIN Module m   ON  m.ModuleLabel = a.SegmentData   INNER JOIN  Vw_SettingPersonal b ON b.ReportDate=a.TanggalData AND b.KdCabang=a.KodeCabang WHERE  " & strfilteradditional & " "



        Return sql


    End Function


    Function GenerateSqlImportDataFieldALL(strfilteradditional As String, strmoduledata As String) As String

        Dim sql As String
        sql = "SELECT '' AS ID,'('+ CONVERT(VARCHAR(20), m.PK_Module_ID)+') ' +m.ModuleLabel AS module,'(' + (SELECT mf.FieldName FROM   ModuleField mf WHERE  mf.FK_Module_ID = m.PK_Module_ID AND mf.FieldLabel = a.NamaField AND mf.FK_Module_ID=m.PK_Module_ID )  + ') ' +       a.NamaField  AS namafield,    a.FieldValue,    '' AS ValueToKeep,        a.ReferenceField AS KeyField, a.ReferenceValue AS KeyFieldValue,        'FALSE' as sta,        a.ValidationMessage FROM   ValidationReport a        INNER JOIN Module m   ON  m.ModuleLabel = a.SegmentData  WHERE  " & strfilteradditional & " ORDER BY a.KeyFieldValue"



        Return sql


    End Function


    Shared Function ValidateBIInfo(liststrkode As Data.DataTable) As Boolean

        Dim objListParam(1) As SqlParameter
        objListParam(0) = New SqlParameter
        objListParam(1) = New SqlParameter



        objListParam(0).ParameterName = "@TableToValidateBI"
        objListParam(0).SqlDbType = SqlDbType.Structured
        objListParam(0).Value = liststrkode


        objListParam(1).ParameterName = "@userid"
        objListParam(1).Value = NawaBLL.Common.SessionCurrentUser.UserID





        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_ValidationAntasenaBI", objListParam)

    End Function
    Function GenerateCountSqlImportData(strfilteradditional As String, strmoduledata As String) As String


        Dim sql As String
        sql = "SELECT count(1) FROM   ValidationReport vr        INNER JOIN Module m             ON  m.ModuleLabel = vr.SegmentData WHERE  vr." & strfilteradditional & "  "



        Return sql


    End Function

    Function GenerateSqlImportData(strfilteradditional As String, strmoduledata As String) As String


        Dim sql As String
        sql = "SELECT '' AS ID,'('+ CONVERT(VARCHAR(20), m.PK_Module_ID)+') ' +m.ModuleLabel AS module, '(' + (SELECT mf.FieldName FROM   ModuleField mf WHERE  mf.FK_Module_ID = m.PK_Module_ID AND mf.FieldLabel = vr.NamaField AND mf.FK_Module_ID=m.PK_Module_ID )  + ') ' +       vr.NamaField  AS namafield,    vr.FieldValue  ,  '' AS ValueToKeep,       vr.ReferenceField AS KeyField, vr.ReferenceValue AS KeyFieldValue,        'FALSE' as sta,        vr.ValidationMessage FROM   ValidationReport vr        INNER JOIN Module m             ON  m.ModuleLabel = vr.SegmentData WHERE  vr." & strfilteradditional & " ORDER BY vr.KeyFieldValue "



        Return sql


    End Function



    Function CreateExcel2007WithData(pathFolder As String, strFilteradditional As String, strmoduledata As String) As String
        'done:tambah coding create excel dengan data




        Using objdb As New NawaDAL.NawaDataEntities
            objSchemaModule = objdb.Modules.Where(Function(x) x.ModuleName = "DataReplacer").FirstOrDefault

            If objSchemaModule.IsUseDesigner Then
                If Not objSchemaModule Is Nothing Then
                    objSchemaModuleField = objdb.ModuleFields.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID).OrderBy(Function(y) y.Sequence).ToList
                End If
                objSchemaModuleFieldDefault = objdb.ModuleFieldDefaults.OrderBy(Function(y) y.Sequence).ToList
                objshemaModuleValidation = objdb.ModuleValidations.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID And x.FK_ModuleAction_ID = Common.ModuleActionEnum.Update).ToList
            Else

            End If

        End Using


        Dim strsqlcount As String = GenerateCountSqlImportData(strFilteradditional, strmoduledata)
        Dim objcount As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsqlcount, Nothing)
        If objcount.Rows(0)(0) > 1048575 Then
            Throw New Exception("Export excel only support max 1.048.575 rows ")
        End If

        Dim strsql As String = GenerateSqlImportData(strFilteradditional, strmoduledata)

        Dim objtb As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)

        Dim intmaxrow As Integer
        If objtb.Rows.Count + 1000 > Integer.MaxValue Then
            intmaxrow = Integer.MaxValue
        Else

            intmaxrow = objtb.Rows.Count + 1000
        End If


        Dim strFileName As String = pathFolder & "\" & Guid.NewGuid.ToString
        While IO.File.Exists(strFileName)
            strFileName = pathFolder & "\" & Guid.NewGuid.ToString
        End While
        Dim objfs As New IO.FileInfo(strFileName)
        Using objpackage As New ExcelPackage(objfs)
            Dim objWorksheetData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Data")

            Dim objWorksheetParam As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Parameter")
            Dim intCounterReference As Integer = 1
            Dim intx As Integer = 1
            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    objWorksheetParam.Cells(intx, intCounterReference).Value = item.FieldLabel
                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParam.Cells(intx, intCounterReference).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 0 Then intx -= 1


                    objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParam.Cells(2, intCounterReference, intx, intCounterReference))


                    intCounterReference += 1
                End If

            Next
            objWorksheetParam.Hidden = eWorkSheetHidden.Hidden


            Dim objWorksheetParamData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("ParameterData")
            intCounterReference = 1
            intx = 1

            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then

                    objWorksheetParamData.Cells(intx, intCounterReference).Value = item.TableReferenceFieldKey
                    objWorksheetParamData.Cells(intx, intCounterReference + 1).Value = item.TableReferenceFieldDisplayName
                    objWorksheetParamData.Cells(intx, intCounterReference + 2).Value = item.FieldLabel


                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParamData.Cells(intx, intCounterReference).Value = item1(item.TableReferenceFieldKey)
                        objWorksheetParamData.Cells(intx, intCounterReference + 1).Value = item1(item.TableReferenceFieldDisplayName)
                        objWorksheetParamData.Cells(intx, intCounterReference + 2).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 2 Then intx -= 1


                    '  objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParamData.Cells(3, intCounterReference, intx, intCounterReference))


                    intCounterReference += 4
                End If

            Next



            'add operator list

            objWorksheetParam.Cells(1, intCounterReference).Value = "Action"
            objWorksheetParam.Cells(2, intCounterReference).Value = "Insert"
            objWorksheetParam.Cells(3, intCounterReference).Value = "Update"
            objWorksheetParam.Cells(4, intCounterReference).Value = "Delete"
            objpackage.Workbook.Names.Add("Action", objWorksheetParam.Cells(2, intCounterReference, 4, intCounterReference))



            objWorksheetData.Cells(1, 1).Value = "Action"
            objWorksheetData.Cells(1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(1, 1).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(1, 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(1, 1).Style.Font.Bold = True

            objWorksheetData.Cells(1, 1).AddComment("Please Select Action.", NawaBLL.Common.SessionCurrentUser.UserID)


            Dim objvalidation As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, 1, intmaxrow, 1).DataValidation.AddListDataValidation
            objvalidation.AllowBlank = True
            objvalidation.Formula.ExcelFormula = "Action"
            objvalidation.ShowErrorMessage = True



            Dim intDataX As Integer
            Dim intDatay As Integer
            intDataX = 1
            intDatay = 2
            'Dim strField As String = ""
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)

                'If item.FK_FieldType_ID <> NawaBLL.Common.MFieldType.ReferenceTable Then
                '    strField += item.FieldName & ","
                'Else
                '    strField += "(" item.FieldName & "),"
                'End If


                Select Case item.FK_FieldType_ID
                    Case NawaBLL.Common.MFieldType.ReferenceTable
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Select a Value from drop-down List.", NawaBLL.Common.SessionCurrentUser.UserID)

                        'tambahin validasi kalau picklist
                        Dim objvalidationdata As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).DataValidation.AddListDataValidation
                        objvalidationdata.AllowBlank = True
                        objvalidationdata.Formula.ExcelFormula = item.FieldName
                        objvalidationdata.ShowErrorMessage = True

                    Case NawaBLL.Common.MFieldType.IDENTITY


                        If item.IsPrimaryKey Then

                            objWorksheetData.Cells(intDataX, intDatay).AddComment("Mandatory for edit and Delete operation.", NawaBLL.Common.SessionCurrentUser.UserID)
                        Else
                            objWorksheetData.Cells(intDataX, intDatay).AddComment("ID is AutoNumber.", NawaBLL.Common.SessionCurrentUser.UserID)
                        End If


                    Case NawaBLL.Common.MFieldType.BIGINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Bigint ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.BooleanValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.DATETIMEValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Date with format(" & NawaBLL.SystemParameterBLL.GetDateFormat & ") ", NawaBLL.Common.SessionCurrentUser.UserID)
                        objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat

                    Case NawaBLL.Common.MFieldType.FLOATValue, NawaBLL.Common.MFieldType.MONEYValue, NawaBLL.Common.MFieldType.NUMERICDECIMALValue, NawaBLL.Common.MFieldType.REALValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Float", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.INTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Int(-2.147.483,648 to 2.147.483.648)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.SMALLINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Smallint(-32.768 to 32.768) ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.TINYINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Tinyint(0 to 255)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.VARCHARValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Varchar (Max length :" & item.SizeField & ")", NawaBLL.Common.SessionCurrentUser.UserID)







                End Select




                intDatay += 1
            Next



            If objSchemaModule.IsSupportActivation Then


                For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                    If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                        objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    End If
                Next
            End If


            objWorksheetData.Cells(intDataX, intDatay).Value = "Validation Error"
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(intDataX, intDatay).AddComment("Validation Result : ", NawaBLL.Common.SessionCurrentUser.UserID)

            'If strField.Length > 0 Then
            '    strField = strField.Remove(strField.Length - 1, 1)
            'End If
            'Dim strsql As String = "select " & strField & " from " & objSchemaModule.ModuleName



            objWorksheetData.Cells("B2").LoadFromDataTable(objtb, False)

            Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
            Dim intcolnumber As Integer = 0
            For Each item As System.Data.DataColumn In objtb.Columns
                If item.DataType = GetType(Date) Then
                    intcolnumber = intcolnumber + 1
                    objWorksheetData.Column(intcolnumber).Style.Numberformat.Format = dateformat
                End If
            Next

            objWorksheetData.Cells.AutoFitColumns()

            objpackage.SaveAs(objfs)
            Return strFileName

        End Using


    End Function


    Function CreateExcel2007WithDataALL(pathFolder As String, strFilteradditional As String, strmoduledata As String) As String
        'done:tambah coding create excel dengan data




        Using objdb As New NawaDAL.NawaDataEntities
            objSchemaModule = objdb.Modules.Where(Function(x) x.ModuleName = "DataReplacer").FirstOrDefault
            If Not objSchemaModule Is Nothing Then
                If objSchemaModule.IsUseDesigner Then
                    If Not objSchemaModule Is Nothing Then
                        objSchemaModuleField = objdb.ModuleFields.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID).OrderBy(Function(y) y.Sequence).ToList
                    End If
                    objSchemaModuleFieldDefault = objdb.ModuleFieldDefaults.OrderBy(Function(y) y.Sequence).ToList
                    objshemaModuleValidation = objdb.ModuleValidations.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID And x.FK_ModuleAction_ID = Common.ModuleActionEnum.Update).ToList
                Else

                End If
            Else
                objSchemaModuleField = New List(Of NawaDAL.ModuleField)
            End If

        End Using




        Dim sqlcount As String = GenerateCountSqlImportDataFieldALL(strFilteradditional, strmoduledata)

        Dim objcount As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, sqlcount, Nothing)

        If objcount.Rows(0)(0) > 1048575 Then
            Throw New Exception("Export excel only support max 1.048.575 rows ")
        End If



        Dim strsql As String = GenerateSqlImportDataFieldALL(strFilteradditional, strmoduledata)

        Dim objtb As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)

        Dim intmaxrow As Integer
        If objtb.Rows.Count + 1000 > Integer.MaxValue Then
            intmaxrow = Integer.MaxValue
        Else

            intmaxrow = objtb.Rows.Count + 1000
        End If


        Dim strFileName As String = pathFolder & Guid.NewGuid.ToString
        While IO.File.Exists(strFileName)
            strFileName = pathFolder & Guid.NewGuid.ToString
        End While
        Dim objfs As New IO.FileInfo(strFileName)
        Using objpackage As New ExcelPackage(objfs)
            Dim objWorksheetData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Data")

            Dim objWorksheetParam As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Parameter")
            Dim intCounterReference As Integer = 1
            Dim intx As Integer = 1
            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    objWorksheetParam.Cells(intx, intCounterReference).Value = item.FieldLabel
                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParam.Cells(intx, intCounterReference).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 0 Then intx -= 1


                    objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParam.Cells(2, intCounterReference, intx, intCounterReference))


                    intCounterReference += 1
                End If

            Next
            objWorksheetParam.Hidden = eWorkSheetHidden.Hidden


            Dim objWorksheetParamData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("ParameterData")
            intCounterReference = 1
            intx = 1

            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then

                    objWorksheetParamData.Cells(intx, intCounterReference).Value = item.TableReferenceFieldKey
                    objWorksheetParamData.Cells(intx, intCounterReference + 1).Value = item.TableReferenceFieldDisplayName
                    objWorksheetParamData.Cells(intx, intCounterReference + 2).Value = item.FieldLabel


                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParamData.Cells(intx, intCounterReference).Value = item1(item.TableReferenceFieldKey)
                        objWorksheetParamData.Cells(intx, intCounterReference + 1).Value = item1(item.TableReferenceFieldDisplayName)
                        objWorksheetParamData.Cells(intx, intCounterReference + 2).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 2 Then intx -= 1


                    '  objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParamData.Cells(3, intCounterReference, intx, intCounterReference))


                    intCounterReference += 4
                End If

            Next



            'add operator list

            objWorksheetParam.Cells(1, intCounterReference).Value = "Action"
            objWorksheetParam.Cells(2, intCounterReference).Value = "Insert"
            objWorksheetParam.Cells(3, intCounterReference).Value = "Update"
            objWorksheetParam.Cells(4, intCounterReference).Value = "Delete"
            objpackage.Workbook.Names.Add("Action", objWorksheetParam.Cells(2, intCounterReference, 4, intCounterReference))



            objWorksheetData.Cells(1, 1).Value = "Action"
            objWorksheetData.Cells(1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(1, 1).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(1, 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(1, 1).Style.Font.Bold = True

            objWorksheetData.Cells(1, 1).AddComment("Please Select Action.", NawaBLL.Common.SessionCurrentUser.UserID)


            Dim objvalidation As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, 1, intmaxrow, 1).DataValidation.AddListDataValidation
            objvalidation.AllowBlank = True
            objvalidation.Formula.ExcelFormula = "Action"
            objvalidation.ShowErrorMessage = True



            Dim intDataX As Integer
            Dim intDatay As Integer
            intDataX = 1
            intDatay = 2
            'Dim strField As String = ""
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)

                'If item.FK_FieldType_ID <> NawaBLL.Common.MFieldType.ReferenceTable Then
                '    strField += item.FieldName & ","
                'Else
                '    strField += "(" item.FieldName & "),"
                'End If


                Select Case item.FK_FieldType_ID
                    Case NawaBLL.Common.MFieldType.ReferenceTable
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Select a Value from drop-down List.", NawaBLL.Common.SessionCurrentUser.UserID)

                        'tambahin validasi kalau picklist
                        Dim objvalidationdata As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).DataValidation.AddListDataValidation
                        objvalidationdata.AllowBlank = True
                        objvalidationdata.Formula.ExcelFormula = item.FieldName
                        objvalidationdata.ShowErrorMessage = True

                    Case NawaBLL.Common.MFieldType.IDENTITY


                        If item.IsPrimaryKey Then

                            objWorksheetData.Cells(intDataX, intDatay).AddComment("Mandatory for edit and Delete operation.", NawaBLL.Common.SessionCurrentUser.UserID)
                        Else
                            objWorksheetData.Cells(intDataX, intDatay).AddComment("ID is AutoNumber.", NawaBLL.Common.SessionCurrentUser.UserID)
                        End If


                    Case NawaBLL.Common.MFieldType.BIGINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Bigint ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.BooleanValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.DATETIMEValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Date with format(" & NawaBLL.SystemParameterBLL.GetDateFormat & ") ", NawaBLL.Common.SessionCurrentUser.UserID)
                        objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat

                    Case NawaBLL.Common.MFieldType.FLOATValue, NawaBLL.Common.MFieldType.MONEYValue, NawaBLL.Common.MFieldType.NUMERICDECIMALValue, NawaBLL.Common.MFieldType.REALValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Float", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.INTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Int(-2.147.483,648 to 2.147.483.648)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.SMALLINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Smallint(-32.768 to 32.768) ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.TINYINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Tinyint(0 to 255)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.VARCHARValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Varchar (Max length :" & item.SizeField & ")", NawaBLL.Common.SessionCurrentUser.UserID)







                End Select




                intDatay += 1
            Next



            If objSchemaModule.IsSupportActivation Then


                For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                    If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                        objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    End If
                Next
            End If


            objWorksheetData.Cells(intDataX, intDatay).Value = "Validation Error"
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(intDataX, intDatay).AddComment("Validation Result : ", NawaBLL.Common.SessionCurrentUser.UserID)

            'If strField.Length > 0 Then
            '    strField = strField.Remove(strField.Length - 1, 1)
            'End If
            'Dim strsql As String = "select " & strField & " from " & objSchemaModule.ModuleName



            objWorksheetData.Cells("B2").LoadFromDataTable(objtb, False)

            Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
            Dim intcolnumber As Integer = 0
            For Each item As System.Data.DataColumn In objtb.Columns
                If item.DataType = GetType(Date) Then
                    intcolnumber = intcolnumber + 1
                    objWorksheetData.Column(intcolnumber).Style.Numberformat.Format = dateformat
                End If
            Next

            objWorksheetData.Cells.AutoFitColumns()

            objpackage.SaveAs(objfs)
            Return strFileName

        End Using


    End Function
    Function CreateExcel2007WithDataWithErrorMessage(pathFolder As String, strFilteradditional As String) As String
        'done:tambah coding create excel dengan data

        Dim lngFk_Module_Id As Long = 0
        Dim strKeyFieldValue As String = ""

        Using objdb As New NawaDAL.NawaDataEntities
            objSchemaModule = objdb.Modules.Where(Function(x) x.ModuleName = Me.ModuleName).FirstOrDefault
            lngFk_Module_Id = objSchemaModule.PK_Module_ID
            Using ObjDb2 As New NawaDatadevEntities
                Dim ObjLHBU_FormInfo As ORS_FormInfo = ObjDb2.ORS_FormInfo.Where(Function(x) x.FK_Module_ID = lngFk_Module_Id).FirstOrDefault
                If Not ObjLHBU_FormInfo Is Nothing Then
                    strKeyFieldValue = ObjLHBU_FormInfo.KeyField
                End If

            End Using

            If objSchemaModule.IsUseDesigner Then
                If Not objSchemaModule Is Nothing Then
                    objSchemaModuleField = objdb.ModuleFields.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID).OrderBy(Function(y) y.Sequence).ToList
                End If
                objSchemaModuleFieldDefault = objdb.ModuleFieldDefaults.OrderBy(Function(y) y.Sequence).ToList
                objshemaModuleValidation = objdb.ModuleValidations.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID And x.FK_ModuleAction_ID = Common.ModuleActionEnum.Update).ToList
            Else

            End If

        End Using




        Dim strFileName As String = pathFolder & "\" & Guid.NewGuid.ToString
        While IO.File.Exists(strFileName)
            strFileName = pathFolder & "\" & Guid.NewGuid.ToString
        End While
        Dim objfs As New IO.FileInfo(strFileName)
        Using objpackage As New ExcelPackage(objfs)
            Dim objWorksheetData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Data")



            Dim objWorksheetParam As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Parameter")
            Dim intCounterReference As Integer = 1
            Dim intx As Integer = 1
            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    objWorksheetParam.Cells(intx, intCounterReference).Value = item.FieldLabel
                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParam.Cells(intx, intCounterReference).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 0 Then intx -= 1


                    objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParam.Cells(2, intCounterReference, intx, intCounterReference))


                    intCounterReference += 1
                End If

            Next


            objWorksheetParam.Hidden = eWorkSheetHidden.Hidden

            'add operator list

            objWorksheetParam.Cells(1, intCounterReference).Value = "Action"
            objWorksheetParam.Cells(2, intCounterReference).Value = "Insert"
            objWorksheetParam.Cells(3, intCounterReference).Value = "Update"
            objWorksheetParam.Cells(4, intCounterReference).Value = "Delete"
            objpackage.Workbook.Names.Add("Action", objWorksheetParam.Cells(2, intCounterReference, 4, intCounterReference))



            objWorksheetData.Cells(1, 1).Value = "Action"
            objWorksheetData.Cells(1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(1, 1).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(1, 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(1, 1).Style.Font.Bold = True

            objWorksheetData.Cells(1, 1).AddComment("Please Select Action.", NawaBLL.Common.SessionCurrentUser.UserID)

            Dim sqlcount As String = GenerateCountSqlImportDataWithErrorMessage(strFilteradditional)
            Dim objcount As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, sqlcount, Nothing)


            If objcount.Rows(0)(0) > 1048575 Then
                Throw New Exception("Export excel only support max 1.048.575 rows ")
            End If


            Dim strsql As String = GenerateSqlImportDataWithErrorMessage(strFilteradditional)

            Dim objtb As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)
            Dim intmaxrow As Integer
            If objtb.Rows.Count + 1000 > Integer.MaxValue Then
                intmaxrow = Integer.MaxValue
            Else

                intmaxrow = objtb.Rows.Count + 1000
            End If

            Dim objvalidation As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, 1, intmaxrow, 1).DataValidation.AddListDataValidation
            objvalidation.AllowBlank = True
            objvalidation.Formula.ExcelFormula = "Action"
            objvalidation.ShowErrorMessage = True



            Dim intDataX As Integer
            Dim intDatay As Integer
            intDataX = 1
            intDatay = 2

            Dim lstDateFieldIndex As New List(Of Integer)

            'Dim strField As String = ""
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)


                Select Case item.FK_FieldType_ID
                    Case NawaBLL.Common.MFieldType.ReferenceTable
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Select a Value from drop-down List.", NawaBLL.Common.SessionCurrentUser.UserID)

                        'tambahin validasi kalau picklist
                        Dim objvalidationdata As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).DataValidation.AddListDataValidation
                        objvalidationdata.AllowBlank = True
                        objvalidationdata.Formula.ExcelFormula = item.FieldName
                        objvalidationdata.ShowErrorMessage = True

                    Case NawaBLL.Common.MFieldType.IDENTITY


                        If item.IsPrimaryKey Then

                            objWorksheetData.Cells(intDataX, intDatay).AddComment("Mandatory for edit and Delete operation.", NawaBLL.Common.SessionCurrentUser.UserID)
                        Else
                            objWorksheetData.Cells(intDataX, intDatay).AddComment("ID is AutoNumber.", NawaBLL.Common.SessionCurrentUser.UserID)
                        End If


                    Case NawaBLL.Common.MFieldType.BIGINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Bigint ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.BooleanValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.DATETIMEValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Date with format(" & NawaBLL.SystemParameterBLL.GetDateFormat & ") ", NawaBLL.Common.SessionCurrentUser.UserID)
                        objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                        lstDateFieldIndex.Add(intDatay)

                    Case NawaBLL.Common.MFieldType.FLOATValue, NawaBLL.Common.MFieldType.MONEYValue, NawaBLL.Common.MFieldType.NUMERICDECIMALValue, NawaBLL.Common.MFieldType.REALValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Float", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.INTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Int(-2.147.483,648 to 2.147.483.648)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.SMALLINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Smallint(-32.768 to 32.768) ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.TINYINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Tinyint(0 to 255)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.VARCHARValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Varchar (Max length :" & item.SizeField & ")", NawaBLL.Common.SessionCurrentUser.UserID)







                End Select




                intDatay += 1
            Next



            If objSchemaModule.IsSupportActivation Then


                For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                    If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                        objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    End If
                Next
                intDatay += 1
            End If



            'tambah setting validation

            objWorksheetData.Cells(intDataX, intDatay).Value = "Validation Error"
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(intDataX, intDatay).AddComment("Validation Result : ", NawaBLL.Common.SessionCurrentUser.UserID)


            'If strField.Length > 0 Then
            '    strField = strField.Remove(strField.Length - 1, 1)
            'End If
            'Dim strsql As String = "select " & strField & " from " & objSchemaModule.ModuleName


            objWorksheetData.Cells("B2").LoadFromDataTable(objtb, False)
            If objtb.Rows.Count > 0 Then
                objWorksheetData.Cells("A2:A" & objtb.Rows.Count + 1).Value = "Update"
            End If

            'Cek Data yg Invalid, utk yg invalid tambah comment validation message & beri warna cell
            Dim LstFieldName As New List(Of String)
            For y As Integer = 0 To objtb.Columns.Count - 1
                LstFieldName.Add(objtb.Columns(y).ColumnName)
            Next

            Dim namedStyle = objWorksheetData.Workbook.Styles.CreateNamedStyle("ErrorField")
            namedStyle.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            namedStyle.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)

            Dim namedStyleDate = objWorksheetData.Workbook.Styles.CreateNamedStyle("ErrorFieldDate")
            namedStyleDate.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            namedStyleDate.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
            namedStyleDate.Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat

            Using ObjDb As New NawaDatadevEntities
                For intRow As Integer = 0 To objtb.Rows.Count - 1
                    Dim LstErrorField As New List(Of String)
                    Dim LstErrorMessage As New List(Of String)

                    Dim lngRowId As Long = objtb.Rows(intRow).Item(strKeyFieldValue).ToString

                    Dim ObjvalidationReportList As List(Of Vw_validationReport_List) = ObjDb.Vw_validationReport_List.Where(Function(x) x.ModuleID = lngFk_Module_Id And x.KeyFieldValue = lngRowId).ToList
                    For Each objValidationReport As Vw_validationReport_List In ObjvalidationReportList
                        If LstErrorField.Contains(objValidationReport.FieldName) Then
                            LstErrorMessage(LstErrorField.IndexOf(objValidationReport.FieldName)) = LstErrorMessage(LstErrorField.IndexOf(objValidationReport.FieldName)) &
                                vbCrLf & objValidationReport.ValidationMessage
                        Else
                            LstErrorField.Add(objValidationReport.FieldName)
                            LstErrorMessage.Add(objValidationReport.ValidationMessage)
                        End If
                    Next

                    'Update ke Cell
                    Dim intRowIndex As Integer = intRow + 2
                    Dim intColIndex As Integer = 0
                    For x As Integer = 0 To LstErrorField.Count - 1
                        If LstFieldName.Contains(LstErrorField(x)) Then
                            intColIndex = LstFieldName.IndexOf(LstErrorField(x)) + 2
                            objWorksheetData.Cells(intRowIndex, intColIndex).AddComment(LstErrorMessage(x), NawaBLL.Common.SessionCurrentUser.UserID)

                            If lstDateFieldIndex.Contains(intColIndex) Then
                                objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorFieldDate"
                            Else
                                objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorField"
                            End If
                        End If
                    Next

                Next
            End Using

            'Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
            'Dim intcolnumber As Integer = 0
            'For Each item As System.Data.DataColumn In objtb.Columns
            '    If item.DataType = GetType(Date) Then
            '        intcolnumber = intcolnumber + 1
            '        objWorksheetData.Column(intcolnumber).Style.Numberformat.Format = dateformat
            '    End If
            'Next

            objWorksheetData.Cells.AutoFitColumns()
            intDatay = 2
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                If item.IsShowInForm Then
                    objWorksheetData.Column(intDatay).Hidden = False
                Else
                    objWorksheetData.Column(intDatay).Hidden = True
                End If
                intDatay += 1
            Next
            objpackage.SaveAs(objfs)
            Return strFileName

        End Using


    End Function
    Function CreateExcel2007WithDataWithErrorMessageALLRow(pathFolder As String, strFilteradditional As String, indexpage As Integer, intpagesize As Integer) As String
        'done:tambah coding create excel dengan data

        Dim lngFk_Module_Id As Long = 0
        Dim strKeyFieldValue As String = ""


        Using objdb As New NawaDAL.NawaDataEntities
            objSchemaModule = objdb.Modules.Where(Function(x) x.ModuleName = Me.ModuleName).FirstOrDefault
            lngFk_Module_Id = objSchemaModule.PK_Module_ID
            Using ObjDb2 As New NawaDatadevEntities
                Dim ObjLHBU_FormInfo As ORS_FormInfo = ObjDb2.ORS_FormInfo.Where(Function(x) x.FK_Module_ID = lngFk_Module_Id).FirstOrDefault
                If Not ObjLHBU_FormInfo Is Nothing Then
                    strKeyFieldValue = ObjLHBU_FormInfo.KeyField
                End If

            End Using

            If objSchemaModule.IsUseDesigner Then
                If Not objSchemaModule Is Nothing Then
                    objSchemaModuleField = objdb.ModuleFields.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID).OrderBy(Function(y) y.Sequence).ToList
                End If
                objSchemaModuleFieldDefault = objdb.ModuleFieldDefaults.OrderBy(Function(y) y.Sequence).ToList
                objshemaModuleValidation = objdb.ModuleValidations.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID And x.FK_ModuleAction_ID = Common.ModuleActionEnum.Update).ToList
            Else

            End If

        End Using




        Dim strFileName As String = pathFolder & "\" & Guid.NewGuid.ToString
        While IO.File.Exists(strFileName)
            strFileName = pathFolder & "\" & Guid.NewGuid.ToString
        End While
        Dim objfs As New IO.FileInfo(strFileName)
        Using objpackage As New ExcelPackage(objfs)
            Dim objWorksheetData As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Data")



            Dim objWorksheetParam As ExcelWorksheet = objpackage.Workbook.Worksheets.Add("Parameter")
            Dim intCounterReference As Integer = 1
            Dim intx As Integer = 1
            For Each item As NawaDAL.ModuleField In objSchemaModuleField

                intx = 1
                If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    objWorksheetParam.Cells(intx, intCounterReference).Value = item.FieldLabel
                    intx += 1
                    Dim strquery As String = NawaBLL.Common.GetQueryRef(item.TabelReferenceName, item.TableReferenceFieldKey, item.TableReferenceFieldDisplayName, "", item.TableReferenceFieldDisplayName)
                    Dim objdata As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

                    For Each item1 As DataRow In objdata.Rows
                        objWorksheetParam.Cells(intx, intCounterReference).Value = "(" & item1(item.TableReferenceFieldKey) & ") " & item1(item.TableReferenceFieldDisplayName)

                        intx += 1

                    Next
                    If intx > 0 Then intx -= 1


                    objpackage.Workbook.Names.Add(item.FieldName, objWorksheetParam.Cells(2, intCounterReference, intx, intCounterReference))


                    intCounterReference += 1
                End If

            Next


            objWorksheetParam.Hidden = eWorkSheetHidden.Hidden

            'add operator list

            objWorksheetParam.Cells(1, intCounterReference).Value = "Action"
            objWorksheetParam.Cells(2, intCounterReference).Value = "Insert"
            objWorksheetParam.Cells(3, intCounterReference).Value = "Update"
            objWorksheetParam.Cells(4, intCounterReference).Value = "Delete"
            objpackage.Workbook.Names.Add("Action", objWorksheetParam.Cells(2, intCounterReference, 4, intCounterReference))



            objWorksheetData.Cells(1, 1).Value = "Action"
            objWorksheetData.Cells(1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(1, 1).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(1, 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(1, 1).Style.Font.Bold = True

            objWorksheetData.Cells(1, 1).AddComment("Please Select Action.", NawaBLL.Common.SessionCurrentUser.UserID)



            Dim strsql As String = GenerateSqlImportDataWithErrorMessageALL(strFilteradditional)




            ' Dim objtb As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)
            Dim objtb As DataTable = NawaDevBLL.NawaFramework.ExecutePaging(strsql, "", indexpage, intpagesize)
            Dim intmaxrow As Integer
            If objtb.Rows.Count + 1000 > intpagesize Then
                intmaxrow = intpagesize
            Else

                intmaxrow = objtb.Rows.Count + 1000
            End If

            Dim objvalidation As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, 1, intmaxrow, 1).DataValidation.AddListDataValidation
            objvalidation.AllowBlank = True
            objvalidation.Formula.ExcelFormula = "Action"
            objvalidation.ShowErrorMessage = True



            Dim intDataX As Integer
            Dim intDatay As Integer
            intDataX = 1
            intDatay = 2

            Dim lstDateFieldIndex As New List(Of Integer)

            'Dim strField As String = ""
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)


                Select Case item.FK_FieldType_ID
                    Case NawaBLL.Common.MFieldType.ReferenceTable
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Select a Value from drop-down List.", NawaBLL.Common.SessionCurrentUser.UserID)

                        'tambahin validasi kalau picklist
                        Dim objvalidationdata As OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList = objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).DataValidation.AddListDataValidation
                        objvalidationdata.AllowBlank = True
                        objvalidationdata.Formula.ExcelFormula = item.FieldName
                        objvalidationdata.ShowErrorMessage = True

                    Case NawaBLL.Common.MFieldType.IDENTITY


                        If item.IsPrimaryKey Then

                            objWorksheetData.Cells(intDataX, intDatay).AddComment("Mandatory for edit and Delete operation.", NawaBLL.Common.SessionCurrentUser.UserID)
                        Else
                            objWorksheetData.Cells(intDataX, intDatay).AddComment("ID is AutoNumber.", NawaBLL.Common.SessionCurrentUser.UserID)
                        End If


                    Case NawaBLL.Common.MFieldType.BIGINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Bigint ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.BooleanValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.DATETIMEValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Date with format(" & NawaBLL.SystemParameterBLL.GetDateFormat & ") ", NawaBLL.Common.SessionCurrentUser.UserID)
                        objWorksheetData.Cells(2, intDatay, intmaxrow, intDatay).Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                        lstDateFieldIndex.Add(intDatay)
                    Case NawaBLL.Common.MFieldType.FLOATValue, NawaBLL.Common.MFieldType.MONEYValue, NawaBLL.Common.MFieldType.NUMERICDECIMALValue, NawaBLL.Common.MFieldType.REALValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Float", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.INTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Int(-2.147.483,648 to 2.147.483.648)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.SMALLINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Smallint(-32.768 to 32.768) ", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.TINYINTValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Tinyint(0 to 255)", NawaBLL.Common.SessionCurrentUser.UserID)
                    Case NawaBLL.Common.MFieldType.VARCHARValue
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Varchar (Max length :" & item.SizeField & ")", NawaBLL.Common.SessionCurrentUser.UserID)







                End Select




                intDatay += 1
            Next



            If objSchemaModule.IsSupportActivation Then


                For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                    If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                        objWorksheetData.Cells(intDataX, intDatay).Value = item.FieldLabel
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
                        objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                        objWorksheetData.Cells(intDataX, intDatay).AddComment("Data Type: Boolean ", NawaBLL.Common.SessionCurrentUser.UserID)
                    End If
                Next
                intDatay += 1
            End If



            'tambah setting validation

            objWorksheetData.Cells(intDataX, intDatay).Value = "Validation Error"
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Bold = True
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            objWorksheetData.Cells(intDataX, intDatay).Style.Font.Color.SetColor(System.Drawing.Color.White)
            objWorksheetData.Cells(intDataX, intDatay).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
            objWorksheetData.Cells(intDataX, intDatay).AddComment("Validation Result : ", NawaBLL.Common.SessionCurrentUser.UserID)


            'If strField.Length > 0 Then
            '    strField = strField.Remove(strField.Length - 1, 1)
            'End If
            'Dim strsql As String = "select " & strField & " from " & objSchemaModule.ModuleName





            objWorksheetData.Cells("B2").LoadFromDataTable(objtb, False)
            If objtb.Rows.Count > 0 Then
                objWorksheetData.Cells("A2:A" & objtb.Rows.Count + 1).Value = "Update"
            End If

            'Cek Data yg Invalid, utk yg invalid tambah comment validation message & beri warna cell
            Dim LstFieldName As New List(Of String)
            For y As Integer = 0 To objtb.Columns.Count - 1
                LstFieldName.Add(objtb.Columns(y).ColumnName)
            Next

            Dim namedStyle = objWorksheetData.Workbook.Styles.CreateNamedStyle("ErrorField")
            namedStyle.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            namedStyle.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)

            Dim namedStyleDate = objWorksheetData.Workbook.Styles.CreateNamedStyle("ErrorFieldDate")
            namedStyleDate.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            namedStyleDate.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
            namedStyleDate.Style.Numberformat.Format = NawaBLL.SystemParameterBLL.GetDateFormat

            'Indra, 26 Mei 2019:
            'Ganti dari looping per baris ke looping per kolom
            Dim LstRowID As New List(Of String)
            For x As Integer = 0 To objtb.Rows.Count - 1
                LstRowID.Add(objtb.Rows(x).Item(strKeyFieldValue).ToString)
            Next


            Using ObjDb As New NawaDatadevEntities

                Dim objlisterror As Data.DataTable = GetListInvalid(strFilteradditional)
                Dim ObjvalidationReportList As List(Of Vw_validationReport_List) = NawaDevBLL.NawaFramework.ConvertDataTable(Of Vw_validationReport_List)(objlisterror)
                'Dim ObjvalidationReportList As List(Of Vw_validationReport_List) = ObjDb.Vw_validationReport_List.Where(Function(x) x.ModuleID = lngFk_Module_Id And x.FieldName = StrFieldName).OrderBy(Function(x) x.KeyFieldValue).ToList


                For intColumn As Integer = 0 To objtb.Columns.Count - 1
                    Dim LstErrorKeyFieldValue As New List(Of String)
                    Dim LstErrorMessage As New List(Of String)
                    Dim StrFieldName As String = LstFieldName(intColumn)

                    Dim ObjvalidationReportListfiltered As List(Of Vw_validationReport_List) = ObjvalidationReportList.FindAll(Function(x) x.FieldName = StrFieldName).OrderBy(Function(x) x.KeyFieldValue).ToList

                    'Dim ObjvalidationReportList As List(Of Vw_validationReport_List) = ObjDb.Vw_validationReport_List.Where(Function(x) x.ModuleID = lngFk_Module_Id And x.FieldName = StrFieldName).OrderBy(Function(x) x.KeyFieldValue).ToList
                    For Each objValidationReport As Vw_validationReport_List In ObjvalidationReportListfiltered
                        If LstErrorKeyFieldValue.Contains(objValidationReport.KeyFieldValue) Then
                            LstErrorMessage(LstErrorKeyFieldValue.IndexOf(objValidationReport.KeyFieldValue)) = LstErrorMessage(LstErrorKeyFieldValue.IndexOf(objValidationReport.KeyFieldValue)) &
                                vbCrLf & objValidationReport.ValidationMessage
                        Else
                            LstErrorKeyFieldValue.Add(objValidationReport.KeyFieldValue)
                            LstErrorMessage.Add(objValidationReport.ValidationMessage)
                        End If
                    Next

                    'Update ke Cell
                    Dim intRowIndex As Integer = 0
                    Dim intColIndex As Integer = intColumn + 2
                    For x As Integer = 0 To LstErrorKeyFieldValue.Count - 1
                        If LstRowID.Contains(LstErrorKeyFieldValue(x)) Then
                            intRowIndex = LstRowID.IndexOf(LstErrorKeyFieldValue(x)) + 2
                            objWorksheetData.Cells(intRowIndex, intColIndex).AddComment(LstErrorMessage(x), NawaBLL.Common.SessionCurrentUser.UserID)

                            If lstDateFieldIndex.Contains(intColIndex) Then
                                objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorFieldDate"
                            Else
                                objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorField"
                            End If

                        End If
                    Next

                Next

            End Using

            'Using ObjDb As New NawaDatadevEntities
            '    For intRow As Integer = 0 To objtb.Rows.Count - 1
            '        Dim LstErrorField As New List(Of String)
            '        Dim LstErrorMessage As New List(Of String)

            '        Dim lngRowId As Long = objtb.Rows(intRow).Item(strKeyFieldValue).ToString

            '        Dim ObjvalidationReportList As List(Of Vw_validationReport_List) = ObjDb.Vw_validationReport_List.Where(Function(x) x.ModuleID = lngFk_Module_Id And x.KeyFieldValue = lngRowId).ToList
            '        For Each objValidationReport As Vw_validationReport_List In ObjvalidationReportList
            '            If LstErrorField.Contains(objValidationReport.FieldName) Then
            '                LstErrorMessage(LstErrorField.IndexOf(objValidationReport.FieldName)) = LstErrorMessage(LstErrorField.IndexOf(objValidationReport.FieldName)) &
            '                    vbCrLf & objValidationReport.ValidationMessage
            '            Else
            '                LstErrorField.Add(objValidationReport.FieldName)
            '                LstErrorMessage.Add(objValidationReport.ValidationMessage)
            '            End If
            '        Next

            '        'Update ke Cell
            '        Dim intRowIndex As Integer = intRow + 2
            '        Dim intColIndex As Integer = 0
            '        For x As Integer = 0 To LstErrorField.Count - 1
            '            If LstFieldName.Contains(LstErrorField(x)) Then
            '                intColIndex = LstFieldName.IndexOf(LstErrorField(x)) + 2
            '                objWorksheetData.Cells(intRowIndex, intColIndex).AddComment(LstErrorMessage(x), NawaBLL.Common.SessionCurrentUser.UserID)

            '                If lstDateFieldIndex.Contains(intColIndex) Then
            '                    objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorFieldDate"
            '                Else
            '                    objWorksheetData.Cells(intRowIndex, intColIndex).StyleName = "ErrorField"
            '                End If
            '            End If
            '        Next

            '    Next
            'End Using


            'Dim dateformat As String = NawaBLL.SystemParameterBLL.GetDateFormat
            'Dim intcolnumber As Integer = 0
            'For Each item As System.Data.DataColumn In objtb.Columns
            '    If item.DataType = GetType(Date) Then
            '        intcolnumber = intcolnumber + 1
            '        objWorksheetData.Column(intcolnumber).Style.Numberformat.Format = dateformat
            '    End If
            'Next
            objWorksheetData.Cells.AutoFitColumns()
            'objWorksheetData.Column(intDatay).Style.WrapText = True

            intDatay = 2
            For Each item As NawaDAL.ModuleField In objSchemaModuleField
                If item.IsShowInForm Then
                    objWorksheetData.Column(intDatay).Hidden = False
                Else
                    objWorksheetData.Column(intDatay).Hidden = True
                End If
                intDatay += 1
            Next

            objpackage.SaveAs(objfs)
            Return strFileName

        End Using
    End Function


    Shared Function GetListInvalid(filteradditional As String) As DataTable

        Dim objsqlparameter(0) As SqlClient.SqlParameter
        objsqlparameter(0) = New SqlClient.SqlParameter

        objsqlparameter(0).ParameterName = "@filteradditional"
        objsqlparameter(0).Value = filteradditional

        Return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_getListInvalidOneReporting", objsqlparameter)


    End Function

    Shared Function GetPrimaryKeyField(objSchemaModuleField) As String
        Dim strprimarykeyfield As String = ""
        For Each item As NawaDAL.ModuleField In objSchemaModuleField
            If item.IsPrimaryKey Then
                strprimarykeyfield = item.FieldName
            End If
        Next
        Return strprimarykeyfield

    End Function





    Function GenerateCountSqlImportDataWithErrorMessageALL(strfilteradditional As String) As String


        Dim lngFk_Module_Id As Long = 0
        Dim strKeyFieldValue As String = ""


        Using objdb As New NawaDAL.NawaDataEntities
            objSchemaModule = objdb.Modules.Where(Function(x) x.ModuleName = Me.ModuleName).FirstOrDefault
            lngFk_Module_Id = objSchemaModule.PK_Module_ID
            Using ObjDb2 As New NawaDatadevEntities
                Dim ObjLHBU_FormInfo As ORS_FormInfo = ObjDb2.ORS_FormInfo.Where(Function(x) x.FK_Module_ID = lngFk_Module_Id).FirstOrDefault
                If Not ObjLHBU_FormInfo Is Nothing Then
                    strKeyFieldValue = ObjLHBU_FormInfo.KeyField
                End If

            End Using

            If objSchemaModule.IsUseDesigner Then
                If Not objSchemaModule Is Nothing Then
                    objSchemaModuleField = objdb.ModuleFields.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID).OrderBy(Function(y) y.Sequence).ToList
                End If
                objSchemaModuleFieldDefault = objdb.ModuleFieldDefaults.OrderBy(Function(y) y.Sequence).ToList
                objshemaModuleValidation = objdb.ModuleValidations.Where(Function(x) x.FK_Module_ID = objSchemaModule.PK_Module_ID And x.FK_ModuleAction_ID = Common.ModuleActionEnum.Update).ToList
            Else

            End If

        End Using



        Dim strtable As String = ""
        Dim strfield As String = ""
        Dim strfilter As String = ""
        Dim pk As String = GetPrimaryKeyField(objSchemaModuleField)
        strtable = " from " & Me.ModuleName & " "
        For Each item As NawaDAL.ModuleField In objSchemaModuleField
            Dim stralias As String = item.TabelReferenceNameAlias
            If stralias = "" Then
                stralias = item.TabelReferenceName
            End If

            If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                strtable += " left join " & item.TabelReferenceName & " " & stralias & " on " & Me.ModuleName & "." & item.FieldName & "=" & stralias & "." & item.TableReferenceFieldKey & " " & item.TableReferenceAdditonalJoin
                If item.TableReferenceFilter <> "" Then
                    strtable += " AND " & item.TableReferenceFilter
                End If

                strfield += " '('+ CONVERT(VARCHAR, " & Me.ModuleName & "." & item.FieldName & ") +') '+ " & stralias & "." & item.TableReferenceFieldDisplayName & " AS " & item.FieldName & ","
            Else
                strfield += Me.ModuleName & "." & item.FieldName & ","
            End If
        Next


        If objSchemaModule.IsSupportActivation Then
            For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                    strfield += Me.ModuleName & "." & item.FieldName & ","
                End If
            Next
        End If


        If TableValidationReport <> "" Then
            strtable &= " INNER JOIN [" & TableValidationReport & "] a  ON " & ModuleName & "." & pk & " = a.[RecordID] and a.SegmentData = '" & objSchemaModule.ModuleLabel & "'"
            strtable &= " INNER JOIN Vw_SettingPersonal b ON b.ReportDate = a.TanggalData AND b.KdCabang = a.KodeCabang "
        End If

        If strfield.Length > 0 Then
            strfield += " replace( a.[MessageDetailView] ,'</br>',CHAR(13)+CHAR(10)) AS ErrorMessage"
            strfield = strfield.Remove(strfield.Length - 1, 1)
        End If


        If strfilteradditional.Length > 0 Then
            strfilter = " where " & strfilteradditional
        Else
            strfilter = ""
        End If





        strtable = " select count(1) " & strtable & strfilter

        Return strtable
    End Function
    Function GenerateSqlImportDataWithErrorMessageALL(strfilteradditional As String) As String

        Dim strtable As String = ""
        Dim strfield As String = ""
        Dim strfilter As String = ""
        Dim pk As String = GetPrimaryKeyField(objSchemaModuleField)
        strtable = " from " & Me.ModuleName & " "
        For Each item As NawaDAL.ModuleField In objSchemaModuleField
            Dim stralias As String = item.TabelReferenceNameAlias
            If stralias = "" Then
                stralias = item.TabelReferenceName
            End If

            'If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then

            '    strtable += " left join " & item.TabelReferenceName & " " & stralias & " on " & Me.ModuleName & "." & item.FieldName & "=" & stralias & "." & item.TableReferenceFieldKey & " " & item.TableReferenceAdditonalJoin
            '    If item.TableReferenceFilter <> "" Then
            '        strtable += " AND " & item.TableReferenceFilter
            '    End If

            '    strfield += " '('+ CONVERT(VARCHAR, " & Me.ModuleName & "." & item.FieldName & ") +') '+ " & stralias & "." & item.TableReferenceFieldDisplayName & " AS " & item.FieldName & ","
            'Else
            '    strfield += Me.ModuleName & "." & item.FieldName & ","
            'End If


            If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable And item.FK_ExtType_ID <> NawaBLL.Common.MExtType.PopUpMultiCheckBox Then

                Dim strjoin As String = ""
                If Not item.TableReferenceAdditonalJoin Is Nothing Then
                    strjoin = item.TableReferenceAdditonalJoin.Replace("@userid", NawaBLL.Common.SessionCurrentUser.UserID)
                    strjoin = strjoin.Replace("@PK_MUser_ID", NawaBLL.Common.SessionCurrentUser.PK_MUser_ID)

                End If

                strtable += " left join " & item.TabelReferenceName & " " & stralias & " on " & Me.ModuleName & "." & item.FieldName & "=" & stralias & "." & item.TableReferenceFieldKey & " " & strjoin

                strfield += " '('+ CONVERT(VARCHAR, " & Me.ModuleName & "." & item.FieldName & ") +') '+ " & stralias & "." & item.TableReferenceFieldDisplayName & " AS " & item.FieldName & ","
            Else
                strfield += Me.ModuleName & "." & item.FieldName & ","
            End If

        Next


        If objSchemaModule.IsSupportActivation Then
            For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                    strfield += Me.ModuleName & "." & item.FieldName & ","
                End If
            Next
        End If


        If TableValidationReport <> "" Then
            strtable &= " INNER JOIN [" & TableValidationReport & "] a  ON " & ModuleName & "." & pk & " = a.[RecordID] and a.SegmentData = '" & objSchemaModule.ModuleLabel & "'"
            '
            'strtable &= " INNER JOIN Vw_SettingPersonal b ON b.ReportDate = a.TanggalData AND b.KdCabang = a.KodeCabang "
        End If

        If strfield.Length > 0 Then
            strfield += " replace( a.[MessageDetailView] ,'</br>',CHAR(13)+CHAR(10)) AS ErrorMessage"
            strfield = strfield.Remove(strfield.Length - 1, 1)
        End If


        If strfilteradditional.Length > 0 Then
            strfilter = " where " & strfilteradditional
        Else
            strfilter = ""
        End If





        strtable = " select " & strfield & strtable & strfilter

        Return strtable
    End Function


    Function GenerateCountSqlImportDataWithErrorMessage(strfilteradditional As String) As String

        Dim strtable As String = ""
        Dim strfield As String = ""
        Dim strfilter As String = ""
        Dim pk As String = GetPrimaryKeyField(objSchemaModuleField)
        strtable = " from " & Me.ModuleName & " "
        For Each item As NawaDAL.ModuleField In objSchemaModuleField
            Dim stralias As String = item.TabelReferenceNameAlias
            If stralias = "" Then
                stralias = item.TabelReferenceName
            End If

            If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                strtable += " left join " & item.TabelReferenceName & " " & stralias & " on " & Me.ModuleName & "." & item.FieldName & "=" & stralias & "." & item.TableReferenceFieldKey & " " & item.TableReferenceAdditonalJoin
                If item.TableReferenceFilter <> "" Then
                    strtable += " AND " & item.TableReferenceFilter
                End If

                strfield += " '('+ CONVERT(VARCHAR, " & Me.ModuleName & "." & item.FieldName & ") +') '+ " & stralias & "." & item.TableReferenceFieldDisplayName & " AS " & item.FieldName & ","
            Else
                strfield += Me.ModuleName & "." & item.FieldName & ","
            End If
        Next


        If objSchemaModule.IsSupportActivation Then
            For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                    strfield += Me.ModuleName & "." & item.FieldName & ","
                End If
            Next
        End If


        If TableValidationReport <> "" Then
            strtable &= " INNER JOIN [" & TableValidationReport & "]  ON " & ModuleName & "." & pk & " = [" & TableValidationReport & "].[RecordID] and [" & TableValidationReport & "].SegmentData = '" & objSchemaModule.ModuleLabel & "'"
        End If

        If strfield.Length > 0 Then
            strfield += " replace( [" & TableValidationReport & "].[MessageDetailView] ,'</br>',CHAR(13)+CHAR(10)) AS ErrorMessage"
            strfield = strfield.Remove(strfield.Length - 1, 1)
        End If


        If strfilteradditional.Length > 0 Then
            strfilter = " where " & strfilteradditional
        Else
            strfilter = ""
        End If



        'If Filter.Count > 0 Then
        '    strfilter += " where " & Me.ModuleName & "." & pk & " In ("
        'End If
        'For Each item As String In Filter()
        '    strfilter += "'" & item & "',"
        'Next

        'If Filter.Count > 0 Then
        '    strfilter = strfilter.Remove(strfilter.Length - 1, 1)
        '    strfilter += ")"
        'End If


        strtable = " select Count(1) " & strtable & strfilter

        Return strtable
    End Function
    Function GenerateSqlImportDataWithErrorMessage(strfilteradditional As String) As String

        Dim strtable As String = ""
        Dim strfield As String = ""
        Dim strfilter As String = ""
        Dim pk As String = GetPrimaryKeyField(objSchemaModuleField)
        strtable = " from " & Me.ModuleName & " "
        For Each item As NawaDAL.ModuleField In objSchemaModuleField
            Dim stralias As String = item.TabelReferenceNameAlias
            If stralias = "" Then
                stralias = item.TabelReferenceName
            End If

            If item.FK_FieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                strtable += " left join " & item.TabelReferenceName & " " & stralias & " on " & Me.ModuleName & "." & item.FieldName & "=" & stralias & "." & item.TableReferenceFieldKey & " " & item.TableReferenceAdditonalJoin
                If item.TableReferenceFilter <> "" Then
                    strtable += " AND " & item.TableReferenceFilter
                End If

                strfield += " '('+ CONVERT(VARCHAR, " & Me.ModuleName & "." & item.FieldName & ") +') '+ " & stralias & "." & item.TableReferenceFieldDisplayName & " AS " & item.FieldName & ","
            Else
                strfield += Me.ModuleName & "." & item.FieldName & ","
            End If
        Next


        If objSchemaModule.IsSupportActivation Then
            For Each item As NawaDAL.ModuleFieldDefault In objSchemaModuleFieldDefault.FindAll(Function(x) x.PK_ModuleField_ID = 1)
                If item.FK_FieldType_ID = Common.MFieldType.BooleanValue Then
                    strfield += Me.ModuleName & "." & item.FieldName & ","
                End If
            Next
        End If


        If TableValidationReport <> "" Then
            strtable &= " INNER JOIN [" & TableValidationReport & "]  ON " & ModuleName & "." & pk & " = [" & TableValidationReport & "].[RecordID] and [" & TableValidationReport & "].SegmentData = '" & objSchemaModule.ModuleLabel & "'"
        End If

        If strfield.Length > 0 Then
            strfield += " replace( [" & TableValidationReport & "].[MessageDetailView] ,'</br>',CHAR(13)+CHAR(10)) AS ErrorMessage"
            strfield = strfield.Remove(strfield.Length - 1, 1)
        End If


        If strfilteradditional.Length > 0 Then
            strfilter = " where " & strfilteradditional
        Else
            strfilter = ""
        End If



        'If Filter.Count > 0 Then
        '    strfilter += " where " & Me.ModuleName & "." & pk & " In ("
        'End If
        'For Each item As String In Filter()
        '    strfilter += "'" & item & "',"
        'Next

        'If Filter.Count > 0 Then
        '    strfilter = strfilter.Remove(strfilter.Length - 1, 1)
        '    strfilter += ")"
        'End If


        strtable = " select " & strfield & strtable & strfilter

        Return strtable
    End Function
    Shared Function GetDataByID(id As Long) As NawaDevDAL.Web_ValidationReport_RowComplate
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.Web_ValidationReport_RowComplate.Where(Function(x) x.PK_ID = id).FirstOrDefault
        End Using
    End Function


    'Shared Function GetD01(id As String) As NawaDevDAL.D01_DebiturIndividu
    '    Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
    '        Return (From x In objDb.D01_DebiturIndividu Where x.CIF = id Select x).FirstOrDefault
    '    End Using
    'End Function

    'Shared Function GetD02(id As String) As NawaDevDAL.D02_DebiturBadanUsaha
    '    Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
    '        Return (From x In objDb.D02_DebiturBadanUsaha Where x.CIF = id Select x).FirstOrDefault
    '    End Using
    'End Function
End Class
