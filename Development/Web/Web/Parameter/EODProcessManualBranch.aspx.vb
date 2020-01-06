Imports NawaDevDAL

Partial Class EODProcessManualBranch
    Inherits Parent

#Region "Session"
    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("EODProcessManualBranch.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("EODProcessManualBranch.ObjModule") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")

                Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                If ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                Else
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " View"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " View"

                    DeStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    SettingPaging()
                    LoadTree()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Method"
    Sub SettingPaging()
        CboProcess.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
        StoreProcess.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
    End Sub
    Sub LoadTree()
        Dim intprocess As Long
        If IsNumeric(CboProcess.Value) = True Then
            intprocess = CboProcess.Value
        Else
            intprocess = 0
        End If
        Dim strval As String = ""
        Dim strtext As String = ""
        NawaBLL.EODSchedulerBLL.ProcessTree(Treecombo1, intprocess, strtext)

        CboTask.Text = strtext
        CboTask.Render()
        Treecombo1.ExpandAll()
    End Sub
#End Region

#Region "Direct Events"
    Protected Sub StoreProcess_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim query As String = e.Parameters("query")
            If query Is Nothing Then query = ""

            Dim strfilter As String = ""
            If query.Length > 0 Then
                strfilter = " EODSchedulerName like '" & query & "%'"
            End If
            StoreProcess.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("EODScheduler", "PK_EODScheduler_ID,EODSchedulerName", strfilter, "PK_EODScheduler_ID", e.Start, e.Limit, e.Total)
            StoreProcess.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
    Protected Sub CboProcess_DirectSelect(sender As Object, e As DirectEventArgs)
        Try
            LoadTree()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub

    Protected Sub BtnSelectTask_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            Treecombo1.SetAllChecked()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnUnselectTask_OnClicked(sender As Object, e As DirectEventArgs)
        Try
            Treecombo1.ClearChecked()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As DirectEventArgs)
        Try
            Dim dataDate As Date = NawaBLL.Common.ConvertToDate(NawaBLL.SystemParameterBLL.GetDateFormat, DeStartDate.RawText)
            Dim strProcessid As String = CboProcess.Value
            Dim strTasklist As String = CboTask.Text
            strTasklist = strTasklist.Replace("[", "").Replace("]", "")
            Dim arrtask() As String = Split(strTasklist, ",")

            Dim strtask As String = ""
            For Each item As String In arrtask
                strtask = strtask & item.Split(" - ")(0) & ","
            Next

            If strtask.Length > 0 Then
                strtask = strtask.Substring(0, Len(strtask) - 1)
            End If

            Dim objSetting As SettingPersonal = NawaDevBLL.SLIKParameterBLL.getSettingSLIKPersonal(NawaBLL.Common.SessionCurrentUser.UserID)
            If objSetting Is Nothing Then
                objSetting = New SettingPersonal
            End If

            If objSetting.KodeCabang.ToLower = "all" Then
                Throw New ApplicationException("Run Process per Branch is not allowed for Bankwide setting")
            End If

            If String.IsNullOrEmpty(strtask) Then
                Throw New ApplicationException("Task(s) must be selected")
            End If

            NawaDevBLL.EODSchedulerBLL.SaveEODBranch(dataDate, strProcessid, strtask, objSetting.KodeCabang, ObjModule)

            ''langsung simpen, eod process manual harusnya ngak mungkin pake approval
            'NawaBLL.EODSchedulerBLL.SaveEOD(datadate, strProcessid, ObjModule)

            LblConfirmation.Text = "Request Process Save into Queue"
            Panelconfirmation.Hidden = False
            FormPanelInput.Hidden = True
        Catch appEx As ApplicationException When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
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
