
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
    Public Class VideoRelatedRow
        Inherits VideoRelatedRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal videoId As Integer, ByVal VideoRelatedId As Integer)
            MyBase.New(database, videoId, VideoRelatedId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal videoId As Integer, ByVal VideoRelatedId As Integer) As VideoRelatedRow
            Dim row As VideoRelatedRow
            row = New VideoRelatedRow(_Database, videoId, VideoRelatedId)
            row.Load()
            Return row
        End Function

        Public Shared Function GetByVideoId(ByVal _Database As Database, ByVal data As VideoRelatedRow) As VideoRelatedCollection
            Dim ss As New VideoRelatedCollection
            Dim keyData As String = String.Format(cachePrefixKey & "GetByVideoId_{0}_{1}_{2}_{3}_{4}", data.VideoId, data.OrderBy, data.OrderDirection, data.PageIndex, data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "GetByVideoId_Total"
            ss = CType(CacheUtils.GetCache(keyData), VideoRelatedCollection)
            If Not ss Is Nothing Then
                ''get Total
                data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New VideoRelatedCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_VideoRelated_ListByVideoId_V1"
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
                CacheUtils.SetCache(keyData, ss)
                CacheUtils.SetCache(keyTotal, data.TotalRow)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            
            Return ss
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As VideoRelatedRow
            Dim result As New VideoRelatedRow
            If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                result.VideoId = Convert.ToInt32(reader("VideoId"))
            Else
                result.VideoId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("VideoRelatedId"))) Then
                result.VideoRelatedId = Convert.ToInt32(reader("VideoRelatedId"))
            Else
                result.VideoRelatedId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                result.Arrange = Convert.ToInt32(reader("Arrange"))
            Else
                result.Arrange = 0
            End If
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal VideoId As Integer, ByVal VideoRelatedId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_VideoRelated_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("VideoRelatedId", SqlDbType.Int, 0, VideoRelatedId))
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

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As VideoRelatedRow, ByVal clearCache As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_VideoRelated_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, data.VideoId))
                cmd.Parameters.Add(_Database.InParam("VideoRelatedId", SqlDbType.Int, 0, data.VideoRelatedId))
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

        Public Shared Function ChangeArrange(ByVal _Database As Database, ByVal VideoId As Integer, ByVal VideoRelatedId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_VideoRelated_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("VideoRelatedId", SqlDbType.Int, 0, VideoRelatedId))
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


    Public MustInherit Class VideoRelatedRowBase
        Private m_DB As Database
        Private m_VideoId As Integer = Nothing
        Private m_VideoRelatedId As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Public Shared cachePrefixKey As String = "VideoRelated_"
        Public Property VideoId() As Integer
            Get
                Return m_VideoId
            End Get
            Set(ByVal Value As Integer)
                m_VideoId = Value
            End Set
        End Property
        Public Property VideoRelatedId() As Integer
            Get
                Return m_VideoRelatedId
            End Get
            Set(ByVal Value As Integer)
                m_VideoRelatedId = Value
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

        Public Sub New(ByVal database As Database, ByVal VideoId As Integer, ByVal VideoRelatedId As Integer)
            m_DB = database
            m_VideoId = VideoId
            m_VideoRelatedId = VideoRelatedId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM VideoRelated WHERE VideoId = " & DB.Number(VideoId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then

                    If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                        m_VideoId = Convert.ToInt32(reader("VideoId"))
                    Else
                        m_VideoId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("VideoRelatedId"))) Then
                        m_VideoRelatedId = Convert.ToInt32(reader("VideoRelatedId"))
                    Else
                        m_VideoRelatedId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                        m_Arrange = Convert.ToInt32(reader("Arrange"))
                    Else
                        m_Arrange = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex

            End Try

        End Sub

    End Class

    Public Class VideoRelatedCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Image As VideoRelatedRow)
            Me.List.Add(Image)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As VideoRelatedRow
            Get
                Return CType(Me.List.Item(Index), VideoRelatedRow)
            End Get

            Set(ByVal Value As VideoRelatedRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


