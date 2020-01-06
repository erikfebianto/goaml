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
using NawaDAL;
using NawaBLL;
using System.Data.SqlClient;

namespace NawaDevBLL
{
	[Serializable()]public class UserLockBLL : IDisposable
	{
		public static NawaDAL.MUser GetMuserByPK(string id)
		{
			
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return (from p in objDb.MUsers where p.UserID == id select p).FirstOrDefault;
			}
			
		}
		
		public static bool EditDirect(string strUserid, bool InUser, bool PasswordLock)
		{
			
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						NawaDAL.MUser objuserNew = (from p in objdb.MUsers where p.UserID == strUserid select p).FirstOrDefault;
						objuserNew.InUsed = InUser;
						objuserNew.IsDisabled = PasswordLock;
						if (InUser == false)
						{
							objuserNew.IPAddress = "";
						}
						objuserNew.LastChangePassword = DateTime.Now;
						objuserNew.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objuserNew.ApprovedDate = DateTime.Now;
						
						NawaDAL.MUser objuserBefore = (from p in objdb.MUsers where p.UserID == strUserid select p).FirstOrDefault;
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = "User Lock";
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						
						Type objtype = objuserNew.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							if (!ReferenceEquals(item.GetValue(objuserBefore, null), null))
							{
								objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objuserBefore, null));
							}
							else
							{
								objaudittraildetail.OldValue = "";
							}
							if (!ReferenceEquals(item.GetValue(objuserNew, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objuserNew, null));
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
		public static bool EditApproval(string strUserid, bool InUsed, bool PasswordLock, NawaDAL.Module objModule)
		{
			
			
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						NawaDAL.MUser objuserNew = GetMuserByPK(strUserid);
						objuserNew.InUsed = InUsed;
						objuserNew.IsDisabled = PasswordLock;
						objuserNew.LastChangePassword = DateTime.Now;
						objuserNew.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objuserNew.ApprovedDate = DateTime.Now;
						
						NawaDAL.MUser objuserbefore = (from p in objdb.MUsers where p.UserID == strUserid select p).FirstOrDefault;
						
						
						string xmlNew = NawaBLL.Common.Serialize(objuserNew);
						string xmlbefore = NawaBLL.Common.Serialize(objuserbefore);
						
						
						NawaDAL.ModuleApproval objModuleApproval = new NawaDAL.ModuleApproval();
						objModuleApproval.ModuleName = objModule.ModuleName;
						objModuleApproval.ModuleKey = System.Convert.ToString(objuserNew.PK_MUser_ID);
						objModuleApproval.ModuleField = xmlNew;
						objModuleApproval.ModuleFieldBefore = xmlbefore;
						objModuleApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objModuleApproval.CreatedDate = DateTime.Now;
						objModuleApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objdb.Entry(objModuleApproval).State = System.Data.Entity.EntityState.Added;
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = "User Lock";
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						
						Type objtype = objuserNew.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							if (!ReferenceEquals(item.GetValue(objuserbefore, null), null))
							{
								objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objuserbefore, null));
							}
							else
							{
								objaudittraildetail.OldValue = "";
							}
							if (!ReferenceEquals(item.GetValue(objuserNew, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objuserNew, null));
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
		public static bool Reject(string ID)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						ModuleApproval objApproval = objdb.ModuleApprovals.Where(x => X.PK_ModuleApproval_ID == ID).FirstOrDefault();
						NawaDAL.Module objModule = default(NawaDAL.Module);
						if (!ReferenceEquals(objApproval, null))
						{
							objModule = objdb.Modules.Where(x => x.ModuleName == objApproval.ModuleName).FirstOrDefault;
						}
						
						NawaDAL.MUser objUserdata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(NawaDAL.MUser));
						NawaDAL.MUser objUserdataOld = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, typeof(NawaDAL.MUser));
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						Type objtype = objUserdata.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							if (!ReferenceEquals(item.GetValue(objUserdataOld, null), null))
							{
								objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objUserdataOld, null));
							}
							else
							{
								objaudittraildetail.OldValue = "";
							}
							if (!ReferenceEquals(item.GetValue(objUserdata, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objUserdata, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						
						objdb.Entry(objApproval).State = System.Data.Entity.EntityState.Deleted;
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
		public static bool Accept(string ID)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						ModuleApproval objApproval = objdb.ModuleApprovals.Where(x => X.PK_ModuleApproval_ID == ID).FirstOrDefault();
						NawaDAL.Module objModule;
						if (!ReferenceEquals(objApproval, null))
						{
							objModule = objdb.Modules.Where(x => x.ModuleName == objApproval.ModuleName).FirstOrDefault;
						}
						
						NawaDAL.MUser objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(NawaDAL.MUser));
						NawaDAL.MUser objModuledataOld = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, typeof(NawaDAL.MUser));
						objModuledata.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objModuledata.ApprovedDate = DateTime.Now;
						objdb.Entry(objModuledata).State = System.Data.Entity.EntityState.Modified;
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = "User Lock";
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						Type objtype = objModuledata.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objModuledataOld, null), null))
							{
								objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objModuledataOld, null));
							}
							else
							{
								objaudittraildetail.OldValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							if (!ReferenceEquals(item.GetValue(objModuledata, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
						}
						
						objdb.Entry(objApproval).State = System.Data.Entity.EntityState.Deleted;
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
		public void LoadPanel(FormPanel objPanel, string objdata, string strModulename, string unikkey)
		{
			NawaDAL.MUser objUserLockBLL = NawaBLL.Common.Deserialize(objdata, typeof(NawaDAL.MUser));
			if (!ReferenceEquals(objUserLockBLL, null))
			{
				string strunik = unikkey;
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_MUser_ID" + strunik, System.Convert.ToString(objUserLockBLL.PK_MUser_ID));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "User ID", "UserID" + strunik, objUserLockBLL.UserID);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "In User", "InUsed" + strunik, objUserLockBLL.InUsed);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Lock Password", "IsDisabled" + strunik, objUserLockBLL.IsDisabled);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" + strunik, objUserLockBLL.Active);
				
			}
		}
#region IDisposable Support
		private bool disposedValue; // To detect redundant calls
		
		// IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}
				
				// TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				// TODO: set large fields to null.
			}
			this.disposedValue = true;
		}
		
		// TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
		//Protected Overrides Sub Finalize()
		//    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
		//    Dispose(False)
		//    MyBase.Finalize()
		//End Sub
		
		// This code added by Visual Basic to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}
#endregion
		
	}
	
}
