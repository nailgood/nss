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

	Public Class StoreItemGroupOptionRow
		Inherits StoreItemGroupOptionRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal OptionId As Integer)
			MyBase.New(DB, OptionId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal OptionName As String)
			MyBase.New(DB, OptionName)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal OptionId As Integer) As StoreItemGroupOptionRow
			Dim row As StoreItemGroupOptionRow

			row = New StoreItemGroupOptionRow(DB, OptionId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal OptionName As String) As StoreItemGroupOptionRow
			Dim row As StoreItemGroupOptionRow

			row = New StoreItemGroupOptionRow(DB, OptionName)
			row.Load()

			Return row
		End Function
        Public Shared Function Delete(ByVal optionId As Integer) As Integer
            If optionId < 1 Then
                Return 0
            End If
            Try

                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemGroupOption_Delete"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "OptionId", DbType.Int32, optionId)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Core.LogError("StoreItemGroupOption.vb", "Delete(ByVal optionId:=" & optionId & " As Integer)", ex)
            End Try
            Return 0
        End Function

		
        
        'end 23/10/2009
        'Custom Methods
        Public Shared Function GetListByItemGroupId(ByVal itemGroupid As Integer) As StoreItemGroupOptionCollection
            Dim dr As SqlDataReader = Nothing
            Dim lstResult As New StoreItemGroupOptionCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreItemGroupOption_ListByItemGroup"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, itemGroupid)
                dr = db.ExecuteReader(cmd)
                While dr.Read()
                    lstResult.Add(LoadByReader(dr))
                End While
                Core.CloseReader(dr)
                Return lstResult
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog("GetByItemId", ex)
            End Try
            Return Nothing
        End Function
        Protected Shared Function LoadByReader(ByVal r As IDataReader) As StoreItemGroupOptionRow
            Dim objData As New StoreItemGroupOptionRow
            Try
                objData.OptionId = r.Item("OptionId")
                If IsDBNull(r.Item("OptionName")) Then
                    objData.OptionName = Nothing
                Else
                    objData.OptionName = Convert.ToString(r.Item("OptionName"))
                End If
                Return objData
            Catch ex As Exception
                Throw ex
            End Try
            Return Nothing
        End Function 'Load

        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("StoreItemGroupOption.vb", func, ex)
        End Sub
		Public Shared Function GetAllItemGroupOptions(ByVal DB As Database) As DataSet
			Dim ds As DataSet = DB.GetDataSet("select * from StoreItemGroupOption order by OptionName")
			Return ds
		End Function

		Public Shared Function GetAllStoreItemGroupOptions(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select * from StoreItemGroupOption  order by OptionName")
			Return ds
		End Function

	End Class

	Public MustInherit Class StoreItemGroupOptionRowBase
		Private m_DB As Database
		Private m_OptionId As Integer = Nothing
		Private m_OptionName As String = Nothing


		Public Property OptionId() As Integer
			Get
				Return m_OptionId
			End Get
			Set(ByVal Value As Integer)
				m_OptionId = value
			End Set
		End Property

		Public Property OptionName() As String
			Get
				Return m_OptionName
			End Get
			Set(ByVal Value As String)
				m_OptionName = value
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

		Public Sub New(ByVal DB As Database, ByVal OptionId As Integer)
			m_DB = DB
			m_OptionId = OptionId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal OptionName As String)
			m_DB = DB
			m_OptionName = OptionName
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemGroupOption WHERE " & IIf(OptionId = Nothing, "OptionName = " & DB.Quote(OptionName), "OptionId = " & DB.Number(OptionId))
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

            m_OptionId = Convert.ToInt32(r.Item("OptionId"))
            If IsDBNull(r.Item("OptionName")) Then
                m_OptionName = Nothing
            Else
                m_OptionName = Convert.ToString(r.Item("OptionName"))
            End If
        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO StoreItemGroupOption (" _
			 & " OptionName" _
			 & ") VALUES (" _
			 & m_DB.Quote(OptionName) _
			 & ")"

			OptionId = m_DB.InsertSQL(SQL)

			Return OptionId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE StoreItemGroupOption SET " _
			 & " OptionName = " & m_DB.Quote(OptionName) _
			 & " WHERE OptionId = " & m_DB.quote(OptionId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		
	End Class

	Public Class StoreItemGroupOptionCollection
		Inherits GenericCollection(Of StoreItemGroupOptionRow)
	End Class

End Namespace


