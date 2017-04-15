Imports Components
Imports DataLayer

Partial Class EmailMe
    Inherits SitePage

    Protected SiteName As String = String.Empty
    Private dbItem As StoreItemRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SiteName = SysParam.GetValue("SiteName")

        Try
            dbItem = StoreItemRow.GetRow(DB, Convert.ToInt32(Request.QueryString("ItemId")))
            If dbItem.ItemId = Nothing Then Throw New Exception("")
            ltlItemName.Text = Server.HtmlEncode(dbItem.ItemName)
        Catch ex As Exception
            Response.StatusCode = 404
        End Try

        If Not Request("c") = String.Empty Then
            pnlResult.Visible = True
            pnlFields.Visible = False
        Else
            pnlResult.Visible = False
            pnlFields.Visible = True
        End If
    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If Not IsValid Then Exit Sub
        If Not CheckCaptcha(txtCaptcha.Text.Trim()) Then
            'AddError("Please try the code shown instead again")
            reqTxtCaptcha.Visible = False
            ltCapcha.Text = "<span class='text-danger'>Please try the code shown instead again</span><br>"
            txtCaptcha.Text = ""
            Exit Sub
        End If

        DB.ExecuteSQL("IF NOT EXISTS (SELECT Email FROM StoreItemAvailability WHERE ItemId=" & dbItem.ItemId & " AND Email=" & DB.Quote(txtEmail.Text) & " AND SentDate Is NULL) BEGIN INSERT INTO StoreItemAvailability (ItemId, Email, CreatedDate) VALUES (" & dbItem.ItemId & "," & DB.Quote(txtEmail.Text) & "," & DB.Quote(DateTime.Now) & ") END;")
        litResult.Text = "Thank you. We will email you when this product becomes available."
        pnlResult.Visible = True
        pnlFields.Visible = False
    End Sub
End Class
