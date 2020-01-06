Imports Ext.Net
Imports OfficeOpenXml
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports Microsoft.Reporting.WebForms
Imports System.Security.Principal
Imports NawaDAL

Partial Class BookingCIF
    Inherits Parent


    Public Property propCIF() As String
        Get
            Return Session("BookingCIF.tCIF")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.tCIF") = value
        End Set
    End Property

    Public Property propCIFName() As String
        Get
            Return Session("BookingCIF.lblCIF")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.lblCIF") = value
        End Set
    End Property

    Public Property propFacilityType() As String
        Get
            Return Session("BookingCIF.CbFacilityType")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.CbFacilityType") = value
        End Set
    End Property

    Public Property propCurrencyCode() As String
        Get
            Return Session("BookingCIF.CbCurrencyCode")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.CbCurrencyCode") = value
        End Set
    End Property
    Public Property propNewLimit() As String
        Get
            Return Session("BookingCIF.txtNewLimit")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.txtNewLimit") = value
        End Set
    End Property

    Public Property propCollateralCurrencyCode() As String
        Get
            Return Session("BookingCIF.CbCollateralCurrencyCode")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.CbCollateralCurrencyCode") = value
        End Set
    End Property
    Public Property propCollateralAmt() As String
        Get
            Return Session("BookingCIF.txtCollateralAmt")
        End Get
        Set(ByVal value As String)
            Session("BookingCIF.txtCollateralAmt") = value
        End Set
    End Property

    Protected Sub tCIF_Change_fillFacility()
        Dim strSqll As String = ""
        strSqll = " select FAC_ID,m.CLS_MS_FacilityName from CLS_DM_Facility i "
        strSqll = strSqll & " inner join [dbo].[CLS_MS_Facility] m "
        strSqll = strSqll & " on i.FAC_CODE=m.CLS_MS_FacilityCode "
        strSqll = strSqll & " where i.CUST_NO='" & tCIF.TextValue & "' "
        Try
            CbFacilityType.Clear()
            StoreCbFacilityType.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strSqll)
            StoreCbFacilityType.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub



    Private Function GetTerbilang(ByVal nilai As String) As String
        Dim StrQuery As String = String.Format(" Select   dbo.f_amount_in_word({0}) ", nilai)

        Dim dtSet As Data.DataSet = SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, StrQuery)
        If dtSet.Tables.Count > 0 Then
            Dim dtTable As Data.DataTable = dtSet.Tables(0)
            If dtTable.Rows.Count > 0 Then
                Return dtTable.Rows(0).Item(0).ToString
            End If
        Else
            Return ""
        End If
        Return ""
    End Function

    Sub ClearSession()
        propCIF = Nothing
        propFacilityType = Nothing
        propCurrencyCode = Nothing
        propNewLimit = Nothing
        propCollateralCurrencyCode = Nothing
        propCollateralAmt = Nothing

    End Sub

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("BookingCIF.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("BookingCIF.ObjModule") = value
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



                    FormPanelInput.Title = ObjModule.ModuleLabel
                    load_CbFacilityType()
                    load_CbCurrencyCode()
                    load_CbCollateralCurrencyCode()
                    load_lblCIF()
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
        Dim strReportPath As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(3000001).SettingValue

        Dim reportparameters() As ReportParameter
        ReDim reportparameters(5)
        reportparameters(0) = New ReportParameter("CIFNumber", propCIF)
        reportparameters(1) = New ReportParameter("FacilityType", propFacilityType)
        reportparameters(2) = New ReportParameter("CurrencyCode", propCurrencyCode)
        reportparameters(3) = New ReportParameter("NewLimit", propNewLimit)
        reportparameters(4) = New ReportParameter("CollateralCurrencyCode", propCollateralCurrencyCode)
        reportparameters(5) = New ReportParameter("CollateralAmount", propCollateralAmt)


        ' Dim strReportPath As String = "/Rep/ExistinctBookingSimulation"


         Container1.Visible = False
        panelInput.MinHeight = 1000
        ReportViewer1.Visible = True
        ReportViewer1.SizeToReportContent = True
        ReportViewer1.ProcessingMode = ProcessingMode.Remote
        ReportViewer1.ShowToolBar = False
        ReportViewer1.ServerReport.ReportServerCredentials = New MyReportServerCredentials_existsBooking
        ReportViewer1.ServerReport.ReportServerUrl = New Uri(NawaBLL.SystemParameterBLL.GetSystemParameterByPk(13).SettingValue)
        ReportViewer1.ServerReport.ReportPath = strReportPath
        ReportViewer1.ServerReport.SetParameters(reportparameters)
        ReportViewer1.ServerReport.Refresh()



    End Sub

    Private Function GetCIFName(ByVal CUST_NO As String) As String
        Dim StrQuery As String = String.Format("select top 1 [NAME] from [dbo].[CLS_STG_DWH_CUSTOMER] where CUST_NO='{0}'   ", CUST_NO)

        Dim dtSet As Data.DataSet = SQLHelper.ExecuteDataSet(SQLHelper.strConnectionString, Data.CommandType.Text, StrQuery)
        If dtSet.Tables.Count > 0 Then
            Dim dtTable As Data.DataTable = dtSet.Tables(0)
            If dtTable.Rows.Count > 0 Then
                Return dtTable.Rows(0).Item(0).ToString

            End If
        Else
            Return ""
        End If
        Return ""
    End Function
    Protected Sub tCIF_Change()
        Try

            propCIFName = GetCIFName(tCIF.TextValue)
            lblCIF.Value = propCIFName

            tCIF_Change_fillFacility()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub load_lblCIF()
        Try
            propCIFName = GetCIFName(tCIF.TextValue)
            lblCIF.Value = propCIFName
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub load_CbFacilityType()
        Dim strSqll As String = ""
        strSqll = " select FAC_ID,m.CLS_MS_FacilityName from CLS_DM_Facility i "
        strSqll = strSqll & " inner join [dbo].[CLS_MS_Facility] m "
        strSqll = strSqll & " on i.FAC_CODE=m.CLS_MS_FacilityCode "

        StoreCbFacilityType.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strSqll)
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


            propCIF = tCIF.TextValue
            propFacilityType = CbFacilityType.SelectedItem.Value

            propCurrencyCode = CbCurrencyCode.SelectedItem.Value
            propNewLimit = txtNewLimit.Text
            propCollateralCurrencyCode = CbCollateralCurrencyCode.SelectedItem.Value
            propCollateralAmt = txtCollateralAmt.Text
            propCIFName = lblCIF.Value

            If IsNothing(propCIF) Then
                Throw New Exception("Invalid CIF Number")
            End If

            If IsNothing(propFacilityType) Then
                Throw New Exception("Select Facility Type")
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
        tCIF.SetTextValue("")
        CbFacilityType.SelectedItem.Index = -1
        CbCurrencyCode.SelectedItem.Index = -1
        txtNewLimit.Value = ""
        CbCollateralCurrencyCode.SelectedItem.Index = -1
        txtCollateralAmt.Value = ""
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    'Private Sub txtCollateralAmt_TextChanged(sender As Object, e As EventArgs) Handles txtCollateralAmt.TextChanged
    '    If IsNumeric(txtCollateralAmt.Text) Then
    '        lblterbilangCollateralAmt.Text = GetTerbilang(CDec(txtCollateralAmt.Text))
    '    Else
    '        lblterbilangCollateralAmt.Text = ""
    '    End If


    'End Sub

    'Private Sub txtNewLimit_TextChanged(sender As Object, e As EventArgs) Handles txtNewLimit.TextChanged
    '    If IsNumeric(txtNewLimit.Text) Then
    '        lblterbilangNewLimit.Text = GetTerbilang(CDec(txtNewLimit.Text))
    '    Else
    '        lblterbilangNewLimit.Text = ""
    '    End If

    'End Sub
End Class



<Serializable()>
Public NotInheritable Class MyReportServerCredentials_existsBooking
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
