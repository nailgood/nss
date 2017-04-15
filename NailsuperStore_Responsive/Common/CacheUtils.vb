Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Caching
Imports System.Collections

Namespace Utility

    Public Class CacheUtils

        Public Shared Function GetCache(ByVal key As String) As Object
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            Return cache(key)
        End Function


        Public Shared Sub SetCache(ByVal key As String, ByVal val As Object)

            If key Is Nothing Or key = String.Empty Then
                Return
            End If
            If val Is Nothing Then
                Return
            End If
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            cache.Insert(key, val)
        End Sub

        Public Shared Sub SetCache(ByVal key As String, ByVal val As Object, ByVal depend As CacheDependency)
            If key Is Nothing Or key = String.Empty Then
                Return
            End If
            If val Is Nothing Then
                Return
            End If
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            cache.Insert(key, val, depend)
        End Sub

        ''' <summary>
        ''' timeCountDown: seconds
        ''' </summary>
        Public Shared Sub SetCache(ByVal key As String, ByVal val As Object, ByVal timeCountDown As Integer)
            If val Is Nothing Then
                Return
            End If
            If (timeCountDown < 1) Then
                SetCache(key, val)
            Else
                Dim cache As Cache = System.Web.HttpRuntime.Cache
                cache.Insert(key, val, Nothing, DateTime.Now.AddSeconds(timeCountDown), TimeSpan.Zero, CacheItemPriority.Default, Nothing)
            End If
        End Sub

        Public Shared Sub RemoveCache(ByVal key As String)
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            cache.Remove(key)
        End Sub

        Public Shared Sub Reset()
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            Dim ce As IDictionaryEnumerator = cache.GetEnumerator()
            While (ce.MoveNext())
                cache.Remove(CType(ce.Key, String))
            End While
        End Sub
       
        Public Shared Function RemoveCacheItemWithPrefix(ByVal prefix As String) As String
            Dim result As String = ""
            Dim cache As Cache = System.Web.HttpRuntime.Cache
            Dim ce As IDictionaryEnumerator = cache.GetEnumerator()
            While (ce.MoveNext())
                If ce.Key.ToString().IndexOf(prefix) <> -1 Then
                    cache.Remove(CType(ce.Key, String))
                    result = result & ce.Key.ToString() & ","
                End If
            End While
            Return result
        End Function
        Public Shared Function ClearCacheWithPrefix(ByVal ParamArray prefix() As String) As String
            Try
                Dim result As String = ""
                Dim cache As Cache = System.Web.HttpRuntime.Cache
                Dim ce As IDictionaryEnumerator = cache.GetEnumerator()
                While (ce.MoveNext())
                    If (KeyExists(ce.Key.ToString(), prefix)) Then
                        cache.Remove(CType(ce.Key, String))
                        result = result & ce.Key.ToString() & ","
                    End If
                End While
                Return result
            Catch ex As Exception

            End Try
            Return String.Empty
        End Function

        Private Shared Function KeyExists(ByVal key As String, ByVal ParamArray prefix() As String) As Boolean
            For Each item As String In prefix
                If key.IndexOf(item) <> -1 Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class

    Public Enum enmCache
        TopMenu
        SaleMenu
    End Enum
End Namespace