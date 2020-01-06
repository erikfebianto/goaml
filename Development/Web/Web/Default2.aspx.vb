Imports Ext.Net
Imports System.Data
Imports NawaDAL

Public Class Default2
    Inherits Parent
    Public Property strWhereClause() As String
        Get
            Return Session("DefaultTaskList.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("DefaultTaskList.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("DefaultTaskList.strSort")
        End Get
        Set(ByVal value As String)
            Session("DefaultTaskList.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("DefaultTaskList.indexStart")
        End Get
        Set(ByVal value As String)
            Session("DefaultTaskList.indexStart") = value
        End Set
    End Property
    Dim objTaskList As New NawaBLL.TaskListBLL
    Public objFormModuleView As NawaBLL.FormModuleView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                ClearSession()

                'Check hak akses menu Getting Started
                Using objDb As New NawaDataEntities
                    Dim objModuleStarted As [Module] = objDb.Modules.Where(Function(x) x.ModuleName = "GettingStarted").FirstOrDefault()

                    If Not objModuleStarted Is Nothing Then
                        PortalShortcut.Hidden = Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objModuleStarted.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view)
                    End If
                End Using
            End If

            LoadTasklist()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub ClearSession()
        strWhereClause = ""
        Me.strOrder = ""
        indexStart = ""
    End Sub

    Sub LoadTasklist()
        objTaskList.GetTasklist(GridTaskList)
    End Sub
    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
        objFormModuleView = New NawaBLL.FormModuleView(Me.GridApproval, Nothing)
    End Sub

    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start

            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objTaskList.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            Me.indexStart = intStart
            Me.strWhereClause = strfilter
            Me.strOrder = strsort
            If Me.strOrder = "" Then Me.strOrder = " ModuleName"

            'Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging(" ModuleApproval iNNER JOIN ModuleAction ON moduleapproval.PK_ModuleAction_ID=moduleaction.PK_ModuleAction_ID", "PK_ModuleApproval_ID, ModuleName, ModuleKey,CreatedDate, moduleaction.ModuleActionName AS ActionName,CreatedBy", strWhereClause, strsort, intStart - 1, intLimit, inttotalRecord)

            Dim paramtasklist(6) As SqlClient.SqlParameter
            paramtasklist(0) = New SqlClient.SqlParameter
            paramtasklist(0).ParameterName = "@userid"
            paramtasklist(0).Value = NawaBLL.Common.SessionCurrentUser.UserID

            paramtasklist(1) = New SqlClient.SqlParameter
            paramtasklist(1).ParameterName = "@groupmenuid"
            paramtasklist(1).Value = NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID

            paramtasklist(2) = New SqlClient.SqlParameter
            paramtasklist(2).ParameterName = "@actionid"
            paramtasklist(2).Value = NawaBLL.Common.ModuleActionEnum.Approval

            paramtasklist(3) = New SqlClient.SqlParameter
            paramtasklist(3).ParameterName = "@orderby"
            paramtasklist(3).Value = strOrder

            paramtasklist(4) = New SqlClient.SqlParameter
            paramtasklist(4).ParameterName = "@whereclause"
            paramtasklist(4).Value = Me.strWhereClause

            paramtasklist(5) = New SqlClient.SqlParameter
            paramtasklist(5).ParameterName = "@PageIndex"
            paramtasklist(5).Value = intStart

            paramtasklist(6) = New SqlClient.SqlParameter
            paramtasklist(6).ParameterName = "@PageSize"
            paramtasklist(6).Value = NawaBLL.SystemParameterBLL.GetPageSize

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskList", paramtasklist)

            ''-- start paging ------------------------------------------------------------
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If
            'Dim rangeData As List(Of Object) = If((e.Start < 0 OrElse limit < 0), data, data.GetRange(e.Start, limit))
            ''-- end paging ------------------------------------------------------------
            e.Total = inttotalRecord
            GridTaskList.GetStore.DataSource = DataPaging
            GridTaskList.GetStore.DataBind()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub StoreApproval_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start

            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = objFormModuleView.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            Me.indexStart = intStart
            Me.strWhereClause = strfilter
            Me.strOrder = strsort
            If Me.strOrder = "" Then Me.strOrder = " ModuleName"

            'Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging(" ModuleApproval iNNER JOIN ModuleAction ON moduleapproval.PK_ModuleAction_ID=moduleaction.PK_ModuleAction_ID", "PK_ModuleApproval_ID, ModuleName, ModuleKey,CreatedDate, moduleaction.ModuleActionName AS ActionName,CreatedBy", strWhereClause, strsort, intStart - 1, intLimit, inttotalRecord)

            Dim paramtasklist(6) As SqlClient.SqlParameter
            paramtasklist(0) = New SqlClient.SqlParameter
            paramtasklist(0).ParameterName = "@userid"
            paramtasklist(0).Value = NawaBLL.Common.SessionCurrentUser.UserID

            paramtasklist(1) = New SqlClient.SqlParameter
            paramtasklist(1).ParameterName = "@groupmenuid"
            paramtasklist(1).Value = NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID

            paramtasklist(2) = New SqlClient.SqlParameter
            paramtasklist(2).ParameterName = "@actionid"
            paramtasklist(2).Value = NawaBLL.Common.ModuleActionEnum.Approval

            paramtasklist(3) = New SqlClient.SqlParameter
            paramtasklist(3).ParameterName = "@orderby"
            paramtasklist(3).Value = strOrder

            paramtasklist(4) = New SqlClient.SqlParameter
            paramtasklist(4).ParameterName = "@whereclause"
            paramtasklist(4).Value = Me.strWhereClause

            paramtasklist(5) = New SqlClient.SqlParameter
            paramtasklist(5).ParameterName = "@PageIndex"
            paramtasklist(5).Value = intStart

            paramtasklist(6) = New SqlClient.SqlParameter
            paramtasklist(6).ParameterName = "@PageSize"
            paramtasklist(6).Value = NawaBLL.SystemParameterBLL.GetPageSize

            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskList_Waiting", paramtasklist)

            ''-- start paging ------------------------------------------------------------
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If
            'Dim rangeData As List(Of Object) = If((e.Start < 0 OrElse limit < 0), data, data.GetRange(e.Start, limit))
            ''-- end paging ------------------------------------------------------------
            e.Total = inttotalRecord
            Dim Store = GridApproval.GetStore()

            'For Each s In DataPaging.Rows
            '    s(2) = System.Web.VirtualPathUtility.ToAbsolute("~/") + s(2) + "?ModuleID=" + NawaBLL.Common.EncryptQueryString(s(3), NawaBLL.SystemParameterBLL.GetEncriptionKey)
            'Next

            Store.DataSource = DataPaging
            Store.DataBind()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub ApprovalDetail_OnClicked(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            Response.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & e.ExtraParams(0).Value & "?ModuleID={0}", NawaBLL.Common.EncryptQueryString(e.ExtraParams(1).Value, NawaBLL.SystemParameterBLL.GetEncriptionKey)))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class