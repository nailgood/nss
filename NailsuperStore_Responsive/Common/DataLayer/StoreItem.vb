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
Imports System.Web.UI
Imports Utility
Imports System.Text.RegularExpressions

Namespace DataLayer
    Public Class Product
        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(value As String)
                m_Url = value
            End Set
        End Property
        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(value As String)
                m_Image = value
            End Set
        End Property
        Public Property Icon() As String
            Get
                Return m_Icon
            End Get
            Set(value As String)
                m_Icon = value
            End Set
        End Property
        Public Property Review() As String
            Get
                Return m_Review
            End Get
            Set(value As String)
                m_Review = value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(value As String)
                m_Title = value
            End Set
        End Property
        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(value As String)
                m_SKU = value
            End Set
        End Property
        Public Property Price() As String
            Get
                Return m_Price
            End Get
            Set(value As String)
                m_Price = value
            End Set
        End Property
        Public Property YouSave() As String
            Get
                Return m_YouSave
            End Get
            Set(value As String)
                m_YouSave = value
            End Set
        End Property
        Public Property Promotion() As String
            Get
                Return m_Promotion
            End Get
            Set(value As String)
                m_Promotion = value
            End Set
        End Property
        Public Property AddCart() As String
            Get
                Return m_AddCart
            End Get
            Set(value As String)
                m_AddCart = value
            End Set
        End Property
        Public Property InCart() As String
            Get
                Return m_InCart
            End Get
            Set(value As String)
                m_InCart = value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(value As Integer)
                m_ItemId = value
            End Set
        End Property

        Public Property Index() As Integer
            Get
                Return m_Index
            End Get
            Set(value As Integer)
                m_Index = value
            End Set
        End Property

        Public Property IsFeatured() As Boolean
            Get
                Return m_IsFeatured
            End Get
            Set(value As Boolean)
                m_IsFeatured = value
            End Set
        End Property

        Public Property IsHot() As Boolean
            Get
                Return m_IsHot
            End Get
            Set(value As Boolean)
                m_IsHot = value
            End Set
        End Property
        Public Property IsNew() As Boolean
            Get
                Return m_IsNew
            End Get
            Set(value As Boolean)
                m_IsNew = value
            End Set
        End Property

        Public Property IsBestSeller() As Boolean
            Get
                Return m_IsBestSeller
            End Get
            Set(value As Boolean)
                m_IsBestSeller = value
            End Set
        End Property

        Private m_Title As String
        Private m_Url As String
        Private m_Image As String
        Private m_Icon As String
        Private m_Review As String
        Private m_Price As String
        Private m_YouSave As String
        Private m_Promotion As String
        Private m_AddCart As String
        Private m_InCart As String
        Private m_ItemId As Integer
        Private m_Index As Integer
        Private m_IsFeatured As Boolean
        Private m_IsHot As Boolean
        Private m_IsNew As Boolean
        Private m_IsBestSeller As Boolean
        Private m_SKU As String
    End Class

    Public Class ItemPricing
        Public IsMixMatchPromotion As Boolean = False
        Public IsRangedPricing As Boolean = False
        Public BasePrice As Double = Nothing
        Public LowPrice As Double = Nothing
        Public HighPrice As Double = Nothing
        Public LowSellPrice As Double = Nothing
        Public HighSellPrice As Double = Nothing
        Public SellPrice As Double = Nothing
        Public IsPPU As Boolean = False
        Public PPU As DataTable
    End Class
    Public Class ItemPrice
        Public NormalPrice As Double = 0 '' item 1 gia
        Public RegularPrice As Double = False '' price truoc khi sales TH item 2 gia
        Public SalePrice As Double = 0 '' price sau khi sales Th  item 2 gia
        Public MultiPriceHTML As String = String.Empty '' neu item nhieu hon 2 price thi tra ra HTML table price
        Public MinMultiPrice As Double '' gia nho nhat cua item nhieu hon 2 price , lay ra de tinh point
        Public PercentSave As Double = 0
        Public YouSave As Double = 0
        Public PriceConvertPoint As Double = 0
        Public MultiPriceColection As List(Of ItemMultiPrice)
    End Class
    Public Class ItemMultiPrice
        Public MinQty As Integer = 0
        Public MaxQty As Integer = 0
        Public Price As Double = 0
    End Class
    Public Class StoreItemRow
        Inherits StoreItemRowBase
        Public Shared strXmlData As String
        Public Shared TotalRecords As Integer
        Public Shared IsQuickOrder As Boolean
        Private m_Promotion As PromotionRow
        Private m_Promotions As PromotionCollection
        Private m_Pricing As ItemPricing
        Public Shared dsFreeSample As DataSet
        Private m_youSave As Double
        Private m_savePercent As Double
        Private m_itemIndex As Integer
        Private Shared m_counter As Integer = 0
        Public Shared Property counter() As Integer
            Get
                Return m_counter
            End Get
            Set(ByVal value As Integer)
                m_counter = value
            End Set
        End Property
        Public Property itemIndex() As Integer
            Get
                Return m_itemIndex
            End Get
            Set(ByVal value As Integer)
                m_itemIndex = value
            End Set
        End Property
        Public Property youSave() As Double
            Get
                Return m_youSave
            End Get
            Set(ByVal value As Double)
                m_youSave = value
            End Set
        End Property
        Public Property savePercent() As Double
            Get
                Return m_savePercent
            End Get
            Set(ByVal value As Double)
                m_savePercent = value
            End Set
        End Property
        Public Property Pricing() As ItemPricing
            Get
                Return m_Pricing
            End Get
            Set(ByVal value As ItemPricing)
                m_Pricing = value
            End Set
        End Property

        Public Property Promotions() As PromotionCollection
            Get
                Return m_Promotions
            End Get
            Set(ByVal value As PromotionCollection)
                m_Promotions = value
            End Set
        End Property

        Public Property Promotion() As PromotionRow
            Get
                Return m_Promotion
            End Get
            Set(ByVal value As PromotionRow)
                m_Promotion = value
            End Set
        End Property

        Public Sub New()
            MyBase.New()
            IsLoginViewPrice = False
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer)
            MyBase.New(database, ItemId)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal OrderId As Integer, ByVal IsRewardPoints As Boolean)
            MyBase.New(database, ItemId, MemberId, OrderId, IsRewardPoints)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal SKU As String)
            MyBase.New(database, SKU)
        End Sub 'New

        Public Shared Function IsWebItemplateItem(ByVal sku As String) As Boolean
            Dim templateId As Integer = 0
            Dim packageId As Integer = 0
            StoreItemRow.GetTemplateItemData(sku, templateId, packageId)
            If (templateId > 0 And packageId > 0) Then
                Return True
            End If
            Return False
        End Function

        ''Shared function to get one row
        'Public Shared Function GetRow(ByVal _Database As Database, ByVal ItemId As Integer) As StoreItemRow
        '    Dim row As StoreItemRow
        '    Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
        '    Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}_", ItemId)
        '    row = CType(CacheUtils.GetCache(key), StoreItemRow)

        '    If row IsNot Nothing Then
        '        row.DB = _Database
        '        Initialize(_Database, row, CustomerPriceGroupId)
        '        Return row
        '    Else
        '        row = New StoreItemRow(_Database, ItemId)
        '        row.Load(CustomerPriceGroupId)
        '        Initialize2(_Database, row, CustomerPriceGroupId)
        '        CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
        '        Return row
        '    End If
        'End Function

        Public Shared Function GetRow(ByVal _Database As Database, ByVal ItemId As Integer, Optional ByVal MemberId As Integer = 0, Optional ByVal IsCache As Boolean = False, Optional ByVal IsPromotion As Boolean = True) As StoreItemRow
            Dim row As StoreItemRow
            Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}_{1}", ItemId, MemberId)
            row = CType(CacheUtils.GetCache(key), StoreItemRow)

            If row IsNot Nothing Then
                If Not IsCache Then
                    row.DB = _Database
                    If IsPromotion Then
                        Initialize(_Database, row, CustomerPriceGroupId)
                    End If
                End If

                Return row
            Else
                row = New StoreItemRow(_Database, ItemId)
                row.Load(CustomerPriceGroupId, MemberId)
                If IsPromotion Then
                    Initialize2(_Database, row, CustomerPriceGroupId)
                End If

                CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
                Return row
            End If
        End Function

        Public Shared Function GetRowFromCart(ByVal _Database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, Optional ByVal AddType As Integer = 1, Optional ByVal Qty As Integer = 1) As StoreItemRow
            Dim row As StoreItemRow
            Dim key As String = String.Format(cachePrefixKey & "GetRowFromCart_{0}_{1}_{2}_{3}", ItemId, MemberId, AddType, Qty)
            row = CType(CacheUtils.GetCache(key), StoreItemRow)

            If row IsNot Nothing Then
                Return row
            Else
                row = New StoreItemRow(_Database, ItemId)
                row.LoadFromCart(MemberId)
                InitializeFromCart(_Database, row, MemberId, Qty, AddType)

                CacheUtils.SetCache(key, row, 3600)
                Return row
            End If
        End Function

        Public Shared Function GetRow1(ByVal _Database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal OrderId As Integer, ByVal IsRewardPoints As Boolean) As StoreItemRow
            Dim row As New StoreItemRow(_Database, ItemId, MemberId, OrderId, IsRewardPoints)
            row.Load(ItemId, MemberId, OrderId, IsRewardPoints)
            Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize2(_Database, row, CustomerPriceGroupId)
            Return row
        End Function

        Public Shared Function GetRowInList(ByVal _Database As Database, ByVal ItemId As Integer, ByVal OrderId As Integer, ByVal MemberId As Integer) As StoreItemRow
            Dim row As New StoreItemRow(_Database, ItemId)

            Dim SQL As String = "SELECT ItemId,SKU,dbo.fc_CheckPermissionBuyBrand('" & MemberId.ToString() & "', si.BrandId) AS PermissionBuyBrand,ItemName,ItemNameNew,ItemGroupId,IsFreeGift,QtyOnHand,IsSpecialOrder,AcceptingOrder,URLCode,SalePrice,Price,IsFreeSample,IsFreeGift,IsFlammable,IsActive,Image,ImageAltTag, PriceDesc " & IIf(OrderId > 0, ", [dbo].fc_StoreCartItem_CheckItemInCart(" & OrderId.ToString() & ", ItemId,0) As IsInCart ", "") & " FROM StoreItem si WHERE si.ItemId = " & ItemId.ToString()
            Dim dr As SqlDataReader = Nothing
            Try
                dr = _Database.GetReader(SQL)

                If dr.HasRows Then
                    row = mapList(Of StoreItemRow)(dr).Item(0)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "GetRowInList", ex.ToString())
            End Try

            Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(_Database, row, CustomerPriceGroupId)
            Return row
        End Function

        Public Shared Function GetListAdmin(ByVal pIndex As Integer, ByVal pSize As Integer, ByVal condition As String, ByVal sortBy As String, ByVal sortExp As String, ByRef TotalRecords As Integer) As StoreItemCollection
            Dim si As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListAdmin"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)

                dr = db.ExecuteReader(cmd)

                While dr.Read
                    Dim m As StoreItemRow = LoadAdminList(dr)
                    si.Add(m)
                End While
                Core.CloseReader(dr)
                TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return si
        End Function
        Public Shared Function GetListColection(ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer) As StoreItemCollection
            Dim countdata As Integer = filter.pg * filter.MaxPerPage - filter.MaxPerPage
            Dim WhereCondition As String = String.Empty
            Dim Language As String = Common.GetSiteLanguage()
            Dim key As String = String.Format(DepartmentTabItemRow.cachePrefixKey & "GetListColection_{0}_{1}_{2}_{3}_{4}_{5}_{6}", filter.DepartmentId, filter.ItemId, filter.pg, filter.MaxPerPage, filter.MemberId, filter.OrderId, countdata)
            Dim si As StoreItemCollection
            'si = CType(CacheUtils.GetCache(key), StoreItemCollection)
            'If si Is Nothing Then
            si = New StoreItemCollection
            '    If filter.pg = 1 Then
            '        counter = 0
            '    End If
            'Else
            '    Return si
            'End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                Select Case filter.SortBy
                    Case "price"
                        filter.SortBy = lowSalePriceExp & " asc, " & LowPriceExp & " asc, price asc "
                    Case "product"
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                    Case "best-sellers"
                        filter.SortBy = " IsBestSeller desc, " & LowPriceExp & " asc, itemname asc "
                    Case "new-items"
                        filter.SortBy = " IsNew desc, " & LowPriceExp & " asc, itemname asc "
                    Case "hot-items"
                        filter.SortBy = " ishot desc, " & LowPriceExp & " asc, itemname asc "
                    Case "featured"
                        filter.SortBy = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                    'Case "on-sale"
                    '    filter.SortBy = hasSaleExp & " desc, " & lowSalePriceExp & " asc, itemname asc "
                    Case "top-rated"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetTopRatedSort](si.itemid) desc, itemname asc "
                    Case "most-popular-review"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetMostPopularReviewSort](si.itemid) desc, itemname asc "
                    Case Else
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                End Select
                If filter.IsFeatured Then
                    WhereCondition &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    WhereCondition &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    WhereCondition &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    WhereCondition &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    WhereCondition &= " and si.BrandId = " & Database.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListColection"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, filter.DepartmentId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, filter.ItemId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddInParameter(cmd, "OrderBy", DbType.String, filter.SortBy)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "WhereCondition", DbType.String, WhereCondition)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)
                si = SetFieldItem(dr, si)
                Core.CloseReader(dr)

                TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                CacheUtils.SetCache(key, si, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetListColection", "departmentId=" & filter.DepartmentId & ",ItemId=" & filter.ItemId & ",MemberId=" & filter.MemberId & ",OrderId=" & filter.OrderId & ",PageIndex=" & filter.pg & "<br>Exception: " & ex.ToString() + "")
            End Try

            Return si
        End Function

        Public Shared Function ListBySubCategory(ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim si As New StoreItemCollection
            Dim countdata As Integer = filter.pg * filter.MaxPerPage - filter.MaxPerPage

            If filter.pg = 1 Then
                counter = 0
            End If

            Dim dr As SqlDataReader = Nothing
            Try
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                Dim WhereCondition As String = String.Empty
                Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()

                Select Case filter.SortBy
                    Case "price"
                        filter.SortBy = lowSalePriceExp & " asc, " & LowPriceExp & " asc, price asc "
                    Case "pricehigh"
                        filter.SortBy = lowSalePriceExp & " desc, " & LowPriceExp & " desc, price desc "
                    Case "product"
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                    Case "best-sellers"
                        filter.SortBy = " IsBestSeller desc, " & LowPriceExp & " asc, itemname asc "
                    Case "new-items"
                        filter.SortBy = " HasNewItem desc, NewUntil DESC , itemname asc "
                    Case "hot-items"
                        filter.SortBy = " ishot desc, " & LowPriceExp & " asc, itemname asc "
                    Case "featured"
                        filter.SortBy = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                    Case "sku"
                        filter.SortBy = " SKU ASC "
                    Case "top-rated"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetTopRatedSort](si.itemid) desc, itemname asc "
                    Case "most-popular-review"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetMostPopularReviewSort](si.itemid) desc, itemname asc "
                    Case Else
                        If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                            filter.SortBy = " sd.Arrange ASC, "
                        End If
                        filter.SortBy &= " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                End Select


                WhereCondition &= " from storeitem si with (nolock) " & vbCrLf
                WhereCondition &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid  	" & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    WhereCondition &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If

                If filter.SalesCategoryId <> Nothing Then
                    WhereCondition &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                WhereCondition &= " Where si.IsActive = 1 and si.ItemType = 'item' " & vbCrLf
                If filter.Keyword = Nothing Then
                    WhereCondition &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If

                If Not filter.LoggedInPostingGroup = Nothing Then
                    WhereCondition &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    WhereCondition &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    WhereCondition &= " and HasNewItem = 1 " & vbCrLf
                End If
                If filter.IsHot Then
                    WhereCondition &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    WhereCondition &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    WhereCondition &= " and si.BrandId = " & Database.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                Try
                    If CInt(filter.Feature) > 0 Then
                        WhereCondition &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=" & Database.Number(CInt(filter.Feature)) & ")" & vbCrLf
                    End If
                Catch ex As Exception

                End Try

                If Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low
                    End If

                    'End If
                End If
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If

                    'End If
                End If

                Dim sCon As String = String.Empty
                If (filter.MinPrice > 0 Or filter.MaxPrice > 0) Then
                    If (filter.MinPrice > 0 And filter.MaxPrice <= 0) Then
                        WhereCondition &= " lowsaleprice >= " & filter.MinPrice
                    Else
                        WhereCondition &= " lowsaleprice >= " & filter.MinPrice & " AND lowsaleprice < " & filter.MaxPrice
                    End If
                    sCon = " AND "
                End If
                If (filter.MinRating > 0 Or filter.MaxRating > 0) Then
                    If (filter.MinRating > 0 And filter.MaxRating <= 0) Then
                        WhereCondition &= sCon & " TopRated >= " & filter.MinRating
                    Else
                        WhereCondition &= sCon & " TopRated >= " & filter.MinRating & " AND TopRated < " & filter.MaxRating
                    End If
                End If

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListDefaultItem"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, filter.DepartmentId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddInParameter(cmd, "OrderBy", DbType.String, filter.SortBy)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "WhereCondition", DbType.String, WhereCondition)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                db.AddOutParameter(cmd, "HasNewItem", DbType.Boolean, 1)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    si = SetFieldItem(dr, si)
                    si.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                    si.EnableHasNewItem = True
                    si.HasNewItem = CBool(cmd.Parameters("@HasNewItem").Value)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListBySubCategory " & ConfigData.GlobalRefererName, "departmentId=" & filter.DepartmentId & ",MemberId=" & filter.MemberId & ",OrderId=" & filter.OrderId & ",PageIndex=" & filter.pg & "<br>Exception: " & ex.ToString() + "")
            End Try

            Return si
        End Function

        Public Shared Function GetListDefaultItem(ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim si As New StoreItemCollection
            Dim countdata As Integer = filter.pg * filter.MaxPerPage - filter.MaxPerPage
            ' Dim key As String = String.Format(StoreItemRow.cachePrefixKey & "GetListDefaultItem_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}_{16}", filter.MemberId, filter.OrderId, filter.pg, filter.MaxPerPage, filter.DepartmentId, filter.BrandId, filter.PriceRange, filter.RatingRange, filter.SortBy, filter.ToneId, filter.ShadeId, filter.CollectionId, filter.IsOnSale, filter.IsFeatured, filter.HasPromotion, filter.PromotionId, countdata)
            'si = CType(CacheUtils.GetCache(key), StoreItemCollection)
            'If si Is Nothing Then
            'si = New StoreItemCollection
            If filter.pg = 1 Then
                counter = 0
            End If
            'Else
            'Return si
            'End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                Dim WhereCondition As String = String.Empty
                Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim hasSaleExp As String = String.Empty
                If (filter.Sale24Hour) Then
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](1," & filter.MemberId & ",'" & Now.ToShortDateString & "'," & CustomerPriceGroupId & ",si.itemid,si.price)"
                Else
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](0," & filter.MemberId & ",'" & Now.ToShortDateString & "'," & CustomerPriceGroupId & ",si.itemid,si.price)"
                End If
                Select Case filter.SortBy
                    Case "price"
                        filter.SortBy = lowSalePriceExp & " asc, " & LowPriceExp & " asc, price asc "
                    Case "pricehigh"
                        filter.SortBy = lowSalePriceExp & " desc, " & LowPriceExp & " desc, price desc "
                    Case "product"
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                    Case "best-sellers"
                        filter.SortBy = " IsBestSeller desc, " & LowPriceExp & " asc, itemname asc "
                    Case "new-items"
                        filter.SortBy = " IsNew desc, " & LowPriceExp & " asc, itemname asc "
                    Case "hot-items"
                        filter.SortBy = " ishot desc, " & LowPriceExp & " asc, itemname asc "
                    Case "featured"
                        filter.SortBy = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                    Case "on-sale"
                        filter.SortBy = hasSaleExp & " desc, " & lowSalePriceExp & " asc, itemname asc "
                    Case "top-rated"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetTopRatedSort](si.itemid) desc, itemname asc "
                    Case "most-popular-review"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetMostPopularReviewSort](si.itemid) desc, itemname asc "
                    Case Else
                        filter.SortBy = " IsFeatured DESC, ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                End Select


                WhereCondition &= " from storeitem si with (nolock) " & vbCrLf
                WhereCondition &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    WhereCondition &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    WhereCondition &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    WhereCondition &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    WhereCondition &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                If filter.SalesCategoryId <> Nothing Then
                    WhereCondition &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                WhereCondition &= " Where si.IsActive = 1 and si.ItemType = 'item' " & vbCrLf
                If filter.Keyword = Nothing Then
                    WhereCondition &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If

                ' If GroupItems Then SQLI &= " and si.itemgroupid is null " & vbCrLf
                If filter.IsSearchKeyWord Then
                    WhereCondition &= " and " & vbCrLf
                    WhereCondition &= "kw.KeywordName=" & Database.Quote(filter.Keyword) & vbCrLf
                Else
                    If filter.Keyword <> Nothing Then
                        WhereCondition &= " and " & vbCrLf
                        WhereCondition &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & Database.Quote(filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not filter.LoggedInPostingGroup = Nothing Then
                    WhereCondition &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    WhereCondition &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    WhereCondition &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    WhereCondition &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    WhereCondition &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    WhereCondition &= " and si.BrandId = " & Database.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    WhereCondition &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                Try
                    If CInt(filter.Feature) > 0 Then
                        '' SQLI &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                        'SQLI &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=(Select FeatureId from StoreFeature where URLCode = " & DB.Quote(filter.Feature) & "))" & vbCrLf
                        WhereCondition &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=" & Database.Number(CInt(filter.Feature)) & ")" & vbCrLf
                    End If
                Catch ex As Exception

                End Try

                If Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low
                    End If

                    'End If
                End If
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If

                    'End If
                End If

                If filter.HasPromotion Then

                    If filter.Sale24Hour Then
                        WhereCondition &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & Database.Quote(Now.ToShortDateString) & " and mm.endingdate < " & Database.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf filter.SaleBuy1Get1 Then
                        WhereCondition &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        WhereCondition &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If

                Dim sCon As String = String.Empty
                If Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        WhereCondition &= " and " & lowSalePriceExp & " >= " & low
                    End If
                    sCon = " AND "
                    'End If
                End If
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        WhereCondition &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If

                    'End If
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListDefaultItem"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, filter.DepartmentId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddInParameter(cmd, "OrderBy", DbType.String, filter.SortBy)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "WhereCondition", DbType.String, WhereCondition)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    si = SetFieldItem(dr, si)
                    si.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value) 'dung cho cache
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetListColection", "departmentId=" & filter.DepartmentId & ",MemberId=" & filter.MemberId & ",OrderId=" & filter.OrderId & ",PageIndex=" & filter.pg & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return si
        End Function

        Public Shared Function GetListItemPoint(ByVal pIndex As Integer, ByVal pSize As Integer, ByVal condition As String, ByVal sortBy As String, ByVal sortExp As String, ByRef TotalRecords As Integer) As StoreItemCollection
            Dim si As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListItemPoint"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)

                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim m As StoreItemRow = LoadAdminList(dr)
                    si.Add(m)
                End While
                Core.CloseReader(dr)
                TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return si
        End Function

        Private Shared Function LoadAdminList(ByVal dr As SqlDataReader) As StoreItemRow
            Dim m As New StoreItemRow
            Try
                m.ItemId = CInt(dr("ItemId"))
            Catch
                m.ItemId = 0
            End Try
            Try
                m.ItemName = dr("ItemName")
            Catch

            End Try
            Try
                m.SKU = dr("SKU")
            Catch

            End Try
            Try
                m.Price = IIf(IsDBNull(dr("Price")), Nothing, dr("Price"))
            Catch

            End Try
            ' m.SalePrice = IIf(IsDBNull(dr("SalePrice")), Nothing, dr("SalePrice"))
            Try
                m.Image = IIf(IsDBNull(dr("Image")), Nothing, dr("Image"))
            Catch

            End Try
            Try
                m.IsActive = dr("IsActive")
            Catch

            End Try
            Try
                m.IsHot = dr("IsHot")
            Catch

            End Try
            Try
                m.IsNew = dr("IsNew")
            Catch

            End Try
            Try
                m.IsBestSeller = dr("IsBestSeller")
            Catch

            End Try
            Try
                m.IsFreeSample = dr("IsFreeSample")
            Catch

            End Try
            Try
                m.IsFreeShipping = dr("IsFreeShipping")
            Catch

            End Try
            Try
                m.IsFlammable = IIf(IsDBNull(dr("IsFlammable")), Nothing, dr("IsFlammable"))
            Catch

            End Try
            Try
                m.IsHazMat = dr("IsHazMat")
            Catch

            End Try
            Try
                m.AcceptingOrder = dr("AcceptingOrder")
            Catch

            End Try
            Try
                m.IsSpecialOrder = dr("IsSpecialOrder")
            Catch

            End Try
            Try
                m.IsSellItemInEbay = dr("IsSellInEbay")
            Catch

            End Try
            Try
                m.IsSellInAmazon = dr("IsSellInAmazon")
            Catch

            End Try
            Try
                m.EbayId = IIf(IsDBNull(dr("EbayId")), Nothing, dr("EbayId"))
            Catch

            End Try
            Try
                m.IsOnSale = dr("IsOnSale")
            Catch

            End Try
            Try
                m.IsRewardPoints = IIf(IsDBNull(dr("IsRewardPoints")), Nothing, dr("IsRewardPoints"))
            Catch

            End Try
            Try
                m.RewardPoints = IIf(IsDBNull(dr("RewardPoints")), Nothing, dr("RewardPoints"))
            Catch

            End Try
            Try
                m.ArrangeRewardPoints = dr("ArrangeRewardPoints")
            Catch

            End Try
            Try
                m.QtyOnHand = dr("QtyOnHand")
            Catch

            End Try
            Try
                m.BrandName = IIf(IsDBNull(dr("BrandName")), Nothing, dr("BrandName"))
            Catch

            End Try
            Try
                m.PromotionCode = IIf(IsDBNull(dr("PromotionCode")), Nothing, dr("PromotionCode"))
            Catch

            End Try
            Try
                m.CountSalePrice = dr("iCount")
            Catch

            End Try
            Try
                m.CountCaseSalePrice = dr("iCaseCount")
            Catch

            End Try
            Try
                m.CasePrice = CDbl(dr("CasePrice"))
            Catch

            End Try
            Try
                m.CaseQty = CInt(dr("CaseQty"))
            Catch

            End Try

            Try
                m.Policy = dr("PolicyTitle")
            Catch

            End Try

            'm.IsEbay = IIf(IsDBNull(dr("IsEbay")), Nothing, dr("IsEbay"))
            'm.CountSalePrice = dr("CountSalePrice")
            'm.DepartmentName = IIf(IsDBNull(dr("DepartmentName")), Nothing, dr("DepartmentName"))
            Return m
        End Function
        Public Shared Function ListItemReWardsPointIdByOrder(ByVal orderId As Integer) As List(Of Integer)
            Dim result As New List(Of Integer)
            Dim dr As SqlDataReader = Nothing
            Try
                Dim itemId As Integer = 0
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ListItemReWardsPointIdByOrder"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, orderId)
                dr = db.ExecuteReader(cmd)
                While (dr.Read())
                    itemId = dr.Item("ItemId")
                    result.Add(itemId)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("ListItemReWardsPointIdByOrder(orderId=" & orderId & ")", ex)
            End Try

            Return result
        End Function
        Public Shared Function GetListDepartmentId(ByVal DB As Database, ByVal itemId As Integer) As String
            Dim SQL As String = "Select DepartmentId from StoreDepartmentItem  where ItemId=" & DB.Quote(itemId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim departmentId As String = String.Empty
                    While r.Read()
                        departmentId = r.Item("DepartmentId")
                        result = result & "," & departmentId
                    End While
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetListDepartmentId", ex)
            End Try

            Return result
        End Function
        Public Shared Function GetListBaseColorIdByItemId(ByVal DB As Database, ByVal itemId As Integer) As String
            Dim SQL As String = "Select BaseColorId from StoreBaseColorItem  where ItemId=" & DB.Quote(itemId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim BaseColorId As String = String.Empty
                    While r.Read()
                        BaseColorId = r.Item("BaseColorId")
                        result = result & "," & BaseColorId
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetListBaseColorIdByItemId", ex)
            End Try

            Return result
        End Function
        Public Shared Function GetListCusionColorIdByItemId(ByVal DB As Database, ByVal itemId As Integer) As String
            Dim SQL As String = "Select CusionColorId from StoreCusionColorItem  where ItemId=" & DB.Quote(itemId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim CusionColorId As String = String.Empty
                    While r.Read()
                        CusionColorId = r.Item("CusionColorId")
                        result = result & "," & CusionColorId
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetListCusionColorIdByItemId", ex)
            End Try
            Return result
        End Function
        Public Shared Function GetListLaminateColorIdByItemId(ByVal DB As Database, ByVal itemId As Integer) As String
            Dim SQL As String = "Select LaminateTrimId from StoreLaminateTrimItem where ItemId=" & DB.Quote(itemId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim LaminateTrimId As String = String.Empty
                    While r.Read()
                        LaminateTrimId = r.Item("LaminateTrimId")
                        result = result & "," & LaminateTrimId
                    End While
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetListCusionColorIdByItemId", ex)
            End Try

            Return result
        End Function

        Public Shared Function GetListPostingGroupCode(ByVal DB As Database, ByVal itemId As Integer) As String
            Dim SQL As String = "Select Code from storeitemcustomerpostinggroup  where ItemId=" & DB.Quote(itemId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If r.HasRows Then
                    Dim code As String = String.Empty
                    While r.Read()
                        code = r.Item("Code")
                        result = result & "," & code
                    End While
                End If
                Core.CloseReader(r)

            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("ListPostingGroupCode", ex)
            End Try

            Return result
        End Function
        Public Shared Function GetRowInShipping(ByVal _Database As Database, ByVal ItemId As Integer) As StoreItemRow
            Dim row As StoreItemRow
            Dim key As String = String.Format(cachePrefixKey & "GetRowInShipping_{0}_", ItemId)
            row = CType(CacheUtils.GetCache(key), StoreItemRow)
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If
            row = New StoreItemRow(_Database, ItemId)
            row.LoadInShipping()
            CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
            Return row
        End Function

        Public Shared Function GetRowInCart(ByVal _Database As Database, ByVal ItemId As Integer, ByVal memberId As Integer) As StoreItemRow
            Dim row As New StoreItemRow(_Database, ItemId)
            row.LoadInCart(memberId)

            Return row
        End Function
        Public Shared Function UpdateQtyOnHand(ByVal _Database As Database, ByVal ItemId As Integer, ByVal qty As Integer) As Boolean
            Try
                Dim SQL As String = " UPDATE StoreItem SET QtyOnHand = " & _Database.Quote(qty) & "  WHERE ItemId = " & _Database.Quote(ItemId)
                _Database.ExecuteSQL(SQL)
                Dim storeItemRow As New StoreItemRow()
                storeItemRow.UpdateQtyEbayItem(_Database, ItemId, qty)
                storeItemRow.UpdateQtyAmazonItem(_Database, ItemId, qty)
                StoreItemRow.ClearItemCache(ItemId)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function UpdatePrice(ByVal _Database As Database, ByVal ItemId As Integer, ByVal Price As Double) As Boolean
            Try
                Dim SQL As String = " UPDATE StoreItem SET Price = " & _Database.Quote(Price) & "  WHERE ItemId = " & _Database.Quote(ItemId)
                _Database.ExecuteSQL(SQL)
                StoreItemRow.ClearItemCache(ItemId)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Sub CountViewItem(ByVal _Database As Database, ByVal MemberId As Integer, ByVal ItemId As Integer)
            Try
                Dim sp As String = "sp_StoreItem_CountViewItem"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CountViewItem", "MemberId:=" & MemberId & ",ItemId=" & ItemId & "<br>Exception: " & ex.ToString() + "")

            End Try

        End Sub
        Public Shared Sub CountAddCartItem(ByVal _Database As Database, ByVal MemberId As Integer, ByVal ItemId As Integer)
            Try
                Dim sp As String = "sp_StoreItem_CountAddCartItem"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, ItemId))
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CountAddCartItem", "MemberId:=" & MemberId & ",ItemId=" & ItemId & "<br>Exception: " & ex.ToString() + "")
            End Try

        End Sub
        Public Shared Function SendEndEbayItem(ByVal _Database As Database, ByVal ItemId As Integer, ByVal staus As String) As Boolean
            Try
                Dim SQL As String = " UPDATE StoreItem SET IsEbayEnd = " & staus & "  WHERE ItemId = " & _Database.Quote(ItemId)
                _Database.ExecuteSQL(SQL)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function DoActive(ByVal _Database As Database, ByVal ItemId As Integer) As Boolean
            Try

                Dim SQL As String = " UPDATE StoreItem SET IsActive = ~IsActive WHERE ItemId = " & ItemId
                _Database.ExecuteSQL(SQL)
                StoreItemRowBase.ClearItemCache(ItemId)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        'Public Shared Function GetRowByMemberLogin(ByVal _Database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal url As String) As StoreItemRow
        '    Dim checkItemPoint As Boolean
        '    If (url.Contains("rewardpoint.aspx")) Then
        '        checkItemPoint = True
        '    Else
        '        checkItemPoint = False
        '    End If
        '    Dim row As StoreItemRow
        '    row = New StoreItemRow(_Database)
        '    row.LoadByMemberLoginList(ItemId, MemberId, checkItemPoint)
        '    Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
        '    Initialize(_Database, row, customerPriceGroup)
        '    Return row
        'End Function
        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreItem.vb", func, ex)
        End Sub
        Public Shared Function GetRowURLCodeById(ByVal ItemId As Integer) As String
            Dim result As String
            Dim key As String = String.Format(cachePrefixKey & "GetRowURLCodeById_{0}", ItemId)
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select URLCode from StoreItem where ItemId=" & ItemId)
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowURLCodeById(ByVal ItemId As Integer)", ex)
            End Try
            Return result
        End Function
        Public Shared Function GetRowLogAdminById(ByVal ItemId As Integer) As StoreItemRow
            Dim result As New StoreItemRow
            result.ItemId = ItemId
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select SKU,ItemName from StoreItem where ItemId=" & ItemId)
                If reader.Read() Then
                    result.SKU = reader.Item("SKU")
                    result.ItemName = reader.Item("ItemName")
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowLogAdminById(ByVal ItemId As Integer)", ex)
            End Try
            Return result
        End Function
        Public Shared Sub GetTemplateItemData(ByVal sku As String, ByRef templateId As Integer, ByRef packageId As Integer)
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select TemplateId,PackageId from TemplatePackageItem where ItemSKU='" & sku & "'")
                If reader.Read() Then
                    If IsDBNull(reader.Item("TemplateId")) Then
                        templateId = Nothing
                    Else
                        templateId = Convert.ToInt32(reader.Item("TemplateId"))
                    End If

                    If IsDBNull(reader.Item("PackageId")) Then
                        packageId = Nothing
                    Else
                        packageId = Convert.ToInt32(reader.Item("PackageId"))
                    End If
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetTemplateItemData(ByVal ItemId As Integer)", ex)
            End Try
        End Sub

        Public Shared Function GetEbayRelatedItemList(ByVal ItemId As Integer) As DataSet
            If ItemId < 1 Then
                Return Nothing
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim ds As New DataSet
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Ebay_RealatedItem")
                Dim pItemId As New SqlParameter()
                pItemId.ParameterName = "ItemId"
                pItemId.Value = ItemId
                pItemId.Direction = ParameterDirection.Input
                cmd.Parameters.Add(pItemId)
                ds = db.ExecuteDataSet(cmd)
                Return ds
            Catch ex As Exception
                SendMailLog("GetEbayRelatedItemList(ByVal ItemId As Integer)", ex)
            End Try
            Return Nothing
        End Function
        Public Shared Function GetRowURLBySKU(ByVal sku As String) As String
            Dim result As String
            Dim key As String = String.Format(cachePrefixKey & "GetRowURLCodeBySKU_{0}", sku)
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select URLCode + CAST('/' as varchar) + CAST(ItemId  as varchar) from StoreItem where SKU='" & sku & "'")
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowURLCodeBySKU(ByVal sku As String)", ex)
            End Try
            Return result
        End Function
        Public Shared Function GetRowURLCodeBySKU(ByVal sku As String) As String
            Dim result As String
            Dim key As String = String.Format(cachePrefixKey & "GetRowURLCodeBySKU_{0}", sku)
            result = CType(CacheUtils.GetCache(key), String)
            If result <> "" And Not result Is Nothing Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select URLCode from StoreItem where SKU='" & sku & "'")
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowURLCodeBySKU(ByVal sku As String)", ex)
            End Try
            Return result
        End Function
        Public Shared Function IsSellInEbay(ByVal ItemId As Integer) As Boolean
            Dim result As String = ""
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select [dbo].[fc_Ebay_ItemIsSellInEbay](" & ItemId & ")")
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("IsSellInEbay(ByVal ItemId As Integer)", ex)
            End Try
            If (result = "1") Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function GetIdByURLCode(ByVal urlCode As String) As Integer
            If urlCode Is Nothing Or urlCode = String.Empty Then
                Return 0
            End If
            urlCode = urlCode.Replace("'", "")
            Dim result As Integer
            Dim key As String = String.Format(cachePrefixKey & "GetIdByURLCode_{0}", urlCode)
            result = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select top 1 ItemId from StoreItem where URLCode='" & urlCode & "'")
                If reader.Read() Then
                    result = CInt(reader.GetValue(0).ToString())
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetIdByURLCode(ByVal urlCode(" & urlCode & ") As String)", ex)
            End Try
            Return result
        End Function
        Public Shared Function GetIdBySKU(ByVal sku As String) As Integer
            If sku Is Nothing Or sku = String.Empty Then
                Return 0
            End If
            sku = sku.Replace("'", "")
            Dim result As Integer
            Dim key As String = String.Format(cachePrefixKey & "GetIdBySKU_{0}", sku)
            result = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "Select top 1 ItemId from StoreItem where SKU='" & sku & "'")
                If reader.Read() Then
                    result = CInt(reader.GetValue(0).ToString())
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetIdBySKU(ByVal sku(" & sku & ") As String)", ex)
            End Try
            Return result
        End Function

        Public Shared Function GetByURLCode(ByVal _Database As Database, ByVal urlCode As String) As StoreItemRow
            Dim reader As SqlDataReader = Nothing
            Dim result As StoreItemRow = Nothing
            Try
                Dim Language As String = Common.GetSiteLanguage()
                Dim key As String = String.Format(cachePrefixKey & "GetByURLCode_{0}_{1}", urlCode, Language)
                result = CType(CacheUtils.GetCache(key), StoreItemRow)
                If Not result Is Nothing Then
                    result.DB = _Database
                    result.Promotion = Nothing
                    Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                    Initialize(_Database, result, customerPriceGroup)
                    Return result
                End If
                Dim selectFields As String = "ItemId,Category,ItemType,ItemName,ItemGroupId,IsCollection,BrandId,SKU,Weight,Price,SalePrice,PageTitle,OutsideUSPageTitle,MetaDescription,OutsideUSMetaDescription,MetaKeywords,ShipmentDate,PriceDesc,Image,ImageAltTag,DeliveryTime,CarrierType,Status,InvMsgId,CreateDate,ModifyDate,QtyOnHand,InventoryStockNotification,BODate,LowStockMsg,LowStockThreshold,QtyReserved,LastUpdated,AdditionalInfo,Specifications,ShippingInfo,HelpfulTips,MSDS,IsFeatured,IsActive,IsNew,IsBestSeller,IsTaxFree,IsFreeShipping,IsOnSale,NewUntil,Prefix,IsOversize,IsHazMat,DoExport,LastExport,LastImport,MaximumQuantity,IsRushDelivery,RushDeliveryCharge,IsHot,LiftGateCharge,ScheduleDeliveryCharge,IsSpecialOrder,AcceptingOrder,TaxGroupCode,ChoiceName,PromotionID,Measurement,IsFreeSample,FreeSampleArrange,IsFreeGift,UrlCode,IsFlatFee,FeeShipOversize,IsFlammable,EbayPrice,IsEbay,IsEbayAllow,EbayShippingType,IsRewardPoints,RewardPoints"
                ''selectFields = selectFields.Replace("ShortDesc,LongDesc,ShortViet,LongViet,ShortFrench,LongFrench,ShortSpanish,LongSpanish,", "")
                Dim sql As String = "Select " & selectFields & "," & Common.GenerateCaseLanguage("Short", Language) & " as ShortDesc" & "," & Common.GenerateCaseLanguage("Long", Language) & " as LongDesc from StoreItem where URLCode='" & urlCode.Replace("'", "''") & "'"
                reader = _Database.GetReader(sql)
                If reader.HasRows Then
                    If reader.Read() Then
                        result = LoadByReader(reader)
                    End If

                    If Not result Is Nothing Then
                        CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
                        result.DB = _Database
                        Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                        Initialize(_Database, result, customerPriceGroup)
                    End If
                End If

                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                result = Nothing
                SendMailLog("GetByURLCode(ByVal urlCode As String) As StoreItemRow", ex)
            End Try

            Return result
        End Function

        Public Shared Function GetByURLCode301(ByVal _Database As Database, ByVal urlCode As String) As Integer
            Dim ItemId As Integer = 0
            Dim result As StoreItemRow = Nothing
            Try
                ItemId = CInt(_Database.ExecuteScalar("Select ItemId from StoreItem where URLCode='" & urlCode.Replace("'", "''") & "'"))
            Catch ex As Exception
                result = Nothing
                SendMailLog("GetByURLCode301(ByVal urlCode As String) As StoreItemRow", ex)
            End Try

            Return ItemId
        End Function

        Protected Shared Function LoadByReader(ByVal r As IDataReader) As StoreItemRow
            Try
                Dim item As New StoreItemRow
                item.ItemId = r.Item("ItemId")
                If IsDBNull(r.Item("Weight")) Then
                    item.Weight = Nothing
                Else
                    item.Weight = Convert.ToDouble(r.Item("Weight"))
                End If

                item.ItemName = Convert.ToString(r.Item("ItemName"))
                If r.Item("ItemGroupId") Is Convert.DBNull Then
                    item.ItemGroupId = Nothing
                Else
                    item.ItemGroupId = r.Item("ItemGroupId")
                End If

                If r.Item("SKU") Is Convert.DBNull Then
                    item.SKU = Nothing
                Else
                    item.SKU = Convert.ToString(r.Item("SKU"))
                End If

                If r.Item("Measurement") Is Convert.DBNull Then
                    item.Measurement = Nothing
                Else
                    item.Measurement = Convert.ToString(r.Item("Measurement"))
                End If
                If r.Item("IsFreeGift") Is Convert.DBNull Then
                    item.IsFreeGift = Nothing
                Else
                    item.IsFreeGift = Convert.ToInt32(r.Item("IsFreeGift"))
                End If
                If r.Item("ItemType") Is Convert.DBNull Then
                    item.ItemType = Nothing
                Else
                    item.ItemType = Convert.ToString(r.Item("ItemType"))
                End If
                If r.Item("ImageAltTag") Is Convert.DBNull Then
                    item.ImageAltTag = Nothing
                Else
                    item.ImageAltTag = Convert.ToString(r.Item("ImageAltTag"))
                End If

                If r.Item("PriceDesc") Is Convert.DBNull Then
                    item.PriceDesc = Nothing
                Else
                    item.PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                End If

                Try
                    If r.Item("PermissionBuyBrand") Is Convert.DBNull Then
                        item.PermissionBuyBrand = True
                    Else
                        item.PermissionBuyBrand = CBool(r.Item("PermissionBuyBrand"))
                    End If
                Catch ex As Exception
                    item.PermissionBuyBrand = True
                End Try

                Try
                    If r.Item("IsInCart") Is Convert.DBNull Then
                        item.IsInCart = True
                    Else
                        item.IsInCart = CBool(r.Item("IsInCart"))
                    End If
                Catch ex As Exception
                    item.IsInCart = True
                End Try

                If IsDBNull(r.Item("MaximumQuantity")) Then
                    item.MaximumQuantity = Nothing
                Else
                    item.MaximumQuantity = Convert.ToInt32(r.Item("MaximumQuantity"))
                End If
                If r.Item("Price") Is Convert.DBNull Then
                    item.Price = Nothing
                Else
                    item.Price = Convert.ToDouble(r.Item("Price"))
                End If
                If r.Item("SalePrice") Is Convert.DBNull Then
                    item.SalePrice = Nothing
                Else
                    item.SalePrice = Convert.ToDouble(r.Item("SalePrice"))
                End If
                If r.Item("PageTitle") Is Convert.DBNull Then
                    item.PageTitle = Nothing
                Else
                    item.PageTitle = Convert.ToString(r.Item("PageTitle"))
                End If
                Try
                    If r.Item("OutsideUSPageTitle") Is Convert.DBNull Then
                        item.OutsideUSPageTitle = Nothing
                    Else
                        item.OutsideUSPageTitle = Convert.ToString(r.Item("OutsideUSPageTitle"))
                    End If
                Catch ex As Exception

                End Try


                If r.Item("PageTitle") Is Convert.DBNull Then
                    item.PageTitle = Nothing
                Else
                    item.PageTitle = Convert.ToString(r.Item("PageTitle"))
                End If

                If r.Item("Prefix") Is Convert.DBNull Then
                    item.Prefix = Nothing
                Else
                    item.Prefix = Convert.ToString(r.Item("Prefix"))
                End If
                If r.Item("MetaDescription") Is Convert.DBNull Then
                    item.MetaDescription = Nothing
                Else
                    item.MetaDescription = Convert.ToString(r.Item("MetaDescription"))
                End If
                Try
                    If r.Item("OutsideUSMetaDescription") Is Convert.DBNull Then
                        item.OutsideUSMetaDescription = Nothing
                    Else
                        item.OutsideUSMetaDescription = Convert.ToString(r.Item("OutsideUSMetaDescription"))
                    End If

                Catch ex As Exception

                End Try
                If r.Item("MetaKeywords") Is Convert.DBNull Then
                    item.MetaKeywords = Nothing
                Else
                    item.MetaKeywords = Convert.ToString(r.Item("MetaKeywords"))
                End If
                item.IsOnSale = Convert.ToBoolean(r.Item("IsOnSale"))
                item.IsActive = Convert.ToBoolean(r.Item("IsActive"))
                item.IsNew = Convert.ToBoolean(r.Item("IsNew"))
                item.IsBestSeller = Convert.ToBoolean(r.Item("IsBestSeller"))
                item.IsTaxFree = Convert.ToBoolean(r.Item("IsTaxFree"))
                If r.Item("NewUntil") Is Convert.DBNull Then
                    item.NewUntil = Nothing
                Else
                    item.NewUntil = Convert.ToDateTime(r.Item("NewUntil"))
                End If
                If r.Item("ShipmentDate") Is Convert.DBNull Then
                    item.ShipmentDate = Nothing
                Else
                    item.ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                End If
                If r.Item("Image") Is Convert.DBNull Then
                    item.Image = Nothing
                Else
                    item.Image = Convert.ToString(r.Item("Image"))
                End If
                If r.Item("DeliveryTime") Is Convert.DBNull Then
                    item.DeliveryTime = Nothing
                Else
                    item.DeliveryTime = Convert.ToString(r.Item("DeliveryTime"))
                End If
                If r.Item("InvMsgId") Is Convert.DBNull Then
                    item.InvMsgId = Nothing
                Else
                    item.InvMsgId = Convert.ToInt32(r.Item("InvMsgId"))
                End If
                If r.Item("CarrierType") Is Convert.DBNull Then
                    item.CarrierType = Nothing
                Else
                    item.CarrierType = Convert.ToString(r.Item("CarrierType"))
                End If
                If r.Item("Status") Is Convert.DBNull Then
                    item.Status = Nothing
                Else
                    item.Status = Convert.ToString(r.Item("Status"))
                End If
                If r.Item("QtyOnHand") Is Convert.DBNull Then
                    item.QtyOnHand = Nothing
                Else
                    item.QtyOnHand = Convert.ToInt32(r.Item("QtyOnHand"))
                End If
                If r.Item("InventoryStockNotification") Is Convert.DBNull Then
                    item.InventoryStockNotification = Nothing
                Else
                    item.InventoryStockNotification = Convert.ToInt32(r.Item("InventoryStockNotification"))
                End If
                If r.Item("LowStockMsg") Is Convert.DBNull Then
                    item.LowStockMsg = Nothing
                Else
                    item.LowStockMsg = Convert.ToString(r.Item("LowStockMsg"))
                End If
                If r.Item("LowStockThreshold") Is Convert.DBNull Then
                    item.LowStockThreshold = Nothing
                Else
                    item.LowStockThreshold = Convert.ToInt32(r.Item("LowStockThreshold"))
                End If
                If r.Item("QtyReserved") Is Convert.DBNull Then
                    item.QtyReserved = Nothing
                Else
                    item.QtyReserved = Convert.ToString(r.Item("QtyReserved"))
                End If
                If r.Item("LastUpdated") Is Convert.DBNull Then
                    item.LastUpdated = Nothing
                Else
                    item.LastUpdated = Convert.ToString(r.Item("LastUpdated"))
                End If
                If r.Item("ShortDesc") Is Convert.DBNull Then
                    item.ShortDesc = Nothing
                Else
                    item.ShortDesc = Convert.ToString(r.Item("ShortDesc"))
                End If
                If r.Item("LongDesc") Is Convert.DBNull Then
                    item.LongDesc = Nothing
                Else
                    item.LongDesc = Convert.ToString(r.Item("LongDesc"))
                End If
                If r.Item("MSDS") Is Convert.DBNull Then
                    item.MSDS = Nothing
                Else
                    item.MSDS = Convert.ToString(r.Item("MSDS"))
                End If
                item.IsFeatured = Convert.ToBoolean(r.Item("IsFeatured"))
                item.IsCollection = Convert.ToBoolean(r.Item("IsCollection"))
                item.IsOversize = Convert.ToBoolean(r.Item("IsOversize"))
                item.IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                item.IsRushDelivery = Convert.ToBoolean(r.Item("IsRushDelivery"))
                If IsDBNull(r.Item("RushDeliveryCharge")) Then
                    item.RushDeliveryCharge = Nothing
                Else
                    item.RushDeliveryCharge = r.Item("RushDeliveryCharge")
                End If
                item.DoExport = Convert.ToBoolean(r.Item("DoExport"))
                If IsDBNull(r.Item("BODate")) Then
                    item.BODate = Nothing
                Else
                    item.BODate = Convert.ToDateTime(r.Item("BODate"))
                End If
                If IsDBNull(r.Item("BrandId")) Then
                    item.BrandId = Nothing
                Else
                    item.BrandId = Convert.ToInt32(r.Item("BrandId"))
                End If
                If IsDBNull(r.Item("Category")) Then
                    item.Category = Nothing
                Else
                    item.Category = Convert.ToString(r.Item("Category"))
                End If
                If IsDBNull(r.Item("AdditionalInfo")) Then
                    item.AdditionalInfo = Nothing
                Else
                    item.AdditionalInfo = Convert.ToString(r.Item("AdditionalInfo"))
                End If
                If IsDBNull(r.Item("Specifications")) Then
                    item.Specifications = Nothing
                Else
                    item.Specifications = Convert.ToString(r.Item("Specifications"))
                End If
                If IsDBNull(r.Item("ShippingInfo")) Then
                    item.ShippingInfo = Nothing
                Else
                    item.ShippingInfo = Convert.ToString(r.Item("ShippingInfo"))
                End If
                If IsDBNull(r.Item("HelpfulTips")) Then
                    item.HelpfulTips = Nothing
                Else
                    item.HelpfulTips = Convert.ToString(r.Item("HelpfulTips"))
                End If
                If IsDBNull(r.Item("LastImport")) Then
                    item.LastImport = Nothing
                Else
                    item.LastImport = Convert.ToString(r.Item("LastImport"))
                End If
                If IsDBNull(r.Item("LastExport")) Then
                    item.LastExport = Nothing
                Else
                    item.LastExport = Convert.ToString(r.Item("LastExport"))
                End If
                ''''
                If Not IsDBNull(r.Item("LiftGateCharge")) Then item.LiftGateCharge = Convert.ToDouble(r.Item("LiftGateCharge")) Else item.LiftGateCharge = Nothing
                If Not IsDBNull(r.Item("ScheduleDeliveryCharge")) Then item.ScheduleDeliveryCharge = Convert.ToDouble(r.Item("ScheduleDeliveryCharge")) Else item.ScheduleDeliveryCharge = Nothing
                item.TaxGroupCode = Convert.ToString(r.Item("TaxGroupCode"))
                item.IsHot = Convert.ToBoolean(r.Item("IsHot"))
                item.IsSpecialOrder = Convert.ToBoolean(r.Item("IsSpecialOrder"))
                item.AcceptingOrder = Convert.ToInt32(r.Item("AcceptingOrder"))
                item.IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                item.IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))

                If IsDBNull(r.Item("PromotionId")) Then
                    item.PromotionId = Nothing
                Else
                    item.PromotionId = Convert.ToInt32(r.Item("PromotionId"))
                End If
                'm_InOutStock = Convert.ToString(r.Item("InOutStock"))
                'm_DepartmentId = Convert.ToInt32(r.Item("DepartmentId"))
                If IsDBNull(r.Item("URLCode")) Then
                    item.URLCode = Nothing
                Else
                    item.URLCode = Convert.ToString(r.Item("URLCode"))
                End If
                Try
                    If r.Item("IsFlatFee") Is Convert.DBNull Then
                        item.IsFlatFee = True
                    Else
                        item.IsFlatFee = CBool(r.Item("IsFlatFee"))
                    End If
                Catch ex As Exception
                    item.IsFlatFee = False
                End Try
                If IsDBNull(r.Item("FeeShipOversize")) Then
                    item.FeeShipOversize = Nothing
                Else
                    item.FeeShipOversize = CDbl(r.Item("RushDeliveryCharge"))
                End If
                Try
                    If r.Item("IsFlammable") Is Convert.DBNull Then
                        item.IsFlammable = False
                    Else
                        item.IsFlammable = CBool(r.Item("IsFlammable"))
                    End If
                Catch ex As Exception
                    item.IsFlammable = False
                End Try

                If r.Item("IsRewardPoints") Is Convert.DBNull Then
                    item.IsRewardPoints = False
                Else
                    item.IsRewardPoints = CBool(r.Item("IsRewardPoints"))
                End If
                If IsDBNull(r.Item("RewardPoints")) Then
                    item.RewardPoints = Nothing
                Else
                    item.RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
                End If
                If r.Item("CreateDate") Is Convert.DBNull Then
                    item.CreateDate = Nothing
                Else
                    item.CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                End If
                If r.Item("ModifyDate") Is Convert.DBNull Then
                    item.ModifyDate = Nothing
                Else
                    item.ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
                Return item
            Catch ex As Exception
                Throw ex
            End Try

        End Function 'Load


        Public Shared Function GetRow(ByVal _Database As Database, ByVal SKU As String) As StoreItemRow
            Dim row As StoreItemRow

            row = New StoreItemRow(_Database, SKU)
            'row.Load()
            row.Load(SKU)
            Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(_Database, row, customerPriceGroup)

            Return row
        End Function

        Public Shared Function GetRowSku(ByVal _Database As Database, ByVal SKU As String) As StoreItemRow
            Dim row As StoreItemRow

            row = New StoreItemRow(_Database, SKU)
            'row.Load()
            row.Load(SKU)
            Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(_Database, row, customerPriceGroup)

            Return row
        End Function

        Public Shared Function GetItemNameByItemId(ByVal DB As Database, ByVal ItemId As Integer) As String
            Dim dr As SqlDataReader = Nothing
            Dim Result As String = String.Empty
            Try
                Dim sql As String = String.Format("Select ItemName from StoreItem where ItemId={0}", ItemId)
                dr = DB.GetReader(sql)
                If dr.Read() Then
                    Result = dr.GetString(0)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetItemNameByItemId", ex)
            End Try
            Return Result
        End Function

        Public Shared Function ListItemBuyPoint() As StoreItemCollection
            Dim sci As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim key As String = StoreItemRow.cachePrefixKey & "ListItemBuyPoint"
            Try
                sci = CType(CacheUtils.GetCache(key), StoreItemCollection)
                If sci Is Nothing Then
                    sci = New StoreItemCollection
                Else
                    Return sci
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreItem_ListItemBuyPoint"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim item As StoreItemRow = LoadItemBuyPointByDataReader(dr)
                    sci.Add(item)
                End While
                CacheUtils.SetCache(key, sci, Utility.ConfigData.TimeCacheData)
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return sci
        End Function
        Public Shared Function GetItemBuyPoint(ByVal DB As Database, ByVal itemId As Integer) As StoreItemRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim c As New StoreItemRow
                Dim key As String = cachePrefixKey & "GetItemBuyPoint_" & itemId
                c = CType(CacheUtils.GetCache(key), StoreItemRow)
                If Not c Is Nothing Then
                    Return c
                Else
                    c = New StoreItemRow
                End If

                Dim sp As String = "sp_StoreItem_GetItemBuyPointById"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("ItemId", SqlDbType.Int, 0, itemId))
                r = cmd.ExecuteReader()
                If r.Read Then
                    c = LoadItemBuyPointByDataReader(r)
                End If
                Core.CloseReader(r)
                CacheUtils.SetCache(key, c)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return Nothing
        End Function

        Private Shared Function LoadItemBuyPointByDataReader(ByVal r As IDataReader) As StoreItemRow
            Dim item As New StoreItemRow
            Try
                item.ItemId = r.Item("ItemId")
                If r.Item("Price") Is Convert.DBNull Then
                    item.Price = Nothing
                Else
                    item.Price = Convert.ToDouble(r.Item("Price"))
                End If
                If r.Item("ItemName") Is Convert.DBNull Then
                    item.ItemName = Nothing
                Else
                    item.ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                If r.Item("SKU") Is Convert.DBNull Then
                    item.SKU = Nothing
                Else
                    item.SKU = Convert.ToString(r.Item("SKU"))
                End If
                Return item
            Catch ex As Exception
                Throw ex
            End Try
        End Function 'Load
        Public Shared Function GetListMSDS(sortSku As String, sortItemName As String) As List(Of StoreItemRow)
            Dim qrSort As String = String.Empty
            If Not String.IsNullOrEmpty(sortSku) Then
                qrSort = " Order by SKU " & sortSku
            End If
            If Not String.IsNullOrEmpty(sortItemName) Then
                qrSort = " Order by ItemName " & sortItemName
            End If
            Dim result As List(Of StoreItemRow)
            Dim key As String = String.Format(cachePrefixKey & "GetListMSDS_{0}_{1}", sortSku, sortItemName)
            result = CType(CacheUtils.GetCache(key), List(Of StoreItemRow))
            If Not result Is Nothing Then
                Return result
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                reader = db.ExecuteReader(CommandType.Text, "select SKU,ItemName,MSDS from StoreItem where MSDS is not null and IsActive = 1 " & qrSort)

                If reader.HasRows Then
                    result = mapList(Of StoreItemRow)(reader)
                End If
                Core.CloseReader(reader)
                CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowURLCodeById(ByVal ItemId As Integer)", ex)
            End Try
            Return result
        End Function

        Public Shared Function GetPropertiesByItemId(id As Integer) As StoreItemRow


            Dim reader As SqlDataReader = Nothing
            Dim si As StoreItemRow = New StoreItemRow
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_StoreItemProperties_GetByItemId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, id)
            reader = db.ExecuteReader(cmd)
            Try

                If reader.HasRows Then
                    si = mapList(Of StoreItemRow)(reader).Item(0)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetPropertiesByItemId(id = " & id & ")", ex)
            End Try
            Return si
        End Function

#Region "Set Field Item"
        Public Shared Function SetFieldItem(ByVal dr As SqlDataReader, ByVal si As StoreItemCollection) As StoreItemCollection
            Try
                'Create xml data
                '''''''''''''''''''''
                While dr.Read
                    Dim i As New StoreItemRow()
                    counter += 1

                    If IsDBNull(dr("ItemId")) Then 'Recentlyview
                        i.ItemName = dr("ItemName")
                        If IsDBNull(dr.Item("URLCode")) Then
                            i.URLCode = Nothing
                        Else
                            i.URLCode = dr("URLCode")
                        End If
                        si.Add(i)
                        Continue While
                    End If

                    i.ItemId = CInt(dr("ItemId"))
                    i.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                    i.IsFreeShipping = CBool(dr("IsFreeShipping"))
                    i.IsHot = CBool(dr("IsHot"))
                    i.IsBestSeller = CBool(dr("IsBestSeller"))
                    i.IsNew = CBool(dr("IsNew"))
                    If Not IsDBNull(dr("NewUntil")) Then i.NewUntil = CDate(dr("NewUntil")) Else i.NewUntil = Nothing
                    i.ShortDesc = IIf(IsDBNull(dr("ShortDesc")), Nothing, dr("ShortDesc"))
                    i.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))


                    If (Common.ColumnExists(dr, "MixMatchId")) Then
                        i.MixMatchId = dr("MixMatchId")
                    End If
                    'Set ItemName
                    i.ItemName = dr("ItemName")
                    i.ItemName2 = dr("ItemName")
                    If IsDBNull(dr.Item("URLCode")) Then
                        i.URLCode = Nothing
                    Else
                        i.URLCode = dr("URLCode")
                    End If
                    If Not IsDBNull(dr("ChoiceName")) Then
                        'i.ItemName &= " - " & dr("ChoiceName")
                        If Not IsDBNull(dr("ChoiceName")) AndAlso Not i.ItemName2.Contains(dr("ChoiceName")) Then
                            i.ItemName2 &= " - " & dr("ChoiceName")
                        End If
                    End If

                    If i.PriceDesc <> Nothing AndAlso Not i.ItemName2.Contains(i.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        i.ItemName2 &= " - " & i.PriceDesc
                        'i.ItemName = i.ItemName2
                    End If

                    i.SKU = IIf(IsDBNull(dr("SKU")), Nothing, dr("SKU"))
                    i.Image = IIf(IsDBNull(dr("Image")), Nothing, dr("Image"))
                    Try
                        i.IsSpecialOrder = CBool(IIf(IsDBNull(dr("IsSpecialOrder")), False, dr("IsSpecialOrder")))
                        i.AcceptingOrder = CInt(dr("AcceptingOrder"))
                        i.Price = dr("Price")
                        i.QtyOnHand = dr("QtyOnHand")
                        i.LowPrice = dr("lowprice")
                        i.LowSalePrice = dr("lowsalePrice")
                        'If HttpContext.Current.Session("MemberId") <> Nothing Then
                        Try
                            i.IsFlammable = IIf(String.IsNullOrEmpty(dr("IsFlammable")), False, CBool(dr("IsFlammable")))
                        Catch
                            i.IsFlammable = False
                        End Try

                        'End If

                        Try
                            i.IsLoginViewPrice = CBool(dr("IsLoginViewPrice"))
                        Catch ex As Exception
                            i.IsLoginViewPrice = False
                        End Try

                        i.youSave = i.LowPrice - i.LowSalePrice
                        i.savePercent = FormatCurrency(i.youSave * 100 / i.LowPrice)
                        i.itemIndex = counter
                        i.IsVariance = CBool(dr("IsVariance"))
                        i.CountReview = dr("CountReview")
                        i.AverageReview = dr("AverageReview")
                        Try
                            i.IsActive = dr("IsActive")
                        Catch ex As Exception
                            i.IsActive = True
                        End Try
                        i.CasePrice = dr("CasePrice")
                        i.CaseQty = dr("CaseQty")
                    Catch ex As Exception

                    End Try
                    Try
                        i.ItemNameNew = dr("ItemNameNew")
                    Catch

                    End Try
                    Try
                        i.IsFreeSample = dr("IsFreeSample")
                    Catch

                    End Try
                    Try
                        i.IsFreeGift = dr("IsFreeGift")
                    Catch

                    End Try
                    Try
                        If (i.NewUntil = Nothing And i.IsNew) Or (i.NewUntil <> Nothing And i.NewUntil > Now) Then
                            i.IsNewTrue = True
                        Else
                            i.IsNewTrue = False
                        End If
                    Catch ex As Exception
                        i.IsNewTrue = False
                    End Try
                    si.Add(i)

                End While

            Catch ex As Exception
                Throw ex
            Finally
                dr.Close()
            End Try
            Return si
        End Function
        Private Shared Function CheckCDATA(ByVal strValue As String) As String

            Dim pattern As String = "[^a-zA-Z0-9]"
            If (Regex.IsMatch(strValue, pattern)) Then
                Return "<![CDATA[" & strValue & "]]>"
            End If
            Return strValue
        End Function
#End Region
#Region "List CategoryItem with paging"
        Public Shared Function ListCategoryItem(ByVal SalesCategoryId As Integer, ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim countdata As Integer = filter.pg * filter.MaxPerPage - filter.MaxPerPage
            Dim Language As String = Common.GetSiteLanguage
            Dim key As String = String.Format(SalesCategoryItemRow.cachePrefixKey & "ListCategoryItem_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}", SalesCategoryId, filter.pg, filter.MaxPerPage, Language, filter.MemberId, filter.OrderId, filter.CollectionId, filter.ToneId, filter.ShadeId, countdata)
            Dim sci As StoreItemCollection
            sci = CType(CacheUtils.GetCache(key), StoreItemCollection)
            If sci Is Nothing Then
                sci = New StoreItemCollection
                If filter.pg = 1 Then
                    counter = 0
                End If
            Else
                Return sci
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_CategoryItem_ListCategoryItem"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "SalesCategoryId", DbType.Int32, SalesCategoryId)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                dr = db.ExecuteReader(cmd)
                sci = SetFieldItem(dr, sci)
                Core.CloseReader(dr)
                sci.TotalRecords = CInt(db.GetParameterValue(cmd, "TotalRecords"))
                CacheUtils.SetCache(key, sci, Utility.ConfigData.TimeCacheData)
                Return sci
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("ListCategoryItem", ex)
            End Try
            Return sci
        End Function
#End Region


#Region "LIST WITH PAGING & CACHE"
        Public Shared Function ListByShopSaveId(ByVal ShopSaveId As Integer, ByVal filter As DepartmentFilterFields) As StoreItemCollection
            'Get Cache
            Dim countdata As Integer = filter.pg * filter.MaxPerPage - filter.MaxPerPage
            Dim languageName As String = Common.GetSiteLanguage()
            Dim key As String = String.Format(ShopSaveItemRow.cachePrefixKey & "ListByShopSaveId_{0}_{1}_{2}_{3}_{4}_{5}", languageName, ShopSaveId, filter.pg, filter.MaxPerPage, filter.OrderId, countdata)
            Dim ssi As New StoreItemCollection
            ssi = CType(CacheUtils.GetCache(key), StoreItemCollection)
            If ssi Is Nothing Then
                ssi = New StoreItemCollection
                If filter.pg = 1 Then
                    counter = 0
                End If
            Else
                Return ssi
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ShopSaveItem_ListByShopSaveId_V2"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ShopSaveId", DbType.Int32, ShopSaveId)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "Language", DbType.String, languageName)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                db.AddOutParameter(cmd, "HasNewItem", DbType.Boolean, 0)
                dr = db.ExecuteReader(cmd)
                ssi = SetFieldItem(dr, ssi)
                Core.CloseReader(dr)
                ssi.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                ssi.EnableHasNewItem = True
                ssi.HasNewItem = CBool(cmd.Parameters("@HasNewItem").Value)
                CacheUtils.SetCache(key, ssi, Utility.ConfigData.TimeCacheData)
                Return ssi
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("ListByShopSaveId", ex)
            End Try
            Return ssi
        End Function
