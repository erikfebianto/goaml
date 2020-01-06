Imports Ext.Net
Imports NawaBLL
Imports NawaDAL
Public Class MenuTemplateView
    Inherits Parent


    Public Property objmodule() As NawaDAL.Module
        Get
            Return Session("MenuTemplateView.objmodule")
        End Get
        Set(ByVal value As NawaDAL.Module)
            Session("MenuTemplateView.objmodule") = value
        End Set
    End Property


    Public Property objMenutemplate() As List(Of NawaDAL.MenuTemplate)
        Get
            Return Session("MenuTemplateView.objMenutemplate")
        End Get
        Set(ByVal value As List(Of NawaDAL.MenuTemplate))
            Session("MenuTemplateView.objMenutemplate") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Moduleid As String = Request.Params("ModuleID")
            Dim intModuleid As Integer
            Try
                intModuleid = NawaBLL.Common.DecryptQueryString(Moduleid, NawaBLL.SystemParameterBLL.GetEncriptionKey)
                objmodule = NawaBLL.ModuleBLL.GetModuleByModuleID(intModuleid)


                If Not NawaBLL.ModuleBLL.GetHakAkses(NawaBLL.Common.SessionCurrentUser.FK_MGroupMenu_ID, objmodule.PK_Module_ID, NawaBLL.Common.ModuleActionEnum.view) Then
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


                objMenutemplate = MenuTemplateBLL.GetMenuTemplate
            End If
            Using objtem As New NawaBLL.MenuTemplateBLL
                objtem.GetMenuTemplate(TreePanelMenu, objmodule)
            End Using


        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub


    Protected Sub treepanel_itemClick(sender As Object, e As DirectEventArgs)
        Try
            Dim objparam As String = e.ExtraParams("ID").ToString
            Dim objresult As MenuTemplate = objMenutemplate.Find(Function(x) x.mMenuID = objparam)

            If Not objresult Is Nothing Then
                IDData.Text = objresult.mMenuID
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
                    MenuModule.Text = objmodule.ModuleLabel
                Else
                    MenuModule.Text = ""
                End If
                Dim objaction As NawaDAL.ModuleAction = NawaBLL.ModuleBLL.GetModuleActionByID(objresult.FK_Action_ID.GetValueOrDefault(0))
                If Not objaction Is Nothing Then
                    ModuleAction.Text = objaction.ModuleActionName
                Else
                    ModuleAction.Text = ""
                End If
            Else
                IDData.Text = ""
                MenuLabel.Text = "Root"
                MenuURL.Text = ""
                ModuleAction.Text = ""
                MenuModule.Text = ""
                MenuParent.Text = ""
            End If

            
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            Ext.Net.X.Msg.Alert("Error", ex.Message).Show()
        End Try
    End Sub
End Class