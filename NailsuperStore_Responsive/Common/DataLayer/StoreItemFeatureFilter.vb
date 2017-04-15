Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common

Namespace DataLayer

	Public Class StoreItemFeatureFilterRow
		Inherits StoreItemFeatureFilterRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal FeatureId As Integer)
			MyBase.New(DB, FeatureId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal FeatureId As Integer) As StoreItemFeatureFilterRow
			Dim row As StoreItemFeatureFilterRow

			row = New StoreItemFeatureFilterRow(DB, FeatureId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal FeatureId As Integer)
			Dim row As StoreItemFeatureFilterRow

			row = New StoreItemFeatureFilterRow(DB, FeatureId)
			row.Remove()
		End Sub
        Public Shared Function InsertData(ByVal data As StoreItemFeatureFilterRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItemFeatureFilter_Insert")
                db.AddInParameter(cmd, "ItemId", DbType.String, data.ItemId)
                db.AddInParameter(cmd, "Name", DbType.String, data.Name)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function UpdateData(ByVal data As StoreItemFeatureFilterRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreItemFeatureFilter_Update")
                db.AddInParameter(cmd, "FeatureId", DbType.Int32, data.FeatureId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, data.ItemId)
                db.AddInParameter(cmd, "Name", DbType.String, data.Name)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
		Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
			Dim SQL As String = "select * from StoreItemFeatureFilter"
			If Not SortBy = String.Empty Then
				SortBy = Core.ProtectParam(SortBy)
				SortOrder = Core.ProtectParam(SortOrder)

				SQL &= " order by " & SortBy & " " & SortOrder
			End If
			Return DB.GetDataTable(SQL)
		End Function
        Public Shared Function GetAllFeatures(ByVal DB As Database, ByVal filter As DepartmentFilterFields, ByVal url As String, ByVal CustomerPriceGroupId As Integer) As DataSet
            Dim sqlItem As String = String.Empty
            Dim sql As String = String.Empty
            If (url.Contains("nail-sales-promotion") Or url.Contains("store/category.aspx")) Then
                Dim GroupItems As Boolean = False
                sqlItem &= " Select itemid from(Select "
                sqlItem &= " si.itemid, " & vbCrLf & _
                "case when si.itemgroupid is not null then (select min(price) from storeitem where itemgroupid = si.itemgroupid and isactive = 1) else si.price end as lowprice, " & vbCrLf & _
                "coalesce(sp.unitprice,si.price) as lowsaleprice " & vbCrLf
                sqlItem &= " from storeitem si inner join Storeitemfeaturefilter ci on ci.itemid = si.itemid" & vbCrLf
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
                sql = "select  name,URLCode from Storeitemfeaturefilter where itemid in( " & sqlItem & ") order by name"
                Return DB.GetDataSet(sql)
            ElseIf (url.Contains("/nail-supply/") Or url.Contains("store/default.aspx")) Then
                Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
                Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
                sqlItem &= " Select "
                sqlItem &= " si.itemid from storeitem si inner join Storeitemfeaturefilter ci on ci.itemid = si.itemid" & vbCrLf
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
                If Not filter.PriceRange = String.Empty Then
                    Dim low, high As Integer
                    Dim a() As String = filter.PriceRange.Split("-")
                    If UBound(a) = 1 AndAlso IsNumeric(a(0)) AndAlso IsNumeric(a(1)) Then
                        low = CInt(a(0))
                        high = CInt(a(1))
                        sqlItem &= " and si.price between " & low & " and " & high
                    End If
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
                sql = "select  name,URLCode from Storeitemfeaturefilter where itemid in( " & sqlItem & ") order by name"
                Return DB.GetDataSet(sql)
            Else
                Return GetAllFeatures(DB, filter)
            End If
        End Function
		'Custom Methods
		Public Shared Function GetAllFeatures(ByVal DB As Database, ByVal filter As DepartmentFilterFields) As DataSet
            Dim SQL As String = "select distinct " & IIf(filter.All, "", " top " & filter.MaxPerPage * filter.pg) & " name,URLCode from Storeitemfeaturefilter where "

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

			SQL &= ") order by name"
			Return DB.GetDataSet(SQL)
        End Function
        Public Shared Function GetIdByURLCode(ByVal urlCode As String) As Integer
            Dim result As Integer = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select ShadeId from StoreItemFeatureFilter where URLCode='" & urlCode & "'"
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
        Public Shared Function GetByURLCode(ByVal urlCode As String) As StoreItemFeatureFilterRow
            Dim result As New StoreItemFeatureFilterRow
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select * from StoreItemFeatureFilter where URLCode='" & urlCode & "'"
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
        Protected Shared Function LoadByDataReader(ByVal r As SqlDataReader) As StoreItemFeatureFilterRow
            Dim result As New StoreItemFeatureFilterRow
            result.FeatureId = Convert.ToInt32(r.Item("FeatureId"))
            result.ItemId = Convert.ToInt32(r.Item("ItemId"))
            result.Name = Convert.ToString(r.Item("Name"))
            result.URLCode = Convert.ToString(r.Item("URLCode"))
            Return result
        End Function 'Load
	End Class

	Public MustInherit Class StoreItemFeatureFilterRowBase
		Private m_DB As Database
		Private m_FeatureId As Integer = Nothing
		Private m_ItemId As Integer = Nothing
		Private m_Name As String = Nothing
        Private m_URLCode As String = Nothing

		Public Property FeatureId() As Integer
			Get
				Return m_FeatureId
			End Get
			Set(ByVal Value As Integer)
				m_FeatureId = value
			End Set
		End Property

		Public Property ItemId() As Integer
			Get
				Return m_ItemId
			End Get
			Set(ByVal Value As Integer)
				m_ItemId = value
			End Set
		End Property

		Public Property Name() As String
			Get
				Return m_Name
			End Get
			Set(ByVal Value As String)
				m_Name = value
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
			Set(ByVal Value As DataBase)
				m_DB = Value
			End Set
		End Property

		Public Sub New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal FeatureId As Integer)
			m_DB = DB
			m_FeatureId = FeatureId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemFeatureFilter WHERE FeatureId = " & DB.Number(FeatureId)
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


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_FeatureId = Convert.ToInt32(r.Item("FeatureId"))
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                m_Name = Convert.ToString(r.Item("Name"))
                m_URLCode = Convert.ToString(r.Item("URLCode"))
            Catch ex As Exception
                Throw ex
                ''  Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


            SQL = " INSERT INTO StoreItemFeatureFilter (" _
             & " ItemId" _
             & ",Name,URLCode" _
             & ") VALUES (" _
             & m_DB.NullNumber(ItemId) _
             & "," & m_DB.Quote(Name) _
             & "," & m_DB.Quote(URLCode) & ")"

			FeatureId = m_DB.InsertSQL(SQL)

			Return FeatureId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

            SQL = " UPDATE StoreItemFeatureFilter SET " _
             & " ItemId = " & m_DB.NullNumber(ItemId) _
             & ",Name = " & m_DB.Quote(Name) _
                       & ",URLCode = " & m_DB.Quote(URLCode) _
             & " WHERE FeatureId = " & m_DB.Quote(FeatureId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM StoreItemFeatureFilter WHERE FeatureId = " & m_DB.Number(FeatureId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class StoreItemFeatureFilterCollection
		Inherits GenericCollection(Of StoreItemFeatureFilterRow)
	End Class

End Namespace


