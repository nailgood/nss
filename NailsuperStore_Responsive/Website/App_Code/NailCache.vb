Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Net
Imports Components
Imports DataLayer

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class NailCache
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Sub ClearItemCache(ByVal ItemIdList As String)
        If Not String.IsNullOrEmpty(ItemIdList) Then
            Try
                Dim itemClear As String = ""
                Dim arr() As String = Split(ItemIdList, ",")
                Dim keyClear As String = ""
                Dim lstKeyResult As String = ""
                If arr.Length > 0 Then

                    For Each item As String In arr
                        If item <> "" Then
                            keyClear = Utility.CacheUtils.RemoveCacheItemWithPrefix("StoreItem_GetRow_" & item & "_")
                            If keyClear <> "" Then
                                itemClear &= item & ","
                                lstKeyResult = lstKeyResult & keyClear & ","
                            End If
                        End If
                    Next
                End If

            Catch ex As Exception
                Email.SendError("ToErrorWebService", "[NailCache] ClearItemCache - Error", ex.ToString())
            End Try
        End If
    End Sub

    <WebMethod()> _
    Public Sub SendOrderConfirmation(ByVal OrderId As String)
        Dim Id As Integer

        Try
            Id = Convert.ToInt32(OrderId)
        Catch ex As Exception
            Id = Convert.ToInt32(Utility.Crypt.DecryptTripleDes(OrderId))
        End Try

        _SendOrderConfirmation(Id, True)
    End Sub

    Private Sub _SendOrderConfirmation(ByVal OrderId As Integer, ByVal IsCheckSend As Boolean)
        Dim _basePage As New BasePage
        Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(_basePage.DB, OrderId)


        If dbOrder.OrderId > 0 And dbOrder.MemberId > 0 Then
            'Email.SendError("ToErrorWebService", "[NailCahe] IF _SendOrderConfirmation(ByVal OrderId As Integer, ByVal IsCheckSend As Boolean): " & OrderId, IsCheckSend)
            If IsCheckSend Then
                If dbOrder.IsSentConfirm Then
                    Email.SendError("ToErrorWebService", "[NailCahe] _SendOrderConfirmation: " & OrderId, "Check OrderNo: " & dbOrder.OrderNo & " was sent")
                    Exit Sub
                End If
            End If

            BaseShoppingCart.SendOrderConfirmation(OrderId)
        Else
            'Email.SendError("ToErrorWebService", "[NailCahe] ELSE _SendOrderConfirmation(ByVal OrderId As Integer, ByVal IsCheckSend As Boolean): " & OrderId, IsCheckSend)
        End If
    End Sub

    <WebMethod()> _
    Public Sub ClearCacheWithPrefix(ByVal Prefix As String)
        Try
            Utility.CacheUtils.RemoveCacheItemWithPrefix(Prefix)

        Catch ex As Exception

        End Try

    End Sub
    <WebMethod()> _
    Public Function ClearCacheWithPrefixArray(ByVal ParamArray prefix() As String) As String
        Dim result As String = String.Empty
        Try
            result = Utility.CacheUtils.ClearCacheWithPrefix(prefix)
        Catch ex As Exception

        End Try
        Return result
    End Function
    <WebMethod()> _
   Public Sub RemoveCache(ByVal prefix As String)

        Try
            Utility.CacheUtils.RemoveCache(prefix)
        Catch ex As Exception

        End Try

    End Sub

    <WebMethod()> _
    Public Function BindCountLikeFB(ByVal urltxt As String) As Integer
        Dim iCountLike As Integer = 0
        Dim details As New UrlDetails()
        Dim web As New WebClient()

        Dim url As String = String.Format("https://api.facebook.com/method/fql.query?query=SELECT url, share_count, like_count, comment_count, total_count, click_count FROM link_stat where url='" & urltxt & "'")
        Dim response As String = web.DownloadString(url)
        Dim ds As New DataSet()
        Using stringReader As New StringReader(response)
            ds = New DataSet()
            ds.ReadXml(stringReader)
        End Using

        Try
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                Dim dt As DataTable = ds.Tables("link_stat")
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dtrow As DataRow In dt.Rows
                        iCountLike = CInt(dtrow("like_count"))
                    Next
                Else
                    iCountLike = -1
                End If
            Else
                iCountLike = -1
            End If
        Catch ex As Exception

        End Try

        Return iCountLike
    End Function
    Public Class UrlDetails
        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(ByVal value As String)
                m_Url = value
            End Set
        End Property
        Private m_Url As String
        Public Property SharedCount() As String
            Get
                Return m_SharedCount
            End Get
            Set(ByVal value As String)
                m_SharedCount = value
            End Set
        End Property
        Private m_SharedCount As String
        Public Property LikeCount() As String
            Get
                Return m_LikeCount
            End Get
            Set(ByVal value As String)
                m_LikeCount = value
            End Set
        End Property
        Private m_LikeCount As String
        Public Property CommentCount() As String
            Get
                Return m_CommentCount
            End Get
            Set(ByVal value As String)
                m_CommentCount = value
            End Set
        End Property
        Private m_CommentCount As String
        Public Property ClickCount() As String
            Get
                Return m_ClickCount
            End Get
            Set(ByVal value As String)
                m_ClickCount = value
            End Set
        End Property
        Private m_ClickCount As String
        Public Property TotalCount() As String
            Get
                Return m_TotalCount
            End Get
            Set(ByVal value As String)
                m_TotalCount = value
            End Set
        End Property
        Private m_TotalCount As String
    End Class


End Class
