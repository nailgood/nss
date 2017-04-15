Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionItemCategoryRow
        Inherits NavisionItemCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionItemCategoryCollection
            Dim c As New NavisionItemCategoryCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionItemCategoryRow
                Dim SQL As String = "select * from _NAVISION_ITEM_CATEGORY"

                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionItemCategoryRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try


            Return c
        End Function
    End Class

    Public MustInherit Class NavisionItemCategoryRowBase
        Private m_DB As Database
        Private m_id As Integer = Nothing
        Private m_Category As String = Nothing
        Private m_Name As String = Nothing

        Public Property id() As Integer
            Get
                Return m_id
            End Get
            Set(ByVal Value As Integer)
                m_id = Value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return m_Category
            End Get
            Set(ByVal Value As String)
                m_Category = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal id As Integer)
            m_DB = DB
            m_id = id
        End Sub 'New

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_id = Convert.ToInt32(r.Item("id"))
            m_Category = Trim(Convert.ToString(r.Item("Category")))
            If IsDBNull(r.Item("Name")) Then
                m_Name = Nothing
            Else
                m_Name = Trim(Convert.ToString(r.Item("Name")))
            End If
        End Sub 'Load
    End Class

    Public Class NavisionItemCategoryCollection
        Inherits GenericCollection(Of NavisionItemCategoryRow)
    End Class

End Namespace


