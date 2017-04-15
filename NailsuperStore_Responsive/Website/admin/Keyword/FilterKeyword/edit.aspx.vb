
Imports System.Collections.Generic
Imports System.Linq
Imports Components
Partial Class admin_Keyword_FilterKeyword_edit
    Inherits AdminPage
    Private _Id As Integer = 0
    Private currentFilterProperty As String = String.Empty, currentFilterValue As String = String.Empty
    Private keywordId As String = String.Empty, keywordSynonymId As String = String.Empty
    Public Property KwSearch() As String
        Get
            Return ViewState("KwSearch")
        End Get
        Set(ByVal value As String)
            ViewState("KwSearch") = value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Request.QueryString("id") Then
            _Id = Request.QueryString("id")
        End If
        If Request.QueryString("KwSearch") IsNot Nothing Then
            KwSearch = Request.QueryString("KwSearch")
        End If

        If Request.QueryString("keywordId") IsNot Nothing Then
            keywordId = Request.QueryString("keywordId")
        End If
        If Request.QueryString("keywordSynonymId") IsNot Nothing Then
            keywordSynonymId = Request.QueryString("keywordSynonymId")
        End If
        If Not Page.IsPostBack Then

            'fill keywordname
            If ddlFilterType.SelectedValue = "ReplaceKeyword" Then
                'Dim dt As DataTable = DB.GetDataTable("select distinct a.KeywordName, a.KeywordId from Keyword a inner join Keyword b on a.KeywordId = b.ReplaceKeywordId order by a.KeywordName")
                'lbxKwName.DataSource = dt.DefaultView
                'lbxKwName.DataValueField = "KeywordId"
                'lbxKwName.DataTextField = "KeywordName"
                'lbxKwName.DataBind()
                Dim dt As DataTable = Nothing

                ddlFilterProperty.DataSource = KeywordFilter.filterProperties
                ddlFilterProperty.DataBind()

                If _Id = 0 AndAlso String.IsNullOrEmpty(keywordId) Then 'default
                    dt = DB.GetDataTable("select convert(varchar(20), a.KeywordId) + '-' + b.KeywordName as OriginalKeyword, b.KeywordId as OriginalKeywordId, a.KeywordName, a.KeywordId from Keyword a inner join Keyword b on b.KeywordId = a.ReplaceKeywordId order by a.KeywordName")
                ElseIf Not String.IsNullOrEmpty(keywordId) 'from kw replace redirect
                    dt = DB.GetDataTable("select convert(varchar(20), a.KeywordId) + '-' + b.KeywordName as OriginalKeyword, b.KeywordId as OriginalKeywordId, a.KeywordName, a.KeywordId from Keyword a inner join Keyword b on b.KeywordId = a.ReplaceKeywordId where a.KeywordId = " & keywordId & " and a.ReplaceKeywordId=" & keywordSynonymId & " and exists (select 1 from KeywordFilter c where c.keywordId = a.KeywordId and c.OriginalKeywordId = a.ReplaceKeywordId) order by a.KeywordName")
                    If dt.Rows.Count > 0 Then
                        Response.Redirect("default.aspx?createNew=true&KwSearch=" & keywordId)
                    Else
                        dt = DB.GetDataTable("select convert(varchar(20), a.KeywordId) + '-' + b.KeywordName as OriginalKeyword, b.KeywordId as OriginalKeywordId, a.KeywordName, a.KeywordId from Keyword a inner join Keyword b on b.KeywordId = a.ReplaceKeywordId where a.KeywordId = " & keywordId & " and a.ReplaceKeywordId=" & keywordSynonymId & " and not exists (select 1 from KeywordFilter c where c.keywordId = a.KeywordId and c.OriginalKeywordId = a.ReplaceKeywordId) order by a.KeywordName")
                    End If
                Else 'edit
                    dt = DB.GetDataTable("select c.FilterProperty, c.FilterValue, convert(varchar(20), a.KeywordId) + '-' + b.KeywordName as OriginalKeyword, b.KeywordId as OriginalKeywordId, a.KeywordName, a.KeywordId from Keyword a inner join Keyword b on b.KeywordId = a.ReplaceKeywordId inner join KeywordFilter c on c.Id = " & _Id.ToString() & " and c.KeywordId = a.KeywordId and c.OriginalKeywordId = b.KeywordId order by a.KeywordName")
                    If dt.Rows.Count > 0 Then
                        currentFilterProperty = dt.Rows(0)("FilterProperty")
                        currentFilterValue = dt.Rows(0)("FilterValue")
                    End If
                End If
                lbxKwName.DataSource = dt
                lbxKwName.DataValueField = "OriginalKeyword"
                lbxKwName.DataTextField = "KeywordName"
                lbxKwName.DataBind()
                lbxKwName.SelectedIndex = 0
                If Not String.IsNullOrEmpty(currentFilterValue) Then
                    ddlFilterProperty.SelectedValue = currentFilterProperty
                End If
                ddlFilterProperty_SelectedIndexChanged(Nothing, Nothing)


                End If

        End If
    End Sub

    Private Sub Back()
        If Not String.IsNullOrEmpty(keywordId) Then 'ReplaceKeyword call this
            Response.Redirect("../ReplaceKeyword/default.aspx")
        ElseIf Not String.IsNullOrEmpty(KwSearch) Then
            Response.Redirect("default.aspx?kwSearch=" & KwSearch)
        Else
            Response.Redirect("default.aspx")
        End If

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then

            Dim objkeyword As KeywordFilter
            If (_Id > 0) Then
                objkeyword = KeywordFilter.GetRow(_Id)
            Else
                objkeyword = New KeywordFilter
            End If
            Dim originalString As String = lbxKwName.SelectedValue.Split("-")(1)
            objkeyword.KeyWordName = lbxKwName.SelectedItem.Text
            objkeyword.FilterProperty = ddlFilterProperty.Text
            objkeyword.FilterValue = ddlValue.Text.Replace("- ", "").Trim()
            objkeyword.FilterType = ddlFilterType.Text
            objkeyword.OriginalKeyword = originalString
            If _Id > 0 Then
                objkeyword.ID = _Id
                KeywordFilter.Update(objkeyword)
            Else
                KeywordFilter.Insert(objkeyword)
            End If
            Back()
        End If

    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Back()
    End Sub
    Protected Sub ddlFilterProperty_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlValue.Items.Clear()
        lbxOriginalKeyword.Items.Clear()
        Dim originalString As String = lbxKwName.SelectedValue.Split("-")(1)
        Dim kwName As String = lbxKwName.SelectedItem.Text.Trim()
        lbxOriginalKeyword.Items.Add(New ListItem(originalString))
        Dim kw As String = originalString
        Dim dt As DataTable = KeywordFilter.BuildQueryString(kwName)
        Dim temp = Nothing
        Dim dfFilter As String = String.Empty
        Dim DepartmentId As String = String.Empty, brandId = 0, collectionid = 0, toneid = 0, shadeid = 0

        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                Dim queryString As String = dt.Rows(index)(0).ToString()
                If queryString.Contains("brandid=") AndAlso ddlFilterType.SelectedValue <> "Brands" Then
                    brandId = CInt(queryString.Split("=")(1))
                ElseIf queryString.Contains("DepartmentId=") AndAlso ddlFilterType.SelectedValue <> "Categories"
                    DepartmentId = queryString.Split("=")(1)
                ElseIf queryString.Contains("collectionid=") AndAlso ddlFilterType.SelectedValue <> "Collection"
                    collectionid = CInt(queryString.Split("=")(1))
                ElseIf queryString.Contains("tondeid=") AndAlso ddlFilterType.SelectedValue <> "Tones"
                    toneid = CInt(queryString.Split("=")(1))
                ElseIf queryString.Contains("shadeid=") AndAlso ddlFilterType.SelectedValue <> "Shades"
                    shadeid = CInt(queryString.Split("=")(1))
                End If
            Next
        End If
        Dim replaceKeywordName As String = String.Empty, filterProperty_replaceKw = String.Empty
        Dim ds As DataSet = DataLayer.KeywordRow.GetReplaceKeywordWithFilter(kwName)
        If ds IsNot Nothing AndAlso ds.Tables("1") IsNot Nothing AndAlso ds.Tables("1").Rows.Count > 0 Then
            replaceKeywordName = ds.Tables("1").Rows(0)(0).ToString()
            If ds.Tables("2") IsNot Nothing AndAlso ds.Tables("2").Rows.Count > 0 Then
                Dim drs As DataRow() = ds.Tables("2").Select("FilterProperty = 'DepartmentId'")
                If drs.Length = 1 Then
                    dfFilter = drs(0)(1)
                End If

                If ddlFilterProperty.SelectedValue = "Categories" AndAlso _Id > 0 Then 'edit
                    Dim departmentSel As String = currentFilterValue
                    Dim dtDepartment As DataTable = DB.GetDataTable(String.Format("select DepartmentId from StoreDepartment where ISNULL(AlternateName, Name) = '{0}'", currentFilterValue))
                    If dtDepartment IsNot Nothing AndAlso dtDepartment.Rows.Count = 1 Then
                        Dim deprtmentId As String = dtDepartment.Rows(0)(0)

                        If dfFilter.Length > 0 Then
                            dfFilter = dfFilter.Replace(deprtmentId, "").Replace(",,", ",")
                            If dfFilter(0) = System.Convert.ToChar(",") Then
                                dfFilter = dfFilter.Substring(1, dfFilter.Length - 1)
                            End If
                            If dfFilter(dfFilter.Length - 1) = System.Convert.ToChar(",") Then
                                dfFilter = dfFilter.Substring(0, dfFilter.Length - 1)
                            End If
                        End If

                    End If
                End If
            End If
        End If
        If (Not String.IsNullOrEmpty(replaceKeywordName)) Then
            Dim fqDefault As String = String.Empty

            If ds.Tables("2").Rows.Count > 0 Then
                For dx = 0 To ds.Tables("2").Rows.Count - 1
                    Dim property_ As String = ds.Tables("2").Rows(dx)(0).ToString()
                    If Not String.IsNullOrEmpty(property_) Then
                        Dim vals As String = ds.Tables("2").Rows(0)(1).ToString()
                        If Not String.IsNullOrEmpty(vals) Then
                            Dim val As String() = vals.Split(",")
                            For index = 0 To val.Length - 1
                                fqDefault = IIf(Not String.IsNullOrEmpty(fqDefault), fqDefault + " OR " + property_.ToLower() + ":" + val(index), property_.ToLower() + ":" + val(index))
                            Next
                        End If
                    End If
                    If Not String.IsNullOrEmpty(fqDefault) Then
                        filterProperty_replaceKw = fqDefault
                    End If
                Next
            End If
        End If

        If dt.Rows.Count > 0 Then
            temp = SolrHelper.SearchItem(kw, Nothing, String.Empty, 1, 1, 1, DepartmentId, brandId, String.Empty, String.Empty, False, String.Empty, collectionid, toneid, shadeid, filterProperty_replaceKw, False)
        Else
            temp = SolrHelper.SearchItem(kw, Nothing, String.Empty, 1, 1, 1, String.Empty, 0, String.Empty, String.Empty, False, String.Empty, 0, 0, 0, filterProperty_replaceKw, False)
        End If


        Dim filterProperty As String = ddlFilterProperty.Text
        If Not String.IsNullOrEmpty(filterProperty) Then
            If filterProperty = "Tones" Then
                Dim tonesStr As String = HttpContext.Current.Session("searchCountTone")
                If Not String.IsNullOrEmpty(tonesStr) Then
                    Dim tonesSearchArr = tonesStr.Split(",").Where(Function(i) i.Length > 0).ToArray()
                    If tonesSearchArr.Length > 0 Then
                        Dim whereStr As String = String.Empty
                        For index = 0 To tonesSearchArr.Length - 1
                            If tonesSearchArr(index).Contains(":") Then
                                whereStr = whereStr & tonesSearchArr(index).Split(":")(0) & ","
                            End If
                        Next
                        If whereStr.Length > 0 Then
                            whereStr = whereStr.Substring(0, whereStr.Length - 1)
                            Dim tones = DB.GetDataTable(String.Format("select toneid,tone from StoreTone where ToneId in ({0}) {1} order by tone", whereStr, IIf(toneid > 0 AndAlso _Id = 0, " and toneid <> " & toneid.ToString(), "")))
                            If tones.Rows.Count > 1 Then
                                ddlValue.DataSource = tones
                                ddlValue.DataValueField = "toneid"
                                ddlValue.DataTextField = "tone"
                                ddlValue.DataBind()
                            Else
                                ddlValue.Items.Clear()
                            End If
                        End If
                    End If
                End If
            ElseIf filterProperty = "Shades" Then
                Dim shadesStr As String = HttpContext.Current.Session("searchCountShade")
                If Not String.IsNullOrEmpty(shadesStr) Then
                    Dim shadesSearchArr = shadesStr.Split(",").Where(Function(i) i.Length > 0).ToArray()
                    If shadesSearchArr.Length > 0 Then
                        Dim whereStr As String = String.Empty
                        For index = 0 To shadesSearchArr.Length - 1
                            If shadesSearchArr(index).Contains(":") Then
                                whereStr = whereStr & shadesSearchArr(index).Split(":")(0) & ","
                            End If
                        Next
                        If whereStr.Length > 0 Then
                            whereStr = whereStr.Substring(0, whereStr.Length - 1)
                            Dim tones = DB.GetDataTable(String.Format("select shade from StoreShade where ShadeId in ({0}) {1} order by shade", whereStr, IIf(shadeid > 0 AndAlso _Id = 0, " and shadeid <> " & shadeid.ToString(), "")))
                            If tones.Rows.Count > 1 Then
                                ddlValue.DataSource = tones
                                ddlValue.DataValueField = "shade"
                                ddlValue.DataTextField = "shade"
                                ddlValue.DataBind()
                            Else
                                ddlValue.Items.Clear()
                            End If

                        End If
                    Else
                        ddlValue.Items.Clear()
                    End If
                Else
                    ddlValue.Items.Clear()
                End If
            ElseIf filterProperty = "Collection" Then
                Dim CollectionStr As String = HttpContext.Current.Session("searchCountCollection")
                If Not String.IsNullOrEmpty(CollectionStr) Then
                    Dim CollectionsSearchArr = CollectionStr.Split(",").Where(Function(i) i.Length > 0).ToArray()
                    If CollectionsSearchArr.Length > 0 Then
                        Dim whereStr As String = String.Empty
                        For index = 0 To CollectionsSearchArr.Length - 1
                            If CollectionsSearchArr(index).Contains(":") Then
                                whereStr = whereStr & CollectionsSearchArr(index).Split(":")(0) & ","
                            End If
                        Next
                        If whereStr.Length > 0 Then
                            whereStr = whereStr.Substring(0, whereStr.Length - 1)
                            Dim tones = DB.GetDataTable(String.Format("select CollectionName from StoreCollection where CollectionId in ({0}) {1} order by CollectionName", whereStr, IIf(collectionid > 0 AndAlso _Id = 0, " and CollectionId <> " & collectionid, "")))
                            ddlValue.DataSource = tones
                            If tones.Rows.Count > 1 Then
                                ddlValue.DataValueField = "CollectionName"
                                ddlValue.DataTextField = "CollectionName"
                                ddlValue.DataBind()
                            Else
                                ddlValue.Items.Clear()
                            End If
                        End If
                    Else
                        ddlValue.Items.Clear()
                    End If
                Else
                    ddlValue.Items.Clear()
                End If
            ElseIf filterProperty = "Categories" Then
                Dim DepartmentStr As String = HttpContext.Current.Session("searchCountCategory")
                If Not String.IsNullOrEmpty(DepartmentStr) Then
                    Dim DepartmentSearchArr = DepartmentStr.Split(",").Where(Function(i) i.Length > 0).ToArray()
                    If DepartmentSearchArr.Length > 0 Then
                        Dim whereStr As String = String.Empty
                        For index = 0 To DepartmentSearchArr.Length - 1
                            If DepartmentSearchArr(index).Contains(":") Then
                                Dim departmentSplit = DepartmentSearchArr(index).Split(":")
                                If departmentSplit(1) <> "0" Then
                                    whereStr = whereStr & DepartmentSearchArr(index).Split(":")(0) & ","
                                End If
                            End If
                        Next
                        If whereStr.Length > 0 Then
                            whereStr = whereStr.Substring(0, whereStr.Length - 1)
                            Dim tones = DB.GetDataTable(String.Format("select ParentId, DepartmentId, isnull(AlternateName,Name) as OriginalName, case when ParentId <> 23 then '- ' + isnull(AlternateName,Name) else isnull(AlternateName,Name) end as Name from StoreDepartment where DepartmentId in ({0}) and len(isnull(AlternateName,Name)) > 0 and ParentId is not null {1} {2} order by isnull(AlternateName,Name)", whereStr, IIf(Not String.IsNullOrEmpty(DepartmentId) AndAlso _Id = 0, " and (DepartmentId <> " & DepartmentId & " and parentId <> " & DepartmentId & ")", ""), IIf(String.IsNullOrEmpty(dfFilter), "", String.Format(" and departmentid not in ({0})", dfFilter))))
                            If tones.Rows.Count > 1 Then
                                'xep categories
                                Dim result As List(Of DataRow) = New List(Of DataRow)()
                                Dim parentCategories = tones.Select("ParentId = 23")
                                For idx = 0 To parentCategories.Count() - 1
                                    result.Add(parentCategories(idx))
                                    Dim child = tones.Select("ParentId= " & parentCategories(idx)("DepartmentId"))
                                    If child IsNot Nothing AndAlso child.Length > 0 Then
                                        result.AddRange(child)
                                    End If
                                Next

                                ddlValue.DataSource = result.CopyToDataTable()
                                ddlValue.DataValueField = "OriginalName"
                                ddlValue.DataTextField = "Name"
                                ddlValue.DataBind()
                            Else
                                ddlValue.Items.Clear()
                            End If

                        End If
                    Else
                        ddlValue.Items.Clear()
                    End If
                Else
                    ddlValue.Items.Clear()
                End If
            ElseIf filterProperty = "Brands" Then
                Dim BrandsStr As String = HttpContext.Current.Session("searchCountBrand")
                If Not String.IsNullOrEmpty(BrandsStr) Then
                    Dim BrandsSearchArr = BrandsStr.Split(",").Where(Function(i) i.Length > 0).ToArray()
                    If BrandsSearchArr.Length > 0 Then
                        Dim whereStr As String = String.Empty
                        For index = 0 To BrandsSearchArr.Length - 1
                            If BrandsSearchArr(index).Contains(":") Then
                                whereStr = whereStr & BrandsSearchArr(index).Split(":")(0) & ","
                            End If
                        Next
                        If whereStr.Length > 0 Then
                            whereStr = whereStr.Substring(0, whereStr.Length - 1)
                            Dim tones = DB.GetDataTable(String.Format("select BrandName from StoreBrand where BrandId in ({0}) {1} order by BrandName", whereStr, IIf(brandId > 0 AndAlso _Id = 0, " and BrandId <>" & brandId, "")))
                            If tones.Rows.Count > 1 Then
                                ddlValue.DataSource = tones
                                ddlValue.DataValueField = "BrandName"
                                ddlValue.DataTextField = "BrandName"
                                ddlValue.DataBind()
                            Else
                                ddlValue.Items.Clear()
                            End If

                        End If
                    Else
                        ddlValue.Items.Clear()
                    End If
                Else
                    ddlValue.Items.Clear()
                End If
            End If
        End If
        If Not String.IsNullOrEmpty(currentFilterValue) Then
            ddlValue.SelectedValue = currentFilterValue
        End If
    End Sub
    Protected Sub lbxKwName_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlFilterProperty_SelectedIndexChanged(Nothing, Nothing)
        'Dim KwId As String = lbxKwName.SelectedValue
        'If Not String.IsNullOrEmpty(KwId) Then
        '    If ddlFilterType.SelectedValue = "ReplaceKeyword" Then
        '        Dim dt As DataTable = Nothing
        '        If ID > 0 Then
        '            dt = DB.GetDataTable("select b.KeywordName as OriginalKeyword, b.KeywordId from Keyword a inner join Keyword b on a.KeywordId = b.ReplaceKeywordId inner join KeywordFilter c on a.KeywordId = c.KeywordId and b.KeywordId = c.OriginalKeywordId and c.Id = " & ID.ToString())
        '        Else
        '            dt = DB.GetDataTable("select b.KeywordName as OriginalKeyword, b.KeywordId from Keyword a inner join Keyword b on a.KeywordId = b.ReplaceKeywordId where a.KeywordId = " & KwId)
        '        End If

        '        lbxOriginalKeyword.DataSource = dt
        '        lbxOriginalKeyword.DataTextField = "OriginalKeyword"
        '        lbxOriginalKeyword.DataValueField = "KeywordId"
        '        lbxOriginalKeyword.DataBind()
        '        lbxOriginalKeyword.SelectedIndex = 0
        '        ddlFilterProperty.DataSource = KeywordFilter.filterProperties
        '        ddlFilterProperty.DataBind()
        '        ddlFilterProperty_SelectedIndexChanged(Nothing, Nothing)
        '    End If
        'End If
    End Sub
    Protected Sub lbxOriginalKeyword_SelectedIndexChanged1(sender As Object, e As EventArgs)
        ddlFilterProperty_SelectedIndexChanged(Nothing, Nothing)
    End Sub
End Class
