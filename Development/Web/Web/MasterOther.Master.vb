Imports System.Data
Imports NawaBLL
Imports NawaDAL

<DirectMethodProxyID(IDMode:=DirectMethodProxyIDMode.None)>
Partial Class MasterOther
    Inherits System.Web.UI.MasterPage

    'Public objConn As NawaDAL.DB.Data.dbConnection

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try

            'If Not IsPostBack Then
            '    objConn = New NawaDAL.DB.Data.dbConnection(System.Configuration.ConfigurationManager.AppSettings("DbType"), System.Configuration.ConfigurationManager.ConnectionStrings("NawaDataSql").ConnectionString)
            'End If

            If Not Ext.Net.X.IsAjaxRequest Then

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    'Protected Sub StoreTahun_ReadData(sender As Object, e As StoreReadDataEventArgs)
    '    Try
    '        Dim query As String = e.Parameters("query")
    '        If query Is Nothing Then query = ""

    '        Dim strfilter As String = ""
    '        If query.Length > 0 Then
    '            strfilter = " tahun like '" & query & "%'"
    '        End If
    '        StoreTahun.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("TahunData", "Tahun", strfilter, "Tahun", e.Start, e.Limit, e.Total)
    '        StoreTahun.DataBind()
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try

    'End Sub

    'Protected Sub StoreBulan_ReadData(sender As Object, e As StoreReadDataEventArgs)
    '    Try
    '        Dim query As String = e.Parameters("query")
    '        If query Is Nothing Then query = ""

    '        Dim strfilter As String = ""
    '        If query.Length > 0 Then
    '            strfilter = " BulanName like '" & query & "%'"
    '        End If
    '        StoreBulan.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("BulanData", "BulanData,BulanName", strfilter, "BulanData", e.Start, e.Limit, e.Total)
    '        StoreBulan.DataBind()
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                If Not NawaBLL.Common.SessionCurrentUser Is Nothing Then
                    Session("Show") = False

                    NawaBLL.AuditTrail_UserLoginBLL.AuditAccesss(Request.UserHostAddress, NawaBLL.Common.SessionCurrentUser.UserID, System.IO.Path.GetFileName(Request.Path))

                    'MenuPanel1.ID = "MainMenu"
                    ResourceManager1.Theme = NawaBLL.SystemParameterBLL.GetThemeApplication
                    TopPanel.Title = NawaBLL.LoginBLL.GetAplicationName
                    Dim struser As String = NawaBLL.Common.SessionCurrentUser.UserID
                    If Not NawaBLL.Common.SessionAlternateUser Is Nothing Then
                        struser &= " Alternate by " & NawaBLL.Common.SessionAlternateUser.UserID
                        BtnChangePassword.Hidden = True
                    End If
                    barUserID.Text = "User ID : " & struser
                    barGroup.Text = "Group Menu : " & NawaBLL.MGroupMenuBLL.GetGroupMenuNameByID(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID)

                    barLastSuccessLogin.Text = "Last Success Login : " & GetLastSuccessLogin()
                    barLastFailLogin.Text = "Last Failed Login : " & GetLastFailedLogin()

                    'hLogout.Href = "~/Logout.aspx"
                    'hChangePassword.Href = "~/ChangePassword.aspx"

                    NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal = NawaDevBLL.SLIKParameterBLL.getSettingSLIKPersonal(NawaBLL.Common.SessionCurrentUser.UserID)

                    cboKantorCabang.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    StoreBranch.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    If Not NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal Is Nothing Then
                        barSLIKParameter.Text = " Period : " & NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.ReportDate.ToString("dd-MMM-yyyy") & " | Branch : " & NawaDevBLL.SLIKParameterBLL.getCabangName(NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang)
                    Else
                        barSLIKParameter.Text = " Period :  [Not Selected] | Branch : [Not Selected]"
                        windowSetting.Hidden = False

                    End If

                    CountNotification()
                    CountTaskList()

                    Using objLoginBll As New NawaBLL.LoginBLL
                        If objLoginBll.AplicationAuthentication = NawaBLL.LoginBLL.AplicationAuthenticationEnum.Window Then
                            BtnChangePassword.Visible = False
                        End If
                    End Using

                    If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID = 1 Then
                        BtnChangePassword.Hidden = False
                    End If

                    Dim apppath As String = Request.ApplicationPath
                    If apppath = "/" Then apppath = ""

                    Dim bIsMenuGenerateRunTime As Boolean = True

                    Dim objsystemparamMenu As SystemParameter = SystemParameterBLL.GetSystemParameterByPk(42)
                    If Not objsystemparamMenu Is Nothing Then
                        bIsMenuGenerateRunTime = objsystemparamMenu.SettingValue
                    End If

                    If bIsMenuGenerateRunTime Then
                        Session("apppathnawadata") = apppath
                        NawaBLL.MGroupMenuBLL.LoadMenu(MenuPanel1, apppath)
                    Else
                        Dim strmenu As String
                        strmenu = NawaBLL.MGroupMenuBLL.LoadMenu(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID)
                        strmenu = strmenu.Replace("$AppPathApplication$", apppath)

                        Dim objrootmenu As Ext.Net.Node = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Ext.Net.Node)(strmenu)
                        If Not objrootmenu Is Nothing Then
                            MenuPanel1.Root.Add(objrootmenu)
                        End If
                    End If
                    MenuPanel1.ExpandPath("Root")
                    MenuPanel1.RootVisible = False


                    Dim objsystemparam As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(SystemParameterBLL.SytemParameterEnum.MenuLoading)

                    Dim intmenuloading As Integer = 1
                    If Not objsystemparam Is Nothing Then
                        intmenuloading = objsystemparam.SettingValue
                    End If
                    If intmenuloading = 1 Then
                        MenuPanel1.Listeners.BeforeLoad.Fn = "nodeLoad"
                    End If

                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Function GetLastSuccessLogin() As String
        Dim strReturn As String = "-"

        Using ObjDb As New NawaDevDAL.NawaDatadevEntities
            Dim ObjAuditTrail_UserLogin As List(Of NawaDevDAL.AuditTrail_UserLogin) = ObjDb.AuditTrail_UserLogin.Where(Function(x) x.UserLoginUserid = NawaBLL.Common.SessionCurrentUser.UserID And x.UserLoginAction = "Login" And x.UserLoginDescription = "Login Sucess").OrderByDescending(Function(x) x.UserLoginID).ToList

            If ObjAuditTrail_UserLogin.Count >= 2 Then
                strReturn = ObjAuditTrail_UserLogin(1).UserLoginActionDate.GetValueOrDefault(New Date(1900, 1, 1)).ToString("dd-MMM-yyyy HH:mm")
            End If
        End Using

        Return strReturn
    End Function

    Private Function GetLastFailedLogin() As String
        Dim strReturn As String = "-"

        Using ObjDb As New NawaDevDAL.NawaDatadevEntities
            Dim ObjAuditTrail_UserLogin As List(Of NawaDevDAL.AuditTrail_UserLogin) = ObjDb.AuditTrail_UserLogin.Where(Function(x) x.UserLoginUserid = NawaBLL.Common.SessionCurrentUser.UserID And x.UserLoginDescription = "Login Failed").OrderByDescending(Function(x) x.UserLoginID).ToList

            If ObjAuditTrail_UserLogin.Count >= 1 Then
                strReturn = ObjAuditTrail_UserLogin(0).UserLoginActionDate.GetValueOrDefault(New Date(1900, 1, 1)).ToString("dd-MMM-yyyy HH:mm")
            End If
        End Using

        Return strReturn
    End Function

    Sub CountNotification()

        Dim paramtasklist(0) As SqlClient.SqlParameter
        paramtasklist(0) = New SqlClient.SqlParameter
        paramtasklist(0).ParameterName = "@userid"
        paramtasklist(0).Value = NawaBLL.Common.SessionCurrentUser.UserID

        Dim intjml As Integer = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetCountNotification", paramtasklist)
        If intjml = 0 Then

            ' Ext.Net.X.Msg.Badge("badgenotificationlist", intjml).Hide()
        Else

            Ext.Net.X.Msg.Badge("badgenotificationlist", intjml).Hide()

        End If
    End Sub

    Sub CountTaskList()

        Dim paramtasklist(6) As SqlClient.SqlParameter
        paramtasklist(0) = New SqlClient.SqlParameter
        paramtasklist(0).ParameterName = "@userid"
        paramtasklist(0).Value = NawaBLL.Common.SessionCurrentUser.UserID

        paramtasklist(1) = New SqlClient.SqlParameter
        paramtasklist(1).ParameterName = "@groupmenuid"
        paramtasklist(1).Value = NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID

        paramtasklist(2) = New SqlClient.SqlParameter
        paramtasklist(2).ParameterName = "@actionid"
        paramtasklist(2).Value = NawaBLL.Common.ModuleActionEnum.Approval

        paramtasklist(3) = New SqlClient.SqlParameter
        paramtasklist(3).ParameterName = "@orderby"
        paramtasklist(3).Value = "Jumlah"

        paramtasklist(4) = New SqlClient.SqlParameter
        paramtasklist(4).ParameterName = "@whereclause"
        paramtasklist(4).Value = ""

        paramtasklist(5) = New SqlClient.SqlParameter
        paramtasklist(5).ParameterName = "@PageIndex"
        paramtasklist(5).Value = 0

        paramtasklist(6) = New SqlClient.SqlParameter
        paramtasklist(6).ParameterName = "@PageSize"
        paramtasklist(6).Value = Integer.MaxValue

        Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskList", paramtasklist)

        Dim total As Integer = 0
        For Each item As DataRow In DataPaging.Rows
            total += item("Jumlah")
        Next

        If total = 0 Then

            'badgetasklist.Text = ""

            Ext.Net.X.Msg.Badge("btntaskList", total).Hide()
        Else

            Ext.Net.X.Msg.Badge("btntaskList", total).Show()

        End If

    End Sub

    'Protected Sub UpdateTaskNotification(sender As Object, e As DirectEventArgs)
    '    Try

    '        CountTaskList()
    '        CountNotification()
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub

    <DirectMethod>
    Public Sub SetSessionShowFalse()
        Session("Show") = False
    End Sub

    Protected Sub BtnMenu_Click(sender As Object, e As DirectEventArgs)
        Try

            'If MenuPanel1. Then
            '    MenuPanel1.Hide()
            'Else
            '    MenuPanel1.Show()
            'End If

            If Session("Show") = True Then
                MenuPanel1.Hide()
                Session("Show") = False
            Else
                MenuPanel1.Show()
                Session("Show") = True

            End If

            'If MenuPanel1.Hidden = False Then
            '    MenuPanel1.Hidden = True

            'Else
            '    MenuPanel1.Hidden = True
            'End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    <DirectMethod>
    Public Sub hidemenu()

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub

