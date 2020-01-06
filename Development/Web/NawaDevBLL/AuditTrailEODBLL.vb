Imports NawaDevDAL
Imports NawaBLL

Public Class AuditTrailEODBLL
    Implements IDisposable


    Public Shared Function GetSchedulerLogByPK(taskDetailPKParam As Long) As vw_EODLog

        Using db As New NawaDatadevEntities()
            Dim result As vw_EODLog = db.vw_EODLog.FirstOrDefault(Function(x) x.PK_EODTaskDetailLog_ID = taskDetailPKParam)
            
            Return result
        End Using

    End Function

    Public Shared Function GetEODStatusByStatusName(statusNameParam As String) As MsEODStatu

        Using db As New NawaDatadevEntities()
            Dim result As MsEODStatu = db.MsEODStatus.FirstOrDefault(Function(x) x.MsEODStatusName = statusNameParam)

            Return result
        End Using

    End Function

    Public Shared Function GetAllListVw_EODLog(statusNameParam As String) As List(Of Vw_EODLog)

        Using db As New NawaDatadevEntities()
            Dim result As List(Of Vw_EODLog) = db.vw_EODLog.ToList()

            Return result
        End Using

    End Function

    Public Shared Sub ChangeScheduleStatusToCancel(taskDetailPKParam As Long, currentModuleParam As NawaDAL.Module)


        Using db As New NawaDatadevEntities()
            Dim currentEODLog As vw_EODLog = GetSchedulerLogByPK(taskDetailPKParam)
            Dim schedulerLogData As EODSchedulerLog = db.EODSchedulerLogs.FirstOrDefault(Function(x) x.PK_EODSchedulerLog_ID = currentEODLog.PK_EODSchedulerLog_ID)
            
            ChangeEODStatusWithoutApproval(schedulerLogData, currentModuleParam)
        End Using

    End Sub

    Private Shared Sub ChangeEODStatusWithoutApproval(currentSchedulerLogDataParam As EODSchedulerLog, currentModuleParam As NawaDAL.Module)

        Using objdb As New NawaDatadevEntities
            Using objtrans As Entity.DbContextTransaction = objdb.Database.BeginTransaction()

                Try
                    'Untuk cancel, FK nya adalah 5
                    currentSchedulerLogDataParam.FK_MsEODStatus_ID = 5

                    objdb.Entry(currentSchedulerLogDataParam).State = Entity.EntityState.Modified
                    Dim objaudittrailheader As AuditTrailHeader = NawaFramework.CreateAuditTrail(objdb, Common.SessionCurrentUser.UserID, Common.AuditTrailStatusEnum.AffectedToDatabase, Common.ModuleActionEnum.Update, currentModuleParam.ModuleLabel)
                    NawaFramework.CreateAuditTrailDetailAdd(objdb, objaudittrailheader.PK_AuditTrail_ID, currentSchedulerLogDataParam)

                    objdb.SaveChanges()
                    objtrans.Commit()

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try

            End Using
        End Using

    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
