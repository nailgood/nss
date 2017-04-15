Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Utility
Namespace DataLayer

    Public Class StoreBrandRow
        Inherits StoreBrandRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal BrandId As Integer)
            MyBase.New(BrandId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal BrandName As String)
            MyBase.New(DB, BrandName)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal BrandId As Integer) As StoreBrandRow
            Dim row As StoreBrandRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", BrandId)
            row = CType(CacheUtils.GetCache(key), StoreBrandRow)
            If Not row Is Nothing Then
                Return row
            End If
            row = New StoreBrandRow()
            row.BrandId = BrandId
            row.Load()
            CacheUtils.SetCache(key, row)
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal BrandId As Integer)
            Dim row As StoreBrandRow
            row = New StoreBrandRow(DB, BrandId)
            row.Remove()
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub


        ''' <summary>
        ''' Khoa: dung trong StoreBrowser
        ''' </summary>
        Public Shared Function GetAllBrand(ByVal DB As Database) As StoreBrandCollection
            Dim c As New StoreBrandCollection
            Dim key As String = cachePrefixKey & "GetAllBrand"
            c = CType(CacheUtils.GetCache(key), StoreBrandCollection)
            If Not c Is Nothing Then
                Return c.Clone()
            Else
                c = New StoreBrandCollection
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As StoreBrandRow
                Dim SQL As String = "select * from StoreBrand where BrandId IN (select distinct BrandId from StoreItem where IsActive = 1) order by BrandName asc"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New StoreBrandRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                CacheUtils.SetCache(key, c)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return c.Clone()
        End Function

        Public Shared Function GetListByDepartmentId(ByVal DB As Database, ByVal DepartmentId As Integer) As StoreBrandCollection
            Dim c As New StoreBrandCollection
            Dim key As String = cachePrefixKey & "GetListByDepartmentId_" & DepartmentId
            c = CType(CacheUtils.GetCache(key), StoreBrandCollection)
            If Not c Is Nothing Then
                Return c
            Else
                c = New StoreBrandCollection
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As StoreBrandRow
                Dim SQL As String = "select * from StoreBrand where BrandId IN (select distinct BrandId from StoreItem si inner join StoreDepartmentItem sdi on sdi.ItemId = si.ItemId where si.IsActive = 1 and sdi.DepartmentId = " & DepartmentId & ") order by BrandName asc"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New StoreBrandRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                CacheUtils.SetCache(key, c)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return c
        End Function

        Public Shared Function GetAllStoreBrands(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields) As DataTable
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            'Dim SP_STOREDEPARTMENT_GETLIST As String = "sp_StoreBrand_GetAllStoreBrandsV1"
            Dim SP_STOREDEPARTMENT_GETLIST As String = "sp_StoreBrand_GetAllStoreBrandsV2"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETLIST)

            If filter.ListBrandId <> Nothing Then
                db.AddInParameter(cmd, "ArrBrandId", DbType.String, filter.ListBrandId)
            Else
                db.AddInParameter(cmd, "ArrBrandId", DbType.String, "")
            End If

            db.AddInParameter(cmd, "IsFilterAll", DbType.Boolean, filter.All)
            db.AddInParameter(cmd, "MaxPerPage", DbType.Int32, filter.MaxPerPage)
            db.AddInParameter(cmd, "Pg", DbType.Int32, filter.pg)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, filter.DepartmentId)
            db.AddInParameter(cmd, "Keyword", DbType.String, filter.Keyword)
            db.AddInParameter(cmd, "IsFeatured", DbType.Boolean, filter.IsFeatured)
            db.AddInParameter(cmd, "Feature", DbType.String, filter.Feature)
            db.AddInParameter(cmd, "IsNew", DbType.Boolean, filter.IsNew)
            db.AddInParameter(cmd, "IsOnSale", DbType.Boolean, filter.IsOnSale)
            db.AddInParameter(cmd, "IsHot", DbType.Boolean, filter.IsHot)
            db.AddInParameter(cmd, "HasPromotion", DbType.Boolean, filter.HasPromotion)
            db.AddInParameter(cmd, "PromotionId", DbType.Int32, filter.PromotionId)

            Using dr As SqlDataReader = db.ExecuteReader(cmd)
                Dim dt As New DataTable()
                dt.Load(dr)
                Return dt
            End Using
        End Function

        Public Shared Function GetAllStoreBrandsNarrowSearch(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields) As DataTable
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREDEPARTMENT_GETLIST As String = "sp_StoreBrand_GetAllStoreBrandsForNarrowSearch"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETLIST)

            If filter.ListBrandId <> Nothing Then
                db.AddInParameter(cmd, "ArrBrandId", DbType.String, filter.ListBrandId)
            Else
                db.AddInParameter(cmd, "ArrBrandId", DbType.String, "")
            End If

            db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, filter.DepartmentId)
            db.AddInParameter(cmd, "IsFeatured", DbType.Boolean, filter.IsFeatured)
            db.AddInParameter(cmd, "Feature", DbType.String, filter.Feature)
            db.AddInParameter(cmd, "IsNew", DbType.Boolean, filter.IsNew)
            db.AddInParameter(cmd, "IsOnSale", DbType.Boolean, filter.IsOnSale)
            db.AddInParameter(cmd, "IsHot", DbType.Boolean, filter.IsHot)
            db.AddInParameter(cmd, "HasPromotion", DbType.Boolean, filter.HasPromotion)
            db.AddInParameter(cmd, "PromotionId", DbType.Int32, filter.PromotionId)
            db.AddInParameter(cmd, "ToneId", DbType.Int32, filter.ToneId)
            db.AddInParameter(cmd, "ShareId", DbType.Int32, filter.ShadeId)
            db.AddInParameter(cmd, "CollectionId", DbType.Int32, filter.CollectionId)
            Dim Range As String = ""
            If Not filter.PriceRange Is Nothing Then
                Range = filter.PriceRange.Replace("(", "").Replace(")", "")
            End If
            db.AddInParameter(cmd, "PriceRange", DbType.String, Range)
            Range = ""
            If Not filter.RatingRange Is Nothing Then
                Range = filter.RatingRange.Replace("(", "").Replace(")", "")
            End If
            db.AddInParameter(cmd, "RatingRange", DbType.String, Range)
            Using dr As SqlDataReader = db.ExecuteReader(cmd)
                Dim dt As New DataTable()
                dt.Load(dr)
                dr.Close()

                Return dt
            End Using
        End Function

        Public Shared Function GetListBrandFilterSearchKeyword(ByVal keyword As String, ByVal arrBrandId As String) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreBrand_GetListBrandFilterSearchKeyword")
            db.AddInParameter(cmd, "ArrBrandId", DbType.String, arrBrandId)
            db.AddInParameter(cmd, "KeywordName", DbType.String, keyword)

            Using dr As SqlDataReader = db.ExecuteReader(cmd)
                Dim dt As New DataTable()
                dt.Load(dr)
                Return dt
            End Using

        End Function

        Public Shared Function GetAllStoreBrandsCount(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields) As Integer
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREDEPARTMENT_GETOBJECT As String = "sp_StoreBrand_GetAllStoreBrandsCount"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_GETOBJECT)

            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, filter.DepartmentId)
            db.AddInParameter(cmd, "Keyword", DbType.String, filter.Keyword)
            db.AddInParameter(cmd, "IsFeatured", DbType.Boolean, filter.IsFeatured)
            db.AddInParameter(cmd, "IsNew", DbType.Boolean, filter.IsNew)
            db.AddInParameter(cmd, "IsOnSale", DbType.Boolean, filter.IsOnSale)
            db.AddInParameter(cmd, "HasPromotion", DbType.Boolean, filter.HasPromotion)
            db.AddInParameter(cmd, "PromotionId", DbType.Int32, filter.PromotionId)

            result = Convert.ToInt32(db.ExecuteScalar(cmd))
            Return result
        End Function

        Protected Shared Function LoadByDataReader(ByVal reader As SqlDataReader) As StoreBrandRow
            Dim result As New StoreBrandRow
            If (Not reader.IsDBNull(reader.GetOrdinal("BrandId"))) Then
                result.BrandId = Convert.ToInt32(reader("BrandId"))
            Else
                result.BrandId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("BrandName"))) Then
                result.BrandName = reader("BrandName").ToString()
            Else
                result.BrandName = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                result.URLCode = reader("URLCode").ToString()
            Else
                result.URLCode = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                result.Description = reader("Description").ToString()
            Else
                result.Description = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("HeaderImage"))) Then
                result.HeaderImage = reader("HeaderImage").ToString()
            Else
                result.HeaderImage = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("BrandNameUrl"))) Then
                result.BrandNameUrl = reader("BrandNameUrl").ToString()
            Else
                result.BrandNameUrl = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("IsTop"))) Then
                result.IsTop = CBool(reader("IsTop"))
            Else
                result.IsTop = False
            End If
            Return result
            'End If
        End Function 'Load
        Public Shared Function GetBrandNameById(ByVal id As Integer) As String
            If id < 1 Then
                Return String.Empty
            End If

            Dim result As String
            Dim key As String = String.Format(cachePrefixKey & "GetNameById_{0}", id)
            result = CType(CacheUtils.GetCache(key), String)
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If

            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "SELECT BrandName FROM StoreBrand WHERE BrandId=" & id
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result

        End Function

        Public Shared Function GetByURLCode(ByVal urlCode As String) As StoreBrandRow
            If urlCode = "" Then
                Return New StoreBrandRow
            End If
            Dim result As New StoreBrandRow
            Dim key As String = String.Format(cachePrefixKey & "GetByURLCode_{0}", urlCode)
            result = CType(CacheUtils.GetCache(key), StoreBrandRow)
            If Not result Is Nothing Then
                Return result
            Else
                result = New StoreBrandRow
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select * from StoreBrand where URLCode='" & urlCode & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = LoadByDataReader(reader)
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Shared Function GetIdByURLCode(ByVal urlCode As String) As Integer
            If urlCode = "" Then
                Return Nothing
            End If

            Dim result As Integer = 0
            Dim key As String = String.Format(cachePrefixKey & "GetIdByURLCode_{0}", urlCode)
            result = CType(CacheUtils.GetCache(key), Integer)
            If result > 1 Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select BrandId from StoreBrand where URLCode='" & urlCode & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = CInt(reader.GetValue(0))
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Shared Function GetAllStoreBrands(ByVal DB1 As Database) As DataSet
            Dim result As New DataSet
            Dim key As String = cachePrefixKey & "GetAllStoreBrands"
            result = CType(CacheUtils.GetCache(key), DataSet)
            If Not result Is Nothing Then
                Return result
            Else
                result = New DataSet
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_StoreBrand_GetAllStoreBrand"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            result = db.ExecuteDataSet(cmd)
            CacheUtils.SetCache(key, result)
            Return result
        End Function
        Public Shared Function Insert(ByVal objData As StoreBrandRow) As Integer
            Try
                Dim BrandId As Integer
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreBrand_Insert")
                db.AddOutParameter(cmd, "BrandId", DbType.Int32, 16)
                db.AddInParameter(cmd, "BrandName", DbType.String, objData.BrandName)
                db.AddInParameter(cmd, "Description", DbType.String, objData.Description)
                db.AddInParameter(cmd, "HeaderImage", DbType.String, objData.HeaderImage)
                db.AddInParameter(cmd, "BrandNameUrl", DbType.String, objData.BrandNameUrl)
                db.AddInParameter(cmd, "IsTop", DbType.Boolean, objData.IsTop)
                db.ExecuteNonQuery(cmd)
                BrandId = Convert.ToInt32(db.GetParameterValue(cmd, "BrandId"))
                If (BrandId > 0) Then
                    CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                End If
                Return BrandId
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function Update(ByVal objData As StoreBrandRow) As Integer
            Try
                Dim result As Integer
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreBrand_Update")
                db.AddInParameter(cmd, "BrandId", DbType.Int32, objData.BrandId)
                db.AddInParameter(cmd, "BrandName", DbType.String, objData.BrandName)
                db.AddInParameter(cmd, "Description", DbType.String, objData.Description)
                db.AddInParameter(cmd, "HeaderImage", DbType.String, objData.HeaderImage)
                db.AddInParameter(cmd, "BrandNameUrl", DbType.String, objData.BrandNameUrl)
                db.AddInParameter(cmd, "IsTop", DbType.Boolean, objData.IsTop)
                result = db.ExecuteNonQuery(cmd)
                If result = 1 Then
                    CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                End If
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function

    End Class

    Public MustInherit Class StoreBrandRowBase
        Private m_DB As Database
        Private m_BrandId As Integer = Nothing
        Private m_BrandName As String = Nothing
        Private m_BrandNameUrl As String = Nothing
        Private m_Description As String = Nothing
        Private m_HeaderImage As String = Nothing
        Private m_URLCode As String = Nothing
        Private m_IsTop As Boolean = False
        Public Shared cachePrefixKey As String = "StoreBrand_"

        Public Property BrandId() As Integer
            Get
                Return m_BrandId
            End Get
            Set(ByVal Value As Integer)
                m_BrandId = Value
            End Set
        End Property

        Public Property BrandName() As String
            Get
                Return m_BrandName
            End Get
            Set(ByVal Value As String)
                m_BrandName = Value
            End Set
        End Property

        Public Property BrandNameUrl() As String
            Get
                Return m_BrandNameUrl
            End Get
            Set(ByVal Value As String)
                m_BrandNameUrl = Value
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
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
            End Set
        End Property

        Public Property HeaderImage() As String
            Get
                Return m_HeaderImage
            End Get
            Set(ByVal Value As String)
                m_HeaderImage = Value
            End Set
        End Property

        Public Property IsTop() As Boolean
            Get
                Return m_IsTop
            End Get
            Set(ByVal Value As Boolean)
                m_IsTop = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal BrandId As Integer)
            m_BrandId = BrandId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal BrandName As String)
            m_DB = DB
            m_BrandName = BrandName
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 01, 2009
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_StoreBrand_GetObjectByID"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "BrandId", DbType.Int32, BrandId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("BrandId"))) Then
                    m_BrandId = Convert.ToInt32(reader("BrandId"))
                Else
                    m_BrandId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("BrandName"))) Then
                    m_BrandName = reader("BrandName").ToString()
                Else
                    m_BrandName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                    m_URLCode = reader("URLCode").ToString()
                Else
                    m_URLCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    m_Description = reader("Description").ToString()
                Else
                    m_Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("HeaderImage"))) Then
                    m_HeaderImage = reader("HeaderImage").ToString()
                Else
                    m_HeaderImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("BrandNameUrl"))) Then
                    m_BrandNameUrl = reader("BrandNameUrl").ToString()
                Else
                    m_BrandNameUrl = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsTop"))) Then
                    m_IsTop = CBool(reader("IsTop"))
                Else
                    m_IsTop = False
                End If
            Catch ex As Exception
                Throw ex
                ''   Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub 'Load

        Public Function InsertItemEnable(ByVal BrandId As Integer, ByVal MemberId As Integer) As Integer
            Dim Id As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_StoreItemEnable_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 16)
            db.AddInParameter(cmd, "BrandId", DbType.Int32, BrandId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)


            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey, SalesCategoryItemRow.cachePrefixKey)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            '------------------------------------------------------------------------
            Return Id
        End Function

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 01, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_StoreBrand_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "BrandId", DbType.Int32, BrandId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreBrandCollection
        Inherits GenericCollection(Of StoreBrandRow)
        Public ReadOnly Property Clone() As StoreBrandCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New StoreBrandCollection
                For Each obj In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace
