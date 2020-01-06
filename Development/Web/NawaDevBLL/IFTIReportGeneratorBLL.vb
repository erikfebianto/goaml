Imports Ionic.Zip
Imports Ionic.Zlib
Imports System.IO
Imports System.Xml
Imports System.Text.RegularExpressions
Public Class IFTIReportGeneratorBLL

    Enum TransactionType
        SwiftOutgoing = 1
        SwiftIncoming
        NonSwiftOutgoing
        NonSwiftIncoming
        All
    End Enum

    Enum ReportFormat
        Excel = 1
        XML
    End Enum


    Shared Function GetFileDalam1Zip() As Integer

        Dim objparam As NawaDAL.SystemParameter = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(4013)
        If Not objparam Is Nothing Then
            Return objparam.SettingValue
        Else
            Return 500

        End If



    End Function

    Shared Function GetIdentitasPengirimIndividu(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/noRekening", FillOrDefault(objifti.Sender_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/namaLengkap", FillOrDefault(objifti.Sender_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/tglLahir", FillOrDefault(objifti.Sender_Nasabah_INDV_TanggalLahir, "datetime"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/kewarganegaraan/wargaNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_KewargaNegaraan.GetValueOrDefault(1), "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/kewarganegaraan/idNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/kewarganegaraan/negaraLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NegaraLainnya, "string2"))


        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/alamatSesuaiVoucher/alamat", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/alamatSesuaiVoucher/negaraBagianKota", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_NegaraKota, "string2"))
        If objifti.Sender_Nasabah_INDV_ID_Negara.HasValue Then
            SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/alamatSesuaiVoucher/idNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Negara, "int"))
        End If
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/alamatSesuaiVoucher/negaraLain", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_NegaraLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/noTelp", FillOrDefault(objifti.Sender_Nasabah_INDV_NoTelp, "string2"))
        Select Case objifti.Sender_Nasabah_INDV_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 2
                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/sim", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 3
                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/passport", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 4
                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 5
                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/npwp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 6
                If ObjectAntiNull(objifti.Sender_Nasabah_INDV_NomorID) = True Then
                    SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/perorangan/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
        End Select



    End Function

    Shared Function GetIdentitasPengirimKorporasi(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/noRekening", FillOrDefault(objifti.Sender_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/namaKorporasi", FillOrDefault(objifti.Sender_Nasabah_CORP_NamaKorporasi, "string2"))

        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/alamatSesuaiVoucher/alamat", FillOrDefault(objifti.Sender_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/alamatSesuaiVoucher/negaraBagianKota", FillOrDefault(objifti.Sender_Nasabah_CORP_NegaraKota, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/alamatSesuaiVoucher/idNegara", FillOrDefault(objifti.Sender_Nasabah_CORP_Negara, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/alamatSesuaiVoucher/negaraLain", FillOrDefault(objifti.Sender_Nasabah_CORP_NegaraLainnya, "string2"))

        SetNodeValue(xdoc, "ifti/identitasPengirim/nasabah/korporasi/noTelp", FillOrDefault(objifti.Sender_Nasabah_CORP_NoTelp, "string2"))





    End Function

    Shared Function GetIdentitasPenerimaIndividu(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean

        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xNode, "namaLengkap", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xNode, "tglLahir", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_TanggalLahir, "datetime"))
        SetNodeValue(xNode, "kewarganegaraan/wargaNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_KewargaNegaraan, "int"))
        SetNodeValue(xNode, "kewarganegaraan/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xNode, "kewarganegaraan/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NegaraLainnya, "string2"))
        SetNodeValue(xNode, "pekerjaan", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_Pekerjaan, "int"))
        SetNodeValue(xNode, "pekerjaanLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_PekerjaanLainnya, "string2"))

        SetNodeValue(xNode, "alamatDomisili/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Alamat_Dom, "string2"))
        SetNodeValue(xNode, "alamatDomisili/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Propinsi_Dom, "int"))
        SetNodeValue(xNode, "alamatDomisili/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_PropinsiLainnya_Dom, "string2"))
        SetNodeValue(xNode, "alamatDomisili/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKab_Dom, "int"))
        SetNodeValue(xNode, "alamatDomisili/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKabLainnya_Dom, "string2"))


        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Alamat_Iden, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Propinsi_Iden, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_PropinsiLainnya_Iden, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKab_Iden, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKabLainnya_Iden, "string2"))

        SetNodeValue(xNode, "noTelp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NoTelp, "string2"))


        Select Case objiftibeneficiary.Beneficiary_Nasabah_INDV_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string2"))
                SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))
            Case 2
                SetNodeValue(xNode, "buktiIdentitas/sim", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 3
                SetNodeValue(xNode, "buktiIdentitas/passport", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 4
                SetNodeValue(xNode, "buktiIdentitas/kimsKitasKitap", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 5
                SetNodeValue(xNode, "buktiIdentitas/npwp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 6
                If ObjectAntiNull(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID) = True Then
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xNode, "buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string2"))

        End Select
        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NilaiTransaksiKeuangan, "decimal"))


    End Function

    Shared Function GetSwiftIncomingSenderNonNasabah(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/noRekening", FillOrDefault(objifti.Sender_NonNasabah_NoRekening, "string3"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/namaBank", FillOrDefault(objifti.Sender_NonNasabah_NamaBank, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/namaLengkap", FillOrDefault(objifti.Sender_NonNasabah_NamaLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/tglLahir", FillOrDefault(objifti.Sender_NonNasabah_TanggalLahir, "datetime"))

        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/alamatSesuaiVoucher/alamat", FillOrDefault(objifti.Sender_NonNasabah_ID_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/alamatSesuaiVoucher/negaraBagianKota", FillOrDefault(objifti.Sender_NonNasabah_ID_NegaraBagian, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/alamatSesuaiVoucher/idNegara", FillOrDefault(objifti.Sender_NonNasabah_ID_Negara, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/alamatSesuaiVoucher/negaraLain", FillOrDefault(objifti.Sender_NonNasabah_ID_NegaraLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/nonNasabah/noTelp", FillOrDefault(objifti.Sender_NonNasabah_NoTelp, "string2"))


    End Function

    Shared Function GetIdentitasPenerimaKorporasi(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean


        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(xNode, "namaKorporasi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NamaKorporasi, "string2"))
        SetNodeValue(xNode, "bentukBadan", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBentukBadanUsaha_Id, "int"))
        SetNodeValue(xNode, "bentukBadanLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BentukBadanUsahaLainnya, "string2"))
        SetNodeValue(xNode, "bidangUsaha", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBidangUsaha_Id, "int"))
        SetNodeValue(xNode, "bidangUsahaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BidangUsahaLainnya, "string2"))


        SetNodeValue(xNode, "alamatLengkapKorporasi/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_Propinsi, "int"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_PropinsiLainnya, "string2"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_KotaKab, "int"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_KotaKabLainnya, "string2"))


        SetNodeValue(xNode, "noTelp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NoTelp, "string2"))
        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NilaiTransaksiKeuangan, "decimal"))



    End Function

    Shared Function GetIdentitasPenerimaNonNasabahSwiftIncoming(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean

        SetNodeValue(xNode, "namaLengkap", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NamaLengkap, "string2"))
        SetNodeValue(xNode, "tglLahir", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_TanggalLahir, "datetime"))
        SetNodeValue(xNode, "alamat", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_ID_Alamat, "string2"))
        SetNodeValue(xNode, "noTelp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NoTelp, "string2"))


        Select Case objiftibeneficiary.Beneficiary_NonNasabah_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string2"))
                SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))
            Case 2
                SetNodeValue(xNode, "buktiIdentitas/sim", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 3
                SetNodeValue(xNode, "buktiIdentitas/passport", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 4
                SetNodeValue(xNode, "buktiIdentitas/kimsKitasKitap", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 5
                SetNodeValue(xNode, "buktiIdentitas/npwp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 6
                If ObjectAntiNull(objiftibeneficiary.Beneficiary_NonNasabah_NomorID) = True Then
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xNode, "buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string2"))


        End Select


    End Function

    Shared Function GenerateFileIFTISwiftIncoming(ByVal objifti As NawaDevDAL.IFTI, ByVal strpath As String) As Boolean
        Dim xdoc As XmlDocument

        Try
            xdoc = New XmlDocument
            xdoc.PreserveWhitespace = True
            xdoc.Load(strpath)

            SetNodeValue(xdoc, "ifti/localId", FillOrDefault(objifti.LocalID, "string3"))
            SetNodeValue(xdoc, "ifti/umum/tglLaporan", FillOrDefault(objifti.TanggalLaporan.GetValueOrDefault(Now), "datetime"))
            SetNodeValue(xdoc, "ifti/umum/namaPejabatPjk", FillOrDefault(objifti.NamaPejabatPJKBankPelapor, "string"))
            If objifti.JenisLaporan.GetValueOrDefault(0) = 1 Then
                SetNodeValue(xdoc, "ifti/umum/jenisLaporan", objifti.JenisLaporan.GetValueOrDefault(1).ToString)

            ElseIf objifti.JenisLaporan.GetValueOrDefault(0) = 2 Then
                SetNodeValue(xdoc, "ifti/umum/jenisLaporan", objifti.JenisLaporan.GetValueOrDefault(1).ToString)
                SetNodeValue(xdoc, "ifti/umum/noLtklKoreksi", FillOrDefault(objifti.LTDLNNoKoreksi, "string2"))
            End If
            SetNodeValue(xdoc, "ifti/pjkBankSebagai", FillOrDefault(objifti.Sender_FK_PJKBank_Type.GetValueOrDefault(1), "int"))

            If objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 1 Or objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 2 Or objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 4 Or objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 5 Then

                Select Case objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0)
                    Case 1, 4 'individu (nasabah) 
                        GetIdentitasPengirimIndividu(objifti, xdoc)
                    Case 2, 5 'korporasi (nasabah)
                        GetIdentitasPengirimKorporasi(objifti, xdoc)
                End Select
            ElseIf objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 3 Or objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 6 Then
                GetSwiftIncomingSenderNonNasabah(objifti, xdoc)
            End If

            Dim xmlPenerimaAkhirNasabahIndividu As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/perorangan")
            Dim xmlPenerimaAkhirNasabahKorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/korporasi")
            Dim xmlPenerimaAkhirNonNasabah As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nonNasabah")

            Dim xmlPenerimaPenerusNasabahIndividu As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerus/perorangan")
            Dim xmlPenerimaPenerusNasabahkorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerus/korporasi")


            Dim xmlPenerimaAkhirNasabahIndividuClone As XmlNode = xmlPenerimaAkhirNasabahIndividu.Clone
            Dim xmlPenerimaAkhirNasabahKorporasiClone As XmlNode = xmlPenerimaAkhirNasabahKorporasi.Clone
            Dim xmlPenerimaAkhirNonNasabahClone As XmlNode = xmlPenerimaAkhirNonNasabah.Clone

            Dim xmlPenerimaPenerusNasabahIndividuClone As XmlNode = xmlPenerimaPenerusNasabahIndividu.Clone
            Dim xmlPenerimaPenerusNasabahkorporasiClone As XmlNode = xmlPenerimaPenerusNasabahkorporasi.Clone

            'penanda sudah kepake atau belum.kalau sudah kepake.copy dari clone
            Dim FlagPenerimaAkhirNasabahIndividu As Boolean = False
            Dim FlagPenerimaAkhirNasabahKorporasi As Boolean = False
            Dim FlagPenerimaAkhirNonNasabah As Boolean = False

            Dim FlagPenerimaPenerusIndividu As Boolean = False
            Dim FlagPenerimaPeneruskorporasi As Boolean = False



            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities

                Dim objTlistIftiBeneficiary As List(Of NawaDevDAL.IFTI_Beneficiary) = objDb.IFTI_Beneficiary.Where(Function(x) x.FK_IFTI_ID = objifti.PK_IFTI_ID).ToList
                For Each objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary In objTlistIftiBeneficiary

                    If objiftibeneficiary.PJKBank_type.GetValueOrDefault(0) = 1 Then 'asal /penerima akhir

                        If objiftibeneficiary.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 1 Then
                            If FlagPenerimaAkhirNasabahIndividu = False Then
                                Dim xLastNodePenerimaakhirNasabahIndividu As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/perorangan")
                                GetIdentitasPenerimaIndividu(objiftibeneficiary, xLastNodePenerimaakhirNasabahIndividu)
                                FlagPenerimaAkhirNasabahIndividu = True
                            Else
                                Dim objListNodePenerimaAkhirNasabahIndividu As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/perorangan")
                                objListNodePenerimaAkhirNasabahIndividu(objListNodePenerimaAkhirNasabahIndividu.Count - 1).ParentNode.InsertAfter(xmlPenerimaAkhirNasabahIndividuClone, objListNodePenerimaAkhirNasabahIndividu(objListNodePenerimaAkhirNasabahIndividu.Count - 1))

                                Dim xLastNodePenerimaakhirListNasabahIndividu As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/perorangan")
                                Dim indexAkhir As Integer = xLastNodePenerimaakhirListNasabahIndividu.Count - 1
                                GetIdentitasPenerimaIndividu(objiftibeneficiary, xLastNodePenerimaakhirListNasabahIndividu(indexAkhir))

                            End If
                        ElseIf objiftibeneficiary.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 2 Then
                            If FlagPenerimaAkhirNasabahKorporasi = False Then
                                Dim xLastNodePenerimaakhirNasabahkorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/korporasi")
                                GetIdentitasPenerimaKorporasi(objiftibeneficiary, xLastNodePenerimaakhirNasabahkorporasi)
                                FlagPenerimaAkhirNasabahKorporasi = True
                            Else
                                Dim objListNodePenerimaAkhirNasabahkorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/korporasi")
                                objListNodePenerimaAkhirNasabahkorporasi(objListNodePenerimaAkhirNasabahkorporasi.Count - 1).ParentNode.InsertAfter(xmlPenerimaAkhirNasabahKorporasiClone, objListNodePenerimaAkhirNasabahkorporasi(objListNodePenerimaAkhirNasabahkorporasi.Count - 1))

                                Dim xLastNodePenerimaakhirListNasabahkorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nasabah/korporasi")
                                Dim indexAkhir As Integer = xLastNodePenerimaakhirListNasabahkorporasi.Count - 1
                                GetIdentitasPenerimaKorporasi(objiftibeneficiary, xLastNodePenerimaakhirListNasabahkorporasi(indexAkhir))

                            End If


                        ElseIf objiftibeneficiary.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 3 Then
                            If FlagPenerimaAkhirNonNasabah = False Then
                                Dim xLastNodePenerimaakhirNonNasabah As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nonNasabah")
                                GetIdentitasPenerimaNonNasabahSwiftIncoming(objiftibeneficiary, xLastNodePenerimaakhirNonNasabah)
                                FlagPenerimaAkhirNonNasabah = True
                            Else
                                Dim objListNodePenerimaAkhirNonNasabah As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nonNasabah")
                                objListNodePenerimaAkhirNonNasabah(objListNodePenerimaAkhirNonNasabah.Count - 1).ParentNode.InsertAfter(xmlPenerimaAkhirNonNasabahClone, objListNodePenerimaAkhirNonNasabah(objListNodePenerimaAkhirNonNasabah.Count - 1))

                                Dim xLastNodePenerimaakhirListNonNasabah As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerimaAkhir/nonNasabah")
                                Dim indexAkhir As Integer = xLastNodePenerimaakhirListNonNasabah.Count - 1
                                GetIdentitasPenerimaNonNasabahSwiftIncoming(objiftibeneficiary, xLastNodePenerimaakhirListNonNasabah(indexAkhir))

                            End If

                        End If



                    ElseIf objiftibeneficiary.PJKBank_type.GetValueOrDefault(0) = 2 Then 'penerus


                        If objiftibeneficiary.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 4 Then
                            If FlagPenerimaPenerusIndividu = False Then
                                Dim xLastNodePenerimaPenerusIndividu As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerus/perorangan")
                                GetIdentitasPenerimaIndividuSwiftIncomingPenyelengaraPenerus(objiftibeneficiary, xLastNodePenerimaPenerusIndividu)
                                FlagPenerimaPenerusIndividu = True
                            Else
                                Dim objListNodePenerimaPenerusIndividu As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerus/perorangan")
                                objListNodePenerimaPenerusIndividu(objListNodePenerimaPenerusIndividu.Count - 1).ParentNode.InsertAfter(xmlPenerimaPenerusNasabahIndividuClone, objListNodePenerimaPenerusIndividu(objListNodePenerimaPenerusIndividu.Count - 1))

                                Dim xLastNodePenerimaPenerusListIndividu As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerus/perorangan")
                                Dim indexAkhir As Integer = xLastNodePenerimaPenerusListIndividu.Count - 1
                                GetIdentitasPenerimaIndividuSwiftIncomingPenyelengaraPenerus(objiftibeneficiary, xLastNodePenerimaPenerusListIndividu(indexAkhir))

                            End If
                        ElseIf objiftibeneficiary.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 5 Then
                            If FlagPenerimaPeneruskorporasi = False Then
                                Dim xLastNodePenerimaPenerusKorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/penyelenggaraPenerus/korporasi")
                                GetIdentitasPenerimaKorporasiSwiftIncomingPenyelengaraPenerus(objiftibeneficiary, xLastNodePenerimaPenerusKorporasi)
                                FlagPenerimaPeneruskorporasi = True
                            Else
                                Dim objListNodePenerimaPeneruskorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerus/korporasi")
                                objListNodePenerimaPeneruskorporasi(objListNodePenerimaPeneruskorporasi.Count - 1).ParentNode.InsertAfter(xmlPenerimaPenerusNasabahkorporasiClone, objListNodePenerimaPeneruskorporasi(objListNodePenerimaPeneruskorporasi.Count - 1))

                                Dim xLastNodePenerimaPenerusListKorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/penyelenggaraPenerus/korporasi")
                                Dim indexAkhir As Integer = xLastNodePenerimaPenerusListKorporasi.Count - 1
                                GetIdentitasPenerimaKorporasiSwiftIncomingPenyelengaraPenerus(objiftibeneficiary, xLastNodePenerimaPenerusListKorporasi(indexAkhir))

                            End If

                        End If



                    End If

                Next

            End Using


            GetTransaksiSwiftIncoming(objifti, xdoc)
            GetInformasiLainnya(objifti, xdoc)

            RemoveEndTag(xdoc)
            xdoc.Save(strpath)

        Catch ex As Exception
            Throw
        Finally
            If Not xdoc Is Nothing Then
                xdoc = Nothing
            End If
        End Try
    End Function

    Shared Function GetTransaksiSwiftIncoming(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        SetNodeValue(xdoc, "ifti/transaksi/tglTransaksi", FillOrDefault(objifti.TanggalTransaksi, "datetime"))
        SetNodeValue(xdoc, "ifti/transaksi/timeIndication", FillOrDefault(objifti.TimeIndication, "string"))
        SetNodeValue(xdoc, "ifti/transaksi/sendersReference", FillOrDefault(objifti.SenderReference, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/bankOperationCode", FillOrDefault(objifti.BankOperationCode, "string"))
        SetNodeValue(xdoc, "ifti/transaksi/instructionCode", FillOrDefault(objifti.InstructionCode, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/kanCabPenyelenggaraPengirimAsal", FillOrDefault(objifti.KantorCabangPenyelengaraPengirimAsal, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/typeTransactionCode", FillOrDefault(objifti.TransactionCode, "string2"))

        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/valueDate", FillOrDefault(objifti.ValueDate_TanggalTransaksi, "datetime"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/amount", FillOrDefault(objifti.ValueDate_NilaiTransaksi, "decimal"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/currency", FillOrDefault(objifti.ValueDate_FK_Currency_ID, "int"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/currencyLain", FillOrDefault(objifti.ValueDate_CurrencyLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/amountDalamRupiah", FillOrDefault(objifti.ValueDate_NilaiTransaksiIDR, "decimal"))


        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/currency", FillOrDefault(objifti.Instructed_Currency, "int"))
        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/currencyLain", FillOrDefault(objifti.Instructed_CurrencyLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/instructedAmount", FillOrDefault(objifti.Instructed_Amount, "decimal"))

        SetNodeValue(xdoc, "ifti/transaksi/exchangeRate", FillOrDefault(objifti.ExchangeRate, "decimal"))
        SetNodeValue(xdoc, "ifti/transaksi/sendingInstitution", FillOrDefault(objifti.SendingInstitution, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/tujuanTransaksi", FillOrDefault(objifti.TujuanTransaksi, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/sumberDana", FillOrDefault(objifti.SumberPenggunaanDana, "string2"))




    End Function

    Shared Function GetIdentitasPenerimaKorporasiSwiftIncomingPenyelengaraPenerus(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef XNode As XmlNode) As Boolean


        SetNodeValue(XNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(XNode, "namaBank", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NamaBank, "string2"))
        SetNodeValue(XNode, "namaKorporasi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NamaKorporasi, "string2"))
        SetNodeValue(XNode, "bentukBadan", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBentukBadanUsaha_Id, "int"))
        SetNodeValue(XNode, "bentukBadanLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BentukBadanUsahaLainnya, "string2"))
        SetNodeValue(XNode, "bidangUsaha", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBidangUsaha_Id, "int"))
        SetNodeValue(XNode, "bidangUsahaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BidangUsahaLainnya, "string2"))


        SetNodeValue(XNode, "alamatLengkapKorporasi/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(XNode, "alamatLengkapKorporasi/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_Propinsi, "int"))
        SetNodeValue(XNode, "alamatLengkapKorporasi/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_PropinsiLainnya, "string2"))
        SetNodeValue(XNode, "alamatLengkapKorporasi/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_KotaKab, "int"))
        SetNodeValue(XNode, "alamatLengkapKorporasi/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_KotaKabLainnya, "string2"))


        SetNodeValue(XNode, "noTelp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NoTelp, "string2"))
        SetNodeValue(XNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NilaiTransaksiKeuangan, "decimal"))


    End Function

    Shared Function GetIdentitasPenerimaIndividuSwiftIncomingPenyelengaraPenerus(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean


        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xNode, "namaBank", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NamaBank, "string2"))
        SetNodeValue(xNode, "namaLengkap", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xNode, "tglLahir", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_TanggalLahir, "datetime"))
        SetNodeValue(xNode, "kewarganegaraan/wargaNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_KewargaNegaraan, "int"))
        SetNodeValue(xNode, "kewarganegaraan/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xNode, "kewarganegaraan/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NegaraLainnya, "string2"))
        SetNodeValue(xNode, "pekerjaan", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_Pekerjaan, "int"))
        SetNodeValue(xNode, "pekerjaanLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_PekerjaanLainnya, "string2"))

        SetNodeValue(xNode, "alamatDomisili/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Alamat_Dom, "string2"))
        SetNodeValue(xNode, "alamatDomisili/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Propinsi_Dom, "int"))
        SetNodeValue(xNode, "alamatDomisili/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_PropinsiLainnya_Dom, "string2"))
        SetNodeValue(xNode, "alamatDomisili/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKab_Dom, "int"))
        SetNodeValue(xNode, "alamatDomisili/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKabLainnya_Dom, "string2"))


        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Alamat_Iden, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Propinsi_Iden, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_PropinsiLainnya_Iden, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKab_Iden, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_KotaKabLainnya_Iden, "string2"))

        SetNodeValue(xNode, "noTelp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NoTelp, "string2"))


        Select Case objiftibeneficiary.Beneficiary_Nasabah_INDV_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string2"))
                SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))
            Case 2
                SetNodeValue(xNode, "buktiIdentitas/sim", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 3
                SetNodeValue(xNode, "buktiIdentitas/passport", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 4
                SetNodeValue(xNode, "buktiIdentitas/kimsKitasKitap", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 5
                SetNodeValue(xNode, "buktiIdentitas/npwp", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string3"))

            Case 6
                If ObjectAntiNull(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID) = True Then
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xNode, "buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NomorID, "string2"))

        End Select

        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NilaiTransaksiKeuangan, "decimal"))

    End Function


    Shared Function GenerateReportXMLSwiftIncoming(ByVal DStartdate As Date, ByVal LastUpdateDate As Date, ByVal strDirPath As String, ByVal strDirPathTemplate As String) As String
        Dim Processdate As Date

        Dim lstFile As New List(Of String)
        Dim lstFileZip As New List(Of String)
        Dim intjmlFiledalam1zip As Integer
        Dim strfilenamexml As String
        Dim fileTemplateName As String = "ifti-swift-incoming.xml"
        Dim intcounter As Integer
        Dim FileNameZipReturn As String = ""
        Try
            intjmlFiledalam1zip = GetFileDalam1Zip()
            Processdate = DStartdate


            intcounter = 0
            lstFile.Clear()



            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities

                'Dim objifti As List(Of NawaDevDAL.IFTI) = objDb.IFTIs.Where(Function(x) x.FK_IFTI_Type_ID = 2 And x.IsDataValid = True And System.Data.Entity.DbFunctions.TruncateTime(x.TanggalTransaksi) = System.Data.Entity.DbFunctions.TruncateTime(Processdate) And x.LastUpdateDate <= LastUpdateDate And x.LastUpdateDate >= LastUpdateDate).ToList
                Dim objifti As List(Of NawaDevDAL.IFTI) = objDb.IFTIs.Where(Function(x) x.FK_IFTI_Type_ID = 2 And x.IsDataValid = True And System.Data.Entity.DbFunctions.TruncateTime(x.TanggalTransaksi) = System.Data.Entity.DbFunctions.TruncateTime(Processdate) And System.Data.Entity.DbFunctions.TruncateTime(x.LastUpdateDate) = System.Data.Entity.DbFunctions.TruncateTime(LastUpdateDate)).ToList


                'DataRepository.IFTIProvider.GetPaged("FK_IFTI_Type_ID = 2 and IsDataValid = 1 and TanggalTransaksi = '" & Processdate.ToString("yyyy-MM-dd") & "' and datediff(day,LastUpdateDate,'" & LastUpdateDate.ToString("yyyy-MM-dd") & "')=0", "", 0, Integer.MaxValue, 0)

                For Each Item As NawaDevDAL.IFTI In objifti
                    strfilenamexml = Item.LocalID.Replace("/", "").Replace("\", "") & "-Swift-Incoming.xml"
                    If File.Exists(strDirPath & strfilenamexml) Then
                        IO.File.Delete(strDirPath & strfilenamexml)
                    End If
                    IO.File.Copy(strDirPathTemplate & fileTemplateName, strDirPath & strfilenamexml)
                    GenerateFileIFTISwiftIncoming(Item, strDirPath & strfilenamexml)

                    lstFile.Add(strDirPath & strfilenamexml)

                    If lstFile.Count = intjmlFiledalam1zip And lstFile.Count > 0 Then
                        Using zip As New ZipFile()
                            intcounter += 1
                            Dim fileNamezip As String = strDirPath & Processdate.ToString("yyyyMMdd") & "-" & Right("00" & intcounter, 3) & "-SwiftIncoming.zip"
                            If IO.File.Exists(fileNamezip) Then
                                IO.File.Delete(fileNamezip)
                            End If

                            zip.CompressionLevel = CompressionLevel.Level9
                            zip.AddFiles(lstFile, "")

                            zip.Save(fileNamezip)
                            lstFileZip.Add(fileNamezip)
                        End Using

                        'buang karena sudah di zip
                        For Each Item1 As String In lstFile
                            IO.File.Delete(Item1)
                        Next



                        lstFile.Clear()
                    End If
                Next
                If lstFile.Count > 0 Then

                        Using zip As New ZipFile()
                            intcounter += 1
                        Dim fileNamezip As String = strDirPath & Processdate.ToString("yyyyMMdd") & "-" & Right("00" & intcounter, 3) & "-" & LastUpdateDate.ToString("yyyyMMdd") & "-SwiftIncoming.zip"
                        If IO.File.Exists(fileNamezip) Then
                                IO.File.Delete(fileNamezip)
                            End If

                            zip.CompressionLevel = CompressionLevel.Level9
                            zip.AddFiles(lstFile, "")
                            zip.Save(fileNamezip)
                            lstFileZip.Add(fileNamezip)
                        End Using

                        'buang karena sudah di zip
                        For Each Item1 As String In lstFile
                            IO.File.Delete(Item1)
                        Next
                        lstFile.Clear()
                    End If



                End Using

                If lstFileZip.Count > 1 Then
                Using zip As New ZipFile()

                    Dim dfiledatename As String
                    dfiledatename = DStartdate.ToString("yyyyMMdd")


                    Dim fileNamezip As String = strDirPath & dfiledatename & "-All-" & LastUpdateDate.ToString("yyyyMMdd") & "-SwiftIncoming.zip"
                    If IO.File.Exists(fileNamezip) Then
                        IO.File.Delete(fileNamezip)
                    End If
                    zip.CompressionLevel = CompressionLevel.Level9
                    zip.AddFiles(lstFileZip, "")
                    zip.Save(fileNamezip)

                    For Each Item1 As String In lstFileZip
                        IO.File.Delete(Item1)
                    Next

                    FileNameZipReturn = fileNamezip
                End Using
            Else
                FileNameZipReturn = lstFileZip.Item(0).ToString
                'If lstFileZip.Count <> 0 Then
                '    FileNameZipReturn = lstFileZip.Item(0).ToString
                'Else
                '    FileNameZipReturn = ""
                'End If
            End If

            Return FileNameZipReturn

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Shared Function GenerateReportXMLSwiftOutgoing(ByVal DStartdate As Date, ByVal LastUpdatedate As Date, ByVal strDirPath As String, ByVal strdirPathTemplate As String) As String
        Dim lstFile As New List(Of String)
        Dim lstFileZip As New List(Of String)
        Dim intcounter As Integer
        Dim FileNameZipReturn As String = ""

        Dim Processdate As Date
        Dim intjmlFiledalam1zip As Integer
        Dim strfilenamexml As String
        Dim fileTemplateName As String = "ifti-swift-outgoing.xml"

        Try
            intjmlFiledalam1zip = GetFileDalam1Zip()
            Processdate = DStartdate

            intcounter = 0
            lstFile.Clear()


            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities


                'Dim objifti As List(Of NawaDevDAL.IFTI) = objDb.IFTIs.Where(Function(x) x.FK_IFTI_Type_ID = 1 And x.IsDataValid = True And System.Data.Entity.DbFunctions.TruncateTime(x.TanggalTransaksi) = System.Data.Entity.DbFunctions.TruncateTime(Processdate) And x.LastUpdateDate <= LastUpdatedate And x.LastUpdateDate >= LastUpdatedate).ToList
                Dim objifti As List(Of NawaDevDAL.IFTI) = objDb.IFTIs.Where(Function(x) x.FK_IFTI_Type_ID = 1 And x.IsDataValid = True And System.Data.Entity.DbFunctions.TruncateTime(x.TanggalTransaksi) = System.Data.Entity.DbFunctions.TruncateTime(Processdate) And System.Data.Entity.DbFunctions.TruncateTime(x.LastUpdateDate) = System.Data.Entity.DbFunctions.TruncateTime(LastUpdatedate)).ToList




                For Each Item As NawaDevDAL.IFTI In objifti
                    strfilenamexml = Item.LocalID.Replace("/", "").Replace("\", "") & "-Swift-Outgoing.xml"
                    If File.Exists(strDirPath & strfilenamexml) Then
                        IO.File.Delete(strDirPath & strfilenamexml)
                    End If
                    IO.File.Copy(strdirPathTemplate & fileTemplateName, strDirPath & strfilenamexml)
                    GenerateFileIFTISwiftOutgoing(Item, strDirPath & strfilenamexml)

                    lstFile.Add(strDirPath & strfilenamexml)

                    If lstFile.Count = intjmlFiledalam1zip And lstFile.Count > 0 Then
                        Using zip As New ZipFile()
                            intcounter += 1
                            Dim fileNamezip As String = strDirPath & Processdate.ToString("yyyyMMdd") & "-" & Right("00" & intcounter, 3) & "-SwiftOutgoing.zip"
                            If IO.File.Exists(fileNamezip) Then
                                IO.File.Delete(fileNamezip)
                            End If
                            zip.CompressionLevel = CompressionLevel.Level9
                            zip.AddFiles(lstFile, "")

                            zip.Save(fileNamezip)
                            lstFileZip.Add(fileNamezip)
                        End Using

                        'buang karena sudah di zip
                        For Each Item1 As String In lstFile
                            IO.File.Delete(Item1)
                        Next



                        lstFile.Clear()
                    End If

                Next

                If lstFile.Count > 0 Then

                    Using zip As New ZipFile()
                        intcounter += 1
                        Dim fileNamezip As String = strDirPath & Processdate.ToString("yyyyMMdd") & "-" & Right("00" & intcounter, 3) & "-" & LastUpdatedate.ToString("yyyyMMdd") & "-SwiftOutgoing.zip"
                        If IO.File.Exists(fileNamezip) Then
                            IO.File.Delete(fileNamezip)
                        End If
                        zip.CompressionLevel = CompressionLevel.Level9
                        zip.AddFiles(lstFile, "")
                        zip.Save(fileNamezip)
                        lstFileZip.Add(fileNamezip)
                    End Using

                    'buang karena sudah di zip
                    For Each Item1 As String In lstFile
                        IO.File.Delete(Item1)
                    Next
                    lstFile.Clear()
                End If

            End Using


            If lstFileZip.Count > 1 Then
                Using zip As New ZipFile()

                    Dim dfiledatename As String
                    dfiledatename = DStartdate.ToString("yyyyMMdd")


                    Dim fileNamezip As String = strDirPath & dfiledatename & "-All-" & LastUpdatedate.ToString("yyyyMMdd") & "-SwiftOutgoing.zip"
                    If IO.File.Exists(fileNamezip) Then
                        IO.File.Delete(fileNamezip)
                    End If
                    zip.CompressionLevel = CompressionLevel.Level9
                    zip.AddFiles(lstFileZip, "")
                    zip.Save(fileNamezip)

                    For Each Item1 As String In lstFileZip
                        IO.File.Delete(Item1)
                    Next

                    FileNameZipReturn = fileNamezip
                End Using
            Else
                FileNameZipReturn = lstFileZip.Item(0).ToString
                'If lstFileZip.Count <> 0 Then
                '    FileNameZipReturn = lstFileZip.Item(0).ToString
                'Else
                '    FileNameZipReturn = ""
                'End If
            End If

            Return FileNameZipReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Shared Sub SetNodeValue(ByRef xdoc As XmlNode, ByVal objtag As String, ByVal objvalue As String)
        Dim xNode As XmlNode
        xNode = xdoc.SelectSingleNode(objtag)

        If Not xNode Is Nothing Then

            xNode.InnerText = objvalue



        End If
    End Sub
    Public Shared Function CleanAnySpecialCharacterAndSpasi(ByVal target As String) As String
        Dim _s As String = Regex.Replace(target, "[^a-zA-Z0-9]", "", RegexOptions.Singleline)
        Return cleanAnyDoubleSpaces(_s)
    End Function

    Public Shared Function cleanAnyDoubleSpaces(ByVal target As String) As String
        Dim _s As String = Regex.Replace(target, "[\s]{2,}", " ", RegexOptions.Singleline)
        Return _s.Trim
    End Function

    Public Shared Function CleanAnySpecialCharacter(ByVal target As String) As String
        Dim _s As String = Regex.Replace(target, "[^a-zA-Z0-9 ]", " ", RegexOptions.Singleline)
        Return cleanAnyDoubleSpaces(_s)
    End Function
    Private Shared Function FillOrDefault(ByVal strValueData As Object, ByVal dataType As String) As String
        Dim retVal As String = ""
        Try
            If Not strValueData Is Nothing Then
                Select Case dataType.Trim().ToLower()
                    Case "string"
                        If strValueData Is Nothing Then
                            retVal = ""
                        Else
                            If CStr(strValueData).ToString().Trim().Length <> 0 Then
                                retVal = strValueData.Trim()
                            End If
                        End If
                    Case "string2"
                        If strValueData Is Nothing Then
                            retVal = ""
                        Else
                            If CStr(strValueData).ToString().Trim().Length <> 0 Then
                                retVal = CleanAnySpecialCharacter(strValueData.Trim())
                            End If
                        End If
                    Case "string3"
                        If strValueData Is Nothing Then
                            retVal = ""
                        Else
                            If CStr(strValueData).ToString().Trim().Length <> 0 Then
                                retVal = CleanAnySpecialCharacterAndSpasi(strValueData.Trim())
                            End If
                        End If
                    Case "bigint"
                        If strValueData Is Nothing Then
                            retVal = ""
                        ElseIf strValueData = 0 Then
                            retVal = ""
                        Else
                            retVal = CLng(strValueData).ToString().Trim()
                        End If
                    Case "int"
                        If strValueData Is Nothing Then
                            retVal = ""
                        ElseIf strValueData = 0 Then
                            retVal = ""
                        Else
                            retVal = CInt(strValueData).ToString().Trim()
                        End If
                    Case "int2"
                        If strValueData Is Nothing Then
                            retVal = ""
                        ElseIf strValueData = 0 Then
                            retVal = ""
                        ElseIf strValueData = "12" Then
                            retVal = ""
                        Else
                            retVal = CInt(strValueData).ToString().Trim()
                        End If
                    Case "decimal"
                        If strValueData Is Nothing Then
                            retVal = ""
                        ElseIf strValueData = 0 Then
                            retVal = ""
                        Else
                            retVal = CDec(strValueData).ToString().Trim()
                        End If
                    Case "datetime"
                        If strValueData Is Nothing Then
                            retVal = ""
                        ElseIf strValueData = "0001-01-01" Then
                            retVal = ""
                        ElseIf strValueData = "1900-01-01" Then
                            retVal = ""
                        Else
                            retVal = CDate(strValueData).ToString("dd/MM/yyyy").Trim()
                        End If
                    Case "decimal2"
                        If strValueData Is Nothing Then
                            retVal = "0"
                        Else
                            retVal = CDec(strValueData).ToString().Trim()
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw
        End Try
        Return retVal
    End Function

    Public Shared Function ObjectAntiNull(ByVal obj As Object) As Boolean
        Dim Temp As Boolean

        If IsDBNull(obj) Then
            Temp = False
        ElseIf IsNothing(obj) Then
            Temp = False
        ElseIf CStr(obj).Trim = "" Then
            Temp = False
        Else
            Temp = True
        End If

        Return Temp
    End Function

    Shared Function GetSenderSwiftOutgoingPerorangan(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/noRekening", FillOrDefault(objifti.Sender_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/namaLengkap", FillOrDefault(objifti.Sender_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/tglLahir", FillOrDefault(objifti.Sender_Nasabah_INDV_TanggalLahir, "datetime"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/kewarganegaraan/wargaNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_KewargaNegaraan.GetValueOrDefault(1), "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/kewarganegaraan/idNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/kewarganegaraan/negaraLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NegaraLainnya, "string2"))

        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/pekerjaan", FillOrDefault(objifti.Sender_Nasabah_INDV_Pekerjaan, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/pekerjaanLain", FillOrDefault(objifti.Sender_Nasabah_INDV_PekerjaanLainnya, "string2"))

        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatDomisili/alamat", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatDomisili/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_Propinsi, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatDomisili/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_PropinsiLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatDomisili/idKabKota", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_KotaKab, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatDomisili/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_KotaKabLainnya, "string2"))


        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Propinsi, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_PropinsiLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_KotaKab, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_KotaKabLainnya, "string2"))


        Select Case objifti.Sender_Nasabah_INDV_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 2
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/sim", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 3
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/passport", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 4
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 5
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/npwp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 6
                If ObjectAntiNull(objifti.Sender_Nasabah_INDV_NomorID) = True Then
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/perorangan/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
        End Select



    End Function

    Shared Function GetSenderSwiftOutgoingKorporasi(ByVal objifti As NawaDevDAL.IFTI, ByRef xDoc As XmlDocument) As Boolean


        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/noRekening", FillOrDefault(objifti.Sender_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/namaKorporasi", FillOrDefault(objifti.Sender_Nasabah_CORP_NamaKorporasi, "string2"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/bentukBadan", FillOrDefault(objifti.Sender_Nasabah_CORP_FK_MsBentukBadanUsaha_Id, "int"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/bentukBadanLain", FillOrDefault(objifti.Sender_Nasabah_CORP_BentukBadanUsahaLainnya, "string2"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/bidangUsaha", FillOrDefault(objifti.Sender_Nasabah_CORP_FK_MsBidangUsaha_Id, "int"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/bidangUsahaLain", FillOrDefault(objifti.Sender_Nasabah_CORP_BidangUsahaLainnya, "string2"))

        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatLengkapKorporasi/alamat", FillOrDefault(objifti.Sender_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatLengkapKorporasi/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_CORP_Propinsi, "int"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatLengkapKorporasi/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_CORP_PropinsiLainnya, "string2"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatLengkapKorporasi/idKabKota", FillOrDefault(objifti.Sender_Nasabah_CORP_KotaKab, "int"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatLengkapKorporasi/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_CORP_KotaKabLainnya, "string2"))



        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatKorporasiLuarNegeri", FillOrDefault(objifti.Sender_Nasabah_CORP_LN_AlamatLengkap, "string2"))
        SetNodeValue(xDoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nasabah/korporasi/alamatKanCabAsal", FillOrDefault(objifti.Sender_Nasabah_CORP_Alamat_KantorCabangPengirim, "string2"))




        'writer.WriteEndElement() 'korporasi
    End Function
    Shared Function GetSenderSwiftOutgoingNonNasabah(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        If objifti.FK_IFTI_NonNasabahNominalType_ID.GetValueOrDefault(0) = 1 Then
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/namaLengkap", FillOrDefault(objifti.Sender_NonNasabah_NamaLengkap, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/tglLahir", FillOrDefault(objifti.Sender_NonNasabah_TanggalLahir, "datetime"))


            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objifti.Sender_NonNasabah_ID_Alamat, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objifti.Sender_NonNasabah_ID_Propinsi, "int"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objifti.Sender_NonNasabah_ID_PropinsiLainnya, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objifti.Sender_NonNasabah_ID_KotaKab, "int"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objifti.Sender_NonNasabah_ID_KotaKabLainnya, "string2"))


            Select Case objifti.Sender_NonNasabah_FK_IFTI_IDType.GetValueOrDefault(0)
                Case 1
                    'SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string2"))
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 2
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/sim", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 3
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/passport", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 4
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 5
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/npwp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 6
                    If ObjectAntiNull(objifti.Sender_NonNasabah_NomorID) = True Then
                        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                    Else
                        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                    End If
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/kurangDari100Juta/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string2"))

            End Select
        ElseIf objifti.FK_IFTI_NonNasabahNominalType_ID.GetValueOrDefault(0) = 2 Then
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/namaLengkap", FillOrDefault(objifti.Sender_NonNasabah_NamaLengkap, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/tglLahir", FillOrDefault(objifti.Sender_NonNasabah_TanggalLahir, "datetime"))


            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objifti.Sender_NonNasabah_ID_Alamat, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objifti.Sender_NonNasabah_ID_Propinsi, "int"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objifti.Sender_NonNasabah_ID_PropinsiLainnya, "string2"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objifti.Sender_NonNasabah_ID_KotaKab, "int"))
            SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objifti.Sender_NonNasabah_ID_KotaKabLainnya, "string2"))


            Select Case objifti.Sender_NonNasabah_FK_IFTI_IDType.GetValueOrDefault(0)
                Case 1
                    'SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string2"))
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 2
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/sim", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 3
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/passport", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 4
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 5
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/npwp", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string3"))
                Case 6
                    If ObjectAntiNull(objifti.Sender_NonNasabah_NomorID) = True Then
                        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                    Else
                        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                    End If

                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPengirimAsal/nonNasabah/lebihDari100Juta/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.Sender_NonNasabah_NomorID, "string2"))

            End Select
        End If



    End Function

    Shared Function GetSenderSwiftOutgoingIndividuPenyelengaraPenerus(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean




        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/noRekening", FillOrDefault(objifti.Sender_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xdoc, "nifti/identitasPengirim/penyelenggaraPenerus/perorangan/namaBank", FillOrDefault(objifti.Sender_Nasabah_INDV_NamaBank, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/namaLengkap", FillOrDefault(objifti.Sender_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/tglLahir", FillOrDefault(objifti.Sender_Nasabah_INDV_TanggalLahir, "datetime"))
        'writer.WriteStartElement("kewarganega  raan")

        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/kewarganegaraan/wargaNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_KewargaNegaraan, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/kewarganegaraan/idNegara", FillOrDefault(objifti.Sender_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/kewarganegaraan/negaraLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NegaraLainnya, "string2"))
        'writer.WriteEndElement() 'kewarganegaraan

        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/pekerjaan", FillOrDefault(objifti.Sender_Nasabah_INDV_Pekerjaan, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/pekerjaanLain", FillOrDefault(objifti.Sender_Nasabah_INDV_PekerjaanLainnya, "string2"))

        'writer.WriteStartElement("alamatDomisili")
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatDomisili/alamat", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatDomisili/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_Propinsi, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatDomisili/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_PropinsiLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatDomisili/idKabKota", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_KotaKab, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatDomisili/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_INDV_DOM_KotaKabLainnya, "string2"))
        'writer.WriteEndElement() 'alamatDomisili



        'writer.WriteStartElement("alamatSesuaiBuktiIdentitas")
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Alamat, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_Propinsi, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_PropinsiLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_KotaKab, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_INDV_ID_KotaKabLainnya, "string2"))
        'writer.WriteEndElement()


        Select Case objifti.Sender_Nasabah_INDV_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/ktp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 2
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/sim", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 3
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/passport", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 4
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 5
                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/npwp", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string3"))
            Case 6
                If ObjectAntiNull(objifti.Sender_Nasabah_INDV_NomorID) = True Then
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/perorangan/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.Sender_Nasabah_INDV_NomorID, "string2"))
        End Select

    End Function
    Shared Function GetSenderSwiftOutgoingKorporasiPenyelengaraPenerus(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean



        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/noRekening", FillOrDefault(objifti.Sender_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/namaBank", FillOrDefault(objifti.Sender_Nasabah_CORP_NamaBank, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/namaKorporasi", FillOrDefault(objifti.Sender_Nasabah_CORP_NamaKorporasi, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/bentukBadan", FillOrDefault(objifti.Sender_Nasabah_CORP_FK_MsBentukBadanUsaha_Id, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/bentukBadanLain", FillOrDefault(objifti.Sender_Nasabah_CORP_BentukBadanUsahaLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/bidangUsaha", FillOrDefault(objifti.Sender_Nasabah_CORP_FK_MsBidangUsaha_Id, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/bidangUsahaLain", FillOrDefault(objifti.Sender_Nasabah_CORP_BidangUsahaLainnya, "string2"))

        'writer.WriteStartElement("alamatLengkapKorporasi")
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatLengkapKorporasi/alamat", FillOrDefault(objifti.Sender_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatLengkapKorporasi/idPropinsi", FillOrDefault(objifti.Sender_Nasabah_CORP_Propinsi, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatLengkapKorporasi/propinsiLain", FillOrDefault(objifti.Sender_Nasabah_CORP_PropinsiLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatLengkapKorporasi/idKabKota", FillOrDefault(objifti.Sender_Nasabah_CORP_KotaKab, "int"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatLengkapKorporasi/kabKotaLain", FillOrDefault(objifti.Sender_Nasabah_CORP_KotaKabLainnya, "string2"))

        'writer.WriteEndElement() 'alamatLengkapKorporasi


        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatKorporasiLuarNegeri", FillOrDefault(objifti.Sender_Nasabah_CORP_LN_AlamatLengkap, "string2"))
        SetNodeValue(xdoc, "ifti/identitasPengirim/penyelenggaraPenerus/korporasi/alamatKanCabAsal", FillOrDefault(objifti.Sender_Nasabah_CORP_Alamat_KantorCabangPengirim, "string2"))



        'writer.WriteEndElement() 'korporasi
    End Function

    Shared Function GetBeneficialOwnerSwiftOutgoing(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        SetNodeValue(xdoc, "ifti/keterlibatanBeneficialOwner", FillOrDefault(objifti.FK_IFTI_KeterlibatanBeneficialOwner_ID.GetValueOrDefault(1), "int"))

        If objifti.FK_IFTI_KeterlibatanBeneficialOwner_ID.GetValueOrDefault(0) = 1 Then
            SetNodeValue(xdoc, "ifti/beneficialOwner/hubDgnPemilikDana", FillOrDefault(objifti.BeneficialOwner_HubunganDenganPemilikDana, "string2"))
            If objifti.FK_IFTI_BeneficialOwnerType_ID.GetValueOrDefault(0) = 1 Then 'nasabah

                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/noRekening", FillOrDefault(objifti.BeneficialOwner_NoRekening, "string3"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/namaLengkap", FillOrDefault(objifti.BeneficialOwner_NamaLengkap, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/tglLahir", FillOrDefault(objifti.BeneficialOwner_TanggalLahir, "datetime"))

                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/alamatSesuaiBuktiIdentitas/alamat", FillOrDefault(objifti.BeneficialOwner_ID_Alamat, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/alamatSesuaiBuktiIdentitas/idPropinsi", FillOrDefault(objifti.BeneficialOwner_ID_Propinsi, "int"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/alamatSesuaiBuktiIdentitas/propinsiLain", FillOrDefault(objifti.BeneficialOwner_ID_PropinsiLainnya, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/alamatSesuaiBuktiIdentitas/idKabKota", FillOrDefault(objifti.BeneficialOwner_ID_KotaKab, "int"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/alamatSesuaiBuktiIdentitas/kabKotaLain", FillOrDefault(objifti.BeneficialOwner_ID_KotaKabLainnya, "string2"))

                Select Case objifti.BeneficialOwner_FK_IFTI_IDType.GetValueOrDefault(0)
                    Case 1
                        'SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/ktp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string2"))
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/ktp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 2
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/sim", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 3
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/passport", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 4
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 5
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/npwp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 6
                        If ObjectAntiNull(objifti.BeneficialOwner_NomorID) = True Then
                            SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                        Else
                            SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                        End If

                        SetNodeValue(xdoc, "ifti/beneficialOwner/nasabah/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.BeneficialOwner_NomorID, "string2"))
                End Select

            ElseIf objifti.FK_IFTI_BeneficialOwnerType_ID.GetValueOrDefault(0) = 2 Then 'nonnasabah
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/namaLengkap", FillOrDefault(objifti.BeneficialOwner_NamaLengkap, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/tglLahir", FillOrDefault(objifti.BeneficialOwner_TanggalLahir, "datetime"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/alamatSesuaiBuktiIdentitasVoucher/alamat", FillOrDefault(objifti.BeneficialOwner_ID_Alamat, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/alamatSesuaiBuktiIdentitasVoucher/idPropinsi", FillOrDefault(objifti.BeneficialOwner_ID_Propinsi, "int"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/alamatSesuaiBuktiIdentitasVoucher/propinsiLain", FillOrDefault(objifti.BeneficialOwner_ID_PropinsiLainnya, "string2"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/alamatSesuaiBuktiIdentitasVoucher/idKabKota", FillOrDefault(objifti.BeneficialOwner_ID_KotaKab, "int"))
                SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/alamatSesuaiBuktiIdentitasVoucher/kabKotaLain", FillOrDefault(objifti.BeneficialOwner_ID_KotaKabLainnya, "string2"))


                Select Case objifti.BeneficialOwner_FK_IFTI_IDType.GetValueOrDefault(0)
                    Case 1
                        'SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/ktp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string2"))
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/ktp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 2
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/sim", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 3
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/passport", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 4
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/kimsKitasKitap", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 5
                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/npwp", FillOrDefault(objifti.BeneficialOwner_NomorID, "string3"))
                    Case 6
                        If ObjectAntiNull(objifti.BeneficialOwner_NomorID) = True Then
                            SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                        Else
                            SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/buktiLain/jenisBuktiLain", "")
                        End If

                        SetNodeValue(xdoc, "ifti/beneficialOwner/nonNasabah/buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objifti.BeneficialOwner_NomorID, "string2"))
                End Select

            End If
        ElseIf objifti.FK_IFTI_KeterlibatanBeneficialOwner_ID.GetValueOrDefault(0) = 2 Then
            'tidak ada beneficial owner
        End If




    End Function

    Shared Function GenerateIdentitasPenerimaIndividuSwiftOutgoing(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean
        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NoRekening, "string3"))
        SetNodeValue(xNode, "namaLengkap", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NamaLengkap, "string2"))
        SetNodeValue(xNode, "tglLahir", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_TanggalLahir, "datetime"))
        SetNodeValue(xNode, "kewarganegaraan/wargaNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_KewargaNegaraan, "int"))
        SetNodeValue(xNode, "kewarganegaraan/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_Negara, "int"))
        SetNodeValue(xNode, "kewarganegaraan/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NegaraLainnya, "string2"))




        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Alamat_Iden, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/negaraBagianKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_NegaraBagian, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_Negara, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_ID_NegaraLainnya, "string2"))



        Select Case objiftibeneficiary.Beneficiary_NonNasabah_FK_IFTI_IDType.GetValueOrDefault(0)
            Case 1
                'SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string2"))
                SetNodeValue(xNode, "buktiIdentitas/ktp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))
            Case 2
                SetNodeValue(xNode, "buktiIdentitas/sim", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 3
                SetNodeValue(xNode, "buktiIdentitas/passport", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 4
                SetNodeValue(xNode, "buktiIdentitas/kimsKitasKitap", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 5
                SetNodeValue(xNode, "buktiIdentitas/npwp", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string3"))

            Case 6
                If ObjectAntiNull(objiftibeneficiary.Beneficiary_NonNasabah_NomorID) = True Then
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "Others")
                Else
                    SetNodeValue(xNode, "buktiIdentitas/buktiLain/jenisBuktiLain", "")
                End If

                SetNodeValue(xNode, "buktiIdentitas/buktiLain/noBuktiLain", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NomorID, "string2"))


        End Select
        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_INDV_NilaiTransaksiKeuangan, "decimal"))
    End Function


    Shared Function GenerateIdentitasPenerimaKorporasiSwiftOutgoing(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean

        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NoRekening, "string3"))
        SetNodeValue(xNode, "namaBank", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NamaBank, "string2"))
        SetNodeValue(xNode, "namaKorporasi", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NamaKorporasi, "string2"))
        SetNodeValue(xNode, "bentukBadan", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBentukBadanUsaha_Id, "int"))
        SetNodeValue(xNode, "bentukBadanLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BentukBadanUsahaLainnya, "string2"))
        SetNodeValue(xNode, "bidangUsaha", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_FK_MsBidangUsaha_Id, "int"))
        SetNodeValue(xNode, "bidangUsahaLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_BidangUsahaLainnya, "string2"))


        SetNodeValue(xNode, "alamatLengkapKorporasi/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_AlamatLengkap, "string2"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/negaraBagianKota", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_NegaraBagian, "string2"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_Negara, "int"))
        SetNodeValue(xNode, "alamatLengkapKorporasi/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_ID_NegaraLainnya, "string2"))

        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_Nasabah_CORP_NilaiTransaksiKeuangan, "decimal"))

    End Function

    Shared Function GenerateIdentitasPenerimaNonNasabahSwiftOutgoing(ByVal objiftibeneficiary As NawaDevDAL.IFTI_Beneficiary, ByRef xNode As XmlNode) As Boolean
        SetNodeValue(xNode, "noRekening", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NoRekening, "string3"))
        SetNodeValue(xNode, "namaBank", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NamaBank, "string2"))
        SetNodeValue(xNode, "namaLengkap", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NamaLengkap, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/alamat", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_ID_Alamat, "string2"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/idNegara", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_ID_Negara, "int"))
        SetNodeValue(xNode, "alamatSesuaiBuktiIdentitasVoucher/negaraLain", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_ID_NegaraLainnya, "string2"))
        SetNodeValue(xNode, "nilaiTransaksiDalamRupiah", FillOrDefault(objiftibeneficiary.Beneficiary_NonNasabah_NilaiTransaksikeuangan, "decimal"))
    End Function

    Shared Function GenerateFileIFTISwiftOutgoing(ByVal objifti As NawaDevDAL.IFTI, ByVal strpath As String) As Boolean
        Dim xdoc As XmlDocument
        Try
            xdoc = New XmlDocument
            xdoc.PreserveWhitespace = False

            xdoc.Load(strpath)
            SetNodeValue(xdoc, "ifti/localId", FillOrDefault(objifti.LocalID, "string2"))
            SetNodeValue(xdoc, "ifti/umum/tglLaporan", FillOrDefault(objifti.TanggalLaporan.GetValueOrDefault(Now), "datetime"))
            SetNodeValue(xdoc, "ifti/umum/namaPejabatPjk", FillOrDefault(objifti.NamaPejabatPJKBankPelapor, "string"))
            If objifti.JenisLaporan.GetValueOrDefault(0) = 1 Then
                SetNodeValue(xdoc, "ifti/umum/jenisLaporan", objifti.JenisLaporan.GetValueOrDefault(1).ToString)

            ElseIf objifti.JenisLaporan.GetValueOrDefault(0) = 2 Then
                SetNodeValue(xdoc, "ifti/umum/jenisLaporan", objifti.JenisLaporan.GetValueOrDefault(1).ToString)
                SetNodeValue(xdoc, "ifti/umum/noLtklKoreksi", FillOrDefault(objifti.LTDLNNoKoreksi, "string2"))
            End If
            SetNodeValue(xdoc, "ifti/pjkBankSebagai", FillOrDefault(objifti.Sender_FK_PJKBank_Type.GetValueOrDefault(1), "int"))




            If objifti.Sender_FK_PJKBank_Type.GetValueOrDefault(0) = 1 Then ' Penyelenggara Pengirim Asal / Penyelenggara Penerima akhir
                If objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 1 Then
                    GetSenderSwiftOutgoingPerorangan(objifti, xdoc)
                ElseIf objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 2 Then
                    GetSenderSwiftOutgoingKorporasi(objifti, xdoc)
                ElseIf objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 3 Then
                    GetSenderSwiftOutgoingNonNasabah(objifti, xdoc)
                End If

            ElseIf objifti.Sender_FK_PJKBank_Type.GetValueOrDefault(0) = 2 Then ' Penyelenggara Penerus

                If objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 4 Then
                    GetSenderSwiftOutgoingIndividuPenyelengaraPenerus(objifti, xdoc)
                ElseIf objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 5 Then
                    GetSenderSwiftOutgoingKorporasiPenyelengaraPenerus(objifti, xdoc)
                ElseIf objifti.Sender_FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 6 Then
                    GetSenderSwiftOutgoingNonNasabah(objifti, xdoc)
                End If
            End If
            GetBeneficialOwnerSwiftOutgoing(objifti, xdoc)

            Dim xmlPenerimaNasabahPerorangan As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nasabah/perorangan")
            Dim xmlPenerimaNasabahkorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nasabah/korporasi")
            Dim xmlPenerimaNonNasabah As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nonNasabah")

            Dim xmlPenerimaNasabahPeroranganClone As XmlNode = xmlPenerimaNasabahPerorangan.Clone
            Dim xmlPenerimaNasabahkorporasiClone As XmlNode = xmlPenerimaNasabahkorporasi.Clone
            Dim xmlPenerimaNonNasabahclone As XmlNode = xmlPenerimaNonNasabah.Clone

            Dim FlagPenerimaNasabahPerorangan As Boolean = False
            Dim FlagPenerimaNasabahKorporasi As Boolean = False
            Dim FlagPenerimaNonNasabah As Boolean = False



            Using objDb As NawaDevDAL.NawaDatadevEntities = New NawaDevDAL.NawaDatadevEntities

                Dim objBeneficiary As List(Of NawaDevDAL.IFTI_Beneficiary) = objDb.IFTI_Beneficiary.Where(Function(x) x.FK_IFTI_ID = objifti.PK_IFTI_ID).ToList
                For Each item As NawaDevDAL.IFTI_Beneficiary In objBeneficiary
                    If item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 1 Or item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 4 Then

                        If FlagPenerimaNasabahPerorangan = False Then

                            Dim xLastNodePenerimaNasabahPErorangan As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nasabah/perorangan")
                            GenerateIdentitasPenerimaIndividuSwiftOutgoing(item, xLastNodePenerimaNasabahPErorangan)
                            FlagPenerimaNasabahPerorangan = True
                        Else
                            Dim objListNodePenerimaNasabahPerorangan As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nasabah/perorangan")
                            objListNodePenerimaNasabahPerorangan(objListNodePenerimaNasabahPerorangan.Count - 1).ParentNode.InsertAfter(xmlPenerimaNasabahPeroranganClone, objListNodePenerimaNasabahPerorangan(objListNodePenerimaNasabahPerorangan.Count - 1))

                            Dim xLastNodePenerimaListNasabahIndividu As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nasabah/perorangan")
                            Dim indexAkhir As Integer = xLastNodePenerimaListNasabahIndividu.Count - 1
                            GenerateIdentitasPenerimaIndividuSwiftOutgoing(item, xLastNodePenerimaListNasabahIndividu(indexAkhir))
                        End If

                    ElseIf item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 2 Or item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 5 Then

                        If FlagPenerimaNasabahKorporasi = False Then

                            Dim xLastNodePenerimaNasabahKorporasi As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nasabah/korporasi")
                            GenerateIdentitasPenerimaKorporasiSwiftOutgoing(item, xLastNodePenerimaNasabahKorporasi)
                            FlagPenerimaNasabahPerorangan = True
                        Else
                            Dim objListNodePenerimaNasabahkorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nasabah/korporasi")
                            objListNodePenerimaNasabahkorporasi(objListNodePenerimaNasabahkorporasi.Count - 1).ParentNode.InsertAfter(xmlPenerimaNasabahkorporasiClone, objListNodePenerimaNasabahkorporasi(objListNodePenerimaNasabahkorporasi.Count - 1))

                            Dim xLastNodePenerimaListNasabahkorporasi As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nasabah/korporasi")
                            Dim indexAkhir As Integer = xLastNodePenerimaListNasabahkorporasi.Count - 1
                            GenerateIdentitasPenerimaKorporasiSwiftOutgoing(item, xLastNodePenerimaListNasabahkorporasi(indexAkhir))
                        End If
                    ElseIf item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 3 Or item.FK_IFTI_NasabahType_ID.GetValueOrDefault(0) = 6 Then
                        If FlagPenerimaNonNasabah = False Then

                            Dim xLastNodePenerimaNonNasabah As XmlNode = xdoc.SelectSingleNode("ifti/identitasPenerima/nonNasabah")
                            GenerateIdentitasPenerimaNonNasabahSwiftOutgoing(item, xLastNodePenerimaNonNasabah)
                            FlagPenerimaNonNasabah = True
                        Else
                            Dim objListNodePenerimaNonNasabah As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nonNasabah")
                            objListNodePenerimaNonNasabah(objListNodePenerimaNonNasabah.Count - 1).ParentNode.InsertAfter(xmlPenerimaNonNasabahclone, objListNodePenerimaNonNasabah(objListNodePenerimaNonNasabah.Count - 1))

                            Dim xLastNodePenerimaListNonNasabah As XmlNodeList = xdoc.SelectNodes("ifti/identitasPenerima/nonNasabah")
                            Dim indexAkhir As Integer = xLastNodePenerimaListNonNasabah.Count - 1
                            GenerateIdentitasPenerimaNonNasabahSwiftOutgoing(item, xLastNodePenerimaListNonNasabah(indexAkhir))
                        End If
                    End If
                Next
            End Using



            GenerateTransaksiSwiftOutgoing(objifti, xdoc)
            GetInformasiLainnya(objifti, xdoc)
            RemoveEndTag(xdoc)
            xdoc.Save(strpath)
        Catch ex As Exception
            Throw
        Finally
            If Not xdoc Is Nothing Then
                xdoc = Nothing
            End If
        End Try



    End Function


    Shared Function RemoveEndTag(ByRef xdoc As XmlDocument) As Boolean

        'Dim nsmgr As System.Xml.XmlNamespaceManager = New System.Xml.XmlNamespaceManager(xdoc.NameTable)
        'nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance")

        Dim objNodelist As XmlNodeList = xdoc.SelectNodes("/ifti")
        For Each Item As XmlNode In objNodelist
            ClearNode(Item)
        Next


    End Function
    Shared Sub ClearNode(ByRef Xnode As XmlNode)
        If Xnode.HasChildNodes Then
            For Each Item As XmlNode In Xnode.ChildNodes
                ClearNode(Item)
            Next
        Else

            If Xnode.NodeType = XmlNodeType.Element Then
                If Xnode.InnerText = "" Then
                    CType(Xnode, XmlElement).IsEmpty = True
                End If
            ElseIf Xnode.NodeType = XmlNodeType.Text And Xnode.HasChildNodes = False And Xnode.InnerText = "" Then
                CType(Xnode.ParentNode, XmlElement).IsEmpty = True
            End If
        End If


    End Sub
    Shared Function GetInformasiLainnya(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean



        SetNodeValue(xdoc, "ifti/informasiLainnya/infSendersCorrespondent", FillOrDefault(objifti.InformationAbout_SenderCorrespondent, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/infReceiverCorrespondent", FillOrDefault(objifti.InformationAbout_ReceiverCorrespondent, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/infThirdReimbursementInstitution", FillOrDefault(objifti.InformationAbout_Thirdreimbursementinstitution, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/infIntermediaryInstitution", FillOrDefault(objifti.InformationAbout_IntermediaryInstitution, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/remittanceInformation", FillOrDefault(objifti.RemittanceInformation, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/senderToReceiverInformation", FillOrDefault(objifti.SendertoReceiverInformation, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/regulatoryReporting", FillOrDefault(objifti.RegulatoryReporting, "string2"))
        SetNodeValue(xdoc, "ifti/informasiLainnya/envelopeContents", FillOrDefault(objifti.EnvelopeContents, "string2"))


    End Function
    Shared Function GenerateTransaksiSwiftOutgoing(ByVal objifti As NawaDevDAL.IFTI, ByRef xdoc As XmlDocument) As Boolean

        SetNodeValue(xdoc, "ifti/transaksi/tglTransaksi", FillOrDefault(objifti.TanggalTransaksi, "datetime"))
        SetNodeValue(xdoc, "ifti/transaksi/timeIndication", FillOrDefault(objifti.TimeIndication, "string"))
        SetNodeValue(xdoc, "ifti/transaksi/sendersReference", FillOrDefault(objifti.SenderReference, "string"))
        SetNodeValue(xdoc, "ifti/transaksi/bankOperationCode", FillOrDefault(objifti.BankOperationCode, "string"))
        SetNodeValue(xdoc, "ifti/transaksi/instructionCode", FillOrDefault(objifti.InstructionCode, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/kanCabPenyelenggaraPengirimAsal", FillOrDefault(objifti.KantorCabangPenyelengaraPengirimAsal, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/typeTransactionCode", FillOrDefault(objifti.TransactionCode, "string2"))

        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/valueDate", FillOrDefault(objifti.ValueDate_TanggalTransaksi, "datetime"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/amount", FillOrDefault(objifti.ValueDate_NilaiTransaksi, "decimal"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/currency", FillOrDefault(objifti.ValueDate_FK_Currency_ID, "int"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/currencyLain", FillOrDefault(objifti.ValueDate_CurrencyLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/dateCurrencyAmount/amountDalamRupiah", FillOrDefault(objifti.ValueDate_NilaiTransaksiIDR, "decimal"))


        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/currency", FillOrDefault(objifti.Instructed_Currency, "int"))
        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/currencyLain", FillOrDefault(objifti.Instructed_CurrencyLainnya, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/currencyInstructedAmount/instructedAmount", FillOrDefault(objifti.Instructed_Amount, "decimal"))

        SetNodeValue(xdoc, "ifti/transaksi/exchangeRate", FillOrDefault(objifti.ExchangeRate, "decimal"))
        SetNodeValue(xdoc, "ifti/transaksi/sendingInstitution", FillOrDefault(objifti.SendingInstitution, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/tujuanTransaksi", FillOrDefault(objifti.TujuanTransaksi, "string2"))
        SetNodeValue(xdoc, "ifti/transaksi/sumberDana", FillOrDefault(objifti.SumberPenggunaanDana, "string2"))




    End Function
End Class
