Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports DataLayer
Imports Components
Public Class RewriteUrl
    Public Shared dt As New DataTable
    Public Shared DepartmentURLCode As String
    Public Shared DB1 As New Database
    Public Shared cart As ShoppingCart

    Public Shared Function ReplaceUrl(ByVal str As String)
        'str = str.Replace(" & ", "_and_").Replace("+", "_").Replace("_-_", "_").Replace("/", "or").Replace(" ", "_").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("__", "_").Replace("_%26_", "_").Replace("#", "_").Replace(" > ", "_").Replace("%22", "").Replace("%24", "$").Replace("%3a", "").Replace("%2c", "").Replace(",", "").Replace("&", "and")
        If str <> "" And str <> Nothing Then
            str = RTrim(LTrim(str))
            'str = str.Replace("%3e", "greater").Replace("<", "").Replace(">", "greater").Replace(" / ", "_").Replace("/", "_").Replace(" & ", "_and_").Replace("+", "_").Replace("_-_", "_").Replace(" ", "_").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("&", "_").Replace(",", "_").Replace(".", "_").Replace("%3f", "").Replace(".", "").Replace("?", "").Replace("%26", "and").Replace("%22", "").Replace("%3a", "").Replace("%2c", "").Replace("%24", "$").Replace("%c3%a9", "e").Replace("%25", "percent").Replace("%20", "_").Replace("%22", "").Replace("%c2%a2", "¢").Replace("%c3%a8", "e").Replace("'", "").Replace("#", "").Replace("%e2%80%99", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "_").Replace("__", "_").Replace("__", "_").Replace("è", "e").Replace("é", "e").Replace("¢", "").Replace("e2809c", "").Replace("e2809d", "")
            str = str.Replace("%3e", "greater").Replace("<", "").Replace(">", "greater").Replace(" / ", "-").Replace("/", "-").Replace(" & ", "_and_").Replace("+", "-").Replace("_-_", "-").Replace(" ", "-").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("&", "-").Replace(",", "-").Replace(".", "-").Replace("%3f", "").Replace(".", "").Replace("?", "").Replace("%26", "and").Replace("%22", "").Replace("%3a", "").Replace("%2c", "").Replace("%24", "$").Replace("%c3%a9", "e").Replace("%25", "percent").Replace("%20", "-").Replace("%22", "").Replace("%c2%a2", "¢").Replace("%c3%a8", "e").Replace("'", "").Replace("#", "").Replace("%e2%80%99", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "-").Replace("è", "e").Replace("é", "e").Replace("¢", "").Replace("e2809c", "").Replace("e2809d", "").Replace(" - ", "-").Replace("--", "-").Replace("|", "").Replace("%22", "")
            str = str.Replace("--", "-")
        End If
        Return str
    End Function
    Public Shared Function ReplaceSpace(ByVal str As String)
        If str <> "" And str <> Nothing Then
            str = str.Replace("_", " ")
        End If
        Return str
    End Function
    Public Shared Function ReturnDepartmentName(ByVal DB As Database, ByVal id As String, ByVal Type As String) As String
        Dim snID As Integer
        Dim strName As String = "0"
        Dim strSQL As String = ""

        Try
            If IsNumeric(id) = True Then
                strSQL = String.Format("select {1} from storedepartment where departmentid in (select parentid from storedepartment where departmentid={0})", CInt(id), IIf(Type = "ID", "departmentid", "name"))
                strName = DB.ExecuteScalar(strSQL).ToString()
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnDepartmentName<br><br>Exception: " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

        Return strName
    End Function

    Public Shared Function ReturnDepartment(ByVal DB As Database, ByVal id As Integer) As String
        Dim strName As String = "0"
        Dim strSQL As String = ""

        Try
            If IsNumeric(id) = True Then
                strSQL = "select name, departmentid, namerewriteurl from storedepartment where departmentid=" & id
                dt = DB.GetDataTable(strSQL)
                If dt.Rows.Count > 0 Then
                    strName = IIf(IsDBNull(dt.Rows(0)("NameRewriteUrl")) = False, dt.Rows(0)("NameRewriteUrl"), dt.Rows(0)("name"))
                End If
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnDepartment<br><br>Exception: " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

        Return strName
    End Function

    Public Shared Function ReturnLevel0(ByVal DB As Database, ByVal id As String, ByVal type As String) As String
        Dim strName As String = "0"
        Dim strSQL As String

        Try
            If IsNumeric(id) = True Then
                strSQL = "select top 1 parentid from storedepartment where departmentid= " & CInt(id) & " order by parentid asc"
            Else
                strSQL = "select top 1 parentid from storedepartment where name like '" & id & "' order by parentid asc"
            End If
            strSQL = String.Format("select {1} from storedepartment where departmentid in ({0})", strSQL, IIf(type = "ID", "departmentid", "name"))
            strName = DB.ExecuteScalar(strSQL).ToString()
        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnLevel0<br><br>Exception: " & ex.ToString())
        Finally

        End Try

        Return strName
    End Function

    Public Shared Function ReturnBrandName(ByVal DB As Database, ByVal id As Integer) As String
        Dim strName As String = ""
        Dim strSQL As String
        Try

            strSQL = "select brandname,brandnameurl from storebrand where brandid=" & id
            dt = DB.GetDataTable(strSQL)
            If dt.Rows.Count > 0 Then
                strName = (IIf(IsDBNull(dt.Rows(0)("brandnameurl")) = False And dt.Rows(0)("brandnameurl").ToString <> "", dt.Rows(0)("brandnameurl"), dt.Rows(0)("brandname")))
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnBrandName<br><br>Exception: " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

        Return strName
    End Function

    Public Shared Function ReturnItemName(ByVal DB As Database, ByVal id As Integer) As String
        Dim strName As String = ""
        Dim strSQL As String = ""

        Try
            strSQL = "select * from Vie_cart_item where itemid=" & id
            dt = DB.GetDataTable(strSQL)
            If dt.Rows.Count > 0 Then
                strName = IIf(IsDBNull(dt.Rows(0)("ItemNameNew")) = False, dt.Rows(0)("ItemNameNew").ToString.Replace("%", "percent"), dt.Rows(0)("itemname").ToString.Replace("%", "percent"))
            End If
        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnItemName<br><br>Exception: " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

        Return strName
    End Function

    Public Shared Function ReturnDepartmentID(ByVal DB As Database, ByVal id As Integer) As Integer
        Dim snID As Integer = 23
        Dim strSQL As String = ""

        Try
            If IsNumeric(id) = True Then
                strSQL = "select top 1 departmentid from storedepartmentitem sdi, storeitem si where sdi.itemid=si.itemid and sdi.itemid = " & id & " order by departmentid desc"
                Dim strResult As String = DB.ExecuteScalar(strSQL)
                If Not String.IsNullOrEmpty(strResult) Then
                    snID = CInt(strResult)
                End If
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnDepartmentID<br><br>Exception: " & ex.ToString())
        End Try

        Return snID
    End Function

    Public Shared Function ReturnItemID(ByVal DB As Database, ByVal sku As String) As Integer
        Dim snID As Integer = 0
        Dim strSQL As String

        Try
            strSQL = "select top 1 itemid from storeitem where Sku = '" & sku & "'"
            Dim strResult As String = DB.ExecuteScalar(strSQL)
            If Not String.IsNullOrEmpty(strResult) Then
                snID = CInt(strResult)
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "RewriteUrl.vb", "Function: ReturnItemID<br><br>Exception: " & ex.ToString())
        End Try

        Return snID
    End Function

    Public Shared Function ReturnPrice(ByVal price As Double) As String
        Dim s As String = ""
        s = "<span class=""strike"">" & FormatCurrency(price) & "</span>"
        s &= "<span class=""bold red"">" & FormatCurrency(0) & "</span>"
        Return s
    End Function

    Public Shared Function GetRootDepartmentId(ByVal path As String) As Int32
        Dim departmentID As Integer = 0

        If path.Contains("/") Then
            Dim indexChar As Integer = path.IndexOf("/")
            If indexChar > 0 Then
                departmentID = path.Substring(0, indexChar)
            Else
                departmentID = path
            End If
        End If

        Return departmentID
    End Function
    Public Shared Function GetRootDepartmentCode(ByVal path As String) As String
        If path Is Nothing Then
            Return ""
        End If
        Dim departmentCode As String = ""

        If path.Contains("/") Then
            Dim indexChar As Integer = path.IndexOf("/")
            If indexChar > 0 Then
                departmentCode = path.Substring(0, indexChar)
            End If
        End If

        Return departmentCode
    End Function

    Public Shared Function CheckBrandMemeber(ByVal memberid As String, ByVal brandid As String) As Boolean
        If brandid = Nothing Then
            Return True
        End If
        'Kiem tra xem item co thuoc Brand ko duoc phep ban
        Dim sie As New StoreItemEnable()

        Dim dt As DataTable = sie.ListBrands(memberid)
        If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
            Dim brands As String = dt.Rows(0)("Brands").ToString()
            Dim memberBrands As String = dt.Rows(0)("MemberBrands").ToString()

            If memberBrands.Contains("," & brandid & ",") Then
                Return True
            Else
                If brands.Contains("," & brandid & ",") Then
                    Return False
                Else
                    Return True
                End If
            End If

            dt.Dispose()
        End If
        'end
    End Function
    Public Shared Function StripTags(ByVal html As String) As String
        If html Is Nothing Then
            Return String.Empty
        End If
        ' Remove HTML tags.
        If html Is Nothing Then
            Return String.Empty
        Else
            Return Regex.Replace(html, "<.*?>", "")
        End If

    End Function
End Class
