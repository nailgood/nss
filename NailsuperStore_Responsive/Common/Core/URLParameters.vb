Imports System.Collections.Specialized
Imports System.Web
Imports Utility.Common

Public Class URLParameters
    Dim m_Params As New NameValueCollection

    Public Sub New()
    End Sub

    Public Sub New(ByVal qs As NameValueCollection)
        Dim keys() As String = qs.AllKeys
        For Each key As String In keys
            Add(key, qs(key))
        Next
    End Sub

    Public Sub New(ByVal qs As NameValueCollection, ByVal Exclude As String, ByVal addQuery As Boolean)
        If Not addQuery Then
            Exit Sub
        End If
        Dim m_Exclude As ArrayList = New ArrayList(Exclude.ToLower.Split(";"))
        Dim keys() As String = qs.AllKeys

        For Each key As String In keys
            If Not key = String.Empty Then
                If Not m_Exclude.Contains(key.ToLower) Then
                    Add(key, qs(key))
                End If
            End If
        Next
    End Sub
    Public Sub New(ByVal qs As NameValueCollection, ByVal Exclude As String)
        Dim m_Exclude As ArrayList = New ArrayList(Exclude.ToLower.Split(";"))
        Dim keys() As String = qs.AllKeys

        For Each key As String In keys
            If Not key = String.Empty Then
                If Not m_Exclude.Contains(key.ToLower) Then
                    Add(key, qs(key))
                End If
            End If
        Next
    End Sub

    Public Sub Add(ByVal name As String, ByVal value As String)
        m_Params.Add(name, value)
    End Sub
    Public Sub Clear()
        Try
            m_Params.Clear()
        Catch ex As Exception

        End Try


        'For Each key As String In m_Params
        '    m_Params.Remove(key)
        'Next
    End Sub

    Public Overloads Function ToString(ByVal name As String, ByVal value As String) As String
        Add(name, value)
        Dim Result As String = Me.ToString
        m_Params.Remove(name)

        Return Result
    End Function

    Public Overrides Function ToString() As String
        Dim Conn As String = String.Empty
        Dim Result As String = String.Empty

        For i As Integer = 0 To m_Params.Keys.Count - 1
            If Not m_Params.Keys(i) = Nothing Then
                Result &= Conn & m_Params.Keys(i) & "=" & HttpContext.Current.Server.UrlEncode(m_Params.Item(i))
                Conn = "&"
            End If
        Next
        If Not Result = String.Empty Then Result = "?" & Result
        Return Result
    End Function
    Public Shared Function AddParamaterToURL(ByVal url As String, ByVal paraName As String, ByVal paraValue As String, ByVal addEmptyValue As Boolean) As String
        If url Is Nothing Then
            Return String.Empty
        End If
        Dim indexPara As Integer = url.IndexOf("?")
        Dim result As String = ""
        If indexPara < 0 Then
            If paraValue Is Nothing Or paraValue = "" Then
                If addEmptyValue Then
                    result = url & "?" & paraName & "=" & paraValue
                Else
                    result = url
                End If
            Else
                result = url & "?" & paraName & "=" & paraValue
            End If
            Return result
        ElseIf indexPara = url.Length - 1 Then
            result = url & "" & paraName & "=" & paraValue
            Return result
        End If
        Dim template As String = url.Substring(indexPara + 1, url.Length - indexPara - 1).Replace("=", "&")
        Dim arr() As String = Split(template, "&")
        If arr.Length > 1 Then

            Dim index As Integer = 0
            Dim item As String
            Dim hasFind As Boolean = False
            While index < arr.Length
                item = arr(index).ToString
                If item.ToLower().Trim() = paraName.ToLower().Trim Then
                    If paraValue Is Nothing Or paraValue = "" Then
                        If addEmptyValue Then
                            'result = result & "&" & paraName & "=" & paraValue
                            result = result '& "&" & paraName & "=" & paraValue
                        End If
                    Else
                        result = result & "&" & paraName & "=" & paraValue
                    End If

                    hasFind = True
                Else
                    ''result = result & "&" & item & "=" & arr(index + 1).ToString
                    Dim prAdd As String = String.Empty
                    Try
                        prAdd = arr(index + 1).ToString
                    Catch ex As Exception

                    End Try
                    If (prAdd.Length > 0) Then
                        result = result & "&" & item & "=" & arr(index + 1).ToString
                    End If
                End If
                index = index + 2
            End While
            If Not hasFind Then
                If paraValue Is Nothing Or paraValue = "" Then
                    If addEmptyValue Then
                        result = result & "&" & paraName & "=" & paraValue
                    End If
                Else
                    result = result & "&" & paraName & "=" & paraValue
                End If
                'If addEmptyValue And (paraValue  "" Or paraValue Is Nothing) Then

                'Else
                '    result = result & "&" & paraName & "=" & paraValue
                'End If
            End If
            If result <> "" Then
                result = result.Substring(1, result.Length - 1)
                Return url.Substring(0, indexPara) & "?" & result
            Else
                Return url.Substring(0, indexPara)
            End If

        End If
        Return url
    End Function
    Public Shared Function ReturnCanonical(ByVal Url As String) As String 'for mobile
        If Url.Contains("v") Then
            If Url.Contains("&") Then
                Url = Url.Replace("&v=0", "").Replace("&v=1", "").Replace("v=0&", "").Replace("v=1&", "")
            Else
                Url = Url.Replace("?v=0", "").Replace("?v=1", "")
            End If
        End If
        Return Utility.ConfigData.GlobalRefererName & Url
    End Function
    Public Shared Function ProductUrl(ByVal ProductCode As String, ByVal itemId As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If ProductCode <> "" Then
                strUrl = String.Format("/nail-products/{0}/{1}", ProductCode, itemId)
            End If

        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function ProductUrlByID(ByVal ProductID As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            Dim code As String = DataLayer.StoreItemRow.GetRowURLCodeById(ProductID)
            Return ProductUrl(code, ProductID)
        Catch ex As Exception

        End Try

        Return strUrl
    End Function

    Public Shared Function FaceBookPageUrl(ByVal title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/facebook/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(title)), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function ShopDesignUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/shop-by-design/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function ShopSaveUrl(ByVal Name As String, ByVal Id As Integer, ByVal Type As ShopSaveType) As String
        Dim strUrl As String = String.Empty
        Dim strType As String = "shop-now"

        Try
            Select Case Type
                Case ShopSaveType.WeeklyEmail
                    strType = "promotion"
                Case ShopSaveType.SaveNow
                    strType = "save-now"
                Case ShopSaveType.DealOfDay
                    strType = "deal-of-day"
                Case Else
                    strType = "shop-now"
            End Select
            If Id > 0 Then
                strUrl = String.Format("/{0}/{1}/{2}", strType, ReplaceUrl(HttpUtility.UrlEncode(Name.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function ShopDesignListUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/shop-by-design-collection/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function BlogDetailUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/blog/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function NewsListlUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/news-topic/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function NewsDetailUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/news-detail/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function

    Public Shared Function VideoDetailUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/video-detail/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function MediaListUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/media-topic/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception
        End Try
        Return strUrl
    End Function
    Public Shared Function MediaDetailUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/media-detail/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception
        End Try
        Return strUrl
    End Function
    Public Shared Function DepartmentCollectionUrl(ByVal UrlCode As String, ByVal DeptId As Integer) As String
        Dim strUrl As String
        strUrl = String.Format("/nail-collection/{0}/{1}", UrlCode, DeptId)
        Return strUrl
    End Function


    Public Shared Function PolicyUrl(ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/service/policy.aspx?id={0}", Id)
            End If
        Catch ex As Exception
        End Try

        Return strUrl
    End Function

    Public Shared Function DepartmentUrl(ByVal RewriteCode As String, ByVal DeptId As Integer) As String
        Dim strUrl As String
        strUrl = String.Format("/nail-supply/{0}/{1}", RewriteCode, DeptId)
        Return strUrl
    End Function

    Public Shared Function BrandUrl(ByVal RewriteCode As String, ByVal BrandId As Integer) As String
        Dim strUrl As String
        strUrl = String.Format("/nail-brand/{0}/{1}", RewriteCode, BrandId)
        Return strUrl
    End Function

    Public Shared Function DepartmentUrl(ByVal RewriteCode As String, ByVal parentId As Integer, ByVal DeptId As Integer) As String
        Dim code As String = "nail-supply"
        If parentId = Utility.ConfigData.RootDepartmentID Then
            code = "nail-supplies"
        End If
        Dim strUrl As String
        strUrl = String.Format("/{0}/{1}/{2}", code, RewriteCode, DeptId)
        Return strUrl
    End Function
    Public Shared Function SalesUrl(ByVal SaleName As String, ByVal SaleID As Integer) As String
        Dim strUrl As String = String.Format("/nail-sales/{0}", ReplaceUrl(HttpUtility.UrlEncode(SaleName)))
        If SaleID > 0 Then
            strUrl &= String.Format("/{0}", SaleID)
        End If

        Return strUrl
    End Function

    Public Shared Function SalesUrl(ByVal code As String) As String
        Dim strUrl As String = String.Format("/nail-sales/{0}", code)
        Return strUrl
    End Function

    Public Shared Function MainDepartmentUrl(ByVal DepartmentName As String, ByVal Id As Integer) As String
        Dim strUrl As String
        strUrl = String.Format("/nail-supplies/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(DepartmentName)), Id)
        Return strUrl
    End Function

    Public Shared Function PromotionUrl(ByVal DepartmentName As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        If Id > 0 AndAlso Not String.IsNullOrEmpty(DepartmentName) Then
            strUrl = String.Format("/nail-promotions/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(DepartmentName)), Id)
        End If
        Return strUrl
    End Function
    Public Shared Function PromotionUrl(ByVal DepartmentCode As String) As String
        Dim strUrl As String = String.Empty
        If Not String.IsNullOrEmpty(DepartmentCode) Then
            strUrl = String.Format("/nail-promotions/{0}", DepartmentCode)
        End If

        Return strUrl
    End Function
    Public Shared Function MusicItemUrl(ByVal ProductCode As String) As String
        Dim strUrl As String = String.Empty
        Try
            If ProductCode <> "" Then
                strUrl = String.Format("/music-products/{0}", ProductCode)
            End If

        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function TipDetailUrl(ByVal Title As String, ByVal Id As Integer) As String
        Dim strUrl As String = String.Empty
        Try
            If Id > 0 Then
                strUrl = String.Format("/tips-detail/{0}/{1}", ReplaceUrl(HttpUtility.UrlEncode(Title.ToLower())), Id)
            End If
        Catch ex As Exception

        End Try

        Return strUrl
    End Function
    Public Shared Function ProductReviewUrl(ByVal DeptName As String, ByVal DeptId As Integer) As String
        Dim code As String = "product-reviews"
        Dim strUrl As String
        strUrl = String.Format("/{0}/{1}/{2}", code, DeptName, DeptId)
        Return strUrl
    End Function
    Public Shared Function ReplaceUrl(ByVal str As String)
        If str <> "" And str <> Nothing Then
            str = RTrim(LTrim(str))
            str = str.Replace("%3e", "greater").Replace("<", "").Replace(">", "greater").Replace(" / ", "-").Replace("/", "-").Replace(" & ", "_and_").Replace("+", "-").Replace("_-_", "-").Replace(" ", "-").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("&", "-").Replace(",", "-").Replace(".", "-").Replace("%3f", "").Replace(".", "").Replace("?", "").Replace("%26", "and").Replace("%22", "").Replace("%3a", "").Replace("%2c", "").Replace("%24", "$").Replace("%c3%a9", "e").Replace("%25", "percent").Replace("%20", "-").Replace("%22", "").Replace("%c2%a2", "c").Replace("%c3%a8", "e").Replace("'", "").Replace("#", "").Replace("%e2%80%99", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "-").Replace("è", "e").Replace("é", "e").Replace("¢", "").Replace("e2809c", "").Replace("e2809d", "").Replace(" - ", "-").Replace("--", "-").Replace("|", "").Replace("%22", "").Replace("-–", "-")
            str = str.Replace("--", "-").Replace("'", "")
        End If
        Return str
    End Function
    Public Shared Function ChangeUrlError(ByVal url As String)
        Dim strUrl As String = String.Empty
        Dim arr As String() = Nothing
        Try
            If url.Contains("/embed/how-to-video/") Then
                arr = url.Split("""")
                Dim subArr As String() = arr(arr.Length - 2).Split("/")
                url = arr(0) + subArr(subArr.Length - 1)
                strUrl = String.Format(url, ReplaceUrl(HttpUtility.UrlEncode(url.ToLower())))
            End If

            Utility.Common.Redirect301(strUrl)

        Catch ex As Exception

        End Try
    End Function
End Class
