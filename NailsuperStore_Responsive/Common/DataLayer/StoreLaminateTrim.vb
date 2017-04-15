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

	Public Class StoreLaminateTrimRow
		Inherits StoreLaminateTrimRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal LaminateTrimId As Integer)
			MyBase.New(DB, LaminateTrimId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal LaminateTrimId As Integer) As StoreLaminateTrimRow
			Dim row As StoreLaminateTrimRow

			row = New StoreLaminateTrimRow(DB, LaminateTrimId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal LaminateTrimId As Integer)
			Dim row As StoreLaminateTrimRow

			row = New StoreLaminateTrimRow(DB, LaminateTrimId)
			row.Remove()
		End Sub
        
        'Custom Methods
		Public Shared Function GetAllRows(ByVal DB As Database) As DataSet
			Return DB.GetDataSet("select * from storelaminatetrim order by laminatetrim")
		End Function
        Public Shared Function GetNameById(ByVal id As Integer) As String
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select LaminateTrim from StoreLaminateTrim where LaminateTrimId=" & id
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
	End Class

	Public MustInherit Class StoreLaminateTrimRowBase
		Private m_DB As Database
		Private m_LaminateTrimId As Integer = Nothing
		Private m_LaminateTrim As String = Nothing
		Private m_Swatch As String = Nothing


		Public Property LaminateTrimId() As Integer
			Get
				Return m_LaminateTrimId
			End Get
			Set(ByVal Value As Integer)
				m_LaminateTrimId = Value
			End Set
		End Property

		Public Property LaminateTrim() As String
			Get
				Return m_LaminateTrim
			End Get
			Set(ByVal Value As String)
				m_LaminateTrim = Value
			End Set
		End Property

		Public Property Swatch() As String
			Get
				Return m_Swatch
			End Get
			Set(ByVal Value As String)
				m_Swatch = Value
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

		Public Sub New(ByVal DB As Database, ByVal LaminateTrimId As Integer)
			m_DB = DB
			m_LaminateTrimId = LaminateTrimId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreLaminateTrim WHERE LaminateTrimId = " & DB.Number(LaminateTrimId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
			

		End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_LaminateTrimId = Convert.ToInt32(r.Item("LaminateTrimId"))
                m_LaminateTrim = Convert.ToString(r.Item("LaminateTrim"))
                If IsDBNull(r.Item("Swatch")) Then
                    m_Swatch = Nothing
                Else
                    m_Swatch = Convert.ToString(r.Item("Swatch"))
                End If
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO StoreLaminateTrim (" _
			 & " LaminateTrim" _
			 & ",Swatch" _
			 & ") VALUES (" _
			 & m_DB.Quote(LaminateTrim) _
			 & "," & m_DB.Quote(Swatch) _
			 & ")"

			LaminateTrimId = m_DB.InsertSQL(SQL)

			Return LaminateTrimId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE StoreLaminateTrim SET " _
			 & " LaminateTrim = " & m_DB.Quote(LaminateTrim) _
			 & ",Swatch = " & m_DB.Quote(Swatch) _
			 & " WHERE LaminateTrimId = " & m_DB.Quote(LaminateTrimId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM StoreLaminateTrim WHERE LaminateTrimId = " & m_DB.Quote(LaminateTrimId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class StoreLaminateTrimCollection
		Inherits GenericCollection(Of StoreLaminateTrimRow)
	End Class

End Namespace


