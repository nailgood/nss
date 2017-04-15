Imports System.Web.UI.WebControls
Imports System.Data
Imports Components
Imports DataLayer

Partial Class controls_layout_menu_main_category
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_DepartmentId As Integer
    Public Property DepartmentId() As Integer
        Set(ByVal value As Integer)
            m_DepartmentId = value
        End Set
        Get
            Return m_DepartmentId
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        'If DepartmentId < 0 Then
        '    If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
        '        DepartmentId = CInt(GetQueryString("DepartmentId"))
        '    End If
        'End If
        If (DepartmentId > 0) Then
            Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1
                    Dim dr As DataRow = ds.Tables(0).Rows(i)
                    Dim URLCode As String = dr("URLCode")
                    Dim currentDepartmentId As Integer = CInt(dr("DepartmentId"))
                    Dim mainUrl As String = URLParameters.MainDepartmentUrl(URLCode, currentDepartmentId)
                    ''  Dim ElementId As String = IIf(dr("isQuickOrder"), Utility.Common.DepartmentType.nailcollection, Utility.Common.DepartmentType.nailsupplies) & "-" & URLCode & "-" & currentDepartmentId
                    Dim mainName As String = IIf(Not IsDBNull(dr("AlternateName")), dr("AlternateName"), dr("Name"))
                    If DepartmentId = currentDepartmentId Then
                        ltrMainDepartment.Text &= "<li class='active'><span class='text'>" & mainName & "</span> <span class='icon'>&nbsp;</span></li>"
                    Else
                        ltrMainDepartment.Text &= "<li ><a href='" & mainUrl & "' >" & mainName & "<span class='icon'></span></a></li>"
                    End If

                Next
            End If
        End If
    End Sub

End Class
