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

using NawaBLL;

namespace NawaDevBLL
{
	[Serializable()]public class ChangePasswordBLL : IDisposable
	{
		
		
		public static bool ChangePasword(string strUserid, string strPassword)
		{
			
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						NawaDAL.MUser objuser = (from p in objdb.MUsers where p.UserID == strUserid select p).FirstOrDefault;
						
						objuser.UserPasword = Common.Encrypt(strPassword, objuser.UserPasswordSalt);
						objuser.LastChangePassword = DateTime.Now;
						objuser.LastUpdateBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objuser.ApprovedDate = DateTime.Now;
						
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
						objaudittrailheader.ModuleLabel = "Change Password";
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						
						Type objtype = objuser.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							
							if (!ReferenceEquals(item.GetValue(objuser, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objuser, null));
								
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
