
Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility

Namespace DataLayer
    Public Class KeywordRedirectRow
        Inherits KeywordRedirectRowBase

        Public Property Description As String
        Public Sub New()
            MyBase.New()
        End Sub 'New



        Public Shared Function GetLinkRedirect(ByVal KeywordName As String) As String
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select [dbo].[fc_Keyword_GetLinkRedirect]('" & KeywordName & "')"
                Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                Dim result As String = CStr(db.ExecuteScalar(cmd))
                Return result
            Catch ex As Exception
                Components.Email.SendError("ToError500", "KeywordRedirect >> GetLinkRedirect", "KeywordName=" & KeywordName & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return String.Empty
        End Function

        Public Shared Function Insert(ByVal objKeyword As KeywordRedirectRow) As Integer
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordRedirect_Insert")
                db.AddInParameter(cmd, "KeywordName", DbType.String, objKeyword.KeywordName)
                db.AddInParameter(cmd, "LinkRedirect", DbType.String, objKeyword.LinkRedirect)
                db.AddInParameter(cmd, "Description", DbType.String, objKeyword.Description)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Components.Email.SendError("ToError500", "KeywordRedirect >> Insert", "KeywordName=" & objKeyword.KeywordName & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return False
        End Function
        Public Shared Function Update(ByVal objkeyword As KeywordRedirectRow) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordRedirect_Update")
                db.AddInParameter(cmd, "Id", DbType.Int32, objkeyword.Id)
                db.AddInParameter(cmd, "KeywordName", DbType.String, objkeyword.KeywordName)
                db.AddInParameter(cmd, "LinkRedirect", DbType.String, objkeyword.LinkRedirect)
                db.AddInParameter(cmd, "Description", DbType.String, objkeyword.Description)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result = 1
            Catch ex As Exception
                Components.Email.SendError("ToError500", "KeywordRedirect >> Update", "id=" & objkeyword.Id & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return False
        End Function
        '
        Public Shared Function Delete(ByVal id As Integer) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordRedirect_Delete")
                db.AddInParameter(cmd, "Id", DbType.Int32, id)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "KeywordRedirect >> Delete", "id=" & id & "<br>Exception: " & ex.ToString() + "")

            End Try
            Return False
        End Function
        Public Shared Function GetRow(ByVal id As Integer) As KeywordRedirectRow
            Dim dr As SqlDataReader = Nothing
            Try
                Dim result As New KeywordRedirectRow
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select Id,coalesce(KeywordName,'') as KeywordName,coalesce(LinkRedirect,'') as LinkRedirect, Description from KeywordRedirect where Id=" & id)
                dr = db.ExecuteReader(cmd)
                If dr.Read Then
                    result.Id = dr("Id")
                    result.KeywordName = dr("KeywordName")
                    result.LinkRedirect = dr("LinkRedirect")
                    result.Description = dr("Description").ToString()
                End If
                Core.CloseReader(dr)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "KeywordRedirect >> GetRow", "id=" & id & "<br>Exception: " & ex.ToString() + "")

            End Try
            Return Nothing
        End Function

    End Class

    Public MustInherit Class KeywordRedirectRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_KeywordName As String = Nothing
        Private m_LinkRedirect As String = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property
        Public Property KeywordName() As String
            Get
                Return m_KeywordName
            End Get
            Set(ByVal Value As String)
                m_KeywordName = Value
            End Set
        End Property
        Public Property LinkRedirect() As String
            Get
                Return m_LinkRedirect
            End Get
            Set(ByVal Value As String)
                m_LinkRedirect = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

    End Class

    Public Class KeywordRedirectRowCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordRedirectRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordRedirectRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordRedirectRowBase)
            End Get

            Set(ByVal Value As KeywordRedirectRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


