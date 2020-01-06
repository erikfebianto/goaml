Imports Ext.Net
Imports System.IO
Imports OfficeOpenXml
Imports System.Data
Imports NawaDevDAL
Imports System.Data.SqlClient
Imports Microsoft.SqlServer.Server


Public Class ModuleUploadApprovalDetail
    Inherits Parent

    'Dim flagger As Integer

    Public Property dataparam() As NawaDevBLL.datamodules
        Get
            If Session("ModuleUploadApprovalDetail.dataparam") Is Nothing Then
                Dim dataparamx As New NawaDevBLL.datamodules
                Session("ModuleUploadApprovalDetail.dataparam") = dataparamx
            End If

            Return Session("ModuleUploadApprovalDetail.dataparam")
        End Get
        Set(ByVal value As NawaDevBLL.datamodules)
            Session("ModuleUploadApprovalDetail.dataparam") = value
        End Set
    End Property


    Private _IDReq As Long
    Public Property IDReq() As Long
        Get
            Return _IDReq
        End Get
        Set(ByVal value As Long)
            _IDReq = value
        End Set
    End Property

    Private _objApproval As NawaDAL.ModuleApproval
    Public Property ObjApproval() As NawaDAL.ModuleApproval
        Get
            Return _objApproval
        End Get
        Set(ByVal value As NawaDAL.ModuleApproval)
            _objApproval = value
        End Set
    End Property



    Public Property objSchemaModule() As NawaDAL.Module
        Get
            Return Session("ModuleUploadApprovalDetail.objSchemaModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("ModuleUploadApprovalDetail.objSchemaModule") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadGridModel()

        Try
            Dim strid As String = Request.Params("ID")
            Dim strModuleid As String = Request.Params("ModuleID")

            Try

                IDReq = NawaBLL.Common.DecryptQueryString(strid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                Dim moduleid As Integer = NawaBLL.Common.DecryptQueryString(strModuleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objSchemaModule = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objSchemaModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If
            Catch ex As Exception
                Throw New Exception("Invalid ID Approval.")
            End Try

            If Not Ext.Net.X.IsAjaxRequest Then


                ObjApproval = NawaBLL.ModuleApprovalBLL.GetModuleApprovalByID(IDReq)
                If Not ObjApproval Is Nothing Then
                    PanelInfo.Title = ObjApproval.ModuleName & " Approval"
                    lblModuleName.Text = ObjApproval.ModuleName
                    lblModuleKey.Text = ObjApproval.ModuleKey
                    lblAction.Text = NawaBLL.ModuleBLL.GetModuleActionNamebyID(ObjApproval.PK_ModuleAction_ID)
                    LblCreatedBy.Text = ObjApproval.CreatedBy
                    lblCreatedDate.Text = ObjApproval.CreatedDate.Value.ToString("dd-MMM-yyyy")

                End If



            End If

            Try
                dataparam = NawaBLL.Common.Deserialize(ObjApproval.ModuleField, GetType(NawaDevBLL.datamodules))
            Catch ex As Exception

            End Try

            'Panel1.Hidden = False
            'Panel1.Visible = True
            gridModule.Hidden = False
            gridModule.Visible = True
            storeModule.Visible = True
            storeModuleField.Visible = True
            'container.Visible = True

            loadtable()



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        'flagger = 1


    End Sub

    Public Sub loadtable()

        'LoadGridModel()

        Dim dmodule As System.Data.DataTable = NawaBLL.Common.CopyGenericToDataTable(dataparam.dtmodule)
        Dim dmoduleField As System.Data.DataTable = dataparam.dtmodulefield

        storeModule.DataSource = dmodule
        storeModule.DataBind()
        storeModuleField.DataSource = dmoduleField
        storeModuleField.DataBind()

    End Sub

    Private Function addNewModelField(FieldName As String) As Ext.Net.ModelField
        Dim objfield As Ext.Net.ModelField
        objfield = New Ext.Net.ModelField
        objfield.Name = FieldName
        Return objfield
    End Function

    Shared Function addNewColumn(ColumnText As String, ColumnIndex As String) As Ext.Net.Column
        Dim objcolum As Ext.Net.Column
        objcolum = New Ext.Net.Column
        objcolum.Text = ColumnText
        objcolum.DataIndex = ColumnIndex
        objcolum.ClientIDMode = Web.UI.ClientIDMode.Static
        Return objcolum
    End Function
    Private Sub LoadGridModel()
        Using objdb As New NawaDevDAL.NawaDatadevEntities
            Try
                ''Untuk generate column & model.
                modelModule.Fields.Add(addNewModelField("Action"))
                modelModule.Fields.Add(addNewModelField("ErrorMessage"))
                modelModule.Fields.Add(addNewModelField("PK_Module_ID"))
                modelModule.Fields.Add(addNewModelField("ModuleName"))
                modelModule.Fields.Add(addNewModelField("ModuleLabel"))
                modelModule.Fields.Add(addNewModelField("ModuleDescription"))

                modelModule.Fields.Add(addNewModelField("IsUseDesigner"))
                modelModule.Fields.Add(addNewModelField("IsUseApproval"))
                modelModule.Fields.Add(addNewModelField("IsSupportAdd"))
                modelModule.Fields.Add(addNewModelField("IsSupportEdit"))
                modelModule.Fields.Add(addNewModelField("IsSupportDelete"))
                modelModule.Fields.Add(addNewModelField("IsSupportActivation"))

                modelModule.Fields.Add(addNewModelField("IsSupportView"))
                modelModule.Fields.Add(addNewModelField("IsSupportUpload"))
                modelModule.Fields.Add(addNewModelField("IsSupportDetail"))
                modelModule.Fields.Add(addNewModelField("UrlAdd"))
                modelModule.Fields.Add(addNewModelField("UrlEdit"))
                modelModule.Fields.Add(addNewModelField("UrlDelete"))

                modelModule.Fields.Add(addNewModelField("UrlActivation"))
                modelModule.Fields.Add(addNewModelField("UrlView"))
                modelModule.Fields.Add(addNewModelField("UrlUpload"))
                modelModule.Fields.Add(addNewModelField("UrlApproval"))
                modelModule.Fields.Add(addNewModelField("UrlApprovalDetail"))
                modelModule.Fields.Add(addNewModelField("UrlDetail"))

                modelModule.Fields.Add(addNewModelField("IsUseStoreProcedureValidation"))

                gridModule.ColumnModel.Add(addNewColumn("Action", "Action"))
                gridModule.ColumnModel.Add(addNewColumn("Error Message", "ErrorMessage"))
                gridModule.ColumnModel.Add(addNewColumn("Module Name", "ModuleName"))
                gridModule.ColumnModel.Add(addNewColumn("Module Label", "ModuleLabel"))
                gridModule.ColumnModel.Add(addNewColumn("Module Description", "ModuleDescription"))
                gridModule.ColumnModel.Add(addNewColumn("Use Designer", "IsUseDesigner"))

                gridModule.ColumnModel.Add(addNewColumn("Use Approval", "IsUseApproval"))
                gridModule.ColumnModel.Add(addNewColumn("Support Add", "IsSupportAdd"))
                gridModule.ColumnModel.Add(addNewColumn("Support Edit", "IsSupportEdit"))
                gridModule.ColumnModel.Add(addNewColumn("Support Delete", "IsSupportDelete"))
                gridModule.ColumnModel.Add(addNewColumn("Support Activation", "IsSupportActivation"))
                gridModule.ColumnModel.Add(addNewColumn("Support View", "IsSupportView"))

                gridModule.ColumnModel.Add(addNewColumn("Support Upload", "IsSupportUpload"))
                gridModule.ColumnModel.Add(addNewColumn("Support Detail", "IsSupportDetail"))
                gridModule.ColumnModel.Add(addNewColumn("URL Add", "UrlAdd"))
                gridModule.ColumnModel.Add(addNewColumn("URL Edit", "UrlEdit"))
                gridModule.ColumnModel.Add(addNewColumn("URL Delete", "UrlDelete"))
                gridModule.ColumnModel.Add(addNewColumn("URL Activation", "UrlActivation"))

                gridModule.ColumnModel.Add(addNewColumn("URL View", "UrlView"))
                gridModule.ColumnModel.Add(addNewColumn("URL Upload", "UrlUpload"))
                gridModule.ColumnModel.Add(addNewColumn("URL Approval", "UrlApproval"))
                gridModule.ColumnModel.Add(addNewColumn("URL Approval Detail", "UrlApprovalDetail"))
                gridModule.ColumnModel.Add(addNewColumn("URL Detail", "UrlDetail"))
                gridModule.ColumnModel.Add(addNewColumn("Use Stored Procedure Validation", "IsUseStoreProcedureValidation"))


                ''Module Field
                modelModuleField.Fields.Add(addNewModelField("Action"))
                modelModuleField.Fields.Add(addNewModelField("ErrorMessage"))
                modelModuleField.Fields.Add(addNewModelField("PK_ModuleField_ID"))
                modelModuleField.Fields.Add(addNewModelField("ModuleName"))
                modelModuleField.Fields.Add(addNewModelField("FieldName"))
                modelModuleField.Fields.Add(addNewModelField("FieldLabel"))

                modelModuleField.Fields.Add(addNewModelField("Sequence"))
                modelModuleField.Fields.Add(addNewModelField("Required"))
                modelModuleField.Fields.Add(addNewModelField("IsPrimaryKey"))
                modelModuleField.Fields.Add(addNewModelField("IsUnik"))
                modelModuleField.Fields.Add(addNewModelField("IsShowInView"))
                modelModuleField.Fields.Add(addNewModelField("IsShowInForm"))

                modelModuleField.Fields.Add(addNewModelField("DefaultValue"))
                modelModuleField.Fields.Add(addNewModelField("FK_FieldType_ID"))
                modelModuleField.Fields.Add(addNewModelField("SizeField"))
                modelModuleField.Fields.Add(addNewModelField("FK_ExtType_ID"))
                modelModuleField.Fields.Add(addNewModelField("TabelReferenceName"))
                modelModuleField.Fields.Add(addNewModelField("TabelReferenceNameAlias"))

                modelModuleField.Fields.Add(addNewModelField("TableReferenceFieldKey"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceFieldDisplayName"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceFilter"))
                modelModuleField.Fields.Add(addNewModelField("IsUseRegexValidation"))
                modelModuleField.Fields.Add(addNewModelField("TableReferenceAdditonalJoin"))
                modelModuleField.Fields.Add(addNewModelField("BCasCade"))

                modelModuleField.Fields.Add(addNewModelField("FieldNameParent"))
                modelModuleField.Fields.Add(addNewModelField("FilterCascade"))

                gridModuleField.ColumnModel.Add(addNewColumn("Action", "Action"))
                gridModuleField.ColumnModel.Add(addNewColumn("Error Message", "ErrorMessage"))
                gridModuleField.ColumnModel.Add(addNewColumn("Module Name", "ModuleName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Name", "FieldName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Label", "FieldLabel"))
                gridModuleField.ColumnModel.Add(addNewColumn("Sequence", "Sequence"))

                gridModuleField.ColumnModel.Add(addNewColumn("Required", "Required"))
                gridModuleField.ColumnModel.Add(addNewColumn("Primary Key", "IsPrimaryKey"))
                gridModuleField.ColumnModel.Add(addNewColumn("Unik", "IsUnik"))
                gridModuleField.ColumnModel.Add(addNewColumn("Show in View", "IsShowInView"))
                gridModuleField.ColumnModel.Add(addNewColumn("Show in Form", "IsShowInForm"))
                gridModuleField.ColumnModel.Add(addNewColumn("Default Value", "DefaultValue"))

                gridModuleField.ColumnModel.Add(addNewColumn("Field Type", "FK_FieldType_ID"))
                gridModuleField.ColumnModel.Add(addNewColumn("Size Field", "SizeField"))
                gridModuleField.ColumnModel.Add(addNewColumn("Ext Type", "FK_ExtType_ID"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Name", "TabelReferenceName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Alias", "TabelReferenceNameAlias"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Key", "TableReferenceFieldKey"))

                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Display Name", "TableReferenceFieldDisplayName"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Filter", "TableReferenceFilter"))
                gridModuleField.ColumnModel.Add(addNewColumn("Use Regex", "IsUseRegexValidation"))
                gridModuleField.ColumnModel.Add(addNewColumn("Table Reference Additional Join", "TableReferenceAdditonalJoin"))
                gridModuleField.ColumnModel.Add(addNewColumn("Cascade", "BCasCade"))
                gridModuleField.ColumnModel.Add(addNewColumn("Field Name Parent", "FieldNameParent"))

                gridModuleField.ColumnModel.Add(addNewColumn("Filter Cascade", "FilterCascade"))

            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try
        End Using
    End Sub

    Private Function SubmitData(ByVal dtModule As NawaDevBLL.datamodules) As Boolean
        ''Note: Sekarang ini untuk module yang di flag sebagai update modulename tidak di update ke DB.
        ''Note: Sekarang ini module field yang di flag update juga tidak ada update fieldname dan moduleID.
        ''Note: Untuk module field apakah perlu diperlakukan sama seperti di module designer? Delete dan Insert.
        ''      Jadi tidak ada proses update.

        'Dim OldObj As NawaDAL.Module

        'Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
        '    OldObj = (From x In objDb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault
        'End Using

        Dim ModuleList As New List(Of Integer)
        Dim DeletedModule As String = ""

        Using objdb As New NawaDAL.NawaDataEntities
            Using objtrans As System.Data.Entity.DbContextTransaction = objdb.Database.BeginTransaction()
                Try
                    ''Save Module
                    For Each item As NawaDevBLL.datamodules.ClassModules In dtModule.dtmodule 'DataRow In dtModule.Rows
                        If item.ErrorMessage.ToString <> "" And item.ErrorMessage.ToString IsNot Nothing Then
                            Throw New Exception("Record(s) with error(s) still exist on Module list. Module " & CStr(item.ModuleName) & ". Message:" & item.ErrorMessage)
                        Else
                            If item.Action.ToString = "Insert" Then
                                Dim newObj As New NawaDAL.Module
                                With newObj
                                    .ModuleName = CStr(item.ModuleName)
                                    .ModuleLabel = CStr(item.ModuleLabel)
                                    .ModuleDescription = CStr(item.ModuleDescription)
                                    .IsUseDesigner = CBool(item.IsUseDesigner)
                                    .IsUseApproval = CBool(item.IsUseApproval)
                                    .IsSupportAdd = CBool(item.IsSupportAdd)
                                    .IsSupportEdit = CBool(item.IsSupportEdit)
                                    .IsSupportDelete = CBool(item.IsSupportDelete)
                                    .IsSupportActivation = CBool(item.IsSupportActivation)
                                    .IsSupportView = CBool(item.IsSupportView)
                                    .IsSupportUpload = CBool(item.IsSupportUpload)
                                    .IsSupportDetail = CBool(item.IsSupportDetail)
                                    .UrlAdd = CStr(item.UrlAdd)
                                    .UrlEdit = CStr(item.UrlEdit)
                                    .UrlDelete = CStr(item.UrlDelete)
                                    .UrlActivation = CStr(item.UrlActivation)
                                    .UrlView = CStr(item.UrlView)
                                    .UrlUpload = CStr(item.UrlUpload)
                                    .UrlApproval = CStr(item.UrlApproval)
                                    .UrlApprovalDetail = CStr(item.UrlApprovalDetail)
                                    .UrlDetail = CStr(item.UrlDetail)
                                    .IsUseStoreProcedureValidation = CBool(item.IsUseStoreProcedureValidation)

                                    .CreatedDate = Now
                                    .CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID
                                    .LastUpdateDate = Now
                                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                                End With

                                objdb.Entry(newObj).State = Entity.EntityState.Added
                                objdb.SaveChanges()

                                ''Untuk kebutuhan di save modulefield
                                item.PK_Module_ID = newObj.PK_Module_ID

                                ''Untuk diproses
                                ModuleList.Add(newObj.PK_Module_ID)
                            ElseIf item.Action.ToString = "Update" Then
                                Dim ID As String = item.PK_Module_ID.ToString

                                Dim modifiedObj As NawaDAL.Module = (From x In objdb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault
                                With modifiedObj
                                    .ModuleLabel = CStr(item.ModuleLabel)
                                    .ModuleDescription = CStr(item.ModuleDescription)
                                    .IsUseDesigner = CBool(item.IsUseDesigner)
                                    .IsUseApproval = CBool(item.IsUseApproval)
                                    .IsSupportAdd = CBool(item.IsSupportAdd)
                                    .IsSupportEdit = CBool(item.IsSupportEdit)
                                    .IsSupportDelete = CBool(item.IsSupportDelete)
                                    .IsSupportActivation = CBool(item.IsSupportActivation)
                                    .IsSupportView = CBool(item.IsSupportView)
                                    .IsSupportUpload = CBool(item.IsSupportUpload)
                                    .IsSupportDetail = CBool(item.IsSupportDetail)
                                    .UrlAdd = CStr(item.UrlAdd)
                                    .UrlEdit = CStr(item.UrlEdit)
                                    .UrlDelete = CStr(item.UrlDelete)
                                    .UrlActivation = CStr(item.UrlActivation)
                                    .UrlView = CStr(item.UrlView)
                                    .UrlUpload = CStr(item.UrlUpload)
                                    .UrlApproval = CStr(item.UrlApproval)
                                    .UrlApprovalDetail = CStr(item.UrlApprovalDetail)
                                    .UrlDetail = CStr(item.UrlDetail)
                                    .IsUseStoreProcedureValidation = CBool(item.IsUseStoreProcedureValidation)

                                    .LastUpdateDate = Now
                                    .LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID
                                End With

                                objdb.Entry(modifiedObj).State = Entity.EntityState.Modified
                                objdb.SaveChanges()

                                ''Untuk diproses
                                ModuleList.Add(modifiedObj.PK_Module_ID)
                            ElseIf item.Action.ToString = "Delete" Then
                                Dim ID As String = item.PK_Module_ID.ToString

                                Dim deletedObj As NawaDAL.Module = (From x In objdb.Modules Where x.PK_Module_ID = ID Select x).FirstOrDefault

                                ''Query untuk drop table module yang di delete
                                If DeletedModule <> "" Then
                                    DeletedModule = DeletedModule & " DROP TABLE " & deletedObj.ModuleName & " DROP TABLE " & deletedObj.ModuleName & "_Upload"
                                Else
                                    DeletedModule = "DROP TABLE " & deletedObj.ModuleName & " DROP TABLE " & deletedObj.ModuleName & "_Upload"
                                End If

                                objdb.Entry(deletedObj).State = Entity.EntityState.Deleted
                                objdb.SaveChanges()
                            Else
                                Throw New Exception("Undefined Action On Module list. Action" & item.Action)
                            End If
                        End If
                    Next

                    ''Save Module Field
                    Dim dtModuleField As DataTable = dtModule.dtmodulefield
                    For Each item As DataRow In dtModuleField.Rows
                        If item("ErrorMessage").ToString <> "" And item("ErrorMessage").ToString IsNot Nothing Then
                            Throw New Exception("Record(s) with error(s) still exist on Module Field list. Field " & CStr(item("FieldName")) & " at module " & CStr(item("ModuleName")) & ". Message:" & item("ErrorMessage"))
                        Else
                            If item("Action").ToString = "Insert" Then
                                ''Validasi Primary Key
                                Dim validateKey() As DataRow
                                validateKey = dtModuleField.Select("ModuleName = '" & CStr(item("ModuleName")) & "' And IsPrimaryKey=1")
                                If validateKey.Count = 0 Then
                                    Throw New Exception("No Primary Key found for module " & CStr(item("ModuleName")))
                                End If

                                Dim newObj As New NawaDAL.ModuleField
                                With newObj
                                    Dim ModuleID As Long = 0
                                    Try
                                        Dim Param As String = CStr(item("ModuleName"))
                                        ModuleID = (From x In objdb.Modules Where x.ModuleName = Param Select x).FirstOrDefault.PK_Module_ID

                                        If ModuleID = Nothing Or ModuleID = 0 Then
                                            Throw New Exception
                                        End If
                                    Catch ex As Exception
                                        Dim findRow() As DataRow

                                        For Each LstModule As NawaDevBLL.datamodules.ClassModules In dtModule.dtmodule
                                            If LstModule.ModuleName.ToUpper = CStr(item("ModuleName")).ToUpper Then
                                                ModuleID = LstModule.PK_Module_ID
                                            End If
                                        Next
                                        'findRow = dtModule.Select("ModuleName = '" & CStr(item("ModuleName")) & "'")

                                        ''Harusnya pasti return 1 row saja. Kalau lebih dari 1 berarti validasinya kelewat
                                        For Each item2 In findRow
                                            ModuleID = CLng(item2("PK_Module_ID"))
                                        Next
                                    End Try

                                    .FK_Module_ID = ModuleID

                                    .FieldName = CStr(item("FieldName"))
                                    .FieldLabel = CStr(item("FieldLabel"))
                                    .Sequence = CInt(item("Sequence"))
                                    .Required = CBool(item("Required"))
                                    .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                    .IsUnik = CBool(item("IsUnik"))
                                    .IsShowInView = CBool(item("IsShowInView"))
                                    .IsShowInForm = CBool(item("IsShowInForm"))
                                    .DefaultValue = CStr(item("DefaultValue"))

                                    Dim FieldTypeID As Integer = 0
                                    If CStr(item("FK_FieldType_ID")) <> "" And CStr(item("FK_FieldType_ID")) IsNot Nothing Then
                                        Dim Param As String = CStr(item("FK_FieldType_ID"))
                                        FieldTypeID = (From x In objdb.MFieldTypes Where x.FieldTypeCaption = Param Select x).FirstOrDefault.PK_FieldType_ID
                                    End If

                                    .FK_FieldType_ID = FieldTypeID

                                    .SizeField = CInt(item("SizeField"))

                                    Dim ExtTypeID As Integer = 0
                                    If CStr(item("FK_ExtType_ID")) <> "" And CStr(item("FK_ExtType_ID")) IsNot Nothing Then
                                        Dim Param As String = CStr(item("FK_ExtType_ID"))
                                        ExtTypeID = (From x In objdb.MExtTypes Where x.ExtTypeName = Param Select x).FirstOrDefault.PK_ExtType_ID
                                    End If

                                    .FK_ExtType_ID = ExtTypeID

                                    .TabelReferenceName = CStr(item("TabelReferenceName"))
                                    .TabelReferenceNameAlias = CStr(item("TabelReferenceNameAlias"))
                                    .TableReferenceFieldKey = CStr(item("TableReferenceFieldKey"))
                                    .TableReferenceFieldDisplayName = CStr(item("TableReferenceFieldDisplayName"))
                                    .TableReferenceFilter = CStr(item("TableReferenceFilter"))
                                    .IsUseRegexValidation = CBool(item("IsUseRegexValidation"))
                                    .TableReferenceAdditonalJoin = CStr(item("TableReferenceAdditonalJoin"))
                                    .BCasCade = CBool(item("BCasCade"))
                                    .FieldNameParent = CStr(item("FieldNameParent"))
                                    .FilterCascade = CStr(item("FilterCascade"))
                                End With

                                objdb.Entry(newObj).State = Entity.EntityState.Added
                                objdb.SaveChanges()
                            ElseIf item("Action").ToString = "Update" Then
                                ''Validasi Primary Key
                                Dim validateKey() As DataRow
                                validateKey = dtModuleField.Select("ModuleName = '" & CStr(item("ModuleName")) & "' And IsPrimaryKey=1")
                                If validateKey.Count = 0 Then
                                    Throw New Exception("No Primary Key found for module " & CStr(item("ModuleName")))
                                End If

                                Dim ID As String = item("PK_ModuleField_ID").ToString

                                Dim modifiedObj As NawaDAL.ModuleField = (From x In objdb.ModuleFields Where x.PK_ModuleField_ID = ID Select x).FirstOrDefault
                                With modifiedObj
                                    .FieldLabel = CStr(item("FieldLabel"))
                                    .Sequence = CInt(item("Sequence"))
                                    .Required = CBool(item("Required"))
                                    .IsPrimaryKey = CBool(item("IsPrimaryKey"))
                                    .IsUnik = CBool(item("IsUnik"))
                                    .IsShowInView = CBool(item("IsShowInView"))
                                    .IsShowInForm = CBool(item("IsShowInForm"))
                                    .DefaultValue = CStr(item("DefaultValue"))

                                    Dim FieldTypeID As Integer = 0
                                    If CStr(item("FK_FieldType_ID")) <> "" And CStr(item("FK_FieldType_ID")) IsNot Nothing Then
                                        Dim strFK_FieldType_ID As String = CStr(item("FK_FieldType_ID"))
                                        FieldTypeID = (From x In objdb.MFieldTypes Where x.FieldTypeCaption = strFK_FieldType_ID Select x).FirstOrDefault.PK_FieldType_ID
                                    End If

                                    .FK_FieldType_ID = FieldTypeID

                                    .SizeField = CInt(item("SizeField"))

                                    Dim ExtTypeID As Integer = 0
                                    If CStr(item("FK_ExtType_ID")) <> "" And CStr(item("FK_ExtType_ID")) IsNot Nothing Then
                                        Dim strFK_ExtType_ID As String = CStr(item("FK_ExtType_ID"))
                                        ExtTypeID = (From x In objdb.MExtTypes Where x.ExtTypeName = strFK_ExtType_ID Select x).FirstOrDefault.PK_ExtType_ID
                                    End If

                                    .FK_ExtType_ID = ExtTypeID

                                    .TabelReferenceName = CStr(item("TabelReferenceName"))
                                    .TabelReferenceNameAlias = CStr(item("TabelReferenceNameAlias"))
                                    .TableReferenceFieldKey = CStr(item("TableReferenceFieldKey"))
                                    .TableReferenceFieldDisplayName = CStr(item("TableReferenceFieldDisplayName"))
                                    .TableReferenceFilter = CStr(item("TableReferenceFilter"))
                                    .IsUseRegexValidation = CBool(item("IsUseRegexValidation"))
                                    .TableReferenceAdditonalJoin = CStr(item("TableReferenceAdditonalJoin"))
                                    .BCasCade = CBool(item("BCasCade"))
                                    .FieldNameParent = CStr(item("FieldNameParent"))
                                    .FilterCascade = CStr(item("FilterCascade"))
                                End With

                                objdb.Entry(modifiedObj).State = Entity.EntityState.Modified
                                objdb.SaveChanges()
                            ElseIf item("Action").ToString = "Delete" Then
                                Dim ID As String = item("PK_ModuleField_ID").ToString

                                Dim deletedObj As NawaDAL.ModuleField = (From x In objdb.ModuleFields Where x.PK_ModuleField_ID = ID Select x).FirstOrDefault

                                objdb.Entry(deletedObj).State = Entity.EntityState.Deleted
                                objdb.SaveChanges()
                            Else
                                Throw New Exception("Undefined Action on Module Field list. Action:" & item("Action"))
                            End If
                        End If
                    Next
                    objtrans.Commit()
                Catch ex As Exception
                    objtrans.Rollback()
                    Throw New Exception(ex.Message)
                End Try
            End Using
        End Using

        Using objdb As New NawaDAL.NawaDataEntities
            ''Create, Alter, Drop table fisik
            ProcessModule(DeletedModule, ModuleList, objdb)
        End Using
    End Function

    Protected Sub ProcessModule(QueryDelete As String, ListModule As List(Of Integer), db As NawaDAL.NawaDataEntities)
        Try
            Dim parameters As SqlParameter() = {
                        New SqlParameter() With {.SqlDbType = SqlDbType.VarChar, .Size = -1, .ParameterName = "DeleteQuery", .Value = QueryDelete},
                        FillModuleList(ListModule)
                    }

            NawaDAL.SQLHelper.ExecuteNonQuery(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_Process_ModuleUpload", parameters)
            'Dim detailInfos As List(Of NawaDAL.Module) = db.Database.SqlQuery(Of NawaDAL.Module)("EXEC usp_Process_ModuleUpload @DeleteQuery, @ModuleList", parameters).ToList
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function FillModuleList(ListData As List(Of Integer)) As SqlParameter
        Dim tableSchema As SqlMetaData() = {
                    New SqlMetaData("ID", SqlDbType.Int)
                }

        Dim tableData As New List(Of SqlDataRecord)

        If ListData IsNot Nothing Then
            If ListData.Count > 0 Then
                For Each itemlist As Integer In ListData
                    Dim tableRow As New SqlDataRecord(tableSchema)
                    tableRow.SetInt32(0, itemlist)
                    tableData.Add(tableRow)
                Next
            Else
                tableData = Nothing
            End If
        Else
            tableData = Nothing
        End If

        Dim sqlParameter As New SqlParameter() With {
                .SqlDbType = SqlDbType.Structured,
                .ParameterName = "ModuleList",
                .TypeName = "[dbo].[LocationTableType]",
                .Value = tableData
            }

        Return sqlParameter
    End Function


    Protected Sub BtnSave_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try

            Try
                SubmitData(dataparam)
                LblConfirmation.Text = "Data Approved. Click Ok to Back To Module Approval."
                Panelconfirmation.Hidden = False
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            End Try


            ' NawaBLL.MGroupAccessBLL.Accept(Me.IDReq)


            Session("DataMenu") = Nothing
            'Container.Hidden = True
            FormPanelInput.Hidden = True
            Panelconfirmation.Hidden = False
            'container.Render()
            Panelconfirmation.Render()

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub
    Protected Sub BtnReject_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        Try
            NawaBLL.MGroupAccessBLL.Reject(Me.IDReq)
            LblConfirmation.Text = "Data Rejected. Click Ok to Back To Module Approval."
            'container.Hidden = True
            FormPanelInput.Hidden = True
            Panelconfirmation.Hidden = False
            'container.Render()
            Panelconfirmation.Render()


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As Ext.Net.DirectEventArgs)
        'Try

        '    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objSchemaModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID")))
        'Catch ex As Exception
        '    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
        '    Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        'End Try

        loadtable()

    End Sub

    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try


            Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & objSchemaModule.UrlApproval & "?ModuleID={0}", Request.Params("ModuleID")))
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
End Class

'<Serializable()> Public Class datamodules

'    Public dtmodule As DataTable
'    Public dtmodulefield As DataTable

'End Class
