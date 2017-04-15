Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Members_Default
    Inherits SitePage
    Public memberID As Integer = 0
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        'memberID = Utility.Common.GetMemberIdFromCartCookie()
        'If memberID < 1 Then
        '    Response.Redirect("/members/login.aspx")
        'End If
        If Not HasAccess() Then
            Response.Redirect("/members/login.aspx")
        End If
        memberID = Session("MemberId")
        Dim contentRow As String = "<div class=""content-row""><strong>{0}: </strong><span>{1}</span></div>"
        Dim dbMember As MemberRow = MemberRow.GetRow(memberID)
        Dim dbMemberType As MemberTypeRow = MemberTypeRow.GetRow(DB, dbMember.MemberTypeId)
        Dim dbBilling As MemberAddressRow = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND Label = 'Default Billing Address'"))
        Dim dbShipping As MemberAddressRow = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND Label = 'Default Shipping Address'"))
        Dim dbCountry As CountryRow = CountryRow.GetRow(DB, dbBilling.Country)

        ''Account Details
        'ltlAccountDetails.Text = String.Format(contentRow, "Country", Server.HtmlEncode(dbCountry.CountryName))
        ltlAccountDetails.Text &= String.Format(contentRow, "Email Address", Server.HtmlEncode(dbMember.Customer.Email))
        ltlAccountDetails.Text &= String.Format(contentRow, "Username", Server.HtmlEncode(dbMember.Username))
        'ltlAccountDetails.Text &= String.Format(contentRow, "Password", Server.HtmlEncode(dbMember.Password))
        'ltlAccountDetails.Text &= String.Format(contentRow, "Professional Status", Server.HtmlEncode(dbMemberType.MemberType))
        'ltlAccountDetails.Text &= String.Format(contentRow, "Member Since", Server.HtmlEncode(dbMember.CreateDate))
        ''If Not dbMember.CanPostJob AndAlso DB.ExecuteScalar("select top 1 applicationid from postjobapplication where isapproved = 0 and memberid = " & DB.Number(memberID)) = Nothing Then ltlAccountDetails.Text &= "<tr><td colspan=""2"" style=""padding-top:7px;padding-bottom:0px;"">Are you an employer?<br /><a href=""/members/apply.aspx"" class=""linkpage"">Apply to Post Jobs in 'Salon Jobs' section</a></td></tr>" Else ltlAccountDetails.Text &= "<tr stype=""padding-top:10px""><td></td></tr>"

        ''Billing Address
        ltlBillingDetails.Text = String.Format(contentRow, "Name", Server.HtmlEncode(dbBilling.FirstName & " " & dbBilling.LastName))
        ltlBillingDetails.Text &= String.Format(contentRow, "Address", Server.HtmlEncode(dbBilling.Address1))

        If dbBilling.Address2 <> Nothing Then ltlBillingDetails.Text &= String.Format(contentRow, "Address 2", Server.HtmlEncode(dbBilling.Address2))
        ltlBillingDetails.Text &= String.Format(contentRow, "City", Server.HtmlEncode(dbBilling.City))
        If dbBilling.Country = "US" Then
            ltlBillingDetails.Text &= String.Format(contentRow, "State", Server.HtmlEncode(StateRow.GetRow(DB, dbBilling.State).StateName))
        ElseIf dbBilling.Region <> Nothing Then
            ltlBillingDetails.Text &= String.Format(contentRow, "Province/Region", Server.HtmlEncode(dbBilling.Region))
        End If
        ltlBillingDetails.Text &= String.Format(contentRow, "Zipcode", Server.HtmlEncode(dbBilling.Zip))
        ltlBillingDetails.Text &= String.Format(contentRow, "Country", Server.HtmlEncode(CountryRow.GetRow(DB, dbBilling.Country).CountryName))
        If (String.IsNullOrEmpty(dbBilling.PhoneExt)) Then
            ltlBillingDetails.Text &= String.Format(contentRow, "Phone", Server.HtmlEncode(dbBilling.Phone))
        Else
            ltlBillingDetails.Text &= String.Format(contentRow, "Phone", Server.HtmlEncode(dbBilling.Phone & " ext " & dbBilling.PhoneExt))
        End If

        ''Shipping Address
        ltlShippingDetails.Text = String.Format(contentRow, "Name", Server.HtmlEncode(dbShipping.FirstName & " " & dbShipping.LastName))
        ltlShippingDetails.Text &= String.Format(contentRow, "Address", Server.HtmlEncode(dbShipping.Address1))
        If dbShipping.Address2 <> Nothing Then ltlShippingDetails.Text &= String.Format(contentRow, "Address 2", Server.HtmlEncode(dbShipping.Address2))
        ltlShippingDetails.Text &= String.Format(contentRow, "City", Server.HtmlEncode(dbShipping.City))
        If dbShipping.Country = "US" Then
            ltlShippingDetails.Text &= String.Format(contentRow, "State", Server.HtmlEncode(StateRow.GetRow(DB, dbShipping.State).StateName))
        ElseIf dbShipping.Region <> Nothing Then
            ltlShippingDetails.Text &= String.Format(contentRow, "Province/Region", Server.HtmlEncode(dbShipping.Region))
        End If
        ltlShippingDetails.Text &= String.Format(contentRow, "Zipcode", Server.HtmlEncode(dbShipping.Zip))
        ltlShippingDetails.Text &= String.Format(contentRow, "Country", Server.HtmlEncode(CountryRow.GetRow(DB, dbShipping.Country).CountryName))
        If (String.IsNullOrEmpty(dbShipping.PhoneExt)) Then
            ltlShippingDetails.Text &= String.Format(contentRow, "Phone", Server.HtmlEncode(dbShipping.Phone))
        Else
            ltlShippingDetails.Text &= String.Format(contentRow, "Phone", Server.HtmlEncode(dbShipping.Phone & " ext " & dbShipping.PhoneExt))
        End If

        ''check has mailling member
        Dim MailingMemberId As Integer = DB.ExecuteScalar("SELECT MemberId FROM MailingMember WHERE Email=" & DB.Quote(dbMember.Customer.Email))
        If MailingMemberId > 0 Then
            btnUnsubscribe.Visible = True
        Else
            btnUnsubscribe.Visible = False
        End If
    End Sub

    'Protected Sub btnEditShipping_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditShipping.Click
    '    Dim dbMember As MemberRow = MemberRow.GetRow(memberID)
    '    Dim dbShipping As MemberAddressRow = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND AddressType = 'Shipping'"))
    '    Dim sID As String = Convert.ToString(dbShipping.AddressId)
    '    DB.Close()
    '    If dbMember.MemberTypeId = 7 Then
    '        Response.Redirect("/members/registerConsumer.aspx")
    '    Else
    '        Response.Redirect("/members/address.aspx")
    '    End If
    'End Sub
  
End Class