Imports Utility
Imports System.Web

Namespace Utility

    Public Class CookieUtil

        'SetTripleDESEncryptedCookie - key & value only
        Public Shared Sub SetTripleDESEncryptedCookie(ByVal key As String, ByVal value As String)
            'Convert parts
            'key = Crypt.EncryptTripleDES(key)
            value = CryptData.Crypt.EncryptTripleDes(value)

            SetCookie(key, value)
        End Sub

        'SetTripleDESEncryptedCookie - overloaded method with expires parameter
        Public Shared Sub SetTripleDESEncryptedCookie(ByVal key As String, ByVal value As String, ByVal expires As Date)
            'Convert parts
            'key = Crypt.EncryptTripleDES(key)
            value = CryptData.Crypt.EncryptTripleDes(value)

            SetCookie(key, value, expires)
        End Sub

        'SetCookie - key & value only
        Public Shared Sub SetCookie(ByVal key As String, ByVal value As String)
            'Encode Part
            'key = HttpContext.Current.Server.UrlEncode(key)
            value = HttpContext.Current.Server.UrlEncode(value)

            Dim cookie As HttpCookie
            cookie = New HttpCookie(key, value)
            SetCookie(cookie)
        End Sub

        'SetCookie - overloaded with expires parameter
        Public Shared Sub SetCookie(ByVal key As String, ByVal value As String, ByVal expires As Date)
            'Encode Parts
            'key = HttpContext.Current.Server.UrlEncode(key)
            value = HttpContext.Current.Server.UrlEncode(value)

            Dim cookie As HttpCookie
            cookie = New HttpCookie(key, value)
            cookie.Expires = expires
            SetCookie(cookie)
        End Sub

        'SetCookie - HttpCookie only
        'final step to set the cookie to the response clause
        Public Shared Sub SetCookie(ByVal cookie As HttpCookie)
            HttpContext.Current.Response.Cookies.Set(cookie)
        End Sub

        Public Shared Function GetTripleDESEncryptedCookieValue(ByVal key As String) As String
            'encrypt key only - encoding done in GetCookieValue
            'key = Crypt.EncryptTripleDES(key)

            'get value 
            Dim value As String
            value = GetCookieValue(key)
            'decrypt value
            value = CryptData.Crypt.DecryptTripleDes(value)
            Return value
        End Function

        Public Shared Function GetCookie(ByVal key As String) As HttpCookie
            'encode key for retrieval
            'key = HttpContext.Current.Server.UrlEncode(key)
            Return HttpContext.Current.Request.Cookies.Get(key)
        End Function

        Public Shared Function GetCookieValue(ByVal key As String) As String
            Try
                'get value 
                Dim value As String = GetCookie(key).Value
                'decode stored value
                value = HttpContext.Current.Server.UrlDecode(value)
                Return value
            Catch
                Return String.Empty
            End Try
        End Function

    End Class

End Namespace
