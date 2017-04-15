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

	Public Class MemberResumeRow
		Inherits MemberResumeRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ResumeId As Integer)
			MyBase.New(DB, ResumeId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal MemberId As Integer) As MemberResumeRow
			Dim row As MemberResumeRow

			row = New MemberResumeRow(DB, MemberId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MemberId As Integer)
			Dim row As MemberResumeRow

			row = New MemberResumeRow(DB, MemberId)
			row.Remove()
        End Sub
       
        'end 23/10/2009
        Public Shared Sub RemoveRow1(ByVal DB As Database, ByVal MemberId As Integer)
            Dim row As MemberResumeRow

            row = New MemberResumeRow(DB, MemberId)
            row.Remove1()
        End Sub

		Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
			Dim SQL As String = "select * from MemberResume"
			If Not SortBy = String.Empty Then
				SortBy = Core.ProtectParam(SortBy)
				SortOrder = Core.ProtectParam(SortOrder)

				SQL &= " order by " & SortBy & " " & SortOrder
			End If
			Return DB.GetDataTable(SQL)
		End Function
      
        'Custom Methods

	End Class

	Public MustInherit Class MemberResumeRowBase
		Private m_DB As Database
		Private m_ResumeId As Integer = Nothing
		Private m_MemberId As Integer = Nothing
		Private m_FirstName As String = Nothing
		Private m_LastName As String = Nothing
		Private m_Phone As String = Nothing
		Private m_Email As String = Nothing
		Private m_Address1 As String = Nothing
		Private m_Address2 As String = Nothing
		Private m_City As String = Nothing
		Private m_State As String = Nothing
		Private m_Zipcode As String = Nothing
		Private m_JobCategoryId As Integer = Nothing
		Private m_Relocate As Boolean = Nothing
		Private m_HourRequirement As String = Nothing
		Private m_LicensedSalonProfessional As Boolean = Nothing
		Private m_Student As Boolean = Nothing
		Private m_YearsExperience As Integer = Nothing
		Private m_Resume As String = Nothing
		Private m_CoverLetter As String = Nothing
		Private m_ProfileType As String = Nothing
		Private m_IsAgreed As Boolean = Nothing
		Private m_AgreeDate As DateTime = Nothing
		Private m_IsActive As Boolean = Nothing


		Public Property ResumeId() As Integer
			Get
				Return m_ResumeId
			End Get
			Set(ByVal Value As Integer)
				m_ResumeId = value
			End Set
		End Property

		Public Property MemberId() As Integer
			Get
				Return m_MemberId
			End Get
			Set(ByVal Value As Integer)
				m_MemberId = value
			End Set
		End Property

		Public Property FirstName() As String
			Get
				Return m_FirstName
			End Get
			Set(ByVal Value As String)
				m_FirstName = value
			End Set
		End Property

		Public Property LastName() As String
			Get
				Return m_LastName
			End Get
			Set(ByVal Value As String)
				m_LastName = value
			End Set
		End Property

		Public Property Phone() As String
			Get
				Return m_Phone
			End Get
			Set(ByVal Value As String)
				m_Phone = value
			End Set
		End Property

		Public Property Email() As String
			Get
				Return m_Email
			End Get
			Set(ByVal Value As String)
				m_Email = value
			End Set
		End Property

		Public Property City() As String
			Get
				Return m_City
			End Get
			Set(ByVal Value As String)
				m_City = value
			End Set
		End Property

		Public Property Address1() As String
			Get
				Return m_Address1
			End Get
			Set(ByVal Value As String)
				m_Address1 = Value
			End Set
		End Property

		Public Property Address2() As String
			Get
				Return m_Address2
			End Get
			Set(ByVal Value As String)
				m_Address2 = Value
			End Set
		End Property

		Public Property State() As String
			Get
				Return m_State
			End Get
			Set(ByVal Value As String)
				m_State = Value
			End Set
		End Property

		Public Property Zipcode() As String
			Get
				Return m_Zipcode
			End Get
			Set(ByVal Value As String)
				m_Zipcode = value
			End Set
		End Property

		Public Property JobCategoryId() As Integer
			Get
				Return m_JobCategoryId
			End Get
			Set(ByVal Value As Integer)
				m_JobCategoryId = value
			End Set
		End Property

		Public Property Relocate() As Boolean
			Get
				Return m_Relocate
			End Get
			Set(ByVal Value As Boolean)
				m_Relocate = value
			End Set
		End Property

		Public Property HourRequirement() As String
			Get
				Return m_HourRequirement
			End Get
			Set(ByVal Value As String)
				m_HourRequirement = value
			End Set
		End Property

		Public Property LicensedSalonProfessional() As Boolean
			Get
				Return m_LicensedSalonProfessional
			End Get
			Set(ByVal Value As Boolean)
				m_LicensedSalonProfessional = value
			End Set
		End Property

		Public Property Student() As Boolean
			Get
				Return m_Student
			End Get
			Set(ByVal Value As Boolean)
				m_Student = value
			End Set
		End Property

		Public Property YearsExperience() As Integer
			Get
				Return m_YearsExperience
			End Get
			Set(ByVal Value As Integer)
				m_YearsExperience = value
			End Set
		End Property

		Public Property [Resume]() As String
			Get
				Return m_Resume
			End Get
			Set(ByVal Value As String)
				m_Resume = value
			End Set
		End Property

		Public Property CoverLetter() As String
			Get
				Return m_CoverLetter
			End Get
			Set(ByVal Value As String)
				m_CoverLetter = value
			End Set
		End Property

		Public Property ProfileType() As String
			Get
				Return m_ProfileType
			End Get
			Set(ByVal Value As String)
				m_ProfileType = value
			End Set
		End Property

		Public Property IsAgreed() As Boolean
			Get
				Return m_IsAgreed
			End Get
			Set(ByVal Value As Boolean)
				m_IsAgreed = value
			End Set
		End Property

		Public Property AgreeDate() As DateTime
			Get
				Return m_AgreeDate
			End Get
			Set(ByVal Value As DateTime)
				m_AgreeDate = value
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

		Public Sub New(ByVal DB As Database, ByVal MemberId As Integer)
			m_DB = DB
            m_MemberId = MemberId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM MemberResume WHERE MemberId = " & DB.Number(MemberId)
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
			m_ResumeId = Convert.ToInt32(r.Item("ResumeId"))
			m_MemberId = Convert.ToInt32(r.Item("MemberId"))
			m_FirstName = Convert.ToString(r.Item("FirstName"))
			m_LastName = Convert.ToString(r.Item("LastName"))
			If IsDBNull(r.Item("Phone")) Then
				m_Phone = Nothing
			Else
				m_Phone = Convert.ToString(r.Item("Phone"))
			End If
			m_Email = Convert.ToString(r.Item("Email"))
			m_City = Convert.ToString(r.Item("City"))
			m_State = Convert.ToString(r.Item("State"))
			m_Zipcode = Convert.ToString(r.Item("Zipcode"))
			m_JobCategoryId = Convert.ToInt32(r.Item("JobCategoryId"))
			m_Relocate = Convert.ToBoolean(r.Item("Relocate"))
			m_HourRequirement = Convert.ToString(r.Item("HourRequirement"))
			m_LicensedSalonProfessional = Convert.ToBoolean(r.Item("LicensedSalonProfessional"))
			m_Student = Convert.ToBoolean(r.Item("Student"))
			m_YearsExperience = Convert.ToInt32(r.Item("YearsExperience"))
			m_Resume = Convert.ToString(r.Item("Resume"))
			If IsDBNull(r.Item("CoverLetter")) Then
				m_CoverLetter = Nothing
			Else
				m_CoverLetter = Convert.ToString(r.Item("CoverLetter"))
			End If
			If IsDBNull(r.Item("Address1")) Then
				m_Address1 = Nothing
			Else
				m_Address1 = Convert.ToString(r.Item("Address1"))
			End If
			If IsDBNull(r.Item("Address2")) Then
				m_Address2 = Nothing
			Else
				m_Address2 = Convert.ToString(r.Item("Address2"))
			End If
			m_ProfileType = Convert.ToString(r.Item("ProfileType"))
			m_IsAgreed = Convert.ToBoolean(r.Item("IsAgreed"))
			If IsDBNull(r.Item("AgreeDate")) Then
				m_AgreeDate = Nothing
			Else
				m_AgreeDate = Convert.ToDateTime(r.Item("AgreeDate"))
			End If
			m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


            SQL = " INSERT INTO MemberResume (" _
             & " MemberId" _
             & ",FirstName" _
             & ",LastName" _
             & ",Phone" _
             & ",Email" _
             & ",Address1" _
             & ",Address2" _
             & ",City" _
             & ",State" _
             & ",Zipcode" _
             & ",JobCategoryId" _
             & ",Relocate" _
             & ",HourRequirement" _
             & ",LicensedSalonProfessional" _
             & ",Student" _
             & ",YearsExperience" _
             & ",Resume" _
             & ",CoverLetter" _
             & ",ProfileType" _
             & ",IsAgreed" _
             & ",AgreeDate" _
             & ",IsActive" _
             & ") VALUES (" _
             & m_DB.NullNumber(MemberId) _
             & "," & m_DB.Quote(FirstName) _
             & "," & m_DB.Quote(LastName) _
             & "," & m_DB.Quote(Phone) _
             & "," & m_DB.Quote(Email) _
             & "," & m_DB.Quote(Address1) _
             & "," & m_DB.Quote(Address2) _
             & "," & m_DB.Quote(City) _
             & "," & m_DB.Quote(State) _
             & "," & m_DB.Quote(Zipcode) _
             & "," & m_DB.NullNumber(JobCategoryId) _
             & "," & CInt(Relocate) _
             & "," & m_DB.Quote(HourRequirement) _
             & "," & CInt(LicensedSalonProfessional) _
             & "," & CInt(Student) _
             & "," & m_DB.Number(YearsExperience) _
             & "," & m_DB.NQuote([Resume]) _
             & "," & m_DB.NQuote(CoverLetter) _
             & "," & m_DB.Quote(ProfileType) _
             & "," & CInt(IsAgreed) _
             & "," & m_DB.NullQuote(AgreeDate) _
             & "," & CInt(IsActive) _
             & ")"

			ResumeId = m_DB.InsertSQL(SQL)

			Return ResumeId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

            SQL = " UPDATE MemberResume SET " _
             & " MemberId = " & m_DB.NullNumber(MemberId) _
             & ",FirstName = " & m_DB.Quote(FirstName) _
             & ",LastName = " & m_DB.Quote(LastName) _
             & ",Phone = " & m_DB.Quote(Phone) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",Address1 = " & m_DB.Quote(Address1) _
             & ",Address2 = " & m_DB.Quote(Address2) _
             & ",City = " & m_DB.Quote(City) _
             & ",State = " & m_DB.Quote(State) _
             & ",Zipcode = " & m_DB.Quote(Zipcode) _
             & ",JobCategoryId = " & m_DB.NullNumber(JobCategoryId) _
             & ",Relocate = " & CInt(Relocate) _
             & ",HourRequirement = " & m_DB.Quote(HourRequirement) _
             & ",LicensedSalonProfessional = " & CInt(LicensedSalonProfessional) _
             & ",Student = " & CInt(Student) _
             & ",YearsExperience = " & m_DB.Number(YearsExperience) _
             & ",Resume = " & m_DB.NQuote([Resume]) _
             & ",CoverLetter = " & m_DB.NQuote(CoverLetter) _
             & ",ProfileType = " & m_DB.Quote(ProfileType) _
             & ",IsAgreed = " & CInt(IsAgreed) _
             & ",AgreeDate = " & m_DB.NullQuote(AgreeDate) _
             & ",IsActive = " & CInt(IsActive) _
             & " WHERE ResumeId = " & m_DB.Quote(ResumeId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM MemberResume WHERE ResumeId = " & m_DB.Number(ResumeId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
        Public Sub Remove1()
            Dim SQL As String

            SQL = "DELETE FROM MemberResume WHERE MemberId = " & m_DB.Number(MemberId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
        'Job Category
        Public Shared Function GetJobCategory(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from jobcategory")
            Return dt
        End Function
        'end job category
	End Class

	Public Class MemberResumeCollection
		Inherits GenericCollection(Of MemberResumeRow)
	End Class

End Namespace


