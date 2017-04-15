Imports Components
Imports Controls
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Partial Class admin_shopdesign_edit
    Inherits AdminPage
    Public IdShop As Integer
    Dim strIsChecked As String = ""
    Dim newImageName As String = ""
    Dim ImagePath As String = ""
    Dim ThumbImagePath As String = ""
    Dim smallImg As String = ""
    Dim largeImg As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("ShopDesignId") <> Nothing AndAlso IsNumeric(Request.QueryString("ShopDesignId")) Then
            IdShop = CInt(Request.QueryString("ShopDesignId"))
        End If
        If Not IsPostBack Then
            LoadCategory()
            LoadData()
        End If
    End Sub

    Private Sub LoadCategory()
        Dim dt As DataTable = CategoryRow.GetCategoryById(DB, IdShop, Utility.Common.CategoryType.ShopDesign)
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("IsChecked") = 1 Then
                If strIsChecked = "" Then
                    strIsChecked = dt.Rows(i)("CategoryId")
                Else
                    strIsChecked &= "," & dt.Rows(i)("CategoryId")
                End If
            End If
        Next
        Dim ParentNode As New TreeNode
        'Dim ExpandedList As String = ''
        Dim aExpanded() As String = Split(strIsChecked, ",")
        Dim aChecked() As String = Split(strIsChecked, ",")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim name As String = dt.Rows(i)("CategoryName")
            Dim ParentId As Integer = IIf(IsDBNull(dt.Rows(i)("ParentId")), Nothing, dt.Rows(i)("ParentId"))
            Dim CategoryId As Integer = CInt(dt.Rows(i)("CategoryId"))
            Dim tNode As TreeNode = New TreeNode
            tNode.Value = CategoryId
            tNode.Text = dt.Rows(i)("CategoryName")
            tNode.ImageUrl = "/includes/theme-admin/images/folderplus.gif"
            tNode.SelectAction = TreeNodeSelectAction.None

            If Array.IndexOf(aExpanded, CategoryId.ToString) > -1 Then
                tNode.Expanded = True
            Else
                tNode.Expanded = False
            End If
            If Array.IndexOf(aChecked, CategoryId.ToString()) > -1 Then
                tNode.Checked = True
            End If

            If ParentId = Nothing Then
                tvCategory.Nodes.Add(tNode)
                tNode.Expanded = True
                ParentNode = tNode
            Else
                While ParentNode.Value <> ParentId
                    ' gan lai parent cho node htai
                    For k As Integer = 0 To tvCategory.Nodes.Count() - 1
                        If tvCategory.Nodes(k).Value = ParentId Then
                            ParentNode = tvCategory.Nodes(k)
                        End If
                    Next
                End While
                ParentNode.ChildNodes.Add(tNode)
            End If
        Next
        'tvCategory.DataSource = ParentNode
        'tvCategory.DataBind()

        'cblCategory.DataSource = dt
        'cblCategory.DataTextField = "CategoryName"
        'cblCategory.DataValueField = "CategoryId"
        'cblCategory.DataBind()
        'cblCategory.SelectedValues = strIsChecked
    End Sub

    Private Sub LoadData()
        
        If IdShop < 1 Then
            Exit Sub
        End If
        Dim data As ShopDesignRow = ShopDesignRow.GetRow(DB, IdShop)
        txtTitle.Text = data.Title
        txtPageTitle.Text = data.PageTitle
        txtMetaDescription.Text = data.MetaDescription
        txtMetaKeyword.Text = data.MetaKeyword
        chkIsActive.Checked = data.IsActive
        txtShortDesc.Text = data.ShortDescription
        txtInstruction.Text = data.Instruction
        txtDesc.Text = data.Description
        hpimg.ImageUrl = Utility.ConfigData.ShopDesignThumbPath & data.Image
        fulImage.CurrentFileName = data.Image
        If fulImage.CurrentFileName Is Nothing Then
            divImg.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then
            Exit Sub
        End If
        For i As Integer = 0 To tvCategory.Nodes.Count - 1
            strIsChecked = strIsChecked & GetCheckedNodes(tvCategory.Nodes.Item(i))
        Next

        If strIsChecked = "" Then
            lblCategory.Text = "Please select category"
            Exit Sub
        End If
        Dim item As New ShopDesignRow
        Dim itemBefore As New ShopDesignRow
        Dim logdetail As New AdminLogDetailRow
        Dim oldimg As String = ""
        Try
            If IdShop > 0 Then
                item = ShopDesignRow.GetRow(DB, IdShop)
                itemBefore = CloneObject.Clone(item)
                oldimg = item.Image
            End If

            item.Title = txtTitle.Text
            item.PageTitle = txtPageTitle.Text
            item.MetaDescription = txtMetaDescription.Text
            item.MetaKeyword = txtMetaKeyword.Text
            item.IsActive = chkIsActive.Checked
            item.ShortDescription = txtShortDesc.Text
            item.Instruction = txtInstruction.Text
            item.Description = txtDesc.Text
            item.ModifiedDate = Date.Now

            Dim filename As String = fulImage.NewFileName
            If filename <> Nothing Then
                Dim checkimg As String = filename.Substring(filename.LastIndexOf(".") + 1)
                Dim allowImg() As String = Utility.ConfigData.AllowImageUpload().Split(",")
                Dim check As Integer = 0
                For i As Integer = 0 To allowImg.Length - 1
                    If (allowImg(i).Equals(checkimg.ToLower())) Then
                        check += 1
                    End If
                Next
                If check = 0 Then
                    AddError("Image is not valid.")
                    Exit Sub
                End If

                fulImage.Folder = "~" & Utility.ConfigData.ShopDesignPath()
                newImageName = Guid.NewGuid().ToString()
                filename = newImageName & "." & checkimg
                ImagePath = Server.MapPath("~" & Utility.ConfigData.ShopDesignPath())
                ThumbImagePath = Server.MapPath("~" & Utility.ConfigData.ShopDesignThumbPath())
                smallImg = Server.MapPath("~" & Utility.ConfigData.ShopDesignSmallPath)
                largeImg = Server.MapPath("~" & Utility.ConfigData.ShopDesignLargePath)
                fulImage.SaveNewFile()
                If oldimg <> String.Empty Then
                    ''delete old img
                    Utility.File.DeleteFile(ImagePath & oldimg)
                    Utility.File.DeleteFile(ThumbImagePath & oldimg)
                End If

                Core.CropByAnchor(ImagePath & fulImage.NewFileName, ThumbImagePath & filename, 278, 156, Utility.Common.ImageAnchorPosition.Center)
                Core.CropByAnchor(ImagePath & fulImage.NewFileName, smallImg & filename, 80, 80, Utility.Common.ImageAnchorPosition.Center)
                Core.CropByAnchor(ImagePath & fulImage.NewFileName, largeImg & filename, 728, 410, Utility.Common.ImageAnchorPosition.Center)
                item.Image = filename
            End If

            If IdShop > 0 Then
                item.Update()
                item.RemoveAllCategory()
                item.InsertShopDesignCategory(strIsChecked, IdShop)
                logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ShopDesign, itemBefore, item)
            Else
                item.CreatedDate = Date.Now
                IdShop = item.Insert()
                item.InsertShopDesignCategory(strIsChecked, IdShop)
                logdetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(item, Utility.Common.ObjectType.ShopDesign)
            End If
            logdetail.ObjectId = item.ShopDesignId
            logdetail.ObjectType = Utility.Common.ObjectType.ShopDesign.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logdetail)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Function GetCheckedNodes(ByVal input As TreeNode) As String
        Dim result As String = ""
        If (Not IsExists(result, input.Value)) AndAlso input.Checked Then
            result = result & input.Value & ","
        End If
        For Each node As TreeNode In input.ChildNodes
            If node.Checked Then
                result = result & node.Value & ","
            End If
            Dim Checked As String = GetCheckedNodes(node)
            If Not Checked = String.Empty AndAlso Not IsExists(result, Checked) Then
                result = result & Checked & ","
            End If
        Next
        Return result
    End Function

    Private Shared Function IsExists(ByVal lstID As String, ByVal id As String) As Boolean
        id = IIf(id.Contains(","), id.Substring(0, id.Length - 1), id)
        Dim arr As String() = lstID.Split(",")
        For i As Integer = 0 To arr.Length - 1
            If id = arr(i) Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams())
    End Sub
End Class
