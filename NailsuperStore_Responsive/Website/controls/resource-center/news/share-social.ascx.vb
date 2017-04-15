
Imports Components
Imports DataLayer
Imports System.Data.SqlClient


Partial Class controls_resource_center_news_share_social
    Inherits ModuleControl

    Private NewsId As Integer
    Private showMultimedia As Boolean = False
    Private WebRoot As String = ""

    Public Overrides Property Args As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("NewId") <> Nothing AndAlso IsNumeric(Request.QueryString("NewId")) Then
            NewsId = CInt(IIf(Request.QueryString("NewId").Contains(","), Request.QueryString("NewId").Split(",")(0), Request.QueryString("NewId")))
        End If
        Dim news As NewsRow = NewsRow.GetRow(DB, NewsId)
        If Not IsPostBack Then
            LoadImage(NewsId)
            LoadDocument(NewsId)
            SetSocialNetwork(news)
            If showMultimedia Then
                ltrMultimediaHeader.Text = "<div class='title'>Multimedia</div>" & ltrImage.Text & ltrDoc.Text
            End If
        End If
    End Sub

    Private Sub SetSocialNetwork(ByVal objItem As NewsRow)

        Dim shareURL As String = Utility.ConfigData.GlobalRefererName & URLParameters.NewsDetailUrl(objItem.Title, objItem.NewsId)
        Dim shareTitle As String = objItem.Title
        Dim shareDesc As String = String.Empty
        If objItem.ShortDescription <> Nothing Then
            shareDesc = Utility.Common.RemoveHTMLTag(objItem.ShortDescription)
        End If


        'SetPageMetaSocialNetwork(shareTitle, shareDesc, "", shareURL, objItem.ThumbImage, Utility.ConfigData.VideoThumbPath, 360, 202)
        uShare.shareURL = shareURL
        'uShare.shareDescription = shareDesc

    End Sub

    Private Sub LoadImage(ByVal newsId As Integer)
        Dim data As New NewsImageRow
        Dim collect As NewsImageCollection = NewsImageRow.GetByNewId(DB, newsId)
        Dim imgHTML As String = ""
        Dim result As String = ""
        For Each objImage As NewsImageRow In collect
            imgHTML = imgHTML & CreateImageItem(objImage)
        Next
        If (imgHTML <> "") Then
            result = " <div class='imagesHeader'><b class='arrow-left'></b>Images</div>"
            result = result & " <div id='divImageContent'>"
            result = result & imgHTML & "</div>"
            ltrImage.Text = result
            showMultimedia = True
        End If

    End Sub

    Private Function CreateImageItem(ByVal image As NewsImageRow) As String
        If Not System.IO.File.Exists(Server.MapPath("~" & Utility.ConfigData.PathSmallNewsImage & image.FileName)) Then
            Return String.Empty
        End If
        Dim imgPath As String = Utility.ConfigData.PathSmallNewsImage
        Dim result As String = "<div style=""margin:0px 10px 10px 0; float: left; position: relative;"">"
        result = result & "        	<a target=""_blank"" href=" & WebRoot & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.PathNewImage & image.FileName & "><img src=" & Utility.ConfigData.CDNMediaPath & imgPath & image.FileName & " /></a>"
        result = result & "               <div class='divImgTitle'><a target=""_blank"" href=" & WebRoot & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.PathNewImage & image.FileName & ">" & image.ImageName & "</a>"
        result = result & "                 </div>"
        result = result & "</div>"
        Return result
    End Function

    Private Sub LoadDocument(ByVal newsId As Integer)
        Dim docHTML As String = ""
        Dim result As String = ""
        Dim collect As NewsDocumentCollection = NewsDocumentRow.ListAllActive(DB, newsId)
        For Each objDoc As NewsDocumentRow In collect
            docHTML = docHTML & CreateDocumentItem(objDoc)
        Next
        If (docHTML <> String.Empty) Then
            docHTML = "<ul class='podcastList'>" & docHTML & "</ul>"
            result = result & "<div class='imagesHeader'><b class='arrow-left'></b>Document</div>"
            result = result & "  <div class='podcastContent' id='divDocContent' >" & docHTML & "</div>"
            ltrDoc.Text = result
            showMultimedia = True
        End If
    End Sub

    Private Function CreateDocumentItem(ByVal doc As NewsDocumentRow) As String
        Dim objDoc As DocumentRow = DocumentRow.GetRow(DB, doc.DocumentId)
        'If Not System.IO.File.Exists(Server.MapPath("/" & Utility.ConfigData.PathNewDocument & objDoc.FileName)) Then
        '    Return String.Empty
        'End If
        'Dim docPath As String = Utility.ConfigData.PathNewDocument
        Dim result As String = "<li><a target=""_blank"" href=" & WebRoot & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.PathNewDocument & objDoc.FileName & ">" & objDoc.DocumentName & "</a></li>"
        Return result
    End Function


End Class

