Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components
Imports DataLayer

Public Class admin_ajax
    Inherits AdminPage

    Private Function Escape(ByVal s As String)
        Dim t As String

        t = Replace(s, "'", "\'")
        t = Trim(t)

        Return "'" & t & "'"
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FunctionName As String
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        FunctionName = Request("f")
        Select Case FunctionName
            Case "DisplayItemActive"
                DisplayItemActive()
            Case "DisplayItems"
                DisplayItems()
            Case "DisplayItemsEx"
                DisplayItemsEx()
            Case "GetItemInfoEx"
                GetItemInfoEx()
            Case "DisplayItemsAndCollections"
                DisplayItemsAndCollections()
            Case "DisplayItemEnable"
                DisplayItemEnable()
            Case "DisplaySwatches"
                DisplaySwatches()
            Case "DisplayDepartments"
                DisplayDepartments()
            Case "DisplayItemsAndDepartments"
                DisplayItemsAndDepartments()
            Case "GetDepartmentInfo"
                GetDepartmentInfo()
            Case "GetItemInfo"
                GetItemInfo()
            Case "GetItemEnableInfo"
                GetItemEnableInfo()
            Case "GetImportStatus"
                GetImportStatus()
            Case "ExtendSession"
                ExtendSession()
            Case "DisplayKeyword"
                DisplayKeyword()
            Case "DisplayKeywordSynonym"
                DisplayKeywordSynonym()
            Case "SuggestAdminKeywordSynonym"
                SuggestAdminKeywordSynonym()
            Case "GetKeywordSynonymData"
                GetKeywordSynonymData()

        End Select
    End Sub

    Private Sub ExtendSession()
        If LoggedInAdminId > 0 Then
            Response.Write(1)
        Else
            Response.Write(0)
        End If
    End Sub

    Private Sub DisplayItems()
        Dim ItemId, SQL, q, SubSQL
        Dim sArray1, sArray2, sConn
        sConn = ""

        ItemId = Request("ItemId")
        q = Request("q")

        If IsNumeric(Request("ItemGroupId")) AndAlso Not CInt(Request("ItemGroupId")) = Nothing Then
            SubSQL = " and itemid not in (select itemid from storeitemgrouprel) and itemid not in (select itemid from StoreBaseColorItem) and itemid not in (select itemid from StoreCusionColorItem) and itemid not in (select itemid from storelaminatetrimitem) "
        Else
            SubSQL = ""
        End If
        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 10 * from StoreItem where (Itemname like " & DB.Quote(q & "%") & " or SKU like " & DB.Quote(q & "%") & ") " & SubSQL & " order by ItemName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("ItemName") & ", (" & dr("SKU") & ")")
                sArray2 &= sConn & Escape(dr("ItemId"))
                sConn = ","
            End While

        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub DisplayDepartments()
        Dim DepartmentId, SQL, q, SubSQL
        Dim sArray1, sArray2, sConn
        sConn = ""

        DepartmentId = Request("DepartmentId")
        q = Request("q")

        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 10 departmentid, name + case when parentid <> 23 then ' (' + (select top 1 name from storedepartment where parentid is not null and sd.lft between lft and rgt order by lft) + ')' else '' end as name from StoreDepartment sd where name like " & DB.Quote(q & "%") & " order by Name, lft asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("Name"))
                sArray2 &= sConn & Escape(dr("DepartmentId"))
                sConn = ","
            End While

        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub DisplayItemsEx()
        Dim ItemGroupId, SQL, q, SubSQL
        Dim sArray1, sArray2, sConn
        sConn = ""

        ItemGroupId = Request("ItemGroupId")
        q = Request("q")

        SubSQL = " and itemgroupid is null and itemid not in (select itemid from StoreBaseColorItem) and itemid not in (select itemid from StoreCusionColorItem) and itemid not in (select itemid from storelaminatetrimitem) "

        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 10 * from StoreItem where (Itemname like " & DB.Quote(q & "%") & " or SKU like " & DB.Quote(q & "%") & ") " & SubSQL & " order by ItemName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("ItemName") & ", (" & dr("SKU") & ")")
                sArray2 &= sConn & Escape(dr("ItemId"))
                sConn = ","
            End While
        Catch ex As Exception

        End Try
        Core.CloseReader(dr)
        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub DisplayItemsAndCollections()
        Dim ItemId, SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""

        ItemId = Request("ItemId")
        q = Request("q")

        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 10 * from StoreItem where (Itemname like " & DB.Quote(q & "%") & " or SKU like " & DB.Quote(q & "%") & ") order by ItemName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("ItemName") & ", (" & dr("SKU") & ")")
                sArray2 &= sConn & Escape(dr("ItemId"))
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayItemsAndCollections", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub
    Private Sub DisplayItemEnable()
        Dim MemberId, SQL, q, Type
        Dim sArray1, sArray2, sConn
        sConn = ""

        MemberId = Request("MemberId")
        q = Request("q")
        Type = Request("Type")
        sArray1 = "new Array("
        sArray2 = "new Array("
        If Type = "user" Then
            SQL = " select top 10 c.*, m.memberid, m.UserName from Customer c, Member m where (UserName like " & DB.Quote("%" & q & "%") & ") and c.customerid = m.customerid order by username asc"
        ElseIf Type = "mail" Then
            SQL = " select top 10 c.*, m.memberid, m.UserName from Customer c, Member m where (email like " & DB.Quote("%" & q & "%") & ") and c.customerid = m.customerid order by email asc"
        End If

        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                If Type = "user" Then
                    sArray1 &= sConn & Escape(dr("UserName") & ", (" & dr("email") & ")")
                ElseIf Type = "mail" Then
                    sArray1 &= sConn & Escape(dr("email") & ", (" & dr("UserName") & ")")
                End If

                sArray2 &= sConn & Escape(dr("MemberId"))
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayItemEnable", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub DisplaySwatches()
        Dim ItemId, SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""

        ItemId = Request("ItemId")
        q = Request("q")

        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 10 * from StoreItem where itemtype = 'swatch' and (Itemname like " & DB.Quote(q & "%") & " or SKU like " & DB.Quote(q & "%") & ") order by ItemName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("ItemName") & ", (" & dr("SKU") & ")")
                sArray2 &= sConn & Escape(dr("ItemId"))
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplaySwatches", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub DisplayItemsAndDepartments()
        Dim ItemId, SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""

        ItemId = Request("ItemId")
        q = Request("q")

        sArray1 = "new Array("
        sArray2 = "new Array("

        SQL = " select top 5 ItemId As RecId, ItemName As RecordName, 'Item' As RecType from StoreItem where (Itemname like " & DB.Quote(q & "%") & " or SKU like " & DB.Quote(q & "%") & ") "
        SQL &= " UNION "
        SQL &= " select top 5 DepartmentId As RecId, Name As RecordName, 'Department' As RecType from StoreDepartment where Name like " & DB.Quote(q & "%")
        SQL &= " order by RecordName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("RecordName") & " [" & dr("RecType") & "]")
                sArray2 &= sConn & Escape(dr("RecId") & "-" & dr("RecType"))
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayItemsAndDepartments", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub

    Private Sub GetDepartmentInfo()
        Dim DepartmentId, SQL, sResult
        sResult = ""
        Dim dr As SqlDataReader = Nothing
        Try
            DepartmentId = Request("DepartmentId")

            SQL = " select * from StoreDepartment where DepartmentId  = " & DB.Quote(DepartmentId)
            dr = DB.GetReader(SQL)
            If dr.Read Then
                sResult = dr("DepartmentId") & "|" & dr("Name")
            End If
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayItemsAndDepartments", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        Response.Write(sResult)
    End Sub

    Private Sub GetItemInfo()
        Dim ItemId, SQL, sResult
        sResult = ""
        Dim dr As SqlDataReader = Nothing
        Try
            ItemId = Request("ItemId")

            SQL = " select * from StoreItem where ItemId  = " & DB.Quote(ItemId)
            dr = DB.GetReader(SQL)
            If dr.Read Then
                sResult = dr("SKU") & "|" & dr("ItemName")
            End If

            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> GetItemInfo", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        Response.Write(sResult)
    End Sub
    Private Sub GetItemEnableInfo()
        Dim MemberId, SQL, sResult
        sResult = ""
        Dim dr As SqlDataReader = Nothing
        Try
            MemberId = Request("MemberId")

            SQL = " select c.CustomerNo, m.memberid, c.Email, m.Username from customer c, Member m where c.customerid = m.customerid and m.memberId  = " & DB.Quote(MemberId)
            dr = DB.GetReader(SQL)
            If dr.Read Then
                sResult = dr("CustomerNo") & "|" & dr("MemberId") & "|" & dr("Email") & "|" & dr("Username")
            End If
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> GetItemEnableInfo", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        Response.Write(sResult)
    End Sub

    Private Sub GetItemInfoEx()
        Dim ItemId, ItemGroupId, SQL, sResult
        sResult = ""
        Dim dr As SqlDataReader = Nothing
        Try
            ItemId = Request("ItemId")
            ItemGroupId = Request("ItemGroupId")

            SQL = " select SKU,ItemName from StoreItem where ItemId  = " & DB.Quote(ItemId)
            dr = DB.GetReader(SQL)
            If dr.Read Then
                sResult = dr("SKU") & "@" & dr("ItemName")
            End If
            Core.CloseReader(dr)

            Dim Options As String = ""
            SQL = " select go.OptionName,go.OptionId from storeitemgroupoptionrel gor inner join storeitemgroupoption go on gor.optionid = go.optionid where itemgroupid = " & ItemGroupId
            Dim dv As DataView = DB.GetDataView(SQL)
            For i As Integer = 0 To dv.Count - 1
                Options &= IIf(Options = String.Empty, "", "[~]") & "<strong style=""color:black"">" & dv(i)("OptionName") & ":</strong><br />" & vbCrLf
                SQL = "select ChoiceId,ChoiceName from storeitemgroupchoice where optionid = " & dv(i)("OptionId")
                dr = DB.GetReader(SQL)
                Options &= "<select name=""OPTIONS_" & dv(i)("OptionId") & """ id=""OPTIONS_" & dv(i)("OptionId") & """>" & vbCrLf
                While dr.Read
                    Options &= "<option value=""" & dr("ChoiceId") & """>" & dr("ChoiceName") & "</option>" & vbCrLf
                End While
                dr.Close()
                Options &= "</select><br />" & vbCrLf
            Next
            sResult &= "@" & Options & "<br />"
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> GetItemInfoEx", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        Response.Write(sResult)
    End Sub

    Private Sub GetImportStatus()
        Try
            Dim FileNames As String = Request("FileNames")
            Dim dv As DataView, drv As DataRowView
            Dim SQL, email, conn As String

            conn = ""

            If Not ImportIsRunning() Then
                Response.Write("ImportSuccess")

                SQL = "select top 1 * from NavisionImport where BCPStart is not null and BCPDate is null and BCPFail = 0"
                dv = DB.GetDataSet(SQL).Tables(0).DefaultView
                If dv.Count > 0 Then
                    drv = dv(0)

                    SQL = "update NavisionImport set BCPFail = 1 where FileName = " & DB.Quote(drv("FileName"))
                    DB.ExecuteSQL(SQL)

                    email = SysParam.GetValue("NavisionImportNotificationEmail")
                    'Core.SendSimpleMail(email, email, email, email, "Navision Import Failed: " & drv("FileName"), "File " & drv("FileName") & " was not processed." & vbCrLf & vbCrLf & "A System Error has occured. Our systems administrators have been notified and are working to fix the problem. Please contact Customer Service at webmaster@americaneagle.com if you need further assistance.")

                    email = SysParam.GetValue("NavisionAdminEmail")
                    'Core.SendSimpleMail(email, email, email, email, "Navision Import Failed: " & drv("FileName"), "File " & drv("FileName") & " was not processed." & vbCrLf & vbCrLf & "The import script was terminated while running.")
                End If
            End If

            SQL = "select * from NavisionImport where FileName in " & DB.QuoteMultiple(FileNames) & " order by ImportDate desc"
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)

                If drv("BCPFail") Then
                    Response.Write(conn & drv("BCPStart") & "||<span style=""color:red;font-weight:bold"">Failed!</span>")
                Else
                    If Not IsDBNull(drv("BCPStart")) Then
                        If IsDBNull(drv("BCPDate")) Then
                            Response.Write(conn & drv("BCPStart") & "|" & drv("BCPDate") & "|<SPAN style=""font-weight:bold"">Processing <IMG src=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/images/indicator.gif"" align=absMiddle></span>")
                        Else
                            Response.Write(conn & drv("BCPStart") & "|" & drv("BCPDate") & "|" & IIf(drv("RowsImported") > 0, "<span style=""color:green;font-weight:bold"">Imported (" & drv("RowsImported") & " rows)</span>", "Skipped"))
                        End If
                    Else
                        Response.Write(conn & "||")
                    End If
                End If
                conn = "[~]"
            Next
        Catch ex As Exception
            AddError(ex.ToString)
        End Try
    End Sub

    Private Sub DisplayKeyword()
        Dim SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""
        Dim dr As SqlDataReader = Nothing
        Try
            q = Request("q")
            sArray1 = "new Array("
            sArray2 = "new Array("
            SQL = " select top 10 KeywordName from Keyword where KeywordName like " & DB.FilterQuote(q)
            dr = DB.GetReader(SQL)
            Dim KeywordName As String
            While dr.Read
                KeywordName = GetFromReader("KeywordName", dr)
                sArray1 &= sConn & Escape(HightLightSearch(KeywordName, q))
                sArray2 &= sConn & Escape(KeywordName)
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayKeyword", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
            Core.CloseReader(dr)
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDivKeyword('al', " & sArray1 & ", " & sArray2 & ")")
    End Sub
    Private Sub SuggestAdminKeywordSynonym()
        Dim SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""
        Dim dr As SqlDataReader = Nothing
        Try
            q = Request("q")
            sArray1 = "new Array("
            sArray2 = "new Array("
            SQL = " select top 10 KeywordName from Keyword where KeywordName like " & DB.FilterQuote(q) & " and KeywordId IN (SELECT KeywordId FROM KeywordSynonym)"
            dr = DB.GetReader(SQL)
            Dim KeywordName As String
            While dr.Read
                KeywordName = GetFromReader("KeywordName", dr)
                sArray1 &= sConn & Escape(HightLightSearch(KeywordName, q))
                sArray2 &= sConn & Escape(KeywordName)
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> SuggestAdminKeywordSynonym", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"

        Response.Write("showQueryDivKeyword('al', " & sArray1 & ", " & sArray2 & ")")
    End Sub
    Private Sub DisplayKeywordSynonym()
        Dim SQL, q
        Dim sArray1, sArray2, sConn
        sConn = ""
        Dim dr As SqlDataReader = Nothing
        Try
            Dim mainKeywordId = Request("keywordid")
            q = Request("q")
            sArray1 = "new Array("
            sArray2 = "new Array("
            SQL = " select top 10 KeywordName from Keyword where KeywordName like " & DB.FilterQuote(q) & " and KeywordId<>" & mainKeywordId & " and KeywordId not in(Select KeywordSynonymId from KeywordSynonym where KeywordId=" & mainKeywordId & ")"
            dr = DB.GetReader(SQL)
            Dim KeywordName As String
            While dr.Read
                KeywordName = GetFromReader("KeywordName", dr)
                sArray1 &= sConn & Escape(KeywordName)
                sArray2 &= sConn & Escape(KeywordName)
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> DisplayKeywordSynonym", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Response.Write("showQueryDivKeyword('al', " & sArray1 & ", " & sArray2 & ")")
    End Sub
    Private Sub GetKeywordSynonymData()
        Dim keywordId As Integer = 0
        Dim dr As SqlDataReader = Nothing
        Try
            Dim SQL As String = String.Empty
            Dim keyword = Request("keyword")

            SQL = " select KeywordId from Keyword where KeywordName='" & keyword & "'"
            dr = DB.GetReader(SQL)
            While dr.Read
                keywordId = CInt(GetFromReader("KeywordId", dr))
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Ajax.aspx.vb >> GetKeywordSynonymData", "Exception: " & ex.ToString() & "<br>SQL: " & SQL & "<br>" & SitePage.GetSessionList())
        End Try

        Response.Write("BindKeywordSynonymData(" & keywordId & ")")
    End Sub
   
    Private Function GetFromReader(ByVal fieldName As String, ByVal dr As SqlDataReader) As String
        Try
            Return dr(fieldName)
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function
    Private Function HightLightSearch(ByVal SourceSearch As String, ByVal CharSearch As String) As String
        Dim result As String = String.Empty
        CharSearch = CharSearch.Trim()
        Try
            Dim temp As String = SourceSearch.ToLower()
            Dim tempSource As String = SourceSearch
            Dim charbegin As Integer = temp.IndexOf(CharSearch.ToLower())
            While (charbegin <> -1)
                result += tempSource.Substring(0, charbegin)
                result += "<span class='highlight'>" + tempSource.Substring(charbegin, CharSearch.Length()) + "</span>"
                tempSource = tempSource.Substring(charbegin + CharSearch.Length())
                temp = temp.Substring(charbegin + CharSearch.Length())
                charbegin = temp.IndexOf(CharSearch.ToLower())
            End While
            result += tempSource.Substring(0)
        Catch ex As Exception

        End Try
        Return result
    End Function
    Private Sub DisplayItemActive()
        Dim ItemGroupId, SQL, q, SubSQL
        Dim sArray1, sArray2, sConn
        sConn = ""

        q = Request("q")

        sArray1 = "new Array("
        sArray2 = "new Array("
        SQL = " select top 20 * from StoreItem where (SKU like " & DB.Quote(q & "%") & " or Itemname like " & DB.Quote(q & "%") & ") order by ItemName asc"
        Dim dr As SqlDataReader = Nothing
        Try
            dr = DB.GetReader(SQL)
            While dr.Read
                sArray1 &= sConn & Escape(dr("SKU") & ", (" & dr("ItemName") & ")")
                sArray2 &= sConn & Escape(dr("ItemId"))
                sConn = ","
            End While
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)

        End Try

        sArray1 = sArray1 & ")"
        sArray2 = sArray2 & ")"
        Response.Write("showQueryDiv('al', " & sArray1 & "," & sArray2 & ")")
    End Sub
End Class