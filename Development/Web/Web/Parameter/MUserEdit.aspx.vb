Imports Ext.Net
Partial Class MUserEdit
    Inherits Parent

    Public objMuserBll As NawaBLL.MUserBLL

    Public Property MsEmployee() As Data.DataTable
        Get

            If Session("MUserAdd.MsEmployee") Is Nothing Then
                Dim objdt As New Data.DataTable
                objdt.Columns.Add(New Data.DataColumn("Userid", GetType(String)))
                objdt.Columns.Add(New Data.DataColumn("NIK", GetType(String)))
                objdt.Columns.Add(New Data.DataColumn("Supervisor", GetType(String)))
                objdt.Columns.Add(New Data.DataColumn("Jabatan", GetType(String)))
                Session("MUserAdd.MsEmployee") = objdt
            End If

            Return Session("MUserAdd.MsEmployee")

        End Get
        Set(ByVal value As Data.DataTable)
            Session("MUserAdd.MsEmployee") = value
        End Set
    End Property

    Public Property AuthenticationMode() As Integer
        Get

            If Session("MUserEdit.AuthenticationMode") Is Nothing Then
                Dim objsystemparameter As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3)
                Dim intauthentication As Integer = 2 'default formauthentication=2
                If Not objsystemparameter Is Nothing Then
                    intauthentication = objsystemparameter.SettingValue
                    Session("MUserEdit.AuthenticationMode") = intauthentication
                End If
            End If
            Return Session("MUserEdit.AuthenticationMode")
        End Get
        Set(ByVal value As Integer)
            Session("MUserEdit.AuthenticationMode") = value
        End Set
    End Property


    Public Property ObjMUserDataEdit() As NawaDAL.MUser
        Get
            Return Session("MUserEdit.ObjMUser")
        End Get
        Set(ByVal value As NawaDAL.MUser)
            Session("MUserEdit.ObjMUser") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("MUserEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MUserEdit.ObjModule") = value
        End Set
    End Property

    Private Sub MUserAdd_Init(sender As Object, e As EventArgs) Handles Me.Init
        objMuserBll = New NawaBLL.MUserBLL(FormPanelInput)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            objMuserBll.BentukformEdit()


            If Not Ext.Net.X.IsAjaxRequest Then


                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjMUserDataEdit = NawaBLL.MUserBLL.GetMuserbyPKuserId(id)
                LoadDataUser()
            End If

            If AuthenticationMode = 1 Then 'kalau ldap tambah button kana
                Dim objtextuser As Ext.Net.TextField = Ext.Net.X.GetCmp("UserID")
                Dim objRightButotn As New Ext.Net.Button
                objRightButotn.ID = "btncheckldap"
                objRightButotn.ClientIDMode = ClientIDMode.Static
                objRightButotn.Icon = Icon.UserAdd
                AddHandler objRightButotn.DirectClick, AddressOf btnright_click
                objtextuser.RightButtons.Add(objRightButotn)


                Dim objpassword As TextField = CType(FormPanelInput.FindControl("UserPassword"), Ext.Net.TextField)
                objpassword.Visible = False
                Dim objpasswordsalt As TextField = CType(FormPanelInput.FindControl("UserPaswordReType"), Ext.Net.TextField)
                objpasswordsalt.Visible = False

            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnright_click(sender As Object, e As DirectEventArgs)
        Try


            Dim userid As String = CType(FormPanelInput.FindControl("UserID"), Ext.Net.TextField).Text

            If userid = "" Then
                Throw New Exception("Please enter User ID.")
            End If
            Dim dtresult As Data.DataTable = NawaDevBLL.ActiveDirectoryBLL.CheckUser(userid)

            If dtresult Is Nothing Then
                Ext.Net.X.Msg.Alert("Warning", "User " & userid & " not found in active directory ").Show()
            Else
                CType(FormPanelInput.FindControl("UserName"), Ext.Net.TextField).Text = dtresult.Rows(0).Item("displayname")
                CType(FormPanelInput.FindControl("UserEmailAddress"), Ext.Net.TextField).Text = dtresult.Rows(0).Item("Mail")



            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub LoadDataUser()
        If Not ObjMUserDataEdit Is Nothing Then
            With ObjMUserDataEdit
                CType(FormPanelInput.FindControl("PK_MUser_ID"), DisplayField).Text = .PK_MUser_ID
                CType(FormPanelInput.FindControl("UserID"), TextField).Text = .UserID
                CType(FormPanelInput.FindControl("UserName"), TextField).Text = .UserName
                Try
                    CType(FormPanelInput.FindControl("FK_MRole_ID"), ComboBox).Text = .FK_MRole_ID

                Catch ex As Exception

                End Try
                Try
                    CType(FormPanelInput.FindControl("FK_MGroupMenu_ID"), ComboBox).Text = .FK_MGroupMenu_ID
                Catch ex As Exception

                End Try


                CType(FormPanelInput.FindControl("UserEmailAddress"), TextField).Text = .UserEmailAddress

            End With

        End If

    End Sub

    Sub ClearSession()
        ObjMUserDataEdit = Nothing
        Session("MUserEdit.AuthenticationMode") = Nothing
    End Sub
    Public Function IsDataAddValid()
        Try


            Dim txtUserId As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserID"), Ext.Net.TextField)
            If txtUserId.Text.Trim.Contains("-") Then
                Throw New Exception("User ID can not contain - char")
            End If

            'validate unik userid
            If Not NawaBLL.MUserBLL.IsUserIdUnikEdit(txtUserId.Text.Trim, ObjMUserDataEdit.PK_MUser_ID) Then

                Throw New Exception("Userid " & txtUserId.Text.Trim & " already exists.")
            End If

            'validate userid belum ada di approval

            If NawaBLL.MUserBLL.IsUseridExistInApproval(txtUserId.Text.Trim) Then

                Throw New Exception("Userid " & txtUserId.Text.Trim & " already exists in approval.")
            End If



            If AuthenticationMode <> 1 Then
                Dim objPassword As TextField = CType(FormPanelInput.FindControl("UserPassword"), Ext.Net.TextField)
                Dim objPasswordretype As TextField = CType(FormPanelInput.FindControl("UserPaswordReType"), Ext.Net.TextField)

                If objPassword.Text.Trim.Length > 0 Then
                    If objPasswordretype.Text.Trim <> objPassword.Text.Trim Then
                        Throw New Exception("Password and Retype Password does not match.")
                    End If



                    Dim strMinimumpasswordlength As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(NawaBLL.SystemParameterBLL.SytemParameterEnum.MinimumPasswordLength).SettingValue
                    If Not NawaBLL.MUserBLL.ValidateMinimumPasswordLengh(objPassword.Text.Trim) Then
                        Throw New Exception("Minimum Password Lengh is " & strMinimumpasswordlength & " char")

                    End If



                    If Not NawaBLL.MUserBLL.ValidateLowerAndUpperCase(objPassword.Text.Trim) Then

                        Throw New Exception("Password Must have Lower And Upper Case char")

                    End If

                    If Not NawaBLL.MUserBLL.ValidateAlfaNumericSymbol(objPassword.Text.Trim) Then

                        Throw New Exception("Password Must have Alfa Numeric Symbol")

                    End If
                    'If Not NawaBLL.MUserBLL.ValidateAlfaNumeric(objPassword.Text.Trim) Then

                    '    Throw New Exception("Password Must have Alfa Numeric")

                    'End If





                    If Not NawaBLL.MUserBLL.ValidatePasswordRecycleCount(txtUserId.Text, objPassword.Text.Trim) Then

                        Throw New Exception("Your Password have Ever Used.Please Change to another Password.")
                    End If

                End If

            End If





            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub


    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            If IsDataAddValid() Then



                Dim txtUserId As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserID"), Ext.Net.TextField)
                Dim txtUserName As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserName"), Ext.Net.TextField)
                Dim cboRole As Ext.Net.ComboBox = CType(FormPanelInput.FindControl("FK_MRole_ID"), Ext.Net.ComboBox)
                Dim cboGroup As Ext.Net.ComboBox = CType(FormPanelInput.FindControl("FK_MGroupMenu_ID"), Ext.Net.ComboBox)
                Dim txtUserPassword As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserPassword"), Ext.Net.TextField)





                Dim txtEmailAddress As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserEmailAddress"), Ext.Net.TextField)
                Dim strpassword As String = ""
                Dim strsaltdefault As String = ""


                With ObjMUserDataEdit
                    Dim oRand As New Random

                    .UserID = txtUserId.Text.Trim
                    .UserName = txtUserName.Text.Trim
                    .FK_MRole_ID = cboRole.SelectedItem.Value
                    .FK_MGroupMenu_ID = cboGroup.SelectedItem.Value

                    If AuthenticationMode <> 1 Then


                        If txtUserPassword.Text.Trim <> "" Then
                            'ganti password
                            .UserPasword = NawaBLL.Common.Encrypt(txtUserPassword.Text, .UserPasswordSalt)
                        End If
                    End If

                    .UserEmailAddress = txtEmailAddress.Text.Trim
                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .ApprovedBy = ""
                    .LastUpdateDate = Now
                    .ApprovedDate = Nothing
                    .IPAddress = ""
                    .Active = True
                    .InUsed = False
                    .IsDisabled = False
                    .LastLogin = Now
                    .LastChangePassword = DateSerial(1900, 1, 1)
                End With



                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objMuserBll.SaveEditTanpaApproval(ObjMUserDataEdit, ObjModule)



                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else

                    objMuserBll.SaveEditApproval(ObjMUserDataEdit, ObjModule)





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