Option Explicit On

Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports System.Collections
Imports Utility
Namespace DataLayer
    Public Class MemberSubmissionFileRow
        Inherits MemberSubmissionFileRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal FileName As String, ByVal SubmisssionId As Integer)
            MyBase.New(database, FileName, SubmisssionId)
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal FileName As String, ByVal SubmisssionId As Integer) As MemberSubmissionFileRow
            Dim row As MemberSubmissionFileRow
            row = New MemberSubmissionFileRow(_Database, FileName, SubmisssionId)
            row.Load()
            Return row
        End Function
    End Class
    Public MustInherit Class MemberSubmissionFileRowBase
        Private m_DB As Database
        Private m_FileId As Integer = Nothing
        Private m_SubmissionId As Integer = Nothing
        Private m_FileName As String = Nothing
        Private m_AdminUploadFile As String = Nothing
        Private Shared cachePrefixKey = "GalleryById_"
        Private m_NewId As Integer = Nothing
        Public Property FileId() As Integer
            Get
                Return m_FileId
            End Get
            Set(ByVal Value As Integer)
                m_FileId = Value
            End Set
        End Property
        Public Property NewId() As Integer
            Get
                Return m_NewId
            End Get
            Set(ByVal Value As Integer)
                m_NewId = Value
            End Set
        End Property
        Public Property SubmissionId() As Integer
            Get
                Return m_SubmissionId
            End Get
            Set(ByVal Value As Integer)
                m_SubmissionId = Value
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
        Public Property AdminUploadFile() As String
            Get
                Return m_AdminUploadFile
            End Get
            Set(ByVal Value As String)
                m_AdminUploadFile = Value
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

        Public Sub New(ByVal database As Database, ByVal FileName As String, ByVal SubmissionId As Integer)
            m_DB = database
            m_FileName = FileName
            m_SubmissionId = SubmissionId
        End Sub 'New
        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_MemberSubmissionFile_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "SubmisssionId", DbType.Int32, SubmissionId)
                db.AddInParameter(cmd, "FileName", DbType.String, FileName)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            If IsDBNull(r.Item("FileId")) Then
                m_FileId = Nothing
            Else
                m_FileId = Convert.ToInt32(r.Item("FileId"))
            End If
            If IsDBNull(r.Item("SubmissionId")) Then
                m_SubmissionId = Nothing
            Else
                m_SubmissionId = Convert.ToString(r.Item("SubmissionId"))
            End If
            If IsDBNull(r.Item("FileName")) Then
                m_FileName = Nothing
            Else
                m_FileName = Convert.ToString(r.Item("FileName"))
            End If
            If IsDBNull(r.Item("AdminUploadFile")) Then
                m_AdminUploadFile = Nothing
            Else
                m_AdminUploadFile = Convert.ToString(r.Item("AdminUploadFile"))
            End If
        End Sub 'Load
        
        Public Overridable Sub Insert(ByVal db As EnterpriseLibrary.Data.Database, ByVal trans As DbTransaction)
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_MemberSubmissionFile_Insert")
            db.AddInParameter(cmd, "SubmissionId", DbType.Int32, SubmissionId)
            db.AddInParameter(cmd, "FileName", DbType.String, FileName)
            db.AddInParameter(cmd, "AdminUploadFile", DbType.String, AdminUploadFile)
            db.AddOutParameter(cmd, "FileId", DbType.Int64, 1)
            db.ExecuteNonQuery(cmd, trans)
            NewId = Convert.ToInt32(db.GetParameterValue(cmd, "FileId"))
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub
        Public Overridable Sub Insert()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_MemberSubmissionFile_Insert")
            db.AddInParameter(cmd, "SubmissionId", DbType.Int32, SubmissionId)
            db.AddInParameter(cmd, "FileName", DbType.String, FileName)
            db.AddInParameter(cmd, "AdminUploadFile", DbType.String, AdminUploadFile)
            db.AddOutParameter(cmd, "FileId", DbType.Int64, 1)
            db.ExecuteNonQuery(cmd)
            NewId = Convert.ToInt32(db.GetParameterValue(cmd, "FileId"))
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub
        'Public Overloads Sub Update()
        '    Try
        '        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        '        Dim SP As String = "sp_MemberSubmissionFile_Update"
        '        Dim cm As DbCommand = db.GetStoredProcCommand(SP)
        '        db.AddInParameter(cm, "FileId", DbType.Int32, FileId)
        '        db.AddInParameter(cm, "SubmissionId", DbType.Int32, SubmissionId)
        '        db.AddInParameter(cm, "FileName", DbType.String, FileName)
        '        db.AddParameter(cm, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
        '        db.ExecuteNonQuery(cm)
        '        Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cm, "return_value"))
        '    Catch ex As Exception

        '    End Try
        'End Sub
        Public Shared Sub AdminUpload(ByVal FileId As Integer, ByVal AdminUploadFile As String)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_MemberSubmissionFile_Update"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cm, "FileId", DbType.String, FileId)
            db.AddInParameter(cm, "AdminUploadFile", DbType.String, AdminUploadFile)
            db.ExecuteNonQuery(cm)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub
        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_MemberSubmissionFile_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "FileId", DbType.Int32, FileId)
            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub 'Remove
    End Class
    Public Class MemberSubmissionFileCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal MemberSubmissionFile As MemberSubmissionFileRow)
            Me.List.Add(MemberSubmissionFile)
        End Sub

        Public Function Contains(ByVal MemberSubmissionFile As MemberSubmissionFileRow) As Boolean
            Return Me.List.Contains(MemberSubmissionFile)
        End Function

        Public Function IndexOf(ByVal MemberSubmissionFile As MemberSubmissionFileRow) As Integer
            Return Me.List.IndexOf(MemberSubmissionFile)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal MemberSubmissionFile As MemberSubmissionFileRow)
            Me.List.Insert(Index, MemberSubmissionFile)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As MemberSubmissionFileRow
            Get
                Return CType(Me.List.Item(Index), MemberSubmissionFileRow)
            End Get

            Set(ByVal Value As MemberSubmissionFileRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace

