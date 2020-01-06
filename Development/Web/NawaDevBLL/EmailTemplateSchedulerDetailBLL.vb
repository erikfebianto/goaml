Imports Ext.Net

Public Class EmailTemplateSchedulerDetailBLL
    Implements IDisposable
    Public oPanelinput As FormPanel

    Public Sub New(oPanel As Ext.Net.FormPanel)
        oPanelinput = oPanel
    End Sub

    Shared Function DeleteTanpaapproval(unik As String, objSchemaModule As NawaDAL.Module)

        Using objdb As New NawaDevDAL.NawaDatadevEntities


            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objdel As NawaDevDAL.EmailTemplateSchedulerDetail = objdb.EmailTemplateSchedulerDetails.Where(Function(x) x.EmailID = unik).FirstOrDefault

                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    Dim objtype As Type = objdel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(objdel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objdel, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    objdb.Entry(objdel).State = Entity.EntityState.Deleted

                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Shared Function GetEmailStatus(emailstatus As Integer) As String
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objresult As NawaDevDAL.EmailStatu = objDb.EmailStatus.Where(Function(x) x.PK_EmailStatus_ID = emailstatus).FirstOrDefault
            If Not objresult Is Nothing Then
                Return objresult.EmailStatusName
            Else
                Return ""
            End If
        End Using

    End Function

    Shared Function GetEmailTemplateNameBySchedulerID(emailschedulerid As String) As String
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objresult As NawaDevDAL.EmailTemplateScheduler = objDb.EmailTemplateSchedulers.Where(Function(x) x.PK_EmailTemplateScheduler_ID = emailschedulerid).FirstOrDefault

            If Not objresult Is Nothing Then
                Dim objemtemp As NawaDevDAL.EmailTemplate = objDb.EmailTemplates.Where(Function(y) y.PK_EmailTemplate_ID = objresult.PK_EmailTemplate_ID).FirstOrDefault
                If Not objemtemp Is Nothing Then
                    Return objemtemp.EmailTemplateName
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Using
    End Function

    Shared Function GetEmailTemplateSchedulerbyEmailID(EmailID As String) As NawaDevDAL.EmailTemplateSchedulerDetail
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.EmailTemplateSchedulerDetails.Where(Function(x) x.EmailID = EmailID).FirstOrDefault

        End Using

    End Function

    Shared Function IsDataValidDelete(unik As String, objmodule As NawaDAL.Module)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            If objDb.EmailTemplateSchedulerDetails.Where(Function(x) x.EmailID = unik).Count > 0 Then
                Return True
            Else
                Return False
            End If

        End Using

    End Function

    Shared Sub LoadPanel(oPanelinput As FormPanel, strModulename As String, unikkey As String)

        Dim objShedulerEmailTemplateDetail As NawaDevDAL.EmailTemplateSchedulerDetail = GetEmailTemplateSchedulerbyEmailID(unikkey)

        If Not objShedulerEmailTemplateDetail Is Nothing Then

            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "EmailID", "EmailID", objShedulerEmailTemplateDetail.EmailID)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Template", "EmailTemplate", GetEmailTemplateNameBySchedulerID(objShedulerEmailTemplateDetail.FK_EmailTEmplateScheduler_ID))
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email To", "EmailTo", objShedulerEmailTemplateDetail.EmailTo)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email CC", "EmailCC", objShedulerEmailTemplateDetail.EmailCC)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email BCC", "EmailBCC", objShedulerEmailTemplateDetail.EmailBCC)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Subject", "EmailSubject", objShedulerEmailTemplateDetail.EmailSubject)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Body", "EmailBody", objShedulerEmailTemplateDetail.EmailBody)
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Process Date", "Processdate", objShedulerEmailTemplateDetail.ProcessDate.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat))
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Send Email Date", "SendEmailDate", objShedulerEmailTemplateDetail.SendEmailDate.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat))
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Status", "EmailStatus", GetEmailStatus(objShedulerEmailTemplateDetail.FK_EmailStatus_ID))
            NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Error Message", "ErrorMessage", objShedulerEmailTemplateDetail.ErrorMessage)

        End If
    End Sub
    Shared Function SaveEditTanpaApproval(ObjEmailTemplateSchedulerDetail As NawaDevDAL.EmailTemplateSchedulerDetail, objSchemaModule As NawaDAL.Module) As Boolean
        'todohendra: save ke table EmailTemplateSchedulerDetail

        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    objdb.Entry(ObjEmailTemplateSchedulerDetail).State = Entity.EntityState.Modified

                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    Dim objtype As Type = ObjEmailTemplateSchedulerDetail.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(ObjEmailTemplateSchedulerDetail, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(ObjEmailTemplateSchedulerDetail, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next

                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Sub BentukformEdit()
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "EmailID", "EmailID", "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Template", "EmailTemplate", "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email To", "EmailTo", True, 1000, "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email CC", "EmailCC", False, 1000, "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email BCC", "EmailBCC", False, 1000, "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email Subject", "EmailSubject", True, 1000, "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtHTMLText(oPanelinput, "Email Body", "EmailBody")
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Process Date", "Processdate", "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Send Email Date", "SendEmailDate", "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Status", "EmailStatus", "")
        NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Error Message", "ErrorMessage", "")

    End Sub

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

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
#End Region

End Class