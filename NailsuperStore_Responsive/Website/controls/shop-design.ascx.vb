Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class controls_shop_design
    Inherits BaseControl
    Public m_index As Integer = 0
    Private m_ShopDesign As ShopDesignRow = Nothing
    Public Property ShopDesign() As ShopDesignRow
        Set(ByVal value As ShopDesignRow)
            m_ShopDesign = value
        End Set
        Get
            Return m_ShopDesign
        End Get
    End Property
    Public Property index() As Integer
        Set(ByVal value As Integer)
            m_index = value
        End Set
        Get
            Return m_index
        End Get
    End Property
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BinData()
        If Not Page.IsPostBack Then
            'update log
            ViewedItemRow.updateSearchResult(String.Empty, Request.RawUrl, "List")
        End If
    End Sub

    Public Sub BinData()
        Try
            If index < 1 AndAlso Not Session("indexRender") Is Nothing Then
                index = Session("indexRender")
            End If
            If ShopDesign Is Nothing AndAlso Not Session("shopDesignRender") Is Nothing Then
                ShopDesign = Session("shopDesignRender")
            End If
            'Dim strclass As String = ""
            'If ((index Mod 3) = 0) Then
            '    strclass = " last"
            'Else
            '    strclass = String.Empty
            'End If

            If ShopDesign Is Nothing Then
                Return
            End If

            divShopDesign.Text = "<div class='col-sm-4 dvShopDesign" & "' id='shopDesignItem_" & index & "'><article><div id='imgShopDesign_" & index & "' class='image'><a href='" & URLParameters.ShopDesignUrl(ShopDesign.Title, ShopDesign.ShopDesignId) & "'><img src='" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ShopDesignThumbPath & ShopDesign.Image & "' alt='" & ShopDesign.Title & "'/></a></div>"
            divShopDesign.Text &= "<div class='title'><a href='" & URLParameters.ShopDesignUrl(ShopDesign.Title, ShopDesign.ShopDesignId) & "'>" & ShopDesign.Title.ToString() & "</a></div>"
            divShopDesign.Text &= "<div class='shortdesc'>" & ShopDesign.ShortDescription.ToString() & "</div>"
            divShopDesign.Text &= "<div class='tb-img'><a href='" & URLParameters.ShopDesignUrl(ShopDesign.Title, ShopDesign.ShopDesignId) & "' class='bt-add-cart'>Buy This Design</a></div></article></div>"
        Catch ex As Exception
            Email.SendError("ToError500", "Control Shop Design", "Index: " & index & "<br>Exception: " & ex.ToString())
        End Try
    End Sub
End Class
