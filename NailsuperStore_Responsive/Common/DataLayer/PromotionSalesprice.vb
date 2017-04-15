Option Explicit On

'Author: Lam Le
'Date: 9/30/2009 10:10:05 AM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Imports System.Web
Imports Components

Namespace DataLayer

    Public Class PromotionSalespriceRow
        Inherits PromotionSalespriceRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As PromotionSalespriceRow
            Dim row As PromotionSalespriceRow

            row = New PromotionSalespriceRow(DB, Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
            Dim row As PromotionSalespriceRow

            row = New PromotionSalespriceRow(DB, Id)
            row.Remove()
        End Sub
        'Long add 23/10/2009
        Public Shared Function GetListPromotionSalesprices(ByVal Id As String, ByVal Type As String) As DataTable

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "[sp_PromotionSalesprices_DepartmentId]"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "Type", DbType.String, Type)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function
        Public Shared Function GetListPromotionSalespricesByDepartmentCode(ByVal code As String, ByVal Type As String) As DataTable
            If code = "" Then
                Return New DataTable
            End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_PromotionSalesprices_DepartmentCode"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "URLCode", DbType.String, code)
            db.AddInParameter(cmd, "Type", DbType.String, Type)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function
        'end 23/10/2009
        'Custom Methods




    End Class

    Public MustInherit Class PromotionSalespriceRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_SubTitle As String = Nothing
        Private m_MainTitle As String = Nothing
        Private m_LinkPage As String = Nothing
        Private m_Image As String = Nothing
        Private m_MobileImage As String = Nothing
        Private m_TextHtml As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_StartingDate As DateTime
        Private m_EndingDate As DateTime
        Private m_DepartmentID As Integer = Nothing
        Private m_Type As String = Nothing
        Public Shared cachePromotion As String = "PromotionSaleprice"
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property SubTitle() As String
            Get
                Return m_SubTitle
            End Get
            Set(ByVal Value As String)
                m_SubTitle = Value
            End Set
        End Property

        Public Property MainTitle() As String
            Get
                Return m_MainTitle
            End Get
            Set(ByVal Value As String)
                m_MainTitle = Value
            End Set
        End Property

        Public Property LinkPage() As String
            Get
                Return m_LinkPage
            End Get
            Set(ByVal Value As String)
                m_LinkPage = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property
        Public Property MobileImage() As String
            Get
                Return m_MobileImage
            End Get
            Set(ByVal Value As String)
                m_MobileImage = Value
            End Set
        End Property
        Public Property TextHtml() As String
            Get
                Return m_TextHtml
            End Get
            Set(ByVal Value As String)
                m_TextHtml = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property
        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal value As DateTime)
                m_StartingDate = value
            End Set
        End Property

        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal value As DateTime)
                m_EndingDate = value
            End Set
        End Property

        Public Property DepartmentID() As Integer
            Get
                Return m_DepartmentID
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentID = Value
            End Set
        End Property
        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal Value As String)
                m_Type = Value
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

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_PromotionSalesprice_GETOBJECT As String = "sp_PromotionSalesprices_Detail"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_PromotionSalesprice_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SubTitle"))) Then
                    m_SubTitle = reader("SubTitle").ToString()
                Else
                    m_SubTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MainTitle"))) Then
                    m_MainTitle = reader("MainTitle").ToString()
                Else
                    m_MainTitle = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LinkPage"))) Then
                    m_LinkPage = reader("LinkPage").ToString()
                Else
                    m_LinkPage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                    m_Image = reader("Image").ToString()
                Else
                    m_Image = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MobileImage"))) Then
                    m_MobileImage = reader("MobileImage").ToString()
                Else
                    m_MobileImage = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TextHtml"))) Then
                    m_TextHtml = reader("TextHtml").ToString()
                Else
                    m_TextHtml = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                    m_StartingDate = Convert.ToDateTime(reader("StartingDate"))
                Else
                    m_StartingDate = Nothing
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                    m_EndingDate = Convert.ToDateTime(reader("EndingDate"))
                Else
                    m_EndingDate = Nothing
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentID"))) Then
                    m_DepartmentID = Convert.ToInt32(reader("DepartmentID"))
                Else
                    m_DepartmentID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                    m_Type = reader("Type").ToString()
                Else
                    m_Type = ""
                End If
            End If
        End Sub 'Load

        Public Overridable Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_PromotionSalesprice_INSERT As String = "sp_PromotionSalesprice_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_PromotionSalesprice_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "SubTitle", DbType.String, SubTitle)
            db.AddInParameter(cmd, "MainTitle", DbType.String, MainTitle)
            db.AddInParameter(cmd, "LinkPage", DbType.String, LinkPage)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "MobileImage", DbType.String, MobileImage)
            db.AddInParameter(cmd, "TextHtml", DbType.String, TextHtml)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, DepartmentID)
            db.AddInParameter(cmd, "Type", DbType.String, Type)
            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            CacheUtils.RemoveCache(cachePromotion)
            '------------------------------------------------------------------------
            Return Id
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_PromotionSalesprice_UPDATE As String = "sp_PromotionSalesprice_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_PromotionSalesprice_UPDATE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "SubTitle", DbType.String, SubTitle)
            db.AddInParameter(cmd, "MainTitle", DbType.String, MainTitle)
            db.AddInParameter(cmd, "LinkPage", DbType.String, LinkPage)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "MobileImage", DbType.String, MobileImage)
            db.AddInParameter(cmd, "TextHtml", DbType.String, TextHtml)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "DepartmentID", DbType.Int32, DepartmentID)
            db.AddInParameter(cmd, "Type", DbType.String, Type)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
            CacheUtils.RemoveCache(cachePromotion)
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_PromotionSalesprice_DELETE As String = "sp_PromotionSalesprice_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_PromotionSalesprice_DELETE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)

            db.ExecuteNonQuery(cmd)

            CacheUtils.RemoveCache(cachePromotion)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class PromotionSalespriceCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal PromotionSalesprice As PromotionSalespriceRow)
            Me.List.Add(PromotionSalesprice)
        End Sub

        Public Function Contains(ByVal PromotionSalesprice As PromotionSalespriceRow) As Boolean
            Return Me.List.Contains(PromotionSalesprice)
        End Function

        Public Function IndexOf(ByVal PromotionSalesprice As PromotionSalespriceRow) As Integer
            Return Me.List.IndexOf(PromotionSalesprice)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal PromotionSalesprice As PromotionSalespriceRow)
            Me.List.Insert(Index, PromotionSalesprice)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As PromotionSalespriceRow
            Get
                Return CType(Me.List.Item(Index), PromotionSalespriceRow)
            End Get

            Set(ByVal Value As PromotionSalespriceRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal PromotionSalesprice As PromotionSalespriceRow)
            Me.List.Remove(PromotionSalesprice)
        End Sub
    End Class

End Namespace
