Imports NawaDAL
Imports Ext.Net
Public Class MGroupAccessDelete
    Inherits Parent




    Public Property GroupName() As String
        Get
            Return Session("MGroupAccessDelete.GroupName")
        End Get
        Set(ByVal value As String)
            Session("MGroupAccessDelete.GroupName") = value
        End Set
    End Property


    Public Property objMGroupMenuSettting() As List(Of NawaDAL.MGroupMenuSettting)
        Get
            Return Session("MGroupAccessDelete.objMGroupMenuSettting")
        End Get
        Set(ByVal value As List(Of NawaDAL.MGroupMenuSettting))
            Session("MGroupAccessDelete.objMGroupMenuSettting") = value
        End Set
    End Property


    Public Property ObjMGroupMenuSetttingEdit() As NawaDAL.MGroupMenuSettting
        Get
            Return Session("MGroupAccessDelete.ObjMGroupMenuSetttingEdit")
        End Get
        Set(ByVal value As NawaDAL.MGroupMenuSettting)
            Session("MGroupAccessDelete.ObjMGroupMenuSetttingEdit") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("MGroupAccessDelete.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MGroupAccessDelete.ObjModule") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Dim intModuleid As Integer
            Try
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)


                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Delete) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If

                GridpanelAdd.Title = ObjModule.ModuleLabel & " Delete"




            Catch ex As Exception

            End Try


            If Not Ext.Net.X.IsAjaxRequest Then
                LoadData()
                LoadDataEdit()
            End If




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub LoadData()

        Dim strgroupname As String = Request.Params("ID")
        Try

            GroupName = NawaBLL.Common.DecryptQueryString(strgroupname, NawaBLL.SystemParameterBLL.GetEncriptionKey)
            txtGroupMenuName.Text = GroupName
        Catch ex As Exception
            Throw
        End Try

        objMGroupMenuSettting = NawaBLL.MGroupAccessBLL.GetMGroupMenuSettingEdit(GroupName)

    End Sub

  

    Protected Sub BtnCancelSave_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try

    End Sub
  







    Protected Sub LoadDataEdit()

        Try


            Store_ReadData(Nothing, Nothing)




            TreePanelMenu.Hidden = False

            Using objgroupaccessbll As New NawaBLL.MGroupAccessBLL
                objgroupaccessbll.CreateTree(TreePanelMenu, objMGroupMenuSettting, ObjModule)
            End Using
            TreePanelMenu.Render()



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try

            StoreView.DataSource = NawaBLL.MGroupAccessBLL.GetGroupMenuAccessEdit(GroupName)
            StoreView.DataBind()




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub


    Protected Sub BtnSaveData_DirectClick(sender As Object, e As DirectEventArgs)
        Try


            'cek ada di approval ?
            If NawaBLL.MGroupAccessBLL.IsDataValidEdit(GroupName, ObjModule) Then



                Dim grid1 As String = e.ExtraParams("Grid1")
                Dim oNode As System.Xml.XmlNode = JSON.DeserializeXmlNode("{records:{record:" + grid1 + "}}")


                Dim objlistMGroupMenuAccess As New List(Of MGroupMenuAccess)


                Dim objnewMGroupMenuAccess As MGroupMenuAccess


                For Each item As System.Xml.XmlNode In oNode.SelectNodes("records/record")


                    Dim objrand As New Random
                    objnewMGroupMenuAccess = New MGroupMenuAccess
                    objnewMGroupMenuAccess.PK_MGroupMenuAcess_ID = objrand.Next
                    objnewMGroupMenuAccess.FK_Module_ID = item.SelectSingleNode("PK_Module_ID").InnerXml
                    objnewMGroupMenuAccess.FK_GroupMenu_ID = item.SelectSingleNode("PK_MGroupMenu_ID").InnerXml

                    If item.SelectSingleNode("BAdd").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bAdd = True
                    Else
                        objnewMGroupMenuAccess.bAdd = False
                    End If



                    If item.SelectSingleNode("BEdit").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bEdit = True
                    Else
                        objnewMGroupMenuAccess.bEdit = False
                    End If



                    If item.SelectSingleNode("BDelete").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bDelete = True
                    Else
                        objnewMGroupMenuAccess.bDelete = False
                    End If



                    If item.SelectSingleNode("BView").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bView = True
                    Else
                        objnewMGroupMenuAccess.bView = False
                    End If


                    If item.SelectSingleNode("BActivation").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bActivation = True
                    Else
                        objnewMGroupMenuAccess.bActivation = False
                    End If



                    If item.SelectSingleNode("BApproval").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bApproval = True
                    Else
                        objnewMGroupMenuAccess.bApproval = False
                    End If

                    If item.SelectSingleNode("BUpload").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bUpload = True
                    Else
                        objnewMGroupMenuAccess.bUpload = False

                    End If

                    If item.SelectSingleNode("BDetail").InnerXml = "true" Then
                        objnewMGroupMenuAccess.bDetail = True
                    Else
                        objnewMGroupMenuAccess.bDetail = False
                    End If



                    objlistMGroupMenuAccess.Add(objnewMGroupMenuAccess)
                Next



                Dim objData As New NawaBLL.ModuleAccessApprovalData
                objData.oMGroupMenuAccess = objlistMGroupMenuAccess
                objData.oMGroupMenuSettting = objMGroupMenuSettting


                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (ObjModule.IsUseApproval) Then

                    NawaBLL.MGroupAccessBLL.SaveDeleteTanpaApproval(objData, ObjModule)
                    Panelconfirmation.Hidden = False
                    Panel1.Hidden = True
                    LblConfirmation.Text = "Data Deleted from Database"
                Else
                    NawaBLL.MGroupAccessBLL.SaveDeleteApproval(objData, ObjModule)
                    Panelconfirmation.Hidden = False
                    Panel1.Hidden = True
                    LblConfirmation.Text = "Data Saved into Pending Approval"
                End If


            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
 

   


   
End Class