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

Namespace DataLayer
    Public Class DealDayRow
        Inherits DealDayRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DealDayId As Integer)
            MyBase.New(database, DealDayId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DealDayId As Integer) As DealDayRow
            Dim row As DealDayRow
            row = New DealDayRow(_Database, DealDayId)
            row.Load()

            Return row
        End Function
        Public Shared Function GetRowView(ByVal _Database As Database, ByVal currentDate As DateTime) As DealDayRow
            Dim DealDay As New DealDayRow(_Database)
            Dim reader As SqlDataReader = Nothing
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DealDay_GetRowView"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                Dim para As SqlParameter = New SqlParameter("currentDate", SqlDbType.DateTime)
                para.Value = currentDate
                cmd.Parameters.Add(para)
                reader = db.ExecuteReader(cmd)
                While reader.Read

                    If (Not reader.IsDBNull(reader.GetOrdinal("DealDayId"))) Then
                        DealDay.DealDayId = Convert.ToInt32(reader("DealDayId"))
                    Else
                        DealDay.DealDayId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        DealDay.Title = reader("Title").ToString()
                    Else
                        DealDay.Title = ""
                    End If
                    If IsDBNull(reader.Item("CreatedDate")) Then
                        DealDay.CreatedDate = Nothing
                    Else
                        DealDay.CreatedDate = Convert.ToDateTime(reader.Item("CreatedDate"))
                    End If
                    If IsDBNull(reader.Item("StartDate")) Then
                        DealDay.StartDate = Nothing
                    Else
                        DealDay.StartDate = Convert.ToDateTime(reader.Item("StartDate"))
                    End If
                    If IsDBNull(reader.Item("EndDate")) Then
                        DealDay.EndDate = Nothing
                    Else
                        DealDay.EndDate = Convert.ToDateTime(reader.Item("EndDate"))
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        DealDay.IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        DealDay.IsActive = True
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("BannerImage"))) Then
                        DealDay.BannerImage = reader("BannerImage").ToString()
                    Else
                        DealDay.BannerImage = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                        DealDay.Content = reader("Content").ToString()
                    Else
                        DealDay.Content = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        DealDay.MetaDescription = reader("MetaDescription").ToString()
                    Else
                        DealDay.MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                        DealDay.MetaKeyword = reader("MetaKeyword").ToString()
                    Else
                        DealDay.MetaKeyword = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        DealDay.PageTitle = reader("PageTitle").ToString()
                    Else
                        DealDay.PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        DealDay.ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        DealDay.ItemId = 0
                    End If

                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return DealDay
        End Function
        Public Shared Function ListAll(ByVal _Database As Database) As DealDayCollection
            Dim ss As New DealDayCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DealDay_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim DealDay As New DealDayRow(_Database)
                    DealDay.ItemId = CInt(dr("ItemId"))
                    DealDay.DealDayId = CInt(dr("DealDayId"))
                    DealDay.Title = dr("Title")
                    DealDay.StartDate = Convert.ToDateTime(dr("StartDate"))
                    DealDay.EndDate = Convert.ToDateTime(dr("EndDate"))
                    DealDay.IsActive = CBool(dr("IsActive"))
                    DealDay.BannerImage = dr("BannerImage")
                    DealDay.Content = dr("Content")
                    ss.Add(DealDay)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss

        End Function

        Public Shared Sub Delete(ByVal _Database As Database, ByVal DealDayId As Integer)
            Dim SQL As String = "exec dbo.sp_DealDay_Delete " & DealDayId
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub ChangeIsActive(ByVal _Database As Database, ByVal DealDayId As Integer)
            Dim SQL As String = "exec dbo.sp_DealDay_ChangeIsActive " & DealDayId
            _Database.ExecuteSQL(SQL)
        End Sub

    End Class


    Public MustInherit Class DealDayRowBase
        Private m_DB As Database
        Private m_DealDayId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_IsActive As Boolean = True
        Private m_BannerImage As String = True
        Private m_ItemId As Integer = Nothing
        Private m_Content As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing

        Public Property DealDayId() As Integer
            Get
                Return m_DealDayId
            End Get
            Set(ByVal Value As Integer)
                m_DealDayId = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
            End Set
        End Property
        Public Property StartDate() As DateTime
            Get
                Return m_StartDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartDate = Value
            End Set
        End Property
        Public Property EndDate() As DateTime
            Get
                Return m_EndDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndDate = Value
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
        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
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
        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property BannerImage() As String
            Get
                Return m_BannerImage
            End Get
            Set(ByVal Value As String)
                m_BannerImage = Value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
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

        Public Sub New(ByVal database As Database, ByVal DealDayId As Integer)
            m_DB = database
            m_DealDayId = DealDayId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM DealDay WHERE DealDayId = " & DB.Number(DealDayId)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("DealDayId"))) Then
                    m_DealDayId = Convert.ToInt32(reader("DealDayId"))
                Else
                    m_DealDayId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                    m_Title = reader("Title").ToString()
                Else
                    m_Title = ""
                End If
                If IsDBNull(reader.Item("CreatedDate")) Then
                    m_CreatedDate = Nothing
                Else
                    m_CreatedDate = Convert.ToDateTime(reader.Item("CreatedDate"))
                End If
                If IsDBNull(reader.Item("StartDate")) Then
                    m_StartDate = Nothing
                Else
                    m_StartDate = Convert.ToDateTime(reader.Item("StartDate"))
                End If
                If IsDBNull(reader.Item("EndDate")) Then
                    m_EndDate = Nothing
                Else
                    m_EndDate = Convert.ToDateTime(reader.Item("EndDate"))
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = True
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("BannerImage"))) Then
                    m_BannerImage = reader("BannerImage").ToString()
                Else
                    m_BannerImage = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                    m_Content = reader("Content").ToString()
                Else
                    m_Content = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    m_ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    m_ItemId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
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

            End If
        End Sub

        Public Overridable Function Insert() As Integer
            Dim SQL As String = ""

            SQL = " INSERT INTO DealDay (" _
             & "Title" _
             & ",CreatedDate" _
             & ",StartDate" _
             & ",EndDate" _
             & ",IsActive" _
             & ",BannerImage" _
             & ",ItemId" _
             & ",Content" _
              & ",MetaDescription" _
               & ",MetaKeyword" _
                 & ",PageTitle" _
             & ") VALUES (" _
             & "" & m_DB.Quote(Title) _
             & "," & m_DB.NullQuote(CreatedDate) _
             & "," & m_DB.NullQuote(StartDate) _
             & "," & m_DB.NullQuote(EndDate) _
             & "," & Convert.ToInt16(IsActive) _
             & "," & m_DB.Quote(BannerImage) _
             & "," & ItemId _
             & "," & m_DB.Quote(Content) _
              & "," & m_DB.Quote(MetaDescription) _
               & "," & m_DB.Quote(MetaKeyword) _
                   & "," & m_DB.Quote(PageTitle) _
             & ")"
            DealDayId = m_DB.InsertSQL(SQL)
            Return DealDayId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String
            SQL = " UPDATE DealDay SET " _
             & " Title = " & m_DB.Quote(Title) _
             & ",StartDate = " & m_DB.NullQuote(StartDate) _
             & ",EndDate = " & m_DB.NullQuote(EndDate) _
             & ",IsActive = " & Convert.ToInt16(IsActive) _
              & ",ItemId = " & ItemId _
             & ",BannerImage = " & m_DB.Quote(BannerImage) _
             & ",Content = " & m_DB.Quote(Content) _
               & ",MetaDescription = " & m_DB.Quote(MetaDescription) _
                 & ",MetaKeyword = " & m_DB.Quote(MetaKeyword) _
                    & ",PageTitle = " & m_DB.Quote(PageTitle) _
             & " WHERE DealDayId = " & CInt(DealDayId)
            m_DB.ExecuteSQL(SQL)
        End Sub
    End Class

    Public Class DealDayCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal DealDay As DealDayRow)
            Me.List.Add(DealDay)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DealDayRow
            Get
                Return CType(Me.List.Item(Index), DealDayRow)
            End Get

            Set(ByVal Value As DealDayRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace
