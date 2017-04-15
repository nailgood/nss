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
    Public Class LandingPageRow
        Inherits LandingPageRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal UrlCode As String)
            MyBase.New(database, UrlCode)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As LandingPageRow
            Dim row As LandingPageRow
            row = New LandingPageRow(_Database, id)
            row.Load()
            Return row
        End Function
        Public Shared Function GetRowByUrlcode(ByVal _Database As Database, ByVal UrlCode As String) As LandingPageRow
            Dim row As LandingPageRow
            row = New LandingPageRow(_Database, UrlCode)
            row.LoadByUrlCode()
            Return row
        End Function

        Public Shared Function CheckLandingPageByItem(ByVal ItemId As Integer, ByVal CustomerPriceGroupId As Integer) As LandingPageRow
            Dim row As New LandingPageRow()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_LandingPage_CheckLandingPageByItem")
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
                Dim reader As SqlDataReader = db.ExecuteReader(cmd)
                If Not reader Is Nothing Then
                    If reader.Read Then
                        row = LoadByReader(reader)
                    End If
                End If
            Catch ex As Exception

            End Try
            Return row
        End Function

        Public Shared Function CheckLandingPageByUrlCode(ByVal UrlCode As String, ByVal CustomerPriceGroupId As Integer) As LandingPageRow
            Dim row As New LandingPageRow()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_LandingPage_CheckLandingPageByUrlCode")
                db.AddInParameter(cmd, "UrlCode", DbType.String, UrlCode)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
                Dim reader As SqlDataReader = db.ExecuteReader(cmd)
                If Not reader Is Nothing Then
                    If reader.Read Then
                        row = LoadByReader(reader)
                    End If
                End If
            Catch ex As Exception

            End Try
            Return row
        End Function

        Protected Shared Function LoadByReader(ByVal reader As IDataReader) As LandingPageRow
            Dim row As New LandingPageRow
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        row.Id = Convert.ToInt32(reader("Id"))
                    Else
                        row.Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("UrlCode"))) Then
                        row.UrlCode = reader("UrlCode").ToString()
                    Else
                        row.UrlCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        row.Title = reader("Title").ToString()
                    Else
                        row.Title = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        row.ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        row.ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CustomerPriceGroupId"))) Then
                        row.CustomerPriceGroupId = Convert.ToInt32(reader("CustomerPriceGroupId"))
                    Else
                        row.CustomerPriceGroupId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                        row.StartingDate = Convert.ToDateTime(reader("StartingDate"))
                    Else
                        row.StartingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                        row.EndingDate = Convert.ToDateTime(reader("EndingDate"))
                    Else
                        row.EndingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        row.IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        row.IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("FileLocation"))) Then
                        row.FileLocation = reader("FileLocation").ToString()
                    Else
                        row.FileLocation = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        row.PageTitle = reader("PageTitle").ToString()
                    Else
                        row.PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        row.MetaDescription = reader("MetaDescription").ToString()
                    Else
                        row.MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                        row.MetaKeywords = reader("MetaKeywords").ToString()
                    Else
                        row.MetaKeywords = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("GoogleABCode"))) Then
                        row.GoogleABCode = reader("GoogleABCode").ToString()
                    Else
                        row.GoogleABCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("UrlReturn"))) Then
                        row.UrlReturn = reader("UrlReturn").ToString()
                    Else
                        row.UrlReturn = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return row
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As LandingPageRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LandingPage_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("UrlCode", SqlDbType.VarChar, 0, data.UrlCode))
                cmd.Parameters.Add(_Database.InParam("Title", SqlDbType.NVarChar, 0, data.Title))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, data.ItemId))
                cmd.Parameters.Add(_Database.InParam("CustomerPriceGroupId", SqlDbType.Int, 0, data.CustomerPriceGroupId))
                cmd.Parameters.Add(_Database.InParam("StartingDate", SqlDbType.DateTime, 0, data.StartingDate))
                cmd.Parameters.Add(_Database.InParam("EndingDate", SqlDbType.DateTime, 0, data.EndingDate))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileLocation", SqlDbType.VarChar, 0, data.FileLocation))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeywords", SqlDbType.NVarChar, 0, data.MetaKeywords))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("GoogleABCode", SqlDbType.NVarChar, 0, data.GoogleABCode))
                cmd.Parameters.Add(_Database.InParam("UrlReturn", SqlDbType.VarChar, 0, data.UrlReturn))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Update(ByVal _Database As Database, ByVal data As LandingPageRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LandingPage_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, data.Id))
                cmd.Parameters.Add(_Database.InParam("UrlCode", SqlDbType.VarChar, 0, data.UrlCode))
                cmd.Parameters.Add(_Database.InParam("Title", SqlDbType.NVarChar, 0, data.Title))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, data.ItemId))
                cmd.Parameters.Add(_Database.InParam("CustomerPriceGroupId", SqlDbType.Int, 0, data.CustomerPriceGroupId))
                cmd.Parameters.Add(_Database.InParam("StartingDate", SqlDbType.DateTime, 0, data.StartingDate))
                cmd.Parameters.Add(_Database.InParam("EndingDate", SqlDbType.DateTime, 0, data.EndingDate))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileLocation", SqlDbType.VarChar, 0, data.FileLocation))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeywords", SqlDbType.NVarChar, 0, data.MetaKeywords))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("GoogleABCode", SqlDbType.NVarChar, 0, data.GoogleABCode))
                cmd.Parameters.Add(_Database.InParam("UrlReturn", SqlDbType.VarChar, 0, data.UrlReturn))
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
        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LandingPage_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
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

        Public Shared Function GetList(ByVal condition As String, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_LandingPage_GetList")
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                total = result.Tables(1).Rows(0)(0).ToString()
                Return result.Tables(0)
            Catch ex As Exception

            End Try
            Return New DataTable
        End Function

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LandingPage_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
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

        Public Shared Function CheckURLCode(ByVal _Database As Database, ByVal Id As Integer, ByVal URLCode As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LandingPage_CheckURLcode"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.InParam("URLCode", SqlDbType.VarChar, 0, URLCode))
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

    End Class

    Public MustInherit Class LandingPageRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_UrlCode As String = Nothing
        Private m_Title As String = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_CustomerPriceGroupId As Integer = Nothing
        Private m_StartingDate As DateTime = Nothing
        Private m_EndingDate As DateTime = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_FileLocation As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_GoogleABCode As String = Nothing
        Private m_UrlReturn As String = Nothing

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property UrlCode() As String
            Get
                Return m_UrlCode
            End Get
            Set(ByVal Value As String)
                m_UrlCode = Value
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

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerPriceGroupId = Value
            End Set
        End Property

        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartingDate = Value
            End Set
        End Property

        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndingDate = Value
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

        Public Property FileLocation() As String
            Get
                Return m_FileLocation
            End Get
            Set(ByVal Value As String)
                m_FileLocation = Value
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

        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
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

        Public Property GoogleABCode() As String
            Get
                Return m_GoogleABCode
            End Get
            Set(ByVal Value As String)
                m_GoogleABCode = Value
            End Set
        End Property
        Public Property UrlReturn() As String
            Get
                Return m_UrlReturn
            End Get
            Set(ByVal Value As String)
                m_UrlReturn = Value
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
        Public Sub New(ByVal database As Database, ByVal UrlCode As String)
            m_DB = database
            m_UrlCode = UrlCode
        End Sub 'New
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM LandingPage WHERE Id = " & m_DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("LandingPage.vb", "Load", ex)
            End Try
        End Sub

        Protected Overridable Sub LoadByUrlCode()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM LandingPage WHERE UrlCode = " & m_DB.Quote(UrlCode)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("LandingPage.vb", "LoadByUrlCode", ex)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("UrlCode"))) Then
                        m_UrlCode = reader("UrlCode").ToString()
                    Else
                        m_UrlCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        m_Title = reader("Title").ToString()
                    Else
                        m_Title = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CustomerPriceGroupId"))) Then
                        m_CustomerPriceGroupId = Convert.ToInt32(reader("CustomerPriceGroupId"))
                    Else
                        m_CustomerPriceGroupId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                        m_StartingDate = Convert.ToDateTime(reader("StartingDate"))
                    Else
                        m_StartingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                        m_EndingDate = Convert.ToDateTime(reader("EndingDate"))
                    Else
                        m_EndingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("FileLocation"))) Then
                        m_FileLocation = reader("FileLocation").ToString()
                    Else
                        m_FileLocation = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("GoogleABCode"))) Then
                        m_GoogleABCode = reader("GoogleABCode").ToString()
                    Else
                        m_GoogleABCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("UrlReturn"))) Then
                        m_UrlReturn = reader("UrlReturn").ToString()
                    Else
                        m_UrlReturn = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class
    Public Class LandingPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As LandingPageRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As LandingPageRowBase
            Get
                Return CType(Me.List.Item(Index), LandingPageRowBase)
            End Get

            Set(ByVal Value As LandingPageRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace

