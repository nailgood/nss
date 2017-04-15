

Imports Components
Imports DataLayer
Imports System.Data

Partial Class includes_masterpage_checkout
    Inherits System.Web.UI.MasterPage


    Private Sub MasterPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim form As HtmlForm = CType(Me.FindControl("formMain"), HtmlForm)
        form.Action = HttpContext.Current.Request.RawUrl
        Utility.Common.GenerateMatserPageClass(form)



        If Not IsPostBack Then
            If Utility.Common.IsiPad Then
                ltriPadScrollBar.Text = " <link href='/includes/scripts/nyroModal/styles/ipad-scollbar.css' rel='stylesheet' type='text/css' />"
            End If
            If Utility.Common.IsViewFromAdmin Then
                fullHeader.Visible = False
                fullFooter.Visible = False
                checkoutHeader.Visible = False
                checkoutFooter.Visible = False
                'ucSecure.Visible = False
            Else
                If Request.Path.Contains("addressbook/edit.aspx") Then
                    loading.Visible = False
                End If
                VisibleHeader()
                If Request.Path = "/store/cart.aspx" Then
                    checkoutHeader.Visible = False

                    If Not Request.QueryString("act") Is Nothing Then
                        If Request.QueryString("act") = "checkoutcart" Then
                            ltrScript.Text = "<script type='text/javascript'>GoItemErrorCheckOut();</script>"
                            Exit Sub
                        End If
                    End If
                End If
            End If
            SetIndexFollow()
        End If
    End Sub

    Private Sub VisibleHeader()
        If Request.Path = "/store/cart.aspx" Or Request.Path = "/store/reward-point.aspx" Or Request.Path = "/store/free-sample.aspx" Or Request.Path = "/store/confirmation.aspx" Then
            fullHeader.Visible = True
            fullFooter.Visible = True
            checkoutHeader.Visible = True
            checkoutFooter.Visible = False
        ElseIf Request.Path = "/members/orderhistory/view.aspx" Then
            fullHeader.Visible = True
            fullFooter.Visible = True
            checkoutHeader.Visible = False
            checkoutFooter.Visible = False
            'ucSecure.Visible = False
        Else
            fullHeader.Visible = False
            fullFooter.Visible = False
            checkoutHeader.Visible = True
            checkoutFooter.Visible = True
        End If
    End Sub
    Protected Overloads Sub OnInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        SitePage.LoadRegionControl(rightcontent, Utility.Common.ContentToolRegion.CT_Right.ToString, Page)
    End Sub
    Private Sub SetIndexFollow()
        Try
            Dim path = Request.Path
            Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL(path)
            If Not row.IsIndexed Or Not row.IsFollowed Then
                Dim sp As New SitePage()
                sp.SetIndexFollow(row, ltIndexFollow, False)
            End If
        Catch

        End Try

    End Sub
End Class

