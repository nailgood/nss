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
    Public Class Ebay_ItemSellRow
        Inherits Ebay_ItemSellBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ID As Integer)
            MyBase.New(database, ID)
        End Sub 'New    


        Public Shared Function UpdateEbayQty(ByVal _Database As Database, ByVal NailItemId As Integer, ByVal qtyUpdate As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Ebay_ItemSell_UpdateQtyRevise"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NailItemId", SqlDbType.Int, 0, NailItemId))
                cmd.Parameters.Add(_Database.InParam("QtyImport", SqlDbType.Int, 0, qtyUpdate))
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


    Public MustInherit Class Ebay_ItemSellBase
        Private m_DB As Database
        Private m_EbayItemSellId As Integer = Nothing
        Private m_EbayId As String = Nothing
        Private m_SKU As String = Nothing
        Private m_PictureUrl As String = Nothing
        Private m_Quantity As Integer = Nothing
        Private m_FixedPrice As Decimal = Nothing
        Private m_EbayStatus As String = Nothing
        Private m_NailIsEbay As Boolean = Nothing
        Private m_QuantityRevise As Integer = Nothing


        Public Property EbayItemSellId() As Integer
            Get
                Return m_EbayItemSellId
            End Get
            Set(ByVal Value As Integer)
                m_EbayItemSellId = Value
            End Set
        End Property
        Public Property EbayId() As String
            Get
                Return m_EbayId
            End Get
            Set(ByVal Value As String)
                m_EbayId = Value
            End Set
        End Property

        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal Value As String)
                m_SKU = Value
            End Set
        End Property
        Public Property PictureUrl() As String
            Get
                Return m_PictureUrl
            End Get
            Set(ByVal Value As String)
                m_PictureUrl = Value
            End Set
        End Property
        Public Property Quantity() As Integer
            Get
                Return m_Quantity
            End Get
            Set(ByVal Value As Integer)
                m_Quantity = Value
            End Set
        End Property
        Public Property FixedPrice() As Decimal
            Get
                Return m_FixedPrice
            End Get
            Set(ByVal Value As Decimal)
                m_FixedPrice = Value
            End Set
        End Property
        Public Property EbayStatus() As String
            Get
                Return m_EbayStatus
            End Get
            Set(ByVal Value As String)
                m_EbayStatus = Value
            End Set
        End Property
        Public Property QuantityRevise() As String
            Get
                Return m_QuantityRevise
            End Get
            Set(ByVal Value As String)
                m_QuantityRevise = Value
            End Set
        End Property
        Public Property NailIsEbay() As Boolean
            Get
                Return m_NailIsEbay
            End Get
            Set(ByVal Value As Boolean)
                m_NailIsEbay = Value
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

        Public Sub New(ByVal database As Database, ByVal EbayItemSellId As Integer)
            m_DB = database
            m_EbayItemSellId = EbayItemSellId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Ebay_ItemSell WHERE EbayItemSellId = " & DB.Number(m_EbayItemSellId)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("EbayItemSellId"))) Then
                    m_EbayItemSellId = Convert.ToInt32(reader("EbayItemSellId"))
                Else
                    m_EbayItemSellId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("EbayId"))) Then
                    m_EbayId = reader("EbayId").ToString()
                Else
                    m_EbayId = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SKU"))) Then
                    m_SKU = reader("SKU").ToString()
                Else
                    m_SKU = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PictureUrl"))) Then
                    m_PictureUrl = reader("PictureUrl").ToString()
                Else
                    m_PictureUrl = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Quantity"))) Then
                    m_Quantity = Convert.ToInt32(reader("Quantity"))
                Else
                    m_Quantity = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("FixedPrice"))) Then
                    m_FixedPrice = reader("FixedPrice").ToString()
                Else
                    m_FixedPrice = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EbayStatus"))) Then
                    m_EbayStatus = reader("EbayStatus").ToString()
                Else
                    m_EbayStatus = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("QuantityRevise"))) Then
                    m_QuantityRevise = Convert.ToInt32(reader("QuantityRevise"))
                Else
                    m_QuantityRevise = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NailIsEbay"))) Then
                    m_NailIsEbay = Convert.ToBoolean(reader("NailIsEbay"))
                Else
                    m_NailIsEbay = True
                End If

            End If
        End Sub

    End Class

    Public Class Ebay_ItemSellCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal objData As Ebay_ItemSellRow)
            Me.List.Add(objData)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As Ebay_ItemSellRow
            Get
                Return CType(Me.List.Item(Index), Ebay_ItemSellRow)
            End Get

            Set(ByVal Value As Ebay_ItemSellRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace




