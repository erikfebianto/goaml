Imports System.IO
Imports System.Threading
Imports OfficeOpenXml
Public Class EmailTemplateBLL
    Private mylog As New NawaConsoleLog
    Sub run()

        'looping di emailtemplate yg aktif dan status monitoringnya bukan 1(none)
        While True


            Try

                Thread.Sleep(My.Settings.IntThreadInterval)
                Using objDb As NawaDataEntities = New NawaDataEntities
                    objDb.Database.ExecuteSqlCommand("exec usp_SchedulerEmail ")

                End Using

                'insert detailnya
                Dim dt As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT * FROM EmailTemplateScheduler ets INNER JOIN EmailTemplate et ON ets.PK_EmailTemplate_ID=et.PK_EmailTemplate_ID WHERE ets.FK_EmailStatus_ID=1 AND et.FK_Monitoringduration_ID <>1")

                For Each item As DataRow In dt.Rows
                    Try



                        Dim dtTablePrimary As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT eta.NamaTable, eta.QueryData, eta.FieldUnikPrimaryTable  FROM EmailTemplateAdditional eta WHERE eta.FK_EmailTableType_ID=1 AND eta.FK_EmailTemplate_ID=" & item("PK_EmailTemplate_ID"), Nothing)

                        For Each item1 As DataRow In dtTablePrimary.Rows

                            Using objDb As NawaDataEntities = New NawaDataEntities

                                Dim objparamtable As New Data.SqlClient.SqlParameter("@tablename", item1("NamaTable"))
                                Dim objparamquerydata As New Data.SqlClient.SqlParameter("@querydata", System.Net.WebUtility.HtmlDecode(item1("QueryData")))

                                objDb.Database.ExecuteSqlCommand("exec usp_CreateTableEmailPrimary @tablename,@querydata", objparamtable, objparamquerydata)

                            End Using


                            Dim sql As String
                            sql = "INSERT INTO EmailTemplateSchedulerDetail "
                            sql += " ( "
                            sql += " 	 "
                            sql += " 	FK_EmailTEmplateScheduler_ID, "
                            sql += " 	UnikFieldTablePrimary, "
                            sql += " 	EmailTo, "
                            sql += " 	EmailCC, "
                            sql += " 	EmailBCC, "
                            sql += " 	EmailSubject, "
                            sql += " 	EmailBody, "
                            sql += " 	ProcessDate, "
                            sql += " 	SendEmailDate, "
                            sql += " 	FK_EmailStatus_ID, "
                            sql += " 	ErrorMessage, "
                            sql += " 	retrycount "
                            sql += " ) "
                            sql += " select  "
                            sql += " '" & item("PK_EmailTemplateScheduler_ID") & "' ,"
                            sql += " " & item1("FieldUnikPrimaryTable") & " ,"
                            sql += " '" & item("EmailTo") & "' ,"
                            sql += " '" & item("EmailCC") & "' ,"
                            sql += " '" & item("EmailBCC") & "' ,"
                            sql += " '" & item("EmailSubject") & "' ,"
                            sql += " '" & item("EmailBody") & "' ,"
                            sql += " '" & CDate(item("ProcessDate")).ToString("yyyy-MM-dd HH:mm:ss") & "' ,"
                            sql += " null ,"
                            sql += " '1' ,"
                            sql += " '' ,"
                            sql += " '0' "
                            sql += " from __" & item1("NamaTable")


                            SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, sql, Nothing)

                            'buat table detail dulu untuk setiap master


                            Dim dtschedulerdetail As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT * FROM EmailTemplateSchedulerDetail WHERE FK_EmailTEmplateScheduler_ID=" & item("PK_EmailTemplateScheduler_ID"), Nothing)

                            Dim dtemaildetail As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT etd.Replacer , SUBSTRING(etd.FieldReplacer,1,1)+'__'+SUBSTRING(etd.FieldReplacer,2,LEN(etd.FieldReplacer)-1) AS FieldReplacer  FROM EmailTemplateDetail etd WHERE etd.FK_EmailTemplate_ID=" & item("PK_EmailTemplate_ID"), Nothing)

                            For Each rowschedulerdetail As DataRow In dtschedulerdetail.Rows

                                Dim dtTableAdditional As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT eta.NamaTable, eta.QueryData, eta.FieldUnikPrimaryTable ,eta.FK_EmailTableType_ID FROM EmailTemplateAdditional eta WHERE (eta.FK_EmailTableType_ID=2 or eta.FK_EmailTableType_ID=3) AND eta.FK_EmailTemplate_ID=" & item("PK_EmailTemplate_ID"), Nothing)
                                For Each item2 As DataRow In dtTableAdditional.Rows


                                    'generate table additional detail
                                    Using objDb As NawaDataEntities = New NawaDataEntities
                                        Dim strquery As String = System.Net.WebUtility.HtmlDecode(item2("QueryData"))
                                        strquery = strquery.Replace("@ID", "'" & rowschedulerdetail("UnikFieldTablePrimary") & "'")

                                        Dim objparamtable As New Data.SqlClient.SqlParameter("@tablename", item2("NamaTable"))
                                        Dim objparamquerydata As New Data.SqlClient.SqlParameter("@querydata", strquery)

                                        objDb.Database.ExecuteSqlCommand("exec usp_CreateTableEmailPrimary @tablename,@querydata", objparamtable, objparamquerydata)
                                    End Using
                                Next


                                ' Insert Table Email Attachment
                                Dim StrSQLAtttachment As String = "INSERT INTO [dbo].[EmailTemplateSchedulerDetailAttachment] ([FK_EmailTEmplateSchedulerDetail_ID], [FK_EmailAttachmentType_ID], [NamaReport], [ParameterReport], [IsiFile], [FileName], EmailRenderAsName, FileExtension, [MimeType]) " & vbCrLf _
                                            & "Select EmailTemplateSchedulerDetail.PK_EmailTemplateSchedulerDetail_ID, EmailTemplateAttachment.FK_EmailAttachmentType_ID, EmailTemplateAttachment.NamaReport, EmailTemplateAttachment.ParameterReport, EmailTemplateAttachment.IsiFile, EmailTemplateAttachment.NamaFile, EmailRenderAs.EmailRenderAsName, EmailRenderAs.FileExtension, EmailRenderAs.MimeType  " & vbCrLf _
                                            & "From EmailTemplateSchedulerDetail INNER Join " & vbCrLf _
                                            & "EmailTemplateScheduler On EmailTemplateSchedulerDetail.FK_EmailTEmplateScheduler_ID = EmailTemplateScheduler.PK_EmailTemplateScheduler_ID INNER Join " & vbCrLf _
                                            & "EmailTemplateAttachment On EmailTemplateScheduler.PK_EmailTemplate_ID = EmailTemplateAttachment.FK_EmailTemplate_ID " & vbCrLf _
                                            & "INNER JOIN EmailRenderAs on EmailTemplateAttachment.FK_EmailRenderAs_Id=EmailRenderAs.PK_EmailRenderAs_ID " & vbCrLf _
                                            & "Where EmailTemplateSchedulerDetail.PK_EmailTemplateSchedulerDetail_ID = {0} "
                                StrSQLAtttachment = String.Format(StrSQLAtttachment, rowschedulerdetail("PK_EmailTemplateSchedulerDetail_ID").ToString())

                                SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, StrSQLAtttachment, Nothing).ToString()

                                'replace isi tabledetail

                                For Each rowreplacer As DataRow In dtemaildetail.Rows

                                    Using objDb As NawaDataEntities = New NawaDataEntities
                                        Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateSchedulerDetail_ID", rowschedulerdetail("PK_EmailTemplateSchedulerDetail_ID"))
                                        Dim objparamreplacer As New Data.SqlClient.SqlParameter("@Replacer", "'" & rowreplacer("Replacer") & "'")
                                        Dim objparamfieldreplacer As New Data.SqlClient.SqlParameter("@FieldReplacer", rowreplacer("FieldReplacer"))
                                        Dim objparampkemailtemplate As New Data.SqlClient.SqlParameter("@pk_emailitemplate_id", item("PK_EmailTemplate_ID"))

                                        objDb.Database.ExecuteSqlCommand("exec usp_replaceEmailSchedulerDetail @PK_EmailTemplateSchedulerDetail_ID,@Replacer,@FieldReplacer,@pk_emailitemplate_id", objparampk, objparamreplacer, objparamfieldreplacer, objparampkemailtemplate)

                                    End Using
                                Next
                                ' Replace $EmailID$ With EmailID Field
                                Using objDb As NawaDataEntities = New NawaDataEntities
                                    Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateSchedulerDetail_ID", rowschedulerdetail("PK_EmailTemplateSchedulerDetail_ID"))
                                    Dim objparamreplacer As New Data.SqlClient.SqlParameter("@Replacer", "$EmailId$")
                                    Dim objparamfieldreplacer As New Data.SqlClient.SqlParameter("@FieldReplacer", rowschedulerdetail("EmailID"))
                                    Dim objparampkemailtemplate As New Data.SqlClient.SqlParameter("@pk_emailitemplate_id", item("PK_EmailTemplate_ID"))
                                    objDb.Database.ExecuteSqlCommand("exec usp_replaceEmailSchedulerDetail @PK_EmailTemplateSchedulerDetail_ID,@Replacer,@FieldReplacer,@pk_emailitemplate_id", objparampk, objparamreplacer, objparamfieldreplacer, objparampkemailtemplate)
                                End Using
                                ' Update EmailIDReference with EmailID because of System Initiate
                                SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, String.Format("UPDATE EmailTemplateSchedulerDetail SET EmailIDReference=EmailID WHERE PK_EmailTemplateSchedulerDetail_ID = {0} AND (EmailIDReference IS NULL OR EmailIDReference='')", rowschedulerdetail("PK_EmailTemplateSchedulerDetail_ID").ToString()), Nothing).ToString()


                                '    Dim dtTableAttachment As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT eta.NamaTable, eta.QueryData, eta.FieldUnikPrimaryTable ,eta.FK_EmailTableType_ID FROM EmailTemplateAdditional eta WHERE (eta.FK_EmailTableType_ID=3) AND eta.FK_EmailTemplate_ID=" & item("PK_EmailTemplate_ID"), Nothing)


                                '    Dim pathFile As String = My.Settings.strTempFileAttachment
                                '    For Each item3 As DataRow In dtTableAttachment.Rows
                                '        'create excel,convert binary,simpan ke emailtemplateschedulerid
                                '        Dim strfileName As String = pathFile & "\" & Now.ToString("yyyyMMddHHmmss") & " - " & rowschedulerdetail("UnikFieldTablePrimary") & ".xlsx"
                                '        'todohendra:buat table conditional formatting
                                '        Dim strconditionalFormating As String = SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, " SELECT eacf.ConditionalFormatting FROM EmailAttachmentConditionalFormating eacf WHERE eacf.FK_EmailTemplate_ID=" & item("PK_EmailTemplate_ID"), Nothing)
                                '        strconditionalFormating = strconditionalFormating.ToLower.Replace("today()", """" & Now.ToString("yyyy-MM-dd") & """")
                                '        CreateExcelAttachment(item3("NamaTable"), strfileName, strconditionalFormating)
                                '        If IO.File.Exists(strfileName) Then
                                '            SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, " UPDATE EmailTemplateSchedulerDetail SET Attachment = '" & strfileName & "' WHERE PK_EmailTemplateSchedulerDetail_ID=" & rowschedulerdetail("PK_EmailTemplateSchedulerDetail_ID"), Nothing)
                                '        End If
                                '   Next
                                '
                                'update status jadi queue untuk EmailTemplateScheduler dan EmailTemplateSchedulerdetail
                            Next
                        Next
                        Using objDb As NawaDataEntities = New NawaDataEntities
                            Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateScheduler_ID", item("PK_EmailTemplateScheduler_ID"))
                            objDb.Database.ExecuteSqlCommand("exec usp_updateEmailStatusScheduler @PK_EmailTemplateScheduler_ID", objparampk)
                        End Using
                    Catch ex As Exception
                        mylog.LogError("An error has been occurred on RunEODProcess, Run EOD Email Template ", ex)
                    End Try
                Next

                ' Update Generate Email Attachment Status
                SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, "EXEC usp_UpdateEmailGenerateAttachmentStatus", Nothing).ToString()

                'Generate Email Attachment From SSRS
                Dim StrSql As String
                'StrSql = "SELECT FK_EmailTEmplateSchedulerDetail_ID, FK_EmailAttachmentType_ID,   " & vbCrLf _
                '        & " NamaReport, ParameterReport, IsiFile, [FileName], EmailRenderAsName, FileExtension, MimeType " & vbCrLf _
                '        & " FROM EmailTemplateSchedulerDetailAttachment " & vbCrLf _
                '        & " WHERE        (EmailTemplateSchedulerDetailAttachment.FK_EmailGenerateAttachmentStatus_ID = 2)"
                StrSql = "SELECT FK_EmailTEmplateSchedulerDetail_ID, FK_EmailAttachmentType_ID,   " & vbCrLf _
                        & " NamaReport, ParameterReport, IsiFile, [FileName], EmailRenderAsName, FileExtension, MimeType " & vbCrLf _
                        & " FROM EmailTemplateSchedulerDetailAttachment a " & vbCrLf _
                        & " LEFT join EmailTemplateSchedulerDetail b" & vbCrLf _
                        & " ON a.FK_EmailTEmplateSchedulerDetail_ID = b.PK_EmailTEmplateSchedulerDetail_ID" & vbCrLf _
                        & " WHERE        (a.FK_EmailGenerateAttachmentStatus_ID = 2)" & vbCrLf _
                        & " AND FK_EmailStatus_ID NOT IN ('4','5')"

                Dim DtNeedGenerateEmailAttachment As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, StrSql)
                For Each DrRow In DtNeedGenerateEmailAttachment.Rows
                    Dim FK_EmailTEmplateSchedulerDetail_ID As Integer
                    FK_EmailTEmplateSchedulerDetail_ID = Integer.Parse(DrRow("FK_EmailTEmplateSchedulerDetail_ID").ToString())
                    Dim NamaReport As String
                    NamaReport = DrRow("NamaReport").ToString()
                    Dim StrReportParameter As String
                    StrReportParameter = DrRow("ParameterReport").ToString()
                    Dim StrRenderAs As String
                    StrRenderAs = DrRow("EmailRenderAsName").ToString()
                    Dim strFileExtention As String = DrRow("FileExtension").ToString()
                    ReportingServiceAttachment.GenerateEmailAttachmentFromSSRS(FK_EmailTEmplateSchedulerDetail_ID, strFileExtention, NamaReport, StrReportParameter, StrRenderAs)
                Next

                'send email
                Dim intretrycounttosend = 0
                Using objDb As NawaDataEntities = New NawaDataEntities
                    Dim objsysparam As SystemParameter = objDb.SystemParameters.Where(Function(x) x.PK_SystemParameter_ID = 2012).FirstOrDefault
                    If Not objsysparam Is Nothing Then
                        intretrycounttosend = objsysparam.SettingValue
                    End If
                End Using

                'Dim dtEmail As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT * FROM EmailTemplateScheduler a INNER JOIN EmailTemplateSchedulerDetail b ON a.PK_EmailTemplateScheduler_ID=b.FK_EmailTEmplateScheduler_ID WHERE (b.FK_EmailStatus_ID=2 OR b.FK_EmailStatus_ID=3 or b.FK_EmailStatus_ID=5 )  and b.retrycount<" & intretrycounttosend, Nothing)

                Dim dtEmail As DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT * FROM EmailTemplateScheduler a INNER JOIN EmailTemplateSchedulerDetail b ON a.PK_EmailTemplateScheduler_ID=b.FK_EmailTEmplateScheduler_ID  WHERE (b.FK_EmailStatus_ID=2 OR b.FK_EmailStatus_ID=3 or b.FK_EmailStatus_ID=5 ) and b.retrycount<" & intretrycounttosend, Nothing)

                Dim strsender As String = ""
                Using objDb As NawaDataEntities = New NawaDataEntities
                    Dim objsysparam As SystemParameter = objDb.SystemParameters.Where(Function(x) x.PK_SystemParameter_ID = 2005).FirstOrDefault
                    If Not objsysparam Is Nothing Then
                        strsender = objsysparam.SettingValue
                    End If
                End Using

                For Each item As DataRow In dtEmail.Rows


                    Try

                        ''update status 3 (inprogress)
                        Using objDb As NawaDataEntities = New NawaDataEntities

                            Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateSchedulerDetail_ID", item("PK_EmailTemplateSchedulerDetail_ID"))
                            Dim objparamfk As New Data.SqlClient.SqlParameter("@FkStatusemailid", 3)
                            Dim objparamerr As New Data.SqlClient.SqlParameter("@errorMessage", "")
                            objDb.Database.ExecuteSqlCommand("exec usp_updateEmailStatusSchedulerProcess @PK_EmailTemplateSchedulerDetail_ID,@FkStatusemailid,@errorMessage", objparampk, objparamfk, objparamerr)
                        End Using

                        Dim Header As New Dictionary(Of String, String)
                        Header.Add("EmailID", item("EmailID"))

                        'item("PK_EmailTemplateSchedulerDetail_ID")
                        ' Add Attachment
                        Dim DtEmailAttachment As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT [FK_EmailTEmplateSchedulerDetail_ID], [IsiFile], [FileName] FROM [EmailTemplateSchedulerDetailAttachment] WHERE FK_EmailTEmplateSchedulerDetail_ID=" & item("PK_EmailTemplateSchedulerDetail_ID"))
                        Dim DicAttachment As Dictionary(Of String, Stream) = New Dictionary(Of String, Stream)
                        For Each DrEmailAttachment In DtEmailAttachment.Rows
                            Dim FileAttachmentEmailRaw As Byte()
                            FileAttachmentEmailRaw = DrEmailAttachment("IsiFile")
                            If Not IsNothing(FileAttachmentEmailRaw) Then
                                Dim MsDataToAttach As Stream = New MemoryStream(FileAttachmentEmailRaw)
                                Dim StrFileName As String
                                StrFileName = DrEmailAttachment("FileName").ToString()
                                DicAttachment.Add(StrFileName, MsDataToAttach)
                            End If
                        Next

                        SendEmail(strsender, Header, item("EmailTo"), item("EmailCC"), item("EmailBCC"), item("EmailSubject"), item("EmailBody"), DicAttachment)

                        ''update status 4 (done/sucesssend)
                        Using objDb As NawaDataEntities = New NawaDataEntities

                            Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateSchedulerDetail_ID", item("PK_EmailTemplateSchedulerDetail_ID"))
                            Dim objparamfk As New Data.SqlClient.SqlParameter("@FkStatusemailid", 4)
                            Dim objparamerr As New Data.SqlClient.SqlParameter("@errorMessage", "")
                            objDb.Database.ExecuteSqlCommand("exec usp_updateEmailStatusSchedulerProcess @PK_EmailTemplateSchedulerDetail_ID,@FkStatusemailid,@errorMessage", objparampk, objparamfk, objparamerr)
                        End Using

                    Catch ex As Exception
                        ''update status 5 (failtosend)
                        Using objDb As NawaDataEntities = New NawaDataEntities

                            Dim objparampk As New Data.SqlClient.SqlParameter("@PK_EmailTemplateSchedulerDetail_ID", item("PK_EmailTemplateSchedulerDetail_ID"))
                            Dim objparamerr As New Data.SqlClient.SqlParameter("@errorMessage", ex.Message)
                            objDb.Database.ExecuteSqlCommand("exec usp_updateLogErrorEmail @PK_EmailTemplateSchedulerDetail_ID,@errorMessage", objparampk, objparamerr)


                        End Using



                    End Try



                Next


            Catch ex As Exception
                mylog.LogError(ex.Message, ex)
            End Try
        End While

    End Sub

    Public Function CreateExcelAttachment(strnamatable As String, strfileName As String, strconditionalFormating As String) As Boolean

        If IO.File.Exists(strfileName) Then
            IO.File.Delete(strfileName)
        End If

        Dim objfileinfo As IO.FileInfo
        objfileinfo = New IO.FileInfo(strfileName)

        Using objtbl As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "select * from [__" & strnamatable & "]", Nothing)
            Using resource As New ExcelPackage(objfileinfo)
                Dim ws As ExcelWorksheet = resource.Workbook.Worksheets.Add("Attachment")
                ws.Cells("A1").LoadFromDataTable(objtbl, True)

                Dim dateformat As String = SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, "SELECT sp.SettingValue FROM SystemParameter sp WHERE sp.PK_SystemParameter_ID=6", Nothing)
                Dim intcolnumber As Integer = 0
                For Each item As DataColumn In objtbl.Columns
                    If item.DataType = GetType(Date) Then
                        intcolnumber = intcolnumber + 1
                        ws.Column(intcolnumber).Style.Numberformat.Format = dateformat

                    End If
                Next



                ws.Cells(ws.Dimension.Address).AutoFitColumns()




                If strconditionalFormating <> "" Then
                    'Dim _statement As String = "DATEDIF($C1,""" & Now.ToString("yyyy-MM-dd") & """, ""m"")=1"
                    Dim _statement As String = strconditionalFormating
                    Dim _cond = ws.ConditionalFormatting.AddExpression(New ExcelAddress(ws.Dimension.Address))
                    _cond.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    _cond.Style.Fill.BackgroundColor.Color = System.Drawing.Color.Blue
                    _cond.Formula = _statement


                End If



                resource.Save()

            End Using
        End Using

        Return True
    End Function

    Public Function SendEmail(ByVal sender As String, strHeaderLIst As Dictionary(Of String, String), ByVal StrRecipientTo As String, ByVal StrRecipientCC As String, ByVal StrRecipientBCC As String, ByVal StrSubject As String, ByVal strbody As String) As Boolean
        Dim oEmail As EMail
        Try
            oEmail = New EMail
            oEmail.Sender = sender
            oEmail.HeaderList = strHeaderLIst
            oEmail.Recipient = StrRecipientTo
            oEmail.RecipientCC = StrRecipientCC
            oEmail.RecipientBCC = StrRecipientBCC
            'If strattachment <> "" Then
            '    oEmail.MailAttachment = strattachment
            'End If
            oEmail.Subject = StrSubject
            oEmail.Body = strbody.Replace(vbLf, "<br>")
            If oEmail.SendEmail() Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function SendEmail(ByVal sender As String, strHeaderLIst As Dictionary(Of String, String), ByVal StrRecipientTo As String, ByVal StrRecipientCC As String, ByVal StrRecipientBCC As String, ByVal StrSubject As String, ByVal strbody As String, DicAttachment As Dictionary(Of String, Stream)) As Boolean
        Dim oEmail As EMail
        Try
            oEmail = New EMail
            oEmail.Sender = sender
            oEmail.HeaderList = strHeaderLIst
            oEmail.Recipient = StrRecipientTo
            oEmail.RecipientCC = StrRecipientCC
            oEmail.RecipientBCC = StrRecipientBCC

            oEmail.Subject = StrSubject
            oEmail.Body = strbody.Replace(vbLf, "<br>")
            If oEmail.SendEmail(DicAttachment) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
