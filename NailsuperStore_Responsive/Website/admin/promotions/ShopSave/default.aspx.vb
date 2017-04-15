Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility.Common
Imports Utility

Public Class admin_shopsave
    Inherits AdminPage
    Private TotalRecords As Integer
    Protected ShopSaveId As Integer
    Protected Type As Integer = 1
    Private PathFolderImage As String = Utility.ConfigData.ShopSaveBannerFolder
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("Type") <> Nothing AndAlso Request.QueryString("Type").Length > 0 Then
            Type = CInt(Request.QueryString("Type"))
            If Type <> ShopSaveType.SaveNow And Type <> ShopSaveType.ShopNow And Type <> ShopSaveType.DealOfDay And Type <> ShopSaveType.ShopYourWay And Type <> ShopSaveType.TopCategory And Type <> ShopSaveType.WeeklyEmail Then
                Type = ShopSaveType.ShopNow
            End If
        End If

        If Not IsPostBack Then
            fuBanner.ImageDisplayFolder = PathFolderImage
            fuBanner.Folder = PathFolderImage
            fuImage.Folder = PathFolderImage & "home/"
            fuImage.ImageDisplayFolder = PathFolderImage & "home/"
            fuImageMobile.Folder = PathFolderImage & "home/mobile/"
            fuImageMobile.ImageDisplayFolder = PathFolderImage & "home/mobile/"
            LoadList()
        End If
    End Sub

    Private Sub LoadDetail()

        pnList.Visible = False
        pnNew.Visible = True
        ltrHeader.Text = "Edit"
        Dim shopsave As ShopSaveRow = ShopSaveRow.GetRow(DB, ShopSaveId)
        hidBanner.Value = shopsave.Banner
        hidHomeBanner.Value = shopsave.HomeBanner
        hidMobileBanner.Value = shopsave.MobileBanner
        txtName.Text = shopsave.Name
        txtMetaKeyword.Text = shopsave.MetaKeyword
        txtMetaDescription.Text = shopsave.MetaDescription
        txtOutsideUSMetaDescription.Text = shopsave.OutsideUSMetaDescription
        chkIsActive.Checked = shopsave.IsActive
        txtShortContent.Text = shopsave.ShortContent
        txtContent.Text = shopsave.Content
        txtPageTitle.Text = shopsave.PageTitle
        txtOutsideUSPageTitle.Text = shopsave.OutsideUSPageTitle
        hidId.Value = ShopSaveId
        Type = shopsave.Type

        txtUrl.Text = shopsave.Url

        If Not String.IsNullOrEmpty(shopsave.Banner) Then
            fuBanner.CurrentFileName = shopsave.Banner
            hpBanner.ImageUrl = fuBanner.Folder & shopsave.Banner & "?d=" & DateTime.Now.Second.ToString()
        Else
            hpBanner.ImageUrl = String.Empty
        End If

        If Not String.IsNullOrEmpty(shopsave.HomeBanner) Then
            fuImage.CurrentFileName = shopsave.HomeBanner
            hpimg.ImageUrl = fuImage.Folder & shopsave.HomeBanner & "?d=" & DateTime.Now.Second.ToString()
            hpimg1.ImageUrl = fuImageMobile.Folder & shopsave.HomeBanner & "?d=" & DateTime.Now.Second.ToString()
        Else
            hpimg.ImageUrl = String.Empty
            hpimg1.ImageUrl = String.Empty
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
    End Sub
    Private Function GetTitle() As String
        If Type = ShopSaveType.SaveNow Then
            Return "Deals Center"
        ElseIf Type = ShopSaveType.DealOfDay Then
            Return "Deal of the day"
        ElseIf Type = ShopSaveType.ShopNow Then
            Return "Deals Center"
        ElseIf Type = ShopSaveType.ShopYourWay Then
            Return "Promotion Section 1"
        ElseIf Type = ShopSaveType.WeeklyEmail Then
            Return "Weekly Email"
        Else
            Return "Promotion Section 2"
        End If
        Return String.Empty
    End Function
    Private Sub LoadList()
        ltrHeader.Text = GetTitle()
        If Type > 0 Then
            Dim collection As ShopSaveCollection = ShopSaveRow.ListByType(DB, Type, -1) '-1: without IsActive
            TotalRecords = collection.Count
            rptShopSave.DataSource = collection
            rptShopSave.DataBind()

            If rptShopSave.Items.Count = 0 Then
                rptShopSave.DataSource = Nothing
                rptShopSave.DataBind()
            Else
            End If

            btnAddNew.Visible = True

        Else
            rptShopSave.DataSource = Nothing
            rptShopSave.DataBind()

            btnAddNew.Visible = False

        End If

    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim shopsave As New ShopSaveRow(DB)

        Try

            ltMssImage.Text = String.Empty
            ltmsgimage1.Text = String.Empty
            shopsave.IsActive = chkIsActive.Checked
            shopsave.IsTab = False 'chkIsTab.Checked
            shopsave.IsHtml = True 'chkIsHtml.Checked
            If String.IsNullOrEmpty(hidBanner.Value) = False Then
                shopsave.Banner = hidBanner.Value
            End If
            If String.IsNullOrEmpty(hidHomeBanner.Value) = False Then
                shopsave.HomeBanner = hidHomeBanner.Value
            End If
            If String.IsNullOrEmpty(hidMobileBanner.Value) = False Then
                shopsave.MobileBanner = hidMobileBanner.Value
            End If
            shopsave.Name = txtName.Text.Trim()
            shopsave.MetaKeyword = txtMetaKeyword.Text.Trim()
            shopsave.MetaDescription = txtMetaDescription.Text.Trim()
            shopsave.OutsideUSMetaDescription = txtOutsideUSMetaDescription.Text.Trim()
            shopsave.Type = Type
            shopsave.Url = txtUrl.Text.Trim()
            shopsave.PageTitle = txtPageTitle.Text
            shopsave.OutsideUSPageTitle = txtOutsideUSPageTitle.Text
            shopsave.ShortContent = txtShortContent.Text
            shopsave.Content = txtContent.Text

            If shopsave.Type <> 6 Then
                If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(shopsave.HomeBanner) Then
                    ltMssImage.Text = "Image is required."
                    'Exit Sub
                End If
                If String.IsNullOrEmpty(fuImageMobile.NewFileName) AndAlso String.IsNullOrEmpty(shopsave.HomeBanner) Then
                    ltmsgimage1.Text = "Image is required."
                    'Exit Sub
                End If
                If Not String.IsNullOrEmpty(ltmsgimage1.Text) Or Not String.IsNullOrEmpty(ltMssImage.Text) Then
                    Exit Sub
                End If
            End If
            Dim logSubject As String = ""
            If (hidId.Value.Length > 0) Then
                ShopSaveId = hidId.Value
                shopsave.ShopSaveId = ShopSaveId
                logSubject = "Update Shop Save"
                shopsave.Update()
            Else
                logSubject = "Insert Shop Save"
                ShopSaveId = shopsave.Insert()
            End If

            CacheUtils.ClearCacheWithPrefix("ShopSave_ListByType_")
            WriteLogDetail(logSubject, shopsave)

            If ShopSaveId > 0 Then
                Dim ImageNameNew As String = GetImageName(shopsave.Name)
                If fuBanner.NewFileName <> String.Empty Then
                    fuBanner.NewFileName = ImageNameNew & fuBanner.OriginalExtension
                    fuBanner.SaveNewFile()
                    shopsave.Banner = ImageNameNew & fuBanner.OriginalExtension
                ElseIf fuBanner.MarkedToDelete Then
                    fuBanner.RemoveOldFile()
                    shopsave.Banner = String.Empty
                End If
                If fuImage.NewFileName <> String.Empty Then
                    fuImage.NewFileName = ImageNameNew & fuImage.OriginalExtension
                    fuImage.SaveNewFile()
                    shopsave.HomeBanner = ImageNameNew & fuImage.OriginalExtension
                    hidHomeBanner.Value = shopsave.HomeBanner
                    Dim width As Integer
                    width = Core.GetWidth(Server.MapPath(fuImage.Folder) & fuImage.NewFileName)
                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    shopsave.HomeBanner = String.Empty
                End If
                If fuImageMobile.NewFileName <> String.Empty Then
                    fuImageMobile.NewFileName = fuImage.NewFileName
                    fuImageMobile.SaveNewFile()
                ElseIf fuImageMobile.MarkedToDelete Then
                    fuImageMobile.RemoveOldFile()
                End If
                shopsave.Update()
            End If

            If ShopSaveId > 0 Then
                LoadList()
                btnCancel_Click(sender, e)
            End If

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnList.Visible = True
        pnNew.Visible = False
        txtContent.Text = ""
        txtShortContent.Text = ""
        txtName.Text = ""
        txtMetaKeyword.Text = ""
        txtMetaDescription.Text = ""
        hidId.Value = ""
        txtPageTitle.Text = ""
        txtUrl.Text = ""
        txtShortContent.Text = ""
        txtOutsideUSMetaDescription.Text = String.Empty
        txtOutsideUSPageTitle.Text = String.Empty
        fuBanner.CurrentFileName = String.Empty
        fuBanner.NewFileName = String.Empty
        hpBanner.ImageUrl = String.Empty


        fuImage.CurrentFileName = String.Empty
        fuImage.NewFileName = String.Empty
        hpimg.ImageUrl = String.Empty
        ltrHeader.Text = GetTitle()
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        pnList.Visible = False
        pnNew.Visible = True

        ltrHeader.Text = "Add new"
    End Sub


    Protected Sub rptShopSave_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptShopSave.ItemCommand
        If e.CommandName = "Delete" Then
            Dim objDelete As ShopSaveRow = ShopSaveRow.GetRow(DB, e.CommandArgument)
            Dim f As New Controls.FileUpload
            f.RemoveFileName(Utility.ConfigData.ShopSaveBannerFolder, objDelete.Banner)
            f.RemoveFileName(Utility.ConfigData.ShopSaveBannerFolder & "home/", objDelete.HomeBanner)
            f.RemoveFileName(Utility.ConfigData.ShopSaveBannerFolder & "mobile/", objDelete.MobileBanner)
            ShopSaveRow.Delete(DB, e.CommandArgument)
            LoadList()
        ElseIf e.CommandName = "Edit" Then

            ShopSaveId = e.CommandArgument
            LoadDetail()
        ElseIf e.CommandName = "Active" Then
            ShopSaveRow.ChangeIsActive(DB, e.CommandArgument)
            LoadList()
        ElseIf e.CommandName = "Up" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, False)
            LoadList()
        ElseIf e.CommandName = "Down" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, True)
            LoadList()
        End If
    End Sub

    Protected Sub rptShopSave_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptShopSave.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As ShopSaveRow = e.Item.DataItem

            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)


            If e.Item.ItemIndex = 0 AndAlso TotalRecords > 1 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 AndAlso TotalRecords > 1 Then
                imbDown.Visible = False
            ElseIf TotalRecords < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            End If

            Dim litName As Literal = CType(e.Item.FindControl("litName"), Literal)
            Dim item As ShopSaveRow = CType(e.Item.DataItem, ShopSaveRow)
            If litName IsNot Nothing Then
                If Type = 6 Then
                    litName.Text = String.Format("<a href=""{1}/promotion/{2}/{3}"" target=""_blank"">{0}</a>", item.Name, Utility.ConfigData.GlobalRefererName, HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.Name.ToLower())), item.ShopSaveId)
                Else
                    litName.Text = item.Name
                End If
            End If


        End If
    End Sub

    Private Function GetImageName(ByVal ShopSaveName As String) As String
        Dim pattern As String = "[^A-Za-z0-9 ]"
        Dim result As String = Regex.Replace(ShopSaveName, pattern, "")
        result = result.Replace("  ", " ")
        result = result.Replace(" ", "-")
        Return result.ToLower()
    End Function
    Private Sub ResizeImage(ByVal OriginalPath As String, ByVal NewPath As String, ByVal FileName As String, ByVal inputWidth As Integer)
        Try
            Dim OriginalPathFile As String = Server.MapPath(OriginalPath & FileName)
            Dim sRate As Double = 0
            Dim height, width, sHeight As Integer
            Core.GetImageSize(OriginalPathFile, width, height)
            If height > width Then
                sRate = CDbl(height / width)
            Else
                sRate = CDbl(width / height)
            End If
            sHeight = inputWidth * sRate
            Core.ResizeImage(OriginalPathFile, Server.MapPath(NewPath) & FileName, inputWidth, sHeight)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnEditMetaTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditMetaTag.Click
        Dim url As String = "/store/main-shop-save.aspx"
        Dim objPage As ContentToolPageRow = ContentToolPageRow.GetRowByURL(url)

        Response.Redirect("/admin/content/Pages/register.aspx?PageId=" & objPage.PageId)
    End Sub
End Class
