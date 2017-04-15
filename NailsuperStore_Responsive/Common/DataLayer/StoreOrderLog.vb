Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility

Namespace DataLayer

    Public Class StoreOrderLogRow
        Public Sub New()
            MyBase.New()
        End Sub 'New


        Public Shared Function Add(ByVal OrderId As Integer, ByVal PageName As String, ByVal Action As String, ByVal Description As String) As Boolean
            'Dim strIP As String = "74.222.39.162;74.222.39.163;74.222.39.164;74.222.39.165;74.222.39.166;75.144.111.78;198.0.241.41;198.0.241.42;198.0.241.43;198.0.241.44;198.0.241.45;113.161.73.102;"
            'If strIP.Contains(Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")) Then
            '    Return True
            'End If

            Dim result As Boolean = False
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrderLog_Insert")
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                db.AddInParameter(cmd, "PageName", DbType.String, PageName)
                db.AddInParameter(cmd, "Description", DbType.String, Description)
                db.AddInParameter(cmd, "Action", DbType.String, Action)
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, DateTime.Now)
                If db.ExecuteNonQuery(cmd) > 0 Then
                    result = True
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "StoreOrderLog >> Add", "OrderId=" & OrderId & "<br>PageName=" & PageName & "<br>Description=" & Description & "<br>Action=" & Action & "<br>CreatedDate=" & DateTime.Now.ToString() & "<br>Exception=" & ex.ToString())
            End Try

            Return result
        End Function

        'Author: Cuong'
        Public Shared Function GetLatestStep(ByVal OrderId As Integer) As String
            Dim reader As SqlDataReader = Nothing
            Dim Result As String = String.Empty
            Try
                Dim SP As String = "sp_StoreOrderLog_GetLatestStep"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                db.AddOutParameter(cmd, "@result", DbType.AnsiString, -1)
                reader = db.ExecuteReader(cmd)
                reader.Read()

                Result = cmd.Parameters("@result").Value
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "StoreOrderLog > GetLatestStep", "OrderId: " & OrderId & "<br>Exception: " & ex.ToString() & "")
            End Try
            Return Result
        End Function
    End Class

End Namespace