#Region "Setting Branch"

    Protected Sub BtnSaveSLIK_DirectEvent(sender As Object, e As DirectEventArgs)
        Try
            Dim objnewparam As NawaDevDAL.SettingPersonal = NawaDevBLL.SLIKParameterBLL.getSettingSLIKPersonal(NawaBLL.Common.SessionCurrentUser.UserID)
            Dim objslikparameter As NawaDevDAL.SLIKParameter = NawaDevBLL.SLIKParameterBLL.getSlikparameter
            If objnewparam Is Nothing Then
                objnewparam = New NawaDevDAL.SettingPersonal
                objnewparam.ReportDate = Date.Now
            End If
            objnewparam.ReportDate = txtPeriod.Text
            objnewparam.UserID = NawaBLL.Common.SessionCurrentUser.UserID
            objnewparam.KodeCabang = cboKantorCabang.Value

            NawaDevBLL.SLIKParameterBLL.SaveSettingToDb(objnewparam)
            NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal = objnewparam
            windowSetting.Hidden = True
            Dim objbarslik As ToolbarTextItem = TopPanel.FindControl("barSLIKParameter")

            If Not NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal Is Nothing Then
                objbarslik.Text = " Period : " & NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.ReportDate.ToString("dd-MMM-yyyy") & " | Branch : " & NawaDevBLL.SLIKParameterBLL.getCabangName(NawaDevBLL.SLIKParameterBLL.SessionSettingSLIKPersonal.KodeCabang)
            Else
                objbarslik.Text = " Period :  [Not Selected] | Branch : [Not Selected]"
            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub StoreBranch_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim query As String = e.Parameters("query")
            If query Is Nothing Then query = ""


            Dim strfilter As String = ""
            If query.Length > 0 Then
                strfilter = " NamaCabang like '%" & query & "%'"
            End If

            ''Indra Bu 15 April 2019:
            ''Sementara ditutup dulu
            'If strfilter = "" Then
            '    strfilter += " kode IN (SELECT  mbu.FK_Branch_ID FROM MappingBranchUser mbu WHERE mbu.UserID='" & NawaBLL.Common.SessionCurrentUser.UserID & "')"
            'Else
            '    strfilter += " and kode IN (SELECT  mbu.FK_Branch_ID FROM MappingBranchUser mbu WHERE mbu.UserID='" & NawaBLL.Common.SessionCurrentUser.UserID & "')"
            'End If

            StoreBranch.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("vw_KodeCabangActive", "Kode,NamaCabang", strfilter, "Kode", e.Start, e.Limit, e.Total)
            StoreBranch.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    <DirectMethod>
    Sub BtnSettingBranchAndPeriod_DirectClick()
        Try
            windowSetting.Hidden = False
            If StoreBranch.DataSource = Nothing Then
                StoreBranch.DataSource = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.Text, "SELECT Kode, NamaCabang FROM vw_KodeCabangActive", Nothing)
            End If

            Dim objnewparam As NawaDevDAL.SettingPersonal = NawaDevBLL.SLIKParameterBLL.getSettingSLIKPersonal(NawaBLL.Common.SessionCurrentUser.UserID)
            If objnewparam Is Nothing Then
                cboKantorCabang.Value = ""
                txtPeriod.SetValue("")
            Else
                cboKantorCabang.ClearValue()
                cboKantorCabang.DoQuery(objnewparam.KodeCabang, True)
                cboKantorCabang.SetValueAndFireSelect(objnewparam.KodeCabang)

                txtPeriod.SetValue(objnewparam.ReportDate)
                BtnSaveSLIK.Focus()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

