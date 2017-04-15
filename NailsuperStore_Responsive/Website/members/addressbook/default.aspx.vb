Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Member_Addressbook_Default
    Inherits SitePage
    Private o As StoreOrderRow
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        Dim dbMember As MemberRow = MemberRow.GetRow(Utility.Common.GetCurrentMemberId())
        'ltlPageTitle.Text = "My Address Book"
        'ltlMemberNavigation.Text = MemberRow.MemberNavigationString
        Dim cOrderId As Integer = 0
        Dim CookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
        If CookieOrderId <> 0 Then cOrderId = CookieOrderId
        If Session("OrderId") <> Nothing Then cOrderId = Session("OrderId")
        o = StoreOrderRow.GetRow(DB, cOrderId)
        Dim ds As DataSet = dbMember.GetAddressBook()
        rptAddressBook.DataSource = ds
        rptAddressBook.DataBind()
        If ds.Tables(0).DefaultView.Count = 0 Then
            addressbook.Visible = False
            divNoItems.Visible = True
            ltlNoItems.Text = "<table style=""margin:20px 0 15px 20px;"" cellspacing=""0"" cellpadding=""0"" border=""0""  summary=""product""><tr><Td>There are currently no entries in your address book.</td></tr></table>"
        Else
            addressbook.Visible = True
            divNoItems.Visible = False
        End If
    End Sub

    Protected Sub rptAddressBook_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptAddressBook.ItemCommand
        If e.CommandName = "Remove" Then
			Dim dbAddressBook As MemberAddressRow = MemberAddressRow.GetRow(DB, e.CommandArgument)
			dbAddressBook.MarkDeleted()
			'dbAddressBook.Remove()
            DB.Close()
            Response.Redirect("/members/addressbook/")
        ElseIf e.CommandName = "Edit" Then
            Dim dbAddressBook As MemberAddressRow = MemberAddressRow.GetRow(DB, e.CommandArgument)
            DB.Close()
            Response.Redirect("/members/addressbook/edit.aspx?AddressId=" & e.CommandArgument)
        End If
    End Sub

    Protected Sub rptAddressBook_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAddressBook.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim btnEdit As Button = CType(e.Item.FindControl("btnEdit"), Button)
            Dim btnDelete As Button = CType(e.Item.FindControl("btnDelete"), Button)
            Dim ltlLabel As Literal = CType(e.Item.FindControl("ltlLabel"), Literal)
            Dim ltlName As Literal = CType(e.Item.FindControl("ltlName"), Literal)
            Dim ltlAddress As Literal = CType(e.Item.FindControl("ltlAddress"), Literal)
            Dim AddressId As Integer = e.Item.DataItem("AddressId")
            Dim name As String = "", namelabel As String = "", address As String = ""
            If o.BillingAddressId = AddressId Or o.ShippingAddressId = AddressId Then
                btnDelete.Visible = False
            End If
            btnDelete.CommandArgument = AddressId
            btnEdit.CommandArgument = AddressId

            If Not IsDBNull(e.Item.DataItem("FirstName")) Then name &= e.Item.DataItem("FirstName")
            If Not IsDBNull(e.Item.DataItem("LastName")) Then name &= " " & e.Item.DataItem("LastName")
            If Not IsDBNull(e.Item.DataItem("Label")) Then namelabel &= e.Item.DataItem("Label")
            If Not IsDBNull(e.Item.DataItem("Address1")) Then address &= e.Item.DataItem("Address1")
            If Not IsDBNull(e.Item.DataItem("Address2")) Then address &= "<br>" & e.Item.DataItem("Address2")
            If Not IsDBNull(e.Item.DataItem("City")) Then address &= "<br>" & e.Item.DataItem("City")
            If Not IsDBNull(e.Item.DataItem("State")) Then address &= ", " & StateRow.GetRow(DB, Convert.ToString(e.Item.DataItem("State"))).StateName
            If Not IsDBNull(e.Item.DataItem("Region")) Then address &= ", " & e.Item.DataItem("Region")
            If Not IsDBNull(e.Item.DataItem("Zip")) Then address &= " " & e.Item.DataItem("Zip")
            If Not IsDBNull(e.Item.DataItem("Country")) Then address &= "<br>" & CountryRow.GetRow(DB, Convert.ToString(e.Item.DataItem("Country"))).CountryName

            ltlLabel.Text = namelabel
            ltlName.Text = name
            ltlAddress.Text = address

        End If
    End Sub

    
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        DB.Close()
        Response.Redirect("/members/addressbook/edit.aspx")
    End Sub
End Class