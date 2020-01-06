Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports System.Data
Imports System.Data.SqlClient
Imports Ext.Net
Imports CookComputing.XmlRpc

Partial Class SLIK_ValidationProcessView
    Inherits Parent

#Region "Session"
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("ValidationProcessView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ValidationProcessView.ObjModule") = value
        End Set
    End Property
    Public Property TotalProgress As Integer
        Get
            Return Session("ValidationProcessView.TotalProgress")
        End Get
        Set(ByVal value As Integer)
            Session("ValidationProcessView.TotalProgress") = value
        End Set
    End Property
    Public Property CurrentProgress As Integer
        Get
            Return Session("ValidationProcessView.CurrentProgress")
        End Get
        Set(ByVal value As Integer)
            Session("ValidationProcessView.CurrentProgress") = value
        End Set
    End Property
    Public Property ThreadError As String
        Get
            Return Session("ValidationProcessView.ThreadError")
        End Get
        Set(ByVal value As String)
            Session("ValidationProcessView.ThreadError") = value
        End Set
    End Property
    Public Property ObjSettingPersonal As SettingPersonal
        Get
            Return Session("ValidationProcessView.ObjSettingPersonal")
        End Get
        Set(ByVal value As SettingPersonal)
            Session("ValidationProcessView.ObjSettingPersonal") = value
        End Set
    End Property
    Public Property ListFormInfo As List(Of FormInformation)
        Get
            Return Session("ValidationProcessView.ListFormInfo")
        End Get
        Set(ByVal value As List(Of FormInformation))
            Session("ValidationProcessView.ListFormInfo") = value
        End Set
    End Property
    Public Property ListSelectedForm As List(Of FormInformation)
        Get
            Return Session("ValidationProcessView.ListSelectedForm")
        End Get
        Set(ByVal value As List(Of FormInformation))
            Session("ValidationProcessView.ListSelectedForm") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim moduleEncrypt As String = Request.Params("ModuleID")
                ClearSession()

                Dim moduleID As Integer = NawaBLL.Common.DecryptQueryString(moduleEncrypt, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleID)

                If Me.ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                End If

                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If

                TreePanelForm.RootVisible = False
                FormPanelInput.Title = ObjModule.ModuleLabel
                LoadCboDataSet()
                DisableTreeButton(True)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

#End Region

#Region "Method"
    Private Sub ClearSession()
        ObjSettingPersonal = New SettingPersonal
        ListFormInfo = New List(Of FormInformation)
        ListSelectedForm = New List(Of FormInformation)
        TotalProgress = 0
        CurrentProgress = 0
        ThreadError = ""

        CboDataSet.SetValue("")
        TreePanelForm.Root.Clear()
    End Sub
    Private Sub LoadCboDataSet()
        Dim conn As New SqlConnection(NawaDAL.SQLHelper.strConnectionString)
        Dim queryString As String = "SELECT Pk_Ref_DataSet_Id ID, Ref_DataSetName Label FROM Ref_DataSet"

        Dim result As DataTable
        result = NawaDAL.SQLHelper.ExecuteTable(conn, CommandType.Text, queryString)

        CboDataSet.GetStore().DataSource = result
        CboDataSet.GetStore().DataBind()
    End Sub
    Private Sub DisableTreeButton(isDisabled As Boolean)
        BtnSelect.Disabled = isDisabled
        BtnCollapse.Disabled = isDisabled
        BtnExpand.Disabled = isDisabled
    End Sub
    Private Function GetModuleName(moduleID As String) As String
        Dim moduleName As String = ""

        Using ObjDb As New NawaDatadevEntities
            Dim objModule As NawaDevDAL.[Module] = ObjDb.Modules.Where(Function(x) x.PK_Module_ID = moduleID).FirstOrDefault
            If Not objModule Is Nothing Then
                moduleName = objModule.ModuleName
            End If
        End Using

        Return moduleName
    End Function

