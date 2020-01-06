<%@ WebHandler Language="VB" Class="DownloadLHBUErrorDetail" %>

Imports System
Imports System.Web
Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports System.IO

Public Class DownloadLHBUErrorDetail : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim strid As String = context.Request.Params("ID")
        'Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)

        'Dim ListData As List(Of GeneratedFileList) = FileGenerationBLL.GetData
        'Dim objdownload As GeneratedFileList = ListData.Find(Function(x) x.PK_GeneratedFileList_ID = strid)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objdownload As TextFileTemporaryTable = (From x In objDb.TextFileTemporaryTables Where (x.Pk_TextFileTemporaryTable_ID = strid)).FirstOrDefault

            'Dim Item As String = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.Text, "SELECT TOP 1 CONVERT(VARCHAR(MAX),gfl.FileBin) FROM GeneratedFileList AS gfl WHERE PK_GeneratedFileList_ID = " & strid, Nothing)

            If Not objdownload Is Nothing Then
                context.Response.Clear()
                context.Response.ClearHeaders()
                context.Response.AddHeader("Content-Disposition", "attachment;filename=" & objdownload.FileName.Replace(".txt", "") & "_Error.txt")
                context.Response.Charset = Encoding.UTF8.WebName
                context.Response.ContentEncoding = Encoding.UTF8
                context.Response.AddHeader("cache-control", "max-age=0")
                context.Response.ContentType = "text/plain"

                Dim sb As New StringBuilder
                For Each err As String In objdownload.ErrorMessage.Replace("</br> ", "|").Split("|")
                    sb.AppendLine(err)
                Next

                context.Response.BinaryWrite(System.Text.Encoding.Unicode.GetBytes(sb.ToString()))
                context.Response.End()

            End If
        End Using

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class