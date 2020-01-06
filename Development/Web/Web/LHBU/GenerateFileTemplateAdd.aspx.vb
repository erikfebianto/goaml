Imports NawaDevDAL
Imports System.Data.SqlClient
Imports System.Data
Partial Class GenerateFileTemplateAdd
    Inherits Parent
    Public objGenerateFileTemplateBLL As New NawaDevBLL.FileGenerationBLL()


    Public Property objGenerateFileTemplateAdditionalEdit() As NawaDevDAL.GenerateFileTemplateAdditional
        Get
            Return Session("GenerateFileTemplateAdd.objGenerateFileTemplateAdditionalEdit")
        End Get
        Set(ByVal value As NawaDevDAL.GenerateFileTemplateAdditional)
            Session("GenerateFileTemplateAdd.objGenerateFileTemplateAdditionalEdit") = value
        End Set
    End Property

    Public Property objListGenerateFileTemplateAdditionalAdd() As List(Of NawaDevDAL.GenerateFileTemplateAdditional)
        Get
            If Session("GenerateFileTemplateAdd.objListGenerateFileTemplateAdditionalAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDevDAL.GenerateFileTemplateAdditional)

                Session("GenerateFileTemplateAdd.objListGenerateFileTemplateAdditionalAdd") = oNewScheduler

            End If
            Return Session("GenerateFileTemplateAdd.objListGenerateFileTemplateAdditionalAdd")
        End Get
        Set(ByVal value As List(Of NawaDevDAL.GenerateFileTemplateAdditional))
            Session("GenerateFileTemplateAdd.objListGenerateFileTemplateAdditionalAdd") = value
        End Set
    End Property


    Public Property objGenerateFileTemplateDetailEdit() As NawaDevDAL.GenerateFileTemplateDetail
        Get
            Return Session("GenerateFileTemplateAdd.objGenerateFileTemplateDetailEdit")
        End Get
        Set(ByVal value As NawaDevDAL.GenerateFileTemplateDetail)
            Session("GenerateFileTemplateAdd.objGenerateFileTemplateDetailEdit") = value
        End Set
    End Property

    Public Property objListGenerateFileTemplateDetailAdd() As List(Of NawaDevDAL.GenerateFileTemplateDetail)
        Get
            If Session("GenerateFileTemplateAdd.objListGenerateFileTemplateDetailAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDevDAL.GenerateFileTemplateDetail)

                Session("GenerateFileTemplateAdd.objListGenerateFileTemplateDetailAdd") = oNewScheduler

            End If
            Return Session("GenerateFileTemplateAdd.objListGenerateFileTemplateDetailAdd")
        End Get
        Set(ByVal value As List(Of NawaDevDAL.GenerateFileTemplateDetail))
            Session("GenerateFileTemplateAdd.objListGenerateFileTemplateDetailAdd") = value
        End Set
    End Property

    Public Property objGenerateFileTemplateAddData() As NawaDevDAL.GenerateFileTemplate
        Get
            If Session("GenerateFileTemplateAdd.objGenerateFileTemplateAddData") Is Nothing Then
                Session("GenerateFileTemplateAdd.objGenerateFileTemplateAddData") = New NawaDevDAL.GenerateFileTemplate
            End If
            Return Session("GenerateFileTemplateAdd.objGenerateFileTemplateAddData")
        End Get
        Set(ByVal value As NawaDevDAL.GenerateFileTemplate)
            Session("GenerateFileTemplateAdd.objGenerateFileTemplateAddData") = value
        End Set
    End Property
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("GenerateFileTemplateAdd.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("GenerateFileTemplateAdd.ObjModule") = value
        End Set
    End Property
    Private Sub Load_Init(sender As Object, e As EventArgs) Handles Me.Init
        objGenerateFileTemplateBLL = New NawaDevBLL.FileGenerationBLL()
    End Sub
    Sub ClearSession()
        objGenerateFileTemplateAddData = Nothing
        ObjModule = Nothing
        objGenerateFileTemplateDetailEdit = Nothing
        objListGenerateFileTemplateDetailAdd = Nothing
        objGenerateFileTemplateAdditionalEdit = Nothing
        objListGenerateFileTemplateAdditionalAdd = Nothing
    End Sub
    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()

                Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                If ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                Else
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Add"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Add"

                    LoadSegmenData()
                    LoadDelivMethod()
                End If
            End If

            'objEodTask.BentukformAdd()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub LoadSegmenData()
        Dim querystring As String = "SELECT Kode +' - '+ nama DisplayField,oksd.Kode DisplayValue FROM ORS_FormInfo AS oksd where FK_Module_ID is not null"

        Dim objdt As System.Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, querystring, Nothing)

        StoreSegmenData.DataSource = objdt
        StoreSegmenData.DataBind()
    End Sub
    Sub LoadDelivMethod()
        Dim queryString As String = "SELECT PK_ID, MethodType FROM ORS_Ref_DeliveryMethod"
        Dim listData As System.Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, queryString, Nothing)

        CbDelivMethod.GetStore().DataSource = listData
        CbDelivMethod.GetStore().DataBind()
    End Sub
    Sub ClearinputAdditional()
        txtTableName.Text = ""
        txtquery.Text = ""
        hQueryObjectDesigner.Value = ""
        txtFieldUnik.Text = ""
        txtFieldUnik.Hidden = True

        cbTanggalData.Text = ""
        cbKodeCabang.Text = ""
        cbRecordId.Text = ""
    End Sub

    Sub Clearinput()

        txtRepalcer.Text = ""
        cboField.ClearValue()


    End Sub
    Sub BindGridGenerateFileTemplate()
        StoreEmailDetail.DataSource = objListGenerateFileTemplateDetailAdd
        StoreEmailDetail.DataBind()
    End Sub

    Sub SaveAddGenerateFileTemplateAdditional()
        'done: code SaveAddGenerateFileTemplateAdditional



        Dim objrand As New Random
        Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
        While Not objListGenerateFileTemplateAdditionalAdd.Find(Function(x) x.PK_GenerateFileTemplateAdditional_ID = intpk) Is Nothing
            intpk = objrand.Next(Integer.MinValue, -1)
        End While

        Dim objnewGenerateFileTemplateAdditional As New GenerateFileTemplateAdditional
        With objnewGenerateFileTemplateAdditional
            .PK_GenerateFileTemplateAdditional_ID = intpk
            .FK_GenerationFileTemplate_ID = 0
            .SQLQuery = (txtquery.Text)
            .Alias = txtTableName.Text.Trim
            .OutputFormat = txtOutputFormat.Text.Trim
            .TableSource = txtTableSource.Text.Trim
            .TanggalData = cbTanggalData.SelectedItem.Value
            .KodeCabang = cbKodeCabang.SelectedItem.Value
            .RecordId = cbRecordId.SelectedItem.Value
        End With

        objListGenerateFileTemplateAdditionalAdd.Add(objnewGenerateFileTemplateAdditional)

        BindGridGenerateFileTemplateAdditonal()
        FormPanelAdditional.Hidden = True
        WindowAdditional.Hidden = True
        ClearinputAdditional()

    End Sub
    Sub BindGridGenerateFileTemplateAdditonal()
        'done : code BindGridGenerateFileTemplateAdditonal

        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListGenerateFileTemplateAdditionalAdd)

        StoreAdditional.DataSource = objdt
        StoreAdditional.DataBind()
    End Sub


    Sub SaveEditGenerateFileTemplateAdditional()
        'done: code SaveEditGenerateFileTemplateAdditional
        With objGenerateFileTemplateAdditionalEdit
            .SQLQuery = (txtquery.Text)
            .Alias = txtTableName.Text.Trim
            .OutputFormat = txtOutputFormat.Text.Trim
            .TableSource = txtTableSource.Text.Trim
            .TanggalData = cbTanggalData.SelectedItem.Value
            .KodeCabang = cbKodeCabang.SelectedItem.Value
            .RecordId = cbRecordId.SelectedItem.Value
        End With
        BindGridGenerateFileTemplateAdditonal()
        objGenerateFileTemplateAdditionalEdit = Nothing
        FormPanelAdditional.Hidden = True
        WindowAdditional.Hidden = True
        ClearinputAdditional()
    End Sub
    Sub SaveAddGenerateFileTemplateDetail()

        Dim objrand As New Random
        Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
        While Not objListGenerateFileTemplateDetailAdd.Find(Function(x) x.PK_GenerateFileTemplateDetail_ID = intpk) Is Nothing
            intpk = objrand.Next(Integer.MinValue, -1)
        End While

        Dim objnewGenerateFileTemplateDetail As New NawaDevDAL.GenerateFileTemplateDetail


        objnewGenerateFileTemplateDetail.PK_GenerateFileTemplateDetail_ID = intpk
        objnewGenerateFileTemplateDetail.FK_GenerationFileTemplate_ID = 0
        objnewGenerateFileTemplateDetail.Replacer = txtRepalcer.Text
        objnewGenerateFileTemplateDetail.FieldReplacer = cboField.Value
        'objnewGenerateFileTemplateDetail.QueryObjectDesigner = hQueryObjectDesigner.Value


        objListGenerateFileTemplateDetailAdd.Add(objnewGenerateFileTemplateDetail)

        BindGridGenerateFileTemplate()
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()


    End Sub
    Sub SaveEditGenerateFileTemplateDetail()
        With objGenerateFileTemplateDetailEdit
            .Replacer = txtRepalcer.Text
            .FieldReplacer = cboField.Value

        End With
        BindGridGenerateFileTemplate()
        objGenerateFileTemplateDetailEdit = Nothing
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()


    End Sub


    Function IsDataAddValidAdditional() As Boolean

        If txtTableName.Text = "" Or txtTableName Is Nothing Then
            Throw New Exception("Please Fill Alias")
        End If
        If txtquery.Text = "" Or txtquery Is Nothing Then
            Throw New Exception("Please Fill SQL Query")
        End If
        Return True
    End Function

    Protected Sub btnsaveAdditional_DirectEvent(sender As Object, e As DirectEventArgs)
        'done:code btnsaveAdditional_DirectEvent
        Try

            If IsDataAddValidAdditional() Then


                If Me.objGenerateFileTemplateAdditionalEdit Is Nothing Then
                    SaveAddGenerateFileTemplateAdditional()
                Else
                    SaveEditGenerateFileTemplateAdditional()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Protected Sub BtnCancelAdditional_DirectEvent(sender As Object, e As DirectEventArgs)
        'done:code BtnCancelAdditional_DirectEvent

        Try
            FormPanelAdditional.Hidden = True
            WindowAdditional.Hidden = True
            ClearinputAdditional()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
    Sub LoadComboFieldReff()
        Try
            'done: LoadComboField
            cbTanggalData.GetStore.RemoveAll()
            cbKodeCabang.GetStore.RemoveAll()
            cbRecordId.GetStore.RemoveAll()

            'Dim dt As Data.DataTable = NawaBLL.Nawa.BLL.NawaFramework.GetListFieldByQuery(Server.HtmlDecode(item.SQLQuery))
            If txtquery.Text = "" Or txtquery.Text Is Nothing Then
                Throw New Exception("Please Fill SQL Query")
                Exit Sub
            End If
            If txtTableName.Text = "" Or txtTableName.Text Is Nothing Then
                Throw New Exception("Please Fill Table Name")
                Exit Sub
            End If

            'Dim dt As List(Of NawaDevBLL.TableProperties) = NawaDevBLL.TablePropertiesDataBLL.GetColumnProperties(Server.HtmlDecode(txtquery.Text.Trim), txtTableName.Text.Trim)
            Dim dt As List(Of NawaDevBLL.TableProperties) = NawaDevBLL.TablePropertiesDataBLL.GetColumnProperties((txtquery.Text.Trim), txtTableName.Text.Trim)

            For Each ds As NawaDevBLL.TableProperties In dt
                Dim strfield As String = "[" + ds.TableName + "].[" & ds.FieldName & "]"
                cbTanggalData.AddItem(strfield, strfield)
                cbKodeCabang.AddItem(strfield, strfield)
                cbRecordId.AddItem(strfield, strfield)
            Next
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Sub LoadComboField()
        'done: LoadComboField
        cboField.GetStore.RemoveAll()


        'For Each item As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditionalAdd
        '    Dim dt As Data.DataTable = NawaBLL.Nawa.BLL.NawaFramework.GetListFieldByQuery(Server.HtmlDecode(item.SQLQuery))
        '    For Each item1 As Data.DataColumn In dt.Columns
        '        Dim strfield As String = "[" + item.Alias + "].[" & item1.ColumnName & "]"
        '        cboField.AddItem(strfield, strfield)
        '    Next
        'Next

        For Each item As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditionalAdd
            'Dim dt As Data.DataTable = NawaBLL.Nawa.BLL.NawaFramework.GetListFieldByQuery(Server.HtmlDecode(item.SQLQuery))
            'Dim dt As List(Of NawaDevBLL.TableProperties) = NawaDevBLL.TablePropertiesDataBLL.GetColumnProperties(Server.HtmlDecode(item.SQLQuery), item.Alias)
            Dim dt As List(Of NawaDevBLL.TableProperties) = NawaDevBLL.TablePropertiesDataBLL.GetColumnProperties((item.SQLQuery), item.Alias)

            For Each ds As NawaDevBLL.TableProperties In dt
                Dim strfield As String = "[" + ds.TableName + "].[" & ds.FieldName & "]"
                cboField.AddItem(strfield, strfield)
            Next


        Next



    End Sub

    Protected Sub btnAddAdditionalTable_DirectClick(sender As Object, e As DirectEventArgs)
        'done:coding additional table

        Try
            ClearinputAdditional()

            FormPanelAdditional.Hidden = False
            WindowAdditional.Hidden = False
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub BtnCancelReplacer_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            FormSchedulerDetail.Hidden = True
            WindowDetail.Hidden = True
            Clearinput()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnSaveReplacer_DirectEvent(sender As Object, e As DirectEventArgs)
        Try

            If Me.objGenerateFileTemplateDetailEdit Is Nothing Then
                SaveAddGenerateFileTemplateDetail()
            Else
                SaveEditGenerateFileTemplateDetail()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub ClearReplacer()
        txtRepalcer.Text = ""
        cboField.Text = ""
    End Sub
    Protected Sub btnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
        Try

            LoadComboField()
            ClearReplacer()

            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Sub LoadDataEdit(id As Long)
        LoadComboField()
        objGenerateFileTemplateDetailEdit = objListGenerateFileTemplateDetailAdd.Find(Function(x) x.PK_GenerateFileTemplateDetail_ID = id)

        If Not objGenerateFileTemplateDetailEdit Is Nothing Then
            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
            Clearinput()
            With objGenerateFileTemplateDetailEdit
                txtRepalcer.Text = .Replacer
                cboField.Value = .FieldReplacer


            End With
        End If
    End Sub

    Sub DeleteRecordTaskDetail(id As Long)
        BtnCancelReplacer_DirectEvent(Nothing, Nothing)

        Dim objdel As NawaDevDAL.GenerateFileTemplateDetail = objListGenerateFileTemplateDetailAdd.Find(Function(x) x.PK_GenerateFileTemplateDetail_ID = id)
        If Not objdel Is Nothing Then
            objListGenerateFileTemplateDetailAdd.Remove(objdel)
            Dim intcounter As Integer = 0
            BindGridGenerateFileTemplate()

        End If
    End Sub
    Sub LoadDataEditAdditional(id As Long)
        'done: code LoadDataEditAdditional



        objGenerateFileTemplateAdditionalEdit = objListGenerateFileTemplateAdditionalAdd.Find(Function(x) x.PK_GenerateFileTemplateAdditional_ID = id)

        If Not objGenerateFileTemplateAdditionalEdit Is Nothing Then
            FormPanelAdditional.Hidden = False
            WindowAdditional.Hidden = False
            ClearinputAdditional()
            With objGenerateFileTemplateAdditionalEdit
                'txtquery.Text = Server.HtmlDecode(.SQLQuery)
                txtquery.Text = (.SQLQuery)
                'hQueryObjectDesigner.Value = .QueryDataDesigner
                txtTableName.Text = .Alias
                'cboTableType.Value = .FK_EmailTableType_ID
                txtOutputFormat.Text = .OutputFormat
                txtTableSource.Text = .TableSource
                txtFieldUnik.Text = .UnikField
                cbTanggalData.SetValueAndFireSelect(.TanggalData)
                cbKodeCabang.SetValueAndFireSelect(.KodeCabang)
                cbRecordId.SetValueAndFireSelect(.RecordId)
            End With
        End If
    End Sub
    Sub DeleteRecordAdditonal(id As Long)
        'done: code  DeleteRecordAdditonal
        BtnCancelAdditional_DirectEvent(Nothing, Nothing)

        Dim objdel As NawaDevDAL.GenerateFileTemplateAdditional = objListGenerateFileTemplateAdditionalAdd.Find(Function(x) x.PK_GenerateFileTemplateAdditional_ID = id)
        If Not objdel Is Nothing Then
            objListGenerateFileTemplateAdditionalAdd.Remove(objdel)
            Dim intcounter As Integer = 0
            BindGridGenerateFileTemplateAdditonal()

        End If

    End Sub

    Protected Sub GridCommandAdditional(sender As Object, e As Ext.Net.DirectEventArgs)
        'done:codinggridcommandadditional
        Try
            Dim id As String = e.ExtraParams(0).Value
            If e.ExtraParams(1).Value = "Edit" Then
                LoadDataEditAdditional(id)
            ElseIf e.ExtraParams(1).Value = "Delete" Then
                DeleteRecordAdditonal(id)


            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim id As String = e.ExtraParams(0).Value
            If e.ExtraParams(1).Value = "Edit" Then
                LoadDataEdit(id)
            ElseIf e.ExtraParams(1).Value = "Delete" Then
                DeleteRecordTaskDetail(id)


            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Public Function IsDataAddValid()
        Try
            If NawaDevBLL.FileGenerationBLL.IsGenerateFileTemplateNameAlreadyExist(txtGenerateFileTemplate.Text.Trim) Then
                Throw New Exception("Generate File Template Name " & txtGenerateFileTemplate.Text.Trim & " already exists.")
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then

                With objGenerateFileTemplateAddData
                    .PK_GenerationFileTemplate_ID = 0
                    .GenerateFileTemplateName = txtGenerateFileTemplate.Text.Trim
                    .OutputFormat = txtBody.Text.Trim
                    .FileNameFormat = txtFileNameFormat.Text.Trim
                    .LHBUFormName = CbOJKSegmenData.SelectedItem.Value
                    .FK_DelivMethod_ID = CbDelivMethod.SelectedItem.Value
                    .Active = True
                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .CreatedDate = DateTime.Now
                    .LastUpdateDate = DateTime.Now
                End With


                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                    objGenerateFileTemplateBLL.SaveAddTanpaApproval(objGenerateFileTemplateAddData, objListGenerateFileTemplateAdditionalAdd, objListGenerateFileTemplateDetailAdd, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else
                    objGenerateFileTemplateBLL.SaveAddApproval(objGenerateFileTemplateAddData, objListGenerateFileTemplateAdditionalAdd, objListGenerateFileTemplateDetailAdd, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Pending Approval"
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub



    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


End Class