#Region "Get Form"
    Private Sub CreateTreeForm()
        ListFormInfo = GetFormToValidate()

        TreePanelForm.Root.Clear()
        Dim objRoot As New Node With {.Text = "Root", .NodeID = "Root"}
        TreePanelForm.Root.Add(objRoot)

        Dim objNodePrtNeraca As New Node
        objNodePrtNeraca.NodeID = "NodeParentNeraca"
        objNodePrtNeraca.Text = "Antar Form"
        objNodePrtNeraca.Expanded = True
        objNodePrtNeraca.Checked = False
        objNodePrtNeraca.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "neraca"})
        objNodePrtNeraca.CustomAttributes.Add(New ConfigItem With {.Name = "moduleID", .Value = "Validasi Neraca"})
        objRoot.Children.Add(objNodePrtNeraca)

        Dim objNodeChdNeraca As New Node
        objNodeChdNeraca.NodeID = "NodeChildNeraca"
        objNodeChdNeraca.Text = "Validasi Antar Form"
        objNodeChdNeraca.Leaf = True
        objNodeChdNeraca.Checked = False
        objNodeChdNeraca.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "neraca"})
        objNodeChdNeraca.CustomAttributes.Add(New ConfigItem With {.Name = "moduleID", .Value = "Validasi Neraca"})
        objNodePrtNeraca.Children.Add(objNodeChdNeraca)

        Dim listPeriod = (From data In ListFormInfo
                          Group By prd = New With {Key .Periode = data.Periode, Key .PeriodeStr = data.PeriodeStr} Into Group
                          Select New FormInformation With {.Periode = prd.Periode, .PeriodeStr = prd.PeriodeStr}).ToList()

        If listPeriod.Count > 0 Then
            For idxPeriod = 0 To listPeriod.Count - 1
                Dim objNodePeriod As New Node
                objNodePeriod.NodeID = "Node/" & listPeriod(idxPeriod).Periode
                objNodePeriod.Text = listPeriod(idxPeriod).PeriodeStr
                objNodePeriod.Expanded = True
                objNodePeriod.Checked = False
                objNodePeriod.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "period"})
                objNodePeriod.CustomAttributes.Add(New ConfigItem With {.Name = "period", .Value = listPeriod(idxPeriod).Periode})
                objRoot.Children.Add(objNodePeriod)

                Dim listGroup = (From data In ListFormInfo.Where(Function(x) x.Periode = listPeriod(idxPeriod).Periode).ToList()
                                 Group By ki = New With {Key .FK_KelompokInformasi = data.FK_KelompokInformasi, Key .KelompokInfoStr = data.KelompokInfoStr} Into Group
                                 Select New FormInformation With {.FK_KelompokInformasi = ki.FK_KelompokInformasi, .KelompokInfoStr = ki.KelompokInfoStr}).ToList()
                If listGroup.Count > 0 Then
                    For idxGroup = 0 To listGroup.Count - 1
                        Dim objNodeGroup As New Node
                        objNodeGroup.NodeID = objNodePeriod.NodeID + "/" + listGroup(idxGroup).FK_KelompokInformasi
                        objNodeGroup.Text = listGroup(idxGroup).KelompokInfoStr
                        objNodeGroup.Checked = False
                        objNodeGroup.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "group"})
                        objNodeGroup.CustomAttributes.Add(New ConfigItem With {.Name = "period", .Value = listPeriod(idxPeriod).Periode})
                        objNodeGroup.CustomAttributes.Add(New ConfigItem With {.Name = "groupID", .Value = listGroup(idxGroup).FK_KelompokInformasi})
                        objNodePeriod.Children.Add(objNodeGroup)

                        Dim listValidForm = ListFormInfo.Where(Function(x) x.Periode = listPeriod(idxPeriod).Periode And x.FK_KelompokInformasi = listGroup(idxGroup).FK_KelompokInformasi).ToList()
                        For idxForm = 0 To listValidForm.Count - 1
                            Dim objNodeForm As New Node
                            objNodeForm.NodeID = objNodeGroup.NodeID + "/" + listValidForm(idxForm).KodeForm
                            objNodeForm.Text = listValidForm(idxForm).KodeForm + " - " + listValidForm(idxForm).NamaForm
                            objNodeForm.Leaf = True
                            objNodeForm.Checked = False

                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "form"})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "moduleID", .Value = listValidForm(idxForm).FK_Module_ID})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "period", .Value = listPeriod(idxPeriod).Periode})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "groupID", .Value = listGroup(idxGroup).FK_KelompokInformasi})

                            objNodeGroup.Children.Add(objNodeForm)
                        Next
                    Next
                End If
            Next
        End If
    End Sub
    Private Function GetFormToValidate() As List(Of FormInformation)
        Dim listForm As New List(Of FormInformation)

        Try
            Dim queryString As String
            queryString = "SELECT fi.Kode KodeForm, fi.Nama NamaForm, fi.FK_Module_ID, " & vbCrLf &
                    "fi.FK_KelompokInformasi, ki.Label KelompokInfoStr, fi.Periode, prd.Label PeriodeStr " & vbCrLf &
                    "FROM dbo.ORS_FormInfo fi " & vbCrLf &
                    "LEFT JOIN Ref_KelompokInformasi ki " & vbCrLf &
                    "    ON ki.Sandi_Referensi = fi.FK_KelompokInformasi" & vbCrLf &
                    "LEFT JOIN Ref_Periode prd " & vbCrLf &
                    "    ON prd.Sandi_Referensi = fi.Periode" & vbCrLf &
                    "WHERE fi.[Active] = 1 AND ISNULL(fi.Periode, '') <> '' AND ISNULL(fi.FK_KelompokInformasi, 0) <> 0 " & vbCrLf &
                    " AND fi.FK_DataSet = @DataSetID " & vbCrLf &
                    " AND EXISTS(SELECT 1 FROM MUser usr INNER JOIN MGroupMenu gm ON gm.PK_MGroupMenu_ID = usr.FK_MGroupMenu_ID" & vbCrLf &
                    "           WHERE usr.UserID = @UserID AND EXISTS(SELECT 1 FROM  MGroupMenuAccess acs " & vbCrLf &
                    "        WHERE acs.FK_GroupMenu_ID = gm.PK_MGroupMenu_ID AND acs.FK_Module_ID = fi.FK_Module_ID AND acs.bView = 1)" & vbCrLf &
                    ") " & vbCrLf &
                    "ORDER BY fi.Kode "

            Dim conn As New SqlConnection(NawaDAL.SQLHelper.strConnectionString)
            Dim sqlParam(1) As SqlClient.SqlParameter

            sqlParam(0) = New SqlClient.SqlParameter
            sqlParam(0).ParameterName = "@UserID"
            sqlParam(0).Value = NawaBLL.Common.SessionCurrentUser.UserID
            sqlParam(0).SqlDbType = SqlDbType.VarChar

            sqlParam(1) = New SqlClient.SqlParameter
            sqlParam(1).ParameterName = "@DataSetID"
            sqlParam(1).Value = CboDataSet.Value
            sqlParam(1).SqlDbType = SqlDbType.VarChar

            Dim reader = NawaDAL.SQLHelper.ExecuteReader(conn, CommandType.Text, queryString, sqlParam)

            Try
                If reader.HasRows = True Then
                    While reader.Read()
                        Dim objForm As New FormInformation
                        For idx As Integer = 0 To reader.FieldCount - 1
                            If (reader.GetName(idx) = "KodeForm") Then
                                objForm.KodeForm = If(IsDBNull(reader("KodeForm")), "", reader("KodeForm"))

                            ElseIf (reader.GetName(idx) = "NamaForm") Then
                                objForm.NamaForm = If(IsDBNull(reader("NamaForm")), "", reader("NamaForm"))

                            ElseIf (reader.GetName(idx) = "FK_Module_ID") Then
                                objForm.FK_Module_ID = If(IsDBNull(reader("FK_Module_ID")), 0, reader("FK_Module_ID"))

                            ElseIf (reader.GetName(idx) = "FK_KelompokInformasi") Then
                                objForm.FK_KelompokInformasi = If(IsDBNull(reader("FK_KelompokInformasi")), 0, reader("FK_KelompokInformasi"))

                            ElseIf (reader.GetName(idx) = "KelompokInfoStr") Then
                                objForm.KelompokInfoStr = If(IsDBNull(reader("KelompokInfoStr")), "", reader("KelompokInfoStr"))

                            ElseIf (reader.GetName(idx) = "Periode") Then
                                objForm.Periode = If(IsDBNull(reader("Periode")), "", reader("Periode"))

                            ElseIf (reader.GetName(idx) = "PeriodeStr") Then
                                objForm.PeriodeStr = If(IsDBNull(reader("PeriodeStr")), "", reader("PeriodeStr"))
                            End If
                        Next

                        listForm.Add(objForm)
                    End While
                    reader.Close()
                End If
            Catch ex As Exception
                reader.Close()
                Throw New Exception(ex.Message)
            End Try

            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return listForm
    End Function
