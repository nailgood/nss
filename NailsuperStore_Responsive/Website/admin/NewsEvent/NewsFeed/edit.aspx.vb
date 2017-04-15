Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_NewsEvent_NewsFeed_edit
    Inherits AdminPage
    Protected NewsFeedId As Integer = 0
    Private nf As NewsFeedRow
    Protected ImagePath As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        NewsFeedId = Convert.ToInt32(Request("NewsFeedId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub
    Private Sub LoadFromDB()
        If NewsFeedId <= 0 Then
            Return
        End If
        nf = NewsFeedRow.GetRow(DB, NewsFeedId)
        txtTitle.Text = nf.Title
        txtShortContent.Text = nf.ShortContent
        txtUrl.Text = nf.Url
        chkIsActive.Checked = nf.IsActive
        fuImage.CurrentFileName = nf.Image
        fuImage.EnableDelete = False
        dtSubmitdate.Value = nf.CreatedDate.Date
        'ImagePath = Utility.ConfigData.NewsFeedImg & nf.Image
        ' F_Source.SelectedValue = nf.Source
  
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim isError As String = String.Empty
        Try
            DB.BeginTransaction()

            If NewsFeedId > 0 Then
                nf = NewsFeedRow.GetRow(DB, NewsFeedId)
            Else
                nf = New NewsFeedRow()
            End If
            nf.Title = txtTitle.Text
            nf.ShortContent = txtShortContent.Text
            nf.Url = txtUrl.Text
            If fuImage.NewFileName <> String.Empty Then
                nf.Image = fuImage.NewFileName
                fuImage.SaveNewFile()
                ResizeImage(fuImage.NewFileName)
            End If
            nf.CreatedDate = IIf(dtSubmitdate.Value = "#12:00:00 AM#", Date.Now, dtSubmitdate.Value)
            nf.IsActive = chkIsActive.Checked
            If NewsFeedId > 0 Then
                nf.NewsFeedId = NewsFeedId
                nf.Update()
            Else
                nf.Insert()
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "[Insert News Feed]", "<br>Error: " & ex.ToString())
            DB.RollbackTransaction()
            isError = "Insert Error: " & ex.ToString
        Finally
            DB.CommitTransaction()
            If isError <> String.Empty Then
                AddError(isError)
            Else
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If
        End Try
    End Sub
    Private Sub ResizeImage(ByVal FileName As String)
        Dim ImagePath As String = Utility.ConfigData.NewsFeedImg
        Dim sRate As Double = 0
        Dim sHeight As Integer = 0
        Dim height, width As Integer
        height = Core.GetHeight(Server.MapPath(ImagePath & "fullupload\") & FileName)
        width = Core.GetWidth(Server.MapPath(ImagePath & "fullupload\") & FileName)
        If height > width Then
            sRate = CDbl(height / width)
        Else
            sRate = CDbl(width / height)
        End If
        sHeight = 70 * sRate
        Core.ResizeImage(Server.MapPath(ImagePath & "fullupload\") & FileName, Server.MapPath(ImagePath) & FileName, 70, sHeight)
    End Sub
End Class
