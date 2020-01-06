<%@ WebHandler Language="VB" Class="Handler"  %>

Imports System
Imports System.Web
Imports NawaBLL
Imports NawaDAL



Public Class Handler : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As Object = 0
        Try


            Dim TanggalValuta As String = HttpUtility.HtmlEncode(context.Request.Params("TanggalValuta"))
            Dim TanggalJatuhTempo As String = HttpUtility.HtmlEncode(context.Request.Params("TanggalJatuhTempo"))
            Dim FormDescription As String = HttpUtility.HtmlEncode(context.Request.Params("FormDescription"))
            Dim MataUang1 As String = HttpUtility.HtmlEncode(context.Request.Params("MataUang1"))
            Dim MataUang2 As String = HttpUtility.HtmlEncode(context.Request.Params("MataUang2"))

            Dim strsql As String = String.Format("Exec usp_CalculateJangkaWaktu '{0}','{1}','{2}','{3}','{4}'",
                DateTime.ParseExact(TanggalValuta, NawaBLL.SystemParameterBLL.GetDateFormat(), System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                DateTime.ParseExact(TanggalJatuhTempo, NawaBLL.SystemParameterBLL.GetDateFormat(), System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                FormDescription,
                MataUang1,
                MataUang2
                )

            result = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.Text, strsql, Nothing)

            context.Response.ContentType = "text/plain"
            context.Response.Write(result)

        Catch ex As Exception
            context.Response.ContentType = "text/plain"
            context.Response.Write(result)
        End Try



    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
