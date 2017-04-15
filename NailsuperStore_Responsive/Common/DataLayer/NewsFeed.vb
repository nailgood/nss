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
Imports Utility
Namespace DataLayer
    Public Class NewsFeedRow
        Inherits NewsFeedRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal SubmissionId As Integer)
            MyBase.New(database, SubmissionId)
        End Sub 'New

        'end 23/10/2009
        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal SubmissionId As Integer) As NewsFeedRow
            Dim row As NewsFeedRow

            row = New NewsFeedRow(_Database, SubmissionId)
            row.Load()

            Return row
        End Function
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal SubmissionId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsFeed_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsFeedId", SqlDbType.Int, 0, SubmissionId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class
    Public MustInherit Class NewsFeedRowBase
        Private m_DB As Database
        Private m_NewsFeedId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_ShortContent As String = Nothing
        Private m_Url As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_Image As String = Nothing
        Private m_Source As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Public Shared cachePrefixKey As String = "NewsFeed_GetListNewsFeed"
        'Public Shared CountRow As Integer = 0
        Public Property NewsFeedId() As Integer
            Get
                Return m_NewsFeedId
            End Get
            Set(ByVal value As Integer)
                m_NewsFeedId = value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal value As String)
                m_Title = value
            End Set
        End Property
        Public Property ShortContent() As String
            Get
                Return m_ShortContent
            End Get
            Set(ByVal Value As String)
                m_ShortContent = Value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(ByVal Value As String)
                m_Url = Value
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

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property

        Public Property Source() As String
            Get
                Return m_Source
            End Get
            Set(ByVal Value As String)
                m_Source = Value
            End Set
        End Property

        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
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

        Public Sub New(ByVal database As Database, ByVal NewsFeedId As Integer)
            m_DB = database
            m_NewsFeedId = NewsFeedId
        End Sub 'New
        Public Shared Function GetListNewsFeed(ByVal DB As Database) As DataTable
            Dim dt As DataTable
            Dim key As String = cachePrefixKey
            dt = CType(CacheUtils.GetCache(key), DataTable)
            If Not dt Is Nothing Then
                Return dt
            End If
            dt = DB.GetDataTable("exec sp_NewsFeed_GetAll")
            CacheUtils.SetCache(key, dt, Utility.ConfigData.TimeCacheDataItem)
            Return dt
        End Function
        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_NewsFeed_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "NewsFeedId", DbType.Int32, NewsFeedId)
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
            m_NewsFeedId = Convert.ToInt32(r.Item("NewsFeedId"))
            If IsDBNull(r.Item("Title")) Then
                m_Title = Nothing
            Else
                m_Title = Convert.ToString(r.Item("Title"))
            End If
            If IsDBNull(r.Item("ShortContent")) Then
                m_ShortContent = Nothing
            Else
                m_ShortContent = Convert.ToString(r.Item("ShortContent"))
            End If
            If IsDBNull(r.Item("Url")) Then
                m_Url = Nothing
            Else
                m_Url = Convert.ToString(r.Item("Url"))
            End If
            If IsDBNull(r.Item("IsActive")) Then
                m_IsActive = Nothing
            Else
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            End If
            If IsDBNull(r.Item("Image")) Then
                m_Image = Nothing
            Else
                m_Image = Convert.ToString(r.Item("Image"))
            End If
            If IsDBNull(r.Item("Source")) Then
                m_Source = Nothing
            Else
                m_Source = Convert.ToString(r.Item("Source"))
            End If
            If IsDBNull(r.Item("CreatedDate")) Then
                m_CreatedDate = Nothing
            Else
                m_CreatedDate = Convert.ToDateTime(r.Item("CreatedDate"))
            End If
        End Sub 'Load
        Public Overridable Sub Insert()
            Dim result As Integer = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_NewsFeed_Insert")
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "ShortContent", DbType.String, ShortContent)
            db.AddInParameter(cmd, "Url", DbType.String, Url)
            db.AddInParameter(cmd, "IsActive", DbType.Int16, Convert.ToInt32(IsActive))
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "Source", DbType.Int16, Source)
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, CreatedDate)
            db.AddOutParameter(cmd, "NewsFeedId", DbType.Int64, 1)
            db.ExecuteNonQuery(cmd)
            result = Convert.ToInt32(db.GetParameterValue(cmd, "NewsFeedId"))
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub

        Public Overloads Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_NewsFeed_Update"
                Dim cm As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cm, "NewsFeedId", DbType.Int64, NewsFeedId)
                db.AddInParameter(cm, "Title", DbType.String, Title)
                db.AddInParameter(cm, "ShortContent", DbType.String, ShortContent)
                db.AddInParameter(cm, "Url", DbType.String, Url)
                db.AddInParameter(cm, "IsActive", DbType.Int16, IsActive)
                db.AddInParameter(cm, "Image", DbType.String, Image)
                db.AddInParameter(cm, "Source", DbType.Int16, Source)
                db.AddInParameter(cm, "CreatedDate", DbType.DateTime, CreatedDate)
                db.ExecuteNonQuery(cm)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception

            End Try
        End Sub
        Public Shared Function Delete(ByVal _Database As Database, ByVal NewsFeedId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsFeed_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsFeedId", SqlDbType.Int, 0, NewsFeedId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class

End Namespace
