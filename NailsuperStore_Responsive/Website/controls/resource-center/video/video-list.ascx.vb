Imports Components
Imports DataLayer
Imports System.IO
Imports System.Collections.Generic

Partial Class controls_resource_center_video_video_list
    Inherits BaseControl

    Public index As Integer = 0
    Private m_VideoList As VideoCollection = Nothing
    Public Property VideoList() As VideoCollection
        Set(ByVal value As VideoCollection)
            m_VideoList = value
        End Set
        Get
            Return m_VideoList
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
        If VideoList Is Nothing AndAlso Not Session("videoListRender") Is Nothing Then
            VideoList = Session("videoListRender")
        End If
        If Not VideoList Is Nothing AndAlso VideoList.Count > 0 Then
            index = countPageSize
            Dim beginIndex As Integer = index + 1
            Dim endIndex As Integer = beginIndex + VideoList.Count - 1
            If Not Session("VideoIndexRender") Is Nothing AndAlso Not String.IsNullOrEmpty(Session("VideoIndexRender")) Then
                hidVideoIndex.Value = Session("VideoIndexRender") '' dugn cho load full width video trg search
            Else
                hidVideoIndex.Value = beginIndex.ToString + "," + endIndex.ToString
            End If

            rptlstVideo.DataSource = VideoList
            rptlstVideo.DataBind()
        End If
        
    End Sub

    Protected Sub rptlstVideo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptlstVideo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As VideoRow = e.Item.DataItem
            ' hidVideoId.Value &= item.VideoId & ","
            Dim ucVideo As controls_resource_center_video_video = CType(e.Item.FindControl("ucVideo"), controls_resource_center_video_video)
            index = index + 1
            ucVideo.Fill(item, index)
        End If
    End Sub


End Class
