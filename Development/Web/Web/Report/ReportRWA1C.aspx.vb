Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports RWADAL
Imports Ext.Net
Imports Microsoft.Reporting.WebForms

Imports System.Data
Imports System.Data.SqlClient
Partial Class Report_ReportRWA1C
    Inherits Parent


#Region "Variables"
    Enum NamaJenisReport
        EksposurAset = 1
        EksposurKewajiban = 2
        EksposurCounterparty = 3
        EksposurSettlementRisk = 4
        EksposurSekuritisasi = 5
        EksposurSyariah = 6
    End Enum

    Dim _JenisReportNumber As Integer
    Dim _JenisTagihanNumber As Integer
    Dim _ChildNumber As Char
    Dim _ChildChildNumber As Integer
    Dim _KategoriPortofolio As New StringBuilder
#End Region

#Region "Entities"
    Private _MsJenisTagihanReportDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportDataTable
    Private Property MsJenisTagihanReportDatatable() As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportDataTable
        Get
            If _MsJenisTagihanReportDataTable Is Nothing Then
                If Session("MsJenisTagihanReportDatatable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.MsJenisTagihanReportTableAdapter
                        Session("MsJenisTagihanReportDatatable") = adapter.GetData()
                        _MsJenisTagihanReportDataTable = Session("MsJenisTagihanReportDatatable")
                        Return _MsJenisTagihanReportDataTable
                    End Using
                Else
                    Return CType(Session("MsJenisTagihanReportDatatable"), RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportDataTable)
                End If
            Else
                Return _MsJenisTagihanReportDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportDataTable)
            _MsJenisTagihanReportDataTable = value
            Session("MsJenisTagihanReportDatatable") = _MsJenisTagihanReportDataTable
        End Set
    End Property

    Private ReadOnly Property ProcessMonth() As Integer
        Get
            Dim IntReturn As Integer = 0
            IntReturn = Month(DatTanggalData.SelectedValue)
            Return IntReturn
        End Get
    End Property

    Private ReadOnly Property ProcessYear() As Integer
        Get
            Dim IntReturn As Integer = 0
            IntReturn = Year(DatTanggalData.SelectedValue)
            Return IntReturn
        End Get
    End Property

    REM Eksposur Aset

    Private _ReportBI_1C_AsetDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetDataTable
    Private Property ReportBI_1C_AsetDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetDataTable
        Get
            If _ReportBI_1C_AsetDataTable Is Nothing Then
                If Session("ReportBI_1C_AsetDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_AsetTableAdapter
                        Session("ReportBI_1C_AsetDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_AsetDataTable = Session("ReportBI_1C_AsetDataTable")
                        Return _ReportBI_1C_AsetDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_AsetDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetDataTable)
                End If
            Else
                Return _ReportBI_1C_AsetDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetDataTable)
            _ReportBI_1C_AsetDataTable = value
            Session("ReportBI_1C_AsetDataTable") = _ReportBI_1C_AsetDataTable
        End Set
    End Property

    Private _ReportBI_1C_KewajibanDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanDataTable
    Private Property ReportBI_1C_KewajibanDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanDataTable
        Get
            If _ReportBI_1C_KewajibanDataTable Is Nothing Then
                If Session("ReportBI_1C_KewajibanDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_KewajibanTableAdapter
                        Session("ReportBI_1C_KewajibanDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_KewajibanDataTable = Session("ReportBI_1C_KewajibanDataTable")
                        Return _ReportBI_1C_KewajibanDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_KewajibanDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanDataTable)
                End If
            Else
                Return _ReportBI_1C_KewajibanDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanDataTable)
            _ReportBI_1C_KewajibanDataTable = value
            Session("ReportBI_1C_KewajibanDataTable") = _ReportBI_1C_KewajibanDataTable
        End Set
    End Property

    Private _ReportBI_1C_CounterpartyDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyDataTable
    Private Property ReportBI_1C_CounterpartyDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyDataTable
        Get
            If _ReportBI_1C_CounterpartyDataTable Is Nothing Then
                If Session("ReportBI_1C_CounterpartyDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_CounterpartyTableAdapter
                        Session("ReportBI_1C_CounterpartyDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_CounterpartyDataTable = Session("ReportBI_1C_CounterpartyDataTable")
                        Return _ReportBI_1C_CounterpartyDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_CounterpartyDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyDataTable)
                End If
            Else
                Return _ReportBI_1C_CounterpartyDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyDataTable)
            _ReportBI_1C_CounterpartyDataTable = value
            Session("ReportBI_1C_CounterpartyDataTable") = _ReportBI_1C_CounterpartyDataTable
        End Set
    End Property

    Private _ReportBI_1C_SettlementDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementDataTable
    Private Property ReportBI_1C_SettlementDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementDataTable
        Get
            If _ReportBI_1C_SettlementDataTable Is Nothing Then
                If Session("ReportBI_1C_SettlementDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_SettlementTableAdapter
                        Session("ReportBI_1C_SettlementDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_SettlementDataTable = Session("ReportBI_1C_SettlementDataTable")
                        Return _ReportBI_1C_SettlementDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_SettlementDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementDataTable)
                End If
            Else
                Return _ReportBI_1C_SettlementDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementDataTable)
            _ReportBI_1C_SettlementDataTable = value
            Session("ReportBI_1C_SettlementDataTable") = _ReportBI_1C_SettlementDataTable
        End Set
    End Property

    Private _ReportBI_1C_SekuritisasiDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiDataTable
    Private Property ReportBI_1C_SekuritisasiDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiDataTable
        Get
            If _ReportBI_1C_SekuritisasiDataTable Is Nothing Then
                If Session("ReportBI_1C_SekuritisasiDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_SekuritisasiTableAdapter
                        Session("ReportBI_1C_SekuritisasiDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_SekuritisasiDataTable = Session("ReportBI_1C_SekuritisasiDataTable")
                        Return _ReportBI_1C_SekuritisasiDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_SekuritisasiDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiDataTable)
                End If
            Else
                Return _ReportBI_1C_SekuritisasiDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiDataTable)
            _ReportBI_1C_SekuritisasiDataTable = value
            Session("ReportBI_1C_SekuritisasiDataTable") = _ReportBI_1C_SekuritisasiDataTable
        End Set
    End Property

    Private _ReportBI_1C_SyariahDataTable As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SyariahDataTable
    Private Property ReportBI_1C_SyariahDataTable() As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SyariahDataTable
        Get
            If _ReportBI_1C_SyariahDataTable Is Nothing Then
                If Session("ReportBI_1C_SyariahDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualRekapitulasiTableAdapters.ReportBI_1C_SyariahTableAdapter
                        Session("ReportBI_1C_SyariahDataTable") = adapter.GetData(ProcessMonth, ProcessYear)
                        _ReportBI_1C_SyariahDataTable = Session("ReportBI_1C_SyariahDataTable")
                        Return _ReportBI_1C_SyariahDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1C_SyariahDataTable"), RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SyariahDataTable)
                End If
            Else
                Return _ReportBI_1C_SyariahDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SyariahDataTable)
            _ReportBI_1C_SyariahDataTable = value
            Session("ReportBI_1C_SyariahDataTable") = _ReportBI_1C_SyariahDataTable
        End Set
    End Property

    Private _EksposurAsetDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurAsetDataTable
    Private _EksposurKewajibanDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurKewajibanDataTable
    Private _EksposurCounterpartyDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurCounterpartyDataTable
    Private _EksposurSettlementDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSettlementDataTable
    Private _EksposurSekuritisasiDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSekuritisasiDataTable
    Private _EksposurSyariahDataTable As New RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSyariahDataTable
