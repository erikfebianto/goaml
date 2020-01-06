
Partial Class RedirectPage
    Inherits System.Web.UI.Page

    Private Sub RedirectPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'budget_id=
            'project_id=
            Dim strbudgetid As String = Request.Params("budget_id")
            Dim strprojectid As String = Request.Params("project_id")
            Dim strmoduleid As String = Request.Params("module_id")



            If strbudgetid <> "" Then
                'todohendra:tambah page redirect untuk budgetid jika sudah tau pagenya
            End If
            If strprojectid <> "" Then
                'todohendra:tambah page redirect untuk projectid jika sudah tau pagenya
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
