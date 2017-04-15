Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class controls_product_item_description
    Inherits BaseControl
    Private m_SKU As String = Nothing
    Public Property SKU() As String
        Set(ByVal value As String)
            m_SKU = value
        End Set
        Get
            Return m_SKU
        End Get
    End Property
    Private m_Description As String = Nothing
    Public Property Description() As String
        Set(ByVal value As String)
            m_Description = value
        End Set
        Get
            Return m_Description
        End Get
    End Property
    Public Sub LoadDescription()
        'ltrSKU.Text = IIf(Not String.IsNullOrEmpty(SKU), "Item#: " & SKU, "")
        ltrDescription.Text = Description
    End Sub
    Private m_Item As StoreItemRow = Nothing

    Public Property Item() As StoreItemRow
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
        Get
            Return m_Item
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Item Is Nothing AndAlso Not Session("itemRender") Is Nothing Then
            Item = Session("itemRender")
            'ltrSKU.Text = IIf(Not String.IsNullOrEmpty(Item.SKU), "Item#: " & Item.SKU, "")
            ltrDescription.Text = BBCodeHelper.ConvertBBCodeToHTML(Item.LongDesc)
        End If

    End Sub

End Class
