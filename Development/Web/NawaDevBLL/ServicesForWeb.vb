Imports CookComputing.XmlRpc

<XmlRpcUrl("http://localhost:1450/")>
Public Interface ServicesForWeb
    Inherits IXmlRpcProxy

    <XmlRpcMethod("GenerateTextFileFromWeb")>
    Function GenerateTextFileFromWeb(TanggalData As DateTime,
                                     KodeCabang As String,
                                     UserName As String,
                                     ArrTemplate() As String) As Boolean

    <XmlRpcMethod("ValidateRecordsFromWeb")>
    Function ValidateRecordsFromWeb(Bulan As String,
                                     ModuleName() As String,
                                     Tahun As String, kodecabang As String) As Boolean

    <XmlRpcMethod("CleanRecords")> _
    Function CleanRecords(RecordID() As String,
                          UserName As String) As Boolean

    <XmlRpcMethod("CleanRecordsAll")>
    Function CleanRecordsAll(UserName As String) As Boolean

    <XmlRpcMethod("InsertAsDictionary")> _
    Function InsertAsDictionary(RecordID() As String) As Boolean
End Interface
