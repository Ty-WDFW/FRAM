Imports System.Data.SQLite

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SQLiteConn As New SQLiteConnection

        SQLiteConn.ConnectionString = "Data Source=MyPath\MysqliteFile.db; Integrated Security=true"

        SQLiteConn.Open()

    End Sub
End Class