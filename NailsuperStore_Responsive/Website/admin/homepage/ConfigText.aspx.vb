Imports Components
Imports System.Data
Imports DataLayer
Imports Controls
Imports Utility
Partial Class admin_homepage_ConfigText
    Inherits AdminPage


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindRepeater()
        End If
    End Sub

    Private Sub BindRepeater()
        Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, LoggedInAdminId)
        configtext.DataSource = SysparamRow.GetList(DB, dbAdmin.IsInternal, "Home Page")
        configtext.DataBind()
    End Sub

    Private prevGroup As String = ""

    Private Sub configtext_OnItemDataBound(ByVal sender As System.Object, ByVal e As RepeaterItemEventArgs) Handles configtext.ItemDataBound
        Dim plcEditPlace As PlaceHolder = Nothing
        Dim plcValidatePlace As PlaceHolder = Nothing
        Dim lblSysparamName As Label = Nothing
        Dim trHeaderRow As HtmlTableRow = Nothing
        Dim lblHeaderLabel As Label = Nothing
        Dim bHasRow As Boolean = False
        Dim btnSave As Button = Nothing

        Select Case e.Item.ItemType
            Case ListItemType.Item
                lblSysparamName = e.Item.FindControl("sysparamName")
                plcEditPlace = e.Item.FindControl("editPlace")
                plcValidatePlace = e.Item.FindControl("validatePlace")
                trHeaderRow = e.Item.FindControl("headerRow")
                lblHeaderLabel = e.Item.FindControl("headerLabel")
                btnSave = e.Item.FindControl("saveButton")
                bHasRow = True
            Case ListItemType.AlternatingItem
                lblSysparamName = e.Item.FindControl("sysparamNameALT")
                plcEditPlace = e.Item.FindControl("editPlaceALT")
                plcValidatePlace = e.Item.FindControl("validatePlaceALT")
                trHeaderRow = e.Item.FindControl("headerRowALT")
                lblHeaderLabel = e.Item.FindControl("headerLabelALT")
                btnSave = e.Item.FindControl("saveButtonALT")
                bHasRow = True
        End Select

        If bHasRow Then
            Dim id As Integer = e.Item.DataItem("ParamId")
            Dim groupName As String = e.Item.DataItem("GroupName")
            Dim name As String = e.Item.DataItem("Name")
            Dim value As String
            If IsDBNull(e.Item.DataItem("Value")) Then
                value = String.Empty
            Else
                'If e.Item.DataItem("IsEncrypted") Then
                'value = Utility.Crypt.DecryptTripleDES(e.Item.DataItem("Value"))
                'Else
                value = e.Item.DataItem("Value")
                'End If
            End If
            Dim type As String = IIf(IsDBNull(e.Item.DataItem("Type")), String.Empty, e.Item.DataItem("Type"))
            Dim comments As String
            If IsDBNull(e.Item.DataItem("Comments")) Then
                comments = ""
            Else
                comments = e.Item.DataItem("Comments")
            End If

            lblSysparamName.Text = comments

            Dim box As New TextBox
            Dim drp As New DropDownList
            Select Case type
                Case String.Empty
                    btnSave.Visible = False
                Case Else
                    box.ID = "PARAM" & id
                    box.Text = value
                    plcEditPlace.Controls.Add(box)
                    btnSave.CommandArgument = id & "|" & box.UniqueID
            End Select
            box.Columns = 40
            box.MaxLength = 255
            If groupName = prevGroup Then
                trHeaderRow.Visible = False
            Else
                trHeaderRow.Visible = True
                lblHeaderLabel.Text = "Shop Your Way Menu" 'groupName
                prevGroup = groupName
            End If
        End If
    End Sub

    Private Sub configtext_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles configtext.ItemCommand
        If e.CommandName = "Save" Then
            Dim tmp As String = e.CommandArgument
            Dim aTmp() As String = tmp.Split("|")
            Dim value As String = Request(aTmp(1))
            Dim id As Integer = aTmp(0)
            Dim dbSysParam As SysparamRow = SysparamRow.GetRow(DB, id)
            Dim changeLog As String = String.Empty
            If dbSysParam.Name = "PasswordEx" AndAlso value > 0 Then
                DB.ExecuteSQL("Update Admin Set PasswordEx=Password,PasswordDate=null where Password is not null")
            End If
            If (dbSysParam.Value.ToString().ToLower().Trim() <> value.ToLower().Trim()) Then
                changeLog = dbSysParam.Comments & "|" & dbSysParam.Value.ToString().Trim() & "|" & value.Trim() & "[br]"
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = id
                logDetail.ObjectType = Utility.Common.ObjectType.Sysparam.ToString()
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
            dbSysParam.Value = value
            dbSysParam.Update()


            '' WriteLogDetail("Update System param", dbSysParam)
            Utility.CacheUtils.RemoveCache("Sysparam_ListAll")
            'Core.LogEvent("Value for System Parameter """ & dbSysParam.Name & """ was set to """ & dbSysParam.Value & """ by user """ & Session("Username") & """", Diagnostics.EventLogEntryType.Information)
        End If
        BindRepeater()
    End Sub
  
End Class
