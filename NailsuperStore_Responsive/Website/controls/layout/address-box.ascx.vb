
Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_layout_address_box
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Cart As ShoppingCart = Nothing
    Private m_AddressType As String
    Public Property AddressType() As String
        Get
            Return m_AddressType
        End Get
        Set(ByVal Value As String)
            m_AddressType = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()
            If String.IsNullOrEmpty(AddressType) Then
                AddressType = Utility.Common.MemberAddressType.Billing.ToString()
              
            End If
            If AddressType = Utility.Common.MemberAddressType.Billing.ToString() Then
                divDaytimePhone.Visible = True
                divEmail.Visible = True
                divSameAddress.Visible = True
                lblPhoneLabel.InnerHtml = "<span class='required'>Billing, Home, or Work Phone</span><br /><span class='small'>Must match billing address</span></label>"
                txtPhone.Attributes.Add("placeholder", "Billing, Home, or Work Phone")
            ElseIf AddressType = Utility.Common.MemberAddressType.Shipping.ToString() Then
                lblPhoneLabel.InnerHtml = "<span class='required'>Shipping, Home, or Work Phone</span><br /><span class='small'>Must match shipping address</span></label>"
                txtPhone.Attributes.Add("placeholder", "Shipping, Home, or Work Phone")
            Else
                lblPhoneLabel.InnerHtml = "<span class='required'> Phone</span>"
                txtPhone.Attributes.Add("placeholder", "Phone")
            End If
        End If
    End Sub

End Class
