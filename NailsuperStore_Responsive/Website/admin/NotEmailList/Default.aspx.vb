
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class Admin_NotEmailList_default
    Inherits AdminPage
    Private m_dtCSV As DataTable
    Private m_iColumnCount As Int32

    Protected Sub ctlImportButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctlImportButton.Click
        btnRemove.Visible = False
        txtEmail.Text = ""
        txtNoOrderEmail.Text = ""
        txtOrderEmail.Text = ""
        lblMsg.Text = ""
        If ctlCSVFile.Value <> "" Then
            If ctlCSVFile.Value.IndexOf(".csv") = -1 Then
                AddError("Invalid file type - csv files only. Please check again")
                Exit Sub
            End If

            Try
                txtEmail.Text = ""
                PopulateDataTableFromUploadedFile(ctlCSVFile.PostedFile.InputStream)

                ctlGridView.DataSource = m_dtCSV
                ctlGridView.DataBind()
                ctlGridView.Visible = True
                btnRemove.Visible = True
            Catch ex As Exception
                AddError("The file you choose not exist. Please check again")
                Exit Sub
            End Try
            If txtEmail.Text.Trim <> "" Then
                GetOrderEmail()
                GetNoOrderEmail()
            Else
                AddError("No data to remove. Please check again.")
                Exit Sub
            End If

        Else
            AddError("Please choose a file to import.")
            Exit Sub
        End If
    End Sub

    Private Sub PopulateDataTableFromUploadedFile(ByVal strm As System.IO.Stream)

        Dim srdr As System.IO.StreamReader = New System.IO.StreamReader(strm)
        Dim strLine As String = String.Empty
        Dim iLineCount As Int32 = 0

        While (True)

            strLine = srdr.ReadLine()

            If iLineCount = 0 Then
                m_dtCSV = CreateDataTableForCSVData(strLine)
            End If
            If strLine <> Nothing Then
                If iLineCount > 0 Then
                    AddDataRowToTable(strLine, m_dtCSV)
                    If txtEmail.Text.Trim = "" Then
                        txtEmail.Text = "'" & strLine.Replace("""", "") & "'"
                    Else
                        txtEmail.Text = txtEmail.Text & ",'" & strLine.Replace("""", "") & "'"
                    End If

                End If
            Else
                Exit While
            End If

            iLineCount = iLineCount + 1
        End While

    End Sub
    Private Function CreateDataTableForCSVData(ByVal strLine As String) As DataTable
        Dim dt As DataTable = New DataTable("CSVTable")
        Dim strVals As String() = strLine.Split(New Char() {","})
        m_iColumnCount = strVals.Length
        Dim idx As Integer = 0
        Dim strVal As String = ""
        Dim strColumnName As String = ""
        For Each strVal In strVals
            strColumnName = String.Format("EmailAddress", idx = idx + 1)
            dt.Columns.Add(strColumnName, Type.GetType("System.String"))
        Next strVal

        Return dt
    End Function

    Private Function AddDataRowToTable(ByVal strCSVLine As String, ByVal dt As DataTable) As DataRow

        Dim strVals As String() = strCSVLine.Split(New Char() {","})
        Dim iTotalNumberOfValues As Int32 = strVals.Length
        ' If number of values in this line are more than the columns
        ' currently in table, then we need to add more columns to table.
        If iTotalNumberOfValues > m_iColumnCount Then
            Dim i As Integer = 0
            Dim iDiff As Int32 = iTotalNumberOfValues - m_iColumnCount
            Dim strColumnName As String = ""
            For i = 0 To iDiff - 1
                strColumnName = String.Format("EmailAddress", (m_iColumnCount + i))
                dt.Columns.Add(strColumnName, Type.GetType("System.String"))
            Next

            m_iColumnCount = iTotalNumberOfValues
        End If
        Dim idx As Integer = 0
        Dim drow As DataRow = dt.NewRow()
        Dim strVal As String = ""
        Dim strColumnName1 As String = ""
        For Each strVal In strVals
            strColumnName1 = String.Format("EmailAddress", idx = idx + 1)
            drow(strColumnName1) = strVal.Trim()
        Next

        dt.Rows.Add(drow)

        Return drow
    End Function


    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        lblMsg.Text = ""
        Try
            If txtOrderEmail.Text.Trim = "" Then
                txtOrderEmail.Text = "''"
            End If

            DeleteStoreCatalogRequest()
            DeleteMailingMember()
            DeleteStoreOrder()
            DeleteMemberAddress()
            DeleteMember()
            DeleteCustomer()
            txtEmail.Text = ""
            btnRemove.Visible = False
            ctlGridView.Visible = False
            If lblMsg.Text.Trim = "" Then
                lblMsg.Text = "In database have not email data from email list to remove."
            End If
        Catch ex As Exception
            AddError("Error while delete data. The file you choose not exist. Please check again")
            Exit Sub
        End Try

    End Sub
    Private Sub DeleteMemberAddress()
        If txtNoOrderEmail.Text.Trim <> "" Then
            Dim sSQL As String = " from memberaddress "
            sSQL += "where memberid in "
            sSQL += "(select memberid from member where customerid in "
            sSQL += "(select customerid from customer where email in (" & txtNoOrderEmail.Text.Trim & ")))"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Member Address."
            End If
        End If
    End Sub

    Private Sub DeleteMember()
        If txtNoOrderEmail.Text.Trim <> "" Then
            Dim sSQL As String = " from member where customerid in "
            sSQL += "(select customerid from customer where email in (" & txtNoOrderEmail.Text.Trim & "))"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Member."
            End If
        End If
    End Sub

    Private Sub DeleteCustomer()
        If txtNoOrderEmail.Text.Trim <> "" Then
            Dim sSQL As String = " from customer where email in (" & txtNoOrderEmail.Text.Trim & ")"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Customer."
            End If
        End If
    End Sub

    Private Sub DeleteStoreOrder()
        If txtNoOrderEmail.Text.Trim <> "" Then
            Dim sSQL As String = " from StoreOrder where email in (" & txtNoOrderEmail.Text.Trim & ")"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Shopping Cart."
            End If
        End If
    End Sub

    Private Sub DeleteMailingMember()
        Dim sEmail As String = ""
        Dim sSQL As String = ""
        If txtNoOrderEmail.Text.Trim <> "" Then
            sEmail = txtNoOrderEmail.Text.Trim
        End If
        If sEmail <> "" Then
            If txtOrderEmail.Text.Trim <> "" Then
                sEmail = sEmail & "," & txtOrderEmail.Text.Trim
            End If
        Else
            sEmail = txtOrderEmail.Text.Trim
        End If
        If sEmail <> "" Then
            sSQL = " from MailingMember where email not in (select email from MailingRecipient) AND email in (" & sEmail & ")"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Mailing Member."
            End If
        End If


    End Sub

    Private Sub DeleteStoreCatalogRequest()
        Dim sEmail As String = ""
        Dim sSQL As String = ""
        If txtNoOrderEmail.Text.Trim <> "" Then
            sEmail = txtNoOrderEmail.Text.Trim
        End If
        If sEmail <> "" Then
            If txtOrderEmail.Text.Trim <> "" Then
                sEmail = sEmail & "," & txtOrderEmail.Text.Trim
            End If
        Else
            sEmail = txtOrderEmail.Text.Trim
        End If
        If sEmail <> "" Then
            sSQL = " from StoreCatalogRequest where email not in (select email from MailingRecipient) AND email in (" & sEmail & ")"
            If DB.ExecuteScalar(" select * " & sSQL) > 0 Then
                sSQL = " delete " & sSQL
                Dim i As Integer = 0
                i = DB.ExecuteSQL(sSQL)
                lblMsg.Text = lblMsg.Text & "<br> Removed " & CStr(i) & " Store Catalog Request."
            End If
        End If

    End Sub

    Private Sub GetOrderEmail()
        Dim sSQL As String = "Select distinct Email from StoreOrder where Email in (" & txtEmail.Text.Trim & ")  AND OrderNo is not null"
        Dim dt As DataTable = DB.GetDataTable(sSQL)
        Dim i As Integer = 0
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If txtOrderEmail.Text.Trim = "" Then
                    txtOrderEmail.Text = "'" & dt.Rows(i)("Email") & "'"
                Else
                    txtOrderEmail.Text = txtOrderEmail.Text & ",'" & dt.Rows(i)("Email") & "'"
                End If
            Next
        End If
        'txtOrderEmail.Text = txtOrderEmail.Text & "total: " & dt.Rows.Count - 1
    End Sub

    Private Sub GetNoOrderEmail()

        Dim arrOrderEmail() As String = Nothing
        Dim p As String = ""
        Dim i As Integer = 0
        Dim strEmail As String = ""
        If txtOrderEmail.Text.Trim <> "" Then
            strEmail = txtEmail.Text.Trim.Replace("'", "")
            arrOrderEmail = strEmail.Split(",")
            For Each p In arrOrderEmail
                If txtOrderEmail.Text.Trim.IndexOf(p) = -1 Then
                    If i = 0 Then
                        txtNoOrderEmail.Text = "'" & p & "'"
                    Else
                        txtNoOrderEmail.Text = txtNoOrderEmail.Text & ",'" & p & "'"
                    End If
                    i = i + 1
                End If
            Next
        Else
            txtNoOrderEmail.Text = txtEmail.Text
        End If
        'txtNoOrderEmail.Text = txtNoOrderEmail.Text & "total: " & i.ToString
    End Sub

End Class
