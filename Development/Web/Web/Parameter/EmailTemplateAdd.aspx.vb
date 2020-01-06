Imports NawaDAL

Partial Class EmailTemplateAdd
    Inherits Parent
    Public objEmailTemplateBLL As NawaBLL.EmailTemplateBLL

    Public Property objEmailTemplateAttachmentedit() As NawaDAL.EmailTemplateAttachment
        Get
            Return Session("EmailTemplateAdd.objEmailTemplateActionEdit")
        End Get
        Set(ByVal value As NawaDAL.EmailTemplateAttachment)
            Session("EmailTemplateAdd.objEmailTemplateActionEdit") = value
        End Set
    End Property

    Public Property objEmailTemplateActionEdit() As NawaDAL.EmailTemplateAction
        Get
            Return Session("EmailTemplateAdd.objEmailTemplateActionEdit")
        End Get
        Set(ByVal value As NawaDAL.EmailTemplateAction)
            Session("EmailTemplateAdd.objEmailTemplateActionEdit") = value
        End Set
    End Property

    Public Property objEmailTemplateAdditionalEdit() As NawaDAL.EmailTemplateAdditional
        Get
            Return Session("EmailTemplateAdd.objEmailTemplateAdditionalEdit")
        End Get
        Set(ByVal value As NawaDAL.EmailTemplateAdditional)
            Session("EmailTemplateAdd.objEmailTemplateAdditionalEdit") = value
        End Set
    End Property

    Public Property objListEmailTemplateActionAdd() As List(Of NawaDAL.EmailTemplateAction)
        Get
            If Session("EmailTemplateAdd.objListEmailTemplateActionAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDAL.EmailTemplateAction)

                Session("EmailTemplateAdd.objListEmailTemplateActionAdd") = oNewScheduler

            End If
            Return Session("EmailTemplateAdd.objListEmailTemplateActionAdd")
        End Get
        Set(ByVal value As List(Of NawaDAL.EmailTemplateAction))
            Session("EmailTemplateAdd.objListEmailTemplateActionAdd") = value
        End Set
    End Property

    Public Property objListEmailTemplateAdditionalAdd() As List(Of NawaDAL.EmailTemplateAdditional)
        Get
            If Session("EmailTemplateAdd.objListEmailTemplateAdditionalAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDAL.EmailTemplateAdditional)

                Session("EmailTemplateAdd.objListEmailTemplateAdditionalAdd") = oNewScheduler

            End If
            Return Session("EmailTemplateAdd.objListEmailTemplateAdditionalAdd")
        End Get
        Set(ByVal value As List(Of NawaDAL.EmailTemplateAdditional))
            Session("EmailTemplateAdd.objListEmailTemplateAdditionalAdd") = value
        End Set
    End Property

    Public Property objEmailTemplateDetailEdit() As NawaDAL.EmailTemplateDetail
        Get
            Return Session("EmailTemplateAdd.objEmailTemplateDetailEdit")
        End Get
        Set(ByVal value As NawaDAL.EmailTemplateDetail)
            Session("EmailTemplateAdd.objEmailTemplateDetailEdit") = value
        End Set
    End Property

    Public Property objListEmailTemplateAttachmentAdd() As List(Of NawaDAL.EmailTemplateAttachment)
        Get
            If Session("EmailTemplateAdd.objListEmailTemplateAttachmentAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDAL.EmailTemplateAttachment)

                Session("EmailTemplateAdd.objListEmailTemplateAttachmentAdd") = oNewScheduler

            End If
            Return Session("EmailTemplateAdd.objListEmailTemplateAttachmentAdd")
        End Get
        Set(ByVal value As List(Of NawaDAL.EmailTemplateAttachment))
            Session("EmailTemplateAdd.objListEmailTemplateAttachmentAdd") = value
        End Set
    End Property

    Public Property objListEmailTemplateDetailAdd() As List(Of NawaDAL.EmailTemplateDetail)
        Get
            If Session("EmailTemplateAdd.objListEmailTemplateDetailAdd") Is Nothing Then
                Dim oNewScheduler As New List(Of NawaDAL.EmailTemplateDetail)

                Session("EmailTemplateAdd.objListEmailTemplateDetailAdd") = oNewScheduler

            End If
            Return Session("EmailTemplateAdd.objListEmailTemplateDetailAdd")
        End Get
        Set(ByVal value As List(Of NawaDAL.EmailTemplateDetail))
            Session("EmailTemplateAdd.objListEmailTemplateDetailAdd") = value
        End Set
    End Property

    Public Property objEmailTemplateAddData() As NawaDAL.EmailTemplate
        Get
            If Session("EmailTemplateAdd.objEmailTemplateAddData") Is Nothing Then
                Session("EmailTemplateAdd.objEmailTemplateAddData") = New NawaDAL.EmailTemplate
            End If
            Return Session("EmailTemplateAdd.objEmailTemplateAddData")
        End Get
        Set(ByVal value As NawaDAL.EmailTemplate)
            Session("EmailTemplateAdd.objEmailTemplateAddData") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("EmailTemplateAdd.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EmailTemplateAdd.ObjModule") = value
        End Set
    End Property

    Private Sub Load_Init(sender As Object, e As EventArgs) Handles Me.Init
        objEmailTemplateBLL = New NawaBLL.EmailTemplateBLL()
    End Sub

    Sub ClearSession()
        objEmailTemplateAddData = Nothing
        ObjModule = Nothing
        objEmailTemplateDetailEdit = Nothing
        objListEmailTemplateDetailAdd = Nothing
        objEmailTemplateAdditionalEdit = Nothing
        objListEmailTemplateAdditionalAdd = Nothing
        objListEmailTemplateAttachmentAdd = Nothing
        objListEmailTemplateActionAdd = Nothing
        objEmailTemplateActionEdit = Nothing
        objEmailTemplateAttachmentedit = Nothing
    End Sub

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()
                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Add"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Add"
                    startDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    NawaBLL.Nawa.BLL.NawaFramework.GetPicker(CboMonitoringDuration, "SELECT PK_MonitoringDuration_Id as ID,MonitoringDurationName as Name FROM MonitoringDuration", True, True)
                    LoadTableType()
                    Loadactiontype()
                    LoadEmailAttachmentType()
                    LoadStoreRenderAs()

                    Dim intmaxfilesize As Double = NawaBLL.SystemParameterBLL.GetMaxFileSize
                    Dim strmaxfilesize As String = (intmaxfilesize / 1048576) & " MB"
                    FileReport.Listeners.Change.Handler = "#{LblFileReport}.setValue(#{FileReport}.value);if(!UpdateUploadInfo(this.button.fileInputEl.dom," & intmaxfilesize & ",'" & strmaxfilesize & "')) {this.reset();};  "

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


    Sub Loadactiontype()
        StoreEmailActionType.DataSource = NawaBLL.EmailTemplateBLL.GetEmailActionTypeAll
        StoreEmailActionType.DataBind()
    End Sub

    Sub LoadTableType()
        StoreTableType.DataSource = NawaBLL.EmailTemplateBLL.GetListEmailTableType
        StoreTableType.DataBind()
    End Sub

    Sub ClearinputAdditional()
        txtTableName.Text = ""
        txtquery.Text = ""
        hQueryObjectDesigner.Value = ""
        txtFieldUnik.Text = ""
        txtFieldUnik.Hidden = True
        cboTableType.ClearValue()
    End Sub

    Sub Clearinput()

        txtRepalcer.Text = ""
        cboField.ClearValue()

    End Sub

    Sub BindGridEmailTemplate()
        StoreEmailDetail.DataSource = objListEmailTemplateDetailAdd
        StoreEmailDetail.DataBind()
    End Sub

    Sub SaveAddEmailTemplateAdditional()
        'done: code SaveAddEmailTemplateAdditional

        Dim objrand As New Random
        Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
        While Not objListEmailTemplateAdditionalAdd.Find(Function(x) x.PK_EmailTemplateAdditional_ID = intpk) Is Nothing
            intpk = objrand.Next(Integer.MinValue, -1)
        End While

        Dim objnewEmailTemplateAdditional As New EmailTemplateAdditional

        objnewEmailTemplateAdditional.PK_EmailTemplateAdditional_ID = intpk
        objnewEmailTemplateAdditional.FK_EmailTemplate_ID = 0
        objnewEmailTemplateAdditional.QueryData = Server.HtmlEncode(txtquery.Text)
        objnewEmailTemplateAdditional.QueryDataDesigner = hQueryObjectDesigner.Value
        objnewEmailTemplateAdditional.NamaTable = txtTableName.Text.Trim
        objnewEmailTemplateAdditional.FK_EmailTableType_ID = CInt(cboTableType.Value)
        objnewEmailTemplateAdditional.FieldUnikPrimaryTable = txtFieldUnik.Text

        'objnewEmailTemplateDetail.QueryObjectDesigner = hQueryObjectDesigner.Value

        objListEmailTemplateAdditionalAdd.Add(objnewEmailTemplateAdditional)

        BindGridEmailTemplateAdditonal()
        FormPanelAdditional.Hidden = True
        WindowAdditional.Hidden = True
        ClearinputAdditional()

    End Sub

    Sub BindGridEmailTemplateAdditonal()
        'done : code BindGridEmailTemplateAdditonal

        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListEmailTemplateAdditionalAdd)
        Dim objcol As New Data.DataColumn
        objcol.ColumnName = "EmailTableTypeName"
        objcol.DataType = GetType(String)
        objdt.Columns.Add(objcol)

        Dim objtabletype As List(Of NawaDAL.EmailTableType) = NawaBLL.EmailTemplateBLL.GetListEmailTableType

        For Each item As Data.DataRow In objdt.Rows
            Dim objcek As NawaDAL.EmailTableType = objtabletype.Find(Function(x) x.PK_EmailTableType_ID = item("FK_EmailTableType_ID"))
            If objcek Is Nothing Then
                item("EmailTableTypeName") = ""
            Else
                item("EmailTableTypeName") = objcek.EmailTableTypeName
            End If

        Next
        StoreAdditional.DataSource = objdt
        StoreAdditional.DataBind()
    End Sub

    Sub SaveEditEmailTemplateAdditional()
        'done: code SaveEditEmailTemplateAdditional
        With objEmailTemplateAdditionalEdit
            .QueryData = Server.HtmlEncode(txtquery.Text)
            .QueryDataDesigner = hQueryObjectDesigner.Text
            .NamaTable = txtTableName.Text.Trim
            .FK_EmailTableType_ID = CInt(cboTableType.Value)
            .FieldUnikPrimaryTable = txtFieldUnik.Text.Trim
        End With
        BindGridEmailTemplateAdditonal()
        objEmailTemplateAdditionalEdit = Nothing
        FormPanelAdditional.Hidden = True
        WindowAdditional.Hidden = True
        ClearinputAdditional()
    End Sub

    Sub SaveAddEmailTemplateDetail()

        Dim objrand As New Random
        Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
        While Not objListEmailTemplateDetailAdd.Find(Function(x) x.PK_EmailTemplateDetail_ID = intpk) Is Nothing
            intpk = objrand.Next(Integer.MinValue, -1)
        End While

        Dim objnewEmailTemplateDetail As New NawaDAL.EmailTemplateDetail

        objnewEmailTemplateDetail.PK_EmailTemplateDetail_ID = intpk
        objnewEmailTemplateDetail.FK_EmailTemplate_ID = 0
        objnewEmailTemplateDetail.Replacer = txtRepalcer.Text
        objnewEmailTemplateDetail.FieldReplacer = cboField.Value
        'objnewEmailTemplateDetail.QueryObjectDesigner = hQueryObjectDesigner.Value

        objListEmailTemplateDetailAdd.Add(objnewEmailTemplateDetail)

        BindGridEmailTemplate()
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()

    End Sub

    Sub SaveEditEmailTemplateDetail()
        With objEmailTemplateDetailEdit
            .Replacer = txtRepalcer.Text
            .FieldReplacer = cboField.Value

        End With
        BindGridEmailTemplate()
        objEmailTemplateDetailEdit = Nothing
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()

    End Sub

    Protected Sub cboTableType_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            'done: code cboTableType_DirectEvent
            If cboTableType.Value = "1" Then
                txtFieldUnik.Hidden = False
            Else
                txtFieldUnik.Hidden = True
                txtFieldUnik.Text = ""
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Function IsDataAddValidAdditional() As Boolean
        If cboTableType.Value = "1" Then
            If txtFieldUnik.Text.Trim = "" Then
                Throw New Exception("Field Unik Name is required if Table Type is Primary Table. ")
            End If
        End If
        Return True
    End Function

    Protected Sub btnsaveAdditional_DirectEvent(sender As Object, e As DirectEventArgs)
        'done:code btnsaveAdditional_DirectEvent
        Try

            If IsDataAddValidAdditional() Then

                If Me.objEmailTemplateAdditionalEdit Is Nothing Then
                    SaveAddEmailTemplateAdditional()
                Else
                    SaveEditEmailTemplateAdditional()
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

    Sub LoadComboField()
        'done: LoadComboField
        cboField.GetStore.RemoveAll()

        For Each item As NawaDAL.EmailTemplateAdditional In objListEmailTemplateAdditionalAdd
            Dim dt As Data.DataTable = NawaBLL.Nawa.BLL.NawaFramework.GetListFieldByQuery(Server.HtmlDecode(item.QueryData))
            For Each item1 As Data.DataColumn In dt.Columns
                Dim strfield As String = "[" + item.NamaTable + "].[" & item1.ColumnName & "]"
                cboField.AddItem(strfield, strfield)
            Next

        Next

    End Sub

    Sub clearinputEmailAttachment()

        ComboBoxEmailAttachmentType.Clear()
        TxtReportName.Text = ""
        TxtReportParameter.Text = ""
        FileReport.Clear()
        txtFileName.Text = ""
    End Sub

    Protected Sub BtnCancelAttachment_Click(sender As Object, e As DirectEventArgs)
        Try
            'done:BtnCancelAttachment_Click

            WindowEmailAttachment.Hidden = True
            clearinputEmailAttachment()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub SaveEmailtemplateAttachmentAdd()
        'done:SaveEmailtemplateAttachmentAdd
        Dim objnewEmailTemplateAttachmentAdd As New NawaDAL.EmailTemplateAttachment
        With objnewEmailTemplateAttachmentAdd

            Dim objrand As New Random
            Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
            While Not objListEmailTemplateAttachmentAdd.Find(Function(x) x.PK_EmailTemplateAttachment_ID = intpk) Is Nothing
                intpk = objrand.Next(Integer.MinValue, -1)
            End While

            .PK_EmailTemplateAttachment_ID = intpk
            .FK_EmailTemplate_ID = 0
            .FK_EmailAttachmentType_ID = ComboBoxEmailAttachmentType.SelectedItem.Value

            If ComboBoxEmailAttachmentType.SelectedItem.Value = 1 Then
                'tipe file
                .NamaFile = FileReport.FileName
                .IsiFile = FileReport.FileBytes
                .ParameterReport = ""
                .FK_EmailRenderAs_Id = Nothing
                .NamaReport = ""

            Else
                'tipe reporting service
                .NamaReport = TxtReportName.Text.Trim
                .ParameterReport = TxtReportParameter.Text.Trim
                .FK_EmailRenderAs_Id = cboRenderAs.SelectedItem.Value
                .NamaFile = txtFileName.Text
                .IsiFile = Nothing

            End If

            objListEmailTemplateAttachmentAdd.Add(objnewEmailTemplateAttachmentAdd)
        End With
        BindGridAttachment()
        WindowEmailAttachment.Hidden = True
        clearinputEmailAttachment()

    End Sub

    Sub SaveEmailtemplateAttachmentEdit()
        'todohendra:SaveEmailtemplateAttachmentEdit

        With objEmailTemplateAttachmentedit
            .FK_EmailAttachmentType_ID = ComboBoxEmailAttachmentType.SelectedItem.Value

            If ComboBoxEmailAttachmentType.SelectedItem.Value = 1 Then
                'tipe file
                .NamaFile = FileReport.FileName
                .IsiFile = FileReport.FileBytes
                .ParameterReport = ""
                .FK_EmailRenderAs_Id = Nothing
                .NamaReport = ""
            Else
                'tipe reporting service
                .ParameterReport = TxtReportParameter.Text.Trim
                .FK_EmailRenderAs_Id = cboRenderAs.SelectedItem.Value
                .NamaFile = txtFileName.Text
                .IsiFile = Nothing
                .NamaReport = TxtReportName.Text.Trim
            End If
        End With
        BindGridAttachment()
        WindowEmailAttachment.Hidden = True
        clearinputEmailAttachment()
    End Sub

    Function isdataAtachmentValid() As Integer
        If ComboBoxEmailAttachmentType.SelectedItem.Value = 1 Then
            If Not FileReport.HasFile Then
                Throw New Exception("File Report Is Required")
            End If

        ElseIf ComboBoxEmailAttachmentType.SelectedItem.Value = 2 Then
            If cboRenderAs.SelectedItem.Value = "" Then
                Throw New Exception("Render As Is Required")
            End If

        End If
        Return True
    End Function

    Protected Sub btnSubmitAttachment_DirectEvent(sender As Object, e As DirectEventArgs)
        Try

            If isdataAtachmentValid() Then
                If objEmailTemplateAttachmentedit Is Nothing Then
                    SaveEmailtemplateAttachmentAdd()
                Else
                    SaveEmailtemplateAttachmentEdit()
                End If
                BindGridAttachment()
                clearinputEmailAttachment()
                WindowEmailAttachment.Hidden = True
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnAddEmailAttachment_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            'done:btnAddEmailAttachment_DirectClick
            WindowEmailAttachment.Hidden = False
            clearinputEmailAttachment()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
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

            If Me.objEmailTemplateDetailEdit Is Nothing Then
                SaveAddEmailTemplateDetail()
            Else
                SaveEditEmailTemplateDetail()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
        Try

            LoadComboField()

            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Sub LoadDataEdit(id As Long)
        LoadComboField()
        objEmailTemplateDetailEdit = objListEmailTemplateDetailAdd.Find(Function(x) x.PK_EmailTemplateDetail_ID = id)

        If Not objEmailTemplateDetailEdit Is Nothing Then
            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
            Clearinput()
            With objEmailTemplateDetailEdit
                txtRepalcer.Text = .Replacer
                cboField.Value = .FieldReplacer

            End With
        End If
    End Sub

    Sub DeleteRecordTaskDetail(id As Long)
        BtnCancelReplacer_DirectEvent(Nothing, Nothing)

        Dim objdel As NawaDAL.EmailTemplateDetail = objListEmailTemplateDetailAdd.Find(Function(x) x.PK_EmailTemplateDetail_ID = id)
        If Not objdel Is Nothing Then
            objListEmailTemplateDetailAdd.Remove(objdel)
            Dim intcounter As Integer = 0
            BindGridEmailTemplate()

        End If
    End Sub

    Sub LoadDataEditAdditional(id As Long)
        'done: code LoadDataEditAdditional

        objEmailTemplateAdditionalEdit = objListEmailTemplateAdditionalAdd.Find(Function(x) x.PK_EmailTemplateAdditional_ID = id)

        If Not objEmailTemplateAdditionalEdit Is Nothing Then
            FormPanelAdditional.Hidden = False
            WindowAdditional.Hidden = False
            ClearinputAdditional()
            With objEmailTemplateAdditionalEdit
                txtquery.Text = Server.HtmlDecode(.QueryData)
                hQueryObjectDesigner.Value = .QueryDataDesigner
                txtTableName.Text = .NamaTable
                cboTableType.Value = .FK_EmailTableType_ID
                txtFieldUnik.Text = .FieldUnikPrimaryTable
            End With
        End If
    End Sub

    Sub DeleteRecordAdditonal(id As Long)
        'done: code  DeleteRecordAdditonal
        BtnCancelAdditional_DirectEvent(Nothing, Nothing)

        Dim objdel As NawaDAL.EmailTemplateAdditional = objListEmailTemplateAdditionalAdd.Find(Function(x) x.PK_EmailTemplateAdditional_ID = id)
        If Not objdel Is Nothing Then
            objListEmailTemplateAdditionalAdd.Remove(objdel)
            Dim intcounter As Integer = 0
            BindGridEmailTemplateAdditonal()

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

    Sub LoadDataEditAction(id As Integer)
        'done:LoadDataEditAction
        objEmailTemplateActionEdit = objListEmailTemplateActionAdd.Find(Function(x) x.PK_EmailTemplate_Action_ID = id)
        If Not objEmailTemplateActionEdit Is Nothing Then
            WindowEmailAction.Hidden = False
            clearinputEmailAction()
            cboEmailAction.SetValue(objEmailTemplateActionEdit.FK_EmailActionType_ID)
            TxtsqlToExecute.Text = objEmailTemplateActionEdit.TSQLtoExecute
        End If

    End Sub

    Sub LoadEmailAttachmentType()
        'done:LoadEmailAttachmentType
        StorecboEmailAttachmentType.DataSource = NawaBLL.EmailTemplateBLL.GetEMailAttachmentType
        StorecboEmailAttachmentType.DataBind()
    End Sub

    Sub DeleteRecordAction(id As Integer)
        'done:DeleteRecordAction
        BtnCancelAction_Click(Nothing, Nothing)
        Dim objDel As NawaDAL.EmailTemplateAction = objListEmailTemplateActionAdd.Find(Function(x) x.PK_EmailTemplate_Action_ID = id)
        If Not objDel Is Nothing Then
            objListEmailTemplateActionAdd.Remove(objDel)
            BindGridAction()
        End If
    End Sub

    Sub LoadStoreRenderAs()
        StoreRenderAs.DataSource = NawaBLL.EmailTemplateBLL.GetStoreRenderAs
        StoreRenderAs.DataBind()
    End Sub

    Sub BindGridAttachment()
        'todohendra:BindGridAttachment
        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListEmailTemplateAttachmentAdd)
        Dim objcol As New Data.DataColumn
        objcol.ColumnName = "EmailAttachmentType"
        objcol.DataType = GetType(String)
        objdt.Columns.Add(objcol)

        Dim objcol1 As New Data.DataColumn
        objcol1.ColumnName = "EmailRenderAsName"
        objcol1.DataType = GetType(String)
        objdt.Columns.Add(objcol1)

        Dim objattachment As List(Of NawaDAL.EmailAttachmentType) = NawaBLL.EmailTemplateBLL.GetEMailAttachmentType
        Dim objRender As List(Of NawaDAL.EmailRenderA) = NawaBLL.EmailTemplateBLL.GetStoreRenderAs

        For Each item As Data.DataRow In objdt.Rows
            Dim objcek As NawaDAL.EmailAttachmentType = objattachment.Find(Function(x) x.PK_EmailAttachmentType_ID = item("FK_EmailAttachmentType_ID"))

            If objcek Is Nothing Then
                item("EmailAttachmentType") = ""
            Else
                item("EmailAttachmentType") = objcek.EmailAttachmentType1
            End If

            If item("FK_EmailRenderAs_Id").ToString <> "" Then
                Dim objcek1 As NawaDAL.EmailRenderA = objRender.Find(Function(x) x.PK_EmailRenderAs_ID = item("FK_EmailRenderAs_Id"))
                If objcek1 Is Nothing Then
                    item("EmailRenderAsName") = ""
                Else
                    item("EmailRenderAsName") = objcek1.EmailRenderAsName
                End If
            End If

        Next

        StoreEmailAttachment.DataSource = objdt
        StoreEmailAttachment.DataBind()

    End Sub

    Sub DeleteRecordAttachment(id As Integer)
        'done:DeleteRecordAttachment
        BtnCancelAttachment_Click(Nothing, Nothing)
        Dim objDel As NawaDAL.EmailTemplateAttachment = objListEmailTemplateAttachmentAdd.Find(Function(x) x.PK_EmailTemplateAttachment_ID = id)
        If Not objDel Is Nothing Then
            objListEmailTemplateAttachmentAdd.Remove(objDel)
            BindGridAttachment()
        End If
    End Sub

    Sub LoadDataEditAttachment(id As Integer)
        'todohendra:LoadDataEditAttachment
        objEmailTemplateAttachmentedit = objListEmailTemplateAttachmentAdd.Find(Function(x) x.PK_EmailTemplateAttachment_ID = id)
        If Not objEmailTemplateAttachmentedit Is Nothing Then
            WindowEmailAttachment.Hidden = False
            clearinputEmailAttachment()

            ComboBoxEmailAttachmentType.SetValue(objEmailTemplateAttachmentedit.FK_EmailAttachmentType_ID)
            TxtReportName.Text = objEmailTemplateAttachmentedit.NamaReport
            TxtReportParameter.Text = objEmailTemplateAttachmentedit.ParameterReport
            LblFileReport.Text = objEmailTemplateAttachmentedit.NamaFile
            cboRenderAs.SetValue(objEmailTemplateAttachmentedit.FK_EmailRenderAs_Id)
            txtFileName.Text = objEmailTemplateAttachmentedit.NamaFile
        End If
    End Sub

    Protected Sub GridEmailAttachmentCommand(sender As Object, e As Ext.Net.DirectEventArgs)

        Try
            'done:GridEmailAttachmentCommand
            Dim id As String = e.ExtraParams(0).Value
            If e.ExtraParams(1).Value = "Edit" Then
                LoadDataEditAttachment(id)
            ElseIf e.ExtraParams(1).Value = "Delete" Then
                DeleteRecordAttachment(id)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub GridCommandEmailAction(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            'done:GridCommandEmailAction

            Try
                Dim id As String = e.ExtraParams(0).Value
                If e.ExtraParams(1).Value = "Edit" Then
                    LoadDataEditAction(id)
                ElseIf e.ExtraParams(1).Value = "Delete" Then
                    DeleteRecordAction(id)

                End If
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try
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

    Sub ClearDetail()
        startDate.Text = ""
        StartTime.SelectedTime = Nothing
        chkExcludeHoliday.Checked = False
    End Sub

    <DirectMethod>
    Public Sub CekMonitoringStatus()
        If CboMonitoringDuration.Value <> "" Then
            Dim result As String = CboMonitoringDuration.Value.ToString.Split("-")(0)
            If result = "1" Then
                ClearDetail()
                FieldSet1.Hidden = True
            Else
                FieldSet1.Hidden = False
            End If
        Else
            ClearDetail()
            FieldSet1.Hidden = True

        End If

    End Sub

    Protected Sub Storetrigger_Readdata(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = NawaBLL.Nawa.BLL.NawaFramework.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next

            Dim strTable As String = ""

            NawaBLL.Nawa.BLL.NawaFramework.GetPicker(CboMonitoringDuration, "SELECT PK_MonitoringDuration_Id as ID,MonitoringDurationName as Name FROM MonitoringDuration", False, True)

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("MonitoringDuration", "PK_MonitoringDuration_Id as ID,MonitoringDurationName as Name", strfilter, strsort, intStart, intLimit, inttotalRecord)
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strfilter, strsort, intStart, intLimit, inttotalRecord)

            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If

            e.Total = inttotalRecord

            Dim objStoredata As Ext.Net.Store = CType(sender, Ext.Net.Store)
            objStoredata.DataSource = DataPaging
            objStoredata.DataBind()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Sub cboEmailAttachmentType_change(sender As Object, e As DirectEventArgs)
        Try
            'done:cboEmailAttachmentType_change
            If ComboBoxEmailAttachmentType.SelectedItem.Value = 1 Then
                FileReport.Hidden = False
                LblFileReport.Hidden = False
                cboRenderAs.Hidden = True
                TxtReportName.Hidden = True
                TxtReportParameter.Hidden = True
                TxtReportParameter.Text = ""
                cboRenderAs.Clear()
                txtFileName.Hidden = True
            ElseIf ComboBoxEmailAttachmentType.SelectedItem.Value = 2 Then
                FileReport.Hidden = True
                LblFileReport.Hidden = True
                FileReport.Clear()
                LblFileReport.Text = ""
                TxtReportParameter.Hidden = False
                cboRenderAs.Hidden = False
                TxtReportName.Hidden = False
                txtFileName.Hidden = False

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
            NawaBLL.EmailTemplateBLL.IsEmailTemplateNameAlreadyExist(txtEmailTemplate.Text.Trim)
            'Throw New Exception("Email Template Name " & txtEmailTemplate.Text.Trim & " already exists.")
            'End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Sub BtnCancelAction_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            'done handle cancel window email action

            WindowEmailAction.Hidden = True
            clearinputEmailAction()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub SaveEmailActionAdd()
        Dim objNewEmailAction As New NawaDAL.EmailTemplateAction
        With objNewEmailAction

            Dim objrand As New Random
            Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
            While Not objListEmailTemplateActionAdd.Find(Function(x) x.PK_EmailTemplate_Action_ID = intpk) Is Nothing
                intpk = objrand.Next(Integer.MinValue, -1)
            End While
            .PK_EmailTemplate_Action_ID = intpk
            .FK_EmailTemplate_ID = 0
            .FK_EmailActionType_ID = cboEmailAction.SelectedItem.Value
            .TSQLtoExecute = TxtsqlToExecute.Text.Trim
            objListEmailTemplateActionAdd.Add(objNewEmailAction)
        End With
    End Sub

    Sub SaveEmailActionEdit()
        With objEmailTemplateActionEdit
            .FK_EmailActionType_ID = cboEmailAction.SelectedItem.Value
            .TSQLtoExecute = TxtsqlToExecute.Text.Trim
        End With
    End Sub

    Protected Sub btnSubmitAction_DirectEvent(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            'done: save inputan email action

            If objEmailTemplateActionEdit Is Nothing Then
                SaveEmailActionAdd()
            Else
                SaveEmailActionEdit()

            End If

            BindGridAction()
            clearinputEmailAction()
            WindowEmailAction.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub BindGridAction()

        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListEmailTemplateActionAdd)
        Dim objcol As New Data.DataColumn
        objcol.ColumnName = "EmailActionTypeName"
        objcol.DataType = GetType(String)
        objdt.Columns.Add(objcol)

        Dim objtabletype As List(Of NawaDAL.EmailActionType) = NawaBLL.EmailTemplateBLL.GetEmailActionTypeAll

        For Each item As Data.DataRow In objdt.Rows
            Dim objcek As NawaDAL.EmailActionType = objtabletype.Find(Function(x) x.PK_EmailActionType_Id = item("FK_EmailActionType_ID"))
            If objcek Is Nothing Then
                item("EmailActionTypeName") = ""
            Else
                item("EmailActionTypeName") = objcek.EmailActionTypeName
            End If

        Next

        StoreEmailTemplateAction.DataSource = objdt
        StoreEmailTemplateAction.DataBind()
    End Sub

    Sub clearinputEmailAction()
        'done:clear inputan email action
        cboEmailAction.Clear()
        TxtsqlToExecute.Text = ""

    End Sub

    Protected Sub btnAddEmailAction_DirectClick(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            'done:btnAddEmailAction_DirectClick
            WindowEmailAction.Hidden = False
            clearinputEmailAction()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then

                With objEmailTemplateAddData
                    .PK_EmailTemplate_ID = 0
                    .EmailTemplateName = txtEmailTemplate.Text.Trim
                    .EmailTo = txtEmailTo.Text.Trim
                    .EmailCC = txtEmailCC.Text.Trim
                    .EmailBCC = txtEmailBCC.Text.Trim
                    .EmailSubject = txtEmailSubject.Text.Trim
                    .EmailBody = txtBody.Text.Trim
                    .FK_Monitoringduration_ID = CboMonitoringDuration.Value.ToString.Split("-")(0)
                    If .FK_Monitoringduration_ID <> "1" Then
                        .StartDate = NawaBLL.Common.ConvertToDate(NawaBLL.SystemParameterBLL.GetDateFormat, startDate.RawText)
                        .StartTime = StartTime.Value.ToString
                        .ExcludeHoliday = chkExcludeHoliday.Checked
                    Else
                        .StartDate = Nothing
                        .StartTime = Nothing
                        .ExcludeHoliday = Nothing
                    End If

                    .Active = True
                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .CreatedDate = DateTime.Now
                    .LastUpdateDate = DateTime.Now
                End With

                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                    objEmailTemplateBLL.SaveAddTanpaApproval(objEmailTemplateAddData, objListEmailTemplateAdditionalAdd, objListEmailTemplateDetailAdd, objListEmailTemplateActionAdd, objListEmailTemplateAttachmentAdd, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else
                    objEmailTemplateBLL.SaveAddApproval(objEmailTemplateAddData, objListEmailTemplateAdditionalAdd, objListEmailTemplateDetailAdd, objListEmailTemplateActionAdd, objListEmailTemplateAttachmentAdd, ObjModule)
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