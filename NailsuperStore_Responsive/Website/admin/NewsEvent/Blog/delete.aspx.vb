Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class admin_NewsEvent_Blog_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim NewsId As Integer = Convert.ToInt32(Request("NewsId"))
        Try
            ''Delete Iamges
            Dim news As NewsRow = NewsRow.GetRow(DB, NewsId)
            Dim ImageName As String = news.ThumbImage
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = NewsId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(news, Utility.Common.ObjectType.Blog)
            If (NewsRow.Delete(DB, NewsId)) Then
                'Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathNewImage)
                Dim ThumbImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathThumbBlogImage)
                'Utility.File.DeleteFile(ImagePath & ImageName)
                Utility.File.DeleteFile(ThumbImagePath & ImageName)

                logDetail.ObjectType = Utility.Common.ObjectType.Blog.ToString()
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class
