Imports NawaDAL
Imports Ext.Net
Public Class MGroupAccessAdd
    Inherits Parent



    Protected Sub ChangePage(sender As Object, e As DirectEventArgs)
        Try

            If Session("Awal") = "Awal" Then
                Session("Awal") = ""
            Else
                If btnSelectAll.Checked Then
                    btnSelectAll.Tag = "disabled"
                End If

                btnSelectAll.Clear()
            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Public Property objMGroupMenuSettting() As List(Of NawaDAL.MGroupMenuSettting)
        Get
            Return Session("MGroupAccessAdd.objMGroupMenuSettting")
        End Get
        Set(ByVal value As List(Of NawaDAL.MGroupMenuSettting))
            Session("MGroupAccessAdd.objMGroupMenuSettting") = value
        End Set
    End Property


    Public Property ObjMGroupMenuSetttingEdit() As NawaDAL.MGroupMenuSettting
        Get
            Return Session("MGroupAccessAdd.ObjMGroupMenuSetttingEdit")
        End Get
        Set(ByVal value As NawaDAL.MGroupMenuSettting)
            Session("MGroupAccessAdd.ObjMGroupMenuSetttingEdit") = value
        End Set
    End Property

    Public Property ObjModule() As NawaDAL.Module
        Get
            Return Session("MGroupAccessAdd.ObjModule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MGroupAccessAdd.ObjModule") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Dim intModuleid As Integer
            Try
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                ObjModule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)


                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, ObjModule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Insert) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)

                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If

                GridpanelAdd.Title = ObjModule.ModuleLabel & " Add"
            

                

            Catch ex As Exception

            End Try


            If Not Ext.Net.X.IsAjaxRequest Then
                Session("Awal") = Nothing
                LoadGroupMenu()

            End If




        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Sub LoadGroupMenu()
        CboGroupmenu_Read(Nothing, Nothing)
    End Sub

    Protected Sub BtnSaveMenu_Click(sender As Object, e As DirectEventArgs)
        Try

            If Not ObjMGroupMenuSetttingEdit Is Nothing Then
                ObjMGroupMenuSetttingEdit.mMenuLabel = MenuLabel.Text


                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value

                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)



                Dim objselectedtree As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
                objselectedtree.ToProxyNode.SetText(ObjMGroupMenuSetttingEdit.mMenuLabel)
                objselectedtree.ToProxyNode.Set("mMenu", ObjMGroupMenuSetttingEdit)
                objselectedtree.ToProxyNode.Set("MenuLabel", ObjMGroupMenuSetttingEdit.mMenuLabel)
                objselectedtree.ToProxyNode.Set("MenuURL", ObjMGroupMenuSetttingEdit.mMenuURL)

                Dim strModule As String = ""

                If Not objdatamodule Is Nothing Then
                    strModule = objdatamodule.ModuleLabel
                End If

                Dim straction As String = ""
                Dim objaction As NawaDAL.ModuleAction = NawaBLL.ModuleBLL.GetModuleActionByID(ObjMGroupMenuSetttingEdit.FK_Action_ID.GetValueOrDefault(0))
                If Not objaction Is Nothing Then
                    straction = objaction.ModuleActionName
                End If

                objselectedtree.ToProxyNode.Set("moduleid", strModule)
                objselectedtree.ToProxyNode.Set("actionid", straction)



            End If

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As DirectEventArgs)
        Try

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub StoreAction_ReadData(sender As Object, e As StoreReadDataEventArgs)

        Try
            Dim query As String = e.Parameters("query")
            If query Is Nothing Then query = ""
            Dim strfilter As String = ""
            If query.Length > 0 Then
                strfilter = " ModuleActionName like '" & query & "%' "
            End If

            Dim moduleid As String = cboModule.SelectedItem.Value
            Dim strtable As String
            If moduleid Is Nothing Then
                strtable = "ModuleAction"
            Else
                strtable = "ModuleAction ma  INNER JOIN (SELECT CASE WHEN  m.IsSupportView =1 THEN 5 ELSE 0 END AS pk    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " UNION  SELECT CASE WHEN  m.IsUseApproval =1 THEN 6 ELSE 0 END    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " UNION  SELECT CASE WHEN  m.IsSupportUpload=1 THEN " & moduleid & " ELSE 0 END    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " )xx ON ma.PK_ModuleAction_ID=xx.pk"
            End If


            StoreAction.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging(strtable, "PK_ModuleAction_ID, ModuleActionName", strfilter, "PK_ModuleAction_ID", e.Start, e.Limit, e.Total)
            StoreAction.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    <DirectMethod>
    Public Sub ClearURL()
        MenuURL.Text = ""
    End Sub

    'Protected Sub btnDelete_DirectClick(sender As Object, e As DirectEventArgs)
    '    Try

    '        If TreePanelMenu.SelectedNodes.Count > 0 Then


    '            Dim objdel As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)

    '            Dim objmenudel As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objdel.Attributes("mMenuID").ToString)

    '            'TreePanelMenu.Root.Remove(TreePanelMenu.Root.Where(Function(x) x.NodeID = objdel.NodeID).First())
    '            Dim obnodedel As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objdel.NodeID)
    '            obnodedel.Remove()

    '            objMGroupMenuSettting.Remove(objmenudel)


    '        End If


    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub


    'Protected Sub btnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
    '    Try
    '        If TreePanelMenu.SelectedNodes.Count > 0 Then
    '            Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)

    '            Dim objmenuactive As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)

    '            Dim obnodeselect As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objactivenode.NodeID)
    '            obnodeselect.Expand(False)

    '            Dim objnewnode As New Ext.Net.Node


    '            Dim objnewMenutemplate As New NawaDAL.MGroupMenuSettting
    '            Dim objrand As New Random
    '            objnewMenutemplate.PK_MGroupMenuSettting_ID = objrand.Next
    '            objnewMenutemplate.FK_MGroupMenu_ID = cboGroupMenu.SelectedItem.Value
    '            objnewMenutemplate.mMenuLabel = "New Menu"
    '            objnewMenutemplate.mMenuURL = ""
    '            objnewMenutemplate.mMenuParentID = objactivenode.NodeID
    '            objnewMenutemplate.mMenuID = objactivenode.NodeID & "/" & objactivenode.Children.Count + 1
    '            objnewMenutemplate.urutan = objactivenode.Children.Count
    '            objMGroupMenuSettting.Add(objnewMenutemplate)



    '            objnewnode.Text = "New Menu"
    '            objnewnode.NodeID = objactivenode.NodeID & "/" & objactivenode.Children.Count + 1
    '            objnewnode.CustomAttributes.Add(New ConfigItem("mMenu", objnewMenutemplate))
    '            objnewnode.CustomAttributes.Add(New ConfigItem("mMenuID", objnewMenutemplate.mMenuID, ParameterMode.Value))
    '            objnewnode.CustomAttributes.Add(New ConfigItem("MenuLabel", objnewMenutemplate.mMenuLabel))


    '            obnodeselect.AppendChild(objnewnode)

    '            'TreePanelMenu.GetSelectionModel().Select(objnewnode.NodeID)





    '        End If
    '    Catch ex As Exception
    '        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
    '        Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
    '    End Try
    'End Sub
    Protected Sub RemoteMove(sender As Object, e As RemoteMoveEventArgs)
        Try



            e.Accept = True

            Dim objmove As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = e.NodeID)

            objmove.mMenuParentID = e.TargetNodeID
            Dim objfilter As List(Of MGroupMenuSettting) = objMGroupMenuSettting.FindAll(Function(x) x.mMenuParentID = e.TargetNodeID)
            objmove.urutan = objfilter.Count + 1

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub



    
    <DirectMethod>
    Public Sub IsHaveRight(data As Object)

        Dim i As Integer
        i = 10


    End Sub


    Protected Sub BtnOk_DirectClick(sender As Object, e As DirectEventArgs)

        Try
            Store_ReadData(Nothing, Nothing)
            StoreView.Reload()

            If cboGroupMenu.SelectedItem.Value = "" Then
                objMGroupMenuSettting = Nothing
                TreePanelMenu.Hidden = True
                PanelData.Hidden = True
            Else
                objMGroupMenuSettting = NawaBLL.MGroupAccessBLL.GetMGroupMenuSettingNew(cboGroupMenu.SelectedItem.Value)
                TreePanelMenu.Hidden = False
                PanelData.Hidden = False
                Using objgroupaccessbll As New NawaBLL.MGroupAccessBLL
                    objgroupaccessbll.CreateTree(TreePanelMenu, objMGroupMenuSettting, ObjModule)
                End Using
                TreePanelMenu.Render()
                PanelData.Render()
                Session("Awal") = "Awal"

            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub Store_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            StoreView.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
            StoreView.DataSource = NawaBLL.MGroupAccessBLL.GetGroupMenuAccessAdd(cboGroupMenu.SelectedItem.Value)
            StoreView.DataBind()


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try


    End Sub
    Protected Sub CboGroupmenu_Read(sender As Object, e As StoreReadDataEventArgs)
        GroupMenuStore.DataSource = NawaBLL.MGroupAccessBLL.GetGroupMenuAdd
        GroupMenuStore.DataBind()
    End Sub


    Protected Sub BtnSaveData_DirectClick(sender As Object, e As DirectEventArgs)
        Try


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



                NawaBLL.MGroupAccessBLL.SaveAddTanpaApproval(objData, ObjModule, "$AppPathApplication$")
                Panelconfirmation.Hidden = False
                Panel1.Hidden = True
                LblConfirmation.Text = "Data Saved into Database"
            Else
                NawaBLL.MGroupAccessBLL.SaveAddApproval(objData, ObjModule)
                Panelconfirmation.Hidden = False
                Panel1.Hidden = True
                LblConfirmation.Text = "Data Saved into Pending Approval"
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


    Protected Sub BtnCancelSave_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & ObjModule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try

    End Sub
    Protected Sub MenuModule_Select(sender As Object, e As DirectEventArgs)
        Try


            If Not ObjMGroupMenuSetttingEdit Is Nothing Then


                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value

                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not objdatamodule Is Nothing Then
                    If actionid = NawaBLL.Common.ModuleActionEnum.view Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlView
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Approval Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlApproval
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Upload Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlUpload
                    End If

                    MenuURL.Text = ObjMGroupMenuSetttingEdit.mMenuURL
                Else
                    MenuURL.Text = ""
                End If


            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub cboAction_DirectClick(sender As Object, e As DirectEventArgs)
        Try


            If Not ObjMGroupMenuSetttingEdit Is Nothing Then


                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value

                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not objdatamodule Is Nothing Then
                    ObjMGroupMenuSetttingEdit.FK_Module_ID = moduleid

                    If actionid = NawaBLL.Common.ModuleActionEnum.view Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlView
                        ObjMGroupMenuSetttingEdit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Approval Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlApproval
                        ObjMGroupMenuSetttingEdit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Upload Then
                        ObjMGroupMenuSetttingEdit.mMenuURL = objdatamodule.UrlUpload
                        ObjMGroupMenuSetttingEdit.FK_Action_ID = actionid
                    End If

                    MenuURL.Text = ObjMGroupMenuSetttingEdit.mMenuURL
                Else
                    MenuURL.Text = ""
                    ObjMGroupMenuSetttingEdit.mMenuURL = ""
                    ObjMGroupMenuSetttingEdit.FK_Module_ID = Nothing
                    ObjMGroupMenuSetttingEdit.FK_Action_ID = Nothing
                End If


            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub StoreModule_ReadData(sender As Object, e As StoreReadDataEventArgs)

        Try
            Dim query As String = e.Parameters("query")
            If query Is Nothing Then query = ""
            Dim strfilter As String = ""
            If query.Length > 0 Then
                strfilter = " ModuleLabel like '" & query & "%' and active=1"
            Else
                strfilter = " active=1"
            End If

            StoreModule.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("Module", "PK_Module_ID,ModuleLabel", strfilter, "PK_Module_ID", e.Start, e.Limit, e.Total)
            StoreModule.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Protected Sub btnDown_directClick(sender As Object, e As DirectEventArgs)
        Try

            If TreePanelMenu.SelectedNodes.Count > 0 Then

                Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)


                Dim objmenuactive As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)

                Dim objmenunext As MGroupMenuSettting = objMGroupMenuSettting.FindLast(Function(x) x.mMenuParentID = objmenuactive.mMenuParentID And x.urutan > objmenuactive.urutan)

                'Dim obnodeselect As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objactivenode.NodeID)
                Dim obnodeselect As Ext.Net.NodeProxy = objactivenode.ToProxyNode




                Dim objnext As Ext.Net.NodeProxy = obnodeselect.NextSibling()
                If Not objmenunext Is Nothing Then


                    Dim urutanactive As Integer = objmenuactive.urutan
                    Dim urutannext As Integer = objmenunext.urutan


                    objmenuactive.urutan = urutannext
                    objmenunext.urutan = urutanactive
                    objactivenode.ToProxyNode.PreviousSibling()

                    obnodeselect.ParentNode.InsertBefore(objnext, obnodeselect)

                End If



            End If



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

    Protected Sub btnUp_directClick(sender As Object, e As DirectEventArgs)
        Try

            If TreePanelMenu.SelectedNodes.Count > 0 Then

                Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)


                Dim objmenuactive As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)

                Dim objmenuprev As MGroupMenuSettting = objMGroupMenuSettting.FindLast(Function(x) x.mMenuParentID = objmenuactive.mMenuParentID And x.urutan < objmenuactive.urutan)

                'Dim obnodeselect As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objactivenode.NodeID)
                Dim obnodeselect As Ext.Net.NodeProxy = objactivenode.ToProxyNode




                Dim objprev As Ext.Net.NodeProxy = obnodeselect.PreviousSibling()
                If Not objmenuprev Is Nothing Then


                    Dim urutanactive As Integer = objmenuactive.urutan
                    Dim urutanprev As Integer = objmenuprev.urutan


                    objmenuactive.urutan = urutanprev
                    objmenuprev.urutan = urutanactive
                    objactivenode.ToProxyNode.PreviousSibling()

                    obnodeselect.ParentNode.InsertBefore(obnodeselect, objprev)

                End If



            End If



        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub treepanel_itemClick(sender As Object, e As DirectEventArgs)
        Try



            ObjMGroupMenuSetttingEdit = Nothing
            Dim objparam As String = e.ExtraParams("ID").ToString
            Dim objresult As NawaDAL.MGroupMenuSettting = Newtonsoft.Json.JsonConvert.DeserializeObject(Of MGroupMenuSettting)(objparam)

            '   Dim objresult As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objparam)


            If Not objresult Is Nothing Then


                ObjMGroupMenuSetttingEdit = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objresult.mMenuID)

                'btnAddNew.Enable()
                'btnDelete.Enable()


                btnUp.Enable()
                btnDown.Enable()
                IDData.Text = objresult.mMenuID

                IDData.Text = objresult.mMenuID


                MenuLabel.Text = objresult.mMenuLabel
                Dim objparent As MGroupMenuSettting = objMGroupMenuSettting.Find(Function(x) x.mMenuID = objresult.mMenuParentID)
                If Not objparent Is Nothing Then
                    MenuParent.Text = objparent.mMenuLabel
                Else
                    MenuParent.Text = "Root"
                End If

                MenuURL.Text = objresult.mMenuURL
                Dim objmodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(objresult.FK_Module_ID.GetValueOrDefault(0))
                If Not objmodule Is Nothing Then

                    cboModule.DoQuery(objmodule.ModuleLabel, True)
                    cboModule.SetValueAndFireSelect(objmodule.PK_Module_ID)


                Else
                    cboModule.Text = ""
                End If


                Dim objaction As NawaDAL.ModuleAction = NawaBLL.ModuleBLL.GetModuleActionByID(objresult.FK_Action_ID.GetValueOrDefault(0))
                If Not objaction Is Nothing Then
                    cboAction.DoQuery(objaction.ModuleActionName, True)
                    cboAction.SetValueAndFireSelect(objaction.PK_ModuleAction_ID)

                Else
                    cboAction.Text = ""
                End If
            Else
                IDData.Text = ""
                MenuLabel.Text = "Root"
                MenuURL.Text = ""
                cboAction.Text = ""
                cboModule.Text = ""
                MenuParent.Text = ""


                'btnAddNew.Enable()
                'btnDelete.Disable()

                btnUp.Disable()
                btnDown.Disable()

            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub

End Class