
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
    Public Class KeywordIgnoreRow
        Inherits KeywordIgnoreRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New



        Public Shared Function GetAll() As KeywordIgnoreCollection
            Dim dr As SqlDataReader = Nothing
            Dim lstResult As New KeywordIgnoreCollection
            Dim key As String = cachePrefixKey & "GetAll"
            lstResult = CType(CacheUtils.GetCache(key), KeywordIgnoreCollection)
            If Not lstResult Is Nothing AndAlso lstResult.Count > 0 Then
                Return lstResult
            End If
            Try
                lstResult = New KeywordIgnoreCollection
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select Id,coalesce(KeywordName,'') as KeywordName from KeywordIgnore")
                dr = db.ExecuteReader(cmd)
                While (dr.Read())
                    Dim objKeyword As New KeywordIgnoreRow
                    objKeyword.Id = dr("Id")
                    objKeyword.KeywordName = dr("KeywordName")
                    lstResult.Add(objKeyword)
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, lstResult)
                Return lstResult
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "RedirectKeyword-GetAll", "<br>Exception: " & ex.ToString() + "")
            End Try
            Return Nothing
        End Function

    End Class

    Public MustInherit Class KeywordIgnoreRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_KeywordName As String = Nothing
        Public Shared cachePrefixKey As String = "KeywordIgnore_"
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


        Public Sub New()
        End Sub 'New

    End Class

    Public Class KeywordIgnoreCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordIgnoreRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordIgnoreRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordIgnoreRowBase)
            End Get

            Set(ByVal Value As KeywordIgnoreRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace



