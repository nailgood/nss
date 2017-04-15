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
Imports Components.Core
Imports NavisionCodes
Imports Utility

Namespace DataLayer

    Public Class StoreCartItemRow
        Inherits StoreCartItemRowBase

        Private m_Promotion As PromotionRow
        Private m_Pricing As ItemPricing

        Private Shared Sub Initialize(ByRef row As StoreCartItemRow, ByVal CustomerPriceGroupId As Integer)
            If row.Type <> "item" Or row.IsFreeGift Or row.IsFreeSample Then Exit Sub

            'Kiem tra lai xem co MixMatchId moi phat sinh ko
            'Chi kiem tra neu la item le (case ko kiem tra)
            If row.AddType <> 2 Then
                row.MixMatchId = StoreItemRow.GetMixMatchIdDescription(row.MixMatchId, row.ItemId, CustomerPriceGroupId, Utility.Common.MixmatchType.Normal, row.MixMatchDescription)
            End If


            Dim tmp As String = StoreItemRow.GetCustomerDiscountWithQuantity(row.ItemId, System.Web.HttpContext.Current.Session("MemberId"), row.Quantity, row.AddType) 'Khoa them AddType cho Buy in Bulk
            If IsNumeric(tmp) Then row.LowSalePrice = FormatNumber(tmp, 2)

            row.LowPrice = row.Price
            row.HighPrice = row.Price
        End Sub

        Public Property Promotion() As PromotionRow
            Get
                Return m_Promotion
            End Get
            Set(ByVal value As PromotionRow)
                m_Promotion = value
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

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CartItemId As Integer)
            MyBase.New(DB, CartItemId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderNo As String, ByVal SKU As String, ByVal IsItem As Boolean, Optional ByVal ShippingAgentCode As String = "", Optional ByVal IsNavisionOrderNo As Boolean = False)
            MyBase.New(DB, OrderNo, SKU, IsItem, ShippingAgentCode, IsNavisionOrderNo)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal IsCoupon As Boolean)
            MyBase.New(DB, OrderId, ItemId, IsCoupon)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String)
            MyBase.New(DB, OrderId, ItemId, Attributes)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFreeItem As Boolean, ByVal IsPromo As Boolean)
            MyBase.New(DB, OrderId, ItemId, Attributes, Swatches, IsFreeItem, IsPromo)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFreeItem As Boolean, ByVal IsPromo As Boolean, ByVal IsFreeGift As Boolean)
            MyBase.New(DB, OrderId, ItemId, Attributes, Swatches, IsFreeItem, IsPromo, IsFreeGift)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal CarrierId As Integer)
            MyBase.New(DB, OrderId, CarrierId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CartItemId As Integer) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, CartItemId)
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)

            Return row
        End Function
        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal IsCoupon As Boolean) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderId, ItemId, IsCoupon)
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)

            Return row
        End Function

        Public Function GetLowestPrice() As Double
            Return GetLowestPrice(True)
        End Function

        Public Function GetLowestPrice(ByVal CheckPromotionPrice As Boolean) As Double
            Dim x As Double = Price
            If Promotion Is Nothing Or PromotionID <> Nothing Then
                If x > CustomerPrice AndAlso CustomerPrice > 0 Then x = CustomerPrice
                If x > QuantityPrice AndAlso QuantityPrice > 0 Then x = QuantityPrice
            End If
            If CheckPromotionPrice AndAlso x > PromotionPrice AndAlso PromotionPrice > 0 Then x = PromotionPrice

            Return x
        End Function

        Public Function PromotionIsLowestPrice() As Boolean
            PromotionIsLowestPrice = False
            Dim x As Double = Price
            If x > CustomerPrice AndAlso CustomerPrice > 0 Then x = CustomerPrice
            If x > QuantityPrice AndAlso QuantityPrice > 0 Then x = QuantityPrice
            If x > PromotionPrice AndAlso PromotionPrice > 0 Then PromotionIsLowestPrice = True
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderNo As String, ByVal SKU As String, ByVal IsItem As Boolean) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderNo, SKU, IsItem)
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)

            Return row
        End Function

        Public Shared Function GetRowNavision(ByVal DB As Database, ByVal OrderNo As String, ByVal SKU As String, ByVal IsItem As Boolean, Optional ByVal ShippingAgentCode As String = "") As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderNo, SKU, IsItem, ShippingAgentCode, True)
            row.Load(True)

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderNo As String, ByVal SKU As String) As StoreCartItemRow
            Return GetRow(DB, OrderNo, SKU, True)
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer, ByVal CarrierId As Integer) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderId, CarrierId)
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderId, ItemId, Attributes)
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFree As Boolean, ByVal IsPromo As Boolean, ByVal AddType As Integer) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderId, ItemId, Attributes, Swatches, IsFree, IsPromo)
            row.AddType = AddType
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)
            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFree As Boolean, ByVal IsPromo As Boolean, ByVal IsFreeGift As Boolean, ByVal AddType As Integer) As StoreCartItemRow
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, OrderId, ItemId, Attributes, Swatches, IsFree, IsPromo, IsFreeGift)
            row.AddType = AddType
            row.Load()
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)
            Return row
        End Function

        Public Shared Function GetFreeCartItemByMixMamatch(ByVal DB As Database, ByVal OrderId As Integer, ByVal itemId As Integer, ByVal mmId As Integer) As StoreCartItemRow

            Dim row As StoreCartItemRow
            row = New StoreCartItemRow(DB)
            row.LoadFreeItemByMixmatch(OrderId, itemId, mmId)
            Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Initialize(row, customerPriceGroupId)
            Return row
        End Function
        Public Shared Function GetCartItemIdFreeGift(ByVal DB As Database, ByVal OrderId As Integer) As Integer
            Dim Id As Integer = 0
            Try
                Dim SQL As String = "SELECT ISNULL(CartItemId,0) from StoreCartItem WHERE OrderId=" & OrderId & " AND IsFreeGift = 1"
                Id = DB.ExecuteScalar(SQL)
            Catch ex As Exception
                Id = 0
            End Try
            Return Id
        End Function

        Public Shared Function CountOtherCartItemSameMixmatch(ByVal DB As Database, ByVal OrderId As Integer, ByVal CartItemId As Integer, ByVal MixMatchId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim SQL As String = "SELECT COUNT(CartItemId) from StoreCartItem WHERE [Type] = 'item' AND IsFreeItem = 0 AND OrderId=" & OrderId & " AND CartItemId <> " & CartItemId & " AND MixMatchId = " & MixMatchId
                result = DB.ExecuteScalar(SQL)
            Catch ex As Exception
                result = 0
            End Try
            Return result
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CartItemId As Integer)
            Dim row As StoreCartItemRow

            row = New StoreCartItemRow(DB, CartItemId)
            row.Remove()
        End Sub
        Public Shared Function IsItemIncart(ByVal DB As Database, ByVal ItemId As Integer, ByVal MemberId As Integer, ByVal url As String) As Boolean
            Try
                Dim result As Integer = 0
                Dim checkItemPoint As Boolean
                If (url.Contains("rewardpoint.aspx")) Then
                    checkItemPoint = True
                Else
                    checkItemPoint = False
                End If
                result = DB.ExecuteScalar("SELECT  dbo.fc_StoreItem_IsInCart2(" & CStr(MemberId) & ", " & ItemId & " ," & IIf(checkItemPoint = True, "1", "0") & ")")
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function ValidateFreeItem(ByVal OrderId As Integer, ByVal MixMatchId As Integer) As Boolean
            Dim result As Boolean
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "select [dbo].[fc_StoreCartItem_ValidateFreeItem](" & MixMatchId & "," & OrderId & ")"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                result = CType(db.ExecuteScalar(cmd), Boolean)
            Catch ex As Exception
                SendMailLog("ValidateFreeItem(" & OrderId & "," & MixMatchId & ")", ex)
            End Try
            Return result

            'Dim result As Boolean = True
            'Try
            '    Dim dr As SqlDataReader = Nothing
            '    dr = DB.GetReader("SELECT SUM(Quantity) AS 'Quantity' FROM StoreCartItem WHERE OrderId = " & OrderId & " AND MixMatchId = " & MixMatchId & " AND IsFreeItem = 1 GROUP BY TotalFreeAllowed")
            '    Dim Quantity As Integer = 0
            '    If dr.HasRows Then
            '        While dr.Read
            '            Quantity = dr(0)
            '        End While
            '    End If
            '    Core.CloseReader(dr)
            '    If Quantity = 0 Then
            '        result = True
            '    Else
            '        result = False
            '    End If
            'Catch ex As Exception
            'End Try

            'Return result
        End Function

        Public Shared Sub RemoveByOrderId(ByVal DB As Database, ByVal OrderId As Integer)
            Dim SQL As String = ""

            SQL = "DELETE FROM StoreCartItem WHERE OrderId = " & DB.Number(OrderId)
            DB.ExecuteSQL(SQL)
        End Sub 'Remove StoreCartItemByOrderId
        ''''''''''''''''''''''''''''

        'Custom Methods
        Public Sub New(ByVal database As Database, ByVal dbStoreItem As StoreItemRow)
            MyBase.New(database)

            ItemId = dbStoreItem.ItemId
            ItemGroupId = dbStoreItem.ItemGroupId
            ItemName = dbStoreItem.ItemName
            Price = FormatNumber(dbStoreItem.Price, 2)
            Weight = dbStoreItem.Weight
            SalePrice = dbStoreItem.SalePrice
            PriceDesc = dbStoreItem.PriceDesc
            IsTaxFree = dbStoreItem.IsTaxFree
            Image = dbStoreItem.Image
            CarrierType = dbStoreItem.CarrierType
            Status = dbStoreItem.Status
            SKU = dbStoreItem.SKU
            PriceDesc = dbStoreItem.PriceDesc
            IsOversize = dbStoreItem.IsOversize
            IsHazMat = dbStoreItem.IsHazMat
            IsFreeShipping = dbStoreItem.IsFreeShipping
            IsFreeSample = dbStoreItem.IsFreeSample
            IsFlammable = dbStoreItem.IsFlammable
        End Sub 'New

        Public Sub UpdateItemDetails(ByVal ID As Integer)
            Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRowUpdateForCart(DB, ID)
            ItemId = dbStoreItem.ItemId
            ItemGroupId = dbStoreItem.ItemGroupId
            ItemName = dbStoreItem.ItemName
            Price = Utility.Common.RoundCurrency(dbStoreItem.Price)
            Weight = dbStoreItem.Weight
            SalePrice = dbStoreItem.SalePrice
            PriceDesc = dbStoreItem.PriceDesc
            IsTaxFree = dbStoreItem.IsTaxFree
            Image = dbStoreItem.Image
            Status = dbStoreItem.Status
            SKU = dbStoreItem.SKU
            PriceDesc = dbStoreItem.PriceDesc
            IsOversize = dbStoreItem.IsOversize
            IsHazMat = dbStoreItem.IsHazMat
            IsFreeShipping = dbStoreItem.IsFreeShipping
            IsFreeSample = dbStoreItem.IsFreeSample
        End Sub

        Private Sub UpdateOrder(ByVal DB As Database, ByVal OrderId As Integer)
            Dim Order As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
            Dim SQL As String = "select coalesce(sum(subtotal), 0) from storecartitem where [type] = 'item' and orderid=" & OrderId
            Order.BaseSubTotal = DB.ExecuteScalar(SQL)

            SQL = "select coalesce(sum(total), 0) from storecartitem where [type] = 'item' and orderid=" & OrderId
            Order.SubTotal = DB.ExecuteScalar(SQL)
            Order.Total = Order.SubTotal + Order.Shipping + Order.Tax - Order.Discount
            Order.Update()
        End Sub
      
        Public Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreCartItem.vb", func, ex)
        End Sub

        Public Shared Function GetItemListIdByOrderId(ByVal OrderId As String) As String
            Dim result As String = ""
            If OrderId > 0 Then
                Dim reader As SqlDataReader = Nothing
                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim sp As String = "sp_StoreCartItem_GetItemListIdByOrderId"
                    Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                    db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                    reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                    If reader.Read() Then
                        result = reader(0).ToString()
                    End If

                    Core.CloseReader(reader)
                Catch ex As Exception
                    Core.CloseReader(reader)
                    Email.SendError("ToError500", "GetItemListIdByOrderId", "OrderId: " & OrderId & "<br>Exception: " & ex.ToString())
                End Try
            End If
            Return result
        End Function

        Public Shared Function GetCartItem(ByVal _Database As Database, ByVal OrderId As Integer, ByVal itemId As Integer, ByVal isRewardpoint As Boolean, ByVal isFreeItem As Boolean) As StoreCartItemRow
            Dim dr As SqlDataReader = Nothing
            Dim row As StoreCartItemRow
            Dim sql As String = String.Empty
            Try

                sql = "Select * from StoreCartItem where OrderId=" & OrderId & " and ItemId=" & itemId
                sql &= IIf(isRewardpoint, " and IsRewardPoints=1", " and IsRewardPoints=0")
                sql &= IIf(isFreeItem, " and IsFreeItem=1", " and IsFreeItem=0")

                dr = _Database.GetReader(sql)
                If dr.Read Then
                    row = New StoreCartItemRow(_Database)
                    row.Load(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCartItem(OrderId=" & OrderId & ",ItemId=" & itemId & ",isRewardpoint=" & isRewardpoint & ",isFreeItem=" & isFreeItem & ") & sql=" & sql, ex)
            End Try

            If Not row Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Initialize(row, customerPriceGroupId)
            End If
            Return row
        End Function

        Public Shared Function GetListMixMatchId(ByVal _Database As Database, ByVal OrderId As Integer) As List(Of Integer)
            Dim dr As SqlDataReader = Nothing
            Dim result As New List(Of Integer)
            Dim mmId As Integer = 0
            Dim sql As String = String.Empty

            Try
                sql = "SELECT DISTINCT  MixmatchId FROM StoreCartItem ci LEFT JOIN MixMatch mm ON (mm.Id=ci.MixMatchId) WHERE OrderId=" & OrderId & " and ci.[Type]='item' and IsFreeItem=0 and mm.[Type]=1"
                dr = _Database.GetReader(sql)
                While dr.Read
                    mmId = Convert.ToInt32(dr.Item("MixmatchId"))
                    If mmId > 0 Then
                        result.Add(mmId)
                    End If
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListMixMatchId(OrderId=" & OrderId & ")", ex)
            End Try
            Return result
        End Function

        Public Shared Function GetListMixMatchIdLogin(ByVal _Database As Database, ByVal OrderId As Integer) As List(Of Integer)
            Dim dr As SqlDataReader = Nothing
            Dim lst As New List(Of Integer)
            Dim customerpriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Try
                Dim id As Integer = 0
                Dim sp As String = "sp_StoreCartItem_GetListMixMatchForLogin"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.InParam("CutomerPriceGroupId", SqlDbType.Int, 0, customerpriceGroupId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    id = Convert.ToInt32(dr.Item("MixMatchId"))
                    lst.Add(id)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return lst
        End Function
        Public Shared Function GetListProductReview(ByVal _Database As Database, ByVal OrderId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow

            Try
                Dim sp As String = "sp_StoreCartItem_GetListProductReview"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                dr = cmd.ExecuteReader()

                While dr.Read
                    row = New StoreCartItemRow(_Database)
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListProductReview(ByVal DB As Database, ByVal OrderId As Integer) - OrderId= " & OrderId.ToString(), ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
                If c.Count > 1 Then
                    c(c.Count - 1).Final = "1"
                End If
            End If
            Return c
        End Function
        Public Shared Function GetCartItems(ByVal _Database As Database, ByVal OrderId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow

            Try
                Dim sp As String = "sp_StoreCartItem_GetListCartByOrder"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                dr = cmd.ExecuteReader()

                If dr.HasRows Then
                    While dr.Read()
                        row = New StoreCartItemRow(_Database)
                        row.Load(dr)
                        c.Add(row)
                    End While
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCartItems(ByVal DB As Database, ByVal OrderId As Integer) - OrderId= " & OrderId.ToString(), ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
                If c.Count > 1 Then
                    c(c.Count - 1).Final = "1"
                End If
            End If
            Return c
        End Function
        Public Shared Function GetListFreeCartItemByMixmatch(ByVal _Database As Database, ByVal OrderId As Integer, ByVal mixmatchId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow
            Try
                Dim sp As String = "sp_StoreCartItem_GetListFreeCartItemByMixmatch"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.InParam("MixmatchId", SqlDbType.Int, 0, mixmatchId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    row = New StoreCartItemRow(_Database)
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetListFreeCartItemByMixmatch(ByVal DB As Database, ByVal OrderId As Integer=" & OrderId & ",ByVal mixmatchId As Integer=" & mixmatchId & ")", ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
                If c.Count > 1 Then
                    c(c.Count - 1).Final = "1"
                End If
            End If
            Return c
        End Function

        Public Shared Function GetListPopupCart(ByVal OrderId As Integer) As StoreCartItemCollection
            Dim c As New StoreCartItemCollection
            If OrderId > 0 Then
                Dim r As SqlDataReader = Nothing
                Dim row As StoreCartItemRow

                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim sp As String = "sp_StoreCartItem_GetListPopupCart"
                    Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                    db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                    r = db.ExecuteReader(cmd)

                    While r.Read
                        row = New StoreCartItemRow(Nothing)
                        row.CartItemId = Convert.ToInt32(r.Item("CartItemId"))
                        row.ItemId = Convert.ToInt32(r.Item("ItemId"))
                        If IsDBNull(r.Item("ItemName")) Then
                            row.ItemName = Nothing
                        Else
                            row.ItemName = Convert.ToString(r.Item("ItemName"))
                        End If
                        If IsDBNull(r.Item("FreeItemIds")) Then
                            row.FreeItemIds = Nothing
                        Else
                            row.FreeItemIds = Convert.ToString(r.Item("FreeItemIds"))
                        End If
                        If IsDBNull(r.Item("Image")) Then
                            row.Image = Nothing
                        Else
                            row.Image = Convert.ToString(r.Item("Image"))
                        End If
                        row.Quantity = Convert.ToInt32(r.Item("Quantity"))

                        If IsDBNull(r.Item("Total")) Then
                            row.Total = Nothing
                        Else
                            row.Total = Convert.ToDouble(r.Item("Total"))
                        End If
                        If IsDBNull(r.Item("SubTotal")) Then
                            row.SubTotal = Nothing
                        Else
                            row.SubTotal = Convert.ToDouble(r.Item("SubTotal"))
                        End If
                        row.IsFreeItem = Convert.ToBoolean(r.Item("IsFreeItem"))
                        row.IsFreeGift = Convert.ToBoolean(r.Item("IsFreeGift"))
                        If IsDBNull(r.Item("SKU")) Then
                            row.SKU = Nothing
                        Else
                            row.SKU = Convert.ToString(r.Item("SKU"))
                        End If
                        If IsDBNull(r.Item("Type")) Then
                            If row.SKU <> Nothing Then
                                row.Type = "item"
                            Else
                                row.Type = Nothing
                            End If
                        Else
                            row.Type = Convert.ToString(r.Item("Type"))
                        End If

                        If IsDBNull(r.Item("RewardPoints")) Then
                            row.RewardPoints = Nothing
                        Else
                            row.RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
                        End If
                        If IsDBNull(r.Item("IsRewardPoints")) Then
                            row.IsRewardPoints = False
                        Else
                            row.IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
                        End If
                        If IsDBNull(r.Item("SubTotalPoint")) Then
                            row.SubTotalPoint = Nothing
                        Else
                            row.SubTotalPoint = Convert.ToInt32(r.Item("SubTotalPoint"))
                        End If
                        If IsDBNull(r.Item("PromotionID")) Then
                            row.PromotionID = Nothing
                        Else
                            row.PromotionID = Convert.ToInt32(r.Item("PromotionID"))
                        End If
                        Try
                            If IsDBNull(r.Item("AddType")) Then
                                row.AddType = 1
                            Else
                                row.AddType = Convert.ToInt32(r.Item("AddType"))
                            End If
                        Catch ex As Exception
                            row.AddType = 1
                        End Try
                        c.Add(row)
                    End While
                    Core.CloseReader(r)
                Catch ex As Exception
                    Core.CloseReader(r)
                    SendMailLog("GetListPopupCart(ByVal DB As Database, ByVal OrderId As Integer) - OrderId= " & OrderId.ToString(), ex)
                End Try
            End If

            Return c
        End Function

        Public Shared Function GetCartItemByProductCoupon(ByVal _Database As Database, ByVal OrderId As Integer, ByVal productCouponCode As String) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow
            Try
                Dim sp As String = "sp_StoreCartItem_GetListCartByProductCoupon"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.InParam("ProductCouponCode", SqlDbType.VarChar, 100, productCouponCode))
                dr = cmd.ExecuteReader()
                While dr.Read
                    row = New StoreCartItemRow(_Database)
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCartItemByProductCoupon- OrderId= " & OrderId.ToString() & "-productCouponCode=" & productCouponCode, ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
                If c.Count > 1 Then
                    c(c.Count - 1).Final = "1"
                End If
            End If
            Return c
        End Function

        Public Shared Sub RemoveProductCoupon(ByVal _Database As Database, ByVal OrderId As Integer, ByVal Coupon As String)
            Try
                Dim sp As String = "sp_StoreCartItem_RemoveProductCoupon"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.InParam("PromotionCode", SqlDbType.VarChar, 0, Coupon))
                cmd.ExecuteNonQuery()
            Catch ex As Exception

            End Try

        End Sub
        Public Shared Sub ReviseDefaultDiscountPercentItem(ByVal _Database As Database, ByVal orderId As Integer, ByVal mmId As Integer)
            Try
                Dim sp As String = "sp_StoreCartItem_ReviseDefaultDiscountPercentItem"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.InParam("MixMatchId", SqlDbType.Int, 0, mmId))
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                SendMailLog("ReviseDefaultDiscountPercentItem(OrderId=" & orderId & ",MixMatchId=" & mmId & ")", ex)
            End Try

        End Sub

        Public Shared Sub DeleteMixMatchNotValid(ByVal _Database As Database, ByVal OrderId As Integer, ByVal MixMatchid As Integer)
            Try
                Dim sp As String = "sp_StoreCartItem_DeleteMixMatchNotValid"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.InParam("MixMatchid", SqlDbType.Int, 0, MixMatchid))
                cmd.ExecuteNonQuery()
            Catch ex As Exception
            End Try

        End Sub

        Public Shared Function UpdateCartQty(ByVal _Database As Database, ByVal cartItemId As Integer, ByVal orderId As Integer, ByVal qty As Integer, ByVal addByPoint As Boolean) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_UpdateQtyCart"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CartItemId", SqlDbType.Int, 0, cartItemId))
                If (addByPoint) Then
                    cmd.Parameters.Add(_Database.InParam("AddByPoint", SqlDbType.Bit, 0, 1))
                Else
                    cmd.Parameters.Add(_Database.InParam("AddByPoint", SqlDbType.Bit, 0, 0))
                End If
                cmd.Parameters.Add(_Database.InParam("Qty", SqlDbType.Int, 0, qty))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Dim result As Integer = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                    Utility.Common.DeleteCachePopupCart(orderId)
                    Return True
                End If
            Catch ex As Exception
            End Try
            Return False
        End Function
        Public Shared Function InsertItemCartRevise(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_InsertCartRevise"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Dim result As Integer = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
            End Try
            Return False
        End Function
        Public Shared Function ReviseCartItem(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_ReviseCartItem"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Dim result As Integer = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
            End Try
            Return False
        End Function

        Public Shared Function GetCartItemsInPaypalProcess(ByVal _Database As Database, ByVal OrderId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow
            Try
                Dim sp As String = "sp_StoreCartItem_GetListInPaypayProcess"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    row = New StoreCartItemRow(_Database)
                    row.LoadInPaypalProcess(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCartItemsInPaypalProcess(ByVal DB As Database, ByVal OrderId As Integer)", ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
            End If
            Return c
        End Function
        'Public Shared Function GetCartItemsInShipping(ByVal DB As Database, ByVal OrderId As Integer) As StoreCartItemCollection
        '    Dim dr As SqlDataReader = Nothing
        '    Dim c As New StoreCartItemCollection
        '    Dim row As StoreCartItemRow
        '    Dim SQL As String = String.Empty

        '    Try
        '        Dim selectField As String = String.Empty
        '        selectField = "ItemId"
        '        selectField &= ",ItemName"
        '        selectField &= ",IsFreeItem"
        '        selectField &= ",IsFreeGift"
        '        selectField &= ",MixMatchId"
        '        selectField &= ",IsFreeSample"
        '        selectField &= ",CartItemId"
        '        selectField &= ",IsHazMat,IsFlammable"
        '        selectField &= ",PromotionID"
        '        selectField &= ",CouponMessage"
        '        selectField &= ",Type"
        '        selectField &= ",FreeItemIds"
        '        selectField &= ",CarrierType"
        '        selectField &= ",IsOversize"
        '        selectField &= ",IsRushDelivery"
        '        selectField &= ",IsFreeShipping"
        '        selectField &= ",IsLiftGate"
        '        selectField &= ",IsScheduleDelivery"
        '        selectField &= ",IsInsideDelivery"
        '        selectField &= ",AttributeSKU"
        '        selectField &= ",Attributes"
        '        selectField &= ",SKU"
        '        selectField &= ",Quantity"
        '        selectField &= ",CustomerPrice"
        '        selectField &= ",QuantityPrice"
        '        selectField &= ",PromotionPrice"
        '        selectField &= ",Price"
        '        selectField &= ",DiscountQuantity"
        '        selectField &= ",SubTotal"
        '        selectField &= ",Total"
        '        selectField &= ",[dbo].[fc_StoreCartItem_GetImage](ItemId) as Image,PriceDesc,IsRewardPoints,RewardPoints,SubTotalPoint,AddType"
        '        SQL = "SELECT " & selectField & " FROM StoreCartItem WHERE ([type] = 'item' or [type] = '" & Common.CartItemTypeBuyPoint & "') and OrderId=" & DB.Quote(OrderId) & " order by IsFreeSample, storecartitem.mixmatchid, isFreeItem, isFreeGift, cartitemid"
        '        dr = DB.GetReader(SQL)
        '        While dr.Read
        '            row = New StoreCartItemRow(DB)
        '            row.LoadInShipping(dr)
        '            c.Add(row)
        '        End While
        '        Core.CloseReader(dr)
        '    Catch ex As Exception
        '        Core.CloseReader(dr)
        '        Email.SendError("ToError500", "StoreCartItem.vb > GetCartItemsInShipping(ByVal DB As Database, ByVal OrderId As Integer)", "SQL: " & SQL & "<br><br>Exception:<br>" & ex.ToString())
        '    End Try

        '    If Not c Is Nothing Then
        '        Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
        '        For i As Integer = 0 To c.Count - 1
        '            Initialize(c(i), customerPriceGroupId)
        '        Next
        '        If c.Count > 1 Then
        '            c(c.Count - 1).Final = "1"
        '        End If
        '    End If
        '    Return c
        'End Function


        Public Shared Function GetCartItemInforSelectedChangeFreeQty(ByVal DB As Database, ByVal CartItemId As Integer) As StoreCartItemRow
            Dim dr As SqlDataReader = Nothing

            Dim sql As String = String.Empty
            Try
                sql = "Select Quantity,TotalFreeAllowed,MixMatchId,OrderId,cartitemid,FreeItemIds,Quantity from StoreCartItem where CartItemId=" & CartItemId
                dr = DB.GetReader(sql)
                Dim row As New StoreCartItemRow
                If dr.Read Then
                    row.CartItemId = Convert.ToInt32(dr.Item("CartItemId"))
                    If IsDBNull(dr.Item("MixMatchId")) Then
                        row.MixMatchId = Nothing
                    Else
                        row.MixMatchId = Convert.ToInt32(dr.Item("MixMatchId"))
                    End If
                    If IsDBNull(dr.Item("TotalFreeAllowed")) Then
                        row.TotalFreeAllowed = Nothing
                    Else
                        row.TotalFreeAllowed = Convert.ToInt32(dr.Item("TotalFreeAllowed"))
                    End If
                    row.OrderId = Convert.ToInt32(dr.Item("OrderId"))
                    If IsDBNull(dr.Item("FreeItemIds")) Then
                        row.FreeItemIds = Nothing
                    Else
                        row.FreeItemIds = Convert.ToString(dr.Item("FreeItemIds"))
                    End If

                    If IsDBNull(dr.Item("Quantity")) Then
                        row.Quantity = Nothing
                    Else
                        row.Quantity = Convert.ToInt32(dr.Item("Quantity"))
                    End If
                End If

                Core.CloseReader(dr)
                Return row
            Catch ex As Exception
                Core.CloseReader(dr)
                Throw ex
            End Try
            Return Nothing
        End Function


        Public Shared Function GetCartItemsForCart(ByVal OrderId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow
            Dim SQL As String = "SELECT ItemId,ItemName,Quantity,IsFreeItem,IsFreeGift,SKU,AttributeSKU,IsFlammable,IsOverSize,AddType,MixMatchId,[dbo].[fc_MixMatch_IsAvailablePurchaseItem](MixMatchId,ItemId) as MixMatchValue,'' as MixMatchDescription,IsFreeSample, Price, Swatches, Attributes, DiscountQuantity, LineDiscountAmount, Total, SubTotal, PriceDesc,[dbo].[fc_StoreCartItem_GetImage](ItemId) as Image,CartItemId,OrderId,IsHazMat,PromotionId,CouponMessage,CouponPrice,Type,ItemGroupId,CustomerPrice,QuantityPrice,PromotionPrice,SalePrice,RawPrice, case when mixmatchid > 0 then case when (select top 1 id from mixmatchline tmml where tmml.mixmatchid = storecartitem.mixmatchid and tmml.itemid = storecartitem.itemid and [value] = '100' and discounttype = 'Disc. %') is not null then 0 else 1 end else 0 end as orderfreeitems, case when (select count(ItemId) from storecartitem sci where orderid = " & OrderId & " and type='item' and isfreeitem = 1 and sci.mixmatchid = storecartitem.mixmatchid) > 0 then 1 else 0 end as hasfreeitems,IsRewardPoints,RewardPoints,SubTotalPoint,COALESCE(FreeItemIds,'') as FreeItemIds FROM StoreCartItem  WITH (NOLOCK) WHERE ([type] = 'item' or [type] = '" & Common.CartItemTypeBuyPoint & "') and OrderId=" & Database.Quote(OrderId) & " and (IsFreeItem = 0 or IsFreeGift = 1 or MixMatchId IS NULL OR MixMatchId = 0) AND CartItemId NOT IN (SELECT CartItemId FROM StoreCartItem where Attributesku='FREEITEM') order by CreatedDate ASC, AddType DESC, IsFreeItem ASC"

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                dr = db.ExecuteReader(CommandType.Text, SQL)

                While dr.Read
                    row = New StoreCartItemRow()
                    row.LoadInCart(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "StoreCartItem.vb > GetCartItemsForCart", "OrderId" & OrderId.ToString() & "<br>SQL: " & SQL & "<br>" & ex.ToString())
            End Try

            If Not c Is Nothing AndAlso c.Count > 0 Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
            End If
            Return c
        End Function

        Public Shared Function GetCartReview(ByVal DB As Database, ByVal OrderId As Integer, ByVal MemberId As Integer) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Dim row As StoreCartItemRow
            Try
                Dim sp As String = "sp_StoreCartItem_GetCartReview"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(DB.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    row = New StoreCartItemRow(DB)
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetCartReview(ByVal DB As Database, ByVal OrderId As Integer, ByVal MemberId As Integer)", ex)
            End Try
            If Not c Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For i As Integer = 0 To c.Count - 1
                    Initialize(c(i), customerPriceGroupId)
                Next
                If c.Count > 1 Then
                    c(c.Count - 1).Final = "1"
                End If
            End If
            Return c
        End Function


        Public Shared Function GetFreeItems(ByVal DB As Database, ByVal orderId As Integer, ByVal cartItemId As Integer, ByVal mixmatchId As Integer, Optional loadDefaultItem As Boolean = True) As StoreCartItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreCartItemCollection
            Try
                Dim sp As String = "sp_StoreCartItem_GetFreeItems"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(DB.InParam("CartItemId", SqlDbType.Int, 0, cartItemId))
                cmd.Parameters.Add(DB.InParam("Mixmatchid", SqlDbType.Int, 0, mixmatchId))
                cmd.Parameters.Add(DB.InParam("LoadDefaultItem", SqlDbType.Bit, 0, loadDefaultItem))
                dr = cmd.ExecuteReader()
                While dr.Read
                    Dim row As New StoreCartItemRow
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetFreeItems(ByVal DB As Database, ByVal orderId=" & orderId & " As Integer, ByVal cartItemId=" & cartItemId & " As Integer, ByVal mixmatchId=" & mixmatchId & " As Integer)", ex)
            End Try
            Return c
        End Function
        Public Shared Function CheckSameMixmatch(ByVal _Database As Database, ByVal CartItemId As Integer, ByVal CartItemIdNext As Integer) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_CheckSameMixmatch"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CartItemId", SqlDbType.Int, 0, CartItemId))
                cmd.Parameters.Add(_Database.InParam("CartItemIdNext", SqlDbType.Int, 0, CartItemIdNext))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Dim result As Integer = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
            End Try
            Return False
        End Function

        Public Function GetPromotion() As PromotionRow
            Return PromotionRow.GetRow(DB, ItemId, True)
        End Function
        Public Function CheckExistLog_Sassi(ByVal CartId As Integer) As Boolean
            Dim result As Integer
            result = DB.ExecuteScalar("Select Count(isnull(CartItemId,0)) From StoreCartItemLog Where CartItemId = " & CartId)
            If result > 0 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class StoreCartItemRowBase
        Private m_DB As Database
        Private m_TotalFreeAllowed As Integer = Nothing
        Private m_CartItemId As Integer = Nothing
        Private m_MixMatchId As Integer = Nothing
        Private m_HasFreeItems As Boolean = False
        Private m_OrderId As Integer = Nothing
        Private m_DiscountQuantity As Integer = Nothing
        Private m_RegistryItemId As Integer = Nothing
        Private m_RecipientId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_ItemGroupId As Integer = Nothing
        Private m_ItemName As String = Nothing
        Private m_Image As String = Nothing
        Private m_DepartmentId As Integer = Nothing
        Private m_Quantity As Integer = Nothing
        Private m_SKU As String = Nothing
        Private m_Prefix As String = Nothing
        Private m_AttributeSKU As String = Nothing
        Private m_Attributes As String = Nothing
        Private m_Swatches As String = Nothing
        Private m_Price As Double = Nothing
        Private m_AttributePrice As Double = Nothing
        Private m_RawPrice As Double = Nothing
        Private m_SalePrice As Double = Nothing
        Private m_IsTaxFree As Boolean = Nothing
        Private m_Weight As Double = Nothing
        Private m_IsFreeItem As Boolean = Nothing
        Private m_IsFreeGift As Boolean = Nothing
        Private m_IsPromoItem As Boolean = Nothing
        Private m_IsFreeShipping As Boolean = Nothing
        Private m_PriceDesc As String = Nothing
        Private m_ShipmentDate As DateTime = Nothing
        Private m_Status As String = Nothing
        Private m_LineNo As Integer = Nothing
        Private m_LineDiscountPercent As Double = Nothing
        Private m_LineDiscountAmount As Double = Nothing
        Private m_LineDiscountAmountCust As Double = Nothing
        Private m_SubTotal As Double = Nothing
        Private m_Total As Double = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing
        Private m_Type As String = Nothing
        Private m_CarrierType As Integer = Nothing
        Private m_AdditionalType As String = Nothing
        Private m_FreeItemIds As String = Nothing
        Private m_IsOversize As Boolean = Nothing
        Private m_IsHazMat As Boolean = Nothing
        Private m_AdditionalShipping As Double = Nothing
        Private m_LowPrice As Double = Nothing
        Private m_HighPrice As Double = Nothing
        Private m_LowSalePrice As Double = Nothing
        Private m_HighSalePrice As Double = Nothing
        Private m_PromotionPrice As Double = Nothing
        Private m_CustomerPrice As Double = Nothing
        Private m_QuantityPrice As Double = Nothing
        Private m_IsRushDelivery As Boolean = False
        Private m_RushDeliveryCharge As Double = Nothing
        Private m_IsLiftGate As Boolean = Nothing
        Private m_LiftGateCharge As Double = Nothing
        Private m_IsScheduleDelivery As Boolean = False
        Private m_ScheduleDeliveryCharge As Double = Nothing
        Private m_IsInsideDelivery As Boolean = False
        Private m_InsideDeliveryCharge As Double = Nothing
        Private m_PromotionID As Integer = Nothing
        Private m_CouponPrice As Double = Nothing
        Private m_CouponMessage As String = Nothing
        Private m_Final As String = Nothing
        Private m_IsFreeSample As Boolean = Nothing
        Private m_IsFlammable As Boolean = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_ModifiedDate As DateTime = Nothing
        'Private m_IsNotFree As Boolean = Nothing
        Private m_IsReviewed As Boolean = Nothing
        Private m_IsRewardPoints As Boolean = False
        Private m_RewardPoints As Integer = Nothing
        Private m_SubTotalPoint As Integer = Nothing
        Private m_AddType As Integer = Nothing
        Private m_FreeShipping As Double = Nothing
        Public Shared cachePrefixKey As String = "StoreCartItem_"
        Private m_MixMatchDescription As String = Nothing
        Private m_IsFirstReview As Boolean = False
        Public Property IsInsideDelivery() As Boolean
            Get
                Return m_IsInsideDelivery
            End Get
            Set(ByVal value As Boolean)
                m_IsInsideDelivery = value
            End Set
        End Property

        Public Property InsideDeliveryCharge() As Double
            Get
                Return m_InsideDeliveryCharge
            End Get
            Set(ByVal value As Double)
                m_InsideDeliveryCharge = value
            End Set
        End Property

        Public Property IsScheduleDelivery() As Boolean
            Get
                Return m_IsScheduleDelivery
            End Get
            Set(ByVal value As Boolean)
                m_IsScheduleDelivery = value
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

        Public Property IsLiftGate() As Boolean
            Get
                Return m_IsLiftGate
            End Get
            Set(ByVal value As Boolean)
                m_IsLiftGate = value
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

        Public Property IsRushDelivery() As Boolean
            Get
                Return m_IsRushDelivery
            End Get
            Set(ByVal value As Boolean)
                m_IsRushDelivery = value
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

        Public Property ItemGroupId() As Integer
            Get
                Return m_ItemGroupId
            End Get
            Set(ByVal value As Integer)
                m_ItemGroupId = value
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

        Public Property FreeItemIds() As String
            Get
                Return m_FreeItemIds
            End Get
            Set(ByVal value As String)
                m_FreeItemIds = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal value As String)
                m_Type = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal value As String)
                m_Status = value
            End Set
        End Property

        Public Property CarrierType() As Integer
            Get
                Return m_CarrierType
            End Get
            Set(ByVal value As Integer)
                m_CarrierType = value
            End Set
        End Property

        Public Property ShipmentDate() As DateTime
            Get
                Return m_ShipmentDate
            End Get
            Set(ByVal value As DateTime)
                m_ShipmentDate = value
            End Set
        End Property

        Public Property CartItemId() As Integer
            Get
                Return m_CartItemId
            End Get
            Set(ByVal Value As Integer)
                m_CartItemId = Value
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

        Public Property DiscountQuantity() As Integer
            Get
                Return m_DiscountQuantity
            End Get
            Set(ByVal Value As Integer)
                m_DiscountQuantity = Value
            End Set
        End Property

        Public Property RegistryItemId() As Integer
            Get
                Return m_RegistryItemId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryItemId = Value
            End Set
        End Property

        Public Property RecipientId() As Integer
            Get
                Return m_RecipientId
            End Get
            Set(ByVal Value As Integer)
                m_RecipientId = Value
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

        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal Value As String)
                m_ItemName = Value
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

        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentId = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return m_Quantity
            End Get
            Set(ByVal Value As Integer)
                m_Quantity = Value
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

        Public Property Prefix() As String
            Get
                Return m_Prefix
            End Get
            Set(ByVal Value As String)
                m_Prefix = Value
            End Set
        End Property

        Public Property AttributeSKU() As String
            Get
                Return m_AttributeSKU
            End Get
            Set(ByVal Value As String)
                m_AttributeSKU = Value
            End Set
        End Property

        Public Property Attributes() As String
            Get
                Return m_Attributes
            End Get
            Set(ByVal Value As String)
                m_Attributes = Value
            End Set
        End Property

        Public Property Swatches() As String
            Get
                Return m_Swatches
            End Get
            Set(ByVal Value As String)
                m_Swatches = Value
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

        Public Property AttributePrice() As Double
            Get
                Return m_AttributePrice
            End Get
            Set(ByVal Value As Double)
                m_AttributePrice = Value
            End Set
        End Property

        Public Property RawPrice() As Double
            Get
                Return m_RawPrice
            End Get
            Set(ByVal Value As Double)
                m_RawPrice = Value
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

        Public Property HasFreeItems() As Boolean
            Get
                Return m_HasFreeItems
            End Get
            Set(ByVal Value As Boolean)
                m_HasFreeItems = Value
            End Set
        End Property

        Public Property TotalFreeAllowed() As Integer
            Get
                Return m_TotalFreeAllowed
            End Get
            Set(ByVal value As Integer)
                m_TotalFreeAllowed = value
            End Set
        End Property

        Public Property Total() As Double
            Get
                Return m_Total
            End Get
            Set(ByVal Value As Double)
                m_Total = Value
            End Set
        End Property

        Public Property SubTotal() As Double
            Get
                Return m_SubTotal
            End Get
            Set(ByVal Value As Double)
                m_SubTotal = Value
            End Set
        End Property

        Public Property AdditionalShipping() As Double
            Get
                Return m_AdditionalShipping
            End Get
            Set(ByVal Value As Double)
                m_AdditionalShipping = Value
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

        Public Property IsFreeItem() As Boolean
            Get
                Return m_IsFreeItem
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeItem = Value
            End Set
        End Property

        Public Property IsFreeGift() As Boolean
            Get
                Return m_IsFreeGift
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeGift = Value
            End Set
        End Property


        Public Property IsPromoItem() As Boolean
            Get
                Return m_IsPromoItem
            End Get
            Set(ByVal Value As Boolean)
                m_IsPromoItem = Value
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

        Public Property IsHazMat() As Boolean
            Get
                Return m_IsHazMat
            End Get
            Set(ByVal Value As Boolean)
                m_IsHazMat = Value
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

        Public Property IsFreeShipping() As Boolean
            Get
                Return m_IsFreeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeShipping = Value
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

        Public Property AdditionalType() As String
            Get
                Return m_AdditionalType
            End Get
            Set(ByVal Value As String)
                m_AdditionalType = Value
            End Set
        End Property

        Public Property LineNo() As Integer
            Get
                Return m_LineNo
            End Get
            Set(ByVal Value As Integer)
                m_LineNo = Value
            End Set
        End Property

        Public Property LineDiscountPercent() As Double
            Get
                Return m_LineDiscountPercent
            End Get
            Set(ByVal Value As Double)
                m_LineDiscountPercent = Value
            End Set
        End Property

        Public Property LineDiscountAmount() As Double
            Get
                Return m_LineDiscountAmount
            End Get
            Set(ByVal Value As Double)
                m_LineDiscountAmount = Value
            End Set
        End Property

        Public Property LineDiscountAmountCust() As Double
            Get
                Return m_LineDiscountAmountCust
            End Get
            Set(ByVal Value As Double)
                m_LineDiscountAmountCust = Value
            End Set
        End Property

        Public Property CustomerPrice() As Double
            Get
                Return m_CustomerPrice
            End Get
            Set(ByVal Value As Double)
                m_CustomerPrice = Value
            End Set
        End Property

        Public Property PromotionPrice() As Double
            Get
                Return m_PromotionPrice
            End Get
            Set(ByVal Value As Double)
                m_PromotionPrice = Value
            End Set
        End Property

        Public Property QuantityPrice() As Double
            Get
                Return m_QuantityPrice
            End Get
            Set(ByVal Value As Double)
                m_QuantityPrice = Value
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

        Public Property LastExport() As DateTime
            Get
                Return m_LastExport
            End Get
            Set(ByVal Value As DateTime)
                m_LastExport = Value
            End Set
        End Property

        Public Property PromotionID() As Integer
            Get
                Return m_PromotionID
            End Get
            Set(ByVal Value As Integer)
                m_PromotionID = Value
            End Set
        End Property

        Public Property CouponPrice() As Double
            Get
                Return m_CouponPrice
            End Get
            Set(ByVal Value As Double)
                m_CouponPrice = Value
            End Set
        End Property

        Public Property CouponMessage() As String
            Get
                Return m_CouponMessage
            End Get
            Set(ByVal value As String)
                m_CouponMessage = value
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
        Public Property IsFreeSample() As Boolean
            Get
                Return m_IsFreeSample
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeSample = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal Value As DateTime)
                m_ModifiedDate = Value
            End Set
        End Property
        Public Property IsReviewed() As Boolean
            Get
                Return m_IsReviewed
            End Get
            Set(ByVal Value As Boolean)
                m_IsReviewed = Value
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
        Public Property SubTotalPoint() As Integer
            Get
                Return m_SubTotalPoint
            End Get
            Set(ByVal Value As Integer)
                m_SubTotalPoint = Value
            End Set
        End Property
        Public Property AddType() As Integer
            Get
                Return m_AddType
            End Get
            Set(ByVal Value As Integer)
                m_AddType = Value
            End Set
        End Property

        Public Property FreeShipping() As Double
            Get
                Return m_FreeShipping
            End Get
            Set(ByVal value As Double)
                m_FreeShipping = value
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
        Public Property isFirstReview() As Boolean
            Get
                Return m_IsFirstReview
            End Get
            Set(ByVal value As Boolean)
                m_IsFirstReview = value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CartItemId As Integer)
            m_DB = DB
            m_CartItemId = CartItemId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderNo As String, ByVal SKU As String, ByVal IsItem As Boolean, Optional ByVal ShippingAgentCode As String = "", Optional ByVal IsNavisionOrderNo As Boolean = False)
            m_DB = DB

            If IsItem Then
                m_ItemId = DB.ExecuteScalar("select top 1 ItemId from StoreItem where SKU = " & DB.Quote(SKU))
                If ItemId = Nothing Then Throw New Exception("Item not found for StoreCartItem (OrderLines)." & vbCrLf & "SKU: " & SKU)
            Else
                If CarrierType = Nothing Then CarrierType = GetCarrierType(DB, ShippingAgentCode)
                If CarrierType = Nothing AndAlso ShippingAgentCode <> "" Then Throw New Exception("Carrier not found for StoreCartItem (OrderLines)." & vbCrLf & "Shipping Agent Code: " & ShippingAgentCode)
            End If

            If Not IsNavisionOrderNo Then
                m_OrderId = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where OrderNo = " & DB.Quote(OrderNo))
            Else
                m_OrderId = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where NavisionOrderNo = " & DB.Quote(OrderNo))
            End If
            If m_OrderId = Nothing Then Throw New Exception("Order not found for StoreCartItem (OrderLines)." & vbCrLf & IIf(IsNavisionOrderNo, "Navision", "") & "OrderNo: " & OrderNo)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String)
            m_DB = DB
            m_OrderId = OrderId
            m_ItemId = ItemId
            m_Attributes = Attributes
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFreeItem As Boolean, ByVal IsPromo As Boolean)
            m_DB = DB
            m_OrderId = OrderId
            m_ItemId = ItemId
            m_Attributes = Attributes
            m_Swatches = Swatches
            m_IsFreeItem = IsFreeItem
            m_IsPromoItem = IsPromo
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String, ByVal IsFreeItem As Boolean, ByVal IsPromo As Boolean, ByVal IsFreeGift As Boolean)
            m_DB = DB
            m_OrderId = OrderId
            m_ItemId = ItemId
            m_Attributes = Attributes
            m_Swatches = Swatches
            m_IsFreeItem = IsFreeItem
            m_IsPromoItem = IsPromo
            m_IsFreeGift = IsFreeGift
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal CarrierId As Integer)
            m_DB = DB
            m_OrderId = OrderId
            m_CarrierType = CarrierId
            m_Type = "carrier"
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer, ByVal ItemId As Integer, ByVal IsCoupon As Boolean)
            m_DB = DB
            m_OrderId = OrderId
            m_ItemId = ItemId
            m_Type = "carrier"
        End Sub 'New
        'Long
        Public Function ReturnSku(ByVal id As Integer) As String
            Dim sku As String = ""
            Dim strSQL As String
            Try
                strSQL = "select sku from storeitem where itemid = " & id
                sku = DB.ExecuteScalar(strSQL)
                If sku Is Nothing = True Then
                    sku = ""
                Else
                    sku = sku
                End If
                Return sku
            Catch ex As Exception

            Finally

            End Try
            Return sku
        End Function
        Public Function ReturnAttSku(ByVal id As Integer, ByVal ATT As String) As String
            Dim sku As String = ""
            Dim strSQL As String
            Try
                If ATT = "FREEITEM" Then
                    strSQL = "select  top 1 AttributeSku  from storecartitem where itemid = " & id & " and AttributeSku = 'FREEITEM'"
                Else
                    strSQL = "select  top 1 AttributeSku  from storecartitem where itemid = " & id & " and AttributeSku <> 'FREEITEM'"
                End If
                sku = DB.ExecuteScalar(strSQL)
                If sku Is Nothing = True Then
                    sku = ""
                Else
                    sku = sku
                End If

                Return sku
            Catch ex As Exception

            Finally
            End Try
            Return sku
        End Function
        'End

        Public Shared Sub UpdateDefaultFreeShipping(ByVal DB As Database, ByVal OrderId As Integer)
            Dim SQL As String = ""
            SQL = "UPDATE StoreCartItem SET IsFreeShipping = (SELECT StoreItem.IsFreeShipping FROM StoreItem WHERE StoreItem.ItemId = StoreCartItem.ItemId ) WHERE [Type] <> 'carrier' And StoreCartItem.PromotionId = 0 AND OrderId = " & DB.Number(OrderId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Function DeleteFreeItemGift(ByVal OrderId As Integer) As Integer
            Dim result As Integer = 0

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreCartItem_DeleteFreeItemGift")
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                result = CInt(db.ExecuteScalar(cmd))

            Catch ex As Exception
                Email.SendError("ToError500", "DeleteFreeItemGift", "OrderId: " & OrderId & "<br>Exception:" & ex.ToString())
            End Try

            Return result
        End Function
        Public Shared Function GetRowBuyPointByItemId(ByVal DB As Database, ByVal ItemId As Integer, ByVal OrderId As Integer) As StoreCartItemRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim c As New StoreCartItemRow
                Dim sp As String = "sp_StoreCartItem_GetCartItemBuyPointByItemId"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("ItemId", SqlDbType.Int, 0, ItemId))
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                r = cmd.ExecuteReader()
                If r.Read Then
                    c = LoadCartItemBuyPointByDataReader(r)
                End If
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New StoreCartItemRow
        End Function

        Public Shared Function GetRowBuyPointByOrderId(ByVal DB As Database, ByVal OrderId As Integer) As StoreCartItemRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim c As New StoreCartItemRow
                Dim sp As String = "sp_StoreCartItem_GetCartItemBuyPointByOrderId"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                r = cmd.ExecuteReader()
                If r.Read Then
                    c = LoadCartItemBuyPointByDataReader(r)
                End If
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New StoreCartItemRow
        End Function
        Public Shared Function RemoveItemBuyPoint(ByVal DB As Database, ByVal OrderId As Integer) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_RemoveItemBuyPoint"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Dim result As Integer = CInt(cmd.Parameters("result").Value)
                If (result > 0) Then
                    Utility.Common.DeleteCachePopupCart(OrderId)
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Private Shared Function LoadCartItemBuyPointByDataReader(ByVal r As IDataReader) As StoreCartItemRow
            Dim item As New StoreCartItemRow
            Try
                item.CartItemId = r.Item("CartItemId")
                item.ItemId = r.Item("ItemId")
                item.OrderId = r.Item("OrderId")
                If r.Item("Type") Is Convert.DBNull Then
                    item.Type = Nothing
                Else
                    item.Type = Convert.ToString(r.Item("Type"))
                End If
                If r.Item("ItemName") Is Convert.DBNull Then
                    item.ItemName = Nothing
                Else
                    item.ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                If r.Item("Price") Is Convert.DBNull Then
                    item.Price = Nothing
                Else
                    item.Price = Convert.ToDouble(r.Item("Price"))
                End If
                If r.Item("SKU") Is Convert.DBNull Then
                    item.SKU = Nothing
                Else
                    item.SKU = Convert.ToString(r.Item("SKU"))
                End If
                item.Quantity = r.Item("Quantity")
                If r.Item("SubTotal") Is Convert.DBNull Then
                    item.SubTotal = Nothing
                Else
                    item.SubTotal = Convert.ToDouble(r.Item("SubTotal"))
                End If
                If r.Item("Total") Is Convert.DBNull Then
                    item.Total = Nothing
                Else
                    item.Total = Convert.ToDouble(r.Item("Total"))
                End If
                Return item
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function ProcessMixmatchPromotions(ByVal DB As Database, ByVal MixMatchId As String, ByVal PMixMatchId As String, ByVal AMixmatchId As String, ByVal AllFreeIds As String, ByVal OrderId As Integer) As Boolean
            Try
                Dim sp As String = "sp_StoreCartItem_ProcessMixmatchPromotions"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(DB.InParam("MixMatchId", SqlDbType.VarChar, 0, MixMatchId))
                cmd.Parameters.Add(DB.InParam("PMixMatchId", SqlDbType.VarChar, 0, PMixMatchId))
                cmd.Parameters.Add(DB.InParam("AMixmatchId", SqlDbType.VarChar, 0, AMixmatchId))
                cmd.Parameters.Add(DB.InParam("AllFreeIds", SqlDbType.VarChar, 0, AllFreeIds))
                cmd.ExecuteNonQuery()
                Return True
            Catch ex As Exception
                Email.SendError("ToError500", "ProcessMixmatchPromotions", "MixMatchId: " & MixMatchId & "<br>PMixMatchId: " & PMixMatchId & "<br>AMixmatchId: " & AMixmatchId & "<br>AllFreeIds: " & AllFreeIds & "<br>OrderId: " & OrderId & "<br>Exception: " & ex.ToString())
                Return False
            End Try

        End Function

        Public Shared Function ItemCount(ByVal MemberId As String, ByVal OrderId As Integer) As Integer
            Dim i As Integer
            Dim keyData As String = cachePrefixKey & "ItemCount_" & OrderId
            i = CType(CacheUtils.GetCache(keyData), Integer)
            If i > 0 Then
                Return i
            Else
                i = 0
            End If


            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreCartItem_ItemCount"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                i = db.ExecuteScalar(cmd)
            Catch ex As Exception
                Core.LogError("StoreCartItem.vb", "ItemCount(ByVal MemberId As String, ByVal OrderId As Integer)", ex)
            Finally
                CacheUtils.SetCache(keyData, i)
            End Try

            Return i
        End Function
        Public Shared Function CountCartRow(ByVal DB As Database, ByVal orderId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_StoreCartItem_GetRowCount"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, orderId))
                result = cmd.ExecuteScalar()
            Catch ex As Exception
                Email.SendError("ToError500", "CountCartRow", "orderId: " & orderId & ex.ToString())
            End Try
            Return result
        End Function

        Protected Overridable Sub Load(Optional ByVal FromNavision As Boolean = False)
            Dim st1 As String = AttributeSKU
            Dim st2 As String = Attributes
            Dim r1 As SqlDataReader = Nothing
            Dim SQL As String

            SQL = "SELECT * FROM StoreCartItem WHERE "
            If CartItemId <> Nothing Then
                SQL &= "CartItemId = " & CartItemId
            Else
                SQL &= "OrderId = " & OrderId & " "
                If ItemId <> Nothing Then
                    If AddType >= 0 Then
                        SQL &= " AND AddType = " & AddType.ToString() & " "
                    End If

                    SQL &= " AND ItemId = " & ItemId

                    Dim strAttsku As String = ReturnAttSku(ItemId, Attributes)
                    Dim sa As StoreAttributeCollection = StoreAttributeRow.GetRowsByItemSku(DB, ReturnSku(ItemId))
                    If sa.Count > 0 Then
                        If Attributes <> Nothing Then
                            SQL &= IIf(FromNavision, "", " and IsFreeItem = " & Convert.ToInt32(IsFreeItem)) & " and (Attributesku = 'FREE' or Attributesku like 'Discount%')"
                        Else
                            SQL &= IIf(FromNavision, "", " and IsFreeItem = " & Convert.ToInt32(IsFreeItem)) & " and Attributesku IS NULL"
                        End If
                    ElseIf Attributes = "IsFreeSample" Then
                        SQL &= " AND IsFreeSample = 1"
                    ElseIf Attributes = "IsItemPoint" Then
                        SQL &= " AND IsRewardPoints = 1 and RewardPoints > 0"
                    Else
                        SQL &= " AND IsRewardPoints = 0 and IsFreeSample = 0 " & IIf(FromNavision, "", " and IsFreeItem = " & Convert.ToInt32(IsFreeItem)) '& " and Attributesku = " & DB.Quote(AttributeSKU) '& " and Swatches " & IIf(Trim(Swatches) = Nothing, " is ", " = ") & DB.Quote(Swatches)
                    End If

                    SQL &= " AND IsFreeGift = " & CInt(IsFreeGift)
                Else
                    SQL &= " AND Type = 'carrier' and CarrierType = " & CarrierType
                End If
            End If

            Try
                r1 = m_DB.GetReader(SQL)
                If r1.Read Then
                    Me.Load(r1)
                End If
                Core.CloseReader(r1)
            Catch ex As Exception
                Core.CloseReader(r1)
                Email.SendError("ToError500", "StoreCartItem.vb > Load(Optional ByVal FromNavision As Boolean = False)", "SQL: " & SQL & "<br><br>" & ex.ToString())
            End Try
        End Sub
        Protected Overridable Sub LoadFreeItemByMixmatch(ByVal orderId As Integer, ByVal itemId As Integer, ByVal mmId As Integer)
            Dim r1 As SqlDataReader = Nothing
            Dim SQL As String
            SQL = "Select * from StoreCartItem where OrderId=" & orderId & " and ItemId=" & itemId & " and MixMatchId=" & mmId & " and IsFreeItem=1"
            Try
                r1 = m_DB.GetReader(SQL)
                If r1.Read Then
                    Me.Load(r1)
                End If
                Core.CloseReader(r1)
            Catch ex As Exception
                Core.CloseReader(r1)
                Core.LogError("StoreCartItem.vb", "LoadFreeItemByMixmatch(orderId=" & orderId & ",itemId=" & itemId & ",mmId=" & mmId & ")", ex)
            End Try
        End Sub
        'Protected Overridable Sub LoadInShipping(ByVal r As SqlDataReader)
        '    Try
        '        m_ItemId = Convert.ToInt32(r.Item("ItemId"))
        '        If IsDBNull(r.Item("ItemName")) Then
        '            m_ItemName = Nothing
        '        Else
        '            m_ItemName = Convert.ToString(r.Item("ItemName"))
        '        End If
        '        m_CartItemId = Convert.ToInt32(r.Item("CartItemId"))
        '        If IsDBNull(r.Item("Type")) Then
        '            m_Type = Nothing
        '        Else
        '            m_Type = Convert.ToString(r.Item("Type"))
        '        End If
        '        If IsDBNull(r.Item("SKU")) Then
        '            m_SKU = Nothing
        '        Else
        '            m_SKU = Convert.ToString(r.Item("SKU"))
        '        End If
        '        m_Quantity = Convert.ToInt32(r.Item("Quantity"))
        '        m_Price = Convert.ToDouble(r.Item("Price"))

        '        If IsDBNull(r.Item("Total")) Then
        '            m_Total = Nothing
        '        Else
        '            m_Total = Convert.ToDouble(r.Item("Total"))
        '        End If
        '        If IsDBNull(r.Item("SubTotal")) Then
        '            m_SubTotal = Nothing
        '        Else
        '            m_SubTotal = Convert.ToDouble(r.Item("SubTotal"))
        '        End If
        '        If IsDBNull(r.Item("PriceDesc")) Then
        '            m_PriceDesc = Nothing
        '        Else
        '            m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
        '        End If
        '        If IsDBNull(r.Item("CarrierType")) Then
        '            m_CarrierType = Nothing
        '        Else
        '            m_CarrierType = Convert.ToString(r.Item("CarrierType"))
        '        End If
        '        If (m_Type = Utility.Common.CartItemTypeBuyPoint) Then
        '            Exit Sub
        '        End If
        '        m_IsFreeItem = Convert.ToBoolean(r.Item("IsFreeItem"))
        '        If IsDBNull(r.Item("IsFreeGift")) Then
        '            m_IsFreeGift = False
        '        Else
        '            m_IsFreeGift = Convert.ToBoolean(r.Item("IsFreeGift"))
        '        End If
        '        If IsDBNull(r.Item("MixMatchId")) Then
        '            m_MixMatchId = Nothing
        '        Else
        '            m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
        '        End If
        '        m_IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))
        '        m_IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
        '        m_IsFlammable = Convert.ToBoolean(r.Item("IsFlammable"))
        '        If IsDBNull(r.Item("PromotionId")) Then
        '            m_PromotionID = Nothing
        '        Else
        '            m_PromotionID = Convert.ToInt32(r.Item("PromotionId"))
        '        End If
        '        If IsDBNull(r.Item("CouponMessage")) Then
        '            m_CouponMessage = Nothing
        '        Else
        '            m_CouponMessage = Convert.ToString(r.Item("CouponMessage"))
        '        End If

        '        If IsDBNull(r.Item("FreeItemIds")) Then
        '            m_FreeItemIds = Nothing
        '        Else
        '            m_FreeItemIds = Convert.ToString(r.Item("FreeItemIds"))
        '        End If

        '        m_IsOversize = Convert.ToBoolean(r.Item("IsOversize"))
        '        m_IsRushDelivery = Convert.ToBoolean(r.Item("IsRushDelivery"))
        '        m_IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
        '        m_IsLiftGate = Convert.ToBoolean(r.Item("IsLiftGate"))
        '        m_IsScheduleDelivery = Convert.ToBoolean(r.Item("IsScheduleDelivery"))
        '        m_IsInsideDelivery = Convert.ToBoolean(r.Item("IsInsideDelivery"))
        '        If IsDBNull(r.Item("AttributeSKU")) Then
        '            m_AttributeSKU = Nothing
        '        Else
        '            m_AttributeSKU = Convert.ToString(r.Item("AttributeSKU"))
        '        End If
        '        If IsDBNull(r.Item("Attributes")) Then
        '            m_Attributes = Nothing
        '        Else
        '            m_Attributes = Convert.ToString(r.Item("Attributes"))
        '        End If

        '        If r.Item("CustomerPrice") Is Convert.DBNull Then
        '            m_CustomerPrice = Nothing
        '        Else
        '            m_CustomerPrice = Convert.ToDouble(r.Item("CustomerPrice"))
        '        End If
        '        If r.Item("PromotionPrice") Is Convert.DBNull Then
        '            m_PromotionPrice = Nothing
        '        Else
        '            m_PromotionPrice = Convert.ToDouble(r.Item("PromotionPrice"))
        '        End If
        '        If IsDBNull(r.Item("DiscountQuantity")) Then
        '            m_DiscountQuantity = Nothing
        '        Else
        '            m_DiscountQuantity = Convert.ToInt32(r.Item("DiscountQuantity"))
        '        End If

        '        If IsDBNull(r.Item("Image")) Then
        '            m_Image = Nothing
        '        Else
        '            m_Image = Convert.ToString(r.Item("Image"))
        '        End If

        '        If IsDBNull(r.Item("RewardPoints")) Then
        '            m_RewardPoints = Nothing
        '        Else
        '            m_RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
        '        End If
        '        If IsDBNull(r.Item("IsRewardPoints")) Then
        '            m_IsRewardPoints = False
        '        Else
        '            m_IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
        '        End If
        '        If IsDBNull(r.Item("SubTotalPoint")) Then
        '            m_SubTotalPoint = Nothing
        '        Else
        '            m_SubTotalPoint = Convert.ToInt32(r.Item("SubTotalPoint"))
        '        End If
        '        Try
        '            If r.Item("AddType") Is Convert.DBNull Then
        '                m_AddType = 1
        '            Else
        '                m_AddType = CInt(r.Item("AddType"))
        '            End If
        '        Catch ex As Exception
        '            m_AddType = 1
        '        End Try
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        'End Sub 'Load for shipping page
        Protected Overridable Sub LoadInCart(ByVal r As SqlDataReader)
            Try
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                If IsDBNull(r.Item("ItemName")) Then
                    m_ItemName = Nothing
                Else
                    m_ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                m_Quantity = Convert.ToInt32(r.Item("Quantity"))
                If IsDBNull(r.Item("SKU")) Then
                    m_SKU = Nothing
                Else
                    m_SKU = Convert.ToString(r.Item("SKU"))
                End If
                If IsDBNull(r.Item("FreeItemIds")) Then
                    m_FreeItemIds = Nothing
                Else
                    m_FreeItemIds = Convert.ToString(r.Item("FreeItemIds"))
                End If
                If IsDBNull(r.Item("Total")) Then
                    m_Total = Nothing
                Else
                    m_Total = Convert.ToDouble(r.Item("Total"))
                End If
                If IsDBNull(r.Item("SubTotal")) Then
                    m_SubTotal = Nothing
                Else
                    m_SubTotal = Convert.ToDouble(r.Item("SubTotal"))
                End If
                m_CartItemId = Convert.ToInt32(r.Item("CartItemId"))
                m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                If IsDBNull(r.Item("Type")) Then
                    If m_SKU <> Nothing Then
                        m_Type = "item"
                    Else
                        m_Type = Nothing
                    End If
                Else
                    m_Type = Convert.ToString(r.Item("Type"))
                End If
                m_Price = Convert.ToDouble(r.Item("Price"))
                If IsDBNull(r.Item("PriceDesc")) Then
                    m_PriceDesc = Nothing
                Else
                    m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                End If
                If (m_Type = Common.CartItemTypeBuyPoint) Then
                    Exit Sub
                End If
                m_IsFreeItem = Convert.ToBoolean(r.Item("IsFreeItem"))
                If IsDBNull(r.Item("IsFreeGift")) Then
                    m_IsFreeGift = False
                Else
                    m_IsFreeGift = Convert.ToBoolean(r.Item("IsFreeGift"))
                End If
                If IsDBNull(r.Item("IsOverSize")) Then
                    m_IsOversize = False
                Else
                    m_IsOversize = Convert.ToBoolean(r.Item("IsOverSize"))
                End If
                If IsDBNull(r.Item("AttributeSKU")) Then
                    m_AttributeSKU = Nothing
                Else
                    m_AttributeSKU = Convert.ToString(r.Item("AttributeSKU"))
                End If
                Try
                    If r.Item("IsFlammable") Is Convert.DBNull Then
                        m_IsFlammable = False
                    Else
                        m_IsFlammable = CBool(r.Item("IsFlammable"))
                    End If
                Catch ex As Exception
                    m_IsFlammable = False
                End Try
                If IsDBNull(r.Item("MixMatchId")) Then
                    m_MixMatchId = Nothing
                Else
                    m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
                End If
                m_HasFreeItems = Convert.ToBoolean(r.Item("hasfreeitems"))
                'm_IsNotFree = Convert.ToBoolean(r.Item("IsNotFree"))
                m_IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))
                If IsDBNull(r.Item("Swatches")) Then
                    m_Swatches = Nothing
                Else
                    m_Swatches = Convert.ToString(r.Item("Swatches"))
                End If
                If IsDBNull(r.Item("Attributes")) Then
                    m_Attributes = Nothing
                Else
                    m_Attributes = Convert.ToString(r.Item("Attributes"))
                End If
                If IsDBNull(r.Item("DiscountQuantity")) Then
                    m_DiscountQuantity = Nothing
                Else
                    m_DiscountQuantity = Convert.ToInt32(r.Item("DiscountQuantity"))
                End If
                If r.Item("LineDiscountAmount") Is Convert.DBNull Then
                    m_LineDiscountAmount = Nothing
                Else
                    m_LineDiscountAmount = Convert.ToDouble(r.Item("LineDiscountAmount"))
                End If


                If IsDBNull(r.Item("Image")) Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If

                m_IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                If IsDBNull(r.Item("PromotionId")) Then
                    m_PromotionID = Nothing
                Else
                    m_PromotionID = Convert.ToInt32(r.Item("PromotionId"))
                End If
                If IsDBNull(r.Item("CouponMessage")) Then
                    m_CouponMessage = Nothing
                Else
                    m_CouponMessage = Convert.ToString(r.Item("CouponMessage"))
                End If
                If r.Item("CouponPrice") Is Convert.DBNull Then
                    m_CouponPrice = Nothing
                Else
                    m_CouponPrice = Convert.ToDouble(r.Item("CouponPrice"))
                End If

                If IsDBNull(r.Item("ItemGroupId")) Then
                    m_ItemGroupId = Nothing
                Else
                    m_ItemGroupId = Convert.ToInt32(r.Item("ItemGroupId"))
                End If
                If r.Item("CustomerPrice") Is Convert.DBNull Then
                    m_CustomerPrice = Nothing
                Else
                    m_CustomerPrice = Convert.ToDouble(r.Item("CustomerPrice"))
                End If
                If r.Item("QuantityPrice") Is Convert.DBNull Then
                    m_QuantityPrice = Nothing
                Else
                    m_QuantityPrice = Convert.ToDouble(r.Item("QuantityPrice"))
                End If
                If r.Item("PromotionPrice") Is Convert.DBNull Then
                    m_PromotionPrice = Nothing
                Else
                    m_PromotionPrice = Convert.ToDouble(r.Item("PromotionPrice"))
                End If

                If IsDBNull(r.Item("SalePrice")) Then
                    m_SalePrice = Nothing
                Else
                    m_SalePrice = Convert.ToDouble(r.Item("SalePrice"))
                End If
                If IsDBNull(r.Item("RawPrice")) Then
                    m_RawPrice = Nothing
                Else
                    m_RawPrice = Convert.ToDouble(r.Item("RawPrice"))
                End If
                If IsDBNull(r.Item("RewardPoints")) Then
                    m_RewardPoints = Nothing
                Else
                    m_RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
                End If
                If IsDBNull(r.Item("IsRewardPoints")) Then
                    m_IsRewardPoints = False
                Else
                    m_IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
                End If
                If IsDBNull(r.Item("SubTotalPoint")) Then
                    m_SubTotalPoint = Nothing
                Else
                    m_SubTotalPoint = Convert.ToInt32(r.Item("SubTotalPoint"))
                End If
                If IsDBNull(r.Item("MixMatchDescription")) Then
                    m_MixMatchDescription = Nothing
                Else
                    m_MixMatchDescription = Convert.ToString(r.Item("MixMatchDescription"))
                End If
                Try
                    If r.Item("AddType") Is Convert.DBNull Then
                        m_AddType = 1
                    Else
                        m_AddType = CInt(r.Item("AddType"))
                    End If
                Catch ex As Exception
                    m_AddType = 1
                End Try
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load for cart page
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_CartItemId = Convert.ToInt32(r.Item("CartItemId"))
                m_Weight = Convert.ToDouble(r.Item("Weight"))
                m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                m_RecipientId = Convert.ToInt32(r.Item("RecipientId"))
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                If IsDBNull(r.Item("ItemGroupId")) Then
                    m_ItemGroupId = Nothing
                Else
                    m_ItemGroupId = Convert.ToInt32(r.Item("ItemGroupId"))
                End If
                If IsDBNull(r.Item("ItemName")) Then
                    m_ItemName = Nothing
                Else
                    m_ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                If IsDBNull(r.Item("FreeItemIds")) Then
                    m_FreeItemIds = Nothing
                Else
                    m_FreeItemIds = Convert.ToString(r.Item("FreeItemIds"))
                End If
                If IsDBNull(r.Item("DiscountQuantity")) Then
                    m_DiscountQuantity = Nothing
                Else
                    m_DiscountQuantity = Convert.ToInt32(r.Item("DiscountQuantity"))
                End If
                Try
                    If IsDBNull(r.Item("MixMatchId")) Then
                        m_MixMatchId = 0
                    Else
                        m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
                    End If
                Catch ex As Exception
                    m_MixMatchId = 0
                End Try

                If IsDBNull(r.Item("TotalFreeAllowed")) Then
                    m_TotalFreeAllowed = Nothing
                Else
                    m_TotalFreeAllowed = Convert.ToInt32(r.Item("TotalFreeAllowed"))
                End If
                If IsDBNull(r.Item("CarrierType")) Then
                    m_CarrierType = Nothing
                Else
                    m_CarrierType = Convert.ToString(r.Item("CarrierType"))
                End If
                If IsDBNull(r.Item("PriceDesc")) Then
                    m_PriceDesc = Nothing
                Else
                    m_PriceDesc = Convert.ToString(r.Item("PriceDesc"))
                End If
                m_AttributePrice = Convert.ToDouble(r.Item("AttributePrice"))
                If IsDBNull(r.Item("Image")) Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If
                If IsDBNull(r.Item("DepartmentId")) Then
                    m_DepartmentId = Nothing
                Else
                    m_DepartmentId = Convert.ToInt32(r.Item("DepartmentId"))
                End If
                If IsDBNull(r.Item("RegistryItemId")) Then
                    m_RegistryItemId = Nothing
                Else
                    m_RegistryItemId = Convert.ToInt32(r.Item("RegistryItemId"))
                End If
                m_Quantity = Convert.ToInt32(r.Item("Quantity"))
                m_Price = Convert.ToDouble(r.Item("Price"))
                If IsDBNull(r.Item("Total")) Then
                    m_Total = Nothing
                Else
                    m_Total = Convert.ToDouble(r.Item("Total"))
                End If
                If IsDBNull(r.Item("SubTotal")) Then
                    m_SubTotal = Nothing
                Else
                    m_SubTotal = Convert.ToDouble(r.Item("SubTotal"))
                End If
                If IsDBNull(r.Item("AdditionalShipping")) Then
                    m_AdditionalShipping = Nothing
                Else
                    m_AdditionalShipping = Convert.ToDouble(r.Item("AdditionalShipping"))
                End If
                If IsDBNull(r.Item("SalePrice")) Then
                    m_SalePrice = Nothing
                Else
                    m_SalePrice = Convert.ToDouble(r.Item("SalePrice"))
                End If
                If IsDBNull(r.Item("RawPrice")) Then
                    m_RawPrice = Nothing
                Else
                    m_RawPrice = Convert.ToDouble(r.Item("RawPrice"))
                End If
                m_IsFreeItem = Convert.ToBoolean(r.Item("IsFreeItem"))
                If IsDBNull(r.Item("IsFreeGift")) Then
                    m_IsFreeGift = False
                Else
                    m_IsFreeGift = Convert.ToBoolean(r.Item("IsFreeGift"))
                End If
                m_IsPromoItem = Convert.ToBoolean(r.Item("IsPromoItem"))
                m_IsTaxFree = Convert.ToBoolean(r.Item("IsTaxFree"))
                Try
                    If r.Item("IsFreeShipping") Is Convert.DBNull Then
                        m_IsFreeShipping = False
                    Else
                        m_IsFreeShipping = CBool(r.Item("IsFreeShipping"))
                    End If
                Catch ex As Exception
                    m_IsFreeShipping = False
                End Try

                m_IsFreeSample = Convert.ToBoolean(r.Item("IsFreeSample"))
                m_IsOversize = Convert.ToBoolean(r.Item("IsOversize"))
                m_IsHazMat = Convert.ToBoolean(r.Item("IsHazMat"))
                If IsDBNull(r.Item("ShipmentDate")) Then
                    m_ShipmentDate = Nothing
                Else
                    m_ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                End If
                If IsDBNull(r.Item("AdditionalType")) Then
                    m_AdditionalType = Nothing
                Else
                    m_AdditionalType = Convert.ToString(r.Item("AdditionalType"))
                End If
                If IsDBNull(r.Item("SKU")) Then
                    m_SKU = Nothing
                Else
                    m_SKU = Convert.ToString(r.Item("SKU"))
                End If
                If IsDBNull(r.Item("Type")) Then
                    If m_SKU <> Nothing Then
                        m_Type = "item"
                    Else
                        m_Type = Nothing
                    End If
                Else
                    m_Type = Convert.ToString(r.Item("Type"))
                End If
                If IsDBNull(r.Item("Status")) Then
                    m_Status = Nothing
                Else
                    m_Status = Convert.ToString(r.Item("Status"))
                End If
                If IsDBNull(r.Item("LineNo")) Then
                    m_LineNo = Nothing
                Else
                    m_LineNo = Convert.ToInt32(r.Item("LineNo"))
                End If
                If r.Item("LineDiscountAmount") Is Convert.DBNull Then
                    m_LineDiscountAmount = Nothing
                Else
                    m_LineDiscountAmount = Convert.ToDouble(r.Item("LineDiscountAmount"))
                End If
                If r.Item("LineDiscountAmountCust") Is Convert.DBNull Then
                    m_LineDiscountAmountCust = Nothing
                Else
                    m_LineDiscountAmountCust = Convert.ToDouble(r.Item("LineDiscountAmountCust"))
                End If
                If r.Item("LineDiscountPercent") Is Convert.DBNull Then
                    m_LineDiscountPercent = Nothing
                Else
                    m_LineDiscountPercent = Convert.ToDouble(r.Item("LineDiscountPercent"))
                End If
                If IsDBNull(r.Item("Prefix")) Then
                    m_Prefix = Nothing
                Else
                    m_Prefix = Convert.ToString(r.Item("Prefix"))
                End If
                If IsDBNull(r.Item("AttributeSKU")) Then
                    m_AttributeSKU = Nothing
                Else
                    m_AttributeSKU = Convert.ToString(r.Item("AttributeSKU"))
                End If
                If IsDBNull(r.Item("Attributes")) Then
                    m_Attributes = Nothing
                Else
                    m_Attributes = Convert.ToString(r.Item("Attributes"))
                End If
                If IsDBNull(r.Item("Swatches")) Then
                    m_Swatches = Nothing
                Else
                    m_Swatches = Convert.ToString(r.Item("Swatches"))
                End If
                m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
                If IsDBNull(r.Item("LastExport")) Then
                    m_LastExport = Nothing
                Else
                    m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
                End If
                If r.Item("CustomerPrice") Is Convert.DBNull Then
                    m_CustomerPrice = Nothing
                Else
                    m_CustomerPrice = Convert.ToDouble(r.Item("CustomerPrice"))
                End If
                If r.Item("PromotionPrice") Is Convert.DBNull Then
                    m_PromotionPrice = Nothing
                Else
                    m_PromotionPrice = Convert.ToDouble(r.Item("PromotionPrice"))
                End If

                If r.Item("CouponPrice") Is Convert.DBNull Then
                    m_CouponPrice = Nothing
                Else
                    m_CouponPrice = Convert.ToDouble(r.Item("CouponPrice"))
                End If

                If IsDBNull(r.Item("PromotionId")) Then
                    m_PromotionID = Nothing
                Else
                    m_PromotionID = Convert.ToInt32(r.Item("PromotionId"))
                End If

                If IsDBNull(r.Item("CouponMessage")) Then
                    m_CouponMessage = Nothing
                Else
                    m_CouponMessage = Convert.ToString(r.Item("CouponMessage"))
                End If

                If r.Item("QuantityPrice") Is Convert.DBNull Then
                    m_QuantityPrice = Nothing
                Else
                    m_QuantityPrice = Convert.ToDouble(r.Item("QuantityPrice"))
                End If
                m_IsRushDelivery = Convert.ToBoolean(r.Item("IsRushDelivery"))
                If IsDBNull(r.Item("RushDeliveryCharge")) Then
                    m_RushDeliveryCharge = Nothing
                Else
                    m_RushDeliveryCharge = r.Item("RushDeliveryCharge")
                End If
                m_IsLiftGate = Convert.ToBoolean(r.Item("IsLiftGate"))
                If IsDBNull(r.Item("LiftGateCharge")) Then
                    m_LiftGateCharge = Nothing
                Else
                    m_LiftGateCharge = r.Item("LiftGateCharge")
                End If
                m_IsScheduleDelivery = Convert.ToBoolean(r.Item("IsScheduleDelivery"))
                If Not IsDBNull(r.Item("ScheduleDeliveryCharge")) Then m_ScheduleDeliveryCharge = Convert.ToDouble(r.Item("ScheduleDeliveryCharge")) Else m_ScheduleDeliveryCharge = Nothing
                m_IsInsideDelivery = Convert.ToBoolean(r.Item("IsInsideDelivery"))
                If Not IsDBNull(r.Item("InsideDeliveryCharge")) Then m_InsideDeliveryCharge = Convert.ToDouble(r.Item("InsideDeliveryCharge")) Else m_InsideDeliveryCharge = Nothing
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
                    If IsDBNull(r.Item("CreatedDate")) Then
                        m_CreatedDate = Nothing
                    Else
                        m_CreatedDate = Convert.ToDateTime(r.Item("CreatedDate"))
                    End If
                Catch ex As Exception
                    m_CreatedDate = Nothing
                End Try
                Try
                    If IsDBNull(r.Item("ModifiedDate")) Then
                        m_ModifiedDate = Nothing
                    Else
                        m_ModifiedDate = Convert.ToDateTime(r.Item("ModifiedDate"))
                    End If
                Catch ex As Exception
                    m_ModifiedDate = Nothing
                End Try
                Try
                    If r.Item("IsReviewed") Is Convert.DBNull Then
                        m_IsReviewed = False
                    Else
                        m_IsReviewed = CBool(r.Item("IsReviewed"))
                    End If
                Catch ex As Exception
                    m_IsReviewed = False
                End Try
                Try
                    If r.Item("AddType") Is Convert.DBNull Then
                        m_AddType = 1
                    Else
                        m_AddType = CInt(r.Item("AddType"))
                    End If
                Catch ex As Exception
                    m_AddType = 1
                End Try
                If IsDBNull(r.Item("RewardPoints")) Then
                    m_RewardPoints = Nothing
                Else
                    m_RewardPoints = Convert.ToInt32(r.Item("RewardPoints"))
                End If
                If IsDBNull(r.Item("IsRewardPoints")) Then
                    m_IsRewardPoints = False
                Else
                    m_IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
                End If
                If IsDBNull(r.Item("SubTotalPoint")) Then
                    m_SubTotalPoint = Nothing
                Else
                    m_SubTotalPoint = Convert.ToInt32(r.Item("SubTotalPoint"))
                End If
                If r.Item("FreeShipping") Is Convert.DBNull Then
                    m_FreeShipping = Nothing
                Else
                    m_FreeShipping = Convert.ToDouble(r.Item("FreeShipping"))
                End If
                Try
                    If IsDBNull(r.Item("IsFirstReview")) Then
                        m_IsFirstReview = False
                    Else
                        m_IsFirstReview = Convert.ToBoolean(r.Item("IsFirstReview"))
                    End If
                Catch

                End Try
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Protected Overridable Sub LoadInPaypalProcess(ByVal r As SqlDataReader)
            Try
                If IsDBNull(r.Item("SKU")) Then
                    m_SKU = Nothing
                Else
                    m_SKU = Convert.ToString(r.Item("SKU"))
                End If
                m_Price = Convert.ToDouble(r.Item("Price"))
                m_IsFreeItem = Convert.ToBoolean(r.Item("IsFreeItem"))
                If IsDBNull(r.Item("IsFreeGift")) Then
                    m_IsFreeGift = False
                Else
                    m_IsFreeGift = Convert.ToBoolean(r.Item("IsFreeGift"))
                End If
                If IsDBNull(r.Item("PromotionId")) Then
                    m_PromotionID = Nothing
                Else
                    m_PromotionID = Convert.ToInt32(r.Item("PromotionId"))
                End If
                If r.Item("CustomerPrice") Is Convert.DBNull Then
                    m_CustomerPrice = Nothing
                Else
                    m_CustomerPrice = Convert.ToDouble(r.Item("CustomerPrice"))
                End If
                If r.Item("QuantityPrice") Is Convert.DBNull Then
                    m_QuantityPrice = Nothing
                Else
                    m_QuantityPrice = Convert.ToDouble(r.Item("QuantityPrice"))
                End If
                If r.Item("PromotionPrice") Is Convert.DBNull Then
                    m_PromotionPrice = Nothing
                Else
                    m_PromotionPrice = Convert.ToDouble(r.Item("PromotionPrice"))
                End If
                If IsDBNull(r.Item("ItemName")) Then
                    m_ItemName = Nothing
                Else
                    m_ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                m_Quantity = Convert.ToInt32(r.Item("Quantity"))
                m_CartItemId = Convert.ToInt32(r.Item("CartItemId"))
                If IsDBNull(r.Item("Type")) Then
                    m_Type = Nothing
                Else
                    m_Type = Convert.ToString(r.Item("Type"))
                End If
                If IsDBNull(r.Item("MixMatchId")) Then
                    m_MixMatchId = Nothing
                Else
                    m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
                End If
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                If IsDBNull(r.Item("DiscountQuantity")) Then
                    m_DiscountQuantity = Nothing
                Else
                    m_DiscountQuantity = Convert.ToInt32(r.Item("DiscountQuantity"))
                End If
                If IsDBNull(r.Item("Total")) Then
                    m_Total = Nothing
                Else
                    m_Total = Convert.ToDouble(r.Item("Total"))
                End If
                If IsDBNull(r.Item("IsRewardPoints")) Then
                    m_IsRewardPoints = False
                Else
                    m_IsRewardPoints = Convert.ToBoolean(r.Item("IsRewardPoints"))
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Try
                Dim sp As String = "sp_StoreCartItem_Insert"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(DB.InParam("CarrierType", SqlDbType.Int, 0, CarrierType))
                cmd.Parameters.Add(DB.InParam("RecipientId", SqlDbType.Int, 0, RecipientId))
                cmd.Parameters.Add(DB.InParam("MixMatchId", SqlDbType.Int, 0, MixMatchId))
                cmd.Parameters.Add(DB.InParam("Weight", SqlDbType.Float, 0, Weight))
                cmd.Parameters.Add(DB.InParam("ItemId", SqlDbType.Int, 0, ItemId))
                cmd.Parameters.Add(DB.InParam("ItemGroupId", SqlDbType.Int, 0, ItemGroupId))
                cmd.Parameters.Add(DB.InParam("DiscountQuantity", SqlDbType.Int, 0, DiscountQuantity))
                cmd.Parameters.Add(DB.InParam("RegistryItemId", SqlDbType.Int, 0, RegistryItemId))
                cmd.Parameters.Add(DB.InParam("Type", SqlDbType.VarChar, 0, Type))
                cmd.Parameters.Add(DB.InParam("ItemName", SqlDbType.VarChar, 0, ItemName))
                cmd.Parameters.Add(DB.InParam("PriceDesc", SqlDbType.VarChar, 0, PriceDesc))
                cmd.Parameters.Add(DB.InParam("Image", SqlDbType.VarChar, 0, Image))
                cmd.Parameters.Add(DB.InParam("DepartmentId", SqlDbType.Int, 0, DepartmentId))
                cmd.Parameters.Add(DB.InParam("Quantity", SqlDbType.Int, 0, Quantity))
                cmd.Parameters.Add(DB.InParam("SKU", SqlDbType.VarChar, 0, SKU))
                cmd.Parameters.Add(DB.InParam("Prefix", SqlDbType.VarChar, 0, Prefix))
                cmd.Parameters.Add(DB.InParam("AttributeSKU", SqlDbType.VarChar, 0, AttributeSKU))
                cmd.Parameters.Add(DB.InParam("Attributes", SqlDbType.VarChar, 0, Attributes))
                cmd.Parameters.Add(DB.InParam("AttributePrice", SqlDbType.Float, 0, AttributePrice))
                cmd.Parameters.Add(DB.InParam("Swatches", SqlDbType.VarChar, 0, Swatches))
                cmd.Parameters.Add(DB.InParam("Price", SqlDbType.Money, 0, Price))
                cmd.Parameters.Add(DB.InParam("RawPrice", SqlDbType.Money, 0, RawPrice))
                cmd.Parameters.Add(DB.InParam("SalePrice", SqlDbType.Money, 0, SalePrice))
                cmd.Parameters.Add(DB.InParam("IsTaxFree", SqlDbType.Bit, 0, CInt(IsTaxFree)))
                cmd.Parameters.Add(DB.InParam("IsFreeItem", SqlDbType.Bit, 0, CInt(IsFreeItem)))
                cmd.Parameters.Add(DB.InParam("IsFreeGift", SqlDbType.Bit, 0, CInt(IsFreeGift)))
                cmd.Parameters.Add(DB.InParam("IsPromoItem", SqlDbType.Bit, 0, CInt(IsPromoItem)))
                cmd.Parameters.Add(DB.InParam("FreeItemIds", SqlDbType.VarChar, 0, FreeItemIds))
                cmd.Parameters.Add(DB.InParam("IsFreeShipping", SqlDbType.Bit, 0, CInt(IsFreeShipping)))
                cmd.Parameters.Add(DB.InParam("ShipmentDate", SqlDbType.DateTime, 0, m_DB.NullDate(ShipmentDate)))
                cmd.Parameters.Add(DB.InParam("Status", SqlDbType.VarChar, 0, Status))
                cmd.Parameters.Add(DB.InParam("LineDiscountPercent", SqlDbType.Float, 0, LineDiscountPercent))
                cmd.Parameters.Add(DB.InParam("LineDiscountAmount", SqlDbType.Float, 0, LineDiscountAmount))
                cmd.Parameters.Add(DB.InParam("LineDiscountAmountCust", SqlDbType.Float, 0, LineDiscountAmountCust))
                cmd.Parameters.Add(DB.InParam("SubTotal", SqlDbType.Float, 0, SubTotal))
                cmd.Parameters.Add(DB.InParam("Total", SqlDbType.Float, 0, Total))
                cmd.Parameters.Add(DB.InParam("AdditionalShipping", SqlDbType.Float, 0, AdditionalShipping))
                cmd.Parameters.Add(DB.InParam("AdditionalType", SqlDbType.VarChar, 0, AdditionalType))
                cmd.Parameters.Add(DB.InParam("LineNo", SqlDbType.Int, 0, LineNo))
                cmd.Parameters.Add(DB.InParam("DoExport", SqlDbType.Bit, 0, CInt(DoExport)))
                cmd.Parameters.Add(DB.InParam("LastExport", SqlDbType.DateTime, 0, m_DB.NullDate(LastExport)))
                cmd.Parameters.Add(DB.InParam("IsOversize", SqlDbType.Bit, 0, CInt(IsOversize)))
                cmd.Parameters.Add(DB.InParam("IsHazMat", SqlDbType.Bit, 0, CInt(IsHazMat)))
                cmd.Parameters.Add(DB.InParam("CustomerPrice", SqlDbType.Float, 0, CustomerPrice))
                cmd.Parameters.Add(DB.InParam("PromotionPrice", SqlDbType.Float, 0, PromotionPrice))
                cmd.Parameters.Add(DB.InParam("QuantityPrice", SqlDbType.Float, 0, QuantityPrice))
                cmd.Parameters.Add(DB.InParam("TotalFreeAllowed", SqlDbType.Int, 0, TotalFreeAllowed))
                cmd.Parameters.Add(DB.InParam("IsRushDelivery", SqlDbType.Bit, 0, CInt(IsRushDelivery)))
                cmd.Parameters.Add(DB.InParam("RushDeliveryCharge", SqlDbType.Float, 0, RushDeliveryCharge))
                cmd.Parameters.Add(DB.InParam("IsLiftGate", SqlDbType.Bit, 0, CInt(IsLiftGate)))
                cmd.Parameters.Add(DB.InParam("LiftGateCharge", SqlDbType.Float, 0, LiftGateCharge))
                cmd.Parameters.Add(DB.InParam("IsScheduleDelivery", SqlDbType.Bit, 0, CInt(IsScheduleDelivery)))
                cmd.Parameters.Add(DB.InParam("ScheduleDeliveryCharge", SqlDbType.Float, 0, ScheduleDeliveryCharge))
                cmd.Parameters.Add(DB.InParam("IsInsideDelivery", SqlDbType.Bit, 0, CInt(IsInsideDelivery)))
                cmd.Parameters.Add(DB.InParam("InsideDeliveryCharge", SqlDbType.Float, 0, InsideDeliveryCharge))
                cmd.Parameters.Add(DB.InParam("CreatedDate", SqlDbType.DateTime, 0, DateTime.Now))
                cmd.Parameters.Add(DB.InParam("ModifiedDate", SqlDbType.DateTime, 0, DateTime.Now))
                cmd.Parameters.Add(DB.InParam("IsFreeSample", SqlDbType.Bit, 0, CInt(IsFreeSample)))
                cmd.Parameters.Add(DB.InParam("IsFlammable", SqlDbType.Bit, 0, CInt(IsFlammable)))
                cmd.Parameters.Add(DB.InParam("IsRewardPoints", SqlDbType.Bit, 0, CInt(IsRewardPoints)))
                cmd.Parameters.Add(DB.InParam("RewardPoints", SqlDbType.Int, 0, RewardPoints))
                cmd.Parameters.Add(DB.InParam("SubTotalPoint", SqlDbType.Int, 0, SubTotalPoint))
                cmd.Parameters.Add(DB.InParam("FreeShipping", SqlDbType.Float, 0, FreeShipping))

                cmd.Parameters.Add(DB.InParam("AddType", SqlDbType.Int, 0, AddType))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                CartItemId = CInt(cmd.Parameters("result").Value)
                Return CartItemId
            Catch ex As Exception
                Email.SendError("ToError500", "Insert Cart Error", "OrderId: " & OrderId & "<br>CarrierType: " & CarrierType & "<br>Sku: " & SKU & "<br>Exception: " & ex.ToString())
            End Try
        End Function

        Public Overridable Sub Update()
            If m_DB Is Nothing Then
                m_DB = New Database
                m_DB.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            End If

            Dim SQL As String = " UPDATE StoreCartItem SET " _
            & " OrderId = " & m_DB.Number(OrderId) _
            & ",CarrierType = " & m_DB.Number(CarrierType) _
            & ",Weight = " & m_DB.Number(Weight) _
            & ",RecipientId = " & m_DB.Number(RecipientId) _
            & ",MixMatchId = " & m_DB.NullNumber(MixMatchId) _
            & ",RegistryItemId = " & m_DB.NullNumber(RegistryItemId) _
            & ",ItemId = " & m_DB.Number(ItemId) _
            & ",ItemGroupId = " & m_DB.NullNumber(ItemGroupId) _
            & ",DiscountQuantity = " & m_DB.Number(DiscountQuantity) _
            & ",ItemName = " & m_DB.Quote(ItemName) _
            & ",[Type] = " & If(IsDBNull(SKU) Or ItemId = 0, m_DB.Quote("carrier"), m_DB.Quote("item")) _
            & ",PriceDesc = " & m_DB.Quote(PriceDesc) _
            & ",Image = " & m_DB.Quote(Image) _
            & ",DepartmentId = " & m_DB.Number(DepartmentId) _
            & ",Quantity = " & m_DB.Number(Quantity) _
            & ",SKU = " & m_DB.Quote(SKU) _
            & ",Prefix = " & m_DB.Quote(Prefix) _
            & ",AttributeSKU = " & m_DB.Quote(AttributeSKU) _
            & ",Attributes = " & m_DB.Quote(Attributes) _
            & ",AttributePrice = " & m_DB.Number(AttributePrice) _
            & ",Swatches = " & m_DB.Quote(Swatches) _
            & ",Price = " & m_DB.Number(Price) _
            & ",RawPrice = " & m_DB.NullDouble(RawPrice) _
            & ",SalePrice = " & m_DB.Number(SalePrice) _
            & ",IsTaxFree = " & CInt(IsTaxFree) _
            & ",IsFreeItem = " & CInt(IsFreeItem) _
            & ",IsFreeGift = " & CInt(IsFreeGift) _
            & ",IsPromoItem = " & CInt(IsPromoItem) _
            & ",FreeItemIds = " & m_DB.Quote(FreeItemIds) _
            & ",IsFreeShipping = " & CInt(IsFreeShipping) _
            & ",ShipmentDate = " & m_DB.Quote(ShipmentDate) _
            & ",Status = " & m_DB.Quote(Status) _
            & ",LineDiscountPercent = " & DB.Quote(LineDiscountPercent) _
            & ",LineDiscountAmount = " & DB.Quote(LineDiscountAmount) _
            & ",LineDiscountAmountCust = " & DB.Quote(LineDiscountAmountCust) _
            & ",SubTotal = " & m_DB.Number(SubTotal) _
             & ",SubTotalPoint = " & m_DB.Number(SubTotalPoint) _
            & ",Total = " & m_DB.Number(Total) _
            & ",AdditionalShipping = " & m_DB.Number(AdditionalShipping) _
            & ",AdditionalType = " & m_DB.Quote(AdditionalType) _
            & ",[LineNo] = " & m_DB.Number(LineNo) _
            & ",DoExport = " & CInt(DoExport) _
            & ",LastExport = " & m_DB.Quote(LastExport) _
            & ",IsOversize = " & CInt(IsOversize) _
            & ",IsHazMat = " & CInt(IsHazMat) _
            & ",CustomerPrice = " & DB.Number(CustomerPrice) _
            & ",PromotionPrice = " & DB.Number(PromotionPrice) _
            & ",QuantityPrice = " & DB.Number(QuantityPrice) _
            & ",TotalFreeAllowed = " & DB.Number(TotalFreeAllowed) _
            & ",IsRushDelivery = " & CInt(IsRushDelivery) _
            & ",RushDeliveryCharge = " & DB.Number(RushDeliveryCharge) _
            & ",IsLiftGate = " & CInt(IsLiftGate) _
            & ",LiftGateCharge = " & DB.Number(LiftGateCharge) _
            & ",IsScheduleDelivery = " & CInt(IsScheduleDelivery) _
            & ",ScheduleDeliveryCharge = " & DB.Number(ScheduleDeliveryCharge) _
            & ",IsInsideDelivery = " & CInt(IsInsideDelivery) _
            & ",InsideDeliveryCharge = " & DB.Number(InsideDeliveryCharge) _
            & ",PromotionID = " & CInt(PromotionID) _
            & ",CouponPrice = " & m_DB.Number(CouponPrice) _
            & ",CouponMessage = " & m_DB.Quote(CouponMessage) _
            & ",ModifiedDate = " & m_DB.Quote(DateTime.Now) _
            & ",IsFreeSample = " & CInt(IsFreeSample) _
             & ",IsFlammable = " & CInt(IsFlammable) _
               & ",IsRewardPoints = " & CInt(IsRewardPoints) _
               & ",RewardPoints = " & CInt(RewardPoints) _
                 & ",FreeShipping = " & m_DB.Number(FreeShipping) _
            & " WHERE " & IIf(CartItemId <> Nothing, "CartItemId = " & DB.Number(CartItemId), "OrderId = " & m_DB.Number(OrderId) & " and ItemId = " & DB.Number(ItemId))

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Overridable Sub UpdatePlus()
            Dim SQL As String

            SQL = " UPDATE StoreCartItem SET " _
             & " OrderId = " & m_DB.Number(OrderId) _
             & ",CarrierType = " & m_DB.Number(CarrierType) _
             & ",Weight = " & m_DB.Number(Weight) _
             & ",RecipientId = " & m_DB.Number(RecipientId) _
             & ",MixMatchId = " & m_DB.NullNumber(MixMatchId) _
             & ",RegistryItemId = " & m_DB.NullNumber(RegistryItemId) _
             & ",ItemId = " & m_DB.Number(ItemId) _
             & ",ItemGroupId = " & m_DB.NullNumber(ItemGroupId) _
             & ",DiscountQuantity = " & m_DB.Number(DiscountQuantity) _
             & ",ItemName = " & m_DB.Quote(ItemName) _
             & ",[Type] = " & If(IsDBNull(SKU) Or ItemId = 0, m_DB.Quote("carrier"), m_DB.Quote("item")) _
             & ",PriceDesc = " & m_DB.Quote(PriceDesc) _
             & ",Image = " & m_DB.Quote(Image) _
             & ",DepartmentId = " & m_DB.Number(DepartmentId) _
             & ",Quantity = " & m_DB.Number(Quantity) _
             & ",SKU = " & m_DB.Quote(SKU) _
             & ",Prefix = " & m_DB.Quote(Prefix) _
             & ",AttributeSKU = " & m_DB.Quote(AttributeSKU) _
             & ",Attributes = " & m_DB.Quote(Attributes) _
             & ",AttributePrice = " & m_DB.Number(AttributePrice) _
             & ",Swatches = " & m_DB.Quote(Swatches) _
             & ",Price = " & m_DB.Number(Price) _
             & ",RawPrice = " & m_DB.NullDouble(RawPrice) _
             & ",SalePrice = " & m_DB.Number(SalePrice) _
             & ",IsTaxFree = " & CInt(IsTaxFree) _
             & ",IsFreeItem = " & CInt(IsFreeItem) _
             & ",IsFreeGift = " & CInt(IsFreeGift) _
             & ",IsPromoItem = " & CInt(IsPromoItem) _
             & ",FreeItemIds = " & m_DB.Quote(FreeItemIds) _
             & ",IsFreeShipping = " & CInt(IsFreeShipping) _
             & ",ShipmentDate = " & m_DB.Quote(ShipmentDate) _
             & ",Status = " & m_DB.Quote(Status) _
             & ",LineDiscountPercent = " & DB.Quote(LineDiscountPercent) _
             & ",LineDiscountAmount = " & DB.Quote(LineDiscountAmount) _
             & ",LineDiscountAmountCust = " & DB.Quote(LineDiscountAmountCust) _
             & ",SubTotal = " & m_DB.Number(SubTotal) _
              & ",SubTotalPoint = " & m_DB.Number(SubTotalPoint) _
             & ",Total = " & m_DB.Number(Total) _
             & ",AdditionalShipping = " & m_DB.Number(AdditionalShipping) _
             & ",AdditionalType = " & m_DB.Quote(AdditionalType) _
             & ",[LineNo] = " & m_DB.Number(LineNo) _
             & ",DoExport = " & CInt(DoExport) _
             & ",LastExport = " & m_DB.Quote(LastExport) _
             & ",IsOversize = " & CInt(IsOversize) _
             & ",IsHazMat = " & CInt(IsHazMat) _
             & ",CustomerPrice = " & DB.Number(CustomerPrice) _
             & ",PromotionPrice = " & DB.Number(PromotionPrice) _
             & ",QuantityPrice = " & DB.Number(QuantityPrice) _
             & ",TotalFreeAllowed = " & DB.Number(TotalFreeAllowed) _
             & ",IsRushDelivery = " & CInt(IsRushDelivery) _
             & ",RushDeliveryCharge = " & DB.Number(RushDeliveryCharge) _
             & ",IsLiftGate = " & CInt(IsLiftGate) _
             & ",LiftGateCharge = " & DB.Number(LiftGateCharge) _
             & ",IsScheduleDelivery = " & CInt(IsScheduleDelivery) _
             & ",ScheduleDeliveryCharge = " & DB.Number(ScheduleDeliveryCharge) _
             & ",IsInsideDelivery = " & CInt(IsInsideDelivery) _
             & ",InsideDeliveryCharge = " & DB.Number(InsideDeliveryCharge) _
             & ",PromotionID = " & CInt(PromotionID) _
             & ",CouponPrice = " & m_DB.Number(CouponPrice) _
             & ",CouponMessage = " & m_DB.Quote(CouponMessage) _
             & ",ModifiedDate = " & m_DB.Quote(DateTime.Now) _
             & ",IsFreeSample = " & CInt(IsFreeSample) _
              & ",IsFlammable = " & CInt(IsFlammable) _
                & ",IsRewardPoints = " & CInt(IsRewardPoints) _
                & ",RewardPoints = " & CInt(RewardPoints) _
             & " WHERE " & IIf(CartItemId <> Nothing, "CartItemId = " & DB.Number(CartItemId), "OrderId = " & m_DB.Number(OrderId) & " and ItemId = " & DB.Number(ItemId))

            m_DB.ExecuteSQL(SQL)
        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreCartItem WHERE " & IIf(CartItemId <> Nothing, "CartItemId = " & DB.Number(CartItemId), "OrderId = " & m_DB.Number(OrderId) & " and ItemId = " & DB.Number(ItemId))
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove

    End Class

    Public Class StoreCartItemCollection
        Inherits GenericCollection(Of StoreCartItemRow)
    End Class

End Namespace
