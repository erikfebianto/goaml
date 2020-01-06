Imports NawaDevDAL
Imports NawaDevBLL
Imports CookComputing.XmlRpc
Imports System.Data
Imports System.Data.SqlClient

Partial Class LHBU_GenerateTextFileView
    Inherits Parent

#Region "Session"
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("GenerateTextFileView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("GenerateTextFileView.ObjModule") = value
        End Set
    End Property
    Public Property ListTemplate As List(Of GenerateFileTemplate)
        Get
            Return Session("GenerateTextFileView.ListTemplate")
        End Get
        Set(ByVal value As List(Of GenerateFileTemplate))
            Session("GenerateTextFileView.ListTemplate") = value
        End Set
    End Property
    Public Property ListCategory As List(Of GenerateFileCategory)
        Get
            Return Session("GenerateTextFileView.ListCategory")
        End Get
        Set(ByVal value As List(Of GenerateFileCategory))
            Session("GenerateTextFileView.ListCategory") = value
        End Set
    End Property
    Public Property ListSelectedCategory As List(Of GenerateFileCategory)
        Get
            Return Session("GenerateTextFileView.ListSelectedCategory")
        End Get
        Set(ByVal value As List(Of GenerateFileCategory))
            Session("GenerateTextFileView.ListSelectedCategory") = value
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
                Else
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If
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
        ObjModule = Nothing
        ListTemplate = New List(Of GenerateFileTemplate)
        ListCategory = New List(Of GenerateFileCategory)
        ListSelectedCategory = New List(Of GenerateFileCategory)
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
    Private Sub GetTemplateByCategory()
        ListTemplate = New List(Of GenerateFileTemplate)
        Dim categoryStr As String = ""

        For Each itemCategory In ListSelectedCategory
            If categoryStr <> "" Then
                categoryStr = categoryStr + ", "
            End If
            categoryStr = categoryStr + "'" + itemCategory.FormCategoryCode + "'"
        Next

        Using objDB As New NawaDatadevEntities
            Dim queryString As String
            queryString = "SELECT gft.PK_GenerationFileTemplate_ID, gft.GenerateFileTemplateName, gft.LHBUFormName FROM GenerateFileTemplate gft " & vbCrLf &
                        " INNER JOIN ORS_FormInfo fi On fi.Kode = gft.LHBUFormName And fi.[Active] = 1 " & vbCrLf &
                        " WHERE EXISTS(SELECT 1 FROM MUser usr INNER JOIN MGroupMenu gm ON gm.PK_MGroupMenu_ID = usr.FK_MGroupMenu_ID " & vbCrLf &
                           " WHERE usr.UserID = '" & NawaBLL.Common.SessionCurrentUser.UserID & "' AND EXISTS(SELECT 1 FROM  MGroupMenuAccess acs " & vbCrLf &
                                " WHERE acs.FK_GroupMenu_ID = gm.PK_MGroupMenu_ID AND acs.FK_Module_ID = fi.FK_Module_ID AND acs.bView = 1) ) " & vbCrLf &
                           " AND EXISTS(SELECT 1 FROM dbo.ORS_FormCategoryMapping fcm WHERE fcm.FormCode = gft.LHBUFormName " & vbCrLf &
                                " AND fcm.FormCategoryCode IN (" & categoryStr & ") )"

            Dim templateRecords As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryString)
            For Each itemTemplate As DataRow In templateRecords.Rows
                ListTemplate.Add(New GenerateFileTemplate With {
                    .PK_GenerationFileTemplate_ID = itemTemplate("PK_GenerationFileTemplate_ID"),
                    .GenerateFileTemplateName = itemTemplate("GenerateFileTemplateName"),
                    .LHBUFormName = itemTemplate("LHBUFormName")
                })
            Next
        End Using
    End Sub
    Private Function CheckValidation() As String
        Dim listValidation As New List(Of String)
        Dim kodeCabang As String = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang
        Dim tanggalData As Date = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.ReportDate

        Dim objValidationOR As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9009)
        Dim objValidationAntasena As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9010)

        If objValidationOR Is Nothing Then
            objValidationOR = New NawaDAL.SystemParameter With {.SettingValue = "true"}
        End If

        If objValidationAntasena Is Nothing Then
            objValidationAntasena = New NawaDAL.SystemParameter With {.SettingValue = "true"}
        End If

        Dim isValidateOneReporting As Boolean = (objValidationOR.SettingValue.ToLower() = "false")
        Dim isValidateBIAntasena As Boolean = (objValidationAntasena.SettingValue.ToLower() = "false")

        If isValidateOneReporting Or isValidateBIAntasena Then
            Using objDb As New NawaDatadevEntities
                Dim listForm As List(Of String) = ListTemplate.Select(Function(x) x.LHBUFormName).Distinct().ToList()
                For Each itemForm In listForm
                    Dim listCabang As New List(Of String)
                    If kodeCabang = "All" Then
                        Dim paramQueryCabang(1) As SqlClient.SqlParameter
                        paramQueryCabang(0) = New SqlClient.SqlParameter
                        paramQueryCabang(0).ParameterName = "@KodeForm"
                        paramQueryCabang(0).SqlDbType = SqlDbType.VarChar
                        paramQueryCabang(0).Value = itemForm

                        paramQueryCabang(1) = New SqlClient.SqlParameter
                        paramQueryCabang(1).ParameterName = "@TanggalData"
                        paramQueryCabang(1).SqlDbType = SqlDbType.VarChar
                        paramQueryCabang(1).Value = tanggalData.ToString("yyyyMMdd")

                        Dim cabangRecords As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetDistinctCabangForm", paramQueryCabang)
                        For Each itemCabang As DataRow In cabangRecords.Rows
                            listCabang.Add(itemCabang("Kode_Cabang_BI"))
                        Next
                    Else
                        listCabang.Add(kodeCabang)
                    End If

                    For Each itemCabang In listCabang
                        Dim queryValidate As String =
                        "SELECT fi.Nama, ISNULL(fsr.Status_Validasi, '''') Status_Validasi, ISNULL(fsr.Jumlah_Data_Tidak_Valid, 0) InvalidTotal FROM ORS_formInfo fi " & vbCrLf &
                        "LEFT JOIN ORS_FormCategoryMapping fcm ON fi.Kode = fcm.FormCode " & vbCrLf &
                        "LEFT JOIN ORS_FormStatusReport fsr ON fsr.Form_Category = fcm.FormCategoryCode " & vbCrLf &
                            "AND fsr.Report_Date = '" & tanggalData.ToString("yyyyMMdd") & "' AND fsr.Kode_Cabang_BI = '" & itemCabang & "' " & vbCrLf &
                        "WHERE fcm.FormCode = '" & itemForm & "' "

                        Dim validateRecords As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryValidate)
                        For Each itemInvalid As DataRow In validateRecords.Rows
                            If itemInvalid("Status_Validasi").ToString <> "4" Then 'Sukses validasi BI Antasena
                                Dim errorMsg As String = "<b>" & itemInvalid("Nama") & "</b> - "
                                Dim errorDetail As String = ""

                                If isValidateOneReporting Then
                                    Select Case itemInvalid("Status_Validasi").ToString
                                        Case "1" 'Belum valid OneReporting
                                            errorDetail = "There are " & itemInvalid("InvalidTotal") & " invalid record(s) validated by OneReporting"
                                        Case "2", "3" 'Sukses validasi OneReporting
                                            errorDetail = ""
                                        Case Else
                                            errorDetail = "Not yet validated by OneReporting"
                                    End Select
                                End If

                                If isValidateBIAntasena And String.IsNullOrEmpty(errorDetail) Then
                                    Select Case itemInvalid("Status_Validasi").ToString
                                        Case "3" 'Belum valid BI Antasena
                                            errorDetail = "There are " & itemInvalid("InvalidTotal") & " invalid record(s) validated by BI Antasena"
                                        Case Else
                                            errorDetail = "Not yet validated by BI Antasena"
                                    End Select

                                End If

                                If kodeCabang = "All" And String.IsNullOrEmpty(errorDetail) Then
                                    errorDetail += " (Branch: " & itemCabang & ")"
                                End If

                                If Not String.IsNullOrEmpty(errorDetail) Then
                                    errorMsg += errorDetail
                                    listValidation.Add(errorMsg)
                                End If
                            End If
                        Next
                    Next
                Next
            End Using
        End If

        If listValidation.Count > 0 Then
            Return "The following form(s) still have error validation: <br>" & String.Join("<br>", listValidation.Select(Function(x) "* " + x).ToList())
        Else
            Return ""
        End If
    End Function
    Private Function GetSelectedTemplate() As List(Of String)
        Dim listForm As New List(Of String)

        For Each item In ListTemplate
            listForm.Add(item.PK_GenerationFileTemplate_ID.ToString)
        Next

        Return listForm
    End Function
    Private Sub Generate()
        Dim proxy As ServicesForWeb
        Dim Successrun As Boolean = False

        Dim validation = CheckValidation()
        If Not String.IsNullOrEmpty(validation) Then
            Throw New Exception(validation)
        Else
            'proxy = CType(XmlRpcProxyGen.Create(GetType(ServicesForWeb)), ServicesForWeb)
            'proxy.Url = System.Configuration.ConfigurationManager.AppSettings("ProxyURLPath")

            'Dim objSettingTimeOut As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9014)
            'If objSettingTimeOut Is Nothing Then
            '    objSettingTimeOut = New NawaDAL.SystemParameter With {.SettingValue = "3600"}
            'End If

            'proxy.Timeout = Convert.ToInt32(objSettingTimeOut.SettingValue)

            Dim userName As String = NawaBLL.Common.SessionCurrentUser.UserID
            Dim kodeCabang As String = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang
            Dim tanggalData As Date = NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.ReportDate
            Dim arrTemplate() As String = GetSelectedTemplate().ToArray

            Dim listtemplate As String = String.Join(",", arrTemplate)
            'Successrun = proxy.GenerateTextFileFromWeb(tanggalData, kodeCabang, userName, arrTemplate)


            NawaDevBLL.FileGenerationBLL.GenerateTextFile(tanggalData, kodeCabang, userName, listtemplate)


            Dim objModuleView As NawaDAL.[Module]
            Using objDB As New NawaDAL.NawaDataEntities
                objModuleView = objDB.Modules.ToList.FirstOrDefault(Function(x) x.ModuleName = "FileGenerationList")
            End Using

            Dim moduleID As String = NawaBLL.Common.EncryptQueryString(objModuleView.PK_Module_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objModuleView.UrlView & "?ModuleID={0}", moduleID), "Loading...")
        End If
    End Sub