#End Region

        'end 23/10/2009
        Public Function GetFeatureFilters() As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetFeatureFilters"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function


        Private Shared Sub Initialize(ByVal DB As Database, ByRef row As StoreItemRow, ByVal CustomerPriceGroupId As Integer)
            If System.Web.HttpContext.Current Is Nothing Then Exit Sub
            row.m_Promotion = Nothing
            row.MixMatchId = GetMixMatchID(DB, row.ItemId, CustomerPriceGroupId)

            Try
                If row.MixMatchId <> Nothing Then
                    row.m_Promotion = PromotionRow.GetRow(DB, row.MixMatchId, False)
                End If
            Catch ex As Exception
            End Try

            If row.LowSalePrice = Nothing Then
                Dim tmp As String = GetCustomerDiscount(row.ItemId, System.Web.HttpContext.Current.Session("MemberId"))  'Khoa them AddType cho Buy in Bulk
                If IsNumeric(tmp) Then row.LowSalePrice = FormatNumber(tmp, 2)
            End If

            row.LowPrice = row.Price
            row.HighPrice = row.Price
        End Sub

        Private Shared Sub Initialize2(ByVal DB As Database, ByRef row As StoreItemRow, ByVal CustomerPriceGroupId As Integer)
            If System.Web.HttpContext.Current Is Nothing Then Exit Sub
            row.m_Promotion = Nothing
            'row.MixMatchId = GetMixMatchID(DB, row.ItemId, CustomerPriceGroupId)

            Try
                If row.MixMatchId <> Nothing Then
                    row.m_Promotion = PromotionRow.GetRow(DB, row.MixMatchId, False)
                End If
            Catch ex As Exception
            End Try

            If row.LowSalePrice = Nothing Then
                Dim tmp As String = GetCustomerDiscount(row.ItemId, Utility.Common.GetCurrentMemberId())  'Khoa them AddType cho Buy in Bulk
                If IsNumeric(tmp) Then row.LowSalePrice = FormatNumber(tmp, 2)
            End If

            row.LowPrice = row.Price
            row.HighPrice = row.Price
        End Sub

        Private Shared Sub InitializeFromCart(ByVal DB As Database, ByRef row As StoreItemRow, ByVal MemberId As Integer, ByVal Qty As Integer, ByVal AddType As Integer)
            If System.Web.HttpContext.Current Is Nothing Then Exit Sub
            row.m_Promotion = Nothing

            If row.LowSalePrice = Nothing Then
                Dim tmp As String = GetCustomerDiscountWithQuantity(row.ItemId, MemberId, Qty, AddType)  'Khoa them AddType cho Buy in Bulk
                If IsNumeric(tmp) Then row.LowSalePrice = FormatNumber(tmp, 2)
            End If

            row.LowPrice = row.Price
            row.HighPrice = row.Price
        End Sub

        Public Shared Function GetMixMatchIdDescription(ByVal MixMatchId As Integer, ByVal ItemId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal MixMatchType As Utility.Common.MixmatchType, ByRef MixMatchDescription As String) As Integer
            Dim MixmatchIdNew As Integer = 0
            Dim r As SqlDataReader = Nothing

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_Mixmatch_GetIDDescription"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "MixMatchId", DbType.Int32, MixMatchId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
                db.AddInParameter(cmd, "MixMatchType", DbType.Int16, MixMatchType)

                'Dim cmd As DbCommand = db.ExecuteReader("DECLARE @MixMatchId AS INTEGER = [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & ItemId & ", " & CustomerPriceGroupId & ",0)  SELECT @MixMatchId AS MixmatchId, (case when @MixMatchId > 0 then (SELECT COALESCE (Description,'') FROM MixMatch WHERE Id=@MixMatchId and [Type]=" & MixMatchType & ") ELSE null END) AS MixMatchDescription")
                r = db.ExecuteReader(cmd)
                While r.Read()
                    If r.Item("MixmatchId") IsNot Convert.DBNull Then
                        MixmatchIdNew = r.Item("MixmatchId")
                    End If
                    If r.Item("MixMatchDescription") IsNot Convert.DBNull Then
                        MixMatchDescription = r.Item("MixMatchDescription")
                    End If
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetMixMatchIdDescription", ex)
            End Try

            Return MixmatchIdNew
        End Function

        Public Shared Function GetCustomerDiscount(ByVal ItemId As Integer, ByVal MemberId As Integer) As Double
            Dim discount As Double = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETOBJECT As String = "sp_StoreItem_GetCustomerDiscount"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETOBJECT)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            discount = Convert.ToDouble(db.ExecuteScalar(cmd))
            Return discount
        End Function

        Public Shared Function GetCustomerDiscountWithQuantity(ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal Quantity As Integer, ByVal AddType As Integer) As Double
            Dim discount As Double = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREITEM_GETOBJECT As String = "sp_StoreItem_GetCustomerDiscountWithQuantity"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETOBJECT)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "Quantity", DbType.Int32, Quantity)
            db.AddInParameter(cmd, "AddType", DbType.Int32, AddType)
            discount = Convert.ToDouble(db.ExecuteScalar(cmd))
            Return discount
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal ItemId As Integer)
            Dim row As StoreItemRow

            row = New StoreItemRow(_Database, ItemId)
            row.Remove()
        End Sub



        Public Shared Function GetAlternateImagesCount(ByVal DB1 As Database, ByVal ItemId As Integer, Optional ByVal TopRecords As Integer = Nothing) As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetAlternateImagesCount"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            Return Convert.ToInt32(db.ExecuteScalar(cmd))
            '------------------------------------------------------------------------
        End Function

        Public Function GetAllTips() As DataSet
            Dim UseEnglish As Boolean = System.Web.HttpContext.Current.Session("Language") = Nothing
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetAllTips"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "UseEnglish", DbType.Boolean, UseEnglish)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function
        Public Function GetCountItemReviewByMember(ByVal db As Database, ByVal itemId As Integer, ByVal memberId As Integer) As Integer
            Dim SQL As String = "Select COUNT(*) from StoreItemReview where ItemId=" & itemId & " and MemberId=" & memberId
            Dim rating As Integer = db.ExecuteScalar(SQL)
            Return rating
        End Function
        Public Function GetCountReviewed() As Integer
            Dim SQL As String = "Select COUNT(*) from StoreItemReview where isActive = 1 and ItemId=" & ItemId
            Dim Count As Integer = DB.ExecuteScalar(SQL)
            Return Count
        End Function
        Public Function GetReviewRating() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SQL As String = "select coalesce(avg(cast(numstars as float)),0) from storeitemreview where isactive = 1 and " & IIf(ItemGroupId <> Nothing, "itemid in (select itemid from storeitem where itemgroupid = " & ItemGroupId & ")", "itemid = " & ItemId)
            Dim rating As Double = db.ExecuteScalar(CommandType.Text, SQL)
            db = Nothing
            Return CInt((FormatNumber((rating * 10) / 5, 0) * 5).ToString.PadRight(2, "0"))
        End Function

        Public Shared Function GetAllStoreItems(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select sku + ' - ' + itemname as itemname, itemid from StoreItem order by ItemName")
            Return ds
        End Function
        Public Shared Function GetAllStoreItemsActive(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select sku + ' - ' + itemname as itemname, itemid from StoreItem where IsActive=1 order by ItemName")
            Return ds
        End Function
        Public Shared Function GetSelectedChoices(ByVal DB As Database, ByVal ItemId As Integer) As DataTable
            Return DB.GetDataTable("select o.optionid, o.optionname, c.choiceid, c.choicename from storeitemgroupoption o inner join storeitemgroupchoice c on o.optionid = c.optionid inner join storeitemgroupchoicerel r on c.choiceid = r.choiceid where r.itemid = " & ItemId)
        End Function
        Public Shared Function IsItemMultiPrice(ByVal itemId As Integer, ByVal memberId As Integer) As Boolean
            Dim dv As DataView = StoreOrderRow.FillPricing(itemId, memberId)
            If Not dv Is Nothing Then
                If (dv.Count > 0) Then
                    For i As Integer = 0 To dv.Count - 1
                        If (CInt(dv(i)("minimumquantity")) > 1) Then
                            Return True
                        End If
                    Next
                End If
            End If
            Return False
        End Function
        Public Function GetSelectedChoices() As DataSet
            Return DB.GetDataSet("select o.optionid, o.optionname, c.choiceid, c.choicename from storeitemgroupoption o inner join storeitemgroupchoice c on o.optionid = c.optionid inner join storeitemgroupchoicerel r on c.choiceid = r.choiceid where r.itemid = " & ItemId)
        End Function

        Public Shared Function RemoveOptionChoices(ByVal groupId As Integer, ByVal itemId As Integer) As Boolean
            If groupId < 1 Or itemId < 1 Then
                Return 0
            End If
            Try

                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemGroupChoiceRel_Delete"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "ItemGroupId", DbType.Int32, groupId)
                dbAcess.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("StoreItem.vb", "RemoveOptionChoices(ByVal groupId:=" & groupId & " As Integer, ByVal itemId:=" & itemId & " As Integer)", ex)
            End Try
            Return False
        End Function

        Public Shared Sub InsertOptionChoices(ByVal DB As Database, ByVal itemId As Integer, ByVal sChoices As String)
            Dim val As String() = sChoices.Split(",")
            For i As Integer = 0 To UBound(val)
                If IsNumeric(val(i)) Then
                    InsertOptionChoice(DB, itemId, CInt(val(i)))
                End If
            Next
        End Sub

        Private Shared Sub InsertOptionChoice(ByVal DB As Database, ByVal itemId As Integer, ByVal id As Integer)
            Dim SQL As String = "insert into storeitemgroupchoicerel (itemid, optionid, choiceid) select " & DB.NullNumber(itemId) & ", optionid, " & DB.NullNumber(id) & " from storeitemgroupchoice where choiceid = " & id
            DB.ExecuteSQL(SQL)
        End Sub

        Public Function IsPediItem() As Boolean
            Dim id As Integer = Nothing

            id = DB.ExecuteScalar("select top 1 coalesce(itemid,0) from StoreBaseColorItem where itemid = " & ItemId)
            If id = Nothing Then id = DB.ExecuteScalar("select top 1 coalesce(itemid,0) from StoreCusionColorItem where itemid = " & ItemId)
            If id = Nothing Then id = DB.ExecuteScalar("select top 1 coalesce(itemid,0) from StoreLaminateTrimItem where itemid = " & ItemId)

            Return id > 0
        End Function

        Public Shared Function GetItemIdByChoices(ByVal DB As Database, ByVal ItemGroupId As Integer, ByVal Choices As String) As Integer
            Dim SQL, JoinSQL, WhereSQL, Vals() As String
            Vals = Split(Choices, ",")

            JoinSQL = ""
            WhereSQL = ""
            For i As Integer = 0 To UBound(Vals)
                If IsNumeric(Vals(i)) AndAlso CInt(Vals(i)) <> Nothing Then
                    JoinSQL &= "	inner join storeitemgroupchoicerel c" & i & " on r.itemid = c" & i & ".itemid " & vbCrLf
                    WhereSQL &= "	and c" & i & ".choiceid = " & Vals(i) & " " & vbCrLf
                End If
            Next

            SQL = "select " & vbCrLf &
             "	top 1 r.itemid " & vbCrLf &
             "from " & vbCrLf &
             "	storeitemgroupchoicerel r " & vbCrLf &
             JoinSQL & vbCrLf &
             "where " & vbCrLf &
             "	r.itemid in (select itemid from storeitem where itemgroupid = " & ItemGroupId & " and isactive=1) " & vbCrLf &
             WhereSQL

            Dim id As Integer = DB.ExecuteScalar(SQL)
            Return id
            'If id < 1 Then
            '    SQL = "select " & vbCrLf & _
            ' "	top 1 r.itemid " & vbCrLf & _
            ' "from " & vbCrLf & _
            ' "	storeitemgroupchoicerel r " & vbCrLf & _
            ' JoinSQL & vbCrLf & _
            ' "where " & vbCrLf & _
            ' "	r.itemid in (select itemid from storeitem where itemgroupid = " & ItemGroupId & " and isactive=1) and  choiceid=" & 
            'End If
        End Function

        Public Sub RemoveDepartmentItems()
            Dim SQL As String = "delete from StoreDepartmentItem where ItemId = " & DB.Quote(ItemId.ToString)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Function GetItemReviewsCount(ByVal DB1 As Database, ByVal ItemId As Integer, ByVal itemgroupid As Integer) As Integer
            '---------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 27, 2009
            '---------------------------------------------------------------------
            Dim itemReviewsCount = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETITEM As String = "sp_StoreItem_GetItemReviewsCount"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETITEM)

            db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, itemgroupid)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            itemReviewsCount = Convert.ToInt32(db.ExecuteScalar(cmd))

            Return itemReviewsCount
        End Function
        Public Shared Function CountRelatedItem(ByVal ItemId As Integer) As Integer

            Dim result = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETITEM As String = "sp_StoreItem_CountRelatedItem"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETITEM)

            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            result = Convert.ToInt32(db.ExecuteScalar(cmd))

            Return result
        End Function

        Public Shared Function GetItemReviews(ByVal DB1 As Database, ByVal ItemId As Integer, ByVal itemgroupid As Integer, ByVal TopRecords As Integer) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetItemReviews"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "TopRecords", DbType.Int32, TopRecords)
            db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, itemgroupid)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Sub InsertDepartmentItems(ByVal DepartmentListSeparatedByComma As String)
            If DepartmentListSeparatedByComma = String.Empty Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO StoreDepartmentItem (ItemId, DepartmentId) Select " & DB.Quote(ItemId) & ", DepartmentId FROM StoreDepartment WHERE DepartmentId IN " & DB.NumberMultiple(DepartmentListSeparatedByComma)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertItemFeatures(ByVal FeatureListSeparatedByComma As String)
            If FeatureListSeparatedByComma = String.Empty Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO StoreItemFeature (ItemId, FeatureId) Select " & DB.Quote(ItemId) & ", FeatureId FROM ItemFeature WHERE FeatureId IN " & DB.NumberMultiple(FeatureListSeparatedByComma)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertItemOccasions(ByVal OccasionListSeparatedByComma As String)
            If OccasionListSeparatedByComma = String.Empty Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO StoreItemOccasion (ItemId, OccasionId) Select " & DB.Quote(ItemId) & ", OccasionId FROM Occasion WHERE OccasionId IN " & DB.NumberMultiple(OccasionListSeparatedByComma)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertItemSwatches(ByVal SwatchListSeparatedByComma As String)
            If SwatchListSeparatedByComma = String.Empty Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO StoreItemSwatch (ItemId, SwatchId) Select " & DB.Quote(ItemId) & ", SwatchId FROM Swatch WHERE SwatchId IN " & DB.NumberMultiple(SwatchListSeparatedByComma)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertToCollection(ByVal ChildItemId As Integer)
            If ChildItemId = Nothing Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO CollectionItem (ParentId, ItemId, SortOrder) Select " & DB.Quote(ItemId) & ", ItemId, coalesce((select Max(SortOrder) from CollectionItem where ParentId = " & DB.Quote(ItemId) & "),0) + 1 FROM StoreItem WHERE ItemId = " & DB.Quote(ChildItemId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub RemoveCollectionItem(ByVal _Database As Database, ByVal ParentId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "delete from CollectionItem where ItemId = " & _Database.Quote(ItemId.ToString) & " and ParentId = " & _Database.Quote(ParentId)
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertRelatedItem(ByVal ChildItemId As Integer)
            If ChildItemId = Nothing Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO RelatedItem (ParentId, ItemId, SortOrder) Select " & DB.Quote(ItemId) & ", ItemId, coalesce((select Max(SortOrder) from RelatedItem where ParentId = " & DB.Quote(ItemId) & "),0) + 1 FROM StoreItem WHERE ItemId = " & DB.Quote(ChildItemId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertRelatedSwatch(ByVal ChildItemId As Integer)
            If ChildItemId = Nothing Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO RelatedSwatch (ParentId, ItemId, SortOrder) Select " & DB.Quote(ItemId) & ", ItemId, coalesce((select Max(SortOrder) from RelatedSwatch where ParentId = " & DB.Quote(ItemId) & "),0) + 1 FROM StoreItem WHERE ItemId = " & DB.Quote(ChildItemId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub RemoveRelatedItem(ByVal _Database As Database, ByVal ParentId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "delete from RelatedItem where ItemId = " & _Database.Quote(ItemId.ToString) & " and ParentId = " & _Database.Quote(ParentId)
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub RemoveRelatedSwatch(ByVal _Database As Database, ByVal ParentId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "delete from RelatedSwatch where ItemId = " & _Database.Quote(ItemId.ToString) & " and ParentId = " & _Database.Quote(ParentId)
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Shared Function IsDiscontinued(ByVal Status As String) As Boolean
            If Status = "C1" Or Status = "C3" Or Status = "KC" Or Status = "J1" Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function DiscontinuedStatusList() As String
            Return "('C1', 'C3', 'KC', 'J1')"
        End Function

        Public Shared Function GetShortInventoryMessage(ByVal DB As Database, ByVal Status As String, ByVal QtyOnHand As Integer, ByVal ShipmentDate As Date, ByVal DropShip As Boolean) As String
            Dim Result As String

            If DropShip Then
                Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 1"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                Return Result
            End If

            'Logic taken from old chiasso.com site
            Dim Discontinued As Boolean = IsDiscontinued(Status)
            If Discontinued Then
                'If item is discontinued
                If QtyOnHand > 0 Then
                    Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 2"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                Else
                    If ShipmentDate <= Date.Today Then
                        Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 3"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                    Else
                        Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 4"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                    End If
                End If
            Else
                If QtyOnHand <= 0 Then

                    If ShipmentDate <= Date.Today Then
                        Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 5"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                    Else
                        Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 6"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                    End If

                Else
                    Result = Replace(Replace(DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId = 7"), "%%qtyonhand%%", QtyOnHand), "%%shipmentdate%%", ShipmentDate)
                End If
            End If
            Return Result
        End Function

        Public Function GetDefaultDepartmentId() As Integer
            Dim departmentId As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_StoreItems_GetDefaultDepartmentID"
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)

                db.AddInParameter(cmd, "ItemID", DbType.Int32, ItemId)

                departmentId = Convert.ToInt32(db.ExecuteScalar(cmd))
            Catch ex As Exception
                Email.SendError("ToError500", "StoreItem.vb > GetDefaultDepartmentId(" & ItemId & ")", ex.ToString())
            End Try
            Return departmentId
        End Function

        Public Shared Function GetDefaultDepartmentId(ByVal itemId As Integer) As Integer
            Dim departmentId As Integer = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_DELETE As String = "sp_StoreItems_GetDefaultDepartmentID"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_DELETE)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, itemId)

            departmentId = Convert.ToInt32(db.ExecuteScalar(cmd))
            '------------------------------------------------------------------------
            Return departmentId
        End Function
        Public Shared Function GetDefaultDepartmentCodeByItemCode(ByVal urlCode As String) As String
            Dim departmentCode As String = ""

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItems_GetDefaultDepartmentCode")

            db.AddInParameter(cmd, "URLCode", DbType.String, urlCode)

            departmentCode = db.ExecuteScalar(cmd)
            '------------------------------------------------------------------------
            Return departmentCode
        End Function

        Public Shared Function GetDefaultDepartmentNameByItemId(ByVal itemId As Integer) As String
            Dim departmentName As String = String.Empty

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREDEPARTMENT_DELETE As String = "sp_StoreItems_GetDefaultDepartmentName"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREDEPARTMENT_DELETE)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, itemId)

            departmentName = db.ExecuteScalar(cmd)
            '------------------------------------------------------------------------
            Return departmentName
        End Function
        Public Function GetFeatures() As ItemFeatureCollection
            Dim SQL As String = "select f.* from itemfeature f, storeitemfeature sif where sif.featureid = f.featureid and ItemId = " & DB.Quote(ItemId)
            Dim row As ItemFeatureRow
            Dim collection As New ItemFeatureCollection
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    While r.Read()
                        row = New ItemFeatureRow(DB)
                        row.Load(r)
                        collection.Add(row)
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetFeatures", ex)
            End Try
            Return collection
        End Function

        Public Sub InsertFeature(ByVal Code As String)
            Dim SQL As String = "insert Into StoreItemFeature (FeatureId, ItemId) select FeatureId, " & ItemId & " from ItemFeature where Code = " & DB.Quote(Code)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Function GetBrandName() As String
            Dim result As String = String.Empty
            If BrandId > 0 Then
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                result = db.ExecuteScalar(CommandType.Text, "SELECT BrandName FROM StoreBrand WHERE BrandId = " & BrandId)
            End If

            Return result
        End Function

        Public Shared Function GetPromotionItems(ByVal DB As Database, ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim SQL As String = String.Empty

            SQL = " select si.itemname, si.ItemId, si.Price, coalesce(tmp.lowprice,si.price) as lowprice, coalesce(tmp.highprice,si.price) as highprice from storeitem si left outer join (select r2.itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem si2 inner join storeitemgrouprel r2 on si2.itemid = r2.itemid group by r2.itemgroupid) tmp on tmp.itemgroupid = si.itemid INNER JOIN MixMatchLine l ON si.ItemId = l.ItemId INNER JOIN MixMatch m ON l.MixMatchId = m.Id where si.IsActive = 1 and m.IsActive = 1 AND EXISTS (SELECT TOP 1 Id FROM MixMatchLine WHERE MixMatchId = m.Id AND [Value] " & IIf(filter.GetItems, " > 0 ", " = 0 ") & ") AND [Value] " & IIf(filter.GetItems, " > 0 ", " = 0 ") & " AND MixMatchId = " & filter.PromotionId
            Dim Promotion As PromotionRow = PromotionRow.GetRow(DB, filter.PromotionId, False)
            Dim p As PromotionCollection = New PromotionCollection
            p.Add(Promotion)
            Dim dv As DataView, drv As DataRowView
            If filter.GetItems Then
                dv = Promotion.GetItems
            Else
                dv = Promotion.PurchaseItems
            End If

            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                Dim item As New StoreItemRow(DB)
                item.ItemName = drv("itemName")
                item.ItemId = drv("itemid")
                item.Price = drv("price")
                item.LowPrice = drv("LowPrice")
                item.HighPrice = drv("HighPrice")
                item.m_Promotions = p
                c.Add(item)
            Next

            Return c
        End Function
        Public Shared Function GetPromotionItems(ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim SQL As String = String.Empty

            SQL = " select si.itemname, si.ItemId, si.Price, coalesce(tmp.lowprice,si.price) as lowprice, coalesce(tmp.highprice,si.price) as highprice from storeitem si left outer join (select r2.itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem si2 inner join storeitemgrouprel r2 on si2.itemid = r2.itemid group by r2.itemgroupid) tmp on tmp.itemgroupid = si.itemid INNER JOIN MixMatchLine l ON si.ItemId = l.ItemId INNER JOIN MixMatch m ON l.MixMatchId = m.Id where si.IsActive = 1 and m.IsActive = 1 AND EXISTS (SELECT TOP 1 Id FROM MixMatchLine WHERE MixMatchId = m.Id AND [Value] " & IIf(filter.GetItems, " > 0 ", " = 0 ") & ") AND [Value] " & IIf(filter.GetItems, " > 0 ", " = 0 ") & " AND MixMatchId = " & filter.PromotionId
            Dim Promotion As PromotionRow = PromotionRow.GetRowPro(filter.PromotionId, False)
            Dim p As PromotionCollection = New PromotionCollection
            p.Add(Promotion)
            Dim dv As DataView, drv As DataRowView
            If filter.GetItems Then
                dv = Promotion.GetItems
            Else
                dv = Promotion.PurchaseItems
            End If

            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                Dim item As New StoreItemRow()
                item.ItemName = drv("itemName")
                item.ItemId = drv("itemid")
                item.Price = drv("price")
                item.LowPrice = drv("LowPrice")
                item.HighPrice = drv("HighPrice")
                item.m_Promotions = p
                c.Add(item)
            Next

            Return c
        End Function
        Public Shared Function GetFreeSampleText(ByVal pageIndex As Integer) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim r As SqlDataReader = Nothing
            Dim query As String = "[GetCustomersPageWise]"
            Dim cmd As SqlCommand = New SqlCommand(query)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
            cmd.Parameters.AddWithValue("@PageSize", 10)
            cmd.Parameters.Add("@PageCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
        End Function
        Public Shared Function GetFreeSampleColection(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer) As StoreItemCollection

            Dim c As New StoreItemCollection

            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_FreeSample_List"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                If filter.OrderId > 0 Then
                    db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                End If
                db.AddInParameter(cmd, "OrderBy", DbType.String, "FreeSampleArrange")
                db.AddInParameter(cmd, "OrderDirection", DbType.String, "ASC")
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    Dim item As New StoreItemRow(DB1)
                    item.ItemId = r.Item("ItemId")
                    If r.Item("BrandId") Is Convert.DBNull Then
                        item.BrandId = 0
                    Else
                        item.BrandId = Convert.ToInt32(r.Item("BrandId"))
                    End If
                    item.ItemName = Convert.ToString(r.Item("ItemName"))
                    item.IsNew = Convert.ToBoolean(r.Item("IsNew"))
                    item.IsInCart = Convert.ToBoolean(r.Item("IsInCart"))
                    If r.Item("IsFlammable") Is Convert.DBNull Then
                        item.IsFlammable = False
                    Else
                        item.IsFlammable = Convert.ToBoolean(r.Item("IsFlammable"))
                    End If
                    If r.Item("NewUntil") Is Convert.DBNull Then
                        item.NewUntil = Nothing
                    Else
                        item.NewUntil = Convert.ToDateTime(r.Item("NewUntil"))
                    End If
                    If r.Item("Image") Is Convert.DBNull Then
                        item.Image = Nothing
                    Else
                        item.Image = Convert.ToString(r.Item("Image"))
                    End If

                    If r.Item("ShortDesc") Is Convert.DBNull Then
                        item.ShortDesc = Nothing
                    Else
                        item.ShortDesc = Convert.ToString(r.Item("ShortDesc"))
                    End If
                    If r.Item("URLCode") Is Convert.DBNull Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = Convert.ToString(r.Item("URLCode"))
                    End If

                    item.IsHot = Convert.ToBoolean(r.Item("IsHot"))
                    item.IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                    'Set ItemName2
                    item.ItemName2 = item.ItemName
                    If Not IsDBNull(r("ChoiceName")) AndAlso Not item.ItemName2.Contains(r("ChoiceName")) Then
                        item.ItemName2 &= " - " & r("ChoiceName")
                    End If
                    If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso r("ChoiceName").ToString().Trim().Replace(" ", "") <> r("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        item.ItemName2 &= " - " & r("PriceDesc")
                    End If
                    If TotalRecords <= 0 Then
                        TotalRecords = Convert.ToInt32(r("TotalRecords"))
                    End If
                    c.Add(item)
                End While
                Core.CloseReader(r)

            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetFreeSampleColection", ex)
            End Try

            Return c
        End Function
        Public Shared Function GetFreeGiftColectionByLevel(ByVal orderId As Integer, ByVal levelId As Integer) As StoreItemCollection

            Dim c As New StoreItemCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_FreeGift_ListByLevel"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                If orderId > 0 Then
                    db.AddInParameter(cmd, "OrderId", DbType.Int32, orderId)
                End If
                db.AddInParameter(cmd, "Levelid", DbType.Double, levelId)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    Dim item As New StoreItemRow()
                    item.ItemId = r.Item("ItemId")
                    If r.Item("BrandId") Is Convert.DBNull Then
                        item.BrandId = 0
                    Else
                        item.BrandId = Convert.ToInt32(r.Item("BrandId"))
                    End If
                    item.ItemName = Convert.ToString(r.Item("ItemName"))
                    item.IsInCart = Convert.ToBoolean(r.Item("IsInCart"))
                    If r.Item("IsFlammable") Is Convert.DBNull Then
                        item.IsFlammable = False
                    Else
                        item.IsFlammable = Convert.ToBoolean(r.Item("IsFlammable"))
                    End If

                    If r.Item("Image") Is Convert.DBNull Then
                        item.Image = Nothing
                    Else
                        item.Image = Convert.ToString(r.Item("Image"))
                    End If

                    If r.Item("ShortDesc") Is Convert.DBNull Then
                        item.ShortDesc = Nothing
                    Else
                        item.ShortDesc = Convert.ToString(r.Item("ShortDesc"))
                    End If
                    If r.Item("URLCode") Is Convert.DBNull Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = Convert.ToString(r.Item("URLCode"))
                    End If
                    If r.Item("IsHazMat") Is Convert.DBNull Then
                        item.IsHazMat = False
                    Else
                        item.IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                    End If

                    item.ItemName2 = item.ItemName
                    If Not IsDBNull(r("ChoiceName")) AndAlso Not item.ItemName2.Contains(r("ChoiceName")) Then
                        item.ItemName2 &= " - " & r("ChoiceName")
                    End If
                    If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso r("ChoiceName").ToString().Trim().Replace(" ", "") <> r("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        item.ItemName2 &= " - " & r("PriceDesc")
                    End If

                    c.Add(item)
                End While
                Core.CloseReader(r)

            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "StoreItem.vb > GetFreeGiftColectionByLevel", "OrderId: " & orderId.ToString() & "<br>levelId: " & levelId.ToString() & "<br>Exception: " & ex.ToString())
            End Try

            Return c
        End Function
        Private Shared Function XMLizeString(ByVal sInput As String) As String
            If Not (IsAlphaNumeric(sInput)) Then
                Return " " & sInput & "]]>"
            Else
                Return sInput
            End If
        End Function
        Private Shared Function IsAlphaNumeric(ByVal TestString As String) As Boolean

            Dim sTemp As String
            Dim iLen As Integer
            Dim iCtr As Integer
            Dim sChar As String

            'returns true if all characters in a string are alphabetical
            '   or numeric
            'returns false otherwise or for empty string

            sTemp = TestString
            iLen = Len(sTemp)
            If iLen > 0 Then
                For iCtr = 1 To iLen
                    sChar = Mid(sTemp, iCtr, 1)
                    If Not sChar Like "[0-9A-Za-z.:, ]" Then _
                         Exit Function
                Next

                IsAlphaNumeric = True
            End If
        End Function
        Public Shared Function GetListItemRewardPoint(ByVal DB As Database, ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer) As StoreItemCollection

            Dim c As New StoreItemCollection
            Dim Count As Integer = 0
            Dim dr As SqlDataReader = Nothing
            Try
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItem_GetListRewardPoint"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                dbAcess.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                dbAcess.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                dbAcess.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                dbAcess.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, customerPriceGroupId)
                dr = dbAcess.ExecuteReader(cmd)
                While dr.Read
                    Count = Count + 1
                    Dim item As New StoreItemRow(DB)
                    item.LowPrice = dr("LowPrice")
                    item.IsRewardPoints = True
                    item.RewardPoints = dr("RewardPoints")
                    item.HighPrice = dr("HighPrice")
                    item.LowSalePrice = IIf(IsDBNull(dr("LowSalePrice")), Nothing, dr("LowSalePrice"))
                    item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                    item.ItemId = dr("ItemId")
                    item.IsNew = CBool(dr("isnew"))
                    If Not IsDBNull(dr("newuntil")) Then item.NewUntil = dr("newuntil") Else item.NewUntil = Nothing
                    item.IsBestSeller = CBool(dr("IsBestSeller"))
                    item.IsHot = CBool(dr("IsHot"))
                    item.IsFreeShipping = CBool(dr("IsFreeShipping"))
                    item.IsFreeSample = CBool(dr("IsFreeSample"))
                    item.IsFreeGift = CInt(dr("IsFreeGift"))
                    item.IsSpecialOrder = CBool(IIf(IsDBNull(dr("IsSpecialOrder")), False, dr("IsSpecialOrder")))
                    item.PermissionBuyBrand = CBool(IIf(IsDBNull(dr("PermissionBuyBrand")), False, dr("PermissionBuyBrand")))
                    item.AcceptingOrder = dr("AcceptingOrder")
                    item.ItemName = dr("ItemName")
                    item.ItemName2 = dr("ItemName")
                    item.IsFlammable = dr("IsFlammable")
                    If IsDBNull(dr.Item("URLCode")) Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = dr("URLCode")
                    End If
                    item.QtyOnHand = CInt(dr("QtyOnHand"))
                    item.IsVariance = CBool(dr("IsVariance"))
                    If Not IsDBNull(dr("choicename")) Then
                        item.ItemName &= " - " & dr("choicename")

                        If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                            item.ItemName2 &= " - " & dr("choicename")
                        End If

                    End If

                    If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        item.ItemName2 &= " - " & dr("PriceDesc")
                    End If

                    item.Price = dr("Price")
                    item.SKU = IIf(IsDBNull(dr("sku")), Nothing, dr("sku"))
                    If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
                    item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    item.ShortDesc = IIf(IsDBNull(dr("Shortdesc")), Nothing, dr("Shortdesc"))
                    'item.MixMatchId = IIf(IsDBNull(dr("mixmatchid")), Nothing, dr("mixmatchid"))
                    item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
                    item.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))
                    If TotalRecords <= 0 Then
                        TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                    End If
                    item.CountReview = dr("CountReview")
                    item.AverageReview = dr("AverageReview")
                    'If filter.MemberId <> 0 Then
                    ''item.ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(DB, item, False, 1, 0, filter.MemberId) & "</div>"
                    item.IsFlammable = IIf(IsDBNull(dr("isFlammable")), False, dr("isFlammable")) '
                    'End If

                    item.ShowInventory = BaseShoppingCart.Inventory(dr("Status"), IIf(IsDBNull(dr("BODate")), Nothing, dr("BODate")), dr("AcceptingOrder"), dr("QtyOnHand"), dr("IsSpecialOrder"), IIf(IsDBNull(dr("LowStockThreshold")), Nothing, dr("LowStockThreshold")), IIf(IsDBNull(dr("LowStockMsg")), Nothing, dr("LowStockMsg")))
                    c.Add(item)
                End While

            Catch ex As Exception
                SendMailLog("GetListItemRewardPoint", ex)
            End Try
            Core.CloseReader(dr)
            Return c
        End Function

        Public Shared Function ListByDepartmentTabId(ByVal DepartmentTabId As Integer, ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim Language As String = Common.GetSiteLanguage()
            Dim ssi As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentTabItem_ListByDepartmentTabItemId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentTabId", DbType.Int32, DepartmentTabId)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                db.AddOutParameter(cmd, "HasNewItem", DbType.Boolean, 0)
                dr = db.ExecuteReader(cmd)
                ssi = SetFieldItem(dr, ssi)
                Core.CloseReader(dr)
                ssi.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                ssi.EnableHasNewItem = True
                ssi.HasNewItem = CBool(cmd.Parameters("@HasNewItem").Value)
                Return ssi
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("ListByDepartmentTabId", ex)
            End Try
            Return ssi
        End Function

        Public Shared Function ListByItemSku(ByVal ItemSku As String, ByVal filter As DepartmentFilterFields) As StoreItemCollection
            Dim Language As String = Common.GetSiteLanguage()
            Dim ssi As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Catalog_ListBySku"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemSku", DbType.String, ItemSku)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                db.AddOutParameter(cmd, "HasNewItem", DbType.Boolean, 0)

                dr = db.ExecuteReader(cmd)
                ssi = SetFieldItem(dr, ssi)
                Core.CloseReader(dr)
                ssi.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                ssi.EnableHasNewItem = True
                ssi.HasNewItem = 0
                Return ssi
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("ListBySku", ex)
            End Try
            Return ssi
        End Function


        Public Shared Sub InfoToAddCart(ByVal OrderId As Integer, ByVal ItemId As Integer, ByRef TotalInCart As Integer, ByRef QtyOnhand As Integer, ByRef SKU As String, ByRef IsSpecialOrder As Boolean, ByRef AcceptingOrder As Integer, ByRef MaximumQuantity As Integer, ByRef RewardPoints As Integer)

            Dim reader As SqlDataReader = Nothing
            Try
                Dim SP As String = "sp_StoreCartItem_GetCartItemInfor"
                Dim DB As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = DB.GetStoredProcCommand(SP)
                DB.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                DB.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                reader = CType(DB.ExecuteReader(cmd), SqlDataReader)
                If (reader.Read) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("QuantityCart"))) Then
                        TotalInCart = Convert.ToInt32(reader("QuantityCart"))
                    Else
                        TotalInCart = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MaximumQuantity"))) Then
                        MaximumQuantity = Convert.ToInt32(reader("MaximumQuantity"))
                    Else
                        MaximumQuantity = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("QtyOnHand"))) Then
                        QtyOnhand = Convert.ToInt32(reader("QtyOnHand"))
                    Else
                        QtyOnhand = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("sku"))) Then
                        SKU = reader("sku").ToString()
                    Else
                        SKU = String.Empty
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsSpecialOrder"))) Then
                        IsSpecialOrder = Convert.ToBoolean(reader("IsSpecialOrder").ToString())
                    Else
                        IsSpecialOrder = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("AcceptingOrder"))) Then
                        AcceptingOrder = Convert.ToInt32(reader("AcceptingOrder"))
                    Else
                        AcceptingOrder = Utility.Common.ItemAcceptingStatus.None
                    End If


                    If (Not reader.IsDBNull(reader.GetOrdinal("RewardPoints"))) Then
                        RewardPoints = Convert.ToInt32(reader("RewardPoints"))
                    Else
                        RewardPoints = 0
                    End If
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("InfoToAddCart", ex)

            End Try

        End Sub


        Public Shared Function GetItemSearchKeyword(ByVal database As Database, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection
            'Get Cache
            Dim Language As String = Common.GetSiteLanguage()
            Dim dr As SqlDataReader = Nothing
            Dim ssi As New StoreItemCollection
            Try
                Dim hasSaleExp As String = String.Empty
                If (filter.Sale24Hour) Then
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](1,0,'" & Now.ToShortDateString & "',0,si.itemid,si.price)"
                Else
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](0,0,'" & Now.ToShortDateString & "',0,si.itemid,si.price)"
                End If

                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                Dim sortExp As String = String.Empty
                Select Case filter.SortBy
                    Case "price"
                        sortExp = lowSalePriceExp & " asc, " & LowPriceExp & " asc, price asc "
                    Case "product"
                        sortExp = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                    Case "best-sellers"
                        sortExp = " IsBestSeller desc, " & LowPriceExp & " asc, itemname asc "
                    Case "new-items"
                        sortExp = " IsNew desc, " & LowPriceExp & " asc, itemname asc "
                    Case "hot-items"
                        sortExp = " ishot desc, " & LowPriceExp & " asc, itemname asc "
                    Case "featured"
                        sortExp = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                    Case "on-sale"
                        sortExp = hasSaleExp & " desc, " & lowSalePriceExp & " asc, itemname asc "
                    Case "top-rated"
                        sortExp = "[dbo].[fc_StoreItem_GetTopRatedSort](si.itemid) desc, itemname asc "
                    Case "most-popular-review"
                        sortExp = "[dbo].[fc_StoreItem_GetMostPopularReviewSort](si.itemid) desc, itemname asc "
                    Case Else
                        sortExp = " ItemName ASC"
                End Select

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreItem_GetListItemSearchKeyword"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "KeywordName", DbType.String, filter.Keyword)
                db.AddInParameter(cmd, "Language", DbType.String, Language)
                db.AddInParameter(cmd, "BrandCode", DbType.String, filter.BrandCode)
                db.AddInParameter(cmd, "PageIndex", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "SortOrder", DbType.String, sortExp)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim i As New StoreItemRow()
                    Dim item As New StoreItemRow(database)
                    item.LowPrice = dr("LowPrice")
                    item.HighPrice = dr("HighPrice")
                    item.LowSalePrice = IIf(IsDBNull(dr("LowSalePrice")), Nothing, dr("LowSalePrice"))
                    'item.HighSalePrice = IIf(IsDBNull(dr("HighSalePrice")), Nothing, dr("HighSalePrice"))
                    item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                    item.ItemId = dr("ItemId")
                    item.IsNew = CBool(dr("isnew"))
                    If Not IsDBNull(dr("newuntil")) Then item.NewUntil = dr("newuntil") Else item.NewUntil = Nothing
                    item.IsBestSeller = CBool(dr("IsBestSeller"))
                    item.IsHot = CBool(dr("IsHot"))
                    item.IsFreeShipping = CBool(dr("IsFreeShipping"))
                    item.IsFreeSample = CBool(dr("IsFreeSample"))
                    item.IsFreeGift = CInt(dr("IsFreeGift"))
                    item.IsSpecialOrder = CBool(IIf(IsDBNull(dr("IsSpecialOrder")), False, dr("IsSpecialOrder")))
                    item.AcceptingOrder = dr("AcceptingOrder")
                    item.ItemName = dr("ItemName")
                    item.ItemName2 = dr("ItemName")
                    item.IsFlammable = dr("IsFlammable")
                    If IsDBNull(dr.Item("URLCode")) Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = dr("URLCode")
                    End If
                    item.QtyOnHand = CInt(dr("QtyOnHand"))
                    item.IsVariance = CBool(dr("IsVariance"))
                    If Not IsDBNull(dr("choicename")) Then
                        item.ItemName &= " - " & dr("choicename")

                        If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                            item.ItemName2 &= " - " & dr("choicename")
                        End If

                    End If

                    If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        item.ItemName2 &= " - " & dr("PriceDesc")
                    End If

                    item.Price = dr("Price")
                    item.SKU = IIf(IsDBNull(dr("sku")), Nothing, dr("sku"))
                    If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
                    item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    item.ShortDesc = IIf(IsDBNull(dr("Shortdesc")), Nothing, dr("Shortdesc"))
                    'item.MixMatchId = IIf(IsDBNull(dr("mixmatchid")), Nothing, dr("mixmatchid"))
                    item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
                    If filter.OrderId <> Nothing Then item.IsInCart = dr("isincart")
                    'If (totalRow < 1) Then
                    '    totalRow = CInt(dr("Total"))
                    'End If
                    ''c.Add(item)
                    'item.MixMatchId = GetMixMatchID(0, item.ItemId)
                    'If item.MixMatchId <> Nothing Then
                    '    item.Promotion = PromotionRow.GetRow(database, item.MixMatchId, False)
                    'End If
                    item.ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(database, item, False, 1, 0, filter.MemberId, True) & "</div>"
                    ssi.Add(item)
                End While
                Core.CloseReader(dr)
                totalRow = CInt(cmd.Parameters("@TotalRecords").Value)
                Return ssi
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetItemSearchKeyword", ex)
            End Try
            Return ssi
        End Function

        Public Shared Function GetActiveItems(ByVal DB As Database, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection
            If filter.PromotionId <> Nothing Then Return GetPromotionItems(DB, filter)
            Dim c As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim SQL As String = String.Empty
                Dim SQLI As String = ""
                Dim SQLG As String = ""
                Dim counter As Integer = 0
                Dim sField As String = Common.GetSiteLanguage()
                'Group ItemGroup Items together?
                Dim GroupItems As Boolean = False



                SQLI = "WITH ActiveItemTmp AS" & vbCrLf
                SQLI &= "("
                ''SQLI &= "select " & vbCrLf
                If GroupItems Then SQLG &= "select " & vbCrLf

                Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()



                Dim dbMember As MemberRow
                If Not filter.MemberId = Nothing Then
                    dbMember = MemberRow.GetRow(filter.MemberId)
                Else
                    dbMember = New MemberRow(DB)
                    dbMember.Customer = New CustomerRow
                End If

                Dim hasSaleExp As String = String.Empty
                If (filter.Sale24Hour) Then
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](1," & dbMember.MemberId & ",'" & Now.ToShortDateString & "'," & dbMember.Customer.CustomerPriceGroupId & ",si.itemid,si.price)"
                Else
                    hasSaleExp = "[dbo].[fc_StoreItem_GetHassaleItem](0," & dbMember.MemberId & ",'" & Now.ToShortDateString & "'," & dbMember.Customer.CustomerPriceGroupId & ",si.itemid,si.price)"
                End If

                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                Select Case filter.SortBy
                    Case "price"
                        filter.SortBy = lowSalePriceExp & " asc, " & LowPriceExp & " asc, price asc "
                    Case "product"
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                    Case "best-sellers"
                        filter.SortBy = " IsBestSeller desc, " & LowPriceExp & " asc, itemname asc "
                    Case "new-items"
                        filter.SortBy = " IsNew desc, " & LowPriceExp & " asc, itemname asc "
                    Case "hot-items"
                        filter.SortBy = " ishot desc, " & LowPriceExp & " asc, itemname asc "
                    Case "featured"
                        filter.SortBy = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                    Case "on-sale"
                        filter.SortBy = hasSaleExp & " desc, " & lowSalePriceExp & " asc, itemname asc "
                    Case "top-rated"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetTopRatedSort](si.itemid) desc, itemname asc "
                    Case "most-popular-review"
                        filter.SortBy = "[dbo].[fc_StoreItem_GetMostPopularReviewSort](si.itemid) desc, itemname asc "
                    Case Else
                        filter.SortBy = " ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp & " asc "
                End Select
                filter.SortOrder = ""

                If filter.SortBy = String.Empty Then
                    'filter.SortBy = " lowsaleprice, LowPrice "
                    'filter.SortOrder = " asc "
                    filter.SortBy = " isFeatured desc, " & LowPriceExp & " asc, itemname asc "
                End If
                Dim sortExp As String = String.Empty
                If filter.SalesCategoryId <> Nothing Then
                    sortExp &= " sortorder "
                Else
                    sortExp &= " " & vbCrLf & Components.Core.ProtectParam(filter.SortBy) & " " & Components.Core.ProtectParam(filter.SortOrder) & ", si.ItemId DESC "
                End If
                SQLI &= "SELECT ROW_NUMBER() OVER(ORDER BY " & sortExp & " ) AS RowNum,"
                ''''''''''''''''''''''''''''''''''ITEMS''''''''''''''''''''''''''''''''''
                SQLI &= " si.itemid, " & vbCrLf &
                 "si.itemgroupid, " & vbCrLf &
                 "isnull(si.brandid,'') as Brandid, " & vbCrLf &
                 "si.price, " & vbCrLf &
                 "si.isnew, " & vbCrLf &
                 "si.isfeatured, " & vbCrLf &
                 "si.newuntil, " & vbCrLf &
                 "si.isbestseller, si.IsSpecialOrder, si.AcceptingOrder, " & vbCrLf &
                 "si.ishot, " & vbCrLf &
                 "si.isfreeshipping, " & vbCrLf &
                 "si.image, " & vbCrLf &
                 "si.sku,si.UrlCode, " & vbCrLf &
                 "si.qtyonhand, " & vbCrLf &
                 Common.GenerateCaseLanguage("si.short", sField) & " as shortdesc," & vbCrLf &
                 "si.itemname, " & vbCrLf &
                 "si.IsFreeSample, si.IsFreeGift, " & vbCrLf &
                 "si.pricedesc, isnull(si.IsFlammable,0) as IsFlammable, " & vbCrLf &
                 "dbo.fc_StoreItem_HasVariance(si.itemid) as IsVariance, " & vbCrLf & hasSaleExp & " as hassale," & vbCrLf & LowPriceExp & " as lowprice, " & vbCrLf &
                 "dbo.fc_StoreItem_GetMixMatchDescriptionByItem(si.itemid," & CustomerPriceGroupId & "," & filter.MemberId & "," & filter.OrderId & ") as MixMatchDescription, " & vbCrLf &
                 "case when si.itemgroupid is not null then (select max(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as highprice, " & vbCrLf &
                 lowSalePriceExp & " as lowsaleprice " & vbCrLf
                If Not GroupItems Then SQLI &= ", si.choicename "
                If filter.SalesCategoryId <> Nothing Then SQLI &= ", sci.sortorder " & vbCrLf

                If Not filter.OrderId = Nothing Then
                    SQLI &= ", case when exists (select cartitemid from storecartitem with (nolock) where orderid = " & filter.OrderId & " and itemid = si.itemid) then 1 else 0 end as isincart " & vbCrLf
                End If

                SQLI &= " from storeitem si with (nolock) " & vbCrLf
                SQLI &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & dbMember.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & dbMember.Customer.CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    SQLI &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If

                'Long edit Sales & special
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    SQLI &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    SQLI &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                'End
                If filter.SalesCategoryId <> Nothing Then
                    ' SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                    SQLI &= " and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                SQLI &= " where si.IsActive = 1 " & vbCrLf
                If filter.Keyword = Nothing Then
                    SQLI &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If

                If GroupItems Then SQLI &= " and si.itemgroupid is null " & vbCrLf
                If filter.IsSearchKeyWord Then
                    SQLI &= " and " & vbCrLf
                    SQLI &= "kw.KeywordName=" & DB.Quote(filter.Keyword) & vbCrLf
                Else
                    If filter.Keyword <> Nothing Then
                        SQLI &= " and " & vbCrLf
                        SQLI &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not filter.LoggedInPostingGroup = Nothing Then
                    SQLI &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    SQLI &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    SQLI &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    SQLI &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    SQLI &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    SQLI &= " and si.BrandId = " & DB.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                If Not filter.Feature = String.Empty Then
                    '' SQLI &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                    SQLI &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=(Select FeatureId from StoreFeature where URLCode = " & DB.Quote(filter.Feature) & "))" & vbCrLf
                End If
                If Not filter.PriceRange = String.Empty Then
                    Dim low, high As Integer
                    Dim a() As String = filter.PriceRange.Split("-")
                    If UBound(a) = 1 AndAlso IsNumeric(a(0)) AndAlso IsNumeric(a(1)) Then
                        low = CInt(a(0))
                        high = CInt(a(1))
                        SQLI &= " and si.price between " & low & " and " & high
                    End If
                End If

                SQL &= SQLI & vbCrLf & IIf(GroupItems, " union " & vbCrLf & SQLG, "") & vbCrLf


                'If filter.HasPromotion Then

                '    If filter.Sale24Hour Then
                '        SQL &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & DB.Quote(Now.ToShortDateString) & " and mm.endingdate < " & DB.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "
                '    ElseIf filter.SaleBuy1Get1 Then
                '        SQL &= " where itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                '    Else
                '        SQL &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "
                '    End If
                'End If

                If filter.HasPromotion Then

                    If filter.Sale24Hour Then
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & DB.Quote(Now.ToShortDateString) & " and mm.endingdate < " & DB.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf filter.SaleBuy1Get1 Then
                        SQL &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If

                SQL &= ") " & vbCrLf
                Dim startRow As String
                Dim endRow As String
                startRow = ((filter.pg * filter.MaxPerPage) - filter.MaxPerPage) + 1
                endRow = filter.pg * filter.MaxPerPage
                SQL &= "SELECT * ,(Select COUNT(*) from ActiveItemTmp ) as Total FROM ActiveItemTmp WHERE RowNum BETWEEN " & startRow & "  AND " & endRow



                dr = DB.GetReader(SQL)
                Dim LastId As Integer = Nothing
                If Not dr Is Nothing Then
                    While dr.Read


                        'skip first (pg-1) * maxperpage records
                        If filter.IsItemIdOnly Then
                            If c.Count = 0 Then 'haven't retrieved the last item yet
                                If filter.ItemId = dr("itemid") Then
                                    Dim item As New StoreItemRow(DB, LastId)
                                    c.Add(item)
                                Else
                                    LastId = dr("itemid")
                                End If
                            ElseIf filter.ItemId <> dr("itemid") Then 'have the last item, need the next
                                c.Add(New StoreItemRow(DB, CInt(dr("itemid"))))
                                Exit While 'we have what we need
                            End If
                        Else
                            '' If (counter > filter.MaxPerPage * (filter.pg - 1)) Or filter.MaxPerPage = -1 Then
                            Dim item As New StoreItemRow(DB)
                            item.LowPrice = dr("LowPrice")
                            item.HighPrice = dr("HighPrice")
                            item.LowSalePrice = IIf(IsDBNull(dr("LowSalePrice")), Nothing, dr("LowSalePrice"))
                            'item.HighSalePrice = IIf(IsDBNull(dr("HighSalePrice")), Nothing, dr("HighSalePrice"))
                            item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                            item.ItemId = dr("ItemId")
                            item.IsNew = CBool(dr("isnew"))
                            If Not IsDBNull(dr("newuntil")) Then item.NewUntil = dr("newuntil") Else item.NewUntil = Nothing
                            item.IsBestSeller = CBool(dr("IsBestSeller"))
                            item.IsHot = CBool(dr("IsHot"))
                            item.IsFreeShipping = CBool(dr("IsFreeShipping"))
                            item.IsFreeSample = CBool(dr("IsFreeSample"))
                            item.IsFreeGift = CInt(dr("IsFreeGift"))
                            item.IsSpecialOrder = CBool(dr("IsSpecialOrder"))
                            item.AcceptingOrder = CInt(dr("AcceptingOrder"))
                            item.ItemName = dr("ItemName")
                            item.ItemName2 = dr("ItemName")
                            item.IsFlammable = dr("IsFlammable")
                            If IsDBNull(dr.Item("URLCode")) Then
                                item.URLCode = Nothing
                            Else
                                item.URLCode = dr("URLCode")
                            End If
                            item.QtyOnHand = CInt(dr("QtyOnHand"))
                            item.IsVariance = CBool(dr("IsVariance"))
                            If Not IsDBNull(dr("choicename")) Then
                                item.ItemName &= " - " & dr("choicename")

                                If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                                    item.ItemName2 &= " - " & dr("choicename")
                                End If

                            End If

                            If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                                item.ItemName2 &= " - " & dr("PriceDesc")
                            End If

                            item.Price = dr("Price")
                            item.SKU = IIf(IsDBNull(dr("sku")), Nothing, dr("sku"))
                            If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
                            item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                            item.ShortDesc = IIf(IsDBNull(dr("Shortdesc")), Nothing, dr("Shortdesc"))
                            item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
                            item.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))
                            If filter.OrderId <> Nothing Then item.IsInCart = dr("isincart")
                            If (totalRow < 1) Then
                                totalRow = CInt(dr("Total"))
                            End If
                            c.Add(item)
                        End If
                    End While
                End If
                Core.CloseReader(dr)
                If c.Count = 1 AndAlso filter.IsItemIdOnly Then
                    c.Add(New StoreItemRow(DB, 0))
                End If
                If Not filter.IsItemIdOnly Then
                    For i As Integer = 0 To c.Count - 1
                        'c(i).MixMatchId = DB.ExecuteScalar("select top 1 mm.id from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,getdate()) and coalesce(mm.endingdate+1,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null) and mml.itemid = " & c(i).ItemId)
                        c(i).MixMatchId = GetMixMatchID(DB, c(i).MixMatchId, CustomerPriceGroupId)
                        If c(i).MixMatchId <> Nothing Then
                            c(i).Promotion = PromotionRow.GetRow(DB, c(i).MixMatchId, False)
                        End If
                        If filter.MemberId <> Nothing Then
                            c(i).ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(DB, c(i), False, 1, 0, filter.MemberId, True) & "</div>"
                        End If
                    Next
                End If
                Return c
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetActiveItems", ex)
            End Try

            Return c
        End Function


        Public Shared Sub SetMember(ByRef filter As DepartmentFilterFields, ByRef isInternational As Boolean)
            Dim CookieOrderId As Integer = 0
            Try
                filter.OrderId = 0
                CookieOrderId = Utility.Common.GetOrderIdFromCartCookie()
                If CookieOrderId <> 0 Then filter.OrderId = CookieOrderId
                If HttpContext.Current.Session("OrderId") <> Nothing Then filter.OrderId = HttpContext.Current.Session("OrderId")
                Dim dbMember As MemberRow = MemberRow.GetRow(Utility.Common.GetCurrentMemberId())
                filter.MemberId = dbMember.MemberId
                If dbMember.MemberId > 0 Then

                    isInternational = dbMember.IsInternational
                Else
                    isInternational = False
                End If
                filter.LoggedInPostingGroup = dbMember.CustomerPostingGroup
            Catch ex As Exception
            End Try
        End Sub
        'Public Shared Function BindList(ByVal sic As StoreItemCollection, ByVal countData As Integer, ByVal ucproduct As Object, ByVal isInternational As Boolean) As String
        '    Dim pageHolder As New Page()
        '    Dim strXmlData As String = ""
        '    strXmlData = "<Data>"
        '    For i As Integer = 0 To sic.Count - 1
        '        ucproduct.IsFirstLoad = False
        '        ucproduct.Fill(sic(i).itemIndex, sic(i), "", 0, isInternational)
        '        pageHolder.Controls.Add(ucproduct)
        '        Dim output As New System.IO.StringWriter()
        '        HttpContext.Current.Server.Execute(pageHolder, output, False)
        '        strXmlData += vbCrLf & "<Items>"
        '        strXmlData += SetXMLtag("content", output.ToString, True)
        '        strXmlData += SetXMLtag("RowNum", sic(i).itemIndex, True)
        '        strXmlData += "</Items>"
        '    Next
        '    strXmlData += vbCrLf & "<PageCount>" & vbCrLf & "<PageCount>" & countData & "</PageCount>" & vbCrLf & "</PageCount>" & vbCrLf & "</Data>"
        '    Return strXmlData
        'End Function
        Public Shared Function BindList(ByVal sic As StoreItemCollection, ByVal countData As Integer, ByVal ucproduct As Object, ByVal isInternational As Boolean, ByVal isPageCollection As Boolean) As String
            Dim pageHolder As New Page()
            Dim strXmlData As String = ""
            ' strXmlData = "<Data>"

            For i As Integer = 0 To sic.Count - 1
                ''Dim sb As New StringBuilder()
                ''Dim tw As System.IO.StringWriter = New System.IO.StringWriter(sb)
                ''Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
                ''Dim uc As New UserControl
                ucproduct.IsFirstLoad = False
                ucproduct.Fill(sic(i).itemIndex, sic(i), "", 0, isInternational)
                ''uc = ucproduct
                ''uc.RenderControl(hw)
                pageHolder.Controls.Add(ucproduct)
                Dim output As New System.IO.StringWriter()
                HttpContext.Current.Server.Execute(pageHolder, output, False)

                'strXmlData += vbCrLf & "<Items>"
                'strXmlData += SetXMLtag("content", output.ToString, True)
                'strXmlData += SetXMLtag("RowNum", sic(i).itemIndex, True)
                'strXmlData += "</Items>"
                strXmlData &= output.ToString() & IIf(isPageCollection, "", "<div class=""ver-line"">&nbsp;</div>") 'sb.ToString() & IIf(isPageCollection, "", "<div class=""ver-line"">&nbsp;</div>") 
            Next
            'strXmlData += vbCrLf & "<PageCount>" & vbCrLf & "<PageCount>" & countData & "</PageCount>" & vbCrLf & "</PageCount>" & vbCrLf & "</Data>"
            Return strXmlData
        End Function
        Public Shared Function ReturnPrice(ByVal objPrice As Object) As String
            Dim str As String = String.Empty
            If objPrice.NormalPrice > 0 Then
                str = "<span class=""bold"">" & FormatCurrency(objPrice.NormalPrice) & "</span>" 'si.ShowPrice
            ElseIf objPrice.RegularPrice > 0 Then
                str = "<span class=""strike bold"">" & FormatCurrency(objPrice.RegularPrice) & "</span>&nbsp;&nbsp;<span class=""red bold"">" & FormatCurrency(objPrice.SalePrice) & "</span>"
            ElseIf objPrice.MinMultiPrice > 0 Then
                str = "<span class=""aslowas"">As low as " & FormatCurrency(objPrice.MinMultiPrice) & "</span>"
            End If
            Return str
        End Function
        Public Shared Function SetXMLtag(ByVal colName As String, ByVal Value As String, ByVal cData As Boolean)
            Return vbCrLf & "<" & colName & ">" & IIf(cData, CheckCDATA(Value), Value) & "</" & colName & ">"
        End Function

        Public Shared Function CountNarrowSearch(ByVal filter As DepartmentFilterFields, ByVal flag As String) As List(Of String)
            Dim cacheName As String = String.Empty
            Dim lst As New List(Of String)

            If HttpContext.Current.Request.RawUrl.Contains("nail-supply") Or HttpContext.Current.Request.RawUrl.Contains("sub-category") Then

                cacheName = "CountNarrowSearch" & "_" & IIf(flag.Contains("Brand"), "Brand", flag) & HttpContext.Current.Request.RawUrl.Replace("/nail-supply", "").Replace("/sub-category.aspx", "")
                lst = CType(CacheUtils.GetCache(cacheName), List(Of String))
                If lst IsNot Nothing Then
                    Return lst
                Else
                    lst = New List(Of String)
                End If
            End If

            Dim arr As Array
            If flag = "Price" Then
                arr = [Enum].GetValues(GetType(Utility.Common.Price))
            ElseIf flag = "Rating" Then
                arr = [Enum].GetValues(GetType(Utility.Common.Rating))
            ElseIf flag.Contains("Brand") Then
                arr = flag.Replace("Brand,", "").Split(",")
                flag = "Brand"
            End If

            Try
                Dim SQL As String = String.Empty
                Dim SQU As String = ""
                Dim SQFor As String = ""

                Dim sField As String = Common.GetSiteLanguage()

                Dim CustomerPriceGroupId As Integer = 0
                If Not IsNumeric(System.Web.HttpContext.Current.Session("CustomerPriceGroupId")) Then
                    CustomerPriceGroupId = GetCustomerPriceGroupByMember(filter.MemberId)
                End If
                HttpContext.Current.Session("CustomerPriceGroupId") = CustomerPriceGroupId

                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"

                SQU &= "SELECT COUNT(si.ItemId) FROM StoreItem si WITH (NOLOCK) LEFT OUTER JOIN (SELECT MIN(UnitPrice) AS UnitPrice, ItemId FROM salesprice WITH (NOLOCK) WHERE minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((memberid is null and salestype = 2)) group by itemid) sp on sp.itemid = si.itemid "
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    SQU &= " INNER JOIN StoreDepartmentItem sd ON si.ItemId = sd.ItemId AND sd.DepartmentId = " & filter.DepartmentId
                End If

                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    SQU &= " INNER JOIN SalesCategoryItem sci ON si.itemid = sci.itemid "
                End If

                If filter.SalesCategoryId <> Nothing Then SQU &= " AND SalesCategoryId = " & filter.SalesCategoryId & " "

                SQU &= " WHERE si.IsActive = 1 "

                If filter.Keyword = Nothing Then SQU &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 "

                If Not filter.LoggedInPostingGroup = Nothing Then
                    SQU &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(filter.LoggedInPostingGroup) & ") "
                End If

                If filter.IsFeatured Then SQU &= " and si.IsFeatured = 1"

                If filter.IsNew Then SQU &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) "

                If filter.IsHot Then SQU &= " and si.IsHot = 1 "

                If filter.IsOnSale Then SQU &= " and (si.IsOnSale = 1 ) "

                If filter.BrandId > 0 Then SQU &= " and si.BrandId = " & Database.Number(filter.BrandId)

                If Not filter.CollectionId = 0 Then
                    SQU &= " and si.ItemId in (SELECT ItemId FROM storecollectionitem with (NOLOCK) WHERE collectionid = " & filter.CollectionId & ") "
                End If
                If Not filter.ToneId = 0 Then
                    SQU &= " and si.ItemId in (SELECT ItemId FROM storetoneitem with (NOLOCK) WHERE toneid = " & filter.ToneId & ") "
                End If
                If Not filter.ShadeId = 0 Then
                    SQU &= " and si.ItemId in (SELECT ItemId FROM storeshadeitem with (NOLOCK) WHERE shadeid = " & filter.ShadeId & ") "
                End If
                If Not filter.Feature = String.Empty Then
                    SQU &= " and si.ItemId in (SELECT ItemId FROM StoreItemFeature where FeatureId = (SELECT FeatureId FROM StoreFeature WHERE URLCode = " & Database.Quote(filter.Feature) & "))"
                End If

                If flag = "Price" Or flag = "Brand" Then
                    If Not filter.RatingRange = String.Empty Then
                        Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                        Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                        If (high > 0) Then
                            SQU &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                        Else
                            SQU &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                        End If
                    End If
                End If

                If flag = "Rating" Or flag = "Brand" Then
                    If Not filter.PriceRange = String.Empty Then
                        Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                        Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                        If (high > 0) Then
                            SQU &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                        Else
                            SQU &= " and " & lowSalePriceExp & " >= " & low
                        End If
                    End If
                End If

                Dim max As Integer = arr.Length - 1
                For i As Integer = 0 To max
                    SQFor &= "|"
                    If flag = "Price" Then
                        SQFor &= " AND " & lowSalePriceExp & " >= " & arr(i)
                    ElseIf flag = "Rating" Then
                        SQFor &= " AND dbo.fc_StoreItem_GetTopRatedSort(si.ItemId) >= " & arr(i)
                    End If

                    If i < max Then
                        If flag = "Price" Then
                            SQFor &= " AND " & lowSalePriceExp & " < " & arr(i + 1)
                        ElseIf flag = "Rating" Then
                            SQFor &= " AND dbo.fc_StoreItem_GetTopRatedSort(si.ItemId) < " & arr(i + 1)
                        End If
                    End If

                    If flag = "Brand" Then SQFor &= " and si.BrandId = " & Database.Number(arr(i))
                Next

                If Not String.IsNullOrEmpty(SQFor) Then
                    SQFor = SQFor.Substring(1)
                End If

                Dim dr As SqlDataReader
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Try
                    Dim sp As String = "sp_StoreItem_CountNarrowSearch"
                    Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                    db.AddInParameter(cmd, "SQLUnion", DbType.String, SQU)
                    db.AddInParameter(cmd, "SQLFor", DbType.String, SQFor)
                    dr = db.ExecuteReader(cmd)
                    While dr.Read()
                        lst.Add(dr(0))
                    End While

                    If HttpContext.Current.Request.RawUrl.Contains("nail-supply") Then
                        cacheName = "CountNarrowSearch" & "_" & flag & HttpContext.Current.Request.RawUrl.Replace("/nail-supply", "")
                        CacheUtils.SetCache(cacheName, lst)
                    End If

                    Core.CloseReader(dr)
                Catch ex As Exception
                    Core.CloseReader(dr)
                End Try

            Catch ex As Exception
                Email.SendError("ToError500", "CountNarrowSearch" & flag, "RawUrl" & HttpContext.Current.Request.RawUrl() & "<br>Exception:" & ex.ToString())
            End Try

            Return lst
        End Function

        Public Shared Function GetCountActiveItemsNarrowSearch(ByVal DB As Database, ByVal filter As DepartmentFilterFields) As Integer
            ''If filter.PromotionId <> Nothing Then Return GetPromotionItems(DB, filter)
            Dim c As New StoreItemCollection
            Try
                Dim SQL As String = String.Empty
                Dim SQLI As String = ""
                Dim SQLG As String = ""

                Dim sField As String = Common.GetSiteLanguage()
                'Group ItemGroup Items together?
                Dim GroupItems As Boolean = False
                If GroupItems Then SQLG &= "select " & vbCrLf

                Dim CustomerPriceGroupId As Integer = 0
                If Not IsNumeric(System.Web.HttpContext.Current.Session("CustomerPriceGroupId")) Then
                    'CustomerPriceGroupId = DB.ExecuteScalar("select top 1 coalesce(CustomerPriceGroupId,0) from customer where customerid = (select top 1 memberid from member where memberid = " & filter.MemberId & ")")
                    CustomerPriceGroupId = GetCustomerPriceGroupByMember(filter.MemberId)
                End If
                System.Web.HttpContext.Current.Session("CustomerPriceGroupId") = CustomerPriceGroupId


                Dim dbMember As MemberRow
                If Not filter.MemberId = Nothing Then
                    dbMember = MemberRow.GetRow(filter.MemberId)
                Else
                    dbMember = New MemberRow(DB)
                    dbMember.Customer = New CustomerRow
                End If

                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"

                SQLI &= "SELECT COUNT (*) "

                SQLI &= " from storeitem si with (nolock) " & vbCrLf
                SQLI &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & dbMember.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & dbMember.Customer.CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    SQLI &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If
                'Long edit Sales & special
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                'End
                If filter.SalesCategoryId <> Nothing Then
                    ' SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                    SQLI &= " and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                SQLI &= " where si.IsActive = 1 " & vbCrLf
                If filter.Keyword = Nothing Then
                    SQLI &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If

                If GroupItems Then SQLI &= " and si.itemgroupid is null " & vbCrLf

                If Not filter.LoggedInPostingGroup = Nothing Then
                    SQLI &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    SQLI &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    SQLI &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    SQLI &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    SQLI &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    SQLI &= " and si.BrandId = " & DB.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                If Not filter.Feature = String.Empty Then
                    '' SQLI &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                    SQLI &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=(Select FeatureId from StoreFeature where URLCode = " & DB.Quote(filter.Feature) & "))" & vbCrLf
                End If

                If (filter.MinPrice > 0 Or filter.MaxPrice > 0) Then
                    If (filter.MinPrice > 0 And filter.MaxPrice <= 0) Then
                        SQLI &= " and " & lowSalePriceExp & " >= " & filter.MinPrice
                    Else
                        SQLI &= " and " & lowSalePriceExp & " >= " & filter.MinPrice & " AND " & lowSalePriceExp & " < " & filter.MaxPrice
                    End If
                ElseIf Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        SQLI &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        SQLI &= " and " & lowSalePriceExp & " >= " & low
                    End If
                End If
                If (filter.MinRating > 0 Or filter.MaxRating > 0) Then
                    If (filter.MinRating > 0 And filter.MaxRating <= 0) Then
                        SQLI &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & filter.MinRating
                    Else
                        SQLI &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & filter.MinRating & " AND dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & filter.MaxRating
                    End If
                ElseIf Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        SQLI &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        SQLI &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If
                End If

                SQL &= SQLI & vbCrLf & IIf(GroupItems, " union " & vbCrLf & SQLG, "") & vbCrLf
                Return DB.ExecuteScalar(SQL)
            Catch ex As Exception
                SendMailLog("GetCountActiveItemsNarrowSearch", ex)
            End Try
            Return 0
        End Function

        Public Shared Sub CheckShowSortFieldInSearchKeyword(ByVal KeywordName As String, ByRef hasNew As Boolean, ByRef hasHost As Boolean, ByRef hasBestSeller As Boolean, ByRef hasToprate As Boolean)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_CheckShowSortFieldInSearchKeyword"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "KeywordName", DbType.String, KeywordName)
                db.AddOutParameter(cmd, "countHotItem", DbType.Int32, 0)
                db.AddOutParameter(cmd, "countNewItem", DbType.Int32, 0)
                db.AddOutParameter(cmd, "countBestSeller", DbType.Int32, 0)
                db.AddOutParameter(cmd, "countTopRate", DbType.Int32, 0)
                db.ExecuteNonQuery(cmd)
                Dim countHotItem As Integer = CInt(cmd.Parameters("@countHotItem").Value)
                Dim countNewItem As Integer = CInt(cmd.Parameters("@countNewItem").Value)
                Dim countBestSeller As Integer = CInt(cmd.Parameters("@countBestSeller").Value)
                Dim countTopRate As Integer = CInt(cmd.Parameters("@countTopRate").Value)
                If countHotItem > 0 Then
                    hasHost = True
                Else
                    hasHost = False
                End If
                If countNewItem > 0 Then
                    hasNew = True
                Else
                    hasNew = False
                End If
                If countBestSeller > 0 Then
                    hasBestSeller = True
                Else
                    hasBestSeller = False
                End If
                If countTopRate > 0 Then
                    hasToprate = True
                Else
                    hasToprate = False
                End If
            Catch ex As Exception
            End Try

        End Sub

        Public Shared Sub GetItemSearchInfor(ByRef item As StoreItemRow, ByVal memberId As Integer, ByVal orderId As Integer)
            Dim dr As SqlDataReader = Nothing
            Try
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim sql As String = "Select ItemId,[dbo].[fc_StoreItem_GetLowprice](itemgroupid,price) as LowPrice, dbo.[fc_StoreItemEnable_IsLoginViewPrice](" & Utility.Common.GetCurrentMemberId().ToString() & ", si.BrandId) AS IsLoginViewPrice, "
                sql = sql & " case when si.itemgroupid is not null then (select max(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as highprice,"
                sql = sql & "price,PriceDesc,newuntil,isnull(CasePrice, 0) as CasePrice,isnull(CaseQty, 0) as CaseQty"
                sql = sql & ",IsFreeShipping,IsFreeSample,IsFreeGift,IsSpecialOrder,AcceptingOrder,IsFlammable,"
                sql = sql & "QtyOnHand,dbo.fc_StoreItem_HasVariance(si.itemid) as IsVariance,dbo.fc_StoreItem_GetMixMatchDescriptionByItem(si.itemid," & customerPriceGroupId & "," & memberId & "," & orderId & ") as MixMatchDescription,"
                sql = sql & "choicename,itemgroupid,dbo.fc_CheckPermissionBuyBrand(" & Utility.Common.GetCurrentMemberId() & ", si.BrandId) AS PermissionBuyBrand,coalesce([dbo].[fc_StoreItem_GetCurrentLowSalePriceItem](SI.itemid),SI.price) as lowsaleprice"
                sql = sql & " from StoreItem si where SKU='" & item.SKU & "'"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                dr = db.ExecuteReader(cmd)
                If dr.Read() Then
                    item.ItemId = dr("ItemId")
                    item.LowPrice = dr("LowPrice")
                    item.HighPrice = dr("HighPrice")
                    item.LowSalePrice = dr("lowsaleprice")
                    item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                    If Not IsDBNull(dr("newuntil")) Then item.NewUntil = dr("newuntil") Else item.NewUntil = Nothing
                    'item.IsBestSeller = CBool(dr("IsBestSeller"))
                    'item.IsHot = CBool(dr("IsHot"))
                    item.IsFreeShipping = CBool(dr("IsFreeShipping"))
                    item.IsFreeSample = CBool(dr("IsFreeSample"))
                    item.IsFreeGift = CInt(dr("IsFreeGift"))
                    item.IsSpecialOrder = CBool(dr("IsSpecialOrder"))
                    item.AcceptingOrder = CInt(dr("AcceptingOrder"))
                    item.ItemName2 = item.ItemName
                    If Not IsDBNull(dr("IsFlammable")) Then item.IsFlammable = CBool(dr("IsFlammable")) Else item.IsFlammable = Nothing
                    item.QtyOnHand = CInt(dr("QtyOnHand"))
                    item.IsVariance = CBool(dr("IsVariance"))
                    item.Price = dr("Price")
                    If Not IsDBNull(dr("choicename")) Then
                        item.ItemName &= " - " & dr("choicename")

                        If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                            item.ItemName2 &= " - " & dr("choicename")
                        End If
                    End If
                    If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                        item.ItemName2 &= " - " & dr("PriceDesc")
                    End If
                    If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
                    item.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))
                    item.PermissionBuyBrand = CBool(dr("PermissionBuyBrand"))
                    item.CasePrice = dr("CasePrice")
                    item.CaseQty = dr("CaseQty")
                    item.IsLoginViewPrice = dr("IsLoginViewPrice")
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetItemSearchInfor", ex)
            End Try




        End Sub

        Public Shared Function CheckURLCodeDuplicate(ByVal urlCode As String, ByVal ItemId As Integer) As Boolean
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREITEM_GETOBJECT As String = "sp_StoreItem_CheckDuplicateURLCode"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETOBJECT)
            db.AddInParameter(cmd, "urlCode", DbType.String, urlCode)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            result = Convert.ToInt32(db.ExecuteScalar(cmd))
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function CheckSKUDuplicate(ByVal SKU As String, ByVal ItemId As Integer) As Boolean
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREITEM_GETOBJECT As String = "sp_StoreItem_CheckDuplicateSKU"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETOBJECT)
            db.AddInParameter(cmd, "SKU", DbType.String, SKU)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            result = Convert.ToInt32(db.ExecuteScalar(cmd))
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function GetCustomerPriceGroupByMember(ByVal memberId As Integer) As Integer
            Dim customerPriceGroup As Integer = 0

            If memberId > 0 Then
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_STOREITEM_GETOBJECT As String = "sp_CustomerPriceGroup_GetIDByMember"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETOBJECT)

                db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)

                customerPriceGroup = Convert.ToInt32(db.ExecuteScalar(cmd))
            End If


            Return customerPriceGroup
        End Function

        Public Shared Function GetMixMatchID(ByVal DB As Database, ByVal itemId As Integer, ByVal CustomerPriceGroupId As Integer) As Integer
            Dim key As String = String.Format(cachePrefixKey & "GetMixMatchID_{0}_{1}", itemId, CustomerPriceGroupId)
            Dim result As Integer = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If

            Try
                Dim sql As String = "Select [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & itemId & "," & CustomerPriceGroupId & ",0) --StoreItem.GetMixMatchID>ItemId:" & itemId
                result = DB.ExecuteScalar(sql)
                CacheUtils.SetCache(key, result, 3600)
            Catch ex As Exception
                SendMailLog("GetMixMatchID(itemId: " & itemId & ",CustomerPriceGroupId: " & CustomerPriceGroupId & ")", ex)
            End Try

            Return result
        End Function

        Public Shared Function GetHandlingFee(ByVal DB As Database, ByVal itemId As Integer, Optional ByVal IsItemCase As Boolean = False, Optional ByVal IsFirstManual As Boolean = False) As Double
            Dim key As String = String.Format(cachePrefixKey & "GetHandlingFee_{0}_{1}", itemId, IsItemCase)
            Dim result As Double = CType(CacheUtils.GetCache(key), Integer)
            If result > 0 Then
                Return result
            End If

            Try
                Dim sql As String = "Select [dbo].[fc_StoreItem_GetSpecialHandlingFee](" & itemId & ", " & CInt(IsItemCase) & ", " & CInt(IsFirstManual) & ")"
                result = DB.ExecuteScalar(sql)
                CacheUtils.SetCache(key, result, 3600)
            Catch ex As Exception
                Email.SendError("ToError500", "StoreItem.vb > GetHandlingFee", "itemId: " & itemId & "<br>IsItemCase: " & IsItemCase.ToString() & "<br>Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function GetActiveItemsCount(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields) As Integer
            Dim SQL As String = String.Empty
            Dim SQLI As String = ""
            Dim SQLG As String = ""
            Dim counter As Integer = 0
            Dim Context As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim sField As String
            If Context.Session("Language") = Nothing Then sField = "desc" Else sField = "viet"

            ''Group ItemGroup Items together?
            Dim GroupItems As Boolean = False

            SQL = " select count(*) "
            SQL &= " from ("

            SQLI = "select " & vbCrLf
            If GroupItems Then SQLG = "select " & vbCrLf

            Dim CustomerPriceGroupId As Integer = 0
            If Not IsNumeric(System.Web.HttpContext.Current.Session("CustomerPriceGroupId")) Then
                CustomerPriceGroupId = DB1.ExecuteScalar("select top 1 coalesce(CustomerPriceGroupId,0) from customer where customerid = (select top 1 memberid from member where memberid = " & filter.MemberId & ")")
                'CustomerPriceGroupId = GetCustomerPriceGroupID(filter.MemberId)
            End If
            System.Web.HttpContext.Current.Session("CustomerPriceGroupId") = CustomerPriceGroupId

            'Long edit 03/28/2011(Count for only salesspecial set in admin)
            Dim dbMember As MemberRow
            If Not filter.MemberId = Nothing Then
                dbMember = MemberRow.GetRow(filter.MemberId)
            Else
                dbMember = New MemberRow(DB1)
                dbMember.Customer = New CustomerRow
            End If

            SQLI &= " si.itemid, " & vbCrLf &
            "case when si.itemgroupid is not null then (select min(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as lowprice, " & vbCrLf &
            "coalesce(sp.unitprice,si.price) as lowsaleprice " & vbCrLf
            SQLI &= " from storeitem si with (nolock) " & vbCrLf
            SQLI &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & DB1.Quote(Now.ToShortDateString) & " and endingdate = " & DB1.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & dbMember.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & dbMember.Customer.CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
            If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                SQLI &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
            End If

            'Long edit Sales & special
            If filter.HasPromotion Then
                SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
            End If
            'End
            If filter.SalesCategoryId <> Nothing Then
                'SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                SQLI &= " and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
            End If
            SQLI &= " where si.IsActive = 1 " & vbCrLf
            If filter.Keyword = Nothing Then
                SQLI &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
            End If
            If GroupItems Then SQLI &= " and si.itemgroupid is null " & vbCrLf
            If filter.Keyword <> Nothing Then
                SQLI &= " and " & vbCrLf
                SQLI &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB1.Quote(filter.Keyword) & ")) " & vbCrLf
            End If
            If Not filter.LoggedInPostingGroup = Nothing Then
                SQLI &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB1.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
            End If
            If filter.IsFeatured Then
                SQLI &= " and si.IsFeatured = 1" & vbCrLf
            End If
            If filter.IsNew Then
                SQLI &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
            End If
            If filter.IsHot Then
                SQLI &= " and si.IsHot = 1 " & vbCrLf
            End If
            If filter.IsOnSale Then
                SQLI &= " and (si.IsOnSale = 1 ) " & vbCrLf
            End If
            If Not filter.BrandId = 0 Then
                SQLI &= " and si.BrandId = " & DB1.Number(filter.BrandId) & vbCrLf
            End If
            If Not filter.CollectionId = 0 Then
                SQLI &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
            End If
            If Not filter.ToneId = 0 Then
                SQLI &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
            End If
            If Not filter.ShadeId = 0 Then
                SQLI &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
            End If
            If Not filter.Feature = String.Empty Then
                SQLI &= " and si.ItemId in (select itemid from storeitemfeaturefilter with (nolock) where URLCode = " & DB1.Quote(filter.Feature) & ") " & vbCrLf
            End If
            If Not filter.PriceRange = String.Empty Then
                Dim low, high As Integer
                Dim a() As String = filter.PriceRange.Split("-")
                If UBound(a) = 1 AndAlso IsNumeric(a(0)) AndAlso IsNumeric(a(1)) Then
                    low = CInt(a(0))
                    high = CInt(a(1))
                    SQLI &= " and si.price between " & low & " and " & high
                End If
            End If

            SQL &= SQLI & vbCrLf & IIf(GroupItems, " union " & vbCrLf & SQLG, "") & vbCrLf

            SQL &= ") tmp1 " & vbCrLf
            'End 28/03/2011
            If filter.HasPromotion Then
                If filter.Sale24Hour Then
                    SQL &= " where itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & DB1.Quote(Now.ToShortDateString) & " and mm.endingdate < " & DB1.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB1.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                ElseIf filter.SaleBuy1Get1 Then
                    SQL &= " where itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,getdate()) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB1.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                Else
                    SQL &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,getdate()) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB1.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "

                End If
            End If

            Return DB1.ExecuteScalar(SQL)
        End Function

        Public Shared Function GetListItems(ByVal filter As DepartmentFilterFields, ByRef total As Integer) As StoreItemCollection
            Dim key As String = String.Empty
            Dim sField As String = Common.GetSiteLanguage
            Dim c As StoreItemCollection = New StoreItemCollection
            Dim sortBy = filter.SortBy
            Dim dr As SqlDataReader = Nothing
            Try
                If filter.pg = 1 Then
                    counter = 0
                End If
                If (filter.MemberId < 0) Then
                    filter.MemberId = Utility.Common.GetCurrentMemberId()
                End If

                If (filter.OrderId < 0) Then
                    filter.OrderId = Utility.Common.GetCurrentOrderId()
                End If
                If filter.PromotionId <> Nothing Then Return GetPromotionItems(filter)
                Dim SQL As String = String.Empty
                Dim SQLI As String = ""
                Dim SQLG As String = ""
                Dim GroupItems As Boolean = False
                SQL = " select {0}"
                SQL &= "  from ("
                SQLI = "select distinct " & vbCrLf
                If GroupItems Then SQLG = "select " & vbCrLf
                Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                ''''''''''''''''''''''''''''''''''ITEMS''''''''''''''''''''''''''''''''''
                SQLI &= " si.itemid, " & vbCrLf &
                 "si.itemgroupid, " & vbCrLf &
                 "isnull(si.brandid,'') as Brandid, " & vbCrLf &
                 "si.price, " & vbCrLf &
                 "si.image, " & vbCrLf &
                 "si.sku,si.UrlCode,si.Status,si.BODate,si.LowStockThreshold,si.LowStockMsg, " & vbCrLf &
                 "si.qtyonhand, " & vbCrLf &
                 "coalesce(si.short" & sField & ", si.shortdesc) as shortdesc, " & vbCrLf &
                 "si.itemname, " & vbCrLf &
                 "IsSpecialOrder, AcceptingOrder, " & vbCrLf &
                 "si.IsFreeSample, si.IsFreeGift, " & vbCrLf &
                 "si.pricedesc, " & vbCrLf &
                 "case when exists(select top 1 itemid from salesprice sp with (nolock) where sp.minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) and sp.itemid = si.itemid and sp.unitprice < si.price)then 1 when exists(select mm.id from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mml.itemid = si.itemid and mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) then 1 else 0 end as hassale," & vbCrLf &
                 "case when si.itemgroupid is not null then (select min(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as lowprice, " & vbCrLf &
                 "case when si.itemgroupid is not null then (select max(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as highprice, " & vbCrLf &
                 "coalesce(sp.unitprice,si.price) as lowsaleprice, " & vbCrLf &
                  "dbo.fc_StoreItem_GetMixMatchDescriptionByItem(si.itemid," & CustomerPriceGroupId & "," & filter.MemberId & "," & filter.OrderId & ") as MixMatchDescription, " & vbCrLf &
                 "si.IsFlammable, dbo.[fc_Review_CountReview](si.ItemId) AS CountReview, dbo.[fc_Review_AverageReview](si.ItemId) AS AverageReview " & vbCrLf &
                 ",dbo.fc_CheckPermissionBuyBrand(" & IIf(HttpContext.Current.Session("MemberId") Is Nothing, 0, Convert.ToInt32(HttpContext.Current.Session("MemberId"))) & ", si.BrandId) AS PermissionBuyBrand" & vbCrLf



                If Not GroupItems Then SQLI &= ", si.choicename "
                'If filter.SalesCategoryId <> Nothing Then SQLI &= ", sci.sortorder " & vbCrLf

                If Not filter.OrderId = Nothing Then
                    SQLI &= ", case when exists (select cartitemid from storecartitem with (nolock) where orderid = " & filter.OrderId & " and itemid = si.itemid) then 1 else 0 end as isincart " & vbCrLf
                End If

                SQLI &= " from storeitem si with (nolock) " & vbCrLf
                SQLI &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                SQLI &= " where si.IsActive = 1 and si.brandid <> 0 " & vbCrLf
                If filter.Keyword = Nothing Then
                    SQLI &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If GroupItems Then SQLI &= " and si.itemgroupid is null " & vbCrLf
                If Not filter.LoggedInPostingGroup = Nothing Then
                    SQLI &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    SQLI &= " and si.BrandId = " & Database.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    SQLI &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If
                If Not filter.Feature = String.Empty Then
                    ''SQLI &= " and si.ItemId in (select itemid from storeitemfeaturefilter with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                    ''SQLI &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                    SQLI &= " and si.ItemId in (select itemid from StoreItemFeature where FeatureId=(Select FeatureId from StoreFeature where URLCode = " & Database.Quote(filter.Feature) & "))" & vbCrLf


                End If

                SQL &= SQLI & vbCrLf & IIf(GroupItems, " union " & vbCrLf & SQLG, "") & vbCrLf
                SQL &= ") tmp1 " & vbCrLf
                If filter.HasPromotion Then
                    SQL &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "
                End If
                Select Case filter.SortBy
                    Case "price"
                        filter.SortBy = " lowsaleprice asc, LowPrice asc, price asc "
                    Case "product"
                        filter.SortBy = " ItemName, lowsaleprice asc, LowPrice asc "
                    Case "on-sale"
                        filter.SortBy = " hassale desc, lowsaleprice asc, itemname asc "
                    Case Else
                        filter.SortBy = " ItemName, lowsaleprice asc, LowPrice asc "
                End Select
                filter.SortOrder = ""

                If filter.SortBy = String.Empty Then
                    filter.SortBy = " isFeatured desc, Lowprice asc, itemname asc "
                End If
                Dim sqlTotal As String = String.Format(SQL, " count(*) ")
                Dim db1 As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand
                cmd = db1.GetSqlStringCommand(sqlTotal)
                total = db1.ExecuteScalar(cmd)
                'fix loi paging sai khi load pagezie > count record
                'If total * 2 < filter.MaxPerPage * filter.pg Then
                '    filter.pg = 1
                'End If
                'end
                If filter.SalesCategoryId <> Nothing Then
                    SQL &= " order by " & Components.Core.ProtectParam(filter.SortOrder)
                Else
                    SQL &= " order by " & vbCrLf & Components.Core.ProtectParam(filter.SortBy) & " " & Components.Core.ProtectParam(filter.SortOrder) & ", ItemId DESC "
                End If
                ' Dim top As Integer = IIf(
                SQL = String.Format(SQL, " top " & filter.pg * filter.MaxPerPage & " * ")
                cmd = db1.GetSqlStringCommand(SQL)
                dr = db1.ExecuteReader(cmd)
                Dim LastId As Integer = Nothing
                counter = 0
                While dr.Read
                    counter += 1

                    'skip first (pg-1) * maxperpage records
                    If filter.IsItemIdOnly Then
                        If c.Count = 0 Then 'haven't retrieved the last item yet
                            If filter.ItemId = dr("itemid") Then
                                ' Dim item As New StoreItemRow(DB, LastId)
                                Dim item As New StoreItemRow()
                                item.ItemId = LastId
                                c.Add(item)
                            Else
                                LastId = dr("itemid")
                            End If
                        ElseIf filter.ItemId <> dr("itemid") Then 'have the last item, need the next
                            ' c.Add(New StoreItemRow(DB, CInt(dr("itemid"))))
                            Dim item As New StoreItemRow()
                            item.ItemId = dr("itemid")
                            c.Add(item)
                            Exit While 'we have what we need
                        End If
                    Else
                        If (counter > filter.MaxPerPage * (filter.pg - 1)) Or filter.MaxPerPage = -1 Then
                            Dim item As New StoreItemRow()
                            item.LowPrice = dr("LowPrice")
                            item.HighPrice = dr("HighPrice")
                            item.LowSalePrice = IIf(IsDBNull(dr("LowSalePrice")), Nothing, dr("LowSalePrice"))
                            'item.HighSalePrice = IIf(IsDBNull(dr("HighSalePrice")), Nothing, dr("HighSalePrice"))
                            item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
                            item.ItemId = dr("ItemId")
                            item.IsFreeSample = CBool(dr("IsFreeSample"))
                            item.IsFreeGift = CInt(dr("IsFreeGift"))
                            item.ItemName = dr("ItemName")
                            item.ItemName2 = dr("ItemName")
                            If IsDBNull(dr.Item("URLCode")) Then
                                item.URLCode = Nothing
                            Else
                                item.URLCode = dr("URLCode")
                            End If
                            item.QtyOnHand = CInt(dr("QtyOnHand"))
                            If Not IsDBNull(dr("choicename")) Then
                                item.ItemName &= " - " & dr("choicename")

                                If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                                    item.ItemName2 &= " - " & dr("choicename")
                                End If

                            End If

                            If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                                item.ItemName2 &= " - " & dr("PriceDesc")
                            End If

                            item.Price = dr("Price")
                            item.SKU = IIf(IsDBNull(dr("sku")), Nothing, dr("sku"))
                            If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
                            item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                            item.ShortDesc = IIf(IsDBNull(dr("Shortdesc")), Nothing, dr("Shortdesc"))
                            'item.MixMatchId = IIf(IsDBNull(dr("mixmatchid")), Nothing, dr("mixmatchid"))
                            item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
                            If filter.OrderId <> Nothing Then item.IsInCart = dr("isincart")
                            item.IsSpecialOrder = CBool(dr("IsSpecialOrder"))
                            item.AcceptingOrder = CInt(dr("AcceptingOrder"))
                            item.CountReview = dr("CountReview")
                            item.AverageReview = dr("AverageReview")
                            item.IsFlammable = IIf(IsDBNull(dr("IsFlammable")), False, dr("IsFlammable"))
                            item.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))
                            item.PermissionBuyBrand = CBool(dr("PermissionBuyBrand"))
                            item.youSave = item.LowPrice - item.LowSalePrice
                            item.savePercent = FormatCurrency(item.youSave * 100 / item.LowPrice)
                            item.itemIndex = counter
                            c.Add(item)
                        End If
                    End If
                End While
                Core.CloseReader(dr)
                If sortBy <> "on-sale" Then
                    CacheUtils.SetCache(key, c, Utility.ConfigData.TimeCacheData)
                End If

                Return c
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListItems", ex)
            End Try

            Return c
        End Function

        Private Shared Function GetCustomerPriceGroupID(ByVal memberId As Integer) As Integer

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERWISHLISTITEM_DELETE As String = "sp_StoreItem_GetCustomerPriceGroup"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERWISHLISTITEM_DELETE)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetTemplateItemId(ByVal templateId As Integer, ByVal packageId As Integer) As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItem_GettemplateIdAddCart")
                db.AddInParameter(cmd, "TemplateId", DbType.Int32, templateId)
                db.AddInParameter(cmd, "PackageId", DbType.Int32, packageId)
                Return Convert.ToInt32(db.ExecuteScalar(cmd))
            Catch ex As Exception

            End Try
            Return 0
        End Function

        Public Function GetCollections() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select CollectionId from StoreCollectionItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("CollectionId"))
                Conn = ","
            Next
            Return s
        End Function

        Public Function GetSelectedPostingGroups() As String
            Dim dt As DataTable = DB.GetDataTable("SELECT Code FROM StoreItemCustomerPostingGroup WHERE ItemId = " & ItemId)
            Dim s As String = String.Empty
            For Each row As DataRow In dt.Rows
                s &= IIf(s = String.Empty, "", ",") & row("code")
            Next
            Return s
        End Function

        Public Sub RemoveAllPostingGroups()
            DB.ExecuteSQL("delete from storeitemcustomerpostinggroup where itemid = " & ItemId)
        End Sub

        Public Sub InsertPostingGroups(ByVal Values As String)
            Dim aValues As String() = Values.Split(",")
            For Each value As String In aValues
                InsertPostingGroup(value)
            Next
        End Sub

        Private Sub InsertPostingGroup(ByVal value As String)
            DB.ExecuteSQL("insert into storeitemcustomerpostinggroup (itemid, code) values (" & ItemId & "," & DB.Quote(value) & ")")
        End Sub

        Public Sub RemoveAllCollections()
            DB.ExecuteSQL("delete from StoreCollectionItem where ItemId = " & ItemId)
        End Sub

        Public Sub InsertCollections(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertCollection(ids(i))
            Next
        End Sub

        Private Sub InsertCollection(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreCollectionItem (CollectionId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub

        Public Function GetTones() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select ToneId from StoreToneItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("ToneId"))
                Conn = ","
            Next

            Return s
        End Function

        Public Function GetShades() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select ShadeId from StoreShadeItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("ShadeId"))
                Conn = ","
            Next

            Return s
        End Function

        Public Sub RemoveAllTones()
            DB.ExecuteSQL("delete from StoreToneItem where ItemId = " & ItemId)
        End Sub

        Public Sub RemoveAllShades()
            DB.ExecuteSQL("delete from StoreShadeItem where ItemId = " & ItemId)
        End Sub

        Public Sub InsertTones(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertTone(ids(i))
            Next
        End Sub

        Private Sub InsertTone(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreToneItem (ToneId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub

        Public Sub InsertShades(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertShade(ids(i))
            Next
        End Sub

        Private Sub InsertShade(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreShadeItem (ShadeId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub

        Public Sub InsertItemProperties(si As StoreItemRow)
            If si.HandlingFeeForCase + si.HandlingFeeForItem > 0 Then
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_TIP_INSERT As String = "sp_StoreItemProperties_Insert"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TIP_INSERT)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, si.ItemId)
                db.AddInParameter(cmd, "HandlingFeeForItem", DbType.String, si.HandlingFeeForItem)
                db.AddInParameter(cmd, "HandlingFeeForCase", DbType.String, si.HandlingFeeForCase)
                db.ExecuteNonQuery(cmd)
            End If

        End Sub
        Public Sub UpdateItemProperties(si As StoreItemRow)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemProperties_Update"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "HandlingFeeForItem", DbType.Double, HandlingFeeForItem)
                db.AddInParameter(cmd, "HandlingFeeForCase", DbType.Double, HandlingFeeForCase)

                db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "StoreItem.vb > Update", "UpdateItemProperties:" & ItemId & "<br>Exception: " & ex.ToString())
            End Try
        End Sub
        Public Shared Function GetDepartments(ByVal _DB As Database, ByVal ItemId As Integer) As DataSet
            Return _DB.GetDataSet("select sd.* from storedepartment sd inner join storedepartmentitem sdi on sd.departmentid = sdi.departmentid where itemid = " & ItemId)
        End Function

        Public Sub CopyFromNavision(ByVal r As NavisionItemRow, ByVal sLowerCase As String, ByVal sUpperCase As String)
            If r.Description.Length = 0 Then Exit Sub

            Dim s As String = String.Empty
            Category = Trim(r.Item_Category_Code)
            SKU = Trim(r.Item_No)
            ItemType = "Item"
            IsFeatured = 0
            Price = r.Unit_Price
            IsActive = (Price > 0 And Not ((r.Blocked) = "Y")) And (r.BlockedWeb = "N")
            IsNew = False
            IsTaxFree = False
            IsFreeShipping = False
            LastImport = Now()
            QtyOnHand = r.Inventory
            Status = "IN"
            TaxGroupCode = r.Tax_Group_Code
            IsHazMat = r.Hazmat = "Y"

            If ItemId = Nothing Then
                ItemName = Trim(ChangeCase(DB, r.Description, sLowerCase, sUpperCase))
                If Trim(ItemName) = Nothing Then Exit Sub
                Weight = Trim(r.Net_Weight)
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Shared Sub UpdateAllItemDepartments(ByVal DB As Database)
            DB.ExecuteSQL("insert into storedepartmentitem (itemid, departmentid) select itemid, departmentid from storeitemcategorydepartment nicd inner join storeitemcategory nic on nicd.categoryid = nic.id inner join storeitem si on nic.category = si.category where itemid not in (select sdi.itemid from storedepartmentitem sdi where sdi.itemid = si.itemid)")
        End Sub

        Public Function GetPromotions() As PromotionCollection
            Return PromotionRow.GetRows(DB, ItemId)
        End Function

        Public Function GetPromotion() As PromotionRow
            Return PromotionRow.GetRow(DB, ItemId, True)
        End Function

        Public Function GetPromotion(ByVal _Database As Database) As PromotionRow
            Return PromotionRow.GetRow(_Database, ItemId, True)
        End Function

        Public Shared Function GetRelatedItems(ByVal DB As Database, ByVal ItemId As Integer, ByVal ItemGroupId As Integer) As DataSet
            Dim SQL As String = "select si.itemname, si.shortdesc, si.shortviet, si.shortfrench," &
            " si.shortspanish, si.itemid, si.image, si.brandid, sb.brandname, " &
            "sdi.departmentid from relateditem ri inner join storeitem si on " &
            "ri.parentid = si.itemid left outer join storebrand sb " &
            "on si.brandid = sb.brandid left join StoreDepartmentItem sdi ON " &
            "sdi.ItemId = si.ItemId where si.isactive=1 " & IIf(ItemGroupId <> Nothing,
            " and ItemGroupId = " & ItemGroupId, " and si.itemid = " & ItemId)
            Return DB.GetDataSet(SQL)
        End Function
        Public Shared Function GetItemByVideoId(ByVal _Database As Database, ByVal VideoId As Integer, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection
            Dim ss As New StoreItemCollection
            Dim keyData As String = String.Format(ItemRelatedVideoRow.cachePrefixKey & "GetItemByVideoId_{0}_{1}_{2}_{3}_{4}_{5}_{6}", VideoId, filter.OrderId, filter.MemberId, filter.SortBy, filter.SortOrder, filter.pg, filter.MaxPerPage)
            Dim keyTotal As String = ItemRelatedVideoRow.cachePrefixKey & "GetItemByVideoId_Total"
            ss = CType(CacheUtils.GetCache(keyData), StoreItemCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New StoreItemCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_ItemRelatedVideo_ListItemByVideoId"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, filter.OrderId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, filter.MemberId))
                cmd.Parameters.Add(_Database.InParam("OrderBy", SqlDbType.VarChar, 0, filter.SortBy))
                cmd.Parameters.Add(_Database.InParam("OrderDirection", SqlDbType.VarChar, 0, filter.SortOrder))
                cmd.Parameters.Add(_Database.InParam("CurrentPage", SqlDbType.Int, 0, filter.pg))
                cmd.Parameters.Add(_Database.InParam("PageSize", SqlDbType.Int, 0, filter.MaxPerPage))
                cmd.Parameters.Add(_Database.OutParam("TotalRecords", SqlDbType.Int, 0))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataRelatedFromReader(dr, _Database))
                End While
                Core.CloseReader(dr)
                If ss.Count > 0 Then
                    totalRow = CInt(cmd.Parameters("TotalRecords").Value)
                End If
                CacheUtils.SetCache(keyData, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Private Shared Function GetDataRelatedFromReader(ByVal dr As SqlDataReader, ByVal db As Database) As StoreItemRow
            Dim item As New StoreItemRow(db)
            item.LowPrice = dr("LowPrice")
            item.HighPrice = dr("HighPrice")
            item.LowSalePrice = IIf(IsDBNull(dr("LowSalePrice")), Nothing, dr("LowSalePrice"))
            'item.HighSalePrice = IIf(IsDBNull(dr("HighSalePrice")), Nothing, dr("HighSalePrice"))
            item.PriceDesc = IIf(IsDBNull(dr("PriceDesc")), Nothing, dr("PriceDesc"))
            item.ItemId = dr("ItemId")
            item.IsNew = CBool(dr("isnew"))
            Try
                If dr("IsLoginViewPrice") Is Convert.DBNull Then
                    item.IsLoginViewPrice = False
                Else
                    item.IsLoginViewPrice = CBool(dr("IsLoginViewPrice"))
                End If
            Catch ex As Exception
                item.IsLoginViewPrice = False
            End Try
            If Not IsDBNull(dr("newuntil")) Then item.NewUntil = dr("newuntil") Else item.NewUntil = Nothing
            item.IsBestSeller = CBool(dr("IsBestSeller"))
            item.IsHot = CBool(dr("IsHot"))
            item.IsFreeShipping = CBool(dr("IsFreeShipping"))
            item.IsFreeSample = CBool(dr("IsFreeSample"))
            item.IsFreeGift = CInt(dr("IsFreeGift"))
            item.IsSpecialOrder = CBool(dr("IsSpecialOrder"))
            item.AcceptingOrder = CInt(dr("AcceptingOrder"))
            item.ItemName = dr("ItemName")
            item.ItemName2 = dr("ItemName")
            item.IsFlammable = dr("IsFlammable")
            If IsDBNull(dr.Item("URLCode")) Then
                item.URLCode = Nothing
            Else
                item.URLCode = dr("URLCode")
            End If
            item.QtyOnHand = CInt(dr("QtyOnHand"))
            item.IsVariance = CBool(dr("IsVariance"))
            If Not IsDBNull(dr("choicename")) Then
                item.ItemName &= " - " & dr("choicename")

                If Not IsDBNull(dr("choicename")) AndAlso Not item.ItemName2.Contains(dr("choicename")) Then
                    item.ItemName2 &= " - " & dr("choicename")
                End If

            End If

            If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso dr("choicename").ToString().Trim().Replace(" ", "") <> dr("PriceDesc").ToString().Trim().Replace(" ", "") Then
                item.ItemName2 &= " - " & dr("PriceDesc")
            End If

            item.Price = dr("Price")
            item.SKU = IIf(IsDBNull(dr("sku")), Nothing, dr("sku"))
            If IsDBNull(dr("itemgroupid")) Then item.ItemGroupId = Nothing Else item.ItemGroupId = dr("itemgroupid")
            item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
            item.ShortDesc = IIf(IsDBNull(dr("Shortdesc")), Nothing, dr("Shortdesc"))
            'item.MixMatchId = IIf(IsDBNull(dr("mixmatchid")), Nothing, dr("mixmatchid"))
            item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
            item.MixMatchDescription = IIf(IsDBNull(dr("MixMatchDescription")), Nothing, dr("MixMatchDescription"))
            Try
                item.IsInCart = IIf(IsDBNull(dr("isincart")), False, dr("isincart"))
            Catch
                item.IsInCart = False
            End Try
            Try
                item.CasePrice = IIf(IsDBNull(dr("CasePrice")), 0, dr("CasePrice"))
            Catch
                item.CasePrice = 0
            End Try
            Try
                item.CaseQty = IIf(IsDBNull(dr("CaseQty")), 0, dr("CaseQty"))
            Catch
                item.CaseQty = 0
            End Try
            item.CountReview = dr("CountReview")
            item.AverageReview = dr("AverageReview")
            item.IsFlammable = IIf(IsDBNull(dr("IsFlammable")), False, dr("IsFlammable"))
            item.youSave = item.LowPrice - item.LowSalePrice
            item.savePercent = FormatCurrency(item.youSave * 100 / item.LowPrice)
            If Not dr("RowNum") Is Nothing Then
                item.itemIndex = Convert.ToInt32(dr("RowNum"))
            Else
                item.itemIndex = 0
            End If

            Return item
        End Function
        Public Shared Function GetRelatedItemsColection(ByVal DB1 As Database, ByVal ItemId As Integer, ByVal ItemGroupId As Integer, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection
            Dim c As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim TopRecords As Integer = filter.MaxPerPage * filter.pg
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetRelatedItemsV2"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)
                db.AddInParameter(cmd, "PageIndex", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, ItemGroupId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, filter.OrderId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, filter.MemberId)
                db.AddInParameter(cmd, "DateString", DbType.String, Now.ToShortDateString)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim item As StoreItemRow = GetDataRelatedFromReader(dr, DB1)
                    c.Add(item)
                    If (totalRow < 1) Then
                        totalRow = dr("TotalRow")
                    End If
                End While
                If (c.Count > 0) Then
                    c(c.Count - 1).Final = "1"
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetRelatedItemsColection", ex)
            End Try
            For Each row As StoreItemRow In c
                Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Initialize(DB1, row, customerPriceGroup)
                row.ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(DB1, row, False, 1, 0, HttpContext.Current.Session("MemberId"), True) & "</div>"
            Next
            Return c
            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetItemRelatedVideoColection(ByVal db As Database, ByVal VideoId As Integer, ByVal ItemGroupId As Integer, ByVal filter As DepartmentFilterFields, ByRef totalRow As Integer) As StoreItemCollection
            Dim result As New StoreItemCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim TopRecords As Integer = filter.MaxPerPage * filter.pg
                Dim sp As String = "sp_StoreItem_GetItemRelatedVideo"
                Dim cmd As SqlCommand = db.CreateCommand(sp)
                cmd.Parameters.Add(db.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(db.InParam("ItemGroupId", SqlDbType.Int, 0, ItemGroupId))
                cmd.Parameters.Add(db.InParam("PageIndex", SqlDbType.Int, 0, filter.pg))
                cmd.Parameters.Add(db.InParam("PageSize", SqlDbType.Int, 0, filter.MaxPerPage))
                cmd.Parameters.Add(db.InParam("OrderId", SqlDbType.Int, 0, filter.OrderId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    Dim item As New StoreItemRow()
                    item.IsSpecialOrder = Convert.ToBoolean(dr.Item("IsSpecialOrder"))
                    item.AcceptingOrder = Convert.ToInt32(dr.Item("AcceptingOrder"))
                    item.Status = Convert.ToString(dr.Item("Status"))
                    If IsDBNull(dr.Item("BODate")) Then
                        item.BODate = Nothing
                    Else
                        item.BODate = Convert.ToDateTime(dr.Item("BODate"))
                    End If
                    If dr.Item("QtyOnHand") Is Convert.DBNull Then
                        item.QtyOnHand = Nothing
                    Else
                        item.QtyOnHand = Convert.ToInt32(dr.Item("QtyOnHand"))
                    End If
                    If dr.Item("LowStockThreshold") Is Convert.DBNull Then
                        item.LowStockThreshold = Nothing
                    Else
                        item.LowStockThreshold = Convert.ToInt32(dr.Item("LowStockThreshold"))
                    End If
                    If dr.Item("LowStockMsg") Is Convert.DBNull Then
                        item.LowStockMsg = Nothing
                    Else
                        item.LowStockMsg = Convert.ToString(dr.Item("LowStockMsg"))
                    End If
                    item.ItemId = dr("ItemId")
                    item.SKU = dr("Sku")
                    item.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    If IsDBNull(dr.Item("URLCode")) Then
                        item.URLCode = Nothing
                    Else
                        item.URLCode = Convert.ToString(dr.Item("URLCode"))
                    End If
                    item.ItemName = dr("ItemName")
                    item.IsInCart = dr("isincart")
                    item.BrandId = IIf(IsDBNull(dr("brandid")), Nothing, dr("brandid"))
                    Try
                        If dr.Item("IsFlammable") Is Convert.DBNull Then
                            item.IsFlammable = False
                        Else
                            item.IsFlammable = CBool(dr.Item("IsFlammable"))
                        End If
                    Catch ex As Exception
                        item.IsFlammable = False
                    End Try
                    If dr.Item("Price") Is Convert.DBNull Then
                        item.Price = Nothing
                    Else
                        item.Price = Convert.ToDouble(dr.Item("Price"))
                    End If
                    If dr.Item("PriceDesc") Is Convert.DBNull Then
                        item.PriceDesc = Nothing
                    Else
                        item.PriceDesc = Convert.ToString(dr.Item("PriceDesc"))
                    End If
                    If dr.Item("RewardPoints") Is Convert.DBNull Then
                        item.RewardPoints = Nothing

                    Else
                        item.RewardPoints = Convert.ToInt32(dr.Item("RewardPoints"))
                    End If
                    Dim IsRewardPoints As Int32
                    If dr.Item("IsRewardPoints") Is Convert.DBNull Then
                        IsRewardPoints = 0
                    Else
                        IsRewardPoints = Convert.ToInt32(dr.Item("IsRewardPoints"))
                    End If
                    If (IsRewardPoints = 1) Then
                        item.IsRewardPoints = True
                    Else
                        item.IsRewardPoints = False
                    End If
                    If dr.Item("lowprice") Is Convert.DBNull Then
                        item.LowPrice = Nothing
                    Else
                        item.LowPrice = Convert.ToDouble(dr.Item("lowprice"))
                    End If
                    If dr.Item("lowsaleprice") Is Convert.DBNull Then
                        item.LowSalePrice = Nothing
                    Else
                        item.LowSalePrice = Convert.ToDouble(dr.Item("lowsaleprice"))
                    End If
                    If dr.Item("ShortDesc") Is Convert.DBNull Then
                        item.ShortDesc = Nothing
                    Else
                        item.ShortDesc = Convert.ToString(dr.Item("ShortDesc"))
                    End If

                    If HttpContext.Current.Session("MemberId") <> Nothing Then
                        item.ShowPrice = "<div>" & BaseShoppingCart.DisplayListPricing(db, item, False, 1, 0, HttpContext.Current.Session("MemberId"), True) & "</div>"
                    End If

                    item.ShowInventory = BaseShoppingCart.Inventory(dr.Item("Status"), IIf(IsDBNull(dr.Item("BODate")), Nothing, dr.Item("BODate")), dr.Item("AcceptingOrder"), dr.Item("QtyOnHand"), dr.Item("IsSpecialOrder"), IIf(IsDBNull(dr.Item("LowStockThreshold")), Nothing, dr.Item("LowStockThreshold")), IIf(IsDBNull(dr.Item("LowStockMsg")), Nothing, dr.Item("LowStockMsg")))
                    result.Add(item)
                    If (totalRow < 1) Then
                        totalRow = dr("TotalRow")
                    End If
                End While
                If (result.Count > 0) Then
                    result(result.Count - 1).Final = "1"
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetRelatedItemsColection", ex)
            End Try
            For Each row As StoreItemRow In result
                Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Initialize(db, row, customerPriceGroup)
            Next
            Return result
        End Function

        Public Shared Function GetRelatedItems(ByVal DB1 As Database, ByVal ItemId As Integer, ByVal ItemGroupId As Integer, ByVal TopRecords As Integer) As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetRelatedItems"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "TopRecords", DbType.Int32, TopRecords)
            db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, ItemGroupId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)


            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Shared Sub getInfoSearchItem(listItemIds As String, memberID As Integer, orderID As Integer, customerPriceGroupId As Integer, ByRef listItem As StoreItemCollection)
            Dim dicItem As Dictionary(Of Integer, DataRow) = New Dictionary(Of Integer, DataRow)()
            Dim rd As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItem_getInfoSearchItem")
                db.AddInParameter(cmd, "ListItemIds", DbType.String, listItemIds)
                db.AddInParameter(cmd, "MemberID", DbType.Int32, memberID)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, orderID)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, customerPriceGroupId)

                rd = db.ExecuteReader(cmd)
                If rd.HasRows Then
                    While rd.Read
                        Dim item = listItem.OfType(Of StoreItemRow).First(Function(i) i.ItemId.ToString() = rd("ItemId").ToString())
                        item.MixMatchDescription = rd("MixMatchDescription").ToString()
                        item.PermissionBuyBrand = rd("PermissionBuyBrand").ToString()
                    End While
                End If

            Catch ex As Exception
                If rd IsNot Nothing Then
                    rd.Close()
                End If
                SendMailLog("getInfoSearchItem", ex)
            End Try
        End Sub

        Public Function GetBaseColorDataSet() As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 28, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetBaseColorList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Function GetBaseColors() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select * from StoreBaseColorItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("BaseColorId"))
                Conn = ","
            Next

            Return s
        End Function

        Public Sub RemoveAllBaseColors()
            DB.ExecuteSQL("delete from StoreBaseColorItem where ItemId = " & ItemId)
        End Sub

        Public Sub InsertBaseColors(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertBaseColor(ids(i))
            Next
        End Sub

        Private Sub InsertBaseColor(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreBaseColorItem (BaseColorId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub

        Public Function GetCusionColorDataSet() As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 28, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetCusionColorList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Function GetCusionColors() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select * from StoreCusionColorItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("CusionColorId"))
                Conn = ","
            Next

            Return s
        End Function

        Public Sub RemoveAllCusionColors()
            DB.ExecuteSQL("delete from StoreCusionColorItem where ItemId = " & ItemId)
        End Sub

        Public Sub InsertCusionColors(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertCusionColor(ids(i))
            Next
        End Sub

        Private Sub InsertCusionColor(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreCusionColorItem (CusionColorId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub

        Public Function GetLaminateTrimDataSet() As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 28, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEM_GETLIST As String = "sp_StoreItem_GetLaminateTrimList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, ItemId)

            Return db.ExecuteDataSet(cmd)

            '------------------------------------------------------------------------
        End Function

        Public Function GetLaminateTrims() As String
            Dim s, Conn, SQL As String
            Dim dv As DataView, drv As DataRowView

            SQL = "select * from StoreLaminateTrimItem where ItemId = " & ItemId

            s = String.Empty
            Conn = ""
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                s &= Conn & CStr(drv("LaminateTrimId"))
                Conn = ","
            Next

            Return s
        End Function

        Public Sub RemoveAllLaminateTrims()
            DB.ExecuteSQL("delete from StoreLaminateTrimItem where ItemId = " & ItemId)
        End Sub

        Public Sub InsertLaminateTrims(ByVal sids As String)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertLaminateTrim(ids(i))
            Next
        End Sub

        Private Sub InsertLaminateTrim(ByVal c As Integer)
            DB.ExecuteSQL("insert into StoreLaminateTrimItem (LaminateTrimId, ItemId) values (" & c & "," & ItemId & ")")
        End Sub



        Public Sub DeleteFromAllStoreItemGroupOptions()
            DB.ExecuteSQL("delete from StoreItemGroupOptionRel where ItemGroupId = " & ItemId)
        End Sub

        Public Sub InsertToStoreItemGroupOptions(ByVal Elements As String)
            If Elements = String.Empty Then Exit Sub

            Dim aElements As String() = Elements.Split(",")
            For Each Element As String In aElements
                InsertToStoreItemGroupOption(Element)
            Next
        End Sub

        Public Sub InsertToStoreItemGroupOption(ByVal OptionId As Integer)
            Dim SQL As String = "insert into StoreItemGroupOptionRel (ItemGroupId, OptionId) values (" & ItemId & "," & OptionId & ")"
            DB.ExecuteSQL(SQL)
        End Sub
        Public Shared Function GetLongDescByLanguage(ByVal DB As Database, ByVal lang As String, ByVal itemId As String) As String
            Dim column As String = "LongDesc"
            Select Case lang
                Case LanguageCode.Vietnamese
                    column = "LongViet"
                Case LanguageCode.French
                    column = "LongFrench"
                Case LanguageCode.Spanish
                    column = "LongSpanish"
                Case LanguageCode.SouthKorea
                    column = "LongKorea"
                Case Else
                    column = "LongDesc"
            End Select
            Dim dr As SqlDataReader = Nothing
            Dim Result As String = String.Empty
            Try
                Dim sql As String = String.Format("Select {0}, isnull({2},'') from StoreItem where ItemId={1}", column, itemId, "LongDesc")
                dr = DB.GetReader(sql)
                If dr.Read() Then
                    If dr.GetValue(0) Is Nothing Or IsDBNull(dr.GetValue(0)) Then
                        Result = dr.GetValue(1)
                    Else
                        Result = dr.GetString(0)
                    End If
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetLongDescByLanguage", ex)

            End Try
            Return Result
        End Function
        Public Shared Sub InsertToStoreItemGroupChoiceRel(ByVal DB As Database, ByVal ItemId As Integer, ByVal ChoiceId As Integer, ByVal OptionId As Integer)
            Dim SQL As String = "insert into StoreItemGroupChoiceRel (ItemId, ChoiceId, OptionId) values (" & ItemId & "," & ChoiceId & "," & OptionId & ")"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub DeleteAllStoreItemGroupChoiceRel(ByVal DB As Database, ByVal ItemId As Integer)
            Dim SQL As String = "delete from StoreItemGroupChoiceRel where itemid = " & ItemId
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertToStoreItemGroupRel(ByVal Id As Integer)
            Dim SQL As String = "if not exists (select top 1 itemid from storeitemgrouprel where itemgroupid = " & ItemId & " and itemid = " & Id & ") begin insert into StoreItemGroupRel (ItemGroupId, ItemId) values (" & ItemId & "," & Id & ") end"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub DeleteStoreItemGroupRel(ByVal Id As Integer)
            Dim SQL As String = "delete from storeitemgrouprel where itemgroupid = " & ItemId & " and itemid = " & Id
            DB.ExecuteSQL(SQL)
        End Sub

        Public Function GetStoreItemGroupOptions() As DataSet
            Dim SQL As String = "select o.* from storeitemgroupoptionrel r inner join storeitemgroupoption o on r.optionid = o.optionid where itemgroupid = " & ItemId
            Return DB.GetDataSet(SQL)
        End Function
    End Class

    Public Class DepartmentFilterFields
        Public BrandId As Integer = Nothing
        Public SalesCategoryId As Integer = Nothing
        Public CollectionId As Integer = Nothing
        Public ToneId As Integer = Nothing
        Public ShadeId As Integer = Nothing
        Public DepartmentId As Integer = Nothing
        Public DepartmentTabId As Integer = Nothing
        Public ShopSaveId As Integer = Nothing
        Public SortBy As String = Nothing
        Public SortOrder As String = Nothing
        Public IsFeatured As Boolean = False
        Public IsNew As Boolean = False
        Public IsHot As Boolean = False
        Public IsOnSale As Boolean = False
        Public HasPromotion As Boolean = False
        Public PriceRange As String = Nothing
        Public PriceHigh As String = Nothing
        Public PromotionId As Integer = Nothing
        Public MemberId As Integer = Nothing
        Public GetItems As Boolean = False
        Public pg As Integer = Nothing
        Public MaxPerPage As Integer
        Public IsItemIdOnly As Boolean = False
        Public ItemId As Integer = Nothing
        Public Field1Name As String = Nothing
        Public Field1Value As Object = Nothing
        Public Field2Name As String = Nothing
        Public Field2Value As Object = Nothing
        Public All As Boolean = False
        Public Keyword As String = String.Empty
        Public OrderId As Integer = Nothing
        Public Sale24Hour As Boolean = False
        Public SaleBuy1Get1 As Boolean = False
        Public Feature As String = Nothing
        Public LoggedInPostingGroup As String = Nothing
        Public PageParams As String = Nothing
        Public ListBrandId As String = Nothing
        Public IsSearchKeyWord As Boolean = False
        Public BrandCode As String = Nothing
        Public MinPrice As Double = 0
        Public MaxPrice As Double = 0
        Public MinRating As Double = 0
        Public MaxRating As Double = 0
        Public RatingRange As String = Nothing
        Public ItemSku As String = Nothing
    End Class

    Public Class DepartmentFilterField
        Public DB As Database
        Public Text As String
        Public SelectedValue As String
        Public Filter As DepartmentFilterFields

        Public Sub New(ByVal Db As Database, ByVal Text As String, ByVal Selectedvalue As String, ByVal filter As DepartmentFilterFields)
            Me.DB = Db
            Me.Text = Text
            Me.SelectedValue = Selectedvalue
            Me.Filter = filter
        End Sub

        Public ReadOnly Property DataSource() As ArrayList
            Get
                Dim al As ArrayList = Nothing
                Select Case Text
                    Case "Select a Brand:"
                        'al = StoreItemRow.GetBrands(DB, Filter)
                    Case "Select Collection:"
                        'al = StoreCollectionRow.GetAllCollections(DB, Filter)
                    Case "Select Tones:"
                        'al = StoreTones.GetAllTones(DB, Filter)
                End Select
                Return al
            End Get
        End Property
    End Class

    Public MustInherit Class StoreItemRowBase
        Private m_DB As Database
        Private m_ItemId As Integer = Nothing
        Private m_MixMatchId As Integer = Nothing
        Private m_IsInCart As Boolean = Nothing
        Private m_IsInCollection As Boolean = Nothing
        Private m_ItemGroupId As Integer = Nothing
        Private m_Category As String = Nothing
        Private m_ItemType As String = Nothing
        Private m_ItemName As String = Nothing
        Private m_ItemNameNew As String = Nothing
        Private m_ItemName2 As String = Nothing
        Private m_SKU As String = Nothing
        Private m_Measurement As String = Nothing
        Private m_Price As Double = Nothing
        Private m_SalePrice As Double = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsNew As Boolean = Nothing
        Private m_IsBestSeller As Boolean = Nothing
        Private m_NewUntil As DateTime = Nothing
        Private m_IsTaxFree As Boolean = Nothing
        Private m_ShipmentDate As DateTime = Nothing
        Private m_PriceDesc As String = Nothing
        Private m_ChoiceName As String = Nothing
        Private m_Image As String = Nothing
        Private m_ImageAltTag As String = Nothing
        Private m_DeliveryTime As String = Nothing
        Private m_CarrierType As String = Nothing
        Private m_Status As String = Nothing
        Private m_InvMsgId As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_QtyOnHand As Integer = Nothing
        Private m_InventoryStockNotification As Integer = Nothing
        Private m_LowStockMsg As String = Nothing
        Private m_LowStockThreshold As Integer = Nothing
        Private m_QtyReserved As Integer = Nothing
        Private m_LastUpdated As DateTime = Nothing
        Private m_ShortDesc As String = Nothing
        Private m_LongDesc As String = Nothing
        Private m_ShortViet As String = Nothing
        Private m_LongViet As String = Nothing
        Private m_ShortFrench As String = Nothing
        Private m_LongFrench As String = Nothing
        Private m_ShortSpanish As String = Nothing
        Private m_LongSpanish As String = Nothing
        Private m_ShortKorea As String = Nothing
        Private m_LongKorea As String = Nothing
        Private m_AdditionalInfo As String = Nothing
        Private m_Specifications As String = Nothing
        Private m_ShippingInfo As String = Nothing
        Private m_HelpfulTips As String = Nothing
        Private m_MSDS As String = Nothing
        Private m_IsFeatured As Boolean = Nothing
        Private m_IsFreeShipping As Boolean = Nothing
        Private m_IsOnSale As Boolean = Nothing
        Private m_IsCollection As Boolean = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_IsOversize As Boolean = Nothing
        Private m_IsHazMat As Boolean = Nothing
        Private m_BrandId As Integer = Nothing
        Private m_Prefix As String = Nothing
        Private m_LastImport As String = Nothing
        Private m_Weight As Double = Nothing
        Private m_LastExport As String = Nothing
        Private m_LowPrice As Double = Nothing
        Private m_HighPrice As Double = Nothing
        Private m_LowSalePrice As Double = Nothing
        Private m_HighSalePrice As Double = Nothing
        Private m_BODate As DateTime = Nothing
        Private m_MaximumQuantity As Integer = Nothing
        Private m_IsRushDelivery As Boolean = False
        Private m_RushDeliveryCharge As Double = Nothing
        Private m_LiftGateCharge As Double = Nothing
        Private m_ScheduleDeliveryCharge As Double = Nothing
        Private m_IsHot As Boolean = Nothing
        Private m_IsSpecialOrder As Boolean = Nothing
        Private m_AcceptingOrder As Integer = Nothing
        Private m_TaxGroupCode As String = String.Empty
        Private m_PromotionId As Integer = Nothing
        Private m_IsFreeSample As Boolean = Nothing
        Private m_IsFreeGift As Integer = Nothing
        Private m_Final As String = Nothing
        Private m_IsVariance As Boolean = False
        Private m_PermissionBuyBrand As Boolean = True
        Private m_IsLoginViewPrice As Boolean = True
        Private m_FreeSampleArrange As Integer = Nothing
        Private m_MixMatchDescription As String = Nothing
        Private m_URLCode As String = Nothing
        Private m_IsFlatFee As Boolean = False
        Private m_FeeShipOversize As Double = Nothing
        Private m_IsEbayAllow As Boolean = True
        Private m_IsFlammable As Boolean = True
        Public Shared cachePrefixKey As String = "StoreItem_"
        Private m_IsEbay As Boolean = True
        Private m_EbayShippingType As String = Nothing
        Private m_IsRewardPoints As Boolean = False
        Private m_RewardPoints As Integer = Nothing
        Private m_ArrangeRewardPoints As Integer = Nothing
        Private m_CountReview As Integer = Nothing
        Private m_AverageReview As Double = Nothing
        Private m_ShowPrice As String = String.Empty
        Private m_ShowInventory As String = String.Empty
        Private m_OutsideUSPageTitle As String = Nothing
        Private m_OutsideUSMetaDescription As String = Nothing
        Private m_LastDepartmentName As String = Nothing
        Private m_EbayPrice As Double = Nothing
        Private m_ItemSku As String = Nothing
        ''use in Admin
        Private m_DepartmentName As String = Nothing
        Private m_BrandName As String = Nothing
        Private m_PromotionCode As String = Nothing
        Private m_IsSellItemInEbay As Boolean = False
        Private m_EbayId As Integer = Nothing
        Private m_IsSellInAmazon As Boolean = False
        Private m_CountSalePrice As Integer = Nothing
        Private m_CountCaseSalePrice As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_CaseQty As Integer = Nothing
        Private m_CasePrice As Double = Nothing

        Private m_Policy As String = String.Empty
        Private m_IsFirstClassPackage As Boolean = Nothing

        Private m_IsNewTrue As Boolean
        ''' <summary>
        ''' Manual handling fee for item and case
        ''' </summary>
        Private m_ItemProId As Integer = Nothing
        Private m_HandlingFeeForItem As Double = Nothing
        Private m_HandlingFeeForCase As Double = Nothing
        ''' <summary>
        ''' End field
        ''' </summary>
#Region "Property for Admin Log"
        Private m_ListDepartmentId As String = String.Empty
        Private m_ListPostingGroupCode As String = String.Empty
        Private m_ListCollectionId As String = String.Empty
        Private m_ListToneId As String = String.Empty
        Private m_ListShapeId As String = String.Empty
        Private m_ListBaseColorId As String = String.Empty
        Private m_ListCusionColorId As String = String.Empty
        Private m_ListLaminateColorId As String = String.Empty
#End Region

        Public Property Policy() As String
            Get
                Return m_Policy
            End Get
            Set(ByVal value As String)
                m_Policy = value
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

        Public Property BrandName() As String
            Get
                Return m_BrandName
            End Get
            Set(ByVal Value As String)
                m_BrandName = Value
            End Set
        End Property

        Public Property PromotionCode() As String
            Get
                Return m_PromotionCode
            End Get
            Set(ByVal Value As String)
                m_PromotionCode = Value
            End Set
        End Property

        Public Property IsNewTrue() As Boolean
            Get
                Return m_IsNewTrue
            End Get
            Set(ByVal value As Boolean)
                m_IsNewTrue = value
            End Set
        End Property

        Public Property IsSellItemInEbay() As Boolean
            Get
                Return m_IsSellItemInEbay
            End Get
            Set(ByVal Value As Boolean)
                m_IsSellItemInEbay = Value
            End Set
        End Property

        Public Property EbayId() As Integer
            Get
                Return m_EbayId
            End Get
            Set(ByVal Value As Integer)
                m_EbayId = Value
            End Set
        End Property

        Public Property IsSellInAmazon() As Boolean
            Get
                Return m_IsSellInAmazon
            End Get
            Set(ByVal Value As Boolean)
                m_IsSellInAmazon = Value
            End Set
        End Property

        Public Property CountSalePrice() As Integer
            Get
                Return m_CountSalePrice
            End Get
            Set(ByVal Value As Integer)
                m_CountSalePrice = Value
            End Set
        End Property
        Public Property CountCaseSalePrice() As Integer
            Get
                Return m_CountCaseSalePrice
            End Get
            Set(ByVal Value As Integer)
                m_CountCaseSalePrice = Value
            End Set
        End Property
        Public Property LastDepartmentName() As String
            Get
                Return m_LastDepartmentName
            End Get
            Set(ByVal Value As String)
                m_LastDepartmentName = Value
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

        Public Property OutsideUSMetaDescription() As String
            Get
                Return m_OutsideUSMetaDescription
            End Get
            Set(ByVal Value As String)
                m_OutsideUSMetaDescription = Value
            End Set
        End Property


        Public Property ListDepartmentId() As String
            Get
                Return m_ListDepartmentId
            End Get
            Set(ByVal value As String)
                m_ListDepartmentId = value
            End Set
        End Property
        Public Property ListPostingGroupCode() As String
            Get
                Return m_ListPostingGroupCode
            End Get
            Set(ByVal value As String)
                m_ListPostingGroupCode = value
            End Set
        End Property
        Public Property ListCollectionId() As String
            Get
                Return m_ListCollectionId
            End Get
            Set(ByVal value As String)
                m_ListCollectionId = value
            End Set
        End Property
        Public Property ListToneId() As String
            Get
                Return m_ListToneId
            End Get
            Set(ByVal value As String)
                m_ListToneId = value
            End Set
        End Property
        Public Property ListShapeId() As String
            Get
                Return m_ListShapeId
            End Get
            Set(ByVal value As String)
                m_ListShapeId = value
            End Set
        End Property
        Public Property ListBaseColorId() As String
            Get
                Return m_ListBaseColorId
            End Get
            Set(ByVal value As String)
                m_ListBaseColorId = value
            End Set
        End Property
        Public Property ListCusionColorId() As String
            Get
                Return m_ListCusionColorId
            End Get
            Set(ByVal value As String)
                m_ListCusionColorId = value
            End Set
        End Property
        Public Property ListLaminateColorId() As String
            Get
                Return m_ListLaminateColorId
            End Get
            Set(ByVal value As String)
                m_ListLaminateColorId = value
            End Set
        End Property
        Public Property FreeSampleArrange() As Integer
            Get
                Return m_FreeSampleArrange
            End Get
            Set(ByVal value As Integer)
                m_FreeSampleArrange = value
            End Set
        End Property


        Public Property IsVariance() As Boolean
            Get
                Return m_IsVariance
            End Get
            Set(ByVal value As Boolean)
                m_IsVariance = value
            End Set
        End Property

        Public Property PermissionBuyBrand() As Boolean
            Get
                Return m_PermissionBuyBrand
            End Get
            Set(ByVal value As Boolean)
                m_PermissionBuyBrand = value
            End Set
        End Property

        Public Property IsLoginViewPrice() As Boolean
            Get
                Return m_IsLoginViewPrice
            End Get
            Set(ByVal value As Boolean)
                m_IsLoginViewPrice = value
            End Set
        End Property

        Public Property TaxGroupCode() As String
            Get
                Return m_TaxGroupCode
            End Get
            Set(ByVal value As String)
                m_TaxGroupCode = value
            End Set
        End Property

        Public Property IsSpecialOrder() As Boolean
            Get
                Return m_IsSpecialOrder
            End Get
            Set(ByVal value As Boolean)
                m_IsSpecialOrder = value
            End Set
        End Property

        Public Property AcceptingOrder() As Integer
            Get
                Return m_AcceptingOrder
            End Get
            Set(ByVal value As Integer)
                m_AcceptingOrder = value
            End Set
        End Property

        Public Property IsHot() As Boolean
            Get
                Return m_IsHot
            End Get
            Set(ByVal value As Boolean)
                m_IsHot = value
            End Set
        End Property

        Public Property MaximumQuantity() As Integer
            Get
                Return m_MaximumQuantity
            End Get
            Set(ByVal value As Integer)
                m_MaximumQuantity = value
            End Set
        End Property

        Public Property HighPrice() As Double
            Get
                Return m_HighPrice
            End Get
            Set(ByVal value As Double)
                m_HighPrice = value
            End Set
        End Property

        Public Property LowPrice() As Double
            Get
                Return m_LowPrice
            End Get
            Set(ByVal value As Double)
                m_LowPrice = value
            End Set
        End Property

        Public Property HighSalePrice() As Double
            Get
                Return m_HighSalePrice
            End Get
            Set(ByVal value As Double)
                m_HighSalePrice = value
            End Set
        End Property

        Public Property LowSalePrice() As Double
            Get
                Return m_LowSalePrice
            End Get
            Set(ByVal value As Double)
                m_LowSalePrice = value
            End Set
        End Property

        Public Property Weight() As Double
            Get
                Return m_Weight
            End Get
            Set(ByVal value As Double)
                m_Weight = value
            End Set
        End Property

        Public Property BrandId() As Integer
            Get
                Return m_BrandId
            End Get
            Set(ByVal Value As Integer)
                m_BrandId = Value
            End Set
        End Property

        Public Property AdditionalInfo() As String
            Get
                Return m_AdditionalInfo
            End Get
            Set(ByVal Value As String)
                m_AdditionalInfo = Value
            End Set
        End Property

        Public Property Specifications() As String
            Get
                Return m_Specifications
            End Get
            Set(ByVal Value As String)
                m_Specifications = Value
            End Set
        End Property

        Public Property ShippingInfo() As String
            Get
                Return m_ShippingInfo
            End Get
            Set(ByVal Value As String)
                m_ShippingInfo = Value
            End Set
        End Property

        Public Property HelpfulTips() As String
            Get
                Return m_HelpfulTips
            End Get
            Set(ByVal Value As String)
                m_HelpfulTips = Value
            End Set
        End Property

        Public Property ItemName2() As String
            Get
                Return m_ItemName2
            End Get
            Set(ByVal Value As String)
                m_ItemName2 = Value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return m_Category
            End Get
            Set(ByVal Value As String)
                m_Category = Value
            End Set
        End Property

        Public Property LastImport() As String
            Get
                Return m_LastImport
            End Get
            Set(ByVal Value As String)
                m_LastImport = Value
            End Set
        End Property

        Public Property LastExport() As String
            Get
                Return m_LastExport
            End Get
            Set(ByVal Value As String)
                m_LastExport = Value
            End Set
        End Property

        Public Property IsOversize() As Boolean
            Get
                Return m_IsOversize
            End Get
            Set(ByVal Value As Boolean)
                m_IsOversize = Value
            End Set
        End Property

        Public Property IsRushDelivery() As Boolean
            Get
                Return m_IsRushDelivery
            End Get
            Set(ByVal value As Boolean)
                m_IsRushDelivery = value
            End Set
        End Property

        Public Property LiftGateCharge() As Double
            Get
                Return m_LiftGateCharge
            End Get
            Set(ByVal value As Double)
                m_LiftGateCharge = value
            End Set
        End Property

        Public Property ScheduleDeliveryCharge() As Double
            Get
                Return m_ScheduleDeliveryCharge
            End Get
            Set(ByVal value As Double)
                m_ScheduleDeliveryCharge = value
            End Set
        End Property

        Public Property RushDeliveryCharge() As Double
            Get
                Return m_RushDeliveryCharge
            End Get
            Set(ByVal value As Double)
                m_RushDeliveryCharge = value
            End Set
        End Property

        Public Property IsHazMat() As Boolean
            Get
                Return m_IsHazMat
            End Get
            Set(ByVal Value As Boolean)
                m_IsHazMat = Value
            End Set
        End Property

        Public Property IsCollection() As Boolean
            Get
                Return m_IsCollection
            End Get
            Set(ByVal Value As Boolean)
                m_IsCollection = Value
            End Set
        End Property

        Public Property IsOnSale() As Boolean
            Get
                Return m_IsOnSale
            End Get
            Set(ByVal Value As Boolean)
                m_IsOnSale = Value
            End Set
        End Property

        Public Property IsFeatured() As Boolean
            Get
                Return m_IsFeatured
            End Get
            Set(ByVal Value As Boolean)
                m_IsFeatured = Value
            End Set
        End Property

        Public Property IsFreeShipping() As Boolean
            Get
                Return m_IsFreeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeShipping = Value
            End Set
        End Property
        Public Property IsFreeSample() As Boolean
            Get
                Return m_IsFreeSample
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeSample = Value
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

        Public Property MixMatchId() As Integer
            Get
                Return m_MixMatchId
            End Get
            Set(ByVal Value As Integer)
                m_MixMatchId = Value
            End Set
        End Property

        Public Property MixMatchDescription() As String
            Get
                Return m_MixMatchDescription
            End Get
            Set(ByVal Value As String)
                m_MixMatchDescription = Value
            End Set
        End Property

        Public Property IsInCart() As Boolean
            Get
                Return m_IsInCart
            End Get
            Set(ByVal Value As Boolean)
                m_IsInCart = Value
            End Set
        End Property
        Public Property IsInCollection() As Boolean
            Get
                Return m_IsInCollection
            End Get
            Set(ByVal Value As Boolean)
                m_IsInCollection = Value
            End Set
        End Property
        Public Property ItemGroupId() As Integer
            Get
                Return m_ItemGroupId
            End Get
            Set(ByVal Value As Integer)
                m_ItemGroupId = Value
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

        Public Property ItemNameNew() As String
            Get
                Return m_ItemNameNew
            End Get
            Set(ByVal Value As String)
                m_ItemNameNew = Value
            End Set
        End Property

        Public Property ItemType() As String
            Get
                Return m_ItemType
            End Get
            Set(ByVal Value As String)
                m_ItemType = Value
            End Set
        End Property

        Public Property Prefix() As String
            Get
                Return m_Prefix
            End Get
            Set(ByVal Value As String)
                m_Prefix = Value
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

        Public Property Price() As Double
            Get
                Return m_Price
            End Get
            Set(ByVal Value As Double)
                m_Price = Value
            End Set
        End Property

        Public Property SalePrice() As Double
            Get
                Return m_SalePrice
            End Get
            Set(ByVal Value As Double)
                m_SalePrice = Value
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

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property IsNew() As Boolean
            Get
                Return m_IsNew
            End Get
            Set(ByVal Value As Boolean)
                m_IsNew = Value
            End Set
        End Property

        Public Property IsBestSeller() As Boolean
            Get
                Return m_IsBestSeller
            End Get
            Set(ByVal Value As Boolean)
                m_IsBestSeller = Value
            End Set
        End Property

        Public Property DoExport() As Boolean
            Get
                Return m_DoExport
            End Get
            Set(ByVal Value As Boolean)
                m_DoExport = Value
            End Set
        End Property

        Public Property NewUntil() As DateTime
            Get
                Return m_NewUntil
            End Get
            Set(ByVal Value As DateTime)
                m_NewUntil = Value
            End Set
        End Property

        Public Property IsTaxFree() As Boolean
            Get
                Return m_IsTaxFree
            End Get
            Set(ByVal Value As Boolean)
                m_IsTaxFree = Value
            End Set
        End Property

        Public Property ShipmentDate() As DateTime
            Get
                Return m_ShipmentDate
            End Get
            Set(ByVal Value As DateTime)
                m_ShipmentDate = Value
            End Set
        End Property

        Public Property PriceDesc() As String
            Get
                Return m_PriceDesc
            End Get
            Set(ByVal Value As String)
                m_PriceDesc = Value
            End Set
        End Property
        Public Property ChoiceName() As String
            Get
                Return m_ChoiceName
            End Get
            Set(ByVal Value As String)
                m_ChoiceName = Value
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

        Public Property ImageAltTag() As String
            Get
                Return m_ImageAltTag
            End Get
            Set(ByVal Value As String)
                m_ImageAltTag = Value
            End Set
        End Property

        Public Property DeliveryTime() As String
            Get
                Return m_DeliveryTime
            End Get
            Set(ByVal Value As String)
                m_DeliveryTime = Value
            End Set
        End Property

        Public Property CarrierType() As String
            Get
                Return m_CarrierType
            End Get
            Set(ByVal Value As String)
                m_CarrierType = Value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal Value As String)
                m_Status = Value
            End Set
        End Property

        Public Property InvMsgId() As Integer
            Get
                Return m_InvMsgId
            End Get
            Set(ByVal Value As Integer)
                m_InvMsgId = Value
            End Set
        End Property

        Public Property QtyOnHand() As Integer
            Get
                Return m_QtyOnHand
            End Get
            Set(ByVal Value As Integer)
                m_QtyOnHand = Value
            End Set
        End Property

        Public Property InventoryStockNotification() As Integer
            Get
                Return m_InventoryStockNotification
            End Get
            Set(ByVal Value As Integer)
                m_InventoryStockNotification = Value
            End Set
        End Property

        Public Property BODate() As DateTime
            Get
                Return m_BODate
            End Get
            Set(ByVal value As DateTime)
                m_BODate = value
            End Set
        End Property

        Public Property LowStockMsg() As String
            Get
                Return m_LowStockMsg
            End Get
            Set(ByVal value As String)
                m_LowStockMsg = value
            End Set
        End Property

        Public Property LowStockThreshold() As Integer
            Get
                Return m_LowStockThreshold
            End Get
            Set(ByVal value As Integer)
                m_LowStockThreshold = value
            End Set
        End Property

        Public Property QtyReserved() As Integer
            Get
                Return m_QtyReserved
            End Get
            Set(ByVal Value As Integer)
                m_QtyReserved = Value
            End Set
        End Property

        Public Property LastUpdated() As DateTime
            Get
                Return m_LastUpdated
            End Get
            Set(ByVal Value As DateTime)
                m_LastUpdated = Value
            End Set
        End Property

        Public Property ShortDesc() As String
            Get
                Return m_ShortDesc
            End Get
            Set(ByVal Value As String)
                m_ShortDesc = Value
            End Set
        End Property

        Public Property LongDesc() As String
            Get
                Return m_LongDesc
            End Get
            Set(ByVal Value As String)
                m_LongDesc = Value
            End Set
        End Property

        Public Property ShortViet() As String
            Get
                Return m_ShortViet
            End Get
            Set(ByVal Value As String)
                m_ShortViet = Value
            End Set
        End Property

        Public Property LongViet() As String
            Get
                Return m_LongViet
            End Get
            Set(ByVal Value As String)
                m_LongViet = Value
            End Set
        End Property

        Public Property ShortFrench() As String
            Get
                Return m_ShortFrench
            End Get
            Set(ByVal Value As String)
                m_ShortFrench = Value
            End Set
        End Property

        Public Property LongFrench() As String
            Get
                Return m_LongFrench
            End Get
            Set(ByVal Value As String)
                m_LongFrench = Value
            End Set
        End Property

        Public Property ShortSpanish() As String
            Get
                Return m_ShortSpanish
            End Get
            Set(ByVal Value As String)
                m_ShortSpanish = Value
            End Set
        End Property

        Public Property LongSpanish() As String
            Get
                Return m_LongSpanish
            End Get
            Set(ByVal Value As String)
                m_LongSpanish = Value
            End Set
        End Property
        Public Property ShortKorea() As String
            Get
                Return m_ShortKorea
            End Get
            Set(ByVal Value As String)
                m_ShortKorea = Value
            End Set
        End Property

        Public Property LongKorea() As String
            Get
                Return m_LongKorea
            End Get
            Set(ByVal Value As String)
                m_LongKorea = Value
            End Set
        End Property
        Public Property MSDS() As String
            Get
                Return m_MSDS
            End Get
            Set(ByVal Value As String)
                m_MSDS = Value
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

        Public Property PromotionId() As Integer
            Get
                Return m_PromotionId
            End Get
            Set(ByVal Value As Integer)
                m_PromotionId = Value
            End Set
        End Property

        Public Property Measurement() As String
            Get
                Return m_Measurement
            End Get
            Set(ByVal Value As String)
                m_Measurement = Value
            End Set
        End Property
        Public Property Final() As String
            Get
                Return m_Final
            End Get
            Set(ByVal value As String)
                m_Final = value
            End Set
        End Property
        Public Property URLCode() As String
            Get
                Return m_URLCode
            End Get
            Set(ByVal value As String)
                m_URLCode = value
            End Set
        End Property
        Public Property IsFreeGift() As Integer
            Get
                Return m_IsFreeGift
            End Get
            Set(ByVal Value As Integer)
                m_IsFreeGift = Value
            End Set
        End Property
        Public Property IsFlatFee() As Boolean
            Get
                Return m_IsFlatFee
            End Get
            Set(ByVal Value As Boolean)
                m_IsFlatFee = Value
            End Set
        End Property
        Public Property IsEbayAllow() As Boolean
            Get
                Return m_IsEbayAllow
            End Get
            Set(ByVal Value As Boolean)
                m_IsEbayAllow = Value
            End Set
        End Property
        Public Property FeeShipOversize() As Double
            Get
                Return m_FeeShipOversize
            End Get
            Set(ByVal Value As Double)
                m_FeeShipOversize = Value
            End Set
        End Property
        Public Property IsFlammable() As Boolean
            Get
                Return m_IsFlammable
            End Get
            Set(ByVal value As Boolean)
                m_IsFlammable = value
            End Set
        End Property
        Public Property IsEbay() As Boolean
            Get
                Return m_IsEbay
            End Get
            Set(ByVal Value As Boolean)
                m_IsEbay = Value
            End Set
        End Property
        Public Property EbayShippingType() As String
            Get
                Return m_EbayShippingType
            End Get
            Set(ByVal value As String)
                m_EbayShippingType = value
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
        Public Property IsRewardPoints() As Boolean
            Get
                Return m_IsRewardPoints
            End Get
            Set(ByVal Value As Boolean)
                m_IsRewardPoints = Value
            End Set
        End Property
        Public Property RewardPoints() As Integer
            Get
                Return m_RewardPoints
            End Get
            Set(ByVal Value As Integer)
                m_RewardPoints = Value
            End Set
        End Property
        Public Property ArrangeRewardPoints() As Integer
            Get
                Return m_ArrangeRewardPoints
            End Get
            Set(ByVal Value As Integer)
                m_ArrangeRewardPoints = Value
            End Set
        End Property
        Public Property CountReview() As Integer
            Get
                Return m_CountReview
            End Get
            Set(ByVal Value As Integer)
                m_CountReview = Value
            End Set
        End Property
        Public Property AverageReview() As Double
            Get
                Return m_AverageReview
            End Get
            Set(ByVal Value As Double)
                m_AverageReview = Value
            End Set
        End Property
        Public Property ShowPrice() As String
            Get
                Return m_ShowPrice
            End Get
            Set(ByVal Value As String)
                m_ShowPrice = Value
            End Set
        End Property
        Public Property ShowInventory() As String
            Get
                Return m_ShowInventory
            End Get
            Set(ByVal Value As String)
                m_ShowInventory = Value
            End Set
        End Property
        Public Property EbayPrice() As Double
            Get
                Return m_EbayPrice
            End Get
            Set(ByVal value As Double)
                m_EbayPrice = value
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
        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property
        Public Property CaseQty() As Integer
            Get
                Return m_CaseQty
            End Get
            Set(ByVal Value As Integer)
                m_CaseQty = Value
            End Set
        End Property
        Public Property CasePrice() As Double
            Get
                Return m_CasePrice
            End Get
            Set(ByVal Value As Double)
                m_CasePrice = Value
            End Set
        End Property
        Public Property HandlingFeeForItem() As Double
            Get
                Return m_HandlingFeeForItem
            End Get
            Set(ByVal Value As Double)
                m_HandlingFeeForItem = Value
            End Set
        End Property
        Public Property HandlingFeeForCase() As Double
            Get
                Return m_HandlingFeeForCase
            End Get
            Set(ByVal Value As Double)
                m_HandlingFeeForCase = Value
            End Set
        End Property
        Public Property ItemProId() As Integer
            Get
                Return m_ItemProId
            End Get
            Set(ByVal Value As Integer)
                m_ItemProId = Value
            End Set
        End Property
        Public Property IsFirstClassPackage() As Boolean
            Get
                Return m_IsFirstClassPackage
            End Get
            Set(ByVal Value As Boolean)
                m_IsFirstClassPackage = Value
            End Set
        End Property
        Public Property ItemSku() As String
            Get
                Return m_ItemSku
            End Get
            Set(ByVal value As String)
                m_ItemSku = value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer)
            m_DB = database
            m_ItemId = ItemId
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal OrderId As Integer, ByVal IsRewardPoints As Boolean)
            m_DB = database
            m_ItemId = ItemId
            m_MemberId = MemberId
            m_OrderId = OrderId
            m_IsRewardPoints = IsRewardPoints
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal SKU As String)
            m_DB = database
            m_SKU = SKU
        End Sub 'New

        Protected Overridable Sub Load(ByVal SKU As String)
            Dim r As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_StoreItem_GetObjectBySKU"
                Dim cmd As SqlCommand = m_DB.CreateCommand(sp)
                cmd.Parameters.Add(m_DB.InParam("SKU", SqlDbType.VarChar, 20, SKU))
                r = cmd.ExecuteReader()

                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", String.Empty, ex)
            End Try
        End Sub

        Protected Overridable Sub LoadInCart(ByVal memberId As Integer)
            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = "SELECT ItemId,PriceDesc,Measurement,SKU,QtyOnHand,IsSpecialOrder,AcceptingOrder,URLCode,IsHazMat,IsFlammable,IsActive,IsRewardPoints,RewardPoints,CasePrice,CaseQty,dbo.fc_CheckPermissionBuyBrand(" & CStr(memberId) & ", si.BrandId) AS PermissionBuyBrand FROM StoreItem si WHERE " & IIf(ItemId <> Nothing, "si.ItemId = " & DB.Quote(ItemId), "SKU = " & DB.Quote(SKU))

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                dr = db.ExecuteReader(CommandType.Text, SQL)

                If dr.HasRows Then
                    If dr.Read Then
                        Me.LoadInCart(dr)
                    End If
                End If

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "StoreItem.vb > LoadInCart", "SQL: " & SQL & "<br>MemberId: " & memberId & "<br>" & ex.ToString() & BaseShoppingCart.GetSessionList())
            End Try
        End Sub
        Protected Overridable Sub LoadInShipping()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT ItemId,URLCode,IsRushDelivery,RushDeliveryCharge,RushDeliveryCharge,ScheduleDeliveryCharge FROM StoreItem si WHERE " & IIf(ItemId <> Nothing, "si.ItemId = " & DB.Quote(ItemId), "SKU = " & DB.Quote(SKU))
                r = m_DB.GetReader(SQL)
                If r.HasRows Then
                    If r.Read Then
                        Me.LoadInShipping(r)
                    End If
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", String.Empty, ex)

            End Try
        End Sub
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT si.*,[dbo].[fc_StoreItem_IsInCollection](ItemId) as IsInCollection  FROM StoreItem si WHERE " & IIf(ItemId <> Nothing, "si.ItemId = " & DB.Quote(ItemId), "SKU = " & DB.Quote(SKU))
                r = m_DB.GetReader(SQL)
                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", String.Empty, ex)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal CustomerPriceGroupId As Integer, Optional ByVal MemberId As Integer = 0)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT si.*,[dbo].[fc_StoreItem_IsInCollection](ItemId) as IsInCollection, [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & ItemId & "," & CustomerPriceGroupId & ",0) as MixMatchId, [dbo].[fc_StoreItem_GetCurrentLowSalePriceItem](si.ItemId) as LowSalePrice, dbo.[fc_StoreItemEnable_IsLoginViewPrice](" & MemberId & ", si.BrandId) AS IsLoginViewPrice FROM StoreItem si WHERE " & IIf(ItemId <> Nothing, "si.ItemId = " & DB.Quote(ItemId), "SKU = " & DB.Quote(SKU)) & "  -- StoreItem.GetMixMatchID>ItemId"
                r = m_DB.GetReader(SQL)
                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", String.Empty, ex)
            End Try
        End Sub

        Protected Overridable Sub LoadFromCart(ByVal MemberId As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "SELECT si.* FROM StoreItem si WHERE si.ItemId = " & DB.Quote(ItemId)
                r = m_DB.GetReader(SQL)
                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb > LoadFromCart", String.Empty, ex)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal OrderId As Integer, ByVal IsRewardPoints As Boolean)
            Dim r As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Dim CustomerPriceGroupId As Integer = 0 'Khoa tam thoi tat vi khong co price group >> Utility.Common.GetCurrentCustomerPriceGroupId()
            Try
                SQL = "SELECT si.*, " _
                        & " dbo.fc_StoreItem_GetMixMatchDescriptionByItem(SI.ItemId," & CustomerPriceGroupId.ToString() & "," & MemberId & "," & OrderId & ") AS MixMatchDescription," _
                        & " dbo.fc_CheckPermissionBuyBrand(" & MemberId.ToString() & ", si.BrandId) AS PermissionBuyBrand," _
                        & " dbo.fc_StoreCartItem_CheckItemInCart(" & OrderId.ToString() & ", si.ItemId," & IIf(IsRewardPoints, 1, 0) & ") AS IsInCart, " _
                        & " [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & ItemId & "," & CustomerPriceGroupId & ",0) AS MixMatchId " _
                        & " FROM StoreItem si WHERE si.ItemId = " & ItemId.ToString() & ""

                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", "ItemId: " & ItemId & "<br>MemberId: " & MemberId & "<br>CustomerPriceGroupId: " & CustomerPriceGroupId.ToString(), ex)
            End Try
        End Sub
        'Protected Overridable Sub LoadByMemberLoginList(ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal checkItemPoint As Boolean)
        '    Dim r As SqlDataReader = Nothing
        '    Dim SQL As String = String.Empty
        '    Try
        '        SQL = "SELECT si.ItemId,si.IsRewardPoints,si.RewardPoints,si.SalePrice,si.Price,si.IsFreeSample,si.IsFreeGift,si.IsFlammable,si.QtyOnHand,si.IsSpecialOrder,si.AcceptingOrder, dbo.fc_CheckPermissionBuyBrand(" & CStr(MemberId) & ", si.BrandId) AS PermissionBuyBrand,  dbo.fc_StoreItem_IsInCart2(" & CStr(MemberId) & ", si.ItemId," & IIf(checkItemPoint = True, "1", "0") & ") AS IsInCart, si.PriceDesc FROM StoreItem si WHERE si.ItemId = " & CStr(ItemId) & ""
        '        r = m_DB.GetReader(SQL)
        '        If r.Read Then
        '            Me.LoadByMemberDataReader(r)
        '        End If
        '        Core.CloseReader(r)
        '    Catch ex As Exception
        '        Core.CloseReader(r)
        '        Core.LogError("StoreItem.vb,ItemId:" & ItemId & ",MemberId:" & MemberId, String.Empty, ex)
        '    End Try
        'End Sub

        'Protected Overridable Sub LoadByMemberDataReader(ByVal r As IDataReader)
        '    Try
        '        m_ItemId = r.Item("ItemId")
        '        m_IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))
        '        If r.Item("IsFreeGift") Is Convert.DBNull Then
        '            m_IsFreeGift = Nothing
        '        Else
        '            m_IsFreeGift = Convert.ToInt32(r.Item("IsFreeGift"))
        '        End If
        '        Try
        '            If r.Item("PermissionBuyBrand") Is Convert.DBNull Then
        '                m_PermissionBuyBrand = True
        '            Else
        '                m_PermissionBuyBrand = CBool(r.Item("PermissionBuyBrand"))
        '            End If
        '        Catch ex As Exception
        '            m_PermissionBuyBrand = True
        '        End Try
        '        Try
        '            If r.Item("IsFlammable") Is Convert.DBNull Then
        '                m_IsFlammable = False
        '            Else
        '                m_IsFlammable = CBool(r.Item("IsFlammable"))
        '            End If
        '        Catch ex As Exception
        '            m_IsFlammable = False
        '        End Try
        '        Try
        '            If r.Item("IsInCart") Is Convert.DBNull Then
        '                m_IsInCart = True
        '            Else
        '                m_IsInCart = CBool(r.Item("IsInCart"))
        '            End If
        '        Catch ex As Exception
        '            m_IsInCart = True
        '        End Try
        '        If r.Item("QtyOnHand") Is Convert.DBNull Then
        '            m_QtyOnHand = Nothing
        '        Else
        '            m_QtyOnHand = Convert.ToInt32(r.Item("QtyOnHand"))
        '        End If
        '        m_IsSpecialOrder = Convert.ToBoolean(r.Item("IsSpecialOrder"))
        '        m_AcceptingOrder = Convert.ToInt32(r.Item("AcceptingOrder"))

        '        If r.Item("Price") Is Convert.DBNull Then
        '            m_Price = Nothing

        '        Else
        '            m_Price = Convert.ToDouble(r.Item("Price"))
        '        End If
        '        If r.Item("SalePrice") Is Convert.DBNull Then
        '            m_SalePrice = Nothing
        '        Else
        '            m_SalePrice = Convert.ToDouble(r.Item("SalePrice"))
        '        End If
        '        If r.Item("PriceDesc") Is Convert.DBNull Then
        '            m_PriceDesc = Nothing
        '        Else
        '            m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
        '        End If
        '        If r.Item("ChoiceName") Is Convert.DBNull Then
        '            m_ChoiceName = Nothing
        '        Else
        '            m_ChoiceName = Convert.ToString(r.Item("ChoiceName"))
        '        End If
        '        If r.Item("RewardPoints") Is Convert.DBNull Then
        '            m_RewardPoints = Nothing

        '        Else
        '            m_RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
        '        End If
        '        Dim IsRewardPoints As Int32
        '        If r.Item("IsRewardPoints") Is Convert.DBNull Then
        '            IsRewardPoints = 0
        '        Else
        '            IsRewardPoints = Convert.ToInt32(r.Item("IsRewardPoints"))
        '        End If
        '        If (IsRewardPoints = 1) Then
        '            m_IsRewardPoints = True
        '        Else
        '            m_IsRewardPoints = False
        '        End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Sub 'Load

        Protected Overridable Sub Load(ByVal r As IDataReader)
            Try
                m_ItemId = r.Item("ItemId")
                m_ItemName = Convert.ToString(r.Item("ItemName"))
                m_ItemNameNew = Convert.ToString(r.Item("ItemNameNew"))
                m_IsOnSale = Convert.ToBoolean(r.Item("IsOnSale"))
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                m_IsNew = Convert.ToBoolean(r.Item("IsNew"))
                m_IsBestSeller = Convert.ToBoolean(r.Item("IsBestSeller"))
                m_IsTaxFree = Convert.ToBoolean(r.Item("IsTaxFree"))

                If r.Item("SKU") IsNot Convert.DBNull Then
                    m_SKU = Convert.ToString(r.Item("SKU"))
                End If
                If r.Item("PriceDesc") IsNot Convert.DBNull Then
                    m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                End If
                If r.Item("ChoiceName") IsNot Convert.DBNull Then
                    m_ChoiceName = Convert.ToString(r.Item("ChoiceName"))
                End If
                If r.Item("Price") Is Convert.DBNull Then
                    m_Price = Nothing
                Else
                    m_Price = Convert.ToDouble(r.Item("Price"))
                End If
                If r.Item("SalePrice") Is Convert.DBNull Then
                    m_SalePrice = Nothing
                Else
                    m_SalePrice = Convert.ToDouble(r.Item("SalePrice"))
                End If

                If r.Item("IsFreeGift") Is Convert.DBNull Then
                    m_IsFreeGift = Nothing
                Else
                    m_IsFreeGift = Convert.ToInt32(r.Item("IsFreeGift"))
                End If

                If r.Item("ItemType") Is Convert.DBNull Then
                    m_ItemType = Nothing
                Else
                    m_ItemType = Convert.ToString(r.Item("ItemType"))
                End If

                ' -----------------------TRY
                Try
                    If r.Item("LowSalePrice") Is Convert.DBNull Then
                        m_LowSalePrice = Nothing
                    Else
                        m_LowSalePrice = Convert.ToDouble(r.Item("LowSalePrice"))
                    End If
                Catch ex As Exception
                    m_LowSalePrice = Nothing
                End Try

                Try
                    If IsDBNull(r.Item("Weight")) Then
                        m_Weight = Nothing
                    Else
                        m_Weight = Convert.ToDouble(r.Item("Weight"))
                    End If
                Catch ex As Exception
                    m_Weight = Nothing
                End Try
                Try
                    If r.Item("ItemGroupId") Is Convert.DBNull Then
                        m_ItemGroupId = Nothing
                    Else
                        m_ItemGroupId = r.Item("ItemGroupId")
                    End If
                Catch ex As Exception
                    m_ItemGroupId = Nothing
                End Try
                Try
                    If r.Item("Measurement") Is Convert.DBNull Then
                        m_Measurement = Nothing
                    Else
                        m_Measurement = Convert.ToString(r.Item("Measurement"))
                    End If
                Catch ex As Exception
                    m_Measurement = Nothing
                End Try
                Try
                    If r.Item("ImageAltTag") Is Convert.DBNull Then
                        m_ImageAltTag = Nothing
                    Else
                        m_ImageAltTag = Convert.ToString(r.Item("ImageAltTag"))
                    End If
                Catch ex As Exception
                    m_ImageAltTag = Nothing
                End Try
                Try
                    If r.Item("PermissionBuyBrand") Is Convert.DBNull Then
                        m_PermissionBuyBrand = True
                    Else
                        m_PermissionBuyBrand = CBool(r.Item("PermissionBuyBrand"))
                    End If
                Catch ex As Exception
                    m_PermissionBuyBrand = True
                End Try
                Try
                    If r.Item("IsLoginViewPrice") Is Convert.DBNull Then
                        m_IsLoginViewPrice = False
                    Else
                        m_IsLoginViewPrice = CBool(r.Item("IsLoginViewPrice"))
                    End If
                Catch ex As Exception
                    m_IsLoginViewPrice = False
                End Try
                Try
                    If r.Item("IsInCart") Is Convert.DBNull Then
                        m_IsInCart = True
                    Else
                        m_IsInCart = CBool(r.Item("IsInCart"))
                    End If
                Catch ex As Exception
                    m_IsInCart = False
                End Try
                Try
                    If r.Item("IsInCollection") Is Convert.DBNull Then
                        m_IsInCollection = True
                    Else
                        m_IsInCollection = CBool(r.Item("IsInCollection"))
                    End If
                Catch ex As Exception
                    m_IsInCollection = True
                End Try
                Try
                    If IsDBNull(r.Item("MaximumQuantity")) Then
                        m_MaximumQuantity = Nothing
                    Else
                        m_MaximumQuantity = Convert.ToInt32(r.Item("MaximumQuantity"))
                    End If
                Catch ex As Exception
                    m_MaximumQuantity = Nothing
                End Try
                Try
                    If r.Item("PageTitle") Is Convert.DBNull Then
                        m_PageTitle = Nothing
                    Else
                        m_PageTitle = Convert.ToString(r.Item("PageTitle"))
                    End If
                Catch ex As Exception
                    m_PageTitle = Nothing
                End Try
                Try
                    If r.Item("OutsideUSPageTitle") Is Convert.DBNull Then
                        m_OutsideUSPageTitle = Nothing
                    Else
                        m_OutsideUSPageTitle = Convert.ToString(r.Item("OutsideUSPageTitle"))
                    End If
                Catch ex As Exception
                    m_OutsideUSPageTitle = Nothing
                End Try
                Try
                    If r.Item("Prefix") Is Convert.DBNull Then
                        m_Prefix = Nothing
                    Else
                        m_Prefix = Convert.ToString(r.Item("Prefix"))
                    End If
                Catch ex As Exception
                    m_Prefix = Nothing
                End Try
                Try
                    If r.Item("MetaDescription") Is Convert.DBNull Then
                        m_MetaDescription = Nothing
                    Else
                        m_MetaDescription = Convert.ToString(r.Item("MetaDescription"))
                    End If
                Catch ex As Exception
                    m_MetaDescription = Nothing
                End Try
                Try
                    If r.Item("OutsideUSMetaDescription") Is Convert.DBNull Then
                        m_OutsideUSMetaDescription = Nothing
                    Else
                        m_OutsideUSMetaDescription = Convert.ToString(r.Item("OutsideUSMetaDescription"))
                    End If
                Catch ex As Exception
                    m_OutsideUSMetaDescription = Nothing
                End Try
                Try
                    If r.Item("MetaKeywords") Is Convert.DBNull Then
                        m_MetaKeywords = Nothing
                    Else
                        m_MetaKeywords = Convert.ToString(r.Item("MetaKeywords"))
                    End If
                Catch ex As Exception
                    m_MetaKeywords = Nothing
                End Try
                Try
                    If r.Item("NewUntil") Is Convert.DBNull Then
                        m_NewUntil = Nothing
                    Else
                        m_NewUntil = Convert.ToDateTime(r.Item("NewUntil"))
                    End If
                Catch ex As Exception
                    m_NewUntil = Nothing
                End Try
                Try
                    If r.Item("ShipmentDate") Is Convert.DBNull Then
                        m_ShipmentDate = Nothing
                    Else
                        m_ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                    End If
                Catch ex As Exception
                    m_ShipmentDate = Nothing
                End Try
                Try
                    If r.Item("ModifyDate") Is Convert.DBNull Then
                        m_ModifyDate = Nothing
                    Else
                        m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                    End If
                Catch ex As Exception
                    m_ModifyDate = Nothing
                End Try
                Try
                    If r.Item("LowStockMsg") Is Convert.DBNull Then
                        m_LowStockMsg = Nothing
                    Else
                        m_LowStockMsg = Convert.ToString(r.Item("LowStockMsg"))
                    End If
                Catch ex As Exception
                    m_LowStockMsg = Nothing
                End Try
                Try
                    If r.Item("LowStockThreshold") Is Convert.DBNull Then
                        m_LowStockThreshold = Nothing
                    Else
                        m_LowStockThreshold = Convert.ToInt32(r.Item("LowStockThreshold"))
                    End If
                Catch ex As Exception
                    m_LowStockThreshold = Nothing
                End Try

                If r.Item("Image") Is Convert.DBNull Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If
                If r.Item("DeliveryTime") Is Convert.DBNull Then
                    m_DeliveryTime = Nothing
                Else
                    m_DeliveryTime = Convert.ToString(r.Item("DeliveryTime"))
                End If
                If r.Item("InvMsgId") Is Convert.DBNull Then
                    m_InvMsgId = Nothing
                Else
                    m_InvMsgId = Convert.ToInt32(r.Item("InvMsgId"))
                End If
                If r.Item("CarrierType") Is Convert.DBNull Then
                    m_CarrierType = Nothing
                Else
                    m_CarrierType = Convert.ToString(r.Item("CarrierType"))
                End If
                If r.Item("Status") Is Convert.DBNull Then
                    m_Status = Nothing
                Else
                    m_Status = Convert.ToString(r.Item("Status"))
                End If
                If r.Item("QtyOnHand") Is Convert.DBNull Then
                    m_QtyOnHand = Nothing
                Else
                    m_QtyOnHand = Convert.ToInt32(r.Item("QtyOnHand"))
                End If
                If r.Item("InventoryStockNotification") Is Convert.DBNull Then
                    m_InventoryStockNotification = Nothing
                Else
                    m_InventoryStockNotification = Convert.ToInt32(r.Item("InventoryStockNotification"))
                End If
                If r.Item("QtyReserved") Is Convert.DBNull Then
                    m_QtyReserved = Nothing
                Else
                    m_QtyReserved = Convert.ToString(r.Item("QtyReserved"))
                End If
                If r.Item("LastUpdated") Is Convert.DBNull Then
                    m_LastUpdated = Nothing
                Else
                    m_LastUpdated = Convert.ToString(r.Item("LastUpdated"))
                End If

                If r.Item("ShortDesc") Is Convert.DBNull Then
                    m_ShortDesc = Nothing
                Else
                    m_ShortDesc = Convert.ToString(r.Item("ShortDesc"))
                End If
                If r.Item("LongDesc") Is Convert.DBNull Then
                    m_LongDesc = Nothing
                Else
                    m_LongDesc = Convert.ToString(r.Item("LongDesc"))
                End If
                If r.Item("ShortViet") Is Convert.DBNull Then
                    m_ShortViet = Nothing
                Else
                    m_ShortViet = Convert.ToString(r.Item("ShortViet"))
                End If
                If r.Item("LongViet") Is Convert.DBNull Then
                    m_LongViet = Nothing
                Else
                    m_LongViet = Convert.ToString(r.Item("LongViet"))
                End If
                If r.Item("ShortFrench") Is Convert.DBNull Then
                    m_ShortFrench = Nothing
                Else
                    m_ShortFrench = Convert.ToString(r.Item("ShortFrench"))
                End If
                If r.Item("LongFrench") Is Convert.DBNull Then
                    m_LongFrench = Nothing
                Else
                    m_LongFrench = Convert.ToString(r.Item("LongFrench"))
                End If
                If r.Item("ShortSpanish") Is Convert.DBNull Then
                    m_ShortSpanish = Nothing
                Else
                    m_ShortSpanish = Convert.ToString(r.Item("ShortSpanish"))
                End If
                If r.Item("LongSpanish") Is Convert.DBNull Then
                    m_LongSpanish = Nothing
                Else
                    m_LongSpanish = Convert.ToString(r.Item("LongSpanish"))
                End If

                If r.Item("MSDS") Is Convert.DBNull Then
                    m_MSDS = Nothing
                Else
                    m_MSDS = Convert.ToString(r.Item("MSDS"))
                End If

                If IsDBNull(r.Item("RushDeliveryCharge")) Then
                    m_RushDeliveryCharge = Nothing
                Else
                    m_RushDeliveryCharge = r.Item("RushDeliveryCharge")
                End If
                m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
                If IsDBNull(r.Item("BODate")) Then
                    m_BODate = Nothing
                Else
                    m_BODate = Convert.ToDateTime(r.Item("BODate"))
                End If
                If IsDBNull(r.Item("BrandId")) Then
                    m_BrandId = Nothing
                Else
                    m_BrandId = Convert.ToInt32(r.Item("BrandId"))
                End If
                If IsDBNull(r.Item("Category")) Then
                    m_Category = Nothing
                Else
                    m_Category = Convert.ToString(r.Item("Category"))
                End If
                Try

                Catch ex As Exception

                End Try
                If IsDBNull(r.Item("AdditionalInfo")) Then
                    m_AdditionalInfo = Nothing
                Else
                    m_AdditionalInfo = Convert.ToString(r.Item("AdditionalInfo"))
                End If
                If IsDBNull(r.Item("Specifications")) Then
                    m_Specifications = Nothing
                Else
                    m_Specifications = Convert.ToString(r.Item("Specifications"))
                End If
                If IsDBNull(r.Item("ShippingInfo")) Then
                    m_ShippingInfo = Nothing
                Else
                    m_ShippingInfo = Convert.ToString(r.Item("ShippingInfo"))
                End If
                If IsDBNull(r.Item("HelpfulTips")) Then
                    m_HelpfulTips = Nothing
                Else
                    m_HelpfulTips = Convert.ToString(r.Item("HelpfulTips"))
                End If
                If IsDBNull(r.Item("LastImport")) Then
                    m_LastImport = Nothing
                Else
                    m_LastImport = Convert.ToString(r.Item("LastImport"))
                End If
                If Not IsDBNull(r.Item("LiftGateCharge")) Then m_LiftGateCharge = Convert.ToDouble(r.Item("LiftGateCharge")) Else m_LiftGateCharge = Nothing
                If Not IsDBNull(r.Item("ScheduleDeliveryCharge")) Then m_ScheduleDeliveryCharge = Convert.ToDouble(r.Item("ScheduleDeliveryCharge")) Else m_ScheduleDeliveryCharge = Nothing

                m_IsFeatured = Convert.ToBoolean(r.Item("IsFeatured"))
                m_IsCollection = Convert.ToBoolean(r.Item("IsCollection"))
                m_IsOversize = Convert.ToBoolean(r.Item("IsOversize"))
                m_IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                m_IsRushDelivery = Convert.ToBoolean(r.Item("IsRushDelivery"))
                m_TaxGroupCode = Convert.ToString(r.Item("TaxGroupCode"))
                m_IsHot = Convert.ToBoolean(r.Item("IsHot"))
                m_IsSpecialOrder = Convert.ToBoolean(r.Item("IsSpecialOrder"))
                m_AcceptingOrder = Convert.ToInt32(r.Item("AcceptingOrder"))
                m_IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                m_IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))

                If IsDBNull(r.Item("PromotionId")) Then
                    m_PromotionId = Nothing
                Else
                    m_PromotionId = Convert.ToInt32(r.Item("PromotionId"))
                End If
                'm_InOutStock = Convert.ToString(r.Item("InOutStock"))
                'm_DepartmentId = Convert.ToInt32(r.Item("DepartmentId"))
                If IsDBNull(r.Item("URLCode")) Then
                    m_URLCode = Nothing
                Else
                    m_URLCode = Convert.ToString(r.Item("URLCode"))
                End If
                Try
                    If IsDBNull(r.Item("IsFlatFee")) Then
                        m_IsFlatFee = False
                    Else
                        m_IsFlatFee = CBool(r.Item("IsFlatFee"))
                    End If

                Catch ex As Exception
                    m_IsFlatFee = False
                End Try

                If IsDBNull(r.Item("FeeShipOversize")) Then
                    m_FeeShipOversize = Nothing
                Else
                    m_FeeShipOversize = CDbl(r.Item("FeeShipOversize"))
                End If
                Try
                    If IsDBNull(r.Item("IsEbayAllow")) Then
                        m_IsEbayAllow = False
                    Else
                        m_IsEbayAllow = CBool(r.Item("IsEbayAllow"))
                    End If

                Catch ex As Exception
                    m_IsEbayAllow = False
                End Try
                Try
                    If r.Item("IsFlammable") Is Convert.DBNull Then
                        m_IsFlammable = False
                    Else
                        m_IsFlammable = CBool(r.Item("IsFlammable"))
                    End If
                Catch ex As Exception
                    m_IsFlammable = False
                End Try
                Try
                    If IsDBNull(r.Item("IsEbay")) Then
                        m_IsEbay = False
                    Else
                        m_IsEbay = CBool(r.Item("IsEbay"))
                    End If

                Catch ex As Exception
                    m_IsEbayAllow = False
                End Try
                Try
                    If IsDBNull(r.Item("EbayShippingType")) Then
                        m_EbayShippingType = Nothing
                    Else
                        m_EbayShippingType = Convert.ToString(r.Item("EbayShippingType"))
                    End If
                Catch ex As Exception
                    m_EbayShippingType = Nothing
                End Try
                If IsDBNull(r.Item("IsRewardPoints")) Then
                    m_IsRewardPoints = False
                Else
                    m_IsRewardPoints = CBool(r.Item("IsRewardPoints"))
                End If
                If IsDBNull(r.Item("RewardPoints")) Then
                    m_RewardPoints = Nothing
                Else
                    m_RewardPoints = CDbl(r.Item("RewardPoints"))
                End If
                If r.Item("ArrangeRewardPoints") Is Convert.DBNull Then
                    m_ArrangeRewardPoints = Nothing
                Else
                    m_ArrangeRewardPoints = Convert.ToInt32(r.Item("ArrangeRewardPoints"))
                End If
                If r.Item("EbayPrice") Is Convert.DBNull Then
                    m_EbayPrice = Nothing

                Else
                    m_EbayPrice = Convert.ToDouble(r.Item("EbayPrice"))
                End If
                Try
                    If IsDBNull(r.Item("MixMatchDescription")) Then
                        m_MixMatchDescription = Nothing
                    Else
                        m_MixMatchDescription = Convert.ToString(r.Item("MixMatchDescription"))
                    End If
                Catch
                End Try

                Try
                    If IsDBNull(r.Item("MixMatchId")) Then
                        m_MixMatchId = Nothing
                    Else
                        m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
                    End If
                Catch
                End Try

                If r.Item("CasePrice") Is Convert.DBNull Then
                    m_CasePrice = Nothing

                Else
                    m_CasePrice = Convert.ToDouble(r.Item("CasePrice"))
                End If
                If r.Item("CaseQty") Is Convert.DBNull Then
                    m_CaseQty = Nothing

                Else
                    m_CaseQty = Convert.ToInt32(r.Item("CaseQty"))
                End If
                Try
                    If r.Item("IsFirstClassPackage") Is Convert.DBNull Then
                        m_IsFirstClassPackage = False
                    Else
                        m_IsFirstClassPackage = CBool(r.Item("IsFirstClassPackage"))
                    End If
                Catch ex As Exception
                    m_IsInCart = False
                End Try
                Try
                    If (NewUntil = Nothing And IsNew) Or (NewUntil <> Nothing And NewUntil > Now) Then
                        IsNewTrue = True
                    Else
                        IsNewTrue = False
                    End If
                Catch ex As Exception
                    IsNewTrue = False
                End Try
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load



        Protected Overridable Sub LoadInCart(ByVal r As IDataReader)
            Try
                m_ItemId = r.Item("ItemId")
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                If r.Item("PriceDesc") IsNot Convert.DBNull Then
                    m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                End If
                If r.Item("QtyOnHand") Is Convert.DBNull Then
                    m_QtyOnHand = Nothing
                Else
                    m_QtyOnHand = Convert.ToInt32(r.Item("QtyOnHand"))
                End If

                If r.Item("IsRewardPoints") Is Convert.DBNull Then
                    m_IsRewardPoints = False
                Else
                    m_IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
                End If

                If r.Item("RewardPoints") Is Convert.DBNull Then
                    m_RewardPoints = Nothing
                Else
                    m_RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
                End If
                If r.Item("IsFlammable") Is Convert.DBNull Then
                    IsFlammable = Nothing
                Else
                    IsFlammable = Convert.ToDouble(r.Item("IsFlammable"))
                End If
                If r.Item("IsHazMat") Is Convert.DBNull Then
                    IsHazMat = Nothing
                Else
                    IsHazMat = Convert.ToDouble(r.Item("IsHazMat"))
                End If
                Try
                    If r.Item("Measurement") Is Convert.DBNull Then
                        m_Measurement = Nothing
                    Else
                        m_Measurement = Convert.ToString(r.Item("Measurement"))
                    End If
                Catch ex As Exception
                    m_Measurement = Nothing
                End Try
                m_IsSpecialOrder = Convert.ToBoolean(r.Item("IsSpecialOrder"))
                m_AcceptingOrder = Convert.ToInt32(r.Item("AcceptingOrder"))
                If IsDBNull(r.Item("URLCode")) Then
                    m_URLCode = Nothing
                Else
                    m_URLCode = Convert.ToString(r.Item("URLCode"))
                End If
                If IsDBNull(r.Item("SKU")) Then
                    m_SKU = Nothing
                Else
                    m_SKU = Convert.ToString(r.Item("SKU"))
                End If

                If r.Item("PermissionBuyBrand") Is Convert.DBNull Then
                    m_PermissionBuyBrand = True
                Else
                    m_PermissionBuyBrand = CBool(r.Item("PermissionBuyBrand"))
                End If
                Try
                    If r.Item("CaseQty") Is Convert.DBNull Then
                        m_CaseQty = Nothing
                    Else
                        m_CaseQty = Convert.ToInt32(r.Item("CaseQty"))
                    End If
                Catch ex As Exception
                    m_CaseQty = Nothing
                End Try

            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load
        Protected Overridable Sub LoadInShipping(ByVal r As IDataReader)
            Try
                m_ItemId = r.Item("ItemId")
                If IsDBNull(r.Item("URLCode")) Then
                    m_URLCode = Nothing
                Else
                    m_URLCode = Convert.ToString(r.Item("URLCode"))
                End If
                m_IsRushDelivery = Convert.ToBoolean(r.Item("IsRushDelivery"))
                If IsDBNull(r.Item("RushDeliveryCharge")) Then
                    m_RushDeliveryCharge = Nothing
                Else
                    m_RushDeliveryCharge = r.Item("RushDeliveryCharge")
                End If
                If Not IsDBNull(r.Item("ScheduleDeliveryCharge")) Then m_ScheduleDeliveryCharge = Convert.ToDouble(r.Item("ScheduleDeliveryCharge")) Else m_ScheduleDeliveryCharge = Nothing
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load

        Public Shared Function GetAllItemActive(ByVal _DB As Database) As DataSet
            Dim SQL As String = " Select * from StoreItem where IsActive=1 order by LastUpdated DESC"
            Dim ds As DataSet = _DB.GetDataSet(SQL)
            Return ds
        End Function

        Public Overridable Sub Insert()
            Dim SQL As String

            SQL = " INSERT INTO StoreItem (" _
             & " ItemName" _
             & ",ItemNameNew" _
             & ",ItemType" _
             & ",SKU" _
             & ",Price" _
             & ",ItemGroupId" _
             & ",Weight" _
             & ",SalePrice" _
             & ",PageTitle" _
             & ",OutsideUSPageTitle" _
             & ",MetaDescription" _
             & ",OutsideUSMetaDescription" _
             & ",MetaKeywords" _
             & ",IsActive" _
             & ",IsNew" _
             & ",IsBestSeller" _
             & ",NewUntil" _
             & ",IsTaxFree" _
             & ",ShipmentDate" _
             & ",PriceDesc" _
             & ",Image" _
             & ",ImageAltTag" _
             & ",DeliveryTime" _
             & ",CarrierType" _
             & ",Status" _
             & ",InvMsgId" _
             & ",CreateDate" _
             & ",ModifyDate" _
             & ",QtyOnHand" _
             & ",InventoryStockNotification" _
             & ",LowStockMsg" _
             & ",LowStockThreshold" _
             & ",BODate" _
             & ",QtyReserved" _
             & ",LastUpdated" _
             & ",ShortDesc" _
             & ",LongDesc" _
             & ",ShortViet" _
             & ",LongViet" _
             & ",ShortFrench" _
             & ",LongFrench" _
             & ",ShortSpanish" _
             & ",LongSpanish" _
             & ",AdditionalInfo" _
             & ",Specifications" _
             & ",ShippingInfo" _
             & ",HelpfulTips" _
             & ",MSDS" _
             & ",Prefix" _
             & ",IsFeatured" _
             & ",IsOnSale" _
             & ",BrandId" _
             & ",Category" _
             & ",LastImport" _
             & ",LastExport" _
             & ",DoExport" _
             & ",IsCollection" _
             & ",IsOversize" _
             & ",IsHazMat" _
             & ",MaximumQuantity" _
             & ",IsRushDelivery" _
             & ",RushDeliveryCharge" _
             & ",IsHot" _
             & ",LiftGateCharge" _
             & ",ScheduleDeliveryCharge" _
             & ",IsSpecialOrder" _
             & ",AcceptingOrder" _
             & ",TaxGroupCode" _
             & ",IsFreeShipping" _
             & ",PromotionID" _
            & ",Measurement" _
             & ",IsFreeSample" _
             & ",IsFreeGift" _
             & ",URLCode" _
             & ",IsFlatFee" _
             & ",FeeShipOversize" _
             & ",IsEbayAllow" _
             & ",IsFlammable" _
             & ",EbayShippingType" _
             & ",IsRewardPoints" _
             & ",RewardPoints" _
             & ",IsFirstClassPackage" _
             & ") VALUES (" _
             & m_DB.Quote(ItemName) _
             & "," & m_DB.Quote(ItemNameNew) _
             & "," & m_DB.Quote(ItemType) _
             & "," & m_DB.Quote(SKU) _
             & "," & m_DB.Number(Price) _
             & "," & m_DB.NullNumber(ItemGroupId) _
             & "," & m_DB.Number(Weight) _
             & "," & m_DB.Number(SalePrice) _
             & "," & m_DB.Quote(PageTitle) _
               & "," & m_DB.Quote(OutsideUSPageTitle) _
             & "," & m_DB.Quote(MetaDescription) _
              & "," & m_DB.Quote(OutsideUSMetaDescription) _
             & "," & m_DB.Quote(MetaKeywords) _
             & "," & CInt(IsActive) _
             & "," & CInt(IsNew) _
             & "," & CInt(IsBestSeller) _
             & "," & m_DB.NullQuote(NewUntil) _
             & "," & CInt(IsTaxFree) _
             & "," & m_DB.NullQuote(ShipmentDate) _
             & "," & m_DB.Quote(PriceDesc) _
             & "," & m_DB.Quote(Image) _
             & "," & m_DB.Quote(ImageAltTag) _
             & "," & m_DB.Quote(DeliveryTime) _
             & "," & m_DB.Quote(CarrierType) _
             & "," & m_DB.Quote(Status) _
             & "," & m_DB.Number(InvMsgId) _
             & "," & m_DB.NullQuote(Now) _
             & "," & m_DB.NullQuote(Now) _
             & "," & m_DB.Number(QtyOnHand) _
             & "," & m_DB.NullNumber(InventoryStockNotification) _
             & "," & m_DB.Quote(LowStockMsg) _
             & "," & m_DB.NullNumber(LowStockThreshold) _
             & "," & m_DB.NullQuote(BODate) _
             & "," & m_DB.Number(QtyReserved) _
             & "," & m_DB.NullQuote(LastUpdated) _
             & "," & m_DB.Quote(ShortDesc) _
             & "," & m_DB.Quote(LongDesc) _
             & "," & m_DB.NQuote(ShortViet) _
             & "," & m_DB.NQuote(LongViet) _
             & "," & m_DB.NQuote(ShortFrench) _
             & "," & m_DB.NQuote(LongFrench) _
             & "," & m_DB.NQuote(ShortSpanish) _
             & "," & m_DB.NQuote(LongSpanish) _
             & "," & m_DB.Quote(AdditionalInfo) _
             & "," & m_DB.Quote(Specifications) _
             & "," & m_DB.Quote(ShippingInfo) _
             & "," & m_DB.Quote(HelpfulTips) _
             & "," & m_DB.Quote(MSDS) _
             & "," & m_DB.Quote(Prefix) _
             & "," & CInt(IsFeatured) _
             & "," & CInt(IsOnSale) _
             & "," & m_DB.NullNumber(BrandId) _
             & "," & m_DB.Quote(Category) _
             & "," & m_DB.Quote(LastImport) _
             & "," & m_DB.Quote(LastExport) _
             & "," & CInt(DoExport) _
             & "," & CInt(IsCollection) _
             & "," & CInt(IsOversize) _
             & "," & CInt(IsHazMat) _
             & "," & DB.NullNumber(MaximumQuantity) _
             & "," & CInt(IsRushDelivery) _
             & "," & DB.Number(RushDeliveryCharge) _
             & "," & CInt(IsHot) _
             & "," & DB.Number(LiftGateCharge) _
             & "," & DB.Number(ScheduleDeliveryCharge) _
             & "," & CInt(IsSpecialOrder) _
             & "," & CInt(AcceptingOrder) _
             & "," & DB.Quote(TaxGroupCode) _
             & "," & CInt(IsFreeShipping) _
             & "," & m_DB.NullNumber(PromotionId) _
             & "," & CInt(Measurement) _
             & "," & CInt(IsFreeSample) _
             & "," & CInt(IsFreeGift) _
             & ",[dbo].[GenerateURLCode](" & m_DB.Quote(URLCode) & ")" _
             & "," & CInt(IsFlatFee) _
             & "," & FeeShipOversize _
             & "," & CInt(IsEbayAllow) _
             & "," & CInt(IsFlammable) _
             & "," & m_DB.Quote(EbayShippingType) _
             & "," & IIf(IsRewardPoints, "1", "0") _
             & "," & m_DB.Number(RewardPoints) _
             & "," & CInt(IsFirstClassPackage) _
             & ")"

            m_DB.ExecuteSQL(SQL)
            'Clear cach

            UpdateGroup()
        End Sub 'Insert

        Function AutoInsert() As Integer
            Dim SQL As String = "SELECT @@IDENTITY"

            Insert()
            Return m_DB.ExecuteScalar(SQL)
        End Function

        Private Sub UpdateGroup()
            If Not Me.ItemGroupId = Nothing Then
                DB.ExecuteSQL("update storeitem set choicename = (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = storeitem.itemid order by o.sortorder) where itemid = " & ItemId)
            End If
        End Sub
        Private Function CheckDescriptionUpdate() As Boolean
            If Not IsActive Then
                Return True
            End If
            Dim shortDes As String = Nothing
            Dim longDes As String = Nothing
            Try
                shortDes = ShortDesc.Trim()
            Catch ex As Exception

            End Try
            Try
                longDes = LongDesc.Trim()
            Catch ex As Exception

            End Try
            If (shortDes Is Nothing Or longDes Is Nothing Or shortDes = "" Or longDes = "") And IsFreeSample = False Then
                Dim adminID As String = System.Web.HttpContext.Current.Session("AdminId")
                Dim ipAddress As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                Dim MemberId As String = System.Web.HttpContext.Current.Session("MemberId")
                Dim MemberUsername As String = System.Web.HttpContext.Current.Session("Username")
                Dim msg = "Update Desc to Empty,User Update:{0},ip:" & ipAddress & ",date:" & Now() & ",link:" & System.Web.HttpContext.Current.Request.RawUrl
                If adminID Is Nothing And MemberUsername Is Nothing Then
                    msg = String.Format(msg, "Unknow")
                ElseIf Not adminID Is Nothing Then
                    msg = String.Format(msg, "Admin(id:" & adminID & ")")
                Else
                    msg = String.Format(msg, "Member(id:" & MemberId & ",username:" & MemberUsername & ")")
                End If
                Email.SendError("ToError500", "Update Item Description Empty", msg)
                Return False
            End If
            Return True
        End Function
        Protected Function CheckItemFreeSampleCart(ByVal objItem As StoreItemRow, ByVal type As String)
            If type = Utility.Common.AdminLogAction.Update.ToString() Then ''update
                Dim oldFreeSample As Boolean = CBool(DB.ExecuteScalar("Select IsFreeSample from StoreItem where ItemId=" & objItem.ItemId))
                If oldFreeSample <> objItem.IsFreeSample Then ''co update lai co IsFreeSample
                    Return True
                End If
                Dim result As Integer = 0
                result = DB.ExecuteScalar("SELECT TOP 1 (ItemId) FROM StoreCartItem WHERE IsFreeSample = 1 AND " & IIf(objItem.ItemId <> Nothing, "ItemId = " & DB.Quote(objItem.ItemId), " SKU = " & DB.Quote(objItem.SKU)) & " and orderid in (select orderid from storeOrder where PaymentType is null)")
                If (result > 0 And objItem.IsFreeSample = False) Then
                    Return True
                End If
                If objItem.IsFreeSample Then
                    If objItem.IsActive = False Then
                        Return True
                    End If
                    If (objItem.QtyOnHand <= 0 AndAlso objItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.None) Then
                        Return True
                    End If
                End If
            ElseIf type = Utility.Common.AdminLogAction.DoActive.ToString() Then ''Active
                Dim newActive As Boolean = False
                If objItem.IsActive = False Then
                    newActive = True
                End If
                Dim result As Integer = 0
                result = DB.ExecuteScalar("SELECT TOP 1 (ItemId) FROM StoreCartItem WHERE IsFreeSample = 1 AND " & IIf(objItem.ItemId <> Nothing, "ItemId = " & DB.Quote(objItem.ItemId), " SKU = " & DB.Quote(objItem.SKU)) & " and orderid in (select orderid from storeOrder where PaymentType is null)")
                If result > 0 Then
                    If Not (newActive) Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function
        Protected Function CheckItemFreeGiftCart(ByVal objItem As StoreItemRow, ByVal objFreeGift As FreeGiftRow, ByVal type As String) As Boolean
            Dim result As Integer = 0
            If Not objItem Is Nothing Then ''update item
                result = DB.ExecuteScalar("Select [dbo].[fc_StoreItem_IsFreeGift](" & objItem.ItemId & ")")
                If result = 0 Then ''not free gift
                    Return False
                End If
                ''kiem tra xem item hien tai co nam trogn cart hay khong
                result = DB.ExecuteScalar("SELECT TOP 1 (ItemId) FROM StoreCartItem WHERE IsFreeItem = 1 AND ItemId = " & objItem.ItemId & " and (FreeItemIds is null or FreeItemIds='') and orderid in (select orderid from storeOrder where PaymentType is null)")
                If result < 1 Then
                    Return False
                End If
                If type = Utility.Common.AdminLogAction.Update.ToString() Then ''update item
                    If objItem.IsActive = False Then
                        Return True
                    End If
                    If (objItem.QtyOnHand <= 0 AndAlso objItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.None) Then
                        Return True
                    End If
                ElseIf type = Utility.Common.AdminLogAction.DoActive.ToString() Then ''Active item
                    Dim newActive As Boolean = False
                    If objItem.IsActive = False Then
                        newActive = True
                    End If
                    If Not (newActive) Then
                        Return True
                    End If
                    'ElseIf type = Utility.Common.AdminLogAction.DoActive.ToString() Then ''delete item
                    '    Return True
                    'End If
                End If
            End If
            If Not objFreeGift Is Nothing Then
                ''kiem tra xem item hien tai co nam trogn cart hay khong
                result = DB.ExecuteScalar("SELECT TOP 1 (ItemId) FROM StoreCartItem WHERE IsFreeItem = 1 AND ItemId = " & objFreeGift.ItemId & " and (FreeItemIds is null or FreeItemIds='') and orderid in (select orderid from storeOrder where PaymentType is null)")
                If result < 1 Then
                    Return False
                End If
                If type = Utility.Common.AdminLogAction.Update.ToString() Then ''update item
                    If objFreeGift.IsActive = False Then
                        Return True
                    End If
                ElseIf type = Utility.Common.AdminLogAction.Delete.ToString() Then ''Active item
                    Return True
                End If
            End If
            Return False
        End Function


        Public Sub CheckFreeSampleItem(ByVal objItem As StoreItemRow, ByVal type As String)
            If CheckItemFreeSampleCart(objItem, type) = True Then
                Dim r As SqlDataReader = Nothing
                Try
                    r = DB.GetReader("Select OrderId From StoreCartItem Where " & IIf(objItem.ItemId <> Nothing, "ItemId = " & DB.Quote(objItem.ItemId), " SKU = " & DB.Quote(objItem.SKU)) & " and orderid in (select orderid from storeOrder where PaymentType is null)")
                    While r.Read
                        Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & r(0))
                    End While
                Catch ex As Exception

                End Try
                Core.CloseReader(r)
                m_DB.ExecuteSQL("Delete From StoreCartItem where " & IIf(objItem.ItemId <> Nothing, "ItemId = " & DB.Quote(objItem.ItemId), " SKU = " & DB.Quote(objItem.SKU)) & " and orderid in (select orderid from storeOrder where PaymentType is null)")
            End If
        End Sub
        Public Sub CheckFreeGiftItem(ByVal objItem As StoreItemRow, ByVal objFreeGift As FreeGiftRow, ByVal type As String)
            Dim itemId As Integer = 0
            If Not objItem Is Nothing Then
                itemId = objItem.ItemId
            ElseIf Not objFreeGift Is Nothing Then
                itemId = objFreeGift.ItemId
            End If
            If itemId < 1 Then
                Exit Sub
            End If
            If CheckItemFreeGiftCart(objItem, objFreeGift, type) = True Then
                Dim r As SqlDataReader = Nothing
                Try
                    r = DB.GetReader("Select OrderId From StoreCartItem Where ItemId=" & itemId & " and IsFreeItem=1 and (FreeItemIds is null or FreeItemIds='') and orderid in (select orderid from storeOrder where PaymentType is null)")
                    While r.Read
                        Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & r(0))
                    End While
                Catch ex As Exception

                End Try
                Core.CloseReader(r)
                m_DB.ExecuteSQL("Delete From StoreCartItem where ItemId=" & itemId & " and IsFreeItem = 1 AND (FreeItemIds is null or FreeItemIds='') and orderid in (select orderid from storeOrder where PaymentType is null)")
            End If
        End Sub
        Public Overridable Sub Update()
            'If Not CheckDescriptionUpdate() Then
            '    Exit Sub
            'End If
            CheckFreeSampleItem(Me, Utility.Common.AdminLogAction.Update.ToString())
            CheckFreeGiftItem(Me, Nothing, Utility.Common.AdminLogAction.Update.ToString())
            IPNUpdate()

            'Dim SQL As String
            'SQL = " UPDATE StoreCartItem SET " _
            ' & " Weight = " & DB.Number(Weight)

            ''Neu item la free sample thi ko update gia trong Cart Item
            'If Not IsFreeSample Then
            '    SQL &= ",Price = " & DB.Quote(Price) _
            '    & ",SalePrice = " & DB.Quote(SalePrice)
            'End If

            'SQL &= ",PriceDesc = " & DB.Quote(PriceDesc) _
            '& ",IsNew = " & CInt(IsNew) _
            '& ",IsTaxFree = " & CInt(IsTaxFree) _
            '& ",IsOversize = " & CInt(IsOversize) _
            '& ",IsFreeShipping = " & CInt(IsFreeShipping) _
            '& ",IsOnSale = " & CInt(IsOnSale) _
            '& ",IsHazMat = " & CInt(IsHazMat) _
            '& ",IsFlammable = " & CInt(IsFlammable) _
            '& ",ItemName = " & DB.Quote(ItemName) _
            '& " WHERE " & IIf(ItemId <> Nothing, "ItemId = " & DB.Quote(ItemId), " SKU = " & DB.Quote(SKU)) & " and orderid in (select orderid from storeOrder where PaymentType is null)"
            ''& " WHERE " & IIf(ItemId <> Nothing, "ItemId = " & DB.Quote(ItemId), " SKU = " & DB.Quote(SKU))
            'm_DB.ExecuteSQL(SQL)

            If Not IsFlatFee Then
                m_DB.ExecuteSQL("Delete from FeeShippingState where ItemId=" & ItemId)
            End If

            UpdateQtyEbayItem(m_DB, ItemId, QtyOnHand)
            UpdateQtyAmazonItem(DB, ItemId, QtyOnHand)
            'Clear cache
            ClearItemCache(ItemId)

            'End Create Update StoreCartItem
        End Sub 'Update

        Public Function UpdateQtyEbayItem(ByVal _DB As Database, ByVal _ItemId As Integer, ByVal _QtyOnHand As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Ebay_ItemSell_UpdateQtyRevise"
                Dim cmd As SqlCommand = _DB.CreateCommand(sp)
                cmd.Parameters.Add(_DB.InParam("NailItemId", SqlDbType.Int, 0, _ItemId))
                cmd.Parameters.Add(_DB.InParam("QtyImport", SqlDbType.Int, 0, _QtyOnHand))
                cmd.Parameters.Add(_DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            Return result

        End Function
        Public Function UpdateQtyAmazonItem(ByVal _DB As Database, ByVal _ItemId As Integer, ByVal _QtyOnHand As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Amazon_ItemSell_UpdateQtyRevise"
                Dim cmd As SqlCommand = _DB.CreateCommand(sp)
                cmd.Parameters.Add(_DB.InParam("NailItemId", SqlDbType.Int, 0, _ItemId))
                cmd.Parameters.Add(_DB.InParam("QtyImport", SqlDbType.Int, 0, _QtyOnHand))
                cmd.Parameters.Add(_DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            Return result

        End Function
        Public Sub IPNUpdate()
            Dim SQL As String

            SQL = " UPDATE StoreItem SET " _
             & " ItemName = " & DB.Quote(ItemName) _
             & ", ItemNameNew = " & DB.Quote(ItemNameNew) _
             & ",ItemType = " & DB.Quote(ItemType) _
             & ",SKU = " & DB.Quote(SKU) _
              & ",URLCode = [dbo].[GenerateURLCode](" & DB.Quote(URLCode) & ")" _
             & ",Price = " & DB.Quote(Price) _
             & ",ItemGroupId = " & DB.NullNumber(ItemGroupId) _
             & ",Weight = " & DB.Number(Weight) _
             & ",SalePrice = " & DB.Quote(SalePrice) _
             & ",PriceDesc = " & DB.Quote(PriceDesc) _
             & ",PageTitle = " & DB.Quote(PageTitle) _
              & ",OutsideUSPageTitle = " & DB.Quote(OutsideUSPageTitle) _
             & ",MetaKeywords = " & DB.Quote(MetaKeywords) _
             & ",MetaDescription = " & DB.Quote(MetaDescription) _
               & ",OutsideUSMetaDescription = " & DB.Quote(OutsideUSMetaDescription) _
             & ",IsActive = " & CInt(IsActive) _
             & ",IsNew = " & CInt(IsNew) _
             & ",IsBestSeller = " & CInt(IsBestSeller) _
             & ",NewUntil = " & DB.Quote(NewUntil) _
             & ",ShipmentDate = " & DB.Quote(ShipmentDate) _
             & ",IsTaxFree = " & CInt(IsTaxFree) _
             & ",Image = " & DB.Quote(Image) _
             & ",ImageAltTag = " & DB.Quote(ImageAltTag) _
             & ",DeliveryTime = " & DB.Quote(DeliveryTime) _
             & ",InvMsgId = " & m_DB.NullQuote(InvMsgId) _
             & ",CarrierType = " & DB.Quote(CarrierType) _
             & ",Status = " & DB.Quote(Status) _
             & ",QtyOnHand = " & DB.Quote(QtyOnHand) _
             & ",InventoryStockNotification = " & DB.NullNumber(InventoryStockNotification) _
             & ",BODate = " & DB.NullQuote(BODate) _
             & ",LowStockMsg = " & DB.Quote(LowStockMsg) _
             & ",LowStockThreshold = " & DB.NullNumber(LowStockThreshold) _
             & ",QtyReserved = " & DB.Quote(QtyReserved) _
             & ",LastUpdated = " & DB.Quote(LastUpdated) _
             & ",ModifyDate = " & DB.Quote(Now()) _
             & ",ShortDesc = " & DB.Quote(ShortDesc) _
             & ",LongDesc = " & DB.Quote(LongDesc) _
             & ",ShortViet = " & DB.NQuote(ShortViet) _
             & ",LongViet = " & DB.NQuote(LongViet) _
             & ",ShortFrench = " & m_DB.NQuote(ShortFrench) _
             & ",LongFrench = " & m_DB.NQuote(LongFrench) _
             & ",ShortSpanish = " & m_DB.NQuote(ShortSpanish) _
             & ",LongSpanish = " & m_DB.NQuote(LongSpanish) _
             & ",AdditionalInfo = " & DB.Quote(AdditionalInfo) _
             & ",Specifications = " & DB.Quote(Specifications) _
             & ",ShippingInfo = " & DB.Quote(ShippingInfo) _
             & ",HelpfulTips = " & DB.Quote(HelpfulTips) _
             & ",MSDS = " & DB.Quote(MSDS) _
             & ",Prefix = " & DB.Quote(Prefix) _
             & ",IsFeatured = " & CInt(IsFeatured) _
             & ",IsOnSale = " & CInt(IsOnSale) _
             & ",BrandId = " & DB.NullNumber(BrandId) _
             & ",Category = " & DB.Quote(Category) _
             & ",LastImport = " & DB.Quote(LastImport) _
             & ",LastExport = " & DB.Quote(LastExport) _
             & ",DoExport = " & CInt(DoExport) _
             & ",IsCollection = " & CInt(IsCollection) _
             & ",IsOversize = " & CInt(IsOversize) _
             & ",IsHazMat = " & CInt(IsHazMat) _
             & ",MaximumQuantity = " & DB.NullNumber(MaximumQuantity) _
             & ",IsRushDelivery = " & CInt(IsRushDelivery) _
             & ",RushDeliveryCharge = " & DB.Number(RushDeliveryCharge) _
             & ",IsHot = " & CInt(IsHot) _
             & ",LiftGateCharge = " & DB.Number(LiftGateCharge) _
             & ",ScheduleDeliveryCharge = " & DB.Number(ScheduleDeliveryCharge) _
             & ",IsSpecialOrder = " & CInt(IsSpecialOrder) _
             & ",AcceptingOrder = " & CInt(AcceptingOrder) _
             & ",TaxGroupCode = " & DB.Quote(TaxGroupCode) _
             & ",IsFreeShipping = " & CInt(IsFreeShipping) _
             & ",PromotionId = " & m_DB.NullNumber(PromotionId) _
             & ",Measurement = " & DB.Quote(Measurement) _
             & ",IsFreeSample = " & CInt(IsFreeSample) _
             & ",IsFreeGift = " & CInt(IsFreeGift) _
              & ",IsFlatFee = " & CInt(IsFlatFee) _
             & ",IsEbayAllow = " & CInt(IsEbayAllow) _
              & ",FeeShipOversize = " & FeeShipOversize _
              & ",IsFlammable = " & CInt(IsFlammable) _
              & ",EbayShippingType = " & DB.Quote(EbayShippingType) _
            & ",IsRewardPoints = " & IIf(IsRewardPoints, "1", "0") _
              & ",RewardPoints = " & DB.Number(RewardPoints) _
            & ",ArrangeRewardPoints = " & DB.Number(ArrangeRewardPoints) _
              & ",EbayPrice = " & DB.Quote(EbayPrice) _
              & ",CaseQty = " & DB.Quote(CaseQty) _
              & ",CasePrice = " & DB.Quote(CasePrice) _
              & ",ChoiceName = " & DB.Quote(ChoiceName) _
               & ",IsFirstClassPackage = " & CInt(IsFirstClassPackage) _
             & " WHERE " & IIf(ItemId <> Nothing, "ItemId = " & DB.Quote(ItemId), " SKU = " & DB.Quote(SKU))

            m_DB.ExecuteSQL(SQL)
            UpdateGroup()

        End Sub

        Public Sub IPNUpdateInstore()
            If Not CheckDescriptionUpdate() Then
                Exit Sub
            End If
            Dim SQL As String = "exec dbo.sp_StoreItem_Update " & ItemId
            SQL = SQL & ",'" & DB.Quote(ItemName) & "',"
            SQL = SQL & ",'" & DB.Quote(ItemNameNew) & "',"
            SQL = SQL & ",'" & DB.Quote(ItemType) & "',"
            SQL = SQL & ",'" & DB.Quote(SKU) & "',"
            SQL = SQL & "," & DB.Quote(Price) & ","
            SQL = SQL & "," & DB.NullNumber(ItemGroupId) & ","
            SQL = SQL & "," & DB.Number(Weight) & ","
            SQL = SQL & "," & DB.Quote(SalePrice) & ","
            SQL = SQL & ",'" & DB.Quote(PriceDesc) & "',"
            SQL = SQL & ",'" & DB.Quote(PageTitle) & "',"
            SQL = SQL & ",'" & DB.Quote(MetaKeywords) & "',"
            SQL = SQL & ",'" & DB.Quote(MetaDescription) & "',"
            SQL = SQL & "," & Convert.ToInt32(IsActive) & ","
            SQL = SQL & "," & Convert.ToInt32(IsNew) & ","
            SQL = SQL & "," & Convert.ToInt32(IsBestSeller) & ","
            SQL = SQL & ",'" & DB.Quote(NewUntil) & "',"
            SQL = SQL & ",'" & DB.Quote(ShipmentDate) & "',"
            SQL = SQL & "," & Convert.ToInt32(IsTaxFree) & ","
            SQL = SQL & ",'" & DB.Quote(Image) & "',"
            SQL = SQL & ",'" & DB.Quote(ImageAltTag) & "',"
            SQL = SQL & ",'" & DB.Quote(DeliveryTime) & "',"
            SQL = SQL & "," & DB.NullQuote(InvMsgId) & ","
            SQL = SQL & ",'" & DB.Quote(CarrierType) & "',"
            SQL = SQL & ",'" & DB.Quote(Status) & "',"
            SQL = SQL & "," & DB.Quote(QtyOnHand) & ","
            SQL = SQL & "," & DB.NullNumber(InventoryStockNotification) & ","
            SQL = SQL & ",'" & DB.NullQuote(BODate) & "',"
            SQL = SQL & ",'" & DB.Quote(LowStockMsg) & "',"
            SQL = SQL & "," & DB.NullNumber(LowStockThreshold) & ","
            SQL = SQL & "," & DB.Quote(QtyReserved) & ","
            SQL = SQL & ",'" & DB.Quote(LastUpdated) & "',"
            SQL = SQL & ",'" & DB.Quote(Now()) & "',"
            SQL = SQL & ",'" & DB.Quote(ShortDesc) & "',"
            SQL = SQL & ",'" & DB.Quote(LongDesc) & "',"
            SQL = SQL & ",'" & DB.NQuote(ShortViet) & "',"
            SQL = SQL & ",'" & DB.NQuote(LongViet) & "',"
            SQL = SQL & ",'" & DB.NQuote(ShortFrench) & "',"
            SQL = SQL & ",'" & DB.NQuote(LongFrench) & "',"
            SQL = SQL & ",'" & DB.NQuote(ShortSpanish) & "',"
            SQL = SQL & ",'" & DB.NQuote(LongSpanish) & "',"
            SQL = SQL & ",'" & DB.Quote(AdditionalInfo) & "',"
            SQL = SQL & ",'" & DB.Quote(Specifications) & "',"
            SQL = SQL & ",'" & DB.Quote(ShippingInfo) & "',"
            SQL = SQL & ",'" & DB.Quote(HelpfulTips) & "',"
            SQL = SQL & ",'" & DB.Quote(MSDS) & "',"
            SQL = SQL & ",'" & DB.Quote(Prefix) & "',"
            SQL = SQL & "," & Convert.ToInt32(IsFeatured) & ","
            SQL = SQL & "," & Convert.ToInt32(IsOnSale) & ","
            SQL = SQL & "," & DB.NullNumber(BrandId) & ","
            SQL = SQL & ",'" & DB.Quote(Category) & "',"
            SQL = SQL & ",'" & DB.Quote(LastImport) & "',"
            SQL = SQL & ",'" & DB.Quote(LastExport) & "',"
            SQL = SQL & "," & Convert.ToInt32(DoExport) & ","
            SQL = SQL & "," & Convert.ToInt32(IsCollection) & ","
            SQL = SQL & "," & Convert.ToInt32(IsOversize) & ","
            SQL = SQL & "," & Convert.ToInt32(IsHazMat) & ","
            SQL = SQL & "," & DB.NullNumber(MaximumQuantity) & ","
            SQL = SQL & "," & Convert.ToInt32(IsRushDelivery) & ","
            SQL = SQL & "," & DB.Number(RushDeliveryCharge) & ","
            SQL = SQL & "," & Convert.ToInt32(IsHot) & ","
            SQL = SQL & "," & DB.Number(ScheduleDeliveryCharge) & ","
            SQL = SQL & "," & Convert.ToInt32(IsSpecialOrder) & ","
            SQL = SQL & ",'" & DB.Quote(TaxGroupCode) & "',"
            SQL = SQL & "," & Convert.ToInt32(IsFreeShipping) & ","
            SQL = SQL & "," & m_DB.NullNumber(PromotionId) & ","
            SQL = SQL & ",'" & DB.Quote(Measurement) & "',"
            SQL = SQL & "," & Convert.ToInt32(IsFreeSample) & ","
            SQL = SQL & "," & IsFreeGift
            m_DB.ExecuteSQL(SQL)
            UpdateGroup()
        End Sub
        Public Shared Function GetRowUpdateForCart(ByVal _Database As Database, ByVal ItemId As Integer) As StoreItemRow
            Dim row As StoreItemRow
            Dim key As String = String.Format(cachePrefixKey & "GetRowUpdateForCart_{0}_", ItemId)
            row = CType(CacheUtils.GetCache(key), StoreItemRow)
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If
            row = New StoreItemRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "Select ItemId,ItemGroupId,ItemName, ItemNameNew ,Price,Weight ,SalePrice ,IsNew,IsOnSale,NewUntil ,IsTaxFree ,Image,Status,SKU,PriceDesc,IsOversize,IsHazMat,IsFreeShipping,IsFreeSample from StoreItem where ItemId=" & ItemId
                r = _Database.GetReader(SQL)
                If r.Read() Then
                    row.ItemId = r.Item("ItemId")
                    If r.Item("ItemGroupId") Is Convert.DBNull Then
                        row.ItemGroupId = Nothing
                    Else
                        row.ItemGroupId = r.Item("ItemGroupId")
                    End If
                    row.ItemName = Convert.ToString(r.Item("ItemName"))
                    row.ItemNameNew = Convert.ToString(r.Item("ItemNameNew"))
                    If r.Item("Price") Is Convert.DBNull Then
                        row.Price = Nothing

                    Else
                        row.Price = Convert.ToDouble(r.Item("Price"))
                    End If
                    If IsDBNull(r.Item("Weight")) Then
                        row.Weight = Nothing
                    Else
                        row.Weight = Convert.ToDouble(r.Item("Weight"))
                    End If
                    If r.Item("SalePrice") Is Convert.DBNull Then
                        row.SalePrice = Nothing
                    Else
                        row.SalePrice = Convert.ToDouble(r.Item("SalePrice"))
                    End If
                    If r.Item("PriceDesc") Is Convert.DBNull Then
                        row.PriceDesc = Nothing
                    Else
                        row.PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                    End If
                    row.IsNew = Convert.ToBoolean(r.Item("IsNew"))
                    row.IsOnSale = Convert.ToBoolean(r.Item("IsOnSale"))
                    If r.Item("NewUntil") Is Convert.DBNull Then
                        row.NewUntil = Nothing
                    Else
                        row.NewUntil = Convert.ToDateTime(r.Item("NewUntil"))
                    End If
                    row.IsTaxFree = Convert.ToBoolean(r.Item("IsTaxFree"))
                    If r.Item("Image") Is Convert.DBNull Then
                        row.Image = Nothing
                    Else
                        row.Image = Convert.ToString(r.Item("Image"))
                    End If
                    If r.Item("Status") Is Convert.DBNull Then
                        row.Status = Nothing
                    Else
                        row.Status = Convert.ToString(r.Item("Status"))
                    End If
                    If r.Item("SKU") Is Convert.DBNull Then
                        row.SKU = Nothing
                    Else
                        row.SKU = Convert.ToString(r.Item("SKU"))
                    End If
                    row.IsOversize = Convert.ToBoolean(r.Item("IsOversize"))
                    row.IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                    row.IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                    row.IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("StoreItem.vb", String.Empty, ex)
            End Try
            CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
            Return row
        End Function


        Public Sub IPNUpdateFreeSamples()
            Dim SQL As String = "exec dbo.sp_StoreItem_UpdateFreeSample " & ItemId
            SQL = SQL & "," & Convert.ToInt32(IsFreeSample) & ""
            m_DB.ExecuteSQL(SQL)
        End Sub
        Public Shared Sub ChangeArrangeFreeSample(ByVal _Database As Database, ByVal ItemId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_StoreItem_ChangeArrangeFreeSample " & ItemId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
        End Sub

        Public Sub IPNUpdateItemPoint()
            Dim SQL As String = "exec dbo.sp_StoreItem_UpdateItemPoint " & ItemId
            SQL = SQL & "," & Convert.ToInt32(IsRewardPoints) & "," & Convert.ToInt32(RewardPoints) & ""
            m_DB.ExecuteSQL(SQL)
        End Sub
        Public Shared Sub ChangeArrangeItemPoint(ByVal _Database As Database, ByVal ItemId As Integer, ByVal IsUp As Boolean)
            Dim SQL As String = "exec dbo.sp_StoreItem_ChangeArrangeItemPoint " & ItemId & ", " & IsUp
            _Database.ExecuteSQL(SQL)
        End Sub
        Public Sub Remove()
            m_DB.ExecuteSQL("delete from storedepartmentitem where ItemId = " & DB.Quote(ItemId))
            m_DB.ExecuteSQL("delete from ViewedItem where ItemId = " & DB.Quote(ItemId))
            m_DB.ExecuteSQL("delete from AlbumItem where ItemId = " & DB.Quote(ItemId))
            m_DB.ExecuteSQL("DELETE FROM StoreItem WHERE ItemId = " & DB.Quote(ItemId))
            ClearItemCache(ItemId)
        End Sub 'Remove
        Public Shared Sub ClearItemCache(ByVal ItemId As Integer)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey, SalesCategoryItemRow.cachePrefixKey, StoreItemReviewRow.cachePrefixKey)
        End Sub
    End Class

    Public Class StoreItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreItem As StoreItemRow)
            Me.List.Add(StoreItem)
        End Sub

        Public Function Contains(ByVal StoreItem As StoreItemRow) As Boolean
            Return Me.List.Contains(StoreItem)
        End Function

        Public Function IndexOf(ByVal StoreItem As StoreItemRow) As Integer
            Return Me.List.IndexOf(StoreItem)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreItem As StoreItemRow)
            Me.List.Insert(Index, StoreItem)
        End Sub
        Public Function GetXml(ByVal sic As StoreItemCollection) As Xml.Formatting

        End Function
        Default Public Property Item(ByVal Index As Integer) As StoreItemRow
            Get
                Return CType(Me.List.Item(Index), StoreItemRow)
            End Get

            Set(ByVal Value As StoreItemRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreItem As StoreItemRow)
            Me.List.Remove(StoreItem)
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
        Private m_IsQuickOrder As Boolean
        Public Property IsQuickOrder() As Boolean
            Get
                Return m_IsQuickOrder
            End Get
            Set(ByVal value As Boolean)
                m_IsQuickOrder = value
            End Set
        End Property

        Private m_HasNewItem As Boolean
        Public Property HasNewItem() As Boolean
            Get
                Return m_HasNewItem
            End Get
            Set(ByVal value As Boolean)
                m_HasNewItem = value
            End Set
        End Property

        Private m_enableHasNewItem As Boolean
        Public Property EnableHasNewItem() As Boolean
            Get
                Return m_enableHasNewItem
            End Get
            Set(ByVal value As Boolean)
                m_enableHasNewItem = value
            End Set
        End Property


    End Class
End Namespace


