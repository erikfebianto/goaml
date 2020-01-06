// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Linq;
// End of VB project level imports

using Ext.Net;

namespace NawaDevBLL
{
	public class NawaFramework
	{
		
		
		
		public static dynamic GetPicker(Ext.Net.DropDownField cb, string strtableName, string strFieldName, string strfilter, string strsort, string jsscript, bool brender, bool brefresh, int intwidth, int intheight, int intMinwidth = 150)
		{
			Ext.Net.Window objwindow = default(Ext.Net.Window);
			Ext.Net.GridPanel objGrid = default(Ext.Net.GridPanel);
			Ext.Net.Store objStore = default(Ext.Net.Store);
			Ext.Net.Model objModel = default(Ext.Net.Model);
			Ext.Net.ModelField objModelfield = default(Ext.Net.ModelField);
			
			if (brender)
			{
				
				objwindow = cb.Component[0];
				objGrid = objwindow.Items[0];
				objStore = objGrid.GetStore();
				objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize();
				objStore.RemoteFilter = true;
				objStore.RemoteSort = true;
				objModel = new Ext.Net.Model();
				objModelfield = new Ext.Net.ModelField();
				
				objStore.Model.Add(objModel);
				
				
			}
			else
			{
				objwindow = cb.Component[0];
				objGrid = objwindow.Items[0];
				objStore = objGrid.GetStore();
				
				if (brefresh)
				{
					objStore.Model.Clear();
					objModel = new Ext.Net.Model();
					objStore.Model.Add(objModel);
				}
				else
				{
					objModel = objStore.Model[0];
				}
				
			}
			
			
			
			DataTable datatable = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "select top 0 " + strFieldName + " from " + strtableName + " where 1=2", null);
			
			foreach (DataColumn item in datatable.Columns)
			{
				
				
				
				switch (item.DataType.ToString())
				{
					case "System.Boolean":
						
						
						objModelfield = new Ext.Net.ModelField();
						objModelfield.Name = item.ColumnName;
						objModelfield.Type = ModelFieldType.Boolean;
						objModel.Fields.Add(objModelfield);
						break;
						
					case "System.DateTime":
						
						objModelfield = new Ext.Net.ModelField();
						objModelfield.Name = item.ColumnName;
						objModelfield.Type = ModelFieldType.Date;
						objModel.Fields.Add(objModelfield);
						break;
						
					case "System.Decimal":
						objModelfield = new Ext.Net.ModelField();
						objModelfield.Name = item.ColumnName;
						objModelfield.Type = ModelFieldType.Float;
						objModel.Fields.Add(objModelfield);
						break;
						
					default:
						objModelfield = new Ext.Net.ModelField();
						objModelfield.Name = item.ColumnName;
						objModelfield.Type = ModelFieldType.Auto;
						
						objModel.Fields.Add(objModelfield);
						break;
				}
				
			}
			
			if (brender)
			{
				
				objGrid.SelectionModel.Clear();
				objGrid.ColumnModel.Columns.Clear();
				objGrid.Plugins.Clear();
				
				RowSelectionModel objRowSelection = new RowSelectionModel();
				objRowSelection.Mode = SelectionMode.Single;
				objRowSelection.Listeners.Select.Handler = jsscript;
				objRowSelection.AllowDeselect = true;
				objGrid.SelectionModel.Add(objRowSelection);
				
				FilterHeader objfilterheader = new FilterHeader();
				objfilterheader.Remote = true;
				objGrid.Plugins.Add(objfilterheader);
				
				
				foreach (System.Data.DataColumn item in datatable.Columns)
				{
					if (item.DataType.ToString() == "System.Boolean")
					{
						Ext.Net.BooleanColumn objcolumn = new Ext.Net.BooleanColumn();
						objcolumn.DataIndex = item.ColumnName;
						objcolumn.Text = item.ColumnName;
						objcolumn.Flex = 1;
						objcolumn.MinWidth = 150;
						objGrid.ColumnModel.Columns.Add(objcolumn);
					}
					else if (item.DataType.ToString() == "System.DateTime")
					{
						Ext.Net.DateColumn objcolumn = new Ext.Net.DateColumn();
						objcolumn.DataIndex = item.ColumnName;
						objcolumn.Text = item.ColumnName;
						objcolumn.Format = NawaBLL.SystemParameterBLL.GetDateFormat();
						objcolumn.MinWidth = 150;
						objcolumn.Flex = 1;
						objGrid.ColumnModel.Columns.Add(objcolumn);
						
					}
					else if (item.DataType.ToString() == "System.Decimal" || item.DataType.ToString() == "System.Integer")
					{
						Ext.Net.NumberColumn objcolumn = new Ext.Net.NumberColumn();
						objcolumn.DataIndex = item.ColumnName;
						objcolumn.Text = item.ColumnName;
						
						objcolumn.MinWidth = 150;
						objcolumn.Flex = 1;
						objGrid.ColumnModel.Columns.Add(objcolumn);
					}
					else
					{
						Ext.Net.Column objcolumn = new Ext.Net.Column();
						objcolumn.DataIndex = item.ColumnName;
						objcolumn.Text = item.ColumnName;
						objcolumn.MinWidth = 150;
						objcolumn.Flex = 1;
						objGrid.ColumnModel.Columns.Add(objcolumn);
					}
					
				}
				
				
				objwindow.Width = intwidth;
				cb.MinWidth = 300;
				objwindow.Height = intheight;
				
				
				//TryCast(Store.Proxy(0), PageProxy).Total = CInt(conn.GetDataTable("SELECT COUNT(*) " & sQuery.Substring(sQuery.LastIndexOf(" FROM ")) & cond, oParam).Rows(0)(0))
				
				int inttotal = 0;
				objStore.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging(strtableName, strFieldName, strfilter, strsort, 0, NawaBLL.SystemParameterBLL.GetPageSize(), ref inttotal);
				objStore.DataBind();
				
			}
			else
			{
				//'cb.Render()
				
			}
			
			
			
		}
		
