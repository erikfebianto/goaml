Imports Ext.Net
Imports NawaDAL
Imports NawaBLL
Imports System.Data.SqlClient
Public Class EODSchedulerBLL
    Implements IDisposable
    Shared Function GetEODSchedulerByPK(id As Long) As NawaDAL.EODScheduler
        'Using objDb As NawaDevDAL.NawaDataDevEntities = New NawaDevDAL.NawaDataDevEntities
        '    objDb.MUsers.Where(Function(x) x.PK_MUser_ID = id).ToList()

        'End Using

        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return (From x In objDb.EODSchedulers Where x.PK_EODScheduler_ID = id Select x).FirstOrDefault
        End Using
    End Function

    Shared Function SaveEOD(datadate As Date, intprocessid As Long, objSchemaModule As [Module]) As Boolean




        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim oparamdatadate As SqlClient.SqlParameter
                    oparamdatadate = New SqlParameter
                    oparamdatadate.ParameterName = "@Datadate"
                    oparamdatadate.DbType = DbType.DateTime
                    oparamdatadate.Value = datadate

                    Dim oparamprocessid As SqlClient.SqlParameter
                    oparamprocessid = New SqlParameter
                    oparamprocessid.ParameterName = "@processid"
                    oparamprocessid.DbType = DbType.Int64
                    oparamprocessid.Value = intprocessid

                    Dim oparamuserid As SqlClient.SqlParameter
                    oparamuserid = New SqlParameter
                    oparamuserid.ParameterName = "@userID"
                    oparamuserid.DbType = DbType.String
                    oparamuserid.Value = NawaBLL.Common.SessionCurrentUser.UserID


                    objdb.Database.ExecuteSqlCommand("usp_insertEODSchedulerManual @Datadate,@processid,@userID", oparamdatadate, oparamprocessid, oparamuserid)


                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()


                    Dim objaudittraildetail As NawaDAL.AuditTrailDetail
                    objaudittraildetail = New NawaDAL.AuditTrailDetail
                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                    objaudittraildetail.FieldName = "DataDate"
                    objaudittraildetail.OldValue = ""
                    objaudittraildetail.NewValue = datadate.ToString(NawaBLL.SystemParameterBLL.GetDateFormat)                    
                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added


                    objaudittraildetail = New NawaDAL.AuditTrailDetail
                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                    objaudittraildetail.FieldName = "ProcessID"
                    objaudittraildetail.OldValue = ""
                    objaudittraildetail.NewValue = intprocessid
                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added


                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Function

    Sub SettingColor(objPanelOld As FormPanel, objPanelNew As FormPanel, objdata As String, objdatabefore As String, strmodulename As String, unikkeyold As String, unikkeynew As String)
        If objdata.Length > 0 And objdatabefore.Length > 0 Then
            'Dim objModuleData As NawaDAL.Module = ModuleBLL.Deserialize(objdata, GetType(NawaDAL.Module))
            'Dim objModuleDataBefore As NawaDAL.Module = ModuleBLL.Deserialize(objdata, GetType(NawaDAL.Module))
            Dim objdisplayOld As DisplayField
            Dim objdisplayNew As DisplayField
            objdisplayOld = objPanelOld.FindControl("PK_EODScheduler_ID" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("PK_EODScheduler_ID" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("EODSchedulerName" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("EODSchedulerName" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("EODSchedulerDescription" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("EODSchedulerDescription" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("EODPeriod" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("EODPeriod" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("EODPeriodType" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("EODPeriodType" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("StartDate" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("StartDate" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If
            objdisplayOld = objPanelOld.FindControl("Active" & unikkeyold)
            objdisplayNew = objPanelNew.FindControl("Active" & unikkeynew)
            If objdisplayOld.Text <> objdisplayNew.Text Then
                objdisplayOld.FieldStyle = "Color:red"
                objdisplayNew.FieldStyle = "Color:red"
            End If

        End If
    End Sub

    Shared Function Accept(ID As String) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            objModuledata.objScheduler.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objModuledata.objScheduler.ApprovedDate = Now
                            objdb.Entry(objModuledata.objScheduler).State = Entity.EntityState.Added
                            'audittrail

                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            For Each item As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                item.FK_EODSCheduler_ID = objModuledata.objScheduler.PK_EODScheduler_ID
                                objdb.Entry(item).State = Entity.EntityState.Added
                            Next
                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next
                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""
                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If
                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If
                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                        Case NawaBLL.Common.ModuleActionEnum.Update
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            Dim objModuledataOld As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(EODSchedulerDataBLL))
                            objModuledata.objScheduler.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objModuledata.objScheduler.ApprovedDate = Now
                            objdb.EODSchedulers.Attach(objModuledata.objScheduler)
                            objdb.Entry(objModuledata.objScheduler).State = Entity.EntityState.Modified
                            For Each itemx As EODSchedulerDetail In (From x In objdb.EODSchedulerDetails Where x.FK_EODSCheduler_ID = objModuledata.objScheduler.PK_EODScheduler_ID Select x).ToList
                                Dim objcek As EODSchedulerDetail = objModuledata.objSchedulerDetail.Find(Function(x) x.PK_EODSchedulerDetail_ID = itemx.PK_EODSchedulerDetail_ID)
                                If objcek Is Nothing Then
                                    objdb.Entry(itemx).State = Entity.EntityState.Deleted
                                End If
                            Next
                            For Each item As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                Dim obcek As EODSchedulerDetail = (From x In objdb.EODSchedulerDetails Where x.PK_EODSchedulerDetail_ID = item.PK_EODSchedulerDetail_ID Select x).FirstOrDefault
                                If obcek Is Nothing Then
                                    objdb.Entry(item).State = Entity.EntityState.Added
                                Else
                                    objdb.Entry(obcek).CurrentValues.SetValues(item)
                                    objdb.Entry(obcek).State = Entity.EntityState.Modified
                                End If
                            Next
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledataOld, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objModuledataOld, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Next
                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""
                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If
                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If
                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                        Case NawaBLL.Common.ModuleActionEnum.Delete
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            objdb.Entry(objModuledata.objScheduler).State = Entity.EntityState.Deleted
                            Dim objTaskDetail As List(Of EODSchedulerDetail) = objdb.EODSchedulerDetails.Where(Function(x) x.FK_EODSCheduler_ID = objModuledata.objScheduler.PK_EODScheduler_ID).ToList
                            For Each item As EODSchedulerDetail In objTaskDetail
                                objdb.Entry(item).State = Entity.EntityState.Deleted
                            Next
                            'audittrail
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next
                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""
                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If
                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If
                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                        Case NawaBLL.Common.ModuleActionEnum.Activation
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            objdb.Entry(objModuledata.objScheduler).State = Entity.EntityState.Modified
                            'audittrail
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next
                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""
                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If
                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If
                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                    End Select
                    objdb.Entry(objApproval).State = Entity.EntityState.Deleted
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function Reject(ID As String) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            'audittrail
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.objScheduler.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objScheduler, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objScheduler, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next



                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""


                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then

                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If

                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If




                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                        Case NawaBLL.Common.ModuleActionEnum.Update
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            Dim objModuledataOld As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(EODSchedulerDataBLL))


                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.objScheduler.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                If Not item.GetValue(objModuledataOld.objScheduler, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objModuledataOld.objScheduler, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                If Not item.GetValue(objModuledata.objScheduler, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objScheduler, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next


                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""


                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then

                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If

                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If




                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                        Case NawaBLL.Common.ModuleActionEnum.Delete
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            'audittrail
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.objScheduler.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objScheduler, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objScheduler, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""


                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then

                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If

                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If




                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next

                        Case NawaBLL.Common.ModuleActionEnum.Activation
                            Dim objModuledata As EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(EODSchedulerDataBLL))
                            'audittrail
                            Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objModuledata.objScheduler.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objScheduler, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objScheduler, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDAL.EODSchedulerDetail In objModuledata.objSchedulerDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                    objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                    objaudittraildetail.FieldName = item.Name
                                    objaudittraildetail.OldValue = ""


                                    If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                        If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then

                                            objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                        Else
                                            objaudittraildetail.NewValue = ""
                                        End If

                                    Else
                                        objaudittraildetail.NewValue = ""
                                    End If




                                    objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                Next
                            Next
                    End Select
                    objdb.Entry(objApproval).State = Entity.EntityState.Deleted
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function GetEODPeriodById(id As Integer) As NawaDAL.MsEODPeriod
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.MsEODPeriods.Where(Function(x) x.PK_MsEODPeriod_Id = id).FirstOrDefault()
        End Using
    End Function
    Shared Sub LoadPanel(objPanel As FormPanel, objdata As String, strModulename As String, unikkey As String)
        Dim objEODSchedulerBLL As NawaBLL.EODSchedulerDataBLL = NawaBLL.Common.Deserialize(objdata, GetType(NawaBLL.EODSchedulerDataBLL))
        Dim objEODScheduler As NawaDAL.EODScheduler
        objEODScheduler = objEODSchedulerBLL.objScheduler
        If Not objEODScheduler Is Nothing Then
            Dim strunik As String = unikkey
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" & strunik, objEODScheduler.PK_EODScheduler_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" & strunik, objEODScheduler.EODSchedulerName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" & strunik, objEODScheduler.EODSchedulerDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" & strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" & strunik, objEODScheduler.FK_MsEODPeriod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" & strunik, objEODScheduler.StartDate.GetValueOrDefault.ToString("dd-MMM-yyyy HH:mm:ss"))
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objEODScheduler.Active)
            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField
            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODSchedulerDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
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
            objColum.Text = "Task Name"
            objColum.DataIndex = "EODTaskName"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)
            objColum = New Ext.Net.Column
            objColum.Text = "Order No"
            objColum.DataIndex = "OrderNo"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objdt As Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objEODSchedulerBLL.objSchedulerDetail)
            Dim objcol As New Data.DataColumn
            objcol.ColumnName = "EODTaskName"
            objcol.DataType = GetType(String)
            objdt.Columns.Add(objcol)

            For Each item As DataRow In objdt.Rows
                Dim objtask As NawaDAL.EODTask = NawaBLL.EODTaskBLL.GetEODTaskByPK(item("FK_EODTask_ID"))
                If Not objtask Is Nothing Then
                    item("EODTaskName") = objtask.EODTaskName
                End If

            Next

            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdt)

            'Using objdatax As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam)

            'End Using
        End If
    End Sub

    Shared Function ActivationTanpaapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try


                    Dim objTaskdel As NawaDAL.EODScheduler = objdb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = ID).FirstOrDefault


                    objTaskdel.Active = Not objTaskdel.Active
                    objdb.Entry(objTaskdel).State = Entity.EntityState.Modified



                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objTaskdel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(objTaskdel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objTaskdel, Nothing)

                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next




                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using


    End Function
    Shared Function ActivationDenganapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim objTaskdel As NawaDAL.EODScheduler = GetEODSchedulerByPK(ID)
                    Dim objTaskDetail As List(Of NawaDAL.EODSchedulerDetail) = GetEODSchedulerDetailByFKID(ID)

                    objTaskdel.Active = Not objTaskdel.Active
                    Dim objTaskDataBLL As New NawaBLL.EODSchedulerDataBLL
                    objTaskDataBLL.objScheduler = objTaskdel
                    objTaskDataBLL.objSchedulerDetail = objTaskDetail


                    Dim xmldata As String = NawaBLL.Common.Serialize(objTaskDataBLL)
                    Dim objModuleApproval As New ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = objTaskDataBLL.objScheduler.PK_EODScheduler_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added



                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objTaskdel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(objTaskdel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objTaskdel, Nothing)

                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDAL.EODSchedulerDetail In objTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""


                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then

                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If

                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next


                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Shared Sub LoadPanelActivation(objPanel As FormPanel, strModulename As String, unikkey As String)
        Dim objEODScheduler As NawaDAL.EODScheduler = GetEODSchedulerByPK(unikkey)
        If Not objEODScheduler Is Nothing Then
            Dim strunik As String = Guid.NewGuid.ToString
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" & strunik, objEODScheduler.PK_EODScheduler_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" & strunik, objEODScheduler.EODSchedulerName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" & strunik, objEODScheduler.EODSchedulerDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" & strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" & strunik, objEODScheduler.FK_MsEODPeriod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" & strunik, objEODScheduler.StartDate.GetValueOrDefault.ToString("dd-MMM-yyyy HH:mm:ss"))
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objEODScheduler.Active.ToString & " -->" & (Not objEODScheduler.Active).ToString)
            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField
            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODSchedulerDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
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
            objColum.Text = "Task Name"
            objColum.DataIndex = "EODTaskName"
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
            objparam(0).ParameterName = "@EODSchedulerID"
            objparam(0).SqlDbType = SqlDbType.BigInt
            objparam(0).Value = objEODScheduler.PK_EODScheduler_ID
            Using objdata As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam)
                Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdata)
            End Using
        End If
    End Sub

    Shared Sub LoadPanelDelete(objPanel As FormPanel, strModulename As String, unikkey As String)
        Dim objEODScheduler As NawaDAL.EODScheduler = GetEODSchedulerByPK(unikkey)
        If Not objEODScheduler Is Nothing Then
            Dim strunik As String = Guid.NewGuid.ToString
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" & strunik, objEODScheduler.PK_EODScheduler_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" & strunik, objEODScheduler.EODSchedulerName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" & strunik, objEODScheduler.EODSchedulerDescription)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" & strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" & strunik, objEODScheduler.FK_MsEODPeriod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" & strunik, objEODScheduler.StartDate.GetValueOrDefault.ToString("dd-MMM-yyyy HH:mm:ss"))
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objEODScheduler.Active)
            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField
            objField = New Ext.Net.ModelField
            objField.Name = "PK_EODSchedulerDetail_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "EODTaskName"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)
            objField = New Ext.Net.ModelField
            objField.Name = "OrderNo"
            objField.Type = ModelFieldType.Int
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
            objColum.Text = "Task Name"
            objColum.DataIndex = "EODTaskName"
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
            objparam(0).ParameterName = "@EODSchedulerID"
            objparam(0).SqlDbType = SqlDbType.BigInt
            objparam(0).Value = objEODScheduler.PK_EODScheduler_ID
            Using objdata As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam)
                Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdata)
            End Using
        End If
    End Sub
    Shared Function DeleteDenganapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objSchedulerdel As NawaDAL.EODScheduler = objdb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = ID).FirstOrDefault
                    Dim objSchedulerDetail As List(Of NawaDAL.EODSchedulerDetail) = objdb.EODSchedulerDetails.Where(Function(x) x.FK_EODSCheduler_ID = ID).ToList
                    Dim objTaskDataBLL As New NawaBLL.EODSchedulerDataBLL
                    objTaskDataBLL.objScheduler = objSchedulerdel
                    objTaskDataBLL.objSchedulerDetail = objSchedulerDetail
                    Dim xmldata As String = NawaBLL.Common.Serialize(objTaskDataBLL)
                    Dim objModuleApproval As New ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = objTaskDataBLL.objScheduler.PK_EODScheduler_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objSchedulerdel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objSchedulerdel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objSchedulerdel, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function DeleteTanpaapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objTaskdel As NawaDAL.EODScheduler = objdb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = ID).FirstOrDefault
                    Dim objTaskDetail As List(Of NawaDAL.EODSchedulerDetail) = objdb.EODSchedulerDetails.Where(Function(x) x.FK_EODSCheduler_ID = ID).ToList
                    objdb.Entry(objTaskdel).State = Entity.EntityState.Deleted
                    For Each item As EODSchedulerDetail In objTaskDetail
                        objdb.Entry(item).State = Entity.EntityState.Deleted
                    Next
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objTaskdel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objTaskdel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objTaskdel, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function IsDataValidDelete(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Dim objdel As NawaDAL.EODScheduler = objdb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = ID).FirstOrDefault
            If Not objdel Is Nothing Then
                Dim objapprovaldel As NawaDAL.ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.ModuleName = objSchemaModule.ModuleName And x.ModuleKey = ID).FirstOrDefault()
                If Not objapprovaldel Is Nothing Then
                    Throw New Exception(objSchemaModule.ModuleLabel & " " & objdel.EODSchedulerName & " already exist in pending approval.")
                End If
            End If
        End Using
        Return True
    End Function
    Shared Function GetEODSchedulerByPKID(id As Long) As EODScheduler
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = id).FirstOrDefault
        End Using
    End Function
    Shared Function GetEODSchedulerDetailByFKID(id As Long) As List(Of EODSchedulerDetail)
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.EODSchedulerDetails.Where(Function(x) x.FK_EODSCheduler_ID = id).ToList
        End Using
    End Function
    Shared Function GetMsEODPeriods() As List(Of MsEODPeriod)
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.MsEODPeriods.ToList()
        End Using
    End Function
    Function SaveEditApproval(objData As NawaDAL.EODScheduler, objSchedulerDetail As List(Of NawaDAL.EODSchedulerDetail), objModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objxData As New NawaBLL.EODSchedulerDataBLL
                    objxData.objScheduler = objData
                    objxData.objSchedulerDetail = objSchedulerDetail
                    Dim xmldata As String = NawaBLL.Common.Serialize(objxData)

                    Dim objEODSchedulerbefore As NawaDAL.EODScheduler = objdb.EODSchedulers.Where(Function(x) x.PK_EODScheduler_ID = objxData.objScheduler.PK_EODScheduler_ID).FirstOrDefault
                    Dim obEODSchedulerDetailBefore As List(Of NawaDAL.EODSchedulerDetail) = objdb.EODSchedulerDetails.Where(Function(x) x.FK_EODSCheduler_ID = objxData.objScheduler.PK_EODScheduler_ID).ToList

                    Dim objxDatabefore As New NawaBLL.EODSchedulerDataBLL
                    objxDatabefore.objScheduler = objEODSchedulerbefore
                    objxDatabefore.objSchedulerDetail = obEODSchedulerDetailBefore
                    Dim xmlbefore As String = NawaBLL.Common.Serialize(objxDatabefore)


                    Dim objModuleApproval As New NawaDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = objxData.objScheduler.PK_EODScheduler_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = xmlbefore
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        If Not item.GetValue(objEODSchedulerbefore, Nothing) Is Nothing Then
                            objaudittraildetail.OldValue = item.GetValue(objEODSchedulerbefore, Nothing)
                        Else
                            objaudittraildetail.OldValue = ""
                        End If
                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objData, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Function SaveAddApproval(objData As NawaDAL.EODScheduler, objSchedulerDetail As List(Of NawaDAL.EODSchedulerDetail), objModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objxData As New NawaBLL.EODSchedulerDataBLL
                    objxData.objScheduler = objData
                    objxData.objSchedulerDetail = objSchedulerDetail
                    Dim xmldata As String = NawaBLL.Common.Serialize(objxData)

                    Dim objModuleApproval As New NawaDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = 0
                        .ModuleField = xmldata
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objData, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Function SaveEditTanpaApproval(objData As NawaDAL.EODScheduler, objSchedulerDetail As List(Of NawaDAL.EODSchedulerDetail), objModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objData.ApprovedDate = Now
                    objdb.Entry(objData).State = Entity.EntityState.Modified
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    For Each itemx As EODSchedulerDetail In (From x In objdb.EODSchedulerDetails Where x.FK_EODSCheduler_ID = objData.PK_EODScheduler_ID Select x).ToList
                        Dim objcek As EODSchedulerDetail = objSchedulerDetail.Find(Function(x) x.PK_EODSchedulerDetail_ID = itemx.PK_EODSchedulerDetail_ID)
                        If objcek Is Nothing Then
                            objdb.Entry(itemx).State = Entity.EntityState.Deleted
                        End If
                    Next
                    For Each item As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        Dim obcek As EODSchedulerDetail = (From x In objdb.EODSchedulerDetails Where x.PK_EODSchedulerDetail_ID = item.PK_EODSchedulerDetail_ID Select x).FirstOrDefault
                        If obcek Is Nothing Then
                            objdb.Entry(item).State = Entity.EntityState.Added
                        Else
                            objdb.Entry(obcek).CurrentValues.SetValues(item)
                            objdb.Entry(obcek).State = Entity.EntityState.Modified
                        End If
                    Next
                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objData, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Function SaveAddTanpaApproval(objData As NawaDAL.EODScheduler, objSchedulerDetail As List(Of NawaDAL.EODSchedulerDetail), objModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objData.ApprovedDate = Now
                    objdb.Entry(objData).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    For Each item As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        item.FK_EODSCheduler_ID = objData.PK_EODScheduler_ID
                        objdb.Entry(item).State = Entity.EntityState.Added
                    Next
                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objData, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objData, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDAL.EODSchedulerDetail In objSchedulerDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                            objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                            objaudittraildetail.FieldName = item.Name
                            objaudittraildetail.OldValue = ""
                            If Not item.GetValue(itemheader, Nothing) Is Nothing Then
                                If item.GetValue(itemheader, Nothing).GetType.ToString <> "System.Byte[]" Then
                                    objaudittraildetail.NewValue = item.GetValue(itemheader, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Else
                                objaudittraildetail.NewValue = ""
                            End If
                            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                        Next
                    Next
                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

#Region "Run Process Per Branch"
    Shared Function SaveEODBranch(dataDate As Date, processID As Long, taskList As String, kodeCabang As String, objModule As NawaDAL.[Module]) As Boolean
        Using objDB As New NawaDAL.NawaDataEntities
            Using objTrans As System.Data.Entity.DbContextTransaction = objDB.Database.BeginTransaction()
                Try
                    Dim paramDate As SqlClient.SqlParameter
                    paramDate = New SqlParameter
                    paramDate.ParameterName = "@Datadate"
                    paramDate.DbType = DbType.DateTime
                    paramDate.Value = dataDate

                    Dim paramProcess As SqlClient.SqlParameter
                    paramProcess = New SqlParameter
                    paramProcess.ParameterName = "@processid"
                    paramProcess.DbType = DbType.Int64
                    paramProcess.Value = processID

                    Dim paramUser As SqlClient.SqlParameter
                    paramUser = New SqlParameter
                    paramUser.ParameterName = "@userID"
                    paramUser.DbType = DbType.String
                    paramUser.Value = NawaBLL.Common.SessionCurrentUser.UserID

                    Dim paramTask As SqlClient.SqlParameter
                    paramTask = New SqlParameter
                    paramTask.ParameterName = "@taskid"
                    paramTask.DbType = DbType.String
                    paramTask.Value = taskList

                    Dim paramBranch As SqlClient.SqlParameter
                    paramBranch = New SqlParameter
                    paramBranch.ParameterName = "@kodecabang"
                    paramBranch.DbType = DbType.String
                    paramBranch.Value = kodeCabang

                    objDB.Database.ExecuteSqlCommand("usp_insertEODSchedulerManualBranch @Datadate,@processid,@taskid,@userID,@kodecabang", paramDate, paramProcess, paramTask, paramUser, paramBranch)

                    Dim objATHeader As New NawaDAL.AuditTrailHeader
                    objATHeader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objATHeader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objATHeader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objATHeader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objATHeader.ModuleLabel = objModule.ModuleLabel
                    objDB.Entry(objATHeader).State = Entity.EntityState.Added
                    objDB.SaveChanges()

                    Dim objATDetail As NawaDAL.AuditTrailDetail
                    objATDetail = New NawaDAL.AuditTrailDetail
                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                    objATDetail.FieldName = "DataDate"
                    objATDetail.OldValue = ""
                    objATDetail.NewValue = dataDate.ToString(NawaBLL.SystemParameterBLL.GetDateFormat)
                    objDB.Entry(objATDetail).State = Entity.EntityState.Added

                    objATDetail = New NawaDAL.AuditTrailDetail
                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                    objATDetail.FieldName = "ProcessID"
                    objATDetail.OldValue = ""
                    objATDetail.NewValue = processID.ToString
                    objDB.Entry(objATDetail).State = Entity.EntityState.Added

                    objATDetail = New NawaDAL.AuditTrailDetail
                    objATDetail.FK_AuditTrailHeader_ID = objATHeader.PK_AuditTrail_ID
                    objATDetail.FieldName = "KodeCabang"
                    objATDetail.OldValue = ""
                    objATDetail.NewValue = kodeCabang
                    objDB.Entry(objATDetail).State = Entity.EntityState.Added

                    objDB.SaveChanges()
                    objTrans.Commit()
                Catch ex As Exception
                    objTrans.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Function

#End Region

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
