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

	Public Class StoreItemGroupChoiceRow
		Inherits StoreItemGroupChoiceRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ChoiceId As Integer)
			MyBase.New(DB, ChoiceId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ChoiceName As String, ByVal OptionId As Integer)
			MyBase.New(DB, ChoiceName, OptionId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ChoiceId As Integer) As StoreItemGroupChoiceRow
			Dim row As StoreItemGroupChoiceRow

			row = New StoreItemGroupChoiceRow(DB, ChoiceId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal ChoiceName As String, ByVal OptionId As Integer) As StoreItemGroupChoiceRow
			Dim row As StoreItemGroupChoiceRow

			row = New StoreItemGroupChoiceRow(DB, ChoiceName, OptionId)
			row.Load()

			Return row
		End Function

		
      
        'end 23/10/2009
        'Custom Methods
		Public Shared Function GetRowsByOption(ByVal DB As Database, ByVal OptionId As Integer) As DataSet
			Return DB.GetDataSet("select * from storeitemgroupchoice where optionid = " & OptionId)
		End Function
        Public Shared Function Delete(ByVal choiceId As Integer) As Integer
            If choiceId < 1 Then
                Return 0
            End If
            Try
                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemGroupChoice_Delete"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "ChoiceId", DbType.Int32, choiceId)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Core.LogError("StoreItemGroupChoice.vb", "Delete(ByVal choiceId:=" & choiceId & " As Integer)", ex)
            End Try
            Return 0
        End Function

    End Class

	Public MustInherit Class StoreItemGroupChoiceRowBase
		Private m_DB As Database
		Private m_ChoiceId As Integer = Nothing
		Private m_OptionId As Integer = Nothing
		Private m_ChoiceName As String = Nothing
        Private m_ThumbImage As String = Nothing

		Public Property ChoiceId() As Integer
			Get
				Return m_ChoiceId
			End Get
			Set(ByVal Value As Integer)
				m_ChoiceId = value
			End Set
		End Property

		Public Property OptionId() As Integer
			Get
				Return m_OptionId
			End Get
			Set(ByVal Value As Integer)
				m_OptionId = value
			End Set
		End Property

        Public Property ThumbImage() As String
            Get
                Return m_ThumbImage
            End Get
            Set(ByVal Value As String)
                m_ThumbImage = Value
            End Set
        End Property
        Public Property ChoiceName() As String
            Get
                Return m_ChoiceName
            End Get
            Set(ByVal Value As String)
                m_ChoiceName = value
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

		Public Sub New(ByVal DB As Database, ByVal ChoiceId As Integer)
			m_DB = DB
			m_ChoiceId = ChoiceId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ChoiceName As String, ByVal OptionId As Integer)
			m_DB = DB
			m_ChoiceName = ChoiceName
			m_OptionId = OptionId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT ChoiceId,OptionId,ChoiceName,SortOrder,ThumbImage FROM StoreItemGroupChoice WHERE " & IIf(ChoiceId = Nothing, "ChoiceName = " & DB.Quote(ChoiceName) & " and OptionId = " & OptionId, "ChoiceId = " & DB.Number(ChoiceId))
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
                m_ChoiceId = Convert.ToInt32(r.Item("ChoiceId"))
                If IsDBNull(r.Item("OptionId")) Then
                    m_OptionId = Nothing
                Else
                    m_OptionId = Convert.ToInt32(r.Item("OptionId"))
                End If
                If IsDBNull(r.Item("ChoiceName")) Then
                    m_ChoiceName = Nothing
                Else
                    m_ChoiceName = Convert.ToString(r.Item("ChoiceName"))
                End If
                If IsDBNull(r.Item("ThumbImage")) Then
                    m_ThumbImage = Nothing
                Else
                    m_ThumbImage = Convert.ToString(r.Item("ThumbImage"))
                End If
            Catch ex As Exception

            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


            SQL = " INSERT INTO StoreItemGroupChoice (" _
             & " OptionId" _
             & ",ChoiceName" _
             & ",ThumbImage" _
             & ") VALUES (" _
             & m_DB.NullNumber(OptionId) _
             & "," & m_DB.Quote(ChoiceName) _
             & "," & m_DB.Quote(ThumbImage) _
             & ")"

			ChoiceId = m_DB.InsertSQL(SQL)

			Return ChoiceId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

            SQL = " UPDATE StoreItemGroupChoice SET " _
             & " OptionId = " & m_DB.NullNumber(OptionId) _
             & ",ChoiceName = " & m_DB.Quote(ChoiceName) _
            & ",ThumbImage = " & m_DB.Quote(ThumbImage) _
             & " WHERE ChoiceId = " & m_DB.Quote(ChoiceId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		
	End Class

	Public Class StoreItemGroupChoiceCollection
		Inherits GenericCollection(Of StoreItemGroupChoiceRow)
	End Class

End Namespace