		public static List<NawaDAL.MGroupMenuAccess> ObjGroupMenuAccess(int roleid, int moduleid)
		{
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return objDb.MGroupMenuAccesses.Where(x => X.FK_GroupMenu_ID == roleid && X.FK_Module_ID == moduleid).ToList();
			}
			
			
		}
		public static Ext.Net.Checkbox ExtCheckBox(FieldSet pn, string strLabel, string strFieldName, bool bRequired)
		{
			Ext.Net.Checkbox objCheck = new Ext.Net.Checkbox();
			objCheck.ID = strFieldName;
			objCheck.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			if (!Ext.Net.X.IsAjaxRequest)
			{
				objCheck.FieldLabel = strLabel;
			}
			pn.Add(objCheck);
			return objCheck;
			
		}
		public static Ext.Net.NumberField ExtNumber(FieldSet pn, string strLabel, string strFieldName, bool bRequired, int intDecimalPrecition, double dminvalue, double dmaxvalue)
		{
			Ext.Net.NumberField objNumberField = new Ext.Net.NumberField();
			objNumberField.ID = strFieldName;
			objNumberField.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			if (!Ext.Net.X.IsAjaxRequest)
			{
				objNumberField.FieldLabel = strLabel;
				objNumberField.LabelStyle = "word-wrap: break-word";
				objNumberField.LabelWidth = 100;
				objNumberField.Name = strFieldName;
				objNumberField.AllowBlank = !bRequired;
				objNumberField.BlankText = strLabel + " is required.";
				objNumberField.DecimalPrecision = intDecimalPrecition;
				objNumberField.MinValue = dminvalue;
				objNumberField.MaxValue = dmaxvalue;
				objNumberField.Width = objNumberField.LabelWidth + 150;
				objNumberField.AnchorHorizontal = "40%";
			}
			pn.Add(objNumberField);
			return objNumberField;
		}
		
		
		
		public static Ext.Net.ComboBox ExtCombo(FormPanel pn, string strLabel, string strFieldName, bool bRequired, int intgridpos, string strTableRef, string strFieldKey, string strFieldDisplay, string strFilterField, string strTableRefAlias)
		{
			using (Ext.Net.ComboBox objcombo = new Ext.Net.ComboBox())
			{
				objcombo.ID = strFieldName;
				objcombo.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				if (!Ext.Net.X.IsAjaxRequest)
				{
					objcombo.FieldLabel = strLabel;
					
					objcombo.LabelWidth = 100;
					objcombo.AnchorHorizontal = "80%";
					objcombo.Name = strFieldName;
					objcombo.AllowBlank = !bRequired;
					objcombo.BlankText = strLabel + " is required.";
					objcombo.Width = objcombo.LabelWidth + 150;
					objcombo.MatchFieldWidth = true;
					objcombo.MinChars = System.Convert.ToInt32("0");
					objcombo.ForceSelection = true;
					objcombo.AnyMatch = true;
					
					objcombo.QueryMode = DataLoadMode.Remote;
					objcombo.ValueField = strFieldKey;
					objcombo.DisplayField = strFieldDisplay;
					objcombo.TriggerAction = Ext.Net.TriggerAction.Query;
					
					Ext.Net.FieldTrigger objFieldtrigger = new Ext.Net.FieldTrigger();
					objFieldtrigger.Icon = Ext.Net.TriggerIcon.Clear;
					objFieldtrigger.Hidden = true;
					objFieldtrigger.Weight = System.Convert.ToInt32("-1");
					objcombo.Triggers.Add(objFieldtrigger);
					
					
					objcombo.Listeners.Select.Handler = "this.getTrigger(0).show();";
					
					objcombo.Listeners.TriggerClick.Handler = "if (index == 0) {  this.clearValue(); this.getTrigger(0).hide();}";
					
					
					
					//buat store dan modelnya
					
					using (Ext.Net.Store objStore = new Ext.Net.Store())
					{
						objStore.ID = "_Store_" + objcombo.ID;
						objStore.ClientIDMode = System.Web.UI.ClientIDMode.Static;
						
						
						using (Ext.Net.Model objModel = new Ext.Net.Model())
						{
							objModel.Fields.Add(strFieldKey, Ext.Net.ModelFieldType.String);
							objModel.Fields.Add(strFieldDisplay, Ext.Net.ModelFieldType.String);
							objStore.Model.Add(objModel);
						}
						
						
						objcombo.PageSize = NawaBLL.SystemParameterBLL.GetPageSize();
						objStore.PageSize = NawaBLL.SystemParameterBLL.GetPageSize();
						objStore.IsPagingStore = true;
						
						objStore.Proxy.Add(new PageProxy());
						objStore.ReadData += Callback;
						
						objcombo.Store.Add(objStore);
						
						//objStore.DataSource = NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, System.Data.CommandType.Text, GetQueryRef(strTableRef & " " & strTableRefAlias, strFieldKey, strFieldDisplay, strFilterField), Nothing)
						// objStore.DataBind()
					}
					
				}
				
				pn.Add(objcombo);
				return objcombo;
			}
			
		}
		public static void Callback(object sender, StoreReadDataEventArgs e)
		{
			string query = e.Parameters["query"];
			if (ReferenceEquals(query, null))
			{
				query = "";
			}
			string strfilter = "";
			if (query.Length > 0)
			{
				strfilter = " ModuleActionName like '" + query + "%'";
			}
			
			//StoreAction.DataSource = NawaDAL.SQLHelper.ExecuteTabelPaging("ModuleAction", "PK_ModuleAction_ID,ModuleActionName", strfilter, "PK_ModuleAction_ID", e.Start, e.Limit, e.Total)
			//StoreAction.DataBind()
		}
		public static string GetQueryRef(string strTable, string strfieldkey, string strfielddisplay, string strfilter)
		{
			string strquery = "";
			strquery = "select " + strfieldkey + ", convert(Varchar(1000),[" + strfieldkey + "])+ ' - '+ convert(varchar(1000), [" + strfielddisplay + "]) as [" + strfielddisplay + "] from " + strTable;
			if (strfilter.Trim().Length > 0)
			{
				strquery = strquery + " where " + strfilter;
			}
			return strquery;
		}
		
		
		public static string HTMLEncode(string ostr)
		{
			
			return System.Web.HttpUtility.HtmlEncode(ostr);
			
		}
		
		public static string HTMLDecode(string ostr)
		{
			
			return System.Web.HttpUtility.HtmlDecode(ostr);
			
		}
		
		
		public static Ext.Net.MultiUpload ExtFileUpload(FormPanel pn, string strLabel, string strFieldName, bool bRequired, int sizelimitMB)
		{
			Ext.Net.MultiUpload objDateField = new Ext.Net.MultiUpload();
			objDateField.ID = strFieldName;
			objDateField.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			if (!Ext.Net.X.IsAjaxRequest)
			{
				objDateField.FileDropAnywhere = true;
				objDateField.AutoStartUpload = true;
				objDateField.FileSizeLimit = System.Convert.ToString(sizelimitMB);
				objDateField.FileTypes = "*.*";
				objDateField.FileUploadLimit = 100;
				objDateField.FileQueueLimit = 0;
				objDateField.Listeners.UploadStart.Handler = "Ext.Msg.wait('Uploading...');";
				objDateField.Listeners.UploadError.Fn = "uploadError";
				objDateField.Listeners.FileSelectionError.Fn = "fileSelectionError";
				
				
			}
			pn.Add(objDateField);
			return objDateField;
		}
		
	}
	
}
