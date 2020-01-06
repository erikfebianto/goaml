Imports System.Reflection
Imports Ext.Net
Public Class NawaFramework

    Public Shared Function CreateAuditTrail(ByRef objdb As NawaDevDAL.NawaDatadevEntities, Approver As String, FK_AuditTrailStatus_ID As NawaBLL.Common.AuditTrailStatusEnum, FK_ModuleAction_ID As NawaBLL.Common.ModuleActionEnum, modulelabel As String) As NawaDevDAL.AuditTrailHeader


        Dim objaudittrailheader As New NawaDevDAL.AuditTrailHeader
        objaudittrailheader.ApproveBy = Approver
        objaudittrailheader.CreatedBy = Approver
        objaudittrailheader.CreatedDate = Now.ToString("yyyy-MM-dd HH:mm:ss")
        objaudittrailheader.FK_AuditTrailStatus_ID = FK_AuditTrailStatus_ID
        objaudittrailheader.FK_ModuleAction_ID = FK_ModuleAction_ID
        objaudittrailheader.ModuleLabel = modulelabel
        objdb.Entry(objaudittrailheader).State = Entity.EntityState.Added
        objdb.SaveChanges()
        Return objaudittrailheader
    End Function

    Public Shared Sub CreateAuditTrailDetailAdd(ByRef objdb As NawaDevDAL.NawaDatadevEntities, PKAuditTrailHeaderid As Long, objectdata As Object)

        Dim objtype As Type = objectdata.GetType
        Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
        For Each item As System.Reflection.PropertyInfo In properties
            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
            objaudittraildetail.FK_AuditTrailHeader_ID = PKAuditTrailHeaderid
            objaudittraildetail.FieldName = item.Name
            objaudittraildetail.OldValue = ""
            If Not item.GetValue(objectdata, Nothing) Is Nothing Then
                If item.GetValue(objectdata, Nothing).GetType.ToString <> "System.Byte[]" Then
                    objaudittraildetail.NewValue = item.GetValue(objectdata, Nothing)
                Else
                    objaudittraildetail.NewValue = ""
                End If
            Else
                objaudittraildetail.NewValue = ""
            End If
            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
        Next

    End Sub



    Public Shared Sub CreateAuditTrailDetailEdit(ByRef objdb As NawaDevDAL.NawaDatadevEntities, PKAuditTrailHeaderid As Long, objectdata As Object, objectdataold As Object)

        Dim objtype As Type = objectdata.GetType
        Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
        For Each item As System.Reflection.PropertyInfo In properties
            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
            objaudittraildetail.FK_AuditTrailHeader_ID = PKAuditTrailHeaderid
            objaudittraildetail.FieldName = item.Name

            If Not objectdataold Is Nothing Then


                If Not item.GetValue(objectdataold, Nothing) Is Nothing Then
                    If item.GetValue(objectdataold, Nothing).GetType.ToString <> "System.Byte[]" Then
                        objaudittraildetail.OldValue = item.GetValue(objectdataold, Nothing)
                    Else
                        objaudittraildetail.OldValue = ""
                    End If

                Else
                    objaudittraildetail.OldValue = ""
                End If
            Else
                objaudittraildetail.OldValue = ""
            End If

            If Not item.GetValue(objectdata, Nothing) Is Nothing Then
                If item.GetValue(objectdata, Nothing).GetType.ToString <> "System.Byte[]" Then
                    objaudittraildetail.NewValue = item.GetValue(objectdata, Nothing)
                Else
                    objaudittraildetail.NewValue = ""
                End If

            Else
                objaudittraildetail.NewValue = ""
            End If
            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
        Next

    End Sub
    Public Shared Sub CreateAuditTrailDetailEditAndActivation(ByRef objdb As NawaDevDAL.NawaDatadevEntities, PKAuditTrailHeaderid As Long, objectdata As Object, objectdataold As Object)

        Dim objtype As Type = objectdata.GetType
        Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
        For Each item As System.Reflection.PropertyInfo In properties
            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
            objaudittraildetail.FK_AuditTrailHeader_ID = PKAuditTrailHeaderid
            objaudittraildetail.FieldName = item.Name

            If Not objectdataold Is Nothing Then


                If Not item.GetValue(objectdataold, Nothing) Is Nothing Then
                    If item.GetValue(objectdataold, Nothing).GetType.ToString <> "System.Byte[]" Then
                        objaudittraildetail.OldValue = item.GetValue(objectdataold, Nothing)
                    Else
                        objaudittraildetail.OldValue = ""
                    End If

                Else
                    objaudittraildetail.OldValue = ""
                End If
            Else
                objaudittraildetail.OldValue = ""
            End If

            If Not item.GetValue(objectdata, Nothing) Is Nothing Then
                If item.GetValue(objectdata, Nothing).GetType.ToString <> "System.Byte[]" Then
                    objaudittraildetail.NewValue = item.GetValue(objectdata, Nothing)
                Else
                    objaudittraildetail.NewValue = ""
                End If

            Else
                objaudittraildetail.NewValue = ""
            End If
            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
        Next

    End Sub

    Public Shared Sub CreateAuditTrailDetailDelete(ByRef objdb As NawaDevDAL.NawaDatadevEntities, PKAuditTrailHeaderid As Long, objectdata As Object)

        Dim objtype As Type = objectdata.GetType
        Dim properties() As System.Reflection.PropertyInfo = objtype.GetProperties
        For Each item As System.Reflection.PropertyInfo In properties
            Dim objaudittraildetail As New NawaDevDAL.AuditTrailDetail
            objaudittraildetail.FK_AuditTrailHeader_ID = PKAuditTrailHeaderid
            objaudittraildetail.FieldName = item.Name
            If Not item.GetValue(objectdata, Nothing) Is Nothing Then
                If item.GetValue(objectdata, Nothing).GetType.ToString <> "System.Byte[]" Then
                    objaudittraildetail.OldValue = item.GetValue(objectdata, Nothing)
                Else
                    objaudittraildetail.OldValue = ""
                End If
            Else
                objaudittraildetail.OldValue = ""
            End If
            objaudittraildetail.NewValue = ""
            objdb.Entry(objaudittraildetail).State = Entity.EntityState.Added
        Next

    End Sub

    Public Shared Function ExecutePaging(strsql As String, strorder As String, intpage As Integer, intpagesize As Integer) As Data.DataTable

        Dim objsqlparameter(3) As SqlClient.SqlParameter
        objsqlparameter(0) = New SqlClient.SqlParameter
        objsqlparameter(1) = New SqlClient.SqlParameter
        objsqlparameter(2) = New SqlClient.SqlParameter
        objsqlparameter(3) = New SqlClient.SqlParameter

        objsqlparameter(0).ParameterName = "@querydata"
        objsqlparameter(0).Value = strsql


        objsqlparameter(1).ParameterName = "@orderby"
        objsqlparameter(1).Value = strorder


        objsqlparameter(2).ParameterName = "@PageNum"
        objsqlparameter(2).Value = intpage


        objsqlparameter(3).ParameterName = "@PageSize"
        objsqlparameter(3).Value = intpagesize




        Return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_PagingQuery", objsqlparameter)

    End Function

    Public Shared Function ConvertDataTable(Of T)(ByVal dt As DataTable) As List(Of T)
        Dim data As List(Of T) = New List(Of T)()

        For Each row As DataRow In dt.Rows
            Dim item As T = GetItem(Of T)(row)
            data.Add(item)
        Next

        Return data
    End Function

    Public Shared Function GetItem(Of T)(ByVal dr As DataRow) As T
        Dim temp As Type = GetType(T)
        Dim obj As T = Activator.CreateInstance(Of T)()

        For Each column As DataColumn In dr.Table.Columns

            For Each pro As PropertyInfo In temp.GetProperties()

                If pro.Name = column.ColumnName Then
                    If Not IsDBNull(dr(column.ColumnName)) Then
                        pro.SetValue(obj, dr(column.ColumnName), Nothing)
                    Else
                        pro.SetValue(obj, Nothing)
                    End If

                Else
                        Continue For
                End If
            Next
        Next

        Return obj
    End Function
    Public Shared Function ExtRadio(pn As Panel, strLabel As String, strFieldName As String, bRequired As Boolean, intgridpos As Integer, strTableRef As String, strFieldKey As String, strFieldDisplay As String, strFilterField As String) As Ext.Net.RadioGroup
        Dim objRadioGroup As New Ext.Net.RadioGroup
        objRadioGroup.ID = strFieldName
        objRadioGroup.ClientIDMode = Web.UI.ClientIDMode.Static
        '      If Not X.IsAjaxRequest Then
        objRadioGroup.FieldLabel = strLabel
        objRadioGroup.AnchorHorizontal = "40%"
        objRadioGroup.AllowBlank = Not bRequired
        objRadioGroup.Items.Clear()

        Using objdt As Data.DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, GetQueryRef(strTableRef, strFieldKey, strFieldDisplay, strFilterField), Nothing)
            For Each item As DataRow In objdt.Rows
                Using objradio As New Ext.Net.Radio
                    objradio.ID = strFieldName & "_" & item(1).ToString
                    objradio.InputValue = item(0).ToString

                    objradio.BoxLabel = item(1).ToString
                    objRadioGroup.Items.Add(objradio)
                End Using
            Next
        End Using
        'done: sampe sini cari cara bind radiogroup objRadioGroup.Items
        '  End If
        pn.Add(objRadioGroup)
    End Function


    Public Shared Function GetStringValue(strtable, strfield, strfilter) As Object
        Dim strquery As String = "select " & strfield & " from " & strtable & strfilter
        Dim strresult As Object = NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strquery, Nothing)

        If strresult Is DBNull.Value Then
            Return ""
        Else
            Return strresult
        End If
    End Function



    Public Shared Function convertInteger(intInteger As Object) As Integer

        If intInteger Is DBNull.Value Then
            Return 0
        End If

        Return intInteger

    End Function

    Public Shared Function extInfoPanelupdate(pn As FormPanel, validationResult As String, strinfopanelcontrol As String) As Ext.Net.InfoPanel

        Dim objInfopanel As Ext.Net.InfoPanel = pn.FindControl(strinfopanelcontrol)
        objInfopanel.Html = validationResult
        If objInfopanel.Html = "" Then
            objInfopanel.Hidden = True
        Else
            objInfopanel.Hidden = False
        End If

        objInfopanel.UpdateLayout()
        pn.UpdateLayout()


        Return objInfopanel


    End Function
    Public Shared Function extInfoPanel(pn As FormPanel, ValidationResult As String) As Ext.Net.InfoPanel
        If ValidationResult <> "" Then
            Dim objInfopanel As New Ext.Net.InfoPanel
            objInfopanel.ID = "infoPanelEdit"
            objInfopanel.ClientIDMode = Web.UI.ClientIDMode.Static
            If Not X.IsAjaxRequest Then
                objInfopanel.Visible = True
                objInfopanel.UI = UI.Warning
                objInfopanel.Title = "Validation Result"
                objInfopanel.IconCls = "#Error"
                objInfopanel.Margin = 10
                objInfopanel.Html = ValidationResult
                If objInfopanel.Html = "" Then
                    objInfopanel.Hidden = True
                Else
                    objInfopanel.Hidden = False
                End If
            End If

            pn.Insert(0, objInfopanel)

            objInfopanel.UpdateLayout()
            pn.UpdateLayout()

            Return objInfopanel
        End If

    End Function

    Public Function GetValidationResult(strTableValidation As String, strTableValidationPkName As String, strtableValidationpkvalue As String, strtablevalidationmsgfield As String) As String
        Dim strsql As String = "select [" & strtablevalidationmsgfield & "] from [" & strTableValidation & "] where [" & strTableValidationPkName & "] = '" & strtableValidationpkvalue.Replace("'", "''") & "'"
        Return NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strsql, Nothing)
    End Function

    Public Shared Function ExtGridPanel(pn As Panel, strLabel As String, objStore As Ext.Net.Store, objColumnModelBase As List(Of Ext.Net.ColumnBase), objdata As Object) As Ext.Net.GridPanel
        Dim objGridPanel As New Ext.Net.GridPanel
        Dim objpagingtoolbar As New Ext.Net.PagingToolbar
        Dim objFilterheader As New Ext.Net.FilterHeader
        objFilterheader.Remote = False
        objGridPanel.Title = strLabel
        objGridPanel.BottomBar.Add(objpagingtoolbar)
        objGridPanel.Plugins.Add(objFilterheader)
        objGridPanel.Store.Add(objStore)
        objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
        For Each item As ColumnBase In objColumnModelBase
            objGridPanel.ColumnModel.Columns.Add(item)
        Next
        pn.Add(objGridPanel)
        objStore.DataSource = objdata
        objStore.DataBind()

        Return objGridPanel
    End Function
    Public Shared Function GetPicker(cb As Ext.Net.DropDownField, strtableName As String, strFieldName As String, strfilter As String, strsort As String, jsscript As String, brender As Boolean, brefresh As Boolean, intwidth As Integer, intheight As Integer, Optional intMinwidth As Integer = 150)
        Dim objwindow As Ext.Net.Window
        Dim objGrid As Ext.Net.GridPanel
        Dim objStore As Ext.Net.Store
        Dim objModel As Ext.Net.Model
        Dim objModelfield As Ext.Net.ModelField

        If brender Then

            objwindow = cb.Component(0)
            objGrid = objwindow.Items(0)
            objStore = objGrid.GetStore
            objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
            objStore.RemoteFilter = True
            objStore.RemoteSort = True
            objModel = New Ext.Net.Model
            objModelfield = New Ext.Net.ModelField

            objStore.Model.Add(objModel)


        Else
            objwindow = cb.Component(0)
            objGrid = objwindow.Items(0)
            objStore = objGrid.GetStore

            If brefresh Then
                objStore.Model.Clear()
                objModel = New Ext.Net.Model
                objStore.Model.Add(objModel)
            Else
                objModel = objStore.Model(0)
            End If

        End If



        Dim datatable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select top 0 " & strFieldName & " from " & strtableName & " where 1=2", Nothing)

        For Each item As DataColumn In datatable.Columns



            Select Case item.DataType.ToString
                Case "System.Boolean"


                    objModelfield = New Ext.Net.ModelField
                    objModelfield.Name = item.ColumnName
                    objModelfield.Type = ModelFieldType.Boolean
                    objModel.Fields.Add(objModelfield)

                Case "System.DateTime"

                    objModelfield = New Ext.Net.ModelField
                    objModelfield.Name = item.ColumnName
                    objModelfield.Type = ModelFieldType.Date
                    objModel.Fields.Add(objModelfield)

                Case "System.Decimal"
                    objModelfield = New Ext.Net.ModelField
                    objModelfield.Name = item.ColumnName
                    objModelfield.Type = ModelFieldType.Float
                    objModel.Fields.Add(objModelfield)

                Case Else
                    objModelfield = New Ext.Net.ModelField
                    objModelfield.Name = item.ColumnName
                    objModelfield.Type = ModelFieldType.Auto

                    objModel.Fields.Add(objModelfield)
            End Select

        Next

        If brender Then

            objGrid.SelectionModel.Clear()
            objGrid.ColumnModel.Columns.Clear()
            objGrid.Plugins.Clear()

            Dim objRowSelection As New RowSelectionModel
            objRowSelection.Mode = SelectionMode.Single
            objRowSelection.Listeners.Select.Handler = jsscript
            objRowSelection.AllowDeselect = True
            objGrid.SelectionModel.Add(objRowSelection)

            Dim objfilterheader As New FilterHeader
            objfilterheader.Remote = True
            objGrid.Plugins.Add(objfilterheader)


            For Each item As Data.DataColumn In datatable.Columns
                If item.DataType.ToString = "System.Boolean" Then
                    Dim objcolumn As New Ext.Net.BooleanColumn
                    objcolumn.DataIndex = item.ColumnName
                    objcolumn.Text = item.ColumnName
                    objcolumn.Flex = 1
                    objcolumn.MinWidth = 150
                    objGrid.ColumnModel.Columns.Add(objcolumn)
                ElseIf item.DataType.ToString = "System.DateTime" Then
                    Dim objcolumn As New Ext.Net.DateColumn
                    objcolumn.DataIndex = item.ColumnName
                    objcolumn.Text = item.ColumnName
                    objcolumn.Format = NawaBLL.SystemParameterBLL.GetDateFormat
                    objcolumn.MinWidth = 150
                    objcolumn.Flex = 1
                    objGrid.ColumnModel.Columns.Add(objcolumn)

                ElseIf item.DataType.ToString = "System.Decimal" Or item.DataType.ToString = "System.Integer" Then
                    Dim objcolumn As New Ext.Net.NumberColumn
                    objcolumn.DataIndex = item.ColumnName
                    objcolumn.Text = item.ColumnName

                    objcolumn.MinWidth = 150
                    objcolumn.Flex = 1
                    objGrid.ColumnModel.Columns.Add(objcolumn)
                Else
                    Dim objcolumn As New Ext.Net.Column
                    objcolumn.DataIndex = item.ColumnName
                    objcolumn.Text = item.ColumnName
                    objcolumn.MinWidth = 150
                    objcolumn.Flex = 1
                    objGrid.ColumnModel.Columns.Add(objcolumn)
                End If

            Next


            objwindow.Width = intwidth
            cb.MinWidth = 300
            objwindow.Height = intheight


            'TryCast(Store.Proxy(0), PageProxy).Total = CInt(conn.GetDataTable("SELECT COUNT(*) " & sQuery.Substring(sQuery.LastIndexOf(" FROM ")) & cond, oParam).Rows(0)(0))

            Dim inttotal As Integer
            objStore.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging(strtableName, strFieldName, strfilter, strsort, 0, NawaBLL.SystemParameterBLL.GetPageSize, inttotal)
            objStore.DataBind()

        Else
            ''cb.Render()

        End If



    End Function

    Public Shared Function ObjGroupMenuAccess(roleid As Integer, moduleid As Integer) As List(Of NawaDAL.MGroupMenuAccess)
        Using objDb As NawaDAL.NawaDataEntities = New NawaDAL.NawaDataEntities
            Return objDb.MGroupMenuAccesses.Where(Function(x) x.FK_GroupMenu_ID = roleid And x.FK_Module_ID = moduleid).ToList
        End Using

    End Function
    Public Shared Function ExtCheckBox(pn As FieldSet, strLabel As String, strFieldName As String, bRequired As Boolean) As Ext.Net.Checkbox
        Dim objCheck As New Ext.Net.Checkbox
        objCheck.ID = strFieldName
        objCheck.ClientIDMode = Web.UI.ClientIDMode.Static
        If Not X.IsAjaxRequest Then
            objCheck.FieldLabel = strLabel
        End If
        pn.Add(objCheck)
        Return objCheck

    End Function
    Public Shared Function ExtNumber(pn As FieldSet, strLabel As String, strFieldName As String, bRequired As Boolean, intDecimalPrecition As Integer, dminvalue As Double, dmaxvalue As Double) As Ext.Net.NumberField
        Dim objNumberField As New Ext.Net.NumberField
        objNumberField.ID = strFieldName
        objNumberField.ClientIDMode = Web.UI.ClientIDMode.Static
        If Not X.IsAjaxRequest Then
            objNumberField.FieldLabel = strLabel
            objNumberField.LabelStyle = "word-wrap: break-word"
            objNumberField.LabelWidth = 100
            objNumberField.Name = strFieldName
            objNumberField.AllowBlank = Not bRequired
            objNumberField.BlankText = strLabel & " is required."
            objNumberField.DecimalPrecision = intDecimalPrecition
            objNumberField.MinValue = dminvalue
            objNumberField.MaxValue = dmaxvalue
            objNumberField.Width = objNumberField.LabelWidth + 150
            objNumberField.AnchorHorizontal = "40%"
        End If
        pn.Add(objNumberField)
        Return objNumberField
    End Function



    Public Shared Function ExtCombo(pn As FormPanel, strLabel As String, strFieldName As String, bRequired As Boolean, intgridpos As Integer, strTableRef As String, strFieldKey As String, strFieldDisplay As String, strFilterField As String, strTableRefAlias As String) As Ext.Net.ComboBox
        Using objcombo As New Ext.Net.ComboBox
            objcombo.ID = strFieldName
            objcombo.ClientIDMode = Web.UI.ClientIDMode.Static
            If Not Ext.Net.X.IsAjaxRequest Then
                objcombo.FieldLabel = strLabel

                objcombo.LabelWidth = 100
                objcombo.AnchorHorizontal = "80%"
                objcombo.Name = strFieldName
                objcombo.AllowBlank = Not bRequired
                objcombo.BlankText = strLabel & " is required."
                objcombo.Width = objcombo.LabelWidth + 150
                objcombo.MatchFieldWidth = True
                objcombo.MinChars = "0"
                objcombo.ForceSelection = True
                objcombo.AnyMatch = True

                objcombo.QueryMode = DataLoadMode.Remote
                objcombo.ValueField = strFieldKey
                objcombo.DisplayField = strFieldDisplay
                objcombo.TriggerAction = Ext.Net.TriggerAction.Query

                Dim objFieldtrigger As New Ext.Net.FieldTrigger
                objFieldtrigger.Icon = Ext.Net.TriggerIcon.Clear
                objFieldtrigger.Hidden = True
                objFieldtrigger.Weight = "-1"
                objcombo.Triggers.Add(objFieldtrigger)


                objcombo.Listeners.Select.Handler = "this.getTrigger(0).show();"

                objcombo.Listeners.TriggerClick.Handler = "if (index == 0) {  this.clearValue(); this.getTrigger(0).hide();}"



                'buat store dan modelnya

                Using objStore As New Ext.Net.Store
                    objStore.ID = "_Store_" + objcombo.ID
                    objStore.ClientIDMode = Web.UI.ClientIDMode.Static


                    Using objModel As New Ext.Net.Model
                        objModel.Fields.Add(strFieldKey, Ext.Net.ModelFieldType.String)
                        objModel.Fields.Add(strFieldDisplay, Ext.Net.ModelFieldType.String)
                        objStore.Model.Add(objModel)
                    End Using

                    objcombo.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                    objStore.IsPagingStore = True

                    objStore.Proxy.Add(New PageProxy)
                    AddHandler objStore.ReadData, AddressOf Callback

                    objcombo.Store.Add(objStore)

                    'objStore.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, System.Data.CommandType.Text, GetQueryRef(strTableRef & " " & strTableRefAlias, strFieldKey, strFieldDisplay, strFilterField), Nothing)
                    ' objStore.DataBind()
                End Using
            End If

            pn.Add(objcombo)
            Return objcombo
        End Using
    End Function
    Shared Sub Callback(sender As Object, e As StoreReadDataEventArgs)
        Dim query As String = e.Parameters("query")
        If query Is Nothing Then query = ""
        Dim strfilter As String = ""
        If query.Length > 0 Then
            strfilter = " ModuleActionName like '" & query & "%'"
        End If

        'StoreAction.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("ModuleAction", "PK_ModuleAction_ID,ModuleActionName", strfilter, "PK_ModuleAction_ID", e.Start, e.Limit, e.Total)
        'StoreAction.DataBind()
    End Sub
    Shared Function GetQueryRef(strTable As String, strfieldkey As String, strfielddisplay As String, strfilter As String) As String
        Dim strquery As String
        strquery = "select " & strfieldkey & ", convert(Varchar(1000),[" & strfieldkey & "])+ ' - '+ convert(varchar(1000), [" & strfielddisplay & "]) as [" & strfielddisplay & "] from " & strTable
        If strfilter.Trim.Length > 0 Then
            strquery = strquery & " where " & strfilter
        End If
        Return strquery
    End Function


    Shared Function HTMLEncode(ostr As String) As String

        Return System.Web.HttpUtility.HtmlEncode(ostr)

    End Function

    Shared Function HTMLDecode(ostr As String) As String

        Return System.Web.HttpUtility.HtmlDecode(ostr)

    End Function


    Public Shared Function ExtFileUpload(pn As FormPanel, strLabel As String, strFieldName As String, bRequired As Boolean, sizelimitMB As Integer) As Ext.Net.MultiUpload
        Dim objDateField As New Ext.Net.MultiUpload
        objDateField.ID = strFieldName
        objDateField.ClientIDMode = Web.UI.ClientIDMode.Static
        If Not X.IsAjaxRequest Then
            objDateField.FileDropAnywhere = True
            objDateField.AutoStartUpload = True
            objDateField.FileSizeLimit = sizelimitMB
            objDateField.FileTypes = "*.*"
            objDateField.FileUploadLimit = 100
            objDateField.FileQueueLimit = 0
            objDateField.Listeners.UploadStart.Handler = "Ext.Msg.wait('Uploading...');"
            objDateField.Listeners.UploadError.Fn = "uploadError"
            objDateField.Listeners.FileSelectionError.Fn = "fileSelectionError"


        End If
        pn.Add(objDateField)
        Return objDateField
    End Function

End Class
