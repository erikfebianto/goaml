Imports System.IO
Imports Ionic.Zip
Imports Ionic.Zlib
Public Class ReportingServiceAttachment
    Private mylog As New NawaConsoleLog



    Public Shared Function GenerateEmailAttachmentFromSSRS(FK_EmailTEmplateSchedulerDetail_ID As Integer, strFileExtention As String, StrReportName As String, strParameter As String, StrFormat As String) As Boolean

        Dim StrURL As String
        Dim StrCommand As String = "Render"
        'Dim StrFormat As String = "PDF"
        If String.IsNullOrEmpty(StrFormat) Then
            StrFormat = "PDF"
        End If


        Dim ObjParamReportingServiceURL As SystemParameter = Common.GetSystemParameter(13)
        Dim ObjParamReportingServiceUserName As SystemParameter = Common.GetSystemParameter(9)
        Dim ObjParamReportingServicePassword As SystemParameter = Common.GetSystemParameter(10)

        Dim ReportingServiceURL As String = "http://localhost/ReportServer_SQL2016?/"
        If Not ObjParamReportingServiceURL Is Nothing Then
            ReportingServiceURL = ObjParamReportingServiceURL.SettingValue
            If Not String.IsNullOrEmpty(ReportingServiceURL) Then
                If ReportingServiceURL.EndsWith("/") And Not ReportingServiceURL.Contains("?") Then
                    'ReportingServiceURL = ReportingServiceURL.Remove(ReportingServiceURL.LastIndexOf("/"), 1)

                    ReportingServiceURL = ReportingServiceURL.Insert(ReportingServiceURL.LastIndexOf("/"), "?")
                ElseIf Not ReportingServiceURL.EndsWith("?") Then
                    ReportingServiceURL = ReportingServiceURL & "/"
                Else
                    ReportingServiceURL = ReportingServiceURL & "?/"
                End If
            End If
        End If
        Dim StrReportingServiceUserName As String = "dev"
        If Not ObjParamReportingServiceURL Is Nothing Then
            StrReportingServiceUserName = ObjParamReportingServiceUserName.SettingValue
        End If
        Dim StrReportingServicePassword As String = "nawadata"
        If Not ObjParamReportingServiceURL Is Nothing Then
            StrReportingServicePassword = Common.DecryptRijndael(ObjParamReportingServicePassword.SettingValue, ObjParamReportingServicePassword.EncriptionKey)
        End If
        If Not strParameter.StartsWith("&") Then
            strParameter = "&" & strParameter
        End If




        'cek current email harus di zip ngak

        Dim bNeedZip As Boolean = False


        Dim sql As String
        sql = " " & vbCrLf _
    & "SELECT COUNT(1) " & vbCrLf _
    & "FROM   EmailTemplateSchedulerDetail AS etsd " & vbCrLf _
    & "       INNER JOIN EmailTemplateScheduler " & vbCrLf _
    & "            ON  etsd.FK_EmailTEmplateScheduler_ID = EmailTemplateScheduler.PK_EmailTemplateScheduler_ID " & vbCrLf _
    & "       INNER JOIN ( " & vbCrLf _
    & "                SELECT s.val " & vbCrLf _
    & "                FROM   dbo.[Split]( " & vbCrLf _
    & "                           ( " & vbCrLf _
    & "                               SELECT sp.SettingValue " & vbCrLf _
    & "                               FROM   SystemParameter AS sp " & vbCrLf _
    & "                               WHERE  sp.PK_SystemParameter_Id = 4016 " & vbCrLf _
    & "                           ), " & vbCrLf _
    & "                           ',' " & vbCrLf _
    & "                       ) AS s " & vbCrLf _
    & "            ) xx " & vbCrLf _
    & "            ON  CONVERT(VARCHAR(50), xx.val) = CONVERT(VARCHAR(50), EmailTemplateScheduler.PK_EmailTemplate_ID) " & vbCrLf _
    & "WHERE  (etsd.PK_EmailTemplateSchedulerDetail_ID = " & FK_EmailTEmplateSchedulerDetail_ID & ") " & vbCrLf _
    & ""

        Dim intresult As Integer = SQLHelper.ExecuteScalar(SQLHelper.strConnectionString, CommandType.Text, sql, Nothing)



        Dim strpasswordzip As String

        Using objDb As NawaDataEntities = New NawaDataEntities
            Dim objsysparam As SystemParameter = objDb.SystemParameters.Where(Function(x) x.PK_SystemParameter_ID = 4015).FirstOrDefault
            If Not objsysparam Is Nothing Then
                strpasswordzip = Common.DecryptRijndael(objsysparam.SettingValue, objsysparam.EncriptionKey)

            End If
        End Using



        'Dim arremail As List(Of String) = EmailTemplateNeedZip.Split(",").ToList




        Try
            'URL = Convert.ToString((Convert.ToString((Convert.ToString((Convert.ToString((Convert.ToString(URL & Convert.ToString("&rs:Command=")) & Command()) + "&rs:Format=") & Format()) + "&ReportMonth=") & paramReportMonth) + "&ReportYear=") & paramReportYear) + "&EmpID=") & paramEmpID
            StrURL = ReportingServiceURL & StrReportName & "&rs:Command=" & StrCommand & "&rs:Format=" & StrFormat & strParameter

            Dim Req As System.Net.HttpWebRequest = DirectCast(System.Net.WebRequest.Create(StrURL), System.Net.HttpWebRequest)
            'Req.Credentials = System.Net.CredentialCache.DefaultCredentials
            Req.Credentials = New Net.NetworkCredential(StrReportingServiceUserName, StrReportingServicePassword)
            Req.Method = "GET"

            Dim objResponse As System.Net.WebResponse = Req.GetResponse()
            Dim MemoryStreamData As MemoryStream = New MemoryStream()
            Dim stream As System.IO.Stream = objResponse.GetResponseStream()



            If intresult = 0 Then
                'ngak butuh di zip
                Dim buf As Byte() = New Byte(1023) {}
                Dim len As Integer = stream.Read(buf, 0, 1024)
                While len > 0
                    MemoryStreamData.Write(buf, 0, len)
                    len = stream.Read(buf, 0, 1024)
                End While
                stream.Close()
                MemoryStreamData.Close()



                Dim StrSQLAttachment As String = "UPDATE [dbo].[EmailTemplateSchedulerDetailAttachment] SET  [IsiFile]=@IsiFile, FK_EmailGenerateAttachmentStatus_ID=1  WHERE " & vbCrLf &
                                                "FK_EmailTEmplateSchedulerDetail_ID=@FK_EmailTEmplateSchedulerDetail_ID and NamaReport=@NamaReport"


                Dim ListSqlParam As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)
                ListSqlParam.Add(New SqlClient.SqlParameter("@FK_EmailTEmplateSchedulerDetail_ID", FK_EmailTEmplateSchedulerDetail_ID))

                Dim IsiFile As Byte() = MemoryStreamData.ToArray()
                Dim SqlParam As SqlClient.SqlParameter
                SqlParam = New SqlClient.SqlParameter("@IsiFile", SqlDbType.VarBinary, IsiFile.Length)
                SqlParam.Value = IsiFile
                ListSqlParam.Add(SqlParam)

                'Dim StrFileName As String = StrReportName & Guid.NewGuid().ToString() & "." & StrFormat
                ListSqlParam.Add(New SqlClient.SqlParameter("@NamaReport", StrReportName))

                SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, StrSQLAttachment, ListSqlParam.ToArray()).ToString()


            Else
                'butuh di zip
                Dim file As FileStream
                Dim strFilePathToZip As String = My.Settings.strTempFileAttachment & "\" & StrReportName & strFileExtention
                Dim strFileZip As String = My.Settings.strTempFileAttachment & "\" & StrReportName & ".zip"



                If IO.File.Exists(strFilePathToZip) Then
                    IO.File.Delete(strFilePathToZip)
                End If

                If IO.File.Exists(strFileZip) Then
                    IO.File.Delete(strFileZip)
                End If

                stream.CopyTo(MemoryStreamData)
                file = New FileStream(strFilePathToZip, FileMode.Create, System.IO.FileAccess.Write)
                file.Write(MemoryStreamData.ToArray(), 0, MemoryStreamData.Length)
                file.Close()
                MemoryStreamData.Close()
                stream.Close()

                file.Dispose()
                MemoryStreamData.Dispose()
                stream.Dispose()

                Using zip As New ZipFile

                    zip.CompressionLevel = CompressionLevel.Level9
                    zip.Password = strpasswordzip

                    zip.AddFile(strFilePathToZip, "")


                    zip.Save(strFileZip)

                    zip.Dispose()
                End Using

                Dim fileStreamZip As FileStream = New FileStream(strFileZip, FileMode.Open, System.IO.FileAccess.Read)
                Dim StrSQLAttachment As String = "UPDATE [dbo].[EmailTemplateSchedulerDetailAttachment] SET  [IsiFile]=@IsiFile, Filename=@Filename,FK_EmailGenerateAttachmentStatus_ID=1,FileExtension='.zip' WHERE " & vbCrLf &
                                           "FK_EmailTEmplateSchedulerDetail_ID=@FK_EmailTEmplateSchedulerDetail_ID and NamaReport=@NamaReport"





                Dim ListSqlParam As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)
                ListSqlParam.Add(New SqlClient.SqlParameter("@FK_EmailTEmplateSchedulerDetail_ID", FK_EmailTEmplateSchedulerDetail_ID))

                Dim IsiFile As Byte() = New Byte(fileStreamZip.Length) {}
                fileStreamZip.Read(IsiFile, 0, fileStreamZip.Length)

                Dim SqlParam As SqlClient.SqlParameter
                SqlParam = New SqlClient.SqlParameter("@IsiFile", SqlDbType.VarBinary, IsiFile.Length)
                SqlParam.Value = IsiFile
                ListSqlParam.Add(SqlParam)




                'Dim StrFileName As String = StrReportName & Guid.NewGuid().ToString() & "." & StrFormat
                ListSqlParam.Add(New SqlClient.SqlParameter("@NamaReport", StrReportName))
                ListSqlParam.Add(New SqlClient.SqlParameter("@Filename", StrReportName & ".zip"))

                SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, StrSQLAttachment, ListSqlParam.ToArray()).ToString()
                fileStreamZip.Dispose()
                fileStreamZip = Nothing


                If IO.File.Exists(strFilePathToZip) Then
                    IO.File.Delete(strFilePathToZip)
                End If

                If IO.File.Exists(strFileZip) Then
                    IO.File.Delete(strFileZip)
                End If

            End If


            'Const StrSQLAttachment As String = "UPDATE [dbo].[EmailTemplateSchedulerDetailAttachment] SET  [IsiFile]=@IsiFile, FK_EmailGenerateAttachmentStatus_ID=1  WHERE " & vbCrLf &
            '                                "FK_EmailTEmplateSchedulerDetail_ID=@FK_EmailTEmplateSchedulerDetail_ID and NamaReport=@NamaReport"


            'Dim ListSqlParam As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)
            'ListSqlParam.Add(New SqlClient.SqlParameter("@FK_EmailTEmplateSchedulerDetail_ID", FK_EmailTEmplateSchedulerDetail_ID))

            'Dim IsiFile As Byte() = MemoryStreamData.ToArray()
            'Dim SqlParam As SqlClient.SqlParameter
            'SqlParam = New SqlClient.SqlParameter("@IsiFile", SqlDbType.VarBinary, IsiFile.Length)
            'SqlParam.Value = IsiFile
            'ListSqlParam.Add(SqlParam)

            ''Dim StrFileName As String = StrReportName & Guid.NewGuid().ToString() & "." & StrFormat
            'ListSqlParam.Add(New SqlClient.SqlParameter("@NamaReport", StrReportName))

            'SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, StrSQLAttachment, ListSqlParam.ToArray()).ToString()
            Return True
        Catch ex As Exception
            Dim strerror As String = "UPDATE EmailTemplateSchedulerDetail SET FK_EmailStatus_ID = 4,ErrorMessage = '" & ex.Message.ToString & "'  WHERE PK_EmailTemplateSchedulerDetail_ID=" & FK_EmailTEmplateSchedulerDetail_ID
            SQLHelper.ExecuteNonQuery(SQLHelper.strConnectionString, CommandType.Text, strerror, Nothing)
            Throw
        End Try


    End Function

End Class
