Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports Humanizer

Namespace Components
    Partial Class Popup_freeitem
        Inherits SitePage

        Public CartItemId As Integer
        Private OrderId As Integer
        Public MixmatchId As Integer

        Public MediaUrl As String = Utility.ConfigData.CDNMediaPath() + Utility.ConfigData.MediaUrl
        Public DefaultFreeItemIdSelect As Integer
        Public TotalFreeCanChoose As Integer = 0
        Public TotalFreeAllowed As Integer = 0
        Public FreeItemIds As String = String.Empty

        Private isAllowLink As Boolean = False
        Private isDiscount As Boolean = False
        Private memberID As Integer = 0
        Private isCustomerInternational As Boolean = False
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                isAllowLink = True
                If (Not Request.UrlReferrer Is Nothing AndAlso Request.UrlReferrer.OriginalString.Contains("/store/revise-cart.aspx")) Then
                    isAllowLink = False
                End If
                If Request("discount") IsNot Nothing AndAlso Request("discount") = "1" Then
                    isDiscount = True
                End If
                If Request("mmId") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("mmId")) Then
                    MixmatchId = Request("mmId")
                End If
                If Request("orderId") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("orderId")) Then
                    OrderId = Request("orderId")
                End If
                If Request("cartItemId") IsNot Nothing Then
                    CartItemId = Request("cartItemId")
                End If
                memberID = Utility.Common.GetMemberIdFromCartCookie()
                isCustomerInternational = MemberRow.CheckMemberIsInternational(memberID, OrderId)
                Dim isCollection As Boolean = DB.ExecuteScalar("select isnull(IsCollection, 0) from Mixmatch where Id =" + MixmatchId.ToString())
                If isCollection Then
                    main.Attributes("data-fix") = "fix"
                End If
                If isDiscount Then
                    LoadDataDiscountPercent()
                    TotalFreeCanChoose += TotalFreeAllowed
                Else
                    DefaultFreeItemIdSelect = 0
                    LoadData()
                End If
                ltrButton.Text = " <input type='button' class='btnCartLarge' onclick='CloseFreeItem();' value='Add to Cart'>"
            End If
        End Sub

        Private Sub LoadDataDiscountPercent()
            Dim cartQty As Integer = DB.ExecuteScalar("Select coalesce(sum(Quantity),0) from StoreCartItem where OrderId=" & OrderId & " and MixMatchId=" & MixmatchId & " and IsFreeItem=0 and ItemId in(Select ItemId from MixMatchLine where MixMatchId=" & MixmatchId & " and Value=0)")
            If cartQty < 1 Then
                cartQty = DB.ExecuteScalar("Select coalesce(sum(Quantity),0) from StoreCartItem where OrderId=" & OrderId & " and ItemId in(Select ItemId from MixMatchLine where MixMatchId=" & MixmatchId & " and Value=0)")
            End If

            Dim objMM As MixMatchRow = MixMatchRow.GetRow(DB, mixmatchId)
            TotalFreeAllowed = Math.Floor((cartQty * objMM.Optional) / objMM.Mandatory)

            Dim countGiveFree As Integer = Math.Floor(cartQty / objMM.Mandatory)
            If (countGiveFree > objMM.TimesApplicable And objMM.TimesApplicable > 0) Then
                countGiveFree = objMM.TimesApplicable
                TotalFreeAllowed = countGiveFree * objMM.Optional
            End If

            Dim lstFreeItem As StoreItemCollection = MixMatchRow.GetListFreeItem(MixmatchId, memberID, isCustomerInternational)
            If lstFreeItem Is Nothing Then
                Exit Sub
            End If

            If lstFreeItem.Count < 1 Then
                Exit Sub
            End If
            For Each objItem As StoreItemRow In lstFreeItem
                ltrItemList.Text &= CreateItemHTML(objItem.ItemId, objItem.ItemName, objItem.URLCode, objItem.Image, Nothing, Nothing, TotalFreeAllowed, Nothing, True, True)
            Next
            If ltrItemList.Text.Length > 10 Then
                ltrItemList.Text &= "<div id='endAddedItem'></div>"
            End If
            For Each objItem As StoreItemRow In lstFreeItem
                ltrItemList.Text &= CreateItemHTML(objItem.ItemId, objItem.ItemName, objItem.URLCode, objItem.Image, Nothing, Nothing, TotalFreeAllowed, Nothing, True)
                hidListId.Value = hidListId.Value & "," & objItem.ItemId
            Next

        End Sub

        Private Function CreatePrice(ByVal itemId As Integer, ByVal mmId As Integer) As String
            Dim dtPrice As DataTable = DB.GetDataTable("Select si.Price,mml.Value from StoreItem si left join MixMatchLine mml on(mml.ItemId=si.ItemId)  where mml.ItemId=" & itemId & " and MixMatchId=" & mmId & " ORDER BY Value DESC")
            If Not dtPrice Is Nothing Then
                Dim price As Double = dtPrice.Rows(0)("Price")
                Dim mmValue As Double = dtPrice.Rows(0)("Value")
                ''get lowesr price
                Dim tmp As String = StoreItemRow.GetCustomerDiscountWithQuantity(itemId, System.Web.HttpContext.Current.Session("MemberId"), 1, 1)
                '' If IsNumeric(tmp) Then ci.LowSalePrice = Utility.Common.RoundCurrency(tmp)
                Dim LowSalePrice As Double = Utility.Common.RoundCurrency(tmp)

                If mmValue < 100 AndAlso mmValue > 0 Then
                    If LowSalePrice > 0 Then
                        price = LowSalePrice
                    End If
                    LowSalePrice = price - (mmValue * price) / 100
                End If

                Dim YouSave As Double = price - LowSalePrice
                Dim PercentSave As Double = 1 - (LowSalePrice / price)
                PercentSave = Utility.Common.RoundCurrency(PercentSave * 100)
                Dim result As String = ""
                result &= "<ul class='sec-price'><li class='price'><span class='strike bold'>" & Utility.Common.ViewCurrency(price) & "</span>&nbsp;&nbsp;<span class='red bold'>" & Utility.Common.ViewCurrency(LowSalePrice) & "</span></li>"
                result &= "<li class='save'>You Save: <span class='yousave'>" & Utility.Common.ViewCurrency(YouSave) & "</span> (<span class='savepercent'>" & PercentSave & "</span>%)</li></ul>"
                Return result
            End If
            Return String.Empty
        End Function

        Private Function CreateItemHTML(ByVal itemid As Integer, ByVal name As String, ByVal urlCode As String, ByVal image As String, ByVal DefaultFreeItemIdSelect As Integer, ByVal defaultQtyFreeItem As Integer, ByVal maxQty As Integer, ByVal lstCurrentSelect As StoreCartItemCollection, ByVal isDiscountPercent As Boolean, Optional ByVal isAdded As Boolean = False) As String
            If (String.IsNullOrEmpty(image)) Then
                image = Utility.ConfigData.NoImageItem
            End If


            If isAdded AndAlso Not isDiscount Then
                Dim qty As Integer = 0
                For Each currentFree As StoreCartItemRow In lstCurrentSelect
                    If (currentFree.ItemId = itemid) Then
                        qty = currentFree.Quantity
                        If qty > maxQty Then
                            qty = maxQty
                        End If
                    End If
                Next
                If (qty = 0) Then
                    Return String.Empty
                End If
            End If

            Dim link As String = URLParameters.ProductUrl(urlCode, itemid)
            Dim result As String = "<div class='item " + IIf(isAdded, "addedItem", "") + "'>"
            result &= "                   <a class='" & IIf(isAllowLink, "", "adiable") & "' onclick=""gotoLinkPopupReviseFreeItem(' " & link & " ')"" href='javascript:void(0);'><img alt='" & name & "' src='" & MediaUrl & "/items/featured/" & image & "' alt='" & name & "'/></a>"

            result &= "             <div class='name " & IIf(isDiscount, "discount-item-name", "free-item-name") & "'><a class='" & IIf(isAllowLink, "", "adiable") & "' onclick=""gotoLinkPopupReviseFreeItem('" & link & "')"" href='javascript:void(0);'>" & name & "</a></div>"
            Dim saveprice As String = String.Empty
            If isDiscountPercent Then
                saveprice = CreatePrice(itemid, MixmatchId)
                Dim dbQty As Integer = DB.ExecuteScalar("Select Quantity from StoreCartItem where OrderId=" & OrderId & " and MixMatchId=" & MixmatchId & " and IsFreeItem=1 and ItemId=" & itemid)
                If isAdded AndAlso dbQty = 0 Then
                    Return String.Empty
                End If
                result &= GenerateComboboxQty(itemid, maxQty, dbQty, saveprice, "bao-cart-price", isAdded)
            Else
                If DefaultFreeItemIdSelect > 0 AndAlso defaultQtyFreeItem > 0 AndAlso itemid = DefaultFreeItemIdSelect Then
                    result &= "<input type='text' id='txtQty_" & itemid & "' value=" & defaultQtyFreeItem & " disabled='disabled' class='qty-df' />"
                Else
                    Dim dbQty As Integer = 0
                    For Each currentFree As StoreCartItemRow In lstCurrentSelect
                        If (currentFree.ItemId = itemid) Then
                            dbQty = currentFree.Quantity
                            If dbQty > maxQty Then
                                dbQty = maxQty
                            End If
                        End If
                    Next
                    result &= GenerateComboboxQty(itemid, maxQty, dbQty, saveprice, "bao-cart", isAdded)
                End If
            End If
            result &= "        </div>"
            Return result
        End Function
        Private Function GenerateComboboxQty(ByVal itemId As Integer, ByVal maxQty As Integer, ByVal defaultSelect As Integer, ByVal saveprice As String, ByVal css As String, ByVal isAdded As Boolean) As String
            Dim result As String = "<div class=""" & css & """>" & saveprice _
            & "<div class=""qty"">" _
            & "     <div class=""plus""><a href=""javascript:void(DetectQty('{0}', true, this));void(Increase('{0}'));"">+</a></div>" _
            & "     <div class=""min""><a href=""javascript:void(DetectQty('{0}', false, this));void(Decrease('{0}',0));"">–</a></div>" _
            & "     <div class=""pull-left""><input type=""tel"" class=""txt-qty" + IIf(isAdded, " added", "") + """name=""{0}"" id=""{0}"" value=""{1}"" onkeypress=""InputCartQty('{2}','{0}',event);"" maxlength=""4""><input type=""hidden"" id=""hid{2}"" value=""{1}"" /></div>" _
            & "     <div class=""bao-arrow"">" _
            & "         <ul>" _
            & "             <li><a href=""javascript:void(DetectQty('{0}', true));void(Increase('{0}',0));""><b class=""arrow-up arrow-down-act"" id=""{0}""></b></a></li>" _
            & "             <li class=""bd-qty""><a href=""javascript:void(DetectQty('{0}', false));void(Decrease('{0}',0));""><b class=""arrow-down arrow-down-act"" id=""{0}""></b></a></li>" _
            & "         </ul>" _
            & "     </div>" _
            & "</div>" _
            & "<div class=""update-ctr"" id=""pnUpdate_{2}"">" _
            & "     <a href=""javascript:void(OnChangeQty({3},{4},'{5}',{2},'{0}'));"">Update</a>  | <a href=""javascript:void(CancelUpdate({2},'{0}'))"">Cancel</a>" _
            & "</div>"

            Dim textIncart As String = String.Empty
            If defaultSelect > 0 Then
                textIncart = ("<div id=""lblInCart{0}"" name=""{0}"" class=""incart"">Added | <a onclick=""DeleteCartItemMixMatch('{2}');"" class=""remove"">Remove</a></div>")
            Else
                textIncart = "<div id=""lblInCart{0}"" name=""{0}"" class=""incart""></div>"
            End If

            result = String.Format(result & textIncart & "</div>", "txtQty_" & itemId, defaultSelect, itemId, MixmatchId, TotalFreeAllowed, FreeItemIds)
            If (Not isAdded) Then
                TotalFreeCanChoose -= defaultSelect
            End If
            Return result
        End Function
        Private Sub LoadData()
            Dim maxQty As Integer = 0
            Dim defaultQtyFreeItem As Integer = 0

            Dim freeItemList As StoreCartItemCollection = StoreCartItemRow.GetFreeItems(DB, OrderId, CartItemId, MixmatchId, False)
            If freeItemList.Count > 0 Then
                'DefaultFreeItemIdSelect = DB.ExecuteScalar("Select ItemId from MixMatchLine where MixMatchId=" & MixmatchId & " and Value=100 and IsDefaultSelect=1")
                'If DefaultFreeItemIdSelect > 0 Then
                '    defaultQtyFreeItem = DB.ExecuteScalar("Select Quantity from StoreCartItem where OrderId=" & OrderId & " and MixMatchId=" & MixmatchId & " and ItemId=" & DefaultFreeItemIdSelect & " and IsFreeItem=1")
                'End If

                FreeItemIds = freeItemList(0).FreeItemIds
                TotalFreeAllowed = freeItemList(0).TotalFreeAllowed
                TotalFreeCanChoose = TotalFreeAllowed
                maxQty = TotalFreeAllowed
                If Not String.IsNullOrEmpty(freeItemList(0).FreeItemIds) Then
                    Dim vals() As String = Split(freeItemList(0).FreeItemIds, ",")
                    If UBound(vals) > 0 Then
                        If TotalFreeAllowed > 1 Then
                            maxQty = TotalFreeAllowed - defaultQtyFreeItem
                        End If
                    End If
                End If
            ElseIf TotalFreeCanChoose <= 0 And TotalFreeAllowed <= 0 Then
                'Dim sci As StoreCartItemRow = StoreCartItemRow.GetRow(DB, CartItemId)
                Dim count = DB.ExecuteScalar("select distinct (a.Quantity/c.Mandatory)*c.Optional - isnull((select sum(b.DefaultSelectQty*a.Quantity) from StoreCartItem a inner join MixMatchLine b on a.MixMatchId = b.MixMatchId and a.CartItemId = " + CartItemId.ToString() + " and Value > 0 and IsDefaultSelect = 1), 0) from StoreCartItem a inner join MixMatchLine b on a.MixMatchId = b.MixMatchId  and a.CartItemId = " + CartItemId.ToString() + " and b.Value > 0 and b.IsDefaultSelect = 0 inner join MixMatch c on c.Id = b.MixMatchId")
                TotalFreeCanChoose = count
                TotalFreeAllowed = count
            End If

            'Dim SQL As String = ""
            'Dim whereItemId As String = String.Empty
            'whereItemId = DB.NumberMultiple(FreeItemIds)
            'SQL = "select itemname + coalesce((select top 1 case when coalesce(choicename,'') = '' then '' else ' - ' + choicename + ' ' end from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = si.itemid order by o.sortorder),'')   as itemname,coalesce(URLCode,'') as URLCode, si.itemid,coalesce(image,'') as image,QtyOnHand from storeitem si"
            'SQL = SQL & " left join MixMatchLine mml on(mml.ItemId=si.ItemId ) "
            'SQL = SQL & " WHERE " & IIf(whereItemId.Contains("NULL"), " 1=1 ", " si.itemid In " & whereItemId) & " And si.IsActive=1 "
            'SQL = SQL & "  AND mml.MixMatchId=" & MixmatchId & " And mml.Value=100"
            'SQL = SQL & "  AND (si.QtyOnHand>0 OR (AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.AcceptingOrder & " Or AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.InStock & "))"
            'SQL = SQL & " and exists (select 1 from mixmatchline f where f.MixMatchId = " + MixmatchId.ToString() + " and f.ItemId = si.ItemId and f.IsDefaultSelect = 0) "
            'SQL = SQL & "  ORDER BY si.itemid"

            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase()
                Dim cmd As Common.DbCommand = db.GetStoredProcCommand("sp_Mixmatch_GetListFreeItem")
                db.AddInParameter(cmd, "MixmatchId", DbType.Int32, MixmatchId)
                db.AddInParameter(cmd, "AcceptingOrder", DbType.Int16, Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                db.AddInParameter(cmd, "InStock", DbType.Int16, Utility.Common.ItemAcceptingStatus.InStock)
                db.AddInParameter(cmd, "WhereItemIds", DbType.String, FreeItemIds)
                dr = db.ExecuteReader(cmd)
                If Not dr Is Nothing Then
                    Dim tmpMaxQty As Integer = 0
                    Dim itemname As String = String.Empty
                    Dim itemid As String = String.Empty
                    Dim urlCode As String = String.Empty
                    Dim image As String = String.Empty
                    Dim QtyOnHand As Integer = 0
                    Dim dt As DataTable = New DataTable()
                    dt.Load(dr)
                    For Each row As DataRow In dt.Rows
                        itemname = row("itemname")
                        image = row("image")
                        itemid = row("itemid")
                        urlCode = row("urlCode")
                        QtyOnHand = row("QtyOnHand")
                        tmpMaxQty = maxQty
                        ltrItemList.Text &= CreateItemHTML(itemid, itemname, urlCode, image, DefaultFreeItemIdSelect, defaultQtyFreeItem, tmpMaxQty, freeItemList, isDiscount, True)
                    Next
                    If ltrItemList.Text.Length > 10 Then
                        ltrItemList.Text &= "<div id='endAddedItem'></div>"
                    End If
                    For Each row As DataRow In dt.Rows
                        itemname = row("itemname")
                        image = row("image")
                        itemid = row("itemid")
                        urlCode = row("urlCode")
                        QtyOnHand = row("QtyOnHand")
                        tmpMaxQty = maxQty
                        ltrItemList.Text &= CreateItemHTML(itemid, itemname, urlCode, image, DefaultFreeItemIdSelect, defaultQtyFreeItem, tmpMaxQty, freeItemList, isDiscount)
                    Next
                End If
            Catch ex As Exception
            End Try
            Core.CloseReader(dr)
        End Sub

    End Class
End Namespace
