Imports Components
Imports DataLayer
Imports System.IO
Imports System.Collections.Generic
Partial Class controls_product_testimonial_item
    Inherits BaseControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim DepartmentId As Integer = Request("DepartmentId")
        If Not IsPostBack Then
            LoadData(DepartmentId)
        End If
    End Sub

    Private Sub LoadData(ByVal DepartmentId As Integer)
        Dim result As List(Of StoreDepartmentReviewRow) = StoreDepartmentReviewRow.ListByDepartmentId(DepartmentId)
        rptTestimonial.DataSource = result
        rptTestimonial.DataBind()
    End Sub

    Protected Sub rptTestimonial_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptTestimonial.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim row As StoreDepartmentReviewRow = e.Item.DataItem
            Dim salonName, State, Country As String
            StoreItemReviewRow.GetReviewAuthorData(row.MemberId, salonName, State, Country)
            Dim ltrTitle As Literal = e.Item.FindControl("ltrTitle")
            Dim ltrStar As Literal = e.Item.FindControl("ltrStar")
            Dim ltrComment As Literal = e.Item.FindControl("ltrComment")
            Dim ltrAuthor As Literal = e.Item.FindControl("ltrAuthor")
            Dim ltrAddress As Literal = e.Item.FindControl("ltrAddress")

            ltrTitle.Text = "<a href=""" & URLParameters.ProductUrl(row.URLCode, row.ItemId) & "#review-section"">" & row.ReviewTitle & "</a>"
            'ltrStar.Text = "<img src='" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/smallstar" & row.NumStars & "0.png' style='border-style: none' alt='" & row.ItemName & "' />"
            ltrStar.Text = SitePage.BindIconStar(row.NumStars)
            ltrComment.Text = Utility.Common.GetProductReviewComment(row.Comment)
            ltrAuthor.Text = row.ReviewerName
            ltrAddress.Text = IIf(String.IsNullOrEmpty(salonName), State & " - " & Country, salonName & ", " & State & " - " & Country)
        End If
    End Sub
End Class
