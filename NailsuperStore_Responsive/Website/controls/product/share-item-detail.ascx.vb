Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class controls_product_share_item_detail

    Inherits BaseControl
    Public shareURL As String = String.Empty
    Public shareDescription As String
    Public linkMSDS As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ucAddthis.shareURL = shareURL
        'ucAddthis.shareDescription = shareDescription
    End Sub
End Class
