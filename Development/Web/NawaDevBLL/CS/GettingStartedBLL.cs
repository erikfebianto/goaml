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

namespace NawaDevBLL
{
	public class GettingStartedBLL : IDisposable
	{
		
		
		
		public static DataTable GetGettingStarted(int intGroup)
		{
			
			SqlParameter[] objListParam = new SqlParameter[1];
			objListParam[0] = new SqlParameter();
			
			objListParam[0].ParameterName = "@groupid";
			objListParam[0].Value = intGroup;
			
			
			
			return NawaDAL.SQLHelper.ExecuteTable(NawaDAL.SQLHelper.strConnectionString, CommandType.StoredProcedure, "usg_GettingStartedList", objListParam);
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
