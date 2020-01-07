Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net
Imports RWADAL

Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.Collections.Generic

Partial Class RWA_ReportRWA1B
    Inherits Parent

#Region "Variables"
    Enum NamaJenisReport
        EksposurAset = 1
        EksposurKewajiban = 2
        EksposurCounterparty = 3
        EksposurStatementRisk = 4
        EksposurSekuritisasi = 5
    End Enum

    Enum NamaPengelompokkan
        Rating = 1
        LTV = 2
        GolonganDebitur = 3
        NoGrouping = 4
        IncludeAsSubReport = 5
    End Enum

    Dim _JenisReportNumber As Integer
    Dim _JenisTagihanNumber As Integer
    Dim _ChildNumber As Char
    Dim _NamaJenisTagihanReport As New StringBuilder
    Dim _NamaPengelompokkan As New NamaPengelompokkan
#End Region

#Region "Entities"
    Private _MsJenisTagihanReportDataTable As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
    Private Property MsJenisTagihanReportDatatable() As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
        Get
            If _MsJenisTagihanReportDataTable Is Nothing Then
                If Session("MsJenisTagihanReportDatatable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.MsJenisTagihanReportTableAdapter
                        Session("MsJenisTagihanReportDatatable") = adapter.GetData()
                        _MsJenisTagihanReportDataTable = Session("MsJenisTagihanReportDatatable")
                        Return _MsJenisTagihanReportDataTable
                    End Using
                Else
                    Return CType(Session("MsJenisTagihanReportDatatable"), RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
                End If
            Else
                Return _MsJenisTagihanReportDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
            _MsJenisTagihanReportDataTable = value
            Session("MsJenisTagihanReportDatatable") = _MsJenisTagihanReportDataTable
        End Set
    End Property

    Private _MsKategoriRincianPerhitunganATMRDataTable As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRDataTable
    Private Property MsKategoriRincianPerhitunganATMRDataTable() As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRDataTable
        Get
            If _MsKategoriRincianPerhitunganATMRDataTable Is Nothing Then
                If Session("MsKategoriRincianPerhitunganATMRDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.MsKategoriRincianPerhitunganATMRTableAdapter
                        Session("MsKategoriRincianPerhitunganATMRDataTable") = adapter.GetData()
                        _MsKategoriRincianPerhitunganATMRDataTable = Session("MsKategoriRincianPerhitunganATMRDataTable")
                        Return _MsKategoriRincianPerhitunganATMRDataTable
                    End Using
                Else
                    Return CType(Session("MsKategoriRincianPerhitunganATMRDataTable"), RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRDataTable)
                End If
            Else
                Return _MsKategoriRincianPerhitunganATMRDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRDataTable)
            _MsKategoriRincianPerhitunganATMRDataTable = value
            Session("MsKategoriRincianPerhitunganATMRDataTable") = _MsKategoriRincianPerhitunganATMRDataTable
        End Set
    End Property

    REM Eksposur Aset

    Private _ReportBI_1B_AsetDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_AsetDataTable
    Private Property ReportBI_1B_AsetDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_AsetDataTable
        Get
            If _ReportBI_1B_AsetDataTable Is Nothing Then
                If Session("ReportBI_1B_AsetDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_AsetTableAdapter
                        Session("ReportBI_1B_AsetDataTable") = adapter.GetData()
                        _ReportBI_1B_AsetDataTable = Session("ReportBI_1B_AsetDataTable")
                        Return _ReportBI_1B_AsetDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_AsetDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_AsetDataTable)
                End If
            Else
                Return _ReportBI_1B_AsetDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_AsetDataTable)
            _ReportBI_1B_AsetDataTable = value
            Session("ReportBI_1B_AsetDataTable") = _ReportBI_1B_AsetDataTable
        End Set
    End Property

    Private _ReportBI_1B_Aset_JaminanDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanDataTable
    Private Property ReportBI_1B_Aset_JaminanDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanDataTable
        Get
            If _ReportBI_1B_Aset_JaminanDataTable Is Nothing Then
                If Session("ReportBI_1B_Aset_JaminanDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Aset_JaminanTableAdapter
                        Session("ReportBI_1B_Aset_JaminanDataTable") = adapter.GetData()
                        _ReportBI_1B_Aset_JaminanDataTable = Session("ReportBI_1B_Aset_JaminanDataTable")
                        Return _ReportBI_1B_Aset_JaminanDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Aset_JaminanDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanDataTable)
                End If
            Else
                Return _ReportBI_1B_Aset_JaminanDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanDataTable)
            _ReportBI_1B_Aset_JaminanDataTable = value
            Session("ReportBI_1B_Aset_JaminanDataTable") = _ReportBI_1B_Aset_JaminanDataTable
        End Set
    End Property

    REM Eksposur Kewajiban
    Private _ListExcludedAdministratifLainnyaDataTable As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedAdministratifLainnyaDataTable
    Private Property ListExcludedAdministratifLainnyaDataTable() As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedAdministratifLainnyaDataTable
        Get
            If _ListExcludedAdministratifLainnyaDataTable Is Nothing Then
                If Session("ListExcludedAdministratifLainnyaDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.MsJenisTransaksiRekeningAdministratifTableAdapters.ListExcludedAdministratifLainnyaTableAdapter
                        Session("ListExcludedAdministratifLainnyaDataTable") = adapter.GetData()
                        _ListExcludedAdministratifLainnyaDataTable = Session("ListExcludedAdministratifLainnyaDataTable")
                        Return _ListExcludedAdministratifLainnyaDataTable
                    End Using
                Else
                    Return CType(Session("ListExcludedAdministratifLainnyaDataTable"), RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedAdministratifLainnyaDataTable)
                End If
            Else
                Return _ListExcludedAdministratifLainnyaDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedAdministratifLainnyaDataTable)
            _ListExcludedAdministratifLainnyaDataTable = value
            Session("ListExcludedAdministratifLainnyaDataTable") = _ListExcludedAdministratifLainnyaDataTable
        End Set
    End Property

    Private _ListExcludedTRADataTable As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedTRADataTable
    Private Property ListExcludedTRADataTable() As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedTRADataTable
        Get
            If _ListExcludedTRADataTable Is Nothing Then
                If Session("ListExcludedTRADataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.MsJenisTransaksiRekeningAdministratifTableAdapters.ListExcludedTRATableAdapter
                        Session("ListExcludedTRADataTable") = adapter.GetData()
                        _ListExcludedTRADataTable = Session("ListExcludedTRADataTable")
                        Return _ListExcludedTRADataTable
                    End Using
                Else
                    Return CType(Session("ListExcludedTRADataTable"), RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedTRADataTable)
                End If
            Else
                Return _ListExcludedTRADataTable
            End If
        End Get
        Set(ByVal value As RWADAL.MsJenisTransaksiRekeningAdministratif.ListExcludedTRADataTable)
            _ListExcludedTRADataTable = value
            Session("ListExcludedTRADataTable") = _ListExcludedTRADataTable
        End Set
    End Property

    Private _MsJenisTransaksiRekeningAdministratifDataTable As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable
    Private Property MsJenisTransaksiRekeningAdministratifDataTable() As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable
        Get
            If _MsJenisTransaksiRekeningAdministratifDataTable Is Nothing Then
                If Session("MsJenisTransaksiRekeningAdministratifDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.MsJenisTransaksiRekeningAdministratifTableAdapter
                        Session("MsJenisTransaksiRekeningAdministratifDataTable") = adapter.GetData()
                        _MsJenisTransaksiRekeningAdministratifDataTable = Session("MsJenisTransaksiRekeningAdministratifDataTable")
                        Return _MsJenisTransaksiRekeningAdministratifDataTable
                    End Using
                Else
                    Return CType(Session("MsJenisTransaksiRekeningAdministratifDataTable"), RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable)
                End If
            Else
                Return _MsJenisTransaksiRekeningAdministratifDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable)
            _MsJenisTransaksiRekeningAdministratifDataTable = value
            Session("MsJenisTransaksiRekeningAdministratifDataTable") = _MsJenisTransaksiRekeningAdministratifDataTable
        End Set
    End Property

    Private _JenisEksposurKewajibanDataTable As RWADAL.ReportATMRPerIndividual.JenisEksposurKewajibanDataTable
    Private Property JenisEksposurKewajibanDataTable() As RWADAL.ReportATMRPerIndividual.JenisEksposurKewajibanDataTable
        Get
            If _JenisEksposurKewajibanDataTable Is Nothing Then
                If Session("JenisEksposurKewajibanDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.JenisEksposurKewajibanTableAdapter
                        Session("JenisEksposurKewajibanDataTable") = adapter.GetData()
                        _JenisEksposurKewajibanDataTable = Session("JenisEksposurKewajibanDataTable")
                        Return _JenisEksposurKewajibanDataTable
                    End Using
                Else
                    Return CType(Session("JenisEksposurKewajibanDataTable"), RWADAL.ReportATMRPerIndividual.JenisEksposurKewajibanDataTable)
                End If
            Else
                Return _JenisEksposurKewajibanDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.JenisEksposurKewajibanDataTable)
            _JenisEksposurKewajibanDataTable = value
            Session("JenisEksposurKewajibanDataTable") = _JenisEksposurKewajibanDataTable
        End Set
    End Property

    Private _ReportBI_1B_KewajibanDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_KewajibanDataTable
    Private Property ReportBI_1B_KewajibanDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_KewajibanDataTable
        Get
            If _ReportBI_1B_KewajibanDataTable Is Nothing Then
                If Session("ReportBI_1B_KewajibanDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_KewajibanTableAdapter
                        Session("ReportBI_1B_KewajibanDataTable") = adapter.GetData()
                        _ReportBI_1B_KewajibanDataTable = Session("ReportBI_1B_KewajibanDataTable")
                        Return _ReportBI_1B_KewajibanDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_KewajibanDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_KewajibanDataTable)
                End If
            Else
                Return _ReportBI_1B_KewajibanDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_KewajibanDataTable)
            _ReportBI_1B_KewajibanDataTable = value
            Session("ReportBI_1B_KewajibanDataTable") = _ReportBI_1B_KewajibanDataTable
        End Set
    End Property

    Private _ReportBI_1B_Kewajiban_SummaryDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryDataTable
    Private Property ReportBI_1B_Kewajiban_SummaryDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryDataTable
        Get
            If _ReportBI_1B_Kewajiban_SummaryDataTable Is Nothing Then
                If Session("ReportBI_1B_Kewajiban_SummaryDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Kewajiban_SummaryTableAdapter
                        Session("ReportBI_1B_Kewajiban_SummaryDataTable") = adapter.GetData()
                        _ReportBI_1B_Kewajiban_SummaryDataTable = Session("ReportBI_1B_Kewajiban_SummaryDataTable")
                        Return _ReportBI_1B_Kewajiban_SummaryDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Kewajiban_SummaryDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryDataTable)
                End If
            Else
                Return _ReportBI_1B_Kewajiban_SummaryDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryDataTable)
            _ReportBI_1B_Kewajiban_SummaryDataTable = value
            Session("ReportBI_1B_Kewajiban_SummaryDataTable") = _ReportBI_1B_Kewajiban_SummaryDataTable
        End Set
    End Property

    Private _ReportBI_1B_Kewajiban_JaminanDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanDataTable
    Private Property ReportBI_1B_Kewajiban_JaminanDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanDataTable
        Get
            If _ReportBI_1B_Kewajiban_JaminanDataTable Is Nothing Then
                If Session("ReportBI_1B_Kewajiban_JaminanDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Kewajiban_JaminanTableAdapter
                        Session("ReportBI_1B_Kewajiban_JaminanDataTable") = adapter.GetData()
                        _ReportBI_1B_Kewajiban_JaminanDataTable = Session("ReportBI_1B_Kewajiban_JaminanDataTable")
                        Return _ReportBI_1B_Kewajiban_JaminanDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Kewajiban_JaminanDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanDataTable)
                End If
            Else
                Return _ReportBI_1B_Kewajiban_JaminanDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanDataTable)
            _ReportBI_1B_Kewajiban_JaminanDataTable = value
            Session("ReportBI_1B_Kewajiban_JaminanDataTable") = _ReportBI_1B_Kewajiban_JaminanDataTable
        End Set
    End Property

    Private _ReportBI_1B_Kewajiban_TRADataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_TRADataTable
    Private Property ReportBI_1B_Kewajiban_TRADataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_TRADataTable
        Get
            If _ReportBI_1B_Kewajiban_TRADataTable Is Nothing Then
                If Session("ReportBI_1B_Kewajiban_TRADataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Kewajiban_TRATableAdapter
                        Session("ReportBI_1B_Kewajiban_TRADataTable") = adapter.GetData()
                        _ReportBI_1B_Kewajiban_TRADataTable = Session("ReportBI_1B_Kewajiban_TRADataTable")
                        Return _ReportBI_1B_Kewajiban_TRADataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Kewajiban_TRADataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_TRADataTable)
                End If
            Else
                Return _ReportBI_1B_Kewajiban_TRADataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_TRADataTable)
            _ReportBI_1B_Kewajiban_TRADataTable = value
            Session("ReportBI_1B_Kewajiban_TRADataTable") = _ReportBI_1B_Kewajiban_TRADataTable
        End Set
    End Property

    REM Eksposur Counterparty

    Private _JenisEksposurCounterpartyDataTable As RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyDataTable
    Private Property JenisEksposurCounterpartyDataTable() As RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyDataTable
        Get
            If _JenisEksposurCounterpartyDataTable Is Nothing Then
                If Session("JenisEksposurCounterpartyDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.JenisEksposurCounterpartyTableAdapter
                        Session("JenisEksposurCounterpartyDataTable") = adapter.GetData()
                        _JenisEksposurCounterpartyDataTable = Session("JenisEksposurCounterpartyDataTable")
                        Return _JenisEksposurCounterpartyDataTable
                    End Using
                Else
                    Return CType(Session("JenisEksposurCounterpartyDataTable"), RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyDataTable)
                End If
            Else
                Return _JenisEksposurCounterpartyDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyDataTable)
            _JenisEksposurCounterpartyDataTable = value
            Session("JenisEksposurCounterpartyDataTable") = _JenisEksposurCounterpartyDataTable
        End Set
    End Property

    Private _KriteriaPotentialFutureEksposurDataTable As RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurDataTable
    Private Property KriteriaPotentialFutureEksposurDataTable() As RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurDataTable
        Get
            If _KriteriaPotentialFutureEksposurDataTable Is Nothing Then
                If Session("KriteriaPotentialFutureEksposurDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.KriteriaPotentialFutureEksposurTableAdapter
                        Session("KriteriaPotentialFutureEksposurDataTable") = adapter.GetData()
                        _KriteriaPotentialFutureEksposurDataTable = Session("KriteriaPotentialFutureEksposurDataTable")
                        Return _KriteriaPotentialFutureEksposurDataTable
                    End Using
                Else
                    Return CType(Session("KriteriaPotentialFutureEksposurDataTable"), RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurDataTable)
                End If
            Else
                Return _KriteriaPotentialFutureEksposurDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurDataTable)
            _KriteriaPotentialFutureEksposurDataTable = value
            Session("KriteriaPotentialFutureEksposurDataTable") = _KriteriaPotentialFutureEksposurDataTable
        End Set
    End Property

    Private _ReportBI_1B_CounterpartyDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_CounterpartyDataTable
    Private Property ReportBI_1B_CounterpartyDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_CounterpartyDataTable
        Get
            If _ReportBI_1B_CounterpartyDataTable Is Nothing Then
                If Session("ReportBI_1B_CounterpartyDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_CounterpartyTableAdapter
                        Session("ReportBI_1B_CounterpartyDataTable") = adapter.GetData()
                        _ReportBI_1B_CounterpartyDataTable = Session("ReportBI_1B_CounterpartyDataTable")
                        Return _ReportBI_1B_CounterpartyDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_CounterpartyDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_CounterpartyDataTable)
                End If
            Else
                Return _ReportBI_1B_CounterpartyDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_CounterpartyDataTable)
            _ReportBI_1B_CounterpartyDataTable = value
            Session("ReportBI_1B_CounterpartyDataTable") = _ReportBI_1B_CounterpartyDataTable
        End Set
    End Property

    Private _ReportBI_1B_Counterparty_SummaryDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_SummaryDataTable
    Private Property ReportBI_1B_Counterparty_SummaryDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_SummaryDataTable
        Get
            If _ReportBI_1B_Counterparty_SummaryDataTable Is Nothing Then
                If Session("ReportBI_1B_Counterparty_SummaryDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Counterparty_SummaryTableAdapter
                        Session("ReportBI_1B_Counterparty_SummaryDataTable") = adapter.GetData()
                        _ReportBI_1B_Counterparty_SummaryDataTable = Session("ReportBI_1B_Counterparty_SummaryDataTable")
                        Return _ReportBI_1B_Counterparty_SummaryDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Counterparty_SummaryDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_SummaryDataTable)
                End If
            Else
                Return _ReportBI_1B_Counterparty_SummaryDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_SummaryDataTable)
            _ReportBI_1B_Counterparty_SummaryDataTable = value
            Session("ReportBI_1B_Counterparty_SummaryDataTable") = _ReportBI_1B_Counterparty_SummaryDataTable
        End Set
    End Property

    Private _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable
    Private Property ReportBI_1B_Counterparty_PotentialFutureExposureDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable
        Get
            If _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable Is Nothing Then
                If Session("ReportBI_1B_Counterparty_PotentialFutureExposureDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_Counterparty_PotentialFutureExposureTableAdapter
                        Session("ReportBI_1B_Counterparty_PotentialFutureExposureDataTable") = adapter.GetData()
                        _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable = Session("ReportBI_1B_Counterparty_PotentialFutureExposureDataTable")
                        Return _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_Counterparty_PotentialFutureExposureDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable)
                End If
            Else
                Return _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable)
            _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable = value
            Session("ReportBI_1B_Counterparty_PotentialFutureExposureDataTable") = _ReportBI_1B_Counterparty_PotentialFutureExposureDataTable
        End Set
    End Property

    REM Eksposur Sekuritisasi

    Private _MsBobotResikoSekuritisasiDataTable As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiDataTable
    Private Property MsBobotResikoSekuritisasiDataTable() As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiDataTable
        Get
            If _MsBobotResikoSekuritisasiDataTable Is Nothing Then
                If Session("MsBobotResikoSekuritisasiDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.MsBobotResikoSekuritisasiTableAdapter
                        Session("MsBobotResikoSekuritisasiDataTable") = adapter.GetData()
                        _MsBobotResikoSekuritisasiDataTable = Session("MsBobotResikoSekuritisasiDataTable")
                        Return _MsBobotResikoSekuritisasiDataTable
                    End Using
                Else
                    Return CType(Session("MsBobotResikoSekuritisasiDataTable"), RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiDataTable)
                End If
            Else
                Return _MsBobotResikoSekuritisasiDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiDataTable)
            _MsBobotResikoSekuritisasiDataTable = value
            Session("MsBobotResikoSekuritisasiDataTable") = _MsBobotResikoSekuritisasiDataTable
        End Set
    End Property

    Private _ReportBI_1B_SekuritisasiDataTable As RWADAL.ReportATMRPerIndividual.ReportBI_1B_SekuritisasiDataTable
    Private Property ReportBI_1B_SekuritisasiDataTable() As RWADAL.ReportATMRPerIndividual.ReportBI_1B_SekuritisasiDataTable
        Get
            If _ReportBI_1B_SekuritisasiDataTable Is Nothing Then
                If Session("ReportBI_1B_SekuritisasiDataTable") Is Nothing Then
                    'todo: ambil data lalu set ke session 
                    Using adapter As New RWADAL.ReportATMRPerIndividualTableAdapters.ReportBI_1B_SekuritisasiTableAdapter
                        Session("ReportBI_1B_SekuritisasiDataTable") = adapter.GetData()
                        _ReportBI_1B_SekuritisasiDataTable = Session("ReportBI_1B_SekuritisasiDataTable")
                        Return _ReportBI_1B_SekuritisasiDataTable
                    End Using
                Else
                    Return CType(Session("ReportBI_1B_SekuritisasiDataTable"), RWADAL.ReportATMRPerIndividual.ReportBI_1B_SekuritisasiDataTable)
                End If
            Else
                Return _ReportBI_1B_SekuritisasiDataTable
            End If
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.ReportBI_1B_SekuritisasiDataTable)
            _ReportBI_1B_SekuritisasiDataTable = value
            Session("ReportBI_1B_SekuritisasiDataTable") = _ReportBI_1B_SekuritisasiDataTable
        End Set
    End Property

    Private Property _ExcludedMsJenisTagihanReportDataTable() As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
        Get
            If Session("Report1B._ExcludedMsJenisTagihanReportDataTable") Is Nothing Then
                Session("Report1B._ExcludedMsJenisTagihanReportDataTable") = New RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
            End If
            Return CType(Session("Report1B._ExcludedMsJenisTagihanReportDataTable"), RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
            Session("Report1B._ExcludedMsJenisTagihanReportDataTable") = value
        End Set
    End Property

    Private Property _ExcludedAdministratifMsJenisTagihanReportDataTable() As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
        Get
            If Session("Report1B._ExcludedAdministratifMsJenisTagihanReportDataTable") Is Nothing Then
                Session("Report1B._ExcludedAdministratifMsJenisTagihanReportDataTable") = New RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable
            End If
            Return CType(Session("Report1B._ExcludedAdministratifMsJenisTagihanReportDataTable"), RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportDataTable)
            Session("Report1B._ExcludedAdministratifMsJenisTagihanReportDataTable") = value
        End Set
    End Property

    Private Property _ExcludedTRADataTable() As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable
        Get
            If Session("Report1B._ExcludedTRADataTable") Is Nothing Then
                Session("Report1B._ExcludedTRADataTable") = New RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable
            End If
            Return CType(Session("Report1B._ExcludedTRADataTable"), RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable)
        End Get
        Set(ByVal value As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifDataTable)
            Session("Report1B._ExcludedTRADataTable") = value
        End Set
    End Property

    Private _EksposurAsetDataTable As New RWADAL.ReportATMRPerIndividual.EksposurAsetDataTable
    Private _EksposurKewajibanDataTable As New RWADAL.ReportATMRPerIndividual.EksposurKewajibanDataTable
    Private _EksposurKewajibanSummaryDataTable As New RWADAL.ReportATMRPerIndividual.EksposurKewajibanSummaryDataTable
    Private _EksposurKewajibanTRADataTable As New RWADAL.ReportATMRPerIndividual.EksposurKewajibanTRADataTable
    Private _EksposurCounterpartyDataTable As New RWADAL.ReportATMRPerIndividual.EksposurCounterpartyDataTable
    Private _EksposurCounterpartySummaryDataTable As New RWADAL.ReportATMRPerIndividual.EksposurCounterpartySummaryDataTable
    Private _EksposurCounterpartyPotentialFutureExposureDataTable As New RWADAL.ReportATMRPerIndividual.EksposurCounterpartyPotentialFutureExposureDataTable
    Private _EksposurSekuritisasiDataTable As New RWADAL.ReportATMRPerIndividual.EksposurSekuritisasiDataTable
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

#Region "Clear Session"
    Private Sub ClearSession()
        MsJenisTagihanReportDatatable = Nothing
        MsKategoriRincianPerhitunganATMRDataTable = Nothing

        ReportBI_1B_AsetDataTable = Nothing
        ReportBI_1B_Aset_JaminanDataTable = Nothing

        ListExcludedAdministratifLainnyaDataTable = Nothing
        ListExcludedTRADataTable = Nothing
        MsJenisTransaksiRekeningAdministratifDataTable = Nothing
        JenisEksposurKewajibanDataTable = Nothing
        ReportBI_1B_KewajibanDataTable = Nothing
        ReportBI_1B_Kewajiban_SummaryDataTable = Nothing
        ReportBI_1B_Kewajiban_JaminanDataTable = Nothing
        ReportBI_1B_Kewajiban_TRADataTable = Nothing

        JenisEksposurCounterpartyDataTable = Nothing
        KriteriaPotentialFutureEksposurDataTable = Nothing
        ReportBI_1B_CounterpartyDataTable = Nothing
        ReportBI_1B_Counterparty_SummaryDataTable = Nothing
        ReportBI_1B_Counterparty_PotentialFutureExposureDataTable = Nothing

        MsBobotResikoSekuritisasiDataTable = Nothing
        ReportBI_1B_SekuritisasiDataTable = Nothing

        Session("Report1B._ExcludedMsJenisTagihanReportDataTable") = Nothing
        Session("Report1B._ExcludedAdministratifMsJenisTagihanReportDataTable") = Nothing
        Session("Report1B._ExcludedTRADataTable") = Nothing
    End Sub
#End Region

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
            Me.ReportViewer1.Visible = True
            PanelReport.Hidden = False

            ReportViewer1.LocalReport.DataSources.Clear()

            GenerateReportEksposurAset()
            GenerateReportEksposurKewajiban()
            GenerateReportEksposurKewajibanSummary()
            GenerateReportEksposurKewajibanTRA()
            GenerateReportEksposurCounterparty()
            GenerateReportEksposurCounterpartySummary()
            GenerateReportEksposurCounterpartyPotentialFutureExposure()
            GenerateReportEksposurSekuritisasi()

            AddHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingEvent

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Private Sub GenerateReportEksposurAset()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurAset
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            Dim _MsKategoriRincianPerhitunganATMRRowList As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow()
            Dim _ReportBI_1B_AsetRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_AsetRow()
            Dim _ReportBI_1B_Aset_JaminanRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow()

            Dim _EksposurAsetRow As RWADAL.ReportATMRPerIndividual.EksposurAsetRow
            Dim LstSubReport As New List(Of Integer)

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                If _ExcludedMsJenisTagihanReportDataTable.FindByPK_MsJenisTagihanReportId(_MsJenisTagihanReportRow.PK_MsJenisTagihanReportId) Is Nothing Then

                    REM Get current Jenis Tagihan Report child record.

                    _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                        "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                        "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                    REM Check if current Jenis Tagihan Report record has child record.

                    If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                        If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                            _ChildNumber = "a"

                            REM current MsJenisTagihanReport record has child, then collect the related child.

                            For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList

                                REM Get NamaJenisTagihanReport

                                _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                                _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                                _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                                _NamaJenisTagihanReport.Append(_ChildNumber & ".")
                                _NamaJenisTagihanReport.Append(" " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)

                                REM Add NamaJenisTagihanReport into EksposurAset data table

                                _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                                    _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                                    Select Case _NamaPengelompokkan
                                        Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                            _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                            For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                                _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                                _EksposurAsetRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                                _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                                _EksposurAsetRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                                _EksposurAsetRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                                _ReportBI_1B_AsetRowList = Me.ReportBI_1B_AsetDataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                If _ReportBI_1B_AsetRowList.Length > 0 Then
                                                    _EksposurAsetRow.TagihanBersih = _ReportBI_1B_AsetRowList(0).TotalTagihanBersih
                                                Else
                                                    _EksposurAsetRow.TagihanBersih = 0
                                                End If

                                                _ReportBI_1B_Aset_JaminanRowList = Me.ReportBI_1B_Aset_JaminanDataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                If _ReportBI_1B_Aset_JaminanRowList.Length > 0 Then

                                                    For Each _ReportBI_1B_AsetJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow In _ReportBI_1B_Aset_JaminanRowList
                                                        Select Case _ReportBI_1B_AsetJaminanRow.BobotResikoJaminan
                                                            Case 0
                                                                _EksposurAsetRow.TotalBagianDijamin_0 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                            Case 20
                                                                _EksposurAsetRow.TotalBagianDijamin_20 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                            Case 50
                                                                _EksposurAsetRow.TotalBagianDijamin_50 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                            Case 100
                                                                _EksposurAsetRow.TotalBagianDijamin_100 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        End Select
                                                    Next

                                                Else
                                                    _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                                    _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                                    _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                                    _EksposurAsetRow.TotalBagianDijamin_100 = 0
                                                End If

                                                _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                    _EksposurAsetRow.TotalBagianDijamin_0 +
                                                    _EksposurAsetRow.TotalBagianDijamin_20 +
                                                    _EksposurAsetRow.TotalBagianDijamin_50 +
                                                    _EksposurAsetRow.TotalBagianDijamin_100)
                                                _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                                _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                                    (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                                    (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                                    (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                                    (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                                _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)
                                            Next
                                        Case NamaPengelompokkan.NoGrouping
                                            _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                            _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                            _EksposurAsetRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                            _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                            _EksposurAsetRow.NamaKategoriRincian = _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport
                                            _EksposurAsetRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                            _ReportBI_1B_AsetRowList = Me.ReportBI_1B_AsetDataTable.Select(
                                                                                            "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                            "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                                                            "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                                                            "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_AsetRowList.Length > 0 Then
                                                _EksposurAsetRow.TagihanBersih = _ReportBI_1B_AsetRowList(0).TotalTagihanBersih
                                            Else
                                                _EksposurAsetRow.TagihanBersih = 0
                                            End If

                                            _ReportBI_1B_Aset_JaminanRowList = Me.ReportBI_1B_Aset_JaminanDataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_Aset_JaminanRowList.Length > 0 Then

                                                For Each _ReportBI_1B_AsetJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow In _ReportBI_1B_Aset_JaminanRowList

                                                    Select Case _ReportBI_1B_AsetJaminanRow.BobotResikoJaminan
                                                        Case 0
                                                            _EksposurAsetRow.TotalBagianDijamin_0 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 20
                                                            _EksposurAsetRow.TotalBagianDijamin_20 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 50
                                                            _EksposurAsetRow.TotalBagianDijamin_50 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 100
                                                            _EksposurAsetRow.TotalBagianDijamin_100 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                    End Select
                                                Next

                                            Else

                                                _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_100 = 0

                                            End If

                                            _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                                                            _EksposurAsetRow.TotalBagianDijamin_0 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_20 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_50 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_100)
                                            _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                            _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                                (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                                (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                                (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                                (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                            _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)
                                        Case NamaPengelompokkan.IncludeAsSubReport
                                            If Not LstSubReport.Contains(_ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId) Then
                                                Dim StrNamaJenisTagihanReport As String = ""
                                                StrNamaJenisTagihanReport = _NamaJenisTagihanReport.ToString.Replace(_ChildNumber & "." & " " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport, " " & _MsJenisTagihanReportRow.NamaJenisTagihanReport)

                                                'utk include as subreport, diambil sebagai sub report bagi tagihan parent nya
                                                Using ObjDb As New NawaDatadevEntities
                                                    For Each ObjJenisTagihanSubReport As RWA_MsJenisTagihanReport In ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.FK_MsJenisTagihanReportParentId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId).ToList
                                                        LstSubReport.Add(ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId)

                                                        Dim ObjMsKategoriRincianList As List(Of RWA_MsKategoriRincianPerhitunganATMR) = ObjDb.RWA_MsKategoriRincianPerhitunganATMR.Where(Function(x) x.Fk_JenisTagihanReport_Id = ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId).ToList
                                                        Dim ObjMsKategoriRincian As New rwa_MsKategoriRincianPerhitunganATMR
                                                        If ObjMsKategoriRincianList.Count > 0 Then
                                                            ObjMsKategoriRincian = ObjMsKategoriRincianList(0)
                                                        End If

                                                        _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                                        _EksposurAsetRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                                        _EksposurAsetRow.NamaJenisTagihanReport = StrNamaJenisTagihanReport
                                                        _EksposurAsetRow.NamaKategoriRincian = ObjJenisTagihanSubReport.NamaJenisTagihanReport
                                                        _EksposurAsetRow.BobotResiko = ObjMsKategoriRincian.BobotResiko.GetValueOrDefault(0)

                                                        _ReportBI_1B_AsetRowList = Me.ReportBI_1B_AsetDataTable.Select(
                                                                                                        "Fk_MsJenisTagihanReport_Id=" & ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId & " AND " &
                                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & ObjMsKategoriRincian.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                        If _ReportBI_1B_AsetRowList.Length > 0 Then
                                                            _EksposurAsetRow.TagihanBersih = _ReportBI_1B_AsetRowList(0).TotalTagihanBersih
                                                        Else
                                                            _EksposurAsetRow.TagihanBersih = 0
                                                        End If

                                                        _ReportBI_1B_Aset_JaminanRowList = Me.ReportBI_1B_Aset_JaminanDataTable.Select(
                                                                "Fk_MsJenisTagihanReport_Id=" & ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId & " AND " &
                                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & ObjMsKategoriRincian.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                                "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                                "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                        If _ReportBI_1B_Aset_JaminanRowList.Length > 0 Then

                                                            For Each _ReportBI_1B_AsetJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow In _ReportBI_1B_Aset_JaminanRowList

                                                                Select Case _ReportBI_1B_AsetJaminanRow.BobotResikoJaminan
                                                                    Case 0
                                                                        _EksposurAsetRow.TotalBagianDijamin_0 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                                    Case 20
                                                                        _EksposurAsetRow.TotalBagianDijamin_20 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                                    Case 50
                                                                        _EksposurAsetRow.TotalBagianDijamin_50 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                                    Case 100
                                                                        _EksposurAsetRow.TotalBagianDijamin_100 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                                End Select
                                                            Next

                                                        Else

                                                            _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                                            _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                                            _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                                            _EksposurAsetRow.TotalBagianDijamin_100 = 0

                                                        End If

                                                        _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                                                                        _EksposurAsetRow.TotalBagianDijamin_0 +
                                                                                                        _EksposurAsetRow.TotalBagianDijamin_20 +
                                                                                                        _EksposurAsetRow.TotalBagianDijamin_50 +
                                                                                                        _EksposurAsetRow.TotalBagianDijamin_100)
                                                        _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                                        _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                                            (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                                            (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                                            (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                                            (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                                        _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)
                                                    Next
                                                End Using
                                            End If
                                    End Select

                                    '_ChildNumber = Chr(Asc(_ChildNumber) + 1)
                                Else
                                    _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                    _EksposurAsetRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                    _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                    _EksposurAsetRow.NamaKategoriRincian = "N/A"
                                    _EksposurAsetRow.BobotResiko = 0
                                    _EksposurAsetRow.TagihanBersih = 0
                                    _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                    _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                    _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                    _EksposurAsetRow.TotalBagianDijamin_100 = 0

                                    _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                        _EksposurAsetRow.TotalBagianDijamin_0 +
                                        _EksposurAsetRow.TotalBagianDijamin_20 +
                                        _EksposurAsetRow.TotalBagianDijamin_50 +
                                        _EksposurAsetRow.TotalBagianDijamin_100)
                                    _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                    _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                        (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                        (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                        (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                        (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                    _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)

                                    '_IsReportDisplayed = False
                                End If

                                _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                            Next
                        Else
                            _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                            _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                            _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                            _NamaJenisTagihanReport.Append(" " & _MsJenisTagihanReportRow.NamaJenisTagihanReport)

                            _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                                _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                                Select Case _NamaPengelompokkan
                                    Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                            _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                            _EksposurAsetRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                            _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                            _EksposurAsetRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                            _EksposurAsetRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                            _ReportBI_1B_AsetRowList = Me.ReportBI_1B_AsetDataTable.Select(
                                                                                            "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                            "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                                                            "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                                                            "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_AsetRowList.Length > 0 Then
                                                _EksposurAsetRow.TagihanBersih = _ReportBI_1B_AsetRowList(0).TotalTagihanBersih
                                            Else
                                                _EksposurAsetRow.TagihanBersih = 0
                                            End If

                                            _ReportBI_1B_Aset_JaminanRowList = Me.ReportBI_1B_Aset_JaminanDataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_Aset_JaminanRowList.Length > 0 Then

                                                For Each _ReportBI_1B_AsetJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow In _ReportBI_1B_Aset_JaminanRowList
                                                    Select Case _ReportBI_1B_AsetJaminanRow.BobotResikoJaminan
                                                        Case 0
                                                            _EksposurAsetRow.TotalBagianDijamin_0 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 20
                                                            _EksposurAsetRow.TotalBagianDijamin_20 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 50
                                                            _EksposurAsetRow.TotalBagianDijamin_50 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                        Case 100
                                                            _EksposurAsetRow.TotalBagianDijamin_100 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                    End Select
                                                Next

                                            Else
                                                _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                                _EksposurAsetRow.TotalBagianDijamin_100 = 0
                                            End If

                                            _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                                                            _EksposurAsetRow.TotalBagianDijamin_0 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_20 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_50 +
                                                                                            _EksposurAsetRow.TotalBagianDijamin_100)
                                            _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                            _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                                (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                                (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                                (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                                (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                            _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)
                                        Next
                                    Case NamaPengelompokkan.NoGrouping
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                        _EksposurAsetRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                        _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                        _EksposurAsetRow.NamaKategoriRincian = _MsJenisTagihanReportRow.NamaJenisTagihanReport
                                        _EksposurAsetRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                        _ReportBI_1B_AsetRowList = Me.ReportBI_1B_AsetDataTable.Select(
                                                                                        "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_AsetRowList.Length > 0 Then
                                            _EksposurAsetRow.TagihanBersih = _ReportBI_1B_AsetRowList(0).TotalTagihanBersih
                                        Else
                                            _EksposurAsetRow.TagihanBersih = 0
                                        End If

                                        _ReportBI_1B_Aset_JaminanRowList = Me.ReportBI_1B_Aset_JaminanDataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_Aset_JaminanRowList.Length > 0 Then

                                            For Each _ReportBI_1B_AsetJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Aset_JaminanRow In _ReportBI_1B_Aset_JaminanRowList

                                                Select Case _ReportBI_1B_AsetJaminanRow.BobotResikoJaminan
                                                    Case 0
                                                        _EksposurAsetRow.TotalBagianDijamin_0 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                    Case 20
                                                        _EksposurAsetRow.TotalBagianDijamin_20 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                    Case 50
                                                        _EksposurAsetRow.TotalBagianDijamin_50 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                    Case 100
                                                        _EksposurAsetRow.TotalBagianDijamin_100 = _ReportBI_1B_AsetJaminanRow.TotalBagianDijamin
                                                End Select
                                            Next

                                        Else

                                            _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                            _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                            _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                            _EksposurAsetRow.TotalBagianDijamin_100 = 0

                                        End If

                                        _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                                                        _EksposurAsetRow.TotalBagianDijamin_0 +
                                                                                        _EksposurAsetRow.TotalBagianDijamin_20 +
                                                                                        _EksposurAsetRow.TotalBagianDijamin_50 +
                                                                                        _EksposurAsetRow.TotalBagianDijamin_100)
                                        _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                        _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                            (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                            (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                            (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                            (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                        _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)
                                    Case NamaPengelompokkan.IncludeAsSubReport

                                End Select
                            Else
                                _EksposurAsetRow = _EksposurAsetDataTable.NewRow

                                _EksposurAsetRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                _EksposurAsetRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                _EksposurAsetRow.NamaKategoriRincian = "N/A"
                                _EksposurAsetRow.BobotResiko = 0
                                _EksposurAsetRow.TagihanBersih = 0
                                _EksposurAsetRow.TotalBagianDijamin_0 = 0
                                _EksposurAsetRow.TotalBagianDijamin_20 = 0
                                _EksposurAsetRow.TotalBagianDijamin_50 = 0
                                _EksposurAsetRow.TotalBagianDijamin_100 = 0

                                _EksposurAsetRow.BagianTidakDijamin = _EksposurAsetRow.TagihanBersih - (
                                                                                _EksposurAsetRow.TotalBagianDijamin_0 +
                                                                                _EksposurAsetRow.TotalBagianDijamin_20 +
                                                                                _EksposurAsetRow.TotalBagianDijamin_50 +
                                                                                _EksposurAsetRow.TotalBagianDijamin_100)
                                _EksposurAsetRow.ATMRSebelumMRK = (_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.TagihanBersih
                                _EksposurAsetRow.ATMRSetelahMRK = ((_EksposurAsetRow.BobotResiko / 100) * _EksposurAsetRow.BagianTidakDijamin) +
                                    (_EksposurAsetRow.TotalBagianDijamin_0 * 0) +
                                    (_EksposurAsetRow.TotalBagianDijamin_20 * 0.2) +
                                    (_EksposurAsetRow.TotalBagianDijamin_50 * 0.5) +
                                    (_EksposurAsetRow.TotalBagianDijamin_100 * 1)

                                _EksposurAsetDataTable.AddEksposurAsetRow(_EksposurAsetRow)

                                '_IsReportDisplayed = False
                            End If
                        End If

                        _JenisTagihanNumber += 1
                    End If

                End If

            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurAset", CType(_EksposurAsetDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurAset Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurKewajiban()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurKewajiban
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)

            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            Dim _MsKategoriRincianPerhitunganATMRRowList As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow()
            Dim _ReportBI_1B_KewajibanRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_KewajibanRow()
            Dim _ReportBI_1B_Kewajiban_JaminanRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow()

            Dim _EksposurKewajibanRow As RWADAL.ReportATMRPerIndividual.EksposurKewajibanRow
            Dim LstSubReport As New List(Of Integer)

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                REM Get current Jenis Tagihan Report child record.

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                REM Check if current Jenis Tagihan Report record has child record.

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        _ChildNumber = "a"

                        REM current MsJenisTagihanReport record has child, then collect the related child.

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList

                            REM Get NamaJenisTagihanReport

                            _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                            _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                            _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                            _NamaJenisTagihanReport.Append(_ChildNumber & ".")
                            _NamaJenisTagihanReport.Append(" " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)

                            REM Add NamaJenisTagihanReport into EksposurKewajiban data table

                            _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                                _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                                Select Case _NamaPengelompokkan
                                    Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                            _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                            _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                            _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                            _EksposurKewajibanRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                            _EksposurKewajibanRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                            _ReportBI_1B_KewajibanRowList = Me.ReportBI_1B_KewajibanDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_KewajibanRowList.Length > 0 Then
                                                _EksposurKewajibanRow.TagihanBersih = _ReportBI_1B_KewajibanRowList(0).TotalTagihanBersih
                                            Else
                                                _EksposurKewajibanRow.TagihanBersih = 0
                                            End If

                                            _ReportBI_1B_Kewajiban_JaminanRowList = Me.ReportBI_1B_Kewajiban_JaminanDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_Kewajiban_JaminanRowList.Length > 0 Then

                                                For Each _ReportBI_1B_KewajibanJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow In _ReportBI_1B_Kewajiban_JaminanRowList
                                                    Select Case _ReportBI_1B_KewajibanJaminanRow.BobotResikoJaminan
                                                        Case 0
                                                            _EksposurKewajibanRow.TotalBagianDijamin_0 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                        Case 20
                                                            _EksposurKewajibanRow.TotalBagianDijamin_20 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                        Case 50
                                                            _EksposurKewajibanRow.TotalBagianDijamin_50 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                        Case 100
                                                            _EksposurKewajibanRow.TotalBagianDijamin_100 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    End Select
                                                Next

                                            Else
                                                _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                                _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                                _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                                _EksposurKewajibanRow.TotalBagianDijamin_100 = 0
                                            End If

                                            _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                _EksposurKewajibanRow.TotalBagianDijamin_100)
                                            _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                            _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                                (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                                (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                                (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                                (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                            _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)
                                        Next
                                    Case NamaPengelompokkan.NoGrouping
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                        _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                        _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                        _EksposurKewajibanRow.NamaKategoriRincian = _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport
                                        _EksposurKewajibanRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                        _ReportBI_1B_KewajibanRowList = Me.ReportBI_1B_KewajibanDataTable.Select(
                                                                                        "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_KewajibanRowList.Length > 0 Then
                                            _EksposurKewajibanRow.TagihanBersih = _ReportBI_1B_KewajibanRowList(0).TotalTagihanBersih
                                        Else
                                            _EksposurKewajibanRow.TagihanBersih = 0
                                        End If

                                        _ReportBI_1B_Kewajiban_JaminanRowList = Me.ReportBI_1B_Kewajiban_JaminanDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_Kewajiban_JaminanRowList.Length > 0 Then

                                            For Each _ReportBI_1B_KewajibanJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow In _ReportBI_1B_Kewajiban_JaminanRowList

                                                Select Case _ReportBI_1B_KewajibanJaminanRow.BobotResikoJaminan
                                                    Case 0
                                                        _EksposurKewajibanRow.TotalBagianDijamin_0 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 20
                                                        _EksposurKewajibanRow.TotalBagianDijamin_20 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 50
                                                        _EksposurKewajibanRow.TotalBagianDijamin_50 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 100
                                                        _EksposurKewajibanRow.TotalBagianDijamin_100 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                End Select
                                            Next

                                        Else

                                            _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_100 = 0

                                        End If

                                        _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_100)
                                        _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                        _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                        _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)
                                    Case NamaPengelompokkan.IncludeAsSubReport
                                        If Not LstSubReport.Contains(_ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId) Then
                                            Dim StrNamaJenisTagihanReport As String = ""
                                            StrNamaJenisTagihanReport = _NamaJenisTagihanReport.ToString.Replace(_ChildNumber & "." & " " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport, " " & _MsJenisTagihanReportRow.NamaJenisTagihanReport)

                                            'utk include as subreport, diambil sebagai sub report bagi tagihan parent nya
                                            Using ObjDb As New NawaDatadevEntities
                                                For Each ObjJenisTagihanSubReport As RWA_MsJenisTagihanReport In ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.FK_MsJenisTagihanReportParentId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId).ToList
                                                    LstSubReport.Add(ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId)

                                                    Dim ObjMsKategoriRincianList As List(Of RWA_MsKategoriRincianPerhitunganATMR) = ObjDb.RWA_MsKategoriRincianPerhitunganATMR.Where(Function(x) x.Fk_JenisTagihanReport_Id = ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId).ToList
                                                    Dim ObjMsKategoriRincian As New RWA_MsKategoriRincianPerhitunganATMR
                                                    If ObjMsKategoriRincianList.Count > 0 Then
                                                        ObjMsKategoriRincian = ObjMsKategoriRincianList(0)
                                                    End If

                                                    _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                                    _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                                    _EksposurKewajibanRow.NamaJenisTagihanReport = StrNamaJenisTagihanReport
                                                    _EksposurKewajibanRow.NamaKategoriRincian = ObjJenisTagihanSubReport.NamaJenisTagihanReport
                                                    _EksposurKewajibanRow.BobotResiko = ObjMsKategoriRincian.BobotResiko.GetValueOrDefault(0)

                                                    _ReportBI_1B_KewajibanRowList = Me.ReportBI_1B_KewajibanDataTable.Select(
                                                            "Fk_MsJenisTagihanReport_Id=" & ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId & " AND " &
                                                            "Fk_MsKategoriRincianPerhitunganATMR_Id=" & ObjMsKategoriRincian.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                            "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                            "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                    If _ReportBI_1B_KewajibanRowList.Length > 0 Then
                                                        _EksposurKewajibanRow.TagihanBersih = _ReportBI_1B_KewajibanRowList(0).TotalTagihanBersih
                                                    Else
                                                        _EksposurKewajibanRow.TagihanBersih = 0
                                                    End If

                                                    _ReportBI_1B_Kewajiban_JaminanRowList = Me.ReportBI_1B_Kewajiban_JaminanDataTable.Select(
                                                  "Fk_MsJenisTagihanReport_Id=" & ObjJenisTagihanSubReport.PK_MsJenisTagihanReportId & " AND " &
                                                  "Fk_MsKategoriRincianPerhitunganATMR_Id=" & ObjMsKategoriRincian.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                  "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                  "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                    If _ReportBI_1B_Kewajiban_JaminanRowList.Length > 0 Then

                                                        For Each _ReportBI_1B_KewajibanJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow In _ReportBI_1B_Kewajiban_JaminanRowList

                                                            Select Case _ReportBI_1B_KewajibanJaminanRow.BobotResikoJaminan
                                                                Case 0
                                                                    _EksposurKewajibanRow.TotalBagianDijamin_0 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                                Case 20
                                                                    _EksposurKewajibanRow.TotalBagianDijamin_20 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                                Case 50
                                                                    _EksposurKewajibanRow.TotalBagianDijamin_50 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                                Case 100
                                                                    _EksposurKewajibanRow.TotalBagianDijamin_100 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                            End Select
                                                        Next

                                                    Else

                                                        _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                                        _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                                        _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                                        _EksposurKewajibanRow.TotalBagianDijamin_100 = 0

                                                    End If

                                                    _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                            _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                            _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                            _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                            _EksposurKewajibanRow.TotalBagianDijamin_100)
                                                    _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                                    _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                                 (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                                 (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                                 (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                                 (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                                    _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)
                                                Next
                                            End Using
                                        End If

                                End Select

                                '_ChildNumber = Chr(Asc(_ChildNumber) + 1)
                            Else
                                _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                _EksposurKewajibanRow.NamaKategoriRincian = "N/A"
                                _EksposurKewajibanRow.BobotResiko = 0


                                _EksposurKewajibanRow.TagihanBersih = 0


                                _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                _EksposurKewajibanRow.TotalBagianDijamin_100 = 0

                                _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                    _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                    _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                    _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                    _EksposurKewajibanRow.TotalBagianDijamin_100)
                                _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                    (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                    (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                    (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                    (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)

                                '_IsReportDisplayed = False

                                '_ChildNumber = Chr(Asc(_ChildNumber) + 1)
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    Else
                        _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                        _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                        _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                        _NamaJenisTagihanReport.Append(" " & _MsJenisTagihanReportRow.NamaJenisTagihanReport)

                        _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                        If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                            _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                            Select Case _NamaPengelompokkan
                                Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                    _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                        _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                        _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                        _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                        _EksposurKewajibanRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                        _EksposurKewajibanRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                        _ReportBI_1B_KewajibanRowList = Me.ReportBI_1B_KewajibanDataTable.Select(
                                                                                        "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_KewajibanRowList.Length > 0 Then
                                            _EksposurKewajibanRow.TagihanBersih = _ReportBI_1B_KewajibanRowList(0).TotalTagihanBersih
                                        Else
                                            _EksposurKewajibanRow.TagihanBersih = 0
                                        End If

                                        _ReportBI_1B_Kewajiban_JaminanRowList = Me.ReportBI_1B_Kewajiban_JaminanDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_Kewajiban_JaminanRowList.Length > 0 Then

                                            For Each _ReportBI_1B_KewajibanJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow In _ReportBI_1B_Kewajiban_JaminanRowList
                                                Select Case _ReportBI_1B_KewajibanJaminanRow.BobotResikoJaminan
                                                    Case 0
                                                        _EksposurKewajibanRow.TotalBagianDijamin_0 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 20
                                                        _EksposurKewajibanRow.TotalBagianDijamin_20 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 50
                                                        _EksposurKewajibanRow.TotalBagianDijamin_50 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                    Case 100
                                                        _EksposurKewajibanRow.TotalBagianDijamin_100 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                End Select
                                            Next

                                        Else
                                            _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                            _EksposurKewajibanRow.TotalBagianDijamin_100 = 0
                                        End If

                                        _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                                                        _EksposurKewajibanRow.TotalBagianDijamin_100)
                                        _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                        _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                            (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                        _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)
                                    Next
                                Case NamaPengelompokkan.NoGrouping
                                    _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                                    _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                    _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                    _EksposurKewajibanRow.NamaKategoriRincian = _MsJenisTagihanReportRow.NamaJenisTagihanReport
                                    _EksposurKewajibanRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                    _ReportBI_1B_KewajibanRowList = Me.ReportBI_1B_KewajibanDataTable.Select(
                                                                                    "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                    If _ReportBI_1B_KewajibanRowList.Length > 0 Then
                                        _EksposurKewajibanRow.TagihanBersih = _ReportBI_1B_KewajibanRowList(0).TotalTagihanBersih
                                    Else
                                        _EksposurKewajibanRow.TagihanBersih = 0
                                    End If

                                    _ReportBI_1B_Kewajiban_JaminanRowList = Me.ReportBI_1B_Kewajiban_JaminanDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                    If _ReportBI_1B_Kewajiban_JaminanRowList.Length > 0 Then

                                        For Each _ReportBI_1B_KewajibanJaminanRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_JaminanRow In _ReportBI_1B_Kewajiban_JaminanRowList

                                            Select Case _ReportBI_1B_KewajibanJaminanRow.BobotResikoJaminan
                                                Case 0
                                                    _EksposurKewajibanRow.TotalBagianDijamin_0 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                Case 20
                                                    _EksposurKewajibanRow.TotalBagianDijamin_20 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                Case 50
                                                    _EksposurKewajibanRow.TotalBagianDijamin_50 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                                Case 100
                                                    _EksposurKewajibanRow.TotalBagianDijamin_100 = _ReportBI_1B_KewajibanJaminanRow.TotalBagianDijamin
                                            End Select
                                        Next

                                    Else

                                        _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                                        _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                                        _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                                        _EksposurKewajibanRow.TotalBagianDijamin_100 = 0

                                    End If

                                    _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                                                    _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                                                    _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                                                    _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                                                    _EksposurKewajibanRow.TotalBagianDijamin_100)
                                    _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                                    _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                        (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                        (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                        (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                        (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                                    _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)
                                Case NamaPengelompokkan.IncludeAsSubReport

                            End Select
                        Else
                            _EksposurKewajibanRow = _EksposurKewajibanDataTable.NewRow

                            _EksposurKewajibanRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                            _EksposurKewajibanRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                            _EksposurKewajibanRow.NamaKategoriRincian = "N/A"
                            _EksposurKewajibanRow.BobotResiko = 0
                            _EksposurKewajibanRow.TagihanBersih = 0


                            _EksposurKewajibanRow.TotalBagianDijamin_0 = 0
                            _EksposurKewajibanRow.TotalBagianDijamin_20 = 0
                            _EksposurKewajibanRow.TotalBagianDijamin_50 = 0
                            _EksposurKewajibanRow.TotalBagianDijamin_100 = 0

                            _EksposurKewajibanRow.BagianTidakDijamin = _EksposurKewajibanRow.TagihanBersih - (
                                                                            _EksposurKewajibanRow.TotalBagianDijamin_0 +
                                                                            _EksposurKewajibanRow.TotalBagianDijamin_20 +
                                                                            _EksposurKewajibanRow.TotalBagianDijamin_50 +
                                                                            _EksposurKewajibanRow.TotalBagianDijamin_100)
                            _EksposurKewajibanRow.ATMRSebelumMRK = (_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.TagihanBersih
                            _EksposurKewajibanRow.ATMRSetelahMRK = ((_EksposurKewajibanRow.BobotResiko / 100) * _EksposurKewajibanRow.BagianTidakDijamin) +
                                (_EksposurKewajibanRow.TotalBagianDijamin_0 * 0) +
                                (_EksposurKewajibanRow.TotalBagianDijamin_20 * 0.2) +
                                (_EksposurKewajibanRow.TotalBagianDijamin_50 * 0.5) +
                                (_EksposurKewajibanRow.TotalBagianDijamin_100 * 1)

                            _EksposurKewajibanDataTable.AddEksposurKewajibanRow(_EksposurKewajibanRow)

                            '_IsReportDisplayed = False
                        End If
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajiban", CType(_EksposurKewajibanDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurKewajiban Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurKewajibanSummary()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurKewajiban

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()

            Dim _ReportBI_1B_KewajibanSummaryRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryRow()

            Dim _EksposurKewajibanSummaryRow As RWADAL.ReportATMRPerIndividual.EksposurKewajibanSummaryRow

            'Utk yg Sub Report
            Dim LstPkJenisTagihanReportIdSubReport As New List(Of Integer)
            Dim LstKelonggaranTarikSubReport As New List(Of Double)
            Dim LstTransaksiRekeningAdmSubReport As New List(Of Double)

            Dim LstPkJenisTagihanReportId As New List(Of Integer)
            Dim LstKelonggaranTarik As New List(Of Double)
            Dim LstTransaksiRekeningAdm As New List(Of Double)

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            _ReportBI_1B_KewajibanSummaryRowList = Me.ReportBI_1B_Kewajiban_SummaryDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                            'Cek, kalau Sebagai Sub Report, dijumlahkan ke parent di atasnya
                            Dim IsSubReport As Boolean = False
                            Using ObjDb As New NawaDatadevEntities
                                Dim ObjMsKategoriRincianPerhitunganATMR As List(Of RWA_MsKategoriRincianPerhitunganATMR) = ObjDb.RWA_MsKategoriRincianPerhitunganATMR.Where(Function(x) x.Fk_JenisTagihanReport_Id = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId).ToList
                                If ObjMsKategoriRincianPerhitunganATMR.Count > 0 Then
                                    IsSubReport = (ObjMsKategoriRincianPerhitunganATMR(0).Fk_JenisPengelompokkan_Id = 5)
                                End If
                            End Using

                            If _ReportBI_1B_KewajibanSummaryRowList.Length > 0 Then
                                For Each _ReportBI_1B_KewajibanSummaryRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryRow In _ReportBI_1B_KewajibanSummaryRowList
                                    If Not IsSubReport Then
                                        LstPkJenisTagihanReportId.Add(_ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    Else 'Sub Report
                                        Dim DblKelonggaranTarik As Double = 0
                                        Dim DblTransaksiRekeningAdm As Double = 0

                                        Select Case _ReportBI_1B_KewajibanSummaryRow.Fk_JenisEksposurKewajiban_Id
                                            Case 1
                                                DblKelonggaranTarik = _ReportBI_1B_KewajibanSummaryRow.TotalTRANetto
                                            Case 2
                                                DblTransaksiRekeningAdm = _ReportBI_1B_KewajibanSummaryRow.TotalTRANetto
                                        End Select

                                        If Not LstPkJenisTagihanReportIdSubReport.Contains(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId) Then
                                            LstPkJenisTagihanReportIdSubReport.Add(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId)
                                            LstKelonggaranTarikSubReport.Add(DblKelonggaranTarik)
                                            LstTransaksiRekeningAdmSubReport.Add(DblTransaksiRekeningAdm)
                                        Else
                                            Dim IntIndex As Integer = LstPkJenisTagihanReportIdSubReport.IndexOf(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId)

                                            LstKelonggaranTarikSubReport(IntIndex) = LstKelonggaranTarikSubReport(IntIndex) + DblKelonggaranTarik
                                            LstTransaksiRekeningAdmSubReport(IntIndex) = LstTransaksiRekeningAdmSubReport(IntIndex) + DblTransaksiRekeningAdm
                                        End If
                                    End If
                                Next
                            Else
                                If Not IsSubReport Then
                                    LstPkJenisTagihanReportId.Add(_ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                Else 'Sub Report
                                    Dim DblKelonggaranTarik As Double = 0
                                    Dim DblTransaksiRekeningAdm As Double = 0

                                    If Not LstPkJenisTagihanReportIdSubReport.Contains(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId) Then
                                        LstPkJenisTagihanReportIdSubReport.Add(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId)
                                        LstKelonggaranTarikSubReport.Add(DblKelonggaranTarik)
                                        LstTransaksiRekeningAdmSubReport.Add(DblTransaksiRekeningAdm)
                                    Else
                                        Dim IntIndex As Integer = LstPkJenisTagihanReportIdSubReport.IndexOf(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId)

                                        LstKelonggaranTarikSubReport(IntIndex) = LstKelonggaranTarikSubReport(IntIndex) + DblKelonggaranTarik
                                        LstTransaksiRekeningAdmSubReport(IntIndex) = LstTransaksiRekeningAdmSubReport(IntIndex) + DblTransaksiRekeningAdm
                                    End If
                                End If
                            End If
                        Next
                    Else
                        _ReportBI_1B_KewajibanSummaryRowList = ReportBI_1B_Kewajiban_SummaryDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                        If _ReportBI_1B_KewajibanSummaryRowList.Length > 0 Then
                            For Each _ReportBI_1B_KewajibanSummaryRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryRow In _ReportBI_1B_KewajibanSummaryRowList
                                LstPkJenisTagihanReportId.Add(_MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            Next
                        Else
                            LstPkJenisTagihanReportId.Add(_MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                        End If
                    End If
                End If
            Next

            For x As Integer = 0 To LstPkJenisTagihanReportId.Count - 1
                LstKelonggaranTarik.Add(0)
                LstTransaksiRekeningAdm.Add(0)
            Next

            _ReportBI_1B_KewajibanSummaryRowList = ReportBI_1B_Kewajiban_SummaryDataTable.Select("ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
            If _ReportBI_1B_KewajibanSummaryRowList.Length > 0 Then
                For Each _ReportBI_1B_KewajibanSummaryRow As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_SummaryRow In _ReportBI_1B_KewajibanSummaryRowList
                    If LstPkJenisTagihanReportId.Contains(_ReportBI_1B_KewajibanSummaryRow.Fk_MsJenisTagihanReport_Id) Then
                        Dim IntIndex As Integer = LstPkJenisTagihanReportId.IndexOf(_ReportBI_1B_KewajibanSummaryRow.Fk_MsJenisTagihanReport_Id)

                        Select Case _ReportBI_1B_KewajibanSummaryRow.Fk_JenisEksposurKewajiban_Id
                            Case 1  'LCU
                                LstKelonggaranTarik(IntIndex) = LstKelonggaranTarik(IntIndex) + _ReportBI_1B_KewajibanSummaryRow.TotalTRANetto
                            Case 2  'Adm Lainnya
                                LstTransaksiRekeningAdm(IntIndex) = LstTransaksiRekeningAdm(IntIndex) + _ReportBI_1B_KewajibanSummaryRow.TotalTRANetto
                        End Select
                    End If
                Next
            End If

            For x As Integer = 0 To LstPkJenisTagihanReportId.Count - 1
                _EksposurKewajibanSummaryRow = _EksposurKewajibanSummaryDataTable.NewRow

                _EksposurKewajibanSummaryRow.PK_MsJenisTagihanReportId = LstPkJenisTagihanReportId(x)
                _EksposurKewajibanSummaryRow.KelonggaranTarik = LstKelonggaranTarik(x)
                _EksposurKewajibanSummaryRow.TransaksiRekeningAdm = LstTransaksiRekeningAdm(x)

                _EksposurKewajibanSummaryDataTable.AddEksposurKewajibanSummaryRow(_EksposurKewajibanSummaryRow)
            Next

            'Add for sub report
            For x As Integer = 0 To LstPkJenisTagihanReportIdSubReport.Count - 1
                _EksposurKewajibanSummaryRow = _EksposurKewajibanSummaryDataTable.NewRow

                _EksposurKewajibanSummaryRow.PK_MsJenisTagihanReportId = LstPkJenisTagihanReportIdSubReport(x)
                _EksposurKewajibanSummaryRow.KelonggaranTarik = LstKelonggaranTarikSubReport(x)
                _EksposurKewajibanSummaryRow.TransaksiRekeningAdm = LstTransaksiRekeningAdmSubReport(x)

                _EksposurKewajibanSummaryDataTable.AddEksposurKewajibanSummaryRow(_EksposurKewajibanSummaryRow)
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajibanSummary", CType(_EksposurKewajibanSummaryDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurKewajibanSummary Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurKewajibanTRA()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurKewajiban

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()

            Dim _ReportBI_1B_KewajibanTRARowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Kewajiban_TRARow()
            'Utk Sub Report
            Dim LstIDName As New List(Of String)
            Dim LstPK_MsJenisTagihanReportId As New List(Of Integer)
            Dim LstTotalTRANetto As New List(Of Double)
            Dim LstTotalTagihanBersih As New List(Of Double)
            Dim LstNamaTRA As New List(Of String)
            Dim LstFKKValue As New List(Of Double)

            Dim _EksposurKewajibanTRARow As RWADAL.ReportATMRPerIndividual.EksposurKewajibanTRARow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList

                            '###########
                            If _ExcludedAdministratifMsJenisTagihanReportDataTable.FindByPK_MsJenisTagihanReportId(_ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId) IsNot Nothing Then

                                'Cek, kalau Sebagai Sub Report, dijumlahkan ke parent di atasnya
                                Dim IsSubReport As Boolean = False
                                Using ObjDb As New NawaDatadevEntities
                                    Dim ObjMsKategoriRincianPerhitunganATMR As List(Of RWA_MsKategoriRincianPerhitunganATMR) = ObjDb.RWA_MsKategoriRincianPerhitunganATMR.Where(Function(x) x.Fk_JenisTagihanReport_Id = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId).ToList
                                    If ObjMsKategoriRincianPerhitunganATMR.Count > 0 Then
                                        IsSubReport = (ObjMsKategoriRincianPerhitunganATMR(0).Fk_JenisPengelompokkan_Id = 5)
                                    End If
                                End Using

                                If Not IsSubReport Then
                                    For Each _MsJenisTransaksiRekeningAdministratifRow As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifRow In Me.MsJenisTransaksiRekeningAdministratifDataTable.Rows
                                        If _ExcludedTRADataTable.FindByPk_MsJenisTransaksiRekeningAdministratif_Id(_MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id) Is Nothing Then
                                            _EksposurKewajibanTRARow = _EksposurKewajibanTRADataTable.NewRow
                                            _EksposurKewajibanTRARow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId

                                            _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                                    "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                                            If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                                _EksposurKewajibanTRARow.TotalTRANetto = _ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto
                                                _EksposurKewajibanTRARow.TotalTagihanBersih = _ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih
                                            Else
                                                _EksposurKewajibanTRARow.TotalTRANetto = 0
                                                _EksposurKewajibanTRARow.TotalTagihanBersih = 0
                                            End If

                                            _EksposurKewajibanTRARow.NamaJenisTransaksiRekeningAdministratif = _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif
                                            _EksposurKewajibanTRARow.FKKValue = _MsJenisTransaksiRekeningAdministratifRow.FKKValue

                                            _EksposurKewajibanTRADataTable.AddEksposurKewajibanTRARow(_EksposurKewajibanTRARow)
                                        End If
                                    Next
                                Else    'Sub Report
                                    For Each _MsJenisTransaksiRekeningAdministratifRow As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifRow In Me.MsJenisTransaksiRekeningAdministratifDataTable.Rows
                                        If _ExcludedTRADataTable.FindByPk_MsJenisTransaksiRekeningAdministratif_Id(_MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id) Is Nothing Then
                                            If Not LstIDName.Contains(CStr(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId) & _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif) Then
                                                LstIDName.Add(CStr(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId) & _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif)

                                                LstPK_MsJenisTagihanReportId.Add(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId)

                                                _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                                        "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                        "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                                                If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                                    LstTotalTRANetto.Add(_ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto)
                                                    LstTotalTagihanBersih.Add(_ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih)
                                                Else
                                                    LstTotalTRANetto.Add(0)
                                                    LstTotalTagihanBersih.Add(0)
                                                End If

                                                LstNamaTRA.Add(_MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif)
                                                LstFKKValue.Add(_MsJenisTransaksiRekeningAdministratifRow.FKKValue)
                                            Else
                                                Dim IntIndex As Integer = LstIDName.IndexOf(CStr(_ChildMsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId) & _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif)

                                                _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                                        "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                        "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                                If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                                    LstTotalTRANetto(IntIndex) = LstTotalTRANetto(IntIndex) + _ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto
                                                    LstTotalTagihanBersih(IntIndex) = LstTotalTagihanBersih(IntIndex) + _ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            Else
                                For Each _MsJenisTransaksiRekeningAdministratifRow As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifRow In Me.MsJenisTransaksiRekeningAdministratifDataTable.Rows

                                    _EksposurKewajibanTRARow = _EksposurKewajibanTRADataTable.NewRow

                                    _EksposurKewajibanTRARow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId

                                    _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                            "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                            "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                            "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                            "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                                    If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                        _EksposurKewajibanTRARow.TotalTRANetto = _ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto
                                        _EksposurKewajibanTRARow.TotalTagihanBersih = _ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih
                                    Else
                                        _EksposurKewajibanTRARow.TotalTRANetto = 0
                                        _EksposurKewajibanTRARow.TotalTagihanBersih = 0
                                    End If

                                    _EksposurKewajibanTRARow.NamaJenisTransaksiRekeningAdministratif = _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif
                                    _EksposurKewajibanTRARow.FKKValue = _MsJenisTransaksiRekeningAdministratifRow.FKKValue

                                    _EksposurKewajibanTRADataTable.AddEksposurKewajibanTRARow(_EksposurKewajibanTRARow)
                                Next
                            End If
                            '##############
                        Next
                    Else
                        If _ExcludedAdministratifMsJenisTagihanReportDataTable.FindByPK_MsJenisTagihanReportId(_MsJenisTagihanReportRow.PK_MsJenisTagihanReportId) IsNot Nothing Then

                            For Each _MsJenisTransaksiRekeningAdministratifRow As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifRow In Me.MsJenisTransaksiRekeningAdministratifDataTable.Rows
                                If _ExcludedTRADataTable.FindByPk_MsJenisTransaksiRekeningAdministratif_Id(_MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id) Is Nothing Then
                                    _EksposurKewajibanTRARow = _EksposurKewajibanTRADataTable.NewRow

                                    _EksposurKewajibanTRARow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId

                                    _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                            "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                            "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                            "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                            "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                                    If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                        _EksposurKewajibanTRARow.TotalTRANetto = _ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto
                                        _EksposurKewajibanTRARow.TotalTagihanBersih = _ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih
                                    Else
                                        _EksposurKewajibanTRARow.TotalTRANetto = 0
                                        _EksposurKewajibanTRARow.TotalTagihanBersih = 0
                                    End If

                                    _EksposurKewajibanTRARow.NamaJenisTransaksiRekeningAdministratif = _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif
                                    _EksposurKewajibanTRARow.FKKValue = _MsJenisTransaksiRekeningAdministratifRow.FKKValue

                                    _EksposurKewajibanTRADataTable.AddEksposurKewajibanTRARow(_EksposurKewajibanTRARow)
                                End If
                            Next
                        Else
                            For Each _MsJenisTransaksiRekeningAdministratifRow As RWADAL.ReportATMRPerIndividual.MsJenisTransaksiRekeningAdministratifRow In Me.MsJenisTransaksiRekeningAdministratifDataTable.Rows

                                _EksposurKewajibanTRARow = _EksposurKewajibanTRADataTable.NewRow

                                _EksposurKewajibanTRARow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId

                                _ReportBI_1B_KewajibanTRARowList = Me.ReportBI_1B_Kewajiban_TRADataTable.Select(
                                        "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                        "Fk_MsJenisTRA_Id=" & _MsJenisTransaksiRekeningAdministratifRow.Pk_MsJenisTransaksiRekeningAdministratif_Id & " AND " &
                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                                If _ReportBI_1B_KewajibanTRARowList.Length > 0 Then
                                    _EksposurKewajibanTRARow.TotalTRANetto = _ReportBI_1B_KewajibanTRARowList(0).TotalTRANetto
                                    _EksposurKewajibanTRARow.TotalTagihanBersih = _ReportBI_1B_KewajibanTRARowList(0).TotalTagihanBersih
                                Else
                                    _EksposurKewajibanTRARow.TotalTRANetto = 0
                                    _EksposurKewajibanTRARow.TotalTagihanBersih = 0
                                End If

                                _EksposurKewajibanTRARow.NamaJenisTransaksiRekeningAdministratif = _MsJenisTransaksiRekeningAdministratifRow.NamaJenisTransaksiRekeningAdministratif
                                _EksposurKewajibanTRARow.FKKValue = _MsJenisTransaksiRekeningAdministratifRow.FKKValue

                                _EksposurKewajibanTRADataTable.AddEksposurKewajibanTRARow(_EksposurKewajibanTRARow)
                            Next
                        End If

                    End If
                End If
            Next

            For x As Integer = 0 To LstIDName.Count - 1
                _EksposurKewajibanTRARow = _EksposurKewajibanTRADataTable.NewRow

                _EksposurKewajibanTRARow.PK_MsJenisTagihanReportId = LstPK_MsJenisTagihanReportId(x)
                _EksposurKewajibanTRARow.TotalTRANetto = LstTotalTRANetto(x)
                _EksposurKewajibanTRARow.TotalTagihanBersih = LstTotalTagihanBersih(x)
                _EksposurKewajibanTRARow.NamaJenisTransaksiRekeningAdministratif = LstNamaTRA(x)
                _EksposurKewajibanTRARow.FKKValue = LstFKKValue(x)

                _EksposurKewajibanTRADataTable.AddEksposurKewajibanTRARow(_EksposurKewajibanTRARow)
            Next
            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajibanTRA", CType(_EksposurKewajibanTRADataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurKewajiban Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurCounterparty()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurCounterparty
            _JenisTagihanNumber = 1

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()

            Dim _MsKategoriRincianPerhitunganATMRRowList As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow()
            Dim _ReportBI_1B_CounterpartyRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_CounterpartyRow()

            Dim _EksposurCounterpartyRow As RWADAL.ReportATMRPerIndividual.EksposurCounterpartyRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                REM Get current Jenis Tagihan Report child record.

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                REM Check if current Jenis Tagihan Report record has child record.

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        _ChildNumber = "a"

                        REM current MsJenisTagihanReport record has child, then collect the related child.

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList

                            REM Get NamaJenisTagihanReport

                            _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                            _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                            _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                            _NamaJenisTagihanReport.Append(_ChildNumber & ".")
                            _NamaJenisTagihanReport.Append(" " & _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport)

                            REM Add NamaJenisTagihanReport into EksposurCounterparty data table

                            _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                            If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                                _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                                Select Case _NamaPengelompokkan
                                    Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                            _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                            _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                            _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                            _EksposurCounterpartyRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                            _EksposurCounterpartyRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                            _ReportBI_1B_CounterpartyRowList = Me.ReportBI_1B_CounterpartyDataTable.Select(
                                                "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                            If _ReportBI_1B_CounterpartyRowList.Length > 0 Then
                                                _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersih
                                                _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersihSetelahMRK
                                            Else
                                                _EksposurCounterpartyRow.TagihanBersih = 0
                                                _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0
                                            End If


                                            _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                                            _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK

                                            _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)
                                        Next
                                    Case NamaPengelompokkan.NoGrouping
                                        _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                        _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                        _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                        _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                        _EksposurCounterpartyRow.NamaKategoriRincian = _ChildMsJenisTagihanReportRow.NamaJenisTagihanReport
                                        _EksposurCounterpartyRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                        _ReportBI_1B_CounterpartyRowList = Me.ReportBI_1B_CounterpartyDataTable.Select(
                                                                                        "Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_CounterpartyRowList.Length > 0 Then
                                            _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersih
                                            _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersihSetelahMRK
                                        Else
                                            _EksposurCounterpartyRow.TagihanBersih = 0
                                            _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0
                                        End If

                                        _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                                        _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK

                                        _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)
                                    Case NamaPengelompokkan.IncludeAsSubReport

                                End Select

                                '_ChildNumber = Chr(Asc(_ChildNumber) + 1)
                            Else
                                _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                _EksposurCounterpartyRow.NamaKategoriRincian = "N/A"
                                _EksposurCounterpartyRow.BobotResiko = 0


                                _EksposurCounterpartyRow.TagihanBersih = 0
                                _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0

                                _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                                _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK


                                _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)

                                '_IsReportDisplayed = False

                                '_ChildNumber = Chr(Asc(_ChildNumber) + 1)
                            End If

                            _ChildNumber = Chr(Asc(_ChildNumber) + 1)
                        Next
                    Else
                        _NamaJenisTagihanReport.Remove(0, _NamaJenisTagihanReport.Length)
                        _NamaJenisTagihanReport.Append(_JenisReportNumber & ".")
                        _NamaJenisTagihanReport.Append(_JenisTagihanNumber & ".")
                        _NamaJenisTagihanReport.Append(" " & _MsJenisTagihanReportRow.NamaJenisTagihanReport)

                        _MsKategoriRincianPerhitunganATMRRowList = MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                        If _MsKategoriRincianPerhitunganATMRRowList.Length > 0 Then
                            _NamaPengelompokkan = _MsKategoriRincianPerhitunganATMRRowList(0).Fk_JenisPengelompokkan_Id

                            Select Case _NamaPengelompokkan
                                Case NamaPengelompokkan.Rating, NamaPengelompokkan.LTV, NamaPengelompokkan.GolonganDebitur
                                    _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    For Each _MsKategoriRincianPerhitunganATMRRow As RWADAL.ReportATMRPerIndividual.MsKategoriRincianPerhitunganATMRRow In _MsKategoriRincianPerhitunganATMRRowList
                                        _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                        _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                        _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                        _EksposurCounterpartyRow.NamaKategoriRincian = _MsKategoriRincianPerhitunganATMRRow.NamaKategoriRincian
                                        _EksposurCounterpartyRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRow.BobotResiko

                                        _ReportBI_1B_CounterpartyRowList = Me.ReportBI_1B_CounterpartyDataTable.Select(
                                                                                        "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                        "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRow.Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                        If _ReportBI_1B_CounterpartyRowList.Length > 0 Then
                                            _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersih
                                            _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersihSetelahMRK
                                        Else
                                            _EksposurCounterpartyRow.TagihanBersih = 0
                                            _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0
                                        End If

                                        _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                                        _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK


                                        _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)
                                    Next
                                Case NamaPengelompokkan.NoGrouping
                                    _MsKategoriRincianPerhitunganATMRRowList = Me.MsKategoriRincianPerhitunganATMRDataTable.Select("Fk_JenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)
                                    _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                                    _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                    _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                                    _EksposurCounterpartyRow.NamaKategoriRincian = _MsJenisTagihanReportRow.NamaJenisTagihanReport
                                    _EksposurCounterpartyRow.BobotResiko = _MsKategoriRincianPerhitunganATMRRowList(0).BobotResiko

                                    _ReportBI_1B_CounterpartyRowList = Me.ReportBI_1B_CounterpartyDataTable.Select(
                                                                                    "Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                                                    "Fk_MsKategoriRincianPerhitunganATMR_Id=" & _MsKategoriRincianPerhitunganATMRRowList(0).Pk_MsKategoriRincianPerhitunganATMR_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)

                                    If _ReportBI_1B_CounterpartyRowList.Length > 0 Then
                                        _EksposurCounterpartyRow.TagihanBersih = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersih
                                        _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = _ReportBI_1B_CounterpartyRowList(0).TotalTagihanBersihSetelahMRK
                                    Else
                                        _EksposurCounterpartyRow.TagihanBersih = 0
                                        _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0
                                    End If

                                    _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                                    _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK


                                    _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)
                                Case NamaPengelompokkan.IncludeAsSubReport

                            End Select
                        Else
                            _EksposurCounterpartyRow = _EksposurCounterpartyDataTable.NewRow

                            _EksposurCounterpartyRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                            _EksposurCounterpartyRow.NamaJenisTagihanReport = _NamaJenisTagihanReport.ToString
                            _EksposurCounterpartyRow.NamaKategoriRincian = "N/A"
                            _EksposurCounterpartyRow.BobotResiko = 0
                            _EksposurCounterpartyRow.TagihanBersih = 0
                            _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK = 0

                            _EksposurCounterpartyRow.ATMRSebelumMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TagihanBersih
                            _EksposurCounterpartyRow.ATMRSetelahMRK = (_EksposurCounterpartyRow.BobotResiko / 100) * _EksposurCounterpartyRow.TotalTagihanBersihSetelahMRK


                            _EksposurCounterpartyDataTable.AddEksposurCounterpartyRow(_EksposurCounterpartyRow)

                            '_IsReportDisplayed = False
                        End If
                    End If

                    _JenisTagihanNumber += 1
                End If
            Next

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterparty", CType(_EksposurCounterpartyDataTable, DataTable))
            Dim test As New ReportDataSource()
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurCounterparty Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurCounterpartySummary()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurCounterparty

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()

            Dim _ReportBI_1B_CounterpartySummaryRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_SummaryRow()
            Dim _EksposurCounterpartySummaryRow As RWADAL.ReportATMRPerIndividual.EksposurCounterpartySummaryRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            For Each JenisEksposurCounterPartyRow As RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyRow In Me.JenisEksposurCounterpartyDataTable.Rows
                                _EksposurCounterpartySummaryRow = _EksposurCounterpartySummaryDataTable.NewRow

                                _EksposurCounterpartySummaryRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                _EksposurCounterpartySummaryRow.NamaEksposurCounterparty = JenisEksposurCounterPartyRow.NamaEksposurCounterparty

                                _EksposurCounterpartySummaryRow.TotalTagihanDerivatif = 0
                                _EksposurCounterpartySummaryRow.TotalPotentialExposure = 0
                                _EksposurCounterpartySummaryRow.TotalTagihanBersih = 0


                                _ReportBI_1B_CounterpartySummaryRowList = Me.ReportBI_1B_Counterparty_SummaryDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "Fk_JenisEksposurCounterparty_Id = " & JenisEksposurCounterPartyRow.Pk_JenisEksposurCounterparty_Id)
                                If _ReportBI_1B_CounterpartySummaryRowList.Length > 0 Then
                                    _EksposurCounterpartySummaryRow.TotalTagihanDerivatif = _ReportBI_1B_CounterpartySummaryRowList(0).TotalTagihanDerivatif
                                    _EksposurCounterpartySummaryRow.TotalPotentialExposure = _ReportBI_1B_CounterpartySummaryRowList(0).TotalPotentialExposure
                                    _EksposurCounterpartySummaryRow.TotalTagihanBersih = _ReportBI_1B_CounterpartySummaryRowList(0).TotalTagihanBersih
                                End If

                                _EksposurCounterpartySummaryDataTable.AddEksposurCounterpartySummaryRow(_EksposurCounterpartySummaryRow)
                            Next
                        Next
                    Else
                        For Each JenisEksposurCounterPartyRow As RWADAL.ReportATMRPerIndividual.JenisEksposurCounterpartyRow In Me.JenisEksposurCounterpartyDataTable.Rows
                            _EksposurCounterpartySummaryRow = _EksposurCounterpartySummaryDataTable.NewRow

                            _EksposurCounterpartySummaryRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                            _EksposurCounterpartySummaryRow.NamaEksposurCounterparty = JenisEksposurCounterPartyRow.NamaEksposurCounterparty

                            _EksposurCounterpartySummaryRow.TotalTagihanDerivatif = 0
                            _EksposurCounterpartySummaryRow.TotalPotentialExposure = 0
                            _EksposurCounterpartySummaryRow.TotalTagihanBersih = 0


                            _ReportBI_1B_CounterpartySummaryRowList = Me.ReportBI_1B_Counterparty_SummaryDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "Fk_JenisEksposurCounterparty_Id = " & JenisEksposurCounterPartyRow.Pk_JenisEksposurCounterparty_Id)
                            If _ReportBI_1B_CounterpartySummaryRowList.Length > 0 Then
                                _EksposurCounterpartySummaryRow.TotalTagihanDerivatif = _ReportBI_1B_CounterpartySummaryRowList(0).TotalTagihanDerivatif
                                _EksposurCounterpartySummaryRow.TotalPotentialExposure = _ReportBI_1B_CounterpartySummaryRowList(0).TotalPotentialExposure
                                _EksposurCounterpartySummaryRow.TotalTagihanBersih = _ReportBI_1B_CounterpartySummaryRowList(0).TotalTagihanBersih
                            End If

                            _EksposurCounterpartySummaryDataTable.AddEksposurCounterpartySummaryRow(_EksposurCounterpartySummaryRow)
                        Next

                    End If
                End If
            Next

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterpartySummary", CType(_EksposurCounterpartySummaryDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurCounterpartySummary Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurCounterpartyPotentialFutureExposure()
        Try
            _JenisReportNumber = NamaJenisReport.EksposurCounterparty

            Dim _MsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()
            _MsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select("Fk_JenisReport_Id=" & _JenisReportNumber)
            Dim _ChildMsJenisTagihanReportRowList As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow()

            Dim _ReportBI_1B_Counterparty_PotentialFutureExposureRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_Counterparty_PotentialFutureExposureRow()
            Dim _EksposurCounterpartyPotentialFutureExposureRow As RWADAL.ReportATMRPerIndividual.EksposurCounterpartyPotentialFutureExposureRow

            For Each _MsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _MsJenisTagihanReportRowList

                _ChildMsJenisTagihanReportRowList = Me.MsJenisTagihanReportDatatable.Select(
                    "FK_JenisReport_Id=" & _JenisReportNumber & " " &
                    "AND FK_MsJenisTagihanReportParentId=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId)

                If _MsJenisTagihanReportRow.FK_MsJenisTagihanReportParentId = 0 Then
                    If _ChildMsJenisTagihanReportRowList.Length > 0 Then

                        For Each _ChildMsJenisTagihanReportRow As RWADAL.ReportATMRPerIndividual.MsJenisTagihanReportRow In _ChildMsJenisTagihanReportRowList
                            For Each KriteriaPotentialFutureEksposurRow As RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurRow In Me.KriteriaPotentialFutureEksposurDataTable.Rows
                                _EksposurCounterpartyPotentialFutureExposureRow = _EksposurCounterpartyPotentialFutureExposureDataTable.NewRow

                                _EksposurCounterpartyPotentialFutureExposureRow.PK_MsJenisTagihanReportId = _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                                _EksposurCounterpartyPotentialFutureExposureRow.SisaJangkaWaktu = KriteriaPotentialFutureEksposurRow.SisaJangkaWaktu
                                _EksposurCounterpartyPotentialFutureExposureRow.PersenSukuBunga = 0
                                _EksposurCounterpartyPotentialFutureExposureRow.PersenNilaiTukarDanEmas = 0


                                _ReportBI_1B_Counterparty_PotentialFutureExposureRowList = Me.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _ChildMsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                        "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString & " AND " &
                                                        "Fk_KriteriaPotentialFutureEksposur_Id = " & KriteriaPotentialFutureEksposurRow.Pk_KriteriaPotentialFutureEksposur_Id)
                                If _ReportBI_1B_Counterparty_PotentialFutureExposureRowList.Length > 0 Then
                                    _EksposurCounterpartyPotentialFutureExposureRow.PersenSukuBunga = _ReportBI_1B_Counterparty_PotentialFutureExposureRowList(0).TotalSukuBunga
                                    _EksposurCounterpartyPotentialFutureExposureRow.PersenNilaiTukarDanEmas = _ReportBI_1B_Counterparty_PotentialFutureExposureRowList(0).TotalNilaiTukarEmas
                                End If

                                _EksposurCounterpartyPotentialFutureExposureDataTable.AddEksposurCounterpartyPotentialFutureExposureRow(_EksposurCounterpartyPotentialFutureExposureRow)
                            Next
                        Next
                    Else
                        For Each KriteriaPotentialFutureEksposurRow As RWADAL.ReportATMRPerIndividual.KriteriaPotentialFutureEksposurRow In Me.KriteriaPotentialFutureEksposurDataTable.Rows
                            _EksposurCounterpartyPotentialFutureExposureRow = _EksposurCounterpartyPotentialFutureExposureDataTable.NewRow

                            _EksposurCounterpartyPotentialFutureExposureRow.PK_MsJenisTagihanReportId = _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId
                            _EksposurCounterpartyPotentialFutureExposureRow.SisaJangkaWaktu = KriteriaPotentialFutureEksposurRow.SisaJangkaWaktu
                            _EksposurCounterpartyPotentialFutureExposureRow.PersenSukuBunga = 0
                            _EksposurCounterpartyPotentialFutureExposureRow.PersenNilaiTukarDanEmas = 0


                            _ReportBI_1B_Counterparty_PotentialFutureExposureRowList = Me.ReportBI_1B_Counterparty_PotentialFutureExposureDataTable.Select("Fk_MsJenisTagihanReport_Id=" & _MsJenisTagihanReportRow.PK_MsJenisTagihanReportId & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "Fk_KriteriaPotentialFutureEksposur_Id = " & KriteriaPotentialFutureEksposurRow.Pk_KriteriaPotentialFutureEksposur_Id)
                            If _ReportBI_1B_Counterparty_PotentialFutureExposureRowList.Length > 0 Then
                                _EksposurCounterpartyPotentialFutureExposureRow.PersenSukuBunga = _ReportBI_1B_Counterparty_PotentialFutureExposureRowList(0).TotalSukuBunga
                                _EksposurCounterpartyPotentialFutureExposureRow.PersenNilaiTukarDanEmas = _ReportBI_1B_Counterparty_PotentialFutureExposureRowList(0).TotalNilaiTukarEmas
                            End If

                            _EksposurCounterpartyPotentialFutureExposureDataTable.AddEksposurCounterpartyPotentialFutureExposureRow(_EksposurCounterpartyPotentialFutureExposureRow)
                        Next
                    End If
                End If
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterpartyPotentialFutureExposure", CType(_EksposurCounterpartyPotentialFutureExposureDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurCounterpartyPotentialFutureExposure Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Private Sub GenerateReportEksposurSekuritisasi()
        Try
            Dim _MsBobotResikoSekuritisasiRowList As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiRow()
            Dim _ReportBI_1B_SekuritisasiRowList As RWADAL.ReportATMRPerIndividual.ReportBI_1B_SekuritisasiRow()
            Dim _EksposurSekuritisasiRow As RWADAL.ReportATMRPerIndividual.EksposurSekuritisasiRow

            _MsBobotResikoSekuritisasiRowList = Me.MsBobotResikoSekuritisasiDataTable.Select("IsUnrated=0")

            For Each _MsBobotResikoSekuritisasiRow As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiRow In _MsBobotResikoSekuritisasiRowList
                _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

                _EksposurSekuritisasiRow.NamaKategoriBobotResiko = _MsBobotResikoSekuritisasiRow.NamaKategoriBobotResiko
                _EksposurSekuritisasiRow.BobotResikoSekuritisasi = _MsBobotResikoSekuritisasiRow.BobotResikoSekuritisasi

                _ReportBI_1B_SekuritisasiRowList = Me.ReportBI_1B_SekuritisasiDataTable.Select("Fk_MsBobotResikoSekuritisasi_Id=" & _MsBobotResikoSekuritisasiRow.Pk_MsBobotResikoSekuritisasi_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                If _ReportBI_1B_SekuritisasiRowList.Length > 0 Then
                    _EksposurSekuritisasiRow.NilaiEksposur = _ReportBI_1B_SekuritisasiRowList(0).TotalTagihanBersih
                Else
                    _EksposurSekuritisasiRow.NilaiEksposur = 0

                    '_IsReportDisplayed = False
                End If

                _EksposurSekuritisasiRow.ATMR = (_EksposurSekuritisasiRow.BobotResikoSekuritisasi / 100) * _EksposurSekuritisasiRow.NilaiEksposur

                _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)
            Next

            REM Bobot resiko with IsUnrated status true (Tanpa Peringkat)

            _MsBobotResikoSekuritisasiRowList = Me.MsBobotResikoSekuritisasiDataTable.Select("IsUnrated=1")

            _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

            _EksposurSekuritisasiRow.NamaKategoriBobotResiko = "Tanpa Peringkat"
            _EksposurSekuritisasiRow.BobotResikoSekuritisasi = 0
            _EksposurSekuritisasiRow.NilaiEksposur = 0
            _EksposurSekuritisasiRow.ATMR = 0

            _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)

            For Each _MsBobotResikoSekuritisasiRow As RWADAL.ReportATMRPerIndividual.MsBobotResikoSekuritisasiRow In _MsBobotResikoSekuritisasiRowList
                _EksposurSekuritisasiRow = _EksposurSekuritisasiDataTable.NewRow

                _EksposurSekuritisasiRow.NamaKategoriBobotResiko = _MsBobotResikoSekuritisasiRow.NamaKategoriBobotResiko
                _EksposurSekuritisasiRow.BobotResikoSekuritisasi = _MsBobotResikoSekuritisasiRow.BobotResikoSekuritisasi

                _ReportBI_1B_SekuritisasiRowList = Me.ReportBI_1B_SekuritisasiDataTable.Select("Fk_MsBobotResikoSekuritisasi_Id=" & _MsBobotResikoSekuritisasiRow.Pk_MsBobotResikoSekuritisasi_Id & " AND " &
                                                    "ProcessMonth=" & Month(DatTanggalData.SelectedValue).ToString & " AND " &
                                                    "ProcessYear=" & Year(DatTanggalData.SelectedValue).ToString)
                If _ReportBI_1B_SekuritisasiRowList.Length > 0 Then
                    _EksposurSekuritisasiRow.NilaiEksposur = _ReportBI_1B_SekuritisasiRowList(0).TotalTagihanBersih
                Else
                    _EksposurSekuritisasiRow.NilaiEksposur = 0

                    '_IsReportDisplayed = False
                End If

                _EksposurSekuritisasiRow.ATMR = (_EksposurSekuritisasiRow.BobotResikoSekuritisasi / 100) * _EksposurSekuritisasiRow.NilaiEksposur

                _EksposurSekuritisasiDataTable.AddEksposurSekuritisasiRow(_EksposurSekuritisasiRow)
            Next

            REM Binding Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurSekuritisasi", CType(_EksposurSekuritisasiDataTable, DataTable))
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.LocalReport.Refresh()
        Catch ex As Exception
            Throw New Exception("ReportATMRPerIndividual.GenerateReportEksposurSekuritisasi Failed.<br />Message : " & ex.Message)
        End Try
    End Sub

    Protected Sub SubreportProcessingEvent(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        Try
            e.DataSources.Clear()

            REM Binding Sub Report Data Source

            Dim rds As New ReportDataSource("ReportATMRPerIndividual_EksposurAset", CType(_EksposurAsetDataTable, DataTable))
            e.DataSources.Add(rds)

            Dim rds_kewajiban As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajiban", CType(_EksposurKewajibanDataTable, DataTable))
            e.DataSources.Add(rds_kewajiban)

            Dim rds_kewajiban_summary As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajibanSummary", CType(_EksposurKewajibanSummaryDataTable, DataTable))
            e.DataSources.Add(rds_kewajiban_summary)

            Dim rds_kewajiban_tra As New ReportDataSource("ReportATMRPerIndividual_EksposurKewajibanTRA", CType(_EksposurKewajibanTRADataTable, DataTable))
            e.DataSources.Add(rds_kewajiban_tra)

            Dim rds_counterparty As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterparty", CType(_EksposurCounterpartyDataTable, DataTable))
            e.DataSources.Add(rds_counterparty)

            Dim rds_counterpartysummary As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterpartySummary", CType(_EksposurCounterpartySummaryDataTable, DataTable))
            e.DataSources.Add(rds_counterpartysummary)

            Dim rds_counterpartypotentialfutureexposure As New ReportDataSource("ReportATMRPerIndividual_EksposurCounterpartyPotentialFutureExposure", CType(_EksposurCounterpartyPotentialFutureExposureDataTable, DataTable))
            e.DataSources.Add(rds_counterpartypotentialfutureexposure)

            Dim rds_sekuritisasi As New ReportDataSource("ReportATMRPerIndividual_EksposurSekuritisasi", CType(_EksposurSekuritisasiDataTable, DataTable))
            e.DataSources.Add(rds_sekuritisasi)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
            LogError(ex)
        End Try
    End Sub

End Class
