Imports Components
Imports Components.Core
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


Public Enum PromotionType
    LineSpecific
    LeastExpensive
End Enum

Public Class PromotionRow
    Public Text As String = Nothing
    Public ItemId As Integer = Nothing
    Public PercentOff As Double = Nothing
    Public AllAreFree As Boolean = False
    Public Price As Double = Nothing
    Public LinesToTrigger As Integer = Nothing
    Public Type As PromotionType = Nothing
    Public Mandatory As Integer = Nothing
    Public [Optional] As Integer = Nothing
    Public TimesApplicable As Integer = Nothing
    Public MixMatchId As Integer = Nothing
    Public PurchaseItems As DataView = Nothing
    Public GetItems As DataView = Nothing
    Public Description As String = String.Empty
    Public EndingDate As DateTime = Nothing
    Public MixMatchType As Integer = Nothing
    Public DB As Database
    Private Const PromotionCacheTime As Integer = 60

    Public Sub New()
        Dim tPurchaseItems As DataTable = New DataTable
        Dim tGetItems As DataTable = New DataTable

        tPurchaseItems.Columns.Add(New DataColumn("ItemId", GetType(Integer)))
        tPurchaseItems.Columns.Add(New DataColumn("PercentOff", GetType(Double)))
        tPurchaseItems.Columns.Add(New DataColumn("SetPrice", GetType(Double)))
        tPurchaseItems.Columns.Add(New DataColumn("ItemName", GetType(String)))
        tPurchaseItems.Columns.Add(New DataColumn("Image", GetType(String)))
        tPurchaseItems.Columns.Add(New DataColumn("SKU", GetType(String)))
        tPurchaseItems.Columns.Add(New DataColumn("URLCode", GetType(String)))
        tPurchaseItems.Columns.Add(New DataColumn("Price", GetType(Double)))
        tPurchaseItems.Columns.Add(New DataColumn("HighPrice", GetType(Double)))
        tPurchaseItems.Columns.Add(New DataColumn("LowPrice", GetType(Double)))
        tPurchaseItems.Columns.Add(New DataColumn("DepartmentId", GetType(Integer)))

        tGetItems.Columns.Add(New DataColumn("ItemId", GetType(Integer)))
        tGetItems.Columns.Add(New DataColumn("PercentOff", GetType(Double)))
        tGetItems.Columns.Add(New DataColumn("SetPrice", GetType(Double)))
        tGetItems.Columns.Add(New DataColumn("ItemName", GetType(String)))
        tGetItems.Columns.Add(New DataColumn("Image", GetType(String)))
        tGetItems.Columns.Add(New DataColumn("SKU", GetType(String)))
        tGetItems.Columns.Add(New DataColumn("URLCode", GetType(String)))
        tGetItems.Columns.Add(New DataColumn("Price", GetType(Double)))
        tGetItems.Columns.Add(New DataColumn("HighPrice", GetType(Double)))
        tGetItems.Columns.Add(New DataColumn("LowPrice", GetType(Double)))
        tGetItems.Columns.Add(New DataColumn("DepartmentId", GetType(Integer)))
        tGetItems.Columns.Add(New DataColumn("DefaultSelectQty", GetType(Integer)))
        tGetItems.Columns.Add(New DataColumn("IsDefaultSelect", GetType(Boolean)))
        tGetItems.Columns.Add(New DataColumn("Value", GetType(Double)))
        PurchaseItems = New DataView
        tPurchaseItems.TableName = "PurchaseItems"
        PurchaseItems.Table = tPurchaseItems
        GetItems = New DataView
        tGetItems.TableName = "GetItems"
        GetItems.Table = tGetItems
    End Sub

    Private Shared Function GetMixMatchByItem(ByVal itemId As Integer) As DataTable
        '------------------------------------------------------------------------
        'Author: Lam Le
        'Date: October 26, 2009 02:13:03 PM
        '------------------------------------------------------------------------
        Dim mixMatchId As Integer = 0

        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

        Dim SP_STOREITEM_GETLIST As String = "sp_MixMatch_GetMixMatchIDByItem"

        Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEM_GETLIST)

        db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)

        Return db.ExecuteDataSet(cmd).Tables(0)
        '------------------------------------------------------------------------
    End Function

    Public Shared Function GetRows(ByVal DB As Database, ByVal Id As Integer) As PromotionCollection
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing

        Dim c As New PromotionCollection
        Dim SQL As String '= "SELECT mm.Id FROM MixMatch mm INNER JOIN MixMatchLine mml ON mm.Id = mml.MixMatchId inner join storeitem si on mml.itemid = si.itemid WHERE mm.isactive = 1 and getdate() between coalesce(startingdate,getdate()) and coalesce(endingdate+1,getdate()+1) and qtyonhand > 0 and si.ItemId = " & Id & " order by mm.Id"
        Dim dv As DataView, drv As DataRowView
        Dim IndividualPromo As Integer = 0
        Dim LowestPriceIndex As Integer = -1
        Dim LowestPrice As Double = 0
        Dim HasPurchaseItem As Boolean = False

        'dv = DB.GetDataView(SQL)
        dv = GetMixMatchByItem(Id).DefaultView

        For i As Integer = 0 To dv.Count - 1
            drv = dv(i)
            Dim p As PromotionRow = PromotionRow.GetRow(DB, drv("Id"), False)
            If p.LinesToTrigger = p.Optional Then IndividualPromo += 1
            Dim tmp As New DataView
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.RowFilter = "itemid = " & Id
            If (tmp.Count > 0 AndAlso Not HasPurchaseItem) OrElse (tmp.Count = 0) Then
                If tmp.Count > 0 Then HasPurchaseItem = True
                c.Add(p)
            End If
        Next

        If IndividualPromo > 1 Then 'Multiple single item promotions (shouldn't happen but just in case)
            For i As Integer = 0 To c.Count - 1
                Dim tmp As New DataView
                tmp.Table = c(i).GetItems.Table.Copy
                tmp.RowFilter = "itemid = " & Id
                If tmp.Count > 0 Then
                    Dim ThisPrice As Double
                    If Not IsDBNull(tmp(0)("SetPrice")) Then
                        ThisPrice = tmp(0)("SetPrice")
                    Else
                        ThisPrice = tmp(0)("Price") * (1 - (tmp(0)("PercentOff") / 100))
                    End If
                    If LowestPriceIndex = -1 OrElse (LowestPrice > ThisPrice) Then
                        LowestPrice = ThisPrice
                        LowestPriceIndex = i
                    End If
                End If
            Next

            Dim coll As New PromotionCollection
            For i As Integer = 0 To c.Count - 1
                If i = LowestPriceIndex Then
                    coll.Add(c(i))
                    c.Clear()
                    c = coll
                    Exit For
                End If
            Next
        End If

        Return c
    End Function


    Public Shared Function GetRowOld(ByVal DB As Database, ByVal Id As Integer, ByVal ByItem As Boolean) As PromotionRow
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing

        Dim p As New PromotionRow()
        Dim s As DateTime = Now
        Dim SubSQL As String = " "
        If ByItem Then
            SubSQL &= "i.ItemId = "
        Else
            SubSQL &= "m.Id = "
        End If
        SubSQL &= Id & " "



        Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()

        'Long Edit Feb 20 2014
        Dim SQL As String = "SELECT " & vbCrLf &
         "	m.Type, " & vbCrLf &
         "	m.Description, " & vbCrLf &
         "	m.EndingDate, " & vbCrLf &
         "	i.ItemName, " & vbCrLf &
         "	i.image, " & vbCrLf &
         "  i.SKU, " & vbCrLf &
         "  i.Price, " & vbCrLf &
         "  (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = i.itemid order by o.sortorder) as choicename, " & vbCrLf &
         "  coalesce(tmp.lowprice,i.price) as lowprice, " & vbCrLf &
         "  coalesce(tmp.highprice,i.price) as highprice, " & vbCrLf &
         "	l.MixMatchId, " & vbCrLf &
         "	l.ItemId, " & vbCrLf &
         "	l.DiscountType, " & vbCrLf &
         "	l.[Value], " & vbCrLf &
         "	m.DiscountType as PromotionType, " & vbCrLf &
         "	m.LinesToTrigger, " & vbCrLf &
         "	m.TimesApplicable, " & vbCrLf &
         "	m.Mandatory, " & vbCrLf &
         "	m.[Optional] " & vbCrLf &
         "	, sdi.DepartmentId,coalesce(IsDefaultSelect,0) as IsDefaultSelect,coalesce(DefaultSelectQty,0) as  DefaultSelectQty " & vbCrLf &
         "FROM " & vbCrLf &
         "	MixMatchLine l " & vbCrLf &
         "	INNER JOIN MixMatch m ON l.MixMatchId = m.Id " & vbCrLf &
         "	INNER JOIN StoreItem i ON l.ItemId = i.ItemId " & vbCrLf &
         "	left outer join (select itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem group by itemgroupid) tmp on tmp.itemgroupid = i.itemgroupid " & vbCrLf &
         "	Left JOIN StoreDepartmentItem sdi ON l.ItemId = sdi.ItemId " & vbCrLf &
         "WHERE " & vbCrLf &
         "	" & SubSQL & vbCrLf &
         "	AND m.IsActive = 1 and i.IsActive=1 and (i.QtyOnHand>0 or (i.AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.AcceptingOrder & " or i.AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.InStock & "))" & vbCrLf &
         "	AND getdate() between coalesce(m.startingdate,getdate()) and coalesce(m.endingdate + 1,getdate() + 1)" & vbCrLf &
                  "ORDER BY " & vbCrLf &
         "	l.IsDefaultSelect DESC,m.Id, " & vbCrLf &
         "	l.[LineNo] --MemberId=" & Utility.Common.GetCurrentMemberId() & ",OrderId=" & Utility.Common.GetCurrentOrderId()

        '"	AND not exists (select itemid from freegift where isactive = 1 and itemid = l.itemid) " & vbCrLf & _
        'End Edit Feb 20 2014
        Dim dr As SqlDataReader = Nothing
        Dim row As DataRow
        Try
            If DB.IsOpen Then
                dr = DB.GetReader(SQL)








                Dim Buy As Boolean = False
                Dim Content As String = String.Empty
                Dim sGet As String = String.Empty
                Dim Counter As Integer = 0

                While dr.Read
                    If p.LinesToTrigger = 0 Then
                        If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                            p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                            If Not p Is Nothing Then
                                dr.Close()
                                Return p
                            End If
                        End If
                        p.LinesToTrigger = dr("linestotrigger")
                        p.Mandatory = dr("mandatory")
                        p.[Optional] = dr("optional")
                        p.MixMatchId = dr("MixMatchId")
                        p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                        p.TimesApplicable = dr("TimesApplicable")
                        p.Description = dr("Description")
                        p.EndingDate = dr("EndingDate")
                        p.MixMatchType = dr("Type")
                    End If

                    If p.Type = PromotionType.LeastExpensive Then
                        row = p.PurchaseItems.Table.NewRow
                    Else
                        row = IIf(CDbl(dr("Value")) > 0 AndAlso p.Mandatory <> p.LinesToTrigger, p.GetItems.Table.NewRow, p.PurchaseItems.Table.NewRow)
                    End If

                    row("ItemId") = dr("ItemId")
                    row("ItemName") = dr("ItemName")
                    If Not IsDBNull(dr("choicename")) Then
                        row("ItemName") = dr("ItemName") & " - " & dr("choicename")
                    Else
                        row("ItemName") = dr("ItemName")
                    End If
                    If dr.IsDBNull(dr.GetOrdinal("DepartmentId")) Then
                        row("DepartmentId") = 23
                    Else
                        row("DepartmentId") = dr("DepartmentId")
                    End If
                    If Not IsDBNull(dr("Image")) Then
                        row("Image") = dr("Image")
                    Else
                        row("Image") = String.Empty
                    End If
                    row("SKU") = dr("sku")
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")

                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = dr("Value")
                    Else
                        row("PercentOff") = dr("Value")
                        row("SetPrice") = DBNull.Value
                    End If

                    If p.Type = PromotionType.LeastExpensive Then
                        p.PurchaseItems.Table.Rows.Add(row)
                    Else
                        If CDbl(dr("Value")) > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                            row("DefaultSelectQty") = dr("DefaultSelectQty")
                            row("IsDefaultSelect") = dr("IsDefaultSelect")
                            row("Value") = dr("Value")
                            p.GetItems.Table.Rows.Add(row)
                        Else
                            p.PurchaseItems.Table.Rows.Add(row)
                        End If
                    End If
                End While
            End If
        Catch ex As Exception
        End Try

        Core.CloseReader(dr)
        Dim tmp As New DataView()
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If

        Return p
    End Function

    Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer, ByVal ByItem As Boolean) As PromotionRow
        Return GetRowOld(DB, Id, ByItem)

        If System.Web.HttpContext.Current Is Nothing Then Return Nothing

        Dim p As New PromotionRow()
        '      Dim s As DateTime = Now
        '      Dim SubSQL As String = " "
        'If ByItem Then
        '          SubSQL &= "i.ItemId = "
        '      Else
        '          SubSQL &= "m.Id = "
        '      End If
        '      SubSQL &= Id & " "



        Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()

        ''Long Edit Feb 20 2014
        'Dim SQL As String = "SELECT " & vbCrLf &
        ' "	m.Type, " & vbCrLf &
        ' "	m.Description, " & vbCrLf &
        ' "	m.EndingDate, " & vbCrLf &
        ' "	i.ItemName, " & vbCrLf &
        ' "	i.image, " & vbCrLf &
        ' "  i.SKU, " & vbCrLf &
        ' "  i.Price, " & vbCrLf &
        ' "  (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = i.itemid order by o.sortorder) as choicename, " & vbCrLf &
        ' "  coalesce(tmp.lowprice,i.price) as lowprice, " & vbCrLf &
        ' "  coalesce(tmp.highprice,i.price) as highprice, " & vbCrLf &
        ' "	l.MixMatchId, " & vbCrLf &
        ' "	l.ItemId, " & vbCrLf &
        ' "	l.DiscountType, " & vbCrLf &
        ' "	l.[Value], " & vbCrLf &
        ' "	m.DiscountType as PromotionType, " & vbCrLf &
        ' "	m.LinesToTrigger, " & vbCrLf &
        ' "	m.TimesApplicable, " & vbCrLf &
        ' "	m.Mandatory, " & vbCrLf &
        ' "	m.[Optional] " & vbCrLf &
        ' "	, sdi.DepartmentId,coalesce(IsDefaultSelect,0) as IsDefaultSelect,coalesce(DefaultSelectQty,0) as  DefaultSelectQty " & vbCrLf &
        ' "FROM " & vbCrLf &
        ' "	MixMatchLine l " & vbCrLf &
        ' "	INNER JOIN MixMatch m ON l.MixMatchId = m.Id " & vbCrLf &
        ' "	INNER JOIN StoreItem i ON l.ItemId = i.ItemId " & vbCrLf &
        ' "	left outer join (select itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem group by itemgroupid) tmp on tmp.itemgroupid = i.itemgroupid " & vbCrLf &
        ' "	Left JOIN StoreDepartmentItem sdi ON l.ItemId = sdi.ItemId " & vbCrLf &
        ' "WHERE " & vbCrLf &
        ' "	" & SubSQL & vbCrLf &
        ' "	AND m.IsActive = 1 and i.IsActive=1 and (i.QtyOnHand>0 or (i.AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.AcceptingOrder & " or i.AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.InStock & "))" & vbCrLf &
        ' "	AND getdate() between coalesce(m.startingdate,getdate()) and coalesce(m.endingdate + 1,getdate() + 1)" & vbCrLf &
        '          "ORDER BY " & vbCrLf &
        ' "	l.IsDefaultSelect DESC,m.Id, " & vbCrLf &
        ' "	l.[LineNo] --MemberId=" & Utility.Common.GetCurrentMemberId() & ",OrderId=" & Utility.Common.GetCurrentOrderId()

        '"	AND not exists (select itemid from freegift where isactive = 1 and itemid = l.itemid) " & vbCrLf & _
        'End Edit Feb 20 2014
        Dim dr As SqlDataReader = Nothing
        Dim row As DataRow
        Try
            Dim dbMic As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = dbMic.GetStoredProcCommand("sp_Mixmatch_getPromotionRowById_Item")
            dbMic.AddInParameter(cmd, "Id", DbType.Int32, Id)
            dbMic.AddInParameter(cmd, "Item", DbType.Boolean, ByItem)
            dbMic.AddInParameter(cmd, "AcceptingOrder", DbType.Boolean, Utility.Common.ItemAcceptingStatus.AcceptingOrder)
            dbMic.AddInParameter(cmd, "AcceptingInStock", DbType.Boolean, Utility.Common.ItemAcceptingStatus.InStock)

            dr = dbMic.ExecuteReader(cmd)

            If dr.HasRows Then
                Dim Buy As Boolean = False
                Dim Content As String = String.Empty
                Dim sGet As String = String.Empty
                Dim Counter As Integer = 0

                While dr.Read
                    If p.LinesToTrigger = 0 Then
                        If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                            p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                            If Not p Is Nothing Then
                                dr.Close()
                                Return p
                            End If
                        End If
                        p.LinesToTrigger = dr("linestotrigger")
                        p.Mandatory = dr("mandatory")
                        p.[Optional] = dr("optional")
                        p.MixMatchId = dr("MixMatchId")
                        p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                        p.TimesApplicable = dr("TimesApplicable")
                        p.Description = dr("Description")
                        p.EndingDate = dr("EndingDate")
                        p.MixMatchType = dr("Type")
                    End If

                    If p.Type = PromotionType.LeastExpensive Then
                        row = p.PurchaseItems.Table.NewRow
                    Else
                        row = IIf(CDbl(dr("Value")) > 0 AndAlso p.Mandatory <> p.LinesToTrigger, p.GetItems.Table.NewRow, p.PurchaseItems.Table.NewRow)
                    End If

                    row("ItemId") = dr("ItemId")
                    row("ItemName") = dr("ItemName")
                    If Not IsDBNull(dr("choicename")) Then
                        row("ItemName") = dr("ItemName") & " - " & dr("choicename")
                    Else
                        row("ItemName") = dr("ItemName")
                    End If
                    If dr.IsDBNull(dr.GetOrdinal("DepartmentId")) Then
                        row("DepartmentId") = 23
                    Else
                        row("DepartmentId") = dr("DepartmentId")
                    End If
                    If Not IsDBNull(dr("Image")) Then
                        row("Image") = dr("Image")
                    Else
                        row("Image") = String.Empty
                    End If
                    row("SKU") = dr("sku")
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")

                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = dr("Value")
                    Else
                        row("PercentOff") = dr("Value")
                        row("SetPrice") = DBNull.Value
                    End If

                    If p.Type = PromotionType.LeastExpensive Then
                        p.PurchaseItems.Table.Rows.Add(row)
                    Else
                        If CDbl(dr("Value")) > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                            row("DefaultSelectQty") = dr("DefaultSelectQty")
                            row("IsDefaultSelect") = dr("IsDefaultSelect")
                            row("Value") = dr("Value")
                            p.GetItems.Table.Rows.Add(row)
                        Else
                            p.PurchaseItems.Table.Rows.Add(row)
                        End If
                    End If
                End While
            End If
        Catch ex As Exception
        End Try

        Dim tmp As New DataView()
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If

        Return p
    End Function
    Public Shared Function GetRowPro(ByVal Id As Integer, ByVal ByItem As Boolean) As PromotionRow
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing
        Dim p As PromotionRow = New PromotionRow()

        Dim s As DateTime = Now
        Dim SubSQL As String = " "
        If ByItem Then
            SubSQL &= "i.ItemId = "
        Else
            SubSQL &= "m.Id = "
        End If
        SubSQL &= Id & " "
        Dim MemberId As Integer = 0
        If IsNumeric(System.Web.HttpContext.Current.Session("MemberId")) Then MemberId = System.Web.HttpContext.Current.Session("MemberId")

        Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
        Dim SQL As String = "SELECT " & vbCrLf & _
         "	m.Type, " & vbCrLf & _
         "	m.Description, " & vbCrLf & _
         "	m.EndingDate, " & vbCrLf & _
         "	i.ItemName, " & vbCrLf & _
         "	i.image, " & vbCrLf & _
         "  i.SKU, " & vbCrLf & _
         "  i.Price, " & vbCrLf & _
         "  (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = i.itemid order by o.sortorder) as choicename, " & vbCrLf & _
         "  coalesce(tmp.lowprice,i.price) as lowprice, " & vbCrLf & _
         "  coalesce(tmp.highprice,i.price) as highprice, " & vbCrLf & _
         "	l.MixMatchId, " & vbCrLf & _
         "	l.ItemId, " & vbCrLf & _
         "	l.DiscountType, " & vbCrLf & _
         "	l.[Value], " & vbCrLf & _
         "	m.DiscountType as PromotionType, " & vbCrLf & _
         "	m.LinesToTrigger, " & vbCrLf & _
         "	m.TimesApplicable, " & vbCrLf & _
         "	m.Mandatory, " & vbCrLf & _
         "	m.[Optional] " & vbCrLf & _
         "	, sdi.DepartmentId,coalesce(IsDefaultSelect,0) as IsDefaultSelect,coalesce(DefaultSelectQty,0) as  DefaultSelectQty " & vbCrLf & _
         "FROM " & vbCrLf & _
         "	MixMatchLine l " & vbCrLf & _
         "	INNER JOIN MixMatch m ON l.MixMatchId = m.Id " & vbCrLf & _
         "	INNER JOIN StoreItem i ON l.ItemId = i.ItemId " & vbCrLf & _
         "	left outer join (select itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem group by itemgroupid) tmp on tmp.itemgroupid = i.itemgroupid " & vbCrLf & _
         "	Left JOIN StoreDepartmentItem sdi ON l.ItemId = sdi.ItemId " & vbCrLf & _
         "WHERE " & vbCrLf & _
         "	" & SubSQL & vbCrLf & _
         "	AND m.IsActive = 1 and i.IsActive=1" & vbCrLf & _
         "	AND getdate() between coalesce(m.startingdate,getdate()) and coalesce(m.endingdate + 1,getdate() + 1)" & vbCrLf & _
         "	AND not exists (select itemid from freegift where isactive = 1 and itemid = l.itemid) " & vbCrLf & _
         "ORDER BY " & vbCrLf & _
         "	l.IsDefaultSelect DESC,m.Id, " & vbCrLf & _
         "	l.[LineNo]"


        'End Edit Feb 20 2014
        Dim Value As Double, pItemId As Integer, pItemName As String, pSKU, pImage As String, pDepartmentId As Integer
        Dim dr As SqlDataReader = Nothing
        Dim row As DataRow
        Try
            Dim db1 As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db1.GetSqlStringCommand(SQL)
            dr = db1.ExecuteReader(cmd)
            Dim Buy As Boolean = False
            Dim Content As String = String.Empty
            Dim sGet As String = String.Empty
            Dim Counter As Integer = 0
            While dr.Read
                Value = dr("Value")
                pItemId = dr("ItemId")
                pItemName = dr("ItemName")
                'Modified by: Lam Le
                'Modified Date: November 24, 2009
                'pDepartmentId = dr("DepartmentId")
                If dr.IsDBNull(dr.GetOrdinal("DepartmentId")) Then
                    pDepartmentId = 23
                Else
                    pDepartmentId = dr("DepartmentId")
                End If
                If Not IsDBNull(dr("Image")) Then pImage = dr("Image") Else pImage = String.Empty
                If Not IsDBNull(dr("choicename")) Then pItemName &= " - " & dr("choicename")
                pSKU = dr("sku")

                If p.LinesToTrigger = 0 Then
                    If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                        p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                        If Not p Is Nothing Then
                            dr.Close()
                            Return p
                        End If
                    End If
                    p.LinesToTrigger = dr("linestotrigger")
                    p.Mandatory = dr("mandatory")
                    p.[Optional] = dr("optional")
                    p.MixMatchId = dr("MixMatchId")
                    p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                    p.TimesApplicable = dr("TimesApplicable")
                    p.Description = dr("Description")
                    p.EndingDate = dr("EndingDate")
                    p.MixMatchType = dr("Type")
                End If

                If p.Type = PromotionType.LineSpecific Then
                    If Value > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                        row = p.GetItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        row("DefaultSelectQty") = dr("DefaultSelectQty")
                        row("IsDefaultSelect") = dr("IsDefaultSelect")
                        row("Value") = dr("Value")
                        p.GetItems.Table.Rows.Add(row)
                    Else
                        row = p.PurchaseItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        p.PurchaseItems.Table.Rows.Add(row)
                    End If
                ElseIf p.Type = PromotionType.LeastExpensive Then
                    row = p.PurchaseItems.Table.NewRow
                    row("ItemId") = pItemId
                    row("ItemName") = pItemName
                    row("DepartmentId") = pDepartmentId
                    row("Image") = pImage
                    row("SKU") = pSKU
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")
                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = Value
                    Else
                        row("PercentOff") = Value
                        row("SetPrice") = DBNull.Value
                    End If
                    p.PurchaseItems.Table.Rows.Add(row)
                End If
            End While

        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        Dim tmp As DataView = New DataView
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If
        Return p
    End Function
    Public Shared Function GetRow(ByVal Id As Integer, ByVal ByItem As Boolean) As PromotionRow
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing
        Dim p As PromotionRow = New PromotionRow()

        Dim s As DateTime = Now
        Dim SubSQL As String = " "
        If ByItem Then
            SubSQL &= "i.ItemId = "
        Else
            SubSQL &= "m.Id = "
        End If
        SubSQL &= Id & " "
        Dim MemberId As Integer = Utility.Common.GetCurrentMemberId()
        Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
        Dim SQL As String = "SELECT " & vbCrLf &
         "	m.Type, " & vbCrLf &
         "	m.Description, " & vbCrLf &
         "	m.EndingDate, " & vbCrLf &
         "	i.ItemName, " & vbCrLf &
         "	i.image, " & vbCrLf &
         "  i.SKU, " & vbCrLf &
         "  i.Price, " & vbCrLf &
         "  (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = i.itemid order by o.sortorder) as choicename, " & vbCrLf &
         "  coalesce(tmp.lowprice,i.price) as lowprice, " & vbCrLf &
         "  coalesce(tmp.highprice,i.price) as highprice, " & vbCrLf &
         "	l.MixMatchId, " & vbCrLf &
         "	l.ItemId, " & vbCrLf &
         "	l.DiscountType, " & vbCrLf &
         "	l.[Value], " & vbCrLf &
         "	m.DiscountType as PromotionType, " & vbCrLf &
         "	m.LinesToTrigger, " & vbCrLf &
         "	m.TimesApplicable, " & vbCrLf &
         "	m.Mandatory, " & vbCrLf &
         "	m.[Optional] " & vbCrLf &
         "	, sdi.DepartmentId,coalesce(IsDefaultSelect,0) as IsDefaultSelect,coalesce(DefaultSelectQty,0) as  DefaultSelectQty " & vbCrLf &
         "FROM " & vbCrLf &
         "	MixMatchLine l " & vbCrLf &
         "	INNER JOIN MixMatch m ON l.MixMatchId = m.Id " & vbCrLf &
         "	INNER JOIN StoreItem i ON l.ItemId = i.ItemId " & vbCrLf &
         "	left outer join (select itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem group by itemgroupid) tmp on tmp.itemgroupid = i.itemgroupid " & vbCrLf &
         "	Left JOIN StoreDepartmentItem sdi ON l.ItemId = sdi.ItemId " & vbCrLf &
         "WHERE " & vbCrLf &
         "	" & SubSQL & vbCrLf &
         "	AND m.IsActive = 1 and i.IsActive=1" & vbCrLf &
         "	AND getdate() between coalesce(m.startingdate,getdate()) and coalesce(m.endingdate + 1,getdate() + 1)" & vbCrLf &
         "	AND not exists (select itemid from freegift where isactive = 1 and itemid = l.itemid) " & vbCrLf &
         "ORDER BY " & vbCrLf &
         "	l.IsDefaultSelect DESC,m.Id, " & vbCrLf &
         "	l.[LineNo]"


        'End Edit Feb 20 2014
        Dim Value As Double, pItemId As Integer, pItemName As String, pSKU, pImage As String, pDepartmentId As Integer
        Dim dr As SqlDataReader = Nothing
        Dim row As DataRow
        Try
            ' If DB.IsOpen Then
            Dim db1 As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            'dr = DB.GetReader(SQL)
            dr = db1.ExecuteReader(SQL)
            Dim Buy As Boolean = False
            Dim Content As String = String.Empty
            Dim sGet As String = String.Empty
            Dim Counter As Integer = 0


            While dr.Read
                Value = dr("Value")
                pItemId = dr("ItemId")
                pItemName = dr("ItemName")
                'Modified by: Lam Le
                'Modified Date: November 24, 2009
                'pDepartmentId = dr("DepartmentId")
                If dr.IsDBNull(dr.GetOrdinal("DepartmentId")) Then
                    pDepartmentId = 23
                Else
                    pDepartmentId = dr("DepartmentId")
                End If
                If Not IsDBNull(dr("Image")) Then pImage = dr("Image") Else pImage = String.Empty
                If Not IsDBNull(dr("choicename")) Then pItemName &= " - " & dr("choicename")
                pSKU = dr("sku")

                If p.LinesToTrigger = 0 Then
                    If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                        p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                        If Not p Is Nothing Then
                            dr.Close()
                            Return p
                        End If
                    End If
                    p.LinesToTrigger = dr("linestotrigger")
                    p.Mandatory = dr("mandatory")
                    p.[Optional] = dr("optional")
                    p.MixMatchId = dr("MixMatchId")
                    p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                    p.TimesApplicable = dr("TimesApplicable")
                    p.Description = dr("Description")
                    p.EndingDate = dr("EndingDate")
                    p.MixMatchType = dr("Type")
                End If

                If p.Type = PromotionType.LineSpecific Then
                    If Value > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                        row = p.GetItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        row("DefaultSelectQty") = dr("DefaultSelectQty")
                        row("IsDefaultSelect") = dr("IsDefaultSelect")
                        row("Value") = dr("Value")
                        p.GetItems.Table.Rows.Add(row)
                    Else
                        row = p.PurchaseItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        p.PurchaseItems.Table.Rows.Add(row)
                    End If
                ElseIf p.Type = PromotionType.LeastExpensive Then
                    row = p.PurchaseItems.Table.NewRow
                    row("ItemId") = pItemId
                    row("ItemName") = pItemName
                    row("DepartmentId") = pDepartmentId
                    row("Image") = pImage
                    row("SKU") = pSKU
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")
                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = Value
                    Else
                        row("PercentOff") = Value
                        row("SetPrice") = DBNull.Value
                    End If
                    p.PurchaseItems.Table.Rows.Add(row)
                End If
            End While
            ' End If
        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        Dim tmp As DataView = New DataView
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If

        'System.Web.HttpContext.Current.Cache.Insert("Promotion__" & p.MixMatchId & "__Member__" & MemberId, p, Nothing, DateTime.Now.AddSeconds(PromotionCacheTime), TimeSpan.Zero)

        Return p
    End Function
    Public Shared Function GetRowInCart(ByVal DB As Database, ByVal Id As Integer, ByVal MixmatchID As Integer) As PromotionRow
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing
        'If DB Is Nothing Then
        '    Dim bb As New BasePage
        '    DB = bb.DB
        'End If
        Dim p As PromotionRow = New PromotionRow()

        Dim s As DateTime = Now
        Dim SubSQL As String = "m.Id = "

        SubSQL &= "(SELECT TOP 1 mm.Id FROM MixMatch mm INNER JOIN MixMatchLine mml ON mm.Id = mml.MixMatchId inner join storeitem si on mml.itemid = si.itemid WHERE qtyonhand > 0 and mm.IsActive=1 and MixMatchId = m.Id and MixMatchId=" & MixmatchID & " AND si.ItemId = " & Id & ") "



        Dim MemberId As Integer = 0
        If IsNumeric(System.Web.HttpContext.Current.Session("MemberId")) Then MemberId = System.Web.HttpContext.Current.Session("MemberId")



        Dim CustomerPriceGroupId As Integer = 0
        If Not IsNumeric(System.Web.HttpContext.Current.Session("CustomerPriceGroupId")) Then
            CustomerPriceGroupId = DB.ExecuteScalar("select top 1 coalesce(CustomerPriceGroupId,0) from customer where customerid = (select top 1 memberid from member where memberid = " & MemberId & ")")
        End If
        System.Web.HttpContext.Current.Session("CustomerPriceGroupId") = CustomerPriceGroupId

        Dim SQL As String = "SELECT " & vbCrLf & _
         "	i.ItemName, " & vbCrLf & _
         "	i.image, " & vbCrLf & _
         "  i.SKU, " & vbCrLf & _
         "  i.Price, " & vbCrLf & _
         "  (select top 1 choicename from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = i.itemid order by o.sortorder) as choicename, " & vbCrLf & _
         "  coalesce(tmp.lowprice,i.price) as lowprice, " & vbCrLf & _
         "  coalesce(tmp.highprice,i.price) as highprice, " & vbCrLf & _
         "	l.MixMatchId, " & vbCrLf & _
         "	l.ItemId, " & vbCrLf & _
         "	l.DiscountType, " & vbCrLf & _
         "	l.[Value], " & vbCrLf & _
         "	m.DiscountType as PromotionType, " & vbCrLf & _
         "	m.LinesToTrigger, " & vbCrLf & _
         "	m.TimesApplicable, " & vbCrLf & _
         "	m.Mandatory, " & vbCrLf & _
         "	m.[Optional] " & vbCrLf & _
         "	, sdi.DepartmentId " & vbCrLf & _
         "FROM " & vbCrLf & _
         "	MixMatchLine l " & vbCrLf & _
         "	INNER JOIN MixMatch m ON l.MixMatchId = m.Id " & vbCrLf & _
         "	INNER JOIN StoreItem i ON l.ItemId = i.ItemId " & vbCrLf & _
         "	left outer join (select itemgroupid, coalesce(min(price),0) as lowprice, coalesce(max(price),0) as highprice from storeitem group by itemgroupid) tmp on tmp.itemgroupid = i.itemgroupid " & vbCrLf & _
         "	Left JOIN StoreDepartmentItem sdi ON l.ItemId = sdi.ItemId " & vbCrLf & _
         "WHERE " & vbCrLf & _
         "	" & SubSQL & vbCrLf & _
         "	AND m.IsActive = 1 " & vbCrLf & _
         "	AND getdate() between coalesce(m.startingdate,getdate()) and coalesce(m.endingdate + 1,getdate() + 1)" & vbCrLf & _
         "	AND not exists (select itemid from freegift where isactive = 1 and itemid = l.itemid) " & vbCrLf & _
         "	AND (m.CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId) & " or m.CustomerPriceGroupId is null) " & vbCrLf & _
         "ORDER BY " & vbCrLf & _
         "	m.Id, " & vbCrLf & _
         "	l.[LineNo]"
        ''KHOA nho bo MM trong Free Gift
        Dim Value As Double, pItemId As Integer, pItemName As String, pSKU, pImage As String, pDepartmentId As Integer
        Dim dr As SqlDataReader = Nothing
        Dim row As DataRow
        Try
            dr = DB.GetReader(SQL)
            Dim Buy As Boolean = False
            Dim Content As String = String.Empty
            Dim sGet As String = String.Empty
            Dim Counter As Integer = 0
            While dr.Read
                Value = dr("Value")
                pItemId = dr("ItemId")
                pItemName = dr("ItemName")
                'Modified by: Lam Le
                'Modified Date: November 24, 2009
                'pDepartmentId = dr("DepartmentId")
                If dr.IsDBNull(dr.GetOrdinal("DepartmentId")) Then
                    pDepartmentId = 23
                Else
                    pDepartmentId = dr("DepartmentId")
                End If
                If Not IsDBNull(dr("Image")) Then pImage = dr("Image") Else pImage = String.Empty
                If Not IsDBNull(dr("choicename")) Then pItemName &= " - " & dr("choicename")
                pSKU = dr("sku")

                If p.LinesToTrigger = 0 Then
                    If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                        p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                        If Not p Is Nothing Then
                            dr.Close()
                            Return p
                        End If
                    End If
                    p.LinesToTrigger = dr("linestotrigger")
                    p.Mandatory = dr("mandatory")
                    p.[Optional] = dr("optional")
                    p.MixMatchId = dr("MixMatchId")
                    p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                    p.TimesApplicable = dr("TimesApplicable")
                End If

                If p.Type = PromotionType.LineSpecific Then
                    If Value > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                        row = p.GetItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        p.GetItems.Table.Rows.Add(row)
                    Else
                        row = p.PurchaseItems.Table.NewRow
                        row("ItemId") = pItemId
                        row("ItemName") = pItemName
                        row("DepartmentId") = pDepartmentId
                        row("Image") = pImage
                        row("SKU") = pSKU
                        row("Price") = dr("Price")
                        row("LowPrice") = dr("LowPrice")
                        row("HighPrice") = dr("HighPrice")
                        If dr("DiscountType") = "Deal Price" Then
                            row("PercentOff") = DBNull.Value
                            row("SetPrice") = Value
                        Else
                            row("PercentOff") = Value
                            row("SetPrice") = DBNull.Value
                        End If
                        p.PurchaseItems.Table.Rows.Add(row)
                    End If
                ElseIf p.Type = PromotionType.LeastExpensive Then
                    row = p.PurchaseItems.Table.NewRow
                    row("ItemId") = pItemId
                    row("ItemName") = pItemName
                    row("DepartmentId") = pDepartmentId
                    row("Image") = pImage
                    row("SKU") = pSKU
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")
                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = Value
                    Else
                        row("PercentOff") = Value
                        row("SetPrice") = DBNull.Value
                    End If
                    p.PurchaseItems.Table.Rows.Add(row)
                End If
            End While

        Catch ex As Exception

        End Try
        Core.CloseReader(dr)

        Dim tmp As DataView = New DataView
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If

        'System.Web.HttpContext.Current.Cache.Insert("Promotion__" & p.MixMatchId & "__Member__" & MemberId, p, Nothing, DateTime.Now.AddSeconds(PromotionCacheTime), TimeSpan.Zero)

        Return p
    End Function
    ''' <summary>
    ''' Get Distinct promotion row - Trung Nguyen add
    ''' </summary>
    ''' <param name="DB"></param>
    ''' <param name="Id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDistinctRow(ByVal DB As Database, ByVal Id As Integer) As PromotionRow
        If System.Web.HttpContext.Current Is Nothing Then Return Nothing

        Dim p As PromotionRow = New PromotionRow()

        Dim s As DateTime = Now
        Dim SubSQL As String = "m.Id = " & Id & " "

        Dim MemberId As Integer = 0
        If IsNumeric(System.Web.HttpContext.Current.Session("MemberId")) Then MemberId = System.Web.HttpContext.Current.Session("MemberId")




        Dim Value As Double, pItemId As Integer, pItemName As String, pSKU, URLCode, pImage As String, pDepartmentId As Integer

        Dim edb As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Dim SP_STOREPROMO_GETLIST As String = "sp_PromoClick_GetList"
        Dim cmd As DbCommand = edb.GetStoredProcCommand(SP_STOREPROMO_GETLIST)
        edb.AddInParameter(cmd, "Id", DbType.Int32, Id)
        ''edb.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)

        Dim dr As IDataReader = edb.ExecuteReader(cmd)
        Dim Buy As Boolean = False
        Dim Content As String = String.Empty
        Dim sGet As String = String.Empty
        Dim Counter As Integer = 0

        Dim row As DataRow
        While dr.Read
            Value = dr("Value")
            pItemId = dr("ItemId")
            pItemName = dr("ItemName")
            URLCode = dr("URLCode")
            pDepartmentId = dr("DepartmentId")
            If Not IsDBNull(dr("Image")) Then pImage = dr("Image") Else pImage = String.Empty
            If Not IsDBNull(dr("choicename")) Then pItemName &= " - " & dr("choicename")
            pSKU = dr("sku")

            If p.LinesToTrigger = 0 Then
                If Not System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId")) Is Nothing Then
                    p = System.Web.HttpContext.Current.Cache("Promotion_" & dr("MixMatchId"))
                    If Not p Is Nothing Then
                        dr.Close()
                        Return p
                    End If
                End If
                p.LinesToTrigger = dr("linestotrigger")
                p.Mandatory = dr("mandatory")
                p.[Optional] = dr("optional")
                p.MixMatchId = dr("MixMatchId")
                p.Type = IIf(dr("PromotionType") = "Line spec.", PromotionType.LineSpecific, PromotionType.LeastExpensive)
                p.TimesApplicable = dr("TimesApplicable")
            End If

            If p.Type = PromotionType.LineSpecific Then
                If Value > 0 AndAlso p.Mandatory <> p.LinesToTrigger Then
                    row = p.GetItems.Table.NewRow
                    row("ItemId") = pItemId
                    row("ItemName") = pItemName
                    row("DepartmentId") = pDepartmentId
                    row("Image") = pImage
                    row("SKU") = pSKU
                    row("URLCode") = URLCode
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")
                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = Value
                    Else
                        row("PercentOff") = Value
                        row("SetPrice") = DBNull.Value
                    End If
                    p.GetItems.Table.Rows.Add(row)
                Else
                    row = p.PurchaseItems.Table.NewRow
                    row("ItemId") = pItemId
                    row("ItemName") = pItemName
                    row("DepartmentId") = pDepartmentId
                    row("Image") = pImage
                    row("SKU") = pSKU
                    row("URLCode") = URLCode
                    row("Price") = dr("Price")
                    row("LowPrice") = dr("LowPrice")
                    row("HighPrice") = dr("HighPrice")
                    If dr("DiscountType") = "Deal Price" Then
                        row("PercentOff") = DBNull.Value
                        row("SetPrice") = Value
                    Else
                        row("PercentOff") = Value
                        row("SetPrice") = DBNull.Value
                    End If
                    p.PurchaseItems.Table.Rows.Add(row)
                End If
            ElseIf p.Type = PromotionType.LeastExpensive Then
                row = p.PurchaseItems.Table.NewRow
                row("ItemId") = pItemId
                row("ItemName") = pItemName
                row("DepartmentId") = pDepartmentId
                row("Image") = pImage
                row("SKU") = pSKU
                row("URLCode") = URLCode
                row("Price") = dr("Price")
                row("LowPrice") = dr("LowPrice")
                row("HighPrice") = dr("HighPrice")
                If dr("DiscountType") = "Deal Price" Then
                    row("PercentOff") = DBNull.Value
                    row("SetPrice") = Value
                Else
                    row("PercentOff") = Value
                    row("SetPrice") = DBNull.Value
                End If
                p.PurchaseItems.Table.Rows.Add(row)
            End If
        End While
        dr.Close()

        Dim tmp As DataView = New DataView
        If p.Type = PromotionType.LeastExpensive Then
            p.AllAreFree = True
            tmp.Table = p.PurchaseItems.Table.Copy
            tmp.Sort = "lowprice asc"
            If tmp.Count > 0 Then
                row = p.GetItems.Table.NewRow()
                row("ItemId") = tmp(0)("ItemId")
                row("ItemName") = tmp(0)("ItemName")
                row("DepartmentId") = tmp(0)("DepartmentId")
                row("Image") = tmp(0)("Image")
                row("SKU") = tmp(0)("SKU")
                row("URLCode") = tmp(0)("URLCode")
                row("Price") = tmp(0)("Price")
                row("LowPrice") = tmp(0)("LowPrice")
                row("HighPrice") = tmp(0)("HighPrice")
                row("PercentOff") = 100
                row("SetPrice") = DBNull.Value
                p.GetItems.Table.Rows.Add(row)
            End If
        Else
            tmp.Table = p.GetItems.Table.Copy
            tmp.RowFilter = "(percentoff < 100 or setprice > 0)"
            p.AllAreFree = tmp.Count = 0
        End If

        'System.Web.HttpContext.Current.Cache.Insert("Promotion__" & p.MixMatchId & "__Member__" & MemberId, p, Nothing, DateTime.Now.AddSeconds(PromotionCacheTime), TimeSpan.Zero)

        Return p
    End Function
End Class

Public Class PromotionCollection
    Inherits GenericCollection(Of PromotionRow)
End Class
