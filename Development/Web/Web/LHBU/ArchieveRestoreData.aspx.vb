
Imports System.Threading
Imports NawaDAL
Imports NawaDevDAL

Partial Class EODProcessManualSatuan
    Inherits Parent

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("EODProcessManualSatuan.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODProcessManualSatuan.ObjModule") = value
        End Set
    End Property


    Public Property objProcessID() As Integer
        Get
            Return Session("EODProcessManualSatuan.objProcessID")
        End Get
        Set(ByVal value As Integer)
            Session("EODProcessManualSatuan.objProcessID") = value
        End Set
    End Property
    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                objProcessID = Request.Params("ProcessID")

                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " View"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " View"


                    If Not Ext.Net.X.IsAjaxRequest Then
                        txtStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat.Replace("dd-", "").Replace("-dd", "")
                    End If

                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            'objEodTask.BentukformAdd()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub ClearSession()
        objProcessID = Nothing
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As DirectEventArgs)
        Try
            If IsValid() Then

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Function IsValid() As Boolean
        'Indra Buana, 13 Desember 2018:
        'Tambah pengecekan Archive:
        '- Jika di DB Transaksi tidak ada data, maka keluarkan error bahwa tidak ada data yg di backup (Result = 1)
        '- Jika di DB Archive sudah ada data, maka keluarkan konfirmasi ke user apakah yakin mau melakukan backup ulang (Result = 2)

        'Pengecekan Restore: 
        '- Jika di DB Archive tidak ada data, maka keluarkan error bahwa tidak ada data yg di restore (Result = 1)
        '- Jika di DB Transaksi sudah ada data, maka keluarkan konfirmasi ke user apakah yakin mau melakukan restore ulang (Result = 2)

        Dim intProcessType As Integer = 0
        If ucProcess.Value = "Archieving" Then
            intProcessType = 1
        Else
            intProcessType = 2
        End If


        Dim IntResult As Integer = 0
        Dim StrQuery As String = ""
        TanggalData = CType(txtStartDate.Value, DateTime)
        StrQuery = "EXEC usp_ArchiveValidation '" & TanggalData.ToString("yyyyMMdd") & "', " & intProcessType.ToString
        Dim dtSet As Data.DataSet = SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, StrQuery)
        If dtSet.Tables.Count > 0 Then
            Dim dtTable As Data.DataTable = dtSet.Tables(0)
            If dtTable.Rows.Count > 0 Then
                IntResult = dtTable.Rows(0).Item(0).ToString
            End If
        End If

        If IntResult = 1 Then
            If intProcessType = 1 Then
                Throw New Exception("Proses Archive gagal karena tidak ada data transaksi untuk periode data yang dipilih!")
            ElseIf intProcessType = 2 Then
                Throw New Exception("Proses Restore gagal karena tidak ada data History untuk periode data yang dipilih!")
            End If
        ElseIf IntResult = 2 Then
            WindowPrompt.Hidden = False
            FormPanelPrompt.Hidden = False
            BtnCancelPrompt.Focus()

            If intProcessType = 1 Then
                LblPrompt.Text = "Periode data yang dipilih sudah ada di data History. Apakah anda yakin untuk melakukan proses Archive ulang?"
            ElseIf intProcessType = 2 Then
                LblPrompt.Text = "Periode data yang dipilih sudah ada di data Transaksi. Apakah anda yakin untuk melakukan proses restore ulang?"
            End If
        ElseIf IntResult = 0 Then
            StartProcess()
        End If

        Return True
    End Function

    Sub StartProcess()
        WindowPrompt.Hidden = True
        FormPanelPrompt.Hidden = True

        WindowProgress.Hidden = False
        FormPanelProgress.Hidden = False
        btnOK.Hidden = True
        StartLongAction()

        StrListModule = AddLists()
        intTotalProgress = AddLists.Count
        intCurrentProgress = 0

        TanggalData = CType(txtStartDate.Value, DateTime)

        trdValidateRecord = New System.Threading.Thread(AddressOf ThreadProcess)
        trdValidateRecord.IsBackground = True
        trdValidateRecord.Start()

        LblConfirmation.Text = "Archieving Antasena Form..."
        Panelconfirmation.Hidden = False
        FormPanelInput.Hidden = True
    End Sub

