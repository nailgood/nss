
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
    Public Class DocumentRow
        Inherits DocumentRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DocumentId As Integer)
            MyBase.New(database, DocumentId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DocumentId As Integer) As DocumentRow
            Dim row As DocumentRow
            row = New DocumentRow(_Database, DocumentId)
            row.Load()
            Return row
        End Function

        Public Shared Function ListAll(ByVal _Database As Database) As DocumentCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim ss As New DocumentCollection
                Dim keyData As String = cachePrefixKey & "ListAll"
                ss = CType(CacheUtils.GetCache(keyData), DocumentCollection)
                If Not ss Is Nothing Then
                    Return ss
                Else
                    ss = New DocumentCollection
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Document_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(keyData, ss)
                Return ss
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return New DocumentCollection
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As DocumentRow
            Dim result As New DocumentRow
            If (Not reader.IsDBNull(reader.GetOrdinal("DocumentId"))) Then
                result.DocumentId = Convert.ToInt32(reader("DocumentId"))
            Else
                result.DocumentId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("DocumentName"))) Then
                result.DocumentName = reader("DocumentName").ToString()
            Else
                result.DocumentName = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = Convert.ToBoolean(reader("IsActive"))
            Else
                result.IsActive = True
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("FileName"))) Then
                result.FileName = reader("FileName").ToString()
            Else
                result.FileName = ""
            End If
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal DocumentId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Document_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DocumentId", SqlDbType.Int, 0, DocumentId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, DocumentRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As DocumentRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Document_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DocumentName", SqlDbType.NVarChar, 0, data.DocumentName))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileName", SqlDbType.NVarChar, 0, data.FileName))
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
        Public Shared Function Update(ByVal _Database As Database, ByVal data As DocumentRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Document_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DocumentId", SqlDbType.NVarChar, 0, data.DocumentId))
                cmd.Parameters.Add(_Database.InParam("DocumentName", SqlDbType.NVarChar, 0, data.DocumentName))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileName", SqlDbType.NVarChar, 0, data.FileName))
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
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal DocumentId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Document_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DocumentId", SqlDbType.Int, 0, DocumentId))
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


    Public MustInherit Class DocumentRowBase
        Private m_DB As Database
        Private m_DocumentId As Integer = Nothing
        Private m_DocumentName As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_FileName As String = Nothing
        Public Shared cachePrefixKey As String = "Document_"

        Public Property DocumentId() As Integer
            Get
                Return m_DocumentId
            End Get
            Set(ByVal Value As Integer)
                m_DocumentId = Value
            End Set
        End Property
        Public Property DocumentName() As String
            Get
                Return m_DocumentName
            End Get
            Set(ByVal Value As String)
                m_DocumentName = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return m_FileName
            End Get
            Set(ByVal Value As String)
                m_FileName = Value
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

        Public Sub New(ByVal database As Database, ByVal DocumentId As Integer)
            m_DB = database
            m_DocumentId = DocumentId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM Document WHERE DocumentId = " & DB.Number(DocumentId)
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

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("DocumentId"))) Then
                    m_DocumentId = Convert.ToInt32(reader("DocumentId"))
                Else
                    m_DocumentId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("DocumentName"))) Then
                    m_DocumentName = reader("DocumentName").ToString()
                Else
                    m_DocumentName = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = True
                End If


                If (Not reader.IsDBNull(reader.GetOrdinal("FileName"))) Then
                    m_FileName = reader("FileName").ToString()
                Else
                    m_FileName = ""
                End If

            End If
        End Sub

    End Class

    Public Class DocumentCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Document As DocumentRow)
            Me.List.Add(Document)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DocumentRow
            Get
                Return CType(Me.List.Item(Index), DocumentRow)
            End Get

            Set(ByVal Value As DocumentRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


