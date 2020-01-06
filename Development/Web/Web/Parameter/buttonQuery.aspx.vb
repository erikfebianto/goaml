
Partial Class buttonQuery
    Inherits System.Web.UI.Page


    Public Function GetRootURL() As String        
        Return Request.CurrentExecutionFilePath.Remove(Request.CurrentExecutionFilePath.LastIndexOf("/") + 1)
    End Function

    Public Function GetQueryDesignerObject() As String
        If Session("buttonQuery.GetQueryDesignerObject") Is Nothing Or Session("buttonQuery.GetQueryDesignerObject") = "" Then
            Return "{}"
        Else
            Return Session("buttonQuery.GetQueryDesignerObject")
        End If

    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
                ViewDesignerQuery.Value = Session("buttonQuery.DataQuery")
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
