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
Imports System.Web.UI.WebControls

Namespace DataLayer
    Public Class StoreFeatureRow
        Inherits StoreFeatureBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As StoreFeatureRow
            Dim row As StoreFeatureRow
            row = New StoreFeatureRow(_Database, id)
            row.Load()
            Return row
        End Function

        'Khoa viet rieng ko tra ve data set
        Public Shared Function GetAllFeatures(ByVal filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As List(Of ListItem)
            Dim SQL As String = String.Empty

            If (url.Contains("/nail-supply/") Or url.Contains("store/default.aspx") Or url.Contains("store/sub-category.aspx") Or url.Contains("/nail-brand/") Or url.Contains("/nail-collection/") Or url.Contains("store/collection.aspx")) Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                SQL &= " Select "
                SQL &= " ci.FeatureId from storeitem si inner join Storeitemfeature ci on ci.itemid = si.itemid" & vbCrLf
                SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    SQL &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If
                'Long edit Sales & special
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    SQL &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    SQL &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                'End
                If filter.SalesCategoryId <> Nothing Then
                    ' SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                    SQL &= " and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                SQL &= " where si.IsActive = 1 " & vbCrLf
                If filter.Keyword = Nothing Then
                    SQL &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    SQL &= " and " & vbCrLf
                    SQL &= "kw.KeywordName=" & Database.Quote(filter.Keyword) & vbCrLf
                Else
                    If filter.Keyword <> Nothing Then
                        SQL &= " and " & vbCrLf
                        SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & Database.Quote(filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not filter.LoggedInPostingGroup = Nothing Then
                    SQL &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    SQL &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    SQL &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    SQL &= " and si.BrandId = " & Database.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If

                If Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        SQL &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        SQL &= " and " & lowSalePriceExp & " >= " & low
                    End If
                End If
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        SQL &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        SQL &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If
                End If
                If filter.HasPromotion Then

                    If filter.Sale24Hour Then
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & Database.Quote(Now.ToShortDateString) & " and mm.endingdate < " & Database.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf filter.SaleBuy1Get1 Then
                        SQL &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If
                SQL = "select  FeatureName,URLCode from StoreFeature where FeatureId in( " & SQL & ") order by name"
            Else


            End If

            Dim dr As SqlDataReader
            Dim list As New List(Of ListItem)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try

                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                dr = db.ExecuteReader(cmd)

                While dr.Read()
                    Dim item As New ListItem(dr("FeatureName").ToString(), dr("FeatureId").ToString())
                    list.Add(item)
                End While

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
        End Function

        Public Shared Function GetAllFeatures(ByVal DB As Database, ByVal filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As DataSet
            Dim sqlItem As String = String.Empty
            Dim sql As String = String.Empty

            If (url.Contains("nail-sales-promotion") Or url.Contains("store/category.aspx")) Then
                Dim GroupItems As Boolean = False
                sqlItem &= " Select FeatureId from(Select "
                sqlItem &= " si.itemid,ci.FeatureId, " & vbCrLf & _
                "case when si.itemgroupid is not null then (select min(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as lowprice, " & vbCrLf & _
                "coalesce(sp.unitprice,si.price) as lowsaleprice " & vbCrLf
                sqlItem &= " from storeitem si inner join Storeitemfeature ci on ci.itemid = si.itemid" & vbCrLf
                sqlItem &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    sqlItem &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                sqlItem &= " where si.IsActive = 1 and si.brandid <> 0 " & vbCrLf
                If filter.Keyword = Nothing Then
                    sqlItem &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If GroupItems Then sqlItem &= " and si.itemgroupid is null " & vbCrLf
                If Not filter.LoggedInPostingGroup = Nothing Then
                    sqlItem &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    sqlItem &= " and si.BrandId = " & DB.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If


                sqlItem &= ") tmp1 " & vbCrLf
                If filter.HasPromotion Then
                    sqlItem &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "
                End If

                filter.SortOrder = ""

                If filter.SortBy = String.Empty Then
                    filter.SortBy = " isFeatured desc, Lowprice asc, itemname asc "
                End If
                sql = "select  name,URLCode from StoreFeature where FeatureId in( " & sqlItem & ") order by name"
                Return DB.GetDataSet(sql)
            ElseIf (url.Contains("/nail-supply/") Or url.Contains("store/default.aspx") Or url.Contains("/nail-brand/")) Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                sqlItem &= " Select "
                sqlItem &= " ci.FeatureId from storeitem si inner join Storeitemfeature ci on ci.itemid = si.itemid" & vbCrLf
                sqlItem &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                    sqlItem &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
                End If
                'Long edit Sales & special
                If filter.HasPromotion AndAlso filter.DepartmentId <= 23 Then
                    sqlItem &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    sqlItem &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    sqlItem &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                'End
                If filter.SalesCategoryId <> Nothing Then
                    ' SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                    sqlItem &= " and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                End If
                sqlItem &= " where si.IsActive = 1 " & vbCrLf
                If filter.Keyword = Nothing Then
                    sqlItem &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If filter.IsSearchKeyWord Then
                    sqlItem &= " and " & vbCrLf
                    sqlItem &= "kw.KeywordName=" & DB.Quote(filter.Keyword) & vbCrLf
                Else
                    If filter.Keyword <> Nothing Then
                        sqlItem &= " and " & vbCrLf
                        sqlItem &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not filter.LoggedInPostingGroup = Nothing Then
                    sqlItem &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If filter.IsFeatured Then
                    sqlItem &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If filter.IsNew Then
                    sqlItem &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If filter.IsHot Then
                    sqlItem &= " and si.IsHot = 1 " & vbCrLf
                End If
                If filter.IsOnSale Then
                    sqlItem &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not filter.BrandId = 0 Then
                    sqlItem &= " and si.BrandId = " & DB.Number(filter.BrandId) & vbCrLf
                End If
                If Not filter.CollectionId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & filter.CollectionId & ") " & vbCrLf
                End If
                If Not filter.ToneId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & filter.ToneId & ") " & vbCrLf
                End If
                If Not filter.ShadeId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & filter.ShadeId & ") " & vbCrLf
                End If

                'If Not filter.Feature = String.Empty Then
                '    sqlItem &= " and si.ItemId in (select itemid from storeitemfeaturefilter with (nolock) where URLCode = " & DB.Quote(filter.Feature) & ") " & vbCrLf
                'End If
                'If Not filter.PriceRange = String.Empty Then
                '    Dim low, high As Integer
                '    Dim a() As String = filter.PriceRange.Split("-")
                '    If UBound(a) = 1 AndAlso IsNumeric(a(0)) AndAlso IsNumeric(a(1)) Then
                '        low = CInt(a(0))
                '        high = CInt(a(1))
                '        sqlItem &= " and si.price between " & low & " and " & high
                '    End If
                'End If

                If Not filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.PriceRange, True)
                    If (high > 0) Then
                        sqlItem &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        sqlItem &= " and " & lowSalePriceExp & " >= " & low
                    End If
                End If
                If Not filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(filter.RatingRange, True)
                    If (high > 0) Then
                        sqlItem &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        sqlItem &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If

                    'End If
                End If
                If filter.HasPromotion Then

                    If filter.Sale24Hour Then
                        sqlItem &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & DB.Quote(Now.ToShortDateString) & " and mm.endingdate < " & DB.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf filter.SaleBuy1Get1 Then
                        sqlItem &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        sqlItem &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If
                sql = "select  name,URLCode from StoreFeature where FeatureId in( " & sqlItem & ") order by name"
                Return DB.GetDataSet(sql)
            Else
                Return GetAllFeatures(DB, filter)
            End If
        End Function
        Public Shared Function GetAllFeatures(ByVal DB As Database, ByVal filter As DepartmentFilterFields) As DataSet
            ''   Dim SQL As String = "select distinct " & IIf(filter.All, "", " top " & filter.MaxPerPage * filter.pg) & " name,URLCode from Storeitemfeaturefilter where "
            Dim SQL As String = "select FeatureId from StoreItemFeature where "


            'Long add 03/29/2011
            If filter.HasPromotion Then
                SQL &= " ItemId in (Select sci.itemid from SalesCategoryItem sci inner join StoreItem si on sci.ItemId = si.ItemId where si.IsActive = 1) and "
            End If
            'End
            SQL &= "  itemid in ( select si.itemid from storeitem si with (nolock) " & vbCrLf
            If filter.DepartmentId <> Nothing AndAlso filter.DepartmentId <> 23 Then
                SQL &= " 	inner join storedepartmentitem sd with (nolock) on si.itemid = sd.itemid and sd.departmentid = " & filter.DepartmentId
            End If

            If filter.IsSearchKeyWord = True Then
                SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                SQL &= "  inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
            End If

            If filter.HasPromotion OrElse filter.PromotionId <> Nothing Then SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity > 0 and getdate() between coalesce(startingdate,getdate()) and coalesce(endingdate,getdate()+1) and ((salestype = 0 and memberid = " & filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and (select top 1 CustomerPriceGroupId from customer with (nolock) where customerid = (select top 1 customerid from member with (nolock) where memberid = " & filter.MemberId & ")) = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf

            SQL &= " where si.isactive = 1 " & vbCrLf
            If filter.IsSearchKeyWord Then
                SQL &= " and " & vbCrLf
                SQL &= "kw.KeywordName=" & DB.Quote(filter.Keyword) & vbCrLf
            Else
                If filter.Keyword <> Nothing Then
                    SQL &= " and " & vbCrLf
                    SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(filter.Keyword) & ")) " & vbCrLf
                End If
            End If

            If filter.IsFeatured Then
                SQL &= " and si.IsFeatured = 1" & vbCrLf
            End If
            If filter.IsNew Then
                SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
            End If
            If filter.IsOnSale Then
                SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
            End If
            If Not filter.BrandId = 0 Then
                SQL &= " and si.BrandId = " & DB.Number(filter.BrandId) & vbCrLf
            End If
            'If Not Filter.CollectionId = 0 Then
            '	SQL &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & Filter.CollectionId & ") " & vbCrLf
            'End If
            'If Not Filter.ToneId = 0 Then
            '	SQL &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & Filter.ToneId & ") " & vbCrLf
            'End If
            'If Not Filter.ShadeId = 0 Then
            '	SQL &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & Filter.ShadeId & ") " & vbCrLf
            'End If
            If filter.HasPromotion OrElse filter.PromotionId <> Nothing Then
                SQL &= " and (" & vbCrLf & _
                 " (select top 1 mixmatchid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.isactive = 1 and not exists (select itemid from freegift with (nolock) where isactive = 1 and itemid = mml.itemid) and mml.itemid = si.itemid) is not null " & vbCrLf & _
                 " or sp.unitprice is not null) " & vbCrLf
            End If
            SQL &= ")"
            SQL = "Select Name,URLCode from StoreFeature where FeatureId in(" & SQL & ") order by name"
            Return DB.GetDataSet(SQL)
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As StoreFeatureRow
            Dim result As New StoreFeatureRow
            If (Not reader.IsDBNull(reader.GetOrdinal("FeatureId"))) Then
                result.FeatureId = Convert.ToInt32(reader("FeatureId"))
            Else
                result.FeatureId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                result.Name = reader("Name").ToString()
            Else
                result.Name = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                result.URLCode = reader("URLCode").ToString()
            Else
                result.URLCode = ""
            End If
            Return result
        End Function

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreFeature_Delete")
                db.AddInParameter(cmd, "FeatureId", DbType.Int32, Id)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result

        End Function

        Public Shared Function Insert(ByVal data As StoreFeatureRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreFeature_Insert")
                db.AddInParameter(cmd, "Name", DbType.String, data.Name)
                db.AddInParameter(cmd, "URLCode", DbType.String, data.URLCode)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function Update(ByVal data As StoreFeatureRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreFeature_Update")
                db.AddInParameter(cmd, "FeatureId", DbType.Int32, data.FeatureId)
                db.AddInParameter(cmd, "Name", DbType.String, data.Name)
                db.AddInParameter(cmd, "URLCode", DbType.String, data.URLCode)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
    End Class


    Public MustInherit Class StoreFeatureBase
        Private m_DB As Database
        Private m_FeatureId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_URLCode As String = Nothing
        Public Property FeatureId() As Integer
            Get
                Return m_FeatureId
            End Get
            Set(ByVal Value As Integer)
                m_FeatureId = Value
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

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_FeatureId = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreFeature WHERE FeatureId = " & m_DB.Number(m_FeatureId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try


        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try

                If (Not reader Is Nothing And Not reader.IsClosed) Then

                    If (Not reader.IsDBNull(reader.GetOrdinal("FeatureId"))) Then
                        m_FeatureId = Convert.ToInt32(reader("FeatureId"))
                    Else
                        m_FeatureId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        m_Name = reader("Name").ToString()
                    Else
                        m_Name = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                        m_URLCode = reader("URLCode").ToString()
                    Else
                        m_URLCode = ""
                    End If

                End If
            Catch ex As Exception
                Throw ex
                ''  
            End Try

        End Sub

    End Class

    Public Class StoreFeatureCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As StoreFeatureRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreFeatureRow
            Get
                Return CType(Me.List.Item(Index), StoreFeatureRow)
            End Get

            Set(ByVal Value As StoreFeatureRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace



