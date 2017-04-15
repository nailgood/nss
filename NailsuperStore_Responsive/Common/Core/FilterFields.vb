Imports System.Collections.Specialized
Imports system.Web

Namespace Components

	Public Enum FilterFieldType As Integer
		All = 1
		OnlyPost = 2
		OnlyGet = 3
	End Enum

	Public Class FilterFields
		Inherits NameObjectCollectionBase

		Private Function FilterName(ByVal s As String) As String
			If s = String.Empty Then Return String.Empty

			Dim aArray() As String = s.Split("$"c)
			Return aArray(aArray.Length - 1)
		End Function

		Protected Sub AddUnique(ByVal name As String, ByVal value As String)
			If name.StartsWith("F_") Or name.Contains("$F_") Then
				If Not value = String.Empty Then BaseAdd(FilterName(name), value)
			End If
		End Sub

		Public Sub Add(ByVal name As String, ByVal value As String)
			If name.StartsWith("F_") Or name.Contains("$F_") Then
				If Not value = String.Empty Then BaseAdd(FilterName(name), value)
			End If
		End Sub

		Public Sub Add(ByVal name As String, ByVal values As NameValueCollection, ByVal keyName As String)

			If Not values(keyName) Is Nothing Then
				For Each val As String In values.GetValues(keyName)
					If name.StartsWith("F_") Or name.Contains("$F_") Then
						If Not val = String.Empty Then BaseAdd(FilterName(name), System.Web.HttpUtility.UrlEncode(val))
					End If
				Next
			End If
		End Sub

		Public Overloads Function ToString() As String
			Return ToString("")
		End Function

		Public Overloads Function ToString(ByVal removeList As String) As String
			Dim Params As String = String.Empty
			Dim list(1) As String

			If Not removeList = String.Empty Then
				list = removeList.Split(";"c)
			End If

			For i As Integer = 0 To Count - 1
				If Array.IndexOf(list, BaseGetKey(i)) < 0 Then
					If Not BaseGet(i) = String.Empty Then Params &= FilterName(BaseGetKey(i)) + "=" + Convert.ToString(BaseGet(i)) + "&"
				End If
			Next
			Params = Params.TrimEnd("&"c)
			Return Params
		End Function

		Public Overloads Function ToString(ByVal preserve As FilterFieldType, ByVal removeList As String, ByVal viewState As System.Web.UI.StateBag) As String
			Dim Request As HttpRequest = HttpContext.Current.Request
			Dim Server As HttpServerUtility = HttpContext.Current.Server
			Dim list(1) As String

			If Not removeList = String.Empty Then
				list = removeList.Split(";"c)
			End If

			If preserve = FilterFieldType.All Or preserve = FilterFieldType.OnlyPost Then
				For i As Integer = 0 To Request.Form.Count - 1
					If Array.IndexOf(list, Request.Form.AllKeys(i)) < 0 AndAlso Request.Form.AllKeys(i) <> "__EVENTTARGET" AndAlso Request.Form.AllKeys(i) <> "__EVENTARGUMENT" AndAlso Request.Form.AllKeys(i) <> "__VIEWSTATE" AndAlso BaseGet(Request.Form.AllKeys(i)) Is Nothing Then
						For Each val As String In Request.Form.GetValues(i)
							If Not val = String.Empty Then AddUnique(FilterName(Request.Form.AllKeys(i)), Server.UrlEncode(val))
						Next
					End If
				Next
			End If

			If preserve = FilterFieldType.All Or preserve = FilterFieldType.OnlyGet Then
				If Not viewState Is Nothing Then
					Dim length As Integer = viewState.Count
					Dim keys(length) As String
					Dim values(length) As System.Web.UI.StateItem

					viewState.Keys.CopyTo(keys, 0)
					viewState.Values.CopyTo(values, 0)

					For i As Integer = 0 To length - 1
						If Not values(i).Value Is Nothing Then
							If Not values(i).Value.ToString() = String.Empty Then AddUnique(FilterName(keys(i)), Server.UrlEncode(values(i).Value.ToString()))
						End If
					Next
				End If

				For i As Integer = 0 To Request.QueryString.Count - 1
					If Array.IndexOf(list, Request.QueryString.AllKeys(i)) < 0 AndAlso BaseGet(Request.QueryString.AllKeys(i)) Is Nothing Then
						For Each val As String In Request.QueryString.GetValues(i)
							If Not val = String.Empty Then AddUnique(FilterName(Request.QueryString.AllKeys(i)), Server.UrlEncode(val))
						Next
					End If
				Next
			End If

			Return ToString("")
		End Function


	End Class

End Namespace
