Imports NawaBLL
Imports NawaDAL

Partial Class Login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not IsPostBack Then
                If Not NawaBLL.Common.SessionCurrentUser Is Nothing Then
                    Ext.Net.X.Redirect("Default.aspx")
                ElseIf Request.Params("ReturnUrl") <> "" Then
                    Ext.Net.X.Js.Call("parent.NawadataDirect.RedirectLogout")
                End If
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub





    'Function AutenticateScorpion(ldapdomain As String, applicationid As System.Guid, userid As String, password As String) As Boolean

    '    Dim objlogin As New Scorpion.Security
    '    Try

    '        Dim objresult As String = objlogin.Login(applicationid, ldapdomain, userid, password)
    '        'If objresult = "" Then


    '        'Else

    '        Dim strgroupmenu As String = objlogin.GetRole(applicationid, ldapdomain, userid)
    '        strgroupmenu = strgroupmenu.Trim

    '        NawaDevBLL.IFTIBLL.Createuser(userid, strgroupmenu)

    '        Dim ouser As MUser = NawaBLL.MUserBLL.GetMuserbyUSerId(userid)
    '        If Not ouser Is Nothing Then
    '            Dim strgroupmenuserver As String = NawaBLL.MGroupMenuBLL.GetGroupMenuNameByID(ouser.FK_MGroupMenu_ID.GetValueOrDefault(0))

    '            If strgroupmenu.ToLower.Trim = strgroupmenuserver.ToLower Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        Else
    '            Return False
    '        End If

    '        'End If
    '    Catch ex As Exception
    '        Throw
    '    End Try


    'End Function
    Function AutenticateScorpion(ldapdomain As String, applicationid As System.Guid, userid As String, password As String) As Boolean

        Dim objlogin As New Scorpion.WebService1
        Try
            Dim objresult As String = objlogin.Login(applicationid.ToString, ldapdomain, userid, password)
            If objresult = "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try


    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.ServerClick
        Try
            Dim objsysparam As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(NawaBLL.SystemParameterBLL.SytemParameterEnum.LoginUsedIPAddress)
            Dim IsCheckIPAddress As Boolean = True
            If Not objsysparam Is Nothing Then
                IsCheckIPAddress = objsysparam.SettingValue
            End If
            Using objLoginBll As New LoginBLL
                If objLoginBll.AplicationAuthentication = LoginBLL.AplicationAuthenticationEnum.Window Then
                    'done: cek ke AD
                    '         Dim struseridapp As String = LoginBLL.GetUserAppByUserLDAP(txtUsername.Text.Trim)
                    Dim objUser As MUser = LoginBLL.GetMuserByUserID(txtUsername.Text.Trim)

                    ' Dim strApplicationid As System.Guid = New Guid(SystemParameterBLL.GetSystemParameterByPk(21).SettingValue)
                    Dim strLDAPConnection As String = SystemParameterBLL.GetSystemParameterByPk(21).SettingValue
                    Dim strLDAPDomain As String = SystemParameterBLL.GetSystemParameterByPk(22).SettingValue

                    Dim strerror As String = ""
                    If Not objUser Is Nothing Then
                        If objUser.PK_MUser_ID <> 1 Then
                            Dim dLastLogin As Date
                            If objUser.LastLogin.HasValue Then
                                dLastLogin = objUser.LastLogin
                            Else
                                dLastLogin = objUser.CreatedDate
                            End If
                            'Done: cek last login apakah melebihi batas parameter Account Lock after Not Used (days)
                            NawaBLL.LoginBLL.CekParameterLoginAccountLock(objUser, dLastLogin)
                        End If
                        'Done:Validasi user disabled
                        If objUser.IsDisabled.GetValueOrDefault(False) Then
                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9000)
                            If Not objparam Is Nothing Then
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                Throw New Exception(objparam.SettingValue)
                            Else
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User cannot login because user has already been disabled.")
                                Throw New Exception("User cannot login because user has already been disabled.")
                            End If
                        End If
                        If Not objUser.Active.GetValueOrDefault(False) Then
                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9001)
                            If Not objparam Is Nothing Then
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                Throw New Exception(objparam.SettingValue)
                            Else
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User cannot login because user has already been inactive.")
                                Throw New Exception("User cannot login because user has already been inactive.")
                            End If
                        End If
                        'Done:Validasi group menu disabled
                        NawaBLL.LoginBLL.IsGroupMenuActive(objUser)
                        'Done:Validasi role disabled
                        NawaBLL.LoginBLL.IsRoleActive(objUser)
                        'done:Validasi n salah password
                        If objUser.PK_MUser_ID = 1 Then
                            Dim Salt As String = objUser.UserPasswordSalt
                            If objUser.UserPasword = NawaBLL.Common.Encrypt(txtPassword.Text.Trim, Salt) Then
                                If objUser.PK_MUser_ID = 1 OrElse (Now.Subtract(Convert.ToDateTime(objUser.LastChangePassword)).Days <= NawaBLL.LoginBLL.GetExpiredPasswordDay) Then
                                    If IsCheckIPAddress Then
                                        If objUser.PK_MUser_ID = 1 Or ((Convert.ToBoolean(objUser.InUsed) And objUser.IPAddress = Me.Request.UserHostAddress) Or Not Convert.ToBoolean(objUser.InUsed)) Then
                                            Dim intjmlalternatetask As Integer = NawaBLL.LoginBLL.IsHaveAlternateTask(objUser.UserID)
                                            If intjmlalternatetask > 0 Then
                                                'ada alternate,jadi harus pilih
                                                NawaBLL.LoginBLL.SetTempAlternate(objUser)
                                                Dim strpath As String = Request.ApplicationPath
                                                If strpath = "/" Then strpath = ""
                                                NawaBLL.Common.GetApplicationPath = strpath
                                                Dim StrRedirectNext As String = FormsAuthentication.GetRedirectUrl("", True).ToLower
                                                Dim StrRedirect As String = ""
                                                If StrRedirectNext <> "" Then
                                                    StrRedirect = "ChooseAlternateView.aspx?RedirectURL=" & StrRedirectNext.Replace(strpath.ToLower, "")
                                                Else
                                                    StrRedirect = "ChooseAlternateView.aspx"
                                                End If
                                                'Me.Response.Redirect(StrRedirect, False)
                                                Ext.Net.X.Redirect(StrRedirect)
                                            Else
                                                NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                                Dim strpath As String = Request.ApplicationPath
                                                If strpath = "/" Then strpath = ""
                                                NawaBLL.Common.GetApplicationPath = strpath
                                                Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                                Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                                Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                                If indexquestion > 0 Then
                                                    strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                                End If
                                                If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                                    StrRedirect = "Default.aspx"
                                                Else
                                                    StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                                End If
                                                FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                                'Me.Response.Redirect(StrRedirect, False)
                                                Ext.Net.X.Redirect(StrRedirect)
                                            End If
                                        Else
                                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9002)
                                            If Not objparam Is Nothing Then
                                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                                Throw New Exception(objparam.SettingValue)
                                            Else
                                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Login failed. User Name is being used on a another computer.")
                                                Throw New Exception("Login failed. User Name is being used on a another computer.")
                                            End If
                                        End If
                                    Else
                                        Dim intjmlalternatetask As Integer = NawaBLL.LoginBLL.IsHaveAlternateTask(objUser.UserID)
                                        If intjmlalternatetask > 0 Then
                                            'ada alternate,jadi harus pilih
                                            NawaBLL.LoginBLL.SetTempAlternate(objUser)
                                            Dim strpath As String = Request.ApplicationPath
                                            If strpath = "/" Then strpath = ""
                                            NawaBLL.Common.GetApplicationPath = strpath
                                            Dim StrRedirectNext As String = FormsAuthentication.GetRedirectUrl("", True).ToLower
                                            Dim StrRedirect As String = ""
                                            If StrRedirectNext <> "" Then
                                                StrRedirect = "ChooseAlternateView.aspx?RedirectURL=" & StrRedirectNext.Replace(strpath.ToLower, "")
                                            Else
                                                StrRedirect = "ChooseAlternateView.aspx"
                                            End If
                                            'Me.Response.Redirect(StrRedirect, False)
                                            Ext.Net.X.Redirect(StrRedirect)
                                        Else
                                            NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                            Dim strpath As String = Request.ApplicationPath.ToLower
                                            If strpath = "/" Then strpath = ""
                                            NawaBLL.Common.GetApplicationPath = strpath
                                            Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                            Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                            Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                            If indexquestion > 0 Then
                                                strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                            End If
                                            If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                                StrRedirect = "Default.aspx"
                                            Else
                                                StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                            End If
                                            FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                            'Me.Response.Redirect(StrRedirect, False)
                                            Ext.Net.X.Redirect(StrRedirect)
                                        End If
                                    End If
                                Else
                                    'Done: force change password
                                    FormsAuthentication.SetAuthCookie("ForceChangePassword", False)
                                    Dim strpkuserid As String = NawaBLL.Common.EncryptQueryString(objUser.PK_MUser_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                                    'Response.Redirect("ForceChangePassword.aspx?PKUserID=" & strpkuserid, False)
                                    Ext.Net.X.Redirect("ForceChangePassword.aspx?PKUserID=" & strpkuserid)
                                End If
                            Else
                                NawaBLL.Common.GetPasswordRetry(objUser.UserID) += 1
                                If objUser.PK_MUser_ID <> 1 AndAlso NawaBLL.Common.GetPasswordRetry(objUser.UserID) >= NawaBLL.LoginBLL.GetAccountLockWrongPassword Then
                                    NawaBLL.LoginBLL.DisabledUser(objUser)
                                    NawaBLL.Common.GetPasswordRetry(objUser.UserID) = 0
                                    Throw New Exception("Password retry limit exceeded. The User has been disabled. Please contact your administrator.")
                                End If
                                Throw New Exception(" User or Password is not valid.")
                            End If
                        Else
                            'kalau bukan sysadmin ,cek AD


                            ' If AutenticateScorpion(strLDAPDomain, strApplicationid, txtUsername.Text.Trim, txtPassword.Text.Trim) Then
                            If NawaDevBLL.ActiveDirectoryBLL.AuthenticateUser(strLDAPDomain, txtUsername.Text.Trim, txtPassword.Text.Trim, strLDAPConnection, strerror) Then
                                If IsCheckIPAddress Then
                                    If ((Convert.ToBoolean(objUser.InUsed) And objUser.IPAddress = Me.Request.UserHostAddress) Or Not Convert.ToBoolean(objUser.InUsed)) Then
                                        'valid login ad
                                        NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                        Dim strpath As String = Request.ApplicationPath
                                        If strpath = "/" Then strpath = ""
                                        NawaBLL.Common.GetApplicationPath = strpath
                                        Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                        Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                        Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                        If indexquestion > 0 Then
                                            strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                        End If
                                        If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                            StrRedirect = "Default.aspx"
                                        Else
                                            StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                        End If
                                        FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                        'Me.Response.Redirect(StrRedirect, False)
                                        Ext.Net.X.Redirect(StrRedirect)
                                    Else
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Login failed. User Name is being used on a another computer.")
                                        Throw New Exception("Login failed. User Name is being used on a another computer.")
                                    End If
                                Else
                                    'valid login ad
                                    NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                    Dim strpath As String = Request.ApplicationPath
                                    If strpath = "/" Then strpath = ""
                                    NawaBLL.Common.GetApplicationPath = strpath
                                    Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                    Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                    Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                    If indexquestion > 0 Then
                                        strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                    End If
                                    If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                        StrRedirect = "Default.aspx"
                                    Else
                                        StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                    End If
                                    FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                    'Me.Response.Redirect(StrRedirect, False)
                                    Ext.Net.X.Redirect(StrRedirect)
                                End If
                            Else
                                'bukan

                                NawaBLL.Common.GetPasswordRetry(objUser.UserID) += 1
                                If objUser.PK_MUser_ID <> 1 AndAlso NawaBLL.Common.GetPasswordRetry(objUser.UserID) >= NawaBLL.LoginBLL.GetAccountLockWrongPassword Then
                                    NawaBLL.LoginBLL.DisabledUser(objUser)
                                    NawaBLL.Common.GetPasswordRetry(objUser.UserID) = 0
                                    Dim objparamx As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9003)
                                    If Not objparamx Is Nothing Then
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparamx.SettingValue)
                                        Throw New Exception(objparamx.SettingValue)
                                    Else
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Password retry limit exceeded. The User has been disabled. Please contact your administrator.")
                                        Throw New Exception("Password retry limit exceeded. The User has been disabled. Please contact your administrator")
                                    End If
                                End If
                                Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9004)
                                If Not objparam Is Nothing Then
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                    Throw New Exception(objparam.SettingValue)
                                Else
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User Or  Password Is Not valid.")
                                    Throw New Exception("User Or  Password Is Not valid")
                                End If


                            End If
                        End If
                    Else

                        '

                        ' If AutenticateScorpion(strLDAPDomain, strApplicationid, txtUsername.Text.Trim, txtPassword.Text.Trim) Then
                        If NawaDevBLL.ActiveDirectoryBLL.AuthenticateUser(strLDAPDomain, txtUsername.Text.Trim, txtPassword.Text.Trim, strLDAPConnection, strerror) Then
                            objUser = LoginBLL.GetMuserByUserID(txtUsername.Text.Trim)

                            If Not objUser Is Nothing Then



                                If IsCheckIPAddress Then
                                    If ((Convert.ToBoolean(objUser.InUsed) And objUser.IPAddress = Me.Request.UserHostAddress) Or Not Convert.ToBoolean(objUser.InUsed)) Then
                                        'valid login ad
                                        NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                        Dim strpath As String = Request.ApplicationPath
                                        If strpath = "/" Then strpath = ""
                                        NawaBLL.Common.GetApplicationPath = strpath
                                        Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                        Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                        Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                        If indexquestion > 0 Then
                                            strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                        End If
                                        If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                            StrRedirect = "Default.aspx"
                                        Else
                                            StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                        End If
                                        FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                        'Me.Response.Redirect(StrRedirect, False)
                                        Ext.Net.X.Redirect(StrRedirect)
                                    Else
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Login failed. User Name is being used on a another computer.")
                                        Throw New Exception("Login failed. User Name is being used on a another computer.")
                                    End If
                                Else
                                    'valid login ad
                                    NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                    Dim strpath As String = Request.ApplicationPath
                                    If strpath = "/" Then strpath = ""
                                    NawaBLL.Common.GetApplicationPath = strpath
                                    Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                    Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                    Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                    If indexquestion > 0 Then
                                        strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                    End If
                                    If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                        StrRedirect = "Default.aspx"
                                    Else
                                        StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                    End If
                                    FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                    'Me.Response.Redirect(StrRedirect, False)
                                    Ext.Net.X.Redirect(StrRedirect)
                                End If
                            Else
                                'bukan

                                NawaBLL.Common.GetPasswordRetry(objUser.UserID) += 1
                                If objUser.PK_MUser_ID <> 1 AndAlso NawaBLL.Common.GetPasswordRetry(objUser.UserID) >= NawaBLL.LoginBLL.GetAccountLockWrongPassword Then
                                    NawaBLL.LoginBLL.DisabledUser(objUser)
                                    NawaBLL.Common.GetPasswordRetry(objUser.UserID) = 0
                                    Dim objparamx As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9003)
                                    If Not objparamx Is Nothing Then
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparamx.SettingValue)
                                        Throw New Exception(objparamx.SettingValue)
                                    Else
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Password retry limit exceeded. The User has been disabled. Please contact your administrator.")
                                        Throw New Exception("Password retry limit exceeded. The User has been disabled. Please contact your administrator")
                                    End If
                                End If
                                Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9004)
                                If Not objparam Is Nothing Then
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                    Throw New Exception(objparam.SettingValue)
                                Else
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User Or  Password Is Not valid.")
                                    Throw New Exception("User Or  Password Is Not valid")
                                End If


                            End If
                        Else

                            Throw New Exception("User " & txtUsername.Text.Trim & " has not been found. Please contact your administrator to create your account first or Configure LDAP first.")

                        End If


                    End If
                ElseIf objLoginBll.AplicationAuthentication = LoginBLL.AplicationAuthenticationEnum.Form Then
                    Dim objUser As MUser = LoginBLL.GetMuserByUserID(txtUsername.Text.Trim)
                    If Not objUser Is Nothing Then
                        If objUser.PK_MUser_ID <> 1 Then
                            Dim dLastLogin As Date
                            If objUser.LastLogin.HasValue Then
                                dLastLogin = objUser.LastLogin
                            Else
                                dLastLogin = objUser.CreatedDate
                            End If
                            'Done: cek last login apakah melebihi batas parameter Account Lock after Not Used (days)
                            NawaBLL.LoginBLL.CekParameterLoginAccountLock(objUser, dLastLogin)
                        End If
                        'Done:Validasi user disabled
                        If objUser.IsDisabled.GetValueOrDefault(False) Then
                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9000)
                            If Not objparam Is Nothing Then
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                Throw New Exception(objparam.SettingValue)
                            Else
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User cannot login because user has already been disabled.")
                                Throw New Exception("User cannot login because user has already been disabled.")
                            End If
                        End If
                        'Done:Validasi user activation disabled
                        If Not objUser.Active.GetValueOrDefault(False) Then
                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9001)
                            If Not objparam Is Nothing Then
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                Throw New Exception(objparam.SettingValue)
                            Else
                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User cannot login because user has already been inactive.")
                                Throw New Exception("User cannot login because user has already been inactive.")
                            End If
                        End If
                        'Done:Validasi group menu disabled
                        NawaBLL.LoginBLL.IsGroupMenuActive(objUser)
                        'Done:Validasi role disabled
                        NawaBLL.LoginBLL.IsRoleActive(objUser)
                        'done:Validasi n salah password
                        If objUser.PK_MUser_ID = 1 OrElse NawaBLL.Common.GetPasswordRetry(objUser.UserID) < NawaBLL.LoginBLL.GetAccountLockWrongPassword Then
                            Dim Salt As String = objUser.UserPasswordSalt
                            If objUser.UserPasword = NawaBLL.Common.Encrypt(txtPassword.Text.Trim, Salt) Then
                                If objUser.PK_MUser_ID = 1 OrElse (Now.Subtract(Convert.ToDateTime(objUser.LastChangePassword)).Days <= NawaBLL.LoginBLL.GetExpiredPasswordDay) Then
                                    If IsCheckIPAddress Then
                                        If objUser.PK_MUser_ID = 1 Or ((Convert.ToBoolean(objUser.InUsed) And objUser.IPAddress = Me.Request.UserHostAddress) Or Not Convert.ToBoolean(objUser.InUsed)) Then
                                            Dim intjmlalternatetask As Integer = NawaBLL.LoginBLL.IsHaveAlternateTask(objUser.UserID)
                                            If intjmlalternatetask > 0 Then
                                                NawaBLL.LoginBLL.SetTempAlternate(objUser)
                                                Dim strpath As String = Request.ApplicationPath
                                                If strpath = "/" Then strpath = ""
                                                NawaBLL.Common.GetApplicationPath = strpath
                                                Dim StrRedirectNext As String = FormsAuthentication.GetRedirectUrl("", True).ToLower
                                                Dim StrRedirect As String = ""
                                                If StrRedirectNext <> "" Then
                                                    StrRedirect = "ChooseAlternateView.aspx?RedirectURL=" & StrRedirectNext.Replace(strpath.ToLower, "")
                                                Else
                                                    StrRedirect = "ChooseAlternateView.aspx"
                                                End If
                                                'Me.Response.Redirect(StrRedirect, False)
                                                Ext.Net.X.Redirect(StrRedirect)
                                            Else
                                                NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                                Dim strpath As String = Request.ApplicationPath
                                                If strpath = "/" Then strpath = ""
                                                NawaBLL.Common.GetApplicationPath = strpath
                                                Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                                Dim strpathtocheck As String = StrRedirect.Replace(strpath.ToLower, "")
                                                Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                                If indexquestion > 0 Then
                                                    strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                                End If
                                                If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                                    StrRedirect = "Default.aspx"
                                                Else
                                                    StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                                End If
                                                FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                                'Me.Response.Redirect(StrRedirect, False)
                                                Ext.Net.X.Redirect(StrRedirect)
                                            End If
                                        Else
                                            Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9002)
                                            If Not objparam Is Nothing Then
                                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                                Throw New Exception(objparam.SettingValue)
                                            Else
                                                LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Login failed. User Name is being used on a another computer.")
                                                Throw New Exception("Login failed. User Name is being used on a another computer.")
                                            End If
                                        End If
                                    Else
                                        'cek kalau ada alternate,maka redirect pilih user yg mau dipakai
                                        Dim intjmlalternatetask As Integer = NawaBLL.LoginBLL.IsHaveAlternateTask(objUser.UserID)
                                        If intjmlalternatetask > 0 Then
                                            NawaBLL.LoginBLL.SetTempAlternate(objUser)
                                            Dim strpath As String = Request.ApplicationPath
                                            If strpath = "/" Then strpath = ""
                                            NawaBLL.Common.GetApplicationPath = strpath
                                            Dim StrRedirectNext As String = FormsAuthentication.GetRedirectUrl("", True).ToLower
                                            Dim StrRedirect As String = ""
                                            If StrRedirectNext <> "" Then
                                                StrRedirect = "ChooseAlternateView.aspx?RedirectURL=" & StrRedirectNext.Replace(strpath.ToLower, "")
                                            Else
                                                StrRedirect = "ChooseAlternateView.aspx"
                                            End If
                                            'Me.Response.Redirect(StrRedirect, False)
                                            Ext.Net.X.Redirect(StrRedirect)
                                        Else
                                            'tidak ada alternate, jadi langsung login
                                            NawaBLL.LoginBLL.SetCurrentUserLogin(objUser, Request.UserHostAddress)
                                            Dim strpath As String = Request.ApplicationPath
                                            If strpath = "/" Then strpath = ""
                                            NawaBLL.Common.GetApplicationPath = strpath
                                            Dim StrRedirect As String = FormsAuthentication.GetRedirectUrl(NawaBLL.Common.SessionCurrentUser.UserID, True).ToLower
                                            Dim strpathtocheck As String = ""
                                            If strpath <> "" Then
                                                strpathtocheck = StrRedirect.Replace(strpath.ToLower, "")
                                            End If


                                            Dim indexquestion As String = strpathtocheck.IndexOf("?")
                                            If indexquestion > 0 Then
                                                strpathtocheck = strpathtocheck.Substring(0, indexquestion)
                                            End If
                                            If Not NawaBLL.LoginBLL.IsAuthenticatePage(strpathtocheck, NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID) Then
                                                StrRedirect = "Default.aspx"
                                            Else
                                                StrRedirect = "Default.aspx?RedirectURL=" & StrRedirect.Replace(strpath.ToLower, "")
                                            End If
                                            FormsAuthentication.SetAuthCookie(NawaBLL.Common.SessionCurrentUser.UserID, False)
                                            Ext.Net.X.Redirect(StrRedirect)
                                            'Me.Response.Redirect(StrRedirect, False)
                                        End If
                                    End If
                                Else
                                    'Done: force change password
                                    FormsAuthentication.SetAuthCookie("ForceChangePassword", False)
                                    Dim strpkuserid As String = NawaBLL.Common.EncryptQueryString(objUser.PK_MUser_ID, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                                    'Response.Redirect("ForceChangePassword.aspx?PKUserID=" & strpkuserid)
                                    Ext.Net.X.Redirect("ForceChangePassword.aspx?PKUserID=" & strpkuserid)
                                End If
                            Else
                                NawaBLL.Common.GetPasswordRetry(objUser.UserID) += 1
                                If objUser.PK_MUser_ID <> 1 AndAlso NawaBLL.Common.GetPasswordRetry(objUser.UserID) >= NawaBLL.LoginBLL.GetAccountLockWrongPassword Then
                                    NawaBLL.LoginBLL.DisabledUser(objUser)
                                    NawaBLL.Common.GetPasswordRetry(objUser.UserID) = 0
                                    Dim objparamx As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9003)
                                    If Not objparamx Is Nothing Then
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparamx.SettingValue)
                                        Throw New Exception(objparamx.SettingValue)
                                    Else
                                        LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "Password retry limit exceeded. The User has been disabled. Please contact your administrator.")
                                        Throw New Exception("Password retry limit exceeded. The User has been disabled. Please contact your administrator")
                                    End If
                                End If
                                Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9004)
                                If Not objparam Is Nothing Then
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                                    Throw New Exception(objparam.SettingValue)
                                Else
                                    LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User Or  Password Is Not valid.")
                                    Throw New Exception("User Or  Password Is Not valid")
                                End If
                            End If
                        End If
                    Else
                        Dim objparam As NawaDAL.SystemParameter = SystemParameterBLL.GetSystemParameterByPk(9005)
                        If Not objparam Is Nothing Then
                            LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, objparam.SettingValue)
                            Throw New Exception(objparam.SettingValue)
                        Else
                            LoginBLL.Setuserfaillogin(txtUsername.Text.Trim, Me.Request.UserHostAddress, "User has Not been found. Please contact your administrator to create your account first.")
                            Throw New Exception("User has Not been found. Please contact your administrator to create your account first.")
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = ex.Message
            'Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
