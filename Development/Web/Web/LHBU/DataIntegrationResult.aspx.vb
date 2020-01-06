Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports System.IO
Imports OfficeOpenXml
Imports Ionic.Zip

Partial Class LHBU_DataIntegrationResult
    Inherits Parent

    Public FileGeneration As FileGenerationBLL
    Public objFormModuleView As NawaBLL.FormModuleView

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("DataIntegrationResult.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("DataIntegrationResult.ObjModule") = value
        End Set
    End Property
    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
        objFormModuleView = New NawaBLL.FormModuleView(Me.gridFileList, Nothing)
    End Sub

    Public Property strWhereClause() As String
        Get
            Return Session("LHBU_DataIntegrationResult.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("LHBU_DataIntegrationResult.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("LHBU_DataIntegrationResult.strSort")
        End Get
        Set(ByVal value As String)
            Session("LHBU_DataIntegrationResult.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("LHBU_DataIntegrationResult.indexStart")
        End Get
        Set(ByVal value As String)
            Session("LHBU_DataIntegrationResult.indexStart") = value
        End Set
    End Property

    Public Property objListFile() As List(Of vw_TextFileTemporaryTable)
        Get
            If Session("DataIntegrationResult.objListFile") Is Nothing Then

                Session("DataIntegrationResult.objListFile") = FileGenerationBLL.GetDataIntegrationList() 'GetDataParam(SettingSLIK.KodeCabang, SettingSLIK.Bulan, SettingSLIK.Tahun)
            End If
            Return Session("DataIntegrationResult.objListFile")
        End Get
        Set(ByVal value As List(Of vw_TextFileTemporaryTable))
            Session("DataIntegrationResult.objListFile") = value
        End Set
    End Property

    Public Property objDownloadList() As List(Of TextFileTemporaryTable)
        Get
            Return Session("DataIntegrationResult.objDownloadList")
        End Get
        Set(ByVal value As List(Of TextFileTemporaryTable))
            Session("DataIntegrationResult.objDownloadList") = value
        End Set
    End Property

    Public Property objDownloadError() As List(Of TextFileTemporaryTable)
        Get
            Return Session("DataIntegrationResult.objDownloadError")
        End Get
        Set(ByVal value As List(Of TextFileTemporaryTable))
            Session("DataIntegrationResult.objDownloadError") = value
        End Set
    End Property

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
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
                    TanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    colStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
                    colCompleteDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"

                    gridFileList.Title = ObjModule.ModuleLabel
                    storeFileList.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub ClearSession()
        ObjModule = Nothing
        objListFile = Nothing
        objDownloadList = Nothing
        objDownloadError = Nothing
    End Sub

    Protected Sub StoreView_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start
            'If intStart = 0 Then intStart = 1
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objFormModuleView.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            Me.indexStart = intStart
            strsort = IIf(strsort = "", "StartDate desc", strsort)
            Me.strOrder = strsort
            'Dim DataPaging As Data.DataTable = objFormModuleView.getDataPaging(strWhereClause, strsort, intStart, intLimit, inttotalRecord)

            Me.strWhereClause = strfilter

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging("vw_TextFileTemporaryTable", "Pk_TextFileTemporaryTable_ID,FileName,FK_FormInfo_ID,DataSource,TanggalData,Status,StartDate,EndDate,TotalRecord,ErrorMessage", strfilter, strsort, intStart, intLimit, inttotalRecord)
            ''-- start paging ------------------------------------------------------------
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If
            'Dim rangeData As List(Of Object) = If((e.Start < 0 OrElse limit < 0), data, data.GetRange(e.Start, limit))
            ''-- end paging ------------------------------------------------------------
            e.Total = inttotalRecord
            gridFileList.GetStore.DataSource = DataPaging
            gridFileList.GetStore.DataBind()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub LoadData()
        Dim objdt As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListFile)

        storeFileList.DataSource = objdt
        storeFileList.DataBind()

        TanggalData.Format = NawaBLL.SystemParameterBLL.GetDateFormat
        colStartDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
        colCompleteDate.Format = NawaBLL.SystemParameterBLL.GetDateFormat + " HH:mm"
    End Sub

    Protected Sub GridCommand(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            Response.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/LHBU/DownloadLHBUSourceFile.ashx" & "?ID={0}&Val={1}", e.ExtraParams(0).Value, ObjModule.IsUseApproval))

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub GridCommandError(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            Response.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/LHBU/DownloadLHBUErrorDetail.ashx" & "?ID={0}&Val={1}", e.ExtraParams(0).Value, ObjModule.IsUseApproval))

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class
