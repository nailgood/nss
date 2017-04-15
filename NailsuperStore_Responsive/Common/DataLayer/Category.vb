
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
Imports Database
Namespace DataLayer
    Public Class CategoryRow
        Inherits CategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal CategoryId As Integer)
            MyBase.New(database, CategoryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal CategoryId As Integer) As CategoryRow
            If CategoryId <= 0 Then
                Return Nothing
            End If

            'Get cache
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", CategoryId)
            Dim row As CategoryRow
            row = CType(CacheUtils.GetCache(key), CategoryRow)
            If Not row Is Nothing Then
                Return CloneObject.Clone(row)
            End If

            'Get db
            row = New CategoryRow(_Database, CategoryId)
            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Try
                SQL = "SELECT * FROM Category WHERE CategoryId = " & _Database.Number(CategoryId)
                dr = _Database.GetReader(SQL)

                If dr.HasRows Then
                    row = mapList(Of CategoryRow)(dr).Item(0)
                    CacheUtils.SetCache(key, row)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Dim rawURL As String = String.Empty
                If Not System.Web.HttpContext.Current Is Nothing Then
                    If Not System.Web.HttpContext.Current.Request Is Nothing Then
                        rawURL = System.Web.HttpContext.Current.Request.RawUrl
                    End If
                End If
                Email.SendError("ToError500", "Category.vb >> GetRow", "SQL: " & SQL & "<br/>Page: " & rawURL & "<br><br>Function: Protected Overridable Sub Load()" & "<br><br>Exception: " & ex.ToString())
            End Try

            Return row
        End Function

        Public Shared Function ListAllParent(ByVal condition As String) As CategoryCollection
            'Get cache
            Dim ss As New CategoryCollection
            Dim key As String = String.Format(cachePrefixKey & "ListAllParent_{0}", condition)
            ss = CType(CacheUtils.GetCache(key), CategoryCollection)
            If Not ss Is Nothing Then
                Return ss.Clone()
            Else
                ss = New CategoryCollection
            End If

            'Get db
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Category_ListAllParent"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)

                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListAllParent", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            CacheUtils.SetCache(key, ss)
            Return ss
        End Function

        Public Shared Function ListByItem(ByVal condition As String) As List(Of CategoryRow)
            Dim result As New List(Of CategoryRow)
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase
                Dim sp As String = "sp_Category_ListByItem"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of CategoryRow)(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Category.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return result
        End Function

        Public Shared Function ListByType(ByVal _Database As Database, ByVal type As Integer) As CategoryCollection
            Dim ss As New CategoryCollection
            Dim key As String = cachePrefixKey & "ListByType_" & type
            ss = CType(CacheUtils.GetCache(key), CategoryCollection)
            If Not ss Is Nothing Then
                Return ss.Clone()
            Else
                ss = New CategoryCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Category_ListByType"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, type))
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Category.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

        Public Shared Function GetAllVideoCategoryByType(ByVal _Database As Database, ByVal Type As Utility.Common.CategoryType) As CategoryCollection
            Dim ss As New CategoryCollection
            Dim key As String = cachePrefixKey & "GetAllVideoCategoryByType_" & Type
            ss = CType(CacheUtils.GetCache(key), CategoryCollection)
            If Not ss Is Nothing Then
                Return ss.Clone()
            Else
                ss = New CategoryCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_VideoCategory_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Type", DbType.Int32, Type)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Category.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

        Public Shared Function GetCategoryById(ByVal _Database As Database, ByVal Id As Integer, ByVal Type As Integer) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim Con As String = ""
            If Type = Utility.Common.CategoryType.News Then
                Con = "[dbo].[fc_NewsCategory_IsChecked](" & Id & " , CategoryId) as IsChecked"
            ElseIf Type = Utility.Common.CategoryType.ShopDesign Then
                Con = "[dbo].[fc_ShopDesignCategory_IsChecked](" & Id & " , CategoryId) as IsChecked"
            Else
                Con = "[dbo].[fc_VideoCategory_IsChecked](" & Id & " , CategoryId) as IsChecked"
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Category_GetById"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, Type))
            cmd.Parameters.Add(_Database.InParam("Condition", SqlDbType.VarChar, 0, Con))
            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetCategoryNameByCategoryId(ByVal Id As Integer) As String
            If Id < 1 Then
                Return Nothing
            End If
            Dim result As String = ""
            Dim key As String = cachePrefixKey & "GetCategoryNameByCategoryId_" & Id
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim sql As String = "Select CategoryName from Category where CategoryId=" & Id & ""
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

        Public Shared Function CountItemShopDesign(ByVal DB As Database, ByVal CategoryId As Integer) As String
            Dim result As String = "0"
            If CategoryId < 1 Then
                Return result
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT COUNT(ShopDesignId) as Count FROM ShopDesignCategory WHERE CategoryId =" & CategoryId
                r = DB.GetReader(SQL)
                While r.Read()
                    result = r.Item("Count")
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return result
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As CategoryRow
            Dim result As New CategoryRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                    result.CategoryId = Convert.ToInt32(reader("CategoryId"))
                Else
                    result.CategoryId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                    result.ParentId = Convert.ToInt32(reader("ParentId"))
                Else
                    result.ParentId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CategoryName"))) Then
                    result.CategoryName = reader("CategoryName").ToString()
                Else
                    result.CategoryName = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                    result.Type = Convert.ToInt32(reader("Type"))
                Else
                    result.Type = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Banner"))) Then
                    result.Banner = reader("Banner").ToString()
                Else
                    result.Banner = ""
                End If
                Return result
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Shared Function FullByCategoryId(ByVal cateId As Integer) As String
            Dim key As String = String.Format(cachePrefixKey & "FullByCategoryId_{0}", cateId)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If Not result Is Nothing Then
                Return result
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_Category_FullByCategoryId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, cateId)
                result = db.ExecuteScalar(cmd)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function FullByShopDesignId(ByVal ShopDesignId As Integer) As String
            Dim key As String = String.Format(cachePrefixKey & "FullByShopDesignId_{0}", ShopDesignId)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If Not result Is Nothing Then
                Return result
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_Category_FullByShopDesignId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                result = db.ExecuteScalar(cmd)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal CategoryId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Category_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, CategoryId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As CategoryRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Category_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryName", SqlDbType.NVarChar, 0, data.CategoryName))
                cmd.Parameters.Add(_Database.InParam("ParentId", SqlDbType.Int, 0, data.ParentId))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.NVarChar, 0, data.MetaKeyword))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, data.Type))
                cmd.Parameters.Add(_Database.InParam("Banner", SqlDbType.VarChar, 0, data.Banner))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                data.CategoryId = result
            Catch ex As Exception

            End Try
            If result > 0 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Update(ByVal _Database As Database, ByVal data As CategoryRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Category_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.NVarChar, 0, data.CategoryId))
                cmd.Parameters.Add(_Database.InParam("ParentId", SqlDbType.NVarChar, 0, data.ParentId))
                cmd.Parameters.Add(_Database.InParam("CategoryName", SqlDbType.NVarChar, 0, data.CategoryName))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.NVarChar, 0, data.MetaKeyword))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, data.Type))
                cmd.Parameters.Add(_Database.InParam("Banner", SqlDbType.VarChar, 0, data.Banner))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, VideoRow.cachePrefixKey, NewsRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal CategoryId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Category_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, CategoryId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, VideoRow.cachePrefixKey, NewsRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeArrange(ByVal _Database As Database, ByVal CategoryId As Integer, ByVal ParentId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Category_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, CategoryId))
                cmd.Parameters.Add(_Database.InParam("ParentId", SqlDbType.Int, 0, ParentId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function
    End Class


    Public MustInherit Class CategoryRowBase
        Private m_DB As Database
        Private m_CategoryId As Integer = Nothing
        Private m_ParentId As Integer = Nothing
        Private m_CategoryName As String = Nothing
        Private m_Arrange As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_LinkDetail As String = Nothing
        Private m_Type As String = Nothing
        Private m_Banner As String = Nothing
        Private m_Condition As String = Nothing
        Private m_TotalRow As Integer = Nothing
        Public Shared cachePrefixKey As String = "Category_"

        Public Property CategoryId() As Integer
            Get
                Return m_CategoryId
            End Get
            Set(ByVal Value As Integer)
                m_CategoryId = Value
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
        Public Property CategoryName() As String
            Get
                Return m_CategoryName
            End Get
            Set(ByVal Value As String)
                m_CategoryName = Value
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
        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal Value As Integer)
                m_Type = Value
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
        Public Property LinkDetail() As String
            Get
                Return m_LinkDetail
            End Get
            Set(ByVal Value As String)
                m_LinkDetail = Value
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
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
            End Set
        End Property
        Public Property TotalRow() As String
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As String)
                m_TotalRow = Value
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
        Public Property Banner() As String
            Get
                Return m_Banner
            End Get
            Set(ByVal Value As String)
                m_Banner = Value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal CategoryId As Integer)
            m_DB = database
            m_CategoryId = CategoryId
        End Sub 'New

        Protected Overridable Sub Load()
            If (CategoryId < 1) Then
                Exit Sub
            End If
            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Try
                SQL = "SELECT * FROM Category WHERE CategoryId = " & DB.Number(CategoryId)
                dr = m_DB.GetReader(SQL)
                If dr IsNot Nothing AndAlso dr.Read Then
                    Me.Load(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Category.vb", "SQL: " & SQL & "<br><br>Function: Protected Overridable Sub Load()" & "<br><br>Exception: " & ex.ToString())
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                        m_CategoryId = Convert.ToInt32(reader("CategoryId"))
                    Else
                        m_CategoryId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                        m_ParentId = Convert.ToInt32(reader("ParentId"))
                    Else
                        m_ParentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CategoryName"))) Then
                        m_CategoryName = reader("CategoryName").ToString()
                    Else
                        m_CategoryName = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                        m_Arrange = Convert.ToInt32(reader("Arrange"))
                    Else
                        m_Arrange = 0
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        m_Type = Convert.ToInt32(reader("Type"))
                    Else
                        m_Type = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Banner"))) Then
                        m_Banner = reader("Banner").ToString()
                    Else
                        m_Banner = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

    End Class

    Public Class CategoryCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Category As CategoryRow)
            Me.List.Add(Category)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As CategoryRow
            Get
                Return CType(Me.List.Item(Index), CategoryRow)
            End Get

            Set(ByVal Value As CategoryRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As CategoryCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New CategoryCollection
                For Each obj In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

