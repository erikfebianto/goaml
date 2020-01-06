Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL

Imports Ext.Net

Public Class CleansingBLL
    Shared Function GetData() As List(Of NawaDevDAL.CleansingReport)

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.CleansingReports.ToList
        End Using
    End Function

    Shared Function GetDataParam(ID As String) As NawaDevDAL.CleansingReport

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.CleansingReports Where x.PK_Record_ID = ID Select x).FirstOrDefault
        End Using
    End Function

    Function SaveDocument(objFileList As CleansingReport,
                            objModule As NawaDAL.Module)
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    objdb.Entry(objFileList).State = Entity.EntityState.Modified

                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Function SaveAllDocument(objFileList As List(Of CleansingReport),
                           objModule As NawaDAL.Module)
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    For Each item In objFileList
                        objdb.Entry(item).State = Entity.EntityState.Modified
                        objdb.SaveChanges()
                    Next
                    objtrans.Commit()
                Catch
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
End Class
