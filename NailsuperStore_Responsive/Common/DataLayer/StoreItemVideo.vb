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
    Public Class StoreItemVideoRow
        Inherits StoreItemVideoRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal itemVideoId As Integer)
            MyBase.New(database, itemVideoId)
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal itemVideoId As Integer) As StoreItemVideoRow
            Dim row As StoreItemVideoRow
            row = New StoreItemVideoRow(_Database, itemVideoId)
            row.Load()
            Return row
        End Function
        Public Shared Function ListByItemId(ByVal ItemId As Integer) As StoreItemVideoCollection
            If ItemId <= 0 Then
                Return Nothing
            End If
            Dim reader As SqlDataReader = Nothing
            Dim tabs As New StoreItemVideoCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreItemVideo_ListByItemId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                reader = db.ExecuteReader(cmd)
                While reader.Read
                    Dim tab As New StoreItemVideoRow()
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemVideoId"))) Then
                        tab.ItemVideoId = Convert.ToInt32(reader("ItemVideoId"))
                    Else
                        tab.ItemVideoId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        tab.ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        tab.ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Url"))) Then
                        tab.Url = Convert.ToString(reader("Url"))
                    Else
                        tab.Url = String.Empty
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                        tab.ThumbImage = Convert.ToString(reader("ThumbImage"))
                    Else
                        tab.ThumbImage = String.Empty
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        tab.Description = Convert.ToString(reader("Description"))
                    Else
                        tab.Description = String.Empty
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                        tab.Arrange = Convert.ToInt32(reader("Arrange"))
                    Else
                        tab.Arrange = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        tab.IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        tab.IsActive = True
                    End If
                    tabs.Add(tab)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return tabs
        End Function
        Public Shared Function ListByItemId(ByVal DB As Database, ByVal ItemId As Integer) As DataTable
            If ItemId <= 0 Then
                Return Nothing
            End If
            Try
                Return DB.GetDataTable("select siv.url,siv.ItemVideoId,coalesce(siv.ThumbImage,'') as ThumbImage, COALESCE(siv.description,'') as description,COALESCE(siv.Name,'') as Name from StoreItem si inner join storeitemvideo siv on si.ItemId = siv.itemid where siv.isActive = 1 and si.itemid = " & ItemId & " order by Arrange asc")

            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Public Shared Function Insert(ByVal item As StoreItemVideoRow) As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_StoreItemVideo_INSERT As String = "sp_StoreItemVideo_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_StoreItemVideo_INSERT)

            db.AddInParameter(cmd, "ItemId", DbType.Int32, item.ItemId)
            db.AddInParameter(cmd, "Url", DbType.String, item.Url)
            db.AddInParameter(cmd, "ThumbImage", DbType.String, item.ThumbImage)
            db.AddInParameter(cmd, "Description", DbType.String, item.Description)
            db.AddInParameter(cmd, "CreateDate", DbType.DateTime, item.CreatedDate)
            db.AddInParameter(cmd, "IsActive", DbType.Int32, Convert.ToUInt32(item.IsActive))

            db.ExecuteNonQuery(cmd)
        End Function

        Public Shared Function Update(ByVal item As StoreItemVideoRow) As Integer
            Dim result As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItemVideo_Update")
            db.AddInParameter(cmd, "ItemVideoId", DbType.Int32, item.ItemVideoId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, item.ItemId)
            db.AddInParameter(cmd, "Url", DbType.String, item.Url)
            db.AddInParameter(cmd, "ThumbImage", DbType.String, item.ThumbImage)
            db.AddInParameter(cmd, "Description", DbType.String, item.Description)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, item.IsActive)
            db.AddOutParameter(cmd, "returnValue", DbType.Int32, 0)
            db.ExecuteNonQuery(cmd)
            result = Convert.ToInt32(db.GetParameterValue(cmd, "returnValue"))
            '------------------------------------------------------------------------
            Return result
        End Function

        Public Shared Sub Delete(ByVal _Database As Database, ByVal StoreItemVideoId As Integer)
            Dim SQL As String = "exec dbo.sp_StoreItemVideo_Delete " & StoreItemVideoId
            _Database.ExecuteSQL(SQL)
        End Sub
        Public Shared Sub DeleteByItemId(ByVal _Database As Database, ByVal itemId As Integer)
            Dim SQL As String = "exec dbo.sp_StoreItemVideo_DeleteByItemId " & itemId
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub ChangeIsActive(ByVal _Database As Database, ByVal StoreItemVideoId As Integer)
            Dim SQL As String = "exec dbo.sp_StoreItemVideo_ChangeIsActive " & StoreItemVideoId
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub ChangeArrangeItem(ByVal _Database As Database, ByVal ShopSaveId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_StoreItemVideo_ChangeArrange " & ShopSaveId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
        End Sub
        Public Shared Function ConvertLinkVideoToImage(ByVal link As String) As String
            If (link = "") Then
                Return String.Empty
            End If
            Dim result As String = "http://img.youtube.com/vi/{0}/0.jpg"
            Dim indexPara As Integer = link.LastIndexOf("?v=")
            If indexPara > 0 Then
                Dim value As String = String.Empty
                Dim indexQ As Integer = link.IndexOf("&")
                If indexQ > 0 Then
                    value = link.Substring(indexPara + 3, indexQ - indexPara - 3)
                Else
                    value = link.Substring(indexPara + 3, link.Length - indexPara - 3)
                End If

                result = String.Format(result, value)
            End If
            Return result
        End Function
        Public Shared Function IsListVideo(ByVal _Database As Database, ByVal ItemId As Integer) As Boolean
            Dim dtVideo As DataTable = _Database.GetDataTable("select si.* from StoreItem si inner join storeitemvideo siv on si.ItemId = siv.itemid where siv.IsActive = 1 and si.itemid = " & ItemId)
            If dtVideo.Rows.Count > 1 Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class


    Public MustInherit Class StoreItemVideoRowBase
        Private m_DB As Database
        Private m_ItemVideoId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_Url As String = Nothing
        Private m_ThumbImage As String = Nothing
        Private m_Description As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_Arrange As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property ItemVideoId() As Integer
            Get
                Return m_ItemVideoId
            End Get
            Set(ByVal Value As Integer)
                m_ItemVideoId = Value
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

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
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

        Public Property ThumbImage() As String
            Get
                Return m_ThumbImage
            End Get
            Set(ByVal Value As String)
                m_ThumbImage = Value
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
        Public Sub New(ByVal database As Database, ByVal ItemVideoId As Integer)
            m_DB = database
            m_ItemVideoId = ItemVideoId
        End Sub 'New
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemVideo WHERE ItemVideoId = " & DB.Number(ItemVideoId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemVideoId"))) Then
                        m_ItemVideoId = Convert.ToInt32(reader("ItemVideoId"))
                    Else
                        m_ItemVideoId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Url"))) Then
                        m_Url = Convert.ToString(reader("Url"))
                    Else
                        m_Url = String.Empty
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                        m_ThumbImage = Convert.ToString(reader("ThumbImage"))
                    Else
                        m_ThumbImage = String.Empty
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        m_Description = Convert.ToString(reader("Description"))
                    Else
                        m_Description = String.Empty
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                        m_Arrange = Convert.ToInt32(reader("Arrange"))
                    Else
                        m_Arrange = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''   Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
    End Class

    Public Class StoreItemVideoCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreItemVideo As StoreItemVideoRow)
            Me.List.Add(StoreItemVideo)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreItemVideoRow
            Get
                Return CType(Me.List.Item(Index), StoreItemVideoRow)
            End Get

            Set(ByVal Value As StoreItemVideoRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace