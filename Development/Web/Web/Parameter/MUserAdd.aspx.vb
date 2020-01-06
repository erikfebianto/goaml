Imports Ext.Net
Partial Class MUserAdd
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

            If Session("ModuleAdd.AuthenticationMode") Is Nothing Then
                Dim objsystemparameter As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3)
                Dim intauthentication As Integer = 2 'default formauthentication=2
                If Not objsystemparameter Is Nothing Then
                    intauthentication = objsystemparameter.SettingValue
                    Session("ModuleAdd.AuthenticationMode") = intauthentication
                End If
            End If
            Return Session("ModuleAdd.AuthenticationMode")
        End Get
        Set(ByVal value As Integer)
            Session("ModuleAdd.AuthenticationMode") = value
        End Set
    End Property

    Public Property ObjMUserDataAdd() As NawaDAL.MUser
        Get
            Return Session("ModuleAdd.ObjMUser")
        End Get
        Set(ByVal value As NawaDAL.MUser)
            Session("ModuleAdd.ObjMUser") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("ModuleAdd.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ModuleAdd.ObjModule") = value
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

                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Add"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Add"
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
            objMuserBll.Bentukform()


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

                MsEmployee = Nothing

                Dim objnewrow As Data.DataRow = MsEmployee.NewRow
                objnewrow.Item("Userid") = dtresult.Rows(0).Item("cn")
                objnewrow.Item("NIK") = dtresult.Rows(0).Item("cn")
                objnewrow.Item("Supervisor") = dtresult.Rows(0).Item("manager")
                objnewrow.Item("Jabatan") = dtresult.Rows(0).Item("title")

                MsEmployee.Rows.Add(objnewrow)

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub ClearSession()
        Session("ModuleAdd.AuthenticationMode") = Nothing
        Session("MUserAdd.MsEmployee") = Nothing
    End Sub
    Public Function IsDataAddValid()
        Try

            Dim txtUserId As Ext.Net.TextField = CType(FormPanelInput.FindControl("UserID"), Ext.Net.TextField)
            If txtUserId.Text.Trim.Contains("-") Then
                Throw New Exception("User ID can not contain - char")
            End If

            If AuthenticationMode <> 1 Then
                Dim objPassword As TextField = CType(FormPanelInput.FindControl("UserPassword"), Ext.Net.TextField)
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
                If Not NawaBLL.MUserBLL.ValidatePasswordRecycleCount(txtUserId.Text, objPassword.Text.Trim) Then

                    Throw New Exception("Your Password have Ever Used.Please Change to another Password.")
                End If
            End If


            'If Not NawaBLL.MUserBLL.ValidateAlfaNumeric(objPassword.Text.Trim) Then

            '    Throw New Exception("Password Must have Alfa Numeric")

            'End If

            'validate unik userid
            If Not NawaBLL.MUserBLL.IsUserIdUnikAdd(txtUserId.Text.Trim) Then

                Throw New Exception("Userid " & txtUserId.Text.Trim & " already exists.")
            End If

            'validate userid belum ada di approval

            If NawaBLL.MUserBLL.IsUseridExistInApproval(txtUserId.Text.Trim) Then

                Throw New Exception("Userid " & txtUserId.Text.Trim & " already exists in approval.")
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

                If AuthenticationMode <> 1 Then
                    txtUserPassword = CType(FormPanelInput.FindControl("UserPassword"), Ext.Net.TextField)

                Else

                    Dim objsystemparameter As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(49)
                    If Not objsystemparameter Is Nothing Then
                        strpassword = NawaBLL.Common.DecryptRijndael(objsystemparameter.SettingValue, objsystemparameter.EncriptionKey)
                    End If


                End If





                ObjMUserDataAdd = New NawaDAL.MUser

                With ObjMUserDataAdd
                    Dim oRand As New Random
                    .PK_MUser_ID = oRand.Next
                    .UserID = txtUserId.Text.Trim
                    .UserName = txtUserName.Text.Trim
                    .FK_MRole_ID = cboRole.SelectedItem.Value
                    .FK_MGroupMenu_ID = cboGroup.SelectedItem.Value



                    Dim strSalt = Guid.NewGuid.ToString


                    If AuthenticationMode = 1 Then

                        .UserPasword = NawaBLL.Common.Encrypt(strpassword, strSalt)
                        .UserPasswordSalt = strSalt
                    ElseIf AuthenticationMode = 2 Then

                        .UserPasword = NawaBLL.Common.Encrypt(txtUserPassword.Text, strSalt)
                        .UserPasswordSalt = strSalt
                    End If




                    .UserEmailAddress = txtEmailAddress.Text.Trim
                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                    .ApprovedBy = ""
                    .CreatedDate = Now
                    .LastUpdateDate = Now
                    .ApprovedDate = Nothing
                    .IPAddress = ""
                    .Active = True
                    .InUsed = False
                    .IsDisabled = False
                    .LastLogin = Nothing
                    .LastChangePassword = DateSerial(1900, 1, 1)
                End With



                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objMuserBll.SaveAddTanpaApproval(ObjMUserDataAdd, ObjModule)

                    If AuthenticationMode = 1 Then
                        If Not MsEmployee Is Nothing Then
                            If MsEmployee.Rows.Count > 0 Then

                                Dim osqlparam(3) As System.Data.SqlClient.SqlParameter

                                osqlparam(0) = New Data.SqlClient.SqlParameter
                                osqlparam(0).ParameterName = "@userid"
                                osqlparam(0).Value = MsEmployee.Rows(0).Item("Userid")

                                osqlparam(1) = New Data.SqlClient.SqlParameter
                                osqlparam(1).ParameterName = "@nik"
                                osqlparam(1).Value = MsEmployee.Rows(0).Item("NIK")


                                osqlparam(2) = New Data.SqlClient.SqlParameter
                                osqlparam(2).ParameterName = "@supervisor"
                                osqlparam(2).Value = MsEmployee.Rows(0).Item("supervisor")



                                osqlparam(3) = New Data.SqlClient.SqlParameter
                                osqlparam(3).ParameterName = "@jabatan"
                                osqlparam(3).Value = MsEmployee.Rows(0).Item("Jabatan")


                                NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.StoredProcedure, "Usp_InsertMsEmployee", osqlparam)
                            End If
                        End If
                    End If


                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else

                    objMuserBll.SaveAddApproval(ObjMUserDataAdd, ObjModule)

                    If AuthenticationMode = 1 Then
                        If Not MsEmployee Is Nothing Then
                            If MsEmployee.Rows.Count > 0 Then

                                Dim osqlparam(3) As System.Data.SqlClient.SqlParameter

                                osqlparam(0) = New Data.SqlClient.SqlParameter
                                osqlparam(0).ParameterName = "@userid"
                                osqlparam(0).Value = MsEmployee.Rows(0).Item("Userid")

                                osqlparam(1) = New Data.SqlClient.SqlParameter
                                osqlparam(1).ParameterName = "@nik"
                                osqlparam(1).Value = MsEmployee.Rows(0).Item("NIK")


                                osqlparam(2) = New Data.SqlClient.SqlParameter
                                osqlparam(2).ParameterName = "@supervisor"
                                osqlparam(2).Value = MsEmployee.Rows(0).Item("supervisor")



                                osqlparam(3) = New Data.SqlClient.SqlParameter
                                osqlparam(3).ParameterName = "@jabatan"
                                osqlparam(3).Value = MsEmployee.Rows(0).Item("Jabatan")


                                NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.StoredProcedure, "Usp_InsertMsEmployeeApproval", osqlparam)
                            End If
                        End If
                    End If


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