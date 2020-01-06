Imports Ext.Net
Public Class NawaDataPicker

    Private _dropdown As Ext.Net.DropDownField
    Public Property cbodropdown() As Ext.Net.DropDownField
        Get
            Return _dropdown
        End Get
        Set(ByVal value As Ext.Net.DropDownField)
            _dropdown = value
        End Set
    End Property

    Private _tablename As String
    Public Property TableName() As String
        Get
            Return _tablename
        End Get
        Set(ByVal value As String)
            _tablename = value
        End Set
    End Property

    Private _fieldname As String
    Public Property FieldName() As String
        Get
            Return _fieldname
        End Get
        Set(ByVal value As String)
            _fieldname = value
        End Set
    End Property

    Private _script As String
    Public Property Script() As String
        Get
            Return _script
        End Get
        Set(ByVal value As String)
            _script = value
        End Set
    End Property

    Public Function GetPickerNew(cb As Ext.Net.DropDownField, strtableName As String, strFieldName As String, strfilter As String, strsort As String, jsscript As String, brender As Boolean, brefresh As Boolean, intwidth As Integer, intheight As Integer, Optional intMinwidth As Integer = 150)

        Dim objGrid As Ext.Net.GridPanel
        Dim objWindow As Ext.Net.Window
        Dim objStore As Ext.Net.Store
        Dim objModel As Ext.Net.Model
        Dim objModelfield As Ext.Net.ModelField
        Me.TableName = strtableName
        Me.FieldName = strFieldName
        Me.Script = jsscript


        If brender Then
            objWindow = cb.Component(0)
            objGrid = objWindow.Items(0)

            objStore = objGrid.GetStore
            objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
            objStore.RemoteFilter = True
            objStore.RemoteSort = True
            objModel = New Ext.Net.Model
            objModelfield = New Ext.Net.ModelField

            objStore.Model.Add(objModel)


        Else


            objWindow = cb.Component(0)
            objGrid = objWindow.Items(0)


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


            objGrid.Width = intwidth
            cb.MinWidth = 300
            objGrid.Height = intheight
            'objGrid.UpdateContent()


            'TryCast(Store.Proxy(0), PageProxy).Total = CInt(conn.GetDataTable("SELECT COUNT(*) " & sQuery.Substring(sQuery.LastIndexOf(" FROM ")) & cond, oParam).Rows(0)(0))

            Dim inttotal As Integer
            objStore.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging(strtableName, strFieldName, strfilter, strsort, 0, NawaBLL.SystemParameterBLL.GetPageSize, inttotal)
            objStore.DataBind()

        Else
            ''cb.Render()

        End If

        Me.cbodropdown = cb

    End Function
End Class
