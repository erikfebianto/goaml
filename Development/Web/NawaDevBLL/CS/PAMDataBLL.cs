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


namespace NawaDevBLL
{
	public class PAMDataBLL
	{
		
		
		
		private NawaDevDAL.EP_MS_PAM mPam = new NawaDevDAL.EP_MS_PAM();
		public NawaDevDAL.EP_MS_PAM PAM
		{
			get
			{
				return mPam;
			}
			set
			{
				mPam = value;
			}
		}
		
		
		private List<NawaDevDAL.EP_TR_PAM_Budget> mBudget = new List<NawaDevDAL.EP_TR_PAM_Budget>();
		public List<NawaDevDAL.EP_TR_PAM_Budget> Budget
		{
			get
			{
				return mBudget;
			}
			set
			{
				mBudget = value;
			}
		}
		private List<NawaDevDAL.EP_TR_PAM_Investment> mInvesment = new List<NawaDevDAL.EP_TR_PAM_Investment>();
		public List<NawaDevDAL.EP_TR_PAM_Investment> Invesment
		{
			get
			{
				return mInvesment;
			}
			set
			{
				mInvesment = value;
			}
		}
		
		private List<NawaDevDAL.EP_TR_PAM_Qualitative_Target> mTargeQualitative = new List<NawaDevDAL.EP_TR_PAM_Qualitative_Target>();
		public List<NawaDevDAL.EP_TR_PAM_Qualitative_Target> TargetQualitative
		{
			get
			{
				return mTargeQualitative;
			}
			set
			{
				mTargeQualitative = value;
			}
		}
		
		
		private List<NawaDevDAL.EP_TR_PAM_Qualitative_Target> mTargeQuantitative = new List<NawaDevDAL.EP_TR_PAM_Qualitative_Target>();
		public List<NawaDevDAL.EP_TR_PAM_Qualitative_Target> TargeQuantitative
		{
			get
			{
				return mTargeQuantitative;
			}
			set
			{
				mTargeQuantitative = value;
			}
		}
		
		private List<NawaDevDAL.EP_TR_PAM_Risk> mRisk = new List<NawaDevDAL.EP_TR_PAM_Risk>();
		public List<NawaDevDAL.EP_TR_PAM_Risk> Risk
		{
			get
			{
				return mRisk;
			}
			set
			{
				mRisk = value;
			}
		}
		
		private List<NawaDevDAL.EP_TR_PAM_Attachment> mAttachment = new List<NawaDevDAL.EP_TR_PAM_Attachment>();
		public List<NawaDevDAL.EP_TR_PAM_Attachment> Attachment
		{
			get
			{
				return mAttachment;
			}
			set
			{
				mAttachment = value;
			}
		}
		
		
		private List<NawaDevDAL.EP_TR_PAM_Approval_Role> mApprovalRole = new List<NawaDevDAL.EP_TR_PAM_Approval_Role>();
		public List<NawaDevDAL.EP_TR_PAM_Approval_Role> ApprovalRole
		{
			get
			{
				return mApprovalRole;
			}
			set
			{
				mApprovalRole = value;
			}
		}
		
		
		private List<NawaDevDAL.EP_TR_PAM_Approval_Member> mApprovalMember = new List<NawaDevDAL.EP_TR_PAM_Approval_Member>();
		public List<NawaDevDAL.EP_TR_PAM_Approval_Member> ApprovalMember
		{
			get
			{
				return mApprovalMember;
			}
			set
			{
				mApprovalMember = value;
			}
		}
		
	}
	
}
