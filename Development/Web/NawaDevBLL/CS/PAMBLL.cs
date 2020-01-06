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

using System.Data.SqlClient;
using System.Text;
using Ext.Net;


namespace NawaDevBLL
{
	public class PAMBLL : IDisposable
	{
		
		public enum EP_BudgetTypeBLL
		{
			Budgeted = 1,
			Adhoc,
			NoBudgetID,
			WithoutBudget
		}
		
		
		
		public static DataTable GetWorkflowconditionResult(string struserid, string strjson, int strmoduleid, int intcurrency = 1)
		{
			
			
			SqlParameter[] objListParam = new SqlParameter[4];
			objListParam[0] = new SqlParameter();
			
			objListParam[0].ParameterName = "@UserID";
			objListParam[0].Value = struserid;
			
			objListParam[1] = new SqlParameter();
			
			objListParam[1].ParameterName = "@jsondata";
			objListParam[1].Value = strjson;
			
			objListParam[2] = new SqlParameter();
			objListParam[2].ParameterName = "@moduleid";
			objListParam[2].Value = strmoduleid;
			
			objListParam[3] = new SqlParameter();
			objListParam[3].ParameterName = "@currencyfilter";
			objListParam[3].Value = intcurrency;
			
			
			return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "EP_usp_CekWorkflowCondition", objListParam);
		}
		
