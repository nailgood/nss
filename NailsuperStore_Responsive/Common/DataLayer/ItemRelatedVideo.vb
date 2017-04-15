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
Namespace DataLayer
    Public Class ItemRelatedVideoRow
        Inherits ItemRelatedVideoRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal videoId As Integer, ByVal ItemId As Integer)
            MyBase.New(database, videoId, ItemId)
        End Sub 'New

        Public Shared Function GetItemByVideoId(ByVal _Database As Database, ByVal data As ItemRelatedVideoRow) As ItemRelatedVideoCollection
            Dim ss As New ItemRelatedVideoCollection
            Dim keyData As String = String.Format(cachePrefixKey & "GetItemByVideoId_{0}_{1}_{2}_{3}_{4}", data.VideoId, data.OrderBy, data.OrderDirection, data.PageIndex, data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "GetItemByVideoId_Total"
            ss = CType(CacheUtils.GetCache(keyData), ItemRelatedVideoCollection)
            If Not ss Is Nothing Then
                data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New ItemRelatedVideoCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_ItemRelatedVideo_ListItemByVideoId"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, data.VideoId))
                cmd.Parameters.Add(_Database.InParam("OrderBy", SqlDbType.VarChar, 0, data.OrderBy))
                cmd.Parameters.Add(_Database.InParam("OrderDirection", SqlDbType.VarChar, 0, data.OrderDirection))
                cmd.Parameters.Add(_Database.InParam("CurrentPage", SqlDbType.Int, 0, data.PageIndex))
                cmd.Parameters.Add(_Database.InParam("PageSize", SqlDbType.Int, 0, data.PageSize))
                cmd.Parameters.Add(_Database.OutParam("TotalRecords", SqlDbType.Int, 0))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                data.TotalRow = Convert.ToInt32(cmd.Parameters("TotalRecords").Value)
                CacheUtils.SetCache(keytotal,data.TotalRow)
                CacheUtils.SetCache(keyData, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader)
            Dim result As New ItemRelatedVideoRow
            If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                result.VideoId = Convert.ToInt32(reader("VideoId"))
            Else
                result.VideoId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                result.ItemId = Convert.ToInt32(reader("ItemId"))
            Else
                result.ItemId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                result.Arrange = Convert.ToInt32(reader("Arrange"))
            Else
                result.Arrange = 0
            End If
            Return result
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As ItemRelatedVideoRow, ByVal clearCache As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ItemRelatedVideo_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, data.VideoId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, data.ItemId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                If (clearCache) Then
                    CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                End If
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal VideoId As Integer, ByVal ItemId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ItemRelatedVideo_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
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

        Public Shared Function ChangeArrange(ByVal _Database As Database, ByVal VideoId As Integer, ByVal ItemId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ItemRelatedVideo_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
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

        Public MustInherit Class ItemRelatedVideoRowBase
            Private m_DB As Database
            Private m_VideoId As Integer = Nothing
            Private m_ItemId As Integer = Nothing
            Private m_Arrange As Integer = Nothing
            Private m_TotalRow As Integer = Nothing
            Private m_PageIndex As Integer = Nothing
            Private m_PageSize As Integer = Nothing
            Private m_Condition As String = Nothing
            Private m_OrderBy As String = Nothing
            Private m_OrderDirection As String = Nothing
            Public Shared cachePrefixKey As String = "ItemRelatedVideo_"

            Public Property VideoId() As Integer
                Get
                    Return m_VideoId
                End Get
                Set(ByVal Value As Integer)
                    m_VideoId = Value
                End Set
            End Property

            Public Property ItemId() As Integer
                Get
                    Return m_ItemId
                End Get
                Set(ByVal Value As Integer)
                    m_ItemId = Value
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

            Public Sub New()
            End Sub 'New

            Public Sub New(ByVal database As Database)
                m_DB = database
            End Sub 'New

            Public Sub New(ByVal database As Database, ByVal VideoId As Integer, ByVal ItemId As Integer)
                m_DB = database
                m_VideoId = VideoId
            m_ItemId = ItemId
            End Sub 'New
        End Class

        Public Class ItemRelatedVideoCollection
            Inherits CollectionBase
            Public Sub New()
            End Sub

        Public Sub Add(ByVal Image As ItemRelatedVideoRow)
            Me.List.Add(Image)
        End Sub

            Default Public Property Item(ByVal Index As Integer) As ItemRelatedVideoRow
                Get
                    Return CType(Me.List.Item(Index), ItemRelatedVideoRow)
                End Get

                Set(ByVal Value As ItemRelatedVideoRow)
                    Me.List(Index) = Value
                End Set
            End Property
        End Class


End Namespace

