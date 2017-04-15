Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:13:06 PM

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
Imports System.Web.UI.WebControls

Namespace DataLayer

    Public Class StoreToneRow
        Inherits StoreToneRowBase

        Private Shared cachePrefixKey As String = "StoreTone_"

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ToneId As Integer)
            MyBase.New(DB, ToneId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Tone As String)
            MyBase.New(DB, Tone)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetToneFilter(ByVal tondeIds As String) As List(Of ListItem)
            If String.IsNullOrEmpty(tondeIds) Then
                Return New List(Of ListItem)()
            End If

            Dim List As List(Of ListItem) = New List(Of ListItem)()
            Dim dr = Nothing
            Dim SQL As String = "select ToneId, Tone from StoreTone a where exists (select 1 from dbo.SplitString('" + tondeIds + "',',') b where a.toneid = LTRIM(RTRIM(part)))"
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                dr = db.ExecuteReader(cmd)

                While dr.Read()
                    Dim item As New ListItem(dr("Tone").ToString(), dr("ToneId").ToString())
                    List.Add(item)
                End While
                Return List
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetToneFilter", ex)
                Return New List(Of ListItem)()
            End Try
        End Function

        Public Shared Function GetStoreToneForSearch() As List(Of String)
            Dim c As List(Of String)
            Dim key As String = String.Format(cachePrefixKey & "GetStoreToneForSearch")

            c = CType(CacheUtils.GetCache(key), List(Of String))
            If Not c Is Nothing Then
                Return c
            End If

            c = New List(Of String)
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "select ToneId, replace(Tone, ' and ', ' & ') as Tone from StoreTone"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SP)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    c.Add(r.Item("Tone").ToString().ToLower().Trim())
                End While
                Core.CloseReader(r)

                CacheUtils.SetCache(key, c, Utility.ConfigData.TimeCacheData)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetStoreToneForSearch", ex)
            End Try

            Return c
        End Function
        Public Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreTone.vb", func, ex)
        End Sub

        Public Shared Function GetRow(ByVal DB As Database, ByVal ToneId As Integer) As StoreToneRow
            Dim row As StoreToneRow

            row = New StoreToneRow(DB, ToneId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal Tone As String) As StoreToneRow
            Dim row As StoreToneRow

            row = New StoreToneRow(DB, Tone)
            row.LoadByName()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ToneId As Integer)
            Dim row As StoreToneRow

            row = New StoreToneRow(DB, ToneId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllTones(ByVal DB1 As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORETONE_GETLIST As String = "sp_StoreTone_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_GETLIST)

            Return db.ExecuteDataSet(cmd)

            '------------------------------------------------------------------------
        End Function
        'Khoa viet rieng ko tra ve dataset
        Public Shared Function GetAllTones(ByVal Filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As List(Of ListItem)
            Dim SQL As String = String.Empty
            If url.Contains("/nail-supply/") Or url.Contains("store/default.aspx") Or url.Contains("store/sub-category.aspx") Or url.Contains("store/collection.aspx") Or url.Contains("/nail-brand/") Or url.Contains("/nail-collection/") Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                SQL &= " Select "
                SQL &= " ci.toneid from storeitem si inner join storetoneitem ci on ci.itemid = si.itemid" & vbCrLf
                SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(Filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
                    SQL &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
                End If
                'Long edit Sales & special
                If Filter.HasPromotion AndAlso Filter.DepartmentId <= 23 Then
                    SQL &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If Filter.IsSearchKeyWord Then
                    SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    SQL &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                'End
                If Filter.SalesCategoryId <> Nothing Then
                    SQL &= " and salescategoryid = " & Filter.SalesCategoryId & " " & vbCrLf
                End If
                SQL &= " where si.IsActive = 1 " & vbCrLf
                If Filter.Keyword = Nothing Then
                    SQL &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If Filter.IsSearchKeyWord Then
                    SQL &= " and " & vbCrLf
                    SQL &= "kw.KeywordName=" & Database.Quote(Filter.Keyword) & vbCrLf
                Else
                    If Filter.Keyword <> Nothing Then
                        SQL &= " and " & vbCrLf
                        SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & Database.Quote(Filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not Filter.LoggedInPostingGroup = Nothing Then
                    SQL &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & Database.Quote(Filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If Filter.IsFeatured Then
                    SQL &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If Filter.IsNew Then
                    SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If Filter.IsHot Then
                    SQL &= " and si.IsHot = 1 " & vbCrLf
                End If
                If Filter.IsOnSale Then
                    SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not Filter.BrandId = 0 Then
                    SQL &= " and si.BrandId = " & Database.Number(Filter.BrandId) & vbCrLf
                End If
                If Not Filter.CollectionId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & Filter.CollectionId & ") " & vbCrLf
                End If
                If Not Filter.ShadeId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & Filter.ShadeId & ") " & vbCrLf
                End If
                Try
                    If CInt(Filter.Feature) > 0 Then
                        SQL &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where FeatureId = " & Database.Number(Filter.Feature) & ") " & vbCrLf
                    End If
                Catch ex As Exception

                End Try

                If Not Filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(Filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(Filter.PriceRange, True)
                    If (high > 0) Then
                        SQL &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        SQL &= " and " & lowSalePriceExp & " >= " & low
                    End If
                End If
                If Not Filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(Filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(Filter.RatingRange, True)
                    If (high > 0) Then
                        SQL &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        SQL &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If
                End If
                If Filter.HasPromotion Then
                    If Filter.Sale24Hour Then
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & Database.Quote(Now.ToShortDateString) & " and mm.endingdate < " & Database.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf Filter.SaleBuy1Get1 Then
                        SQL &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        SQL &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & Database.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If
                SQL = "select ToneId, Tone from StoreTone where toneid in( " & SQL & ") order by Tone"
            Else
                SQL = "select ToneId, Tone from StoreTone where toneid in ("
                SQL &= " select toneid from storetoneitem sti with (nolock) inner join storeitem si with (nolock) on sti.itemid = si.itemid " & vbCrLf
                If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
                    SQL &= " 	inner join storedepartmentitem sd with (nolock) on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
                End If
                If Filter.IsSearchKeyWord = True Then
                    SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    SQL &= "  inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity > 0 and getdate() between coalesce(startingdate,getdate()) and coalesce(endingdate,getdate()+1) and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and (select top 1 CustomerPriceGroupId from customer with (nolock) where customerid = (select top 1 customerid from member with (nolock) where memberid = " & Filter.MemberId & ")) = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                SQL &= " where si.isactive = 1 " & vbCrLf

                If Filter.IsSearchKeyWord Then
                    SQL &= " and " & vbCrLf
                    SQL &= "kw.KeywordName=" & Database.Quote(Filter.Keyword) & vbCrLf
                Else
                    If Filter.Keyword <> Nothing Then
                        SQL &= " and " & vbCrLf
                        SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & Database.Quote(Filter.Keyword) & ")) " & vbCrLf
                    End If
                End If
                If Filter.IsFeatured Then
                    SQL &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If Filter.IsNew Then
                    SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If Filter.IsOnSale Then
                    SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not Filter.BrandId = 0 Then
                    SQL &= " and si.BrandId = " & Database.Number(Filter.BrandId) & vbCrLf
                End If
                If Not Filter.Feature = Nothing Then
                    SQL &= " and si.itemid in (select itemid from storeitemfeaturefilter where name = " & Database.Quote(Filter.Feature) & ") "
                End If
                If Filter.IsHot Then
                    SQL &= " and si.IsHot = 1 " & vbCrLf
                End If
                If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then
                    SQL &= " and (" & vbCrLf & _
                     " (select top 1 mixmatchid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.isactive = 1 and not exists (select itemid from freegift with (nolock) where isactive = 1 and itemid = mml.itemid) and mml.itemid = si.itemid) is not null " & vbCrLf & _
                     " or sp.unitprice is not null) " & vbCrLf
                End If
                SQL &= ") order by Tone"
            End If

            Dim dr As SqlDataReader
            Dim list As New List(Of ListItem)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try

                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                dr = db.ExecuteReader(cmd)

                While dr.Read()
                    Dim item As New ListItem(dr("Tone").ToString(), dr("ToneId").ToString())
                    list.Add(item)
                End While

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return list
        End Function

        Public Shared Function GetAllTones(ByVal DB As Database, ByVal Filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As DataSet
            Dim sqlItem As String = String.Empty
            Dim sql As String = String.Empty
            If (url.Contains("nail-sales-promotion") Or url.Contains("store/category.aspx")) Then
                Dim GroupItems As Boolean = False
                sqlItem &= " Select toneid from(Select "
                sqlItem &= " ci.toneid,si.itemid, " & vbCrLf & _
                "case when si.itemgroupid is not null then (select min(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as lowprice, " & vbCrLf & _
                "coalesce(sp.unitprice,si.price) as lowsaleprice " & vbCrLf
                sqlItem &= " from storeitem si inner join storetoneitem ci on ci.itemid = si.itemid" & vbCrLf
                sqlItem &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(Filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If Filter.HasPromotion AndAlso Filter.DepartmentId <= 23 Then
                    sqlItem &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                sqlItem &= " where si.IsActive = 1 and si.brandid <> 0 " & vbCrLf
                If Filter.Keyword = Nothing Then
                    sqlItem &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If GroupItems Then sqlItem &= " and si.itemgroupid is null " & vbCrLf
                If Not Filter.LoggedInPostingGroup = Nothing Then
                    sqlItem &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(Filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If Not Filter.BrandId = 0 Then
                    sqlItem &= " and si.BrandId = " & DB.Number(Filter.BrandId) & vbCrLf
                End If
                If Not Filter.CollectionId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & Filter.CollectionId & ") " & vbCrLf
                End If

                If Not Filter.ShadeId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & Filter.ShadeId & ") " & vbCrLf
                End If
                Try
                    If CInt(Filter.Feature) > 0 Then
                        sqlItem &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where FeatureId = " & DB.Number(Filter.Feature) & ") " & vbCrLf
                    End If
                Catch ex As Exception

                End Try


                sqlItem &= ") tmp1 " & vbCrLf
                If Filter.HasPromotion Then
                    sqlItem &= " where (lowsaleprice < lowprice or itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null))) "
                End If

                Filter.SortOrder = ""

                If Filter.SortBy = String.Empty Then
                    Filter.SortBy = " isFeatured desc, Lowprice asc, itemname asc "
                End If
                sql = "select * from StoreTone where toneid in( " & sqlItem & ") order by Tone"
                Return DB.GetDataSet(sql)
            ElseIf (url.Contains("/nail-supply/") Or url.Contains("store/default.aspx") Or url.Contains("/nail-brand/")) Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                sqlItem &= " Select "
                sqlItem &= " ci.toneid from storeitem si inner join storetoneitem ci on ci.itemid = si.itemid" & vbCrLf
                sqlItem &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(Filter.Sale24Hour, " and startingdate = " & DB.Quote(Now.ToShortDateString) & " and endingdate = " & DB.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
                    sqlItem &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
                End If
                'Long edit Sales & special
                If Filter.HasPromotion AndAlso Filter.DepartmentId <= 23 Then
                    sqlItem &= " inner join salescategoryitem sci on si.itemid = sci.itemid " & vbCrLf
                End If
                If Filter.IsSearchKeyWord Then
                    sqlItem &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                    sqlItem &= " inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
                End If
                'End
                If Filter.SalesCategoryId <> Nothing Then
                    ' SQLI &= " inner join salescategoryitem sci on si.itemid = sci.itemid and salescategoryid = " & filter.SalesCategoryId & " " & vbCrLf
                    sqlItem &= " and salescategoryid = " & Filter.SalesCategoryId & " " & vbCrLf
                End If
                sqlItem &= " where si.IsActive = 1 " & vbCrLf
                If Filter.Keyword = Nothing Then
                    sqlItem &= " AND si.IsFreeSample = 0 AND si.IsFreeGift < 2 " & vbCrLf
                End If
                If Filter.IsSearchKeyWord Then
                    sqlItem &= " and " & vbCrLf
                    sqlItem &= "kw.KeywordName=" & DB.Quote(Filter.Keyword) & vbCrLf
                Else
                    If Filter.Keyword <> Nothing Then
                        sqlItem &= " and " & vbCrLf
                        sqlItem &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(Filter.Keyword) & ")) " & vbCrLf
                    End If
                End If

                If Not Filter.LoggedInPostingGroup = Nothing Then
                    sqlItem &= " and si.itemid not in (select itemid from storeitemcustomerpostinggroup where code = " & DB.Quote(Filter.LoggedInPostingGroup) & ") " & vbCrLf
                End If
                If Filter.IsFeatured Then
                    sqlItem &= " and si.IsFeatured = 1" & vbCrLf
                End If
                If Filter.IsNew Then
                    sqlItem &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
                End If
                If Filter.IsHot Then
                    sqlItem &= " and si.IsHot = 1 " & vbCrLf
                End If
                If Filter.IsOnSale Then
                    sqlItem &= " and (si.IsOnSale = 1 ) " & vbCrLf
                End If
                If Not Filter.BrandId = 0 Then
                    sqlItem &= " and si.BrandId = " & DB.Number(Filter.BrandId) & vbCrLf
                End If
                If Not Filter.CollectionId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & Filter.CollectionId & ") " & vbCrLf
                End If
                If Not Filter.ShadeId = 0 Then
                    sqlItem &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & Filter.ShadeId & ") " & vbCrLf
                End If
                Try
                    If CInt(Filter.Feature) > 0 Then
                        '' sqlItem &= " and si.ItemId in (select itemid from storeitemfeaturefilter with (nolock) where URLCode = " & DB.Quote(Filter.Feature) & ") " & vbCrLf
                        sqlItem &= " and si.ItemId in (select itemid from StoreItemFeature with (nolock) where FeatureId = " & DB.Number(Filter.Feature) & ") " & vbCrLf

                    End If
                Catch ex As Exception

                End Try

                If Not Filter.PriceRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(Filter.PriceRange, False)
                    Dim high As Double = Common.GetMinMaxValue(Filter.PriceRange, True)
                    If (high > 0) Then
                        sqlItem &= " and " & lowSalePriceExp & " >= " & low & " and " & lowSalePriceExp & " < " & high
                    Else
                        sqlItem &= " and " & lowSalePriceExp & " >= " & low
                    End If
                End If
                If Not Filter.RatingRange = String.Empty Then
                    Dim low As Double = Common.GetMinMaxValue(Filter.RatingRange, False)
                    Dim high As Double = Common.GetMinMaxValue(Filter.RatingRange, True)
                    If (high > 0) Then
                        sqlItem &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low & " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) < " & high
                    Else
                        sqlItem &= " and dbo.fc_StoreItem_GetTopRatedSort( si.itemid ) >= " & low
                    End If

                    'End If
                End If
                If Filter.HasPromotion Then

                    If Filter.Sale24Hour Then
                        sqlItem &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.startingdate >= " & DB.Quote(Now.ToShortDateString) & " and mm.endingdate < " & DB.Quote(Now.AddDays(1).ToShortDateString) & " and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    ElseIf Filter.SaleBuy1Get1 Then
                        sqlItem &= " and si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.mandatory > 0 and mm.optional > 0 and getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    Else
                        sqlItem &= " and " & lowSalePriceExp & " < " & LowPriceExp & " or si.itemid in (select itemid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where getdate() between coalesce(mm.startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(mm.endingdate,getdate()+1) and mm.isactive = 1 AND (mm.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or mm.CustomerPriceGroupId is null)) "
                    End If
                End If
                sql = "select * from StoreTone where toneid in( " & sqlItem & ") order by Tone"
                Return DB.GetDataSet(sql)
            Else
                Return GetAllTones(DB, Filter)
            End If
        End Function
        Public Shared Function GetAllTones(ByVal DB As Database, ByVal Filter As DepartmentFilterFields) As DataSet
            Dim SQL As String = "select * from StoreTone where toneid in ("

            SQL &= " select toneid from storetoneitem sti with (nolock) inner join storeitem si with (nolock) on sti.itemid = si.itemid " & vbCrLf
            If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
                SQL &= " 	inner join storedepartmentitem sd with (nolock) on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
            End If
            If Filter.IsSearchKeyWord = True Then
                SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
                SQL &= "  inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
            End If
            If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity > 0 and getdate() between coalesce(startingdate,getdate()) and coalesce(endingdate,getdate()+1) and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and (select top 1 CustomerPriceGroupId from customer with (nolock) where customerid = (select top 1 customerid from member with (nolock) where memberid = " & Filter.MemberId & ")) = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
            SQL &= " where si.isactive = 1 " & vbCrLf

            If Filter.IsSearchKeyWord Then
                SQL &= " and " & vbCrLf
                SQL &= "kw.KeywordName=" & DB.Quote(Filter.Keyword) & vbCrLf
            Else
                If Filter.Keyword <> Nothing Then
                    SQL &= " and " & vbCrLf
                    SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(Filter.Keyword) & ")) " & vbCrLf
                End If
            End If



            If Filter.IsFeatured Then
                SQL &= " and si.IsFeatured = 1" & vbCrLf
            End If
            If Filter.IsNew Then
                SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
            End If
            If Filter.IsOnSale Then
                SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
            End If
            If Not Filter.BrandId = 0 Then
                SQL &= " and si.BrandId = " & DB.Number(Filter.BrandId) & vbCrLf
            End If
            If Not Filter.Feature = Nothing Then
                SQL &= " and si.itemid in (select itemid from storeitemfeaturefilter where name = " & DB.Quote(Filter.Feature) & ") "
            End If
            If Filter.IsHot Then
                SQL &= " and si.IsHot = 1 " & vbCrLf
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
            If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then
                SQL &= " and (" & vbCrLf & _
                 " (select top 1 mixmatchid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.isactive = 1 and not exists (select itemid from freegift with (nolock) where isactive = 1 and itemid = mml.itemid) and mml.itemid = si.itemid) is not null " & vbCrLf & _
                 " or sp.unitprice is not null) " & vbCrLf
            End If

            SQL &= ") order by Tone"
            Return DB.GetDataSet(SQL)
        End Function
        Public Shared Function GetIdByURLCode(ByVal urlCode As String) As Integer
            Dim result As Integer = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select ToneId from StoreTone where URLCode='" & urlCode & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = CInt(reader.GetValue(0))
                End If
                Core.CloseReader(reader)
                db = Nothing
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Shared Function GetToneNameById(ByVal Id As Integer) As String
            Dim result As String = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select Tone from StoreTone where ToneId=" & Id & ""
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                db = Nothing
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Shared Function GetByURLCode(ByVal urlCode As String) As StoreToneRow
            Dim result As New StoreToneRow
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select * from StoreTone where URLCode='" & urlCode & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = LoadByDataReader(reader)
                End If
                Core.CloseReader(reader)
                db = Nothing
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Shared Function LoadByDataReader(ByVal reader As SqlDataReader) As StoreToneRow
            Dim result As New StoreToneRow
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("ToneId"))) Then
                    result.ToneId = Convert.ToInt32(reader("ToneId"))
                Else
                    result.ToneId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Tone"))) Then
                    result.Tone = reader("Tone").ToString()
                Else
                    result.Tone = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                    result.URLCode = reader("URLCode").ToString()
                Else
                    result.URLCode = ""
                End If
            End If
            Return result
        End Function 'Load
    End Class

    Public MustInherit Class StoreToneRowBase
        Private m_DB As Database
        Private m_ToneId As Integer = Nothing
        Private m_Tone As String = Nothing
        Private m_URLCode As String = Nothing

        Public Property ToneId() As Integer
            Get
                Return m_ToneId
            End Get
            Set(ByVal Value As Integer)
                m_ToneId = Value
            End Set
        End Property

        Public Property Tone() As String
            Get
                Return m_Tone
            End Get
            Set(ByVal Value As String)
                m_Tone = Value
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

        Public Sub New(ByVal DB As Database, ByVal ToneId As Integer)
            m_DB = DB
            m_ToneId = ToneId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Tone As String)
            m_DB = DB
            m_Tone = Tone
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STORETONE_GETOBJECT As String = "sp_StoreTone_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_GETOBJECT)
                db.AddInParameter(cmd, "ToneId", DbType.Int32, ToneId)
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

        Protected Overridable Sub LoadByName()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STORETONE_GETOBJECT As String = "sp_StoreTone_GetObjectByName"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_GETOBJECT)
                db.AddInParameter(cmd, "ToneName", DbType.String, Tone)
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
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ToneId"))) Then
                        m_ToneId = Convert.ToInt32(reader("ToneId"))
                    Else
                        m_ToneId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Tone"))) Then
                        m_Tone = reader("Tone").ToString()
                    Else
                        m_Tone = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                        m_URLCode = reader("URLCode").ToString()
                    Else
                        m_URLCode = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORETONE_INSERT As String = "sp_StoreTone_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_INSERT)

            db.AddOutParameter(cmd, "ToneId", DbType.Int32, ToneId)
            db.AddInParameter(cmd, "Tone", DbType.String, Tone)
            db.AddInParameter(cmd, "URLCode", DbType.String, URLCode)
            db.ExecuteNonQuery(cmd)

            ToneId = Convert.ToInt32(db.GetParameterValue(cmd, "ToneId"))

            '------------------------------------------------------------------------
            Return ToneId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORETONE_UPDATE As String = "sp_StoreTone_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_UPDATE)

            db.AddInParameter(cmd, "ToneId", DbType.Int32, ToneId)
            db.AddInParameter(cmd, "Tone", DbType.String, Tone)
            db.AddInParameter(cmd, "URLCode", DbType.String, URLCode)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORETONE_DELETE As String = "sp_StoreTone_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_DELETE)

            db.AddInParameter(cmd, "ToneId", DbType.Int32, ToneId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub
    End Class

    Public Class StoreToneCollection
        Inherits GenericCollection(Of StoreToneRow)
    End Class

End Namespace


