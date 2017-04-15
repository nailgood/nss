Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel

Namespace Controls

    '<summary>
    'IdDdl Server Control Class for System.Web.UI.WebControls.DropDownList Tag
    '</summary>

    <DefaultProperty("TableName"), ToolboxData("<{0}:IdDdl runat=server></{0}:IdDdl>")> _
    Public Class AEDropDownList
        Inherits System.Web.UI.WebControls.DropDownList

        Dim myDB As Database
        Dim dt As DataTable
        Private _TableName As String
        Private _DisplayColumn As String
        Private _ValueColumn As String
        Private _sCondition As String = ""
        Private _sOrderBy As String = ""
        Private _DisplayColumnSeperator As String = "-"
        Private _ValueColumnSeperator As String = "#"
        Private _FirstField As Boolean = False
        Private _FirstFieldText As String = ""
        Private _FirstFieldValue As String = ""
        Private _bLoaded As Boolean = False

        '/ <summary>
        '/ Property 'TableName' to get the DropDownList Table Name from the Server Control Tag.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property TableName() As String
            Get
                Return _TableName
            End Get
            Set(ByVal Value As String)
                _TableName = Value
            End Set
        End Property

        '/ <summary>
        '/ Property 'DisplayColumn' to get the DropDownList Display Column from the Server Control Tag.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property DisplayColumn() As String
            Get
                Return _DisplayColumn
            End Get
            Set(ByVal Value As String)
                _DisplayColumn = Value
            End Set
        End Property

        '/ <summary>
        '/ Property 'ValueColumn' to get the DropDownList Value Columns from the Server Control Tag.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property ValueColumn() As String
            Get
                Return _ValueColumn
            End Get
            Set(ByVal Value As String)
                _ValueColumn = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property Condition() As String
            Get
                Return _sCondition
            End Get
            Set(ByVal Value As String)
                _sCondition = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property OrderBy() As String
            Get
                Return _sOrderBy
            End Get
            Set(ByVal Value As String)
                _sOrderBy = Value
            End Set
        End Property

        '/ <summary>
        '/ Property 'DisplayColumnSeperator' to get the DropDownList Display Column Seperator from the Server Control Tag.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property DisplayColumnSeperator() As String
            Get
                Return _DisplayColumnSeperator
            End Get
            Set(ByVal Value As String)
                _DisplayColumnSeperator = Value
            End Set
        End Property

        '/ <summary>
        '/ Property 'ValueColumnSeperator' to get the DropDownList Value Column Seperator from the Server Control Tag.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property ValueColumnSeperator() As String
            Get
                Return _ValueColumnSeperator
            End Get
            Set(ByVal Value As String)
                _ValueColumnSeperator = Value
            End Set
        End Property



        '/ <summary>
        '/ Property 'FirstField' to set the DropDownList Text and Value Column [Default : Set true / false to set the First Text and Value as ""]
        '/ You can set any other Text by setting 'FirstField' as true and set any Text for 'FirstFieldText'.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property FirstField() As Boolean
            Get
                Return _FirstField
            End Get
            Set(ByVal Value As Boolean)
                _FirstField = Value
            End Set
        End Property

        '/ <summary>
        '/ Property 'FirstFieldText' to set the DropDownList First Text and Value Column.
        '/ Browsable from the Visual Studio.NET Designer.
        '/ </summary>

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property FirstFieldText() As String
            Get
                If Nothing = Me._FirstFieldText Then
                    Me.FirstFieldText = ""
                End If
                Return _FirstFieldText
            End Get
            Set(ByVal Value As String)
                _FirstFieldText = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(True)> _
        Public Property FirstFieldValue() As String
            Get
                If Nothing = Me._FirstFieldValue Then
                    Me.FirstFieldValue = ""
                End If
                Return _FirstFieldValue
            End Get
            Set(ByVal Value As String)
                _FirstFieldValue = Value
            End Set
        End Property


        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(False)> _
        Public Overrides Property DataSource() As Object
            Get
                Return MyBase.DataSource
            End Get
            Set(ByVal Value As Object)
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(False)> _
        Public Overrides Property DataTextField() As String
            Get
                Return MyBase.DataTextField
            End Get
            Set(ByVal Value As String)
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(False)> _
        Public Property DB() As Database
            Get
                Return myDB
            End Get
            Set(ByVal Value As Database)
                myDB = Value
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(False)> _
        Public Overrides Property DataValueField() As String
            Get
                Return MyBase.DataValueField
            End Get
            Set(ByVal Value As String)
            End Set
        End Property

        <Bindable(True), Category("Appearance"), DefaultValue(""), Browsable(False)> _
        Public Overrides Property DataMember() As String
            Get
                Return MyBase.DataMember
            End Get
            Set(ByVal Value As String)
            End Set
        End Property

        '/ <summary>
        '/ Default Constructor
        '/ </summary>

        Public Sub AEDropDownList()

        End Sub

        Private Sub LoadDataInListItem()
            Try
                Dim bOpened As Boolean = False

                If (_bLoaded) Then Return

                _bLoaded = True

                Me.ValueColumn = Me.ValueColumn.Replace(",", " ||'" + Me.ValueColumnSeperator + "'|| ")
                Me.DisplayColumn = Me.DisplayColumn.Replace(",", " ||'" + Me.DisplayColumnSeperator + "'|| ")
                If (Me._sCondition <> "") Then
                    Me._sCondition = " where " + Me._sCondition
                End If
                If (Me._sOrderBy = "") Then
                    Me._sOrderBy = Me.DisplayColumn
                End If
                Dim strQry As String = "SELECT " + Me.ValueColumn + " VALUE_FIELD, " + Me.DisplayColumn + " DISPLAY_FIELD FROM " + Me.TableName + Me._sCondition + " ORDER BY " + Me._sOrderBy

                If (Not myDB.IsOpen()) Then
                    bOpened = True
                    ' -- Edit connection string By Trung Nguyen --
                    'myDB.Open(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
                    myDB.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                End If

                dt = myDB.GetDataSet(strQry).Tables(0)
                dt.TableName = Me.tableName

                If (bOpened) Then
                    myDB.Close()
                End If

                Me.Items.Add(New ListItem("Executing...", "Executing..."))

                Dim i As Integer
                For i = 0 To dt.Rows.Count - 1
                    Me.Items.Add(New ListItem(dt.Rows(i)("DISPLAY_FIELD").ToString(), dt.Rows(i)("VALUE_FIELD").ToString()))
                Next

                If (Me.firstField) Then
                    If (Me.Items(0).Text <> "") Then
                        Me.Items.Insert(0, New ListItem(Me.firstFieldText, Me.firstFieldValue))
                    End If
                End If
            Catch ex As Exception
            End Try
        End Sub

        Protected Overrides Function SaveViewState() As Object
            Dim objArr(Me.Items.Count + 1) As Object
            Try
                If (Not Page.IsPostBack) Then
                    Me.LoadDataInListItem()
                End If

                Dim baseState As Object = MyBase.SaveViewState()
                objArr(0) = baseState
            Catch ex As Exception
            End Try

            Return objArr
        End Function

        Protected Overrides Sub LoadViewState(ByVal savedState As Object)
            Try
                If (Not savedState Is DBNull.Value) Then
                    Dim objSaveStateArr() As Object = savedState()
                    If (Not objSaveStateArr(0) Is DBNull.Value) Then
                        MyBase.LoadViewState(objSaveStateArr(0))
                    End If
                End If
            Catch ex As Exception
            End Try
        End Sub

        Protected Overrides Sub RenderContents(ByVal htw As HtmlTextWriter)
            Try
                Dim li As ListItem
                For Each li In Me.Items
                    htw.WriteBeginTag("option")
                    If li.Selected Then
                        htw.WriteAttribute("selected", "selected", False)
                        li.Attributes.Render(htw)
                        htw.WriteAttribute("value", li.Value.ToString())
                        htw.Write(HtmlTextWriter.TagRightChar)
                        htw.Write(li.Text)
                        htw.WriteEndTag("option")
                        htw.WriteLine()
                    End If
                Next
            Catch ex As Exception
                htw.WriteBeginTag("option")
                htw.WriteAttribute("value", "Error")
                htw.Write(HtmlTextWriter.TagRightChar)
                htw.Write(ex.Message)
                htw.WriteEndTag("option")
                htw.WriteLine()
            End Try
            Try
                Me.DataBind()
            Catch ex As Exception
                MyBase.RenderContents(htw)
            End Try
        End Sub
    End Class
End Namespace