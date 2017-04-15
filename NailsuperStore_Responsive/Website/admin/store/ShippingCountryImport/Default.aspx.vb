
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_shippingcountry_default
    Inherits AdminPage
    Private m_dtCSV As DataTable
    Private m_iColumnCount As Int32

    Protected Sub ctlImportButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctlImportButton.Click
        btnRemove.Visible = False
        If ctlCSVFile.Value <> "" Then
            If ctlCSVFile.Value.IndexOf(".csv") = -1 Then
                AddError("Invalid file type - csv files only. Please check again")
                Exit Sub
            End If

            Try
                txtDataRow.Text = ""
                PopulateDataTableFromUploadedFile(ctlCSVFile.PostedFile.InputStream)

                ctlGridView.DataSource = m_dtCSV
                ctlGridView.DataBind()
                ctlGridView.Visible = True
                btnRemove.Visible = True
            Catch ex As Exception
                AddError("The file you choose not exist. Please check again")
                Exit Sub
            End Try

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
                    If txtDataRow.Text.Trim = "" Then
                        txtDataRow.Text = "'" & strLine.Replace("""", "") & "'"
                    Else
                        txtDataRow.Text = txtDataRow.Text & "###'" & strLine.Replace("""", "") & "'"
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
            strColumnName = String.Format(strVal, idx = idx + 1)
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
                strColumnName = String.Format(dt.Columns(i).ColumnName, (m_iColumnCount + i))
                dt.Columns.Add(strColumnName, Type.GetType("System.String"))
            Next

            m_iColumnCount = iTotalNumberOfValues
        End If
        Dim idx As Integer = 0
        Dim drow As DataRow = dt.NewRow()
        Dim strVal As String = ""
        Dim strColumnName1 As String = ""
        For Each strVal In strVals
            strColumnName1 = String.Format(dt.Columns(idx).ColumnName, idx = idx + 1)
            drow(strColumnName1) = strVal.Trim()
            idx = idx + 1
        Next

        dt.Rows.Add(drow)

        Return drow
    End Function

    Private Sub AddDataShipping()

        Dim strRows As String() = txtDataRow.Text.Split(New Char() {"###"})

        Dim i As Integer = 0
        Dim strRow As String = ""
        Dim strColumnName1 As String = ""
        Dim strVal As String = ""
        Dim dbShippingRange As ShippingRangeRow
        Dim dbCountry As CountryRow
        Dim dtCountry As DataTable
        Dim CountryCode As String = ""
        Dim strVals As String()
        Dim CountryId As Integer = 0
        Dim dtShippingRegion As DataTable

        Try
            DB.BeginTransaction()
            For Each strRow In strRows
                strVals = strRow.Replace("'", "").Split(New Char() {","})

                CountryCode = strVals(0)
                If CountryCode.Trim <> "" And CountryCode.Trim <> "US" Then
                    dtCountry = CountryRow.GetCountryByCountryCode(DB, CountryCode).Tables(0)
                    If dtCountry.Rows.Count > 0 Then
                        CountryId = dtCountry.Rows(0)("CountryId")
                        dbCountry = CountryRow.GetRow(DB, CountryId)
                        dbCountry.IsShippingActive = 1
                        dbCountry.ShippingCode = CountryCode
                        dbCountry.Update()
                    Else
                        AddError("There are a country code: " & CountryCode & " in file .csv not exist in database. Please check again or contact administrator")
                        DB.RollbackTransaction()
                        Exit Sub
                    End If

                    Dim ShippingRangeId As Integer = ShippingRangeRow.GetShippingRangeId(DB, CountryCode)


                    If ShippingRangeId > 0 Then
                        dbShippingRange = ShippingRangeRow.GetRow(DB, ShippingRangeId)
                    Else
                        dbShippingRange = New ShippingRangeRow(DB)
                    End If
                    dbShippingRange.LowValue = CountryCode
                    dbShippingRange.HighValue = CountryCode
                    dbShippingRange.FirstPoundOver = strVals(3)
                    dbShippingRange.AdditionalPound = strVals(4)
                    dbShippingRange.MethodId = 15
                    dbShippingRange.CountryId = CountryId
                    dbShippingRange.OverUnderValue = 0
                    dbShippingRange.AdditionalThreshold = 0

                    dtShippingRegion = ShippingRegionRow.GetListByRegionCode(strVals(2))
                    If dtShippingRegion.Rows.Count > 0 Then
                        dbShippingRange.RegionId = dtShippingRegion.Rows(0)("RegionId")
                    End If

                    If ShippingRangeId > 0 Then
                        dbShippingRange.Update()
                    Else
                        dbShippingRange.Insert()
                    End If

                    i = i + 1
                End If

            Next
            lblMsg.Text = "Add Shipping Rates: " & i.ToString()
            DB.CommitTransaction()
        Catch ex As Exception
            DB.RollbackTransaction()
        End Try
    End Sub



    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        lblMsg.Text = ""
        Try
            AddDataShipping()
            txtDataRow.Text = ""
            btnRemove.Visible = False
            ctlGridView.Visible = False
            'If lblMsg.Text.Trim = "" Then
            '    lblMsg.Text = "In database have not email data from email list to remove."
            'End If
        Catch ex As Exception
            AddError("Error while Add data. The file you choose not exist. Please check again")
            Exit Sub
        End Try

    End Sub

End Class
