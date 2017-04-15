Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Drawing

Public Class admin_navision_mixmatch_Edit
    Inherits AdminPage
    '--------------VARIABLES----------------------
    Protected Id As Integer = 0
    Protected iType As Integer = 0

    '--------------METHODS------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Id = Convert.ToInt32(Request("Id"))
            iType = CInt(Request.QueryString("Type"))
        Catch ex As Exception
        End Try

        If Not IsPostBack Then
            LoadDefaut()
            LoadFromDB()
        End If

    End Sub

    Private Sub LoadDefaut()
        trMobileImage.Visible = False
        fuImage.Folder = Utility.ConfigData.PathPromotion
        fuImage.ImageDisplayFolder = Utility.ConfigData.PathPromotion
        fuMobileImage.Folder = Utility.ConfigData.PathPromotionMobile
        fuMobileImage.ImageDisplayFolder = Utility.ConfigData.PathPromotionMobile

        If iType = 2 Then
            ltrHeader.Text = "Strip banner"
            trDepartment.Visible = True
            LoadDepartment()
        ElseIf iType = 1 Then
            ltrHeader.Text = "Left banner"
            trDepartment.Visible = True
            LoadDepartment()
        ElseIf iType = 0 Then
            ltrHeader.Text = "Strip banner"
            trMobileImage.Visible = True
        ElseIf iType = 3 Then
            ltrHeader.Text = "Exclusive Offers"
        ElseIf iType = 4 Then
            ltrHeader.Text = "Bonus Offers"
        End If
    End Sub

    Private Sub LoadFromDB()
        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbProSales As PromotionSalespriceRow = PromotionSalespriceRow.GetRow(DB, Id)
        txtSubTitle.Text = dbProSales.SubTitle
        txtMainTitle.Text = dbProSales.MainTitle
        txtLinkPage.Text = dbProSales.LinkPage
        dtStartingDate.Value = dbProSales.StartingDate
        dtEndingDate.Value = dbProSales.EndingDate
        chkIsActive.Checked = dbProSales.IsActive
        txtTextHtml.Text = dbProSales.TextHtml
        ddlDepartment.SelectedValue = dbProSales.DepartmentID
        fuImage.CurrentFileName = dbProSales.Image
        If iType = 0 Then
            fuMobileImage.CurrentFileName = dbProSales.MobileImage
            hpimgMobile.ImageUrl = Utility.ConfigData.PathPromotionMobile & dbProSales.MobileImage & "?d=" & DateTime.Now.Second.ToString()
            If fuMobileImage.CurrentFileName <> Nothing Then
                divImgMobile.Visible = True

            End If
        End If
        hpimg.ImageUrl = Utility.ConfigData.PathPromotion & dbProSales.Image & "?d=" & DateTime.Now.Second.ToString()

        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbProSales.Image) & """></script>"
        End If
    End Sub

    Private Sub LoadDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "Name"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        If iType = 1 Then
            ddlDepartment.Items.Insert(0, New ListItem("-- Home Page --", 0))
        End If

        ds.Dispose()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try

            Dim logDetail As New AdminLogDetailRow
            Dim dbProSales As PromotionSalespriceRow
            Dim dbProSalesbefore As New PromotionSalespriceRow
            If Id > 0 Then
                dbProSales = PromotionSalespriceRow.GetRow(DB, Id)
                dbProSalesbefore = CloneObject.Clone(dbProSales)
            Else
                dbProSales = New PromotionSalespriceRow(DB)
            End If

            dbProSales.SubTitle = txtSubTitle.Text
            dbProSales.MainTitle = txtMainTitle.Text
            dbProSales.LinkPage = txtLinkPage.Text
            dbProSales.StartingDate = dtStartingDate.Value
            dbProSales.EndingDate = dtEndingDate.Value
            dbProSales.IsActive = chkIsActive.Checked
            dbProSales.TextHtml = txtTextHtml.Text
            dbProSales.Type = iType.ToString().Trim()
            If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(dbProSales.Image) Then
                ltMessage.Text = "Image is required."
                Exit Sub
            End If
            If iType = 0 Then
                If String.IsNullOrEmpty(fuMobileImage.NewFileName) AndAlso String.IsNullOrEmpty(dbProSales.MobileImage) Then
                    ltImageMobileMessage.Text = "Mobile Image is required."
                    Exit Sub
                End If
            End If
            If dbProSales.DepartmentID > 0 Or iType = 2 Or iType = 1 Then
                dbProSales.DepartmentID = IIf(String.IsNullOrEmpty(ddlDepartment.SelectedValue), 0, ddlDepartment.SelectedValue)
            End If
      
            If fuImage.NewFileName <> Nothing Then
                If Utility.Common.IsBannerWidthValid(fuImage, 1140) = False Then
                    ltMessage.Text = String.Format(Resources.Admin.BannerWidth, "Image", "1140")
                    Exit Sub
                End If

            End If
            If iType = 0 Then
                If (fuMobileImage.NewFileName <> Nothing) Then
                    If Utility.Common.IsBannerWidthValid(fuMobileImage, Utility.ConfigData.DefaultMobileBannerWidth) = False Then
                        ltImageMobileMessage.Text = String.Format(Resources.Admin.BannerWidth, "Mobile Image", Utility.ConfigData.DefaultMobileBannerWidth.ToString())
                        Exit Sub
                    End If
                End If
                

            End If
            Dim arr As String()
            If Id <> 0 Then
                dbProSales.Update()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                Id = dbProSales.AutoInsert
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            If Id <> 0 Then
                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = Id.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    dbProSales.Image = fuImage.NewFileName
                    dbProSales.Update()
                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbProSales.Image = Nothing
                    dbProSales.Update()
                End If
                If iType = 0 Then
                    If fuMobileImage.NewFileName <> String.Empty Then
                        arr = fuMobileImage.NewFileName.Split(".")
                        fuMobileImage.NewFileName = Id.ToString & "." & arr(1)
                        fuMobileImage.SaveNewFile()
                        dbProSales.MobileImage = fuMobileImage.NewFileName
                        dbProSales.Update()
                    ElseIf fuMobileImage.MarkedToDelete Then
                        fuMobileImage.RemoveOldFile()
                        dbProSales.MobileImage = Nothing
                        dbProSales.Update()
                    End If
                End If
            End If

            If (logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()) Then
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.StripBanner, dbProSalesbefore, dbProSales)
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbProSales, Utility.Common.ObjectType.StripBanner)
            End If
            logDetail.ObjectId = Id
            logDetail.ObjectType = Utility.Common.ObjectType.StripBanner.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?Type=" & iType)
        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?Type=" & iType)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & Id & "&Type=" & iType)
    End Sub
    Public Function GetImageWidth(ByVal ImgFile As String) As Integer
        Try
            Dim newImage As Image = Image.FromFile(ImgFile)
            Return newImage.Width
        Catch ex As Exception
        End Try
    End Function

End Class