#Region "Get Form"
    Private Sub CreateTreeForm()
        ListCategory = GetFormToGenerate()

        TreePanelForm.Root.Clear()
        Dim objRoot As New Node With {.Text = "Root", .NodeID = "Root"}
        TreePanelForm.Root.Add(objRoot)

        Dim listPeriod = (From data In ListCategory
                          Group By prd = New With {Key .Periode = data.Periode, Key .PeriodeStr = data.PeriodeStr} Into Group
                          Select New GenerateFileCategory With {.Periode = prd.Periode, .PeriodeStr = prd.PeriodeStr}).ToList()

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

                Dim listGroup = (From data In ListCategory.Where(Function(x) x.Periode = listPeriod(idxPeriod).Periode).ToList()
                                 Group By ki = New With {Key .FK_KelompokInformasi = data.FK_KelompokInformasi, Key .KelompokInfoStr = data.KelompokInfoStr} Into Group
                                 Select New GenerateFileCategory With {.FK_KelompokInformasi = ki.FK_KelompokInformasi, .KelompokInfoStr = ki.KelompokInfoStr}).ToList()
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

                        Dim listValidForm = ListCategory.Where(Function(x) x.Periode = listPeriod(idxPeriod).Periode And x.FK_KelompokInformasi = listGroup(idxGroup).FK_KelompokInformasi).ToList()
                        For idxForm = 0 To listValidForm.Count - 1
                            Dim objNodeForm As New Node
                            objNodeForm.NodeID = objNodeGroup.NodeID + "/" + listValidForm(idxForm).FormCategoryCode.ToString
                            objNodeForm.Text = "(" + listValidForm(idxForm).FormCategoryCode + ") - " + listValidForm(idxForm).FormCategoryName
                            objNodeForm.Leaf = True
                            objNodeForm.Checked = False

                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "level", .Value = "category"})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "category", .Value = listValidForm(idxForm).FormCategoryCode})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "period", .Value = listPeriod(idxPeriod).Periode})
                            objNodeForm.CustomAttributes.Add(New ConfigItem With {.Name = "groupID", .Value = listGroup(idxGroup).FK_KelompokInformasi})

                            objNodeGroup.Children.Add(objNodeForm)
                        Next
                    Next
                End If
            Next
        End If
    End Sub
    Private Function GetFormToGenerate() As List(Of GenerateFileCategory)
        Dim listForm As New List(Of GenerateFileCategory)

        Try
            Dim queryString As String
            'Kondisi Lama group per Template
            'queryString = "SELECT gft.PK_GenerationFileTemplate_ID, gft.GenerateFileTemplateName, gft.LHBUFormName, fi.FK_Module_ID, " & vbCrLf &
            '                " fi.FK_KelompokInformasi, ki.Label KelompokInfoStr, fi.Periode, prd.Label PeriodeStr " & vbCrLf &
            '            " FROM GenerateFileTemplate gft " & vbCrLf &
            '            " INNER JOIN ORS_FormInfo fi ON gft.LHBUFormName = fi.Kode " & vbCrLf &
            '            " INNER JOIN Ref_KelompokInformasi ki ON ki.Sandi_Referensi = fi.FK_KelompokInformasi " & vbCrLf &
            '            " INNER JOIN Ref_Periode prd ON prd.Sandi_Referensi = fi.Periode " & vbCrLf &
            '            " WHERE fi.FK_DataSet = @DataSetID " & vbCrLf &
            '            " AND fi.[Active] = 1 AND EXISTS(SELECT 1 FROM MUser usr INNER JOIN MGroupMenu gm ON gm.PK_MGroupMenu_ID = usr.FK_MGroupMenu_ID " & vbCrLf &
            '                      " WHERE usr.UserID = @UserID AND EXISTS(SELECT 1 FROM  MGroupMenuAccess acs " & vbCrLf &
            '                   " WHERE acs.FK_GroupMenu_ID = gm.PK_MGroupMenu_ID AND acs.FK_Module_ID = fi.FK_Module_ID AND acs.bView = 1) " & vbCrLf &
            '            " ) " & vbCrLf &
            '            " ORDER BY gft.GenerateFileTemplateName "

            queryString = "SELECT DISTINCT fc.FormCategoryCode, fc.FormCategoryName, fi.FK_KelompokInformasi, ki.Label KelompokInfoStr, fi.Periode, prd.Label PeriodeStr " & vbCrLf &
                        " FROM ORS_FormCategory fc " & vbCrLf &
                        " INNER JOIN ORS_FormCategoryMapping fcm ON fcm.FormCategoryCode = fc.FormCategoryCode " & vbCrLf &
                        " INNER JOIN ORS_FormInfo fi ON fcm.FormCode = fi.Kode " & vbCrLf &
                        " INNER JOIN Ref_KelompokInformasi ki ON ki.Sandi_Referensi = fi.FK_KelompokInformasi " & vbCrLf &
                        " INNER JOIN Ref_Periode prd ON prd.Sandi_Referensi = fi.Periode " & vbCrLf &
                        " WHERE fi.FK_DataSet = @DataSetID AND fi.[Active] = 1 " & vbCrLf &
                            " AND EXISTS(SELECT 1 FROM MUser usr INNER JOIN MGroupMenu gm ON gm.PK_MGroupMenu_ID = usr.FK_MGroupMenu_ID " & vbCrLf &
                                " WHERE usr.UserID = @UserID AND EXISTS(SELECT 1 FROM  MGroupMenuAccess acs " & vbCrLf &
                                    " WHERE acs.FK_GroupMenu_ID = gm.PK_MGroupMenu_ID And acs.FK_Module_ID = fi.FK_Module_ID And acs.bView = 1) ) " & vbCrLf &
                        " ORDER BY fc.FormCategoryCode "

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
                        Dim objForm As New GenerateFileCategory
                        For idx As Integer = 0 To reader.FieldCount - 1
                            If (reader.GetName(idx) = "FormCategoryCode") Then
                                objForm.FormCategoryCode = If(IsDBNull(reader("FormCategoryCode")), "", reader("FormCategoryCode"))

                            ElseIf (reader.GetName(idx) = "FormCategoryName") Then
                                objForm.FormCategoryName = If(IsDBNull(reader("FormCategoryName")), "", reader("FormCategoryName"))

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

