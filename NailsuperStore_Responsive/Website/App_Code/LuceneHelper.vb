Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Collections.Generic
Imports Lucene.Net
Imports Lucene.Net.Analysis
Imports Lucene.Net.Analysis.Standard
Imports Lucene.Net.Documents
Imports Lucene.Net.Index
Imports Lucene.Net.QueryParsers
Imports Lucene.Net.Search
Imports Lucene.Net.Store
Imports Version = Lucene.Net.Util.Version
Imports DataLayer
Imports System.Reflection
Imports Utility.Common
Imports Humanizer
Public Class LuceneHelper

    Private _luceneStoreItemDir As String = Utility.ConfigData.LuceneItemIndexPath
    Private _luceneKeywordDir As String = Utility.ConfigData.LuceneKeywordIndexPath
    Private _luceneArticleDir As String = Utility.ConfigData.LuceneArticleIndexPath
    Private _luceneVideoDir As String = Utility.ConfigData.LuceneVideoIndexPath
    Private _directoryTemp As FSDirectory
    Private Shared m_counter As Integer = 0
    Public Property counter() As Integer
        Get
            Return m_counter
        End Get
        Set(ByVal value As Integer)
            m_counter = value
        End Set
    End Property
    Private Function _directory(ByVal Type As Integer) As FSDirectory
        Dim luceneDir As String
        If (Type = 1) Then
            luceneDir = _luceneStoreItemDir
        ElseIf (Type = 2) Then
            luceneDir = _luceneArticleDir
        ElseIf (Type = 3) Then
            luceneDir = _luceneKeywordDir
        ElseIf (Type = 4) Then
            luceneDir = _luceneVideoDir
        End If

        _directoryTemp = FSDirectory.Open(New DirectoryInfo(luceneDir))

        If (IndexWriter.IsLocked(_directoryTemp)) Then
            IndexWriter.Unlock(_directoryTemp)
        End If

        Dim lockFilePath As String = Path.Combine(luceneDir, "write.lock")



        If (File.Exists(lockFilePath)) Then
            File.Delete(lockFilePath)
        End If
        Return _directoryTemp

    End Function

    '''''''''''''''''''''''''''''''''''''''
    ''''''''Search Store Item Lucene'''''''
    '''''''''''''''''''''''''''''''''''''''
    Public Function LuceneTypeSort(ByVal orderBy As String) As Integer
        If orderBy = "price" Or orderBy = "top-rated" Then
            Return SortField.FLOAT
        ElseIf orderBy = "product" Then
            Return SortField.STRING
        Else
            Return SortField.INT
        End If
    End Function
    Public Function LuceneTypeSortExp(ByVal orderBy As String) As Boolean
        If orderBy = "product" Then
            Return False
        End If
        Return True
    End Function
    Public Function TypeSort(ByVal orderBy As String) As SortField()
        Dim sort As SortField()
        Select Case orderBy
            Case "price"
                sort = New SortField() {New SortField("price-filter", SortField.DOUBLE, False), New SortField("LowPrice", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "pricehigh"
                sort = New SortField() {New SortField("price-filter", SortField.DOUBLE, True), New SortField("LowPrice", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}

            Case "product"
                sort = New SortField() {New SortField("product", SortField.STRING, False), New SortField("price", SortField.FLOAT, False), New SortField("LowPrice", SortField.FLOAT, False)}
            Case "best-sellers"
                sort = New SortField() {New SortField("best-sellers", SortField.INT, True), New SortField("LowPrice", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "new-items"
                sort = New SortField() {New SortField("new-items", SortField.INT, True), New SortField("LowPrice", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "hot-items"
                sort = New SortField() {New SortField("hot-items", SortField.INT, True), New SortField("LowPrice", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "on-sale"
                sort = New SortField() {New SortField("on-sale", SortField.INT, True), New SortField("price", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "top-rated"
                sort = New SortField() {New SortField("top-rated", SortField.FLOAT, True), New SortField("price", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case "most-popular-review"
                sort = New SortField() {New SortField("most-popular-review", SortField.INT, True), New SortField("price", SortField.FLOAT, False), New SortField("product", SortField.STRING, False)}
            Case Else
                sort = New SortField() {New SortField("product", SortField.STRING, False), New SortField("price", SortField.FLOAT, False), New SortField("LowPrice", SortField.FLOAT, False)}
        End Select
        Return sort
    End Function
    Private Function RemoveKeywordIgnore(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If
        If (keyword.Contains(" ")) Then
            Dim lstEmptyKeyword As KeywordIgnoreCollection = KeywordIgnoreRow.GetAll()
            If Not lstEmptyKeyword Is Nothing AndAlso lstEmptyKeyword.Count > 0 Then
                Dim result As String = String.Empty
                Dim arrKeyword() As String = keyword.Split(" ")
                If (arrKeyword.Length > 0) Then
                    For Each s As String In arrKeyword
                        Dim singleKeyword As String = s
                        If Not String.IsNullOrEmpty(s) Then
                            For Each objEmptyKeyword As KeywordIgnoreRow In lstEmptyKeyword
                                If (objEmptyKeyword.KeywordName.Trim().ToLower() = s.Trim().ToLower()) Then
                                    s = String.Empty
                                    Exit For
                                End If
                            Next
                        End If
                        If String.IsNullOrEmpty(s) Then
                            Continue For
                        End If
                        result = result & s & " "
                    Next
                    Return result.Trim()
                End If
            End If
        End If
        Return keyword
    End Function
    Private Function ConvertToSingular(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If
        If (keyword.Contains(" ")) Then
            Dim result As String = String.Empty
            Dim arrKeyword() As String = keyword.Split(" ")
            If (arrKeyword.Length > 0) Then
                For Each s As String In arrKeyword
                    If String.IsNullOrEmpty(s) Then
                        Continue For
                    End If
                    Dim strSingularize As String = s.Humanize.Singularize(True)
                    If Not String.IsNullOrEmpty(strSingularize) Then
                        s = strSingularize.ToLower()
                    End If
                    result = result & s & " "
                Next
                Return result.Trim()
            End If

        End If
        Return keyword
    End Function
    Private Function ConvertToSingularV1(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If
        If Not (keyword.Contains(" ")) Then
            keyword = keyword & " "
        End If
        Dim result As String = String.Empty
        Dim arrKeyword() As String = keyword.Split(" ")
        If (arrKeyword.Length > 0) Then
            For Each s As String In arrKeyword
                If String.IsNullOrEmpty(s) Then
                    Continue For
                End If
                Dim strSingularize As String = s.Humanize.Singularize(True)
                If Not String.IsNullOrEmpty(strSingularize) Then
                    s = strSingularize.ToLower()
                End If
                result = result & s & " "
            Next
            Return result.Trim()
        End If

        Return keyword
    End Function

    Private Function ConvertToSingularizePluralize(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If

        If keyword.Contains(" ") Then
            Return keyword
        End If

        Dim result As String = String.Empty
        Dim strSingularize As String = keyword.Humanize.Singularize(True)
        If String.IsNullOrEmpty(strSingularize) Then
            result = keyword & " "
        Else
            result = strSingularize.ToLower() & " "
        End If

        Dim strPluralize As String = keyword.Humanize.Pluralize(True)
        If String.IsNullOrEmpty(strPluralize) Then
            result &= keyword
        Else
            result &= strPluralize.ToLower()
        End If

        'Dim arrKeyword() As String = keyword.Split(" ")
        'If (arrKeyword.Length > 0) Then
        '    For Each s As String In arrKeyword
        '        If String.IsNullOrEmpty(s) Then
        '            Continue For
        '        End If

        '        If Not String.IsNullOrEmpty(strSingularize) Then
        '            s = strSingularize.ToLower()
        '        End If
        '        result = result & s & " "
        '    Next
        '    Return result.Trim()
        'End If

        Return result.Trim()
    End Function

    Public Function SearchItemInLucene(ByVal textSearch As String, ByVal orderBy As String, ByVal orderExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal brandId As Integer, ByVal PriceDesc As String, ByVal Rating As String, ByVal loadMore As Boolean, ByVal currentFilterActive As String) As DataLayer.StoreItemCollection

        If pageIndex = 1 Then
            counter = 0
        End If
        Dim lstItem As New StoreItemCollection
        Dim temp As String
        temp = textSearch
        If temp.Contains("Search by keyword or item") Then
            temp = ""
        Else
            temp = RemoveKeywordIgnore(temp)
            temp = temp.ToLower() 'ConvertToSingularV1(temp)
            temp = ConvertToSingularizePluralize(temp.ToLower())

        End If

        Dim language As String = Utility.Common.GetSiteLanguage()
        temp = ReplaceChar(temp)

        If (Not String.IsNullOrEmpty(temp.Replace("*", "").Replace("?", ""))) Then
            Dim searcher As IndexSearcher = New IndexSearcher(_directory(1), False)
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)
            Dim isAllowSearchSingleKeywrodInDescription As Boolean = True
            If (temp.Contains(" ")) Then ''key word la cum tu
                isAllowSearchSingleKeywrodInDescription = False
            End If
            Dim totalQuery As BooleanQuery = Nothing
            If loadMore Then
                If HttpContext.Current.Session("querySearch") Is Nothing Then
                    totalQuery = MakeTotalSearchQuery(isAllowSearchSingleKeywrodInDescription, temp, searcher, analyzer, MakeListKeywordSearch(temp), False)
                Else
                    totalQuery = HttpContext.Current.Session("querySearch")
                End If
            Else
                totalQuery = MakeTotalSearchQuery(isAllowSearchSingleKeywrodInDescription, temp, searcher, analyzer, MakeListKeywordSearch(temp), False)
                HttpContext.Current.Session("querySearch") = totalQuery
            End If


            If (totalQuery Is Nothing) Then
                Return Nothing
            End If
            Dim lstBrandId As String = ","
            Dim lstCountBrand As String = ","
            Dim bqFilter As New BooleanQuery
            Dim bqFilterExcluseBrand As New BooleanQuery
            Dim bqFilterExclusePrice As New BooleanQuery
            Dim bqFilterExcluseRating As New BooleanQuery
            ''filter with BrandId
            Dim filter As Filter
            Dim hasFilterBrand As Boolean = False
            Dim hasFilterPrice As Boolean = False
            Dim hasFilterRating As Boolean = False
            If (brandId > 0) Then
                Dim queryBrand As Query
                Dim parserBrand As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"BrandId"}, analyzer)
                queryBrand = parseQuery(brandId & "$", parserBrand)
                bqFilter.Add(queryBrand, BooleanClause.Occur.MUST)
                bqFilterExclusePrice.Add(queryBrand, BooleanClause.Occur.MUST)
                bqFilterExcluseRating.Add(queryBrand, BooleanClause.Occur.MUST)
                hasFilterBrand = True
            End If

            ''filter with Price
            If (Not PriceDesc Is Nothing And Not String.IsNullOrEmpty(PriceDesc)) Then
                Dim queryPrice As Query
                Dim min As Double = GetMinMaxValue(PriceDesc, False)
                Dim max As Double = GetMinMaxValue(PriceDesc, True)
                Dim p As Boolean = False
                If (max = 0) Then
                    max = Double.MaxValue
                    p = True
                End If
                queryPrice = NumericRangeQuery.NewDoubleRange("price-filter", min, max, True, p)
                bqFilter.Add(queryPrice, BooleanClause.Occur.MUST)
                bqFilterExcluseBrand.Add(queryPrice, BooleanClause.Occur.MUST)
                bqFilterExcluseRating.Add(queryPrice, BooleanClause.Occur.MUST)
                hasFilterPrice = True

            End If

            ''filter with rating
            If (Not Rating Is Nothing And Not String.IsNullOrEmpty(Rating)) Then
                Dim queryRating As Query
                Dim min As Double = GetMinMaxValue(Rating, False)
                Dim max As Double = GetMinMaxValue(Rating, True)
                Dim r As Boolean = False
                If (max = 0) Then
                    max = 5
                    r = True
                End If
                queryRating = NumericRangeQuery.NewDoubleRange("top-rated-filter", min, max, True, r)
                bqFilter.Add(queryRating, BooleanClause.Occur.MUST)
                bqFilterExcluseBrand.Add(queryRating, BooleanClause.Occur.MUST)
                bqFilterExclusePrice.Add(queryRating, BooleanClause.Occur.MUST)
                hasFilterRating = True

            End If

            If (hasFilterBrand Or hasFilterPrice Or hasFilterRating) Then
                filter = New QueryFilter(bqFilter)

            End If
            Dim hits As Hits = Nothing

            If orderBy Is Nothing Then
                hits = searcher.Search(totalQuery, filter, Sort.RELEVANCE)

            Else
                'Dim sort As New Sort(New SortField(orderBy, LuceneTypeSort(orderBy), LuceneTypeSortExp(orderBy)))
                Dim sort As New Sort(TypeSort(orderBy))
                hits = searcher.Search(totalQuery, filter, sort)

            End If

            Dim beginRow As Integer = (pageIndex - 1) * pageSize
            Dim endRow As Integer = beginRow + pageSize - 1
            Dim bId As Integer
            Dim hasNew As Boolean = False
            Dim hasBestSale As Boolean = False
            Dim hasHots As Boolean = False
            Dim ishot As Boolean = False
            Dim isNew As Boolean = False
            Dim isBestSeller As Boolean = False
            Dim hasToprate As Boolean = False
            Dim mostTopRate As Integer = 0
            total = hits.Length
            If (total > 0) Then
                If (beginRow >= total) Then
                    beginRow = total - 1

                End If
                If (endRow >= total) Then
                    endRow = total - 1

                End If
            End If

            If loadMore Then
                Dim memberID As Integer = Utility.Common.GetCurrentMemberId()
                Dim orderID As Integer = Utility.Common.GetCurrentOrderId()
                For i As Integer = beginRow To endRow
                    Dim doc As Document = hits.Doc(i)
                    counter += 1
                    Dim item As New StoreItemRow
                    item.SKU = doc.Get("SKU")
                    item.URLCode = doc.Get("UrlCode")
                    item.ShortDesc = doc.Get("Short" & language)
                    item.ItemName = doc.Get("ItemName") '' & " | " & hits.Score(i)
                    item.LastDepartmentName = doc.Get("LastDepartmentName")
                    item.Image = doc.Get("Image")
                    item.BrandId = doc.Get("BrandId")
                    item.LowSalePrice = doc.Get("price")
                    item.ShowPrice = doc.Get("PriceDesc")
                    item.IsNew = isNew
                    item.IsHot = ishot
                    item.IsBestSeller = isBestSeller
                    item.CountReview = doc.Get("CountReview")
                    item.AverageReview = doc.Get("AverageReview")
                    item.QtyOnHand = doc.Get("QtyOnHand")
                    item.IsSpecialOrder = CBool(doc.Get("IsSpecialOrder"))
                    item.AcceptingOrder = CInt(doc.Get("AcceptingOrder"))
                    StoreItemRow.GetItemSearchInfor(item, memberID, orderID)
                    item.itemIndex = counter
                    lstItem.Add(item)
                Next
            Else
                Dim i As Integer = 0
                While (i < hits.Length)
                    Dim doc As Document = hits.Doc(i)
                    bId = doc.Get("BrandId")
                    mostTopRate = Convert.ToInt32(doc.Get("most-popular-review"))
                    If (doc.Get("hot-items") = "1") Then
                        ishot = True
                    Else
                        ishot = False
                    End If
                    If (doc.Get("new-items") = "1") Then
                        isNew = True
                    Else
                        isNew = False
                    End If
                    If (doc.Get("best-sellers") = "1") Then
                        isBestSeller = True
                    Else
                        isBestSeller = False
                    End If
                    If mostTopRate > 0 And hasToprate = False Then
                        hasToprate = True
                    End If
                    If (i = 0) Then
                        counter += 1
                        Dim item As New StoreItemRow
                        item.ItemName = doc.Get("ItemName")
                        item.SKU = doc.Get("SKU")
                        item.URLCode = doc.Get("UrlCode")
                        item.ItemId = doc.Get("ItemId")
                        If Not lstBrandId.Contains("," & bId & ",") Then
                            lstBrandId = lstBrandId & bId & ","
                            'lstCountBrand = lstCountBrand & bId & ":" & FacetedSearchBrand(searcher.GetIndexReader(), bId, bq) & ","
                        End If
                        lstItem.Add(item)
                    Else
                        'If loadMore Then
                        '    Exit While
                        'End If
                        If Not lstBrandId.Contains("," & bId & ",") Then
                            lstBrandId = lstBrandId & bId & ","
                            'lstCountBrand = lstCountBrand & bId & ":" & FacetedSearchBrand(searcher.GetIndexReader(), bId, bq) & ","
                        End If
                    End If
                    i = i + 1
                    If isNew Then
                        hasNew = True
                    End If
                    If ishot Then
                        hasHots = True
                    End If
                    If isBestSeller Then
                        hasBestSale = True
                    End If
                End While
                If (hits.Length > 0) Then
                    If HttpContext.Current.Session("searchBrandId") Is Nothing Then
                        'HttpContext.Current.Session("searchBrandId") = lstBrandId
                        hits = searcher.Search(totalQuery, Nothing, Sort.RELEVANCE)
                        While (i < hits.Length)
                            Dim doc As Document = hits.Doc(i)
                            bId = doc.Get("BrandId")
                                If Not lstBrandId.Contains("," & bId & ",") Then
                                    lstBrandId = lstBrandId & bId & ","
                            End If
                            i = i + 1
                        End While
                        HttpContext.Current.Session("searchBrandId") = lstBrandId
                    End If
                    If String.IsNullOrEmpty(currentFilterActive) Then
                        HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), HttpContext.Current.Session("searchBrandId"), totalQuery, bqFilter, bqFilterExcluseBrand, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountRating") = FacetedSearchRating(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountPrice") = FacetedSearchPrice(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                    ElseIf currentFilterActive.ToLower().Equals("brandid") Then
                        HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), HttpContext.Current.Session("searchBrandId"), totalQuery, bqFilter, bqFilterExcluseBrand, hasFilterBrand, hasFilterPrice, hasFilterRating, True)
                        HttpContext.Current.Session("searchCountRating") = FacetedSearchRating(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountPrice") = FacetedSearchPrice(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                    ElseIf currentFilterActive.Equals("price") Then
                        '' HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), lstBrandId, totalQuery, bqFilter, bqFilterExcluseBrand, hasFilter, False)
                        HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), HttpContext.Current.Session("searchBrandId"), totalQuery, bqFilter, bqFilterExcluseBrand, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountRating") = FacetedSearchRating(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountPrice") = FacetedSearchPrice(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, True)
                    ElseIf currentFilterActive.Equals("rating") Then
                        ''HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), lstBrandId, totalQuery, bqFilter, bqFilterExcluseBrand, hasFilter, False)
                        HttpContext.Current.Session("searchCountBrand") = FacetedSearchBrand(searcher.GetIndexReader(), HttpContext.Current.Session("searchBrandId"), totalQuery, bqFilter, bqFilterExcluseBrand, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                        HttpContext.Current.Session("searchCountRating") = FacetedSearchRating(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, True)
                        HttpContext.Current.Session("searchCountPrice") = FacetedSearchPrice(searcher.GetIndexReader(), totalQuery, bqFilter, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, False)
                    End If
                End If

                HttpContext.Current.Session("searchHasToprate") = hasToprate
                ''  HttpContext.Current.Session("searchCountPrice") = Nothing
            End If
            analyzer.Close()
            searcher.Close()
        End If
        Return lstItem
    End Function



    '''''''''''''''''''''''''''''''''''''''
    ''''''''SEARCH KEYWORD LUCENE''''''''''
    '''''''''''''''''''''''''''''''''''''''
    '''main search method
    Private Function _SearchKeywordLucene(ByVal textSearch As String) As String
        Dim searchQuery As String
        Dim sArray1, sArray2, sConn As String
        sConn = ""

        searchQuery = textSearch

        If searchQuery.Contains("Search by keyword or item") Then
            searchQuery = ""
        End If

        searchQuery = ForSearchLucene(searchQuery)


        sArray1 = "new Array("
        sArray2 = "new Array("

        If (Utility.ConfigData.ActiveSearchKeyword) Then
            ' validation
            If (Not String.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) Then
                ' set up lucene searcher
                Dim searcher As IndexSearcher = New IndexSearcher(_directory(3), False)
                Dim hits_limit As Integer = Utility.ConfigData.NumberOfDisplayKeyword
                Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)

                Dim Parser As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"KeywordName"}, analyzer)
                Parser.SetAllowLeadingWildcard(True)
                Dim query As Query = parseQuery(searchQuery, Parser)

                Dim hits = searcher.Search(query, Nothing, hits_limit, New Sort("TotalPoint", True)).scoreDocs

                For Each item As ScoreDoc In hits
                    Dim doc As Document = searcher.Doc(item.doc)
                    sArray1 &= sConn & Escape(doc.Get("KeywordName"))
                    sArray2 &= sConn & Escape(HightLightSearchKeyword(doc.Get("KeywordName"), textSearch))
                    sConn = ","

                Next

                analyzer.Close()
                searcher.Close()

            End If

        End If
        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Dim result As String = sArray1 & "," & sArray2
        Return result
    End Function


    Public Function HightLightSearchSKU(ByVal SourceSearch As String, ByVal CharSearch As String) As String
        Dim result As String = String.Empty
        Dim sku As String = "#"
        Try
            Dim temp As String = SourceSearch.ToLower()
            Dim tempSource As String = SourceSearch
            Dim charbegin As Integer = temp.IndexOf(CharSearch.ToLower())
            While (charbegin <> -1)
                'sku = "<span class='highlightSKU'>#</span>"
                result += tempSource.Substring(0, charbegin)
                result += "<span class='highlightSKU'>" + tempSource.Substring(charbegin, CharSearch.Length()) + "</span>"
                tempSource = tempSource.Substring(charbegin + CharSearch.Length())
                temp = temp.Substring(charbegin + CharSearch.Length())
                charbegin = temp.IndexOf(CharSearch.ToLower())
            End While
            result += tempSource.Substring(0)
        Catch ex As Exception

        End Try
        Return sku & result
    End Function
    Public Function HightLightSearchKeyword(ByVal SourceSearch As String, ByVal CharSearch As String) As String
        If String.IsNullOrEmpty(SourceSearch) Then
            Return String.Empty
        End If
        SourceSearch = SourceSearch.Replace("'", "")
        Dim result As String = String.Empty
        Try
            Dim charTemp As String() = CharSearch.Trim().Split(" ")

            For i As Integer = 0 To charTemp.Length - 1
                For j As Integer = i + 1 To charTemp.Length - 1
                    If charTemp(i).Length < charTemp(j).Length Then
                        Dim tmp As String = charTemp(i)
                        charTemp(i) = charTemp(j)
                        charTemp(j) = tmp
                    End If
                Next
            Next

            Dim sourcesplit As String() = SourceSearch.Trim().Split(" ")
            For i As Integer = 0 To sourcesplit.Length - 1
                Dim word As String = sourcesplit(i)
                If (word.Contains("<span") Or word.Contains("class='highlightKeyword'")) Then
                    Continue For
                End If
                For Each item As String In charTemp
                    If Not String.IsNullOrEmpty(item) Then
                        Dim temp As String = word.ToLower()
                        Dim tempSource As String = sourcesplit(i)
                        If (tempSource.Contains("<span") Or tempSource.Contains("class='highlightKeyword'")) Then
                            Continue For
                        End If
                        Dim charbegin As Integer = temp.IndexOf(item.ToLower())
                        Dim tmp As String = String.Empty
                        While (charbegin <> -1)
                            tmp += tempSource.Substring(0, charbegin)
                            tmp += "<span class='highlightKeyword'>" + tempSource.Substring(charbegin, item.Length()) + "</span>"

                            tempSource = tempSource.Substring(charbegin + item.Length())
                            temp = temp.Substring(charbegin + item.Length())
                            charbegin = temp.IndexOf(item.ToLower())
                        End While
                        tmp += tempSource.Substring(0)

                        sourcesplit(i) = tmp
                    End If
                Next
            Next
            result = String.Join(" ", sourcesplit)
        Catch ex As Exception

        End Try
        Return result
    End Function

    Public Function HightLightSearchKeyword(ByVal SourceSearch As String, ByVal lstCharSearch As KeywordCollection) As String
        Dim result As String = SourceSearch
        If lstCharSearch Is Nothing Then
            Return String.Empty
        End If
        For Each strCharSearch As KeywordRow In lstCharSearch
            result = HightLightSearchKeyword(result, strCharSearch.KeywordName)
        Next
        Return result
    End Function

    Private Function parseQuery(ByVal searchQuery As String, ByVal parser As QueryParser) As Query
        Dim query As Query
        Try
            query = parser.Parse(searchQuery.Trim())
        Catch ex As ParseException
            query = parser.Parse(QueryParser.Escape(searchQuery.Trim()))
        End Try
        Return query
    End Function
    Private Function GetLikeQueryExpression(ByVal str As String) As String
        Dim search As String() = str.Trim().Split(" ")
        For i As Integer = 0 To search.Length() - 1
            If (Not String.IsNullOrEmpty(search(i))) Then
                search(i) = search(i).Trim() + "*"
            End If
        Next
        str = String.Join(" ", search)
        Return str
    End Function
    Private Function ForSearchLucene(ByVal str As String) As String
        Dim search As String() = str.Trim().Split(" ")
        For i As Integer = 0 To search.Length() - 1
            If (Not String.IsNullOrEmpty(search(i))) Then
                search(i) = "*" + search(i).Trim() + "*"
            End If
        Next
        str = String.Join(" ", search)
        Return str
    End Function

    Private Function Escape(ByVal s As String) As String
        Dim t As String

        t = Replace(s, "'", "\'")
        t = Replace(t, vbCrLf, "<br/>")
        t = Trim(t)

        Return "'" & t & "'"
    End Function

    Private Function ReplaceChar(ByVal src As String) As String
        If Not String.IsNullOrEmpty(src) Then
            Dim result As String
            result = Replace(src, " -", " ")
            result = Replace(result, "- ", " ")
            While (result.Contains("."))
                result = Replace(result, ".", " ")
            End While
            While (result.Contains("  "))
                result = Replace(result, "  ", " ")
            End While
            result = Trim(result)
            Return result
        Else
            Return String.Empty
        End If
    End Function
    '''''''''''''''''''''''''
    '''did you mean lucene'''
    ''' '''''''''''''''''''''
    Public Function SuggestSimilar(ByVal term As String) As String
        Dim result As String = String.Empty
        If Not String.IsNullOrEmpty(term) Then
            Dim spellPath As String = Utility.ConfigData.LuceneSpellCheckIndexPath()
            ' create the spell checker
            Dim spell = New SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.GetDirectory(spellPath, False))

            ' get 2 similar words
            Dim similarWords As String() = spell.SuggestSimilar(term, 1)

            ' show the similar words
            For wordIndex As Integer = 0 To similarWords.Length - 1
                'Console.WriteLine(similarWords(wordIndex) + " is similar to " + term)
                result = similarWords(wordIndex)
            Next
        End If
        Return result
    End Function




    Public Function FacetedSearchBrand(ByVal indexReader As IndexReader, ByVal lstBrandId As String, ByVal bq As BooleanQuery, ByVal bqFilterTotal As BooleanQuery, ByVal bqFilterExcluseBrand As BooleanQuery, ByVal hasFilterBrand As Boolean, ByVal hasFilterPrice As Boolean, ByVal hasFilterRating As Boolean, ByVal isBrandActive As Boolean) As String
        Dim result As String = ","

        Dim lst As String() = lstBrandId.Trim().Split(",")
        For Each brandId As String In lst
            If (Not String.IsNullOrEmpty(brandId)) Then
                Dim searchQueryFilter = New QueryFilter(bq)
                Dim searchBitArray As BitArray = searchQueryFilter.Bits(indexReader)

                Dim brandQuery = New TermQuery(New Term("BrandId", brandId))
                Dim brandQueryFilter = New QueryFilter(brandQuery)
                Dim brandBitArray As BitArray = brandQueryFilter.Bits(indexReader)

                Dim combinedResults As BitArray
                If String.IsNullOrEmpty(bqFilterExcluseBrand.ToString()) AndAlso isBrandActive Then
                    combinedResults = searchBitArray.[And](brandBitArray)
                Else
                    If (hasFilterRating Or hasFilterPrice) Then
                        Dim fltQueryFilter = New QueryFilter(bqFilterExcluseBrand)
                        Dim fltBitArray As BitArray = fltQueryFilter.Bits(indexReader)
                        combinedResults = searchBitArray.[And](fltBitArray).[And](brandBitArray)
                    Else
                        combinedResults = searchBitArray.[And](brandBitArray)
                    End If
                End If
                result += brandId & ":" & GetCardinality(combinedResults) & ","
            End If
        Next
        Return result
    End Function
    Private Function CountFacetedSearchRating(ByVal min As Double, ByVal max As Double, ByVal indexReader As IndexReader, ByVal bq As BooleanQuery, ByVal bqFilterTotal As BooleanQuery, ByVal bqFilterExcluseRating As BooleanQuery, ByVal hasFilterBrand As Boolean, ByVal hasFilterPrice As Boolean, ByVal hasFilterRating As Boolean, ByVal isRatingActive As Boolean, ByVal maxInclusive As Boolean) As Integer
        Dim searchQueryFilter = New QueryFilter(bq)
        Dim searchBitArray As BitArray = searchQueryFilter.Bits(indexReader)

        Dim queryRating As Query = NumericRangeQuery.NewDoubleRange("top-rated-filter", min, max, True, maxInclusive)
        Dim ratingQueryFilter = New QueryFilter(queryRating)
        Dim ratingBitArray As BitArray = ratingQueryFilter.Bits(indexReader)
        Dim combinedResults As BitArray
        If String.IsNullOrEmpty(bqFilterExcluseRating.ToString()) And isRatingActive Then
            combinedResults = searchBitArray.[And](ratingBitArray)
        Else

            If (hasFilterBrand Or hasFilterPrice) Then
                Dim fltQueryFilter = New QueryFilter(bqFilterExcluseRating)
                Dim fltBitArray As BitArray = fltQueryFilter.Bits(indexReader)
                combinedResults = searchBitArray.[And](fltBitArray).[And](ratingBitArray)
            Else
                combinedResults = searchBitArray.[And](ratingBitArray)
            End If
        End If
        Dim count As Integer = GetCardinality(combinedResults)
        If (count > 0) Then
            Return count
        End If
        Return 0
    End Function
    Private Function CountFacetedSearchPrice(ByVal min As Double, ByVal max As Double, ByVal indexReader As IndexReader, ByVal bq As BooleanQuery, ByVal bqFilterTotal As BooleanQuery, ByVal bqFilterExclusePrice As BooleanQuery, ByVal hasFilterBrand As Boolean, ByVal hasFilterPrice As Boolean, ByVal hasFilterRating As Boolean, ByVal isPriceActive As Boolean, ByVal maxInclusive As Boolean) As Integer
        Dim searchQueryFilter = New QueryFilter(bq)
        Dim searchBitArray As BitArray = searchQueryFilter.Bits(indexReader)

        Dim queryPrice As Query = NumericRangeQuery.NewDoubleRange("price-filter", min, max, True, maxInclusive)
        Dim priceQueryFilter = New QueryFilter(queryPrice)
        Dim priceBitArray As BitArray = priceQueryFilter.Bits(indexReader)
        Dim combinedResults As BitArray
        If String.IsNullOrEmpty(bqFilterExclusePrice.ToString()) And isPriceActive Then
            combinedResults = searchBitArray.[And](priceBitArray)
        Else
            If (hasFilterBrand Or hasFilterRating) Then
                Dim fltQueryFilter = New QueryFilter(bqFilterExclusePrice)

                Dim fltBitArray As BitArray = fltQueryFilter.Bits(indexReader)
                combinedResults = searchBitArray.[And](fltBitArray).[And](priceBitArray)
            Else
                combinedResults = searchBitArray.[And](priceBitArray)
            End If
        End If

        Dim count As Integer = GetCardinality(combinedResults)
        Return count
    End Function
    Public Function FacetedSearchPrice(ByVal indexReader As IndexReader, ByVal bq As BooleanQuery, ByVal bqFilterTotal As BooleanQuery, ByVal bqFilterExclusePrice As BooleanQuery, ByVal hasFilterBrand As Boolean, ByVal hasFilterPrice As Boolean, ByVal hasFilterRating As Boolean, ByVal isPriceActive As Boolean) As String
        Dim result As String = ";"
        Dim tmp As String = ""
        Dim lstLevel As New List(Of FacetedSearchLevel)
        If HttpContext.Current.Session("FacetedSearchPriceLevel") Is Nothing Then
            Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Price))
            Dim len As Integer = arr.Length - 1
            For i As Integer = 0 To len

                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                Dim p As Boolean = False
                If (i = 0) Then
                    max = CType(arr(i + 1), Double)
                    tmp = "," & max
                    min = Double.MinValue
                ElseIf (i = len) Then
                    min = CType(arr(i), Double)
                    max = Double.MaxValue
                    tmp = min & ","
                    p = True
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    tmp = min & "," & max
                End If
                Dim count As Integer = CountFacetedSearchPrice(min, max, indexReader, bq, bqFilterTotal, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, isPriceActive, p)
                If (count > 0) Then
                    result += "(" & tmp & "):" & count & ";"
                    Dim objFacetedSearchLevel As New FacetedSearchLevel()
                    objFacetedSearchLevel.min = min
                    objFacetedSearchLevel.max = max
                    objFacetedSearchLevel.maxInclusive = p
                    lstLevel.Add(objFacetedSearchLevel)
                End If
            Next
            '' Return result
            If Not lstLevel Is Nothing AndAlso lstLevel.Count > 0 Then
                HttpContext.Current.Session("FacetedSearchPriceLevel") = lstLevel
            End If
        Else
            lstLevel = HttpContext.Current.Session("FacetedSearchPriceLevel")
            For Each objLevel As FacetedSearchLevel In lstLevel
                If objLevel Is Nothing Then
                    Continue For
                End If
                Dim count As Integer = CountFacetedSearchPrice(objLevel.min, objLevel.max, indexReader, bq, bqFilterTotal, bqFilterExclusePrice, hasFilterBrand, hasFilterPrice, hasFilterRating, isPriceActive, objLevel.maxInclusive)
                If (objLevel.max = Double.MaxValue) Then
                    tmp = objLevel.min & ","
                ElseIf (objLevel.min = Double.MinValue) Then
                    tmp = "," & objLevel.max
                Else
                    tmp = objLevel.min & "," & objLevel.max
                End If
                result += "(" & tmp & "):" & count & ";"
            Next
        End If
        Return result
    End Function
    Public Class FacetedSearchLevel
        Public min As Double = 0
        Public max As Double = 0
        Public maxInclusive As Boolean = False
    End Class
    Public Function FacetedSearchRating(ByVal indexReader As IndexReader, ByVal bq As BooleanQuery, ByVal bqFilterTotal As BooleanQuery, ByVal bqFilterExcluseRating As BooleanQuery, ByVal hasFilterBrand As Boolean, ByVal hasFilterPrice As Boolean, ByVal hasFilterRating As Boolean, ByVal isRatingActive As Boolean) As String
        Dim lstLevel As New List(Of FacetedSearchLevel)
        Dim tmp As String = ""
        Dim result As String = ";"
        If HttpContext.Current.Session("FacetedSearchRatingLevel") Is Nothing Then
            Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Rating))
            Dim len As Integer = arr.Length - 1
            For i As Integer = 0 To len

                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                Dim maxInclusive As Boolean = False
                If (i = len) Then
                    min = CType(arr(i), Double)
                    tmp = min & ","
                    maxInclusive = True
                    max = Double.MaxValue
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    tmp = min & "," & max
                End If

                Dim count As Integer = CountFacetedSearchRating(min, max, indexReader, bq, bqFilterTotal, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, isRatingActive, maxInclusive)
                If (count > 0) Then
                    result += "(" & tmp & "):" & count & ";"
                    Dim objFacetedSearchLevel As New FacetedSearchLevel()
                    objFacetedSearchLevel.min = min
                    objFacetedSearchLevel.max = max
                    objFacetedSearchLevel.maxInclusive = maxInclusive
                    lstLevel.Add(objFacetedSearchLevel)
                End If

            Next
            If Not lstLevel Is Nothing AndAlso lstLevel.Count > 0 Then
                HttpContext.Current.Session("FacetedSearchRatingLevel") = lstLevel
            End If
            ''HttpContext.Current.Session("TestFacetedSearchRatingLevelEnum") = result
            Return result
        Else
            lstLevel = HttpContext.Current.Session("FacetedSearchRatingLevel")
            For Each objLevel As FacetedSearchLevel In lstLevel
                If objLevel Is Nothing Then
                    Continue For
                End If
                Dim count As Integer = CountFacetedSearchRating(objLevel.min, objLevel.max, indexReader, bq, bqFilterTotal, bqFilterExcluseRating, hasFilterBrand, hasFilterPrice, hasFilterRating, isRatingActive, objLevel.maxInclusive)
                ''Dim count As Integer = CountFacetedSearchRating(min, max, indexReader, bq, bqFilterTotal, bqFilterExcluseRating, hasFilter, isRatingActive, maxInclusive)
                If (objLevel.max = Double.MaxValue) Then
                    tmp = objLevel.min & ","
                Else
                    tmp = objLevel.min & "," & objLevel.max
                End If
                result += "(" & tmp & "):" & count & ";"
            Next
            ''HttpContext.Current.Session("TestFacetedSearchRatingLevelSession") = result
        End If
        Return result
    End Function

    Public Function GetCardinality(ByVal bitArray As BitArray) As Integer
        Dim _bitsSetArray256 = New Byte() {0, 1, 1, 2, 1, 2, _
         2, 3, 1, 2, 2, 3, _
         2, 3, 3, 4, 1, 2, _
         2, 3, 2, 3, 3, 4, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 1, 2, 2, 3, _
         2, 3, 3, 4, 2, 3, _
         3, 4, 3, 4, 4, 5, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 3, 4, 4, 5, _
         4, 5, 5, 6, 1, 2, _
         2, 3, 2, 3, 3, 4, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 2, 3, 3, 4, _
         3, 4, 4, 5, 3, 4, _
         4, 5, 4, 5, 5, 6, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 3, 4, 4, 5, _
         4, 5, 5, 6, 3, 4, _
         4, 5, 4, 5, 5, 6, _
         4, 5, 5, 6, 5, 6, _
         6, 7, 1, 2, 2, 3, _
         2, 3, 3, 4, 2, 3, _
         3, 4, 3, 4, 4, 5, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 3, 4, 4, 5, _
         4, 5, 5, 6, 2, 3, _
         3, 4, 3, 4, 4, 5, _
         3, 4, 4, 5, 4, 5, _
         5, 6, 3, 4, 4, 5, _
         4, 5, 5, 6, 4, 5, _
         5, 6, 5, 6, 6, 7, _
         2, 3, 3, 4, 3, 4, _
         4, 5, 3, 4, 4, 5, _
         4, 5, 5, 6, 3, 4, _
         4, 5, 4, 5, 5, 6, _
         4, 5, 5, 6, 5, 6, _
         6, 7, 3, 4, 4, 5, _
         4, 5, 5, 6, 4, 5, _
         5, 6, 5, 6, 6, 7, _
         4, 5, 5, 6, 5, 6, _
         6, 7, 5, 6, 6, 7, _
         6, 7, 7, 8}
        Dim array = DirectCast(bitArray.[GetType]().GetField("m_array", BindingFlags.NonPublic Or BindingFlags.Instance).GetValue(bitArray), UInteger())
        Dim count As Integer = 0

        For index As Integer = 0 To array.Length - 1
            count += _bitsSetArray256(array(index) And &HFF) + _bitsSetArray256((array(index) >> 8) And &HFF) + _bitsSetArray256((array(index) >> 16) And &HFF) + _bitsSetArray256((array(index) >> 24) And &HFF)
        Next

        Return count
    End Function


    Public Function SearchArticleInLucene(ByVal textSearch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal Load As Boolean) As DataLayer.RelatedArticleCollection
        Dim lstRelatedArticle As New RelatedArticleCollection
        Dim searchQuery, searchQuerymatch, temp As String
        temp = textSearch
        If temp.Contains("Search by keyword or item") Then
            temp = ""
        Else
            temp = RemoveKeywordIgnore(temp) ''remove KeywordIgnore
        End If
        searchQuery = ForSearchLucene(temp)
        searchQuerymatch = GetLikeQueryExpression(temp)

        If (Not String.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) Then
            ' set up lucene searcher
            Dim searcher As IndexSearcher = New IndexSearcher(_directory(2), False)
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)

            Dim Parser1 As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"Title", "DescSearch"}, analyzer)


            'search same match
            Dim query1 As Query = parseQuery(temp & "*", Parser1)

            'search like each word
            Dim query2 As Query = parseQuery(searchQuerymatch, Parser1)

            'search like each word both front and behind
            Parser1.SetAllowLeadingWildcard(True)

            Dim query3 As Query = parseQuery(searchQuery, Parser1)

            Dim bq As New BooleanQuery
            bq.Add(query1, BooleanClause.Occur.SHOULD)
            bq.Add(query2, BooleanClause.Occur.SHOULD)
            bq.Add(query3, BooleanClause.Occur.SHOULD)
            Dim hits As Hits = Nothing
            hits = searcher.Search(bq, Sort.RELEVANCE)
            total = hits.Length

            If Load Then
                Dim beginRow As Integer = (pageIndex - 1) * pageSize
                Dim endRow As Integer = beginRow + pageSize - 1
                If (beginRow >= total) Then
                    beginRow = total - 1

                End If
                If (endRow >= total) Then
                    endRow = total - 1

                End If
                For i As Integer = beginRow To endRow
                    Dim doc As Document = hits.Doc(i)
                    Dim item As New RelatedArticleRow
                    item.Id = doc.Get("Id")
                    item.CategoryId = doc.Get("CategoryId")
                    item.Title = doc.Get("Title")
                    item.Description = doc.Get("Description")
                    item.ShortDescription = doc.Get("ShortDescription")
                    item.Type = doc.Get("Type")
                    lstRelatedArticle.Add(item)
                Next
            End If
            analyzer.Close()
            searcher.Close()
        End If
        Return lstRelatedArticle
    End Function
    Public Function SearchArticleInLuceneByType(ByVal textSearch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal type As Integer, ByVal load As Boolean) As DataLayer.RelatedArticleCollection
        Dim lstRelatedArticle As New RelatedArticleCollection
        Dim searchQuery, searchQuerymatch, temp As String
        temp = textSearch
        If temp.Contains("Search by keyword or item") Then
            temp = ""
        Else
            temp = RemoveKeywordIgnore(temp)
        End If
        searchQuery = ForSearchLucene(temp)
        searchQuerymatch = GetLikeQueryExpression(temp)

        If (Not String.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) Then
            ' set up lucene searcher
            Dim searcher As IndexSearcher = New IndexSearcher(_directory(2), False)
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)

            Dim Parser1 As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"Title", "DescSearch"}, analyzer)


            'search same match
            Dim query1 As Query = parseQuery(temp & "*", Parser1)

            'search like each word
            Dim query2 As Query = parseQuery(searchQuerymatch, Parser1)

            'search like each word both front and behind
            Parser1.SetAllowLeadingWildcard(True)

            Dim query3 As Query = parseQuery(searchQuery, Parser1)

            Dim bq As New BooleanQuery
            bq.Add(query1, BooleanClause.Occur.SHOULD)
            bq.Add(query2, BooleanClause.Occur.SHOULD)
            bq.Add(query3, BooleanClause.Occur.SHOULD)

            ''filter with Type
            Dim bqFilter As New BooleanQuery
            Dim filter As Filter
            If (type > 0) Then
                Dim queryType As Query
                Dim parserBrand As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"Type"}, analyzer)
                queryType = parseQuery(type & "$", parserBrand)
                bqFilter.Add(queryType, BooleanClause.Occur.MUST)
                filter = New QueryFilter(bqFilter)
            End If

            Dim hits As Hits = Nothing
            hits = searcher.Search(bq, filter, Sort.RELEVANCE)
            total = hits.Length
            If (total > 0 AndAlso load = True) Then
                Dim beginRow As Integer = (pageIndex - 1) * pageSize
                Dim endRow As Integer = beginRow + pageSize - 1
                If (beginRow >= total) Then
                    beginRow = total - 1

                End If
                If (endRow >= total) Then
                    endRow = total - 1

                End If
                For i As Integer = beginRow To endRow
                    Dim doc As Document = hits.Doc(i)
                    Dim item As New RelatedArticleRow
                    item.Id = doc.Get("Id")
                    item.CategoryId = doc.Get("CategoryId")
                    item.Title = doc.Get("Title")
                    item.Description = doc.Get("Description")
                    item.ShortDescription = doc.Get("ShortDescription")
                    item.Type = doc.Get("Type")
                    lstRelatedArticle.Add(item)
                Next
            End If
            analyzer.Close()
            searcher.Close()
        End If
        Return lstRelatedArticle
    End Function

    Public Function SearchVideoInLucene(ByVal textSearch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal Load As Boolean) As DataLayer.VideoCollection
        Dim lstvideo As New VideoCollection
        Dim searchQuery, searchQuerymatch, temp As String
        temp = textSearch
        If temp.Contains("Search by keyword or item") Then
            temp = ""
        Else
            temp = RemoveKeywordIgnore(temp)
        End If
        searchQuery = ForSearchLucene(temp)
        searchQuerymatch = GetLikeQueryExpression(temp)

        If (Not String.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) Then
            ' set up lucene searcher
            Dim searcher As IndexSearcher = New IndexSearcher(_directory(4), False)
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)

            Dim Parser1 As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"Title", "ShortDescription"}, analyzer)


            'search same match
            Dim query1 As Query = parseQuery(temp & "*", Parser1)

            'search like each word
            Dim query2 As Query = parseQuery(searchQuerymatch, Parser1)

            'search like each word both front and behind
            Parser1.SetAllowLeadingWildcard(True)

            Dim query3 As Query = parseQuery(searchQuery, Parser1)

            Dim bq As New BooleanQuery
            bq.Add(query1, BooleanClause.Occur.SHOULD)
            bq.Add(query2, BooleanClause.Occur.SHOULD)
            bq.Add(query3, BooleanClause.Occur.SHOULD)

            Dim hits As Hits = Nothing
            hits = searcher.Search(bq, Sort.RELEVANCE)
            total = hits.Length
            If Load Then
                Dim beginRow As Integer = (pageIndex - 1) * pageSize
                Dim endRow As Integer = beginRow + pageSize - 1
                If (beginRow >= total) Then
                    beginRow = total - 1

                End If
                If (endRow >= total) Then
                    endRow = total - 1

                End If
                For i As Integer = beginRow To endRow

                    Dim doc As Document = hits.Doc(i)
                    Dim item As New VideoRow
                    item.VideoId = Convert.ToInt32(doc.Get("VideoId"))
                    item.ThumbImage = doc.Get("ThumbImage")
                    item.IsYoutubeImage = CBool(doc.Get("IsYoutubeImage"))
                    item.Title = doc.Get("Title")
                    item.ShortDescription = doc.Get("ShortDescription")
                    item.ViewsCount = Convert.ToInt32(doc.Get("ViewsCount"))
                    Dim strDate As String = doc.Get("CreatedDate").ToString()
                    item.CreatedDate = Convert.ToDateTime(strDate)
                    lstvideo.Add(item)
                Next
            End If
            analyzer.Close()
            searcher.Close()
        End If
        Return lstvideo
    End Function
