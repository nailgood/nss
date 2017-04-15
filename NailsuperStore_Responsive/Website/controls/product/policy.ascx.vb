Imports Components
Imports DataLayer

Partial Class controls_policy
    Inherits System.Web.UI.UserControl
    Public ItemId As Integer
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If ItemId > 0 Then
                LoadDetail()
            End If
        End If

    End Sub

    Private Sub LoadDetail()
        Dim collection As PolicyCollection = PolicyRow.ListByItemId(ItemId)
        If collection.Count > 0 Then
            rptPolicy.DataSource = collection
            rptPolicy.DataBind()
        End If

    End Sub

    Protected Sub rptPolicy_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPolicy.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim p As PolicyRow = e.Item.DataItem
            Dim litPolicy As Literal = CType(e.Item.FindControl("litPolicy"), Literal)
            If p.IsPopup = False Then
                If String.IsNullOrEmpty(p.TextLink) Then
                    litPolicy.Text = String.Format("<a href=""{1}"">{0}</a>", p.Message, URLParameters.PolicyUrl(p.PolicyId))
                Else
                    litPolicy.Text = String.Format("{0} <br><a href=""{1}"" target=""_blank"">{2}</a>", p.Message, URLParameters.PolicyUrl(p.PolicyId), p.TextLink)
                End If

            Else
                If String.IsNullOrEmpty(p.TextLink) Then
                    litPolicy.Text = String.Format("<a href=""javascript:void(ShowPolicyPopup('{1}', '{2}'));"">{0}</a>", p.Message, p.PolicyId.ToString(), p.Title, p.TextLink)
                Else
                    ''litPolicy.Text = String.Format("{0} <br><a href=""{1}"" target=""_blank"">{2}</a>", p.Message, URLParameters.PolicyUrl(p.PolicyId), p.TextLink)
                    litPolicy.Text = String.Format("{0}<br><a href=""javascript:void(ShowPolicyPopup('{1}', '{2}'));"">{3}</a>", p.Message, p.PolicyId.ToString(), p.Title, p.TextLink)
                End If
                Dim hdf As HiddenField = CType(e.Item.FindControl("hidPolicyContent"), HiddenField)
                hdf.Value &= p.Content
                hdf.ID = hdf.ID & p.PolicyId.ToString()


            End If

        End If
    End Sub
End Class
