
Partial Class NDSDropDownField
    Inherits System.Web.UI.UserControl

    Private m_label As String
    Private m_strfield As String
    Private m_strtablequery As String
    Private m_strfilter As String
    Private m_anchorlayout As String = "100%"
    Private m_allowblank As Boolean = True
    Private m_LabelAlign As LabelAlign
    Private m_LabelWidth As Integer
    Public Event OnValueChanged As System.EventHandler
    'Shadows Event OnValueChanged()

    Public Property StringValue() As String
        Get
            Return Session("ComboBox" + Me.ID + ".StringValue")
        End Get
        Set(ByVal value As String)
            Session("ComboBox" + Me.ID + ".StringValue") = value
        End Set

    End Property

    Public Property Label() As String
        Get
            Return m_label
        End Get
        Set(ByVal value As String)
            m_label = value
        End Set
    End Property

    Public Property StringField() As String
        Get
            Return m_strfield
        End Get
        Set(ByVal value As String)
            m_strfield = value
        End Set
    End Property

    Public Property StringTable() As String
        Get
            Return m_strtablequery
        End Get
        Set(ByVal value As String)
            m_strtablequery = value
        End Set
    End Property

    Public Property StringFilter() As String
        Get
            Return Session("ComboBox" + Me.ID + ".StringFilter")
        End Get
        Set(ByVal value As String)
            Session("ComboBox" + Me.ID + ".StringFilter") = value
        End Set

    End Property
    Public Property AnchorHorizontal() As String
        Get
            Return m_anchorlayout
        End Get
        Set(ByVal value As String)
            m_anchorlayout = value
        End Set
    End Property
    Public Property LabelAlign() As LabelAlign
        Get
            Return m_LabelAlign
        End Get
        Set(ByVal value As LabelAlign)
            m_LabelAlign = value
        End Set
    End Property

    Public Property LabelWidth() As Integer
        Get
            Return m_LabelWidth
        End Get
        Set(ByVal value As Integer)
            m_LabelWidth = value
        End Set
    End Property

    Public Property AllowBlank() As String
        Get
            Return m_allowblank
        End Get
        Set(ByVal value As String)
            m_allowblank = value
        End Set
    End Property

    Public ReadOnly Property TextValue() As String
        Get
            'If Not String.IsNullOrEmpty(ComboBox1.Text) And ComboBox1.Text.Contains("-") Then
            '    Return ComboBox1.Text.Substring(0, ComboBox1.Text.IndexOf("-"))
            'Else
            '    Return ""
            'End If

            '-- SCB doesn't want to show the ID, just the name in combobox text
            If Not String.IsNullOrEmpty(ComboBox1.Value) Then
                Return StringValue
            Else
                Return Nothing
            End If

        End Get
    End Property

    Public ReadOnly Property TextRawValue() As String
        Get
            'Return ComboBox1.Text
            Return StringValue
        End Get
    End Property

    Dim objNawaDataPicker As NawaDevBLL.NawaDataPicker = New NawaDevBLL.NawaDataPicker


    Protected Sub storePicker_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            Dim intStart As Integer = e.Start
            Dim intLimit As Int16 = e.Limit
            Dim inttotalRecord As Integer
            Dim strfilter As String = NawaBLL.Nawa.BLL.NawaFramework.GetWhereClauseHeader(e)
            Dim strsort As String = ""
            For Each item As DataSorter In e.Sort
                strsort += item.Property & " " & item.Direction.ToString
            Next
            'Dim objStoredata As Ext.Net.Store = CType(sender, Ext.Net.Store)

            objNawaDataPicker.GetPickerNew(ComboBox1, StringTable, StringField, "", "", "GridClick(#{ComboBox1},#{GridPanel},#{Window});", True, True, 600, 300)

            If Ext.Net.X.IsAjaxRequest Then
                ComboBox1.Show()
            End If

            'Tambahan untuk filter kolom yang mengandung spasi
            If Not (String.IsNullOrEmpty(strfilter) Or strfilter = "") Then
                Dim posisiLike As Int16 = InStr(strfilter, "like")
                Dim fieldFilter As String = Left(strfilter, posisiLike - 1)

                If fieldFilter.Contains(" ") Then
                    fieldFilter = "[" + Left(fieldFilter.Trim(), posisiLike - 1) + "]"
                    Dim fields As String() = StringField.Split(",")
                    Dim field As String

                    For Each field In fields
                        If InStr(field, fieldFilter) > 0 Then
                            Dim posisiAS As Int16 = InStr(field, "AS")
                            fieldFilter = Left(field, posisiAS - 1)
                            Ext.Net.X.Js.Call("console.log", fieldFilter)
                        End If
                    Next

                    strfilter = " " + fieldFilter + " " + Mid(strfilter, posisiLike, Len(strfilter) - posisiLike)
                End If

                'Return
            End If

            Dim realFilter As String = StringFilter

            If String.IsNullOrEmpty(StringFilter) Then
                realFilter = strfilter
            ElseIf Not String.IsNullOrEmpty(strfilter) Then
                realFilter = StringFilter & " AND " & strfilter
            End If

            'StringFilter = strfilter
            Dim DataPaging As Data.DataTable = NawaDAL.SQLHelper.ExecuteTabelPaging(StringTable, StringField, realFilter, strsort, intStart, intLimit, inttotalRecord)
            Dim limit As Integer = e.Limit
            If (e.Start + e.Limit) > inttotalRecord Then
                limit = inttotalRecord - e.Start
            End If

            e.Total = inttotalRecord
            store.DataSource = DataPaging
            store.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Private Sub NDSDropDownField_Load(sender As Object, e As EventArgs) Handles Me.Load
        ComboBox1.AnchorHorizontal = AnchorHorizontal
        If Not Ext.Net.X.IsAjaxRequest Then
            objNawaDataPicker.GetPickerNew(ComboBox1, StringTable, StringField, "", "", "GridClick(#{ComboBox1},#{GridPanel},#{Window});", True, True, 600, 300)
            ComboBox1.FieldLabel = Label
            ComboBox1.AllowBlank = AllowBlank
            ComboBox1.LabelAlign = LabelAlign
            ComboBox1.LabelWidth = LabelWidth
            Window.Title = "List of " & Me.Label
        End If
    End Sub

    Public Sub SetTextValue(values As String)
        'ComboBox1.Text = values

        If Not String.IsNullOrEmpty(values) And values.Contains("-") Then
            ComboBox1.Value = values.Substring(values.IndexOf("-") + 1, Len(values) - values.IndexOf("-") - 1)
            StringValue = values.Substring(0, values.IndexOf("-"))
        Else
            ComboBox1.Value = Nothing
            StringValue = Nothing
        End If
    End Sub

    Public Sub GridPanel_OnClick(sender As Object, e As DirectEventArgs)
        Dim ID As String = JSON.Deserialize(Of Dictionary(Of String, String))(e.ExtraParams(0).Value).ElementAt(0).Value
        StringValue = ID

        RaiseEvent OnValueChanged(sender, e)
    End Sub

    Public Sub RefreshData(value As String)
        StringFilter = value
    End Sub

End Class
