Imports NawaBLL
Partial Class AuditTrailEODViewDetail
    Inherits ParentPage
    Enum EodTaskDetailTypeEnum
        SSIS = 1
        Storeprocedure
        SQLserveragent
        API
    End Enum


    Public Property IDUnik() As String
        Get
            Return Session("AuditTrailEODViewDetail.IDUnik")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailEODViewDetail.IDUnik") = value
        End Set
    End Property

    Private Sub AuditTrailEODView_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        ActionType = NawaBLL.Common.ModuleActionEnum.Detail

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            Dim IDData As String = Request.Params("ID")
            IDUnik = NawaBLL.Common.DecryptQueryString(IDData, NawaBLL.SystemParameterBLL.GetEncriptionKey)


            If Not Ext.Net.X.IsAjaxRequest Then
                FormPanelInput.Title = ObjModule.ModuleLabel
                LoadDataDetail()

            End If



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try

    End Sub

    Sub LoadDataDetail()
        Dim objTaskDetailEODLog As NawaDAL.EODTaskDetailLog = NawaBLL.AuditTrailEODBLL.GetTaskDetailEODLogbypk(IDUnik)
        Dim objTaskDetailEOD As NawaDAL.EODTaskDetail = NawaBLL.AuditTrailEODBLL.GetTaskDetailEODbypk(objTaskDetailEODLog.FK_EODTAskDetail_ID)
        If Not objTaskDetailEOD Is Nothing Then
            If objTaskDetailEOD.FK_EODTaskDetailType_ID = EodTaskDetailTypeEnum.SSIS Then

                'ambil log ssis
                If Not objTaskDetailEODLog.executionID Is Nothing Then
                    NawaBLL.AuditTrailEODBLL.LoadDataAuditTrailEODSSIS(FormPanelInput, objTaskDetailEODLog.executionID)
                End If

            ElseIf objTaskDetailEOD.FK_EODTaskDetailType_ID = EodTaskDetailTypeEnum.Storeprocedure Or objTaskDetailEOD.FK_EODTaskDetailType_ID = EodTaskDetailTypeEnum.API Then
                'ambil log storeprocedure
                If Not objTaskDetailEODLog.executionID Is Nothing Then
                    NawaBLL.AuditTrailEODBLL.LoadDataAuditTrailEODSP(FormPanelInput, objTaskDetailEODLog.executionID)
                End If
            End If
        End If
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub


End Class
