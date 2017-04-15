Imports System
Imports System.IO

Partial Class ImageDropDown
    Inherits System.Web.UI.UserControl

    Private _Folder As String

    Public Property Folder() As String
        Get
            Return _Folder
        End Get
        Set(ByVal Value As String)
            _Folder = Value
        End Set
    End Property

    Public Property Required() As Boolean
        Get
            Return Viewstate("Required")
        End Get
        Set(ByVal Value As Boolean)
            Viewstate("Required") = Value
        End Set
    End Property

    Public Property ImageName() As String
        Get
            Return drpImages.SelectedValue
        End Get
        Set(ByVal Value As String)
            drpImages.SelectedValue = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        drpImages.Attributes("OnChange") = "this.form." & imgPreview.ClientID & ".src = eval('this[this.selectedIndex].value') == '' ? '/includes/theme-admin/images/spacer.gif' : '" & Folder & "/' + eval('this[this.selectedIndex].value')"

        If Not IsPostBack Then
            BindDropDown()
        End If
        If ImageName = String.Empty Then
            imgPreview.ImageUrl = "/includes/theme-admin/images/spacer.gif"
        Else
            imgPreview.ImageUrl = Folder & "/" & ImageName
        End If
        valRequiredImage.Enabled = Required
    End Sub

    Private Sub BindDropDown()
        Dim DirInfo As DirectoryInfo = New DirectoryInfo(Server.MapPath(Folder))

        drpImages.DataSource = DirInfo.GetFiles("*.gif")
        drpImages.DataTextField = "Name"
        drpImages.DataValueField = "Name"
        drpImages.DataBind()
        drpImages.Items.Insert(0, New ListItem(""))
    End Sub
End Class
