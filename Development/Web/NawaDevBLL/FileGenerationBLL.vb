'Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL

Imports Ext.Net
Imports System.Text
Imports System.Data.SqlClient
Public Class FileGenerationBLL


    Shared Function GenerateTextFile(reportdate As DateTime, kodecabang As String, username As String, ListGenerateFileTemplate As String) As Boolean

        Dim objListParam(3) As SqlParameter
        objListParam(0) = New SqlParameter
        objListParam(1) = New SqlParameter
        objListParam(2) = New SqlParameter
        objListParam(3) = New SqlParameter




        objListParam(0).ParameterName = "@ReportDate"
        objListParam(0).Value = reportdate

        objListParam(1).ParameterName = "@kodecabang"
        objListParam(1).Value = kodecabang


        objListParam(2).ParameterName = "@UserName"
        objListParam(2).Value = username




        objListParam(3).ParameterName = "@ListGenerateFileTemplate"
        objListParam(3).Value = ListGenerateFileTemplate




        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GenerateAntTextRequest", objListParam)

    End Function

    Shared Function GenerateTextFileFromWeb(tanggalData As DateTime, userName As String, kodeForm As String) As Boolean
        DeletePrevious(tanggalData, kodeForm)
        InsertBase(tanggalData, kodeForm, userName)
        ProcessFile(tanggalData, kodeForm)
        Return True
    End Function

    Shared Sub ProcessFile(ByVal tanggalData As DateTime, ByVal kodeForms As String)
        Dim files = GetFiles(tanggalData, kodeForms)

        For Each file In files
            UpdateStatus(file.PK_GeneratedFileList_ID)
            FillTemporaryTable(file.TanggalData, file.FK_LHBU_FormInfo_ID)
            FillActualTable(file.PK_GeneratedFileList_ID, file.TanggalData, file.FK_LHBU_FormInfo_ID)
        Next
    End Sub
    Shared Sub FillTemporaryTable(ByVal tanggalData As DateTime, ByVal kodeForm As String)

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            objDb.Database.ExecuteSqlCommand("exec usp_GenerateFileContent {0},{1}", tanggalData.ToString("yyyy-MM-dd"), kodeForm)
        End Using


    End Sub
    Shared Sub FillActualTable(ByVal recordID As Long, ByVal tanggalData As DateTime, ByVal kodeForm As String)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            objDb.Database.ExecuteSqlCommand("exec usp_FillFileContent {0},{1},{2}", recordID, tanggalData.ToString("yyyy-MM-dd"), kodeForm)
        End Using
    End Sub


    Shared Sub UpdateStatus(ByVal PK_GeneratedFileList_ID As Long)
        Dim sb = New StringBuilder()
        sb.Append("UPDATE GeneratedFileList SET GenerationStatus = 'PROCESSING', UpdatedBy='System',UpdatedDate=GETDATE()")
        sb.Append("WHERE PK_GeneratedFileList_ID =" + PK_GeneratedFileList_ID.ToString())

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            objDb.Database.ExecuteSqlCommand(sb.ToString())
        End Using

    End Sub
    Shared Function GetFiles(ByVal tanggalData As DateTime, ByVal kodeForms As String) As List(Of GeneratedFileList)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim query As String = "SELECT * FROM GeneratedFileList WHERE TanggalData = '" + tanggalData.ToString("yyyy-MM-dd") + "' and FK_LHBU_FormInfo_ID in ('" + kodeForms + "') and isnull(SubmitStatus,'') != 'Submitted'"
            Return objDb.Database.SqlQuery(Of GeneratedFileList)(query).ToList
        End Using
    End Function

    Shared Sub InsertBase(tanggalData As DateTime, kodeForm As String, userName As String)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            objDb.Database.ExecuteSqlCommand("exec usp_GenerateFiles {0},{1},{2}", tanggalData.ToString("yyyy-MM-dd"), kodeForm, userName)
        End Using
    End Sub
    Shared Sub DeletePrevious(tanggalData As DateTime, kodeForm As String)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            objDb.Database.ExecuteSqlCommand("exec usp_CleanFiles {0},{1}", tanggalData.ToString("yyyy-MM-dd"), kodeForm)
        End Using
    End Sub

    Shared Function GetData() As List(Of NawaDevDAL.GeneratedFileList)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GeneratedFileLists.OrderByDescending(Function(X) X.StartDate).ToList
        End Using
    End Function

    Shared Function GetDataList() As List(Of NawaDevDAL.Vw_GeneratedFileList)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.Vw_GeneratedFileList.OrderByDescending(Function(X) X.StartDate)).ToList
        End Using
    End Function
    Shared Function GetDataIntegrationList() As List(Of NawaDevDAL.vw_TextFileTemporaryTable)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.vw_TextFileTemporaryTable.OrderByDescending(Function(X) X.Pk_ORS_TextFileTemporaryTable_ID)).ToList
        End Using
    End Function

    Shared Function GetDataToApproveList() As List(Of NawaDevDAL.Vw_GeneratedFileList)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.Vw_GeneratedFileList.Where(Function(x) x.ApprovalStatus = "WAITING APPROVAL").OrderByDescending(Function(X) X.StartDate)).ToList
        End Using
    End Function

    Shared Function GetDataSubmitToApproveList() As List(Of NawaDevDAL.Vw_GeneratedFileList)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return (From x In objDb.Vw_GeneratedFileList.Where(Function(x) x.SubmitStatus = "WAITING APPROVAL").OrderByDescending(Function(X) X.StartDate)).ToList
        End Using
    End Function





    'Shared Function GetSLIKParam() As NawaDevDAL.SLIKParameter
    '    Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
    '        Return objDb.SLIKParameters.FirstOrDefault
    '    End Using
    'End Function

    'Shared Function GetUserBranch(UserID As String) As NawaDevDAL.MappingBranchUser
    '    Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
    '        Return (From x In objDb.MappingBranchUsers Where x.UserID = UserID Select x).FirstOrDefault
    '    End Using
    'End Function

    Function SaveDocument(objFileList As Vw_GeneratedFileList,
                            objModule As NawaDAL.Module)
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim ObjGeneratedFileList As GeneratedFileList = objdb.GeneratedFileLists.Where(Function(x) x.PK_GeneratedFileList_ID = objFileList.PK_GeneratedFileList_ID).FirstOrDefault
                    If Not ObjGeneratedFileList Is Nothing Then
                        ObjGeneratedFileList.ApprovalStatus = objFileList.ApprovalStatus
                        ObjGeneratedFileList.SubmitStatus = objFileList.SubmitStatus
                        ObjGeneratedFileList.Comment = objFileList.Comment
                        ObjGeneratedFileList.SubmitNote = objFileList.SubmitNote
                        If ObjGeneratedFileList.SubmitStatus = "SUBMITTED" Then
                            objdb.Database.ExecuteSqlCommand("usp_UpdateLHBUForm {0}, {1}", ObjGeneratedFileList.FK_LHBU_FormInfo_ID, ObjGeneratedFileList.PK_GeneratedFileList_ID)
                        End If

                        objdb.Entry(ObjGeneratedFileList).State = Entity.EntityState.Modified
                    End If

                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Function SaveAllDocument(objFileList As List(Of Vw_GeneratedFileList),
                            objModule As NawaDAL.Module)

        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    For Each ObjVw_GeneratedFileList As Vw_GeneratedFileList In objFileList
                        Dim ObjGeneratedFileList As GeneratedFileList = objdb.GeneratedFileLists.Where(Function(x) x.PK_GeneratedFileList_ID = ObjVw_GeneratedFileList.PK_GeneratedFileList_ID).FirstOrDefault
                        If Not ObjGeneratedFileList Is Nothing Then
                            ObjGeneratedFileList.ApprovalStatus = ObjVw_GeneratedFileList.ApprovalStatus
                            ObjGeneratedFileList.SubmitStatus = ObjVw_GeneratedFileList.SubmitStatus
                            ObjGeneratedFileList.Comment = ObjVw_GeneratedFileList.Comment
                            ObjGeneratedFileList.SubmitNote = ObjVw_GeneratedFileList.SubmitNote
                            If ObjGeneratedFileList.SubmitStatus = "SUBMITTED" Then
                                objdb.Database.ExecuteSqlCommand("usp_UpdateLHBUForm {0}, {1}", ObjGeneratedFileList.FK_LHBU_FormInfo_ID, ObjGeneratedFileList.PK_GeneratedFileList_ID)
                            End If

                            objdb.Entry(ObjGeneratedFileList).State = Entity.EntityState.Modified
                        End If
                    Next
                    objdb.SaveChanges()

                    'For Each item In objFileList
                    '    objdb.Entry(item).State = Entity.EntityState.Modified
                    '    objdb.SaveChanges()
                    'Next
                    objtrans.Commit()
                Catch
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Shared Sub ActivationTanpaapproval(id As String, objSchemaModule As NawaDAL.Module)
        'done:ActivationTanpaapproval
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try


                    Dim objTaskdel As NawaDevDAL.GenerateFileTemplate = objdb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = id).FirstOrDefault


                    objTaskdel.Active = Not objTaskdel.Active
                    objdb.Entry(objTaskdel).State = Entity.EntityState.Modified



                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
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
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
    End Sub

    Shared Sub ActivationDenganapproval(unikkey As String, objSchemaModule As NawaDAL.Module)
        'done:ActivationDenganapproval

        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    Dim objGenerateFileTemplate As NawaDevDAL.GenerateFileTemplate = GetGenerateFileTemplateByID(unikkey)
                    Dim objGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = GetListGenerateFileTemplateAdditionalByPKID(unikkey)
                    Dim objGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail) = GetListGenerateFileTemplateDetailByPKID(unikkey)


                    objGenerateFileTemplate.Active = Not objGenerateFileTemplate.Active
                    Dim objGenerateFileTemplateDataBLL As New NawaDevBLL.FileGenerationDataBLL
                    objGenerateFileTemplateDataBLL.objGenerateFileTemplate = objGenerateFileTemplate
                    objGenerateFileTemplateDataBLL.objListGenerateFileTemplateAdditional = objGenerateFileTemplateAdditional
                    objGenerateFileTemplateDataBLL.objListGenerateFileTemplateDetail = objGenerateFileTemplateDetail


                    Dim xmldata As String = NawaBLL.Common.Serialize(objGenerateFileTemplateDataBLL)
                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = objGenerateFileTemplateDataBLL.objGenerateFileTemplate.PK_GenerationFileTemplate_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With

                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added



                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()



                    Dim objtype As Type = objGenerateFileTemplate.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""

                        If Not item.GetValue(objGenerateFileTemplate, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objGenerateFileTemplate, Nothing)

                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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


                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using

    End Sub

    Shared Sub LoadPanelActivation(objPanel As FormPanel, objmodulename As String, unikkey As String)
        'done: code anelActivation


        'Dim objGenerateFileTemplateDataBLL As NawaBLL.GenerateFileTemplateDataBLL = NawaBLL.Common.Deserialize(objdata, GetType(NawaBLL.GenerateFileTemplateDataBLL))
        Dim objGenerateFileTemplate As NawaDevDAL.GenerateFileTemplate = GetGenerateFileTemplateByID(unikkey)
        Dim objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = GetListGenerateFileTemplateAdditionalByPKID(unikkey)
        Dim objListGenerateFileTemplatedetail As List(Of NawaDevDAL.GenerateFileTemplateDetail) = GetListGenerateFileTemplateDetailByPKID(unikkey)

        If Not objGenerateFileTemplate Is Nothing Then
            Dim strunik As String = unikkey
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_GenerationFileTemplate_ID" & strunik, objGenerateFileTemplate.PK_GenerationFileTemplate_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Template Name", "GenerateFileTemplateName" & strunik, objGenerateFileTemplate.GenerateFileTemplateName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "File Name Format", "FileNameFormat" & strunik, objGenerateFileTemplate.FileNameFormat)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Output Format", "OutputFormat" & strunik, objGenerateFileTemplate.OutputFormat)

            Dim infoName As String = ""
            Using objDB As New NawaDatadevEntities
                Dim objInfo As ORS_FormInfo = objDB.ORS_FormInfo.ToList.FirstOrDefault(Function(x) x.Kode = objGenerateFileTemplate.LHBUFormName)
                If Not objInfo Is Nothing Then
                    infoName = objInfo.Kode & " - " & objInfo.Nama
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Information Name", "OJKSegmentData" & strunik, infoName)

            Dim delivMethod As String = ""
            Using objDB As New NawaDatadevEntities
                Dim paramDeliv(0) As SqlClient.SqlParameter
                paramDeliv(0) = New SqlClient.SqlParameter
                paramDeliv(0).ParameterName = "@ID"
                paramDeliv(0).SqlDbType = SqlDbType.Int
                paramDeliv(0).Value = IIf(objGenerateFileTemplate.FK_DelivMethod_ID Is Nothing, 0, objGenerateFileTemplate.FK_DelivMethod_ID)

                Dim dataDelivMethod As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT MethodType FROM ORS_Ref_DeliveryMethod WHERE PK_ID = @ID", paramDeliv)
                If dataDelivMethod.Rows.Count > 0 Then
                    delivMethod = dataDelivMethod.Rows(0).Item("MethodType")
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Delivery Method", "DelivMethod" & strunik, delivMethod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objGenerateFileTemplate.Active.ToString & " -->" & (Not objGenerateFileTemplate.Active).ToString)

            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_GenerateFileTemplateAdditional_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FK_GenerationFileTemplate_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SQLQuery"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "Alias"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "Sequence"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "FieldTahun"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FieldBulan"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FieldKodeCabang"
            objField.Type = ModelFieldType.String
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
            objColum.Text = "Alias"
            objColum.DataIndex = "Alias"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)



            objColum = New Ext.Net.Column
            objColum.Text = "Sequence"
            objColum.DataIndex = "Sequence"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)


            objColum = New Ext.Net.Column
            objColum.Text = "SQLQuery"
            objColum.DataIndex = "SQLQuery"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Tahun"
            objColum.DataIndex = "FieldTahun"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Bulan"
            objColum.DataIndex = "FieldBulan"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Kode Cabang"
            objColum.DataIndex = "FieldKodeCabang"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objdt As Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objListGenerateFileTemplateAdditional)
            Dim objcol As New Data.DataColumn


            objcol.ColumnName = "EmailTableType"
            objcol.DataType = GetType(String)
            objdt.Columns.Add(objcol)


            'For Each item As DataRow In objdt.Rows
            '    Dim objtask As NawaDevDAL.EmailTableType = GetEmailTableTypeByID(item("FK_EmailTableType_ID"))
            '    If Not objtask Is Nothing Then
            '        item("EmailTableType") = objtask.EmailTableTypeName
            '    End If

            'Next


            Dim objStoreReplacer As New Ext.Net.Store
            objStoreReplacer.ID = strunik & "StoreGridReplacer"
            objStoreReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModelReplacer As New Ext.Net.Model
            Dim objFieldReplacer As Ext.Net.ModelField




            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "PK_GenerateFileTemplateDetail_ID"
            objFieldReplacer.Type = ModelFieldType.Auto
            objModelReplacer.Fields.Add(objFieldReplacer)


            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "Replacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)

            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "FieldReplacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)


            objStoreReplacer.Model.Add(objModelReplacer)


            Dim objListcolumnReplacer As New List(Of ColumnBase)
            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumnReplacer.Add(objcolumnNo)
            End Using


            Dim objColumReplacer As Ext.Net.Column


            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "Replacer"
            objColumReplacer.DataIndex = "Replacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)

            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "FieldReplacer"
            objColumReplacer.DataIndex = "FieldReplacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)



            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Table Reference", objStore, objListcolumn, objdt)

            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Replacer", objStoreReplacer, objListcolumnReplacer, objListGenerateFileTemplatedetail)

        End If


    End Sub
    Shared Function Accept(ID As String) As Boolean
        'done: accept


        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert

                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))
                            Dim objGenerateFileTemplate As GenerateFileTemplate = objModuledata.objGenerateFileTemplate
                            Dim objGenerateFileTemplateAdditional As List(Of GenerateFileTemplateAdditional) = objModuledata.objListGenerateFileTemplateAdditional
                            Dim objGenerateFileTemplateDetail As List(Of GenerateFileTemplateDetail) = objModuledata.objListGenerateFileTemplateDetail

                            objGenerateFileTemplate.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objGenerateFileTemplate.ApprovedDate = Now

                            objdb.Entry(objGenerateFileTemplate).State = Entity.EntityState.Added




                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            'For Each item As GenerateFileTemplateAdditional In objGenerateFileTemplateAdditional
                            '    item.FK_GenerationFileTemplate_ID = objGenerateFileTemplate.PK_GenerationFileTemplate_ID
                            '    objdb.Entry(item).State = Entity.EntityState.Added
                            'Next


                            'For Each item As GenerateFileTemplateDetail In objGenerateFileTemplateDetail
                            '    item.FK_GenerationFileTemplate_ID = objGenerateFileTemplate.PK_GenerationFileTemplate_ID
                            '    objdb.Entry(item).State = Entity.EntityState.Added
                            'Next





                            Dim objtype As Type = objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next


                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))
                            Dim objModuledataOld As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(FileGenerationDataBLL))

                            Dim objGenerateFileTemplate As GenerateFileTemplate = objModuledata.objGenerateFileTemplate

                            objGenerateFileTemplate.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objGenerateFileTemplate.ApprovedDate = Now

                            objdb.GenerateFileTemplates.Attach(objGenerateFileTemplate)
                            objdb.Entry(objGenerateFileTemplate).State = Entity.EntityState.Modified


                            For Each itemx As GenerateFileTemplateAdditional In (From x In objdb.GenerateFileTemplateAdditionals Where x.FK_GenerationFileTemplate_ID = objModuledata.objGenerateFileTemplate.PK_GenerationFileTemplate_ID Select x).ToList
                                Dim objcek As GenerateFileTemplateAdditional = objModuledata.objListGenerateFileTemplateAdditional.Find(Function(x) x.PK_GenerateFileTemplateAdditional_ID = itemx.PK_GenerateFileTemplateAdditional_ID)
                                If objcek Is Nothing Then
                                    objdb.Entry(itemx).State = Entity.EntityState.Deleted
                                End If
                            Next
                            For Each item As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                Dim obcek As GenerateFileTemplateAdditional = (From x In objdb.GenerateFileTemplateAdditionals Where x.PK_GenerateFileTemplateAdditional_ID = item.PK_GenerateFileTemplateAdditional_ID Select x).FirstOrDefault
                                If obcek Is Nothing Then
                                    objdb.Entry(item).State = Entity.EntityState.Added
                                Else
                                    objdb.Entry(obcek).CurrentValues.SetValues(item)
                                    objdb.Entry(obcek).State = Entity.EntityState.Modified
                                End If
                            Next



                            For Each itemx As GenerateFileTemplateDetail In (From x In objdb.GenerateFileTemplateDetails Where x.FK_GenerationFileTemplate_ID = objModuledata.objGenerateFileTemplate.PK_GenerationFileTemplate_ID Select x).ToList
                                Dim objcek As GenerateFileTemplateDetail = objModuledata.objListGenerateFileTemplateDetail.Find(Function(x) x.PK_GenerateFileTemplateDetail_ID = itemx.PK_GenerateFileTemplateDetail_ID)
                                If objcek Is Nothing Then
                                    objdb.Entry(itemx).State = Entity.EntityState.Deleted
                                End If
                            Next
                            For Each item As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                Dim obcek As GenerateFileTemplateDetail = (From x In objdb.GenerateFileTemplateDetails Where x.PK_GenerateFileTemplateDetail_ID = item.PK_GenerateFileTemplateDetail_ID Select x).FirstOrDefault
                                If obcek Is Nothing Then
                                    objdb.Entry(item).State = Entity.EntityState.Added
                                Else
                                    objdb.Entry(obcek).CurrentValues.SetValues(item)
                                    objdb.Entry(obcek).State = Entity.EntityState.Modified
                                End If
                            Next


                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()




                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledataOld.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objModuledataOld.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Next

                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))

                            objdb.Entry(objModuledata.objGenerateFileTemplate).State = Entity.EntityState.Deleted

                            For Each item As GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objdb.Entry(item).State = Entity.EntityState.Deleted
                            Next

                            For Each item As GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objdb.Entry(item).State = Entity.EntityState.Deleted
                            Next


                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next
                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))

                            objdb.Entry(objModuledata.objGenerateFileTemplate).State = Entity.EntityState.Modified




                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next

                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
        'done:reject
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID
                        Case NawaBLL.Common.ModuleActionEnum.Insert

                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))



                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()




                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next




                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))
                            Dim objModuledataOld As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(FileGenerationDataBLL))


                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledataOld.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objModuledataOld.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Next




                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))



                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next


                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                            Dim objModuledata As FileGenerationDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(FileGenerationDataBLL))






                            'audittrail
                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()

                            Dim objtype As Type = objModuledata.objGenerateFileTemplate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objModuledata.objGenerateFileTemplate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objModuledata.objGenerateFileTemplate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next


                            For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objModuledata.objListGenerateFileTemplateAdditional
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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



                            For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objModuledata.objListGenerateFileTemplateDetail
                                objtype = itemheader.GetType
                                properties = objtype.GetProperties
                                For Each item As System.Reflection.PropertyInfo In properties
                                    Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
    Shared Sub SettingColor(objPanelOld As FormPanel, objPanelNew As FormPanel, objdata As String, objdatabefore As String, unikkeyold As String, unikkeynew As String)

        If objdata.Length > 0 And objdatabefore.Length > 0 Then
            Nawa.BLL.NawaFramework.SetColor("PK_GenerationFileTemplate_ID", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("GenerateFileTemplateName", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("EmailTo", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("EmailCC", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("EmailBCC", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("EmailSubject", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("EmailBody", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("FK_Monitoringduration_ID", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("StartDate", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("StartTime", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("ExcludeHoliday", objPanelOld, objPanelNew, unikkeyold, unikkeynew)
            Nawa.BLL.NawaFramework.SetColor("Active", objPanelOld, objPanelNew, unikkeyold, unikkeynew)

        End If
    End Sub

    Shared Sub LoadPanelDelete(objPanel As FormPanel, strModulename As String, unikkey As String)

        'done: loadPanel



        'Dim objGenerateFileTemplateDataBLL As NawaBLL.GenerateFileTemplateDataBLL = NawaBLL.Common.Deserialize(objdata, GetType(NawaBLL.GenerateFileTemplateDataBLL))
        Dim objGenerateFileTemplate As NawaDevDAL.GenerateFileTemplate = GetGenerateFileTemplateByID(unikkey)
        Dim objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = GetListGenerateFileTemplateAdditionalByPKID(unikkey)
        Dim objListGenerateFileTemplatedetail As List(Of NawaDevDAL.GenerateFileTemplateDetail) = GetListGenerateFileTemplateDetailByPKID(unikkey)

        If Not objGenerateFileTemplate Is Nothing Then
            Dim strunik As String = unikkey
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_GenerationFileTemplate_ID" & strunik, objGenerateFileTemplate.PK_GenerationFileTemplate_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Template Name", "GenerateFileTemplateName" & strunik, objGenerateFileTemplate.GenerateFileTemplateName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "File Name Format", "FileNameFormat" & strunik, objGenerateFileTemplate.FileNameFormat)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Output Format", "OutputFormat" & strunik, objGenerateFileTemplate.OutputFormat)

            Dim infoName As String = ""
            Using objDB As New NawaDatadevEntities
                Dim objInfo As ORS_FormInfo = objDB.ORS_FormInfo.ToList.FirstOrDefault(Function(x) x.Kode = objGenerateFileTemplate.LHBUFormName)
                If Not objInfo Is Nothing Then
                    infoName = objInfo.Kode & " - " & objInfo.Nama
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Information Name", "OJKSegmentData" & strunik, infoName)

            Dim delivMethod As String = ""
            Using objDB As New NawaDatadevEntities
                Dim paramDeliv(0) As SqlClient.SqlParameter
                paramDeliv(0) = New SqlClient.SqlParameter
                paramDeliv(0).ParameterName = "@ID"
                paramDeliv(0).SqlDbType = SqlDbType.Int
                paramDeliv(0).Value = IIf(objGenerateFileTemplate.FK_DelivMethod_ID Is Nothing, 0, objGenerateFileTemplate.FK_DelivMethod_ID)

                Dim dataDelivMethod As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT MethodType FROM ORS_Ref_DeliveryMethod WHERE PK_ID = @ID", paramDeliv)
                If dataDelivMethod.Rows.Count > 0 Then
                    delivMethod = dataDelivMethod.Rows(0).Item("MethodType")
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Delivery Method", "DelivMethod" & strunik, delivMethod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objGenerateFileTemplate.Active)

            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_GenerateFileTemplateAdditional_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FK_GenerationFileTemplate_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SQLQuery"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "Alias"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "Sequence"
            objField.Type = ModelFieldType.String
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
            objColum.Text = "SQL Query"
            objColum.DataIndex = "SQLQuery"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)



            objColum = New Ext.Net.Column
            objColum.Text = "Alias"
            objColum.DataIndex = "Alias"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)


            objColum = New Ext.Net.Column
            objColum.Text = "Sequence"
            objColum.DataIndex = "Sequence"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objStoreReplacer As New Ext.Net.Store
            objStoreReplacer.ID = strunik & "StoreGridReplacer"
            objStoreReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModelReplacer As New Ext.Net.Model
            Dim objFieldReplacer As Ext.Net.ModelField

            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "PK_GenerateFileTemplateDetail_ID"
            objFieldReplacer.Type = ModelFieldType.Auto
            objModelReplacer.Fields.Add(objFieldReplacer)


            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "Replacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)

            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "FieldReplacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)


            objStoreReplacer.Model.Add(objModelReplacer)



            Dim objListcolumnReplacer As New List(Of ColumnBase)
            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumnReplacer.Add(objcolumnNo)
            End Using


            Dim objColumReplacer As Ext.Net.Column


            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "Replacer"
            objColumReplacer.DataIndex = "Replacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)

            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "Field Replacer"
            objColumReplacer.DataIndex = "FieldReplacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)

            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Table Reference", objStore, objListcolumn, objListGenerateFileTemplateAdditional)
            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Replacer", objStoreReplacer, objListcolumnReplacer, objListGenerateFileTemplatedetail)
        End If
    End Sub

    Shared Function IsDataValidDelete(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Dim objdel As NawaDevDAL.GenerateFileTemplate = objdb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = ID).FirstOrDefault
            If Not objdel Is Nothing Then
                Dim objapprovaldel As NawaDevDAL.ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.ModuleName = objSchemaModule.ModuleName And x.ModuleKey = ID).FirstOrDefault()
                If Not objapprovaldel Is Nothing Then
                    Throw New Exception(objSchemaModule.ModuleLabel & " " & objdel.GenerateFileTemplateName & " already exist in pending approval.")
                End If
            End If
        End Using
        Return True
    End Function


    Shared Function DeleteDenganapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        'done:code  DeleteDenganapproval
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objGenerateFileTemplateDel As NawaDevDAL.GenerateFileTemplate = objdb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = ID).FirstOrDefault
                    Dim objGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail) = objdb.GenerateFileTemplateDetails.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList
                    Dim objGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = objdb.GenerateFileTemplateAdditionals.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList


                    Dim objTaskDataBLL As New NawaDevBLL.FileGenerationDataBLL
                    objTaskDataBLL.objGenerateFileTemplate = objGenerateFileTemplateDel
                    objTaskDataBLL.objListGenerateFileTemplateDetail = objGenerateFileTemplateDetail
                    objTaskDataBLL.objListGenerateFileTemplateAdditional = objGenerateFileTemplateAdditional

                    Dim xmldata As String = NawaBLL.Common.Serialize(objTaskDataBLL)
                    Dim objModuleApproval As New ModuleApproval
                    With objModuleApproval
                        .ModuleName = objSchemaModule.ModuleName
                        .ModuleKey = objTaskDataBLL.objGenerateFileTemplate.PK_GenerationFileTemplate_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = ""
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()
                    Dim objtype As Type = objGenerateFileTemplateDel.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objGenerateFileTemplateDel, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objGenerateFileTemplateDel, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next
                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
    Shared Function DeleteTanpaapproval(ID As String, objSchemaModule As NawaDAL.Module) As Boolean
        'done:code DeleteTanpaapproval

        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objTaskdel As NawaDevDAL.GenerateFileTemplate = objdb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = ID).FirstOrDefault
                    Dim objTaskDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail) = objdb.GenerateFileTemplateDetails.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList
                    Dim objTaskAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = objdb.GenerateFileTemplateAdditionals.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList

                    objdb.Entry(objTaskdel).State = Entity.EntityState.Deleted
                    For Each item As GenerateFileTemplateDetail In objTaskDetail
                        objdb.Entry(item).State = Entity.EntityState.Deleted
                    Next
                    For Each item As GenerateFileTemplateAdditional In objTaskAdditional
                        objdb.Entry(item).State = Entity.EntityState.Deleted
                    Next
                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
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
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objTaskDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objTaskAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
    Shared Sub LoadPanel(objPanel As FormPanel, objdata As String, strModulename As String, unikkey As String)

        'done: loadPanel



        Dim objGenerateFileTemplateDataBLL As NawaDevBLL.FileGenerationDataBLL = NawaBLL.Common.Deserialize(objdata, GetType(NawaDevBLL.FileGenerationDataBLL))
        Dim objGenerateFileTemplate As NawaDevDAL.GenerateFileTemplate = objGenerateFileTemplateDataBLL.objGenerateFileTemplate

        If Not objGenerateFileTemplate Is Nothing Then
            Dim strunik As String = unikkey
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_GenerationFileTemplate_ID" & strunik, objGenerateFileTemplate.PK_GenerationFileTemplate_ID)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Template Name", "GenerateFileTemplateName" & strunik, objGenerateFileTemplate.GenerateFileTemplateName)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "File Name Format", "FileNameFormat" & strunik, objGenerateFileTemplate.FileNameFormat)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Output Format", "OutputFormat" & strunik, objGenerateFileTemplate.OutputFormat)

            Dim infoName As String = ""
            Using objDB As New NawaDatadevEntities
                Dim objInfo As ORS_FormInfo = objDB.ORS_FormInfo.ToList.FirstOrDefault(Function(x) x.Kode = objGenerateFileTemplate.LHBUFormName)
                If Not objInfo Is Nothing Then
                    infoName = objInfo.Kode & " - " & objInfo.Nama
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Information Name", "OJKSegmentData" & strunik, infoName)

            Dim delivMethod As String = ""
            Using objDB As New NawaDatadevEntities
                Dim paramDeliv(0) As SqlClient.SqlParameter
                paramDeliv(0) = New SqlClient.SqlParameter
                paramDeliv(0).ParameterName = "@ID"
                paramDeliv(0).SqlDbType = SqlDbType.Int
                paramDeliv(0).Value = IIf(objGenerateFileTemplate.FK_DelivMethod_ID Is Nothing, 0, objGenerateFileTemplate.FK_DelivMethod_ID)

                Dim dataDelivMethod As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT MethodType FROM ORS_Ref_DeliveryMethod WHERE PK_ID = @ID", paramDeliv)
                If dataDelivMethod.Rows.Count > 0 Then
                    delivMethod = dataDelivMethod.Rows(0).Item("MethodType")
                End If
            End Using
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Delivery Method", "DelivMethod" & strunik, delivMethod)
            Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" & strunik, objGenerateFileTemplate.Active)

            Dim objStore As New Ext.Net.Store
            objStore.ID = strunik & "StoreGrid"
            objStore.ClientIDMode = Web.UI.ClientIDMode.Static

            Dim objModel As New Ext.Net.Model
            Dim objField As Ext.Net.ModelField

            objField = New Ext.Net.ModelField
            objField.Name = "PK_GenerateFileTemplateAdditional_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FK_GenerationFileTemplate_ID"
            objField.Type = ModelFieldType.Auto
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "SQLQuery"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "Alias"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "Sequence"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)


            objField = New Ext.Net.ModelField
            objField.Name = "FieldTahun"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FieldBulan"
            objField.Type = ModelFieldType.String
            objModel.Fields.Add(objField)

            objField = New Ext.Net.ModelField
            objField.Name = "FieldKodeCabang"
            objField.Type = ModelFieldType.String
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
            objColum.Text = "Alias"
            objColum.DataIndex = "Alias"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)



            objColum = New Ext.Net.Column
            objColum.Text = "Sequence"
            objColum.DataIndex = "Sequence"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)


            objColum = New Ext.Net.Column
            objColum.Text = "SQLQuery"
            objColum.DataIndex = "SQLQuery"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Tahun"
            objColum.DataIndex = "FieldTahun"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Bulan"
            objColum.DataIndex = "FieldBulan"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            objColum = New Ext.Net.Column
            objColum.Text = "Field Kode Cabang"
            objColum.DataIndex = "FieldKodeCabang"
            objColum.ClientIDMode = Web.UI.ClientIDMode.Static
            objColum.Flex = 1
            objListcolumn.Add(objColum)

            Dim objdt As Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(objGenerateFileTemplateDataBLL.objListGenerateFileTemplateAdditional)
            Dim objcol As New Data.DataColumn


            objcol.ColumnName = "EmailTableType"
            objcol.DataType = GetType(String)
            objdt.Columns.Add(objcol)


            'For Each item As DataRow In objdt.Rows
            '    Dim objtask As NawaDevDAL.EmailTableType = GetEmailTableTypeByID(item("FK_EmailTableType_ID"))
            '    If Not objtask Is Nothing Then
            '        item("EmailTableType") = objtask.EmailTableTypeName
            '    End If

            'Next


            Dim objStoreReplacer As New Ext.Net.Store
            objStoreReplacer.ID = strunik & "StoreGridReplacer"
            objStoreReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            Dim objModelReplacer As New Ext.Net.Model
            Dim objFieldReplacer As Ext.Net.ModelField




            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "PK_GenerateFileTemplateDetail_ID"
            objFieldReplacer.Type = ModelFieldType.Auto
            objModelReplacer.Fields.Add(objFieldReplacer)


            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "Replacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)

            objFieldReplacer = New Ext.Net.ModelField
            objFieldReplacer.Name = "FieldReplacer"
            objFieldReplacer.Type = ModelFieldType.String
            objModelReplacer.Fields.Add(objFieldReplacer)


            objStoreReplacer.Model.Add(objModelReplacer)


            Dim objListcolumnReplacer As New List(Of ColumnBase)
            Using objcolumnNo As New Ext.Net.RowNumbererColumn
                objcolumnNo.Text = "No."
                objcolumnNo.ClientIDMode = Web.UI.ClientIDMode.Static
                objListcolumnReplacer.Add(objcolumnNo)
            End Using


            Dim objColumReplacer As Ext.Net.Column


            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "Replacer"
            objColumReplacer.DataIndex = "Replacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)

            objColumReplacer = New Ext.Net.Column
            objColumReplacer.Text = "FieldReplacer"
            objColumReplacer.DataIndex = "FieldReplacer"
            objColumReplacer.ClientIDMode = Web.UI.ClientIDMode.Static
            objColumReplacer.Flex = 1
            objListcolumnReplacer.Add(objColumReplacer)



            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Table Reference", objStore, objListcolumn, objdt)

            Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Replacer", objStoreReplacer, objListcolumnReplacer, objGenerateFileTemplateDataBLL.objListGenerateFileTemplateDetail)

        End If


    End Sub
    Shared Function GetMonitoringdurationByID(ID As Integer) As NawaDevDAL.MonitoringDuration
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.MonitoringDurations.Where(Function(x) x.PK_MonitoringDuration_Id = ID).FirstOrDefault
        End Using

    End Function
    Shared Function GetListGenerateFileTemplateDetailByPKID(ID As Integer) As List(Of NawaDevDAL.GenerateFileTemplateDetail)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GenerateFileTemplateDetails.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList
        End Using
    End Function


    Shared Function GetListGenerateFileTemplateAdditionalByPKID(ID As Integer) As List(Of NawaDevDAL.GenerateFileTemplateAdditional)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GenerateFileTemplateAdditionals.Where(Function(x) x.FK_GenerationFileTemplate_ID = ID).ToList
        End Using
    End Function
    Shared Function GetGenerateFileTemplateByID(ID As Integer) As NawaDevDAL.GenerateFileTemplate

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = ID).FirstOrDefault
        End Using
    End Function

    Function SaveEditTanpaApproval(objData As NawaDevDAL.GenerateFileTemplate, objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional), objListGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail), objSchemaModule As NawaDAL.Module) As Boolean
        'done: SaveEditTanpaApproval
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objData.ApprovedDate = DateTime.Now
                    objdb.Entry(objData).State = Entity.EntityState.Modified




                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()





                    For Each itemx As GenerateFileTemplateAdditional In (From x In objdb.GenerateFileTemplateAdditionals Where x.FK_GenerationFileTemplate_ID = objData.PK_GenerationFileTemplate_ID Select x).ToList
                        Dim objcek As GenerateFileTemplateAdditional = objListGenerateFileTemplateAdditional.Find(Function(x) x.PK_GenerateFileTemplateAdditional_ID = itemx.PK_GenerateFileTemplateAdditional_ID)
                        If objcek Is Nothing Then
                            objdb.Entry(itemx).State = Entity.EntityState.Deleted
                        End If
                    Next
                    For Each item As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        Dim obcek As GenerateFileTemplateAdditional = (From x In objdb.GenerateFileTemplateAdditionals Where x.PK_GenerateFileTemplateAdditional_ID = item.PK_GenerateFileTemplateAdditional_ID Select x).FirstOrDefault
                        If obcek Is Nothing Then
                            objdb.Entry(item).State = Entity.EntityState.Added
                        Else
                            objdb.Entry(obcek).CurrentValues.SetValues(item)
                            objdb.Entry(obcek).State = Entity.EntityState.Modified
                        End If
                    Next



                    For Each itemx As GenerateFileTemplateDetail In (From x In objdb.GenerateFileTemplateDetails Where x.FK_GenerationFileTemplate_ID = objData.PK_GenerationFileTemplate_ID Select x).ToList
                        Dim objcek As GenerateFileTemplateDetail = objListGenerateFileTemplateDetail.Find(Function(x) x.PK_GenerateFileTemplateDetail_ID = itemx.PK_GenerateFileTemplateDetail_ID)
                        If objcek Is Nothing Then
                            objdb.Entry(itemx).State = Entity.EntityState.Deleted
                        End If
                    Next
                    For Each item As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        Dim obcek As GenerateFileTemplateDetail = (From x In objdb.GenerateFileTemplateDetails Where x.PK_GenerateFileTemplateDetail_ID = item.PK_GenerateFileTemplateDetail_ID Select x).FirstOrDefault
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
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
    Function SaveAddTanpaApproval(objData As NawaDevDAL.GenerateFileTemplate, objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional), objListGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail), objSchemaModule As NawaDAL.Module) As Boolean
        'done: code SaveAddTanpaApproval

        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objData.ApprovedDate = DateTime.Now
                    objdb.Entry(objData).State = Entity.EntityState.Added




                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                    objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase
                    objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert
                    objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    For Each item As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        item.FK_GenerationFileTemplate_ID = objData.PK_GenerationFileTemplate_ID
                        objdb.Entry(item).State = Entity.EntityState.Added


                    Next

                    For Each item As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        item.FK_GenerationFileTemplate_ID = objData.PK_GenerationFileTemplate_ID
                        objdb.Entry(item).State = Entity.EntityState.Added
                    Next




                    Dim objtype As Type = objData.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
    Shared Function IsApprovalExists(moduleKey As String) As Boolean

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.ModuleApprovals.Where(Function(x) x.ModuleKey = moduleKey And x.ModuleName = "GenerateFileTemplate").Count > 0
        End Using

    End Function
    Shared Function IsGenerateFileTemplateNameAlreadyExist(strGenerateFileTemplatename As String) As Boolean

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GenerateFileTemplates.Where(Function(x) x.GenerateFileTemplateName = strGenerateFileTemplatename).Count > 0
        End Using

    End Function

    Shared Function IsGenerateFileTemplateNameAlreadyExistEdit(strGenerateFileTemplatename As String, PK As Integer) As Boolean

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.GenerateFileTemplates.Where(Function(x) x.GenerateFileTemplateName = strGenerateFileTemplatename And x.PK_GenerationFileTemplate_ID <> PK).Count > 0
        End Using

    End Function

    Function SaveEditApproval(objData As NawaDevDAL.GenerateFileTemplate, objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional), objListGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail), objModule As NawaDAL.Module) As Boolean
        'done: SaveEditApprovals
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objxData As New NawaDevBLL.FileGenerationDataBLL
                    objxData.objGenerateFileTemplate = objData
                    objxData.objListGenerateFileTemplateAdditional = objListGenerateFileTemplateAdditional
                    objxData.objListGenerateFileTemplateDetail = objListGenerateFileTemplateDetail

                    Dim xmldata As String = NawaBLL.Common.Serialize(objxData)


                    Dim objGenerateFileTemplateBefore As NawaDevDAL.GenerateFileTemplate = objdb.GenerateFileTemplates.Where(Function(x) x.PK_GenerationFileTemplate_ID = objxData.objGenerateFileTemplate.PK_GenerationFileTemplate_ID).FirstOrDefault
                    Dim objListGenerateFileTemplateAdditionalBefore As List(Of NawaDevDAL.GenerateFileTemplateAdditional) = objdb.GenerateFileTemplateAdditionals.Where(Function(x) x.FK_GenerationFileTemplate_ID = objxData.objGenerateFileTemplate.PK_GenerationFileTemplate_ID).ToList
                    Dim objListGenerateFileTemplateDetailBefore As List(Of NawaDevDAL.GenerateFileTemplateDetail) = objdb.GenerateFileTemplateDetails.Where(Function(x) x.FK_GenerationFileTemplate_ID = objxData.objGenerateFileTemplate.PK_GenerationFileTemplate_ID).ToList

                    Dim objxDatabefore As New NawaDevBLL.FileGenerationDataBLL
                    objxDatabefore.objGenerateFileTemplate = objGenerateFileTemplateBefore
                    objxDatabefore.objListGenerateFileTemplateAdditional = objListGenerateFileTemplateAdditionalBefore
                    objxDatabefore.objListGenerateFileTemplateDetail = objListGenerateFileTemplateDetailBefore
                    Dim xmlbefore As String = NawaBLL.Common.Serialize(objxDatabefore)

                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objModule.ModuleName
                        .ModuleKey = objxData.objGenerateFileTemplate.PK_GenerationFileTemplate_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = xmlbefore
                        .PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                        .CreatedDate = Now
                        .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                    End With
                    objdb.Entry(objModuleApproval).State = Entity.EntityState.Added
                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
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
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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


                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function


    Function SaveAddApproval(objData As NawaDevDAL.GenerateFileTemplate, objListGenerateFileTemplateAdditional As List(Of NawaDevDAL.GenerateFileTemplateAdditional), objListGenerateFileTemplateDetail As List(Of NawaDevDAL.GenerateFileTemplateDetail), objModule As NawaDAL.Module) As Boolean
        'done:code  SaveAddApproval
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objxData As New NawaDevBLL.FileGenerationDataBLL
                    objxData.objGenerateFileTemplate = objData
                    objxData.objListGenerateFileTemplateAdditional = objListGenerateFileTemplateAdditional
                    objxData.objListGenerateFileTemplateDetail = objListGenerateFileTemplateDetail

                    Dim xmldata As String = NawaBLL.Common.Serialize(objxData)

                    Dim objModuleApproval As New NawaDevDAL.ModuleApproval
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
                    Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
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
                        Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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

                    For Each itemheader As NawaDevDAL.GenerateFileTemplateAdditional In objListGenerateFileTemplateAdditional
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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


                    For Each itemheader As NawaDevDAL.GenerateFileTemplateDetail In objListGenerateFileTemplateDetail
                        objtype = itemheader.GetType
                        properties = objtype.GetProperties
                        For Each item As System.Reflection.PropertyInfo In properties
                            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
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
                    Nawa.BLL.NawaFramework.SendEmailModuleApproval(objModuleApproval.PK_ModuleApproval_ID)

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function
End Class