#Region "Prompt Save"
    Sub BtnSubmitPrompt_DirectEvent()
        Try
            StartProcess()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub BtnCancelPrompt_DirectEvent()
        Try
            WindowPrompt.Hidden = True
            FormPanelPrompt.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

    Protected Function AddLists() As List(Of String)
        Using db As New NawaDatadevEntities

            Return db.ORS_FormInfo.Select(Function(x) x.Kode).ToList()
        End Using
    End Function

    Protected Sub RefreshProgress(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim dblPercent As Double = 0
        If intTotalProgress > 0 Then
            dblPercent = CDbl(intCurrentProgress) / CDbl(intTotalProgress)
        End If
        If intTotalProgress > 0 And intCurrentProgress = intTotalProgress Then

            ResourceManager.GetInstance.AddScript("{0}.stopTask('longactionprogress');", Me.TaskManager1.ClientID)
            Progress1.UpdateProgress(dblPercent, ucProcess.Value & " Antasena selesai")
            btnOK.Hidden = False
        Else
            Progress1.UpdateProgress(dblPercent, ucProcess.Value & " Form " & StrListModule(intCurrentProgress) & " (" & CStr(intCurrentProgress) & " dari " & CStr(intTotalProgress) & ")")
        End If
    End Sub

    Sub StartLongAction()
        Session("LongActionProgress") = 0
        intTotalProgress = AddLists.Count
        intCurrentProgress = 0

        System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf LongAction))
        ResourceManager.GetInstance.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID)
    End Sub

    Private Sub LongAction()
        Dim i As Integer = 0
        Do While (i < 10)
            System.Threading.Thread.Sleep(1000)
            Me.Session("LongActionProgress") = (i + 1)
            i = (i + 1)
        Loop

        Me.Session.Remove("LongActionProgress")
    End Sub

    'Sub ThreadProcess()
    '    For Each StrModule As String In StrListModule()
    '        Try
    '            Dim FormSName As String = IIf(ucProcess.Value = "Archieving", StrModule, StrModule & "_Archieve")
    '            Dim FormTName As String = IIf(ucProcess.Value = "Archieving", StrModule & "_Archieve", StrModule)
    '            Thread.Sleep(400)
    '            Dim strQuery As String = "exec usp_ArchieveLHBUData_Process '" & FormSName & "', '" & FormTName & "', '" & TanggalData.ToString("yyyy-MM-dd") & "'"
    '            SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, strQuery)
    '        Catch ex As Exception
    '            'Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        End Try

    '        intCurrentProgress += 1
    '    Next
    'End Sub
    Sub ThreadProcess()
        For Each StrModule As String In StrListModule()
            Try
                Using objDb As New NawaDatadevEntities
                    Dim ModuleId As Integer = objDb.ORS_FormInfo.Where(Function(x) x.Kode = StrModule).FirstOrDefault().FK_Module_ID
                    Dim ModuleName As String = objDb.Modules.Where(Function(x) x.PK_Module_ID = ModuleId).FirstOrDefault().ModuleName

                    Dim FormSName As String = IIf(ucProcess.Value = "Archieving", ModuleName, ModuleName & "_Archieve")
                    Dim FormTName As String = IIf(ucProcess.Value = "Archieving", ModuleName & "_Archieve", ModuleName)
                    Thread.Sleep(400)
                    Dim strQuery As String = "exec usp_ArchieveLHBUData_Process '" & FormSName & "', '" & FormTName & "', '" & TanggalData.ToString("yyyy-MM-dd") & "'"
                    SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, strQuery)
                End Using
            Catch ex As Exception
                'Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            intCurrentProgress += 1
        Next
    End Sub

    Private trdValidateRecord As System.Threading.Thread

    Public Property TanggalData() As DateTime
        Get
            Return Session("ValidationProcessView.TanggalData")
        End Get
        Set(ByVal value As DateTime)
            Session("ValidationProcessView.TanggalData") = value
        End Set
    End Property

    Public Property StrListModule() As List(Of String)
        Get
            Return Session("ValidationProcessView.StrListModule")
        End Get
        Set(ByVal value As List(Of String))
            Session("ValidationProcessView.StrListModule") = value
        End Set
    End Property

    Public Property intTotalProgress() As Integer
        Get
            Return Session("ValidationProcessView.intTotalProgress")
        End Get
        Set(ByVal value As Integer)
            Session("ValidationProcessView.intTotalProgress") = value
        End Set
    End Property

    Public Property intCurrentProgress() As Integer
        Get
            Return Session("ValidationProcessView.intCurrentProgress")
        End Get
        Set(ByVal value As Integer)
            Session("ValidationProcessView.intCurrentProgress") = value
        End Set
    End Property

    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Panelconfirmation.Hidden = True
            FormPanelInput.Hidden = False
            txtStartDate.Value = ""
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Panelconfirmation.Hidden = True
            FormPanelInput.Hidden = False
            WindowProgress.Hidden = True
            txtStartDate.Value = ""
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


End Class
