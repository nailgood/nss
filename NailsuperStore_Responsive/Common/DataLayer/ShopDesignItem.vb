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
    Public Class ShopDesignItemRow
        Inherits ShopDesignItemRowBase
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal db As Database)
            MyBase.New(db)
        End Sub
        Public Sub New(ByVal db As Database, ByVal ShopDesignId As Integer, ByVal ItemId As Integer)
            MyBase.New(db, ShopDesignId, ItemId)
        End Sub

        Public Shared Function ListItemByShopDesignId(ByVal ShopDesignId As Integer) As ShopDesignItemRowCollection
            Dim key As String = String.Format(cachePrefixKey & "ListItemByShopDesignId_{0}", ShopDesignId)
            Dim result As New ShopDesignItemRowCollection
            result = CType(CacheUtils.GetCache(key), ShopDesignItemRowCollection)
            If Not result Is Nothing Then
                Return result
            Else
                result = New ShopDesignItemRowCollection
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_ShopDesignItem_ListItemByShopDesignId"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                r = db.ExecuteReader(cmd)
                While r.Read
                    result.Add(GetDataListFromReader(r))
                End While
                Core.CloseReader(r)
                CacheUtils.SetCache(key, result)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return result
        End Function

        Public Shared Function ListItemByShopDesignId2(ByVal ShopDesignId As Integer) As List(Of ShopDesignItemRow)
            Dim result As New List(Of ShopDesignItemRow)
            Dim r As SqlDataReader = Nothing
            Dim sp As String = "sp_ShopDesignItem_ListItemByShopDesignId"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try

                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of ShopDesignItemRow)(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "ListItemByShopDesignId2", ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function AdminListItemByShopDesignId(ByVal ShopDesignId As Integer) As ShopDesignItemRowCollection
            'Get cache
            Dim key As String = String.Format(cachePrefixKey & "AdminListItemByShopDesignId_{0}", ShopDesignId)
            Dim result As New ShopDesignItemRowCollection
            result = CType(CacheUtils.GetCache(key), ShopDesignItemRowCollection)
            If Not result Is Nothing Then
                Return result
            Else
                result = New ShopDesignItemRowCollection
            End If

            'Get db
            Dim r As SqlDataReader = Nothing
            Dim sp As String = "sp_ShopDesignItem_AdminListItemByShopDesignId"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    While r.Read
                        result.Add(GetDataListFromReader(r))
                    End While
                    CacheUtils.SetCache(key, result)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "AdminListItemByShopDesignId", ex.ToString())
            End Try

            Return result
        End Function

        Private Shared Function GetDataListFromReader(ByVal reader As SqlDataReader) As ShopDesignItemRow
            Dim result As New ShopDesignItemRow
            If Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId")) Then
                result.ShopDesignId = CInt(reader("ShopDesignId"))
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("ItemId")) Then
                result.ItemId = CInt(reader("ItemId"))
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("SKU")) Then
                result.SKU = reader("SKU")
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("ItemName")) Then
                result.ItemName = reader("ItemName")
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("QtyDefault")) Then
                result.QtyDefault = CInt(reader("QtyDefault"))
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("IsActive")) Then
                result.IsActive = Convert.ToBoolean(reader("IsActive"))
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("SortOrder")) Then
                result.SortOrder = CInt(reader("SortOrder"))
            End If
            Return result
        End Function


    End Class

    Public MustInherit Class ShopDesignItemRowBase
        Private m_DB As Database
        Private m_ShopDesignId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_QtyDefault As Integer = Nothing
        Private m_IsActive As Boolean = True
        Private m_SortOrder As Integer = Nothing
        Private m_SKU As String = Nothing
        Private m_ItemName As String = Nothing

        Public Shared cachePrefixKey As String = "ShopDesignItem_"
        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(ByVal value As Database)
                m_DB = value
            End Set
        End Property
        Public Property ShopDesignId() As Integer
            Get
                Return m_ShopDesignId
            End Get
            Set(ByVal value As Integer)
                m_ShopDesignId = value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal value As Integer)
                m_ItemId = value
            End Set
        End Property
        Public Property QtyDefault() As Integer
            Get
                Return m_QtyDefault
            End Get
            Set(ByVal value As Integer)
                m_QtyDefault = value
            End Set
        End Property
        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal value As Integer)
                m_SortOrder = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
            End Set
        End Property
        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal value As String)
                m_SKU = value
            End Set
        End Property
        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal value As String)
                m_ItemName = value
            End Set
        End Property

        Public Sub New()
        End Sub
        Public Sub New(ByVal db As Database)
            m_DB = db
        End Sub
        Public Sub New(ByVal db As Database, ByVal ShopDesignId As Integer, ByVal ItemId As Integer)
            m_DB = db
            m_ShopDesignId = ShopDesignId
            m_ItemId = ItemId
        End Sub

        Public Shared Function GetRow(ByVal DB As Database, ByVal ItemId As Integer, ByVal ShopDesignId As Integer) As ShopDesignItemRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}_{1}", ItemId, ShopDesignId)
            Dim result As ShopDesignItemRow = CType(CacheUtils.GetCache(key), ShopDesignItemRow)
            If Not result Is Nothing Then
                Return result
            Else
                result = New ShopDesignItemRow(DB, ShopDesignId, ItemId)
            End If
            result.Load()
            CacheUtils.SetCache(key, result)
            Return result
        End Function

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT *, (SELECT SKU FROM StoreItem WHERE ItemId = " & ItemId & ") as SKU FROM ShopDesignItem WHERE ShopDesignId =" & ShopDesignId & " AND ItemId=" & ItemId
                r = m_DB.GetReader(SQL)
                If r.Read() Then
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
                    If Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId")) Then
                        m_ShopDesignId = CInt(reader("ShopDesignId"))
                    Else
                        m_ShopDesignId = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("ItemId")) Then
                        m_ItemId = CInt(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("SKU")) Then
                        m_SKU = reader("SKU")
                    Else
                        m_SKU = ""
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("QtyDefault")) Then
                        m_QtyDefault = CInt(reader("QtyDefault"))
                    Else
                        m_QtyDefault = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("IsActive")) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("SortOrder")) Then
                        m_SortOrder = CInt(reader("SortOrder"))
                    Else
                        m_SortOrder = 0
                    End If
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Overridable Function Insert() As Integer
            Dim sp As String = "sp_ShopDesignItem_Insert"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddOutParameter(cmd, "ItemId", DbType.Int32, 32)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.AddInParameter(cmd, "SKU", DbType.Int32, SKU)
            db.AddInParameter(cmd, "QtyDefault", DbType.Int32, QtyDefault)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            ItemId = db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Return ItemId
        End Function

        Public Overridable Function Update() As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_ShopDesignItem_Update"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.AddInParameter(cmd, "SKU", DbType.String, SKU)
            db.AddInParameter(cmd, "QtyDefault", DbType.Int32, QtyDefault)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            result = db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal ShopDesignId As Integer, ByVal ItemId As Integer) As Boolean
            Try
                Dim sp As String = "sp_ShopDesignItem_Delete"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                Dim result As Integer = db.ExecuteNonQuery(cmd)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                If result > 0 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
                Return False
        End Function

        Public Shared Function ChangeSortOrder(ByVal ShopDesignId As Integer, ByVal ItemId As Integer, ByVal IsUp As Boolean) As Boolean
            Try
                Dim sp As String = "sp_ShopDesignItem_ChangeSortOrder"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "IsUp", DbType.Int32, IsUp)
                Dim result As Integer = db.ExecuteNonQuery(cmd)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                If result > 0 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

    End Class

    Public Class ShopDesignItemRowCollection
        Inherits CollectionBase
        Public Sub New()
        End Sub

        Public Sub Add(ByVal item As ShopDesignItemRow)
            Me.List.Add(item)
        End Sub

        Default Public Property Item(ByVal index As Integer) As ShopDesignItemRow
            Get
                Return CType(Me.List(index), ShopDesignItemRow)
            End Get
            Set(ByVal value As ShopDesignItemRow)
                Me.List(index) = value
            End Set
        End Property

    End Class
End Namespace

