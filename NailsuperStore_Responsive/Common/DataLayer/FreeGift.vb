Option Explicit On

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

	Public Class FreeGiftRow
		Inherits FreeGiftRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

		Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal FreeGiftId As Integer)
			MyBase.New(DB, FreeGiftId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal FreeGiftId As Integer) As FreeGiftRow
			Dim row As FreeGiftRow

			row = New FreeGiftRow(DB, FreeGiftId)
			row.Load()
            Return row
        End Function

        Public Shared Sub Delete(ByVal FreeGiftId As Integer)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "exec dbo.sp_FreeGift_Delete " & FreeGiftId
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "FreeGift.vb > Delete", "FreeGiftId:" & FreeGiftId & "<br>Exception: " & ex.ToString())
            End Try
        End Sub

        Public Shared Sub ChangeArrangeItem(ByVal FreeGiftId As Integer, ByVal IsUp As Boolean)

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_FreeGift_ChangeArrange"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "FreeGiftId", DbType.Int32, FreeGiftId)
                db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)

                db.ExecuteNonQuery(cmd)
            Catch ex As Exception

            End Try

        End Sub
        Public Shared Sub ChangeIsActive(ByVal FreeGiftId As Integer)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "exec dbo.sp_FreeGift_ChangeIsActive " & FreeGiftId
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "FreeGift.vb > ChangeIsActive", "FreeGiftId:" & FreeGiftId & "<br>Exception: " & ex.ToString())

            End Try

        End Sub
        Public Shared Sub ChangeIsAddCart(ByVal ItemId As Integer, ByVal FreeGiftId As Integer)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "exec dbo.sp_FreeGift_ChangeIsAddCart " & ItemId & ", " & FreeGiftId
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                db.ExecuteNonQuery(cmd)
                StoreItemRow.ClearItemCache(ItemId)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "FreeGift.vb > ChangeIsAddCart", "ItemId:" & ItemId & "<br>Exception: " & ex.ToString())

            End Try

        End Sub
        Public Shared Function GetFreeGiftColection(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer) As FreeGiftCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim c As New FreeGiftCollection
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP As String = "sp_FreeGift_List"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "OrderBy", DbType.String, "FreeGiftId")
                db.AddInParameter(cmd, "OrderDirection", DbType.String, "DESC")
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                Dim Count As Integer = 0
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Count = Count + 1
                    Dim row As New FreeGiftRow(DB1)
                    row.FreeGiftId = dr("FreeGiftId")
                    row.ItemId = dr("ItemId")
                    row.LevelId = dr("Level")
                    row.Image = dr("Image")
                    row.IsActive = dr("IsActive")
                    row.ItemName = dr("ItemName")
                    row.Banner = IIf(IsDBNull(dr("Banner")), Nothing, dr("Banner"))
                    row.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                    c.Add(row)
                End While
                If Count Mod 2 <> 0 Then
                    Dim FG As New FreeGiftRow(DB1)
                    FG.FreeGiftId = 111111111
                    ''FG.MinimumAmount = 11111111111
                    FG.Image = ""
                    FG.IsActive = 0
                    FG.Image = "nobg.gif"
                    FG.Banner = "nobg.gif"
                    c.Add(FG)
                End If
                Core.CloseReader(dr)
                Return c
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return New FreeGiftCollection
        End Function

	End Class

	Public MustInherit Class FreeGiftRowBase
		Private m_DB As Database
		Private m_FreeGiftId As Integer = Nothing
		Private m_ItemId As Integer = Nothing
        Private m_LevelId As Integer = Nothing
		Private m_IsActive As Boolean = Nothing
        Private m_Image As String = Nothing
        Private m_Banner As String = Nothing
        Private m_ItemName As String = Nothing

		Public Property FreeGiftId() As Integer
			Get
				Return m_FreeGiftId
			End Get
			Set(ByVal Value As Integer)
				m_FreeGiftId = value
			End Set
		End Property

		Public Property Image() As String
			Get
				Return m_Image
			End Get
			Set(ByVal Value As String)
				m_Image = value
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

        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal Value As String)
                m_ItemName = Value
            End Set
        End Property

        Public Property LevelId() As Integer
            Get
                Return m_LevelId
            End Get
            Set(ByVal Value As Integer)
                m_LevelId = Value
            End Set
        End Property

		Public Property IsActive() As Boolean
			Get
				Return m_IsActive
			End Get
			Set(ByVal Value As Boolean)
				m_IsActive = value
			End Set
		End Property

        Public Property Banner() As String
            Get
                Return m_Banner
            End Get
            Set(ByVal Value As String)
                m_Banner = Value
            End Set
        End Property
		Public Property DB() As Database
			Get
				DB = m_DB
			End Get
			Set(ByVal Value As DataBase)
				m_DB = Value
			End Set
		End Property

		Public Sub New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal FreeGiftId As Integer)
			m_DB = DB
			m_FreeGiftId = FreeGiftId
		End Sub	'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try

                Dim SQL As String

                SQL = "SELECT * FROM FreeGift WHERE FreeGiftId = " & DB.Number(FreeGiftId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub

		Protected Overridable Sub Load(ByVal r As sqlDataReader)
			m_FreeGiftId = Convert.ToInt32(r.Item("FreeGiftId"))
			m_ItemId = Convert.ToInt32(r.Item("ItemId"))
            m_LevelId = Convert.ToInt32(r.Item("LevelId"))
            m_Image = Convert.ToString(r.Item("Image"))
            m_Banner = Convert.ToString(r.Item("Banner"))
			m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


            SQL = " INSERT INTO FreeGift (" _
             & " ItemId" _
             & ",LevelId" _
             & ",Image" _
             & ",IsActive" _
             & ",Banner" _
             & ",Arrange" _
             & ") VALUES (" _
             & m_DB.NullNumber(ItemId) _
             & "," & m_DB.Number(LevelId) _
             & "," & m_DB.Quote(Image) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Quote(Banner) _
             & ", (SELECT (ISNULL(MAX(Arrange), 0) + 1) FROM FreeGift WHERE LevelId = " & DB.Quote(LevelId) & "))"

			FreeGiftId = m_DB.InsertSQL(SQL)

			Return FreeGiftId
		End Function

		Public Overridable Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_FreeGift_Update"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "FreeGiftId", DbType.Int32, FreeGiftId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "LevelId", DbType.Int32, LevelId)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                db.AddInParameter(cmd, "Banner", DbType.String, Banner)
                db.AddInParameter(cmd, "Image", DbType.String, Image)

                db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "FreeGift.vb > Update", "FreeGiftId:" & FreeGiftId & "<br>Exception: " & ex.ToString())
            End Try

        End Sub 'Update

    End Class

	Public Class FreeGiftCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal FreeGift As FreeGiftRow)
            Me.List.Add(FreeGift)
        End Sub

        Public Function Contains(ByVal FreeGift As FreeGiftRow) As Boolean
            Return Me.List.Contains(FreeGift)
        End Function

        Public Function IndexOf(ByVal FreeGift As FreeGiftRow) As Integer
            Return Me.List.IndexOf(FreeGift)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal FreeGift As FreeGiftRow)
            Me.List.Insert(Index, FreeGift)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As FreeGiftRow
            Get
                Return CType(Me.List.Item(Index), FreeGiftRow)
            End Get

            Set(ByVal Value As FreeGiftRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal FreeGift As FreeGiftRow)
            Me.List.Remove(FreeGift)
        End Sub
	End Class

End Namespace


