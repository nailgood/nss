Option Explicit On

Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Web
Imports Utility
Imports Database
Imports System.Web.UI.WebControls

Namespace DataLayer
    Public Class StoreDepartmentRow
        Inherits StoreDepartmentRowBase

        Private _cacheKey As String = "StoreDepartment_"
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            MyBase.New(database, DepartmentId)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentName As String, ByVal ParentId As Integer)
            MyBase.New(database, DepartmentName, ParentId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetDepartmentForSearch() As List(Of String)
            Dim c As List(Of String)
            Dim key As String = String.Format(cachePrefixKey & "GetDepartmentForSearch")

            c = CType(CacheUtils.GetCache(key), List(Of String))
            If Not c Is Nothing Then
                Return c
            End If

            c = New List(Of String)
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "select DepartmentId, replace(Name, ' and ', ' & ') as Name from StoreDepartment"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SP)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    c.Add(r.Item("Name").ToString().ToLower().Trim())
                End While
                Core.CloseReader(r)

                CacheUtils.SetCache(key, c, Utility.ConfigData.TimeCacheData)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetDepartmentForSearch", ex)
            End Try

            Return c
        End Function
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DepartmentId As Integer) As StoreDepartmentRow
            Dim row As StoreDepartmentRow

            row = New StoreDepartmentRow(_Database, DepartmentId)
            row.Load()

            Return row
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As StoreDepartmentRow
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    Dim result As New StoreDepartmentRow
                    If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                        result.DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                    Else
                        result.DepartmentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Lft"))) Then
                        result.Lft = Convert.ToInt32(reader("Lft"))
                    Else
                        result.Lft = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Rgt"))) Then
                        result.Rgt = Convert.ToInt32(reader("Rgt"))
                    Else
                        result.Rgt = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                        result.ParentId = Convert.ToInt32(reader("ParentId"))
                    Else
                        result.ParentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        result.Name = reader("Name").ToString()
                    Else
                        result.Name = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImage"))) Then
                        result.LargeImage = reader("LargeImage").ToString()
                    Else
                        result.LargeImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageUrl"))) Then
                        result.LargeImageUrl = reader("LargeImageUrl").ToString()
                    Else
                        result.LargeImageUrl = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageAltTag"))) Then
                        result.LargeImageAltTag = reader("LargeImageAltTag").ToString()
                    Else
                        result.LargeImageAltTag = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        result.Image = reader("Image").ToString()
                    Else
                        result.Image = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ImageAltTag"))) Then
                        result.ImageAltTag = reader("ImageAltTag").ToString()
                    Else
                        result.ImageAltTag = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("NameImage"))) Then
                        result.NameImage = reader("NameImage").ToString()
                    Else
                        result.NameImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        result.PageTitle = reader("PageTitle").ToString()
                    Else
                        result.PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSPageTitle"))) Then
                        result.OutsideUSPageTitle = reader("OutsideUSPageTitle").ToString()
                    Else
                        result.OutsideUSPageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        result.MetaDescription = reader("MetaDescription").ToString()
                    Else
                        result.MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSMetaDescription"))) Then
                        result.OutsideUSMetaDescription = reader("OutsideUSMetaDescription").ToString()
                    Else
                        result.OutsideUSMetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                        result.MetaKeywords = reader("MetaKeywords").ToString()
                    Else
                        result.MetaKeywords = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsInactive"))) Then
                        result.IsInactive = Convert.ToBoolean(reader("IsInactive"))
                    Else
                        result.IsInactive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        result.Description = reader("Description").ToString()
                    Else
                        result.Description = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MenuImage"))) Then
                        result.MenuImage = reader("MenuImage").ToString()
                    Else
                        result.MenuImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MenuImageWidth"))) Then
                        result.MenuImageWidth = Convert.ToInt32(reader("MenuImageWidth"))
                    Else
                        result.MenuImageWidth = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TitleImage"))) Then
                        result.TitleImage = reader("TitleImage").ToString()
                    Else
                        result.TitleImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("NameRewriteUrl"))) Then
                        result.NameRewriteUrl = reader("NameRewriteUrl").ToString()
                    Else
                        result.NameRewriteUrl = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsQuickOrder"))) Then
                        result.IsQuickOrder = Convert.ToBoolean(reader("IsQuickOrder"))
                    Else
                        result.IsQuickOrder = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsFilter"))) Then
                        result.IsFilter = Convert.ToBoolean(reader("IsFilter"))
                    Else
                        result.IsFilter = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("BannerEffect"))) Then
                        result.BannerEffect = Convert.ToInt32(reader("BannerEffect"))
                    Else
                        result.BannerEffect = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                        result.URLCode = reader("URLCode").ToString()
                    Else
                        result.URLCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("AlternateName"))) Then
                        result.AlternateName = reader("AlternateName").ToString()
                    Else
                        result.AlternateName = ""
                    End If
                    Return result
                End If
                Return Nothing
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreDepartment.vb", func, ex)
        End Sub

        Public Shared Function FullByUrlCode(ByVal URLCode As String) As String
            Dim key As String = String.Format("{0}FullByUrlCode_{1}", cachePrefixKey, URLCode)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If result IsNot Nothing Then
                Return result
            End If

            result = String.Empty
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "[sp_StoreDepartment_FullByUrlCode]"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "URLCode", DbType.String, URLCode)
                result = db.ExecuteScalar(cmd)

                Core.CloseReader(dr)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function

        Public Shared Function FullByDeparmentId(ByVal DepartmentId As Integer) As String
            Dim key As String = String.Format("{0}FullByDeparmentId_{1}", cachePrefixKey, DepartmentId)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If result IsNot Nothing Then
                Return result
            End If

            result = String.Empty
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "[sp_StoreDepartment_FullByDepartmentId]"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                result = db.ExecuteScalar(cmd)

                Core.CloseReader(dr)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function
        Public Shared Function GetAllLevelDepartment() As StoreDepartmentCollection

            Dim c As New StoreDepartmentCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreDepartment_GetAllLevelDepartment"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    Dim item As New StoreDepartmentRow()
                    item.DepartmentId = r.Item("DepartmentId")
                    item.Name = Convert.ToString(r.Item("Name"))
                    If r.Item("AlternateName") Is Convert.DBNull Then
                        item.AlternateName = Nothing
                    Else
                        item.AlternateName = Convert.ToString(r.Item("AlternateName"))
                    End If
                    If r.Item("URLCode") Is Convert.DBNull Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = Convert.ToString(r.Item("URLCode"))
                    End If
                    If r.Item("ParentId") Is Convert.DBNull Then
                        item.ParentId = Nothing
                    Else
                        item.ParentId = Convert.ToInt32(r.Item("ParentId"))
                    End If
                    If r.Item("PageTitle") Is Convert.DBNull Then
                        item.PageTitle = Nothing
                    Else
                        item.PageTitle = Convert.ToString(r.Item("PageTitle"))
                    End If
                    If r.Item("MetaDescription") Is Convert.DBNull Then
                        item.MetaDescription = Nothing
                    Else
                        item.MetaDescription = Convert.ToString(r.Item("MetaDescription"))
                    End If
                    If r.Item("MetaKeywords") Is Convert.DBNull Then
                        item.MetaKeywords = Nothing
                    Else
                        item.MetaKeywords = Convert.ToString(r.Item("MetaKeywords"))
                    End If
                    c.Add(item)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetAllLevelDepartment", ex)
            End Try

            Return c
        End Function
        Public Shared Function FullByItemId(ByVal ItemId As Integer, ByVal DepartmentId As Integer) As String
            Dim key As String = String.Format("{0}FullByItemId_{1}_{2}", cachePrefixKey, ItemId, DepartmentId)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If result IsNot Nothing Then
                Return result
            End If

            result = String.Empty
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "[sp_StoreDepartment_FullByItemId]"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                result = db.ExecuteScalar(cmd)

                Core.CloseReader(dr)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function

        'Shared function to get one row
        Public Shared Function GetRowById(ByVal _Database As Database, ByVal DepartmentId As Integer) As StoreDepartmentRow
            Dim ss As StoreDepartmentRow = Nothing
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_StoreDepartment_GetById"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DepartmentId", SqlDbType.Int, 0, DepartmentId))
                dr = cmd.ExecuteReader()
                If dr.HasRows Then
                    If dr.Read Then
                        ss = GetDataFromReader(dr)
                    End If
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetRowById(ByVal _Database As Database, ByVal DepartmentId As Integer)", ex)
            End Try
            Return ss
        End Function
        Public Shared Function IsItemIncategory(ByVal _Database As Database, ByVal departmentCode As String, ByVal itemcode As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_StoreDepartment_IsItemInDepartment"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DepartmentURLCode", SqlDbType.VarChar, 0, departmentCode))
                cmd.Parameters.Add(_Database.InParam("ItemCode", SqlDbType.VarChar, 0, itemcode))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If (result = 1) Then
                Return True
            End If
            Return False
        End Function


        Public Shared Function GetRow(ByVal _Database As Database, ByVal DepartmentName As String, ByVal ParentId As Integer) As StoreDepartmentRow
            Dim row As StoreDepartmentRow

            row = New StoreDepartmentRow(_Database, DepartmentName, ParentId)
            row.Load()

            Return row
        End Function

        'Custom Methods
        Public Shared Function DepartmentInsert(ByVal DB As Database, ByVal DepartmentId As Integer, ByVal Name As String) As Integer
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Dim SQL As String = "exec dbo.sp_StoreDepartmentInsert " & DepartmentId & "," & DB.Quote(Name)
            DB.ExecuteSQL(SQL)

            Dim DeptId As Integer = DB.ExecuteScalar("SELECT top 1 DepartmentId FROM StoreDepartment ORDER BY DepartmentId DESC")
            Return DeptId
        End Function

        Public Shared Function ChangeDepartmentSortOrder(ByVal DepartmentId As Integer, ByVal sAction As String) As Boolean
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Dim result As Integer
            Try
                Dim sp As String = "sp_StoreDepartment_ChangeDepartmentArrange"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                db.AddInParameter(cmd, "sAction", DbType.String, sAction)
                result = db.ExecuteNonQuery(cmd)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
            End Try
            Return False
        End Function


        Public Shared Function GetDepartmentsToKeepOpened(ByVal DB As Database, ByVal DEPARTMENTS As String) As String
            Dim Str, sConn, sSQL As String
            Dim rsDepartments As SqlDataReader = Nothing

            Try
                GetDepartmentsToKeepOpened = String.Empty
                If DB.IsEmpty(DEPARTMENTS) Then Exit Function
                sSQL = " SELECT P2.DepartmentId FROM StoreDepartment AS P1, StoreDepartment AS P2" _
                  & " WHERE P1.lft BETWEEN P2.lft AND P2.rgt" _
                  & " AND P1.DepartmentId IN " & DB.NumberMultiple(DEPARTMENTS)

                Str = ""
                sConn = ""
                rsDepartments = DB.GetReader(sSQL)
                While rsDepartments.Read
                    Str = Str & sConn & CStr(rsDepartments("DepartmentId"))
                    sConn = ","
                End While
                Core.CloseReader(rsDepartments)
            Catch ex As Exception
                Core.CloseReader(rsDepartments)
            End Try
            GetDepartmentsToKeepOpened = Str
        End Function
        Public Function GetChildrenDepartments() As DataSet

            Dim ds As DataSet

            Dim key As String = cachePrefixKey & "GetChildrenDepartments_" & DepartmentId
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If



            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreDepartment_GetChildrenDepartments"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)

            ds = db.ExecuteDataSet(cmd)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetDepartmentByParentId(ByVal ParentId As Integer) As List(Of StoreDepartmentRow)
            Dim result As List(Of StoreDepartmentRow) = New List(Of StoreDepartmentRow)
            Dim dr As SqlDataReader = Nothing
            Dim sp As String = "sp_StoreDepartment_GetDepartmentByParentId"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, ParentId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of StoreDepartmentRow)(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "GetDepartmentByParentId", ex.ToString())
            End Try

            Return result
        End Function


        Public Shared Function GetDepartmentPath(ByVal ItemId As Integer, ByVal departmentID As Integer) As String
            Dim result As String = String.Empty
            ItemId = IIf(ItemId = Nothing, 0, ItemId)
            Dim key As String = cachePrefixKey & "GetDepartmentPath_" & ItemId & "_" & departmentID
            result = CType(CacheUtils.GetCache(key), String)
            If Not result Is Nothing Then
                Return result
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreDepartmentItem_GetDepartmentPath")
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, departmentID)
            Dim reader As System.Data.IDataReader = db.ExecuteReader(cmd)

            Try
                If reader.Read Then
                    result = CStr(reader.GetValue(0))
                    CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "GetDepartmentPath", ex.ToString())
            End Try

            Return result
        End Function
        Public Shared Function GetDepartmentPathByURLCode(ByVal ItemCode As String, ByVal departmentCode As String) As String
            If ItemCode Is Nothing And departmentCode Is Nothing Then
                Return String.Empty
            End If
            Dim result As String = String.Empty
            Dim key As String = cachePrefixKey & "GetDepartmentPathByURLCode" & ItemCode & "_" & departmentCode
            result = CType(CacheUtils.GetCache(key), String)
            If Not result Is Nothing Then
                Return result
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreDepartmentItem_GetDepartmentPathByURLCode")
            db.AddInParameter(cmd, "ItemCode", DbType.String, ItemCode)
            db.AddInParameter(cmd, "DepartmentCode", DbType.String, departmentCode)
            Dim reader As System.Data.IDataReader = db.ExecuteReader(cmd)

            Try
                If reader.Read Then
                    result = CStr(reader.GetValue(0))
                    CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "GetDepartmentPathByURLCode", ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function LoadListMainPage(ByVal departmentId As Integer) As StoreDepartmentCollection
            Dim result As StoreDepartmentCollection = Nothing
            Dim key As String = cachePrefixKey & "LoadListMainPage_" & departmentId
            result = CType(CacheUtils.GetCache(key), StoreDepartmentCollection)
            If Not result Is Nothing AndAlso result.Count > 0 Then
                Return result
            Else
                result = New StoreDepartmentCollection
            End If

            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreDepartment_GetListMainPage")
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, departmentId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                While (reader.Read())
                    Dim objDepartment As New StoreDepartmentRow
                    objDepartment.DepartmentId = Convert.ToInt32(reader.Item("DepartmentId"))
                    objDepartment.ParentId = Convert.ToInt32(reader.Item("ParentId"))
                    objDepartment.Name = Convert.ToString(reader.Item("Name"))
                    objDepartment.URLCode = Convert.ToString(reader.Item("URLCode"))
                    objDepartment.LargeImage = Convert.ToString(reader.Item("LargeImage"))
                    result.Add(objDepartment)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "LoadListMainPage", "departmentId=" & departmentId & "<br>Exception: " & ex.ToString() + "")
            End Try

            If Not result Is Nothing AndAlso result.Count > 0 Then
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            End If

            Return result
        End Function
        Public Shared Function ReplaceUrl(ByVal str As String) As String
            If str <> "" And str <> Nothing Then
                str = RTrim(LTrim(str))
                str = str.Replace("/", "-").Replace("%3e", "greater").Replace("<", "").Replace(">", "greater").Replace(" / ", "-").Replace(" & ", "_and_").Replace("+", "-").Replace("_-_", "-").Replace(" ", "-").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("&", "-").Replace(",", "-").Replace(".", "-").Replace("%3f", "").Replace(".", "").Replace("?", "").Replace("%26", "and").Replace("%22", "").Replace("%3a", "").Replace("%2c", "").Replace("%24", "$").Replace("%c3%a9", "e").Replace("%25", "percent").Replace("%20", "-").Replace("%22", "").Replace("%c2%a2", "¢").Replace("%c3%a8", "e").Replace("'", "").Replace("#", "").Replace("%e2%80%99", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "-").Replace("è", "e").Replace("é", "e").Replace("¢", "").Replace("e2809c", "").Replace("e2809d", "").Replace(" - ", "-").Replace("--", "-")
                str = str.Replace("--", "-").Replace("_", "-")
            End If
            Return str
        End Function

        Private Shared Function GetBreadCrumd(ByVal departmentId As Integer) As SqlDataReader
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------

            Dim reader As SqlDataReader

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SALESPRICE_GETOBJECT As String = "sp_StoreDepartment_GetBreadCrumbByDepartment"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_GETOBJECT)

            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, departmentId)

            reader = CType(db.ExecuteReader(cmd), SqlDataReader)

            Return reader
            '------------------------------------------------------------------------
        End Function

        Public Shared Function DisplayBreadCrumb(ByVal DB As Database, ByVal DepartmentId As Integer, ByVal IsLastLink As Boolean)
            'Dim SQL As String
            Dim Result As String = String.Empty
            Dim res As SqlDataReader = Nothing
            Try
                Result &= "<a class=""bdcrmblnk"" href=""/"">Home</a>"
                res = GetBreadCrumd(DepartmentId)
                Dim flag As Boolean = False
                While res.Read()
                    Dim DepartmentCode As String = ""
                    If IsDBNull(res("NameRewriteUrl")) = False Then
                        If res("NameRewriteUrl") <> "" Then
                            DepartmentCode = ReplaceUrl(res("NameRewriteUrl"))
                        Else
                            DepartmentCode = ReplaceUrl(res("URLCode"))
                        End If
                    Else
                        DepartmentCode = ReplaceUrl(res("URLCode"))
                    End If
                    If Trim(DepartmentId) = Trim(res("DepartmentId")) Then
                        'If IsLastLink Then
                        '    'Rewrite Url Result &= " &gt; <a class=""bdcrmblnk"" href=""/store/default.aspx?DepartmentId=" & res("DepartmentId") & """>" & res("NAME") & "</a>"
                        '    Result &= " &gt; <a class=""bdcrmblnk"" href=""/" & DepartmentName & "/" & res("DepartmentId") & ".aspx"">" & res("NAME") & "</a>"

                        'Else
                        Result &= " &gt; <span class=""bcative"">" & res("NAME") & "</span>"
                        'End If
                    Else
                        'Result &= " &gt; <a class=""bdcrmblnk"" href=""/store/default.aspx?DepartmentId=" & res("DepartmentId") & """>" & res("NAME") & "</a>"
                        If Not flag Then
                            Result &= " &gt; <a class=""bdcrmblnk"" href=""/" & GetBreadCrumdByLevel(CInt(res("ParentId"))) & "/" & DepartmentCode & """>" & res("NAME") & "</a>"
                            flag = True
                        Else
                            Result &= " &gt; <a class=""bdcrmblnk"" href=""/" & GetBreadCrumdByLevel(CInt(res("ParentId"))) & "/" & DepartmentCode & """>" & res("NAME") & "</a>"
                        End If
                        ''Result &= " &gt; <a class=""bdcrmblnk"" href=""/nail-supplies/" & DepartmentCode & """>" & res("NAME") & "</a>"
                    End If
                End While
                Core.CloseReader(res)
            Catch ex As Exception
                Core.CloseReader(res)
            End Try
            Return Result
        End Function

        Public Shared Function CheckDuplicateURLCode(ByVal urlCode As String, ByVal DepartmentId As Integer) As Boolean
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREDEPARTMENT_CHECKDUPLICATEURLCODE As String = "sp_StoreDepartment_CheckDuplicateURLCode"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_CHECKDUPLICATEURLCODE)
            db.AddInParameter(cmd, "URLCode", DbType.String, urlCode)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
            db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
            db.ExecuteNonQuery(cmd)
            result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
#Region "Sassi"

        Private Shared Function GetSassiBreadCrumdByLevel(ByVal parentId As Integer) As String
            If parentId = Utility.ConfigData.RootDepartmentID Then
                Return "beauty-supplies"
            End If
            Return "beauty-supply"
        End Function

#End Region


        Public Shared Function DisplayBreadCrumbItemDetail(ByVal DB As Database, ByVal DepartmentId As Integer, ByVal IsLastLink As Boolean, ByVal ServerVariable As String)
            'Dim SQL As String
            Dim res As SqlDataReader = Nothing
            Dim Result As String = String.Empty
            Try

                res = GetBreadCrumd(DepartmentId)
                While res.Read()

                    Dim DepartmentCode As String = ""
                    DepartmentCode = ReplaceUrl(res("URLCode"))
                    If Trim(DepartmentId) = Trim(res("DepartmentId")) Then
                        Result &= "<li class='active'><a href='/" & GetBreadCrumdByLevel(CInt(res("ParentId"))) & "/" & DepartmentCode & "'>" & res("NAME") & "</a></li>"
                    Else
                        Result &= "<li><a  href='/" & GetBreadCrumdByLevel(CInt(res("ParentId"))) & "/" & DepartmentCode & "'>" & res("NAME") & "</a></li>"
                    End If
                End While
                Core.CloseReader(res)
            Catch ex As Exception
                Core.CloseReader(res)
            End Try
            Return Result
        End Function

        Private Shared Function GetBreadCrumdByLevel(ByVal parentId As Integer) As String
            If parentId = Utility.ConfigData.RootDepartmentID Then
                Return "nail-supplies"
            End If
            Return "nail-supply"
        End Function



        Public Shared Function DisplayBreadCrumb(ByVal _DB As Database, ByVal DepartmentId As Integer, ByVal IsLastLink As Boolean, ByVal PageParams As String)
            If System.Web.HttpContext.Current.Request.ServerVariables("URL").Contains("/store/") Then
                Dim SQL, sConn, sAll, s, sPromotion As String
                Dim res As SqlDataReader = Nothing
                Try
                    Dim Result As String = String.Empty
                    Dim Context As System.Web.HttpContext = System.Web.HttpContext.Current
                    Dim BrandId As Integer = Nothing
                    If IsNumeric(Context.Request("brandid")) Then
                        BrandId = Context.Request("brandid")
                    End If

                    SQL = ""
                    SQL &= " SELECT D2.* FROM StoreDepartment AS D1, StoreDepartment AS D2"
                    SQL &= " WHERE D1.lft BETWEEN D2.lft AND D2.rgt"
                    SQL &= " AND D1.DepartmentId = " & _DB.Quote(DepartmentId)
                    SQL &= " AND D1.IsInactive = 0"
                    SQL &= " ORDER BY D2.LFT"

                    Trace.Write(SQL)
                    sConn = ""
                    res = _DB.GetReader(SQL)

                    If Context.Request("F_Promotion") = "Y" Then
                        sPromotion = "<span class=""mag"">Sales &amp; Specials</span>"
                    Else
                        sPromotion = ""
                    End If

                    Dim IsItemPage As Boolean = Context.Request.ServerVariables("URL") = "/store/item.aspx"

                    While res.Read()
                        If Context.Request("F_All") = "Y" AndAlso (IsDBNull(res("ParentId")) OrElse res("ParentId") = 23) Then sAll = "All " Else sAll = ""
                        If res("DepartmentId") <> 23 Then s = sAll & res("NAME") Else s = res("NAME")

                        If Trim(DepartmentId) = Trim(res("DepartmentId")) Then
                            If IsLastLink OrElse IsItemPage OrElse BrandId <> Nothing OrElse (DepartmentId = 23 AndAlso (sAll <> "" OrElse sPromotion <> "")) Then
                                Dim qs As URLParameters
                                qs = New URLParameters(Context.Request.QueryString, "F_Promotion;ItemId;DepartmentId")
                                Result &= sConn & "<a class=""bdcrmblnk"" href=""/store/default.aspx" & IIf(res("DepartmentId") <> 23, qs.ToString("DepartmentId", res("DepartmentId")), "") & """>" & s & "</a>"
                                sConn = " &gt; "
                                If sPromotion <> "" AndAlso IsItemPage Then Result &= sConn & "<a href=""/store/default.aspx?DepartmentId=" & res("DepartmentId") & "&F_Promotion=Y" & IIf(Context.Request("F_All") = "Y", "&F_All=Y", "") & """ class=""maglnk"">Sales &amp; Specials</a>"
                            Else
                                Dim qs As URLParameters
                                qs = New URLParameters(Context.Request.QueryString, "F_Promotion;ItemId")
                                If sPromotion <> "" Then
                                    Result &= sConn & "<a href=""/store/default.aspx" & IIf(res("DepartmentId") <> 23, qs.ToString, "") & """>" & s & "</a>" & sConn & sPromotion
                                Else
                                    Result &= sConn & "<span class=""bcative"">" & s & "</span>"
                                End If
                            End If
                        Else
                            Result &= sConn & "<a class=""bdcrmblnk"" href=""/store/default.aspx?DepartmentId=" & res("DepartmentId") & "&" & IIf(res("DepartmentId") <> 23, PageParams, "") & """>" & s & "</a>"
                        End If
                        sConn = " &gt; "
                        If DepartmentId = 23 AndAlso (sPromotion <> "" OrElse sAll <> "") Then
                            If sPromotion <> "" Then
                                If Not IsItemPage Then Result &= sConn & sPromotion
                            Else
                                If BrandId = Nothing Then Result &= sConn & "<span class=""bcative"">All Items</span>" Else Result &= sConn & "<a class=""bdcrmblnk"" href=""/store/default.aspx?DepartmentId=" & DepartmentId & "&" & PageParams & """>" & Trim("All Items") & "</a>"
                            End If
                        End If
                    End While
                    Core.CloseReader(res)
                    If BrandId <> Nothing Then Result &= sConn & "<span class=""bcative"">" & StoreBrandRow.GetBrandNameById(BrandId) & "</span>"

                    Return Result
                Catch ex As Exception
                    Core.CloseReader(res)
                End Try

            End If

            Return ""
        End Function

        Public Shared Function GetDefaultDepartment(ByVal _DB As Database) As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------

            Dim departmentId As Integer = 0
            Dim key As String = cachePrefixKey & "GetDefaultDepartment"
            departmentId = CType(CacheUtils.GetCache(key), Integer)
            If departmentId > 0 Then
                Return departmentId
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreDepartment_GetDefaultDepartment"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            departmentId = Convert.ToInt32(db.ExecuteScalar(cmd))
            CacheUtils.SetCache(key, departmentId, ConfigData.CacheTimeDepartment)
            '------------------------------------------------------------------------

            Return departmentId
        End Function
        Public Shared Function GetDefaultDepartmentURLCode(ByVal _DB As Database) As String

            Dim code As String = 0
            Dim key As String = cachePrefixKey & "GetDefaultDepartmentURLCode"
            code = CType(CacheUtils.GetCache(key), String)
            If code <> "" Then
                Return code
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreDepartment_GetDefaultDepartmentURLCode"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            code = db.ExecuteScalar(cmd)
            CacheUtils.SetCache(key, code, ConfigData.CacheTimeDepartment)
            '------------------------------------------------------------------------
            Return code
        End Function
        Public Shared Function CheckTab(ByVal Path As String) As Boolean
            Dim bExist As Boolean = False

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            'Lay DepartmentId
            Dim DepartmentId As Int32 = 23
            Try
                If Not Path.Contains("main.aspx") Then
                    Dim i As Int16 = Path.LastIndexOf("_") + 1
                    Dim y As Int16 = Path.LastIndexOf(".aspx")
                    DepartmentId = Path.Substring(i, y - i)
                Else
                    DepartmentId = CInt(HttpContext.Current.Request("DepartmentId"))
                End If

            Catch ex As Exception

            End Try
            Dim SP As String = "sp_StoreDepartment_CheckTab"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)

            bExist = Convert.ToInt32(db.ExecuteScalar(cmd)) > 0

            Return bExist
        End Function
        Public Shared Function GetChildDepartmentByParentId(ByVal parentId As Integer) As DataSet
            Dim ds As DataSet

            Dim key As String = cachePrefixKey & "GetChildDepartmentByParentId_" & parentId
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If


            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreDepartment_GetChildrenDepartments"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, parentId)

            ds = db.ExecuteDataSet(cmd)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetChildDepartmentByParent(ByVal parentId As Integer) As DataSet
            Dim key As String = cachePrefixKey & "GetChildDepartmentByParent_" & parentId
            Dim ds As DataSet
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If
            '' Dim sql As String = "select URLCode,Name from StoreDepartment where lft > " & parentLeft & " and rgt < " & parentright & " and IsInactive = 0 order by lft"
            Dim sql As String = "select ParentId,NameRewriteUrl,DepartmentId,URLCode,Name,IsQuickOrder from StoreDepartment where ParentId = " & parentId & " and IsInactive = 0 and [dbo].[fc_StoreDepartment_CountItem](DepartmentId)>0 order by Arrange"


            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
            cmd.CommandType = CommandType.Text
            ds = db.ExecuteDataSet(cmd)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetByBrandId(ByVal DepartmentId As Integer, ByVal brandId As Integer) As DataTable
            Try
                Dim ds As DataSet
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreDepartment_GetByBrandId")
                db.AddInParameter(cmd, "BrandId", DbType.Int32, brandId)
                db.AddInParameter(cmd, "CurrentDepartmentId", DbType.Int32, DepartmentId)
                ds = db.ExecuteDataSet(cmd)
                Return ds.Tables(0)
            Catch ex As Exception

            End Try
            Return Nothing
        End Function

        Public Function GetSiblingDepartments() As DataSet
            Dim ds As DataSet

            Dim key As String = cachePrefixKey & "GetSiblingDepartments_" & DepartmentId
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If

            Dim SQL As String = "SELECT Coalesce(ImageAltTag,Name) As ImgAltTag, * FROM StoreDepartment WHERE ParentId IN (SELECT ParentId FROM StoreDepartment WHERE DepartmentId = " & DB.Quote(DepartmentId) & ") AND IsInactive = 0"
            ds = DB.GetDataSet(SQL)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Function GetActiveItemsForNavigator() As DataSet
            Dim ds As DataSet
            Dim key As String = cachePrefixKey & "GetActiveItemsForNavigator_" & DepartmentId
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If
            Dim SQL As String
            SQL = " select "
            SQL &= " coalesce((case when tmp.IsNew = 1 and NewUntil is not null and DateDiff(d, getdate(), NewUntil) >=0 then 1 else 0 end),0) as WhatsNew,"
            SQL &= " coalesce((select ItemId from StoreItemFeature sif, ItemFeature i where sif.FeatureId = i.FeatureId and sif.ItemId = tmp.ItemId and i.Code = 'Exclusive'),0) as Exclusive,"
            SQL &= " coalesce((select ItemId from StoreItemFeature sif, ItemFeature i where sif.FeatureId = i.FeatureId and sif.ItemId = tmp.ItemId and i.Code = 'WebOnly'),0) as WebOnly,"
            SQL &= " coalesce((select ItemId from StoreItemFeature sif, ItemFeature i where sif.FeatureId = i.FeatureId and sif.ItemId = tmp.ItemId and i.Code = 'BestSellers'),0) as BestSellers,"
            SQL &= " case when (NofSale > 0 or Price < OrigPrice) then 1 else 0 End as IsOnSale from ("
            SQL &= " SELECT "
            SQL &= " (select count(*) from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') and Price < OrigPrice) as NofSale,"
            SQL &= " coalesce((select top 1 si2.Price from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') order by si2.Price asc), Price) as MinPrice, "
            SQL &= " coalesce((select top 1 si2.Price from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') order by si2.Price desc), Price)  as MaxPrice,"
            SQL &= " coalesce((select top 1 si2.OrigPrice from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') and si2.OrigPrice > 0 order by si2.OrigPrice asc), OrigPrice)  as MinOrigPrice, "
            SQL &= " coalesce((select top 1 si2.OrigPrice from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') and si2.OrigPrice > 0 order by si2.OrigPrice desc), OrigPrice) as MaxOrigPrice, "
            SQL &= " si.* FROM StoreItem si "
            SQL &= " where si.IsActive = 1 and si.Status not in ('C1', 'KC', 'J1') AND 'SaleCenterOnly' NOT IN (SELECT iff.Code FROM StoreItemFeature sif, ItemFeature iff WHERE ItemId = si.ItemId and sif.FeatureId = iff.FeatureId) "
            SQL &= " and	si.itemId in "
            SQL &= " ("
            SQL &= " select top 1 sdi.ItemId from StoreDepartmentItem sdi, StoreDepartment sd, StoreDepartment p"
            SQL &= " where"
            SQL &= "        si.ItemId = sdi.ItemId"
            SQL &= " and 	sdi.DepartmentId = sd.DepartmentId "
            SQL &= " and 	p.lft <= sd.lft "
            SQL &= " and 	p.rgt >= sd.lft"
            SQL &= " and 	sd.IsInactive = 0 "
            SQL &= " and 	p.DepartmentId = " & DepartmentId
            SQL &= " )) as tmp "
            ds = DB.GetDataSet(SQL)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetMainLevelDepartments(ByVal _DB As Database) As DataSet
            Dim ds As DataSet
            Dim key As String = cachePrefixKey & "GetMainLevelDepartments"
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                Return ds
            End If
            Dim SQL As String
            SQL = ""
            SQL &= " select DepartmentId, Name, AlternateName, URLCode, ParentId,isQuickOrder from StoreDepartment where ParentId = 23 and IsInactive = 0"
            SQL &= " order by Arrange ASC"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
            ds = db.ExecuteDataSet(cmd)
            Return ds
            ds = _DB.GetDataSet(SQL)
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            End If
            Return ds
        End Function
        Public Shared Function GetAllChildDepartments(ByVal _DB As Database) As DataSet
            Dim ds As DataSet
            Dim key As String = cachePrefixKey & "GetAllChildDepartments"
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If

            Dim SQL As String
            SQL = ""
            SQL &= " select * from StoreDepartment where ParentId is not null"
            SQL &= " order by Arrange ASC"
            ds = _DB.GetDataSet(SQL)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetMainLevelDepartment(ByVal _DB As Database, ByVal DepartmentId As Integer) As StoreDepartmentRow
            Dim row As StoreDepartmentRow
            Dim key As String = cachePrefixKey & "GetMainLevelDepartment_" & DepartmentId
            row = CType(CacheUtils.GetCache(key), StoreDepartmentRow)
            If Not row Is Nothing Then
                Return row
            End If

            Dim MainDepartmentId As Integer = 23 'Store Home
            If DepartmentId <> 23 Then
                MainDepartmentId = GetDepartmentIDMainLevel(DepartmentId)
            End If

            If MainDepartmentId > 0 Then
                row = StoreDepartmentRow.GetRow(_DB, MainDepartmentId)
                CacheUtils.SetCache(key, row, ConfigData.CacheTimeDepartment)
            End If

            Return row
        End Function

        Public Shared Function GetEndDepartment(ByVal _Database As Database, ByVal ItemId As Integer) As StoreDepartmentRow
            Dim row As New StoreDepartmentRow
            Dim key As String = cachePrefixKey & "GetEndDepartment_" & ItemId
            row = CType(CacheUtils.GetCache(key), StoreDepartmentRow)
            If Not row Is Nothing Then
                Return row
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreDepartment WHERE DepartmentId = dbo.GetLastestDepartment(" & _Database.Quote(ItemId) & ")"
                reader = _Database.GetReader(SQL)
                If reader.Read Then
                    row.Load(reader)
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, row, ConfigData.CacheTimeDepartment)

            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return row
        End Function

        Private Shared Function GetDepartmentIDMainLevel(ByVal departmentId As Integer) As Integer
            Dim result As Integer = 0
            Dim key As String = cachePrefixKey & "GetDepartmentIDMainLevel_" & departmentId
            result = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREDEPARTMENT_GETLIST As String = "sp_StoreDepartment_GetMainLevelDepartment"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETLIST)
                db.AddInParameter(cmd, "DepartmentID", DbType.Int32, departmentId)

                result = Convert.ToInt32(db.ExecuteScalar(cmd))
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function GetSaleDepartments(ByVal _Db As Database) As DataSet
            Dim ds As DataSet
            Dim key As String = cachePrefixKey & "GetSaleDepartments"
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If
            Dim SQL As String

            Dim SQLExclude As String
            SQLExclude = ""
            SQLExclude &= " SELECT DD1.DepartmentId FROM StoreDepartment AS DD1, StoreDepartment AS DD2 WHERE DD1.lft BETWEEN DD2.lft AND DD2.rgt AND DD2.DepartmentId in (SELECT DepartmentId FROM StoreDepartment WHERE IsInactive = 1"
            SQLExclude &= " )"

            SQL = " select distinct sd.DepartmentId, sd.Name, sd.NameRewriteUrl from ("
            SQL &= " SELECT "
            SQL &= " (select count(*) from StoreItem si2 where si2.IsActive=1 and si2.Status not in ('C1', 'KC', 'J1') and Price < OrigPrice) as NofSale,"
            SQL &= " si.* FROM StoreItem si "
            SQL &= " WHERE si.IsActive = 1 and si.Status not in ('C1', 'KC', 'J1')"
            SQL &= " ) as tmp, StoreDepartment sd, StoreDepartmentItem sdi, StoreDepartment sd2 where (NofSale > 0 or Price < OrigPrice) and tmp.ItemId = sdi.ItemId"
            SQL &= " and sd2.DepartmentId = sdi.DepartmentId"
            SQL &= " and sd.lft < sd2.lft and sd.rgt > sd2.rgt and sd.parentid = 23"
            SQL &= " and sd.DepartmentId not in (" & SQLExclude & ")"

            ds = _Db.GetDataSet(SQL)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function

        Public Shared Function GetDepartmentMenu(ByVal departmentId As Single) As SqlDataReader
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREDEPARTMENT_GETLIST As String = "sp_StoreDepartment_GetDepartmentMenu"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETLIST)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, departmentId)
            reader = CType(db.ExecuteReader(cmd), SqlDataReader)
            Return reader
            '------------------------------------------------------------------------
        End Function
        Protected Shared Function LoadByDataReader(ByVal reader As SqlDataReader)
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                Dim result As New StoreDepartmentRow
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                    result.DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                Else
                    result.DepartmentId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Lft"))) Then
                    result.Lft = Convert.ToInt32(reader("Lft"))
                Else
                    result.Lft = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Rgt"))) Then
                    result.Rgt = Convert.ToInt32(reader("Rgt"))
                Else
                    result.Rgt = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                    result.ParentId = Convert.ToInt32(reader("ParentId"))
                Else
                    result.ParentId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LargeImage"))) Then
                    result.LargeImage = reader("LargeImage").ToString()
                Else
                    result.LargeImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageUrl"))) Then
                    result.LargeImageUrl = reader("LargeImageUrl").ToString()
                Else
                    result.LargeImageUrl = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageAltTag"))) Then
                    result.LargeImageAltTag = reader("LargeImageAltTag").ToString()
                Else
                    result.LargeImageAltTag = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                    result.Image = reader("Image").ToString()
                Else
                    result.Image = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ImageAltTag"))) Then
                    result.ImageAltTag = reader("ImageAltTag").ToString()
                Else
                    result.ImageAltTag = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NameImage"))) Then
                    result.NameImage = reader("NameImage").ToString()
                Else
                    result.NameImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                    result.PageTitle = reader("PageTitle").ToString()
                Else
                    result.PageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSPageTitle"))) Then
                    result.OutsideUSPageTitle = reader("OutsideUSPageTitle").ToString()
                Else
                    result.OutsideUSPageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    result.MetaDescription = reader("MetaDescription").ToString()
                Else
                    result.MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSMetaDescription"))) Then
                    result.OutsideUSMetaDescription = reader("OutsideUSMetaDescription").ToString()
                Else
                    result.OutsideUSMetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                    result.MetaKeywords = reader("MetaKeywords").ToString()
                Else
                    result.MetaKeywords = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsInactive"))) Then
                    result.IsInactive = Convert.ToBoolean(reader("IsInactive"))
                Else
                    result.IsInactive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    result.Description = reader("Description").ToString()
                Else
                    result.Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MenuImage"))) Then
                    result.MenuImage = reader("MenuImage").ToString()
                Else
                    result.MenuImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MenuImageWidth"))) Then
                    result.MenuImageWidth = Convert.ToInt32(reader("MenuImageWidth"))
                Else
                    result.MenuImageWidth = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TitleImage"))) Then
                    result.TitleImage = reader("TitleImage").ToString()
                Else
                    result.TitleImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NameRewriteUrl"))) Then
                    result.NameRewriteUrl = reader("NameRewriteUrl").ToString()
                Else
                    result.NameRewriteUrl = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsQuickOrder"))) Then
                    result.IsQuickOrder = Convert.ToBoolean(reader("IsQuickOrder"))
                Else
                    result.IsQuickOrder = False
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsFilter"))) Then
                    result.IsFilter = Convert.ToBoolean(reader("IsFilter"))
                Else
                    result.IsFilter = False
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("BannerEffect"))) Then
                    result.BannerEffect = Convert.ToInt32(reader("BannerEffect"))
                Else
                    result.BannerEffect = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                    result.URLCode = reader("URLCode").ToString()
                Else
                    result.URLCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AlternateName"))) Then
                    result.AlternateName = reader("AlternateName").ToString()
                Else
                    result.AlternateName = ""
                End If
                Return result
            End If
            Return New StoreDepartmentRow
        End Function
        Public Shared Function GetByURLCode(ByVal _Db As Database, ByVal code As String) As StoreDepartmentRow
            If code = "" Then
                Return New StoreDepartmentRow
            End If
            Dim result As StoreDepartmentRow
            Dim key As String = cachePrefixKey & "GetByURLCode_" & code
            result = CType(CacheUtils.GetCache(key), StoreDepartmentRow)
            If Not result Is Nothing Then
                Return result
            Else
                result = New StoreDepartmentRow
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT top 1 * FROM StoreDepartment WHERE URLCode='" & code.Replace("'", "''") & "'"
                r = _Db.GetReader(SQL)
                If r.Read Then
                    result = LoadByDataReader(r)
                End If
                Core.CloseReader(r)
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return result
        End Function
        Public Shared Function GetDepartmentNameByURLCode(ByVal _Db As Database, ByVal code As String) As String
            Dim r As SqlDataReader
            Try
                If code = "" Then
                    Return String.Empty
                End If
                Dim result As String
                Dim key As String = cachePrefixKey & "GetDepartmentNameByURLCode_" & code
                result = CType(CacheUtils.GetCache(key), String)
                If result <> "" Then
                    Return result
                End If
                Dim SQL As String
                SQL = "SELECT top 1 Name FROM StoreDepartment WHERE URLCode='" & code & "'"
                r = _Db.GetReader(SQL)
                If r.Read Then
                    result = r.GetValue(0).ToString()
                End If
                r.Close()
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
                Return result
            Catch ex As Exception

            End Try
        End Function
        Public Shared Function GetDepartmentIdByDepertmentCode(ByVal departmentCode As String) As Integer
            If departmentCode = "" Then
                Return 0
            End If
            Dim result As Integer = 0
            Dim key As String = cachePrefixKey & "GetDepartmentIdByDepertmentCode_" & departmentCode
            result = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If
            Dim sql As String = "Select DepartmentId from StoreDepartment where URLCode='" & departmentCode & "'"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim reader As SqlDataReader = Nothing
            Try
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read Then
                    result = Convert.ToInt32(reader.GetValue(0))
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Public Shared Function GetURLCodeByDepertmentId(ByVal Id As Integer) As String
            If Id < 1 Then
                Return Nothing
            End If
            Dim result As String = ""
            Dim key As String = cachePrefixKey & "GetURLCodeByDepertmentId" & Id
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim sql As String = "Select URLCode from StoreDepartment where DepartmentId=" & Id & ""
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim reader As SqlDataReader = Nothing
            Try
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Public Shared Function GetDepartmentNameByDepertmentId(ByVal Id As Integer) As String
            If Id < 1 Then
                Return Nothing
            End If
            Dim result As String = ""
            Dim key As String = cachePrefixKey & "GetDepartmentNameByDepertmentId_" & Id
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim sql As String = "Select Name from StoreDepartment where DepartmentId=" & Id & ""
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim reader As SqlDataReader = Nothing
            Try
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, ConfigData.CacheTimeDepartment)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Public Shared Function GetListCategoryFilterSearchKeyword(keyword As String, categories As String, Optional filterCategory As String = "") As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreDepartment_GetListDepartmentFilterSearchKeyword")
            db.AddInParameter(cmd, "ArrDepartmentId", DbType.String, categories)
            db.AddInParameter(cmd, "KeywordName", DbType.String, keyword)
            db.AddInParameter(cmd, "CategoryFilter", DbType.String, filterCategory)

            Using dr As SqlDataReader = db.ExecuteReader(cmd)
                Dim dt As New DataTable()
                dt.Load(dr)
                Return dt
            End Using
        End Function
        Public Shared Function GetFilterProductReview() As DataSet
            Dim ds As DataSet

            Dim key As String = cachePrefixKey & "GetFilterProductReview"
            ds = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing Then
                Return ds
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreDepartment_GetFilterProductReview"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            ds = db.ExecuteDataSet(cmd)
            CacheUtils.SetCache(key, ds, ConfigData.CacheTimeDepartment)
            Return ds
        End Function
        Public Shared Function GetListSlideEffectByDepartmentId(ByVal DB As Database, ByVal DepartmentId As Integer) As String
            Dim SQL As String = "Select EffectCode from DepartmentSlideEffect where DepartmentId=" & DB.Quote(DepartmentId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    Dim EffectCode As String = String.Empty
                    While r.Read()
                        EffectCode = r.Item("EffectCode")
                        result = result & "," & EffectCode
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)

            End Try
            Return result
        End Function
    End Class

    Public MustInherit Class StoreDepartmentRowBase
        Private m_DB As Database
        Private m_DepartmentId As Integer = Nothing
        Private m_Lft As Integer = Nothing
        Private m_Rgt As Integer = Nothing
        Private m_ParentId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_NameRewriteUrl As String = Nothing
        Private m_ImageAltTag As String = Nothing
        Private m_LargeImageAltTag As String = Nothing
        Private m_LargeImageUrl As String = Nothing
        Private m_LargeImage As String = Nothing
        Private m_ImageMap As String = Nothing
        Private m_Image As String = Nothing
        Private m_SortMethod As String = Nothing
        Private m_NameImage As String = Nothing
        Private m_TitleOn As String = Nothing
        Private m_TitleOff As String = Nothing
        Private m_TitleWidth As Integer = Nothing
        Private m_PageTitle As String = Nothing
        Private m_OutsideUSPageTitle As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_OutsideUSMetaDescription As String = Nothing
        Private m_ContentText As String
        Private m_IsInactive As Boolean = Nothing
        Private m_MenuImage As String = Nothing
        Private m_MenuImageWidth As Integer = Nothing
        Private m_TitleImage As String = Nothing
        Private m_Description As String = Nothing
        Private m_IsQuickOrder As Boolean = Nothing
        Private m_IsFilter As Boolean = Nothing
        Private m_BannerEffect As Integer = Nothing
        Private m_URLCode As String = Nothing
        Private m_AlternateName As String = Nothing
        Private m_ListEffectCode As String = String.Empty
        Private m_CountItem As Integer = 0

        Public Shared cachePrefixKey As String = "StoreDepartment_"

        Public Property CountItem() As Integer
            Get
                Return m_CountItem
            End Get
            Set(ByVal value As Integer)
                m_CountItem = value
            End Set
        End Property

        Public Property ListEffectCode() As String
            Get
                Return m_ListEffectCode
            End Get
            Set(ByVal value As String)
                m_ListEffectCode = value
            End Set
        End Property

        Public Property MenuImage() As String
            Get
                Return m_MenuImage
            End Get
            Set(ByVal Value As String)
                m_MenuImage = Value
            End Set
        End Property

        Public Property MenuImageWidth() As Integer
            Get
                Return m_MenuImageWidth
            End Get
            Set(ByVal Value As Integer)
                m_MenuImageWidth = Value
            End Set
        End Property

        Public Property TitleImage() As String
            Get
                Return m_TitleImage
            End Get
            Set(ByVal Value As String)
                m_TitleImage = Value
            End Set
        End Property
        Public Property URLCode() As String
            Get
                Return m_URLCode
            End Get
            Set(ByVal Value As String)
                m_URLCode = Value
            End Set
        End Property

        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentId = Value
            End Set
        End Property

        Public Property Lft() As Integer
            Get
                Return m_Lft
            End Get
            Set(ByVal Value As Integer)
                m_Lft = Value
            End Set
        End Property

        Public Property Rgt() As Integer
            Get
                Return m_Rgt
            End Get
            Set(ByVal Value As Integer)
                m_Rgt = Value
            End Set
        End Property

        Public Property ParentId() As Integer
            Get
                Return m_ParentId
            End Get
            Set(ByVal Value As Integer)
                m_ParentId = Value
            End Set
        End Property

        Public Property SortMethod() As String
            Get
                Return m_SortMethod
            End Get
            Set(ByVal Value As String)
                m_SortMethod = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property NameRewriteUrl() As String
            Get
                Return m_NameRewriteUrl
            End Get
            Set(ByVal Value As String)
                m_NameRewriteUrl = Value
            End Set
        End Property

        Public Property ImageAltTag() As String
            Get
                Return m_ImageAltTag
            End Get
            Set(ByVal Value As String)
                m_ImageAltTag = Value
            End Set
        End Property

        Public Property LargeImageAltTag() As String
            Get
                Return m_LargeImageAltTag
            End Get
            Set(ByVal Value As String)
                m_LargeImageAltTag = Value
            End Set
        End Property

        Public Property LargeImageUrl() As String
            Get
                Return m_LargeImageUrl
            End Get
            Set(ByVal Value As String)
                m_LargeImageUrl = Value
            End Set
        End Property

        Public Property ImageMap() As String
            Get
                Return m_ImageMap
            End Get
            Set(ByVal Value As String)
                m_ImageMap = Value
            End Set
        End Property

        Public Property LargeImage() As String
            Get
                Return m_LargeImage
            End Get
            Set(ByVal Value As String)
                m_LargeImage = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property

        Public Property NameImage() As String
            Get
                Return m_NameImage
            End Get
            Set(ByVal Value As String)
                m_NameImage = Value
            End Set
        End Property

        Public Property TitleOn() As String
            Get
                Return m_TitleOn
            End Get
            Set(ByVal Value As String)
                m_TitleOn = Value
            End Set
        End Property

        Public Property TitleOff() As String
            Get
                Return m_TitleOff
            End Get
            Set(ByVal Value As String)
                m_TitleOff = Value
            End Set
        End Property

        Public Property TitleWidth() As Integer
            Get
                Return m_TitleWidth
            End Get
            Set(ByVal Value As Integer)
                m_TitleWidth = Value
            End Set
        End Property

        Public Property BannerEffect() As Integer
            Get
                Return m_BannerEffect
            End Get
            Set(ByVal Value As Integer)
                m_BannerEffect = Value
            End Set
        End Property

        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property OutsideUSPageTitle() As String
            Get
                Return m_OutsideUSPageTitle
            End Get
            Set(ByVal Value As String)
                m_OutsideUSPageTitle = Value
            End Set
        End Property

        Public Property MetaKeywords() As String
            Get
                Return m_MetaKeywords
            End Get
            Set(ByVal Value As String)
                m_MetaKeywords = Value
            End Set
        End Property

        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
            End Set
        End Property
        Public Property OutsideUSMetaDescription() As String
            Get
                Return m_OutsideUSMetaDescription
            End Get
            Set(ByVal Value As String)
                m_OutsideUSMetaDescription = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
            End Set
        End Property

        Public Property ContentText() As String
            Get
                Return m_ContentText
            End Get
            Set(ByVal Value As String)
                m_ContentText = Value
            End Set
        End Property

        Public Property IsInactive() As Boolean
            Get
                Return m_IsInactive
            End Get
            Set(ByVal Value As Boolean)
                m_IsInactive = Value
            End Set
        End Property

        Public Property IsFilter() As Boolean
            Get
                Return m_IsFilter
            End Get
            Set(ByVal Value As Boolean)
                m_IsFilter = Value
            End Set
        End Property

        Public Property IsQuickOrder() As Boolean
            Get
                Return m_IsQuickOrder
            End Get
            Set(ByVal Value As Boolean)
                m_IsQuickOrder = Value
            End Set
        End Property
        Public Property AlternateName() As String
            Get
                Return m_AlternateName
            End Get
            Set(ByVal Value As String)
                m_AlternateName = Value
            End Set
        End Property
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            m_DB = database
            m_DepartmentId = DepartmentId
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentName As String, ByVal ParentId As Integer)
            m_DB = database
            m_Name = DepartmentName
            m_ParentId = ParentId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT * FROM StoreDepartment WHERE " & IIf(DepartmentId <> Nothing, "DepartmentId = " & DB.Number(DepartmentId), "ParentId = " & DB.Number(ParentId) & " and Name = " & DB.Quote(Name))
                If SQL = "SELECT * FROM StoreDepartment WHERE ParentId = 0 and Name = NULL" Then
                    'Dim rawURL As String = String.Empty
                    'If Not System.Web.HttpContext.Current Is Nothing Then
                    '    If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    '        rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                    '    End If
                    'End If

                    'Email.SendError("ToError500", "StoreDepartment.vb >> Load", "Url: " & rawURL & "<br>SQL: " & SQL & BaseShoppingCart.GetSessionList())
                    Exit Sub
                End If

                Dim dbs As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                r = dbs.ExecuteReader(CommandType.Text, SQL)
                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                        m_DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                    Else
                        m_DepartmentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Lft"))) Then
                        m_Lft = Convert.ToInt32(reader("Lft"))
                    Else
                        m_Lft = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Rgt"))) Then
                        m_Rgt = Convert.ToInt32(reader("Rgt"))
                    Else
                        m_Rgt = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                        m_ParentId = Convert.ToInt32(reader("ParentId"))
                    Else
                        m_ParentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        m_Name = reader("Name").ToString()
                    Else
                        m_Name = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImage"))) Then
                        m_LargeImage = reader("LargeImage").ToString()
                    Else
                        m_LargeImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageUrl"))) Then
                        m_LargeImageUrl = reader("LargeImageUrl").ToString()
                    Else
                        m_LargeImageUrl = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LargeImageAltTag"))) Then
                        m_LargeImageAltTag = reader("LargeImageAltTag").ToString()
                    Else
                        m_LargeImageAltTag = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        m_Image = reader("Image").ToString()
                    Else
                        m_Image = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ImageAltTag"))) Then
                        m_ImageAltTag = reader("ImageAltTag").ToString()
                    Else
                        m_ImageAltTag = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("NameImage"))) Then
                        m_NameImage = reader("NameImage").ToString()
                    Else
                        m_NameImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSPageTitle"))) Then
                        m_OutsideUSPageTitle = reader("OutsideUSPageTitle").ToString()
                    Else
                        m_OutsideUSPageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        m_MetaDescription = reader("MetaDescription").ToString()
                    Else
                        m_MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSMetaDescription"))) Then
                        m_OutsideUSMetaDescription = reader("OutsideUSMetaDescription").ToString()
                    Else
                        m_OutsideUSMetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                        m_MetaKeywords = reader("MetaKeywords").ToString()
                    Else
                        m_MetaKeywords = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsInactive"))) Then
                        m_IsInactive = Convert.ToBoolean(reader("IsInactive"))
                    Else
                        m_IsInactive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        m_Description = reader("Description").ToString()
                    Else
                        m_Description = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MenuImage"))) Then
                        m_MenuImage = reader("MenuImage").ToString()
                    Else
                        m_MenuImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MenuImageWidth"))) Then
                        m_MenuImageWidth = Convert.ToInt32(reader("MenuImageWidth"))
                    Else
                        m_MenuImageWidth = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TitleImage"))) Then
                        m_TitleImage = reader("TitleImage").ToString()
                    Else
                        m_TitleImage = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("NameRewriteUrl"))) Then
                        m_NameRewriteUrl = reader("NameRewriteUrl").ToString()
                    Else
                        m_NameRewriteUrl = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsQuickOrder"))) Then
                        m_IsQuickOrder = Convert.ToBoolean(reader("IsQuickOrder"))
                    Else
                        m_IsQuickOrder = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsFilter"))) Then
                        m_IsFilter = Convert.ToBoolean(reader("IsFilter"))
                    Else
                        m_IsFilter = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("BannerEffect"))) Then
                        m_BannerEffect = Convert.ToInt32(reader("BannerEffect"))
                    Else
                        m_BannerEffect = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                        m_URLCode = reader("URLCode").ToString()
                    Else
                        m_URLCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("AlternateName"))) Then
                        m_AlternateName = reader("AlternateName").ToString()
                    Else
                        m_AlternateName = ""
                    End If
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace & "<br>" & IIf(DepartmentId <> Nothing, "DepartmentId = " & DB.Quote(DepartmentId), "ParentId = " & DB.Number(ParentId) & " and Name = " & DB.Quote(Name)))
            End Try

        End Sub

        Public Overridable Sub Update()
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Dim SQL As String

            SQL = " UPDATE StoreDepartment SET " _
             & " Lft = " & m_DB.Number(Lft) _
             & ",Rgt = " & m_DB.Number(Rgt) _
             & ",ParentId = " & m_DB.NullNumber(ParentId) _
             & ",Name = " & m_DB.Quote(Name) _
             & ",NameRewriteUrl = " & m_DB.Quote(NameRewriteUrl) _
             & ",LargeImage = " & m_DB.Quote(LargeImage) _
             & ",LargeImageUrl = " & m_DB.Quote(LargeImageUrl) _
             & ",LargeImageAltTag = " & m_DB.Quote(LargeImageAltTag) _
             & ",Image = " & m_DB.Quote(Image) _
             & ",ImageAltTag = " & m_DB.Quote(ImageAltTag) _
             & ",NameImage = " & m_DB.Quote(NameImage) _
             & ",PageTitle = " & m_DB.Quote(PageTitle) _
             & ",MetaDescription = " & m_DB.Quote(MetaDescription) _
             & ",MetaKeywords = " & m_DB.Quote(MetaKeywords) _
             & ",IsInactive = " & CInt(IsInactive) _
             & ",Description = " & m_DB.Quote(Description) _
             & ",MenuImage = " & m_DB.Quote(MenuImage) _
             & ",MenuImageWidth = " & m_DB.Number(MenuImageWidth) _
             & ",TitleImage = " & m_DB.Quote(TitleImage) _
             & ",IsQuickOrder = " & CInt(IsQuickOrder) _
             & ",IsFilter = " & CInt(IsFilter) _
             & ",BannerEffect = " & CInt(BannerEffect) _
             & ",URLCode = " & m_DB.Quote(URLCode) _
             & ",OutsideUSPageTitle = " & m_DB.Quote(OutsideUSPageTitle) _
             & ",OutsideUSMetaDescription = " & m_DB.Quote(OutsideUSMetaDescription) _
             & ",AlternateName = " & m_DB.Quote(AlternateName) _
             & " WHERE DepartmentId = " & m_DB.Quote(DepartmentId)

            m_DB.ExecuteSQL(SQL)
        End Sub
        Public Overridable Sub Insert()
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_STOREDEPARTMENT_INSERT As String = "sp_StoreDepartment_Insert"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_INSERT)

                db.AddInParameter(cmd, "ParentId", DbType.Int32, ParentId)
                db.AddInParameter(cmd, "Name", DbType.String, Name)
                db.AddInParameter(cmd, "URLCode", DbType.String, URLCode)
                db.AddInParameter(cmd, "AlternateName", DbType.String, AlternateName)
                db.AddInParameter(cmd, "BannerEffect", DbType.Int32, BannerEffect)
                db.AddInParameter(cmd, "LargeImage", DbType.String, LargeImage)
                db.AddInParameter(cmd, "LargeImageUrl", DbType.String, LargeImageUrl)
                db.AddInParameter(cmd, "LargeImageAltTag", DbType.String, LargeImageAltTag)
                db.AddInParameter(cmd, "Image", DbType.String, Image)
                db.AddInParameter(cmd, "ImageAltTag", DbType.String, ImageAltTag)
                db.AddInParameter(cmd, "NameImage", DbType.String, NameImage)
                db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
                db.AddInParameter(cmd, "OutsideUSPageTitle", DbType.String, OutsideUSPageTitle)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
                db.AddInParameter(cmd, "OutsideUSMetaDescription", DbType.String, OutsideUSMetaDescription)
                db.AddInParameter(cmd, "MetaKeywords", DbType.String, MetaKeywords)
                db.AddInParameter(cmd, "IsInactive", DbType.Boolean, IsInactive)
                db.AddInParameter(cmd, "IsQuickOrder", DbType.Boolean, IsQuickOrder)
                db.AddInParameter(cmd, "IsFilter", DbType.Boolean, IsFilter)
                db.AddInParameter(cmd, "Description", DbType.String, Description)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                DepartmentId = result
            Catch ex As Exception
                Core.LogError("StoreDepartment.vb", "Insert", ex)
                Throw ex
            End Try
        End Sub

    End Class


    Public Class StoreDepartmentCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreDepartment As StoreDepartmentRow)
            Me.List.Add(StoreDepartment)
        End Sub

        Public Function Contains(ByVal StoreDepartment As StoreDepartmentRow) As Boolean
            Return Me.List.Contains(StoreDepartment)
        End Function

        Public Function IndexOf(ByVal StoreDepartment As StoreDepartmentRow) As Integer
            Return Me.List.IndexOf(StoreDepartment)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreDepartment As StoreDepartmentRow)
            Me.List.Insert(Index, StoreDepartment)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreDepartmentRow
            Get
                Return CType(Me.List.Item(Index), StoreDepartmentRow)
            End Get

            Set(ByVal Value As StoreDepartmentRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreDepartment As StoreDepartmentRow)
            Me.List.Remove(StoreDepartment)
        End Sub

    End Class
End Namespace
