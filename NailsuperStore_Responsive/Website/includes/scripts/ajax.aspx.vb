Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
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
Imports LuceneHelper



Partial Class includes_ajax
    Inherits SitePage
    Protected ReWriteURL As RewriteUrl
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim FunctionName As String
        FunctionName = Request("f")

        Select Case FunctionName
            Case "GetRelatedItems"
                GetRelatedItems()
            Case "GetAddressDetails"
                GetAddressDetails()
            Case "GetItemDetails"
                GetItemDetails()
            
            Case "GetItemReviews"
                GetItemReviews()
            Case "SubmitItemReview"
                SubmitItemReview()
            Case "GetBrands"
                GetBrands()
            Case "GetAlternateImages"
                GetAlternateImages()
            Case "GetItemURLCode"
                GetItemURLCode()
            Case "GetItemSearch"
                GetItemSearch()
            Case "DisplayItemsSearchLucene"
                DisplayItemsSearchLucene()
        End Select
    End Sub
    Private Sub GetItemSearch()
        Dim ItemId, SQL, sResult
        sResult = ""

        If (IsNumeric(Request("ItemId"))) Then
            ItemId = Request("ItemId")
        Else
            Exit Sub
        End If
        Dim dr As SqlDataReader = Nothing
        Try
            SQL = " select * from StoreItem where ItemId  = " & ItemId
            dr = DB.GetReader(SQL)
            If dr.Read Then
                sResult = dr("SKU") & "|" & dr("ItemName")
            End If
        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        Response.Write(sResult)
    End Sub

    Private Function GetFromReader(ByVal fieldName As String, ByVal dr As SqlDataReader) As String
        Try
            Return dr(fieldName)
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function
    Private Sub GetItemURLCode()
        Try
            If (IsNumeric(Request("ItemId"))) Then
                Dim ItemId As String = Request("ItemId")
                Dim sql As String = "Select [dbo].[GetItemURLCode](" & ItemId & ")"
                Dim result As String = DB.ExecuteScalar(sql)
                Response.Write(result.ToLower())
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetListAlternateImages(ByVal ItemId As Integer, ByVal pg As Integer) As String
        Try
           
            Dim ImageItem As String = Request("ItemImage")
            Dim NameItem As String = Request("ItemName")
            Dim s As String = String.Empty
            Dim PageSize As Integer = 3
            Dim iCount As Integer
            Dim lstImg As StoreItemImageCollection = StoreItemImageRow.GetListImageByItem(ItemId, pg, PageSize, iCount)
            Dim iPages As Integer = 0
            If PageSize > 0 Then
                iPages = Math.Ceiling(CDbl(iCount / PageSize))
            End If
            Dim sNext As String = String.Empty
            Dim sPrev As String = String.Empty
            Dim sLink As String
            If lstImg.Count > 0 Then
                For Each img As StoreItemImageRow In lstImg
                    s &= "<div style='height: 200%; width: 100%; float: none; margin: auto; background: url(/assets/items/featured/" & img.Image & ") no-repeat scroll left bottom;'><a href=""javascript:void(0);"" onclick=""updateImage('/assets/items/large/" & img.Image & "','" & img.ImageAltTag.Replace("""", "\'").Replace("'", "\'") & "');""><img src=""/assets/nobg.gif"" height=""115px"" width=""115px"" style=""margin-bottom:0px;border:solid 1px #dddddd;"" alt=""" & img.ImageAltTag & """ /></a></div><br />" & vbCrLf
                Next
                If iPages > 1 Then
                    s &= "<div class=""primarytxtc smallest"">" & vbCrLf
                    For i As Integer = 1 To iPages
                        sLink = "<a href=""javascript:void(0);"" onclick=""enqueue('/includes/ajax.aspx?F=GetAlternateImages&ItemId=" & ItemId & "&pg=" & i & "', processAlternates, loadingAlternateImage('divAlternates'))"">"
                        If i = pg Then
                            s &= "<b>" & i & "</b> "
                        Else
                            s &= sLink & i & "</a> "
                        End If
                        If pg = i - 1 Then
                            sNext = " " & sLink & "Next &raquo;</a>"
                        ElseIf pg = i + 1 Then
                            sPrev = sLink & "&laquo; Prev</a> "
                        End If
                    Next
                    If sNext = String.Empty Then sNext = " <span style=""color:#999999;"">Next &raquo;</span>"
                    If sPrev = String.Empty Then sPrev = "<span style=""color:#999999;"">&laquo; Prev</span> "
                    s &= "</div><div class=""primarytxtc smallest"" style=""margin-top:3px;"">" & sPrev & sNext & "</div>" & vbCrLf
                End If
            Else
                If Not ImageItem = String.Empty Then
                    'Trung Nguyen modify - 1/8/2010
                    s = "<div style='height: 115px; width: 115px; float: none; margin: auto; background: url(/assets/items/featured/" & ImageItem & ") no-repeat scroll left bottom;'><a href=""javascript:void(0);"" onclick=""updateImage('/assets/items/large/" & ImageItem & "','" & NameItem.Replace("""", "\'").Replace("'", "\'") & "');""><img src=""/assets/nobg.gif"" height=""115px"" width=""115px"" style=""margin-bottom:0px;border:solid 1px #dddddd;"" alt=""" & NameItem.ToString.Replace("""", "'") & """ /></a></div>" & vbCrLf
                Else
                    s = "<div class=""smallest secondarytxtc"" style=""margin-top:10px;font-style:italic;width:100px;"">Sorry, no alternate images are available for this product</div>"
                End If
            End If

            Return s
        Catch ex As Exception
        End Try
        Return String.Empty
    End Function
    Private Sub GetAlternateImages()

        Dim ItemId As Integer = Nothing
        If (IsNumeric(Request("ItemId"))) Then
            ItemId = Request("ItemId")
        Else
            Exit Sub
        End If
        Dim pg As Integer = Nothing
        If (IsNumeric(Request("ItemId"))) Then
            pg = Request("pg")
        Else
            pg = 1
        End If
        Response.Write(GetListAlternateImages(ItemId, pg))

    End Sub

    Private Sub GetAddressDetails()
        Dim Id As Integer = Nothing
        If (IsNumeric(Request("Id"))) Then
            Id = Request("Id")
        End If
        Dim a As MemberAddressRow = MemberAddressRow.GetRow(DB, Id)
        Response.Write(a.Company & "[~]" & a.FirstName & "[~]" & a.LastName & "[~]" & a.Address1 & "[~]" & a.Address2 & "[~]" & a.City & "[~]" & a.State & "[~]" & a.Zip & "[~]" & a.Phone & "[~]" & a.Fax & "[~]" & a.Country & "[~]" & a.Region)
    End Sub

    

    Private Sub GetBrands()
        Try
            'Long edit 07102009 Dim PageIndex As Integer = Request("PageIndex")
            Dim PageIndex As Integer '= IIf(IsNumeric(Request("PageIndex")) = True, Request("PageIndex"), 0)
            Dim PageSize As Integer '= Request("PageSize")
            Dim DepartmentId As Integer
            Dim SelectedId As Integer
            Dim PageUrl As String = Trim(Request("PageUrl"))

            If Not Request("PageIndex") Is Nothing Then
                If Request("PageIndex").ToString().Trim() <> "" Then
                    PageIndex = IIf(IsNumeric(Request("PageIndex")), CInt(Request("PageIndex")), 0)
                Else
                    PageIndex = 1
                End If
            Else
                PageIndex = 1
            End If

            If Not Request("PageSize") Is Nothing Then
                If Request("PageSize").ToString().Trim() <> "" Then
                    PageSize = IIf(IsNumeric(Request("PageSize")), CInt(Request("PageSize")), 0)
                Else
                    PageSize = 8
                End If
            Else
                PageSize = 8
            End If

            If Not Request("SelectedId") Is Nothing Then
                If Request("SelectedId").ToString().Trim() <> "" Then
                    SelectedId = IIf(IsNumeric(Request("SelectedId")), CInt(Request("SelectedId")), 0)
                Else
                    SelectedId = 0
                End If
            Else
                SelectedId = 0
            End If

            'If Request("SelectedId") <> Nothing Then
            '    SelectedId = Request("SelectedId").Replace(".aspx", "")
            'End If

            If Not Request("DepartmentId") Is Nothing Then
                If Request("DepartmentId").ToString().Trim() <> "" Then
                    DepartmentId = IIf(IsNumeric(Request("DepartmentId")), CInt(Request("DepartmentId")), 0)
                Else
                    DepartmentId = 23
                End If
            Else
                DepartmentId = 23
            End If

            'If Request("DepartmentId") <> Nothing Then DepartmentId = Request("DepartmentId")

            Dim filter As New DepartmentFilterFields
            'filter.DepartmentId = DepartmentId
            'filter.IsFeatured = Department.DepartmentId = ParentDepartment.DepartmentId AndAlso Request("F_All") <> "Y"
            filter.All = Request("F_All") <> String.Empty
            'filter.IsOnSale = Request("F_Sale") <> String.Empty
            filter.MaxPerPage = PageSize
            filter.pg = PageIndex
            'filter.ToneId = IIf(Not IsNumeric(Request("ToneId")), Nothing, Request("ToneId"))
            'filter.CollectionId = IIf(Not IsNumeric(Request("CollectionId")), Nothing, Request("CollectionId"))

            Dim TotalBrands As Integer = StoreBrandRow.GetAllStoreBrandsCount(DB, filter)

            Dim dv As DataView = StoreBrandRow.GetAllStoreBrands(DB, filter).DefaultView
            For i As Integer = PageSize * (PageIndex - 1) To Math.Min((PageSize * PageIndex) - 1, dv.Count - 1)
                'Rewrite url
                Dim BrandName As String = IIf(dv(i)("BrandNameUrl").ToString <> "", dv(i)("BrandNameUrl"), dv(i)("BrandName"))
                If SelectedId = dv(i)("BrandId") AndAlso (DepartmentId = Nothing OrElse DepartmentId = 23) Then

                    If LCase(PageUrl) = "/store/default.aspx" Or LCase(PageUrl) = "/store/sub-category.aspx" Then
                        Response.Write("<div class=""navitem"">" & dv(i)("BrandName") & "</div>")
                    Else

                        'Response.Write("<div class=""navitem""><a href=""/store/default.aspx?F_BrandId=" & dv(i)("BrandId") & "&F_All=Y"">" & dv(i)("BrandName") & "</a></div>")
                        Response.Write("<div class=""navitem""><a href=""/brand/" & ReWriteURL.ReplaceUrl(BrandName) & "/" & dv(i)("BrandId") & ".aspx"">" & dv(i)("BrandName") & "</a></div>")
                    End If
                Else

                    'Response.Write("<div class=""navitem""><a href=""/store/default.aspx?F_BrandId=" & dv(i)("BrandId") & "&F_All=Y"" class=""lgry"">" & dv(i)("BrandName") & "</a></div>")
                    Response.Write("<div class=""navitem""><a href=""/brand/" & ReWriteURL.ReplaceUrl(BrandName) & "/" & dv(i)("BrandId") & ".aspx"" class=""lgry"">" & dv(i)("BrandName") & "</a></div>")
                    'end rewrite url
                End If
            Next

            Dim NumPages As Integer = 0
            If PageSize > 0 Then
                NumPages = Math.Ceiling(CDbl(TotalBrands / PageSize))
            End If

            Dim ctr As Integer = 1
            If NumPages > 1 Then
                Response.Write("<br /><div>Page ")
                While ctr <= NumPages
                    If ctr = PageIndex Then
                        Response.Write(" " & ctr & " ")
                    Else
                        Response.Write(" <a href=""javascript:void(0);"" onclick=""PaginateBrands(" & ctr & "," & NumPages & ");"">" & ctr & "</a> ")
                    End If
                    ctr += 1
                End While
                Response.Write("</div><br />")
            End If
        Catch ex As Exception
            Dim eh As New ErrorHandler(DB)
            eh.LogError(ex)
            Exit Sub
        End Try
    End Sub

    Private Sub GetItemDetails()
        Dim ItemGroupId, OptionId, ItemId As Integer, si As StoreItemRow, Choices, Language As String
        Dim enlarge As String = ""
        Dim width, height As Integer
        Dim ShortDescription As String
        Dim LongDescription As String

        Try
            'System.Threading.Thread.Sleep(1000)
            If (IsNumeric(Request("ItemGroupId"))) Then
                ItemGroupId = Request("ItemGroupId")
            End If
            If (IsNumeric(Request("ItemId"))) Then
                ItemId = Request("ItemId")
            End If
            If (IsNumeric(Request("OptionId"))) Then
                OptionId = Request("OptionId")
            End If

            Choices = Request("Choices")
            Language = Request("L")

            Session("Language") = LanguageCode.GetLanguageCode(Language)
            If Session("MemberId") <> Nothing Then
                Dim Member As MemberRow = MemberRow.GetRow(Session("Memberid"))
                Member.DB = DB
                Member.Customer.LanguageCode = Session("Language")
                Member.Customer.Update()
            End If

            If ItemGroupId <> Nothing Then
                si = StoreItemRow.GetRow(DB, StoreItemRow.GetItemIdByChoices(DB, ItemGroupId, Choices))

                'If Core.FileExists(Server.MapPath("/assets/items/large/") & si.Image) Then
                'Core.GetImageSize(Server.MapPath("/assets/items/large/") & si.Image, width, height)
                'If width > 230 OrElse height > 230 Then
                enlarge = "<a href=""#ViewLarger"" onclick=""showLarger();""><img src=""/images/global/prd-lnk-larger.gif"" width=""49"" height=""22"" style=""border-style:none"" alt=""Enlarge"" /></a>"
                'End If
                'End If

                NSS.FillPricing(DB, si)

                Dim StatusMsg As String
                Dim Qty As Integer = si.QtyOnHand
                Select Case si.Status
                    Case "BD"
                        If si.BODate <> Nothing Then
                            StatusMsg = "<span class=""red rtpad"">Expected in stock on " & si.BODate.ToShortDateString & "</span>"
                        Else
                            StatusMsg = "<span class=""red rtpad"">Backordered</span>"
                        End If
                        Qty = 0
                    Case "BO"
                        StatusMsg = "<span class=""red rtpad"">Backordered</span>"
                        Qty = 0
                    Case "DC"
                        StatusMsg = "<span class=""red bold rtpad"">Discontinued</span>"
                        Qty = 0
                    Case Else
                        If si.QtyOnHand > 0 Then
                            StatusMsg = "In Stock"
                            If si.LowStockThreshold > 0 AndAlso si.QtyOnHand <= si.LowStockThreshold Then
                                StatusMsg &= " - <span class=""red"">"
                                If si.LowStockMsg <> Nothing Then
                                    StatusMsg &= si.LowStockMsg.Replace("[QTY]", si.QtyOnHand)
                                Else
                                    StatusMsg &= SysParam.GetValue("HurryMessage").Replace("[QTY]", si.QtyOnHand)
                                End If
                                StatusMsg &= "</span>"
                            ElseIf si.LowStockThreshold = Nothing AndAlso SysParam.GetValue("HurryMessageThreshold") <> Nothing AndAlso si.QtyOnHand <= SysParam.GetValue("HurryMessageThreshold") Then
                                StatusMsg &= " - <span class=""red"">"
                                If si.LowStockMsg <> Nothing Then
                                    StatusMsg &= si.LowStockMsg.Replace("[QTY]", si.QtyOnHand)
                                End If
                                StatusMsg &= SysParam.GetValue("HurryMessage").Replace("[QTY]", si.QtyOnHand)
                            End If
                            StatusMsg &= "</span>"
                        Else
                            StatusMsg = "<span class=""rtpad red"">Out of Stock</span>"
                        End If
                End Select
                Select Case Session("Language")
                    Case LanguageCode.Vietnamese
                        ShortDescription = si.ShortViet
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongViet)
                    Case LanguageCode.French
                        ShortDescription = si.ShortFrench
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongFrench)
                    Case LanguageCode.Spanish
                        ShortDescription = si.ShortSpanish
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongSpanish)
                    Case Else
                        ShortDescription = si.ShortDesc
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongDesc)
                End Select
                If ShortDescription = Nothing Then
                    ShortDescription = BBCodeHelper.ConvertBBCodeToHTML(si.ShortDesc)
                End If
                If LongDescription = Nothing Then
                    LongDescription = si.LongDesc
                End If
                ''get list Option and List Choice
                Dim sql As String = "select OptionId,ChoiceId from storeitemgroupchoicerel where itemid = " & si.ItemId
                Dim dsVariance As DataSet
                dsVariance = DB.GetDataSet(sql)
                Dim optionList As String = String.Empty
                Dim choiceList As String = String.Empty
                If Not dsVariance Is Nothing Then
                    If dsVariance.Tables.Count > 0 Then

                        For i As Integer = 0 To dsVariance.Tables(0).Rows.Count - 1
                            optionList = optionList & dsVariance.Tables(0).Rows(i)("OptionId").ToString() & "*"
                            choiceList = choiceList & dsVariance.Tables(0).Rows(i)("ChoiceId").ToString() & "*"
                        Next

                    End If
                End If

                Response.Write(si.SKU & "|" & si.ItemName & "|/assets/items/" & IIf(si.Image = Nothing, "na.jpg", si.Image) & "|" & enlarge & "|" & IIf(si.BrandId <> Nothing, "by <a href=""/store/default.aspx?brandid=" & si.BrandId & """>" & si.GetBrandName() & "</a>", "") & "|" & NSS.DisplayPricing(DB, si, False) & "|" & ShortDescription & "|" & LongDescription & "|" & Qty & "|" & si.ItemId & "|" & si.Pricing.SellPrice & "|" & StatusMsg & "|" & IIf(si.Status = "DC", "1", "0") & "|" & optionList & "|" & choiceList)
            ElseIf ItemId <> Nothing Then
                si = StoreItemRow.GetRow(DB, ItemId)
                Select Case Session("Language")
                    Case LanguageCode.Vietnamese
                        ShortDescription = si.ShortViet
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongViet)
                    Case LanguageCode.French
                        ShortDescription = si.ShortFrench
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongFrench)
                    Case LanguageCode.Spanish
                        ShortDescription = si.ShortSpanish
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongSpanish)
                    Case Else
                        ShortDescription = si.ShortDesc
                        LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongDesc)
                End Select
                If ShortDescription = Nothing Then
                    ShortDescription = si.ShortDesc
                End If
                If LongDescription = Nothing Then
                    LongDescription = BBCodeHelper.ConvertBBCodeToHTML(si.LongDesc)
                End If
                Response.Write(LongDescription & "|" & ShortDescription)
            End If
        Catch ex As Exception
            Dim eh As New ErrorHandler(DB)
            eh.LogError(ex)
            Exit Sub
        End Try
    End Sub

    Private Sub GetRelatedItems()
        Try
            Dim ItemId As Integer '= Request("ItemId")
            Dim ItemGroupid As Integer '= Request("ItemGroupId")
            Dim PageIndex As Integer  'Request("PageIndex")
            Dim PageSize As Integer = IIf(DB.IsEmpty(Request("PageSize")) OrElse Not IsNumeric(Request("PageSize")), 3, Request("PageSize"))
            Dim dv As DataView, drv As DataRowView
            Dim StartRecord, EndRecord As Integer
            Dim conn As String = ""
            Dim s As String = String.Empty
            Dim departmentid As Integer = 0
            Dim itemname As String = ""
            Dim itemnamenew As String = ""
            Dim BrandName As String = ""

            If Not Request("PageIndex") Is Nothing Then
                If Request("PageIndex").ToString().Trim() <> "" Then
                    PageIndex = IIf(IsNumeric(Request("PageIndex")), CInt(Request("PageIndex")), 0)
                Else
                    PageIndex = 1
                End If
            Else
                PageIndex = 1
            End If

            If Not Request("ItemId") Is Nothing Then
                If IsDBNull(Request("ItemId")) = False Then
                    ItemId = CInt(Request("ItemId"))
                Else
                    ItemId = 0
                End If
            Else
                ItemId = 0
            End If

            If Not Request("ItemGroupid") Is Nothing Then
                If IsDBNull(Request("ItemGroupid")) = False Then
                    ItemGroupid = CInt(Request("ItemGroupid"))
                Else
                    ItemGroupid = 0
                End If
            Else
                ItemGroupid = 0
            End If

            StartRecord = PageIndex * PageSize
            EndRecord = StartRecord + PageSize - 1

            dv = StoreItemRow.GetRelatedItems(DB, ItemId, ItemGroupid, EndRecord + 1).Tables(0).DefaultView

            If StartRecord > dv.Count Then Exit Sub
            If EndRecord > dv.Count - 1 Then EndRecord = dv.Count - 1

            For i As Integer = StartRecord To EndRecord
                Try
                    drv = dv(i)

                    BrandName = drv("BrandName").ToString
                    BrandName = System.Web.HttpUtility.UrlEncode(BrandName)
                    BrandName = ReWriteURL.ReplaceUrl(BrandName)
                    departmentid = ReWriteURL.ReturnDepartmentID(DB, drv("ItemId"))
                    itemname = ReWriteURL.ReplaceUrl(System.Web.HttpUtility.UrlEncode(drv("ItemName")))
                    'itemnamenew = ReWriteURL.ReplaceUrl(drv("ItemNameNew"))
                    itemnamenew = IIf(IsDBNull(drv("ItemNameNew")) = False, ReWriteURL.ReplaceUrl(drv("ItemNameNew")), CStr(drv("ItemNameNew")))
                    s &= conn & drv("ItemId") & "|" & drv("ItemName") & "|" & drv("ShortDesc") & "|" & drv("ShortViet") & "|" & drv("image") & "|" & drv("BrandId") & "|" & BrandName & "|" & itemnamenew & "|" & departmentid & "|" & itemname
                    conn = "[~]"
                Catch ex As Exception
                    Response.Redirect(Request.UrlReferrer.ToString, False)
                End Try

            Next

            Response.Write(s)
        Catch ex As Exception
            Dim eh As New ErrorHandler(DB)
            eh.LogError(ex)
            Exit Sub
        End Try
    End Sub

    Private Sub GetItemReviews()
        Try
            Dim ItemId As Integer '= Request("ItemId")
            Dim ItemGroupid As Integer '= Request("ItemGroupId")
            Dim PageIndex As Integer '= Request("PageIndex")
            Dim PageSize As Integer = IIf(DB.IsEmpty(Request("PageSize")) OrElse Not IsNumeric(Request("PageSize")), 3, Request("PageSize"))
            Dim dv As DataView, drv As DataRowView
            Dim StartRecord, EndRecord As Integer
            Dim conn As String = ""
            Dim s As String = String.Empty

            If Not Request("PageIndex") Is Nothing Then
                If Request("PageIndex").ToString().Trim() <> "" Then
                    PageIndex = IIf(IsNumeric(Request("PageIndex")), CInt(Request("PageIndex")), 0)
                Else
                    PageIndex = 1
                End If
            Else
                PageIndex = 1
            End If

            If Not Request("ItemId") Is Nothing Then
                If IsDBNull(Request("ItemId")) = False Then
                    ItemId = CInt(Request("ItemId"))
                Else
                    ItemId = 0
                End If
            Else
                ItemId = 0
            End If

            If Not Request("ItemGroupid") Is Nothing Then
                If IsDBNull(Request("ItemGroupid")) = False Then
                    ItemGroupid = CInt(Request("ItemGroupid"))
                Else
                    ItemGroupid = 0
                End If
            Else
                ItemGroupid = 0
            End If

            StartRecord = PageIndex * PageSize
            EndRecord = StartRecord + PageSize - 1

            dv = StoreItemRow.GetItemReviews(DB, ItemId, ItemGroupid, EndRecord + 1).Tables(0).DefaultView

            If StartRecord > dv.Count Then Exit Sub
            If EndRecord > dv.Count - 1 Then EndRecord = dv.Count - 1

            For i As Integer = StartRecord To EndRecord
                drv = dv(i)
                s &= conn & drv("reviewtitle") & "|" & drv("reviewername") & "|" & drv("numstars") & "|" & CDate(drv("dateadded")).ToShortDateString & "|" & drv("comment")
                conn = "[~]"
            Next

            Response.Write(s)
        Catch ex As Exception
            Dim eh As New ErrorHandler(DB)
            eh.LogError(ex)
            Exit Sub
        End Try
    End Sub

    Private Sub SubmitItemReview()
        Try
            If Session("MemberId") = Nothing Then
                Response.Write("Your session has timed out. Please <a href=""/members/"">sign in</a> to your account.")
                Exit Sub
            End If
            Dim ItemId As Integer = Nothing '' Request("ItemId")
            If (IsNumeric(Request("ItemId"))) Then
                ItemId = Request("ItemId")
            End If
            Dim Stars As Integer = Nothing '' Request("Stars") / 10
            If (IsNumeric(Request("Stars"))) Then
                Stars = Request("Stars") / 10
            End If
            Dim Name As String = Request("Name")

            Dim Title As String = Request("Title")
            Dim Comments As String = Request("Comments")

            If Stars < 1 OrElse Stars > 5 Then Throw New Exception("Invalid rating!")

            Dim r As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ItemId, Session("MemberId"))
            If r.ReviewId = Nothing Then
                r.FirstName = IIf(Trim(Name) = String.Empty, "Anonymous", Trim(Core.StripHTML(Name)))
                r.NumStars = Stars
                r.ReviewTitle = Trim(Core.StripHTML(Title))
                r.Comment = Core.StripHTML(Comments)
                r.MemberId = Session("MemberId")
                r.DateAdded = Now()
                r.Insert()
                Response.Write("success")
            Else
                Response.Write("You have already submitted a review for this product. <a href=""javascript:void(0);"" onclick=""readReviews();"">Click here</a> to go back to product reviews.")
            End If
        Catch ex As Exception
            Response.Write("An error occurred while submitting your request.")
        End Try
    End Sub
    Private Function RpForSearch(ByVal str As String) As String
        'str = Replace(str, " ", "-")
        'str = Replace(str, "---", "-")
        'str = Replace(str, "--", "-")
        str = Replace(str, "*", "")
        Return str
    End Function
    Private Function Escape(ByVal s As String)
        Dim t As String

        t = Replace(s, "'", "\'")
        t = Trim(t)

        Return "'" & t & "'"
    End Function

    ''''''''SEARCH LUCENE.NET'''''''''''''''
    Private Sub DisplayItemsSearchLucene()
        Dim searchQuery As String = String.Empty
        If Not (Request("q") Is Nothing) Then
            searchQuery = Request("q").ToString()
        End If
        If Not String.IsNullOrEmpty(searchQuery) AndAlso Not searchQuery.Contains("Search by keyword or item") Then
            Dim displaySearch As New LuceneHelper
            Response.Write("showQueryDivSearch('al', " & displaySearch.DisplayItemsSearchLucene(searchQuery) & ")")
        Else
            Response.Write("showQueryDivSearch('al', new Array(),new Array(),new Array(),new Array(),new Array(),new Array(),new Array(),new Array())")
        End If
      
    End Sub
End Class

