Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Data.SqlClient
Namespace DataLayer
    Public Class AlbumRow
        Inherits AlbumRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal AlbumId As Integer)
            MyBase.New(database, AlbumId)
        End Sub 'New
        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal ItemId As Integer, ByVal AlbumId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AlbumItem_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
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
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal AlbumId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Album_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
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
        Public Shared Function Delete(ByVal _Database As Database, ByVal AlbumId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Album_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
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
        Public Shared Function Remove_AlbumItem(ByVal _Database As Database, ByVal AlbumId As Integer, ByVal ItemId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AlbumItem_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AlbumId", SqlDbType.Int, 0, AlbumId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
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

        Private Shared Sub SendMailLog(ByVal ex As Exception)
            Email.SendError("ToError500", "Album.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
        End Sub

        Public Shared Function LoadAll() As AlbumCollection
            Dim ss As New AlbumCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Album_LoadAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim album As AlbumRow = GetData(dr)
                    ss.Add(album)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog(ex)
            End Try

            Return ss
        End Function
        Public Shared Function GetAllByItemId(ByVal _Database As Database, ByVal itemId As Integer) As AlbumCollection
            Dim ss As New AlbumCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Album_GetAllByItemId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, itemId))
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim album As AlbumRow = GetData(dr)
                    ss.Add(GetData(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog(ex)
            End Try
            Return ss
        End Function
        Public Shared Function CountByItemId(ByVal _Database As Database, ByVal itemId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Album_CountByItemId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, itemId))
                result = db.ExecuteScalar(cmd)
            Catch ex As Exception
                SendMailLog(ex)
            End Try
            Return result
        End Function
        Public Shared Function CountByItemCode(ByVal _Database As Database, ByVal itemCode As String) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Album_CountByItemURLCode"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("URLCode", SqlDbType.VarChar, 0, itemCode))
                result = db.ExecuteScalar(cmd)
            Catch ex As Exception
                SendMailLog(ex)
            End Try
            Return result
        End Function
        Private Shared Function GetData(ByVal reader As SqlDataReader) As AlbumRow
            Dim result As New AlbumRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("AlbumId"))) Then
                    result.AlbumId = Convert.ToInt32(reader("AlbumId"))
                Else
                    result.AlbumId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    result.Description = reader("Description").ToString()
                Else
                    result.Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    result.MetaDescription = reader("MetaDescription").ToString()
                Else
                    result.MetaDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                    result.MetaKeywords = reader("MetaKeywords").ToString()
                Else
                    result.MetaKeywords = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                    result.PageTitle = reader("PageTitle").ToString()
                Else
                    result.PageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ThumImg"))) Then
                    result.ThumImg = reader("ThumImg").ToString()
                Else
                    result.ThumImg = ""
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return result
        End Function
    End Class
    Public MustInherit Class AlbumRowBase
        Private m_DB As Database
        Private m_AlbumId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_ThumImg As String = Nothing
        Private m_Description As String = Nothing
        Private m_IsActive As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Public Property AlbumId() As Integer
            Get
                Return m_AlbumId
            End Get
            Set(ByVal value As Integer)
                m_AlbumId = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
        Public Property ThumImg() As String
            Get
                Return m_ThumImg
            End Get
            Set(ByVal value As String)
                m_ThumImg = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
            End Set
        End Property
        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal value As Integer)
                m_Arrange = value
            End Set
        End Property
        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal value As String)
                m_PageTitle = value
            End Set
        End Property
        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal value As String)
                m_MetaDescription = value
            End Set
        End Property
        Public Property MetaKeywords() As String
            Get
                Return m_MetaKeywords
            End Get
            Set(ByVal value As String)
                m_MetaKeywords = value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal value As DateTime)
                m_CreateDate = value
            End Set
        End Property

        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifyDate = value
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
        Public Sub New(ByVal database As Database, ByVal AlbumId As Integer)
            m_DB = database
            AlbumId = 0
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal AlbumId As Integer) As AlbumRow
            Dim row As AlbumRow

            row = New AlbumRow(_Database, AlbumId)
            row.LoadByAlbumId(AlbumId)

            Return row
        End Function

        Protected Overridable Sub LoadByAlbumId(ByVal AlbumId As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = ""
                SQL = "SELECT * FROM Album WHERE AlbumId = " & AlbumId
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Album.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If IsDBNull(r.Item("AlbumId")) Then
                    m_AlbumId = 0
                Else
                    m_AlbumId = Convert.ToInt32(r.Item("AlbumId"))
                End If
                If IsDBNull(r.Item("Name")) Then
                    m_Name = Nothing
                Else
                    m_Name = Convert.ToString(r.Item("Name"))
                End If
                If IsDBNull(r.Item("ThumImg")) Then
                    m_ThumImg = Nothing
                Else
                    m_ThumImg = Convert.ToString(r.Item("ThumImg"))
                End If
                If (Not r.IsDBNull(r.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(r("IsActive"))
                Else
                    m_IsActive = False
                End If
                If IsDBNull(r.Item("Arrange")) Then
                    m_Arrange = 0
                Else
                    m_Arrange = Convert.ToInt32(r.Item("Arrange"))
                End If
                If IsDBNull(r.Item("PageTitle")) Then
                    m_PageTitle = Nothing
                Else
                    m_PageTitle = Convert.ToString(r.Item("PageTitle"))
                End If
                If IsDBNull(r.Item("MetaDescription")) Then
                    m_MetaDescription = Nothing
                Else
                    m_MetaDescription = Convert.ToString(r.Item("MetaDescription"))
                End If

                If IsDBNull(r.Item("Description")) Then
                    Description = Nothing
                Else
                    Description = Convert.ToString(r.Item("Description"))
                End If


                If IsDBNull(r.Item("MetaKeywords")) Then
                    m_MetaKeywords = Nothing
                Else
                    m_MetaKeywords = Convert.ToString(r.Item("MetaKeywords"))
                End If
                If IsDBNull(r.Item("CreatedDate")) Then
                    m_CreateDate = Nothing
                Else
                    m_CreateDate = Convert.ToDateTime(r.Item("CreatedDate"))
                End If
                If IsDBNull(r.Item("ModifyDate")) Then
                    m_ModifyDate = Nothing
                Else
                    m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub
        Public Overridable Sub Update()
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_Album_Update"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cm, "AlbumId", DbType.Int32, AlbumId)
            db.AddInParameter(cm, "Name", DbType.String, Name)
            db.AddInParameter(cm, "ThumImg", DbType.String, ThumImg)
            db.AddInParameter(cm, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cm, "Arrange", DbType.Int32, Arrange)
            db.AddInParameter(cm, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cm, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cm, "Description", DbType.String, Description)
            db.AddInParameter(cm, "MetaKeywords", DbType.String, MetaKeywords)
            db.AddInParameter(cm, "ModifyDate", DbType.DateTime, Date.Now)
            db.ExecuteNonQuery(cm)
            'Catch ex As Exception

            'End Try

        End Sub

        Public Overridable Sub Insert()
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_Album_Insert"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddOutParameter(cm, "AlbumId", DbType.Int32, 1)
            db.AddInParameter(cm, "Name", DbType.String, Name)
            db.AddInParameter(cm, "ThumImg", DbType.String, ThumImg)
            db.AddInParameter(cm, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cm, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cm, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cm, "Description", DbType.String, Description)
            db.AddInParameter(cm, "MetaKeywords", DbType.String, MetaKeywords)
            db.AddInParameter(cm, "CreateDate", DbType.DateTime, Date.Now)
            db.ExecuteNonQuery(cm)
            AlbumId = Convert.ToInt32(db.GetParameterValue(cm, "AlbumId"))
            'Catch ex As Exception

            'End Try

        End Sub
        Public Sub InsertAlbumSong(ByVal AlbId As Integer, ByVal SId As Integer)
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_AlbumSong_Insert"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddOutParameter(cm, "Id", DbType.Int32, 1)
            db.AddInParameter(cm, "AlbumId", DbType.Int32, AlbId)
            db.AddInParameter(cm, "SongId", DbType.Int32, SId)

            db.ExecuteNonQuery(cm)
            'AlbumId = Convert.ToInt32(db.GetParameterValue(cm, "Id"))
            'Catch ex As Exception

            'End Try

        End Sub
        Public Sub InsertAlbumItem(ByVal AlbumId As Integer, ByVal ItemId As Integer)
            'Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_AlbumItem_Insert"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddOutParameter(cm, "Id", DbType.Int32, 1)
            db.AddInParameter(cm, "AlbumId", DbType.Int32, AlbumId)
            db.AddInParameter(cm, "ItemId", DbType.Int32, ItemId)

            db.ExecuteNonQuery(cm)
            'AlbumId = Convert.ToInt32(db.GetParameterValue(cm, "Id"))
            'Catch ex As Exception

            'End Try

        End Sub
        Public Sub RemoveAlbumSong_Album(ByVal AlbId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_AlbumSong_Album_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "AlbumId", DbType.Int32, AlbId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
        Public Sub RemoveAlbumItem_Item(ByVal ItemId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_AlbumItem_Item_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove

        Public Sub RemoveAlbumSong_Song(ByVal SId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_AlbumSong_Song_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "SongId", DbType.Int32, SId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class
    Public Class AlbumCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Album As AlbumRow)
            Me.List.Add(Album)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AlbumRow
            Get
                Return CType(Me.List.Item(Index), AlbumRow)
            End Get

            Set(ByVal Value As AlbumRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As AlbumCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AlbumCollection
                For Each obj As AlbumRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

