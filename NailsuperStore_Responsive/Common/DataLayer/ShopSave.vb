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
    Public Class ShopSaveRow
        Inherits ShopSaveRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            MyBase.New(database, DepartmentId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ShopSaveId As Integer) As ShopSaveRow

            Dim row As ShopSaveRow

            row = New ShopSaveRow(_Database, ShopSaveId)
            row.Load()

            Return row
        End Function
        Public Shared Function ListShopSave(ByVal _Database As Database, ByRef topRecord As String) As ShopSaveCollection
            Dim key As String = String.Format("ShopSave_Load_{0}", topRecord)
            Dim tabs As New ShopSaveCollection
            tabs = CType(CacheUtils.GetCache(key), ShopSaveCollection)
            If Not tabs Is Nothing Then
                Return tabs
            Else
                tabs = New ShopSaveCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopSave_Load"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "topRecord", DbType.String, topRecord)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New ShopSaveRow(_Database)
                    tab.Type = CInt(dr("Type"))
                    tab.ShopSaveId = CInt(dr("ShopSaveId"))
                    tab.Name = dr("Name")
                    If (Not dr.IsDBNull(dr.GetOrdinal("HomeBanner"))) Then
                        tab.HomeBanner = dr("HomeBanner").ToString()
                    Else
                        tab.HomeBanner = ""
                    End If
                    If (Not dr.IsDBNull(dr.GetOrdinal("ShortContent"))) Then
                        tab.ShortContent = dr("ShortContent").ToString()
                    Else
                        tab.Content = ""
                    End If
                    If (Not dr.IsDBNull(dr.GetOrdinal("Url"))) Then
                        tab.Url = dr("Url").ToString()
                    Else
                        tab.Url = ""
                    End If
                    tabs.Add(tab)
                End While
                Core.CloseReader(dr)
                tabs.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                CacheUtils.SetCache(key, tabs, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Shopsave.vb-ListShopSave", ex.ToString())
            End Try

            Return tabs
        End Function

        Public Shared Function ListByType(ByVal _Database As Database, ByVal Type As String, ByVal IsActive As Integer) As ShopSaveCollection
            Dim key As String = String.Format("ShopSave_ListByType_{0}_{1}", Type, IsActive)
            Dim tabs As New ShopSaveCollection
            tabs = CType(CacheUtils.GetCache(key), ShopSaveCollection)
            If Not tabs Is Nothing Then
                Return tabs
            Else
                tabs = New ShopSaveCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopSave_ListByType"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Type", DbType.String, Type)
                db.AddInParameter(cmd, "IsActive", DbType.Int32, IsActive)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New ShopSaveRow(_Database)
                    tab.Type = CInt(dr("Type"))
                    tab.ShopSaveId = CInt(dr("ShopSaveId"))
                    tab.Arrange = CInt(dr("Arrange"))
                    tab.IsActive = CBool(dr("IsActive"))
                    tab.IsTab = CBool(dr("IsTab"))
                    tab.IsHtml = CBool(dr("IsHtml"))
                    tab.Name = dr("Name")
                    If (Not dr.IsDBNull(dr.GetOrdinal("HomeBanner"))) Then
                        tab.HomeBanner = dr("HomeBanner").ToString()
                    Else
                        tab.HomeBanner = ""
                    End If
                    If (Not dr.IsDBNull(dr.GetOrdinal("Content"))) Then
                        tab.Content = dr("Content").ToString()
                    Else
                        tab.Content = ""
                    End If
                    If (Not dr.IsDBNull(dr.GetOrdinal("ShortContent"))) Then
                        tab.ShortContent = dr("ShortContent").ToString()
                    Else
                        tab.ShortContent = ""
                    End If
                    If (Not dr.IsDBNull(dr.GetOrdinal("Url"))) Then
                        tab.Url = dr("Url").ToString()
                    Else
                        tab.Url = ""
                    End If
                    'stab.DepartmentName = dr("DepartmentName")
                    tabs.Add(tab)

                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, tabs, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Shopsave.vb-ListByType", "Type:=" & Type & "<br>Exception: " & ex.ToString() + "")

            End Try

            Return tabs

        End Function


        Public Shared Function ListAll(ByVal _Database As Database) As ShopSaveCollection
            Dim key As String = "ShopSave_ListAll"
            Dim ss As New ShopSaveCollection
            ss = CType(CacheUtils.GetCache(key), ShopSaveCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New ShopSaveCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopSave_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim shopsave As New ShopSaveRow(_Database)
                    shopsave.Type = CInt(dr("Type"))
                    shopsave.ShopSaveId = CInt(dr("ShopSaveId"))
                    shopsave.Arrange = CInt(dr("Arrange"))
                    shopsave.ArrangeTab = CInt(dr("ArrangeTab"))
                    shopsave.IsActive = CBool(dr("IsActive"))
                    shopsave.Name = dr("Name")
                    shopsave.IsTab = CBool(dr("IsTab"))
                    ss.Add(shopsave)
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Shopsave.vb-ListAll", "<br>Exception: " & ex.ToString() + "")

            End Try

            Return ss

        End Function

        Public Shared Function ListByIsTab(ByVal _Database As Database, ByVal IsTab As Boolean) As ShopSaveCollection
            Dim tabs As New ShopSaveCollection
            Dim key As String = "ShopSave_ListByIsTab_" + IsTab.ToString()
            tabs = CType(CacheUtils.GetCache(key), ShopSaveCollection)
            If Not tabs Is Nothing Then
                Return tabs
            Else
                tabs = New ShopSaveCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopSave_ListByIsTab"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "IsTab", DbType.Boolean, IsTab)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New ShopSaveRow(_Database)
                    tab.ShopSaveId = CInt(dr("ShopSaveId"))
                    tab.IsHtml = CBool(dr("IsHtml"))
                    tab.Name = dr("Name")
                    tabs.Add(tab)
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, tabs)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Shopsave.vb-ListByIsTab", "IsTab:" & IsTab.ToString() & "<br>Exception: " & ex.ToString() + "")

            End Try

            Return tabs

        End Function

        Public Shared Sub Delete(ByVal _Database As Database, ByVal ShopSaveId As Integer)
            Try
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Dim SQL As String = "exec dbo.sp_ShopSave_Delete " & ShopSaveId
                _Database.ExecuteSQL(SQL)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-Delete", "ShopSaveId:" & ShopSaveId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub

        Public Shared Sub ChangeIsActive(ByVal _Database As Database, ByVal ShopSaveId As Integer)
            Try
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Dim SQL As String = "exec dbo.sp_ShopSave_ChangeIsActive " & ShopSaveId
                _Database.ExecuteSQL(SQL)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-ChangeIsActive", "ShopSaveId:" & ShopSaveId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub

        Public Shared Sub ChangeArrangeTab(ByVal _Database As Database, ByVal ShopSaveId As Integer, ByVal IsUp As Boolean)
            Try
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Dim SQL As String = "exec dbo.sp_ShopSave_ChangeArrangeTab " & ShopSaveId & ", " & IsUp
                _Database.ExecuteSQL(SQL)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-ChangeArrangeTab", "ShopSaveId:" & ShopSaveId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub

        Public Shared Sub ChangeArrange(ByVal _Database As Database, ByVal ShopSaveId As Integer, ByVal IsUp As Boolean)
            Try
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Dim SQL As String = "exec dbo.sp_ShopSave_ChangeArrange " & ShopSaveId & ", " & IsUp
                _Database.ExecuteSQL(SQL)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-ChangeArrange", "ShopSaveId:" & ShopSaveId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub
        Public Shared Sub ChangeIsTab(ByVal _Database As Database, ByVal ShopSaveId As Integer)
            Try
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Dim SQL As String = "exec dbo.sp_ShopSave_ChangeIsTab " & ShopSaveId
                _Database.ExecuteSQL(SQL)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-ChangeIsTab", "ShopSaveId:" & ShopSaveId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub
    End Class


    Public MustInherit Class ShopSaveRowBase
        Private m_DB As Database
        Private m_ShopSaveId As Integer = Nothing
        Private m_Type As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_ArrangeTab As Integer = Nothing
        Private m_IsActive As Boolean = True
        Private m_IsTab As Boolean = True
        Private m_IsHtml As Boolean = False
        Private m_Name As String = Nothing
        Private m_ShortContent As String = Nothing
        Private m_Content As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_OutsideUSPageTitle As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_OutsideUSMetaDescription As String = Nothing
        Private m_DepartmentName As String = Nothing
        Private m_Url As String = Nothing
        Private m_Banner As String = Nothing
        Private m_MobileBanner As String = Nothing
        Private m_HomeBanner As String = Nothing

        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal Value As Integer)
                m_Type = Value
            End Set
        End Property

        Public Property ShopSaveId() As Integer
            Get
                Return m_ShopSaveId
            End Get
            Set(ByVal Value As Integer)
                m_ShopSaveId = Value
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

        Public Property ArrangeTab() As Integer
            Get
                Return m_ArrangeTab
            End Get
            Set(ByVal Value As Integer)
                m_ArrangeTab = Value
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

        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(ByVal Value As String)
                m_Url = Value
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

        Public Property Content() As String
            Get
                Return m_Content
            End Get
            Set(ByVal Value As String)
                m_Content = Value
            End Set
        End Property
        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property OutsideUSPageTitle() As String
            Get
                Return m_OutsideUSPageTitle
            End Get
            Set(ByVal Value As String)
                m_OutsideUSPageTitle = Value
            End Set
        End Property
        Public Property MetaKeyword() As String
            Get
                Return m_MetaKeyword
            End Get
            Set(ByVal Value As String)
                m_MetaKeyword = Value
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
        Public Property OutsideUSMetaDescription() As String
            Get
                Return m_OutsideUSMetaDescription
            End Get
            Set(ByVal Value As String)
                m_OutsideUSMetaDescription = Value
            End Set
        End Property
        Public Property DepartmentName() As String
            Get
                Return m_DepartmentName
            End Get
            Set(ByVal Value As String)
                m_DepartmentName = Value
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

        Public Property IsTab() As Boolean
            Get
                Return m_IsTab
            End Get
            Set(ByVal Value As Boolean)
                m_IsTab = Value
            End Set
        End Property

        Public Property IsHtml() As Boolean
            Get
                Return m_IsHtml
            End Get
            Set(ByVal Value As Boolean)
                m_IsHtml = Value
            End Set
        End Property
        Public Property Banner() As String
            Get
                Return m_Banner
            End Get
            Set(ByVal Value As String)
                m_Banner = Value
            End Set
        End Property
        Public Property MobileBanner() As String
            Get
                Return m_MobileBanner
            End Get
            Set(ByVal Value As String)
                m_MobileBanner = Value
            End Set
        End Property
        Public Property HomeBanner() As String
            Get
                Return m_HomeBanner
            End Get
            Set(ByVal Value As String)
                m_HomeBanner = Value
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

        Public Sub New(ByVal database As Database, ByVal ShopSaveId As Integer)
            m_DB = database
            m_ShopSaveId = ShopSaveId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "SELECT * FROM ShopSave WHERE ShopSaveId = " & ShopSaveId
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                r = db.ExecuteReader(cmd)
                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Load", ex.ToString())
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ShopSaveId"))) Then
                        m_ShopSaveId = Convert.ToInt32(reader("ShopSaveId"))
                    Else
                        m_ShopSaveId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        m_Type = Convert.ToInt16(reader("Type"))
                    Else
                        m_Type = 1
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        m_Name = reader("Name").ToString()
                    Else
                        m_Name = ""
                    End If


                    If (Not reader.IsDBNull(reader.GetOrdinal("Url"))) Then
                        m_Url = reader("Url").ToString()
                    Else
                        m_Url = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                        m_MetaKeyword = reader("MetaKeyword").ToString()
                    Else
                        m_MetaKeyword = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSPageTitle"))) Then
                        m_OutsideUSPageTitle = reader("OutsideUSPageTitle").ToString()
                    Else
                        m_OutsideUSPageTitle = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        m_MetaDescription = reader("MetaDescription").ToString()
                    Else
                        m_MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSMetaDescription"))) Then
                        m_OutsideUSMetaDescription = reader("OutsideUSMetaDescription").ToString()
                    Else
                        m_OutsideUSMetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                        m_Arrange = Convert.ToInt16(reader("Arrange"))
                    Else
                        m_Arrange = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ArrangeTab"))) Then
                        m_ArrangeTab = Convert.ToInt16(reader("ArrangeTab"))
                    Else
                        m_ArrangeTab = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsTab"))) Then
                        m_IsTab = Convert.ToBoolean(reader("IsTab"))
                    Else
                        m_IsTab = True
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsHtml"))) Then
                        m_IsHtml = Convert.ToBoolean(reader("IsHtml"))
                    Else
                        m_IsHtml = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ShortContent"))) Then
                        m_ShortContent = reader("ShortContent").ToString()
                    Else
                        m_ShortContent = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                        m_Content = reader("Content").ToString()
                    Else
                        m_Content = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Banner"))) Then
                        m_Banner = reader("Banner").ToString()
                    Else
                        m_Banner = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MobileBanner"))) Then
                        m_MobileBanner = reader("MobileBanner").ToString()
                    Else
                        m_MobileBanner = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("HomeBanner"))) Then
                        m_HomeBanner = reader("HomeBanner").ToString()
                    Else
                        m_HomeBanner = ""
                    End If
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Public Overridable Function Insert() As Integer
            Try
                Dim SQL As String = ""

                SQL = " INSERT INTO ShopSave (" _
                 & "Type" _
                 & ",Name" _
                 & ",Arrange" _
                 & ",ArrangeTab" _
                 & ",IsActive" _
                 & ",IsTab" _
                 & ",IsHtml" _
                 & ",CreatedDate" _
                 & ",ModifiedDate" _
                  & ",PageTitle" _
                 & ",MetaKeyword" _
                 & ",MetaDescription" _
                 & ",ShortContent" _
                 & ",Content" _
                 & ",Url" _
                 & ",OutsideUSPageTitle" _
                 & ",OutsideUSMetaDescription" _
                 & ",Banner" _
                  & ",MobileBanner" _
                 & ",HomeBanner" _
                 & ") VALUES (" _
                 & "" & m_DB.Quote(Type) _
                 & "," & m_DB.NQuote(Name) _
                 & ", ISNULL((SELECT Max(Arrange)+1 FROM ShopSave WHERE [Type] = " & m_DB.Quote(Type) & "), '0')" _
                 & ", ISNULL((SELECT Max(ArrangeTab)+1 FROM ShopSave), '0')" _
                 & "," & CInt(IsActive) _
                 & "," & CInt(IsTab) _
                 & "," & CInt(IsHtml) _
                 & "," & m_DB.NullQuote(DateTime.Now) _
                 & "," & m_DB.NullQuote(DateTime.Now) _
                  & "," & m_DB.Quote(PageTitle) _
                 & "," & m_DB.Quote(MetaKeyword) _
                 & "," & m_DB.Quote(MetaDescription) _
                 & "," & m_DB.NQuote(ShortContent) _
                 & "," & m_DB.NQuote(Content) _
                 & "," & m_DB.NQuote(Url) _
                 & "," & m_DB.Quote(OutsideUSPageTitle) _
                 & "," & m_DB.Quote(OutsideUSMetaDescription) _
                 & "," & m_DB.Quote(Banner) _
                 & "," & m_DB.Quote(MobileBanner) _
                 & "," & m_DB.Quote(HomeBanner) _
                 & ")"


                ShopSaveId = m_DB.InsertSQL(SQL)
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
                Return ShopSaveId
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-Insert", "<br>Exception: " & ex.ToString() + "")

            End Try
            Return 0
        End Function

        Public Overridable Sub Update()
            Try
                Dim SQL As String

                SQL = " UPDATE ShopSave SET " _
                 & " Name = " & m_DB.Quote(Name) _
                  & ",Url = " & m_DB.Quote(Url) _
                 & ",IsActive = " & CInt(IsActive) _
                 & ",IsTab = " & CInt(IsTab) _
                 & ",IsHtml = " & CInt(IsHtml) _
                 & ",ShortContent = " & m_DB.Quote(ShortContent) _
                 & ",Content = " & m_DB.Quote(Content) _
                    & ",PageTitle = " & m_DB.Quote(PageTitle) _
                 & ",MetaKeyword = " & m_DB.Quote(MetaKeyword) _
                 & ",MetaDescription = " & m_DB.Quote(MetaDescription) _
                 & ",Type = " & CInt(Type) _
                 & ",ModifiedDate = " & m_DB.Quote(DateTime.Now.ToString()) _
                 & ",OutsideUSPageTitle = " & m_DB.Quote(OutsideUSPageTitle) _
                 & ",OutsideUSMetaDescription = " & m_DB.Quote(OutsideUSMetaDescription) _
                 & ",Banner = " & m_DB.Quote(Banner) _
                      & ",MobileBanner = " & m_DB.Quote(MobileBanner) _
                 & ",HomeBanner = " & m_DB.Quote(HomeBanner) _
                 & " WHERE ShopSaveId = " & CInt(ShopSaveId)

                m_DB.ExecuteSQL(SQL)
                CacheUtils.ClearCacheWithPrefix("ShopSave_")
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Shopsave.vb-Update", "<br>Exception: " & ex.ToString() + "")

            End Try
          

    

        End Sub
    End Class

    Public Class ShopSaveCollection
        Inherits CollectionBase
        Public TotalRecords As Integer = Nothing
        Public Sub New()
        End Sub

        Public Sub Add(ByVal ShopSave As ShopSaveRow)
            Me.List.Add(ShopSave)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ShopSaveRow
            Get
                Return CType(Me.List.Item(Index), ShopSaveRow)
            End Get

            Set(ByVal Value As ShopSaveRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace
