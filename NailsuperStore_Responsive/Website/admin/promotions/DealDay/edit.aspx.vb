Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_promotions_dealday_edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            InitData()
            LoadFromDB()
        End If

    End Sub
   
    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbDealDay As DealDayRow = DealDayRow.GetRow(DB, Id)
        txtTitle.Text = dbDealDay.Title
        dtStartingDate.Value = dbDealDay.StartDate.Date
        txtStartHour.Text = dbDealDay.StartDate.Hour
        If dbDealDay.StartDate.Minute < 10 Then
            txtStartMinute.Text = "0" & dbDealDay.StartDate.Minute
        Else
            txtStartMinute.Text = dbDealDay.StartDate.Minute
        End If

        chkIsActive.Checked = dbDealDay.IsActive
        dtEndingDate.Value = dbDealDay.EndDate.Date
        txtEndHour.Text = dbDealDay.EndDate.Hour
        If dbDealDay.EndDate.Minute < 10 Then
            txtEndMinute.Text = "0" & dbDealDay.EndDate.Minute
        Else
            txtEndMinute.Text = dbDealDay.EndDate.Minute
        End If
        Dim ItemSku As StoreItemRow = StoreItemRow.GetRow(DB, dbDealDay.ItemId)
        txtSKU.Text = ItemSku.SKU
        txtMetaDescription.Text = dbDealDay.MetaDescription
        txtMetaKeyword.Text = dbDealDay.MetaKeyword
        txtPageTitle.Text = dbDealDay.PageTitle
        fuImage.CurrentFileName = dbDealDay.BannerImage
        hpimg.ImageUrl = ConfigurationManager.AppSettings("PathDealDay") & dbDealDay.BannerImage
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbDealDay.BannerImage) & """></script>"
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then

            If ValidateData() Then
                Try

                    Dim dbDealDayRow As DealDayRow
                    If Id <> 0 Then
                        dbDealDayRow = DealDayRow.GetRow(DB, Id)
                    Else
                        dbDealDayRow = New DealDayRow(DB)
                    End If
                    dbDealDayRow.Title = txtTitle.Text
                    Dim dateVariable As DateTime = New DateTime(dtStartingDate.Value.Year, dtStartingDate.Value.Month, dtStartingDate.Value.Day, CInt(txtStartHour.Text), CInt(txtStartMinute.Text), 0)
                    dbDealDayRow.StartDate = dateVariable
                    dateVariable = New DateTime(dtEndingDate.Value.Year, dtEndingDate.Value.Month, dtEndingDate.Value.Day, CInt(txtEndHour.Text), CInt(txtEndMinute.Text), 0)
                    dbDealDayRow.EndDate = dateVariable
                    dbDealDayRow.IsActive = chkIsActive.Checked
                    dbDealDayRow.CreatedDate = Date.Now
                    dbDealDayRow.MetaKeyword = txtMetaKeyword.Text
                    dbDealDayRow.MetaDescription = txtMetaDescription.Text
                    dbDealDayRow.PageTitle = txtPageTitle.Text
                    dbDealDayRow.ItemId = itemID
                    fuImage.Width = 754
                    fuImage.Height = 320
                    fuImage.AutoResize = True
                    If fuImage.NewFileName <> String.Empty Then

                        fuImage.SaveNewFile()
                        dbDealDayRow.BannerImage = fuImage.NewFileName
                        Dim ImagePath As String = Server.MapPath("/assets/dealday/")
                        Core.ResizeImage(ImagePath & fuImage.NewFileName, ImagePath & "mobile/" & fuImage.NewFileName, 320, 140)
                    ElseIf fuImage.MarkedToDelete Then
                        dbDealDayRow.BannerImage = Nothing
                    End If
                   
                    If Id <> 0 Then
                        dbDealDayRow.Update()
                    Else
                        Id = dbDealDayRow.Insert()
                    End If
                    If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
                    Response.Redirect("default.aspx")
                Catch ex As SqlException
                    AddError(ErrHandler.ErrorText(ex))
                End Try
            End If
        End If
    End Sub
    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub
    Private Function ValidateData() As Boolean
        ''check reuire StartDate
        Dim message As String = String.Empty
        If dtStartingDate.Text = String.Empty Then
            message = "Field 'Starting Date' is blank"
            ViewError(message)
            Return False
        End If
        If txtStartHour.Text = String.Empty Then
            message = "Field 'Hour of Starting Date' is blank"
            ViewError(message)
            Return False
        End If
        If txtStartMinute.Text = String.Empty Then
            message = "Field ' Minute of Starting Date' is blank"
            ViewError(message)
            Return False
        End If

        If dtEndingDate.Text = String.Empty Then
            message = "Field 'Ending Date' is blank"
            ViewError(message)
            Return False
        End If
        If txtEndHour.Text = String.Empty Then
            message = "Field 'Hour of Ending Date' is blank"
            ViewError(message)
            Return False
        End If
        If txtEndMinute.Text = String.Empty Then
            message = "Field ' Minute of Ending Date' is blank"
            ViewError(message)
            Return False
        End If
        ''check housr is valid
        Dim hour As Integer = CInt(txtStartHour.Text)
        If hour < 0 And hour > 24 Then
            message = "Field ' Hour of Starting Date' must be between 0 and 24"
            ViewError(message)
            Return False
        End If
        hour = CInt(txtEndHour.Text)
        If hour < 0 And hour > 24 Then
            message = "Field ' Hour of Ending Date' must be between 0 and 24"
            ViewError(message)
            Return False
        End If
        ''check Seconds is Valid
        hour = CInt(txtStartMinute.Text)
        If hour < 0 And hour > 60 Then
            message = "Field ' Minute of Starting Date' must be between 0 and 60"
            ViewError(message)
            Return False
        End If
        hour = CInt(txtEndMinute.Text)
        If hour < 0 And hour > 60 Then
            message = "Field ' Minute of Ending Date' must be between 0 and 60"
            ViewError(message)
            Return False
        End If
        ''check EndDate more than StartDate
        Dim startDate As DateTime = New DateTime(dtStartingDate.Value.Year, dtStartingDate.Value.Month, dtStartingDate.Value.Day, CInt(txtStartHour.Text), CInt(txtStartMinute.Text), 0)
        Dim endDate As DateTime = New DateTime(dtEndingDate.Value.Year, dtEndingDate.Value.Month, dtEndingDate.Value.Day, CInt(txtEndHour.Text), CInt(txtEndMinute.Text), 0)
        If (endDate < startDate) Then
            message = "EndDate must be more than StartDate"
            ViewError(message)
            Return False
        End If
        ''check SKU is Valid
        If txtSKU.Text = String.Empty Then
            message = "Field 'SKU' is blank"
            ViewError(message)
            Return False
        End If
        Dim ItemSku As StoreItemRow = StoreItemRow.GetRowSku(DB, txtSKU.Text)
        If ItemSku Is Nothing Or ItemSku.ItemId <= 0 Then
            message = "Field 'SKU' is not valid. Please input another value. "
            ViewError(message)
            Return False
        End If
        itemID = ItemSku.ItemId
        Return True
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub InitData()
        txtSKU.Attributes.Add("readonly", "readonly")
        ''txtSKU.Attributes.Add("disabled", "disabled")
        Dim now As DateTime = Date.Now
        dtStartingDate.Value = now.Date
        dtEndingDate.Value = now.AddDays(1).Date
    End Sub

   

  

 
End Class
