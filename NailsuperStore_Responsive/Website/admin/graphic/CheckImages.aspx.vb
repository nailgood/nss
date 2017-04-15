Imports Components
Imports DataLayer

Partial Class CheckImages
    Inherits AdminPage
    Protected Style As String = ""
    Private SQL, Sort As String
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AdminId") = Nothing Then Response.Redirect("/")
        If Not Page.IsPostBack Then
            LoadData()
        End If
        
    End Sub
    Private Sub LoadData()
        Sort = Request("sort")
        If Sort Is Nothing = True Then
            Sort = ""
        Else
            Sort = Sort.Replace("-", " ")
        End If
        SQL = "select sku, case when Image is null then 'No' else Image end as Image, ItemName, case when longdesc is null then 'No' else 'Yes' end as HasDescription, case when ShortDesc is null then 'No' else 'Yes' end as ShortDesc , case when UrlCode is null then 'No' else 'Yes' end as UrlCode from storeitem where isactive = 1 and Image is null "
        Dim dt As DataTable = DB.GetDataTable(SQL & Sort)
        rptMissing.DataSource = dt
        rptMissing.DataBind()
    End Sub
    'Private Sub LoadData_Old()
    '    Dim ShortDesc, Condition, SQL As String
    '    Dim YesRecords, NoRecords, NoneSpecified As Integer
    '    Dim dYes, dNo As Integer
    '    Dim Sort As String = Request("sort")
    '    If Sort Is Nothing = True Then
    '        Sort = ""
    '    Else
    '        Sort = Sort.Replace("-", " ")
    '    End If
    '    SQL = "select sku, image, ItemName, case when longdesc is null then 'No' else 'Yes' end as HasDescription, isnull(ShortDesc,'') as ShortDesc, UrlCode from storeitem where isactive = 1 "
    '    Select Case GetQueryString("chk")
    '        Case "i" 'Image
    '            SQL &= " and Image is null "
    '            Style = "Image"
    '        Case "sd" 'shortdesc
    '            SQL &= " and ShortDesc is null "
    '            Style = "ShortDesc"
    '        Case "ld" 'longdesc
    '            SQL &= " and LongDesc is null "
    '            Style = "LongDesc"
    '        Case "uc" 'urlcode
    '            SQL &= " and UrlCode is null "
    '            Style = "UrlCode"
    '        Case "dp" 'department'
    '            SQL = "select sku, image, ItemName, case when longdesc is null then 'No' else 'Yes' end as HasDescription, isnull(ShortDesc,'') as ShortDesc, UrlCode from StoreItem where IsActive = 1 and ItemId not in(select distinct itemid from StoreDepartmentItem)"
    '            Style = "Department"
    '    End Select


    '    Dim dt As DataTable = DB.GetDataTable(SQL & Sort)

    '    'Response.Write("<table border=""1""><tr><th>SKU</th><th>Product Name</th><th><a href='/checkimages.aspx?sort=Order-by-HasDescription-Asc'>LongDescription?</a></th><th><a href='/checkimages.aspx?sort=Order-by-ShortDesc-Asc'>ShortDesciption</a></th><th>Image Name</th><th>Exists?</th></tr>")
    '    ltcontent.Text &= "<table border=""1""><tr><th>SKU</th><th>Product Name</th><th>LongDescription</th><th>ShortDesciption</th><th>Image Name</th><th>UrlCode</th><th>Exists?</th></tr>"
    '    For Each row As DataRow In dt.Rows
    '        ShortDesc = row("ShortDesc")
    '        ltcontent.Text &= "<tr><td>" & row("SKU") & "</td><td>" & row("ItemName") & "</td><td>" & row("hasdescription") & "</td><td>" & IIf(ShortDesc = "" Or ShortDesc.Length <= 14, "No", "Yes") & "</td><td>" & row("Image") & "</td><td>" & row("UrlCode") & "</td><td>"
    '        'Response.Write("<tr><td>" & row("SKU") & "</td><td>" & row("ItemName") & "</td><td>" & row("hasdescription") & "</td><td>" & IIf(ShortDesc = "" Or ShortDesc.Length <= 14, "No", "Yes") & "</td><td>" & row("Image") & "</td><td>")
    '        Select Case row("HasDescription")
    '            Case "Yes"
    '                dYes += 1
    '            Case Else
    '                dNo += 1
    '        End Select
    '        If Not IsDBNull(row("Image")) AndAlso Not Trim(row("Image")) = String.Empty Then
    '            If Core.FileExists(Server.MapPath("/assets/items/cart/") & row("Image")) Then
    '                ltcontent.Text &= "Yes"
    '                'Response.Write("Yes")
    '                YesRecords += 1
    '            Else
    '                ltcontent.Text &= "No"
    '                'Response.Write("No")
    '                NoRecords += 1
    '            End If
    '        Else
    '            ltcontent.Text &= "NA"
    '            'Response.Write("NA")
    '            NoneSpecified += 1
    '        End If
    '        ltcontent.Text &= "</td></tr>" & vbCrLf
    '        'Response.Write("</td></tr>" & vbCrLf)
    '    Next
    '    ltcontent.Text &= "</table><p>" & dt.Rows.Count & " records</p><p><b>IMAGES</b><br>" & YesRecords & " - Yes<br>" & NoRecords & " - No<br>" & NoneSpecified & " - NA</p>"
    '    'Response.Write("</table><p>" & dt.Rows.Count & " records</p><p><b>IMAGES</b><br>" & YesRecords & " - Yes<br>" & NoRecords & " - No<br>" & NoneSpecified & " - NA</p>")
    '    ltcontent.Text &= "<p><b>DESCRIPTIONS</b><br>" & dYes & " - Yes<br>" & dNo & " - No"
    '    'Response.Write("<p><b>DESCRIPTIONS</b><br>" & dYes & " - Yes<br>" & dNo & " - No")
    'End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        SQL = "select sku, case when Image is null then 'No' else Image end as Image, ItemName, case when longdesc is null then 'No' else 'Yes' end as HasDescription, case when ShortDesc is null then 'No' else 'Yes' end as ShortDesc , case when UrlCode is null then 'No' else 'Yes' end as UrlCode from storeitem where isactive = 1 "
        Dim Con As String = ""
        For Each li As ListItem In chkMissing.Items
            If li.Selected Then
                If li.Value = "i" Then
                    SQL &= " and Image is null "
                End If
                If li.Value = "ld" Then
                    SQL &= " and LongDesc is null "
                End If
                If li.Value = "sd" Then
                    SQL &= " and ShortDesc is null "
                End If
                If li.Value = "uc" Then
                    SQL &= " and UrlCode is null "
                End If
                If li.Value = "dp" Then
                    SQL &= " and ItemId not in(select distinct itemid from StoreDepartmentItem)"
                End If
            End If
        Next
        Dim dt As DataTable = DB.GetDataTable(SQL & Sort)
        rptMissing.DataSource = dt
        rptMissing.DataBind()
    End Sub
End Class
