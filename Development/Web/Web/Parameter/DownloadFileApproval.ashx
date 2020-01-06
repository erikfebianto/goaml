<%@ WebHandler Language="VB" Class="DownloadFileApproval" %>

Imports System
Imports System.Web
Imports NawaDAL

Public Class DownloadFileApproval : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            Dim IDData As String = HttpUtility.HtmlEncode(context.Request.Params("ID"))
            Dim fieldname As String = HttpUtility.HtmlEncode(context.Request.Params("Fieldname"))
            Dim strNewOld As String = HttpUtility.HtmlEncode(context.Request.Params("NewOld"))



            Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
                Dim objmoduleapproval As NawaDAL.ModuleApproval = objDb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = IDData).FirstOrDefault
                Dim strModulename As String = ""

                If Not objmoduleapproval Is Nothing Then
                    strModulename = objmoduleapproval.ModuleName

                    Dim objData As String = ""
                    If strNewOld = "new" Then
                        objData = objmoduleapproval.ModuleField
                    ElseIf strNewOld = "old" Then
                        objData = objmoduleapproval.ModuleFieldBefore
                    End If
                    Dim objxmldoc As New Xml.XmlDocument
                    objxmldoc.LoadXml(objData)
                    Dim objNodeFilename As Xml.XmlNode
                    Dim objNode As Xml.XmlNode

                    objNodeFilename = objxmldoc.SelectSingleNode("/Data/" & strModulename & "/" & fieldname & "Name")
                    objNode = objxmldoc.SelectSingleNode("/Data/" & strModulename & "/" & fieldname)
                    Dim strfilename As String = objNodeFilename.InnerXml


                    If objNode.InnerXml <> "" Then
                        Dim arrdata As Byte() = Convert.FromBase64String(objNode.InnerXml)
                        If Not arrdata Is Nothing Then
                            context.Response.Clear()
                            context.Response.ClearHeaders()
                            context.Response.AddHeader("content-disposition", "attachment;filename=" & strfilename)
                            context.Response.Charset = ""
                            context.Response.AddHeader("cache-control", "max-age=0")



                            context.Response.ContentType = System.Web.MimeMapping.GetMimeMapping(strfilename)
                            context.Response.BinaryWrite(arrdata)
                            context.Response.End()


                        End If
                    End If


                End If
            End Using




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