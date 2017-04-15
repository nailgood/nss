
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

Namespace DataLayer
    Public Class StorePromotionItemRow
        Inherits StorePromotionItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            MyBase.New(database, Id)
        End Sub 'New
        Public Shared Function GetListByPromotion(ByVal DB As Database, ByVal PromotionId As Integer) As StorePromotionItemCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StorePromotionItemCollection
            Try
                Dim row As StorePromotionItemRow
                Dim sql As String = "Select Id,PromotionId,ItemId from StorePromotionItem where PromotionId=" & PromotionId
                dr = DB.GetReader(sql)
                While dr.Read
                    row = New StorePromotionItemRow()
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "GetListByPromotion", "PromotionId: " & PromotionId & "<br>Exception: " & ex.ToString())
            End Try
            Return c
        End Function
        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal Id As Integer) As StorePromotionItemRow
            Dim row As StorePromotionItemRow
            row = New StorePromotionItemRow(_Database, Id)
            row.Load()
            Return row
        End Function
   
      
    End Class


    Public MustInherit Class StorePromotionItemRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_PromotionId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
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
        Public Property PromotionId() As Integer
            Get
                Return m_PromotionId
            End Get
            Set(ByVal Value As Integer)
                m_PromotionId = Value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "Select * from StorePromotionItem where Id=" & DB.Number(Id)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PromotionId"))) Then
                    m_PromotionId = Convert.ToInt32(reader("PromotionId"))
                Else
                    m_PromotionId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    m_ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    m_ItemId = 0
                End If


            End If
        End Sub
        Public Shared Function InsertListPromotionItem(ByVal DB As Database, ByVal lstSKU As List(Of String), ByVal promotionId As Integer) As Boolean
            Try
                Dim sql As String = String.Empty
                For Each sku As String In lstSKU
                    sql = sql & "Insert into StorePromotionItem(PromotionId,ItemId) values(" & promotionId & ",(Select ItemId from StoreItem where SKU='" & sku & "')); "
                Next
                If Not String.IsNullOrEmpty(sql) Then
                    DB.ExecuteSQL(sql)
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function


    End Class

    Public Class StorePromotionItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal DealDay As StorePromotionItemRow)
            Me.List.Add(DealDay)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StorePromotionItemRow
            Get
                Return CType(Me.List.Item(Index), StorePromotionItemRow)
            End Get

            Set(ByVal Value As StorePromotionItemRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace
