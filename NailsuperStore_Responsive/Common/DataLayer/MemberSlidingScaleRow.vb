Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class MemberSlidingScaleRow
        Inherits MemberSlidingScaleRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ScaleId As Integer)
            MyBase.New(DB, ScaleId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ScaleId As Integer) As MemberSlidingScaleRow
            Dim row As MemberSlidingScaleRow

            row = New MemberSlidingScaleRow(DB, ScaleId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ScaleId As Integer)
            Dim row As MemberSlidingScaleRow

            row = New MemberSlidingScaleRow(DB, ScaleId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class MemberSlidingScaleRowBase
        Private m_DB As Database
        Private m_ScaleId As Integer = Nothing
        Private m_PercentageDiscount As Double = Nothing
        Private m_RangeStart As Double = Nothing
        Private m_RangeEnd As Double = Nothing
        Private m_IsFreeShipping As Boolean = Nothing


        Public Property ScaleId() As Integer
            Get
                Return m_ScaleId
            End Get
            Set(ByVal Value As Integer)
                m_ScaleId = Value
            End Set
        End Property

        Public Property PercentageDiscount() As Double
            Get
                Return m_PercentageDiscount
            End Get
            Set(ByVal Value As Double)
                m_PercentageDiscount = Value
            End Set
        End Property

        Public Property RangeStart() As Double
            Get
                Return m_RangeStart
            End Get
            Set(ByVal Value As Double)
                m_RangeStart = Value
            End Set
        End Property

        Public Property RangeEnd() As Double
            Get
                Return m_RangeEnd
            End Get
            Set(ByVal Value As Double)
                m_RangeEnd = Value
            End Set
        End Property

        Public Property IsFreeShipping() As Boolean
            Get
                Return m_IsFreeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeShipping = Value
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

        Public Sub New(ByVal DB As Database, ByVal ScaleId As Integer)
            m_DB = DB
            m_ScaleId = ScaleId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM MemberSlidingScale WHERE ScaleId = " & DB.Number(ScaleId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_ScaleId = Convert.ToInt32(r.Item("ScaleId"))
            m_PercentageDiscount = Convert.ToDouble(r.Item("PercentageDiscount"))
            If IsDBNull(r.Item("RangeStart")) Then
                m_RangeStart = Nothing
            Else
                m_RangeStart = Convert.ToDouble(r.Item("RangeStart"))
            End If
            If IsDBNull(r.Item("RangeEnd")) Then
                m_RangeEnd = Nothing
            Else
                m_RangeEnd = Convert.ToDouble(r.Item("RangeEnd"))
            End If
            m_IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO MemberSlidingScale (" _
             & " PercentageDiscount" _
             & ",RangeStart" _
             & ",RangeEnd" _
             & ",IsFreeShipping" _
             & ") VALUES (" _
             & m_DB.Number(PercentageDiscount) _
             & "," & m_DB.Number(RangeStart) _
             & "," & m_DB.NullNumber(RangeEnd) _
             & "," & CInt(IsFreeShipping) _
             & ")"

            ScaleId = m_DB.InsertSQL(SQL)

            Return ScaleId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE MemberSlidingScale SET " _
             & " PercentageDiscount = " & m_DB.Number(PercentageDiscount) _
             & ",RangeStart = " & m_DB.Number(RangeStart) _
             & ",RangeEnd = " & m_DB.NullNumber(RangeEnd) _
             & ",IsFreeShipping = " & CInt(IsFreeShipping) _
             & " WHERE ScaleId = " & m_DB.Quote(ScaleId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM MemberSlidingScale WHERE ScaleId = " & m_DB.Quote(ScaleId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class MemberSlidingScaleCollection
        Inherits GenericCollection(Of MemberSlidingScaleRow)
    End Class

End Namespace

