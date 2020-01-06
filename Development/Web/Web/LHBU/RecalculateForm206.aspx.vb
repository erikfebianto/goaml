Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports CookComputing.XmlRpc
Imports System.Data.Entity
Imports OfficeOpenXml

Partial Class LHBU_RecalculateForm206
    Inherits Parent

    'Inherits System.Web.UI.Page

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("RecalculateForm206.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("RecalculateForm206.ObjModule") = value
        End Set
    End Property

    Public Property objListFile() As List(Of GeneratedFileList)
        Get
            If Session("RecalculateForm206.objListFile") Is Nothing Then
                Session("RecalculateForm206.objListFile") = FileGenerationBLL.GetData()
            End If
            Return Session("RecalculateForm206.objListFile")
        End Get
        Set(ByVal value As List(Of GeneratedFileList))
            Session("RecalculateForm206.objListFile") = value
        End Set
    End Property

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                'ClearSession()

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

                    'TanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    'If Not IsPostBack Then
                    '    TanggalData.SetValue(DateTime.Now)
                    'End If

                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnRecalculate_Click(sender As Object, e As EventArgs)

        Try
            WindowProgress.Hidden = False
            'StartLongAction()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Sub btnOK_DirectEvent()
        'If Me.Session("LongActionError") Is Nothing Then
        '    Dim intmoduleID As Integer = 0
        '    Dim directory As String = ""

        '    Using objdb As New NawaDAL.NawaDataEntities
        '        intmoduleID = (From x In objdb.Modules Where x.ModuleName = "TextFileTemporaryTable" Select x).FirstOrDefault.PK_Module_ID
        '        directory = (From x In objdb.Modules Where x.ModuleName = "TextFileTemporaryTable" Select x).FirstOrDefault.UrlView
        '    End Using
        '    Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

        '    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
        'Else
        '    Dim intmoduleID As Integer = 0
        '    Dim directory As String = ""

        '    Using objdb As New NawaDAL.NawaDataEntities
        '        intmoduleID = (From x In objdb.Modules Where x.ModuleName = "TextFileUpload" Select x).FirstOrDefault.PK_Module_ID
        '        directory = (From x In objdb.Modules Where x.ModuleName = "TextFileUpload" Select x).FirstOrDefault.UrlView
        '    End Using
        '    Dim Moduleid As String = NawaBLL.Common.EncryptQueryString(intmoduleID, NawaBLL.SystemParameterBLL.GetEncriptionKey)

        '    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & directory & "?ModuleID={0}", Moduleid), "Loading")
        'End If

    End Sub

    Protected Sub RefreshProgress(ByVal sender As Object, ByVal e As DirectEventArgs)
        '    Dim progress As Object = Me.Session("LongActionProgress")
        '    If Not progress Is Nothing Then
        '        Progress1.UpdateProgress((progress) / 10.0F, String.Format(""))
        '    Else
        '        ResourceManager.GetInstance.AddScript("{0}.stopTask('longactionprogress');", Me.TaskManager1.ClientID)
        '        If Me.Session("LongActionError") Is Nothing Then
        '            Progress1.UpdateProgress(1, " Upload Selesai")
        '            btnOK.Hidden = False
        '        Else

        '            Progress1.UpdateProgress(1, CStr(Me.Session("LongActionError")))
        '            btnOK.Hidden = False
        '        End If

        '    End If


    End Sub

    Sub StartLongAction()
        '    Session("LongActionProgress") = 0

        '    System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf LongAction))
        '    ResourceManager.GetInstance.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID)
    End Sub
End Class
