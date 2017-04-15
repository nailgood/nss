Imports DataLayer
Imports Components

Partial Class admin_content_Confirm
    Inherits AdminPage

    Protected dbPage As ContentToolPageRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dbPage = ContentToolPageRow.GetRow(DB, CInt(Request("PageId")))
    End Sub
End Class
