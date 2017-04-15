Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer

    Public Class AdminSectionRow
        Inherits AdminSectionRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SectionId As Integer)
            MyBase.New(DB, SectionId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SectionId As Integer) As AdminSectionRow
            Dim row As AdminSectionRow

            row = New AdminSectionRow(DB, SectionId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SectionId As Integer)
            Dim row As AdminSectionRow

            row = New AdminSectionRow(DB, SectionId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetPermissionList(ByVal DB As Database, ByVal AdminId As Integer) As AdminSectionCollection
            Dim sSQL As String
            Dim collection As New AdminSectionCollection

            sSQL = " SELECT DISTINCT ads.SectionId, ads.Code, ads.Description from Admin a, AdminAdminGroup aag, AdminAccess aa, AdminSection ads" _
                 & " WHERE a.AdminId = aag.AdminId" _
                 & " AND aag.GroupId = aa.GroupId" _
                 & " AND aa.SectionId = ads.SectionId" _
                 & " AND a.AdminId = " & DB.Quote(AdminId.ToString)

            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(sSQL)
                If Not r Is Nothing Then
                    Dim row As AdminSectionRow
                    While r.Read
                        row = New AdminSectionRow(DB)
                        row.Load(r)
                        If Not row.Code Is Nothing Then
                            collection.Add(row)
                        Else
                            Exit While
                        End If
                    End While
                   
                End If
                Core.CloseReader(r)
                Return collection
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New AdminSectionCollection
        End Function

    End Class

    Public MustInherit Class AdminSectionRowBase
        Private m_DB As Database
        Private m_SectionId As Integer = Nothing
        Private m_Code As String = Nothing
        Private m_Description As String = Nothing


        Public Property SectionId() As Integer
            Get
                Return m_SectionId
            End Get
            Set(ByVal Value As Integer)
                m_SectionId = value
            End Set
        End Property

        Public Property Code() As String
            Get
                Return m_Code
            End Get
            Set(ByVal Value As String)
                m_Code = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = value
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

        Public Sub New(ByVal database As Database, ByVal SectionId As Integer)
            m_DB = database
            m_SectionId = SectionId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM AdminSection WHERE SectionId = " & DB.Quote(SectionId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "AdminSection.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_Code = Convert.ToString(r.Item("Code"))
                m_Description = Convert.ToString(r.Item("Description"))
            Catch ex As Exception
                Throw ex
            End Try
            
        End Sub 'Load

        Public Overridable Sub Insert()
            Dim SQL As String

            SQL = " INSERT INTO AdminSection (" _
             & " Code" _
             & ",Description" _
             & ") VALUES (" _
             & m_DB.Quote(Code) _
             & "," & m_DB.Quote(Description) _
             & ")"

            m_DB.ExecuteSQL(SQL)
        End Sub 'Insert

        Function AutoInsert() As Integer
            Dim SQL As String = "SELECT @@IDENTITY"

            Insert()
            Return m_DB.ExecuteScalar(SQL)
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE AdminSection SET " _
             & " Code = " & m_DB.Quote(Code) _
             & ",Description = " & m_DB.Quote(Description) _
             & " WHERE SectionId = " & m_DB.Quote(SectionId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM AdminSection WHERE SectionId = " & m_DB.Quote(SectionId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class AdminSectionCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AdminSection As AdminSectionRow)
            Me.List.Add(AdminSection)
        End Sub

        Public Function Contains(ByVal AdminSection As AdminSectionRow) As Boolean
            Return Me.List.Contains(AdminSection)
        End Function

        Public Function IndexOf(ByVal AdminSection As AdminSectionRow) As Integer
            Return Me.List.IndexOf(AdminSection)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal AdminSection As AdminSectionRow)
            Me.List.Insert(Index, AdminSection)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminSectionRow
            Get
                Return CType(Me.List.Item(Index), AdminSectionRow)
            End Get

            Set(ByVal Value As AdminSectionRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal AdminSection As AdminSectionRow)
            Me.List.Remove(AdminSection)
        End Sub
        Public ReadOnly Property Clone() As AdminSectionCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminSectionCollection
                For Each obj As AdminSectionRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace


