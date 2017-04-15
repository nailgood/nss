Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components


Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Database
Namespace DataLayer

    Public Class MemberAddressRow
        Inherits MemberAddressRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal AddressId As Integer)
            MyBase.New(DB, AddressId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal AddressId As Integer) As MemberAddressRow
            Dim row As MemberAddressRow

            row = New MemberAddressRow(DB, AddressId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal AddressId As Integer)
            Dim row As MemberAddressRow

            row = New MemberAddressRow(DB, AddressId)
            row.Remove()
        End Sub
        Public Shared Function IsNotCompleteAddress(ByVal DB As Database, ByVal memberID As Integer) As Boolean
            If memberID < 1 Then
                Return True
            End If
            Try
                Dim result As Integer = DB.ExecuteScalar("Select [dbo].[fc_MemberAddress_IsNotComplete](" & memberID & ")")
                If result = 1 Then
                    Return True
                ElseIf result = 99 Then ' Cap nhat lai truong hop table Member LastOrderId=0 nhung StoreOrder co OrderId
                    result = DB.ExecuteScalar("Update Member SET LastOrderId = (SELECT TOP 1 OrderId FROM StoreOrder WHERE OrderNo IS NULL AND MemberId = " & memberID & " ORDER BY OrderId DESC) WHERE MemberId = " & memberID)
                    Return True
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "IsNotCompleteAddress", "memberId=" & memberID & "<br>Exception: " & ex.ToString() + "")

            End Try
            Return False
        End Function
        Private Shared Function GetFromDataReader(ByVal r As SqlDataReader) As MemberAddressRow
            Dim result As New MemberAddressRow
            Try
                result.AddressId = Convert.ToInt32(r.Item("AddressId"))
                result.MemberId = Convert.ToInt32(r.Item("MemberId"))
                result.AddressType = Convert.ToString(r.Item("AddressType"))
                result.Label = Convert.ToString(r.Item("Label"))
                If IsDBNull(r.Item("Company")) Then
                    result.Company = Nothing
                Else
                    result.Company = Convert.ToString(r.Item("Company"))
                End If
                result.FirstName = Convert.ToString(r.Item("FirstName"))
                If IsDBNull(r.Item("MiddleInitial")) Then
                    result.MiddleInitial = Nothing
                Else
                    result.MiddleInitial = Convert.ToString(r.Item("MiddleInitial"))
                End If
                result.LastName = Convert.ToString(r.Item("LastName"))
                result.Address1 = Convert.ToString(r.Item("Address1"))
                If IsDBNull(r.Item("Address2")) Then
                    result.Address2 = Nothing
                Else
                    result.Address2 = Convert.ToString(r.Item("Address2"))
                End If
                result.City = Convert.ToString(r.Item("City"))
                If IsDBNull(r.Item("State")) Then
                    result.State = Nothing
                Else
                    result.State = Convert.ToString(r.Item("State"))
                End If
                If IsDBNull(r.Item("Region")) Then
                    result.Region = Nothing
                Else
                    result.Region = Convert.ToString(r.Item("Region"))
                End If
                If IsDBNull(r.Item("Zip")) Then
                    result.Zip = Nothing
                Else
                    result.Zip = Convert.ToString(r.Item("Zip"))
                End If
                If IsDBNull(r.Item("Country")) Then
                    result.Country = Nothing
                Else
                    result.Country = Convert.ToString(r.Item("Country"))
                End If
                If IsDBNull(r.Item("Email")) Then
                    result.Email = Nothing
                Else
                    result.Email = Convert.ToString(r.Item("Email"))
                End If
                If IsDBNull(r.Item("Phone")) Then
                    result.Phone = Nothing
                Else
                    result.Phone = Convert.ToString(r.Item("Phone"))
                End If
                If IsDBNull(r.Item("PhoneExt")) Then
                    result.PhoneExt = Nothing
                Else
                    result.PhoneExt = Convert.ToString(r.Item("PhoneExt"))
                End If
                If IsDBNull(r.Item("Fax")) Then
                    result.Fax = Nothing
                Else
                    result.Fax = Convert.ToString(r.Item("Fax"))
                End If
                result.IsDefault = Convert.ToBoolean(r.Item("IsDefault"))
                result.CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                result.ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                If IsDBNull(r.Item("LastExport")) Then
                    result.LastExport = Nothing
                Else
                    result.LastExport = Convert.ToDateTime(r.Item("LastExport"))
                End If
                If IsDBNull(r.Item("LastImport")) Then
                    result.LastImport = Nothing
                Else
                    result.LastImport = Convert.ToDateTime(r.Item("LastImport"))
                End If
                result.IsDeleted = Convert.ToBoolean(r.Item("IsDeleted"))
                result.DoExport = Convert.ToBoolean(r.Item("DoExport"))
            Catch ex As Exception
                result = Nothing
                Throw ex
            End Try
            Return result
        End Function
        Public Shared Function GetAddressByType(ByVal _Database As Database, ByVal memberId As Integer, ByVal addressType As String) As MemberAddressRow
            If memberId < 1 Then
                Return New MemberAddressRow
            End If
            Dim dr As SqlDataReader = Nothing
            Dim ss As New MemberAddressRow
            Try
                Dim sp As String = "sp_MemberAddress_GetByAddressType"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                cmd.Parameters.Add(_Database.InParam("AddressType", SqlDbType.VarChar, 0, addressType))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss = GetFromDataReader(dr)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Public Shared Function GetOrderAddressByType(ByVal _Database As Database, ByVal orderId As Integer, ByVal addressType As String) As MemberAddressRow
            Dim dr As SqlDataReader = Nothing
            Dim ss As New MemberAddressRow
            Try
                Dim sp As String = "sp_MemberAddress_GetOrderAddressByType"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.InParam("AddressType", SqlDbType.VarChar, 0, addressType))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss = GetFromDataReader(dr)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Public Shared Function GetListAddressByType(ByVal memberId As Integer, ByVal addressType As String) As List(Of MemberAddressRow)
            Dim dr As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_MemberAddress_GetListAddressByType"
            Dim address As New List(Of MemberAddressRow)

            Try               
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)
                db.AddInParameter(cmd, "AddressType", DbType.String, addressType)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    address = mapList(Of MemberAddressRow)(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetListAddressByType", "memberId=" & memberId & ",addressType=" & addressType & "<br>Exception: " & ex.ToString() + "")
            End Try

            Return address
        End Function
        Public Sub CopyFromNavision(ByVal r As NavisionAddressRow)
            Dim iMemberId As Integer = DB.ExecuteScalar("select top 1 memberid from member m inner join customer c on m.customerid = c.customerid where c.customerno = " & DB.Quote(r.Customer_No))
            If iMemberId = Nothing Then Exit Sub
            If Not AddressId = Nothing AndAlso Not MemberId = Nothing AndAlso iMemberId <> MemberId Then Exit Sub
            MemberId = iMemberId
            FirstName = r.First_Name
            LastName = r.Last_Name
            Address1 = r.Address_1
            Address2 = r.Address_2
            Company = r.Company & r.Company_2
            City = r.City
            State = r.State
            Region = r.Region
            Zip = r.Zip
            Label = r.Label
            Phone = r.Phone

            If AddressId = Nothing Then
                Select Case Label
                    Case "Default Billing Address"
                        AddressType = "Billing"
                    Case "Default Shipping Address"
                        AddressType = "Shipping"
                    Case Else
                        AddressType = "AddressBook"
                End Select
                Insert()
            Else
                Update()
            End If
        End Sub

        'Custom Methods
        Public Sub MarkDeleted()
            DB.ExecuteSQL("UPDATE MemberAddress SET IsDeleted = 1, DoExport = 1 WHERE AddressId = " & AddressId)
        End Sub
        Public Shared Function Delete(ByVal AddressId As Integer) As Boolean
            Dim result As Integer = 0
            Try

                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_MemberAddress_Delete"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "AddressId", DbType.Int32, AddressId)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("MemberAddress.vb", "Delete(ByVal AddressId:=" & AddressId & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function GetCollectionForExport(ByVal DB As Database) As MemberAddressCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New MemberAddressCollection
            Try
                Dim row As MemberAddressRow
                Dim SQL As String = "select * from memberaddress where DoExport = 1 AND AddressType = 'AddressBook' order by memberid"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New MemberAddressRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return c
        End Function
    End Class

    Public MustInherit Class MemberAddressRowBase
        Private m_DB As Database
        Private m_AddressId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_AddressType As String = Nothing
        Private m_Label As String = Nothing
        Private m_Company As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_MiddleInitial As String = Nothing
        Private m_LastName As String = Nothing
        Private m_Address1 As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_City As String = Nothing
        Private m_State As String = Nothing
        Private m_Region As String = Nothing
        Private m_Zip As String = Nothing
        Private m_Country As String = Nothing
        Private m_Email As String = Nothing
        Private m_Phone As String = Nothing
        Private m_Fax As String = Nothing
        Private m_IsDefault As Boolean = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_IsDeleted As Boolean = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing
        Private m_LastImport As DateTime = Nothing
        Private m_PhoneExt As String = Nothing
        Private m_DaytimePhone As String = Nothing
        Private m_DaytimePhoneExt As String = Nothing

        Public Property LastExport() As DateTime
            Get
                Return m_LastExport
            End Get
            Set(ByVal value As DateTime)
                m_LastExport = value
            End Set
        End Property

        Public Property LastImport() As DateTime
            Get
                Return m_LastImport
            End Get
            Set(ByVal value As DateTime)
                m_LastImport = value
            End Set
        End Property

        Public Property DoExport() As Boolean
            Get
                Return m_DoExport
            End Get
            Set(ByVal value As Boolean)
                m_DoExport = value
            End Set
        End Property

        Public Property IsDeleted() As Boolean
            Get
                Return m_IsDeleted
            End Get
            Set(ByVal value As Boolean)
                m_IsDeleted = value
            End Set
        End Property


        Public Property AddressId() As Integer
            Get
                Return m_AddressId
            End Get
            Set(ByVal Value As Integer)
                m_AddressId = Value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property AddressType() As String
            Get
                Return m_AddressType
            End Get
            Set(ByVal Value As String)
                m_AddressType = Value
            End Set
        End Property

        Public Property Label() As String
            Get
                Return m_Label
            End Get
            Set(ByVal Value As String)
                m_Label = Value
            End Set
        End Property

        Public Property Company() As String
            Get
                Return m_Company
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Company = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_FirstName = Value
            End Set
        End Property

        Public Property MiddleInitial() As String
            Get
                Return m_MiddleInitial
            End Get
            Set(ByVal Value As String)
                m_MiddleInitial = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_LastName = Value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return m_Address1
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Address1 = Value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return m_Address2
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Address2 = Value
            End Set
        End Property

        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_City = Value
            End Set
        End Property

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_State = Value
            End Set
        End Property

        Public Property Region() As String
            Get
                Return m_Region
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Region = Value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return m_Zip
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Zip = Value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_Country = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Return m_Phone
            End Get
            Set(ByVal Value As String)
                m_Phone = Value
            End Set
        End Property
        Public Property PhoneExt() As String
            Get
                Return m_PhoneExt
            End Get
            Set(ByVal Value As String)
                m_PhoneExt = Value
            End Set
        End Property
        Public Property DaytimePhone() As String
            Get
                Return m_DaytimePhone
            End Get
            Set(ByVal Value As String)
                m_DaytimePhone = Value
            End Set
        End Property
        Public Property DaytimePhoneExt() As String
            Get
                Return m_DaytimePhoneExt
            End Get
            Set(ByVal Value As String)
                m_DaytimePhoneExt = Value
            End Set
        End Property
        Public Property Fax() As String
            Get
                Return m_Fax
            End Get
            Set(ByVal Value As String)
                m_Fax = Value
            End Set
        End Property
        Public Property IsDefault() As Boolean
            Get
                Return m_IsDefault
            End Get
            Set(ByVal Value As Boolean)
                m_IsDefault = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
            End Set
        End Property

        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal Value As DateTime)
                m_ModifyDate = Value
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

        Public Sub New(ByVal DB As Database, ByVal AddressId As Integer)
            m_DB = DB
            m_AddressId = AddressId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM MemberAddress WHERE AddressId = " & DB.Quote(AddressId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_AddressId = Convert.ToInt32(r.Item("AddressId"))
            m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            m_AddressType = Convert.ToString(r.Item("AddressType"))
            m_Label = Convert.ToString(r.Item("Label"))
            If IsDBNull(r.Item("Company")) Then
                m_Company = Nothing
            Else
                m_Company = Convert.ToString(r.Item("Company"))
            End If
            m_FirstName = Convert.ToString(r.Item("FirstName"))
            If IsDBNull(r.Item("MiddleInitial")) Then
                m_MiddleInitial = Nothing
            Else
                m_MiddleInitial = Convert.ToString(r.Item("MiddleInitial"))
            End If
            m_LastName = Convert.ToString(r.Item("LastName"))
            m_Address1 = Convert.ToString(r.Item("Address1"))
            If IsDBNull(r.Item("Address2")) Then
                m_Address2 = Nothing
            Else
                m_Address2 = Convert.ToString(r.Item("Address2"))
            End If
            m_City = Convert.ToString(r.Item("City"))
            If IsDBNull(r.Item("State")) Then
                m_State = Nothing
            Else
                m_State = Convert.ToString(r.Item("State"))
            End If
            If IsDBNull(r.Item("Region")) Then
                m_Region = Nothing
            Else
                m_Region = Convert.ToString(r.Item("Region"))
            End If
            If IsDBNull(r.Item("Zip")) Then
                m_Zip = Nothing
            Else
                m_Zip = Convert.ToString(r.Item("Zip"))
            End If
            If IsDBNull(r.Item("Country")) Then
                m_Country = Nothing
            Else
                m_Country = Convert.ToString(r.Item("Country"))
            End If
            If IsDBNull(r.Item("Email")) Then
                m_Email = Nothing
            Else
                m_Email = Convert.ToString(r.Item("Email"))
            End If
            If IsDBNull(r.Item("Phone")) Then
                m_Phone = Nothing
            Else
                m_Phone = Convert.ToString(r.Item("Phone"))
            End If

            If IsDBNull(r.Item("PhoneExt")) Then
                m_PhoneExt = Nothing
            Else
                m_PhoneExt = Convert.ToString(r.Item("PhoneExt"))
            End If
            If IsDBNull(r.Item("DaytimePhone")) Then
                m_DaytimePhone = Nothing
            Else
                m_DaytimePhone = Convert.ToString(r.Item("DaytimePhone"))
            End If
            If IsDBNull(r.Item("DaytimePhoneExt")) Then
                m_DaytimePhoneExt = Nothing
            Else
                m_DaytimePhoneExt = Convert.ToString(r.Item("DaytimePhoneExt"))
            End If
            '''''''
            If IsDBNull(r.Item("Fax")) Then
                m_Fax = Nothing
            Else
                m_Fax = Convert.ToString(r.Item("Fax"))
            End If
            m_IsDefault = Convert.ToBoolean(r.Item("IsDefault"))
            m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
            m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
            If IsDBNull(r.Item("LastExport")) Then
                m_LastExport = Nothing
            Else
                m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
            End If
            If IsDBNull(r.Item("LastImport")) Then
                m_LastImport = Nothing
            Else
                m_LastImport = Convert.ToDateTime(r.Item("LastImport"))
            End If
            m_IsDeleted = Convert.ToBoolean(r.Item("IsDeleted"))
            m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Try
                Dim SQL As String
                SQL = " INSERT INTO MemberAddress (" _
                 & " MemberId" _
                 & ",AddressType" _
                 & ",Label" _
                 & ",Company" _
                 & ",FirstName" _
                 & ",MiddleInitial" _
                 & ",LastName" _
                 & ",Address1" _
                 & ",Address2" _
                 & ",City" _
                 & ",State" _
                 & ",Region" _
                 & ",Zip" _
                 & ",Country" _
                 & ",Email" _
                 & ",Phone" _
                 & ",PhoneExt" _
                 & ",DaytimePhone" _
                 & ",DaytimePhoneExt" _
                 & ",Fax" _
                 & ",IsDefault" _
                 & ",CreateDate" _
                 & ",ModifyDate" _
                 & ",IsDeleted" _
                 & ",DoExport" _
                 & ",LastExport" _
                 & ",LastImport" _
                 & ") VALUES (" _
                 & m_DB.Quote(MemberId) _
                 & "," & m_DB.Quote(AddressType) _
                 & "," & m_DB.Quote(Label) _
                 & "," & m_DB.NQuote(Company) _
                 & "," & m_DB.NQuote(FirstName) _
                 & "," & m_DB.Quote(MiddleInitial) _
                 & "," & m_DB.NQuote(LastName) _
                 & ",'" & Address1.Replace("'", "") & "'" _
                 & "," & m_DB.NQuote(Address2) _
                 & ",'" & City & "'" _
                 & "," & m_DB.NQuote(State) _
                 & "," & m_DB.NQuote(Region) _
                 & "," & m_DB.Quote(Zip) _
                 & "," & m_DB.Quote(Country) _
                 & "," & m_DB.Quote(Email) _
                 & "," & m_DB.Quote(Phone) _
                 & "," & m_DB.Quote(PhoneExt) _
                 & "," & m_DB.Quote(DaytimePhone) _
                 & "," & m_DB.Quote(DaytimePhoneExt) _
                 & "," & m_DB.Quote(Fax) _
                 & "," & CInt(IsDefault) _
                 & "," & m_DB.Quote(Now) _
                 & "," & m_DB.Quote(Now) _
                 & "," & CInt(IsDeleted) _
                 & "," & CInt(DoExport) _
                 & "," & m_DB.NullQuote(LastExport) _
                 & "," & m_DB.NullQuote(LastImport) _
                 & ")"
                AddressId = m_DB.InsertSQL(SQL)
                Return AddressId
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Overridable Sub Update()
            Dim SQL As String
            If AddressId = 0 Then
                Insert()
            Else
                SQL = " UPDATE MemberAddress SET " _
           & " MemberId = " & m_DB.Quote(MemberId) _
           & ",AddressType = " & m_DB.Quote(AddressType) _
           & ",Label = " & m_DB.Quote(Label) _
           & ",Company = " & m_DB.NQuote(Company) _
           & ",FirstName = " & m_DB.NQuote(FirstName) _
           & ",MiddleInitial = " & m_DB.Quote(MiddleInitial) _
           & ",LastName = " & m_DB.NQuote(LastName) _
           & ",Address1 = " & m_DB.NQuote(Address1) _
           & ",Address2 = " & m_DB.NQuote(Address2) _
           & ",City = " & m_DB.NQuote(City) _
           & ",State = " & m_DB.NQuote(State) _
           & ",Region = " & m_DB.NQuote(Region) _
           & ",Zip = " & m_DB.Quote(Zip) _
           & ",Country = " & m_DB.Quote(Country) _
           & ",Email = " & m_DB.Quote(Email) _
           & ",Phone = " & m_DB.Quote(Phone) _
           & ",PhoneExt = " & m_DB.Quote(PhoneExt) _
           & ",DaytimePhone = " & m_DB.Quote(DaytimePhone) _
           & ",DaytimePhoneExt = " & m_DB.Quote(DaytimePhoneExt) _
           & ",Fax = " & m_DB.Quote(Fax) _
           & ",IsDefault = " & CInt(IsDefault) _
           & ",ModifyDate = " & m_DB.Quote(Now) _
           & ",IsDeleted = " & CInt(IsDeleted) _
           & ",DoExport = " & CInt(DoExport) _
           & ",LastExport = " & m_DB.NullQuote(LastExport) _
           & ",LastImport = " & m_DB.NullQuote(LastImport) _
           & " WHERE AddressId = " & m_DB.Quote(AddressId)
                m_DB.ExecuteSQL(SQL)
            End If



        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String
            SQL = "DELETE FROM MemberAddress WHERE AddressId = " & m_DB.Quote(AddressId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class MemberAddressCollection
        Inherits GenericCollection(Of MemberAddressRow)
    End Class
End Namespace


