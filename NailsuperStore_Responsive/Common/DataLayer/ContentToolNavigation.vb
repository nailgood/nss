Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Utility

Namespace DataLayer

    Public Class ContentToolNavigationRow
        Inherits ContentToolNavigationRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal NavigationId As Integer)
            MyBase.New(database, NavigationId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal NavigationId As Integer) As ContentToolNavigationRow
            Dim key As String = String.Format("{0}GetRow_{1}", cachePrefixKey, NavigationId)
            Dim row As ContentToolNavigationRow = CType(CacheUtils.GetCache(key), ContentToolNavigationRow)
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If

            row = New ContentToolNavigationRow(_Database, NavigationId)
            row.Load()

            CacheUtils.SetCache(key, row)
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal NavigationId As Integer)
            Dim row As ContentToolNavigationRow

            row = New ContentToolNavigationRow(_Database, NavigationId)
            row.Remove()
        End Sub

        Public Shared Function GetMainMenu(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select * from ContentToolNavigation where ParentId is null order by SortOrder")
            Return ds
        End Function

        Public Shared Function GetListMain() As ContentToolNavigationCollection

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_ContentToolNavigation_GetListMain"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            Dim dr As SqlDataReader = Nothing
            Dim cols As New ContentToolNavigationCollection

            Try
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim nav As New ContentToolNavigationRow()
                    nav.Load(dr)
                    cols.Add(nav)
                End While
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListMain()", ex)
            End Try
            Return cols
        End Function

        Public Shared Function GetListByParentId(ByVal ID As Integer) As ContentToolNavigationCollection

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "[sp_ContentToolNavigation_GetListByParentId]"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            Dim dr As SqlDataReader = Nothing
            Dim cols As New ContentToolNavigationCollection
            db.AddInParameter(cmd, "ParentID", DbType.Int64, ID)

            Try
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim nav As New ContentToolNavigationRow()
                    nav.Load(dr)
                    cols.Add(nav)
                End While
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListByParentId()", ex)
            End Try
            Return cols
        End Function

        Public Shared Function GetSubNavigation(ByVal Db As Database, ByVal ParentId As Integer) As DataSet
            Dim ds As DataSet = Db.GetDataSet("select * from ContentToolNavigation where ParentId = " & ParentId & " order by SortOrder")
            Return ds
        End Function

        Public Shared Function GetSubNavigationByURL(ByVal Db As Database, ByVal PageURL As String) As DataSet
            Dim SQL As String = ""

            SQL &= " select 0 as Parent, NavigationId, case when IsInternalLink = 1 then (select top 1 PageURl from ContentToolPage where PageId = ctn.PageId) else URL end as URL, Title, SortOrder from ContentToolNavigation ctn where ParentId in (select top 1 NavigationId from ContentToolPage where PageURL = " & Db.Quote(PageURL) & ") "
            SQL &= " order by Parent desc, SortOrder "

            Dim ds As DataSet = Db.GetDataSet(SQL)
            Return ds
        End Function

        Public Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("ContentToolNavigation.vb", func, ex)
        End Sub
    End Class

    Public MustInherit Class ContentToolNavigationRowBase
        Private m_DB As Database
        Private m_NavigationId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_ParentId As Integer = Nothing
        Private m_URL As String = Nothing
        Private m_Target As String = Nothing
        Private m_Level As Integer = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_IsHidden As Boolean = Nothing
        Public Shared cachePrefixKey As String = "ContentToolNavigation_"

        Public Property NavigationId() As Integer
            Get
                Return m_NavigationId
            End Get
            Set(ByVal Value As Integer)
                m_NavigationId = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = Value
            End Set
        End Property

        Public Property IsHidden() As Boolean
            Get
                Return m_IsHidden
            End Get
            Set(ByVal Value As Boolean)
                m_IsHidden = Value
            End Set
        End Property

        Public Property ParentId() As Integer
            Get
                Return m_ParentId
            End Get
            Set(ByVal Value As Integer)
                m_ParentId = Value
            End Set
        End Property

        Public Property URL() As String
            Get
                Return m_URL
            End Get
            Set(ByVal Value As String)
                m_URL = Value
            End Set
        End Property

        Public Property Target() As String
            Get
                Return m_Target
            End Get
            Set(ByVal Value As String)
                m_Target = Value
            End Set
        End Property

        Public Property Level() As Integer
            Get
                Return m_Level
            End Get
            Set(ByVal Value As Integer)
                m_Level = Value
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

        Public Sub New(ByVal database As Database, ByVal NavigationId As Integer)
            m_DB = database
            m_NavigationId = NavigationId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT NavigationID, [Title], [ParentId], IsHidden, [URL], [Target], [Level], [SortOrder] FROM ContentToolNavigation ctn WHERE NavigationId = " & DB.Quote(NavigationId)
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
            m_Title = Convert.ToString(r.Item("Title"))
            If IsDBNull(r.Item("NavigationId")) Then
                m_NavigationId = Nothing
            Else
                m_NavigationId = Convert.ToInt32(r.Item("NavigationId"))
            End If
            If IsDBNull(r.Item("ParentId")) Then
                m_ParentId = Nothing
            Else
                m_ParentId = Convert.ToInt32(r.Item("ParentId"))
            End If
            m_IsHidden = Convert.ToBoolean(r.Item("IsHidden"))
            If IsDBNull(r.Item("URL")) Then
                m_URL = Nothing
            Else
                m_URL = Convert.ToString(r.Item("URL"))
            End If
            If IsDBNull(r.Item("Target")) Then
                m_Target = Nothing
            Else
                m_Target = Convert.ToString(r.Item("Target"))
            End If
            If IsDBNull(r.Item("Level")) Then
                m_Level = Nothing
            Else
                m_Level = Convert.ToInt16(r.Item("Level"))
            End If
            If IsDBNull(r.Item("SortOrder")) Then
                m_SortOrder = Nothing
            Else
                m_SortOrder = Convert.ToInt16(r.Item("SortOrder"))
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "[sp_ContentToolNavigation_Insert]"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            db.AddOutParameter(cmd, "NavigationId", DbType.Int32, NavigationId)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "ParentId", DbType.Int64, ParentId)
            db.AddInParameter(cmd, "IsHidden", DbType.Boolean, IsHidden)
            db.AddInParameter(cmd, "Url", DbType.String, URL)
            db.AddInParameter(cmd, "Target", DbType.String, Target)
            db.AddInParameter(cmd, "Level", DbType.Int16, Level)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)

            Try
                db.ExecuteNonQuery(cmd)
                NavigationId = Convert.ToInt32(db.GetParameterValue(cmd, "NavigationId"))
            Catch ex As Exception

            End Try

            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Return NavigationId
        End Function

        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "[sp_ContentToolNavigation_Update]"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cmd, "NavigationId", DbType.Int64, NavigationId)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "ParentId", DbType.Int64, ParentId)
            db.AddInParameter(cmd, "IsHidden", DbType.Boolean, IsHidden)
            db.AddInParameter(cmd, "Url", DbType.String, URL)
            db.AddInParameter(cmd, "Target", DbType.String, Target)

            Try
                db.ExecuteNonQuery(cmd)
            Catch ex As Exception

            End Try

            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM ContentToolNavigation WHERE NavigationId = " & m_DB.Quote(NavigationId)
            m_DB.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub 'Remove
    End Class

    Public Class ContentToolNavigationCollection
        Inherits GenericCollection(Of ContentToolNavigationRow)
    End Class

End Namespace


