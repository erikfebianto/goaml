Imports NawaBLL
<Serializable()> _
Public Class ChangePasswordBLL
    Implements IDisposable


    Shared Function ChangePasword(strUserid As String, strPassword As String) As Boolean

        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim objuser As NawaDAL.MUser = (From p In objdb.MUsers Where p.UserID = strUserid Select p).FirstOrDefault

                    objuser.UserPasword = Common.Encrypt(strPassword, objuser.UserPasswordSalt)
                    objuser.LastChangePassword = DateTime.Now
                    objuser.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objuser.ApprovedDate = Now



                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objaudittrailheader.ModuleLabel = "Change Password"
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objuser.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(objuser, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objuser, Nothing)

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
