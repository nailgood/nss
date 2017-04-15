Imports System
Imports System.Collections
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.InteropServices
Imports DataLayer
Imports Utility

Namespace Components

	Public Class SitePrincipal
		Implements System.Security.Principal.IPrincipal

		Private m_DB As Database
		Private m_Member As MemberRow

		Public ReadOnly Property Member() As MemberRow
			Get
				Return m_Member
			End Get
		End Property

		Public Sub New(ByVal _Database As Database, ByVal MemberId As Integer)
			m_DB = _Database
            m_Member = MemberRow.GetRow(MemberId)
		End Sub

		Public Sub New(ByVal _dataBase As Database, ByVal Username As String)
			m_DB = _dataBase
			m_Member = MemberRow.GetRowByUsername(_dataBase, Username)
		End Sub

		Public Shared Function ValidateLogin( _
		  ByVal db As Database, _
		  ByVal Username As String, _
		  ByVal Password As String, _
		  ByVal bPersist As Boolean) _
		  As Integer

			Dim MemberId As Integer = MemberRow.ValidateMemberCredentials(db, Username, Password)
			If MemberId > 0 Then
				If bPersist Then CookieUtil.SetTripleDESEncryptedCookie("MemberId", MemberId.ToString, Today.AddDays(15))
				Return MemberId
			Else
				Return Nothing
			End If
		End Function

		' IPrincipal Interface Implementation
		' ---
		Public ReadOnly Property Identity() As Principal.IIdentity _
		  Implements Principal.IPrincipal.Identity
			Get
				Return Nothing
			End Get

		End Property

		Public Function IsInRole(ByVal role As String) As Boolean Implements System.Security.Principal.IPrincipal.IsInRole
			Return True
		End Function
	End Class

End Namespace
