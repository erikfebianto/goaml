
Public Class TableProperties
    Private eFieldName As String
    Private eTableName As String
    Private eTableFieldName As String
    Sub New()
        eFieldName = ""
        eTableName = ""
        eTableFieldName = ""
    End Sub
    Public Property FieldName As String
        Get
            Return eFieldName
        End Get
        Set(value As String)
            eFieldName = value
        End Set
    End Property
    Public Property TableName As String
        Get
            Return eTableName
        End Get
        Set(value As String)
            eTableName = value
        End Set
    End Property
    Public Property TableFieldName As String
        Get
            Return eTableFieldName
        End Get
        Set(value As String)
            eTableFieldName = value
        End Set
    End Property
End Class

Public Class sp_columnsProperties
    Private eFieldName As String
    Private eTableName As String
    Private eTypeName As String
    Private eNullable As Boolean
    Private eIsIdentity As Boolean
    Sub New()
        eFieldName = ""
        eTableName = ""
        eTypeName = ""
        eNullable = False
        eIsIdentity = False
    End Sub
    Public Property FieldName As String
        Get
            Return eFieldName
        End Get
        Set(value As String)
            eFieldName = value
        End Set
    End Property
    Public Property TableName As String
        Get
            Return eTableName
        End Get
        Set(value As String)
            eTableName = value
        End Set
    End Property
    Public Property TypeName As String
        Get
            Return eTypeName
        End Get
        Set(value As String)
            eTypeName = value
        End Set
    End Property
    Public Property Nullable As Boolean
        Get
            Return eNullable
        End Get
        Set(value As Boolean)
            eNullable = value
        End Set
    End Property
    Public Property IsIdentity As Boolean
        Get
            Return eIsIdentity
        End Get
        Set(value As Boolean)
            eIsIdentity = value
        End Set
    End Property
End Class

