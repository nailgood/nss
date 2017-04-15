Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class members_nail_art_trends_gallery
    Inherits SitePage
    Protected Name, SalonName As String
    Protected Shared PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Private Shared PageIndex As Integer = 1
    Protected Shared TotalRecords As Integer = 0
    Private Shared mrc As MemberSubmissionCollection
    Protected Shared hidGalleryIndex As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        mrc = MemberSubmissionRow.GetGallery(PageSize, PageIndex)
        TotalRecords = MemberSubmissionRow.TotalRecord
        If TotalRecords > 0 Then
            rptGallery.DataSource = mrc
            rptGallery.DataBind()
        End If
        hidGalleryIndex = PageIndex & "," & mrc.Count
    End Sub

    Protected Sub rptGallery_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptGallery.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim mr As MemberSubmissionRow = e.Item.DataItem
            Dim ucGallery As controls_resource_center_gallery = CType(e.Item.FindControl("gallery"), controls_resource_center_gallery)
            'ucGallery.Fill(mr)
            ucGallery.MemberSubmission = mr
        End If
    End Sub

    <WebMethod()> _
    Public Shared Function GetDataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        Dim xmlData As String = ""
        mrc = New MemberSubmissionCollection
        mrc = MemberSubmissionRow.GetGallery(pageSize, pageIndex)
        Dim htmlGallery As String = String.Empty
        If mrc.Count > 0 Then
            For i As Integer = 0 To mrc.Count - 1
                HttpContext.Current.Session("galleryRender") = mrc(i)
                htmlGallery &= Utility.Common.RenderUserControl("~/controls/resource-center/gallery.ascx")
                HttpContext.Current.Session("galleryRender") = Nothing
            Next
        End If
        htmlGallery &= "<input type='hidden' id='nexthidVideoIndex' clientidmode='Static' value='" & ((pageIndex - 1) * pageSize) + 1 & "' />"
        Return htmlGallery
    End Function
End Class
