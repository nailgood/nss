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
    Public Class DepartmentTabItemRow
        Inherits DepartmentTabItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New


        Public Shared Function ListByTabId(ByVal _Database As Database, ByVal DepartmentTabId As Integer) As DepartmentTabItemCollection

            Dim tabs As New DepartmentTabItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim sp As String = "sp_DepartmentTabItem_ListByTabId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentTabId", DbType.Int32, DepartmentTabId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New DepartmentTabItemRow(_Database)
                    tab.DepartmentTabId = CInt(dr("DepartmentTabId"))
                    tab.ItemId = CInt(dr("ItemId"))
                    tab.IsActive = CBool(dr("IsActive"))
                    tab.SKU = dr("SKU")
                    tab.ItemName = dr("ItemName")
                    tabs.Add(tab)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return tabs

        End Function
        Public Shared Function InsertDepartmentTab(ByVal item As DepartmentTabRow) As Integer
            'Dim SQL As String = "exec dbo.sp_DepartmentTab_Insert " & item.DepartmentId & "," & item.IsActive & ",'" & Common.FitterStringInDB(item.Name) & "','" & item.URLCode & "','" & Common.FitterStringInDB(item.PageTitle) & "','" & Common.FitterStringInDB(item.MetaDescription) & "','" & Common.FitterStringInDB(item.MetaKeyword) & "','" & DateTime.Now & "','" & Common.FitterStringInDB(item.OutsideUSPageTitle) & "','" & Common.FitterStringInDB(item.OutsideUSMetaDescription) & "','" & item.Image & "'"
            'Return _Database.ExecuteSQL(SQL)
            ' '' CacheUtils.RemoveCacheWithPrefix(cachePrefixKey)
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_Insert"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, item.DepartmentId)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, item.IsActive)
                db.AddInParameter(cmd, "Name", DbType.String, item.Name)
                db.AddInParameter(cmd, "URlCode", DbType.String, item.URLCode)
                db.AddInParameter(cmd, "PageTitle", DbType.String, item.PageTitle)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, item.MetaDescription)
                db.AddInParameter(cmd, "MetaKeyword", DbType.String, item.MetaKeyword)
                db.AddInParameter(cmd, "OutsideUSPageTitle", DbType.String, item.OutsideUSPageTitle)
                db.AddInParameter(cmd, "OutsideUSMetaDescription", DbType.String, item.OutsideUSMetaDescription)
                db.AddInParameter(cmd, "Image", DbType.String, item.Image)
                db.AddInParameter(cmd, "Description", DbType.String, item.Description)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Email.SendError("ToError500", "DepartmentTabItem.vb- InsertDepartmentTab", "Exception: " & ex.ToString())
            End Try
            Return result
        End Function
        Public Shared Function UpdateDepartmentTab(ByVal item As DepartmentTabRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTab_Update"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, item.DepartmentId)
                db.AddInParameter(cmd, "DepartmentTabId", DbType.Int32, item.DepartmentTabId)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, item.IsActive)
                db.AddInParameter(cmd, "Name", DbType.String, item.Name)
                db.AddInParameter(cmd, "URlCode", DbType.String, item.URLCode)
                db.AddInParameter(cmd, "PageTitle", DbType.String, item.PageTitle)
                db.AddInParameter(cmd, "MetaDescription", DbType.String, item.MetaDescription)
                db.AddInParameter(cmd, "MetaKeyword", DbType.String, item.MetaKeyword)
                db.AddInParameter(cmd, "OutsideUSPageTitle", DbType.String, item.OutsideUSPageTitle)
                db.AddInParameter(cmd, "OutsideUSMetaDescription", DbType.String, item.OutsideUSMetaDescription)
                db.AddInParameter(cmd, "Image", DbType.String, item.Image)
                db.AddInParameter(cmd, "Description", DbType.String, item.Description)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            Catch ex As Exception
                Email.SendError("ToError500", "DepartmentTabItem.vb- UpdateDepartmentTab", "Exception: " & ex.ToString())
            End Try
            Return result
        End Function

        Public Shared Function InsertList(ByVal _Database As Database, ByVal DepartmentTabId As Integer, ByVal IsActive As Boolean, ByVal ListSKU As String) As Integer
            Dim SQL As String = "exec dbo.sp_DepartmentTabItem_InsertList " & ListSKU
            Return _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Function

        Public Shared Sub Delete(ByVal _Database As Database, ByVal DepartmentTabItemId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "exec dbo.sp_DepartmentTabItem_Delete " & DepartmentTabItemId & "," & ItemId
            _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub

        Public Shared Sub ChangeIsActive(ByVal _Database As Database, ByVal DepartmentTabItemId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "exec dbo.sp_DepartmentTabItem_ChangeIsActive " & DepartmentTabItemId & "," & ItemId
            _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub
        Public Shared Sub ChangeArrange(ByVal _Database As Database, ByVal DepartmentTabItemId As Integer, ByVal ItemId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_DepartmentTabItem_ChangeArrange " & DepartmentTabItemId & "," & ItemId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub

        Public Shared Function CountListActiveSKU(ByVal _Database As Database, ByVal DepartmentTabId As Integer, ByVal ListSKU As String) As Integer
            Dim SQL As String = ""

            SQL = "SELECT " _
            & " COUNT(ItemId)" _
            & " FROM StoreItem " _
            & " WHERE SKU IN (" & ListSKU & ") AND SKU NOT IN (SELECT S.SKU FROM StoreItem S INNER JOIN DepartmentTabItem DT ON DT.ItemId = S.ItemId AND DT.DepartmentTabId =  " & _Database.Quote(DepartmentTabId) & ")"

            Return _Database.ExecuteScalar(SQL)
        End Function

    End Class


    Public MustInherit Class DepartmentTabItemRowBase
        Private m_DB As Database
        Private m_DepartmentTabItemId As Integer = Nothing
        Private m_DepartmentTabId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_SKU As String = Nothing
        Private m_ItemName As String = Nothing
        Private m_IsActive As Boolean = True
        Public Shared cachePrefixKey As String = "DepartmentTabItem_"

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
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

        Public Property DepartmentTabId() As Integer
            Get
                Return m_DepartmentTabId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentTabId = Value
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

        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal Value As String)
                m_SKU = Value
            End Set
        End Property

        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal Value As String)
                m_ItemName = Value
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

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentTabId"))) Then
                    m_DepartmentTabId = Convert.ToInt32(reader("DepartmentTabId"))
                Else
                    m_DepartmentTabId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    m_ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    m_ItemId = 0
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
        End Sub

        Public Overridable Function Insert(ByVal SKU As String, ByVal clearCache As Boolean) As Integer
            Dim SQL As String = "exec dbo.sp_DepartmentTabItem_Insert " & DepartmentTabId & ",'" & SKU & "','" & DateTime.Now & "',1"
            If (clearCache) Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            End If
            Return m_DB.ExecuteSQL(SQL)
        End Function


    End Class

    Public Class DepartmentTabItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal DepartmentTabItem As DepartmentTabItemRow)
            Me.List.Add(DepartmentTabItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DepartmentTabItemRow
            Get
                Return CType(Me.List.Item(Index), DepartmentTabItemRow)
            End Get

            Set(ByVal Value As DepartmentTabItemRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace