Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_cartsave
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Cart As ShoppingCart = Nothing

    Private MemberId As Integer
    Protected SaveCartCount As String = String.Empty
    Public Sub New(ByVal objCart As ShoppingCart)
        Cart = objCart
    End Sub
    Public Sub New()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MemberId = Utility.Common.GetCurrentMemberId()
        If MemberId < 1 Then
            Exit Sub
        End If

        If Not IsPostBack Then
            BindData()
        End If
    End Sub
    

    Private Sub BindData()

        Dim list As List(Of SaveCartRow) = SaveCartRow.ListByMemberId(MemberId)
        If list.Count > 0 Then
            rptCartSave.DataSource = list
            rptCartSave.DataBind()
            rptCartSave.Visible = True

            SaveCartCount = String.Format("Saved for later (<span id=""savecart-count"">{0} item{1}</span>)", list.Count, IIf(list.Count > 1, "s", ""))
        Else
            rptCartSave.Visible = False
        End If
        
    End Sub
    
    Private Sub rptCartSave_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartSave.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim mixMatchId As Integer = 0
            Dim sc As SaveCartRow = e.Item.DataItem
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, sc.ItemId)
            
            Dim sURL As String = URLParameters.ProductUrl(si.URLCode, si.ItemId)
            Dim ltrImage As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrImage"), System.Web.UI.WebControls.Literal)

            If Not ltrImage Is Nothing Then
                If String.IsNullOrEmpty(si.Image) Then
                    si.Image = "na.jpg"
                End If
                ltrImage.Text = "<a href='" & sURL & "'><img src='" & Utility.ConfigData.CDNMediaPath & "/assets/items/related/" & si.Image & "'/></a>"
            End If

            Dim trRow As HtmlTableRow = CType(e.Item.FindControl("trRow"), HtmlTableRow)
            trRow.ID = "trSaveRow_" & sc.ItemId.ToString()
            trRow.ClientIDMode = UI.ClientIDMode.Static

            Dim hplRemove As System.Web.UI.WebControls.HyperLink = CType(e.Item.FindControl("hplRemove"), System.Web.UI.WebControls.HyperLink)
            If Not hplRemove Is Nothing Then
                hplRemove.Attributes.Add("OnClick", "OnRemoveSaveCartItem('" & si.ItemId & "'," & IIf(sc.Type = "case", "true", "false") & ");")
            End If

            Dim hplMove As System.Web.UI.WebControls.HyperLink = CType(e.Item.FindControl("hplMove"), System.Web.UI.WebControls.HyperLink)
            If Not hplMove Is Nothing Then
                hplMove.Attributes.Add("OnClick", "OnMoveSaveCartItem('" & sc.ItemId & "', " & sc.Qty & ", " & IIf(sc.Type = "case", "true", "false") & ");")
            End If

            Dim strItemName As String = si.ItemName
            Dim strPriceDesc As String = IIf(si.PriceDesc <> Nothing, " - " & si.PriceDesc, "")
            Dim measure As String = SitePage.ShowMeasurement(si.PriceDesc, si.Measurement)
            strPriceDesc &= IIf(measure.Length > 0, " (" & measure & ")", "")

            Dim divItemName As HtmlGenericControl = CType(e.Item.FindControl("divItemName"), HtmlGenericControl)
            If Not divItemName Is Nothing Then
                divItemName.InnerHtml = "<a href='" & sURL & "'>" & Server.HtmlEncode(strItemName) & strPriceDesc & "</a>"
            End If

            Dim divQty As HtmlGenericControl = CType(e.Item.FindControl("divQty"), HtmlGenericControl)
            If Not divQty Is Nothing Then
                divQty.InnerHtml = "<span class=""text"">QTY: </span> "

                If sc.Type = "case" Then
                    divQty.InnerHtml &= String.Format("<span class=""caseqty"">{0} {1}</span>", sc.Qty, IIf(sc.Qty > 1, " cases", " case"))
                Else
                    divQty.InnerHtml &= sc.Qty
                End If
            End If

            Dim divSKU As HtmlGenericControl = CType(e.Item.FindControl("divSKU"), HtmlGenericControl)
            If Not divSKU Is Nothing Then
                divSKU.InnerHtml = String.Format("Item# {0}", si.SKU)
            End If

            Dim tdPrice As HtmlTableCell = CType(e.Item.FindControl("tdPrice"), HtmlTableCell)
            Dim divSmallPrice As HtmlGenericControl = CType(e.Item.FindControl("divSmallPrice"), HtmlGenericControl)
            Dim Price As String = String.Empty
            If sc.Type <> "case" Then
                If sc.ItemPriceSale = 0 Or sc.ItemPriceSale > sc.ItemPriceNotSale Then
                    Price = FormatCurrency(sc.ItemPriceNotSale)
                Else
                    Price = String.Format("<span class='strike'>{1}</span><br><span class='red'>{0}</span>", FormatCurrency(sc.ItemPriceSale), FormatCurrency(sc.ItemPriceNotSale))
                End If
            Else
                If sc.CasePriceCartNotSale = 0 Or sc.CasePriceCartNotSale = Nothing Then
                    Price = FormatCurrency(sc.CasePriceCart)
                Else
                    Price = String.Format("<span class='strike'>{1}</span><br><span class='red'>{0}</span>", FormatCurrency(sc.CasePriceCart), FormatCurrency(sc.CasePriceCartNotSale))
                End If
            End If

            tdPrice.InnerHtml = Price.Replace("&nbsp;&nbsp;", "<br>").Replace("bold", "")
            divSmallPrice.InnerHtml = Price.Replace("bold", "")
            'Show error
            Dim bOutOfStock As Boolean = False
            If Not bOutOfStock Then
                divSKU.InnerHtml &= String.Format(" &nbsp;|&nbsp; Availability: In Stock")
            End If
        End If
    End Sub
    
End Class
