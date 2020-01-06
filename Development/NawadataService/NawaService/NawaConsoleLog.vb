
Public Class NawaConsoleLog
    Sub LogInfo(objmessage As Object)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objlog As New LogConsoleService
            With objlog
                .LogCreatedDate = DateTime.Now
                .LogInfo = objmessage.ToString
                .LogDescription = objmessage.ToString
                .LogStatus = "INFO"
            End With
            objDb.LogConsoleServices.Add(objlog)
            objDb.SaveChanges()
        End Using

    End Sub
    Sub LogError(ByVal objmessage As Object, ByVal objexception As System.Exception)
        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objlog As New LogConsoleService
            With objlog
                .LogCreatedDate = DateTime.Now
                .LogInfo = objmessage.ToString
                .LogDescription = objexception.ToString
                .LogStatus = "ERROR"
            End With
            objDb.LogConsoleServices.Add(objlog)
            objDb.SaveChanges()
        End Using

    End Sub
End Class
