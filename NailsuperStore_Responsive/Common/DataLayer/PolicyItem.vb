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
    Public Class PolicyItemRow
        Inherits PolicyItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Shared Function ListByPolicyId(ByVal PolicyId As String) As DataTable
            Dim dt As DataTable

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_PolicyItem_ListByPolicyId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PolicyId", DbType.Int32, PolicyId)
                dt = db.ExecuteDataSet(cmd).Tables(0)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > ListByPolicyId", "PolicyId: " & PolicyId & "<br>Exception: " & ex.ToString())
            End Try

            Return dt
        End Function

        Public Shared Function ListByPolicyId(ByVal _Database As Database, ByVal PolicyId As Integer, ByVal CurrentPage As Integer, ByVal PageSize As Integer) As PolicyItemCollection
            Dim tabs As New PolicyItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_PolicyItem_ListByPolicyId_V1"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PolicyId", DbType.Int32, PolicyId)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, CurrentPage)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim tab As New PolicyItemRow(_Database)
                    tab.PolicyId = CInt(dr("PolicyId"))
                    tab.ItemId = CInt(dr("ItemId"))
                    tab.IsActive = CBool(dr("IsActive"))
                    tab.SKU = dr("SKU")
                    tab.ItemName = dr("ItemName")
                    tab.Arrange = dr("Arrange")
                    tabs.Add(tab)

                End While
                Core.CloseReader(dr)
                tabs.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return tabs

        End Function

        Public Shared Function InsertItemByPolicyIDsItemId(ByVal _Database As Database, ByVal PolicyIDs As String, ByVal ItemId As Integer, ByVal clearCache As Boolean) As Integer
            Dim SQL As String = "exec [sp_PolicyItem_Insert_ByPolicyIDsItemId] " & PolicyIDs & "," & ItemId
            If (clearCache) Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
            End If
            Return _Database.ExecuteSQL(SQL)

        End Function
        Public Shared Function InsertItem(ByVal _Database As Database, ByVal item As PolicyItemRow, ByVal clearCache As Boolean) As Integer
            Dim SQL As String = "exec dbo.sp_PolicyItem_Insert " & item.PolicyId & ",'" & item.SKU & "','" & item.CreatedDate & "'," & item.IsActive
            If (clearCache) Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
            End If
            Return _Database.ExecuteSQL(SQL)

        End Function
        Public Shared Function InsertList(ByVal _Database As Database, ByVal PolicyId As Integer, ByVal IsActive As Boolean, ByVal ListSKU As String) As Integer
            Dim SQL As String = "exec dbo.sp_PolicyItem_InsertList " & ListSKU
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
            Return _Database.ExecuteSQL(SQL)
        End Function

        Public Shared Sub Delete(ByVal _Database As Database, ByVal PolicyItemId As Integer, ByVal ItemId As Integer, ByVal clearCache As Boolean)
            Dim SQL As String = "exec dbo.sp_PolicyItem_Delete " & PolicyItemId & "," & ItemId
            _Database.ExecuteSQL(SQL)
            If clearCache Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
            End If
        End Sub

        Public Shared Function CountListActiveSKU(ByVal _Database As Database, ByVal PolicyId As Integer, ByVal ListSKU As String) As Integer
            Dim SQL As String = ""

            SQL = "SELECT " _
            & " COUNT(ItemId)" _
            & " FROM StoreItem " _
            & " WHERE SKU IN (" & ListSKU & ") AND SKU NOT IN (SELECT S.SKU FROM StoreItem S INNER JOIN PolicyItem DT ON DT.ItemId = S.ItemId AND DT.PolicyId =  " & _Database.Quote(PolicyId) & ")"

            Return _Database.ExecuteScalar(SQL)
        End Function

        Public Shared Sub ChangeArrange(ByVal _Database As Database, ByVal ItemId As Integer, ByVal PolicyId As Integer, ByVal Arrange As Integer)
            Dim SQL As String = "UPDATE PolicyItem SET Arrange = " & _Database.Quote(Arrange) & " WHERE ItemId = " & _Database.Quote(ItemId) & " AND PolicyId = " & _Database.Quote(PolicyId)
            _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
        End Sub
        Public Shared Sub ChangeArrangeItem(ByVal _Database As Database, ByVal PolicyId As Integer, ByVal ItemId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_PolicyItem_ChangeArrange " & PolicyId & "," & ItemId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")
        End Sub

    End Class


    Public MustInherit Class PolicyItemRowBase
        Private m_DB As Database
        Private m_PolicyItemId As Integer = Nothing
        Private m_PolicyId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_SKU As String = Nothing
        Private m_ItemName As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_Arrange As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Public Shared cachePrefixKey As String = "PolicyItem_"
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property PolicyId() As Integer
            Get
                Return m_PolicyId
            End Get
            Set(ByVal Value As Integer)
                m_PolicyId = Value
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

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("PolicyId"))) Then
                        m_PolicyId = Convert.ToInt32(reader("PolicyId"))
                    Else
                        m_PolicyId = 0
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
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Public Overridable Function Insert(ByVal ListSKU As String) As Integer
            Dim SQL As String = ""
            Dim arrList As Array = Split(ListSKU, ",")
            Dim index As Integer = 0
            Dim sku As String = String.Empty
            For i = 0 To arrList.Length - 1
                sku = arrList(i).ToString()
                If (sku <> String.Empty) Then
                    SQL = SQL & " INSERT INTO PolicyItem (" _
                         & "PolicyId" _
                         & ",ItemId" _
                         & ",IsActive" _
                         & ",CreatedDate" _
                         & ",Arrange" _
                         & ") SELECT " _
                         & "" & m_DB.Quote(PolicyId) _
                         & ", ItemId" _
                         & "," & CInt(IsActive) _
                         & "," & m_DB.NullQuote(DateTime.Now) _
                         & ", " & index.ToString() & " FROM StoreItem WHERE SKU=" & sku & ";"
                    index = index + 1
                End If
            Next
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey, "Policy_")

            Dim iResult As Int16 = m_DB.InsertSQL(SQL)

            Return iResult
        End Function


    End Class

    Public Class PolicyItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal PolicyItem As PolicyItemRow)
            Me.List.Add(PolicyItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As PolicyItemRow
            Get
                Return CType(Me.List.Item(Index), PolicyItemRow)
            End Get

            Set(ByVal Value As PolicyItemRow)
                Me.List(Index) = Value
            End Set
        End Property
        Private m_TotalRecords As Integer

        Public Property TotalRecords() As Integer
            Get
                Return m_TotalRecords
            End Get
            Set(ByVal value As Integer)
                m_TotalRecords = value
            End Set
        End Property
    End Class
End Namespace