Imports Ext.Net
Imports NawaBLL
Imports NawaDAL
Public Class MenuTemplateEdit
    Inherits Parent
    Public Property objMenuTemplateedit() As NawaDAL.MenuTemplate
        Get
            Return Session("MenuTemplateEdit.objMenuTemplateedit")
        End Get
        Set(ByVal value As NawaDAL.MenuTemplate)
            Session("MenuTemplateEdit.objMenuTemplateedit") = value
        End Set
    End Property
    Public Property objmodule() As NawaDAL.Module
        Get
            Return Session("MenuTemplateEdit.objmodule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MenuTemplateEdit.objmodule") = value
        End Set
    End Property
    Public Property objMenutemplate() As List(Of NawaDAL.MenuTemplate)
        Get
            Return Session("MenuTemplateEdit.objMenutemplate")
        End Get
        Set(ByVal value As List(Of NawaDAL.MenuTemplate))
            Session("MenuTemplateEdit.objMenutemplate") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Moduleid As String = Request.Params("ModuleID")
            Dim intModuleid As Integer
            Try
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objmodule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)
                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.Update) Then
                    Dim strIDCode As String = 1
                    strIDCode = NawaBLL.Common.EncryptQueryString(strIDCode, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                    Ext.Net.X.Redirect(String.Format(NawaBLL.Common.GetApplicationPath & "/UnAuthorizeAccess.aspx?ID={0}", strIDCode), "Loading...")
                    Exit Sub
                End If
            Catch ex1 As Exception
            End Try
            If Not Ext.Net.X.IsAjaxRequest Then
                Panel6.Title = objmodule.ModuleLabel
                PanelData.Title = objmodule.ModuleLabel & " Detail"
                cboAction.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                StoreAction.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                cboModule.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                StoreModule.PageSize = NawaBLL.SystemParameterBLL.GetPageSize
                objMenutemplate = MenuTemplateBLL.GetMenuTemplate
            End If
            Using objtem As New NawaBLL.MenuTemplateBLL
                objtem.GetMenuTemplateEdit(TreePanelMenu, objmodule)
                If Not Ext.Net.X.IsAjaxRequest Then
                    Ext.Net.X.Js.Call("refreshmenu")
                End If
            End Using
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnDown_directClick(sender As Object, e As DirectEventArgs)
        Try
            'done: btnDown_directClick
            If Not TreePanelMenu.SelectedNodes Is Nothing Then
                Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
                Dim objmenuactive As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)
                Dim objmenunext As MenuTemplate = objMenutemplate.Where(Function(x) x.mMenuParentID = objmenuactive.mMenuParentID And x.urutan > objmenuactive.urutan).OrderBy(Function(x) x.urutan).FirstOrDefault
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
    Protected Sub BtnSaveMenu_Click(sender As Object, e As DirectEventArgs)
        Try
            If Not objMenuTemplateedit Is Nothing Then
                objMenuTemplateedit.mMenuLabel = MenuLabel.Text
                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value
                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)
                Dim objselectedtree As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
                objselectedtree.ToProxyNode.SetText(objMenuTemplateedit.mMenuLabel)
                objselectedtree.ToProxyNode.Set("mMenu", objMenuTemplateedit)
                objselectedtree.ToProxyNode.Set("MenuLabel", objMenuTemplateedit.mMenuLabel)
                objselectedtree.ToProxyNode.Set("MenuURL", objMenuTemplateedit.mMenuURL)
                Dim strModule As String = ""
                If Not objdatamodule Is Nothing Then
                    strModule = objdatamodule.ModuleLabel
                End If
                Dim straction As String = ""
                Dim objaction As NawaDAL.ModuleAction = NawaBLL.ModuleBLL.GetModuleActionByID(objMenuTemplateedit.FK_Action_ID.GetValueOrDefault(0))
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
    Protected Sub BtnConfirmation_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            Dim Moduleid As String = Request.Params("ModuleID")

            Ext.Net.X.Redirect(NawaBLL.Common.GetApplicationPath & objmodule.UrlView & "?ModuleID=" & Moduleid)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()

        End Try
    End Sub
    Protected Sub StoreAction_ReadData(sender As Object, e As StoreReadDataEventArgs)
        Try
            'done: StoreAction_ReadData
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
                strtable = "ModuleAction ma  INNER JOIN (SELECT CASE WHEN  m.IsSupportView =1 THEN 5 ELSE 0 END AS pk    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " UNION  SELECT CASE WHEN  m.IsUseApproval =1 THEN 6 ELSE 0 END    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " UNION  SELECT CASE WHEN  m.IsSupportUpload=1 THEN 7 ELSE 0 END    FROM Module m WHERE m.PK_Module_ID=" & moduleid & " )xx ON ma.PK_ModuleAction_ID=xx.pk"
            End If
            StoreAction.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging(strtable, "PK_ModuleAction_ID, ModuleActionName", strfilter, "PK_ModuleAction_ID", e.Start, e.Limit, e.Total)
            StoreAction.DataBind()
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub cboAction_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            'done: cboAction_DirectClick

            If Not objMenuTemplateedit Is Nothing Then


                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value

                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not objdatamodule Is Nothing Then
                    objMenuTemplateedit.FK_Module_ID = moduleid

                    If actionid = NawaBLL.Common.ModuleActionEnum.view Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlView
                        objMenuTemplateedit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Approval Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlApproval
                        objMenuTemplateedit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Upload Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlUpload
                        objMenuTemplateedit.FK_Action_ID = actionid
                    End If

                    MenuURL.Text = objMenuTemplateedit.mMenuURL
                Else
                    MenuURL.Text = ""
                    objMenuTemplateedit.mMenuURL = ""
                    objMenuTemplateedit.FK_Module_ID = Nothing
                    objMenuTemplateedit.FK_Action_ID = Nothing
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
    Protected Sub MenuModule_Select(sender As Object, e As DirectEventArgs)
        Try

            If Not objMenuTemplateedit Is Nothing Then


                Dim moduleid As String = cboModule.SelectedItem.Value
                Dim actionid As String = cboAction.SelectedItem.Value

                Dim objdatamodule As NawaDAL.Module = NawaBLL.ModuleBLL.GetModuleByModuleID(moduleid)

                If Not objdatamodule Is Nothing Then
                    If actionid = NawaBLL.Common.ModuleActionEnum.view Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlView
                        objMenuTemplateedit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Approval Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlApproval
                        objMenuTemplateedit.FK_Action_ID = actionid
                    ElseIf actionid = NawaBLL.Common.ModuleActionEnum.Upload Then
                        objMenuTemplateedit.mMenuURL = objdatamodule.UrlUpload
                        objMenuTemplateedit.FK_Action_ID = actionid
                    End If

                    MenuURL.Text = objMenuTemplateedit.mMenuURL
                Else
                    MenuURL.Text = ""
                    objMenuTemplateedit.mMenuURL = ""
                    objMenuTemplateedit.FK_Module_ID = Nothing
                    objMenuTemplateedit.FK_Action_ID = Nothing
                End If


            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnUp_directClick(sender As Object, e As DirectEventArgs)
        Try
            'done: btnUp_directClick
            If Not TreePanelMenu.SelectedNodes Is Nothing Then
                Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
                Dim objmenuactive As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)
                Dim objmenuprev As MenuTemplate = objMenutemplate.Where(Function(x) x.mMenuParentID = objmenuactive.mMenuParentID And x.urutan < objmenuactive.urutan).OrderByDescending(Function(x) x.urutan).FirstOrDefault
                Dim obnodeProxyselect As Ext.Net.NodeProxy = objactivenode.ToProxyNode
                Dim objNodeProxyprev As Ext.Net.NodeProxy = obnodeProxyselect.PreviousSibling()
                If Not objmenuprev Is Nothing Then
                    Dim urutanactive As Integer = objmenuactive.urutan
                    Dim urutanprev As Integer = objmenuprev.urutan
                    Dim idactive As String = objmenuactive.mMenuID
                    Dim idprev As String = objmenuprev.mMenuID
                    objmenuactive.urutan = urutanprev
                    objmenuprev.urutan = urutanactive
                    obnodeProxyselect.ParentNode.InsertBefore(obnodeProxyselect, objNodeProxyprev)
                    'Dim objnodeSelect As Node = FindNodeById(TreePanelMenu.Root, objactivenode.NodeID)
                    'objnodeSelect.NodeID = idprev
                    'objnodeSelect.CustomAttributes(0).Value = Newtonsoft.Json.JsonConvert.SerializeObject(objmenuactive)
                    'UpdateNOdeAnak(objmenuactive, idactive, objnodeSelect)
                    'Dim objnodeprev As Node = FindNodeById(CType(sender, TreePanel).Root, idprev)
                    'objnodeprev.NodeID = idactive
                    'objnodeprev.CustomAttributes(0).Value = Newtonsoft.Json.JsonConvert.SerializeObject(objmenuprev)
                    'UpdateNOdeAnak(objmenuprev, idprev, objnodeprev)
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Public Shared Function FindNodeById(tree As TreePanelBase, nodeID As String) As Node

        Return FindNodeById(tree.Root, nodeID)
    End Function

    Private Shared Function FindNodeById(items As NodeCollection, nodeID As String) As Node
        For Each item As Node In items
            If item.NodeID = nodeID Then
                Return TryCast(item, Node)
            Else
                Dim node = FindNodeById(TryCast(item, Node).Children, nodeID)
                If node IsNot Nothing Then
                    Return node
                End If
            End If
        Next
        Return Nothing
    End Function

    Protected Sub RemoteMove(sender As Object, e As RemoteMoveEventArgs)
        Try
            'todohendra: RemoteMove


            e.Accept = True



            Dim objmove As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = e.NodeID)

            objmove.mMenuParentID = e.TargetNodeID

            Dim intcounter As Integer = FindNodeById(CType(sender, TreePanel).Root, e.TargetNodeID).Children.Count
            Dim newid As String = e.TargetNodeID & "/" & intcounter
            Do While Not objMenutemplate.Find(Function(x) x.mMenuID = newid) Is Nothing
                intcounter += 1
                newid = e.TargetNodeID & "/" & intcounter
            Loop
            objmove.mMenuID = newid

            Dim objnodemove As Node = FindNodeById(CType(sender, TreePanel).Root, e.NodeID)



            Dim objselectedtree As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
            objselectedtree.ToProxyNode.Set("mMenu", objmove)
            objselectedtree.ToProxyNode.Reload()





            objnodemove.NodeID = objmove.mMenuID
            objnodemove.ParentNode.NodeID = e.TargetNodeID
            objnodemove.CustomAttributes(0).Value = Newtonsoft.Json.JsonConvert.SerializeObject(objmove)


            '  UpdateNOdeAnak(objmove, e.NodeID, objnodemove)

            'cara dapetin jumlah anak dari nodeproxy penerima.
            ' FindNodeById(CType(sender, TreePanel).Root, e.TargetNodeID).Children.Count



            Dim objfilter As List(Of MenuTemplate) = objMenutemplate.FindAll(Function(x) x.mMenuParentID = e.TargetNodeID)
            objmove.urutan = objfilter.Count + 1

        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnAddNew_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            'done: btnAddNew_DirectClick
            If Not TreePanelMenu.SelectedNodes Is Nothing Then
                Dim objactivenode As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)
                Dim objmenuactive As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objactivenode.Attributes("mMenuID").ToString)
                Dim obnodeselect As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objactivenode.NodeID)
                obnodeselect.Expand(False)
                Dim objnewnode As New Ext.Net.Node
                Dim objnewMenutemplate As New NawaDAL.MenuTemplate
                objnewMenutemplate.mMenuLabel = "New Menu"
                objnewMenutemplate.mMenuURL = ""
                If objactivenode.NodeID = "Root" Then
                    objnewMenutemplate.mMenuParentID = Nothing
                Else
                    objnewMenutemplate.mMenuParentID = objactivenode.NodeID
                End If
                If objactivenode.NodeID = "Root" Then
                    Dim intcounter As Integer = objactivenode.Children.Count + 1
                    Dim strnewid As String = intcounter
                    While objMenutemplate.FindAll(Function(x) x.mMenuID = strnewid).ToList.Count > 0
                        intcounter += 1
                        strnewid = intcounter
                    End While
                    objnewMenutemplate.mMenuID = strnewid

                    Dim inturutan As Integer = objMenutemplate.FindAll(Function(x) x.mMenuParentID Is Nothing And x.urutan = intcounter).ToList.Count()
                    While inturutan > 0
                        intcounter += 1
                        inturutan = objMenutemplate.FindAll(Function(x) x.mMenuParentID Is Nothing And x.urutan = intcounter).ToList.Count()
                    End While
                    objnewMenutemplate.urutan = intcounter
                Else
                    Dim intcounter As Integer = objactivenode.Children.Count + 1
                    Dim strnewid As String = objactivenode.NodeID & "/" & intcounter
                    While objMenutemplate.FindAll(Function(x) x.mMenuID = strnewid).ToList.Count > 0
                        intcounter += 1
                        strnewid = objactivenode.NodeID & "/" & intcounter
                    End While
                    objnewMenutemplate.mMenuID = strnewid
                    intcounter = objactivenode.Children.Count + 1
                    Dim inturutan As Integer = objMenutemplate.FindAll(Function(x) x.mMenuParentID = objactivenode.NodeID And x.urutan = intcounter).ToList.Count()
                    While inturutan > 0
                        intcounter += 1
                        inturutan = objMenutemplate.FindAll(Function(x) x.mMenuParentID = objactivenode.NodeID And x.urutan = intcounter).ToList.Count()
                    End While
                    objnewMenutemplate.urutan = intcounter
                End If
                objMenutemplate.Add(objnewMenutemplate)


                objnewnode.Text = "New Menu"
                objnewnode.NodeID = objnewMenutemplate.mMenuID

                objnewnode.CustomAttributes.Add(New ConfigItem("mMenu", objnewMenutemplate))
                objnewnode.CustomAttributes.Add(New ConfigItem("mMenuID", objnewMenutemplate.mMenuID, ParameterMode.Value))
                objnewnode.CustomAttributes.Add(New ConfigItem("MenuLabel", objnewMenutemplate.mMenuLabel))
                obnodeselect.AppendChild(objnewnode)
                'TreePanelMenu.GetSelectionModel().Select(objnewnode.NodeID)
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub btnSaveMenuAll_DirectClick(sender As Object, e As DirectEventArgs)
        Try
            If NawaBLL.MenuTemplateBLL.IsDataValidEdit(objmodule) Then


                If NawaBLL.Common.SessionCurrentUser.FK_MRole_ID.ToString = 1 OrElse Not (objmodule.IsUseApproval) Then

                    NawaBLL.MenuTemplateBLL.SaveEditTanpaApproval(objMenutemplate, objmodule)
                    Panelconfirmation.Hidden = False
                    Panel1.Hidden = True
                    LblConfirmation.Text = "Data Saved into Database"
                Else
                    NawaBLL.MenuTemplateBLL.SaveEditApproval(objMenutemplate, objmodule)
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

    Sub DeleteMenuTemplate(oMenutemplate As MenuTemplate, ByRef arrdel As ArrayList)

        For Each item As MenuTemplate In objMenutemplate.FindAll(Function(x) x.mMenuParentID = oMenutemplate.mMenuID)
            arrdel.Add(item)
            DeleteMenuTemplate(item, arrdel)
        Next
        arrdel.Add(oMenutemplate)
    End Sub
    Protected Sub btnDelete_DirectClick(sender As Object, e As DirectEventArgs)
        Try

            If Not TreePanelMenu.SelectedNodes Is Nothing Then


                Dim objdel As Ext.Net.SubmittedNode = TreePanelMenu.SelectedNodes(0)

                Dim objmenudel As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objdel.Attributes("mMenuID").ToString)

                Dim obnodedel As Ext.Net.NodeProxy = TreePanelMenu.GetNodeById(objdel.NodeID)






                obnodedel.RemoveAll()
                obnodedel.Remove()
                Dim arrdel As New ArrayList
                DeleteMenuTemplate(objmenudel, arrdel)

                For Each item As MenuTemplate In arrdel
                    objMenutemplate.Remove(item)
                Next


                'objMenutemplate.Remove(objmenudel)


                objMenuTemplateedit = Nothing

                IDData.Text = ""
                MenuLabel.Text = "Root"
                MenuURL.Text = ""
                cboAction.Text = ""
                cboModule.Text = ""
                MenuParent.Text = ""


                btnAddNew.Enable()
                btnDelete.Disable()
                btnSaveMenuAll.Enable()
                btnUp.Disable()
                btnDown.Disable()

            End If


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
    Protected Sub treepanel_itemClick(sender As Object, e As DirectEventArgs)
        Try
            'done: treepanel_itemClick
            objMenuTemplateedit = Nothing
            Dim objparam As String = e.ExtraParams("ID").ToString
            Dim objresult As MenuTemplate = Newtonsoft.Json.JsonConvert.DeserializeObject(Of MenuTemplate)(objparam)
            If Not objresult Is Nothing Then
                objMenuTemplateedit = objMenutemplate.Find(Function(x) x.mMenuID = objresult.mMenuID)
                btnAddNew.Enable()
                btnDelete.Enable()
                btnSaveMenuAll.Enable()
                btnUp.Enable()
                btnDown.Enable()
                IDData.Text = objMenuTemplateedit.mMenuID
                MenuLabel.Text = objresult.mMenuLabel
                Dim objparent As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objresult.mMenuParentID)
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
                btnAddNew.Enable()
                btnDelete.Disable()
                btnSaveMenuAll.Enable()
                btnUp.Disable()
                btnDown.Disable()
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class