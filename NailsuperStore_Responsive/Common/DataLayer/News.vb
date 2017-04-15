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
Imports Utility
Imports System.Text.RegularExpressions
Imports Database

Namespace DataLayer
    Public Class NewsRow
        Inherits NewsRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal NewsId As Integer)
            MyBase.New(DB, NewsId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal NewsId As Integer) As NewsRow
            'Get cache
            Dim row As NewsRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", NewsId)
            row = CType(CacheUtils.GetCache(key), NewsRow)
            If Not row Is Nothing Then
                Return row
            End If

            'Get db
            row = New NewsRow(DB, NewsId)
            row.Load()
            CacheUtils.SetCache(key, row)
            Return row
        End Function


        Public Sub RemoveAllNewsCategory(ByVal _Database As Database)
            Dim result As Integer = 0
            Dim sp As String = "sp_NewsCategory_Delete"
            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, NewsId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()

                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "RemoveAllNewsCategory", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Public Sub InsertNewsCategory(ByVal _Database As Database, ByVal sids As String, ByVal NewsId As Integer)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertListNewsCategory(_Database, ids(i), NewsId)
            Next
        End Sub
        Private Sub InsertListNewsCategory(ByVal _Database As Database, ByVal c As Integer, ByVal NewsId As Integer)
            'DB.ExecuteSQL("insert into VideoCategory (VideoId, CategoryId) values (" & VideoId & "," & c & ")")
            Dim result As Integer = 0
            Dim sp As String = "sp_NewsCategory_Insert"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, NewsId))
            cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, c))
            cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        End Sub
        Public Shared Function ListNewsForSitemap() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_NEWS_LISTNEWSFORSITEMAP As String = "sp_News_ListNewsForSitemap"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_NEWS_LISTNEWSFORSITEMAP)
            Return db.ExecuteDataSet(cmd)
        End Function

        Public Shared Function ListTop3News() As List(Of NewsRow)
            Dim result As New List(Of NewsRow)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_News_Top3News"
            Dim r As SqlDataReader

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of NewsRow)(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "ListTop3News", "Exception: " & ex.ToString())
            End Try
            
            Return result
        End Function

        Public Shared Function GetTopByCategoryId(ByVal CategoryId As Integer) As NewsRow
            'Get cache
            Dim ss As New NewsRow
            Dim keyData As String = cachePrefixKey & "GetTopByCategoryId_{0}" & CategoryId
            ss = CType(CacheUtils.GetCache(keyData), NewsRow)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New NewsRow
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_News_GetTopByCategoryId"
            Dim dr As SqlDataReader = Nothing

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, CategoryId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    ss = mapList(Of NewsRow)(dr)(0)
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetTopByCategoryId", "Exception: " & ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function GetListCategoryIdByNewsId(ByVal DB As Database, ByVal NewsId As Integer) As String
            Dim SQL As String = "Select CategoryId from NewsCategory where NewsId=" & DB.Quote(NewsId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing

            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim CategoryId As String = String.Empty
                    While r.Read()
                        CategoryId = r.Item("CategoryId")
                        result = result & "," & CategoryId
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "GetListCategoryIdByNewsId", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function
        Public Shared Function SetXMLtag(ByVal colName As String, ByVal Value As String, ByVal cData As Boolean)
            Return vbCrLf & "<" & colName & ">" & IIf(cData, CheckCDATA(Value), Value) & "</" & colName & ">"
        End Function

        Private Shared Function CheckCDATA(ByVal strValue As String) As String
            Dim pattern As String = "[^a-zA-Z0-9]"
            If (Regex.IsMatch(strValue, pattern)) Then
                Return "<![CDATA[" & strValue & "]]>"
            End If
            Return strValue
        End Function
    End Class

    Public MustInherit Class NewsRowBase
        Private m_DB As Database
        Private m_NewsId As Integer = Nothing
        Private m_CategoryId As Integer = Nothing
        Private m_ThumbImage As String = Nothing
        Private m_Arrange As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_Title As String = Nothing
        Private m_ShortDescription As String = Nothing
        Private m_Description As String = Nothing
        Private m_CreatedDate As Date = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Private m_IsFacebook As Boolean = False

        Private m_ListCategoryId As String = String.Empty

        Public Shared cachePrefixKey As String = "News_"

        Public Property ListCategoryId() As String
            Get
                Return m_ListCategoryId
            End Get
            Set(ByVal value As String)
                m_ListCategoryId = value
            End Set
        End Property
        Public Property NewsId() As Integer
            Get
                Return m_NewsId
            End Get
            Set(ByVal Value As Integer)
                m_NewsId = Value
            End Set
        End Property
        Public Property CategoryId() As Integer
            Get
                Return m_CategoryId
            End Get
            Set(ByVal Value As Integer)
                m_CategoryId = Value
            End Set
        End Property
        Public Property ThumbImage() As String
            Get
                Return m_ThumbImage
            End Get
            Set(ByVal Value As String)
                m_ThumbImage = Value
            End Set
        End Property

        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal Value As Integer)
                m_Arrange = Value
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
        Public Property MetaKeyword() As String
            Get
                Return m_MetaKeyword
            End Get
            Set(ByVal Value As String)
                m_MetaKeyword = Value
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
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = Value
            End Set
        End Property
        Public Property ShortDescription() As String
            Get
                Return m_ShortDescription
            End Get
            Set(ByVal Value As String)
                m_ShortDescription = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As Date)
                m_CreatedDate = Value
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
        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As Integer)
                m_TotalRow = Value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal Value As Integer)
                m_PageIndex = Value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal Value As Integer)
                m_PageSize = Value
            End Set
        End Property

        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal Value As String)
                m_OrderBy = Value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal Value As String)
                m_OrderDirection = Value
            End Set
        End Property
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
            End Set
        End Property

        Public Property IsFacebook() As Boolean
            Get
                Return m_IsFacebook
            End Get
            Set(ByVal Value As Boolean)
                m_IsFacebook = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal NewsId As Integer)
            m_DB = database
            m_NewsId = NewsId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT [dbo].[fc_Category_ReturnCateId](NewsId,'News') as CategoryId, *  FROM News WHERE NewsId = " & DB.Number(NewsId)
                r = m_DB.GetReader(SQL)

                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Load", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Public Shared Function ListByCategoryId(ByVal data As NewsRow) As NewsCollection
            'Get cache
            Dim ss As New NewsCollection
            Dim keyData As String = String.Format(cachePrefixKey & "ListByCategoryId_{0}_{1}_{2}_{3}_{4}", data.Condition, data.OrderBy, data.OrderDirection, data.PageIndex, data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "ListByCategoryId_Total_" & data.Condition
            ss = CType(CacheUtils.GetCache(keyData), NewsCollection)
            'If Not ss Is Nothing Then
            '    ''get Total
            '    data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
            '    Return ss
            'Else
            ss = New NewsCollection
            'End If

            'Get db
            Dim sp As String = "sp_News_ListByCategoryId"
            Dim dr As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, data.Condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, data.OrderBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, data.OrderDirection)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataListByCategoryFromReader(dr))
                    End While
                    Core.CloseReader(dr)
                    data.TotalRow = Convert.ToInt32(cmd.Parameters("@TotalRecords").Value)
                    CacheUtils.SetCache(keyData, ss)
                    CacheUtils.SetCache(keyTotal, data.TotalRow)
                Else
                    Core.CloseReader(dr)
                End If

            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListByCategoryId", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
            Return ss

        End Function
        Public Shared Function ListSummary(ByVal data As NewsRow) As NewsCollection
            Dim ss As New NewsCollection
            Dim keyData As String = String.Format(cachePrefixKey & "ListSummary_{0}_{1}", data.PageIndex, data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "ListSummary_Total"
            ss = CType(CacheUtils.GetCache(keyData), NewsCollection)
            If Not ss Is Nothing Then
                ''get Total
                data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New NewsCollection
            End If

            'Get db
            Dim sp As String = "sp_News_ListSummary"
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataListByCategoryFromReader(dr))
                    End While
                    data.TotalRow = Convert.ToInt32(cmd.Parameters("@TotalRecords").Value)
                    CacheUtils.SetCache(keyData, ss)
                    CacheUtils.SetCache(keyTotal, data.TotalRow)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListSummary", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            Return ss
        End Function
        Public Shared Function ListByCategoryYearMonth(ByVal CatId As Integer, ByVal iYear As Integer, ByVal iMonth As Integer) As NewsCollection
            Dim ss As New NewsCollection
            Dim dr As SqlDataReader = Nothing
            Dim sp As String = "sp_News_ListByCategoryIdYearMonth"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, CatId)
                db.AddInParameter(cmd, "Year", DbType.Int32, iYear)
                db.AddInParameter(cmd, "Month", DbType.Int32, iMonth)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataListByCategoryFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListByCategoryYearMonth", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            Return ss
        End Function
        Private Shared Function GetDataListByCategoryFromReader(ByVal reader As SqlDataReader) As NewsRow
            Dim result As New NewsRow
            If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                result.NewsId = Convert.ToInt32(reader("NewsId"))
            Else
                result.NewsId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                result.ThumbImage = reader("ThumbImage").ToString()
            Else
                result.ThumbImage = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title").ToString()
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                result.ShortDescription = reader("ShortDescription").ToString()
            Else
                result.ShortDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                result.CreatedDate = Convert.ToDateTime(reader.Item("CreatedDate"))
            Else
                result.CreatedDate = Nothing
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                result.CategoryId = Convert.ToInt32(reader("CategoryId"))
            Else
                result.CategoryId = 0
            End If
            Return result
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As NewsRow
            Dim result As New NewsRow
            If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                result.NewsId = Convert.ToInt32(reader("NewsId"))
            Else
                result.NewsId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                result.CategoryId = Convert.ToInt32(reader("CategoryId"))
            Else
                result.CategoryId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                result.ThumbImage = reader("ThumbImage").ToString()
            Else
                result.ThumbImage = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = Convert.ToBoolean(reader("IsActive"))
            Else
                result.IsActive = True
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                result.Arrange = Convert.ToInt32(reader("Arrange"))
            Else
                result.Arrange = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                result.MetaDescription = reader("MetaDescription").ToString()
            Else
                result.MetaDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                result.MetaKeyword = reader("MetaKeyword").ToString()
            Else
                result.MetaKeyword = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                result.PageTitle = reader("PageTitle").ToString()
            Else
                result.PageTitle = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title").ToString()
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                result.ShortDescription = reader("ShortDescription").ToString()
            Else
                result.ShortDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                result.Description = reader("Description").ToString()
            Else
                result.Description = ""
            End If
            Return result
        End Function
        Public Shared Function Delete(ByVal _Database As Database, ByVal NewsId As Integer) As Boolean

            Dim sp As String = "sp_News_Delete"
            Dim result As Integer = 0
            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, NewsId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Delete", "Exception: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsDocumentRow.cachePrefixKey, NewsImageRow.cachePrefixKey)
            Return result = 1
        End Function


        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal NewsId As Integer) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_News_ChangeIsActive"

            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, NewsId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Delete", "ChangeIsActive: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsDocumentRow.cachePrefixKey, NewsImageRow.cachePrefixKey)

            Return result = 1
        End Function

        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal NewsId As Integer, ByVal CategoryId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_News_ChangeArrange"
            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, NewsId))
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, CategoryId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Delete", "ChangeChangeArrange: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Return result = 1
        End Function
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                    m_NewsId = Convert.ToInt32(reader("NewsId"))
                Else
                    m_NewsId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                    m_ThumbImage = reader("ThumbImage").ToString()
                Else
                    m_ThumbImage = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = True
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                    m_MetaKeyword = reader("MetaKeyword").ToString()
                Else
                    m_MetaKeyword = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                    m_PageTitle = reader("PageTitle").ToString()
                Else
                    m_PageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                    m_Title = reader("Title").ToString()
                Else
                    m_Title = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                    m_ShortDescription = reader("ShortDescription").ToString()
                Else
                    m_ShortDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    m_Description = reader("Description").ToString()
                Else
                    m_Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                    m_CreatedDate = reader("CreatedDate").ToString()
                Else
                    m_CreatedDate = DateTime.MinValue
                End If
            End If
        End Sub
        Public Overridable Function Insert() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_News_INSERT As String = "sp_News_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_News_INSERT)

            db.AddOutParameter(cmd, "NewsId", DbType.Int32, 32)
            db.AddInParameter(cmd, "ThumbImage", DbType.String, ThumbImage)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "ShortDescription", DbType.String, ShortDescription)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cmd, "MetaKeyword", DbType.String, MetaKeyword)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)

            db.ExecuteNonQuery(cmd)

            NewsId = Convert.ToInt32(db.GetParameterValue(cmd, "NewsId"))
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------
            Return NewsId
        End Function
        Public Overridable Function Update() As Boolean
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_News_UPDATE As String = "sp_News_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_News_UPDATE)
            db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
            db.AddInParameter(cmd, "ThumbImage", DbType.String, ThumbImage)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "ShortDescription", DbType.String, ShortDescription)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cmd, "MetaKeyword", DbType.String, MetaKeyword)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cmd, "IsFacebook", DbType.Boolean, IsFacebook)
            Dim result As Integer = db.ExecuteNonQuery(cmd)

            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsDocumentRow.cachePrefixKey, NewsImageRow.cachePrefixKey)
            If result = 1 Then
                Return True
            End If
            Return False
            '------------------------------------------------------------------------

        End Function 'Update
        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_News_Delete")
            db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
            db.ExecuteNonQuery(cmd)
            ''------------------------------------------------------------------------
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsDocumentRow.cachePrefixKey, NewsImageRow.cachePrefixKey)
        End Sub 'Remove


    End Class
    Public Class NewsCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal News As NewsRow)
            Me.List.Add(News)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As NewsRow
            Get
                Return CType(Me.List.Item(Index), NewsRow)
            End Get

            Set(ByVal Value As NewsRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As NewsCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New NewsCollection
                For Each obj As NewsRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace
