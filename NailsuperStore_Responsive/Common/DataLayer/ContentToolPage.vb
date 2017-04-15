Option Explicit On

'Author: Lam Le
'Date: 9/30/2009 10:10:06 AM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Imports System.Object
Imports System.IO
Imports Components
Namespace DataLayer


    Public Class ContentToolPageRow
        Inherits ContentToolPageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal PageId As Integer)
            MyBase.New(database, PageId)
        End Sub 'New

        Public Shared Function GetRow(ByVal _Database As Database, ByVal PageId As Integer) As ContentToolPageRow
            Dim key As String = String.Format("{0}GetRow_{1}", cachePrefixKey, PageId)
            Dim row As ContentToolPageRow = CType(CacheUtils.GetCache(key), ContentToolPageRow)
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If

            row = New ContentToolPageRow(_Database, PageId)
            row.Load()

            CacheUtils.SetCache(key, row)
            Return row
        End Function
        Public Shared Function GetSEODataByPageURL(ByVal _Database As Database, ByVal pageURL As String) As List(Of String)
            Dim key As String = String.Format("{0}GetSEODataByPageURL_{1}", cachePrefixKey, pageURL)
            Dim places As List(Of String) = CType(CacheUtils.GetCache(key), List(Of String))
            If Not places Is Nothing Then
                Return places
            End If
            places = New List(Of String)

            Dim pageTitle As String = String.Empty
            Dim metakeyword As String = String.Empty
            Dim metaDescription As String = String.Empty
            Dim dt As DataTable = _Database.GetDataTable("select Title,MetaDescription,MetaKeywords from ContentToolPage where PageURL='" & pageURL & "'")
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    pageTitle = dt.Rows(0)("Title").ToString()
                    metaDescription = dt.Rows(0)("MetaDescription").ToString()
                    metakeyword = dt.Rows(0)("MetaKeywords").ToString()
                End If
            End If
            places(0) = pageTitle
            places(1) = metakeyword
            places(2) = metaDescription
            CacheUtils.SetCache(key, places)
            Return places
        End Function
       
        Public Shared Function GetPageList(ByVal DB As Database) As DataSet
            Dim key As String = String.Format("{0}GetPageList", cachePrefixKey)
            Dim ds As DataSet = CType(CacheUtils.GetCache(key), DataSet)
            If Not ds Is Nothing AndAlso Not ds.Tables Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                Return ds
            End If
            ds = DB.GetDataSet("select PageId, Rtrim(name)+ '  (' + Rtrim(PageURL) + ')' as name, cts.SectionName from ContentToolPage ctp, ContentToolSection cts where ctp.Sectionid = cts.sectionid and ctp.PageURL is not null order by cts.Sectionname, ctp.Name")
            CacheUtils.SetCache(key, ds)
            Return ds
        End Function

        Public Shared Function GetRowByURL(ByVal URL As String) As ContentToolPageRow
            Dim key As String = String.Format("{0}GetRowByURL_{1}", cachePrefixKey, URL)
            Dim row As ContentToolPageRow = CType(CacheUtils.GetCache(key), ContentToolPageRow)
            If Not row Is Nothing Then
                Return row
            End If
            row = New ContentToolPageRow
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_ContentToolPage_GetObjectByUrl"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "PageUrl", DbType.String, URL)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    row = LoadByReader(dr)
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, row)
                Return row
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function

        Public Shared Function FullNavigationByPageUrl(ByVal URL As String) As String
            Dim key As String = String.Format("{0}FullNavigationByPageUrl_{1}", cachePrefixKey, URL)
            Dim result As String = CType(CacheUtils.GetCache(key), String)
            If result IsNot Nothing Then
                Return result
            End If

            result = String.Empty
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_ContentToolPage_FullNavigationByPageUrl"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "PageUrl", DbType.String, URL)
                result = db.ExecuteScalar(cmd)

                Core.CloseReader(dr)
                CacheUtils.SetCache(key, result)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function

        Public Shared Function Insert(ByVal objPage As ContentToolPageRow) As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ContentToolPage_Insert")
                db.AddInParameter(cmd, "PageURL", DbType.String, objPage.PageURL)
                db.AddInParameter(cmd, "Name", DbType.String, objPage.Name)
                db.AddInParameter(cmd, "Title", DbType.String, objPage.Title)
                db.AddInParameter(cmd, "MetaTitle", DbType.String, objPage.MetaTitle)
                db.AddInParameter(cmd, "IsShowContent", DbType.Boolean, objPage.IsShowContent)
                db.AddInParameter(cmd, "IsFullScreen", DbType.Boolean, objPage.IsFullScreen)
                db.AddInParameter(cmd, "Content", DbType.String, objPage.Content)
                db.AddInParameter(cmd, "IsIndexed", DbType.Boolean, objPage.IsIndexed)
                db.AddInParameter(cmd, "IsFollowed", DbType.Boolean, objPage.IsFollowed)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, objPage.MetaDescription)
                db.AddInParameter(cmd, "MetaKeywords", DbType.String, objPage.MetaKeywords)
                db.AddInParameter(cmd, "NavigationId", DbType.Int32, objPage.NavigationId)
                db.AddInParameter(cmd, "NavigationText", DbType.String, objPage.NavigationText)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                '------------------------------------------------------------------------
                Return result
            Catch ex As Exception
                Core.LogError("ContentToolPage.vb", "Insert", ex)
            End Try
            Return 0
        End Function

        Public Shared Function Update(ByVal objPage As ContentToolPageRow) As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ContentToolPage_Update")
                db.AddInParameter(cmd, "PageId", DbType.Int32, objPage.PageId)
                db.AddInParameter(cmd, "PageURL", DbType.String, objPage.PageURL)
                db.AddInParameter(cmd, "Name", DbType.String, objPage.Name)
                db.AddInParameter(cmd, "Title", DbType.String, objPage.Title)
                db.AddInParameter(cmd, "MetaTitle", DbType.String, objPage.MetaTitle)
                db.AddInParameter(cmd, "IsShowContent", DbType.Boolean, objPage.IsShowContent)
                db.AddInParameter(cmd, "IsFullScreen", DbType.Boolean, objPage.IsFullScreen)
                db.AddInParameter(cmd, "Content", DbType.String, objPage.Content)
                db.AddInParameter(cmd, "IsIndexed", DbType.Boolean, objPage.IsIndexed)
                db.AddInParameter(cmd, "IsFollowed", DbType.Boolean, objPage.IsFollowed)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, objPage.MetaDescription)
                db.AddInParameter(cmd, "MetaKeywords", DbType.String, objPage.MetaKeywords)
                db.AddInParameter(cmd, "NavigationId", DbType.Int32, objPage.NavigationId)
                db.AddInParameter(cmd, "NavigationText", DbType.String, objPage.NavigationText)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                '------------------------------------------------------------------------
                Return result
            Catch ex As Exception
                Core.LogError("ContentToolPage.vb", "Update", ex)
            End Try
            Return 0
        End Function

        Public Shared Function Delete(ByVal pageId As Integer) As Boolean
            If pageId < 1 Then
                Return False
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ContentToolPage_Delete")
                db.AddInParameter(cmd, "PageId", DbType.Int32, pageId)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return result
            Catch ex As Exception
                Core.LogError("ContentToolPage.vb", "Delete(ByVal pageId As Integer:" & pageId & ")", ex)
            End Try
            Return 0
        End Function

        Private Shared Function LoadByReader(ByVal reader As SqlDataReader) As ContentToolPageRow
            Dim objPage As New ContentToolPageRow
            If (Not reader.IsDBNull(reader.GetOrdinal("PageId"))) Then
                objPage.PageId = Convert.ToInt32(reader("PageId"))
            Else
                objPage.PageId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("PageURL"))) Then
                objPage.PageURL = reader("PageURL").ToString()
            Else
                objPage.PageURL = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                objPage.Name = reader("Name").ToString()
            Else
                objPage.Name = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                objPage.Title = reader("Title").ToString()
            Else
                objPage.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("MetaTitle"))) Then
                objPage.MetaTitle = reader("MetaTitle").ToString()
            Else
                objPage.MetaTitle = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsShowContent"))) Then
                objPage.IsShowContent = Convert.ToBoolean(reader("IsShowContent"))
            Else
                objPage.IsShowContent = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                objPage.Content = reader("Content").ToString()
            Else
                objPage.Content = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsIndexed"))) Then
                objPage.IsIndexed = Convert.ToBoolean(reader("IsIndexed"))
            Else
                objPage.IsIndexed = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsFollowed"))) Then
                objPage.IsFollowed = Convert.ToBoolean(reader("IsFollowed"))
            Else
                objPage.IsFollowed = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("IsFullScreen"))) Then
                objPage.IsFullScreen = Convert.ToBoolean(reader("IsFullScreen"))
            Else
                objPage.IsFullScreen = False
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                objPage.MetaDescription = reader("MetaDescription").ToString()
            Else
                objPage.MetaDescription = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                objPage.MetaKeywords = reader("MetaKeywords").ToString()
            Else
                objPage.MetaKeywords = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NavigationId"))) Then
                objPage.NavigationId = Convert.ToInt32(reader("NavigationId"))
            Else
                objPage.NavigationId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NavigationText"))) Then
                objPage.NavigationText = reader("NavigationText").ToString()
            Else
                objPage.NavigationText = ""
            End If
           
            Return objPage
        End Function 'Load

       
    End Class

    Public MustInherit Class ContentToolPageRowBase
        Private m_DB As Database
        Private m_PageId As Integer = Nothing
        Private m_PageURL As String = Nothing
        Private m_Title As String = Nothing
        Private m_Name As String = Nothing
        Private m_IsShowContent As Boolean = Nothing
        Private m_Content As String = Nothing
        Private m_IsIndexed As Boolean = Nothing
        Private m_IsFollowed As Boolean = Nothing
        Private m_IsFullScreen As Boolean = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaTitle As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_ModifiedDate As DateTime = Nothing
        Private m_NavigationId As Integer = Nothing
        Private m_NavigationText As String = Nothing
        Public Shared cachePrefixKey As String = "ContentToolPage_"

        Public Property PageId() As Integer
            Get
                Return m_PageId
            End Get
            Set(ByVal Value As Integer)
                m_PageId = Value
            End Set
        End Property

        Public Property PageURL() As String
            Get
                Return m_PageURL
            End Get
            Set(ByVal Value As String)
                m_PageURL = Value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return m_Content
            End Get
            Set(ByVal Value As String)
                m_Content = Value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property IsShowContent() As Boolean
            Get
                Return m_IsShowContent
            End Get
            Set(ByVal Value As Boolean)
                m_IsShowContent = Value
            End Set
        End Property

        Public Property IsIndexed() As Boolean
            Get
                Return m_IsIndexed
            End Get
            Set(ByVal Value As Boolean)
                m_IsIndexed = Value
            End Set
        End Property

        Public Property IsFollowed() As Boolean
            Get
                Return m_IsFollowed
            End Get
            Set(ByVal Value As Boolean)
                m_IsFollowed = Value
            End Set
        End Property

        Public Property IsFullScreen() As Boolean
            Get
                Return m_IsFullScreen
            End Get
            Set(ByVal Value As Boolean)
                m_IsFullScreen = Value
            End Set
        End Property

        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
            End Set
        End Property

        Public Property MetaTitle() As String
            Get
                Return m_MetaTitle
            End Get
            Set(ByVal Value As String)
                m_MetaTitle = Value
            End Set
        End Property

        Public Property MetaKeywords() As String
            Get
                Return m_MetaKeywords
            End Get
            Set(ByVal Value As String)
                m_MetaKeywords = Value
            End Set
        End Property

        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal Value As DateTime)
                m_ModifiedDate = Value
            End Set
        End Property

        Public Property NavigationId() As Integer
            Get
                Return m_NavigationId
            End Get
            Set(ByVal Value As Integer)
                m_NavigationId = Value
            End Set
        End Property

        Public Property NavigationText() As String
            Get
                Return m_NavigationText
            End Get
            Set(ByVal Value As String)
                m_NavigationText = Value
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

        Public Sub New(ByVal database As Database, ByVal PageId As Integer)
            m_DB = database
            m_PageId = PageId
        End Sub 'New

        Public Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_CONTENTTOOLPAGE_GETOBJECT As String = "sp_ContentToolPage_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTENTTOOLPAGE_GETOBJECT)
                db.AddInParameter(cmd, "PageId", DbType.Int32, PageId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Components.Core.CloseReader(reader)
            Catch ex As Exception
                Components.Core.CloseReader(reader)
            End Try
        End Sub

        Public Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("PageId"))) Then
                    m_PageId = Convert.ToInt32(reader("PageId"))
                Else
                    m_PageId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("PageURL"))) Then
                    m_PageURL = reader("PageURL").ToString()
                Else
                    m_PageURL = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                    m_Title = reader("Title").ToString()
                Else
                    m_Title = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaTitle"))) Then
                    m_MetaTitle = reader("MetaTitle").ToString()
                Else
                    m_MetaTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsShowContent"))) Then
                    m_IsShowContent = Convert.ToBoolean(reader("IsShowContent"))
                Else
                    m_IsShowContent = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                    m_Content = reader("Content").ToString()
                Else
                    m_Content = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsIndexed"))) Then
                    m_IsIndexed = Convert.ToBoolean(reader("IsIndexed"))
                Else
                    m_IsIndexed = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsFollowed"))) Then
                    m_IsFollowed = Convert.ToBoolean(reader("IsFollowed"))
                Else
                    m_IsFollowed = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsFullScreen"))) Then
                    m_IsFullScreen = Convert.ToBoolean(reader("IsFullScreen"))
                Else
                    m_IsFullScreen = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                    m_MetaKeywords = reader("MetaKeywords").ToString()
                Else
                    m_MetaKeywords = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                    m_ModifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                Else
                    m_ModifiedDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NavigationId"))) Then
                    m_NavigationId = Convert.ToInt32(reader("NavigationId"))
                Else
                    m_NavigationId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NavigationText"))) Then
                    m_NavigationText = reader("NavigationText").ToString()
                Else
                    m_NavigationText = ""
                End If

            End If
        End Sub 'Load






    End Class

    Public Class ContentToolPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Page As ContentToolPageRow)
            Me.List.Add(Page)
        End Sub

        Public Function Contains(ByVal Page As ContentToolPageRow) As Boolean
            Return Me.List.Contains(Page)
        End Function

        Public Function IndexOf(ByVal Page As ContentToolPageRow) As Integer
            Return Me.List.IndexOf(Page)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Page As ContentToolPageRow)
            Me.List.Insert(Index, Page)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ContentToolPageRow
            Get
                Return CType(Me.List.Item(Index), ContentToolPageRow)
            End Get

            Set(ByVal Value As ContentToolPageRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Page As ContentToolPageRow)
            Me.List.Remove(Page)
        End Sub
    End Class

End Namespace


