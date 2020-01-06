Imports CookComputing.XmlRpc
Imports System.Threading


Public Class ServicesForWeb
    Inherits ListenerService
    Private mylog As New NawaConsoleLog

    Sub run()
        'Using objDb As NawaDataEntities = New NawaDataEntities
        '    objDb.Database.ExecuteSqlCommand("exec usp_GenerateFiles")
        'End Using
        'GenerateTextFileFromWeb()
    End Sub

    Public Shared Function GenerateTextFileFromWeb()
        GenerateTextFileFromWeb(Nothing, Nothing, Nothing, Nothing)
    End Function

    <XmlRpcMethod("GenerateTextFileFromWeb")>
    Public Shared Function GenerateTextFileFromWeb(ReportDate As Date, KodeCabang As String, UserName As String, ArrTemplate() As String) As Boolean
        'Ini utk yg bulannya dr 1 - 9, karena kalo gak, pas generatenya difilter 2 digit
        'Bulan = CInt(Bulan)

        'Insert D01
        ''Console.WriteLine("Enter Generate. Param:" & ReportDate & "," & KodeCabang)

        Dim mylog As New NawaConsoleLog
        Dim ListTemplate As List(Of String) = ArrTemplate.ToList
        Using objDb As New NawaDataEntities
            Using objTrans As Entity.DbContextTransaction = objDb.Database.BeginTransaction()
                Try
                    Dim templateStr As String = ""
                    Dim reqId As Integer = objDb.Vw_GeneratedFileList.ToList.Select(Function(x) x.Request_ID).DefaultIfEmpty(0).Max() + 1

                    For Each itemTemplate In ListTemplate
                        Dim objTemplate As GenerateFileTemplate = objDb.GenerateFileTemplates.ToList.FirstOrDefault(Function(x) x.PK_GenerationFileTemplate_ID = Convert.ToInt32(itemTemplate))

                        If KodeCabang.ToLower = "all" Then
                            'Jika kode cabang = All, maka generate text file per jumlah cabang
                            Dim paramQueryCabang(1) As SqlClient.SqlParameter
                            paramQueryCabang(0) = New SqlClient.SqlParameter
                            paramQueryCabang(0).ParameterName = "@KodeForm"
                            paramQueryCabang(0).SqlDbType = SqlDbType.VarChar
                            If objTemplate Is Nothing Then
                                paramQueryCabang(0).Value = ""
                            Else
                                paramQueryCabang(0).Value = objTemplate.LHBUFormName
                            End If

                            paramQueryCabang(1) = New SqlClient.SqlParameter
                            paramQueryCabang(1).ParameterName = "@TanggalData"
                            paramQueryCabang(1).SqlDbType = SqlDbType.VarChar
                            paramQueryCabang(1).Value = ReportDate.ToString("yyyyMMdd")

                            Dim cabangRecords As DataTable = SQLHelper.ExecuteTable(objDb.Database.Connection, objTrans, CommandType.StoredProcedure, "usp_GetDistinctCabangForm", paramQueryCabang)
                            For Each itemCabang As DataRow In cabangRecords.Rows
                                DeletePrevious(objDb, ReportDate, itemTemplate, itemCabang("Kode_Cabang_BI"))
                                InsertBase(objDb, ReportDate, itemTemplate, itemCabang("Kode_Cabang_BI"), UserName, reqId)
                            Next
                        Else
                            DeletePrevious(objDb, ReportDate, itemTemplate, KodeCabang)
                            InsertBase(objDb, ReportDate, itemTemplate, KodeCabang, UserName, reqId)
                        End If
                        'InsertRecordCount(ReportDate, KodeCabang, FormCode(i))

                        If templateStr <> "" Then
                            templateStr = templateStr + ","
                        End If
                        templateStr = templateStr + itemTemplate
                    Next


                    Dim queryString As String = "SELECT PK_GeneratedFileList_ID, PeriodeLaporan, FK_LHBU_FormInfo_ID, KodeCabang " & vbCrLf &
                                "FROM Vw_GeneratedFileList " & vbCrLf &
                                "WHERE PeriodeLaporan = '" & ReportDate.ToString("yyyyMMdd") & "'" & vbCrLf &
                                "AND FK_Template_ID IN (" & templateStr & ")" & vbCrLf &
                                "AND ISNULL(SubmitStatus, '') <> 'Submitted'" & vbCrLf

                    If KodeCabang.ToLower <> "all" Then
                        queryString += " AND KodeCabang = '" & KodeCabang & "'"
                    End If

                    Dim ProcessedRecords As DataTable = SQLHelper.ExecuteTable(objDb.Database.Connection, objTrans, CommandType.Text, queryString, Nothing)
                    For Each items As DataRow In ProcessedRecords.Rows

                        Dim SQL As String = ""

                        'Update Status
                        SQL = "UPDATE GeneratedFileList SET GenerationStatus = 'PROCESSING', UpdatedBy='System',UpdatedDate=GETDATE()"
                        SQL += "WHERE PK_GeneratedFileList_ID=" & items("PK_GeneratedFileList_ID")
                        SQLHelper.ExecuteScalar(objDb.Database.Connection, objTrans, CommandType.Text, SQL, Nothing)

                        Dim paramFileName As New Data.SqlClient.SqlParameter()
                        Dim paramTanggal As New Data.SqlClient.SqlParameter()
                        Dim paramForm As New Data.SqlClient.SqlParameter()
                        Dim paramKodeCabang As New Data.SqlClient.SqlParameter()

                        'Update ID Operasional
                        paramFileName = New Data.SqlClient.SqlParameter("@recordID", items("PK_GeneratedFileList_ID"))
                        paramTanggal = New Data.SqlClient.SqlParameter("@tanggal", items("PeriodeLaporan"))
                        paramKodeCabang = New Data.SqlClient.SqlParameter("@kodecabang", items("KodeCabang"))
                        objDb.Database.ExecuteSqlCommand("exec usp_UpdateForm_IDOperasional @recordID, @tanggal, @kodecabang", paramFileName, paramTanggal, paramKodeCabang)

                        'Isi data temporary
                        paramFileName = New Data.SqlClient.SqlParameter("@recordID", items("PK_GeneratedFileList_ID"))
                        paramTanggal = New Data.SqlClient.SqlParameter("@tanggal", items("PeriodeLaporan"))
                        paramForm = New Data.SqlClient.SqlParameter("@kodeform", items("FK_LHBU_FormInfo_ID"))
                        paramKodeCabang = New Data.SqlClient.SqlParameter("@kodecabang", items("KodeCabang"))
                        objDb.Database.ExecuteSqlCommand("exec usp_GenerateFileContent @recordID,@tanggal,@kodeform,@kodecabang", paramFileName, paramTanggal, paramForm, paramKodeCabang)

                        'Update ke table actual
                        paramFileName = New Data.SqlClient.SqlParameter("@recordID", items("PK_GeneratedFileList_ID"))
                        paramTanggal = New Data.SqlClient.SqlParameter("@tanggal", items("PeriodeLaporan"))
                        paramForm = New Data.SqlClient.SqlParameter("@kodeform", items("FK_LHBU_FormInfo_ID"))
                        paramKodeCabang = New Data.SqlClient.SqlParameter("@kodecabang", items("KodeCabang"))
                        objDb.Database.ExecuteSqlCommand("exec usp_FillFileContent @recordID,@tanggal,@kodeform,@kodecabang", paramFileName, paramTanggal, paramForm, paramKodeCabang)

                    Next

                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    mylog.LogError("An Error has been occurred on GenerateTextFileFromWeb: ", ex)
                    Throw
                End Try
            End Using
        End Using
    End Function

    Protected Shared Sub DeletePrevious(objdb As NawaDataEntities, ReportDate As Date, TemplateID As String, KodeCabang As String)
        'Hapus dulu data yang sudah ada supaya tidak duplikat
        'Console.WriteLine("Enter delete. Param:" & ReportDate & "," & FormCode & "," & KodeCabang)

        Dim paramTanggal As New Data.SqlClient.SqlParameter("@tanggal", ReportDate)
        Dim paramTemplate As New Data.SqlClient.SqlParameter("@templateID", TemplateID)
        Dim paramCabang As New Data.SqlClient.SqlParameter("@kodecabang", KodeCabang)
        objdb.Database.ExecuteSqlCommand("exec usp_CleanFiles @tanggal,@templateID,@kodecabang", paramTanggal, paramTemplate, paramCabang)

    End Sub

    Protected Shared Sub InsertBase(objdb As NawaDataEntities, ReportDate As Date, TemplateID As String, KodeCabang As String, UserName As String, ReqID As Integer)
        'Insert dulu default value ke table untuk menunjukkan bahwa dokumen sedang digenerate
        ''Console.WriteLine("Enter insert. Param:" & Bulan & "," & Tahun & "," & ModuleName & "," & KodeCabang)
        '  Using objDb As NawaDataEntities = New NawaDataEntities
        Dim paramTanggal As New Data.SqlClient.SqlParameter("@tanggal", ReportDate)
        Dim paramTemplate As New Data.SqlClient.SqlParameter("@templateID", TemplateID)
        Dim paramCabang As New Data.SqlClient.SqlParameter("@kodecabang", KodeCabang)
        Dim paramUser As New Data.SqlClient.SqlParameter("@username", UserName)
        Dim paramReq As New Data.SqlClient.SqlParameter("@reqID", ReqID)

        objdb.Database.ExecuteSqlCommand("exec usp_GenerateFiles @tanggal,@templateID,@kodecabang,@username,@reqID", paramTanggal, paramTemplate, paramCabang, paramUser, paramReq)
        ' End Using
    End Sub

    Protected Shared Sub InsertRecordCount(ReportDate As Date, Bulan As String, Tahun As String, ModuleName As String, KodeCabang As String, GenerateType As String)
        'Insert dulu default value ke table untuk menunjukkan bahwa dokumen sedang digenerate
        Console.WriteLine("Enter insert count. Param:" & Bulan & "," & Tahun & "," & ModuleName & "," & KodeCabang)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim paramBulan As New Data.SqlClient.SqlParameter("@bulan", Bulan)
            Dim paramTahun As New Data.SqlClient.SqlParameter("@tahun", Tahun)
            Dim paramModule As New Data.SqlClient.SqlParameter("@modulename", ModuleName)
            Dim paramCabang As New Data.SqlClient.SqlParameter("@kodecabang", KodeCabang)
            Dim paramGenerateType As New Data.SqlClient.SqlParameter("@GenerateType", GenerateType)

            objDb.Database.ExecuteSqlCommand("exec usp_InsertCounts @bulan,@tahun,@modulename,@kodecabang,@GenerateType", paramBulan, paramTahun, paramModule, paramCabang, paramGenerateType)
        End Using
    End Sub

    <XmlRpcMethod("ValidateRecordsFromWeb")>
    Public Shared Function ValidateRecordsFromWeb(Bulan As String, ModuleName() As String, Tahun As String, kodecabang As String) As Boolean
        For i As Integer = 0 To ModuleName.Length - 1
            Console.WriteLine("Start. Param:" & Bulan & "," & Tahun & "," & ModuleName(i).ToString)
            'Dim paramBulan As New Data.SqlClient.SqlParameter("@bulan", Bulan)
            'Dim paramTahun As New Data.SqlClient.SqlParameter("@tahun", Tahun)
            'Dim paramkodecabang As New Data.SqlClient.SqlParameter("@kodekantorcabang", kodecabang)
            'Dim parammodule As New Data.SqlClient.SqlParameter("@ModuleTable", ModuleName(i))

            'Using objDb As NawaDataEntities = New NawaDataEntities
            '    objDb.Database.ExecuteSqlCommand("exec usp_ExecuteValidationBySegmentData @bulan,@tahun,@kodekantorcabang,@ModuleTable", paramBulan, paramTahun, paramkodecabang, parammodule)
            'End Using
        Next

        Dim objtest As New ServicesForWeb
        objtest.ThreadStartValidateRecords(Bulan, ModuleName, Tahun, kodecabang)
    End Function

    Private ObjValidate As ValidateFromWebBLL
    Private trdValidateRecord As Thread
    Private Sub ThreadStartValidateRecords(Bulan As String, ModuleName() As String, Tahun As String, kodecabang As String)
        ObjValidate = New ValidateFromWebBLL
        ObjValidate.Bulan = Bulan
        ObjValidate.ModuleName = ModuleName
        ObjValidate.Tahun = Tahun
        ObjValidate.kodecabang = kodecabang

        trdValidateRecord = New Thread(AddressOf ObjValidate.RunValidate)
        trdValidateRecord.IsBackground = True
        trdValidateRecord.Start()
    End Sub

    <XmlRpcMethod("CleanRecords")>
    Public Shared Function CleanRecords(RecordID() As String,
                          UserName As String) As Boolean
        Console.WriteLine("Clean records")
        Console.WriteLine("Record count:" & RecordID.Length)

        Dim paramUserName As New Data.SqlClient.SqlParameter("@UserName", UserName)
        Dim Records As String = ""
        For i As Integer = 0 To RecordID.Length - 1
            Console.WriteLine("Record processed:" & RecordID(i))

            Records = Records + RecordID(i) + ","
        Next

        Dim paramRecordID As New Data.SqlClient.SqlParameter("@RecordID", Records)
        Using objDb As NawaDataEntities = New NawaDataEntities
            objDb.Database.ExecuteSqlCommand("exec usp_ExecuteCleansing @RecordID,@UserName", paramRecordID, paramUserName)
        End Using
    End Function

    <XmlRpcMethod("CleanRecordsAll")>
    Public Shared Function CleanRecordsAll(UserName As String) As Boolean
        Console.WriteLine("Clean records all")

        Dim paramUserName As New Data.SqlClient.SqlParameter("@UserName", UserName)
        Using objDb As NawaDataEntities = New NawaDataEntities
            'objDb.Database.ExecuteSqlCommand("exec usp_ExecuteCleansing @RecordID,@UserName", paramRecordID, paramUserName)
            objDb.Database.ExecuteSqlCommand("exec usp_ExecuteCleansing NULL,@UserName", paramUserName)
        End Using
    End Function

    <XmlRpcMethod("InsertAsDictionary")>
    Public Shared Function InsertAsDictionary(RecordID() As String) As Boolean
        Console.WriteLine("Insert to Dictionary")
        Console.WriteLine("Record count:" & RecordID.Length)

        If RecordID.Length < 1 Then
            Using objDb As NawaDataEntities = New NawaDataEntities
                objDb.Database.ExecuteSqlCommand("exec usp_InsertDataDictionary NULL")
            End Using
        Else
            For i As Integer = 0 To RecordID.Length - 1
                Console.WriteLine("Record processed:" & RecordID(i))
                Dim paramRecordID As New Data.SqlClient.SqlParameter("@RecordID", RecordID(i))
                Using objDb As NawaDataEntities = New NawaDataEntities
                    objDb.Database.ExecuteSqlCommand("exec usp_InsertDataDictionary @RecordID", paramRecordID)
                End Using
            Next
        End If
    End Function
    '<XmlRpcMethod("CallProcessRPCFromWeb")> _
    'Public Shared Function CallProcessRPCFromWeb(ByVal ValSchedulerId As String, ByVal ValProcessDate As String, ByVal ValProcessDownloadFromMISICBS As String, ByVal ValProcessDownloadFromMISNCBS As String, ByVal ValProcessGenerateCTRReport As String, ByVal ValProcessPurgingData As String, ByVal ValProcessNegativeScreeningDaily As String, ByVal ValProcessNegativeScreeningMonthly As String, ByVal ValProcessRemittanceNewsScreening As String, ByVal ValProcessDataQualityMonitoring As String, ByVal ValPocessDataNasabah As String, ByVal ValDownloadFileWorldcheckDeletedDaily As String, ByVal ValDownloadFileWorldcheckDeletedWeekly As String, ByVal ValDownloadFileWorldcheckDeletedMonthly As String, ByVal ValDownloadFileWorldcheckNewUpdateDaily As String, ByVal ValDownloadFileWorldcheckNewUpdateWeekly As String, ByVal ValDownloadFileWorldcheckNewUpdateMonthly As String, ByVal ValExtractSaveFileWorldcheckDeletedDaily As String, ByVal ValExtractSaveFileWorldcheckDeletedWeekly As String, ByVal ValExtractSaveFileWorldcheckDeletedMonthly As String, ByVal ValExtractSaveFileWorldcheckNewUpdateDaily As String, ByVal ValExtractSaveFileWorldcheckNewUpdateWeekly As String, ByVal ValExtractSaveFileWorldcheckNewUpdateMonthly As String, ByVal ValProcessIFTIReport As String) As Boolean
    '    Dim LongSchedulerId As Long = Convert.ToInt64(ValSchedulerId)
    '    Dim DateProcessDate As DateTime = Convert.ToDateTime(ValProcessDate)
    '    Dim BolProcessDownloadDataFromMISICBS As Boolean = Convert.ToBoolean(ValProcessDownloadFromMISICBS)
    '    Dim BolProcessDownloadDataFromMISNCBS As Boolean = Convert.ToBoolean(ValProcessDownloadFromMISNCBS)
    '    Dim bolProcessCTRReport As Boolean = Convert.ToBoolean(ValProcessGenerateCTRReport)
    '    Dim bolProcessPurgingData As Boolean = Convert.ToBoolean(ValProcessPurgingData)

    '    Dim BolProcessNegativeScreeningDaily As Boolean = Convert.ToBoolean(ValProcessNegativeScreeningDaily)
    '    Dim BolProcessNegativeScreeningMonthly As Boolean = Convert.ToBoolean(ValProcessNegativeScreeningMonthly)
    '    Dim bolProcessRemittanceNewsScreening As Boolean = Convert.ToBoolean(ValProcessRemittanceNewsScreening)
    '    Dim bolProcessDataQualityMonitoring As Boolean = Convert.ToBoolean(ValProcessDataQualityMonitoring)
    '    Dim bolProcessDataNasabah As Boolean = Convert.ToBoolean(ValPocessDataNasabah)


    '    Dim bolDownloadFileWorldcheckDeletedDaily As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckDeletedDaily)
    '    Dim bolDownloadFileWorldcheckDeletedWeekly As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckDeletedWeekly)
    '    Dim bolDownloadFileWorldcheckDeletedMonthly As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckDeletedMonthly)
    '    Dim bolDownloadFileWorldcheckNewUpdateDaily As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckNewUpdateDaily)
    '    Dim bolDownloadFileWorldcheckNewUpdateWeekly As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckNewUpdateWeekly)
    '    Dim bolDownloadFileWorldcheckNewUpdateMonthly As Boolean = Convert.ToBoolean(ValDownloadFileWorldcheckNewUpdateMonthly)
    '    Dim bolExtractSaveFileWorldcheckDeletedDaily As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckDeletedDaily)
    '    Dim bolExtractSaveFileWorldcheckDeletedWeekly As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckDeletedWeekly)
    '    Dim bolExtractSaveFileWorldcheckDeletedMonthly As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckDeletedMonthly)
    '    Dim bolExtractSaveFileWorldcheckNewUpdateDaily As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckNewUpdateDaily)
    '    Dim bolExtractSaveFileWorldcheckNewUpdateWeekly As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckNewUpdateWeekly)
    '    Dim bolExtractSaveFileWorldcheckNewUpdateMonthly As Boolean = Convert.ToBoolean(ValExtractSaveFileWorldcheckNewUpdateMonthly)
    '    Dim bolprocessIFTIReport As Boolean = Convert.ToBoolean(ValProcessIFTIReport)

    '    Try
    '        InsertProgressLogETL("Start Process CallProcessManualFromWeb", DateProcessDate)
    '        Using ObjCTRConsole As New CTRService
    '            ObjCTRConsole.CallProcessManualFromWeb(LongSchedulerId, DateProcessDate, BolProcessDownloadDataFromMISICBS, BolProcessDownloadDataFromMISNCBS, bolProcessCTRReport, bolProcessPurgingData, BolProcessNegativeScreeningDaily, BolProcessNegativeScreeningMonthly, bolProcessRemittanceNewsScreening, bolProcessDataQualityMonitoring, bolProcessDataNasabah, False, False, False, False, False, False, False, False, False, False, bolDownloadFileWorldcheckDeletedDaily, bolDownloadFileWorldcheckDeletedWeekly, bolDownloadFileWorldcheckDeletedMonthly, bolDownloadFileWorldcheckNewUpdateDaily, bolDownloadFileWorldcheckNewUpdateWeekly, bolDownloadFileWorldcheckNewUpdateMonthly, bolExtractSaveFileWorldcheckDeletedDaily, bolExtractSaveFileWorldcheckDeletedWeekly, bolExtractSaveFileWorldcheckDeletedMonthly, bolExtractSaveFileWorldcheckNewUpdateDaily, bolExtractSaveFileWorldcheckNewUpdateWeekly, bolExtractSaveFileWorldcheckNewUpdateMonthly, bolprocessIFTIReport)
    '        End Using
    '        InsertProgressLogETL("End Process CallProcessManualFromWeb", DateProcessDate)
    '        Return True
    '    Catch ex As Exception
    '        myLog.Error(ex.StackTrace)
    '        Return False
    '    End Try
    'End Function
End Class
