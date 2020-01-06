Imports Ext.Net
Imports NawaDAL
Imports NawaBLL
Imports System.Data.SqlClient
<Serializable()> _
Public Class ResetPasswordBLL
    Implements IDisposable
    Shared Function GetMuserByPK(id As String) As NawaDAL.MUser

        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return (From p In objDb.MUsers Where p.UserID = id Select p).FirstOrDefault
        End Using
    End Function

    Shared Function EditPaswordDirect(strUserid As String, strPassword As String) As Boolean

        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim objuserNew As NawaDAL.MUser = (From p In objdb.MUsers Where p.UserID = strUserid Select p).FirstOrDefault

                    objuserNew.UserPasword = Common.Encrypt(strPassword, objuserNew.UserPasswordSalt)
                    objuserNew.LastChangePassword = DateTime.Now
                    objuserNew.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objuserNew.ApprovedDate = Now

                    Dim objuserBefore As NawaDAL.MUser = (From p In objdb.MUsers Where p.UserID = strUserid Select p).FirstOrDefault


                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = "Reset Password"
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objuserNew.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        If Not item.GetValue(objuserbefore, Nothing) Is Nothing Then
                            objaudittraildetail.OldValue = item.GetValue(objuserbefore, Nothing)
                        Else
                            objaudittraildetail.OldValue = ""
                        End If
                        If Not item.GetValue(objuserNew, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objuserNew, Nothing)
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
    Shared Function EditPaswordApproval(strUserid As String, strPassword As String, objModule As NawaDAL.Module) As Boolean


        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim objuserNew As NawaDAL.MUser = GetMuserByPK(strUserid)
                    objuserNew.UserPasword = Common.Encrypt(strPassword, objuserNew.UserPasswordSalt)
                    objuserNew.LastChangePassword = DateTime.Now
                    objuserNew.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objuserNew.ApprovedDate = Now

                    Dim objuserbefore As NawaDAL.MUser = (From p In objdb.MUsers Where p.UserID = strUserid Select p).FirstOrDefault


                    Dim xmlNew As String = NawaBLL.Common.Serialize(objuserNew)
                    Dim xmlbefore As String = NawaBLL.Common.Serialize(objuserbefore)


                    Dim objModuleApproval As New NawaDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = objuserNew.PK_MUser_ID
                        .ModuleField = xmlNew
                        .ModuleFieldBefore = xmlbefore
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added

                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = "Reset Password"
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objuserNew.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        If Not item.GetValue(objuserbefore, Nothing) Is Nothing Then
                            objaudittraildetail.OldValue = item.GetValue(objuserbefore, Nothing)
                        Else
                            objaudittraildetail.OldValue = ""
                        End If
                        If Not item.GetValue(objuserNew, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objuserNew, Nothing)
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
    Shared Function Reject(ID As String) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If

                    Dim objUserdata As NawaDAL.MUser = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(NawaDAL.MUser))
                    Dim objUserdataOld As NawaDAL.MUser = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(NawaDAL.MUser))


                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objUserdata.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        If Not item.GetValue(objUserdataOld, Nothing) Is Nothing Then
                            objaudittraildetail.OldValue = item.GetValue(objUserdataOld, Nothing)
                        Else
                            objaudittraildetail.OldValue = ""
                        End If
                        If Not item.GetValue(objUserdata, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objUserdata, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next

                    objdb.Entry(objApproval).State = Entity.EntityState.Deleted
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function Accept(ID As String) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If

                    Dim objModuledata As NawaDAL.MUser = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(NawaDAL.MUser))
                    Dim objModuledataOld As NawaDAL.MUser = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(NawaDAL.MUser))
                    objModuledata.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objModuledata.ApprovedDate = Now
                    objdb.Entry(objModuledata).State = Entity.EntityState.Modified

                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = "Reset Password"
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    Dim objtype As Type = objModuledata.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objModuledataOld, Nothing) Is Nothing Then
                            objaudittraildetail.OldValue = item.GetValue(objModuledataOld, Nothing)
                        Else
                            objaudittraildetail.OldValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objModuledata, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                    Next

                    objdb.Entry(objApproval).State = Entity.EntityState.Deleted
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Sub LoadPanel(objPanel As FormPanel, objdata As String, strModulename As String, unikkey As String)
        Dim objResetPasswordBLL As NawaDAL.MUser = NawaBLL.Common.Deserialize(objdata, GetType(NawaDAL.MUser))
        If Not objResetPasswordBLL Is Nothing Then
            Dim strunik As String = unikkey
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_MUser_ID" & strunik, objResetPasswordBLL.PK_MUser_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "User ID", "UserID" & strunik, objResetPasswordBLL.UserID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Staff Name", "UserName" & strunik, objResetPasswordBLL.UserName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "User Pasword", "UserPasword" & strunik, objResetPasswordBLL.UserPasword)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objResetPasswordBLL.Active)

        End If
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
