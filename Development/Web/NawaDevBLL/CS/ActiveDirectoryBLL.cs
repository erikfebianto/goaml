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

using System.DirectoryServices;

namespace NawaDevBLL
{
	public class ActiveDirectoryBLL
	{
		
		
		public static bool AuthenticateUser(string domain, string username, string password, string LdapPath, ref string Errmsg)
		{
			Errmsg = "";
			string domainAndUsername = Convert.ToString(domain + Convert.ToString("\\")) + username;
			DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);
			try
			{
				// Bind to the native AdsObject to force authentication.
				object obj = entry.NativeObject;
				DirectorySearcher search = new DirectorySearcher(entry);
				search.Filter = (Convert.ToString("(SAMAccountName=") + username) + ")";
				search.PropertiesToLoad.Add("cn");
				SearchResult result = search.FindOne();
				if (ReferenceEquals(result, null))
				{
					return false;
				}
				// Update the new path to the user in the directory
				LdapPath = result.Path;
				string _filterAttribute = (string) (result.Properties["cn"][0]);
			}
			catch (Exception ex)
			{
				Errmsg = ex.Message;
				throw (new Exception("Error authenticating user." + ex.Message));
				return false;
				
			}
			return true;
			
			
		}
	}
	
}