#End Region

#Region "Clear Session"
    Private Sub ClearSession()
        MsJenisTagihanReportDatatable = Nothing

        ReportBI_1C_AsetDataTable = Nothing
        ReportBI_1C_KewajibanDataTable = Nothing
        ReportBI_1C_CounterpartyDataTable = Nothing
        ReportBI_1C_SettlementDataTable = Nothing
        ReportBI_1C_SekuritisasiDataTable = Nothing
        ReportBI_1C_SyariahDataTable = Nothing
    End Sub
#End Region

    Dim ModuleList As List(Of NawaDAL.Module) = NawaBLL.ModuleBLL.GetModuleAll

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("GenerateTextFileView.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("GenerateTextFileView.ObjModule") = value
        End Set
    End Property

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Dim strmodule As String = Request.Params("ModuleID")
                ClearSession()

                Try
                    Dim intmodule As Integer = NawaBLL.Common.DecryptQueryString(strmodule, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Me.ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intmodule)


                    If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
                        Dim strIDCode As String = 1
                        strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                        Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                        Exit Sub
                    End If

                    FormPanelInput.Title = ObjModule.ModuleLabel
                    PanelReport.Hidden = True
                Catch ex As Exception
                    Throw New Exception("Invalid Module ID")
                End Try
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub Generate()
        Try
            ClearSession()

            ReportViewer1.LocalReport.DataSources.Clear()
            Me.ReportViewer1.Visible = True
            PanelReport.Hidden = False

            GenerateReportEksposurAset()
            GenerateReportEksposurKewajiban()
            GenerateReportEksposurCounterparty()
            GenerateReportEksposurSettlement()
            GenerateReportEksposurSekuritisasi()
            GenerateReportEksposurSyariah()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Private Sub GenerateReportEksposurAset()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurAset
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            Dim _ChildChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()

            Dim _ReportBI_1C_AsetRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetRow()
            Dim _EksposurAsetRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurAsetRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                    _EksposurAsetRow.No = _JenisTagihanNumber & "."

                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                    _KategoriPortofolio.Append(_MsJenisTagihanReportRow.NamaJenisTagihanReport)
                    _EksposurAsetRow.KategoriPortofolio = _KategoriPortofolio.ToString

                    _ReportBI_1C_AsetRowList = Me.ReportBI_1C_AsetDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ReportBI_1C_AsetRowList.Length > 0 Then
                        For Each _ReportBI_1C_AsetRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetRow In _ReportBI_1C_AsetRowList
                            _EksposurAsetRow.TagihanBersih = _ReportBI_1C_AsetRow.TotalTagihanBersih
                            _EksposurAsetRow.ATMRSebelumMRK = _ReportBI_1C_AsetRow.TotalATMRSebelumMRK
                            _EksposurAsetRow.ATMRSetelahMRK = _ReportBI_1C_AsetRow.TotalATMRSetelahMRK
                        Next
                    Else
                        _EksposurAsetRow.TagihanBersih = 0
                        _EksposurAsetRow.ATMRSebelumMRK = 0
                        _EksposurAsetRow.ATMRSetelahMRK = 0

                        '_IsReportDisplayed = False
                    End If

                    _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then
                        _ChildNumber = "a"

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                            _EksposurAsetRow.No = ""

                            _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                            _KategoriPortofolio.Append("    " & _ChildNumber & ". " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                            _EksposurAsetRow.KategoriPortofolio = _KategoriPortofolio.ToString

                            _ReportBI_1C_AsetRowList = Me.ReportBI_1C_AsetDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ReportBI_1C_AsetRowList.Length > 0 Then
                                For Each _ReportBI_1C_AsetRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetRow In _ReportBI_1C_AsetRowList
                                    _EksposurAsetRow.TagihanBersih = _ReportBI_1C_AsetRow.TotalTagihanBersih
                                    _EksposurAsetRow.ATMRSebelumMRK = _ReportBI_1C_AsetRow.TotalATMRSebelumMRK
                                    _EksposurAsetRow.ATMRSetelahMRK = _ReportBI_1C_AsetRow.TotalATMRSetelahMRK
                                Next
                            Else
                                _EksposurAsetRow.TagihanBersih = 0
                                _EksposurAsetRow.ATMRSebelumMRK = 0
                                _EksposurAsetRow.ATMRSetelahMRK = 0

                                '_IsReportDisplayed = False
                            End If

                            _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)

                            _ChildChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                                                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                                                    "AND FK_MsJenisTagihanReportParentId=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ChildChildMsJenisTagihanReportRowList.Length > 0 Then
                                _ChildChildNumber = 1

                                For Each _ChildChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildChildMsJenisTagihanReportRowList
                                    _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                    _EksposurAsetRow.No = ""

                                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                                    _KategoriPortofolio.Append("         " & _ChildChildNumber & ") " & _ChildChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                                    _EksposurAsetRow.KategoriPortofolio = _KategoriPortofolio.ToString

                                    _ReportBI_1C_AsetRowList = Me.ReportBI_1C_AsetDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    If _ReportBI_1C_AsetRowList.Length > 0 Then
                                        For Each _ReportBI_1C_AsetRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_AsetRow In _ReportBI_1C_AsetRowList
                                            _EksposurAsetRow.TagihanBersih = _ReportBI_1C_AsetRow.TotalTagihanBersih
                                            _EksposurAsetRow.ATMRSebelumMRK = _ReportBI_1C_AsetRow.TotalATMRSebelumMRK
                                            _EksposurAsetRow.ATMRSetelahMRK = _ReportBI_1C_AsetRow.TotalATMRSetelahMRK
                                        Next
                                    Else
                                        _EksposurAsetRow.TagihanBersih = 0
                                        _EksposurAsetRow.ATMRSebelumMRK = 0
                                        _EksposurAsetRow.ATMRSetelahMRK = 0

                                        '_IsReportDisplayed = False
                                    End If

                                    _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)


                                    _ChildChildNumber += 1
                                Next
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurAset", CType(_EksposurAsetDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurAset Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurKewajiban()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurKewajiban
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            Dim _ChildChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()

            Dim _ReportBI_1C_KewajibanRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanRow()
            Dim _EksposurKewajibanRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurKewajibanRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                    _EksposurKewajibanRow.No = _JenisTagihanNumber & "."

                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                    _KategoriPortofolio.Append(_MsJenisTagihanReportRow.NamaJenisTagihanReport)
                    _EksposurKewajibanRow.KategoriPortofolio = _KategoriPortofolio.ToString

                    _ReportBI_1C_KewajibanRowList = Me.ReportBI_1C_KewajibanDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ReportBI_1C_KewajibanRowList.Length > 0 Then
                        For Each _ReportBI_1C_KewajibanRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanRow In _ReportBI_1C_KewajibanRowList
                            _EksposurKewajibanRow.TagihanBersih = _ReportBI_1C_KewajibanRow.TotalTagihanBersih
                            _EksposurKewajibanRow.ATMRSebelumMRK = _ReportBI_1C_KewajibanRow.TotalATMRSebelumMRK
                            _EksposurKewajibanRow.ATMRSetelahMRK = _ReportBI_1C_KewajibanRow.TotalATMRSetelahMRK
                        Next
                    Else
                        _EksposurKewajibanRow.TagihanBersih = 0
                        _EksposurKewajibanRow.ATMRSebelumMRK = 0
                        _EksposurKewajibanRow.ATMRSetelahMRK = 0

                        '_IsReportDisplayed = False
                    End If

                    _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then
                        _ChildNumber = "a"

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                            _EksposurKewajibanRow.No = ""

                            _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                            _KategoriPortofolio.Append("    " & _ChildNumber & ". " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                            _EksposurKewajibanRow.KategoriPortofolio = _KategoriPortofolio.ToString

                            _ReportBI_1C_KewajibanRowList = Me.ReportBI_1C_KewajibanDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ReportBI_1C_KewajibanRowList.Length > 0 Then
                                For Each _ReportBI_1C_KewajibanRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanRow In _ReportBI_1C_KewajibanRowList
                                    _EksposurKewajibanRow.TagihanBersih = _ReportBI_1C_KewajibanRow.TotalTagihanBersih
                                    _EksposurKewajibanRow.ATMRSebelumMRK = _ReportBI_1C_KewajibanRow.TotalATMRSebelumMRK
                                    _EksposurKewajibanRow.ATMRSetelahMRK = _ReportBI_1C_KewajibanRow.TotalATMRSetelahMRK
                                Next
                            Else
                                _EksposurKewajibanRow.TagihanBersih = 0
                                _EksposurKewajibanRow.ATMRSebelumMRK = 0
                                _EksposurKewajibanRow.ATMRSetelahMRK = 0

                                '_IsReportDisplayed = False
                            End If

                            _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)

                            _ChildChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                                                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                                                    "AND FK_MsJenisTagihanReportParentId=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ChildChildMsJenisTagihanReportRowList.Length > 0 Then
                                _ChildChildNumber = 1

                                For Each _ChildChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildChildMsJenisTagihanReportRowList
                                    _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                    _EksposurKewajibanRow.No = ""

                                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                                    _KategoriPortofolio.Append("         " & _ChildChildNumber & ") " & _ChildChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                                    _EksposurKewajibanRow.KategoriPortofolio = _KategoriPortofolio.ToString

                                    _ReportBI_1C_KewajibanRowList = Me.ReportBI_1C_KewajibanDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    If _ReportBI_1C_KewajibanRowList.Length > 0 Then
                                        For Each _ReportBI_1C_KewajibanRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_KewajibanRow In _ReportBI_1C_KewajibanRowList
                                            _EksposurKewajibanRow.TagihanBersih = _ReportBI_1C_KewajibanRow.TotalTagihanBersih
                                            _EksposurKewajibanRow.ATMRSebelumMRK = _ReportBI_1C_KewajibanRow.TotalATMRSebelumMRK
                                            _EksposurKewajibanRow.ATMRSetelahMRK = _ReportBI_1C_KewajibanRow.TotalATMRSetelahMRK
                                        Next
                                    Else
                                        _EksposurKewajibanRow.TagihanBersih = 0
                                        _EksposurKewajibanRow.ATMRSebelumMRK = 0
                                        _EksposurKewajibanRow.ATMRSetelahMRK = 0

                                        '_IsReportDisplayed = False
                                    End If

                                    _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)


                                    _ChildChildNumber += 1
                                Next
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurKewajiban", CType(_EksposurKewajibanDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurKewajiban Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurCounterparty()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurCounterparty
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            Dim _ChildChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()

            Dim _ReportBI_1C_CounterpartyRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyRow()
            Dim _EksposurCounterpartyRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurCounterpartyRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                    _EksposurCounterpartyRow.No = _JenisTagihanNumber & "."

                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                    _KategoriPortofolio.Append(_MsJenisTagihanReportRow.NamaJenisTagihanReport)
                    _EksposurCounterpartyRow.KategoriPortofolio = _KategoriPortofolio.ToString

                    _ReportBI_1C_CounterpartyRowList = Me.ReportBI_1C_CounterpartyDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ReportBI_1C_CounterpartyRowList.Length > 0 Then
                        For Each _ReportBI_1C_CounterpartyRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyRow In _ReportBI_1C_CounterpartyRowList
                            _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1C_CounterpartyRow.TotalTagihanBersih
                            _EksposurCounterpartyRow.ATMRSebelumMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSebelumMRK
                            _EksposurCounterpartyRow.ATMRSetelahMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSetelahMRK
                        Next
                    Else
                        _EksposurCounterpartyRow.TagihanBersih = 0
                        _EksposurCounterpartyRow.ATMRSebelumMRK = 0
                        _EksposurCounterpartyRow.ATMRSetelahMRK = 0

                        '_IsReportDisplayed = False
                    End If

                    _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then
                        _ChildNumber = "a"

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                            _EksposurCounterpartyRow.No = ""

                            _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                            _KategoriPortofolio.Append("    " & _ChildNumber & ". " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                            _EksposurCounterpartyRow.KategoriPortofolio = _KategoriPortofolio.ToString

                            _ReportBI_1C_CounterpartyRowList = Me.ReportBI_1C_CounterpartyDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ReportBI_1C_CounterpartyRowList.Length > 0 Then
                                For Each _ReportBI_1C_CounterpartyRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyRow In _ReportBI_1C_CounterpartyRowList
                                    _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1C_CounterpartyRow.TotalTagihanBersih
                                    _EksposurCounterpartyRow.ATMRSebelumMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSebelumMRK
                                    _EksposurCounterpartyRow.ATMRSetelahMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSetelahMRK
                                Next
                            Else
                                _EksposurCounterpartyRow.TagihanBersih = 0
                                _EksposurCounterpartyRow.ATMRSebelumMRK = 0
                                _EksposurCounterpartyRow.ATMRSetelahMRK = 0

                                '_IsReportDisplayed = False
                            End If

                            _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)

                            _ChildChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                                                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                                                    "AND FK_MsJenisTagihanReportParentId=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ChildChildMsJenisTagihanReportRowList.Length > 0 Then
                                _ChildChildNumber = 1

                                For Each _ChildChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildChildMsJenisTagihanReportRowList
                                    _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                    _EksposurCounterpartyRow.No = ""

                                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                                    _KategoriPortofolio.Append("         " & _ChildChildNumber & ") " & _ChildChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                                    _EksposurCounterpartyRow.KategoriPortofolio = _KategoriPortofolio.ToString

                                    _ReportBI_1C_CounterpartyRowList = Me.ReportBI_1C_CounterpartyDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    If _ReportBI_1C_CounterpartyRowList.Length > 0 Then
                                        For Each _ReportBI_1C_CounterpartyRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_CounterpartyRow In _ReportBI_1C_CounterpartyRowList
                                            _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1C_CounterpartyRow.TotalTagihanBersih
                                            _EksposurCounterpartyRow.ATMRSebelumMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSebelumMRK
                                            _EksposurCounterpartyRow.ATMRSetelahMRK = _ReportBI_1C_CounterpartyRow.TotalATMRSetelahMRK
                                        Next
                                    Else
                                        _EksposurCounterpartyRow.TagihanBersih = 0
                                        _EksposurCounterpartyRow.ATMRSebelumMRK = 0
                                        _EksposurCounterpartyRow.ATMRSetelahMRK = 0

                                        '_IsReportDisplayed = False
                                    End If

                                    _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)


                                    _ChildChildNumber += 1
                                Next
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurCounterparty", CType(_EksposurCounterpartyDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurCounterparty Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurSettlement()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurSettlementRisk
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            Dim _ChildChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()

            Dim _ReportBI_1C_SettlementRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementRow()
            Dim _EksposurSettlementRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSettlementRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    _EksposurSettlementRow = _EksposurSettlementDataTable.NewRow

                    _EksposurSettlementRow.No = _JenisTagihanNumber & "."

                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                    _KategoriPortofolio.Append(_MsJenisTagihanReportRow.NamaJenisTagihanReport)
                    _EksposurSettlementRow.JenisTransaksi = _KategoriPortofolio.ToString

                    _ReportBI_1C_SettlementRowList = Me.ReportBI_1C_SettlementDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ReportBI_1C_SettlementRowList.Length > 0 Then
                        For Each _ReportBI_1C_SettlementRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementRow In _ReportBI_1C_SettlementRowList
                            _EksposurSettlementRow.NilaiEksposur = _ReportBI_1C_SettlementRow.TotalNilaiEksposur
                            _EksposurSettlementRow.FaktorPengurangModal = _ReportBI_1C_SettlementRow.TotalFaktorPengurangModal
                            _EksposurSettlementRow.ATMR = _ReportBI_1C_SettlementRow.TotalATMR
                        Next
                    Else
                        _EksposurSettlementRow.NilaiEksposur = 0
                        _EksposurSettlementRow.FaktorPengurangModal = 0
                        _EksposurSettlementRow.ATMR = 0

                        '_IsReportDisplayed = False
                    End If

                    _EksposurSettlementDataTable.AddEksposurSettlementRow(_EksposurSettlementRow)

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then
                        _ChildNumber = "a"

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _EksposurSettlementRow = _EksposurSettlementDataTable.NewRow

                            _EksposurSettlementRow.No = ""

                            _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                            _KategoriPortofolio.Append("    " & _ChildNumber & ". " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                            _EksposurSettlementRow.JenisTransaksi = _KategoriPortofolio.ToString

                            _ReportBI_1C_SettlementRowList = Me.ReportBI_1C_SettlementDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ReportBI_1C_SettlementRowList.Length > 0 Then
                                For Each _ReportBI_1C_SettlementRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementRow In _ReportBI_1C_SettlementRowList
                                    _EksposurSettlementRow.NilaiEksposur = _ReportBI_1C_SettlementRow.TotalNilaiEksposur
                                    _EksposurSettlementRow.FaktorPengurangModal = _ReportBI_1C_SettlementRow.TotalFaktorPengurangModal
                                    _EksposurSettlementRow.ATMR = _ReportBI_1C_SettlementRow.TotalATMR
                                Next
                            Else
                                _EksposurSettlementRow.NilaiEksposur = 0
                                _EksposurSettlementRow.FaktorPengurangModal = 0
                                _EksposurSettlementRow.ATMR = 0

                                '_IsReportDisplayed = False
                            End If

                            _EksposurSettlementDataTable.AddEksposurSettlementRow(_EksposurSettlementRow)

                            _ChildChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                                                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                                                    "AND FK_MsJenisTagihanReportParentId=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ChildChildMsJenisTagihanReportRowList.Length > 0 Then
                                _ChildChildNumber = 1

                                For Each _ChildChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildChildMsJenisTagihanReportRowList
                                    _EksposurSettlementRow = _EksposurSettlementDataTable.NewRow

                                    _EksposurSettlementRow.No = ""

                                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                                    _KategoriPortofolio.Append("         " & _ChildChildNumber & ") " & _ChildChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                                    _EksposurSettlementRow.JenisTransaksi = _KategoriPortofolio.ToString

                                    _ReportBI_1C_SettlementRowList = Me.ReportBI_1C_SettlementDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    If _ReportBI_1C_SettlementRowList.Length > 0 Then
                                        For Each _ReportBI_1C_SettlementRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SettlementRow In _ReportBI_1C_SettlementRowList
                                            _EksposurSettlementRow.NilaiEksposur = _ReportBI_1C_SettlementRow.TotalNilaiEksposur
                                            _EksposurSettlementRow.FaktorPengurangModal = _ReportBI_1C_SettlementRow.TotalFaktorPengurangModal
                                            _EksposurSettlementRow.ATMR = _ReportBI_1C_SettlementRow.TotalATMR
                                        Next
                                    Else
                                        _EksposurSettlementRow.NilaiEksposur = 0
                                        _EksposurSettlementRow.FaktorPengurangModal = 0
                                        _EksposurSettlementRow.ATMR = 0

                                        '_IsReportDisplayed = False
                                    End If

                                    _EksposurSettlementDataTable.AddEksposurSettlementRow(_EksposurSettlementRow)


                                    _ChildChildNumber += 1
                                Next
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurSettlement", CType(_EksposurSettlementDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurSettlement Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurSekuritisasi()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurSekuritisasi
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            Dim _ChildChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()

            Dim _ReportBI_1C_SekuritisasiRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiRow()
            Dim _EksposurSekuritisasiRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSekuritisasiRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

                    _EksposurSekuritisasiRow.No = _JenisTagihanNumber & "."

                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                    _KategoriPortofolio.Append(_MsJenisTagihanReportRow.NamaJenisTagihanReport)
                    _EksposurSekuritisasiRow.JenisTransaksi = _KategoriPortofolio.ToString

                    _ReportBI_1C_SekuritisasiRowList = Me.ReportBI_1C_SekuritisasiDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ReportBI_1C_SekuritisasiRowList.Length > 0 Then
                        For Each _ReportBI_1C_SekuritisasiRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiRow In _ReportBI_1C_SekuritisasiRowList
                            _EksposurSekuritisasiRow.FaktorPengurangModal = _ReportBI_1C_SekuritisasiRow.TotalFaktorPengurangModal
                            _EksposurSekuritisasiRow.ATMR = _ReportBI_1C_SekuritisasiRow.TotalATMR
                        Next
                    Else
                        _EksposurSekuritisasiRow.FaktorPengurangModal = 0
                        _EksposurSekuritisasiRow.ATMR = 0

                        '_IsReportDisplayed = False
                    End If

                    _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then
                        _ChildNumber = "a"

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

                            _EksposurSekuritisasiRow.No = ""

                            _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                            _KategoriPortofolio.Append("    " & _ChildNumber & ". " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                            _EksposurSekuritisasiRow.JenisTransaksi = _KategoriPortofolio.ToString

                            _ReportBI_1C_SekuritisasiRowList = Me.ReportBI_1C_SekuritisasiDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ReportBI_1C_SekuritisasiRowList.Length > 0 Then
                                For Each _ReportBI_1C_SekuritisasiRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiRow In _ReportBI_1C_SekuritisasiRowList
                                    _EksposurSekuritisasiRow.FaktorPengurangModal = _ReportBI_1C_SekuritisasiRow.TotalFaktorPengurangModal
                                    _EksposurSekuritisasiRow.ATMR = _ReportBI_1C_SekuritisasiRow.TotalATMR
                                Next
                            Else
                                _EksposurSekuritisasiRow.FaktorPengurangModal = 0
                                _EksposurSekuritisasiRow.ATMR = 0

                                '_IsReportDisplayed = False
                            End If

                            _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)

                            _ChildChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                                                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                                                    "AND FK_MsJenisTagihanReportParentId=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _ChildChildMsJenisTagihanReportRowList.Length > 0 Then
                                _ChildChildNumber = 1

                                For Each _ChildChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow In _ChildChildMsJenisTagihanReportRowList
                                    _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

                                    _EksposurSekuritisasiRow.No = ""

                                    _KategoriPortofolio.Remove(0, _KategoriPortofolio.Length)
                                    _KategoriPortofolio.Append("         " & _ChildChildNumber & ") " & _ChildChildMsJenisTagihanReportRow.NamaJenisTagihanReport)
                                    _EksposurSekuritisasiRow.JenisTransaksi = _KategoriPortofolio.ToString

                                    _ReportBI_1C_SekuritisasiRowList = Me.ReportBI_1C_SekuritisasiDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    If _ReportBI_1C_SekuritisasiRowList.Length > 0 Then
                                        For Each _ReportBI_1C_SekuritisasiRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SekuritisasiRow In _ReportBI_1C_SekuritisasiRowList
                                            _EksposurSekuritisasiRow.FaktorPengurangModal = _ReportBI_1C_SekuritisasiRow.TotalFaktorPengurangModal
                                            _EksposurSekuritisasiRow.ATMR = _ReportBI_1C_SekuritisasiRow.TotalATMR
                                        Next
                                    Else
                                        _EksposurSekuritisasiRow.FaktorPengurangModal = 0
                                        _EksposurSekuritisasiRow.ATMR = 0

                                        '_IsReportDisplayed = False
                                    End If

                                    _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)


                                    _ChildChildNumber += 1
                                Next
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurSekuritisasi", CType(_EksposurSekuritisasiDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurSekuritisasi Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    'Utk Syariah cuma munculkan total Eksposur & pengurang modalnya aja.
    Private Sub GenerateReportEksposurSyariah()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurSyariah
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividualRekapitulasi.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _EksposurSyariahRow As RWADAL.ReportATMRPerIndividualRekapitulasi.EksposurSyariahRow

            _EksposurSyariahRow = _EksposurSyariahDataTable.NewRow
            With _EksposurSyariahRow
                .No = 1
                .JenisTransaksi = ""
                .FaktorPengurangModal = 0
                .ATMR = 0

                If Me.ReportBI_1C_SyariahDataTable.Rows.Count > 0 Then
                    Dim ReportSyariahRow As RWADAL.ReportATMRPerIndividualRekapitulasi.ReportBI_1C_SyariahRow = Me.ReportBI_1C_SyariahDataTable.Rows(0)
                    .FaktorPengurangModal = ReportSyariahRow.TotalFaktorPengurangModal
                    .ATMR = ReportSyariahRow.TotalATMR
                End If

                _EksposurSyariahDataTable.AddEksposurSyariahRow(_EksposurSyariahRow)
            End With

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividualRekapitulasi_EksposurSyariah", CType(_EksposurSyariahDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividualRekapitulasi.GenerateReportEksposurSyariah Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

End Class