		public static bool IsExeFile(byte[] FileContent)
		{
			byte[] twoBytes = SubByteArray(FileContent, 0, 2);
			return (Encoding.UTF8.GetString(twoBytes) == "MZ") || (Encoding.UTF8.GetString(twoBytes) == "ZM");
		}
		private static byte[] SubByteArray(byte[] data, int index, int length)
		{
			byte[] result = new byte[length - 1+ 1];
			Array.Copy(data, index, result, 0, length);
			return result;
		}
		
		
		
		
		public static List<NawaDevDAL.EP_MS_Doc_Type> getDocumentTypeList()
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Doc_Type.Where(x => X.Active == true).ToList();
			}
			
			
		}
		
		public static NawaDevDAL.EP_MS_Doc_Type getDocumentTypeID(int id)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Doc_Type.Where(x => X.PK_EP_MS_Doc_Type_ID == id).FirstOrDefault;
			}
			
			
		}
		public static List<NawaDevDAL.EP_MS_Attachment_Document_Type> GetAttachmentTypeList()
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Attachment_Document_Type.Where(x => X.Active == true).ToList();
			}
			
			
		}
		
		public static NawaDevDAL.EP_MS_Attachment_Document_Type GetAttachmentTypeByID(int id)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Attachment_Document_Type.Where(x => X.PK_Attachment_Doc_Type_ID == id).FirstOrDefault;
			}
			
			
		}
		public static List<NawaDevDAL.EP_MS_Probability> getProbability()
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Probability.Where(x => X.Active == true).ToList();
			}
			
		}
		
		public static NawaDevDAL.EP_MS_Probability GetProbabilityByID(int ID)
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Probability.Where(x => X.PK_EP_MS_Probability_ID == ID).FirstOrDefault;
			}
			
		}
		public static NawaDevDAL.EP_MS_Project_Stage GetProjectStageByID(int ID)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Project_Stage.Where(x => X.PK_Project_Stage_ID == ID).FirstOrDefault;
			}
			
			
			
		}
		public static List<NawaDevDAL.EP_MS_Currency> GetcurrencyActive()
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Currency.ToList();
			}
			
			
		}
		
		
		public static DataTable GetPAMBudgetRemainingAfterUsedAgain(int pkbudgetid, double amountused)
		{
			
			
			SqlParameter[] objListParam = new SqlParameter[2];
			objListParam[0] = new SqlParameter();
			objListParam[1] = new SqlParameter();
			
			
			
			objListParam[0].ParameterName = "@pkbudgetid";
			objListParam[0].Value = pkbudgetid;
			
			objListParam[1].ParameterName = "@amountused";
			objListParam[1].Value = amountused;
			
			
			
			return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "EP_usp_GetBudgetRemaining", objListParam);
		}
		
		public static double getkursValue(int intcurrencyid, DateTime checkdate)
		{
			
			
			
			System.Data.SqlClient.SqlParameter[] objListParam = new System.Data.SqlClient.SqlParameter[2];
			objListParam[0] = new System.Data.SqlClient.SqlParameter();
			objListParam[0].ParameterName = "@currencyId";
			objListParam[0].Value = intcurrencyid;
			
			objListParam[1] = new System.Data.SqlClient.SqlParameter();
			objListParam[1].ParameterName = "@checkdate";
			objListParam[1].Value = checkdate;
			return System.Convert.ToDouble( NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "EP_usp_GetKursIDRValue", objListParam));
			
		}
		
		public static bool IsNotHaveKursValue(int intcurrencyid, DateTime checkdate)
		{
			
			return System.Convert.ToBoolean(NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.Text, "SELECT dbo.EP_ufn_IsNotHaveKursValue(" + System.Convert.ToString(intcurrencyid) + ",'" + checkdate.ToString("yyyy-MM-dd") + "')", null));
			
		}
		
		public static List<NawaDevDAL.EP_MS_PAM_Budget_Type> GetMsBudgetTyeActive()
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_PAM_Budget_Type.Where(x => X.Active == true).ToList();
			}
			
			
		}
		
		
		public static NawaDevDAL.EP_MS_Currency GetMsCurrencyByCurrencyCode(string currency_code)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Currency.Where(x => X.Currency_Code == currency_code).FirstOrDefault;
			}
			
			
		}
		
		public static NawaDevDAL.EP_MS_Currency GetMsCurrencyByID(int Pkid)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Currency.Where(x => X.PK_Currency_ID == Pkid).FirstOrDefault;
			}
			
			
		}
		
		public static DataTable GetBudgetData(int pkBudgetID)
		{
			
			System.Data.SqlClient.SqlParameter[] objListParam = new System.Data.SqlClient.SqlParameter[1];
			objListParam[0] = new System.Data.SqlClient.SqlParameter();
			objListParam[0].ParameterName = "@PK_Budget_ID";
			objListParam[0].Value = pkBudgetID;
			return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "EP_usp_GetBudgetByBudgetID", objListParam);
			
		}
		
		public static string GenerateProjectID(int ProjectType)
		{
			
			System.Data.SqlClient.SqlParameter[] objListParam = new System.Data.SqlClient.SqlParameter[1];
			objListParam[0] = new System.Data.SqlClient.SqlParameter();
			
			objListParam[0].ParameterName = "@IntProjectType";
			objListParam[0].Value = ProjectType;
			
			return System.Convert.ToString( NawaDAL.SQLHelper.ExecuteScalar(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GenerateProjectID", objListParam));
		}
		
		
		public static List<NawaDevDAL.EP_MS_Period_Type> getClosingPeriod()
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Period_Type.ToList();
			}
			
			
			
		}
		
		public static List<NawaDevDAL.EP_TR_PAM_Milestones> GetInitialMilestone()
		{
			
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				
				List<NawaDevDAL.EP_MS_Project_Stage> objListState = objDb.EP_MS_Project_Stage.Where(x => X.Active == true).ToList();
				
				List<NawaDevDAL.EP_TR_PAM_Milestones> objListMilestone = new List<NawaDevDAL.EP_TR_PAM_Milestones>();
				
				
				foreach (NawaDevDAL.EP_MS_Project_Stage item in objListState)
				{
					NawaDevDAL.EP_TR_PAM_Milestones objNewMilestone = new NawaDevDAL.EP_TR_PAM_Milestones();
					
					
					Random objrand = new Random();
					long intpk = objrand.Next(int.MinValue, -1);
					while (!(objListMilestone.Find(x => x.PK_PAM_Milestones_ID == intpk), null)))
					{
						intpk = objrand.Next(int.MinValue, -1);
					}
					objNewMilestone.FK_PAM_ID = intpk;
					objNewMilestone.FK_Project_Stage_ID = item.PK_Project_Stage_ID;
					objNewMilestone.Start_Date = null;
					objNewMilestone.End_Date = null;
					objListMilestone.Add(objNewMilestone);
					
				}
				return objListMilestone;
			}
			
			
			
			
		}
		public static bool GenerateGridQuantitative(Ext.Net.GridPanel objgrid)
		{
			
			
			System.Data.DataTable objdt = GetQuantitativeInitial();
			Ext.Net.Model objModel = new Ext.Net.Model();
			objModel.ID = "Modelview";
			
			Ext.Net.ModelField objModelField = default(Ext.Net.ModelField);
			
			
			foreach (DataColumn item in objdt.Columns)
			{
				
				switch (item.DataType.ToString())
				{
					case "System.Boolean":
						objModelField = new Ext.Net.ModelField();
						objModelField.Name = item.ColumnName;
						objModelField.Type = ModelFieldType.Boolean;
						objModel.Fields.Add(objModelField);
						break;
						
					case "System.DateTime":
						
						objModelField = new Ext.Net.ModelField();
						objModelField.Name = item.ColumnName;
						objModelField.Type = ModelFieldType.Date;
						objModel.Fields.Add(objModelField);
						break;
						
					case "System.Decimal":
						objModelField = new Ext.Net.ModelField();
						objModelField.Name = item.ColumnName;
						objModelField.Type = ModelFieldType.Float;
						objModel.Fields.Add(objModelField);
						break;
						
					default:
						objModelField = new Ext.Net.ModelField();
						objModelField.Name = item.ColumnName;
						objModelField.Type = ModelFieldType.Auto;
						
						objModel.Fields.Add(objModelField);
						break;
				}
				
			}
			
			foreach (System.Data.DataColumn item in objdt.Columns)
			{
				if (item.DataType.ToString() == "System.Boolean")
				{
					Ext.Net.ComponentColumn objcolumn = new Ext.Net.ComponentColumn();
					objcolumn.DataIndex = item.ColumnName;
					objcolumn.Text = item.ColumnName;
					objcolumn.Flex = 1;
					objcolumn.MinWidth = 150;
					
					objcolumn.Editor = true;
					Ext.Net.Checkbox objcheckbox = new Ext.Net.Checkbox();
					objcolumn.Component.Add(objcheckbox);
					objgrid.ColumnModel.Columns.Add(objcolumn);
				}
				else if (item.DataType.ToString() == "System.DateTime")
				{
					Ext.Net.ComponentColumn objcolumn = new Ext.Net.ComponentColumn();
					objcolumn.DataIndex = item.ColumnName;
					objcolumn.Text = item.ColumnName;
					objcolumn.Editor = true;
					objcolumn.MinWidth = 150;
					objcolumn.Flex = 1;
					Ext.Net.DateField objDate = new Ext.Net.DateField();
					objcolumn.Component.Add(objDate);
					objgrid.ColumnModel.Columns.Add(objcolumn);
					
				}
				else if (item.DataType.ToString() == "System.Decimal" || item.DataType.ToString() == "System.Integer" || item.DataType.ToString() == "System.Double")
				{
					Ext.Net.ComponentColumn objcolumn = new Ext.Net.ComponentColumn();
					objcolumn.DataIndex = item.ColumnName;
					objcolumn.Text = item.ColumnName;
					objcolumn.Editor = true;
					objcolumn.MinWidth = 150;
					objcolumn.Flex = 1;
					Ext.Net.NumberField objNumber = new Ext.Net.NumberField();
					objcolumn.Component.Add(objNumber);
					objgrid.ColumnModel.Columns.Add(objcolumn);
				}
				else
				{
					Ext.Net.Column objcolumn = new Ext.Net.Column();
					objcolumn.DataIndex = item.ColumnName;
					objcolumn.Text = item.ColumnName;
					objcolumn.MinWidth = 150;
					objcolumn.Flex = 1;
					objgrid.ColumnModel.Columns.Add(objcolumn);
				}
				
			}
			
			
			objgrid.Store[0].Model.Add(objModel);
			objgrid.Store[0].DataSource = objdt;
			objgrid.Store[0].DataBind();
			
		}
		public static System.Data.DataTable GetQuantitativeInitial()
		{
			
			return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetTargetQuantitativeInitial", null);
			
		}
		public static List<NawaDevDAL.EP_MS_Project_Category> GetProjectCategoryByProjectype(int intprojectype)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_Project_Category.Where(x => X.FK_Project_Type_ID == intprojectype).ToList();
			}
			
			
		}
		
		public static List<NawaDevDAL.EP_MS_ProjectType> GetProjectType()
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EP_MS_ProjectType.Where(x => X.Active == true).ToList();
			}
			
			
		}
		
#region IDisposable Support
		
		private bool disposedValue; // To detect redundant calls
		
		// IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}
				
				// TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				// TODO: set large fields to null.
			}
			disposedValue = true;
		}
		
		// TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
		//Protected Overrides Sub Finalize()
		//    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
		//    Dispose(False)
		//    MyBase.Finalize()
		//End Sub
		
		// This code added by Visual Basic to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
			Dispose(true);
			// TODO: uncomment the following line if Finalize() is overridden above.
			// GC.SuppressFinalize(Me)
		}
		
#endregion
		
	}
}
