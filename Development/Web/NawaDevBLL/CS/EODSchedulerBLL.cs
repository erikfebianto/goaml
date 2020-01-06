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
	public class EODSchedulerBLL : IDisposable
	{
		public static NawaDAL.EODScheduler GetEODSchedulerByPK(long id)
		{
			//Using objDb As NawaDevDAL.NawaDataDevEntities = New NawaDevDAL.NawaDataDevEntities
			//    objDb.MUsers.Where(Function(x) x.PK_MUser_ID = id).ToList()
			
			//End Using
			
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return (from x in objDb.EODSchedulers where x.PK_EODScheduler_ID == id select x).FirstOrDefault;
			}
			
		}
		
		public static bool SaveEOD(DateTime datadate, long intprocessid, Module objSchemaModule)
		{
			
			
			
			
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						System.Data.SqlClient.SqlParameter oparamdatadate = default(System.Data.SqlClient.SqlParameter);
						oparamdatadate = new SqlParameter();
						oparamdatadate.ParameterName = "@Datadate";
						oparamdatadate.DbType = DbType.DateTime;
						oparamdatadate.Value = datadate;
						
						System.Data.SqlClient.SqlParameter oparamprocessid = default(System.Data.SqlClient.SqlParameter);
						oparamprocessid = new SqlParameter();
						oparamprocessid.ParameterName = "@processid";
						oparamprocessid.DbType = DbType.Int64;
						oparamprocessid.Value = intprocessid;
						
						System.Data.SqlClient.SqlParameter oparamuserid = default(System.Data.SqlClient.SqlParameter);
						oparamuserid = new SqlParameter();
						oparamuserid.ParameterName = "@userID";
						oparamuserid.DbType = DbType.String;
						oparamuserid.Value = NawaBLL.Common.SessionCurrentUser.UserID;
						
						
						objdb.Database.ExecuteSqlCommand("usp_insertEODSchedulerManual @Datadate,@processid,@userID", oparamdatadate, oparamprocessid, oparamuserid);
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						NawaDAL.AuditTrailDetail objaudittraildetail = default(NawaDAL.AuditTrailDetail);
						objaudittraildetail = new NawaDAL.AuditTrailDetail();
						objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
						objaudittraildetail.FieldName = "DataDate";
						objaudittraildetail.OldValue = "";
						objaudittraildetail.NewValue = datadate.ToString(NawaBLL.SystemParameterBLL.GetDateFormat());
						objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						
						
						objaudittraildetail = new NawaDAL.AuditTrailDetail();
						objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
						objaudittraildetail.FieldName = "ProcessID";
						objaudittraildetail.OldValue = "";
						objaudittraildetail.NewValue = intprocessid;
						objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						
						
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
		
		
		
		public void SettingColor(FormPanel objPanelOld, FormPanel objPanelNew, string objdata, string objdatabefore, string strmodulename, string unikkeyold, string unikkeynew)
		{
			if (objdata.Length > 0 & objdatabefore.Length > 0)
			{
				//Dim objModuleData As NawaDAL.Module = ModuleBLL.Deserialize(objdata, GetType(NawaDAL.Module))
				//Dim objModuleDataBefore As NawaDAL.Module = ModuleBLL.Deserialize(objdata, GetType(NawaDAL.Module))
				DisplayField objdisplayOld = default(DisplayField);
				DisplayField objdisplayNew = default(DisplayField);
				objdisplayOld = objPanelOld.FindControl("PK_EODScheduler_ID" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("PK_EODScheduler_ID" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("EODSchedulerName" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("EODSchedulerName" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("EODSchedulerDescription" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("EODSchedulerDescription" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("EODPeriod" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("EODPeriod" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("EODPeriodType" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("EODPeriodType" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("StartDate" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("StartDate" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
				}
				objdisplayOld = objPanelOld.FindControl("Active" + unikkeyold);
				objdisplayNew = objPanelNew.FindControl("Active" + unikkeynew);
				if (objdisplayOld.Text != objdisplayNew.Text)
				{
					objdisplayOld.FieldStyle = "Color:red";
					objdisplayNew.FieldStyle = "Color:red";
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
						NawaDAL.Module objModule = default(NawaDAL.Module);
						if (!ReferenceEquals(objApproval, null))
						{
							objModule = objdb.Modules.Where(x => x.ModuleName == objApproval.ModuleName).FirstOrDefault();
						}
						if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Insert)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							objModuledata.objScheduler.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objModuledata.objScheduler.ApprovedDate = DateTime.Now;
							objdb.Entry(objModuledata.objScheduler).State = System.Data.Entity.EntityState.Added;
							//audittrail
							
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
							objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
							objdb.SaveChanges();
							
							foreach (NawaDAL.EODSchedulerDetail item in objModuledata.objSchedulerDetail)
							{
								item.FK_EODSCheduler_ID = objModuledata.objScheduler.PK_EODScheduler_ID;
								objdb.Entry(item).State = System.Data.Entity.EntityState.Added;
							}
							Type objtype = objModuledata.GetType();
							System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(objModuledata, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Update)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							EODSchedulerDataBLL objModuledataOld = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, typeof(EODSchedulerDataBLL));
							objModuledata.objScheduler.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objModuledata.objScheduler.ApprovedDate = DateTime.Now;
							objdb.EODSchedulers.Attach(objModuledata.objScheduler);
							objdb.Entry(objModuledata.objScheduler).State = System.Data.Entity.EntityState.Modified;
							foreach (EODSchedulerDetail itemx in (from x in objdb.EODSchedulerDetails where x.FK_EODSCheduler_ID == objModuledata.objScheduler.PK_EODScheduler_ID select x).ToList())
							{
								EODSchedulerDetail objcek = objModuledata.objSchedulerDetail.Find(x => X.PK_EODSchedulerDetail_ID == itemx.PK_EODSchedulerDetail_ID);
								if (ReferenceEquals(objcek, null))
								{
									objdb.Entry(itemx).State = System.Data.Entity.EntityState.Deleted;
								}
							}
							foreach (NawaDAL.EODSchedulerDetail item in objModuledata.objSchedulerDetail)
							{
								EODSchedulerDetail obcek = (from x in objdb.EODSchedulerDetails where x.PK_EODSchedulerDetail_ID == item.PK_EODSchedulerDetail_ID select x).FirstOrDefault;
								if (ReferenceEquals(obcek, null))
								{
									objdb.Entry(item).State = System.Data.Entity.EntityState.Added;
								}
								else
								{
									objdb.Entry(obcek).CurrentValues.SetValues(item);
									objdb.Entry(obcek).State = System.Data.Entity.EntityState.Modified;
								}
							}
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
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
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Delete)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							objdb.Entry(objModuledata.objScheduler).State = System.Data.Entity.EntityState.Deleted;
							List<EODSchedulerDetail> objTaskDetail = objdb.EODSchedulerDetails.Where(x => X.FK_EODSCheduler_ID == objModuledata.objScheduler.PK_EODScheduler_ID).ToList();
							foreach (EODSchedulerDetail item in objTaskDetail)
							{
								objdb.Entry(item).State = System.Data.Entity.EntityState.Deleted;
							}
							//audittrail
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
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
								if (!ReferenceEquals(item.GetValue(objModuledata, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Activation)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							objdb.Entry(objModuledata.objScheduler).State = System.Data.Entity.EntityState.Modified;
							//audittrail
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
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
								if (!ReferenceEquals(item.GetValue(objModuledata, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
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
						if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Insert)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							//audittrail
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
							objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
							objdb.SaveChanges();
							Type objtype = objModuledata.objScheduler.GetType();
							System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(objModuledata.objScheduler, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata.objScheduler, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							
							
							
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									
									
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
										
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									
									
									
									
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Update)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							EODSchedulerDataBLL objModuledataOld = NawaBLL.Common.Deserialize(objApproval.ModuleFieldBefore, typeof(EODSchedulerDataBLL));
							
							
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
							objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
							objdb.SaveChanges();
							Type objtype = objModuledata.objScheduler.GetType();
							System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								if (!ReferenceEquals(item.GetValue(objModuledataOld.objScheduler, null), null))
								{
									objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objModuledataOld.objScheduler, null));
								}
								else
								{
									objaudittraildetail.OldValue = "";
								}
								if (!ReferenceEquals(item.GetValue(objModuledata.objScheduler, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata.objScheduler, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							
							
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									
									
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
										
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									
									
									
									
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Delete)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							//audittrail
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
							objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
							objdb.SaveChanges();
							Type objtype = objModuledata.objScheduler.GetType();
							System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(objModuledata.objScheduler, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata.objScheduler, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									
									
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
										
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									
									
									
									
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
							}
						}
						else if (objApproval.PK_ModuleAction_ID == NawaBLL.Common.ModuleActionEnum.Activation)
						{
							EODSchedulerDataBLL objModuledata = NawaBLL.Common.Deserialize(objApproval.ModuleField, typeof(EODSchedulerDataBLL));
							//audittrail
							NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
							objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
							objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.Rejected;
							objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation;
							objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
							objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
							objdb.SaveChanges();
							Type objtype = objModuledata.objScheduler.GetType();
							System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(objModuledata.objScheduler, null), null))
								{
									objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objModuledata.objScheduler, null));
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
							
							foreach (NawaDAL.EODSchedulerDetail itemheader in objModuledata.objSchedulerDetail)
							{
								objtype = itemheader.GetType();
								properties = objtype.GetProperties();
								foreach (System.Reflection.PropertyInfo item in properties)
								{
									NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
									objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
									objaudittraildetail.FieldName = item.Name;
									objaudittraildetail.OldValue = "";
									
									
									if (!ReferenceEquals(item.GetValue(itemheader, null), null))
									{
										if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
										{
											
											objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
										}
										else
										{
											objaudittraildetail.NewValue = "";
										}
										
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									
									
									
									
									objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
								}
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
		public static NawaDAL.MsEODPeriod GetEODPeriodById(int id)
		{
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return objDb.MsEODPeriods.Where(x => X.PK_MsEODPeriod_Id == id).FirstOrDefault();
			}
			
		}
		public static void LoadPanel(FormPanel objPanel, string objdata, string strModulename, string unikkey)
		{
			NawaBLL.EODSchedulerDataBLL objEODSchedulerBLL = NawaBLL.Common.Deserialize(objdata, typeof(NawaBLL.EODSchedulerDataBLL));
			NawaDAL.EODScheduler objEODScheduler = default(NawaDAL.EODScheduler);
			objEODScheduler = objEODSchedulerBLL.objScheduler;
			if (!ReferenceEquals(objEODScheduler, null))
			{
				string strunik = unikkey;
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" + strunik, objEODScheduler.PK_EODScheduler_ID);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" + strunik, objEODScheduler.EODSchedulerName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" + strunik, objEODScheduler.EODSchedulerDescription);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" + strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" + strunik, objEODScheduler.FK_MsEODPeriod);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" + strunik, objEODScheduler.StartDate.GetValueOrDefault().ToString("dd-MMM-yyyy HH:mm:ss"));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" + strunik, objEODScheduler.Active);
				Ext.Net.Store objStore = new Ext.Net.Store();
				objStore.ID = strunik + "StoreGrid";
				objStore.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				Ext.Net.Model objModel = new Ext.Net.Model();
				Ext.Net.ModelField objField = default(Ext.Net.ModelField);
				objField = new Ext.Net.ModelField();
				objField.Name = "PK_EODSchedulerDetail_ID";
				objField.Type = ModelFieldType.Auto;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "EODTaskName";
				objField.Type = ModelFieldType.String;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "OrderNo";
				objField.Type = ModelFieldType.Int;
				objModel.Fields.Add(objField);
				objStore.Model.Add(objModel);
				List<ColumnBase> objListcolumn = new List<ColumnBase>();
				using (Ext.Net.RowNumbererColumn objcolumnNo = new Ext.Net.RowNumbererColumn())
				{
					objcolumnNo.Text = "No.";
					objcolumnNo.ClientIDMode = System.Web.UI.ClientIDMode.Static;
					objListcolumn.Add(objcolumnNo);
				}
				
				Ext.Net.Column objColum = default(Ext.Net.Column);
				objColum = new Ext.Net.Column();
				objColum.Text = "Task Name";
				objColum.DataIndex = "EODTaskName";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				objColum = new Ext.Net.Column();
				objColum.Text = "Order No";
				objColum.DataIndex = "OrderNo";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				
				System.Data.DataTable objdt = NawaBLL.Common.CopyGenericToDataTable(objEODSchedulerBLL.objSchedulerDetail);
				System.Data.DataColumn objcol = new System.Data.DataColumn();
				objcol.ColumnName = "EODTaskName";
				objcol.DataType = typeof(string);
				objdt.Columns.Add(objcol);
				
				foreach (DataRow item in objdt.Rows)
				{
					NawaDAL.EODTask objtask = EODTaskBLL.GetEODTaskByPK(System.Convert.ToInt64(item["FK_EODTask_ID"]));
					if (!ReferenceEquals(objtask, null))
					{
						item["EODTaskName"] = objtask.EODTaskName;
					}
					
				}
				
				
				
				NawaBLL.Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdt);
				
				//Using objdatax As Data.DataTable = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam)
				
				//End Using
			}
		}
		
		public static bool ActivationTanpaapproval(string ID, NawaDAL.Module objSchemaModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						
						NawaDAL.EODScheduler objTaskdel = objdb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == ID).FirstOrDefault;
						
						
						objTaskdel.Active = !objTaskdel.Active;
						objdb.Entry(objTaskdel).State = System.Data.Entity.EntityState.Modified;
						
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						
						Type objtype = objTaskdel.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							
							if (!ReferenceEquals(item.GetValue(objTaskdel, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objTaskdel, null));
								
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
		public static bool ActivationDenganapproval(string ID, NawaDAL.Module objSchemaModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						
						NawaDAL.EODScheduler objTaskdel = GetEODSchedulerByPK(long.Parse(ID));
						List<NawaDAL.EODSchedulerDetail> objTaskDetail = GetEODSchedulerDetailByFKID(long.Parse(ID));
						
						objTaskdel.Active = !objTaskdel.Active;
						NawaBLL.EODSchedulerDataBLL objTaskDataBLL = new NawaBLL.EODSchedulerDataBLL();
						objTaskDataBLL.objScheduler = objTaskdel;
						objTaskDataBLL.objSchedulerDetail = objTaskDetail;
						
						
						string xmldata = NawaBLL.Common.Serialize(objTaskDataBLL);
						ModuleApproval objModuleApproval = new ModuleApproval();
						objModuleApproval.ModuleName = objSchemaModule.ModuleName;
						objModuleApproval.ModuleKey = objTaskDataBLL.objScheduler.PK_EODScheduler_ID;
						objModuleApproval.ModuleField = xmldata;
						objModuleApproval.ModuleFieldBefore = "";
						objModuleApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation;
						objModuleApproval.CreatedDate = DateTime.Now;
						objModuleApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						
						objdb.Entry(objModuleApproval).State = System.Data.Entity.EntityState.Added;
						
						
						
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Activation;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						
						
						Type objtype = objTaskdel.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							
							if (!ReferenceEquals(item.GetValue(objTaskdel, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objTaskdel, null));
								
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						
						foreach (NawaDAL.EODSchedulerDetail itemheader in objTaskDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								
								
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
									
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		
		public static void LoadPanelActivation(FormPanel objPanel, string strModulename, string unikkey)
		{
			NawaDAL.EODScheduler objEODScheduler = GetEODSchedulerByPK(long.Parse(unikkey));
			if (!ReferenceEquals(objEODScheduler, null))
			{
				string strunik = Guid.NewGuid().ToString();
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" + strunik, objEODScheduler.PK_EODScheduler_ID);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" + strunik, objEODScheduler.EODSchedulerName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" + strunik, objEODScheduler.EODSchedulerDescription);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" + strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" + strunik, objEODScheduler.FK_MsEODPeriod);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" + strunik, objEODScheduler.StartDate.GetValueOrDefault().ToString("dd-MMM-yyyy HH:mm:ss"));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" + strunik, objEODScheduler.Active.ToString() + " -->" + (!objEODScheduler.Active).ToString());
				Ext.Net.Store objStore = new Ext.Net.Store();
				objStore.ID = strunik + "StoreGrid";
				objStore.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				Ext.Net.Model objModel = new Ext.Net.Model();
				Ext.Net.ModelField objField = default(Ext.Net.ModelField);
				objField = new Ext.Net.ModelField();
				objField.Name = "PK_EODSchedulerDetail_ID";
				objField.Type = ModelFieldType.Auto;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "EODTaskName";
				objField.Type = ModelFieldType.String;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "OrderNo";
				objField.Type = ModelFieldType.Int;
				objModel.Fields.Add(objField);
				objStore.Model.Add(objModel);
				List<ColumnBase> objListcolumn = new List<ColumnBase>();
				using (Ext.Net.RowNumbererColumn objcolumnNo = new Ext.Net.RowNumbererColumn())
				{
					objcolumnNo.Text = "No.";
					objcolumnNo.ClientIDMode = System.Web.UI.ClientIDMode.Static;
					objListcolumn.Add(objcolumnNo);
				}
				
				Ext.Net.Column objColum = default(Ext.Net.Column);
				objColum = new Ext.Net.Column();
				objColum.Text = "Task Name";
				objColum.DataIndex = "EODTaskName";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				
				objColum = new Ext.Net.Column();
				objColum.Text = "Order No";
				objColum.DataIndex = "OrderNo";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				System.Data.SqlClient.SqlParameter[] objparam = new System.Data.SqlClient.SqlParameter[1];
				objparam[0] = new System.Data.SqlClient.SqlParameter();
				objparam[0].ParameterName = "@EODSchedulerID";
				objparam[0].SqlDbType = SqlDbType.BigInt;
				objparam[0].Value = objEODScheduler.PK_EODScheduler_ID;
				using (System.Data.DataTable objdata = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam))
				{
					NawaBLL.Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdata);
				}
				
			}
		}
		
		public static void LoadPanelDelete(FormPanel objPanel, string strModulename, string unikkey)
		{
			NawaDAL.EODScheduler objEODScheduler = GetEODSchedulerByPK(long.Parse(unikkey));
			if (!ReferenceEquals(objEODScheduler, null))
			{
				string strunik = Guid.NewGuid().ToString();
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "ID", "PK_EODScheduler_ID" + strunik, objEODScheduler.PK_EODScheduler_ID);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Name", "EODSchedulerName" + strunik, objEODScheduler.EODSchedulerName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Description", "EODSchedulerDescription" + strunik, objEODScheduler.EODSchedulerDescription);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period", "EODPeriod" + strunik, GetEODPeriodById(objEODScheduler.EODPeriod).MsEODPeriodName);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Scheduler Period Type", "EODPeriodType" + strunik, objEODScheduler.FK_MsEODPeriod);
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Start Date", "StartDate" + strunik, objEODScheduler.StartDate.GetValueOrDefault().ToString("dd-MMM-yyyy HH:mm:ss"));
				NawaBLL.Nawa.BLL.NawaFramework.ExtDisplayField(objPanel, "Active", "Active" + strunik, objEODScheduler.Active);
				Ext.Net.Store objStore = new Ext.Net.Store();
				objStore.ID = strunik + "StoreGrid";
				objStore.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				Ext.Net.Model objModel = new Ext.Net.Model();
				Ext.Net.ModelField objField = default(Ext.Net.ModelField);
				objField = new Ext.Net.ModelField();
				objField.Name = "PK_EODSchedulerDetail_ID";
				objField.Type = ModelFieldType.Auto;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "EODTaskName";
				objField.Type = ModelFieldType.String;
				objModel.Fields.Add(objField);
				objField = new Ext.Net.ModelField();
				objField.Name = "OrderNo";
				objField.Type = ModelFieldType.Int;
				objModel.Fields.Add(objField);
				objStore.Model.Add(objModel);
				List<ColumnBase> objListcolumn = new List<ColumnBase>();
				using (Ext.Net.RowNumbererColumn objcolumnNo = new Ext.Net.RowNumbererColumn())
				{
					objcolumnNo.Text = "No.";
					objcolumnNo.ClientIDMode = System.Web.UI.ClientIDMode.Static;
					objListcolumn.Add(objcolumnNo);
				}
				
				Ext.Net.Column objColum = default(Ext.Net.Column);
				objColum = new Ext.Net.Column();
				objColum.Text = "Task Name";
				objColum.DataIndex = "EODTaskName";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				
				objColum = new Ext.Net.Column();
				objColum.Text = "Order No";
				objColum.DataIndex = "OrderNo";
				objColum.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				objColum.Flex = 1;
				objListcolumn.Add(objColum);
				System.Data.SqlClient.SqlParameter[] objparam = new System.Data.SqlClient.SqlParameter[1];
				objparam[0] = new System.Data.SqlClient.SqlParameter();
				objparam[0].ParameterName = "@EODSchedulerID";
				objparam[0].SqlDbType = SqlDbType.BigInt;
				objparam[0].Value = objEODScheduler.PK_EODScheduler_ID;
				using (System.Data.DataTable objdata = SQLHelper.ExecuteTable(SQLHelper.strConnectionString, CommandType.StoredProcedure, "usp_GetSchedulerDetailBySchedulerID", objparam))
				{
					NawaBLL.Nawa.BLL.NawaFramework.ExtGridPanel(objPanel, "Scheduler Detail", objStore, objListcolumn, objdata);
				}
				
			}
		}
		public static bool DeleteDenganapproval(string ID, NawaDAL.Module objSchemaModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						NawaDAL.EODScheduler objSchedulerdel = objdb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == ID).FirstOrDefault;
						List<NawaDAL.EODSchedulerDetail> objSchedulerDetail = objdb.EODSchedulerDetails.Where(x => x.FK_EODSCheduler_ID == ID).ToList();
						NawaBLL.EODSchedulerDataBLL objTaskDataBLL = new NawaBLL.EODSchedulerDataBLL();
						objTaskDataBLL.objScheduler = objSchedulerdel;
						objTaskDataBLL.objSchedulerDetail = objSchedulerDetail;
						string xmldata = NawaBLL.Common.Serialize(objTaskDataBLL);
						ModuleApproval objModuleApproval = new ModuleApproval();
						objModuleApproval.ModuleName = objSchemaModule.ModuleName;
						objModuleApproval.ModuleKey = objTaskDataBLL.objScheduler.PK_EODScheduler_ID;
						objModuleApproval.ModuleField = xmldata;
						objModuleApproval.ModuleFieldBefore = "";
						objModuleApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
						objModuleApproval.CreatedDate = DateTime.Now;
						objModuleApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objdb.Entry(objModuleApproval).State = System.Data.Entity.EntityState.Added;
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						Type objtype = objSchedulerdel.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objSchedulerdel, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objSchedulerdel, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objSchedulerDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		public static bool DeleteTanpaapproval(string ID, NawaDAL.Module objSchemaModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						NawaDAL.EODScheduler objTaskdel = objdb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == ID).FirstOrDefault;
						List<NawaDAL.EODSchedulerDetail> objTaskDetail = objdb.EODSchedulerDetails.Where(x => x.FK_EODSCheduler_ID == ID).ToList();
						objdb.Entry(objTaskdel).State = System.Data.Entity.EntityState.Deleted;
						foreach (EODSchedulerDetail item in objTaskDetail)
						{
							objdb.Entry(item).State = System.Data.Entity.EntityState.Deleted;
						}
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Delete;
						objaudittrailheader.ModuleLabel = objSchemaModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						Type objtype = objTaskdel.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objTaskdel, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objTaskdel, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objTaskDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		public static bool IsDataValidDelete(string ID, NawaDAL.Module objSchemaModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				NawaDAL.EODScheduler objdel = objdb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == ID).FirstOrDefault;
				if (!ReferenceEquals(objdel, null))
				{
					NawaDAL.ModuleApproval objapprovaldel = objdb.ModuleApprovals.Where(x => x.ModuleName == objSchemaModule.ModuleName && x.ModuleKey == ID).FirstOrDefault();
					if (!ReferenceEquals(objapprovaldel, null))
					{
						throw (new Exception(objSchemaModule.ModuleLabel + " " + objdel.EODSchedulerName + " already exist in pending approval."));
					}
				}
			}
			
			return true;
		}
		public static EODScheduler GetEODSchedulerByPKID(long id)
		{
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return objDb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == id).FirstOrDefault;
			}
			
		}
		public static List<EODSchedulerDetail> GetEODSchedulerDetailByFKID(long id)
		{
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return objDb.EODSchedulerDetails.Where(x => X.FK_EODSCheduler_ID == id).ToList();
			}
			
		}
		public static List<MsEODPeriod> GetMsEODPeriods()
		{
			using (NawaDAL.NawaDataEntities objDb = new NawaDAL.NawaDataEntities())
			{
				return objDb.MsEODPeriods.ToList();
			}
			
		}
		public bool SaveEditApproval(NawaDAL.EODScheduler objData, List<NawaDAL.EODSchedulerDetail> objSchedulerDetail, NawaDAL.Module objModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						NawaBLL.EODSchedulerDataBLL objxData = new NawaBLL.EODSchedulerDataBLL();
						objxData.objScheduler = objData;
						objxData.objSchedulerDetail = objSchedulerDetail;
						string xmldata = NawaBLL.Common.Serialize(objxData);
						
						NawaDAL.EODScheduler objEODSchedulerbefore = objdb.EODSchedulers.Where(x => X.PK_EODScheduler_ID == objxData.objScheduler.PK_EODScheduler_ID).FirstOrDefault;
						List<NawaDAL.EODSchedulerDetail> obEODSchedulerDetailBefore = objdb.EODSchedulerDetails.Where(x => x.FK_EODSCheduler_ID == objxData.objScheduler.PK_EODScheduler_ID).ToList();
						
						NawaBLL.EODSchedulerDataBLL objxDatabefore = new NawaBLL.EODSchedulerDataBLL();
						objxDatabefore.objScheduler = objEODSchedulerbefore;
						objxDatabefore.objSchedulerDetail = obEODSchedulerDetailBefore;
						string xmlbefore = NawaBLL.Common.Serialize(objxDatabefore);
						
						
						NawaDAL.ModuleApproval objModuleApproval = new NawaDAL.ModuleApproval();
						objModuleApproval.ModuleName = objModule.ModuleName;
						objModuleApproval.ModuleKey = objxData.objScheduler.PK_EODScheduler_ID;
						objModuleApproval.ModuleField = xmldata;
						objModuleApproval.ModuleFieldBefore = xmlbefore;
						objModuleApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objModuleApproval.CreatedDate = DateTime.Now;
						objModuleApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objdb.Entry(objModuleApproval).State = System.Data.Entity.EntityState.Added;
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						Type objtype = objData.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							if (!ReferenceEquals(item.GetValue(objEODSchedulerbefore, null), null))
							{
								objaudittraildetail.OldValue = System.Convert.ToString(item.GetValue(objEODSchedulerbefore, null));
							}
							else
							{
								objaudittraildetail.OldValue = "";
							}
							if (!ReferenceEquals(item.GetValue(objData, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objData, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objSchedulerDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		public bool SaveAddApproval(NawaDAL.EODScheduler objData, List<NawaDAL.EODSchedulerDetail> objSchedulerDetail, NawaDAL.Module objModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						NawaBLL.EODSchedulerDataBLL objxData = new NawaBLL.EODSchedulerDataBLL();
						objxData.objScheduler = objData;
						objxData.objSchedulerDetail = objSchedulerDetail;
						string xmldata = NawaBLL.Common.Serialize(objxData);
						
						NawaDAL.ModuleApproval objModuleApproval = new NawaDAL.ModuleApproval();
						objModuleApproval.ModuleName = objModule.ModuleName;
						objModuleApproval.ModuleKey = System.Convert.ToString(0);
						objModuleApproval.ModuleField = xmldata;
						objModuleApproval.ModuleFieldBefore = "";
						objModuleApproval.PK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
						objModuleApproval.CreatedDate = DateTime.Now;
						objModuleApproval.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objdb.Entry(objModuleApproval).State = System.Data.Entity.EntityState.Added;
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.WaitingToApproval;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
						objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						Type objtype = objData.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objData, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objData, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objSchedulerDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		public bool SaveEditTanpaApproval(NawaDAL.EODScheduler objData, List<NawaDAL.EODSchedulerDetail> objSchedulerDetail, NawaDAL.Module objModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objData.ApprovedDate = DateTime.Now;
						objdb.Entry(objData).State = System.Data.Entity.EntityState.Modified;
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Update;
						objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						
						foreach (EODSchedulerDetail itemx in (from x in objdb.EODSchedulerDetails where x.FK_EODSCheduler_ID == objData.PK_EODScheduler_ID select x).ToList())
						{
							EODSchedulerDetail objcek = objSchedulerDetail.Find(x => X.PK_EODSchedulerDetail_ID == itemx.PK_EODSchedulerDetail_ID);
							if (ReferenceEquals(objcek, null))
							{
								objdb.Entry(itemx).State = System.Data.Entity.EntityState.Deleted;
							}
						}
						foreach (NawaDAL.EODSchedulerDetail item in objSchedulerDetail)
						{
							EODSchedulerDetail obcek = (from x in objdb.EODSchedulerDetails where x.PK_EODSchedulerDetail_ID == item.PK_EODSchedulerDetail_ID select x).FirstOrDefault;
							if (ReferenceEquals(obcek, null))
							{
								objdb.Entry(item).State = System.Data.Entity.EntityState.Added;
							}
							else
							{
								objdb.Entry(obcek).CurrentValues.SetValues(item);
								objdb.Entry(obcek).State = System.Data.Entity.EntityState.Modified;
							}
						}
						Type objtype = objData.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objData, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objData, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objSchedulerDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
		public bool SaveAddTanpaApproval(NawaDAL.EODScheduler objData, List<NawaDAL.EODSchedulerDetail> objSchedulerDetail, NawaDAL.Module objModule)
		{
			using (NawaDAL.NawaDataEntities objdb = new NawaDAL.NawaDataEntities())
			{
				using (System.Data.Entity.DbContextTransaction objtrans = objdb.Database.BeginTransaction())
				{
					try
					{
						objData.ApprovedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objData.ApprovedDate = DateTime.Now;
						objdb.Entry(objData).State = System.Data.Entity.EntityState.Added;
						NawaDAL.AuditTrailHeader objaudittrailheader = new NawaDAL.AuditTrailHeader();
						objaudittrailheader.ApproveBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedBy = NawaBLL.Common.SessionCurrentUser.UserID;
						objaudittrailheader.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						objaudittrailheader.FK_AuditTrailStatus_ID = NawaBLL.Common.AuditTrailStatusEnum.AffectedToDatabase;
						objaudittrailheader.FK_ModuleAction_ID = NawaBLL.Common.ModuleActionEnum.Insert;
						objaudittrailheader.ModuleLabel = objModule.ModuleLabel;
						objdb.Entry(objaudittrailheader).State = System.Data.Entity.EntityState.Added;
						objdb.SaveChanges();
						foreach (NawaDAL.EODSchedulerDetail item in objSchedulerDetail)
						{
							item.FK_EODSCheduler_ID = objData.PK_EODScheduler_ID;
							objdb.Entry(item).State = System.Data.Entity.EntityState.Added;
						}
						Type objtype = objData.GetType();
						System.Reflection.PropertyInfo[] properties = objtype.GetProperties();
						foreach (System.Reflection.PropertyInfo item in properties)
						{
							NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
							objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
							objaudittraildetail.FieldName = item.Name;
							objaudittraildetail.OldValue = "";
							if (!ReferenceEquals(item.GetValue(objData, null), null))
							{
								objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(objData, null));
							}
							else
							{
								objaudittraildetail.NewValue = "";
							}
							objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
						}
						foreach (NawaDAL.EODSchedulerDetail itemheader in objSchedulerDetail)
						{
							objtype = itemheader.GetType();
							properties = objtype.GetProperties();
							foreach (System.Reflection.PropertyInfo item in properties)
							{
								NawaDAL.AuditTrailDetail objaudittraildetail = new NawaDAL.AuditTrailDetail();
								objaudittraildetail.FK_AuditTrailHeader_ID = objaudittrailheader.PK_AuditTrail_ID;
								objaudittraildetail.FieldName = item.Name;
								objaudittraildetail.OldValue = "";
								if (!ReferenceEquals(item.GetValue(itemheader, null), null))
								{
									if (item.GetValue(itemheader, null).GetType().ToString() != "System.Byte[]")
									{
										objaudittraildetail.NewValue = System.Convert.ToString(item.GetValue(itemheader, null));
									}
									else
									{
										objaudittraildetail.NewValue = "";
									}
								}
								else
								{
									objaudittraildetail.NewValue = "";
								}
								objdb.Entry(objaudittraildetail).State = System.Data.Entity.EntityState.Added;
							}
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
