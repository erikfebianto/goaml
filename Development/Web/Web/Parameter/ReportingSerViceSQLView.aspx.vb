Imports ReportingService
Imports Microsoft.Reporting.WebForms
Imports System.Net
Imports System.Security.Principal

Partial Class Parameter_ReportingSerViceSQLView
    Inherits ParentPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not Ext.Net.X.IsAjaxRequest Then
                FormPanel1.Title = ObjModule.ModuleLabel & " View"
                LoadDataReport()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub LoadDataReport()
        Dim objreport As New ReportingService.ReportingService2010
        Dim objreportservercredential As New MyReportServerCredentials
        objreport.Credentials = objreportservercredential.NetworkCredentials

        Dim objitem As CatalogItem()

        objitem = objreport.ListChildren("/", True)
        For Each item As CatalogItem In objitem
            If item.TypeName = "Report" Then
                cboReport.Items.Add(New Ext.Net.ListItem(item.Name, item.Path))
            End If

        Next


    End Sub

    Private Sub Parameter_ReportingSerViceSQLView_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        ActionType = NawaBLL.Common.ModuleActionEnum.view

    End Sub

    Protected Sub btnviewReport_Click(sender As Object, e As EventArgs) Handles btnviewReport.Click
        Try
            ReportViewer1.SizeToReportContent = True

            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ReportViewer1.ServerReport.ReportServerCredentials = New MyReportServerCredentials
            ReportViewer1.ServerReport.ReportServerUrl = New Uri(NawaBLL.SystemParameterBLL.GetSystemParameterByPk(13).SettingValue)
            ReportViewer1.ServerReport.ReportPath = cboReport.Value



            Dim objlistparameter As ReportParameterInfoCollection = ReportViewer1.ServerReport.GetParameters()
            If objlistparameter.Count > 0 Then
                If objlistparameter(0).Name.ToLower = "userid" Then
                    Dim objparamuserid As New ReportParameter("userid", NawaBLL.Common.SessionCurrentUser.UserID)
                    ReportViewer1.ServerReport.SetParameters(objparamuserid)
                End If

                If objlistparameter(0).Name.ToLower = "pkuserid" Then
                    Dim objparampkuserid As New ReportParameter("pkuserid", NawaBLL.Common.SessionCurrentUser.PK_MUser_ID)
                    ReportViewer1.ServerReport.SetParameters(objparampkuserid)
                End If
            End If



            ReportViewer1.ServerReport.Refresh()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class


<Serializable()>
Public NotInheritable Class MyReportServerCredentials
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
