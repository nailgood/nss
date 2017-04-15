Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class admin_LandingPage_delete
    Inherits AdminPage

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Id As Integer = Convert.ToInt32(Request("Id"))

        Try

            Dim dbLandingPage As LandingPageRow = LandingPageRow.GetRow(DB, Id)
            If Not dbLandingPage Is Nothing Then
                Dim logDetail As New AdminLogDetailRow
                dbLandingPage.GoogleABCode = String.Empty
                logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(dbLandingPage, Utility.Common.ObjectType.LandingPage)
                LandingPageRow.Delete(DB, Id)

                logDetail.ObjectType = Utility.Common.ObjectType.LandingPage.ToString()
                logDetail.ObjectId = Id
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)

                If (Not GetPageParams(FilterFieldType.All) = String.Empty) Then
                    Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
                End If
            End If
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
        Response.Redirect("default.aspx")

    End Sub
End Class
