Imports Components
Imports DataLayer
Partial Class sendmailcart
    Inherits SitePage
    Protected SiteName As String = String.Empty

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If Not IsValid Then Exit Sub
        SendMailCart()
        pnlFields.Visible = False
        ltmsg.Text = "Your cart has been sucessfuly emailed."
    End Sub
    Private Sub SendMailCart()
        Dim URL As String = Utility.ConfigData.SendmailCartUrl & "?orderId=" & Cart.Order.OrderId
        Dim r As System.Net.HttpWebRequest = System.Net.WebRequest.Create(URL)
        r.Method = "GET"
        r.KeepAlive = False
        r.ProtocolVersion = System.Net.HttpVersion.Version10
        r.ServicePoint.ConnectionLimit = 1
        r.KeepAlive = False
        r.ProtocolVersion = System.Net.HttpVersion.Version10
        r.ServicePoint.ConnectionLimit = 1
        r.Headers.Add("UserAgent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)")
        Dim sSubject As String = "Saved Shopping Cart on nss.com"
        Dim sr As System.IO.StreamReader
        Dim resp As System.Net.HttpWebResponse = Nothing
        Try
            'Get the data as an HttpWebResponse object
            resp = r.GetResponse()
            sr = New System.IO.StreamReader(resp.GetResponseStream())
            'Dim sourceString As String = New System.Net.WebClient().DownloadString(URL)
            Dim HTML As String = sr.ReadToEnd()
            'Dim HTML As String = New System.Net.WebClient().DownloadString(URL)
            ' sr.Close()
            HTML = Replace(HTML, "href=""/", "href=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
            HTML = Replace(HTML, "src=""/", "src=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
            HTML = HTML.Replace("#TONAME#", txtName.Text)
            Dim bccConfirm As String = SysParam.GetValue("ToReportOrderConfirmation")
            Dim SiteName As String = System.Configuration.ConfigurationManager.AppSettings("GlobalWebsiteName")
            Email.SendHTMLMail(FromEmailType.NoReply, txtEmail.Text, txtName.Text, sSubject, HTML)

        Catch ex As Exception
            Email.SendError("ToError500", sSubject, "<br>Exception: " & ex.ToString())
            ltmsg.Text = "Send Fail!"
        Finally
            If Not resp Is Nothing Then resp.Close()
        End Try
    End Sub
End Class
