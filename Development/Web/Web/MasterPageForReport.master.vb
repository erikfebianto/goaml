
Partial Class MasterPageForReport
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
    End Sub
End Class

