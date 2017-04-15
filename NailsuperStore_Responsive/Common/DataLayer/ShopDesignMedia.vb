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
Imports Utility.Common

Namespace DataLayer
    Public Class ShopDesignMediaRow
        Inherits ShopDesignMediaRowBase
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub
        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub

        Public Shared Function ListByShopDesignId(ByVal ShopDesignId As Integer, ByVal Type As ShopDesignMediaType) As ShopDesignMediaCollection
            Dim key As String = String.Format(cachePrefixKey & "ListByShopDesignId_{0}_{1}", ShopDesignId, CInt(Type))
            Dim result As New ShopDesignMediaCollection
            result = CType(CacheUtils.GetCache(key), ShopDesignMediaCollection)
            If Not result Is Nothing Then
                Return result
            Else
                result = New ShopDesignMediaCollection
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_ShopDesignMedia_ListByShopDesignId"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "Type", DbType.Int32, CInt(Type))
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

        Private Shared Function GetDataListFromReader(ByVal reader As SqlDataReader) As ShopDesignMediaRow
            Dim result As New ShopDesignMediaRow
            If Not reader.IsDBNull(reader.GetOrdinal("Id")) Then
                result.Id = CInt(reader("Id"))
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("Url")) Then
                result.Url = reader("Url")
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("Tag")) Then
                result.Tag = reader("Tag")
            End If
            If Not reader.IsDBNull(reader.GetOrdinal("Type")) Then
                result.Type = CInt(reader("Type"))
            End If
            Return result
        End Function

    End Class

    Public MustInherit Class ShopDesignMediaRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_ShopDesignId As Integer = Nothing
        Private m_Url As String = Nothing
        Private m_Tag As String = Nothing
        Private m_Description As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_Type As Integer = Nothing

        Public Shared cachePrefixKey As String = "ShopDesignMedia_"
        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(ByVal value As Database)
                m_DB = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal value As Integer)
                m_Id = value
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
        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(ByVal value As String)
                m_Url = value
            End Set
        End Property
        Public Property Tag() As String
            Get
                Return m_Tag
            End Get
            Set(ByVal value As String)
                m_Tag = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
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
        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property

        Public Sub New()
        End Sub
        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub
        Public Sub New(ByVal db As Database, ByVal Id As Integer)
            m_DB = db
            m_Id = Id
        End Sub

        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As ShopDesignMediaRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", Id)
            Dim result As ShopDesignMediaRow
            result = CType(CacheUtils.GetCache(key), ShopDesignMediaRow)
            If Not result Is Nothing Then
                Return result
            End If
            result = New ShopDesignMediaRow(DB, Id)
            result.Load()
            CacheUtils.SetCache(key, result)
            Return result
        End Function

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT * FROM ShopDesignMedia WHERE Id =" & Id
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
                    If Not reader.IsDBNull(reader.GetOrdinal("Id")) Then
                        m_Id = CInt(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("ShopDesignId")) Then
                        m_ShopDesignId = CInt(reader("ShopDesignId"))
                    Else
                        m_ShopDesignId = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("Url")) Then
                        m_Url = reader("Url")
                    Else
                        m_Url = ""
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("Tag")) Then
                        m_Tag = reader("Tag")
                    Else
                        m_Tag = ""
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("SortOrder")) Then
                        m_SortOrder = reader("SortOrder")
                    Else
                        m_SortOrder = ""
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("Type")) Then
                        m_Type = CInt(reader("Type"))
                    Else
                        m_Type = 0
                    End If
                    If Not reader.IsDBNull(reader.GetOrdinal("Description")) Then
                        m_Description = reader("Description")
                    Else
                        m_Description = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Overridable Function Insert() As Integer
            Dim sp As String = "sp_ShopDesignMedia_Insert"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.AddInParameter(cmd, "Url", DbType.String, Url)
            db.AddInParameter(cmd, "Tag", DbType.String, Tag)
            db.AddInParameter(cmd, "Type", DbType.Int32, Type)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.ExecuteNonQuery(cmd)
            Id = CInt(db.GetParameterValue(cmd, "Id"))
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Return Id
        End Function

        Public Overridable Function Update() As Boolean
            Dim sp As String = "sp_ShopDesignMedia_Update"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
            db.AddInParameter(cmd, "Url", DbType.String, Url)
            db.AddInParameter(cmd, "Tag", DbType.String, Tag)
            db.AddInParameter(cmd, "Type", DbType.Int32, Type)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ShopDesignMedia_Delete"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                result = db.ExecuteNonQuery(cmd)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeSortOrder(ByVal Id As Integer, ByVal ShopDesignId As Integer, ByVal Type As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ShopDesignMedia_ChangeSortOrder"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                db.AddInParameter(cmd, "ShopDesignId", DbType.Int32, ShopDesignId)
                db.AddInParameter(cmd, "Type", DbType.Int32, Type)
                db.AddInParameter(cmd, "IsUp", DbType.Int32, IsUp)
                result = db.ExecuteNonQuery(cmd)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public Class ShopDesignMediaCollection
        Inherits CollectionBase
        Public Sub New()
        End Sub
        Public Sub Add(ByVal media As ShopDesignMediaRow)
            Me.List.Add(media)
        End Sub
        Default Public Property Item(ByVal index As Integer) As ShopDesignMediaRow
            Get
                Return CType(Me.List.Item(index), ShopDesignMediaRow)
            End Get
            Set(ByVal value As ShopDesignMediaRow)
                Me.List(index) = value
            End Set
        End Property
    End Class
End Namespace
