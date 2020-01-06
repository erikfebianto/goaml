Imports NawaDevDAL
Imports NawaDevBLL
Imports NawaDAL
Imports NawaBLL
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.Collections.Generic
Imports System.IO
Partial Class SLIK_TextFileUploadView
    Inherits Parent
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("TextFileUploadView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("TextFileUploadView.ObjModule") = value
        End Set
    End Property

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                'ClearSession()
                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel

                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If

            'objEodTask.BentukformAdd()
            LoadJenisFile()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub LoadJenisFile()
        Dim QueryString As String = ""
        Dim objdt As System.Data.DataTable

        'Jenis Perjalanan Dinas
        QueryString = "SELECT DISTINCT DisplayValue = cm.FK_FormInfo_ID, DisplayField = m.ModuleLabel FROM TextFileColumnMapping cm INNER JOIN ORS_FormInfo fi ON cm.FK_FormInfo_ID = fi.Kode INNER JOIN Module m ON fi.FK_Module_ID = m.PK_Module_ID"

        QueryString = QueryString & " ORDER BY m.ModuleLabel"
        objdt = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, querystring, Nothing)
        StoreJenisFile.DataSource = objdt
        StoreJenisFile.DataBind()

    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As DirectEventArgs)
        Try
            WindowProgress.Hidden = False
            StartLongAction()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub btnOK_DirectEvent()
        If Me.Session("LongActionError") Is Nothing Then
            Dim intmoduleID As Integer = 0
            Dim directory As String = ""

            Using objdb As New NawaDAL.NawaDataEntities
                intmoduleID = (From x In objdb.Modules Where x.ModuleName = "TextFileTemporaryTable" Select x).FirstOrDefault.PK_Module_ID
                directory = (From x In objdb.Modules Where x.ModuleName = "TextFileTemporaryTable" Select x).FirstOrDefault.UrlView
            End Using
            Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
        Else
            Dim intmoduleID As Integer = 0
            Dim directory As String = ""

            Using objdb As New NawaDAL.NawaDataEntities
                intmoduleID = (From x In objdb.Modules Where x.ModuleName = "TextFileUpload" Select x).FirstOrDefault.PK_Module_ID
                directory = (From x In objdb.Modules Where x.ModuleName = "TextFileUpload" Select x).FirstOrDefault.UrlView
            End Using
            Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
        End If

    End Sub
    Private Shared Function GetToArray(ms As MemoryStream) As Byte()
        Return ms.ToArray
    End Function

    Private Shared Function GetBuffer1(buffer As Byte) As Byte
        Return buffer
    End Function

    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Shared Function addNewModelField(FieldName As String, FieldType As ModelFieldType) As Ext.Net.ModelField
        Dim objfield As Ext.Net.ModelField
        objfield = New Ext.Net.ModelField
        objfield.Name = FieldName
        objfield.Type = FieldType
        objfield.AllowNull = True
        Return objfield
    End Function

    Shared Function addNewColumn(ColumnText As String, ColumnIndex As String, Flex As Integer, ClientMode As Web.UI.ClientIDMode) As Ext.Net.Column
        Dim objcolum As Ext.Net.Column
        objcolum = New Ext.Net.Column
        objcolum.Text = ColumnText
        objcolum.DataIndex = ColumnIndex
        objcolum.ClientIDMode = ClientMode
        'objcolum.Flex = Flex
        Return objcolum
    End Function

    Sub StartLongAction()
        Session("LongActionProgress") = 0

        System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf LongAction))
        ResourceManager.GetInstance.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID)
    End Sub

    Private Sub LongAction()
        Try
            Dim stream As Stream = uploadText.PostedFile.InputStream
            Dim reader As New StreamReader(uploadText.PostedFile.InputStream)
            Dim line As String = Nothing

            Me.Session("LongActionProgress") = 2
            System.Threading.Thread.Sleep(1000)

            Dim textFileTemporaryTable As New TextFileTemporaryTable
            With textFileTemporaryTable
                .FileName = uploadText.PostedFile.FileName
                .FK_FormInfo_ID = CboJenisFile.SelectedItem.Value
                .DataSource = "SLIK"
                .TanggalData = Date.Now
                .Status = "Ready to Integrate"
            End With

            Dim lines As Integer = 0
            While (reader.Peek() <> -1)
                line = reader.ReadLine().Trim()

                ''Baca Header 022011006201610100000005 
                'If lines = 0 Then
                '    Dim formName As String = "F" + line.Substring(line.Trim.Length - 11, 3)
                '    If formName = "F102" Or formName = "F202" Or formName = "F207" Or formName = "F301" Or formName = "F201" Then
                '        Dim jenisLaporan As String = line.Substring(line.Trim.Length - 21, 2)
                '        If jenisLaporan = "01" Then
                '            formName += "K"
                '        End If
                '        If jenisLaporan = "08" Then
                '            formName += "S"
                '        End If
                '    End If

                '    'Dim TextFileTemporaryTableDetails As New List(Of TextFileTemporaryTableDetail)
                '    Dim tanggalData As DateTime = DateTime.ParseExact(line.Substring(line.Trim.Length - 19, 8), "ddMMyyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                '    Dim dataSource As String = IIf(uploadText.PostedFile.FileName.ToLower.Contains("sibs"), "SIBS", "MUREX")

                '    textFileTemporaryTable.FileName = uploadText.PostedFile.FileName
                '    textFileTemporaryTable.FK_FormInfo_ID = formName
                '    textFileTemporaryTable.DataSource = dataSource
                '    textFileTemporaryTable.TanggalData = tanggalData
                '    textFileTemporaryTable.Status = "Ready to Integrate"

                'ElseIf line.Length <> 0 Then
                Dim textFileTemporaryTableDetail As New TextFileTemporaryTableDetail()
                textFileTemporaryTableDetail.Line = lines
                textFileTemporaryTableDetail.Data = line

                textFileTemporaryTable.TextFileTemporaryTableDetails.Add(textFileTemporaryTableDetail)
                'End If



                lines += 1
            End While

            Me.Session("LongActionProgress") = 6

            stream.Seek(0, SeekOrigin.Begin)
            Using br As New BinaryReader(stream)
                textFileTemporaryTable.[Data] = br.ReadBytes((stream.Length))
            End Using

            Dim Directory As String = ""

            Using db As New NawaDatadevEntities

                If db.ORS_FormInfo.Any(Function(X) X.Kode = textFileTemporaryTable.FK_FormInfo_ID) Then
                    db.TextFileTemporaryTables.Add(textFileTemporaryTable)
                    db.SaveChanges()

                Else
                    Throw New Exception("Unable parse file to LHBU Form")
                End If

            End Using

            Using myConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("NawadataSql").ConnectionString)
                Using myCmd As SqlCommand = myConn.CreateCommand
                    myCmd.CommandText = "exec usp_TextFileTemporaryTable_Process"
                    myConn.Open()

                    Me.Session("LongActionProgress") = 8
                    System.Threading.Thread.Sleep(1000)

                    myCmd.ExecuteNonQuery()

                End Using
            End Using

            If textFileTemporaryTable.TextFileTemporaryTableDetails.Count = 0 Then
                Me.Session("LongActionError") = "Upload Failed! File " + uploadText.PostedFile.FileName + " does not have detail record"
            Else
                Me.Session("LongActionError") = Nothing
            End If

        Catch ex As Exception
            Me.Session("LongActionError") = "Upload Failed! Unable Parse " + uploadText.PostedFile.FileName + " to LHBU Form"
        End Try


        Me.Session.Remove("LongActionProgress")
    End Sub
    Protected Sub RefreshProgress(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim progress As Object = Me.Session("LongActionProgress")
        If Not progress Is Nothing Then
            Progress1.UpdateProgress((progress) / 10.0F, String.Format(""))
        Else
            ResourceManager.GetInstance.AddScript("{0}.stopTask('longactionprogress');", Me.TaskManager1.ClientID)
            If Me.Session("LongActionError") Is Nothing Then
                Progress1.UpdateProgress(1, " Upload Selesai")
                btnOK.Hidden = False
            Else

                Progress1.UpdateProgress(1, CStr(Me.Session("LongActionError")))
                btnOK.Hidden = False
            End If

        End If


    End Sub


End Class
