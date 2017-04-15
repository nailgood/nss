Imports System.Data.SqlClient
Imports DataLayer
Imports Components

Public Class RequestCatalog
    Inherits System.Web.UI.UserControl

    Public Property MemberId() As Integer
        Get
            Return m_MemberId
        End Get
        Set(ByVal Value As Integer)
            m_MemberId = Value
        End Set
    End Property

    Public Property CatalogId() As Integer
        Get
            Return m_CatalogId
        End Get
        Set(ByVal Value As Integer)
            m_CatalogId = Value
        End Set
    End Property

    Public Property DB() As Database
        Get
            Return m_DB
        End Get
        Set(ByVal Value As Database)
            m_DB = Value
        End Set
    End Property

    Public Property IsAdmin() As Boolean
        Get
            Return m_IsAdmin
        End Get
        Set(ByVal Value As Boolean)
            m_IsAdmin = Value
        End Set
    End Property

    Private m_MemberId As Integer
    Private m_CatalogId As Integer
    Private m_DB As Database
    Private m_IsAdmin As Boolean
    Private m_ErrHandler As ErrorHandler
    Private params As String
    Private curPage As BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SQL As String
        curPage = CType(Page, BasePage)
        m_ErrHandler = curPage.ErrHandler
        params = curPage.GetPageParams(FilterFieldType.All)

        If CatalogId <> 0 And IsAdmin = True Then Delete.Visible = True Else Delete.Visible = False

        If Not Page.IsPostBack Then
            State.Items.AddRange(StateRow.GetStateList().ToArray())
            State.DataBind()

            SQL = "SELECT [Value],ItemId FROM LookupItem WHERE ListId = 1 ORDER BY SortOrder"

            Salutation.DataSource = DB.GetDataSet(SQL)
            Salutation.DataTextField = "Value"
            Salutation.DataValueField = "ItemId"
            Salutation.DataBind()

            If CatalogId <> 0 Then
                Dim dbCatalog As StoreCatalogRequestRow = StoreCatalogRequestRow.GetRow(DB, CatalogId)
                FirstName.Text = dbCatalog.FirstName
                LastName.Text = dbCatalog.LastName
                Email.Text = dbCatalog.Email
                Company.Text = dbCatalog.Company
                Address1.Text = dbCatalog.Address1
                Address2.Text = dbCatalog.Address2
                City.Text = dbCatalog.City
                ViewState("CatalogMemberId") = dbCatalog.MemberId
                State.SelectedValue = dbCatalog.State
                Zip.Value = dbCatalog.Zip
                Phone.Text = dbCatalog.Phone
                Salutation.SelectedValue = dbCatalog.Salutation
                Ext.Text = dbCatalog.PhoneExt
                ltlDateRequested.Text = dbCatalog.DateRequested
            End If
            'DB.Close()
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not Page.IsValid Then Exit Sub
        Try
            '           DB.Open(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
            DB.BeginTransaction()

            If CatalogId <> 0 Then
                Dim dbCatalog As StoreCatalogRequestRow = StoreCatalogRequestRow.GetRow(DB, CatalogId)
                dbCatalog.FirstName = FirstName.Text
                dbCatalog.LastName = LastName.Text
                dbCatalog.Email = Email.Text
                dbCatalog.Company = Company.Text
                dbCatalog.Address1 = Address1.Text
                dbCatalog.Address2 = Address2.Text
                dbCatalog.City = City.Text
                dbCatalog.State = State.SelectedValue
                dbCatalog.Zip = Zip.Value
                dbCatalog.Phone = Phone.Text
                dbCatalog.Salutation = Salutation.SelectedValue
                dbCatalog.PhoneExt = Ext.Text
                dbCatalog.MemberId = ViewState("CatalogMemberId")
                dbCatalog.Update()
            Else
                Dim dbCatalog As StoreCatalogRequestRow = New StoreCatalogRequestRow(DB)
                dbCatalog.FirstName = FirstName.Text
                dbCatalog.LastName = LastName.Text
                dbCatalog.Email = Email.Text
                dbCatalog.Company = Company.Text
                dbCatalog.Address1 = Address1.Text
                dbCatalog.Address2 = Address2.Text
                dbCatalog.City = City.Text
                dbCatalog.State = State.SelectedValue
                dbCatalog.Zip = Zip.Value
                dbCatalog.Phone = Phone.Text
                dbCatalog.Salutation = Salutation.SelectedValue
                dbCatalog.PhoneExt = Ext.Text
                If IsAdmin = True Then dbCatalog.MemberId = 0 Else dbCatalog.MemberId = MemberId
                dbCatalog.Insert()
            End If
			DB.CommitTransaction()
			DB.Close()
            If IsAdmin = True Then Response.Redirect("default.aspx?" & params) Else Response.Redirect("thankyou.aspx")
        Catch ex As ApplicationException
			DB.RollbackTransaction()
			curPage.AddError(ex.Message)
		Catch ex As SqlException
			DB.RollbackTransaction()
			curPage.AddError(curPage.ErrHandler.ErrorText(ex))
		Finally
			'            If Not DB Is Nothing Then DB.Close()
        End Try
    End Sub

	Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
		DB.Close()
		If IsAdmin = True Then Response.Redirect("default.aspx?" & params) Else Response.Redirect("/Catalog/default.aspx")
	End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
		If CatalogId <> 0 Then
			DB.Close()
			Response.Redirect("delete.aspx?RequestId=" & CatalogId & "&" & params)
		End If
    End Sub
End Class