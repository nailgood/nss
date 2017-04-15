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
    Public Class DepartmentTabRow
        Inherits DepartmentTabRowBase

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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DepartmentTabId As Integer) As DepartmentTabRow
            Dim row As DepartmentTabRow

            row = New DepartmentTabRow(_Database, DepartmentTabId)
            row.Load()

            Return row
        End Function

        Public Shared Function ListByDepartmentId(ByVal _Database As Database, ByVal DepartmentId As Integer) As DepartmentTabCollection


            Dim tabs As New DepartmentTabCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_ListByDepartmentId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New DepartmentTabRow(_Database)
                    tab.DepartmentId = CInt(dr("DepartmentId"))
                    tab.DepartmentTabId = CInt(dr("DepartmentTabId"))
                    tab.Arrange = CInt(dr("Arrange"))
                    tab.IsActive = CBool(dr("IsActive"))
                    tab.Name = dr("Name")
                    tab.DepartmentName = dr("DepartmentName")
                    tabs.Add(tab)

                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return tabs

        End Function

        Public Shared Function FrontEndListByDepartmentId(ByVal _Database As Database, ByVal DepartmentId As Integer) As DepartmentTabCollection

            Dim tabs As New DepartmentTabCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_FrontEndListByDepartmentId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New DepartmentTabRow(_Database)
                    tab.DepartmentId = CInt(dr("DepartmentId"))
                    tab.DepartmentTabId = CInt(dr("DepartmentTabId"))
                    tab.Arrange = CInt(dr("Arrange"))
                    tab.Name = dr("Name")
                    tab.URLCode = dr("URLCode")
                    tabs.Add(tab)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return tabs

        End Function
        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("Admin.vb", func, ex)
        End Sub
        Public Shared Function FrontEndListOnMobileByDepartmentId(ByVal _Database As Database, ByVal DepartmentId As Integer) As DepartmentTabCollection
            Dim dr As SqlDataReader = Nothing
            Dim tabs As New DepartmentTabCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_FrontEndListOnMobileByDepartmentId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New DepartmentTabRow()
                    tab.DepartmentId = CInt(dr("DepartmentId"))
                    tab.DepartmentTabId = CInt(dr("DepartmentTabId"))
                    tab.Arrange = CInt(dr("Arrange"))
                    tab.Name = dr("Name")
                    tab.URLCode = dr("URLCode")
                    tabs.Add(tab)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                ''send mail log here
                SendMailLog("FrontEndListOnMobileByDepartmentId(DepartmentId=" & DepartmentId & ")", ex)
            End Try
            Return tabs
        End Function
        Public Shared Function Delete(ByVal DepartmentTabId As Integer) As Boolean
            'Dim SQL As String = "exec dbo.sp_DepartmentTab_Delete " & DepartmentTabId
            '_Database.ExecuteSQL(SQL)
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_Delete"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentTabId", DbType.Int32, DepartmentTabId)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            Catch ex As Exception
                Email.SendError("ToError500", "DepartmentTab.vb- Delete", "DepartmentTabId=" & DepartmentTabId & ",Exception: " & ex.ToString())
            End Try
            If (result > 0) Then
                Return True
            End If
            Return False
        End Function

        Public Shared Sub ChangeIsActive(ByVal _Database As Database, ByVal DepartmentTabId As Integer)
            Dim SQL As String = "exec dbo.sp_DepartmentTab_ChangeIsActive " & DepartmentTabId
            _Database.ExecuteSQL(SQL)
        End Sub
        Public Shared Sub ChangeArrange(ByVal _Database As Database, ByVal DepartmentTabId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_DepartmentTab_ChangeArrange " & DepartmentTabId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
        End Sub
        Protected Shared Function LoadByDataReader(ByVal reader As SqlDataReader) As DepartmentTabRow

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                Dim result As New DepartmentTabRow
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentTabId"))) Then
                    result.DepartmentTabId = Convert.ToInt32(reader("DepartmentTabId"))
                Else
                    result.DepartmentTabId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                    result.DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                Else
                    result.DepartmentId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                    result.Arrange = Convert.ToInt16(reader("Arrange"))
                Else
                    result.Arrange = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    result.IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    result.IsActive = True
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    result.MetaDescription = reader("MetaDescription").ToString()
                Else
                    result.MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSMetaDescription"))) Then
                    result.OutsideUSMetaDescription = reader("OutsideUSMetaDescription").ToString()
                Else
                    result.OutsideUSMetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                    result.MetaKeyword = reader("MetaKeyword").ToString()
                Else
                    result.MetaKeyword = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                    result.PageTitle = reader("PageTitle").ToString()
                Else
                    result.PageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OutsideUSPageTitle"))) Then
                    result.OutsideUSPageTitle = reader("OutsideUSPageTitle").ToString()
                Else
                    result.OutsideUSPageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                    result.URLCode = reader("URLCode").ToString()
                Else
                    result.URLCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    result.Description = reader("Description").ToString()
                Else
                    result.Description = ""
                End If
                Return result
            End If
            Return Nothing
        End Function
        Public Shared Function GetByURLCode(ByVal m_DB As Database, ByVal code As String, ByVal departmentId As Integer) As DepartmentTabRow

            Dim tabs As New DepartmentTabRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM DepartmentTab WHERE URLCode = '" & code & "' and DepartmentId=" & departmentId
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    tabs = LoadByDataReader(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

            Return tabs
        End Function
        Public Shared Function GetByURLCode(ByVal code As String, ByVal departmentId As Integer) As DepartmentTabRow

            Dim tabs As New DepartmentTabRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM DepartmentTab WHERE URLCode = '" & code & "' and DepartmentId=" & departmentId
                Dim db1 As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db1.GetSqlStringCommand(SQL)
                r = db1.ExecuteReader(cmd)
                If r.Read Then
                    tabs = LoadByDataReader(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

            Return tabs
        End Function

        Public Shared Function FullByTabURLCode(ByVal TabURLCode As String, ByVal DepartmentId As Integer)
            Dim result As String
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_FullByTabURLCode"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                db.AddInParameter(cmd, "URLCode", DbType.String, TabURLCode)
                result = db.ExecuteScalar(cmd)
            Catch ex As Exception

            End Try
            Return result
        End Function
    End Class


    Public MustInherit Class DepartmentTabRowBase
        Private m_DB As Database
        Private m_DepartmentTabId As Integer = Nothing
        Private m_DepartmentId As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_IsActive As Boolean = True
        Private m_Name As String = Nothing
        Private m_DepartmentName As String = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_OutsideUSMetaDescription As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_OutsideUSPageTitle As String = Nothing
        Private m_URLCode As String = Nothing
        Private m_Image As String = Nothing
        Private m_Description As String = Nothing
        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentId = Value
            End Set
        End Property

        Public Property DepartmentTabId() As Integer
            Get
                Return m_DepartmentTabId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentTabId = Value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property
        Public Property URLCode() As String
            Get
                Return m_URLCode
            End Get
            Set(ByVal Value As String)
                m_URLCode = Value
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
        Public Property OutsideUSPageTitle() As String
            Get
                Return m_OutsideUSPageTitle
            End Get
            Set(ByVal Value As String)
                m_OutsideUSPageTitle = Value
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

        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
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

        Public Sub New(ByVal database As Database, ByVal DepartmentTabId As Integer)
            m_DB = database
            m_DepartmentTabId = DepartmentTabId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM DepartmentTab WHERE DepartmentTabId = " & DB.Number(DepartmentTabId)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentTabId"))) Then
                    m_DepartmentTabId = Convert.ToInt32(reader("DepartmentTabId"))
                Else
                    m_DepartmentTabId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                    m_DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                Else
                    m_DepartmentId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                    m_Arrange = Convert.ToInt16(reader("Arrange"))
                Else
                    m_Arrange = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = True
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                    m_Image = reader("Image").ToString()
                Else
                    m_Image = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                    m_URLCode = reader("URLCode").ToString()
                Else
                    m_URLCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    m_Description = reader("Description").ToString()
                Else
                    m_Description = ""
                End If
            End If
        End Sub

        Public Overridable Function Insert() As Integer
            Dim SQL As String = ""

            SQL = " INSERT INTO DepartmentTab (" _
             & "DepartmentId" _
             & ",Name" _
             & ",Arrange" _
             & ",IsActive" _
             & ",PageTitle" _
             & ",MetaDescription" _
             & ",MetaKeyword" _
             & ",CreatedDate" _
             & ",ModifiedDate" _
             & ",URLCode" _
             & ",OutsideUSPageTitle" _
             & ",OutsideUSMetaDescription" _
             & ",Image" _
             & ",Description" _
             & ") VALUES (" _
             & "" & m_DB.Quote(DepartmentId) _
             & "," & m_DB.Quote(Name) _
             & "," & m_DB.Quote(Arrange) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Quote(PageTitle) _
             & "," & m_DB.Quote(MetaDescription) _
             & "," & m_DB.Quote(MetaKeyword) _
             & "," & m_DB.NullQuote(DateTime.Now) _
             & "," & m_DB.NullQuote(DateTime.Now) _
              & "," & m_DB.Quote(URLCode) _
             & "," & m_DB.Quote(OutsideUSPageTitle) _
             & "," & m_DB.Quote(OutsideUSMetaDescription) _
              & "," & m_DB.Quote(Image) _
              & "," & m_DB.Quote(Description) _
            & ")"


            DepartmentTabId = m_DB.InsertSQL(SQL)

            Return DepartmentTabId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE DepartmentTab SET " _
             & " Name = " & m_DB.Quote(Name) _
             & " PageTitle = " & m_DB.Quote(PageTitle) _
             & " MetaDescription = " & m_DB.Quote(MetaDescription) _
             & " MetaKeyword = " & m_DB.Quote(MetaKeyword) _
             & ",IsActive = " & CInt(IsActive) _
             & ",Arrange = " & CInt(Arrange) _
             & ",DepartmentId = " & CInt(DepartmentId) _
             & ",ModifiedDate = " & m_DB.Quote(DateTime.Now.ToString()) _
             & ",URLCode = " & m_DB.Quote(URLCode) _
             & " OutsideUSPageTitle = " & m_DB.Quote(OutsideUSPageTitle) _
             & " OutsideUSMetaDescription = " & m_DB.Quote(OutsideUSMetaDescription) _
             & " Image = " & m_DB.Quote(Image) _
             & " Description = " & m_DB.Quote(Description) _
             & " WHERE DepartmentTabId = " & CInt(DepartmentTabId)

            m_DB.ExecuteSQL(SQL)
        End Sub
    End Class

    Public Class DepartmentTabCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal DepartmentTab As DepartmentTabRow)
            Me.List.Add(DepartmentTab)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DepartmentTabRow
            Get
                Return CType(Me.List.Item(Index), DepartmentTabRow)
            End Get

            Set(ByVal Value As DepartmentTabRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace