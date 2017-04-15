Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_resource_center_media_media_list
    Inherits BaseControl
    Public index As Integer = 0
    Private m_MediaList As VideoCollection = Nothing
    Public Property MediaList() As VideoCollection
        Set(ByVal value As VideoCollection)
            m_MediaList = value
        End Set
        Get
            Return m_MediaList
        End Get
    End Property
    Private m_countPageSize As Integer = Nothing
    Public Property countPageSize() As Integer
        Set(ByVal value As Integer)
            m_countPageSize = value
        End Set
        Get
            Return m_countPageSize
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindData()
    End Sub
    Public Sub BindData()
        If countPageSize < 1 AndAlso Not Session("videoCountPageSizeRender") Is Nothing Then
            countPageSize = Session("videoCountPageSizeRender")
        End If
        If MediaList Is Nothing AndAlso Not Session("MediaListRender") Is Nothing Then
            MediaList = Session("MediaListRender")
        End If
        If Not MediaList Is Nothing AndAlso MediaList.Count > 0 Then
            index = countPageSize
            Dim beginIndex As Integer = index + 1
            Dim endIndex As Integer = beginIndex + MediaList.Count - 1
            hidVideoIndex.Value = beginIndex.ToString() + "," + endIndex.ToString()
            dvMedia.DataSource = MediaList
            dvMedia.DataBind()
        End If
    End Sub

    Protected Sub dvMedia_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles dvMedia.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim media As VideoRow = e.Item.DataItem
            Dim ucMedia As controls_resource_center_media_media = CType(e.Item.FindControl("ucMedia"), controls_resource_center_media_media)
            index = index + 1
            ucMedia.Fill(media, index)
        End If
    End Sub
End Class
