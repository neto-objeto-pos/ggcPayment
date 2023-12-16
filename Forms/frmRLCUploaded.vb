Imports System.IO

Public Class frmRLCUploaded
    Private Sub initGrid()
        With DataGridView1
            .RowCount = 0

            'Set No of Columns
            .ColumnCount = 2

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "FILE NAME"


            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 160

            'Set No of Rows
            .RowCount = 1
        End With
    End Sub

    Private Sub loadDetail()
        Dim dir As New DirectoryInfo(Environment.GetEnvironmentVariable("RLC-UPATH"))

        With DataGridView1
            Dim lnCtr As Integer
            .RowCount = Directory.EnumerateFiles(Environment.GetEnvironmentVariable("RLC-UPATH")).Count

            For Each sfile In dir.GetFiles()
                .Item(0, lnCtr).Value = lnCtr + 1
                .Item(1, lnCtr).Value = sfile.Name
                lnCtr = lnCtr + 1
            Next

            .ClearSelection()
        End With
    End Sub

    Private Sub frmRLCUploaded_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call initGrid()
        Call loadDetail()
    End Sub

    Private Sub cmdButton00_Click(sender As Object, e As EventArgs) Handles cmdButton00.Click
        Me.Close()
    End Sub
End Class