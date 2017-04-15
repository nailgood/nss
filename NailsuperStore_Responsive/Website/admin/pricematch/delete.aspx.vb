Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_pricematch_delete
    Inherits AdminPage
    Protected s As ContactUsSubjectRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim PriceMatchId As Integer = Convert.ToInt32(Request("PriceMatchId"))
        'Try
        SendMail(PriceMatchId)
        'Catch ex As Exception

        'End Try
        Try

            DB.BeginTransaction()
            PriceMatchRow.RemoveRow(DB, PriceMatchId)
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Private Sub SendMail(ByVal PriceMatchId As Integer)
        Dim strItem As String = ""
        Dim dtItem As New DataTable
        Dim i As Integer = 0
        Dim dbPriceMatch As PriceMatchRow = PriceMatchRow.GetRow(DB, PriceMatchId)
        dtItem = DB.GetDataTable("select * from pricematchitem where pricematchid = " & PriceMatchId)
        strItem = strItem & "Your Name: " & dbPriceMatch.YourName & vbCrLf

        strItem = strItem & "Phone Number: " & dbPriceMatch.PhoneNumber & vbCrLf
        strItem = strItem & "Email Address: " & dbPriceMatch.EmailAddress & vbCrLf
        strItem = strItem & "Competitor's Company Name: " & dbPriceMatch.CompetitorsCompanyName & vbCrLf
        strItem = strItem & "Competitor's Phone Number: " & dbPriceMatch.CompetitorsPhoneNumber & vbCrLf
        strItem = strItem & "Competitor is Website: " & dbPriceMatch.CompetitorsWebsite & vbCrLf


        strItem = strItem & "ITEMS" & vbCrLf

        'strItem = strItem & "Item # or Product Name | Competitor Price" & vbCrLf
        For i = 0 To dtItem.Rows.Count - 1
            strItem = strItem & "Item # or Product Name : "
            strItem = strItem & dtItem.Rows(i)("ItemNumberProductName") & " | " & "Competitor Price : " & dtItem.Rows(i)("CompetitorPrice") & vbCrLf
        Next

        Dim UserId As String = Session("AdminId")
        dtItem = DB.GetDataTable("select * from admin where adminid = " & UserId)
        Dim EmailFrom As String = dtItem.Rows(0)("Email")
        Dim Username As String = dtItem.Rows(0)("Username")
        Dim EmailTo As String = ConfigurationManager.AppSettings("EditPriceMatchEmail")
        Dim strTitle As String = Username & " have changed the Price Match on " & Now() & " :"
        Dim Usernameto As String = "Kevin"
        Dim Subject As String = "Price Match Email Alert"
        strItem = strTitle & vbCrLf & strItem & vbCrLf
        Dim lb As New Label
        lb.Text = strItem
        's = ContactUsSubjectRow.GetRow(DB, "10")
        'Core.SendSimpleMail(EmailFrom, Username, s.Email, s.Name, Subject, strItem)
        Dim res As DataTable = DB.GetDataTable("select * from Vie_ContactUsSubjectEmail where subjectid='10'") ' & DB.Quote(SubjectId))
        If res.Rows.Count > 0 Then
            Dim j As Integer
            Dim Name As String = ""
            Dim Email As String = ""
            For j = 0 To res.Rows.Count - 1
                Name = res.Rows(j)("Name")
                Email = res.Rows(j)("Email")
                If Name <> "" And Email <> "" Then
                    Core.SendSimpleMail(EmailFrom, Username, Email, Name, Subject, strItem)
                    ' Core.SendSimpleMail(Email, Name, txtEmailAddress.Text, txtFirstName.Text & " " & txtLastName.Text, sSubject, Msg)
                End If
            Next
        End If
    End Sub
End Class

