Option Explicit On

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
Imports Database

Namespace DataLayer
    Public Class DepartmentOldRow
        Inherits DepartmentOldRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            MyBase.New(database, DepartmentId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DepartmentId As Integer) As DepartmentOldRow
            Dim row As DepartmentOldRow

            row = New DepartmentOldRow(_Database, DepartmentId)
            row.Load()

            Return row

        End Function
        Public Shared Function GetByURLCode(ByVal _Database As Database, ByVal DepartmentURLCode As String) As DepartmentOldRow
            Dim row As New DepartmentOldRow
            Dim reader As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT DepartmentOldId,DepartmentNewId FROM DepartmentOld DO INNER JOIN StoreDepartment SD ON SD.DepartmentId = DO.DepartmentNewId WHERE DO.Type=1 AND DO.UrlCodeOld = " & _Database.Quote(DepartmentURLCode)
                reader = _Database.GetReader(SQL)
                Try

                    '' row = mapList(Of DepartmentOldRow)(r).Item(0)
                    If (reader.Read()) Then
                        If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentOldId"))) Then
                            row.DepartmentOldId = Convert.ToInt32(reader("DepartmentOldId"))
                        Else
                            row.DepartmentOldId = 0
                        End If

                        If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentNewId"))) Then
                            row.DepartmentNewId = Convert.ToInt32(reader("DepartmentNewId"))
                        Else
                            row.DepartmentNewId = 0
                        End If


                    End If

                Catch ex As Exception
                    Dim rawURL As String = String.Empty
                    If Not System.Web.HttpContext.Current Is Nothing Then
                        If Not System.Web.HttpContext.Current.Request Is Nothing Then
                            rawURL = System.Web.HttpContext.Current.Request.RawUrl
                        End If
                    End If
                    Components.Email.SendError("ToError500", "GetByURLCode", "DepartmentURLCode-" & DepartmentURLCode & "<br/>Page:" & rawURL & "</br>" & ex.Message & ",Stack trace:" & ex.StackTrace)

                End Try

                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                row = New DepartmentOldRow
                row.DepartmentNewId = Nothing
                Dim rawURL As String = String.Empty
                If Not System.Web.HttpContext.Current Is Nothing Then
                    If Not System.Web.HttpContext.Current.Request Is Nothing Then
                        rawURL = System.Web.HttpContext.Current.Request.RawUrl
                    End If
                End If

                Components.Email.SendError("ToError500", "Error 500", "DepartmentURLCode-" & DepartmentURLCode & "<br/>Page:" & rawURL & "</br>" & ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return row
        End Function

        Public Shared Function ReplaceUrl(ByVal str As String) As String
            If str <> "" And str <> Nothing Then
                str = RTrim(LTrim(str))
                str = str.Replace("/", "-").Replace("%3e", "greater").Replace("<", "").Replace(">", "greater").Replace(" / ", "-").Replace(" & ", "_and_").Replace("+", "-").Replace("_-_", "-").Replace(" ", "-").Replace("%23", "").Replace("%2b", "").Replace("%2f", "").Replace("&", "-").Replace(",", "-").Replace(".", "-").Replace("%3f", "").Replace(".", "").Replace("?", "").Replace("%26", "and").Replace("%22", "").Replace("%3a", "").Replace("%2c", "").Replace("%24", "$").Replace("%c3%a9", "e").Replace("%25", "percent").Replace("%20", "-").Replace("%22", "").Replace("%c2%a2", "¢").Replace("%c3%a8", "e").Replace("'", "").Replace("#", "").Replace("%e2%80%99", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "-").Replace("è", "e").Replace("é", "e").Replace("¢", "").Replace("e2809c", "").Replace("e2809d", "").Replace(" - ", "-").Replace("--", "-")
                str = str.Replace("--", "-").Replace("_", "-")
            End If
            Return str
        End Function

    End Class

    Public MustInherit Class DepartmentOldRowBase
        Private m_DB As Database
        Private m_DepartmentOldId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_DepartmentNewId As Integer = Nothing

        Public Property DepartmentOldId() As Integer
            Get
                Return m_DepartmentOldId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentOldId = Value
            End Set
        End Property

        Public Property DepartmentNewId() As Integer
            Get
                Return m_DepartmentNewId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentNewId = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DepartmentOldId As Integer)
            m_DB = database
            m_DepartmentOldId = DepartmentOldId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT DO.*, SD.Name FROM DepartmentOld DO INNER JOIN StoreDepartment SD ON SD.DepartmentId = DO.DepartmentNewId WHERE DO.DepartmentOldId = " & DB.Quote(DepartmentOldId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentOldId"))) Then
                    m_DepartmentOldId = Convert.ToInt32(reader("DepartmentOldId"))
                Else
                    m_DepartmentOldId = 0
                End If
          
                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentNewId"))) Then
                    m_DepartmentNewId = Convert.ToInt32(reader("DepartmentNewId"))
                Else
                    m_DepartmentNewId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
             
            End If
        End Sub

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE DepartmentOld SET " _
             & ",DepartmentNewId = " & m_DB.NullNumber(DepartmentNewId) _
             & ",Name = " & m_DB.Quote(Name) _
             & " WHERE DepartmentOldId = " & m_DB.Quote(DepartmentOldId)

            m_DB.ExecuteSQL(SQL)
        End Sub
    End Class

    Public Class DepartmentOldCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal DepartmentOld As DepartmentOldRow)
            Me.List.Add(DepartmentOld)
        End Sub

        Public Function Contains(ByVal DepartmentOld As DepartmentOldRow) As Boolean
            Return Me.List.Contains(DepartmentOld)
        End Function

        Public Function IndexOf(ByVal DepartmentOld As DepartmentOldRow) As Integer
            Return Me.List.IndexOf(DepartmentOld)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal DepartmentOld As DepartmentOldRow)
            Me.List.Insert(Index, DepartmentOld)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DepartmentOldRow
            Get
                Return CType(Me.List.Item(Index), DepartmentOldRow)
            End Get

            Set(ByVal Value As DepartmentOldRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal DepartmentOld As DepartmentOldRow)
            Me.List.Remove(DepartmentOld)
        End Sub

    End Class
End Namespace