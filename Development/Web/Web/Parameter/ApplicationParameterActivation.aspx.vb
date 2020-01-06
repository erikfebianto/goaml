Imports Ext.Net
Partial Class ApplicationParameterActivation
    Inherits Parent

    Public objsystemparameterbll As NawaBLL.SystemParameterBLL

    Public Property ObjParameterEdit() As NawaDAL.SystemParameter
        Get
            Return Session("ApplicationParameterActivation.ObjParameterEdit")
        End Get
        Set(ByVal value As NawaDAL.SystemParameter)
            Session("ApplicationParameterActivation.ObjParameterEdit") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("ApplicationParameterActivation.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ApplicationParameterActivation.ObjModule") = value
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


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Activation) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel & " Activation"
                    Panelconfirmation.Title = ObjModule.ModuleLabel & " Activation"
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If




            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strid As String = Request.Params("ID")
                Dim id As String = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjParameterEdit = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(id)
                LoadDataUser()

            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub LoadDataUser()
        If Not ObjParameterEdit Is Nothing Then
            With ObjParameterEdit
                txtID.Text = .PK_SystemParameter_ID
                txtParameterGroup.Text = NawaBLL.SystemParameterBLL.GetParameterGroupNameByPK(.FK_SystemParameterGroup_ID)
                txtSettingName.Text = .SettingName
                If ObjParameterEdit.fk_MFieldType_ID = NawaBLL.Common.MFieldType.ReferenceTable Then
                    txtValue.Text = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, Data.CommandType.Text, "SELECT  ApplicationAuthentication FROM SystemParameterpicklist WHERE Pk_ApplicationAuthentication_ID=" & .SettingValue.Replace("'", "''") & " AND SettingName='" & ObjParameterEdit.SettingName & "'", Nothing)
                Else
                    If ObjParameterEdit.Hide Then
                        txtValue.Text = ".........."
                    Else
                        txtValue.Text = .SettingValue
                    End If

                End If


                txtActivation.Text = .Active.ToString & "-->" & (Not .Active).ToString
            End With


        End If

    End Sub

    Sub ClearSession()
        ObjParameterEdit = Nothing
    End Sub
    Public Function IsDataEditValid()
        Try
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




                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    objsystemparameterbll.ActivationTanpaapproval(ObjParameterEdit.PK_SystemParameter_ID, ObjModule)

                    Panelconfirmation.Hidden = False
                    FormPanelInput.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else

                    objsystemparameterbll.ActivationDenganapproval(ObjParameterEdit.PK_SystemParameter_ID, ObjModule)
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