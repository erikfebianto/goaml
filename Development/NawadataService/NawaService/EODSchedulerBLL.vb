Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Threading
Imports Microsoft.SqlServer.Dts.Runtime

Public Class EODSchedulerBLL
    Implements IDisposable

    Enum EODTaskDetailType
        SSIS = 1
        StoreProcedure
        SSISinSQLagent
        API
    End Enum

    Enum MsEODStatus
        OnQueue = 1
        Inprogress
        Sucess
        ErrorProcess
        TryCanceling
        Canceled
    End Enum

    Private mylog As New NawaConsoleLog

    Sub New()

    End Sub

    Sub run()
        Try


            While True
                Thread.Sleep(My.Settings.IntThreadInterval)
                ExecSPCurrentScheduler()

                Using objDb As NawaDataEntities = New NawaDataEntities
                    Dim objListEODSchedulerLog As List(Of EODSchedulerLog) = objDb.EODSchedulerLogs.Where(Function(x) x.FK_MsEODStatus_ID = 1).Take(1).ToList

                    For Each item As EODSchedulerLog In objListEODSchedulerLog
                        Dim lngPK_EODSchedulerLog_ID As Long = item.PK_EODSchedulerLog_ID
                        Dim strkodecabang As String = item.KodeCabang

                        ExecUpdateParamModifiedDate(item.DataDate.GetValueOrDefault(DateTime.Now))
                        If Not strkodecabang Is Nothing Then
                            ExecUpdateKodeCabang(strkodecabang)
                        End If
                        RunEODScheduler(lngPK_EODSchedulerLog_ID)
                    Next

                End Using

            End While
        Catch ex As Exception
            mylog.LogError("An Error has been occurred on RunEODScheduler: ", ex)
            Throw ex
        End Try
    End Sub


    Sub ExecUpdateKodeCabang(ByVal strkodecabang As String)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objkodecabang As New Data.SqlClient.SqlParameter("@kodecabang", strkodecabang)

            objDb.Database.ExecuteSqlCommand("exec Usp_Console_UpdateParamBranchCode @kodecabang", objkodecabang)
        End Using
    End Sub
    Sub ExecUpdateParamModifiedDate(ByVal processdate As Date)
        Using objDb As NawaDataEntities = New NawaDataEntities

            ' Create a SqlParameter for each parameter in the stored procedure.

            Dim objparamdate As New Data.SqlClient.SqlParameter("@ProcessDate", processdate.ToString("yyyy-MM-dd"))

            objDb.Database.ExecuteSqlCommand("exec Usp_Console_UpdateParamModifiedDate @ProcessDate", objparamdate)
        End Using
    End Sub



    Public Sub ExecuteAPI(PathDLL As String, Classname As String, methodName As String, PK_EODTaskDetail_ID As Long)
        Dim assembly__1 As Assembly = Assembly.LoadFile(PathDLL)
        Dim magicType As Type = assembly__1.[GetType](Classname)


        Dim magicConstructor As ConstructorInfo = magicType.GetConstructor(Type.EmptyTypes)
        Dim magicClassObject As Object = magicConstructor.Invoke(New Object() {})

        ' Get the ItsMagic method and invoke with a parameter value of 100

        Dim magicMethod As MethodInfo = magicType.GetMethod(methodName)
        Dim objListSystemParameter As System.Reflection.ParameterInfo() = magicMethod.GetParameters()
        If objListSystemParameter.Length > 0 Then

            Dim objparam As Object() = New Object() {PK_EODTaskDetail_ID}
            Dim magicValue As Object = magicMethod.Invoke(magicClassObject, objparam)
            magicValue = Nothing
            objparam = Nothing
        Else
            Dim magicValue As Object = magicMethod.Invoke(magicClassObject, Nothing)
            magicValue = Nothing
        End If


        magicMethod = Nothing
        magicClassObject = Nothing
        magicType = Nothing
        assembly__1 = Nothing


    End Sub



    Sub RunEODScheduler(lngPK_EODSchedulerLog_ID As Long)
        Dim IsprocessValid As Boolean = True
        Dim strErrorMessage As String = ""
        Dim lngTaskDetailProcess As Long

        Using objDb As NawaDataEntities = New NawaDataEntities
            Try
                Dim objEodSchedulerLog As EODSchedulerLog = objDb.EODSchedulerLogs.Where(Function(x) x.PK_EODSchedulerLog_ID = lngPK_EODSchedulerLog_ID).FirstOrDefault()
                Dim EODSchedulerID As Long = 0
                If Not objEodSchedulerLog Is Nothing Then
                    EODSchedulerID = objEodSchedulerLog.FK_EODSchedulerID
                End If

                'udpate status jadi inprogress (2)
                EODSchedulerStart(lngPK_EODSchedulerLog_ID)

                For Each itemTaskLog As EODTaskLog In objDb.EODTaskLogs.Where(Function(x) x.EODSchedulerLogID = lngPK_EODSchedulerLog_ID).ToList
                    'update status task log  jadi inprogress (2)
                    EODTaskStart(itemTaskLog.PK_EODTaskLog_ID)

                    For Each itemTaskDetailLog As EODTaskDetailLog In objDb.EODTaskDetailLogs.Where(Function(x) x.EODTaskLogID = itemTaskLog.PK_EODTaskLog_ID).ToList
                        If IsprocessValid Then

                            Try

                                'update status jadi inprocess (2)
                                EODTaskDetailStart(itemTaskDetailLog.PK_EODTaskDetailLog_ID)

                                Dim objEOdTaskDetail As EODTaskDetail = objDb.EODTaskDetails.Where(Function(x) x.PK_EODTaskDetail_ID = itemTaskDetailLog.FK_EODTAskDetail_ID).FirstOrDefault()

                                If Not objEOdTaskDetail Is Nothing Then

                                    If objEOdTaskDetail.FK_EODTaskDetailType_ID = EODTaskDetailType.SSIS Then
                                        Dim datdate As Date = objEodSchedulerLog.DataDate
                                        objDb.Database.ExecuteSqlCommand("update SystemParameter set SettingValue='" & datdate.ToString("yyyyMMdd") & "' where PK_SystemParameter_ID=3003")


                                        objDb.Database.ExecuteSqlCommand("exec Usp_Console_UpdateParamEODTaskDetailLogID @PK_EODTaskDetailLog_ID",
                                            New Data.SqlClient.SqlParameter("@PK_EODTaskDetailLog_ID", itemTaskDetailLog.PK_EODTaskDetailLog_ID))

                                        If RunEodTaskDetailSSIS(objEOdTaskDetail, itemTaskDetailLog) Then
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                                        Else
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.ErrorProcess), "Error Running SSIS " & objEOdTaskDetail.SSISName)

                                        End If

                                        If GetLastStatusEODSchedulerLog(lngPK_EODSchedulerLog_ID) = MsEODStatus.TryCanceling Then
                                            IsprocessValid = False
                                        End If

                                    ElseIf objEOdTaskDetail.FK_EODTaskDetailType_ID = EODTaskDetailType.StoreProcedure Then
                                        RunEodTaskDetailStoreProcedure(objEOdTaskDetail, itemTaskDetailLog, objEodSchedulerLog)

                                        EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                                        If GetLastStatusEODSchedulerLog(lngPK_EODSchedulerLog_ID) = MsEODStatus.TryCanceling Then
                                            IsprocessValid = False
                                        End If

                                    ElseIf objEOdTaskDetail.FK_EODTaskDetailType_ID = EODTaskDetailType.SSISinSQLagent Then

                                        objDb.Database.ExecuteSqlCommand("exec Usp_Console_UpdateParamEODTaskDetailLogID @PK_EODTaskDetailLog_ID",
                                            New Data.SqlClient.SqlParameter("@PK_EODTaskDetailLog_ID", itemTaskDetailLog.PK_EODTaskDetailLog_ID))

                                        If RunEODTaskDetailSSISSQLAgent(objEOdTaskDetail, itemTaskDetailLog) Then
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                                        Else
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.ErrorProcess), "Error Running SSIS " & objEOdTaskDetail.SSISName)

                                        End If
                                        If GetLastStatusEODSchedulerLog(lngPK_EODSchedulerLog_ID) = MsEODStatus.TryCanceling Then
                                            IsprocessValid = False
                                        End If
                                    ElseIf objEOdTaskDetail.FK_EODTaskDetailType_ID = EODTaskDetailType.API Then

                                        Dim strpathddl As String = ""
                                        Dim objparam As SystemParameter = GetSystemParameter(4004)
                                        If Not objparam Is Nothing Then
                                            strpathddl = objparam.SettingValue
                                        End If

                                        Try
                                            ExecuteAPI(strpathddl, objEOdTaskDetail.SSISName, objEOdTaskDetail.StoreProcedureName, itemTaskDetailLog.PK_EODTaskDetailLog_ID)
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                                            If GetLastStatusEODSchedulerLog(lngPK_EODSchedulerLog_ID) = MsEODStatus.TryCanceling Then
                                                IsprocessValid = False
                                            End If
                                        Catch ex As Exception
                                            ' Dim errormessage As String = ex.Message + " " + ex.StackTrace
                                            Dim errormessage As String = ""
                                            If Not ex.InnerException Is Nothing Then
                                                errormessage += " " + ex.InnerException.Message + " " + ex.InnerException.StackTrace
                                            End If
                                            IsprocessValid = False
                                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.ErrorProcess), "Error Running API " & objEOdTaskDetail.SSISName & " " & errormessage)

                                        End Try


                                    End If
                                Else
                                    EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.ErrorProcess), "There is no task detail " & objEOdTaskDetail.StoreProcedureName)
                                End If
                            Catch ex As Exception

                                strErrorMessage = ex.Message
                                EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.ErrorProcess), strErrorMessage)
                                IsprocessValid = False
                                lngTaskDetailProcess = itemTaskDetailLog.PK_EODTaskDetailLog_ID

                                Dim objEOdTaskDetail As EODTaskDetail = objDb.EODTaskDetails.Where(Function(x) x.PK_EODTaskDetail_ID = itemTaskDetailLog.FK_EODTAskDetail_ID).FirstOrDefault()

                                If objEOdTaskDetail.FK_EODTaskDetailType_ID = EODTaskDetailType.SSIS Then
                                    mylog.LogError("An error has been occurred on RunEODProcess, package " + objEOdTaskDetail.SSISName, ex)
                                Else
                                    mylog.LogError("An error has been occurred on RunEODProcess, Store Procedure " + objEOdTaskDetail.StoreProcedureName, ex)
                                End If

                            End Try
                        Else

                            '//Kalo sebelumnya ada process package yg gagal,
                            '//Maka gak usah dijalankan, sisanya lgsg dianggap gagal aja

                            '//Update Status Log Task Detail jadi In Progress
                            EODTaskDetailStart(itemTaskDetailLog.PK_EODTaskDetailLog_ID)

                            '//Update Status Log Package jadi Error
                            EODTaskDetailEnd(itemTaskDetailLog.PK_EODTaskDetailLog_ID, Convert.ToInt32(MsEODStatus.Canceled), "Task Detail is canceled")
                        End If

                    Next

                    If IsprocessValid Then
                        EODTaskEnd(itemTaskLog.PK_EODTaskLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                    Else
                        EODTaskEnd(itemTaskLog.PK_EODTaskLog_ID, Convert.ToInt32(MsEODStatus.Canceled), "Task Detail is canceled")
                    End If

                Next

                If IsprocessValid Then
                    EodSchedulerEnd(lngPK_EODSchedulerLog_ID, Convert.ToInt32(MsEODStatus.Sucess), "")
                Else
                    EodSchedulerEnd(lngPK_EODSchedulerLog_ID, Convert.ToInt32(MsEODStatus.Canceled), "Task Detail is canceled")
                End If
            Catch ex As Exception
                mylog.LogError("An error has been occurred on RunEODProcess, Run EOD Scheduler", ex)
            End Try
        End Using

    End Sub

    Function GetLastStatusEODSchedulerLog(lngPK_EODSchedulerLog_ID As Long) As Long
        Return Convert.ToInt64(SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, "SELECT el.FK_MsEODStatus_ID FROM EODSchedulerLog AS el WHERE el.PK_EODSchedulerLog_ID=" & lngPK_EODSchedulerLog_ID, Nothing))
    End Function
    Private Function ExecuteSSISBySQLAgent(strFullFileDirectory As String, strPackageName As String) As Boolean

        Dim strPkgLocation As String
        Dim pkg As New Package()
        Dim app As New Application()

        Try

            strPkgLocation = strFullFileDirectory
            pkg = app.LoadPackage(strPkgLocation, Nothing)

            Dim strListConnectionString As String = ""
            Dim strCommand As String = "/FILE ""\""" & strFullFileDirectory & "\"" "


            Using objDb As NawaDataEntities = New NawaDataEntities

                Dim objConnectionString As List(Of NawadataConnectionString) = objDb.NawadataConnectionStrings.Where(Function(x) x.Active = True).ToList

                If Not pkg Is Nothing Then
                    For i As Integer = 0 To pkg.Connections.Count - 1
                        Dim objFind As NawadataConnectionString = objConnectionString.Find(Function(x) x.ConnecitonName.ToLower = pkg.Connections(i).Name.ToLower)
                        If Not objFind Is Nothing Then

                            Dim connectionstring As String = GetConnectionStringByPk(objFind.PK_NawaDataConnectionString_ID)
                            strListConnectionString += " /CONNECTION " & pkg.Connections(i).Name & ";""\"" " & connectionstring & "\"" "
                        End If

                    Next
                End If

            End Using

            strCommand += strListConnectionString & " /CHECKPOINTING OFF /REPORTING E"


            Dim sql As String
            sql = " " & vbCrLf _
    & "DECLARE @nawajobname VARCHAR(200)= " & vbCrLf _
    & "DECLARE @nawaloginname VARCHAR(200)= " & vbCrLf _
    & "DECLARE @nawacommand VARCHAR(8000)= " & vbCrLf _
    & " " & vbCrLf _
    & " " & vbCrLf _
    & "/****** Object:  Job [test12]    Script Date: 9/2/2017 4:08:18 PM ******/ " & vbCrLf _
    & "EXEC msdb.dbo.sp_delete_job @job_name=@nawajobname , @delete_unused_schedule=1 " & vbCrLf _
    & " " & vbCrLf _
    & " " & vbCrLf _
    & "/****** Object:  Job [test12]    Script Date: 9/2/2017 4:08:18 PM ******/ " & vbCrLf _
    & "BEGIN TRANSACTION " & vbCrLf _
    & "DECLARE @ReturnCode INT " & vbCrLf _
    & "SELECT @ReturnCode = 0 " & vbCrLf _
    & "/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 9/2/2017 4:08:19 PM ******/ " & vbCrLf _
    & "IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1) " & vbCrLf _
    & "BEGIN " & vbCrLf _
    & "EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]' " & vbCrLf _
    & "IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback " & vbCrLf _
    & " " & vbCrLf _
    & "END " & vbCrLf _
    & " " & vbCrLf _
    & "DECLARE @jobId BINARY(16) " & vbCrLf _
    & "EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@nawajobname,  " & vbCrLf _
    & "		@enabled=1,  " & vbCrLf _
    & "		@notify_level_eventlog=0,  " & vbCrLf _
    & "		@notify_level_email=0,  " & vbCrLf _
    & "		@notify_level_netsend=0,  " & vbCrLf _
    & "		@notify_level_page=0,  " & vbCrLf _
    & "		@delete_level=0,  " & vbCrLf _
    & "		@description=N'No description available.',  " & vbCrLf _
    & "		@category_name=N'[Uncategorized (Local)]',  " & vbCrLf _
    & "		@owner_login_name=@nawaloginname , @job_id = @jobId OUTPUT " & vbCrLf _
    & "IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback " & vbCrLf _
    & "/****** Object:  Step [test12]    Script Date: 9/2/2017 4:08:19 PM ******/ " & vbCrLf _
    & "EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=@nawajobname,  " & vbCrLf _
    & "		@step_id=1,  " & vbCrLf _
    & "		@cmdexec_success_code=0,  " & vbCrLf _
    & "		@on_success_action=1,  " & vbCrLf _
    & "		@on_success_step_id=0,  " & vbCrLf _
    & "		@on_fail_action=2,  " & vbCrLf _
    & "		@on_fail_step_id=0,  " & vbCrLf _
    & "		@retry_attempts=0,  " & vbCrLf _
    & "		@retry_interval=0,  " & vbCrLf _
    & "		@os_run_priority=0, @subsystem=N'SSIS',  " & vbCrLf _
    & "		@command=@nawacommand,  " & vbCrLf _
    & "		@database_name=N'master',  " & vbCrLf _
    & "		@flags=0 " & vbCrLf _
    & "IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback " & vbCrLf _
    & "EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1 " & vbCrLf _
    & "IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback " & vbCrLf _
    & "EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)' " & vbCrLf _
    & "IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback " & vbCrLf _
    & "COMMIT TRANSACTION " & vbCrLf _
    & "GOTO EndSave " & vbCrLf _
    & "QuitWithRollback: " & vbCrLf _
    & "    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION " & vbCrLf _
    & "EndSave: " & vbCrLf _
    & " " & vbCrLf _
    & "EXEC msdb.dbo.sp_start_job @nawajobname " & vbCrLf _
    & ""

            'Execute

            Return (False)
        Catch ex As Exception
            Throw
        Finally
            pkg = Nothing

            app = Nothing

        End Try

    End Function

    Function GetConnectionStringByPk(pk As Integer) As String

        Dim objparam(0) As SqlParameter
        objparam(0) = New SqlParameter
        objparam(0).ParameterName = "@pkid"
        objparam(0).Value = pk

        Return Convert.ToString(SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.StoredProcedure, "Nawa_usp_GetConnectionStringByPK", objparam))
    End Function

    Private Function ExecuteDTSX(strFullFileDirectory As String, strPackageName As String) As Boolean
        Dim strPkgLocation As String
        Dim pkg As New Package()
        Dim app As New Application()
        Dim pkgResults As New DTSExecResult()

        Try
            strPkgLocation = strFullFileDirectory

            pkg = app.LoadPackage(strPkgLocation, Nothing)


            Using objDb As NawaDataEntities = New NawaDataEntities

                Dim objConnectionString As List(Of NawadataConnectionString) = objDb.NawadataConnectionStrings.Where(Function(x) x.Active = True).ToList

                If Not pkg Is Nothing Then
                    For i As Integer = 0 To pkg.Connections.Count - 1
                        Dim objFind As NawadataConnectionString = objConnectionString.Find(Function(x) x.ConnecitonName.ToLower = pkg.Connections(i).Name.ToLower)
                        If Not objFind Is Nothing Then

                            Dim connectionstring As String = GetConnectionStringByPk(objFind.PK_NawaDataConnectionString_ID)
                            pkg.Connections(i).ConnectionString = connectionstring


                        End If

                    Next
                End If

            End Using

            'Execute
            pkgResults = pkg.Execute(Nothing, Nothing, Nothing, Nothing, Nothing)

            Select Case pkgResults
                Case DTSExecResult.Canceled
                    Throw New Exception((Convert.ToString("Dtsx Package ") & strPackageName) + " is canceled")
                Case DTSExecResult.Success
                    Return (True)
                Case DTSExecResult.Failure
                    Dim strDTSXErrorMessage As String = ""
                    For i As Integer = 0 To pkg.Errors.Count - 1
                        strDTSXErrorMessage += pkg.Errors(i).Description + ", "
                    Next
                    Throw New Exception(Convert.ToString((Convert.ToString("Error running package ") & strPackageName) + ": ") & strDTSXErrorMessage)
                Case DTSExecResult.Completion
                    Return (True)
            End Select
            Return (False)
        Catch ex As Exception
            Throw
        Finally
            pkg = Nothing
            pkgResults = Nothing
            app = Nothing

        End Try

    End Function

    Function RunEODTaskDetailSSISSQLAgent(ByVal objEODTaskDetail As EODTaskDetail, ByVal objEODTaskLogDetail As EODTaskDetailLog) As Boolean
        If objEODTaskDetail.SSISFIle.Length > 0 Then
            Dim strFullFileDirectory As String = ""
            strFullFileDirectory = My.Settings.StrTempFileDirectory & "\" & objEODTaskDetail.SSISName
            System.IO.File.WriteAllBytes(strFullFileDirectory, objEODTaskDetail.SSISFIle)

            Return (ExecuteSSISBySQLAgent(strFullFileDirectory, IO.Path.GetFileNameWithoutExtension(objEODTaskDetail.SSISName)))
        Else
            Return False
        End If
    End Function

    Function RunEodTaskDetailSSIS(ByVal objEODTaskDetail As EODTaskDetail, ByVal objEODTaskLogDetail As EODTaskDetailLog) As Boolean

        If objEODTaskDetail.SSISFIle.Length > 0 Then
            Dim strFullFileDirectory As String = ""
            strFullFileDirectory = My.Settings.StrTempFileDirectory & "\" & objEODTaskDetail.SSISName
            System.IO.File.WriteAllBytes(strFullFileDirectory, objEODTaskDetail.SSISFIle)

            Return (ExecuteDTSX(strFullFileDirectory, objEODTaskDetail.SSISName))
        Else
            Return False
        End If

    End Function

    Function RunEodTaskDetailStoreProcedure(ByVal objEODTaskDetail As EODTaskDetail, ByVal objEODTaskLogDetail As EODTaskDetailLog, objSchedulerLog As EODSchedulerLog) As Boolean
        If objEODTaskDetail.StoreProcedureName.Length > 0 Then


            Using objDb As NawaDataEntities = New NawaDataEntities

                Dim paramStr As String = " @PK_EODTaskDetailLog_ID"
                Dim objParamTaskID As New SqlParameter("@PK_EODTaskDetailLog_ID", objEODTaskLogDetail.PK_EODTaskDetailLog_ID)
                Dim objParamDate As New SqlParameter("@DataDate", objSchedulerLog.DataDate)
                Dim objParamBranch As New SqlParameter("@KodeCabang", objSchedulerLog.KodeCabang)

                If objEODTaskDetail.IsUseParameterProcessDate Then
                    paramStr += ", @DataDate"
                End If
                If objEODTaskDetail.IsUseParameterBranch Then
                    paramStr += ", @KodeCabang"
                End If



                If Not objEODTaskDetail.IsUseParameterProcessDate And objEODTaskDetail.IsUseParameterBranch Then
                    objDb.Database.ExecuteSqlCommand("EXEC " + objEODTaskDetail.StoreProcedureName & paramStr, objParamTaskID, objParamBranch)
                End If

                If Not objEODTaskDetail.IsUseParameterProcessDate And Not objEODTaskDetail.IsUseParameterBranch Then
                    objDb.Database.ExecuteSqlCommand("EXEC " + objEODTaskDetail.StoreProcedureName & paramStr, objParamTaskID)
                End If
                If objEODTaskDetail.IsUseParameterProcessDate And Not objEODTaskDetail.IsUseParameterBranch Then
                    objDb.Database.ExecuteSqlCommand("EXEC " + objEODTaskDetail.StoreProcedureName & paramStr, objParamTaskID, objParamDate)
                End If

                If objEODTaskDetail.IsUseParameterProcessDate And objEODTaskDetail.IsUseParameterBranch Then
                    objDb.Database.ExecuteSqlCommand("EXEC " + objEODTaskDetail.StoreProcedureName & paramStr, objParamTaskID, objParamDate, objParamBranch)
                End If



                Return True
            End Using
        End If

    End Function

    Sub EODTaskDetailEnd(ByVal lngPK_EODTaskDetailLog_ID As Long, intEODStatus As Integer, strErrorMessage As String)

        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODTaskDetailLog = objDb.EODTaskDetailLogs.Where(Function(x) x.PK_EODTaskDetailLog_ID = lngPK_EODTaskDetailLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.Enddate = DateTime.Now
                objdata.FK_MsEODStatus_ID = intEODStatus
                objdata.ErrorMessage = strErrorMessage
                objDb.SaveChanges()
            End If
        End Using
    End Sub

    Sub EODTaskDetailStart(ByVal lngPK_EODTaskDetailLog_ID As Long)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODTaskDetailLog = objDb.EODTaskDetailLogs.Where(Function(x) x.PK_EODTaskDetailLog_ID = lngPK_EODTaskDetailLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.StartDate = DateTime.Now
                objdata.FK_MsEODStatus_ID = MsEODStatus.Inprogress
                objDb.SaveChanges()
            End If
        End Using
    End Sub

    Sub EodSchedulerEnd(ByVal lngPK_EODSchedulerLog_ID As Long, intEODStatus As Integer, strErrorMessage As String)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODSchedulerLog = objDb.EODSchedulerLogs.Where(Function(x) x.PK_EODSchedulerLog_ID = lngPK_EODSchedulerLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.Enddate = DateTime.Now
                objdata.FK_MsEODStatus_ID = intEODStatus
                objdata.ErrorMessage = strErrorMessage
                objDb.SaveChanges()
            End If
        End Using
    End Sub

    Sub EODTaskEnd(ByVal lngPK_EODTaskLog_ID As Long, intEODStatus As Integer, strErrorMessage As String)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODTaskLog = objDb.EODTaskLogs.Where(Function(x) x.PK_EODTaskLog_ID = lngPK_EODTaskLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.Enddate = DateTime.Now
                objdata.FK_MsEODStatus_ID = intEODStatus
                objdata.ErrorMessage = strErrorMessage
                objDb.SaveChanges()
            End If
        End Using
    End Sub

    Sub EODTaskStart(ByVal lngPK_EODTaskLog_ID As Long)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODTaskLog = objDb.EODTaskLogs.Where(Function(x) x.PK_EODTaskLog_ID = lngPK_EODTaskLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.StartDate = DateTime.Now
                objdata.FK_MsEODStatus_ID = MsEODStatus.Inprogress
                objDb.SaveChanges()
            End If
        End Using
    End Sub

    Sub EODSchedulerStart(ByVal lngPK_EODSchedulerLog_ID As Long)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objdata As EODSchedulerLog = objDb.EODSchedulerLogs.Where(Function(x) x.PK_EODSchedulerLog_ID = lngPK_EODSchedulerLog_ID).FirstOrDefault
            If Not objdata Is Nothing Then
                objdata.StartDate = DateTime.Now
                objdata.FK_MsEODStatus_ID = MsEODStatus.Inprogress
                objDb.SaveChanges()
            End If
        End Using

    End Sub

    Sub ExecSPCurrentScheduler()
        Using objDb As NawaDataEntities = New NawaDataEntities

            ' Create a SqlParameter for each parameter in the stored procedure.
            Dim intPeriodprocessdate As Integer = objDb.SystemParameters.Where(Function(x) x.PK_SystemParameter_ID = 3000).FirstOrDefault.SettingValue

            'Dim objparamdate As New Data.SqlClient.SqlParameter("@datProcessDate", DateAdd(DateInterval.Day, intPeriodprocessdate, DateTime.Now).ToString("yyyy-MM-dd"))
            Dim objparamdate As New Data.SqlClient.SqlParameter("@datProcessDate", DateTime.Now)
            Dim objparamuser As New Data.SqlClient.SqlParameter("@strUserId", "sysadmin")
            Dim objparamdatadate As New Data.SqlClient.SqlParameter("@datadate", DateAdd(DateInterval.Day, intPeriodprocessdate, DateTime.Now).ToString("yyyy-MM-dd"))

            objDb.Database.ExecuteSqlCommand("exec Usp_Console_InsertEODLog @datProcessDate,@strUserId,@datadate", objparamdate, objparamuser, objparamdatadate)
        End Using

    End Sub

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class