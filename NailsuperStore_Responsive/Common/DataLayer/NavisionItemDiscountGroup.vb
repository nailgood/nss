Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionItemDiscountGroupRow
        Inherits NavisionItemDiscountGroupRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionItemDiscountGroupCollection
            Dim r As SqlDataReader = Nothing
            Dim row As NavisionItemDiscountGroupRow
            Dim c As New NavisionItemDiscountGroupCollection
            Try
                Dim SQL As String = "select * from _NAVISION_ITEM_DISCOUNT_GROUP"

                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionItemDiscountGroupRow(DB)
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

    Public MustInherit Class NavisionItemDiscountGroupRowBase
        Private m_DB As Database
        Private m_Code As String = Nothing
        Private m_Description As String = Nothing
        Private m_Id As Integer = Nothing

        Public Property Code() As String
            Get
                Return m_Code
            End Get
            Set(ByVal Value As String)
                m_Code = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_Code = Convert.ToString(r.Item("Code"))
            If IsDBNull(r.Item("Description")) Then
                m_Description = Nothing
            Else
                m_Description = Convert.ToString(r.Item("Description"))
            End If
            m_Id = Convert.ToInt32(r.Item("Id"))
        End Sub 'Load
    End Class

    Public Class NavisionItemDiscountGroupCollection
        Inherits GenericCollection(Of NavisionItemDiscountGroupRow)
    End Class

End Namespace


