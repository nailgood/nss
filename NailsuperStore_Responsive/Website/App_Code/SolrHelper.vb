Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Net
Imports DataLayer
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Public Enum SolrAction
    RELOAD = 1
End Enum
Public Class SolrHelper

    Private Shared keywordNoResult As String() = {"foot bath liners", "nail polish shaker",
        "nail polish strips", "nail poster",
        "nail sign",
        "nail uniform",
        "nail vinyls",
        "nail wipe container",
        "no well tips",
        "opi avoplex",
        "opi brazil collection",
        "pedi liners",
        "pedicure chair part",
        "pedicure parts",
        "polish corrector",
        "poster",
        "rapid dry",
        "spa chair parts",
        "sugar scrub",
        "tip blender",
        "toe rings",
        "tool box",
        "vented manicure table",
        "wax paper",
        "white nail dryer table"
        }

    Public Shared SolrServerUrl As String = Utility.ConfigData.SolrServerURL

    Private Shared keywordRedirect As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Public Shared Function getKwSearchIdFromCache(ByVal kwName As String) As Integer
        If Not String.IsNullOrEmpty(kwName) Then
            Dim kwSearchId As Integer = Utility.CacheUtils.GetCache("SolrHelper_" + kwName)
            If kwSearchId > 0 Then
                Return kwSearchId
            End If
        End If
        Return 0
    End Function

    Public Shared Function getKeywordRedirect(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If

        If Not keywordRedirect.ContainsKey(keyword) Then
            SearchItem(keyword, Nothing, Nothing, 1, 1, 1, String.Empty, 0, String.Empty, String.Empty, False, False, 0, 0, 0, "", True)
        End If
        If keywordRedirect.ContainsKey(keyword) Then
            Return keywordRedirect(keyword)
        End If

        Return String.Empty
    End Function

    Private Shared Function RequestUrl(ByVal url As String) As XmlDocument
        Dim xmlDoc As XmlDocument = New XmlDocument()
        Try
            Dim request As HttpWebRequest = WebRequest.Create(url)
            Dim response As HttpWebResponse = request.GetResponse()

            xmlDoc.Load(response.GetResponseStream())
            Return xmlDoc
        Catch ex As Exception

        End Try
        Return xmlDoc
    End Function

    Private Shared Function getXmlNodeLucene(ByVal name As String, ByVal doc As XmlNode) As String
        Try
            For Each Field As XmlNode In doc.ChildNodes
                If Field.Attributes("name").Value = name Then
                    If name <> "departmentid" Then
                        Return Field.InnerXml
                    Else
                        Dim departmentId As String = String.Empty
                        For Each node As XmlNode In Field.ChildNodes
                            departmentId = departmentId & node.InnerXml & ","
                        Next
                        Return departmentId.Substring(0, departmentId.Length - 1)
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function
    Private Shared Function getFacet(ByVal urlRequest As String, ByVal key As String, ByVal categoryFilter As String, ByVal brandFilter As String, ByVal priceFilter As String, ByVal rattingFilter As String, ByVal collectionFilter As String, ByVal toneFilter As String, ByVal shadeFilter As String) As Dictionary(Of String, String)

        Dim xmlDoc As XmlDocument = New XmlDocument()
        Dim dic As Dictionary(Of String, String) = New Dictionary(Of String, String)(3)

        Dim lstCountBrand As String = ",", lstCountCategory As String = ",", lstCountCollection As String = ",", lstCountTone As String = ",", lstCountShade As String = ","
        Dim pricesStr As String = ";"
        Dim rattingsStr As String = ";"

        If (key.Contains("collections")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(collectionFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + collectionFilter, "")
            End If

            xmlDoc = getResponse(urlRequest)
            'get collectionid
            Dim collections As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='collectionid']/int")
            If collections.Count > 0 Then
                For i = 0 To collections.Count - 1
                    Dim collectionIdStr As String = collections.ItemOf(i).Attributes("name").Value
                    Dim collectioncount = collections.ItemOf(i).InnerXml
                    If collectionIdStr <> "0" Then
                        lstCountCollection = lstCountCollection & collectionIdStr & ":" & collectioncount & ","
                    End If

                Next
            End If
            dic.Add("collections", lstCountCollection)
            If Not String.IsNullOrEmpty(collectionFilter) Then
                urlRequest = urlRequest & ("&fq=" + collectionFilter)
            End If
        End If

        If (key.Contains("tones")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(toneFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + toneFilter, "")
            End If

            xmlDoc = getResponse(urlRequest)
            'get toneid
            Dim tones As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='toneid']/int")
            If tones.Count > 0 Then
                For i = 0 To tones.Count - 1
                    Dim toneIdStr As String = tones.ItemOf(i).Attributes("name").Value
                    Dim tonecount = tones.ItemOf(i).InnerXml
                    If toneIdStr <> "0" Then
                        lstCountTone = lstCountTone & toneIdStr & ":" & tonecount & ","
                    End If

                Next
            End If
            dic.Add("tones", lstCountTone)
            If Not String.IsNullOrEmpty(toneFilter) Then
                urlRequest = urlRequest & ("&fq=" + toneFilter)
            End If
        End If

        If (key.Contains("shades")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(shadeFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + shadeFilter, "")
            End If

            xmlDoc = getResponse(urlRequest)
            'get shadeid
            Dim shades As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='shadeid']/int")
            If shades.Count > 0 Then
                For i = 0 To shades.Count - 1
                    Dim shadeIdStr As String = shades.ItemOf(i).Attributes("name").Value
                    Dim shadecount = shades.ItemOf(i).InnerXml
                    If shadeIdStr <> "0" Then
                        lstCountShade = lstCountShade & shadeIdStr & ":" & shadecount & ","
                    End If

                Next
            End If
            dic.Add("shades", lstCountShade)
            If Not String.IsNullOrEmpty(shadeFilter) Then
                urlRequest = urlRequest & ("&fq=" + shadeFilter)
            End If
        End If

        If (key.Contains("brands")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(brandFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + brandFilter, "")
            End If

            xmlDoc = getResponse(urlRequest)

            'get brandid
            Dim brands As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='brandid']/int")
            If brands.Count > 0 Then
                For i = 0 To brands.Count - 1
                    Dim brandIdStr As String = brands.ItemOf(i).Attributes("name").Value
                    Dim Brandcount = brands.ItemOf(i).InnerXml
                    If brandIdStr <> "0" Then
                        lstCountBrand = lstCountBrand & brandIdStr & ":" & Brandcount & ","
                    End If

                Next
            End If
            dic.Add("brands", lstCountBrand)
            If Not String.IsNullOrEmpty(brandFilter) Then
                urlRequest = urlRequest & ("&fq=" + brandFilter)
            End If
        End If

        If (key.Contains("categories")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(categoryFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + categoryFilter, "")
            End If
            xmlDoc = getResponse(urlRequest)
            'get categories
            Dim categories As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='departmentid']/int")
            If categories.Count > 0 Then
                For i = 0 To categories.Count - 1
                    Dim DepartmentIdStr As String = categories.ItemOf(i).Attributes("name").Value
                    Dim categorycount = categories.ItemOf(i).InnerXml
                    lstCountCategory = lstCountCategory & DepartmentIdStr & ":" & categorycount & ","
                Next
            End If
            dic.Add("categories", lstCountCategory)
            If Not String.IsNullOrEmpty(categoryFilter) Then
                urlRequest = urlRequest & "&fq=" + categoryFilter
            End If
        End If

        If (key.Contains("prices")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(priceFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + priceFilter, "")
            End If
            xmlDoc = getResponse(urlRequest)
            'get price
            Dim prices As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_intervals']/lst[@name='pricefilter']/int")
            If prices.Count > 0 Then
                For i = 0 To prices.Count - 1
                    Dim pricesAtt As String = prices.ItemOf(i).Attributes("name").Value
                    Dim priceCount = prices.ItemOf(i).InnerXml
                    pricesStr &= pricesAtt.Replace("[", "(").Replace("]", ")").Replace("*", "") & ":" & priceCount & ";"
                Next
            End If
            dic.Add("prices", pricesStr)
            If Not String.IsNullOrEmpty(priceFilter) Then
                urlRequest = urlRequest & "&fq=" + priceFilter
            End If
        End If

        If (key.Contains("rattings")) Then
            xmlDoc = New XmlDocument()
            If Not String.IsNullOrEmpty(rattingFilter) Then
                urlRequest = urlRequest.Replace("&fq=" + rattingFilter, "")
            End If
            xmlDoc = getResponse(urlRequest)
            'get ratting
            Dim rattings As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_intervals']/lst[@name='averagereview']/int")
            If rattings.Count > 0 Then
                For i = 0 To rattings.Count - 1
                    Dim rattingsAtt As String = rattings.ItemOf(i).Attributes("name").Value
                    Dim rattingCount = rattings.ItemOf(i).InnerXml
                    rattingsStr = rattingsStr & rattingsAtt.Replace("[", "(").Replace("]", ")").Replace("*", "") & ":" & rattingCount & ";"
                Next
            End If
            dic.Add("rattings", rattingsStr)
            If Not String.IsNullOrEmpty(rattingFilter) Then
                urlRequest = urlRequest & "&fq=" + rattingFilter
            End If
        End If
        Return dic
    End Function
    Private Shared Function getFacetNewItems(ByVal xmlDoc As XmlDocument) As Boolean

        'get categories
        Dim result As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='newitems']/int")
        If result.Count > 0 Then
            For i = 0 To result.Count - 1
                Dim trueString As String = result.ItemOf(i).Attributes("name").Value
                If trueString = "true" Then
                    Dim countStr As String = result.ItemOf(i).InnerXml
                    Dim count As Integer = 0
                    Integer.TryParse(countStr, count)
                    Return count > 0
                End If
            Next
        End If
        Return False
    End Function
    Private Shared Function GetMinMaxValue(ByVal value As String, ByVal MaxValue As Boolean) As Double
        Try
            value = value.Replace("(", "")
            value = value.Replace(")", "")
            Dim i As Integer = value.IndexOf(",")
            If i >= 0 Then
                If MaxValue Then
                    value = value.Substring(i)
                Else
                    value = value.Substring(0, i)
                End If
                value = value.Replace(",", "")
                Return Convert.ToDouble(value)
            End If
        Catch ex As Exception

        End Try
        Return 0
    End Function

    'Public Shared Function getKeyWordAnalysis(ByVal keyword As String) As String
    '    keyword = keyword.Trim()
    '    Dim result As String = String.Empty
    '    If String.IsNullOrEmpty(keyword) Then
    '        Return String.Empty
    '    End If

    '    Dim url As String = String.Format("http://localhost:8983/solr/db/analysis/field?analysis.fieldvalue=""{0}""&analysis.fieldname=itemname&wt=xml", keyword)

    '    Try
    '        Dim request As HttpWebRequest = WebRequest.Create(url)
    '        Dim response As HttpWebResponse = request.GetResponse()

    '        Dim xmlDoc As XmlDocument = New XmlDocument()
    '        xmlDoc.Load(response.GetResponseStream())
    '        If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
    '            Dim keywordArr As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='analysis']/lst[@name='field_names']/lst[@name='itemname']/lst[@name='index']/arr[@name='org.apache.lucene.analysis.en.EnglishMinimalStemFilter']/lst/str[@name='text']")

    '            If keywordArr.Count > 0 Then
    '                For i = 0 To keywordArr.Count - 1
    '                    result &= keywordArr.ItemOf(i).InnerXml & " "
    '                Next
    '            End If

    '        End If
    '        Return result.Trim()
    '    Catch ex As Exception
    '        Components.Email.SendError("ToError500", "getKeyWordAnalysis", ex.ToString())
    '        Return String.Empty
    '    End Try

    'End Function

    Private Shared Function buildField(ByVal field As String, ByVal keyword As String) As String
        If keyword.Contains(" ") Then
            Dim result As String = String.Empty
            Dim listStr As String() = keyword.Split(" ").Where(Function(i) i.Trim().Length > 0).ToArray()
            For Each item As String In listStr
                result &= String.Format("AND {0}:""{1}"" ", field, item)
            Next
            result = result.Substring(3, result.Length - 4)
            Return result
        Else
            Return String.Format("""{0}"":{1}", field, keyword)
        End If
    End Function
    Private Shared Function buildField(ByVal field As String, ParamArray ByVal kws As String()) As String
        If kws.Count() > 0 Then
            Dim result As String = String.Empty
            For Each item As String In kws
                result &= String.Format("AND {0}:""{1}"" ", field, item)
            Next
            result = result.Substring(3, result.Length - 4)
            Return result
        End If
        Return String.Empty
    End Function

    Private Shared Function removeShopWord(ByVal str As String) As String
        Dim stopword As String = "a an and are as at be but by for if in into is it no not of on or s such t that the their then there these they this to was will with"
        Return String.Join(" ", str.Split(" ").Where(Function(wrd) Not stopword.Contains(wrd)))
    End Function

    Public Shared Function buildSearch(ByVal kw As String, ByVal getKwRedirect As Boolean, ByVal isInBulk As Boolean, Optional ByVal autocomplete As Boolean = False) As String
        Dim q As String = Utility.ConfigData.SolrServerURL

        'Dim queryCache As String = KeywordRow.getQueryCacheSearch(kw, autocomplete)
        'If Not String.IsNullOrEmpty(queryCache) Then
        '    If Not autocomplete Then
        '        Return q + queryCache
        '    End If
        '    Dim request = WebRequest.Create(q + queryCache)
        '    Dim response = request.GetResponse()
        '    Using stream2 As Stream = response.GetResponseStream()
        '        Dim streamReader2 As StreamReader = New StreamReader(stream2)
        '        Dim result = streamReader2.ReadToEnd()
        '        Return result.Trim().Replace("?(", "").Replace("}})", "}}")
        '    End Using

        'End If


        Dim kwOrigin As String = kw
        kw = removeShopWord(kw.Trim())
        kw = Utility.Common.CheckSearchKeyword(kw)
        'Add keyword
        Dim fieldSearch As String = String.Empty, fieldSynSearch As String = String.Empty

        If getKwRedirect Then
            fieldSearch = "hasredirect:true AND itemname:""" & kwOrigin + """"
            Return q + fieldSearch
        End If

        If IsKeywordSKU(kw) Then
            Dim skuNumber As Integer = 0
            fieldSearch = "sku:" & kw
            If autocomplete Then
                Try
                    Dim request As HttpWebRequest = WebRequest.Create(q + fieldSearch)
                    Dim response As HttpWebResponse = request.GetResponse()
                    Using stream As Stream = response.GetResponseStream()
                        Dim streamReader As StreamReader = New StreamReader(stream)
                        Dim result = streamReader.ReadToEnd()
                        Return result.Trim().Replace("?(", "").Replace("}})", "}}")
                    End Using
                Catch ex As WebException
                    If (ex.Status = WebExceptionStatus.ProtocolError) Then
                        Dim resp As WebResponse = ex.Response
                        Using sr = New StreamReader(resp.GetResponseStream())
                            Dim str As String = sr.ReadToEnd()
                            Return str
                        End Using

                    End If
                End Try
            End If
            Return q + fieldSearch
        End If

        If isInBulk Then
            Dim keywordBuyInBulk As String = kwOrigin
            Dim listKeyWordBuyInBulk As IEnumerable(Of KeywordSynonymRow) = KeywordSynonymRow.GetBuyInBulkSynonym(kwOrigin)
            If listKeyWordBuyInBulk.Count > 0 Then
                For Each synKw As KeywordSynonymRow In listKeyWordBuyInBulk.OrderByDescending(Function(i) i.KeywordSynonymName.Length)
                    keywordBuyInBulk = ((" " + keywordBuyInBulk + " ").Replace(synKw.KeywordSynonymName, "")).Replace("  ", " ").Trim()
                Next

                If String.IsNullOrEmpty(keywordBuyInBulk.Trim()) Then
                    keywordBuyInBulk = "*"
                    fieldSearch = IIf(String.IsNullOrEmpty(fieldSearch), "", fieldSearch & " OR ") & "isinbulk:true"
                Else
                    Dim keywordBuyInBulkTemp As String = keywordBuyInBulk
                    keywordBuyInBulk = keywordBuyInBulk.Replace("&", "")
                    If keywordBuyInBulk.Contains(" ") Then
                        fieldSearch = IIf(String.IsNullOrEmpty(fieldSearch), "", fieldSearch & " OR ") & String.Format("itemnamepricedesc:""{0}""^={1} OR (({5})^={2} OR itemnamepricedesc:""{0}""~10000^={2}) OR longdesc:""{0}""^={3} OR ({6})^={4}", keywordBuyInBulk, 900000, 10000, 1000, 100, buildField("itemnamepricedesc", keywordBuyInBulk), buildField("longdesc", keywordBuyInBulk))
                    Else
                        fieldSearch = fieldSearch & String.Format("itemnamepricedesc:""{0}""^={1} OR longdesc:""{0}""^={2}", keywordBuyInBulk, 900000, 1000)
                    End If

                    Dim listKeyWordBuyInBulkSyn As KeywordCollection = KeywordSynonymRow.GetListSynonymSearchNew(keywordBuyInBulkTemp)

                    If listKeyWordBuyInBulkSyn.Count > 0 Then
                        Dim index As Integer = listKeyWordBuyInBulkSyn.Count - 1
                        While (index >= 0)
                            Dim keyword As String = listKeyWordBuyInBulkSyn.Item(index).KeywordName
                            keyword = removeString(keyword)
                            If keyword.Contains(" ") Then
                                fieldSearch = fieldSearch & String.Format(" OR itemnamepricedesc:""{0}""^={1} OR (({5})^={2} OR itemnamepricedesc:""{0}""~10000^={2}) OR longdesc:""{0}""^={3} OR ({6})^={4}", keyword, 10000, 1000, 100, 10, buildField("itemnamepricedesc", keywordBuyInBulk), buildField("longdesc", keywordBuyInBulk))
                            Else
                                fieldSearch = fieldSearch & String.Format(" OR (itemnamepricedesc:""{0}""^={1} OR pricedescsearch:""{0}""^={1}) OR longdesc:""{0}""^={2}", keyword, 10000, 0.1)
                            End If
                            index = index - 1
                        End While
                    End If

                    'Build merge, collectiontoneshade
                    If keywordBuyInBulk.Contains(" ") Then
                        fieldSearch &= " OR " & buildMergeSearch(keywordBuyInBulk, autocomplete)

                        'fieldSearch &= String.Format(" OR departmentcollectiontoneshade:""{0}""^=9000000", kw.Replace(" ", ""))
                    End If
                    fieldSearch &= buildSearchDepartmentCollectionToneShade(keywordBuyInBulkTemp)

                End If

                If autocomplete Then

                    If keywordBuyInBulk <> "*" Then

                        If Not keywordBuyInBulk.Contains(" ") Then
                            fieldSearch = fieldSearch + String.Format(" OR (autocompleteitemname:""{0}""^=-10 OR autocompletelongdesc:""{0}""^=-100) ", keywordBuyInBulk)
                        Else
                            fieldSearch = fieldSearch + String.Format(" OR (autocompleteitemname:""{0}""~5000^=-10 OR autocompletelongdesc:""{0}""~5000^=-100) ", keywordBuyInBulk)

                            Dim listStr = keywordBuyInBulk.Trim().Split(" ")
                            If listStr.Length = 2 Then
                                fieldSearch = fieldSearch + String.Format(" OR ((autocompleteitemname:""{0}""~5000^=-20 OR autocompleteitemname:""{1}""^=-40) OR (autocompletelongdesc:""{0}""~5000^=-200 OR autocompletelongdesc:""{1}""^=-400)) ", keywordBuyInBulk, keywordBuyInBulk.Replace(" ", ""))
                            Else
                                Dim listKeyWordAutoCp As List(Of String) = New List(Of String)()
                                Dim fieldSearchAutoCp As String = String.Empty

                                Dim index As Int16 = 0
                                For Each str As String In listStr
                                    index = keywordBuyInBulk.IndexOf(str, index)
                                    If keywordBuyInBulk.Length > index + str.Length Then
                                        listKeyWordAutoCp.Add(keywordBuyInBulk.Remove(index + str.Length, 1))
                                    End If
                                Next

                                listKeyWordAutoCp.Add(keywordBuyInBulk.Replace(" ", ""))

                                If listKeyWordAutoCp.Count > 1 Then
                                    Dim longestKw As String = listKeyWordAutoCp.OrderBy(Function(x) x.Length).First()
                                    fieldSearchAutoCp = String.Format(" OR (autocompleteitemname:""{0}""~5^=-60 OR autocompletelongdesc:""{0}""~5000^=-600)", longestKw)
                                    listKeyWordAutoCp.Remove(longestKw)

                                    index = listKeyWordAutoCp.Count - 1
                                    While (index >= 0)
                                        Dim keyword As String = listKeyWordAutoCp.Item(index)
                                        fieldSearchAutoCp = fieldSearchAutoCp & String.Format(" OR (autocompleteitemname:""{0}""~5000^=-0.0001 OR autocompletelongdesc:""{0}""~5000^=-0.000001)", keyword)
                                        index = index - 1
                                    End While
                                End If
                                If Not String.IsNullOrEmpty(fieldSearchAutoCp) Then
                                    fieldSearch &= fieldSearchAutoCp
                                End If
                            End If

                        End If
                        fieldSearch = exceptKw(removeString(keywordBuyInBulk), autocomplete) + "(" + fieldSearch + ")"
                    End If

                    fieldSearch = fieldSearch & "&fq=isinbulk:true"
                    Try
                        Dim request As HttpWebRequest = WebRequest.Create(q + fieldSearch)
                        Dim response As HttpWebResponse = request.GetResponse()
                        Using stream As Stream = response.GetResponseStream()
                            Dim streamReader As StreamReader = New StreamReader(stream)
                            Dim result = streamReader.ReadToEnd()

                            Dim resultTemp As String = result.Trim().Replace("?(", "").Replace("}})", "}}")
                            If getResultCount(result) > 0 Then
                                KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
                                Return resultTemp
                            ElseIf kw.Split(" ").Count() >= 2 Then
                                fieldSearch = buildSplitPhase(kw)
                                If Not String.IsNullOrEmpty(fieldSearch) Then
                                    request = WebRequest.Create(q + fieldSearch)
                                    response = request.GetResponse()
                                    Using stream2 As Stream = response.GetResponseStream()
                                        Dim streamReader2 As StreamReader = New StreamReader(stream2)
                                        result = streamReader2.ReadToEnd()
                                        KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
                                        Return result.Trim().Replace("?(", "").Replace("}})", "}}")
                                    End Using
                                End If
                            Else
                                Return resultTemp
                            End If
                        End Using
                    Catch ex As WebException
                        If (ex.Status = WebExceptionStatus.ProtocolError) Then
                            Dim resp As WebResponse = ex.Response
                            Using sr = New StreamReader(resp.GetResponseStream())
                                Dim str As String = sr.ReadToEnd()
                                Return str
                            End Using

                        End If
                    End Try
                End If
            End If

            If keywordBuyInBulk <> "*" Then
                fieldSearch = exceptKw(removeString(keywordBuyInBulk), autocomplete) + "hasredirect:false AND (" + fieldSearch + ")"
            Else
                fieldSearch = fieldSearch + "hasredirect:false AND (" + fieldSearch + ")"
            End If

            fieldSearch = fieldSearch & "&fq=isinbulk:true"
            KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
            Return q + fieldSearch
        Else ' khong buy in bulk
            If kw.Contains(" ") Then
                fieldSearch = fieldSearch & String.Format("itemnamepricedesc:""{0}""^={1} OR (({5})^={2} OR itemnamepricedesc:""{0}""~10000^={2}) OR longdesc:""{0}""^={3} OR ({6})^={4}", kw, 900000, 10000, 1000, 100, buildField("itemnamepricedesc", kw), buildField("longdesc", kw))
            Else
                fieldSearch = fieldSearch & String.Format("itemnamepricedesc:""{0}""^={1} OR longdesc:""{0}""^={2}", kw, 900000, 1000)
            End If

            Dim listKeyWord As KeywordCollection = KeywordSynonymRow.GetListSynonymSearchNew(kw)

            If listKeyWord.Count > 0 Then
                Dim index As Integer = listKeyWord.Count - 1
                While (index >= 0)
                    Dim keyword As String = listKeyWord.Item(index).KeywordName
                    If keyword.Contains(" ") Then
                        fieldSearch = fieldSearch & String.Format(" OR itemnamepricedesc:""{0}""^={1} OR (({5})^={2} OR itemnamepricedesc:""{0}""~10000^={2}) OR longdesc:""{0}""^={3} OR ({6})^={4}", keyword, 10000, 1000, 100, 10, buildField("itemnamepricedesc", kw), buildField("longdesc", kw))
                    Else
                        fieldSearch = fieldSearch & String.Format(" OR (itemnamepricedesc:""{0}""^={1} OR pricedescsearch:""{0}""^={1}) OR longdesc:""{0}""^={2}", keyword, 10000, 0.1)
                    End If
                    index = index - 1
                End While
            End If

            'Build merge, collectiontoneshade
            If kw.Contains(" ") Then
                fieldSearch &= " OR " & buildMergeSearch(kw, autocomplete)

                'fieldSearch &= String.Format(" OR departmentcollectiontoneshade:""{0}""^=9000000", kw.Replace(" ", ""))

            End If
            fieldSearch &= buildSearchDepartmentCollectionToneShade(kwOrigin)

            If autocomplete Then
                If Not kw.Contains(" ") Then
                    fieldSearch = fieldSearch + String.Format(" OR (autocompleteitemname:""{0}""^=-10 OR autocompletelongdesc:""{0}""^=-100) ", kw)
                Else
                    fieldSearch = fieldSearch + String.Format(" OR (autocompleteitemname:""{0}""~5000^=-10 OR autocompletelongdesc:""{0}""~5000^=-100) ", kw)

                    Dim listStr = kw.Trim().Split(" ")
                    If listStr.Length = 2 Then
                        fieldSearch = fieldSearch + String.Format(" OR ((autocompleteitemname:""{0}""~5000^=-20 OR autocompleteitemname:""{1}""^=-40) OR (autocompletelongdesc:""{0}""~5000^=-200 OR autocompletelongdesc:""{1}""^=-400)) ", kw, kw.Replace(" ", ""))
                    Else
                        Dim listKeyWordAutoCp As List(Of String) = New List(Of String)()
                        Dim fieldSearchAutoCp As String = String.Empty

                        Dim index As Int16 = 0
                        For Each str As String In listStr
                            index = kw.IndexOf(str, index)
                            If kw.Length > index + str.Length Then
                                listKeyWordAutoCp.Add(kw.Remove(index + str.Length, 1))
                            End If
                        Next

                        listKeyWordAutoCp.Add(kw.Replace(" ", ""))

                        If listKeyWordAutoCp.Count > 1 Then
                            Dim longestKw As String = listKeyWordAutoCp.OrderBy(Function(x) x.Length).First()
                            fieldSearchAutoCp = String.Format(" OR (autocompleteitemname:""{0}""~5^=-60 OR autocompletelongdesc:""{0}""~5000^=-600)", longestKw)
                            listKeyWordAutoCp.Remove(longestKw)

                            index = listKeyWordAutoCp.Count - 1
                            While (index >= 0)
                                Dim keyword As String = listKeyWordAutoCp.Item(index)
                                fieldSearchAutoCp = fieldSearchAutoCp & String.Format(" OR (autocompleteitemname:""{0}""~5000^=-0.0001 OR autocompletelongdesc:""{0}""~5000^=-0.000001)", keyword)
                                index = index - 1
                            End While
                        End If
                        If Not String.IsNullOrEmpty(fieldSearchAutoCp) Then
                            fieldSearch &= fieldSearchAutoCp
                        End If
                    End If

                End If
            End If

            'fieldSearch = exceptKw(kw, autocomplete) + "(" + fieldSearch + ")"

        End If

        If autocomplete Then

            Try
                fieldSearch = exceptKw(kw, autocomplete) + "(" + fieldSearch + ")"
                'include redirect kw
                fieldSearch = "itemname:""" + kwOrigin + """^9999999 OR (" + fieldSearch + ")"

                Dim request As HttpWebRequest = WebRequest.Create(q + fieldSearch)
                Dim response As HttpWebResponse = request.GetResponse()
                Using stream As Stream = response.GetResponseStream()
                    Dim streamReader As StreamReader = New StreamReader(stream)
                    Dim result = streamReader.ReadToEnd()

                    Dim resultTemp As String = result.Trim().Replace("?(", "").Replace("}})", "}}")
                    If getResultCount(result) > 0 Then
                        KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
                        Return resultTemp
                    ElseIf kw.Split(" ").Count() >= 2 Then
                        fieldSearch = buildSplitPhase(kw)
                        If Not String.IsNullOrEmpty(fieldSearch) Then
                            request = WebRequest.Create(q + fieldSearch)
                            response = request.GetResponse()
                            Using stream2 As Stream = response.GetResponseStream()
                                Dim streamReader2 As StreamReader = New StreamReader(stream2)
                                result = streamReader2.ReadToEnd()
                                KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
                                Return result.Trim().Replace("?(", "").Replace("}})", "}}")
                            End Using
                        End If
                    Else
                        Return resultTemp
                    End If
                End Using
            Catch ex As WebException
                If (ex.Status = WebExceptionStatus.ProtocolError) Then
                    Dim resp As WebResponse = ex.Response
                    Using sr = New StreamReader(resp.GetResponseStream())
                        Dim str As String = sr.ReadToEnd()
                        Return str
                    End Using

                End If
            End Try

        End If
        kw = removeString(kw)
        fieldSearch = exceptKw(kw, autocomplete) + "hasredirect:false AND (" + fieldSearch + ")"
        KeywordRow.setQueryCacheSearch(kw, autocomplete, fieldSearch)
        Return q + fieldSearch
    End Function

    Shared Function getResultCount(ByVal resultSearch As String) As Integer
        Dim first As Integer = resultSearch.IndexOf("""numFound"":")
        Dim last As Integer = resultSearch.IndexOf(",""start""")

        Dim numFoundStr As String = resultSearch.Substring(first + 11, last - first - 11)

        Dim numFound As Integer = 0
        If Integer.TryParse(numFoundStr, numFound) Then
            Return numFound
        End If

        Return 0
    End Function

    Public Shared Sub standarnizerKw(ByRef kws As List(Of String))
        If kws.Contains(" And ") Then
            kws.Remove(" And ")
            kws.Add(" & ")
        End If
    End Sub
    Public Shared Sub checkDepartment(ByVal kwNeedCheck As List(Of String), ByVal departments As List(Of String), ByRef departmentMatch As List(Of String), ByRef departmentOficial As List(Of String))
        If departments Is Nothing Then
            departments = departmentMatch.ToList()
        End If
        standarnizerKw(kwNeedCheck)
        Dim resultmatch = departments.Where(Function(department) kwNeedCheck.All(Function(kw) department.ToLower().Split(" ").Any(Function(departmentKw) String.CompareOrdinal(departmentKw, kw) = 0)))

        If resultmatch.Count() > 0 Then
            departmentMatch.AddRange(resultmatch)
            departmentMatch = departmentMatch.Distinct().ToList()

            For Each departmentName As String In departments
                If departmentName.Contains(" ") Then
                    Dim tonewords = departmentName.Split(" ")
                    If tonewords.Length = kwNeedCheck.Count Then
                        Dim isTrue = True
                        For Each kw As String In kwNeedCheck
                            If Not tonewords.Contains(kw) Then
                                isTrue = False
                                Exit For
                            End If
                        Next
                        If isTrue Then
                            departmentOficial.Add(departmentName)
                        End If
                    End If
                Else
                    If kwNeedCheck.Contains(departmentName) Then
                        departmentOficial.Add(departmentName)
                    End If
                End If
            Next

        End If
    End Sub
    Public Shared Sub checkCollection(ByVal kwNeedCheck As List(Of String), ByVal collections As List(Of String), ByRef collectionMatch As List(Of String), ByRef collectionOficial As List(Of String))
        If collections Is Nothing Then
            collections = collectionMatch.ToList()
        End If
        standarnizerKw(kwNeedCheck)
        Dim resultmatch = collections.Where(Function(department) kwNeedCheck.All(Function(kw) department.ToLower().Split(" ").Any(Function(departmentKw) String.CompareOrdinal(departmentKw, kw) = 0)))

        If resultmatch.Count() > 0 Then
            collectionMatch.AddRange(resultmatch)
            collectionMatch = collectionMatch.Distinct().ToList()

            For Each collectionName As String In collections
                If collectionName.Contains(" ") Then
                    Dim tonewords = collectionName.Split(" ").Where(Function(i) i.Length > 0)
                    If tonewords.Count() = kwNeedCheck.Count Then
                        Dim isTrue = True
                        For Each kw As String In kwNeedCheck
                            If Not tonewords.Contains(kw) Then
                                isTrue = False
                                Exit For
                            End If
                        Next
                        If isTrue Then
                            collectionOficial.Add(collectionName)
                        End If
                    End If
                Else
                    If kwNeedCheck.Contains(collectionName) Then
                        collectionOficial.Add(collectionName)
                    End If
                End If
            Next

        End If
    End Sub
    Public Shared Sub checkTone(ByVal kwNeedCheck As List(Of String), ByVal tones As List(Of String), ByRef toneMatch As List(Of String), ByRef toneOficial As List(Of String))
        If tones Is Nothing Then
            tones = toneMatch.ToList()
        End If
        standarnizerKw(kwNeedCheck)
        Dim resultmatch = tones.Where(Function(tone) kwNeedCheck.All(Function(kw) tone.ToLower().Split(" ").Any(Function(toneKw) String.CompareOrdinal(toneKw, kw) = 0)))

        If resultmatch.Count() > 0 Then
            toneMatch.AddRange(resultmatch)
            toneMatch = toneMatch.Distinct().ToList()


            For Each toneName As String In tones
                If toneName.Contains(" ") Then
                    Dim tonewords = toneName.Split(" ")
                    If tonewords.Length = kwNeedCheck.Count Then
                        Dim isTrue = True
                        For Each kw As String In kwNeedCheck
                            If Not tonewords.Contains(kw) Then
                                isTrue = False
                                Exit For
                            End If
                        Next
                        If isTrue Then
                            toneOficial.Add(toneName)
                        End If
                    End If
                Else
                    If kwNeedCheck.Contains(toneName) Then
                        toneOficial.Add(toneName)
                    End If
                End If
            Next

        End If
    End Sub
    Public Shared Sub checkShade(ByVal kwNeedCheck As List(Of String), ByVal shades As List(Of String), ByRef shadeMatch As List(Of String), ByRef shadeOficial As List(Of String))
        If shades Is Nothing Then
            shades = shadeMatch.ToList()
        End If
        standarnizerKw(kwNeedCheck)
        Dim resultmatch = shades.Where(Function(department) kwNeedCheck.All(Function(kw) department.ToLower().Split(" ").Any(Function(departmentKw) String.CompareOrdinal(departmentKw, kw) = 0)))

        If resultmatch.Count() > 0 Then
            shadeMatch.AddRange(resultmatch)
            shadeMatch = shadeMatch.Distinct().ToList()

            For Each shadeName As String In shades
                If shadeName.Contains(" ") Then
                    Dim tonewords = shadeName.Split(" ")
                    If tonewords.Length = kwNeedCheck.Count Then
                        Dim isTrue = True
                        For Each kw As String In kwNeedCheck
                            If Not tonewords.Contains(kw) Then
                                isTrue = False
                                Exit For
                            End If
                        Next
                        If isTrue Then
                            shadeOficial.Add(shadeName)
                        End If
                    End If
                Else
                    If kwNeedCheck.Contains(shadeName) Then
                        shadeOficial.Add(shadeName)
                    End If
                End If
            Next

        End If
    End Sub

    Private Shared Function buildSearchDepartmentCollectionToneShade(ByVal kw As String) As String
        Try
            Dim result As String = String.Empty

            Dim departments = StoreDepartmentRow.GetDepartmentForSearch().ToList()
            Dim collections = StoreCollectionRow.GetCollectionForSearch().ToList()
            Dim tones = StoreToneRow.GetStoreToneForSearch().ToList()
            Dim shades = StoreShadeRow.GetStoreShadeForSearch().ToList()
            kw = kw.Replace("-", " ")
            Dim kws = New List(Of String)(kw.Split(" "))

            Dim departmentMatch = New List(Of String)
            Dim collectionMath = New List(Of String)
            Dim toneMatch = New List(Of String)
            Dim shadeMatch = New List(Of String)

            Dim departmentOfficial = New List(Of String)
            Dim collectionOfficial = New List(Of String)
            Dim toneOfficial = New List(Of String)
            Dim shadeOfficial = New List(Of String)

            Dim toneSynonym = KeywordSynonymRow.getSynonymInKw(kw, "tone")
            If toneSynonym.Count > 0 Then
                tones.RemoveAll(Function(tone) toneSynonym.Any(Function(toneSyn) tone = toneSyn.Key))
            End If

            Dim kwNoCheck = New List(Of String)

            Dim numberKwProcess As Integer = kws.Count
            Dim countKwProcess As Integer = 0

            Dim kwChecks = New List(Of String)
            While countKwProcess < numberKwProcess
                For i = 0 To kws.Count - countKwProcess - 1
                    kwChecks.Clear()
                    For j = i To kws.Count - 1
                        Dim kwCp = kws(j)
                        If kwChecks.Any(Function(kwChek) String.CompareOrdinal(kwCp, kw) = 0) Then
                            Continue For
                        End If
                        kwChecks.Add(kwCp)

                        If kwChecks.Count = countKwProcess + 1 Then
                            checkDepartment(kwChecks, IIf(countKwProcess = 0, departments, departmentMatch.ToList()), departmentMatch, departmentOfficial)
                            checkCollection(kwChecks, IIf(countKwProcess = 0, collections, collectionMath.ToList()), collectionMath, collectionOfficial)
                            checkTone(kwChecks, IIf(countKwProcess = 0, tones, toneMatch.ToList()), toneMatch, toneOfficial)
                            checkShade(kwChecks, IIf(countKwProcess = 0, shades, shadeMatch.ToList()), shadeMatch, shadeOfficial)
                            kwChecks.RemoveAt(kwChecks.Count - 1)
                        End If
                    Next
                Next

                countKwProcess = countKwProcess + 1
            End While

            If departmentOfficial.Count > 0 Then
                departmentOfficial = departmentOfficial.Distinct().ToList()
            End If
            If collectionOfficial.Count > 0 Then
                collectionOfficial = collectionOfficial.Distinct().ToList()
            End If
            If toneOfficial.Count > 0 Then
                toneOfficial = toneOfficial.Distinct().ToList()
                For index = 0 To toneOfficial.Count - 1
                    toneSynonym.Add(toneOfficial(index), toneOfficial(index))
                Next

            End If
            If shadeOfficial.Count > 0 Then
                shadeOfficial = shadeOfficial.Distinct().ToList()
            End If

            If departmentOfficial.Count > 0 Then
                For Each department As String In departmentOfficial
                    Dim itemname As String = " " + kw.Replace(" ", "  ") + " "
                    Dim departmentword = department.Split(" ").Where(Function(i) i.Length > 0)
                    For Each dpm As String In departmentword
                        itemname = itemname.Replace(" " + dpm + " ", "")
                    Next
                    Dim itemnameDepartment = itemname.Trim()
                    If Not String.IsNullOrEmpty(itemnameDepartment) Then
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND departmentname:""{1}"")", itemnameDepartment, department.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND departmentname:""{1}"")", itemnameDepartment, department.Replace(" ", "")))
                    Else
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(departmentname:""{0}"")", department.Replace(" ", "")), String.Format(" OR (departmentname:""{0}"")", department.Replace(" ", "")))
                    End If

                    For Each collection As String In collectionOfficial
                        Dim collectionWord = collection.Split(" ").Where(Function(i) i.Length > 0)
                        For Each clt As String In collectionWord
                            itemname = itemname.Replace(" " + clt + " ", "")
                        Next
                        Dim itemnameCollection = itemname.Trim()
                        If Not String.IsNullOrEmpty(itemnameCollection) Then
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND collectionname:""{1}"")", itemnameCollection, collection.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND collectionname:""{1}"")", itemnameCollection, collection.Replace(" ", "")))
                        Else
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(collectionname:""{0}"")", collection), String.Format(" OR (collectionname:""{0}"")", collection))
                        End If
                        For Each tone As String In toneSynonym.Keys
                            Dim toneword = toneSynonym(tone).Split(" ").Where(Function(i) i.Length > 0)
                            For Each t As String In toneword
                                itemname = itemname.Replace(" " + t + " ", "")
                            Next
                            Dim itemnameTone = itemname.Trim()
                            If Not String.IsNullOrEmpty(itemnameTone) Then
                                result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND tonename:""{1}"")", itemnameTone, tone.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND tonename:""{1}"")", itemnameTone, tone.Replace(" ", "")))
                            Else
                                result &= IIf(String.IsNullOrEmpty(result), String.Format("(tonename:""{0}"")", tone.Replace(" ", "")), String.Format(" OR (tonename:""{0}"")", tone.Replace(" ", "")))
                            End If
                            For Each shade As String In shadeOfficial
                                Dim shadeWord = shade.Split(" ").Where(Function(i) i.Length > 0)
                                For Each s As String In shadeWord
                                    itemname = itemname.Replace(" " + s + " ", "")
                                Next
                                Dim itemnameShade = itemname.Trim()
                                If Not String.IsNullOrEmpty(itemnameShade) Then
                                    result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND shadename:""{1})""", itemnameShade, shade.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")))
                                Else
                                    result &= IIf(String.IsNullOrEmpty(result), String.Format("(shadename:""{0}"")", shade.Replace(" ", "")), String.Format(" OR (shadename:""{0}"")", shade.Replace(" ", "")))
                                End If
                            Next
                        Next
                    Next
                Next
            End If

            If collectionOfficial.Count > 0 Then
                For Each collection As String In collectionOfficial
                    Dim itemname As String = " " + kw.Replace(" ", "  ") + " "
                    Dim collectionWord = collection.Split(" ").Where(Function(i) i.Length > 0)
                    For Each clt As String In collectionWord
                        itemname = itemname.Replace(" " + clt + " ", "")
                    Next
                    Dim itemnameCollection = itemname.Trim()
                    If Not String.IsNullOrEmpty(itemnameCollection) Then
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND collectionname:""{1}"")", itemnameCollection, collection.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND collectionname:""{1}"")", itemnameCollection, collection.Replace(" ", "")))
                    Else
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(collectionname:""{0}"")", collection.Replace(" ", "")), String.Format(" OR (collectionname:""{0}"")", collection.Replace(" ", "")))
                    End If
                    For Each tone As String In toneSynonym.Keys
                        Dim toneword = toneSynonym(tone).Split(" ").Where(Function(i) i.Length > 0)
                        For Each t As String In toneword
                            itemname = itemname.Replace(" " + t + " ", "")
                        Next
                        Dim itemnameTone = itemname.Trim()
                        If Not String.IsNullOrEmpty(itemnameTone) Then
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND tonename:""{1})""", itemnameTone, tone.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND tonename:""{1}"")", itemnameTone, tone.Replace(" ", "")))
                        Else
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(tonename:""{0}"")", tone), String.Format(" OR (tonename:""{0}"")", tone))

                        End If
                        For Each shade As String In shadeOfficial
                            Dim shadeWord = shade.Split(" ").Where(Function(i) i.Length > 0)
                            For Each s As String In shadeWord
                                itemname = itemname.Replace(" " + s + " ", "")
                            Next
                            Dim itemnameShade = itemname.Trim()
                            If Not String.IsNullOrEmpty(itemnameShade) Then
                                result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, tone.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")))
                            Else
                                result &= IIf(String.IsNullOrEmpty(result), String.Format("(shadename:""{0}"")", tone.Replace(" ", "")), String.Format(" OR (shadename:""{0}"")", shade.Replace(" ", "")))
                            End If
                        Next
                    Next
                Next
            End If

            If toneSynonym.Count > 0 Then
                For Each tone As String In toneSynonym.Keys
                    Dim itemname As String = " " + kw.Replace(" ", "  ") + " "
                    Dim toneword = toneSynonym(tone).Split(" ").Where(Function(i) i.Length > 0)
                    For Each t As String In toneword
                        itemname = itemname.Replace(" " + t + " ", "")
                    Next
                    Dim itemnameTone = itemname.Trim()
                    If Not String.IsNullOrEmpty(itemnameTone) Then
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND tonename:""{1}"")", itemnameTone, tone.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND tonename:""{1}"")", itemnameTone, tone.Replace(" ", "")))
                    Else
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(tonename:""{0}"")", tone.Replace(" ", "")), String.Format(" OR (tonename:""{0}"")", tone.Replace(" ", "")))
                    End If
                    For Each shade As String In shadeOfficial
                        Dim shadeWord = shade.Split(" ").Where(Function(i) i.Length > 0)
                        For Each s As String In shadeWord
                            itemname = itemname.Replace(" " + s + " ", "")
                        Next
                        Dim itemnameShade = itemname.Trim()
                        If Not String.IsNullOrEmpty(itemnameShade) Then
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")))
                        Else
                            result &= IIf(String.IsNullOrEmpty(result), String.Format("(shadename:""{0}"")", shade.Replace(" ", "")), String.Format(" OR (shadename:""{0}"")", shade.Replace(" ", "")))

                        End If
                    Next
                Next
            End If

            If shadeOfficial.Count > 0 Then
                For Each shade As String In shadeOfficial
                    Dim itemname As String = " " + kw.Replace(" ", "  ") + " "
                    Dim shadeWord = shade.Split(" ").Where(Function(i) i.Length > 0)
                    For Each s As String In shadeWord
                        itemname = itemname.Replace(" " + s + " ", "")
                    Next
                    Dim itemnameShade = itemname.Trim()
                    If Not String.IsNullOrEmpty(itemnameShade) Then
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")), String.Format(" OR (itemnamepricedesc:""{0}""~10000 AND shadename:""{1}"")", itemnameShade, shade.Replace(" ", "")))
                    Else
                        result &= IIf(String.IsNullOrEmpty(result), String.Format("(shadename:""{0}"")", shade.Replace(" ", "")), String.Format(" OR (shadename:""{0}"")", shade.Replace(" ", "")))

                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(result) Then
                Return String.Format(" OR (({0})^=0.00000001)", result.Replace("&", "%26"))
            End If
        Catch ex As Exception
            Dim a = ex
        End Try

        Return String.Empty
    End Function
    Private Shared Function exceptKw(ByVal kw As String, ByVal autocomplete As Boolean) As String
        If kw.Length > 30 Then
            Return ""
        End If
        If Not kw.Contains("non ") Then
            Dim arrStr As String() = kw.Split(" ").Where(Function(i) i.Trim().Length > 0).Select(Function(i) "non " + i).ToArray()
            Dim query As String = String.Empty
            For Each item As String In arrStr
                If item = "non -" Then
                    Continue For
                End If
                query &= String.Format("{1} -itemnamepricedesc:""{0}"" OR -longdesc:""{0}"" ", item, IIf(String.IsNullOrEmpty(query), "", " OR"))
                If autocomplete Then
                    query &= String.Format(" OR (-autocompleteitemname:""{0}"" AND -itemnamepricedesc:""{0}"") Or (-autocompletelongdesc:""{0}"" AND -longdesc:""{0}"")", item)
                End If
            Next

            Return String.Format("({0}) AND ", query)
        End If
        Return ""
    End Function

    Private Shared Function buildSplitPhase(ByVal kw As String) As String
        If Not kw.Contains(" ") Then
            Return String.Empty
        End If



        Dim fieldSearch As String = String.Empty
        Dim listKeyword As List(Of String) = New List(Of String)()
        Dim index As Int16 = 0
        Dim listStr As String() = Utility.Common.CheckSearchKeyword(kw).Trim().Split(" ").Where(Function(i) i.Trim().Length > 0).Distinct().ToArray()

        If listStr.Length = 2 Then
            Dim mainString As String = String.Empty, subString As String = String.Empty
            If Regex.IsMatch(listStr(0), "^[0-9 ]+$") Then
                mainString = listStr(1)
            ElseIf Regex.IsMatch(listStr(1), "^[0-9 ]+$") Then
                mainString = listStr(0)
            Else
                Dim modelIn As java.io.InputStream = New java.io.FileInputStream(Utility.ConfigData.LuceneItemIndexPath + "//en-pos-maxent.bin")
                Dim model As opennlp.tools.postag.POSModel = New opennlp.tools.postag.POSModel(modelIn)
                Dim tagger As opennlp.tools.postag.POSTaggerME = New opennlp.tools.postag.POSTaggerME(model)
                Dim tags As String() = tagger.tag(listStr)

                If tags.Count() = 2 Then
                    If tags.All(Function(k) k = "NN") Then
                        mainString = listStr(0)
                        subString = listStr(1)
                    ElseIf tags.Any(Function(k) k <> "NN") Then
                        mainString = IIf(listStr(0) = "NN", listStr(0), listStr(1))
                    End If

                End If
            End If

            fieldSearch = String.Format("itemnamepricedesc:{0}", mainString)

            If Not String.IsNullOrEmpty(subString) Then
                fieldSearch = fieldSearch & String.Format(" OR itemnamepricedesc:{0}", subString)
            End If

        Else
            For indexFirst = 0 To listStr.Length - 2
                For indexSecond = indexFirst + 1 To listStr.Length - 1
                    Dim firstString As String = listStr(indexFirst)
                    Dim secondString As String = listStr(indexSecond)

                    If Not String.IsNullOrEmpty(fieldSearch) Then
                        fieldSearch = fieldSearch & String.Format(" OR itemnamepricedesc:(""{1}"")^=10000 OR ({0})^=100 OR longdesc:(""{1}"")^=0.1", buildField("itemnamepricedesc", firstString, secondString), firstString + " " + secondString)
                    Else
                        fieldSearch = String.Format("itemnamepricedesc:(""{1}"")^=10000 OR ({0})^=100 OR longdesc:(""{1}"")^=0.1", buildField("itemnamepricedesc", firstString, secondString), firstString + " " + secondString)
                    End If

                Next
            Next
        End If
        Return "hasredirect:false AND (" + fieldSearch + ")"
    End Function
    Private Shared Function buildMergeSearch(ByVal kw As String, Optional ByVal buildAutocomplete As Boolean = False) As String

        Dim fieldSearch As String = String.Empty
        Dim listKeyword As List(Of String) = New List(Of String)()
        Dim index As Int16 = 0
        Dim listStr As String() = kw.Trim().Split(" ").Where(Function(i) i.Trim().Length > 0).ToArray()
        If listStr.Length = 2 Then
            fieldSearch = String.Format("itemnamepricedesc:""{0}""^=5 OR longdesc:""{0}""^=0.0001", kw.Replace(" ", ""))
        Else
            For Each str As String In listStr
                index = kw.IndexOf(str, index)
                If kw.Length > index + str.Length Then
                    listKeyword.Add(kw.Remove(index + str.Length, 1))
                End If
            Next

            listKeyword.Add(kw.Replace(" ", ""))
            If listKeyword.Count > 0 Then
                Dim longestKw As String = listKeyword.OrderByDescending(Function(x) x.Length).First()
                fieldSearch = String.Format("itemnamepricedesc:""{0}""^=5 OR longdesc:""{0}""^=0.0001 OR itemnamepricedesc:""{0}""~10000^=3 OR longdesc:""{0}""~10000^=0.00015", longestKw)
                listKeyword.Remove(longestKw)

                index = listKeyword.Count - 1
                While (index >= 0)
                    Dim keyword As String = listKeyword.Item(index)
                    fieldSearch = fieldSearch & String.Format(" OR itemnamepricedesc:""{0}""^=1 OR longdesc:""{0}""^=0.00001", keyword)
                    index = index - 1
                End While
            End If
        End If

        Return fieldSearch

    End Function

    Private Shared m_counter As Integer = 0
    Private Shared fieldList As String = "collectionid,toneid,shadeid,hasredirect,urlredirect,hotitems,newitems,bestsellers,sku, urlcode,itemname,image,brandid,price,pricedesc,countreview,averagereview,qtyonhand,isspecialorder,acceptingorder,mostpopularreview"
    Public Shared Function SearchItem(ByVal textSearch As String, ByVal orderBy As String, ByVal orderExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal DepartmentId As String, ByVal brandId As Integer, ByVal PriceDesc As String, ByVal Rating As String, ByVal loadMore As Boolean, ByVal currentFilterActive As String, ByVal collectionid As Integer, ByVal toneid As Integer, shadeid As Integer, ByVal filterDefault As String, Optional ByVal getKwRedirect As Boolean = False) As DataLayer.StoreItemCollection
        Dim lstItem As New StoreItemCollection
        Dim url As String = String.Empty
        Try
            If pageIndex = 1 Then
                m_counter = 0
            End If
            Dim temp As String
            temp = textSearch.Trim().ToLower()
            temp = temp.Replace("*", "").Replace("?", "").Replace("#", "").Replace("""", "").Replace(" or ", "").Replace(" and ", "").Replace("\", "")

            If temp.Contains("Search by keyword or item") Then
                temp = ""
            End If


            If String.IsNullOrEmpty(temp) Or temp.Length = 1 OrElse SolrHelper.keywordNoResult.Any(Function(i) i.Replace(" ", "") = temp.Replace(" ", "")) Then
                HttpContext.Current.Session("searchCountCategory") = Nothing
                HttpContext.Current.Session("searchCountBrand") = Nothing
                HttpContext.Current.Session("searchCountRating") = Nothing
                HttpContext.Current.Session("searchCountPrice") = Nothing
                HttpContext.Current.Session("searchBrandId") = String.Empty
                HttpContext.Current.Session("searchDepartmentId") = String.Empty
                HttpContext.Current.Session("searchHasToprate") = Nothing
                HttpContext.Current.Session("searchCountCollection") = Nothing
                HttpContext.Current.Session("searchCountTone") = Nothing
                HttpContext.Current.Session("searchCountCategory") = Nothing
                Return lstItem

            End If

            Dim lstBrandId As String = ","
            Dim lstCountBrand As String = ","
            Dim lstDepartmentId As String = ","
            Dim lstCountCategory As String = ","
            Dim lstCollectionId As String = ","
            Dim lstToneId As String = ","
            Dim lstShadeId As String = ","

            'filter with Collection
            Dim CollectionIdfq As String = String.Empty
            If (collectionid > 0) Then
                CollectionIdfq = "collectionid:" + collectionid.ToString()
            End If

            'filter with tone
            Dim ToneIdfq As String = String.Empty
            If (toneid > 0) Then
                ToneIdfq = "toneid:" + toneid.ToString()
            End If

            'filter with shade
            Dim ShadeIdfq As String = String.Empty
            If (shadeid > 0) Then
                ShadeIdfq = "shadeid:" + shadeid.ToString()
            End If

            ''filter with BrandId
            Dim BrandIdfq As String = String.Empty
            If (brandId > 0) Then
                BrandIdfq = "brandid:" + brandId.ToString()
            End If

            'filter with PriceDesc
            Dim Pricefq As String = String.Empty
            If (Not PriceDesc Is Nothing And Not String.IsNullOrEmpty(PriceDesc)) Then
                Dim min As Double = GetMinMaxValue(PriceDesc, False)
                Dim max As Double = GetMinMaxValue(PriceDesc, True)
                Pricefq = "pricefilter:[" + IIf(min = 0, "*", min.ToString()) + " TO " + IIf(max = 0, "*", max.ToString()) + " }"
            End If

            'filter with rating
            Dim Rattingfq As String = String.Empty
            If (Not Rating Is Nothing And Not String.IsNullOrEmpty(Rating)) Then
                Dim min As Double = GetMinMaxValue(Rating, False)
                Dim max As Double = GetMinMaxValue(Rating, True)
                Rattingfq = "averagereview:[" + IIf(min = 0, "*", min.ToString()) + " TO " + IIf(max = 0, "*", max.ToString()) + " }"
            End If

            'filter with category
            Dim Categoryfq As String = String.Empty
            If (Not String.IsNullOrEmpty(DepartmentId) AndAlso DepartmentId <> "0") Then
                Categoryfq = "departmentid:" + DepartmentId.ToString()
            End If

            'Add facet collection
            Dim facetCollectionQuery As String = "&facet.field=collectionid&facet.mincount=1"
            'Add facet tone
            Dim facetToneQuery As String = "&facet.field=toneid&facet.mincount=1"
            'Add facet shade
            Dim facetShadeQuery As String = "&facet.field=shadeid&facet.mincount=1"

            'Add facet category
            Dim facetCategoryQuery As String = "&facet.field=departmentid&facet.mincount=1"

            'Add facet brandid
            Dim facetBrandidQuery As String = "&facet.field=brandid&facet.mincount=1"


            'Add facet ratting.
            Dim facetRattingQuery As String = "facet.interval=averagereview"
            Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Rating))
            Dim len As Integer = arr.Length - 1
            For i As Integer = 0 To len

                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                If (i = len) Then
                    min = CType(arr(i), Double)
                    facetRattingQuery &= String.Format("&f.averagereview.facet.interval.set=[{0},*]", min)
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    facetRattingQuery &= String.Format("&f.averagereview.facet.interval.set=[{0},{1})", IIf(min = 0, "*", min.ToString()), max)
                End If

            Next

            'Add facet price ratting.
            Dim facetPriceQuery As String = "facet.interval=pricefilter"
            arr = [Enum].GetValues(GetType(Utility.Common.Price))
            len = arr.Length - 1
            For i As Integer = 0 To len
                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                Dim p As Boolean = False
                If (i = 0) Then
                    max = CType(arr(i + 1), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[*,{0})", max)
                ElseIf (i = len) Then
                    min = CType(arr(i), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[{0},*)", min)
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[{0},{1})", min, max)
                End If
            Next

            'Add facet newitems for sort
            Dim facetNewItem As String = "&facet.field=newitems&facet.mincount=1"

            'Final query
            Dim q As String = buildSearch(temp, getKwRedirect, IIf(filterDefault.Contains("isinbulk:true"), True, False))


            'add filter
            Dim fq As String = String.Empty
            If (Not String.IsNullOrEmpty(filterDefault)) Then
                fq &= "&fq=(" & filterDefault + ")"
            End If
            If (Not String.IsNullOrEmpty(BrandIdfq)) Then
                fq &= "&fq=" & BrandIdfq
            End If

            If (Not String.IsNullOrEmpty(Pricefq)) Then
                fq &= "&fq=" & Pricefq
            End If

            If (Not String.IsNullOrEmpty(Rattingfq)) Then
                fq &= "&fq=" & Rattingfq
            End If

            If (Not String.IsNullOrEmpty(Categoryfq)) Then
                fq &= "&fq=" & Categoryfq
            End If
            If (Not String.IsNullOrEmpty(CollectionIdfq)) Then
                fq &= "&fq=" & CollectionIdfq
            End If
            If (Not String.IsNullOrEmpty(ToneIdfq)) Then
                fq &= "&fq=" & ToneIdfq
            End If
            If (Not String.IsNullOrEmpty(ShadeIdfq)) Then
                fq &= "&fq=" & ShadeIdfq
            End If


            'Add facet
            Dim facetStr As String = String.Empty
            If Not String.IsNullOrEmpty(facetCollectionQuery) Then
                facetStr &= "&" & facetCollectionQuery
            End If
            If Not String.IsNullOrEmpty(facetToneQuery) Then
                facetStr &= "&" & facetToneQuery
            End If
            If Not String.IsNullOrEmpty(facetShadeQuery) Then
                facetStr &= "&" & facetShadeQuery
            End If
            If Not String.IsNullOrEmpty(facetCategoryQuery) Then
                facetStr &= "&" & facetCategoryQuery
            End If
            If Not String.IsNullOrEmpty(facetBrandidQuery) Then
                facetStr &= "&" & facetBrandidQuery
            End If
            If Not String.IsNullOrEmpty(facetPriceQuery) Then
                facetStr &= "&" & facetPriceQuery
            End If
            If Not String.IsNullOrEmpty(facetRattingQuery) Then
                facetStr &= "&" & facetRattingQuery
            End If
            If Not String.IsNullOrEmpty(facetNewItem) Then
                facetStr &= "&" & facetNewItem
            End If

            If Not String.IsNullOrEmpty(facetStr) Then
                q &= "&facet=true" & facetStr
            End If

            url = String.Empty
            Dim sort As String = "&sort="
            Dim xmlDoc As System.Xml.XmlDocument = New XmlDocument()
            Dim hits As XmlNodeList = Nothing
            If orderBy Is Nothing Then
                'search with sort RELEVANCE
                url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", pageSize, (pageIndex - 1) * pageSize) & fq
                'Dim request As HttpWebRequest = WebRequest.Create(Utility.ConfigData.GlobalRefererName & url)
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    hits = xmlDoc.SelectNodes("/response/result/doc")
                End If
            Else
                'search with sort
                If orderBy = "price" Then
                    sort &= "pricefilter asc"
                ElseIf orderBy = "pricehigh" Then
                    sort &= "pricefilter desc"
                ElseIf orderBy = "most-popular-review" Then
                    sort &= "mostpopularreview desc"
                ElseIf orderBy = "top-rated" Then
                    sort &= "toprate desc"
                ElseIf orderBy = "new-items" Then
                    sort &= " newitems desc, newuntil desc , itemname asc"
                ElseIf orderBy = "sku" Then
                    sort &= "sku asc"
                End If

                url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", pageSize, (pageIndex - 1) * pageSize, fieldList) & fq + IIf(sort = "&sort=", String.Empty, sort)
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    hits = xmlDoc.SelectNodes("/response/result/doc")
                End If

            End If

            Dim beginRow As Integer = (pageIndex - 1) * pageSize
            Dim endRow As Integer = beginRow + pageSize - 1
            Dim bId As Integer
            Dim cId As String
            Dim hasNew As Boolean = False
            Dim hasBestSale As Boolean = False
            Dim hasHots As Boolean = False
            Dim ishot As Boolean = False
            Dim isNew As Boolean = False
            Dim isBestSeller As Boolean = False
            Dim hasToprate As Boolean = False
            Dim mostTopRate As Integer = 0

            Dim collectionIdL As Integer
            Dim toneIdL As Integer
            Dim shadeIdL As Integer

            Dim totalCount As Integer = 0
            Integer.TryParse(xmlDoc.SelectNodes("/response/result").Item(0).Attributes("numFound").Value, total)
            If Not getKwRedirect Then
                Integer.TryParse(xmlDoc.SelectNodes("/response/result").Item(0).Attributes("numFound").Value, totalCount)
                If totalCount > 0 Then
                    If (beginRow >= totalCount) Then
                        beginRow = totalCount - 1

                    End If
                    If (endRow >= totalCount) Then
                        endRow = totalCount - 1

                    End If
                End If
            End If

            If total = 0 AndAlso Not getKwRedirect Then
                If Not getKwRedirect Then
                    'check ip allow access
                    Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                    If Not KeywordIPExcludeRow.CheckNotAllowTrackingKeyword(ip) Then
                        'Utility.CacheUtils.SetCache(temp, totalCount, 10, New CacheItemRemovedCallback(AddressOf loggingKw))
                        loggingKw(temp, totalCount, CacheItemRemovedReason.Expired)
                    End If
                End If

                If temp.Split(" ").Count() >= 2 Then
                    If filterDefault.Contains("isinbulk:true") Then
                        Dim keywordBuyInBulk As String = removeString(temp)
                        Dim listKeyWordBuyInBulk As IEnumerable(Of KeywordSynonymRow) = KeywordSynonymRow.GetBuyInBulkSynonym(keywordBuyInBulk)
                        If listKeyWordBuyInBulk.Count > 0 Then
                            For Each synKw As KeywordSynonymRow In listKeyWordBuyInBulk.OrderByDescending(Function(i) i.KeywordSynonymName.Length)
                                keywordBuyInBulk = ((" " + keywordBuyInBulk + " ").Replace(synKw.KeywordSynonymName, "")).Replace("  ", " ").Trim()
                            Next

                            If String.IsNullOrEmpty(keywordBuyInBulk.Trim()) Then
                                keywordBuyInBulk = "*"
                            End If
                            temp = keywordBuyInBulk
                        End If
                    End If
                    temp = removeString(temp)
                    Dim qS = buildSplitPhase(temp)
                    If Not String.IsNullOrEmpty(temp) Then
                        q = Utility.ConfigData.SolrServerURL + qS
                        If Not String.IsNullOrEmpty(facetStr) Then
                            q &= "&facet=true" & facetStr
                        End If

                        If orderBy Is Nothing Then
                            url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", pageSize, (pageIndex - 1) * pageSize) & fq
                            xmlDoc = getResponse(url)
                            If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                                hits = xmlDoc.SelectNodes("/response/result/doc")
                            End If
                        Else
                            url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", pageSize, (pageIndex - 1) * pageSize, fieldList) & fq + IIf(sort = "&sort=", String.Empty, sort)
                            xmlDoc = getResponse(url)
                            If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                                hits = xmlDoc.SelectNodes("/response/result/doc")
                            End If

                        End If

                        Integer.TryParse(xmlDoc.SelectNodes("/response/result").Item(0).Attributes("numFound").Value, total)
                        If Not getKwRedirect Then
                            Integer.TryParse(xmlDoc.SelectNodes("/response/result").Item(0).Attributes("numFound").Value, totalCount)
                            If totalCount > 0 Then
                                If (beginRow >= totalCount) Then
                                    beginRow = totalCount - 1

                                End If
                                If (endRow >= totalCount) Then
                                    endRow = totalCount - 1

                                End If
                            End If
                        End If
                    End If

                    If total = 0 Then
                        HttpContext.Current.Session("searchCountCategory") = Nothing
                        HttpContext.Current.Session("searchCountBrand") = Nothing
                        HttpContext.Current.Session("searchCountRating") = Nothing
                        HttpContext.Current.Session("searchCountPrice") = Nothing
                        HttpContext.Current.Session("searchBrandId") = String.Empty
                        HttpContext.Current.Session("searchDepartmentId") = String.Empty
                        HttpContext.Current.Session("searchHasToprate") = Nothing
                        HttpContext.Current.Session("searchCountCollection") = Nothing
                        HttpContext.Current.Session("searchCountTone") = Nothing
                        HttpContext.Current.Session("searchCountCategory") = Nothing

                        Return Nothing
                    End If

                Else
                    HttpContext.Current.Session("searchCountCategory") = Nothing
                    HttpContext.Current.Session("searchCountBrand") = Nothing
                    HttpContext.Current.Session("searchCountRating") = Nothing
                    HttpContext.Current.Session("searchCountPrice") = Nothing
                    HttpContext.Current.Session("searchBrandId") = String.Empty
                    HttpContext.Current.Session("searchDepartmentId") = String.Empty
                    HttpContext.Current.Session("searchHasToprate") = Nothing
                    HttpContext.Current.Session("searchCountCollection") = Nothing
                    HttpContext.Current.Session("searchCountTone") = Nothing
                    HttpContext.Current.Session("searchCountCategory") = Nothing

                    If Not getKwRedirect Then
                        'check ip allow access
                        Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                        If Not KeywordIPExcludeRow.CheckNotAllowTrackingKeyword(ip) Then
                            'Utility.CacheUtils.SetCache(temp, totalCount, 10, New CacheItemRemovedCallback(AddressOf loggingKw))
                            loggingKw(temp, totalCount, CacheItemRemovedReason.Expired)
                        End If
                    End If

                    Return Nothing
                End If
            Else
                loggingKw(temp, total, CacheItemRemovedReason.Expired)
            End If


            If loadMore Then
                Dim memberID As Integer = Utility.Common.GetCurrentMemberId()
                Dim orderID As Integer = Utility.Common.GetCurrentOrderId()

                Dim listItemIds As String = String.Empty

                For i As Integer = 0 To pageSize - 1
                    Dim doc As XmlNode = hits(i)
                    If doc Is Nothing Then
                        Continue For
                    End If
                    m_counter += 1
                    Dim item As New StoreItemRow
                    Try
                        Integer.TryParse(getXmlNodeLucene("id", doc), item.ItemId)
                        listItemIds += item.ItemId.ToString() + ","

                        Integer.TryParse(getXmlNodeLucene("brandid", doc), item.BrandId)
                        item.SKU = getXmlNodeLucene("sku", doc)
                        item.URLCode = getXmlNodeLucene("urlcode", doc)
                        'item.ShortDesc = getXmlNodeLucene("ShortDesc", doc)
                        item.ItemName = getXmlNodeLucene("itemname", doc) '' & " | " & hits.Score(i)
                        item.LastDepartmentName = getXmlNodeLucene("lastdepartmentname", doc)
                        item.Image = getXmlNodeLucene("image", doc)
                        Integer.TryParse(getXmlNodeLucene("brandid", doc), item.BrandId)
                        Double.TryParse(getXmlNodeLucene("price", doc), item.LowSalePrice)
                        item.ShowPrice = getXmlNodeLucene("pricedesc", doc)

                        isNew = CBool(getXmlNodeLucene("newitems", doc))
                        item.IsNew = isNew
                        item.IsNewTrue = isNew
                        item.IsHot = ishot
                        item.IsBestSeller = isBestSeller
                        Integer.TryParse(getXmlNodeLucene("countreview", doc), item.CountReview)
                        Double.TryParse(getXmlNodeLucene("averagereview", doc), item.AverageReview)
                        Integer.TryParse(getXmlNodeLucene("qtyonhand", doc), item.QtyOnHand)
                        item.IsSpecialOrder = CBool(getXmlNodeLucene("isspecialorder", doc))
                        Integer.TryParse(getXmlNodeLucene("acceptingorder", doc), item.AcceptingOrder)
                        'StoreItemRow.GetItemSearchInfor(item, memberID, orderID)

                        Double.TryParse(getXmlNodeLucene("lowpriceold", doc), item.LowPrice)
                        Double.TryParse(getXmlNodeLucene("highprice", doc), item.HighPrice)
                        Double.TryParse(getXmlNodeLucene("caseprice", doc), item.CasePrice)
                        Double.TryParse(getXmlNodeLucene("lowsaleprice", doc), item.LowSalePrice)

                        DateTime.TryParse(getXmlNodeLucene("newuntil", doc), item.NewUntil)
                        Boolean.TryParse(getXmlNodeLucene("isfreeshipping", doc), item.IsFreeShipping)
                        Boolean.TryParse(getXmlNodeLucene("isfreesample", doc), item.IsFreeSample)

                        item.ItemName2 = item.ItemName
                        Double.TryParse(getXmlNodeLucene("price", doc), item.Price)
                        Dim priceDesc_ As String = getXmlNodeLucene("pricedescsearch", doc)
                        item.PriceDesc = IIf(String.IsNullOrEmpty(priceDesc_), Nothing, priceDesc_)
                        Dim choiname = getXmlNodeLucene("choicename", doc)
                        If Not String.IsNullOrEmpty(choiname) Then
                            item.ItemName &= " - " & choiname

                            If Not item.ItemName2.Contains(choiname) Then
                                item.ItemName2 &= " - " & choiname
                            End If
                        End If
                        If item.PriceDesc <> Nothing AndAlso Not item.ItemName2.Contains(item.PriceDesc) AndAlso choiname.Trim().Replace(" ", "") <> priceDesc_.ToString().Trim().Replace(" ", "") Then
                            item.ItemName2 &= " - " & priceDesc_
                        End If

                        Double.TryParse(getXmlNodeLucene("itemgroupid", doc), item.ItemGroupId)

                        Integer.TryParse(getXmlNodeLucene("caseqty", doc), item.CaseQty)
                        Integer.TryParse(getXmlNodeLucene("isvariance", doc), item.IsVariance)
                        Integer.TryParse(getXmlNodeLucene("itemgroupid", doc), item.ItemGroupId)
                        Integer.TryParse(getXmlNodeLucene("isfreegift", doc), item.IsFreeGift)

                        Boolean.TryParse(getXmlNodeLucene("isflammable", doc), item.IsFlammable)
                        If memberID = 0 Then
                            Dim checkLoginViewPrice As Boolean = False
                            If Boolean.TryParse(getXmlNodeLucene("enableisloginviewprice", doc), checkLoginViewPrice) Then
                                If checkLoginViewPrice Then
                                    item.IsLoginViewPrice = True
                                End If
                            End If
                        Else
                            item.IsLoginViewPrice = False
                        End If

                        item.itemIndex = m_counter
                        lstItem.Add(item)

                        'log when press back button.
                        'check ip allow access
                        If HttpContext.Current.Session("KeywordSearchId") = Nothing Then
                            Dim kwSearchid As Integer = getKwSearchIdFromCache(temp)
                            If kwSearchid > 0 Then
                                HttpContext.Current.Session("KeywordSearchId") = kwSearchid
                            End If

                        End If
                    Catch ex As Exception

                    End Try
                Next
                StoreItemRow.getInfoSearchItem(listItemIds, memberID, orderID, Utility.Common.GetCurrentCustomerPriceGroupId(), lstItem)

            Else
                'Log.
                If Not getKwRedirect Then
                    'check ip allow access
                    Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                    If Not KeywordIPExcludeRow.CheckNotAllowTrackingKeyword(ip) Then
                        'Utility.CacheUtils.SetCache(temp, totalCount, 10, New CacheItemRemovedCallback(AddressOf loggingKw))
                        loggingKw(temp, totalCount, CacheItemRemovedReason.Expired)
                    End If
                End If
                total = totalCount
                'keyword redirect.
                Dim doc As XmlNode = hits(0)

                If getKwRedirect Then
                    Dim kwRedirect As String = getXmlNodeLucene("itemname", doc)
                    If kwRedirect.Split(" ").Length = temp.Split(" ").Length Then
                        Dim urlRedirect As String = getXmlNodeLucene("urlredirect", doc)
                        If Not String.IsNullOrEmpty(urlRedirect) Then
                            If Not keywordRedirect.ContainsKey(temp) Then
                                keywordRedirect.Add(temp, urlRedirect)
                            End If
                            Return Nothing
                        End If
                    End If
                End If

                Dim i As Integer = 0
                While (i < hits.Count)
                    Try

                        doc = hits(i)

                        Integer.TryParse(getXmlNodeLucene("brandid", doc), bId)
                        Integer.TryParse(getXmlNodeLucene("collectionid", doc), collectionIdL)
                        Integer.TryParse(getXmlNodeLucene("toneid", doc), toneIdL)
                        Integer.TryParse(getXmlNodeLucene("shadeid", doc), shadeIdL)

                        Integer.TryParse(getXmlNodeLucene("mostpopularreview", doc), mostTopRate)

                        cId = getXmlNodeLucene("departmentid", doc)

                        If (getXmlNodeLucene("hotitems", doc) = "1") Then
                            ishot = True
                        Else
                            ishot = False
                        End If
                        If (getXmlNodeLucene("newitems", doc) = "true") Then
                            isNew = True
                        Else
                            isNew = False
                        End If
                        If (getXmlNodeLucene("bestsellers", doc) = "1") Then
                            isBestSeller = True
                        Else
                            isBestSeller = False
                        End If
                        If mostTopRate > 0 And hasToprate = False Then
                            hasToprate = True
                        End If
                        If (i = 0) Then
                            m_counter += 1
                            Dim item As New StoreItemRow
                            item.ItemName = getXmlNodeLucene("itemname", doc)
                            item.SKU = getXmlNodeLucene("sku", doc)
                            item.URLCode = getXmlNodeLucene("urlcode", doc)
                            item.ItemId = getXmlNodeLucene("id", doc)
                            If Not lstBrandId.Contains("," & bId & ",") Then
                                lstBrandId = lstBrandId & bId & ","
                            End If
                            If Not lstCollectionId.Contains("," & collectionIdL & ",") Then
                                lstCollectionId = collectionid & collectionIdL & ","
                            End If
                            If Not lstToneId.Contains("," & toneIdL & ",") Then
                                lstToneId = lstToneId & toneIdL & ","
                            End If
                            If Not lstShadeId.Contains("," & shadeIdL & ",") Then
                                lstShadeId = lstShadeId & shadeIdL & ","
                            End If

                            lstDepartmentId = lstDepartmentId & cId & ","
                            lstItem.Add(item)
                        Else
                            'If loadMore Then
                            '    Exit While
                            'End If
                            If Not lstBrandId.Contains("," & bId & ",") Then
                                lstBrandId = lstBrandId & bId & ","
                            End If

                            'lstDepartmentId = lstDepartmentId & cId & ","
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
                    Catch ex As Exception
                        i = i + 1
                    End Try
                End While

                If Not hasNew Then
                    hasNew = getFacetNewItems(xmlDoc)
                End If
                HttpContext.Current.Session("searchHasNew") = hasNew

                'Fill category
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    Dim listCache As Dictionary(Of String, String) = New Dictionary(Of String, String)(4)

                    'get cateagories
                    Dim categories As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='departmentid']/int")
                    If categories.Count > 0 Then
                        For i = 0 To categories.Count - 1
                            Dim DepartmentIdStr As String = categories.ItemOf(i).Attributes("name").Value
                            If Not lstDepartmentId.Contains("," & DepartmentIdStr & ",") Then
                                lstDepartmentId = lstDepartmentId & DepartmentIdStr & ","
                            End If
                        Next
                    End If
                    'HttpContext.Current.Session.Add("Lucene_lstDepartmentId_" + textSearch, lstDepartmentId)
                End If

                'Fill colection
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get collections
                    Dim collections As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='collectionid']/int")
                    If collections.Count > 0 Then
                        For i = 0 To collections.Count - 1
                            Dim CollectionIdStr As String = collections.ItemOf(i).Attributes("name").Value
                            If Not lstCollectionId.Contains("," & CollectionIdStr & ",") Then
                                lstCollectionId = lstCollectionId & CollectionIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill tone
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get tones
                    Dim tones As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='toneid']/int")
                    If tones.Count > 0 Then
                        For i = 0 To tones.Count - 1
                            Dim ToneIdStr As String = tones.ItemOf(i).Attributes("name").Value
                            If Not lstToneId.Contains("," & ToneIdStr & ",") Then
                                lstToneId = lstToneId & ToneIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill shade
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get shade
                    Dim shades As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='shadeid']/int")
                    If shades.Count > 0 Then
                        For i = 0 To shades.Count - 1
                            Dim ShadeIdStr As String = shades.ItemOf(i).Attributes("name").Value
                            If Not lstShadeId.Contains("," & ShadeIdStr & ",") Then
                                lstShadeId = lstShadeId & ShadeIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill brandid
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    Dim listCache As Dictionary(Of String, String) = New Dictionary(Of String, String)(4)

                    'get brandid
                    Dim brands As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='brandid']/int")
                    If brands.Count > 0 Then
                        For i = 0 To brands.Count - 1
                            Dim brandIdStr As String = brands.ItemOf(i).Attributes("name").Value
                            If Not lstBrandId.Contains("," & brandIdStr & ",") Then
                                lstBrandId = lstBrandId & brandIdStr & ","
                            End If
                        Next
                    End If
                End If

                If (hits.Count > 0) Then
                    HttpContext.Current.Session("searchBrandId") = lstBrandId
                    HttpContext.Current.Session("searchDepartmentId") = lstDepartmentId
                    HttpContext.Current.Session("searchCollectionId") = lstCollectionId
                    HttpContext.Current.Session("searchToneId") = lstToneId
                    HttpContext.Current.Session("searchShadeId") = lstShadeId

                    Dim dic As Dictionary(Of String, String) = Nothing
                    If String.IsNullOrEmpty(currentFilterActive) Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf currentFilterActive.Equals("toneid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountTone") = dic("tones")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & ToneIdfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf currentFilterActive.Equals("shadeid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountShade") = dic("shades")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & ShadeIdfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                    ElseIf currentFilterActive.Equals("collectionid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "collections", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & CollectionIdfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories shades tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                    ElseIf currentFilterActive.Equals("brandid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "brands", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & BrandIdfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf currentFilterActive.Equals("price") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Pricefq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf currentFilterActive.Equals("rating") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Rattingfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf currentFilterActive.Equals("DepartmentId") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                            If Not String.IsNullOrEmpty(Categoryfq) Then
                                urlFilter &= "&fq=" + Categoryfq
                            End If
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "categories", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Categoryfq
                        If Not String.IsNullOrEmpty(filterDefault) Then
                            urlFilter &= "&fq=(" + filterDefault + ")"
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices brands rattings collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    End If
                Else
                    HttpContext.Current.Session("searchBrandId") = String.Empty
                    HttpContext.Current.Session("searchDepartmentId") = String.Empty
                End If

                HttpContext.Current.Session("searchHasToprate") = hasToprate

            End If
        Catch ex As Exception
            Components.Email.SendError("ToError500", "Solr Search item", url + " - " + ex.ToString())
        End Try
        If Not String.IsNullOrEmpty(filterDefault) Then
            If filterDefault.Replace("OR", "|").Split("|").Where(Function(i) i.Contains("departmentid")).Count() > 1 AndAlso HttpContext.Current.Session("searchDepartmentId") IsNot Nothing Then
                HttpContext.Current.Session("searchDepartmentId") = filterDefault + "_" + HttpContext.Current.Session("searchDepartmentId").ToString()
            End If
        End If
        Return lstItem
    End Function

    Public Shared Function SpellWord(ByVal keyword As String) As String
        Try
            'Dim temp As String = keyword.Replace("*", "").Replace("?", "")
            'If Not String.IsNullOrEmpty(temp) Then
            '    Dim url = SolrServerUrl + DefaultCore + "/spell?q=" + temp + "&wt=xml"

            '    Dim xmlDoc As XmlDocument = RequestUrl(url)
            '    If Not xmlDoc Is Nothing Then
            '        Dim result As String = xmlDoc.SelectNodes("/response/lst[@name='spellcheck']/lst[@name='suggestions']/lst/arr[@name='suggestion']/str").Item(0).InnerText
            '        If Not String.IsNullOrEmpty(result) Then
            '            Return result
            '        End If
            '    End If
            'End If
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Shared Sub loggingKw(ByVal keyword As String, ByVal count As Object, ByVal reason As CacheItemRemovedReason)
        If reason <> CacheItemRemovedReason.Expired Then
            Return
        End If
        Try
            If (IsKeywordSKU(keyword)) Then
                Exit Sub
            End If

            Dim objKeyword As New KeywordRow
            objKeyword.KeywordName = keyword
            Dim keywordSearchId As Integer = KeywordRow.Insert(Nothing, objKeyword, 0, count)
            If keywordSearchId > 0 Then
                HttpContext.Current.Session("KeywordSearchId") = keywordSearchId
                Utility.CacheUtils.SetCache("SolrHelper_" + keyword, keywordSearchId, 50000)
            Else
                HttpContext.Current.Session.Remove("KeywordSearchId")
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        End Try

    End Sub

    Private Shared Function IsKeywordSKU(ByVal kw As String) As Boolean
        If kw.Length = 6 Then
            Dim skuNumber As Integer = 0
            Return Integer.TryParse(kw, skuNumber)
        End If
        Return False
    End Function

    Private Shared Function getResponse(ByVal url As String) As XmlDocument

        url = url.Replace("#", "%23")

        Dim xmlDoc As XmlDocument = New XmlDocument()
        Dim request As HttpWebRequest = WebRequest.Create(url)

        Dim response As HttpWebResponse = request.GetResponse()
        Try
            xmlDoc.Load(response.GetResponseStream())
        Catch ex As System.Xml.XmlException

        End Try
        Return xmlDoc
    End Function


    Public Shared Function SearchItem(ByVal filter As DepartmentFilterFields, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal orderby As String, ByVal CurrentFilterActive As String) As DataLayer.StoreItemCollection
        Dim lstItem As New StoreItemCollection
        Dim url As String = String.Empty
        Try
            If filter.pg = 1 Or filter.pg = -1 Then
                m_counter = 0
            End If

            Dim lstBrandId As String = ","
            Dim lstCountBrand As String = ","
            Dim lstDepartmentId As String = ","
            Dim lstCountCategory As String = ","
            Dim lstCollectionId As String = ","
            Dim lstToneId As String = ","
            Dim lstShadeId As String = ","

            'filter with Collection
            Dim CollectionIdfq As String = String.Empty
            If (filter.CollectionId > 0) Then
                CollectionIdfq = "collectionid:" + filter.CollectionId.ToString()
            End If

            'filter with tone
            Dim ToneIdfq As String = String.Empty
            If (filter.ToneId > 0) Then
                ToneIdfq = "toneid:" + filter.ToneId.ToString()
            End If

            'filter with shade
            Dim ShadeIdfq As String = String.Empty
            If (filter.ShadeId > 0) Then
                ShadeIdfq = "shadeid:" + filter.ShadeId.ToString()
            End If

            ''filter with BrandId
            Dim BrandIdfq As String = String.Empty
            If (filter.BrandId > 0) Then
                BrandIdfq = "brandid:" + filter.BrandId.ToString()
            End If

            'filter with PriceDesc
            Dim Pricefq As String = String.Empty
            If (Not String.IsNullOrEmpty(filter.PriceRange)) Then
                Dim min As Double = GetMinMaxValue(filter.PriceRange, False)
                Dim max As Double = GetMinMaxValue(filter.PriceRange, True)
                Pricefq = "pricefilter:[" + IIf(min = 0, "*", min.ToString()) + " TO " + IIf(max = 0, "*", max.ToString()) + " }"
            End If

            'filter with rating
            Dim Rattingfq As String = String.Empty
            If (Not String.IsNullOrEmpty(filter.RatingRange)) Then
                Dim min As Double = GetMinMaxValue(filter.RatingRange, False)
                Dim max As Double = GetMinMaxValue(filter.RatingRange, True)
                Rattingfq = "averagereview:[" + IIf(min = 0, "*", min.ToString()) + " TO " + IIf(max = 0, "*", max.ToString()) + " }"
            End If

            'filter with category
            Dim Categoryfq As String = String.Empty
            If (filter.DepartmentId > 0) Then
                Categoryfq = "departmentid:" + filter.DepartmentId.ToString()
            End If

            'Add facet collection
            Dim facetCollectionQuery As String = "&facet.field=collectionid&facet.mincount=1"
            'Add facet tone
            Dim facetToneQuery As String = "&facet.field=toneid&facet.mincount=1"
            'Add facet shade
            Dim facetShadeQuery As String = "&facet.field=shadeid&facet.mincount=1"

            'Add facet category
            Dim facetCategoryQuery As String = "&facet.field=departmentid&facet.mincount=1"

            'Add facet brandid
            Dim facetBrandidQuery As String = "&facet.field=brandid&facet.mincount=1"


            'Add facet ratting.
            Dim facetRattingQuery As String = "facet.interval=averagereview"
            Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Rating))
            Dim len As Integer = arr.Length - 1
            For i As Integer = 0 To len

                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                If (i = len) Then
                    min = CType(arr(i), Double)
                    facetRattingQuery &= String.Format("&f.averagereview.facet.interval.set=[{0},*]", min)
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    facetRattingQuery &= String.Format("&f.averagereview.facet.interval.set=[{0},{1})", IIf(min = 0, "*", min.ToString()), max)
                End If

            Next

            'Add facet price ratting.
            Dim facetPriceQuery As String = "facet.interval=pricefilter"
            arr = [Enum].GetValues(GetType(Utility.Common.Price))
            len = arr.Length - 1
            For i As Integer = 0 To len
                Dim min As Double = 0
                Dim max As Double = Double.MaxValue
                Dim p As Boolean = False
                If (i = 0) Then
                    max = CType(arr(i + 1), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[*,{0})", max)
                ElseIf (i = len) Then
                    min = CType(arr(i), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[{0},*)", min)
                Else
                    min = CType(arr(i), Double)
                    max = CType(arr(i + 1), Double)
                    facetPriceQuery &= String.Format("&f.pricefilter.facet.interval.set=[{0},{1})", min, max)
                End If
            Next

            'Add facet newitems for sort
            Dim facetNewItem As String = "&facet.field=newitems&facet.mincount=1"

            'Final query

            Dim q As String = Utility.ConfigData.SolrServerURL + "departmentid:" & filter.DepartmentId.ToString()

            'add filter
            Dim fq As String = String.Empty
            If (Not String.IsNullOrEmpty(BrandIdfq)) Then
                fq &= "&fq=" & BrandIdfq
            End If

            If (Not String.IsNullOrEmpty(Pricefq)) Then
                fq &= "&fq=" & Pricefq
            End If

            If (Not String.IsNullOrEmpty(Rattingfq)) Then
                fq &= "&fq=" & Rattingfq
            End If

            If (Not String.IsNullOrEmpty(Categoryfq)) Then
                fq &= "&fq=" & Categoryfq
            End If
            If (Not String.IsNullOrEmpty(CollectionIdfq)) Then
                fq &= "&fq=" & CollectionIdfq
            End If
            If (Not String.IsNullOrEmpty(ToneIdfq)) Then
                fq &= "&fq=" & ToneIdfq
            End If
            If (Not String.IsNullOrEmpty(ShadeIdfq)) Then
                fq &= "&fq=" & ShadeIdfq
            End If


            'Add facet
            Dim facetStr As String = String.Empty
            If Not String.IsNullOrEmpty(facetCollectionQuery) Then
                facetStr &= "&" & facetCollectionQuery
            End If
            If Not String.IsNullOrEmpty(facetToneQuery) Then
                facetStr &= "&" & facetToneQuery
            End If
            If Not String.IsNullOrEmpty(facetShadeQuery) Then
                facetStr &= "&" & facetShadeQuery
            End If
            If Not String.IsNullOrEmpty(facetCategoryQuery) Then
                facetStr &= "&" & facetCategoryQuery
            End If
            If Not String.IsNullOrEmpty(facetBrandidQuery) Then
                facetStr &= "&" & facetBrandidQuery
            End If
            If Not String.IsNullOrEmpty(facetPriceQuery) Then
                facetStr &= "&" & facetPriceQuery
            End If
            If Not String.IsNullOrEmpty(facetRattingQuery) Then
                facetStr &= "&" & facetRattingQuery
            End If
            If Not String.IsNullOrEmpty(facetNewItem) Then
                facetStr &= "&" & facetNewItem
            End If

            If Not String.IsNullOrEmpty(facetStr) Then
                q &= "&facet=true" & facetStr
            End If

            url = String.Empty
            Dim sort As String = "&sort="
            Dim xmlDoc As System.Xml.XmlDocument = New XmlDocument()
            Dim hits As XmlNodeList = Nothing
            If orderby Is Nothing Then
                'sort &= "itemname asc, lowpriceold asc, lowprice asc"
                sort &= "newitems desc, brandidorder desc, itemname asc"
                'search with sort RELEVANCE
                url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", PageSize, (PageIndex - 1) * PageSize, fieldList) & fq + IIf(sort = "&sort=", String.Empty, sort)
                'Dim request As HttpWebRequest = WebRequest.Create(Utility.ConfigData.GlobalRefererName & url)
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    hits = xmlDoc.SelectNodes("/response/result/doc")
                End If
            Else
                'search with sort
                If orderby = "price" Then
                    sort &= "pricefilter asc"
                ElseIf orderby = "pricehigh" Then
                    sort &= "pricefilter desc"
                ElseIf orderby = "most-popular-review" Then
                    sort &= "mostpopularreview desc"
                ElseIf orderby = "top-rated" Then
                    sort &= "toprate desc"
                ElseIf orderby = "new-items" Then
                    sort &= " newitems desc, newuntil desc , itemname asc"
                ElseIf orderby = "sku" Then
                    sort &= "sku asc"
                End If

                url = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", PageSize, (PageIndex - 1) * PageSize, fieldList) & fq + IIf(sort = "&sort=", String.Empty, sort)
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    hits = xmlDoc.SelectNodes("/response/result/doc")
                End If

            End If

            Dim beginRow As Integer = (PageIndex - 1) * PageSize
            Dim endRow As Integer = beginRow + PageSize - 1
            Dim bId As Integer
            Dim cId As String
            Dim hasNew As Boolean = False
            Dim hasBestSale As Boolean = False
            Dim hasHots As Boolean = False
            Dim ishot As Boolean = False
            Dim isNew As Boolean = False
            Dim isBestSeller As Boolean = False
            Dim hasToprate As Boolean = False
            Dim mostTopRate As Integer = 0

            Dim collectionIdL As Integer
            Dim toneIdL As Integer
            Dim shadeIdL As Integer

            Dim totalCount As Integer = 0
            Integer.TryParse(xmlDoc.SelectNodes("/response/result").Item(0).Attributes("numFound").Value, totalCount)

            If totalCount = 0 Then
                HttpContext.Current.Session("searchCountCategory") = Nothing
                HttpContext.Current.Session("searchCountBrand") = Nothing
                HttpContext.Current.Session("searchCountRating") = Nothing
                HttpContext.Current.Session("searchCountPrice") = Nothing
                HttpContext.Current.Session("searchBrandId") = String.Empty
                HttpContext.Current.Session("searchDepartmentId") = String.Empty
                HttpContext.Current.Session("searchHasToprate") = Nothing
                HttpContext.Current.Session("searchCountCollection") = Nothing
                HttpContext.Current.Session("searchCountTone") = Nothing
                HttpContext.Current.Session("searchCountCategory") = Nothing
                Return Nothing

            End If

            If filter.pg <> -1 Then
                Dim memberID As Integer = Utility.Common.GetCurrentMemberId()
                Dim orderID As Integer = Utility.Common.GetCurrentOrderId()
                For i As Integer = 0 To PageSize - 1
                    Dim doc As XmlNode = hits(i)
                    If doc Is Nothing Then
                        Continue For
                    End If
                    m_counter += 1
                    Dim item As New StoreItemRow
                    Try
                        item.SKU = getXmlNodeLucene("sku", doc)
                        item.URLCode = getXmlNodeLucene("urlcode", doc)
                        'item.ShortDesc = getXmlNodeLucene("ShortDesc", doc)
                        item.ItemName = getXmlNodeLucene("itemname", doc) '' & " | " & hits.Score(i)
                        item.LastDepartmentName = getXmlNodeLucene("lastdepartmentname", doc)
                        item.Image = getXmlNodeLucene("image", doc)
                        Integer.TryParse(getXmlNodeLucene("brandid", doc), item.BrandId)
                        Double.TryParse(getXmlNodeLucene("price", doc), item.LowSalePrice)
                        item.ShowPrice = getXmlNodeLucene("pricedesc", doc)
                        isNew = CBool(getXmlNodeLucene("newitems", doc))
                        item.IsNew = isNew
                        item.IsNewTrue = isNew
                        item.IsHot = ishot
                        item.IsBestSeller = isBestSeller
                        Integer.TryParse(getXmlNodeLucene("countreview", doc), item.CountReview)
                        Double.TryParse(getXmlNodeLucene("averagereview", doc), item.AverageReview)
                        Integer.TryParse(getXmlNodeLucene("qtyonhand", doc), item.QtyOnHand)
                        item.IsSpecialOrder = CBool(getXmlNodeLucene("isspecialorder", doc))
                        Integer.TryParse(getXmlNodeLucene("acceptingorder", doc), item.AcceptingOrder)
                        StoreItemRow.GetItemSearchInfor(item, memberID, orderID)
                        item.itemIndex = m_counter
                        'Dim isloginViewprice = False
                        'If Boolean.TryParse(getXmlNodeLucene("enableisloginviewprice", doc), isloginViewprice) Then
                        '    item.IsLoginViewPrice = isloginViewprice
                        '    'If isloginViewprice Then
                        '    '    item.IsLoginViewPrice = memberID > 0
                        '    'End If
                        'End If

                        lstItem.Add(item)

                    Catch ex As Exception

                    End Try
                Next
            Else
                Dim doc As XmlNode = hits(0)

                Dim i As Integer = 0
                While (i < hits.Count)
                    Try
                        doc = hits(i)
                        Integer.TryParse(getXmlNodeLucene("brandid", doc), bId)
                        Integer.TryParse(getXmlNodeLucene("collectionid", doc), collectionIdL)
                        Integer.TryParse(getXmlNodeLucene("toneid", doc), toneIdL)
                        Integer.TryParse(getXmlNodeLucene("shadeid", doc), shadeIdL)

                        Integer.TryParse(getXmlNodeLucene("mostpopularreview", doc), mostTopRate)

                        cId = getXmlNodeLucene("departmentid", doc)

                        If (getXmlNodeLucene("hotitems", doc) = "1") Then
                            ishot = True
                        Else
                            ishot = False
                        End If
                        If (getXmlNodeLucene("newitems", doc) = "true") Then
                            isNew = True
                        Else
                            isNew = False
                        End If
                        If (getXmlNodeLucene("bestsellers", doc) = "1") Then
                            isBestSeller = True
                        Else
                            isBestSeller = False
                        End If
                        If mostTopRate > 0 And hasToprate = False Then
                            hasToprate = True
                        End If
                        If (i = 0) Then
                            m_counter += 1
                            Dim item As New StoreItemRow
                            item.ItemName = getXmlNodeLucene("itemname", doc)
                            item.SKU = getXmlNodeLucene("sku", doc)
                            item.URLCode = getXmlNodeLucene("urlcode", doc)
                            item.ItemId = getXmlNodeLucene("id", doc)
                            If Not lstBrandId.Contains("," & bId & ",") Then
                                lstBrandId = lstBrandId & bId & ","
                            End If
                            If Not lstCollectionId.Contains("," & collectionIdL & ",") Then
                                lstCollectionId = filter.CollectionId & collectionIdL & ","
                            End If
                            If Not lstToneId.Contains("," & toneIdL & ",") Then
                                lstToneId = lstToneId & toneIdL & ","
                            End If
                            If Not lstShadeId.Contains("," & shadeIdL & ",") Then
                                lstShadeId = lstShadeId & shadeIdL & ","
                            End If

                            lstDepartmentId = lstDepartmentId & cId & ","
                            lstItem.Add(item)
                        Else
                            'If loadMore Then
                            '    Exit While
                            'End If
                            If Not lstBrandId.Contains("," & bId & ",") Then
                                lstBrandId = lstBrandId & bId & ","
                            End If

                            'lstDepartmentId = lstDepartmentId & cId & ","
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
                    Catch ex As Exception
                        i = i + 1
                    End Try
                End While

                If Not hasNew Then
                    hasNew = getFacetNewItems(xmlDoc)
                End If
                HttpContext.Current.Session("searchHasNew") = hasNew

                'Fill category
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    Dim listCache As Dictionary(Of String, String) = New Dictionary(Of String, String)(4)

                    'get cateagories
                    Dim categories As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='departmentid']/int")
                    If categories.Count > 0 Then
                        For i = 0 To categories.Count - 1
                            Dim DepartmentIdStr As String = categories.ItemOf(i).Attributes("name").Value
                            If Not lstDepartmentId.Contains("," & DepartmentIdStr & ",") Then
                                lstDepartmentId = lstDepartmentId & DepartmentIdStr & ","
                            End If
                        Next
                    End If
                    'HttpContext.Current.Session.Add("Lucene_lstDepartmentId_" + textSearch, lstDepartmentId)
                End If

                'Fill colection
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get collections
                    Dim collections As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='collectionid']/int")
                    If collections.Count > 0 Then
                        For i = 0 To collections.Count - 1
                            Dim CollectionIdStr As String = collections.ItemOf(i).Attributes("name").Value
                            If Not lstCollectionId.Contains("," & CollectionIdStr & ",") Then
                                lstCollectionId = lstCollectionId & CollectionIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill tone
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get tones
                    Dim tones As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='toneid']/int")
                    If tones.Count > 0 Then
                        For i = 0 To tones.Count - 1
                            Dim ToneIdStr As String = tones.ItemOf(i).Attributes("name").Value
                            If Not lstToneId.Contains("," & ToneIdStr & ",") Then
                                lstToneId = lstToneId & ToneIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill shade
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    'get shade
                    Dim shades As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='shadeid']/int")
                    If shades.Count > 0 Then
                        For i = 0 To shades.Count - 1
                            Dim ShadeIdStr As String = shades.ItemOf(i).Attributes("name").Value
                            If Not lstShadeId.Contains("," & ShadeIdStr & ",") Then
                                lstShadeId = lstShadeId & ShadeIdStr & ","
                            End If
                        Next
                    End If
                End If

                'Fill brandid
                url = q & "&wt=xml&rows=0&start=0"
                xmlDoc = getResponse(url)
                If (xmlDoc.HasChildNodes AndAlso Not xmlDoc("response").IsEmpty) Then
                    Dim listCache As Dictionary(Of String, String) = New Dictionary(Of String, String)(4)

                    'get brandid
                    Dim brands As XmlNodeList = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='brandid']/int")
                    If brands.Count > 0 Then
                        For i = 0 To brands.Count - 1
                            Dim brandIdStr As String = brands.ItemOf(i).Attributes("name").Value
                            If Not lstBrandId.Contains("," & brandIdStr & ",") Then
                                lstBrandId = lstBrandId & brandIdStr & ","
                            End If
                        Next
                    End If
                End If

                If (hits.Count > 0) Then
                    HttpContext.Current.Session("searchBrandId") = lstBrandId
                    HttpContext.Current.Session("searchDepartmentId") = lstDepartmentId
                    HttpContext.Current.Session("searchCollectionId") = lstCollectionId
                    HttpContext.Current.Session("searchToneId") = lstToneId
                    HttpContext.Current.Session("searchShadeId") = lstShadeId

                    Dim dic As Dictionary(Of String, String) = Nothing
                    If String.IsNullOrEmpty(CurrentFilterActive) Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf CurrentFilterActive.Equals("toneid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountTone") = dic("tones")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & ToneIdfq
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf CurrentFilterActive.Equals("shadeid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountShade") = dic("shades")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & ShadeIdfq
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories collections tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                    ElseIf CurrentFilterActive.Equals("collectionid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "collections", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & CollectionIdfq
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices brands categories shades tones", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                    ElseIf CurrentFilterActive.Equals("brandid") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "brands", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & BrandIdfq
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings prices categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf CurrentFilterActive.Equals("price") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Pricefq
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf CurrentFilterActive.Equals("rating") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "rattings", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountRating") = dic("rattings")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Rattingfq
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices brands categories collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    ElseIf CurrentFilterActive.Equals("DepartmentId") Then
                        Dim urlFilter As String = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}", 0, 0)
                        If Not String.IsNullOrEmpty(Categoryfq) Then
                            urlFilter &= "&fq=" + Categoryfq
                        End If
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "categories", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountCategory") = dic("categories")

                        urlFilter = q & String.Format("&wt=xml&fl=*&rows={0}&start={1}&fq=", 0, 0) & Categoryfq
                        If Not String.IsNullOrEmpty(BrandIdfq) Then
                            urlFilter &= "&fq=" + BrandIdfq
                        End If
                        If Not String.IsNullOrEmpty(Pricefq) Then
                            urlFilter &= "&fq=" + Pricefq
                        End If
                        If Not String.IsNullOrEmpty(Rattingfq) Then
                            urlFilter &= "&fq=" + Rattingfq
                        End If
                        If Not String.IsNullOrEmpty(CollectionIdfq) Then
                            urlFilter &= "&fq=" + CollectionIdfq
                        End If
                        If Not String.IsNullOrEmpty(ToneIdfq) Then
                            urlFilter &= "&fq=" + ToneIdfq
                        End If
                        If Not String.IsNullOrEmpty(ShadeIdfq) Then
                            urlFilter &= "&fq=" + ShadeIdfq
                        End If
                        dic = getFacet(urlFilter, "prices brands rattings collections tones shades", Categoryfq, BrandIdfq, Pricefq, Rattingfq, CollectionIdfq, ToneIdfq, ShadeIdfq)
                        HttpContext.Current.Session("searchCountPrice") = dic("prices")
                        HttpContext.Current.Session("searchCountBrand") = dic("brands")
                        HttpContext.Current.Session("searchCountRatting") = dic("rattings")
                        HttpContext.Current.Session("searchCountCollection") = dic("collections")
                        HttpContext.Current.Session("searchCountTone") = dic("tones")
                        HttpContext.Current.Session("searchCountShade") = dic("shades")
                    End If
                Else
                    HttpContext.Current.Session("searchBrandId") = String.Empty
                    HttpContext.Current.Session("searchDepartmentId") = String.Empty
                End If

                HttpContext.Current.Session("searchHasToprate") = hasToprate

            End If
            lstItem.TotalRecords = totalCount
        Catch ex As Exception
            Components.Email.SendError("ToError500", "Solr Search item", url + " - " + ex.ToString())
        End Try

        Return lstItem
    End Function

    Private Shared Function removeString(ByVal str As String) As String
        Return str.Replace("&", "").Replace(" and ", "").Replace(" or ", "")
    End Function
End Class
