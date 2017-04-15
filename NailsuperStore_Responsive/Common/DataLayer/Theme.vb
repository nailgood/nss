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

    Public Class ThemeRow
        Inherits ThemeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ThemeId As Integer)
            MyBase.New(DB, ThemeId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ThemeId As Integer) As ThemeRow
            Dim row As ThemeRow

            row = New ThemeRow(DB, ThemeId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ThemeId As Integer)
            Dim row As ThemeRow

            row = New ThemeRow(DB, ThemeId)
            row.Remove()
        End Sub
        
        'Custom Methods
        Public Shared Function GetActiveRow(ByVal DB As Database) As ThemeRow
            'Dim ThemeId As Integer = DB.ExecuteScalar("select top 1 themeid from theme where isactive = 1")
            Dim Themeid As Integer = GetThemeIDActive()

            If ThemeId = Nothing Then ThemeId = 1
            If IsNumeric(System.Web.HttpContext.Current.Session("ThemeId")) AndAlso CInt(System.Web.HttpContext.Current.Session("ThemeId")) <> Nothing Then
                ThemeId = System.Web.HttpContext.Current.Session("ThemeId")
            End If
            Return ThemeRow.GetRow(DB, ThemeId)
        End Function

        Private Shared Function GetThemeIDActive() As Integer
            Dim themeId As Integer = 0
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_THEME_GETOBJECT As String = "sp_Theme_GetThemeIDActive"

            Dim cmd As DbCommand = DB.GetStoredProcCommand(SP_THEME_GETOBJECT)

            themeId = Convert.ToInt32(db.ExecuteScalar(cmd))

            Return themeId

            '------------------------------------------------------------------------
        End Function

        Public Function GetCss() As DataView
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_THEME_GETLIST As String = "sp_Theme_GetCSS"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_THEME_GETLIST)

            db.AddInParameter(cmd, "ThemeId", DbType.Int32, ThemeId)

            Return db.ExecuteDataSet(cmd).Tables(0).DefaultView

            '------------------------------------------------------------------------
        End Function

        Public Sub Activate()
            Dim SQL As String = "update theme set isactive = case when themeid = " & themeId & " then 1 else 0 end"
            db.ExecuteSQL(SQL)
            System.Web.HttpContext.Current.Cache.Remove("ThemeCss")
            'GenerateStylesheet()
        End Sub

        Private Sub GenerateStylesheet()
            'Create the stylesheet
            If Core.FileExists(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp")) Then System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp"))
            Dim sw As System.IO.StreamWriter = New System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp"))
            Dim dv As DataView = GetCss()
            Dim drv As DataRowView
            Dim PreviousClassName As String = String.Empty
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                If PreviousClassName <> drv("ClassName") Then
                    If i > 0 Then
                        sw.Write("}" & vbCrLf & vbCrLf)
                    End If
                    sw.Write("/* " & drv("ClassName") & " */" & vbCrLf & drv("CssClass") & " {")
                End If
                sw.Write(drv("property") & ": " & drv("value") & ";")
                PreviousClassName = drv("ClassName")
            Next
            sw.Write("}")
            sw.Close()
            sw.Dispose()
            System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp"), System.Web.HttpContext.Current.Server.MapPath("/includes/theme.css"), True)
            If Core.FileExists(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp")) Then System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("/includes/theme.tmp"))
        End Sub

    End Class

    Public MustInherit Class ThemeRowBase
        Private m_DB As Database
        Private m_ThemeId As Integer = Nothing
        Private m_Theme As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_Logo As String = Nothing
        Private m_LogoFooter As String = Nothing
        Private m_Tagline As String = Nothing


        Public Property ThemeId() As Integer
            Get
                Return m_ThemeId
            End Get
            Set(ByVal Value As Integer)
                m_ThemeId = Value
            End Set
        End Property

        Public Property Theme() As String
            Get
                Return m_Theme
            End Get
            Set(ByVal Value As String)
                m_Theme = Value
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

        Public Property Logo() As String
            Get
                Return m_Logo
            End Get
            Set(ByVal Value As String)
                m_Logo = Value
            End Set
        End Property

        Public Property LogoFooter() As String
            Get
                Return m_LogoFooter
            End Get
            Set(ByVal Value As String)
                m_LogoFooter = Value
            End Set
        End Property

        Public Property Tagline() As String
            Get
                Return m_Tagline
            End Get
            Set(ByVal Value As String)
                m_Tagline = Value
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

        Public Sub New(ByVal DB As Database, ByVal ThemeId As Integer)
            m_DB = DB
            m_ThemeId = ThemeId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:07 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_THEME_GETOBJECT As String = "sp_Theme_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_THEME_GETOBJECT)

                db.AddInParameter(cmd, "ThemeId", DbType.Int32, ThemeId)

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
            'Date: October 26, 2009 02:13:07 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ThemeId"))) Then
                        m_ThemeId = Convert.ToInt32(reader("ThemeId"))
                    Else
                        m_ThemeId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Theme"))) Then
                        m_Theme = reader("Theme").ToString()
                    Else
                        m_Theme = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Logo"))) Then
                        m_Logo = reader("Logo").ToString()
                    Else
                        m_Logo = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LogoFooter"))) Then
                        m_LogoFooter = reader("LogoFooter").ToString()
                    Else
                        m_LogoFooter = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Tagline"))) Then
                        m_Tagline = reader("Tagline").ToString()
                    Else
                        m_Tagline = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            SQL = " INSERT INTO Theme (" _
             & " Theme" _
             & ",IsActive" _
             & ",Logo" _
             & ",LogoFooter" _
             & ",Tagline" _
             & ") VALUES (" _
             & m_DB.Quote(Theme) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Quote(Logo) _
             & "," & m_DB.Quote(LogoFooter) _
             & "," & m_DB.Quote(Tagline) _
             & ")"

            ThemeId = m_DB.InsertSQL(SQL)

            Return ThemeId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Theme SET " _
             & " Theme = " & m_DB.Quote(Theme) _
             & ",IsActive = " & CInt(IsActive) _
             & ",Logo = " & m_DB.Quote(Logo) _
             & ",LogoFooter = " & m_DB.Quote(LogoFooter) _
             & ",Tagline = " & m_DB.Quote(Tagline) _
             & " WHERE ThemeId = " & m_DB.Quote(ThemeId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Theme WHERE ThemeId = " & m_DB.Quote(ThemeId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ThemeCollection
        Inherits GenericCollection(Of ThemeRow)
    End Class

End Namespace


