
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_graphic_InforBanner_Default
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            BindList()
        End If
    End Sub
    Private total As Integer = 0
    Private Sub BindList()
        Dim lstBanner As InforBannerCollection = InforBannerRow.GetAllByType(Utility.Common.InforBannerType.Main)
        If Not lstBanner Is Nothing AndAlso lstBanner.Count > 0 Then
            gvList.Pager.NofRecords = lstBanner.Count
            gvList.DataSource = lstBanner
            total = lstBanner.Count
        Else
            gvList.DataSource = Nothing
        End If
        gvList.DataBind()
    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand

        Try
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)
            If e.CommandName = "Down" Then
                InforBannerRow.ChangeArrange(id, False)
            ElseIf (e.CommandName = "Up") Then
                InforBannerRow.ChangeArrange(id, True)
            End If
        Catch ex As Exception

        End Try

        BindList()
    End Sub

   
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx")
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ltrImage As Literal = CType(e.Row.FindControl("ltrImage"), Literal)
            Dim objBanner As InforBannerRow = e.Row.DataItem
            If Not String.IsNullOrEmpty(objBanner.Image) Then
                ltrImage.Text = "<img src='" & Utility.ConfigData.PathMainInforBanner & "/" & objBanner.Image & "?t=" & DateTime.Now.Millisecond.ToString() & "' width='100px'/>"
            End If

            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            imbUp.CommandArgument = objBanner.Id
            imbDown.CommandArgument = objBanner.Id
            If e.Row.DataItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Row.DataItemIndex = total - 1 Then
                imbDown.Visible = False
            End If
        End If
    End Sub




    
  
End Class

