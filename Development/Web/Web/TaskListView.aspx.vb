Imports System.Data
Imports Ext.Net
Imports OfficeOpenXml
Partial Class TaskListView
    Inherits ParentPage


    Dim objTaskList As New NawaBLL.TaskListBLL
    Public Property strWhereClause() As String
        Get
            Return Session("TaskListView.strWhereClause")
        End Get
        Set(ByVal value As String)
            Session("TaskListView.strWhereClause") = value
        End Set
    End Property
    Public Property strOrder() As String
        Get
            Return Session("TaskListView.strSort")
        End Get
        Set(ByVal value As String)
            Session("TaskListView.strSort") = value
        End Set
    End Property
    Public Property indexStart() As String
        Get
            Return Session("TaskListView.indexStart")
        End Get
        Set(ByVal value As String)
            Session("TaskListView.indexStart") = value
        End Set
    End Property
    'Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    objFormModuleView = New NawaBLL.FormModuleView(Me.GridpanelView, Me.BtnAdd)
    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Dim Moduleid As String = Request.Params("ModuleID")
            'Dim intModuleid As Integer
            'Try
            '    intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            '    Dim objmodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)


            '    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
            '        Dim strIDCode As String = 1
            '        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

            '        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
            '        Exit Sub
            '    End If

            '    objFormModuleView.ModuleID = objmodule.PK_Module_ID
            '    objFormModuleView.ModuleName = objmodule.ModuleName



            '    'objFormModuleView.AddField("PK_MUser_ID", "ID", 1, True, True, NawaBLL.Common.MFieldType.IDENTITY,,,,, )
            '    'objFormModuleView.AddField("UserID", "User ID", 2, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
            '    'objFormModuleView.AddField("UserName", "User Name", 3, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, )
            '    'objFormModuleView.AddField("FK_MRole_ID", "Role Name", 4, False, True, NawaBLL.Common.MFieldType.ReferenceTable, "MRole", "PK_MRole_ID", "RoleName")
            '    'objFormModuleView.AddField("FK_MGroupMenu_ID", "Group Name", 5, False, True, NawaBLL.Common.MFieldType.ReferenceTable, "MGroupMenu", "PK_MGroupMenu_ID", "GroupMenuName")
            '    'objFormModuleView.AddField("UserEmailAddress", "Email Address", 6, False, True, NawaBLL.Common.MFieldType.VARCHARValue,,,,, 250)
            '    'objFormModuleView.AddField("IPAddress", "IP Address", 7, False, False, NawaBLL.Common.MFieldType.VARCHARValue)
            '    'objFormModuleView.AddField("InUsed", "In Used", 8, False, False, NawaBLL.Common.MFieldType.BooleanValue)
            '    'objFormModuleView.AddField("IsDisabled", "Is Disabled", 9, False, False, NawaBLL.Common.MFieldType.BooleanValue)
            '    'objFormModuleView.AddField("LastLogin", "Last Login", 10, False, False, NawaBLL.Common.MFieldType.DATETIMEValue)
            '    'objFormModuleView.AddField("LastChangePassword", "Last Change Password", 11, False, False, NawaBLL.Common.MFieldType.DATETIMEValue)





            'Catch ex As Exception

            'End Try




            LoadTasklist()
            GridTaskList.Title = ObjModule.ModuleLabel

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Sub LoadTasklist()

        objTaskList.GetTasklist(GridTaskList)

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
            If Me.strOrder = "" Then Me.strOrder = " jumlah desc"

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



    Private Sub Parameterview_Init(sender As Object, e As EventArgs) Handles Me.Init

        ActionType = NawaBLL.Common.ModuleActionEnum.view
    End Sub

End Class

