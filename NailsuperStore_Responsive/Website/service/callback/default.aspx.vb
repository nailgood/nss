Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class Request
    Inherits SitePage

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequest.Click
        Page.Validate("valGroup")
        If (Page.IsValid) Then
            SendMail()
            Session("sendmail") = 1
            Response.Redirect("/service/callback/thankyou.aspx")
        End If
    End Sub
    Private Sub SendMail()
        Try
            Dim sMsg As String = ""
            sMsg = "Request CallBack: " & Environment.NewLine & Environment.NewLine & _
             "Country: " & drCountry.SelectedItem.Text & Environment.NewLine & _
             "Prefer language: " & drLang.SelectedItem.Text & Environment.NewLine & _
             "Name: " & txtName.Text & Environment.NewLine & _
             "Email: " & txtEmail.Text & Environment.NewLine & _
             "Phone number or Skype ID: " & txtNumber.Text & Environment.NewLine & _
             "When should we call you?: " & drCallme.SelectedItem.Text & Environment.NewLine & _
             "How can we help?" & Environment.NewLine & _
             "" & txtNote.Text
            Dim res As DataTable = DB.GetDataTable("select * from Vie_EmailLanguage where languageid=" & drLang.SelectedValue)
            If res.Rows.Count > 0 Then
                Dim i As Integer
                Dim ToName As String = ""
                Dim ToEmail As String = ""
                For i = 0 To res.Rows.Count - 1
                    ToName = res.Rows(i)("Name")
                    ToEmail = res.Rows(i)("Email")
                    If ToName <> "" And ToEmail <> "" Then
                        Email.SendSimpleMail(FromEmailType.NoReply, ToEmail, ToName, "Request Call Back", sMsg, "")
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub LoadDropdown()
        Try
            drCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            drCountry.DataBind()
            drCountry.SelectedValue = "US"
            Dim dsLang As DataSet = RequestCallBackLanguageRow.GetEmailLanguages(DB)
            drLang.DataSource = dsLang
            drLang.DataTextField = "Language"
            drLang.DataValueField = "LanguageId"
            drLang.DataBind()
            drLang.SelectedValue = "5"
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadDropdown()
        End If

    End Sub
End Class
