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
    Public Class StoreOrderReviewRow
        Inherits StoreOrderReviewRowBase
        Public Shared cachePrefixKey As String = "StoreOrderReview_"
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer)
            MyBase.New(DB, OrderId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer) As StoreOrderReviewRow
            Dim row As StoreOrderReviewRow

            row = New StoreOrderReviewRow(DB, OrderId)
            row.Load()

            Return row
        End Function
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal OrderId As Integer)
            Dim row As StoreOrderReviewRow
            row = New StoreOrderReviewRow(DB, OrderId)
            row.Remove()
            Utility.CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------
        End Sub
        'Custom Methods

    End Class
    Public MustInherit Class StoreOrderReviewRowBase
        Private m_DB As Database
        Private m_OrderId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_ItemArrived As Boolean = Nothing
        Private m_ItemDescribed As Boolean = Nothing
        Private m_ServicePrompt As Byte = Nothing
        Private m_NumStars As Integer = Nothing
        Private m_DateAdded As DateTime = Nothing
        Private m_IsRecommendFriend As Boolean = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_Comment As String = Nothing
        Private m_IsFacebook As Boolean = Nothing
        Private m_IsShared As Boolean = Nothing
        Private m_Name As String = Nothing
        Private m_OrderNo As String = Nothing
        Public baseCachePrefixKey As String = "StoreOrderReview_"
        Public itemIndex As Integer = 0
        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property
        Public Property ItemArrived() As Boolean
            Get
                Return m_ItemArrived
            End Get
            Set(ByVal Value As Boolean)
                m_ItemArrived = Value
            End Set
        End Property
        Public Property ItemDescribed() As Boolean
            Get
                Return m_ItemDescribed
            End Get
            Set(ByVal Value As Boolean)
                m_ItemDescribed = Value
            End Set
        End Property
        Public Property ServicePrompt() As Byte
            Get
                Return m_ServicePrompt
            End Get
            Set(ByVal Value As Byte)
                m_ServicePrompt = Value
            End Set
        End Property
        Public Property NumStars() As Integer
            Get
                Return m_NumStars
            End Get
            Set(ByVal Value As Integer)
                m_NumStars = Value
            End Set
        End Property

        Public Property DateAdded() As DateTime
            Get
                Return m_DateAdded
            End Get
            Set(ByVal Value As DateTime)
                m_DateAdded = Value
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
        Public Property Comment() As String
            Get
                Return m_Comment
            End Get
            Set(ByVal Value As String)
                m_Comment = Value
            End Set
        End Property
        Public Property IsFacebook() As Boolean
            Get
                Return m_IsFacebook
            End Get
            Set(ByVal Value As Boolean)
                m_IsFacebook = Value
            End Set
        End Property
        Public Property IsShared() As Boolean
            Get
                Return m_IsShared
            End Get
            Set(ByVal Value As Boolean)
                m_IsShared = Value
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
        Public Property OrderNo() As String
            Get
                Return m_OrderNo
            End Get
            Set(ByVal Value As String)
                m_OrderNo = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer)
            m_DB = DB
            m_OrderId = OrderId
        End Sub 'New
        Public Shared Function CheckReview(ByVal db As Database, ByVal OrderId As Integer) As Boolean
            Dim id As Integer = db.ExecuteScalar("Select OrderId From StoreOrderReview Where OrderId = " & OrderId)
            If id > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function GetOrderReview(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields) As StoreOrderReviewCollection

            Dim c As New StoreOrderReviewCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreOrderReview_GetOrderReviews"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                r = db.ExecuteReader(cmd)
                While r.Read
                    Dim o As New StoreOrderReviewRow(DB1)
                    o.OrderId = r.Item("OrderId")
                    o.MemberId = Convert.ToInt32(r.Item("MemberId"))

                    o.ItemArrived = Convert.ToString(r.Item("ItemArrived"))
                    o.ItemDescribed = Convert.ToString(r.Item("ItemDescribed"))
                    o.NumStars = Convert.ToInt32(r.Item("NumStars"))
                    If r.Item("DateAdded") Is Convert.DBNull Then
                        o.DateAdded = Nothing
                    Else
                        o.DateAdded = Convert.ToDateTime(r.Item("DateAdded"))
                    End If
                    If r.Item("Comment") Is Convert.DBNull Then
                        o.Comment = Nothing
                    Else
                        o.Comment = Convert.ToString(r.Item("Comment"))
                    End If
                    If r.Item("Name") Is Convert.DBNull Then
                        o.Name = Nothing
                    Else
                        o.Name = Convert.ToString(r.Item("Name"))
                    End If
                    If r.Item("OrderNo") Is Convert.DBNull Then
                        o.OrderNo = Nothing
                    Else
                        o.OrderNo = Convert.ToString(r.Item("OrderNo"))
                    End If
                    o.ServicePrompt = Convert.ToByte(r.Item("ServicePrompt"))
                    c.Add(o)
                End While
                Core.CloseReader(r)
                c.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return c
        End Function

        Public Shared Function GetOrderReviewNarrowSearch(ByVal filter As DepartmentFilterFields) As StoreOrderReviewCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New StoreOrderReviewCollection
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreOrderReview_GetOrderReviewsNarrowSearch"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)

                ''narrow search rating
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    db.AddInParameter(cmd, "LowStar", DbType.Int32, Convert.ToInt32(low))
                    db.AddInParameter(cmd, "HighStar", DbType.Int32, Convert.ToInt32(high))
                End If

                r = db.ExecuteReader(cmd)
                While r.Read
                    Dim o As New StoreOrderReviewRow()
                    o.itemIndex = r.Item("row")
                    o.OrderId = r.Item("OrderId")
                    o.MemberId = Convert.ToInt32(r.Item("MemberId"))

                    o.ItemArrived = Convert.ToString(r.Item("ItemArrived"))
                    o.ItemDescribed = Convert.ToString(r.Item("ItemDescribed"))
                    o.NumStars = Convert.ToInt32(r.Item("NumStars"))
                    If r.Item("DateAdded") Is Convert.DBNull Then
                        o.DateAdded = Nothing
                    Else
                        o.DateAdded = Convert.ToDateTime(r.Item("DateAdded"))
                    End If
                    If r.Item("Comment") Is Convert.DBNull Then
                        o.Comment = Nothing
                    Else
                        o.Comment = Convert.ToString(r.Item("Comment"))
                    End If
                    If r.Item("Name") Is Convert.DBNull Then
                        o.Name = Nothing
                    Else
                        o.Name = Convert.ToString(r.Item("Name"))
                    End If
                    If r.Item("OrderNo") Is Convert.DBNull Then
                        o.OrderNo = Nothing
                    Else
                        o.OrderNo = Convert.ToString(r.Item("OrderNo"))
                    End If
                    o.ServicePrompt = Convert.ToByte(r.Item("ServicePrompt"))
                    c.Add(o)
                End While
                Core.CloseReader(r)
                c.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try


            Return c
        End Function

        Protected Overridable Sub Load()

            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_STOREORDERREVIEW_GETOBJECT As String = "sp_StoreOrderReview_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREORDERREVIEW_GETOBJECT)

                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)

                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderId"))) Then
                    m_OrderId = Convert.ToInt32(reader("OrderId"))
                Else
                    m_OrderId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                    m_MemberId = Convert.ToInt32(reader("MemberId"))
                Else
                    m_MemberId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemArrived"))) Then
                    m_ItemArrived = Convert.ToBoolean(reader("ItemArrived"))
                Else
                    m_ItemArrived = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemDescribed"))) Then
                    m_ItemDescribed = Convert.ToBoolean(reader("ItemDescribed"))
                Else
                    m_ItemDescribed = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ServicePrompt"))) Then
                    m_ServicePrompt = Convert.ToByte(reader("ServicePrompt"))
                Else
                    m_ServicePrompt = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NumStars"))) Then
                    m_NumStars = Convert.ToInt32(reader("NumStars"))
                Else
                    m_NumStars = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DateAdded"))) Then
                    m_DateAdded = Convert.ToDateTime(reader("DateAdded"))
                Else
                    m_DateAdded = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Comment"))) Then
                    m_Comment = reader("Comment").ToString()
                Else
                    m_Comment = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderNo"))) Then
                    m_OrderNo = reader("OrderNo").ToString()
                Else
                    m_OrderNo = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsFacebook"))) Then
                    m_IsFacebook = Convert.ToBoolean(reader("IsFacebook"))
                Else
                    m_IsFacebook = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsShared"))) Then
                    m_IsShared = Convert.ToBoolean(reader("IsShared"))
                Else
                    m_IsShared = False
                End If
            End If
        End Sub 'Load

        Public Overridable Sub Insert()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEMREVIEW_INSERT As String = "sp_StoreOrderReview_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMREVIEW_INSERT)

            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemArrived", DbType.Boolean, ItemArrived)
            db.AddInParameter(cmd, "ItemDescribed", DbType.Boolean, ItemDescribed)
            db.AddInParameter(cmd, "ServicePrompt", DbType.Byte, ServicePrompt)
            db.AddInParameter(cmd, "NumStars", DbType.Int32, NumStars)
            db.AddInParameter(cmd, "DateAdded", DbType.DateTime, DateTime.Now)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "Comment", DbType.String, Comment)
            db.ExecuteNonQuery(cmd)
            Utility.CacheUtils.ClearCacheWithPrefix(baseCachePrefixKey)
            '------------------------------------------------------------------------
        End Sub

        Public Overridable Sub Update()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEMREVIEW_UPDATE As String = "sp_StoreOrderReview_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMREVIEW_UPDATE)

            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemArrived", DbType.Boolean, ItemArrived)
            db.AddInParameter(cmd, "ItemDescribed", DbType.Boolean, ItemDescribed)
            db.AddInParameter(cmd, "ServicePrompt", DbType.Byte, ServicePrompt)
            db.AddInParameter(cmd, "NumStars", DbType.Int32, NumStars)
            db.AddInParameter(cmd, "DateAdded", DbType.DateTime, DateAdded)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "Comment", DbType.String, Comment)
            db.AddInParameter(cmd, "IsFacebook", DbType.Boolean, IsFacebook)
            db.AddInParameter(cmd, "IsShared", DbType.Boolean, IsShared)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
            ' ''clear cache item review
            Utility.CacheUtils.ClearCacheWithPrefix(baseCachePrefixKey)
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:47 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREORDERREVIEW_DELETE As String = "sp_StoreOrderReview_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREORDERREVIEW_DELETE)

            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)

            db.ExecuteNonQuery(cmd)

        End Sub 'Remove
    End Class
    Public Class StoreOrderReviewCollection
        Inherits GenericCollection(Of StoreOrderReviewRow)
        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreOrderReview As StoreOrderReviewRow)
            Me.List.Add(StoreOrderReview)
        End Sub

        Public Function Contains(ByVal StoreOrderReview As StoreOrderReviewRow) As Boolean
            Return Me.List.Contains(StoreOrderReview)
        End Function

        Public Function IndexOf(ByVal StoreOrderReview As StoreOrderReviewRow) As Integer
            Return Me.List.IndexOf(StoreOrderReview)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreOrderReview As StoreOrderReviewRow)
            Me.List.Insert(Index, StoreOrderReview)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreOrderReviewRow
            Get
                Return CType(Me.List.Item(Index), StoreOrderReviewRow)
            End Get

            Set(ByVal Value As StoreOrderReviewRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreOrderReview As StoreOrderReviewRow)
            Me.List.Remove(StoreOrderReview)
        End Sub

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
