Imports NawaDAL
Partial Class AuditTrailDetail
    Inherits Parent



    Public Property ObjAuditTrailHeader() As AuditTrailHeader
        Get

            Return Session("AuditTrailDetail.ObjAuditTrailHeader")
        End Get
        Set(ByVal value As AuditTrailHeader)
            Session("AuditTrailDetail.ObjAuditTrailHeader") = value
        End Set
    End Property


    Public Property ObjListAuditTrailDetail() As List(Of NawaDAL.AuditTrailDetail)
        Get
            Return Session("AuditTrailDetail.ObjListAuditTrailDetail")
        End Get
        Set(ByVal value As List(Of NawaDAL.AuditTrailDetail))
            Session("AuditTrailDetail.ObjListAuditTrailDetail") = value
        End Set
    End Property



    Public Property StrUnikKey() As String
        Get
            Return Session("AuditTrailDetail.StrUnikKey")
        End Get
        Set(ByVal value As String)
            Session("AuditTrailDetail.StrUnikKey") = value
        End Set
    End Property
    Public Property objmodule() As NawaDAL.Module
        Get
            Return Session("AuditTrailDetail.objmodule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("AuditTrailDetail.objmodule") = value
        End Set
    End Property

    Sub ClearSession()
        objmodule = Nothing
        StrUnikKey = Nothing
        ObjListAuditTrailDetail = Nothing
        ObjAuditTrailHeader = Nothing
    End Sub

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        Try


            Dim Moduleid As String = Request.Params("ModuleID")
            Dim IDData As String = Request.Params("ID")
            Dim intModuleid As Integer
            Try
                If Not Ext.Net.X.IsAjaxRequest Then
                    ClearSession()
                End If
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objmodule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)

                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Detail) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If
                FormPanelInput.Title = objmodule.ModuleLabel & " Detail"
                Panelconfirmation.Title = objmodule.ModuleLabel & " Detail"
                StrUnikKey = NawaBLL.Common.DecryptQueryString(IDData, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                If Not Ext.Net.X.IsAjaxRequest Then
                    'done:buat method loadpaneldelete

                    'NawaBLL.EODSchedulerBLL.LoadPanelActivation(FormPanelInput, objmodule.ModuleName, StrUnikKey)

                    LoadData()

                End If


            Catch ex As Exception

            End Try



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub LoadData()
        ObjAuditTrailHeader = NawaBLL.AuditTrailHeaderBLL.GetAuditTrailHeader(StrUnikKey)
        ObjListAuditTrailDetail = NawaBLL.AuditTrailHeaderBLL.GetListAuditTrailDetail(StrUnikKey)
        If Not ObjAuditTrailHeader Is Nothing Then
            With ObjAuditTrailHeader
                LblID.Text = .PK_AuditTrail_ID
                LblCreatedDate.Text = .CreatedDate.GetValueOrDefault.ToString(NawaBLL.SystemParameterBLL.GetDateFormat & " HH:mm:ss")
                LblCreatedBy.Text = .CreatedBy
                LblApproveBy.Text = .ApproveBy
                LblModuleLabel.Text = .ModuleLabel
                lblModuleAction.Text = NawaBLL.ModuleBLL.GetModuleActionByID(.FK_ModuleAction_ID).ModuleActionName
                lblStatusAuditTrail.Text = NawaBLL.AuditTrailHeaderBLL.GetAuditTrailStatus(.FK_AuditTrailStatus_ID).AuditTrailStatusName

                storedetail.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                storedetail.Reload()
                
            End With

        End If

    End Sub


    Private Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs) Handles BtnConfirmation.DirectClick
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & objmodule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
    Protected Sub BtnCancel_Click(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & objmodule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub

    Protected Sub StoreDetail_readdata(sender As Object, e As StoreReadDataEventArgs)


        storedetail.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("AuditTrailDetail", "PK_AuditTrailDetail_id, FK_AuditTrailHeader_ID, FieldName,OldValue, NewValue", "FK_AuditTrailHeader_ID=" & ObjAuditTrailHeader.PK_AuditTrail_ID, "PK_AuditTrailDetail_id", e.Start, e.Limit, e.Total)
        storedetail.DataBind()

        
    End Sub
End Class
