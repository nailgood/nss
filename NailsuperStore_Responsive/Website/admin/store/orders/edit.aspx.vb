Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Imports System.Collections.Generic

Partial Class admin_store_orders_edit
    Inherits AdminPage

    Protected params As String
    Public OrderId As Integer
    Protected c As ShoppingCart
    Protected o As StoreOrderRow
    Protected LiftGateService As Double = SysParam.GetValue("LiftGateService")
    Protected InsideDeliveryService As Double = SysParam.GetValue("InsideDeliveryService")


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        OrderId = CType(Request("OrderId"), Integer)
        c = New ShoppingCart(DB, OrderId, True)
        litOrderNo.Text = c.Order.OrderNo
        ''  dtl.Cart = c
        LoadExportLog()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnReExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReExport.Click
        StoreOrderRow.ProcessReExport(DB, OrderId)
        Dim ex As ExportLogRow = ExportLogRow.GetRow(DB, OrderId)
        If Not ex Is Nothing Then
            If Not String.IsNullOrEmpty(ex.HeaderFile) Then
                ex.NoteHeaderFile = ex.NoteHeaderFile & ex.HeaderFile & ", "
            End If
            If Not String.IsNullOrEmpty(ex.CartItemFile) Then
                ex.NoteCartItemFile = ex.NoteCartItemFile & ex.CartItemFile & ", "
            End If
            ex.Update()
        End If
        Dim msg As String = "OrderNo: " & c.Order.OrderNo
        msg = msg & "<br/>Username: " & LoggedInUsername
        msg = msg & "<br/>Re-Export Date: " & DateTime.Now.ToString()

        Email.SendReport("ToReportPayment", "Re-Export Order! - OrderNo=" & c.Order.OrderNo, msg)
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub LoadExportLog()
        Dim ex As ExportLogRow = ExportLogRow.GetRow(DB, OrderId)
        If Not ex Is Nothing Then
            If (Not String.IsNullOrEmpty(ex.HeaderFile) AndAlso ex.OrderStatus = 3 AndAlso Not String.IsNullOrEmpty(ex.HeaderFile) AndAlso ex.CartItemStatus = 3) Or Not String.IsNullOrEmpty(ex.NoteHeaderFile) Or Not String.IsNullOrEmpty(ex.NoteCartItemFile) Then
                lbHeaderFile.Text = ex.HeaderFile
                lbCartItemFile.Text = ex.CartItemFile
                tblExport.Visible = True
                If (Not String.IsNullOrEmpty(ex.HeaderFile) AndAlso ex.OrderStatus < 3) Then
                    lbHeaderFile.Enabled = False
                    spPendingHeader.Visible = True
                End If
                If (Not String.IsNullOrEmpty(ex.CartItemFile) AndAlso ex.CartItemStatus < 3) Then
                    lbCartItemFile.Enabled = False
                    spPendingCartItem.Visible = True
                End If
                '' xu ly cac HeaderFile va CartItemFile cu dc chua trong NoteCartItemFile va NoteHeaderFile
                If (Not String.IsNullOrEmpty(ex.NoteHeaderFile) Or Not String.IsNullOrEmpty(ex.NoteCartItemFile)) Then

                    Dim lstHeaderFile As String() = ex.NoteHeaderFile.Replace(ex.HeaderFile & ",", String.Empty).Trim().Split(",")
                    Dim lstCartItemFile As String() = ex.NoteCartItemFile.Replace(ex.CartItemFile & ",", String.Empty).Trim().Split(",")
                    Dim lstNote As New List(Of NoteOrderFile)
                    Dim bAddNote As Boolean = False
                    '' so sanh 2 list NoteHeaderFile va ListCartItemFile sau khi split de, chon list lon hon de chay vong lap va add vao ListNote chua HeaderFile va CartItemFile
                    If lstHeaderFile.Length >= lstCartItemFile.Length Then
                        For i As Integer = 0 To lstHeaderFile.Length - 1
                            bAddNote = False
                            Dim note As New NoteOrderFile
                            If (Not String.IsNullOrEmpty(lstHeaderFile(i))) Then
                                note.HeaderFile = lstHeaderFile(i)
                                bAddNote = True
                            End If
                            If i < lstCartItemFile.Length Then
                                If (Not String.IsNullOrEmpty(lstCartItemFile(i))) Then
                                    note.CartItemFile = lstCartItemFile(i)
                                    bAddNote = True
                                End If
                            End If
                            If (bAddNote) Then
                                lstNote.Add(note)
                            End If
                        Next
                    Else
                        For i As Integer = 0 To lstCartItemFile.Length - 1
                            bAddNote = False
                            Dim note As New NoteOrderFile
                            If (Not String.IsNullOrEmpty(lstCartItemFile(i))) Then
                                note.CartItemFile = lstCartItemFile(i)
                                bAddNote = True
                            End If
                            If i < lstHeaderFile.Length Then
                                If (Not String.IsNullOrEmpty(lstHeaderFile(i))) Then
                                    note.HeaderFile = lstHeaderFile(i)
                                    bAddNote = True
                                End If
                            End If
                            If (bAddNote) Then
                                lstNote.Add(note)
                            End If
                        Next
                    End If
                    '' dua lstNote vao repeater de hien thi tung row co HeaderFile va CartItemFile cho download
                    rptNoteFile.DataSource = lstNote
                    rptNoteFile.DataBind()
                    If lstNote.Count > 0 Then
                        rptNoteFile.Visible = True
                    Else
                        rptNoteFile.Visible = False
                    End If
                Else
                    rptNoteFile.Visible = False
                End If
            Else
                tblExport.Visible = False
            End If
        End If
    End Sub

    Protected Sub lbCartItemFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCartItemFile.Click
        DownloadExport(lbCartItemFile.Text)
    End Sub

    Protected Sub lbHeaderFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbHeaderFile.Click
        If Email.IsLocal Then
            DownloadExport(lbHeaderFile.Text)
        Else
            DownloadExport(lbHeaderFile.Text.Replace(".EDI", "-EDI") & ".axx")
        End If
    End Sub

    Private Sub DownloadExport(ByVal sFileName As String)
        sFileName = sFileName.Trim()
        Dim filePath As String = Utility.ConfigData.ArchivePath & sFileName
        Dim PathDes As String = Request.PhysicalApplicationPath & Utility.ConfigData.FolderCopyArchivePath
        Try
            If (File.Exists(filePath)) Then
                If (Not Directory.Exists(PathDes)) Then
                    Directory.CreateDirectory(PathDes)
                End If
                File.Copy(filePath, PathDes & sFileName, True)
                Dim context As HttpContext = HttpContext.Current
                context.Response.Buffer = True
                context.Response.Clear()
                context.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName)
                context.Response.ContentType = "application/octet-stream"
                context.Response.WriteFile("~" & Utility.ConfigData.FolderCopyArchivePath + sFileName)
                context.Response.Flush()
                context.Response.Close()
                File.Delete(PathDes & sFileName)
            Else
                ltrMsgError.Text = "<span class=""red"">" & sFileName & " can't download.</span>"
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub rptNoteFile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptNoteFile.ItemCommand
        If e.CommandName.Equals("DownloadHeader") Then
            If Email.IsLocal Then
                DownloadExport(e.CommandArgument)
            Else
                DownloadExport(e.CommandArgument.Replace(".EDI", "-EDI") & ".axx")
            End If
        ElseIf e.CommandName.Equals("DownloadCartItem") Then
            DownloadExport(e.CommandArgument)
        End If
    End Sub

    Protected Sub rptNoteFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptNoteFile.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim note As NoteOrderFile = DirectCast(e.Item.DataItem, NoteOrderFile)
            Dim lbNoteHeaderFile As LinkButton = CType(e.Item.FindControl("lbNoteHeaderFile"), LinkButton)
            Dim lbNoteCartItemFile As LinkButton = CType(e.Item.FindControl("lbNoteCartItemFile"), LinkButton)

            lbNoteHeaderFile.Text = note.HeaderFile
            lbNoteHeaderFile.CommandArgument = note.HeaderFile

            lbNoteCartItemFile.Text = note.CartItemFile
            lbNoteCartItemFile.CommandArgument = note.CartItemFile
        End If
    End Sub
End Class

Public Class NoteOrderFile
    Public HeaderFile As String
    Public CartItemFile As String
End Class