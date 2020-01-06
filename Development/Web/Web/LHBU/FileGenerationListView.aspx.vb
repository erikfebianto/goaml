Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net

Partial Class LHBU_FileGenerationListView
    Inherits Parent

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("FileApprovalView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("FileApprovalView.ObjModule") = value
        End Set
    End Property

    Public Property objListFile() As List(Of Vw_GeneratedFileList)
        Get
            Return Session("FileApprovalView.objListFile")
        End Get
        Set(ByVal value As List(Of Vw_GeneratedFileList))
            Session("FileApprovalView.objListFile") = value
        End Set
    End Property



    Protected Sub RefreshTime(sender As Object, e As DirectEventArgs)
        Try
            LoadData()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()

                Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)

                If ObjModule Is Nothing Then
                    Throw New Exception("Invalid Module ID")
                Else
                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    gridFileList.Title = ObjModule.ModuleLabel
                    storeFileList.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    colPeriodeLaporan.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    colStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
                    colCompleteDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"

                    LoadData()
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ClearSession()
        ObjModule = Nothing
        objListFile = Nothing
    End Sub

    Protected Sub LoadData()
        'Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListFile)
        objListFile = FileGenerationBLL.GetDataList()

        storeFileList.DataSource = objListFile
        storeFileList.DataBind()
    End Sub
End Class
