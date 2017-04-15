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
    Public Class NewsDocumentRow
        Inherits NewsDocumentRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As NewsDocumentRow
            Dim row As NewsDocumentRow
            Dim key As String = cachePrefixKey & "GetRow_" & Id
            row = CType(CacheUtils.GetCache(key), NewsDocumentRow)
            If Not row Is Nothing Then
                Return row
            End If
            row = New NewsDocumentRow(DB, Id)
            row.Load()

            Return row
        End Function

    End Class
    Public MustInherit Class NewsDocumentRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_NewsId As Integer = Nothing
        Private m_DocumentId As Integer = Nothing
        Private m_Arrange As String = Nothing
        Private m_IsActive As Boolean = True
        Public Shared cachePrefixKey As String = "NewsDocument_"
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
        Public Property DocumentId() As Integer
            Get
                Return m_DocumentId
            End Get
            Set(ByVal Value As Integer)
                m_DocumentId = Value
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
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
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
                SQL = "SELECT * FROM NewsDocument WHERE Id = " & DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try


        End Sub
        Public Shared Function ListAll(ByVal _Database As Database) As NewsDocumentCollection
            Dim nd As New NewsDocumentCollection
            Dim key As String = cachePrefixKey & "ListAll"
            nd = CType(CacheUtils.GetCache(key), NewsDocumentCollection)
            If Not nd Is Nothing Then
                Return nd
            Else
                nd = New NewsDocumentCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_NewsDocument_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    nd.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, nd)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return nd
        End Function
        Public Shared Function ListAllForNewsId(ByVal _Database As Database, ByVal NewsId As Integer) As NewsDocumentCollection
            Dim nd As New NewsDocumentCollection
            Dim key As String = cachePrefixKey & "ListAllForNewsId_" & NewsId
            nd = CType(CacheUtils.GetCache(key), NewsDocumentCollection)
            If Not nd Is Nothing Then
                Return nd
            Else
                nd = New NewsDocumentCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_NewsDocument_ListByNewsId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    nd.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, nd)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return nd
        End Function
        Public Shared Function ListAllActive(ByVal _Database As Database, ByVal NewsId As Integer) As NewsDocumentCollection
            Dim nd As New NewsDocumentCollection
            Dim key As String = cachePrefixKey & "ListAllActive_" & NewsId
            nd = CType(CacheUtils.GetCache(key), NewsDocumentCollection)
            If Not nd Is Nothing Then
                Return nd
            Else
                nd = New NewsDocumentCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_NewsDocument_ListAllActive"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    nd.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, nd)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return nd
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As NewsDocumentRow
            Dim result As New NewsDocumentRow
            If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                result.Id = Convert.ToInt32(reader("Id"))
            Else
                result.Id = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                result.NewsId = Convert.ToInt32(reader("Id"))
            Else
                result.NewsId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("DocumentId"))) Then
                result.DocumentId = Convert.ToInt32(reader("DocumentId"))
            Else
                result.DocumentId = 0
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


            Return result
        End Function
        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsDocument_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result = 1 Then

                Return True
            End If
            Return False
        End Function


        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsDocument_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result = 1 Then

                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal Id As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsDocument_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            If result = 1 Then

                Return True
            End If
            Return False
        End Function
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                    m_NewsId = Convert.ToInt32(reader("NewsId"))
                Else
                    m_NewsId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DocumentId"))) Then
                    m_DocumentId = Convert.ToInt32(reader("DocumentId"))
                Else
                    m_DocumentId = 0
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

            End If
        End Sub
        Public Overridable Function Insert(ByVal clearCache As Boolean) As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_News_INSERT As String = "sp_NewsDocument_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_News_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
            db.AddInParameter(cmd, "DocumentId", DbType.Int32, DocumentId)
            db.AddInParameter(cmd, "IsActive", DbType.String, IsActive)
            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            If (clearCache) Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            End If
            '------------------------------------------------------------------------
            Return Id
        End Function
        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_News_UPDATE As String = "sp_NewsDocument_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_News_UPDATE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "NewsId", DbType.Int32, NewsId)
            db.AddInParameter(cmd, "DocumentId", DbType.Int32, DocumentId)
            db.AddInParameter(cmd, "Arrange", DbType.String, Arrange)
            db.AddInParameter(cmd, "IsActive", DbType.String, IsActive)


            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------

        End Sub 'Update
        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_News_DELETE As String = "sp_NewsDocument_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_News_DELETE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class
    Public Class NewsDocumentCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal News As NewsDocumentRow)
            Me.List.Add(News)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As NewsDocumentRow
            Get
                Return CType(Me.List.Item(Index), NewsDocumentRow)
            End Get

            Set(ByVal Value As NewsDocumentRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace
