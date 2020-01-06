Imports Ext.Net
Imports NawaBLL

<Serializable()>
Public Class EODTaskBLL
    Implements IDisposable
    Public oPanelinput As FormPanel

    Sub New(ObjPanel As Ext.Net.FormPanel)
        oPanelinput = ObjPanel
    End Sub
    Sub New()

    End Sub

#Region "Comment"
    'Shared Function IshasParameter(strProcedurename As String, strParameter As String) As Boolean
    '    'ambil list dari db informationschema.parameters lalu cek
    '    Dim strsql As String = "SELECT COUNT(1) FROM INFORMATION_SCHEMA.PARAMETERS p WHERE p.SPECIFIC_NAME='" & strProcedurename & "' AND p.PARAMETER_NAME='" & strParameter & "'"
    '    If Convert.ToInt32(NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)) = 0 Then
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function
    'Shared Function GetEODTaskActive() As List(Of EODTask)
    '    Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
    '        Return objDb.EODTasks.Where(Function(x) x.Active = True).ToList
    '    End Using
    'End Function


    'Shared Function IsDataValidDelete(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
    '    Using objdb As New NawaDAL.NawaDataEntities
    '        Dim objdel As NawaDAL.EODTask = objdb.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
    '        If Not objdel Is Nothing Then
    '            Dim objapprovaldel As NawaDAL.ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.ModuleName = objSchemaModule.ModuleName And x.ModuleKey = ID).FirstOrDefault()
    '            If Not objapprovaldel Is Nothing Then

    '                Throw New Exception(objSchemaModule.ModuleLabel & " " & objdel.EODTaskName & " already exist in pending approval.")
    '            End If
    '        End If
    '    End Using
    '    Return True
    'End Function

    'Shared Function GetEODTaskByPK(ID As Long) As EODTask
    '    Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
    '        Return objDb.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
    '    End Using

    'End Function

#End Region
    Shared Function GetEODTaskByPK(ID As Long) As NawaDevDAL.EODTask
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
        End Using

    End Function
    Sub LoadPanel(objPanel As FormPanel, objData As String, unikID As String)
        Dim objEODTaskBLL As NawaDevBLL.EODTaskDataBLL = NawaBLL.Common.Deserialize(objData, GetType(NawaDevBLL.EODTaskDataBLL))
        If Not objEODTaskBLL Is Nothing Then
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODTask_ID" & unikID, objEODTaskBLL.ObjEODTask.PK_EODTask_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Name", "EODTaskName" & unikID, objEODTaskBLL.ObjEODTask.EODTaskName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Description", "EODTaskDescription" & unikID, objEODTaskBLL.ObjEODTask.EODTaskDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & unikID, objEODTaskBLL.ObjEODTask.Active.ToString)

            Dim objStore As New Ext.Net.Store
            objStore.ID = unikID & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODTaskDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskDetailType"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SSISName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "StoreProcedureName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "Keterangan"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterProcessDate"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterBranch"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objStore.Model.Add(objModel)

            Dim objListcolumn As New List(Of ColumnBase)

            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumn.Add(objcolumnNo)
            End Using

            Dim objColumn As Ext.Net.Column

            objColumn = New Ext.Net.Column
            objColumn.Text = "Task Detail Type"
            objColumn.DataIndex = "EODTaskDetailType"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "SSIS Name"
            objColumn.DataIndex = "SSISName"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "Store Procedure Name"
            objColumn.DataIndex = "StoreProcedureName"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "Use Parameter Data Date"
            objColumn.DataIndex = "IsUseParameterProcessDate"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "Use Parameter Branch"
            objColumn.DataIndex = "IsUseParameterBranch"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "Description"
            objColumn.DataIndex = "Keterangan"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            objColumn = New Ext.Net.Column
            objColumn.Text = "Order No"
            objColumn.DataIndex = "OrderNo"
            objColumn.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumn.Flex = 1
            objListcolumn.Add(objColumn)

            Dim objparam(0) As System.Data.SqlClient.SqlParameter
            objparam(0) = New SqlClient.SqlParameter
            objparam(0).ParameterName = "@EODTASKID"
            objparam(0).SqlDbType = SqlDbType.BigInt
            objparam(0).Value = objEODTaskBLL.ObjEODTask.PK_EODTask_ID

            Dim objdt As DataTable = NawaBLL.Common.CopyGenericToDataTable(objEODTaskBLL.ObjEODTaskDetail)

            Dim objcol As New DataColumn
            objcol.ColumnName = "EODTaskDetailType"
            objcol.DataType = GetType(String)
            objdt.Columns.Add(objcol)

            For Each item As DataRow In objdt.Rows
                Select Case item.Item("FK_EODTaskDetailType_ID")
                    Case 1
                        item.Item("EODTaskDetailType") = "SSIS"
                    Case 2
                        item.Item("EODTaskDetailType") = "Store Procedure"
                    Case 3
                        item.Item("EODTaskDetailType") = "SSIS in SQL Agent"
                    Case 4
                        item.Item("EODTaskDetailType") = "API"
                End Select
            Next

            'Using objdatasource As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskDetailByTaskID", objparam)
            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Task Detail", objStore, objListcolumn, objdt)
            'End Using
        End If
    End Sub
    Sub LoadPanelDelete(objPanel As FormPanel, strModulename As String, unikkey As String)
        Dim objEODTask As NawaDevDAL.EODTask = GetEODTaskByID(unikkey)
        If Not objEODTask Is Nothing Then
            Dim strunik As String = Guid.NewGuid.ToString
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODTask_ID" & strunik, objEODTask.PK_EODTask_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Name", "EODTaskName" & strunik, objEODTask.EODTaskName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Description", "EODTaskDescription" & strunik, objEODTask.EODTaskDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objEODTask.Active)

            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODTaskDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskDetailType"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SSISName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "StoreProcedureName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "Keterangan"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterProcessDate"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterBranch"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objStore.Model.Add(objModel)

            Dim objListcolumn As New List(Of ColumnBase)

            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumn.Add(objcolumnNo)
            End Using

            Dim objColum As Ext.Net.Column

            objColum = New Ext.Net.Column
            objColum.Text = "Task Detail Type"
            objColum.DataIndex = "EODTaskDetailType"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "SSIS Name"
            objColum.DataIndex = "SSISName"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Store Procedure Name"
            objColum.DataIndex = "StoreProcedureName"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Use Parameter Process Date"
            objColum.DataIndex = "IsUseParameterProcessDate"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Use Parameter Branch"
            objColum.DataIndex = "IsUseParameterBranch"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Description"
            objColum.DataIndex = "Keterangan"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Order No"
            objColum.DataIndex = "OrderNo"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objparam(0) As System.Data.SqlClient.SqlParameter
            objparam(0) = New SqlClient.SqlParameter
            objparam(0).ParameterName = "@EODTASKID"
            objparam(0).SqlDbType = SqlDbType.BigInt
            objparam(0).Value = objEODTask.PK_EODTask_ID

            Using objdata As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskDetailByTaskID", objparam)
                Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Task Detail", objStore, objListcolumn, objdata)
            End Using

        End If
    End Sub
    Sub LoadPanelActivation(objPanel As FormPanel, strModulename As String, unikkey As String)
        Dim objEODTask As NawaDevDAL.EODTask = GetEODTaskByID(unikkey)
        If Not objEODTask Is Nothing Then
            Dim strunik As String = Guid.NewGuid.ToString
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODTask_ID" & strunik, objEODTask.PK_EODTask_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Name", "EODTaskName" & strunik, objEODTask.EODTaskName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Task Description", "EODTaskDescription" & strunik, objEODTask.EODTaskDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objEODTask.Active.ToString & " ==> " & (Not objEODTask.Active).ToString)

            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODTaskDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskDetailType"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SSISName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "StoreProcedureName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "Keterangan"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterProcessDate"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "IsUseParameterBranch"
            objField.Type = ModelFieldType.Boolean
            objModel.Fields.Add(objField)

            objStore.Model.Add(objModel)

            Dim objListcolumn As New List(Of ColumnBase)

            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumn.Add(objcolumnNo)
            End Using

            Dim objColum As Ext.Net.Column

            objColum = New Ext.Net.Column
            objColum.Text = "Task Detail Type"
            objColum.DataIndex = "EODTaskDetailType"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "SSIS Name"
            objColum.DataIndex = "SSISName"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Store Procedure Name"
            objColum.DataIndex = "StoreProcedureName"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Use Parameter Process Date"
            objColum.DataIndex = "IsUseParameterProcessDate"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Use Parameter Branch"
            objColum.DataIndex = "IsUseParameterBranch"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)


            objColum = New Ext.Net.Column
            objColum.Text = "Description"
            objColum.DataIndex = "Keterangan"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Order No"
            objColum.DataIndex = "OrderNo"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objparam(0) As System.Data.SqlClient.SqlParameter
            objparam(0) = New SqlClient.SqlParameter
            objparam(0).ParameterName = "@EODTASKID"
            objparam(0).SqlDbType = SqlDbType.BigInt
            objparam(0).Value = objEODTask.PK_EODTask_ID

            Using objdata As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTaskDetailByTaskID", objparam)
                Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Task Detail", objStore, objListcolumn, objdata)
            End Using

        End If
    End Sub

    Function GetEODTaskByID(id As Long) As NawaDevDAL.EODTask
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.EODTasks Where x.PK_EODTask_ID = id Select x).FirstOrDefault
        End Using
    End Function
    Function GetListEODTaskDetailByID(taskID As Long) As List(Of NawaDevDAL.EODTaskDetail)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.EODTaskDetails Where x.FK_EODTask_ID = taskID Select x).ToList
        End Using
    End Function
    Function IsExistInPendingApproval(ID As Long, objModule As NawaDAL.[Module]) As Boolean
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.ModuleApprovals.Where(Function(x) x.ModuleKey = ID And x.ModuleName = objModule.ModuleName).Count > 0
        End Using
    End Function

    Sub SaveAddApproval(objTask As NawaDevDAL.EODTask, listTaskDetail As List(Of NawaDevDAL.EODTaskDetail), objModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objData As New NawaDevBLL.EODTaskDataBLL
                    objData.ObjEODTask = objTask
                    objData.ObjEODTaskDetail = listTaskDetail
                    Dim dataXML As String = NawaBLL.Common.Serialize(objData)

                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = 0
                        .ModuleField = dataXML
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objDB.Entry(objModuleApproval).State = Entity.EntityState.Added

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objATHeader.ModuleLabel = objModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objData, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemDetail As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemDetail.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""

                            If Not item.GetValue(itemDetail, Nothing) Is Nothing Then
                                If item.GetValue(itemDetail, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemDetail, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objDB.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
    Sub SaveAddTanpaApproval(objTask As NawaDevDAL.EODTask, listTaskDetail As List(Of NawaDevDAL.EODTaskDetail), objModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    objTask.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objTask.ApprovedDate = Now
                    objDB.Entry(objTask).State = Entity.EntityState.Added

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objATHeader.ModuleLabel = objModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    For Each item As NawaDevDAL.EODTaskDetail In listTaskDetail
                        item.FK_EODTask_ID = objTask.PK_EODTask_ID
                        objDB.Entry(item).State = Entity.EntityState.Added
                    Next

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemDetail As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemDetail.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemDetail, Nothing) Is Nothing Then
                                If item.GetValue(itemDetail, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemDetail, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If

                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Sub SaveEditApproval(objTask As NawaDevDAL.EODTask, listTaskDetail As List(Of NawaDevDAL.EODTaskDetail), objModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objData As New NawaDevBLL.EODTaskDataBLL
                    objData.ObjEODTask = objTask
                    objData.ObjEODTaskDetail = listTaskDetail
                    Dim dataXML As String = NawaBLL.Common.Serialize(objData)

                    Dim objTaskBefore As NawaDevDAL.EODTask = objDB.EODTasks.Where(Function(x) x.PK_EODTask_ID = objTask.PK_EODTask_ID).FirstOrDefault
                    Dim objTaskDetailBefore As List(Of NawaDevDAL.EODTaskDetail) = objDB.EODTaskDetails.Where(Function(x) x.FK_EODTask_ID = objTask.PK_EODTask_ID).ToList

                    Dim objDataBefore As New NawaDevBLL.EODTaskDataBLL
                    objDataBefore.ObjEODTask = objTaskBefore
                    objDataBefore.ObjEODTaskDetail = objTaskDetailBefore
                    Dim dataXMLBefore As String = NawaBLL.Common.Serialize(objDataBefore)

                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = objTask.PK_EODTask_ID
                        .ModuleField = dataXML
                        .ModuleFieldBefore = dataXMLBefore
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objDB.Entry(objModuleApproval).State = Entity.EntityState.Added

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objATHeader.ModuleLabel = objModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name

                        If Not item.GetValue(objTaskBefore, Nothing) Is Nothing Then
                            objATDetail.OldValue = item.GetValue(objTaskBefore, Nothing)
                        Else
                            objATDetail.OldValue = ""
                        End If

                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If
                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
    Sub SaveEditTanpaApproval(objTask As NawaDevDAL.EODTask, listTaskDetail As List(Of NawaDevDAL.EODTaskDetail), objModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    objTask.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objTask.ApprovedDate = Now
                    objDB.Entry(objTask).State = Entity.EntityState.Modified

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objATHeader.ModuleLabel = objModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    For Each itemx As NawaDevDAL.EODTaskDetail In (From x In objDB.EODTaskDetails Where x.FK_EODTask_ID = objTask.PK_EODTask_ID Select x).ToList
                        Dim objcek As NawaDevDAL.EODTaskDetail = listTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = itemx.PK_EODTaskDetail_ID)
                        If objcek Is Nothing Then
                            objDB.Entry(itemx).State = Entity.EntityState.Deleted
                        End If
                    Next

                    For Each item As NawaDevDAL.EODTaskDetail In listTaskDetail
                        Dim obcek As NawaDevDAL.EODTaskDetail = (From x In objDB.EODTaskDetails Where x.PK_EODTaskDetail_ID = item.PK_EODTaskDetail_ID Select x).FirstOrDefault
                        If obcek Is Nothing Then
                            objDB.Entry(item).State = Entity.EntityState.Added
                        Else
                            objDB.Entry(obcek).CurrentValues.SetValues(item)
                            objDB.Entry(obcek).State = Entity.EntityState.Modified
                        End If
                    Next

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If

                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Sub DeleteDenganapproval(ID As String, objSchemaModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objTask As NawaDevDAL.EODTask = objDB.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
                    Dim listTaskDetail As List(Of NawaDevDAL.EODTaskDetail) = objDB.EODTaskDetails.Where(Function(x) x.FK_EODTask_ID = ID).ToList

                    Dim objTaskDataBLL As New NawaDevBLL.EODTaskDataBLL
                    objTaskDataBLL.ObjEODTask = objTask
                    objTaskDataBLL.ObjEODTaskDetail = listTaskDetail

                    Dim dataXML As String = NawaBLL.Common.Serialize(objTaskDataBLL)
                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = ID
                        .ModuleField = dataXML
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objDB.Entry(objModuleApproval).State = Entity.EntityState.Added

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objATHeader.ModuleLabel = objSchemaModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If
                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
    Sub DeleteTanpaapproval(ID As String, objSchemaModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objTask As NawaDevDAL.EODTask = objDB.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
                    Dim listTaskDetail As List(Of NawaDevDAL.EODTaskDetail) = objDB.EODTaskDetails.Where(Function(x) x.FK_EODTask_ID = ID).ToList

                    objDB.Entry(objTask).State = Entity.EntityState.Deleted

                    For Each item As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objDB.Entry(item).State = Entity.EntityState.Deleted
                    Next

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objATHeader.ModuleLabel = objSchemaModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If
                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Sub ActivationDenganapproval(ID As String, objSchemaModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objTask As NawaDevDAL.EODTask = GetEODTaskByID(ID)
                    Dim listTaskDetail As List(Of NawaDevDAL.EODTaskDetail) = GetListEODTaskDetailByID(ID)

                    objTask.Active = Not objTask.Active
                    Dim objTaskDataBLL As New NawaDevBLL.EODTaskDataBLL
                    objTaskDataBLL.ObjEODTask = objTask
                    objTaskDataBLL.ObjEODTaskDetail = listTaskDetail

                    Dim dataXML As String = NawaBLL.Common.Serialize(objTaskDataBLL)
                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = ID
                        .ModuleField = dataXML
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objDB.Entry(objModuleApproval).State = Entity.EntityState.Added

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                    objATHeader.ModuleLabel = objSchemaModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.EODTaskDetail In listTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                            objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                            objATDetail.FieldName = item.Name
                            objATDetail.OldValue = ""

                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                            Else
                                objATDetail.NewValue = ""
                            End If
                            objDB.Entry(objATDetail).State = Entity.EntityState.Added
                        Next
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
    Sub ActivationTanpaapproval(ID As String, objSchemaModule As NawaDAL.Module)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objTask As NawaDevDAL.EODTask = objDB.EODTasks.Where(Function(x) x.PK_EODTask_ID = ID).FirstOrDefault
                    objTask.Active = Not objTask.Active
                    objDB.Entry(objTask).State = Entity.EntityState.Modified

                    Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                    objATHeader.ModuleLabel = objSchemaModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objtype As Type = objTask.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                        objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                        objATDetail.FieldName = item.Name
                        objATDetail.OldValue = ""

                        If Not item.GetValue(objTask, Nothing) Is Nothing Then
                            objATDetail.NewValue = item.GetValue(objTask, Nothing)
                        Else
                            objATDetail.NewValue = ""
                        End If
                        objDB.Entry(objATDetail).State = Entity.EntityState.Added
                    Next

                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Sub Accept(ID As String)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objApproval As NawaDevDAL.ModuleApproval = objDB.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objDB.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If

                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))
                            objModuledata.ObjEODTask.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objModuledata.ObjEODTask.ApprovedDate = Now
                            objDB.Entry(objModuledata.ObjEODTask).State = Entity.EntityState.Added

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            For Each item As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                item.FK_EODTask_ID = objModuledata.ObjEODTask.PK_EODTask_ID
                                objDB.Entry(item).State = Entity.EntityState.Added
                            Next

                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Update
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))
                            Dim objModuledataOld As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(EODTaskDataBLL))

                            objModuledata.ObjEODTask.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objModuledata.ObjEODTask.ApprovedDate = Now
                            objDB.EODTasks.Attach(objModuledata.ObjEODTask)
                            objDB.Entry(objModuledata.ObjEODTask).State = Entity.EntityState.Modified

                            For Each itemx As NawaDevDAL.EODTaskDetail In (From x In objDB.EODTaskDetails Where x.FK_EODTask_ID = objModuledata.ObjEODTask.PK_EODTask_ID Select x).ToList
                                Dim objcek As NawaDevDAL.EODTaskDetail = objModuledata.ObjEODTaskDetail.Find(Function(x) x.PK_EODTaskDetail_ID = itemx.PK_EODTaskDetail_ID)
                                If objcek Is Nothing Then
                                    objDB.Entry(itemx).State = Entity.EntityState.Deleted
                                End If
                            Next

                            For Each item As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                Dim obcek As NawaDevDAL.EODTaskDetail = (From x In objDB.EODTaskDetails Where x.PK_EODTaskDetail_ID = item.PK_EODTaskDetail_ID Select x).FirstOrDefault
                                If obcek Is Nothing Then
                                    objDB.Entry(item).State = Entity.EntityState.Added
                                Else
                                    objDB.Entry(obcek).CurrentValues.SetValues(item)
                                    objDB.Entry(obcek).State = Entity.EntityState.Modified
                                End If
                            Next

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledataOld, Nothing) Is Nothing Then
                                    objATDetail.OldValue = item.GetValue(objModuledataOld, Nothing)
                                Else
                                    objATDetail.OldValue = ""
                                End If
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Delete
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))
                            objDB.Entry(objModuledata.ObjEODTask).State = Entity.EntityState.Deleted

                            Dim listTaskDetail As List(Of NawaDevDAL.EODTaskDetail) = objDB.EODTaskDetails.Where(Function(x) x.FK_EODTask_ID = objModuledata.ObjEODTask.PK_EODTask_ID).ToList
                            For Each item As NawaDevDAL.EODTaskDetail In listTaskDetail
                                objDB.Entry(item).State = Entity.EntityState.Deleted
                            Next

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Activation
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))
                            objDB.Entry(objModuledata.ObjEODTask).State = Entity.EntityState.Modified

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next
                    End Select

                    objDB.Entry(objApproval).State = Entity.EntityState.Deleted
                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
    Sub Reject(ID As String)
        Using objDB As New NawaDevDAL.NawaDatadevEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim objApproval As NawaDevDAL.ModuleApproval = objDB.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objDB.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If

                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.ObjEODTask.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata.ObjEODTask, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata.ObjEODTask, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Update
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))
                            Dim objModuledataOld As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(EODTaskDataBLL))

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.ObjEODTask.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                If Not item.GetValue(objModuledataOld.ObjEODTask, Nothing) Is Nothing Then
                                    objATDetail.OldValue = item.GetValue(objModuledataOld.ObjEODTask, Nothing)
                                Else
                                    objATDetail.OldValue = ""
                                End If
                                If Not item.GetValue(objModuledata.ObjEODTask, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata.ObjEODTask, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Delete
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.ObjEODTask.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata.ObjEODTask, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata.ObjEODTask, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Activation
                            Dim objModuledata As EODTaskDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODTaskDataBLL))

                            Dim objATHeader As New NawaDevDAL.AuditTrailHeader
                            objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objATHeader.ModuleLabel = objModule.ModuleLabel
                            objDB.Entry(objATHeader).State = Entity.EntityState.Added
                            objDB.SaveChanges()

                            Dim objtype As Type = objModuledata.ObjEODTask.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                objATDetail.FieldName = item.Name
                                objATDetail.OldValue = ""
                                If Not item.GetValue(objModuledata.ObjEODTask, Nothing) Is Nothing Then
                                    objATDetail.NewValue = item.GetValue(objModuledata.ObjEODTask, Nothing)
                                Else
                                    objATDetail.NewValue = ""
                                End If
                                objDB.Entry(objATDetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.EODTaskDetail In objModuledata.ObjEODTaskDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objATDetail As New NawaDevDAL.AuditTrailDetail
                                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                                    objATDetail.FieldName = item.Name
                                    objATDetail.OldValue = ""

                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objATDetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objATDetail.NewValue = ""
                                        End If
                                    Else
                                        objATDetail.NewValue = ""
                                    End If

                                    objDB.Entry(objATDetail).State = Entity.EntityState.Added
                                Next
                            Next
                    End Select

                    objDB.Entry(objApproval).State = Entity.EntityState.Deleted
                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class