<%@ WebHandler Language="VB" Class="DownloadFile"  %>

Imports System
Imports System.Web
Imports NawaBLL
Imports NawaDAL



Public Class DownloadFile : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try


            Dim Moduleid As String = HttpUtility.HtmlEncode(context.Request.Params("ModuleID"))
            Dim IDData As String = HttpUtility.HtmlEncode(context.Request.Params("ID"))
            Dim strFilename As String = HttpUtility.HtmlEncode(context.Request.Params("Filename"))
            Dim strFieldname As String = HttpUtility.HtmlEncode(context.Request.Params("FieldName"))

            Dim intModuleid As Integer = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            Dim strIDdata As String = NawaBLL.Common.DecryptQueryString(IDData, NawaBLL.SystemParameterBLL.GetEncriptionKey)

            Dim objModule As NawaDAL.Module = ModuleBLL.GetModuleByModuleID(intModuleid)
            Dim strtablename As String = ""
            If Not objModule Is Nothing Then
                strtablename = objModule.ModuleName
            End If
            Dim strPrimarykey As String = ModuleBLL.GetPrimaryKeyField(objModule.PK_Module_ID)
            If strtablename <> "" And strPrimarykey <> "" Then
                Dim strsql As String = "select " & strFieldname & " from " & strtablename & " where  " & strPrimarykey & " ='" & strIDdata & "'"

                Dim arrdata As Byte() = Nothing
                Dim result As Object = Nothing
                result = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.Text, strsql, Nothing)
                If result.GetType.FullName <> "System.DBNull" Then
                    arrdata = result
                End If



                If Not arrdata Is Nothing Then
                    context.Response.Clear()
                    context.Response.ClearHeaders()
                    context.Response.AddHeader("content-disposition", "attachment;filename=" & strFilename)
                    context.Response.Charset = ""
                    context.Response.AddHeader("cache-control", "max-age=0")



                    context.Response.ContentType = System.Web.MimeMapping.GetMimeMapping(strFilename)
                    context.Response.BinaryWrite(arrdata)
                    context.Response.End()


                End If

            End If




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try



    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class