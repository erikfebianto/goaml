Imports NawaDevDAL
Imports System.Data.SqlClient
<Serializable()> Public Class SLIKParameterBLL
    Implements IDisposable

    Shared Function getSlikparameter() As SLIKParameter


        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.SLIKParameters.FirstOrDefault
        End Using
    End Function

    Shared Function getCabangName(KodeCabang As String) As String


        Dim objListParam(0) As SqlParameter
        objListParam(0) = New SqlParameter


        objListParam(0).ParameterName = "@kodecabang"
        objListParam(0).Value = KodeCabang


        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetBranchName", objListParam)
    End Function

    Shared Function getSettingSLIKPersonal(userid As String) As SettingPersonal
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.SettingPersonals.Where(Function(x) x.UserID = userid).FirstOrDefault()
        End Using
    End Function

    Shared Function SaveSettingToDb(objSettingSlikpersonal As SettingPersonal)

        Dim objListParam(2) As SqlParameter

        objListParam(0) = New SqlParameter
        objListParam(0).ParameterName = "@userid"
        objListParam(0).Value = objSettingSlikpersonal.UserID

        objListParam(1) = New SqlParameter
        objListParam(1).ParameterName = "@ReportDate"
        objListParam(1).Value = objSettingSlikpersonal.ReportDate

        objListParam(2) = New SqlParameter
        objListParam(2).ParameterName = "@kodecabang"
        objListParam(2).Value = objSettingSlikpersonal.KodeCabang

        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_SaveSettingPersonal", objListParam)
    End Function

    Public Shared Property SessionSettingSLIKPersonal() As SettingPersonal
        Get
            Dim ObjWebPage As New System.Web.UI.Page
            Try
                Return ObjWebPage.Session("SLIK.SettingSLIKPersonal")

            Catch
                Return Nothing
            Finally
                ObjWebPage.Dispose()
            End Try
        End Get
        Set(ByVal Value As SettingPersonal)
            Dim ObjWebPage As New System.Web.UI.Page
            Try
                ObjWebPage.Session("SLIK.SettingSLIKPersonal") = Value
            Catch
                Throw
            Finally
                ObjWebPage.Dispose()
            End Try
        End Set
    End Property


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