#End Region

#Region "Progress"
    Private trdValidateRecord As System.Threading.Thread
    Private Sub StartLongAction()
        Session("LongActionProgress") = 0
        CurrentProgress = 0
        TotalProgress = ListSelectedForm.Count

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
    Protected Sub RefreshProgress(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim dblPercent As Double = 0
        If TotalProgress > 0 Then
            dblPercent = CDbl(CurrentProgress) / CDbl(TotalProgress)
        End If
        If TotalProgress > 0 And CurrentProgress >= TotalProgress Then
            ResourceManager.GetInstance.AddScript("{0}.stopTask('longactionprogress');", Me.TaskManager1.ClientID)
            BtnConfirm.Hidden = (ThreadError <> "")
            LblValidateError.Hidden = (ThreadError = "")
            LblValidateError.Html = ThreadError
            Progress1.UpdateProgress(dblPercent, IIf(ThreadError = "", "Validasi selesai", "Validasi gagal"))
            ThreadError = ""
        Else
            Progress1.UpdateProgress(dblPercent, "Validasi Forms " & CStr(CurrentProgress) & " dari " & CStr(TotalProgress))
        End If
    End Sub
    Private Sub UpdateProgress(ByVal intCurrentProgress As Integer, ByVal intTotal As Integer)
        Dim lngPercent As Long = 0
        If intTotal > 0 Then
            lngPercent = CLng(intCurrentProgress) / CLng(intTotal)
        End If
        Progress1.UpdateProgress(lngPercent, "Validasi Forms " & CStr(intCurrentProgress) & " dari " & CStr(intTotal))
    End Sub
    Private Sub ThreadProcess()
        Dim formName As String = ""
        Try
            For Each objForm As FormInformation In ListSelectedForm
                formName = objForm.NamaForm
                If objForm.KodeForm = "FormNeraca" Then
                    Dim strQuery As String = "exec Usp_ExecuteValidationInterform '" & ObjSettingPersonal.ReportDate.ToString("yyyy-MM-dd") & "', '" & ObjSettingPersonal.KodeCabang & "'"
                    SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, strQuery)
                Else
                    'System.Threading.Thread.Sleep(1000)
                    Dim moduleName As String = GetModuleName(objForm.FK_Module_ID)
                    Dim strQuery As String = "exec usp_ExecuteValidationBySegmentData '" & ObjSettingPersonal.ReportDate.ToString("yyyy-MM-dd") & "', '" & ObjSettingPersonal.KodeCabang & "', '" & moduleName & "'"
                    SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, strQuery)
                End If

                Dim query As String = "exec usp_GenerateTableForReporting 1,'" & ObjSettingPersonal.ReportDate.ToString("yyyy-MM-dd") & "'"
                SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, query)

                CurrentProgress += 1
            Next
        Catch ex As Exception
            CurrentProgress = TotalProgress
            ThreadError = "An error occured when validating " & formName & ". <br><b>Error: " & ex.Message & "</b>"
        End Try
    End Sub

