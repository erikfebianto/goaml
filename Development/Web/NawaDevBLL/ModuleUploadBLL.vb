<Serializable()> Public Class datamodules

    'Public dtmodule As DataTable
    Public dtmodulefield As DataTable

    'Public dtmodule As List(Of Modules)
    'Public dtmodulefield As List(Of NawaDevDAL.ModuleField)

    Private _dtmodule As List(Of ClassModules)
    Public Property dtmodule() As List(Of ClassModules)
        Get
            Return _dtmodule
        End Get
        Set(ByVal value As List(Of ClassModules))
            _dtmodule = value
        End Set
    End Property

    Public Class ClassModules
        Private _Action As String
        Public Property Action() As String
            Get
                Return _Action
            End Get
            Set(ByVal value As String)
                _Action = value
            End Set
        End Property

        Private _ErrorMessage As String
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal value As String)
                _ErrorMessage = value
            End Set
        End Property

        Private _PK_Module_ID As String
        Public Property PK_Module_ID() As String
            Get
                Return _PK_Module_ID
            End Get
            Set(ByVal value As String)
                _PK_Module_ID = value
            End Set
        End Property

        Private _ModuleName As String
        Public Property ModuleName() As String
            Get
                Return _ModuleName
            End Get
            Set(ByVal value As String)
                _ModuleName = value
            End Set
        End Property

        Private _ModuleLabel As String
        Public Property ModuleLabel() As String
            Get
                Return _ModuleLabel
            End Get
            Set(ByVal value As String)
                _ModuleLabel = value
            End Set
        End Property

        Private _ModuleDescription As String
        Public Property ModuleDescription() As String
            Get
                Return _ModuleDescription
            End Get
            Set(ByVal value As String)
                _ModuleDescription = value
            End Set
        End Property

        Private _IsUseDesigner As String
        Public Property IsUseDesigner() As String
            Get
                Return _IsUseDesigner
            End Get
            Set(ByVal value As String)
                _IsUseDesigner = value
            End Set
        End Property

        Private _IsUseApproval As String
        Public Property IsUseApproval() As String
            Get
                Return _IsUseApproval
            End Get
            Set(ByVal value As String)
                _IsUseApproval = value
            End Set
        End Property

        Private _IsSupportAdd As String
        Public Property IsSupportAdd() As String
            Get
                Return _IsSupportAdd
            End Get
            Set(ByVal value As String)
                _IsSupportAdd = value
            End Set
        End Property

        Private _IsSupportEdit As String
        Public Property IsSupportEdit() As String
            Get
                Return _IsSupportEdit
            End Get
            Set(ByVal value As String)
                _IsSupportEdit = value
            End Set
        End Property

        Private _IsSupportDelete As String
        Public Property IsSupportDelete() As String
            Get
                Return _IsSupportDelete
            End Get
            Set(ByVal value As String)
                _IsSupportDelete = value
            End Set
        End Property

        Private _IsSupportActivation As String
        Public Property IsSupportActivation() As String
            Get
                Return _IsSupportActivation
            End Get
            Set(ByVal value As String)
                _IsSupportActivation = value
            End Set
        End Property

        Private _IsSupportView As String
        Public Property IsSupportView() As String
            Get
                Return _IsSupportView
            End Get
            Set(ByVal value As String)
                _IsSupportView = value
            End Set
        End Property

        Private _IsSupportUpload As String
        Public Property IsSupportUpload() As String
            Get
                Return _IsSupportUpload
            End Get
            Set(ByVal value As String)
                _IsSupportUpload = value
            End Set
        End Property

        Private _IsSupportDetail As String
        Public Property IsSupportDetail() As String
            Get
                Return _IsSupportDetail
            End Get
            Set(ByVal value As String)
                _IsSupportDetail = value
            End Set
        End Property

        Private _UrlAdd As String
        Public Property UrlAdd() As String
            Get
                Return _UrlAdd
            End Get
            Set(ByVal value As String)
                _UrlAdd = value
            End Set
        End Property

        Private _UrlEdit As String
        Public Property UrlEdit() As String
            Get
                Return _UrlEdit
            End Get
            Set(ByVal value As String)
                _UrlEdit = value
            End Set
        End Property

        Private _UrlDelete As String
        Public Property UrlDelete() As String
            Get
                Return _UrlDelete
            End Get
            Set(ByVal value As String)
                _UrlDelete = value
            End Set
        End Property

        Private _UrlActivation As String
        Public Property UrlActivation() As String
            Get
                Return _UrlActivation
            End Get
            Set(ByVal value As String)
                _UrlActivation = value
            End Set
        End Property

        Private _UrlView As String
        Public Property UrlView() As String
            Get
                Return _UrlView
            End Get
            Set(ByVal value As String)
                _UrlView = value
            End Set
        End Property

        Private _UrlUpload As String
        Public Property UrlUpload() As String
            Get
                Return _UrlUpload
            End Get
            Set(ByVal value As String)
                _UrlUpload = value
            End Set
        End Property

        Private _UrlApproval As String
        Public Property UrlApproval() As String
            Get
                Return _UrlApproval
            End Get
            Set(ByVal value As String)
                _UrlApproval = value
            End Set
        End Property

        Private _UrlApprovalDetail As String
        Public Property UrlApprovalDetail() As String
            Get
                Return _UrlApprovalDetail
            End Get
            Set(ByVal value As String)
                _UrlApprovalDetail = value
            End Set
        End Property

        Private _UrlDetail As String
        Public Property UrlDetail() As String
            Get
                Return _UrlDetail
            End Get
            Set(ByVal value As String)
                _UrlDetail = value
            End Set
        End Property

        Private _IsUseStoreProcedureValidation As String
        Public Property IsUseStoreProcedureValidation() As String
            Get
                Return _IsUseStoreProcedureValidation
            End Get
            Set(ByVal value As String)
                _IsUseStoreProcedureValidation = value
            End Set
        End Property

        Private _Active As String
        Public Property Active() As String
            Get
                Return _Active
            End Get
            Set(ByVal value As String)
                _Active = value
            End Set
        End Property

        Private _CreatedBy As String
        Public Property CreatedBy() As String
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As String)
                _CreatedBy = value
            End Set
        End Property

        Private _LastUpdateBy As String
        Public Property LastUpdateBy() As String
            Get
                Return _LastUpdateBy
            End Get
            Set(ByVal value As String)
                _LastUpdateBy = value
            End Set
        End Property

        Private _ApprovedBy As String
        Public Property ApprovedBy() As String
            Get
                Return _ApprovedBy
            End Get
            Set(ByVal value As String)
                _ApprovedBy = value
            End Set
        End Property

        Private _CreatedDate As String
        Public Property CreatedDate() As String
            Get
                Return _CreatedDate
            End Get
            Set(ByVal value As String)
                _CreatedDate = value
            End Set
        End Property

        Private _LastUpdateDate As String
        Public Property LastUpdateDate() As String
            Get
                Return _LastUpdateDate
            End Get
            Set(ByVal value As String)
                _LastUpdateDate = value
            End Set
        End Property

        Private _ApprovedDate As String
        Public Property ApprovedDate() As String
            Get
                Return _ApprovedDate
            End Get
            Set(ByVal value As String)
                _ApprovedDate = value
            End Set
        End Property

    End Class

End Class


Public Class ModuleUploadBLL
    Implements IDisposable

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
