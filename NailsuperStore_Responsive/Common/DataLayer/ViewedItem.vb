Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Database
Imports Components
Imports Components.Core
Imports System.Web
Imports Utility

Namespace DataLayer

    Public Class ViewedItemRow
        Inherits ViewedItemRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            MyBase.New(database, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal Id As Integer) As ViewedItemRow
            Dim row As ViewedItemRow

            row = New ViewedItemRow(_Database, Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal Id As Integer)
            Dim row As ViewedItemRow

            row = New ViewedItemRow(_Database, Id)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Sub Add(ByVal DB As Database, ByVal SessionNo As String, ByVal ItemId As Integer, ByVal MemberId As Integer)
            Dim Item As New ViewedItemRow(DB)

            'If DB.ExecuteScalar("select top 1 coalesce(itemid, 0) from vieweditem where sessionno = " & DB.Quote(SessionNo) & " and itemid = " & ItemId) <> 0 Then
            '    DB.ExecuteSQL("update vieweditem set createdate=getdate() where sessionno = " & DB.Quote(SessionNo) & " and itemid = " & ItemId)
            '    Exit Sub
            'End If

            If (IsExistedItem(SessionNo, ItemId) <> 0) Then
                UpdateViewed(SessionNo, ItemId)
                Exit Sub
            End If

            Item.SessionNo = SessionNo
            Item.ItemId = ItemId
            Item.MemberId = MemberId
            Item.Insert()
        End Sub 'Insert

        Private Shared Sub UpdateViewed(ByVal sessionNo As String, ByVal itemId As Integer)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_ViewedItem_UpdateViewed"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "SessionNo", DbType.String, sessionNo)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, Utility.Common.GetCurrentMemberId())
                db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)
                db.ExecuteNonQuery(cmd)
            Catch ex As Exception

            End Try

        End Sub

        Public Shared Sub UpdateMemberId(ByVal SessionNo As String, ByVal MemberId As Integer)
            If MemberId > 0 Then
                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim SP As String = "sp_ViewedItem_UpdateMemberId"
                    Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                    db.AddInParameter(cmd, "@SessionNo", DbType.String, SessionNo)
                    db.AddInParameter(cmd, "@MemberId", DbType.Int64, MemberId)
                    db.ExecuteNonQuery(cmd)
                Catch ex As Exception

                End Try
            End If
        End Sub

        Private Shared Function IsExistedItem(ByVal sessionNo As String, ByVal itemId As Integer) As Integer
            Dim id As Integer = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_VIEWEDITEM_GETLIST As String = "sp_ViewedItem_GetExistedItem"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETLIST)
            db.AddInParameter(cmd, "SessionNo", DbType.String, sessionNo)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)

            id = Convert.ToInt32(db.ExecuteScalar(cmd))
            Return id
        End Function

        Public Shared Function GetRecentlyViewed(ByVal DB As Database, ByVal SessionNo As String, ByVal memberId As Integer, ByVal orderId As Integer) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim r As SqlDataReader = Nothing
            Try
                r = GetRecentlyViewedReader(SessionNo, orderId, memberId)
                c = StoreItemRow.SetFieldItem(r, c)
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

            Return c
        End Function

        Public Shared Function GetRecentlyViewed(ByVal OrderId As Integer, ByVal MemberId As Integer, ByVal sessionNo As String, ByVal joinString As String) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim Context As System.Web.HttpContext = System.Web.HttpContext.Current
                Dim Language As String = Common.GetSiteLanguage
                Dim SP_VIEWEDITEM_GETLIST As String = "sp_ViewedItem_GetListBySession2"
                Dim customerPricegroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETLIST)
                db.AddInParameter(cmd, "SessionNo", DbType.String, sessionNo)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, customerPricegroupId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cmd, "JoinString", DbType.String, joinString)
                r = db.ExecuteReader(cmd)
                c = StoreItemRow.SetFieldItem(r, c)
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

            Return c
        End Function

        Public Shared Function GetRecentlyViewed1(ByVal Database As Database, ByVal SessionNo As String, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection

            Dim c As New StoreItemCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim Language As String = Common.GetSiteLanguage
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_VIEWEDITEM_GETLIST As String = "sp_ViewedItem_GetListBySessionV1"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETLIST)
                db.AddInParameter(cmd, "PageIndex", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "SessionNo", DbType.String, SessionNo)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                r = db.ExecuteReader(cmd)


                While r.Read

                    Dim i As New StoreItemRow(Database)
                    i.ItemId = r("itemid")
                    i.URLCode = r("URLCode")
                    i.Image = IIf(IsDBNull(r("image")), Nothing, r("Image"))
                    i.Price = r("price")
                    i.LowPrice = r("lowprice")
                    i.HighPrice = r("highprice")
                    i.LowSalePrice = IIf(IsDBNull(r("lowsaleprice")), Nothing, r("lowsaleprice"))
                    If Not IsDBNull(r("pricedesc")) Then
                        i.ItemName = r("itemname") & " - " & r("pricedesc")
                    Else
                        i.ItemName = r("itemname")
                    End If
                    i.ItemName2 = r("itemname")
                    'If LCase(Language) = "vn" Then
                    '    i.ShortDesc = IIf(IsDBNull(r("shortViet")), r("shortdesc"), r("shortViet"))
                    'ElseIf LCase(Language) = "ks" Then
                    '    i.ShortDesc = IIf(IsDBNull(r("shortKorea")), r("shortdesc"), r("shortKorea"))
                    'Else
                    i.ShortDesc = IIf(IsDBNull(r("shortdesc")), Nothing, r("shortdesc"))
                    'End If
                    i.ItemType = "Item"
                    If IsDBNull(r("itemgroupid")) Then i.ItemGroupId = Nothing Else i.ItemGroupId = r("itemgroupid")
                    i.QtyOnHand = r("QtyOnHand")
                    i.AcceptingOrder = CInt(r("AcceptingOrder"))
                    i.IsSpecialOrder = CBool(r("IsSpecialOrder"))
                    i.PermissionBuyBrand = r("PermissionBuyBrand")
                    i.IsFreeSample = CBool(r("IsFreeSample"))
                    i.IsVariance = CBool(r("IsVariance"))
                    i.IsFreeGift = CInt(r("IsFreeGift"))
                    i.IsFlammable = CBool(r("IsFlammable"))
                    'i.BrandId = CInt(r("BrandId"))
                    If IsDBNull(r("BrandId")) Then
                        i.BrandId = Nothing
                    Else
                        i.BrandId = CInt(r("BrandId"))
                    End If

                    If Not IsDBNull(r("choicename")) AndAlso Not i.ItemName2.Contains(r("choicename")) Then
                        i.ItemName2 &= " - " & r("choicename")
                    End If

                    If (r("PriceDesc") IsNot Nothing And Not IsDBNull(r("pricedesc"))) AndAlso Not i.ItemName2.Contains(r("PriceDesc")) Then
                        If r("choicename").ToString().Trim().Replace(" ", "") <> r("PriceDesc").ToString().Trim().Replace(" ", "") Then
                            i.ItemName2 &= " - " & r("PriceDesc")
                        End If
                    End If
                    i.PriceDesc = r("PriceDesc")
                    i.MixMatchDescription = IIf(IsDBNull(r("MixMatchDescription")), Nothing, r("MixMatchDescription"))
                    i.CountReview = r("CountReview")
                    i.AverageReview = r("AverageReview")
                    If filter.MemberId <> Nothing Then
                        i.ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(Database, i, False, 1, 0, filter.MemberId, True) & "</div>"
                        i.IsFlammable = IIf(IsDBNull(r("IsFlammable")), False, r("IsFlammable"))
                    End If
                    c.Add(i)



                End While
                Components.Core.CloseReader(r)
                totalRow = Convert.ToInt32(db.GetParameterValue(cmd, "TotalRecords"))
                For i As Integer = 0 To c.Count - 1
                    c(i).Promotions = c(i).GetPromotions()
                Next
                Return c
            Catch ex As Exception

            End Try
            Components.Core.CloseReader(r)
            Return Nothing
        End Function

        Private Shared Function GetRecentlyViewedReader(ByVal sessionNo As String, ByVal OrderId As Integer, ByVal MemberId As Integer) As SqlDataReader
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim Language As String = Common.GetSiteLanguage
            Dim SP_VIEWEDITEM_GETLIST As String = "sp_ViewedItem_GetListBySession"
            Dim customerPricegroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETLIST)

            db.AddInParameter(cmd, "SessionNo", DbType.String, sessionNo)
            db.AddInParameter(cmd, "Language", DbType.String, Language)
            db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, customerPricegroupId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            Return db.ExecuteReader(cmd)

            '------------------------------------------------------------------------
        End Function

        Private Shared Function GetRecentlyViewedReader(ByVal OrderId As Integer, ByVal MemberId As Integer, ByVal customerPricegroupId As Integer, ByVal sessionNo As String) As SqlDataReader
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim Context As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim Language As String = Common.GetSiteLanguage
            Dim SP_VIEWEDITEM_GETLIST As String = "sp_ViewedItem_GetListBySession2"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETLIST)
            db.AddInParameter(cmd, "SessionNo", DbType.String, sessionNo)
            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddInParameter(cmd, "Language", DbType.String, Language)
            db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, customerPricegroupId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            Return db.ExecuteReader(cmd)

            '------------------------------------------------------------------------
        End Function

    End Class



    Public MustInherit Class ViewedItemRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_SessionNo As String = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing


        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Property SessionNo() As String
            Get
                Return m_SessionNo
            End Get
            Set(ByVal Value As String)
                m_SessionNo = Value
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

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
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

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_VIEWEDITEM_GETOBJECT As String = "sp_ViewedItem_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_GETOBJECT)

                db.AddInParameter(cmd, "Id", DbType.Int32, Id)

                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SessionNo"))) Then
                        m_SessionNo = reader("SessionNo").ToString()
                    Else
                        m_SessionNo = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                        m_MemberId = Convert.ToInt32(reader("MemberId"))
                    Else
                        m_MemberId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CreateDate"))) Then
                        m_CreateDate = Convert.ToDateTime(reader("CreateDate"))
                    Else
                        m_CreateDate = DateTime.Now
                    End If
                End If
            Catch ex As Exception
                Throw ex


            End Try

        End Sub 'Load

        Public Overridable Sub Insert()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_VIEWEDITEM_INSERT As String = "sp_ViewedItem_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "SessionNo", DbType.String, SessionNo)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            'db.AddInParameter(cmd, "CreateDate", DbType.DateTime, DateTime.Now)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_VIEWEDITEM_INSERT As String = "sp_ViewedItem_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "SessionNo", DbType.String, SessionNo)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            'db.AddInParameter(cmd, "CreateDate", DbType.DateTime, DateTime.Now)

            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            '------------------------------------------------------------------------
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_VIEWEDITEM_UPDATE As String = "sp_ViewedItem_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_UPDATE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "SessionNo", DbType.String, SessionNo)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_VIEWEDITEM_DELETE As String = "sp_ViewedItem_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_VIEWEDITEM_DELETE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove

        Public Shared Function updateSearchResult(ByVal keyword As String, ByVal rawUrl As String, Optional ByVal pageType As String = Nothing) As Int16
            If String.IsNullOrEmpty(keyword) AndAlso pageType Is Nothing Then
                Return 0
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ViewedItem_UpdateSearchResult")
            db.AddInParameter(cmd, "KeywordName", DbType.String, keyword)
            db.AddInParameter(cmd, "SessionNo", DbType.String, HttpContext.Current.Session.SessionID)
            db.AddInParameter(cmd, "MemberId", DbType.String, Utility.Common.GetCurrentMemberId())
            db.AddInParameter(cmd, "UrlKeyword", DbType.String, rawUrl)
            If pageType IsNot Nothing Then
                db.AddInParameter(cmd, "PageType", DbType.String, pageType)
            End If
            Try
                Return db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Shared Function getLastUrlByPageType(ByVal pageType As String, Optional ByVal notSearchResult As Boolean = False) As String
            If String.IsNullOrEmpty(pageType) Then
                Return "/"
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim cmd As DbCommand = db.GetSqlStringCommand("select top 1 isnull(UrlKeyword, '/') from ViewedItem where (PageType = @PageType " & IIf(notSearchResult, "", "or KeywordName is not null") & ") and SessionNo = @SessionNo order by CreateDate desc")
            db.AddInParameter(cmd, "PageType", DbType.String, pageType)
            db.AddInParameter(cmd, "SessionNo", DbType.String, HttpContext.Current.Session.SessionID)
            Try
                Dim result As String = db.ExecuteScalar(cmd)
                If String.IsNullOrEmpty(result) Then
                    Return "/"
                Else
                    Return result
                End If
            Catch ex As Exception
                Return "/"
            End Try
        End Function
    End Class


    Public Class ViewedItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal ViewedItem As ViewedItemRow)
            Me.List.Add(ViewedItem)
        End Sub

        Public Function Contains(ByVal ViewedItem As ViewedItemRow) As Boolean
            Return Me.List.Contains(ViewedItem)
        End Function

        Public Function IndexOf(ByVal ViewedItem As ViewedItemRow) As Integer
            Return Me.List.IndexOf(ViewedItem)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal ViewedItem As ViewedItemRow)
            Me.List.Insert(Index, ViewedItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ViewedItemRow
            Get
                Return CType(Me.List.Item(Index), ViewedItemRow)
            End Get

            Set(ByVal Value As ViewedItemRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal ViewedItem As ViewedItemRow)
            Me.List.Remove(ViewedItem)
        End Sub
    End Class

End Namespace