#End Region

#End Region

#Region "Direct Events"
    Protected Sub CboDataSet_OnSelected()
        Dim isEmpty As Boolean = (String.IsNullOrEmpty(CboDataSet.Value))
        TbFilter.SetText("")
        DisableTreeButton(isEmpty)
        ListSelectedForm = New List(Of FormInformation)

        Try
            If isEmpty Then
                TreePanelForm.Root.Clear()
            Else
                CreateTreeForm()
            End If

            TreePanelForm.Render()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub TbFilter_OnChanged()
        If Not String.IsNullOrEmpty(CboDataSet.Value) Then
            BtnExpand.Disabled = (Not String.IsNullOrEmpty(TbFilter.Text))
            BtnCollapse.Disabled = (Not String.IsNullOrEmpty(TbFilter.Text))
        End If
    End Sub
    Protected Sub BtnSelect_OnClick()
        ListSelectedForm = New List(Of FormInformation)
        CeSelectAll.SetValue(Not CeSelectAll.Checked)

        If CeSelectAll.Checked Then
            TreePanelForm.SetAllChecked()
            ListSelectedForm.Add(New FormInformation With {.KodeForm = "FormNeraca", .NamaForm = "Form Neraca"})
            ListSelectedForm.AddRange(ListFormInfo)
        Else
            TreePanelForm.ClearChecked()
        End If

        BtnSelect.Text = If(CeSelectAll.Checked, "Unselect All", "Select All")
        BtnSelect.Icon = If(CeSelectAll.Checked, Icon.Cross, Icon.Tick)
    End Sub
    Protected Sub BtnExpand_OnClick()
        TreePanelForm.ExpandAll()
    End Sub
    Protected Sub BtnCollapse_OnClick()
        TreePanelForm.CollapseAll()
    End Sub
    Protected Sub BtnGenerate_OnClicked()
        Try
            If String.IsNullOrEmpty(CboDataSet.Value) Then
                CboDataSet.SetActiveError(CboDataSet.BlankText)
                Throw New ApplicationException("Data Set must be defined")
            ElseIf ListSelectedForm.Count = 0 Then
                Throw New ApplicationException("Select Form to Validate")
            End If

            Using ObjDb As New NawaDatadevEntities
                ObjSettingPersonal = ObjDb.SettingPersonals.Where(Function(x) x.UserID = NawaBLL.Common.SessionCurrentUser.UserID).FirstOrDefault
                If ObjSettingPersonal Is Nothing Then
                    Throw New ApplicationException("Period & Branch in Setting must be defined")
                End If
            End Using

            WindowProgress.Hidden = False
            BtnConfirm.Hidden = True
            LblValidateError.Hidden = True
            LblValidateError.Html = ""
            ThreadError = ""

            StartLongAction()

            trdValidateRecord = New System.Threading.Thread(AddressOf ThreadProcess)
            trdValidateRecord.IsBackground = True
            trdValidateRecord.Start()

            Dim proxy As ServicesForWeb
            Dim Successrun As Boolean = False

            '    proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            '    proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            '    Dim Bulan As String = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.Bulan
            '    Dim Tahun As String = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.Tahun
            '    Dim ModuleName() As String = GetSelectedForm().ToArray
            '    If ModuleName.Length = 0 Then
            '        Throw New Exception("Select Module to validate")
            '    End If
            '    Dim kodecabang As String = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang

            '    Successrun = proxy.ValidateRecordsFromWeb(Bulan, ModuleName, Tahun, kodecabang)

            '    Dim intmoduleID As Integer = 0
            '    Dim directory As String = ""

            '    Using objdb As New NawaDAL.NawaDataEntities
            '        intmoduleID = (From x In objdb.Modules Where x.ModuleName = "ValidationReport" Select x).FirstOrDefault.PK_Module_ID
            '        directory = (From x In objdb.Modules Where x.ModuleName = "ValidationReport" Select x).FirstOrDefault.UrlView
            '    End Using
            '    Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

            '    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
        Catch appEx As ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirm_OnClicked()
        Dim intmoduleID As Integer = 0
        Dim directory As String = ""

        Using objdb As New NawaDAL.NawaDataEntities
            intmoduleID = (From x In objdb.Modules Where x.ModuleName = "ValidationSummary" Select x).FirstOrDefault.PK_Module_ID
            directory = (From x In objdb.Modules Where x.ModuleName = "ValidationSummary" Select x).FirstOrDefault.UrlView
        End Using
        Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
    End Sub
    Protected Sub TreePanelItem_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            Dim nodeID As String = e.ExtraParams("NodeID").ToString
            Dim level As String = e.ExtraParams("Level").ToString
            Dim isChecked As Boolean = e.ExtraParams("IsChecked")

            TreePanelForm.GetNodeById(nodeID).SetChecked(isChecked)
            'TreePanelForm.GetNodeById(nodeID).ChildNodes().SetChecked(isChecked)

            Select Case level
                Case "neraca"
                    TreePanelForm.GetNodeById(nodeID).ChildNodes().SetChecked(isChecked)
                    ListSelectedForm.RemoveAll(Function(x) x.KodeForm = "FormNeraca")
                    If isChecked Then
                        ListSelectedForm.Add(New FormInformation With {.KodeForm = "FormNeraca", .NamaForm = "Form Neraca"})
                    End If

                Case "period"
                    TreePanelForm.GetNodeById(nodeID).EachChild(New JFunction("onCheckTreeParent"))
                    TreePanelForm.GetNodeById(nodeID).ChildNodes().EachChild(New JFunction("onCheckTreeParent"))
                    Dim period As String = e.ExtraParams("Period").ToString

                    Dim listCurrent As New List(Of FormInformation)
                    listCurrent.AddRange(ListFormInfo.Where(Function(x) x.Periode = period))

                    If Not String.IsNullOrEmpty(TbFilter.Text.Trim) Then
                        listCurrent.RemoveAll(Function(x) (x.KodeForm & " " & x.NamaForm).ToLower().IndexOf(TbFilter.Text.Trim) < 0)
                    End If

                    ListSelectedForm.RemoveAll(Function(x) listCurrent.Any(Function(y) x.Periode = y.Periode And x.FK_Module_ID = y.FK_Module_ID))
                    If isChecked Then
                        ListSelectedForm.AddRange(listCurrent)
                    End If

                Case "group"
                    TreePanelForm.GetNodeById(nodeID).EachChild(New JFunction("onCheckTreeParent"))
                    Dim period As String = e.ExtraParams("Period").ToString
                    Dim groupID As String = e.ExtraParams("GroupID").ToString

                    Dim listCurrent As New List(Of FormInformation)
                    listCurrent.AddRange(ListFormInfo.Where(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID))

                    If Not String.IsNullOrEmpty(TbFilter.Text.Trim) Then
                        listCurrent.RemoveAll(Function(x) (x.KodeForm & " " & x.NamaForm).ToLower().IndexOf(TbFilter.Text.Trim) < 0)
                    End If

                    ListSelectedForm.RemoveAll(Function(x) listCurrent.Any(Function(y) x.FK_KelompokInformasi = y.FK_KelompokInformasi And x.FK_Module_ID = y.FK_Module_ID))
                    If isChecked Then
                        ListSelectedForm.AddRange(listCurrent)
                    End If

                Case "form"
                    Dim period As String = e.ExtraParams("Period").ToString
                    Dim groupID As String = e.ExtraParams("GroupID").ToString
                    Dim moduleID As String = e.ExtraParams("ModuleID").ToString
                    ListSelectedForm.RemoveAll(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID And x.FK_Module_ID = moduleID)

                    If isChecked Then
                        ListSelectedForm.AddRange(ListFormInfo.Where(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID And x.FK_Module_ID = moduleID))
                    End If
            End Select

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Custom Variable [Untuk tampung data Form]"
    Public Class FormInformation
        Private kodeValue As String
        Private namaValue As String
        Private moduleValue As Integer
        Private kelompokInfoValue As String
        Private kelompokInfoStrValue As String
        Private periodeValue As String
        Private periodeStrValue As String

        Sub New()
            kodeValue = ""
            namaValue = ""
            moduleValue = 0
            kelompokInfoValue = ""
            kelompokInfoStrValue = ""
            periodeValue = ""
            periodeStrValue = ""
        End Sub
        Public Property KodeForm As String
            Get
                Return kodeValue
            End Get
            Set(value As String)
                kodeValue = value
            End Set
        End Property
        Public Property NamaForm As String
            Get
                Return namaValue
            End Get
            Set(value As String)
                namaValue = value
            End Set
        End Property
        Public Property FK_Module_ID As Integer
            Get
                Return moduleValue
            End Get
            Set(value As Integer)
                moduleValue = value
            End Set
        End Property
        Public Property FK_KelompokInformasi As String
            Get
                Return kelompokInfoValue
            End Get
            Set(value As String)
                kelompokInfoValue = value
            End Set
        End Property
        Public Property KelompokInfoStr As String
            Get
                Return kelompokInfoStrValue
            End Get
            Set(value As String)
                kelompokInfoStrValue = value
            End Set
        End Property
        Public Property Periode As String
            Get
                Return periodeValue
            End Get
            Set(value As String)
                periodeValue = value
            End Set
        End Property
        Public Property PeriodeStr As String
            Get
                Return periodeStrValue
            End Get
            Set(value As String)
                periodeStrValue = value
            End Set
        End Property
    End Class
#End Region

End Class


