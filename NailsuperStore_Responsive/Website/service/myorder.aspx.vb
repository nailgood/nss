Imports Components

Partial Class myorder
	Inherits SitePage

	Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
		If Not IsPostBack Then
			BindData()
		End If
	End Sub

	Private Sub BindData()
		rptFAQ.DataSource = DB.GetDataSet("select question, faqid from faq where ismyorderpage = 1 order by faqcategoryid, sortorder")
		rptFAQ.DataBind()

	End Sub

    'Protected Sub ServerCheckValidEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
    '    If Not String.IsNullOrEmpty(txtEmail.Text) Then
    '        Dim email As String = txtEmail.Text.Trim()
    '        If Not Utility.Common.CheckValidEmail(email) Then
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
    'Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
    '    If Not IsValid Then Exit Sub

    '    Response.Redirect("/members/orderhistory/view.aspx?OrderNo=" & Server.UrlEncode(Utility.Crypt.EncryptTripleDes(Trim(txtOrderNo.Text))) & "&e=" & Server.UrlEncode(Utility.Crypt.EncryptTripleDes(Trim(txtEmail.Text))))
    'End Sub
   
End Class
