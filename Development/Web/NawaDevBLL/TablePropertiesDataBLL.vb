Imports Ext.Net
Imports NawaDevDAL
Imports NawaDAL
Imports NawaBLL
Imports System.Data.SqlClient
Public Class TablePropertiesDataBLL
    Implements IDisposable
    Shared Function GetColumnProperties(ByVal query As String, ByVal TableAlias As String) As List(Of TableProperties)
        Dim initialquery As String = "declare @Period datetime "
        initialquery += " set @Period=Dateadd(year,5,getdate()) " & vbCrLf
        query = initialquery & query

        Dim e As New List(Of TableProperties)
        Dim Prop As New TableProperties

        Dim datab As New DataTable
        datab = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, query)

        For Each dtrow As DataColumn In datab.Columns
            Prop = New TableProperties
            Prop.TableName = TableAlias
            Prop.FieldName = dtrow.ColumnName
            Prop.TableFieldName = "[" & Prop.TableName & "]." & dtrow.ColumnName
            e.Add(Prop)
        Next
        Return e
    End Function
    Shared Function sp_columnsProperties(ByVal TableName As String) As List(Of sp_columnsProperties)
        Dim e As New List(Of sp_columnsProperties)
        Dim Prop As New sp_columnsProperties
        Dim datab As New DataTable


        Dim query As String
        query = "select *,COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity " &
            "From INFORMATION_SCHEMA.COLUMNS " &
            "Where Table_Name = '" & TableName & "'"


        datab = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, query)
        If datab.Rows.Count > 0 Then
            Prop.TableName = datab.TableName
            For Each dtrow As DataRow In datab.Rows
                Prop = New sp_columnsProperties
                Prop.TableName = dtrow("TABLE_NAME")
                Prop.FieldName = dtrow("COLUMN_NAME")
                Prop.TypeName = dtrow("DATA_TYPE")
                If dtrow("IS_NULLABLE") = "NO" Then
                    Prop.Nullable = False
                Else
                    Prop.Nullable = True
                End If
                Prop.IsIdentity = dtrow("IsIdentity")
                e.Add(Prop)
            Next
        End If

        Return e
    End Function
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
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
