Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class Index
	Inherits AdminPage
    Private TotalRecords As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        fuImage.ImageDisplayFolder = Utility.ConfigData.FreesampleFolderUpload
        fuImage.Folder = fuImage.ImageDisplayFolder
        fuMobileImage.ImageDisplayFolder = Utility.ConfigData.FreesampleFolderUpload & "mobile"
        fuMobileImage.Folder = fuMobileImage.ImageDisplayFolder
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ItemId"
            BindList()
        End If
    End Sub
    
    Private Sub btnEditMetaTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditMetaTag.Click
        Dim url As String = "/store/free-sample.aspx"
        Dim objPage As ContentToolPageRow = ContentToolPageRow.GetRowByURL(url)

        Response.Redirect("/admin/content/Pages/register.aspx?PageId=" & objPage.PageId)
    End Sub
    Private Sub BindList()
        hidPopUpSKU.Value = ""
        Dim SQLFields, SQL As String
        Dim Conn As String = " AND "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Storeitem WHERE IsFreeSample = 1"


        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        Dim i As Integer = 0

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY FreeSampleArrange DESC")

        TotalRecords = res.Tables(0).DefaultView.Count
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()



        txtOrderMin.Text = DB.ExecuteScalar("select value from Sysparam where name = 'FreeSampleOrderMin'")
        txtQty.Text = DB.ExecuteScalar("select value from Sysparam where name = 'FreeSampleQty'")
        ''View Image
        Dim fullImage As String = LoadImage(fuImage.ImageDisplayFolder)
        fuImage.CurrentFileName = fullImage
        hpimg.ImageUrl = fuImage.ImageDisplayFolder & fullImage

        Dim mobileImage As String = LoadImage(fuMobileImage.ImageDisplayFolder)
        fuMobileImage.CurrentFileName = mobileImage
        hpMobileimg.ImageUrl = fuMobileImage.ImageDisplayFolder & mobileImage

    End Sub
    Public Function LoadImage(ByVal pathImg As String) As String
        Try
            Dim imgResult As String = String.Empty
            Dim di As New DirectoryInfo(Server.MapPath(pathImg))
            Dim rgFiles As FileInfo() = di.GetFiles()
            For Each fi As FileInfo In rgFiles
                imgResult = fi.Name
            Next
            Return imgResult
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function
    
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        If hidPopUpSKU.Value <> "" Then
            Dim arr As Array = Split(hidPopUpSKU.Value.Trim(), ";")
            ''If arr(0) <> "thisForm" Then
            Dim si As StoreItemRow
            For i As Integer = 0 To arr.Length - 1
                If (arr(i).ToString() <> "") Then
                    si = StoreItemRow.GetRowSku(DB, arr(i).ToString())
                    Dim siold As StoreItemRow = CloneObject.Clone(si)
                    si.IsFreeSample = True
                    si.IPNUpdateFreeSamples()

                    Dim logDetail As New AdminLogDetailRow
                    logDetail.ObjectId = si.ItemId
                    logDetail.ObjectType = Utility.Common.ObjectType.FreeSample.ToString()
                    Dim changeLog As String = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FreeSample, siold, si)
                    logDetail.Message = changeLog
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString
                    AdminLogHelper.WriteLuceneLogDetail(logDetail)
                End If
            Next

            ''End If
        End If
        BindList()
    End Sub
   
    Protected Sub btnSaveImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveImage.Click

        If fuImage.NewFileName <> String.Empty Then
            fuImage.SaveNewFile()
        End If
        If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
        Response.Redirect("default.aspx")
    End Sub
    Protected Sub btnSaveMobileImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveMobileImage.Click

        If fuMobileImage.NewFileName <> String.Empty Then
            fuMobileImage.SaveNewFile()
        End If
        If fuMobileImage.NewFileName <> String.Empty Or fuMobileImage.MarkedToDelete Then fuMobileImage.RemoveOldFile()
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim rowView As DataRowView = e.Row.DataItem
            'Arrange
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            Dim ItemId As Integer = Convert.ToInt32(rowView("ItemId"))
            imbUp.CommandArgument = ItemId.ToString
            imbDown.CommandArgument = ItemId.ToString()
            If e.Row.DataItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Row.DataItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
            hidPopUpSKU.Value = hidPopUpSKU.Value & rowView("sku") & ";"
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            StoreItemRow.ChangeArrangeFreeSample(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "Down" Then
            StoreItemRow.ChangeArrangeFreeSample(DB, e.CommandArgument, True)
        End If
        BindList()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim OrderMin As Integer = DB.ExecuteScalar("select value from Sysparam where name = 'FreeSampleOrderMin'")
        If txtOrderMin.Text <> "" Then
            If CInt(txtOrderMin.Text) <> CInt(OrderMin) Then
                Dim changeLog As String = String.Empty
                DB.ExecuteSQL("Update Sysparam set value = " & CInt(txtOrderMin.Text) & " where Name = 'FreeSampleOrderMin' ")

                Dim dbSysParam As SysparamRow = SysparamRow.GetRow(DB, "FreeSampleOrderMin")
                changeLog = dbSysParam.Comments & "|" & OrderMin.ToString().Trim() & "|" & txtOrderMin.Text & "[br]"
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = dbSysParam.ParamId
                logDetail.ObjectType = Utility.Common.ObjectType.Sysparam.ToString()
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
                Utility.CacheUtils.RemoveCache("Sysparam_ListAll")
            End If
        End If
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        Dim QtyFreeSample As Integer = DB.ExecuteScalar("select value from Sysparam where name = 'FreeSampleQty'")
        If txtQty.Text <> "" Then
            If CInt(txtQty.Text) <> CInt(QtyFreeSample) Then
                DB.ExecuteSQL("Update Sysparam set value = " & CInt(txtQty.Text) & " where Name = 'FreeSampleQty' ")
                Utility.CacheUtils.RemoveCache("Sysparam_ListAll")
                Dim changeLog As String = String.Empty
                Dim dbSysParam As SysparamRow = SysparamRow.GetRow(DB, "FreeSampleQty")
                changeLog = dbSysParam.Comments & "|" & QtyFreeSample.ToString().Trim() & "|" & txtQty.Text & "[br]"
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = dbSysParam.ParamId
                logDetail.ObjectType = Utility.Common.ObjectType.Sysparam.ToString()
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)

            End If
        End If
        Response.Redirect("default.aspx")
    End Sub
End Class

