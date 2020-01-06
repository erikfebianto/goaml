Imports NawaDevBLL

Partial Class EODTaskAdd
    Inherits Parent

#Region "Session"
    Private ObjEODTask As NawaDevBLL.EODTaskBLL
    Public Property ObjModule As NawaDAL.Module
        Get
            Return Session("EODTaskAdd.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODTaskAdd.ObjModule") = value
        End Set
    End Property
    Public Property ObjTask As NawaDevDAL.EODTask
        Get
            If Session("EODTaskAdd.ObjTask") Is Nothing Then
                Session("EODTaskAdd.ObjTask") = New NawaDevDAL.EODTask
            End If
            Return Session("EODTaskAdd.ObjTask")
        End Get
        Set(ByVal value As NawaDevDAL.EODTask)
            Session("EODTaskAdd.ObjTask") = value
        End Set
    End Property
    Public Property ListTaskDetail As List(Of NawaDevDAL.EODTaskDetail)
        Get
            If Session("EODTaskAdd.ListTaskDetail") Is Nothing Then
                Session("EODTaskAdd.ListTaskDetail") = New List(Of NawaDevDAL.EODTaskDetail)
            End If
            Return Session("EODTaskAdd.ListTaskDetail")
        End Get
        Set(ByVal value As List(Of NawaDevDAL.EODTaskDetail))
            Session("EODTaskAdd.ListTaskDetail") = value
        End Set
    End Property
    Public Property ObjDetail As NawaDevDAL.EODTaskDetail
        Get
            Return Session("EODTaskAdd.ObjDetail")
        End Get
        Set(ByVal value As NawaDevDAL.EODTaskDetail)
            Session("EODTaskAdd.ObjDetail") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()
                Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                If Not ObjModule Is Nothing Then
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Add"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Add"
                    StoreDetailType.Reload()

                    Dim objrand As New Random
                    ObjTask.PK_EODTask_ID = objrand.Next
                Else
                    Throw New Exception("Invalid Module ID")
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub Load_Init(sender As Object, e As EventArgs) Handles Me.Init
        ObjEODTask = New NawaDevBLL.EODTaskBLL(FormPanelInput)
    End Sub
#End Region

