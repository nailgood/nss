Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:13:04 PM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer

    Public Class StoreItemImageRow
        Inherits StoreItemImageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

		Public Sub New(ByVal DB As Database, ByVal Id As Integer)
			MyBase.New(DB, Id)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ItemId As Integer, ByVal Image As String)
			MyBase.New(DB, ItemId, Image)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As StoreItemImageRow
			Dim row As StoreItemImageRow

			row = New StoreItemImageRow(DB, Id)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal ItemId As Integer, ByVal Image As String) As StoreItemImageRow
			Dim row As StoreItemImageRow

			row = New StoreItemImageRow(DB, ItemId, Image)
			row.Load()

			Return row
		End Function

        Public Shared Function GetRowByItemIdAndImage(ByVal DB1 As Database, ByVal ItemId As Integer, ByVal Image As String) As StoreItemImageRow
            Dim row As New StoreItemImageRow(DB1)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREITEMIMAGE_GETOBJECT As String = "sp_StoreItemImage_GetRowByItemIdAndImage"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMIMAGE_GETOBJECT)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "Image", DbType.String, Image)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    row.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            
            '------------------------------------------------------------------------
            Return row
        End Function
        Public Shared Function GetListImageByItem(ByVal ItemId As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer) As StoreItemImageCollection
            Dim c As New StoreItemImageCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemImage_GetListImageByItem"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                dbAcess.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                dbAcess.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                dbAcess.AddOutParameter(cmd, "TotalRecords", DbType.Int16, 1)
                dr = dbAcess.ExecuteReader(cmd)
                While dr.Read
                    Dim item As New StoreItemImageRow
                    item.Id = dr("Id")
                    item.Image = dr("Image")
                    If IsDBNull(dr.Item("ImageAltTag")) Then
                        item.ImageAltTag = Nothing
                    Else
                        item.ImageAltTag = dr("ImageAltTag")
                    End If
                    If total <= 0 Then
                        total = Convert.ToInt32(dr("TotalRecords"))
                    End If
                    c.Add(item)
                End While

            Catch ex As Exception

            End Try
            Core.CloseReader(dr)
            Return c
        End Function
        Public Shared Function ChangeOrder(ByVal _Database As Database, ByVal id As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_StoreItemImage_ChangeOrder"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, id))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
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
       
		Public Shared Function GetRowByItemAndType(ByVal DB As Database, ByVal ItemId As Integer, ByVal ImageType As String) As StoreItemImageRow
			Dim row As StoreItemImageRow
            Dim Id As Integer
            Id = DB.ExecuteScalar("SELECT Id FROM StoreItemImage WHERE ItemId=" & ItemId & " AND ImageType = " & DB.Quote(ImageType))
            row = New StoreItemImageRow(DB, Id)

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
			Dim row As StoreItemImageRow

			row = New StoreItemImageRow(DB, Id)
			row.Remove()
		End Sub

		'Custom Methods

	End Class

	Public MustInherit Class StoreItemImageRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_ItemId As Integer = Nothing
		Private m_Image As String = Nothing
		Private m_ImageAltTag As String = Nothing
		Private m_SortOrder As Integer = Nothing


		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = Value
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

		Public Property Image() As String
			Get
				Return m_Image
			End Get
			Set(ByVal Value As String)
				m_Image = Value
			End Set
		End Property

		Public Property ImageAltTag() As String
			Get
				Return m_ImageAltTag
			End Get
			Set(ByVal Value As String)
				m_ImageAltTag = Value
			End Set
		End Property

		Public Property SortOrder() As Integer
			Get
				Return m_SortOrder
			End Get
			Set(ByVal Value As Integer)
				m_SortOrder = Value
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
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal Id As Integer)
			m_DB = DB
			m_Id = Id
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ItemId As Integer, ByVal Image As String)
			m_DB = DB
			m_ItemId = ItemId
			m_Image = Image
		End Sub	'New

		Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREITEMIMAGE_GETOBJECT As String = "sp_StoreItemImage_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMIMAGE_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            
            '------------------------------------------------------------------------
		End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        m_Image = reader("Image").ToString()
                    Else
                        m_Image = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ImageAltTag"))) Then
                        m_ImageAltTag = reader("ImageAltTag").ToString()
                    Else
                        m_ImageAltTag = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                        m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                    Else
                        m_SortOrder = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEMIMAGE_INSERT As String = "sp_StoreItemImage_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMIMAGE_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "ImageAltTag", DbType.String, ImageAltTag)
            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))

            '------------------------------------------------------------------------
			Return Id
		End Function

		Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEMIMAGE_UPDATE As String = "sp_StoreItemImage_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMIMAGE_UPDATE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "ImageAltTag", DbType.String, ImageAltTag)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

		End Sub	'Update

		Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:04 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREITEMIMAGE_DELETE As String = "sp_StoreItemImage_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREITEMIMAGE_DELETE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreItemImageCollection
        Inherits GenericCollection(Of StoreItemImageRow)
    End Class
End Namespace


