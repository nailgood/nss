Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_list_member_address
    Inherits ModuleControl

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Private m_CurrentAddressType As String
    Public Property CurrentAddressType() As String
        Set(ByVal value As String)
            m_CurrentAddressType = value
        End Set
        Get
            Return m_CurrentAddressType
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Cart Is Nothing AndAlso Not Session("cartRender") Is Nothing Then
            Cart = Session("cartRender")
            If Not Cart Is Nothing Then
                CurrentAddressType = HttpContext.Current.Session("addressTypeRender")

            End If
        End If
        If Cart Is Nothing Then
            Dim orderId As Integer = Session("OrderId")
            If orderId < 1 Then
                orderId = Utility.Common.GetOrderIdFromCartCookie()
            End If
            If orderId > 0 Then
                Cart = New ShoppingCart(DB, orderId, False)
            End If
        End If
        Dim lstAddress As List(Of MemberAddressRow) = MemberAddressRow.GetListAddressByType(Cart.Order.MemberId, CurrentAddressType)
        Dim curentAddressId As Integer = 0
        If (CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()) Then
            curentAddressId = Cart.Order.ShippingAddressId

        ElseIf (CurrentAddressType = Utility.Common.MemberAddressType.Billing.ToString()) Then
            curentAddressId = Cart.Order.BillingAddressId
        End If
        ltrListAddress.Text = BindListAddress(lstAddress, CurrentAddressType, curentAddressId)
    End Sub
    Private Function GetDisable(ByVal objCurrentAddress As MemberAddressRow, ByVal isAddressIntCountry As Boolean, ByVal isOrderShipAddressIntCountry As Boolean) As String
        If (isOrderShipAddressIntCountry) Then '' neu dia chi ship hien tai cua order la 1 country Int
            If Not (isAddressIntCountry) Then '' neu address hien tai ko phai la  1 country Int
                Return "disabled='disabled'" ''--> ko cho chon
            Else
                Return String.Empty
            End If
        Else
            If Not (isAddressIntCountry) Then
                Return String.Empty
            Else
                Return "disabled='disabled'"
            End If
        End If
      
        Return String.Empty
    End Function
    Private Function CheckOrderIsCompleteIntAddress(ByVal lstAddress As List(Of MemberAddressRow)) As Boolean
        For Each Address As MemberAddressRow In lstAddress
            If (Address.AddressId = Cart.Order.ShippingAddressId) Then
                Return Utility.Common.IsCompleteIntAddress(Address)
            End If
        Next
        Return False
    End Function

    Private Function BindListAddress(ByVal lstAddress As List(Of MemberAddressRow), ByVal type As String, ByVal defaultSelect As Integer) As String
        Dim IsCurrentOrderShippingIntCountry As Boolean = False

        If (CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()) Then
            IsCurrentOrderShippingIntCountry = CheckOrderIsCompleteIntAddress(lstAddress)
        End If
        Dim isAddressIntCountry As Boolean = False
        Dim html As String = String.Empty
        Dim index As Integer = 1
        Dim ctrId As String = String.Empty
        Dim country As String = String.Empty
        Dim link As String = String.Empty

        Dim strDisable As String = String.Empty

        For Each Address As MemberAddressRow In lstAddress
            isAddressIntCountry = Utility.Common.IsCompleteIntAddress(Address)
            country = DB.ExecuteScalar("select top 1 countryname from country where countrycode = '" & Address.Country & "'")
            link = "/store/billing.aspx"
            If isAddressIntCountry Then
                link = "/store/billingint.aspx"
            End If
            If (CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()) Then ''chi list Shipping moi cho disable/enable, list billing luc nao cung cho chon
                strDisable = GetDisable(Address, isAddressIntCountry, IsCurrentOrderShippingIntCountry)
                If Not String.IsNullOrEmpty(strDisable) Then
                    If String.IsNullOrEmpty(hidListAddressBookId.Value) Then
                        hidListAddressBookId.Value = "," & Address.AddressId & ","
                    Else
                        If Not hidListAddressBookId.Value.Contains("," & Address.AddressId & ",") Then
                            hidListAddressBookId.Value &= Address.AddressId & ","
                        End If
                    End If
                    Continue For
                End If
                'If Not Session("ShippingAddressIdRender") Is Nothing Then
                '    If (Address.AddressId = Session("ShippingAddressIdRender")) Then
                '        Continue For
                '    End If
                'End If

                If Not Cart.Order.IsSameAddress Then
                    If Address.AddressId = Cart.Order.BillingAddressId Then
                        Continue For
                    End If
                End If

            Else
                strDisable = String.Empty
            End If
            Dim eventSelect As String = "onclick='ChangeOrder" & type & "Address(" & Address.AddressId & ");'"
            Dim raditextCSS As String = " radio-text"
            ctrId = "rdo" & type & "Address"
            html &= "<div class='data' id='row" & type & "Address_" & Address.AddressId & "'>"
            If Not String.IsNullOrEmpty(strDisable) Then
                eventSelect = String.Empty
                raditextCSS = String.Empty
            End If
            html &= "<div class='select'><input " & eventSelect & " type='radio' " & strDisable & "  class='" & IIf(String.IsNullOrEmpty(strDisable), "radio-node", "radio-node-disable") & "' id='" & ctrId & "_" & Address.AddressId & "' name='" & ctrId & "'" & IIf(defaultSelect = Address.AddressId, "checked='checked'", "") & ">"
            html &= "   <label class='radio-label " & ctrId & "' for='" & ctrId & "_" & Address.AddressId & "'>&nbsp;</label>"
            html &= "</div>"
            html &= "<div class='name" & raditextCSS & "' " & eventSelect & ">" & Address.FirstName & " " & Address.LastName & "</div>"
            html &= "<div class='address" & raditextCSS & "' >"
            html &= "    <ul>"
            html &= "        <li class='value' " & eventSelect & ">" & Address.Address1 & "<br>"
            If isAddressIntCountry Then
                If Not String.IsNullOrEmpty(Address.Region) Then
                    html &= Address.City & " , " & Address.Region
                Else
                    html &= Address.City
                End If

                If String.IsNullOrEmpty(Address.Zip) Then
                    html &= ", " & country & " </li>"
                Else
                    html &= ", " & Address.Zip & ", " & country & " </li>"
                End If

            Else
                html &= Address.City & " , " & Address.State & ", " & Address.Zip & ", " & country & " </li>"
            End If
            If String.IsNullOrEmpty(strDisable) Then
                html &= "        <li><span class='edit'><a href='" & link & "?type=" & type & "&id=" & Address.AddressId & "'>Edit</a> </span>"
            Else
                html &= "        <li>"
            End If


            If (Address.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                If (defaultSelect = Address.AddressId) Then
                    html &= "<span class='sep' style='display:none;' id='sepAddress" & type & "_" & Address.AddressId & "'>|</span> <span style='display:none;' class='remove' id='lnkRemoveAddress" & type & "_" & Address.AddressId & "'><a href='javascript:void(0)' onclick='DeleteAddress(" & Address.AddressId & ")'>Remove</a> </span>"
                Else
                    If String.IsNullOrEmpty(strDisable) Then
                        html &= "<span class='sep' id='sepAddress" & type & "_" & Address.AddressId & "'>|</span> <span class='remove' id='lnkRemoveAddress" & type & "_" & Address.AddressId & "'><a href='javascript:void(0)' onclick='DeleteAddress(" & Address.AddressId & ")'>Remove</a> </span>"
                    Else
                        html &= "<span class='remove' id='lnkRemoveAddress" & type & "_" & Address.AddressId & "'><a href='javascript:void(0)' onclick='DeleteAddress(" & Address.AddressId & ")'>Remove</a> </span>"
                    End If
                End If
            End If
            html &= "</li></ul>"
            html &= "</div>"
            html &= "</div>"
            index = index + 1
            If (CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()) Then
                If (Address.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                    If String.IsNullOrEmpty(hidListAddressBookId.Value) Then
                        hidListAddressBookId.Value = "," & Address.AddressId & ","
                    Else
                        If Not hidListAddressBookId.Value.Contains("," & Address.AddressId & ",") Then
                            hidListAddressBookId.Value &= Address.AddressId & ","
                        End If
                    End If
                End If
            Else
                hidListAddressBookId.Visible = False
            End If

        Next
        Return html
    End Function
End Class
