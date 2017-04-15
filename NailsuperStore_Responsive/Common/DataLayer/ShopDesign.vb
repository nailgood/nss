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
Imports System.Text.RegularExpressions
Namespace DataLayer
    Public Class ShopDesignRow
        Inherits ShopDesignRowBase
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub
        Public Sub New(ByVal DB As Database, ByVal ShopDesignId As Integer)
            MyBase.New(DB, ShopDesignId)
        End Sub

        Public Shared Function GetRow(ByVal DB As Database, ByVal ShopDesignId As Integer) As ShopDesignRow
            Dim row As ShopDesignRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", ShopDesignId)
            row = CType(CacheUtils.GetCache(key), ShopDesignRow)
            If Not row Is Nothing Then
                Return row
            End If
            row = New ShopDesignRow(DB, ShopDesignId)
            row.Load()
            CacheUtils.SetCache(key, row)
            Return row
        End Function

        Public Sub InsertShopDesignCategory(ByVal lstIdCate As String, ByVal ShopDesignId As Integer)
            Dim ids As String() = lstIdCate.Split(",")
            For i As Integer = 0 To ids.Count - 1
                If IsNumeric(ids(i)) Then
                    InsertListShopDesignCategory(ids(i), ShopDesignId)
                End If
            Next
        End Sub

        Private Sub InsertListShopDesignCategory(ByVal idcate As Integer, ByVal ShopDesignId As Integer)
            Dim result As Integer = 0
            Dim sp As String = "sp_ShopDesign_InsertListShopDesignCategory"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.AddInParameter(cmd, "CategoryId", DbType.Int32, idcate)
            db.ExecuteNonQuery(cmd)
        End Sub

        Public Sub RemoveAllCategory()
            Dim sp As String = "sp_ShopDesign_RemoveAllCategory"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.ExecuteNonQuery(cmd)
        End Sub

        Public Shared Function CountItem(ByVal DB As Database, ByVal ShopDesignId As Integer) As String
            Dim result As String = "0"
            If ShopDesignId < 0 Then
                Return result
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT COUNT(ItemId) as Count FROM ShopDesignItem WHERE ShopDesignId=" & ShopDesignId
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

        Public Shared Function CountMedia(ByVal DB As Database, ByVal ShopDesignId As Integer, ByVal Type As Integer) As String
            Dim result As String = "0"
            If ShopDesignId < 0 Then
                Return result
            End If

            Dim r As SqlDataReader = Nothing
            Dim SQL As String = "SELECT COUNT(Id) as Count FROM ShopDesignMedia WHERE ShopDesignId=" & ShopDesignId & " and Type=" & Type
            Try
                r = DB.GetReader(SQL)
                While r.Read()
                    result = r.Item("Count")
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "CountMedia", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function ListTop3ByCategoryID(ByVal cateID As Integer) As List(Of ShopDesignRow)
            'Get cache
            Dim key As String = String.Format(cachePrefixKey & "ListTop3ByCategoryID_{0}", cateID)
            Dim result As List(Of ShopDesignRow) = CType(CacheUtils.GetCache(key), List(Of ShopDesignRow))
            If Not result Is Nothing Then
                Return result
            End If

            'Get db
            Dim r As SqlDataReader = Nothing
            Dim sp As String = "sp_ShopDesign_ListTop3ByCategoryID"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, cateID)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of ShopDesignRow)(r)
                    CacheUtils.SetCache(key, result)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "ListTop3ByCategoryID", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function ListByCategoryID(ByVal data As ShopDesignRow) As List(Of ShopDesignRow)
            'Get cache
            Dim key As String = String.Format(cachePrefixKey & "ListByCategoryID_{0}_{1}_{2}", data.CategoryId, data.PageIndex, data.PageSize)
            Dim keyTotal As String = String.Format(cachePrefixKey & "ListByCategoryID_total_{0}_{1}_{2}", data.CategoryId, data.PageIndex, data.PageSize)
            Dim result As List(Of ShopDesignRow) = CType(CacheUtils.GetCache(key), List(Of ShopDesignRow))
            If Not result Is Nothing Then
                data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return result
            End If

            'Get db
            result = New List(Of ShopDesignRow)
            Dim r As SqlDataReader = Nothing
            Dim sp As String = "sp_ShopDesign_ListByCategoryID"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, data.CategoryId)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of ShopDesignRow)(r)
                    data.TotalRow = CInt(cmd.Parameters("@TotalRecords").Value)
                    CacheUtils.SetCache(keyTotal, data.TotalRow)
                    CacheUtils.SetCache(key, result)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "ListByCategoryID", "Exception: " & ex.ToString())
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

    Public MustInherit Class ShopDesignRowBase
        Private m_DB As Database
        Private m_ShopDesignId As Integer = Nothing
        Private m_Image As String = Nothing
        Private m_Title As String = Nothing
        Private m_ShortDescription As String = Nothing
        Private m_Description As String = Nothing
        Private m_Instruction As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_CreatedDate As Date = Nothing
        Private m_ModifiedDate As Date = Nothing
        Private m_IsActive As Boolean = True
        Private m_CategoryId As Integer = Nothing
        Private m_CategoryName As String = Nothing
        Private m_ParentId As Integer = Nothing

        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing

        Private m_SortOrder As Integer = Nothing
        Public ItemIndex As Integer = 0
        Public Shared cachePrefixKey As String = "ShopDesign_"

        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property
        Public Property ShopDesignId() As Integer
            Get
                Return m_ShopDesignId
            End Get
            Set(ByVal Value As Integer)
                m_ShopDesignId = Value
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
        Public Property Instruction() As String
            Get
                Return m_Instruction
            End Get
            Set(ByVal Value As String)
                m_Instruction = Value
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
        Public Property MetaKeyword() As String
            Get
                Return m_MetaKeyword
            End Get
            Set(ByVal Value As String)
                m_MetaKeyword = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As Date)
                m_CreatedDate = Value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal Value As Date)
                m_ModifiedDate = Value
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
        Public Property CategoryId() As Integer
            Get
                Return m_CategoryId
            End Get
            Set(ByVal Value As Integer)
                m_CategoryId = Value
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
        Public Property ParentId() As Integer
            Get
                Return m_ParentId
            End Get
            Set(ByVal Value As Integer)
                m_ParentId = Value
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
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
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

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = Value
            End Set
        End Property
        Public Sub New()
        End Sub
        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub
        Public Sub New(ByVal database As Database, ByVal ShopDesignId As Integer)
            m_DB = database
            m_ShopDesignId = ShopDesignId
        End Sub

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM ShopDesign WHERE ShopDesignId = " & DB.Number(ShopDesignId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId"))) Then
                        m_ShopDesignId = Convert.ToInt32(reader("ShopDesignId"))
                    Else
                        m_ShopDesignId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        m_Title = reader("Title")
                    Else
                        m_Title = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        m_Image = reader("Image")
                    Else
                        m_Image = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                        m_ShortDescription = reader("ShortDescription")
                    Else
                        m_ShortDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Instruction"))) Then
                        m_Instruction = reader("Instruction").ToString()
                    Else
                        m_Instruction = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                    Else
                        m_CreatedDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                        m_modifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                    Else
                        m_modifiedDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        m_Description = reader("Description").ToString()
                    Else
                        m_Description = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                        m_MetaKeyword = reader("MetaKeyword").ToString()
                    Else
                        m_MetaKeyword = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        m_MetaDescription = reader("MetaDescription").ToString()
                    Else
                        m_MetaDescription = ""
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Function ListAllAdmin(ByVal condition As String, ByVal categoryId As Integer) As List(Of ShopDesignRow)
            'Get cache
            Dim result As New List(Of ShopDesignRow)
            Dim dr As SqlDataReader = Nothing
            Dim key As String = String.Format(cachePrefixKey & "ListAllAdmin_{0}_{1}", condition, categoryId)
            result = CType(CacheUtils.GetCache(key), List(Of ShopDesignRow))
            If Not result Is Nothing Then
                Return result
            Else
                result = New List(Of ShopDesignRow)
            End If

            'Get db
            Dim sp As String = "sp_ShopDesign_ListAllAdmin"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, categoryId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of ShopDesignRow)(dr)
                    CacheUtils.SetCache(key, result)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListAllAdmin", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Overridable Function Insert() As Integer
            Dim sp As String = "sp_ShopDesign_Insert"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim cmd As SqlCommand = DB.GetStoredProcCommand(sp)
                DB.AddOutParameter(cmd, "ShopDesignId", DbType.Int32, 32)
                DB.AddInParameter(cmd, "Title", DbType.String, Title)
                DB.AddInParameter(cmd, "Image", DbType.String, Image)
                DB.AddInParameter(cmd, "ShortDescription", DbType.String, ShortDescription)
                DB.AddInParameter(cmd, "Instruction", DbType.String, Instruction)
                DB.AddInParameter(cmd, "Description", DbType.String, Description)
                DB.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
                DB.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
                DB.AddInParameter(cmd, "MetaKeyword", DbType.String, MetaKeyword)
                DB.AddInParameter(cmd, "CreatedDate", DbType.DateTime, CreatedDate)
                DB.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, ModifiedDate)
                DB.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                DB.ExecuteNonQuery(cmd)

                ShopDesignId = Convert.ToInt32(DB.GetParameterValue(cmd, "ShopDesignId"))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Insert", "Exception: " & ex.ToString())
            End Try

            Return ShopDesignId
        End Function

        Public Overridable Function Update() As Boolean
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_ShopDesign_Update"
            Dim result As Integer = 0

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "Title", DbType.String, Title)
                db.AddInParameter(cmd, "Image", DbType.String, Image)
                db.AddInParameter(cmd, "ShortDescription", DbType.String, ShortDescription)
                db.AddInParameter(cmd, "Instruction", DbType.String, Instruction)
                db.AddInParameter(cmd, "Description", DbType.String, Description)
                db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
                db.AddInParameter(cmd, "MetaKeyword", DbType.String, MetaKeyword)
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, ModifiedDate)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                result = db.ExecuteNonQuery(cmd)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Update", "Exception: " & ex.ToString())
            End Try

            Return result > 0
        End Function

        Public Shared Function ChangeActive(ByVal ShopDesignId As Integer) As Boolean
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_ShopDesign_ChangeActive"

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "ChangeActive", "Exception: " & ex.ToString())
            End Try

            Return result > 0
        End Function

        Public Shared Function Delete(ByVal ShopDesignId As Integer) As Boolean
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_ShopDesign_Delete"

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "ChangeActive", "Exception: " & ex.ToString())
            End Try

            Return result > 0
        End Function

        Public Shared Function ChangeSortOrder(ByVal ShopDesignId As Integer, ByVal cateId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopDesign_ChangeSortOrder"
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "CategoryId", DbType.Int32, cateId)
                db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)
                result = CInt(db.ExecuteNonQuery(cmd))
            Catch ex As Exception

            End Try
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function GetDataListFromReader(ByVal reader As SqlDataReader) As ShopDesignRow
            Dim result As New ShopDesignRow
            If (Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId"))) Then
                result.ShopDesignId = Convert.ToInt32(reader("ShopDesignId"))
            Else
                result.ShopDesignId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title")
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = reader("IsActive")
            Else
                result.IsActive = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                result.Image = reader("Image")
            Else
                result.Image = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                result.ShortDescription = reader("ShortDescription")
            Else
                result.ShortDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                result.CategoryId = CInt(reader("CategoryId"))
            Else
                result.CategoryId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ParentId"))) Then
                result.ParentId = CInt(reader("ParentId"))
            Else
                result.ParentId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryName"))) Then
                result.CategoryName = reader("CategoryName")
            Else
                result.CategoryName = ""
            End If
            Return result
        End Function


        Public Shared Function GetListFromReader(ByVal reader As SqlDataReader) As ShopDesignRow
            Dim result As New ShopDesignRow
            If (Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId"))) Then
                result.ShopDesignId = Convert.ToInt32(reader("ShopDesignId"))
            Else
                result.ShopDesignId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title")
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = reader("IsActive")
            Else
                result.IsActive = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                result.Image = reader("Image")
            Else
                result.Image = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                result.ShortDescription = reader("ShortDescription")
            Else
                result.ShortDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                result.ModifiedDate = reader("ModifiedDate")
            Else
                result.ModifiedDate = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                result.SortOrder = CInt(reader("SortOrder"))
            Else
                result.SortOrder = 0
            End If
            Return result
        End Function
    End Class

    Public Class ShopDesignCollection
        Inherits CollectionBase
        Public Sub New()
        End Sub

        Public Sub Add(ByVal shopdesign As ShopDesignRow)
            Me.List.Add(shopdesign)
        End Sub

        Default Public Property Item(ByVal index As String) As ShopDesignRow
            Get
                Return CType(Me.List.Item(index), ShopDesignRow)
            End Get
            Set(ByVal value As ShopDesignRow)
                Me.List(index) = value
            End Set
        End Property
    End Class
End Namespace