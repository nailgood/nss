Option Explicit On

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
Imports System.Collections.Generic

Namespace DataLayer

    Public Class StoreCollectionRow
        Inherits StoreCollectionRowBase

        Private Shared cachePrefixKey As String = "StoreCollection_"
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CollectionId As Integer)
            MyBase.New(DB, CollectionId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Collection As String)
            MyBase.New(DB, Collection)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CollectionId As Integer) As StoreCollectionRow
            Dim row As StoreCollectionRow

            row = New StoreCollectionRow(DB, CollectionId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal Collection As String) As StoreCollectionRow
            Dim row As StoreCollectionRow

            row = New StoreCollectionRow(DB, Collection)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CollectionId As Integer)
            Dim row As StoreCollectionRow

            row = New StoreCollectionRow(DB, CollectionId)
            row.Remove()
        End Sub

        Public Shared Function GetAllCollections(ByVal DB As Database) As DataSet
            Dim SQL As String = "select * from StoreCollection"
            Return DB.GetDataSet(SQL)
        End Function

        Public Shared Function GetCollectionFilter(ByVal collectionIds As String) As List(Of ListItem)
            If String.IsNullOrEmpty(collectionIds) Then
                Return New List(Of ListItem)()
            End If

            Dim List As List(Of ListItem) = New List(Of ListItem)()
            Dim dr = Nothing
            Dim SQL As String = "select collectionid, collectionname from StoreCollection a where exists (select 1 from dbo.SplitString('" + collectionIds + "',',') b where a.CollectionId = LTRIM(RTRIM(part)))"
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                dr = db.ExecuteReader(cmd)

                While dr.Read()
                    Dim item As New ListItem(dr("collectionname").ToString(), dr("collectionid").ToString())
                    List.Add(item)
                End While
                Return List
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCollectionFilter", ex)
                Return New List(Of ListItem)()
            End Try
        End Function
        Public Shared Function GetCollectionForSearch() As List(Of String)
            Dim c As List(Of String)
            Dim key As String = String.Format(cachePrefixKey & "GetCollectionForSearch")

            c = CType(CacheUtils.GetCache(key), List(Of String))
            If Not c Is Nothing Then
                Return c
            End If

            c = New List(Of String)
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "select CollectionId, replace(CollectionName, ' and ',' & ') as CollectionName from StoreCollection"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SP)
                r = db.ExecuteReader(cmd)
                While r.Read()
                    c.Add(Convert.ToString(r.Item("CollectionName")).ToLower().Trim())
                End While
                Core.CloseReader(r)

                CacheUtils.SetCache(key, c, Utility.ConfigData.TimeCacheData)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog("GetCollectionForSearch", ex)
            End Try

            Return c
        End Function
        Public Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreCollection.vb", func, ex)
        End Sub

        Public Shared Function GetAllCollections(ByVal Filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As List(Of ListItem)
            Dim SQL As String = String.Empty
            If (url.Contains("/nail-supply/") Or url.Contains("store/default.aspx") Or url.Contains("store/sub-category.aspx") Or url.Contains("/nail-brand/") Or url.Contains("/nail-collection/") Or url.Contains("store/collection.aspx")) Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                SQL &= " Select "
                SQL &= " ci.CollectionId from storeitem si inner join storecollectionitem ci on ci.itemid = si.itemid" & vbCrLf
                SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 " & IIf(Filter.Sale24Hour, " and startingdate = " & Database.Quote(Now.ToShortDateString) & " and endingdate = " & Database.Quote(Now.ToShortDateString), " and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) ") & " and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and " & CustomerPriceGroupId & " = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
                If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
                    SQL &= " 	inner join storedepartmentitem sd on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
                End If
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

                If Not Filter.ToneId = 0 Then
                    SQL &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & Filter.ToneId & ") " & vbCrLf
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
                SQL = "select CollectionId, CollectionName from StoreCollection where collectionid in( " & SQL & ") order by CollectionName"
            Else
                'Else
                SQL = "select CollectionId, CollectionName from StoreCollection where collectionid in ("

                SQL &= " select collectionid from storecollectionitem sci with (nolock) inner join storeitem si with (nolock) on sci.itemid = si.itemid " & vbCrLf
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
                    SQL &= "kw.KeywordName= " & Database.Quote(Filter.Keyword) & vbCrLf
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
                    SQL &= " and (" & vbCrLf &
                     " (select top 1 mixmatchid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.isactive = 1 and not exists (select itemid from freegift with (nolock) where isactive = 1 and itemid = mml.itemid) and mml.itemid = si.itemid) is not null " & vbCrLf &
                     " or sp.unitprice is not null) " & vbCrLf
                End If
                SQL &= ") order by CollectionName"
            End If

            Dim dr As SqlDataReader
            Dim list As New List(Of ListItem)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try

                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                dr = db.ExecuteReader(cmd)

                While dr.Read()
                    Dim item As New ListItem(dr("CollectionName").ToString(), dr("CollectionId").ToString())
                    list.Add(item)
                End While

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return list
        End Function

        'Public Shared Function GetAllCollections(ByVal DB As Database, ByVal Filter As DepartmentFilterFields) As DataSet
        '    Dim SQL As String = "select * from StoreCollection where collectionid in ("

        '    SQL &= " select collectionid from storecollectionitem sci with (nolock) inner join storeitem si with (nolock) on sci.itemid = si.itemid " & vbCrLf
        '    If Filter.DepartmentId <> Nothing AndAlso Filter.DepartmentId <> 23 Then
        '        SQL &= " 	inner join storedepartmentitem sd with (nolock) on si.itemid = sd.itemid and sd.departmentid = " & Filter.DepartmentId
        '    End If
        '    If Filter.IsSearchKeyWord = True Then
        '        SQL &= " inner join KeywordItem kwi on kwi.itemid = si.itemid " & vbCrLf
        '        SQL &= "  inner join Keyword kw on(kwi.KeywordId=kw.KeywordId) " & vbCrLf
        '    End If

        '    If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then SQL &= " left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity > 0 and getdate() between coalesce(startingdate,getdate()) and coalesce(endingdate,getdate()+1) and ((salestype = 0 and memberid = " & Filter.MemberId & ") or (memberid is null and salestype = 2) or (salestype = 1 and (select top 1 CustomerPriceGroupId from customer with (nolock) where customerid = (select top 1 customerid from member with (nolock) where memberid = " & Filter.MemberId & ")) = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid " & vbCrLf
        '    SQL &= " where si.isactive = 1 " & vbCrLf


        '    If Filter.IsSearchKeyWord Then
        '        SQL &= " and " & vbCrLf
        '        SQL &= "kw.KeywordName=" & DB.Quote(Filter.Keyword) & vbCrLf
        '    Else
        '        If Filter.Keyword <> Nothing Then
        '            SQL &= " and " & vbCrLf
        '            SQL &= "si.itemid in (select [key] from CONTAINSTABLE(storeitem, * , " & DB.Quote(Filter.Keyword) & ")) " & vbCrLf
        '        End If
        '    End If
        '    If Filter.IsFeatured Then
        '        SQL &= " and si.IsFeatured = 1" & vbCrLf
        '    End If
        '    If Filter.IsNew Then
        '        SQL &= " and si.IsNew = 1 and (NewUntil is null or NewUntil > GetDate()) " & vbCrLf
        '    End If
        '    If Filter.IsOnSale Then
        '        SQL &= " and (si.IsOnSale = 1 ) " & vbCrLf
        '    End If
        '    If Not Filter.BrandId = 0 Then
        '        SQL &= " and si.BrandId = " & DB.Number(Filter.BrandId) & vbCrLf
        '    End If
        '    If Not Filter.Feature = Nothing Then
        '        SQL &= " and si.itemid in (select itemid from storeitemfeaturefilter where name = " & DB.Quote(Filter.Feature) & ") "
        '    End If
        '    If Filter.IsHot Then
        '        SQL &= " and si.IsHot = 1 " & vbCrLf
        '    End If
        '    'If Not Filter.CollectionId = 0 Then
        '    '	SQL &= " and si.ItemId in (select itemid from storecollectionitem with (nolock) where collectionid = " & Filter.CollectionId & ") " & vbCrLf
        '    'End If
        '    'If Not Filter.ToneId = 0 Then
        '    '	SQL &= " and si.ItemId in (select itemid from storetoneitem with (nolock) where toneid = " & Filter.ToneId & ") " & vbCrLf
        '    'End If
        '    'If Not Filter.ShadeId = 0 Then
        '    '	SQL &= " and si.ItemId in (select itemid from storeshadeitem with (nolock) where shadeid = " & Filter.ShadeId & ") " & vbCrLf
        '    'End If
        '    If Filter.HasPromotion OrElse Filter.PromotionId <> Nothing Then
        '        SQL &= " and (" & vbCrLf & _
        '         " (select top 1 mixmatchid from mixmatch mm with (nolock) inner join mixmatchline mml with (nolock) on mm.id = mml.mixmatchid where mm.isactive = 1 and not exists (select itemid from freegift with (nolock) where isactive = 1 and itemid = mml.itemid) and mml.itemid = si.itemid) is not null " & vbCrLf & _
        '         " or sp.unitprice is not null) " & vbCrLf
        '    End If

        '    SQL &= ") order by CollectionName"
        '    Return DB.GetDataSet(SQL)
        'End Function

        Public Shared Function GetCollectionNameById(ByVal id As Integer) As String
            If (id < 1) Then
                Return String.Empty
            End If
            Dim reader As SqlDataReader = Nothing
            Dim result As String = String.Empty
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select CollectionName from StoreCollection where CollectionId=" & id & ""
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
        Public Shared Function GetByURLCode(ByVal urlCode As String) As StoreCollectionRow
            Dim reader As SqlDataReader = Nothing
            Dim result As New StoreCollectionRow
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select * from StoreCollection where URLCode='" & urlCode & "'"
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
        Public Shared Function GetIDByURLCode(ByVal urlCode As String) As Integer
            Dim reader As SqlDataReader = Nothing
            Dim result As Integer = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select CollectionId from StoreCollection where URLCode='" & urlCode & "'"
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
        Protected Shared Function LoadByDataReader(ByVal r As SqlDataReader) As StoreCollectionRow
            Dim result As New StoreCollectionRow
            result.CollectionId = Convert.ToInt32(r.Item("CollectionId"))
            If IsDBNull(r.Item("CollectionName")) Then
                result.CollectionName = Nothing
            Else
                result.CollectionName = Convert.ToString(r.Item("CollectionName"))
            End If
            If IsDBNull(r.Item("URLCode")) Then
                result.URLCode = Nothing
            Else
                result.URLCode = Convert.ToString(r.Item("URLCode"))
            End If
            Return result
        End Function 'Load
    End Class

    Public MustInherit Class StoreCollectionRowBase
        Private m_DB As Database
        Private m_CollectionId As Integer = Nothing
        Private m_CollectionName As String = Nothing
        Private m_URLCode As String = Nothing

        Public Property CollectionId() As Integer
            Get
                Return m_CollectionId
            End Get
            Set(ByVal Value As Integer)
                m_CollectionId = Value
            End Set
        End Property

        Public Property CollectionName() As String
            Get
                Return m_CollectionName
            End Get
            Set(ByVal Value As String)
                m_CollectionName = Value
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

        Public Sub New(ByVal DB As Database, ByVal CollectionId As Integer)
            m_DB = DB
            m_CollectionId = CollectionId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Collection As String)
            m_DB = DB
            m_CollectionName = Collection
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreCollection WHERE " & IIf(CollectionId <> Nothing, "CollectionId = " & DB.Number(CollectionId), "CollectionName = " & DB.Quote(CollectionName))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_CollectionId = Convert.ToInt32(r.Item("CollectionId"))
                If IsDBNull(r.Item("CollectionName")) Then
                    m_CollectionName = Nothing
                Else
                    m_CollectionName = Convert.ToString(r.Item("CollectionName"))
                End If
                If IsDBNull(r.Item("URLCode")) Then
                    m_URLCode = Nothing
                Else
                    m_URLCode = Convert.ToString(r.Item("URLCode"))
                End If
            Catch ex As Exception
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim returnParameters As New SqlParameter("returnVal", SqlDbType.Int)
            returnParameters.Direction = ParameterDirection.ReturnValue
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreCollection_Insert")
            db.AddInParameter(cmd, "CollectionName", DbType.String, CollectionName)
            cmd.Parameters.Add(returnParameters)
            Try
                db.ExecuteNonQuery(cmd)
                CollectionId = Convert.ToInt32(returnParameters.Value)
            Catch ex As Exception
            End Try

            Return CollectionId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreCollection SET " _
             & " CollectionName = " & m_DB.Quote(CollectionName) _
                       & ", URLCode = " & m_DB.Quote(URLCode) _
             & " WHERE CollectionId = " & m_DB.Quote(CollectionId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreCollection WHERE CollectionId = " & m_DB.Quote(CollectionId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

End Namespace


