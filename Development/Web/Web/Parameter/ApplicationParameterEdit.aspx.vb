Imports Ext.Net
Partial Class ApplicationParameterEdit
    Inherits Parent

    Public objsystemparameterbll As NawaBLL.SystemParameterBLL

    Public Property ObjParameterEdit() As NawaDAL.SystemParameter
        Get
            Return Session("ApplicationParameterEdit.ObjParameterEdit")
        End Get
        Set(ByVal value As NawaDAL.SystemParameter)
            Session("ApplicationParameterEdit.ObjParameterEdit") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("ApplicationParameterEdit.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ApplicationParameterEdit.ObjModule") = value
        End Set
    End Property

    Private Sub MUserAdd_Init(sender As Object, e As EventArgs) Handles Me.Init
        objsystemparameterbll = New NawaBLL.SystemParameterBLL()
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



            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjParameterEdit = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(id)


            End If
            LoadControl()

            If Not Ext.Net.X.IsAjaxRequest Then
                LoadDataUser()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub LoadControl()

        If ObjParameterEdit.fk_MFieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then

            NawaBLL.Nawa.BLL.NawaFramework.ExtCombo(FormPanelInput, "Value", "txtValue", True, "SystemParameterPickList", "Pk_ApplicationAuthentication_ID", "ApplicationAuthentication", "SettingName='" & ObjParameterEdit.SettingName & "'")
        Else

            If ObjParameterEdit.Hide Then
                NawaBLL.Nawa.BLL.NawaFramework.ExtTextPassword(FormPanelInput, "Value", "txtValue", True, 250)
            Else
                NawaBLL.Nawa.BLL.NawaFramework.ExtText(FormPanelInput, "Value", "txtValue", True, 250, "")
            End If


        End If
    End Sub

    Sub LoadDataUser()
        If Not ObjParameterEdit Is Nothing Then
            With ObjParameterEdit
                txtID.Text = .PK_SystemParameter_ID
                txtParameterGroup.Text = NawaBLL.SystemParameterBLL.GetParameterGroupNameByPK(.FK_SystemParameterGroup_ID)
                txtSettingName.Text = .SettingName

                If ObjParameterEdit.fk_MFieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    Dim objcontrol As Ext.Net.ComboBox = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.ComboBox)
                    objcontrol.SetValueAndFireSelect(CType(.SettingValue, Object))
                Else

                    Dim objcontrol As Ext.Net.TextField = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.TextField)
                    If ObjParameterEdit.IsEncript Then
                        Try
                            objcontrol.Text = NawaBLL.Common.DecryptRijndael(.SettingValue, .EncriptionKey)
                        Catch ex As Exception

                        End Try

                    Else
                        objcontrol.Text = .SettingValue
                    End If

                End If


            End With
            

        End If

    End Sub

    Sub ClearSession()
        ObjParameterEdit = Nothing
    End Sub
    Public Function IsDataEditValid()
        Try
            'If ObjParameterEdit.fk_MFieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
            '    Dim objcontrol As Ext.Net.DropDownField = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.DropDownField)
            '    If objcontrol.Value = "" Then

            '    End If
            'Else
            '    Dim objcontrol As Ext.Net.TextField = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.TextField)

            'End If
            'If txtValue.Text = "" Then

            'End If
            If NawaBLL.SystemParameterBLL.IsExistInApproval(ObjParameterEdit.PK_SystemParameter_ID, ObjModule) Then
                Throw New Exception("Data Already Exists In Pending Approval.")
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
            If IsDataEditValid() Then

                If ObjParameterEdit.fk_MFieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    Dim objcontrol As Ext.Net.ComboBox = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.ComboBox)
                    ObjParameterEdit.SettingValue = objcontrol.Value

                Else

                    Dim objcontrol As Ext.Net.TextField = CType(FormPanelInput.FindControl("txtValue"), Ext.Net.TextField)

                    If ObjParameterEdit.IsEncript Then
                        ObjParameterEdit.SettingValue = NawaBLL.Common.EncryptRijndael(objcontrol.Text, ObjParameterEdit.EncriptionKey)
                    Else
                        ObjParameterEdit.SettingValue = objcontrol.Text
                    End If


                End If


                    If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objsystemparameterbll.SaveEditTanpaApproval(ObjParameterEdit, ObjModule)
                    If ObjParameterEdit.PK_SystemParameter_ID = NawaBLL.SystemParameterBLL.SytemParameterEnum.SessionTimeOut Then
                        NawaBLL.SystemParameterBLL.UpdateSessionStatewebconfig(ObjParameterEdit.SettingValue)
                    ElseIf ObjParameterEdit.PK_SystemParameter_ID = NawaBLL.SystemParameterBLL.SytemParameterEnum.AplicationAuthentication Then
                        '  NawaBLL.SystemParameterBLL.UpdateApplicationAuthenticationwebconfig(ObjParameterEdit.SettingValue)
                    End If

                    Panelconfirmation.Hidden = False
                        FormPanelInput.Hidden = True
                        LblConfirmation.Text = "Data Saved into Database"
                    Else

                        objsystemparameterbll.SaveEditApproval(ObjParameterEdit, ObjModule)
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