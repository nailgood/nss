Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Public Class admin_store_freegift_Edit
    Inherits AdminPage

    Protected FreeGiftId As Integer
    Protected si As StoreItemRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        FreeGiftId = Convert.ToInt32(Request("FreeGiftId"))
        If hidPopUpSKU.Value <> "" Then
            txtSku.Text = hidPopUpSKU.Value
        End If

        If Not IsPostBack Then

            LoadItemFreeGift()
            LoadLevel()
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If FreeGiftId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbFreeGift As FreeGiftRow = FreeGiftRow.GetRow(DB, FreeGiftId)
        drlLevel.SelectedValue = dbFreeGift.LevelId
        fuImage.CurrentFileName = dbFreeGift.Image
        drpItemId.SelectedValue = dbFreeGift.ItemId
        si = StoreItemRow.GetRow(DB, dbFreeGift.ItemId)
        txtSku.Text = si.SKU
        chkAddCart.Checked = IIf(si.IsFreeGift = 1, 1, 0)
        chkIsActive.Checked = dbFreeGift.IsActive
        fuImage1.CurrentFileName = dbFreeGift.Banner
        hpimg.ImageUrl = ConfigurationManager.AppSettings("PathFreeGift") & dbFreeGift.Banner

        If fuImage1.CurrentFileName <> Nothing Then
            divImg.Visible = True
        End If
    End Sub
    Private Sub LoadItemFreeGift()
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader("Select itemid from freegift where isactive = 1")
            While dr.Read
                If hidItemId.Value <> Nothing Then
                    hidItemId.Value &= "," & dr("itemid")
                Else
                    hidItemId.Value &= dr("itemid")
                End If

            End While

        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
    End Sub
    Private Sub LoadLevel()
        drlLevel.Items.Clear()
        ''Dim item As New ListItem("", level.Id)
        Dim lstLevel As FreeGiftLevelCollection = FreeGiftLevelRow.ListAll()
        For Each level As FreeGiftLevelRow In lstLevel
            If level.IsActive Then
                ''Dim name As String=level.Name & "( from $" &  
                Dim item As New ListItem(level.Name, level.Id)
                drlLevel.Items.Add(item)
            End If
        Next
        Dim emptyitem As New ListItem("- - -", "")
        drlLevel.Items.Insert(0, emptyitem)
    End Sub

    Public Function CheckSku(ByVal Sku As String, ByVal LevelId As Integer) As String
        ' Dim dtfg As DataTable = DB.GetDataTable("Select freegiftid, si.SKU, fg.isactive from freegift fg inner join storeitem si on fg.ItemId = si.ItemId and fg.isactive = 1 and si.sku = '" & Sku & "' AND LevelId = " & LevelId.ToString())
        Dim dtfg As DataTable = DB.GetDataTable("Select top 10 freegiftid, fgl.Name from freegift fg inner join storeitem si on fg.ItemId = si.ItemId inner join FreeGiftLevel fgl on fg.LevelId = fgl.Id Where si.sku = '" & Sku & "' AND LevelId = " & LevelId.ToString())
        If dtfg.Rows.Count > 0 Then
            ' For i As Integer = 0 To dtfg.Rows.Count - 1
            'If HttpContext.Current.Request("FreeGiftId") = dtfg.Rows(i)("freegiftid") Then
            Return "This item is in " & LCase(dtfg.Rows(0)("Name").ToString).Replace("orders over ", "") & " Free Gift. Please choose another item."
            ' Else
            'Return String.Empty
            'End If
            'Next
        Else
            Return String.Empty
        End If
        Return String.Empty
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim oldIsFreeGift As Integer = 0
            DB.BeginTransaction()
            Dim isUpDate As Boolean = False
            Dim dbFreeGift As FreeGiftRow
            Dim dbFreeGiftOld As FreeGiftRow = Nothing
            If FreeGiftId <> 0 Then
                dbFreeGift = FreeGiftRow.GetRow(DB, FreeGiftId)
                dbFreeGiftOld = CloneObject.Clone(dbFreeGift)
            Else
                dbFreeGift = New FreeGiftRow(DB)
            End If
            Dim msgError As String = String.Empty
            If txtSku.Text <> "" Then
                If Request("FreeGiftId") = Nothing Then
                    msgError = CheckSku(txtSku.Text, drlLevel.SelectedValue)
                    ' If CheckSku(txtSku.Text, drlLevel.SelectedValue) = False And chkIsActive.Checked = True Then
                    If msgError <> String.Empty Then
                        lbError.Text = msgError
                        Exit Sub
                    End If
                End If
                si = StoreItemRow.GetRowSku(DB, txtSku.Text)
            Else
                si = StoreItemRow.GetRow(DB, dbFreeGift.ItemId)
            End If
            If Not si Is Nothing Then
                oldIsFreeGift = si.IsFreeGift
            End If
            dbFreeGift.LevelId = drlLevel.SelectedValue
            If si.ItemId <> dbFreeGift.ItemId Then
                msgError = CheckSku(txtSku.Text, drlLevel.SelectedValue)
                If msgError <> String.Empty Then
                    lbError.Text = msgError
                    Exit Sub
                End If
            End If
            dbFreeGift.ItemId = si.ItemId 'drpItemId.SelectedValue
            dbFreeGift.IsActive = chkIsActive.Checked
            If chkIsActive.Checked Then
                si.IsFreeGift = IIf(chkAddCart.Checked, 1, 2)
            Else
                si.IsFreeGift = 0
                'DB.ExecuteSQL("delete StoreCartItem where CartItemId  in (select  c.CartItemId from StoreOrder o inner join StoreCartItem c on o.OrderId = c.OrderId where o.OrderNo is null and o.SellToCustomerId = 0 and ItemId = " & dbFreeGift.ItemId & " and IsFreeItem = 1 and freeitemids is null )")
            End If
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbFreeGift.Image = fuImage.NewFileName
                Core.ResizeImage(Server.MapPath(fuImage.Folder) & fuImage.NewFileName, Server.MapPath(fuImage.Folder) & fuImage.NewFileName, 405, 78)
            ElseIf fuImage.MarkedToDelete Then
                dbFreeGift.Image = Nothing
            End If

            If FreeGiftId <> 0 Then
                si.CheckFreeGiftItem(Nothing, dbFreeGift, Utility.Common.AdminLogAction.Update.ToString())
                dbFreeGift.Update()
                ''si.Update()
                DB.ExecuteScalar("Update StoreItem set IsFreeGift=" & si.IsFreeGift & " where ItemId=" & si.ItemId)
                StoreItemRowBase.ClearItemCache(si.ItemId)
                isUpDate = True
            Else
                FreeGiftId = dbFreeGift.Insert
                DB.ExecuteScalar("Update StoreItem set IsFreeGift=" & si.IsFreeGift & " where ItemId=" & si.ItemId)
                StoreItemRowBase.ClearItemCache(si.ItemId)
                isUpDate = False
            End If
            Dim arr As String()
            If FreeGiftId <> 0 Then
                If fuImage1.NewFileName <> String.Empty Then
                    arr = fuImage1.NewFileName.Split(".")
                    fuImage1.NewFileName = FreeGiftId.ToString & "." & arr(1)
                    fuImage1.SaveNewFile()
                    dbFreeGift.Banner = fuImage1.NewFileName
                    dbFreeGift.Update()
                ElseIf fuImage1.MarkedToDelete Then
                    fuImage1.RemoveOldFile()
                    dbFreeGift.Banner = Nothing
                    dbFreeGift.Update()
                    'ElseIf dbFreeGift.Banner = "" Then
                    '    ltMssImage.Text = "Image is required."
                    '    Exit Sub
                End If
                Core.ResizeImage(Server.MapPath(fuImage1.Folder) & fuImage1.NewFileName, Server.MapPath(fuImage1.Folder) & fuImage1.NewFileName, 475, 205)
            End If
            DB.CommitTransaction()

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.FreeGift.ToString()
            logDetail.ObjectId = FreeGiftId
            If isUpDate Then
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FreeGift, dbFreeGiftOld, dbFreeGift)
                If oldIsFreeGift <> si.IsFreeGift Then
                    logDetail.Message = logDetail.Message & "IsAddCart|" & IIf(oldIsFreeGift = 1, "True", "False") & "|" & IIf(si.IsFreeGift = 1, "True", "False") & "[br]"
                End If
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbFreeGift, Utility.Common.ObjectType.FreeGift)
                logDetail.Message = logDetail.Message & "IsAddCart|" & IIf(si.IsFreeGift = 1, "True", "False") & "[br]"
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            'If fuImage.NewFileName <> String.Empty OrElse fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?FreeGiftId=" & FreeGiftId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

