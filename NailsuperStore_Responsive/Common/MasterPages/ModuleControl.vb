Imports System.Web.UI
Imports System.web.Caching
Imports MasterPages

Namespace Components

	Public MustInherit Class ModuleControl
		Inherits BaseControl
		Implements IModule

		Private m_IsDesignMode As Boolean
		Private m_HTMLContent As String

		Public Sub New()
		End Sub

		Public Overridable ReadOnly Property EditMode() As Boolean Implements IModule.EditMode
			Get
				Return True
			End Get
		End Property

		Public Overridable Property IsDesignMode() As Boolean Implements IModule.IsDesignMode
			Get
				Return m_IsDesignMode
			End Get
			Set(ByVal Value As Boolean)
				m_IsDesignMode = Value
			End Set
		End Property

		Public MustOverride Property Args() As String Implements IModule.Args

		Public Property HTMLContent() As String Implements MasterPages.IModule.HTMLContent
            Get
                If m_HTMLContent = "<!--345-->" Or m_HTMLContent = "&nbsp;" Then
                    m_HTMLContent = ""
                Else
                    m_HTMLContent = "<div id=""dContent"">" & m_HTMLContent & "</div>"
                End If
                Return m_HTMLContent
            End Get
			Set(ByVal Value As String)
				m_HTMLContent = Value
			End Set
		End Property

	End Class
End Namespace