#End Region

    <DirectMethod>
    Sub BtnChangePassword_DirectClick()
        Try
            Ext.Net.X.Redirect("ChangePassword.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    <DirectMethod>
    Sub btnLogout_DirectClick()
        Try
            Ext.Net.X.Redirect("Logout.aspx")
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    <DirectMethod>
    Sub btntaskList_DirectClick()
        Try

            Dim apppath As String = Request.ApplicationPath
            If apppath = "/" Then apppath = ""

            Dim objMOdule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleName("TaskList")
            If Not objMOdule Is Nothing Then
                Ext.Net.X.AddScript("loadPageInButton(#{Panel9}, '" & Guid.NewGuid.ToString & "','" & apppath & objMOdule.UrlView & "?ModuleID=" & NawaBLL.Common.EncryptQueryString(objMOdule.PK_Module_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey) & "','" & objMOdule.ModuleLabel & "'  ,#{MenuPanel1},#{BtnMenu})")
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    '<DirectMethod>
    'Sub btnnotification_DirectClick()
    '    Try

    '        Dim apppath As String = Request.ApplicationPath
    '        If apppath = "/" Then apppath = ""

    '        Dim objMOdule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleName("Notification")
    '        If Not objMOdule Is Nothing Then
    '            Ext.Net.X.AddScript("loadPageInButton(#{Panel9}, '" & Guid.NewGuid.ToString & "','" & apppath & objMOdule.UrlView & "?ModuleID=" & NawaBLL.Common.EncryptQueryString(objMOdule.PK_Module_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey) & "','" & objMOdule.ModuleLabel & "'  ,#{MenuPanel1},#{BtnMenu})")
    '        End If
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try

    'End Sub

    <DirectMethod>
    Public Function AddSubMenu(ByVal parentid As String) As String


        Dim objmenuitem As New Ext.Net.NodeCollection


        Dim ObjWebPage As New System.Web.UI.Page
        'Using objdb As New NawaDataEntities



        Dim apppath As String = Request.ApplicationPath
        If apppath = "/" Then apppath = ""



        Dim objlistGroupmenusetting As List(Of MGroupMenuSettting)
        objlistGroupmenusetting = ObjWebPage.Session("DataMenu")
        '    Dim objlistGroupmenusetting As System.Data.Entity.Core.Objects.ObjectResult(Of MGroupMenuSettting) = objdb.usp_GetMgroupMenuSettingByGroupMenuID1(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID)

        Dim listdata As List(Of MGroupMenuSettting) = objlistGroupmenusetting.ToList.Where(Function(x) x.mMenuParentID = parentid).ToList.OrderBy(Function(x) x.urutan).ToList

        Dim unikkey As String = Guid.NewGuid.ToString

        Dim strEncriptuser As String = ""

        Dim objSupportEncript As SystemParameter

        If NawaBLL.Common.SessionSystemParameter28 Is Nothing Then
            NawaBLL.Common.SessionSystemParameter28 = SystemParameterBLL.GetSystemParameterByPk(28)

        End If
        objSupportEncript = NawaBLL.Common.SessionSystemParameter28
        If Not objSupportEncript Is Nothing Then
            '  strEncriptuser = JsonWebToken.GetJwtToken(NawaBLL.Common.SessionCurrentUser.UserID)
        End If

        For Each item As MGroupMenuSettting In listdata.ToList




            Dim oMenuitem As New Ext.Net.Node
            oMenuitem.NodeID = item.mMenuID
            oMenuitem.Text = item.mMenuLabel

            Dim redirecturl As String = ""

            If item.FK_Module_ID.HasValue Then
                Dim intmodulid As Integer = item.FK_Module_ID
                redirecturl = item.mMenuURL

                'If redirecturl.Contains("?") Then
                '    redirecturl = String.Format(item.mMenuURL.ToLower.Replace("$userid$", strDecriptuser) & "&ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                'Else
                '    redirecturl = String.Format(item.mMenuURL.ToLower.Replace("$userid$", strDecriptuser) & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                'End If

                If redirecturl.Contains("?") Then
                    If Not objSupportEncript Is Nothing Then
                        redirecturl = String.Format(item.mMenuURL.ToLower.Replace("$userid$", strEncriptuser) & "&ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                    Else
                        redirecturl = String.Format(item.mMenuURL & "&ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                    End If
                Else
                    If Not objSupportEncript Is Nothing Then
                        redirecturl = String.Format(item.mMenuURL.ToLower.Replace("$userid$", strEncriptuser) & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))
                    Else
                        redirecturl = String.Format(item.mMenuURL & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(intmodulid, NawaBLL.SystemParameterBLL.GetEncriptionKey))

                    End If

                End If
            Else
                redirecturl = item.mMenuURL

            End If

            If redirecturl <> "" Then

                Dim strparam As NawaDAL.SystemParameter

                If NawaBLL.Common.SessionSystemParameter3001 Is Nothing Then
                    NawaBLL.Common.SessionSystemParameter3001 = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3001)
                End If
                strparam = NawaBLL.Common.SessionSystemParameter3001
                Dim listOfCompare As New List(Of String)
                If Not strparam Is Nothing Then
                    listOfCompare = strparam.SettingValue.Split(";").ToList
                End If

                Dim result As String = listOfCompare.FirstOrDefault(Function(x) redirecturl.ToLower.Contains(x.ToLower))
                If result <> String.Empty Then
                    oMenuitem.Qtip = redirecturl

                Else
                    oMenuitem.Qtip = apppath & redirecturl

                End If

                oMenuitem.Href = "#"
                ' oMenuitem.CustomAttributes.Add(New ConfigItem("hash", Guid.NewGuid.ToString))
                oMenuitem.Qtip = item.mMenuLabel
            Else
                oMenuitem.Qtip = ""
            End If

            If objlistGroupmenusetting.ToList.Where(Function(x) x.mMenuParentID = item.mMenuID).Count = 0 Then
                oMenuitem.Leaf = True
            End If

            'If redirecturl <> "" Then
            '    'oMenuitem.Href = apppath & redirecturl
            '    oMenuitem.Listeners.Click.Handler = "addTab(#{Panel9}, '" & Guid.NewGuid.ToString & "', '" & apppath & redirecturl & "', this,'" & oMenuitem.Text & "');"
            '    oMenuitem.DirectEvents.Click.EventMask.Msg = "Loading.."
            '    oMenuitem.DirectEvents.Click.EventMask.MinDelay = 500
            'Else
            '    'oMenuitem.Href = redirecturl
            '    oMenuitem.Listeners.Click.Handler = "addTab(#{Panel9}, '" & Guid.NewGuid.ToString & "', '" & redirecturl & "',this,'" & oMenuitem.Text & "');"
            '    oMenuitem.DirectEvents.Click.EventMask.Msg = "Loading.."
            '    oMenuitem.DirectEvents.Click.EventMask.MinDelay = 500
            'End If

            'AddSubMenu(oMenuitem, item, apppath)
            objmenuitem.Add(oMenuitem)

        Next


        Return objmenuitem.ToJson
        ' End Using

    End Function
End Class