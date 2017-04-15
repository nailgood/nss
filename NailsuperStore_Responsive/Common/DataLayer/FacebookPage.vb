
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
    Public Class FacebookPageRow
        Inherits FacebookPageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal FacebookPageId As Integer)
            MyBase.New(database, FacebookPageId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal FacebookPageId As Integer) As FacebookPageRow
            Dim row As FacebookPageRow
            row = New FacebookPageRow(_Database, FacebookPageId)
            row.Load()
            Return row
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As FacebookPageRow
            Dim result As New FacebookPageRow
            If (Not reader.IsDBNull(reader.GetOrdinal("PageId"))) Then
                result.PageId = Convert.ToInt32(reader("PageId"))
            Else
                result.PageId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Thumb"))) Then
                result.Thumb = reader("Thumb").ToString()
            Else
                result.Thumb = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Link"))) Then
                result.Link = reader("Thumb").ToString()
            Else
                result.Link = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                result.MetaDescription = reader("MetaDescription").ToString()
            Else
                result.MetaDescription = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                result.PageTitle = reader("PageTitle").ToString()
            Else
                result.PageTitle = ""
            End If

            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal FacebookPageId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_FacebookPage_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("PageId", SqlDbType.Int, 0, FacebookPageId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As FacebookPageRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_FacebookPage_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Thumb", SqlDbType.VarChar, 0, data.Thumb))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("Link", SqlDbType.VarChar, 0, data.Link))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function Update(ByVal _Database As Database, ByVal data As FacebookPageRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_FacebookPage_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("PageId", SqlDbType.Int, 0, data.PageId))
                cmd.Parameters.Add(_Database.InParam("Thumb", SqlDbType.VarChar, 0, data.Thumb))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("Link", SqlDbType.VarChar, 0, data.Link))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function


    End Class


    Public MustInherit Class FacebookPageRowBase
        Private m_DB As Database
        Private m_PageId As Integer = Nothing
        Private m_Link As String = Nothing
        Private m_Thumb As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing

        Public Property PageId() As Integer
            Get
                Return m_PageId
            End Get
            Set(ByVal Value As Integer)
                m_PageId = Value
            End Set
        End Property

        Public Property Thumb() As String
            Get
                Return m_Thumb
            End Get
            Set(ByVal Value As String)
                m_Thumb = Value
            End Set
        End Property

        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property Link() As String
            Get
                Return m_Link
            End Get
            Set(ByVal Value As String)
                m_Link = Value
            End Set
        End Property


        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
            End Set
        End Property

        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As Integer)
                m_TotalRow = Value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal Value As Integer)
                m_PageIndex = Value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal Value As Integer)
                m_PageSize = Value
            End Set
        End Property

        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal Value As String)
                m_OrderBy = Value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal Value As String)
                m_OrderDirection = Value
            End Set
        End Property
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
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

        Public Sub New(ByVal database As Database, ByVal PageId As Integer)
            m_DB = database
            m_PageId = PageId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try

                Dim SQL As String

                SQL = "SELECT * FROM FacebookPage WHERE PageId = " & DB.Number(PageId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                Else
                    PageId = 0
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try


        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("PageId"))) Then
                    m_PageId = Convert.ToInt32(reader("PageId"))
                Else
                    m_PageId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Thumb"))) Then
                    m_Thumb = reader("Thumb").ToString()
                Else
                    m_Thumb = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Link"))) Then
                    m_Link = reader("Link").ToString()
                Else
                    m_Link = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                    m_PageTitle = reader("PageTitle").ToString()
                Else
                    m_PageTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                    m_MetaDescription = reader("MetaDescription").ToString()
                Else
                    m_MetaDescription = ""
                End If

            End If
        End Sub

    End Class

    Public Class FacebookPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal FacebookPage As FacebookPageRow)
            Me.List.Add(FacebookPage)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As FacebookPageRow
            Get
                Return CType(Me.List.Item(Index), FacebookPageRow)
            End Get

            Set(ByVal Value As FacebookPageRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


