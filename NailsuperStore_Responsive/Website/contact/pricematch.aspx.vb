Imports Components
Imports DataLayer

Partial Class pricematch_default
	Inherits SitePage

    Private Sub SendMail(ByVal PriceMatchId As Integer)
        Dim strItem As String = ""
        Dim dtItem As New DataTable
        Dim i As Integer = 0
        Dim dbPriceMatch As PriceMatchRow = PriceMatchRow.GetRow(DB, PriceMatchId)
        dtItem = DB.GetDataTable("select * from pricematchitem where pricematchid = " & PriceMatchId)
        strItem = "<br />" & strItem & "Your Name: " & dbPriceMatch.YourName & "<br />"

        strItem = strItem & "Phone Number: " & dbPriceMatch.PhoneNumber & "<br />"
        strItem = strItem & "Email Address: " & dbPriceMatch.EmailAddress & "<br />"
        strItem = strItem & "Competitor's Company Name: " & dbPriceMatch.CompetitorsCompanyName & "<br />"
        strItem = strItem & "Competitor's Phone Number: " & dbPriceMatch.CompetitorsPhoneNumber & "<br />"
        strItem = strItem & "Competitor is Website: " & dbPriceMatch.CompetitorsWebsite & "<br />"
        strItem = strItem & "ITEMS:" & "<br />"
        strItem = strItem & "<table width='400px' border='1' style='border-color:#FFC; border-style:solid'><tr style='background-color:#CCC; padding:2px; color:black'><td style='width:50%'>Item # or Product Name : </td><td>Competitor Price </td></tr>"
        For i = 0 To dtItem.Rows.Count - 1
            strItem = strItem & "<tr><td>" & dtItem.Rows(i)("ItemNumberProductName") & "</td><td>" & dtItem.Rows(i)("CompetitorPrice") & "</td></tr>"
        Next
        strItem = strItem & "</table>"

        Dim strTitle As String = txtYourName.Text & " have Price Match Request on " & Now() & " :"
        Dim Usernameto As String = "Kevin"
        Dim Subject As String = "Price Match Email Alert"
        strItem = strTitle & vbCrLf & strItem & vbCrLf
        Dim lb As New Label
        lb.Text = strItem
        Dim mailTo As String = String.Empty
        Dim mailtoName As String = String.Empty
        Dim mailBCC As String = String.Empty
        Dim res As DataTable = DB.GetDataTable("select * from Vie_ContactUsSubjectEmail where subjectid='10'") ' & DB.Quote(SubjectId))
        If res.Rows.Count > 0 Then
            Dim j As Integer
            For j = 0 To res.Rows.Count - 1
                If String.IsNullOrEmpty(mailTo) Then
                    mailTo = res.Rows(j)("Email")
                    mailtoName = res.Rows(j)("Name")
                Else
                    mailBCC = res.Rows(j)("Email") & "," & mailBCC
                End If
            Next
        End If
        If mailTo <> "" Then
            'Response.Write(strItem)
            If String.IsNullOrEmpty(mailBCC) Then
                Email.SendHTMLMail(FromEmailType.NoReply, mailTo, mailtoName, Subject, strItem)
            Else
                If mailBCC.LastIndexOf(",") = mailBCC.Length - 1 Then
                    mailBCC = mailBCC.Substring(0, mailBCC.Length - 1)
                End If
                Email.SendHTMLMail(FromEmailType.NoReply, mailTo, mailtoName, Subject, strItem, mailBCC)
            End If


        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadInfo()
        End If
    End Sub
    'End 05/12/2009

    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(e.Value) Then
            e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        End If
        If Not String.IsNullOrEmpty(txtPhoneNumber.Text) Then
            Dim array() As String = txtPhoneNumber.Text.Trim().Split(" ")
            Dim phone1 As String = String.Empty
            Dim phone2 As String = String.Empty
            Dim phone3 As String = String.Empty
            Dim phoneExt As String = String.Empty
            Try
                Dim tmp As String = array(0)
                If Not String.IsNullOrEmpty(tmp) Then
                    Dim lstPhone() As String = tmp.Split("-")
                    phone1 = lstPhone(0)
                    phone2 = lstPhone(1)
                    phone3 = lstPhone(2)
                End If
                If array.Length > 1 Then
                    phoneExt = array(1)
                End If

                If (String.IsNullOrEmpty(phone1) And String.IsNullOrEmpty(phone2) And String.IsNullOrEmpty(phone3)) Then
                    e.IsValid = False
                    Exit Sub
                End If
                e.IsValid = Utility.Common.CheckUSPhoneValid(phone1, phone2, phone3)
                If (e.IsValid) Then
                    e.IsValid = CheckPhoneExt(phoneExt.Trim())
                End If
                If (e.IsValid = False) Then
                    cusvPhoneUS.ErrorMessage = "Phone Number is invalid"
                End If
            Catch ex As Exception
                e.IsValid = False
                cusvPhoneUS.ErrorMessage = "Phone Number is invalid"
            End Try
        End If

    End Sub

    Protected Sub ServerCheckPhoneUS2(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(e.Value) Then
            e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        End If
        If Not String.IsNullOrEmpty(txtCompetitorsPhoneNumber.Text) Then
            Dim array() As String = txtCompetitorsPhoneNumber.Text.Trim().Split(" ")
            Dim phone1 As String = String.Empty
            Dim phone2 As String = String.Empty
            Dim phone3 As String = String.Empty
            Dim phoneExt As String = String.Empty
            Try
                Dim tmp As String = array(0)
                If Not String.IsNullOrEmpty(tmp) Then
                    Dim lstPhone() As String = tmp.Split("-")
                    phone1 = lstPhone(0)
                    phone2 = lstPhone(1)
                    phone3 = lstPhone(2)
                End If
                If array.Length > 1 Then
                    phoneExt = array(1)
                End If

                If (String.IsNullOrEmpty(phone1) And String.IsNullOrEmpty(phone2) And String.IsNullOrEmpty(phone3)) Then
                    e.IsValid = False
                    Exit Sub
                End If
                e.IsValid = Utility.Common.CheckUSPhoneValid(phone1, phone2, phone3)
                If (e.IsValid) Then
                    e.IsValid = CheckPhoneExt(phoneExt.Trim())
                End If
                If (e.IsValid = False) Then
                    cusPhoneUS2.ErrorMessage = "Competitors Phone Number is invalid"
                End If
            Catch ex As Exception
                e.IsValid = False
                cusPhoneUS2.ErrorMessage = "Competitors Phone Number is invalid"
            End Try
        End If

    End Sub

    Protected Function CheckPhoneExt(ByVal ext As String) As Boolean
        If (String.IsNullOrEmpty(ext)) Then
            Return True
        End If
        If (ext.Length > 4) Then
            Return False
        Else

        End If
        Dim nPhone As Integer = -1
        Try
            nPhone = CInt(ext)
        Catch ex As Exception
            nPhone = -1
        End Try
        If (nPhone < 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub ServerCheckItem(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        'Dim hasOne As Boolean = False
        'If Trim(tbItem1.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice1.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #1")
        '    End If
        'End If
        'If Trim(tbItem2.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice2.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #2")
        '    End If
        'End If
        'If Trim(tbItem3.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice3.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #3")
        '    End If
        'End If
        'If Trim(tbItem4.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice4.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #4")
        '    End If
        'End If
        'If Trim(tbItem5.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice5.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #5")
        '    End If
        'End If
        'If Trim(tbItem6.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice6.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #6")
        '    End If
        'End If
        'If Trim(tbItem7.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice7.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #7")
        '    End If
        'End If
        'If Trim(tbItem8.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice8.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #8")
        '    End If
        'End If
        'If Trim(tbItem9.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice9.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #9")
        '    End If
        'End If
        'If Trim(tbItem10.Text) <> Nothing Then
        '    hasOne = True
        '    If Trim(tbPrice10.Text) = Nothing Then
        '        bError = True
        '        AddError("Please specify a price for item #10")
        '    End If
        'End If
        'If Not hasOne Then
        '    bError = True
        '    AddError("Please specify at least one item and price")
        'End If
    End Sub

    Protected Sub ServerCheckValidEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(txtEmailAddress.Text) Then
            Dim email As String = txtEmailAddress.Text.Trim()
            If Not Utility.Common.CheckValidEmail(email) Then
                e.IsValid = False
            End If
        End If
    End Sub
    Dim bError As Boolean = False
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' Page.Validate()
        'If Not IsValid Then bError = True
        'If bError Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbPriceMatch As PriceMatchRow = New PriceMatchRow(DB)
            dbPriceMatch.YourName = txtYourName.Text
            dbPriceMatch.PhoneNumber = txtPhoneNumber.Text
            dbPriceMatch.EmailAddress = txtEmailAddress.Text
            dbPriceMatch.CompetitorsCompanyName = txtCompetitorsCompanyName.Text
            dbPriceMatch.CompetitorsPhoneNumber = txtCompetitorsPhoneNumber.Text
            dbPriceMatch.CompetitorsWebsite = txtCompetitorsWebsite.Text

            dbPriceMatch.Insert()

            Dim i As PriceMatchItemRow
            If Trim(tbItem1.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem1.Text
                i.CompetitorPrice = tbPrice1.Text
                i.Insert()
            End If
            If Trim(tbItem2.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem2.Text
                i.CompetitorPrice = tbPrice2.Text
                i.Insert()
            End If
            If Trim(tbItem3.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem3.Text
                i.CompetitorPrice = tbPrice3.Text
                i.Insert()
            End If
            If Trim(tbItem4.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem4.Text
                i.CompetitorPrice = tbPrice4.Text
                i.Insert()
            End If
            If Trim(tbItem5.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem5.Text
                i.CompetitorPrice = tbPrice5.Text
                i.Insert()
            End If
            If Trim(tbItem6.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem6.Text
                i.CompetitorPrice = tbPrice6.Text
                i.Insert()
            End If
            If Trim(tbItem7.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem7.Text
                i.CompetitorPrice = tbPrice7.Text
                i.Insert()
            End If
            If Trim(tbItem8.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem8.Text
                i.CompetitorPrice = tbPrice8.Text
                i.Insert()
            End If
            If Trim(tbItem9.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem9.Text
                i.CompetitorPrice = tbPrice9.Text
                i.Insert()
            End If
            If Trim(tbItem10.Text) <> Nothing Then
                i = New PriceMatchItemRow(DB)
                i.PriceMatchId = dbPriceMatch.PriceMatchId
                i.ItemNumberProductName = tbItem10.Text
                i.CompetitorPrice = tbPrice10.Text
                i.Insert()
            End If

            DB.CommitTransaction()
            SendMail(dbPriceMatch.PriceMatchId)
        Catch ex As Exception
            DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
            Exit Sub
        End Try

        'lit.Text = "<p></p><p>Thank you for your inquiry. Your submission will be reviewed by a customer service representative.</p>"
        'pnlForm.Visible = False

        GoMsg("Thank you for your inquiry. Your submission will be reviewed by a customer service representative.")
    End Sub

    Private Sub LoadInfo()
        If HasAccess() Then
            Dim o As StoreOrderRow = Cart.Order
            Dim CurrentMemberId As Integer = 0
            Dim Member As MemberRow = Nothing
            Dim MemberBillingAddress As MemberAddressRow = Nothing

            CurrentMemberId = Session("memberId")
            If Member Is Nothing Then Member = MemberRow.GetRow(Session("MemberId"))
            MemberBillingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Billing Address'"))
            txtYourName.Text = MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName
            txtPhoneNumber.Text = MemberBillingAddress.Phone
            txtEmailAddress.Text = MemberBillingAddress.Email

        End If
    End Sub
End Class
