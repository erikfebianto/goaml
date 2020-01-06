Imports Ext.Net
Imports OfficeOpenXml
Imports System.Data.SqlClient
Imports System.Data
Imports ReportingService
Imports Microsoft.Reporting.WebForms
Imports System.Net
Imports System.Security.Principal

Partial Class BookingGroup
    Inherits Parent


    Public Property propGroup() As String
        Get
            Return Session("BookingGroup.tGroup")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.tGroup") = value
        End Set
    End Property

    Public Property propFacilityType() As String
        Get
            Return Session("BookingGroup.CbFacilityType")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.CbFacilityType") = value
        End Set
    End Property

    Public Property propSectorL1() As String
        Get
            Return Session("BookingGroup.SectorL1")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.SectorL1") = value
        End Set
    End Property

    Public Property propSectorL2() As String
        Get
            Return Session("BookingGroup.SectorL2")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.SectorL2") = value
        End Set
    End Property

    Public Property propCurrencyCode() As String
        Get
            Return Session("BookingGroup.CbCurrencyCode")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.CbCurrencyCode") = value
        End Set
    End Property
    Public Property propNewLimit() As String
        Get
            Return Session("BookingGroup.txtNewLimit")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.txtNewLimit") = value
        End Set
    End Property

    Public Property propCollateralCurrencyCode() As String
        Get
            Return Session("BookingGroup.CbCollateralCurrencyCode")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.CbCollateralCurrencyCode") = value
        End Set
    End Property
    Public Property propCollateralAmt() As String
        Get
            Return Session("BookingGroup.txtCollateralAmt")
        End Get
        Set(ByVal value As String)
            Session("BookingGroup.txtCollateralAmt") = value
        End Set
    End Property



    Protected Sub StoretGroup_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim query As String = e.Parameters("query")
            If query Is Nothing Then query = ""
            Dim strfilter As String = ""
            If query.Length > 0 Then
                strfilter = "  GroupidName like '%" & query & "%'"
            End If
            StoretGroup.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("vw_GroupList", "GROUP_ID,GroupidName ", strfilter, "GroupidName", e.Start, e.Limit, e.Total)
            StoretGroup.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub



    Sub ClearSession()
        propGroup = Nothing
        propFacilityType = Nothing
        propSectorL1 = Nothing
        propSectorL2 = Nothing
        propCurrencyCode = Nothing
        propNewLimit = Nothing
        propCollateralCurrencyCode = Nothing
        propCollateralAmt = Nothing

    End Sub

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("BookingGroup.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("BookingGroup.ObjModule") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()
                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    '  StoretGroup.PageSize = NawaBLL.SystemParameterBLL.GetPageSize

                    FormPanelInput.Title = ObjModule.ModuleLabel

                    load_CbSectorL1()
                    load_CbSectorL2(CbSectorL1.SelectedItem.Value)
                    load_CbFacilityType()
                    load_CbCurrencyCode()
                    load_CbCollateralCurrencyCode()
                    ' ReportViewer1.Visible = False

                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub showReport()
        Try


            Dim strReportPath As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3000002).SettingValue

            Dim reportparameters() As ReportParameter
            ReDim reportparameters(7)
            reportparameters(0) = New ReportParameter("GroupID", propGroup)
            reportparameters(1) = New ReportParameter("SectorL1", propSectorL1)
            reportparameters(2) = New ReportParameter("SectorL2", propSectorL2)
            reportparameters(3) = New ReportParameter("FacitlityType", propFacilityType)
            reportparameters(4) = New ReportParameter("CurrencyCode", propCurrencyCode)
            reportparameters(5) = New ReportParameter("NewLimit", propNewLimit)
            reportparameters(6) = New ReportParameter("CollateralCurrencyCode", propCollateralCurrencyCode)
            reportparameters(7) = New ReportParameter("CollateralAmt", propCollateralAmt)


            ' Dim strReportPath As String = "/Rep/NewBookingSimulation"


            Container1.Visible = False
            panelInput.MinHeight = 1000
            ReportViewer1.Visible = True
            ReportViewer1.SizeToReportContent = True
            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ReportViewer1.ShowToolBar = False
            ReportViewer1.ServerReport.ReportServerCredentials = New MyReportServerCredentials_newBooking
            ReportViewer1.ServerReport.ReportServerUrl = New Uri(NawaBLL.SystemParameterBLL.GetSystemParameterByPk(13).SettingValue)
            ReportViewer1.ServerReport.ReportPath = strReportPath
            ReportViewer1.ServerReport.SetParameters(reportparameters)
            ReportViewer1.ServerReport.Refresh()


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub tGroup_Change(sender As Object, e As DirectEventArgs)
        Try
            If Not IsNothing(tGroup.SelectedItem.Text) Then
                Dim groupsplit() = tGroup.SelectedItem.Text.ToString.Split("-")
                If groupsplit.Length > 1 Then
                    lblGroup.Value = groupsplit(1).Trim
                Else
                    lblGroup.Value = ""
                End If
            Else
                lblGroup.Value = ""
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub load_CbSectorL1()
        StoreCbSectorL1.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select rtrim(ltrim(Sector_L1_Code)) as Sector_L1_Code,'('+ rtrim(ltrim(Sector_L1_Code))  +') ' + rtrim(ltrim(Sector_L1_Name)) as Sector_L1_Name from CLS_MS_Sector_L1 order by rtrim(ltrim(Sector_L1_Code)) asc")
        StoreCbSectorL1.DataBind()
    End Sub

    Sub load_CbSectorL2(ByVal L1Code As String)
        StoreCbSectorL2.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select rtrim(ltrim(Sector_L2_Code)) as Sector_L2_Code,'('+  rtrim(ltrim(Sector_L2_Code)) +') ' + rtrim(ltrim(Sector_L2_Name)) as Sector_L2_Name from CLS_MS_Sector_L2 where Sector_L1_Code='" & Replace(L1Code, "'", "''") & "' order by rtrim(ltrim(Sector_L2_Code)) asc")
        StoreCbSectorL2.DataBind()
    End Sub

    Protected Sub CbSectorL1_Change(sender As Object, e As DirectEventArgs)
        Try
            CbSectorL2.Clear()
            StoreCbSectorL2.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select rtrim(ltrim(Sector_L2_Code)) as Sector_L2_Code, '('+ rtrim(ltrim(Sector_L2_Code)) +') ' + rtrim(ltrim(Sector_L2_Name)) as Sector_L2_Name from CLS_MS_Sector_L2 where Sector_L1_Code='" & Replace(CbSectorL1.SelectedItem.Value, "'", "''") & "' order by  rtrim(ltrim(Sector_L2_Code)) asc")
            StoreCbSectorL2.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub load_CbFacilityType()
        StoreCbFacilityType.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select ltrim(rtrim(CLS_MS_FacilityCode)) as CLS_MS_FacilityCode,'('+ ltrim(rtrim(CLS_MS_FacilityCode))+') ' +ltrim(rtrim(CLS_MS_FacilityName)) as CLS_MS_FacilityName from CLS_MS_Facility")
        StoreCbFacilityType.DataBind()
    End Sub
    Sub load_CbCurrencyCode()
        StoreCbCurrencyCode.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select ltrim(rtrim(CLS_MS_MataUangCode)) as CLS_MS_MataUangCode,'('+ ltrim(rtrim(CLS_MS_MataUangCode))+') ' + CLS_MS_MataUangName as CLS_MS_MataUangName from CLS_MS_MataUang where CLS_MS_MataUangCode<>'000' order by CLS_MS_MataUangCode asc")
        StoreCbCurrencyCode.DataBind()
    End Sub
    Sub load_CbCollateralCurrencyCode()
        StoreCbCollateralCurrencyCode.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select ltrim(rtrim(CLS_MS_MataUangCode)) as CLS_MS_MataUangCode,'('+ ltrim(rtrim(CLS_MS_MataUangCode))+') ' + CLS_MS_MataUangName as CLS_MS_MataUangName from CLS_MS_MataUang where CLS_MS_MataUangCode<>'000' order by CLS_MS_MataUangCode asc")
        StoreCbCollateralCurrencyCode.DataBind()
    End Sub



    Protected Sub ButtonSimulate_Click(sender As Object, e As EventArgs) Handles ButtonSimulate.Click
        Try


            propGroup = tGroup.SelectedItem.Value
            propSectorL1 = CbSectorL1.SelectedItem.Value
            propSectorL2 = CbSectorL2.SelectedItem.Value
            propFacilityType = CbFacilityType.SelectedItem.Value

            propCurrencyCode = CbCurrencyCode.SelectedItem.Value
            propNewLimit = txtNewLimit.Text
            propCollateralCurrencyCode = CbCollateralCurrencyCode.SelectedItem.Value
            propCollateralAmt = txtCollateralAmt.Text


            If IsNothing(propFacilityType) Then
                Throw New Exception("Select Facility Type")
            End If

            If IsNothing(propSectorL1) Then
                Throw New Exception("Select Sector L1")
            End If
            If IsNothing(propSectorL2) Then
                Throw New Exception("Select Sector L2")
            End If

            If IsNothing(propCurrencyCode) Then
                Throw New Exception("Select Currency Code")
            End If

            If IsNumeric(propNewLimit) = False Then
                Throw New Exception("Invalid New Limit Value")
            End If

            If IsNothing(propCollateralCurrencyCode) Then
                Throw New Exception("Select Collateral Currency Code")
            End If

            If IsNumeric(propCollateralAmt) = False Then
                Throw New Exception("Invalid Collateral Amount")
            End If



            showReport()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Function CheckRole() As NawaDAL.MRole
        Using objdb As New NawaDAL.NawaDataEntities
            Dim User As NawaDAL.MUser = (From x In objdb.MUsers Where x.PK_MUser_ID = NawaBLL.Common.SessionCurrentUser.PK_MUser_ID Select x).FirstOrDefault
            Dim Role As NawaDAL.MRole = (From x In objdb.MRoles Where x.PK_MRole_ID = User.FK_MRole_ID Select x).FirstOrDefault

            Return Role
        End Using
    End Function




    Sub ClearInput()
        tGroup.SelectedItem.Index = -1
        CbFacilityType.SelectedItem.Index = -1
        CbCurrencyCode.SelectedItem.Index = -1
        txtNewLimit.Value = ""
        CbCollateralCurrencyCode.SelectedItem.Index = -1
        txtCollateralAmt.Value = ""
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub BookingGroup_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub

    Private Sub BookingGroup_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad

    End Sub