#End Region

#Region "Direct Events"
    Protected Sub CboDataSet_OnSelected()
        Dim isEmpty As Boolean = (String.IsNullOrEmpty(CboDataSet.Value))
        TbFilter.SetText("")
        DisableTreeButton(isEmpty)
        ListSelectedCategory = New List(Of GenerateFileCategory)

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
        ListSelectedCategory = New List(Of GenerateFileCategory)
        CeSelectAll.SetValue(Not CeSelectAll.Checked)

        If CeSelectAll.Checked Then
            TreePanelForm.SetAllChecked()
            ListSelectedCategory.AddRange(ListCategory)
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
            Try
                If String.IsNullOrEmpty(CboDataSet.Value) Then
                    CboDataSet.SetActiveError(CboDataSet.BlankText)
                    Throw New ApplicationException("Data Set must be defined")
                ElseIf ListSelectedCategory.Count = 0 Then
                    Throw New ApplicationException("Category must be selected")
                ElseIf NawaDevBLL.SLIKParameterBLL.getSettingSLIKPersonal(NawaBLL.Common.SessionCurrentUser.UserID) Is Nothing Then
                    Throw New ApplicationException("Period & Branch in Setting must be defined")
                End If
            Catch ex As Exception When TypeOf ex Is ApplicationException
                Ext.Net.X.Msg.Alert("Information", ex.Message).Show()
            End Try

            GetTemplateByCategory()
            Generate()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub TreePanelItem_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            Dim nodeID As String = e.ExtraParams("NodeID").ToString
            Dim level As String = e.ExtraParams("Level").ToString
            Dim isChecked As Boolean = e.ExtraParams("IsChecked")

            TreePanelForm.GetNodeById(nodeID).SetChecked(isChecked)
            'TreePanelForm.GetNodeById(nodeID).ChildNodes().SetChecked(isChecked)

            Select Case level
                Case "period"
                    TreePanelForm.GetNodeById(nodeID).EachChild(New JFunction("onCheckTreeParent"))
                    TreePanelForm.GetNodeById(nodeID).ChildNodes().EachChild(New JFunction("onCheckTreeParent"))
                    Dim period As String = e.ExtraParams("Period").ToString

                    Dim listCurrent As New List(Of GenerateFileCategory)
                    listCurrent.AddRange(ListCategory.Where(Function(x) x.Periode = period))

                    If Not String.IsNullOrEmpty(TbFilter.Text.Trim) Then
                        listCurrent.RemoveAll(Function(x) ("(" & x.FormCategoryCode & ") - " & x.FormCategoryName).ToLower().IndexOf(TbFilter.Text.Trim) < 0)
                    End If

                    ListSelectedCategory.RemoveAll(Function(x) listCurrent.Any(Function(y) x.Periode = y.Periode And x.FormCategoryCode = y.FormCategoryCode))
                    If isChecked Then
                        ListSelectedCategory.AddRange(listCurrent)
                    End If

                Case "group"
                    TreePanelForm.GetNodeById(nodeID).EachChild(New JFunction("onCheckTreeParent"))
                    Dim period As String = e.ExtraParams("Period").ToString
                    Dim groupID As String = e.ExtraParams("GroupID").ToString

                    Dim listCurrent As New List(Of GenerateFileCategory)
                    listCurrent.AddRange(ListCategory.Where(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID))

                    If Not String.IsNullOrEmpty(TbFilter.Text.Trim) Then
                        listCurrent.RemoveAll(Function(x) ("(" & x.FormCategoryCode & ") - " & x.FormCategoryName).ToLower().IndexOf(TbFilter.Text.Trim) < 0)
                    End If

                    ListSelectedCategory.RemoveAll(Function(x) listCurrent.Any(Function(y) x.FK_KelompokInformasi = y.FK_KelompokInformasi And x.FormCategoryCode = y.FormCategoryCode))
                    If isChecked Then
                        ListSelectedCategory.AddRange(listCurrent)
                    End If

                Case "category"
                    Dim period As String = e.ExtraParams("Period").ToString
                    Dim groupID As String = e.ExtraParams("GroupID").ToString
                    Dim category As String = e.ExtraParams("Category").ToString
                    ListSelectedCategory.RemoveAll(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID And x.FormCategoryCode = category)

                    If isChecked Then
                        ListSelectedCategory.AddRange(ListCategory.Where(Function(x) x.Periode = period And x.FK_KelompokInformasi = groupID And x.FormCategoryCode = category))
                    End If
            End Select

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Custom Variable [Untuk tampung data Category]"
    Public Class GenerateFileCategory
        Private ctgCodeValue As String
        Private ctgNameValue As String
        Private kelompokInfoValue As String
        Private kelompokInfoStrValue As String
        Private periodeValue As String
        Private periodeStrValue As String

        Sub New()
            ctgCodeValue = ""
            ctgNameValue = ""
            kelompokInfoValue = ""
            kelompokInfoStrValue = ""
            periodeValue = ""
            periodeStrValue = ""
        End Sub

        Public Property FormCategoryCode As String
            Get
                Return ctgCodeValue
            End Get
            Set(value As String)
                ctgCodeValue = value
            End Set
        End Property
        Public Property FormCategoryName As String
            Get
                Return ctgNameValue
            End Get
            Set(value As String)
                ctgNameValue = value
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
