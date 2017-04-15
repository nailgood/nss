Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Web.Mail
Imports System.Text.RegularExpressions
Imports Utility.Common
Imports System.Configuration

Namespace Components

    Public Class Param
#Region "Input param store"
        ''input param to Store
        Public Shared Function ObjectToDB(ByVal input As DateTime) As Object
            Try
                If input = "#12:00:00 AM#" Then
                    Return DBNull.Value
                End If
                If input = DateTime.MinValue Then
                    Return DBNull.Value
                End If
                '' Return DateTime.Parse(input)
                Return input
            Catch ex As Exception
                Return DBNull.Value
            End Try
        End Function
        Public Shared Function ObjectToDB(ByVal input As String) As Object
            Try
                If String.IsNullOrEmpty(input) Then
                    Return DBNull.Value
                End If
                Return input
            Catch ex As Exception
                Return DBNull.Value
            End Try
        End Function
#End Region
    End Class
End Namespace