#Region "Search item"

    ''www.ibm.com/developerworks/community/blogs/markashworth/entry/fuzzy_on_fuzzy_term_searches2?lang=en
    Public Function MakerQueryByKeyWord(ByVal isAllowSearchSingleKeywrodInDescription As Boolean, ByVal textSearch As String, ByVal searcher As IndexSearcher, ByVal analyzer As StandardAnalyzer, ByVal parserName As QueryParser, ByVal parserShortDesc As QueryParser, ByVal parserLongDesc As QueryParser, ByVal checkSingularPlural As Boolean, ByRef boot As Single, ByVal isSearchSuggest As Boolean) As BooleanQuery
        Dim lstItem As New StoreItemCollection
        Dim likeQueryExpression, temp As String
        temp = textSearch
        temp = ReplaceChar(temp)
        Dim strKWTotalPlural As String = String.Empty

        Dim language As String = Utility.Common.GetSiteLanguage()
        If (Not String.IsNullOrEmpty(temp.Replace("*", "").Replace("?", ""))) Then

            Dim luceneFuzzyFactor As Single = Utility.ConfigData.LuceneFuzzyFactor
            Dim arrKwSearch() As String = temp.Split(" ") ''temp.Replace("-", " ").Split(" ")
            Dim queryItemName(arrKwSearch.Length) As TermQuery
            Dim queryLongDesc(arrKwSearch.Length) As TermQuery
            Dim arrKwPlural(arrKwSearch.Length) As String
            Dim queryPhraseItemName As New PhraseQuery  ''search nguyen cum tu trong item name, SKU, URL Code
            Dim queryPhraseLongDesc As New PhraseQuery ''search nguyen cum tu trong long desc
            Dim index As Integer = 0

            For Each keyword As String In arrKwSearch
                If Not String.IsNullOrEmpty(keyword) Then
                    If (checkSingularPlural) Then
                        Dim pluralTemplete As String = keyword.Humanize.Pluralize(False).ToLower()
                        If (keyword.Equals(pluralTemplete)) Then
                            pluralTemplete = keyword.Humanize.Singularize(False).ToLower()
                        End If
                        arrKwPlural(index) = pluralTemplete
                        strKWTotalPlural = strKWTotalPlural & pluralTemplete.Trim() & " "
                    End If
                    queryPhraseItemName.Add(New Term("ItemName", keyword.ToLower()))
                    queryPhraseLongDesc.Add(New Term("LongDesc", keyword.ToLower()))
                    queryItemName(index) = New TermQuery(New Term("ItemName", keyword.ToLower()))
                    queryLongDesc(index) = New TermQuery(New Term("LongDesc", keyword.ToLower()))
                    index = index + 1
                End If
            Next
            likeQueryExpression = GetLikeQueryExpression(temp & " " & strKWTotalPlural)
            Dim bqEachWordItemName As New BooleanQuery
            Dim bqEachWordLongDesc As New BooleanQuery
            Dim bqLongDesc As New BooleanQuery
            Dim bqItemName As New BooleanQuery

            index = 0
            For Each tmp As TermQuery In queryItemName
                If Not tmp Is Nothing Then
                    bqItemName.Add(tmp, BooleanClause.Occur.MUST)
                    bqEachWordItemName.Add(tmp, BooleanClause.Occur.SHOULD)
                    If (checkSingularPlural) Then
                        Dim qTmpPlural As New TermQuery(New Term("ItemName", arrKwPlural(index)))
                        bqEachWordItemName.Add(qTmpPlural, BooleanClause.Occur.SHOULD)
                    End If
                End If
                index = index + 1
            Next
            index = 0
            For Each tmp As TermQuery In queryLongDesc
                If Not tmp Is Nothing Then
                    bqLongDesc.Add(tmp, BooleanClause.Occur.MUST)
                    bqEachWordLongDesc.Add(tmp, BooleanClause.Occur.SHOULD)
                    If (checkSingularPlural) Then
                        Dim qTmpPlural As New TermQuery(New Term("LongDesc", arrKwPlural(index)))
                        bqEachWordLongDesc.Add(qTmpPlural, BooleanClause.Occur.SHOULD)
                    End If
                End If
                index = index + 1
            Next

            'tach tung va search like vd : artisan* , nail*, luu y: khong search TH like '%%' vi se lam cham toc do va cung khong can thiet phai search TH nay
            Dim queryLikeName As Query = parseQuery(likeQueryExpression, parserName)
            Dim queryLikeLongDesc As Query = parseQuery(likeQueryExpression, parserLongDesc)

            Dim LuceneBoostFactor As Single = Utility.ConfigData.LuceneBoostFactor
            Dim totalQuery As Integer = 8 ''tong so cau query
            If Not (isSearchSuggest) Then
                totalQuery = totalQuery - 2 ''trong ket qua trg search khong search like
                If Not (isAllowSearchSingleKeywrodInDescription) Then '' TH tach tu khong cho search trogn description
                    totalQuery = totalQuery - 2 ''bo cau  4 va cau 6
                End If
            End If

            Dim bootQueryPhraseItemName As Single = 0
            Dim arrBootValue(totalQuery - 1) As Single
            arrBootValue(totalQuery - 1) = boot
            Dim i As Integer = totalQuery - 2
            Dim maxBoot As Single = Utility.ConfigData.MaxBoostValue
            While (i >= 0) ''tinh boots value cho moi cau, luu y la cau o level cao hon( sap tren dau) thi phai co boots lon hon 
                Dim prevBoot As Single = 0
                prevBoot = arrBootValue(i + 1)
                prevBoot = prevBoot * LuceneBoostFactor
                If (prevBoot > maxBoot) Then
                    prevBoot = maxBoot
                End If
                arrBootValue(i) = prevBoot
                i = i - 1
            End While
            i = 0
            boot = arrBootValue(i)

            ''''''''''''''''set boot theo thu tu cao den thap'''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''



            '1/ item name: nguyên cụm từ "artisan brush"
            SetBootSubQuery(queryPhraseItemName, i, arrBootValue)

            '2/ item name: tach tung tu trogn cum ra va search , bat buoc phai co tat ca cac tu  nhung khong nhat thiet phai dung nguyen cum    
            SetBootSubQuery(bqItemName, i, arrBootValue)

            '3/ full desc: nguyên cụm từ "artisan brush"
            SetBootSubQuery(queryPhraseLongDesc, i, arrBootValue)

            If (isAllowSearchSingleKeywrodInDescription) Then
                '4/ full desc: tach tung tu trogn cum ra va search , bat buoc phai co tat ca cac tu  nhung khong nhat thiet phai dung nguyen cum 
                SetBootSubQuery(bqLongDesc, i, arrBootValue)
            End If

            '5/ item name: tách từng từ va search  chinh xac
            SetBootSubQuery(bqEachWordItemName, i, arrBootValue)

            If (isAllowSearchSingleKeywrodInDescription) Then
                '6/ full desc: tách từng từ va search chinh xac
                SetBootSubQuery(bqEachWordLongDesc, i, arrBootValue)
            End If
            If (isSearchSuggest) Then

                '7/ item name: tách từng từ va search like "artisan*" hoặc "brush*"
                SetBootSubQuery(queryLikeName, i, arrBootValue)

                '8/ full desc: tách từng từ va search like "artisan*" hoặc "brush*"
                SetBootSubQuery(queryLikeLongDesc, i, arrBootValue)
            End If



            Dim bq As New BooleanQuery
            bq.Add(queryPhraseItemName, BooleanClause.Occur.SHOULD)
            bq.Add(bqItemName, BooleanClause.Occur.SHOULD)

            bq.Add(queryPhraseLongDesc, BooleanClause.Occur.SHOULD)
            If (isAllowSearchSingleKeywrodInDescription) Then
                bq.Add(bqLongDesc, BooleanClause.Occur.SHOULD)
            End If

            Dim kw As String = HttpContext.Current.Request.QueryString("kw")
            If Not String.IsNullOrEmpty(kw) AndAlso Not kw.Contains(" ") Then
                ' KHOA tách từng từ va search  chinh xac
                bq.Add(bqEachWordItemName, BooleanClause.Occur.SHOULD)
                bq.Add(bqEachWordLongDesc, BooleanClause.Occur.SHOULD)
                'Khoa sua lai chi cho chay voi search 1 tu
            End If


            If (isAllowSearchSingleKeywrodInDescription) Then
                bq.Add(bqEachWordLongDesc, BooleanClause.Occur.SHOULD)
            End If


            If (isSearchSuggest) Then
                bq.Add(queryLikeName, BooleanClause.Occur.SHOULD)
                bq.Add(queryLikeLongDesc, BooleanClause.Occur.SHOULD)
            End If



            Return bq
        End If
        Return Nothing
    End Function
    Private Sub SetBootSubQuery(ByRef query As Query, ByRef index As Integer, ByVal arrBootValue() As Single)
        ''Exit Sub
        query.SetBoost(arrBootValue(index))
        index = index + 1
    End Sub
    Public Function MakeListKeywordSearch(ByVal textSearch As String) As KeywordCollection
        Dim lstKeyword As KeywordCollection = KeywordSynonymRow.GetListSynonymSearch(textSearch)
        If (lstKeyword Is Nothing) Then
            lstKeyword = New KeywordCollection
        End If
        Dim objMainKeyword As New KeywordRow
        objMainKeyword.KeywordName = textSearch
        If (lstKeyword.Count < 1) Then
            lstKeyword.Add(objMainKeyword)
        Else
            lstKeyword.Insert(0, objMainKeyword)
        End If
        Return lstKeyword
    End Function
    Public Function MakeTotalSearchQuery(ByVal isAllowSearchSingleKeywrodInDescription As Boolean, ByVal textSearch As String, ByVal searcher As IndexSearcher, ByVal analyzer As StandardAnalyzer, ByVal lstKeywordRelated As KeywordCollection, ByVal isSuggest As Boolean) As BooleanQuery
        Dim ParserItemName As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"SKU", "ItemName", "UrlCode"}, analyzer)
        Dim ParserShortDesc As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"ShortDesc"}, analyzer)
        Dim ParserLongDesc As QueryParser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"LongDesc"}, analyzer)
        Dim totalQuery As New BooleanQuery
        Dim currentBoot As Single = Utility.ConfigData.DefaultLuceneBoost
        Dim LuceneBoostFactor As Single = Utility.ConfigData.LuceneBoostFactor
        Dim index As Integer = lstKeywordRelated.Count - 1

        While (index >= 0) ''key word deu tien la key word do nguoi dung go vo, cac keyword ke tiep la KeywordSynonym
            Dim subQuery As BooleanQuery = MakerQueryByKeyWord(isAllowSearchSingleKeywrodInDescription, lstKeywordRelated(index).KeywordName.ToLower(), searcher, analyzer, ParserItemName, ParserShortDesc, ParserLongDesc, False, currentBoot, isSuggest)
            subQuery.SetBoost(currentBoot) ''set boot de bao dam main keyword sap dau tien, cac KeywordSynonym sap theo thu tu nhu trong admin
            currentBoot = currentBoot * LuceneBoostFactor
            totalQuery.Add(subQuery, BooleanClause.Occur.SHOULD) ''day la cau query tong dung de search, cho nay la add tung cau query con cua moi keyword  vao cau query tong,BooleanClause.Occur.SHOULD co nghia la phep OR
            index = index - 1
        End While
        Return totalQuery
    End Function
    Public Function DisplayItemsSearchLucene(ByVal textSearch As String) As String

        Dim sArray1, sArray2, sArray3, sArray4, sArray5, sArray6, sConn As String
        Dim temp As String
        sConn = ""
        temp = textSearch

        If temp.Contains("Search by keyword or item") Then
            temp = ""
        End If
        temp = ReplaceChar(temp)
        sArray1 = "new Array("
        sArray2 = "new Array("
        sArray3 = "new Array("
        sArray4 = "new Array("
        sArray5 = "new Array("
        sArray6 = "new Array("

        If (Not String.IsNullOrEmpty(temp.Replace("*", "").Replace("?", ""))) Then
            ' set up lucene searcher
            Dim searcher As IndexSearcher = New IndexSearcher(_directory(1), False)
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)
            Dim lstKeywordRelated As KeywordCollection = MakeListKeywordSearch(textSearch)
            Dim isAllowSearchSingleKeywrodInDescription As Boolean = True '' cho search trong description khi tach tu
            Dim totalQuery As BooleanQuery = MakeTotalSearchQuery(isAllowSearchSingleKeywrodInDescription, temp, searcher, analyzer, lstKeywordRelated, True)
            If (totalQuery Is Nothing) Then
                Return String.Empty
            End If
            Dim hits = searcher.Search(totalQuery, Nothing, Utility.ConfigData.NumberOfDisplayItem, Sort.RELEVANCE).scoreDocs
            Dim Price As Double = 0
            Dim LowPrice As Double = 0
            Dim bPrice As Boolean = True
            For Each item As ScoreDoc In hits
                Dim doc As Document = searcher.Doc(item.doc)
                sArray1 &= sConn & Escape(HightLightSearchKeyword(doc.Get("ItemName"), lstKeywordRelated)) 'DivItem(dr("Imgage"), dr("ItemName"), dr("ShortDesc")) 
                sArray2 &= sConn & Escape(doc.Get("UrlCode") + "/" + doc.Get("ItemId"))
                sArray3 &= sConn & Escape(doc.Get("Image"))
                sArray4 &= sConn & Escape(HightLightSearchKeyword(doc.Get("ShortDesc"), lstKeywordRelated))
                sArray5 &= sConn & Escape(HightLightSearchSKU(doc.Get("SKU"), textSearch))
                If (bPrice) Then
                    sArray6 &= sConn & Escape(doc.Get("PriceDesc"))
                Else
                    sArray6 &= sConn & Escape("")
                End If

                sConn = ","
            Next
            'HttpContext.Current.Session("MemberId")
            analyzer.Close()
            searcher.Close()

        End If
        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        sArray3 = sArray3 & ")"
        sArray4 = sArray4 & ")"
        sArray5 = sArray5 & ")"
        sArray6 = sArray6 & ")"
        Dim result As String = sArray1 & "," & sArray2 & "," & sArray3 & "," & sArray4 & "," & sArray5 & "," & _SearchKeywordLucene(textSearch) & "," & sArray6
        Return result
    End Function
#End Region

End Class
