Imports Components
Imports DataLayer
Partial Class controls_product_revieworder
    Inherits System.Web.UI.UserControl
    Public IsFirstLoad As Boolean
    Protected itemindex As Integer = 0
    Public Sub Fill(ByVal so As StoreOrderReviewRow, ByVal isFirst As Boolean)
        itemindex = so.itemIndex
        IsFirstLoad = isFirst
        lblName.Text = so.Name
        'litStar.Text = "/includes/theme/images/star" & so.NumStars & "0.png"
        litStar.Text = SitePage.BindIconStar(CStr(so.NumStars))
        litStar2.Text = Utility.Common.ConvertStartNumberToTextLevelOrderReview(so.NumStars)
        lblContent.Text = so.Comment
        'Dim divreply As System.Web.UI.HtmlControls.HtmlGenericControl = CType(e.Item.FindControl("adminreply"), System.Web.UI.HtmlControls.HtmlGenericControl)
        If Not adminreply Is Nothing Then
            Dim objReply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(so.OrderId, Utility.Common.TypeReview.OrderReview)
            If (objReply.ReplyReviewId > 0) Then
                adminreply.InnerHtml = String.Format(adminreply.InnerHtml, Replace(objReply.Content.Trim, vbCrLf, "<br/>"))
                ''str, vbCrLf, "<br/>"
            Else
                adminreply.Visible = False
            End If
        End If
    End Sub
End Class
