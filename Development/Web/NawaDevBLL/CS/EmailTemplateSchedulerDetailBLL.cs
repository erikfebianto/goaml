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
	
	public class EmailTemplateSchedulerDetailBLL : IDisposable
	{
		public FormPanel oPanelinput;
		
		public EmailTemplateSchedulerDetailBLL(Ext.Net.FormPanel oPanel)
		{
			oPanelinput = oPanel;
		}
		
		public static dynamic DeleteTanpaapproval(string unik, NawaDAL.Module objSchemaModule)
		{
			
			using (NawaDevDAL.NawaDatadevEntities objdb = new NawaDevDAL.NawaDatadevEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						NawaDevDAL.EmailTemplateSchedulerDetail objdel = objdb.EmailTemplateSchedulerDetails.Where(x => X.EmailID == unik).FirstOrDefault();
						
						NawaDevDAL.AuditTrailHeader objaudittrailheader = new NawaDevDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						Type objtype = objdel.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDevDAL.AuditTrailDetail objaudittraildetail = new NawaDevDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							
							if (!ReferenceEquals(item.GetValue(objdel, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objdel, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						objdb.Entry(objdel).State = System.Data.Entity.EntityState.Deleted;
						
						objdb.SaveChanges();
						objtrans.Commit();
					}
					catch (Exception)
					{
						objtrans.Rollback();
						throw;
					}
				}
				
			}
			
		}
		
		public static string GetEmailStatus(int emailstatus)
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				NawaDevDAL.EmailStatu objresult = objDb.EmailStatus.Where(x => X.PK_EmailStatus_ID == emailstatus).FirstOrDefault;
				if (!ReferenceEquals(objresult, null))
				{
					return objresult.EmailStatusName;
				}
				else
				{
					return "";
				}
			}
			
			
		}
		
		public static string GetEmailTemplateNameBySchedulerID(string emailschedulerid)
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				NawaDevDAL.EmailTemplateScheduler objresult = objDb.EmailTemplateSchedulers.Where(x => X.PK_EmailTemplateScheduler_ID == emailschedulerid).FirstOrDefault;
				
				if (!ReferenceEquals(objresult, null))
				{
					NawaDevDAL.EmailTemplate objemtemp = objDb.EmailTemplates.Where(y => y.PK_EmailTemplate_ID == objresult.PK_EmailTemplate_ID).FirstOrDefault;
					if (!ReferenceEquals(objemtemp, null))
					{
						return objemtemp.EmailTemplateName;
					}
					else
					{
						return "";
					}
				}
				else
				{
					return "";
				}
			}
			
		}
		
		public static NawaDevDAL.EmailTemplateSchedulerDetail GetEmailTemplateSchedulerbyEmailID(string EmailID)
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				return objDb.EmailTemplateSchedulerDetails.Where(x => X.EmailID == EmailID).FirstOrDefault;
				
			}
			
			
		}
		
		public static dynamic IsDataValidDelete(string unik, NawaDAL.Module objmodule)
		{
			using (NawaDevDAL.NawaDatadevEntities objDb = new NawaDevDAL.NawaDatadevEntities())
			{
				if (objDb.EmailTemplateSchedulerDetails.Where(x => X.EmailID == unik).Count() > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				
			}
			
			
		}
		
		public static void LoadPanel(FormPanel oPanelinput, string strModulename, string unikkey)
		{
			
			NawaDevDAL.EmailTemplateSchedulerDetail objShedulerEmailTemplateDetail = GetEmailTemplateSchedulerbyEmailID(unikkey);
			
			if (!ReferenceEquals(objShedulerEmailTemplateDetail, null))
			{
				
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "EmailID", "EmailID", objShedulerEmailTemplateDetail.EmailID);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Template", "EmailTemplate", GetEmailTemplateNameBySchedulerID(objShedulerEmailTemplateDetail.FK_EmailTEmplateScheduler_ID));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email To", "EmailTo", objShedulerEmailTemplateDetail.EmailTo);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email CC", "EmailCC", objShedulerEmailTemplateDetail.EmailCC);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email BCC", "EmailBCC", objShedulerEmailTemplateDetail.EmailBCC);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Subject", "EmailSubject", objShedulerEmailTemplateDetail.EmailSubject);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Body", "EmailBody", objShedulerEmailTemplateDetail.EmailBody);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Process Date", "Processdate", objShedulerEmailTemplateDetail.ProcessDate.GetValueOrDefault().ToString(NawaBLL.SystemParameterBLL.GetDateFormat()));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Send Email Date", "SendEmailDate", objShedulerEmailTemplateDetail.SendEmailDate.GetValueOrDefault().ToString(NawaBLL.SystemParameterBLL.GetDateFormat()));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Status", "EmailStatus", GetEmailStatus(objShedulerEmailTemplateDetail.FK_EmailStatus_ID));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Error Message", "ErrorMessage", objShedulerEmailTemplateDetail.ErrorMessage);
				
			}
		}
		public static bool SaveEditTanpaApproval(NawaDevDAL.EmailTemplateSchedulerDetail ObjEmailTemplateSchedulerDetail, NawaDAL.Module objSchemaModule)
		{
			//todohendra: save ke table EmailTemplateSchedulerDetail
			
			using (NawaDevDAL.NawaDatadevEntities objdb = new NawaDevDAL.NawaDatadevEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						objdb.Entry(ObjEmailTemplateSchedulerDetail).State = System.Data.Entity.EntityState.Modified;
						
						NawaDevDAL.AuditTrailHeader objaudittrailheader = new NawaDevDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						Type objtype = ObjEmailTemplateSchedulerDetail.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDevDAL.AuditTrailDetail objaudittraildetail = new NawaDevDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							
							if (!ReferenceEquals(item.GetValue(ObjEmailTemplateSchedulerDetail, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(ObjEmailTemplateSchedulerDetail, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						
						objdb.SaveChanges();
						objtrans.Commit();
					}
					catch (Exception)
					{
						objtrans.Rollback();
						throw;
					}
				}
				
			}
			
		}
		
		public void BentukformEdit()
		{
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "EmailID", "EmailID", "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Template", "EmailTemplate", "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email To", "EmailTo", true, 1000, "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email CC", "EmailCC", false, 1000, "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email BCC", "EmailBCC", false, 1000, "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtText(oPanelinput, "Email Subject", "EmailSubject", true, 1000, "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtHTMLText(oPanelinput, "Email Body", "EmailBody");
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Process Date", "Processdate", "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Send Email Date", "SendEmailDate", "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Email Status", "EmailStatus", "");
			NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(oPanelinput, "Error Message", "ErrorMessage", "");
			
		}
		
#region IDisposable Support
		
		private bool disposedValue; // To detect redundant calls
		
		// This code added by Visual Basic to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
			Dispose(true);
			// TODO: uncomment the following line if Finalize() is overridden above.
			// GC.SuppressFinalize(Me)
		}
		
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
#endregion
		
	}
}
