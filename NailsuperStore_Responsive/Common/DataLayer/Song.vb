Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Data.SqlClient
Namespace DataLayer
    Public Class SongRow
        Inherits SongRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal SongId As Integer)
            MyBase.New(database, SongId)
        End Sub 'New
        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal AlbumId As Integer, ByVal SongId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AlbumSong_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SongId", SqlDbType.Int, 0, SongId))
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
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
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal SongId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Song_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SongId", SqlDbType.Int, 0, SongId))
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
        Public Shared Function Delete(ByVal _Database As Database, ByVal SongId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Song_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SongId", SqlDbType.Int, 0, SongId))
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
        Public Shared Function Remove_AlbumSong(ByVal _Database As Database, ByVal SongId As Integer, ByVal AlbumId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AlbumSong_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SongId", SqlDbType.Int, 0, SongId))
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
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
        Public Shared Function GetAllByAlbumId(ByVal _Database As Database, ByVal albumId As Integer) As SongCollection
            Dim ss As New SongCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Album_GetAllByAlbumId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, albumId))
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetData(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Private Shared Function GetData(ByVal reader As SqlDataReader) As SongRow
            Dim result As New SongRow
            If (Not reader.IsDBNull(reader.GetOrdinal("SongId"))) Then
                result.SongId = Convert.ToInt32(reader("SongId"))
            Else
                result.SongId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                result.Name = reader("Name").ToString()
            Else
                result.Name = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("FileSong"))) Then
                result.FileSong = reader("FileSong").ToString()
            Else
                result.FileSong = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("FileLength"))) Then
                result.FileLength = reader("FileLength").ToString()
            Else
                result.FileLength = ""
            End If


            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = Convert.ToBoolean(reader("IsActive"))
            Else
                result.IsActive = True
            End If

            If IsDBNull(reader.Item("CreatedDate")) Then
                result.CreateDate = Nothing
            Else
                result.CreateDate = Convert.ToDateTime(reader.Item("CreatedDate"))
            End If
            If IsDBNull(reader.Item("ModifyDate")) Then
                result.ModifyDate = Nothing
            Else
                result.ModifyDate = Convert.ToDateTime(reader.Item("ModifyDate"))
            End If
            Return result
        End Function
    End Class
    Public MustInherit Class SongRowBase
        Private m_DB As Database
        Private m_SongId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_FileSong As String = Nothing
        Private m_FileLength As String = Nothing
        Private m_IsActive As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Public Property SongId() As Integer
            Get
                Return m_SongId
            End Get
            Set(ByVal value As Integer)
                m_SongId = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Public Property FileSong() As String
            Get
                Return m_FileSong
            End Get
            Set(ByVal value As String)
                m_FileSong = value
            End Set
        End Property
        Public Property FileLength() As String
            Get
                Return m_FileLength
            End Get
            Set(ByVal value As String)
                m_FileLength = value
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
        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal value As Integer)
                m_Arrange = value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal value As DateTime)
                m_CreateDate = value
            End Set
        End Property

        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifyDate = value
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
        Public Sub New(ByVal database As Database, ByVal SongId As Integer)
            m_DB = database
            SongId = 0
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal SongId As Integer) As SongRow
            Dim row As SongRow

            row = New SongRow(_Database, SongId)
            row.LoadBySongId(SongId)

            Return row
        End Function

        Protected Overridable Sub LoadBySongId(ByVal SongId As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = ""
                SQL = "SELECT * FROM Song WHERE SongId = " & SongId
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            If IsDBNull(r.Item("SongId")) Then
                m_SongId = 0
            Else
                m_SongId = Convert.ToInt32(r.Item("SongId"))
            End If
            If IsDBNull(r.Item("Name")) Then
                m_Name = Nothing
            Else
                m_Name = Convert.ToString(r.Item("Name"))
            End If
            If IsDBNull(r.Item("FileSong")) Then
                m_FileSong = Nothing
            Else
                m_FileSong = Convert.ToString(r.Item("FileSong"))
            End If
            If IsDBNull(r.Item("FileLength")) Then
                m_FileLength = Nothing
            Else
                m_FileLength = Convert.ToString(r.Item("FileLength"))
            End If
            If (Not r.IsDBNull(r.GetOrdinal("IsActive"))) Then
                m_IsActive = Convert.ToBoolean(r("IsActive"))
            Else
                m_IsActive = False
            End If
            If IsDBNull(r.Item("Arrange")) Then
                m_Arrange = 0
            Else
                m_Arrange = Convert.ToInt32(r.Item("Arrange"))
            End If
            If IsDBNull(r.Item("CreatedDate")) Then
                m_CreateDate = Nothing
            Else
                m_CreateDate = Convert.ToDateTime(r.Item("CreatedDate"))
            End If
            If IsDBNull(r.Item("ModifyDate")) Then
                m_ModifyDate = Nothing
            Else
                m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
            End If
        End Sub
        Public Overridable Sub Update()
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_Song_Update"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cm, "SongId", DbType.Int32, SongId)
            db.AddInParameter(cm, "Name", DbType.String, Name)
            db.AddInParameter(cm, "FileSong", DbType.String, FileSong)
            db.AddInParameter(cm, "FileLength", DbType.String, FileLength)
            db.AddInParameter(cm, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cm, "Arrange", DbType.Int32, Arrange)
            db.AddInParameter(cm, "ModifyDate", DbType.DateTime, Date.Now)
            db.ExecuteNonQuery(cm)
            'Catch ex As Exception

            'End Try

        End Sub
        Public Overridable Sub Insert()
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_Song_Insert"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddOutParameter(cm, "SongId", DbType.Int32, 1)
            db.AddInParameter(cm, "Name", DbType.String, Name)
            db.AddInParameter(cm, "FileSong", DbType.String, FileSong)
            db.AddInParameter(cm, "FileLength", DbType.String, FileLength)
            db.AddInParameter(cm, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cm, "CreateDate", DbType.DateTime, Date.Now)
            db.ExecuteNonQuery(cm)
            SongId = Convert.ToInt32(db.GetParameterValue(cm, "SongId"))
            'Catch ex As Exception

            'End Try

        End Sub
    End Class
    Public Class SongCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Song As SongRow)
            Me.List.Add(Song)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As SongRow
            Get
                Return CType(Me.List.Item(Index), SongRow)
            End Get

            Set(ByVal Value As SongRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace

