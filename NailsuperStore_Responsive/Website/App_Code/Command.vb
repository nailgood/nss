Imports System.Reflection
Imports DataLayer
Imports Database
Imports Utility
Imports Components
Imports Components.Core
Imports System.Data.SqlClient
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.IO
Imports System.Collections.Generic
Imports PayPal.Payments.DataObjects
Imports PayPal.Payments.Transactions
Imports PayPal.Payments.Common.Utility
Imports PayPalHandler
Imports System.Linq
Imports System.Net
Imports System.Xml

Namespace Components
    Public Class Command

#Region "Command functionality"
        Private m_CommandName As String = ""
        Public Shared cartItemCount As Integer = 0
        Private popupCart As String = "<div class=""arrow""></div><section>{0}</section>"
        Public Sub New(ByVal commandName As String)
            m_CommandName = commandName
        End Sub

        Public Shared Function Create(ByVal commandName As String) As Command
            Return New Command(commandName)
        End Function

        Public Function Execute(ByVal data As Object) As Object
            Dim type As Type = Me.[GetType]()
            Dim method As MethodInfo = type.GetMethod(m_CommandName)
            Dim args As Object() = New Object() {data}
            Try
                Return method.Invoke(Me, args)
            Catch ex As Exception
                ' TODO: Add logging functionality
                Throw
            End Try
        End Function

        Public Function Execute(ByVal data As Object, ByVal data2 As Object) As Object
            Dim type As Type = Me.[GetType]()
            Dim method As MethodInfo = type.GetMethod(m_CommandName)
            Dim args As Object() = New Object() {data, data2}
            Try
                Return method.Invoke(Me, args)
            Catch ex As Exception
                ' TODO: Add logging functionality
                Throw
            End Try
        End Function

        Public Function Execute(ByVal data As Object()) As Object
            Dim type As Type = Me.[GetType]()
            Dim method As MethodInfo = type.GetMethod(m_CommandName)
            Try
                Return method.Invoke(Me, data)
            Catch ex As Exception
                ' TODO: Add logging functionality
                Throw
            End Try
        End Function



        Public Function RenderUserControl(ByVal path As String, ByVal obj As Object()) As String
            Dim pageHolder As New Page()
            Dim viewControl As UserControl = LoadControl(path, obj)

            pageHolder.Controls.Add(viewControl)
            Dim output As New StringWriter()
            HttpContext.Current.Server.Execute(pageHolder, output, False)
            Return output.ToString()
        End Function

        Private Overloads Function LoadControl(ByVal UserControlPath As String, ByVal ParamArray constructorParameters As Object()) As UserControl
            Dim constParamTypes As New Generic.List(Of Type)
            For Each constParam As Object In constructorParameters
                constParamTypes.Add(constParam.[GetType]())
            Next
            Dim pageHolder As New Page()
            Dim ctl As UserControl = DirectCast(pageHolder.LoadControl(UserControlPath), UserControl)

            ' Find the relevant constructor
            Dim constructor As Reflection.ConstructorInfo = ctl.[GetType]().BaseType.GetConstructor(constParamTypes.ToArray())

            'And then call the relevant constructor
            If constructor Is Nothing Then
                Throw New MemberAccessException("The requested constructor was not found on : " + ctl.[GetType]().BaseType.ToString())
            Else
                constructor.Invoke(ctl, constructorParameters)
            End If

            ' Finally return the fully initialized UC
            Return ctl
        End Function
#End Region

#Region "Search Keyword"
        Public Function SearchKeyword(ByVal Lookup As Object, ByVal LookupKeyword As Object) As Object
            Dim strLog As String = String.Empty
            Dim strSearch As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim result As New Object()

            If Not String.IsNullOrEmpty(Lookup) Then
                strLog = Utility.Common.CheckSearchKeyword(Lookup.Trim())
                strSearch = strLog
            End If

            LogSearch(strLog)
            GetKeywordSearch(strLog)

            If strLog.Length < 1 Then
                linkRedirect = HttpContext.Current.Request.UrlReferrer.ToString
            Else
                'HttpContext.Current.Session.Remove("DepartmentURLCode")

                'Check redirect keyword
                If Not String.IsNullOrEmpty(LookupKeyword) Then
                    linkRedirect = "/store/searchresult.aspx?kw=" & HttpContext.Current.Server.UrlEncode(strSearch) & "&searchkw=1&F_All=Y&F_Search=Y"
                Else
                    Dim redirectLink As String = KeywordRedirectRow.GetLinkRedirect(strSearch.Trim())
                    If Not String.IsNullOrEmpty(redirectLink) Then
                        linkRedirect = redirectLink
                    Else
                        linkRedirect = "/store/searchresult.aspx?kw=" & HttpContext.Current.Server.UrlEncode(strSearch) & "&F_All=Y&F_Search=Y"
                    End If

                End If
            End If


            result(0) = linkRedirect
            Return result
        End Function

        Private Sub LogSearch(ByVal keyword As String)
            Dim strPath As String = Utility.ConfigData.LogSearchFilePath
            Dim sContents As String = ""
            Try
                If Core.FileExists(strPath) = True Then
                    sContents = Core.OpenFile(strPath)
                End If
                sContents = sContents & Date.Now() & "[" & keyword & "]" & vbCrLf
                Core.WriteFile(strPath, sContents)
            Catch ex As Exception

            End Try
        End Sub

        Private Function IsKeywordSKU(ByVal keyword As String) As Boolean
            Dim result As Integer
            Try
                result = CInt(keyword)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Private Sub GetKeywordSearch(ByVal keyword As String)
            If (keyword.Trim() = "Search by keyword or item") Then
                Exit Sub
            End If

            If (IsKeywordSKU(keyword)) Then
                Exit Sub
            End If

            'Check ip allow access
            Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            If KeywordIPExcludeRow.CheckNotAllowTrackingKeyword(ip) Then
                Exit Sub
            End If

            Dim objKeyword As New KeywordRow
            objKeyword.KeywordName = keyword
            Dim memberId As Integer = Common.GetCurrentMemberId()
            'Dim keywordSearchId As Integer = KeywordRow.Insert(objKeyword, memberId)
            'If keywordSearchId > 0 Then
            '    HttpContext.Current.Session("KeywordSearchId") = keywordSearchId
            'Else
            '    HttpContext.Current.Session.Remove("KeywordSearchId")
            'End If
        End Sub
#End Region

#Region "Google Analytics"
        Public Function SubmitGA(ByVal OrderId As Object) As Object
            Dim strError As String = String.Empty
            Dim result As Object() = New Object(0) {}

            Dim DB As New Database()
            Try
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                DB.ExecuteScalar("UPDATE StoreOrder SET IsSubmitGA = 1 WHERE OrderId IN (" & OrderId & ")")

            Catch ex As Exception
                Email.SendError("ToError500", "Submit Google Analytics", "OrderId: " & OrderId & "<br>RawUrl: " & HttpContext.Current.Request.RawUrl() & "<br>Exception:" & ex.ToString())
                'strError = "Convert is invalid. An error occurred while submitting Google Analytics."
            Finally
                DB.Close()
            End Try

            result(0) = ""
            Return result
        End Function
#End Region

#Region "Public execution commands"
        Public Function RemoveCartItemBuyPoint(ByVal ci As Object, ByVal si As Object) As Object
            Dim cartItemId As Integer
            Dim ItemId As Integer
            Dim strError As String = String.Empty
            Dim result As Object() = New Object(3) {} '0=popup cart, 1=error, 2=itemid, 3=count cart item , 4=method return

            Dim sp As New SitePage()
            sp.Cart.RemoveCartItem(cartItemId)

            Try
                cartItemId = CInt(ci)
                ItemId = CInt(si)
            Catch ex As Exception
                strError = "Convert is invalid. An error occurred while adding the items to your cart."
            End Try

            'Process
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            sp.Cart.RemoveItemBuyPoint(cartItemId, CashPointRow.GetTotalCashPointByMember(DB, Utility.Common.GetCurrentMemberId(), Utility.Common.GetCurrentOrderId()))
            sp.Cart.RecalculateOrderDetail("Command.RemoveCartItemBuyPoint")
            DB.Close()

            result(0) = New HtmlString(String.Format(popupCart, Utility.Common.RenderUserControl("~/controls/product/popup-cart.ascx")))
            result(1) = New HtmlString(strError)
            result(2) = ""
            result(3) = sp.Cart.GetCartItemCount()
            Return result
        End Function

        Public Function RemoveCartItem(ByVal ci As Object, ByVal si As Object) As Object
            Dim cartItemId As Integer
            Dim ItemId As Integer
            Dim strError As String = String.Empty
            Dim isDeleteFreeGift As Integer = 0

            Try
                cartItemId = CInt(ci)
                ItemId = CInt(si)
            Catch ex As Exception
                strError = "Convert is invalid. An error occurred while adding the items to your cart."
            End Try

            'Process
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Dim mmId As Integer = 0

            'Neu item point bi remove thi mmId=-1
            Dim sp As New SitePage()
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()

            'Check item free gift
            Dim resultFreeGift As Integer = DB.ExecuteScalar("Select COUNT(*) from StoreCartItem where CartItemId=" & cartItemId & " and IsFreeItem=1 and (FreeItemIds is null or FreeItemIds='')")
            If (resultFreeGift > 0) Then
                isDeleteFreeGift = 1

                If orderId > 0 Then
                    StoreCartItemRow.DeleteFreeItemGift(orderId)
                    Utility.Common.OrderLog(orderId, "RemoveCartItem >> DeleteFreeItemGift", Nothing)
                End If
            Else
                Dim SKU As String = DB.ExecuteScalar("SELECT (SKU + ' | Quantity=' + CAST(Quantity as varchar(1))+ ' | IsFreeItem=' + CAST(IsFreeItem as varchar(1))) AS 'SKU' FROM StoreCartItem WHERE CartItemId=" & cartItemId)
                mmId = DB.ExecuteScalar("SELECT CASE IsRewardPoints WHEN 1 THEN -1 ELSE coalesce(MixmatchId,0) END FROM StoreCartItem WHERE CartItemId=" & cartItemId)
                sp.Cart.RemoveCartItem(cartItemId)
                Utility.Common.OrderLog(orderId, "RemoveCartItem >> RemoveCartItem", {"SKU=" & SKU})
            End If
            If (mmId > 0) Then
                If MixMatchRow.IsDiscountPercent(mmId) > 0 Then
                    Dim isFreeItem As Boolean = CBool(DB.ExecuteScalar("Select IsFreeItem from StoreCartItem where CartItemId=" & cartItemId))
                    If Not isFreeItem Then
                        StoreCartItemRow.ReviseDefaultDiscountPercentItem(DB, orderId, mmId)
                    End If
                Else
                    Dim lstMixMatchId As New System.Collections.Generic.List(Of Integer)
                    lstMixMatchId.Add(mmId)
                    sp.Cart.ResetCartMixMatch(lstMixMatchId)
                End If


            End If

            sp.Cart.RecalculateOrderDetail("Command.RemoveCartItem")
            DB.Close()
            Dim result As Object() = New Object(4) {} '0=popup cart, 1=error, 2=itemid, 3=count cart item, 4 Is Delete Free Gift
            result(0) = New HtmlString(String.Format(popupCart, Utility.Common.RenderUserControl("~/controls/product/popup-cart.ascx")))
            result(1) = New HtmlString(strError)
            result(2) = IIf(mmId < 0, "Point", "") + ItemId.ToString()
            result(3) = sp.Cart.GetCartItemCount()
            result(4) = isDeleteFreeGift
            Return result
        End Function

        Public Function AddCart(ByVal i As Object, ByVal q As Object, ByVal isAddDetail As Boolean) As Object
            Dim ItemId As Integer
            Dim Qty As Integer
            Dim ItemList As Integer = 0
            Dim iList As Boolean = False

            'When error returned
            Dim strItemId As String = String.Empty
            Dim strError As String = String.Empty
            Dim strMaxQty As String = String.Empty

            'Check add cart 1 item or many
            Try
                If CInt(i) > 0 Then
                    iList = False
                End If
            Catch
                iList = True
            End Try

            'Add many items
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Dim sp As New SitePage()
            Dim bError As Boolean = False
            If iList Then
                ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)

                For j As Integer = 0 To i.Length - 1
                    Dim err As String = String.Empty
                    Dim maxqty As String = String.Empty

                    Try
                        ItemId = CInt(i(j))
                        Qty = CInt(q(j))
                        ItemList += 1
                        strItemId &= "|" & ItemId

                        DoAddCart(DB, sp, ItemId, Qty, bError, err, maxqty)

                        strError &= IIf(Not String.IsNullOrEmpty(err), err & "<br><br>", "")
                        strMaxQty &= "|" & maxqty

                        If Not bError Then
                            Utility.Common.AddPointKeyword(ItemId, Utility.Common.ItemAction.AddCart, SysParam.GetValue("SearchPointAddCart"))
                        End If

                    Catch ex As Exception
                    End Try
                Next

                sp.Cart.RecalculateOrderDetail("Command.AddCart > iList")

                'Reset Cart
                strItemId = strItemId.Substring(1)
                strMaxQty = strMaxQty.Substring(1)
            Else
                Try
                    ItemId = CInt(i)
                    Qty = SitePage.QtyInput(q)
                Catch ex As Exception
                    strError = "Item/quantity is invalid. An error occurred while adding the items to your cart."
                End Try

                'Add 1 item
                DoAddCart(DB, sp, ItemId, Qty, bError, strError, strMaxQty)

                If Not bError Then
                    Utility.Common.AddPointKeyword(ItemId, Utility.Common.ItemAction.AddCart, SysParam.GetValue("SearchPointAddCart"))
                    sp.Cart.RecalculateOrderDetail("Command.AddCart > Else")
                End If
            End If
            HttpContext.Current.Session.Remove("GetFedExRate")
            If Not isAddDetail Then
                cartItemCount = sp.Cart.GetCartItemCount()
                Dim result As Object() = New Object(4) {} '0=popup cart, 1=error, 2=maxqty, 3=id, 4=count cart item
                Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
                Dim ci As StoreCartItemRow = StoreCartItemRow.GetCartItem(DB, orderId, ItemId, False, False)
                Dim bRender As Boolean = True
                Dim bAddNew As Boolean = True
                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If iList AndAlso ItemList = 1 Then
                    bRender = False
                Else
                    If ci IsNot Nothing Then
                        bAddNew = (Qty = ci.Quantity)
                        If Not ci.MixMatchId > 0 AndAlso cartItemCount > 1 Then
                            If (countCartRow > 1) Then '' must redirect  becauce render very slowly
                                bRender = False
                            End If
                        End If
                    End If
                End If


                DB.Close()
                If bRender Then
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    result(0) = New HtmlString(String.Format(popupCart, Utility.Common.RenderUserControl("~/controls/product/popup-cart.ascx")))
                    HttpContext.Current.Session.Remove("CartRender")
                Else
                    Dim amt As Double = sp.Cart.Order.RawPriceDiscountAmount()
                    Dim merchandiseSubTotal As Double = sp.Cart.Order.BaseSubTotal + IIf(amt > 0, amt, 0)
                    Dim YouSave As String = "$0"
                    If sp.Cart.Order.TotalDiscount > 0 OrElse amt > 0 Then
                        YouSave = "-" & FormatCurrency(sp.Cart.Order.TotalDiscount + IIf(amt > 0, amt, 0), 2)
                    End If
                    Dim textFreeShip As String = sp.getTextFreeShip(sp.Cart.Order.SubTotal) 'show on popup cart header
                    result(0) = New HtmlString(bAddNew & "@" & BuildRowPopup(ci) & "@" & FormatCurrency(merchandiseSubTotal, 2) & "@" & YouSave & "@" & FormatCurrency(sp.Cart.Order.SubTotal) & "@" & textFreeShip)
                End If

                result(1) = New HtmlString(strError)
                result(2) = strMaxQty
                result(3) = IIf(iList, strItemId, ItemId)
                result(4) = cartItemCount
                Return result
            Else
                'Utility.Common.AddPointKeyword(ItemId, Utility.Common.ItemAction.AddCart, SysParam.GetValue("SearchPointAddCart"))
                DB.Close()
                Dim result As Object() = New Object(1) {}
                result(0) = New HtmlString(strError)
                result(1) = strMaxQty
                Return result
            End If
        End Function

        Public Function AddCartCase(ByVal i As Object, ByVal q As Object, ByVal isAddDetail As Boolean) As Object
            Dim ItemId As Integer
            Dim Qty As Integer
            Dim ItemList As Integer = 0
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()

            'When error returned
            Dim strItemId As String = String.Empty
            Dim strError As String = String.Empty
            Dim strMaxQty As String = String.Empty

            'Add many items
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Dim sp As New SitePage()
            Dim bError As Boolean = False

            Try
                ItemId = CInt(i)
                Qty = CInt(q)
            Catch ex As Exception
            End Try

            Dim strItemName As String = String.Empty
            If ItemId > 0 And Qty > 0 Then
                Try
                    ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                    DB.BeginTransaction()

                    Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                    strItemName = si.ItemName

                    If si.CaseQty > 0 And si.CasePrice > 0 Then
                        Dim TotalInCart As Integer = DB.ExecuteScalar("SELECT [dbo].[fc_StoreCartItem_GetOtherQuantity](" & orderId & ",0," & ItemId & ")")
                        Dim QtyReal As Integer = si.CaseQty * Qty

                        If si.AcceptingOrder <> 2 AndAlso TotalInCart + QtyReal > si.QtyOnHand Then
                            If si.QtyOnHand > 0 Then
                                'Bao loi
                                Dim validQty = Math.Floor(si.QtyOnHand / si.CaseQty)
                                strError &= "We only have " & validQty & " cases in stock for item <b>" & strItemName & "</b> " & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " items in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & "."

                                If si.QtyOnHand - TotalInCart > 0 Then
                                    strMaxQty = Math.Floor((si.QtyOnHand - TotalInCart) / si.CaseQty)
                                Else
                                    strMaxQty = 0 'si.QtyOnHand
                                End If
                            Else
                                'Bao loi
                                strError &= "Item <b>" & strItemName & "</b> is out of stock."
                                strMaxQty = -2
                            End If

                            bError = True
                        Else
                            sp.Cart.Add2Cart(ItemId, Nothing, Qty, 24, "Case")
                            strMaxQty = 0
                            bError = False
                        End If
                    Else
                        'Bao loi
                        strError &= "Item <b>" & strItemName & "</b> is unavailable for buy in bulk."
                        bError = False
                    End If

                    If Not bError Then
                        If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                    Else
                        If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                    End If

                Catch ex As Exception
                    If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                    strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                    strMaxQty = -1
                End Try
            Else
                Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                strItemName = si.ItemName

                If ItemId < 1 Then
                    strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                ElseIf Qty < 1 Then
                    strError &= "Please select at least one item <b>" & strItemName & "</b> to add to your shopping cart."
                    strMaxQty = -1
                End If
            End If

            HttpContext.Current.Session.Remove("GetFedExRate")
            If Not isAddDetail Then
                cartItemCount = sp.Cart.GetCartItemCount()
                Dim result As Object() = New Object(4) {} '0=popup cart, 1=error, 2=maxqty, 3=id, 4=count cart item
                Dim ci As StoreCartItemRow = StoreCartItemRow.GetCartItem(DB, orderId, ItemId, False, False)
                Dim bRender As Boolean = True
                Dim bAddNew As Boolean = True
                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)

                If ci IsNot Nothing Then
                    bAddNew = (Qty = ci.Quantity)
                    If Not ci.MixMatchId > 0 AndAlso cartItemCount > 1 Then
                        If (countCartRow > 1) Then '' must redirect  becauce render very slowly
                            bRender = False
                        End If
                    End If
                End If

                DB.Close()
                If bRender Then
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    result(0) = New HtmlString(String.Format(popupCart, Utility.Common.RenderUserControl("~/controls/product/popup-cart.ascx")))
                    HttpContext.Current.Session.Remove("CartRender")
                Else
                    Dim amt As Double = sp.Cart.Order.RawPriceDiscountAmount()
                    Dim merchandiseSubTotal As Double = sp.Cart.Order.BaseSubTotal + IIf(amt > 0, amt, 0)
                    Dim YouSave As String = "$0"
                    If sp.Cart.Order.TotalDiscount > 0 OrElse amt > 0 Then
                        YouSave = "-" & FormatCurrency(sp.Cart.Order.TotalDiscount + IIf(amt > 0, amt, 0), 2)
                    End If
                    result(0) = New HtmlString(bAddNew & "@" & BuildRowPopup(ci) & "@" & FormatCurrency(merchandiseSubTotal, 2) & "@" & YouSave & "@" & FormatCurrency(sp.Cart.Order.SubTotal))
                End If

                result(1) = New HtmlString(strError)
                result(2) = strMaxQty
                result(3) = ItemId
                result(4) = cartItemCount
                Return result
            Else
                Utility.Common.AddPointKeyword(ItemId, Utility.Common.ItemAction.AddCart, SysParam.GetValue("SearchPointAddCart"))
                DB.Close()
                Dim result As Object() = New Object(1) {}
                result(0) = New HtmlString(strError)
                result(1) = strMaxQty
                Return result
            End If
        End Function

        Private Function BuildRowPopup(ByVal ci As StoreCartItemRow) As String
            Dim strImg As String = String.Empty
            Dim strItemName As String = String.Empty
            Dim strPrice As String = String.Empty
            Dim strFree As String = String.Empty
            Dim strRemove As String = String.Empty

            If ci Is Nothing Then
                Return String.Empty
            End If
            'Link
            Dim linkItemDetail As String = URLParameters.ProductUrl(StoreItemRow.GetRowURLCodeById(ci.ItemId), ci.ItemId)

            'Image
            Dim imgSrc As String = Utility.ConfigData.CDNMediaPath & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image)
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                strImg &= "<img src=""" & imgSrc & """ class=""img"" alt=""" & ci.ItemName & """ width=""58px"" height=""58px"" />"
            Else
                strImg &= "<a href='" & linkItemDetail & "'><img src=""" & imgSrc & """ class=""img"" alt=""" & ci.ItemName & """ width=""58px"" height=""58px"" /></a>"
            End If

            'ItemName
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                strItemName = "" & ci.ItemName & ""
            Else
                strItemName = "<a href='" & linkItemDetail & "'>" & ci.ItemName & "</a>"
            End If

            'Price
            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                strPrice = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
            Else
                If (ci.SubTotal <> ci.Total) Then
                    strPrice = "<span class='price1'>" & FormatCurrency(ci.SubTotal) & "</span> <span class='price2'>" & FormatCurrency(ci.Total) & "</span>"
                Else
                    strPrice = "<span class='price3'>" & FormatCurrency(ci.SubTotal) & "</span>"
                End If
            End If

            'FreeItem
            If ci.IsFreeItem = True Then
                If Not String.IsNullOrEmpty(ci.FreeItemIds) Then
                    strFree = "<div class='free'>FREE Item</div>"
                Else
                    Dim freeText As String = "FREE Gift"
                    'If ci.PromotionID > 0 Then
                    '    Dim objPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, ci.PromotionID)
                    '    If Not objPromotion Is Nothing Then
                    '        If Not objPromotion.IsProductCoupon Then
                    '            freeText = "FREE Item"
                    '        End If
                    '    End If
                    'End If

                    strFree = "<div class='free'>" & freeText & "</div>"
                End If
            End If

            'Remove
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                strRemove = String.Format("<span class=""c-remove"" href=""#"" onclick=""mainScreen.ExecuteCommand('RemoveCartItemBuyPoint', 'methodHandlers.ShowPopupCart', ['{0}', 0]);"">Remove</span>", ci.OrderId)
            Else
                strRemove = String.Format("<span class=""c-remove"" href=""#"" onclick=""mainScreen.ExecuteCommand('RemoveCartItem', 'methodHandlers.ShowPopupCart', ['{0}', '{1}']);"">Remove</span>", ci.CartItemId, ci.ItemId)
            End If

            Dim str As String = String.Format("<div class=""cart-wrapper"" id=""PopCart_{7}"">" _
                & "<div class=""w-img"">" _
                & "<div class=""c-img"" summary=""image"">{0}</div>{1}" _
                & "</div>" _
                & "<div class=""desc"">{2}" _
                & "<div class=""prod-name"">{3}</div>" _
                & "<div class=""sku-qty"">Item# {4} &nbsp;|&nbsp; Qty: {5}</div>" _
                & "</div>" _
                & "<div class=""price-desc"">{6}</div>" _
                & "</div>", strImg, strRemove, strFree, strItemName, ci.SKU, ci.Quantity, strPrice, ci.CartItemId)

            Return str
        End Function

        Private Sub DoAddCart(ByVal DB As Database, ByVal sp As SitePage, ByVal ItemId As Integer, ByVal Qty As Integer, ByRef bError As Boolean, ByRef strError As String, ByRef strMaxQty As String, Optional ByVal isCase As Boolean = False)
            Dim strItemName As String = String.Empty
            If ItemId > 0 And Qty > 0 Then
                Try
                    ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                    DB.BeginTransaction()

                    Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                    strItemName = si.ItemName

                    If (Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) Or si.IsSpecialOrder) Then
                        sp.Cart.Add2Cart(ItemId, Nothing, Qty, 24, IIf(isCase, "Case", "Myself"))
                    Else
                        Dim TotalInCart As Integer = DB.ExecuteScalar("SELECT [dbo].[fc_StoreCartItem_GetOtherQuantity](" & Common.GetCurrentOrderId() & ",0," & ItemId & ")") 'DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(HttpContext.Current.Session("OrderId")) & " and ItemId = " & ItemId)
                        Dim MaximumQty As Integer = si.MaximumQuantity

                        If MaximumQty > 0 AndAlso MaximumQty < Qty Then
                            'Bao loi
                            strError &= "Item <b>" & si.ItemName & "</b> has a maximum purchase quantity of " & MaximumQty & " per order"
                            strMaxQty = MaximumQty
                            bError = True
                        ElseIf TotalInCart + Qty > si.QtyOnHand Then
                            If si.QtyOnHand > 0 Then
                                'Bao loi
                                strError &= "We only have " & si.QtyOnHand & " units in stock for item <b>" & strItemName & "</b> " & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & "."
                                If si.QtyOnHand - TotalInCart > 0 Then
                                    strMaxQty = (si.QtyOnHand - TotalInCart)
                                Else
                                    strMaxQty = 0 'si.QtyOnHand
                                End If
                            Else
                                'Bao loi
                                strError &= "Item <b>" & strItemName & "</b> is out of stock."
                                strMaxQty = -2
                            End If

                            bError = True
                        ElseIf MaximumQty > 0 AndAlso MaximumQty < (Qty + TotalInCart) Then
                            'Bao loi
                            strError &= "Total items <b>" & strItemName & "</b> (including in cart) have a maximum purchase quantity of " & MaximumQty & " per order"
                            strMaxQty = MaximumQty - TotalInCart
                            bError = True
                        Else
                            sp.Cart.Add2Cart(ItemId, Nothing, Qty, 24, IIf(isCase, "Case", "Myself"))
                            strMaxQty = 0
                            bError = False
                        End If
                    End If

                    If Not bError Then
                        If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                    Else
                        If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                    End If

                Catch ex As Exception
                    If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                    strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                    strMaxQty = -1
                End Try
            Else
                Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                strItemName = si.ItemName

                If ItemId < 1 Then
                    strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                ElseIf Qty < 1 Then
                    strError &= "Please select at least one item <b>" & strItemName & "</b> to add to your shopping cart."
                    strMaxQty = -1
                End If
            End If
        End Sub
        Public Function AddCartRewardPoint(ByVal i As Object, ByVal q As Object) As Object
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()

            Dim linkRedirect As String = String.Empty
            Dim strError As String = String.Empty
            Dim htmlPointSummary As String = String.Empty
            Dim cartItemId As Integer = 0
            Dim strItemName As String = String.Empty
            Dim iMaxQty As Integer = 0
            Dim bError As Boolean = False

            Dim ItemId, Qty As Integer
            Dim currentPage As String = HttpContext.Current.Request.Path

            If memberId > 0 Then
                Try
                    ItemId = CInt(i)
                    Qty = SitePage.QtyInput(q)
                Catch ex As Exception
                    strError = "Item/quantity is invalid. An error occurred while adding the items to your cart."
                End Try

                Dim sp As New SitePage()
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                If ItemId > 0 And Qty > 0 Then

                    Try
                        ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                        DB.BeginTransaction()
                        Dim pointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
                        Dim balance As Integer = sp.Cart.GetCurrentBalancePoint(pointAvailable)
                        Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                        strItemName = si.ItemName

                        If (Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) Or si.IsSpecialOrder) Then
                            If (si.RewardPoints * Qty > balance) Then
                                Dim maxItemAdd As Integer = Utility.Common.GetMaximunQtyAddPoint(balance, si.RewardPoints)
                                strError = Utility.Common.GetErrorMaximunAddPointMessage(balance, si.RewardPoints, si.SKU, Qty)
                                If (maxItemAdd = 0) Then
                                    maxItemAdd = 1
                                End If
                                iMaxQty = maxItemAdd
                                bError = True
                            Else
                                cartItemId = sp.Cart.AddRewardPoint2Cart(ItemId, Nothing, Qty, Nothing, "Myself")
                                iMaxQty = 0
                            End If
                        Else
                            Dim TotalInCart As Integer = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & orderId & " and ItemId = " & ItemId)
                            Dim MaximumQty As Integer = si.MaximumQuantity
                            If MaximumQty > 0 AndAlso MaximumQty < Qty Then
                                strError = "Item " & si.ItemName & " has a maximum purchase quantity of " & MaximumQty & " per order"
                                iMaxQty = MaximumQty
                                bError = True
                            ElseIf TotalInCart + Qty > si.QtyOnHand Then
                                If si.QtyOnHand > 0 Then
                                    iMaxQty = si.QtyOnHand
                                    strError = "We only have " & si.QtyOnHand & " units in stock for this item" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & "."
                                Else
                                    strError = "Item <b>" & strItemName & "</b> is out of stock."
                                End If
                                bError = True
                            ElseIf (si.RewardPoints * Qty > balance) Then
                                Dim maxItemAdd As Integer = Utility.Common.GetMaximunQtyAddPoint(balance, si.RewardPoints)
                                strError = Utility.Common.GetErrorMaximunAddPointMessage(balance, si.RewardPoints, si.SKU, Qty)
                                If (maxItemAdd = 0) Then
                                    maxItemAdd = 1
                                End If
                                iMaxQty = maxItemAdd
                                bError = True
                            Else
                                cartItemId = sp.Cart.AddRewardPoint2Cart(ItemId, Nothing, Qty, Nothing, "Myself")
                                iMaxQty = 0
                            End If
                        End If

                        If Not bError Then
                            If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                            sp.Cart.RecalculateOrderDetail("Command.AddCartRewardPoint")
                            HttpContext.Current.Session("CartRender") = sp.Cart
                            If currentPage.Contains("/store/reward-point.aspx") Then
                                htmlPointSummary = Utility.Common.RenderUserControl("~/controls/checkout/reward-point-summary.ascx")
                            ElseIf currentPage.Contains("/members/purchased-product.aspx") Then
                                htmlPointSummary = Utility.Common.RenderUserControl("~/controls/product/popup-cart.ascx")
                            End If
                            HttpContext.Current.Session.Remove("CartRender")
                        Else
                            If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                        End If

                        'If Not bError Then
                        '    sp.Cart.RecalculateOrderDetail()
                        'End If
                    Catch ex As Exception
                        If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                        strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                        iMaxQty = -1
                    End Try
                Else
                    Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                    strItemName = si.ItemName

                    If ItemId < 1 Then
                        strError &= "An error occurred while adding the item <b>" & strItemName & "</b> to your cart"
                    ElseIf Qty < 1 Then
                        strError &= "Please select at least one item <b>" & strItemName & "</b> to add to your shopping cart."
                        iMaxQty = -1
                    End If
                End If

                cartItemCount = sp.Cart.GetCartItemCount()
                DB.Close()
            Else
                linkRedirect = "/members/login.aspx?url=" & currentPage.Replace("/ExecuteCommand", "")
            End If

            Dim result As Object() = New Object(6) {} '0=popup cart, 1=error, 2=maxqty, 3=id, 5=redirect 6=count cart item
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlPointSummary)
            result(2) = iMaxQty
            result(3) = ItemId
            result(4) = cartItemId
            result(5) = linkRedirect
            result(6) = cartItemCount
            Return result
        End Function
        'Public Function RemoveCartItemRewardPoint(ByVal cartItemId As Object, ByVal itemid As Object) As Object
        '    Dim linkRedirect As String = String.Empty
        '    Dim strError As String = String.Empty
        '    Dim htmlPointSummary As String = String.Empty
        '    Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
        '    'Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        '    Dim cartCount As Integer = 0
        '    Dim sp As New SitePage()

        '    If (memberId > 0) Then
        '        Try
        '            Dim DB As New Database()
        '            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
        '            sp.Cart.DB = DB
        '            sp.Cart.RemoveCartItem(cartItemId)
        '            sp.Cart.RecalculateOrderDetail("RemoveCartItemRewardPoint")
        '            HttpContext.Current.Session("CartRender") = sp.Cart
        '            htmlPointSummary = Utility.Common.RenderUserControl("~/controls/checkout/reward-point-summary.ascx")
        '            HttpContext.Current.Session.Remove("CartRender")
        '            cartCount = sp.Cart.GetCartItemCount()
        '            DB.Close()
        '        Catch ex As Exception
        '            strError = "An error occurred while deleting item"
        '        End Try
        '    Else
        '        linkRedirect = "/members/login.aspx"
        '        sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
        '    End If
        '    Dim result As Object() = New Object(4) {}
        '    result(0) = New HtmlString(strError)
        '    result(1) = New HtmlString(htmlPointSummary)
        '    result(2) = itemid
        '    result(3) = linkRedirect
        '    result(4) = cartCount
        '    Return result
        'End Function
        'Public Function BuyItemPoint(ByVal p_itemId As Object) As Object
        '    Dim strError As String = String.Empty
        '    Dim linkRedirect As String = String.Empty
        '    Dim htmlPointSummary As String = String.Empty
        '    Dim lstItemPointDelete As String = String.Empty
        '    Dim cartCount As Integer = 0
        '    Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        '    Dim memberId As Integer = Utility.Common.GetCurrentMemberId()

        '    Dim sp As New SitePage()
        '    If (memberId > 0 And orderId > 0) Then
        '        Dim itemId As Integer = CInt(p_itemId)
        '        Try
        '            Dim DB As New Database()
        '            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
        '            orderId = HttpContext.Current.Session("OrderId")
        '            sp.Cart.DB = DB
        '            Dim pointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
        '            Dim lstPointIdBefore As List(Of Integer) = StoreItemRow.ListItemReWardsPointIdByOrder(orderId)

        '            If (sp.Cart.AddBuyPoint2Cart(itemId, pointAvailable) > 0) Then
        '                sp.Cart.RecalculateOrderDetail("Command.BuyItemPoint")
        '                Dim lstPointIdAfter As List(Of Integer) = StoreItemRow.ListItemReWardsPointIdByOrder(orderId)
        '                If Not lstPointIdBefore Is Nothing AndAlso lstPointIdBefore.Count > 0 Then
        '                    If lstPointIdAfter Is Nothing Or lstPointIdAfter.Count = 0 Then
        '                        For Each pointId As Integer In lstPointIdBefore
        '                            lstItemPointDelete = lstItemPointDelete & pointId & ","
        '                        Next
        '                    End If
        '                End If
        '                HttpContext.Current.Session("CartRender") = sp.Cart
        '                htmlPointSummary = Utility.Common.RenderUserControl("~/controls/checkout/reward-point-summary.ascx")
        '                HttpContext.Current.Session.Remove("CartRender")
        '                cartCount = sp.Cart.GetCartItemCount()
        '                DB.Close()
        '            Else
        '                strError = "An error occurred while adding the items to your cart."
        '            End If
        '        Catch ex As Exception
        '            strError = "An error occurred while deleting item"
        '        End Try
        '    Else
        '        linkRedirect = "/members/login.aspx"
        '        sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
        '    End If

        '    Utility.Common.OrderLog(orderId, "Buy Point", {"ItemId Buy Point=" & p_itemId})
        '    Dim result As Object() = New Object(5) {}
        '    result(0) = New HtmlString(strError)
        '    result(1) = New HtmlString(htmlPointSummary)
        '    result(2) = p_itemId
        '    result(3) = linkRedirect
        '    result(4) = lstItemPointDelete
        '    result(5) = cartCount
        '    Return result
        'End Function
        'Public Function RemoveBuyPointItem(ByVal currentPage As Object) As Object
        '    Dim strError As String = String.Empty
        '    Dim linkRedirect As String = String.Empty
        '    Dim htmlPointSummary As String = String.Empty
        '    Dim htmlCart As String = String.Empty
        '    Dim htmlSumarryBox As String = String.Empty
        '    Dim htmlFreeGift As String = String.Empty
        '    Dim lstItemPointDelete As String = String.Empty
        '    Dim memberId As Integer = Utility.Common.GetCurrentMemberId()

        '    Dim sp As New SitePage()
        '    If (memberId > 0 Or currentPage.ToString().Contains("store/cart.aspx")) Then
        '        Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        '        Try
        '            Dim DB As New Database()
        '            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

        '            If (orderId > 0) Then
        '                sp.Cart.DB = DB
        '                Dim pointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
        '                Dim lstPointIdBefore As List(Of Integer) = StoreItemRow.ListItemReWardsPointIdByOrder(orderId)
        '                If (sp.Cart.RemoveItemBuyPoint(orderId, pointAvailable)) Then
        '                    Utility.CacheUtils.RemoveCacheItemWithPrefix(StoreCartItemRow.cachePrefixKey & "ItemCount_" & orderId)
        '                    sp.Cart.RecalculateOrderDetail("Command.RemoveBuyPointItem")
        '                    If (currentPage.ToString().Contains("store/reward-point.aspx")) Then
        '                        Dim lstPointIdAfter As List(Of Integer) = StoreItemRow.ListItemReWardsPointIdByOrder(orderId)
        '                        HttpContext.Current.Session("CartRender") = sp.Cart
        '                        htmlPointSummary = Utility.Common.RenderUserControl("~/controls/checkout/reward-point-summary.ascx")
        '                        If Not lstPointIdBefore Is Nothing AndAlso lstPointIdBefore.Count > 0 Then
        '                            If lstPointIdAfter Is Nothing Or lstPointIdAfter.Count = 0 Then
        '                                For Each pointId As Integer In lstPointIdBefore
        '                                    lstItemPointDelete = lstItemPointDelete & pointId & ","
        '                                Next
        '                            End If
        '                        End If
        '                    Else

        '                        Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
        '                        HttpContext.Current.Session("CartRender") = objCart
        '                        htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
        '                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
        '                        htmlFreeGift = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-cart.ascx")

        '                    End If
        '                    HttpContext.Current.Session.Remove("CartRender")
        '                    cartItemCount = sp.Cart.GetCartItemCount()
        '                End If
        '            Else
        '                linkRedirect = "/members/login.aspx"
        '                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
        '            End If
        '            DB.Close()
        '        Catch ex As Exception
        '            strError = "An error occurred while deleting item"
        '        End Try
        '    Else
        '        linkRedirect = "/members/login.aspx"
        '        sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
        '    End If
        '    Dim result As Object() = New Object(8) {}
        '    result(0) = New HtmlString(htmlCart)
        '    result(1) = New HtmlString(htmlSumarryBox)
        '    result(2) = New HtmlString(htmlFreeGift)
        '    result(3) = New HtmlString(htmlPointSummary)
        '    result(4) = New HtmlString(strError)
        '    result(5) = cartItemCount
        '    result(6) = linkRedirect
        '    result(7) = currentPage
        '    result(8) = lstItemPointDelete
        '    Return result
        'End Function

        Public Function UpdateCartQty(ByVal cartItemUpdateId As Integer, ByVal lstCartId As String, ByVal lstCartQty As String) As Object
            Dim linkRedirect As String = String.Empty
            Dim lstMixMatchId As New List(Of Integer)
            Dim strError As String = String.Empty
            Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
            Dim validQty As Integer = 0
            Dim bError As Boolean = False
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlFreeGift As String = String.Empty
            Dim htmlFreeSample As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty
            Dim htmlRewardsPoints As String = String.Empty
            Dim memberId As Integer = 0
            Dim orderId As Integer = 0
            Dim firstCartIdError As Integer = 0
            Dim isOK As Integer = 0
            Dim bRemove As Boolean = False
            Try
                orderId = Common.GetCurrentOrderId()
                memberId = Common.GetCurrentMemberId()

                Dim sp As New SitePage()
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                sp.Cart.DB = DB
                Dim pointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
                Dim arrCartId() As String = lstCartId.Split(";")
                Dim arrCartQty() As String = lstCartQty.Split(";")
                Dim ci As StoreCartItemRow
                Dim si As StoreItemRow
                Dim qty As Integer = 0
                Dim qtyCase As Integer = 0
                Dim CartItemId As Integer = 0
                For i As Integer = 0 To arrCartId.Length - 1
                    If String.IsNullOrEmpty(arrCartId(i)) Then
                        Continue For
                    End If
                    CartItemId = CInt(arrCartId(i).ToString())
                    If CartItemId < 1 Then
                        Continue For
                    End If
                    qty = CInt(arrCartQty(i).ToString())
                    If Not IsNumeric(qty) Then
                        If firstCartIdError < 1 Then
                            firstCartIdError = CartItemId
                        End If
                        strError = "Please input a valid quantity"
                        Exit For
                    End If

                    'Connect db to get info
                    ci = StoreCartItemRow.GetRow(DB, CartItemId)
                    si = StoreItemRow.GetRow(DB, ci.ItemId)

                    Dim defaultMMId As Integer = 0
                    If ci.AddType = 2 Then
                        qtyCase = qty 'Save lai qty case de update
                        qty = qty * si.CaseQty
                    End If

                    If (ci.IsRewardPoints) Then
                        ci.MixMatchId = 0
                        If (si.IsRewardPoints) Then
                            Dim oldQty As Integer = ci.Quantity
                            Dim newQty As Integer = qty
                            Dim oldPoint As Integer = oldQty * ci.RewardPoints
                            Dim newBalance As Integer = sp.Cart.GetCurrentBalancePoint(pointAvailable)
                            Dim totalPoint As Integer = oldPoint + newBalance
                            Dim MaxNumberAdd As Integer = Utility.Common.GetMaximunQtyAddPoint(totalPoint, ci.RewardPoints)
                            If (newQty > MaxNumberAdd) Then ''ko du point
                                Dim maxItemAdd As Integer = Utility.Common.GetMaximunQtyAddPoint(totalPoint, ci.RewardPoints)
                                strError = strError & "<br>" & Utility.Common.GetErrorMaximunAddPointMessage(totalPoint, ci.RewardPoints, si.SKU, newQty)
                                bError = True
                                If firstCartIdError < 1 Then
                                    firstCartIdError = CartItemId
                                End If
                                Continue For
                            End If
                        Else
                            strError = strError & "<br> Item <b>" & ci.ItemName & " Item#: " & si.SKU & " </b> not allow by w/Point. Please remove it from your shopping cart and proceed to check out."
                            bError = True
                            If firstCartIdError < 1 Then
                                firstCartIdError = CartItemId
                            End If
                            Continue For
                        End If
                    Else
                        If ci.AddType <> 2 Then
                            defaultMMId = DB.ExecuteScalar("select [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & si.ItemId & "," & customerPriceGroup & ",1)")
                            If (ci.MixMatchId <> defaultMMId AndAlso defaultMMId > 0) Then
                                ci.MixMatchId = defaultMMId
                            ElseIf BaseShoppingCart.CheckCartMixmatchValid(DB, ci.MixMatchId, customerPriceGroup) = False Then
                                If ci.MixMatchId > 0 Then
                                    DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & ci.OrderId & "and  Type='item' and IsFreeItem=1 and MixMatchId=" & ci.MixMatchId)
                                End If
                                ci.MixMatchId = 0
                            End If
                        End If
                    End If
                    If (si.QtyOnHand < 1 And (Not ((Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) And si.IsActive = True) OrElse (si.IsSpecialOrder And si.IsActive = True) OrElse ci.AttributeSKU = "FREE"))) Or (qty < 1 And qty <> ci.Quantity) Then
                        If Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) OrElse si.IsSpecialOrder Then
                            sp.Cart.RemoveItemAttribute(ci.AttributeSKU, ci.OrderId)
                            sp.Cart.RemoveCartItem(ci.CartItemId)
                        Else
                            sp.Cart.RemoveCartItem(ci.CartItemId)
                        End If

                        bRemove = True
                        If Not lstMixMatchId.Contains(ci.MixMatchId) Then
                            lstMixMatchId.Add(ci.MixMatchId)
                        End If
                    Else
                        If CartItemId = cartItemUpdateId Then
                            Dim Others As Integer = DB.ExecuteScalar("SELECT [dbo].[fc_StoreCartItem_GetOtherQuantity](" & ci.OrderId & "," & ci.CartItemId & "," & ci.ItemId & ")")
                            Dim maximumqty As Integer = si.MaximumQuantity
                            If maximumqty > 0 AndAlso maximumqty < qty Then
                                If ci.Quantity <> qty Then
                                    Dim isReviseDiscountItem As Boolean = False
                                    If MixMatchRow.IsDiscountPercent(ci.MixMatchId) > 0 Then
                                        If CInt(maximumqty) < ci.Quantity Then
                                            isReviseDiscountItem = True
                                        End If
                                    Else
                                        If Not lstMixMatchId.Contains(ci.MixMatchId) Then
                                            lstMixMatchId.Add(ci.MixMatchId)
                                        End If
                                    End If


                                    If StoreCartItemRow.UpdateCartQty(DB, ci.CartItemId, ci.OrderId, CInt(maximumqty), ci.IsRewardPoints) Then
                                        ci.Quantity = CInt(maximumqty)
                                        sp.Cart.RecalculateCartItem(ci, True)
                                        If isReviseDiscountItem Then
                                            StoreCartItemRow.ReviseDefaultDiscountPercentItem(DB, ci.OrderId, ci.MixMatchId)
                                        End If
                                    End If

                                    strError = strError & "<br>Item <b>" & si.ItemName & " #" & si.SKU & "</b> has a maximum purchase quantity of " & maximumqty & " per order"
                                    validQty = maximumqty
                                    If firstCartIdError < 1 Then
                                        firstCartIdError = CartItemId
                                    End If
                                    bError = True
                                End If
                            ElseIf Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) OrElse si.IsSpecialOrder OrElse (Not String.IsNullOrEmpty(ci.AttributeSKU)) Then
                                'If item special
                                If ci.Quantity <> qty And ci.AttributeSKU <> "FREE" Then
                                    Dim isReviseDiscountItem As Boolean = False
                                    Dim newQty As Integer = 0
                                    If ci.AddType = 2 Then
                                        newQty = qtyCase
                                    Else
                                        newQty = qty
                                        If ci.MixMatchId > 0 Then
                                            If MixMatchRow.IsDiscountPercent(ci.MixMatchId) > 0 Then
                                                If qty < ci.Quantity Then
                                                    isReviseDiscountItem = True
                                                End If
                                            Else
                                                If Not lstMixMatchId.Contains(ci.MixMatchId) Then
                                                    lstMixMatchId.Add(ci.MixMatchId)
                                                End If
                                            End If
                                        End If
                                    End If

                                    If StoreCartItemRow.UpdateCartQty(DB, ci.CartItemId, ci.OrderId, newQty, ci.IsRewardPoints) Then
                                        ci.Quantity = newQty
                                        sp.Cart.RecalculateCartItem(ci, True)
                                        If isReviseDiscountItem Then
                                            StoreCartItemRow.ReviseDefaultDiscountPercentItem(DB, ci.OrderId, ci.MixMatchId)
                                        End If
                                    End If

                                    If sp.Cart.CheckItemAttribute(ci.ItemId, ci.CartItemId, ci.OrderId) = False Then
                                        UpdateItemAttribute(DB, ci.ItemId, ci.OrderId, newQty)
                                    End If
                                End If
                            Else
                                If si.QtyOnHand - Others >= qty Then
                                    Dim oldQty As Integer = ci.Quantity
                                    Dim newQty As Integer = IIf(ci.AddType = 2, qtyCase, qty)
                                    If (oldQty <> newQty) Then
                                        Dim isReviseDiscountItem As Boolean = False
                                        If ci.MixMatchId > 0 Then
                                            If MixMatchRow.IsDiscountPercent(ci.MixMatchId) > 0 Then
                                                If qty <> ci.Quantity Then
                                                    isReviseDiscountItem = True
                                                End If
                                            Else
                                                If Not lstMixMatchId.Contains(ci.MixMatchId) Then
                                                    lstMixMatchId.Add(ci.MixMatchId)
                                                End If
                                            End If
                                        End If

                                        If (StoreCartItemRow.UpdateCartQty(DB, ci.CartItemId, ci.OrderId, newQty, ci.IsRewardPoints)) Then
                                            ci.Quantity = newQty
                                            sp.Cart.RecalculateCartItem(ci, True)
                                            If isReviseDiscountItem Then
                                                StoreCartItemRow.ReviseDefaultDiscountPercentItem(DB, ci.OrderId, ci.MixMatchId)
                                            End If
                                        End If
                                    End If
                                Else
                                    If ci.AddType = 2 Then
                                        validQty = Math.Floor(si.QtyOnHand / si.CaseQty)
                                        strError = strError & String.Format("<br>We only have {0:N0} cases in stock for item <b>{1} Item#: {2}</b>. Please revise quantity it from your shopping cart and proceed to check out.", validQty, ci.ItemName, ci.SKU)
                                    Else
                                        strError = strError & "<br>We only have " & (si.QtyOnHand - Others) & " units in stock for item <b>" & ci.ItemName & " Item#: " & ci.SKU & "</b>, and there are already " & ci.Quantity + Others & " in your shopping cart. Please revise quantity it from your shopping cart and proceed to check out."
                                        validQty = (si.QtyOnHand - Others)
                                    End If

                                    bError = True
                                    If firstCartIdError < 1 Then
                                        firstCartIdError = CartItemId
                                    End If
                                    Continue For
                                End If
                            End If
                            'End If
                        End If
                    End If
                Next

                If Not bError Then
                    isOK = 1
                    HttpContext.Current.Session.Remove("GetFedExRate")
                    Utility.Common.OrderLog(orderId, "UpdateCartQty", {"CartItemUpdateId=" & cartItemUpdateId, "lstCartId=" & lstCartId, "lstCartQty=" & lstCartQty})
                    'Update success
                    If sp.Cart.ResetCartMixMatch(lstMixMatchId) Then
                        linkRedirect = "/store/cart.aspx"
                        DB.Close()
                        Exit Try
                    End If
                    sp.Cart.RecalculateOrderDetail("Page_Load")
                    Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)
                    Utility.Common.DeleteCachePopupCart(orderId)

                    'Render cart HTML
                    Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                    HttpContext.Current.Session("CartRender") = objCart
                    If ci.MixMatchId > 0 Or bRemove Then
                        Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                        If (countCartRow > Utility.ConfigData.MaxCartCount) Then '' must redirect  becauce render very slowly
                            linkRedirect = "/store/cart.aspx"
                            DB.Close()
                            Exit Try
                            'htmlCart = NSS.DisplayCartPricing(DB, ci, True, True)
                            'htmlCart &= "|" & NSS.DisplayCartPricing(DB, ci, True, False)
                        Else
                            htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                        End If
                    Else
                        If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                            htmlCart = Utility.Common.FormatPointPrice(ci.RewardPoints)
                            htmlCart &= "|" & Utility.Common.FormatPointPrice(ci.SubTotalPoint)
                        Else
                            htmlCart = NSS.DisplayCartPricing(DB, ci, True, True)
                            htmlCart &= "|" & NSS.DisplayCartPricing(DB, ci, True, False)
                        End If
                    End If

                    If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                        Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                        Dim r As Object()
                        If arr(0) <> "US" Then
                            r = ChangeCountryEstimateShipping(arr(0), "")
                            htmlSumarryBox = r(0).ToString()
                        ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                            r = CalEstimateShipping(arr(0), arr(1))
                            htmlEstimatePopup = r(0).ToString()
                            htmlSumarryBox = r(2).ToString()
                        Else
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        End If

                        If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                            If Not String.IsNullOrEmpty(strError) Then
                                strError &= "<br><br>"
                            End If
                            strError = r(1).ToString()
                        End If
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    htmlFreeGift = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-cart.ascx")
                    htmlFreeSample = Utility.Common.RenderUserControl("~/controls/checkout/free-samples-cart.ascx")
                    htmlRewardsPoints = Utility.Common.RenderUserControl("~/controls/checkout/rewards-points-cart.ascx")
                    cartItemCount = sp.Cart.GetCartItemCount()
                    If ShoppingCart.CheckTotalFreeSample(objCart.Order.OrderId) Then
                        strError = String.Format(Resources.Alert.FreeSamplesMin, CDbl(SysParam.GetValue("FreeSampleOrderMin")))
                    End If
                    Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(objCart.Order.OrderId)
                    If MinFreeGiftLevelInvalid > 0 Then
                        If Not String.IsNullOrEmpty(strError) Then
                            strError = strError & "<br/><br/>"
                        End If
                        If MinFreeGiftLevelInvalid > 150 Then
                            strError = strError & String.Format(Resources.Alert.FreeGiftMin2, MinFreeGiftLevelInvalid)
                        Else
                            strError = strError & String.Format(Resources.Alert.FreeGiftMin, MinFreeGiftLevelInvalid)
                        End If

                    End If

                    HttpContext.Current.Session.Remove("CartRender")
                End If
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(12) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlEstimatePopup)
            result(3) = New HtmlString(htmlFreeGift)
            result(4) = New HtmlString(htmlFreeSample)
            result(5) = New HtmlString(htmlRewardsPoints)
            result(6) = New HtmlString(strError)
            result(7) = cartItemUpdateId
            result(8) = validQty
            result(9) = cartItemCount
            result(10) = firstCartIdError
            result(11) = isOK
            result(12) = linkRedirect
            Return result
        End Function

        Private Sub UpdateItemAttribute(ByVal DB As Database, ByVal ItemId As Integer, ByVal orderid As Integer, ByVal Qty As Integer)
            Dim dt As DataTable = DB.GetDataTable("select sku from StoreAttribute where itemid = " & ItemId) '' StoreItemRow.GetListStoreItem("select * from StoreAttribute where itemid = " & ItemId)
            Dim dtCart As DataTable
            Dim i, j As Integer
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dtCart = DB.GetDataTable("select cartitemid,quantity from storecartitem where sku like " & dt.Rows(i)("sku") & " and orderid = " & orderid) ''StoreItemRow.GetListStoreItem("select * from storecartitem where sku like " & dt.Rows(i)("sku") & " and orderid = " & orderid)
                    For j = 0 To dtCart.Rows.Count - 1
                        If dtCart.Rows(j)("quantity") > Qty Then
                            DB.ExecuteSQL("update storecartitem set quantity = " & Qty & " where cartitemid = " & dtCart.Rows(j)("cartitemid"))
                        Else
                            DB.ExecuteSQL("update storecartitem set quantity = " & dtCart.Rows(j)("quantity") & " where cartitemid = " & dtCart.Rows(j)("cartitemid"))
                        End If
                    Next
                Next
            End If

        End Sub
        Public Function DeleteCartItem(ByVal cartItemId As Object) As Object
            Dim isOK As Integer = 0
            Dim strError As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlFreeGift As String = String.Empty
            Dim htmlFreeSamples As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty
            Dim htmlRewardsPoints As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = 0
            Dim orderId As Integer = 0
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                orderId = Utility.Common.GetCurrentOrderId()
                memberId = Utility.Common.GetCurrentMemberId()

                Dim sp As New SitePage()
                Dim pointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
                Dim ci As StoreCartItemRow = StoreCartItemRow.GetRow(DB, CInt(cartItemId))
                If ci.AttributeSKU <> Nothing Then
                    sp.Cart.RemoveItemAttribute(ci.AttributeSKU, ci.OrderId)
                End If
                Dim lstMM As New List(Of Integer)
                sp.Cart.RemoveCartItem(ci.CartItemId)
                If MixMatchRow.IsDiscountPercent(ci.MixMatchId) > 0 Then
                    If Not ci.IsFreeItem Then
                        StoreCartItemRow.ReviseDefaultDiscountPercentItem(DB, ci.OrderId, ci.MixMatchId)
                    End If
                Else
                    lstMM.Add(ci.MixMatchId)
                End If
                sp.Cart.ResetCartMixMatch(lstMM)
                sp.Cart.RecalculateOrderDetail("Command.DeleteCartItem")

                Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                HttpContext.Current.Session("CartRender") = objCart
                If ci.MixMatchId > 0 Then
                    If Not (ci.IsFreeItem = True And ci.SubTotal > 0 And ci.MixMatchDescription.Contains("%")) Then
                        If StoreCartItemRow.CountOtherCartItemSameMixmatch(DB, orderId, ci.CartItemId, ci.MixMatchId) > 0 Then
                            Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                            If (countCartRow > Utility.ConfigData.MaxCartCount) Then '' must redirect  becauce render very slowly
                                linkRedirect = "/store/cart.aspx"
                            Else
                                htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                    Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                    Dim r As Object()
                    If arr(0) <> "US" Then
                        r = ChangeCountryEstimateShipping(arr(0), "")
                        htmlSumarryBox = r(0).ToString()
                    ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                        r = CalEstimateShipping(arr(0), arr(1))
                        htmlEstimatePopup = r(0).ToString()
                        htmlSumarryBox = r(2).ToString()
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                        If Not String.IsNullOrEmpty(strError) Then
                            strError &= "<br><br>"
                        End If
                        strError = r(1).ToString()
                    End If
                Else
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                End If

                htmlFreeGift = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-cart.ascx")
                htmlFreeSamples = Utility.Common.RenderUserControl("~/controls/checkout/free-samples-cart.ascx")
                htmlRewardsPoints = Utility.Common.RenderUserControl("~/controls/checkout/rewards-points-cart.ascx")
                cartItemCount = sp.Cart.GetCartItemCount()
                HttpContext.Current.Session.Remove("CartRender")
                HttpContext.Current.Session.Remove("GetFedExRate")
                isOK = 1

                Utility.Common.OrderLog(orderId, "DeleteCartItem", {"SKU=" & ci.SKU, "Quantity=" & ci.Quantity, "IsFreeItem=" & ci.IsFreeItem, "MixMatchId=" & ci.MixMatchId})

                If sp.Cart.CheckTotalFreeSample(sp.Cart.Order.OrderId) Then
                    strError = String.Format(Resources.Alert.FreeSamplesMin, CDbl(SysParam.GetValue("FreeSampleOrderMin")))
                End If
                Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(objCart.Order.OrderId)
                If MinFreeGiftLevelInvalid > 0 Then
                    If Not String.IsNullOrEmpty(strError) Then
                        strError = strError & "<br/><br/>"
                    End If
                    strError = strError & String.Format(Resources.Alert.FreeGiftMin, MinFreeGiftLevelInvalid)
                End If

                If HttpContext.Current.Request.RawUrl.Contains("revise-cart.aspx") Then
                    If cartItemCount = 0 OrElse (cartItemCount <= 4 AndAlso objCart.Order.SubTotal = 0) Then
                        linkRedirect = "/store/cart.aspx"
                        strError = String.Empty
                    End If
                End If

                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(9) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = New HtmlString(htmlFreeGift)
            result(4) = New HtmlString(htmlFreeSamples)
            result(5) = New HtmlString(htmlRewardsPoints)
            result(6) = New HtmlString(strError)
            result(7) = cartItemCount
            result(8) = isOK
            result(9) = linkRedirect
            Return result
        End Function

        Public Function DeleteCartItemMixMatch(ByVal ItemId As Object) As Object
            Dim strError As String = String.Empty
            Dim orderId As Integer = 0
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                orderId = Utility.Common.GetCurrentOrderId()

                Dim sp As New SitePage()
                sp.Cart.DeleteCartItemMixMatch(ItemId, orderId)

                Utility.Common.OrderLog(orderId, "DeleteCartItemMixMatch", {"ItemId=" & ItemId})
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(strError)
            result(1) = ItemId
            Return result
        End Function

        Public Function MoveSaveCartItem(ByVal itemId As Object, ByVal Qty As Object, Optional ByVal isCase As Boolean = False) As Object
            Dim SaveCartCount As Integer = 0
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim strError As String = String.Empty
            Dim strMaxQty As String = String.Empty
            Dim bError As Boolean = False

            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty
            Dim htmlFreeGift As String = String.Empty
            Dim htmlFreeSamples As String = String.Empty
            Dim htmlRewardsPoints As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim htmlSaveCart As String = String.Empty
            Dim isOK As Integer = 0
            Try

                If memberId < 1 Then
                    strError = Resources.Alert.SaveForLaterNoMember
                ElseIf itemId < 1 Then
                    strError = Resources.Alert.UnknowError
                Else
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    Dim sp As New SitePage()

                    'Add 1 item
                    DoAddCart(DB, sp, itemId, Qty, bError, strError, strMaxQty, isCase)

                    If Not bError Then
                        sp.Cart.RecalculateOrderDetail("Command.AddCart > Else")
                        HttpContext.Current.Session.Remove("GetFedExRate")
                        If Not SaveCartRow.Delete(itemId, memberId, isCase) Then
                            strError = Resources.Alert.UnknowError
                        End If
                        Utility.Common.OrderLog(orderId, "MoveSaveCartItem", {"itemId=" & itemId, "Qty=" & Qty, "isCase=" & isCase})
                        SaveCartCount = SaveCartRow.CountSaveCart(memberId)
                        If orderId < 1 Then
                            orderId = Utility.Common.GetCurrentOrderId()
                        End If

                        Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                        If (countCartRow > Utility.ConfigData.MaxCartCount) Then '' must redirect  becauce render very slowly
                            linkRedirect = "/store/cart.aspx"
                        Else
                            'Update success
                            'sp.Cart.ResetCartMixMatch(lstMixMatchId)
                            Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)

                            'Render cart HTML
                            Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                            HttpContext.Current.Session("CartRender") = objCart
                            htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")

                            If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                                Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                                Dim r As Object()
                                If arr(0) <> "US" Then
                                    r = ChangeCountryEstimateShipping(arr(0), "")
                                    htmlSumarryBox = r(0).ToString()
                                ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                                    r = CalEstimateShipping(arr(0), arr(1))
                                    htmlEstimatePopup = r(0).ToString()
                                    htmlSumarryBox = r(2).ToString()
                                Else
                                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                                End If

                                If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                                    If Not String.IsNullOrEmpty(strError) Then
                                        strError &= "<br><br>"
                                    End If
                                    strError = r(1).ToString()
                                End If
                            Else
                                htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                            End If

                            htmlFreeGift = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-cart.ascx")
                            htmlFreeSamples = Utility.Common.RenderUserControl("~/controls/checkout/free-samples-cart.ascx")
                            htmlRewardsPoints = Utility.Common.RenderUserControl("~/controls/checkout/rewards-points-cart.ascx")
                            cartItemCount = sp.Cart.GetCartItemCount()
                            htmlSaveCart = Utility.Common.RenderUserControl("~/controls/checkout/cart-saveforlater.ascx")
                            HttpContext.Current.Session.Remove("CartRender")
                            isOK = 1
                        End If
                    End If

                    DB.Close()
                End If
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(14) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = New HtmlString(htmlFreeGift)
            result(4) = New HtmlString(htmlFreeSamples)
            result(5) = New HtmlString(htmlRewardsPoints)
            result(6) = New HtmlString(strError)
            result(7) = strMaxQty
            result(8) = cartItemCount
            result(9) = SaveCartCount
            result(10) = isOK
            result(11) = linkRedirect
            result(12) = itemId
            result(13) = New HtmlString(htmlSaveCart)
            Return result

        End Function

        Public Function SaveCartItem(ByVal cartItemId As Object) As Object
            Dim strError As String = String.Empty
            Dim htmlSaveCart As String = String.Empty
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim resultTemp As Object() = New Object(8) {}
            Dim linkRedirect = String.Empty
            Try
                If memberId < 1 Then
                    strError = Resources.Alert.SaveForLaterNoMember
                    linkRedirect = "/members/login.aspx"
                ElseIf String.IsNullOrEmpty(cartItemId) OrElse cartItemId = "0" Then
                    strError = Resources.Alert.UnknowError
                Else
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    Dim sp As New SitePage()
                    Dim ci As StoreCartItemRow = StoreCartItemRow.GetRow(DB, CInt(cartItemId))
                    Dim sc As New SaveCartRow()
                    If ci.ItemId = 0 Then
                        Exit Try
                    End If
                    sc.ItemId = ci.ItemId
                    sc.MemberId = memberId
                    sc.Qty = ci.Quantity
                    sc.Type = IIf(ci.AddType <> 2, ci.Type, "case")
                    If SaveCartRow.Insert(sc) Then
                        DB.Close()
                        resultTemp = DeleteCartItem(cartItemId)
                        Utility.Common.OrderLog(ci.OrderId, "SaveCartItem", {"SKU=" & ci.SKU, "Quantity=" & ci.Quantity, "IsFreeItem=" & ci.IsFreeItem, "MixMatchId=" & ci.MixMatchId})
                        htmlSaveCart = Utility.Common.RenderUserControl("~/controls/checkout/cart-saveforlater.ascx")
                    End If
                End If
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(10) {}
            result(0) = resultTemp(0)
            result(1) = resultTemp(1)
            result(2) = resultTemp(2)
            result(3) = resultTemp(3)
            result(4) = resultTemp(4)
            result(5) = resultTemp(5)
            result(6) = resultTemp(6)
            result(7) = New HtmlString(htmlSaveCart)
            result(8) = resultTemp(7)
            result(9) = resultTemp(8)

            If String.IsNullOrEmpty(linkRedirect) Then
                result(10) = resultTemp(9)
            Else
                result(10) = linkRedirect
            End If
            Return result
        End Function

        Public Function DeleteSaveCartItem(ByVal itemId As Object, Optional ByVal isCase As Boolean = False) As Object
            Dim SaveCartCount As Integer = 0
            Dim strError As String = String.Empty
            Dim memberId As Integer = 0

            Try
                memberId = Utility.Common.GetCurrentMemberId()

                If memberId < 1 Then
                    strError = Resources.Alert.SaveForLaterNoMember
                Else
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    Dim sp As New SitePage()
                    If SaveCartRow.Delete(itemId, memberId, isCase) Then
                        Utility.Common.OrderLog(Utility.Common.GetCurrentOrderId(), "DeleteSaveCartItem", {"SKU=" & itemId, "itemId=" & itemId.ToString()})
                        SaveCartCount = SaveCartRow.CountSaveCart(memberId)
                    Else
                        strError = "Delete is error!"
                    End If

                    DB.Close()
                End If
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(strError)
            result(1) = SaveCartCount
            Return result

        End Function

        Public Function DeleteCartFreeGiftItem(ByVal cartItemId As Object) As Object
            Dim isOK As Integer = 0
            Dim strError As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlFreeSamples As String = String.Empty
            Dim htmlFreeGift As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty
            Dim memberId As Integer = 0
            Dim orderId As Integer = 0
            Dim linkRedirect As String = String.Empty
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                orderId = Utility.Common.GetCurrentOrderId()
                memberId = Utility.Common.GetCurrentMemberId()

                Dim sp As New SitePage()
                Dim prevFreeGiftid As Integer = StoreCartItemRow.DeleteFreeItemGift(orderId)
                sp.Cart.RecalculateOrderDetail("Command.DeleteCartFreeGiftItem")

                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If (countCartRow > Utility.ConfigData.MaxCartCount) Then
                    linkRedirect = "/store/cart.aspx"
                Else
                    Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                    HttpContext.Current.Session("CartRender") = objCart

                    If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                        Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                        Dim r As Object()
                        If arr(0) <> "US" Then
                            r = ChangeCountryEstimateShipping(arr(0), "")
                            htmlSumarryBox = r(0).ToString()
                        ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                            r = CalEstimateShipping(arr(0), arr(1))
                            htmlEstimatePopup = r(0).ToString()
                            htmlSumarryBox = r(2).ToString()
                        Else
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        End If

                        If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                            If Not String.IsNullOrEmpty(strError) Then
                                strError &= "<br><br>"
                            End If
                            strError = r(1).ToString()
                        End If
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    HttpContext.Current.Session.Remove("CartRender")
                    HttpContext.Current.Session.Remove("GetFedExRate")
                End If

                isOK = 1
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(5) {}
            result(0) = New HtmlString(htmlEstimatePopup)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlFreeGift)
            result(3) = New HtmlString(strError)
            result(4) = isOK
            result(5) = linkRedirect
            Return result
        End Function
        Public Function SelectFreeGift(ByVal itemId As Object) As Object

            Dim strError As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim orderId As Integer = 0
            Try
                orderId = Utility.Common.GetCurrentOrderId()

                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                Dim CurrentFreeGiftId As Integer = DB.ExecuteScalar("select CartItemId from storeCartItem WITH (NOLOCK) where orderid = " & orderId & " and isfreeitem = 1 and freeitemids is null")
                Dim ci As StoreCartItemRow = StoreCartItemRow.GetRow(DB, CurrentFreeGiftId)
                Dim oldquantity As Integer = ci.Quantity
                If DB.ExecuteScalar("select top 1 coalesce(cartitemid,0) from storecartitem WITH (NOLOCK) where orderid = " & orderId & " and isfreeitem = 1 and itemid = " & itemId) > 0 Then
                    ci.Remove()
                    ci = StoreCartItemRow.GetRow(DB, orderId, itemId, Nothing, Nothing, True, True, ci.AddType)
                    ci.Quantity += oldquantity
                Else
                    ci.UpdateItemDetails(itemId)
                End If
                ci.Update()
                htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                DB.Close()
            Catch ex As Exception

                strError = ex.Message
            End Try

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(strError)

            Return result

        End Function
        Public Function InsertCartFreeItem(ByVal mmId As Integer, ByVal TotalFreeAllowed As Integer, ByVal FreeItemIds As String, ByVal DefaultFreeItemIdSelect As Integer, ByVal arrItemId As Object, ByVal arrQty As Object) As Object
            Dim linkRedirect As String = String.Empty
            Dim strError As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim memberId As Integer = 0
            Dim orderId As Integer = 0
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Try

                Dim sp As New SitePage()
                sp.Cart.DB = DB
                DB.BeginTransaction()
                orderId = Utility.Common.GetCurrentOrderId()
                memberId = Utility.Common.GetCurrentMemberId()

                'Delete old free item
                If DefaultFreeItemIdSelect > 0 Then
                    DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & orderId & " and MixMatchId =" & mmId & " and IsFreeItem=1 and ItemId<>" & DefaultFreeItemIdSelect)
                Else
                    DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & orderId & " and MixMatchId =" & mmId & " and IsFreeItem=1 ")
                End If

                Dim itemId As Integer = 0
                Dim qty As Integer = 0
                Dim cartItemIdFree As Integer = 0
                For i As Integer = 0 To arrItemId.Length - 1
                    itemId = CInt(arrItemId(i))
                    qty = CInt(arrQty(i))
                    If (itemId > 0 AndAlso qty > 0 AndAlso itemId <> DefaultFreeItemIdSelect) Then
                        cartItemIdFree = sp.Cart.Add2Cart(itemId, Nothing, qty, 0, "Myself", "", "", 0, "", True, True, mmId)

                    End If
                Next

                DB.ExecuteSQL("Update StoreCartItem set TotalFreeAllowed=" & TotalFreeAllowed & ", FreeItemIds='" & FreeItemIds & "' where OrderId =" & orderId & " and IsFreeItem=1 and MixMatchId =" & mmId)

                If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                objCart.RecalculateOrderDetail("Page_Load")
                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If (countCartRow > Utility.ConfigData.MaxCartCount) Then '' must redirect  becauce render very slowly
                    linkRedirect = "/store/cart.aspx"
                Else
                    HttpContext.Current.Session("CartRender") = objCart
                    htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    cartItemCount = sp.Cart.GetCartItemCount()
                    HttpContext.Current.Session.Remove("CartRender")
                End If
            Catch ex As Exception
                If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                strError = ex.Message
            End Try
            DB.Close()
            Dim result As Object() = New Object(4) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(strError)
            result(3) = cartItemCount
            result(4) = linkRedirect
            Return result

        End Function
        Public Function InsertCartFreeItemDiscountPercent(ByVal mmId As Integer, ByVal arrItemId As Object, ByVal arrQty As Object) As Object
            Dim linkRedirect As String = String.Empty
            Dim strError As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlFreeGift As String = String.Empty
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Try

                Dim sp As New SitePage()
                sp.Cart.DB = DB
                DB.BeginTransaction()

                Dim itemId As Integer = 0
                Dim qty As Integer = 0
                Dim cartItemIdFree As Integer = 0
                DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & orderId & " and MixMatchId =" & mmId & " and IsFreeItem=1; Update  StoreCartItem set MixMatchId=" & mmId & " where OrderId=" & orderId & " and IsFreeItem<>1 and (MixMatchId<1 or MixMatchId is null) and ItemId in(Select ItemId from MixMatchLine where MixMatchId=" & mmId & " and Value=0)")
                Dim TotalFreeAllowed As Integer = 0
                Dim FreeItemIds As String = String.Empty
                For i As Integer = 0 To arrItemId.Length - 1
                    itemId = CInt(arrItemId(i))
                    qty = CInt(arrQty(i))
                    If (itemId > 0 AndAlso qty > 0) Then
                        cartItemIdFree = sp.Cart.Add2Cart(itemId, Nothing, qty, 0, "Myself", "", "", 0, "", True, True, mmId)
                        TotalFreeAllowed += qty
                        If String.IsNullOrEmpty(FreeItemIds) Then
                            FreeItemIds = itemId
                        Else
                            FreeItemIds = "," & itemId
                        End If
                    End If
                Next
                Dim sqlUpdate As String = "Update StoreCartItem set TotalFreeAllowed=" & TotalFreeAllowed & ", FreeItemIds='" & FreeItemIds & "' where OrderId =" & orderId & " and IsFreeItem=1 and MixMatchId =" & mmId
                DB.ExecuteSQL(sqlUpdate)

                If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                sp.Cart.RecalculateOrderDetail("InsertCartFreeItemDiscountPercent")
                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If (countCartRow > Utility.ConfigData.MaxCartCount) Then '' must redirect  becauce render very slowly
                    linkRedirect = "/store/cart.aspx"
                Else
                    Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                    HttpContext.Current.Session("CartRender") = objCart
                    htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    htmlFreeGift = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-cart.ascx")
                    cartItemCount = sp.Cart.GetCartItemCount()
                    HttpContext.Current.Session.Remove("CartRender")
                End If
            Catch ex As Exception
                If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                strError = ex.Message
            End Try
            DB.Close()
            Dim result As Object() = New Object(5) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlFreeGift)
            result(3) = New HtmlString(strError)
            result(4) = cartItemCount
            result(5) = linkRedirect
            Return result

        End Function
        Public Function InsertCartFreeDiscountItem(ByVal MixMatchId As Integer, ByVal TotalFreeAllowed As Integer, ByVal FreeItemIds As String, ByVal ItemId As Integer, ByVal Qty As Integer) As Object
            Dim strError As String = String.Empty
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
            Dim CountFreeItem As Integer = 0
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Try

                Dim sp As New SitePage()
                sp.Cart.DB = DB
                DB.BeginTransaction()

                Dim QtyAvaiFree As Integer = 1
                Dim r As String = DB.ExecuteScalar("DELETE FROM StoreCartItem WHERE OrderId=" & orderId & " AND MixMatchId =" & MixMatchId & " AND IsFreeItem=1 AND ItemId = " & ItemId _
                    & ";  UPDATE StoreCartItem Set MixMatchId=" & MixMatchId & " where OrderId=" & orderId & " And IsFreeGift<>1 And AddType<>2 And IsFreeItem<>1 And (MixMatchId<1 Or MixMatchId Is NULL) And ItemId In (Select ItemId FROM MixMatchLine WHERE MixMatchId=" & MixMatchId & " And Value=0); (Select TotalFreeAllowed- SUM(Quantity) FROM StoreCartItem WHERE Orderid = " & orderId & " And MixMatchId = " & MixMatchId & " And IsFreeItem=1 GROUP BY TotalFreeAllowed)")

                If Not String.IsNullOrEmpty(r) Then
                    QtyAvaiFree = CInt(r)

                    If Qty > QtyAvaiFree Then
                        Qty = QtyAvaiFree
                    End If
                Else
                    If Qty > TotalFreeAllowed Then
                        Qty = TotalFreeAllowed
                    End If
                End If

                'Dim TotalFreeAllowed As Integer = 0
                'Dim FreeItemIds As String = String.Empty

                If QtyAvaiFree > 0 Then

                    If (ItemId > 0 AndAlso Qty > 0) Then
                        Dim CartItemId As Integer = sp.Cart.Add2Cart(ItemId, Nothing, Qty, 0, "Myself", "", "", 0, "", True, True, MixMatchId)
                        If CartItemId > 0 Then
                            'CountFreeItem = DB.ExecuteScalar("If (Select TotalFreeAllowed- SUM(Quantity) FROM StoreCartItem WHERE Orderid = " & orderId & " And MixMatchId = " & MixMatchId & " And IsFreeItem=1 GROUP BY TotalFreeAllowed) >= 0" _
                            '                                    & "UPDATE StoreCartItem Set TotalFreeAllowed=" & TotalFreeAllowed & ", FreeItemIds ='" & FreeItemIds & "' WHERE OrderId =" & orderId & " AND CartItemId=" & CartItemId & ";" _
                            '                                    & "SELECT SUM(Quantity) FROM StoreCartItem WHERE OrderId =" & orderId & " AND MixMatchid = " & MixMatchId & " AND IsFreeItem = 1" _
                            '                                & " ELSE" _
                            '                                    & " DELETE FROM StoreCartItem WHERE CartItemId = " & CartItemId)

                            CountFreeItem = DB.ExecuteScalar("UPDATE StoreCartItem Set TotalFreeAllowed=" & TotalFreeAllowed & ", FreeItemIds = " & IIf(String.IsNullOrEmpty(FreeItemIds), "(SELECT TOP 1 FreeItemIds FROM StoreCartItem WHERE OrderId =" & orderId & " AND MixMatchId = " & MixMatchId & " AND IsFreeItem = 0)", "'" & FreeItemIds & "'") & " WHERE CartItemId=" & CartItemId & ";" _
                                                               & "select isnull(sum(CountItem), 0) from (SELECT ISNULL(SUM(Quantity),0) - (a.Quantity * isnull(sum(b.DefaultSelectQty), 0)) as CountItem FROM StoreCartItem a left join MixMatchLine b on b.MixMatchId = a.MixMatchId and a.ItemId = b.ItemId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId WHERE a.OrderId =" & orderId & " AND a.MixMatchid = " & MixMatchId & " AND IsFreeItem = 1  and not exists (select 1 from MixMatchLine b where a.MixMatchId = b.MixMatchId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId and b.IsActive = 1) group by a.Quantity) a")
                        End If
                    Else
                        CountFreeItem = DB.ExecuteScalar("SELECT ISNULL(SUM(Quantity),0) - (a.Quantity * isnull(sum(b.DefaultSelectQty), 0))  FROM StoreCartItem a left join MixMatchLine b on b.MixMatchId = a.MixMatchId and a.ItemId = b.ItemId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId WHERE a.OrderId =" & orderId & " AND a.MixMatchid = " & MixMatchId & " AND IsFreeItem = 1  and not exists (select 1 from MixMatchLine b where a.MixMatchId = b.MixMatchId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId and b.IsActive = 1) group by a.Quantity having ISNULL(SUM(Quantity),0) - (sum(a.Quantity) * isnull(sum(b.DefaultSelectQty), 0)) > 0")
                    End If
                Else
                    CountFreeItem = DB.ExecuteScalar("SELECT ISNULL(SUM(Quantity),0) - (a.Quantity * isnull(sum(b.DefaultSelectQty), 0))  FROM StoreCartItem a left join MixMatchLine b on b.MixMatchId = a.MixMatchId and a.ItemId = b.ItemId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId WHERE a.OrderId =" & orderId & " AND a.MixMatchid = " & MixMatchId & " AND IsFreeItem = 1  and not exists (select 1 from MixMatchLine b where a.MixMatchId = b.MixMatchId and b.IsDefaultSelect = 1 and a.ItemId = b.ItemId and b.IsActive = 1) group by a.Quantity having ISNULL(SUM(Quantity),0) - (sum(a.Quantity) * isnull(sum(b.DefaultSelectQty), 0)) > 0")
                    'strError = "The number of free items exceeds our promotion."
                    strError = "error"
                End If
                If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                'sp.Cart.RecalculateOrderDetail("InsertCartFreeItemDiscountPercent")
            Catch ex As Exception
                If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                strError = ex.Message
            End Try
            DB.Close()
            Dim result As Object() = New Object(3) {}
            result(0) = New HtmlString(strError)
            result(1) = ItemId
            result(2) = TotalFreeAllowed - CountFreeItem
            result(3) = Qty
            Return result
        End Function
        Public Function AddCartFreeSample(ByVal arrId As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim sp As New SitePage()
            Dim isCheckLogin As Boolean = False

            If HttpContext.Current.Request.UrlReferrer.PathAndQuery.Contains("act=checkout") Then
                isCheckLogin = True
            End If

            If (memberId < 1 AndAlso isCheckLogin = True) Then
                linkRedirect = "/members/login.aspx"
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
            Else
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    sp.Cart.DB = DB
                    ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                    Dim Total As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
                    If sp.Cart Is Nothing Then
                        strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $0"
                    Else

                        If Total <= sp.Cart.SubTotalPuChasePoint Then
                            Dim QtySample As Integer = CInt(SysParam.GetValue("FreeSampleQty"))
                            'Check limit select free sample
                            If arrId.Length > QtySample Then
                                strError = "There is a maximum of " & QtySample & " samples per order. Please make sure you have selected only " & QtySample & " samples and deselect any additional samples. You can select more samples on your next order."
                            Else
                                DB.ExecuteScalar("delete storecartitem where orderid = " & orderId & " and IsFreeSample = 1")
                                For Each itemId As String In arrId
                                    If String.IsNullOrEmpty(itemId) Then
                                        Continue For
                                    End If
                                    sp.Cart.Add2Cart(CInt(itemId), Nothing, 1, Nothing, "IsFreeSample", Nothing, Nothing, Nothing, Nothing, False, True, Nothing)
                                Next
                            End If
                        Else
                            strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $" & sp.Cart.SubTotalPuChasePoint
                        End If
                    End If
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(strError)
            result(1) = linkRedirect
            Return result

        End Function

        Public Function RefeshCart() As Object
            Dim linkRedirect As String = String.Empty
            Dim isOK As Integer = 0

            Dim strError As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty
            Dim cartItemCount As Integer = 0

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim sp As New SitePage()
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                sp.Cart.RecalculateOrderDetail("RefeshCart")

                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If (countCartRow > Utility.ConfigData.MaxCartCount) Then 'Must redirect  becauce render very slowly
                    linkRedirect = "/store/cart.aspx"
                Else
                    'Update success
                    Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)

                    'Render cart HTML
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")

                    If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                        Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                        Dim r As Object()
                        If arr(0) <> "US" Then
                            r = ChangeCountryEstimateShipping(arr(0), "")
                            htmlSumarryBox = r(0).ToString()
                        ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                            r = CalEstimateShipping(arr(0), arr(1))
                            htmlEstimatePopup = r(0).ToString()
                            htmlSumarryBox = r(2).ToString()
                        Else
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        End If

                        If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                            If Not String.IsNullOrEmpty(strError) Then
                                strError &= "<br><br>"
                            End If
                            strError = r(1).ToString()
                        End If
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    cartItemCount = sp.Cart.GetCartItemCount()
                    HttpContext.Current.Session.Remove("CartRender")
                    isOK = 1
                End If

                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(6) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = New HtmlString(strError)
            result(4) = cartItemCount
            result(5) = linkRedirect
            result(6) = isOK
            Return result
        End Function

        Public Function AddFreeSamples(ByVal ItemId As Object) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim isOK As Integer = 0

            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlEstimatePopup As String = String.Empty

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim sp As New SitePage()
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                sp.Cart.DB = DB
                ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                Dim Total As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
                If sp.Cart Is Nothing Then
                    strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $0"
                Else
                    If Total <= sp.Cart.SubTotalPuChasePoint Then
                        Dim QtySample As Integer = CInt(SysParam.GetValue("FreeSampleQty"))
                        If CInt(DB.ExecuteScalar("SELECT COUNT(CartItemId) FROM StoreCartItem WHERE OrderId = " & orderId & " AND IsFreeSample = 1")) < QtySample Then
                            sp.Cart.Add2Cart(CInt(ItemId), Nothing, 1, Nothing, "IsFreeSample", Nothing, Nothing, Nothing, Nothing, False, True, Nothing)
                        Else
                            strError = "Limit " & QtySample & " free samples for each order, you have to remove free samples in your cart if you want to choose again."
                        End If
                    Else
                        strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $" & sp.Cart.SubTotalPuChasePoint
                    End If
                End If

                If String.IsNullOrEmpty(strError) Then
                    Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                    If (countCartRow > Utility.ConfigData.MaxCartCount) Then 'Must redirect  becauce render very slowly
                        linkRedirect = "/store/cart.aspx"
                    Else
                        'Update success
                        Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)

                        'Render cart HTML
                        HttpContext.Current.Session("CartRender") = sp.Cart
                        htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")

                        If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                            Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                            Dim r As Object()
                            If arr(0) <> "US" Then
                                r = ChangeCountryEstimateShipping(arr(0), "")
                                htmlSumarryBox = r(0).ToString()
                            ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                                r = CalEstimateShipping(arr(0), arr(1))
                                htmlEstimatePopup = r(0).ToString()
                                htmlSumarryBox = r(2).ToString()
                            Else
                                htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                            End If

                            If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                                If Not String.IsNullOrEmpty(strError) Then
                                    strError &= "<br><br>"
                                End If
                                strError = r(1).ToString()
                            End If
                        Else
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        End If

                        HttpContext.Current.Session.Remove("CartRender")
                        isOK = 1
                    End If
                End If

                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(6) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = New HtmlString(strError)
            result(4) = linkRedirect
            result(5) = isOK
            result(6) = ItemId
            Return result
        End Function

        Public Function DeleteFreeSamples(ByVal ItemId As Object) As Object
            Dim strError As String = String.Empty
            Dim isOK As Integer = 0
            Dim CartItemId As Integer = 0
            Dim htmlEstimatePopup As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim sp As New SitePage()
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                CartItemId = CInt(DB.ExecuteScalar("SELECT CartItemId FROM StoreCartItem WHERE OrderId = " & orderId & " AND IsFreeSample = 1 AND ItemId = " & ItemId & " ; DELETE StoreCartItem WHERE OrderId = " & orderId & " AND IsFreeSample = 1 AND ItemId = " & ItemId))

                If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                    Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                    Dim r As Object()
                    If arr(0) <> "US" Then
                        r = ChangeCountryEstimateShipping(arr(0), "")
                        htmlSumarryBox = r(0).ToString()
                    ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                        r = CalEstimateShipping(arr(0), arr(1))
                        htmlEstimatePopup = r(0).ToString()
                        htmlSumarryBox = r(2).ToString()
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                        If Not String.IsNullOrEmpty(strError) Then
                            strError &= "<br><br>"
                        End If
                        strError = r(1).ToString()
                    End If
                Else
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                End If
                isOK = 1
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(5) {}

            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = isOK
            result(4) = CartItemId
            result(5) = ItemId
            Return result
        End Function

        'Public Function GoFreeSamplePage() As Object

        '    Dim strError As String = String.Empty
        '    Dim linkRedirect As String = String.Empty
        '    Dim memberId As Integer = Common.GetCurrentMemberId()
        '    Dim sp As New SitePage()
        '    If (memberId < 1) Then
        '        linkRedirect = "/members/login.aspx"
        '        sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
        '    Else
        '        Dim orderId As Integer = 0
        '        Try
        '            If Not HttpContext.Current.Session Is Nothing Then
        '                orderId = HttpContext.Current.Session("OrderId")
        '            End If
        '            If orderId < 1 Then
        '                orderId = Utility.Common.GetOrderIdFromCartCookie()
        '            End If

        '            Dim Total As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
        '            If sp.Cart Is Nothing Then
        '                strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $0"
        '            Else
        '                If sp.Cart.SubTotalPuChasePoint < Total Then
        '                    strError = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $" & sp.Cart.SubTotalPuChasePoint
        '                End If


        '            End If
        '        Catch ex As Exception
        '            strError = ex.Message
        '        End Try
        '    End If
        '    If String.IsNullOrEmpty(strError) AndAlso String.IsNullOrEmpty(linkRedirect) Then
        '        linkRedirect = "/store/free-sample.aspx?act=checkout"
        '    End If
        '    Dim result As Object() = New Object(1) {}
        '    result(0) = New HtmlString(strError)
        '    result(1) = linkRedirect
        '    Return result

        'End Function

        Public Function ChangeItemGroup(ByVal itemGroupId As Object, ByVal hidListOptionId As Object, ByVal lstChoiceId As Object, ByVal choiceId As Object) As Object
            Dim strError As String = String.Empty
            Dim itemName As String = String.Empty
            Dim sku As String = String.Empty
            Dim htmlImageDetail As String = String.Empty
            Dim htmlVideo As String = String.Empty
            Dim htmlPriceBox As String = String.Empty
            Dim htmlDescription As String = String.Empty
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                Dim arrOption() As String = hidListOptionId.Split(",")
                If (arrOption.Length > 0) Then
                    For Each op As String In arrOption
                        If Not String.IsNullOrEmpty(op) Then
                            lstChoiceId = lstChoiceId.Replace(";" & op & ",", ",")
                        End If
                    Next
                End If
                Dim itemId As Integer = StoreItemRow.GetItemIdByChoices(DB, itemGroupId, lstChoiceId.Replace(";", ","))
                If (itemId < 1) Then
                    Dim lstChoiceDefault As String = choiceId.Replace(";", ",")
                    itemId = StoreItemRow.GetItemIdByChoices(DB, itemGroupId, lstChoiceDefault)
                End If
                If (itemId > 0) Then
                    Dim objSelectItem As StoreItemRow = StoreItemRow.GetRow(DB, itemId)
                    If (Not objSelectItem Is Nothing) Then
                        Dim sp As New SitePage()
                        HttpContext.Current.Session("itemRender") = objSelectItem
                        Dim strPriceDesc As String = IIf(objSelectItem.PriceDesc <> Nothing, " - " & objSelectItem.PriceDesc, "")
                        Dim measure As String = sp.ShowMeasurement(objSelectItem.PriceDesc, objSelectItem.Measurement)
                        strPriceDesc &= IIf(measure.Length > 0, " (" & measure & ")", "")
                        itemName = objSelectItem.ItemName & strPriceDesc
                        sku = objSelectItem.SKU
                        Dim dtVideo As DataTable = StoreItemVideoRow.ListByItemId(DB, objSelectItem.ItemId)
                        HttpContext.Current.Session("itemVideoRender") = dtVideo
                        htmlImageDetail = Utility.Common.RenderUserControl("~/controls/product/image-item-detail.ascx")
                        htmlVideo = Utility.Common.RenderUserControl("~/controls/product/video-item-detail.ascx")
                        htmlPriceBox = Utility.Common.RenderUserControl("~/controls/product/price-box-item-detail.ascx")
                        htmlDescription = Utility.Common.RenderUserControl("~/controls/product/item-description.ascx")
                        HttpContext.Current.Session("itemRender") = Nothing
                        HttpContext.Current.Session("itemVideoRender") = Nothing
                    End If
                End If
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try
            Dim result As Object() = New Object(6) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(itemName)
            result(2) = New HtmlString(htmlImageDetail)
            result(3) = New HtmlString(htmlVideo)
            result(4) = New HtmlString(htmlPriceBox)
            result(5) = New HtmlString(htmlDescription)
            result(6) = New HtmlString(sku)
            Return result
        End Function


#Region "Submit order"

        Public Function ChangeShippingMethod(ByVal methodId As Object, ByVal isRenderListCart As Object) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlListCart As String = String.Empty
            Dim htmlShippingOption As String = String.Empty
            Dim htmlFreightShippingOption As String = String.Empty

            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    sp.Cart.DB = DB

                    HttpContext.Current.Session.Remove("GetFedExRate")
                    Dim o As StoreOrderRow = sp.Cart.Order
                    If (o.ShipToCountry = "US" OrElse (o.IsSameAddress AndAlso o.BillToCountry = "US")) AndAlso Not sp.Cart.CheckShippingSpecialUS Then
                        Dim selectedMethod As String = methodId.ToString()
                        If selectedMethod = Nothing Then
                            selectedMethod = Utility.Common.DefaultShippingId
                        End If
                        Utility.Common.ResetOrderShippingMethod(selectedMethod, o.OrderId, DB)
                        o.CarrierType = methodId
                    Else
                        o.CarrierType = Utility.Common.USPSPriorityShippingId
                    End If

                    StoreOrderRow.ResetCartItemHandlingFee(DB, o.OrderId)
                    sp.RecalculateSignatureConfirmation(DB, o)

                    o.ShipmentInsured = 0
                    sp.Cart.RecalculateOrderDetail(Utility.Common.SystemFunction.ChangeOrderAddress.ToString())
                    If (Common.UPSNextDayShippingId() = o.CarrierType Or Common.UPS2DayShippingId() = o.CarrierType) AndAlso sp.Cart.Order.IsFlammableCartItem = Common.FlammableCart.BlockedHazMat Then
                        strError = Resources.Alert.CartItemBlockedHazMat
                    End If

                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    htmlShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-option.ascx")
                    htmlFreightShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-freight-option.ascx")

                    If isRenderListCart Then
                        htmlListCart = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                    End If
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(6) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlShippingOption)
            result(2) = New HtmlString(htmlFreightShippingOption)
            result(3) = New HtmlString(htmlSumarryBox)
            result(4) = New HtmlString(htmlListCart)
            result(5) = linkRedirect
            If methodId = Utility.Common.PickupShippingId Then
                result(6) = 1
            Else
                result(6) = 0
            End If
            Return result
        End Function
        Public Function CheckShippingInsurance(ByVal isSelect As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    sp.Cart.Order.ShipmentInsured = IIf(isSelect, 1, 0)
                    sp.Cart.Order.Update()
                    sp.Cart._RecalculateShipping("")
                    sp.Cart.Order.TotalSpecialHandlingFee = DB.ExecuteScalar("select coalesce(SUM(SpecialHandlingFee*Quantity),0) from StoreCartItem where OrderId=" & sp.Cart.Order.OrderId & "  and Type<>'carrier'")
                    sp.Cart.Order.Total = sp.Cart.Order.SubTotal + sp.Cart.Order.Shipping + sp.Cart.Order.Tax + sp.Cart.Order.TotalSpecialHandlingFee
                    sp.Cart.Order.Update()
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = linkRedirect
            Return result
        End Function
        Public Function CheckShippingSignature(ByVal isSelect As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    Dim Cart As ShoppingCart = sp.Cart
                    If isSelect Then
                        Dim Signature As Double = 0
                        Dim Sql As String = "select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType)  " & vbCrLf &
                                "where orderid = " & orderId & " and  type = 'item'  and Code in(" & Utility.Common.USShippingCode & ")"
                        Signature = ShipmentMethod.GetValue(DB.ExecuteScalar(Sql), Utility.Common.ShipmentValue.Signature)
                        Cart.Order.SignatureDeclineCommnent = ""
                        Cart.Order.IsSignatureConfirmation = True
                        Cart.Order.SignatureConfirmation = Signature
                    Else
                        Cart.Order.SignatureDeclineCommnent = "Without Signature Confirmation, FedEx driver will drop off your package(s) by your front door. We will not take any responsibility in case your package(s) are stolen."
                        Cart.Order.IsSignatureConfirmation = False
                        Cart.Order.SignatureConfirmation = 0

                    End If
                    Cart._RecalculateShipping("")
                    Cart.RecalculateOrderUpdate()

                    HttpContext.Current.Session("CartRender") = Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = linkRedirect
            Return result
        End Function
        Public Function AddCoupon(ByVal couponCode As String, ByVal isRenderListCart As Object) As Object
            couponCode = couponCode.Trim()
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlListCoupon As String = String.Empty
            Dim htmlListCart As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    System.Web.HttpContext.Current.Session("PromotionAddID") = Nothing
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    Dim Pro As StorePromotionRow = StorePromotionRow.GetRow(DB, couponCode)
                    Dim strStatus As String = String.Empty
                    sp.Cart.DB = DB
                    CheckCouponCode(DB, memberId, sp.Cart, Pro, strError, strStatus)
                    Dim strMsg As String = ""
                    Dim isAdd As Boolean = False
                    If strStatus = "IsProductTrue" Then
                        isAdd = True
                        strMsg = sp.Cart.AddProductCoupon(couponCode)
                        If strMsg = "" Or strMsg.Contains("Although you've entered a valid promo code") Then
                            strError = "<span class='CouponRed'>Although you've entered a valid promo code " & couponCode & ", your order does not currently meet the code's usage criteria.</span>"
                        ElseIf strMsg.Contains("The promotion code you entered is only valid from") Or strMsg.Contains("The promotion code you entered does not exist") Or strMsg.Contains("The promotion you entered is no longer active on the website") Or strMsg.Contains("This promotion has a") Then
                            If strMsg.Contains("This promotion has a minimum purchase amount of") Or strMsg.Contains("This promotion has a maximum purchase amount of") Then
                                Dim subtotal As Double = 0
                                If Not String.IsNullOrEmpty(couponCode) Then
                                    subtotal = DB.ExecuteScalar("Select Total from StoreCartItem where OrderId=" & orderId & " and PromotionID=(Select PromotionId from StorePromotion where PromotionCode='" & couponCode & "')")
                                End If

                                Dim subtotalMsg As String = String.Format(Resources.Alert.ItemSubTotal, subtotal)
                                strMsg = strMsg & "." & subtotalMsg
                            End If

                            strError = strMsg
                            DB.ExecuteSQL("update storecartitem set promotionid = null, CouponPrice = 0 where orderid = " & orderId & " and Promotionid=" & Pro.PromotionId)
                        End If

                    ElseIf strStatus = "IsOrderTrue" Then
                        isAdd = True
                        Dim sErrorMsg As String = String.Empty
                        Dim IsPromotionValid As Boolean = StorePromotionRow.ValidatePromotion(DB, Nothing, couponCode, sErrorMsg, sp.Cart.Order.SubTotal + sp.Cart.Order.Discount)
                        If IsPromotionValid Then
                            sp.Cart.AddOrderCoupon(couponCode)
                        Else
                            If sErrorMsg.Contains("This promotion has a minimum") Or sErrorMsg.Contains("This promotion has a maximum") Or sErrorMsg.Contains("The promotion code you entered") Or sErrorMsg.Contains("The promotion code you entered is only valid from") Then
                                If sErrorMsg.Contains("This promotion has a minimum") Or sErrorMsg.Contains("This promotion has a maximum") Then
                                    Dim subtotalMsg As String = String.Format(Resources.Alert.OrderSubTotal, sp.Cart.Order.SubTotal)
                                    sErrorMsg = sErrorMsg & "." & subtotalMsg
                                End If
                            End If
                            strError = sErrorMsg
                        End If
                    End If

                    If isAdd Then
                        Dim Cart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                        Cart.RecalculateOrderDetail("AddCoupon")
                        System.Web.HttpContext.Current.Session("PromotionAddID") = Nothing
                        HttpContext.Current.Session("CartRender") = Cart
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        htmlListCoupon = Utility.Common.RenderUserControl("~/controls/checkout/coupon-list.ascx")
                        If isRenderListCart Then
                            htmlListCart = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                        End If
                        HttpContext.Current.Session.Remove("CartRender")
                    End If
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(4) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlListCoupon)
            result(3) = New HtmlString(htmlListCart)
            result(4) = linkRedirect
            Return result
        End Function
        Private Sub CheckCouponCode(ByVal DB As Database, ByVal memberId As Integer, ByVal Cart As ShoppingCart, ByVal Pro As StorePromotionRow, ByRef returnMsg As String, ByRef returnStatus As String)

            Dim SQL As String = ""
            Dim Count As Integer
            If Pro.PromotionCode <> "" Then
                If Pro.IsProductCoupon = True Then
                    Count = DB.ExecuteScalar("select Count(si.PromotionId) from StoreCartItem si inner join StorePromotion sp on si.PromotionID = sp.PromotionId where sp.IsProductCoupon = 1 and OrderId = " & Cart.Order.OrderId & " and sp.PromotionId = " & Pro.PromotionId)
                    If Count > 0 Then
                        returnMsg = "<span class='lnpadBottom10 CouponRed'>" & "Your coupon code had been used." & "</span>"
                        returnStatus = "False"
                    Else
                        returnStatus = "IsProductTrue"
                        If Pro.IsOneUse = True Then
                            SQL = "select storeorder.orderid from storeorder,storecartitem where storeorder.orderid=storecartitem.orderid "
                            SQL = SQL & " and PromotionID is not null and PromotionID <> 0 and orderno is not null  and memberid='" & memberId & "' and promotionid='" & Pro.PromotionId & "'"
                            Dim dt As DataTable = DB.GetDataTable(SQL)
                            If dt.Rows.Count > 0 Then
                                returnMsg = "Your coupon code is single use."
                                returnStatus = "False"
                            End If
                        End If
                    End If
                Else
                    Count = DB.ExecuteScalar("select Count(so.PromotionCode) from StoreOrder so inner join StorePromotion sp on so.PromotionCode = sp.PromotionCode where sp.IsProductCoupon = 0 and OrderId = " & Cart.Order.OrderId & " and sp.PromotionId = " & Pro.PromotionId)
                    If Count > 0 Then
                        returnMsg = "Your coupon code had been used."
                        returnStatus = "False"
                    Else
                        returnStatus = "IsOrderTrue"
                        If Pro.IsOneUse = True Then
                            SQL = "SELECT si.OrderId FROM StoreOrder si "
                            SQL = SQL & " where PromotionCode is not null and orderno is not null  and memberid='" & memberId & "' and promotioncode='" & Pro.PromotionCode & "'"
                            Dim dt As DataTable = DB.GetDataTable(SQL)
                            If dt.Rows.Count > 0 Then
                                returnMsg = "Your coupon code is single use."
                                returnStatus = "False"
                            End If
                        End If
                    End If
                End If
            Else
                returnMsg = "The coupon code you entered does not exist."
                returnStatus = "False"
            End If
        End Sub
        Public Function DeleteCoupon(ByVal couponCode As String, ByVal isRenderListCart As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlListCoupon As String = String.Empty
            Dim htmlListCart As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    sp.Cart.DB = DB
                    sp.Cart.RemoveCouponAndRecalculateAllOrderValues(couponCode, sp.Cart.Order)
                    Dim lstCartItem As StoreCartItemCollection = StoreCartItemRow.GetCartItemByProductCoupon(DB, orderId, couponCode)
                    sp.Cart.RefreshCartItemInforFromItem(lstCartItem, DB)
                    sp.Cart.RecalculateOrderDetail("Command.DeleteCoupon")
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    htmlListCoupon = Utility.Common.RenderUserControl("~/controls/checkout/coupon-list.ascx")
                    If isRenderListCart Then
                        htmlListCart = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                    End If
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(4) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlListCoupon)
            result(3) = New HtmlString(htmlListCart)
            result(4) = linkRedirect
            Return result
        End Function
        Public Function PaymentPointForOrder(ByVal pointValue As Object, ByVal pointMsg As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlRewardsPoints As String = String.Empty
            Dim redeenablePoint As Integer = 0
            Dim sp As New SitePage()

            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    sp.Cart.GetPurchasePoint(sp.Cart.Order, pointValue, pointMsg)
                    sp.Cart.RecalculateOrderDetail("PaymentPointForOrder")
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    htmlRewardsPoints = Utility.Common.RenderUserControl("~/controls/checkout/reward-point-summary.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(4) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlRewardsPoints)
            result(3) = linkRedirect
            result(4) = redeenablePoint
            Return result
        End Function

        Public Function RequestOversizeFee(ByVal checked As Boolean, ByVal type As Integer) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    Dim isLiftGate As Boolean = False
                    Dim IsScheduleDelivery As Boolean = False
                    Dim IsInsideDelivery As Boolean = False
                    StoreOrderRow.GetFreightShippingOption(sp.Cart.Order.OrderId, isLiftGate, IsScheduleDelivery, IsInsideDelivery)

                    Dim strLiftGate, strScheduleDelivery, strInsideDelivery, FreightComment As String
                    FreightComment = ""
                    strLiftGate = "You requested a liftgate delivery. A single surcharge of " & FormatCurrency(SysParam.GetValue("LiftGateCharge")) & " was added to your order."
                    Dim ScheduleDeliverysurcharge As Double = SysParam.GetValue("ScheduleDeliveryCharge")
                    strScheduleDelivery = "You requested a scheduled delivery. A single surcharge of " & FormatCurrency(ScheduleDeliverysurcharge) & " was added to your order."
                    strInsideDelivery = "You requested an inside delivery. A single surcharge of " & FormatCurrency(SysParam.GetValue("InsideDeliveryService")) & " was added to your order."
                    If sp.Cart.Order.Comments Is Nothing Then
                        sp.Cart.Order.Comments = String.Empty
                    End If
                    If type = "1" Then
                        If checked Then
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strLiftGate, "") & strLiftGate & "|"
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsLiftGate=1 where IsOversize=1 and OrderId=" & orderId)
                        Else
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strLiftGate, "")
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsLiftGate=0 where IsOversize=1 and OrderId=" & orderId)
                        End If
                    ElseIf type = 2 Then
                        If checked Then
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strScheduleDelivery, "") & strScheduleDelivery & "|"
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsScheduleDelivery=1 where IsOversize=1 and OrderId=" & orderId)
                        Else
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strScheduleDelivery, "")
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsScheduleDelivery=0 where IsOversize=1 and OrderId=" & orderId)
                        End If
                    ElseIf type = 3 Then
                        If checked Then
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strInsideDelivery, "") & strInsideDelivery & "|"
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsInsideDelivery=1 where IsOversize=1 and OrderId=" & orderId)
                        Else
                            sp.Cart.Order.Comments = sp.Cart.Order.Comments.Replace(strInsideDelivery, "")
                            sp.Cart.Order.Update()
                            DB.ExecuteSQL("Update StoreCartItem set IsInsideDelivery=0 where IsOversize=1 and OrderId=" & orderId)
                        End If
                    End If
                    ''Dim Cart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                    sp.Cart.RecalculateOrderDetail("RequestOversizeFee")
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = linkRedirect
            Return result
        End Function
        Public Function GetListCartItem() As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlCartList As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlCartList = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlCartList)
            result(2) = linkRedirect
            Return result


        End Function
        Public Function ChangeOrderSameAddress(ByVal isSame As Object, ByVal isRenderListCart As Object, ByVal lstCouponInValid As Object) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlShippingList As String = String.Empty
            Dim htmlShippingOption As String = String.Empty
            Dim htmlFreightShippingOption As String = String.Empty
            Dim htmlCartList As String = String.Empty
            Dim htmlListShippingAddress As String = String.Empty
            Dim htmlListCoupon As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    Dim oldShippingMthod As Integer = sp.Cart.Order.CarrierType
                    sp.Cart.Order.IsSameAddress = CBool(isSame)
                    If sp.Cart.Order.IsSameAddress Then
                        ''delete coupon in valid
                        If Not String.IsNullOrEmpty(lstCouponInValid) Then
                            Dim arrCoupon As String() = lstCouponInValid.ToString().Split(",")
                            If (arrCoupon.Length > 0) Then
                                For Each couponCode As String In arrCoupon
                                    If Not (String.IsNullOrEmpty(couponCode)) Then
                                        sp.Cart.RemoveCouponAndRecalculateAllOrderValues(couponCode, sp.Cart.Order)
                                    End If
                                Next
                            End If
                        End If

                        Web.HttpContext.Current.Session("GetFedExRate") = Nothing
                        '' Dim defaultShipingID As Integer = DB.ExecuteScalar("Select AddressId from MemberAddress where AddressType='Shipping' and MemberId=" & memberId)


                        ''get current billing 
                        Dim currentBillingAddress As MemberAddressRow = MemberAddressRow.GetOrderAddressByType(DB, sp.Cart.Order.OrderId, Utility.Common.MemberAddressType.Billing.ToString())

                        sp.Cart.Order.ShipToSalonName = currentBillingAddress.Company
                        sp.Cart.Order.ShipToName = currentBillingAddress.FirstName
                        sp.Cart.Order.ShipToName2 = currentBillingAddress.LastName
                        sp.Cart.Order.ShipToAddress = currentBillingAddress.Address1
                        sp.Cart.Order.ShipToAddress2 = currentBillingAddress.Address2
                        sp.Cart.Order.ShipToCity = currentBillingAddress.City
                        sp.Cart.Order.ShipToCountry = currentBillingAddress.Country
                        sp.Cart.Order.ShipToCounty = currentBillingAddress.State
                        sp.Cart.Order.ShipToFax = currentBillingAddress.Fax
                        sp.Cart.Order.ShipToPhone = currentBillingAddress.Phone
                        sp.Cart.Order.ShipToPhoneExt = currentBillingAddress.PhoneExt
                        sp.Cart.Order.ShipToZipcode = currentBillingAddress.Zip

                        Dim billingType As String = currentBillingAddress.AddressType
                        Dim shippingAddress As MemberAddressRow = Nothing
                        If billingType = Utility.Common.MemberAddressType.Billing.ToString() Then ''default billing address
                            shippingAddress = MemberAddressRow.GetAddressByType(DB, memberId, Utility.Common.MemberAddressType.Shipping.ToString()) ''update lai default shipping address
                            sp.Cart.Order.ShippingAddressId = shippingAddress.AddressId
                            shippingAddress.Company = sp.Cart.Order.ShipToSalonName
                            shippingAddress.FirstName = sp.Cart.Order.ShipToName
                            shippingAddress.LastName = sp.Cart.Order.ShipToName2
                            shippingAddress.Address1 = sp.Cart.Order.ShipToAddress
                            shippingAddress.Address2 = sp.Cart.Order.ShipToAddress2
                            shippingAddress.City = sp.Cart.Order.ShipToCity
                            shippingAddress.Country = sp.Cart.Order.ShipToCountry
                            shippingAddress.State = sp.Cart.Order.ShipToCounty
                            shippingAddress.Fax = sp.Cart.Order.ShipToFax
                            shippingAddress.Phone = sp.Cart.Order.ShipToPhone
                            shippingAddress.PhoneExt = sp.Cart.Order.ShipToPhoneExt
                            shippingAddress.Zip = sp.Cart.Order.ShipToZipcode
                            shippingAddress.DB = DB
                            shippingAddress.Update()
                        Else
                            sp.Cart.Order.ShippingAddressId = sp.Cart.Order.BillingAddressId
                        End If
                        Dim isUSAddress As Boolean = Utility.Common.IsUSAddress(currentBillingAddress)


                        If Not (isUSAddress) Then

                            sp.Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId
                            Dim SqlSpecialHandlingFee As String = String.Empty
                            SqlSpecialHandlingFee = "Update StoreCartItem set SpecialHandlingFee = 0 where OrderId=" & sp.Cart.Order.OrderId & "  and Type<>'carrier'; Update StoreOrder set TotalSpecialHandlingFee=0 where OrderId=" & sp.Cart.Order.OrderId
                            DB.ExecuteSQL(SqlSpecialHandlingFee)
                        Else
                            If (sp.Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId) Then
                                sp.Cart.Order.CarrierType = Utility.Common.DefaultShippingId
                            End If
                        End If

                        If oldShippingMthod <> sp.Cart.Order.CarrierType Then
                            Utility.Common.ResetOrderShippingMethod(sp.Cart.Order.CarrierType, sp.Cart.Order.OrderId, DB)
                        End If
                        If isUSAddress Then
                            sp.CheckAddressType(DB, sp.Cart.Order)
                            HttpContext.Current.Session.Remove("CheckResidential")
                        End If
                        sp.Cart.RecalculateOrderSignatureConfirmation(sp.Cart.Order, isUSAddress)
                        StoreOrderRow.ResetCartItemHandlingFee(DB, sp.Cart.Order.OrderId)
                    End If

                    sp.Cart.RecalculateOrderDetail(Utility.Common.SystemFunction.ChangeOrderAddress)
                    DB.ExecuteSQL("Update Member set IsSameDefaultAddress=" & (IIf(sp.Cart.Order.IsSameAddress = True, 1, 0)) & " where MemberId=" & memberId)
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    If sp.Cart.Order.IsSameAddress Then

                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        htmlShippingList = Utility.Common.RenderUserControl("~/controls/checkout/shipping-list.ascx", False)
                        htmlFreightShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-freight-option.ascx")
                        htmlShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-option.ascx")
                        htmlListCoupon = Utility.Common.RenderUserControl("~/controls/checkout/coupon-list.ascx")
                        If isRenderListCart Then
                            htmlCartList = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                        End If

                    Else
                        DB.ExecuteSQL("Update StoreOrder set ShippingAddressId=0 where OrderId=" & sp.Cart.Order.OrderId)

                        HttpContext.Current.Session("addressTypeRender") = Utility.Common.MemberAddressType.Shipping.ToString()
                        htmlListShippingAddress = Utility.Common.RenderUserControl("~/controls/checkout/list-member-address.ascx")
                        HttpContext.Current.Session("addressTypeRender") = Nothing

                    End If
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(9) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlShippingList)
            result(3) = New HtmlString(htmlShippingOption)
            result(4) = New HtmlString(htmlFreightShippingOption)
            result(5) = New HtmlString(htmlCartList)
            result(6) = New HtmlString(htmlListShippingAddress)
            result(7) = linkRedirect
            result(8) = isSame
            result(9) = New HtmlString(htmlListCoupon)
            Return result
        End Function
        Public Function ChangeOrderShippingAddress(ByVal addressId As Object, ByVal isRenderListCart As Object, ByVal lstCouponInValid As Object) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlShippingList As String = String.Empty
            Dim htmlShippingOption As String = String.Empty
            Dim htmlFreightShippingOption As String = String.Empty
            Dim htmlCartList As String = String.Empty
            Dim billingAddressId As Integer = 0
            Dim shippingAddressId As Integer = 0
            Dim isSameAddress As Boolean = False
            Dim htmlListCoupon As String = String.Empty

            HttpContext.Current.Session.Remove("GetFedExRate")
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                    Dim memberAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, addressId)
                    If memberAddress Is Nothing Then
                        strError = "Address is not valid"
                    Else
                        If memberAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() AndAlso memberAddress.AddressId = sp.Cart.Order.BillingAddressId Then
                            sp.Cart.Order.IsSameAddress = True
                        End If
                        Dim oldShippingMthod As Integer = sp.Cart.Order.CarrierType
                        sp.Cart.Order.ShippingAddressId = addressId
                        sp.Cart.Order.ShipToSalonName = memberAddress.Company
                        sp.Cart.Order.ShipToName = memberAddress.FirstName
                        sp.Cart.Order.ShipToName2 = memberAddress.LastName
                        sp.Cart.Order.ShipToAddress = memberAddress.Address1
                        sp.Cart.Order.ShipToAddress2 = memberAddress.Address2
                        sp.Cart.Order.ShipToCity = memberAddress.City
                        sp.Cart.Order.ShipToCountry = memberAddress.Country
                        sp.Cart.Order.ShipToCounty = memberAddress.State
                        sp.Cart.Order.ShipToFax = memberAddress.Fax
                        sp.Cart.Order.ShipToPhone = memberAddress.Phone
                        sp.Cart.Order.ShipToPhoneExt = memberAddress.PhoneExt
                        sp.Cart.Order.ShipToZipcode = memberAddress.Zip
                        Dim isUSAddress As Boolean = Utility.Common.IsUSAddress(memberAddress)
                        If isUSAddress Then
                            If (sp.Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId) Then
                                sp.Cart.Order.CarrierType = Utility.Common.DefaultShippingId
                            End If
                        Else
                            If Utility.Common.IsCompleteIntAddress(memberAddress) Then
                                sp.Cart.Order.ShipToCounty = memberAddress.Region
                                sp.Cart.Order.IsSameAddress = True
                            End If
                            sp.Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId
                            Dim SqlSpecialHandlingFee As String = "Update StoreCartItem set SpecialHandlingFee = 0 where OrderId=" & sp.Cart.Order.OrderId & "  and Type<>'carrier'; Update StoreOrder set TotalSpecialHandlingFee=0 where OrderId=" & sp.Cart.Order.OrderId
                            DB.ExecuteSQL(SqlSpecialHandlingFee)
                        End If
                        If oldShippingMthod <> sp.Cart.Order.CarrierType Then
                            Utility.Common.ResetOrderShippingMethod(sp.Cart.Order.CarrierType, sp.Cart.Order.OrderId, DB)
                        End If
                        If isUSAddress Then
                            sp.CheckAddressType(DB, sp.Cart.Order)
                            HttpContext.Current.Session.Remove("CheckResidential")
                        End If
                        ''delete coupon in valid
                        If Not String.IsNullOrEmpty(lstCouponInValid) Then
                            Dim arrCoupon As String() = lstCouponInValid.ToString().Split(",")
                            If (arrCoupon.Length > 0) Then
                                For Each couponCode As String In arrCoupon
                                    If Not (String.IsNullOrEmpty(couponCode)) Then
                                        sp.Cart.RemoveCouponAndRecalculateAllOrderValues(couponCode, sp.Cart.Order)
                                    End If
                                Next
                            End If
                        End If

                        StoreOrderRow.ResetCartItemHandlingFee(DB, sp.Cart.Order.OrderId)
                        sp.Cart.RecalculateOrderSignatureConfirmation(sp.Cart.Order, isUSAddress)
                        sp.Cart.RecalculateOrderDetail(Utility.Common.SystemFunction.ChangeOrderAddress)
                        HttpContext.Current.Session("CartRender") = sp.Cart
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")

                        htmlShippingList = Utility.Common.RenderUserControl("~/controls/checkout/shipping-list.ascx", False)
                        htmlFreightShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-freight-option.ascx")
                        htmlShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-option.ascx")
                        htmlListCoupon = Utility.Common.RenderUserControl("~/controls/checkout/coupon-list.ascx")
                        If isRenderListCart Then
                            htmlCartList = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                        End If
                        HttpContext.Current.Session.Remove("CartRender")
                    End If
                    billingAddressId = sp.Cart.Order.BillingAddressId
                    shippingAddressId = sp.Cart.Order.ShippingAddressId
                    isSameAddress = sp.Cart.Order.IsSameAddress
                    DB.ExecuteSQL("Update Member set IsSameDefaultAddress=" & (IIf(isSameAddress = True, 1, 0)) & " where MemberId=" & memberId)
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(10) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlShippingList)
            result(3) = New HtmlString(htmlShippingOption)
            result(4) = New HtmlString(htmlFreightShippingOption)
            result(5) = New HtmlString(htmlCartList)
            result(6) = linkRedirect
            result(7) = billingAddressId
            result(8) = shippingAddressId
            result(9) = isSameAddress
            result(10) = New HtmlString(htmlListCoupon)
            Return result
        End Function

        Public Function ChangeOrderBillingAddress(ByVal addressId As Object, ByVal isRenderListCart As Object, ByVal lstCouponInValid As Object) As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim htmlShippingList As String = String.Empty
            Dim htmlShippingOption As String = String.Empty
            Dim htmlFreightShippingOption As String = String.Empty
            Dim htmlCartList As String = String.Empty
            Dim htmlListShippingAddress As String = String.Empty
            Dim isSameAddress As Boolean
            Dim billingAddressId As Integer = 0
            Dim shippingAddressId As Integer = 0
            Dim isUSAddress As Boolean = False
            Dim IsCompleteIntAddress As Boolean = False
            Dim htmlListCoupon As String = String.Empty
            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)


                    Dim Cart As New ShoppingCart(DB)
                    Dim selectBillingAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, addressId)
                    If selectBillingAddress Is Nothing Then
                        strError = "Address is not valid"
                    Else
                        Dim oldShippingMethod As Integer = Cart.Order.CarrierType
                        isUSAddress = Utility.Common.IsUSAddress(selectBillingAddress)
                        Cart.Order.BillingAddressId = addressId
                        Cart.Order.BillToSalonName = selectBillingAddress.Company
                        Cart.Order.BillToName = selectBillingAddress.FirstName
                        Cart.Order.BillToName2 = selectBillingAddress.LastName
                        Cart.Order.BillToAddress = selectBillingAddress.Address1
                        Cart.Order.BillToAddress2 = selectBillingAddress.Address2
                        Cart.Order.BillToCity = selectBillingAddress.City
                        Cart.Order.BillToCountry = selectBillingAddress.Country
                        Cart.Order.BillToFax = selectBillingAddress.Fax
                        Cart.Order.BillToPhone = selectBillingAddress.Phone
                        Cart.Order.BillToPhoneExt = selectBillingAddress.PhoneExt
                        Cart.Order.BillToZipcode = selectBillingAddress.Zip
                        If selectBillingAddress.Country = "US" Then
                            Cart.Order.BillToCounty = selectBillingAddress.State
                        Else
                            Cart.Order.BillToCounty = selectBillingAddress.Region
                        End If

                        If Not (isUSAddress) Then
                            IsCompleteIntAddress = Utility.Common.IsCompleteIntAddress(selectBillingAddress)
                            If IsCompleteIntAddress Then
                                Cart.Order.IsSameAddress = True
                            End If
                            Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId
                        End If

                        If selectBillingAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() AndAlso selectBillingAddress.AddressId = Cart.Order.ShippingAddressId Then
                            Cart.Order.IsSameAddress = True
                        End If
                        isSameAddress = Cart.Order.IsSameAddress
                        If Cart.Order.IsSameAddress Then
                            ''delete coupon in valid
                            If Not String.IsNullOrEmpty(lstCouponInValid) Then
                                Dim arrCoupon As String() = lstCouponInValid.ToString().Split(",")
                                If (arrCoupon.Length > 0) Then
                                    For Each couponCode As String In arrCoupon
                                        If Not (String.IsNullOrEmpty(couponCode)) Then
                                            Cart.RemoveCouponAndRecalculateAllOrderValues(couponCode, Cart.Order)
                                        End If
                                    Next
                                End If
                            End If
                            Cart.Order.ShipToSalonName = Cart.Order.BillToSalonName
                            Cart.Order.ShipToName = Cart.Order.BillToName
                            Cart.Order.ShipToName2 = Cart.Order.BillToName2
                            Cart.Order.ShipToAddress = Cart.Order.BillToAddress
                            Cart.Order.ShipToAddress2 = Cart.Order.BillToAddress2
                            Cart.Order.ShipToCity = Cart.Order.BillToCity
                            Cart.Order.ShipToCountry = Cart.Order.BillToCountry
                            Cart.Order.ShipToCounty = Cart.Order.BillToCounty
                            Cart.Order.ShipToFax = Cart.Order.BillToFax
                            Cart.Order.ShipToPhone = Cart.Order.BillToPhone
                            Cart.Order.ShipToPhoneExt = Cart.Order.BillToPhoneExt
                            Cart.Order.ShipToZipcode = Cart.Order.BillToZipcode
                            If (isUSAddress) Then
                                If (Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId) Then
                                    Cart.Order.CarrierType = Utility.Common.DefaultShippingId
                                End If
                            End If
                            If oldShippingMethod <> Cart.Order.CarrierType Then
                                Utility.Common.ResetOrderShippingMethod(Cart.Order.CarrierType, Cart.Order.OrderId, DB)
                            End If
                            If isUSAddress Then
                                sp.CheckAddressType(DB, Cart.Order)
                                HttpContext.Current.Session.Remove("CheckResidential")
                            End If
                            Cart.RecalculateOrderSignatureConfirmation(Cart.Order, isUSAddress)
                            If (selectBillingAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                                Cart.Order.ShippingAddressId = selectBillingAddress.AddressId
                            Else
                                ''get default shipping address
                                Dim shippingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, memberId, Utility.Common.MemberAddressType.Shipping.ToString())
                                shippingAddress.Company = Cart.Order.ShipToSalonName
                                shippingAddress.FirstName = Cart.Order.ShipToName
                                shippingAddress.LastName = Cart.Order.ShipToName2
                                shippingAddress.Address1 = Cart.Order.ShipToAddress
                                shippingAddress.Address2 = Cart.Order.ShipToAddress2
                                shippingAddress.City = Cart.Order.ShipToCity
                                shippingAddress.Country = Cart.Order.ShipToCountry
                                shippingAddress.State = Cart.Order.ShipToCounty
                                shippingAddress.Fax = Cart.Order.ShipToFax
                                shippingAddress.Phone = Cart.Order.ShipToPhone
                                shippingAddress.PhoneExt = Cart.Order.ShipToPhoneExt
                                shippingAddress.Zip = Cart.Order.ShipToZipcode
                                shippingAddress.DB = DB
                                shippingAddress.Update()
                                Cart.Order.ShippingAddressId = shippingAddress.AddressId
                            End If
                        End If

                        If (Cart.Order.IsSameAddress) Then
                            StoreOrderRow.ResetCartItemHandlingFee(DB, Cart.Order.OrderId)
                            Web.HttpContext.Current.Session("GetFedExRate") = Nothing
                        End If

                        Dim flammable As Common.FlammableCart = Cart.HasFlammableCartItem()
                        If flammable = Common.FlammableCart.BlockedHazMat AndAlso Cart.IsHazardousMaterialFee(Cart.Order.CarrierType) Then
                            strError = Resources.Alert.CartItemBlockedHazMat
                        ElseIf flammable = Common.FlammableCart.HazMat AndAlso Not Cart.HasCountryHazMat(Cart.Order.ShipToCountry) Then 'Or Cart.CheckShippingSpecialUS())
                            strError = Resources.Alert.CountryBlockedHazMat
                        End If

                        Cart.Order.IsFlammableCartItem = flammable
                        Cart.RecalculateOrderDetail(Utility.Common.SystemFunction.ChangeOrderAddress.ToString())
                        HttpContext.Current.Session("CartRender") = Cart

                        If (Cart.Order.IsSameAddress) Then
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                            htmlShippingList = Utility.Common.RenderUserControl("~/controls/checkout/shipping-list.ascx", False)
                            htmlFreightShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-freight-option.ascx")
                            htmlShippingOption = Utility.Common.RenderUserControl("~/controls/checkout/shipping-option.ascx")
                            htmlListCoupon = Utility.Common.RenderUserControl("~/controls/checkout/coupon-list.ascx")
                            If isRenderListCart Then
                                htmlCartList = Utility.Common.RenderUserControl("~/controls/product/order-detail.ascx")
                            End If
                        Else
                            HttpContext.Current.Session("addressTypeRender") = Utility.Common.MemberAddressType.Shipping.ToString()
                            htmlListShippingAddress = Utility.Common.RenderUserControl("~/controls/checkout/list-member-address.ascx")
                            HttpContext.Current.Session("addressTypeRender") = Nothing
                        End If
                        HttpContext.Current.Session.Remove("CartRender")
                    End If
                    '' isSameAddress = Cart.Order.IsSameAddress
                    DB.ExecuteSQL("Update Member set IsSameDefaultAddress=" & (IIf(isSameAddress = True, 1, 0)) & " where MemberId=" & memberId)
                    billingAddressId = Cart.Order.BillingAddressId
                    shippingAddressId = Cart.Order.ShippingAddressId

                    DB.Close()
                    HttpContext.Current.Session.Remove("CheckMemberIsInternational")

                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(12) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = New HtmlString(htmlShippingList)
            result(3) = New HtmlString(htmlShippingOption)
            result(4) = New HtmlString(htmlFreightShippingOption)
            result(5) = New HtmlString(htmlCartList)
            result(6) = New HtmlString(htmlListShippingAddress)
            result(7) = linkRedirect
            result(8) = isSameAddress
            result(9) = billingAddressId
            result(10) = shippingAddressId
            result(11) = IsCompleteIntAddress
            result(12) = New HtmlString(htmlListCoupon)
            Return result
        End Function
        Public Function ChangeOrderAddress(ByVal type As Object, ByVal addressId As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim htmlSumarryBox As String = String.Empty
            Dim sp As New SitePage()

            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    sp.Cart.RecalculateOrderDetail("ChangeOrderAddress")
                    HttpContext.Current.Session("CartRender") = sp.Cart
                    htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    HttpContext.Current.Session.Remove("CartRender")
                    DB.Close()
                    HttpContext.Current.Session.Remove("CheckMemberIsInternational")

                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlSumarryBox)
            result(2) = linkRedirect
            Return result
        End Function
        Public Function DeleteAddress(ByVal addressId As Object) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim sp As New SitePage()

            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    If (addressId <> sp.Cart.Order.BillingAddressId AndAlso addressId <> sp.Cart.Order.ShippingAddressId) Then
                        Dim objAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, addressId)
                        If Not objAddress Is Nothing Then
                            If (objAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                                If Not (MemberAddressRow.Delete(addressId)) Then
                                    strError = "An error has occurred while trying to delete this address. Please try again."
                                End If
                            Else
                                strError = "Cannot delete address because it is currently in use."
                            End If
                        Else
                            strError = "Address is not exists."
                        End If
                    Else
                        strError = "Cannot delete address because it is currently in use."
                    End If

                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = linkRedirect
            result(2) = addressId
            Return result
        End Function
        Public Function ValidateQtyFreeItemCoupon(ByVal DB As Database, ByVal orderId As Integer, ByVal memberId As Integer) As String
            Dim msg As String = String.Empty
            Dim SKU As String = String.Empty
            Dim promotionCode As String = String.Empty
            Dim typeError As Integer = 0
            StoreOrderRow.GetPromotionCheckoutNotValid(orderId, memberId, SKU, promotionCode, typeError)
            If Not String.IsNullOrEmpty(SKU) AndAlso Not String.IsNullOrEmpty(promotionCode) Then
                If (typeError = 1) Then ''qty
                    msg = "Quantity on hand is not enough for free item SKU #" & SKU & " , please remove your coupon " & promotionCode
                ElseIf (typeError = 2) Then
                    msg = "Free item SKU #" & SKU & "  is not available for customer outsite of 48 states within continental USA, please remove your coupon " & promotionCode
                ElseIf (typeError = 3) Then
                    msg = "Free item SKU #" & SKU & "  available only in store or by phone order, please remove your coupon " & promotionCode
                End If
            End If
            Return msg
        End Function
        Public Function PlaceOrder(ByVal cardName As Object, ByVal cardType As Object, ByVal cardNumber As Object, ByVal expireMonth As Object, ByVal expireYear As Object, ByVal cardSecurity As Object, ByVal note As String) As Object

            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()


            Dim sp As New SitePage()
            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else
                If Not Utility.Common.IsCreditCardNumberValid(cardNumber) Then
                    strError = "Please enter a valid credit card number."
                    Return ReturnPlaceOrderError(Nothing, strError)
                End If
                Dim orderId As Integer = Utility.Common.GetCurrentOrderId()

                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    Utility.Common.OrderLog(orderId, "Place Order", Nothing)

                    Dim Member As MemberRow = MemberRow.GetRow(memberId)

                    Dim Cart As New ShoppingCart(DB)
                    Dim objOrder As StoreOrderRow = Cart.Order

                    If objOrder Is Nothing Then
                        Email.SendError("ToError500", "[Payment] PlaceOrder >> Order Is Nothing", "OrderId: " & orderId)
                        Exit Try
                    End If

                    'Check number wrong Credit Card
                    If HttpContext.Current.Session("InvalidCreditCard") IsNot Nothing Then
                        Try
                            Dim arr As String() = CType(HttpContext.Current.Session("InvalidCreditCard"), String())
                            If Convert.ToInt16(arr(0)) >= 5 Then
                                If Convert.ToDateTime(arr(1)) >= DateTime.Now Then
                                    strError = "You entered wrong credit card many times. Try again in " & Convert.ToDateTime(arr(1)).Subtract(DateTime.Now).Seconds.ToString() & " seconds."
                                    Return ReturnPlaceOrderError(DB, strError)
                                Else
                                    HttpContext.Current.Session.Remove("InvalidCreditCard")
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                    'Order Price Min
                    strError = Cart.CheckOrderPriceMin(objOrder, Resources.Alert.OrderMin)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    'Check Billing & Shipping invalid
                    If objOrder.BillingAddressId < 1 Then
                        strError = "Please select a billing address."
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    If objOrder.ShippingAddressId < 1 Then
                        strError = "Please select a shipping address."
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    'Check Russia
                    If Utility.Common.IsShipToRussia(objOrder) Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.Russia)
                    End If

                    'Check PO Box
                    If Utility.Common.IsOrderShippingPOBoxAddress(objOrder) Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.POBoxShippingAddress)
                    End If

                    'Check Flammable
                    Dim flammable As Common.FlammableCart = Cart.HasFlammableCartItem()
                    If flammable = Common.FlammableCart.BlockedHazMat AndAlso Cart.IsHazardousMaterialFee() Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.CartItemBlockedHazmat2)
                    ElseIf flammable = Common.FlammableCart.HazMat AndAlso Not Cart.HasCountryHazMat(objOrder.ShipToCountry) Then ' Or Cart.CheckShippingSpecialUS()) Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.CountryBlockedHazMat)
                    End If

                    'If Utility.Common.CheckShippingInternational(DB, objOrder) Then
                    '    Dim SKUFlammable As String = Utility.Common.GetFlammableSKU(DB, objOrder.OrderId)
                    '    If Not String.IsNullOrEmpty(SKUFlammable) Then
                    '        strError = "The item " & SKUFlammable & " is not available for customer outsite of 48 states within continental USA. Please go back to <a href=""cart.aspx"">shopping cart</a> and remove them before proceeding to check out."
                    '        Return ReturnPlaceOrderError(DB, strError)
                    '    End If
                    'End If

                    'Free Gift invalid
                    Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(objOrder.OrderId)
                    If MinFreeGiftLevelInvalid > 0 Then
                        Return ReturnPlaceOrderError(DB, String.Format(Resources.Alert.FreeGiftMin, MinFreeGiftLevelInvalid))
                    End If

                    'Special Handling Fee invalid
                    strError = CheckSpecialHandlingFeeValid(DB, objOrder)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    'FreeSample invalid
                    If Cart.CheckTotalFreeSample(objOrder.OrderId) Then
                        Return ReturnPlaceOrderError(DB, String.Format(Resources.Alert.FreeSamplesMin, CDbl(SysParam.GetValue("FreeSampleOrderMin"))))
                    End If

                    'Shipping TBD
                    Dim shippingTBD As String = StoreOrderRow.GetShippingTBDName(objOrder.OrderId)
                    If Not String.IsNullOrEmpty(shippingTBD) Then
                        Return ReturnPlaceOrderError(DB, String.Format(Resources.Alert.ShippingInvalid, shippingTBD))
                    End If

                    'Check Product coupon + order coupon valid
                    strError = ValidateQtyFreeItemCoupon(DB, orderId, memberId)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    'Tracking Credit Card click
                    Email.SendReport("ToReportPayment", "[CreditCard] Click", "OrderId: " & objOrder.OrderId & "<br>Card Type: " & cardType & "<br>Name: " & cardName & "<br>Number Length: " & cardNumber & "<br>CID Length: " & cardSecurity.ToString().Length)

                    Dim strExpireDate As String = GetExpireDate(expireMonth, expireYear)
                    Dim dtExpireDate = Convert.ToDateTime(strExpireDate)
                    Dim bCheckPayeezy As Boolean

                    If SitePage.IsLocal() Then
                        bCheckPayeezy = True
                    Else
                        bCheckPayeezy = CheckPayeezy(DB, objOrder, dtExpireDate, cardNumber, cardSecurity, strError)
                    End If

                    If Not bCheckPayeezy Then
                        Utility.Common.OrderLog(orderId, "Place Order Invalid Credit Card", Nothing)

                        Dim arr(1) As String
                        If HttpContext.Current.Session("InvalidCreditCard") IsNot Nothing Then
                            Try
                                Dim arrTemp As String() = CType(HttpContext.Current.Session("InvalidCreditCard"), String())
                                arr(0) = CInt(arrTemp(0)) + 1
                                arr(1) = DateTime.Now.AddSeconds(33).ToString()
                            Catch ex As Exception
                            End Try
                        Else
                            arr(0) = "1"
                            arr(1) = DateTime.Now.AddSeconds(33).ToString()
                        End If
                        HttpContext.Current.Session("InvalidCreditCard") = arr
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    Dim quantityerror As Boolean = False
                    CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & HttpContext.Current.Session("OrderId")) 'Clear cache header

                    Dim cardTypeId As Int16 = cardType
                    Dim ipLocation As String = sp.GetCityLocation(objOrder.RemoteIP)
                    Dim bSuccess As Boolean = False
                    Try
                        DB.BeginTransaction()
                        objOrder.PaymentType = "CC"
                        objOrder.CardNumber = cardNumber
                        objOrder.CIDNumber = cardSecurity
                        objOrder.CardHolderName = cardName
                        objOrder.CardTypeId = IIf(cardTypeId > 0, cardTypeId, Nothing)
                        objOrder.ExpirationDate = dtExpireDate
                        objOrder.Notes = note
                        objOrder.SellToCustomerId = Member.CustomerId
                        objOrder.DoExport = True
                        objOrder.ProcessDate = Now()
                        objOrder.ReferralCode = IIf(HttpContext.Current.Session("ref") <> Nothing, HttpContext.Current.Session("ref"), String.Empty)
                        objOrder.ProcessSessionID = HttpContext.Current.Session.SessionID
                        objOrder.IPLocation = ipLocation
                        If objOrder.HazardousMaterialFee > 0 Then
                            objOrder.Comments &= String.Format("|Hazardous Material Fee {0:C2}", objOrder.HazardousMaterialFee)
                        End If

                        Dim CheckOutGuest As Boolean = IIf(HttpContext.Current.Session("CheckOutGuest") IsNot Nothing AndAlso HttpContext.Current.Session("CheckOutGuest") = "1", True, False)
                        If objOrder.UpdateCheckout(CheckOutGuest) Then

                            'Note cash point
                            Cart.GetPoints(objOrder.PurchasePoint + objOrder.TotalRewardPoint)
                            Dim PointAvailable As Integer = CashPointRow.GetTotalCashPointByMember(DB, memberId, objOrder.OrderId)
                            Cart.AddBuyPointFromOrder(PointAvailable)
                            MemberReferRow.UpdateStatusReferFriendFromOrder(DB, objOrder.MemberId, objOrder.OrderId)

                            DB.CommitTransaction()
                            bSuccess = True
                        Else
                            strError = "There was an error while processing your order. Please contact our Customer Service department."
                            Email.SendError("ToErrorPayment", "[Payment] Order Processing Error Else!", "MemberId: " & HttpContext.Current.Session("MemberId") & "<br>UserName: " & HttpContext.Current.Session("Username"))
                            Return ReturnPlaceOrderError(DB, strError)
                        End If
                    Catch ex As Exception
                        DB.RollbackTransaction()

                        If quantityerror Then
                            strError = ex.ToString
                            Email.SendError("ToErrorPayment", "[Payment] Quantity Error!", ex.ToString)
                        Else
                            strError = "There was an error while processing your order. Please contact our Customer Service department."
                            Email.SendError("ToErrorPayment", "[Payment] Order Processing Error!", "MemberId: " & HttpContext.Current.Session("MemberId") & "<br>UserName: " & HttpContext.Current.Session("Username") & "<br>Exception: " & ex.ToString)
                        End If
                        Return ReturnPlaceOrderError(DB, strError)
                    End Try

                    If bSuccess Then
                        If Not SitePage.IsLocal() Then
                            logOrderDevice(objOrder.OrderId, HttpContext.Current.Request.UserAgent)
                            Components.ShoppingCart.SendOrderConfirmation(objOrder.OrderId)

                            'Maxmind in background
                            If objOrder.PaymentType = "CC" Then
                                Dim maxmind As Maxmind.Entities.OutputField = New Maxmind.Entities.OutputField(objOrder.OrderId, objOrder.CardNumber)
                            End If
                        End If

                        'Send Inventory Stock Notifications 
                        Dim strSubject As String = String.Empty
                        Dim strBody As String = String.Empty
                        Dim dv As DataView = DB.GetDataView("SELECT si.ItemId, si.SKU, si.QtyOnHand, si.ItemName, ISNULL(si.InventoryStockNotification, 0) AS 'InventoryStockNotification', si.IsSpecialOrder, si.AcceptingOrder, [dbo].[fc_StoreCartItem_SumQuantityByItemId](" & objOrder.OrderId & ", si.Itemid) AS Quantity FROM StoreItem si, StoreCartItem sci WHERE si.ItemId = sci.ItemId AND  OrderId = " & objOrder.OrderId & " and sci.[type] = 'item'")
                        For i As Integer = 0 To dv.Count - 1
                            Dim qtyUpdate As Integer = CInt(dv(i)("QtyOnHand"))
                            If qtyUpdate < dv(i)("Quantity") Then
                                quantityerror = True
                                qtyUpdate = 0
                            Else
                                qtyUpdate -= dv(i)("Quantity")
                            End If
                            StoreItemRow.UpdateQtyOnHand(DB, CInt(dv(i)("ItemId")), qtyUpdate)

                            Try
                                If CBool(dv(i)("IsSpecialOrder")) = True Or CInt(dv(i)("AcceptingOrder")) > 0 Then
                                    Continue For
                                End If

                                qtyUpdate = CInt(dv(i)("QtyOnHand"))
                                Dim InventoryStockNotification As Integer = CInt(dv(i)("InventoryStockNotification"))
                                If (SysParam.GetValue("SendInventoryStockNotifications") = "1" AndAlso qtyUpdate <= SysParam.GetValue("InventoryStockNotification")) OrElse (InventoryStockNotification > 0 AndAlso qtyUpdate <= InventoryStockNotification) Then
                                    strBody &= "Item: " & dv(i)("SKU") & vbCrLf &
                                        "<br>Item Name: " & dv(i)("ItemName") & vbCrLf &
                                        "<br>Current Quantity: " & qtyUpdate & vbCrLf &
                                        "<br>Item Stock Threshold: " & IIf(InventoryStockNotification = 0, "Not Set", InventoryStockNotification) & vbCrLf &
                                        "<br>Global Stock Threshold: " & SysParam.GetValue("InventoryStockNotification") & vbCrLf &
                                        "<br>----------------------------<br><br>"
                                    strSubject &= ", " & dv(i)("SKU")
                                End If
                            Catch ex As Exception
                                Email.SendError("ToError500", "[Payment] Inventory Stock Notification! For", ex.ToString)
                            End Try
                        Next

                        If Not String.IsNullOrEmpty(strSubject) Then
                            If strSubject.Length > 2 Then
                                strSubject = strSubject.Substring(1)
                            End If

                            Email.SendReport("InventoryStockEmail", "Inventory Stock Warning! - " & strSubject, strBody)
                        End If

                        HttpContext.Current.Session.Remove("OrderId")
                        DB.Close()

                        linkRedirect = "/store/confirmation.aspx?OrderId=" & objOrder.OrderId.ToString()
                    End If
                Catch ex As Exception
                    Email.SendError("ToError500", "[Payment] PlaceOrder", "OrderId: " & orderId & "<br>Exception: " & ex.ToString)
                    strError = ex.Message
                End Try
            End If

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(strError)
            result(1) = linkRedirect
            Return result
        End Function

        Private Function GetExpireDate(ByVal month As String, ByVal year As String) As String
            Dim result As String = String.Empty
            If (month = String.Empty Or year = String.Empty) Then
                result = ""
            Else
                result = month & "/" & 1 & "/" & year
            End If
            Return result
        End Function
        Private Function CheckPayeezy(ByVal DB As Database, ByVal o As StoreOrderRow, ByVal expireDate As DateTime, ByVal cardNumber As String, ByVal cardSecurity As String, ByRef errorReturn As String) As Boolean
            Dim flag As Boolean = False

            Dim op As New StoreOrderPayflowRow()
            op = StoreOrderPayflowRow.GetRowByOrderId(DB, o.OrderId)
            If op.OrderId > 0 AndAlso op.PayflowId > 0 AndAlso op.Result = 0 And Not String.IsNullOrEmpty(op.PnRef) And op.RespMsg = "Approved" Then
                Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayeezy SECOND PAID", "OrderId=" & o.OrderId & "<br>PayflowId=" & op.PayflowId & "<br>Result=" & op.Result)
                Return True
            End If

            'Populate the Billing address details.
            Dim Bill As New BillTo()
            Bill.FirstName = o.BillToName
            Bill.LastName = o.BillToName2
            Bill.Street = o.BillToAddress
            Bill.City = o.BillToCity
            Bill.Zip = o.BillToZipcode
            Bill.State = o.BillToCounty

            ''Populate the Shipping address details.
            'Dim Ship As New ShipTo()
            'Ship.ShipToFirstName = o.ShipToName
            'Ship.ShipToLastName = o.ShipToName2
            'Ship.ShipToStreet = o.ShipToAddress
            'Ship.ShipToCity = o.ShipToCity
            'Ship.ShipToZip = o.ShipToZipcode
            'Ship.ShipToState = o.ShipToCounty

            ''Populate the invoice
            'Dim invoice As New Invoice()
            'invoice.BillTo = Bill
            'invoice.ShipTo = Ship
            'invoice.InvNum = op.OrderId

            'invoice.Amt = New Currency(Decimal.Parse(o.Total))

            'Populate the Credit Card details.
            Dim strExpDate As String = String.Format("{0:MMyy}", expireDate)
            Dim Card As New CreditCard(cardNumber, strExpDate)
            Card.Cvv2 = cardSecurity

            Try
                Dim payeezyObj = New With
                {
                    Key .merchant_ref = "Nail Superstore",
                    Key .transaction_type = "authorize",
                    Key .method = "credit_card",
                    Key .amount = (Math.Round(o.Total, 2) * 100).ToString(),
                    Key .currency_code = "USD",
                    Key .credit_card = New With
                    {
                        Key .card_number = cardNumber,
                        Key .cvv = Card.Cvv2,
                        Key .type = "visa",
                        Key .exp_date = strExpDate,
                        Key .cardholder_name = cardNumber
                    },
                    Key .billing_address = New With
                    {
                        Key .street = Bill.Street,
                        Key .city = Bill.City,
                        Key .state_province = Bill.State,
                        Key .zip_postal_code = Bill.Zip,
                        Key .country = Bill.BillToCountry
                    }
                }
                Dim objJson As String = Newtonsoft.Json.JsonConvert.SerializeObject(payeezyObj)
                Dim Trans = Payeezy.Payeezy.Authorize(objJson, op)
                op.OrderId = o.OrderId
                op.CreatedDate = Now
                op.Insert()

                If op.Result >= 0 Then
                    If op.PayflowId > 0 AndAlso op.Result = 0 Then
                        flag = True
                    Else
                        Dim strMsg As String = op.RespMsg
                        If Not op.RespMsg.Length > 0 Then
                            strMsg = op.RespMsg
                        End If
                        errorReturn = op.RespMsg
                    End If
                Else
                    errorReturn = "A System Error has occured. Our system administrators have been notified and are working to fix the problem. Please contact Customer Service at support@nss.com"
                    Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayeezy Else 2", "OrderId=" & o.OrderId & "<br>Result=" & op.Result & "<br>Message=" & "This is message")
                End If
            Catch ex As Exception
                errorReturn = "A System Error has occured. Our system administrators have been notified and are working to fix the problem. Please contact Customer Service at support@nss.com"
                Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayeezy Exception", "OrderId=" & o.OrderId & "<br>Exception=" & ex.ToString())
            End Try

            Return flag
        End Function
        Private Function CheckPayflowPro(ByVal DB As Database, ByVal o As StoreOrderRow, ByVal expireDate As DateTime, ByVal cardNumber As String, ByVal cardSecurity As String, ByRef errorReturn As String) As Boolean
            Dim flag As Boolean = False

            'Khoa: Authorize Payflow Pro
            Dim op As New StoreOrderPayflowRow()
            op = StoreOrderPayflowRow.GetRowByOrderId(DB, o.OrderId)
            If op.OrderId > 0 AndAlso op.PayflowId > 0 AndAlso op.Result = 0 And Not String.IsNullOrEmpty(op.PnRef) And op.RespMsg = "Approved" Then
                Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayflowPro SECOND PAID", "OrderId=" & o.OrderId & "<br>PayflowId=" & op.PayflowId & "<br>Result=" & op.Result)
                Return True
            End If

            'If op.OrderId > 0 AndAlso op.PayflowId > 0 AndAlso op.Result = 0 Then
            '    Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayflowPro StoreOrderPayflowRow.GetRow", "OrderId=" & o.OrderId & "<br>PayflowId=" & op.PayflowId & "<br>Result=" & op.Result)
            '    Return True
            'End If

            'Populate the Billing address details.
            Dim Bill As New BillTo()
            Bill.FirstName = o.BillToName
            Bill.LastName = o.BillToName2
            Bill.Street = o.BillToAddress
            Bill.City = o.BillToCity
            Bill.Zip = o.BillToZipcode
            Bill.State = o.BillToCounty

            'Populate the Shipping address details.
            Dim Ship As New ShipTo()
            Ship.ShipToFirstName = o.ShipToName
            Ship.ShipToLastName = o.ShipToName2
            Ship.ShipToStreet = o.ShipToAddress
            Ship.ShipToCity = o.ShipToCity
            Ship.ShipToZip = o.ShipToZipcode
            Ship.ShipToState = o.ShipToCounty

            'Populate the invoice
            Dim invoice As New Invoice()
            invoice.BillTo = Bill
            invoice.ShipTo = Ship
            invoice.InvNum = op.OrderId

            invoice.Amt = New Currency(Decimal.Parse(o.Total))

            'Populate the Credit Card details.
            Dim strExpDate As String = String.Format("{0:MMyy}", expireDate)
            Dim Card As New CreditCard(cardNumber, strExpDate)
            Card.Cvv2 = cardSecurity

            'Create the Tender.
            Dim tender As New PayPal.Payments.DataObjects.CardTender(Card)

            'Get connection to paypal
            Dim bLive As Boolean = IIf(SysParam.GetValue("API_IS_LIVE") = "1", True, False)
            Dim PayflowUser As New UserInfo(SysParam.GetValue("PaypalUser"), SysParam.GetValue("PaypalMerchant"), SysParam.GetValue("PaypalPartner"), SysParam.GetValue("PaypalPassword"))

            Try
                'Create the transaction
                Dim Trans As New AuthorizationTransaction(PayflowUser, Constants.GetConnection(bLive), invoice, tender, PayflowUtility.RequestId)
                Trans.SubmitTransaction()

                op.OrderId = o.OrderId
                op.PnRef = Trans.Response.TransactionResponse.Pnref
                op.Result = Trans.Response.TransactionResponse.Result
                op.RespMsg = String.Format("${0} - {1}", invoice.Amt, Trans.Response.TransactionResponse.RespMsg)
                op.AvsAddr = IIf(Trans.Response.TransactionResponse.AVSAddr = Nothing, "", Trans.Response.TransactionResponse.AVSAddr)
                op.AvsZip = IIf(Trans.Response.TransactionResponse.AVSZip = Nothing, "", Trans.Response.TransactionResponse.AVSZip)
                op.Cvv2Match = IIf(Trans.Response.TransactionResponse.CVV2Match = Nothing, "", Trans.Response.TransactionResponse.CVV2Match)
                op.CreatedDate = Now
                op.Insert()

                If Trans.Response.TransactionResponse.Result >= 0 Then
                    If op.PayflowId > 0 AndAlso Trans.Response.TransactionResponse.Result = 0 Then
                        flag = True
                    Else
                        Dim strMsg As String = Constants.ShowMsg(Trans.Response.TransactionResponse.Result)
                        If Not strMsg.Length > 0 Then
                            strMsg = Trans.Response.TransactionResponse.RespMsg
                        End If
                        errorReturn = strMsg
                    End If
                Else
                    errorReturn = "A System Error has occured. Our system administrators have been notified and are working to fix the problem. Please contact Customer Service at support@nss.com"
                    Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayflowPro Else 2", "OrderId=" & o.OrderId & "<br>Result=" & Trans.Response.TransactionResponse.Result & "<br>Message=" & Constants.ShowMsg(Trans.Response.TransactionResponse.Result))
                End If
            Catch ex As Exception
                errorReturn = "A System Error has occured. Our system administrators have been notified and are working to fix the problem. Please contact Customer Service at support@nss.com"
                Email.SendError("ToErrorPayment", "[SubmitOrder] CheckPayflowPro Exception", "OrderId=" & o.OrderId & "<br>Exception=" & ex.ToString())
            End Try

            Return flag
        End Function
        Private Function ReturnPlaceOrderError(ByVal DB As Database, ByVal errorMsg As String) As Object
            If Not DB Is Nothing Then
                If Not DB.Transaction Is Nothing Then
                    DB.RollbackTransaction()
                End If
                DB.Close()
            End If

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(errorMsg)
            result(1) = String.Empty

            Return result
        End Function

        Private Function CheckSpecialHandlingFeeValid(ByVal DB As Database, ByVal o As StoreOrderRow) As String
            Dim result As String = String.Empty
            If o.CarrierType = Utility.Common.DefaultShippingId Then
                Dim TotalCurrentSpecialHandlingFee As Double = DB.ExecuteScalar("select coalesce(SUM([dbo].[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType)*Quantity),0) from StoreCartItem where OrderId=" & o.OrderId & "  AND Type <>'carrier' AND IsOversize = 0 AND CarrierType=" & Utility.Common.DefaultShippingId & " AND Weight >= 1")
                Dim OrderSpecialHandlingFee As Double = DB.ExecuteScalar("Select TotalSpecialHandlingFee from StoreOrder where OrderId=" & o.OrderId)
                If TotalCurrentSpecialHandlingFee <> OrderSpecialHandlingFee Then
                    BaseShoppingCart.SendTrackingOrder(o.OrderId)
                    result = "A System Error has occured. Our system administrators have been notified And are working to fix the problem. Please contact Customer Service at support@nss.com"
                    Components.Email.SendError("ToError500", "SpecialHandlingFee Error- Not allow checkout-URL:  " & HttpContext.Current.Request.Url.AbsoluteUri.ToString, "<br>OrderId : " & o.OrderId.ToString() & ",MemberId:" & o.MemberId.ToString)
                End If
            End If
            Return result
        End Function

        Public Function SelectPaypalPayment() As Object
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim htmlPaypalPopup As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim arrMail As New ArrayList
            Dim sp As New SitePage()

            If (memberId < 1) Then
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
                linkRedirect = "/members/login.aspx"
            Else

                Dim orderId As Integer = Common.GetCurrentOrderId()
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    sp.Cart.DB = DB
                    Dim objOrder As StoreOrderRow = sp.Cart.Order
                    If objOrder.BillingAddressId < 1 Then
                        strError = "Please select a billing address."
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    If objOrder.ShippingAddressId < 1 Then
                        strError = "Please select a shipping address."
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    ''valid date free item product coupon + order coupon
                    strError = ValidateQtyFreeItemCoupon(DB, orderId, memberId)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    strError = CheckSpecialHandlingFeeValid(DB, objOrder)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    If Utility.Common.IsShipToRussia(objOrder) Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.Russia)
                    End If
                    If Utility.Common.IsOrderShippingPOBoxAddress(objOrder) Then
                        Return ReturnPlaceOrderError(DB, Resources.Alert.POBoxShippingAddress)
                    End If
                    strError = sp.Cart.CheckOrderPriceMin(objOrder, Resources.Alert.OrderMin)
                    If Not String.IsNullOrEmpty(strError) Then
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    If sp.Cart.CheckTotalFreeSample(objOrder.OrderId) Then
                        Dim OrderTotalFreeSample As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
                        strError = String.Format(Resources.Alert.FreeSamplesMin, OrderTotalFreeSample)
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(objOrder.OrderId)
                    If MinFreeGiftLevelInvalid > 0 Then
                        strError = strError & String.Format(Resources.Alert.FreeGiftMin, MinFreeGiftLevelInvalid)
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    If Utility.Common.CheckShippingInternational(DB, sp.Cart.Order) Then
                        Dim SKUFlammable As String = Utility.Common.GetFlammableSKU(DB, objOrder.OrderId)
                        If Not String.IsNullOrEmpty(SKUFlammable) Then
                            strError = "The item# " & SKUFlammable & " is not available for customer outsite of 48 states within continental USA. Please go back to <a href=""revise-cart.aspx"">shopping cart</a> and remove them before proceeding to check out."
                            Return ReturnPlaceOrderError(DB, strError)
                        End If
                    End If
                    If objOrder.CarrierType = 1 Then
                        strError = "UPS 3-Day Service is unavailable at this time. Please contact Customer Service at support@nss.com"
                        Email.SendError("ToError500", "Paypal UPS 3-Day Service", "OrderId: " & objOrder.OrderId & "<br>Session List:<br>" & SitePage.GetSessionList())
                        Return ReturnPlaceOrderError(DB, strError)
                    End If

                    Dim shippingTBD As String = StoreOrderRow.GetShippingTBDName(objOrder.OrderId)
                    If Not String.IsNullOrEmpty(shippingTBD) Then
                        strError = String.Format(Resources.Alert.ShippingInvalid, shippingTBD)
                        Return ReturnPlaceOrderError(DB, strError)
                    End If
                    htmlPaypalPopup = Utility.Common.RenderUserControl("~/controls/checkout/paypal-popup.ascx")

                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlPaypalPopup)
            result(2) = linkRedirect
            Return result


        End Function


#End Region
#Region "Estimate Shipping"
        Public Class EstimateShippingInfor
            Private m_EstimateTotalWeight As Double = 0
            Private m_EstimateTotalFreeWeight As Double = 0
            Private m_EstimateTotalFreightWeight As Double = 0
            Private m_EstimateTotalFreighFreeWeight As Double = 0
            Private m_EstimateZipcode As String = ""
            Private m_EstimateState As String = ""
            Public Property EstimateTotalWeight() As Double
                Get
                    Return m_EstimateTotalWeight
                End Get
                Set(ByVal value As Double)
                    m_EstimateTotalWeight = value
                End Set
            End Property
            Public Property EstimateTotalFreeWeight() As Double
                Get
                    Return m_EstimateTotalFreeWeight
                End Get
                Set(ByVal value As Double)
                    m_EstimateTotalFreeWeight = value
                End Set
            End Property
            Public Property EstimateTotalFreightWeight() As Double
                Get
                    Return m_EstimateTotalFreightWeight
                End Get
                Set(ByVal value As Double)
                    m_EstimateTotalFreightWeight = value
                End Set
            End Property
            Public Property EstimateTotalFreighFreeWeight() As Double
                Get
                    Return m_EstimateTotalFreighFreeWeight
                End Get
                Set(ByVal value As Double)
                    m_EstimateTotalFreighFreeWeight = value
                End Set
            End Property
            Public Property EstimateZipcode() As String
                Get
                    Return m_EstimateZipcode
                End Get
                Set(ByVal value As String)
                    m_EstimateZipcode = value
                End Set
            End Property
            Public Property EstimateState() As String
                Get
                    Return m_EstimateState
                End Get
                Set(ByVal value As String)
                    m_EstimateState = value
                End Set
            End Property
        End Class
        Public Function CalEstimateShipping(ByVal country As String, ByVal zipCode As String) As Object
            Dim isInternational As Boolean = False
            Dim strError As String = String.Empty
            Dim htmlPopupEstimateShipping As String = String.Empty
            Dim memberId As Integer = Common.GetCurrentMemberId()
            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim dbZipCode As New ZipCodeRow()
            Dim resultTemp As Object() = New Object(2) {}

            Try
                If country = "US" AndAlso String.IsNullOrEmpty(zipCode) Then
                    Exit Try
                End If

                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

                Dim sp As New SitePage()
                HttpContext.Current.Session("GetFedExRate") = Nothing
                Dim dShipping As Double = 0
                Dim Cart As New ShoppingCart(DB, orderId, False)
                'DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping=0 WHERE OrderId = " & Cart.Order.OrderId)
                Utility.Common.DeleteCachePopupCart(Cart.Order.OrderId)
                If zipCode.Trim.Length < 5 Then
                    strError = Resources.Alert.InvalidZipCode
                Else
                    dbZipCode = ZipCodeRow.GetRow(DB, zipCode)
                    If dbZipCode.StateCode = "" Then
                        strError = Resources.Alert.InvalidZipCode
                    Else
                        If country <> "US" OrElse (country = "US" AndAlso (dbZipCode.StateCode = "PR" Or dbZipCode.StateCode = "VI" Or dbZipCode.StateCode = "HI" Or dbZipCode.StateCode = "AK" Or dbZipCode.StateCode = "AE" Or dbZipCode.StateCode = "AP" Or dbZipCode.StateCode = "AA")) Then
                            isInternational = True
                            Exit Try
                        End If

                        Dim lstTBD As New System.Collections.Generic.List(Of String)
                        Dim dsShippingList As DataTable = Cart.GetShippingMethodPrices(zipCode, dbZipCode.StateCode)
                        If dsShippingList IsNot Nothing AndAlso dsShippingList.Rows.Count > 0 Then
                            Dim objResultShipping As EstimateShippingInfor = GetWeightForCalculateEstimateShipping(DB, Cart, zipCode)
                            objResultShipping.EstimateState = dbZipCode.StateCode
                            Dim lstEstimateShippingAvailable As New List(Of Integer)

                            'Bind Data here
                            htmlPopupEstimateShipping = BindEstimateShippingPopup(DB, Cart, dsShippingList, zipCode, memberId, objResultShipping, lstEstimateShippingAvailable)
                            HttpContext.Current.Session("EstimateShipping") = country & "|" & zipCode
                            If lstEstimateShippingAvailable.Count() > 0 Then
                                resultTemp = ChangeEstimateShipping(lstEstimateShippingAvailable(0), country, zipCode)
                            End If
                        Else
                            strError = Resources.Alert.InvalidZipCode
                        End If
                    End If
                End If
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            If memberId <= 0 Then 'Chi ghi log neu chua login, vi login roi se auto estimate shipping
                Utility.Common.OrderLog(orderId, "Estimate Shipping", {"Country=" & country, "ZipCode=" & zipCode})
            End If
            Dim result As Object() = New Object(3) {}


            If isInternational Then
                If dbZipCode.StateCode = "AE" Then 'Vi State Code Military AE = United Arab Emirates nen phai thay doi
                    dbZipCode.StateCode = "APE"
                End If

                resultTemp = ChangeCountryEstimateShipping(dbZipCode.StateCode, "")
                result(0) = resultTemp(0)
                result(1) = resultTemp(1)
                result(2) = String.Empty
            Else
                result(0) = New HtmlString(htmlPopupEstimateShipping)
                result(1) = New HtmlString(strError)
                If resultTemp(0) Is Nothing Then
                    result(2) = New HtmlString(String.Empty)
                Else
                    result(2) = resultTemp(0)
                End If
            End If

            result(3) = isInternational
            Return result
        End Function
        Public Shared Function calculateDateShipping(StartDate As DateTime, NumberOfBusinessDays As Byte, checkHour As Boolean, Optional isFullText As Boolean = False) As String

            If NumberOfBusinessDays = 0 Then
                Return String.Empty
            End If
            Dim holidayDays As Date() = ConfigData.HolidayDate
            If checkHour And (StartDate.Hour > ConfigData.HourShipping - 1) And Not (holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month)) And (StartDate.DayOfWeek <> DayOfWeek.Saturday And StartDate.DayOfWeek <> DayOfWeek.Sunday) Then
                NumberOfBusinessDays += 1
            End If

            Dim index As Int16 = 0
            Do Until NumberOfBusinessDays = index
                If (holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month)) Or StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday Then
                    While (holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month)) Or StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday
                        If StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday Then
                            While StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday
                                If holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month) Then
                                    'nghi bu
                                    index = index - 1
                                    StartDate = StartDate.AddDays(1)
                                Else
                                    StartDate = StartDate.AddDays(1)
                                End If
                            End While
                        End If
                        If holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month) And StartDate.DayOfWeek <> DayOfWeek.Saturday And StartDate.DayOfWeek <> DayOfWeek.Sunday Then
                            StartDate = StartDate.AddDays(1)
                        End If
                    End While
                    'index = index + 1
                Else
                    StartDate = StartDate.AddDays(1)
                    index = index + 1
                End If
            Loop

            index = 0
            Do Until 1 = index
                If (holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month)) Or StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday Then
                    While (holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month)) Or StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday
                        If StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday Then
                            While StartDate.DayOfWeek = DayOfWeek.Saturday Or StartDate.DayOfWeek = DayOfWeek.Sunday
                                If holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month) Then
                                    'nghi bu
                                    index = index - 1
                                    StartDate = StartDate.AddDays(1)
                                Else
                                    StartDate = StartDate.AddDays(1)
                                End If
                            End While
                        End If
                        If holidayDays IsNot Nothing AndAlso holidayDays.Any(Function(holiday) holiday.Day = StartDate.Day AndAlso holiday.Month = StartDate.Month) And StartDate.DayOfWeek <> DayOfWeek.Saturday And StartDate.DayOfWeek <> DayOfWeek.Sunday Then
                            StartDate = StartDate.AddDays(1)
                        End If
                    End While
                    'index = index + 1
                Else
                    If index < 0 Then 'Co nghi bu
                        StartDate = StartDate.AddDays(1)
                    End If
                    index = index + 1
                End If
            Loop

            If Not isFullText Then
                Return String.Format("<br><span class='estimate' style='font-size: 12px; color: rgb(89, 89, 91);'><a href=""#"" data-html=""true"" data-toggle=""popover""><span class='estimate-shipping-text' style=""color: #3b76ba;text-decoration: underline;"">Est Delivery:</span></a> {0}</span>", StartDate.ToString("ddd, MMM dd"))
            Else
                Return String.Format("<br><span class='estimate' style='font-size: 12px; color: rgb(89, 89, 91); margin-left: 28px;'><a href=""#"" data-html=""true"" data-toggle=""popover""><span class='estimate-shipping-text' style=""color: #3b76ba;text-decoration: underline;"">Est Delivery:</span></a> {0}</span>", StartDate.ToString("ddd, MMM dd"))
            End If

        End Function
        Private Function BindEstimateShippingPopup(ByVal DB As Database, ByVal Cart As ShoppingCart, ByVal dsList As DataTable, ByVal zipCode As String, ByVal memberId As Integer, ByVal objShippingInfor As EstimateShippingInfor, ByRef lstEstimateShippingAvailable As List(Of Integer)) As String
            Dim htmlResult As String = "<table>"
            Dim ltrShippingName As String = String.Empty
            Dim htmlPrice As String = String.Empty
            Dim htmlRow As String = String.Empty
            Dim bFirstCheck As Boolean = False
            For Each dr As DataRow In dsList.Rows
                htmlRow = String.Empty
                ltrShippingName = String.Empty
                htmlPrice = String.Empty
                Dim name As String = dr("Name")
                Dim iMethodId As String = dr("MethodId")
                Dim code As String = dr("Code")
                Dim dPrices As Double = 0
                Dim isTBD As Boolean = False
                Dim htmlSelect As String = String.Empty
                Dim htmlName As String = String.Empty
                If iMethodId = Utility.Common.TruckShippingId Then
                    dPrices = Cart.RecalculateShippingCartPrice("US", zipCode, iMethodId, Cart.Order.OrderId, code, objShippingInfor.EstimateTotalFreightWeight + objShippingInfor.EstimateTotalWeight, 0, False)
                    htmlPrice = FormatCurrency(dPrices.ToString)
                    lstEstimateShippingAvailable.Add(iMethodId)
                    'If (memberId > 0) Then
                    htmlSelect = "<input type='radio' class='radio-node' id='rbtnShipping_" & iMethodId & "' name='shipping' onClick='ChangeEstimateShipping(" & iMethodId & ")' /><label class='radio-label shipping' for='rbtnShipping_" & iMethodId & "'>&nbsp;</label>"
                    htmlName = "<span onClick='ChangeEstimateShipping(" & iMethodId & ")'>" & name & "</span>" & calculateDateShipping(DateTime.Now, dr("Days"), True, False)
                    htmlRow = "<tr><td class='select'>" & htmlSelect & "</td><td class='name'>" & htmlName & "</td><td class='value'>" & htmlPrice & "</td></tr>"
                    'Else
                    '    htmlSelect = String.Empty
                    '    htmlName = "<span>" & name & "</span>"
                    '    htmlRow = "<tr><td class='select'>&nbsp;</td><td class='name' style='cursor:default;'>" & htmlName & "</td><td class='value'>" & htmlPrice & "</td></tr>"
                    'End If

                Else
                    Dim isFreeShip As Boolean = False
                    If iMethodId = Utility.Common.PickupShippingId Then
                        dPrices = 0
                    Else
                        If iMethodId = Utility.Common.DefaultShippingId Then
                            If (Utility.Common.CheckShippingSpecialUS("US", objShippingInfor.EstimateState)) Then
                                isFreeShip = False
                            Else
                                Dim strFreeShippingOrderAmount As String = SysParam.GetValue("FreeShippingOrderAmount")
                                If Not String.IsNullOrEmpty(strFreeShippingOrderAmount) Then
                                    Dim FreeShippingOrderAmount As Double = CDbl(strFreeShippingOrderAmount)
                                    If Cart.Order.SubTotal > FreeShippingOrderAmount Then
                                        isFreeShip = True
                                    End If
                                End If
                            End If

                            If objShippingInfor.EstimateTotalWeight = objShippingInfor.EstimateTotalFreeWeight Then
                                isFreeShip = True
                            End If

                            If isFreeShip Then
                                dPrices = 0
                            Else
                                dPrices = Cart.RecalculateShippingCartPrice("US", objShippingInfor.EstimateZipcode, iMethodId.ToString, Cart.Order.OrderId, code, objShippingInfor.EstimateTotalWeight, objShippingInfor.EstimateTotalFreeWeight, isFreeShip)
                                If dPrices <= 0 Then
                                    isTBD = True
                                End If
                            End If
                        Else
                            dPrices = Cart.RecalculateShippingCartPrice("US", objShippingInfor.EstimateZipcode, iMethodId.ToString, Cart.Order.OrderId, code, objShippingInfor.EstimateTotalWeight, objShippingInfor.EstimateTotalFreeWeight, False)
                            If dPrices <= 0 Then
                                isTBD = True
                            End If
                        End If

                    End If

                    If isFreeShip Then
                        Dim dFreePrice As Double = Cart.RecalculateShippingCartPrice("US", objShippingInfor.EstimateZipcode, iMethodId.ToString, Cart.Order.OrderId, code, objShippingInfor.EstimateTotalWeight, objShippingInfor.EstimateTotalFreeWeight, isFreeShip)
                        htmlPrice = "<span class=""redbold"">FREE!</span><br/>"
                        If dFreePrice > 0 Then
                            htmlPrice &= "<span class=""strike"">" & FormatCurrency(dFreePrice) & "</span>"
                        End If
                    Else
                        htmlPrice = FormatCurrency(dPrices.ToString)
                    End If

                    If Not isTBD Then
                        ''  Return String.Empty
                        lstEstimateShippingAvailable.Add(iMethodId)

                        ' If memberId > 0 Then
                        htmlSelect = String.Format("<input type='radio' id='rbtnShipping_{0}' class='radio-node' {1} name='shipping' onClick='ChangeEstimateShipping({0})' /><label class='radio-label shipping' for='rbtnShipping_{0}'>&nbsp;</label>", iMethodId, IIf(Not bFirstCheck, "checked='checked'", ""))
                        htmlName = "<span onClick='ChangeEstimateShipping(" & iMethodId & ")'>" & name & "</span>" + calculateDateShipping(DateTime.Now, dr("Days"), True, False)
                        htmlRow = "<tr><td class='select'>" & htmlSelect & "</td><td class='name'>" & htmlName & "</td><td class='value'>" & htmlPrice & "</td></tr>"
                        bFirstCheck = True
                        'Else
                        '    htmlSelect = String.Empty
                        '    htmlName = "<span>" & name & "</span>"
                        '    htmlRow = "<tr><td>&nbsp;</td><td class='name' style='cursor:default;'>" & htmlName & "</td><td class='value'>" & htmlPrice & "</td></tr>"
                        'End If
                    End If

                End If
                htmlResult &= htmlRow
            Next

            If Cart.Order.SubTotal >= CDbl(SysParam.GetValue("USTotalOrderResidential")) Then
                htmlResult &= String.Format("<tr><td></td><td colspan=""2"" class=""msg"">A {0:C2} signature confirmation charge will be added to all orders being delivered to a residential area as determined by shipping carrier.</td>", CDbl(ShipmentMethod.GetValue(Common.DefaultShippingId, Common.ShipmentValue.Signature)))
            Else
                htmlResult &= String.Format("<tr><td></td><td colspan=""2"" class=""msg"">A {0:C2} residential delivery charge will be added to all orders being delivered to a residential area as determined by shipping carrier.</td>", CDbl(ShipmentMethod.GetValue(Common.DefaultShippingId, Common.ShipmentValue.Residential)))
            End If

            ''Shipping Option: 
            ''Resindential
            'htmlResult &= String.Format("<tr><td class='checked'><div class=""checkbox""><label for=""chkShipping_Residential""><input type=""checkbox"" id=""chkShipping_Residential"" onClick=""CheckShipppingOption(this)"" /><span class=""checkbox-icon""></span></label></div></td><td class='nameresidential'><label for=""chkShipping_Residential"">Residential Address</label></td><td class='value'></td></tr>")

            ''Signature Confirmation
            'htmlResult &= String.Format("<tr><td class='checked'><div class=""checkbox""><label for=""chkShipping_Signature""><input type=""checkbox"" id=""chkShipping_Signature"" onClick=""CheckShipppingOption(this)"" /><span class=""checkbox-icon""></span></label></div></td><td class='namechecked'><label for=""chkShipping_Residential"">Signature Confirmation</label></td><td class='value'></td></tr>")

            ''Insurance
            'htmlResult &= String.Format("<tr><td class='checked'><div class=""checkbox""><label for=""chkShipping_Insurance""><input type=""checkbox"" id=""chkShipping_Insurance"" onClick=""CheckShipppingOption(this)"" /><span class=""checkbox-icon""></span></label></div></td><td class='namechecked'><label for=""chkShipping_Insurance"">Shipping Insurance</label></td><td class='value'></td></tr>")

            'If bCartItemFlammable Then
            '    htmlResult &= "<tr><td>&nbsp;</td><td class='msg' colspan='2'>" & Resources.Alert.ShippingNotAvailableFlammable & "</td></tr>"
            'End If
            'htmlResult &= "<tr><td></td><td></td><td class='minimize'><a href='javascript:void(0);' onclick='MinimazeShipping()'>Close</a></td></tr>"
            Return htmlResult & "</table>"
        End Function
        Private Function GetWeightForCalculateEstimateShipping(ByVal DB As Database, ByVal Cart As ShoppingCart, ByVal zipCode As String) As EstimateShippingInfor

            Dim EstimateTotalWeight As Double = 0
            Dim EstimateTotalFreeWeight As Double = 0
            Dim EstimateTotalFreightWeight As Double = 0
            Dim EstimateTotalFreighFreeWeight As Double = 0
            Dim EstimateZipcode As String = ""
            Dim orderId As Integer = Cart.Order.OrderId
            DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = (Select IsFreeShipping from StoreItem where ItemId=StoreCartItem.ItemId) WHERE Type = 'item' and PromotionId<1 AND OrderId = " & orderId)
            EstimateTotalWeight = 0
            EstimateTotalFreeWeight = 0
            EstimateTotalFreightWeight = 0
            EstimateTotalFreighFreeWeight = 0
            EstimateZipcode = String.Empty
            Dim dv As DataView, drv As DataRowView


            Dim Sql As String = String.Empty
            If Cart.HasOversizeItems() Then
                Sql = "select sm.Code, coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & orderId & " and sci.IsOversize = 1 and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem ca inner join shipmentmethod sm on sm.MethodId = ca.carriertype where orderid = " & orderId & " and  type = 'item' and ca.IsOversize=1 group by carriertype, sm.Code"
                dv = DB.GetDataView(Sql)
                If Not dv Is Nothing Then
                    If dv.Count > 0 Then
                        drv = dv(0)
                        EstimateTotalFreighFreeWeight = drv("freeweight")
                        EstimateTotalFreightWeight = drv("weight")
                    End If
                End If
                ''get weight 
                Dim addCondition As String = " and IsOversize<>1"
                Sql = "select sm.Code, coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & orderId & " and IsOversize<>1 and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem ca inner join shipmentmethod sm on sm.MethodId = ca.carriertype where orderid = " & orderId & " and  type = 'item' " & addCondition & " group by carriertype, sm.Code"
                dv = DB.GetDataView(Sql)
                Dim IsOverSize As Boolean = False
                If Not dv Is Nothing Then
                    If dv.Count > 0 Then
                        For i As Integer = 0 To dv.Count - 1
                            drv = dv(i)
                            EstimateTotalFreeWeight = EstimateTotalFreeWeight + drv("freeweight")
                            EstimateTotalWeight = EstimateTotalWeight + drv("weight")
                        Next
                    End If
                End If

            Else

                ''get weight 
                Dim addCondition As String = " and CarrierType<>" & Utility.Common.TruckShippingId
                Sql = "SELECT sm.Code, COALESCE(SUM(weight * quantity),0) AS Weight, (SELECT COALESCE(SUM(weight * quantity),0) AS weight FROM StoreCartItem sci WHERE (IsFreeShipping = 1 OR (SELECT IsFreeShipping FROM StoreItem WHERE ItemId = sci.ItemId) = 1) AND orderid = " & orderId & " and sci.carriertype = ca.carriertype and type = 'item' and isfreeitem = 0) as FreeWeight from storecartitem ca inner join shipmentmethod sm on sm.MethodId = ca.carriertype where orderid = " & orderId & " and  type = 'item' " & addCondition & " group by carriertype, sm.Code"
                dv = DB.GetDataView(Sql)
                Dim IsOverSize As Boolean = False
                If Not dv Is Nothing Then
                    If dv.Count > 0 Then
                        For i As Integer = 0 To dv.Count - 1
                            drv = dv(i)
                            EstimateTotalFreeWeight = EstimateTotalFreeWeight + drv("FreeWeight")
                            EstimateTotalWeight = EstimateTotalWeight + drv("Weight")
                        Next
                    End If
                End If
            End If

            Dim strZipcode As String = Utility.Common.GetSingleZipcode(zipCode)
            If strZipcode.Length = 5 Then
                If IsNumeric(strZipcode) = False Then
                    EstimateZipcode = String.Empty
                Else
                    EstimateZipcode = Cart.GetZipCodeSpecialUS(strZipcode)
                End If
            End If

            Dim result As New EstimateShippingInfor
            result.EstimateTotalWeight = EstimateTotalWeight
            result.EstimateTotalFreeWeight = EstimateTotalFreeWeight
            result.EstimateTotalFreightWeight = EstimateTotalFreightWeight
            result.EstimateTotalFreighFreeWeight = EstimateTotalFreighFreeWeight
            result.EstimateZipcode = EstimateZipcode
            Return result
        End Function
        Public Function ChangeCountryEstimateShipping(ByVal country As String, ByVal zipCode As String) As Object
            HttpContext.Current.Session.Remove("GetFedExRate")
            Dim strError As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)


                Dim sp As New SitePage()
                sp.Cart.DB = DB

                If country <> "US" Then
                    Dim flammable As Common.FlammableCart = sp.Cart.HasFlammableCartItem()
                    If flammable = Common.FlammableCart.BlockedHazMat Then
                        strError = Resources.Alert.CartItemBlockedHazMat
                    ElseIf flammable = Common.FlammableCart.HazMat AndAlso Not sp.Cart.HasCountryHazMat(country) Then
                        strError = Resources.Alert.EstimateCountryBlockedHazMat
                    Else
                        Dim dShipping As Double = 0
                        Dim htmlShipping As String = String.Empty
                        Dim shippingName As String = "International Shipping"
                        dShipping = CDbl(sp.Cart.RecalculateCartSelectShippingInternational(country, zipCode, Utility.Common.USPSPriorityShippingId, True))
                        If dShipping = 0 Then

                            If sp.Cart.HasOversizeItems Then
                                htmlShipping = "<tr>" & vbCrLf &
                                                        "<td class='left'></td><td colspan='2' class=""label-text"">" & sp.Cart.GetShippingTBDLine(Utility.Common.FreightDeliveryShippingName) & "</td><td class='right'></td>" & vbCrLf &
                                                        "</tr>"
                            Else
                                htmlShipping = "<tr>" & vbCrLf &
                                                       "<td class='left'></td><td colspan='2' class=""label-text"">" & sp.Cart.GetShippingTBDLine(shippingName) & "</td><td class='right'></td>" & vbCrLf &
                                                       "</tr>"
                            End If
                        Else

                            htmlShipping = "<tr>" & vbCrLf &
                                                        "<td class='left'></td><td class=""label-text"">" & shippingName & "</td>" & vbCrLf &
                                                        "<td class=""label-data"" >" & FormatCurrency(dShipping) & "</td><td class='right'></td>" & vbCrLf &
                                                        "</tr>"

                            If sp.Cart.HasOversizeItems Then
                                htmlShipping = htmlShipping & "<tr>" & vbCrLf &
                                                         "<td class='left'></td><td colspan='2' class=""label-text"">" & sp.Cart.GetShippingTBDLine(Utility.Common.FreightDeliveryShippingName) & "</td><td class='right'></td>" & vbCrLf &
                                                         "</tr>"

                            End If
                        End If
                        sp.Cart.Order.CarrierType = Utility.Common.USPSPriorityShippingId()
                        sp.Cart.Order.Total = sp.Cart.Order.SubTotal + dShipping
                        sp.Cart.Order.Tax = 0

                        HttpContext.Current.Session("cartRenderShipping") = htmlShipping
                    End If
                End If

                If String.IsNullOrEmpty(strError) AndAlso Not (country = "US" AndAlso zipCode = "") Then
                    HttpContext.Current.Session("CartRenderEstimate") = "1"
                End If

                HttpContext.Current.Session("CartRender") = sp.Cart
                HttpContext.Current.Session("EstimateShipping") = country & "|" & zipCode

                htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                HttpContext.Current.Session.Remove("CartRender")
                HttpContext.Current.Session.Remove("cartRenderShipping")
                HttpContext.Current.Session.Remove("CartRenderEstimate")
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(htmlSumarryBox)
            result(1) = New HtmlString(strError)
            Return result

        End Function

        Public Function OpenFreeSamples() As Object
            Dim strError As String = String.Empty
            Dim htmlFreeSamples As String = String.Empty

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Try
                htmlFreeSamples = Utility.Common.RenderUserControl("~/controls/product/free-samples.ascx")
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(htmlFreeSamples)
            result(1) = New HtmlString(strError)
            Return result
        End Function

        Public Function OpenFreeGift(ByVal LevelId As String) As Object
            Dim strError As String = String.Empty
            Dim htmlFreeGift As String = String.Empty

            Dim orderId As Integer = Common.GetCurrentOrderId()
            Try
                HttpContext.Current.Session("FreeGiftLevelRender") = orderId & "|" & LevelId
                htmlFreeGift = Utility.Common.RenderUserControl("~/controls/product/free-gift.ascx")
                HttpContext.Current.Session.Remove("FreeGiftLevelRender")
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(2) {}
            result(0) = New HtmlString(htmlFreeGift)
            result(1) = New HtmlString(strError)
            Return result
        End Function
        Private Sub UpdateOrderToInternationalAddress(ByVal DB As Database, ByVal objOrder As StoreOrderRow, ByVal countryCode As String, ByVal memberId As Integer)
            'HttpContext.Current.Session("IsInternational") = "Y"
            HttpContext.Current.Session("isUpdateAddress") = 1
            If objOrder.ShippingAddressId > 0 Then
                Dim currentShippingAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, objOrder.ShippingAddressId)
                If currentShippingAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString Then
                    objOrder.BillingAddressId = currentShippingAddress.AddressId
                    If countryCode = "HI" OrElse countryCode = "AK" Then
                        currentShippingAddress.State = countryCode
                        currentShippingAddress.Country = "US"
                    Else
                        currentShippingAddress.Country = countryCode
                    End If

                Else
                    ''get billing address
                    Dim defaultBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, memberId, Utility.Common.MemberAddressType.Billing.ToString())
                    objOrder.BillingAddressId = defaultBillingAddress.AddressId
                    If countryCode = "HI" OrElse countryCode = "AK" Then
                        defaultBillingAddress.State = countryCode
                        defaultBillingAddress.Country = "US"
                        currentShippingAddress.State = countryCode
                        currentShippingAddress.Country = "US"
                    Else
                        defaultBillingAddress.Country = countryCode
                        currentShippingAddress.Country = countryCode
                    End If

                    If defaultBillingAddress.AddressId > 0 Then
                        defaultBillingAddress.DB = DB
                        defaultBillingAddress.Update()
                    End If
                End If

                If currentShippingAddress.AddressId > 0 Then
                    currentShippingAddress.DB = DB
                    currentShippingAddress.Update()
                End If
            Else
                Dim defaultBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, memberId, Utility.Common.MemberAddressType.Billing.ToString())
                Dim defaultShippingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, memberId, Utility.Common.MemberAddressType.Shipping.ToString())
                If countryCode = "HI" OrElse countryCode = "AK" Then
                    defaultBillingAddress.State = countryCode
                    defaultBillingAddress.Country = "US"
                    defaultShippingAddress.State = countryCode
                    defaultShippingAddress.Country = "US"
                Else
                    defaultBillingAddress.Country = countryCode
                    defaultShippingAddress.Country = countryCode
                End If

                If (defaultBillingAddress.AddressId > 0) Then
                    defaultBillingAddress.DB = DB
                    defaultBillingAddress.Update()
                End If
                If (defaultShippingAddress.AddressId > 0) Then
                    defaultShippingAddress.DB = DB
                    defaultShippingAddress.Update()
                End If
                objOrder.ShippingAddressId = defaultShippingAddress.AddressId
                objOrder.BillingAddressId = defaultBillingAddress.AddressId
            End If

            ''update order address
            objOrder.IsSameAddress = True
            objOrder.BillToCountry = countryCode
            objOrder.ShipToCountry = countryCode
            Dim sql As String = String.Empty
            'sql = "Update StoreOrder set IsSameAddress=1,"
            'sql = sql & "BillToCountry='" & countryCode & "',ShipToCountry='" & countryCode & "'"
            'sql = sql & " where OrderId=" & objOrder.OrderId
            sql = "Update Member set IsSameDefaultAddress=1 where MemberId=" & memberId
            DB.ExecuteSQL(sql)
        End Sub

        Private Sub CalculateShippingSpecialUS(ByVal DB As Database, ByVal Cart As ShoppingCart, ByVal country As String, ByVal memberId As Integer, ByVal strZipcode As String, ByVal CarrierType As Integer)

            UpdateOrderToInternationalAddress(DB, Cart.Order, country, memberId)
            Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, strZipcode)
            strZipcode = Cart.GetZipCodeSpecialUS(strZipcode)
            Cart.Order.CarrierType = 15
            Cart.Order.BillToCountry = country
            Cart.Order.ShipToCountry = country
            Cart.Order.BillToCounty = strZipcode
            Cart.Order.ShipToCounty = strZipcode
            Cart.Order.BillToZipcode = strZipcode
            Cart.Order.ShipToZipcode = strZipcode
            Cart.Order.TotalSpecialHandlingFee = 0
            Cart.Order.IsFreeShipping = False

            'GetShippingCharges
            Cart.Order.Update()
            Dim Sql As String = "update storecartitem set carriertype=" & Cart.Order.CarrierType & " where IsOversize=0 and type='item' and orderid = " & Cart.Order.OrderId
            DB.ExecuteSQL(Sql)
            Sql = "update storecartitem set carriertype=" & Utility.Common.TruckShippingId & ",LiftGateCharge=0,InsideDeliveryCharge=0,ScheduleDeliveryCharge=0,isliftgate=0,IsInsideDelivery=0,isscheduledelivery=0"
            Sql += " where IsOversize=1 and type='item' and orderid = " & Cart.Order.OrderId
            Sql += "; update storecartitem set IsFreeShipping=0 where orderid = " & Cart.Order.OrderId
            DB.ExecuteSQL(Sql)
            'Cart.RecalculateOrderDetail("CalculateShippingSpecialUS") 'Khoa Cart.RecalculateAllOrderValues()

            Dim dShipping As Double = 0
            dShipping = CDbl(Cart.RecalculateCartSelectShippingInternational(country, country, "15", False))
            Dim htmlShipping As String = Cart.GetShippingLinesPrices(country, "", "")
            Cart.Order.SubTotal = Cart.Order.SubTotal + dShipping - IIf(Cart.Order.IsPromotionValid, Cart.Order.Discount, 0)
            HttpContext.Current.Session("CartRender") = Cart
            HttpContext.Current.Session("cartRenderShipping") = htmlShipping
        End Sub


        Public Function CheckShipppingOption(ByVal chk As String, ByVal flag As Boolean) As Object
            Dim strError As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim memberId As Integer = 0
            Dim orderId As Integer = 0

            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(htmlSumarryBox)
            result(1) = New HtmlString(strError)

            Return result
        End Function
        Public Function ChangeEstimateShipping(ByVal MethodID As Integer, ByVal countryCode As String, ByVal zipCode As String) As Object
            Dim strError As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty
            Dim orderId As Integer = Common.GetCurrentOrderId()
            HttpContext.Current.Session.Remove("GetFedExRate")

            Dim dTotalSpecialHandlingFee As Double = 0
            Dim dTax As Double = 0
            Dim dFreightShipping As Double = 0
            Dim dTotal As Double = 0
            Dim dShippingFee As Double = 0
            Dim isFreeShipping As Boolean = False
            Dim dWeightNotTruck As Double = 0
            Try
                If String.IsNullOrEmpty(zipCode) Then
                    strError = "Please input your Zip Code"
                    Exit Try
                End If

                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)


                Dim Cart As New ShoppingCart(DB)
                Dim o As StoreOrderRow = Cart.Order
                Utility.Common.DeleteCachePopupCart(orderId)

                Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, zipCode)
                If (MethodID = Utility.Common.USPSPriorityShippingId) Then
                    Dim countryShip As String = o.ShipToCountry
                    If countryCode <> "US" Then
                        o.IsSameAddress = True
                    End If
                End If

                o.ShipToCountry = countryCode
                o.CarrierType = MethodID.ToString()
                o.IsFreeShipping = False

                Dim flammable As Common.FlammableCart = Cart.HasFlammableCartItem()
                If flammable = Common.FlammableCart.BlockedHazMat AndAlso Cart.IsHazardousMaterialFee(o.CarrierType) Then
                    strError = Resources.Alert.CartItemBlockedHazMat
                ElseIf flammable = Common.FlammableCart.HazMat AndAlso Not Cart.HasCountryHazMat(o.ShipToCountry) Then 'Or Cart.CheckShippingSpecialUS()) Then
                    strError = Resources.Alert.CountryBlockedHazMat
                Else
                    Dim htmlShipping As String = String.Empty
                    If MethodID = Utility.Common.USPSPriorityShippingId Then
                        htmlShipping = Cart.GetShippingLineDetails()
                    Else
                        If dbZipCode.StateCode = "IL" Then
                            dTax = Cart.GetCalculateTaxPrices(zipCode)
                        End If

                        If (MethodID = Utility.Common.DefaultShippingId) Then
                            dTotalSpecialHandlingFee = DB.ExecuteScalar("SELECT COALESCE(SUM(dbo.[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType)*Quantity),0) from StoreCartItem WHERE OrderId=" & orderId & " AND [Type] <> 'carrier' AND IsOversize = 0 AND CarrierType=" & Utility.Common.DefaultShippingId & " AND Weight >= 1")
                        Else
                            dTotalSpecialHandlingFee = 0
                        End If

                        Dim dWeightFreightDelivery As Double = 0


                        If MethodID = Utility.Common.PickupShippingId Then
                            htmlShipping = Cart.BuildUSShippingFee(MethodID, dShippingFee, isFreeShipping)
                        ElseIf MethodID <> Utility.Common.TruckShippingId Then
                            'Free Shipping
                            If MethodID = Utility.Common.DefaultShippingId Then
                                If Not String.IsNullOrEmpty(o.PromotionCode) Then
                                    Dim row As StorePromotionRow = StorePromotionRow.CheckCoupon(o.PromotionCode, o.OrderId)
                                    If row.PromotionId > 0 Then
                                        isFreeShipping = row.IsFreeShipping
                                    End If
                                End If

                                If Not isFreeShipping Then
                                    Dim FreeShippingOrderAmount As Double = CDbl(SysParam.GetValue("FreeShippingOrderAmount"))
                                    Dim PriceTotal As Double = o.SubTotal ''GetPriceTotal()             
                                    isFreeShipping = PriceTotal > FreeShippingOrderAmount 'Or dWeightNotTruck = 0
                                End If
                            End If

                            'Phi shipping
                            dWeightNotTruck = Cart.GetWeightTotal(MethodID = Utility.Common.DefaultShippingId, isFreeShipping)
                            If dWeightNotTruck > 0 Then
                                dShippingFee = BaseShoppingCart.GetFedExRate(DB, dWeightNotTruck, MethodID, zipCode, countryCode, o.ShipToAddressType <> 1)
                                If (dShippingFee > 0) Then
                                    Dim extraPercent As Integer = DB.ExecuteScalar("Select coalesce(ExtraShippingPercent,0) from ShipmentMethod where MethodId=" & MethodID)
                                    dShippingFee += (dShippingFee * extraPercent) / 100
                                End If
                            End If

                            'Check free shipping 1 lan nua neu total weight=0
                            If Not isFreeShipping AndAlso dWeightNotTruck = 0 Then
                                isFreeShipping = True
                            End If

                            htmlShipping = Cart.BuildUSShippingFee(MethodID, dShippingFee, isFreeShipping)
                        Else
                            dWeightFreightDelivery = Cart.GetWeightTotal(MethodID = Utility.Common.DefaultShippingId)
                        End If

                        If MethodID <> Utility.Common.PickupShippingId Then
                            dWeightFreightDelivery += Cart.GetWeightTruckTotal()
                            Dim IsFreightDelivery As Boolean = dWeightFreightDelivery > 0
                            If IsFreightDelivery Then
                                dFreightShipping = Cart.RecalculateShippingCartPrice("US", zipCode, Utility.Common.TruckShippingId, orderId, "TRUCK", dWeightFreightDelivery, 0, False)
                                htmlShipping &= Cart.BuildFreightDelivery(dFreightShipping)
                            End If
                        End If
                    End If


                    HttpContext.Current.Session.Remove("GetFedExRate")
                    o.Total = o.SubTotal + dTotalSpecialHandlingFee + dTax + IIf(isFreeShipping, 0, dShippingFee) + dFreightShipping
                    o.TotalSpecialHandlingFee = dTotalSpecialHandlingFee
                    o.Tax = dTax
                    o.Shipping = dShippingFee
                    HttpContext.Current.Session("CartRenderShipping") = htmlShipping

                    DB.Close()
                End If

                If String.IsNullOrEmpty(strError) Then
                    HttpContext.Current.Session("CartRenderEstimate") = "1"
                End If

                HttpContext.Current.Session("CartRender") = Cart

                Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)
                DB.Close()
                htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                HttpContext.Current.Session.Remove("CartRender")
                HttpContext.Current.Session.Remove("CartRenderShipping")
                HttpContext.Current.Session.Remove("CartRenderEstimate")
            Catch ex As Exception
                strError = ex.Message
            End Try

            Utility.Common.OrderLog(orderId, "Estimate Shipping > Choose shipping method", {"MethodID=" & MethodID, "Country=" & countryCode, "ZipCode=" & zipCode})
            Dim result As Object() = New Object(1) {}
            result(0) = New HtmlString(htmlSumarryBox)
            result(1) = New HtmlString(strError)

            Return result
        End Function

        Public Function GetFreeGiftByTotal(ByVal levelId As Integer) As Object

            Dim strError As String = String.Empty
            Dim htmlFreeGiftList As String = String.Empty
            Dim freeGiftLevel As String = String.Empty
            Dim orderId As Integer = Common.GetCurrentOrderId()
            Dim linkRedirect As String = String.Empty
            Try
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                Dim lstFreeGift As StoreItemCollection = StoreItemRow.GetFreeGiftColectionByLevel(orderId, levelId)
                Dim sp As New SitePage()
                If Not sp.Cart Is Nothing Then
                    HttpContext.Current.Session("CartRender") = sp.Cart
                End If
                HttpContext.Current.Session("ListFreeGiftRender") = lstFreeGift
                HttpContext.Current.Session("FreeGiftLevelGender") = levelId
                htmlFreeGiftList = Utility.Common.RenderUserControl("~/controls/product/free-item-list.ascx")
                freeGiftLevel = Utility.Common.RenderUserControl("~/controls/checkout/free-gift-level.ascx")
                HttpContext.Current.Session.Remove("ListFreeGiftRender")
                HttpContext.Current.Session.Remove("FreeGiftLevelGender")
                HttpContext.Current.Session.Remove("CartRender")
                DB.Close()
            Catch ex As Exception
                strError = "An error occurred while loading data"
            End Try
            Dim result As Object() = New Object(3) {}
            result(0) = New HtmlString(strError)
            result(1) = New HtmlString(htmlFreeGiftList)
            result(2) = New HtmlString(freeGiftLevel)
            result(3) = linkRedirect
            Return result
        End Function
        Public Function AddCartFreeGift(ByVal itemId As Object) As Object
            Dim cartCount As Integer = 0
            Dim prevFreeGiftid As Integer = 0
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
            Dim sp As New SitePage()

            Dim isCheckLogin As Boolean = False
            If HttpContext.Current.Request.UrlReferrer.PathAndQuery.Contains("act=checkout") Then
                isCheckLogin = True
            End If

            If (memberId < 1 AndAlso isCheckLogin = True) Then
                linkRedirect = "/members/login.aspx"
                sp.SetLastWebsiteURL(HttpContext.Current.Request.UrlReferrer.PathAndQuery)
            Else
                Try
                    Dim DB As New Database()
                    DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    sp.Cart.DB = DB
                    ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                    Dim freeGiftLevel As Integer = DB.ExecuteScalar("SELECT TOP 1 LevelId FROM FreeGift FG INNER JOIN FreeGiftLevel FL ON FL.Id = FG.LevelId AND FL.IsActive = 1 WHERE ItemId=" & itemId & " AND FG.IsActive = 1 ORDER BY FL.MinValue ASC")
                    If Not FreeGiftLevelRow.CheckAllowAddCart(orderId, freeGiftLevel) Then
                        Dim objMinLevel As Double = CDbl(DB.ExecuteScalar("Select MinValue from FreeGiftLevel where Id=" & freeGiftLevel))
                        strError = "To receive this free gift, your order must be $" & objMinLevel.ToString() & " or more. Your order subtotal is $" & sp.Cart.Order.SubTotal
                    Else
                        prevFreeGiftid = StoreCartItemRow.DeleteFreeItemGift(orderId)
                        Dim cartItemId As Integer = sp.Cart.Add2Cart(itemId, Nothing, 1, Nothing, "FreeGift", Nothing, Nothing, Nothing, Nothing, False, True, Nothing)

                    End If
                    cartCount = sp.Cart.GetCartItemCount()
                    DB.Close()
                Catch ex As Exception
                    strError = ex.Message
                End Try
            End If
            Dim result As Object() = New Object(4) {}
            result(0) = New HtmlString(strError)
            result(1) = linkRedirect
            result(2) = cartCount
            result(3) = prevFreeGiftid
            result(4) = itemId
            Return result

        End Function

        Public Function AddFreeGift(ByVal itemId As Object) As Object
            Dim cartCount As Integer = 0
            Dim strError As String = String.Empty
            Dim linkRedirect As String = String.Empty
            Dim isOK As Integer = 0
            Dim htmlEstimatePopup As String = String.Empty
            Dim htmlCart As String = String.Empty
            Dim htmlSumarryBox As String = String.Empty

            Dim sp As New SitePage()
            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()

            Try
                Dim prevFreeGiftid As Integer = 0
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                sp.Cart.DB = DB
                ShoppingCart.GetShoppingCartFromCookie(DB, False, sp.Cart)
                Dim freeGiftLevel As Integer = DB.ExecuteScalar("SELECT TOP 1 LevelId FROM FreeGift FG INNER JOIN FreeGiftLevel FL ON FL.Id = FG.LevelId AND FL.IsActive = 1 WHERE ItemId=" & itemId & " AND FG.IsActive = 1 ORDER BY FL.MinValue ASC")
                If Not FreeGiftLevelRow.CheckAllowAddCart(orderId, freeGiftLevel) Then
                    Dim objMinLevel As Double = CDbl(DB.ExecuteScalar("Select MinValue from FreeGiftLevel where Id=" & freeGiftLevel))
                    strError = "To receive this free gift, your order must be $" & objMinLevel.ToString() & " or more. Your order subtotal is $" & sp.Cart.Order.SubTotal
                Else
                    prevFreeGiftid = StoreCartItemRow.DeleteFreeItemGift(orderId)
                    Dim cartItemId As Integer = sp.Cart.Add2Cart(itemId, Nothing, 1, Nothing, "FreeGift", Nothing, Nothing, Nothing, Nothing, False, True, Nothing)
                    sp.Cart.RecalculateOrderUpdate()
                End If

                Dim countCartRow As Integer = StoreCartItemRow.CountCartRow(DB, orderId)
                If (countCartRow > Utility.ConfigData.MaxCartCount) Then 'Must redirect  becauce render very slowly
                    linkRedirect = "/store/cart.aspx"
                Else
                    'Update success
                    Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)

                    'Render cart HTML
                    Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                    HttpContext.Current.Session("CartRender") = objCart
                    htmlCart = Utility.Common.RenderUserControl("~/controls/checkout/cart.ascx")
                    cartItemCount = sp.Cart.GetCartItemCount()

                    If HttpContext.Current.Session("EstimateShipping") IsNot Nothing Then
                        Dim arr As String() = HttpContext.Current.Session("EstimateShipping").ToString().Split("|")
                        Dim r As Object()
                        If arr(0) <> "US" Then
                            r = ChangeCountryEstimateShipping(arr(0), "")
                            htmlSumarryBox = r(0).ToString()
                        ElseIf arr(0) = "US" AndAlso Not String.IsNullOrEmpty(arr(1)) Then
                            r = CalEstimateShipping(arr(0), arr(1))
                            htmlEstimatePopup = r(0).ToString()
                            htmlSumarryBox = r(2).ToString()
                        Else
                            htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                        End If

                        If r IsNot Nothing AndAlso r(1) IsNot Nothing AndAlso Not String.IsNullOrEmpty(r(1).ToString()) Then
                            If Not String.IsNullOrEmpty(strError) Then
                                strError &= "<br><br>"
                            End If
                            strError = r(1).ToString()
                        End If
                    Else
                        htmlSumarryBox = Utility.Common.RenderUserControl("~/controls/checkout/cart-summary.ascx")
                    End If

                    HttpContext.Current.Session.Remove("CartRender")
                    isOK = 1
                End If

                cartCount = sp.Cart.GetCartItemCount()
                DB.Close()
            Catch ex As Exception
                strError = ex.Message
            End Try

            Dim result As Object() = New Object(6) {}
            result(0) = New HtmlString(htmlCart)
            result(1) = New HtmlString(htmlEstimatePopup)
            result(2) = New HtmlString(htmlSumarryBox)
            result(3) = New HtmlString(strError)
            result(4) = linkRedirect
            result(5) = cartCount
            result(6) = isOK
            Return result

        End Function
#End Region
#End Region

#Region "Search"
        Public Function ViewMoreResultArticleByType(ByVal keyword As Object, ByVal type As Object, ByVal pageIndex As Object, ByVal pageSize As Object) As Object

            Dim luceneHelper As New LuceneHelper
            Dim strError As String = String.Empty
            Dim htmlArticle As String = String.Empty
            Dim result As Object() = New Object(2) {}
            Dim isMore As Integer = 0
            Dim TotalRelatedArticle As Integer = 0
            Dim lst As RelatedArticleCollection = luceneHelper.SearchArticleInLuceneByType(keyword, CInt(pageIndex), CInt(pageSize), TotalRelatedArticle, CInt(type), True)
            If lst.Count > 0 Then
                Dim countItemIndex As Integer = ((pageIndex - 1) * pageSize)
                If ((countItemIndex + lst.Count) < TotalRelatedArticle) Then
                    isMore = 1
                End If


                If Not lst Is Nothing AndAlso lst.Count > 0 Then
                    Dim rowData As String = String.Empty
                    Dim link As String = String.Empty
                    Dim luceneData As String = String.Empty
                    For Each objArticle As RelatedArticleRow In lst
                        rowData = CreateArticleHTMLRow(objArticle, keyword, luceneHelper, True)
                        htmlArticle = htmlArticle & rowData
                    Next
                    If (pageIndex = 1) Then
                        htmlArticle = "<ul>" & htmlArticle & "</ul>"
                    Else
                        htmlArticle = htmlArticle
                    End If

                End If
            End If

            result(0) = New HtmlString(htmlArticle)
            result(1) = type
            result(2) = isMore
            Return result
        End Function

        Public Function SearchArticle(ByVal keyword As Object, ByVal countProduct As Object, ByVal countVideo As Object, ByVal rawURL As Object) As Object
            Dim luceneHelper As New LuceneHelper
            Dim htmlArticle As String = String.Empty
            Dim htmlTab As String = String.Empty
            Dim pageTitle As String = String.Empty
            Dim result As Object() = New Object(2) {}
            Dim TotalRelatedNew As Integer = 0
            Dim TotalRelatedTip As Integer = 0
            Dim TotalRelatedMedia As Integer = 0
            Dim lstNews As RelatedArticleCollection = luceneHelper.SearchArticleInLuceneByType(keyword, 1, Utility.ConfigData.DefaultTopSearchNewsEvent, TotalRelatedNew, Utility.Common.ArticleType.News, True)
            Dim lstTip As RelatedArticleCollection = luceneHelper.SearchArticleInLuceneByType(keyword, 1, Utility.ConfigData.DefaultTopSearchTip, TotalRelatedTip, Utility.Common.ArticleType.Tips, True)
            Dim lstMediaPress As RelatedArticleCollection = luceneHelper.SearchArticleInLuceneByType(keyword, 1, Utility.ConfigData.DefaultTopSearchMediaPress, TotalRelatedMedia, Utility.Common.ArticleType.MediaPress, True)
            Dim htmlTipData As String = BindArticleData(luceneHelper, lstTip, keyword, Utility.ConfigData.DefaultTopSearchTip, TotalRelatedTip)
            Dim htmlNewData As String = BindArticleData(luceneHelper, lstNews, keyword, Utility.ConfigData.DefaultTopSearchNewsEvent, TotalRelatedNew)
            Dim htmlMediaPressData As String = BindArticleData(luceneHelper, lstMediaPress, keyword, Utility.ConfigData.DefaultTopSearchMediaPress, TotalRelatedMedia)
            If Not String.IsNullOrEmpty(htmlTipData) Then
                htmlArticle = "<div class='group-article' id='dvTip'><div class='name'>Expert Tips & Advice</div>" & htmlTipData & "</div>"
            End If
            If Not String.IsNullOrEmpty(htmlNewData) Then
                htmlArticle = htmlArticle & "<div class='group-article' id='dvNew'><div class='name'>News & Events</div>" & htmlNewData & "</div>"
            End If
            If Not String.IsNullOrEmpty(htmlMediaPressData) Then
                htmlArticle = htmlArticle & "<div class='group-article' id='dvMediaPress'><div class='name'>Media Press</div>" & htmlMediaPressData & "</div>"
            End If
            HttpContext.Current.Session("ActiveTabRender") = "article"
            HttpContext.Current.Session("CountProductGender") = countProduct
            HttpContext.Current.Session("CountVideoGender") = countVideo
            HttpContext.Current.Session("CountArticleGender") = TotalRelatedMedia + TotalRelatedNew + TotalRelatedTip
            HttpContext.Current.Session("KeywordRender") = keyword
            HttpContext.Current.Session("RawURLTabRender") = rawURL
            htmlTab = Utility.Common.RenderUserControl("~/controls/layout/tab-search.ascx")
            HttpContext.Current.Session("ActiveTabRender") = Nothing
            HttpContext.Current.Session("CountProductGender") = Nothing
            HttpContext.Current.Session("CountVideoGender") = Nothing
            HttpContext.Current.Session("CountArticleGender") = Nothing
            HttpContext.Current.Session("KeywordRender") = Nothing
            HttpContext.Current.Session("RawURLTabRender") = Nothing
            result(0) = New HtmlString(htmlTab)
            result(1) = New HtmlString(htmlArticle)
            pageTitle = " Search result for" & Chr(34) & "<span class='kw'>" & keyword & "</span>" & Chr(34)
            result(2) = New HtmlString(pageTitle)
            Return result
        End Function
        Private Function CreateArticleHTMLRow(ByVal objArticle As RelatedArticleRow, ByVal keyword As String, ByVal luceneHelper As LuceneHelper, ByVal showDesc As Boolean) As String
            Dim luceneData As String = luceneHelper.HightLightSearchKeyword(objArticle.Title, keyword)
            Dim link As String = String.Empty
            If (objArticle.Type = Utility.Common.ArticleType.News) Then
                link = URLParameters.NewsDetailUrl(objArticle.Title, objArticle.Id)
            ElseIf (objArticle.Type = Utility.Common.ArticleType.Tips) Then
                link = URLParameters.TipDetailUrl(objArticle.Title, objArticle.Id)
            Else
                link = URLParameters.MediaDetailUrl(objArticle.Title, objArticle.Id)
            End If
            Dim result As String = "<li><a href='" & link & "'>" & luceneData & "</a></li>"
            If showDesc AndAlso Not String.IsNullOrEmpty(objArticle.ShortDescription) Then
                luceneData = luceneHelper.HightLightSearchKeyword(objArticle.ShortDescription, keyword)
                result = result & "<li class='desc'>" & luceneData & "</li>"
            End If
            Return result
        End Function
        Private Function BindArticleData(ByVal luceneHelper As LuceneHelper, ByVal source As RelatedArticleCollection, ByVal keyword As String, ByVal defaultTop As Integer, ByVal total As Integer) As String
            If source Is Nothing Then
                Return String.Empty
            End If
            If source.Count < 1 Then
                Return String.Empty
            End If
            Dim index As Integer = 0
            Dim result As String = String.Empty
            Dim rowData As String = String.Empty
            Dim link As String = String.Empty
            Dim luceneData As String = String.Empty
            For Each objArticle As RelatedArticleRow In source
                If index < defaultTop Then
                    index = index + 1
                    rowData = CreateArticleHTMLRow(objArticle, keyword, luceneHelper, False)
                    result = result & rowData
                End If
            Next
            result = "<ul>" & result & "</ul>"
            If total > source.Count Then
                result = result & "<div class='viewmore'><a onclick=ViewMoreResultArticle(" & source(0).Type & ") href='javascript:void(0);'>View All</a></div>"
            End If
            Return result
        End Function

#End Region
        Public Function deleteRecentlyViewItem(ByVal itemId As String, ByVal itemName As String) As String()
            Dim DB As New Database()
            Try
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                Dim i As Integer = 0
                If itemId.Trim() <> "0" Then
                    i = DB.ExecuteSQL(String.Format("delete ViewedItem where itemid={0} and (({2} > 0 and memberid = {2} ) or ({2} = 0 and SessionNo = '{1}'))", itemId, HttpContext.Current.Session.SessionID, Utility.Common.GetCurrentMemberId()))
                Else
                    i = DB.ExecuteSQL(String.Format("delete ViewedItem where KeywordName='{0}' and (({2} > 0 and memberid = {2} ) or ({2} = 0 and SessionNo = '{1}'))", itemName, HttpContext.Current.Session.SessionID, Utility.Common.GetCurrentMemberId()))
                End If

                If i > 0 Then
                    Return New String() {itemId}
                Else
                    Return New String() {""}
                End If
            Catch ex As Exception
                Return New String() {"error:" + ex.ToString()}
            End Try
            Return New String() {"empty"}
        End Function
        Public Shared Sub logOrderDevice(ByVal OrderId As Integer, ByVal UserAgent As String)
            Try
                Dim manager = HttpContext.Current.Cache("WURFL")
                If manager Is Nothing Then
                    Dim WurflDataFilePath = "~/App_Data/wurfl-latest.zip"
                    Dim WurflPatchFilePath = "~/App_Data/web_browsers_patch.xml"
                    Dim wurflDataFile = HttpContext.Current.Server.MapPath(WurflDataFilePath)
                    Dim wurflPatchFile = HttpContext.Current.Server.MapPath(WurflPatchFilePath)
                    Dim configurer = New WURFL.Config.InMemoryConfigurer().MainFile(wurflDataFile).PatchFile(wurflPatchFile)
                    manager = WURFL.WURFLManagerBuilder.Build(configurer)
                    HttpContext.Current.Cache("WURFL") = manager
                End If

                Dim device As WURFL.IDevice = manager.GetDeviceForRequest(UserAgent)
                Dim isTablet = Boolean.Parse(device.GetCapability("is_tablet"))
                Dim isMobileDevice = Boolean.Parse(device.GetCapability("is_smartphone"))
                Dim form_factor = device.GetCapability("form_factor")
                Dim advertised_device_os = device.GetCapability("advertised_device_os")
                Dim complete_device_name = device.GetCapability("complete_device_name")
                Dim mobile_browser = device.GetCapability("mobile_browser")
                Dim brand_name = device.GetCapability("brand_name")
                Dim model_name = device.GetCapability("model_name")
                Dim device_os = device.GetCapability("device_os")
                Dim device_os_version = device.GetCapability("device_os_version")

                Dim result As String = String.Format("isTablet:{0};isMobileDevice:{1};form_factor:{2};advertised_device_os:{3};complete_device_name:{4}", isTablet, isMobileDevice, form_factor, advertised_device_os, complete_device_name)
                result += String.Format(";mobile_browser:{0};brand_name:{1};model_name:{2};device_os:{3};device_os_version:{4}", mobile_browser, brand_name, model_name, device_os, device_os_version)

                If (Not String.IsNullOrEmpty(result)) Then
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim cmd As DbCommand = db.GetSqlStringCommand("Update StoreOrder set WhereOrder = @Result where OrderId = @OrderId")
                    db.AddInParameter(cmd, "Result", DbType.String, result)
                    db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                    db.ExecuteNonQuery(cmd)
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "logOrderDevice: OrderId : " + OrderId.ToString(), ex.Message)
            End Try

        End Sub
    End Class


End Namespace
