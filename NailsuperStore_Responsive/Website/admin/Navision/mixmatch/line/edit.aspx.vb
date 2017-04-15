Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_mixmatch_line_Edit
    Inherits AdminPage
    Public Property Type() As Integer
        Get
            If ViewState("Type") Is Nothing Then
                Return Utility.Common.MixmatchType.Normal
            End If
            Return CInt(ViewState("Type"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Type") = value
        End Set
    End Property
    Public Property MixMatchId() As Integer
        Get
            If ViewState("MixMatchId") Is Nothing Then
                Return 0
            End If
            Return CInt(ViewState("MixMatchId"))
        End Get
        Set(ByVal value As Integer)
            ViewState("MixMatchId") = value
        End Set
    End Property
    Public Property MixMatchLineId() As Integer
        Get
            If ViewState("MixMatchLineId") Is Nothing Then
                Return 0
            End If
            Return CInt(ViewState("MixMatchLineId"))
        End Get
        Set(ByVal value As Integer)
            ViewState("MixMatchLineId") = value
        End Set
    End Property

    Protected ItemId As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        MixMatchId = Convert.ToInt32(Request("F_MixMatchId"))
        MixMatchLineId = Convert.ToInt32(Request("Id"))
        If hidPopUpSKU.Value <> "" Then
            txtSku.Text = hidPopUpSKU.Value
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, hidPopUpSKU.Value.ToString())
            ItemId = si.ItemId
        ElseIf hidItemid.Value <> ""
            ItemId = hidItemId.Value
        End If

        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpMixMatchId.DataSource = MixMatchRow.GetAllMixMatches(DB)
        drpMixMatchId.DataValueField = "Id"
        drpMixMatchId.DataTextField = "MixMatchNo"
        drpMixMatchId.DataBind()
        drpMixMatchId.Items.Insert(0, New ListItem("", ""))
        Dim objMixmatch As MixMatchRow = MixMatchRow.GetRow(DB, MixMatchId)
        Type = objMixmatch.Type
        drpMixMatchId.SelectedValue = objMixmatch.Id

        If MixMatchLineId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbMixMatchLine As MixMatchLineRow = MixMatchLineRow.GetRow(DB, MixMatchLineId)
        txtLineNo.Text = dbMixMatchLine.LineNo
        ddlDiscountType.SelectedValue = dbMixMatchLine.DiscountType
        txtValue.Text = dbMixMatchLine.Value

        drpMixMatchId.SelectedValue = dbMixMatchLine.MixMatchId
        chkIsActive.Checked = dbMixMatchLine.IsActive
        If dbMixMatchLine.ItemId > 0 Then
            hidItemId.Value = dbMixMatchLine.ItemId
            'Dim si As StoreItemRow = StoreItemRow.GetRow(DB, dbMixMatchLine.ItemId)
            'LookupField.Value = si.SKU & ", (" & si.ItemName & ")"
            ItemId = dbMixMatchLine.ItemId
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, Itemid)
            txtSku.Text = si.SKU
        End If
        txtQtydefault.Text = dbMixMatchLine.DefaultSelectQty
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbMixMatchLineOld As MixMatchLineRow = Nothing
            Dim dbMixMatchLine As MixMatchLineRow
            If MixMatchLineId <> 0 Then
                dbMixMatchLine = MixMatchLineRow.GetRow(DB, MixMatchLineId)
                dbMixMatchLineOld = CloneObject.Clone(dbMixMatchLine)
            Else
                dbMixMatchLine = New MixMatchLineRow(DB)
            End If
            dbMixMatchLine.LineNo = txtLineNo.Text
            dbMixMatchLine.DiscountType = ddlDiscountType.SelectedValue
            dbMixMatchLine.Value = txtValue.Text
            dbMixMatchLine.MixMatchId = drpMixMatchId.SelectedValue
            dbMixMatchLine.ItemId = Itemid
            dbMixMatchLine.IsActive = chkIsActive.Checked

            If txtValue.Text > 0 Then
                If String.IsNullOrEmpty(txtQtydefault.Text) Then
                    txtQtydefault.Text = 0
                End If
                lbmsg.Text = CheckCountLine(txtQtydefault.Text)
                If String.IsNullOrEmpty(lbmsg.Text) Then
                    dbMixMatchLine.DefaultSelectQty = txtQtydefault.Text
                    If txtQtydefault.Text = 0 Then
                        dbMixMatchLine.IsDefaultSelect = False
                    Else
                        dbMixMatchLine.IsDefaultSelect = True
                    End If
                Else
                    Exit Sub
                End If
            End If
            Dim result As Integer = 0
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.MixMatchLine.ToString()
            If CheckLineDuplicate(dbMixMatchLine) Then
                AddError("Mixmatch #" & drpMixMatchId.SelectedItem.Text & " alredy exists item value=0. Please check again")
                Exit Sub
            End If
            If MixMatchLineId <> 0 Then

                result = MixMatchLineRow.UpdateLine(DB, dbMixMatchLine)
                logDetail.ObjectId = MixMatchLineId
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.MixMatchLine, dbMixMatchLineOld, dbMixMatchLine)
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                '' dbMixMatchLine.Update()
            Else
                If Type = Utility.Common.MixmatchType.ProductCoupon Then
                    ''check multi item vallue=0
                    Dim itemValue0 As Integer = MixMatchRow.CountItemBuy(DB, MixMatchLineId)
                    If itemValue0 > 1 Then
                        AddError("Mixmatch #" & drpMixMatchId.SelectedItem.Text & " is not valid because it's more than 1 item  with Value=0")
                        Exit Sub
                    End If
                End If

                '' Id = dbMixMatchLine.Insert
                result = MixMatchLineRow.InsertLine(DB, dbMixMatchLine)
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbMixMatchLine, Utility.Common.ObjectType.MixMatchLine)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If

            If result = 1 Then
                Utility.CacheUtils.RemoveCacheItemWithPrefix(StoreCartItemRow.cachePrefixKey & "ItemCount_")
                Utility.CacheUtils.RemoveCacheItemWithPrefix("popupCart_")
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
                Response.Redirect("default.aspx?type=" & Type & "&" & GetPageParams(FilterFieldType.All))
            ElseIf result = -1 Then
                Dim dbItem As StoreItemRow = StoreItemRow.GetRow(DB, dbMixMatchLine.ItemId)
                AddError("Item #" & dbItem.SKU & " is active at the same time in other mixmatch . Please check again")
            Else
                AddError("A error has occured. Please try again.")
            End If


        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Private Function CheckCountLine(ByVal inputQty As Integer) As String
        Dim msg As String = String.Empty
        Dim countLine As Integer = DB.ExecuteScalar("select COUNT(mixmatchid) from MixMatchLine where Id <> " & MixMatchLineId & " and MixMatchId = " & MixMatchId & " and Value > 0")
        countLine += 1 'this line
        If inputQty > countLine Then
            msg = "Quantity input must less than or equal limit Quantity items free (" & countLine & ")"
        End If
        Return msg
    End Function
    Private Function CheckLineDuplicate(ByVal dbMixMatchLine As MixMatchLineRow) As Boolean
        Dim mixmatchRow As MixMatchRow = mixmatchRow.GetRow(DB, dbMixMatchLine.MixMatchId)
        If mixmatchRow.Type = Utility.Common.MixmatchType.ProductCoupon Then
            If (dbMixMatchLine.Value = 0) Then
                Dim countExists As Integer = 0
                If dbMixMatchLine.Id < 1 Then
                    countExists = DB.ExecuteScalar("Select count(Id) from MixMatchLine where Value=0 and MixMatchId=" & dbMixMatchLine.MixMatchId)
                Else
                    countExists = DB.ExecuteScalar("Select count(Id) from MixMatchLine where Value=0 and MixMatchId=" & dbMixMatchLine.MixMatchId & " and Id<>" & dbMixMatchLine.Id)
                End If
                If countExists > 0 Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?type=" & Type & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & MixMatchLineId & "&type=" & Type & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

