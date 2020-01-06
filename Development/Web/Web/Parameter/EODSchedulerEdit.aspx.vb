Partial Class EODSchedulerEdit
    Inherits Parent
    Public objEodScheduler As NawaBLL.EODSchedulerBLL
    Public Property objEodSchedulerEdit() As NawaDAL.EODScheduler
        Get
            If Session("EODSchedulerEdit.objEodSchedulerEdit") Is Nothing Then
                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                Dim oNewScheduler As NawaDAL.EODScheduler = NawaBLL.EODSchedulerBLL.GetEODSchedulerByPKID(id)
                
                Session("EODSchedulerEdit.objEodSchedulerEdit") = oNewScheduler

            End If
            Return Session("EODSchedulerEdit.objEodSchedulerEdit")
        End Get
        Set(ByVal value As NawaDAL.EODScheduler)
            Session("EODSchedulerEdit.objEodSchedulerEdit") = value
        End Set
    End Property

    Public Property objListEodSchedulerDetailEdit() As List(Of NawaDAL.EODSchedulerDetail)
        Get
            If Session("EODSchedulerEdit.objListEodSchedulerDetailEdit") Is Nothing Then


                Session("EODSchedulerEdit.objListEodSchedulerDetailEdit") = NawaBLL.EODSchedulerBLL.GetEODSchedulerDetailByFKID(objEodSchedulerEdit.PK_EODScheduler_ID)

            End If
            Return Session("EODSchedulerEdit.objListEodSchedulerDetailEdit")
        End Get
        Set(ByVal value As List(Of NawaDAL.EODSchedulerDetail))
            Session("EODSchedulerEdit.objListEodSchedulerDetailEdit") = value
        End Set
    End Property

    Public Property objEodSchedulerDetailEdit() As NawaDAL.EODSchedulerDetail
        Get


            Return Session("EODSchedulerEdit.objEodSchedulerDetailEdit")
        End Get
        Set(ByVal value As NawaDAL.EODSchedulerDetail)
            Session("EODSchedulerEdit.objEodSchedulerDetailEdit") = value
        End Set
    End Property



    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("EODSchedulerEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODSchedulerEdit.ObjModule") = value
        End Set
    End Property
    Private Sub Load_Init(sender As Object, e As EventArgs) Handles Me.Init
        objEodScheduler = New NawaBLL.EODSchedulerBLL()
    End Sub
    Sub ClearSession()
        objEodSchedulerEdit = Nothing
        objListEodSchedulerDetailEdit = Nothing
        ObjModule = Nothing
        objEodSchedulerDetailEdit = Nothing
    End Sub
    Sub LoadComboPeriod()
        storeperiod.DataSource = NawaBLL.EODSchedulerBLL.GetMsEODPeriods
        storeperiod.DataBind()
    End Sub

    Sub LoadTask()
        StoreTask.DataSource = NawaBLL.EODTaskBLL.GetEODTaskActive
        StoreTask.DataBind()
    End Sub

    Sub LoadData()
        If Not objEodSchedulerEdit Is Nothing Then
            With objEodSchedulerEdit
                txtID.Text = .PK_EODScheduler_ID
                txtSchedulerName.Text = .EODSchedulerName
                txtSchedulerDesc.Text = .EODSchedulerDescription


                chkUsePeriodicalScheduler.SetValue(.HasPeriodikScheduler)
                chkUsePeriodicalScheduler_DirectEvent(Nothing, Nothing)
                CboPeriod.SetValueAndFireSelect(.FK_MsEODPeriod.GetValueOrDefault)


                If .EODPeriod.HasValue Then
                    txtPeriod.Text = .EODPeriod


                End If
                If .StartDate.HasValue Then
                    txtStartDate.Text = CDate(.StartDate).ToString(NawaBLL.SystemParameterBLL.GetDateFormat)
                    TxtTime.Text = CDate(.StartDate).ToString("HH:mm:ss")
                End If


            End With
            BindGridScheduler()
        End If
    End Sub

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()
                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Update) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Edit"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Edit"
                    txtStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    LoadComboPeriod()
                    LoadTask()
                    LoadData()

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
    Sub BindTask()
        StoreTask.DataSource = NawaBLL.EODTaskBLL.GetEODTaskActive
        StoreTask.DataBind()
    End Sub

    Protected Sub btnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            cboTask.SetValueAndFireSelect(Nothing)
            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
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

            If txtSchedulerName.Text.Trim = "" Then
                Throw New Exception("Scheduler Name is Required.")
            End If
            If chkUsePeriodicalScheduler.Checked Then
                If txtPeriod.Text = "" Then
                    Throw New Exception("Period is Required.")
                ElseIf txtPeriod.Text = "0" Then
                    Throw New Exception("Period must greater then 0.")
                End If
                If CboPeriod.SelectedItem.Value Is Nothing Then
                    Throw New Exception("Period Type is Required.")
                End If
                If txtStartDate.RawText.Trim = "" Then
                    Throw New Exception("Start Date is Required.")
                End If
                If TxtTime.Text.Trim = "" Then
                    Throw New Exception("Time is Required.")
                End If
            End If
            If objListEodSchedulerDetailEdit.Count = 0 Then
                Throw New Exception("Scheduler Detail required minimal 1 task.")
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Sub ClearPeriod()
        txtPeriod.Text = ""
        txtPeriod.Value = ""
        CboPeriod.Text = ""
        txtStartDate.Text = ""
        TxtTime.Text = "00:00"
    End Sub
    Protected Sub chkUsePeriodicalScheduler_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            If chkUsePeriodicalScheduler.Checked Then
                FieldContainer1.Hidden = False
                ClearPeriod()

            Else
                FieldContainer1.Hidden = True
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then

                With objEodSchedulerEdit



                    .EODSchedulerName = txtSchedulerName.Text.Trim
                    .EODSchedulerDescription = txtSchedulerDesc.Text.Trim
                    .HasPeriodikScheduler = chkUsePeriodicalScheduler.Checked
                    If .HasPeriodikScheduler Then
                        .EODPeriod = txtPeriod.Text
                        .FK_MsEODPeriod = CboPeriod.SelectedItem.Value
                        .StartDate = NawaBLL.Common.ConvertToDate(Replace(NawaBLL.SystemParameterBLL.GetDateFormat, "HH:mm:ss", "") & " HH:mm:ss", CDate(txtStartDate.Value).ToString(NawaBLL.SystemParameterBLL.GetDateFormat) & " " & TxtTime.Text)

                    Else
                        .EODPeriod = Nothing
                        .FK_MsEODPeriod = Nothing
                        .StartDate = Nothing
                    End If
                    .Active = True
                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .CreatedDate = DateTime.Now
                    .LastUpdateDate = DateTime.Now
                End With


                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then
                    objEodScheduler.SaveEditTanpaApproval(objEodSchedulerEdit, objListEodSchedulerDetailEdit, ObjModule)
                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else
                    objEodScheduler.SaveEditApproval(objEodSchedulerEdit, objListEodSchedulerDetailEdit, ObjModule)
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
    Sub SaveAddSchedulerDetail()
        Dim objNewScheduler As New NawaDAL.EODSchedulerDetail
        With objNewScheduler
            Dim objrand As New Random
            Dim intpk As Long = objrand.Next(Integer.MinValue, -1)
            While Not objListEodSchedulerDetailEdit.Find(Function(x) x.PK_EODSchedulerDetail_ID = intpk) Is Nothing
                intpk = objrand.Next(Integer.MinValue, -1)
            End While

            .PK_EODSchedulerDetail_ID = intpk
            .FK_EODSCheduler_ID = objEodSchedulerEdit.PK_EODScheduler_ID
            .FK_EODTask_ID = cboTask.SelectedItem.Value
            .OrderNo = objListEodSchedulerDetailEdit.Count + 1
            .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
            .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
            .ApprovedBy = ""
            .CreatedDate = Nothing
            .LastUpdateDate = DateTime.Now
            .ApprovedDate = Nothing

        End With
        objListEodSchedulerDetailEdit.Add(objNewScheduler)

        BindGridScheduler()
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()

    End Sub

    Sub LoadDataEdit(id As Long)
        objEodSchedulerDetailEdit = objListEodSchedulerDetailEdit.Find(Function(x) x.PK_EODSchedulerDetail_ID = id)
        If Not objEodSchedulerDetailEdit Is Nothing Then
            FormSchedulerDetail.Hidden = False
            WindowDetail.Hidden = False
            Clearinput()
            With objEodSchedulerDetailEdit
                cboTask.SetValueAndFireSelect(.FK_EODTask_ID)
            End With
        End If
    End Sub

    Sub DeleteRecordTaskDetail(id As Long)
        BtnCancelSchedulerDetail_DirectEvent(Nothing, Nothing)
        Dim objdel As NawaDAL.EODSchedulerDetail = objListEodSchedulerDetailEdit.Find(Function(x) x.PK_EODSchedulerDetail_ID = id)
        If Not objdel Is Nothing Then
            objListEodSchedulerDetailEdit.Remove(objdel)
            Dim intcounter As Integer = 0

            For Each item As NawaDAL.EODSchedulerDetail In objListEodSchedulerDetailEdit
                intcounter += 1
                item.OrderNo = intcounter
            Next
            BindGridScheduler()
        End If
    End Sub

    Sub MoveUpRecordTaskDetail(id As Long)
        BtnCancelSchedulerDetail_DirectEvent(Nothing, Nothing)
        Dim objmove As NawaDAL.EODSchedulerDetail = objListEodSchedulerDetailEdit.Find(Function(x) x.PK_EODSchedulerDetail_ID = id)
        Dim objMovebefore As NawaDAL.EODSchedulerDetail = objListEodSchedulerDetailEdit.FindLast(Function(x) x.OrderNo < objmove.OrderNo)
        If objmove.OrderNo > 1 Then
            Dim oldorder As Integer = objmove.OrderNo
            Dim oldorderbefore As Integer = objMovebefore.OrderNo
            objmove.OrderNo = oldorderbefore
            objMovebefore.OrderNo = oldorder
            BindGridScheduler()
        End If

    End Sub
    Sub MoveDownRecordTaskDetail(id As Long)
        BtnCancelSchedulerDetail_DirectEvent(Nothing, Nothing)
        Dim objmove As NawaDAL.EODSchedulerDetail = objListEodSchedulerDetailEdit.Find(Function(x) x.PK_EODSchedulerDetail_ID = id)
        Dim objMovebefore As NawaDAL.EODSchedulerDetail = objListEodSchedulerDetailEdit.FindLast(Function(x) x.OrderNo > objmove.OrderNo)
        If objmove.OrderNo < objListEodSchedulerDetailEdit.Count Then
            Dim oldorder As Integer = objmove.OrderNo
            Dim oldorderbefore As Integer = objMovebefore.OrderNo
            objmove.OrderNo = oldorderbefore
            objMovebefore.OrderNo = oldorder
            BindGridScheduler()
        End If
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

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Sub BindGridScheduler()
        objListEodSchedulerDetailEdit = objListEodSchedulerDetailEdit.OrderBy(Function(x) x.OrderNo).ToList()
        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListEodSchedulerDetailEdit)
        Dim objcol As New Data.DataColumn
        objcol.ColumnName = "TaskName"
        objcol.DataType = GetType(String)
        objdt.Columns.Add(objcol)

        Dim objTask As List(Of NawaDAL.EODTask) = NawaBLL.EODTaskBLL.GetEODTaskActive

        For Each item As Data.DataRow In objdt.Rows
            Dim objcek As NawaDAL.EODTask = objTask.Find(Function(x) x.PK_EODTask_ID = item("FK_EODTask_ID"))
            If objcek Is Nothing Then
                item("TaskName") = ""
            Else
                item("TaskName") = objcek.EODTaskName
            End If

        Next

        StoreSchedulerDetail.DataSource = objdt
        StoreSchedulerDetail.DataBind()
    End Sub
    Sub Clearinput()

        cboTask.ClearValue()

    End Sub
    Sub SaveEditSchedulerDetail()

        With objEodSchedulerDetailEdit
            .FK_EODTask_ID = cboTask.SelectedItem.Value
            .LastUpdateDate = DateTime.Now
        End With
        objEodSchedulerDetailEdit = Nothing
        BindGridScheduler()
        FormSchedulerDetail.Hidden = True
        WindowDetail.Hidden = True
        Clearinput()
    End Sub

    Protected Sub btnSaveSchedulerDetail_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            If IsDataDetailValid() Then
                If Me.objEodSchedulerDetailEdit Is Nothing Then
                    SaveAddSchedulerDetail()
                Else
                    SaveEditSchedulerDetail()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Function IsDataDetailValid() As Boolean
        If cboTask.SelectedItem.Value Is Nothing Then
            Throw New Exception("Please Select Task ")
        End If
        If Me.objEodSchedulerDetailEdit Is Nothing Then
            'cek ngak boleh doble
            If Not objListEodSchedulerDetailEdit.Find(Function(x) x.FK_EODTask_ID = cboTask.SelectedItem.Value) Is Nothing Then
                Throw New Exception("Task Already in List")
            End If
        Else
            If Not objListEodSchedulerDetailEdit.Find(Function(x) x.FK_EODTask_ID = cboTask.SelectedItem.Value And x.PK_EODSchedulerDetail_ID <> objEodSchedulerDetailEdit.PK_EODSchedulerDetail_ID) Is Nothing Then
                Throw New Exception("Task Already in List")
            End If
        End If
        Return True
    End Function
    Protected Sub BtnCancelSchedulerDetail_DirectEvent(sender As Object, e As DirectEventArgs)
        Try

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    'Protected Sub storeperiod_ReadData(sender As Object, e As StoreReadDataEventArgs)
    '    Try
    '        LoadComboPeriod()
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub
End Class
