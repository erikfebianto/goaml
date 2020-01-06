Imports NawaDAL
Imports NawaBLL
Imports NawaDevDAL
Imports NawaDevBLL
Imports Ext.Net

Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Partial Class RWA_ReportRWA1A
    Inherits Parent

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

    Private Sub ClearSession()
        Session("Report1A.LstExcludedReportAdministratifLainnya") = Nothing
    End Sub

    Protected Sub Generate()
        Try
            ClearSession()
            Me.ReportViewer1.Visible = True
            PanelReport.Hidden = False

            Dim strSpaceString As String = String.Empty

            'Table 1
            Dim dTable1 As New DataTable()
            dTable1.Columns.Add("No", GetType(String))
            dTable1.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable1.Columns.Add("TotalTagihan", GetType(Decimal))
            dTable1.Columns.Add("TotalCKPN", GetType(Decimal))
            dTable1.Columns.Add("TotalTagihanBersih", GetType(Decimal))

            'Dim strWhereCondTable1 As String = "Fk_JenisReport_Id =1 AND FK_MsJenisTagihanReportParentId =0"
            Dim strWhereCondTable1 As String = "0"
            pr_GetReportTable1(dTable1, strWhereCondTable1, String.Empty, strSpaceString, 0)

            'Table 2
            Dim dTable2a As New DataTable()
            dTable2a.Columns.Add("No", GetType(String))
            dTable2a.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable2a.Columns.Add("TotalNilaiTRA", GetType(Decimal))
            dTable2a.Columns.Add("TotalPPA", GetType(Decimal))
            dTable2a.Columns.Add("TotalTRANetto", GetType(Decimal))

            Dim dTable2b As New DataTable()
            dTable2b.Columns.Add("No", GetType(String))
            dTable2b.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable2b.Columns.Add("TotalNilaiTRA", GetType(Decimal))
            dTable2b.Columns.Add("TotalPPA", GetType(Decimal))
            dTable2b.Columns.Add("TotalTRANetto", GetType(Decimal))

            Dim strWhereCondTable2 As String = "0"
            pr_GetReportTable2(dTable2a, dTable2b, strWhereCondTable2, String.Empty, strSpaceString, 0)

            'Table 3
            Dim dTable3a As New DataTable()
            dTable3a.Columns.Add("No", GetType(String))
            dTable3a.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable3a.Columns.Add("TotalTagihanBersih", GetType(Decimal))

            Dim dTable3b As New DataTable()
            dTable3b.Columns.Add("No", GetType(String))
            dTable3b.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable3b.Columns.Add("TotalTagihan", GetType(Decimal))
            dTable3b.Columns.Add("TotalCKPN", GetType(Decimal))
            dTable3b.Columns.Add("TotalTagihanBersih", GetType(Decimal))

            Dim dTable3c As New DataTable()
            dTable3c.Columns.Add("No", GetType(String))
            dTable3c.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable3c.Columns.Add("TotalTagihanBersih", GetType(Decimal))

            Dim strWhereCondTable3 As String = "0"
            pr_GetReportTable3(dTable3a, dTable3b, dTable3c, strWhereCondTable3, String.Empty, strSpaceString, 0)

            'Table 4
            Dim dTable4 As New DataTable()
            dTable4.Columns.Add("No", GetType(String))
            dTable4.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable4.Columns.Add("TotalEksposur", GetType(Decimal))

            Dim strWhereCondTable4 As String = "0"
            pr_GetReportTable4(dTable4, strWhereCondTable4, String.Empty, strSpaceString, 0)

            'Table 5
            Dim dTable5 As New DataTable()
            dTable5.Columns.Add("No", GetType(String))
            dTable5.Columns.Add("NamaJenisTagihanReport", GetType(String))
            dTable5.Columns.Add("TotalEksposurSebagaiPengurangModal", GetType(Decimal))
            dTable5.Columns.Add("TotalEksposurSebagaiATMR", GetType(Decimal))

            Dim strWhereCondTable5 As String = "0"
            pr_GetReportTable5(dTable5, strWhereCondTable5, String.Empty, strSpaceString, 0)

            'Table 6
            Dim dTable6 As New DataTable()
            dTable6.Columns.Add("TotalFaktorPengurangModal", GetType(Decimal))
            dTable6.Columns.Add("TotalATMR", GetType(Decimal))

            GetReportTable6(dTable6)

            ReportViewer1.LocalReport.DataSources.Clear()

            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable1", dTable1))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable2a", dTable2a))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable2b", dTable2b))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable3a", dTable3a))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable3b", dTable3b))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable3c", dTable3c))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable4", dTable4))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable5", dTable5))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DsRpt1A_DataTable6", dTable6))

            ReportViewer1.DataBind()
            ReportViewer1.LocalReport.Refresh()

            ReportViewer1.ShowReportBody = True

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    'Eksposur Syariah
    Private Sub GetReportTable6(ByRef DtTable As DataTable)
        Using ObjDb As New NawaDatadevEntities
            Dim StrWhere As String = ""
            StrWhere = "ProcessMonth = " + Month(DatTanggalData.SelectedValue).ToString + " AND ProcessYear = " + Year(DatTanggalData.SelectedValue).ToString

            Dim DtRow As DataRow = DtTable.NewRow
            DtRow("TotalFaktorPengurangModal") = 0
            DtRow("TotalATMR") = 0

            Dim ObjReportSyariah As List(Of RWA_ReportBI_1A_Syariah) = ObjDb.RWA_ReportBI_1A_Syariah.Where(Function(x) x.ProcessMonth = Month(DatTanggalData.SelectedValue).ToString And x.ProcessYear = Year(DatTanggalData.SelectedValue).ToString).ToList
            If ObjReportSyariah.Count > 0 Then
                DtRow("TotalFaktorPengurangModal") = ObjReportSyariah(0).TotalFaktorPengurangModal.GetValueOrDefault(0)
                DtRow("TotalATMR") = ObjReportSyariah(0).TotalATMR.GetValueOrDefault(0)
            End If

            DtTable.Rows.Add(DtRow)
        End Using
    End Sub

    Private Sub pr_GetReportTable3(ByVal P_dTable3a As DataTable, ByVal P_dTable3b As DataTable, ByVal P_dTable3c As DataTable, ByVal strParentId As String, ByVal P_strPK_ID As String, ByVal P_strSpace As String, ByVal IntLevel As Integer)
        Using ObjDb As New NawaDatadevEntities
            Dim listData As List(Of RWA_MsJenisTagihanReport) = ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.Fk_JenisReport_Id = 3 And x.FK_MsJenisTagihanReportParentId = strParentId).ToList


            If listData.Count > 0 Then
                Dim IntCount As Integer = 1

                For Each objData As RWA_MsJenisTagihanReport In listData
                    Dim dRowDataA As DataRow = P_dTable3a.NewRow
                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowDataA("No") = CStr(IntCount)
                    Else
                        dRowDataA("No") = ""
                    End If
                    dRowDataA("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowDataA("TotalTagihanBersih") = 0
                    P_dTable3a.Rows.Add(dRowDataA)

                    Dim dRowDataB As DataRow = P_dTable3b.NewRow
                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowDataB("No") = CStr(IntCount)
                    Else
                        dRowDataB("No") = ""
                    End If
                    dRowDataB("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowDataB("TotalTagihan") = 0
                    dRowDataB("TotalCKPN") = 0
                    dRowDataB("TotalTagihanBersih") = 0
                    P_dTable3b.Rows.Add(dRowDataB)

                    Dim dRowDataC As DataRow = P_dTable3c.NewRow
                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowDataC("No") = CStr(IntCount)
                    Else
                        dRowDataC("No") = ""
                    End If
                    dRowDataC("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowDataC("TotalTagihanBersih") = 0
                    P_dTable3c.Rows.Add(dRowDataC)

                    P_strSpace += "  "

                    Dim strWhereCond As String = objData.PK_MsJenisTagihanReportId.ToString()

                    pr_GetReportTable3(P_dTable3a, P_dTable3b, P_dTable3c, strWhereCond, objData.PK_MsJenisTagihanReportId.ToString(), P_strSpace, IntLevel + 1)
                    IntCount += 1
                Next

            Else

                If P_strPK_ID.Trim <> String.Empty Then
                    Dim dTableA As DataTable = pr_getReportValueTable3a(P_strPK_ID)
                    If dTableA.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Counterparty, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable3a.Rows.Count - 1

                        P_dTable3a.Rows(IntParentIndex).Item("TotalTagihanBersih") = dTableA.Rows(0).Item("TotalTagihanBersih").ToString
                    ElseIf dTableA.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTableA.Rows
                            dRow(0) = P_strSpace + dRow(0).ToString()
                            P_dTable3a.Rows.Add(CopyDataRow(P_dTable3a, dRow))
                        Next
                    End If

                    Dim dTableB As DataTable = pr_getReportValueTable3b(P_strPK_ID)
                    If dTableB.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Counterparty, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable3b.Rows.Count - 1

                        P_dTable3b.Rows(IntParentIndex).Item("TotalTagihan") = dTableB.Rows(0).Item("TotalTagihan").ToString
                        P_dTable3b.Rows(IntParentIndex).Item("TotalCKPN") = dTableB.Rows(0).Item("TotalCKPN").ToString
                        P_dTable3b.Rows(IntParentIndex).Item("TotalTagihanBersih") = dTableB.Rows(0).Item("TotalTagihanBersih").ToString
                    ElseIf dTableA.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTableB.Rows
                            dRow(0) = P_strSpace + dRow(0).ToString()
                            P_dTable3b.Rows.Add(CopyDataRow(P_dTable3b, dRow))
                        Next
                    End If

                    Dim dTableC As DataTable = pr_getReportValueTable3c(P_strPK_ID)
                    If dTableC.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Counterparty, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable3c.Rows.Count - 1

                        P_dTable3c.Rows(IntParentIndex).Item("TotalTagihanBersih") = dTableC.Rows(0).Item("TotalTagihanBersih").ToString
                    ElseIf dTableA.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTableC.Rows
                            dRow(0) = P_strSpace + dRow(0).ToString()
                            P_dTable3c.Rows.Add(CopyDataRow(P_dTable3c, dRow))
                        Next
                    End If
                End If

            End If
        End Using
    End Sub

    Private Function pr_getReportValueTable3a(ByVal P_strJenisTagihanReportID As String) As DataTable
        Dim strCommandText As String = "SELECT '' as No, JTR.NamaJenisTagihanReport, R.TotalTagihanBersih" +
                                                       " FROM RWA_ReportBI_1A_Counterparty R" +
                                                       " INNER JOIN RWA_MsJenisTagihanReport JTR ON JTR.PK_MsJenisTagihanReportID =  R.FK_MsJenisTagihanReport_ID" +
                                                       " INNER JOIN RWA_JenisEksposurCounterparty JEC ON JEC.Pk_JenisEksposurCounterparty_Id = R.Fk_JenisEksposurCounterparty_Id" +
                                                       " WHERE R.Fk_MsJenisTagihanReport_ID = " + P_strJenisTagihanReportID + " AND " + "JEC.Pk_JenisEksposurCounterparty_Id = 1" + pr_getMonthAndYearParameterString()

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    Private Function pr_getReportValueTable3b(ByVal P_strJenisTagihanReportID As String) As DataTable
        Dim strCommandText As String = "SELECT '' as No, JTR.NamaJenisTagihanReport, R.TotalTagihan, R.TotalCKPN, R.TotalTagihanBersih" +
                                                      " FROM RWA_ReportBI_1A_Counterparty R" +
                                                      " INNER JOIN RWA_MsJenisTagihanReport JTR ON JTR.PK_MsJenisTagihanReportID =  R.FK_MsJenisTagihanReport_ID" +
                                                      " INNER JOIN RWA_JenisEksposurCounterparty JEC ON JEC.Pk_JenisEksposurCounterparty_Id = R.Fk_JenisEksposurCounterparty_Id" +
                                                      " WHERE R.Fk_MsJenisTagihanReport_ID = " + P_strJenisTagihanReportID + " AND " + "JEC.Pk_JenisEksposurCounterparty_Id = 2" + pr_getMonthAndYearParameterString()

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    Private Function pr_getReportValueTable3c(ByVal P_strJenisTagihanReportID As String) As DataTable
        Dim strCommandText As String = "SELECT '' as No, JTR.NamaJenisTagihanReport, R.TotalTagihanBersih" +
                                                      " FROM RWA_ReportBI_1A_Counterparty R" +
                                                      " INNER JOIN RWA_MsJenisTagihanReport JTR ON JTR.PK_MsJenisTagihanReportID =  R.FK_MsJenisTagihanReport_ID" +
                                                      " INNER JOIN RWA_JenisEksposurCounterparty JEC ON JEC.Pk_JenisEksposurCounterparty_Id = R.Fk_JenisEksposurCounterparty_Id" +
                                                      " WHERE R.Fk_MsJenisTagihanReport_ID = " + P_strJenisTagihanReportID + " AND " + "JEC.Pk_JenisEksposurCounterparty_Id = 3" + pr_getMonthAndYearParameterString()

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    Private Sub pr_GetReportTable4(ByVal P_dTable4 As DataTable, ByVal strParentId As String, ByVal P_strPK_ID As String, ByVal P_strSpace As String, ByVal IntLevel As Integer)
        Using ObjDb As New NawaDatadevEntities
            Dim listData As List(Of RWA_MsJenisTagihanReport) = ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.Fk_JenisReport_Id = 4 And x.FK_MsJenisTagihanReportParentId = strParentId).ToList

            If listData.Count > 0 Then
                Dim IntCount As Integer = 1

                For Each objData As RWA_MsJenisTagihanReport In listData
                    Dim dRowData As DataRow = P_dTable4.NewRow

                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowData("No") = CStr(IntCount)
                    Else
                        dRowData("No") = ""
                    End If
                    dRowData("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowData("TotalEksposur") = 0
                    P_dTable4.Rows.Add(dRowData)

                    P_strSpace += "  "

                    Dim strWhereCond As String = objData.PK_MsJenisTagihanReportId.ToString()

                    pr_GetReportTable4(P_dTable4, strWhereCond, objData.PK_MsJenisTagihanReportId.ToString(), P_strSpace, IntLevel + 1)
                    IntCount += 1
                Next
            Else
                If P_strPK_ID.Trim() <> String.Empty Then
                    Dim dTable As DataTable = pr_getReportValueTable4(P_strPK_ID)
                    If dTable.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Settlement, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable4.Rows.Count - 1

                        P_dTable4.Rows(IntParentIndex).Item("TotalEksposur") = dTable.Rows(0).Item("TotalEksposur").ToString
                    ElseIf dTable.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTable.Rows
                            dRow(0) = P_strSpace + dRow(0).ToString()
                            P_dTable4.Rows.Add(dRow)
                        Next
                    End If
                End If
            End If
        End Using
    End Sub

    Private Function pr_getReportValueTable4(ByVal P_strJenisTagihanReportID) As DataTable
        Dim strCommandText As String = "SELECT '' as No, JTR.NamaJenisTagihanReport, R.TotalEksposur" +
                                                        " FROM RWA_ReportBI_1A_Settlement R" +
                                                        " INNER JOIN RWA_MsJenisTagihanReport JTR ON JTR.PK_MsJenisTagihanReportID =  R.FK_MsJenisTagihanReport_ID" +
                                                        " WHERE R.Fk_MsJenisTagihanReport_ID =" + P_strJenisTagihanReportID + pr_getMonthAndYearParameterString()

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    Private Sub pr_GetReportTable5(ByVal P_dTable5 As DataTable, ByVal strParentId As String, ByVal P_strPK_ID As String, ByVal P_strSpace As String, ByVal IntLevel As Integer)
        Using ObjDb As New NawaDatadevEntities
            Dim listData As List(Of RWA_MsJenisTagihanReport) = ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.Fk_JenisReport_Id = 5 And x.FK_MsJenisTagihanReportParentId = strParentId).ToList()

            If listData.Count > 0 Then
                Dim IntCount As Integer = 1

                For Each objData As RWA_MsJenisTagihanReport In listData
                    Dim dRowData As DataRow = P_dTable5.NewRow

                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowData("No") = CStr(IntCount)
                    Else
                        dRowData("No") = ""
                    End If

                    'Utk Sekuritisasi Lainnya dihardcode jadi:
                    'Eksposur Sekuritisasi Yang Tidak Tercakup Dalam Ketentuan Bank Indonesia Mengenai Prinsip-Prinsip Kehati-hatian Dalam Aktivitas Sekuritisasi Aset Bagi Bank Umum
                    Dim StrNamaJenisTagihanReport As String = ""
                    If objData.PK_MsJenisTagihanReportId <> 75 Then
                        StrNamaJenisTagihanReport = objData.NamaJenisTagihanReport
                    Else
                        StrNamaJenisTagihanReport = "Eksposur Sekuritisasi Yang Tidak Tercakup Dalam Ketentuan Bank Indonesia Mengenai Prinsip-Prinsip Kehati-hatian Dalam Aktivitas Sekuritisasi Aset Bagi Bank Umum"
                    End If

                    dRowData("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + StrNamaJenisTagihanReport
                    dRowData("TotalEksposurSebagaiPengurangModal") = 0
                    dRowData("TotalEksposurSebagaiATMR") = 0
                    P_dTable5.Rows.Add(dRowData)

                    P_strSpace += "  "

                    Dim strWhereCond As String = objData.PK_MsJenisTagihanReportId.ToString()

                    pr_GetReportTable5(P_dTable5, strWhereCond, objData.PK_MsJenisTagihanReportId.ToString(), P_strSpace, IntLevel + 1)
                    IntCount += 1
                Next

            Else
                If P_strPK_ID.Trim() <> String.Empty Then
                    Dim dTable As DataTable = pr_getReportValueTable5(P_strPK_ID)
                    If dTable.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Settlement, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable5.Rows.Count - 1

                        P_dTable5.Rows(IntParentIndex).Item("TotalEksposurSebagaiPengurangModal") = dTable.Rows(0).Item("TotalEksposurSebagaiPengurangModal").ToString
                        P_dTable5.Rows(IntParentIndex).Item("TotalEksposurSebagaiATMR") = dTable.Rows(0).Item("TotalEksposurSebagaiATMR").ToString
                    ElseIf dTable.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTable.Rows
                            dRow(0) = P_strSpace + dRow(0).ToString()
                            P_dTable5.Rows.Add(dRow)
                        Next
                    End If

                End If

            End If
        End Using
    End Sub

    Private Function pr_getReportValueTable5(ByVal P_strJenisTagihanReportID) As DataTable
        Dim strCommandText As String = "SELECT '' as No, JTR.NamaJenisTagihanReport, R.TotalEksposurSebagaiPengurangModal,R.TotalEksposurSebagaiATMR" +
                                                        " FROM RWA_ReportBI_1A_Sekuritisasi R" +
                                                        " INNER JOIN RWA_MsJenisTagihanReport JTR ON JTR.PK_MsJenisTagihanReportID =  R.FK_MsJenisTagihanReport_ID" +
                                                        " WHERE R.FK_MsJenisTagihanReport_ID =" + P_strJenisTagihanReportID + pr_getMonthAndYearParameterString()

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function
    Private ReadOnly Property LstExcludedReportAdministratifLainnya() As List(Of Integer)
        Get
            If Session("Report1A.LstExcludedReportAdministratifLainnya") Is Nothing Then
                Dim LstTemp As New List(Of Integer)

                Using DtTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT leal.Fk_JenisTagihanReport_Id FROM ListExcludedAdministratifLainnya leal", Nothing)
                    For Each DtRow As DataRow In DtTable.Rows
                        LstTemp.Add(CInt(DtRow.Item(0).ToString))
                    Next
                End Using
                Session("Report1A.LstExcludedReportAdministratifLainnya") = LstTemp
            End If
            Return CType(Session("Report1A.LstExcludedReportAdministratifLainnya"), List(Of Integer))
        End Get
    End Property

    Private Sub pr_GetReportTable2(ByVal P_dTable2a As DataTable, ByVal P_dTable2b As DataTable, ByVal strParentId As String, ByVal P_strPK_ID As String, ByVal P_strSpace As String, ByVal IntLevel As Integer)
        Using ObjDb As New NawaDatadevEntities
            Dim listData As List(Of RWA_MsJenisTagihanReport) = ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.Fk_JenisReport_Id = 2 And x.FK_MsJenisTagihanReportParentId = strParentId).ToList

            If listData.Count > 0 Then
                Dim IntCount As Integer = 1

                For Each objData As RWA_MsJenisTagihanReport In listData
                    Dim dRowDataA As DataRow = P_dTable2a.NewRow
                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowDataA("No") = CStr(IntCount)
                    Else
                        dRowDataA("No") = ""
                    End If

                    dRowDataA("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowDataA("TotalNilaiTRA") = 0
                    dRowDataA("TotalPPA") = 0
                    dRowDataA("TotalTRANetto") = 0
                    P_dTable2a.Rows.Add(dRowDataA)

                    If Not Me.LstExcludedReportAdministratifLainnya.Contains(objData.PK_MsJenisTagihanReportId) Then
                        Dim dRowDataB As DataRow = P_dTable2b.NewRow
                        'Nomor hanya diisi utk parent paling atas saja
                        If IntLevel = 0 Then
                            dRowDataB("No") = CStr(IntCount)
                        Else
                            dRowDataB("No") = ""
                        End If

                        dRowDataB("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                        dRowDataB("TotalNilaiTRA") = 0
                        dRowDataB("TotalPPA") = 0
                        dRowDataB("TotalTRANetto") = 0
                        P_dTable2b.Rows.Add(dRowDataB)
                    End If

                    P_strSpace += "  "

                    Dim strWhereCond As String = objData.PK_MsJenisTagihanReportId.ToString()

                    pr_GetReportTable2(P_dTable2a, P_dTable2b, strWhereCond, objData.PK_MsJenisTagihanReportId.ToString(), P_strSpace, IntLevel + 1)
                    IntCount += 1
                Next

            Else

                If P_strPK_ID.Trim() <> String.Empty Then
                    Dim dTableA As DataTable = pr_getReportValueTable2a(P_strPK_ID)
                    If dTableA.Rows.Count = 1 Then
                        'Kalau cuma ada 1 portofolio Kewajiban, maka nilainya dinaikkan ke tagihan report di atasnya
                        Dim IntParentIndex As Integer = P_dTable2a.Rows.Count - 1

                        P_dTable2a.Rows(IntParentIndex).Item("TotalNilaiTRA") = dTableA.Rows(0).Item("TotalNilaiTRA").ToString
                        P_dTable2a.Rows(IntParentIndex).Item("TotalPPA") = dTableA.Rows(0).Item("TotalPPA").ToString
                        P_dTable2a.Rows(IntParentIndex).Item("TotalTRANetto") = dTableA.Rows(0).Item("TotalTRANetto").ToString
                    ElseIf dTableA.Rows.Count > 0 Then
                        For Each dRow As DataRow In dTableA.Rows
                            dRow(0) = P_strSpace + dRow(0)
                            P_dTable2a.Rows.Add(CopyDataRow(P_dTable2a, dRow))
                        Next
                    End If

                    If Not Me.LstExcludedReportAdministratifLainnya.Contains(CInt(P_strPK_ID)) Then
                        Dim dTableB As DataTable = pr_getReportValueTable2b(P_strPK_ID)
                        If dTableB.Rows.Count = 1 Then
                            'Kalau cuma ada 1 portofolio Kewajiban, maka nilainya dinaikkan ke tagihan report di atasnya
                            Dim IntParentIndex As Integer = P_dTable2b.Rows.Count - 1

                            P_dTable2b.Rows(IntParentIndex).Item("TotalNilaiTRA") = dTableB.Rows(0).Item("TotalNilaiTRA").ToString
                            P_dTable2b.Rows(IntParentIndex).Item("TotalPPA") = dTableB.Rows(0).Item("TotalPPA").ToString
                            P_dTable2b.Rows(IntParentIndex).Item("TotalTRANetto") = dTableB.Rows(0).Item("TotalTRANetto").ToString
                        ElseIf dTableB.Rows.Count > 0 Then
                            For Each dRow As DataRow In dTableB.Rows
                                dRow(0) = P_strSpace + dRow(0)
                                P_dTable2b.Rows.Add(CopyDataRow(P_dTable2b, dRow))
                            Next
                        End If
                    End If
                End If

            End If
        End Using
    End Sub

    Private Function CopyDataRow(ByVal DtTableTo As DataTable, ByVal DtRowFrom As DataRow) As DataRow
        Dim DtRow As DataRow = DtTableTo.NewRow

        For x As Integer = 0 To DtRowFrom.Table.Columns.Count - 1
            DtRow.Item(x) = DtRowFrom.Item(x)
        Next

        Return DtRow
    End Function

    Private Function pr_getReportValueTable2a(ByVal P_strJenisTagihanReportID As String) As DataTable
        Dim strWhereCond As String = String.Empty

        Dim strCommandText As String = "SELECT '' as No, mjtr.NamaJenisTagihanReport, ISNULL(r.TotalNilaiTRA, 0) AS TotalNilaiTRA, ISNULL(r.TotalPPA, 0) AS TotalPPA, ISNULL(r.TotalTRANetto, 0) AS TotalTRANetto" +
                                                       " FROM RWA_MsJenisTagihanReport mjtr" +
                                                       " LEFT JOIN RWA_ReportBI_1A_Kewajiban r ON mjtr.PK_MsJenisTagihanReportId = r.Fk_MsJenisTagihanReport_Id AND r.Fk_JenisEksposurKewajiban_Id = 1" +
                                                       pr_getMonthAndYearParameterString() +
                                                       " WHERE R.Fk_MsJenisTagihanReport_ID = " + P_strJenisTagihanReportID

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    Private Function pr_getReportValueTable2b(ByVal P_strJenisTagihanReportID As String) As DataTable
        Dim strWhereCond As String = String.Empty
        Dim strCommandText As String = "SELECT '' as No, mjtr.NamaJenisTagihanReport, ISNULL(r.TotalNilaiTRA, 0) AS TotalNilaiTRA, ISNULL(r.TotalPPA, 0) AS TotalPPA, ISNULL(r.TotalTRANetto, 0) AS TotalTRANetto" +
                                                       " FROM RWA_MsJenisTagihanReport mjtr" +
                                                       " LEFT JOIN RWA_ReportBI_1A_Kewajiban r ON mjtr.PK_MsJenisTagihanReportId = r.Fk_MsJenisTagihanReport_Id AND r.Fk_JenisEksposurKewajiban_Id = 2" +
                                                       pr_getMonthAndYearParameterString() +
                                                       " WHERE R.Fk_MsJenisTagihanReport_ID = " + P_strJenisTagihanReportID

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

    ''' <summary>
    ''' Utk mendapatkan format nomor child di Eksposur Aset
    ''' utk Level 1, formatnya: a., b., c., dst
    ''' utk level 2, formatnya: 1), 2), 3), dst
    ''' </summary>
    ''' <param name="IntLevel"></param>
    ''' <param name="IntCurrentIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCurrentNumberFormat(ByVal IntLevel As Integer, ByVal IntCurrentIndex As Integer) As String
        Dim StrReturn As String = ""

        If IntLevel = 1 Then
            StrReturn = Chr(Asc("a") - 1 + IntCurrentIndex) & ".  "
        ElseIf IntLevel = 2 Then
            StrReturn = "    " & Str(0 + IntCurrentIndex) + ")  "
        End If

        Return StrReturn
    End Function

    Private Sub pr_GetReportTable1(ByRef P_dTable1 As DataTable, ByVal strParentId As String, ByVal P_strPK_ID As String, ByVal P_strSpace As String, ByVal IntLevel As Integer)
        Using ObjDb As New NawaDatadevEntities
            '"Fk_JenisReport_Id =1 AND FK_MsJenisTagihanReportParentId =0"
            Dim listData As List(Of RWA_MsJenisTagihanReport) = ObjDb.RWA_MsJenisTagihanReport.Where(Function(x) x.Fk_JenisReport_Id = 1 And x.FK_MsJenisTagihanReportParentId = strParentId).ToList()

            If listData.Count > 0 Then
                Dim IntCount As Integer = 1
                For Each objData As RWA_MsJenisTagihanReport In listData
                    Dim dRowData As DataRow = P_dTable1.NewRow

                    'Nomor hanya diisi utk parent paling atas saja
                    If IntLevel = 0 Then
                        dRowData("No") = CStr(IntCount)
                    Else
                        dRowData("No") = ""
                    End If
                    dRowData("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + objData.NamaJenisTagihanReport
                    dRowData("TotalTagihan") = 0
                    dRowData("TotalCKPN") = 0
                    dRowData("TotalTagihanBersih") = 0
                    P_dTable1.Rows.Add(dRowData)

                    P_strSpace += "  "

                    Dim strWhereCond As String = objData.PK_MsJenisTagihanReportId.ToString()

                    pr_GetReportTable1(P_dTable1, strWhereCond, objData.PK_MsJenisTagihanReportId.ToString(), P_strSpace, IntLevel + 1)
                    IntCount += 1
                Next

            Else

                'Dim strWhereCond As String = MsJenisPortofolioAsetColumn.Fk_MsJenisTagihanReport_Id.ToString() + "=" + P_strPK_ID
                Dim ListMsJenisPortofolioAset As List(Of RWA_MsJenisPortofolioAset) = ObjDb.RWA_MsJenisPortofolioAset.Where(Function(x) x.Fk_MsJenisTagihanReport_Id = P_strPK_ID).ToList

                If ListMsJenisPortofolioAset.Count = 1 Then
                    'Kalau cuma ada 1 portofolio Aset, maka nilainya dinaikkan ke tagihan report di atasnya
                    Dim dTable As DataTable = pr_getReportValueTable1(ListMsJenisPortofolioAset)

                    Dim IntParentIndex As Integer = P_dTable1.Rows.Count - 1
                    P_dTable1.Rows(IntParentIndex).Item("TotalTagihan") = dTable.Rows(0).Item("TotalTagihan")
                    P_dTable1.Rows(IntParentIndex).Item("TotalCKPN") = dTable.Rows(0).Item("TotalCKPN")
                    P_dTable1.Rows(IntParentIndex).Item("TotalTagihanBersih") = dTable.Rows(0).Item("TotalTagihanBersih")

                ElseIf ListMsJenisPortofolioAset.Count > 1 Then
                    Dim dTable As DataTable = pr_getReportValueTable1(ListMsJenisPortofolioAset)
                    Dim IntCount As Integer = 1
                    For Each dRow As DataRow In dTable.Rows

                        Dim dRowData As DataRow = P_dTable1.NewRow
                        dRowData("NamaJenisTagihanReport") = Me.GetCurrentNumberFormat(IntLevel, IntCount) + dRow.Item("NamaJenisTagihanReport").ToString()
                        dRowData("TotalTagihan") = dRow.Item("TotalTagihan")
                        dRowData("TotalCKPN") = dRow.Item("TotalCKPN")
                        dRowData("TotalTagihanBersih") = dRow.Item("TotalTagihanBersih")
                        P_dTable1.Rows.Add(dRowData)
                        IntCount += 1
                    Next
                End If

            End If
        End Using
    End Sub

    Private Function pr_getMonthAndYearParameterString() As String
        Return " AND R.ProcessMonth = " + Month(DatTanggalData.SelectedValue).ToString + " AND R.ProcessYear = " + Year(DatTanggalData.SelectedValue).ToString
    End Function

    Private Function pr_getReportValueTable1(ByVal P_ListMsJenisPortofolioAset As List(Of RWA_MsJenisPortofolioAset)) As DataTable
        Dim strWhereCond As String = String.Empty
        For Each objMsJenisPortofolioAset As RWA_MsJenisPortofolioAset In P_ListMsJenisPortofolioAset
            If strWhereCond.Trim() <> String.Empty Then
                strWhereCond += ","
            End If
            strWhereCond += objMsJenisPortofolioAset.Pk_MsJenisPortofolioAset_Id.ToString
        Next

        Dim strCommandText As String = "SELECT mjpa.NamaJenisPortofolioAset AS NamaJenisTagihanReport, ISNULL(r.TotalTagihan, 0) AS TotalTagihan, ISNULL(r.TotalCKPN, 0) AS TotalCKPN, ISNULL(r.TotalTagihanBersih, 0) AS TotalTagihanBersih" +
                                            " FROM RWA_MsJenisPortofolioAset mjpa" +
                                            " INNER JOIN RWA_MsJenisTagihanReport mjtr ON mjpa.Fk_MsJenisTagihanReport_Id = mjtr.PK_MsJenisTagihanReportId" +
                                            " LEFT JOIN RWA_ReportBI_1A_Aset r ON r.Fk_MsJenisPortofolioAset_Id = mjpa.Pk_MsJenisPortofolioAset_Id AND r.Fk_MsJenisTagihanReport_Id = mjpa.Fk_MsJenisTagihanReport_Id" +
                                            pr_getMonthAndYearParameterString() +
                                            " WHERE mjpa.Pk_MsJenisPortofolioAset_Id IN (" & strWhereCond & ")" +
                                            " ORDER BY mjpa.Pk_MsJenisPortofolioAset_Id"

        Dim dTable As DataTable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, strCommandText, Nothing)

        Return dTable
    End Function

End Class
