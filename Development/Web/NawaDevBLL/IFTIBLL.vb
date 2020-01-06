Imports System.Data.SqlClient
Imports System.Text

Public Class IFTIBLL
    Implements IDisposable


    Shared Function Createuser(userid As String, strgroupmenu As String)

        Dim objListParam(1) As SqlParameter

        objListParam(0) = New SqlParameter
        objListParam(0).ParameterName = "@userid"
        objListParam(0).Value = userid

        objListParam(1) = New SqlParameter
        objListParam(1).ParameterName = "@groupmenuname"
        objListParam(1).Value = strgroupmenu




        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_createuserAndGroup", objListParam)
    End Function
    Shared Function ifticopy(pkiftiid As Integer, userid As String)

        Dim objListParam(1) As SqlParameter
        objListParam(0) = New SqlParameter
        objListParam(0).ParameterName = "@Pkiftiid"
        objListParam(0).Value = pkiftiid

        objListParam(1) = New SqlParameter
        objListParam(1).ParameterName = "@userid"
        objListParam(1).Value = userid


        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_IFTI_Copy", objListParam)
    End Function
    Shared Function getListErrorInitial(pkiftiid As Integer)

        Dim objListParam(0) As SqlParameter
        objListParam(0) = New SqlParameter
        objListParam(0).ParameterName = "@Pkiftiid"
        objListParam(0).Value = pkiftiid

        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetListInvalidValidationInitial", objListParam)
    End Function

    Shared Function getListError(pkiftiid As Integer)

        Dim objListParam(0) As SqlParameter
        objListParam(0) = New SqlParameter
        objListParam(0).ParameterName = "@Pkiftiid"
        objListParam(0).Value = pkiftiid

        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetListInvalidValidationEdit", objListParam)
    End Function

    Shared Function Reject(ID As String) As Boolean
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As NawaDevDAL.ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then
                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID

                        Case NawaBLL.Common.ModuleActionEnum.Update
                            Dim objIfti As IFTIDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(IFTIDataBLL))
                            Dim objIftiOld As IFTIDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(IFTIDataBLL))
                            objIfti.objIFTI.LastUpdateDate = Now

                            Dim objiftiupdate As NawaDevDAL.IFTI = objIfti.objIFTI
                            Dim objiftiolddata As NawaDevDAL.IFTI = objIftiOld.objIFTI


                            Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
                            objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                            objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected
                            objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update
                            objaudittrailheader.ModuleLabel = objModule.ModuleLabel
                            objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                            objdb.SaveChanges()
                            Dim objtype As Type = objiftiupdate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name

                                If Not item.GetValue(objiftiolddata, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objiftiolddata, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                If Not item.GetValue(objiftiupdate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objiftiupdate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next


                            For Each itemheader As NawaDevDAL.IFTI_Beneficiary In objIfti.objIFTIBeneficiary
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
    Shared Function Accept(ID As String) As Boolean
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    Dim objApproval As NawaDevDAL.ModuleApproval = objdb.ModuleApprovals.Where(Function(x) x.PK_ModuleApproval_ID = ID).FirstOrDefault()
                    Dim objModule As NawaDevDAL.Module
                    If Not objApproval Is Nothing Then


                        objModule = objdb.Modules.Where(Function(x) x.ModuleName = objApproval.ModuleName).FirstOrDefault
                    End If
                    Select Case objApproval.PK_ModuleAction_ID


                        Case NawaBLL.Common.ModuleActionEnum.Update

                            Dim objIfti As IFTIDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleField, GetType(IFTIDataBLL))
                            Dim objIftiOld As IFTIDataBLL = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, GetType(IFTIDataBLL))


                            objIfti.objIFTI.LastUpdateDate = Now

                            Dim objiftiupdate As NawaDevDAL.IFTI = objIfti.objIFTI
                            Dim objiftiolddata As NawaDevDAL.IFTI = objIftiOld.objIFTI



                            objdb.IFTIs.Attach(objiftiupdate)
                            objdb.Entry(objiftiupdate).State = Entity.EntityState.Modified

                            For Each item As NawaDevDAL.IFTI_Beneficiary In objIfti.objIFTIBeneficiary
                                Dim obcek As NawaDevDAL.IFTI_Beneficiary = (From x In objdb.IFTI_Beneficiary Where x.PK_IFTI_Beneficiary_ID = item.PK_IFTI_Beneficiary_ID Select x).FirstOrDefault
                                If obcek Is Nothing Then
                                    objdb.Entry(item).State = Entity.EntityState.Added
                                Else
                                    objdb.Entry(obcek).CurrentValues.SetValues(item)
                                    objdb.Entry(obcek).State = Entity.EntityState.Modified
                                End If
                            Next

                            For Each item As NawaDevDAL.IFTI_ErrorDescription In objdb.IFTI_ErrorDescription.Where(Function(x) x.FK_IFTI_ID = objiftiupdate.PK_IFTI_ID)
                                objdb.Entry(item).State = Entity.EntityState.Deleted
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

                            Dim objtype As Type = objiftiupdate.GetType
                            Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                            For Each item As System.Reflection.PropertyInfo In properties
                                Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(objiftiolddata, Nothing) Is Nothing Then
                                    objaudittraildetail.OldValue = item.GetValue(objiftiolddata, Nothing)
                                Else
                                    objaudittraildetail.OldValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                                If Not item.GetValue(objiftiupdate, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(objiftiupdate, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                            Next
                            For Each itemheader As NawaDevDAL.IFTI_Beneficiary In objIfti.objIFTIBeneficiary
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

                    End Select
                    objdb.Entry(objApproval).State = Entity.EntityState.Deleted
                    objdb.SaveChanges()
                    objtrans.Commit()


                    objdb.Database.ExecuteSqlCommand("usp_GenerateListOfGeneratedIfti")

                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Shared Function NotExistinApproval(objifti As IFTIDataBLL, objmodule As NawaDAL.Module) As Boolean
        Using objdb As New NawaDAL.NawaDataEntities
            Dim objcount As Integer = objdb.ModuleApprovals.Where(Function(x) x.ModuleName = objmodule.ModuleName And x.ModuleKey = objifti.objIFTI.PK_IFTI_ID).ToList.Count
            If objcount = 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Shared Function saveApprovalIFTI(objifti As IFTIDataBLL, objmodule As NawaDAL.Module)
        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try

                    objifti.objIFTI.IsDataValid = 1
                    objifti.objIFTIErrorDescription.RemoveAll(Function(x) True)



                    Dim objListParam(1) As SqlParameter
                    objListParam(0) = New SqlParameter
                    objListParam(1) = New SqlParameter

                    objListParam(0).ParameterName = "@Pkiftiid"
                    objListParam(0).Value = objifti.objIFTI.PK_IFTI_ID

                    objListParam(1).ParameterName = "@userid"
                    objListParam(1).Value = NawaBLL.Common.SessionCurrentUser.UserID

                    NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_clearIftiValidateEdit", objListParam)



                    Dim objOldIFTI As IFTIDataBLL = GetIFTIByPk(objifti.objIFTI.PK_IFTI_ID)

                    Dim xmldata As String = NawaBLL.Common.Serialize(objifti)
                    Dim xmldataOld As String = NawaBLL.Common.Serialize(objOldIFTI)


                    Dim objModuleApproval As New NawaDAL.ModuleApproval
                    With objModuleApproval
                        .ModuleName = objmodule.ModuleName
                        .ModuleKey = objifti.objIFTI.PK_IFTI_ID
                        .ModuleField = xmldata
                        .ModuleFieldBefore = xmldataOld
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
                    objaudittrailheader.ModuleLabel = objmodule.ModuleLabel
                    objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
                    objdb.SaveChanges()

                    Dim objtype As Type = objifti.objIFTI.GetType
                    Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
                    For Each item As System.Reflection.PropertyInfo In properties
                        Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                        objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                        objaudittraildetail.FieldName = item.Name
                        objaudittraildetail.OldValue = ""
                        If Not item.GetValue(objifti.objIFTI, Nothing) Is Nothing Then
                            objaudittraildetail.NewValue = item.GetValue(objifti.objIFTI, Nothing)
                        Else
                            objaudittraildetail.NewValue = ""
                        End If
                        objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                    Next

                    If objifti.objIFTIBeneficiary.Count > 0 Then


                        For Each itembenef As NawaDevDAL.IFTI_Beneficiary In objifti.objIFTIBeneficiary

                            Dim propertiesbenef() As System.Reflection.PropertyInfo = itembenef.GetType.GetProperties
                            For Each item As System.Reflection.PropertyInfo In propertiesbenef
                                Dim objaudittraildetail As New NawaDAL.AuditTrailDetail
                                objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID
                                objaudittraildetail.FieldName = item.Name
                                objaudittraildetail.OldValue = ""
                                If Not item.GetValue(itembenef, Nothing) Is Nothing Then
                                    objaudittraildetail.NewValue = item.GetValue(itembenef, Nothing)
                                Else
                                    objaudittraildetail.NewValue = ""
                                End If
                                objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
                            Next
                        Next



                    End If

                    objdb.SaveChanges()
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Shared Function GetGeneratedIFTIByID(id As Integer) As NawaDevDAL.vw_IFTI_ListOfGeneratedIFTI
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Return objDb.vw_IFTI_ListOfGeneratedIFTI.Where(Function(x) x.PK_ListOfGeneratedIFTI_ID = id).FirstOrDefault
        End Using
    End Function

    Shared Function ValidateIFTI(objiftieditvalidate As NawaDevDAL.IFTI_ValidateEdit, objListiftiBeneficiairy As List(Of NawaDevDAL.IFTI_BeneficiaryValidateEdit))
        Try
            Dim objListParam(1) As SqlParameter
            objListParam(0) = New SqlParameter
            objListParam(1) = New SqlParameter

            objListParam(0).ParameterName = "@Pkiftiid"
            objListParam(0).Value = objiftieditvalidate.PK_IFTI_ID

            objListParam(1).ParameterName = "@userid"
            objListParam(1).Value = NawaBLL.Common.SessionCurrentUser.UserID

            NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_clearIftiValidateEdit", objListParam)

            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
                objDb.Entry(objiftieditvalidate).State = Entity.EntityState.Added

                For Each item As NawaDevDAL.IFTI_BeneficiaryValidateEdit In objListiftiBeneficiairy
                    objDb.Entry(item).State = Entity.EntityState.Added
                Next

                objDb.SaveChanges()

                Dim objtable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_IftiValidateEdit", objListParam)

            End Using
        Catch ex As System.Data.Entity.Validation.DbEntityValidationException
            Dim sb As New StringBuilder
            For Each item In ex.EntityValidationErrors
                For Each inneritem In item.ValidationErrors
                    sb.Append(inneritem.ErrorMessage)
                Next
            Next

            Throw New Exception(sb.ToString())
        End Try

    End Function


    Shared Function GetKewargaNegaraanByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_Kewarganegaraan
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objCurrency As NawaDevDAL.IFTI_MS_Kewarganegaraan = objDb.IFTI_MS_Kewarganegaraan.Where(Function(x) x.PK_Ms_Kewarganegaraan_ID = intPk).FirstOrDefault()
            Return objCurrency
        End Using
    End Function


    Shared Function GetCurrencyByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_Currency
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objCurrency As NawaDevDAL.IFTI_MS_Currency = objDb.IFTI_MS_Currency.Where(Function(x) x.PK_MS_Currency_ID = intPk).FirstOrDefault()
            Return objCurrency
        End Using
    End Function

    Shared Function GetBidangUsahaByPk(intPk As Integer) As NawaDevDAL.IFTI_Ms_BidangUsaha
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objpekerjaan As NawaDevDAL.IFTI_Ms_BidangUsaha = objDb.IFTI_Ms_BidangUsaha.Where(Function(x) x.PK_Ms_Bidang_Usaha_ID = intPk).FirstOrDefault()
            Return objpekerjaan
        End Using
    End Function

    Shared Function GetPekerjaanByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_Pekerjaan
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objpekerjaan As NawaDevDAL.IFTI_MS_Pekerjaan = objDb.IFTI_MS_Pekerjaan.Where(Function(x) x.PK_MS_Pekerjaan_ID = intPk).FirstOrDefault()
            Return objpekerjaan
        End Using
    End Function

    Shared Function GetCountryActive() As List(Of NawaDevDAL.IFTI_MS_Negara)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objCountryActive As List(Of NawaDevDAL.IFTI_MS_Negara) = objDb.IFTI_MS_Negara.Where(Function(x) x.Active = True).ToList
            Return objCountryActive
        End Using
    End Function

    Shared Function GetCountryByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_Negara
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objnegara As NawaDevDAL.IFTI_MS_Negara = objDb.IFTI_MS_Negara.Where(Function(x) x.PK_MS_Negara_Id = intPk).FirstOrDefault()
            Return objnegara
        End Using
    End Function

    Shared Function GetPropinsiByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_Propinsi
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objPropinsi As NawaDevDAL.IFTI_MS_Propinsi = objDb.IFTI_MS_Propinsi.Where(Function(x) x.PK_MS_Propinsi_ID = intPk).FirstOrDefault()
            Return objPropinsi
        End Using
    End Function



    Shared Function GetBentukBadanUsaha(intbentukBadan As Integer) As String

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objBentukbadan As NawaDevDAL.IFTI_MS_BentukBadanUsaha = objDb.IFTI_MS_BentukBadanUsaha.Where(Function(x) x.PK_Ms_Bentuk_Badan_Usaha_ID = intbentukBadan).FirstOrDefault
            If Not objBentukbadan Is Nothing Then
                Return objBentukbadan.NamaBentukBadanUsaha
            Else
                Return ""
            End If
        End Using

    End Function
    Shared Function GetBeneficialOwnerTypeByPk() As List(Of NawaDevDAL.IFTI_BeneficialOwnerType)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objBeneficialOwnerType As List(Of NawaDevDAL.IFTI_BeneficialOwnerType) = objDb.IFTI_BeneficialOwnerType.Where(Function(x) x.Active = True).ToList
            Return objBeneficialOwnerType
        End Using
    End Function


    Shared Function GetIFTI_IDTYPEbyPK(intpk As Integer) As NawaDevDAL.IFTI_IDType
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNamaParameter As NawaDevDAL.IFTI_IDType = objDb.IFTI_IDType.Where(Function(x) x.PK_IFTI_IDType = intpk).FirstOrDefault
            Return objNamaParameter
        End Using
    End Function
    Shared Function GetIFTI_IDTYPEList() As List(Of NawaDevDAL.IFTI_IDType)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNamaParameter As List(Of NawaDevDAL.IFTI_IDType) = objDb.IFTI_IDType.ToList
            Return objNamaParameter
        End Using
    End Function

    Shared Function GetKotaKabByPk(intPk As Integer) As NawaDevDAL.IFTI_MS_KotaKab
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objKotaKab As NawaDevDAL.IFTI_MS_KotaKab = objDb.IFTI_MS_KotaKab.Where(Function(x) x.PK_MS_KotaKab_Id = intPk).FirstOrDefault()
            Return objKotaKab
        End Using
    End Function

    Shared Function GetKotaKab() As List(Of NawaDevDAL.IFTI_MS_KotaKab)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objKotaKab As List(Of NawaDevDAL.IFTI_MS_KotaKab) = objDb.IFTI_MS_KotaKab.Where(Function(x) x.Active = True).ToList
            Return objKotaKab
        End Using
    End Function

    Shared Function getBenefIFTINasabahTypeALL() As List(Of NawaDevDAL.IFTI_NasabahType)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNasabahType As List(Of NawaDevDAL.IFTI_NasabahType) = objDb.IFTI_NasabahType.ToList()
            Return objNasabahType
        End Using
    End Function

    Shared Function getBenefIFTINasabahType() As List(Of NawaDevDAL.IFTI_NasabahType)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNasabahType As List(Of NawaDevDAL.IFTI_NasabahType) = objDb.IFTI_NasabahType.Where(Function(x) x.FK_IFTI_PJKBank_ID = 1).ToList()
            Return objNasabahType
        End Using
    End Function





    Shared Function getIFTINasabahTypeByPjkBank(intPk As Integer) As NawaDevDAL.IFTI_NasabahType
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNasabahType As NawaDevDAL.IFTI_NasabahType = objDb.IFTI_NasabahType.Where(Function(x) x.PK_IFTI_NasabahType_ID = intPk).FirstOrDefault
            Return objNasabahType
        End Using
    End Function

    Shared Function getIFTINasabahTypeByPjk(intPk As Integer) As List(Of NawaDevDAL.IFTI_NasabahType)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objNasabahType As List(Of NawaDevDAL.IFTI_NasabahType) = objDb.IFTI_NasabahType.Where(Function(x) x.FK_IFTI_PJKBank_ID = intPk).ToList()
            Return objNasabahType
        End Using
    End Function

    Shared Function getIFTI_PJKBankTypeActiveIncoming() As List(Of NawaDevDAL.IFTI_PJKBank)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objgetIFTI_PJKBankTypeActive As List(Of NawaDevDAL.IFTI_PJKBank) = objDb.IFTI_PJKBank.Where(Function(x) x.PK_IFTI_PJKBank_ID = 1).ToList
            Return objgetIFTI_PJKBankTypeActive
        End Using
    End Function





    Shared Function getIFTI_PJKBankTypePK(intpk As Integer) As NawaDevDAL.IFTI_PJKBank
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objgetIFTI_PJKBankTypeActive As NawaDevDAL.IFTI_PJKBank = objDb.IFTI_PJKBank.Where(Function(x) x.PK_IFTI_PJKBank_ID = intpk).FirstOrDefault
            Return objgetIFTI_PJKBankTypeActive
        End Using
    End Function

    Shared Function getIFTI_PJKBankTypeActive() As List(Of NawaDevDAL.IFTI_PJKBank)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objgetIFTI_PJKBankTypeActive As List(Of NawaDevDAL.IFTI_PJKBank) = objDb.IFTI_PJKBank.Where(Function(x) x.Active = True).ToList
            Return objgetIFTI_PJKBankTypeActive
        End Using
    End Function

    Shared Function GetIFTITypeByPk(intPk As Integer) As NawaDevDAL.IFTI_Type
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objIFTIType As NawaDevDAL.IFTI_Type = objDb.IFTI_Type.Where(Function(x) x.PK_IFTI_Type_ID = intPk).FirstOrDefault()
            Return objIFTIType
        End Using
    End Function


    Shared Function GetIFTI_JenisLaporanbyPK(pk As Integer) As NawaDevDAL.IFTI_JenisLaporan
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objIFTI_JenisLaporan As NawaDevDAL.IFTI_JenisLaporan = objDb.IFTI_JenisLaporan.Where(Function(x) x.PK_IFTI_JenisLaporan = pk).FirstOrDefault
            Return objIFTI_JenisLaporan
        End Using
    End Function
    Shared Function GetIFTI_JenisLaporanActive() As List(Of NawaDevDAL.IFTI_JenisLaporan)
        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objIFTI_JenisLaporan As List(Of NawaDevDAL.IFTI_JenisLaporan) = objDb.IFTI_JenisLaporan.Where(Function(x) x.Active = True).ToList
            Return objIFTI_JenisLaporan
        End Using
    End Function

    Shared Function GetIFTIByPk(intPk As Integer) As NawaDevBLL.IFTIDataBLL

        Dim objIFTIDataBLL As New IFTIDataBLL

        Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities
            Dim objIFTI As NawaDevDAL.IFTI = objDb.IFTIs.Where(Function(x) x.PK_IFTI_ID = intPk).FirstOrDefault()
            Dim objListIFTI_Beneficiary As List(Of NawaDevDAL.IFTI_Beneficiary) = objDb.IFTI_Beneficiary.Where(Function(y) y.FK_IFTI_ID = intPk).ToList
            Dim objListIFTI_ErrorDescription As List(Of NawaDevDAL.IFTI_ErrorDescription) = objDb.IFTI_ErrorDescription.Where(Function(z) z.FK_IFTI_ID = intPk).ToList

            objIFTIDataBLL.objIFTI = objIFTI
            objIFTIDataBLL.objIFTIBeneficiary = objListIFTI_Beneficiary
            objIFTIDataBLL.objIFTIErrorDescription = objListIFTI_ErrorDescription
            Return objIFTIDataBLL
        End Using
    End Function

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class