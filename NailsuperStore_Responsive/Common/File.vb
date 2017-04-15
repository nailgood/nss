Imports System.IO
Namespace Utility
    Public Class File
        Public Shared Function DeleteFile(ByVal path As String) As Boolean
            Try
                System.IO.File.Delete(path)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function
    End Class
End Namespace
