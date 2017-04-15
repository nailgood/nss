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
    Public Class NewsAudioRow
        Inherits NewsAudioRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As NewsAudioRow
            Dim row As NewsAudioRow
            row = New NewsAudioRow(_Database, id)
            row.Load()
            Return row
        End Function


        Public Shared Function GetByNewId(ByVal _Database As Database, ByVal newsId As Integer) As NewsAudioCollection
            Dim dr As SqlDataReader = Nothing
            Dim ss As New NewsAudioCollection
            Try
                Dim sp As String = "sp_NewsAudio_GetByByNewId"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, newsId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                ''mail error here
            End Try
            Return ss
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As NewsAudioRow
            Dim result As New NewsAudioRow
            If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                result.Id = Convert.ToInt32(reader("Id"))
            Else
                result.Id = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                result.NewsId = Convert.ToInt32(reader("NewsId"))
            Else
                result.NewsId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("AudioId"))) Then
                result.AudioId = Convert.ToInt32(reader("AudioId"))
            Else
                result.AudioId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                result.Arrange = Convert.ToInt32(reader("Arrange"))
            Else
                result.Arrange = 0
            End If

            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsAudio_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function DeleteByNewsId(ByVal _Database As Database, ByVal newsId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsAudio_DeleteByNewsId"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, newsId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As NewsAudioRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsAudio_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, data.NewsId))
                cmd.Parameters.Add(_Database.InParam("AudioId", SqlDbType.Int, 0, data.AudioId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

    End Class


    Public MustInherit Class NewsAudioRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_NewsId As Integer = Nothing
        Private m_AudioId As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Property AudioId() As Integer
            Get
                Return m_AudioId
            End Get
            Set(ByVal Value As Integer)
                m_AudioId = Value
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

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM NewsAudio WHERE Id = " & DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                ''mail error here
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AudioId"))) Then
                    m_AudioId = Convert.ToInt32(reader("AudioId"))
                Else
                    m_AudioId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                    m_NewsId = Convert.ToInt32(reader("NewsId"))
                Else
                    m_NewsId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                    m_Arrange = Convert.ToInt32(reader("Arrange"))
                Else
                    m_Arrange = 0
                End If
            End If
        End Sub

    End Class

    Public Class NewsAudioCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Image As NewsAudioRow)
            Me.List.Add(Image)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As NewsAudioRow
            Get
                Try
                    Return CType(Me.List.Item(Index), NewsAudioRow)
                Catch ex As Exception

                End Try

            End Get

            Set(ByVal Value As NewsAudioRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace



