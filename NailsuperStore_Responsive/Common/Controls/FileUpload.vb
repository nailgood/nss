Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Text
Imports System.Web
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Collections
Imports System.Collections.Specialized
Imports Components

Namespace Controls

    Public Class FileUpload
        Inherits System.Web.UI.WebControls.WebControl
        Implements System.Web.UI.IPostBackDataHandler

        Private m_OldFileName As String = String.Empty
		Private m_NewFileName As String = String.Empty
		Private m_OriginalName As String = String.Empty
		Private m_OriginalExtension As String = String.Empty
        Private m_Checked As Boolean
		Private m_Required As Boolean = False
		Private m_ImageWidth As Integer = Nothing
		Private m_ImageHeight As String = Nothing
		Private m_AutoResize As Boolean = False

		'TO - 11/14/2006
		Private m_PersistFileName As Boolean = False

		'TO - 8/22/2006
		Private m_EnforceSlash As Boolean = True

		'TO - 9/18/2006
		Private m_EnableDelete As Boolean = True

        Private chk As New HtmlInputCheckBox
        Private hidden As New HtmlInputHidden
        Private File As New HtmlInputFile

		Public MyFile As HttpPostedFile

		Public Property ImageWidth() As Integer
			Get
				Return m_ImageWidth
			End Get
			Set(ByVal value As Integer)
				m_ImageWidth = value
			End Set
        End Property
        Public Property OriginalExtension() As String
            Get
                Return m_OriginalExtension
            End Get
            Set(ByVal value As String)
                m_OriginalExtension = value
            End Set
        End Property

		Public Property ImageHeight() As Integer
			Get
				Return m_ImageHeight
			End Get
			Set(ByVal value As Integer)
				m_ImageHeight = value
			End Set
		End Property

		Public Property AutoResize() As Boolean
			Get
				Return m_AutoResize AndAlso Not ImageWidth = Nothing AndAlso Not ImageHeight = Nothing
			End Get
			Set(ByVal value As Boolean)
				m_AutoResize = value
			End Set
		End Property

		Public Property PersistFileName() As Boolean
			Get
				Return m_PersistFileName
			End Get
			Set(ByVal value As Boolean)
				m_PersistFileName = value
			End Set
		End Property

        Public Property Folder() As String
            Get
                Dim f As String = CStr(ViewState("Folder"))

				'TO - 8/22/2006
				If m_EnforceSlash Then
					If Not Right(f, 1) = "/" Then
						f &= "/"
					End If
				End If

				Return f
			End Get
            Set(ByVal value As String)
                ViewState("Folder") = value
            End Set
        End Property

        Public Property ImageDisplayFolder() As String
            Get
				Dim f As String = CStr(ViewState("ImageDisplayFolder"))

				'TO - 8/22/2006
				If m_EnforceSlash Then
					If Not Right(f, 1) = "/" Then
						f &= "/"
					End If
				End If

				Return f
			End Get
            Set(ByVal value As String)
                ViewState("ImageDisplayFolder") = value
            End Set
		End Property

		'TO - 8/22/2006
		Public Property EnforceSlash() As Boolean
			Get
				Return m_EnforceSlash
			End Get
			Set(ByVal value As Boolean)
				m_EnforceSlash = value
			End Set
		End Property

		'TO - 9/18/2006
		Public Property EnableDelete() As Boolean
			Get
				Return m_EnableDelete
			End Get
			Set(ByVal value As Boolean)
				m_EnableDelete = value
			End Set
		End Property

        Public Property DisplayImage() As Boolean
            Get
                Return CBool(ViewState("DisplayImage"))
            End Get
            Set(ByVal value As Boolean)
                ViewState("DisplayImage") = value
            End Set
        End Property

        Public Property CurrentFileName() As String
            Get
                Return CStr(ViewState("CurrentFileName"))
            End Get
            Set(ByVal value As String)
                ViewState("CurrentFileName") = value
            End Set
        End Property

        Public Property Required() As Boolean
            Get
                Return m_Required
            End Get
            Set(ByVal value As Boolean)
                m_Required = value
            End Set
        End Property

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            Page.RegisterRequiresPostBack(Me)
            MyBase.OnInit(e)
        End Sub

        Public Sub New()
        End Sub

        Public ReadOnly Property MarkedToDelete()
            Get
                Me.EnsureChildControls()
                If IsNothing(MyFile) Then LoadValues()
                Return m_Checked
            End Get
        End Property

        Public Sub RemoveOldFile()
            Me.EnsureChildControls()
            If IsNothing(MyFile) Then LoadValues()
			Try
				System.IO.File.Delete(HttpContext.Current.Server.MapPath(Folder & CurrentFileName))
				If Not (IsNothing(ImageDisplayFolder) OrElse ImageDisplayFolder = Nothing) Then
					System.IO.File.Delete(HttpContext.Current.Server.MapPath(ImageDisplayFolder & CurrentFileName))
				End If
			Catch ex As Exception
			End Try
		End Sub
        Public Sub RemoveFileName(ByVal Path As String, ByVal FileName As String)
            Try
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(Path & FileName))
            Catch ex As Exception
            End Try
        End Sub
        'Public ReadOnly Property NewFileName() As String
        '    Get
        '        Me.EnsureChildControls()
        '        If IsNothing(MyFile) Then LoadValues()
        '        Return m_NewFileName
        '    End Get
        'End Property
        Public Property NewFileName() As String
            Get
                Me.EnsureChildControls()
                If IsNothing(MyFile) Then LoadValues()
                Return m_NewFileName
            End Get
            Set(ByVal value As String)
                m_NewFileName = value
            End Set
        End Property

        Public ReadOnly Property SpecifiedFileName() As String
            Get
                Return m_OriginalName & m_OriginalExtension
            End Get
        End Property

        Private Function GetFileName() As String
            Dim FileName As String = String.Empty
            Try
                FileName = System.IO.Path.GetFileName(MyFile.FileName)
            Catch ex As Exception
                FileName = String.Empty
            End Try

            If FileName = String.Empty Then Return String.Empty
            m_OriginalName = System.IO.Path.GetFileNameWithoutExtension(FileName)
            m_OriginalExtension = System.IO.Path.GetExtension(FileName)
            m_OriginalName = m_OriginalName.Replace(" ", "_")
            m_OriginalName = m_OriginalName.Replace(",", "_")
            m_OriginalName = m_OriginalName.Replace(".", "_")
            Dim tmpName As String = m_OriginalName & m_OriginalExtension
            Dim iCounter As Integer = 1
            If Not PersistFileName Then
                While System.IO.File.Exists(HttpContext.Current.Server.MapPath(Folder & tmpName))
                    tmpName = m_OriginalName & iCounter.ToString() & m_OriginalExtension
                    iCounter += 1
                End While
            Else
                Try
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(Folder & tmpName))
                Catch ex As Exception
                End Try
            End If
            Return tmpName
        End Function

        Public Sub SaveNewFile()
            Me.EnsureChildControls()

            If IsNothing(MyFile) Then LoadValues()

            If NewFileName = String.Empty Then Exit Sub

            If MyFile.ContentLength = 0 Then Throw New ArgumentException("Error while attempting to save file: " & NewFileName)

            MyFile.SaveAs(HttpContext.Current.Server.MapPath(Folder & NewFileName))

            If AutoResize Then
                Core.ResizeImage(HttpContext.Current.Server.MapPath(Folder & NewFileName), HttpContext.Current.Server.MapPath(Folder & NewFileName), ImageWidth, ImageHeight)
            End If
        End Sub

        Public Sub SaveThumbnail(ByVal Width As Integer, ByVal Height As Integer)
            Core.ResizeImage(HttpContext.Current.Server.MapPath(Folder & NewFileName), HttpContext.Current.Server.MapPath(ImageDisplayFolder & NewFileName), Width, Height)
        End Sub

        Protected Overrides Sub CreateChildControls()
            Dim panel As Panel = New Panel()
            Dim link As HyperLink = New HyperLink
            Dim img As Image = New Image
            Dim lbreak As Label = New Label

            Controls.Clear()

            Dim ltl As LiteralControl

            If DisplayImage Then
                If EnableDelete Then
                    ltl = New LiteralControl("delete this image")
                End If
            Else
                If EnableDelete Then
                    ltl = New LiteralControl("delete this file")
                End If
                panel.Controls.Add(New LiteralControl("<FONT size=""1"">Current File:</FONT>"))
            End If

            If DisplayImage Then
                panel.Controls.Add(img)
                img.ImageUrl = ImageDisplayFolder & CurrentFileName
            Else
                panel.Controls.Add(link)
                link.ID = Me.ID.ToString() & "_LINK"
                link.Target = "_blank"
            End If

            panel.Controls.Add(lbreak)

            If EnableDelete Then
                chk.ID = Me.ID.ToString() & "_CHK"
                chk.Value = "Y"
                panel.Controls.Add(chk)
                panel.Controls.Add(ltl)
            End If

            panel.ID = Me.ID.ToString() & "_PNL"
            Controls.Add(panel)

            panel.Visible = Not IsNothing(CurrentFileName)
            link.NavigateUrl = Folder & CurrentFileName
            link.Text = CurrentFileName
            If EnableDelete Then
                chk.Visible = (Not IsNothing(CurrentFileName) AndAlso Not Required)
                ltl.Visible = (Not IsNothing(CurrentFileName) AndAlso Not Required)
            End If
            lbreak.Text = IIf(chk.Visible, "<br>", "")

            hidden.ID = Me.ID.ToString() & "_OLD"
            hidden.Value = CurrentFileName
            Controls.Add(hidden)

            File.ID = Me.ID.ToString() & "_FILE"
            File.Attributes("style") = Me.Attributes("style")
            File.Attributes("class") = Me.Attributes("class")
            Controls.Add(File)
        End Sub

        Private Sub LoadValues()
            MyFile = File.PostedFile
            m_OldFileName = hidden.Value
            m_NewFileName = GetFileName()
        End Sub

        Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim context As HttpContext = HttpContext.Current
            m_Checked = (context.Request.Form(Me.UniqueID.ToString() & "_CHK") = "Y")
            Return False
        End Function

        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
        End Sub
    End Class

End Namespace
