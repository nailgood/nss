Imports System.Reflection

Public Class CloneObject
    Public Shared Function Clone(ByVal source As Object) As Object
        If source Is Nothing Then
            Return Nothing
        End If
        Dim destObject = Activator.CreateInstance(source.GetType())
        For Each fInfo As FieldInfo In source.GetType().GetFields
            fInfo.SetValue(destObject, fInfo.GetValue(source), BindingFlags.Default, Nothing, Nothing)
        Next
        For Each pInfo As PropertyInfo In source.GetType().GetProperties
            If pInfo.CanWrite Then
                pInfo.SetValue(destObject, pInfo.GetValue(source, Nothing), BindingFlags.Default, Nothing, Nothing, Nothing)
            End If

           
        Next
        'Dim sourceMess As String = Utility.Common.ObjectToString(source)
        'Dim descMess As String = Utility.Common.ObjectToString(destObject)
        'If (sourceMess <> descMess) Then
        '    ''return error here
        '    Throw New Exception("Clone object erorr")
        ''''

        'End If
        Return destObject
    End Function
    Public Shared Function Clone(ByVal source As DataView) As DataView
        If source Is Nothing Then
            Return Nothing
        End If
        Return New DataView(source.Table)
    End Function


End Class
