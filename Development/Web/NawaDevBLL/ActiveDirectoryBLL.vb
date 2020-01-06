Imports System.DirectoryServices

Public Class ActiveDirectoryBLL


    Public Shared Function CheckUser(usertocheck As String) As Data.DataTable

        Dim domain As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(22).SettingValue
        Dim username As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(29).SettingValue
        Dim password As String = NawaBLL.Common.DecryptRijndael(NawaBLL.SystemParameterBLL.GetSystemParameterByPk(30).SettingValue, NawaBLL.SystemParameterBLL.GetSystemParameterByPk(30).EncriptionKey)
        Dim LdapPath As String = NawaBLL.SystemParameterBLL.GetSystemParameterByPk(21).SettingValue


        Dim domainAndUsername As String = Convert.ToString(domain & Convert.ToString("\")) & username
        Dim entry As New DirectoryEntry(LdapPath, domainAndUsername, password)
        Try

            ' Bind to the native AdsObject to force authentication.
            Dim obj As [Object] = entry.NativeObject
            Dim search As New DirectorySearcher(entry)
            search.Filter = (Convert.ToString("(SAMAccountName=") & usertocheck) + ")"
            search.PropertiesToLoad.Add("cn")
            search.PropertiesToLoad.Add("displayname")
            search.PropertiesToLoad.Add("manager")
            search.PropertiesToLoad.Add("Mail")
            search.PropertiesToLoad.Add("title")
            Dim result As SearchResult = search.FindOne()

            If result Is Nothing Then
                Return Nothing
            End If
            ' Update the new path to the user in the directory



            Dim objdtresult As Data.DataTable = New Data.DataTable()
            objdtresult.Columns.Add(New Data.DataColumn("cn", GetType(String)))
            objdtresult.Columns.Add(New Data.DataColumn("displayname", GetType(String)))
            objdtresult.Columns.Add(New Data.DataColumn("manager", GetType(String)))
            objdtresult.Columns.Add(New Data.DataColumn("Mail", GetType(String)))
            objdtresult.Columns.Add(New Data.DataColumn("title", GetType(String)))

            Dim cn As String = ""
            Dim displayname As String = ""
            Dim manager As String = ""
            Dim emailaddress As String = ""
            Dim title As String
            If result.Properties("cn").Count > 0 Then
                cn = DirectCast(result.Properties("cn")(0), [String])
            End If

            If result.Properties("displayname").Count > 0 Then
                displayname = DirectCast(result.Properties("displayname")(0), [String])
            End If

            If result.Properties("manager").Count > 0 Then
                manager = DirectCast(result.Properties("manager")(0), [String])
            End If

            If result.Properties("Mail").Count > 0 Then
                emailaddress = DirectCast(result.Properties("Mail")(0), [String])
            End If

            If result.Properties("title").Count > 0 Then
                title = DirectCast(result.Properties("title")(0), [String])
            End If


            Dim objnewrow As Data.DataRow = objdtresult.NewRow
            objnewrow("cn") = cn
            objnewrow("displayname") = displayname
            objnewrow("manager") = manager
            objnewrow("Mail") = emailaddress
            objnewrow("title") = title
            objdtresult.Rows.Add(objnewrow)
            Return objdtresult

        Catch ex As Exception
            Throw New Exception("Error authenticating user." + ex.Message)


        End Try



    End Function


    Public Shared Function AuthenticateUser(domain As String, username As String, password As String, LdapPath As String, ByRef Errmsg As String) As Boolean
        Errmsg = ""
        Dim domainAndUsername As String = Convert.ToString(domain & Convert.ToString("\")) & username
        Dim entry As New DirectoryEntry(LdapPath, domainAndUsername, password)
        Try
            ' Bind to the native AdsObject to force authentication.
            Dim obj As [Object] = entry.NativeObject
            Dim search As New DirectorySearcher(entry)
            search.Filter = (Convert.ToString("(SAMAccountName=") & username) + ")"
            search.PropertiesToLoad.Add("cn")
            Dim result As SearchResult = search.FindOne()
            If result Is Nothing Then
                Return False
            End If
            ' Update the new path to the user in the directory
            LdapPath = result.Path
            Dim _filterAttribute As String = DirectCast(result.Properties("cn")(0), [String])
        Catch ex As Exception
            Errmsg = ex.Message
            Throw New Exception("Error authenticating user." + ex.Message)
            Return False

        End Try
        Return True


    End Function
End Class
