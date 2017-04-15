Imports Components
Imports Components.Core
Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports DataLayer
Imports System.Threading

Public Class admin_navision_index
    Inherits AdminPage

    Private m_FileNames, m_Ids, m_Conn As String
    Private m_ImportIsRunning As Boolean = False

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        m_ImportIsRunning = ImportIsRunning()

        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        Dim SQL, email As String
        Dim dv As DataView, drv As DataRowView

        If Not LoggedInIsInternal Then
            ccFU.Visible = False
            btnSubmit.Visible = False
            btnProcess.Visible = False
        End If

        If Not m_ImportIsRunning Then
            SQL = "select top 1 * from NavisionImport where BCPStart is not null and BCPDate is null and BCPFail = 0"
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            If dv.Count > 0 Then
                drv = dv(0)

                SQL = "update NavisionImport set BCPFail = 1 where FileName = " & DB.Quote(drv("FileName"))
                DB.ExecuteSQL(SQL)

                email = SysParam.GetValue("NavisionImportNotificationEmail")

                email = SysParam.GetValue("NavisionAdminEmail")
            End If
        End If

        gvList.DataSource = DB.GetDataSet("select top 100 * from NavisionImport order by importdate desc, SortOrder desc, filename desc")
        gvList.DataBind()

        If gvList.Rows.Count > 0 AndAlso Not m_ImportIsRunning Then
            btnProcess.Enabled = True
            btnProcess.Text = "Process Import Files"
        Else
            'btnProcess.Enabled = False
            If m_ImportIsRunning Then
                CType(Master.FindControl("HtmlBody"), HtmlGenericControl).Attributes.Add("onload", "loopStatus('" & m_FileNames & "','" & m_Ids & "','" & btnProcess.ClientID & "," & btnSubmit.ClientID & "','" & btnProcess.Text & "," & btnSubmit.Text & "');")
                btnProcess.Text = "Processing Files..."
            End If
        End If
    End Sub

    Private Sub btnproduct_click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProduct.Click
        Dim info As ProcessStartInfo = New ProcessStartInfo("C:\NailSuperStore\nss.com\ProductImport\bin\debug\productimport.exe")
        Dim proc As Process = Process.Start(info)
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        If Not Page.IsValid OrElse m_ImportIsRunning Then Exit Sub

        Dim dv As DataView, drv As DataRowView
        Dim SQL As String
        lit.Text = ""

        Try
            SQL = "select top 1 * from NavisionImport where FileName = " & DB.Quote(ccFU.NewFileName)
            dv = DB.GetDataSet(SQL).Tables(0).DefaultView
            If dv.Count <> 0 Then
                drv = dv(0)
                If Not IsDBNull(drv("BCPDate")) Then
                    AddError("The file specified has already been uploaded and imported.")
                    Exit Sub
                End If
            End If
            Try
                ccFU.SaveNewFile()
            Catch ex As Exception
                AddError("There was an error uploading the file.")
                Exit Sub
            End Try
            Dim path As String = SysParam.GetValue("NavisionUploadPath")
            If Core.FileExists(path & ccFU.NewFileName) Then IO.File.Delete(path & ccFU.NewFileName)
            If Core.FileExists(Server.MapPath(ccFU.Folder & ccFU.NewFileName)) Then
                IO.File.Copy(Server.MapPath(ccFU.Folder & ccFU.NewFileName), path & ccFU.NewFileName)
                IO.File.Delete(Server.MapPath(ccFU.Folder & ccFU.NewFileName))
            End If
        Catch ex As Exception
            AddError(ex.ToString)
            Exit Sub
        End Try

        If FileExists(Server.MapPath(ccFU.Folder & ccFU.NewFileName)) Then
            SQL = "if (select top 1 FileName from NavisionImport where FileName = " & DB.Quote(ccFU.NewFileName) & ") is null begin insert into NavisionImport (FileName, ImportDate) values (" & DB.Quote(ccFU.NewFileName) & "," & DB.Quote(Now()) & ") end"
            DB.ExecuteSQL(SQL)
            lit.Text = "<div style=""color:red"">File upload was successful</div>"
        End If
        LoadFromDB()
    End Sub

    Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcess.Click
        Dim info As ProcessStartInfo = New ProcessStartInfo(AppSettings("NavisionImportPath"))
        Dim proc As Process = Process.Start(info)
        System.Threading.Thread.Sleep(5000)
        Response.Redirect("/admin/Navision/")
    End Sub

    Private Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim div As HtmlGenericControl = CType(e.Row.FindControl("divStatus"), HtmlGenericControl)
            Dim div2 As HtmlGenericControl = CType(e.Row.FindControl("divDate"), HtmlGenericControl)
            Dim div3 As HtmlGenericControl = CType(e.Row.FindControl("divStart"), HtmlGenericControl)

            If Not IsDBNull(e.Row.DataItem("BCPStart")) Then div3.InnerHtml = e.Row.DataItem("BCPStart")

            If e.Row.DataItem("BCPFail") Then
                div.InnerHtml = "<span style=""color:red;font-weight:bold"">Failed!</span>"
            Else
                If Not IsDBNull(e.Row.DataItem("BCPStart")) Then
                    If IsDBNull(e.Row.DataItem("BCPDate")) Then
                        div.InnerHtml = "<span style=""font-weight:bold"">Processing <img src=""/images/indicator.gif"" align=""absmiddle"" /></span>"
                        AddFile(e.Row.DataItem("FileName"), div3.ClientID & "|" & div2.ClientID & "|" & div.ClientID)
                    Else
                        If e.Row.DataItem("RowsImported") > 0 Then
                            div.InnerHtml = "<span style=""color:green;font-weight:bold"">Imported (" & e.Row.DataItem("RowsImported") & " rows)</span>"
                        Else
                            div.InnerHtml = "Skipped"
                        End If
                        div2.InnerHtml = e.Row.DataItem("BCPDate")
                    End If
                Else
                    AddFile(e.Row.DataItem("FileName"), div3.ClientID & "|" & div2.ClientID & "|" & div.ClientID)
                End If
            End If
        End If
    End Sub

    Private Sub AddFile(ByVal FileName As String, ByVal ID As String)
        m_FileNames &= m_Conn & FileName
        m_Ids &= m_Conn & ID
        m_Conn = ","
    End Sub
End Class