#Region "Method"
    Private Sub ClearSession()
        ObjTask = Nothing
        ObjModule = Nothing
        ListTaskDetail = New List(Of NawaDevDAL.EODTaskDetail)
        ObjDetail = Nothing
    End Sub

    Private Sub BindDetail()
        StoreTaskDetail.DataSource = ListTaskDetail.OrderBy(Function(x) x.OrderNo).ToList
        StoreTaskDetail.DataBind()
    End Sub
    Private Sub ClearInput()
        txtKeterangan.Text = ""
        txtProcName.Text = ""
        ChkIsUseprocessDate.Checked = False
        ChkIsUseBranch.Checked = False
        CboTaskDetailType.ClearValue()
        FileSSIS.Reset()
        txtFileName.Text = ""
    End Sub

    Private Function IsDataDetailValid() As Boolean
        If CboTaskDetailType.SelectedItem.Value Is Nothing Then
            Throw New Exception("Please Select Task Detail Type")
        End If
        If CboTaskDetailType.SelectedItem.Value = 1 Or CboTaskDetailType.SelectedItem.Value = 3 Then
            'ssis
            If Me.ObjDetail Is Nothing Then
                If Not FileSSIS.HasFile Then
                    Throw New Exception("Please Upload .dtsx File")
                End If
                If IO.Path.GetExtension(FileSSIS.FileName) <> ".dtsx" Then
                    Throw New Exception("Please Upload .dtsx File")
                End If
            End If
        ElseIf CboTaskDetailType.SelectedItem.Value = 2 Then
            If txtProcName.Text.Trim = "" Then
                Throw New Exception("Please Enter Store Procedure Name.")
            End If
            If Not NawaBLL.EODTaskBLL.IshasParameter(txtProcName.Text.Trim, "@PK_EODTaskDetailLog_ID") Then
                Throw New Exception("Store Procedure Name " & txtProcName.Text & " must have parameter @PK_EODTaskDetailLog_ID")
            End If
            'cek kalau procedurenya harus ada parameter @PK_EODTaskDetailLog_ID
        End If

        Return True
    End Function
    Sub SaveAddTaskDetail()
        Dim objNewTaskdetail As New NawaDevDAL.EODTaskDetail
        With objNewTaskdetail
            .PK_EODTaskDetail_ID = ListTaskDetail.Select(Function(x) x.PK_EODTaskDetail_ID).DefaultIfEmpty(0).Max() + 1
            .FK_EODTask_ID = ObjTask.PK_EODTask_ID
            .FK_EODTaskDetailType_ID = CboTaskDetailType.SelectedItem.Value
            .OrderNo = ListTaskDetail.Count + 1
            If .FK_EODTaskDetailType_ID = 1 Or .FK_EODTaskDetailType_ID = 3 Then
                'ssis
                .SSISName = IO.Path.GetFileName(FileSSIS.FileName)
                .SSISFIle = FileSSIS.FileBytes
                .StoreProcedureName = ""
                .IsUseParameterProcessDate = False
                .IsUseParameterBranch = False
            Else
                'store procedure
                .SSISName = ""
                .SSISFIle = Nothing
                .StoreProcedureName = txtProcName.Text
                .IsUseParameterProcessDate = ChkIsUseprocessDate.Checked
                .IsUseParameterBranch = ChkIsUseBranch.Checked
            End If
            .Keterangan = txtKeterangan.Text.Trim
        End With

        ListTaskDetail.Add(objNewTaskdetail)
        BindDetail()

        FormPanelTaskDetail.Hidden = True
        WindowDetail.Hidden = True
    End Sub
    Sub SaveEditTaskDetail()
        ObjDetail.FK_EODTaskDetailType_ID = CboTaskDetailType.SelectedItem.Value
        If ObjDetail.FK_EODTaskDetailType_ID = 1 Or ObjDetail.FK_EODTaskDetailType_ID = 3 Then
            If FileSSIS.HasFile Then
                ObjDetail.SSISName = IO.Path.GetFileName(FileSSIS.FileName)
                ObjDetail.SSISFIle = FileSSIS.FileBytes
            End If

            ObjDetail.StoreProcedureName = ""
            ObjDetail.IsUseParameterProcessDate = False
            ObjDetail.IsUseParameterBranch = False
        Else
            ObjDetail.SSISName = ""
            ObjDetail.SSISFIle = Nothing
            ObjDetail.StoreProcedureName = txtProcName.Text
            ObjDetail.IsUseParameterProcessDate = ChkIsUseprocessDate.Checked
            ObjDetail.IsUseParameterBranch = ChkIsUseBranch.Checked
        End If
        ObjDetail.Keterangan = txtKeterangan.Text

        ObjDetail = Nothing
        BindDetail()

        FormPanelTaskDetail.Hidden = True
        WindowDetail.Hidden = True
    End Sub

    Private Sub DeleteRecordTaskDetail(id As Long)
        BtnCancelDetail_DirectEvent(Nothing, Nothing)
        Dim objDelete As NawaDevDAL.EODTaskDetail = ListTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = id)
        If Not objDelete Is Nothing Then
            ListTaskDetail.Remove(objDelete)
            Dim counter As Integer = 0
            For Each item As NawaDevDAL.EODTaskDetail In ListTaskDetail
                counter += 1
                item.OrderNo = counter
            Next
            BindDetail()
        End If
    End Sub
    Private Sub MoveUpRecordTaskDetail(id As Long)
        BtnCancelDetail_DirectEvent(Nothing, Nothing)
        Dim objMove As NawaDevDAL.EODTaskDetail = ListTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = id)
        Dim objMoveBefore As NawaDevDAL.EODTaskDetail = ListTaskDetail.FindLast(Function(x) x.OrderNo < objMove.OrderNo)
        If objMove.OrderNo > 1 Then
            Dim orderNo As Integer = objMove.OrderNo
            Dim orderNoBefore As Integer = objMoveBefore.OrderNo
            objMove.OrderNo = orderNoBefore
            objMoveBefore.OrderNo = orderNo
            BindDetail()
        End If
    End Sub
    Private Sub MoveDownRecordTaskDetail(id As Long)
        BtnCancelDetail_DirectEvent(Nothing, Nothing)
        Dim objMove As NawaDevDAL.EODTaskDetail = ListTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = id)
        Dim objMoveBefore As NawaDevDAL.EODTaskDetail = ListTaskDetail.FindLast(Function(x) x.OrderNo > objMove.OrderNo)
        If objMove.OrderNo < ListTaskDetail.Count Then
            Dim orderNo As Integer = objMove.OrderNo
            Dim orderNoBefore As Integer = objMoveBefore.OrderNo
            objMove.OrderNo = orderNoBefore
            objMoveBefore.OrderNo = orderNo
            BindDetail()
        End If
    End Sub
    Private Sub LoadDataEdit(id As Long)
        ObjDetail = ListTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = id)
        If Not ObjDetail Is Nothing Then
            FormPanelTaskDetail.Hidden = False
            WindowDetail.Hidden = False
            ClearInput()
            With ObjDetail
                CboTaskDetailType.SetValueAndFireSelect(.FK_EODTaskDetailType_ID)
                If .FK_EODTaskDetailType_ID = 1 Or .FK_EODTaskDetailType_ID = 3 Then
                    'ssis
                    txtFileName.Text = .SSISName.Trim
                ElseIf .FK_EODTaskDetailType_ID = 2 Then
                    'sp
                    txtProcName.Text = .StoreProcedureName.Trim
                    ChkIsUseprocessDate.Checked = .IsUseParameterProcessDate
                    ChkIsUseBranch.Checked = .IsUseParameterBranch
                End If
                txtKeterangan.Text = .Keterangan
            End With
        End If
    End Sub
    Private Sub DownloadFile(id As Long)
        Dim objdownload As NawaDevDAL.EODTaskDetail = ListTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = id)
        If Not objdownload Is Nothing And (objdownload.FK_EODTaskDetailType_ID = 1 Or objdownload.FK_EODTaskDetailType_ID = 3) Then
            Response.Clear()
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=" & objdownload.SSISName)
            Response.Charset = ""
            Response.AddHeader("cache-control", "max-age=0")
            Me.EnableViewState = False
            Response.ContentType = "ContentType"
            Response.BinaryWrite(objdownload.SSISFIle)
            Response.End()
        End If
    End Sub
