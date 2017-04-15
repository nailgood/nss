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

	Public Class StoreItemGroupRow
		Inherits StoreItemGroupRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
        End Sub 'New

		Public Sub New(ByVal DB As Database, ByVal ItemGroupId As Integer)
			MyBase.New(DB, ItemGroupId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal GroupName As String)
			MyBase.New(DB, GroupName)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ItemGroupId As Integer) As StoreItemGroupRow
			Dim row As StoreItemGroupRow

			row = New StoreItemGroupRow(DB, ItemGroupId)
			row.Load()

			Return row
        End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal GroupName As String) As StoreItemGroupRow
			Dim row As StoreItemGroupRow

			row = New StoreItemGroupRow(DB, GroupName)
			row.Load()

			Return row
		End Function

		
        
        'Custom Methods
		Public ReadOnly Property GetSelectedStoreItemGroupOptions() As String
            Get
                Dim dr As SqlDataReader = Nothing
                Dim Result As String = String.Empty
                Try
                    dr = DB.GetReader("select OptionId from StoreItemGroupOptionRel where ItemGroupId = " & ItemGroupId)
                    Dim Conn As String = String.Empty
                    While dr.Read()
                        Result &= Conn & dr("OptionId")
                        Conn = ","
                    End While
                    Core.CloseReader(dr)
                Catch ex As Exception
                    Core.CloseReader(dr)
                End Try
                Return Result
            End Get
		End Property

		Public Sub DeleteFromAllStoreItemGroupOptions()
			DB.ExecuteSQL("delete from StoreItemGroupOptionRel where ItemGroupId = " & ItemGroupId)
		End Sub

        Public Sub InsertToStoreItemGroupOptions(ByVal Elements As String)
            If Elements = String.Empty Then Exit Sub

            Dim aElements As String() = Elements.Split(",")
            Dim iSort As Integer = 1
            For Each Element As String In aElements
                InsertToStoreItemGroupOption(Element, iSort)
                iSort = iSort + 1
            Next
        End Sub

        Public Sub InsertToStoreItemGroupOption(ByVal OptionId As Integer, ByVal iSort As Integer)
            Dim SQL As String = "insert into StoreItemGroupOptionRel (ItemGroupId, OptionId, SortOrder) values (" & ItemGroupId & "," & OptionId & "," & iSort & ")"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub ChangeArrangeStoreItemGroupOptionRel(ByVal ItemGroupId As Integer, ByVal OptionId As Integer, ByVal SortAction As String)
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_CHANGEARRANGE As String = "sp_StoreItemGroupOptionRel_ChangeArrange"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CHANGEARRANGE)
                db.AddInParameter(cmd, "ItemGroupId", DbType.Int32, ItemGroupId)
                db.AddInParameter(cmd, "OptionId", DbType.Int32, OptionId)
                db.AddInParameter(cmd, "SortAction", DbType.String, SortAction)
                db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Core.LogError("StoreItemGroup.vb", "ChangeArrangeStoreItemGroupOptionRel - ItemGroupId = " & ItemGroupId, ex)
            End Try
        End Sub

        Public Function GetStoreItemGroupOptions() As DataSet
            Dim SQL As String = "select optionname, optionid from storeitemgroupoption where optionid in (select optionid from storeitemgroupoptionrel where itemgroupid = " & ItemGroupId & ") order by sortorder"
            Return DB.GetDataSet(SQL)
        End Function

        Public Function GetRowsByOption(ByVal DB As Database, ByVal OptionId As Integer, ByVal Choices As String) As DataSet
            Dim SQL As String = "select choiceid, choicename from storeitemgroupchoice where choiceid in (select choiceid from storeitemgroupchoicerel where optionid = " & OptionId & " and itemid in (select si.itemid from storeitem si " & IIf(Choices <> String.Empty, " inner join storeitemgroupchoicerel r on si.itemid = r.itemid where r.choiceid in " & DB.NumberMultiple(Choices) & " and ", " where ") & " isactive = 1 and itemgroupid = " & ItemGroupId & ")) order by choicename"
            Return DB.GetDataSet(SQL)
        End Function

        Public Shared Function GetAllItemGroups(ByVal DB As Database) As DataTable
            Dim SQL As String = "select * from storeitemgroup order by groupname"
            Return DB.GetDataTable(SQL)
        End Function
        Public Shared Function Delete(ByVal groupid As Integer) As Integer
            If groupid < 1 Then
                Return 0
            End If
            Try

                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreItemGroup_Delete"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "ItemGroupId", DbType.Int32, groupid)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Core.LogError("StoreItemGroup.vb", "Delete(ByVal groupid:=" & groupid & " As Integer)", ex)
            End Try
            Return 0
        End Function

    End Class

    Public MustInherit Class StoreItemGroupRowBase
        Private m_DB As Database
        Private m_ItemGroupId As Integer = Nothing
        Private m_GroupName As String = Nothing


        Public Property ItemGroupId() As Integer
            Get
                Return m_ItemGroupId
            End Get
            Set(ByVal Value As Integer)
                m_ItemGroupId = Value
            End Set
        End Property

        Public Property GroupName() As String
            Get
                Return m_GroupName
            End Get
            Set(ByVal Value As String)
                m_GroupName = Value
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

        Public Sub New(ByVal DB As Database, ByVal ItemGroupId As Integer)
            m_DB = DB
            m_ItemGroupId = ItemGroupId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal GroupName As String)
            m_DB = DB
            m_GroupName = GroupName
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemGroup WHERE " & IIf(ItemGroupId = Nothing, "Groupname = " & DB.Quote(GroupName), "ItemGroupId = " & DB.Number(ItemGroupId))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try


        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_ItemGroupId = Convert.ToInt32(r.Item("ItemGroupId"))
            m_GroupName = Convert.ToString(r.Item("GroupName"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO StoreItemGroup (" _
             & " GroupName" _
             & ") VALUES (" _
             & m_DB.Quote(GroupName) _
             & ")"

            ItemGroupId = m_DB.InsertSQL(SQL)

            Return ItemGroupId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreItemGroup SET " _
             & " GroupName = " & m_DB.Quote(GroupName) _
             & " WHERE ItemGroupId = " & m_DB.Quote(ItemGroupId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update


    End Class

    Public Class StoreItemGroupCollection
        Inherits GenericCollection(Of StoreItemGroupRow)
    End Class

End Namespace


