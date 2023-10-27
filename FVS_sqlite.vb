Imports System.Data.SQLite

Public Class FVS_SQLite

    '- Run List Selection Variables
    Public Shared RunID(150) As Integer
    Public Shared BaseID(50) As Integer
    Public Shared RunIDName(150) As String
    Public Shared RunBasePeriodID(150) As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SQLiteConn As New SQLiteConnection
        Dim sqlite_cmd As New SQLiteCommand
        Dim drd1 As SQLiteDataReader

        SQLiteConn.ConnectionString = "Data Source=c:\fram_source\fram_test.db;Version=3;Compress=True"

        If SQLiteConn.State = ConnectionState.Closed Then
            SQLiteConn.Open()

        End If

        sqlite_cmd = SQLiteConn.CreateCommand()

        sqlite_cmd.CommandText = "SELECT * FROM RunID;"

        drd1 = sqlite_cmd.ExecuteReader


        Dim str1 As String
        Dim int1 As Integer
        int1 = 0
        ListBox1.Items.Clear()
        Do While drd1.Read
            '- Fill CheckedListBox Items
            If RecordsetSelectionType = 11 Then
                str1 = String.Format("{0,5}-", drd1.GetInt32(1).ToString("####0"))
                str1 &= String.Format("{0,-50}", drd1.GetString(2).ToString)
                'str1 &= String.Format(" {0,-25} -", Mid(drd1.GetString(3).ToString, 1, 25))
                'str1 &= String.Format("{0,-65}", Mid(drd1.GetString(4).ToString, 1, 65))
                ListBox1.Items.Add(str1)
                '- Set RunID Array Values
                BaseID(int1) = drd1.GetInt32(1)
                'RunBasePeriodID(int1) = drd1.GetInt32(5)
                'RunIDName(int1) = drd1.GetString(3)
                int1 = int1 + 1
            Else
                str1 = String.Format("{0,5}-", drd1.GetInt32(1).ToString("####0"))
                str1 &= String.Format("{0,-7}-", drd1.GetString(2).ToString)
                str1 &= String.Format(" {0,-25} -", Mid(drd1.GetString(3).ToString, 1, 25))
                str1 &= String.Format("{0,-65}", Mid(drd1.GetString(4).ToString, 1, 65))
                ListBox1.Items.Add(str1)
                '- Set RunID Array Values
                RunID(int1) = drd1.GetInt32(1)
                RunBasePeriodID(int1) = drd1.GetInt32(5)
                RunIDName(int1) = drd1.GetString(3)
                int1 = int1 + 1
            End If
        Loop
        SQLiteConn.Close()

    End Sub
End Class