End Class



<Serializable()>
Public NotInheritable Class MyReportServerCredentials_newBooking
    Implements IReportServerCredentials

    Public ReadOnly Property ImpersonationUser() As WindowsIdentity _
            Implements IReportServerCredentials.ImpersonationUser
        Get
            'Use the default windows user.  Credentials will be
            'provided by the NetworkCredentials property.
            Return Nothing

        End Get
    End Property

    Public ReadOnly Property NetworkCredentials() As ICredentials _
            Implements IReportServerCredentials.NetworkCredentials
        Get
            'Read the user information from the web.config file.  
            'By reading the information on demand instead of storing 
            'it, the credentials will not be stored in session, 
            'reducing the vulnerable surface area to the web.config 
            'file, which can be secured with an ACL.

            'User name
            Dim userName As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(9).SettingValue


            If (String.IsNullOrEmpty(userName)) Then
                Throw New Exception("Missing user name from Application Parameter")
            End If

            'Password
            Dim password As String = NawaBLL.Common.DecryptRijndael(NawaBLL.SystemParameterBLL.GetSystemParameterByPk(10).SettingValue, NawaBLL.SystemParameterBLL.GetSystemParameterByPk(10).EncriptionKey)

            If (String.IsNullOrEmpty(password)) Then
                Throw New Exception("Missing password from Application Parameter")
            End If

            'Domain
            Dim domain As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(12).SettingValue

            'If (String.IsNullOrEmpty(domain)) Then
            '    Throw New Exception("Missing domain from web.config file")
            'End If

            Return New NetworkCredential(userName, password, domain)

        End Get
    End Property
    Public Function GetFormsCredentials(ByRef authCookie As Cookie, ByRef userName As String, ByRef password As String, ByRef authority As String) As Boolean Implements IReportServerCredentials.GetFormsCredentials

        authCookie = Nothing
        userName = Nothing
        password = Nothing
        authority = Nothing

        'Not using form credentials
        Return False

    End Function

End Class