#End Region

#Region "Direct Events"
    Protected Sub CboTaskDetailType_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            StoreDetailType.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.Text, "SELECT PK_EODTaskDetailType_ID,EODTaskDetailType FROM EODTaskDetailType", Nothing)
            StoreDetailType.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Public Sub CboTaskDetailType_DirectSelect(sender As Object, e As DirectEventArgs) Handles CboTaskDetailType.DirectSelect
        Try
            FieldSSIS.Hidden = Not CboTaskDetailType.SelectedItem.Text.Contains("SSIS")
            FieldProc.Hidden = Not CboTaskDetailType.SelectedItem.Value = 2
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
            ElseIf e.ExtraParams(1).Value = "MoveUp" Then
                MoveUpRecordTaskDetail(id)
            ElseIf e.ExtraParams(1).Value = "MoveDown" Then
                MoveDownRecordTaskDetail(id)
            ElseIf e.ExtraParams(1).Value = "Download" Then
                DownloadFile(id)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            ClearInput()
            FieldSSIS.Hidden = True
            FieldProc.Hidden = True
            FormPanelTaskDetail.Hidden = False
            WindowDetail.Hidden = False
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If TxtTaskName.Text.Trim = "" Then
                Throw New Exception("Please Enter Task Name.")
            ElseIf ListTaskDetail.Count = 0 Then
                Throw New Exception("Task Detail Data at least one data.")
            End If

            With ObjTask
                .EODTaskName = TxtTaskName.Text.Trim
                .EODTaskDescription = TxtTaskDescription.Text.Trim
                .Active = True
                .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                .CreatedDate = DateTime.Now
                .LastUpdateDate = DateTime.Now
            End With

            If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                ObjEODTask.SaveAddTanpaApproval(ObjTask, ListTaskDetail, ObjModule)
                LblConfirmation.Text = "Data Saved into Database"
            Else
                ObjEODTask.SaveAddApproval(ObjTask, ListTaskDetail, ObjModule)
                LblConfirmation.Text = "Data Saved into Pending Approval"
            End If

            Panelconfirmation.Hidden = False
            FormPanelInput.Hidden = True
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnSaveDetail_DirectEvent(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataDetailValid() Then
                If ObjDetail Is Nothing Then
                    SaveAddTaskDetail()
                Else
                    SaveEditTaskDetail()
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
    Protected Sub BtnCancelDetail_DirectEvent(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            FormPanelTaskDetail.Hidden = True
            WindowDetail.Hidden = True
            ObjDetail = Nothing
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
#End Region

End Class
