Imports NawaDevDAL
Partial Class EODProcessManualSatuan
    Inherits Parent

#Region "Session"
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
#End Region

#Region "Page Load"
    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                ClearSession()

                objProcessID = Request.Params("ProcessID")

                Dim strmodule As String = Request.Params("ModuleID")
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
                    txtStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    Dim objscheduler As NawaDAL.EODScheduler = NawaBLL.EODSchedulerBLL.GetEODSchedulerByPKID(objProcessID)
                    If Not objscheduler Is Nothing Then
                        ProcessID.Text = objscheduler.EODSchedulerName
                    End If

                    LoadTree()

                End If
            End If
            'objEodTask.BentukformAdd()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Method"
    Sub ClearSession()
        ObjModule = Nothing
        objProcessID = Nothing
    End Sub
    Sub LoadTree()
        Dim strval As String = ""
        Dim strtext As String = ""
        NawaBLL.EODSchedulerBLL.ProcessTree(Treecombo1, objProcessID, strtext)

        cbotask.Text = strtext
        cbotask.Render()
        Treecombo1.ExpandAll()
    End Sub
#End Region

    'Protected Sub Storetrigger_Readdata(sender As Object, e As StoreReadDataEventArgs)
    '    Try
    '        Dim intStart As Integer = e.Start
    '        Dim intLimit As Int16 = e.Limit
    '        Dim inttotalRecord As Integer
    '        Dim strfilter As String = NawaBLL.Nawa.BLL.NawaFramework.GetWhereClauseHeader(e)
    '        Dim strsort As String = ""
    '        For Each item As DataSorter In e.Sort
    '            strsort += item.Property & " " & item.Direction.ToString
    '        Next

    '        Dim strTable As String = ""


    '        NawaBLL.Nawa.BLL.NawaFramework.GetPicker(cbotask, "SELECT PK_EODScheduler_ID as ID, EODSchedulerName as Nama FROM EODScheduler WHERE ACTIVE=1", False, True)

    '        Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("EODScheduler", "PK_EODScheduler_ID as ID, EODSchedulerName as Nama", strfilter, strsort, intStart, intLimit, inttotalRecord)
    '        'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strfilter, strsort, intStart, intLimit, inttotalRecord)

    '        Dim limit As Integer = e.Limit
    '        If (e.Start + e.Limit) > inttotalRecord Then
    '            limit = inttotalRecord - e.Start
    '        End If

    '        e.Total = inttotalRecord


    '        Dim objStoredata As Ext.Net.Store = CType(sender, Ext.Net.Store)
    '        objStoredata.DataSource = DataPaging
    '        objStoredata.DataBind()

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

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

    Protected Sub cboProcess_DirectSelect(sender As Object, e As DirectEventArgs)
        Try

            LoadTree()



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As DirectEventArgs)
        Try

            Dim datadate As Date = NawaBLL.Common.ConvertToDate(NawaBLL.SystemParameterBLL.GetDateFormat, txtStartDate.RawText)

            Dim strProcessid As String = objProcessID
            Dim strTasklist As String = cbotask.Text
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

            If String.IsNullOrEmpty(strtask) Then
                Throw New ApplicationException("Task(s) must be selected")
            End If

            NawaDevBLL.EODSchedulerBLL.SaveEODBranch(datadate, strProcessid, strtask, objSetting.KodeCabang, ObjModule)

            'NawaBLL.EODSchedulerBLL.SaveEOD(datadate, strProcessid, strtask, ObjModule)

            ''langsung simpen, eod process manual harusnya ngak mungkin pake approval
            'NawaBLL.EODSchedulerBLL.SaveEOD(datadate, strProcessid, ObjModule)


            LblConfirmation.Text = "Request Process Save into Queue"
            Panelconfirmation.Hidden = False
            FormPanelInput.Hidden = True

        Catch appEx As Exception When TypeOf appEx Is ApplicationException
            Ext.Net.X.Msg.Alert("Information", appEx.Message).Show()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "&ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "&ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
